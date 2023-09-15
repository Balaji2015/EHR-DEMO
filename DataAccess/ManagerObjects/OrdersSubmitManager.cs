using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IOrdersSubmitManager : IManagerBase<OrdersSubmit, ulong>
    {
       // int BatchOperationsToOrdersSubmit(IList<OrdersSubmit> savelist, IList<OrdersSubmit> updtList, IList<OrdersSubmit> delList, ISession MySession, string MACAddress);
        //IList<OrdersSubmit> GroupOrdersToSendToLabCorp(IList<OrdersSubmit> submitList, IList<Orders> ordersList, string MACAddress);
        //OrderDetailsDTO UpdateSubmitOrders(IList<OrdersSubmit> insertList, IList<OrdersSubmit> delList, IList<Orders> orderList, ulong EncounterID, string OrderType, string MACAddress);
        bool InsertDummyOrder(IList<OrdersSubmit> submitList, string obj_Type, string facility_Name, string MACAddress);
        //IList<OrdersSubmit> GroupOrdersToSendToQuest(IList<OrdersSubmit> submitList, IList<Orders> ordersList, string MACAddress);
        int InsertDummyOrderForResultEntry(ref IList<OrdersSubmit> ordersSubmitList, ISession MySession, string macAddress);
        IList<OrdersSubmit> GetOrdersSubmitListbyID(ulong OrderSubmitId);
        //void SaveOrderList(IList<OrdersSubmit> ilist, IList<WFObject> ilistwfobj);
        IList<OrdersSubmit> CreateOrderSubmitAndWFObject(IList<OrdersSubmit> ilist, IList<WFObject> ilistwfobj, string MACAddress);
        string GetTestUsingOrderSubmitID(ulong Ordersubmit_ID);
        string GetOrderingPhysicianByOrderSubmitID(ulong OrdersSubmitID);
        ulong GetOrderingPhysicianIdByOrderSubmitID(ulong OrdersSubmitID);
        IList<OrderTaskDTO> GetOrderDetailsForTaskNotification();
    }
    public partial class OrdersSubmitManager : ManagerBase<OrdersSubmit, ulong>, IOrdersSubmitManager
    {
         #region Constructors

          public OrdersSubmitManager()
            : base()
        {

        }
          public OrdersSubmitManager
              (INHibernateSession session)
              : base(session)
          {
          }

        #endregion

        #region IOrdersSubmitManager Members

        //not in use
        //public int BatchOperationsToOrdersSubmit(IList<OrdersSubmit> savelist, IList<OrdersSubmit> updtList, IList<OrdersSubmit> delList, ISession MySession, string MACAddress)
        //{
        //   // return SaveUpdateDeleteWithoutTransaction(ref savelist, updtList, delList, MySession, MACAddress);
        //    GenerateXml XMLObj = new GenerateXml();
        //    return SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savelist, ref updtList, delList, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
        //}

        int iTryCount = 0;
        //public IList<OrdersSubmit> GroupOrdersToSendToLabCorp(IList<OrdersSubmit> submitList, IList<Orders> ordersList, string MACAddress)
        //{
        //    iTryCount = 0;
        //    IList<OrdersSubmit> returnList = new List<OrdersSubmit>();
        //TryAgain:
        //    int iResult = 0;

        //    ISession MySession = Session.GetISession();
        //    ITransaction trans = null;
        //    try
        //    {
        //        trans = MySession.BeginTransaction();
        //        if (submitList != null)
        //        {
        //            if (submitList.Count > 0)
        //            {
        //                iResult = SaveUpdateDeleteWithoutTransaction(ref submitList, null, null, MySession, MACAddress);
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
        //                        //MySession.Close();
        //                        throw new Exception("Deadlock occurred. Transaction failed.");
        //                    }
        //                }
        //                else if (iResult == 1)
        //                {
        //                    trans.Rollback();
        //                    //MySession.Close();
        //                    throw new Exception("Exception occurred. Transaction failed.");
        //                }
        //            }
        //        }
        //        if (ordersList != null)
        //        {
        //            if (ordersList.Count > 0)
        //            {

        //                foreach (Orders objOrd in ordersList)
        //                {
        //                    ulong SubmitId = (from objSub in submitList where objSub.Order_Code_Type == objOrd.Order_Code_Type select objSub.Id).ToList<ulong>()[0];
        //                    objOrd.Order_Submit_ID = SubmitId;
        //                }
                       
        //                    OrdersManager objmap = new OrdersManager();
        //                    iResult = objmap.UpdateSpecimenDetailOnOrders(ordersList, MySession, MACAddress);
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
        //                          //  MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                       // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //            }
        //        }
        //        foreach (OrdersSubmit objSubmit in submitList)
        //         {
        //            WFObject WFObj = new WFObject();
        //            EncounterManager encMngr = new EncounterManager();
        //            Encounter EncRecord = encMngr.GetById(objSubmit.Encounter_ID); 

        //            //WFObj.Obj_Type = ordersList[0].Order_Type.ToUpper();
        //            WFObj.Current_Arrival_Time = objSubmit.Created_Date_And_Time;
        //            if (EncRecord!=null && EncRecord.Assigned_Med_Asst_User_Name != string.Empty)
        //                WFObj.Current_Owner = EncRecord.Assigned_Med_Asst_User_Name;
        //            else
        //                WFObj.Current_Owner = "UNKNOWN";
        //            //WFObj.Fac_Name = ordersList[0].Facility_Name;
        //            WFObj.Parent_Obj_Type = string.Empty;
        //            WFObj.Obj_System_Id = objSubmit.Id;
        //            WFObj.Current_Process = "START";
        //            WFObjectManager objWfMnger = new WFObjectManager();
                    
        //           if (ordersList[0].Move_To_MA=="Y")
        //            {
        //                iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
        //            }
        //            else
        //            {
        //                WFObj.Current_Owner = "UNKNOWN";
        //                iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 2, MACAddress, MySession);
        //            }
                   
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
        //                   // MySession.Close();
        //                    throw new Exception("Deadlock occurred. Transaction failed.");
        //                }
        //            }
        //            else if (iResult == 1)
        //            {
        //                trans.Rollback();
        //               // MySession.Close();
        //                throw new Exception("Exception occurred. Transaction failed.");
        //            }
        //        }

        //        MySession.Flush();
        //        trans.Commit();
        //    }
        //    catch (NHibernate.Exceptions.GenericADOException ex)
        //    {
        //        trans.Rollback();
        //        // MySession.Close();
        //        throw new Exception(ex.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        trans.Rollback();
        //        //MySession.Close();
        //        throw new Exception(e.Message);
        //    }
        //    finally
        //    {
        //        MySession.Close();
        //    }
        //    foreach (OrdersSubmit obj in submitList)
        //        returnList.Add(obj);
        //    return returnList;
        //}
        #endregion

        #region Insert Dummy Order
        public bool InsertDummyOrder(IList<OrdersSubmit> submitList, string obj_Type, string facility_Name,string MACAddress)
        {

            iTryCount = 0;
            GenerateXml XMLObj = new GenerateXml();
            IList<OrdersSubmit> submitListnull = null;
            IList<OrdersSubmit> returnList = new List<OrdersSubmit>();
            TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            //ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        //trans = MySession.BeginTransaction();
                        if (submitList != null)
                        {
                            if (submitList.Count > 0)
                            {
                                //iResult = SaveUpdateDeleteWithoutTransaction(ref submitList, null, null, MySession, MACAddress);

                                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref submitList, ref submitListnull, null, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
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


                                Orders objOrder = new Orders();
                                objOrder.Human_ID = submitList[0].Human_ID;
                                objOrder.Order_Submit_ID = submitList[0].Id;
                                //objOrder.Order_Type = obj_Type;
                                objOrder.Created_By = submitList[0].Created_By;
                                objOrder.Created_Date_And_Time = submitList[0].Created_Date_And_Time;
                                IList<Orders> saveOrderList = new List<Orders>();
                                saveOrderList.Add(objOrder);
                                OrdersManager objOrdersManager = new OrdersManager();
                                IList<Orders> saveOrderListnull = null;
                                //iResult = objOrdersManager.SaveUpdateDeleteWithoutTransaction(ref saveOrderList, null, null, MySession, MACAddress);
                                iResult = objOrdersManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveOrderList, ref saveOrderListnull, null, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
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


                                WFObject WFObj = new WFObject();
                                WFObj.Obj_Type = obj_Type.ToUpper();
                                WFObj.Current_Arrival_Time = submitList[0].Created_Date_And_Time;
                                WFObj.Current_Owner = "UNKNOWN";
                                WFObj.Fac_Name = facility_Name;
                                WFObj.Parent_Obj_Type = string.Empty;
                                WFObj.Obj_System_Id = submitList[0].Id;
                                WFObj.Current_Process = "START";
                                string[] sCurrentProcess = WFObj.Current_Process.ToString().Split(' ');
                                WFObjectManager objWfMnger = new WFObjectManager();
                                iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
                                iResult = objWfMnger.MoveToNextProcess(WFObj.Obj_System_Id, WFObj.Obj_Type, 1, "UNKNOWN", submitList[0].Created_Date_And_Time, MACAddress, sCurrentProcess, MySession);
                                //iResult = objWfMnger.MoveToNextProcess(WFObj.Obj_System_Id, WFObj.Obj_Type, 1, "UNKNOWN", submitList[0].Created_Date_And_Time, MACAddress, MySession);

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
            return true;
 
        }

        #endregion





        #region IOrdersSubmitManager Members


        //public OrderDetailsDTO UpdateSubmitOrders(IList<OrdersSubmit> insertList, IList<OrdersSubmit> delList, IList<Orders> orderList, ulong EncounterID, string OrderType, string MACAddress)
        //{
        //    return new OrderDetailsDTO();
        //}

        #endregion

        //public IList<OrdersSubmit> GroupOrdersToSendToQuest(IList<OrdersSubmit> submitList, IList<Orders> ordersList, string MACAddress)
        //{
        //    iTryCount = 0;
        //    IList<OrdersSubmit> returnList = new List<OrdersSubmit>();
        //TryAgain:
        //    int iResult = 0;

        //    ISession MySession = Session.GetISession();
        //    ITransaction trans = null;
        //    try
        //    {
        //        trans = MySession.BeginTransaction();
        //        if (submitList != null)
        //        {
        //            if (submitList.Count > 0)
        //            {
        //                iResult = SaveUpdateDeleteWithoutTransaction(ref submitList, null, null, MySession, MACAddress);
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
        //                        //MySession.Close();
        //                        throw new Exception("Deadlock occurred. Transaction failed.");
        //                    }
        //                }
        //                else if (iResult == 1)
        //                {
        //                    trans.Rollback();
        //                    //MySession.Close();
        //                    throw new Exception("Exception occurred. Transaction failed.");
        //                }
        //            }
        //        }
        //        if (ordersList != null)
        //        {
        //            if (ordersList.Count > 0)
        //            {
        //                foreach (OrdersSubmit ordsub in submitList)
        //                {
        //                    if (ordsub.Order_Code != string.Empty && ordsub.Temperature_state!=string.Empty)
        //                    {
        //                        var ord = (from rec in ordersList where rec.Lab_Procedure == ordsub.Order_Code select rec);
        //                        foreach (Orders objOrders in ord)
        //                        {
        //                            objOrders.Order_Submit_ID = ordsub.Id;
        //                        }
        //                    }
        //                    //else if (ordsub.Temperature_state == string.Empty )
        //                    //{
        //                    //    var ord = (from rec in ordersList where rec.Temperature == string.Empty && !(submitList.Select(a => a.Order_Code).ToList()).Contains(rec.Lab_Procedure)  select rec);
        //                    //    foreach (Orders objOrders in ord)
        //                    //    {
        //                    //        objOrders.Order_Submit_ID = ordsub.Id;
        //                    //    }
        //                    //}
        //                    else
        //                    {
        //                        var ord = (from rec in ordersList where rec.Lab_Procedure_Description == ordsub.Temperature_state && !(submitList.Select(a => a.Order_Code).ToList()).Contains(rec.Lab_Procedure) select rec);
        //                        foreach (Orders objOrders in ord)
        //                        {
        //                            objOrders.Order_Submit_ID = ordsub.Id;
        //                        }
        //                    }
        //                }


        //                //foreach (Orders objOrd in ordersList)
        //                //{
        //                //    ulong SubmitId = (from objSub in submitList where objSub.Temperature_state == objOrd.Temperature && objSub.Order_Code select objSub.Id).ToList<ulong>()[0];
        //                //    objOrd.Order_Submit_ID = SubmitId;
        //                //}

        //                OrdersManager objmap = new OrdersManager();
        //                iResult = objmap.UpdateSpecimenDetailOnOrders(ordersList, MySession, MACAddress);
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
        //                        //  MySession.Close();
        //                        throw new Exception("Deadlock occurred. Transaction failed.");
        //                    }
        //                }
        //                else if (iResult == 1)
        //                {
        //                    trans.Rollback();
        //                    // MySession.Close();
        //                    throw new Exception("Exception occurred. Transaction failed.");
        //                }
        //            }
        //        }
        //        foreach (OrdersSubmit objSubmit in submitList)
        //        {
        //            WFObject WFObj = new WFObject();
        //            EncounterManager encMngr = new EncounterManager();
        //            Encounter EncRecord = encMngr.GetById(objSubmit.Encounter_ID); 


        //           // WFObj.Obj_Type = ordersList[0].Order_Type.ToUpper();
        //            WFObj.Current_Arrival_Time = objSubmit.Created_Date_And_Time;
        //            WFObj.Current_Owner = EncRecord.Assigned_Med_Asst_User_Name;
        //           // WFObj.Fac_Name = ordersList[0].Facility_Name;
        //            WFObj.Parent_Obj_Type = string.Empty;
        //            WFObj.Obj_System_Id = objSubmit.Id;
        //            WFObj.Current_Process = "START";
        //            WFObjectManager objWfMnger = new WFObjectManager();
                     
                   
        //            if (ordersList[0].Move_To_MA=="Y")
        //            {
        //            iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
        //            }
        //            else
        //            {
        //            WFObj.Current_Owner = "UNKNOWN";
        //            iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 2, MACAddress, MySession);
        //            }
                     
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

        //        MySession.Flush();
        //        trans.Commit();
        //    }
        //    catch (NHibernate.Exceptions.GenericADOException ex)
        //    {
        //        trans.Rollback();
        //        // MySession.Close();
        //        throw new Exception(ex.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        trans.Rollback();
        //        //MySession.Close();
        //        throw new Exception(e.Message);
        //    }
        //    finally
        //    {
        //        MySession.Close();
        //    }
        //    foreach (OrdersSubmit obj in submitList)
        //        returnList.Add(obj);
        //      return returnList;
 
        //}
        // Added by Manimozhi - Manual Result Entry - 
        public int InsertDummyOrderForResultEntry(ref IList<OrdersSubmit> ordersSubmitList, ISession MySession, string macAddress)
        {
            int iResult = 0;
            GenerateXml XMLObj = new GenerateXml();
            //iResult = SaveUpdateDeleteWithoutTransaction(ref ordersSubmitList, null, null, MySession, macAddress);
            IList<OrdersSubmit> ordersSubmitListnull = null;
            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordersSubmitList, ref ordersSubmitListnull, null, MySession, macAddress, false, true, 0, string.Empty, ref XMLObj);
            return iResult;
        }

        public IList<OrdersSubmit> GetOrdersSubmitListbyID(ulong OrderSubmitId)
        {            
            IList<OrdersSubmit> lstorderSubmit = new List<OrdersSubmit>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", OrderSubmitId));
                lstorderSubmit = crit.List<OrdersSubmit>();
                iMySession.Close();
            }
            return lstorderSubmit;
        }
        public string GetTestUsingOrderSubmitID(ulong Ordersubmit_ID)
        {
            string TestOrdered = string.Empty;
            IList<string> temp = new List<string>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ISQLQuery sq = iMySession.CreateSQLQuery("SELECT O.* FROM orders O where O.Order_Submit_ID =" + Ordersubmit_ID.ToString() + ";").AddEntity("O", typeof(Orders));
                //.AddEntity("W", typeof(WFObject));
                temp = sq.List<Orders>().Select(a => a.Lab_Procedure + "-" + a.Lab_Procedure_Description).ToList();
                foreach (string str in temp.Distinct())
                {
                    if (TestOrdered.Trim() == string.Empty)
                        TestOrdered = str;
                    else
                        TestOrdered += " ; " + str;
                }
                iMySession.Close();
            }
            return TestOrdered;

        }
        //BugID:52738 
        public string GetOrderingPhysicianByOrderSubmitID(ulong OrdersSubmitID)
        {
            string Ordering_PhyName=string.Empty;
            IList<OrdersSubmit> lstOrdersSubmit = new List<OrdersSubmit>();
            lstOrdersSubmit = GetOrdersSubmitListbyID(OrdersSubmitID);
            if (lstOrdersSubmit != null && lstOrdersSubmit.Count > 0 && lstOrdersSubmit[0].Id != 0)
            {
                PhysicianManager PhyManager = new PhysicianManager();
                IList<PhysicianLibrary> lstPhyLib = new List<PhysicianLibrary>();
                if (lstOrdersSubmit[0].Physician_ID != 0)
                {
                    lstPhyLib = PhyManager.GetphysiciannameByPhyID(lstOrdersSubmit[0].Physician_ID);
                    if (lstPhyLib != null && lstPhyLib.Count > 0 && lstPhyLib[0].Id != 0)
                        Ordering_PhyName = lstPhyLib[0].PhyPrefix + ' ' + lstPhyLib[0].PhyFirstName + ' ' + lstPhyLib[0].PhyMiddleName + ' ' + lstPhyLib[0].PhyLastName + ' ' + lstPhyLib[0].PhySuffix;
                }
                else
                {
                   EncounterManager objenc = new EncounterManager();
                      IList<Encounter> lstEncounter = new List<Encounter>();
                      lstEncounter = objenc.GetOrderingPhysicianByOrderSubmitID(OrdersSubmitID);
                      if (lstEncounter != null && lstEncounter.Count > 0 && lstEncounter[0].Referring_Physician.Trim() != null && lstEncounter[0].Referring_Physician.Trim() != string.Empty)
                      {
                          Ordering_PhyName = lstEncounter[0].Referring_Physician;
                      }
                }
            }
            return Ordering_PhyName;
        }
        //Added by Suvarnni
        //public void SaveOrderList(IList<OrdersSubmit> ilist,IList<WFObject> ilistwfobj)
        //{
        //    int iResult = 0;
        //    GenerateXml XMLObj = new GenerateXml();
        //    ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    IList<OrdersSubmit> ilistnull = null;
        //   // iResult = SaveUpdateDeleteWithoutTransaction(ref ilist, null, null, iMySession, string.Empty);
        //    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilist, ref ilistnull, null, iMySession, string.Empty, false, true, 0, string.Empty, ref XMLObj);
        //    //Srividhya
        //    ilistwfobj[0].Is_Default_MyQ_LineItem = 1;
        //    ilistwfobj[0].Obj_System_Id = ilist[0].Id;
        //    WFObjectManager objwfObject = new WFObjectManager();
        //    objwfObject.saveListwfobj(ilistwfobj);
        //    iMySession.Close();
        //}

        public IList<OrdersSubmit> CreateOrderSubmitAndWFObject(IList<OrdersSubmit> ilist, IList<WFObject> ilistwfobj, string MACAddress)
        {
            iTryCount = 0;
            GenerateXml XMLObj = new GenerateXml();
            IList<OrdersSubmit> returnList = new List<OrdersSubmit>();
        TryAgain:
            int iResult = 0;
              ISession MySession = Session.GetISession();
              try
              {
                  using (ITransaction transs = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                  {
                      //ISession MySession = Session.GetISession();
                      ITransaction trans = null;
                      try
                      {
                          trans = MySession.BeginTransaction();
                          if (ilist != null)
                          {
                              if (ilist.Count > 0)
                              {
                                  IList<OrdersSubmit> ilistnull = null;
                                  //iResult = SaveUpdateDeleteWithoutTransaction(ref ilist, null, null, MySession, MACAddress);
                                  iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilist, ref ilistnull, null, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
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
                              }
                          }

                          if (ilistwfobj != null)
                          {
                              if (ilistwfobj.Count > 0)
                              {
                                  WFObjectManager WFObjMngr = new WFObjectManager();
                                  ilistwfobj[0].Obj_System_Id = ilist[0].Id;
                                  //iResult = WFObjMngr.SaveUpdateDeleteWithoutTransaction(ref ilistwfobj, null, null, MySession, MACAddress);
                                  IList<WFObject> ilistwfobjnull = null;
                                  iResult = WFObjMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ilistwfobj, ref ilistwfobjnull, null, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
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
            return ilist;

        }


        public ulong GetOrderingPhysicianIdByOrderSubmitID(ulong OrdersSubmitID)
        {
            ulong Ordering_PhyID =0;       
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    ISQLQuery sql;
                    sql = iMySession.CreateSQLQuery("select o.Physician_ID from orders_submit o where o.order_submit_id=" + OrdersSubmitID.ToString() + "");

                    ArrayList arrList = new ArrayList(sql.List());
                    for (int i = 0; i < arrList.Count; i++)
                    {
                        Ordering_PhyID = Convert.ToUInt64(arrList[i]);                       
                    }
                    iMySession.Close();
                }
                return Ordering_PhyID;
          
        }

        public string GetLabLocationAddressbyOrderSubmitID(ulong OrdersSubmitID)
        {
            string LabLocationAddress = string.Empty;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql;
                sql = iMySession.CreateSQLQuery("select os.*,ll.* from orders_submit os, lab_location ll where os.order_submit_id= '" + OrdersSubmitID.ToString() + "' and os.lab_location_id=ll.lab_location_id").AddEntity("os", typeof(OrdersSubmit)).AddEntity("ll", typeof(LabLocation));

                foreach (IList<Object> l in sql.List())
                {
                    LabLocation objLabLocation = (LabLocation)l[1];

                    if (objLabLocation.Street_Address1 != string.Empty)
                    {
                        LabLocationAddress = objLabLocation.Street_Address1 + Environment.NewLine + objLabLocation.City + " " + objLabLocation.State + " " + objLabLocation.ZipCode + Environment.NewLine + objLabLocation.Phone_No + Environment.NewLine + Environment.NewLine;
                    }
                }

                if (LabLocationAddress == string.Empty)
                {
                    ISQLQuery sql1;
                    //sql1 = iMySession.CreateSQLQuery("select ll.* from orders_submit os, lab_location ll where os.order_submit_id= '" + OrdersSubmitID.ToString() + "' and os.lab_id=ll.lab_id").AddEntity("os", typeof(OrdersSubmit)).AddEntity("ll", typeof(LabLocation));
                    sql1 = iMySession.CreateSQLQuery("select os.*,ll.* from orders_submit os, lab_location ll where os.order_submit_id= '" + OrdersSubmitID.ToString() + "' and os.lab_id=ll.lab_id").AddEntity("os", typeof(OrdersSubmit)).AddEntity("ll", typeof(LabLocation));


                    foreach (IList<Object> l in sql1.List())
                    {
                        LabLocation objLabLocation = (LabLocation)l[1];

                        if (objLabLocation.Street_Address1 != string.Empty)
                        {
                            LabLocationAddress = objLabLocation.Street_Address1 + Environment.NewLine + objLabLocation.City + " " + objLabLocation.State + " " + objLabLocation.ZipCode + Environment.NewLine + objLabLocation.Phone_No + Environment.NewLine + Environment.NewLine;
                        }
                    }
                }
                
                iMySession.Close();
            }
            return LabLocationAddress;

        }

        public IList<OrderTaskDTO> GetOrderDetailsForTaskNotification()
        {

            ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            IQuery query1 = mySession.GetNamedQuery("GetOrderObjectDetails.For.TaskCreate");

            ArrayList objects = new ArrayList(query1.List());
            IList<OrderTaskDTO> OrdersSubmitList = new List<OrderTaskDTO>();
            OrderTaskDTO OsObj = new OrderTaskDTO();
            foreach (object[] obj in objects)
            {
                OsObj = new OrderTaskDTO();
                OsObj.Order_Submit_ID = Convert.ToUInt64(obj[0]);
                OsObj.Stat = obj[1].ToString();
                OsObj.Created_Date_And_Time = Convert.ToDateTime((obj[2]));
                OsObj.Task = obj[3].ToString();
                OsObj.Priority = obj[4].ToString();
                OsObj.Ancillary_Order = obj[5].ToString();
                OsObj.Created_By = obj[6].ToString();
                OsObj.Physician_Name = obj[7].ToString();
                OsObj.Lab_Procedure = obj[8].ToString();
                OsObj.Lab_Procedure_Description = obj[9].ToString();
                OsObj.Lab_Name = obj[10].ToString();
                OsObj.ICD = obj[11].ToString();
                OsObj.ICD_Description = obj[12].ToString();
                OsObj.Facility_Name = obj[13].ToString();
                OsObj.Modified_Date_And_Time = Convert.ToDateTime((obj[14]));
                OsObj.Current_Process = obj[15].ToString();
                OsObj.Human_ID = Convert.ToUInt64(obj[16]);

                OrdersSubmitList.Add(OsObj);
            }
            return OrdersSubmitList;
        }
    }
}
