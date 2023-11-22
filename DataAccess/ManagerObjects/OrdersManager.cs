using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Reflection;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IOrdersManager : IManagerBase<Orders, ulong>
    {
        //ulong InsertToOrders(IList<Orders> saveList, IList<OrdersSubmit> objOrderSubmit, IList<OrdersAssessment> ordAssList, IList<string> sOrderingProcedure, ulong EncounterID, string orderType, string MACAddress);
        ulong InsertToOrders(IList<Orders> saveList, IList<OrdersSubmit> objOrderSubmit, IList<OrdersAssessment> ordAssList, IList<string> sOrderingProcedure, ulong EncounterID, string orderType, string MACAddress, IList<OrdersRequiredForms> ilistOrdersRequiredForms, ref string sSelectedOrder, string sLocalTime);
        Stream GetOrdersUsingEncounterID(ulong EncounterID, string orderType, bool LoadQuestionSets);
        Stream LoadOrders(ulong EncounterID, ulong PhysicianID, ulong HumanID, string orderType, string procedureType, DateTime todaysDate, bool LoadQuestionSets);
        Stream LoadOrdersByOrdersSubmitedId(ulong OrderSubmitId, ulong EncounterID, ulong PhysicianID, ulong HumanID, string orderType, string procedureType, DateTime todaysDate, bool LoadQuestionSet);
        Stream DeleteOrders(IList<OrderLabDetailsDTO> obj, IList<OrdersAssessment> ordAssList, ulong EncounterID, string orderType, string MACAddress, IList<string> PlanLst);
        ulong UpdateOrders(IList<Orders> saveList, IList<Orders> updtList, IList<Orders> delList, IList<OrdersAssessment> savOrdAssListForExistingCPT, IList<OrdersAssessment> delOrdAssListForExistingCPT, IList<OrdersAssessment> savOrdAssListForNewCPT, IList<OrdersQuestionSetAfp> afpList, IList<OrdersQuestionSetBloodLead> bloodLeadList, IList<OrdersQuestionSetCytology> cytologyList, TreatmentPlan UpdatePlan, IList<string> PlanDel, IList<string> SelectedProcedures, IList<string> PlanReplace, ulong EncounterID, string orderType, string MACAddress, IList<OrdersQuestionSetAOE> AOESaveList, IList<OrdersSubmit> SavOrderSubmit, IList<OrdersSubmit> UpdOrderSubmit, IList<OrdersSubmit> DelOrderSubmit, IList<OrdersRequiredForms> SaveListOrdersRequiredForms, IList<OrdersRequiredForms> UpdateListOrdersRequiredForms, IList<OrdersRequiredForms> DeleteListOrdersRequiredForms, string sLocalTime);//, Dictionary<string, IList<OrdersQuestionSetAOE>> AOESaveList)
        IList<OrdersAssessment> GetOrdersAssessmentDetails(IList<ulong> OrderIDList);
        IList<OrderLabDetailsDTO> LoadOrdersForCollectSpecimen(ulong OrderSubmitID, string orderType, out IList<OrdersAssessment> OrderAssList);
        IList<OrderLabDetailsDTO> GetOrdersUsingOrderID(IList<ulong> OrderIDList);
        Stream GetOrdersUsingHumanID(ulong HumanID, DateTime createdDate, string OrderType, ulong Physician_Id, bool LoadQuestionSets);
        Stream GetOrdersUsingEncounterIDByOrderSubmitID(ulong OrderSubmitId, ulong EncounterID, string orderType, bool LoadQuestionSet);
        Stream GetOrdersUsingHumanIDByOrderSubmitID(ulong OrderSubmitId, ulong HumanID, DateTime createdDate, string OrderType, ulong PhysicianId, bool LoadQuestionSet);
        IList<Orders> GetOrdersBySubmitID(IList<ulong> order_Submit_Id_List);
        IList<FillOrdersManagementDTO> SearchOrder(string order_type, string order_status, string FacilityName, int orderInDays, DateTime fromDate, DateTime toDate, ulong humanID, ulong physicianID, ulong labId, ulong labLocationId, int pageNumber, int maxResults);
        void SubmitLabImageOrders(ulong ulMyEncounterID, ulong ulMyHumanID, ulong ulMyPhysicianID, string UserName, string MACAddress, DateTime currentDateTime, string orderType, string wfObjType, string medAsstName);
        ulong InsertDummyOrder(IList<OrdersSubmit> ordersubmitlist, IList<Orders> ordersList, string obj_Type, string facility_Name, string MACAddress);
        ulong UpdateOrderAndOrdersSubmit(IList<OrdersSubmit> ordersubmitlist, IList<Orders> ordersList, string MACAddress);
        IList<CheckoutOrdersDTO> GetOrdersByEncounterID(ulong EncounterId);
        FillHumanDTO GetHumanById(ulong HumanId);
        CheckOutPrintOrdersDTO GetOrdersForCheckoutPrint(ulong EncounterID, ulong HumanId);
        IList<ResultEntryDTO> UpdateOrdersForResult(Orders UpdateOrder, string MacAddress);
        void SubmitOrdersToLab(ulong myHumanId, ulong myEncounterId, string objType, string MACAddress, string UserName, DateTime currentDate, string MedAsstUserName);
        //<<<<<<< OrdersManager.cs
        //        bool SubmitOrdersWithOutEncounter(ulong myHumanId, ulong myEncounterId, string objType, string MACAddress, string UserName, DateTime currentDate, string MedAsstUserName, IList<string> OrdersType, string sFacilityName);
        //||||||| 1.131
        //        bool SubmitOrdersWithOutEncounter(ulong myHumanId, ulong myEncounterId, string objType, string MACAddress, string UserName, DateTime currentDate, string MedAsstUserName, IList<string> OrdersType);
        //=======
        bool SubmitOrdersWithOutEncounter(ulong myHumanId, ulong myEncounterId, string objType, string MACAddress, string UserName, DateTime currentDate, string MedAsstUserName, IList<string> OrdersType, string sFacilityName);
        void SubmitOrdersToLabWithoutEncounter(ulong myHumanId, ulong myEncounterId, string objType, string MACAddress, string UserName, DateTime currentDate, string MedAsstUserName);
        bool NotSubmitedOrders(ulong myHumanId, ulong myEncounterID, ulong PhysicianID);
        ulong InsertDummyOrderForManualResultEntry(IList<OrdersSubmit> ordersSubmitList, IList<Orders> ordersList, string obj_Type, string facility_Name, string MACAddress);
        //Added by bala
        Stream SubmitLogic(ulong myHumanId, ulong myEncounterID, ulong PhysicianID, string OrderType, string sUserName, DateTime CreatedDate, string sFacilityName);
        //Dhinesh-For Quest
        IList<ulong> GetWfObjectIDsFromObjSystemIDs(IList<ulong> OrderSubmitIDs, string Objtype);
        // Added by Manimozhi 
        int InsertDummyOrderForResultEntry(ref IList<Orders> ordersList, ISession MySession, string macAddress);
        Stream LoadOrdersAtCheckOut(DateTime Modified_Date_And_Time, ulong HumanID, string LabType);
        Stream GetOrdersUsingOrderSubmitID(DateTime Modified_Date_And_Time, string LabType);
        //<<<<<<< OrdersManager.cs
        //        bool DeletedOrders(ulong OrderSubmitID, string objType, string MacAddress);
        //        bool IsEditable(ulong OrderSubmitID, string objType);

        //||||||| 1.131
        //        void DeletedOrders(ulong OrderSubmitID, string objType, string MacAddress);

        bool DeletedOrders(ulong OrderSubmitID, string objType, string MacAddress);
        bool IsEditable(ulong OrderSubmitID, string objType);
        OrderSpecimenDTO GetSpecimenDetails(IList<ulong> OrderID);
        OrderSpecimenDTO LoadSpecimen(ulong orderSubmitID, string OrderType);
        // Added by Manimozhi - MRE - 5th Dec 2012
        IList<Orders> GetListOfOrdersForMRE(ulong ulOrderSubmitID);
        IList<Orders> GetOutstandingCPTForOrderGenerateProcess(ulong HumanID);
        void UpdateCMGEncounterID(ulong CMGEncounterID, ulong HumanID, string TestsOrdered, string MacAddress);
        void SubmitEmptyOrdersToMA(OrdersSubmit objOrdersSubmit, string MACAddress);
        Stream LoadOrdersCMG(ulong CMG_Encounter_Id, ulong Human_ID);
        void DeleteCMGEncounterIDFromOrders(IList<string> OrderIDList, string MacAddress);
        IList<Orders> GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId(string ObjectType, string CurrentProcess, ulong Human_id);

        IList<string> GetOrderSubmitIDForPrintRequestion(ulong encounterID, ulong PhysicianID, ulong HumanID, string sOrderType);

        IList<Orders> GetOrderId(ulong OrderSubmitID);
        IList<ulong> GetOrderSubmitIDForPrintLable(ulong encounterID, ulong PhysicianID, ulong HumanID);
        Dictionary<string, string> GetLabDashboard(ulong PhyId, DateTime dtFromDate, DateTime dtToDate);
        IList<string> GetOrdersForPrint(string orderid);
        IList<Orders> GetOrdersByOrderSubmitID(List<int> OrderSubmitID);
        FillHumanDTO PatientInsuredBag(ulong HumanId);
        FillHumanDTO GetHumanByIdForCheckout(ulong HumanId, ulong ulEncounterID);
        IList<Orders> GetOrdersByOrderID(ulong[] uOrderID);
        IList<string> GetOrderByHuman(ulong ulHumanID, string sFacility, ulong ulLabID);
        string GetFaxOrders(string ordersubmitid);
    }

    public partial class OrdersManager : ManagerBase<Orders, ulong>, IOrdersManager
    {


        #region Constructors

        public OrdersManager()
            : base()
        {

        }
        public OrdersManager
            (INHibernateSession session)
            : base(session)
        {
        }

        #endregion


        #region CRUD
        int iTryCount = 0;
        public ulong InsertToOutstandingOrders(ulong OrderSubmitID, ulong EncounterId, string orderType, string MACAddress, IList<OrdersRequiredForms> ilistOrdersRequiredForms, ref string sSelectedOrder, string sUserName, string sFacilityName, string sLocalTime)
        {
            ulong ulOrdersubID = 0;
            DiagnosticDTO objDiagnosticDTO = new DiagnosticDTO();
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            OrdersManager objOrdersManager = new OrdersManager();
            LabInsurancePlanManager objLabInsurancePlanManager = new LabInsurancePlanManager();
            IList<Orders> ilstord = new List<Orders>();
            IList<string> sOrderingProcedure = new List<string>();
            IList<OrdersSubmit> ilstOrdersSubmitnew = new List<OrdersSubmit>();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            IList<OrdersSubmit> lstchildorderssubmit = new List<OrdersSubmit>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria critOrdSub1 = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", EncounterId));
                lstchildorderssubmit = critOrdSub1.List<OrdersSubmit>();
                if (lstchildorderssubmit.Count == 0)
                {
                    if (OrderSubmitID != 0)
                    {
                        IList<ulong> ordid = new List<ulong>();
                        //objDiagnosticDTO.objOrdersSubmit = objOrdersSubmitManager.GetById(OrderSubmitID);// for bug ID 52741 
                        //Lab_ID = objDiagnosticDTO.objOrdersSubmit.Lab_ID;
                        ICriteria OrdersCrit = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Order_Submit_ID", OrderSubmitID));
                        IList<Orders> OrdersBySubmitID = OrdersCrit.List<Orders>();
                        objDiagnosticDTO.OrdersLists = OrdersBySubmitID;

                        for (int i = 0; i < OrdersBySubmitID.Count; i++)
                        {
                            //ordid.Add(OrdersBySubmitID[i].Id);
                            Orders objord = new Orders();
                            objord = OrdersBySubmitID[i];
                            objord.Id = 0;
                            objord.Encounter_ID = EncounterId;
                            objord.Version = 1;
                            objord.Created_By = sUserName;
                            objord.Created_Date_And_Time = DateTime.UtcNow;
                            objord.Modified_By = string.Empty;
                            objord.Modified_Date_And_Time = DateTime.MinValue;
                            ilstord.Add(objord);
                        }
                        IList<OrdersSubmit> ilstOrdersSubmit = new List<OrdersSubmit>();

                        ICriteria critOrdSub = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", OrderSubmitID));
                        if (critOrdSub.List<OrdersSubmit>().Count > 0)
                        {
                            ilstOrdersSubmit = critOrdSub.List<OrdersSubmit>();
                            for (int j = 0; j < ilstOrdersSubmit.Count; j++)
                            {
                                OrdersSubmit objordsub = new OrdersSubmit();
                                objordsub = ilstOrdersSubmit[j];
                                objordsub.Id = 0;
                                objordsub.Encounter_ID = EncounterId;
                                objordsub.Version = 1;
                                objordsub.Created_By = sUserName;
                                objordsub.Created_Date_And_Time = DateTime.UtcNow;
                                objordsub.Modified_By = string.Empty;
                                objordsub.Modified_Date_And_Time = DateTime.MinValue;
                                objordsub.Facility_Name = sFacilityName;
                                objordsub.Is_Task_Created = "N";
                                ilstOrdersSubmitnew.Add(objordsub);
                            }
                        }
                        IList<OrdersAssessment> OrdersAssessmentList = null;

                        if (OrdersBySubmitID != null)
                        {
                            IList<ulong> OrderIDs = OrdersBySubmitID.Select(a => a.Id).ToList<ulong>();
                            //ICriteria OrdersAssessmentCrit = iMySession.CreateCriteria(typeof(OrdersAssessment)).Add(Expression.In("Order_ID", ordid.ToArray<ulong>()));
                            ICriteria OrdersAssessmentCrit = iMySession.CreateCriteria(typeof(OrdersAssessment)).Add(Expression.Eq("Order_Submit_ID", OrderSubmitID));
                            OrdersAssessmentList = OrdersAssessmentCrit.List<OrdersAssessment>();
                            IList<OrdersAssessment> ilstOrdAssessment = new List<OrdersAssessment>();
                            var val = OrdersAssessmentList.GroupBy(a => a.Order_Submit_ID).ToList();
                            //if (val.Count > 0)
                            //{
                            //    for (int k = 0; k < OrdersAssessmentList.Count; k++)
                            //    {
                            int k = -1;
                            foreach (var vval in val)
                            {
                                k++;
                                foreach (var v in vval)
                                {
                                    OrdersAssessment objass = new OrdersAssessment();
                                    //objass = OrdersAssessmentList[k];
                                    objass = (OrdersAssessment)v;
                                    objass.Id = 0;
                                    objass.Encounter_ID = EncounterId;
                                    objass.Version = 1;
                                    objass.Created_By = sUserName;
                                    objass.Order_Submit_ID = Convert.ToUInt32(k);
                                    objass.Created_Date_And_Time = DateTime.UtcNow;
                                    objass.Modified_By = string.Empty;
                                    objass.Modified_Date_And_Time = DateTime.MinValue;
                                    ordAssList.Add(objass);
                                }
                            }
                        }
                        WFObjectManager obj_workFlow = new WFObjectManager();
                        obj_workFlow.MoveToNextProcess(OrderSubmitID, "DIAGNOSTIC ORDER", 9, "UNKNOWN", DateTime.UtcNow, null, null, null);
                        OrdersManager oj = new OrdersManager();
                        ulOrdersubID = oj.InsertToOrders(ilstord, ilstOrdersSubmitnew, ordAssList, sOrderingProcedure, EncounterId, orderType, MACAddress, ilistOrdersRequiredForms, ref sSelectedOrder, sLocalTime);
                        if (sSelectedOrder != string.Empty)
                        {
                            ulOrdersubID = Convert.ToUInt32(sSelectedOrder.Split('|')[0]);
                        }
                    }
                }
                else
                {
                    ulOrdersubID = lstchildorderssubmit[0].Id;
                }
            }

            return ulOrdersubID;
        }
        public ulong InsertToOrders(IList<Orders> saveList, IList<OrdersSubmit> objOrderSubmit, IList<OrdersAssessment> ordAssList, IList<string> sOrderingProcedure, ulong EncounterID, string orderType, string MACAddress, IList<OrdersRequiredForms> ilistOrdersRequiredForms, ref string sSelectedOrder, string sLocalTime)
        {
            string Height = string.Empty;
            string Weight = string.Empty;
            ulong humanid = 0;
            bool IsOrders1 = true, IsOrders2 = true, IsOrders3 = true;
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XmlObjEnc = new GenerateXml();
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            iTryCount = 0;
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = null;
            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();

            ulong ulHumanid = 0;
            if (objOrderSubmit.Count > 0)
                ulHumanid = objOrderSubmit[0].Human_ID;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                #region Vitals Values
                ICriteria critgpID = null;
                IList<PatientResults> vitals = new List<PatientResults>();
                IList<string> VitalName = new List<string>();
                VitalName.Add("Height");
                VitalName.Add("Weight");
                if (EncounterID != Convert.ToUInt32("0"))
                {
                    critgpID = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Human_ID", ulHumanid)).Add(Expression.Eq("Results_Type", "Vitals"));
                    vitals = critgpID.List<PatientResults>();
                }
                else
                {
                    critgpID = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Human_ID", ulHumanid)).Add(Expression.Eq("Results_Type", "Vitals")).Add(Expression.Not(Expression.Eq("Vitals_Group_ID", Convert.ToUInt64("0"))))
                .SetProjection(Projections.Max("Vitals_Group_ID"));
                    ulong vitalsId = 0;
                    if (critgpID.List<object>().Count > 0)
                    {
                        vitalsId = Convert.ToUInt64(critgpID.List<object>()[0]);
                    }
                    if (vitalsId != 0)
                    {
                        ICriteria criteriaVital = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Vitals_Group_ID", vitalsId)).Add(Expression.In("Loinc_Observation", VitalName.ToArray<string>())).Add(Expression.Eq("Results_Type", "Vitals"));
                        vitals = criteriaVital.List<PatientResults>();
                    }
                }

                //if (critgpID.List<object>().Count > 0)
                //{
                //    vitalsId = Convert.ToUInt64(critgpID.List<object>()[0]);
                //}

                //ICriteria criteriaVital = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Vitals_Group_ID", vitalsId)).Add(Expression.In("Loinc_Observation", VitalName.ToArray<string>())).Add(Expression.Eq("Results_Type", "Vitals"));
                //IList<PatientResults> vitals = new List<PatientResults>();
                //vitals = criteriaVital.List<PatientResults>();
                foreach (PatientResults vtls in vitals)
                {
                    if (vtls.Loinc_Observation == "Height")
                        Height = vtls.Value;
                    else if (vtls.Loinc_Observation == "Weight")
                        Weight = vtls.Value;
                }
                foreach (OrdersSubmit os in objOrderSubmit)
                {
                    os.Height = Height;
                    os.Weight = Weight;
                    os.Is_Task_Created = "N";
                }
                iMySession.Close();
            }
        #endregion
        TryAgain:

            #region TreatmentPlanCreation
            //TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            //TreatmentPlan SaveTreatmentPlan = null;
            //TreatmentPlan UpdateTreatmentPlan = null;
            //if (EncounterID != 0)
            //{
            //    IList<TreatmentPlan> Treatment_Plan = new List<TreatmentPlan>();
            //    ulong ulHumanID = GetHumanID(saveList, null, null);
            //    Treatment_Plan = objTreatmentPlanMgr.GetTreatmentPlanUsingEncounterId(EncounterID, ulHumanID);
            //    IList<TreatmentPlan> Treatmentlst = (from t in Treatment_Plan where t.Plan_Type.ToUpper() == orderType.ToUpper() select t).ToList<TreatmentPlan>();
            //    if (Treatmentlst.Count == 0)
            //    {
            //        SaveTreatmentPlan = new TreatmentPlan();
            //        SaveTreatmentPlan.Encounter_Id = EncounterID;
            //        SaveTreatmentPlan.Human_ID = ulHumanID; //saveList[0].Human_ID;
            //        if (objOrderSubmit.Count > 0)
            //            SaveTreatmentPlan.Physician_Id = objOrderSubmit[0].Physician_ID;//saveList[0].Physician_ID;
            //        SaveTreatmentPlan.Plan_Type = orderType.ToUpper();
            //        SaveTreatmentPlan.Plan = PlanString(sOrderingProcedure, "");
            //        if (objOrderSubmit.Count > 0)
            //        {
            //            SaveTreatmentPlan.Created_By = objOrderSubmit[0].Created_By;//saveList[0].Created_By;
            //            SaveTreatmentPlan.Created_Date_And_Time = objOrderSubmit[0].Created_Date_And_Time;//saveList[0].Created_Date_And_Time;
            //        }

            //    }
            //    else if (Treatmentlst.Count > 0)
            //    {
            //        UpdateTreatmentPlan = new TreatmentPlan();
            //        UpdateTreatmentPlan = Treatmentlst[0];
            //        UpdateTreatmentPlan.Plan = PlanString(sOrderingProcedure, Treatmentlst[0].Plan);
            //        if (objOrderSubmit.Count > 0)
            //        {
            //            UpdateTreatmentPlan.Modified_By = objOrderSubmit[0].Created_By;//saveList[0].Modified_By;
            //            UpdateTreatmentPlan.Modified_Date_And_Time = objOrderSubmit[0].Created_Date_And_Time;//saveList[0].Modified_Date_And_Time;
            //        }
            //    }
            //}
            #endregion
            #region SaveSpecimen
            //Session.GetISession().Clear();

            ISession MySession = Session.GetISession();
            int iResult = 0;
            //ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        // trans = MySession.BeginTransaction();
                        if (saveList != null)
                        {
                            if (saveList.Count > 0)
                            {
                                //if (specimenList != null)
                                //{
                                //    if (specimenList.Count > 0)
                                //    {
                                //        SpecimenManager objSpecManager = new SpecimenManager();
                                //        iResult = objSpecManager.BatchOperationToSpecimen(ref specimenList, null, null, MySession, MACAddress);
                                //        if (iResult == 2)
                                //        {
                                //            if (iTryCount < 5)
                                //            {
                                //                iTryCount++;
                                //                goto TryAgain;
                                //            }
                                //            else
                                //            {
                                //                trans.Rollback();
                                //                // MySession.Close();
                                //                throw new Exception("Deadlock occurred. Transaction failed.");
                                //            }
                                //        }
                                //        else if (iResult == 1)
                                //        {
                                //            trans.Rollback();
                                //            // MySession.Close();
                                //            throw new Exception("Exception occurred. Transaction failed.");
                                //        }
                                //        //foreach (OrdersSubmit obj in objOrderSubmit)
                                //        //    obj.Specimen_ID = specimenList[0].Id;
                                //    }
                                //}
                                #endregion
                                #region SaveOrderSubmit
                                if (objOrderSubmit[0].Internal_Property_Update_Phone_Number != string.Empty)
                                {
                                    HumanManager objHumanManger = new HumanManager();
                                    objHumanManger.UpdateHumanHomePhoneNumber(objOrderSubmit[0].Human_ID, objOrderSubmit[0].Internal_Property_Update_Phone_Number, MySession, MACAddress);
                                }
                                IList<OrdersSubmit> objOrderSubmitnull = null;
                                if (saveList.Count > 0)
                                    humanid = saveList[0].Human_ID;
                                //iResult = objOrdersSubmitManager.SaveUpdateDeleteWithoutTransaction(ref objOrderSubmit, null, null, MySession, MACAddress);
                                iResult = objOrdersSubmitManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref objOrderSubmit, ref objOrderSubmitnull, null, MySession, MACAddress, true, true, humanid, string.Empty, ref XMLObj);
                                ulong ulSubmitID = 0;
                                if (objOrderSubmit.Count > 0)
                                {
                                    string sTestDate = "";
                                    if (objOrderSubmit[0].Test_Date.Trim() != "")
                                        sTestDate = Convert.ToDateTime(objOrderSubmit[0].Test_Date).ToString("dd-MMM-yyyy");
                                    sSelectedOrder = objOrderSubmit[0].Id.ToString() + " | " + sTestDate;
                                    ulSubmitID = objOrderSubmit[0].Id;
                                }

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

                                if (objOrderSubmit != null)
                                    IsOrders1 = XMLObj.CheckDataConsistency(objOrderSubmit.Cast<object>().ToList(), true, string.Empty);

                                #endregion
                                #region SaveOrders
                                StaticLookupManager objstaticlookupManager = new StaticLookupManager();

                                string CMGStaticLookupValue = objstaticlookupManager.getStaticLookupByFieldName("CMG LAB NAME").Select(a => a.Value).SingleOrDefault();
                                Queue<ulong> OrdersSubmitIDs = new Queue<ulong>();
                                foreach (OrdersSubmit temp in objOrderSubmit)
                                {
                                    OrdersSubmitIDs.Enqueue(temp.Id);
                                }
                                foreach (Orders rec in saveList)
                                {
                                    //if (!rec.Order_Code_Type.StartsWith(CMGStaticLookupValue.ToUpper()))//Commentted for CMG Ancillary
                                    //{
                                    var crit = from rec1 in objOrderSubmit where rec1.Order_Code_Type == rec.Order_Code_Type select rec1;
                                    foreach (OrdersSubmit temp in crit)
                                    {
                                        rec.Order_Submit_ID = temp.Id;
                                    }
                                    //}
                                    //else
                                    //{
                                    //    rec.Order_Submit_ID = OrdersSubmitIDs.Dequeue();
                                    //}
                                }
                                IList<Orders> saveListnull = null;
                                //iResult = SaveUpdateDeleteWithoutTransaction(ref saveList, null, null, MySession, MACAddress);
                                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref saveListnull, null, MySession, MACAddress, true, true, humanid, string.Empty, ref XMLObj);
                                if (saveList.Count() > 0)
                                {
                                    for (int k = 0; k < saveList.Count; k++)
                                    {
                                        if (k == 0)
                                            sSelectedOrder += " | [" + saveList[k].Lab_Procedure + " - " + saveList[k].Lab_Procedure_Description;
                                        else
                                            sSelectedOrder += " ; " + saveList[k].Lab_Procedure + " - " + saveList[k].Lab_Procedure_Description;
                                        if (k == saveList.Count)
                                            sSelectedOrder += "]";
                                    }
                                }

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
                                if (saveList != null)
                                    IsOrders2 = XMLObj.CheckDataConsistency(saveList.Cast<object>().ToList(), true, string.Empty);
                                #endregion
                                #region Assessment
                                if (ordAssList != null)
                                {
                                    if (ordAssList.Count > 0)
                                    {
                                        foreach (OrdersAssessment obj in ordAssList)// for bug ID 52741 
                                        {
                                            //obj.Order_ID = saveList[Convert.ToInt32(obj.Order_ID)].Id; //Commentted for Bug : 56642
                                            //obj.Order_Submit_ID = objOrderSubmit[0].Id;//Add for Bug : 56642
                                            obj.Order_Submit_ID = objOrderSubmit[Convert.ToInt32(obj.Order_Submit_ID)].Id;
                                        }
                                        OrdersAssessmentManager objManager = new OrdersAssessmentManager();
                                        IList<OrdersAssessment> ListUpdate = null;
                                        iResult = objManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordAssList, ref ListUpdate, null, MySession, MACAddress, true, true, humanid, string.Empty, ref XMLObj);
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
                                }
                                if (ordAssList != null)
                                    IsOrders3 = XMLObj.CheckDataConsistency(ordAssList.Cast<object>().ToList(), true, string.Empty);
                                #endregion
                                //if (objBloodLead != null)
                                //{
                                //    IList<OrdersQuestionSetBloodLead> bldList = new List<OrdersQuestionSetBloodLead>();
                                //    IList<Orders> objList = (from obj in saveList where obj.Orders_Question_Set_Segment.ToUpper() == "ZBL" select obj).ToList<Orders>();
                                //    if (objList != null && objList.Count > 0)
                                //    {
                                //        OrdersQuestionSetBloodLead bld = null;
                                //        foreach (Orders objOrdBL in objList)
                                //        {
                                //            bld = new OrdersQuestionSetBloodLead(objBloodLead);
                                //            bld.Order_ID = objOrdBL.Id;
                                //            bldList.Add(bld);
                                //        }
                                //    }
                                //    OrdersQuestionSetBloodLeadManager bldLeadManager = new OrdersQuestionSetBloodLeadManager();

                                //    //bldList.Add(objBloodLead);
                                //    iResult = bldLeadManager.BatchoperationsToOrdersQuestionSetBloodLead(bldList, null, null, MySession, MACAddress);
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
                                //        // MySession.Close();
                                //        throw new Exception("Exception occurred. Transaction failed.");
                                //    }
                                //}
                                //if (objAFP != null)
                                //{
                                //    IList<OrdersQuestionSetAfp> afpList = new List<OrdersQuestionSetAfp>();
                                //    IList<Orders> objList = (from obj in saveList where obj.Orders_Question_Set_Segment.ToUpper() == "ZSA" select obj).ToList<Orders>();
                                //    if (objList != null && objList.Count > 0)
                                //    {
                                //        OrdersQuestionSetAfp afp = null;
                                //        foreach (Orders objOrdAFP in objList)
                                //        {
                                //            afp = new OrdersQuestionSetAfp(objAFP);
                                //            afp.Order_ID = objOrdAFP.Id;
                                //            afpList.Add(afp);
                                //        }
                                //    }
                                //    OrdersQuestionSetAfpManager afpManager = new OrdersQuestionSetAfpManager();

                                //    //afpList.Add(objAFP);
                                //    iResult = afpManager.BatchOperationsToOrdersQuestionSetAfp(afpList, null, null, MySession, MACAddress);
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
                                //        // MySession.Close();
                                //        throw new Exception("Exception occurred. Transaction failed.");
                                //    }
                                //}
                                //if (objCytology != null)
                                //{

                                //    IList<OrdersQuestionSetCytology> cytoList = new List<OrdersQuestionSetCytology>();
                                //    IList<Orders> objList = (from obj in saveList where obj.Orders_Question_Set_Segment.ToUpper() == "ZCY" select obj).ToList<Orders>();
                                //    if (objList != null && objList.Count > 0)
                                //    {
                                //        OrdersQuestionSetCytology cytology = null;
                                //        foreach (Orders objOrdCY in objList)
                                //        {
                                //            cytology = new OrdersQuestionSetCytology(objCytology);
                                //            cytology.Order_ID = objOrdCY.Id;
                                //            cytoList.Add(cytology);
                                //        }
                                //    }
                                //    OrdersQuestionSetCytologyManager cytoManager = new OrdersQuestionSetCytologyManager();

                                //    // cytoList.Add(objCytology);
                                //    iResult = cytoManager.BatchOperationsToOrdersQuestionSetCytology(cytoList, null, null, MySession, MACAddress);
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
                                //            //MySession.Close();
                                //            throw new Exception("Deadlock occurred. Transaction failed.");
                                //        }
                                //    }
                                //    else if (iResult == 1)
                                //    {
                                //        trans.Rollback();
                                //        // MySession.Close();
                                //        throw new Exception("Exception occurred. Transaction failed.");
                                //    }
                                //}

                                //Uncommented by saravanan
                                if (ilistOrdersRequiredForms != null && ilistOrdersRequiredForms.Count > 0)
                                {
                                    OrdersRequiredFormsManager objOrdersRequiredFormsManager = new OrdersRequiredFormsManager();
                                    foreach (OrdersRequiredForms obj in ilistOrdersRequiredForms)
                                    {
                                        obj.Order_Id = saveList.Where(a => a.Lab_Procedure == obj.CPT).Select(a => a.Id).SingleOrDefault();
                                        obj.Created_By = saveList.Where(a => a.Id == obj.Order_Id).Select(a => a.Created_By).SingleOrDefault();
                                        obj.Created_Date_And_Time = saveList.Where(a => a.Id == obj.Order_Id).Select(a => a.Created_Date_And_Time).SingleOrDefault();
                                        obj.Modified_By = saveList.Where(a => a.Id == obj.Order_Id).Select(a => a.Modified_By).SingleOrDefault();
                                        obj.Modified_Date_And_Time = saveList.Where(a => a.Id == obj.Order_Id).Select(a => a.Modified_Date_And_Time).SingleOrDefault();
                                    }
                                    iResult = objOrdersRequiredFormsManager.BatchOperationsToOrdersRequiredForms(ilistOrdersRequiredForms, null, null, MySession, MACAddress);
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

                                //if (AOESaveList != null && AOESaveList.Count > 0)
                                //{
                                //    OrdersQuestionSetAOEManager objOrdersQuestionSetAOEManager = new OrdersQuestionSetAOEManager();
                                //    foreach (OrdersQuestionSetAOE dctent in AOESaveList)
                                //    {
                                //        var DetailsForChildTable = (from rec in saveList where rec.Lab_Procedure == dctent.Order_Code select rec.Id).SingleOrDefault();
                                //        dctent.Orders_ID = Convert.ToUInt64(DetailsForChildTable);
                                //    }
                                //    iResult = objOrdersQuestionSetAOEManager.BatchOperationsToOrdersQuestionSetAOE(AOESaveList, null, null, MySession, MACAddress);
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
                                //            //MySession.Close();
                                //            throw new Exception("Deadlock occurred. Transaction failed.");
                                //        }
                                //    }
                                //    else if (iResult == 1)
                                //    {
                                //        trans.Rollback();
                                //        // MySession.Close();
                                //        throw new Exception("Exception occurred. Transaction failed.");
                                //    }
                                //}
                                //if (objStaOrder != null)
                                //{
                                //    StandingOrderManager objMngr = new StandingOrderManager();
                                //    IList<StandingOrder> save = new List<StandingOrder>();
                                //    objStaOrder.Order_ID = saveList[0].Id;
                                //    save.Add(objStaOrder);
                                //    iResult = objMngr.BatchOperationsToStandingOrder(save, null, null, MySession, MACAddress);
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
                                //        // MySession.Close();
                                //        throw new Exception("Exception occurred. Transaction failed.");
                                //    }
                                //}
                            }
                        }

                        #region Treatment plan

                        if (saveList.Count > 0 && saveList[0].Encounter_ID != 0)
                        {
                            foreach (Orders obj in saveList)
                            {
                                TreatmentPlan item = new TreatmentPlan();
                                item.Human_ID = obj.Human_ID;
                                item.Encounter_Id = obj.Encounter_ID;
                                item.Physician_Id = obj.Physician_ID;
                                item.Created_By = obj.Created_By;
                                item.Created_Date_And_Time = obj.Created_Date_And_Time;
                                item.Plan_Type = "DIAGNOSTIC ORDER";
                                string plan_txt = string.Empty;
                                plan_txt = "* " + obj.Lab_Procedure_Description;
                                item.Plan = plan_txt;
                                item.Version = 0;
                                item.Source_ID = obj.Id;
                                item.Local_Time = sLocalTime;
                                Insert_Tplan.Add(item);
                            }

                            iResult = objTreatmentPlanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, null, MySession, MACAddress, true, true, Insert_Tplan[0].Encounter_Id, string.Empty, ref XmlObjEnc);

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
                                    MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }

                            //if (SaveTreatmentPlan != null || UpdateTreatmentPlan != null)
                            //{                        
                            //    iResult = objTreatmentPlanMgr.SaveUpdateorDeleteTreatmentPlan(SaveTreatmentPlan, UpdateTreatmentPlan, null, MySession, MACAddress);
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
                            //            MySession.Close();
                            //            throw new Exception("Deadlock occurred. Transaction failed.");
                            //        }
                            //    }
                            //    else if (iResult == 1)
                            //    {
                            //        trans.Rollback();
                            //        MySession.Close();
                            //        throw new Exception("Exception occurred. Transaction failed.");
                            //    }

                            //}
                        }

                        #endregion

                        #region CreateAndInsertIntoWorkFlow
                        foreach (OrdersSubmit objSubmit in objOrderSubmit)
                        {
                            WFObject WFObj = new WFObject();
                            EncounterManager encMngr = new EncounterManager();
                            Encounter EncRecord = encMngr.GetById(objSubmit.Encounter_ID);
                            WFObj.Obj_Type = objSubmit.Order_Type.ToUpper();
                            WFObj.Current_Arrival_Time = objSubmit.Created_Date_And_Time;
                            if (EncRecord != null && EncRecord.Assigned_Med_Asst_User_Name != string.Empty)
                                WFObj.Current_Owner = EncRecord.Assigned_Med_Asst_User_Name;
                            else
                                WFObj.Current_Owner = "UNKNOWN";
                            WFObj.Fac_Name = objSubmit.Facility_Name;
                            WFObj.Parent_Obj_Type = string.Empty;
                            WFObj.Obj_System_Id = objSubmit.Id;
                            WFObj.Current_Process = "START";
                            WFObjectManager objWfMnger = new WFObjectManager();

                            if (objSubmit.Move_To_MA == "Y")
                            {
                                iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
                            }
                            else
                            {
                                WFObj.Current_Owner = "UNKNOWN";
                                iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 2, MACAddress, MySession);
                            }



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

                        #endregion


                        // MySession.Flush();
                        //trans.Commit();
                        if (IsOrders1 && IsOrders2 && IsOrders3)
                        {
                            //trans.Commit();
                            //if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                            if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                            {
                                // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                //int trycount = 0;
                                //trytosaveagain:
                                try
                                {
                                    #region "code comitted by Balaji.TJ 2023-01-08"
                                    //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    #endregion

                                    #region "code Modified by Balaji.TJ 2023-01-08"
                                    WriteBlob(humanid, XMLObj.itemDoc, MySession, saveList, null, null, XMLObj, false);
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
                            }

                            if (XmlObjEnc.itemDoc.InnerXml != null && XmlObjEnc.itemDoc.InnerXml != "")
                            {
                                //XmlObjEnc.itemDoc.Save(XmlObjEnc.strXmlFilePath);

                                //int trycount = 0;
                                //trytosaveagain:
                                try
                                {
                                    #region "code comitted by Balaji.TJ 2023-01-08"
                                    //XmlObjEnc.itemDoc.Save(XmlObjEnc.strXmlFilePath);
                                    #endregion
                                    #region "code Modified by Balaji.TJ 2023-01-08"
                                    //WriteBlob(EncounterID, XMLObj.itemDoc, MySession, saveList, null, null, XMLObj, false);
                                    objTreatmentPlanManager.WriteBlob(EncounterID, XmlObjEnc.itemDoc, MySession, Insert_Tplan, null, null, XmlObjEnc, false);
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
                            }
                            trans.Commit();
                        }
                        else
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
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

            //GenerateXml XMLObj = new GenerateXml(); //code comment by balaji.T
            //ulong humanid = 0;
            //if (saveList.Count > 0)
            //{

            //    humanid = saveList[0].Human_ID;
            //    List<object> lstObj = saveList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSaveStatic(lstObj, humanid, string.Empty);
            //}
            //if (objOrderSubmit.Count > 0)
            //{
            //    humanid = objOrderSubmit[0].Human_ID;

            //    List<object> lstObj = objOrderSubmit.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSaveStatic(lstObj, humanid, string.Empty);


            //}
            //if (ordAssList.Count > 0)
            //{
            //    List<object> lstObj = ordAssList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSaveStatic(lstObj, humanid, string.Empty);

            //}

            //return new MemoryStream();
            //if (EncounterID != 0)
            //{
            //    return GetOrdersUsingEncounterID(EncounterID, orderType, false);
            //}
            //else
            //{
            //    return GetOrdersUsingHumanID(saveList[0].Human_ID, saveList[0].Created_Date_And_Time, orderType, saveList[0].Physician_ID, false);
            //}
            return 0;
        }

        public Stream DeleteOrders(IList<OrderLabDetailsDTO> delList, IList<OrdersAssessment> ordAssList, ulong EncounterID, string orderType, string MACAddress, IList<string> PlanLst)
        {
            GenerateXml XMLObj = new GenerateXml();
            IList<Orders> delOrderList = new List<Orders>();
            IList<OrdersQuestionSetAfp> listAFP = new List<OrdersQuestionSetAfp>();
            IList<OrdersQuestionSetBloodLead> listBloodLead = new List<OrdersQuestionSetBloodLead>();
            IList<OrdersQuestionSetCytology> listCyto = new List<OrdersQuestionSetCytology>();
            IList<OrdersQuestionSetAOE> listAOE = new List<OrdersQuestionSetAOE>();
            //IList<StandingOrder> listStandingOrder = new List<StandingOrder>();
            foreach (OrderLabDetailsDTO obj in delList)
            {
                delOrderList.Add(obj.ObjOrder);
                if (obj.objAFP != null)
                    listAFP.Add(obj.objAFP);
                if (obj.objBloodLead != null)
                    listBloodLead.Add(obj.objBloodLead);
                if (obj.objCytology != null)
                    listCyto.Add(obj.objCytology);
                if (obj.OrderAOEList != null)
                    foreach (OrdersQuestionSetAOE objAOE in obj.OrderAOEList)
                        listAOE.Add(objAOE);
            }
            IList<Orders> saveList = null;

            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();

            iTryCount = 0;
            #region TplanGet

            IList<string> ilstGeneralPlanTagList = new List<string>();
            ilstGeneralPlanTagList.Add("TreatmentPlanList");


            IList<object> ilstGeneralPlanBlobFinal = new List<object>();
            ilstGeneralPlanBlobFinal = ReadBlob(EncounterID, ilstGeneralPlanTagList);
            if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
            {
                if (ilstGeneralPlanBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                    {
                        objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                    }
                }
            }
        //string FileName = "Encounter" + "_" + EncounterID + ".xml";
        //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
        //XmlTextReader XmlText = null;
        //try
        //{
        //    if (File.Exists(strXmlFilePath) == true)
        //    {
        //        XmlDocument itemDoc = new XmlDocument();
        //        XmlText = new XmlTextReader(strXmlFilePath);
        //        XmlNodeList xmlTagName = null;
        //        //itemDoc.Load(XmlText);
        //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        {
        //            itemDoc.Load(fs);

        //            XmlText.Close();
        //            #region Treatment_plan
        //            if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
        //            {
        //                xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

        //                if (xmlTagName.Count > 0)
        //                {
        //                    for (int j = 0; j < xmlTagName.Count; j++)
        //                    {
        //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("DIAGNOSTIC ORDER"))
        //                        {

        //                            string TagName = xmlTagName[j].Name;
        //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
        //                            TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
        //                            IEnumerable<PropertyInfo> propInfo = null;
        //                            propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

        //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
        //                            {

        //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
        //                                {
        //                                    foreach (PropertyInfo property in propInfo)
        //                                    {
        //                                        if (property.Name == nodevalue.Name)
        //                                        {
        //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
        //                                                property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
        //                                                property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
        //                                                property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
        //                                                property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
        //                                            else
        //                                                property.SetValue(TreatmentPlan, nodevalue.Value, null);
        //                                        }
        //                                    }
        //                                }

        //                            }
        //                            objTreatmentPlan.Add(TreatmentPlan);
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            fs.Close();
        //            fs.Dispose();
        //        }
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    if (XmlText != null)
        //        XmlText.Close();
        //    throw Ex;
        //}
        #endregion

        TryAgain:
            int iResult = 0;
            //TreatmentPlan UpdateTreatmentPlan = null;
            //TreatmentPlan DeleteTreatmentPlan = null;
            //if (EncounterID != 0)
            //{
            //    TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            //    IList<TreatmentPlan> Treatment_Plan = new List<TreatmentPlan>();
            //    Treatment_Plan = objTreatmentPlanMgr.GetTreatmentPlanUsingEncounterId(EncounterID, delList[0].ObjOrder.Human_ID);
            //    IList<TreatmentPlan> Treatmentlst = (from t in Treatment_Plan where t.Plan_Type.ToUpper() == orderType.ToUpper() select t).ToList<TreatmentPlan>();
            //    if (Treatmentlst.Count > 0)
            //    {
            //        string sPlanValue = Treatmentlst[0].Plan;
            //        foreach (string s in PlanLst)
            //        {
            //            sPlanValue = sPlanValue.Replace(s.Trim(), " ").Trim();
            //        }
            //        if (sPlanValue != string.Empty)
            //        {
            //            UpdateTreatmentPlan = new TreatmentPlan();
            //            UpdateTreatmentPlan = Treatmentlst[0];
            //            UpdateTreatmentPlan.Plan = sPlanValue;
            //            UpdateTreatmentPlan.Modified_By = ordAssList[0].Modified_By;
            //            UpdateTreatmentPlan.Modified_Date_And_Time = ordAssList[0].Modified_Date_And_Time;
            //        }
            //        //IList<string> UpdateList = (from obj in orderList where (PlanID.Contains(obj.Id)==false) select obj.Lab_Procedure_Description + " - " + obj.Order_Notes).ToList<string>();
            //        //if (UpdateList.Count > 0)
            //        //    UpdateTreatmentPlan = UpdatePlan(UpdateList, Treatmentlst[0],new List<string>());
            //        else
            //        {
            //            DeleteTreatmentPlan = new TreatmentPlan();
            //            DeleteTreatmentPlan = Treatmentlst[0];
            //            DeleteTreatmentPlan.Modified_By = ordAssList[0].Modified_By;
            //            DeleteTreatmentPlan.Modified_Date_And_Time = ordAssList[0].Modified_Date_And_Time;
            //        }
            //    }
            //}

            Session.GetISession().Clear();

            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = MySession.BeginTransaction();

                if (ordAssList.Count > 0)
                {
                    OrdersAssessmentManager objManager = new OrdersAssessmentManager();
                    iResult = objManager.BatchOperationsToOrdersAssessment(null, null, ordAssList, MySession, MACAddress);
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

                if (listAFP.Count > 0)
                {

                    OrdersQuestionSetAfpManager objAFpManager = new OrdersQuestionSetAfpManager();

                    iResult = objAFpManager.BatchOperationsToOrdersQuestionSetAfp(null, null, listAFP, MySession, MACAddress);
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
                if (listBloodLead.Count > 0)
                {

                    OrdersQuestionSetBloodLeadManager objBloodLeadManager = new OrdersQuestionSetBloodLeadManager();
                    iResult = objBloodLeadManager.BatchoperationsToOrdersQuestionSetBloodLead(null, null, listBloodLead, MySession, MACAddress);
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
                            //   MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        //  MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (listCyto.Count > 0)
                {

                    OrdersQuestionSetCytologyManager objCytoManager = new OrdersQuestionSetCytologyManager();
                    iResult = objCytoManager.BatchOperationsToOrdersQuestionSetCytology(null, null, listCyto, MySession, MACAddress);
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
                        //  MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }

                if (delOrderList != null)
                {
                    if (delOrderList.Count > 0)
                    {
                        IList<Orders> saveListnull = null;
                        //iResult = SaveUpdateDeleteWithoutTransaction(ref saveList, null, delOrderList, MySession, MACAddress);
                        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref saveListnull, delOrderList, MySession, MACAddress, true, true, 0, string.Empty, ref XMLObj);
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
                                //   MySession.Close();
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
                if (listAOE != null)
                {
                    OrdersQuestionSetAOEManager objOrdersQuestionSetAOEManager = new OrdersQuestionSetAOEManager();
                    if (listAOE.Count > 0)
                    {
                        iResult = objOrdersQuestionSetAOEManager.BatchOperationsToOrdersQuestionSetAOE(null, null, listAOE, MySession, MACAddress);
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
                                //   MySession.Close();
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
                #region Treatment
                Delete_Tplan = (from obj in objTreatmentPlan where delList.Any(a => a.ObjOrder.Id == obj.Source_ID) select obj).ToList<TreatmentPlan>();
                if (Delete_Tplan != null && Delete_Tplan.Count > 0)
                {
                    iResult = objTreatmentPlanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, Delete_Tplan, MySession, MACAddress, true, true, EncounterID, string.Empty, ref XMLObj);

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
                            MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                //if (UpdateTreatmentPlan != null || DeleteTreatmentPlan != null)
                //{
                //    TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
                //    iResult = objTreatmentPlanMgr.SaveUpdateorDeleteTreatmentPlan(null, UpdateTreatmentPlan, DeleteTreatmentPlan, MySession, MACAddress);
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
                //            MySession.Close();
                //            throw new Exception("Deadlock occurred. Transaction failed.");
                //        }
                //    }
                //    else if (iResult == 1)
                //    {
                //        trans.Rollback();
                //        MySession.Close();
                //        throw new Exception("Exception occurred. Transaction failed.");
                //    }

                //}

                #endregion
                //MySession.Flush();
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

            if (EncounterID != 0)
            {
                return GetOrdersUsingEncounterID(EncounterID, orderType, true);
            }
            else
            {
                return GetOrdersUsingHumanID(delOrderList[0].Human_ID, delOrderList[0].Created_Date_And_Time, orderType, delOrderList[0].Physician_ID, true);
            }
        }

        public ulong UpdateOrders(IList<Orders> saveList, IList<Orders> updtList, IList<Orders> delList, IList<OrdersAssessment> savOrdAssListForExistingCPT, IList<OrdersAssessment> delOrdAssListForExistingCPT, IList<OrdersAssessment> savOrdAssListForNewCPT, IList<OrdersQuestionSetAfp> afpList, IList<OrdersQuestionSetBloodLead> bloodLeadList, IList<OrdersQuestionSetCytology> cytologyList, TreatmentPlan UpdatePlan, IList<string> PlanDel, IList<string> SelectedProcedures, IList<string> PlanReplace, ulong EncounterID, string orderType, string MACAddress, IList<OrdersQuestionSetAOE> AOESaveList, IList<OrdersSubmit> SavOrderSubmit, IList<OrdersSubmit> UpdOrderSubmit, IList<OrdersSubmit> DelOrderSubmit, IList<OrdersRequiredForms> SaveListOrdersRequiredForms, IList<OrdersRequiredForms> UpdateListOrdersRequiredForms, IList<OrdersRequiredForms> DeleteListOrdersRequiredForms, string sLocalTime)//, Dictionary<string, IList<OrdersQuestionSetAOE>> AOESaveList)
        {

            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();

            //TreatmentPlan UpdateTreatmentPlan = null;
            ulong uHumanId = 0;
            IList<OrdersSubmit> templist = new List<OrdersSubmit>();
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XmlObjEnc = new GenerateXml();
            bool isOrder = true, isOrderSubmit = true, isOrderAssessmnt = true;
            string ModifiedBy = string.Empty;
            //DateTime ModifiedDAT = new DateTime();

            #region TplanGet

            ulong uEncounterID = EncounterID;
            IList<string> ilstEncounterTag = new List<string>();
            ilstEncounterTag.Add("TreatmentPlanList");

            IList<object> ilstEncounterBlobList = new List<object>();

            //if (uEncounterID != null && uEncounterID != 0)
            {
                ilstEncounterBlobList = ReadBlob(EncounterID, ilstEncounterTag);

                if (ilstEncounterBlobList != null && ilstEncounterBlobList.Count > 0)
                {
                    if (ilstEncounterBlobList[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEncounterBlobList[0]).Count; iCount++)
                        {
                            if (((TreatmentPlan)((IList<object>)ilstEncounterBlobList[0])[iCount]).Encounter_Id == EncounterID && ((TreatmentPlan)((IList<object>)ilstEncounterBlobList[0])[iCount]).Plan_Type == "DIAGNOSTIC ORDER")
                            {
                                objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstEncounterBlobList[0])[iCount]);
                            }
                        }
                    }
                }

            }
            //string FileName = "Encounter" + "_" + EncounterID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    // itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();
            //        #region Treatment_plan
            //        if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("DIAGNOSTIC ORDER"))
            //                    {

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
            //                        TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {

            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(TreatmentPlan, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }

            //                        }
            //                        objTreatmentPlan.Add(TreatmentPlan);
            //                    }
            //                }
            //            }
            //        }
            //        #endregion
            //        fs.Close();
            //        fs.Dispose();

            //    }
            //}



            #endregion

            //if (EncounterID != 0)
            //{
            //    string sPlanText = string.Empty;
            //    TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            //    IList<TreatmentPlan> Treatment_Plan = new List<TreatmentPlan>();
            //    ulong ulHumanID = GetHumanID(saveList, updtList, delList);
            //    Treatment_Plan = objTreatmentPlanMgr.GetTreatmentPlanUsingEncounterId(EncounterID, ulHumanID);
            //    IList<TreatmentPlan> Treatmentlst = (from t in Treatment_Plan where t.Plan_Type.ToUpper() == orderType.ToUpper() select t).ToList<TreatmentPlan>();
            //    if (Treatmentlst.Count > 0)
            //    {
            //        UpdateTreatmentPlan = new TreatmentPlan();
            //        UpdateTreatmentPlan = Treatmentlst[0];
            //        sPlanText = UpdateTreatmentPlan.Plan;
            //        IList<string> NewCPTs = new List<string>();
            //        for (int i = 0; i < saveList.Count; i++)
            //        {
            //            if (i == 0)
            //            {
            //                ModifiedBy = saveList[i].Modified_By;
            //                ModifiedDAT = saveList[i].Modified_Date_And_Time;
            //            }
            //            NewCPTs.Add(saveList[i].Lab_Procedure_Description);
            //        }
            //        sPlanText = PlanString(NewCPTs, sPlanText);
            //        for (int j = 0; j < delList.Count; j++)
            //        {
            //            if (j == 0)
            //            {
            //                ModifiedBy = delList[j].Modified_By;
            //                ModifiedDAT = delList[j].Modified_Date_And_Time;
            //            }
            //            sPlanText = sPlanText.Replace(Environment.NewLine + "*" + delList[j].Lab_Procedure_Description, "");
            //            sPlanText = sPlanText.Replace("*" + delList[j].Lab_Procedure_Description, "");
            //        }
            //        for (int i = 0; i < updtList.Count; i++)
            //        {
            //            if (i == 0)
            //            {
            //                ModifiedBy = updtList[i].Modified_By;
            //                ModifiedDAT = updtList[i].Modified_Date_And_Time;
            //            }
            //        }
            //        UpdateTreatmentPlan.Modified_By = ModifiedBy;
            //        UpdateTreatmentPlan.Modified_Date_And_Time = ModifiedDAT;
            //        UpdateTreatmentPlan.Plan = PlanString(NewCPTs, sPlanText);
            //    }
            //}

            ISession MySession = Session.GetISession();
            if (SavOrderSubmit.Count > 0)
            {
                if (SavOrderSubmit[0].Internal_Property_Update_Phone_Number != string.Empty)
                {
                    HumanManager objHumanManger = new HumanManager();
                    objHumanManger.UpdateHumanHomePhoneNumber(SavOrderSubmit[0].Human_ID, SavOrderSubmit[0].Internal_Property_Update_Phone_Number, MySession, MACAddress);
                }
            }
            else if (UpdOrderSubmit.Count > 0)
            {
                if (UpdOrderSubmit[0].Internal_Property_Update_Phone_Number != string.Empty)
                {
                    HumanManager objHumanManger = new HumanManager();
                    objHumanManger.UpdateHumanHomePhoneNumber(UpdOrderSubmit[0].Human_ID, UpdOrderSubmit[0].Internal_Property_Update_Phone_Number, MySession, MACAddress);
                }
            }
            StaticLookupManager objstaticlookupManager = new StaticLookupManager();
            string CMGStaticLookupValue = objstaticlookupManager.getStaticLookupByFieldName("CMG LAB NAME").Select(a => a.Value).SingleOrDefault();

            iTryCount = 0;

        TryAgain:
            int iResult = 0;


            ITransaction trans = null;
            try
            {
                trans = MySession.BeginTransaction();

                if (SavOrderSubmit.Count > 0)
                {
                    SavOrderSubmit[0].Is_Task_Created = "N";
                    uHumanId = SavOrderSubmit[0].Human_ID;
                }
                else if (UpdOrderSubmit.Count > 0)
                {
                    UpdOrderSubmit[0].Is_Task_Created = "N";
                    uHumanId = UpdOrderSubmit[0].Human_ID;
                }
                iResult = objOrdersSubmitManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SavOrderSubmit, ref UpdOrderSubmit, null, MySession, MACAddress, true, true, uHumanId, string.Empty, ref XMLObj);
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

                if (SavOrderSubmit != null && SavOrderSubmit.Count > 0 && UpdOrderSubmit != null && UpdOrderSubmit.Count > 0)
                    isOrderSubmit = XMLObj.CheckDataConsistency(SavOrderSubmit.Concat(UpdOrderSubmit).Cast<object>().ToList(), true, string.Empty);
                else if ((SavOrderSubmit != null && SavOrderSubmit.Count > 0) || (UpdOrderSubmit != null && UpdOrderSubmit.Count > 0))
                {
                    if (SavOrderSubmit != null && SavOrderSubmit.Count > 0)
                        isOrderSubmit = XMLObj.CheckDataConsistency(SavOrderSubmit.Cast<object>().ToList(), true, string.Empty);
                    else if (UpdOrderSubmit != null && UpdOrderSubmit.Count > 0)
                        isOrderSubmit = XMLObj.CheckDataConsistency(UpdOrderSubmit.Cast<object>().ToList(), true, string.Empty);
                }
                if (saveList.Count > 0 || delList.Count > 0 || updtList.Count > 0)
                {
                    //if (updtList.Count > 0)
                    //    updtList[0].Order_Submit_ID = UpdOrderSubmit[0].Id;
                    if (saveList.Count > 0)
                    {
                        foreach (OrdersSubmit rec in SavOrderSubmit.Concat(UpdOrderSubmit))
                        {
                            foreach (var rec1 in saveList.Where(a => a.Order_Code_Type == rec.Order_Code_Type))
                            {
                                rec1.Order_Submit_ID = rec.Id;
                            }
                        }
                    }
                    if (delList.Count > 0)
                    {
                        delList = delList.Except(delList.Where(a => DelOrderSubmit.Select(b => b.Id).Contains(a.Order_Submit_ID)).ToList<Orders>()).ToList<Orders>();
                    }
                    if (saveList.Count > 0)
                    {
                        uHumanId = saveList[0].Human_ID;
                    }
                    else if (updtList.Count > 0)
                    {
                        uHumanId = updtList[0].Human_ID;
                    }
                    OrdersManager objOrdersManager = new OrdersManager();
                    //iResult = objOrdersManager.SaveUpdateDeleteWithoutTransaction(ref saveList, updtList, delList, MySession, MACAddress);
                    iResult = objOrdersManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref updtList, delList, MySession, MACAddress, true, true, uHumanId, string.Empty, ref XMLObj);
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
                    if (saveList != null && saveList.Count > 0 && updtList != null && updtList.Count > 0)
                        isOrder = XMLObj.CheckDataConsistency(saveList.Concat(updtList).Cast<object>().ToList(), true, string.Empty);
                    else if ((saveList != null && saveList.Count > 0) || (updtList != null && updtList.Count > 0))
                    {
                        if (saveList != null && saveList.Count > 0)
                            isOrder = XMLObj.CheckDataConsistency(saveList.Cast<object>().ToList(), true, string.Empty);
                        else if (updtList != null && updtList.Count > 0)
                            isOrder = XMLObj.CheckDataConsistency(updtList.Cast<object>().ToList(), true, string.Empty);
                    }
                }
                //Added For CMG-Lab Update Functionality

                if (SavOrderSubmit.Concat(UpdOrderSubmit).Any(a => a.Lab_Name.StartsWith(CMGStaticLookupValue)))
                {
                    string OriginalOrderCodeName = string.Empty;
                    foreach (OrdersSubmit obj in SavOrderSubmit.Concat(UpdOrderSubmit))
                    {
                        if (obj.Lab_Name.StartsWith(CMGStaticLookupValue))
                        {
                            OriginalOrderCodeName = obj.Lab_Name;
                            obj.Order_Code_Type = obj.Lab_Name;
                        }
                    }
                    foreach (Orders obj in saveList.Concat(updtList))
                    {
                        obj.Order_Code_Type = OriginalOrderCodeName;
                    }
                    IList<OrdersSubmit> tempOrdersSubmit = new List<OrdersSubmit>();
                    IList<Orders> tempOrders = new List<Orders>();

                    IList<OrdersSubmit> updOrdersSubmit = SavOrderSubmit.Concat(UpdOrderSubmit).ToList<OrdersSubmit>();
                    IList<Orders> updOrders = saveList.Concat(updtList).ToList<Orders>();
                    if (tempOrdersSubmit.Count > 0)
                    {
                        tempOrdersSubmit[0].Is_Task_Created = "N";
                        uHumanId = tempOrdersSubmit[0].Human_ID;
                    }
                    else if (updOrdersSubmit.Count > 0)
                    {
                        updOrdersSubmit[0].Is_Task_Created = "N";
                        uHumanId = updOrdersSubmit[0].Human_ID;
                    }
                    // iResult = objOrdersSubmitManager.SaveUpdateDeleteWithoutTransaction(ref tempOrdersSubmit, updOrdersSubmit, null, MySession, MACAddress);
                    iResult = objOrdersSubmitManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref tempOrdersSubmit, ref updOrdersSubmit, null, MySession, MACAddress, true, true, uHumanId, string.Empty, ref XMLObj);
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
                    OrdersManager objOrdersManager = new OrdersManager();
                    //  iResult = objOrdersManager.SaveUpdateDeleteWithoutTransaction(ref tempOrders, updOrders, null, MySession, MACAddress);
                    iResult = objOrdersManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref tempOrders, ref updOrders, null, MySession, MACAddress, true, true, uHumanId, string.Empty, ref XMLObj);
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

                if (savOrdAssListForExistingCPT.Count > 0 || delOrdAssListForExistingCPT.Count > 0 || savOrdAssListForNewCPT.Count > 0)
                {
                    //Added new
                    foreach (OrdersSubmit rec in UpdOrderSubmit)
                    {
                        //To Map Assessment and Orders . Assessment.Associated_Order_CPT property is used to hold proceudre which is mappend again with orders procedure to find order_id.
                        //foreach (OrdersAssessment rec1 in savOrdAssListForNewCPT.Where(a => a.Internal_Property_Associated_Order_CPT == rec.Lab_Procedure).ToList<OrdersAssessment>())
                        foreach (OrdersAssessment rec1 in savOrdAssListForNewCPT)
                        {
                            if (rec1.Order_Submit_ID != 0)
                                rec1.Order_Submit_ID = rec.Id;
                        }
                    }
                    //End new

                    //foreach (Orders rec in saveList.Concat(updtList))
                    //foreach (OrdersSubmit rec in SavOrderSubmit.Concat(UpdOrderSubmit))
                    foreach (OrdersSubmit rec in SavOrderSubmit)
                    {
                        //To Map Assessment and Orders . Assessment.Associated_Order_CPT property is used to hold proceudre which is mappend again with orders procedure to find order_id.
                        //foreach (OrdersAssessment rec1 in savOrdAssListForNewCPT.Where(a => a.Internal_Property_Associated_Order_CPT == rec.Lab_Procedure).ToList<OrdersAssessment>())
                        foreach (OrdersAssessment rec1 in savOrdAssListForNewCPT)
                        {
                            if (rec1.Order_Submit_ID == 0)
                                rec1.Order_Submit_ID = rec.Id;
                        }
                    }



                    OrdersAssessmentManager objManager = new OrdersAssessmentManager();
                    // iResult = objManager.BatchOperationsToOrdersAssessment(savOrdAssListForNewCPT, savOrdAssListForExistingCPT, delOrdAssListForExistingCPT, MySession, MACAddress);
                    iResult = objManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savOrdAssListForNewCPT, ref savOrdAssListForExistingCPT, delOrdAssListForExistingCPT, MySession, MACAddress, true, true, uHumanId, string.Empty, ref XMLObj);
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
                    if (savOrdAssListForNewCPT != null && savOrdAssListForNewCPT.Count > 0 && savOrdAssListForExistingCPT != null && savOrdAssListForExistingCPT.Count > 0)
                        isOrderAssessmnt = XMLObj.CheckDataConsistency(savOrdAssListForNewCPT.Concat(savOrdAssListForExistingCPT).Cast<object>().ToList(), true, string.Empty);
                    else if ((savOrdAssListForNewCPT != null && savOrdAssListForNewCPT.Count > 0) || (savOrdAssListForExistingCPT != null && savOrdAssListForExistingCPT.Count > 0))
                    {
                        if (savOrdAssListForNewCPT != null && savOrdAssListForNewCPT.Count > 0)
                            isOrderAssessmnt = XMLObj.CheckDataConsistency(savOrdAssListForNewCPT.Cast<object>().ToList(), true, string.Empty);
                        else if (savOrdAssListForExistingCPT != null && savOrdAssListForExistingCPT.Count > 0)
                            isOrderAssessmnt = XMLObj.CheckDataConsistency(savOrdAssListForExistingCPT.Cast<object>().ToList(), true, string.Empty);
                    }
                }
                {
                    IList<OrdersRequiredForms> saveOrderRequiredForms = SaveListOrdersRequiredForms;
                    IList<OrdersRequiredForms> UpdateOrderRequiredForms = UpdateListOrdersRequiredForms;
                    IList<OrdersRequiredForms> DeleteOrderRequiredForms = DeleteListOrdersRequiredForms;
                    foreach (OrdersRequiredForms obj in saveOrderRequiredForms)
                    {
                        IList<ulong> tempIDlist = saveList.Where(a => a.Lab_Procedure == obj.CPT).Select(a => a.Id).ToList<ulong>();
                        if (tempIDlist != null && tempIDlist.Count > 0)
                        {
                            obj.Order_Id = tempIDlist[0];
                        }

                    }
                    OrdersRequiredFormsManager objOrdersRequiredFormsManager = new OrdersRequiredFormsManager();
                    iResult = objOrdersRequiredFormsManager.BatchOperationsToOrdersRequiredForms(saveOrderRequiredForms, UpdateOrderRequiredForms, DeleteOrderRequiredForms, MySession, MACAddress);

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


                #region Tplan

                if (saveList != null && saveList.Count > 0)
                {
                    foreach (Orders obj in saveList)
                    {
                        TreatmentPlan item = new TreatmentPlan();
                        item.Human_ID = obj.Human_ID;
                        item.Encounter_Id = obj.Encounter_ID;
                        item.Physician_Id = obj.Physician_ID;
                        item.Created_By = obj.Created_By;
                        item.Created_Date_And_Time = obj.Created_Date_And_Time;
                        item.Plan_Type = "DIAGNOSTIC ORDER";
                        string plan_txt = string.Empty;
                        plan_txt = "* " + obj.Lab_Procedure_Description;
                        item.Plan = plan_txt;
                        item.Version = 0;
                        item.Source_ID = obj.Id;
                        item.Local_Time = sLocalTime;
                        Insert_Tplan.Add(item);
                    }
                }
                if (updtList != null && updtList.Count > 0)
                {
                    foreach (Orders obj in updtList)
                    {
                        if (objTreatmentPlan.Count > 0)
                        {
                            IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Source_ID == obj.Id).ToList();
                            if (itemlst != null && itemlst.Count > 0)
                            {
                                TreatmentPlan item = itemlst[0];
                                string plan_txt = string.Empty;
                                plan_txt = "* " + obj.Lab_Procedure_Description;
                                item.Plan = plan_txt;
                                item.Modified_By = obj.Modified_By;
                                item.Modified_Date_And_Time = obj.Modified_Date_And_Time;
                                Update_Tplan.Add(item);
                            }

                        }

                    }
                }

                if (delList != null && delList.Count > 0)
                    if (objTreatmentPlan.Count > 0)
                    {
                        Delete_Tplan = (from item in objTreatmentPlan where delList.Any(a => a.Id == item.Source_ID) select item).ToList();
                    }

                if ((Insert_Tplan.Count > 0 && Insert_Tplan[0].Encounter_Id != 0) || (Update_Tplan.Count > 0 && Update_Tplan[0].Encounter_Id != 0) || (Delete_Tplan.Count > 0 && Delete_Tplan[0].Encounter_Id != 0))
                {
                    iResult = objTreatmentPlanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, Delete_Tplan, MySession, MACAddress, true, true, EncounterID, string.Empty, ref XmlObjEnc);

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
                            MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }


                #endregion

                #region Treatment plan
                //if (EncounterID != 0 && UpdateTreatmentPlan != null)
                //{
                //    TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
                //    iResult = objTreatmentPlanMgr.SaveUpdateorDeleteTreatmentPlan(null, UpdateTreatmentPlan, null, MySession, MACAddress);
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
                //            MySession.Close();
                //            throw new Exception("Deadlock occurred. Transaction failed.");
                //        }
                //    }
                //    else if (iResult == 1)
                //    {
                //        trans.Rollback();
                //        MySession.Close();
                //        throw new Exception("Exception occurred. Transaction failed.");
                //    }

                //}
                #endregion

                #region CreateAndInsertIntoWorkFlow
                WFObjectManager objWfMnger = new WFObjectManager();
                foreach (OrdersSubmit objSubmit in SavOrderSubmit)
                {
                    WFObject WFObj = new WFObject();
                    EncounterManager encMngr = new EncounterManager();
                    Encounter EncRecord = encMngr.GetById(objSubmit.Encounter_ID);
                    WFObj.Obj_Type = objSubmit.Order_Type.ToUpper();
                    WFObj.Current_Arrival_Time = objSubmit.Created_Date_And_Time;
                    if (EncRecord != null && EncRecord.Assigned_Med_Asst_User_Name != string.Empty)
                        WFObj.Current_Owner = EncRecord.Assigned_Med_Asst_User_Name;
                    else
                        WFObj.Current_Owner = "UNKNOWN";
                    WFObj.Fac_Name = objSubmit.Facility_Name;
                    WFObj.Parent_Obj_Type = string.Empty;
                    WFObj.Obj_System_Id = objSubmit.Id;
                    WFObj.Current_Process = "START";


                    if (objSubmit.Move_To_MA == "Y")
                    {
                        iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
                    }
                    else
                    {
                        WFObj.Current_Owner = "UNKNOWN";
                        iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 2, MACAddress, MySession);
                    }


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
                foreach (OrdersSubmit objSubmit in DelOrderSubmit)
                {
                    iResult = objWfMnger.MoveToNextProcess(objSubmit.Id, objSubmit.Order_Type, 6, "UNKNOWN", DateTime.Now, MACAddress, null, MySession);
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
                foreach (OrdersSubmit objSubmit in UpdOrderSubmit)
                {
                    WFObject objWFObject = objWfMnger.GetByObjectSystemId(objSubmit.Id, objSubmit.Order_Type);

                    if (objSubmit.Move_To_MA == "Y")
                    {
                        string MA_NAME = string.Empty;
                        string[] ProcessAndUsers = objWFObject.Process_Allocation.Split('|');
                        foreach (string rec in ProcessAndUsers)
                        {
                            if (rec.StartsWith("MA_"))
                                MA_NAME = rec.Split('-')[1];
                        }

                        if (MA_NAME == string.Empty)
                        {
                            EncounterManager objEncounterManager = new EncounterManager();
                            if (objSubmit.Encounter_ID != 0)
                                MA_NAME = objEncounterManager.GetById(objSubmit.Encounter_ID).Assigned_Med_Asst_User_Name;
                            else
                                MA_NAME = "UNKNOWN";
                        }
                        if (objWFObject.Current_Process == "ORDER_GENERATE")
                        {

                            iResult = objWfMnger.MoveToPreviousProcessWithTransaction(objSubmit.Id, objSubmit.Order_Type, 1, MA_NAME, DateTime.Now, MACAddress, MySession);
                        }
                    }
                    else
                    {
                        if (objWFObject.Current_Process == "MA_REVIEW")
                        {

                            iResult = objWfMnger.MoveToNextProcess(objSubmit.Id, objSubmit.Order_Type, 1, "UNKNOWN", DateTime.Now, MACAddress, null, MySession);
                        }
                    }
                }
                #endregion

                //MySession.Flush();
                //trans.Commit();
                if (isOrder && isOrderSubmit && isOrderAssessmnt)
                {
                    //trans.Commit();
                    if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                    {
                        //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                        //int trycount = 0;
                        //trytosaveagain:
                        try
                        {
                            #region "Code comitted by balaji.TJ 2023-01-08"
                            //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            #endregion

                            #region "Code Modified by balaji.TJ 2023-01-08
                            WriteBlob(uHumanId, XMLObj.itemDoc, MySession, saveList, updtList, delList, XMLObj, false);
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
                    }
                    if (XmlObjEnc.itemDoc.InnerXml != null && XmlObjEnc.itemDoc.InnerXml != "")
                    {
                        // XmlObjEnc.itemDoc.Save(XmlObjEnc.strXmlFilePath);
                        int trycount = 0;
                    trytosaveagain:
                        try
                        {
                            #region "Code comitted by balaji.TJ 2023-01-08"
                            //XmlObjEnc.itemDoc.Save(XmlObjEnc.strXmlFilePath);
                            #endregion

                            #region "Code Modified by balaji.TJ 2023-01-08
                            //WriteBlob(EncounterID, XMLObj.itemDoc, MySession, saveList, updtList, delList, XMLObj, false);
                            objTreatmentPlanManager.WriteBlob(EncounterID, XmlObjEnc.itemDoc, MySession, Insert_Tplan, Update_Tplan, Delete_Tplan, XmlObjEnc, false);
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
                    }
                    trans.Commit();
                }
                else
                {
                    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                }
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

            //return new MemoryStream();
            //if (EncounterID != 0)
            //{
            //    return GetOrdersUsingEncounterID(EncounterID, orderType,false);
            //}
            //else
            //{
            //    ulong tempHuman_ID;
            //    ulong tempPhysician_ID;
            //    DateTime tempCreated_Date_And_Time;


            //    tempHuman_ID = GetHumanID(saveList, updtList, delList);
            //    tempPhysician_ID = GetPhysicianID(saveList, updtList, delList);
            //    tempCreated_Date_And_Time = GetCreatedDateAndTime(SavOrderSubmit, UpdOrderSubmit, DelOrderSubmit);
            //    return GetOrdersUsingHumanID(tempHuman_ID, tempCreated_Date_And_Time, orderType, tempPhysician_ID,false);
            //}
            //GenerateXml XMLObj = new GenerateXml();
            //ulong uhumanid = 0;
            //if (saveList.Count > 0)
            //{

            //    uhumanid = saveList[0].Human_ID;
            //    List<object> lstObj = saveList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, uhumanid, string.Empty);
            //}
            //if (updtList.Count > 0)
            //{
            //    uhumanid = updtList[0].Human_ID;

            //    List<object> lstObj = updtList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlUpdate(lstObj, uhumanid, string.Empty);
            //}            
            //if (SavOrderSubmit.Count > 0)
            //{
            //    uhumanid = SavOrderSubmit[0].Human_ID;
            //    List<object> lstObj = SavOrderSubmit.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, uhumanid, string.Empty);
            //}
            //if (UpdOrderSubmit.Count > 0)
            //{
            //    uhumanid = UpdOrderSubmit[0].Human_ID;
            //    List<object> lstObj = UpdOrderSubmit.Cast<object>().ToList();
            //    XMLObj.GenerateXmlUpdate(lstObj, uhumanid, string.Empty);
            //}
            //if (savOrdAssListForExistingCPT.Count > 0)
            //{
            //    List<object> lstObj = savOrdAssListForExistingCPT.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, uhumanid, string.Empty);
            //}
            //if (savOrdAssListForNewCPT.Count > 0)
            //{
            //    List<object> lstObj = savOrdAssListForNewCPT.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, uhumanid, string.Empty);
            //}

            return 0;
        }

        //not in use
        //public int UpdateSpecimenDetailOnOrders(IList<Orders> updtlist, ISession session, string MACAddress)
        //{
        //    IList<Orders> list = null;
        //    GenerateXml XMLObj = new GenerateXml();
        //    //return SaveUpdateDeleteWithoutTransaction(ref list, updtlist, null, session, MACAddress);
        //    return SaveUpdateDelete_DBAndXML_WithoutTransaction(ref list, ref updtlist, null, session, MACAddress, false, true, 0, string.Empty, ref XMLObj);
        //}



        #endregion
        public IList<string> GetOrderSubmitIDForPrintRequestion(ulong encounterID, ulong PhysicianID, ulong HumanID, string sOrderType)
        {

            IList<string> returnList = new List<string>();
            IList<ulong> LabID = new List<ulong>();
            LabID.Add(1);
            LabID.Add(2);
            using (ISession Mysession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (encounterID != 0)
                {

                    //ICriteria criteriaOrdAss = session.GetISession().CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", encounterID)).Add(Expression.In("Lab_ID", LabID.ToArray<ulong>()));
                    //if (criteriaOrdAss.List<OrdersSubmit>().Count > 0)
                    //{
                    //    IList<OrdersSubmit> TempList = criteriaOrdAss.List<OrdersSubmit>();
                    //    foreach (OrdersSubmit obj in TempList)
                    //        returnList.Add(obj.Lab_ID + "|" + obj.Id);
                    //}
                    IQuery query = Mysession.GetNamedQuery("GetOrderSubmitIDForPrintRequestion");
                    query.SetParameter(0, encounterID);
                    query.SetParameter(1, sOrderType);
                    query.SetParameterList("LabID", LabID.ToArray<ulong>());
                    ArrayList arr = new ArrayList(query.List());
                    if (arr.Count > 0)
                    {
                        foreach (object[] obj in arr)
                            returnList.Add(obj[0].ToString() + "|" + obj[1].ToString());
                    }
                }
                else
                {
                    //ICriteria criteriaOrdAss = session.GetISession().CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Encounter_ID", encounterID)).Add(Expression.In("Lab_ID", LabID.ToArray<ulong>()));
                    //if (criteriaOrdAss.List<OrdersSubmit>().Count > 0)
                    //{
                    //    IList<OrdersSubmit> TempList = criteriaOrdAss.List<OrdersSubmit>();
                    //    foreach (OrdersSubmit obj in TempList)
                    //        returnList.Add(obj.Lab_ID + "|" + obj.Id);
                    //}
                    IQuery query = Mysession.GetNamedQuery("GetOrderSubmitIDForPrintRequestionforMenu");
                    query.SetParameter(0, encounterID);
                    query.SetParameter(1, HumanID);
                    query.SetParameter(2, PhysicianID);
                    query.SetParameterList("LabID", LabID.ToArray<ulong>());
                    ArrayList arr = new ArrayList(query.List());
                    if (arr.Count > 0)
                    {
                        foreach (object[] obj in arr)
                            returnList.Add(obj[0].ToString() + "|" + obj[1].ToString());
                    }

                }
                Mysession.Close();
            }
            return returnList;
        }


        public IList<ulong> GetOrderSubmitIDForPrintLable(ulong encounterID, ulong PhysicianID, ulong HumanID)
        {
            IList<ulong> returnList = new List<ulong>();
            IList<ulong> LabID = new List<ulong>();
            LabID.Add(2);
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (encounterID != 0)
                {
                    ICriteria criteriaOrdAss = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", encounterID)).Add(Expression.In("Lab_ID", LabID.ToArray<ulong>())).Add(Expression.Eq("Specimen_In_House", "Y"));
                    if (criteriaOrdAss.List<OrdersSubmit>().Count > 0)
                    {
                        IList<OrdersSubmit> TempList = criteriaOrdAss.List<OrdersSubmit>();
                        foreach (OrdersSubmit obj in TempList)
                            returnList.Add(obj.Id);
                    }
                }
                else
                {
                    ICriteria criteriaOrdAss = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Encounter_ID", encounterID)).Add(Expression.In("Lab_ID", LabID.ToArray<ulong>()));
                    if (criteriaOrdAss.List<OrdersSubmit>().Count > 0)
                    {
                        IList<OrdersSubmit> TempList = criteriaOrdAss.List<OrdersSubmit>();
                        foreach (OrdersSubmit obj in TempList)
                            returnList.Add(obj.Id);
                    }

                }
                iMySession.Close();
            }
            return returnList;
        }


        public void SubmitEmptyOrdersToMA(OrdersSubmit objOrdersSubmit, string MACAddress)
        {
            int iTryCount = 0;
            GenerateXml XMLObj = new GenerateXml();
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            Session.GetISession().Clear();
            ISession MySession = Session.GetISession();
            // ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                    TryAgain:
                        IList<OrdersSubmit> SaveList = new List<OrdersSubmit>();
                        IList<OrdersSubmit> SaveListnull = null;
                        //trans = MySession.BeginTransaction();
                        objOrdersSubmit.Is_Task_Created = "N";
                        SaveList.Add(objOrdersSubmit);
                        //int i = objOrdersSubmitManager.SaveUpdateDeleteWithoutTransaction(ref SaveList, null, null, MySession, MACAddress);
                        int i = objOrdersSubmitManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveList, ref SaveListnull, null, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
                        if (i == 2)
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
                        else if (i == 1)
                        {
                            trans.Rollback();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }

                        WFObject WFObj = new WFObject();
                        EncounterManager encMngr = new EncounterManager();
                        Encounter EncRecord = encMngr.GetById(SaveList[0].Encounter_ID);
                        WFObj.Obj_Type = SaveList[0].Order_Type.ToUpper();
                        WFObj.Current_Arrival_Time = SaveList[0].Created_Date_And_Time;
                        if (EncRecord != null && EncRecord.Assigned_Med_Asst_User_Name != string.Empty)
                            WFObj.Current_Owner = EncRecord.Assigned_Med_Asst_User_Name;
                        else
                            WFObj.Current_Owner = "UNKNOWN";
                        WFObj.Fac_Name = SaveList[0].Facility_Name;
                        WFObj.Parent_Obj_Type = string.Empty;
                        WFObj.Obj_System_Id = SaveList[0].Id;
                        WFObj.Current_Process = "START";
                        WFObjectManager objWfMnger = new WFObjectManager();
                        i = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
                        if (i == 2)
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
                        else if (i == 1)
                        {
                            trans.Rollback();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }

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
                        trans.Commit();
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

        #region GetMethods
        public FillHumanDTO GetHumanById(ulong HumanId)
        {
            FillHumanDTO objFillHuman = new FillHumanDTO();
            IList<Human> objHuman = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteriaByHumanId = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanId));
                objHuman = criteriaByHumanId.List<Human>();

                if (objHuman.Count > 0)
                {
                    foreach (Human obj in objHuman)
                    {
                        IList<PatientInsuredPlan> ilstInsuredPlan = new List<PatientInsuredPlan>();
                        objFillHuman.Human_ID = obj.Id;
                        objFillHuman.Birth_Date = obj.Birth_Date;
                        objFillHuman.City = obj.City;
                        objFillHuman.First_Name = obj.First_Name;
                        objFillHuman.Last_Name = obj.Last_Name;
                        objFillHuman.MI = obj.MI;
                        //objFillHuman.PatientInsuredBag = obj.PatientInsuredBag;
                        //ilstInsuredPlan = (from pi in obj.PatientInsuredBag where pi.Insurance_Type.ToUpper() == "PRIMARY" && pi.Active.ToUpper() == "YES" select pi).ToList<PatientInsuredPlan>();
                        ilstInsuredPlan = (from pi in obj.PatientInsuredBag where pi.Active.ToUpper() == "YES" select pi).ToList<PatientInsuredPlan>();
                        objFillHuman.PatientInsuredBag = ilstInsuredPlan;
                        objFillHuman.Prefix = obj.Prefix;
                        objFillHuman.Sex = obj.Sex;
                        objFillHuman.SSN = obj.SSN;
                        objFillHuman.State = obj.State;
                        objFillHuman.Street_Address1 = obj.Street_Address1;
                        objFillHuman.Suffix = obj.Suffix;
                        objFillHuman.ZipCode = obj.ZipCode;
                        objFillHuman.Street_Address2 = obj.Street_Address2;
                        objFillHuman.Medical_Record_Number = obj.Medical_Record_Number;
                        objFillHuman.Home_Phone_No = obj.Home_Phone_No;
                        objFillHuman.Guarantor_First_Name = obj.Guarantor_First_Name;
                        objFillHuman.Guarantor_Last_Name = obj.Guarantor_Last_Name;
                        objFillHuman.Guarantor_MI = obj.Guarantor_MI;
                        objFillHuman.Guarantor_State = obj.Guarantor_State;
                        objFillHuman.Guarantor_Street_Address1 = obj.Guarantor_Street_Address1;
                        objFillHuman.Guarantor_Street_Address2 = obj.Guarantor_Street_Address2;
                        objFillHuman.Guarantor_Zip_Code = obj.Guarantor_Zip_Code;
                        objFillHuman.Guarantor_City = obj.Guarantor_City;
                        objFillHuman.Race = obj.Race;
                        objFillHuman.Ethnicity = obj.Ethnicity;
                        objFillHuman.Ethnicity_No = obj.Ethnicity_No;
                        objFillHuman.Guarantor_Home_Phone_Number = obj.Guarantor_Home_Phone_Number;
                        objFillHuman.Guarantor_Relationship_No = obj.Guarantor_Relationship_No;
                        objFillHuman.Guarantor_Relationship = obj.Guarantor_Relationship;
                        objFillHuman.Employer_Name = obj.Employer_Name;
                        objFillHuman.cell_phone_no = obj.Cell_Phone_Number;
                        objFillHuman.Work_Phone_No = obj.Work_Phone_No;
                        break;
                    }
                }
                //added by pravin 25-jan-2012
                IQuery query1 = iMySession.GetNamedQuery("Get.Checkout.PatientPane");
                query1.SetString(0, HumanId.ToString());

                ArrayList PatientList = null;
                PatientList = new ArrayList(query1.List());
                // FillHumanDTO p = null;

                if (PatientList.Count > 0)
                {
                    for (int i = 0; i < PatientList.Count; i++)
                    {
                        //  p = new FillHumanDTO();
                        object[] obj1 = (object[])PatientList[i];
                        if (obj1[0] != null) objFillHuman.Insurance_Type = obj1[0].ToString();
                        if (obj1[1] != null) objFillHuman.Ins_Plan_Name = obj1[1].ToString();
                        if (obj1[2] != null) objFillHuman.Assigned_Physician = obj1[2].ToString();
                        if (obj1[3] != null) objFillHuman.CarrierName = obj1[3].ToString();



                    }
                }
                iMySession.Close();
            }
            return objFillHuman;
        }

        public FillHumanDTO PatientInsuredBag(ulong HumanId)
        {
            FillHumanDTO objFillHuman = new FillHumanDTO();
            IList<Human> objHuman = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteriaByHumanId = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanId));
                objHuman = criteriaByHumanId.List<Human>();
                if (objHuman.Count > 0)
                {
                    foreach (Human obj in objHuman)
                    {
                        IList<PatientInsuredPlan> ilstInsuredPlan = new List<PatientInsuredPlan>();
                        objFillHuman.Human_ID = obj.Id;
                        objFillHuman.Birth_Date = obj.Birth_Date;
                        objFillHuman.City = obj.City;
                        objFillHuman.First_Name = obj.First_Name;
                        objFillHuman.Last_Name = obj.Last_Name;
                        objFillHuman.MI = obj.MI;
                        //objFillHuman.PatientInsuredBag = obj.PatientInsuredBag;
                        ilstInsuredPlan = (from pi in obj.PatientInsuredBag where pi.Insurance_Type.ToUpper() == "PRIMARY" && pi.Active.ToUpper() == "YES" select pi).ToList<PatientInsuredPlan>();
                        objFillHuman.PatientInsuredBag = ilstInsuredPlan;
                        objFillHuman.Prefix = obj.Prefix;
                        objFillHuman.Sex = obj.Sex;
                        objFillHuman.SSN = obj.SSN;
                        objFillHuman.State = obj.State;
                        objFillHuman.Street_Address1 = obj.Street_Address1;
                        objFillHuman.Suffix = obj.Suffix;
                        objFillHuman.ZipCode = obj.ZipCode;
                        objFillHuman.Street_Address2 = obj.Street_Address2;
                        objFillHuman.Medical_Record_Number = obj.Medical_Record_Number;
                        objFillHuman.Home_Phone_No = obj.Home_Phone_No;
                        objFillHuman.Guarantor_First_Name = obj.Guarantor_First_Name;
                        objFillHuman.Guarantor_Last_Name = obj.Guarantor_Last_Name;
                        objFillHuman.Guarantor_MI = obj.Guarantor_MI;
                        objFillHuman.Guarantor_State = obj.Guarantor_State;
                        objFillHuman.Guarantor_Street_Address1 = obj.Guarantor_Street_Address1;
                        objFillHuman.Guarantor_Street_Address2 = obj.Guarantor_Street_Address2;
                        objFillHuman.Guarantor_Zip_Code = obj.Guarantor_Zip_Code;
                        objFillHuman.Guarantor_City = obj.Guarantor_City;
                        objFillHuman.Race = obj.Race;
                        objFillHuman.Ethnicity = obj.Ethnicity;
                        objFillHuman.Ethnicity_No = obj.Ethnicity_No;
                        objFillHuman.Guarantor_Home_Phone_Number = obj.Guarantor_Home_Phone_Number;
                        objFillHuman.Guarantor_Relationship_No = obj.Guarantor_Relationship_No;
                        objFillHuman.Guarantor_Relationship = obj.Guarantor_Relationship;
                        objFillHuman.Employer_Name = obj.Employer_Name;
                        objFillHuman.cell_phone_no = obj.Cell_Phone_Number;
                        objFillHuman.Work_Phone_No = obj.Work_Phone_No;
                        break;
                    }
                }
                iMySession.Close();
            }
            return objFillHuman;
        }
        public Stream GetOrdersUsingOrderSubmitID(DateTime Modified_Date_And_Time, string LabType)
        {

            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objOrdersDTO = new OrdersDTO();
            //OrderDetailsDTO objOrderDetDTO = new OrderDetailsDTO();
            IList<OrderLabDetailsDTO> ordLabList = new List<OrderLabDetailsDTO>();
            OrderLabDetailsDTO ordLabObj = new OrderLabDetailsDTO();
            Orders objOrd = new Orders();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            OrdersAssessment objOrdAss = new OrdersAssessment();
            IList<ulong> ulList = new List<ulong>();
            IList<ulong> ulSpecimenIDList = new List<ulong>();
            IList<Orders> iOrderList = new List<Orders>();
            ArrayList orderList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.Orders.GetOrdersUsingOrderSubmitID");
                if (LabType == "LAB")
                    query2.SetString(0, "DIAGNOSTIC ORDER");
                else
                    query2.SetString(0, "IMAGE ORDER");
                query2.SetString(1, Modified_Date_And_Time.ToString("yyyy-MM-dd hh:mm:ss"));

                orderList = new ArrayList(query2.List());

                if (orderList != null)
                {
                    foreach (object[] obj in orderList)
                    {
                        ordLabObj = new OrderLabDetailsDTO();
                        Orders objOrder = new Orders();
                        OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                        //Specimen objSpecimen = new Specimen();
                        objOrder.Id = Convert.ToUInt64(obj[0]);
                        objOrder.Encounter_ID = Convert.ToUInt64(obj[1]);
                        objOrder.Human_ID = Convert.ToUInt64(obj[2]);
                        objOrder.Physician_ID = Convert.ToUInt64(obj[3]);
                        objOrdersSubmit.Lab_Location_ID = Convert.ToUInt64(obj[4]);
                        objOrdersSubmit.Lab_ID = Convert.ToUInt64(obj[5]);
                        objOrder.Lab_Procedure = obj[6].ToString();
                        objOrdersSubmit.Authorization_Required = obj[7].ToString();
                        objOrdersSubmit.Test_Date = Convert.ToString(obj[8]);
                        objOrdersSubmit.Order_Notes = obj[9].ToString();
                        objOrdersSubmit.Specimen_In_House = obj[10].ToString();
                        objOrdersSubmit.Stat = obj[11].ToString();
                        objOrdersSubmit.Fasting = obj[12].ToString();
                        objOrdersSubmit.Created_Date_And_Time = Convert.ToDateTime(obj[13]);
                        objOrder.Created_By = obj[14].ToString();
                        objOrder.Created_Date_And_Time = Convert.ToDateTime(obj[15]);
                        objOrder.Modified_By = obj[16].ToString();
                        objOrder.Modified_Date_And_Time = Convert.ToDateTime(obj[17]);
                        objOrder.Version = Convert.ToInt16(obj[18]);
                        objOrder.Lab_Procedure_Description = obj[19].ToString();
                        objOrdersSubmit.Order_Type = obj[20].ToString();
                        //objOrdersSubmit.Specimen_ID = Convert.ToUInt64(obj[21]);
                        objOrdersSubmit.Facility_Name = obj[22].ToString();
                        objOrdersSubmit.Height = obj[23].ToString();
                        objOrdersSubmit.Weight = obj[24].ToString();
                        objOrder.Order_Code_Type = obj[25].ToString();
                        objOrdersSubmit.Bill_Type = obj[26].ToString();
                        objOrdersSubmit.Is_ABN_Signed = obj[27].ToString();
                        objOrdersSubmit.Specimen_Type = obj[28].ToString();
                        objOrdersSubmit.Culture_Location = obj[29].ToString();
                        //objOrder.Is_In_House = obj[30].ToString();
                        //objOrder.Orders_Internal_Submit_ID = Convert.ToUInt64(obj[31]);
                        //objOrder.TestDate_In_Months = Convert.ToInt16(obj[32]);
                        //objOrder.TestDate_In_Weeks = Convert.ToInt16(obj[33]);
                        //objOrder.TestDate_In_Days = Convert.ToInt16(obj[34]);
                        //objOrder.Patient_Instructions = obj[35].ToString();
                        objOrdersSubmit.Move_To_MA = obj[30].ToString();
                        objOrder.Internal_Property_Current_Process = obj[33].ToString();
                        objOrdersSubmit.Specimen_Collection_Date_And_Time = Convert.ToDateTime(obj[34]);
                        objOrdersSubmit.Temperature = obj[35].ToString();
                        objOrder.Order_Submit_ID = Convert.ToUInt64(obj[36]);
                        ordLabObj.ObjOrder = objOrder;

                        if (obj[31] != null)
                        {
                            ordLabObj.LabName = obj[31].ToString();
                        }
                        if (obj[32] != null)
                        {
                            ordLabObj.LabLocName = obj[32].ToString();
                        }
                        ordLabObj.procedureCodeDesc = obj[6].ToString() + "-" + obj[19].ToString();

                        //ICriteria critSta = session.GetISession().CreateCriteria(typeof(StandingOrder)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critSta.List<StandingOrder>().Count > 0)
                        //{
                        //    ordLabObj.objStandingOrder = critSta.List<StandingOrder>()[0];
                        //}
                        //ICriteria critAfp = iMySession.CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                        //{
                        //    ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                        //}
                        //ICriteria critbloodLead = iMySession.CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                        //{
                        //    ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                        //}
                        //ICriteria critCyto = iMySession.CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                        //{
                        //    ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                        //}
                        //ICriteria critAOE = iMySession.CreateCriteria(typeof(OrdersQuestionSetAOE)).Add(Expression.Eq("Orders_ID", objOrder.Id));
                        //if (critAOE.List<OrdersQuestionSetAOE>().Count > 0)
                        //{
                        //    ordLabObj.OrderAOEList = critAOE.List<OrdersQuestionSetAOE>();
                        //}
                        //ordLabObj.objSpecimen = objSpecimen;
                        ordLabObj.OrdersSubmit = objOrdersSubmit;
                        ordLabList.Add(ordLabObj);
                        //ulList.Add(objOrder.Id);
                        ulList.Add(objOrder.Order_Submit_ID);
                        //ulSpecimenIDList.Add(objOrdersSubmit.Specimen_ID);

                    }
                }
                objOrdersDTO.ilstOrderLabDetailsDTO = ordLabList;
                orderList.Clear();
                if (ulList.Count > 0)
                {
                    objOrdersDTO.OrderAssList = GetOrdersAssessmentDetails(ulList);
                    // objOrderDetDTO.OrderPblmList = GetOrdersProblemListDetails(ulList);
                }

                //ICriteria criteria = session.GetISession().CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteria.List<OrdersSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.OrderSubmitList = criteria.List<OrdersSubmit>();
                //}
                //if (ulSpecimenIDList.Count > 0)
                //{
                //    ICriteria criteriaSpecimen = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.In("Id", ulSpecimenIDList.ToArray<ulong>()));
                //    if (criteriaSpecimen.List<Specimen>().Count > 0)
                //    {
                //        objOrderDetDTO.SpecimenList = criteriaSpecimen.List<Specimen>();
                //    }
                //}
                //ICriteria subCrit = session.GetISession().CreateCriteria(typeof(OrdersInternalSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Order_Type", "INTERNAL " + orderType));
                //if (subCrit.List<OrdersInternalSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.objInternalSubmit = subCrit.List<OrdersInternalSubmit>()[0];
                //}
                //else
                //{
                //    objOrderDetDTO.objInternalSubmit = null;
                //}
                //}
                serializer.WriteObject(stream, objOrdersDTO);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;
        }
        public Stream GetOrdersUsingEncounterID(ulong EncounterID, string orderType, bool LoadQuestionSet)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            //OrderDetailsDTO objOrderDetDTO = new OrderDetailsDTO();
            OrdersDTO objOrdersDTO = new OrdersDTO();
            IList<OrderLabDetailsDTO> ordLabList = new List<OrderLabDetailsDTO>();
            OrderLabDetailsDTO ordLabObj = new OrderLabDetailsDTO();
            Orders objOrd = new Orders();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            OrdersAssessment objOrdAss = new OrdersAssessment();
            IList<ulong> ulList = new List<ulong>();
            IList<ulong> ulSpecimenIDList = new List<ulong>();
            IList<Orders> iOrderList = new List<Orders>();
            //ArrayList orderList = null;
            ulong Order_id = 0;
            ulong Order_submit_id = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //IQuery query2 = iMySession.GetNamedQuery("Fill.Orders.GetOrdersUsingEncounterIDAndType");
                //query2.SetString(0, EncounterID.ToString());
                //query2.SetString(1, orderType);
                //orderList = new ArrayList(query2.List());
                //ISQLQuery query2 = iMySession.CreateSQLQuery("SELECT O.*,OS.*,L.*,LL.*,W.* FROM orders O LEFT JOIN orders_submit OS ON O.Order_Submit_ID=OS.Order_Submit_ID LEFT JOIN lab L ON (OS.Lab_ID=L.Lab_ID) LEFT JOIN lab_location LL ON (OS.Lab_Location_ID=LL.Lab_Location_ID) LEFT JOIN wf_object W ON (O.Order_Submit_ID=W.Obj_System_ID and OS.Order_Type=W.Obj_Type) WHERE O.Encounter_ID='" + EncounterID.ToString() + "'  AND OS.Order_Type='" + orderType + "' ORDER BY CAST(O.Modified_Date_And_Time AS CHAR(100)) DESC").AddEntity("O", typeof(Orders)).AddEntity("OS", typeof(OrdersSubmit)).AddEntity("L", typeof(Lab)).AddEntity("LL", typeof(LabLocation)).AddEntity("W", typeof(WFObject));
                ISQLQuery query2 = iMySession.CreateSQLQuery("SELECT {O.*},{OS.*},{L.*},{LL.*},{W.*} FROM orders O LEFT JOIN orders_submit OS ON O.Order_Submit_ID=OS.Order_Submit_ID LEFT JOIN lab L ON (OS.Lab_ID=L.Lab_ID) LEFT JOIN lab_location LL ON (OS.Lab_Location_ID=LL.Lab_Location_ID) LEFT JOIN wf_object W ON (O.Order_Submit_ID=W.Obj_System_ID and OS.Order_Type=W.Obj_Type) WHERE O.Encounter_ID='" + EncounterID.ToString() + "'  AND OS.Order_Type='" + orderType + "' ORDER BY if(O.Modified_Date_And_Time<>'0001-01-01 00:00:00',CAST(O.Modified_Date_And_Time AS CHAR(100)),CAST(O.Created_date_and_time AS CHAR(100)))  DESC").AddEntity("O", typeof(Orders)).AddEntity("OS", typeof(OrdersSubmit)).AddEntity("L", typeof(Lab)).AddEntity("LL", typeof(LabLocation)).AddEntity("W", typeof(WFObject));
                ArrayList aryorderList = new ArrayList(query2.List());

                //IList<object[]> orderList = query2.List<object[]>();
                IList<ulong> AddedOrderIds = new List<ulong>();
                if (aryorderList != null)  //if (orderList != null)
                {
                    foreach (IList<Object> obj in aryorderList)   //foreach (object[] obj in orderList)
                    {
                        //Add new
                        if (obj.Count() > 0)
                        {
                            ordLabObj = new OrderLabDetailsDTO();
                            Orders objOrder = new Orders();
                            OrdersSubmit objOrdersSubmit = new OrdersSubmit();

                            if (obj[0] != null)
                            {
                                ordLabObj.ObjOrder = (Orders)obj[0];
                                AddedOrderIds.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                                Order_id = ordLabObj.ObjOrder.Id;
                                Order_submit_id = ordLabObj.ObjOrder.Order_Submit_ID;
                            }
                            if (obj[1] != null)
                            {
                                ordLabObj.OrdersSubmit = (OrdersSubmit)obj[1];
                            }
                            if (LoadQuestionSet)
                            {
                                if (ordLabObj.ObjOrder.Order_Submit_ID == OrderSubmitId)
                                {


                                    //ICriteria critAfp = iMySession.CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                    //if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                                    //{
                                    //    ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                                    //}
                                    //ICriteria critbloodLead = iMySession.CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                    //if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                                    //{
                                    //    ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                                    //}
                                    //ICriteria critCyto = iMySession.CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                    //if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                                    //{
                                    //    ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                                    //}
                                    //ICriteria critAOE = iMySession.CreateCriteria(typeof(OrdersQuestionSetAOE)).Add(Expression.Eq("Orders_ID", ordLabObj.ObjOrder.Id));
                                    //if (critAOE.List<OrdersQuestionSetAOE>().Count > 0)
                                    //{
                                    //    ordLabObj.OrderAOEList = critAOE.List<OrdersQuestionSetAOE>();
                                    //}
                                }
                                ordLabObj.OrdersSubmit = null;
                                ordLabObj.ObjOrder = null;
                            }
                            else
                            {
                                if (obj[4] != null)
                                {
                                    ordLabObj.ObjOrder.Internal_Property_Current_Process = ((WFObject)obj[4]).Current_Process;
                                    if (ordLabObj.ObjOrder.Internal_Property_Current_Process.StartsWith("DELETED_"))
                                        continue;
                                }
                                if (obj[2] != null)
                                {
                                    if (((Lab)obj[2]).Lab_Name != null)
                                        ordLabObj.LabName = ((Lab)obj[2]).Lab_Name;
                                }
                                if (obj[3] != null)
                                {
                                    if (((LabLocation)obj[3]).Location_Name != null)
                                        ordLabObj.LabLocName = ((LabLocation)obj[3]).Location_Name;
                                }

                                ordLabObj.procedureCodeDesc = ordLabObj.ObjOrder.Lab_Procedure + "-" + ordLabObj.ObjOrder.Lab_Procedure_Description;
                                //ulList.Add(ordLabObj.ObjOrder.Id);
                                ulList.Add(ordLabObj.ObjOrder.Order_Submit_ID);

                                ICriteria critOrdersRequiredForms = iMySession.CreateCriteria(typeof(OrdersRequiredForms)).Add(Expression.Eq("Order_Id", ordLabObj.ObjOrder.Id));
                                if (critOrdersRequiredForms.List<OrdersRequiredForms>().Count > 0)
                                {
                                    ordLabObj.ilstOrdersRequiredForms = critOrdersRequiredForms.List<OrdersRequiredForms>();
                                }
                                if (ordLabObj.OrdersSubmit.Lab_ID == 1)
                                {
                                    ICriteria questionsSetType = iMySession.CreateCriteria(typeof(OrderCodeLibrary)).Add(Expression.Eq("Order_Code", ordLabObj.ObjOrder.Lab_Procedure)).Add(Expression.Eq("Lab_ID", ordLabObj.OrdersSubmit.Lab_ID));
                                    if (questionsSetType.List<OrderCodeLibrary>().Count > 0)
                                    {
                                        ordLabObj.lstQuestionSetNames.Add(questionsSetType.List<OrderCodeLibrary>()[0].Order_Code_Question_Set_Segment);
                                    }
                                }

                            }
                            ordLabList.Add(ordLabObj);
                        }
                    }

                }
                //End
                //        ordLabObj = new OrderLabDetailsDTO();
                //        Orders objOrder = new Orders();
                //        OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                //        //Specimen objSpecimen = new Specimen();
                //        Order_id = Convert.ToUInt64(obj[0]);
                //        Order_submit_id = Convert.ToUInt64(obj[1]);


                //        ICriteria critOrd = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Id", Order_id));
                //        if (critOrd.List<Orders>().Count > 0)
                //        {
                //            ordLabObj.ObjOrder = critOrd.List<Orders>()[0];

                //            AddedOrderIds.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                //        }

                //        ICriteria critOrdSub = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", ordLabObj.ObjOrder.Order_Submit_ID));
                //        if (critOrdSub.List<OrdersSubmit>().Count > 0)
                //        {
                //            ordLabObj.OrdersSubmit = critOrdSub.List<OrdersSubmit>()[0];
                //        }
                //        if (LoadQuestionSet)
                //        {
                //            if (ordLabObj.ObjOrder.Order_Submit_ID == OrderSubmitId)
                //            {


                //                ICriteria critAfp = iMySession.CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                //                if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                //                {
                //                    ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                //                }
                //                ICriteria critbloodLead = iMySession.CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                //                if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                //                {
                //                    ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                //                }
                //                ICriteria critCyto = iMySession.CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                //                if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                //                {
                //                    ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                //                }
                //                ICriteria critAOE = iMySession.CreateCriteria(typeof(OrdersQuestionSetAOE)).Add(Expression.Eq("Orders_ID", ordLabObj.ObjOrder.Id));
                //                if (critAOE.List<OrdersQuestionSetAOE>().Count > 0)
                //                {
                //                    ordLabObj.OrderAOEList = critAOE.List<OrdersQuestionSetAOE>();
                //                }
                //            }
                //            ordLabObj.OrdersSubmit = null;
                //            ordLabObj.ObjOrder = null;
                //        }
                //        else
                //        {
                //            ICriteria critWfobj = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", ordLabObj.OrdersSubmit.Order_Type)).Add(Expression.Eq("Obj_System_Id", ordLabObj.OrdersSubmit.Id));
                //            if (critWfobj.List<WFObject>().Count > 0)
                //            {
                //                ordLabObj.ObjOrder.Internal_Property_Current_Process = critWfobj.List<WFObject>()[0].Current_Process;
                //                if (ordLabObj.ObjOrder.Internal_Property_Current_Process.StartsWith("DELETED_"))
                //                    continue;
                //            }
                //            //New
                //            XmlTextReader XmlTextLab = new XmlTextReader(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\LabList.xml"));
                //            XDocument xmlLab = XDocument.Load(XmlTextLab);

                //            //IEnumerable<XElement> xmlFac = xmlLab.Element("LabList")
                //            //                                .Elements("Facility").Where(aa => aa.Attribute("Name").Value.ToString() == ClientSession.FacilityName);
                //            ////.OrderBy(s => (string)s.Attribute("City"));
                //            //if (xmlFac != null && xmlFac.Count() > 0)
                //            //{
                //            //    LookUpPerRequest.Add("FacilityCity", xmlFac.Attributes("City").First().Value.ToString());
                //            //}
                //            IEnumerable<XElement> xml = xmlLab.Element("LabList")
                //                .Elements("Lab").Where(aa => aa.Attribute("id").Value.ToString() == ordLabObj.OrdersSubmit.Lab_ID.ToString());
                //            if (xml != null && xml.Count() > 0)
                //            {
                //               string sval= xml.Attributes("name").First().Value.ToString();
                //            }
                //            XmlTextLab.Close();
                //            //end

                //            ICriteria critLab = iMySession.CreateCriteria(typeof(Lab)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_ID));
                //            if (critLab.List<Lab>().Count > 0)
                //            {
                //                ordLabObj.LabName = critLab.List<Lab>()[0].Lab_Name;
                //            }
                //            ICriteria critLabLoc = iMySession.CreateCriteria(typeof(LabLocation)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_Location_ID));
                //            if (critLabLoc.List<LabLocation>().Count > 0)
                //            {
                //                ordLabObj.LabLocName = critLabLoc.List<LabLocation>()[0].Location_Name;
                //            }
                //            //ICriteria critSpe = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Specimen_ID));
                //            //if (critSpe.List<Specimen>().Count > 0)
                //            //{
                //            //    ordLabObj.objSpecimen = critSpe.List<Specimen>()[0];
                //            //}
                //            ordLabObj.procedureCodeDesc = ordLabObj.ObjOrder.Lab_Procedure + "-" + ordLabObj.ObjOrder.Lab_Procedure_Description;
                //            ulList.Add(ordLabObj.ObjOrder.Id);

                //            ICriteria critOrdersRequiredForms = iMySession.CreateCriteria(typeof(OrdersRequiredForms)).Add(Expression.Eq("Order_Id", ordLabObj.ObjOrder.Id));
                //            if (critOrdersRequiredForms.List<OrdersRequiredForms>().Count > 0)
                //            {
                //                ordLabObj.ilstOrdersRequiredForms = critOrdersRequiredForms.List<OrdersRequiredForms>();
                //            }
                //            if (ordLabObj.OrdersSubmit.Lab_ID == 1)
                //            {
                //                ICriteria questionsSetType = iMySession.CreateCriteria(typeof(OrderCodeLibrary)).Add(Expression.Eq("Order_Code", ordLabObj.ObjOrder.Lab_Procedure)).Add(Expression.Eq("Lab_ID", ordLabObj.OrdersSubmit.Lab_ID));
                //                if (questionsSetType.List<OrderCodeLibrary>().Count > 0)
                //                {
                //                    ordLabObj.lstQuestionSetNames.Add(questionsSetType.List<OrderCodeLibrary>()[0].Order_Code_Question_Set_Segment);
                //                }
                //            }

                //            //ulSpecimenIDList.Add(ordLabObj.OrdersSubmit.Specimen_ID);
                //        }

                //        //ordLabObj.objSpecimen = objSpecimen;


                //        ordLabList.Add(ordLabObj);

                //    }
                //}

                //AddedOrderIds = AddedOrderIds.Distinct().ToList<ulong>();
                //ISQLQuery queryId = iMySession.CreateSQLQuery("SELECT OS.*,W.* FROM orders_submit OS , wf_object W  WHERE W.obj_system_id=OS.Order_Submit_Id and W.Current_Process!=\"DELETED_ORDER\" And W.Obj_Type=\"DIAGNOSTIC ORDER\" AND OS.Is_Paper_Order='N' AND OS.Encounter_ID=" + EncounterID + ";").AddEntity("OS", typeof(OrdersSubmit)).AddEntity("W", typeof(WFObject));
                //IList<object[]> tempObjectArray = queryId.List<object[]>();
                //IList<OrdersSubmit> templist = new List<OrdersSubmit>();
                //foreach (object[] obj in tempObjectArray)
                //{
                //    OrdersSubmit tempOrdersSubmit = new OrdersSubmit();
                //    tempOrdersSubmit = (OrdersSubmit)obj[0];
                //    tempOrdersSubmit.Culture_Location = ((WFObject)obj[1]).Current_Process;
                //    templist.Add(tempOrdersSubmit);

                //}
                AddedOrderIds = AddedOrderIds.Distinct().ToList<ulong>();
                IList<OrdersSubmit> templist = new List<OrdersSubmit>();

                if (aryorderList != null)  //if (orderList != null)
                {
                    foreach (IList<Object> obj in aryorderList)   //foreach (object[] obj in orderList)
                    {
                        WFObject objwf = new WFObject();
                        OrdersSubmit objos = new OrdersSubmit();
                        if (obj.Count() > 0)
                        {
                            if (obj[4] != null)
                            {
                                objwf = (WFObject)obj[4];
                            }
                            if (obj[1] != null)
                            {
                                objos = (OrdersSubmit)obj[1];
                            }
                            if (objwf.Obj_System_Id == objos.Id && objwf.Current_Process != "DELETED_ORDER" && objwf.Obj_Type == "DIAGNOSTIC ORDER" && objos.Is_Paper_Order == "N" && objos.Encounter_ID == EncounterID)
                            {
                                OrdersSubmit tempOrdersSubmit = new OrdersSubmit();
                                tempOrdersSubmit = (OrdersSubmit)obj[1];
                                tempOrdersSubmit.Culture_Location = ((WFObject)obj[4]).Current_Process;
                                templist.Add(tempOrdersSubmit);
                            }

                        }
                    }
                }

                objOrdersDTO.ilstOrdersSubmitForPartialOrders = templist;
                objOrdersDTO.ilstOrdersSubmitForPartialOrders = objOrdersDTO.ilstOrdersSubmitForPartialOrders.Where(a => !AddedOrderIds.Contains(a.Id)).ToList<OrdersSubmit>();


                objOrdersDTO.ilstOrderLabDetailsDTO = ordLabList;
                aryorderList.Clear(); //orderList.Clear();
                if (ulList.Count > 0 && !LoadQuestionSet)
                {
                    objOrdersDTO.OrderAssList = GetOrdersAssessmentDetails(ulList);
                }






                //ICriteria criteria = session.GetISession().CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteria.List<OrdersSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.OrderSubmitList = criteria.List<OrdersSubmit>();
                //}
                //ICriteria criteriaOrdAss = session.GetISession().CreateCriteria(typeof(OrdersAssessment)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteriaOrdAss.List<OrdersAssessment>().Count > 0)
                //{
                //    objOrderDetDTO.OrderAssList = criteriaOrdAss.List<OrdersAssessment>();
                //}
                //<**>
                //if (ulSpecimenIDList.Count > 0)
                //{
                //    ICriteria criteriaSpecimen = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.In("Id", ulSpecimenIDList.ToArray<ulong>()));
                //    if (criteriaSpecimen.List<Specimen>().Count > 0)
                //    {
                //        objOrderDetDTO.SpecimenList = criteriaSpecimen.List<Specimen>();
                //    }
                //}
                //ICriteria subCrit = session.GetISession().CreateCriteria(typeof(OrdersInternalSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Order_Type", "INTERNAL " + orderType));
                //if (subCrit.List<OrdersInternalSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.objInternalSubmit = subCrit.List<OrdersInternalSubmit>()[0];
                //}
                //else
                //{
                //    objOrderDetDTO.objInternalSubmit = null;
                //}
                //}
                # region In-house view

                //MOHAN
                IList<ulong> lstsubmit_id = objOrdersDTO.ilstOrderLabDetailsDTO.Where(q => q.OrdersSubmit != null).Select(A => A.OrdersSubmit.Id).Distinct().ToList<ulong>();
                for (int k = 0; k < lstsubmit_id.Count; k++)
                {
                    ICriteria critMgnt = iMySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Order_ID", lstsubmit_id[k]));
                    if (critMgnt.List<FileManagementIndex>().Count > 0)

                        foreach (var obj in objOrdersDTO.ilstOrderLabDetailsDTO.Where(Z => Z.OrdersSubmit.Id == lstsubmit_id[k]))
                        {
                            objOrdersDTO.ilstOrderLabDetailsDTO[objOrdersDTO.ilstOrderLabDetailsDTO.IndexOf(obj)].OrdersSubmit.Internal_Property_File_Path = "TRUE";
                        }
                }

                #endregion

                serializer.WriteObject(stream, objOrdersDTO);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;
        }


        public Stream GetOrdersUsingEncounterIDByOrderSubmitID(ulong OrderSubmitId,ulong EncounterID, string orderType, bool LoadQuestionSet)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            //OrderDetailsDTO objOrderDetDTO = new OrderDetailsDTO();
            OrdersDTO objOrdersDTO = new OrdersDTO();
            IList<OrderLabDetailsDTO> ordLabList = new List<OrderLabDetailsDTO>();
            OrderLabDetailsDTO ordLabObj = new OrderLabDetailsDTO();
            Orders objOrd = new Orders();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            OrdersAssessment objOrdAss = new OrdersAssessment();
            IList<ulong> ulList = new List<ulong>();
            IList<ulong> ulSpecimenIDList = new List<ulong>();
            IList<Orders> iOrderList = new List<Orders>();
            //ArrayList orderList = null;
            ulong Order_id = 0;
            ulong Order_submit_id = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //IQuery query2 = iMySession.GetNamedQuery("Fill.Orders.GetOrdersUsingEncounterIDAndType");
                //query2.SetString(0, EncounterID.ToString());
                //query2.SetString(1, orderType);
                //orderList = new ArrayList(query2.List());
                //ISQLQuery query2 = iMySession.CreateSQLQuery("SELECT O.*,OS.*,L.*,LL.*,W.* FROM orders O LEFT JOIN orders_submit OS ON O.Order_Submit_ID=OS.Order_Submit_ID LEFT JOIN lab L ON (OS.Lab_ID=L.Lab_ID) LEFT JOIN lab_location LL ON (OS.Lab_Location_ID=LL.Lab_Location_ID) LEFT JOIN wf_object W ON (O.Order_Submit_ID=W.Obj_System_ID and OS.Order_Type=W.Obj_Type) WHERE O.Encounter_ID='" + EncounterID.ToString() + "'  AND OS.Order_Type='" + orderType + "' ORDER BY CAST(O.Modified_Date_And_Time AS CHAR(100)) DESC").AddEntity("O", typeof(Orders)).AddEntity("OS", typeof(OrdersSubmit)).AddEntity("L", typeof(Lab)).AddEntity("LL", typeof(LabLocation)).AddEntity("W", typeof(WFObject));
                ISQLQuery query2 = iMySession.CreateSQLQuery("SELECT {O.*},{OS.*},{L.*},{LL.*},{W.*} FROM orders O LEFT JOIN orders_submit OS ON O.Order_Submit_ID=OS.Order_Submit_ID LEFT JOIN lab L ON (OS.Lab_ID=L.Lab_ID) LEFT JOIN lab_location LL ON (OS.Lab_Location_ID=LL.Lab_Location_ID) LEFT JOIN wf_object W ON (O.Order_Submit_ID=W.Obj_System_ID and OS.Order_Type=W.Obj_Type) WHERE O.Order_Submit_ID='" + OrderSubmitId + "' AND O.Encounter_ID='" + EncounterID.ToString() + "'  AND OS.Order_Type='" + orderType + "' ORDER BY if(O.Modified_Date_And_Time<>'0001-01-01 00:00:00',CAST(O.Modified_Date_And_Time AS CHAR(100)),CAST(O.Created_date_and_time AS CHAR(100)))  DESC").AddEntity("O", typeof(Orders)).AddEntity("OS", typeof(OrdersSubmit)).AddEntity("L", typeof(Lab)).AddEntity("LL", typeof(LabLocation)).AddEntity("W", typeof(WFObject));
                ArrayList aryorderList = new ArrayList(query2.List());

                //IList<object[]> orderList = query2.List<object[]>();
                IList<ulong> AddedOrderIds = new List<ulong>();
                if (aryorderList != null)  //if (orderList != null)
                {
                    foreach (IList<Object> obj in aryorderList)   //foreach (object[] obj in orderList)
                    {
                        //Add new
                        if (obj.Count() > 0)
                        {
                            ordLabObj = new OrderLabDetailsDTO();
                            Orders objOrder = new Orders();
                            OrdersSubmit objOrdersSubmit = new OrdersSubmit();

                            if (obj[0] != null)
                            {
                                ordLabObj.ObjOrder = (Orders)obj[0];
                                AddedOrderIds.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                                Order_id = ordLabObj.ObjOrder.Id;
                                Order_submit_id = ordLabObj.ObjOrder.Order_Submit_ID;
                            }
                            if (obj[1] != null)
                            {
                                ordLabObj.OrdersSubmit = (OrdersSubmit)obj[1];
                            }
                            if (LoadQuestionSet)
                            {
                                if (ordLabObj.ObjOrder.Order_Submit_ID == OrderSubmitId)
                                {


                                    //ICriteria critAfp = iMySession.CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                    //if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                                    //{
                                    //    ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                                    //}
                                    //ICriteria critbloodLead = iMySession.CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                    //if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                                    //{
                                    //    ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                                    //}
                                    //ICriteria critCyto = iMySession.CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                    //if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                                    //{
                                    //    ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                                    //}
                                    //ICriteria critAOE = iMySession.CreateCriteria(typeof(OrdersQuestionSetAOE)).Add(Expression.Eq("Orders_ID", ordLabObj.ObjOrder.Id));
                                    //if (critAOE.List<OrdersQuestionSetAOE>().Count > 0)
                                    //{
                                    //    ordLabObj.OrderAOEList = critAOE.List<OrdersQuestionSetAOE>();
                                    //}
                                }
                                ordLabObj.OrdersSubmit = null;
                                ordLabObj.ObjOrder = null;
                            }
                            else
                            {
                                if (obj[4] != null)
                                {
                                    ordLabObj.ObjOrder.Internal_Property_Current_Process = ((WFObject)obj[4]).Current_Process;
                                    if (ordLabObj.ObjOrder.Internal_Property_Current_Process.StartsWith("DELETED_"))
                                        continue;
                                }
                                if (obj[2] != null)
                                {
                                    if (((Lab)obj[2]).Lab_Name != null)
                                        ordLabObj.LabName = ((Lab)obj[2]).Lab_Name;
                                }
                                if (obj[3] != null)
                                {
                                    if (((LabLocation)obj[3]).Location_Name != null)
                                        ordLabObj.LabLocName = ((LabLocation)obj[3]).Location_Name;
                                }

                                ordLabObj.procedureCodeDesc = ordLabObj.ObjOrder.Lab_Procedure + "-" + ordLabObj.ObjOrder.Lab_Procedure_Description;
                                //ulList.Add(ordLabObj.ObjOrder.Id);
                                ulList.Add(ordLabObj.ObjOrder.Order_Submit_ID);

                                ICriteria critOrdersRequiredForms = iMySession.CreateCriteria(typeof(OrdersRequiredForms)).Add(Expression.Eq("Order_Id", ordLabObj.ObjOrder.Id));
                                if (critOrdersRequiredForms.List<OrdersRequiredForms>().Count > 0)
                                {
                                    ordLabObj.ilstOrdersRequiredForms = critOrdersRequiredForms.List<OrdersRequiredForms>();
                                }
                                if (ordLabObj.OrdersSubmit.Lab_ID == 1)
                                {
                                    ICriteria questionsSetType = iMySession.CreateCriteria(typeof(OrderCodeLibrary)).Add(Expression.Eq("Order_Code", ordLabObj.ObjOrder.Lab_Procedure)).Add(Expression.Eq("Lab_ID", ordLabObj.OrdersSubmit.Lab_ID));
                                    if (questionsSetType.List<OrderCodeLibrary>().Count > 0)
                                    {
                                        ordLabObj.lstQuestionSetNames.Add(questionsSetType.List<OrderCodeLibrary>()[0].Order_Code_Question_Set_Segment);
                                    }
                                }

                            }
                            ordLabList.Add(ordLabObj);
                        }
                    }

                }
                //End
                //        ordLabObj = new OrderLabDetailsDTO();
                //        Orders objOrder = new Orders();
                //        OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                //        //Specimen objSpecimen = new Specimen();
                //        Order_id = Convert.ToUInt64(obj[0]);
                //        Order_submit_id = Convert.ToUInt64(obj[1]);


                //        ICriteria critOrd = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Id", Order_id));
                //        if (critOrd.List<Orders>().Count > 0)
                //        {
                //            ordLabObj.ObjOrder = critOrd.List<Orders>()[0];

                //            AddedOrderIds.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                //        }

                //        ICriteria critOrdSub = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", ordLabObj.ObjOrder.Order_Submit_ID));
                //        if (critOrdSub.List<OrdersSubmit>().Count > 0)
                //        {
                //            ordLabObj.OrdersSubmit = critOrdSub.List<OrdersSubmit>()[0];
                //        }
                //        if (LoadQuestionSet)
                //        {
                //            if (ordLabObj.ObjOrder.Order_Submit_ID == OrderSubmitId)
                //            {


                //                ICriteria critAfp = iMySession.CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                //                if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                //                {
                //                    ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                //                }
                //                ICriteria critbloodLead = iMySession.CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                //                if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                //                {
                //                    ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                //                }
                //                ICriteria critCyto = iMySession.CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                //                if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                //                {
                //                    ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                //                }
                //                ICriteria critAOE = iMySession.CreateCriteria(typeof(OrdersQuestionSetAOE)).Add(Expression.Eq("Orders_ID", ordLabObj.ObjOrder.Id));
                //                if (critAOE.List<OrdersQuestionSetAOE>().Count > 0)
                //                {
                //                    ordLabObj.OrderAOEList = critAOE.List<OrdersQuestionSetAOE>();
                //                }
                //            }
                //            ordLabObj.OrdersSubmit = null;
                //            ordLabObj.ObjOrder = null;
                //        }
                //        else
                //        {
                //            ICriteria critWfobj = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", ordLabObj.OrdersSubmit.Order_Type)).Add(Expression.Eq("Obj_System_Id", ordLabObj.OrdersSubmit.Id));
                //            if (critWfobj.List<WFObject>().Count > 0)
                //            {
                //                ordLabObj.ObjOrder.Internal_Property_Current_Process = critWfobj.List<WFObject>()[0].Current_Process;
                //                if (ordLabObj.ObjOrder.Internal_Property_Current_Process.StartsWith("DELETED_"))
                //                    continue;
                //            }
                //            //New
                //            XmlTextReader XmlTextLab = new XmlTextReader(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\LabList.xml"));
                //            XDocument xmlLab = XDocument.Load(XmlTextLab);

                //            //IEnumerable<XElement> xmlFac = xmlLab.Element("LabList")
                //            //                                .Elements("Facility").Where(aa => aa.Attribute("Name").Value.ToString() == ClientSession.FacilityName);
                //            ////.OrderBy(s => (string)s.Attribute("City"));
                //            //if (xmlFac != null && xmlFac.Count() > 0)
                //            //{
                //            //    LookUpPerRequest.Add("FacilityCity", xmlFac.Attributes("City").First().Value.ToString());
                //            //}
                //            IEnumerable<XElement> xml = xmlLab.Element("LabList")
                //                .Elements("Lab").Where(aa => aa.Attribute("id").Value.ToString() == ordLabObj.OrdersSubmit.Lab_ID.ToString());
                //            if (xml != null && xml.Count() > 0)
                //            {
                //               string sval= xml.Attributes("name").First().Value.ToString();
                //            }
                //            XmlTextLab.Close();
                //            //end

                //            ICriteria critLab = iMySession.CreateCriteria(typeof(Lab)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_ID));
                //            if (critLab.List<Lab>().Count > 0)
                //            {
                //                ordLabObj.LabName = critLab.List<Lab>()[0].Lab_Name;
                //            }
                //            ICriteria critLabLoc = iMySession.CreateCriteria(typeof(LabLocation)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_Location_ID));
                //            if (critLabLoc.List<LabLocation>().Count > 0)
                //            {
                //                ordLabObj.LabLocName = critLabLoc.List<LabLocation>()[0].Location_Name;
                //            }
                //            //ICriteria critSpe = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Specimen_ID));
                //            //if (critSpe.List<Specimen>().Count > 0)
                //            //{
                //            //    ordLabObj.objSpecimen = critSpe.List<Specimen>()[0];
                //            //}
                //            ordLabObj.procedureCodeDesc = ordLabObj.ObjOrder.Lab_Procedure + "-" + ordLabObj.ObjOrder.Lab_Procedure_Description;
                //            ulList.Add(ordLabObj.ObjOrder.Id);

                //            ICriteria critOrdersRequiredForms = iMySession.CreateCriteria(typeof(OrdersRequiredForms)).Add(Expression.Eq("Order_Id", ordLabObj.ObjOrder.Id));
                //            if (critOrdersRequiredForms.List<OrdersRequiredForms>().Count > 0)
                //            {
                //                ordLabObj.ilstOrdersRequiredForms = critOrdersRequiredForms.List<OrdersRequiredForms>();
                //            }
                //            if (ordLabObj.OrdersSubmit.Lab_ID == 1)
                //            {
                //                ICriteria questionsSetType = iMySession.CreateCriteria(typeof(OrderCodeLibrary)).Add(Expression.Eq("Order_Code", ordLabObj.ObjOrder.Lab_Procedure)).Add(Expression.Eq("Lab_ID", ordLabObj.OrdersSubmit.Lab_ID));
                //                if (questionsSetType.List<OrderCodeLibrary>().Count > 0)
                //                {
                //                    ordLabObj.lstQuestionSetNames.Add(questionsSetType.List<OrderCodeLibrary>()[0].Order_Code_Question_Set_Segment);
                //                }
                //            }

                //            //ulSpecimenIDList.Add(ordLabObj.OrdersSubmit.Specimen_ID);
                //        }

                //        //ordLabObj.objSpecimen = objSpecimen;


                //        ordLabList.Add(ordLabObj);

                //    }
                //}

                //AddedOrderIds = AddedOrderIds.Distinct().ToList<ulong>();
                //ISQLQuery queryId = iMySession.CreateSQLQuery("SELECT OS.*,W.* FROM orders_submit OS , wf_object W  WHERE W.obj_system_id=OS.Order_Submit_Id and W.Current_Process!=\"DELETED_ORDER\" And W.Obj_Type=\"DIAGNOSTIC ORDER\" AND OS.Is_Paper_Order='N' AND OS.Encounter_ID=" + EncounterID + ";").AddEntity("OS", typeof(OrdersSubmit)).AddEntity("W", typeof(WFObject));
                //IList<object[]> tempObjectArray = queryId.List<object[]>();
                //IList<OrdersSubmit> templist = new List<OrdersSubmit>();
                //foreach (object[] obj in tempObjectArray)
                //{
                //    OrdersSubmit tempOrdersSubmit = new OrdersSubmit();
                //    tempOrdersSubmit = (OrdersSubmit)obj[0];
                //    tempOrdersSubmit.Culture_Location = ((WFObject)obj[1]).Current_Process;
                //    templist.Add(tempOrdersSubmit);

                //}
                AddedOrderIds = AddedOrderIds.Distinct().ToList<ulong>();
                IList<OrdersSubmit> templist = new List<OrdersSubmit>();

                if (aryorderList != null)  //if (orderList != null)
                {
                    foreach (IList<Object> obj in aryorderList)   //foreach (object[] obj in orderList)
                    {
                        WFObject objwf = new WFObject();
                        OrdersSubmit objos = new OrdersSubmit();
                        if (obj.Count() > 0)
                        {
                            if (obj[4] != null)
                            {
                                objwf = (WFObject)obj[4];
                            }
                            if (obj[1] != null)
                            {
                                objos = (OrdersSubmit)obj[1];
                            }
                            if (objwf.Obj_System_Id == objos.Id && objwf.Current_Process != "DELETED_ORDER" && objwf.Obj_Type == "DIAGNOSTIC ORDER" && objos.Is_Paper_Order == "N" && objos.Encounter_ID == EncounterID)
                            {
                                OrdersSubmit tempOrdersSubmit = new OrdersSubmit();
                                tempOrdersSubmit = (OrdersSubmit)obj[1];
                                tempOrdersSubmit.Culture_Location = ((WFObject)obj[4]).Current_Process;
                                templist.Add(tempOrdersSubmit);
                            }

                        }
                    }
                }

                objOrdersDTO.ilstOrdersSubmitForPartialOrders = templist;
                objOrdersDTO.ilstOrdersSubmitForPartialOrders = objOrdersDTO.ilstOrdersSubmitForPartialOrders.Where(a => !AddedOrderIds.Contains(a.Id)).ToList<OrdersSubmit>();


                objOrdersDTO.ilstOrderLabDetailsDTO = ordLabList;
                aryorderList.Clear(); //orderList.Clear();
                if (ulList.Count > 0 && !LoadQuestionSet)
                {
                    objOrdersDTO.OrderAssList = GetOrdersAssessmentDetails(ulList);
                }






                //ICriteria criteria = session.GetISession().CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteria.List<OrdersSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.OrderSubmitList = criteria.List<OrdersSubmit>();
                //}
                //ICriteria criteriaOrdAss = session.GetISession().CreateCriteria(typeof(OrdersAssessment)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteriaOrdAss.List<OrdersAssessment>().Count > 0)
                //{
                //    objOrderDetDTO.OrderAssList = criteriaOrdAss.List<OrdersAssessment>();
                //}
                //<**>
                //if (ulSpecimenIDList.Count > 0)
                //{
                //    ICriteria criteriaSpecimen = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.In("Id", ulSpecimenIDList.ToArray<ulong>()));
                //    if (criteriaSpecimen.List<Specimen>().Count > 0)
                //    {
                //        objOrderDetDTO.SpecimenList = criteriaSpecimen.List<Specimen>();
                //    }
                //}
                //ICriteria subCrit = session.GetISession().CreateCriteria(typeof(OrdersInternalSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Order_Type", "INTERNAL " + orderType));
                //if (subCrit.List<OrdersInternalSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.objInternalSubmit = subCrit.List<OrdersInternalSubmit>()[0];
                //}
                //else
                //{
                //    objOrderDetDTO.objInternalSubmit = null;
                //}
                //}
                # region In-house view

                //MOHAN
                IList<ulong> lstsubmit_id = objOrdersDTO.ilstOrderLabDetailsDTO.Where(q => q.OrdersSubmit != null).Select(A => A.OrdersSubmit.Id).Distinct().ToList<ulong>();
                for (int k = 0; k < lstsubmit_id.Count; k++)
                {
                    ICriteria critMgnt = iMySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Order_ID", lstsubmit_id[k]));
                    if (critMgnt.List<FileManagementIndex>().Count > 0)

                        foreach (var obj in objOrdersDTO.ilstOrderLabDetailsDTO.Where(Z => Z.OrdersSubmit.Id == lstsubmit_id[k]))
                        {
                            objOrdersDTO.ilstOrderLabDetailsDTO[objOrdersDTO.ilstOrderLabDetailsDTO.IndexOf(obj)].OrdersSubmit.Internal_Property_File_Path = "TRUE";
                        }
                }

                #endregion

                serializer.WriteObject(stream, objOrdersDTO);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;
        }

        public Stream GetOrdersUsingEncounterIDForCMG(ulong CMGEncounterID)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            //OrderDetailsDTO objOrderDetDTO = new OrderDetailsDTO();
            OrdersDTO objOrdersDTO = new OrdersDTO();
            IList<OrderLabDetailsDTO> ordLabList = new List<OrderLabDetailsDTO>();
            OrderLabDetailsDTO ordLabObj = new OrderLabDetailsDTO();
            Orders objOrd = new Orders();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            OrdersAssessment objOrdAss = new OrdersAssessment();
            IList<ulong> ulList = new List<ulong>();
            IList<ulong> ulSpecimenIDList = new List<ulong>();
            IList<Orders> iOrderList = new List<Orders>();
            ArrayList orderList = null;
            ulong Order_id = 0;
            ulong Order_submit_id = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.Orders.GetOrdersUsingEncounterIDAndType.CMG");
                query2.SetString(0, CMGEncounterID.ToString());
                orderList = new ArrayList(query2.List());



                if (orderList != null)
                {
                    foreach (object[] obj in orderList)
                    {
                        ordLabObj = new OrderLabDetailsDTO();
                        Orders objOrder = new Orders();
                        OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                        //Specimen objSpecimen = new Specimen();
                        Order_id = Convert.ToUInt64(obj[0]);
                        Order_submit_id = Convert.ToUInt64(obj[1]);
                        ICriteria critOrd = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Id", Order_id));
                        if (critOrd.List<Orders>().Count > 0)
                        {
                            ordLabObj.ObjOrder = critOrd.List<Orders>()[0];
                        }

                        ICriteria critOrdSub = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", ordLabObj.ObjOrder.Order_Submit_ID));
                        if (critOrdSub.List<OrdersSubmit>().Count > 0)
                        {
                            ordLabObj.OrdersSubmit = critOrdSub.List<OrdersSubmit>()[0];



                        }
                        //if (LoadQuestionSet)
                        //{
                        //    if (ordLabObj.ObjOrder.Order_Submit_ID == OrderSubmitId)
                        //    {
                        //        ICriteria critAfp = session.GetISession().CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                        //        if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                        //        {
                        //            ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                        //        }
                        //        ICriteria critbloodLead = session.GetISession().CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                        //        if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                        //        {
                        //            ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                        //        }
                        //        ICriteria critCyto = session.GetISession().CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                        //        if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                        //        {
                        //            ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                        //        }
                        //        ICriteria critAOE = session.GetISession().CreateCriteria(typeof(OrdersQuestionSetAOE)).Add(Expression.Eq("Orders_ID", ordLabObj.ObjOrder.Id));
                        //        if (critAOE.List<OrdersQuestionSetAOE>().Count > 0)
                        //        {
                        //            ordLabObj.OrderAOEList = critAOE.List<OrdersQuestionSetAOE>();
                        //        }
                        //    }
                        //    ordLabObj.OrdersSubmit = null;
                        //    ordLabObj.ObjOrder = null;
                        //}
                        //else
                        //{
                        ICriteria critWfobj = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", ordLabObj.OrdersSubmit.Id));
                        if (critWfobj.List<WFObject>().Count > 0)
                        {
                            ordLabObj.ObjOrder.Internal_Property_Current_Process = critWfobj.List<WFObject>()[0].Current_Process;
                            if (ordLabObj.ObjOrder.Internal_Property_Current_Process.StartsWith("DELETED_"))
                                continue;
                        }
                        ICriteria critLab = iMySession.CreateCriteria(typeof(Lab)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_ID));
                        if (critLab.List<Lab>().Count > 0)
                        {
                            ordLabObj.LabName = critLab.List<Lab>()[0].Lab_Name;
                        }
                        ICriteria critLabLoc = iMySession.CreateCriteria(typeof(LabLocation)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_Location_ID));
                        if (critLabLoc.List<LabLocation>().Count > 0)
                        {
                            ordLabObj.LabLocName = critLabLoc.List<LabLocation>()[0].Location_Name;
                        }
                        //ICriteria critSpe = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Specimen_ID));
                        //if (critSpe.List<Specimen>().Count > 0)
                        //{
                        //    ordLabObj.objSpecimen = critSpe.List<Specimen>()[0];
                        //}
                        ordLabObj.procedureCodeDesc = ordLabObj.ObjOrder.Lab_Procedure + "-" + ordLabObj.ObjOrder.Lab_Procedure_Description;
                        //ulList.Add(ordLabObj.ObjOrder.Id);
                        ulList.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                        //ulSpecimenIDList.Add(ordLabObj.OrdersSubmit.Specimen_ID);
                        //}

                        //ordLabObj.objSpecimen = objSpecimen;


                        ordLabList.Add(ordLabObj);

                    }
                    //Added By ThiyagarajanM (12-02-2013 For CMG)
                    //SELECT order_id,encounter_id,human_id,physician_id FROM `orders` where cmg_encounter_id=65402 order by order_id asc;
                    //ICriteria critID=session.GetISession().CreateCriteria(typeof(Order)).Add(Expression.Eq("CMG_Encounter_ID",CMGEncounterID)).AddOrder(Orders.Asc("Id"));








                    try
                    {

                        ISQLQuery queryId = iMySession.CreateSQLQuery("SELECT O.* FROM orders as O WHERE O.CMG_Encounter_ID='" + CMGEncounterID + "'order by O.Order_ID asc").AddEntity("O", typeof(Orders));
                        objOrdersDTO.CMGEncounter_Physician_ID = queryId.List<Orders>()[0].Physician_ID;
                    }
                    catch
                    {
                        objOrdersDTO.CMGEncounter_Physician_ID = 0;
                    }

                    //objOrdersDTO.Lists = queryId.List<Orders>();

                }



                objOrdersDTO.ilstOrderLabDetailsDTO = ordLabList;
                orderList.Clear();
                if (ulList.Count > 0)
                {
                    objOrdersDTO.OrderAssList = GetOrdersAssessmentDetails(ulList);
                }

                //ICriteria criteria = session.GetISession().CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteria.List<OrdersSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.OrderSubmitList = criteria.List<OrdersSubmit>();
                //}
                //ICriteria criteriaOrdAss = session.GetISession().CreateCriteria(typeof(OrdersAssessment)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteriaOrdAss.List<OrdersAssessment>().Count > 0)
                //{
                //    objOrderDetDTO.OrderAssList = criteriaOrdAss.List<OrdersAssessment>();
                //}
                //<**>
                //if (ulSpecimenIDList.Count > 0)
                //{
                //    ICriteria criteriaSpecimen = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.In("Id", ulSpecimenIDList.ToArray<ulong>()));
                //    if (criteriaSpecimen.List<Specimen>().Count > 0)
                //    {
                //        objOrderDetDTO.SpecimenList = criteriaSpecimen.List<Specimen>();
                //    }
                //}
                //ICriteria subCrit = session.GetISession().CreateCriteria(typeof(OrdersInternalSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Order_Type", "INTERNAL " + orderType));
                //if (subCrit.List<OrdersInternalSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.objInternalSubmit = subCrit.List<OrdersInternalSubmit>()[0];
                //}
                //else
                //{
                //    objOrderDetDTO.objInternalSubmit = null;
                //}
                //}



                //MOhan



                # region In-house view

                IList<ulong> lstsubmit_id = objOrdersDTO.ilstOrderLabDetailsDTO.Where(q => q.OrdersSubmit != null).Select(A => A.OrdersSubmit.Id).Distinct().ToList<ulong>();
                for (int k = 0; k < lstsubmit_id.Count; k++)
                {

                    ICriteria critMgnt = iMySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Order_ID", lstsubmit_id[k])).Add(Expression.Eq("Result_Master_ID", CMGEncounterID)).Add(Expression.Eq("Source", "ORDER"));
                    if (critMgnt.List<FileManagementIndex>().Count > 0)
                        foreach (var obj in objOrdersDTO.ilstOrderLabDetailsDTO.Where(Z => Z.OrdersSubmit.Id == lstsubmit_id[k]))
                        {
                            if (critMgnt.List<FileManagementIndex>().Any(a => a.File_Path != string.Empty))
                                objOrdersDTO.ilstOrderLabDetailsDTO[objOrdersDTO.ilstOrderLabDetailsDTO.IndexOf(obj)].OrdersSubmit.Internal_Property_File_Path = "TRUE";
                            else
                                objOrdersDTO.ilstOrderLabDetailsDTO[objOrdersDTO.ilstOrderLabDetailsDTO.IndexOf(obj)].OrdersSubmit.Internal_Property_File_Path = string.Empty;
                        }
                }


                #endregion

                //End





                serializer.WriteObject(stream, objOrdersDTO);
                stream.Seek(0L, SeekOrigin.Begin);

                iMySession.Close();
            }
            return stream;
        }

        ulong OrderSubmitId = 0;
        public Stream LoadOrdersAtCheckOut(DateTime Modified_Date_And_Time, ulong HumanID, string LabType)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objDTO = new OrdersDTO();
            //OrderDetailsDTO objdetDTO = new OrderDetailsDTO();
            IList<ulong> ulList = new List<ulong>();
            IList<string> IcDCodes = new List<string>();
            ProblemListManager probMgr = new ProblemListManager();
            objDTO.MedAdvProblemList = probMgr.GetProblemList(HumanID);
            object objStream = (object)serializer.ReadObject(GetOrdersUsingOrderSubmitID(Modified_Date_And_Time, LabType));
            objDTO = (OrdersDTO)objStream;
            objDTO.objHuman = GetHumanById(HumanID);
            serializer.WriteObject(stream, objDTO);
            stream.Seek(0L, SeekOrigin.Begin);
            return stream;




        }
        public Stream LoadOrders(ulong EncounterID, ulong PhysicianID, ulong HumanID, string orderType, string procedureType, DateTime todaysDate, bool LoadQuestionSet)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objDTO = new OrdersDTO();
            IList<ulong> ulList = new List<ulong>();
            IList<string> IcDCodes = new List<string>();
            if (LoadQuestionSet)
            {
                OrderSubmitId = Convert.ToUInt32(procedureType);
            }
            if (EncounterID != 0)
            {
                object objStream = (object)serializer.ReadObject(GetOrdersUsingEncounterID(EncounterID, orderType, LoadQuestionSet));
                objDTO = (OrdersDTO)objStream;

            }
            else
            {
                object objStream = (object)serializer.ReadObject(GetOrdersUsingHumanID(HumanID, todaysDate, orderType, PhysicianID, LoadQuestionSet));
                objDTO = (OrdersDTO)objStream;
            }
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (!LoadQuestionSet)
                {
                    ProblemListManager probMgr = new ProblemListManager();
                    objDTO.MedAdvProblemList = probMgr.GetProblemList(HumanID);

                    ICriteria criteria1 = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Assessment_Type", "Selected"));
                    objDTO.AssessmentList = criteria1.List<Assessment>();


                    ICriteria crit = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Results_Type", "Vitals"))
                        .SetProjection(Projections.Max("Vitals_Group_ID"));
                    ulong vitalsId = 0;
                    if (crit.List<object>().Count > 0)
                    {
                        vitalsId = Convert.ToUInt64(crit.List<object>()[0]);
                    }
                    IList<string> VitalName = new List<string>();
                    VitalName.Add("Height");
                    VitalName.Add("Weight");
                    //ICriteria criteriaVital = session.GetISession().CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Vital_Group_ID", vitalsId)).Add(Expression.In("Loinc_Observation", VitalName.ToArray<string>())).Add(Expression.Eq("Results_Type", "Vitals"));
                    //objDTO.VitalsList = criteriaVital.List<PatientResults>();

                    objDTO.objHuman = GetHumanById(HumanID);
                }

                //ICriteria criteriaHuman = session.GetISession().CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID));
                //if (criteriaHuman.List<Human>().Count > 0)
                //{
                //    objDTO.objHuman = criteriaHuman.List<Human>()[0];
                //}

                //ICriteria phy = session.GetISession().CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", PhysicianID));
                //.SetProjection(Projections.ProjectionList()
                //.Add(Projections.Property("PhyPrefix"))
                //.Add(Projections.Property("PhyFirstName"))
                //.Add(Projections.Property("PhyMiddleName"))
                //.Add(Projections.Property("PhyLastName"))
                //.Add(Projections.Property("PhySuffix"))
                //.Add(Projections.Property("PhyNPI")))
                //.Add(Expression.Eq("Id", PhysicianID)).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(PhysicianLibrary)));
                //if (phy.List<PhysicianLibrary>().Count > 0)
                //{
                //    objDTO.objPhysician = phy.List<PhysicianLibrary>()[0];
                //}
                //if (objDTO.objHuman != null && objDTO.objHuman.PatientInsuredBag != null && objDTO.objHuman.PatientInsuredBag.Count > 0)
                //{
                //    ArrayList orderList = null;
                //    IList<ulong> idList = (from pat in objDTO.objHuman.PatientInsuredBag select pat.Insurance_Plan_ID).ToList<ulong>();
                //    string strLabType = orderType.Split(' ')[0];
                //    IQuery query2 = session.GetISession().GetNamedQuery("Get.Orders.GetLabIDBasedOnInsPlan");
                //    query2.SetString(0, strLabType);
                //    query2.SetParameterList("InsList", idList.ToArray<ulong>());
                //    orderList = new ArrayList(query2.List());
                //    foreach (object o in orderList)
                //    {
                //        if (o != null)
                //        {
                //            objDTO.LabIDListBasedOnInsID.Add(Convert.ToUInt64(o));
                //        }
                //    }
                //    if (objDTO.LabIDListBasedOnInsID != null && objDTO.LabIDListBasedOnInsID.Count > 0)
                //    {
                //        ICriteria criteriaPro = session.GetISession().CreateCriteria(typeof(PhysicianProcedure))
                //            .Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Procedure_Type", procedureType))
                //            .Add(Expression.Eq("Lab_ID", objDTO.LabIDListBasedOnInsID[0]))
                //            .AddOrder(Order.Asc("Physician_Procedure_Code"));
                //        objDTO.ProcedureList = criteriaPro.List<PhysicianProcedure>();
                //    }
                //}
                //objfrmOrdersList.objOrderDTO.ilstOrderLabDetailsDTO

                //# region In-house view
                //            if (!LoadQuestionSet)
                //            {
                //                IList<ulong> lstsubmit_id = objDTO.ilstOrderLabDetailsDTO.Where(q => q.OrdersSubmit != null).Select(A => A.OrdersSubmit.Id).Distinct().ToList<ulong>();
                //                for (int k = 0; k < lstsubmit_id.Count; k++)
                //                {
                //                    ICriteria critMgnt = session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Order_ID", lstsubmit_id[k]));
                //                    if (critMgnt.List<FileManagementIndex>().Count > 0)

                //                        foreach (var obj in objDTO.ilstOrderLabDetailsDTO.Where(Z => Z.OrdersSubmit.Id == lstsubmit_id[k]))
                //                        {
                //                            objDTO.ilstOrderLabDetailsDTO[objDTO.ilstOrderLabDetailsDTO.IndexOf(obj)].OrdersSubmit.File_Path = "TRUE";
                //                        }
                //                }
                //            }
                //#endregion
                serializer.WriteObject(stream, objDTO);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;

        }

        public Stream LoadOrdersByOrdersSubmitedId(ulong OrderSubmitId, ulong EncounterID, ulong PhysicianID, ulong HumanID, string orderType, string procedureType, DateTime todaysDate, bool LoadQuestionSet)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objDTO = new OrdersDTO();
            IList<ulong> ulList = new List<ulong>();
            IList<string> IcDCodes = new List<string>();
            if (LoadQuestionSet)
            {
                OrderSubmitId = Convert.ToUInt32(procedureType);
            }
            if (EncounterID != 0)
            {
                object objStream = (object)serializer.ReadObject(GetOrdersUsingEncounterIDByOrderSubmitID(OrderSubmitId,EncounterID, orderType, LoadQuestionSet));
                objDTO = (OrdersDTO)objStream;

            }
            else
            {
                object objStream = (object)serializer.ReadObject(GetOrdersUsingHumanIDByOrderSubmitID(OrderSubmitId,HumanID, todaysDate, orderType, PhysicianID, LoadQuestionSet));
                objDTO = (OrdersDTO)objStream;
            }
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (!LoadQuestionSet)
                {
                    ProblemListManager probMgr = new ProblemListManager();
                    objDTO.MedAdvProblemList = probMgr.GetProblemList(HumanID);

                    ICriteria criteria1 = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Assessment_Type", "Selected"));
                    objDTO.AssessmentList = criteria1.List<Assessment>();


                    ICriteria crit = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Results_Type", "Vitals"))
                        .SetProjection(Projections.Max("Vitals_Group_ID"));
                    ulong vitalsId = 0;
                    if (crit.List<object>().Count > 0)
                    {
                        vitalsId = Convert.ToUInt64(crit.List<object>()[0]);
                    }
                    IList<string> VitalName = new List<string>();
                    VitalName.Add("Height");
                    VitalName.Add("Weight");
                    //ICriteria criteriaVital = session.GetISession().CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Vital_Group_ID", vitalsId)).Add(Expression.In("Loinc_Observation", VitalName.ToArray<string>())).Add(Expression.Eq("Results_Type", "Vitals"));
                    //objDTO.VitalsList = criteriaVital.List<PatientResults>();

                    objDTO.objHuman = GetHumanById(HumanID);
                }

                //ICriteria criteriaHuman = session.GetISession().CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID));
                //if (criteriaHuman.List<Human>().Count > 0)
                //{
                //    objDTO.objHuman = criteriaHuman.List<Human>()[0];
                //}

                //ICriteria phy = session.GetISession().CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", PhysicianID));
                //.SetProjection(Projections.ProjectionList()
                //.Add(Projections.Property("PhyPrefix"))
                //.Add(Projections.Property("PhyFirstName"))
                //.Add(Projections.Property("PhyMiddleName"))
                //.Add(Projections.Property("PhyLastName"))
                //.Add(Projections.Property("PhySuffix"))
                //.Add(Projections.Property("PhyNPI")))
                //.Add(Expression.Eq("Id", PhysicianID)).SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(PhysicianLibrary)));
                //if (phy.List<PhysicianLibrary>().Count > 0)
                //{
                //    objDTO.objPhysician = phy.List<PhysicianLibrary>()[0];
                //}
                //if (objDTO.objHuman != null && objDTO.objHuman.PatientInsuredBag != null && objDTO.objHuman.PatientInsuredBag.Count > 0)
                //{
                //    ArrayList orderList = null;
                //    IList<ulong> idList = (from pat in objDTO.objHuman.PatientInsuredBag select pat.Insurance_Plan_ID).ToList<ulong>();
                //    string strLabType = orderType.Split(' ')[0];
                //    IQuery query2 = session.GetISession().GetNamedQuery("Get.Orders.GetLabIDBasedOnInsPlan");
                //    query2.SetString(0, strLabType);
                //    query2.SetParameterList("InsList", idList.ToArray<ulong>());
                //    orderList = new ArrayList(query2.List());
                //    foreach (object o in orderList)
                //    {
                //        if (o != null)
                //        {
                //            objDTO.LabIDListBasedOnInsID.Add(Convert.ToUInt64(o));
                //        }
                //    }
                //    if (objDTO.LabIDListBasedOnInsID != null && objDTO.LabIDListBasedOnInsID.Count > 0)
                //    {
                //        ICriteria criteriaPro = session.GetISession().CreateCriteria(typeof(PhysicianProcedure))
                //            .Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Procedure_Type", procedureType))
                //            .Add(Expression.Eq("Lab_ID", objDTO.LabIDListBasedOnInsID[0]))
                //            .AddOrder(Order.Asc("Physician_Procedure_Code"));
                //        objDTO.ProcedureList = criteriaPro.List<PhysicianProcedure>();
                //    }
                //}
                //objfrmOrdersList.objOrderDTO.ilstOrderLabDetailsDTO

                //# region In-house view
                //            if (!LoadQuestionSet)
                //            {
                //                IList<ulong> lstsubmit_id = objDTO.ilstOrderLabDetailsDTO.Where(q => q.OrdersSubmit != null).Select(A => A.OrdersSubmit.Id).Distinct().ToList<ulong>();
                //                for (int k = 0; k < lstsubmit_id.Count; k++)
                //                {
                //                    ICriteria critMgnt = session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Order_ID", lstsubmit_id[k]));
                //                    if (critMgnt.List<FileManagementIndex>().Count > 0)

                //                        foreach (var obj in objDTO.ilstOrderLabDetailsDTO.Where(Z => Z.OrdersSubmit.Id == lstsubmit_id[k]))
                //                        {
                //                            objDTO.ilstOrderLabDetailsDTO[objDTO.ilstOrderLabDetailsDTO.IndexOf(obj)].OrdersSubmit.File_Path = "TRUE";
                //                        }
                //                }
                //            }
                //#endregion
                serializer.WriteObject(stream, objDTO);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;

        }

        public Stream LoadOrdersCMG(ulong EncounterID, ulong HumanID)
        {

            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objDTO = new OrdersDTO();
            IList<ulong> ulList = new List<ulong>();
            IList<string> IcDCodes = new List<string>();
            object objStream = (object)serializer.ReadObject(GetOrdersUsingEncounterIDForCMG(EncounterID));
            objDTO = (OrdersDTO)objStream;
            serializer.WriteObject(stream, objDTO);
            stream.Seek(0L, SeekOrigin.Begin);
            return stream;
        }


        public IList<OrdersAssessment> GetOrdersAssessmentDetails(IList<ulong> OrderIDList)
        {
            IList<OrdersAssessment> list = new List<OrdersAssessment>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(OrdersAssessment)).Add(Expression.In("Order_Submit_ID", OrderIDList.ToArray<ulong>()));
                list = crit.List<OrdersAssessment>();
                iMySession.Close();
            }
            return list;
        }

        public IList<OrdersProblemList> GetOrdersProblemListDetails(IList<ulong> OrderIDList)
        {
            IList<OrdersProblemList> ordAssList = new List<OrdersProblemList>();
            OrdersProblemList objOrdAss = new OrdersProblemList();
            ArrayList orderList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (OrderIDList.Count > 0)
                {
                    IQuery query3 = iMySession.GetNamedQuery("Fill.Orders.GetOrdersProblemList");
                    query3.SetParameterList("OrderIDList", OrderIDList.ToArray<ulong>());

                    orderList = new ArrayList(query3.List());

                    foreach (object[] obj in orderList)
                    {
                        objOrdAss = new OrdersProblemList();
                        objOrdAss.Id = Convert.ToUInt64(obj[0]);
                        objOrdAss.Order_ID = Convert.ToUInt64(obj[1]);
                        objOrdAss.Problem_List_ID = Convert.ToUInt64(obj[2]);
                        objOrdAss.Encounter_ID = Convert.ToUInt64(obj[3].ToString());
                        objOrdAss.ICD = obj[4].ToString();
                        objOrdAss.ICD_Description = obj[5].ToString();
                        objOrdAss.Created_By = obj[6].ToString();
                        objOrdAss.Created_Date_And_Time = Convert.ToDateTime(obj[7]);
                        objOrdAss.Modified_By = obj[8].ToString();
                        objOrdAss.Modified_Date_And_Time = Convert.ToDateTime(obj[9]);
                        objOrdAss.Version = Convert.ToInt16(obj[10]);
                        ordAssList.Add(objOrdAss);
                    }
                }
                iMySession.Close();
            }
            return ordAssList;
        }



        public IList<OrderLabDetailsDTO> LoadOrdersForCollectSpecimen(ulong OrderSubmitID, string orderType, out IList<OrdersAssessment> OrderAssList)
        {
            IList<OrderLabDetailsDTO> objOrderDetDTO = new List<OrderLabDetailsDTO>();
            IList<OrderLabDetailsDTO> ordLabList = new List<OrderLabDetailsDTO>();
            OrderLabDetailsDTO ordLabObj = new OrderLabDetailsDTO();
            Orders objOrd = new Orders();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            OrdersAssessment objOrdAss = new OrdersAssessment();
            IList<ulong> ulList = new List<ulong>();
            IList<Orders> iOrderList = new List<Orders>();
            ArrayList orderList = null;
            LabManager objLabManager = new LabManager();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.Orders.GetOrdersForSpecimen");
                query2.SetString(0, OrderSubmitID.ToString());
                query2.SetString(1, orderType);

                orderList = new ArrayList(query2.List());

                if (orderList != null)
                {
                    if (orderList.Count == 0)
                    {
                        OrderAssList = new List<OrdersAssessment>();
                        return objOrderDetDTO;
                    }
                    foreach (object[] obj in orderList)
                    {
                        ordLabObj = new OrderLabDetailsDTO();
                        OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                        //Specimen objSpecimen = new Specimen();
                        Orders objOrder = new Orders();
                        objOrder.Id = Convert.ToUInt64(obj[0]);
                        objOrder.Encounter_ID = Convert.ToUInt64(obj[1]);
                        objOrder.Human_ID = Convert.ToUInt64(obj[2]);
                        objOrder.Physician_ID = Convert.ToUInt64(obj[3]);
                        objOrdersSubmit.Lab_Location_ID = Convert.ToUInt64(obj[4]);
                        objOrdersSubmit.Lab_ID = Convert.ToUInt64(obj[5]);
                        objOrder.Lab_Procedure = obj[6].ToString();
                        objOrdersSubmit.Authorization_Required = obj[7].ToString();
                        objOrdersSubmit.Test_Date = Convert.ToString(obj[8]);
                        objOrdersSubmit.Order_Notes = obj[9].ToString();
                        objOrdersSubmit.Specimen_In_House = obj[10].ToString();
                        objOrdersSubmit.Stat = obj[11].ToString();
                        objOrdersSubmit.Fasting = obj[12].ToString();
                        objOrder.Created_Date_And_Time = Convert.ToDateTime(obj[13]);
                        objOrder.Created_By = obj[14].ToString();
                        objOrder.Created_Date_And_Time = Convert.ToDateTime(obj[15]);
                        objOrder.Modified_By = obj[16].ToString();
                        objOrder.Modified_Date_And_Time = Convert.ToDateTime(obj[17]);
                        objOrder.Version = Convert.ToInt16(obj[18]);
                        objOrder.Lab_Procedure_Description = obj[19].ToString();
                        objOrdersSubmit.Order_Type = obj[20].ToString();
                        //objOrdersSubmit.Specimen_ID = Convert.ToUInt64(obj[21]);
                        objOrdersSubmit.Facility_Name = obj[22].ToString();
                        objOrdersSubmit.Height = obj[23].ToString();
                        objOrdersSubmit.Weight = obj[24].ToString();
                        objOrder.Order_Code_Type = obj[25].ToString();
                        objOrdersSubmit.Bill_Type = obj[26].ToString();
                        objOrdersSubmit.Is_ABN_Signed = obj[27].ToString();
                        objOrdersSubmit.Specimen_Type = obj[28].ToString();
                        objOrdersSubmit.Culture_Location = obj[29].ToString();
                        //objOrder.Order_Submit_ID = Convert.ToUInt64(obj[30]);
                        //objOrdersSubmit.Is_In_House = obj[30].ToString();
                        //objOrdersSubmit.Orders_Internal_Submit_ID = Convert.ToUInt64(obj[31]);
                        //objOrdersSubmit.TestDate_In_Months = Convert.ToInt16(obj[32]);
                        //objOrdersSubmit.TestDate_In_Weeks = Convert.ToInt16(obj[33]);
                        //objOrdersSubmit.TestDate_In_Days = Convert.ToInt16(obj[34]);
                        //objOrdersSubmit.Patient_Instructions = obj[35].ToString();
                        objOrdersSubmit.Temperature = obj[32].ToString();
                        if (obj[33] != null && !(obj[33].ToString().StartsWith("0001-01-01")))
                            objOrdersSubmit.Specimen_Collection_Date_And_Time = Convert.ToDateTime(obj[33]);
                        objOrdersSubmit.Created_Date_And_Time = Convert.ToDateTime(obj[34]);
                        objOrdersSubmit.Modified_Date_And_Time = Convert.ToDateTime(obj[35]);
                        objOrdersSubmit.Quantity = Convert.ToInt32(obj[36]);
                        objOrdersSubmit.Specimen_Unit = obj[37].ToString();

                        ordLabObj.ObjOrder = objOrder;
                        if (obj[30] != null)
                        {
                            ordLabObj.LabName = obj[30].ToString();
                        }
                        if (obj[31] != null)
                        {
                            ordLabObj.LabLocName = obj[31].ToString();
                        }
                        ordLabObj.procedureCodeDesc = obj[6].ToString() + "-" + obj[19].ToString();

                        //ICriteria critSta = session.GetISession().CreateCriteria(typeof(StandingOrder)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critSta.List<StandingOrder>().Count > 0)
                        //{
                        //    ordLabObj.objStandingOrder = critSta.List<StandingOrder>()[0];
                        //}
                        //ICriteria critAfp = iMySession.CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                        //{
                        //    ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                        //}
                        //ICriteria critbloodLead = iMySession.CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                        //{
                        //    ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                        //}
                        //ICriteria critCyto = iMySession.CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                        //{
                        //    ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                        //}
                        //ICriteria critAOE = iMySession.CreateCriteria(typeof(OrdersQuestionSetAOE)).Add(Expression.Eq("Orders_ID", objOrder.Id));
                        //if (critAOE.List<OrdersQuestionSetAOE>().Count > 0)
                        //{
                        //    ordLabObj.OrderAOEList = critAOE.List<OrdersQuestionSetAOE>();
                        //}

                        //if (objOrdersSubmit.Lab_ID == (from rec in objLabManager.GetLabDetails("LAB") where rec.Lab_Name == "Quest Diagnostics" && rec.Lab_Type == "LAB" select rec.Id).SingleOrDefault())
                        //{
                        //    OrderComponentsManager objOrderComponentsManager=new OrderComponentsManager();
                        //    ordLabObj.SubComponents=objOrderComponentsManager.GetByOrderCode(ordLabObj.ObjOrder.Lab_Procedure);
                        //}
                        ordLabObj.OrdersSubmit = objOrdersSubmit;
                        //ordLabObj.objSpecimen = objSpecimen;
                        ordLabList.Add(ordLabObj);
                        //ulList.Add(objOrder.Id);
                        ulList.Add(OrderSubmitID);
                    }
                }
                objOrderDetDTO = ordLabList;
                orderList.Clear();

                if (ulList.Count > 0)
                {
                    OrderAssList = GetOrdersAssessmentDetails(ulList);
                    //objOrderDetDTO.OrderPblmList = GetOrdersProblemListDetails(ulList);
                }
                else
                {
                    OrderAssList = new List<OrdersAssessment>();
                }
                //ICriteria criteria = session.GetISession().CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //objOrderDetDTO.OrderSubmitList = criteria.List<OrdersSubmit>();

                iMySession.Close();
            }
            return objOrderDetDTO;
        }


        public IList<OrderLabDetailsDTO> GetOrdersUsingOrderID(IList<ulong> OrderIDList)
        {
            IList<OrderLabDetailsDTO> ilstOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();

            //   OrdersDTO objOrdersDTO = new OrdersDTO();
            //OrderDetailsDTO objOrderDetDTO = new OrderDetailsDTO();
            IList<OrderLabDetailsDTO> ordLabList = new List<OrderLabDetailsDTO>();
            OrderLabDetailsDTO ordLabObj = new OrderLabDetailsDTO();
            Orders objOrd = new Orders();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            OrdersAssessment objOrdAss = new OrdersAssessment();
            IList<ulong> ulList = new List<ulong>();
            IList<Orders> iOrderList = new List<Orders>();
            ArrayList orderList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.Orders.GetOrdersUsingOrderID");
                query2.SetParameterList("IDList", OrderIDList.ToArray<ulong>());
                orderList = new ArrayList(query2.List());

                if (orderList != null)
                {
                    foreach (object[] obj in orderList)
                    {
                        ordLabObj = new OrderLabDetailsDTO();
                        OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                        //Specimen objSpecimen = new Specimen();
                        Orders objOrder = new Orders();
                        objOrder.Id = Convert.ToUInt64(obj[0]);
                        objOrder.Encounter_ID = Convert.ToUInt64(obj[1]);
                        objOrder.Human_ID = Convert.ToUInt64(obj[2]);
                        objOrder.Physician_ID = Convert.ToUInt64(obj[3]);
                        objOrdersSubmit.Lab_Location_ID = Convert.ToUInt64(obj[4]);
                        objOrdersSubmit.Lab_ID = Convert.ToUInt64(obj[5]);
                        objOrder.Lab_Procedure = obj[6].ToString();
                        objOrdersSubmit.Authorization_Required = obj[7].ToString();
                        objOrdersSubmit.Test_Date = Convert.ToString(obj[8]);
                        objOrdersSubmit.Order_Notes = obj[9].ToString();
                        objOrdersSubmit.Specimen_In_House = obj[10].ToString();
                        objOrdersSubmit.Stat = obj[11].ToString();
                        objOrdersSubmit.Fasting = obj[12].ToString();
                        objOrdersSubmit.Created_Date_And_Time = Convert.ToDateTime(obj[13]);
                        objOrder.Created_By = obj[14].ToString();
                        objOrder.Created_Date_And_Time = Convert.ToDateTime(obj[15]);
                        objOrder.Modified_By = obj[16].ToString();
                        objOrder.Modified_Date_And_Time = Convert.ToDateTime(obj[17]);
                        objOrder.Version = Convert.ToInt16(obj[18]);
                        objOrder.Lab_Procedure_Description = obj[19].ToString();
                        objOrdersSubmit.Order_Type = obj[20].ToString();
                        //objOrdersSubmit.Specimen_ID = Convert.ToUInt64(obj[21]);
                        objOrdersSubmit.Facility_Name = obj[22].ToString();
                        objOrdersSubmit.Height = obj[23].ToString();
                        objOrdersSubmit.Weight = obj[24].ToString();
                        objOrder.Order_Code_Type = obj[25].ToString();
                        objOrdersSubmit.Bill_Type = obj[26].ToString();
                        objOrdersSubmit.Is_ABN_Signed = obj[27].ToString();
                        objOrdersSubmit.Specimen_Type = obj[28].ToString();
                        objOrdersSubmit.Culture_Location = obj[29].ToString();
                        //objOrder.Order_Submit_ID = Convert.ToUInt64(obj[30]);
                        //objOrdersSubmit.Is_In_House = obj[30].ToString();
                        //objOrder.Orders_Internal_Submit_ID = Convert.ToUInt64(obj[31]);
                        //objOrder.TestDate_In_Months = Convert.ToInt16(obj[32]);
                        //objOrder.TestDate_In_Weeks = Convert.ToInt16(obj[33]);
                        //objOrder.TestDate_In_Days = Convert.ToInt16(obj[34]);
                        //objOrder.Patient_Instructions = obj[35].ToString();
                        objOrdersSubmit.Move_To_MA = obj[30].ToString();
                        objOrder.Order_Submit_ID = Convert.ToUInt64(obj[33].ToString());
                        ordLabObj.ObjOrder = objOrder;
                        if (obj[31] != null)
                        {
                            ordLabObj.LabName = obj[31].ToString();
                        }
                        if (obj[32] != null)
                        {
                            ordLabObj.LabLocName = obj[32].ToString();
                        }
                        ordLabObj.procedureCodeDesc = obj[6].ToString() + "-" + obj[19].ToString();

                        //ICriteria critSta = session.GetISession().CreateCriteria(typeof(StandingOrder)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critSta.List<StandingOrder>().Count > 0)
                        //{
                        //    ordLabObj.objStandingOrder = critSta.List<StandingOrder>()[0];
                        //}
                        //ICriteria critAfp = iMySession.CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                        //{
                        //    ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                        //}
                        //ICriteria critbloodLead = iMySession.CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                        //{
                        //    ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                        //}
                        //ICriteria critCyto = iMySession.CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", objOrder.Id));
                        //if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                        //{
                        //    ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                        //}
                        ordLabObj.OrdersSubmit = objOrdersSubmit;
                        //ordLabObj.objSpecimen = objSpecimen;
                        ordLabList.Add(ordLabObj);
                        //ulList.Add(objOrder.Id);
                        ulList.Add(objOrder.Order_Submit_ID);
                    }
                    ilstOrderLabDetailsDTO = ordLabList;
                    orderList.Clear();
                    //if (ulList.Count > 0)
                    //{
                    //    objOrdersDTO.OrderAssList = GetOrdersAssessmentDetails(ulList);
                    //    // objOrderDetDTO.OrderPblmList = GetOrdersProblemListDetails(ulList);
                    //}

                }
                iMySession.Close();
            }
            return ilstOrderLabDetailsDTO;
        }

        public Stream GetOrdersUsingHumanID(ulong HumanID, DateTime createdDate, string OrderType, ulong PhysicianId, bool LoadQuestionSet)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objOrdersDTO = new OrdersDTO();

            //OrderDetailsDTO objOrderDetDTO = new OrderDetailsDTO();
            IList<OrderLabDetailsDTO> ordLabList = new List<OrderLabDetailsDTO>();
            OrderLabDetailsDTO ordLabObj = new OrderLabDetailsDTO();
            Orders objOrd = new Orders();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            OrdersAssessment objOrdAss = new OrdersAssessment();
            IList<ulong> ulList = new List<ulong>();
            IList<ulong> ulSpecimenIDList = new List<ulong>();
            IList<Orders> iOrderList = new List<Orders>();
            ArrayList orderList = null;
            string split = createdDate.ToString("yyyy-MM-dd");
            ulong Order_id = 0;
            ulong Order_submit_id = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.Orders.GetOrdersUsingHumanID");
                query2.SetString(0, HumanID.ToString());
                query2.SetString(1, "%%");
                query2.SetString(2, OrderType);
                query2.SetString(3, "0");
                query2.SetString(4, PhysicianId.ToString());
                orderList = new ArrayList(query2.List());

                IList<ulong> AddedOderSubmitID = new List<ulong>();


                //objOrderDetDTO.OrderCount = Convert.ToInt16(orderList.Count); //commented by prabu 23/01/2012
                if (orderList != null)
                {
                    foreach (object[] obj in orderList)
                    {
                        ordLabObj = new OrderLabDetailsDTO();
                        OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                        //Specimen objSpecimen = new Specimen();
                        Orders objOrder = new Orders();
                        Order_id = Convert.ToUInt64(obj[0]);
                        Order_submit_id = Convert.ToUInt64(obj[1]);


                        //objOrder.Id = Convert.ToUInt64(obj[0]);
                        //objOrder.Encounter_ID = Convert.ToUInt64(obj[1]);
                        //objOrder.Human_ID = Convert.ToUInt64(obj[2]);
                        //objOrder.Physician_ID = Convert.ToUInt64(obj[3]);
                        //objOrdersSubmit.Lab_Location_ID = Convert.ToUInt64(obj[4]);
                        //objOrdersSubmit.Lab_ID = Convert.ToUInt64(obj[5]);
                        //objOrder.Lab_Procedure = obj[6].ToString();
                        //objOrdersSubmit.Authorization_Required = obj[7].ToString();
                        //objOrdersSubmit.Test_Date = Convert.ToString(obj[8]);
                        //objOrdersSubmit.Order_Notes = obj[9].ToString();
                        //objOrdersSubmit.Specimen_In_House = obj[10].ToString();
                        //objSpecimen.Stat = obj[11].ToString();
                        //objSpecimen.Fasting = obj[12].ToString();
                        //objOrdersSubmit.Created_Date_And_Time = Convert.ToDateTime(obj[13]);
                        //objOrder.Created_By = obj[14].ToString();
                        //objOrder.Created_Date_And_Time = Convert.ToDateTime(obj[15]);
                        //objOrder.Modified_By = obj[16].ToString();
                        //objOrder.Modified_Date_And_Time = Convert.ToDateTime(obj[17]);
                        //objOrder.Version = Convert.ToInt16(obj[18]);
                        //objOrder.Lab_Procedure_Description = obj[19].ToString();
                        //objOrdersSubmit.Order_Type = obj[20].ToString();
                        //objOrdersSubmit.Specimen_ID = Convert.ToUInt64(obj[21]);
                        //objOrdersSubmit.Facility_Name = obj[22].ToString();
                        //objOrdersSubmit.Height = obj[23].ToString();
                        //objOrdersSubmit.Weight = obj[24].ToString();
                        //objOrder.Order_Code_Type = obj[25].ToString();
                        //objOrdersSubmit.Bill_Type = obj[26].ToString();
                        //objOrdersSubmit.Is_ABN_Signed = obj[27].ToString();
                        //objSpecimen.Specimen_Type = obj[28].ToString();
                        //objSpecimen.Culture_Location = obj[29].ToString();
                        ////objOrder.Is_In_House = obj[30].ToString();
                        ////objOrder.Orders_Internal_Submit_ID = Convert.ToUInt64(obj[31]);
                        ////objOrder.TestDate_In_Months = Convert.ToInt16(obj[32]);
                        ////objOrder.TestDate_In_Weeks = Convert.ToInt16(obj[33]);
                        ////objOrder.TestDate_In_Days = Convert.ToInt16(obj[34]);
                        ////objOrder.Patient_Instructions = obj[35].ToString();
                        //objOrdersSubmit.Move_To_MA = obj[30].ToString();
                        //objOrdersSubmit.Temperature = obj[35].ToString();
                        //objOrder.Order_Submit_ID = Convert.ToUInt64(obj[36]);
                        //objOrdersSubmit.Version = Convert.ToInt32(obj[37]);
                        //ordLabObj.ObjOrder = objOrder;
                        //if (obj[31] != null)
                        //{
                        //    ordLabObj.LabName = obj[31].ToString();
                        //}
                        //if (obj[32] != null)
                        //{
                        //    ordLabObj.LabLocName = obj[32].ToString();
                        //}
                        //if (obj[33] != null)
                        //{
                        //    objOrder.Current_Process = obj[33].ToString();
                        //}
                        //objSpecimen.Specimen_Collection_Date_And_Time = Convert.ToDateTime(obj[34]);
                        ordLabObj.procedureCodeDesc = obj[6].ToString() + "-" + obj[19].ToString();

                        ICriteria critOrd = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Id", Order_id));
                        if (critOrd.List<Orders>().Count > 0)
                        {
                            ordLabObj.ObjOrder = critOrd.List<Orders>()[0];
                            AddedOderSubmitID.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                        }


                        ICriteria critOrdSub = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", ordLabObj.ObjOrder.Order_Submit_ID));
                        if (critOrdSub.List<OrdersSubmit>().Count > 0)
                        {
                            if (critOrdSub.List<OrdersSubmit>()[0].Order_Code_Type.Trim() != string.Empty)
                                ordLabObj.OrdersSubmit = critOrdSub.List<OrdersSubmit>()[0];
                            else
                                continue;
                        }

                        if (LoadQuestionSet)
                        {
                            if (OrderSubmitId == ordLabObj.ObjOrder.Id)
                            {
                                //ICriteria critAfp = iMySession.CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                //if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                                //{
                                //    ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                                //}
                                //ICriteria critbloodLead = iMySession.CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                //if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                                //{
                                //    ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                                //}
                                //ICriteria critCyto = iMySession.CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                //if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                                //{
                                //    ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                                //}
                                //ICriteria critAOE = iMySession.CreateCriteria(typeof(OrdersQuestionSetAOE)).Add(Expression.Eq("Orders_ID", ordLabObj.ObjOrder.Id));
                                //if (critAOE.List<OrdersQuestionSetAOE>().Count > 0)
                                //{
                                //    ordLabObj.OrderAOEList = critAOE.List<OrdersQuestionSetAOE>();
                                //}
                            }
                            ordLabObj.OrdersSubmit = null;
                            ordLabObj.ObjOrder = null;
                        }
                        else
                        {
                            //ICriteria critSpe = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Specimen_ID));
                            //if (critSpe.List<Specimen>().Count > 0)
                            //{
                            //    ordLabObj.objSpecimen = critSpe.List<Specimen>()[0];
                            //}
                            ICriteria critWfobj = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", ordLabObj.OrdersSubmit.Order_Type)).Add(Expression.Eq("Obj_System_Id", ordLabObj.OrdersSubmit.Id));
                            if (critWfobj.List<WFObject>().Count > 0)
                            {
                                ordLabObj.ObjOrder.Internal_Property_Current_Process = critWfobj.List<WFObject>()[0].Current_Process;
                                if (ordLabObj.ObjOrder.Internal_Property_Current_Process.StartsWith("DELETED_"))
                                    continue;
                            }
                            ICriteria critLab = iMySession.CreateCriteria(typeof(Lab)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_ID));
                            if (critLab.List<Lab>().Count > 0)
                            {
                                ordLabObj.LabName = critLab.List<Lab>()[0].Lab_Name;
                            }
                            ICriteria critLabLoc = iMySession.CreateCriteria(typeof(LabLocation)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_Location_ID));
                            if (critLabLoc.List<LabLocation>().Count > 0)
                            {
                                ordLabObj.LabLocName = critLabLoc.List<LabLocation>()[0].Location_Name;
                            }
                            ordLabObj.procedureCodeDesc = ordLabObj.ObjOrder.Lab_Procedure + "-" + ordLabObj.ObjOrder.Lab_Procedure_Description;
                            //ulSpecimenIDList.Add(ordLabObj.OrdersSubmit.Specimen_ID);
                            //ulList.Add(ordLabObj.ObjOrder.Id);
                            ulList.Add(ordLabObj.ObjOrder.Order_Submit_ID);

                            ICriteria critOrdersRequiredForms = iMySession.CreateCriteria(typeof(OrdersRequiredForms)).Add(Expression.Eq("Order_Id", ordLabObj.ObjOrder.Id));//Order_ID Change to Order_Id By ThiyagarajanM
                            if (critOrdersRequiredForms.List<OrdersRequiredForms>().Count > 0)
                            {
                                ordLabObj.ilstOrdersRequiredForms = critOrdersRequiredForms.List<OrdersRequiredForms>();
                            }


                        }



                        ordLabList.Add(ordLabObj);

                    }
                }
                AddedOderSubmitID = AddedOderSubmitID.Distinct().ToList<ulong>();
                ISQLQuery queryId = iMySession.CreateSQLQuery("SELECT OS.*,W.* FROM orders_submit OS , wf_object W  WHERE W.obj_system_id=OS.Order_Submit_Id and  OS.Encounter_ID=0 and W.Current_Process!=\"DELETED_ORDER\" And W.Obj_Type=\"DIAGNOSTIC ORDER\" AND OS.Is_Paper_Order='N' AND OS.Human_Id=" + HumanID.ToString() + " and OS.Physician_ID=" + PhysicianId.ToString() + " and OS.Order_Type='" + OrderType + "';").AddEntity("OS", typeof(OrdersSubmit)).AddEntity("W", typeof(WFObject));
                IList<object[]> tempObjectArray = queryId.List<object[]>();
                IList<OrdersSubmit> templist = new List<OrdersSubmit>();
                foreach (object[] obj in tempObjectArray)
                {
                    OrdersSubmit tempOrdersSubmit = new OrdersSubmit();
                    tempOrdersSubmit = (OrdersSubmit)obj[0];
                    tempOrdersSubmit.Culture_Location = ((WFObject)obj[1]).Current_Process;
                    templist.Add(tempOrdersSubmit);

                }
                objOrdersDTO.ilstOrdersSubmitForPartialOrders = templist;
                objOrdersDTO.ilstOrdersSubmitForPartialOrders = objOrdersDTO.ilstOrdersSubmitForPartialOrders.Where(a => !AddedOderSubmitID.Contains(a.Id)).ToList<OrdersSubmit>();

                objOrdersDTO.ilstOrderLabDetailsDTO = ordLabList;
                orderList.Clear();
                if (ulList.Count > 0 && !LoadQuestionSet)
                {
                    objOrdersDTO.OrderAssList = GetOrdersAssessmentDetails(ulList);
                    // objOrderDetDTO.OrderPblmList = GetOrdersProblemListDetails(ulList);
                }

                //ICriteria criteria = session.GetISession().CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteria.List<OrdersSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.OrderSubmitList = criteria.List<OrdersSubmit>();
                //}
                //ICriteria criteriaOrdAss = session.GetISession().CreateCriteria(typeof(OrdersAssessment)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteriaOrdAss.List<OrdersAssessment>().Count > 0)
                //{
                //    objOrderDetDTO.OrderAssList = criteriaOrdAss.List<OrdersAssessment>();
                //}

                //if (ulSpecimenIDList.Count > 0)
                //{
                //    ICriteria criteriaSpecimen = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.In("Id", ulSpecimenIDList.ToArray<ulong>()));
                //    if (criteriaSpecimen.List<Specimen>().Count > 0)
                //    {
                //        objOrderDetDTO.SpecimenList = criteriaSpecimen.List<Specimen>();
                //    }
                //}
                //ICriteria subCrit = session.GetISession().CreateCriteria(typeof(OrdersInternalSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Order_Type", "INTERNAL " + orderType));
                //if (subCrit.List<OrdersInternalSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.objInternalSubmit = subCrit.List<OrdersInternalSubmit>()[0];
                //}
                //else
                //{
                //    objOrderDetDTO.objInternalSubmit = null;
                //}
                //}


                # region In-house view

                //MOHAN
                IList<ulong> lstsubmit_id = objOrdersDTO.ilstOrderLabDetailsDTO.Where(q => q.OrdersSubmit != null).Select(A => A.OrdersSubmit.Id).Distinct().ToList<ulong>();
                for (int k = 0; k < lstsubmit_id.Count; k++)
                {
                    ICriteria critMgnt = iMySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Order_ID", lstsubmit_id[k]));
                    if (critMgnt.List<FileManagementIndex>().Count > 0)

                        foreach (var obj in objOrdersDTO.ilstOrderLabDetailsDTO.Where(Z => Z.OrdersSubmit.Id == lstsubmit_id[k]))
                        {
                            objOrdersDTO.ilstOrderLabDetailsDTO[objOrdersDTO.ilstOrderLabDetailsDTO.IndexOf(obj)].OrdersSubmit.Internal_Property_File_Path = "TRUE";
                        }
                }

                #endregion


                serializer.WriteObject(stream, objOrdersDTO);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;
        }

        public Stream GetOrdersUsingHumanIDByOrderSubmitID(ulong OrderSubmitId,ulong HumanID, DateTime createdDate, string OrderType, ulong PhysicianId, bool LoadQuestionSet)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objOrdersDTO = new OrdersDTO();

            //OrderDetailsDTO objOrderDetDTO = new OrderDetailsDTO();
            IList<OrderLabDetailsDTO> ordLabList = new List<OrderLabDetailsDTO>();
            OrderLabDetailsDTO ordLabObj = new OrderLabDetailsDTO();
            Orders objOrd = new Orders();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            OrdersAssessment objOrdAss = new OrdersAssessment();
            IList<ulong> ulList = new List<ulong>();
            IList<ulong> ulSpecimenIDList = new List<ulong>();
            IList<Orders> iOrderList = new List<Orders>();
            ArrayList orderList = null;
            string split = createdDate.ToString("yyyy-MM-dd");
            ulong Order_id = 0;
            ulong Order_submit_id = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("Fill.Orders.GetOrdersUsingHumanIDByOrderSubmitID");
                query2.SetString(0, OrderSubmitId.ToString());
                query2.SetString(1, HumanID.ToString());
                query2.SetString(2, "%%");
                query2.SetString(3, OrderType);
                query2.SetString(4, "0");
                query2.SetString(5, PhysicianId.ToString());
                orderList = new ArrayList(query2.List());

                IList<ulong> AddedOderSubmitID = new List<ulong>();


                //objOrderDetDTO.OrderCount = Convert.ToInt16(orderList.Count); //commented by prabu 23/01/2012
                if (orderList != null)
                {
                    foreach (object[] obj in orderList)
                    {
                        ordLabObj = new OrderLabDetailsDTO();
                        OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                        //Specimen objSpecimen = new Specimen();
                        Orders objOrder = new Orders();
                        Order_id = Convert.ToUInt64(obj[0]);
                        Order_submit_id = Convert.ToUInt64(obj[1]);


                        //objOrder.Id = Convert.ToUInt64(obj[0]);
                        //objOrder.Encounter_ID = Convert.ToUInt64(obj[1]);
                        //objOrder.Human_ID = Convert.ToUInt64(obj[2]);
                        //objOrder.Physician_ID = Convert.ToUInt64(obj[3]);
                        //objOrdersSubmit.Lab_Location_ID = Convert.ToUInt64(obj[4]);
                        //objOrdersSubmit.Lab_ID = Convert.ToUInt64(obj[5]);
                        //objOrder.Lab_Procedure = obj[6].ToString();
                        //objOrdersSubmit.Authorization_Required = obj[7].ToString();
                        //objOrdersSubmit.Test_Date = Convert.ToString(obj[8]);
                        //objOrdersSubmit.Order_Notes = obj[9].ToString();
                        //objOrdersSubmit.Specimen_In_House = obj[10].ToString();
                        //objSpecimen.Stat = obj[11].ToString();
                        //objSpecimen.Fasting = obj[12].ToString();
                        //objOrdersSubmit.Created_Date_And_Time = Convert.ToDateTime(obj[13]);
                        //objOrder.Created_By = obj[14].ToString();
                        //objOrder.Created_Date_And_Time = Convert.ToDateTime(obj[15]);
                        //objOrder.Modified_By = obj[16].ToString();
                        //objOrder.Modified_Date_And_Time = Convert.ToDateTime(obj[17]);
                        //objOrder.Version = Convert.ToInt16(obj[18]);
                        //objOrder.Lab_Procedure_Description = obj[19].ToString();
                        //objOrdersSubmit.Order_Type = obj[20].ToString();
                        //objOrdersSubmit.Specimen_ID = Convert.ToUInt64(obj[21]);
                        //objOrdersSubmit.Facility_Name = obj[22].ToString();
                        //objOrdersSubmit.Height = obj[23].ToString();
                        //objOrdersSubmit.Weight = obj[24].ToString();
                        //objOrder.Order_Code_Type = obj[25].ToString();
                        //objOrdersSubmit.Bill_Type = obj[26].ToString();
                        //objOrdersSubmit.Is_ABN_Signed = obj[27].ToString();
                        //objSpecimen.Specimen_Type = obj[28].ToString();
                        //objSpecimen.Culture_Location = obj[29].ToString();
                        ////objOrder.Is_In_House = obj[30].ToString();
                        ////objOrder.Orders_Internal_Submit_ID = Convert.ToUInt64(obj[31]);
                        ////objOrder.TestDate_In_Months = Convert.ToInt16(obj[32]);
                        ////objOrder.TestDate_In_Weeks = Convert.ToInt16(obj[33]);
                        ////objOrder.TestDate_In_Days = Convert.ToInt16(obj[34]);
                        ////objOrder.Patient_Instructions = obj[35].ToString();
                        //objOrdersSubmit.Move_To_MA = obj[30].ToString();
                        //objOrdersSubmit.Temperature = obj[35].ToString();
                        //objOrder.Order_Submit_ID = Convert.ToUInt64(obj[36]);
                        //objOrdersSubmit.Version = Convert.ToInt32(obj[37]);
                        //ordLabObj.ObjOrder = objOrder;
                        //if (obj[31] != null)
                        //{
                        //    ordLabObj.LabName = obj[31].ToString();
                        //}
                        //if (obj[32] != null)
                        //{
                        //    ordLabObj.LabLocName = obj[32].ToString();
                        //}
                        //if (obj[33] != null)
                        //{
                        //    objOrder.Current_Process = obj[33].ToString();
                        //}
                        //objSpecimen.Specimen_Collection_Date_And_Time = Convert.ToDateTime(obj[34]);
                        ordLabObj.procedureCodeDesc = obj[6].ToString() + "-" + obj[19].ToString();

                        ICriteria critOrd = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Id", Order_id));
                        if (critOrd.List<Orders>().Count > 0)
                        {
                            ordLabObj.ObjOrder = critOrd.List<Orders>()[0];
                            AddedOderSubmitID.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                        }


                        ICriteria critOrdSub = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", ordLabObj.ObjOrder.Order_Submit_ID));
                        if (critOrdSub.List<OrdersSubmit>().Count > 0)
                        {
                            if (critOrdSub.List<OrdersSubmit>()[0].Order_Code_Type.Trim() != string.Empty)
                                ordLabObj.OrdersSubmit = critOrdSub.List<OrdersSubmit>()[0];
                            else
                                continue;
                        }

                        if (LoadQuestionSet)
                        {
                            if (OrderSubmitId == ordLabObj.ObjOrder.Id)
                            {
                                //ICriteria critAfp = iMySession.CreateCriteria(typeof(OrdersQuestionSetAfp)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                //if (critAfp.List<OrdersQuestionSetAfp>().Count > 0)
                                //{
                                //    ordLabObj.objAFP = critAfp.List<OrdersQuestionSetAfp>()[0];
                                //}
                                //ICriteria critbloodLead = iMySession.CreateCriteria(typeof(OrdersQuestionSetBloodLead)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                //if (critbloodLead.List<OrdersQuestionSetBloodLead>().Count > 0)
                                //{
                                //    ordLabObj.objBloodLead = critbloodLead.List<OrdersQuestionSetBloodLead>()[0];
                                //}
                                //ICriteria critCyto = iMySession.CreateCriteria(typeof(OrdersQuestionSetCytology)).Add(Expression.Eq("Order_ID", ordLabObj.ObjOrder.Id));
                                //if (critCyto.List<OrdersQuestionSetCytology>().Count > 0)
                                //{
                                //    ordLabObj.objCytology = critCyto.List<OrdersQuestionSetCytology>()[0];
                                //}
                                //ICriteria critAOE = iMySession.CreateCriteria(typeof(OrdersQuestionSetAOE)).Add(Expression.Eq("Orders_ID", ordLabObj.ObjOrder.Id));
                                //if (critAOE.List<OrdersQuestionSetAOE>().Count > 0)
                                //{
                                //    ordLabObj.OrderAOEList = critAOE.List<OrdersQuestionSetAOE>();
                                //}
                            }
                            ordLabObj.OrdersSubmit = null;
                            ordLabObj.ObjOrder = null;
                        }
                        else
                        {
                            //ICriteria critSpe = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Specimen_ID));
                            //if (critSpe.List<Specimen>().Count > 0)
                            //{
                            //    ordLabObj.objSpecimen = critSpe.List<Specimen>()[0];
                            //}
                            ICriteria critWfobj = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", ordLabObj.OrdersSubmit.Order_Type)).Add(Expression.Eq("Obj_System_Id", ordLabObj.OrdersSubmit.Id));
                            if (critWfobj.List<WFObject>().Count > 0)
                            {
                                ordLabObj.ObjOrder.Internal_Property_Current_Process = critWfobj.List<WFObject>()[0].Current_Process;
                                if (ordLabObj.ObjOrder.Internal_Property_Current_Process.StartsWith("DELETED_"))
                                    continue;
                            }
                            ICriteria critLab = iMySession.CreateCriteria(typeof(Lab)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_ID));
                            if (critLab.List<Lab>().Count > 0)
                            {
                                ordLabObj.LabName = critLab.List<Lab>()[0].Lab_Name;
                            }
                            ICriteria critLabLoc = iMySession.CreateCriteria(typeof(LabLocation)).Add(Expression.Eq("Id", ordLabObj.OrdersSubmit.Lab_Location_ID));
                            if (critLabLoc.List<LabLocation>().Count > 0)
                            {
                                ordLabObj.LabLocName = critLabLoc.List<LabLocation>()[0].Location_Name;
                            }
                            ordLabObj.procedureCodeDesc = ordLabObj.ObjOrder.Lab_Procedure + "-" + ordLabObj.ObjOrder.Lab_Procedure_Description;
                            //ulSpecimenIDList.Add(ordLabObj.OrdersSubmit.Specimen_ID);
                            //ulList.Add(ordLabObj.ObjOrder.Id);
                            ulList.Add(ordLabObj.ObjOrder.Order_Submit_ID);

                            ICriteria critOrdersRequiredForms = iMySession.CreateCriteria(typeof(OrdersRequiredForms)).Add(Expression.Eq("Order_Id", ordLabObj.ObjOrder.Id));//Order_ID Change to Order_Id By ThiyagarajanM
                            if (critOrdersRequiredForms.List<OrdersRequiredForms>().Count > 0)
                            {
                                ordLabObj.ilstOrdersRequiredForms = critOrdersRequiredForms.List<OrdersRequiredForms>();
                            }


                        }



                        ordLabList.Add(ordLabObj);

                    }
                }
                AddedOderSubmitID = AddedOderSubmitID.Distinct().ToList<ulong>();
                ISQLQuery queryId = iMySession.CreateSQLQuery("SELECT OS.*,W.* FROM orders_submit OS , wf_object W  WHERE W.obj_system_id=OS.Order_Submit_Id and  OS.Encounter_ID=0 and W.Current_Process!=\"DELETED_ORDER\" And W.Obj_Type=\"DIAGNOSTIC ORDER\" AND OS.Is_Paper_Order='N' AND OS.Order_Submit_Id="+ OrderSubmitId.ToString() + " AND OS.Human_Id=" + HumanID.ToString() + " and OS.Physician_ID=" + PhysicianId.ToString() + " and OS.Order_Type='" + OrderType + "';").AddEntity("OS", typeof(OrdersSubmit)).AddEntity("W", typeof(WFObject));
                IList<object[]> tempObjectArray = queryId.List<object[]>();
                IList<OrdersSubmit> templist = new List<OrdersSubmit>();
                foreach (object[] obj in tempObjectArray)
                {
                    OrdersSubmit tempOrdersSubmit = new OrdersSubmit();
                    tempOrdersSubmit = (OrdersSubmit)obj[0];
                    tempOrdersSubmit.Culture_Location = ((WFObject)obj[1]).Current_Process;
                    templist.Add(tempOrdersSubmit);

                }
                objOrdersDTO.ilstOrdersSubmitForPartialOrders = templist;
                objOrdersDTO.ilstOrdersSubmitForPartialOrders = objOrdersDTO.ilstOrdersSubmitForPartialOrders.Where(a => !AddedOderSubmitID.Contains(a.Id)).ToList<OrdersSubmit>();

                objOrdersDTO.ilstOrderLabDetailsDTO = ordLabList;
                orderList.Clear();
                if (ulList.Count > 0 && !LoadQuestionSet)
                {
                    objOrdersDTO.OrderAssList = GetOrdersAssessmentDetails(ulList);
                    // objOrderDetDTO.OrderPblmList = GetOrdersProblemListDetails(ulList);
                }

                //ICriteria criteria = session.GetISession().CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteria.List<OrdersSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.OrderSubmitList = criteria.List<OrdersSubmit>();
                //}
                //ICriteria criteriaOrdAss = session.GetISession().CreateCriteria(typeof(OrdersAssessment)).Add(Expression.Eq("Encounter_ID", EncounterID));
                //if (criteriaOrdAss.List<OrdersAssessment>().Count > 0)
                //{
                //    objOrderDetDTO.OrderAssList = criteriaOrdAss.List<OrdersAssessment>();
                //}

                //if (ulSpecimenIDList.Count > 0)
                //{
                //    ICriteria criteriaSpecimen = session.GetISession().CreateCriteria(typeof(Specimen)).Add(Expression.In("Id", ulSpecimenIDList.ToArray<ulong>()));
                //    if (criteriaSpecimen.List<Specimen>().Count > 0)
                //    {
                //        objOrderDetDTO.SpecimenList = criteriaSpecimen.List<Specimen>();
                //    }
                //}
                //ICriteria subCrit = session.GetISession().CreateCriteria(typeof(OrdersInternalSubmit)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Order_Type", "INTERNAL " + orderType));
                //if (subCrit.List<OrdersInternalSubmit>().Count > 0)
                //{
                //    objOrderDetDTO.objInternalSubmit = subCrit.List<OrdersInternalSubmit>()[0];
                //}
                //else
                //{
                //    objOrderDetDTO.objInternalSubmit = null;
                //}
                //}


                # region In-house view

                //MOHAN
                IList<ulong> lstsubmit_id = objOrdersDTO.ilstOrderLabDetailsDTO.Where(q => q.OrdersSubmit != null).Select(A => A.OrdersSubmit.Id).Distinct().ToList<ulong>();
                for (int k = 0; k < lstsubmit_id.Count; k++)
                {
                    ICriteria critMgnt = iMySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Order_ID", lstsubmit_id[k]));
                    if (critMgnt.List<FileManagementIndex>().Count > 0)

                        foreach (var obj in objOrdersDTO.ilstOrderLabDetailsDTO.Where(Z => Z.OrdersSubmit.Id == lstsubmit_id[k]))
                        {
                            objOrdersDTO.ilstOrderLabDetailsDTO[objOrdersDTO.ilstOrderLabDetailsDTO.IndexOf(obj)].OrdersSubmit.Internal_Property_File_Path = "TRUE";
                        }
                }

                #endregion


                serializer.WriteObject(stream, objOrdersDTO);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;
        }
        #endregion
        public bool SubmitOrdersWithOutEncounter(ulong myHumanId, ulong myEncounterId, string objType, string MACAddress, string UserName, DateTime currentDate, string MedAsstUserName, IList<string> OrdersType, string sFacilityName)
        {
            bool SubmitedOrNot = false;
            WFObjectManager objWfObjectManager = new WFObjectManager();
            // WFObject objWf;
            EncounterManager EncounterMgr = new EncounterManager();

            //Encounter EncRecord = EncounterMgr.GetById(ulMyEncounterID);

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                #region LabAndImage
                if (objType == "LabAndImage")
                {
                    ICriteria criOrders = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Encounter_ID", myEncounterId)).Add(Expression.Eq("Human_ID", myHumanId));
                    IList<OrdersSubmit> objLabAndImageOrders = criOrders.List<OrdersSubmit>();
                    foreach (OrdersSubmit Order in objLabAndImageOrders)
                    {
                        ICriteria crit = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", Order.Id))
                        .Add(Expression.Eq("Obj_Type", Order.Order_Type));
                        if (crit.List<WFObject>().Count == 0)
                        {
                            SubmitedOrNot = true;
                            SubmitOrdersToLabWithoutEncounter(myHumanId, 0, Order.Order_Type, MACAddress, UserName, currentDate, MedAsstUserName);
                        }

                    }
                }
                #endregion
                if (MedAsstUserName == string.Empty)
                    MedAsstUserName = "UNKNOWN";
                #region Immunization and Inhouse
                if (objType == "Immunization")
                {
                    ImmunizationManager objImmunMngr = new ImmunizationManager();
                    ImmunizationDTO immunDTO = objImmunMngr.FillImmunization(myHumanId, myEncounterId);
                    IList<Immunization> immunSubmitList = new List<Immunization>();
                    if (immunDTO != null && immunDTO.Immunization.Count > 0)
                    {
                        foreach (Immunization obj in immunDTO.Immunization)
                        {
                            if (obj.Immunization_Group_ID == 0 && obj.Vaccine_In_House == "Y")
                            {
                                obj.Immunization_Group_ID = immunDTO.MaxGroupId + 1;
                                obj.Modified_Date_And_Time = currentDate;
                                immunSubmitList.Add(obj);
                            }
                        }
                        if (immunSubmitList.Count > 0)
                        {
                            ulong immunsubmitId = (ulong)objImmunMngr.SubmitImmunization(immunSubmitList, MACAddress);
                            if (immunsubmitId != 0)
                            {
                                WFObject TempWF = new WFObject();
                                TempWF = objWfObjectManager.GetByObjectSystemId(immunsubmitId, "IMMUNIZATION ORDER");
                                // objWfObjectManager.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 2, MedAsstUserName, currentDate, MACAddress, null, null);//For bug ID 48532
                                objWfObjectManager.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 5, MedAsstUserName, currentDate, MACAddress, null, null);
                                SubmitedOrNot = true;
                            }
                        }
                    }
                }


                #endregion
                #region InHouseSubmit
                if (objType == "InHouseProcedure")
                {
                    ulong MaxGroupIdImm;
                    InHouseProcedureManager objInHouseMngr = new InHouseProcedureManager();
                    ICriteria critGrpID = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", "INTERNAL ORDER"))
                        .SetProjection(Projections.Max("Obj_System_Id"));
                    MaxGroupIdImm = critGrpID.List<ulong>()[0];
                    ICriteria critInHouse = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Encounter_ID", myEncounterId)).Add(Expression.Eq("Human_ID", myHumanId));
                    IList<InHouseProcedure> ilstInHouse = critInHouse.List<InHouseProcedure>();
                    IList<InHouseProcedure> otherProSubmitList = new List<InHouseProcedure>();
                    if (ilstInHouse != null && ilstInHouse.Count > 0)
                    {
                        foreach (InHouseProcedure obj in ilstInHouse)
                        {
                            if (obj.In_House_Procedure_Group_ID == 0)
                            {
                                obj.In_House_Procedure_Group_ID = MaxGroupIdImm + 1;
                                obj.Modified_Date_And_Time = currentDate;
                                otherProSubmitList.Add(obj);
                            }
                        }
                        if (otherProSubmitList.Count > 0)
                        {
                            ulong GroupId = (ulong)objInHouseMngr.SubmitInHouseProcedures(otherProSubmitList, MACAddress);
                            if (GroupId != 0)
                            {
                                WFObject TempWF = new WFObject();
                                TempWF = objWfObjectManager.GetByObjectSystemId(GroupId, "INTERNAL ORDER");
                                objWfObjectManager.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 2, MedAsstUserName, currentDate, MACAddress, null, null);
                                SubmitedOrNot = true;
                            }
                        }
                    }
                }
                #endregion
                #region ReferralOrder
                if (objType == "ReferralOrder")
                {
                    ulong MaxGroupIdRef;
                    ReferralOrderManager objReferralOrderMngr = new ReferralOrderManager();
                    ICriteria critRefGrpID = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", "REFERRAL ORDER"))
                        .SetProjection(Projections.Max("Obj_System_Id"));
                    MaxGroupIdRef = critRefGrpID.List<ulong>()[0];
                    ICriteria critReferral = iMySession.CreateCriteria(typeof(ReferralOrder)).Add(Expression.Eq("Encounter_ID", myEncounterId)).Add(Expression.Eq("Human_ID", myHumanId));
                    IList<ReferralOrder> ilstReferral = critReferral.List<ReferralOrder>();
                    IList<ReferralOrder> ReferralList = new List<ReferralOrder>();
                    if (ilstReferral != null && ilstReferral.Count > 0)
                    {
                        foreach (ReferralOrder obj in ilstReferral)
                        {
                            if (obj.Referral_Order_Group_ID == 0)
                            {
                                obj.Referral_Order_Group_ID = MaxGroupIdRef + 1;
                                obj.Modified_Date_And_Time = currentDate;
                                ReferralList.Add(obj);
                            }
                        }
                        if (ReferralList.Count > 0)
                        {
                            //ulong GroupId = (ulong)objReferralOrderMngr.SubmitReferralOrder(ReferralList,sFacilityName, MACAddress);
                            ulong GroupId = (ulong)objReferralOrderMngr.SubmitReferralOrder(ReferralList, sFacilityName, MedAsstUserName, MACAddress);
                            if (GroupId != 0)
                            {
                                WFObject TempWF = new WFObject();
                                TempWF = objWfObjectManager.GetByObjectSystemId(GroupId, "REFERRAL ORDER");
                                if (TempWF.Current_Process != "BILLING_WAIT" && TempWF.Current_Process != "MA_REVIEW")
                                {
                                    objWfObjectManager.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 1, MedAsstUserName, currentDate, MACAddress, null, null);
                                }
                                SubmitedOrNot = true;
                            }
                        }
                    }
                }
                #endregion
                iMySession.Close();
            }
            return SubmitedOrNot;
        }

        public IList<Orders> GetOrdersBySubmitID(IList<ulong> order_Submit_Id_List)
        {
            IList<Orders> ordersList = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                for (int i = 0; i < order_Submit_Id_List.Count; i++)
                {
                    ICriteria crit = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Order_Submit_ID", order_Submit_Id_List[i]));
                    IList<Orders> o = crit.List<Orders>();
                    for (int j = 0; j < o.Count; j++)
                    {
                        //added by vince for scanning  dec-20-2012///
                        ICriteria criteria = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", order_Submit_Id_List[i]));
                        IList<OrdersSubmit> lstordersubmit = criteria.List<OrdersSubmit>();
                        if (lstordersubmit.Count > 0)
                        {
                            o[j].Internal_Property_Spec_Collection_Date = lstordersubmit[0].Specimen_Collection_Date_And_Time;
                            o[j].Internal_Property_Lab_Name = lstordersubmit[0].Lab_Name;
                        }
                        //added by vince for scanning dec-20-2012///
                        ordersList.Add(o[j]);



                    }
                }

                iMySession.Close();
            }
            return ordersList;
        }

        public IList<FillOrdersManagementDTO> SearchOrder(string order_type, string order_status, string FacilityName, int orderInDays, DateTime fromDate, DateTime toDate, ulong humanID, ulong physicianID, ulong labId, ulong labLocationId, int pageNumber, int maxResults)
        {

            //ulong[] orderSubmitIdUlong = null;
            IList<Orders> ordersList = new List<Orders>();
            IList<Lab> labList = new List<Lab>();
            IList<LabLocation> labLocationList = new List<LabLocation>();
            IList<OrdersAssessment> ordersAssessmentList = new List<OrdersAssessment>();
            IList<ulong> ordersIDList = new List<ulong>();
            IList<WFObject> wfObjectList = new List<WFObject>();
            ArrayList arr = null;
            // ArrayList arr1 = null;
            string human = string.Empty;
            string physician = string.Empty;
            string orderType = string.Empty;
            IList<FillOrdersManagementDTO> ilstFillOrdersManagementDTO = new List<FillOrdersManagementDTO>();
            FillOrdersManagementDTO ObjFillOrderManagementDTO = new FillOrdersManagementDTO();
            LabManager labManager = new LabManager();
            LabLocationManager labLocationManager = new LabLocationManager();
            OrdersAssessmentManager ordersAssessmentManager = new OrdersAssessmentManager();

            string lab = string.Empty;
            string labLocation = string.Empty;
            if (labId == 0)
            {
                lab = "%";
            }
            else
            {
                lab = labId.ToString();
            }
            if (labLocationId == 0)
            {
                labLocation = "%";
            }
            else
            {
                labLocation = labLocationId.ToString();
            }
            if (humanID == 0)
            {
                human = "%";
            }
            else
            {
                human = humanID.ToString();
            }
            if (physicianID == 0)
            {
                physician = "%";
            }
            else
            {
                physician = physicianID.ToString();
            }
            if (order_type.Trim() == string.Empty)
            {
                orderType = "%";
            }
            else
            {
                //Latha - Branch_capella_3_0_50_Production_2011_06_05 - Start
                if (order_type.ToUpper().Contains("INTERNAL") == true)
                {
                    orderType = order_type.Replace("INTERNAL ", string.Empty);
                }
                else
                {
                    orderType = order_type;
                }
                //Latha - Branch_capella_3_0_50_Production_2011_06_05 - End
            }
            if (order_status.Trim() == string.Empty)
            {
                order_status = "%";
            }
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (order_type.Contains("INTERNAL") == false)
                {
                    //string sFacilityCmg = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].Trim().ToUpper();
                    //if (FacilityName == sFacilityCmg)
                    //var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacilityName select f;
                    //IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    //if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary=="Y")
                    //{
                    //    IQuery query = iMySession.GetNamedQuery("Get.GetLabImageOrderSearch.CMGAncillary");
                    //    query.SetString(0, orderType);
                    //    query.SetString(1, FacilityName + "%");
                    //    query.SetString(2, order_status);
                    //    query.SetString(3, fromDate.ToString("yyyy-MM-dd"));
                    //    query.SetString(4, toDate.ToString("yyyy-MM-dd"));
                    //    query.SetString(5, lab);
                    //    query.SetString(6, human);
                    //    query.SetString(7, order_type);
                    //    query.SetString(8, orderType);
                    //    query.SetString(9, FacilityName + "%");
                    //    query.SetString(10, order_status);
                    //    query.SetString(11, fromDate.ToString("yyyy-MM-dd"));
                    //    query.SetString(12, toDate.ToString("yyyy-MM-dd"));
                    //    query.SetString(13, lab);
                    //    query.SetString(14, human);
                    //    query.SetString(15, order_type);
                    //    arr = new ArrayList(query.List());
                    //}
                    //else
                    //{
                    IQuery query = iMySession.GetNamedQuery("Get.GetLabImageOrderSearch");
                    query.SetString(0, orderType);
                    query.SetString(1, FacilityName + "%");
                    query.SetString(2, order_status);
                    query.SetString(3, fromDate.ToString("yyyy-MM-dd"));
                    query.SetString(4, toDate.ToString("yyyy-MM-dd"));
                    query.SetString(5, lab);
                    query.SetString(6, physician);
                    query.SetString(7, human);
                    query.SetString(8, order_type);
                    query.SetString(9, orderType);
                    query.SetString(10, FacilityName + "%");
                    query.SetString(11, order_status);
                    query.SetString(12, fromDate.ToString("yyyy-MM-dd"));
                    query.SetString(13, toDate.ToString("yyyy-MM-dd"));
                    query.SetString(14, lab);
                    query.SetString(15, physician);
                    query.SetString(16, human);
                    query.SetString(17, order_type);
                    arr = new ArrayList(query.List());
                    //}
                    //                    IQuery query = iMySession.GetNamedQuery("Get.GetLabImageOrderSearch");
                    //                     query.SetString(0, orderType);
                    //                     query.SetString(1, FacilityName + "%");
                    //                     query.SetString(2, order_status);
                    //                     query.SetString(3, fromDate.ToString("yyyy-MM-dd"));
                    //                     query.SetString(4, toDate.ToString("yyyy-MM-dd"));
                    //                     query.SetString(5, lab);
                    //                     query.SetString(6, physician);
                    //                     query.SetString(7, human);
                    //                     query.SetString(8, order_type);
                    //                     query.SetString(9, orderType);
                    //                     query.SetString(10, FacilityName + "%");
                    //                     query.SetString(11, order_status);
                    //                     query.SetString(12, fromDate.ToString("yyyy-MM-dd"));
                    //                     query.SetString(13, toDate.ToString("yyyy-MM-dd"));
                    //                     query.SetString(14, lab);
                    //                     query.SetString(15, human);
                    //                     query.SetString(16, order_type);
                    //                     query.SetString(17, orderType);
                    //                     query.SetString(18, FacilityName + "%");
                    //                     query.SetString(19, order_status);
                    //                     query.SetString(20, fromDate.ToString("yyyy-MM-dd"));
                    //                     query.SetString(21, toDate.ToString("yyyy-MM-dd"));
                    //                     query.SetString(22, lab);
                    //                     query.SetString(23, physician);
                    //                     query.SetString(24, human);
                    //                     query.SetString(25, order_type);
                    //                     query.SetString(26, orderType);
                    //                     query.SetString(27, FacilityName + "%");
                    //                     query.SetString(28, order_status);
                    //                     query.SetString(29, fromDate.ToString("yyyy-MM-dd"));
                    //                     query.SetString(30, toDate.ToString("yyyy-MM-dd"));
                    //                     query.SetString(31, lab);
                    //                     query.SetString(32, human);
                    //                     query.SetString(33, order_type);
                    //                     arr = new ArrayList(query.List());
                }
                else if (order_type.Contains("INTERNAL") == true)
                {
                    IQuery query = iMySession.GetNamedQuery("Get.GetInternalLabImageOrderSearch");
                    query.SetString(0, order_type);
                    query.SetString(1, FacilityName + "%");
                    query.SetString(2, order_status);
                    query.SetString(3, fromDate.ToString("yyyy-MM-dd"));
                    query.SetString(4, toDate.ToString("yyyy-MM-dd"));
                    query.SetString(5, lab);
                    query.SetString(6, labLocation);
                    query.SetString(7, physician);
                    query.SetString(8, human);
                    query.SetString(9, orderType);
                    query.SetInt32(10, ((pageNumber - 1) * maxResults));
                    query.SetInt32(11, maxResults);
                    arr = new ArrayList(query.List());
                }

                if (arr != null)
                {
                    IList<ulong> OrderIds = new List<ulong>();
                    IList<FillOrdersManagementDTO> tempilstFillOrdersManagementDTO = new List<FillOrdersManagementDTO>();
                    ulong totalRecordsFound = Convert.ToUInt32(arr.Count);
                    IList<OrderLabDetailsDTO> ilsOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();
                    IList<ulong> ulGroupIdList = new List<ulong>();

                    //for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > arr.Count ? arr.Count : (maxResults * pageNumber)); i++)
                    for (int i = 0; i < arr.Count; i++)
                    {
                        ObjFillOrderManagementDTO = new FillOrdersManagementDTO();
                        object[] obj = (object[])arr[i];
                        ObjFillOrderManagementDTO.Ordered_Date_And_Time = Convert.ToDateTime(obj[0]);
                        ObjFillOrderManagementDTO.Order_Type = obj[1].ToString();
                        ObjFillOrderManagementDTO.Current_Process = obj[2].ToString();
                        if (obj[3] != null)
                        {
                            ObjFillOrderManagementDTO.Human_Id = obj[3].ToString();
                            ObjFillOrderManagementDTO.Human_Name = obj[4].ToString();
                        }
                        ObjFillOrderManagementDTO.Date_Of_Birth = Convert.ToDateTime(obj[5]);
                        if (obj[6] != null)
                            ObjFillOrderManagementDTO.Procedures = obj[6].ToString();
                        ObjFillOrderManagementDTO.Physician_ID = Convert.ToUInt32(obj[8]);
                        ObjFillOrderManagementDTO.Facility_Name = obj[9].ToString();
                        ObjFillOrderManagementDTO.Lab_ID = Convert.ToUInt32(obj[10]);
                        ObjFillOrderManagementDTO.Lab_Location_ID = Convert.ToUInt32(obj[11]);
                        ObjFillOrderManagementDTO.Total_Record_Found = totalRecordsFound;
                        ObjFillOrderManagementDTO.Group_ID = Convert.ToUInt32(obj[13]);
                        ObjFillOrderManagementDTO.Encounter_ID = obj[15].ToString();
                        ObjFillOrderManagementDTO.Specimen_Collected_Date_Time = obj[16].ToString();
                        OrderIds.Add(Convert.ToUInt32(obj[13]));
                        //Added by Manimozhi - bug id -15312
                        ObjFillOrderManagementDTO.File_Management_Index_Order_ID = Convert.ToUInt64(obj[17]);
                        //Added By Saravanakumar
                        if (obj[18] != null)
                            ObjFillOrderManagementDTO.Ordering_Provider = obj[18].ToString();
                        if (obj[19] != null)
                            ObjFillOrderManagementDTO.Lab_Name = obj[19].ToString();

                        ulGroupIdList.Add(ObjFillOrderManagementDTO.Group_ID);
                        //ICriteria cri = iMySession.CreateCriteria(typeof(ResultMaster)).Add(Expression.In.Eq("Order_ID", ObjFillOrderManagementDTO.Group_ID));
                        //IList<ResultMaster> tempResultMaster = cri.List<ResultMaster>();
                        //if (tempResultMaster != null && tempResultMaster.Count > 0)
                        //    ObjFillOrderManagementDTO.Is_Electronic_Result_Available = true;
                        tempilstFillOrdersManagementDTO.Add(ObjFillOrderManagementDTO);
                    }


                    IList<ResultMaster> tempResultMaster = new List<ResultMaster>();
                    if (ulGroupIdList.Count > 0)
                    {
                        ICriteria cri = iMySession.CreateCriteria(typeof(ResultMaster)).Add(Expression.In("Order_ID", ulGroupIdList.ToArray<ulong>()));
                        tempResultMaster = cri.List<ResultMaster>();
                    }
                    if (tempResultMaster != null && tempResultMaster.Count > 0)
                    {
                        for (int g = 0; g < tempilstFillOrdersManagementDTO.Count; g++)
                        {
                            IList<ResultMaster> ResultMasterList = tempResultMaster.Where(a => a.Order_ID == tempilstFillOrdersManagementDTO[g].Group_ID).ToList<ResultMaster>();
                            if (ResultMasterList != null && ResultMasterList.Count > 0)
                                tempilstFillOrdersManagementDTO[g].Is_Electronic_Result_Available = true;
                        }
                    }
                    //if (tempResultMaster != null && tempResultMaster.Count > 0)
                    //    ObjFillOrderManagementDTO.Is_Electronic_Result_Available = true;
                    ilstFillOrdersManagementDTO = tempilstFillOrdersManagementDTO.OrderByDescending(a => a.Ordered_Date_And_Time).ToList<FillOrdersManagementDTO>();
                    string[] split = { " <split> " };
                    if (OrderIds.Count != 0)
                    {
                        IQuery query2 = iMySession.GetNamedQuery("Get.AssessmentAssociatedWithOrders");
                        query2.SetParameterList("OrderIDs", OrderIds.ToArray<ulong>());
                        ArrayList tmp = new ArrayList(query2.List());
                        foreach (object[] obj in tmp)
                        {
                            var Update = ilstFillOrdersManagementDTO.Where(a => a.Group_ID == Convert.ToUInt64(obj[0]));
                            foreach (var tempobj in Update)
                            {
                                List<string> AssList = obj[1].ToString().Split(split, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                                foreach (string s in AssList.Distinct())
                                    tempobj.Assessment += s + ";";
                            }
                        }
                    }

                }


                ////if (order_status.Trim() != string.Empty)
                ////{
                ////    IList<WFObject> wf_objList = wfObjMgr.GetListofObjectsByCurrentProcess(order_status.ToUpper());
                ////    if (wf_objList.Count > 0)
                ////    {
                ////        orderSubmitIdUlong = new ulong[wf_objList.Count];
                ////        for (int i = 0; i < wf_objList.Count; i++)
                ////        {
                ////            orderSubmitIdUlong.SetValue(wf_objList[i].Obj_System_Id, i);
                ////        }

                ////        IQuery query = session.GetISession().GetNamedQuery("Fill.SearchOrderWithCurrentStatus");
                ////        query.SetString(0, fromDate.ToString("yyyy-MM-dd"));
                ////        query.SetString(1, toDate.ToString("yyyy-MM-dd"));
                ////        query.SetString(2, lab);
                ////        query.SetString(3, labLocation);
                ////        query.SetString(4, physician);
                ////        query.SetString(5, human);
                ////        query.SetString(6, orderType);
                ////        query.SetParameterList("OrderSubmitList", orderSubmitIdUlong);
                ////        arr = new ArrayList(query.List());
                ////    }
                ////}
                ////else
                ////{
                ////    IQuery query = session.GetISession().GetNamedQuery("Fill.SearchOrderWithOutCurrentStatus");
                ////    query.SetString(0, fromDate.ToString("yyyy-MM-dd"));
                ////    query.SetString(1, toDate.ToString("yyyy-MM-dd"));
                ////    query.SetString(2, lab);
                ////    query.SetString(3, labLocation);
                ////    query.SetString(4, physician);
                ////    query.SetString(5, human);
                ////    query.SetString(6, orderType);
                ////    arr = new ArrayList(query.List());
                ////}
                ////if (arr != null)
                ////{
                ////    foreach (object[] obj in arr)
                ////    {
                ////        Orders objOrder = new Orders();
                ////        objOrder.Id = Convert.ToUInt64(obj[0]);
                ////        objOrder.Encounter_ID = Convert.ToUInt64(obj[1]);
                ////        objOrder.Human_ID = Convert.ToUInt64(obj[2]);
                ////        objOrder.Physician_ID = Convert.ToUInt64(obj[3]);
                ////        objOrder.Lab_Location_ID = Convert.ToUInt64(obj[4]);
                ////        objOrder.Lab_ID = Convert.ToUInt64(obj[5]);
                ////        objOrder.Lab_Procedure = obj[6].ToString();
                ////        objOrder.Lab_Procedure_Description = obj[7].ToString();
                ////        objOrder.Authorization_Required = obj[8].ToString();
                ////        objOrder.Test_Date = Convert.ToDateTime(obj[9]);
                ////        objOrder.Order_Notes = obj[10].ToString();
                ////        objOrder.Specimen_In_House = obj[11].ToString();
                ////        objOrder.Stat = obj[12].ToString();
                ////        objOrder.Fasting = obj[13].ToString();
                ////        objOrder.Order_Date = Convert.ToDateTime(obj[14]);
                ////        objOrder.Created_By = obj[15].ToString();
                ////        objOrder.Created_Date_And_Time = Convert.ToDateTime(obj[16]);
                ////        objOrder.Modified_By = obj[17].ToString();
                ////        objOrder.Modified_Date_And_Time = Convert.ToDateTime(obj[18]);
                ////        objOrder.Version = Convert.ToInt16(obj[19]);
                ////        objOrder.Order_Type = obj[20].ToString();
                ////        objOrder.Specimen_ID = Convert.ToUInt64(obj[21]);
                ////        objOrder.Facility_Name = obj[22].ToString();
                ////        objOrder.Bill_Type = obj[23].ToString();
                ////        objOrder.Is_ABN_Signed = obj[24].ToString();
                ////        objOrder.Order_Code_Type = obj[25].ToString();
                ////        objOrder.Specimen = obj[26].ToString();
                ////        objOrder.Culture_Location = obj[27].ToString();
                ////        objOrder.Order_Submit_ID = Convert.ToUInt64(obj[28]);
                ////        objOrder.Is_In_House = obj[29].ToString();
                ////        objOrder.Height = obj[30].ToString();
                ////        objOrder.Weight = obj[31].ToString();
                ////        ordersList.Add(objOrder);
                ////        Lab l = labManager.GetById(objOrder.Lab_ID);
                ////        labList.Add(l);
                ////        IList<LabLocation> llist = labLocationManager.GetLabLocationUsinglabID(objOrder.Lab_ID);
                ////        for (int j = 0; j < llist.Count; j++)
                ////        {
                ////            if (labLocationList.Contains(llist[j]) == false)
                ////            {
                ////                labLocationList.Add(llist[j]);
                ////            }
                ////        }
                ////        ordersIDList.Add(objOrder.Id);
                ////        //Latha - Branch_capella_3_0_50_Production_2011_06_05 - Start
                ////        if (order_type.ToUpper() == "DIAGNOSTIC ORDER" || order_type.ToUpper() == "IMAGE ORDER" || order_type.ToUpper() == "INTERNAL DIAGNOSTIC ORDER" || order_type.ToUpper() == "INTERNAL IMAGE ORDER")
                ////        {
                ////            WFObject wfObj = wfObjMgr.GetByObjectSystemId(objOrder.Order_Submit_ID, order_type);
                ////            wfObjectList.Add(wfObj);
                ////        }
                ////        //Latha - Branch_capella_3_0_50_Production_2011_06_05 - End
                ////    }
                ////    ObjFillOrderManagement.Lab_List = labList;
                ////    ObjFillOrderManagement.Lab_Location_List = labLocationList;
                ////    ObjFillOrderManagement.Orders_List = ordersList;
                ////    ObjFillOrderManagement.Orders_Assessment_List = GetOrdersAssessmentDetails(ordersIDList);
                ////    ObjFillOrderManagement.Wf_Object_List = wfObjectList;
                ////}
                iMySession.Close();
            }
            return ilstFillOrdersManagementDTO;
        }

        public void SubmitLabImageOrders(ulong ulMyEncounterID, ulong ulMyHumanID, ulong ulMyPhysicianID, string UserName, string MACAddress, DateTime currentDateTime, string orderType, string wfObjType, string medAsstName)
        {
            //WFObjectManager objWfMnger = new WFObjectManager();
            //OrderDetailsDTO orderDtO = GetOrdersUsingEncounterID(ulMyEncounterID, orderType);
            //OrdersInternalSubmit submitObject;
            //IList<Orders> ordersSubmitList = new List<Orders>();
            //bool alreadySubmitted = false;
            //if (orderDtO.OrderLabList.Count > 0)
            //{
            //    foreach (OrderLabDetailsDTO obj in orderDtO.OrderLabList)
            //    {
            //        if (obj.ObjOrder.Orders_Internal_Submit_ID == 0)
            //            ordersSubmitList.Add(obj.ObjOrder);
            //    }
            //    if (ordersSubmitList.Count > 0)
            //    {
            //        if (orderDtO.objInternalSubmit == null)
            //        {
            //            submitObject = new OrdersInternalSubmit();
            //            submitObject.Encounter_ID = ulMyEncounterID;
            //            submitObject.Human_ID = ulMyHumanID;
            //            submitObject.Physician_ID = ulMyPhysicianID;
            //            submitObject.Order_Type = wfObjType;
            //            submitObject.Created_By = UserName;
            //            submitObject.Created_Date_And_Time = currentDateTime;
            //            submitObject.Modified_By = UserName;
            //            submitObject.Modified_Date_And_Time = currentDateTime;
            //        }
            //        else
            //        {
            //            submitObject = orderDtO.objInternalSubmit;
            //            alreadySubmitted = true;
            //        }
            //        ulong submitId = (ulong)objSubmitMngr.SubmitOrdersForInternalProcessing(submitObject, ordersSubmitList, MACAddress);
            //        if (submitId != 0 && alreadySubmitted == false)
            //        {
            //            WFObject TempWF = new WFObject();
            //            TempWF = objWfMnger.GetByObjectSystemId(submitId, wfObjType);
            //            objWfMnger.MoveToNextProcess(TempWF, 2, medAsstName, currentDateTime, MACAddress, null);
            //        }
            //    }
            //}
        }
        public void SubmitOrdersToLabWithoutEncounter(ulong myHumanId, ulong myEncounterId, string objType, string MACAddress, string UserName, DateTime currentDate, string MedAsstUserName)
        {
            IList<OrdersSubmit> ordMasterList = new List<OrdersSubmit>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT os.* FROM orders_submit os LEFT JOIN wf_object w ON (os.order_submit_id=w.Obj_System_ID AND os.Order_Type=w.Obj_Type) WHERE os.Encounter_ID=" + myEncounterId + " AND os.Human_Id=" + myHumanId + " AND os.Order_Type='" + objType + "' and w.Obj_System_ID IS NULL").AddEntity(typeof(OrdersSubmit));
                //SELECT os.* FROM orders o LEFT JOIN wf_object w ON (o.Order_ID=w.Obj_System_ID AND o.Order_Type=w.Obj_Type) LEFT JOIN OrdersSubmit OS ON OS.Order_Submit_ID=O.Order_Submit_ID 
                ordMasterList = sq.List<OrdersSubmit>();

                foreach (OrdersSubmit odr in ordMasterList)
                {
                    WFObject WFObj = new WFObject();
                    WFObj.Obj_Type = objType;
                    WFObj.Current_Arrival_Time = currentDate;
                    WFObj.Fac_Name = odr.Facility_Name;
                    WFObj.Parent_Obj_Type = string.Empty;
                    WFObj.Obj_System_Id = odr.Id;
                    WFObj.Current_Process = "START";
                    int iCloseType = 1;
                    if (odr.Move_To_MA == "Y")
                    {
                        WFObj.Current_Owner = MedAsstUserName;
                        iCloseType = 1;
                    }
                    else if (odr.Move_To_MA == "N")
                    {
                        WFObj.Current_Owner = "UNKNOWN";
                        iCloseType = 2;
                    }
                    WFObjectManager objWfMnger = new WFObjectManager();
                    objWfMnger.InsertToRCopiaWorkFlowObject(WFObj, iCloseType, MACAddress);
                }
                iMySession.Close();
            }
        }
        public void SubmitOrdersToLab(ulong myHumanId, ulong myEncounterId, string objType, string MACAddress, string UserName, DateTime currentDate, string MedAsstUserName)
        {
            IList<OrdersSubmit> ordMasterList = new List<OrdersSubmit>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT os.* FROM orders_submit os LEFT JOIN wf_object w ON (os.order_submit_id=w.Obj_System_ID AND os.Order_Type=w.Obj_Type) WHERE os.Encounter_ID=" + myEncounterId + " AND os.Human_Id=" + myHumanId + " AND os.Order_Type='" + objType + "' and w.Obj_System_ID IS NULL").AddEntity(typeof(Orders));
                ordMasterList = sq.List<OrdersSubmit>();

                foreach (OrdersSubmit odr in ordMasterList)
                {
                    WFObject WFObj = new WFObject();
                    WFObj.Obj_Type = objType;
                    WFObj.Current_Arrival_Time = currentDate;

                    WFObj.Fac_Name = odr.Facility_Name;
                    WFObj.Parent_Obj_Type = string.Empty;
                    WFObj.Obj_System_Id = odr.Id;
                    WFObj.Current_Process = "START";
                    int iCloseType = 1;
                    if (odr.Move_To_MA == "Y")
                    {
                        WFObj.Current_Owner = MedAsstUserName;
                        iCloseType = 1;
                    }
                    else if (odr.Move_To_MA == "N")
                    {
                        WFObj.Current_Owner = "UNKNOWN";
                        iCloseType = 2;
                    }
                    WFObjectManager objWfMnger = new WFObjectManager();
                    objWfMnger.InsertToRCopiaWorkFlowObject(WFObj, iCloseType, MACAddress);

                }
                iMySession.Close();
            }
        }

        #region Insert Dummy Order
        public ulong InsertDummyOrder(IList<OrdersSubmit> ordersubmitlist, IList<Orders> ordersList, string obj_Type, string facility_Name, string MACAddress)
        {
            GenerateXml XMLObj = new GenerateXml();
            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            ulong OrderId = 0;
            ulong orderSubmitId = 0;
            bool order = true;
            string user_name = string.Empty;
            ISession MySession = Session.GetISession();
            //ITransaction trans = null;
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            IList<OrdersSubmit> ordersubmitlistnull = null;
            //trans = MySession.BeginTransaction();

            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {////////////////////////////////////
                        if (ordersubmitlist != null)
                        {
                            if (ordersubmitlist.Count > 0)
                            {
                                //iResult = objOrdersSubmitManager.SaveUpdateDeleteWithoutTransaction(ref ordersubmitlist, null, null, MySession, MACAddress);
                                iResult = objOrdersSubmitManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordersubmitlist, ref ordersubmitlistnull, null, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
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
                                //foreach (Orders obj in ordersubmitlist)
                                //{
                                orderSubmitId = ordersubmitlist[0].Id;
                                //}

                                foreach (Orders obj in ordersList)
                                {
                                    obj.Order_Submit_ID = orderSubmitId;
                                }
                            }

                            /////////////////////////////////////

                            if (ordersList != null)
                            {
                                if (ordersList.Count > 0)
                                {
                                    IList<Orders> ordersListnull = null;
                                    //iResult = SaveUpdateDeleteWithoutTransaction(ref ordersList, null, null, MySession, MACAddress);
                                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordersList, ref ordersListnull, null, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
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

                                    foreach (Orders obj in ordersList)
                                    {
                                        if (order == true)
                                        {
                                            //////vince 06-12-11
                                            OrderId = obj.Id;
                                            //////vince 06-12-11
                                            WFObject WFObj = new WFObject();
                                            WFObj.Obj_Type = obj_Type.ToUpper();
                                            WFObj.Current_Arrival_Time = obj.Created_Date_And_Time;
                                            WFObj.Current_Owner = "UNKNOWN";
                                            // WFObj.Current_Owner = user_name;
                                            WFObj.Fac_Name = facility_Name;
                                            WFObj.Parent_Obj_Type = string.Empty;
                                            WFObj.Obj_System_Id = orderSubmitId;
                                            WFObj.Current_Process = "START";
                                            WFObjectManager objWfMnger = new WFObjectManager();
                                            iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 2, MACAddress, MySession);
                                            // iResult = objWfMnger.MoveToNextProcess(WFObj.Obj_System_Id, WFObj.Obj_Type, 2, "UNKNOWN", obj.Created_Date_And_Time, MACAddress, null, MySession);



                                            //iResult = objWfMnger.MoveToNextProcess(WFObj, 1, "UNKNOWN", obj.Created_Date_And_Time, MACAddress, MySession);
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
                                            order = false;
                                        }
                                    }
                                }
                            }

                            // MySession.Flush();
                            trans.Commit();
                        }
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
                //MySession.Close();
                throw new Exception(ex1.Message);
            }
            return orderSubmitId;

        }



        public IList<ResultEntryDTO> UpdateOrdersForResult(Orders UpdateOrder, string MacAddress)
        {
            IList<Orders> SaveLst = null;
            IList<Orders> UpdateLst = new List<Orders>();
            UpdateLst.Add(UpdateOrder);
            GenerateXml XMLObj = new GenerateXml();
            //SaveUpdateDeleteWithTransaction(ref SaveLst, UpdateLst, null, MacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveLst, ref UpdateLst, null, MacAddress, false, false, 0, string.Empty);
            ResultMasterManager objResultMasterMgr = new ResultMasterManager();
            return objResultMasterMgr.GetResultEntry(UpdateOrder.Human_ID, string.Empty);
        }

        #endregion

        #region IOrdersManager Members


        public IList<CheckoutOrdersDTO> GetOrdersByEncounterID(ulong EncounterId)
        {
            ArrayList orderList = new ArrayList();
            IList<CheckoutOrdersDTO> checkoutOrdersDtoObj = new List<CheckoutOrdersDTO>();
            CheckoutOrdersDTO objOrdDTO = new CheckoutOrdersDTO();
            //ICriteria crit = session.GetISession().CreateCriteria(typeof(Orders)).Add(Expression.Eq("Encounter_Id",EncounterId));
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.OrdersforCheckOut");
                query.SetString(0, EncounterId.ToString());
                orderList = new ArrayList(query.List());
                //IList<Orders> objorders = new List<Orders>();

                // objorders = crit.List<Orders>();

                if (orderList.Count > 0)
                {
                    foreach (object[] obj in orderList)
                    {
                        objOrdDTO = new CheckoutOrdersDTO();
                        objOrdDTO.Order_ID = Convert.ToUInt64(obj[0]);

                        //vince March-14-2013 for bugID 14971//
                        if (obj[1] != null)
                            objOrdDTO.Procedure_Code = obj[1].ToString();
                        else
                            objOrdDTO.Procedure_Code = string.Empty;
                        if (obj[2] != null)
                            objOrdDTO.Procedure_Code_Description = obj[2].ToString();
                        else
                            objOrdDTO.Procedure_Code_Description = string.Empty;
                        //vince March-14-2013 for bugID 14971//

                        objOrdDTO.Authorization_Required = obj[3].ToString();
                        objOrdDTO.Specimen_In_House = obj[4].ToString();
                        objOrdDTO.Order_Type = obj[5].ToString();
                        //objOrdDTO.specimen_ID = Convert.ToUInt64(obj[6]);
                        objOrdDTO.specimen = obj[7].ToString();
                        objOrdDTO.Modified_Date_And_Time = Convert.ToDateTime(obj[8]);
                        objOrdDTO.labName = obj[9].ToString();
                        objOrdDTO.labLocName = obj[10].ToString();

                        checkoutOrdersDtoObj.Add(objOrdDTO);
                    }

                }
                IList<Immunization> objImmunizationOrders = new List<Immunization>();

                ICriteria critimmu = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Encounter_Id", EncounterId));


                objImmunizationOrders = critimmu.List<Immunization>();

                if (objImmunizationOrders.Count > 0)
                {
                    foreach (Immunization obj in objImmunizationOrders)
                    {
                        objOrdDTO = new CheckoutOrdersDTO();
                        objOrdDTO.Order_ID = obj.Id;
                        objOrdDTO.Procedure_Code = obj.Procedure_Code;
                        objOrdDTO.Procedure_Code_Description = obj.Immunization_Description;
                        objOrdDTO.Authorization_Required = obj.Authorization_Required;
                        objOrdDTO.Order_Type = "IMMUNIZATION ORDER";

                        checkoutOrdersDtoObj.Add(objOrdDTO);
                    }
                }
                ICriteria critref = iMySession.CreateCriteria(typeof(ReferralOrder)).Add(Expression.Eq("Encounter_ID", EncounterId));

                IList<ReferralOrder> objReferralOrders = new List<ReferralOrder>();

                objReferralOrders = critref.List<ReferralOrder>();

                if (objReferralOrders.Count > 0)
                {
                    foreach (ReferralOrder obj in objReferralOrders)
                    {
                        objOrdDTO = new CheckoutOrdersDTO();
                        objOrdDTO.Order_ID = obj.Id;
                        objOrdDTO.To_Physician_Name = obj.To_Physician_Name;
                        objOrdDTO.Reason_For_Referral = obj.Reason_For_Referral;
                        objOrdDTO.Authorization_Required = obj.Authorization_Required;
                        objOrdDTO.To_Facility_Name = obj.To_Facility_Name;
                        objOrdDTO.Order_Type = "REFERRAL ORDER";

                        checkoutOrdersDtoObj.Add(objOrdDTO);
                    }
                }

                ICriteria critInhouse = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Encounter_ID", EncounterId));

                IList<InHouseProcedure> objInhouse = new List<InHouseProcedure>();

                objInhouse = critInhouse.List<InHouseProcedure>();

                if (objInhouse.Count > 0)
                {
                    foreach (InHouseProcedure obj in objInhouse)
                    {
                        objOrdDTO = new CheckoutOrdersDTO();
                        objOrdDTO.Order_ID = obj.Id;
                        //  objOrdDTO.To_Physician_Name = obj.phy;
                        objOrdDTO.Procedure_Code = obj.Procedure_Code;
                        objOrdDTO.Procedure_Code_Description = obj.Procedure_Code_Description;
                        objOrdDTO.Authorization_Required = obj.Authorization_Required;
                        objOrdDTO.To_Facility_Name = obj.Facility_Name;
                        objOrdDTO.Order_Type = "INTERNAL ORDER";

                        checkoutOrdersDtoObj.Add(objOrdDTO);
                    }
                }


                iMySession.Close();

            }
            return checkoutOrdersDtoObj;
        }

        #endregion

        #region IOrdersManager Members


        public CheckOutPrintOrdersDTO GetOrdersForCheckoutPrint(ulong EncounterID, ulong HumanId)
        {

            var serializer = new NetDataContractSerializer();

            CheckOutPrintOrdersDTO chkoutPrintDTO = new CheckOutPrintOrdersDTO();
            object objStream = (object)serializer.ReadObject(GetOrdersUsingEncounterID(EncounterID, "DIAGNOSTIC ORDER", false));
            chkoutPrintDTO.Order_Details_Dto_Lab_Order_List = (OrdersDTO)objStream;

            //object objStreamImage = (object)serializer.ReadObject(GetOrdersUsingEncounterID(EncounterID, "IMAGE ORDER", false));
            //chkoutPrintDTO.Order_Details_Dto_Image_Order_List = (OrdersDTO)objStreamImage;

            ImmunizationManager objImmunMngr = new ImmunizationManager();
            chkoutPrintDTO.ImmunizationDTOobj = objImmunMngr.FillImmunization(HumanId, EncounterID);

            ReferralOrderManager objrefMngr = new ReferralOrderManager();
            chkoutPrintDTO.Referral_Order_Dto = objrefMngr.GetReferralOrderUsingEncounterID(EncounterID);

            ulong Physician_ID = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria critEncounter = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EncounterID)).Add(Expression.Eq("Human_ID", HumanId));
                if (critEncounter.List<Encounter>().Count > 0)
                {
                    Physician_ID = Convert.ToUInt32(critEncounter.List<Encounter>()[0].Appointment_Provider_ID);


                }

                InHouseProcedureManager objInhouseMngr = new InHouseProcedureManager();
                chkoutPrintDTO.Inhouse_procedureDto = objInhouseMngr.FillInHouseProcedure(EncounterID, Physician_ID, HumanId);

                iMySession.Close();
            }
            return chkoutPrintDTO;


        }

        #endregion

        #region Method Used For Plan

        private string PlanString(IList<string> ProcedureList, string PlanText)
        {
            if (ProcedureList != null)
            {
                string[] planTextArray = PlanText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in ProcedureList)
                {
                    if (str != string.Empty && !planTextArray.Any(a => a == "*" + str))
                    {
                        PlanText += "*" + str + Environment.NewLine;
                    }
                }
            }
            return PlanText;
        }
        #endregion
        public bool NotSubmitedOrders(ulong myHumanId, ulong myEncounterID, ulong PhysicianID)
        {

            WFObjectManager ObjWfObjectMgr = new WFObjectManager();

            IList<string> WfObjDetails = new List<string>();
            string str = string.Empty;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.WfObjectsForOrdersWithoutEncounter");
                query.SetString(0, myHumanId.ToString());
                query.SetString(1, myEncounterID.ToString());
                query.SetString(2, PhysicianID.ToString());
                ArrayList WfobjectOrders = new ArrayList(query.List());
                ICriteria critOrders = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Human_ID", myHumanId)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Encounter_ID", myEncounterID));
                if (critOrders != null)
                {
                    IList<OrdersSubmit> ilstOrders = new List<OrdersSubmit>();
                    ilstOrders = critOrders.List<OrdersSubmit>();
                    if (WfobjectOrders.Count < ilstOrders.Count)
                        return false;
                }
                ICriteria critImm = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Human_ID", myHumanId)).Add(Expression.Eq("Physician_Id", PhysicianID)).Add(Expression.Eq("Encounter_Id", myEncounterID));
                if (critImm != null)
                {
                    IList<Immunization> ilstOrders = new List<Immunization>();
                    ilstOrders = critImm.List<Immunization>();
                    foreach (Immunization ord in ilstOrders)
                    {
                        if (ord.Immunization_Group_ID == 0)
                            return false;
                    }
                }
                ICriteria critRefferal = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Human_ID", myHumanId)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Encounter_ID", myEncounterID));
                if (critRefferal != null)
                {
                    IList<InHouseProcedure> ilstOrders = new List<InHouseProcedure>();
                    ilstOrders = critRefferal.List<InHouseProcedure>();
                    foreach (InHouseProcedure ord in ilstOrders)
                    {
                        if (ord.In_House_Procedure_Group_ID == 0)
                            return false;
                    }

                }

                iMySession.Close();
            }
            return true;

        }
        #region Insert Dummy Orders For Manual Result Entry
        public ulong InsertDummyOrderForManualResultEntry(IList<OrdersSubmit> ordersSubmitList, IList<Orders> ordersList, string obj_Type, string facility_Name, string MACAddress)
        {

            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            ulong OrderId = 0;
            ISession MySession = Session.GetISession();
            // ITransaction trans = null;

            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {

                    try
                    {
                        //trans = MySession.BeginTransaction();
                        if (ordersSubmitList != null)
                        {
                            if (ordersSubmitList.Count > 0)
                            {
                                OrdersSubmitManager orderSubmitMngr = new OrdersSubmitManager();
                                iResult = orderSubmitMngr.InsertDummyOrderForResultEntry(ref ordersSubmitList, MySession, MACAddress);
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
                                ordersList[0].Order_Submit_ID = ordersSubmitList[0].Id;
                                OrdersManager orderMngr = new OrdersManager();
                                iResult = orderMngr.InsertDummyOrderForResultEntry(ref ordersList, MySession, MACAddress);
                                foreach (Orders obj in ordersList)
                                {

                                    OrderId = obj.Order_Submit_ID;

                                    WFObject WFObj = new WFObject();
                                    WFObj.Obj_Type = obj_Type.ToUpper();
                                    WFObj.Current_Arrival_Time = obj.Created_Date_And_Time;
                                    WFObj.Current_Owner = "UNKNOWN";
                                    WFObj.Fac_Name = facility_Name;
                                    WFObj.Parent_Obj_Type = string.Empty;
                                    WFObj.Obj_System_Id = obj.Order_Submit_ID;
                                    WFObj.Current_Process = "START";
                                    WFObjectManager objWfMnger = new WFObjectManager();
                                    iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 2, MACAddress, MySession);
                                    // iResult = objWfMnger.MoveToNextProcess(WFObj.Obj_System_Id, WFObj.Obj_Type, 1, "UNKNOWN", obj.Created_Date_And_Time, MACAddress, null, MySession);
                                    //iResult = objWfMnger.MoveToNextProcess(WFObj, 1, "UNKNOWN", obj.Created_Date_And_Time, MACAddress, MySession);
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
                            }
                        }

                        MySession.Flush();
                        trans.Commit();
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
                //MySession.Close();
                throw new Exception(ex1.Message);
            }
            return OrderId;

        }
        #endregion

        public Stream SubmitLogic(ulong myHumanId, ulong myEncounterID, ulong PhysicianID, string OrderType, string sUserName, DateTime CreatedDate, string sFacilityName)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objdetDTO = new OrdersDTO();
            if (myEncounterID > 0)
            {
                object objStream = (object)serializer.ReadObject(GetOrdersUsingEncounterID(myEncounterID, OrderType, false));
                objdetDTO = (OrdersDTO)objStream;

            }
            else
            {
                object objStream = (object)serializer.ReadObject(GetOrdersUsingHumanID(myHumanId, CreatedDate, OrderType, PhysicianID, false));
                objdetDTO = (OrdersDTO)objStream;
            }

            var query1 = from q in objdetDTO.ilstOrderLabDetailsDTO where q.LabName == "LabCorp" select q;
            var query2 = from p in objdetDTO.ilstOrderLabDetailsDTO where p.LabName == "Quest Diagnostics" select p;
            var query3 = from r in objdetDTO.ilstOrderLabDetailsDTO where (r.LabName != "LabCorp" && r.LabName != "Quest Diagnostics") select r;
            OrdersDTO objLabCorp = new OrdersDTO();
            OrdersDTO objQuest = new OrdersDTO();
            OrdersDTO objOtherLab = new OrdersDTO();
            objLabCorp.ilstOrderLabDetailsDTO = query1.ToList<OrderLabDetailsDTO>();
            objQuest.ilstOrderLabDetailsDTO = query2.ToList<OrderLabDetailsDTO>();
            objOtherLab.ilstOrderLabDetailsDTO = query3.ToList<OrderLabDetailsDTO>();
            SubmitOrdersToLabCorp(objLabCorp, myHumanId, myEncounterID, sUserName, CreatedDate, sFacilityName);
            SubmitOrdersToQuest(objQuest, myHumanId, myEncounterID, sUserName, CreatedDate, sFacilityName);
            SubmitOrdersToOtherLab(objOtherLab, myHumanId, myEncounterID, sUserName, CreatedDate, sFacilityName);
            if (myEncounterID != 0)
            {
                return GetOrdersUsingEncounterID(myEncounterID, OrderType, true);
            }
            else
            {
                return GetOrdersUsingHumanID(myHumanId, CreatedDate, OrderType, PhysicianID, true);
            }

        }
        public void SubmitOrdersToLabCorp(OrdersDTO orderDetDTO, ulong myHumanId, ulong myEncounterID, string sUserName, DateTime CreateDate, string sFacility)
        {
            ulong ulHumanID = myHumanId;
            ulong ulEncounterID = myEncounterID;
            string sMyUserName = sUserName;
            DateTime MyCreateDate = CreateDate;
            string sMyFacility = sFacility;
            IList<Orders> ordList = new List<Orders>();
            IList<Orders> ordUpdtList = new List<Orders>();
            IList<OrdersSubmit> ordSubmitList = new List<OrdersSubmit>();
            // IList<OrdersSubmit> ordSubmitListDelete = new List<OrdersSubmit>();
            IList<string> orderCodeTypes = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.ObjOrder.Order_Submit_ID == 0 select obj.ObjOrder.Order_Code_Type).Distinct().ToList<string>();
            ordList = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.ObjOrder.Order_Submit_ID == 0 select obj.ObjOrder).ToList<Orders>();
            foreach (string str in orderCodeTypes)
            {
                OrdersSubmit objSubmit = new OrdersSubmit();
                objSubmit.Human_ID = ulHumanID;
                objSubmit.Encounter_ID = ulEncounterID;
                objSubmit.Order_Code_Type = str;
                objSubmit.Created_By = sMyUserName;
                objSubmit.Created_Date_And_Time = MyCreateDate;
                //objSubmit.Modified_By = ClientSession.UserName;
                //objSubmit.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                objSubmit.Internal_Property_Signature_Date_And_Time = MyCreateDate;
                ordSubmitList.Add(objSubmit);
            }
            //IList<OrdersSubmit> ordSubmitListAfterSubmit = null;
            if (ordSubmitList.Count > 0)
            {
                OrdersSubmitManager OrderSubmitMngr = new OrdersSubmitManager();
                //Orders Revamp
                //ordSubmitListAfterSubmit = OrderSubmitMngr.GroupOrdersToSendToLabCorp(ordSubmitList.ToArray<OrdersSubmit>(), ordList.ToArray<Orders>(), string.Empty);
            }
        }

        public void SubmitOrdersToQuest(OrdersDTO orderDetDTO, ulong myHumanId, ulong myEncounterID, string sUserName, DateTime CreateDate, string sFacility)
        {

            ulong ulHumanID = myHumanId;
            ulong ulEncounterID = myEncounterID;
            string sMyUserName = sUserName;
            DateTime MyCreateDate = CreateDate;
            string sMyFacility = sFacility;
            IList<Orders> ordList = new List<Orders>();
            IList<Orders> ordUpdtList = new List<Orders>();
            IList<OrdersSubmit> ordSubmitList = new List<OrdersSubmit>();
            IList<OrderCodeLibrary> ilstOrderCodeLibrary = new List<OrderCodeLibrary>();
            // IList<OrdersSubmit> ordSubmitListDelete = new List<OrdersSubmit>();
            IList<string> orderCodeTypes = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.ObjOrder.Order_Submit_ID == 0 select obj.ObjOrder.Lab_Procedure).Distinct().ToList<string>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria cri = iMySession.CreateCriteria(typeof(OrderCodeLibrary)).Add(Expression.In("Order_Code", orderCodeTypes.ToArray<string>()));
                ilstOrderCodeLibrary = cri.List<OrderCodeLibrary>();
                ICriteria criStatic = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", "QUEST TEMPRATURE STATE"));
                IList<StaticLookup> slookup = criStatic.List<StaticLookup>();
                ilstOrderCodeLibrary = cri.List<OrderCodeLibrary>();
                var Pap = (from rec in ilstOrderCodeLibrary where rec.PAP_Indicator == "P" select rec);
                foreach (OrderCodeLibrary ordcode in Pap)
                {
                    OrdersSubmit objSubmit = new OrdersSubmit();
                    objSubmit.Human_ID = ulHumanID;
                    objSubmit.Encounter_ID = ulEncounterID;
                    objSubmit.Internal_Property_Temperature_state = ordcode.PAP_Indicator;
                    objSubmit.Created_By = sMyUserName;
                    objSubmit.Created_Date_And_Time = MyCreateDate;
                    objSubmit.Internal_Property_Order_Code = ordcode.Order_Code;
                    //objSubmit.Modified_By = ClientSession.UserName;
                    //objSubmit.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                    objSubmit.Internal_Property_Signature_Date_And_Time = MyCreateDate;
                    ordSubmitList.Add(objSubmit);
                }
                IList<string> ilstTemperature = (from rec in ilstOrderCodeLibrary where rec.PAP_Indicator != "P" && rec.Temperature_State != string.Empty select rec.Temperature_State).Distinct().ToList<string>();
                foreach (string str in ilstTemperature)
                {
                    OrdersSubmit objSubmit = new OrdersSubmit();
                    objSubmit.Human_ID = ulHumanID;
                    objSubmit.Encounter_ID = ulEncounterID;
                    objSubmit.Internal_Property_Temperature_state = slookup.Where(a => a.Description == str).Select(a => a.Value).SingleOrDefault();
                    objSubmit.Created_By = sMyUserName;
                    objSubmit.Created_Date_And_Time = MyCreateDate;

                    //objSubmit.Modified_By = ClientSession.UserName;
                    //objSubmit.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                    objSubmit.Internal_Property_Signature_Date_And_Time = MyCreateDate;
                    ordSubmitList.Add(objSubmit);
                }
                //var ilstPanel=(from rec in ilstOrderCodeLibrary where  rec.Temperature_State==string.Empty && rec.PAP_Indicator!="P"  select rec);
                //foreach(OrderCodeLibrary ordcode in ilstPanel)
                //{
                //    OrdersSubmit objSubmit = new OrdersSubmit();
                //    objSubmit.Human_ID = ulHumanID;
                //    objSubmit.Encounter_ID = ulEncounterID;
                //    //objSubmit.Temperature_state = slookup.Where(a => a.Description == str).Select(a => a.Value).SingleOrDefault();
                //    objSubmit.Created_By = sMyUserName;
                //    objSubmit.Created_Date_And_Time = MyCreateDate;
                //    objSubmit.Order_Code = ordcode.Order_Code;
                //    //objSubmit.Modified_By = ClientSession.UserName;
                //    //objSubmit.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                //    objSubmit.Signature_Date_And_Time = MyCreateDate;
                //    ordSubmitList.Add(objSubmit);

                //}





                //foreach (Orders ord in Pat)
                //{
                //    OrdersSubmit objSubmit = new OrdersSubmit();
                //    objSubmit.Human_ID = ulHumanID;
                //    objSubmit.Encounter_ID = ulEncounterID;
                //    objSubmit.Temperature_state = ord.Temperature;
                //    objSubmit.Created_By = sMyUserName;
                //    objSubmit.Created_Date_And_Time = MyCreateDate;

                //    //objSubmit.Modified_By = ClientSession.UserName;
                //    //objSubmit.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                //    objSubmit.Signature_Date_And_Time = MyCreateDate;
                //    ordSubmitList.Add(objSubmit);
                //}
                //var Temp = (from rec in ilstOrderCodeLibrary where rec.PAP_Indicator != "P" && rec.Temperature_State != string.Empty select rec.Temperature_State).Distinct().ToList<string>();
                //{

                //}
                //var Panel



                ordList = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.ObjOrder.Order_Submit_ID == 0 select obj.ObjOrder).ToList<Orders>();
                //foreach (string str in orderCodeTypes)
                //{
                //    OrdersSubmit objSubmit = new OrdersSubmit();
                //    objSubmit.Human_ID = ulHumanID;
                //    objSubmit.Encounter_ID = ulEncounterID;
                //    objSubmit.Temperature_state = str;
                //    objSubmit.Created_By = sMyUserName;
                //    objSubmit.Created_Date_And_Time = MyCreateDate;

                //    //objSubmit.Modified_By = ClientSession.UserName;
                //    //objSubmit.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                //    objSubmit.Signature_Date_And_Time = MyCreateDate;
                //    ordSubmitList.Add(objSubmit);
                //}
                // IList<OrdersSubmit> ordSubmitListAfterSubmit = null;
                if (ordSubmitList.Count > 0)
                {
                    OrdersSubmitManager OrderSubmitMngr = new OrdersSubmitManager();
                    //Orders Revamp
                    //ordSubmitListAfterSubmit = OrderSubmitMngr.GroupOrdersToSendToQuest(ordSubmitList.ToArray<OrdersSubmit>(), ordList.ToArray<Orders>(), string.Empty);
                }

                iMySession.Close();
            }
        }

        public void SubmitOrdersToOtherLab(OrdersDTO orderDetDTO, ulong myHumanId, ulong myEncounterID, string sUserName, DateTime CreateDate, string sFacility)
        {
            ulong ulHumanID = myHumanId;
            ulong ulEncounterID = myEncounterID;
            string sMyUserName = sUserName;
            DateTime MyCreateDate = CreateDate;
            string sMyFacility = sFacility;
            IList<Orders> ordList = new List<Orders>();
            IList<Orders> ordUpdtList = new List<Orders>();
            IList<OrdersSubmit> ordSubmitList = new List<OrdersSubmit>();
            // IList<OrdersSubmit> ordSubmitListDelete = new List<OrdersSubmit>();
            IList<string> orderCodeTypes = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.ObjOrder.Order_Submit_ID == 0 select obj.ObjOrder.Order_Code_Type).Distinct().ToList<string>();
            ordList = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.ObjOrder.Order_Submit_ID == 0 select obj.ObjOrder).ToList<Orders>();
            foreach (string str in orderCodeTypes)
            {
                OrdersSubmit objSubmit = new OrdersSubmit();
                objSubmit.Human_ID = ulHumanID;
                objSubmit.Encounter_ID = ulEncounterID;
                objSubmit.Order_Code_Type = str;
                objSubmit.Created_By = sMyUserName;
                objSubmit.Created_Date_And_Time = MyCreateDate;
                //objSubmit.Modified_By = ClientSession.UserName;
                //objSubmit.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                objSubmit.Internal_Property_Signature_Date_And_Time = MyCreateDate;
                ordSubmitList.Add(objSubmit);
            }
            //IList<OrdersSubmit> ordSubmitListAfterSubmit = null;
            if (ordSubmitList.Count > 0)
            {
                OrdersSubmitManager OrderSubmitMngr = new OrdersSubmitManager();
                //Orders Revamp
                //ordSubmitListAfterSubmit = OrderSubmitMngr.GroupOrdersToSendToLabCorp(ordSubmitList.ToArray<OrdersSubmit>(), ordList.ToArray<Orders>(), string.Empty);
            }
        }


        //Dhinesh-For Quest
        public IList<ulong> GetWfObjectIDsFromObjSystemIDs(IList<ulong> OrderSubmitIDs, string Objtype)
        {
            IList<ulong> ids = new List<ulong>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.WfObjectsIdsByObjsystemIds");
                query.SetString(0, Objtype);
                query.SetParameterList("WfobjSubIDs", OrderSubmitIDs.ToArray<ulong>());
                ArrayList WfobjectOrders = new ArrayList(query.List());

                foreach (object obj in WfobjectOrders)
                {
                    ids.Add(Convert.ToUInt32(obj));
                }

                iMySession.Close();
            }
            return ids;


        }
        public int InsertDummyOrderForResultEntry(ref IList<Orders> ordersList, ISession MySession, string macAddress)
        {
            int iResult = 0;
            GenerateXml XMLObj = new GenerateXml();
            //iResult = SaveUpdateDeleteWithoutTransaction(ref ordersList, null, null, MySession, macAddress);
            IList<Orders> ordersListnull = null;
            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordersList, ref ordersListnull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);
            return iResult;

        }
        public ulong GetPhysicianIDByOrderSubmitID(ulong OrderSubmitID)
        {
            IList<Orders> ilstOrders = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteriaByOrderSubmitID = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Order_Submit_ID", OrderSubmitID));
                ilstOrders = criteriaByOrderSubmitID.List<Orders>();

                iMySession.Close();
            }
            return ilstOrders[0].Physician_ID;
        }


        public ulong GetHumanID(IList<Orders> savelist, IList<Orders> updatelist, IList<Orders> deletelist)
        {
            ulong ulHumanId = 0;
            if (savelist != null && savelist.Count > 0)
                return savelist[0].Human_ID;
            if (updatelist != null && updatelist.Count > 0)
                return updatelist[0].Human_ID;
            if (deletelist != null && deletelist.Count > 0)
                return deletelist[0].Human_ID;
            return ulHumanId;
        }
        public ulong GetPhysicianID(IList<Orders> savelist, IList<Orders> updatelist, IList<Orders> deletelist)
        {
            ulong PhysicianID = 0;
            if (savelist != null && savelist.Count > 0)
                return savelist[0].Physician_ID;
            if (updatelist != null && updatelist.Count > 0)
                return updatelist[0].Physician_ID;
            if (deletelist != null && deletelist.Count > 0)
                return deletelist[0].Physician_ID;
            return PhysicianID;
        }
        public DateTime GetCreatedDateAndTime(IList<OrdersSubmit> saveOrdersSubmitlist, IList<OrdersSubmit> updateOrdersSubmitlist, IList<OrdersSubmit> deleteOrdersSubmitlist)
        {

            if (saveOrdersSubmitlist != null && saveOrdersSubmitlist.Count > 0)
                return saveOrdersSubmitlist[0].Created_Date_And_Time;
            if (updateOrdersSubmitlist != null && updateOrdersSubmitlist.Count > 0)
                return updateOrdersSubmitlist[0].Created_Date_And_Time;
            if (deleteOrdersSubmitlist != null && deleteOrdersSubmitlist.Count > 0)
                return deleteOrdersSubmitlist[0].Created_Date_And_Time;
            return new DateTime();
        }

        public bool DeletedOrders(ulong OrderSubmitID, string objType, string MacAddress)
        {
            WFObjectManager objWFObjectManager = new WFObjectManager();
            WFObject objWFObject = objWFObjectManager.GetByObjectSystemId(OrderSubmitID, objType);
            if (objWFObject.Current_Process != "MA_REVIEW" && objWFObject.Current_Process != "ORDER_GENERATE")
            {
                return false;
            }
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            OrdersSubmit objOrdersSubmit = objOrdersSubmitManager.GetById(OrderSubmitID);
            OrdersManager objOrdersManager = new OrdersManager();
            IList<ulong> tempIds = new List<ulong>();
            tempIds.Add(objOrdersSubmit.Id);
            string sMyFinalFormatString = string.Empty;
            IList<Orders> lstorders = objOrdersManager.GetOrdersBySubmitID(tempIds);
            IList<int> index = new List<int>();
            #region OldCode
            //TreatmentPlan UpdateTreatmentPlan = null;
            //TreatmentPlan DeleteTreatmentPlan = null;
            //if (objOrdersSubmit.Encounter_ID != 0)
            //{
            //    TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            //    IList<TreatmentPlan> Treatment_Plan = new List<TreatmentPlan>();
            //    Treatment_Plan = objTreatmentPlanMgr.GetTreatmentPlanUsingEncounterId(objOrdersSubmit.Encounter_ID, objOrdersSubmit.Human_ID);
            //    IList<TreatmentPlan> Treatmentlst = (from t in Treatment_Plan where t.Plan_Type.ToUpper() == objType.ToUpper() select t).ToList<TreatmentPlan>();
            //    if (Treatmentlst.Count > 0)
            //    {
            //        string sPlanValue = Treatmentlst[0].Plan;
            //        foreach (Orders s in lstorders)
            //        {

            //            sPlanValue = sPlanValue.Replace("*" + s.Lab_Procedure_Description + Environment.NewLine, "").Trim();

            //            if (sPlanValue != string.Empty)
            //            {
            //                sPlanValue = sPlanValue + Environment.NewLine;
            //            }

            //        }

            //        if (sPlanValue != string.Empty)
            //        {
            //            UpdateTreatmentPlan = new TreatmentPlan();
            //            UpdateTreatmentPlan = Treatmentlst[0];
            //            UpdateTreatmentPlan.Plan = sPlanValue;

            //        }
            //        else
            //        {
            //            DeleteTreatmentPlan = new TreatmentPlan();
            //            DeleteTreatmentPlan = Treatmentlst[0];

            //        }
            //    }
            //}
            #endregion
            //jira #CAP-248
            //Session.GetISession().Clear();
            //ISession MySession = Session.GetISession();
            ITransaction trans = null;
            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();
            GenerateXml XMLObj = new GenerateXml();

            iTryCount = 0;
            #region TplanGet
            ulong EncounterID = objOrdersSubmit.Encounter_ID;
            IList<string> ilstEncounterTag = new List<string>();
            ilstEncounterTag.Add("TreatmentPlanList");

            IList<object> ilstEncounterBlobList = new List<object>();

            ilstEncounterBlobList = ReadBlob(objOrdersSubmit.Encounter_ID, ilstEncounterTag);

            if (ilstEncounterBlobList != null && ilstEncounterBlobList.Count > 0)
            {
                if (ilstEncounterBlobList[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstEncounterBlobList[0]).Count; iCount++)
                    {
                        if (((TreatmentPlan)((IList<object>)ilstEncounterBlobList[0])[iCount]).Encounter_Id == EncounterID && ((TreatmentPlan)((IList<object>)ilstEncounterBlobList[0])[iCount]).Plan_Type == "DIAGNOSTIC ORDER")
                        {
                            objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstEncounterBlobList[0])[iCount]);
                        }
                    }
                }
            }

        //string FileName = "Encounter" + "_" + objOrdersSubmit.Encounter_ID + ".xml";
        ////string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
        //XmlTextReader XmlText = null;
        //try
        //{
        //    if (File.Exists(strXmlFilePath) == true)
        //    {
        //        XmlDocument itemDoc = new XmlDocument();
        //        XmlText = new XmlTextReader(strXmlFilePath);
        //        XmlNodeList xmlTagName = null;
        //        // itemDoc.Load(XmlText);
        //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        {
        //            itemDoc.Load(fs);

        //            XmlText.Close();
        //            #region Treatment_plan
        //            if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
        //            {
        //                xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

        //                if (xmlTagName.Count > 0)
        //                {
        //                    for (int j = 0; j < xmlTagName.Count; j++)
        //                    {
        //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("DIAGNOSTIC ORDER"))
        //                        {

        //                            string TagName = xmlTagName[j].Name;
        //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
        //                            TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
        //                            IEnumerable<PropertyInfo> propInfo = null;
        //                            propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

        //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
        //                            {

        //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
        //                                {
        //                                    foreach (PropertyInfo property in propInfo)
        //                                    {
        //                                        if (property.Name == nodevalue.Name)
        //                                        {
        //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
        //                                                property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
        //                                                property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
        //                                                property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
        //                                                property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
        //                                            else
        //                                                property.SetValue(TreatmentPlan, nodevalue.Value, null);
        //                                        }
        //                                    }
        //                                }

        //                            }
        //                            objTreatmentPlan.Add(TreatmentPlan);
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            fs.Close();
        //            fs.Dispose();
        //        }
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    if (XmlText != null)
        //        XmlText.Close();
        //    throw Ex;
        //}
        #endregion
        TryAgain:
            int iResult = 0;
            try
            {
                //jira #CAP-248
                ISession MySession = Session.GetISession();
                #region TreatmentPlan
                Delete_Tplan = (from obj in objTreatmentPlan where lstorders.Any(a => a.Id == obj.Source_ID) select obj).ToList<TreatmentPlan>();
                if (Delete_Tplan != null && Delete_Tplan.Count > 0)
                {
                    iResult = objTreatmentPlanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, Delete_Tplan, MySession, MacAddress, true, true, EncounterID, string.Empty, ref XMLObj);

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
                        //MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                    // if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                    if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                    {
                        //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                        //    int trycount = 0;
                        //trytosaveagain:
                        try
                        {
                            // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            //WriteBlob(objOrdersSubmit.Human_ID, XMLObj.itemDoc, MySession, null, null, lstorders, XMLObj, false);
                            objTreatmentPlanManager.WriteBlob(objOrdersSubmit.Encounter_ID, XMLObj.itemDoc, MySession, Insert_Tplan, Update_Tplan, Delete_Tplan, XMLObj, false);
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
                    }
                }

                //if (UpdateTreatmentPlan != null || DeleteTreatmentPlan != null)
                //{
                //    TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
                //    iResult = objTreatmentPlanMgr.SaveUpdateorDeleteTreatmentPlan(null, UpdateTreatmentPlan, DeleteTreatmentPlan, MySession, MacAddress);
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
                //            MySession.Close();
                //            throw new Exception("Deadlock occurred. Transaction failed.");
                //        }
                //    }
                //    else if (iResult == 1)
                //    {
                //        trans.Rollback();
                //        MySession.Close();
                //        throw new Exception("Exception occurred. Transaction failed.");
                //    }

                //}
                #endregion
                //WFObjectManager objWFObjectManager = new WFObjectManager();
                objWFObjectManager.MoveToNextProcess(OrderSubmitID, objType, 6, "UNKNOWN", DateTime.Now, MacAddress, null, null);

                //Mark Is_Deleted attribute="Y" in order_submit

                IList<OrdersSubmit> ilstOrderSubmitxml = new List<OrdersSubmit>();
                ilstOrderSubmitxml.Add(objOrdersSubmit);
                ulong humanid = 0;
                if (ilstOrderSubmitxml.Count == 1)
                {
                    MySession = Session.GetISession();
                    humanid = ilstOrderSubmitxml[0].Human_ID;
                    ilstOrderSubmitxml[0].Is_Deleted = "Y";
                    ilstOrderSubmitxml[0].Is_Task_Created = "N";

                    XMLObj = new GenerateXml();
                    IList<OrdersSubmit> ilstOrderSubmitsave = new List<OrdersSubmit>();
                    iResult = objOrdersSubmitManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilstOrderSubmitsave, ref ilstOrderSubmitxml, null, MySession, string.Empty, true, true, humanid, string.Empty, ref XMLObj);
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
                    if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                    {
                        // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                        //int trycount = 0;
                        //trytosaveagain:
                        try
                        {
                            #region "Code comitted by balaji.TJ 2023-01-08"
                            ///XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            #endregion

                            #region "Code Modified by balaji.TJ 2023-01-08
                            WriteBlob(objOrdersSubmit.Human_ID, XMLObj.itemDoc, MySession, null, null, lstorders, XMLObj, false);
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

                    }
                    //List<object> lstObj = ilstOrderSubmitxml.Cast<object>().ToList();
                    //XMLObj.GenerateXmlUpdate(lstObj, humanid, string.Empty,false);
                }
                MySession.Flush();
                return true;
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
        }
        //<<<<<<< OrdersManager.cs
        //      public bool IsEditable(ulong OrderSubmit,string objType)
        //      {
        //            WFObjectManager objWFObjectManager = new WFObjectManager(); 
        //            WFObject objWFObject = objWFObjectManager.GetByObjectSystemId(OrderSubmit, objType);
        //           if (objWFObject.Current_Process != "MA_REVIEW" && objWFObject.Current_Process != "ORDER_GENERATE")
        //               return false;
        //           else
        //               return true;
        //      }
        //||||||| 1.131
        //=======
        public bool IsEditable(ulong OrderSubmit, string objType)
        {
            WFObjectManager objWFObjectManager = new WFObjectManager();
            WFObject objWFObject = objWFObjectManager.GetByObjectSystemId(OrderSubmit, objType);
            if (objWFObject.Current_Process != "MA_REVIEW" && objWFObject.Current_Process != "ORDER_GENERATE" && objWFObject.Current_Process != "ORDER_SEND")
                return false;
            else
                return true;
        }

        public DiagnosticDTO FillDiagnosticDTO(ulong OrderSubmitID, ulong EncounterId, ulong HumanId, ulong Physician_Id, string Procedure_Type, string sFacilityName, string sLegalOrg)
        {

            ulong Lab_ID = 0;
            DiagnosticDTO objDiagnosticDTO = new DiagnosticDTO();
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            OrdersManager objOrdersManager = new OrdersManager();
            LabInsurancePlanManager objLabInsurancePlanManager = new LabInsurancePlanManager();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (OrderSubmitID != 0)
                {
                    objDiagnosticDTO.objOrdersSubmit = objOrdersSubmitManager.GetById(OrderSubmitID);
                    Lab_ID = objDiagnosticDTO.objOrdersSubmit.Lab_ID;
                    ICriteria OrdersCrit = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Order_Submit_ID", OrderSubmitID));
                    IList<Orders> OrdersBySubmitID = OrdersCrit.List<Orders>();
                    objDiagnosticDTO.OrdersLists = OrdersBySubmitID;

                    IList<OrdersAssessment> OrdersAssessmentList = null;
                    if (OrdersBySubmitID != null)
                    {
                        //IList<ulong> OrderIDs = OrdersBySubmitID.Select(a => a.Id).ToList<ulong>();
                        IList<ulong> OrderIDs = OrdersBySubmitID.Select(a => a.Order_Submit_ID).ToList<ulong>();
                        ICriteria OrdersAssessmentCrit = iMySession.CreateCriteria(typeof(OrdersAssessment)).Add(Expression.In("Order_Submit_ID", OrderIDs.ToArray<ulong>()));
                        OrdersAssessmentList = OrdersAssessmentCrit.List<OrdersAssessment>();
                        objDiagnosticDTO.OrderAssList = OrdersAssessmentList;
                    }
                }
                //  ICriteria AssesCrit = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", EncounterId)).Add(Expression.Eq("Assessment_Type", "Selected")).Add(Expression.Eq("Version_Year", "ICD_10")).AddOrder(Order.Desc("Primary_Diagnosis"));
                ICriteria AssesCrit = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Encounter_ID", EncounterId)).Add(Expression.Eq("Assessment_Type", "Selected")).Add(Expression.Eq("Version_Year", "ICD_10")).AddOrder(Order.Desc("Primary_Diagnosis"));
                IList<Assessment> AssessmntList = AssesCrit.List<Assessment>();
                if (AssessmntList.Count > 0)
                {
                    objDiagnosticDTO.AssessmentList = AssessmntList;
                }
                ICriteria Problemcrt = iMySession.CreateCriteria(typeof(ProblemList)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Status", "Active")).Add(Expression.Eq("Is_Active", "Y")).Add(Expression.Eq("Version_Year", "ICD_10")).Add(Expression.Not(Expression.Eq("ICD", "0000")));
                IList<ProblemList> ProblemList = Problemcrt.List<ProblemList>();

                if (ProblemList.Count > 0)
                {
                    objDiagnosticDTO.MedAdvProblemList = ProblemList;
                }
                //Remove Duplicate ICD
                IList<PatientResults> objPatientList = new List<PatientResults>();
                IList<ProblemList> ProblemlistVitals = new List<ProblemList>();
                IList<AllICD_9> allICD9ForVitalsProblemListPFSH = new List<AllICD_9>();
                AllICD_9Manager objAllIcdMgr = new AllICD_9Manager();
                //ISQLQuery sqlquery = session.GetISession().CreateSQLQuery("select v.* from patient_results v inner join dynamic_screen s on (v.Loinc_Observation=s.control_Name)   where v.encounter_id='" + encounterID + "' and v.human_id='" + humanID + "' and Results_Type='Vitals' and v.Vitals_group_id in(select max(Vitals_group_id) from patient_results where human_id=v.human_id and encounter_id= v.encounter_id) group by v.Loinc_Observation order by s.sort_order").AddEntity("v", typeof(PatientResults));
                ArrayList aryPatientResultList;
                IQuery query1 = iMySession.GetNamedQuery("Get.PatientResultsList.Test");
                query1.SetString(0, EncounterId.ToString());
                query1.SetString(1, HumanId.ToString());
                aryPatientResultList = new ArrayList(query1.List());
                foreach (object[] oj in aryPatientResultList)
                {
                    PatientResults objPatientResults = new PatientResults();
                    objPatientResults.Id = Convert.ToUInt32(oj[0]);
                    objPatientResults.Loinc_Observation = oj[1].ToString();
                    objPatientResults.Value = oj[2].ToString();
                    objPatientResults.Units = oj[3].ToString();
                    objPatientList.Add(objPatientResults);

                }

                if (objDiagnosticDTO.MedAdvProblemList.Count > 0)
                {
                    IList<AssessmentVitalsLookup> AssessmentVitalsLookupList = new List<AssessmentVitalsLookup>();
                    IList<AssessmentVitalsLookup> AssessmentVitalLookuplst = new List<AssessmentVitalsLookup>();

                    ICriteria crit2 = iMySession.CreateCriteria(typeof(AssessmentVitalsLookup));
                    AssessmentVitalsLookupList = crit2.List<AssessmentVitalsLookup>();
                    int count = 0;
                    IList<string> VitalsBasedICD_List = new List<string>();
                    AssessmentVitalsLookupManager objAssessVitalsMngr = new AssessmentVitalsLookupManager();
                    VitalsBasedICD_List = objAssessVitalsMngr.GetIcdBasedOnVitals(AssessmentVitalsLookupList, objPatientList);//objPatientList : vitals data for current encounter
                    string[] VitalsICD = new string[AssessmentVitalsLookupList.Count];
                    int k = 0;
                    string[] prblVitalICD = new string[objDiagnosticDTO.MedAdvProblemList.Count];

                    for (int i = 0; i < AssessmentVitalsLookupList.Count; i++)
                    {
                        if (AssessmentVitalsLookupList[i].ICD_10.Trim() != "")
                            VitalsICD[i] = AssessmentVitalsLookupList[i].ICD_10;
                    }

                    if (VitalsBasedICD_List.Count > 0)
                    {
                        for (int i = 0; i < objDiagnosticDTO.AssessmentList.Count; i++)
                        {
                            if (VitalsBasedICD_List.IndexOf(objDiagnosticDTO.AssessmentList[i].ICD) > -1)
                            {
                                count = 1;
                                break;
                            }
                        }
                        for (int i = 0; i < objDiagnosticDTO.MedAdvProblemList.Count; i++)
                        {
                            if (VitalsICD.Contains(objDiagnosticDTO.MedAdvProblemList[i].ICD) && VitalsBasedICD_List.IndexOf(objDiagnosticDTO.MedAdvProblemList[i].ICD) == -1)
                            {
                                prblVitalICD[k] = objDiagnosticDTO.MedAdvProblemList[i].ICD;
                                k++;

                            }
                            if (VitalsBasedICD_List.IndexOf(objDiagnosticDTO.MedAdvProblemList[i].ICD) > -1)
                            {
                                ProblemlistVitals.Add(objDiagnosticDTO.MedAdvProblemList[i]);
                            }
                        }

                        if (k != 0)
                        {
                            for (int i = 0; i < k; i++)
                            {
                                // ProblemlistVitals.Add(objDiagnosticDTO.MedAdvProblemList.First(a => a.ICD == prblVitalICD[i]));
                                objDiagnosticDTO.MedAdvProblemList.Remove(objDiagnosticDTO.MedAdvProblemList.First(a => a.ICD == prblVitalICD[i]));

                            }
                        }

                        if (AssessmentVitalsLookupList.Count > 0 && count == 0)
                        {
                            AssessmentVitalLookuplst = (from val in AssessmentVitalsLookupList
                                                        where (val.Field_Name == "BMI" || val.Field_Name == "BMI PERCENTILE" || val.Field_Name == "BMI STATUS")
                                                        select val).Distinct().ToList();
                            if (AssessmentVitalLookuplst.Count > 0)
                            {
                                objDiagnosticDTO.AssessmentList = (from a in objDiagnosticDTO.AssessmentList where !AssessmentVitalLookuplst.Any(b => b.ICD_10 == a.ICD) select a).ToList();
                                objDiagnosticDTO.MedAdvProblemList = (from a in objDiagnosticDTO.MedAdvProblemList where !AssessmentVitalLookuplst.Any(b => b.ICD_10 == a.ICD) select a).ToList();

                            }

                        }
                    }
                    foreach (ProblemList obj in ProblemlistVitals)
                        objDiagnosticDTO.MedAdvProblemList.Add(obj);
                }

                // if (OrderSubmitID == 0)
                //{
                FillHumanDTO objFillHumnaDTO = new FillHumanDTO();
                objFillHumnaDTO = objOrdersManager.PatientInsuredBag(HumanId);
                objDiagnosticDTO.objFillHumnaDTO = objFillHumnaDTO;
                ulong PrimaryInsurance = 0;
                if (objFillHumnaDTO != null && objFillHumnaDTO.PatientInsuredBag != null && objFillHumnaDTO.PatientInsuredBag.Count > 0)//BugID:53714
                    PrimaryInsurance = objFillHumnaDTO.PatientInsuredBag.Where(a => a != null && a.Insurance_Type != null && a.Insurance_Type.StartsWith("PRIMARY")).Select(a => a.Insurance_Plan_ID).FirstOrDefault();//BugID:53793
                //PrimaryInsurance = objFillHumnaDTO.PatientInsuredBag.Where(a => a != null && a.Insurance_Type != null && a.Insurance_Type.StartsWith("PRIMARY")).Select(a => a.Insurance_Plan_ID).SingleOrDefault();
                if (PrimaryInsurance != 0)
                {
                    IList<ulong> PrimaryInsuranceList = new List<ulong>();
                    PrimaryInsuranceList.Add(PrimaryInsurance);
                    IList<ulong> LabIdBasedOnInsID = new List<ulong>();
                    LabIdBasedOnInsID = objLabInsurancePlanManager.GetLabIDBasedOnInsPlanID(PrimaryInsuranceList.ToArray<ulong>(), "LAB", sLegalOrg);
                    objDiagnosticDTO.PrimaryInsuranceList = LabIdBasedOnInsID;
                }
                //}
                //Commentted for performance
                //LabManager objLabManager = new LabManager();
                //objDiagnosticDTO.labList = objLabManager.GetAll();
                if (OrderSubmitID != 0)
                {
                    EAndMCodingManager objEAndMCodingManager = new EAndMCodingManager();
                    //  objDiagnosticDTO.procedureList = objEAndMCodingManager.GetPhysicianProcedure(Physician_Id, Procedure_Type, Lab_ID);
                    objDiagnosticDTO.procedureList = objEAndMCodingManager.GetPhysicianProcedure(objDiagnosticDTO.objOrdersSubmit.Physician_ID, Procedure_Type, Lab_ID, sLegalOrg);

                }
                //StaticLookupManager objStaticLookupManager = new StaticLookupManager();
                //objDiagnosticDTO.FieldLookUpList = objStaticLookupManager.getStaticLookupByFieldName(new string[] { "BILL TYPE", "SPECIMEN", "SPECIMEN UNIT", "CMG LAB NAME", "IN-HOUSE PROCEDURES LAB NAME" });
                //if(sFacilityName!=string.Empty)
                //{
                //    FacilityManager objFacilityManager = new FacilityManager();
                //    objDiagnosticDTO.facilityList = objFacilityManager.GetFacilityByFacilityname(sFacilityName);

                //}

                iMySession.Close();
            }
            return objDiagnosticDTO;
            //AssessmentManager objAssessmentManager = new AssessmentManager();
            //objDiagnosticDTO.AssessmentList = objAssessmentManager.GetAssessmentPerEncounterIDForOrders(ClientSession.EncounterId);
        }
        public OrderSpecimenDTO GetSpecimenDetails(IList<ulong> OrderID)
        {
            OrderSpecimenDTO objDTO = new OrderSpecimenDTO();
            ArrayList specimenIDList = new ArrayList();
            objDTO.ilstOrderLabDetailsDTO = GetOrdersUsingOrderID(OrderID);
            return objDTO;
        }

        public OrderSpecimenDTO LoadSpecimen(ulong OrderSubmitID, string OrderType)
        {

            IList<Human> InsurancedHumanDetails = new List<Human>();
            OrdersManager ObjOrdersMgr = new OrdersManager();
            OrderSpecimenDTO obj = new OrderSpecimenDTO();
            ArrayList specimenIDList = new ArrayList();
            OrdersManager objmnger = new OrdersManager();
            IList<OrdersAssessment> tempAss = new List<OrdersAssessment>();
            obj.ilstOrderLabDetailsDTO = objmnger.LoadOrdersForCollectSpecimen(OrderSubmitID, OrderType, out tempAss);
            obj.OrderAssList = tempAss;
            if (obj.ilstOrderLabDetailsDTO.Count > 0)
                obj.objHuman = ObjOrdersMgr.GetHumanById(obj.ilstOrderLabDetailsDTO[0].ObjOrder.Human_ID);
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetICDs.ForOrderSubmitIDs");
                query.SetString(0, OrderSubmitID.ToString());
                ArrayList ExamRoomList = new ArrayList(query.List());
                string ICDstemp = string.Empty;
                if (ExamRoomList.Count != 0)
                {
                    foreach (object tempObj in ExamRoomList)
                    {
                        ICDstemp += "," + tempObj.ToString();
                    }
                    obj.ICDs = ICDstemp.Substring(1);
                }
                IList<FillHumanDTO> ilstHumanDTO = new List<FillHumanDTO>();
                FillHumanDTO ObjTempFillHumanDTO = new FillHumanDTO();
                if (obj.objHuman.PatientInsuredBag != null)
                {
                    foreach (PatientInsuredPlan objPatInsuracePlan in obj.objHuman.PatientInsuredBag)
                    {
                        ObjTempFillHumanDTO = ObjOrdersMgr.GetHumanById(objPatInsuracePlan.Insured_Human_ID);
                        ObjTempFillHumanDTO.PatientInsuredBag = null;
                        ilstHumanDTO.Add(ObjTempFillHumanDTO);
                    }
                }
                obj.ilstGuarantor = ilstHumanDTO;

                iMySession.Close();
            }
            return obj;


        }

        public IList<Orders> GetListOfOrdersForMREstring(string OrdersSubmitID)
        {
            IList<Orders> ilstOrders = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select s.* from Orders s where s.Order_Submit_ID in (" + OrdersSubmitID + ")").AddEntity("s", typeof(Orders));
                ilstOrders = sql.List<Orders>();
                iMySession.Close();
            }
            return ilstOrders;
        }
        public IList<Orders> GetListOfOrdersForMRE(ulong ulOrderSubmitID)
        {
            IList<Orders> ilstOrders = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteriaByOrderSubmitID = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Order_Submit_ID", ulOrderSubmitID));
                ilstOrders = criteriaByOrderSubmitID.List<Orders>();
                iMySession.Close();
            }
            return ilstOrders;
        }
        public IList<Orders> GetOutstandingCPTForOrderGenerateProcess(ulong HumanID)
        {
            //IList<string> temp = new List<string>();
            //ISQLQuery sq = session.GetISession().CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"DIAGNOSTIC ORDER\" and w.current_process=\"ORDER_GENERATE\" and O.CMG_Encounter_ID=0;")//comment By ThiyagarajanM 
            //       .AddEntity("O", typeof(Orders));
            //       //.AddEntity("W", typeof(WFObject));
            //temp = sq.List<Orders>().Select(a => a.Lab_Procedure + "-" + a.Lab_Procedure_Description).ToList();
            //return temp;
            StaticLookupManager objstaticlookupManager = new StaticLookupManager();
            string CMGStaticLookupValue = objstaticlookupManager.getStaticLookupByFieldName("CMG LAB NAME").Select(a => a.Value).SingleOrDefault();
            IList<Orders> temp = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"DIAGNOSTIC ORDER\" and O.CMG_Encounter_ID=0 and O.Order_Code_Type='" + CMGStaticLookupValue + "' and w.current_process='RESULT_PROCESS';")
                       .AddEntity("O", typeof(Orders));
                //.AddEntity("W", typeof(WFObject));
                temp = sq.List<Orders>();//.Select(a => a.Lab_Procedure + "-" + a.Lab_Procedure_Description).ToList();

                iMySession.Close();
            }
            return temp;

        }
        public void UpdateCMGEncounterID(ulong CMGEncounterID, ulong HumanID, string TestsOrdered, string MacAddress, ISession Session)
        {

            string[] SplitChar = new string[] { "," };
            OrdersManager objOrdersManager = new OrdersManager();
            IList<Orders> tempOrdersSave = new List<Orders>();
            IList<Orders> UpdateOrders = new List<Orders>();
            //ISQLQuery sq = session.GetISession().CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"DIAGNOSTIC ORDER\" and w.current_process=\"ORDER_GENERATE\" and O.CMG_Encounter_ID=0;")//comment By ThiyagarajanM 
            //      .AddEntity("O", typeof(Orders));
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"DIAGNOSTIC ORDER\" and O.CMG_Encounter_ID=0;")
                     .AddEntity("O", typeof(Orders));
                IList<Orders> tempOrders = new List<Orders>();
                tempOrders = sq.List<Orders>();

                foreach (string str in TestsOrdered.Split(SplitChar, StringSplitOptions.RemoveEmptyEntries))
                {
                    //foreach (Orders obj in tempOrders.Where(a => a.Lab_Procedure == str.Split('-')[0]))
                    foreach (Orders obj in tempOrders.Where(a => a.Id == Convert.ToUInt32(str)))
                    {
                        obj.CMG_Encounter_ID = CMGEncounterID;
                        UpdateOrders.Add(obj);
                    }
                }

                iMySession.Close();
            }
            //objOrdersManager.SaveUpdateDeleteWithoutTransaction(ref tempOrdersSave, UpdateOrders, null, Session, MacAddress);
            GenerateXml XMLObj = new GenerateXml();
            objOrdersManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref tempOrdersSave, ref UpdateOrders, null, Session, MacAddress, false, true, 0, string.Empty, ref XMLObj);
        }
        //During Reshedule
        public void UpdateCMGEncounterID(ulong CMGEncounterID, ulong CMGEncounterIDNew, ulong HumanID, string TestsOrdered, string MacAddress, ISession Session)
        {

            string[] SplitChar = new string[] { "," };
            IList<Orders> tempOrders = new List<Orders>();
            IList<Orders> UpdateOrders = new List<Orders>();
            OrdersManager objOrdersManager = new OrdersManager();
            IList<Orders> tempOrdersSave = new List<Orders>();
            //ISQLQuery sq = session.GetISession().CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"DIAGNOSTIC ORDER\" and w.current_process=\"ORDER_GENERATE\" and O.CMG_Encounter_ID=0;")//comment By ThiyagarajanM 
            //      .AddEntity("O", typeof(Orders));
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"DIAGNOSTIC ORDER\" and O.CMG_Encounter_ID=0;")
                     .AddEntity("O", typeof(Orders));
                ISQLQuery sq1 = iMySession.CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"DIAGNOSTIC ORDER\" and O.CMG_Encounter_ID=" + CMGEncounterID + ";")
                     .AddEntity("O", typeof(Orders));

                //To updated newly added orders
                tempOrders = sq.List<Orders>();
                //To updated exisiting Old CMG encounter orders
                tempOrders = tempOrders.Concat(sq1.List<Orders>()).ToList<Orders>();


                foreach (string str in TestsOrdered.Split(SplitChar, StringSplitOptions.RemoveEmptyEntries))
                {
                    //foreach (Orders obj in tempOrders.Where(a => a.Lab_Procedure == str.Split('-')[0]))
                    foreach (Orders obj in tempOrders.Where(a => a.Id == Convert.ToUInt32(str)))
                    {
                        obj.CMG_Encounter_ID = CMGEncounterIDNew;
                        UpdateOrders.Add(obj);
                    }
                }


                iMySession.Close();
            }
            // objOrdersManager.SaveUpdateDeleteWithoutTransaction(ref tempOrdersSave, UpdateOrders, null, Session, MacAddress);
            GenerateXml XMLObj = new GenerateXml();
            objOrdersManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref tempOrdersSave, ref UpdateOrders, null, Session, MacAddress, false, true, 0, string.Empty, ref XMLObj);

        }


        public void UpdateCMGEncounterID(ulong CMGEncounterID, ulong HumanID, string TestsOrdered, string MacAddress)
        {


            string[] SplitChar = new string[] { " , " };
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT O.* FROM orders O,wf_object W where W.obj_system_id=O.Order_Submit_ID and O.Human_ID=" + HumanID.ToString() + " and w.obj_type=\"DIAGNOSTIC ORDER\" and w.current_process=\"RESULT_PROCESS\" and O.CMG_Encounter_ID=0;")
                      .AddEntity("O", typeof(Orders));
                IList<Orders> tempOrders = new List<Orders>();
                tempOrders = sq.List<Orders>();
                IList<Orders> UpdateOrders = new List<Orders>();
                foreach (string str in TestsOrdered.Split(SplitChar, StringSplitOptions.RemoveEmptyEntries))
                {
                    foreach (Orders obj in tempOrders.Where(a => a.Lab_Procedure == str.Split('-')[0]))
                    {
                        obj.CMG_Encounter_ID = CMGEncounterID;
                        UpdateOrders.Add(obj);
                    }
                }
                OrdersManager objOrdersManager = new OrdersManager();
                IList<Orders> tempOrdersSave = new List<Orders>();
                //objOrdersManager.SaveUpdateDeleteWithTransaction(ref tempOrdersSave, UpdateOrders, null, MacAddress);
                objOrdersManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref tempOrdersSave, ref UpdateOrders, null, MacAddress, false, false, 0, string.Empty);
                iMySession.Close();
            }
        }
        public void DeleteCMGEncounterIDFromOrders(IList<string> OrderIDList, string MacAddress)
        {

            OrdersManager objOrdersManager = new OrdersManager();
            IList<Orders> tempOrders = new List<Orders>();
            string temp = string.Empty;
            foreach (string str in OrderIDList)
            {
                if (temp.Trim() == string.Empty)
                    temp = "'" + str + "'";
                else
                    temp += ",'" + str + "'";
            }
            string OrderIds = temp;  //OrderIDList.Aggregate((c,a)=>c+"'"+a+"',").ToString();
            OrderIds = OrderIds.Substring(0, OrderIds.Length);
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT O.* FROM orders O where O.Order_ID in(" + OrderIds + ");")
                      .AddEntity("O", typeof(Orders));
                tempOrders = sq.List<Orders>();
                foreach (Orders obj in tempOrders)
                {
                    obj.CMG_Encounter_ID = 0;
                }
                IList<Orders> tempOrdersSave = new List<Orders>();
                //objOrdersManager.SaveUpdateDeleteWithTransaction(ref tempOrdersSave, tempOrders, null, MacAddress);
                objOrdersManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref tempOrdersSave, ref tempOrders, null, MacAddress, false, false, 0, string.Empty);
                iMySession.Close();
            }
        }


        public IList<Orders> GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId(string ObjectType, string CurrentProcess, ulong Human_id)
        {
            IList<Orders> lstcriteriaByOrderSubmitID = new List<Orders>();
            LabManager labMgr = new LabManager();
            IList<OrdersSubmit> lstorderSubmit = new List<OrdersSubmit>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql;
                //CAP-1153
                //sql = iMySession.CreateSQLQuery("select o.Order_ID,o.Order_Submit_ID,o.Lab_Procedure,o.Lab_Procedure_Description,os.Lab_ID,CAST(os.Specimen_Collection_Date_And_Time as CHAR(30)),o.Physician_ID,CAST(o.Created_Date_And_Time as CHAR(30)),o.Order_Code_Type,os.Encounter_ID from orders o,wf_object wf,orders_submit os where o.human_id=" + Human_id + " and o.Order_Submit_ID=wf.obj_system_id and wf.Current_process='" + CurrentProcess + "' and o.order_submit_id = os.order_submit_id and wf.obj_type ='" + ObjectType + "' ORDER BY o.Created_Date_And_Time ASC");//.AddEntity("o", typeof(Orders));//.AddEntity("wf", typeof(WFObject)).AddEntity("os", typeof(OrdersSubmit));
                //Cap - 1293
                //sql = iMySession.CreateSQLQuery("select o.Order_ID,o.Order_Submit_ID,o.Lab_Procedure,o.Lab_Procedure_Description,os.Lab_ID,CAST(os.Specimen_Collection_Date_And_Time as CHAR(30)),o.Physician_ID,CAST(o.Created_Date_And_Time as CHAR(30)),o.Order_Code_Type,os.Encounter_ID,l.Lab_Name from orders o,wf_object wf,orders_submit os, lab l where o.human_id=" + Human_id + " and o.Order_Submit_ID=wf.obj_system_id and wf.Current_process='" + CurrentProcess + "' and o.order_submit_id = os.order_submit_id and wf.obj_type ='" + ObjectType + "' and os.Lab_ID=l.Lab_ID ORDER BY o.Created_Date_And_Time ASC");//.AddEntity("o", typeof(Orders));//.AddEntity("wf", typeof(WFObject)).AddEntity("os", typeof(OrdersSubmit));
                sql = iMySession.CreateSQLQuery("select o.Order_ID,o.Order_Submit_ID,o.Lab_Procedure,o.Lab_Procedure_Description,os.Lab_ID,CAST(os.Specimen_Collection_Date_And_Time as CHAR(30)),o.Physician_ID,CAST(o.Created_Date_And_Time as CHAR(30)),o.Order_Code_Type,os.Encounter_ID,l.Lab_Name from orders o,wf_object wf,orders_submit os left join  lab l on  os.Lab_ID=l.Lab_ID where o.human_id=" + Human_id + " and o.Order_Submit_ID=wf.obj_system_id and wf.Current_process='" + CurrentProcess + "' and o.order_submit_id = os.order_submit_id and wf.obj_type ='" + ObjectType + "' ORDER BY o.Created_Date_And_Time ASC");//.AddEntity("o", typeof(Orders));//.AddEntity("wf", typeof(WFObject)).AddEntity("os", typeof(OrdersSubmit));
                ArrayList arrList = new ArrayList(sql.List());
                for (int i = 0; i < arrList.Count; i++)
                {
                    Orders lstCurrentRecord = new Orders();
                    object[] oj = (object[])arrList[i];
                    lstCurrentRecord.Id = Convert.ToUInt64(oj[0]);
                    lstCurrentRecord.Order_Submit_ID = Convert.ToUInt64(oj[1]);
                    lstCurrentRecord.Lab_Procedure = Convert.ToString(oj[2]);
                    lstCurrentRecord.Lab_Procedure_Description = Convert.ToString(oj[3]);
                    lstCurrentRecord.Internal_Property_LabID = Convert.ToUInt64(oj[4]);
                    lstCurrentRecord.Internal_Property_Spec_Collection_Date = Convert.ToDateTime(oj[5]);
                    lstCurrentRecord.Physician_ID = Convert.ToUInt64(oj[6]);
                    lstCurrentRecord.Created_Date_And_Time = Convert.ToDateTime(oj[7]);
                    lstCurrentRecord.Order_Code_Type = Convert.ToString(oj[8]);
                    lstCurrentRecord.Encounter_ID = Convert.ToUInt64(oj[9]);
                    //CAP-1153
                    lstCurrentRecord.Internal_Property_Lab_Name= Convert.ToString(oj[10]);
                    lstcriteriaByOrderSubmitID.Add(lstCurrentRecord);
                }

                // foreach (IList<Object> l in sql.List())
                //{
                //    Orders lstCurrentRecord = (Orders)l[0];
                //    lstCurrentRecord.Internal_Property_LabID = ((OrdersSubmit)l[2]).Lab_ID;
                //    lstCurrentRecord.Internal_Property_Spec_Collection_Date = ((OrdersSubmit)l[2]).Specimen_Collection_Date_And_Time;
                //    lstcriteriaByOrderSubmitID.Add(lstCurrentRecord);
                //}

                iMySession.Close();
            }
            return lstcriteriaByOrderSubmitID;
        }

        //public IList<Orders> GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId(string ObjectType, string CurrentProcess, ulong Human_id)
        //{
        //    IList<Orders> lstcriteriaByOrderSubmitID = new List<Orders>();
        //    LabManager labMgr = new LabManager();
        //    IList<OrdersSubmit> lstorderSubmit = new List<OrdersSubmit>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        //ulong[] arr_Object_System_id;
        //        //lstorderSubmit = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Human_ID", Human_id)).List<OrdersSubmit>();
        //        //ulong[] arr_Object_System_id = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", ObjectType)).Add(Expression.Eq("Current_Process", CurrentProcess)).Add(Expression.In("Obj_System_Id", lstorderSubmit.Select(a => a.Id).ToArray())).List<WFObject>().Where(D => D != null).Select(R => R.Obj_System_Id).ToArray();
        //        //lstcriteriaByOrderSubmitID = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.In("Order_Submit_ID", arr_Object_System_id)).Add(Expression.Eq("Human_ID", Human_id)).List<Orders>().OrderByDescending(a => a.Lab_Procedure_Description).ToList<Orders>();
        //        //foreach (Orders orders in lstcriteriaByOrderSubmitID)
        //        //{
        //        //    orders.Internal_Property_LabID = lstorderSubmit.FirstOrDefault(a => a.Id == orders.Order_Submit_ID).Lab_ID;
        //        //    orders.Internal_Property_Spec_Collection_Date = lstorderSubmit.FirstOrDefault(a => a.Id == orders.Order_Submit_ID).Specimen_Collection_Date_And_Time;
        //        //}

        //        ISQLQuery sql;

        //        sql = iMySession.CreateSQLQuery("select {o.*},{wf.*},{os.*} from orders o,wf_object wf,orders_submit os where o.Order_Submit_ID=wf.obj_system_id and wf.Current_process='" + CurrentProcess + "' and o.order_submit_id = os.order_submit_id and wf.obj_type ='" + ObjectType + "'and o.human_id=" + Human_id + " ORDER BY o.Created_Date_And_Time ASC").AddEntity("o", typeof(Orders)).AddEntity("wf", typeof(WFObject)).AddEntity("os", typeof(OrdersSubmit));


        //        //foreach (IList<Object> l in sql.List())
        //        //{
        //        //    if (((WFObject)l[1]).Current_Process == CurrentProcess)
        //        //    {
        //        //        Orders lstCurrentRecord = (Orders)l[0];
        //        //        lstCurrentRecord.Internal_Property_LabID = ((OrdersSubmit)l[2]).Lab_ID;
        //        //        lstCurrentRecord.Internal_Property_Spec_Collection_Date = ((OrdersSubmit)l[2]).Specimen_Collection_Date_And_Time;
        //        //        lstcriteriaByOrderSubmitID.Add(lstCurrentRecord);
        //        //    }

        //        //}
        //        //Removed Current_process filtering.//BugID:46054
        //        foreach (IList<Object> l in sql.List())
        //        {
        //            Orders lstCurrentRecord = (Orders)l[0];
        //            lstCurrentRecord.Internal_Property_LabID = ((OrdersSubmit)l[2]).Lab_ID;
        //            lstCurrentRecord.Internal_Property_Spec_Collection_Date = ((OrdersSubmit)l[2]).Specimen_Collection_Date_And_Time;
        //            lstcriteriaByOrderSubmitID.Add(lstCurrentRecord);
        //        }

        //        iMySession.Close();
        //    }
        //    return lstcriteriaByOrderSubmitID;
        //}


        //Added by Saravanakumar 
        public IList<string> GetOrdersforCMGAncillary(string ObjectType, string CurrentProcess, ulong Human_id, ulong ulLabID)//, string sFacility)
        {
            IList<string> lstcriteriaByOrderSubmitID = new List<string>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql;

                sql = iMySession.CreateSQLQuery("select os.order_submit_id from orders_submit os, encounter e,wf_object wf where os.Order_Submit_ID=wf.obj_system_id and wf.Current_process='" + CurrentProcess + "' and os.encounter_id= e.encounter_id and wf.obj_type ='" + ObjectType + "'and os.lab_id='" + ulLabID + "' and e.order_submit_id<>0 and os.human_id=" + Human_id);

                ArrayList arr = new ArrayList(sql.List());
                if (arr.Count > 0)
                {
                    for (int i = 0; i < arr.Count; i++)
                    {
                        lstcriteriaByOrderSubmitID.Add(arr[i].ToString());
                    }
                }

                iMySession.Close();
            }
            return lstcriteriaByOrderSubmitID;
        }
        //End
        public string GetFaxOrders(string ordersubmitid)
        {
            string result = string.Empty;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql;
                sql = iMySession.CreateSQLQuery("select os.Lab_Name,concat (h.First_Name,' ',h.Last_Name) as human_name from orders_submit os left join human h on h.human_id = os.human_id where os.order_submit_id ='" + ordersubmitid + "'");
                ArrayList arr = new ArrayList(sql.List());
                if (arr.Count > 0)
                {
                    for (int i = 0; i < arr.Count; i++)
                    {
                        object[] obj = (object[])arr[i];
                        result = obj[0].ToString() + "|" + obj[1].ToString();
                    }
                }
            }

            return result;
        }


        public IDictionary<string, string> GetOrdersforCMGAncillaryParentOrders(IList<string> sordersubmitID)
        {
            IDictionary<string, string> lstcriteriaByOrderSubmitIDnew = new Dictionary<string, string>();
            IList<string> lstcriteriaByOrderSubmitID = new List<string>();

            string svalue = string.Join(",", sordersubmitID.Cast<string>().ToArray());
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql;

                if (sordersubmitID.Count > 0)
                {
                    sql = iMySession.CreateSQLQuery("select o.order_submit_id,o.encounter_id as encid,(select e.order_submit_id from encounter e where e. encounter_id =encid) as ordid ,(select o2.physician_id as phyid from orders_submit o2 where o2.order_submit_id=ordid) as phyid from orders_submit o where o.order_submit_id in (" + svalue + ")");

                    ArrayList arr = new ArrayList(sql.List());
                    if (arr.Count > 0)
                    {
                        for (int i = 0; i < arr.Count; i++)
                        {
                            object[] obj = (object[])arr[i];
                            lstcriteriaByOrderSubmitIDnew.Add(obj[0].ToString(), obj[3].ToString());
                        }
                    }
                }
                iMySession.Close();
            }
            return lstcriteriaByOrderSubmitIDnew;
        }


        public IList<Orders> GetOrderId(ulong OrderSubmitID)
        {
            Orders objOrders = new Orders();
            IList<Orders> ilstOrderID = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select o.* from Orders o where o.order_submit_id='" + OrderSubmitID + "'").AddEntity("o", typeof(Orders));
                ilstOrderID = sql.List<Orders>();
                iMySession.Close();
            }
            if (ilstOrderID.Count > 0)
                return ilstOrderID;
            else
                return ilstOrderID;
        }
        public Dictionary<string, string> GetLabDashboard(ulong PhyId, DateTime dtFromDate, DateTime dtToDate)
        {

            Dictionary<string, string> htResultSet = new Dictionary<string, string>();

            htResultSet.Add("Total Orders Sent for LabCorp", "");

            IQuery query = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                query = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsYetoReceive.With.DateRange");
                query.SetString(0, "Total No. Of Results Yet to be Received for LabCorp");
                query.SetString(1, "1");
                query.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                query.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                query.SetString(4, PhyId.ToString());
                ArrayList arr = new ArrayList(query.List());

                if (arr.Count > 0)
                {
                    object[] ojQuery = (object[])arr[0];
                    htResultSet.Add(ojQuery[0].ToString(), ojQuery[1].ToString());
                }

                IQuery queryLabCorpReviewedbyPhy = null;
                queryLabCorpReviewedbyPhy = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsReviewedbyPhy.With.DateRange");
                queryLabCorpReviewedbyPhy.SetString(0, "Total No. Of Results Received and Reviewed by Physician for LabCorp");
                queryLabCorpReviewedbyPhy.SetString(1, "1");
                queryLabCorpReviewedbyPhy.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                queryLabCorpReviewedbyPhy.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                queryLabCorpReviewedbyPhy.SetString(4, PhyId.ToString());

                ArrayList arrLabCorpReviewedbyPhy = new ArrayList(queryLabCorpReviewedbyPhy.List());

                if (arrLabCorpReviewedbyPhy.Count > 0)
                {
                    object[] ojLabCorpReviewed = (object[])arrLabCorpReviewedbyPhy[0];
                    htResultSet.Add(ojLabCorpReviewed[0].ToString(), ojLabCorpReviewed[1].ToString());
                }

                IQuery queryLabCorpWaitingforPhy = null;
                queryLabCorpWaitingforPhy = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsWaitingforPhy.With.DateRange");
                queryLabCorpWaitingforPhy.SetString(0, "Total No. Of Results Received and Waiting for Physician Review for LabCorp");
                queryLabCorpWaitingforPhy.SetString(1, "1");
                queryLabCorpWaitingforPhy.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                queryLabCorpWaitingforPhy.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                queryLabCorpWaitingforPhy.SetString(4, PhyId.ToString());
                ArrayList arrLabCorpWaitingforPhy = new ArrayList(queryLabCorpWaitingforPhy.List());

                if (arrLabCorpWaitingforPhy.Count > 0)
                {
                    object[] ojLabCorpWaiting = (object[])arrLabCorpWaitingforPhy[0];
                    htResultSet.Add(ojLabCorpWaiting[0].ToString(), ojLabCorpWaiting[1].ToString());

                }

                IQuery queryLabCorpWaitingforMA = null;
                queryLabCorpWaitingforMA = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsWaitingforMAReview.With.DateRange");
                queryLabCorpWaitingforMA.SetString(0, "Total No. Of Results Received and Waiting for MA Review for LabCorp");
                queryLabCorpWaitingforMA.SetString(1, "1");
                queryLabCorpWaitingforMA.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                queryLabCorpWaitingforMA.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                queryLabCorpWaitingforMA.SetString(4, PhyId.ToString());

                ArrayList arrLabCorpWaitingforMA = new ArrayList(queryLabCorpWaitingforMA.List());

                if (arrLabCorpWaitingforMA.Count > 0)
                {
                    object[] ojLabCorpWaitingForMA = (object[])arrLabCorpWaitingforMA[0];
                    htResultSet.Add(ojLabCorpWaitingForMA[0].ToString(), ojLabCorpWaitingForMA[1].ToString());

                }
                int TotalOrdersSentLabcorp = Convert.ToInt32(((object[])arr[0])[1].ToString()) + Convert.ToInt32(((object[])arrLabCorpReviewedbyPhy[0])[1]) + Convert.ToInt32(((object[])arrLabCorpWaitingforMA[0])[1]) + Convert.ToInt32(((object[])arrLabCorpWaitingforPhy[0])[1]);
                htResultSet["Total Orders Sent for LabCorp"] = TotalOrdersSentLabcorp.ToString();

                htResultSet.Add("Total Orders Sent for Quest", "");

                IQuery queryQuest = null;
                queryQuest = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsYetoReceive.With.DateRange");
                queryQuest.SetString(0, "Total No. Of Results Yet to be Received for Quest");
                queryQuest.SetString(1, "2");
                queryQuest.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                queryQuest.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                queryQuest.SetString(4, PhyId.ToString());

                ArrayList arrQuest = new ArrayList(queryQuest.List());

                if (arrQuest.Count > 0)
                {
                    object[] ojqueryQuest = (object[])arrQuest[0];
                    htResultSet.Add(ojqueryQuest[0].ToString(), ojqueryQuest[1].ToString());

                }

                IQuery queryQuestReviewedbyPhy = null;

                queryQuestReviewedbyPhy = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsReviewedbyPhy.With.DateRange");
                queryQuestReviewedbyPhy.SetString(0, "Total No. Of Results Received and Reviewed by Physician for Quest");
                queryQuestReviewedbyPhy.SetString(1, "2");
                queryQuestReviewedbyPhy.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                queryQuestReviewedbyPhy.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                queryQuestReviewedbyPhy.SetString(4, PhyId.ToString());
                ArrayList arrQuestReviewedbyPhy = new ArrayList(queryQuestReviewedbyPhy.List());

                if (arrQuestReviewedbyPhy.Count > 0)
                {
                    object[] ojQuestReviewedbyPhy = (object[])arrQuestReviewedbyPhy[0];
                    htResultSet.Add(ojQuestReviewedbyPhy[0].ToString(), ojQuestReviewedbyPhy[1].ToString());

                }

                IQuery queryQuestWaitingforPhy = null;

                queryQuestWaitingforPhy = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsWaitingforPhy.With.DateRange");
                queryQuestWaitingforPhy.SetString(0, "Total No. Of Results Received and Waiting for Physician Review for Quest");
                queryQuestWaitingforPhy.SetString(1, "2");
                queryQuestWaitingforPhy.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                queryQuestWaitingforPhy.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                queryQuestWaitingforPhy.SetString(4, PhyId.ToString());

                ArrayList arrQuestWaitingforPhy = new ArrayList(queryQuestWaitingforPhy.List());

                if (arrQuestWaitingforPhy.Count > 0)
                {
                    object[] ojQuestWaitingforPhy = (object[])arrQuestWaitingforPhy[0];
                    htResultSet.Add(ojQuestWaitingforPhy[0].ToString(), ojQuestWaitingforPhy[1].ToString());

                }

                IQuery queryQuestWaitingforMA = null;

                queryQuestWaitingforMA = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsWaitingforMAReview.With.DateRange");
                queryQuestWaitingforMA.SetString(0, "Total No. Of Results Received and Waiting for MA Review for Quest");
                queryQuestWaitingforMA.SetString(1, "2");
                queryQuestWaitingforMA.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                queryQuestWaitingforMA.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                queryQuestWaitingforMA.SetString(4, PhyId.ToString());


                ArrayList arrQuestWaitingforMA = new ArrayList(queryQuestWaitingforMA.List());

                if (arrQuestWaitingforMA.Count > 0)
                {
                    object[] ojQuestWaitingforMA = (object[])arrQuestWaitingforMA[0];
                    htResultSet.Add(ojQuestWaitingforMA[0].ToString(), ojQuestWaitingforMA[1].ToString());


                }
                int TotalOrdersSentQuest = Convert.ToInt32(((object[])arrQuest[0])[1]) + Convert.ToInt32(((object[])arrQuestWaitingforPhy[0])[1]) + Convert.ToInt32(((object[])arrQuestWaitingforMA[0])[1]) + Convert.ToInt32(((object[])arrQuestReviewedbyPhy[0])[1]);
                htResultSet["Total Orders Sent for Quest"] = TotalOrdersSentQuest.ToString();
                IQuery queryManuallyLabCorp = null;

                ISQLQuery sql1 = iMySession.CreateSQLQuery("Select u.* from physician_library u where u.Physician_Library_ID='" + PhyId + "'").AddEntity("u", typeof(PhysicianLibrary));
                string LastName = sql1.List<PhysicianLibrary>()[0].PhyLastName;
                queryManuallyLabCorp = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsReceivedManually.With.DateRange");
                queryManuallyLabCorp.SetString(0, "Total No. Of Lab Results received Manually for LabCorp");
                queryManuallyLabCorp.SetString(1, "1");
                queryManuallyLabCorp.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                queryManuallyLabCorp.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                queryManuallyLabCorp.SetString(4, LastName);

                ArrayList arrManuallyLabCorp = new ArrayList(queryManuallyLabCorp.List());

                if (arrManuallyLabCorp.Count > 0)
                {
                    object[] ojManuallyLabCorp = (object[])arrManuallyLabCorp[0];
                    htResultSet.Add(ojManuallyLabCorp[0].ToString(), ojManuallyLabCorp[1].ToString());

                }


                IQuery QueryErrorLog = null;
                QueryErrorLog = iMySession.GetNamedQuery("Get.Lab.Dashboard.ErrorLogReasonCode.With.DateRange");
                QueryErrorLog.SetString(0, "1");
                QueryErrorLog.SetString(1, "1");
                QueryErrorLog.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                QueryErrorLog.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                QueryErrorLog.SetString(4, LastName);

                ArrayList arrErrorLogLabcorp = new ArrayList(QueryErrorLog.List());

                if (arrErrorLogLabcorp.Count > 0)
                {
                    for (int i = 0; i < arrErrorLogLabcorp.Count; i++)
                    {
                        object[] ojErrorLogLabcorp = (object[])arrErrorLogLabcorp[i];
                        if (!htResultSet.ContainsKey(ojErrorLogLabcorp[0].ToString() + " - " + ojErrorLogLabcorp[1].ToString()))
                            htResultSet.Add(ojErrorLogLabcorp[0].ToString() + " - " + ojErrorLogLabcorp[1].ToString(), ojErrorLogLabcorp[2].ToString());
                    }
                }

                IQuery queryManuallyQuest = null;


                queryManuallyQuest = iMySession.GetNamedQuery("Get.Lab.Dashboard.ForResultsReceivedManually.With.DateRange");
                queryManuallyQuest.SetString(0, "Total No. Of Lab Results received Manually for Quest");
                queryManuallyQuest.SetString(1, "2");
                queryManuallyQuest.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                queryManuallyQuest.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                queryManuallyQuest.SetString(4, LastName);

                ArrayList arrManuallyQuest = new ArrayList(queryManuallyQuest.List());

                if (arrManuallyQuest.Count > 0)
                {
                    object[] ojManuallyQuest = (object[])arrManuallyQuest[0];
                    htResultSet.Add(ojManuallyQuest[0].ToString(), ojManuallyQuest[1].ToString());
                }

                IQuery QueryErrorLogQuest = null;


                QueryErrorLogQuest = iMySession.GetNamedQuery("Get.Lab.Dashboard.ErrorLogReasonCode.With.DateRange");
                QueryErrorLogQuest.SetString(0, "2");
                QueryErrorLogQuest.SetString(1, "2");
                QueryErrorLogQuest.SetString(2, dtFromDate.ToString("yyyy-MM-dd"));
                QueryErrorLogQuest.SetString(3, dtToDate.AddDays(1).ToString("yyyy-MM-dd"));
                QueryErrorLogQuest.SetString(4, LastName);



                ArrayList arrErrorLogQuest = new ArrayList(QueryErrorLogQuest.List());

                if (arrErrorLogQuest.Count > 0)
                {


                    for (int i = 0; i < arrErrorLogQuest.Count; i++)
                    {
                        //object[] ojErrorLogQuest = (object[])arrErrorLogQuest[0]; //code comment by balaji.TJ 2015-12-27
                        object[] ojErrorLogQuest = (object[])arrErrorLogQuest[i]; //code added by balaji.TJ 2015-12-27
                        if (!htResultSet.ContainsKey(ojErrorLogQuest[0].ToString() + " - " + ojErrorLogQuest[1].ToString()))
                            htResultSet.Add(ojErrorLogQuest[0].ToString() + " - " + ojErrorLogQuest[1].ToString(), ojErrorLogQuest[2].ToString());

                    }
                }
                iMySession.Close();
            }
            return htResultSet;
        }

        public IDictionary<int, string> GetOrder(string LabProcedureCode, DateTime LabProcedureTestDate, string DaysWeeksMonths, ulong EncounterId, string ProcID)
        {
            string OrderType = "DIAGNOSTIC ORDER";
            IList<string> ProcdedureList = new List<string>();
            int iResult = 0;
            if (LabProcedureCode != string.Empty)
            {
                string[] sProc = LabProcedureCode.Split(',');
                foreach (string sval in sProc)
                {
                    ProcdedureList.Add(sval.ToString());
                }
            }

            IDictionary<int, string> ilstDuplicateProc = new Dictionary<int, string>();
            IList<Orders> ilstOrder = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("FindDuplicateOrder");
                query.SetString(0, EncounterId.ToString());
                query.SetString(1, OrderType);
                ArrayList arr = new ArrayList(query.List());
                if (arr.Count > 0)
                {
                    for (int i = 0; i < arr.Count; i++)
                    {
                        Orders objOrders = new Orders();
                        object[] obj = (object[])arr[i];
                        objOrders.Id = Convert.ToUInt32(obj[0].ToString());
                        objOrders.Encounter_ID = Convert.ToUInt32(obj[1].ToString());
                        objOrders.Human_ID = Convert.ToUInt32(obj[2].ToString());
                        objOrders.Physician_ID = Convert.ToUInt32(obj[3].ToString());
                        objOrders.Lab_Procedure = obj[6].ToString();
                        objOrders.Lab_Procedure_Description = obj[19].ToString();
                        objOrders.Order_Code_Type = obj[25].ToString();
                        objOrders.Order_Submit_ID = Convert.ToUInt32(obj[36].ToString());
                        if (obj[8].ToString().Trim() != "")
                            objOrders.Created_Date_And_Time = Convert.ToDateTime(obj[8].ToString());
                        else
                            objOrders.Created_Date_And_Time = DateTime.MinValue;
                        //  objOrders.Created_By = obj[40].ToString() + "-" + obj[41].ToString() + "-" + obj[42].ToString();BugId:39072
                        objOrders.Created_By = obj[14].ToString();
                        //objOrders.Order_Submit_ID =Convert.ToUInt32(obj[7].ToString());
                        ulong procedureId = 0;
                        ulong.TryParse(ProcID, out procedureId);
                        if (objOrders.Order_Submit_ID != procedureId)
                            ilstOrder.Add(objOrders);
                    }



                    string[] sDuplicateProcedure = LabProcedureCode.Split(',');

                    for (int i = 0; i < sDuplicateProcedure.Count(); i++)
                    {
                        var sProcedure = (from a in ilstOrder where (a.Lab_Procedure == sDuplicateProcedure[i] && (a.Created_Date_And_Time.ToString("dd-MMM-yyyy") == LabProcedureTestDate.ToString("dd-MMM-yyyy") || a.Created_By == DaysWeeksMonths)) select new { a.Lab_Procedure, a.Lab_Procedure_Description }).Distinct().ToList();
                        if (sProcedure.Count > 0)
                        {
                            ilstDuplicateProc.Add(iResult, sProcedure[0].Lab_Procedure + ": " + sProcedure[0].Lab_Procedure_Description.ToString().Replace('-', ' '));
                            iResult++;
                        }
                    }
                }
                iMySession.Close();
            }
            return ilstDuplicateProc;
        }


        public IList<string> GetOrdersForRecommendedMaterials(ulong ulEncounterId, ulong ulHumanId)
        {


            IList<string> result = new List<string>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Encounter_ID", ulEncounterId)).Add(Expression.Eq("Human_ID", ulHumanId));
                IList<Orders> orderList = criteria.List<Orders>();

                foreach (var orderItem in orderList)
                {
                    ICriteria criteriaOrderCodeLibrary = iMySession.CreateCriteria(typeof(OrderCodeLibrary)).Add(Expression.Eq("Order_Code", orderItem.Lab_Procedure));
                    IList<OrderCodeLibrary> OrderCodeLibraryList = criteriaOrderCodeLibrary.List<OrderCodeLibrary>();

                    if (OrderCodeLibraryList.Count > 0)
                    {
                        foreach (var orderCodeLibraryItem in OrderCodeLibraryList)
                        {
                            string loinc = string.Empty;

                            ICriteria criteriaProcedureLoincMap = iMySession.CreateCriteria(typeof(ProcedureLoincMap)).Add(Expression.Eq("Procedure_Code", orderCodeLibraryItem.CPT_Code));
                            IList<ProcedureLoincMap> procedureLoincMapList = criteriaProcedureLoincMap.List<ProcedureLoincMap>();

                            if (procedureLoincMapList.Count > 0)
                            {
                                result.Add(orderItem.Lab_Procedure_Description + "|" + procedureLoincMapList[0].Loinc_Num);
                                break;
                            }
                            else
                            {
                                result.Add(orderItem.Lab_Procedure_Description + "|" + string.Empty);
                                break;
                            }
                        }
                    }
                    else
                        result.Add(orderItem.Lab_Procedure_Description + "|" + string.Empty);
                }
                iMySession.Close();
            }
            return result;
        }

        public IList<string> GetOrdersForPrint(string orderid)
        {
            IList<OrderCodeLibrary> OrderCodeLibraryList = new List<OrderCodeLibrary>();
            IList<string> result = new List<string>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT O.* FROM order_code_library  O where O.Order_Code in('" + orderid + "') or O.CPT_Code in ('" + orderid + "');").AddEntity("O", typeof(OrderCodeLibrary));
                OrderCodeLibraryList = sq.List<OrderCodeLibrary>();
                if (OrderCodeLibraryList.Count > 0)
                {
                    foreach (var orderCodeLibraryItem in OrderCodeLibraryList)
                    {
                        string loinc = string.Empty;

                        ICriteria criteriaProcedureLoincMap = iMySession.CreateCriteria(typeof(ProcedureLoincMap)).Add(Expression.Eq("Procedure_Code", orderCodeLibraryItem.CPT_Code));
                        IList<ProcedureLoincMap> procedureLoincMapList = criteriaProcedureLoincMap.List<ProcedureLoincMap>();

                        if (procedureLoincMapList.Count > 0)
                        {
                            result.Add(procedureLoincMapList[0].Loinc_Num);

                        }
                        else
                        {
                            result.Add(string.Empty);
                        }

                    }
                }
                else
                {
                    result.Add(string.Empty);
                }

                iMySession.Close();
            }
            return result;
        }
        public IList<Orders> GetOrdersByOrderSubmitID(List<int> OrderSubmitID)
        {
            IList<Orders> ilstOrders = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria OrderCri = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.In("Order_Submit_ID", OrderSubmitID));
                ilstOrders = OrderCri.List<Orders>();
                iMySession.Close();
            }
            return ilstOrders;
        }
        public string GetOrdersByOrderSubmitID(int OrderSubmitID)
        {
            string sOrders = string.Empty;
            ArrayList orderList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("GetOrderbyOrderSubmitID");
                query2.SetString(0, OrderSubmitID.ToString());
                orderList = new ArrayList(query2.List());
                if (orderList != null && orderList.Count > 0)
                {
                    sOrders = orderList[0].ToString();
                }
            }
            return sOrders;
        }

        public FillHumanDTO GetHumanByIdForCheckout(ulong HumanId, ulong ulEncounterID)
        {
            FillHumanDTO objFillHuman = new FillHumanDTO();

            #region "Code Modified by Balaji.TJ 2023-01-10"
            IList<string> ilstHuman = new List<string>();
            ilstHuman.Add("HumanList");

            IList<object> ilstHumanBlobFinal = new List<object>();
            ilstHumanBlobFinal = ReadBlob(HumanId, ilstHuman);
            if (ilstHumanBlobFinal != null && ilstHumanBlobFinal.Count > 0)
            {
                if (ilstHumanBlobFinal[0] != null)
                {
                    for (int i = 0; i < ((List<object>)ilstHumanBlobFinal[0]).Count; i++)
                    {
                        objFillHuman.Human_ID = Convert.ToUInt64(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Id);
                        objFillHuman.Birth_Date = Convert.ToDateTime(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Birth_Date);
                        objFillHuman.City = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).City);
                        objFillHuman.First_Name = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).First_Name);
                        objFillHuman.Last_Name = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Last_Name);
                        objFillHuman.MI = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).MI);
                        objFillHuman.Prefix = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Prefix);
                        objFillHuman.Sex = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Sex);
                        objFillHuman.SSN = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).SSN);
                        objFillHuman.State = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).State);
                        objFillHuman.Street_Address1 = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Street_Address1);
                        objFillHuman.Suffix = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Suffix);
                        objFillHuman.ZipCode = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).ZipCode);
                        objFillHuman.Street_Address2 = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Street_Address2);
                        objFillHuman.Medical_Record_Number = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Medical_Record_Number);
                        objFillHuman.Home_Phone_No = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Home_Phone_No);
                        objFillHuman.Guarantor_First_Name = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_First_Name);
                        objFillHuman.Guarantor_Last_Name = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_Last_Name);
                        objFillHuman.Guarantor_MI = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_MI);
                        objFillHuman.Guarantor_State = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_State);
                        objFillHuman.Guarantor_Street_Address1 = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_Street_Address1);
                        objFillHuman.Guarantor_Street_Address2 = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_Street_Address2);
                        objFillHuman.Guarantor_Zip_Code = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_Zip_Code);
                        objFillHuman.Guarantor_City = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_City);
                        objFillHuman.Race = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Race);
                        objFillHuman.Ethnicity = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Ethnicity);
                        objFillHuman.Ethnicity_No = Convert.ToInt16(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Ethnicity_No);
                        objFillHuman.Guarantor_Home_Phone_Number = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_Home_Phone_Number);
                        objFillHuman.Guarantor_Relationship_No = Convert.ToInt16(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_Relationship_No);
                        objFillHuman.Guarantor_Relationship = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Guarantor_Relationship);
                        objFillHuman.Employer_Name = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Employer_Name);
                        objFillHuman.cell_phone_no = Convert.ToString(((Human)((List<object>)ilstHumanBlobFinal[0])[i]).Cell_Phone_Number);

                    }
                }
            }

            #endregion

            #region "Code comment by Balaji.TJ 2023-01-10"


            //string FileName = "Human" + "_" + HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    // itemDoc.Load(strXmlFilePath);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
            //        if (xmlhumanList != null && xmlhumanList.Count > 0)
            //        {
            //            objFillHuman.Human_ID = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
            //            objFillHuman.Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Birth_Date").Value);
            //            objFillHuman.City = xmlhumanList[0].Attributes.GetNamedItem("City").Value;
            //            objFillHuman.First_Name = xmlhumanList[0].Attributes.GetNamedItem("First_Name").Value;
            //            objFillHuman.Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Last_Name").Value;
            //            objFillHuman.MI = xmlhumanList[0].Attributes.GetNamedItem("MI").Value;
            //            objFillHuman.Prefix = xmlhumanList[0].Attributes.GetNamedItem("Prefix").Value;
            //            objFillHuman.Sex = xmlhumanList[0].Attributes.GetNamedItem("Sex").Value;
            //            objFillHuman.SSN = xmlhumanList[0].Attributes.GetNamedItem("SSN").Value;
            //            objFillHuman.State = xmlhumanList[0].Attributes.GetNamedItem("State").Value;
            //            objFillHuman.Street_Address1 = xmlhumanList[0].Attributes.GetNamedItem("Street_Address1").Value;
            //            objFillHuman.Suffix = xmlhumanList[0].Attributes.GetNamedItem("Suffix").Value;
            //            objFillHuman.ZipCode = xmlhumanList[0].Attributes.GetNamedItem("ZipCode").Value;
            //            objFillHuman.Street_Address2 = xmlhumanList[0].Attributes.GetNamedItem("Street_Address2").Value;
            //            objFillHuman.Medical_Record_Number = xmlhumanList[0].Attributes.GetNamedItem("Medical_Record_Number").Value;
            //            objFillHuman.Home_Phone_No = xmlhumanList[0].Attributes.GetNamedItem("Home_Phone_No").Value;
            //            objFillHuman.Guarantor_First_Name = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_First_Name").Value;
            //            objFillHuman.Guarantor_Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Last_Name").Value;
            //            objFillHuman.Guarantor_MI = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_MI").Value;
            //            objFillHuman.Guarantor_State = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_State").Value;
            //            objFillHuman.Guarantor_Street_Address1 = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Street_Address1").Value;
            //            objFillHuman.Guarantor_Street_Address2 = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Street_Address2").Value;
            //            objFillHuman.Guarantor_Zip_Code = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Zip_Code").Value;
            //            objFillHuman.Guarantor_City = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_City").Value;
            //            objFillHuman.Race = xmlhumanList[0].Attributes.GetNamedItem("Race").Value;
            //            objFillHuman.Ethnicity = xmlhumanList[0].Attributes.GetNamedItem("Ethnicity").Value;
            //            objFillHuman.Ethnicity_No = Convert.ToInt16(xmlhumanList[0].Attributes.GetNamedItem("Ethnicity_No").Value);
            //            objFillHuman.Guarantor_Home_Phone_Number = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Home_Phone_Number").Value;
            //            objFillHuman.Guarantor_Relationship_No = Convert.ToInt16(xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Relationship_No").Value);
            //            objFillHuman.Guarantor_Relationship = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Relationship").Value;
            //            objFillHuman.Employer_Name = xmlhumanList[0].Attributes.GetNamedItem("Employer_Name").Value;
            //            objFillHuman.cell_phone_no = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}

            #endregion

            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            //GitLab #3811
            //IQuery query1 = iMySession.GetNamedQuery("Get.Checkout.PatientPane");
            //query1.SetString(0, HumanId.ToString());
            IQuery query1 = iMySession.GetNamedQuery("Get.Checkout.PatientPaneWithEncounter");
            query1.SetString(0, HumanId.ToString());
            query1.SetString(1, ulEncounterID.ToString());

            ArrayList PatientList = null;
            PatientList = new ArrayList(query1.List());
            //FillHumanDTO p = null;

            if (PatientList.Count > 0)
            {
                for (int i = 0; i < PatientList.Count; i++)
                {
                    object[] obj1 = (object[])PatientList[i];
                    if (obj1[0] != null) objFillHuman.Insurance_Type = obj1[0].ToString();
                    if (obj1[1] != null) objFillHuman.Ins_Plan_Name = obj1[1].ToString();
                    if (obj1[2] != null) objFillHuman.Assigned_Physician = obj1[2].ToString();
                    if (obj1[3] != null) objFillHuman.CarrierName = obj1[3].ToString();

                }
            }
            return objFillHuman;
        }

        public IList<string> GetOrderByHuman(ulong ulHumanID, string sFacility, ulong ulLabID)
        {
            IList<string> ilstOrders = new List<string>();
            ArrayList orderList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query2 = iMySession.GetNamedQuery("GetOrderGenerateOrderbyHuman");
                query2.SetString(0, ulLabID.ToString());
                query2.SetString(1, sFacility.ToString());
                query2.SetString(2, ulHumanID.ToString());
                query2.SetString(3, ulLabID.ToString());
                query2.SetString(4, ulHumanID.ToString());
                orderList = new ArrayList(query2.List());
                if (orderList != null && orderList.Count > 0)
                {
                    for (int i = 0; i < orderList.Count; i++)
                    {
                        ilstOrders.Add(orderList[i].ToString());
                    }
                }
            }
            return ilstOrders;
        }

        public IList<Orders> GetOrdersByOrderID(ulong[] uOrderID)
        {
            IList<Orders> ilstOrders = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria OrderCri = iMySession.CreateCriteria(typeof(Orders)).Add(Expression.In("Id", uOrderID));
                ilstOrders = OrderCri.List<Orders>();
                iMySession.Close();
            }
            return ilstOrders;
        }
        public ulong InsertToDMEOrders(IList<Orders> saveList, IList<OrdersSubmit> objOrderSubmit, IList<OrdersAssessment> ordAssList, IList<string> sOrderingProcedure, ulong EncounterID, string orderType, string MACAddress, string sLocalTime)
        {
            string Height = string.Empty;
            string Weight = string.Empty;
            ulong humanid = 0;
            bool IsOrders1 = true, IsOrders2 = true, IsOrders3 = true;
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XmlObjEnc = new GenerateXml();
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            iTryCount = 0;
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = null;
            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();

            ulong ulHumanid = 0;
            if (objOrderSubmit.Count > 0)
                ulHumanid = objOrderSubmit[0].Human_ID;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                #region Vitals Values
                ICriteria critgpID = null;
                IList<PatientResults> vitals = new List<PatientResults>();
                IList<string> VitalName = new List<string>();
                VitalName.Add("Height");
                VitalName.Add("Weight");
                if (EncounterID != Convert.ToUInt32("0"))
                {
                    critgpID = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Human_ID", ulHumanid)).Add(Expression.Eq("Results_Type", "Vitals"));
                    vitals = critgpID.List<PatientResults>();
                }
                else
                {
                    critgpID = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Human_ID", ulHumanid)).Add(Expression.Eq("Results_Type", "Vitals")).Add(Expression.Not(Expression.Eq("Vitals_Group_ID", Convert.ToUInt64("0"))))
                .SetProjection(Projections.Max("Vitals_Group_ID"));
                    ulong vitalsId = 0;
                    if (critgpID.List<object>().Count > 0)
                    {
                        vitalsId = Convert.ToUInt64(critgpID.List<object>()[0]);
                    }
                    ICriteria criteriaVital = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Vitals_Group_ID", vitalsId)).Add(Expression.In("Loinc_Observation", VitalName.ToArray<string>())).Add(Expression.Eq("Results_Type", "Vitals"));
                    vitals = criteriaVital.List<PatientResults>();
                }

                foreach (PatientResults vtls in vitals)
                {
                    if (vtls.Loinc_Observation == "Height")
                        Height = vtls.Value;
                    else if (vtls.Loinc_Observation == "Weight")
                        Weight = vtls.Value;
                }
                foreach (OrdersSubmit os in objOrderSubmit)
                {
                    os.Height = Height;
                    os.Weight = Weight;
                }
                iMySession.Close();
            }
        #endregion
        TryAgain:

            #region TreatmentPlanCreation

            #endregion
            #region SaveSpecimen
            //Session.GetISession().Clear();

            ISession MySession = Session.GetISession();
            int iResult = 0;
            //ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        // trans = MySession.BeginTransaction();
                        if (saveList != null)
                        {
                            if (saveList.Count > 0)
                            {
                                #endregion
                                #region SaveOrderSubmit
                                if (objOrderSubmit[0].Internal_Property_Update_Phone_Number != string.Empty)
                                {
                                    HumanManager objHumanManger = new HumanManager();
                                    objHumanManger.UpdateHumanHomePhoneNumber(objOrderSubmit[0].Human_ID, objOrderSubmit[0].Internal_Property_Update_Phone_Number, MySession, MACAddress);
                                }
                                IList<OrdersSubmit> objOrderSubmitnull = null;
                                if (saveList.Count > 0)
                                    humanid = saveList[0].Human_ID;
                                //iResult = objOrdersSubmitManager.SaveUpdateDeleteWithoutTransaction(ref objOrderSubmit, null, null, MySession, MACAddress);
                                iResult = objOrdersSubmitManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref objOrderSubmit, ref objOrderSubmitnull, null, MySession, MACAddress, true, true, humanid, string.Empty, ref XMLObj);

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

                                if (objOrderSubmit != null)
                                    IsOrders1 = XMLObj.CheckDataConsistency(objOrderSubmit.Cast<object>().ToList(), true, string.Empty);

                                #endregion
                                #region SaveOrders
                                StaticLookupManager objstaticlookupManager = new StaticLookupManager();

                                string CMGStaticLookupValue = objstaticlookupManager.getStaticLookupByFieldName("CMG LAB NAME").Select(a => a.Value).SingleOrDefault();
                                Queue<ulong> OrdersSubmitIDs = new Queue<ulong>();
                                foreach (OrdersSubmit temp in objOrderSubmit)
                                {
                                    OrdersSubmitIDs.Enqueue(temp.Id);
                                }
                                foreach (Orders rec in saveList)
                                {
                                    //if (!rec.Order_Code_Type.StartsWith(CMGStaticLookupValue.ToUpper()))//Commentted for CMG Ancillary
                                    //{
                                    var crit = from rec1 in objOrderSubmit where rec1.Order_Code_Type == rec.Order_Code_Type select rec1;
                                    foreach (OrdersSubmit temp in crit)
                                    {
                                        rec.Order_Submit_ID = temp.Id;
                                    }
                                    //}
                                    //else
                                    //{
                                    //    rec.Order_Submit_ID = OrdersSubmitIDs.Dequeue();
                                    //}
                                }
                                IList<Orders> saveListnull = null;
                                //iResult = SaveUpdateDeleteWithoutTransaction(ref saveList, null, null, MySession, MACAddress);
                                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref saveListnull, null, MySession, MACAddress, true, true, humanid, string.Empty, ref XMLObj);
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
                                if (saveList != null)
                                    IsOrders2 = XMLObj.CheckDataConsistency(saveList.Cast<object>().ToList(), true, string.Empty);
                                #endregion
                                #region Assessment
                                if (ordAssList != null)
                                {
                                    if (ordAssList.Count > 0)
                                    {
                                        foreach (OrdersAssessment obj in ordAssList)// for bug ID 52741 
                                        {
                                            //obj.Order_ID = saveList[Convert.ToInt32(obj.Order_ID)].Id;
                                            obj.Order_Submit_ID = objOrderSubmit[0].Id;
                                        }
                                        OrdersAssessmentManager objManager = new OrdersAssessmentManager();
                                        IList<OrdersAssessment> ListUpdate = null;
                                        iResult = objManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordAssList, ref ListUpdate, null, MySession, MACAddress, true, true, humanid, string.Empty, ref XMLObj);
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
                                }
                                if (ordAssList != null)
                                    IsOrders3 = XMLObj.CheckDataConsistency(ordAssList.Cast<object>().ToList(), true, string.Empty);
                                #endregion
                            }
                        }

                        #region Treatment plan

                        if (saveList.Count > 0 && saveList[0].Encounter_ID != 0)
                        {
                            foreach (Orders obj in saveList)
                            {
                                TreatmentPlan item = new TreatmentPlan();
                                item.Human_ID = obj.Human_ID;
                                item.Encounter_Id = obj.Encounter_ID;
                                item.Physician_Id = obj.Physician_ID;
                                item.Created_By = obj.Created_By;
                                item.Created_Date_And_Time = obj.Created_Date_And_Time;
                                item.Plan_Type = "DIAGNOSTIC ORDER";
                                string plan_txt = string.Empty;
                                plan_txt = "* " + obj.Lab_Procedure_Description;
                                item.Plan = plan_txt;
                                item.Version = 0;
                                item.Source_ID = obj.Id;
                                item.Local_Time = sLocalTime;
                                Insert_Tplan.Add(item);
                            }

                            iResult = objTreatmentPlanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, null, MySession, MACAddress, true, true, Insert_Tplan[0].Encounter_Id, string.Empty, ref XmlObjEnc);

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
                                    MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }

                        #endregion

                        #region CreateAndInsertIntoWorkFlow
                        foreach (OrdersSubmit objSubmit in objOrderSubmit)
                        {
                            WFObject WFObj = new WFObject();
                            EncounterManager encMngr = new EncounterManager();
                            Encounter EncRecord = encMngr.GetById(objSubmit.Encounter_ID);
                            WFObj.Obj_Type = objSubmit.Order_Type.ToUpper();
                            WFObj.Current_Arrival_Time = objSubmit.Created_Date_And_Time;
                            if (EncRecord != null && EncRecord.Assigned_Med_Asst_User_Name != string.Empty)
                                WFObj.Current_Owner = EncRecord.Assigned_Med_Asst_User_Name;
                            else
                                WFObj.Current_Owner = "UNKNOWN";
                            WFObj.Fac_Name = objSubmit.Facility_Name;
                            WFObj.Parent_Obj_Type = string.Empty;
                            WFObj.Obj_System_Id = objSubmit.Id;
                            WFObj.Current_Process = "START";
                            WFObjectManager objWfMnger = new WFObjectManager();

                            if (objSubmit.Move_To_MA == "Y")
                            {
                                iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
                            }
                            else
                            {
                                WFObj.Current_Owner = "UNKNOWN";
                                iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 2, MACAddress, MySession);
                            }



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

                        #endregion


                        // MySession.Flush();
                        //trans.Commit();
                        if (IsOrders1 && IsOrders2 && IsOrders3)
                        {

                            if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                            {
                                //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                   // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    WriteBlob(EncounterID, XMLObj.itemDoc, MySession, saveList, null, null, XMLObj, true);
                                    trans.Commit();
                                }
                                catch (Exception xmlexcep)
                                {
                                    throw new Exception(xmlexcep.Message.ToString());
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

                            }
                            if (XmlObjEnc.strXmlFilePath != null && XmlObjEnc.strXmlFilePath != "")
                            {
                                //XmlObjEnc.itemDoc.Save(XmlObjEnc.strXmlFilePath);
                                int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                    XmlObjEnc.itemDoc.Save(XmlObjEnc.strXmlFilePath);
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
                            }
                        }
                        else
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
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

            return 0;
        }
        public OrdersDTO LoadOrdersDME(ulong EncounterID, ulong PhysicianID, ulong HumanID, string orderType, DateTime todaysDate, bool LoadQuestionSet, string sLegalOrg)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objDTO = new OrdersDTO();
            IList<ulong> ulList = new List<ulong>();
            IList<string> IcDCodes = new List<string>();
            if (EncounterID != 0)
            {
                object objStream = (object)serializer.ReadObject(GetDMEOrdersUsingEncounterID(EncounterID, orderType));
                //object objStream = (object)serializer.ReadObject(GetOrdersUsingEncounterID(EncounterID, orderType,LoadQuestionSet));
                objDTO = (OrdersDTO)objStream;

            }
            else
            {
                object objStream = (object)serializer.ReadObject(GetOrdersUsingHumanID(HumanID, todaysDate, orderType, PhysicianID, LoadQuestionSet));
                objDTO = (OrdersDTO)objStream;
            }
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ProblemListManager probMgr = new ProblemListManager();
                objDTO.MedAdvProblemList = probMgr.GetProblemList(HumanID);

                ICriteria criteria1 = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Assessment_Type", "Selected"));
                objDTO.AssessmentList = criteria1.List<Assessment>();


                ICriteria crit = iMySession.CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Results_Type", "Vitals"))
                    .SetProjection(Projections.Max("Vitals_Group_ID"));
                ulong vitalsId = 0;
                if (crit.List<object>().Count > 0)
                {
                    vitalsId = Convert.ToUInt64(crit.List<object>()[0]);
                }
                IList<string> VitalName = new List<string>();
                VitalName.Add("Height");
                VitalName.Add("Weight");

                ulong LabIdBasedOnInsID = 0;
                LabInsurancePlanManager objLabInsurancePlanManager = new LabInsurancePlanManager();
                LabIdBasedOnInsID = objLabInsurancePlanManager.GetLabIDBasedOnPhyID(PhysicianID, sLegalOrg);
                objDTO.objHuman = GetHumanById(HumanID);
                objDTO.objHuman.Lab_Id = LabIdBasedOnInsID.ToString();
                iMySession.Close();
            }
            return objDTO;

        }

        public Stream GetDMEOrdersUsingEncounterID(ulong EncounterID, string orderType)
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            OrdersDTO objOrdersDTO = new OrdersDTO();
            IList<OrderLabDetailsDTO> ordLabList = new List<OrderLabDetailsDTO>();
            OrderLabDetailsDTO ordLabObj = new OrderLabDetailsDTO();
            Orders objOrd = new Orders();
            IList<OrdersAssessment> ordAssList = new List<OrdersAssessment>();
            OrdersAssessment objOrdAss = new OrdersAssessment();
            IList<ulong> ulList = new List<ulong>();
            IList<ulong> ulSpecimenIDList = new List<ulong>();
            IList<Orders> iOrderList = new List<Orders>();
            //ArrayList orderList = null;
            ulong Order_id = 0;
            ulong Order_submit_id = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISQLQuery query2 = iMySession.CreateSQLQuery("SELECT O.*,OS.*,L.*,LL.* FROM orders O LEFT JOIN orders_submit OS ON O.Order_Submit_ID=OS.Order_Submit_ID LEFT JOIN lab L ON (OS.Lab_ID=L.Lab_ID) LEFT JOIN lab_location LL ON (OS.Lab_Location_ID=LL.Lab_Location_ID) WHERE O.Encounter_ID='" + EncounterID.ToString() + "'  AND OS.Order_Type='" + orderType + "' ORDER BY CAST(if(O.Modified_Date_And_Time<>'0001-01-01 00:00:00',O.Modified_Date_And_Time,O.Created_date_and_time) AS CHAR(100)) DESC").AddEntity("O", typeof(Orders)).AddEntity("OS", typeof(OrdersSubmit)).AddEntity("L", typeof(Lab)).AddEntity("LL", typeof(LabLocation));

                //ISQLQuery query2 = iMySession.CreateSQLQuery("SELECT O.*,OS.* FROM orders O LEFT JOIN orders_submit OS ON O.Order_Submit_ID=OS.Order_Submit_ID WHERE O.Encounter_ID='" + EncounterID.ToString() + "'  AND OS.Order_Type='" + orderType + "' ORDER BY CAST(if(O.Modified_Date_And_Time<>'0001-01-01 00:00:00',O.Modified_Date_And_Time,O.Created_date_and_time) AS CHAR(100)) DESC").AddEntity("O", typeof(Orders)).AddEntity("OS", typeof(OrdersSubmit));


                ISQLQuery query2 = iMySession.CreateSQLQuery("SELECT OS.* FROM orders_submit OS WHERE OS.Encounter_ID='" + EncounterID.ToString() + "'  AND OS.Order_Type='" + orderType + "'").AddEntity("OS", typeof(OrdersSubmit));

                IList<OrdersSubmit> ilstOsSubmit = query2.List<OrdersSubmit>();
                IList<Orders> ilstOrder = new List<Orders>();
                IList<LabLocation> ilstlocation = new List<LabLocation>();

                IList<WFObject> ilstWFObject = new List<WFObject>();
                IList<ulong> AddedOrderIds = new List<ulong>();

                if (ilstOsSubmit.Count > 0)
                {
                    var osID = ilstOsSubmit.Select(aa => aa.Id).ToArray();
                    if (osID.Count() > 0)
                    {
                        var osIDList = string.Join(",", ilstOsSubmit.Select(aa => aa.Id.ToString()).ToArray());
                        ISQLQuery query3 = iMySession.CreateSQLQuery("SELECT O.* FROM orders O WHERE O.Encounter_ID='" + EncounterID.ToString() + "'  AND O.Order_submit_id in(" + osIDList + ") ORDER BY CAST(if(O.Modified_Date_And_Time<>'0001-01-01 00:00:00',O.Modified_Date_And_Time,O.Created_date_and_time) AS CHAR(100)) DESC").AddEntity("O", typeof(Orders));
                        ilstOrder = query3.List<Orders>();
                    }
                    var osLID = ilstOsSubmit.Select(aa => aa.Lab_Location_ID).ToArray();
                    if (osLID.Count() > 0)
                    {
                        var osLIDList = string.Join(",", ilstOsSubmit.Select(aa => aa.Lab_Location_ID.ToString()).ToArray());
                        ISQLQuery query3 = iMySession.CreateSQLQuery("SELECT LL.* FROM lab_location LL WHERE LL.Lab_Location_ID in(" + osLIDList + ")").AddEntity("LL", typeof(LabLocation));
                        ilstlocation = query3.List<LabLocation>();
                    }
                    if (osID.Count() > 0)
                    {
                        var osIDList = string.Join(",", ilstOsSubmit.Select(aa => aa.Id.ToString()).ToArray());
                        ISQLQuery query4 = iMySession.CreateSQLQuery("SELECT W.* FROM Wf_Object W WHERE W.Obj_Type ='DME ORDER' and W.Obj_System_Id in(" + osIDList + ")").AddEntity("W", typeof(WFObject));
                        ilstWFObject = query4.List<WFObject>();
                    }
                    foreach (var osid in ilstOrder)
                    {
                        ordLabObj = new OrderLabDetailsDTO();
                        Orders objOrder = new Orders();
                        OrdersSubmit objOrdersSubmit = new OrdersSubmit();

                        ordLabObj.ObjOrder = (Orders)osid;
                        AddedOrderIds.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                        Order_id = ordLabObj.ObjOrder.Id;
                        Order_submit_id = ordLabObj.ObjOrder.Order_Submit_ID;

                        ordLabObj.OrdersSubmit = ilstOsSubmit.Where(aa => aa.Id == ordLabObj.ObjOrder.Order_Submit_ID).Select(aa => aa).ToList()[0];

                        if (ordLabObj.OrdersSubmit.Lab_Name != null)
                            ordLabObj.LabName = ordLabObj.OrdersSubmit.Lab_Name;
                        if (ilstlocation.Count > 0)
                        {
                            if (ordLabObj.OrdersSubmit.Lab_Location_ID != Convert.ToUInt32(0))
                            {
                                ordLabObj.LabLocName = ilstlocation.Where(aa => aa.Id == ordLabObj.OrdersSubmit.Lab_Location_ID).Select(aa => aa.Location_Name).ToList()[0]; ;
                            }
                        }
                        ordLabObj.procedureCodeDesc = ordLabObj.ObjOrder.Lab_Procedure + "-" + ordLabObj.ObjOrder.Lab_Procedure_Description;

                        var objcurrentProcess = ilstWFObject.Where(aa => aa.Obj_System_Id == ordLabObj.ObjOrder.Order_Submit_ID).Select(aa => aa).ToList();
                        if (objcurrentProcess.Count > 0)
                            ordLabObj.ObjOrder.Internal_Property_Current_Process = objcurrentProcess[0].Current_Process;

                        //ulList.Add(ordLabObj.ObjOrder.Id);
                        ulList.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                        ordLabList.Add(ordLabObj);
                    }
                }
                //IList<object[]> orderList = query2.List<object[]>();



                //if (orderList != null)
                //{
                //    foreach (object[] obj in orderList)
                //    {
                //        if (obj.Count() > 0)
                //        {
                //            ordLabObj = new OrderLabDetailsDTO();
                //            Orders objOrder = new Orders();
                //            OrdersSubmit objOrdersSubmit = new OrdersSubmit();

                //            if (obj[0] != null)
                //            {
                //                ordLabObj.ObjOrder = (Orders)obj[0];
                //                AddedOrderIds.Add(ordLabObj.ObjOrder.Order_Submit_ID);
                //                Order_id = ordLabObj.ObjOrder.Id;
                //                Order_submit_id = ordLabObj.ObjOrder.Order_Submit_ID;
                //            }
                //            if (obj[1] != null)
                //            {
                //                ordLabObj.OrdersSubmit = (OrdersSubmit)obj[1];
                //            }
                //            if (obj[2] != null)
                //            {
                //                if (((Lab)obj[2]).Lab_Name != null)
                //                    ordLabObj.LabName = ((Lab)obj[2]).Lab_Name;
                //            }
                //            if (obj[3] != null)
                //            {
                //                if (((LabLocation)obj[3]).Location_Name != null)
                //                    ordLabObj.LabLocName = ((LabLocation)obj[3]).Location_Name;
                //            }

                //            ordLabObj.procedureCodeDesc = ordLabObj.ObjOrder.Lab_Procedure + "-" + ordLabObj.ObjOrder.Lab_Procedure_Description;
                //            ulList.Add(ordLabObj.ObjOrder.Id);

                //            ICriteria critOrdersRequiredForms = iMySession.CreateCriteria(typeof(OrdersRequiredForms)).Add(Expression.Eq("Order_Id", ordLabObj.ObjOrder.Id));
                //            if (critOrdersRequiredForms.List<OrdersRequiredForms>().Count > 0)
                //            {
                //                ordLabObj.ilstOrdersRequiredForms = critOrdersRequiredForms.List<OrdersRequiredForms>();
                //            }
                //            ordLabList.Add(ordLabObj);
                //        }
                //    }

                //}

                AddedOrderIds = AddedOrderIds.Distinct().ToList<ulong>();
                objOrdersDTO.ilstOrderLabDetailsDTO = ordLabList;
                //orderList.Clear();
                if (ulList.Count > 0)
                {
                    objOrdersDTO.OrderAssList = GetOrdersAssessmentDetails(ulList);
                }
                serializer.WriteObject(stream, objOrdersDTO);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;
        }
        public ulong DMEUpdateOrders(IList<Orders> saveList, IList<Orders> updtList, IList<Orders> delList, IList<OrdersAssessment> savOrdAssListForExistingCPT, IList<OrdersAssessment> delOrdAssListForExistingCPT, IList<OrdersAssessment> savOrdAssListForNewCPT, TreatmentPlan UpdatePlan, IList<string> PlanDel, IList<string> SelectedProcedures, IList<string> PlanReplace, ulong EncounterID, string orderType, string MACAddress, IList<OrdersSubmit> SavOrderSubmit, IList<OrdersSubmit> UpdOrderSubmit, IList<OrdersSubmit> DelOrderSubmit, string sLocalTime)
        {

            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();

            //TreatmentPlan UpdateTreatmentPlan = null;
            ulong uHumanId = 0;
            IList<OrdersSubmit> templist = new List<OrdersSubmit>();
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XmlObjEnc = new GenerateXml();
            bool isOrder = true, isOrderSubmit = true, isOrderAssessmnt = true;
            string ModifiedBy = string.Empty;
            //DateTime ModifiedDAT = new DateTime();

            #region TplanGet
            IList<string> ilstGeneralPlanTagList = new List<string>();
            ilstGeneralPlanTagList.Add("TreatmentPlanList");


            IList<object> ilstGeneralPlanBlobFinal = new List<object>();
            ilstGeneralPlanBlobFinal = ReadBlob(EncounterID, ilstGeneralPlanTagList);
            if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
            {
                if (ilstGeneralPlanBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                    {
                        objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                    }
                }
            }
            //string FileName = "Encounter" + "_" + EncounterID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //XmlTextReader XmlText = null;
            //try
            //{
            //    if (File.Exists(strXmlFilePath) == true)
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        XmlText = new XmlTextReader(strXmlFilePath);
            //        XmlNodeList xmlTagName = null;
            //        // itemDoc.Load(XmlText);
            //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //        {
            //            itemDoc.Load(fs);

            //            XmlText.Close();
            //            #region Treatment_plan
            //            if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("DIAGNOSTIC ORDER"))
            //                        {

            //                            string TagName = xmlTagName[j].Name;
            //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
            //                            TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
            //                            IEnumerable<PropertyInfo> propInfo = null;
            //                            TreatmentPlan = (TreatmentPlan)TreatmentPlan;
            //                            propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                            {

            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(TreatmentPlan, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }

            //                            }
            //                            objTreatmentPlan.Add(TreatmentPlan);
            //                        }
            //                    }
            //                }
            //            }
            //            #endregion

            //            fs.Close();
            //            fs.Dispose();
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    if (XmlText != null)
            //        XmlText.Close();

            //    throw Ex;
            //}
            #endregion

            ISession MySession = Session.GetISession();
            if (SavOrderSubmit.Count > 0)
            {
                if (SavOrderSubmit[0].Internal_Property_Update_Phone_Number != string.Empty)
                {
                    HumanManager objHumanManger = new HumanManager();
                    objHumanManger.UpdateHumanHomePhoneNumber(SavOrderSubmit[0].Human_ID, SavOrderSubmit[0].Internal_Property_Update_Phone_Number, MySession, MACAddress);
                }
            }
            else if (UpdOrderSubmit.Count > 0)
            {
                if (UpdOrderSubmit[0].Internal_Property_Update_Phone_Number != string.Empty)
                {
                    HumanManager objHumanManger = new HumanManager();
                    objHumanManger.UpdateHumanHomePhoneNumber(UpdOrderSubmit[0].Human_ID, UpdOrderSubmit[0].Internal_Property_Update_Phone_Number, MySession, MACAddress);
                }
            }
            StaticLookupManager objstaticlookupManager = new StaticLookupManager();
            string CMGStaticLookupValue = objstaticlookupManager.getStaticLookupByFieldName("CMG LAB NAME").Select(a => a.Value).SingleOrDefault();

            iTryCount = 0;

        TryAgain:
            int iResult = 0;


            ITransaction trans = null;
            try
            {
                trans = MySession.BeginTransaction();

                if (SavOrderSubmit.Count > 0)
                {
                    uHumanId = SavOrderSubmit[0].Human_ID;
                }
                else if (UpdOrderSubmit.Count > 0)
                {
                    uHumanId = UpdOrderSubmit[0].Human_ID;
                }
                else if (DelOrderSubmit.Count > 0)
                {
                    uHumanId = DelOrderSubmit[0].Human_ID;
                }
                iResult = objOrdersSubmitManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SavOrderSubmit, ref UpdOrderSubmit, DelOrderSubmit, MySession, MACAddress, true, true, uHumanId, string.Empty, ref XMLObj);
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

                if (SavOrderSubmit != null && SavOrderSubmit.Count > 0 && UpdOrderSubmit != null && UpdOrderSubmit.Count > 0)
                    isOrderSubmit = XMLObj.CheckDataConsistency(SavOrderSubmit.Concat(UpdOrderSubmit).Cast<object>().ToList(), true, string.Empty);
                else if ((SavOrderSubmit != null && SavOrderSubmit.Count > 0) || (UpdOrderSubmit != null && UpdOrderSubmit.Count > 0))
                {
                    if (SavOrderSubmit != null && SavOrderSubmit.Count > 0)
                        isOrderSubmit = XMLObj.CheckDataConsistency(SavOrderSubmit.Cast<object>().ToList(), true, string.Empty);
                    else if (UpdOrderSubmit != null && UpdOrderSubmit.Count > 0)
                        isOrderSubmit = XMLObj.CheckDataConsistency(UpdOrderSubmit.Cast<object>().ToList(), true, string.Empty);
                }
                if (saveList.Count > 0 || delList.Count > 0 || updtList.Count > 0)
                {
                    //if (updtList.Count > 0)
                    //    updtList[0].Order_Submit_ID = UpdOrderSubmit[0].Id;
                    if (saveList.Count > 0)
                    {
                        foreach (OrdersSubmit rec in SavOrderSubmit.Concat(UpdOrderSubmit))
                        {
                            foreach (var rec1 in saveList.Where(a => a.Order_Code_Type == rec.Order_Code_Type))
                            {
                                rec1.Order_Submit_ID = rec.Id;
                            }
                        }
                    }
                    if (delList.Count > 0)
                    {
                        delList = delList.Except(delList.Where(a => DelOrderSubmit.Select(b => b.Id).Contains(a.Order_Submit_ID)).ToList<Orders>()).ToList<Orders>();
                    }
                    if (saveList.Count > 0)
                    {
                        uHumanId = saveList[0].Human_ID;
                    }
                    else if (updtList.Count > 0)
                    {
                        uHumanId = updtList[0].Human_ID;
                    }
                    OrdersManager objOrdersManager = new OrdersManager();
                    //iResult = objOrdersManager.SaveUpdateDeleteWithoutTransaction(ref saveList, updtList, delList, MySession, MACAddress);
                    iResult = objOrdersManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref updtList, delList, MySession, MACAddress, true, true, uHumanId, string.Empty, ref XMLObj);
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
                    if (saveList != null && saveList.Count > 0 && updtList != null && updtList.Count > 0)
                        isOrder = XMLObj.CheckDataConsistency(saveList.Concat(updtList).Cast<object>().ToList(), true, string.Empty);
                    else if ((saveList != null && saveList.Count > 0) || (updtList != null && updtList.Count > 0))
                    {
                        if (saveList != null && saveList.Count > 0)
                            isOrder = XMLObj.CheckDataConsistency(saveList.Cast<object>().ToList(), true, string.Empty);
                        else if (updtList != null && updtList.Count > 0)
                            isOrder = XMLObj.CheckDataConsistency(updtList.Cast<object>().ToList(), true, string.Empty);
                    }
                }

                if (savOrdAssListForExistingCPT.Count > 0 || delOrdAssListForExistingCPT.Count > 0 || savOrdAssListForNewCPT.Count > 0)
                {
                    //foreach (Orders rec in saveList.Concat(updtList))
                    foreach (OrdersSubmit rec in SavOrderSubmit.Concat(UpdOrderSubmit))
                    {
                        //To Map Assessment and Orders . Assessment.Associated_Order_CPT property is used to hold proceudre which is mappend again with orders procedure to find order_id.
                        //foreach (OrdersAssessment rec1 in savOrdAssListForNewCPT.Where(a => a.Internal_Property_Associated_Order_CPT == rec.Lab_Procedure).ToList<OrdersAssessment>())
                        foreach (OrdersAssessment rec1 in savOrdAssListForNewCPT)
                        {
                            rec1.Order_Submit_ID = rec.Id;
                        }
                    }

                    OrdersAssessmentManager objManager = new OrdersAssessmentManager();
                    // iResult = objManager.BatchOperationsToOrdersAssessment(savOrdAssListForNewCPT, savOrdAssListForExistingCPT, delOrdAssListForExistingCPT, MySession, MACAddress);
                    iResult = objManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savOrdAssListForNewCPT, ref savOrdAssListForExistingCPT, delOrdAssListForExistingCPT, MySession, MACAddress, true, true, uHumanId, string.Empty, ref XMLObj);
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
                    if (savOrdAssListForNewCPT != null && savOrdAssListForNewCPT.Count > 0 && savOrdAssListForExistingCPT != null && savOrdAssListForExistingCPT.Count > 0)
                        isOrderAssessmnt = XMLObj.CheckDataConsistency(savOrdAssListForNewCPT.Concat(savOrdAssListForExistingCPT).Cast<object>().ToList(), true, string.Empty);
                    else if ((savOrdAssListForNewCPT != null && savOrdAssListForNewCPT.Count > 0) || (savOrdAssListForExistingCPT != null && savOrdAssListForExistingCPT.Count > 0))
                    {
                        if (savOrdAssListForNewCPT != null && savOrdAssListForNewCPT.Count > 0)
                            isOrderAssessmnt = XMLObj.CheckDataConsistency(savOrdAssListForNewCPT.Cast<object>().ToList(), true, string.Empty);
                        else if (savOrdAssListForExistingCPT != null && savOrdAssListForExistingCPT.Count > 0)
                            isOrderAssessmnt = XMLObj.CheckDataConsistency(savOrdAssListForExistingCPT.Cast<object>().ToList(), true, string.Empty);
                    }
                }
                #region Tplan

                if (saveList != null && saveList.Count > 0)
                {
                    foreach (Orders obj in saveList)
                    {
                        TreatmentPlan item = new TreatmentPlan();
                        item.Human_ID = obj.Human_ID;
                        item.Encounter_Id = obj.Encounter_ID;
                        item.Physician_Id = obj.Physician_ID;
                        item.Created_By = obj.Created_By;
                        item.Created_Date_And_Time = obj.Created_Date_And_Time;
                        item.Plan_Type = "DME ORDER";
                        string plan_txt = string.Empty;
                        plan_txt = "* " + obj.Lab_Procedure_Description;
                        item.Plan = plan_txt;
                        item.Version = 0;
                        item.Source_ID = obj.Id;
                        item.Local_Time = sLocalTime;
                        Insert_Tplan.Add(item);
                    }
                }
                if (updtList != null && updtList.Count > 0)
                {
                    foreach (Orders obj in updtList)
                    {
                        if (objTreatmentPlan.Count > 0)
                        {
                            IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Source_ID == obj.Id).ToList();
                            if (itemlst != null && itemlst.Count > 0)
                            {
                                TreatmentPlan item = itemlst[0];
                                string plan_txt = string.Empty;
                                plan_txt = "* " + obj.Lab_Procedure_Description;
                                item.Plan = plan_txt;
                                item.Modified_By = obj.Modified_By;
                                item.Modified_Date_And_Time = obj.Modified_Date_And_Time;
                                Update_Tplan.Add(item);
                            }

                        }

                    }
                }

                if (delList != null && delList.Count > 0)
                    if (objTreatmentPlan.Count > 0)
                    {
                        Delete_Tplan = (from item in objTreatmentPlan where delList.Any(a => a.Id == item.Source_ID) select item).ToList();
                    }

                if ((Insert_Tplan.Count > 0 && Insert_Tplan[0].Encounter_Id != 0) || (Update_Tplan.Count > 0 && Update_Tplan[0].Encounter_Id != 0) || (Delete_Tplan.Count > 0 && Delete_Tplan[0].Encounter_Id != 0))
                {
                    iResult = objTreatmentPlanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, Delete_Tplan, MySession, MACAddress, true, true, EncounterID, string.Empty, ref XmlObjEnc);

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
                            MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }


                #endregion
                #region CreateAndInsertIntoWorkFlow
                WFObjectManager objWfMnger = new WFObjectManager();
                foreach (OrdersSubmit objSubmit in SavOrderSubmit)
                {
                    WFObject WFObj = new WFObject();
                    EncounterManager encMngr = new EncounterManager();
                    Encounter EncRecord = encMngr.GetById(objSubmit.Encounter_ID);
                    WFObj.Obj_Type = objSubmit.Order_Type.ToUpper();
                    WFObj.Current_Arrival_Time = objSubmit.Created_Date_And_Time;
                    if (EncRecord != null && EncRecord.Assigned_Med_Asst_User_Name != string.Empty)
                        WFObj.Current_Owner = EncRecord.Assigned_Med_Asst_User_Name;
                    else
                        WFObj.Current_Owner = "UNKNOWN";
                    WFObj.Fac_Name = objSubmit.Facility_Name;
                    WFObj.Parent_Obj_Type = string.Empty;
                    WFObj.Obj_System_Id = objSubmit.Id;
                    WFObj.Current_Process = "START";


                    if (objSubmit.Move_To_MA == "Y")
                    {
                        iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
                    }
                    else
                    {
                        WFObj.Current_Owner = "UNKNOWN";
                        iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 2, MACAddress, MySession);
                    }


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
                foreach (OrdersSubmit objSubmit in DelOrderSubmit)
                {
                    iResult = objWfMnger.MoveToNextProcess(objSubmit.Id, objSubmit.Order_Type, 6, "UNKNOWN", DateTime.Now, MACAddress, null, MySession);
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
                foreach (OrdersSubmit objSubmit in UpdOrderSubmit)
                {
                    WFObject objWFObject = objWfMnger.GetByObjectSystemId(objSubmit.Id, objSubmit.Order_Type);

                    if (objSubmit.Move_To_MA == "Y")
                    {
                        string MA_NAME = string.Empty;
                        string[] ProcessAndUsers = objWFObject.Process_Allocation.Split('|');
                        foreach (string rec in ProcessAndUsers)
                        {
                            if (rec.StartsWith("MA_"))
                                MA_NAME = rec.Split('-')[1];
                        }

                        if (MA_NAME == string.Empty)
                        {
                            EncounterManager objEncounterManager = new EncounterManager();
                            if (objSubmit.Encounter_ID != 0)
                                MA_NAME = objEncounterManager.GetById(objSubmit.Encounter_ID).Assigned_Med_Asst_User_Name;
                            else
                                MA_NAME = "UNKNOWN";
                        }
                        if (objWFObject.Current_Process == "ORDER_GENERATE")
                        {

                            iResult = objWfMnger.MoveToPreviousProcessWithTransaction(objSubmit.Id, objSubmit.Order_Type, 1, MA_NAME, DateTime.Now, MACAddress, MySession);
                        }
                    }
                    else
                    {
                        if (objWFObject.Current_Process == "MA_REVIEW")
                        {

                            iResult = objWfMnger.MoveToNextProcess(objSubmit.Id, objSubmit.Order_Type, 1, "UNKNOWN", DateTime.Now, MACAddress, null, MySession);
                        }
                    }
                }
                #endregion
                if (isOrder && isOrderSubmit && isOrderAssessmnt)
                {

                    if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                    {
                        //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                        int trycount = 0;
                    trytosaveagain:
                        try
                        {
                            // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            WriteBlob(EncounterID, XMLObj.itemDoc, MySession, saveList, updtList, delList, XMLObj, false);
                            trans.Commit();
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
                    }
                    if (XmlObjEnc.strXmlFilePath != null && XmlObjEnc.strXmlFilePath != "")
                    {
                        //XmlObjEnc.itemDoc.Save(XmlObjEnc.strXmlFilePath);
                        int trycount = 0;
                    trytosaveagain:
                        try
                        {
                            XmlObjEnc.itemDoc.Save(XmlObjEnc.strXmlFilePath);
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
                    }
                }
                else
                {
                    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                }
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
            return 0;
        }


        public void GetDataforDMEOrders(OrdersSubmit objOrdersSubmit, out Encounter objEncounter, out Lab objLab, out LabLocation objLabLocation,
            out IList<Orders> objOrders, out IList<OrdersAssessment> objOrdersAssessment)
        {
            EncounterManager mngrEncounter = new EncounterManager();
            objEncounter = mngrEncounter.GetById(objOrdersSubmit.Encounter_ID);
            LabManager mngrLab = new LabManager();
            objLab = mngrLab.GetById(objOrdersSubmit.Lab_ID);
            LabLocationManager mngrLabLoc = new LabLocationManager();
            IList<LabLocation> objLabLoc = new List<LabLocation>();
            objLabLocation = null;
            objLabLoc = mngrLabLoc.GetLabLocationUsinglablocID(objOrdersSubmit.Lab_Location_ID);
            if (objLabLoc != null && objLabLoc.Count > 0)
                objLabLocation = objLabLoc[0];
            OrdersManager mngrOrders = new OrdersManager();
            objOrders = mngrOrders.GetOrdersByOrderSubmitID(new List<int>() { Convert.ToInt32(objOrdersSubmit.Id) });
            OrdersAssessmentManager mngrOrdersAssessment = new OrdersAssessmentManager();
            objOrdersAssessment = mngrOrdersAssessment.GetOrderAssessmentByOrderSubmitID(objOrdersSubmit.Id);
        }

        public ulong UpdateOrderAndOrdersSubmit(IList<OrdersSubmit> ordersubmitlist, IList<Orders> ordersList, string MACAddress)
        {
            GenerateXml XMLObj = new GenerateXml();
            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            ulong orderSubmitId = 0;
            string user_name = string.Empty;
            ISession MySession = Session.GetISession();
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            IList<OrdersSubmit> ordersubmitlistnull = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if (ordersubmitlist != null)
                        {
                            if (ordersubmitlist.Count > 0)
                            {
                                ordersubmitlist.ToList().ForEach(x => x.Is_Task_Created = "N");
                                iResult = objOrdersSubmitManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordersubmitlistnull, ref ordersubmitlist, null, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
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
                                ////foreach (Orders obj in ordersubmitlist)
                                ////{
                                //orderSubmitId = ordersubmitlist[0].Id;
                                ////}

                                //foreach (Orders obj in ordersList)
                                //{
                                //    obj.Order_Submit_ID = orderSubmitId;
                                //}
                            }

                            /////////////////////////////////////

                            if (ordersList != null)
                            {
                                if (ordersList.Count > 0)
                                {
                                    IList<Orders> ordersListnull = null;
                                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordersListnull, ref ordersList, null, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
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
                            }
                            trans.Commit();
                        }
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
                //MySession.Close();
                throw new Exception(ex1.Message);
            }
            return orderSubmitId;

        }




    }
}