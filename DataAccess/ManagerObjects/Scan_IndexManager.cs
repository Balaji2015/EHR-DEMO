using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using NHibernate;
using System.Diagnostics;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IScan_IndexManager : IManagerBase<scan_index, ulong>
    {
        Scan_IndexDTO SaveUpdateDeleteScanIndex(IList<scan_index> InsertList, IList<scan_index> UpdateList, IList<scan_index> DeleteList, ulong humanID, ulong scan_ID, string macAddress);
        IList<scan_index> GetScannedObjectByHumanIDAndInfo(ulong humanID, string Information);
        IList<scan_index> GetScannedObjectByFileName(string fileName);
        IList<scan_index> GetScannedObjectByHumanIDInfoAndEncounterDate(ulong humanID, string Information, DateTime EncounterDate);
        IList<scan_index> GetScannedObjectByScanID(ulong scanID, ulong ulHumanID);
        IList<scan_index> GetScanDocumentlistByHumanId(ulong ulhumanID);
        //IList<Encounter> GetEncounterDetails(ulong ulhumanID);
        scan_index GetscanIndexDetails(string sImagePathname);
        void scan_index_Table_update(string strfacility);
        Scan_IndexDTO SaveUpdateDeleteOnlineDocuments(IList<scan_index> InsertList, IList<scan_index> UpdateList, IList<scan_index> DeleteList, ulong humanID, ulong scan_ID, string macAddress, string Owner, string Facility, string filePath, int pageCount, string fileName, DateTime dtScanReceivedDate, string sScanType);

        Scan_IndexDTO GetScannedListByScanID(ulong scanID, ulong ulWorksetID);
        IList<Orders> GetOutStandingOrders(ulong humanID);

        //srividhya added the below function on 29-Nov-2013
        //int MoveToNextProcessforIndexing(ArrayList arylstCompletedList, string sOwner, DateTime StartTime, IList<ObjectProcessHistoryBillingTemp> ObjectProcessHistoryBillingTempList);
        int GetTotalEncounterCount(ulong ulWorksetID);
        IList<scan_index> GetScanIndexForHuman(ulong Human_Id);
        IList<scan_index> GetScannedObjectScanID(ulong scanID);
        //IList<scan_index> GetPageNumbersforThumbNailDelete(string filename);
        IList<scan_index> GetScanIndexDetailsForOrderID(ulong uScanID);
        IList<scan_index> GetScanIndexDetailsForScanIndexConversionID(ulong uIndexConScanID);
        void SaveUpdateScanIndexforDeleteFiles(IList<scan_index> lstScanIndex);
    }

    public partial class Scan_IndexManager : ManagerBase<scan_index, ulong>, IScan_IndexManager
    {
        #region Constructors


        public Scan_IndexManager()
            : base()
        {

        }
        public Scan_IndexManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion


        public Scan_IndexDTO SaveUpdateDeleteScanIndex(IList<scan_index> InsertList, IList<scan_index> UpdateList, IList<scan_index> DeleteList, ulong humanID, ulong scan_ID, string macAddress)
        {
            Scan_IndexDTO ScanIndexDTOObject = new Scan_IndexDTO();
            SaveUpdateDelete_DBAndXML_WithTransaction(ref InsertList, ref UpdateList, DeleteList, macAddress, false, false, 0, string.Empty);
            return ScanIndexDTOObject;
        }

        public IList<scan_index> GetScannedObjectByHumanIDAndInfo(ulong humanID, string Information)
        {
            IList<scan_index> lstscn = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Human_ID", humanID)).Add(Expression.Eq("Document_Type", Information));
                lstscn = criteria.List<scan_index>();
                iMySession.Close();
            }
            return lstscn;
        }
        public IList<scan_index> GetScannedObjectByHumanID(ulong humanID)
        {
            IList<scan_index> lstscn = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Human_ID", humanID));
                lstscn = criteria.List<scan_index>();
                iMySession.Close();
            }
            return lstscn;
        }

        public IList<scan_index> GetDetailsbyscanid(ulong sacnid)
        {
            IList<scan_index> lstscn = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", sacnid));
                lstscn = criteria.List<scan_index>();
                iMySession.Close();
            }
            return lstscn;
        }

        public IList<scan_index> GetScannedObjectByHumanIDInfoAndEncounterDate(ulong humanID, string Information, DateTime EncounterDate)
        {

            IList<scan_index> lstscn = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.Scan_Index.HumanIDInfoAndEncounterDate");
                query.SetString(0, humanID.ToString());
                query.SetString(1, Information);
                query.SetString(2, EncounterDate.ToString("yyyy-MM-dd") + "%");
                lstscn = query.List<scan_index>();
                iMySession.Close();
            }

            return lstscn;
        }

        public IList<scan_index> GetScannedObjectByFileName(string fileName)
        {
            IList<scan_index> lstscn = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scanned_File_Path", fileName));
                lstscn = criteria.List<scan_index>();
                iMySession.Close();
            }

            return lstscn;
        }
        public IList<scan_index> GetScannedObjectScanID(ulong scanID)
        {
            IList<scan_index> lstscn = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", scanID));
                lstscn = criteria.List<scan_index>();
                iMySession.Close();
            }

            return lstscn;
        }

        public IList<scan_index> GetScannedObjectByScanID(ulong scanID, ulong ulHumanID)
        {
            IList<Scan> lstscan = new List<Scan>();
            IList<scan_index> lstscanindex = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria criteria = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", scanID)).Add(Expression.Eq("Human_ID", ulHumanID));
                lstscanindex = criteria.List<scan_index>();


                if (lstscanindex.Count > 0)
                {
                    ICriteria crit = iMySession.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Scan_ID", scanID));
                    lstscan = crit.List<Scan>();
                    lstscanindex[0].Scan_Information = lstscan[0];


                }

                for (int i = 0; i < lstscanindex.Count; i++)
                {
                    if (lstscanindex[i].Order_ID != 0)
                    {
                        ICriteria criteria_OrdersSubmit = iMySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("id", lstscanindex[i].Order_ID));
                        IList<OrdersSubmit> lstorder = criteria_OrdersSubmit.List<OrdersSubmit>();
                        lstscanindex[i].Object_Type = lstorder[0].Order_Type;
                        lstscanindex[i].Physician_ID = lstorder[0].Physician_ID;
                        lstscanindex[i].lab_name = lstorder[0].Lab_Name;

                        ICriteria criteria_User = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", lstscanindex[i].Physician_ID));
                        IList<User> lstuser = criteria_User.List<User>();
                        lstscanindex[i].Physician_Name = lstuser[0].user_name;
                    }
                }
                iMySession.Close();
            }
            return lstscanindex;

        }



        //Added by Velmurugan on 04.04.2011
        public IList<scan_index> GetScanDocumentlistByHumanId(ulong ulhumanID)
        {
            IList<scan_index> lstscn = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Human_ID", ulhumanID));
                lstscn = crit.List<scan_index>();
                iMySession.Close();
            }

            return lstscn;

        }

        //public IList<Encounter> GetEncounterDetails(ulong ulhumanID)
        //{
        //    IList<Encounter> lstenc = new List<Encounter>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", ulhumanID));
        //        lstenc = crit.List<Encounter>();
        //        iMySession.Close();
        //    }

        //    return lstenc;
        //}

        public scan_index GetscanIndexDetails(string sImagePathname)
        {
            scan_index objscn = new scan_index();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Indexed_File_Path", sImagePathname));
                objscn = crit.List<scan_index>()[0];
                iMySession.Close();
            }
            return objscn;
        }

        //<<<<<<< Scan_IndexManager.cs
        //        public void scan_index_Table_update(string facility)
        //        {




        //            IList<scan_index> lst_scan_index = new List<scan_index>();
        //            ISession current_session = Session.GetISession();

        //            ICriteria crit = current_session.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", "INDEX")).Add(Expression.Eq("Fac_Name", facility)); ;
        //            IList<WFObject> lstWFObject = crit.List<WFObject>();
        //            for (int i = 0; i < lstWFObject.Count; i++)
        //            {

        //                //Scan index Conversion

        //                ICriteria crit_index = current_session.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id));
        //                IList<scan_index> lstWF_Index_conversion = crit_index.List<scan_index>();
        //                for (int k = 0; k < lstWF_Index_conversion.Count; k++)
        //                {

        //                    scan_index obj_scan_index = new scan_index();





        //                    obj_scan_index.Id = lstWF_Index_conversion[k].Id;

        //                    obj_scan_index.Scan_ID = lstWF_Index_conversion[k].Scan_ID;
        //                    obj_scan_index.Created_By = lstWF_Index_conversion[k].Created_By;
        //                    obj_scan_index.Version = lstWF_Index_conversion[k].Version;
        //                    obj_scan_index.Human_ID = lstWF_Index_conversion[k].Human_ID;
        //                    obj_scan_index.Document_Type = lstWF_Index_conversion[k].Document_Type;
        //                    obj_scan_index.Document_Sub_Type = lstWF_Index_conversion[k].Document_Sub_Type;
        //                    obj_scan_index.Document_Date = lstWF_Index_conversion[k].Document_Date;
        //                    obj_scan_index.Page_Selected= lstWF_Index_conversion[k].Page_Selected;
        //                    obj_scan_index.Order_ID= lstWF_Index_conversion[k].Order_ID;
        //                    string file_name = string.Empty;
        //                    string[] index_arr = lstWF_Index_conversion[k].Indexed_File_Path.Split('\\');

        //                    file_name = index_arr[10];



        //                    //// For localobj_scan_index.Indexed_File_Path = index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[5] + "\\" + index_arr[6] + "\\" + file_name;

        //                    obj_scan_index.Indexed_File_Path = index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9] + "\\" + file_name;

        //                    lst_scan_index.Add(obj_scan_index);


        //                }

        //                //End


        //            }

        //            IList<scan_index> scan_index_List = new List<scan_index>();
        //            scan_index_List = null;
        //            SaveUpdateDeleteWithTransaction(ref scan_index_List, lst_scan_index, null, "002564B0B5D6");

        //        }

        //        public Scan_IndexDTO GetScanIndex_orders_list(ulong ulhumanID)
        //||||||| 1.12
        //        public Scan_IndexDTO GetScanIndex_orders_list(ulong ulhumanID)
        //=======
        public void scan_index_Table_update(string facility)
        {




            IList<scan_index> lst_scan_index = new List<scan_index>();
            using (ISession current_session = Session.GetISession())
            {

                ICriteria crit = current_session.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", "INDEX")).Add(Expression.Eq("Fac_Name", facility)); ;
                IList<WFObject> lstWFObject = crit.List<WFObject>();
                for (int i = 0; i < lstWFObject.Count; i++)
                {

                    //Scan index Conversion

                    ICriteria crit_index = current_session.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id));
                    IList<scan_index> lstWF_Index_conversion = crit_index.List<scan_index>();
                    for (int k = 0; k < lstWF_Index_conversion.Count; k++)
                    {

                        scan_index obj_scan_index = new scan_index();





                        obj_scan_index.Id = lstWF_Index_conversion[k].Id;

                        obj_scan_index.Scan_ID = lstWF_Index_conversion[k].Scan_ID;
                        obj_scan_index.Created_By = lstWF_Index_conversion[k].Created_By;
                        obj_scan_index.Version = lstWF_Index_conversion[k].Version;
                        obj_scan_index.Human_ID = lstWF_Index_conversion[k].Human_ID;
                        obj_scan_index.Document_Type = lstWF_Index_conversion[k].Document_Type;
                        obj_scan_index.Document_Sub_Type = lstWF_Index_conversion[k].Document_Sub_Type;
                        obj_scan_index.Document_Date = lstWF_Index_conversion[k].Document_Date;
                        obj_scan_index.Page_Selected = lstWF_Index_conversion[k].Page_Selected;
                        obj_scan_index.Order_ID = lstWF_Index_conversion[k].Order_ID;
                        string file_name = string.Empty;
                        string[] index_arr = lstWF_Index_conversion[k].Indexed_File_Path.Split('\\');

                        file_name = index_arr[10];



                        //// For localobj_scan_index.Indexed_File_Path = index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[5] + "\\" + index_arr[6] + "\\" + file_name;

                        obj_scan_index.Indexed_File_Path = index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9] + "\\" + file_name;

                        lst_scan_index.Add(obj_scan_index);


                    }

                    //End


                }
                current_session.Close();
            }

            IList<scan_index> scan_index_List = new List<scan_index>();
            scan_index_List = null;
            SaveUpdateDeleteWithTransaction(ref scan_index_List, lst_scan_index, null, "002564B0B5D6");

        }

        public Scan_IndexDTO SaveUpdateDeleteOnlineDocuments(IList<scan_index> InsertList, IList<scan_index> UpdateList, IList<scan_index> DeleteList, ulong humanID, ulong scan_ID, string macAddress, string Owner, string Facility, string filePath, int pageCount, string fileName, DateTime dtScanReceivedDate, string sScanType)
        {


            ulong scan_id = 0;

            IList<Scan> lstscan = new List<Scan>();
            Scan_IndexDTO ScanIndexDTOObject = new Scan_IndexDTO();
            if (scan_ID != 0)
            {
                scan_id = scan_ID;
                WFObjectManager objwf_object = new WFObjectManager();
                objwf_object.UpdateOwner(scan_ID, "SCAN", Owner, macAddress);
                SaveUpdateDelete_DBAndXML_WithTransaction(ref InsertList, ref UpdateList, DeleteList, macAddress, false, false, 0, string.Empty);
            }
            else
            {
            TryAgain:
                ISession MySession = Session.GetISession();
                ITransaction trans = null;
                int iResult = 0;
                int iTryCount = 0;
                GenerateXml XMLObj = new GenerateXml();
                try
                {
                    trans = MySession.BeginTransaction();
                    ScanManager objscanmanager = new ScanManager();
                    Scan objscan = new Scan();
                    //objscan.Scan_Type = "Online Chart";
                    objscan.Scan_Type = sScanType;
                    objscan.No_of_Pages = pageCount;
                    //objscan.Scanned_Date = InsertList[0].Created_Date_And_Time;//For bug Id : 66408 
                    objscan.Scanned_Date = dtScanReceivedDate;
                    objscan.Scanned_File_Name = fileName;
                    objscan.Scanned_File_Path = filePath;
                    objscan.Facility_Name = Facility;
                    objscan.Created_By = Owner;
                    objscan.Created_Date_And_Time = InsertList[0].Created_Date_And_Time;
                    lstscan.Add(objscan);
                    IList<Scan> UpdateScanList = null;

                    iResult = objscanmanager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstscan, ref UpdateScanList, null, MySession, macAddress, false, false, 0, string.Empty, ref XMLObj);
                    scan_id = Convert.ToUInt64(lstscan[0].Id);

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
                            throw new Exception("Deadlock is occured. Transaction failed");

                        }
                    }
                    else if (iResult == 1)
                    {

                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception is occured. Transaction failed");

                    }


                    if (lstscan != null)
                    {
                        IList<WFObject> workflowobjectList = new List<WFObject>();
                        WFObject WFObj = new WFObject();
                        WFObj.Fac_Name = Facility;
                        WFObj.Obj_Type = "SCAN";
                        WFObj.Current_Owner = Owner;
                        WFObj.Current_Process = "START";
                        WFObj.Current_Arrival_Time = InsertList[0].Created_Date_And_Time;
                        workflowobjectList.Add(WFObj);

                        WFObject currentwfObject = new WFObject();
                        currentwfObject = workflowobjectList.ElementAt<WFObject>(0);
                        currentwfObject.Obj_System_Id = scan_id;
                        WFObjectManager wfObjectManager = new WFObjectManager();
                        wfObjectManager.InsertToWorkFlowObject(currentwfObject, 2, macAddress, MySession);


                        InsertList[0].Scan_ID = scan_id;
                        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref InsertList, ref UpdateList, DeleteList, MySession, macAddress, false, false, 0, string.Empty, ref XMLObj);
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
                                throw new Exception("Deadlock is occured. Transaction failed");

                            }
                        }
                        else if (iResult == 1)
                        {

                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Exception is occured. Transaction failed");

                        }

                        MySession.Flush();
                        trans.Commit();


                    }
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

            return ScanIndexDTOObject;
        }

        public Scan_IndexDTO GetHumanDetailsByHumanID(ulong humanID)
        {
            Scan_IndexDTO ScanIndexDTOObject = new Scan_IndexDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", humanID));
                IList<Human> humanList = criteria.List<Human>();
                if (humanList.Count > 0)
                {
                    ScanIndexDTOObject.First_Name = humanList[0].First_Name;
                    ScanIndexDTOObject.MI = humanList[0].MI;
                    ScanIndexDTOObject.Last_Name = humanList[0].Last_Name;
                    //ScanIndexDTOObject.Human_Type = humanList[0].Human_Type;
                    ScanIndexDTOObject.Sex = humanList[0].Sex;
                    ScanIndexDTOObject.Birth_Date = humanList[0].Birth_Date;
                }
                iMySession.Close();
            }
            return ScanIndexDTOObject;
        }

        public Scan_IndexDTO GetScannedListByScanID(ulong scanID, ulong ulWorksetID)
        {

            Scan_IndexDTO ScanIndexDTOObject = new Scan_IndexDTO();

            IList<Scan> lstscan = new List<Scan>();
            IList<scan_index> lstscanindex = new List<scan_index>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (ulWorksetID > 0)
                {
                    ICriteria criteria = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", scanID)).Add(Expression.Eq("Workset_ID", ulWorksetID));
                    lstscanindex = criteria.List<scan_index>();
                }
                else
                {
                    ICriteria criteria = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", scanID));
                    lstscanindex = criteria.List<scan_index>();
                }

                ICriteria crit = iMySession.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Scan_ID", scanID));
                lstscan = crit.List<Scan>();
                if (lstscan.Count > 0)
                {
                    ScanIndexDTOObject.ScanDetails = lstscan[0];
                }

                //string[] orderID = lstscanindex.Where(a => a.Order_ID != 0).Select(a => Convert.ToString(a.Order_ID)).ToArray();
                //string orderSubmits = string.Join(",", orderID);
                //if (orderID.Count() > 0)
                //{
                //    IList<object> criteria_orders = iMySession.CreateSQLQuery("SELECT order_submit_id,Physician_ID FROM orders_submit WHERE order_submit_id in ( " + orderSubmits + " )").List<object>();
                //    foreach (object item in criteria_orders)
                //    {
                //        object[] obj = (object[])item;
                //        foreach (scan_index records in lstscanindex)
                //        {
                //            if (records.Order_ID == Convert.ToUInt32(obj[0]))
                //            {
                //                records.Physician_ID = Convert.ToUInt64(obj[1]);
                //            }
                //        }
                //    }
                //}

                ScanIndexDTOObject.ScanIndexList = lstscanindex;


                //if (ScanIndexDTOObject != null & ScanIndexDTOObject.ScanIndexList.Count > 0)
                //{
                //    ICriteria criteria = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", ScanIndexDTOObject.ScanIndexList[0].Human_ID));
                //    IList<Human> humanList = criteria.List<Human>();
                //    if (humanList.Count > 0)
                //    {
                //        ScanIndexDTOObject.First_Name = humanList[0].First_Name;
                //        ScanIndexDTOObject.MI = humanList[0].MI;
                //        ScanIndexDTOObject.Last_Name = humanList[0].Last_Name;
                //        ScanIndexDTOObject.Human_Type = humanList[0].Human_Type;
                //        ScanIndexDTOObject.Sex = humanList[0].Sex;
                //        ScanIndexDTOObject.Birth_Date = humanList[0].Birth_Date;
                //    }
                //}
                iMySession.Close();
            }
            return ScanIndexDTOObject;
        }

        //int iTryCount, j = 0;
        //public int MoveToNextProcessforIndxing(ArrayList arylstCompletedList, string sOwner, DateTime StartTime, IList<ObjectProcessHistoryBillingTemp> ObjectProcessHistoryBillingTempList)
        //{

        //TryAgain:
        //    int iResult = 0;
        //    ISession MySession = Session.GetISession();
        //    ITransaction trans = null;
        //    try
        //    {

        //        WFObjectManager objWFObjectManager = new WFObjectManager();
        //        trans = MySession.BeginTransaction();

        //        for (int i = 0; i < arylstCompletedList.Count; i++)
        //        {
        //            iResult = objWFObjectManager.MoveToNextProcess(Convert.ToUInt32(arylstCompletedList[i]), "DOCUMENTATION", 1, sOwner, StartTime, "", null, MySession);
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
        //                //MySession.Close();
        //                throw new Exception("Exception occurred. Transaction failed.");
        //            }

        //            //discussed with Kumar the Encounter obj is moevd from ma_process/scheduled to billing_wait process
        //            iResult = objWFObjectManager.MoveToNextProcess(Convert.ToUInt32(arylstCompletedList[i]), "ENCOUNTER", 5, sOwner, StartTime, "", null, MySession);
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
        //                    MySession.Close();
        //                    throw new Exception("Deadlock occurred. Transaction failed.");
        //                }
        //            }
        //            else if (iResult == 1)
        //            {
        //                trans.Rollback();
        //                MySession.Close();
        //                throw new Exception("Exception occurred. Transaction failed.");
        //            }
        //        }

        //        if (ObjectProcessHistoryBillingTempList.Count > 0)
        //        {
        //            //iResult = 0;
        //            if (ObjectProcessHistoryBillingTempList != null && ObjectProcessHistoryBillingTempList.Count > 0)
        //            {
        //                ObjectProcessHistoryBillingTempManager ObjProcHisBillTempMngr = new ObjectProcessHistoryBillingTempManager();
        //                iResult = ObjProcHisBillTempMngr.SaveUpdateDeleteWithoutTransaction(ref ObjectProcessHistoryBillingTempList, null, null, MySession, "");
        //                if (iResult == 2)
        //                {
        //                    if (iTryCount < 5)
        //                    {
        //                        iTryCount++;
        //                        //goto TryAgain;
        //                    }
        //                    else
        //                    {
        //                        trans.Rollback();
        //                        //  MySession.Close();
        //                        throw new Exception("Deadlock is occured. Transaction failed");
        //                    }
        //                }
        //                else if (iResult == 1)
        //                {
        //                    trans.Rollback();
        //                    // MySession.Close();
        //                    throw new Exception("Exception is occured. Transaction failed");
        //                }
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

        //    return j;
        //}

        public int GetTotalEncounterCount(ulong ulWorksetID)
        {
            IList<scan_index> ScanIndexList = new List<scan_index>();
            int iTotalNoofLineItem = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("SELECT * FROM scan_index_conversion where workset_id='" + ulWorksetID + "' and doc_type='Encounters' and Doc_Sub_type='Encounter Forms';")
                  .AddEntity("s", typeof(scan_index));
                ScanIndexList = sql.List<scan_index>();

                for (int i = 0; i < ScanIndexList.Count; i++)
                {
                    if ((ScanIndexList[i].Document_To_Date - ScanIndexList[i].Document_Date).Days == 0)
                    {
                        iTotalNoofLineItem = iTotalNoofLineItem + 1;
                    }
                    else
                    {
                        iTotalNoofLineItem = iTotalNoofLineItem + ((ScanIndexList[i].Document_To_Date - ScanIndexList[i].Document_Date).Days + 1);
                    }
                }
                iMySession.Close();
            }
            return iTotalNoofLineItem;
            //query1 = session.GetISession().GetNamedQuery("Get.Total.Encounter.in.Batch");
            //query1.SetString(0, ulWorksetID.ToString());

            //return Convert.ToInt32(query1.List()[0]);
        }

        public IList<Orders> GetOutStandingOrders(ulong humanID)
        {
            IList<Orders> outstandingOrderList = new List<Orders>();


            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq = iMySession.CreateSQLQuery("select o.* from orders o join orders_submit os on os.order_submit_id = o.order_submit_id join wf_object wf on wf.obj_system_id = os.order_submit_id where wf.current_process = 'ORDER GENERATE' and wf.obj_type = 'DIAGNOSTIC ORDER' and  os.human_id ='" + humanID + "';")
                      .AddEntity("O", typeof(Orders));
                outstandingOrderList = sq.List<Orders>();

                iMySession.Close();
            }


            return outstandingOrderList;

        }

        public IList<scan_index> GetScanIndexForHuman(ulong Human_Id)
        {
            IList<scan_index> ilstScanIndex = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Human_ID", Human_Id));
                ilstScanIndex = criteria.List<scan_index>();
                iMySession.Close();
            }
            return ilstScanIndex;

        }


        public IList<scan_index> GetScanIndexDetailsForOrderID(ulong uScanID)
        {
            IList<scan_index> lstScnIndex = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Order_ID", uScanID));
                lstScnIndex = crit.List<scan_index>();
                iMySession.Close();
            }
            return lstScnIndex;
        }
        public IList<scan_index> GetScanIndexDetailsForScanIndexConversionID(ulong uIndexConScanID)
        {
            IList<scan_index> lstScnIndex = new List<scan_index>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Id", uIndexConScanID));
                lstScnIndex = crit.List<scan_index>();
                iMySession.Close();
            }
            return lstScnIndex;
        }

        public void SaveUpdateScanIndexforDeleteFiles(IList<scan_index> lstScanIndex)
        {
            IList<scan_index> lstScanIndexnull = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstScanIndexnull, ref lstScanIndex, null, string.Empty, false, false, 0, string.Empty);
        }


        //public IList<scan_index> GetPageNumbersforThumbNailDelete(string filename)
        //{
        //    IList<scan_index> outstandingOrderList = new List<scan_index>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ISQLQuery sq = iMySession.CreateSQLQuery("select sic.* from scan s join scan_index_conversion sic on s.scan_id = sic.scan_id where s.scanned_file_name = '" + filename + "';")
        //              .AddEntity("sci", typeof(scan_index));
        //        outstandingOrderList = sq.List<scan_index>();
        //        iMySession.Close();
        //    }
        //    return outstandingOrderList;
        //}
    }

}


