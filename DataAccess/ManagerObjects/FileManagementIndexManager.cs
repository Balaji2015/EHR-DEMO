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
using System.IO;
using System.Net;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IFileManagementIndexManager : IManagerBase<FileManagementIndex, ulong>
    {


        FileManagementDTO LoadFileIndex_and_ViewResult(ulong ulEncounterID, ulong ulHumanID, ulong ulOrder_Id, string Source, bool IsCmg);
        void SaveUpdateDeleteFileManagementIndexForOnline_and_Wfobject(IList<FileManagementIndex> addfileList, ulong[] scanID, string macAddress, DateTime transcationTime);
        IList<FileManagementIndex> GetIndexedListByHumanId(ulong ulhumanID, string Source);
        void SaveUpdateDeleteFileManagementIndex(string macAddress, string ftpUserID, string ftpPassword, string ftpServerIP);
        void SaveUpdateDeleteFileManagementIndexForOnline(IList<FileManagementIndex> addfileList, IList<FileManagementIndex> updatefileList, IList<FileManagementIndex> deletefileList, string macAddress);
        FileManagementIndex GetListUsingOrderSubmitAndDocType(ulong order_submit_id);
        //Added by bala welness notes image upload
        IList<FileManagementIndex> GetIndexedListBySource(ulong ulhumanID, ulong ulEncounterID, string Source);

        ///Vince - 03-May-2012 for Exam Photos ///////////////
        IList<FileManagementIndex> SaveUpdateDeleteFileManagementIndexforExamPhotos(IList<FileManagementIndex> addfileList, IList<FileManagementIndex> updatefileList, IList<FileManagementIndex> deletefileList, string macAddress, string Source);
        ///Vince - 03-May-2012 ///////////////
        /// /////Added by manimozhi 13th- Aug-2012
        IList<FileManagementIndex> GetScannedObjectByScanIndexConversionID(ulong ulhumanID, ulong ScanIndexConversionID);


        ///added by vince for Exam Automation on 14-Aug-2012//////
        IList<FileManagementIndex> GetListBySource(string Source);

        /// /////Added by manimozhi 29th-Nov-2012
        IList<FileManagementIndex> GetListOfOrdersNotTied(ulong ulHumanID, string sDocType, string sDocSubType);
        int UpdateFilemanagementIndexforMRE(IList<FileManagementIndex> UpdateFileManagementIndex, ISession MySession, string macaddress);

        IList<FileManagementIndex> GetImagesforAnnotations(ulong ulHumanID, ulong ulOrder_Id, string Source);
        IList<FileManagementIndex> GetListforExamandInhouse(ulong ulHumanID, ulong ulOrder_Id);

        IList<FileManagementIndex> GetFileList(ulong human_id);
        IList<FileManagementIndex> GetEncountersFileList(ulong human_id);
        
        FileManagementDTO GetResultandExamListusing(ulong human_id, ulong order_submit_id);  

        IList<FileManagementIndex> GetCopyPreviousBodyImage(ulong phy_Id, ulong Human_Id, ulong enc_Id);

        FileManagementDTO GetResultandExamListusingOrderSubmitId(ulong orderSubmitId, ulong humanId);

        FileManagementDTO SaveUpdateDelete_FileManagementIndex_Abi_Spirometry(FileManagementDTO ObjSave, FileManagementDTO ObjUpdate, FileManagementDTO ObjDelete, string macAddress);

        IList<FileManagementIndex> SaveUpdatDeleteForCMG(IList<FileManagementIndex> addfileList, IList<FileManagementIndex> updatefileList, IList<FileManagementIndex> deletefileList, string macAddress);

        IList<FileManagementIndex> GetBodyImageList(ulong ulHumanID, ulong ulOrder_Id, string Source);
        IList<FileManagementIndex> GetListbySourceAndHumanId(ulong human_id, string Source);
        //Added by srividhya on 10-mar-2014
        int GetFileMgntIndexCount(ulong ulBatchID);
        //Added By Suvarnni
        IList<FileManagementIndex> GetFileCount(ulong ulhumanID, string Source, string sDocType, string sDocSubType);
        IList<FileManagementIndex> GetFileListUsingHumanID(ulong human_id);
        void SaveFileManagementIndexforNonMedicalFiles(IList<FileManagementIndex> lstFilemngt);
        IList<FileManagementIndex> GetFileListUsingFileIndexID(ulong index_id);
        void UpdateFileManagementIndexforIndexing(IList<FileManagementIndex> lstFilemngt);
        IList<FileManagementIndex> GetFileListUsingScanIndexID(ulong Scanindex_id);
        void SaveFileManagementIndexforDeleteFiles(IList<FileManagementIndex> lstFilemngt);
    }

    public partial class FileManagementIndexManager : ManagerBase<FileManagementIndex, ulong>, IFileManagementIndexManager
    {
        #region Constructors


        public FileManagementIndexManager()
            : base()
        {

        }
        public FileManagementIndexManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods
        public IList<FileManagementIndex> GetIndexedListByHumanId(ulong ulhumanID, string Source)
        {
            //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            IList<FileManagementIndex> fileMangList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulhumanID)).Add(Expression.In("Source", Source.Split(',').Where(A => A != string.Empty && A != null).ToArray()));
                //IList<FileManagementIndex> fileMangList = crit.List<FileManagementIndex>();
                fileMangList = crit.List<FileManagementIndex>();
                mySession.Close();
            }
            return fileMangList;

        }

        public void SaveUpdateDeleteFileManagementIndex(string macAddress, string ftpUserID, string ftpPassword, string ftpServerIP)
        {

            ArrayList fileList = new ArrayList();
            ArrayList scanPath = new ArrayList();
            ArrayList indexPath = new ArrayList();
            IList<scan_index> getIndexList = new List<scan_index>();
            IList<FileManagementIndex> InsertFileManagementList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IList<WFObject> attachToPatientList = mySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", "ATTACH_TO_PATIENT")).List<WFObject>();
                if (attachToPatientList.Count > 0)
                {
                    for (int k = 0; k < attachToPatientList.Count; k++)
                    {
                        IList<Scan> scanList = mySession.CreateCriteria(typeof(Scan)).Add(Expression.Eq("Scan_ID", attachToPatientList[k].Obj_System_Id)).List<Scan>();
                        scanPath.Add(scanList[0].Scanned_File_Path);
                        IList<scan_index> tempIndexList = mySession.CreateCriteria(typeof(scan_index)).Add(Expression.Eq("Scan_ID", attachToPatientList[k].Obj_System_Id)).List<scan_index>();
                        if (tempIndexList.Count > 0)
                        {
                            for (int j = 0; j < tempIndexList.Count; j++)
                            {
                                getIndexList.Add(tempIndexList.ElementAt<scan_index>(j));
                                indexPath.Add(tempIndexList[j].Indexed_File_Path);
                            }
                        }
                    }
                }
                if (getIndexList.Count > 0)
                {
                    for (int i = 0; i < getIndexList.Count; i++)
                    {
                        string uri = ftpServerIP + getIndexList[i].Human_ID + "/";
                        FtpWebRequest reqFTP;
                        WebResponse responseFTP;
                        try
                        {
                            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                            reqFTP.KeepAlive = false;
                            reqFTP.UsePassive = false;
                            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                            responseFTP = (WebResponse)reqFTP.GetResponse();
                            StreamReader readerForFTP = new StreamReader(responseFTP.GetResponseStream());
                            string lsdirectory = readerForFTP.ReadLine();
                            while (lsdirectory != null)
                            {
                                fileList.Add(lsdirectory);
                                lsdirectory = readerForFTP.ReadLine();
                            }
                        }
                        catch (WebException ex)
                        {
                            FtpWebResponse response = (FtpWebResponse)ex.Response;
                            if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                            {

                            }
                        }
                        if (fileList.Count > 0)
                        {
                            for (int j = 0; j < fileList.Count; j++)
                            {
                                if (fileList[j].ToString() == getIndexList[i].Indexed_File_Path.Substring(getIndexList[i].Indexed_File_Path.LastIndexOf("\\") + 1))
                                {
                                    FileManagementIndex tempObject = new FileManagementIndex();
                                    tempObject.Created_By = getIndexList[i].Created_By;
                                    tempObject.Created_Date_And_Time = DateTime.Now;
                                    tempObject.Document_Date = getIndexList[i].Document_Date;
                                    tempObject.Document_Sub_Type = getIndexList[i].Document_Sub_Type;
                                    tempObject.Document_Type = getIndexList[i].Document_Type;
                                    tempObject.File_Path = uri + fileList[j].ToString();
                                    tempObject.Human_ID = getIndexList[i].Human_ID;
                                    tempObject.Scan_Index_Conversion_ID = getIndexList[i].Id;
                                    tempObject.Order_ID = getIndexList[i].Order_ID;
                                    tempObject.Source = "SCAN";
                                    InsertFileManagementList.Add(tempObject);
                                    break;
                                }
                            }
                        }
                    }
                }

                if (InsertFileManagementList.Count > 0)
                {
                    SaveUpdateDeleteWithTransaction(ref InsertFileManagementList, null, null, macAddress);
                    if (attachToPatientList.Count > 0)
                    {
                        int iTryCount = 0;
                        for (int i = 0; i < attachToPatientList.Count; i++)
                        {
                        TryAgain:
                            int iResult = 0;
                            ISession MySession = Session.GetISession();
                            // ITransaction trans = null;
                            try
                            {
                                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                                {
                                    try
                                    {
                                        //trans = MySession.BeginTransaction();

                                        WFObject currentwfObject = new WFObject();
                                        currentwfObject = attachToPatientList.ElementAt<WFObject>(i);
                                        WFObjectManager wfObjectManager = new WFObjectManager();
                                        iResult = wfObjectManager.MoveToNextProcess(currentwfObject.Obj_System_Id, currentwfObject.Obj_Type, 1, "UNKNOWN", DateTime.Now, macAddress, null, session.GetISession());
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

                    //if (scanPath.Count > 0)
                    //{
                    //    for (int i = 0; i < scanPath.Count; i++)
                    //    {
                    //        FileInfo scanPathInfo = new FileInfo(scanPath[i].ToString());
                    //        if (scanPathInfo.Exists)
                    //        {
                    //            DirectoryInfo scanDir = scanPathInfo.Directory;
                    //            scanPathInfo.Delete();
                    //            if (scanDir.GetFiles("*.tif").Length == 0)
                    //            {
                    //                DirectoryInfo oldChartsDir = scanDir.Parent;
                    //                scanDir.Delete(true);
                    //                if (oldChartsDir.GetDirectories().Length == 0)
                    //                {
                    //                    DirectoryInfo dateDir = oldChartsDir.Parent;
                    //                    oldChartsDir.Delete(true);
                    //                    if (dateDir.GetDirectories().Length == 0)
                    //                        dateDir.Delete(true);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    /////////////////

                    //if (indexPath.Count > 0)
                    //{
                    //    for (int i = 0; i < indexPath.Count; i++)
                    //    {
                    //        FileInfo indexPathInfo = new FileInfo(indexPath[i].ToString());
                    //        if (indexPathInfo.Exists)
                    //        {
                    //            string productionpath = indexPathInfo.FullName.Substring(indexPathInfo.FullName.IndexOf("\\Old_Charts") - 8, 8);
                    //            FileInfo moveToProductionInfo = new FileInfo(indexPath[i].ToString().Replace(productionpath + "\\Old_Charts\\Indexed_Images", DateTime.Now.ToString("yyyy-MM-dd") + "\\Move To Production"));
                    //            DirectoryInfo productionhumandir = moveToProductionInfo.Directory;
                    //            moveToProductionInfo.Delete();
                    //            if (productionhumandir.GetFiles("*.tif").Length == 0)
                    //            {
                    //                DirectoryInfo productionDir = productionhumandir.Parent;
                    //                productionhumandir.Delete(true);
                    //                if (productionDir.GetDirectories().Length == 0)
                    //                {
                    //                    productionDir.Delete(true);
                    //                }
                    //            }

                    //            DirectoryInfo humandir = indexPathInfo.Directory;
                    //            indexPathInfo.Delete();
                    //            if (humandir.GetFiles("*.tif").Length == 0)
                    //            {
                    //                DirectoryInfo indexedDir = humandir.Parent;
                    //                humandir.Delete(true);
                    //                if (indexedDir.GetDirectories().Length == 0)
                    //                {
                    //                    DirectoryInfo oldchartsDir = indexedDir.Parent;
                    //                    indexedDir.Delete(true);
                    //                    if (oldchartsDir.GetDirectories().Length == 0)
                    //                    {
                    //                        DirectoryInfo dateDir = oldchartsDir.Parent;
                    //                        oldchartsDir.Delete(true);
                    //                        if (dateDir.GetDirectories().Length == 0)
                    //                            dateDir.Delete(true);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    if (indexPath.Count > 0)
                    {
                        for (int i = 0; i < indexPath.Count; i++)
                        {
                            File.Delete(indexPath[i].ToString());
                            DirectoryInfo indexed_human_dir = new DirectoryInfo(new FileInfo(indexPath[i].ToString()).DirectoryName);
                            if (indexed_human_dir.GetFiles().Length == 0)
                                Directory.Delete(indexed_human_dir.FullName);


                            File.Delete(indexPath[i].ToString().Replace("Indexed_Images", "Move To Production"));
                            DirectoryInfo moved_production_dir = new DirectoryInfo(new FileInfo(indexPath[i].ToString().Replace("Indexed_Images", "Move To Production")).DirectoryName);
                            if (moved_production_dir.GetFiles().Length == 0)
                                Directory.Delete(moved_production_dir.FullName);
                        }
                        DirectoryInfo folder_path = new DirectoryInfo(scanPath[0].ToString().Substring(0, scanPath[0].ToString().IndexOf("Scanned_Images")) + "\\Waiting_For_Delete");
                        if (!folder_path.Exists)
                            Directory.CreateDirectory(folder_path.FullName);
                        for (int t = 0; t < scanPath.Count; t++)
                        {
                            // File.Move(scanPath[t].ToString(), folder_path.FullName + scanPath[t].ToString().Remove(0, scanPath[t].ToString().LastIndexOf("\\")));


                            if (!File.Exists(folder_path.FullName + scanPath[t].ToString().Remove(0, scanPath[t].ToString().LastIndexOf("\\"))))
                                File.Copy(scanPath[t].ToString(), folder_path.FullName + scanPath[t].ToString().Remove(0, scanPath[t].ToString().LastIndexOf("\\")));
                            else
                            {
                                FileInfo file = new FileInfo(folder_path.FullName + scanPath[t].ToString().Remove(0, scanPath[t].ToString().LastIndexOf("\\")));
                                File.Copy(scanPath[t].ToString(), file.FullName.Insert(file.FullName.Length - 4, " - Copy"));
                            }

                            File.Delete(scanPath[t].ToString());
                        }
                    }

                }
                mySession.Close();
            }
        }


        public void SaveUpdateDeleteFileManagementIndexForOnline(IList<FileManagementIndex> addfileList, IList<FileManagementIndex> updatefileList, IList<FileManagementIndex> deletefileList, string macAddress)
        {
            SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList, deletefileList, macAddress);
        }

        #endregion

        public FileManagementIndex GetListUsingOrderSubmitAndDocType(ulong order_submit_id)
        {
            FileManagementIndex file = null;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Order_ID", order_submit_id));
                if (crit.List<FileManagementIndex>() != null && crit.List<FileManagementIndex>().Count > 0)
                {
                    mySession.Close();
                    return crit.List<FileManagementIndex>()[0];
                }
                else
                {
                    mySession.Close();
                    return file;
                }
            }
        }

        public IList<FileManagementIndex> GetIndexedListBySource(ulong ulhumanID, ulong ulEncounterID, string Source)
        {
            IList<FileManagementIndex> lstFile = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulhumanID)).Add(Expression.Eq("Order_ID", ulEncounterID)).Add(Expression.Eq("Source", Source));
                lstFile = crit.List<FileManagementIndex>();
                mySession.Close();
            }
            return lstFile;
        }

        public IList<FileManagementIndex> SaveUpdateDeleteFileManagementIndexforExamPhotos(IList<FileManagementIndex> addfileList, IList<FileManagementIndex> updatefileList, IList<FileManagementIndex> deletefileList, string macAddress, string Source)
        {
            IList<FileManagementIndex> fileIndex = new List<FileManagementIndex>();
            GenerateXml XMLObj = new GenerateXml();
            bool bDataConsistent = true;
            //  ulong uHumanId=0;
            //if(addfileList != null && addfileList.Count>0)
            //{
            //    uHumanId = addfileList[0].Human_ID;
            //}
            //if (updatefileList != null && updatefileList.Count > 0)
            //{
            //    uHumanId = updatefileList[0].Human_ID;
            //}
            //if (deletefileList != null && deletefileList.Count > 0)
            //{
            //    uHumanId = deletefileList[0].Human_ID;
            //}
            ulong uHumanId = addfileList != null && addfileList.Count > 0 ? addfileList[0].Human_ID : updatefileList != null && updatefileList.Count > 0 ? updatefileList[0].Human_ID : deletefileList != null && deletefileList.Count > 0 ? deletefileList[0].Human_ID : 0;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (Source == "")
                {
                    //IList<FileManagementIndex> fileIndex = null;
                    if (addfileList != null && addfileList.Count > 0 || updatefileList != null && updatefileList.Count > 0 || deletefileList != null && deletefileList.Count > 0)
                    {
                        //SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList, deletefileList, macAddress);
                        //SaveUpdateDelete_DBAndXML_WithoutTransaction(ref addfileList, ref updatefileList, deletefileList, mySession, macAddress, true, true, uHumanId, string.Empty, ref XMLObj);
                        SaveUpdateDelete_DBAndXML_WithTransaction(ref addfileList, ref updatefileList, deletefileList, macAddress, true, true, uHumanId, string.Empty);
                        if (addfileList != null && addfileList.Count > 0)
                            fileIndex = addfileList;
                        else if (updatefileList != null && updatefileList.Count > 0)
                        {
                            updatefileList = updatefileList.Select(a =>
                            {
                                a.Version = a.Version + 1;
                                return a;
                            }).ToList<FileManagementIndex>();
                            foreach (FileManagementIndex item in updatefileList)
                            {
                                fileIndex.Add(item);
                            }
                        }
                        else if (deletefileList != null && deletefileList.Count > 0)
                        { }
                    }
                    // return fileIndex;
                }
                else if (Source == "IN-HOUSE" || Source == "BODY IMAGE")
                {
                    // IList<FileManagementIndex> fileIndex = new List<FileManagementIndex>();
                    if (addfileList != null && addfileList.Count > 0 || updatefileList != null && updatefileList.Count > 0 || deletefileList != null && deletefileList.Count > 0)
                    {
                        //SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList, deletefileList, macAddress);                  

                        SaveUpdateDelete_DBAndXML_WithTransaction(ref addfileList, ref updatefileList, deletefileList, macAddress, true, true, uHumanId, string.Empty);
                        if (addfileList != null && addfileList.Count > 0)
                            fileIndex = addfileList;
                        else if (updatefileList != null && updatefileList.Count > 0)
                        {
                            updatefileList = updatefileList.Select(a =>
                            {
                                a.Version = a.Version + 1;
                                return a;
                            }).ToList<FileManagementIndex>();
                            foreach (FileManagementIndex item in updatefileList)
                            {
                                fileIndex.Add(item);
                            }
                        }
                        else if (deletefileList != null && deletefileList.Count > 0)
                        { }
                    }

                    //GenerateXml XMLObj = new GenerateXml();
                    //ulong encounterid = 0;
                    //if (addfileList.Count > 0)
                    //{

                    //    encounterid = addfileList[0].Order_ID;
                    //    List<object> lstObj = addfileList.Cast<object>().ToList();
                    //    XMLObj.GenerateXmlSaveStatic(lstObj, encounterid, string.Empty);
                    //}
                    //if (updatefileList.Count > 0)
                    //{
                    //    encounterid = updatefileList[0].Order_ID;
                    //    List<object> lstObj = updatefileList.Cast<object>().ToList();
                    //    XMLObj.GenerateXmlUpdate(lstObj, encounterid, string.Empty);
                    //}
                    //if (deletefileList.Count > 0)
                    //{
                    //    encounterid = deletefileList[0].Order_ID;
                    //    List<object> lstObj = deletefileList.Cast<object>().ToList();
                    //    XMLObj.DeleteXmlNode(encounterid, lstObj, string.Empty);
                    //}
                    //  return fileIndex;
                }

                else if (Source == "ORDER")
                {
                    // IList<FileManagementIndex> fileIndex = new List<FileManagementIndex>();
                    if (addfileList != null && addfileList.Count > 0 || updatefileList != null && updatefileList.Count > 0 || deletefileList != null && deletefileList.Count > 0)
                    {
                        // SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList, deletefileList, macAddress);
                        SaveUpdateDelete_DBAndXML_WithTransaction(ref addfileList, ref updatefileList, deletefileList, macAddress, true, true, uHumanId, string.Empty);
                        if (addfileList != null && addfileList.Count > 0)
                            fileIndex = addfileList;
                        else if (updatefileList != null && updatefileList.Count > 0)
                        {
                            updatefileList = updatefileList.Select(a =>
                            {
                                a.Version = a.Version + 1;
                                return a;
                            }).ToList<FileManagementIndex>();
                            foreach (FileManagementIndex item in updatefileList)
                            {
                                fileIndex.Add(item);
                            }
                        }
                        else if (deletefileList != null && deletefileList.Count > 0)
                        { }
                    }
                    // return fileIndex;

                }
                else if (Source == "SUMMARY OF CARE")
                {
                    //   IList<FileManagementIndex> fileIndex = new List<FileManagementIndex>();
                    if (addfileList != null && addfileList.Count > 0 || updatefileList != null && updatefileList.Count > 0 || deletefileList != null && deletefileList.Count > 0)
                    {
                        //SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList, deletefileList, macAddress);
                        SaveUpdateDelete_DBAndXML_WithTransaction(ref addfileList, ref updatefileList, deletefileList, macAddress, true, true, uHumanId, string.Empty);
                        if (addfileList != null && addfileList.Count > 0)
                        {
                            fileIndex = addfileList;
                        }
                    }
                    //return fileIndex;
                }

                else
                {
                    //   IList<FileManagementIndex> fileIndex = new List<FileManagementIndex>();
                    if (addfileList != null && addfileList.Count > 0 || updatefileList != null && updatefileList.Count > 0 || deletefileList != null && deletefileList.Count > 0)
                    {
                        //SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList, deletefileList, macAddress);
                        SaveUpdateDelete_DBAndXML_WithTransaction(ref addfileList, ref updatefileList, deletefileList, macAddress, true, true, uHumanId, string.Empty);
                        if (addfileList != null && addfileList.Count > 0)
                            fileIndex = addfileList;
                        else if (updatefileList != null && updatefileList.Count > 0)
                        {
                            updatefileList = updatefileList.Select(a =>
                            {
                                a.Version = a.Version + 1;
                                return a;
                            }).ToList<FileManagementIndex>();
                            foreach (FileManagementIndex item in updatefileList)
                            {
                                fileIndex.Add(item);
                            }
                        }
                        else if (deletefileList != null && deletefileList.Count > 0)
                        { }
                    }
                    mySession.Close();
                }
            }

            //IList<FileManagementIndex> lstSaveUpdate = new List<FileManagementIndex>();
            //if (addfileList != null)
            //    lstSaveUpdate = lstSaveUpdate.Concat<FileManagementIndex>(addfileList).ToList();
            //if (updatefileList != null)
            //    lstSaveUpdate = lstSaveUpdate.Concat<FileManagementIndex>(updatefileList).ToList();

            //bDataConsistent = XMLObj.CheckDataConsistency(lstSaveUpdate.Cast<object>().ToList(), true, string.Empty);
            //if (bDataConsistent)
            //{
            //    XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
            //}
            //else
            //    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");

            return fileIndex;

        }
        public void SaveUpdateDeleteFileManagementIndexForOnline_and_Wfobject(IList<FileManagementIndex> saveList, ulong[] uscanID, string macAddress, DateTime TransactionTime)
        {

            int iResult = 0;
            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            trans = MySession.BeginTransaction();
            int iTryCount = 0;

            if (saveList.Count > 0)
            {

            TryAgain:

                IList<FileManagementIndex> scanList = new List<FileManagementIndex>();
                IList<FileManagementIndex> updateList = new List<FileManagementIndex>();
                GenerateXml objGenerateXML = new GenerateXml();
                try
                {
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref updateList, null, MySession, macAddress, false, false, 0, string.Empty, ref objGenerateXML);
                    if (iResult == 0)
                    {
                        foreach (ulong scanID in uscanID.Distinct())
                        {
                            WFObjectManager wfObjectManager = new WFObjectManager();
                            wfObjectManager.MoveToNextProcess(scanID, "SCAN", 3, "UNKNOWN", TransactionTime, string.Empty, new string[] { "INDEX" }, MySession);
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
                            throw new Exception("Deadlock is occured. Transaction failed");

                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        throw new Exception("Exception is occured. Transaction failed");

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

            }
            trans.Commit();
            MySession.Close();


        }


        // added by manimozhi 13th august 2012
        //public IList<FileManagementIndex> GetScannedObjectByScanIndexConversionID(ulong ulhumanID, ulong ScanIndexConversionID)
        //{
        //    ICriteria crit = session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulhumanID)).Add(Expression.Eq("Scan_Index_Conversion_ID", ScanIndexConversionID));
        //    return crit.List<FileManagementIndex>();
        //}


        //        public IList<FileManagementIndex> GetListBySource(string Source)
        //        {
        //            ICriteria crit = session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Source", Source));
        //            return crit.List<FileManagementIndex>();
        //        }
        //||||||| 1.29
        //=======



        public IList<FileManagementIndex> GetListBySource(string Source)
        {
            IList<FileManagementIndex> lstFile = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Source", Source));
                lstFile = crit.List<FileManagementIndex>();
                mySession.Close();
            }
            return lstFile;
        }





        //public void SaveUpdateDeleteFileManagementIndexForOnline_and_Wfobject(IList<FileManagementIndex> saveList, IList<FileManagementIndex> updatefileList, IList<FileManagementIndex> deletefileList, IList<WFObject> wfObjList, string macAddress)
        //{

        //    int iResult = 0;
        //    ISession MySession = Session.GetISession();
        //    ITransaction trans = null;
        //    trans = MySession.BeginTransaction();
        //    int   iTryCount = 0;

        //    if (saveList.Count > 0)
        //    {
        //        for (int i = 0; i < saveList.Count; i++)
        //        {
        //        TryAgain:

        //            IList<FileManagementIndex> scanList = new List<FileManagementIndex>();
        //            try
        //            {
        //                scanList.Add(saveList.ElementAt<FileManagementIndex>(i));
        //                iResult = SaveUpdateDeleteWithoutTransaction(ref scanList, null, null, MySession, macAddress);
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
        //            catch (NHibernate.Exceptions.GenericADOException ex)
        //            {
        //                trans.Rollback();
        //                //MySession.Close();
        //                throw new Exception(ex.Message);
        //            }
        //            catch (Exception e)
        //            {
        //                trans.Rollback();
        //                // MySession.Close();
        //                throw new Exception(e.Message);
        //            }

        //        }
        //    }

        //    for (int k = 0; k < wfObjList.Count; k++)
        //    {
        //        try
        //        {
        //        TryAgain:
        //            WFObject currentwfObject = new WFObject();
        //            currentwfObject = wfObjList.ElementAt<WFObject>(k);

        //            WFObjectManager wfObjectManager = new WFObjectManager();
        //            iResult = wfObjectManager.InsertToWorkFlowObject(currentwfObject, 1, macAddress, MySession);
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
        //                    throw new Exception("Deadlock is occured. Transaction failed");

        //                }
        //            }
        //            else if (iResult == 1)
        //            {

        //                trans.Rollback();
        //                //MySession.Close();
        //                throw new Exception("Exception is occured. Transaction failed");

        //            }

        //        }

        //        catch (NHibernate.Exceptions.GenericADOException ex)
        //        {
        //            trans.Rollback();
        //            //MySession.Close();
        //            throw new Exception(ex.Message);
        //        }
        //        catch (Exception e)
        //        {
        //            trans.Rollback();
        //            // MySession.Close();
        //            throw new Exception(e.Message);
        //        }
        //    }

        //    trans.Commit();
        //    MySession.Close();
        //    //MySession.Flush();

        //}
        // added by manimozhi 13th august 2012
        public IList<FileManagementIndex> GetScannedObjectByScanIndexConversionID(ulong ulhumanID, ulong ScanIndexConversionID)
        {
            IList<FileManagementIndex> lstFile = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulhumanID)).Add(Expression.Eq("Scan_Index_Conversion_ID", ScanIndexConversionID));

                lstFile = crit.List<FileManagementIndex>();
                mySession.Close();
            }
            return lstFile;
        }

        public IList<FileManagementIndex> GetListOfOrdersNotTied(ulong ulHumanID, string sDocType, string sDocSubType)
        {
            IList<FileManagementIndex> FileManagementList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql1 = mySession.CreateSQLQuery("Select a.* from file_management_index a where a.Human_ID = '" + ulHumanID + "' and a.Doc_Type  in (" + sDocType + ")  and a.Doc_Sub_Type in (" + sDocSubType + ")").AddEntity("a", typeof(FileManagementIndex));
                FileManagementList = sql1.List<FileManagementIndex>();
                //ICriteria crit = session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Document_Type", sDocType)).Add(Expression.Eq("Document_Sub_Type", "Lab Result Form"));
                //return crit.List<FileManagementIndex>();
                mySession.Close();
            }
            return FileManagementList;
        }
        public int UpdateFilemanagementIndexforMRE(IList<FileManagementIndex> UpdateFileManagementIndex, ISession MySession, string macaddress)
        {
            IList<FileManagementIndex> Addlist = new List<FileManagementIndex>();

            return SaveUpdateDeleteWithoutTransaction(ref Addlist, UpdateFileManagementIndex, null, MySession, macaddress);
        }
        public IList<FileManagementIndex> GetImagesforAnnotations(ulong ulHumanID, ulong ulOrder_Id, string Source)
        {
            string[] source = Source.Split(',');
            IList<FileManagementIndex> list_image = null;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Order_ID", ulOrder_Id)).Add(Expression.In("Source", source));
                list_image = crit.List<FileManagementIndex>();
                mySession.Close();
            }
            return list_image;

        }

        public IList<FileManagementIndex> GetListforExamandInhouse(ulong ulHumanID, ulong ulOrder_Id)
        {

            IList<FileManagementIndex> FileManagementList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql1 = mySession.CreateSQLQuery("Select a.* from file_management_index a where a.Human_ID = '" + ulHumanID + "' and a.Order_ID='" + ulOrder_Id + "'  and a.Source in ('ORDER')").AddEntity("a", typeof(FileManagementIndex));
                FileManagementList = sql1.List<FileManagementIndex>();
                mySession.Close();
            }
            return FileManagementList;

        }

        public IList<FileManagementIndex> GetFileList(ulong human_id)
        {
            // ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
            IList<Orders> lstorder = new List<Orders>();
            IList<FileManagementIndex> filemanagementList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql1 = mySession.CreateSQLQuery("Select a.* from file_management_index a where a.Human_ID = '" + human_id + "'  and a.Source in ('ORDER','SCAN','ABI','SPIROMETRY','MESSAGE','ELIGIBILITY VERIFICATION','EXAM','Encounter_XML_DR') and a.Is_Delete <>'Y'").AddEntity("a", typeof(FileManagementIndex));
                fileList = sql1.List<FileManagementIndex>();

                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        if (fileList[i].Order_ID != 0)
                        {
                            IList<OrdersSubmit> ordersList = new List<OrdersSubmit>();
                            ICriteria crit = mySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", fileList[i].Order_ID));
                            ordersList = crit.List<OrdersSubmit>();
                            if (ordersList.Count > 0)
                            {   //Bug ID:62511
                                //if (ordersList[0].Bill_Type.Trim() != string.Empty) //BugID:42001 ,41884
                                //{
                                //    fileList[i].Created_Date_And_Time = ordersList[0].Created_Date_And_Time;//Results entered from diagnostic_order screen
                                //}
                                //else
                                //{
                                fileList[i].Created_Date_And_Time = fileList[i].Document_Date;//Results uploaded from upload scan documents screen
                                //}
                                //if (ordersList[0].Specimen_Collection_Date_And_Time == DateTime.MinValue)
                                //{
                                //    fileList[i].Created_Date_And_Time = ordersList[0].Created_Date_And_Time;
                                //}
                                //else
                                //{
                                //    fileList[i].Created_Date_And_Time = ordersList[0].Specimen_Collection_Date_And_Time;
                                //}
                                // fileList[i].Document_Type = ordersList[0].Order_Type;

                                if (fileList[i].Order_ID != 0)
                                {
                                    ICriteria criti = mySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Order_Submit_ID", fileList[i].Order_ID));
                                    lstorder = criti.List<Orders>();
                                    if (lstorder.Count > 0)
                                    {
                                        string description = string.Empty;
                                        for (int j = 0; j < lstorder.Count; j++)
                                        {
                                            if (description == string.Empty)
                                            {
                                                description = lstorder[j].Lab_Procedure + " - " + lstorder[j].Lab_Procedure_Description;
                                            }
                                            else
                                            {
                                                description = description + "; " + lstorder[j].Lab_Procedure + " - " + lstorder[j].Lab_Procedure_Description;
                                            }
                                        }
                                        fileList[i].Orders_Description = description;
                                    }
                                }
                                filemanagementList.Add(fileList[i]);
                            }

                        }
                        else
                        {
                            filemanagementList.Add(fileList[i]);
                        }

                    }
                }
                mySession.Close();
            }
            return filemanagementList;
        }

        public IList<FileManagementIndex> GetEncountersFileList(ulong human_id)
        {
            // ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
            IList<Orders> lstorder = new List<Orders>();
            IList<FileManagementIndex> filemanagementList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql1 = mySession.CreateSQLQuery("Select a.* from file_management_index a where a.Human_ID = '" + human_id + "'  and a.Source ='SCAN' and a.Doc_Type='Encounters' and a.Is_Delete<>'Y'").AddEntity("a", typeof(FileManagementIndex));
                fileList = sql1.List<FileManagementIndex>();

                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        filemanagementList.Add(fileList[i]);
                    }
                }
                mySession.Close();
            }
            return filemanagementList;
        }

        //public FileManagementDTO GetResultandExamListusing(ulong human_id)
        //{

        //    IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
        //    FileManagementDTO objfilelist = new FileManagementDTO();
        //    fileList = GetFileList(human_id);
        //    if (fileList.Count > 0)
        //    {
        //        objfilelist.FileManagementList = fileList;
        //    }

        //    IList<ResultMaster> resMasterList = new List<ResultMaster>();
        //    IList<ResultOBR> lstObr = new List<ResultOBR>();
        //    IList<Orders> lstorder = new List<Orders>();
        //    using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query = mySession.GetNamedQuery("Fill.ResultMasterList");
        //        query.SetString(0, human_id.ToString());
        //        ArrayList arr = new ArrayList(query.List());

        //        if (arr != null)
        //        {
        //            foreach (object[] obj in arr)
        //            {
        //                if (obj[3] != null)
        //                {
        //                    if (obj[3].ToString() != "")
        //                    {
        //                        ResultMaster resMaster = new ResultMaster();
        //                        resMaster = new ResultMaster();
        //                        resMaster.Id = Convert.ToUInt64(obj[0]);
        //                        resMaster.Order_ID = Convert.ToUInt64(obj[1]);
        //                        resMaster.Created_Date_And_Time = Convert.ToDateTime(obj[2]);
        //                        resMaster.OBR_Specimen_Collected_Date_And_Time = Convert.ToString(obj[3]);
        //                        resMaster.MSH_Date_And_Time_Of_Message = Convert.ToString(obj[7]);
        //                        resMaster.temp_property = obj[4].ToString();
        //                        if (obj[5] != null)
        //                        {
        //                            resMaster.Order_Type = obj[5].ToString();
        //                        }

        //                        if (Convert.ToUInt64(obj[1]) != 0)
        //                        {
        //                            ICriteria crit = mySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Order_Submit_ID", Convert.ToUInt64(obj[1])));
        //                            lstorder = crit.List<Orders>();
        //                            if (lstorder.Count > 0)
        //                            {
        //                                string description = string.Empty;
        //                                for (int i = 0; i < lstorder.Count; i++)
        //                                {
        //                                    if (description == string.Empty)
        //                                    {
        //                                        description = lstorder[i].Lab_Procedure + "-" + lstorder[i].Lab_Procedure_Description;
        //                                    }
        //                                    else
        //                                    {
        //                                        description = description + "; " + lstorder[i].Lab_Procedure + " - " + lstorder[i].Lab_Procedure_Description;
        //                                    }
        //                                }
        //                                resMaster.Orders_Description = description;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ICriteria crit = mySession.CreateCriteria(typeof(ResultOBR)).Add(Expression.Eq("Result_Master_ID", Convert.ToUInt64(obj[0])));
        //                            lstObr = crit.List<ResultOBR>();
        //                            if (lstObr.Count > 0)
        //                            {
        //                                string description = string.Empty;
        //                                for (int i = 0; i < lstObr.Count; i++)
        //                                {
        //                                    if (description == string.Empty)
        //                                    {
        //                                        description = lstObr[i].OBR_Observation_Battery_Identifier + " - " + lstObr[i].OBR_Observation_Battery_Text;
        //                                    }
        //                                    else
        //                                    {
        //                                        description = description + "; " + lstObr[i].OBR_Observation_Battery_Identifier + " - " + lstObr[i].OBR_Observation_Battery_Text;
        //                                    }
        //                                }
        //                                resMaster.Orders_Description = description;
        //                            }
        //                        }
        //                        resMasterList.Add(resMaster);
        //                    }
        //                }
        //            }

        //        }
        //        if (resMasterList.Count > 0)
        //        {
        //            objfilelist.ResultMasterList = resMasterList;
        //        }
        //        mySession.Close();
        //    }
        //    return objfilelist;
        //}

        public FileManagementDTO GetResultandExamListusing(ulong human_id, ulong order_submit_id)
        {

            IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
            FileManagementDTO objfilelist = new FileManagementDTO();
            fileList = GetFileList(human_id);
            if (fileList.Count > 0)
            {
                objfilelist.FileManagementList = fileList;
            }

            IList<ResultMaster> resMasterList = new List<ResultMaster>();
            IList<ResultOBR> lstObr = new List<ResultOBR>();
            IList<Orders> lstorder = new List<Orders>();
            IQuery query;

            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (order_submit_id != 0)
                {
                    query = mySession.GetNamedQuery("Fill.ResultMasterList");
                    query.SetString(0, human_id.ToString());
                    query.SetString(1, order_submit_id.ToString());
                }
                else
                {
                    query = mySession.GetNamedQuery("Fill.ResultMasterListByHumanID");
                    query.SetString(0, human_id.ToString());
                }
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
                                resMaster.MSH_Date_And_Time_Of_Message = Convert.ToString(obj[7]);
                                resMaster.temp_property = obj[4].ToString();
                                if (obj[5] != null)
                                {
                                    resMaster.Order_Type = obj[5].ToString();
                                }

                                if (Convert.ToUInt64(obj[1]) != 0)
                                {
                                    ICriteria crit = mySession.CreateCriteria(typeof(Orders)).Add(Expression.Eq("Order_Submit_ID", Convert.ToUInt64(obj[1])));
                                    lstorder = crit.List<Orders>();
                                    if (lstorder.Count > 0)
                                    {
                                        string description = string.Empty;
                                        for (int i = 0; i < lstorder.Count; i++)
                                        {
                                            if (description == string.Empty)
                                            {
                                                description = lstorder[i].Lab_Procedure + "-" + lstorder[i].Lab_Procedure_Description;
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
                                    ICriteria crit = mySession.CreateCriteria(typeof(ResultOBR)).Add(Expression.Eq("Result_Master_ID", Convert.ToUInt64(obj[0])));
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
                if (resMasterList.Count > 0)
                {
                    objfilelist.ResultMasterList = resMasterList;
                }
                mySession.Close();
            }
            return objfilelist;
        }

        public IList<FileManagementIndex> GetCopyPreviousBodyImage(ulong phy_Id, ulong Human_Id, ulong enc_Id)
        {
            ulong encID = 0;
            IList<FileManagementIndex> list_image = new List<FileManagementIndex>();
            int phy_id = Convert.ToInt32(phy_Id);
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    encID = mySession.CreateCriteria(typeof(Encounter))
                   .SetProjection(Projections.Max("Id"))
                   .Add(Expression.Eq("Appointment_Provider_ID", phy_id))
                   .Add(Expression.Between("Id", 0, (enc_Id - 1))).Add(!Expression.Eq("Date_of_Service", DateTime.MinValue))
                   .Add(Expression.Eq("Human_ID", Human_Id)).Add(!Expression.Eq("Id", enc_Id)).List<ulong>()[0];
                }
                catch
                {
                    encID = 0;
                }


                ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", Human_Id)).Add(Expression.Eq("Order_ID", encID)).Add(Expression.Eq("Source", "BODY IMAGE"));
                list_image = crit.List<FileManagementIndex>();
                mySession.Close();
            }
            return list_image;
        }

        public FileManagementDTO GetResultandExamListusingOrderSubmitId(ulong orderSubmitId, ulong humanId)
        {
            IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
            FileManagementDTO objfilelist = new FileManagementDTO();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql1 = mySession.CreateSQLQuery("Select a.* from file_management_index a where a.Order_ID = '" + orderSubmitId + "'  and a.Source in ('ORDER','SCAN','ABI','SPIROMETRY','MESSAGE') and a.Is_Delete<>'Y'").AddEntity("a", typeof(FileManagementIndex));
                fileList = sql1.List<FileManagementIndex>();
                IList<FileManagementIndex> filemanagementList = new List<FileManagementIndex>();
                for (int i = 0; i < fileList.Count; i++)
                {
                    if (fileList[i].Order_ID != 0)
                    {
                        IList<OrdersSubmit> ordersList = new List<OrdersSubmit>();
                        ICriteria crit = mySession.CreateCriteria(typeof(OrdersSubmit)).Add(Expression.Eq("Id", fileList[i].Order_ID));
                        ordersList = crit.List<OrdersSubmit>();
                        if (ordersList.Count > 0)
                        {
                            if (ordersList[0].Specimen_Collection_Date_And_Time == DateTime.MinValue)
                            {
                                fileList[i].Created_Date_And_Time = ordersList[0].Created_Date_And_Time;
                            }
                            else
                            {
                                fileList[i].Created_Date_And_Time = ordersList[0].Specimen_Collection_Date_And_Time;
                            }
                            fileList[i].Document_Type = ordersList[0].Order_Type;
                            filemanagementList.Add(fileList[i]);
                        }

                    }
                }
                objfilelist.FileManagementList = fileList;

                IList<ResultMaster> resMasterList = new List<ResultMaster>();
                IQuery query = mySession.GetNamedQuery("Fill.ResultMasterListByHumanID");
                query.SetString(0, humanId.ToString());
                ArrayList arr = new ArrayList(query.List());

                if (arr != null)
                {

                    foreach (object[] obj in arr)
                    {
                        if (obj[3] != null)
                        {
                            if (obj[3].ToString() != "")
                            {
                                if (Convert.ToUInt64(obj[1]) == orderSubmitId)
                                {
                                    ResultMaster resMaster = new ResultMaster();
                                    resMaster = new ResultMaster();
                                    resMaster.Id = Convert.ToUInt64(obj[0]);
                                    resMaster.Order_ID = Convert.ToUInt64(obj[1]);
                                    resMaster.Created_Date_And_Time = Convert.ToDateTime(obj[2]);
                                    resMaster.MSH_Date_And_Time_Of_Message = Convert.ToString(obj[3]);
                                    resMaster.temp_property = obj[4].ToString();
                                    if (obj[5] != null)
                                    {
                                        resMaster.Order_Type = obj[5].ToString();
                                    }
                                    resMaster.Is_Electronic_Mode = obj[6].ToString();
                                    resMasterList.Add(resMaster);
                                }
                            }
                        }
                    }

                }
                if (resMasterList.Count > 0)
                {
                    objfilelist.ResultMasterList = resMasterList;
                }
                mySession.Close();
            }
            return objfilelist;



        }


        public FileManagementDTO SaveUpdateDelete_FileManagementIndex_Abi_Spirometry(FileManagementDTO ObjSave, FileManagementDTO ObjUpdate, FileManagementDTO ObjDelete, string macAddress)
        {
            FileManagementDTO ObjResult = new FileManagementDTO();
            FileManagementIndexManager ObjFileManagement_Mngr = new FileManagementIndexManager();
            ABI_ResultsManager ObjABI_Mngr = new ABI_ResultsManager();

            IList<ABI_Results> lstAbiSave = null;
            IList<FileManagementIndex> lst_FileManagementIndex_Save = new List<FileManagementIndex>();
            IList<FileManagementIndex> lst_FileManagementIndex_Update = null;


            if (ObjSave.lstABI.Count > 0)
            {
                lstAbiSave = new List<ABI_Results>();
                lstAbiSave = ObjSave.lstABI;
                ObjABI_Mngr.SaveUpdateDeleteWithTransaction(ref lstAbiSave, null, null, macAddress);
                for (int i = 0; i < ObjSave.lstABI.Count; i++)
                {
                    FileManagementIndex obj = new FileManagementIndex();
                    obj.Source = "ABI";

                    obj.Human_ID = lstAbiSave[i].Patient_ID;
                    obj.Result_Master_ID = Convert.ToUInt64(lstAbiSave[i].Id);
                    obj.Order_ID = lstAbiSave[i].Order_ID;
                    obj.Document_Date = lstAbiSave[i].Created_Date_And_Time;
                    if (ObjSave.lstABI[i].ABI_File_Type == "csv")
                        obj.File_Path = "";
                    else
                        obj.File_Path = ObjSave.lstABI[i].File_Name;

                    obj.Created_By = ObjSave.lstABI[i].Created_By;
                    obj.Created_Date_And_Time = ObjSave.lstABI[i].Created_Date_And_Time;
                    obj.Modified_By = ObjSave.lstABI[i].Modified_By;
                    obj.Modified_Date_And_Time = ObjSave.lstABI[i].Modified_Date_And_Time;
                    lst_FileManagementIndex_Save.Add(obj);
                }
            }


            if (ObjSave.lstSpirometry != null && ObjSave.lstSpirometry.Count > 0)
            {

                SpirometryManager ObjSpirometryManager = new SpirometryManager();
                IList<Spirometry> lstSpirometrySave = new List<Spirometry>();
                IList<Spirometry> lstSpirometryUpdate = null;
                lstSpirometrySave = ObjSave.lstSpirometry;
                // ObjSpirometryManager.SaveUpdateDeleteWithTransaction(ref lstSpirometrySave, null, null, macAddress);
                ObjSpirometryManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref lstSpirometrySave, ref lstSpirometryUpdate, null, macAddress, false, false, 0, "");
                for (int i = 0; i < lstSpirometrySave.Count; i++)
                {
                    FileManagementIndex obj = new FileManagementIndex();
                    obj.Source = "SPIROMETRY";

                    obj.Human_ID = lstSpirometrySave[i].Patient_ID;
                    obj.Result_Master_ID = lstSpirometrySave[i].Id;
                    obj.Order_ID = lstSpirometrySave[i].In_House_Procedure_ID;
                    obj.Document_Date = lstSpirometrySave[i].Created_Date_And_Time;
                    //obj.File_Path = lstSpirometrySave[i].File_Name;
                    obj.Created_By = lstSpirometrySave[i].Created_By;
                    obj.Created_Date_And_Time = lstSpirometrySave[i].Created_Date_And_Time;
                    obj.Modified_By = lstSpirometrySave[i].Modified_By;
                    obj.Modified_Date_And_Time = lstSpirometrySave[i].Modified_Date_And_Time;
                    lst_FileManagementIndex_Save.Add(obj);
                }
            }

            foreach (FileManagementIndex obj_FileManagement in ObjSave.FileManagementList)
            {
                lst_FileManagementIndex_Save.Add(obj_FileManagement);
            }
            SaveUpdateDeleteWithTransaction(ref lst_FileManagementIndex_Save, null, null, macAddress);

            SaveUpdateDelete_DBAndXML_WithTransaction(ref lst_FileManagementIndex_Save, ref lst_FileManagementIndex_Update, null, macAddress, false, false, 0, "");
            return ObjResult;
        }


        public IList<FileManagementIndex> SaveUpdatDeleteForCMG(IList<FileManagementIndex> addfileList, IList<FileManagementIndex> updatefileList, IList<FileManagementIndex> deletefileList, string macAddress)
        {

            FileManagementIndexManager obgMgr = new FileManagementIndexManager();
            obgMgr.SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList, deletefileList, macAddress);
            obgMgr.SaveUpdateDelete_DBAndXML_WithTransaction(ref addfileList, ref updatefileList, deletefileList, macAddress, false, false, 0, string.Empty);

            #region Old
            //  FileManagementIndexManager obgMgr = new FileManagementIndexManager();
            //IList<FileManagementIndex> lstList=new List<FileManagementIndex>();
            //if (addfileList.Count > 0)
            //{
            //    ICriteria crit = session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Result_Master_ID", addfileList[0].Result_Master_ID)).Add(Expression.Eq("Order_ID", addfileList[0].Order_ID)).Add(Expression.Eq("Source", "ORDER"));
            //    lstList = crit.List<FileManagementIndex>();
            //    if (lstList.Count > 0)
            //    {
            //        if (lstList[0].File_Path == string.Empty && lstList.Count == 1)
            //          {

            //            IList<FileManagementIndex> lstSave = new List<FileManagementIndex>();
            //            IList<FileManagementIndex> lstUpdate = new List<FileManagementIndex>();
            //            lstUpdate.Add(lstList[0]);
            //            lstUpdate[0].File_Path = addfileList[0].File_Path;
            //            lstSave=addfileList.Skip(1).ToList<FileManagementIndex>();
            //            obgMgr.SaveUpdateDeleteWithTransaction(ref lstSave, lstUpdate, deletefileList, macAddress);
            //        }
            //        else
            //        {
            //            obgMgr.SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList , deletefileList, macAddress);
            //        }
            //    }
            //    else
            //        obgMgr.SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList, deletefileList, macAddress);
            //}
            #endregion
            return new List<FileManagementIndex>();
        }


        public FileManagementDTO LoadFileIndex_and_ViewResult(ulong ulEncounter_Id, ulong ulHumanID, ulong ulOrder_Id, string Source, bool IsCmg)
        {
            FileManagementDTO ObjLoad = new FileManagementDTO();
            ICriteria crit;
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {

                if (IsCmg)
                {
                    string[] str = new string[1];
                    str[0] = "ORDER";
                    crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Result_Master_ID", ulEncounter_Id)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Order_ID", ulOrder_Id)).Add(Expression.In("Source", str));
                    ObjLoad.FileManagementList = crit.List<FileManagementIndex>();
                }
                else
                {
                    crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Order_ID", ulOrder_Id)).Add(Expression.In("Source", Source.Split(',').Where(A => A != string.Empty && A != null).ToArray()));
                    ObjLoad.FileManagementList = crit.List<FileManagementIndex>();

                }

                ObjLoad.ViewResult = crit.List<FileManagementIndex>().Count > 0 ? true : false;


                //ICriteria cri = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.In("Source", Source.Split(',').Where(A => A != string.Empty && A != null).ToArray()));

                //ObjLoad.FileManagementList = cri.List<FileManagementIndex>();
                mySession.Close();
            }
            return ObjLoad;

        }

        public IList<FileManagementIndex> GetBodyImageList(ulong ulHumanID, ulong ulOrder_Id, string Source)
        {

            string[] source = Source.Split(',');
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Order_ID", ulOrder_Id)).Add(Expression.In("Source", source));
                IList<FileManagementIndex> fileList = crit.List<FileManagementIndex>();
                mySession.Close();
                return fileList;
            }

        }

        public IList<FileManagementIndex> SaveUpdateDeleteFileManagementIndexForRCM(IList<FileManagementIndex> addfileList, IList<FileManagementIndex> updatefileList, IList<FileManagementIndex> deletefileList, string macAddress)
        {
            SaveUpdateDeleteWithTransaction(ref addfileList, updatefileList, deletefileList, macAddress);
            return addfileList;
        }

        public IList<FileManagementIndex> GetListbySourceAndHumanId(ulong human_id, string Source)
        {
            IList<FileManagementIndex> objFileMngIndex = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //CAP-461
                //ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", human_id)).Add(Expression.Eq("Source", Source)).Add(Expression.Eq("Is_Delete", "N"));                
                ISQLQuery crit = mySession.CreateSQLQuery("select * from file_management_index where human_id ='"+ human_id + "' and source ='"+ Source + "' and Is_Delete<>'Y'").AddEntity("a", typeof(FileManagementIndex));               
                objFileMngIndex = crit.List<FileManagementIndex>();
                mySession.Close();
            }

            return objFileMngIndex;
        }

        public int GetFileMgntIndexCount(ulong ulBatchID)
        {
            IList<FileManagementIndex> FileMgntIndexList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                FileMgntIndexList = mySession.CreateSQLQuery("SELECT * FROM file_management_index where Batch_ID=" + ulBatchID + " and doc_type='Encounters' and doc_sub_type='Encounter Forms' and source='SCAN' group by batch_id,human_id,document_date order by human_id").List<FileManagementIndex>();
                mySession.Close();
            }
            return FileMgntIndexList.Count;
        }
        //Added By Suvarnni for Orders Screen
        public IList<FileManagementIndex> GetFileCount(ulong ulhumanID, string Source, string sDocType, string sDocSubType)
        {
            IList<FileManagementIndex> objFileMngIndex = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulhumanID)).Add(Expression.Eq("Source", Source)).Add(Expression.Eq("Document_Type", "")).Add(Expression.Eq("Document_Sub_Type", ""));
                objFileMngIndex = crit.List<FileManagementIndex>();
                mySession.Close();
            }
            return objFileMngIndex;
        }

        public FileManagementDTO GetResultusingResultMasterIdandHumanID(ulong ResultMasterID, ulong humanId)
        {
            FileManagementDTO objfilelist = new FileManagementDTO();
            IList<ResultMaster> resMasterList = new List<ResultMaster>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = mySession.GetNamedQuery("Fill.ResultMasterListWithoutOrderID");
                query.SetString(0, humanId.ToString());
                query.SetString(1, ResultMasterID.ToString());
                ArrayList arr = new ArrayList(query.List());

                if (arr != null)
                {

                    foreach (object[] obj in arr)
                    {
                        ResultMaster resMaster = new ResultMaster();
                        resMaster = new ResultMaster();
                        resMaster.Id = Convert.ToUInt64(obj[0]);
                        resMaster.Created_Date_And_Time = Convert.ToDateTime(obj[1]);
                        resMaster.MSH_Date_And_Time_Of_Message = Convert.ToString(obj[2]);
                        resMaster.temp_property = obj[3].ToString();
                        resMaster.Is_Electronic_Mode = obj[4].ToString();
                        resMasterList.Add(resMaster);
                    }

                }
                if (resMasterList.Count > 0)
                {
                    objfilelist.ResultMasterList = resMasterList;
                }
                //UserManager userMngr = new UserManager();
                //objfilelist.UserList = userMngr.GetMedicalAssistantUserList();
                mySession.Close();
            }
            return objfilelist;

        }

        public IList<FileManagementIndex> GetFileListUsingHumanID(ulong human_id)
        {
            IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql1 = mySession.CreateSQLQuery("Select a.* from file_management_index a where a.Human_ID = '" + human_id + "' and a.generate_link_file_path <>'' and Doc_Sub_Type='Advance Directive' order by a.scan_index_conversion_id desc limit 1;").AddEntity("a", typeof(FileManagementIndex));
                fileList = sql1.List<FileManagementIndex>();
                mySession.Close();
            }
            return fileList;
        }

        public IList<FileManagementIndex> GetFileListUsingFileIndexID(ulong index_id)
        {
            IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql1 = mySession.CreateSQLQuery("Select a.* from file_management_index a where a.File_Management_Index_ID = '" + index_id + "';").AddEntity("a", typeof(FileManagementIndex));
                fileList = sql1.List<FileManagementIndex>();
                mySession.Close();
            }
            return fileList;
        }


        public void SaveFileManagementIndexforNonMedicalFiles(IList<FileManagementIndex> lstFilemngt)
        {
            IList<FileManagementIndex> lstFilemngtnull = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstFilemngt, ref lstFilemngtnull, null, string.Empty, false, false, 0, string.Empty);
        }

        public void SaveFileManagementIndexforDeleteFiles(IList<FileManagementIndex> lstFilemngt)
        {
            IList<FileManagementIndex> lstFilemngtnull = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstFilemngtnull, ref lstFilemngt, null, string.Empty, false, false, 0, string.Empty);
        }

        public IList<FileManagementIndex> GetFileListUsingScanIndexID(ulong Scanindex_id)
        {
            IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql1 = mySession.CreateSQLQuery("Select a.* from file_management_index a where a.Scan_Index_Conversion_ID = '" + Scanindex_id + "';").AddEntity("a", typeof(FileManagementIndex));
                fileList = sql1.List<FileManagementIndex>();
                mySession.Close();
            }
            return fileList;
        }

        public void UpdateFileManagementIndexforIndexing(IList<FileManagementIndex> lstFilemngt)
        {
            IList<FileManagementIndex> lstFilemngtnull = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstFilemngtnull, ref lstFilemngt, null, string.Empty, false, false, 0, string.Empty);
        }
    }
}



