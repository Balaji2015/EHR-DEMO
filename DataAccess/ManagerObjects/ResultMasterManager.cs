using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IResultMasterManager : IManagerBase<ResultMaster, ulong>
    {
        IList<ResultMaster> GetResultReviewNotesBasedOnOrderSubmitId(ulong Order_ID);
        void SaveAndUpdateResultReview(ResultMaster ResultReviewObj, string sMacAddress);
        bool ImprotLabResultsForCapellaOrders(IList<ResultMaster> resultMasterList, IList<ResultORC> resultORCList, IList<ResultOBR> resultOBRList, IList<ResultOBX> resultOBXList, IList<ResultNTE> resultNTEList, IList<ResultZEF> resultZEFList, IList<ResultZPS> resultZPSList, string macAddress);
        void ImprotLabResultsForOutsideOrders(IList<ResultMaster> resultMasterList, IList<ResultORC> resultORCList, IList<ResultOBR> resultOBRList, IList<ResultOBX> resultOBXList, IList<ResultNTE> resultNTEList, IList<ResultZEF> resultZEFList, IList<ResultZPS> resultZPSList, string macAddress);
        FillResultDTO GetResultBySubmitID(ulong Order_ID);
        Stream GetResultByResultMasterID(ulong result_master_id);
        FillResultDTO UpdateResultMasterWithOrderSubmitID(ResultMaster ResultMasterToUpdate, string macAddress);
        IList<ResultMaster> GetHumanID(ulong ulHumanID);
        FillResultMasterAndEntry GetResultMasterIdUsingHumanId(ulong human_id);
        IList<FillResultMasterAndEntry> GetResultMasterAndEntryFromEncounterID(ulong encounter_ID);
        IList<ResultMaster> GetResultByHumanID(ulong HumanID);
        //ResultsReviewDTO GetResultThroughScan(ulong Order_ID);
        IList<ResultOBX> GetResultCodes(string sResultCode, string sResultDescription, string sLoincCode);
        IList<ResultEntryDTO> BatchOperationsThroughResultEntry(IList<ResultMaster> ResultMasterList, IList<ResultOBR> ResultOBRList, IList<ResultORC> ResultORCList, IList<ResultOBX> ResultOBXList, IList<ResultNTE> ResultNTEList, IList<ResultZPS> ResultZPSList, IList<PatientResults> AddPatientResultsList, string sMacAddress);
        IList<ResultEntryDTO> DeleteOperationsThroughResultEntry(IList<ResultMaster> ResultMasterList, IList<ResultOBR> ResultOBRList, IList<ResultORC> ResultORCList, IList<ResultOBX> ResultOBXList, IList<ResultNTE> ResultNTEList, IList<ResultZPS> ResultZPSList, IList<PatientResults> DeletePatientResultsList, string sMacAddress);
        IList<ResultEntryDTO> GetResultEntry(ulong HumanID, string From);
        IList<ResultEntryDTO> UpdateOperationsThroughResultEntry(IList<ResultMaster> UpdateResultMasterList, IList<ResultOBR> UpdateResultOBRList, IList<ResultORC> UpdateResultORCList, IList<ResultOBX> SaveResultOBXList, IList<ResultOBX> UpdateResultOBXList, IList<ResultOBX> DeleteResultOBXList, IList<ResultNTE> SaveResultNTEList, IList<ResultNTE> UpdateResultNTEList, IList<ResultNTE> DeleteResultNTEList, IList<ResultZPS> UpdateResultZPSList, IList<PatientResults> AddPatientResultsList, IList<PatientResults> UpdatePatientResultsList, IList<PatientResults> DeletePatientResults, string sMacAddress);
        Stream GetSearchResultByLoincCodeAndDescription(string sLioncDescription, string sLioncCode, string sMacAddress);
        void SaveResultMasterThroughViewIndexedImages(IList<ResultMaster> ResultMasterList, string sMacAddress);
        // Added By Manimozhi - 24th Sep 2012 
        ResultMaster GetReviewCommentsForViewIndexedImages(ulong HumanId, string OrderID);
        IList<ResultEntryDTO> GetListOfResultsFromResultMasterID(ulong ResultMasterID, ulong ulHumanID);
        void UpdateResultMasterForOutsideOrdersForMRE(ulong resultMasterID, string sIsFilled, string sMacAddress);
        IList<ResultMaster> GetResultMasterListByResultmasterIDForMRE(ulong ulResultMasterID);
        FillResultMasterAndEntry GetResultUsingHumanId(ulong human_id);
        IList<FillOrderException> GetOrderExceptionItems(int PageNumber, int MaxResultsPerPage, string SearchCriteria, string NPINumbers, IList<string> FieldName, IList<string> Value);
        IList<FillOrderException> GetOrderDetails(int PageNumber, int MaxResultsPerPage, ulong OrderID, ulong Result_Master_ID, bool Is_Showall, bool Is_Patient_Matched, ulong Human_ID, ulong Lab_ID);
        void UpdateResultMaster(ulong ResultMasterID, ulong OrderSubmitID, string MacAddress);
        ulong SaveResultMasterforSummary(IList<ResultMaster> lstresultmaster);
        ResultMaster SaveResultMasterItem(ResultMaster objResultMaster, ulong ulFileManagementIndexID);
        void SaveResultMasterforDeleteFiles(IList<ResultMaster> lstResMasterUpdate);
        IList<ResultMaster> GetResultMasterByFileName(string fileName);
    }

    public partial class ResultMasterManager : ManagerBase<ResultMaster, ulong>, IResultMasterManager
    {
        #region Constructors


        public ResultMasterManager()
            : base()
        {

        }
        public ResultMasterManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        int iTryCount = 0;
        public void SaveAndUpdateResultReview(ResultMaster ResultReviewObj, string sMacAddress)
        {
            IList<ResultMaster> ilstResultMaster = null;
            ResultMaster objResultMaster = GetById(ResultReviewObj.Id);
            IList<ResultMaster> resRevInsList = new List<ResultMaster>();
            resRevInsList.Add(objResultMaster);
            if (resRevInsList.Count > 0)
            {
                resRevInsList[0].MA_Notes = ResultReviewObj.MA_Notes;
                resRevInsList[0].Result_Review_Comments = ResultReviewObj.Result_Review_Comments;
                resRevInsList[0].Result_Review_Date = ResultReviewObj.Result_Review_Date;
                resRevInsList[0].Result_Review_By = ResultReviewObj.Result_Review_By;
                resRevInsList[0].Modified_By = ResultReviewObj.Modified_By;
                resRevInsList[0].Modified_Date_And_Time = ResultReviewObj.Modified_Date_And_Time;
                //SaveUpdateDeleteWithTransaction(ref ilstResultMaster, resRevInsList, null, sMacAddress);
                SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstResultMaster, ref resRevInsList, null, sMacAddress, false, false, 0, string.Empty);
            }
        }
        public IList<ResultMaster> GetResultReviewNotesBasedOnOrderSubmitId(ulong Order_ID)
        {
            IList<ResultMaster> resRev = new List<ResultMaster>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ResultMaster)).Add(Expression.Eq("Order_ID", Order_ID));
                if (crit.List<ResultMaster>() != null && crit.List<ResultMaster>().Count > 0)
                {
                    resRev = crit.List<ResultMaster>();
                }
                iMySession.Close();

            }
            return resRev;
        }
        //public void SaveResultMaster(IList<ResultMaster> resultMasterList, IList<ResultORC> resultORCList, IList<ResultOBR> resultOBRList, IList<ResultOBX> resultOBXList, IList<ResultNTE> resultNTEList, IList<ResultZEF> resultZEFList, IList<ResultZPS> resultZPSList, string macAddress)
        public void ImprotLabResultsForOutsideOrders(IList<ResultMaster> resultMasterList, IList<ResultORC> resultORCList, IList<ResultOBR> resultOBRList, IList<ResultOBX> resultOBXList, IList<ResultNTE> resultNTEList, IList<ResultZEF> resultZEFList, IList<ResultZPS> resultZPSList, string macAddress)
        {
            iTryCount = 0;
            GenerateXml XMLObj = new GenerateXml();
            IList<ulong> UnwantedResultMasterID = new List<ulong>();
        TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            WFObjectManager objWFObjectManager = new WFObjectManager();
            try
            {
                trans = MySession.BeginTransaction();

                if (resultMasterList != null && resultMasterList.Count != 0)
                {
                    //Result Mapping To Patient
                    for (int i = 0; i < resultMasterList.Count; i++)
                    {
                        ulong Human_ID;
                        if (resultMasterList[i].PID_External_Patient_ID != string.Empty)
                        {
                            Human_ID = Convert.ToUInt64(resultMasterList[i].PID_External_Patient_ID);
                        }
                        else
                        {
                            Human_ID = 0;
                        }
                        DateTime dtpBirthdate = DateTime.MinValue;
                        string sBirthdate = resultMasterList[i].PID_Patient_Date_Of_Birth;
                        dtpBirthdate = DateTime.ParseExact(sBirthdate, "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                        string value = dtpBirthdate.ToString("yyyy-mm-dd");
                        System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                        dateInfo.ShortDatePattern = "yyyy-mm-dd";
                        DateTime validDate = Convert.ToDateTime(value, dateInfo);
                        //IQuery query1 = session.GetISession().GetNamedQuery("ResultMatchingCriteriaForLabResults");
                        //query1.SetString(0, resultMasterList[i].Order_ID.ToString());
                        //query1.SetString(1, resultMasterList[i].PID_External_Patient_ID);
                        //query1.SetString(2, resultMasterList[i].PID_Patient_Last_Name);
                        //query1.SetString(3, resultMasterList[i].PID_Patient_First_Name);
                        //query1.SetString(4, resultMasterList[i].PID_Patient_Middle_Name);
                        //query1.SetDateTime(5,validDate);
                        //ArrayList arr1 = new ArrayList(query1.List());
                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                        {
                            ISQLQuery sql = iMySession.CreateSQLQuery("select * from orders_submit os , human h where h.human_id=os.human_id and os.order_submit_id= '" + resultMasterList[i].Order_ID.ToString() + "' and os.human_id= '" + resultMasterList[i].PID_External_Patient_ID + "' and  h.First_Name= '" + resultMasterList[i].PID_Patient_First_Name + "' and h.Last_Name= '" + resultMasterList[i].PID_Patient_Last_Name + "' and h.Birth_Date= '" + validDate.ToString("yyyy-MM-dd") + "'");
                            //ISQLQuery sql = session.GetISession().CreateSQLQuery("select * from orders_submit os left join human h on (h.human_id=os.human_id) where os.orders_submit_id= '" + resultMasterList[i].Order_ID.ToString() + "' and os.human_id= '" + resultMasterList[i].PID_External_Patient_ID + "' and  concat('TEST','PATIENT','"+string.Empty+"')='TESTPATIENT'"); //h.last_name,h.first_name,h.mi)='" +resultMasterList[i].PID_Patient_Last_Name+resultMasterList[i].PID_Patient_First_Name+ string.Empty + "'" );
                            if (sql.List() != null && sql.List().Count > 0)
                            {
                                continue;
                            }
                            else
                            {
                                string CurrentPatientSex = resultMasterList[i].PID_Patient_Gender == "M" ? "MALE" : "FEMALE";
                                ISQLQuery sql1 = iMySession.CreateSQLQuery("select s. * from human s where(s.last_name='" + resultMasterList[i].PID_Patient_Last_Name + "' and s.first_name='" + resultMasterList[i].PID_Patient_First_Name + "' and s.Birth_date='" + validDate.ToString("yyyy-MM-dd") + "' and s.sex='" + CurrentPatientSex + "')").AddEntity("s", typeof(Human));
                                if (sql1.List<Human>().Count > 0)
                                {
                                    resultMasterList[i].PID_External_Patient_ID = Convert.ToString(sql.List<Human>()[0].Id);
                                }
                                else
                                {
                                    resultMasterList[i].PID_External_Patient_ID = " ";
                                    UnwantedResultMasterID.Add(Convert.ToUInt64(i));
                                }
                            }
                            iMySession.Close();
                        }
                    }

                    IList<ResultMaster> ResultMasterAfterPatientMapping = resultMasterList.Except(resultMasterList.Where(r => r.PID_External_Patient_ID == " ")).ToList<ResultMaster>();
                    //iResult = SaveUpdateDeleteWithoutTransaction(ref ResultMasterAfterPatientMapping, null, null, MySession, macAddress);
                    IList<ResultMaster> ResultMasterAfterPatientMappingnull = null;
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ResultMasterAfterPatientMapping, ref ResultMasterAfterPatientMappingnull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);


                    //iResult = SaveUpdateDeleteWithoutTransaction(ref resultMasterList, null, null, MySession, macAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                IList<ResultORC> orcInsertList = new List<ResultORC>();
                if (resultORCList != null && resultORCList.Count != 0)
                {
                    for (int i = 0; i < resultORCList.Count; i++)
                    {
                        if (!UnwantedResultMasterID.Contains(resultORCList[i].Result_Master_ID))
                        {
                            int masterID = Convert.ToInt32(resultORCList[i].Result_Master_ID);
                            resultORCList[i].Result_Master_ID = resultMasterList[masterID].Id;
                            orcInsertList.Add(resultORCList[i]);
                        }
                    }
                    ResultORCManager ORCMgr = new ResultORCManager();
                    iResult = ORCMgr.SaveResultORC(ref orcInsertList, MySession, macAddress);
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
                IList<ResultOBR> obrInsertList = new List<ResultOBR>();
                if (resultOBRList != null && resultOBRList.Count != 0)
                {
                    for (int i = 0; i < resultOBRList.Count; i++)
                    {
                        if (!UnwantedResultMasterID.Contains(resultOBRList[i].Result_Master_ID))
                        {
                            int masterID = Convert.ToInt32(resultOBRList[i].Result_Master_ID);
                            resultOBRList[i].Result_Master_ID = resultMasterList[masterID].Id;
                            obrInsertList.Add(resultOBRList[i]);
                        }

                    }

                    ResultOBRManager OBRMgr = new ResultOBRManager();
                    iResult = OBRMgr.SaveResultOBR(ref obrInsertList, MySession, macAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                IList<ResultOBX> obxInsertList = new List<ResultOBX>();
                if (resultOBXList != null && resultOBXList.Count != 0)
                {
                    for (int i = 0; i < resultOBXList.Count; i++)
                    {
                        if (!UnwantedResultMasterID.Contains(resultOBXList[i].Result_Master_ID))
                        {
                            int masterID = Convert.ToInt32(resultOBXList[i].Result_Master_ID);
                            resultOBXList[i].Result_Master_ID = resultMasterList[masterID].Id;
                            int ObrID = Convert.ToInt32(resultOBXList[i].Result_OBR_ID);
                            resultOBXList[i].Result_OBR_ID = obrInsertList[ObrID].Id;
                            obxInsertList.Add(resultOBXList[i]);
                        }
                    }
                    ResultOBXManager OBXMgr = new ResultOBXManager();
                    iResult = OBXMgr.SaveResultOBX(ref obxInsertList, MySession, macAddress);
                    //******************************************************************************************
                    //For Bug ID 55638 (Patient Results should not include electronic reults)
                    //******************************************************************************************
                    //IList<PatientResults> ilisPatientResults = new List<PatientResults>();
                    //PatientResults objPatientResults = new PatientResults();
                    //var resultMasterAndOrderId = (from rec in obxInsertList
                    //                              join rec2 in resultMasterList
                    //                              on rec.Result_Master_ID equals rec2.Order_ID
                    //                              select new { rec.Id, rec2.Order_ID }).ToList();
                    //foreach (ResultOBX obj in obxInsertList)
                    //{
                    //    if (obj.OBX_Loinc_Identifier != string.Empty)
                    //    {
                    //        objPatientResults = new PatientResults();
                    //        var OrdersObjForThisOBX = (from rec in resultMasterAndOrderId where rec.Id == obj.Result_Master_ID select rec).SingleOrDefault();
                    //        ICriteria critOrders = session.GetISession().CreateCriteria(typeof(Orders)).Add(Expression.Eq("Id", (from rec in resultMasterList where rec.Id == obj.Result_Master_ID select rec.Id).SingleOrDefault()));
                    //        IList<Orders> IDS = critOrders.List<Orders>();
                    //        objPatientResults.Abnormal_Flags = obj.OBX_Abnormal_Flag;
                    //        objPatientResults.Created_By = obj.Created_By;
                    //        objPatientResults.Created_Date_And_Time = obj.Created_Date_And_Time;
                    //        if (IDS.Count > 0)
                    //        {
                    //            objPatientResults.Encounter_ID = IDS[0].Encounter_ID;
                    //            objPatientResults.Human_ID = IDS[0].Human_ID;
                    //            objPatientResults.Physician_ID = IDS[0].Physician_ID;
                    //        }
                    //        objPatientResults.Loinc_Identifier = obj.OBX_Loinc_Identifier;
                    //        objPatientResults.Loinc_Observation = obj.OBX_Loinc_Observation_Text;
                    //        objPatientResults.Reference_Range = obj.OBX_Reference_Range;
                    //        objPatientResults.Units = obj.OBX_Units;
                    //        objPatientResults.Value = obj.OBX_Observation_Value;
                    //        objPatientResults.Results_Type = "Results";



                    //        DateTime dtpBirthdate = DateTime.MinValue;
                    //        string sBirthdate = (from rec in obrInsertList where rec.Id == obj.Result_OBR_ID select rec.OBR_Specimen_Collection_Date_And_Time).SingleOrDefault();
                    //        dtpBirthdate = DateTime.ParseExact(sBirthdate, "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                    //        string value = dtpBirthdate.ToString("yyyy-mm-dd");
                    //        System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                    //        dateInfo.ShortDatePattern = "yyyy-mm-dd";
                    //        DateTime validDate = Convert.ToDateTime(value, dateInfo);
                    //        objPatientResults.Captured_date_and_time = validDate;
                    //        ilisPatientResults.Add(objPatientResults);
                    //    }
                    //}
                    //VitalsManager objVitalsManager = new VitalsManager();
                    //IList<PatientResults> ilisPatientResultsnull = null;
                    ////iResult = objVitalsManager.SaveUpdateDeleteWithoutTransaction(ref ilisPatientResults, null, null, MySession, macAddress);
                    //iResult = objVitalsManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilisPatientResults, ref ilisPatientResultsnull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);
                    //if (iResult == 2)
                    //{
                    //    if (iTryCount < 5)
                    //    {
                    //        iTryCount++;
                    //        goto TryAgain;
                    //    }
                    //    else
                    //    {
                    //        trans.Rollback();
                    //        // MySession.Close();
                    //        throw new Exception("Deadlock occurred. Transaction failed.");
                    //    }
                    //}
                    //else if (iResult == 1)
                    //{
                    //    trans.Rollback();
                    //    // MySession.Close();
                    //    throw new Exception("Exception occurred. Transaction failed.");
                    //}
                }
                IList<ResultNTE> nteInsertList = new List<ResultNTE>();
                if (resultNTEList != null && resultNTEList.Count != 0)
                {
                    for (int i = 0; i < resultNTEList.Count; i++)
                    {
                        if (!UnwantedResultMasterID.Contains(resultNTEList[i].Result_Master_ID))
                        {
                            int masterID = Convert.ToInt32(resultNTEList[i].Result_Master_ID);
                            resultNTEList[i].Result_Master_ID = resultMasterList[masterID].Id;
                            if (resultNTEList[i].Comment_Type == "ASR Comments")
                            {
                                int ObrId = Convert.ToInt32(resultNTEList[i].Result_OBR_ID);
                                resultNTEList[i].Result_OBR_ID = obrInsertList[ObrId].Id;
                                int ObxId = Convert.ToInt32(resultNTEList[i].Result_OBX_ID);
                                resultNTEList[i].Result_OBX_ID = obxInsertList[ObxId].Id;
                            }
                            nteInsertList.Add(resultNTEList[i]);
                        }
                    }
                    ResultNTEManager NTEMgr = new ResultNTEManager();
                    iResult = NTEMgr.SaveResultNTE(ref nteInsertList, MySession, macAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                IList<ResultZEF> ZefInsertList = new List<ResultZEF>();
                if (resultZEFList != null && resultZEFList.Count != 0)
                {
                    for (int i = 0; i < resultZEFList.Count; i++)
                    {
                        if (!UnwantedResultMasterID.Contains(resultZEFList[i].Result_Master_ID))
                        {
                            int masterID = Convert.ToInt32(resultZEFList[i].Result_Master_ID);
                            resultZEFList[i].Result_Master_ID = resultMasterList[masterID].Id;
                            ZefInsertList.Add(resultZEFList[i]);
                        }
                    }
                    ResultZEFManager ZEFMgr = new ResultZEFManager();
                    iResult = ZEFMgr.SaveResultZEF(ref ZefInsertList, MySession, macAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }

                IList<ResultZPS> ZpsInsertList = new List<ResultZPS>();
                if (resultZPSList != null && resultZPSList.Count != 0)
                {
                    for (int i = 0; i < resultZPSList.Count; i++)
                    {
                        if (!UnwantedResultMasterID.Contains(resultZPSList[i].Result_Master_ID))
                        {
                            int masterID = Convert.ToInt32(resultZPSList[i].Result_Master_ID);
                            resultZPSList[i].Result_Master_ID = resultMasterList[masterID].Id;
                            ZpsInsertList.Add(resultZPSList[i]);
                        }
                    }
                    ResultZPSManager ZPSMgr = new ResultZPSManager();
                    iResult = ZPSMgr.SaveResultZPS(ref ZpsInsertList, MySession, macAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                //MySession.Flush();
                trans.Commit();
                //If Current Process is Billing_Wait and Result_Process -Start  
                IList<ulong> ControlIds = new List<ulong>();
                for (int i = 0; i < resultORCList.Count; i++)
                {
                    ControlIds.Add(Convert.ToUInt64(resultORCList[i].ORC_Specimen_ID.Substring(4)));

                }
                IList<WFObject> ilstWfObject = objWFObjectManager.GetListofObjectsByCurrentProcess("BILLING_WAIT");
                IList<WFObject> ilstBillingWait = ((from s in ilstWfObject
                                                    where s.Obj_Type == "DIAGNOSTIC ORDER" && ControlIds.Contains(s.Obj_System_Id)
                                                    select s)).ToList<WFObject>();
                if (ilstBillingWait.Count > 0)
                {
                    for (int i = 0; i < ilstBillingWait.Count; i++)
                    {
                        string[] ProcessAllocation = ilstBillingWait[i].Process_Allocation.Split('|');
                        string[] ResultReview = ((from u in ProcessAllocation
                                                  where u.StartsWith("RESULT_REVIEW-")
                                                  select u).Distinct()).ToArray<string>();
                        if (ResultReview.Count() > 0)
                        {
                            for (int k = 0; k < ResultReview.Count(); k++)
                            {
                                string[] UserName = (ResultReview[0].ToString()).Split('-');
                                objWFObjectManager.MoveToPreviousProcess(ilstBillingWait[i].Obj_System_Id, "DIAGNOSTIC ORDER", 1, UserName[1], DateTime.Now, string.Empty);
                            }
                        }

                    }
                }
                IList<WFObject> ilstWfObjectResultReview = objWFObjectManager.GetListofObjectsByCurrentProcess("RESULT_PROCESS");
                IList<WFObject> ilstResultProcess = (from s in ilstWfObjectResultReview
                                                     where s.Obj_Type == "DIAGNOSTIC ORDER" && ControlIds.Contains(s.Obj_System_Id)
                                                     select s).ToList<WFObject>();
                if (ilstResultProcess.Count > 0)
                {
                    UserManager objUserManager = new UserManager();
                    IList<User> User = objUserManager.GetAll();
                    OrdersManager objOrdersManager = new OrdersManager();
                    Orders objOrder = new Orders();
                    for (int i = 0; i < ilstResultProcess.Count; i++)
                    {
                        ulong phyid = objOrdersManager.GetPhysicianIDByOrderSubmitID(ilstResultProcess[0].Obj_System_Id);
                        IList<User> OwnerDetail = (from u in User
                                                   where u.Physician_Library_ID == phyid
                                                   select u).ToList<User>();
                        if (OwnerDetail.Count() > 0)
                        {
                            objWFObjectManager.MoveToNextProcess(ilstResultProcess[i].Obj_System_Id, ilstResultProcess[i].Obj_Type, 1, OwnerDetail[0].user_name, DateTime.Now, string.Empty, null, null);
                        }
                    }
                }
            }
            //If Current Process is Billing_Wait and Result_Process -End 
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                MySession.Close();
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }
            catch (Exception e)
            {
                trans.Rollback();
                MySession.Close();
                //CAP-1942
                throw new Exception(e.Message,e);
            }

        }

        public bool ImprotLabResultsForCapellaOrders(IList<ResultMaster> resultMasterList, IList<ResultORC> resultORCList, IList<ResultOBR> resultOBRList, IList<ResultOBX> resultOBXList, IList<ResultNTE> resultNTEList, IList<ResultZEF> resultZEFList, IList<ResultZPS> resultZPSList, string macAddress)
        {
            bool bUnimportedFile = false;
            //bool bIs_abnormal_obx = false;
            iTryCount = 0;
            GenerateXml XMLObj = new GenerateXml();
            UserManager objUserManager = new UserManager();
            PhysicianManager objPhyLibraryManager = new PhysicianManager();
            IList<ResultMaster> ResultMasterAfterPatientMapping = new List<ResultMaster>();
            IList<ResultORC> ResultORCAfterInsert = new List<ResultORC>();
            IList<ErrorLog> ilstErrorLog = new List<ErrorLog>();
            IList<StaticLookup> ilstStaticLookup = new List<StaticLookup>();
            StaticLookupManager objStaticLookupManager = new StaticLookupManager();
            ilstStaticLookup = objStaticLookupManager.getStaticLookupByFieldName("ERROR_CODE");
        TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            int LastProcessedIndex = 0;
            WFObjectManager objWFObjectManager = new WFObjectManager();

            try
            {


                trans = MySession.BeginTransaction();
                ErrorLogManager objErrorLogManager = new ErrorLogManager();
                if (resultMasterList != null && resultMasterList.Count != 0)
                {
                    if (resultMasterList != null)
                    {
                        ISQLQuery sql;
                        for (int i = 0; i < resultMasterList.Count; i++)
                        {
                            LastProcessedIndex = i;


                            resultMasterList[i].temp_property = i.ToString();
                            IList<OrdersSubmit> MatchedOrdersList = new List<OrdersSubmit>();

                            DateTime dtpBirthdate = DateTime.MinValue;
                            string sBirthdate = resultMasterList[i].PID_Patient_Date_Of_Birth;
                            DateTime validDate = DateTime.MinValue;
                            if (resultMasterList[i].PID_Patient_Date_Of_Birth != string.Empty)
                            {
                                if (sBirthdate.Trim() != string.Empty)
                                    dtpBirthdate = DateTime.ParseExact(sBirthdate, "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                                string value = dtpBirthdate.ToString("yyyy-mm-dd");
                                System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                                dateInfo.ShortDatePattern = "yyyy-mm-dd";
                                validDate = Convert.ToDateTime(value, dateInfo);
                            }



                            if (resultMasterList[i].PID_Patient_First_Name == string.Empty || resultMasterList[i].PID_Patient_Last_Name == string.Empty || resultMasterList[i].PID_Patient_Date_Of_Birth == string.Empty)
                            {
                                ErrorLog objErrorLog = new ErrorLog();
                                objErrorLog.File_Name = resultMasterList[i].File_Name;
                                objErrorLog.Created_By = "ACURUS";
                                objErrorLog.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                objErrorLog.Lab_ID = resultMasterList[i].Lab_ID;
                                objErrorLog.Reason_Code = "ACUR_LAB_03";
                                objErrorLog.Patient_First_Name = resultMasterList[i].PID_Patient_First_Name;
                                objErrorLog.Patient_Last_Name = resultMasterList[i].PID_Patient_Last_Name;
                                objErrorLog.Patient_MI_Name = resultMasterList[i].PID_Patient_Middle_Name;
                                objErrorLog.Patient_Gender = resultMasterList[i].PID_Patient_Gender;
                                objErrorLog.Patient_DOB = validDate;
                                objErrorLog.Is_Deleted = "N";
                                if (ilstStaticLookup.Count > 0)
                                {
                                    StaticLookup objStaticLookup = new StaticLookup();
                                    objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_03").SingleOrDefault();
                                    objErrorLog.Reason_Description = objStaticLookup.Description;
                                }
                                objErrorLog.Result_Master_ID = Convert.ToUInt32(i);
                                ilstErrorLog.Add(objErrorLog);
                                IList<ErrorLog> ilstErrorLognull = null;
                                //objErrorLogManager.SaveUpdateDeleteWithoutTransaction(ref ilstErrorLog, null, null, MySession, macAddress);
                                objErrorLogManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilstErrorLog, ref ilstErrorLognull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);
                                //For Bug ID : 68705
                                //bUnimportedFile = true; 
                                continue;
                            }
                            if (resultMasterList[i].Order_ID != 0)
                            {

                                if (resultMasterList[i].PID_External_Patient_ID != "" && resultMasterList[i].PID_External_Patient_ID.ToUpper() != "NI" && Regex.Matches(resultMasterList[i].PID_External_Patient_ID, @"[a-zA-Z]").Count == 0)
                                {
                                    sql = session.GetISession().CreateSQLQuery("select * from orders_submit os where os.order_submit_id= " + resultMasterList[i].Order_ID.ToString() + " and os.human_id=" + resultMasterList[i].PID_External_Patient_ID).AddEntity("os", typeof(OrdersSubmit));
                                    MatchedOrdersList = sql.List<OrdersSubmit>();
                                }
                                if (MatchedOrdersList != null&&MatchedOrdersList.Count > 0)
                                    resultMasterList[i].Matching_Patient_Id = Convert.ToUInt32(resultMasterList[i].PID_External_Patient_ID);
                                else if (MatchedOrdersList != null && MatchedOrdersList.Count == 0)
                                {
                                    ulong Human_ID;
                                    //CAP-2901
                                    Human_ID = GetHumanIdbyname(resultMasterList[i].PID_Patient_First_Name, resultMasterList[i].PID_Patient_Last_Name, resultMasterList[i].PID_Patient_Date_Of_Birth, resultMasterList[i].PID_Patient_Gender, MySession);
                                    if (Human_ID == 0)
                                    {
                                        //ACUR_LAB_01
                                        ErrorLog objErrorLog = new ErrorLog();
                                        objErrorLog.File_Name = resultMasterList[i].File_Name;
                                        objErrorLog.Created_By = "ACURUS";
                                        objErrorLog.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                        objErrorLog.Lab_ID = resultMasterList[i].Lab_ID;
                                        objErrorLog.Reason_Code = "ACUR_LAB_01";
                                        objErrorLog.Patient_First_Name = resultMasterList[i].PID_Patient_First_Name;
                                        objErrorLog.Patient_Last_Name = resultMasterList[i].PID_Patient_Last_Name;
                                        objErrorLog.Patient_MI_Name = resultMasterList[i].PID_Patient_Middle_Name;
                                        objErrorLog.Patient_Gender = resultMasterList[i].PID_Patient_Gender;
                                        objErrorLog.Patient_DOB = validDate;
                                        objErrorLog.Is_Deleted = "N";
                                        if (ilstStaticLookup.Count > 0)
                                        {
                                            StaticLookup objStaticLookup = new StaticLookup();
                                            objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_01").SingleOrDefault();
                                            objErrorLog.Reason_Description = objStaticLookup.Description;
                                        }
                                        objErrorLog.Result_Master_ID = Convert.ToUInt32(i);
                                        ilstErrorLog.Add(objErrorLog);

                                        resultMasterList[i].Matching_Patient_Id = 0;
                                    }
                                    else
                                    {
                                        resultMasterList[i].Matching_Patient_Id = Human_ID;
                                    }
                                }
                                //muthusamy on 3-Dec-2014 LabChanges fill in matching patient id in result master table.
                                //else if (MatchedOrdersList.Count > 0)
                                //    resultMasterList[i].Matching_Patient_Id = Convert.ToUInt32(resultMasterList[i].PID_External_Patient_ID);
                                //$muthusamy on 3-Dec-2014 LabChanges
                            }
                            else
                            {
                                ulong Human_ID;
                                //CAP-2901
                                Human_ID = GetHumanIdbyname(resultMasterList[i].PID_Patient_First_Name, resultMasterList[i].PID_Patient_Last_Name, resultMasterList[i].PID_Patient_Date_Of_Birth, resultMasterList[i].PID_Patient_Gender, MySession);
                                if (Human_ID == 0)
                                {
                                    //ACUR_LAB_02
                                    ErrorLog objErrorLog = new ErrorLog();
                                    objErrorLog.File_Name = resultMasterList[i].File_Name;
                                    objErrorLog.Created_By = "ACURUS";
                                    objErrorLog.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    objErrorLog.Lab_ID = resultMasterList[i].Lab_ID;
                                    objErrorLog.Reason_Code = "ACUR_LAB_02";
                                    objErrorLog.Patient_First_Name = resultMasterList[i].PID_Patient_First_Name;
                                    objErrorLog.Patient_Last_Name = resultMasterList[i].PID_Patient_Last_Name;
                                    objErrorLog.Patient_MI_Name = resultMasterList[i].PID_Patient_Middle_Name;
                                    objErrorLog.Patient_Gender = resultMasterList[i].PID_Patient_Gender;
                                    objErrorLog.Patient_DOB = validDate;
                                    objErrorLog.Is_Deleted = "N";
                                    if (ilstStaticLookup.Count > 0)
                                    {
                                        StaticLookup objStaticLookup = new StaticLookup();
                                        objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_02").SingleOrDefault();
                                        objErrorLog.Reason_Description = objStaticLookup.Description;
                                    }
                                    objErrorLog.Result_Master_ID = Convert.ToUInt32(i);
                                    ilstErrorLog.Add(objErrorLog);

                                    resultMasterList[i].Matching_Patient_Id = 0;
                                }
                                else
                                    resultMasterList[i].Matching_Patient_Id = Human_ID;
                            }
                            //if (MatchedOrdersList != null && MatchedOrdersList.Count > 0)
                            //{
                            //    ulong Human_ID;
                            //    Human_ID = GetHumanIdbyname(resultMasterList[i].PID_Patient_First_Name, resultMasterList[i].PID_Patient_Last_Name, resultMasterList[i].PID_Patient_Middle_Name, resultMasterList[i].PID_Patient_Date_Of_Birth, resultMasterList[i].PID_Patient_Gender);
                            //    if (Human_ID == 0)
                            //    {
                            //        //ACUR_LAB_01
                            //        ErrorLog objErrorLog = new ErrorLog();
                            //        objErrorLog.File_Name = resultMasterList[i].File_Name;
                            //        objErrorLog.Created_By = "ACURUS";
                            //        objErrorLog.Created_Date_And_Time = DateTime.Now;
                            //        objErrorLog.Lab_ID = resultMasterList[i].Lab_ID;
                            //        objErrorLog.Reason_Code = "ACUR_LAB_01";
                            //        objErrorLog.Patient_First_Name = resultMasterList[i].PID_Patient_First_Name;
                            //        objErrorLog.Patient_Last_Name = resultMasterList[i].PID_Patient_Last_Name;
                            //        objErrorLog.Patient_MI_Name = resultMasterList[i].PID_Patient_Middle_Name;
                            //        objErrorLog.Patient_Gender = resultMasterList[i].PID_Patient_Gender;
                            //        objErrorLog.Patient_DOB = validDate;

                            //        if (ilstStaticLookup.Count > 0)
                            //        {
                            //             StaticLookup objStaticLookup= new StaticLookup();
                            //             objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_01").SingleOrDefault();
                            //            objErrorLog.Reason_Description = objStaticLookup.Description;
                            //        }
                            //        objErrorLog.Result_Master_ID =Convert.ToUInt32(i);
                            //        ilstErrorLog.Add(objErrorLog);

                            //        resultMasterList[i].PID_External_Patient_ID = "0";
                            //    }
                            //    else
                            //        resultMasterList[i].PID_External_Patient_ID = Human_ID.ToString();
                            //}
                            //else
                            //{
                            //    resultMasterList[i].Order_ID = 0;
                            //    ulong Human_ID;
                            //    Human_ID = GetHumanIdbyname(resultMasterList[i].PID_Patient_First_Name, resultMasterList[i].PID_Patient_Last_Name, resultMasterList[i].PID_Patient_Middle_Name, resultMasterList[i].PID_Patient_Date_Of_Birth, resultMasterList[i].PID_Patient_Gender);
                            //    if (Human_ID == 0)
                            //    {
                            //        //ACUR_LAB_02
                            //        ErrorLog objErrorLog = new ErrorLog();
                            //        objErrorLog.File_Name = resultMasterList[i].File_Name;
                            //        objErrorLog.Created_By = "ACURUS";
                            //        objErrorLog.Created_Date_And_Time = DateTime.Now;
                            //        objErrorLog.Lab_ID = resultMasterList[i].Lab_ID;
                            //        objErrorLog.Reason_Code = "ACUR_LAB_02";
                            //        if (ilstStaticLookup.Count > 0)
                            //        {
                            //            StaticLookup objStaticLookup = new StaticLookup();
                            //            objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_02").SingleOrDefault();
                            //            objErrorLog.Reason_Description = objStaticLookup.Description;
                            //        }
                            //        objErrorLog.Patient_First_Name = resultMasterList[i].PID_Patient_First_Name;
                            //        objErrorLog.Patient_Last_Name = resultMasterList[i].PID_Patient_Last_Name;
                            //        objErrorLog.Patient_MI_Name = resultMasterList[i].PID_Patient_Middle_Name;
                            //        objErrorLog.Patient_Gender = resultMasterList[i].PID_Patient_Gender;
                            //        objErrorLog.Patient_DOB = validDate;
                            //        objErrorLog.Result_Master_ID = Convert.ToUInt32(i);
                            //        ilstErrorLog.Add(objErrorLog);

                            //        resultMasterList[i].PID_External_Patient_ID = "0";
                            //    }
                            //    else
                            //    {
                            //        //ACUR_LAB_03
                            //        ErrorLog objErrorLog = new ErrorLog();
                            //        objErrorLog.File_Name = resultMasterList[i].File_Name;
                            //        objErrorLog.Created_By = "ACURUS";
                            //        objErrorLog.Created_Date_And_Time = DateTime.Now;
                            //        objErrorLog.Lab_ID = resultMasterList[i].Lab_ID;
                            //        objErrorLog.Reason_Code = "ACUR_LAB_03";
                            //        if (ilstStaticLookup.Count > 0)
                            //        {
                            //            StaticLookup objStaticLookup = new StaticLookup();
                            //            objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_03").SingleOrDefault();
                            //            objErrorLog.Reason_Description = objStaticLookup.Description;
                            //        }
                            //        objErrorLog.Patient_First_Name = resultMasterList[i].PID_Patient_First_Name;
                            //        objErrorLog.Patient_Last_Name = resultMasterList[i].PID_Patient_Last_Name;
                            //        objErrorLog.Patient_MI_Name = resultMasterList[i].PID_Patient_Middle_Name;
                            //        objErrorLog.Patient_Gender = resultMasterList[i].PID_Patient_Gender;
                            //        objErrorLog.Patient_DOB = validDate;
                            //        objErrorLog.Result_Master_ID = Convert.ToUInt32(i);
                            //        ilstErrorLog.Add(objErrorLog);
                            //        resultMasterList[i].PID_External_Patient_ID = Human_ID.ToString();
                            //    }
                            //}
                        }

                    }
                    #region OldMatching
                    //Result Mapping To Patient
                    //for (int i = 0; i < resultMasterList.Count; i++)
                    //{
                    //    ulong Human_ID;
                    //    if (resultMasterList[i].PID_External_Patient_ID != string.Empty)
                    //    {
                    //        if (resultMasterList[i].PID_External_Patient_ID == "NI")
                    //        {
                    //            Human_ID = GetHumanIdbyname(resultMasterList[i].PID_Patient_First_Name, resultMasterList[i].PID_Patient_Last_Name, resultMasterList[i].PID_Patient_Middle_Name, resultMasterList[i].PID_Patient_Date_Of_Birth);
                    //            resultMasterList[i].PID_External_Patient_ID = Human_ID.ToString();
                    //        }
                    //        else
                    //        {
                    //            Human_ID = Convert.ToUInt64(resultMasterList[i].PID_External_Patient_ID);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Human_ID = 0;
                    //    }
                    //    try
                    //    {
                    //        DateTime dtpBirthdate = DateTime.MinValue;
                    //        string sBirthdate = resultMasterList[i].PID_Patient_Date_Of_Birth;
                    //        if (sBirthdate.Trim() != string.Empty)
                    //            dtpBirthdate = DateTime.ParseExact(sBirthdate, "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                    //        string value = dtpBirthdate.ToString("yyyy-mm-dd");
                    //        System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                    //        dateInfo.ShortDatePattern = "yyyy-mm-dd";
                    //        DateTime validDate = Convert.ToDateTime(value, dateInfo);

                    //        //IQuery query1 = session.GetISession().GetNamedQuery("ResultMatchingCriteriaForLabResults");
                    //        //query1.SetString(0, resultMasterList[i].Order_ID.ToString());
                    //        //query1.SetString(1, resultMasterList[i].PID_External_Patient_ID);
                    //        //query1.SetString(2, resultMasterList[i].PID_Patient_Last_Name);
                    //        //query1.SetString(3, resultMasterList[i].PID_Patient_First_Name);
                    //        //query1.SetString(4, resultMasterList[i].PID_Patient_Middle_Name);
                    //        //query1.SetString(5, validDate.ToString("yyyy-MM-dd"));
                    //        //ArrayList arr1 = new ArrayList(query1.List());

                    //        //h.Last_Name=\"" + resultMasterList[i].PID_Patient_Last_Name + "\" and
                    //        if (resultMasterList[i].PID_External_Patient_ID.Trim() == string.Empty)
                    //        {
                    //            resultMasterList[i].Order_ID = 0;
                    //            continue;
                    //        }

                    //        ISQLQuery sql = session.GetISession().CreateSQLQuery("select * from orders_submit os , human h where h.human_id=os.human_id and os.order_submit_id= " + resultMasterList[i].Order_ID.ToString() + " and os.human_id= " + resultMasterList[i].PID_External_Patient_ID + " and  h.First_Name= '" + resultMasterList[i].PID_Patient_First_Name + "' and h.Last_Name= '" + resultMasterList[i].PID_Patient_Last_Name + "' and h.MI= '" + resultMasterList[i].PID_Patient_Middle_Name + "' and h.Birth_Date= '" + validDate.ToString("yyyy-MM-dd") + "'").AddEntity("h", typeof(Human)).AddEntity("os", typeof(OrdersSubmit));
                    //        if (sql.List() != null && sql.List().Count > 0)
                    //        {
                    //            continue;
                    //        }
                    //        else
                    //        {
                    //            //ISQLQuery sql = session.GetISession().CreateSQLQuery("select s. * from human s where(s.last_name='" + resultMasterList[i].PID_Patient_Last_Name + "' and s.first_name='" + resultMasterList[i].PID_Patient_First_Name + "' and s.Birth_date='" + validDate.ToString("yyyy-MM-dd") + "')").AddEntity("s", typeof(Human));

                    //            ////            ICriteria criteria = session.GetISession().CreateCriteria(typeof(Human)).Add(Expression.Eq("Last_Name", resultMasterList[i].PID_Patient_Last_Name))
                    //            ////.Add(Expression.Eq("First_Name", resultMasterList[i].PID_Patient_First_Name)).Add(Expression.Eq("Birth_Date", validDate));
                    //            //if (sql.List<Human>().Count > 0)
                    //            //{
                    //            //    resultMasterList[i].PID_External_Patient_ID = Convert.ToString(sql.List<Human>()[0].Id);
                    //            //}
                    //            //else
                    //            //{
                    //            // resultMasterList[i].PID_External_Patient_ID = "0";
                    //            resultMasterList[i].Order_ID = 0;
                    //            //ResultMasterToBeSkipped.Add(Convert.ToUInt64(i));
                    //            //UnwantedResultMasterID.Add(Convert.ToUInt64(i));
                    //            //}

                    //        }
                    //    }
                    //    catch (Exception exc)
                    //    {
                    //        continue;
                    //    }
                    //}
                    #endregion
                    ResultMasterAfterPatientMapping = resultMasterList;
                    IList<ResultMaster> ResultMasterAfterPatientMappingnull = null;
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ResultMasterAfterPatientMapping, ref ResultMasterAfterPatientMappingnull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);
                    //iResult = SaveUpdateDeleteWithoutTransaction(ref ResultMasterAfterPatientMapping, null, null, MySession, macAddress);

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


                }
                IList<ResultORC> orcInsertList = new List<ResultORC>();
                if (resultORCList != null && resultORCList.Count != 0)
                {
                    for (int i = 0; i < resultORCList.Count; i++)
                    {
                        resultORCList[i].temp_property = i.ToString();
                        string masterID = resultORCList[i].Result_Master_ID.ToString();
                        resultORCList[i].Result_Master_ID = ResultMasterAfterPatientMapping.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0]; //resultMasterList[masterID].Id;
                        orcInsertList.Add(resultORCList[i]);
                    }
                    ResultORCManager ORCMgr = new ResultORCManager();
                    iResult = ORCMgr.SaveResultORC(ref orcInsertList, MySession, macAddress);
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

                    for (int i = 0; i < ilstErrorLog.Count; i++)
                    {


                        ilstErrorLog[i].Result_Master_ID = ResultMasterAfterPatientMapping[Convert.ToInt32(ilstErrorLog[i].Result_Master_ID)].Id;

                        IList<ResultORC> CheckORCList = orcInsertList.Where(a => a.Result_Master_ID == ilstErrorLog[i].Result_Master_ID).ToList<ResultORC>();

                        if (CheckORCList.Count > 0)
                        {
                            IList<PhysicianLibrary> CheckPhylist = objPhyLibraryManager.GetPhysicianByNPI(CheckORCList[0].ORC_Ordering_Provider_ID);
                            if (CheckPhylist.Count > 0)
                            {
                                //IList<User> checkOwnerDetail = objUserManager.GetUserbyPhysicianLibraryID(CheckPhylist[0].Id);
                                IList<User> checkOwnerDetail = new List<User>();
                                IList<User> tempUser = new List<User>();
                                foreach (PhysicianLibrary CheckPhy in CheckPhylist)
                                {
                                    tempUser = objUserManager.GetUserbyPhysicianLibraryID(CheckPhy.Id);
                                    if (tempUser.Count > 0 && tempUser[0].Legal_Org.ToUpper() == "CMG")
                                    {
                                        checkOwnerDetail.Add(tempUser[0]);
                                    }
                                }
                                if (checkOwnerDetail.Count == 0)
                                {
                                    ilstErrorLog[i].Reason_Code = "ACUR_LAB_05";
                                    if (ilstStaticLookup.Count > 0)
                                    {
                                        StaticLookup objStaticLookup = new StaticLookup();
                                        objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_05").SingleOrDefault();
                                        ilstErrorLog[i].Reason_Description = objStaticLookup.Description;
                                    }
                                }



                            }
                            else
                            {
                                ilstErrorLog[i].Reason_Code = "ACUR_LAB_06";
                                if (ilstStaticLookup.Count > 0)
                                {
                                    StaticLookup objStaticLookup = new StaticLookup();
                                    objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_06").SingleOrDefault();
                                    ilstErrorLog[i].Reason_Description = objStaticLookup.Description;
                                }
                            }

                        }
                    }

                    //iResult = objErrorLogManager.SaveUpdateDeleteWithoutTransaction(ref ilstErrorLog, null, null, MySession, macAddress);
                    IList<ErrorLog> ilstErrorLognull = null;
                    iResult = objErrorLogManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilstErrorLog, ref ilstErrorLognull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);
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
                }


                ResultORCAfterInsert = orcInsertList;

                //IList<ResultSPM> spmInsertList = new List<ResultSPM>();
                //if (resultSPMList != null && resultSPMList.Count != 0)
                //{
                //    for (int i = 0; i < resultSPMList.Count; i++)
                //    {
                //        string masterID = resultSPMList[i].Result_Master_ID.ToString();
                //        resultSPMList[i].Result_Master_ID = ResultMasterAfterPatientMapping.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0]; //resultMasterList[masterID].Id;
                //        spmInsertList.Add(resultSPMList[i]);
                //    }
                //    ResultSPMManager SPMMgr = new ResultSPMManager();
                //    iResult = SPMMgr.SaveResultSPM(ref spmInsertList, MySession, macAddress);
                //    if (iResult == 2)
                //    {
                //        if (iTryCount < 5)
                //        {
                //            iTryCount++;
                //            goto TryAgain;
                //        }
                //        else
                //        {
                //            trans.Rollback();
                //            // MySession.Close();
                //            throw new Exception("Deadlock occurred. Transaction failed.");
                //        }
                //    }
                //    else if (iResult == 1)
                //    {
                //        trans.Rollback();
                //        //MySession.Close();
                //        throw new Exception("Exception occurred. Transaction failed.");
                //    }
                //}

                IList<ResultOBR> obrInsertList = new List<ResultOBR>();
                if (resultOBRList != null && resultOBRList.Count != 0)
                {

                    for (int i = 0; i < resultOBRList.Count; i++)
                    {
                        resultOBRList[i].temp_property = i.ToString();
                        string masterID = resultOBRList[i].Result_Master_ID.ToString();
                        resultOBRList[i].Result_Master_ID = ResultMasterAfterPatientMapping.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0];// resultMasterList[masterID].Id;
                        resultOBRList[i].temp_property = i.ToString();
                        obrInsertList.Add(resultOBRList[i]);

                    }

                    ResultOBRManager OBRMgr = new ResultOBRManager();
                    iResult = OBRMgr.SaveResultOBR(ref obrInsertList, MySession, macAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                IList<ResultOBX> obxInsertList = new List<ResultOBX>();
                if (resultOBXList != null && resultOBXList.Count != 0)
                {
                    for (int i = 0; i < resultOBXList.Count; i++)
                    {
                        resultOBXList[i].temp_property = i.ToString();
                        string masterID = resultOBXList[i].Result_Master_ID.ToString();
                        resultOBXList[i].Result_Master_ID = ResultMasterAfterPatientMapping.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0];// resultMasterList[masterID].Id;
                        string ObrID = resultOBXList[i].Result_OBR_ID.ToString();
                        resultOBXList[i].Result_OBR_ID = obrInsertList.Where(a => a.temp_property == ObrID).Select(a => a.Id).ToList<ulong>()[0];
                        obxInsertList.Add(resultOBXList[i]);
                    }
                    //IList<ResultOBX> objResultobx = new List<ResultOBX>();
                    //if (obxInsertList != null && obxInsertList.Count > 0)
                    //    objResultobx = obxInsertList.Where(a => a.OBX_Abnormal_Flag.Trim() != "" && a.OBX_Abnormal_Flag.Trim() != "N").ToList<ResultOBX>();

                    //if (objResultobx.Count > 0)
                    //    bIs_abnormal_obx = true;

                    ResultOBXManager OBXMgr = new ResultOBXManager();
                    iResult = OBXMgr.SaveResultOBX(ref obxInsertList, MySession, macAddress);
                    //******************************************************************************************
                    //For Bug ID 55638 (Patient Results should not include electronic reults)
                    //******************************************************************************************
                    //AcurusResultsMappingManager AcurusResultMngr = new AcurusResultsMappingManager();
                    //IList<AcurusResultsMapping> AcurusResultList = new List<AcurusResultsMapping>();

                    //IList<PatientResults> ilisPatientResults = new List<PatientResults>();
                    //PatientResults objPatientResults = new PatientResults();
                    //var resultMasterAndOrderId = (from rec in obxInsertList
                    //                              join rec2 in resultMasterList
                    //                              on rec.Result_Master_ID equals rec2.Id
                    //                              select new { rec.Result_Master_ID, rec2.Order_ID }).ToList();

                    //foreach (ResultOBX obj in obxInsertList)
                    //{
                    //    if (obj.OBX_Loinc_Identifier != string.Empty)
                    //    {
                    //        objPatientResults = new PatientResults();
                    //        var OrdersObjForThisOBX = (from rec in resultMasterAndOrderId where rec.Result_Master_ID == obj.Result_Master_ID select rec).ToList()[0];

                    //        //ICriteria critOrders = session.GetISession().CreateCriteria(typeof(Orders)).Add(Expression.Eq("Id", (from rec in resultMasterList where rec.Id == obj.Result_Master_ID select rec.Id).ToList()[0]));
                    //        //IList<Orders> IDS = critOrders.List<Orders>();
                    //        AcurusResultList = AcurusResultMngr.GetAcurusResultsMappingListForResultMaster(resultMasterList[0].Lab_ID, obj.OBX_Observation_Identifier);
                    //        if (AcurusResultList.Count > 0)
                    //        {
                    //            objPatientResults.Acurus_Result_Code = AcurusResultList[0].Acurus_Result_Code;
                    //            objPatientResults.Acurus_Result_Description = AcurusResultList[0].Acurus_Result_Description;
                    //        }
                    //        objPatientResults.Abnormal_Flags = obj.OBX_Abnormal_Flag;
                    //        objPatientResults.Created_By = obj.Created_By;
                    //        objPatientResults.Created_Date_And_Time = obj.Created_Date_And_Time;
                    //        objPatientResults.Result_Master_ID = Convert.ToUInt64(OrdersObjForThisOBX.Result_Master_ID);
                    //        objPatientResults.Encounter_ID = 0;
                    //        objPatientResults.Human_ID = ResultMasterAfterPatientMapping[0].Matching_Patient_Id;
                    //        objPatientResults.Physician_ID = 0;
                    //        objPatientResults.Loinc_Identifier = obj.OBX_Loinc_Identifier;
                    //        objPatientResults.Loinc_Observation = obj.OBX_Loinc_Observation_Text;
                    //        objPatientResults.Reference_Range = obj.OBX_Reference_Range;
                    //        objPatientResults.Units = obj.OBX_Units;
                    //        objPatientResults.Value = obj.OBX_Observation_Value;
                    //        //if (objPatientResults.Value.Length > 100)
                    //        //    continue;
                    //        objPatientResults.Results_Type = "Results";
                    //        DateTime dtpBirthdate = DateTime.MinValue;
                    //        string sBirthdate = (from rec in obrInsertList where rec.Id == obj.Result_OBR_ID select rec.OBR_Specimen_Collection_Date_And_Time).SingleOrDefault();
                    //        if (sBirthdate != string.Empty)
                    //        {
                    //            dtpBirthdate = DateTime.ParseExact(sBirthdate.Substring(0, 8), "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                    //            string value = dtpBirthdate.ToString("yyyy-mm-dd");
                    //            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                    //            dateInfo.ShortDatePattern = "yyyy-mm-dd";
                    //            DateTime validDate = Convert.ToDateTime(value, dateInfo);
                    //            objPatientResults.Captured_date_and_time = validDate;
                    //        }
                    //        ilisPatientResults.Add(objPatientResults);
                    //    }
                    //}
                   // VitalsManager objVitalsManager = new VitalsManager();
                   //// iResult = objVitalsManager.SaveUpdateDeleteWithoutTransaction(ref ilisPatientResults, null, null, MySession, macAddress);
                   // IList<PatientResults> ilisPatientResultsnull = null;
                   // iResult = objVitalsManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilisPatientResults, ref ilisPatientResultsnull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);
                   // if (iResult == 2)
                   // {
                   //     if (iTryCount < 5)
                   //     {
                   //         iTryCount++;
                   //         goto TryAgain;
                   //     }
                   //     else
                   //     {
                   //         trans.Rollback();
                   //         // MySession.Close();
                   //         throw new Exception("Deadlock occurred. Transaction failed.");
                   //     }
                   // }
                   // else if (iResult == 1)
                   // {
                   //     trans.Rollback();
                   //     // MySession.Close();
                   //     throw new Exception("Exception occurred. Transaction failed.");
                   // }
                }

                IList<ResultNTE> nteInsertList = new List<ResultNTE>();
                if (resultNTEList != null && resultNTEList.Count != 0)
                {
                    for (int i = 0; i < resultNTEList.Count; i++)
                    {
                        string masterID = resultNTEList[i].Result_Master_ID.ToString();
                        resultNTEList[i].Result_Master_ID = ResultMasterAfterPatientMapping.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0]; //resultMasterList[masterID].Id;
                        if (resultNTEList[i].Comment_Type == "ASR Comments")
                        {
                            string ObrId = resultNTEList[i].Result_OBR_ID.ToString();
                            //Jira CAP-1116
                            if (obrInsertList.Count > 0)
                            {

                                //resultNTEList[i].Result_OBR_ID = obrInsertList.Where(a => a.temp_property == ObrId).Select(a => a.Id).ToList<ulong>()[0]; // obrInsertList[ObrId].Id;
                                List<ulong> obrInsertListtemp = obrInsertList.Where(a => a.temp_property == ObrId).Select(a => a.Id).ToList<ulong>();
                                if (obrInsertListtemp.Count > 0) { resultNTEList[i].Result_OBR_ID = obrInsertListtemp[0]; }
                            }
                            string ObxId = resultNTEList[i].Result_OBX_ID.ToString();
                            //Jira CAP-1116
                            if (obxInsertList.Count > 0)
                            {
                                //resultNTEList[i].Result_OBX_ID = obxInsertList.Where(a => a.temp_property == ObxId).Select(a => a.Id).ToList<ulong>()[0]; // obxInsertList[ObxId].Id;
                                List<ulong> obxInsertListtemp = obxInsertList.Where(a => a.temp_property == ObxId).Select(a => a.Id).ToList<ulong>();
                                if (obxInsertListtemp.Count > 0) { resultNTEList[i].Result_OBX_ID = obxInsertListtemp[0]; }
                            }
                        }
                        nteInsertList.Add(resultNTEList[i]);
                    }
                    ResultNTEManager NTEMgr = new ResultNTEManager();
                    iResult = NTEMgr.SaveResultNTE(ref nteInsertList, MySession, macAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                IList<ResultZEF> ZefInsertList = new List<ResultZEF>();
                if (resultZEFList != null && resultZEFList.Count != 0)
                {
                    for (int i = 0; i < resultZEFList.Count; i++)
                    {
                        string masterID = resultZEFList[i].Result_Master_ID.ToString();
                        resultZEFList[i].Result_Master_ID = resultMasterList.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0]; // resultMasterList[masterID].Id;
                        ZefInsertList.Add(resultZEFList[i]);
                    }
                    ResultZEFManager ZEFMgr = new ResultZEFManager();
                    iResult = ZEFMgr.SaveResultZEF(ref ZefInsertList, MySession, macAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }

                IList<ResultZPS> ZpsInsertList = new List<ResultZPS>();
                if (resultZPSList != null && resultZPSList.Count != 0)
                {
                    for (int i = 0; i < resultZPSList.Count; i++)
                    {
                        string masterID = resultZPSList[i].Result_Master_ID.ToString();
                        resultZPSList[i].Result_Master_ID = resultMasterList.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0]; // resultMasterList[masterID].Id;
                        ZpsInsertList.Add(resultZPSList[i]);
                    }
                    ResultZPSManager ZPSMgr = new ResultZPSManager();
                    iResult = ZPSMgr.SaveResultZPS(ref ZpsInsertList, MySession, macAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }

                //muthusamy on 3-Dec-2014 LabChanges - entry for the results received from sach in wf_object or in Error log tables 

                IList<ulong> UnmatchedControlIds = new List<ulong>();
                for (int i = 0; i < ResultMasterAfterPatientMapping.Count; i++)
                {

                    if (Convert.ToUInt64(ResultMasterAfterPatientMapping[i].Order_ID) == 0 && Convert.ToUInt64(ResultMasterAfterPatientMapping[i].Matching_Patient_Id) != 0)
                    {
                        UnmatchedControlIds.Add(ResultMasterAfterPatientMapping[i].Id);
                    }



                }
                //UserManager objUserManager = new UserManager();
                //PhysicianManager objPhyLibraryManager = new PhysicianManager();
                // IList<User> User = objUserManager.GetAll();

                if (UnmatchedControlIds.Count > 0)
                {


                    IList<ResultORC> resultOrcCheckList = (from s in ResultORCAfterInsert
                                                           where UnmatchedControlIds.Contains(s.Result_Master_ID)
                                                           select s).ToList<ResultORC>();


                    //IList<PhysicianLibrary> phyLibraryList = objPhyLibraryManager.GetAll();


                    if (resultOrcCheckList.Count > 0)
                    {
                        for (int i = 0; i < resultOrcCheckList.Count; i++)
                        {
                            IList<ResultMaster> FinalResultList = new List<ResultMaster>();
                            IList<PhysicianLibrary> OwnerDetailList = objPhyLibraryManager.GetPhysicianByNPI(resultOrcCheckList[i].ORC_Ordering_Provider_ID);// (from p in phyLibraryList
                            //where p.PhyNPI == resultOrcCheckList[i].ORC_Ordering_Provider_ID
                            //select p).ToList<PhysicianLibrary>();
                            if (OwnerDetailList.Count > 0)
                            {
                                //IList<User> OwnerDetailUser = objUserManager.GetUserbyPhysicianLibraryID(OwnerDetailList[0].Id);
                                IList<User> OwnerDetailUser = new List<User>();
                                IList<User> tempUser = new List<User>();
                                foreach (PhysicianLibrary CheckPhy in OwnerDetailList)
                                {
                                    tempUser = objUserManager.GetUserbyPhysicianLibraryID(CheckPhy.Id);
                                    if (tempUser.Count > 0 && tempUser[0].Legal_Org.ToUpper() == "CMG")
                                    {
                                        OwnerDetailUser.Add(tempUser[0]);
                                    }
                                }
                                //(from u in User
                                // where u.Physician_Library_ID == OwnerDetailList[0].Id
                                // select u).ToList<User>();

                                if (OwnerDetailUser.Count > 0)
                                {

                                    WFObject objWFObject = new WFObject();
                                    objWFObject.Obj_System_Id = resultOrcCheckList[i].Result_Master_ID;
                                    objWFObject.Obj_Type = "DIAGNOSTIC_RESULT";
                                    objWFObject.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    objWFObject.Current_Process = "START";
                                    objWFObject.Current_Owner = OwnerDetailUser[0].user_name;
                                    objWFObject.Fac_Name = "ALL";
                                    if (obxInsertList != null && obxInsertList.Count > 0)
                                    {
                                        IList<ResultOBX> objResultobx = new List<ResultOBX>();
                                        objResultobx = obxInsertList.Where(a => a.OBX_Abnormal_Flag.Trim() != "" && a.OBX_Abnormal_Flag.Trim() != "N" && a.Result_Master_ID == resultOrcCheckList[i].Result_Master_ID).ToList<ResultOBX>();
                                        if (objResultobx.Count>0)
                                            objWFObject.Is_Abnormal = "Yes";
                                    }
                                    //if (bIs_abnormal_obx == true)
                                    //    objWFObject.Is_Abnormal = "Yes";
                                    objWFObjectManager.InsertToWorkFlowObject(objWFObject, 1, string.Empty, MySession);
                                }
                                else
                                {
                                    FinalResultList = ResultMasterAfterPatientMapping.Where(a => a.Id == resultOrcCheckList[i].Result_Master_ID).ToList<ResultMaster>();
                                    ErrorLog objErrorLog = new ErrorLog();
                                    objErrorLog.File_Name = FinalResultList[0].File_Name;
                                    objErrorLog.Created_By = "ACURUS";
                                    objErrorLog.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    objErrorLog.Lab_ID = FinalResultList[0].Lab_ID;
                                    objErrorLog.Reason_Code = "ACUR_LAB_05";
                                    if (ilstStaticLookup.Count > 0)
                                    {
                                        StaticLookup objStaticLookup = new StaticLookup();
                                        objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_05").SingleOrDefault();
                                        objErrorLog.Reason_Description = objStaticLookup.Description;
                                    }
                                    objErrorLog.Patient_First_Name = FinalResultList[0].PID_Patient_First_Name;
                                    objErrorLog.Patient_Last_Name = FinalResultList[0].PID_Patient_Last_Name;
                                    objErrorLog.Patient_MI_Name = FinalResultList[0].PID_Patient_Middle_Name;
                                    objErrorLog.Patient_Gender = FinalResultList[0].PID_Patient_Gender;
                                    DateTime dtpBirthdate = DateTime.MinValue;
                                    string sBirthdate = FinalResultList[0].PID_Patient_Date_Of_Birth;
                                    DateTime validDate = DateTime.MinValue;
                                    if (FinalResultList[0].PID_Patient_Date_Of_Birth != string.Empty)
                                    {
                                        if (sBirthdate.Trim() != string.Empty)
                                            dtpBirthdate = DateTime.ParseExact(sBirthdate, "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                                        string value = dtpBirthdate.ToString("yyyy-mm-dd");
                                        System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                                        dateInfo.ShortDatePattern = "yyyy-mm-dd";
                                        validDate = Convert.ToDateTime(value, dateInfo);
                                    }
                                    objErrorLog.Patient_DOB = validDate;
                                    objErrorLog.Result_Master_ID = resultOrcCheckList[i].Result_Master_ID;
                                    objErrorLog.Is_Deleted = "N";
                                    ilstErrorLog.Add(objErrorLog);

                                    IList<ErrorLog> ilstErrorLognull = null;
                                    //objErrorLogManager.SaveUpdateDeleteWithoutTransaction(ref ilstErrorLog, null, null, MySession, macAddress);
                                    objErrorLogManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilstErrorLog, ref ilstErrorLognull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);
                                }

                            }
                            else
                            {
                                FinalResultList = ResultMasterAfterPatientMapping.Where(a => a.Id == resultOrcCheckList[i].Result_Master_ID).ToList<ResultMaster>();
                                ErrorLog objErrorLog = new ErrorLog();
                                objErrorLog.File_Name = FinalResultList[0].File_Name;
                                objErrorLog.Created_By = "ACURUS";
                                objErrorLog.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                objErrorLog.Lab_ID = FinalResultList[0].Lab_ID;
                                objErrorLog.Reason_Code = "ACUR_LAB_06";
                                if (ilstStaticLookup.Count > 0)
                                {
                                    StaticLookup objStaticLookup = new StaticLookup();
                                    objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_06").SingleOrDefault();
                                    objErrorLog.Reason_Description = objStaticLookup.Description;
                                }
                                objErrorLog.Patient_First_Name = FinalResultList[0].PID_Patient_First_Name;
                                objErrorLog.Patient_Last_Name = FinalResultList[0].PID_Patient_Last_Name;
                                objErrorLog.Patient_MI_Name = FinalResultList[0].PID_Patient_Middle_Name;
                                objErrorLog.Patient_Gender = FinalResultList[0].PID_Patient_Gender;
                                DateTime dtpBirthdate = DateTime.MinValue;
                                string sBirthdate = FinalResultList[0].PID_Patient_Date_Of_Birth;
                                DateTime validDate = DateTime.MinValue;
                                if (FinalResultList[0].PID_Patient_Date_Of_Birth != string.Empty)
                                {
                                    if (sBirthdate.Trim() != string.Empty)
                                        dtpBirthdate = DateTime.ParseExact(sBirthdate, "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                                    string value = dtpBirthdate.ToString("yyyy-mm-dd");
                                    System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                                    dateInfo.ShortDatePattern = "yyyy-mm-dd";
                                    validDate = Convert.ToDateTime(value, dateInfo);
                                }
                                objErrorLog.Patient_DOB = validDate;
                                objErrorLog.Result_Master_ID = resultOrcCheckList[i].Result_Master_ID;
                                objErrorLog.Is_Deleted = "N";
                                ilstErrorLog.Add(objErrorLog);

                                IList<ErrorLog> ilstErrorLognull = null;
                                //objErrorLogManager.SaveUpdateDeleteWithoutTransaction(ref ilstErrorLog, null, null, MySession, macAddress);
                                objErrorLogManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilstErrorLog, ref ilstErrorLognull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);


                            }
                        }
                    }
                }
                //$muthusamy on 3-Dec-2014 LabChanges
                trans.Commit();
                //MySession.Flush();
                //If Current Process is Billing_Wait and Result_Process -Start  
                IList<ulong> ControlIds = new List<ulong>();
                IList<ulong> OrderIds = new List<ulong>();

                for (int i = 0; i < ResultMasterAfterPatientMapping.Count; i++)
                {
                    if (Convert.ToUInt64(ResultMasterAfterPatientMapping[i].Order_ID) != 0 && Convert.ToUInt64(ResultMasterAfterPatientMapping[i].Matching_Patient_Id) != 0)
                    {
                        ControlIds.Add(Convert.ToUInt64(ResultMasterAfterPatientMapping[i].Order_ID));
                    }

                    if (Convert.ToUInt64(ResultMasterAfterPatientMapping[i].Id) != 0 && Convert.ToUInt64(ResultMasterAfterPatientMapping[i].Id) != 0)
                    {
                        OrderIds.Add(Convert.ToUInt64(ResultMasterAfterPatientMapping[i].Id));
                    }

                }


                ControlIds = ControlIds.Distinct().ToList<ulong>();

                if (ControlIds.Count > 0)
                {

                    IList<WFObject> ilstWfObject = objWFObjectManager.GetListofObjectsByCurrentProcess("BILLING_WAIT");
                    IList<WFObject> ilstBillingWait = ((from s in ilstWfObject
                                                        where s.Obj_Type == "DIAGNOSTIC ORDER" && ControlIds.Contains(s.Obj_System_Id)
                                                        select s)).ToList<WFObject>();
                    if (ilstBillingWait.Count > 0)
                    {
                        for (int i = 0; i < ilstBillingWait.Count; i++)
                        {
                            string[] ProcessAllocation = ilstBillingWait[i].Process_Allocation.Split('|');
                            string[] ResultReview = ((from u in ProcessAllocation
                                                      where u.StartsWith("RESULT_REVIEW-")
                                                      select u).Distinct()).ToArray<string>();
                            if (ResultReview.Count() > 0)
                            {
                                for (int k = 0; k < ResultReview.Count(); k++)
                                {
                                    string sAbnormal = string.Empty;
                                    if (obxInsertList != null && obxInsertList.Count > 0)
                                    {
                                        IList<ResultOBX> objResultobx = new List<ResultOBX>();
                                        objResultobx = obxInsertList.Where(a => a.OBX_Abnormal_Flag.Trim() != "" && a.OBX_Abnormal_Flag.Trim() != "N" && OrderIds.Contains(a.Result_Master_ID) ).ToList<ResultOBX>();
                                        if (objResultobx.Count > 0)
                                            sAbnormal = "Yes";
                                    }
                                    if (sAbnormal!=string.Empty)
                                        objWFObjectManager.UpdateAbnormalFlag(ilstBillingWait[i].Obj_System_Id, "DIAGNOSTIC ORDER", sAbnormal);
                                    string[] UserName = (ResultReview[0].ToString()).Split('-');
                                    objWFObjectManager.MoveToPreviousProcess(ilstBillingWait[i].Obj_System_Id, "DIAGNOSTIC ORDER", 1, UserName[1], DateTime.Now, string.Empty);
                                }
                            }

                        }
                    }
                    IList<WFObject> ilstWfObjectResultReview = objWFObjectManager.GetListofObjectsByCurrentProcess("RESULT_PROCESS");
                    IList<WFObject> ilstResultProcess = (from s in ilstWfObjectResultReview
                                                         where s.Obj_Type == "DIAGNOSTIC ORDER" && ControlIds.Contains(s.Obj_System_Id)
                                                         select s).ToList<WFObject>();

                    if (ilstResultProcess.Count > 0)
                    {
                        for (int i = 0; i < ilstResultProcess.Count; i++)
                        {
                            IList<PhysicianLibrary> PhyLibraryList = new List<PhysicianLibrary>();
                            IList<ResultORC> ResultORCAfterList = new List<ResultORC>();
                            IList<ResultMaster> ResultMatchList = ResultMasterAfterPatientMapping.Where(a => a.Order_ID == ilstResultProcess[i].Obj_System_Id).ToList<ResultMaster>();
                            if (ResultMatchList.Count > 0)
                                ResultORCAfterList = ResultORCAfterInsert.Where(a => a.Result_Master_ID == ResultMatchList[0].Id).ToList<ResultORC>();
                            if (ResultORCAfterList.Count > 0)
                                PhyLibraryList = objPhyLibraryManager.GetPhysicianByNPI(ResultORCAfterList[0].ORC_Ordering_Provider_ID);

                            if (PhyLibraryList.Count > 0)
                            {
                                //IList<User> OwnerDetail = objUserManager.GetUserbyPhysicianLibraryID(PhyLibraryList[0].Id);
                                IList<User> OwnerDetail  = new List<User>();
                                IList<User> tempUser = new List<User>();
                                foreach (PhysicianLibrary CheckPhy in PhyLibraryList)
                                {
                                    tempUser = objUserManager.GetUserbyPhysicianLibraryID(CheckPhy.Id);
                                    if (tempUser.Count > 0 && tempUser[0].Legal_Org.ToUpper() == "CMG")
                                    {
                                        OwnerDetail.Add(tempUser[0]);
                                    }
                                }
                                if (OwnerDetail.Count() > 0)

                                {
                                    string sAbnormal = string.Empty;
                                    if (obxInsertList != null && obxInsertList.Count > 0)
                                    {
                                        IList<ResultOBX> objResultobx = new List<ResultOBX>();
                                        objResultobx = obxInsertList.Where(a => a.OBX_Abnormal_Flag.Trim() != "" && a.OBX_Abnormal_Flag.Trim() != "N" && OrderIds.Contains(a.Result_Master_ID)).ToList<ResultOBX>();
                                        if (objResultobx.Count > 0)
                                            sAbnormal = "Yes";
                                    }
                                    if (sAbnormal != string.Empty)
                                        objWFObjectManager.UpdateAbnormalFlag(ilstResultProcess[i].Obj_System_Id, "DIAGNOSTIC ORDER", sAbnormal);

                                    objWFObjectManager.MoveToNextProcess(ilstResultProcess[i].Obj_System_Id, ilstResultProcess[i].Obj_Type, 1, OwnerDetail[0].user_name, DateTime.Now, string.Empty, null, null);
                                }
                                else
                                {
                                    ErrorLog objErrorLog = new ErrorLog();
                                    objErrorLog.File_Name = ResultMatchList[0].File_Name;
                                    objErrorLog.Created_By = "ACURUS";
                                    objErrorLog.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                    objErrorLog.Lab_ID = ResultMatchList[0].Lab_ID;
                                    objErrorLog.Reason_Code = "ACUR_LAB_05";
                                    if (ilstStaticLookup.Count > 0)
                                    {
                                        StaticLookup objStaticLookup = new StaticLookup();
                                        objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_05").SingleOrDefault();
                                        objErrorLog.Reason_Description = objStaticLookup.Description;
                                    }
                                    objErrorLog.Patient_First_Name = ResultMatchList[0].PID_Patient_First_Name;
                                    objErrorLog.Patient_Last_Name = ResultMatchList[0].PID_Patient_Last_Name;
                                    objErrorLog.Patient_MI_Name = ResultMatchList[0].PID_Patient_Middle_Name;
                                    objErrorLog.Patient_Gender = ResultMatchList[0].PID_Patient_Gender;
                                    DateTime dtpBirthdate = DateTime.MinValue;
                                    string sBirthdate = ResultMatchList[0].PID_Patient_Date_Of_Birth;
                                    DateTime validDate = DateTime.MinValue;
                                    if (ResultMatchList[0].PID_Patient_Date_Of_Birth != string.Empty)
                                    {
                                        if (sBirthdate.Trim() != string.Empty)
                                            dtpBirthdate = DateTime.ParseExact(sBirthdate, "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                                        string value = dtpBirthdate.ToString("yyyy-mm-dd");
                                        System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                                        dateInfo.ShortDatePattern = "yyyy-mm-dd";
                                        validDate = Convert.ToDateTime(value, dateInfo);
                                    }
                                    objErrorLog.Patient_DOB = validDate;
                                    objErrorLog.Result_Master_ID = ResultMatchList[0].Id;
                                    objErrorLog.Is_Deleted = "N";
                                    ilstErrorLog.Add(objErrorLog);
                                    IList<ErrorLog> ilstErrorLognull = null;
                                    //objErrorLogManager.SaveUpdateDeleteWithoutTransaction(ref ilstErrorLog, null, null, MySession, macAddress);
                                    objErrorLogManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilstErrorLog, ref ilstErrorLognull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);
                                }
                            }

                            else
                            {
                                ErrorLog objErrorLog = new ErrorLog();
                                objErrorLog.File_Name = ResultMatchList[0].File_Name;
                                objErrorLog.Created_By = "ACURUS";
                                objErrorLog.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                objErrorLog.Lab_ID = ResultMatchList[0].Lab_ID;
                                objErrorLog.Reason_Code = "ACUR_LAB_06";
                                if (ilstStaticLookup.Count > 0)
                                {
                                    StaticLookup objStaticLookup = new StaticLookup();
                                    objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_06").SingleOrDefault();
                                    objErrorLog.Reason_Description = objStaticLookup.Description;
                                }
                                objErrorLog.Patient_First_Name = ResultMatchList[0].PID_Patient_First_Name;
                                objErrorLog.Patient_Last_Name = ResultMatchList[0].PID_Patient_Last_Name;
                                objErrorLog.Patient_MI_Name = ResultMatchList[0].PID_Patient_Middle_Name;
                                objErrorLog.Patient_Gender = ResultMatchList[0].PID_Patient_Gender;
                                DateTime dtpBirthdate = DateTime.MinValue;
                                string sBirthdate = ResultMatchList[0].PID_Patient_Date_Of_Birth;
                                DateTime validDate = DateTime.MinValue;
                                if (ResultMatchList[0].PID_Patient_Date_Of_Birth != string.Empty)
                                {
                                    if (sBirthdate.Trim() != string.Empty)
                                        dtpBirthdate = DateTime.ParseExact(sBirthdate, "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                                    string value = dtpBirthdate.ToString("yyyy-mm-dd");
                                    System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                                    dateInfo.ShortDatePattern = "yyyy-mm-dd";
                                    validDate = Convert.ToDateTime(value, dateInfo);
                                }
                                objErrorLog.Patient_DOB = validDate;
                                objErrorLog.Result_Master_ID = ResultMatchList[0].Id;
                                objErrorLog.Is_Deleted = "N";
                                ilstErrorLog.Add(objErrorLog);

                               // objErrorLogManager.SaveUpdateDeleteWithoutTransaction(ref ilstErrorLog, null, null, MySession, macAddress);
                                IList<ErrorLog> ilstErrorLognull = null;
                                objErrorLogManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilstErrorLog, ref ilstErrorLognull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);


                            }
                        }
                    }
                }
            }
            //If Current Process is Billing_Wait and Result_Process -End 
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                MySession.Close();
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }
            catch (Exception ex)
            {

                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.File_Name = resultMasterList[LastProcessedIndex].File_Name;
                objErrorLog.Created_By = "ACURUS";
                objErrorLog.Created_Date_And_Time = DateTime.Now;
                objErrorLog.Lab_ID = resultMasterList[LastProcessedIndex].Lab_ID;
                objErrorLog.Reason_Code = "ACUR_LAB_04";
                if (ilstStaticLookup.Count > 0)
                {
                    StaticLookup objStaticLookup = new StaticLookup();
                    objStaticLookup = ilstStaticLookup.Where(a => a.Value == "ACUR_LAB_04").SingleOrDefault();
                    objErrorLog.Reason_Description = objStaticLookup.Description;
                }
                objErrorLog.Patient_First_Name = resultMasterList[LastProcessedIndex].PID_Patient_First_Name;
                objErrorLog.Patient_Last_Name = resultMasterList[LastProcessedIndex].PID_Patient_Last_Name;
                objErrorLog.Patient_MI_Name = resultMasterList[LastProcessedIndex].PID_Patient_Middle_Name;
                objErrorLog.Patient_Gender = resultMasterList[LastProcessedIndex].PID_Patient_Gender;

                DateTime dtpBirthdate = DateTime.MinValue;
                string sBirthdate = resultMasterList[LastProcessedIndex].PID_Patient_Date_Of_Birth;
                DateTime validDate = DateTime.MinValue;
                if (resultMasterList[LastProcessedIndex].PID_Patient_Date_Of_Birth != string.Empty)
                {
                    if (sBirthdate.Trim() != string.Empty)
                        dtpBirthdate = DateTime.ParseExact(sBirthdate, "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                    string value = dtpBirthdate.ToString("yyyy-mm-dd");
                    System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                    dateInfo.ShortDatePattern = "yyyy-mm-dd";
                    validDate = Convert.ToDateTime(value, dateInfo);
                }

                objErrorLog.Patient_DOB = validDate;
                objErrorLog.Result_Master_ID = Convert.ToUInt32(LastProcessedIndex);

                //For bug Id :68705
                //objErrorLog.Modified_By = "Error Message : " + ex.Message.ToString() + Environment.NewLine + (ex.InnerException.Message != null ? "InnerException Message : " + ex.InnerException.Message.ToString() + " - " : "");
                string sModifiedby = string.Empty;
                if (ex != null)
                {
                    if (ex.Message != null)
                    {
                        sModifiedby = "Error Message : " + ex.Message.ToString() + Environment.NewLine;
                    }
                    if (ex.InnerException != null)
                    {
                        sModifiedby += "InnerException : " + ex.InnerException.ToString() + Environment.NewLine;

                        if (ex.InnerException.Message != null)
                        {
                            sModifiedby += "InnerException Message : " + ex.InnerException.Message.ToString();
                        }
                    }
                }
                objErrorLog.Modified_By = sModifiedby;
                
                ilstErrorLog.Add(objErrorLog);
                ErrorLogManager objErrorLogManager = new ErrorLogManager();
                //Commented for bug id=45811
                //objErrorLogManager.SaveUpdateDeleteWithoutTransaction(ref ilstErrorLog, null, null, MySession, macAddress);
               // objErrorLogManager.SaveUpdateDeleteWithTransaction(ref ilstErrorLog, null, null, macAddress);
                IList<ErrorLog> ilstErrorLognull = null;
                objErrorLogManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstErrorLog, ref ilstErrorLognull, null, macAddress, false, false, 0, string.Empty);
                bUnimportedFile = true;
                //trans.Rollback();//Commented for ADO transaction error
                //MySession.Close();
                //trans.Rollback();
                //MySession.Close();
                //throw new Exception(e.Message);
            }
            return bUnimportedFile;

        }


        public FillResultDTO GetResultBySubmitID(ulong Order_ID)
        {
            FillResultDTO objFillResultDto = new FillResultDTO();
            IQuery query;
            //ICriteria crit = session.GetISession().CreateCriteria(typeof(ResultMaster)).Add(Expression.Eq("Order_ID", Order_ID));
            //IList<ResultMaster> resList = crit.List<ResultMaster>();
            //objFillResultDto.ResultMasterList = resList;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (Order_ID == 0)
                {
                    query = iMySession.GetNamedQuery("Get.GetResultByZeroSubmitID");
                    query.SetString(0, Order_ID.ToString());
                }
                else
                {
                    query = iMySession.GetNamedQuery("Get.GetResultByKnownSubmitID");
                    query.SetString(0, Order_ID.ToString());
                }
                ArrayList arr = new ArrayList(query.List());
                IList<ResultMaster> resList = new List<ResultMaster>();
                if (arr != null)
                {
                    ResultOBRManager obrMgr = new ResultOBRManager();
                    IList<ResultOBR> ResultOBRList = null;
                    foreach (object[] obj in arr)
                    {
                        ResultMaster resMaster = new ResultMaster();
                        resMaster.Id = Convert.ToUInt64(obj[0]);
                        resMaster.Result_Received_Date = Convert.ToDateTime(obj[1]);
                        resMaster.Order_ID = Convert.ToUInt64(obj[2]);
                        resMaster.MSH_Segment_Type_ID = obj[3].ToString();
                        resMaster.MSH_Delimiter = obj[4].ToString();
                        resMaster.MSH_Sending_Application = obj[5].ToString();
                        resMaster.MSH_Sending_Facility = obj[6].ToString();
                        resMaster.MSH_Receiving_Application = obj[7].ToString();
                        resMaster.MSH_Receiveing_Facility = obj[8].ToString();
                        resMaster.MSH_Date_And_Time_Of_Message = obj[9].ToString();
                        resMaster.MSH_Security = obj[10].ToString();
                        resMaster.MSH_Message_Type = obj[11].ToString();
                        resMaster.MSH_Message_Control_ID = obj[12].ToString();
                        resMaster.MSH_Processing_ID = obj[13].ToString();
                        resMaster.MSH_HL7_Version = obj[14].ToString();
                        resMaster.PID_Segment_Type_ID = obj[15].ToString();
                        resMaster.PID_Sequence_Number = obj[16].ToString();
                        resMaster.PID_External_Patient_ID = obj[17].ToString();
                        resMaster.PID_Lab_Assigned_Patient_ID = obj[18].ToString();
                        resMaster.PID_Alternate_Patient_ID = obj[19].ToString();
                        resMaster.PID_Patient_Last_Name = obj[20].ToString();
                        resMaster.PID_Patient_First_Name = obj[21].ToString();
                        resMaster.PID_Patient_Middle_Name = obj[22].ToString();
                        resMaster.PID_Mother_Maiden_Name = obj[23].ToString();
                        resMaster.PID_Patient_Date_Of_Birth = obj[24].ToString();
                        resMaster.PID_Patient_Gender = obj[25].ToString();
                        resMaster.PID_Patient_Alias = obj[26].ToString();
                        resMaster.PID_Patient_Race = obj[27].ToString();
                        resMaster.PID_Patient_Address1 = obj[28].ToString();
                        resMaster.PID_Patient_Address2 = obj[29].ToString();
                        resMaster.PID_Patient_City = obj[30].ToString();
                        resMaster.PID_Patient_State = obj[31].ToString();
                        resMaster.PID_Patient_Zip = obj[32].ToString();
                        resMaster.PID_Patient_Country_Code = obj[33].ToString();
                        resMaster.PID_Patient_Home_Phone = obj[34].ToString();
                        resMaster.PID_Patient_Work_Phone = obj[35].ToString();
                        resMaster.PID_Patient_Language = obj[36].ToString();
                        resMaster.PID_Patient_Marital_Status = obj[37].ToString();
                        resMaster.PID_Patient_Religion = obj[38].ToString();
                        resMaster.PID_Labcorp_Customer_ID = obj[39].ToString();
                        resMaster.PID_Check_Digit = obj[40].ToString();
                        resMaster.PID_Check_Digit_Scheme = obj[41].ToString();
                        resMaster.PID_Bill_Code = obj[42].ToString();
                        resMaster.PID_ABN_Flag = obj[43].ToString();
                        resMaster.PID_Status_Of_Specimen = obj[44].ToString();
                        resMaster.PID_Fasting = obj[45].ToString();
                        resMaster.PID_Patient_SSN = obj[46].ToString();
                        resMaster.Created_By = obj[47].ToString();
                        resMaster.Created_Date_And_Time = Convert.ToDateTime(obj[48].ToString());
                        resMaster.Modified_By = obj[49].ToString();
                        resMaster.Modified_Date_And_Time = Convert.ToDateTime(obj[50].ToString());
                        resMaster.Version = Convert.ToInt32(obj[51].ToString());
                        resMaster.Result_Review_Comments = obj[52].ToString();
                        resMaster.MA_Notes = obj[56].ToString();
                        if (obj[53] != null && obj[53] != string.Empty)
                        {
                            resMaster.Result_Received_Date = Convert.ToDateTime(obj[53].ToString());
                        }
                        resMaster.Result_Review_By = obj[54].ToString();
                        //Added For Quest
                        ResultOBRList = obrMgr.GetResultByMasterID(resMaster.Id);
                        if (ResultOBRList.Count > 0)
                            resMaster.PID_Status_Of_Specimen = ResultOBRList.Any(a => a.OBR_Order_Result_Status == "P" || a.OBR_Order_Result_Status == "I") ? "P" : ResultOBRList.Any(a => a.OBR_Order_Result_Status == "C") ? "C" : "F";
                        if (obj[55] != null)
                            resMaster.Is_Electronic_Mode = obj[55].ToString();
                        resList.Add(resMaster);
                    }
                    IList<ulong> Order_ID_List = new List<ulong>();
                    //Latha - Main - 30 Jul 2011 - Start
                    if (Order_ID != 0)
                    {
                        Order_ID_List.Add(Order_ID);
                    }
                    //Latha - Main - 30 Jul 2011 - End
                    OrdersManager om = new OrdersManager();
                    IList<Orders> ordersList = om.GetOrdersBySubmitID(Order_ID_List);
                    if (ordersList != null && ordersList.Count > 0)
                    {
                        ulong encounterId = ordersList[0].Encounter_ID;
                        EncounterManager em = new EncounterManager();
                        Encounter enc = em.GetById(encounterId);
                        //Latha - Main - 30 Jul 2011 - Start
                        if (enc != null)
                        {
                            objFillResultDto.Encounter_Date_Of_Service = enc.Date_of_Service;//.ToString("dd-MMM-yyyy");
                        }
                        //Latha - Main - 30 Jul 2011 - End
                    }
                }
                objFillResultDto.ResultMasterList = resList;
                if (Order_ID > 0)
                {
                    if (resList != null && resList.Count > 0)
                    {
                        ResultORCManager orcMgr = new ResultORCManager();
                        objFillResultDto.ResultORCList = orcMgr.GetResultByMasterID(resList[0].Id);
                        ResultOBRManager obrMgr = new ResultOBRManager();
                        objFillResultDto.ResultOBRList = obrMgr.GetResultByMasterID(resList[0].Id);
                        ResultOBXManager obxMgr = new ResultOBXManager();
                        objFillResultDto.ResultOBXList = obxMgr.GetResultByMasterID(resList[0].Id);
                        ResultNTEManager nteMgr = new ResultNTEManager();
                        objFillResultDto.ResultNTEList = nteMgr.GetResultByMasterID(resList[0].Id);
                        ResultZEFManager zefMgr = new ResultZEFManager();
                        objFillResultDto.ResultZEFList = zefMgr.GetResultByMasterID(resList[0].Id);
                        ResultZPSManager zpsMgr = new ResultZPSManager();
                        objFillResultDto.ResultZPSList = zpsMgr.GetResultByMasterID(resList[0].Id);
                    }
                    //ResultLookupManager resultLookupMgr = new ResultLookupManager();
                    //objFillResultDto.ResultLookupList = resultLookupMgr.GetResultLookup();
                }
                if (resList != null && resList.Count > 0)
                {
                    ICriteria critOrder = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", resList[0].Order_ID));
                    IList<OrdersSubmit> ilstOrder = critOrder.List<OrdersSubmit>();
                    if (ilstOrder.Count > 0)
                        objFillResultDto.Lab_ID = ilstOrder[0].Lab_ID;
                }
                iMySession.Close();

            }
            //if (Order_ID > 0)
            //{
            //    if (resList != null && resList.Count > 0)
            //    {
            //        IList<ResultORC> ResultOrcList = new List<ResultORC>();
            //        IList<ResultOBR> ResultObrList = new List<ResultOBR>();
            //        IList<ResultOBX> ResultObxList = new List<ResultOBX>();
            //        IList<ResultNTE> ResultNteList = new List<ResultNTE>();
            //        IList<ResultZEF> ResultZefList = new List<ResultZEF>();
            //        IList<ResultZPS> ResultZpsList = new List<ResultZPS>();
            //        for (int i = 0; i < resList.Count; i++)
            //        {
            //            ResultORCManager orcMgr = new ResultORCManager();
            //            ResultOrcList = orcMgr.GetResultByMasterID(resList[i].Id);
            //            ResultOBRManager obrMgr = new ResultOBRManager();
            //            ResultObrList = obrMgr.GetResultByMasterID(resList[i].Id);
            //            ResultOBXManager obxMgr = new ResultOBXManager();
            //            ResultObxList = obxMgr.GetResultByMasterID(resList[i].Id);
            //            ResultNTEManager nteMgr = new ResultNTEManager();
            //            ResultNteList = nteMgr.GetResultByMasterID(resList[i].Id);
            //            ResultZEFManager zefMgr = new ResultZEFManager();
            //            ResultZefList = zefMgr.GetResultByMasterID(resList[i].Id);
            //            ResultZPSManager zpsMgr = new ResultZPSManager();
            //            ResultZpsList = zpsMgr.GetResultByMasterID(resList[i].Id);
            //            for (int j = 0; j < ResultOrcList.Count; j++)
            //            {
            //                objFillResultDto.ResultORCList.Add(ResultOrcList[j]);
            //            }
            //            for (int k = 0; k < ResultObrList.Count; k++)
            //            {
            //                objFillResultDto.ResultOBRList.Add(ResultObrList[k]);
            //            }
            //            for (int l = 0; l < ResultObxList.Count; l++)
            //            {
            //                objFillResultDto.ResultOBXList.Add(ResultObxList[l]);
            //            }
            //            for (int m = 0; m < ResultNteList.Count; m++)
            //            {
            //                objFillResultDto.ResultNTEList.Add(ResultNteList[m]);
            //            }
            //            for (int p = 0; p < ResultZefList.Count; p++)
            //            {
            //                objFillResultDto.ResultZEFList.Add(ResultZefList[p]);
            //            }
            //            for (int n = 0; n < ResultZpsList.Count; n++)
            //            {
            //                objFillResultDto.ResultZPSList.Add(ResultZpsList[n]);
            //            }
            //        }
            //        //ResultLookupManager resultLookupMgr = new ResultLookupManager();
            //        //objFillResultDto.ResultLookupList = resultLookupMgr.GetResultLookup();


            return objFillResultDto;
        }

        public Stream GetResultByResultMasterID(ulong result_master_id)
        {
            FillResultDTO objFillResultDto = new FillResultDTO();
            IList<ResultMaster> resList = new List<ResultMaster>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ResultMaster)).Add(Expression.Eq("Id", result_master_id));
                resList = crit.List<ResultMaster>();
                objFillResultDto.ResultMasterList = resList;
                if (resList.Count > 0)
                {
                    ResultORCManager orcMgr = new ResultORCManager();
                    objFillResultDto.ResultORCList = orcMgr.GetResultByMasterID(resList[0].Id);
                    ResultOBRManager obrMgr = new ResultOBRManager();
                    objFillResultDto.ResultOBRList = obrMgr.GetResultByMasterID(resList[0].Id);
                    if (objFillResultDto.ResultOBRList.Count > 0)
                        objFillResultDto.ResultMasterList[0].PID_Status_Of_Specimen = objFillResultDto.ResultOBRList.Any(a => a.OBR_Order_Result_Status == "P" || a.OBR_Order_Result_Status == "I") ? "P" : "F";
                    ResultOBXManager obxMgr = new ResultOBXManager();
                    objFillResultDto.ResultOBXList = obxMgr.GetResultByMasterID(resList[0].Id);

                    ResultNTEManager nteMgr = new ResultNTEManager();
                    objFillResultDto.ResultNTEList = nteMgr.GetResultByMasterID(resList[0].Id);
                    ResultZEFManager zefMgr = new ResultZEFManager();
                    objFillResultDto.ResultZEFList = zefMgr.GetResultByMasterID(resList[0].Id);
                    ResultZPSManager zpsMgr = new ResultZPSManager();
                    objFillResultDto.ResultZPSList = zpsMgr.GetResultByMasterID(resList[0].Id);

                    //sara
                    objFillResultDto.ResultSPMList = GetResultByMasterIDforSPM(resList[0].Id);


                    ICriteria critOrder = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", resList[0].Order_ID));
                    IList<OrdersSubmit> ilstOrder = critOrder.List<OrdersSubmit>();
                    if (ilstOrder.Count > 0)
                    {
                        objFillResultDto.Lab_ID = ilstOrder[0].Lab_ID;
                        if (ilstOrder[0].Encounter_ID > 0)
                        {
                            ICriteria objEnc = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", ilstOrder[0].Encounter_ID));

                            if (objEnc.List<Encounter>().Count == 0)
                            {
                                objFillResultDto.Encounter_Date_Of_Service = iMySession.CreateSQLQuery(@"SELECT  E.* FROM Encounter_arc E WHERE E.Encounter_ID = " + ilstOrder[0].Encounter_ID).AddEntity("E", typeof(Encounter)).List<Encounter>().FirstOrDefault().Date_of_Service;
                            }
                            else
                            {
                                objFillResultDto.Encounter_Date_Of_Service = objEnc.List<Encounter>()[0].Date_of_Service;
                            }
                        }
                    }
                }
                iMySession.Close();
            }
            //ResultLookupManager resultLookupMgr = new ResultLookupManager();
            //objFillResultDto.ResultLookupList = resultLookupMgr.GetResultLookup();
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            serializer.WriteObject(stream, objFillResultDto);
            stream.Seek(0L, SeekOrigin.Begin);
            return stream;
        }

        //sarav
        public IList<ResultSPM> GetResultByMasterIDforSPM(ulong result_master_id)
        {
            //ICriteria crit = session.GetISession().CreateCriteria(typeof(ResultSPM)).Add(Expression.Eq("Result_Master_ID", result_master_id));
            //return crit.List<ResultSPM>();


            IList<ResultSPM> ilstSPM = new List<ResultSPM>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select s.* from result_spm s where s.Result_Master_ID='" + result_master_id + "'").AddEntity("s", typeof(ResultSPM));
                ilstSPM = sql.List<ResultSPM>();
                iMySession.Close();
            }
            return ilstSPM;


        }

        public FillResultDTO UpdateResultMasterWithOrderSubmitID(ResultMaster ResultMasterToUpdate, string macAddress)
        {
            IList<ResultMaster> ResultMasterListToUpdate = new List<ResultMaster>();
            ResultMasterListToUpdate.Add(ResultMasterToUpdate);
            IList<ResultMaster> ResultMasterListToInsert = null;
            //SaveUpdateDeleteWithTransaction(ref ResultMasterListToInsert, ResultMasterListToUpdate, null, macAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ResultMasterListToInsert, ref ResultMasterListToUpdate, null, macAddress, false, false, 0, string.Empty);
            return GetResultBySubmitID(0);
        }

        public IList<ResultMaster> GetHumanID(ulong ulHumanID)
        {
            IList<ResultMaster> ilstResultMaster = new List<ResultMaster>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ResultMaster)).Add(Expression.Eq("PID_External_Patient_ID", ulHumanID.ToString()));
                ilstResultMaster = crit.List<ResultMaster>();
                iMySession.Close();
            }
            return ilstResultMaster;
        }

        public FillResultMasterAndEntry GetResultMasterIdUsingHumanId(ulong human_id)
        {
            FillResultMasterAndEntry ObjFillResultMasterAndEntry = new FillResultMasterAndEntry();
            IList<ResultMaster> resMasterList = new List<ResultMaster>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.GetResultMasterIdUsingHumanIdInResultMaster");
                query.SetString(0, human_id.ToString());
                ArrayList arr = new ArrayList(query.List());
                if (arr != null)
                {
                    foreach (object[] obj in arr)
                    {
                        if (obj[3].ToString() != "")
                        {
                            ResultMaster resMaster = new ResultMaster();
                            resMaster.Id = Convert.ToUInt64(obj[0]);
                            resMaster.Order_ID = Convert.ToUInt64(obj[1]);
                            resMaster.Created_Date_And_Time = Convert.ToDateTime(obj[2]);
                            resMaster.MSH_Date_And_Time_Of_Message = Convert.ToString(obj[3]);
                            // This is added to display the tool tip in Patient Chart.
                            //IQuery query1 = session.GetISession().GetNamedQuery("Get.OrderCodeDetailFromOBR");
                            //query1.SetString(0, resMaster.Id.ToString());
                            //ArrayList arr1 = new ArrayList(query1.List());
                            //foreach (object obj1 in arr1)
                            //{
                            resMaster.temp_property = obj[4].ToString();
                            //}
                            resMasterList.Add(resMaster);
                        }
                    }
                }
                iMySession.Close();
            }
            if (resMasterList != null && resMasterList.Count > 0)
            {
                ObjFillResultMasterAndEntry.Result_Master_List = resMasterList;
            }

            return ObjFillResultMasterAndEntry;
        }

        public IList<FillResultMasterAndEntry> GetResultMasterAndEntryFromEncounterID(ulong encounter_ID)
        {
            FillResultMasterAndEntry ObjFillResultMasterAndEntry = null;
            IList<FillResultMasterAndEntry> FillResultMasterAndEntryList = new List<FillResultMasterAndEntry>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.GetResultFromEncounterIDInResultMaster");
                query.SetString(0, encounter_ID.ToString());
                ArrayList arr = new ArrayList(query.List());

                if (arr != null)
                {
                    foreach (object[] obj in arr)
                    {
                        ObjFillResultMasterAndEntry = new FillResultMasterAndEntry();
                        ObjFillResultMasterAndEntry.Order_ID = Convert.ToUInt64(obj[0]);
                        ObjFillResultMasterAndEntry.Human_ID = Convert.ToUInt64(obj[1]);
                        ObjFillResultMasterAndEntry.Result_Master_ID = Convert.ToUInt64(obj[2]);
                        ObjFillResultMasterAndEntry.Order_Code = obj[3].ToString();
                        ObjFillResultMasterAndEntry.Order_Description = obj[4].ToString();
                        ObjFillResultMasterAndEntry.Table = "RESULT MASTER";
                        ObjFillResultMasterAndEntry.Result_Master_List = null;
                        FillResultMasterAndEntryList.Add(ObjFillResultMasterAndEntry);
                    }
                }

                IQuery query1 = iMySession.GetNamedQuery("Get.GetResultFromEncounterIDInResultEntry");
                query1.SetString(0, encounter_ID.ToString());
                ArrayList arr1 = new ArrayList(query1.List());

                if (arr1 != null)
                {
                    foreach (object[] obj in arr1)
                    {
                        ObjFillResultMasterAndEntry = new FillResultMasterAndEntry();
                        ObjFillResultMasterAndEntry.Order_ID = Convert.ToUInt64(obj[0]);
                        ObjFillResultMasterAndEntry.Human_ID = Convert.ToUInt64(obj[1]);
                        ObjFillResultMasterAndEntry.Result_Master_ID = Convert.ToUInt64(obj[2]);
                        ObjFillResultMasterAndEntry.Order_Code = obj[3].ToString();
                        ObjFillResultMasterAndEntry.Order_Description = obj[4].ToString();
                        ObjFillResultMasterAndEntry.Table = "RESULT ENTRY";
                        ObjFillResultMasterAndEntry.Result_Master_List = null;
                        FillResultMasterAndEntryList.Add(ObjFillResultMasterAndEntry);
                    }
                }
                iMySession.Close();
            }
            return FillResultMasterAndEntryList;
        }

        public IList<ResultMaster> GetResultByHumanID(ulong HumanID)
        {
            IList<ResultMaster> resultList = new List<ResultMaster>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ResultMaster)).Add(Expression.Eq("PID_External_Patient_ID", HumanID.ToString()));
                resultList = crit.List<ResultMaster>();
                iMySession.Close();
            }
            return resultList;
        }

        //public ResultsReviewDTO GetResultThroughScan(ulong Order_ID)
        //{
        //    ResultsReviewDTO resRevDto = new ResultsReviewDTO();
        //    IList<ulong> ordIdList = new List<ulong>();
        //    ordIdList.Add(Order_ID);
        //    OrdersManager ordMgr = new OrdersManager();
        //    resRevDto.Orders = ordMgr.GetOrdersBySubmitID(ordIdList);
        //    if (resRevDto.Orders != null && resRevDto.Orders.Count > 0)
        //    {
        //        HumanManager humMgr = new HumanManager();
        //        resRevDto.Human_Record = humMgr.GetById(resRevDto.Orders[0].Human_ID);
        //        PhysicianManager phyMgr = new PhysicianManager();
        //        resRevDto.Physician_Record = phyMgr.GetById(resRevDto.Orders[0].Physician_ID);
        //        ResultReviewManager resRevMgr = new ResultReviewManager();
        //        //Changed by Janani on 16-Nov-11 to remove Order_ID and use Order ID.
        //        //resRevDto.Result_Review_Notes = resRevMgr.GetResultReviewNotesBasedOnOrderSubmitId(resRevDto.Orders[0].Order_ID);
        //        resRevDto.Result_Review_Notes = resRevMgr.GetResultReviewNotesBasedOnOrderSubmitId(resRevDto.Orders[0].Id);
        //        EncounterManager encMgr = new EncounterManager();
        //        if (resRevDto.Orders[0].Encounter_ID != 0)
        //        {
        //            Encounter enc = encMgr.GetById(resRevDto.Orders[0].Encounter_ID);
        //            resRevDto.Encounter_Date_Of_Service = enc.Date_of_Service.ToString("dd-MMM-yyyy");
        //        }
        //    }
        //    return resRevDto;
        //}
        public IList<ResultOBX> GetResultCodes(string sResultCode, string sResultDescription, string sLoincCode)
        {
            IList<ResultOBX> resultList = new List<ResultOBX>();
            string sResult = string.Empty;
            sResult = sResult + "(r.OBX_Observation_Identifier like'" + sResultCode + "%' and r.OBX_Observation_Text like '" + sResultDescription + "%' and r.OBX_Loinc_Identifier like '" + sLoincCode + "%')";
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select r.* from result_obx r where " + sResult + "group by r.OBX_Observation_Text").AddEntity("r", typeof(ResultOBX));
                resultList = sql.List<ResultOBX>();
                iMySession.Close();
            }
            return resultList;
        }

        public IList<ResultEntryDTO> BatchOperationsThroughResultEntry(IList<ResultMaster> ResultMasterList, IList<ResultOBR> ResultOBRList, IList<ResultORC> ResultORCList, IList<ResultOBX> ResultOBXList, IList<ResultNTE> ResultNTEList, IList<ResultZPS> ResultZPSList, IList<PatientResults> AddPatientResultsList, string sMacAddress)
        {
            iTryCount = 0;
            GenerateXml XMLObj = new GenerateXml();
        TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            ITransaction trans = null;

            try
            {
                trans = MySession.BeginTransaction();
                if (ResultMasterList != null && ResultMasterList.Count != 0)
                {
                    for (int i = 0; i < ResultMasterList.Count; i++)
                    {
                        ResultMasterList[i].temp_property = i.ToString();
                    }
                    IList<ResultMaster> ResultMasterListnull = null;
                    //iResult = SaveUpdateDeleteWithoutTransaction(ref ResultMasterList, null, null, MySession, sMacAddress);
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ResultMasterList, ref ResultMasterListnull, null, MySession, sMacAddress, false, true, 0, string.Empty, ref XMLObj);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                IList<ResultORC> orcInsertList = new List<ResultORC>();
                if (ResultORCList != null && ResultORCList.Count != 0)
                {
                    for (int i = 0; i < ResultORCList.Count; i++)
                    {
                        ResultORCList[i].temp_property = i.ToString();
                        string masterID = ResultORCList[i].Result_Master_ID.ToString();
                        ResultORCList[i].Result_Master_ID = ResultMasterList.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0]; //resultMasterList[masterID].Id;
                        orcInsertList.Add(ResultORCList[i]);
                    }
                    ResultORCManager ORCMgr = new ResultORCManager();
                    iResult = ORCMgr.SaveResultORC(ref orcInsertList, MySession, sMacAddress);
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
                IList<ResultOBR> obrInsertList = new List<ResultOBR>();
                if (ResultOBRList != null && ResultOBRList.Count != 0)
                {
                    for (int i = 0; i < ResultOBRList.Count; i++)
                    {
                        ResultOBRList[i].temp_property = i.ToString();
                        string masterID = ResultOBRList[i].Result_Master_ID.ToString();
                        ResultOBRList[i].Result_Master_ID = ResultMasterList.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0];// resultMasterList[masterID].Id;
                        ResultOBRList[i].temp_property = i.ToString();
                        obrInsertList.Add(ResultOBRList[i]);

                    }
                    ResultOBRManager OBRMgr = new ResultOBRManager();
                    iResult = OBRMgr.SaveResultOBR(ref obrInsertList, MySession, sMacAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                IList<ResultOBX> obxInsertList = new List<ResultOBX>();
                if (ResultOBXList != null && ResultOBXList.Count != 0)
                {

                    for (int i = 0; i < ResultOBXList.Count; i++)
                    {
                        ResultOBXList[i].temp_property = i.ToString();
                        string masterID = ResultOBXList[i].Result_Master_ID.ToString();
                        ResultOBXList[i].Result_Master_ID = ResultMasterList.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0];// resultMasterList[masterID].Id;
                        string ObrID = ResultOBXList[i].Result_OBR_ID.ToString();
                        ResultOBXList[i].Result_OBR_ID = obrInsertList.Where(a => a.temp_property == ObrID).Select(a => a.Id).ToList<ulong>()[0];
                        obxInsertList.Add(ResultOBXList[i]);
                    }

                    ResultOBXManager OBXMgr = new ResultOBXManager();
                    iResult = OBXMgr.SaveResultOBX(ref obxInsertList, MySession, sMacAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }

                    IList<PatientResults> lstPatientResultsAdd = new List<PatientResults>();
                    for (int k = 0; k < obxInsertList.Count; k++)
                    {
                        PatientResults objPatientResults = AddPatientResultsList[k];
                        objPatientResults.Result_Master_ID = obxInsertList[k].Result_Master_ID;
                        objPatientResults.Result_OBX_ID = obxInsertList[k].Id;
                        lstPatientResultsAdd.Add(objPatientResults);
                    }
                    if (lstPatientResultsAdd != null && lstPatientResultsAdd.Count != 0)
                    {
                        VitalsManager objVitalsManager = new VitalsManager();
                        IList<PatientResults> lstPatientResultsAddnull = null;
                        iResult = objVitalsManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstPatientResultsAdd, ref lstPatientResultsAddnull, null, MySession, sMacAddress, false, true, 0, string.Empty, ref XMLObj);
                        //iResult = objVitalsManager.SaveUpdateDeleteWithoutTransaction(ref lstPatientResultsAdd, null, null, MySession, sMacAddress);
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
                            // MySession.Close();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }
                    }
                }
                IList<ResultNTE> nteInsertList = new List<ResultNTE>();
                if (ResultNTEList != null && ResultNTEList.Count != 0)
                {

                    for (int i = 0; i < ResultNTEList.Count; i++)
                    {

                        string masterID = ResultNTEList[i].Result_Master_ID.ToString();
                        ResultNTEList[i].Result_Master_ID = ResultMasterList.Where(a => a.temp_property == masterID).Select(a => a.Id).ToList<ulong>()[0]; //resultMasterList[masterID].Id;
                        if (ResultNTEList[i].Comment_Type == "ASR Comments")
                        {
                            string ObrId = ResultNTEList[i].Result_OBR_ID.ToString();
                            ResultNTEList[i].Result_OBR_ID = obrInsertList.Where(a => a.temp_property == ObrId).Select(a => a.Id).ToList<ulong>()[0]; // obrInsertList[ObrId].Id;
                            string ObxId = ResultNTEList[i].Result_OBX_ID.ToString();
                            ResultNTEList[i].Result_OBX_ID = obxInsertList.Where(a => a.temp_property == ObxId).Select(a => a.Id).ToList<ulong>()[0]; // obxInsertList[ObxId].Id;
                        }
                        nteInsertList.Add(ResultNTEList[i]);

                    }


                    ResultNTEManager NTEMgr = new ResultNTEManager();
                    iResult = NTEMgr.SaveResultNTE(ref nteInsertList, MySession, sMacAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (ResultZPSList != null && ResultZPSList.Count != 0)
                {
                    for (int i = 0; i < ResultZPSList.Count; i++)
                    {
                        ResultZPSList[i].Result_Master_ID = ResultMasterList[0].Id;
                    }
                    ResultZPSManager ZPSMgr = new ResultZPSManager();
                    iResult = ZPSMgr.SaveResultZPS(ref ResultZPSList, MySession, sMacAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }

                //MySession.Flush();
                trans.Commit();
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                //MySession.Close();
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }
            catch (Exception e)
            {
                trans.Rollback();
                // MySession.Close();
                //CAP-1942
                throw new Exception(e.Message,e);
            }
            finally
            {
                MySession.Close();
            }
            return GetResultEntry(Convert.ToUInt64(ResultMasterList[0].PID_Alternate_Patient_ID), string.Empty);
        }

        public IList<ResultEntryDTO> GetResultEntry(ulong HumanID, string From)
        {
            ResultEntryDTO ResultDTO = new ResultEntryDTO();
            string CreatedDateTime = string.Empty;
            string sPanelProcedure = string.Empty;
            IList<ResultEntryDTO> ResultEntryLst = new List<ResultEntryDTO>();
            string LabProcedure = string.Empty;
            string OrdersListId = string.Empty;
            string OrderListId = string.Empty;
            ResultMasterManager resultMasterMngr = new ResultMasterManager();
            OrdersManager orderMngr = new OrdersManager();
            IList<OrdersSubmit> OrdrLst = new List<OrdersSubmit>();
            IList<OrdersSubmit> OrdersList = new List<OrdersSubmit>();
            IList<ResultMaster> resultMasterList = new List<ResultMaster>();
            ResultMaster objResultMaster = new ResultMaster();
            IList<ResultMaster> lstResultMaster = new List<ResultMaster>();
            OrdersSubmit objOrdersSubmit = new OrdersSubmit();
            IList<OrdersSubmit> lstOrdersSubmit = new List<OrdersSubmit>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ISQLQuery sqlMainQ = iMySession.CreateSQLQuery("select {s.*},{m.*} from orders_submit s left join result_master m on (s.order_submit_id=m.order_id ) inner join wf_object w on (w.obj_system_id=s.order_submit_id) where s.Human_Id= '" + HumanID + "' and ((m.is_filled is null or m.is_filled='n' or m.is_filled='') and (m.Is_Electronic_Mode is null or m.Is_Electronic_Mode='n' or m.Is_Electronic_Mode='')) and (w.Current_Process not in ('DELETED_ORDER','MA_REVIEW','BILLING_WAIT') and w.obj_type='DIAGNOSTIC ORDER') order by s.Specimen_Collection_Date_And_Time desc;").AddEntity("s", typeof(OrdersSubmit)).AddEntity("m", typeof(ResultMaster));

                foreach (IList<object> l in sqlMainQ.List())
                {
                    objOrdersSubmit = (OrdersSubmit)l[0];
                    lstOrdersSubmit.Add(objOrdersSubmit);
                    objResultMaster = (ResultMaster)l[1];
                    if (objResultMaster != null)
                    {
                        lstResultMaster.Add(objResultMaster);
                    }
                }
                DateTime minDate = DateTime.MinValue;
                OrdrLst = (from s in lstOrdersSubmit where s.Specimen_Collection_Date_And_Time == minDate select s).OrderByDescending(t => t.Specimen_Collection_Date_And_Time).ToList<OrdersSubmit>();
                OrdersList = (from s in lstOrdersSubmit where s.Specimen_Collection_Date_And_Time != minDate select s).OrderByDescending(t => t.Specimen_Collection_Date_And_Time).ToList<OrdersSubmit>();
                resultMasterList = (from r in lstResultMaster where r.Is_Filled != "Y" select r).ToList<ResultMaster>();
                for (int j = 0; j < resultMasterList.Count; j++)
                {
                    ResultDTO.ResultMasterList.Add(resultMasterList[j]);
                }

                if (OrdersList != null && OrdersList.Count > 0)
                {
                    for (int i = 0; i < OrdersList.Count; i++)
                    {
                        if (OrdersListId == string.Empty)
                        {
                            OrdersListId = OrdersList[i].Id.ToString();
                        }
                        else
                        {
                            OrdersListId += "," + OrdersList[i].Id.ToString();
                        }
                        ResultDTO.OrderSubmit.Add(OrdersList[i]);
                    }

                }
                if (OrdrLst != null && OrdrLst.Count > 0)
                {
                    for (int i = 0; i < OrdrLst.Count; i++)
                    {
                        if (OrdersListId == string.Empty)
                        {
                            OrdersListId = OrdrLst[i].Id.ToString();
                        }
                        else
                        {
                            OrdersListId += "," + OrdrLst[i].Id.ToString();
                        }
                        ResultDTO.OrderSubmit.Add(OrdrLst[i]);
                    }


                }
                if (OrdersListId != string.Empty)
                {
                    IList<Orders> ordersList = orderMngr.GetListOfOrdersForMREstring(OrdersListId);
                    for (int j = 0; j < ordersList.Count; j++)
                    {
                        ResultDTO.Orders.Add(ordersList[j]);
                    }
                }
                ResultOBR objResultOBR = new ResultOBR();
                ResultZPS objResultZPS = new ResultZPS();
                IList<ResultOBR> resultObrList = new List<ResultOBR>();
                IList<ResultZPS> resultZpsList = new List<ResultZPS>();

                ISQLQuery sqlquery = iMySession.CreateSQLQuery("select o.*,z.* from result_master r,result_obr o,result_zps z where r.PID_External_Patient_ID ='" + HumanID + "' and r.result_master_id=o.result_master_id and r.result_master_id=z.result_master_id and order_id=0 and r.MSH_Segment_Type_ID = '' and r.Is_Filled<>'Y' order by o.OBR_Specimen_Collection_Date_And_Time desc").AddEntity("o", typeof(ResultOBR)).AddEntity("z", typeof(ResultZPS));
                foreach (IList<object> l in sqlquery.List())
                {
                    objResultOBR = (ResultOBR)l[0];
                    resultObrList.Add(objResultOBR);
                    objResultZPS = (ResultZPS)l[1];
                    resultZpsList.Add(objResultZPS);
                }

                ResultDTO.ResultOBRList = resultObrList;
                ResultDTO.ResultZPSList = resultZpsList;

                if (From == "Load")
                {
                    IList<string> StaticList = new List<string>();
                    StaticList.Add("Results|Laboratory");
                    StaticList.Add("Results|Biopsy Report");

                    StringBuilder strDocType = new StringBuilder();
                    StringBuilder strDocsubType = new StringBuilder();
                    string sDocType = string.Empty;
                    string sDocSubType = string.Empty;
                    for (int i = 0; i < StaticList.Count; i++)
                    {
                        strDocType.Append(',');
                        strDocType.Append('\'' + StaticList[i].Split('|')[0] + '\'');

                        strDocsubType.Append(',');
                        strDocsubType.Append('\'' + StaticList[i].Split('|')[1] + '\'');
                    }
                    sDocType = strDocType.ToString().Substring(1);
                    sDocSubType = strDocsubType.ToString().Substring(1);
                    FileManagementIndexManager IndexManager = new FileManagementIndexManager();
                    ResultDTO.FileManagementIndex = IndexManager.GetListOfOrdersNotTied(HumanID, sDocType, sDocSubType);
                }
                ResultEntryLst.Add(ResultDTO);
                iMySession.Close();
            }


            return ResultEntryLst;
        }

        public IList<ResultEntryDTO> DeleteOperationsThroughResultEntry(IList<ResultMaster> ResultMasterList, IList<ResultOBR> ResultOBRList, IList<ResultORC> ResultORCList, IList<ResultOBX> ResultOBXList, IList<ResultNTE> ResultNTEList, IList<ResultZPS> ResultZPSList, IList<PatientResults> DeletePatientResultsList, string sMacAddress)
        {
            iTryCount = 0;
            ulong ulHumanID = 0;
            GenerateXml XMLObj = new GenerateXml();
        TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            ITransaction trans = null;

            try
            {
                trans = MySession.BeginTransaction();
                if (ResultMasterList != null && ResultMasterList.Count != 0)
                {
                    ulHumanID = Convert.ToUInt64(ResultMasterList[0].PID_External_Patient_ID);
                    IList<ResultMaster> SaveMaster = new List<ResultMaster>();
                    //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveMaster, null, ResultMasterList, MySession, sMacAddress);
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveMaster, ref SaveMaster, ResultMasterList, MySession, sMacAddress, false, true, 0, string.Empty, ref XMLObj);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (ResultORCList != null && ResultORCList.Count > 0)
                {
                    ResultORCManager ORCMgr = new ResultORCManager();
                    iResult = ORCMgr.DeleteResultORC(ResultORCList, MySession, sMacAddress);
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
                if (ResultOBRList != null && ResultOBRList.Count != 0)
                {
                    ResultOBRManager OBRMgr = new ResultOBRManager();
                    iResult = OBRMgr.DeleteResultOBR(ResultOBRList, MySession, sMacAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (ResultOBXList != null && ResultOBXList.Count != 0)
                {
                    ResultOBXManager OBXMgr = new ResultOBXManager();
                    iResult = OBXMgr.DeleteResultOBX(ResultOBXList, MySession, sMacAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (DeletePatientResultsList != null && DeletePatientResultsList.Count != 0)
                {
                    VitalsManager objVitalsManager = new VitalsManager();
                    IList<PatientResults> AddPatientResultsList = new List<PatientResults>();
                    iResult = objVitalsManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref AddPatientResultsList, ref AddPatientResultsList, DeletePatientResultsList, MySession, sMacAddress, false, true, 0, string.Empty, ref XMLObj);
                    //iResult = objVitalsManager.SaveUpdateDeleteWithoutTransaction(ref AddPatientResultsList, null, DeletePatientResultsList, MySession, sMacAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (ResultNTEList != null && ResultNTEList.Count != 0)
                {
                    ResultNTEManager NTEMgr = new ResultNTEManager();
                    iResult = NTEMgr.DeleteResultNTE(ResultNTEList, MySession, sMacAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (ResultZPSList != null && ResultZPSList.Count != 0)
                {
                    ResultZPSManager ZPSMgr = new ResultZPSManager();
                    iResult = ZPSMgr.DeleteResultZPS(ResultZPSList, MySession, sMacAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                MySession.Flush();
                trans.Commit();
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                //MySession.Close();
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }
            catch (Exception e)
            {
                trans.Rollback();
                // MySession.Close();
                //CAP-1942
                throw new Exception(e.Message,e);
            }
            finally
            {
                MySession.Close();
            }
            return GetResultEntry(ulHumanID, string.Empty);

            #region OLD CODE
            //    iTryCount = 0;
            //    ulong ulHumanID = 0;
            //TryAgain:
            //    int iResult = 0;
            //    ISession MySession = Session.GetISession();
            //    ITransaction trans = null;

            //    try
            //    {
            //        trans = MySession.BeginTransaction();
            //        if (ResultMasterList != null && ResultMasterList.Count != 0)
            //        {
            //            ICriteria crit = session.GetISession().CreateCriteria(typeof(ResultMaster)).Add(Expression.Eq("Order_ID", ResultMasterList[0].Order_ID));
            //            ResultMasterList= crit.List<ResultMaster>();
            //            ulHumanID = Convert.ToUInt64(ResultMasterList[0].PID_External_Patient_ID);
            //            IList<ResultMaster> SaveMaster = new List<ResultMaster>();
            //            iResult = SaveUpdateDeleteWithoutTransaction(ref SaveMaster, null, ResultMasterList, MySession, sMacAddress);
            //            if (iResult == 2)
            //            {
            //                if (iTryCount < 5)
            //                {
            //                    iTryCount++;
            //                    goto TryAgain;
            //                }
            //                else
            //                {
            //                    trans.Rollback();
            //                    // MySession.Close();
            //                    throw new Exception("Deadlock occurred. Transaction failed.");
            //                }
            //            }
            //            else if (iResult == 1)
            //            {
            //                trans.Rollback();
            //                // MySession.Close();
            //                throw new Exception("Exception occurred. Transaction failed.");
            //            }
            //        }
            //        if (ResultORCList != null && ResultORCList.Count > 0)
            //        {
            //            ResultORCManager ORCMgr = new ResultORCManager();
            //            ResultORCList = new List<ResultORC>();
            //            for (int i = 0; i < ResultMasterList.Count; i++)
            //            {
            //                ICriteria crit = session.GetISession().CreateCriteria(typeof(ResultORC)).Add(Expression.Eq("Result_Master_ID", ResultMasterList[i].Id));
            //                for (int j = 0; j < crit.List<ResultORC>().Count; j++)
            //                {
            //                    ResultORCList.Add(crit.List<ResultORC>()[j]);
            //                }
            //            }
            //            iResult = ORCMgr.DeleteResultORC(ResultORCList, MySession, sMacAddress);
            //            if (iResult == 2)
            //            {
            //                if (iTryCount < 5)
            //                {
            //                    iTryCount++;
            //                    goto TryAgain;
            //                }
            //                else
            //                {
            //                    trans.Rollback();
            //                    // MySession.Close();
            //                    throw new Exception("Deadlock occurred. Transaction failed.");
            //                }
            //            }
            //            else if (iResult == 1)
            //            {
            //                trans.Rollback();
            //                //MySession.Close();
            //                throw new Exception("Exception occurred. Transaction failed.");
            //            }
            //        }
            //        if (ResultOBRList != null && ResultOBRList.Count != 0)
            //        {
            //            ResultOBRManager OBRMgr = new ResultOBRManager();
            //            ResultOBRList = new List<ResultOBR>();
            //            for (int i = 0; i < ResultMasterList.Count; i++)
            //            {

            //                ICriteria crit = session.GetISession().CreateCriteria(typeof(ResultOBR)).Add(Expression.Eq("Result_Master_ID", ResultMasterList[i].Id));
            //                for (int j = 0; j < crit.List<ResultOBR>().Count; j++)
            //                {
            //                    ResultOBRList.Add(crit.List<ResultOBR>()[j]);
            //                }

            //            }
            //            iResult = OBRMgr.DeleteResultOBR(ResultOBRList, MySession, sMacAddress);
            //            if (iResult == 2)
            //            {
            //                if (iTryCount < 5)
            //                {
            //                    iTryCount++;
            //                    goto TryAgain;
            //                }
            //                else
            //                {
            //                    trans.Rollback();
            //                    //  MySession.Close();
            //                    throw new Exception("Deadlock occurred. Transaction failed.");
            //                }
            //            }
            //            else if (iResult == 1)
            //            {
            //                trans.Rollback();
            //                // MySession.Close();
            //                throw new Exception("Exception occurred. Transaction failed.");
            //            }
            //        }
            //        if (ResultOBXList != null && ResultOBXList.Count != 0)
            //        {
            //            ResultOBXManager OBXMgr = new ResultOBXManager();
            //            ResultOBXList = new List<ResultOBX>();
            //            for (int i = 0; i < ResultMasterList.Count; i++)
            //            {
            //                ICriteria crit = session.GetISession().CreateCriteria(typeof(ResultOBX)).Add(Expression.Eq("Result_Master_ID", ResultMasterList[i].Id));
            //                for (int j = 0; j < crit.List<ResultOBX>().Count; j++)
            //                {
            //                    ResultOBXList.Add(crit.List<ResultOBX>()[j]);
            //                }
            //            }
            //            iResult = OBXMgr.DeleteResultOBX(ResultOBXList, MySession, sMacAddress);
            //            if (iResult == 2)
            //            {
            //                if (iTryCount < 5)
            //                {
            //                    iTryCount++;
            //                    goto TryAgain;
            //                }
            //                else
            //                {
            //                    trans.Rollback();
            //                    // MySession.Close();
            //                    throw new Exception("Deadlock occurred. Transaction failed.");
            //                }
            //            }
            //            else if (iResult == 1)
            //            {
            //                trans.Rollback();
            //                // MySession.Close();
            //                throw new Exception("Exception occurred. Transaction failed.");
            //            }
            //        }
            //        if (ResultNTEList != null && ResultNTEList.Count != 0)
            //        {
            //            ResultNTEManager NTEMgr = new ResultNTEManager();
            //            ResultNTEList = new List<ResultNTE>();
            //            for (int i = 0; i < ResultMasterList.Count; i++)
            //            {
            //                ICriteria crit = session.GetISession().CreateCriteria(typeof(ResultNTE)).Add(Expression.Eq("Result_Master_ID", ResultMasterList[i].Id));
            //                for (int j = 0; j < crit.List<ResultNTE>().Count; j++)
            //                {
            //                    ResultNTEList.Add(crit.List<ResultNTE>()[j]);
            //                }

            //            }
            //            iResult = NTEMgr.DeleteResultNTE(ResultNTEList, MySession, sMacAddress);
            //            if (iResult == 2)
            //            {
            //                if (iTryCount < 5)
            //                {
            //                    iTryCount++;
            //                    goto TryAgain;
            //                }
            //                else
            //                {
            //                    trans.Rollback();
            //                    // MySession.Close();
            //                    throw new Exception("Deadlock occurred. Transaction failed.");
            //                }
            //            }
            //            else if (iResult == 1)
            //            {
            //                trans.Rollback();
            //                // MySession.Close();
            //                throw new Exception("Exception occurred. Transaction failed.");
            //            }
            //        }
            //        if (ResultZPSList != null && ResultZPSList.Count != 0)
            //        {
            //            ResultZPSManager ZPSMgr = new ResultZPSManager();
            //            ResultZPSList = new List<ResultZPS>();
            //            for (int i = 0; i < ResultMasterList.Count; i++)
            //            {                     
            //                ICriteria crit = session.GetISession().CreateCriteria(typeof(ResultZPS)).Add(Expression.Eq("Result_Master_ID", ResultMasterList[i].Id));
            //                for (int j = 0; j < crit.List<ResultZPS>().Count; j++)
            //                {
            //                    ResultZPSList.Add(crit.List<ResultZPS>()[j]);
            //                }
            //            }
            //            iResult = ZPSMgr.DeleteResultZPS(ResultZPSList, MySession, sMacAddress);
            //            if (iResult == 2)
            //            {
            //                if (iTryCount < 5)
            //                {
            //                    iTryCount++;
            //                    goto TryAgain;
            //                }
            //                else
            //                {
            //                    trans.Rollback();
            //                    //MySession.Close();
            //                    throw new Exception("Deadlock occurred. Transaction failed.");
            //                }
            //            }
            //            else if (iResult == 1)
            //            {
            //                trans.Rollback();
            //                // MySession.Close();
            //                throw new Exception("Exception occurred. Transaction failed.");
            //            }
            //        }
            //        MySession.Flush();
            //        trans.Commit();
            //    }
            //    catch (NHibernate.Exceptions.GenericADOException ex)
            //    {
            //        trans.Rollback();
            //        //MySession.Close();
            //        throw new Exception(ex.Message);
            //    }
            //    catch (Exception e)
            //    {
            //        trans.Rollback();
            //        // MySession.Close();
            //        throw new Exception(e.Message);
            //    }
            //    finally
            //    {
            //        MySession.Close();
            //    }
            //    return GetResultEntry(ulHumanID);
            #endregion
        }


        #region IResultMasterManager Members


        public IList<ResultEntryDTO> UpdateOperationsThroughResultEntry(IList<ResultMaster> UpdateResultMasterList, IList<ResultOBR> UpdateResultOBRList, IList<ResultORC> UpdateResultORCList, IList<ResultOBX> SaveResultOBXList, IList<ResultOBX> UpdateResultOBXList, IList<ResultOBX> DeleteResultOBXList, IList<ResultNTE> SaveResultNTEList, IList<ResultNTE> UpdateResultNTEList, IList<ResultNTE> DeleteResultNTEList, IList<ResultZPS> UpdateResultZPSList, IList<PatientResults> AddPatientResultsList, IList<PatientResults> UpdatePatientResultsList, IList<PatientResults> DeletePatientResults, string sMacAddress)
        {
            iTryCount = 0;
            GenerateXml XMLObj = new GenerateXml();
        TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            ITransaction trans = null;

            try
            {
                trans = MySession.BeginTransaction();
                if (UpdateResultMasterList != null && UpdateResultMasterList.Count != 0)
                {
                    IList<ResultMaster> ResultMasterList = null;
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ResultMasterList, ref UpdateResultMasterList, null, MySession, sMacAddress, false, true, 0, string.Empty, ref XMLObj);
                    //iResult = SaveUpdateDeleteWithoutTransaction(ref ResultMasterList, UpdateResultMasterList, null, MySession, sMacAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (UpdateResultORCList != null && UpdateResultORCList.Count != 0)
                {
                    ResultORCManager ORCMgr = new ResultORCManager();
                    iResult = ORCMgr.UpdateResultORC(UpdateResultORCList, MySession, sMacAddress);
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
                if (UpdateResultOBRList != null && UpdateResultOBRList.Count != 0)
                {
                    ResultOBRManager OBRMgr = new ResultOBRManager();
                    iResult = OBRMgr.UpdateResultOBR(UpdateResultOBRList, MySession, sMacAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                IList<ResultOBX> obxInsertList = new List<ResultOBX>();
                IList<ResultOBX> obxUpdateList = new List<ResultOBX>();
                if (SaveResultOBXList != null && SaveResultOBXList.Count != 0 || UpdateResultOBXList != null && UpdateResultOBXList.Count != 0 || DeleteResultOBXList != null && DeleteResultOBXList.Count != 0)
                {
                    //for (int i = 0; i < SaveResultOBXList.Count; i++)
                    //{
                    //    SaveResultOBXList[i].Result_Master_ID = UpdateResultMasterList[0].Id;
                    //    SaveResultOBXList[i].Result_OBR_ID = UpdateResultOBRList[0].Id;
                    //}
                    for (int i = 0; i < SaveResultOBXList.Count; i++)
                    {
                        SaveResultOBXList[i].temp_property = i.ToString();
                        obxInsertList.Add(SaveResultOBXList[i]);
                    }

                    ResultOBXManager OBXMgr = new ResultOBXManager();
                    iResult = OBXMgr.UpdateResultOBX(ref SaveResultOBXList, UpdateResultOBXList, DeleteResultOBXList, MySession, sMacAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                IList<PatientResults> lstPatientResultsAdd = new List<PatientResults>();
                for (int k = 0; k < SaveResultOBXList.Count; k++)
                {
                    PatientResults objPatientResults = AddPatientResultsList[k];
                    //objPatientResults.Result_Master_ID = SaveResultOBXList[k].Result_Master_ID;
                    objPatientResults.Result_OBX_ID = SaveResultOBXList[k].Id;
                    lstPatientResultsAdd.Add(objPatientResults);
                }
                VitalsManager objVitalsManager = new VitalsManager();
                iResult = objVitalsManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstPatientResultsAdd, ref UpdatePatientResultsList, DeletePatientResults, MySession, sMacAddress, false, true, 0, string.Empty, ref XMLObj);
                //iResult = objVitalsManager.SaveUpdateDeleteWithoutTransaction(ref lstPatientResultsAdd, UpdatePatientResultsList, DeletePatientResults, MySession, sMacAddress);
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
                    // MySession.Close();
                    throw new Exception("Exception occurred. Transaction failed.");
                }
                IList<ResultNTE> nteInsertList = new List<ResultNTE>();
                if (SaveResultNTEList != null && SaveResultNTEList.Count != 0 || UpdateResultNTEList != null && UpdateResultNTEList.Count != 0 || DeleteResultNTEList != null && DeleteResultNTEList.Count != 0)
                {
                    //for (int i = 0; i < SaveResultNTEList.Count; i++)
                    //{
                    //    if (SaveResultNTEList[i].Comment_Type == "")
                    //    {
                    //        SaveResultNTEList[i].Comment_Type = "ASR Comments";
                    //        SaveResultNTEList[i].Result_OBR_ID = UpdateResultOBXList[0].Id;
                    //        int id = Convert.ToInt32(SaveResultNTEList[i].Result_OBX_ID);
                    //        SaveResultNTEList[i].Result_OBX_ID = SaveResultOBXList[id].Id;
                    //    }
                    //}
                    for (int i = 0; i < SaveResultNTEList.Count; i++)
                    {
                        if (SaveResultNTEList[i].Comment_Type == "ASR Comments")
                        {

                            string ObxId = SaveResultNTEList[i].Result_OBX_ID.ToString();
                            var query = from c in UpdateResultOBXList where c.Id == Convert.ToUInt64(ObxId) select c;
                            if (query.ToList<ResultOBX>().Count > 0)
                            {
                                SaveResultNTEList[i].Result_OBX_ID = Convert.ToUInt64(ObxId);
                            }
                            if (obxInsertList.Where(a => a.temp_property == ObxId).Select(a => a.Id).ToList<ulong>().Count > 0)
                            {
                                SaveResultNTEList[i].Result_OBX_ID = obxInsertList.Where(a => a.temp_property == ObxId).Select(a => a.Id).ToList<ulong>()[0];
                            }

                        }
                        nteInsertList.Add(SaveResultNTEList[i]);

                    }
                    ResultNTEManager NTEMgr = new ResultNTEManager();
                    iResult = NTEMgr.UpdateResultNTE(nteInsertList, UpdateResultNTEList, DeleteResultNTEList, MySession, sMacAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (UpdateResultZPSList != null && UpdateResultZPSList.Count != 0)
                {
                    ResultZPSManager ZPSMgr = new ResultZPSManager();
                    iResult = ZPSMgr.UpdateResultZPS(UpdateResultZPSList, MySession, sMacAddress);
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
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                //MySession.Flush();
                trans.Commit();
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }
            catch (Exception e)
            {
                trans.Rollback();
                //CAP-1942
                throw new Exception(e.Message,e);
            }
            finally
            {
                MySession.Close();
            }
            IList<ResultEntryDTO> ResultEntryDTO = new List<ResultEntryDTO>();
            if (UpdateResultMasterList.Count > 0)
                ResultEntryDTO = GetResultEntry(Convert.ToUInt64(UpdateResultMasterList[0].PID_Alternate_Patient_ID), string.Empty);
            return ResultEntryDTO;
        }


        #endregion
        // Added by Manimozhi - 5th Mar 2012- For Manual Result Entry
        public Stream GetSearchResultByLoincCodeAndDescription(string sLioncDescription, string sLioncCode, string sMacAddress)
        {
            ArrayList LoincList = null;
            IList<Loinc> searchResultList = new List<Loinc>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.LoinCodeAndDescription");
                query.SetString(0, sLioncDescription);
                query.SetString(1, sLioncCode);
                LoincList = new ArrayList(query.List());
                if (LoincList != null)
                {
                    foreach (object[] obj in LoincList)
                    {
                        Loinc loinccode = new Loinc();
                        loinccode.Long_Common_Name = obj[0].ToString();
                        loinccode.Loinc_Num = obj[1].ToString();
                        searchResultList.Add(loinccode);
                    }

                }
                iMySession.Close();
            }
            //return searchResultList;
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            serializer.WriteObject(stream, searchResultList);
            stream.Seek(0L, SeekOrigin.Begin);
            return stream;
        }

        public void SaveResultMasterThroughViewIndexedImages(IList<ResultMaster> ResultMasterList, string sMacAddress)
        {
            iTryCount = 0;
            GenerateXml XMLObj = new GenerateXml();
        TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            ITransaction trans = null;

            try
            {
                trans = MySession.BeginTransaction();
                if (ResultMasterList != null && ResultMasterList.Count != 0)
                {
                    IList<ResultMaster> ResultMasterListnull = null;
                    SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ResultMasterList, ref ResultMasterListnull, null, MySession, sMacAddress, false, true, 0, string.Empty, ref XMLObj);
                    // iResult = SaveUpdateDeleteWithoutTransaction(ref ResultMasterList, null, null, MySession, sMacAddress);
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
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }

                //MySession.Flush();
                trans.Commit();
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                //MySession.Close();
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }
            catch (Exception e)
            {
                trans.Rollback();
                // MySession.Close();
                //CAP-1942
                throw new Exception(e.Message,e);
            }
            finally
            {
                MySession.Close();
            }

        }

        public ResultMaster GetReviewCommentsForViewIndexedImages(ulong HumanId, string OrderID)
        {
            ResultMaster objResultMaster = new ResultMaster();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IList<ResultMaster> ResultMasterList = new List<ResultMaster>();
                if (OrderID.StartsWith("OrderSubmitID") == true)
                {
                    OrderID = OrderID.Replace("OrderSubmitID", "");
                    //ISQLQuery sql = iMySession.CreateSQLQuery("select r.* from result_master r where r.MSH_Date_And_Time_Of_Message = ' ' and  r.PID_Alternate_Patient_ID = '" + Convert.ToString(HumanId) + "' and r.Order_ID= '" + OrderID + "' order by created_date_and_time  desc").AddEntity("r", typeof(ResultMaster));
                    ISQLQuery sql = iMySession.CreateSQLQuery("select r.* from result_master r where r.MSH_Date_And_Time_Of_Message = ' ' and  r.PID_Alternate_Patient_ID = '" + Convert.ToString(HumanId) + "' and r.Order_ID= '" + OrderID + "' order by modified_date_and_time  desc").AddEntity("r", typeof(ResultMaster));

                    ResultMasterList = sql.List<ResultMaster>();
                    if (ResultMasterList.Count > 0)
                    {
                        objResultMaster = ResultMasterList[0];
                    }
                }
                else if (OrderID.StartsWith("ResultMasterID") == true)
                {
                    OrderID = OrderID.Replace("ResultMasterID", "");
                    //ISQLQuery sql = iMySession.CreateSQLQuery("select r.* from result_master r where r.MSH_Date_And_Time_Of_Message = ' ' and  r.PID_Alternate_Patient_ID = '" + Convert.ToString(HumanId) + "' and r.result_master_id= '" + OrderID + "' order by created_date_and_time  desc").AddEntity("r", typeof(ResultMaster));
                    ISQLQuery sql = iMySession.CreateSQLQuery("select r.* from result_master r where r.MSH_Date_And_Time_Of_Message = ' ' and  r.PID_Alternate_Patient_ID = '" + Convert.ToString(HumanId) + "' and r.result_master_id= '" + OrderID + "' order by modified_date_and_time  desc").AddEntity("r", typeof(ResultMaster));
                    ResultMasterList = sql.List<ResultMaster>();
                    if (ResultMasterList.Count > 0)
                    {
                        objResultMaster = ResultMasterList[0];
                    }
                }
                iMySession.Close();
            }
            return objResultMaster;
        }


        //CAP-2901
        public ulong GetHumanIdbyname(string sFirstname, string sLastname, string sDob, string sSex, ISession MySession)
        {
            ulong ulHumanId = 0;
            if (sDob.Length == 8)
                sDob = sDob.Insert(4, "-").Insert(7, "-");
            else if (sDob.Length == 7)
                sDob = sDob.Insert(4, "-").Insert(6, "-");

            //ArrayList aryHuman_List = null;
            //CAP-2901
            //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            //{
            //IQuery query1 = iMySession.GetNamedQuery("Get.HumanIdFromResult");
            //query1.SetString(0, sFirstname);
            //query1.SetString(1, sLastname);
            //query1.SetString(2, sDob);
            //query1.SetString(3, sSex + "%");

            //aryHuman_List = new ArrayList(query1.List());
            //if (aryHuman_List.Count > 0)
            //    ulHumanId = Convert.ToUInt32(aryHuman_List[0]);

            //iMySession.Close();


                //IList<Human> objHuman = new List<Human>();
                //ISQLQuery sql = iMySession.CreateSQLQuery("select h.*  from Human h  where h.First_Name ='" + sFirstname + "' and h.Last_Name='" + sLastname + "' and h.Birth_Date='" + sDob + "' and h.Sex like '" + sSex + "%' and h.account_status='active';").AddEntity("h", typeof(Human));
                //For bug Id: 71296
                //CAP-3071
                ISQLQuery sql = MySession.CreateSQLQuery("select h.Human_Id from Human h  where h.First_Name =:Firstname and h.Last_Name=:Lastname and h.Birth_Date=:Dob and h.Sex like '" + sSex + "%' and h.account_status='active';");
                sql.SetParameter("Firstname", sFirstname.Replace("'", "''"));
                sql.SetParameter("Lastname", sLastname.Replace("'", "''"));
                sql.SetParameter("Dob", sDob);
            // sql.SetParameter("Sex", sSex);

            //CAP-2901
            IList<object> ilstHuman = sql.List<object>();
            if (ilstHuman.Count != 0)
            {
                var objHuman = ilstHuman.FirstOrDefault();
                if (objHuman != null)
                {
                    ulong.TryParse(objHuman.ToString(), out ulHumanId);
                }
                //if (objHuman.Count > 1)
                //{
                //    objHuman = objHuman.Where(a => a.Account_Status.ToUpper() == "ACTIVE").ToList<Human>();
                //    if (objHuman.Count > 0)
                //        ulHumanId = objHuman[0].Id;
                //}
            }
            //CAP-2901
            //    iMySession.Close();
            //}
            return ulHumanId;

        }



        public IList<ResultEntryDTO> GetListOfResultsFromResultMasterID(ulong ResultMasterID, ulong ulHumanID)
        {

            ResultEntryDTO objFillResultDto = new ResultEntryDTO();
            IList<ResultEntryDTO> ResultEntryLst = new List<ResultEntryDTO>();
            ResultMaster objResultMaster = new ResultMaster();
            ResultZPS objResultZPS = new ResultZPS();
            ResultOBR objResultOBR = new ResultOBR();
            ResultOBX objResultOBX = new ResultOBX();
            ResultNTE objResultNTE = new ResultNTE();
            ResultORC objResultORC = new ResultORC();
            PatientResults objPatientResults = new PatientResults();

            IList<ResultMaster> objResultMasterList = new List<ResultMaster>();
            IList<ResultZPS> objResultZPSList = new List<ResultZPS>();
            IList<ResultOBR> objResultOBRList = new List<ResultOBR>();
            IList<ResultOBX> objResultOBXList = new List<ResultOBX>();
            IList<ResultNTE> objResultNTEList = new List<ResultNTE>();
            IList<ResultORC> objResultORCList = new List<ResultORC>();
            IList<PatientResults> objPatientResultList = new List<PatientResults>();


            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("select {r.*},{z.*},{o.*},{x.*},{n.*},{c.*},{p.*} from result_master r left join result_zps z on (r.result_master_id=z.result_master_id)"
                                                                + "left join  result_obr o on (r.result_master_id=o.result_master_id) left join  result_obx x on (r.result_master_id=x.result_master_id)"
                                                                + "left join result_nte n on (r.result_master_id=n.result_master_id) left join result_orc c on (r.result_master_id=c.result_master_id)"
                                                                + "left join Patient_Results p on (r.result_master_id=p.result_master_id)"
                                                                + "where r.result_master_id= '" + ResultMasterID + "'"
                                                                + "and p.human_id='" + ulHumanID + "' and  p.Results_Type='Manual Results'").AddEntity("r", typeof(ResultMaster)).AddEntity("z", typeof(ResultZPS)).AddEntity("o", typeof(ResultOBR)).AddEntity("x", typeof(ResultOBX)).AddEntity("n", typeof(ResultNTE)).AddEntity("c", typeof(ResultORC)).AddEntity("p", typeof(PatientResults));

                foreach (IList<object> l in sqlquery.List())
                {
                    objResultMaster = (ResultMaster)l[0];
                    if (objResultMaster != null)
                    {
                        objResultMasterList.Add(objResultMaster);
                    }
                    objResultZPS = (ResultZPS)l[1];
                    if (objResultZPS != null)
                    {
                        objResultZPSList.Add(objResultZPS);
                    }
                    objResultOBR = (ResultOBR)l[2];
                    if (objResultOBR != null)
                    {
                        objResultOBRList.Add(objResultOBR);
                    }
                    objResultOBX = (ResultOBX)l[3];
                    if (objResultOBX != null)
                    {
                        objResultOBXList.Add(objResultOBX);
                    }
                    objResultNTE = (ResultNTE)l[4];
                    if (objResultNTE != null)
                    {
                        objResultNTEList.Add(objResultNTE);
                    }
                    objResultORC = (ResultORC)l[5];
                    if (objResultORC != null)
                    {
                        objResultORCList.Add(objResultORC);
                    }
                    objPatientResults = (PatientResults)l[6];
                    if (objPatientResults != null)
                    {
                        objPatientResultList.Add(objPatientResults);
                    }

                }

                objFillResultDto.ResultMasterList = objResultMasterList.Distinct().ToList<ResultMaster>();
                objFillResultDto.ResultZPSList = objResultZPSList.Distinct().ToList<ResultZPS>();
                objFillResultDto.ResultOBRList = objResultOBRList.Distinct().ToList<ResultOBR>();
                objFillResultDto.ResultOBXList = objResultOBXList.Distinct().ToList<ResultOBX>();
                objFillResultDto.ResultNTEList = objResultNTEList.Distinct().ToList<ResultNTE>();
                objFillResultDto.ResultORCList = objResultORCList.Distinct().ToList<ResultORC>();
                objFillResultDto.PatientResultList = objPatientResultList.Distinct().ToList<PatientResults>();


                if (objFillResultDto.ResultOBRList.Count > 0)
                    objFillResultDto.ResultMasterList[0].PID_Status_Of_Specimen = objFillResultDto.ResultOBRList.Any(a => a.OBR_Order_Result_Status == "P" || a.OBR_Order_Result_Status == "I") ? "P" : "F";


                //ICriteria crit = iMySession.CreateCriteria(typeof(ResultMaster)).Add(Expression.Eq("Id", ResultMasterID));
                //IList<ResultMaster> resList = crit.List<ResultMaster>();
                //objFillResultDto.ResultMasterList = resList;
                //if (resList.Count > 0)
                //{
                //    ResultORCManager orcMgr = new ResultORCManager();
                //    objFillResultDto.ResultORCList = orcMgr.GetResultByMasterID(resList[0].Id);
                //    ResultOBRManager obrMgr = new ResultOBRManager();
                //    objFillResultDto.ResultOBRList = obrMgr.GetResultByMasterID(resList[0].Id);
                //    if (objFillResultDto.ResultOBRList.Count > 0)
                //        objFillResultDto.ResultMasterList[0].PID_Status_Of_Specimen = objFillResultDto.ResultOBRList.Any(a => a.OBR_Order_Result_Status == "P" || a.OBR_Order_Result_Status == "I") ? "P" : "F";
                //    ResultOBXManager obxMgr = new ResultOBXManager();
                //    objFillResultDto.ResultOBXList = obxMgr.GetResultByMasterID(resList[0].Id);

                //    ResultNTEManager nteMgr = new ResultNTEManager();
                //    objFillResultDto.ResultNTEList = nteMgr.GetResultByMasterID(resList[0].Id);

                //    ResultZPSManager zpsMgr = new ResultZPSManager();
                //    objFillResultDto.ResultZPSList = zpsMgr.GetResultByMasterID(resList[0].Id);

                //    ICriteria CriteriaPatientResults = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Results_Type", "Manual Results")).Add(Expression.Eq("Result_Master_ID", resList[0].Id));
                //    objFillResultDto.PatientResultList = CriteriaPatientResults.List<PatientResults>();

                //}
                iMySession.Close();
            }

            ResultEntryLst.Add(objFillResultDto);

            return ResultEntryLst;
        }
        public IList<ResultMaster> GetResultMasterListByResultmasterIDForMRE(ulong ulResultMasterID)
        {
            IList<ResultMaster> resList = new List<ResultMaster>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ResultMaster)).Add(Expression.Eq("Id", ulResultMasterID));
                resList = crit.List<ResultMaster>();
                iMySession.Close();
            }
            return resList;
        }
        public void UpdateResultMasterForOutsideOrdersForMRE(ulong resultMasterID, string sIsFilled, string sMacAddress)
        {
            ResultMasterManager ResultMasterMngr = new ResultMasterManager();
            IList<ResultMaster> ResultmasterList = ResultMasterMngr.GetResultMasterListByResultmasterIDForMRE(resultMasterID);
            ResultmasterList[0].Is_Filled = sIsFilled;
            IList<ResultMaster> UpdateResultmasterList = new List<ResultMaster>();
            UpdateResultmasterList.Add(ResultmasterList[0]);
            IList<ResultMaster> AddResultmasterList = null;
            //SaveUpdateDeleteWithTransaction(ref AddResultmasterList, UpdateResultmasterList, null, sMacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref AddResultmasterList, ref UpdateResultmasterList, null, sMacAddress, false, false, 0, string.Empty);
        }


        public FillResultMasterAndEntry GetResultUsingHumanId(ulong human_id)
        {
            FillResultMasterAndEntry ObjFillResultMasterAndEntry = new FillResultMasterAndEntry();
            IList<ResultMaster> resMasterList = new List<ResultMaster>();
            IList<Orders> lstorder = new List<Orders>();
            IList<ResultOBR> lstObr = new List<ResultOBR>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Fill.ResultMasterListByHumanID");
                query.SetString(0, human_id.ToString());
                ArrayList arr = new ArrayList(query.List());

                if (arr != null)
                {
                    foreach (object[] obj in arr)
                    {
                        if (obj[3] != null)
                        {
                            if (obj[3].ToString() != "")
                            {
                                ResultMaster resMaster = new ResultMaster();
                                resMaster = new ResultMaster();
                                resMaster.Id = Convert.ToUInt64(obj[0]);
                                resMaster.Order_ID = Convert.ToUInt64(obj[1]);
                                resMaster.Created_Date_And_Time = Convert.ToDateTime(obj[2]);
                                resMaster.OBR_Specimen_Collected_Date_And_Time = Convert.ToString(obj[3]);
                                resMaster.temp_property = obj[4].ToString();
                                if (obj[5] != null)
                                {
                                    resMaster.Order_Type = obj[5].ToString();
                                }
                                resMaster.Is_Electronic_Mode = obj[6].ToString();

                                if (Convert.ToUInt64(obj[1]) != 0)
                                {
                                    ICriteria crit = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Order_Submit_ID", Convert.ToUInt64(obj[1])));
                                    lstorder = crit.List<Orders>();
                                    if (lstorder.Count > 0)
                                    {
                                        string description = string.Empty;
                                        for (int i = 0; i < lstorder.Count; i++)
                                        {
                                            if (description == string.Empty)
                                            {
                                                description = lstorder[i].Lab_Procedure + " - " + lstorder[i].Lab_Procedure_Description;
                                            }
                                            else
                                            {
                                                description = description + "; " + lstorder[i].Lab_Procedure + " - " + lstorder[i].Lab_Procedure_Description;
                                            }
                                        }
                                        resMaster.Orders_Description = description;
                                    }
                                }
                                else
                                {
                                    ICriteria crit = iMySession.CreateCriteria(typeof(ResultOBR)).Add(Expression.Eq("Result_Master_ID", Convert.ToUInt64(obj[0])));
                                    lstObr = crit.List<ResultOBR>();
                                    if (lstObr.Count > 0)
                                    {
                                        string description = string.Empty;
                                        for (int i = 0; i < lstObr.Count; i++)
                                        {
                                            if (description == string.Empty)
                                            {
                                                description = lstObr[i].OBR_Observation_Battery_Identifier + " - " + lstObr[i].OBR_Observation_Battery_Text;
                                            }
                                            else
                                            {
                                                description = description + "; " + lstObr[i].OBR_Observation_Battery_Identifier + " - " + lstObr[i].OBR_Observation_Battery_Text;
                                            }
                                        }
                                        resMaster.Orders_Description = description;
                                    }

                                }

                                resMasterList.Add(resMaster);
                            }
                        }
                    }

                }
                iMySession.Close();

            }
            if (resMasterList.Count > 0)
            {
                ObjFillResultMasterAndEntry.Result_Master_List = resMasterList;
            }

            return ObjFillResultMasterAndEntry;
        }
        public object GenerateABNForm(string AccountNumberForLabCorp, string PatientName, string IdentificationNumber, string[] TestCodes, string[] DiagCodes)
        {
            object pdfContent = null;
            com.labcorp.www4.AbnRequestType objAbnRequestType = new com.labcorp.www4.AbnRequestType();
            objAbnRequestType.sourceName = "ABNonDemand";
            //objAbnRequestType.accountNumber = "04281070";
            objAbnRequestType.accountNumber = AccountNumberForLabCorp;
            objAbnRequestType.patientName = PatientName;
            objAbnRequestType.identificationNumber = IdentificationNumber;
            objAbnRequestType.testCodes = TestCodes;
            objAbnRequestType.diagCodes = DiagCodes;
            com.labcorp.www4.ABNServiceProxyService objABNServiceProxyService = new com.labcorp.www4.ABNServiceProxyService();
            com.labcorp.www4.AbnResponseType objAbnResponseType = objABNServiceProxyService.checkABN(objAbnRequestType, true, true, 1);
            pdfContent = (object)objAbnResponseType;
            return pdfContent;

        }
        public void GenerateABNDatas(ulong EncounterID, ulong PhysicianId, ulong HumanID, out IList<string> LabCorpCPT, out IList<string> LabCorpICD, out IList<string> QuestOrderSubmitId, out IList<OrdersAssessment> OrdersAssessmentList, out IList<Orders> QuestOrdersList, IList<OrdersAssessment> ilstOrderAssesment, IList<Orders> ilstOrders)
        {
            LabCorpCPT = new List<string>();
            LabCorpICD = new List<string>();
            QuestOrdersList = new List<Orders>();
            OrdersAssessmentList = new List<OrdersAssessment>();
            IQuery query1;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (EncounterID != 0)
                {
                    query1 = iMySession.GetNamedQuery("Orders.Submit.Id.For.ABN.With.Encounter");
                    query1.SetString(0, EncounterID.ToString());
                }
                else
                {
                    query1 = iMySession.GetNamedQuery("Orders.Submit.Id.For.ABN.Without.Encounter");
                    query1.SetString(0, EncounterID.ToString());
                    query1.SetString(1, EncounterID.ToString());
                }
                ArrayList LabCorpAndQuestList = new ArrayList(query1.List());
                IList<string> LabCorpOrderSubmitId = new List<string>();
                QuestOrderSubmitId = new List<string>();
                foreach (object[] objarr in LabCorpAndQuestList)
                {
                    if (objarr[1] != null)
                    {
                        if (objarr[1].ToString() == "1")
                        {
                            LabCorpOrderSubmitId.Add(objarr[0].ToString());
                        }
                        else
                        {
                            QuestOrderSubmitId.Add(objarr[0].ToString());
                        }
                    }
                }
                if (LabCorpOrderSubmitId.Count > 0)
                {
                    IQuery query2 = iMySession.GetNamedQuery("ABN.GetProcedure.LabCorp");
                    query2.SetParameterList("OrderSubmitList", LabCorpOrderSubmitId.ToArray<string>());
                    ArrayList objArrayList = new ArrayList(query2.List());
                    if (objArrayList != null && objArrayList.Count > 0)
                    {
                        IList<string> OrderIDLabCorp = new List<string>();
                        foreach (object[] obj in objArrayList)
                        {
                            LabCorpCPT.Add(obj[1].ToString().Trim());
                            OrderIDLabCorp.Add(obj[0].ToString());
                        }
                        IQuery query3 = iMySession.GetNamedQuery("ABN.ICD.LabCorp");
                        // query3.SetParameterList("OrderID", OrderIDLabCorp.ToArray<string>());       For Bug Id 56642 order_id to order_submit_id change from order_assemment_N
                        query3.SetParameterList("OrderSubmitList", LabCorpOrderSubmitId.ToArray<string>());
                        ArrayList objArrayListOrderAssessment = new ArrayList(query3.List());
                        foreach (object[] obj in objArrayListOrderAssessment)
                        {
                            LabCorpICD.Add(obj[1].ToString().Trim());
                        }
                    }


                }

                if (QuestOrderSubmitId.Count > 0)
                {
                    //IQuery query3 = session.GetISession().GetNamedQuery("ABN.GetProcedure.Quest");
                    //query3.SetParameterList("QuestOrderSubmitId", QuestOrderSubmitId.ToArray<string>());
                    //ArrayList objArrayList = new ArrayList(query3.List());
                    //if (objArrayList != null && objArrayList.Count > 0)
                    //{
                    //    foreach (object[] obj in objArrayList)
                    //    {
                    //        if (obj[1] != null)
                    //        {
                    //            QuestCPT += obj[1].ToString().Trim() + "\t" + obj[2].ToString().Trim() + "\n";
                    //        }

                    //    }
                    //}
                    string joinedIds = string.Join(",", QuestOrderSubmitId.ToArray<string>());

                    ISQLQuery sql = iMySession.CreateSQLQuery("select  o.* from orders o where order_submit_id in(" + joinedIds + ");").AddEntity("o", typeof(Orders));
                    QuestOrdersList = sql.List<Orders>();

                    ISQLQuery sql1 = iMySession.CreateSQLQuery("select o.* from orders_assessment o where o.order_submit_id in(" + joinedIds + ");").AddEntity("o", typeof(OrdersAssessment));
                    OrdersAssessmentList = sql1.List<OrdersAssessment>();

                }
                iMySession.Close();
            }
        }

        public IList<FillOrderException> GetOrderDetails(int PageNumber, int MaxResultsPerPage, ulong OrderID, ulong Result_Master_ID, bool Is_Showall, bool Is_Patient_Matched, ulong Human_ID, ulong Lab_ID)
        {
            IList<FillOrderException> resturnList = new List<FillOrderException>();

            IQuery query;
            int TotalNumberOfRecords = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (Is_Patient_Matched == false)
                {

                    if (PageNumber == 1 && OrderID != 0)
                    {
                        query = iMySession.GetNamedQuery("OrderDetails.No.Criteria.Count");
                        query.SetString(0, OrderID + "%");

                        ArrayList arr = new ArrayList(query.List());
                        TotalNumberOfRecords = Convert.ToInt32(arr[0]);
                    }
                    else
                    {
                        query = iMySession.GetNamedQuery("OrderDetails.ID.With.Criteria.Count");
                        query.SetString(0, Result_Master_ID + "%");

                        ArrayList arr = new ArrayList(query.List());
                        TotalNumberOfRecords = Convert.ToInt32(arr[0]);

                    }
                    IList<ulong> orderid = new List<ulong>();
                    if (OrderID != 0)
                    {
                        query = iMySession.GetNamedQuery("OrderDetails.No.Criteria");
                        PageNumber = PageNumber - 1;
                        query.SetString(0, OrderID + "%");
                        //query.SetParameterList("OrderIds", OrderID);
                        query.SetInt32(1, PageNumber * MaxResultsPerPage);
                        query.SetInt32(2, MaxResultsPerPage);
                    }
                    else
                    {

                        if (!Is_Showall)
                            query = iMySession.GetNamedQuery("OrderDetails.ID.With.Criteria");
                        else
                            query = iMySession.GetNamedQuery("OrderDetails.ID.Showall.With.Criteria");

                        query.SetString(0, Result_Master_ID + "%");
                        ArrayList arr = new ArrayList(query.List());

                        for (int i = 0; i < arr.Count; i++)
                        {
                            orderid.Add(Convert.ToUInt32(arr[i]));
                            //orderid.Add(arr[i].ToString());
                        }
                        if (orderid.Count > 0)
                        {
                            query = iMySession.GetNamedQuery("OrderDetails.With.Criteria");
                            PageNumber = PageNumber - 1;
                            int pParamenter = 0;
                            pParamenter = orderid.Count;
                            //query.SetString(0, OrderID + "%");
                            // query.SetInt32(0, PageNumber * MaxResultsPerPage);
                            // query.SetInt32(1, MaxResultsPerPage);
                            query.SetParameterList("OrderList", orderid.ToArray<ulong>());

                        }
                    }
                    ArrayList arrData = new ArrayList(query.List());
                    string[] ExpectedFormates = new string[] { "yyyymmdd" };
                    foreach (object[] obj in arrData)
                    {
                        FillOrderException objFillOrderException = new FillOrderException();
                        objFillOrderException.HumanId = Convert.ToUInt32(obj[0]);
                        objFillOrderException.Patient_Name_In_Capella = obj[1].ToString();

                        //DateTime dob = DateTime.ParseExact(obj[2].ToString(), ExpectedFormates, System.Globalization.CultureInfo.InstalledUICulture, System.Globalization.DateTimeStyles.None);
                        objFillOrderException.DOB_In_Result = Convert.ToDateTime(obj[2]);
                        string Sex = string.Empty;
                        if (obj[3].ToString().ToUpper() == "FEMALE")
                        {
                            Sex = "Female";
                        }
                        else if (obj[3].ToString() == "MALE")
                        {
                            Sex = "Male";
                        }
                        objFillOrderException.Sex_In_Capella = Sex;
                        objFillOrderException.SSN = obj[4].ToString();
                        if (obj[5].ToString() == ";")
                            objFillOrderException.Lab_Procedure = "";
                        else
                            objFillOrderException.Lab_Procedure = obj[5].ToString();
                        objFillOrderException.Provider_Name = obj[6].ToString();
                        objFillOrderException.Lab_Name = obj[7].ToString();
                        objFillOrderException.Lab_Location_Name = obj[8].ToString();
                        //DateTime orderdate = DateTime.ParseExact(obj[9].ToString(), ExpectedFormates, System.Globalization.CultureInfo.InstalledUICulture, System.Globalization.DateTimeStyles.None);
                        objFillOrderException.Order_Date = Convert.ToDateTime(obj[9]);
                        //objFillOrderException.Order_ID = Convert.ToUInt32(obj[13]);
                        objFillOrderException.TotalCount = TotalNumberOfRecords;
                        resturnList.Add(objFillOrderException);
                    }
                    iMySession.Close();
                    return resturnList;
                }
                else
                {
                    if (PageNumber == 1 && Human_ID != 0 && Is_Showall == false)
                    {
                        query = iMySession.GetNamedQuery("OrderDetails.No.Criteria.Count.ForHuman");
                        query.SetString(0, Human_ID.ToString());
                        query.SetString(1, Lab_ID.ToString());
                        ArrayList arr = new ArrayList(query.List());
                        TotalNumberOfRecords = Convert.ToInt32(arr[0]);
                    }
                    else if (PageNumber == 1 && Human_ID != 0 && Is_Showall == true)
                    {
                        query = iMySession.GetNamedQuery("OrderDetails.No.Criteria.Count.ForHuman.ShowAll");
                        query.SetString(0, Human_ID.ToString());
                        query.SetString(1, Lab_ID.ToString());
                        ArrayList arr = new ArrayList(query.List());
                        TotalNumberOfRecords = Convert.ToInt32(arr[0]);
                    }

                    IList<ulong> orderid = new List<ulong>();
                    if (Human_ID != 0 && Is_Showall == false)
                    {
                        query = iMySession.GetNamedQuery("OrderDetails.No.Criteria.ForHuman.No.ShowAll");
                        PageNumber = PageNumber - 1;
                        query.SetString(0, Human_ID.ToString());
                        query.SetString(1, Lab_ID.ToString());
                        query.SetInt32(2, PageNumber * MaxResultsPerPage);
                        query.SetInt32(3, MaxResultsPerPage);
                    }
                    //else if(Human_ID != 0 && Is_Showall)
                    else
                    {
                        query = iMySession.GetNamedQuery("OrderDetails.No.Criteria.ForHuman.With.ShowAll");
                        PageNumber = PageNumber - 1;
                        query.SetString(0, Human_ID.ToString());
                        query.SetString(1, Lab_ID.ToString());
                        query.SetInt32(2, PageNumber * MaxResultsPerPage);
                        query.SetInt32(3, MaxResultsPerPage);
                    }
                    ArrayList arrData = new ArrayList(query.List());
                    string[] ExpectedFormates = new string[] { "yyyymmdd" };
                    foreach (object[] obj in arrData)
                    {
                        FillOrderException objFillOrderException = new FillOrderException();
                        objFillOrderException.HumanId = Convert.ToUInt32(obj[0]);
                        objFillOrderException.Patient_Name_In_Capella = obj[1].ToString();

                        //DateTime dob = DateTime.ParseExact(obj[2].ToString(), ExpectedFormates, System.Globalization.CultureInfo.InstalledUICulture, System.Globalization.DateTimeStyles.None);
                        objFillOrderException.DOB_In_Result = Convert.ToDateTime(obj[2]);
                        string Sex = string.Empty;
                        if (obj[3].ToString().ToUpper() == "FEMALE")
                        {
                            Sex = "Female";
                        }
                        else if (obj[3].ToString() == "MALE")
                        {
                            Sex = "Male";
                        }
                        objFillOrderException.Sex_In_Capella = Sex;
                        objFillOrderException.SSN = obj[4].ToString();
                        if (obj[5].ToString() == ";")
                            objFillOrderException.Lab_Procedure = "";
                        else
                            objFillOrderException.Lab_Procedure = obj[5].ToString();
                        if (obj[6] != null)
                            objFillOrderException.Provider_Name = obj[6].ToString();
                        objFillOrderException.Lab_Name = obj[7].ToString();
                        if (obj[8] != null)
                            objFillOrderException.Lab_Location_Name = obj[8].ToString();
                        //DateTime orderdate = DateTime.ParseExact(obj[9].ToString(), ExpectedFormates, System.Globalization.CultureInfo.InstalledUICulture, System.Globalization.DateTimeStyles.None);
                        objFillOrderException.Order_Date = Convert.ToDateTime(obj[9]);
                        //objFillOrderException.Order_ID = Convert.ToUInt32(obj[13]);
                        objFillOrderException.Order_Submit_ID = Convert.ToUInt32(obj[10]);
                        objFillOrderException.TotalCount = TotalNumberOfRecords;
                        resturnList.Add(objFillOrderException);
                    }
                    iMySession.Close();
                    return resturnList;
                }

            }

        }
        public void UpdateResultMaster(ulong ResultMasterID, ulong humanID, string MacAddress)
        {
            ResultMaster objResultMaster = GetById(ResultMasterID);
            if (objResultMaster != null)
            {
                objResultMaster.Matching_Patient_Id = humanID;
            }
            ResultORCManager objResultORCManager = new ResultORCManager();
            ResultORC objResultORC = new ResultORC();
            IList<ResultORC> ilstResultORC = new List<ResultORC>();
            ilstResultORC = objResultORCManager.GetResultByMasterID(ResultMasterID);
            ulong PhysicianId = 0;
            if (ilstResultORC.Count > 0)
            {
                PhysicianManager objPhysicianManager = new PhysicianManager();
                IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
                ilstPhysicianLibrary = objPhysicianManager.GetPhysicianByNPI(ilstResultORC[0].ORC_Ordering_Provider_ID);
                if (ilstPhysicianLibrary.Count > 0)
                {
                    PhysicianId = ilstPhysicianLibrary[0].Id;
                }
            }

            VitalsManager objVitalsManager = new VitalsManager();
            IList<PatientResults> ilstPatientResults = objVitalsManager.GetPatientListByResultMasterID(ResultMasterID);
            if (ilstPatientResults.Count > 0)
            {

                foreach (PatientResults obj in ilstPatientResults)
                {



                    obj.Human_ID = humanID;
                    obj.Encounter_ID = 0;
                    obj.Physician_ID = PhysicianId;
                    obj.Modified_Date_And_Time = DateTime.Now;
                }


                IList<PatientResults> saveResult = new List<PatientResults>();
                //objVitalsManager.SaveUpdateDeleteWithTransaction(ref saveResult, saveResult, null, MacAddress);
                objVitalsManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveResult, ref saveResult, null, MacAddress, false, false, 0, string.Empty);
            }
            else
            {
                IList<PatientResults> saveResult = new List<PatientResults>();
                ResultOBXManager objResultOBXManager = new ResultOBXManager();
                IList<ResultOBX> ilstResultOBX = new List<ResultOBX>();
                ilstResultOBX = objResultOBXManager.GetResultByMasterID(ResultMasterID);
                PatientResults objPatientResults = new PatientResults();
                foreach (ResultOBX obj in ilstResultOBX)
                {
                    if (obj.OBX_Loinc_Identifier != string.Empty)
                    {
                        objPatientResults = new PatientResults();
                        AcurusResultsMappingManager objAcurusResultsMappingManager = new AcurusResultsMappingManager();
                        IList<AcurusResultsMapping> AcurusResultList = new List<AcurusResultsMapping>();
                        AcurusResultList = objAcurusResultsMappingManager.GetAcurusResultsMappingListForResultMaster(objResultMaster.Lab_ID, obj.OBX_Observation_Identifier);
                        if (AcurusResultList.Count > 0)
                        {
                            objPatientResults.Acurus_Result_Code = AcurusResultList[0].Acurus_Result_Code;
                            objPatientResults.Acurus_Result_Description = AcurusResultList[0].Acurus_Result_Description;
                        }
                        objPatientResults.Abnormal_Flags = obj.OBX_Abnormal_Flag;
                        objPatientResults.Created_By = obj.Created_By;
                        objPatientResults.Created_Date_And_Time = obj.Created_Date_And_Time;
                        objPatientResults.Result_Master_ID = objResultMaster.Id;
                        objPatientResults.Human_ID = humanID;
                        objPatientResults.Loinc_Identifier = obj.OBX_Loinc_Identifier;
                        objPatientResults.Loinc_Observation = obj.OBX_Loinc_Observation_Text;
                        objPatientResults.Reference_Range = obj.OBX_Reference_Range;
                        objPatientResults.Units = obj.OBX_Units;
                        objPatientResults.Value = obj.OBX_Observation_Value;
                        objPatientResults.Physician_ID = PhysicianId;
                        //if (objPatientResults.Value.Length > 100)
                        //    continue;
                        objPatientResults.Results_Type = "Results";
                        DateTime dtpBirthdate = DateTime.MinValue;

                        ResultOBRManager objResultOBRManager = new ResultOBRManager();
                        ResultOBR objResultOBR = new ResultOBR();
                        objResultOBR = objResultOBRManager.GetById(obj.Result_OBR_ID);
                        string sBirthdate = objResultOBR.OBR_Specimen_Collection_Date_And_Time;
                        if (sBirthdate != string.Empty)
                        {
                            dtpBirthdate = DateTime.ParseExact(sBirthdate.Substring(0, 8), "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture);
                            string value = dtpBirthdate.ToString("yyyy-mm-dd");
                            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                            dateInfo.ShortDatePattern = "yyyy-mm-dd";
                            DateTime validDate = Convert.ToDateTime(value, dateInfo);
                            objPatientResults.Captured_date_and_time = validDate;
                        }
                        saveResult.Add(objPatientResults);
                    }
                }
                if (saveResult.Count > 0)
                {
                    //objVitalsManager.SaveUpdateDeleteWithTransaction(ref saveResult, null, null, MacAddress);
                    IList<PatientResults> saveResultnull = null;
                    objVitalsManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveResult, ref saveResultnull, null, MacAddress, false, false, 0, string.Empty);
                }

            }

            IList<ResultMaster> ilstSaveList = new List<ResultMaster>();
            IList<ResultMaster> ilstUpdateList = new List<ResultMaster>();
            ilstUpdateList.Add(objResultMaster);
            //SaveUpdateDeleteWithTransaction(ref ilstSaveList, ilstUpdateList, null, MacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstSaveList, ref ilstUpdateList, null, MacAddress, false, false, 0, string.Empty);
            if (objResultMaster.Order_ID != 0)
            {
                OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
                OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                string CurrentOwner = "UNKNOWN";
                objOrdersSubmit = objOrdersSubmitManager.GetById(objResultMaster.Order_ID);
                if (objOrdersSubmit.Physician_ID != 0)
                {
                    IList<User> tempUser = new List<User>();
                    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                    {

                        ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", objOrdersSubmit.Physician_ID));
                        tempUser = criteria.List<User>();
                        iMySession.Close();
                    }
                    if (tempUser.Count > 0)
                    {
                        CurrentOwner = tempUser[0].user_name;
                    }

                }
                WFObjectManager objWFObjectManager = new WFObjectManager();
                WFObject objWFObject = new WFObject();
                string objType = "DIAGNOSTIC ORDER";
                objWFObject = objWFObjectManager.GetByObjectSystemId(objOrdersSubmit.Id, objType);
                if (objWFObject.Id != 0)
                {
                    if (objWFObject.Current_Process == "RESULT_PROCESS")
                    {
                        objWFObjectManager.MoveToNextProcess(objWFObject.Obj_System_Id, objType, 1, CurrentOwner, DateTime.Now, MacAddress, null, null);
                    }
                    else if (objWFObject.Current_Process == "MA_RESULTS")
                    {
                        objWFObjectManager.MoveToNextProcess(objWFObject.Obj_System_Id, objType, 2, CurrentOwner, DateTime.Now, MacAddress, null, null);
                    }
                    else if (objWFObject.Current_Process == "ORDER_GENERATE" || objWFObject.Current_Process == "BILLING_WAIT")
                    {
                        objWFObjectManager.MoveToNextProcess(objWFObject.Obj_System_Id, objType, 8, CurrentOwner, DateTime.Now, MacAddress, null, null);
                    }
                }

            }
            //IList<ResultMaster> ilstSaveList = new List<ResultMaster>();
            //IList<ResultMaster> ilstUpdateList = new List<ResultMaster>();
            //ilstUpdateList.Add(objResultMaster);
            //SaveUpdateDeleteWithTransaction(ref ilstSaveList, ilstUpdateList, null, MacAddress);
            ErrorLogManager objErrorLogManager = new ErrorLogManager();
            objErrorLogManager.DeleteErrorLogByResultMasterID(ResultMasterID, MacAddress);
            //WFObjectManager objWFObjectManager = new WFObjectManager();
            //WFObject objWFObject = new WFObject();
            //string objType= "DIAGNOSTIC ORDER";
            //objWFObject = objWFObjectManager.GetByObjectSystemId(OrderSubmitID,objType);
            //if (objWFObject.Id != 0)
            //{
            //    if (objWFObject.Current_Process == "RESULT_PROCESS")
            //    {
            //        objWFObjectManager.MoveToNextProcess(objWFObject.Obj_System_Id, objType, 1,CurrentOwner, DateTime.Now, MacAddress, null, null);
            //    }
            //    else if (objWFObject.Current_Process == "MA_RESULTS")
            //    {
            //        objWFObjectManager.MoveToNextProcess(objWFObject.Obj_System_Id, objType, 2, CurrentOwner, DateTime.Now, MacAddress, null, null);
            //    }
            //    else if (objWFObject.Current_Process == "ORDER_GENERATE" || objWFObject.Current_Process == "BILLING_WAIT")
            //    {
            //        objWFObjectManager.MoveToNextProcess(objWFObject.Obj_System_Id, objType, 8, CurrentOwner, DateTime.Now, MacAddress, null, null);
            //    }


            //}


        }

        public ulong SaveResultMasterforSummary(IList<ResultMaster> lstresultmaster)
        {
            IList<ResultMaster> lstresultmasternull = null;
            //SaveUpdateDeleteWithTransaction(ref lstresultmaster, null, null, string.Empty);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstresultmaster, ref lstresultmasternull, null, string.Empty, false, false, 0, string.Empty);
            return lstresultmaster[0].Id;
        }

        //Muthusamy Changes made for Lab exception
        public IList<FillOrderException> GetOrderExceptionItems(int PageNumber, int MaxResultsPerPage, string SearchCriteria, string NPINumbers, IList<string> FieldName, IList<string> Value)
        {
            IList<FillOrderException> resturnList = new List<FillOrderException>();
            IQuery query = null;
            IQuery queryCount = null;
            int TotalNumberOfRecords = 0;
            bool bCheck = true;
            ArrayList arrData = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (SearchCriteria.ToUpper().Trim() == "ALL RESULTS")
                {
                    queryCount = iMySession.GetNamedQuery("OrderException.WithNPI.Criteria.Count");
                    query = iMySession.GetNamedQuery("OrderException.WithNPI.Criteria");
                }
                else if (SearchCriteria.ToUpper().Trim() == "RESULT ATTACHED TO PATIENT CHART")
                {
                    queryCount = iMySession.GetNamedQuery("OrderException.WithMatchingPatient.Criteria.Count");
                    query = iMySession.GetNamedQuery("OrderException.WithMatchingPatient.Criteria");
                }
                else if (SearchCriteria.ToUpper().Trim() == "RESULT NOT ATTACHED TO PATIENT CHART")
                {
                    queryCount = iMySession.GetNamedQuery("OrderException.WithNotMatchingPatient.Criteria.Count");
                    query = iMySession.GetNamedQuery("OrderException.WithNotMatchingPatient.Criteria");
                }
                else if (SearchCriteria.ToUpper().Trim() == "ALL")
                {
                    queryCount = iMySession.GetNamedQuery("OrderException.WithALL.Criteria.Count");
                    query = iMySession.GetNamedQuery("OrderException.WithALL.Criteria");
                    bCheck = false;

                }
                else if (SearchCriteria.ToUpper().Trim() == "PROVIDER NOT ASSIGNED")
                {
                    queryCount = iMySession.GetNamedQuery("OrderException.WithProviderNotAssigned.Criteria.Count");
                    query = iMySession.GetNamedQuery("OrderException.WithProviderNotAssigned.Criteria");
                    bCheck = false;
                }

                //BugID:46054
                if (query != null && queryCount != null)
                {
                    string subQuery = string.Empty;
                    if (FieldName != null && FieldName.Count > 0)
                    {
                        for (int i = 0; i < FieldName.Count; i++)
                        {
                            switch (FieldName[i])
                            {
                                case "MSH_Date_And_Time_Of_Message":
                                    {
                                        subQuery += "and MSH_Date_And_Time_Of_Message between '" + Value[i].Split('-')[0] + "%' and '" + Value[i].Split('-')[1] + "%' ";
                                    }
                                    break;
                                case "Reason_Code":
                                    {
                                        subQuery += " and (Reason_Code like '%" + Value[i].Split('-')[0] + "%' or Reason_description like '%" + Value[i].Split('-')[1] + "%') ";
                                    } break;
                                case "Lab_Id":
                                    {
                                        subQuery += " and r.Lab_Id = " + Value[i] + " ";
                                    } break;
                                default: { subQuery = string.Empty; } break;
                            }
                        }
                    }
                    if (subQuery != string.Empty)
                    {
                        string QueryCnt = queryCount.ToString();
                        QueryCnt = QueryCnt.Insert(QueryCnt.IndexOf("group by"), subQuery);
                        string Querydata = query.ToString();
                        Querydata = Querydata.Insert(Querydata.IndexOf("group by"), subQuery);
                        ISQLQuery sqlqryCnt = iMySession.CreateSQLQuery(QueryCnt);
                        ISQLQuery sqlqry = iMySession.CreateSQLQuery(Querydata);
                        queryCount = sqlqryCnt;
                        query = sqlqry;
                    }
                    if (bCheck)
                    {
                        queryCount.SetString(0, NPINumbers);
                        TotalNumberOfRecords = queryCount.List().Count;
                        query.SetString(0, NPINumbers);
                        query.SetInt32(1, (PageNumber - 1) * MaxResultsPerPage);
                        query.SetInt32(2, MaxResultsPerPage);
                    }
                    else
                    {
                        TotalNumberOfRecords = queryCount.List().Count;
                        query.SetInt32(0, (PageNumber - 1) * MaxResultsPerPage);
                        query.SetInt32(1, MaxResultsPerPage);
                    }
                    arrData = new ArrayList(query.List());



                    string[] ExpectedFormates = new string[] { "yyyyMMdd" };
                    foreach (object[] obj in arrData)
                    {

                        FillOrderException objFillOrderException = new FillOrderException();
                        ulong HumanID = 0;
                        if (obj[0] != null)
                            ulong.TryParse(obj[0].ToString(), out HumanID);
                        objFillOrderException.HumanId = HumanID;
                        if (obj[1] != null)
                            objFillOrderException.Last_Name_In_Result = obj[1].ToString();
                        if (obj[2] != null)
                            objFillOrderException.First_Name_In_Result = obj[2].ToString();
                        if (obj[3] != null)
                            objFillOrderException.MI_In_Result = obj[3].ToString();
                        if (obj[4] != null && obj[4].ToString().Trim() != string.Empty)
                        {
                            DateTime dob = DateTime.ParseExact(obj[4].ToString(), ExpectedFormates, System.Globalization.CultureInfo.InstalledUICulture, System.Globalization.DateTimeStyles.None);
                            objFillOrderException.DOB_In_Result = dob;
                        }
                        if (obj[5] != null)
                            objFillOrderException.Sex_In_Result = obj[5].ToString();
                        if (obj[6] != null)
                            objFillOrderException.SSN = obj[6].ToString();
                        if (obj[7] != null)
                            objFillOrderException.Lab_Procedure = obj[7].ToString();
                        if (obj[8] != null && obj[9] != null)
                            objFillOrderException.Provider_Name = obj[8].ToString() + " " + obj[9].ToString();
                        if (obj[10] != null)
                            objFillOrderException.Reason_Code = obj[10].ToString();
                        if (obj[11] != null)
                            objFillOrderException.Reason_Description = obj[11].ToString();
                        if (obj[12] != null)
                            objFillOrderException.Result_Master_ID = Convert.ToUInt32(obj[12]);
                        if (obj[13] != null)
                            objFillOrderException.Order_ID = Convert.ToUInt32(obj[13]);
                        if (obj[14] != null)
                            objFillOrderException.Lab_ID = obj[14].ToString();
                        if (obj[15] != null)
                            objFillOrderException.Result_Received_Date_And_Time = obj[15].ToString();
                        if (obj[16] != null)
                            objFillOrderException.Matching_Patient_ID = Convert.ToUInt32(obj[16]);
                        if (obj[17] != null)
                            objFillOrderException.Specimen = obj[17].ToString();
                        if (obj[18] != null)
                            objFillOrderException.Reason_Code = obj[18].ToString();
                        if (obj[19] != null)
                            objFillOrderException.OrderingProviderNPI = obj[19].ToString();
                        if (objFillOrderException.Matching_Patient_ID != 0)
                        {
                            HumanManager objHumanManager = new HumanManager();
                            Human objHuman = objHumanManager.GetById(objFillOrderException.Matching_Patient_ID);
                            objFillOrderException.First_Name_In_Capella = objHuman.First_Name;
                            objFillOrderException.Last_Name_In_Capella = objHuman.Last_Name;
                            objFillOrderException.MI_In_Capella = objHuman.MI;
                            objFillOrderException.DOB_In_Capella = objHuman.Birth_Date;
                            objFillOrderException.Sex_In_Capella = objHuman.Sex;
                            objFillOrderException.Human_Id_In_Capella = objHuman.Id;
                            OrdersManager orderMngr = new OrdersManager();
                            objFillOrderException.OrdersList = orderMngr.GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId("DIAGNOSTIC ORDER", "RESULT_PROCESS", objFillOrderException.Matching_Patient_ID);

                        }
                        if (obj[14] != null && obj[14].ToString() != "")
                        {
                            LabManager objLabMngr = new LabManager();
                            objFillOrderException.Lab_Name = objLabMngr.GetById(Convert.ToUInt32(obj[14])).Lab_Name;
                        }
                        if (obj[13] != null)//Set abnormal using order_id
                        {
                            if (Convert.ToUInt32(obj[13]) != 0)
                            {

                                IList<string> sIsAbnormal = iMySession.CreateSQLQuery("Select is_abnormal from wf_object m where  m.obj_system_id=" + obj[13] + "").List<string>();
                                if (sIsAbnormal != null && sIsAbnormal.Count > 0)
                                {
                                    if (sIsAbnormal[0] != null && sIsAbnormal[0].Trim() != string.Empty)
                                    {
                                        if (sIsAbnormal[0].ToUpper() == "YES")
                                            objFillOrderException.Is_Abnormal = "Yes";
                                        else
                                            objFillOrderException.Is_Abnormal = "No";
                                    }
                                }

                            }
                        }
                        //Set abnormal using result_master_id
                        if (objFillOrderException.Is_Abnormal.Trim() == string.Empty)
                        {
                            if (obj[12] != null)
                            {
                                if (Convert.ToUInt32(obj[12]) != 0)
                                {
                                    IList<string> sIsAbnormal = iMySession.CreateSQLQuery("Select is_abnormal from wf_object m where  m.obj_system_id=" + obj[12] + "").List<string>();
                                    if (sIsAbnormal != null && sIsAbnormal.Count > 0)
                                    {
                                        if (sIsAbnormal[0] != null && sIsAbnormal[0].Trim() != string.Empty)
                                        {
                                            if (sIsAbnormal[0].ToUpper() == "YES")
                                                objFillOrderException.Is_Abnormal = "Yes";
                                            else
                                                objFillOrderException.Is_Abnormal = "No";
                                        }
                                    }

                                }
                            }
                        }
                        if (objFillOrderException.Is_Abnormal.Trim() == string.Empty)
                            objFillOrderException.Is_Abnormal = "No";

                        objFillOrderException.TotalCount = TotalNumberOfRecords;
                        resturnList.Add(objFillOrderException);
                    }
                }
                iMySession.Close();
            }
            return resturnList;
        }
        //$Muthusamy 


        public void UpdateResultMasterListForLab(ulong ResultMasterID, ulong Human_ID, ulong Order_ID, ulong Matching_Patient_ID, string PhysicianNPI, bool bCheck, string MacAddress, ulong ulAutoPhyID)
        {

            PhysicianManager phyMngr = new PhysicianManager();
            IList<PhysicianLibrary> Phylst = phyMngr.GetPhysicianByNPI(PhysicianNPI);
            UserManager userMngr = new UserManager();
            IList<User> userList = new List<User>();
            if (Phylst.Count > 0)
                userList = userMngr.GetUserbyPhysicianLibraryID(Phylst[0].Id);
            string Current_Owner = "";
            if (Phylst.Count > 0)
            {
                IList<User> ResultList = userList.Where(a => a.Physician_Library_ID == Phylst[0].Id).ToList<User>();
                if (ResultList.Count > 0)
                {
                    Current_Owner = ResultList[0].user_name;
                }
                else
                {
                    userList = userMngr.GetUserbyPhysicianLibraryID(ulAutoPhyID);
                    Current_Owner = userList[0].user_name;
                }
            }
            ResultMaster objResultMaster = GetById(ResultMasterID);
            if (objResultMaster != null)
            {
                objResultMaster.Matching_Patient_Id = Human_ID;
                objResultMaster.Modified_By = Current_Owner;
                objResultMaster.Modified_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            }
            IList<ResultMaster> ilstSaveList = new List<ResultMaster>();
            IList<ResultMaster> ilstUpdateList = new List<ResultMaster>();
            ilstUpdateList.Add(objResultMaster);
            //SaveUpdateDeleteWithTransaction(ref ilstSaveList, ilstUpdateList, null, MacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstSaveList, ref ilstUpdateList, null, string.Empty, false, false, 0, string.Empty);

            //BugID:51827 
            ResultORCManager resultORCMngr = new ResultORCManager();
            IList<ResultORC> ilstORCSaveList = new List<ResultORC>();
            IList<ResultORC> ilstORCUpdateList = new List<ResultORC>();
            IList<ResultORC> objResultORC = resultORCMngr.GetResultByMasterID(ResultMasterID);
            if (objResultORC != null && objResultORC.Count > 0)
            {
                foreach (ResultORC objResult in objResultORC)
                {
                    objResult.ORC_Ordering_Provider_ID = PhysicianNPI;
                    ilstORCUpdateList.Add(objResult);
                }
            }
            resultORCMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstORCSaveList, ref ilstORCUpdateList, null, string.Empty, false, false, 0, string.Empty);


            //if (Matching_Patient_ID == 0 || bCheck)
            //{
                iTryCount = 0;
            TryAgain:
                int iResult = 0;
                ISession MySession = Session.GetISession();
                ITransaction trans = null;
                try
                {
                    trans = MySession.BeginTransaction();
                    WFObjectManager objWFObjectManager = new WFObjectManager();
                    WFObject objWFObject = new WFObject();
                    objWFObject.Obj_System_Id = ResultMasterID;
                    objWFObject.Obj_Type = "DIAGNOSTIC_RESULT";
                    objWFObject.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                    objWFObject.Current_Process = "START";
                    objWFObject.Current_Owner = Current_Owner;
                    objWFObject.Fac_Name = "ALL";
                    iResult = objWFObjectManager.InsertToWorkFlowObject(objWFObject, 1, string.Empty, MySession);

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

                    MySession.Flush();
                    trans.Commit();

                }
                catch (NHibernate.Exceptions.GenericADOException ex)
                {
                    trans.Rollback();
                //CAP-1942
                throw new Exception(ex.Message,ex);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                //CAP-1942
                throw new Exception(e.Message,e);
                }
                finally
                {
                    MySession.Close();
                }

                ErrorLogManager objErrorLogManager = new ErrorLogManager();
                objErrorLogManager.UpdateErrorLogByResultMasterID(ResultMasterID, Current_Owner, MacAddress);
            //}



        }

        public void UpdateResultMasterAndWf_Object(ulong ResultMasterID, ulong order_Submit_ID, ulong Human_ID, string NPINumber, string MacAddress)
        {
            //CAP-3187
            //PhysicianManager phyMngr = new PhysicianManager();
            //IList<PhysicianLibrary> Phylst = phyMngr.GetPhysicianByNPI(NPINumber);
            UserManager userMngr = new UserManager();
            string Current_Owner = "";
            
            //CAP-3187
            //if (Phylst.Count > 0)
            //{
            //IList<User> ResultList = userMngr.GetUserbyPhysicianLibraryID(Phylst[0].Id).ToList<User>();
            IList<User> ResultList = userMngr.GetUserbyPhysicianNPI(NPINumber);                
                if (ResultList !=null) { 
                Current_Owner = ResultList[0].user_name;
                }

           // }

            ResultMaster objResultMaster = GetById(ResultMasterID);
            if (objResultMaster != null)
            {
                objResultMaster.Matching_Patient_Id = Human_ID;
                objResultMaster.Order_ID = order_Submit_ID;
                objResultMaster.Modified_By = Current_Owner;
                objResultMaster.Modified_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            }
            IList<ResultMaster> ilstSaveList = new List<ResultMaster>();
            IList<ResultMaster> ilstUpdateList = new List<ResultMaster>();
            ilstUpdateList.Add(objResultMaster);
            //SaveUpdateDeleteWithTransaction(ref ilstSaveList, ilstUpdateList, null, MacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstSaveList, ref ilstUpdateList, null, MacAddress, false, false, 0, string.Empty);
            WFObjectManager objWFObjectManager = new WFObjectManager();
            WFObject objWFObject = new WFObject();
            string objType = "DIAGNOSTIC ORDER";
            objWFObject = objWFObjectManager.GetByObjectSystemId(order_Submit_ID, objType);
            if (objWFObject.Id != 0)
            {
                if (objWFObject.Current_Process == "RESULT_PROCESS")
                {
                    objWFObjectManager.MoveToNextProcess(objWFObject.Obj_System_Id, objType, 1, Current_Owner, DateTime.Now, MacAddress, null, null);
                }
                else if (objWFObject.Current_Process == "ORDER_GENERATE")
                {
                    objWFObjectManager.MoveToNextProcess(objWFObject.Obj_System_Id, objType, 8, Current_Owner, DateTime.Now, MacAddress, null, null);
                }
            }

            ErrorLogManager objErrorLogManager = new ErrorLogManager();
            objErrorLogManager.UpdateErrorLogByResultMasterID(ResultMasterID, Current_Owner, MacAddress);
        }

        public ResultMaster SaveResultMasterItem(ResultMaster objResultMaster, ulong ulFileManagementIndexID)
        {
            //ResultMaster objResMaster = new ResultMaster();
            IList<ResultMaster> saveList = new List<ResultMaster>();
            IList<ResultMaster> updateList = new List<ResultMaster>();
            //if (objResultMaster != null)
            //{
            //    if (objResMaster.Id != 0)
            //        saveList.Add(objResultMaster);
            //    else
            //        updateList.Add(objResultMaster);
            //}
            if (objResultMaster != null)
            {
                if (objResultMaster.Id == 0)
                    saveList.Add(objResultMaster);
                else
                    updateList.Add(objResultMaster);
            }
            if (saveList != null && updateList != null && (saveList.Count > 0 || updateList.Count > 0))
                SaveUpdateDelete_DBAndXML_WithTransaction(ref saveList, ref updateList, null, string.Empty, false, false, 0, string.Empty);
            if (saveList != null && saveList.Count > 0)
                objResultMaster = saveList[0];
            else
                if (updateList != null && updateList.Count > 0)
                    objResultMaster = updateList[0];

            if (ulFileManagementIndexID != 0)
            {
                FileManagementIndexManager fileMgmtIndexMngr = new FileManagementIndexManager();
                IList<FileManagementIndex> addIndexList = null;
                IList<FileManagementIndex> updateIndexList = fileMgmtIndexMngr.GetFileListUsingFileIndexID(ulFileManagementIndexID);
                if (updateIndexList != null && updateIndexList.Count > 0)
                    updateIndexList[0].Result_Master_ID = objResultMaster.Id;
                fileMgmtIndexMngr.SaveUpdateDeleteFileManagementIndexForOnline(addIndexList, updateIndexList, null, string.Empty);
            }

            return objResultMaster;
        }

        public void SaveResultMasterforDeleteFiles(IList<ResultMaster> lstResMasterUpdate)
        {
            IList<ResultMaster> lstResMasnull = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstResMasnull, ref lstResMasterUpdate, null, string.Empty, false, false, 0, string.Empty);
        }

        public IList<ResultMaster> GetResultMasterByFileName(string fileName)
        {
            IList<ResultMaster> ilstResultMaster = new List<ResultMaster>();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(ResultMaster)).Add(Expression.Eq("File_Name", fileName));
            ilstResultMaster = crit.List<ResultMaster>();

            return ilstResultMaster;
        }

    }
}