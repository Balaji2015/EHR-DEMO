using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System.Collections;
using Acurus.Capella.Core.DTO;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using System.Threading;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IDocumentManager : IManagerBase<Documents, ulong>
    {
        FillDocuments SaveUpdateDeleteDocument(IList<Documents> SaveDocuList, IList<Documents> UpdateDocuList, IList<Documents> DeleteDocuList, Encounter objEncounter, ulong ulEncounterID, string MacAddress, bool getDocuments);
        FillDocuments GetDocumentsList(ulong ulEncounterID);
        FillDocuments SaveDocumentsAndMoveToNextProcess(IList<Documents> SaveDocuList, IList<Documents> UpdateDocuList, IList<Documents> DeleteDocuList, IList<TreatmentPlan> SavePlanList, IList<TreatmentPlan> updatePlanList, IList<TreatmentPlan> deletePlanList, ulong ulEncounterID, ulong ulHumanID, ulong ulSelectedPhyID, Encounter ObjEncounter, string sUserName, string sButtonName, string sFacilityName, DateTime dtCurrentDateTime, bool bMovetoReview, string MacAddress, string UserRole, int CloseType, bool bIsSurgeryNeeded);
        int BatchOperationsToDocuments(IList<Documents> savelist, IList<Documents> updtList, IList<Documents> delList, ISession MySession, string MACAddress);
        FillTreatmentPlan SaveDocumentsandPlanList(Encounter encounterRecord, IList<Documents> SaveDocuList, IList<Documents> UpdateDocuList, IList<Documents> DeleteDocuList, IList<TreatmentPlan> SavePlanList, IList<TreatmentPlan> updatePlanList, IList<TreatmentPlan> deletePlanList, ulong ulEncounterID, string UserName, string MacAddress);
        FillDocuments GetAllPrintDocumentDetails(string staticField_Names, ulong Physician_ID, string UserlookupField_Name, ulong PhyID, ulong ulEncounterID);
        FillDocuments GetDocumentsListForCheckout(ulong ulEncounterID);
    }
    public partial class DocumentManager : ManagerBase<Documents, ulong>, IDocumentManager
    {
        #region Constructors

        public DocumentManager()
            : base()
        {

        }
        public DocumentManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods
        public FillDocuments GetDocumentsList(ulong ulEncounterID)
        {
            FillDocuments objFillDocument = new FillDocuments();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crt = iMySession.CreateCriteria(typeof(Documents)).Add(Expression.Eq("Encounter_ID", ulEncounterID));
                objFillDocument.DocumentsList = crt.List<Documents>();
                EncounterManager EncMgr = new EncounterManager();

                IList<Encounter> EncList = EncMgr.GetEncounterByEncounterID(ulEncounterID);
                if (EncList != null && EncList.Count > 0)
                {
                    objFillDocument.EncounterObj = EncList[0];

                    TreatmentPlanManager treatMngr = new TreatmentPlanManager();
                    objFillDocument.Treatment_Plan_List = treatMngr.GetTreatmentPlanUsingEncounterIdForPrintDocuments(ulEncounterID, EncList[0].Human_ID);


                    ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Convert.ToUInt64(objFillDocument.EncounterObj.Encounter_Provider_ID)));
                    if (criteria.List<User>() != null && criteria.List<User>().Count > 0)
                    {
                        objFillDocument.EncPhyuserName = criteria.List<User>()[0].user_name;
                    }
                }
                iMySession.Close();
            }
            return objFillDocument;
        }
        public FillDocuments GetDocumentsListForCheckout(ulong ulEncounterID)
        {
            FillDocuments objFillDocument = new FillDocuments();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crt = iMySession.CreateCriteria(typeof(Documents)).Add(Expression.Eq("Encounter_ID", ulEncounterID));
                objFillDocument.DocumentsList = crt.List<Documents>();
                EncounterManager EncMgr = new EncounterManager();

                IList<Encounter> EncList = EncMgr.GetEncounterByEncounterID(ulEncounterID);
                if (EncList != null && EncList.Count > 0)
                {
                    objFillDocument.EncounterObj = EncList[0];

                    //  TreatmentPlanManager treatMngr = new TreatmentPlanManager();
                    //   objFillDocument.Treatment_Plan_List = treatMngr.GetTreatmentPlanUsingEncounterIdForPrintDocuments(ulEncounterID, EncList[0].Human_ID);


                    ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Convert.ToUInt64(objFillDocument.EncounterObj.Encounter_Provider_ID)));
                    if (criteria.List<User>() != null && criteria.List<User>().Count > 0)
                    {
                        objFillDocument.EncPhyuserName = criteria.List<User>()[0].user_name;
                    }
                }
                iMySession.Close();
            }
            return objFillDocument;
        }
        public FillDocuments GetAllPrintDocumentDetails(string staticField_Names, ulong Physician_ID, string UserlookupField_Name, ulong PhyID, ulong ulEncounterID)
        {
            FillDocuments objdto = new FillDocuments();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria staticcriteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", staticField_Names));
                objdto.lstStaticLookup = staticcriteria.List<StaticLookup>();

                ICriteria criteria = iMySession.CreateCriteria(typeof(UserLookup)).Add(Expression.Eq("Physician_ID", Physician_ID)).Add(Expression.Eq("Field_Name", UserlookupField_Name.ToUpper())).AddOrder(Order.Asc("Sort_Order"));
                objdto.lstUserLookup = criteria.List<UserLookup>();

                ICriteria PhysicianCriteria = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", PhyID));
                objdto.PhysicianLibraryList = PhysicianCriteria.List<PhysicianLibrary>();

                ICriteria crt = iMySession.CreateCriteria(typeof(Documents)).Add(Expression.Eq("Encounter_ID", ulEncounterID));
                objdto.DocumentsList = crt.List<Documents>();
                EncounterManager EncMgr = new EncounterManager();

                IList<Encounter> EncList = EncMgr.GetEncounterByEncounterID(ulEncounterID);
                if (EncList != null && EncList.Count > 0)
                {
                    objdto.EncounterObj = EncList[0];
                    TreatmentPlanManager treatMngr = new TreatmentPlanManager();
                    objdto.Treatment_Plan_List = treatMngr.GetTreatmentPlanUsingEncounterIdForPrintDocuments(ulEncounterID, EncList[0].Human_ID);

                    ICriteria criteriauser = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Convert.ToUInt64(objdto.EncounterObj.Encounter_Provider_ID)));
                    if (criteriauser.List<User>() != null && criteriauser.List<User>().Count > 0)
                    {
                        objdto.EncPhyuserName = criteriauser.List<User>()[0].user_name;
                    }
                }

                iMySession.Close();
            }
            return objdto;
        }
        public FillDocuments GetDocumentsListForPlan(ulong ulEncounterID)
        {
            FillDocuments objFillDocument = new FillDocuments();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crt = iMySession.CreateCriteria(typeof(Documents)).Add(Expression.Eq("Encounter_ID", ulEncounterID));
                objFillDocument.DocumentsList = crt.List<Documents>();
                iMySession.Close();
            }
            return objFillDocument;
        }
        public FillDocuments SaveUpdateDeleteDocument(IList<Documents> SaveDocuList, IList<Documents> UpdateDocuList, IList<Documents> DeleteDocuList, Encounter objEncounter, ulong ulEncounterID, string MacAddress, bool getDocuments)
        {
            GenerateXml XMLObj = new GenerateXml();
            IList<Encounter> UpdateList = new List<Encounter>();
            int iTryCount = 0;
        TryAgain:
            int iResult = 0;
            bool bDocumentsConsistent = true; bool bDocumentsConsistent1 = true;
            ISession MySession = Session.GetISession();

            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    //ITransaction trans = null;
                    try
                    {
                        // trans = MySession.BeginTransaction();
                        if (SaveDocuList != null && SaveDocuList.Count > 0 || UpdateDocuList != null && UpdateDocuList.Count > 0 || DeleteDocuList != null && DeleteDocuList.Count > 0)
                        {                           
                            //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveDocuList, UpdateDocuList, DeleteDocuList, MySession, MacAddress);
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveDocuList, ref UpdateDocuList, DeleteDocuList, MySession, MacAddress, false, false, ulEncounterID, string.Empty, ref XMLObj);
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
                            //if (SaveDocuList != null && SaveDocuList.Count > 0 && UpdateDocuList != null && UpdateDocuList.Count > 0)
                            //    bDocumentsConsistent = XMLObj.CheckDataConsistency(SaveDocuList.Concat(UpdateDocuList).Cast<object>().ToList(), false, string.Empty);
                            //else if ((SaveDocuList != null && SaveDocuList.Count > 0) || (UpdateDocuList != null && UpdateDocuList.Count > 0))
                            //{
                            //    if (SaveDocuList != null && SaveDocuList.Count > 0)
                            //        bDocumentsConsistent = XMLObj.CheckDataConsistency(SaveDocuList.Cast<object>().ToList(), false, string.Empty);
                            //    else if (UpdateDocuList != null && UpdateDocuList.Count > 0)
                            //        bDocumentsConsistent = XMLObj.CheckDataConsistency(UpdateDocuList.Cast<object>().ToList(), false, string.Empty);
                            //}
                        }                       
                         MySession.Flush();
                         trans.Commit();
                        //if (bDocumentsConsistent)
                        //{
                        //    trans.Commit();
                        //    XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                        //}
                        //else
                        //    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                        //trans.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception(ex.Message);
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
            //IList<Encounter> encList = new List<Encounter>();
            //if (objEncounter != null)
            //{
            //    IList<Encounter> saveList = new List<Encounter>();                
            //    EncounterManager encMngr = new EncounterManager();             
            //    if (objEncounter != null)
            //    {
            //        encList = encMngr.UpdateEncounter(objEncounter, string.Empty, new object[] { "false" });
            //    }

            //}
            IList<Encounter> ilstenc = new List<Encounter>();
            if (objEncounter != null)
            {
                EncounterManager encMngr = new EncounterManager();
                ilstenc = encMngr.UpdateEncounter(objEncounter, string.Empty, new object[] { "false" });
            }
            //GenerateXml XMLObj = new GenerateXml();              
            FillDocuments FillDocumentList = new FillDocuments();         
            if (SaveDocuList.Count > 0 && UpdateDocuList.Count > 0)
            {
                foreach (Documents rec in SaveDocuList.Concat(UpdateDocuList))
                    FillDocumentList.DocumentsList.Add(rec);
            }
            else if (UpdateDocuList.Count > 0)
            {
                FillDocumentList.DocumentsList = UpdateDocuList;
            }
            
            if (objEncounter != null)
                FillDocumentList.EncounterObj = ilstenc[0];
            return FillDocumentList;
        }
        //Added a param Save_NoMove - If true, save but don't move to the next process. If false, save and move to the next process.
        public FillDocuments SaveDocumentsAndMoveToNextProcess(IList<Documents> SaveDocuList, IList<Documents> UpdateDocuList, IList<Documents> DeleteDocuList, IList<TreatmentPlan> SavePlanList, IList<TreatmentPlan> updatePlanList, IList<TreatmentPlan> deletePlanList, ulong ulEncounterID, ulong ulHumanID, ulong ulSelectedPhyID, Encounter ObjEncounter, string sUserName, string sButtonName, string sFacilityName, DateTime dtCurrentDateTime, bool bMovetoReview, string MacAddress,string UserRole,int CloseType,bool bIsSurgeryNeeded)
        {
            GenerateXml XMLObj = new GenerateXml();
            EncounterManager EncManager = new EncounterManager();
            int iTryCount = 0;
        TryAgain:
            int iResult = 0;
            bool bDocumentsConsistent = true, bDocumentsConsistent1 = true;
            ISession MySession = Session.GetISession();
            ulong encounter_id = 0;
            //  ITransaction trans = null;
            try
            {
                if ((SaveDocuList != null && SaveDocuList.Count > 0 || UpdateDocuList != null && UpdateDocuList.Count > 0 || DeleteDocuList != null && DeleteDocuList.Count > 0) ||
                             (SavePlanList != null && SavePlanList.Count > 0 || updatePlanList != null && updatePlanList.Count > 0 || deletePlanList != null && deletePlanList.Count > 0))
                {
                    using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        try
                        {

                            //    trans = MySession.BeginTransaction();
                            if (SaveDocuList != null && SaveDocuList.Count > 0 || UpdateDocuList != null && UpdateDocuList.Count > 0 || DeleteDocuList != null && DeleteDocuList.Count > 0)
                            {
                                IList<Documents> SaveUpdateList = new List<Documents>();
                                if (SaveDocuList != null && SaveDocuList.Count > 0)
                                    SaveUpdateList = SaveDocuList;

                                if (SaveUpdateList != null && SaveUpdateList.Count > 0 && UpdateDocuList != null && UpdateDocuList.Count > 0)
                                    SaveUpdateList = SaveUpdateList.Concat(UpdateDocuList).ToList();
                                else if (UpdateDocuList != null && UpdateDocuList.Count > 0)
                                    SaveUpdateList = UpdateDocuList;

                                if (SaveUpdateList != null && SaveUpdateList.Count > 0 && DeleteDocuList != null && DeleteDocuList.Count > 0)
                                    SaveUpdateList = SaveUpdateList.Concat(DeleteDocuList).ToList();
                                else if (DeleteDocuList != null && DeleteDocuList.Count > 0)
                                    SaveUpdateList = DeleteDocuList;

                                 encounter_id = 0;
                                if (SaveUpdateList.Count > 0)
                                {
                                    encounter_id = SaveUpdateList[0].Encounter_ID;

                                    //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveDocuList, UpdateDocuList, DeleteDocuList, MySession, MacAddress);
                                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveDocuList, ref UpdateDocuList, DeleteDocuList, MySession, MacAddress, false, false, encounter_id, string.Empty, ref XMLObj);
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
                                    if (SaveDocuList != null && SaveDocuList.Count > 0 && UpdateDocuList != null && UpdateDocuList.Count > 0)
                                        bDocumentsConsistent = XMLObj.CheckDataConsistency(SaveDocuList.Concat(UpdateDocuList).Cast<object>().ToList(), false, string.Empty);
                                    else if ((SaveDocuList != null && SaveDocuList.Count > 0) || (UpdateDocuList != null && UpdateDocuList.Count > 0))
                                    {
                                        if (SaveDocuList != null && SaveDocuList.Count > 0)
                                            bDocumentsConsistent = XMLObj.CheckDataConsistency(SaveDocuList.Cast<object>().ToList(), true, string.Empty);
                                        else if (UpdateDocuList != null && UpdateDocuList.Count > 0)
                                            bDocumentsConsistent = XMLObj.CheckDataConsistency(UpdateDocuList.Cast<object>().ToList(), false, string.Empty);
                                    }
                                }

                            }

                            if (SavePlanList != null)
                            {
                                TreatmentPlanManager treatMngr = new TreatmentPlanManager();
                                IList<TreatmentPlan> SaveUpdateTreatList = new List<TreatmentPlan>();
                                if (SavePlanList != null && SavePlanList.Count > 0)
                                    SaveUpdateTreatList = SavePlanList;

                                if (SaveUpdateTreatList != null && SaveUpdateTreatList.Count > 0 && updatePlanList != null && updatePlanList.Count > 0)
                                    SaveUpdateTreatList = SaveUpdateTreatList.Concat(updatePlanList).ToList();
                                else if (updatePlanList != null && updatePlanList.Count > 0)
                                    SaveUpdateTreatList = updatePlanList;

                                if (SaveUpdateTreatList != null && SaveUpdateTreatList.Count > 0 && deletePlanList != null && deletePlanList.Count > 0)
                                    SaveUpdateTreatList = SaveUpdateTreatList.Concat(deletePlanList).ToList();
                                else if (deletePlanList != null && deletePlanList.Count > 0)
                                    SaveUpdateTreatList = deletePlanList;

                                 encounter_id = 0;
                                if (SaveUpdateTreatList.Count > 0)
                                {
                                    encounter_id = SaveUpdateTreatList[0].Encounter_Id;
                                    // iResult = treatMngr.BatchOperationsToTreatmentPlan(SavePlanList, updatePlanList, deletePlanList, MySession, MacAddress);
                                    iResult = treatMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SavePlanList, ref updatePlanList, deletePlanList, MySession, MacAddress, true, true, encounter_id, string.Empty, ref XMLObj);
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
                                    if (SavePlanList != null && SavePlanList.Count > 0 && updatePlanList != null && updatePlanList.Count > 0)
                                        bDocumentsConsistent1 = XMLObj.CheckDataConsistency(SavePlanList.Concat(updatePlanList).Cast<object>().ToList(), false, string.Empty);
                                    else if ((SavePlanList != null && SavePlanList.Count > 0) || (updatePlanList != null && updatePlanList.Count > 0))
                                    {
                                        if (SavePlanList != null && SavePlanList.Count > 0)
                                            bDocumentsConsistent1 = XMLObj.CheckDataConsistency(SavePlanList.Cast<object>().ToList(), true, string.Empty);
                                        else if (updatePlanList != null && updatePlanList.Count > 0)
                                            bDocumentsConsistent1 = XMLObj.CheckDataConsistency(updatePlanList.Cast<object>().ToList(), false, string.Empty);
                                    }
                                }

                            }
                            //MySession.Flush();
                            //trans.Commit();
                            if (bDocumentsConsistent && bDocumentsConsistent1)
                            {
                               
                                if (XMLObj.itemDoc.InnerXml != string.Empty)
                                {
                                   // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        TreatmentPlanManager obj = new TreatmentPlanManager();
                                        obj.WriteBlob(encounter_id, XMLObj.itemDoc, MySession, SavePlanList, updatePlanList, deletePlanList, XMLObj, false);
                                        // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
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
            }
            catch (Exception ex1)
            {
                //MySession.Close();
                throw new Exception(ex1.Message);
            }

            EncManager.MoveToNextProcessFromPrintDocuments(ObjEncounter, ulEncounterID, ulHumanID, ulSelectedPhyID, sUserName, sButtonName, sFacilityName, MacAddress, dtCurrentDateTime, bMovetoReview, UserRole, CloseType);
            if (bIsSurgeryNeeded==true)
            {
                EncounterManager encMngr = new EncounterManager();
                IList<Encounter> encList = new List<Encounter>();
                Encounter EncRecord = new Encounter();
                encList = EncManager.GetEncounterByEncounterID(ulEncounterID);
                if (encList.Count > 0)
                    EncRecord = encList[0];
                EncRecord.Proceed_with_Surgery_Planned = "Y";

                EncManager.UpdateEncounter(EncRecord, string.Empty, new object[] { "false" });

                WFObjectManager objenco = new WFObjectManager();
                WFObject WFObj = objenco.GetByObjectSystemIdAnfObjType(ulEncounterID, "ENCOUNTER");
                if (WFObj.Current_Process != "SURGERY_COORDINATOR_PROCESS")
                {
                    objenco.MoveToNextProcess(ulEncounterID, "ENCOUNTER", 6, "UNKNOWN", dtCurrentDateTime, string.Empty, null, null);
                }
            }

            //XML for Documents and TreatmentPlan
            //GenerateXml XMLObj = new GenerateXml();
            //ulong encounterid = 0;
            //if (SaveDocuList != null && UpdateDocuList != null && SaveDocuList.Count > 0 || UpdateDocuList.Count > 0)
            //{
            //    for (int l = 0; l < UpdateDocuList.Count; l++)
            //    {
            //        UpdateDocuList[l].Version = UpdateDocuList[l].Version + 1;
            //    }
            //    if (SaveDocuList.Count > 0)
            //    {
            //        encounterid = SaveDocuList[0].Encounter_ID;
            //    }
            //    else
            //    {
            //        encounterid = UpdateDocuList[0].Encounter_ID;
            //    }
            //    List<object> lstObj = SaveDocuList.Concat(UpdateDocuList).Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}
            //if (SaveDocuList != null && UpdateDocuList != null && DeleteDocuList != null && SaveDocuList.Count == 0 && UpdateDocuList.Count == 0 && DeleteDocuList.Count > 0)
            //{
            //    encounterid = DeleteDocuList[0].Encounter_ID;
            //    List<object> lstObj = DeleteDocuList.Cast<object>().ToList();
            //    XMLObj.DeleteXmlNode(encounterid, lstObj, string.Empty);
            //}
            //TreatmentPlan
            //if (SavePlanList != null && updatePlanList != null && SavePlanList.Count > 0 || updatePlanList.Count > 0)
            //{
            //    for (int l = 0; l < updatePlanList.Count; l++)
            //    {
            //        updatePlanList[l].Version = updatePlanList[l].Version + 1;
            //    }
            //    if (SavePlanList.Count > 0)
            //    {
            //        encounterid = SavePlanList[0].Encounter_Id;
            //    }
            //    else
            //    {
            //        encounterid = updatePlanList[0].Encounter_Id;
            //    }
            //    List<object> lstObj = SavePlanList.Concat(updatePlanList).Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}
            //if (SavePlanList != null && updatePlanList != null && deletePlanList != null && SavePlanList.Count == 0 && updatePlanList.Count == 0 && deletePlanList.Count > 0)
            //{
            //    encounterid = deletePlanList[0].Encounter_Id;
            //    List<object> lstObj = deletePlanList.Cast<object>().ToList();
            //    XMLObj.DeleteXmlNode(encounterid, lstObj, string.Empty);
            //}
            //============================
            return GetDocumentsList(ulEncounterID);
        }
        public int BatchOperationsToDocuments(IList<Documents> savelist, IList<Documents> updtList, IList<Documents> delList, ISession MySession, string MACAddress)
        {
            //return SaveUpdateDeleteWithoutTransaction(ref savelist, updtList, delList, MySession, MACAddress);
            GenerateXml XMLObj = new GenerateXml();
            return SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savelist, ref updtList, delList, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
        }
        //public FillTreatmentPlan SaveDocumentsandPlanList(Encounter encounterRecord, IList<Documents> SaveDocuList, IList<Documents> UpdateDocuList, IList<Documents> DeleteDocuList, IList<TreatmentPlan> SavePlanList, IList<TreatmentPlan> updatePlanList, IList<TreatmentPlan> deletePlanList, ulong ulEncounterID, string UserName, string MacAddress)
        //{
        //    EncounterManager EncManager = new EncounterManager();
        //    FillTreatmentPlan objFilltreatmentPlan = new FillTreatmentPlan();
        //    GenerateXml XMLObj = new GenerateXml();
        //    TreatmentPlanManager treatMngr = new TreatmentPlanManager();
        //    int iTryCount = 0;
        //TryAgain:
        //    int iResult = 0;
        //    bool bDocumentsConsistent = true; bool bDocumentsConsistent1 = true;
        //    ISession MySession = Session.GetISession();
        //    try
        //    {
        //        using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //        {
        //            try
        //            {

        //                if ((SaveDocuList != null && SaveDocuList.Count > 0) || (UpdateDocuList != null && UpdateDocuList.Count > 0) || (DeleteDocuList != null && DeleteDocuList.Count > 0))
        //                {
        //                    IList<Documents> SaveUpdateList = new List<Documents>();
        //                    if (SaveDocuList != null && SaveDocuList.Count > 0)
        //                        SaveUpdateList = SaveDocuList.ToList();
        //                    if (UpdateDocuList != null && UpdateDocuList.Count > 0)
        //                        SaveUpdateList = SaveUpdateList.Concat(UpdateDocuList).ToList();
        //                    if (DeleteDocuList != null && DeleteDocuList.Count > 0)
        //                        SaveUpdateList = SaveUpdateList.Concat(DeleteDocuList).ToList();
        //                    ulong encounter_id = 0;
        //                    if (SaveUpdateList.Count > 0)
        //                        encounter_id = SaveUpdateList[0].Encounter_ID;
        //                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveDocuList, ref UpdateDocuList, DeleteDocuList, MySession, MacAddress, true, false, encounter_id, string.Empty, ref XMLObj);
        //                    //iResult = SaveUpdateDeleteWithoutTransaction(ref SaveDocuList, UpdateDocuList, DeleteDocuList, MySession, MacAddress);
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
        //                            // MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        //MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                    if (SaveDocuList != null && SaveDocuList.Count > 0 && UpdateDocuList != null && UpdateDocuList.Count > 0)
        //                    {
        //                        bDocumentsConsistent = XMLObj.CheckDataConsistency(SaveDocuList.Concat(UpdateDocuList).Cast<object>().ToList(), false);
        //                        objFilltreatmentPlan.FillDocumentList.DocumentsList = SaveDocuList.Concat(UpdateDocuList).ToList();
        //                    }
        //                    else if ((SaveDocuList != null && SaveDocuList.Count > 0) || (UpdateDocuList != null && UpdateDocuList.Count > 0))
        //                    {
        //                        if (SaveDocuList != null && SaveDocuList.Count > 0)
        //                        {
        //                            bDocumentsConsistent = XMLObj.CheckDataConsistency(SaveDocuList.Cast<object>().ToList(), false);
        //                            objFilltreatmentPlan.FillDocumentList.DocumentsList = SaveDocuList.ToList();
        //                        }
        //                        else if (UpdateDocuList != null && UpdateDocuList.Count > 0)
        //                        {
        //                            bDocumentsConsistent = XMLObj.CheckDataConsistency(UpdateDocuList.Cast<object>().ToList(), false);
        //                            objFilltreatmentPlan.FillDocumentList.DocumentsList = UpdateDocuList.ToList();
        //                        }
        //                    }
        //                }

        //                if ((SavePlanList != null && SavePlanList.Count > 0) || (updatePlanList != null && updatePlanList.Count > 0) || (deletePlanList != null && deletePlanList.Count > 0))
        //                {
        //                    IList<TreatmentPlan> SaveUpdateList = new List<TreatmentPlan>();
        //                    if (SavePlanList != null && SavePlanList.Count > 0)
        //                        SaveUpdateList = SavePlanList.ToList();
        //                    if (updatePlanList != null && updatePlanList.Count > 0)
        //                        SaveUpdateList = SaveUpdateList.Concat(updatePlanList).ToList();
        //                    if (deletePlanList != null && deletePlanList.Count > 0)
        //                        SaveUpdateList = SaveUpdateList.Concat(deletePlanList).ToList();
        //                    ulong encounter_id = 0;
        //                    if (SaveUpdateList.Count > 0)
        //                        encounter_id = SaveUpdateList[0].Encounter_Id;
        //                    //iResult = treatMngr.BatchOperationsToTreatmentPlan(SavePlanList, updatePlanList, deletePlanList, MySession, MacAddress);
        //                    iResult = treatMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SavePlanList, ref updatePlanList, deletePlanList, MySession, MacAddress, true, false, encounter_id, string.Empty, ref XMLObj);
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
        //                    if (SavePlanList != null && SavePlanList.Count > 0 && updatePlanList != null && updatePlanList.Count > 0)
        //                    {
        //                        bDocumentsConsistent1 = XMLObj.CheckDataConsistency(SavePlanList.Concat(updatePlanList).Cast<object>().ToList(), false);
        //                        objFilltreatmentPlan.Treatment_Plan_List = SavePlanList.Concat(updatePlanList).ToList();
        //                    }
        //                    else if ((SavePlanList != null && SavePlanList.Count > 0) || (updatePlanList != null && updatePlanList.Count > 0))
        //                    {
        //                        if (SavePlanList != null && SavePlanList.Count > 0)
        //                        {
        //                            bDocumentsConsistent1 = XMLObj.CheckDataConsistency(SavePlanList.Cast<object>().ToList(), false);
        //                            objFilltreatmentPlan.Treatment_Plan_List = SavePlanList.ToList();
        //                        }
        //                        else if (updatePlanList != null && updatePlanList.Count > 0)
        //                        {
        //                            bDocumentsConsistent1 = XMLObj.CheckDataConsistency(updatePlanList.Cast<object>().ToList(), false);
        //                            objFilltreatmentPlan.Treatment_Plan_List = updatePlanList.ToList();
        //                        }
        //                    }
        //                }
        //                if (bDocumentsConsistent && bDocumentsConsistent1)
        //                {
        //                    trans.Commit();
        //                    XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
        //                }
        //                else
        //                    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
        //                //MySession.Flush();
        //                //trans.Commit();
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
        //    if (encounterRecord != null)
        //    {
        //        EncounterManager encMngr = new EncounterManager();
        //        encMngr.UpdateEncounter(encounterRecord, string.Empty, new object[] { "false" });              
        //    }
        //   // FillTreatmentPlan objFilltreatmentPlan = new FillTreatmentPlan();
        //    objFilltreatmentPlan.FillDocumentList = new FillDocuments();

        //    //GenerateXml XMLObj = new GenerateXml();
        //    //if (SaveDocuList.Count > 0)
        //    //{               
        //    //    ulong encounterid = SaveDocuList[0].Encounter_ID;
        //    //    foreach (Documents obj in SaveDocuList)
        //    //    {
        //    //        objFilltreatmentPlan.FillDocumentList.DocumentsList.Add(obj);
        //    //    }
        //    //    List<object> lstObj = SaveDocuList.Cast<object>().ToList();                
        //    //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);      

        //    //}
        //    //if (UpdateDocuList.Count > 0)
        //    //{
        //    //    ulong encounterid = UpdateDocuList[0].Encounter_ID;
        //    //    foreach (Documents objd in UpdateDocuList)
        //    //    {
        //    //        objd.Version +=1;
        //    //        objFilltreatmentPlan.FillDocumentList.DocumentsList.Add(objd);
        //    //    }           
        //    //    List<object> lstObj = UpdateDocuList.Cast<object>().ToList();
        //    //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);

        //    //}
        //    //if(DeleteDocuList.Count>0)
        //    //{
        //    //    ulong encounterid = DeleteDocuList[0].Encounter_ID;                
        //    //    List<object> lstObj = DeleteDocuList.Cast<object>().ToList();
        //    //    XMLObj.DeleteXmlNode(encounterid, lstObj, string.Empty);
        //    //}
        //    //if (SavePlanList.Count > 0)
        //    //{             
        //    //    ulong encounterid = SavePlanList[0].Encounter_Id;
        //    //    foreach (var obj in SavePlanList)
        //    //    {                   
        //    //        objFilltreatmentPlan.Treatment_Plan_List.Add(obj);
        //    //    }
        //    //    List<object> lstObj = SavePlanList.Cast<object>().ToList();
        //    //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);               
        //    //}
        //    //if (updatePlanList.Count > 0)
        //    //{
        //    //    ulong encounterid = updatePlanList[0].Encounter_Id;
        //    //    foreach (var obj in updatePlanList)
        //    //    {
        //    //        obj.Version += 1;
        //    //        objFilltreatmentPlan.Treatment_Plan_List.Add(obj);
        //    //    }
        //    //    //if (SavePlanList.Count > 0)
        //    //    //{
        //    //    //    List<object> lstObj = updatePlanList.Cast<object>().ToList();
        //    //    //    XMLObj.GenerateXmlSaveStatic(lstObj, encounterid, string.Empty);
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    if (SavePlanList.Count == 0)
        //    //    //    {
        //    //    //        List<object> lstObj = updatePlanList.Cast<object>().ToList();
        //    //    //        XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
        //    //    //    }
        //    //    //}
        //    //}
        //     //if (deletePlanList.Count > 0)
        //     //{
        //     //    ulong encounterid = deletePlanList[0].Encounter_Id;
        //     //    List<object> lstObj = deletePlanList.Cast<object>().ToList();
        //     //    XMLObj.DeleteXmlNode(encounterid, lstObj, string.Empty);
        //     //}
        //    if (encounterRecord!=null)
        //    {         

        //        //IList<Encounter> ilstenc = new List<Encounter>();
        //        //ilstenc.Add(encounterRecord);
        //        //foreach (var obj in ilstenc)
        //        //    obj.Version += 1;
        //        objFilltreatmentPlan.FillDocumentList.EncounterObj = encounterRecord;
        //        //ulong encounterid = ilstenc[0].Id;
        //        //List<object> lstObj = ilstenc.Cast<object>().ToList();
        //        //XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);

        //    }           
        //    return objFilltreatmentPlan;
        //}
        public FillTreatmentPlan SaveDocumentsandPlanList(Encounter encounterRecord, IList<Documents> SaveDocuList, IList<Documents> UpdateDocuList, IList<Documents> DeleteDocuList, IList<TreatmentPlan> SavePlanList, IList<TreatmentPlan> updatePlanList, IList<TreatmentPlan> deletePlanList, ulong ulEncounterID, string UserName, string MacAddress)
        {
            EncounterManager EncManager = new EncounterManager();
            TreatmentPlanManager treatMngr = new TreatmentPlanManager();
            GenerateXml XMLObj = new GenerateXml();
            int iTryCount = 0;
        TryAgain:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {

                        if ((SaveDocuList != null && SaveDocuList.Count > 0) || (UpdateDocuList != null && UpdateDocuList.Count > 0) || (DeleteDocuList != null && DeleteDocuList.Count > 0))
                        {
                            // iResult = SaveUpdateDeleteWithoutTransaction(ref SaveDocuList, UpdateDocuList, DeleteDocuList, MySession, MacAddress);
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveDocuList, ref UpdateDocuList, DeleteDocuList, MySession, MacAddress, false, false, ulEncounterID, string.Empty, ref XMLObj);
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

                        if ((SavePlanList != null && SavePlanList.Count > 0) || (updatePlanList != null && updatePlanList.Count > 0) || (deletePlanList != null && deletePlanList.Count > 0))
                        {
                            bool isPhone_Encounter = false;
                            iResult = treatMngr.BatchOperationsToTreatmentPlan(SavePlanList, updatePlanList, deletePlanList, MySession, MacAddress, isPhone_Encounter);
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
            IList<Encounter> ilstenc = new List<Encounter>();
            if (encounterRecord != null)
            {
                EncounterManager encMngr = new EncounterManager();
                ilstenc= encMngr.UpdateEncounter( encounterRecord, string.Empty, new object[] { "false" });               
            }
            FillTreatmentPlan objFilltreatmentPlan = new FillTreatmentPlan();
            objFilltreatmentPlan.FillDocumentList = new FillDocuments();
            if (SaveDocuList.Count > 0 && UpdateDocuList.Count>0)
            {
             foreach (Documents rec in SaveDocuList.Concat(UpdateDocuList))
                objFilltreatmentPlan.FillDocumentList.DocumentsList.Add( rec);
            }
            else if (UpdateDocuList.Count > 0){
                objFilltreatmentPlan.FillDocumentList.DocumentsList = UpdateDocuList;
            }
            else if (SaveDocuList.Count > 0)
            {
                objFilltreatmentPlan.FillDocumentList.DocumentsList = SaveDocuList;
            }
            if (SavePlanList.Count > 0 && updatePlanList.Count>0)
            {
                foreach (TreatmentPlan plan in SavePlanList.Concat(updatePlanList))
                    objFilltreatmentPlan.Treatment_Plan_List.Add(plan);
            }
            else if (SavePlanList.Count > 0)
            {
                objFilltreatmentPlan.Treatment_Plan_List = SavePlanList;
            }
            else if (updatePlanList.Count > 0)
            {
                objFilltreatmentPlan.Treatment_Plan_List = updatePlanList;
            }
            if (encounterRecord != null)
            {
                objFilltreatmentPlan.FillDocumentList.EncounterObj = ilstenc[0];
            }

            //GenerateXml XMLObj = new GenerateXml();
            //if (SaveDocuList.Count > 0)
            //{
            //    ulong encounterid = SaveDocuList[0].Encounter_ID;
            //    foreach (Documents obj in SaveDocuList)
            //    {
            //        objFilltreatmentPlan.FillDocumentList.DocumentsList.Add(obj);
            //    }
            //    List<object> lstObj = SaveDocuList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);

            //}
            //if (UpdateDocuList.Count > 0)
            //{
            //    ulong encounterid = UpdateDocuList[0].Encounter_ID;
            //    foreach (Documents objd in UpdateDocuList)
            //    {
            //        objd.Version += 1;
            //        objFilltreatmentPlan.FillDocumentList.DocumentsList.Add(objd);
            //    }
            //    List<object> lstObj = UpdateDocuList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);

            //}
            //if (DeleteDocuList.Count > 0)
            //{
            //    ulong encounterid = DeleteDocuList[0].Encounter_ID;
            //    List<object> lstObj = DeleteDocuList.Cast<object>().ToList();
            //    XMLObj.DeleteXmlNode(encounterid, lstObj, string.Empty);
            //}
            //if (SavePlanList.Count > 0)
            //{
            //    ulong encounterid = SavePlanList[0].Encounter_Id;
            //    foreach (var obj in SavePlanList)
            //    {
            //        objFilltreatmentPlan.Treatment_Plan_List.Add(obj);
            //    }
            //    List<object> lstObj = SavePlanList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}
            //if (updatePlanList.Count > 0)
            //{
            //    ulong encounterid = updatePlanList[0].Encounter_Id;
            //    foreach (var obj in updatePlanList)
            //    {
            //        obj.Version += 1;
            //        objFilltreatmentPlan.Treatment_Plan_List.Add(obj);
            //    }
            //    if (SavePlanList.Count > 0)
            //    {
            //        List<object> lstObj = updatePlanList.Cast<object>().ToList();
            //        XMLObj.GenerateXmlSaveStatic(lstObj, encounterid, string.Empty);
            //    }
            //    else
            //    {
            //        if (SavePlanList.Count == 0)
            //        {
            //            List<object> lstObj = updatePlanList.Cast<object>().ToList();
            //            XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //        }
            //    }
            //}
            //if (deletePlanList.Count > 0)
            //{
            //    ulong encounterid = deletePlanList[0].Encounter_Id;
            //    List<object> lstObj = deletePlanList.Cast<object>().ToList();
            //    XMLObj.DeleteXmlNode(encounterid, lstObj, string.Empty);
            //}
            //if (encounterRecord != null)
            //{

            //    IList<Encounter> ilstenc = new List<Encounter>();
            //    ilstenc.Add(encounterRecord);
            //    foreach (var obj in ilstenc)
            //        obj.Version += 1;
            //    objFilltreatmentPlan.FillDocumentList.EncounterObj = encounterRecord;
            //    ulong encounterid = ilstenc[0].Id;
            //    List<object> lstObj = ilstenc.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);

            //}
            return objFilltreatmentPlan;
        }
        #endregion
    }
}
