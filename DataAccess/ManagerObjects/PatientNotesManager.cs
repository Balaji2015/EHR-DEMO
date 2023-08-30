using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using Acurus.Capella.Core.DTO;
using System.Collections;
using NHibernate.Criterion;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IPatientNotesManager : IManagerBase<PatientNotes, ulong>
    {                        
        IList<PatientNotes> Getpatientdetails(string humanid);
        IList<PatientNotes> Getpatienttask(string user, string humanid);
        IList<PatientNotes> GetPopupDetails(string humanid);
        IList<PatientNotes> GetMessageTask(string humanid);                
        ulong GetPhysicianIdNew(string Facility);        
        IList<PatientNotes> AddToPatientNotes(IList<PatientNotes> patientlst);        
        IList<PatientNotes> GetIsPopUp(string HumanId);
        IList<PatientNotes> AddPatientlst(IList<PatientNotes> patientlst, IList<PatientNotes> PatientNotesUpdateList, IList<WFObject> WFobjUpdateList);
        void UpdatePopUpTable(IList<PatientNotes> iListPopup, string MacAddress);
        void SavePatientMessage(PatientNotes messages, WFObject Wfobj, string MacAddress);
        IList<PatientNotes> GetPatientNotesByMsgID(ulong ulMsgID);
        void SavePatientNotes(IList<PatientNotes> SaveList, string MacAddress);
        void UpdatePatientMessage(PatientNotes messages, string ObjType, int CloseType, string owner, DateTime startTime, string MacAddress);
        IList<string> MapPhysicianUserListForFacility(string sFacilityName, string sLegalOrg);
        IList<PatientNotes> GetMessageDetails(string MessageDescription, string MessageNotes, string humanid);
        IList<PatientNotes> GetFilteredMessageDetailsByEncounterId(string MessageDescription, string MessageNotes, string EncounterID, string HumanID);                                
        IList<PatientNotes> GetFilteredLineItemMessages(string MessageDescription, string MessageNotes, string HumanID, string ChargeLineID, string ChargeHeaderID);
        IList<PatientNotes> GetFilteredMessageDetails(string MessageDescription, string MessageNotes, string HumanID);                        
        ulong SavePatientAndreturnid(IList<PatientNotes> SaveList, string MacAddress);        
        PatientNotes GetByPatientObjectID(ulong PatientId);
        int SavePatientWithoutTransaction(IList<PatientNotes> ListToInsert, ISession MySession, string MACAddress);
        PatientDetailDto GetPopupDetailsNew(string humanid);
    }
    public partial class PatientNotesManager : ManagerBase<PatientNotes, ulong>, IPatientNotesManager
    {
        #region Constructors

        public PatientNotesManager()
            : base()
        {

        }
        public PatientNotesManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods                
        public IList<PatientNotes> AddPatientlst(IList<PatientNotes> patientlst,IList<PatientNotes> PatientNotesUpdateList,IList<WFObject> WFobjUpdateList)
        {           
            ISession MySession = Session.GetISession();
            iTryCount = 0;
            GenerateXml ObjXML = null;
        TryAgain:
            int iResult = 0;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if ((patientlst != null && patientlst.Count > 0) || (PatientNotesUpdateList != null && PatientNotesUpdateList.Count > 0))
                        {

                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref patientlst, ref PatientNotesUpdateList, null, MySession, string.Empty, false, false, 0, "",ref ObjXML);

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
                        if (WFobjUpdateList != null && WFobjUpdateList.Count > 0)
                        {
                            WFObjectManager WFObjMngr = new WFObjectManager();
                            IList<WFObject> WFObjdumlist = new List<WFObject>();
                            iResult = WFObjMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref WFObjdumlist, ref WFobjUpdateList, null, MySession, string.Empty, false, false, 0, "", ref ObjXML);

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
            return patientlst;
        }
        public IList<PatientNotes> AddToPatientNotes(IList<PatientNotes> SaveList)
        {
            ISession MySession = Session.GetISession();
            IList<PatientNotes> PatNotes = null;
            //GenerateXml XMLObj = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveList, ref PatNotes, null, string.Empty, false, false, 0, string.Empty);
           // SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveList, ref PatNotes, null, MySession, string.Empty, false, false, 0, "", ref XMLObj);
            return SaveList;
        }                
        public IList<PatientNotes> Getpatientdetails(string humanid)
        {
            //ArrayList arrList = null; 
            ArrayList arrypatientdetail = new ArrayList();
            PatientNotes objpatientnotes = new PatientNotes();
            IList<PatientNotes> assign = new List<PatientNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query =iMySession.GetNamedQuery("Get.patientdetail");            
              
                query.SetString(0, humanid);
                
                arrypatientdetail = new ArrayList(query.List());
                iMySession.Close();
            }
            foreach (object[] obj in arrypatientdetail)
            {
                objpatientnotes = new PatientNotes();
                //StaticLookup=new StaticLookup();

                objpatientnotes.Type = Convert.ToString(obj[0]);
                objpatientnotes.Source = Convert.ToString(obj[1]);
                objpatientnotes.SourceID = Convert.ToInt32(obj[2]);
                objpatientnotes.Human_ID = Convert.ToUInt64(obj[3]);
                //objpatientnotes.PatientName =Convert.ToString(obj[4]);
                //objpatientnotes.DOB = Convert.ToDateTime(obj[5]);
                //objpatientnotes.Message_Date_And_Time = Convert.ToDateTime(obj[6]);
                objpatientnotes.Message_Description = Convert.ToString(obj[7]);
                objpatientnotes.Facility_Name = Convert.ToString(obj[8]);
                objpatientnotes.Assigned_To = Convert.ToString(obj[9]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[10]);
                objpatientnotes.Modified_By = Convert.ToString(obj[11]);
                objpatientnotes.Notes = Convert.ToString(obj[12]);
                objpatientnotes.Priority = Convert.ToString(obj[13]);
                objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[14]);
                objpatientnotes.Created_By = Convert.ToString(obj[15]);

                assign.Add(objpatientnotes);

            }

            return assign;


        }
        public IList<PatientNotes> Getpatienttask(string user, string humanid)
        {
            //ArrayList arrList = null;
            IList<PatientNotes> assign = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();
            ArrayList arrypatientdetail = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.Taskpatientdetail");

                query.SetString(0, user);
                query.SetString(1, humanid);               
                arrypatientdetail = new ArrayList(query.List());
                iMySession.Close();
            }
            foreach (object[] obj in arrypatientdetail)
            {
                objpatientnotes = new PatientNotes();
                //StaticLookup=new StaticLookup();

                objpatientnotes.Type = Convert.ToString(obj[0]);
                objpatientnotes.Source = Convert.ToString(obj[1]);
                objpatientnotes.SourceID = Convert.ToInt32(obj[2]);
                objpatientnotes.Human_ID = Convert.ToUInt64(obj[3]);
                //objpatientnotes.PatientName = Convert.ToString(obj[4]);
                //objpatientnotes.DOB = Convert.ToDateTime(obj[5]);
                objpatientnotes.Message_Date_And_Time = Convert.ToDateTime(obj[6]);
                objpatientnotes.Message_Description = Convert.ToString(obj[7]);
                objpatientnotes.Facility_Name = Convert.ToString(obj[8]);
                objpatientnotes.Assigned_To = Convert.ToString(obj[9]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[10]);
                objpatientnotes.Modified_By = Convert.ToString(obj[11]);
                //srividhya
                objpatientnotes.Notes = Convert.ToString(obj[12]);
                objpatientnotes.Priority = Convert.ToString(obj[13]);
                objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[14]);
                objpatientnotes.Created_By = Convert.ToString(obj[15]);
                assign.Add(objpatientnotes);

            }

            return assign;


        }
        public IList<PatientNotes> GetPopupDetails(string humanid)
        {
           // ArrayList arrList = null;
            IList<PatientNotes> assign = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();

            ArrayList arrypatientdetail = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.Popupdetail");
                query.SetString(0, humanid);
                arrypatientdetail = new ArrayList(query.List());
                iMySession.Close();
            }
            foreach (object[] obj in arrypatientdetail)
            {
                objpatientnotes = new PatientNotes();
                objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[0]);
                objpatientnotes.Source = Convert.ToString(obj[1]);
                objpatientnotes.Message_Description = Convert.ToString(obj[2]);
                objpatientnotes.Notes = Convert.ToString(obj[3]);
                objpatientnotes.Priority = Convert.ToString(obj[7]);
                objpatientnotes.Created_By = Convert.ToString(obj[4]);
                objpatientnotes.Modified_By = Convert.ToString(obj[5]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[6]);
                objpatientnotes.Id = Convert.ToUInt64(obj[10]);
                objpatientnotes.Type = Convert.ToString(obj[11]);
                objpatientnotes.Assigned_To = Convert.ToString(obj[12]);
                objpatientnotes.Caller_Name = Convert.ToString(obj[13]);
                objpatientnotes.Relationship = Convert.ToString(obj[14]);
                objpatientnotes.Message_Date_And_Time = Convert.ToDateTime(obj[15]);
                objpatientnotes.Encounter_ID = Convert.ToInt32(obj[16]);
                objpatientnotes.Line_ID = Convert.ToUInt64(obj[17]);
                objpatientnotes.Line_Type = Convert.ToString(obj[18]);
                objpatientnotes.Header_ID = Convert.ToUInt64(obj[19]);
                objpatientnotes.Header_Type = Convert.ToString(obj[20]);
                assign.Add(objpatientnotes);

            }


            return assign;


        }
        public IList<PatientNotes> Getlineitem(string humanid, string chargeline)
        {
            //ArrayList arrList = null;
            IList<PatientNotes> assign = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();
            ArrayList arrypatientdetail = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.lineitemMessages");
                query.SetString(0, humanid);
                query.SetString(1, chargeline.ToString());
                arrypatientdetail = new ArrayList(query.List());
                iMySession.Close();
            }
            foreach (object[] obj in arrypatientdetail)
            {
                objpatientnotes = new PatientNotes();
                //StaticLookup=new StaticLookup();
                objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[0]);
                objpatientnotes.Source = Convert.ToString(obj[1]);
                objpatientnotes.Message_Description = Convert.ToString(obj[2]);
                objpatientnotes.Notes = Convert.ToString(obj[3]);
                objpatientnotes.Priority = Convert.ToString(obj[4]);
                objpatientnotes.Created_By = Convert.ToString(obj[5]);
                objpatientnotes.Modified_By = Convert.ToString(obj[6]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[7]);
                objpatientnotes.Batch_ID = Convert.ToInt32(obj[9]);
                objpatientnotes.Carrier_ID = obj[10].ToString();
                objpatientnotes.Type = obj[11].ToString();
                objpatientnotes.Message_Code = obj[12].ToString();
                if (Convert.ToInt32(obj[8]) != 0)
                {
                    objpatientnotes.SourceID = Convert.ToInt32(obj[8]);
                }
                assign.Add(objpatientnotes);

            }

            return assign;


        }
        public IList<PatientNotes> GetMessageTask(string humanid)
        {
            //ArrayList arrList = null;
              IList<PatientNotes> assign = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();
            ArrayList arrypatientdetail = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.MessageTask");
                query.SetString(0, humanid);
                arrypatientdetail = new ArrayList(query.List());
                iMySession.Close();
            }
            foreach (object[] obj in arrypatientdetail)
            {
                objpatientnotes = new PatientNotes();
                //StaticLookup=new StaticLookup();
                objpatientnotes.Human_ID = Convert.ToUInt64(obj[0]);
                objpatientnotes.Assigned_To = Convert.ToString(obj[1]);
                objpatientnotes.Relationship = Convert.ToString(obj[2]);
                objpatientnotes.Facility_Name = Convert.ToString(obj[3]);
                objpatientnotes.Caller_Name = Convert.ToString(obj[4]);
                objpatientnotes.Message_Orign = Convert.ToString(obj[5]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[6]);
                objpatientnotes.Message_Description = Convert.ToString(obj[7]);
                objpatientnotes.Notes = Convert.ToString(obj[8]);
                objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[9]);
                objpatientnotes.Created_By = Convert.ToString(obj[10]);
                objpatientnotes.Type = Convert.ToString(obj[11]);
                if (Convert.ToString(obj[12]) != string.Empty)
                {
                    objpatientnotes.Source = Convert.ToString(obj[12]);
                }
                if (Convert.ToInt32(obj[13]) != 0)
                {
                    objpatientnotes.SourceID = Convert.ToInt32(obj[13]);
                }
                //objpatientnotes.PatientName = Convert.ToString(obj[14]);
                //objpatientnotes.DOB = Convert.ToDateTime(obj[15]);
                objpatientnotes.Priority = Convert.ToString(obj[14]);
                objpatientnotes.Line_ID = Convert.ToUInt64(obj[15]);
                objpatientnotes.Modified_By = Convert.ToString(obj[16]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[17]);


                //objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[9]);
                //if (objpatientnotes.Source != string.Empty)
                //{
                //    objpatientnotes.Source = Convert.ToString(obj[12]);
                //}
                //objpatientnotes.Created_By = Convert.ToString(obj[10]);
                //objpatientnotes.Notes = Convert.ToString(obj[8]);

                assign.Add(objpatientnotes);

            }

            return assign;


        }
        public ulong GetPhysicianIdNew(string Facility)
        {
            //ArrayList arrList = null;
            //IList<MapFacilityPhysician> assign = new List<MapFacilityPhysician>();
            //MapFacilityPhysician objMapPhysician = new MapFacilityPhysician();
            ArrayList arryPhysicianId = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.PhysicianId");
                query.SetString(0, Facility);
                arryPhysicianId = new ArrayList(query.List());
                iMySession.Close();
            }
            //foreach (object[] obj in arryPhysicianId)
            //{
            ulong PhysicianId = 0;
            for (int i = 0; i < arryPhysicianId.Count; i++)
            {
                //objMapPhysician = new MapFacilityPhysician();
                ////StaticLookup=new StaticLookup();

                PhysicianId = Convert.ToUInt64(arryPhysicianId[0]);


                //assign.Add(objMapPhysician);
            }

            //}

            return PhysicianId;


        }
        public IList<MapFacilityPhysician> GetPhysicianId(string Facility)
        {
            //ArrayList arrList = null;
            IList<MapFacilityPhysician> assign = new List<MapFacilityPhysician>();
            MapFacilityPhysician objMapPhysician = new MapFacilityPhysician();
            ArrayList arryPhysicianId = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.PhysicianId");
                query.SetString(0, Facility);
                arryPhysicianId = new ArrayList(query.List());
                iMySession.Close();
            }
            //foreach (object[] obj in arryPhysicianId)
            //{
            for (int i = 0; i < arryPhysicianId.Count; i++)
            {
                objMapPhysician = new MapFacilityPhysician();
                //StaticLookup=new StaticLookup();

                objMapPhysician.Phy_Rec_ID = Convert.ToUInt64(arryPhysicianId[0]);


                assign.Add(objMapPhysician);
            }

            //}

            return assign;


        }
        public IList<PatientNotes> GetIsPopUp(string HumanId)
        {


            IList<PatientNotes> PopUpLIst = new List<PatientNotes>();
            PatientNotes objPatientNotes = new PatientNotes();
            ArrayList arrList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetIsPopUp");
                query.SetString(0, HumanId);                
                arrList = new ArrayList(query.List());
                iMySession.Close();
            }


            if (arrList != null)
            {
                for (int i = 0; i < arrList.Count; i++)
                {
                    objPatientNotes = new PatientNotes();
                    //StaticLookup=new StaticLookup();

                    objPatientNotes.Is_PopupEnable = arrList[i].ToString();
                    PopUpLIst.Add(objPatientNotes);

                }
            }
            return PopUpLIst;


        }

        int iTryCount = 0;
        public void UpdatePopUpTable(IList<PatientNotes> iListPopup, string MacAddress)
        {
      #region
        //    ISession MySession = Session.GetISession();
        //    ITransaction trans = null;
        //    trans = MySession.BeginTransaction();
        //    int iResult = 0;
        //    GenerateXml ObjXML = null;
        //TryAgain:
        //    try
        //    {
        //        IList<PatientNotes> PatientDummy = new List<PatientNotes>();
        //        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref PatientDummy, ref iListPopup, null, MySession, MacAddress, false, false, 0, "", ref ObjXML);
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
        //                //MySession.Close();
        //                throw new Exception("Deadlock is occured. Transaction failed");
        //            }
        //        }
        //        else if (iResult == 1)
        //        {
        //            trans.Rollback();
        //            //MySession.Close();
        //            throw new Exception("Exception is occured. Transaction failed");
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
        //    // return iListAuth[0].Id;
     #endregion
                IList<PatientNotes> PatientDummy = new List<PatientNotes>();
                SaveUpdateDelete_DBAndXML_WithTransaction(ref PatientDummy, ref iListPopup, null, MacAddress, false, false, 0, string.Empty);
        }

        public IList<PatientNotes> GetPatientNotesByMsgID(ulong ulMsgID)
        {
            IList<PatientNotes> lstpatNotes = new List<PatientNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(PatientNotes)).Add(Expression.Eq("Id", ulMsgID));
                lstpatNotes = criteria.List<PatientNotes>();
                iMySession.Close();
            }
            return lstpatNotes;
        }

        public void SavePatientNotes(IList<PatientNotes> SaveList, string MacAddress)
        {
            IList<PatientNotes> PatNotesTemp = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveList, ref PatNotesTemp, null, MacAddress, false, false, 0, "");
        }
        public void SavePatientMessage(PatientNotes messages, WFObject Wfobj, string MacAddress)
        {

            IList<PatientNotes> messageList = new List<PatientNotes>();
            messageList.Add(messages);
            int iTryCount = 0;
            GenerateXml ObjXML = null;
        TryAgain:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = MySession.BeginTransaction();
                if (messageList != null && messageList.Count > 0)
                {
                    IList<PatientNotes> PatNotesTemp = null;
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref messageList, ref PatNotesTemp, null, MySession, MacAddress, false, false, 0, "", ref ObjXML);
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


                if (Wfobj != null)
                {
                    Wfobj.Obj_System_Id = messageList[0].Id;
                    WFObjectManager WFObjectManager = new WFObjectManager();
                    iResult = WFObjectManager.InsertToWorkFlowObject(Wfobj, 1, MacAddress, MySession);

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
        public void UpdatePatientMessage(PatientNotes messages, string ObjType, int CloseType, string owner, DateTime startTime, string MacAddress)
        {
            IList<PatientNotes> messageList = new List<PatientNotes>();
            IList<PatientNotes> nullList = new List<PatientNotes>();
            nullList = null;
            messageList.Add(messages);
            if (ObjType == string.Empty)
            {
                ObjType = "TASK";
                SaveUpdateDelete_DBAndXML_WithTransaction(ref nullList, ref messageList, null, MacAddress, false, false, 0, "");
                WFObjectManager objWf = new WFObjectManager();
                objWf.UpdateOwner(messages.Id, ObjType, owner, MacAddress);
            }
            else
            {
                int iTryCount = 0;
            TryAgain:
                int iResult = 0;
            GenerateXml objXMl = null;
                ISession MySession = Session.GetISession();
                ITransaction trans = null;
                try
                {
                    trans = MySession.BeginTransaction();
                    if (messageList != null && messageList.Count > 0)
                    {
                        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref nullList, ref messageList, null, MySession, MacAddress, false, false, 0, "", ref objXMl);
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
                    if (messageList != null && messageList.Count > 0)
                    {
                        WFObjectManager objWf = new WFObjectManager();
                        WFObject WFObj = objWf.GetByObjectSystemId(messages.Id, ObjType);
                        iResult = objWf.MoveToNextProcess(WFObj.Obj_System_Id, WFObj.Obj_Type, CloseType, owner, startTime, MacAddress, null, MySession);
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
        public IList<string> MapPhysicianUserListForFacility(string sFacilityName, string sLegalOrg)
        {
            IList<string> UserList = new List<string>();
            string sPhyName = string.Empty;
            if (sFacilityName == "SHOW ALL")
            {
                //Old Code
                //UserManager UserMgr = new UserManager();
                //IList<User> ListUser = UserMgr.GetUserList(sLegalOrg);
                //IList<User> lstUserName = (from user in ListUser where user.status.ToUpper() == "A" select user).ToList<User>();
                //UserList = lstUserName.Select(u => u.user_name + "|" + u.person_name).ToList<string>();
                //Gitlab# 2485 - Physician Name Display Change
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    IList<object> objLst = iMySession.CreateSQLQuery("select * from (select u.user_name as UserName,u.person_name as LastName,'' as FirstName,'' as MI,'' as Suffix from user u where status = 'a' and u.Legal_Org='"+sLegalOrg+ "' and u.physician_library_id = 0 union all select u.user_name as UserName,p.Physician_Last_Name as LastName, Physician_First_Name as FirstName, Physician_Middle_Name as MI,Physician_Suffix as Suffix from user u, physician_library p where status = 'a' and u.physician_library_id <> 0 and u.Legal_Org='" + sLegalOrg + "' and u.Physician_Library_ID = p.Physician_Library_ID) as a order by LastName,FirstName").List<object>();

                    for (int i = 0; i < objLst.Count; i++)
                    {
                        object[] obj = (object[])objLst[i];
                        sPhyName = string.Empty;

                        if (obj[1].ToString() != String.Empty)
                            sPhyName += obj[1].ToString();
                        if (obj[2].ToString() != String.Empty)
                        {
                            if (sPhyName != String.Empty)
                                sPhyName += "," + obj[2].ToString();
                            else
                                sPhyName += obj[2].ToString();
                        }
                        if (obj[3].ToString() != String.Empty)
                            sPhyName += " " + obj[3].ToString();
                        if (obj[4].ToString() != String.Empty)
                            sPhyName += "," + obj[4].ToString();

                        UserList.Add(obj[0].ToString() + "|" + sPhyName);
                    }
                    iMySession.Close();
                }
            }
            else
            {
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    //Old Code
                    //UserList = iMySession.CreateSQLQuery("select concat(us.user_name ,'|',us.person_name) UserName from Map_facility_user f inner join map_facility_physician p on (f.facility_Name=p.facility_Name) inner join user us on (if(us.Physician_library_Id=0,us.user_Name=f.User_name,us.Physician_library_Id=p.Physician_Id)) where f.facility_Name='" + sFacilityName + "' group by UserName").List<string>();

                    //IList<object> objLst = iMySession.CreateSQLQuery("select u.user_name ,'',u.person_name,'','' from Map_facility_user f,user u where u.Physician_Library_Id = 0 and u.user_name = f.user_name and f.facility_Name = '" + sFacilityName + "' group by u.user_name union all select u.user_name, phy.Physician_Last_Name, phy.Physician_First_Name, phy.Physician_Middle_Name, phy.Physician_Suffix from user u, map_facility_physician p, physician_library phy, Map_facility_user where  u.physician_library_id <> 0 and u.Physician_Library_ID = p.Physician_ID and u.physician_library_id = phy.physician_library_id  and   p.facility_Name = '" + sFacilityName + "' group by user_name").List<object>();

                    IList<object> objLst = iMySession.CreateSQLQuery("select * from (select u.user_name as UserName,u.person_name as LastName,'' as FirstName,'' as MI,'' as Suffix from Map_facility_user f,user u where u.Physician_Library_Id = 0 and u.Legal_Org='" + sLegalOrg + "' and  u.user_name = f.user_name and f.facility_Name = '" + sFacilityName + "' group by UserName union all select u.user_name as UserName, phy.Physician_Last_Name as LastName, phy.Physician_First_Name as FirstName, phy.Physician_Middle_Name as MI, phy.Physician_Suffix as Suffix from user u, map_facility_physician p, physician_library phy, Map_facility_user where  u.physician_library_id <> 0 and u.Legal_Org='" + sLegalOrg + "' and u.Physician_Library_ID = p.Physician_ID and u.physician_library_id = phy.physician_library_id  and   p.facility_Name = '" + sFacilityName + "' group by UserName) as a order by LastName,FirstName").List<object>();

                    for (int i = 0; i < objLst.Count; i++)
                    {
                        object[] obj = (object[])objLst[i];
                        sPhyName = string.Empty;

                        if (obj[1].ToString() != String.Empty)
                            sPhyName += obj[1].ToString();
                        if (obj[2].ToString() != String.Empty)
                        {
                            if (sPhyName != String.Empty)
                                sPhyName += "," + obj[2].ToString();
                            else
                                sPhyName += obj[2].ToString();
                        }
                        if (obj[3].ToString() != String.Empty)
                            sPhyName += " " + obj[3].ToString();
                        if (obj[4].ToString() != String.Empty)
                            sPhyName += "," + obj[4].ToString();

                        UserList.Add(obj[0].ToString() + "|" + sPhyName);
                    }

                    iMySession.Close();
                }
            }
            return UserList;

        }
        public IList<PatientNotes> GetMessageDetails(string MessageDescription, string MessageNotes, string humanid)
        {
            //ArrayList arrList = null;
            IList<PatientNotes> assign = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();

            ArrayList arrypatientdetail = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.MessageInfo");
                query.SetString(0, MessageDescription + "%");
                query.SetString(1, "%" + MessageNotes + "%");
                query.SetString(2, humanid);               
                arrypatientdetail = new ArrayList(query.List());
                iMySession.Close();
            }
            foreach (object[] obj in arrypatientdetail)
            {
                objpatientnotes = new PatientNotes();
                objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[0]);
                objpatientnotes.Source = Convert.ToString(obj[1]);
                objpatientnotes.Message_Description = Convert.ToString(obj[2]);
                objpatientnotes.Notes = Convert.ToString(obj[3]);
                objpatientnotes.Priority = Convert.ToString(obj[7]);
                objpatientnotes.Created_By = Convert.ToString(obj[4]);
                objpatientnotes.Modified_By = Convert.ToString(obj[5]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[6]);
                objpatientnotes.Id = Convert.ToUInt64(obj[10]);
                objpatientnotes.Type = Convert.ToString(obj[11]);
                objpatientnotes.Assigned_To = Convert.ToString(obj[12]);
                objpatientnotes.Caller_Name = Convert.ToString(obj[13]);
                objpatientnotes.Relationship = Convert.ToString(obj[14]);
                objpatientnotes.Message_Date_And_Time = Convert.ToDateTime(obj[15]);
                objpatientnotes.Encounter_ID = Convert.ToInt32(obj[16]);
                objpatientnotes.Line_ID = Convert.ToUInt64(obj[17]);
                objpatientnotes.Line_Type = Convert.ToString(obj[18]);
                objpatientnotes.Header_ID = Convert.ToUInt64(obj[19]);
                objpatientnotes.Header_Type = Convert.ToString(obj[20]);
                assign.Add(objpatientnotes);

            }

            return assign;


        }        
        public IList<PatientNotes> GetFilteredMessageDetailsByEncounterId(string MessageDescription, string MessageNotes, string EncounterID, string HumanID)
        {
            //ArrayList arrList = null;
            IList<PatientNotes> assign = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();

            ArrayList arrypatientdetail = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query = iMySession.GetNamedQuery("Get.FilteredMessageInfo");
                query.SetString(0, MessageDescription + "%");
                query.SetString(1, "%" + MessageNotes + "%");
                query.SetString(2, EncounterID);
                query.SetString(3, HumanID);              
                arrypatientdetail = new ArrayList(query.List());
                iMySession.Close();
            }
            foreach (object[] obj in arrypatientdetail)
            {
                objpatientnotes = new PatientNotes();
                objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[0]);
                objpatientnotes.Source = Convert.ToString(obj[1]);
                objpatientnotes.Message_Description = Convert.ToString(obj[2]);
                objpatientnotes.Notes = Convert.ToString(obj[3]);
                objpatientnotes.Priority = Convert.ToString(obj[7]);
                objpatientnotes.Created_By = Convert.ToString(obj[4]);
                objpatientnotes.Modified_By = Convert.ToString(obj[5]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[6]);
                objpatientnotes.Id = Convert.ToUInt64(obj[10]);
                objpatientnotes.Type = Convert.ToString(obj[11]);
                objpatientnotes.Assigned_To = Convert.ToString(obj[12]);
                objpatientnotes.Caller_Name = Convert.ToString(obj[13]);
                objpatientnotes.Relationship = Convert.ToString(obj[14]);
                objpatientnotes.Message_Date_And_Time = Convert.ToDateTime(obj[15]);
                objpatientnotes.Encounter_ID = Convert.ToInt32(obj[16]);
                objpatientnotes.Line_ID = Convert.ToUInt64(obj[17]);
                objpatientnotes.Line_Type = Convert.ToString(obj[18]);
                objpatientnotes.Header_ID = Convert.ToUInt64(obj[19]);
                objpatientnotes.Header_Type = Convert.ToString(obj[20]);
                assign.Add(objpatientnotes);

            }

            return assign;


        }        
        public IList<PatientNotes> GetFilteredLineItemMessages(string MessageDescription, string MessageNotes, string HumanID, string ChargeLineID, string ChargeHeaderID)
        {
            //ArrayList arrList = null;
            IList<PatientNotes> assign = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();

            ArrayList arrypatientdetail = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.FilteredLineItemMessages");
                query.SetString(0, MessageDescription + "%");
                query.SetString(1, "%" + MessageNotes + "%");
                query.SetString(2, HumanID);
                query.SetString(3, ChargeLineID);
                query.SetString(4, ChargeHeaderID);
                arrypatientdetail = new ArrayList(query.List());
                iMySession.Close();
            }
            foreach (object[] obj in arrypatientdetail)
            {
                objpatientnotes = new PatientNotes();
                objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[0]);
                objpatientnotes.Source = Convert.ToString(obj[1]);
                objpatientnotes.Message_Description = Convert.ToString(obj[2]);
                objpatientnotes.Notes = Convert.ToString(obj[3]);
                objpatientnotes.Priority = Convert.ToString(obj[7]);
                objpatientnotes.Created_By = Convert.ToString(obj[4]);
                objpatientnotes.Modified_By = Convert.ToString(obj[5]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[6]);
                objpatientnotes.Id = Convert.ToUInt64(obj[10]);
                objpatientnotes.Type = Convert.ToString(obj[11]);
                objpatientnotes.Assigned_To = Convert.ToString(obj[12]);
                objpatientnotes.Caller_Name = Convert.ToString(obj[13]);
                objpatientnotes.Relationship = Convert.ToString(obj[14]);
                objpatientnotes.Message_Date_And_Time = Convert.ToDateTime(obj[15]);
                objpatientnotes.Encounter_ID = Convert.ToInt32(obj[16]);
                objpatientnotes.Line_ID = Convert.ToUInt64(obj[17]);
                objpatientnotes.Line_Type = Convert.ToString(obj[18]);
                objpatientnotes.Header_ID = Convert.ToUInt64(obj[19]);
                objpatientnotes.Header_Type = Convert.ToString(obj[20]);
                assign.Add(objpatientnotes);

            }

            return assign;


        }
        public IList<PatientNotes> GetFilteredMessageDetails(string MessageDescription, string MessageNotes, string HumanID)
        {
            //ArrayList arrList = null;
              IList<PatientNotes> assign = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();

            ArrayList arrypatientdetail = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetFilteredMessageDetails");
                query.SetString(0, MessageDescription + "%");
                query.SetString(1, "%" + MessageNotes + "%");
                query.SetString(2, HumanID);

                arrypatientdetail = new ArrayList(query.List());
                iMySession.Close();
            }
            foreach (object[] obj in arrypatientdetail)
            {
                objpatientnotes = new PatientNotes();
                objpatientnotes.Created_Date_And_Time = Convert.ToDateTime(obj[0]);
                objpatientnotes.Source = Convert.ToString(obj[1]);
                objpatientnotes.Message_Description = Convert.ToString(obj[2]);
                objpatientnotes.Notes = Convert.ToString(obj[3]);
                objpatientnotes.Priority = Convert.ToString(obj[7]);
                objpatientnotes.Created_By = Convert.ToString(obj[4]);
                objpatientnotes.Modified_By = Convert.ToString(obj[5]);
                objpatientnotes.Modified_Date_And_Time = Convert.ToDateTime(obj[6]);
                objpatientnotes.Id = Convert.ToUInt64(obj[10]);
                objpatientnotes.Type = Convert.ToString(obj[11]);
                objpatientnotes.Assigned_To = Convert.ToString(obj[12]);
                objpatientnotes.Caller_Name = Convert.ToString(obj[13]);
                objpatientnotes.Relationship = Convert.ToString(obj[14]);
                objpatientnotes.Message_Date_And_Time = Convert.ToDateTime(obj[15]);
                objpatientnotes.Encounter_ID = Convert.ToInt32(obj[16]);
                objpatientnotes.Line_ID = Convert.ToUInt64(obj[17]);
                objpatientnotes.Line_Type = Convert.ToString(obj[18]);
                objpatientnotes.Header_ID = Convert.ToUInt64(obj[19]);
                objpatientnotes.Header_Type = Convert.ToString(obj[20]);
                assign.Add(objpatientnotes);

            }

            return assign;


        }                        
        public ulong SavePatientAndreturnid(IList<PatientNotes> SaveList, string MacAddress)
        {
            ulong NoteID = 0;
            if (SaveList != null && SaveList.Count > 0)
            {
                IList<PatientNotes> PatNotesTemp = null;
                SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveList, ref PatNotesTemp, null, MacAddress, false, false, 0, "");
                NoteID = SaveList[0].Id;
            }
            return NoteID;
        }        

        public int SavePatientWithoutTransaction(IList<PatientNotes> ListToInsert, ISession MySession, string MACAddress)
        {
            int iResult = 0;
            GenerateXml ObjXML = null;
            if ((ListToInsert != null))
            {
                IList<PatientNotes> PatNotesTemp = null;
                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ListToInsert, ref PatNotesTemp, null, MySession, MACAddress, false, false, 0, "", ref ObjXML);
            }
            return iResult;
        }

        //added by balaji
        public PatientNotes GetByPatientObjectID(ulong PatientId)
        {
            PatientNotes ResultNotesid = new PatientNotes();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria =iMySession.CreateCriteria(typeof(PatientNotes)).Add(Expression.Eq("Id", PatientId));
                if (criteria.List<PatientNotes>().Count > 0)
                {
                    ResultNotesid = criteria.List<PatientNotes>()[0];
                }
                iMySession.Close();
            }
            return ResultNotesid;
        }
        //Is_Patient_Message
        public void updatePatientMessageAlone(PatientNotes objPatientNotes)
        {
            ISession MySession = Session.GetISession();
            IList<PatientNotes> ilist=new List<PatientNotes>();
            ilist.Add(objPatientNotes);
            IList<PatientNotes> ilistPatient=null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ilistPatient, ref ilist, null, "", false, false, 0, "");
        }

        public PatientDetailDto GetPopupDetailsNew(string humanid)
        {         
            PatientDetailDto PatientDetailsto = new PatientDetailDto();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("select {p.*},{w.*},{u.*},{ph.*},{c.*},{pl.*} from patient_notes p LEFT JOIN wf_object w ON ( p.message_id=w.obj_system_id and w.obj_type='TASK' ) left join user u on (u.user_name = p.Assigned_To and u.user_name<>'') left join physician_library ph on (u.physician_library_id=ph.physician_library_id) left join user c on (c.user_name = p.created_by) left join physician_library pl on (c.physician_library_id=pl.physician_library_id) where p.human_id='" + humanid.ToString() + "' order by p.created_date_and_time desc").AddEntity("p", typeof(PatientNotes)).AddEntity("w", typeof(WFObject)).AddEntity("u", typeof(User)).AddEntity("ph", typeof(PhysicianLibrary)).AddEntity("c", typeof(User)).AddEntity("pl", typeof(PhysicianLibrary));
                PatientNotes PatientNotesRecord = new PatientNotes();
                WFObject WFObjRecord = new WFObject();
                User UserAssignedTo = new User();
                User UserCreatedBy = new User();
                PhysicianLibrary PhyRecord = new PhysicianLibrary();
                PhysicianLibrary PhyCreatedBy = new PhysicianLibrary();
                foreach (IList<Object> l in sql.List())
                {
                    PatientNotesRecord = new PatientNotes();
                    WFObjRecord = new WFObject();
                    UserAssignedTo = new User();
                    UserCreatedBy = new User();
                    PhyRecord = new PhysicianLibrary();
                    PhyCreatedBy = new PhysicianLibrary();

                    PatientNotesRecord = (PatientNotes)l[0];
                    WFObjRecord = (WFObject)l[1];
                    if (WFObjRecord == null)
                        WFObjRecord = new WFObject();
                    UserAssignedTo = (User)l[2];
                    PhyRecord = (PhysicianLibrary)l[3];
                    UserCreatedBy = (User)l[4];
                    PhyCreatedBy = (PhysicianLibrary)l[5];
                    if (UserAssignedTo == null)
                        UserAssignedTo = new User();
                    if (PhyRecord == null)
                        PhyRecord = new PhysicianLibrary();
                    if (UserCreatedBy == null)
                        UserCreatedBy = new User();
                    if (PhyCreatedBy == null)
                        PhyCreatedBy = new PhysicianLibrary();

                    //Gitlab# 2485 - Physician Name Display Change
                    if (UserAssignedTo.Physician_Library_ID==0)
                    {
                        PatientNotesRecord.Assigned_To = UserAssignedTo.person_name;
                    }
                    else
                    {
                        PatientNotesRecord.Assigned_To = String.Empty;
                        if (PhyRecord.PhyLastName != String.Empty)
                            PatientNotesRecord.Assigned_To += PhyRecord.PhyLastName;
                        if (PhyRecord.PhyFirstName != String.Empty)
                        {
                            if (PatientNotesRecord.Assigned_To != String.Empty)
                                PatientNotesRecord.Assigned_To += "," + PhyRecord.PhyFirstName;
                            else
                                PatientNotesRecord.Assigned_To += PhyRecord.PhyFirstName;
                        }
                        if (PhyRecord.PhyMiddleName != String.Empty)
                            PatientNotesRecord.Assigned_To += " " + PhyRecord.PhyMiddleName;
                        if (PhyRecord.PhySuffix != String.Empty)
                            PatientNotesRecord.Assigned_To += "," + PhyRecord.PhySuffix;
                    }

                    if (UserCreatedBy.Physician_Library_ID == 0)
                    {
                        PatientNotesRecord.Created_By = UserCreatedBy.person_name;
                    }
                    else
                    {
                        PatientNotesRecord.Created_By = String.Empty;
                        if (PhyCreatedBy.PhyLastName != String.Empty)
                            PatientNotesRecord.Created_By += PhyCreatedBy.PhyLastName;
                        if (PhyCreatedBy.PhyFirstName != String.Empty)
                        {
                            if (PatientNotesRecord.Created_By != String.Empty)
                                PatientNotesRecord.Created_By += "," + PhyCreatedBy.PhyFirstName;
                            else
                                PatientNotesRecord.Created_By += PhyCreatedBy.PhyFirstName;
                        }
                        if (PhyCreatedBy.PhyMiddleName != String.Empty)
                            PatientNotesRecord.Created_By += " " + PhyCreatedBy.PhyMiddleName;
                        if (PhyCreatedBy.PhySuffix != String.Empty)
                            PatientNotesRecord.Created_By += "," + PhyCreatedBy.PhySuffix;
                    }

                    PatientDetailsto.ilstPatientNotes.Add(PatientNotesRecord);
                    PatientDetailsto.ilstWFObj.Add(WFObjRecord);
                    PatientDetailsto.ilstUser.Add(UserAssignedTo);
                    PatientDetailsto.ilstUser.Add(UserCreatedBy);
                    
                }
                iMySession.Close();
            }
            return PatientDetailsto;
        }
        #endregion
    }
}