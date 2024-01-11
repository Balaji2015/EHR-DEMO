using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.IO;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IScanManager : IManagerBase<Scan, ulong>
    {
        void SaveUpdateDeleteScan(IList<Scan> saveList, IList<Scan> updateList, IList<Scan> deleteList, IList<WFObject> wfObjList, string macAddress);
        IList<Scan> GetScanDocumentsList(string facility, DateTime date);
        IList<Scan> GetScanDocumentsListByUser(string facility, DateTime date, string user);
        IList<Scan> GetScanDocumentsListByID(ulong scanID);
        void Scan_FileCopy(string facility);
        IList<scan_index> Update_ScanFiles_ToDB_And_ToFolder(string facility);
        IList<Scan> Check_duplicate_Files(string facility);
        IList<Scan> UnMovedFiles(string facility);
        IList<Scan> GetOnlineDocumentsList(string facility, IList<string> online_document_list, DateTime document_date);
        IList<Scan> GetScanDocumentsByScanFileNameList(List<string> ScnFileName);
        //bool GetOnlineDocumentsList(string facility, string online_document_list);
        int GetScanDocumentsByScanFilePathDeleteCheck(string ScnFilePath);
    }
    public partial class ScanManager : ManagerBase<Scan, ulong>, IScanManager
    {
        #region Constructors


        public ScanManager()
            : base()
        {

        }
        public ScanManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods
        int iTryCount = 0;

        public IList<Scan> GetscanbyId(ulong sanid)
        {
            IList<Scan> lstscn = new List<Scan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Id", Convert.ToInt32(sanid)));
                lstscn = criteria.List<Scan>();
                iMySession.Close();
            }
            return lstscn;

        }
        public void Scan_FileCopy(string facility)
        {

            ///////////////////////////
            IList<Scan> lst_Scan = new List<Scan>();
            IList<scan_index> lst_scan_index = new List<scan_index>();
            //ISession MySession = Session.GetISession();
            using (ISession MySession = Session.GetISession())
            {
                ICriteria crit = MySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", "INDEX")).Add(Expression.Eq("Fac_Name", facility));
                IList<WFObject> lstWFObject = crit.List<WFObject>();


                for (int i = 0; i < lstWFObject.Count; i++)
                {
                    object obj = MySession.Get(typeof(Scan), Convert.ToInt32(lstWFObject[i].Obj_System_Id));
                    Scan obj_scan = (Scan)obj;

                    string[] arr = obj_scan.Scanned_File_Path.Trim().Split('\\');

                    DirectoryInfo dir = new DirectoryInfo(obj_scan.Scanned_File_Path.Substring(0, obj_scan.Scanned_File_Path.LastIndexOf("\\")));
                    if (!dir.Exists)
                        dir.Create();

                    if (!File.Exists(obj_scan.Scanned_File_Path.Substring(0, obj_scan.Scanned_File_Path.LastIndexOf("\\")) + "\\" + obj_scan.Scanned_File_Name))
                        File.Copy(arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + "1904 N OG AVE_Online_Chart_20120523_9009_01.tif", obj_scan.Scanned_File_Path.Substring(0, obj_scan.Scanned_File_Path.LastIndexOf("\\")) + "\\" + obj_scan.Scanned_File_Name);





                    ICriteria crit_Scan_Index = MySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id)); ;
                    IList<scan_index> lstScan_Index = crit_Scan_Index.List<scan_index>();



                    for (int k = 0; k < lstScan_Index.Count; k++)
                    {

                        string filename = lstScan_Index[k].Indexed_File_Path.Remove(0, lstScan_Index[k].Indexed_File_Path.LastIndexOf("\\") + 1);

                        DirectoryInfo directory = new DirectoryInfo(lstScan_Index[k].Indexed_File_Path.Substring(0, lstScan_Index[k].Indexed_File_Path.LastIndexOf("\\")));
                        if (!directory.Exists)
                            directory.Create();

                        if (!File.Exists(lstScan_Index[k].Indexed_File_Path.Substring(0, lstScan_Index[k].Indexed_File_Path.LastIndexOf("\\")) + "\\" + filename))
                            File.Copy(arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + "1904 N OG AVE_Online_Chart_20120523_9009_01.tif", lstScan_Index[k].Indexed_File_Path.Substring(0, lstScan_Index[k].Indexed_File_Path.LastIndexOf("\\")) + "\\" + filename);


                    }


                }
                MySession.Close();
            }


            ////////////////////////////

        }


        public IList<scan_index> Update_ScanFiles_ToDB_And_ToFolder(string facility)
        {
            ///////////////////

            IList<Scan> lst_Scan = new List<Scan>();
            IList<scan_index> lst_scan_index = new List<scan_index>();
            ISession current_session = Session.GetISession();
            ITransaction trans = null;
            trans = current_session.BeginTransaction();
            ICriteria crit = current_session.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", "INDEX")).Add(Expression.Eq("Fac_Name", facility)); ;
            IList<WFObject> lstWFObject = crit.List<WFObject>();
            for (int i = 0; i < lstWFObject.Count; i++)
            {



                ////Scan


                ICriteria crit_scan = current_session.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id));
                Scan obj_scan = crit_scan.List<Scan>()[0];


                string move_filename = obj_scan.Scanned_File_Name;
                string source_Path = obj_scan.Scanned_File_Path;
                obj_scan.Scan_ID = obj_scan.Scan_ID;


                string[] arr = obj_scan.Scanned_File_Path.Trim().Split('\\');
                ////  For Local  obj_scan.Scanned_File_Path = arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[5] + "\\" + obj_scan.Scanned_File_Name;

                obj_scan.Scanned_File_Path = arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + arr[5] + "\\" + arr[8] + "\\" + obj_scan.Scanned_File_Name;

                lst_Scan.Add(obj_scan);


                DirectoryInfo dir = new DirectoryInfo(arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + arr[5] + "\\" + arr[8]);
                if (!dir.Exists)
                    dir.Create();
                if (!File.Exists(arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + arr[5] + "\\" + arr[8] + "\\" + obj_scan.Scanned_File_Name))
                    File.Copy(source_Path, arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + arr[5] + "\\" + arr[8] + "\\" + obj_scan.Scanned_File_Name);


                // //End
                // //Scan index Conversion

                ICriteria crit_index = current_session.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id));
                IList<scan_index> lstWF_Index_conversion = crit_index.List<scan_index>();
                for (int k = 0; k < lstWF_Index_conversion.Count; k++)
                {

                    scan_index obj_scan_index = new scan_index();
                    obj_scan_index.Id = lstWF_Index_conversion[k].Id;
                    obj_scan_index.Scan_ID = lstWF_Index_conversion[k].Scan_ID;
                    string file_name = string.Empty;
                    string[] index_arr = lstWF_Index_conversion[k].Indexed_File_Path.Split('\\');

                    file_name = index_arr[10];



                    //// For localobj_scan_index.Indexed_File_Path = index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[5] + "\\" + index_arr[6] + "\\" + file_name;

                    obj_scan_index.Indexed_File_Path = index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9] + "\\" + file_name;

                    lst_scan_index.Add(obj_scan_index);

                    DirectoryInfo dir_index = new DirectoryInfo(index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9]);
                    if (!dir_index.Exists)
                        dir_index.Create();
                    if (!File.Exists(index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9] + "\\" + file_name))
                        File.Copy(lstWF_Index_conversion[k].Indexed_File_Path, index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9] + "\\" + file_name);

                }

                ////End




            }



            IList<Scan> scanList = new List<Scan>();


            SaveUpdateDeleteWithTransaction(ref scanList, lst_Scan, null, "002564B0B5D6");



            return lst_scan_index;

            /////////////////////
        }





        //||||||| 1.18
        //=======


        //        public void Scan_FileCopy(string facility)
        //        {

        //            ///////////////////////////
        //            IList<Scan> lst_Scan = new List<Scan>();
        //            IList<scan_index> lst_scan_index = new List<scan_index>();
        //            ISession MySession = Session.GetISession();




        //            ICriteria crit = MySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", "INDEX")).Add(Expression.Eq("Fac_Name", facility));
        //            IList<WFObject> lstWFObject = crit.List<WFObject>();


        //            for (int i = 0; i < lstWFObject.Count; i++)
        //            {
        //                object obj = MySession.Get(typeof(Scan), Convert.ToInt32(lstWFObject[i].Obj_System_Id));
        //                Scan obj_scan = (Scan)obj;

        //                string[] arr = obj_scan.Scanned_File_Path.Trim().Split('\\');

        //                DirectoryInfo dir = new DirectoryInfo(obj_scan.Scanned_File_Path.Substring(0, obj_scan.Scanned_File_Path.LastIndexOf("\\")));
        //                if (!dir.Exists)
        //                    dir.Create();

        //                if (!File.Exists(obj_scan.Scanned_File_Path.Substring(0, obj_scan.Scanned_File_Path.LastIndexOf("\\")) + "\\" + obj_scan.Scanned_File_Name))
        //                    File.Copy(arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + "1904 N OG AVE_Online_Chart_20120523_9009_01.tif", obj_scan.Scanned_File_Path.Substring(0, obj_scan.Scanned_File_Path.LastIndexOf("\\")) + "\\" + obj_scan.Scanned_File_Name);





        //                ICriteria crit_Scan_Index = MySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id)); ;
        //                IList<scan_index> lstScan_Index = crit_Scan_Index.List<scan_index>();



        //                for (int k = 0; k < lstScan_Index.Count; k++)
        //                {

        //                    string filename = lstScan_Index[k].Indexed_File_Path.Remove(0, lstScan_Index[k].Indexed_File_Path.LastIndexOf("\\") + 1);

        //                    DirectoryInfo directory = new DirectoryInfo(lstScan_Index[k].Indexed_File_Path.Substring(0, lstScan_Index[k].Indexed_File_Path.LastIndexOf("\\")));
        //                    if (!directory.Exists)
        //                        directory.Create();

        //                    if (!File.Exists(lstScan_Index[k].Indexed_File_Path.Substring(0, lstScan_Index[k].Indexed_File_Path.LastIndexOf("\\")) + "\\" + filename))
        //                        File.Copy(arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + "1904 N OG AVE_Online_Chart_20120523_9009_01.tif", lstScan_Index[k].Indexed_File_Path.Substring(0, lstScan_Index[k].Indexed_File_Path.LastIndexOf("\\")) + "\\" + filename);


        //                }


        //            }


        //            ////////////////////////////

        //        }


        //        public IList<scan_index> Update_ScanFiles_ToDB_And_ToFolder(string facility)
        //        {
        //            ///////////////////

        //             IList<Scan> lst_Scan = new List<Scan>();
        //             IList<scan_index> lst_scan_index = new List<scan_index>();
        //             ISession current_session = Session.GetISession();
        //             ITransaction trans = null;
        //             trans = current_session.BeginTransaction();
        //            ICriteria crit = current_session.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", "INDEX")).Add(Expression.Eq("Fac_Name", facility)); ;
        //            IList<WFObject> lstWFObject = crit.List<WFObject>();
        //            for (int i = 0; i < lstWFObject.Count; i++)
        //            {



        //                ////Scan


        //                ICriteria crit_scan = current_session.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id));
        //                Scan obj_scan = crit_scan.List<Scan>()[0];


        //                string move_filename = obj_scan.Scanned_File_Name;
        //                string source_Path = obj_scan.Scanned_File_Path;
        //                obj_scan.Scan_ID = obj_scan.Scan_ID;


        //                string[] arr = obj_scan.Scanned_File_Path.Trim().Split('\\');
        //              ////  For Local  obj_scan.Scanned_File_Path = arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[5] + "\\" + obj_scan.Scanned_File_Name;

        //                obj_scan.Scanned_File_Path = arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + arr[5] + "\\" + arr[8] + "\\" + obj_scan.Scanned_File_Name;

        //                lst_Scan.Add(obj_scan);


        //                DirectoryInfo dir = new DirectoryInfo(arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + arr[5] + "\\" + arr[8]);
        //                if (!dir.Exists)
        //                    dir.Create();
        //                if (!File.Exists(arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + arr[5] + "\\" + arr[8] + "\\" + obj_scan.Scanned_File_Name))
        //                    File.Copy(source_Path, arr[0] + "\\" + arr[1] + "\\" + arr[2] + "\\" + arr[3] + "\\" + arr[4] + "\\" + arr[5] + "\\" + arr[8] + "\\" + obj_scan.Scanned_File_Name);


        //               // //End
        //               // //Scan index Conversion

        //                ICriteria crit_index = current_session.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id));
        //                IList<scan_index> lstWF_Index_conversion = crit_index.List<scan_index>();
        //                for (int k = 0; k < lstWF_Index_conversion.Count; k++)
        //                {

        //                    scan_index obj_scan_index = new scan_index();
        //                    obj_scan_index.Id = lstWF_Index_conversion[k].Id;
        //                    obj_scan_index.Scan_ID = lstWF_Index_conversion[k].Scan_ID;
        //                    string file_name = string.Empty;
        //                    string[] index_arr = lstWF_Index_conversion[k].Indexed_File_Path.Split('\\');

        //                    file_name = index_arr[10];



        //                    //// For localobj_scan_index.Indexed_File_Path = index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[5] + "\\" + index_arr[6] + "\\" + file_name;

        //                    obj_scan_index.Indexed_File_Path = index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9] + "\\" + file_name;

        //                    lst_scan_index.Add(obj_scan_index);

        //                    DirectoryInfo dir_index = new DirectoryInfo(index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9]);
        //                    if (!dir_index.Exists)
        //                        dir_index.Create();
        //                    if (!File.Exists(index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9] + "\\" + file_name))
        //                        File.Copy(lstWF_Index_conversion[k].Indexed_File_Path, index_arr[0] + "\\" + index_arr[1] + "\\" + index_arr[2] + "\\" + index_arr[3] + "\\" + index_arr[4] + "\\" + index_arr[5] + "\\" + index_arr[8] + "\\" + index_arr[9] + "\\" + file_name);

        //                }

        //                ////End




        //            }



        //            IList<Scan> scanList = new List<Scan>();


        //          SaveUpdateDeleteWithTransaction(ref scanList, lst_Scan, null,"002564B0B5D6");



        //              return lst_scan_index;

        //            /////////////////////
        //        }

        public IList<Scan> Check_duplicate_Files(string facility)
        {



            IList<Scan> lst_Scan = new List<Scan>();

            ISession current_session = Session.GetISession();
            ITransaction trans = null;
            trans = current_session.BeginTransaction();
            ICriteria crit = current_session.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", "INDEX")).Add(Expression.Eq("Fac_Name", facility)); ;
            IList<WFObject> lstWFObject = crit.List<WFObject>();


            for (int i = 0; i < lstWFObject.Count; i++)
            {

                ICriteria crit_scan = current_session.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id));
                Scan obj_scan = crit_scan.List<Scan>()[0];

                if (!File.Exists(obj_scan.Scanned_File_Path))
                {
                    obj_scan.Scanned_File_Name = obj_scan.Scanned_File_Name + "NO";
                    lst_Scan.Add(obj_scan);
                }
                else
                {
                    obj_scan.Scanned_File_Name = obj_scan.Scanned_File_Name + "YES";
                    lst_Scan.Add(obj_scan);
                }

            }





            return lst_Scan;
        }

        public IList<Scan> UnMovedFiles(string facility)
        {


            IList<Scan> lst_Scan = new List<Scan>();
            IList<scan_index> lst_scan_index = new List<scan_index>();
            ISession MySession = Session.GetISession();

            ICriteria crit = MySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", "INDEX")).Add(Expression.Eq("Fac_Name", facility));
            IList<WFObject> lstWFObject = crit.List<WFObject>();


            for (int i = 0; i < lstWFObject.Count; i++)
            {

                ICriteria crit_scan = MySession.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Scan_ID", lstWFObject[i].Obj_System_Id));
                Scan obj_scan = crit_scan.List<Scan>()[0];
                string filePath = System.Configuration.ConfigurationSettings.AppSettings[facility.Replace(" ", "_") + "_ScanningPath"] + "\\" + facility + "\\" + "Scanned_Images\\";
                if (!File.Exists(filePath + "\\" + obj_scan.Scanned_File_Name))
                {

                }
            }





            return lst_Scan;
        }



        public void SaveUpdateDeleteScan(IList<Scan> saveList, IList<Scan> updateList, IList<Scan> deleteList, IList<WFObject> wfObjList, string macAddress)
        {
            iTryCount = 0;
            if (saveList.Count > 0)
            {
                for (int i = 0; i < saveList.Count; i++)
                {
                TryAgain:
                    int iResult = 0;
                    //ISession MySession = Session.GetISession();
                    using (ISession MySession = Session.GetISession())
                    {
                        ITransaction trans = null;
                        IList<Scan> scanList = new List<Scan>();
                        IList<Scan> scansavedlst = new List<Scan>();
                        try
                        {
                            trans = MySession.BeginTransaction();
                            if (saveList != null && saveList.Count > 0)
                            {
                                ICriteria crit = MySession.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Facility_Name", saveList[0].Facility_Name));
                                scansavedlst = crit.List<Scan>();
                            }
                            if (scansavedlst != null && scansavedlst.Count > 0)
                            {

                                if (!scansavedlst.Any(a => a.Scanned_File_Name.Trim() == saveList[i].Scanned_File_Name && a.No_of_Pages == saveList[i].No_of_Pages && a.Scan_Type.Trim() == saveList[i].Scan_Type.Trim()))
                                {
                                    scanList.Add(saveList.ElementAt<Scan>(i));
                                }



                            }
                            else
                            {
                                foreach (Scan item in saveList)
                                {
                                    scanList.Add(saveList.ElementAt<Scan>(i));
                                }
                            }


                            if (scanList != null && scanList.Count > 0)
                            {
                                iResult = SaveUpdateDeleteWithoutTransaction(ref scanList, null, null, MySession, macAddress);
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

                            if (scanList != null && scanList.Count > 0)
                            {
                                WFObject currentwfObject = new WFObject();

                                currentwfObject = wfObjList.ElementAt<WFObject>(i);
                                currentwfObject.Obj_System_Id = Convert.ToUInt32(scanList[0].Id);
                                WFObjectManager wfObjectManager = new WFObjectManager();

                                iResult = wfObjectManager.InsertToWorkFlowObject(currentwfObject, scanList[0].Close_Type, macAddress, MySession);
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
                        //MySession.Close();
                    }

                }
            }
            else if (updateList.Count > 0)
            {
                for (int i = 0; i < updateList.Count; i++)
                {
                TryAgain:
                    int iResult = 0;
                    //ISession MySession = Session.GetISession();
                    using (ISession MySession = Session.GetISession())
                    {
                        ITransaction trans = null;
                        IList<Scan> scanList = new List<Scan>();
                        IList<Scan> nullList = null;
                        try
                        {
                            trans = MySession.BeginTransaction();
                            scanList.Add(updateList.ElementAt<Scan>(i));
                            iResult = SaveUpdateDeleteWithoutTransaction(ref nullList, scanList, null, MySession, macAddress);
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
                                    throw new Exception("Deadlock is occured. Transaction failed");

                                }
                            }
                            else if (iResult == 1)
                            {

                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Exception is occured. Transaction failed");

                            }
                            MySession.Flush();
                            trans.Commit();
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
                        MySession.Close();
                    }
                }
            }
            else if (deleteList.Count > 0)
            {
                for (int i = 0; i < deleteList.Count; i++)
                {
                TryAgain:
                    int iResult = 0;
                    //ISession MySession = Session.GetISession();
                    using (ISession MySession = Session.GetISession())
                    {
                        ITransaction trans = null;
                        IList<Scan> scanList = new List<Scan>();
                        IList<Scan> nullList = null;
                        try
                        {
                            trans = MySession.BeginTransaction();
                            scanList.Add(deleteList.ElementAt<Scan>(i));
                            iResult = SaveUpdateDeleteWithoutTransaction(ref nullList, null, scanList, MySession, macAddress);
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
                            MySession.Flush();
                            trans.Commit();
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
                        MySession.Close();
                    }
                }
            }


        }

        public IList<Scan> GetScanDocumentsList(string facility, DateTime date)
        {
            IList<Scan> listscan = new List<Scan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select s.* from scan s where s.Scanned_Date like '%" + date.ToString("yyyy-MM-dd") + "%' and s.Facility_Name='" + facility + "'").AddEntity("s", typeof(Scan));
                listscan = sql.List<Scan>();
                iMySession.Close();
            }
            return listscan;
        }
        public IList<Scan> GetScanDocumentsByScanFileNameList(List<string> ScnFileName)
        {
            IList<Scan> listscan = new List<Scan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Scan)).Add(Expression.In("Scanned_File_Name", ScnFileName));
                listscan = criteria.List<Scan>();
                iMySession.Close();
            }
            return listscan;
        }
        public IList<Scan> GetScanDocumentsListByID(ulong scanID)
        {
            IList<Scan> listscan = new List<Scan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Scan_ID", scanID));
                listscan = crit.List<Scan>();
                iMySession.Close();
            }
            return listscan;
        }


        public IList<Scan> GetScanDocumentsListByUser(string facility, DateTime date, string user)
        {
            IList<Scan> listscan = new List<Scan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select s.* from scan s where s.Scanned_Date like '" + date.ToString("yyyy-MM-dd") + "%' and s.Facility_Name='" + facility + "' and s.Created_By='" + user + "' ").AddEntity("s", typeof(Scan));
                listscan = sql.List<Scan>();
                iMySession.Close();
            }
            return listscan;
        }

        public IList<Scan> GetOnlineDocumentsList(string facility, IList<string> online_document_list, DateTime document_date)
        {
            IList<Scan> lstDocuments = new List<Scan>();
            IList<string> lstScanFinal = new List<string>();
            if (online_document_list.Count > 0)
            {
                //var sFileName = string.Join(",", online_document_list.Select(a => String.Format("\"{0}\"", a)).ToArray<string>());

                IList<string> temp_lstDocuments = new List<string>();
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    // ISQLQuery crit = iMySession.CreateSQLQuery("select s.Scanned_File_Name from scan s where s.Facility_name = '" + facility + "' and s.Scanned_File_Name like '%ONLINE%" + document_date.ToString("yyyy-MM-dd") + "%' "); //Old query
                    //ISQLQuery crit = iMySession.CreateSQLQuery("select s.Scanned_File_Name from scan s where s.Facility_name = '" + facility + "' and s.Scanned_Date like '" + document_date.ToString("yyyy-MM-dd") + "%' and Scanned_File_Name in (" + sFileName + ")"); //New query
                    ISQLQuery crit = iMySession.CreateSQLQuery("select s.Scanned_File_Name from scan s where s.Facility_name = '" + facility + "' and date(s.Scanned_Date) = '" + document_date.ToString("yyyy-MM-dd") +"'");
                    temp_lstDocuments = crit.List<string>();

                    if (temp_lstDocuments != null && temp_lstDocuments.Count != 0)
                    {
                        lstScanFinal = online_document_list.Except(temp_lstDocuments).ToList<string>();
                        for (int i = 0; i < lstScanFinal.Count; i++)
                        {
                            Scan objscan = new Scan();
                            objscan.Scan_ID = 0;
                            objscan.Scanned_File_Name = lstScanFinal[i];
                            lstDocuments.Add(objscan);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < online_document_list.Count; i++)
                        {
                            Scan objscan = new Scan();
                            objscan.Scan_ID = 0;
                            objscan.Scanned_File_Name = online_document_list[i];
                            lstDocuments.Add(objscan);
                        }
                    }
                    
                    /*
                    if (temp_lstDocuments != null && temp_lstDocuments.Count == 0)
                    {
                        for (int i = 0; i < online_document_list.Count; i++)
                        {
                            Scan objscan = new Scan();
                            objscan.Scan_ID = 0;
                            objscan.Scanned_File_Name = online_document_list[i];
                            lstDocuments.Add(objscan);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < online_document_list.Count; i++)
                        {
                            if (!temp_lstDocuments.Any(a => a.ToString().Trim().ToUpper().Contains(online_document_list[i].Trim().ToUpper())))
                            {
                                Scan objscan = new Scan();
                                objscan.Scan_ID = 0;
                                objscan.Scanned_File_Name = online_document_list[i];
                                lstDocuments.Add(objscan);
                            }

                        }
                    }*/
                    iMySession.Close();
                }
            }
            return lstDocuments;

        }


        public int GetScanDocumentsByScanFilePathDeleteCheck(string ScnFilePath)
        {
            int flag = 0;
            IList<Scan> listscan = new List<Scan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Scanned_File_Path", ScnFilePath));
                listscan = criteria.List<Scan>();
                iMySession.Close();
            }
            List<int> uscanid = new List<int>();
            IList<scan_index> lstscanindex = new List<scan_index>();
            if (listscan.Count > 0)
            {
                uscanid = listscan.Select(a => a.Id).ToList<int>();
                IScan_IndexManager objscanindex = new Scan_IndexManager();
                lstscanindex = objscanindex.GetscanIndexList(uscanid);
                if (lstscanindex.Count > 0)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;

                }
            }
            else
            {
                flag = 0;
            }
            return flag;
            //return listscan;
        }

        //public bool GetOnlineDocumentsList(string facility, string online_document_list)
        //{
        //    bool sCheck = false;
        //    IList<string> temp_lstDocuments = new List<string>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ISQLQuery crit = iMySession.CreateSQLQuery("select s.Scanned_File_Name from scan s where s.Facility_name = '" + facility + "' and s.Scanned_File_Name ='" + online_document_list + "'");
        //        temp_lstDocuments = crit.List<string>();

        //        if (temp_lstDocuments.Any(a => a.ToString().Trim().ToUpper().Contains(online_document_list.Trim().ToUpper())))
        //        {
        //            sCheck = true;
        //        }
        //        iMySession.Close();
        //    }

        //    return sCheck;

        //}
        #endregion
    }
}
