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
    public partial interface IImmunizationHistoryManager : IManagerBase<ImmunizationHistory, long>
    {

        ImmunizationHistoryDTO GetFromImmunizationHistory(ulong ulHumanID, int iPageNumber, int iMaxResult, ulong ulEncounterID, bool isLoad);
        ImmunizationHistoryDTO InsertIntoImmunizationHistory(IList<ImmunizationHistory> objImmuHistory, IList<ImmunizationHistory> objImmuHistoryInsert, ulong ulHumanID, int iPageNumber, int iMaxResult, string sMacAddress, ulong ulEncounterID, bool isLoad);
        ImmunizationHistoryDTO InsertIntoImmunizationHistoryFromOrders(IList<ImmunizationHistory> objImmuHistory, IList<ImmunizationHistory> objImmuHistoryInsert, ulong ulHumanID, int iPageNumber, int iMaxResult, string sMacAddress, ulong ulEncounterID, bool isLoad,ISession MySession, ref GenerateXml XMLObj);
        ImmunizationHistoryDTO DeleteImmunizationHistoryDetails(IList<ImmunizationHistory> ImmHis, IList<ImmunizationHistory> objImmuHistorySave, IList<ImmunizationHistory> DeleteList, string sMacAddress);
        ImmunizationHistoryDTO UpdateImmunizationHistoryDetails(IList<ImmunizationHistory> objImmuHistory, IList<ImmunizationHistory> objImmuHistorySave, IList<ImmunizationHistory> objImmuHistoryUpdate, ulong ulHumanID, int iPageNumber, int iMaxResult, string sMacAddress, ulong ulEncounterID, bool isLoad);
        int BatchOperationsToImmunizationHistory(IList<ImmunizationHistory> savelist, IList<ImmunizationHistory> updtList, IList<ImmunizationHistory> delList, ISession MySession, string MACAddress, GenerateXml XMLObj);
        //ImmunizationHistoryDTO GetLoadPhyProcedAndVaccAndPhyCodeLib(ulong PhysicianID, string procedureType, ulong LabID);
    }
    public partial class ImmunizationHistoryManager : ManagerBase<ImmunizationHistory, long>, IImmunizationHistoryManager
    {
        #region Constructors

        public ImmunizationHistoryManager()
            : base()
        {

        }
        public ImmunizationHistoryManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods
        public ImmunizationHistoryDTO GetFromImmunizationHistory(ulong ulHumanID, int PageNumber, int MaxResultSet, ulong ulEncounterID, bool Is_Load)
        {
            bool IsPrevious = false;
            ICriteria criteria = null;
            IList<ImmunizationHistory> CommonImmunizationList = new List<ImmunizationHistory>();
            ImmunizationHistoryDTO objImmunizationDTO = new ImmunizationHistoryDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (!Is_Load)
                {
                    criteria = iMySession.CreateCriteria(typeof(ImmunizationHistory)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Encounter_ID", ulEncounterID));
                    CommonImmunizationList = criteria.List<ImmunizationHistory>();
                }
                else if (Is_Load)
                {
                    criteria = iMySession.CreateCriteria(typeof(ImmunizationHistory)).Add(Expression.Eq("Human_ID", ulHumanID));//.Add(Expression.Eq("Encounter_Id", EncounterId));
                    CommonImmunizationList = criteria.List<ImmunizationHistory>();
                    EncounterManager encMngr = new EncounterManager();
                    IList<Encounter> EncLst = new List<Encounter>();
                    EncLst = encMngr.GetEncounterUsingHumanID(ulHumanID);
                    if (EncLst.Count > 0)
                    {
                        foreach (Encounter item in EncLst)
                        {
                            if (ulEncounterID >= item.Id)
                            {
                                if (CommonImmunizationList.Any(a => a.Encounter_ID == item.Id))
                                {
                                    CommonImmunizationList = CommonImmunizationList.Where(a => a.Encounter_ID == item.Id).ToList<ImmunizationHistory>();
                                    IsPrevious = true;
                                    break;

                                }
                            }
                        }
                    }

                    if (!IsPrevious && Is_Load)
                        CommonImmunizationList = CommonImmunizationList.Where(a => a.Human_ID == ulHumanID && a.Encounter_ID == Convert.ToUInt16(0)).ToList<ImmunizationHistory>();

                }
                objImmunizationDTO.ImmunizationCount = CommonImmunizationList.Count;
                if (CommonImmunizationList != null && CommonImmunizationList.Count > 0)
                    CommonImmunizationList = CommonImmunizationList.Skip((PageNumber - 1) * MaxResultSet).Take(MaxResultSet).ToList(); //GetByCriteria(MaxResultSet, PageNumber, criteria);
                objImmunizationDTO.Immunization = CommonImmunizationList;
                iMySession.Close();
            }
            return objImmunizationDTO;

        }
        //not in use
        //public void SaveUpdateDeleteImmunizationHistory(IList<ImmunizationHistory> SaveImmunizationHistory, IList<ImmunizationHistory> UpdateImmunizationHistory, IList<ImmunizationHistory> DeleteImmunizationHistory, string macAddress, ulong HumanId)
        //{
        //    GenerateXml XMLObj = new GenerateXml();
        //    int iTryCount = 0;
        //TryAgain:
        //    int iResult = 0;
        //    ISession MySession = Session.GetISession();
        //    try
        //    {
        //        using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //        {
        //            try
        //            {
        //                bool IsImmunizationHistory = true;
        //                if ((SaveImmunizationHistory != null && SaveImmunizationHistory.Count > 0) || (UpdateImmunizationHistory != null && UpdateImmunizationHistory.Count > 0) || (DeleteImmunizationHistory != null && DeleteImmunizationHistory.Count > 0))
        //                {
        //                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveImmunizationHistory, ref UpdateImmunizationHistory, DeleteImmunizationHistory, MySession, macAddress, true, true, HumanId, string.Empty, ref XMLObj);
        //                    if (iResult == 2)
        //                    {
        //                        if (iTryCount < 5)
        //                        {
        //                            iTryCount++;
        //                            goto TryAgain;
        //                        }
        //                        else
        //                        {
        //                            trans.Rollback();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                    IsImmunizationHistory = XMLObj.CheckDataConsistency(SaveImmunizationHistory.Concat(UpdateImmunizationHistory).Cast<object>().ToList(), true,string.Empty);
        //                }

        //                if (IsImmunizationHistory)
        //                {
        //                    trans.Commit();
        //                    XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
        //                }
        //                else
        //                {
        //                    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
        //                }
        //            }
        //            catch (NHibernate.Exceptions.GenericADOException ex)
        //            {
        //                trans.Rollback();
        //                throw new Exception(ex.Message);
        //            }
        //            catch (Exception e)
        //            {
        //                trans.Rollback();
        //                throw new Exception(e.Message);
        //            }
        //            finally
        //            {
        //                MySession.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        //InsertIntoImmunizationHistory Method Declared But Never Used.
        public ImmunizationHistoryDTO InsertIntoImmunizationHistory(IList<ImmunizationHistory> objImmuHistory, IList<ImmunizationHistory> InsertList, ulong ulHumanID, int iPageNumber, int iMaxResult, string sMacAddress, ulong ulEncounterID, bool isLoad)
        {
            IList<ImmunizationMasterHistory> _insertMasterList = new List<ImmunizationMasterHistory>();
            IList<ImmunizationMasterHistory> _updateMasterList = new List<ImmunizationMasterHistory>();


            if (InsertList.Count > 0)
                for (int i = 0; i < InsertList.Count; i++)
                {
                    ImmunizationMasterHistory objMaster = new ImmunizationMasterHistory();
                    // //bugId:61103 
                    if (InsertList[i].Immunization_History_Master_ID != 0)
                    {
                        //ICriteria crit = Session.GetISession().CreateCriteria(typeof(ImmunizationMasterHistory)).Add(Expression.Eq("Id", InsertList[i].Immunization_History_Master_ID));
                       // objMaster = crit.List<ImmunizationMasterHistory>()[0];
                        objMaster.Administered_Amount = InsertList[i].Administered_Amount;
                        objMaster.Administered_Date = InsertList[i].Administered_Date;
                        objMaster.Administered_Unit = InsertList[i].Administered_Unit;
                        objMaster.CVX_Code = InsertList[i].CVX_Code;
                        objMaster.Date_On_Vis = InsertList[i].Date_On_Vis;
                        objMaster.Dose = InsertList[i].Dose;
                        objMaster.Dose_No = InsertList[i].Dose_No;
                        objMaster.Expiry_Date = InsertList[i].Expiry_Date;
                        objMaster.Human_ID = InsertList[i].Human_ID;
                        objMaster.Immunization_Description = InsertList[i].Immunization_Description;
                        objMaster.Immunization_Order_ID = InsertList[i].Immunization_Order_ID;
                        objMaster.Immunization_Source = InsertList[i].Immunization_Source;
                        objMaster.Is_Deleted = InsertList[i].Is_Deleted;
                        objMaster.Is_VIS_Given = InsertList[i].Is_VIS_Given;
                        objMaster.Location = InsertList[i].Location;
                        objMaster.Lot_Number = InsertList[i].Lot_Number;
                        objMaster.Manufacturer = InsertList[i].Manufacturer;
                        objMaster.Notes = InsertList[i].Notes;
                        objMaster.Physician_ID = InsertList[i].Physician_ID;
                        objMaster.Procedure_Code = InsertList[i].Procedure_Code;
                        objMaster.Protection_State = InsertList[i].Protection_State;
                        objMaster.Route_Of_Administration = InsertList[i].Route_Of_Administration;
                        objMaster.Snomed_Code = InsertList[i].Snomed_Code;
                        objMaster.Vis_Given_Date = InsertList[i].Vis_Given_Date;
                        objMaster.Modified_By = InsertList[i].Modified_By;
                        objMaster.Modified_Date_And_Time = InsertList[i].Modified_Date_And_Time;
                        objMaster.Id = InsertList[i].Immunization_History_Master_ID;
                    //    objMaster.Version = InsertList[i].Version;
                        _updateMasterList.Add(objMaster);
                    }
                    //else
                    if (InsertList[i].Immunization_History_Master_ID == 0)
                    {
                        objMaster.Administered_Amount = InsertList[i].Administered_Amount;
                        objMaster.Administered_Date = InsertList[i].Administered_Date;
                        objMaster.Administered_Unit = InsertList[i].Administered_Unit;
                        objMaster.CVX_Code = InsertList[i].CVX_Code;
                        objMaster.Date_On_Vis = InsertList[i].Date_On_Vis;
                        objMaster.Dose = InsertList[i].Dose;
                        objMaster.Dose_No = InsertList[i].Dose_No;
                        objMaster.Expiry_Date = InsertList[i].Expiry_Date;
                        objMaster.Human_ID = InsertList[i].Human_ID;
                        objMaster.Immunization_Description = InsertList[i].Immunization_Description;
                        objMaster.Immunization_Order_ID = InsertList[i].Immunization_Order_ID;
                        objMaster.Immunization_Source = InsertList[i].Immunization_Source;
                        objMaster.Is_Deleted = InsertList[i].Is_Deleted;
                        objMaster.Is_VIS_Given = InsertList[i].Is_VIS_Given;
                        objMaster.Location = InsertList[i].Location;
                        objMaster.Lot_Number = InsertList[i].Lot_Number;
                        objMaster.Manufacturer = InsertList[i].Manufacturer;
                        objMaster.Notes = InsertList[i].Notes;
                        objMaster.Physician_ID = InsertList[i].Physician_ID;
                        objMaster.Procedure_Code = InsertList[i].Procedure_Code;
                        objMaster.Protection_State = InsertList[i].Protection_State;
                        objMaster.Route_Of_Administration = InsertList[i].Route_Of_Administration;
                        objMaster.Snomed_Code = InsertList[i].Snomed_Code;
                        objMaster.Vis_Given_Date = InsertList[i].Vis_Given_Date;
                        objMaster.Version = InsertList[i].Version;
                        objMaster.Created_By = InsertList[i].Created_By;
                        objMaster.Created_Date_And_Time = InsertList[i].Created_Date_And_Time;
                        _insertMasterList.Add(objMaster);
                    }

                }

            if (_insertMasterList.Count > 0)
            {
                ImmunizationMasterHistoryManager masterManager = new ImmunizationMasterHistoryManager();
                masterManager.InsertIntoImmunizationHistory(null, _insertMasterList, ulHumanID, 0, 0, string.Empty, ulEncounterID, true);
            }
            //bugId:61103 
            //else if (_updateMasterList.Count > 0)
            //{
            //    ImmunizationMasterHistoryManager masterManager = new ImmunizationMasterHistoryManager();
            //    masterManager.UpdateImmunizationHistoryDetails(null, null, _updateMasterList, ulHumanID, 0, 0, string.Empty, 0, true);
            //}
            //bugId:61103 
            //for (int i = 0; i < _insertMasterList.Count; i++)
            //{
            //    for (int j = 0; j < InsertList.Count; j++)
            //    {
            //        if (InsertList[j].Immunization_History_Master_ID == 0)
            //        {
            //            InsertList[j].Immunization_History_Master_ID = _insertMasterList[i].Id;
            //            //bugId:61103 
            //           // break;
            //        }
            //    }
            //}

           // bugId:61103 
            for (int i = 0; i < _insertMasterList.Count; i++)
            {
                for (int j = 0; j < InsertList.Count; j++)
                {
                    if (InsertList[j].Immunization_History_Master_ID == 0 && InsertList[j].Procedure_Code == _insertMasterList[i].Procedure_Code)
                    {
                        InsertList[j].Immunization_History_Master_ID = _insertMasterList[i].Id;
                        
                        break;
                    }
                }
            }

            IList<ImmunizationHistory> UpdateImmunizationHistory = null;
            if (InsertList.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithTransaction(ref InsertList, ref UpdateImmunizationHistory, null, sMacAddress,true, true, InsertList[0].Human_ID, string.Empty);
            }
            return null;
        }

        //DeleteImmunizationHistoryDetails Method Declared But Never Used.
        public ImmunizationHistoryDTO DeleteImmunizationHistoryDetails(IList<ImmunizationHistory> ImmHis, IList<ImmunizationHistory> objImmuHistorySave, IList<ImmunizationHistory> DeleteList, string sMacAddress)//, ulong ulHumanID, int iPageNumber, int iMaxResult, string sMacAddress, ulong ulEncounterID, bool isLoad)
        {
            IList<ImmunizationMasterHistory> _updateMasterList = new List<ImmunizationMasterHistory>();
            if (DeleteList.Count > 0)
                for (int i = 0; i < DeleteList.Count; i++)
                {
                    ImmunizationMasterHistory objMaster = new ImmunizationMasterHistory();
                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(ImmunizationMasterHistory)).Add(Expression.Eq("Id", DeleteList[i].Immunization_History_Master_ID));
                    if (crit.List().Count != 0)
                    {
                        objMaster = crit.List<ImmunizationMasterHistory>()[0];
                        objMaster.Administered_Amount = DeleteList[i].Administered_Amount;
                        objMaster.Administered_Date = DeleteList[i].Administered_Date;
                        objMaster.Administered_Unit = DeleteList[i].Administered_Unit;
                        objMaster.CVX_Code = DeleteList[i].CVX_Code;
                        objMaster.Date_On_Vis = DeleteList[i].Date_On_Vis;
                        objMaster.Dose = DeleteList[i].Dose;
                        objMaster.Dose_No = DeleteList[i].Dose_No;
                        objMaster.Expiry_Date = DeleteList[i].Expiry_Date;
                        objMaster.Human_ID = DeleteList[i].Human_ID;
                        objMaster.Immunization_Description = DeleteList[i].Immunization_Description;
                        objMaster.Immunization_Order_ID = DeleteList[i].Immunization_Order_ID;
                        objMaster.Immunization_Source = DeleteList[i].Immunization_Source;
                        objMaster.Is_Deleted = DeleteList[i].Is_Deleted;
                        objMaster.Is_VIS_Given = DeleteList[i].Is_VIS_Given;
                        objMaster.Location = DeleteList[i].Location;
                        objMaster.Lot_Number = DeleteList[i].Lot_Number;
                        objMaster.Manufacturer = DeleteList[i].Manufacturer;
                        objMaster.Notes = DeleteList[i].Notes;
                        objMaster.Physician_ID = DeleteList[i].Physician_ID;
                        objMaster.Procedure_Code = DeleteList[i].Procedure_Code;
                        objMaster.Protection_State = DeleteList[i].Protection_State;
                        objMaster.Route_Of_Administration = DeleteList[i].Route_Of_Administration;
                        objMaster.Snomed_Code = DeleteList[i].Snomed_Code;
                        objMaster.Vis_Given_Date = DeleteList[i].Vis_Given_Date;
                        objMaster.Modified_By = DeleteList[i].Modified_By;
                        objMaster.Modified_Date_And_Time = DeleteList[i].Modified_Date_And_Time;
                        objMaster.Version = DeleteList[i].Version;
                        _updateMasterList.Add(objMaster);
                    }
                }

            if (_updateMasterList.Count > 0)
            {
                ImmunizationMasterHistoryManager masterManager = new ImmunizationMasterHistoryManager();
                masterManager.UpdateImmunizationHistoryDetails(null, null, _updateMasterList, DeleteList[0].Human_ID, 0, 0, string.Empty, 0, true);
            }
            if (DeleteList.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithTransaction(ref objImmuHistorySave, ref DeleteList, null, sMacAddress, true, true, DeleteList[0].Human_ID, string.Empty);

            }
            return null;

        }
        //UpdateImmunizationHistoryDetails Method Declared But Never Used.
        public ImmunizationHistoryDTO UpdateImmunizationHistoryDetails(IList<ImmunizationHistory> objImmuHistory, IList<ImmunizationHistory> objImmuHistorySave, IList<ImmunizationHistory> UpdateList, ulong ulHumanID, int iPageNumber, int iMaxResult, string sMacAddress, ulong ulEncounterID, bool isLoad)
        {
            IList<ImmunizationMasterHistory> _updateMasterList = new List<ImmunizationMasterHistory>();

            if (UpdateList.Count > 0)
                for (int i = 0; i < UpdateList.Count; i++)
                {
                    ImmunizationMasterHistory objMaster = new ImmunizationMasterHistory();
                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(ImmunizationMasterHistory)).Add(Expression.Eq("Id", UpdateList[i].Immunization_History_Master_ID));
                    if (crit.List().Count != 0)
                    {
                        objMaster = crit.List<ImmunizationMasterHistory>()[0];
                        objMaster.Administered_Amount = UpdateList[i].Administered_Amount;
                        objMaster.Administered_Date = UpdateList[i].Administered_Date;
                        objMaster.Administered_Unit = UpdateList[i].Administered_Unit;
                        objMaster.CVX_Code = UpdateList[i].CVX_Code;
                        objMaster.Date_On_Vis = UpdateList[i].Date_On_Vis;
                        objMaster.Dose = UpdateList[i].Dose;
                        objMaster.Dose_No = UpdateList[i].Dose_No;
                        objMaster.Expiry_Date = UpdateList[i].Expiry_Date;
                        objMaster.Human_ID = UpdateList[i].Human_ID;
                        objMaster.Immunization_Description = UpdateList[i].Immunization_Description;
                        objMaster.Immunization_Order_ID = UpdateList[i].Immunization_Order_ID;
                        objMaster.Immunization_Source = UpdateList[i].Immunization_Source;
                        objMaster.Is_Deleted = UpdateList[i].Is_Deleted;
                        objMaster.Is_VIS_Given = UpdateList[i].Is_VIS_Given;
                        objMaster.Location = UpdateList[i].Location;
                        objMaster.Lot_Number = UpdateList[i].Lot_Number;
                        objMaster.Manufacturer = UpdateList[i].Manufacturer;
                        objMaster.Notes = UpdateList[i].Notes;
                        objMaster.Physician_ID = UpdateList[i].Physician_ID;
                        objMaster.Procedure_Code = UpdateList[i].Procedure_Code;
                        objMaster.Protection_State = UpdateList[i].Protection_State;
                        objMaster.Route_Of_Administration = UpdateList[i].Route_Of_Administration;
                        objMaster.Snomed_Code = UpdateList[i].Snomed_Code;
                        objMaster.Vis_Given_Date = UpdateList[i].Vis_Given_Date;
                        objMaster.Modified_By = UpdateList[i].Modified_By;
                        objMaster.Modified_Date_And_Time = UpdateList[i].Modified_Date_And_Time;
                        objMaster.Version = UpdateList[i].Version;
                        _updateMasterList.Add(objMaster);
                    }
                }

            if (_updateMasterList.Count > 0)
            {
                ImmunizationMasterHistoryManager masterManager = new ImmunizationMasterHistoryManager();
                masterManager.UpdateImmunizationHistoryDetails(null, null, _updateMasterList, ulHumanID, 0, 0, string.Empty, 0, true);
            }
            if (UpdateList.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithTransaction(ref objImmuHistorySave, ref UpdateList, null, sMacAddress, true, true, UpdateList[0].Human_ID, string.Empty);
            }
            return GetFromImmunizationHistory(ulHumanID, iPageNumber, iMaxResult, ulEncounterID, isLoad);
        }
        //Jira #CAP-187 - update in immunization_history
        public ImmunizationHistoryDTO UpdateImmunizationHistoryAndMasterDetails(IList<ImmunizationMasterHistory> objImmuHistoryMaster, IList<ImmunizationHistory> objImmuHistorySave, IList<ImmunizationHistory> UpdateList, ulong ulHumanID, int iPageNumber, int iMaxResult, string sMacAddress, ulong ulEncounterID, bool isLoad)
        {
            if (objImmuHistoryMaster!= null &&  objImmuHistoryMaster.Count > 0)
            {
                ImmunizationMasterHistoryManager masterManager = new ImmunizationMasterHistoryManager();
                masterManager.UpdateImmunizationHistoryDetails(null, null, objImmuHistoryMaster, ulHumanID, 0, 0, string.Empty, 0, true);
            }
            if ((UpdateList!=null && objImmuHistorySave != null) &&  (UpdateList.Count > 0 ||objImmuHistorySave.Count > 0))
            {
                ulong ulHumanid = 0;
                if (UpdateList.Count > 0)
                {
                    ulHumanid = UpdateList[0].Human_ID;
                }
                else if (objImmuHistorySave.Count > 0)
                {
                    ulHumanid = objImmuHistorySave[0].Human_ID;
                }
                if(ulHumanid>0)
                    SaveUpdateDelete_DBAndXML_WithTransaction(ref objImmuHistorySave, ref UpdateList, null, sMacAddress, true, true, ulHumanid, string.Empty);
            }
            return GetFromImmunizationHistory(ulHumanID, iPageNumber, iMaxResult, ulEncounterID, isLoad);
        }
        public ImmunizationHistoryDTO GetLoadPhyProcedAndVaccAndPhyCodeLib(ulong PhysicianID, string procedureType, ulong LabID,string sLegalOrg)
        {
            bool IsPrevious = false;
            ICriteria criteria = null;
            IList<PhysicianProcedure> PhysicianProcedureList = new List<PhysicianProcedure>();
            IList<VaccineManufacturerCodes> listVaccine = new List<VaccineManufacturerCodes>();
            ImmunizationHistoryDTO objImmunizationDTO = new ImmunizationHistoryDTO();
            IList<ProcedureCodeLibrary> listPrb = new List<ProcedureCodeLibrary>();
            IList<ProcedureCodeLibrary> listPrb1 = new List<ProcedureCodeLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (LabID != 0)
                    criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Lab_ID", LabID)).Add(Expression.Eq("Legal_Org",sLegalOrg)).AddOrder(Order.Asc("Sort_Order"));
                else
                    criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Procedure_Type", procedureType)).Add(Expression.Eq("Lab_ID", LabID)).Add(Expression.Eq("Legal_Org", sLegalOrg)).AddOrder(Order.Asc("Sort_Order"));
                PhysicianProcedureList = criteria.List<PhysicianProcedure>();
                PhysicianProcedureList = (PhysicianProcedureList).Any(a => a.Sort_Order == 0) ? (PhysicianProcedureList).Where(a => a.Sort_Order != 0).ToList().Concat(PhysicianProcedureList.Where(a => a.Sort_Order == 0).OrderBy(b => b.Procedure_Description)).ToList() : PhysicianProcedureList;
                objImmunizationDTO.PhysicianProcedure = PhysicianProcedureList;
                if (PhysicianProcedureList != null && PhysicianProcedureList.Count > 0)
                {
                    criteria = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.In("Procedure_Code", PhysicianProcedureList.Select(a => a.Physician_Procedure_Code).ToList()));
                    if (criteria.List<ProcedureCodeLibrary>().Count > 0)
                    {
                        IList<ProcedureCodeLibrary> TempList = criteria.List<ProcedureCodeLibrary>();
                        foreach (ProcedureCodeLibrary obj in TempList)
                            listPrb.Add(obj);
                    }
                    if (listPrb != null && listPrb.Count > 0)
                    {
                        objImmunizationDTO.ProcedureCodeLibrary = listPrb;
                    }
                }
                criteria = iMySession.CreateCriteria(typeof(VaccineManufacturerCodes)).Add(Expression.Eq("Status", "Active")).AddOrder(Order.Asc("Sort_Order"));
                listVaccine = criteria.List<VaccineManufacturerCodes>();
                if (listVaccine != null && listVaccine.Count > 0)
                {
                    objImmunizationDTO.VaccineManufacturerCodes = listVaccine;
                }
                iMySession.Close();
            }
            return objImmunizationDTO;
        }


        public ImmunizationHistoryDTO InsertIntoImmunizationHistoryFromOrders(IList<ImmunizationHistory> objImmuHistory, IList<ImmunizationHistory> InsertList, ulong ulHumanID, int iPageNumber, int iMaxResult, string sMacAddress, ulong ulEncounterID, bool isLoad, ISession MySession, ref GenerateXml XMLObj)
        {
            IList<ImmunizationMasterHistory> _insertMasterList = new List<ImmunizationMasterHistory>();
            IList<ImmunizationMasterHistory> _updateMasterList = new List<ImmunizationMasterHistory>();

            IList<ImmunizationMasterHistory> exisitngList = new List<ImmunizationMasterHistory>();



            for (int i = 0; i < InsertList.Count; i++)
            {
                ImmunizationMasterHistory objMaster = new ImmunizationMasterHistory();

                if (InsertList[i].Immunization_History_Master_ID != 0)
                {
                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(ImmunizationMasterHistory)).Add(Expression.Eq("Id", InsertList[i].Immunization_History_Master_ID));
                    exisitngList = crit.List<ImmunizationMasterHistory>();

                    if (exisitngList.Count > 0)
                    {
                        objMaster = exisitngList[0];


                        objMaster.Administered_Amount = InsertList[i].Administered_Amount;
                        objMaster.Administered_Date = InsertList[i].Administered_Date;
                        objMaster.Administered_Unit = InsertList[i].Administered_Unit;
                        objMaster.CVX_Code = InsertList[i].CVX_Code;
                        objMaster.Date_On_Vis = InsertList[i].Date_On_Vis;
                        objMaster.Dose = InsertList[i].Dose;
                        objMaster.Dose_No = InsertList[i].Dose_No;
                        objMaster.Expiry_Date = InsertList[i].Expiry_Date;
                        objMaster.Human_ID = InsertList[i].Human_ID;
                        objMaster.Immunization_Description = InsertList[i].Immunization_Description;
                        objMaster.Immunization_Order_ID = InsertList[i].Immunization_Order_ID;
                        objMaster.Immunization_Source = InsertList[i].Immunization_Source;
                        objMaster.Is_Deleted = InsertList[i].Is_Deleted;
                        objMaster.Is_VIS_Given = InsertList[i].Is_VIS_Given;
                        objMaster.Location = InsertList[i].Location;
                        objMaster.Lot_Number = InsertList[i].Lot_Number;
                        objMaster.Manufacturer = InsertList[i].Manufacturer;
                        objMaster.Notes = InsertList[i].Notes;
                        objMaster.Physician_ID = InsertList[i].Physician_ID;
                        objMaster.Procedure_Code = InsertList[i].Procedure_Code;
                        objMaster.Protection_State = InsertList[i].Protection_State;
                        objMaster.Route_Of_Administration = InsertList[i].Route_Of_Administration;
                        objMaster.Snomed_Code = InsertList[i].Snomed_Code;
                        objMaster.Vis_Given_Date = InsertList[i].Vis_Given_Date;
                        objMaster.Modified_By = InsertList[i].Modified_By;
                        objMaster.Modified_Date_And_Time = InsertList[i].Modified_Date_And_Time;
                        objMaster.Version = InsertList[i].Version;
                        _updateMasterList.Add(objMaster);
                    }
                    else
                    {
                        objMaster.Administered_Amount = InsertList[i].Administered_Amount;
                        objMaster.Administered_Date = InsertList[i].Administered_Date;
                        objMaster.Administered_Unit = InsertList[i].Administered_Unit;
                        objMaster.CVX_Code = InsertList[i].CVX_Code;
                        objMaster.Date_On_Vis = InsertList[i].Date_On_Vis;
                        objMaster.Dose = InsertList[i].Dose;
                        objMaster.Dose_No = InsertList[i].Dose_No;
                        objMaster.Expiry_Date = InsertList[i].Expiry_Date;
                        objMaster.Human_ID = InsertList[i].Human_ID;
                        objMaster.Immunization_Description = InsertList[i].Immunization_Description;
                        objMaster.Immunization_Order_ID = InsertList[i].Immunization_Order_ID;
                        objMaster.Immunization_Source = InsertList[i].Immunization_Source;
                        objMaster.Is_Deleted = InsertList[i].Is_Deleted;
                        objMaster.Is_VIS_Given = InsertList[i].Is_VIS_Given;
                        objMaster.Location = InsertList[i].Location;
                        objMaster.Lot_Number = InsertList[i].Lot_Number;
                        objMaster.Manufacturer = InsertList[i].Manufacturer;
                        objMaster.Notes = InsertList[i].Notes;
                        objMaster.Physician_ID = InsertList[i].Physician_ID;
                        objMaster.Procedure_Code = InsertList[i].Procedure_Code;
                        objMaster.Protection_State = InsertList[i].Protection_State;
                        objMaster.Route_Of_Administration = InsertList[i].Route_Of_Administration;
                        objMaster.Snomed_Code = InsertList[i].Snomed_Code;
                        objMaster.Vis_Given_Date = InsertList[i].Vis_Given_Date;
                        objMaster.Version = InsertList[i].Version;
                        objMaster.Created_By = InsertList[i].Created_By;
                        objMaster.Created_Date_And_Time = InsertList[i].Created_Date_And_Time;
                        _insertMasterList.Add(objMaster);
                    }
                }
            }

            if (_insertMasterList.Count > 0)
            {
                ImmunizationMasterHistoryManager masterManager = new ImmunizationMasterHistoryManager();
                masterManager.InsertIntoImmunizationHistoryFromOrders(null, _insertMasterList, ulHumanID, 0, 0, string.Empty, ulEncounterID, true, MySession, ref XMLObj);
            }
            //else if (_updateMasterList.Count > 0)
            if (_updateMasterList.Count > 0)
            {
                ImmunizationMasterHistoryManager masterManager = new ImmunizationMasterHistoryManager();
                masterManager.UpdateImmunizationHistoryDetailsFromOrders(null, null, _updateMasterList, ulHumanID, 0, 0, string.Empty, 0, true, MySession, ref XMLObj);
            }

            for (int i = 0; i < _insertMasterList.Count; i++)
            {
                for (int j = 0; j < InsertList.Count; j++)
                {
                    if (InsertList[j].Immunization_History_Master_ID == 0)
                    {
                        InsertList[j].Immunization_History_Master_ID = _insertMasterList[i].Id;
                        break;
                    }
                }
            }
            IList<ImmunizationHistory> UpdateImmunizationHistory = null;
            if (InsertList.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithoutTransaction(ref InsertList, ref UpdateImmunizationHistory, null, MySession, sMacAddress, true, true, ulHumanID, string.Empty, ref XMLObj);
            }
            return null;
        }


        #endregion
        #region IImmunizationHistoryManager Members
        public int BatchOperationsToImmunizationHistory(IList<ImmunizationHistory> savelist, IList<ImmunizationHistory> updtList, IList<ImmunizationHistory> delList, ISession MySession, string MACAddress, GenerateXml XMLObj)
        {
            ulong HumanId = savelist.Count > 0 ? savelist[0].Human_ID : updtList.Count > 0 ? updtList[0].Human_ID : delList.Count > 0 ? delList[0].Human_ID : 0;
            return SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savelist, ref updtList, delList, MySession, MACAddress, true, true, HumanId, string.Empty, ref XMLObj);
        }
        #endregion
    }

}
