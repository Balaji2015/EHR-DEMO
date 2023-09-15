using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
using System.Web;
using System.IO;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IWFObjectManager : IManagerBase<WFObject, ulong>
    {
        int InsertToWorkFlowObject(WFObject WorkFlow, int iCloseType, string sMacAddress, ISession mySession);
        int InsertToRCopiaWorkFlowObject(WFObject WorkFlow, int iCloseType, string sMacAddress);
        int MoveToNextProcess(ulong uObjSystemId, string ObjType, int iCloseType, string sOwner, DateTime StartTime, string sMacAddress, string[] sCurrentProcess, ISession MySessionHere);
        int MoveToPreviousProcess(ulong ulObjSystemID, string ObjType, int iCloseType, string sOwner, DateTime StartTime, string sMacAddress);
        int MoveToPreviousProcessForAdmin(ulong ulObjSystemID, string ObjType, string sOwner, string PreviousProcess, DateTime StartTime, string sMacAddress);
        void UpdateOwner(ulong ulObjSystemID, string ObjType, string sOwner, string sMacAddress);
        //Stream GetListofObjects(string FacName, string[] ObjType, string[] ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays);

        WFObject GetByObjectSystemId(ulong ObjectSystemId, string ObjectType);
        //Added by Chithra on 10-Mar-2016 to include Archived Tables.
        WFObject GetByObjectSystemIdIncludeArchive(ulong ObjectSystemId, string ObjectType);
        IList<WFObject> GetByObjectSystemIdForEncounterandDocumentation(ulong ObjectSystemId);

        IList<WFObject> GetByParentObjectSystemId(ulong ParentObjectSystemId, string ParentObjectType);

        IList<MyQ> GetListofObjectsByProcess(string FacName, string CurrentProcess);

        //IList<BillingMyQDTO> LoadWfObjectforBilling(string Username);

        IList<WFObject> GetListofObjectsByCurrentProcess(string CurrentProcess);

        IList<WFObject> GetListByObjectTypeSubTypeAndCurrentProcess(string ObjectType, string ObjectSubType, string CurrentProcess);

        IList<FillLabAgentDTO> GetWfObjectBasedOnLab(List<string> CurrentProcess, string Obj_Type);

        DataSet GetListOfObjectsByFaciltyObjTypeCurrentProcess(string FacilityName, string objType, string currentProcess, int pageNumber, int maxResults, int Limit, string EncouterAndArcCount);
        Stream LoadMyQ(string GenQFacilityName, string[] GenQObjType, string[] GenQProcessType, string GenQUserName, string MyQFacilityName, string[] MyQObjType, string[] MyQProcessType, string MyQUserName, Boolean bShowAllGeneral, Boolean bShowAllMyQ, int DefaultNoofDays);
        void DeleteDocumentationObject(WFObject DeleteWFObject);

        //
        int MultipleObjectsMoveToNextProcess(ulong uObjSystemIdForFO, string ObjTypeForFO, int iCloseTypeForFO, IList<ulong> uOrders_Submit_Id, string ObjType, int iCloseType, string sOwner, DateTime StartTime, string sMacAddress, string[] sCurrentProcess, ISession MySessionHere);

        int MultipleObjectsMoveToNextProcessForDictation(ulong exception_ID, string ObjTypeForDictation, int Close_TypeForDictation, string sOwner, DateTime StartTime, string[] sCurrentProcess, ulong Encounter_ID, string objTypeForDocumentation, int Close_TypeForDocumentation, string MACAddress);

        int DictationMoveToNextProcess(ulong Exception_Encounter_Id, string obj_type, int close_Type, DateTime date, string MACAddress);
        void MoveToNextProcessForTwoObject(ulong uObjSystemId, string[] ObjType, string sOwner, int iClose_TypeForFirstObject, int iClose_TypeForSecondObject, DateTime StartTime, string[] sCurrentProcess, string MACAddress);
        void updateOwnerForDictationException(string FileName, string sObjType, string sOwner, string MACAddress);
        IList<WFObject> GetListofObjectsByObySystemIdforScan(ulong uObjSystemId);
        //Added by Bala 0n 05-Dec-2013 For Attach Super Bill
        IList<WFObject> GetDocumentationObject(ulong[] uObjSystemId);
        int GetLabResultsWaiting(string FacName, string[] ObjType, string[] ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays);
        void saveListwfobj(IList<WFObject> ilist);
        IList<WFObject> LoadUpdatewfobj(IList<WFObject> ilist, string operationMode);
        IList<Encounter> GetEncounterDetailsByIsProgressNoteGenerate();

        bool IsPreviousEncounterPhysicianProcess(ulong ulEncounterID);
        bool IsPreviousEncounterPhysicianProcess(ulong ulEncounterID, bool isFromArchive);
        IList<WFObject> GetWFObjectsByObjSystemIdForPFSH(ulong objSystemID);
        IList<WFObject> GetByDocumentObjectSystemId(ulong ObjectSystemId, string ObjectType);

        IList<WFObject> GetImmunizationOrder();
        void UpdateAbnormalFlag(ulong ulObjSystemID, string ObjType, string sAbnormal);
        WFObject GetByObjectSystemIdAnfObjType(ulong ObjectSystemId, string ObjectType);
        void GetWfObjDetails(string UserName, ulong encounter_id, out string Obj_Type, out string Curr_Process, out bool Owner_Enc_Mismatch);//Added for CarePointe

        WFObject GetWfObjArchiveByObjectSystemId(ulong ObjectSystemId, string ObjectType);
    }
    public partial class WFObjectManager : ManagerBase<WFObject, ulong>, IWFObjectManager
    {

        #region Constructors

        public WFObjectManager()
            : base()
        {

        }
        public WFObjectManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion



        #region Methods
        public IList<WFObject> GetDetailbyObjectsystemId(ulong sysid)
        {
            IList<WFObject> lstscn = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", (sysid))).Add(Expression.Eq("Obj_Type", "SCAN"));
                lstscn = criteria.List<WFObject>();
                iMySession.Close();
            }
            return lstscn;

        }
        public IList<FillLabAgentDTO> GetWfObjectBasedOnLab(List<string> CurrentProcess, string Obj_Type)
        {
            //string Current_Process = string.Empty;
            //foreach (string str in CurrentProcess)
            //{
            //    Current_Process += ",\'" + str + "\'";

            //}
            //string Lab_Names = string.Empty;
            //foreach (string str in LabName)
            //{
            //    Lab_Names += ",\'" + str + "\'";
            //}
            //string parameter1 = Current_Process.Substring(1);
            //string parameter2 = Lab_Names.Substring(1);
            IList<FillLabAgentDTO> ilstFillLabAgentDTO = new List<FillLabAgentDTO>();
            IQuery query2 = session.GetISession().GetNamedQuery("GetWFObjectsForLab");
            //query2.SetString(0, parameter2);
            query2.SetString(0, Obj_Type);
            query2.SetParameterList("CurrentProcess", CurrentProcess.ToArray<string>());
            // query2.SetParameterList("LabNames", LabName.ToArray<string>());
            //query2.SetString(2, parameter2);
            //query2.SetParameter("CurrentProcessList", CurrentProcess.ToArray<string>());
            //query2.SetParameter("LabList", LabName.ToArray<string>());
            ArrayList objects = new ArrayList(query2.List());
            FillLabAgentDTO FillLabAgentDTO = new FillLabAgentDTO();
            foreach (object[] obj in objects)
            {
                FillLabAgentDTO = new FillLabAgentDTO();
                FillLabAgentDTO.WF_Obj_System_ID = Convert.ToUInt64(obj[1]);
                FillLabAgentDTO.WfObj_ID = Convert.ToUInt64(obj[0]);
                FillLabAgentDTO.Current_Process = (obj[2]).ToString();
                if (obj[3] != null)
                    FillLabAgentDTO.LabName = (obj[3]).ToString();
                FillLabAgentDTO.Obj_Type = (obj[4]).ToString();
                FillLabAgentDTO.Is_Submit_Imediately = (obj[5]).ToString();
                FillLabAgentDTO.Current_Arrival_Time = Convert.ToDateTime((obj[6]));
                FillLabAgentDTO.Order_Code_Type = (obj[7]).ToString();
                ilstFillLabAgentDTO.Add(FillLabAgentDTO);
            }
            return ilstFillLabAgentDTO;
        }


        public int InsertToWorkFlowObject(WFObject WorkFlow, int iCloseType, string sMacAddress, ISession MySession)
        {
            IList<WFObject> WfList = new List<WFObject>();
            //Srividhya
            IList<WFObject> UpdateWfList = null;
            GenerateXml XMLObj = null;
            WorkFlow.Is_Default_MyQ_LineItem = 1;
            WfList.Add(WorkFlow);

            int iResult = 0;

            if (WfList.Count > 0)
            {
                // iResult = SaveUpdateDeleteWithoutTransaction(ref WfList, null, null, MySession, sMacAddress);
                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref WfList, ref UpdateWfList, null, MySession, sMacAddress, false, false, 0, string.Empty, ref XMLObj);
                if (iResult == 2 || iResult == 1)
                {
                    return iResult;
                }
                iResult = MoveToNextProcess(WorkFlow.Obj_System_Id, WorkFlow.Obj_Type, iCloseType, WorkFlow.Current_Owner, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), sMacAddress, null, MySession);
            }
            return iResult;
        }

        public int InsertToRCopiaWorkFlowObject(WFObject WorkFlow, int iCloseType, string sMacAddress)
        {
            IList<WFObject> WfList = new List<WFObject>();
            IList<WFObject> UpdateWfList = null;
            //Srividhya
            WorkFlow.Is_Default_MyQ_LineItem = 1;
            WfList.Add(WorkFlow);

            int iResult = 0;

            if (WfList.Count > 0)
            {
                //SaveUpdateDeleteWithTransaction(ref WfList, null, null, sMacAddress);
                SaveUpdateDelete_DBAndXML_WithTransaction(ref WfList, ref UpdateWfList, null, sMacAddress, false, false, 0, string.Empty);
                iResult = MoveToNextProcess(WorkFlow.Obj_System_Id, WorkFlow.Obj_Type, iCloseType, WorkFlow.Current_Owner, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), sMacAddress, null, null);
            }
            return iResult;
        }


        //public WFObject GetByObjectSystemId(ulong ObjectSystemId, string ObjectType)
        //{

        //    WFObject ResultWFObj = new WFObject();

        //    //srividhya
        //    //ICriteria criteria = session.GetISession().CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", ObjectSystemId)).Add(Expression.Eq("Obj_Type", ObjectType));

        //    //string sd = session.GetISession().CreateSQLQuery("SELECT W.Doc_Type FROM WF_Object W WHERE  W.Obj_System_Id='" + ObjectSystemId + "' AND W.Obj_Type='" + ObjectType + "' ").AddEntity("W", typeof(WFObject)).List<string>()[0]; 

        //    //if (criteria.List<WFObject>().Count > 0)
        //    //{
        //    //    ResultWFObj = criteria.List<WFObject>()[0];
        //    //}

        //    //foreach (IList<Object> l in sql.List())
        //    //{
        //    //    ResultWFObj = (WFObject)l[0];
        //    //}

        //    //if (sql.List<WFObject>().Count > 0)
        //    //{
        //    //    ResultWFObj = sql.List<WFObject>()[0];
        //    //}

        //    //string s = sql.List<WFObject>()[0].Doc_Type;
        //    //ResultWFObj = sql.List<WFObject>()[0];
        //    ArrayList MyList = new ArrayList();
        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = iMySession.GetNamedQuery("Get.Enc.Details");
        //        query1.SetString(0, ObjectSystemId.ToString());
        //        query1.SetString(1, ObjectType);

        //        MyList.AddRange(query1.List());

        //        if (MyList != null)
        //        {
        //            for (int i = 0; i < MyList.Count; i++)
        //            {
        //                object[] oj = (object[])MyList[i];

        //                //string s = MyList[0].ToString();
        //                ResultWFObj.Id = Convert.ToUInt32(oj[0]);
        //                ResultWFObj.Obj_System_Id = Convert.ToUInt32(oj[1]);
        //                ResultWFObj.Obj_Type = oj[2].ToString();
        //                ResultWFObj.Obj_Sub_Type = oj[3].ToString();
        //                ResultWFObj.Fac_Name = oj[4].ToString();
        //                ResultWFObj.Current_Process = oj[5].ToString();
        //                ResultWFObj.Current_Arrival_Time = Convert.ToDateTime(oj[6]);
        //                ResultWFObj.Current_Owner = oj[7].ToString();
        //                ResultWFObj.Priority = oj[8].ToString();
        //                ResultWFObj.Parent_Obj_Type = oj[9].ToString();
        //                ResultWFObj.Parent_Obj_System_Id = Convert.ToUInt32(oj[10]);
        //                ResultWFObj.Process_Allocation = oj[11].ToString();
        //                ResultWFObj.Version = Convert.ToInt32(oj[12]);
        //                ResultWFObj.Doc_Type = oj[13].ToString();
        //                ResultWFObj.Doc_Sub_Type = oj[14].ToString();
        //                //Srividhya
        //                ResultWFObj.Is_Default_MyQ_LineItem = Convert.ToInt32(oj[15]);
        //                //naveena
        //                ResultWFObj.Is_Abnormal = oj[16].ToString(); ;
        //            }

        //        }
        //        //log.Info("Inside GetByObjectSystemId manager call end");
        //        iMySession.Close();
        //    }
        //    return ResultWFObj;
        //}


        public WFObject GetByObjectSystemId(ulong ObjectSystemId, string ObjectType)
        {

            WFObject ResultWFObj = new WFObject();
            IList<WFObject> WFobjList = new List<WFObject>();

            ICriteria criteria = session.GetISession().CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", ObjectSystemId)).Add(Expression.Eq("Obj_Type", ObjectType));
            WFobjList = criteria.List<WFObject>();
            if (WFobjList.Count > 0)
            {
                ResultWFObj = WFobjList[0];
            }
            return ResultWFObj;
        }

        public IList<WFObject> GetByObjectSystemIdForEncounterandDocumentation(ulong ObjectSystemId)
        {
            ISQLQuery criteria = session.GetISession().CreateSQLQuery("Select w.* from wf_object w where w.obj_system_id=" + ObjectSystemId + " and w.Obj_Type in ('ENCOUNTER','DOCUMENTATION')").AddEntity("w.*", typeof(WFObject));
            IList<WFObject> WFobjList = criteria.List<WFObject>();

            return WFobjList;
        }

        public WFObject GetByObjectSystemIdIncludeArchive(ulong ObjectSystemId, string ObjectType)
        {
            WFObject ResultWFObj = new WFObject();

            ArrayList MyList = new ArrayList();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Get.Enc.Details.With.Archive.Tables");
                query1.SetString(0, ObjectSystemId.ToString());
                query1.SetString(1, ObjectType);
                query1.SetString(2, ObjectSystemId.ToString());
                query1.SetString(3, ObjectType);

                MyList.AddRange(query1.List());

                if (MyList != null)
                {
                    for (int i = 0; i < MyList.Count; i++)
                    {
                        object[] oj = (object[])MyList[i];
                        ResultWFObj.Id = Convert.ToUInt32(oj[0]);
                        ResultWFObj.Obj_System_Id = Convert.ToUInt32(oj[1]);
                        ResultWFObj.Obj_Type = oj[2].ToString();
                        ResultWFObj.Obj_Sub_Type = oj[3].ToString();
                        ResultWFObj.Fac_Name = oj[4].ToString();
                        ResultWFObj.Current_Process = oj[5].ToString();
                        ResultWFObj.Current_Arrival_Time = Convert.ToDateTime(oj[6]);
                        ResultWFObj.Current_Owner = oj[7].ToString();
                        ResultWFObj.Priority = oj[8].ToString();
                        ResultWFObj.Parent_Obj_Type = oj[9].ToString();
                        ResultWFObj.Parent_Obj_System_Id = Convert.ToUInt32(oj[10]);
                        ResultWFObj.Process_Allocation = oj[11].ToString();
                        ResultWFObj.Version = Convert.ToInt32(oj[12]);
                        ResultWFObj.Doc_Type = oj[13].ToString();
                        ResultWFObj.Doc_Sub_Type = oj[14].ToString();
                        ResultWFObj.Is_Default_MyQ_LineItem = Convert.ToInt32(oj[15]);
                    }

                }
                iMySession.Close();
            }
            return ResultWFObj;
        }

        public IList<WFObject> GetListByObjectTypeSubTypeAndCurrentProcess(string ObjectType, string ObjectSubType, string CurrentProcess)
        {
            IList<WFObject> listWFobject = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", ObjectType)).Add(Expression.Eq("Obj_Sub_Type", ObjectSubType)).Add(Expression.Eq("Current_Process", CurrentProcess));
                listWFobject = crit.List<WFObject>();
                iMySession.Close();
            }
            return listWFobject;
        }

        public IList<WFObject> GetByParentObjectSystemId(ulong ParentObjectSystemId, string ParentObjectType)
        {

            WFObject ResultWFObj = new WFObject();
            IList<WFObject> wflist = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Parent_Obj_System_Id", ParentObjectSystemId))
                    .Add(Expression.Eq("Parent_Obj_Type", ParentObjectType));
                wflist = criteria.List<WFObject>();
                iMySession.Close();
            }
            return wflist;
        }
        int iTryCount = 0;

        //public int MoveToNextProcess(ulong ulObjSystemID, string ObjType, int iCloseType, string sOwner, DateTime StartTime, string sMacAddress, ISession MySessionHere)
        //{
        //    //Modified By Selvaraman on 24-Aug-2010
        //    WorkFlowManager WFManager = new WorkFlowManager();
        //    ObjectProcessHistoryManager ObjProcHistoryMngr = new ObjectProcessHistoryManager();
        //    ObjectProcessHistory ObjProcHistoryRecord = new ObjectProcessHistory();

        //    //To load the Current WF Object
        //    WFObject WFobj = GetByObjectSystemId(ulObjSystemID, ObjType);

        //    //To load the Original Version of the WF Object
        //    WFObject OriginalWFObj = new WFObject();
        //    OriginalWFObj.Current_Arrival_Time = WFobj.Current_Arrival_Time;
        //    OriginalWFObj.Current_Owner = WFobj.Current_Owner;
        //    OriginalWFObj.Current_Process = WFobj.Current_Process;
        //    OriginalWFObj.Fac_Name = WFobj.Fac_Name;
        //    OriginalWFObj.Id = WFobj.Id;
        //    OriginalWFObj.Obj_Sub_Type = WFobj.Obj_Sub_Type;
        //    OriginalWFObj.Obj_System_Id = WFobj.Obj_System_Id;
        //    OriginalWFObj.Obj_Type = WFobj.Obj_Type;
        //    OriginalWFObj.Parent_Obj_System_Id = WFobj.Parent_Obj_System_Id;
        //    OriginalWFObj.Parent_Obj_Type = WFobj.Parent_Obj_Type;
        //    OriginalWFObj.Priority = WFobj.Priority;
        //    OriginalWFObj.Version = WFobj.Version;

        //    IList<WFObject> OriginalWFList = new List<WFObject>();
        //    OriginalWFList.Add(OriginalWFObj);

        //    string ToProcess;

        //    //Change By Selvaraman - After Begin Transaction, Get Query Should not be there.
        //    //To find the Next process
        //    //ToProcess = WFManager.GetNextProcess(WFobj.Fac_Name, WFobj.Obj_Type, WFobj.Obj_Sub_Type, WFobj.Current_Process, iCloseType);

        //    IList<WorkFlow> TempWorkflow = new List<WorkFlow>();
        //    var work = from w in NHibernateSessionUtility.Instance.WFTable where w.Fac_Name==WFobj.Fac_Name && 
        //               w.Obj_Type==WFobj.Obj_Type && w.Obj_Sub_Type==WFobj.Obj_Sub_Type && w.From_Process ==WFobj.Current_Process 
        //               && w.Close_Type ==iCloseType select w;
        //    ToProcess = work.ToList<WorkFlow>()[0].To_Process;

        //    if (ToProcess == string.Empty)
        //    {
        //        throw new Exception("To Process is not found from the Workflow Table");
        //        return 1;
        //    }

        //    IList<ObjectMaster> ObjMasterList = new List<ObjectMaster>();
        //    ObjectMasterManager ObjMasterMngr = new ObjectMasterManager();

        //    ProcessMasterManager ProcMngr = new ProcessMasterManager();
        //    IList<ProcessMaster> Proclist = new List<ProcessMaster>();

        //    //To Create the Session and the Transaction
        //    ISession MySession = null;
        //    ITransaction trans = null;

        //    try
        //    {
        //        iTryCount = 0;

        //    TryAgain:
        //        int iResult = 0;

        //        if (MySessionHere == null)
        //        {
        //            MySession = session.GetISession();
        //            trans = MySession.BeginTransaction();
        //        }
        //        else
        //        {
        //            MySession = MySessionHere;
        //        }

        //        //Note: Currently, In EHR - Wellness, ParentChild Relationship is not maintained.
        //        #region ParentChildPush
        //        ////To Find whether the Current Process is the Wait Process
        //        //Proclist = ProcMngr.GetAllProcessList();
        //        //var Proc = from p in Proclist where p.Process_Name == WFobj.Current_Process select p;
        //        //Proclist = Proc.ToList<ProcessMaster>();

        //        //if (Proclist[0].Wait_Process.ToUpper() == "Y")
        //        //{
        //        //    Boolean bMoveOtherObj = false;

        //        //    IList<WFObject> AllAddObjects = null;
        //        //    IList<WFObject> AllObjects = new List<WFObject>();

        //        //    IList<WFObject> ForObjProcHistory = new List<WFObject>();
        //        //    WFObject objForObjProcHistory;

        //        //    //If Current Object is Parent Object
        //        //    if (WFobj.Parent_Obj_System_Id == 0)
        //        //    {
        //        //        WFobj.Current_Process = ToProcess;
        //        //        WFobj.Current_Arrival_Time = DateTime.Now.ToUniversalTime();
        //        //        WFobj.Current_Owner = sOwner;
        //        //        AllObjects.Add(WFobj);

        //        //        //Check all the Childs
        //        //        IList<WFObject> ChildObjList = null;
        //        //        ChildObjList = GetByParentObjectSystemId(WFobj.Obj_System_Id, WFobj.Obj_Type);

        //        //        for (int i = 0; i < ChildObjList.Count; i++)
        //        //        {
        //        //            if (ChildObjList[i].Current_Process == WFobj.Current_Process)
        //        //            {
        //        //                //To fill object process history table - Start
        //        //                objForObjProcHistory = new WFObject();
        //        //                objForObjProcHistory.Current_Arrival_Time = ChildObjList[i].Current_Arrival_Time;
        //        //                objForObjProcHistory.Current_Owner = ChildObjList[i].Current_Owner;
        //        //                objForObjProcHistory.Current_Process = ChildObjList[i].Current_Process;
        //        //                objForObjProcHistory.Id = ChildObjList[i].Id;
        //        //                objForObjProcHistory.Obj_Sub_Type = ChildObjList[i].Obj_Sub_Type;
        //        //                objForObjProcHistory.Obj_System_Id = ChildObjList[i].Obj_System_Id;
        //        //                objForObjProcHistory.Obj_Type = ChildObjList[i].Obj_Type;
        //        //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
        //        //                objForObjProcHistory.Parent_Obj_System_Id = ChildObjList[i].Parent_Obj_System_Id;
        //        //                objForObjProcHistory.Parent_Obj_Type = ChildObjList[i].Parent_Obj_Type;
        //        //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
        //        //                objForObjProcHistory.Version = ChildObjList[i].Version;
        //        //                ForObjProcHistory.Add(objForObjProcHistory);
        //        //                //To fill object process history table - End

        //        //                ChildObjList[i].Current_Process = ToProcess;
        //        //                ChildObjList[i].Current_Arrival_Time = DateTime.Now.ToUniversalTime();
        //        //                ChildObjList[i].Current_Owner = sOwner;
        //        //                AllObjects.Add(ChildObjList[i]);
        //        //                bMoveOtherObj = true;
        //        //            }
        //        //            else
        //        //            {
        //        //                bMoveOtherObj = false;
        //        //                break;
        //        //            }
        //        //        }


        //        //        //If Everythng is in WaitProcess - Move to the Next Process
        //        //        if (bMoveOtherObj == true)
        //        //        {
        //        //            //Changed by Selvaraman - 21-Feb-11
        //        //            SaveUpdateDeleteWithoutTransaction(ref AllAddObjects, AllObjects, null, MySession, sMacAddress);
        //        //            //To Append to the ObjectPorcessHistory Table
        //        //            ObjProcHistoryMngr.AppendToObjectProcessHistory(ForObjProcHistory, MySession, StartTime, sMacAddress);
        //        //            if (MySessionHere == null)
        //        //            {
        //        //                MySession.Flush();
        //        //                trans.Commit();
        //        //                MySession.Close();
        //        //            }
        //        //            return 0;
        //        //        }
        //        //    }
        //        //    //Else if Current Object is Child Object
        //        //    else
        //        //    {
        //        //        //Check the Parent and all the Childs 
        //        //        WFObject ParentObj = null;
        //        //        IList<WFObject> ChildObjList = null;

        //        //        //Check the Parent
        //        //        ParentObj = GetByObjectSystemId(WFobj.Parent_Obj_System_Id, WFobj.Parent_Obj_Type);
        //        //        if (ParentObj.Current_Process == WFobj.Current_Process)
        //        //        {
        //        //            //To fill object process history table - Start
        //        //            objForObjProcHistory = new WFObject();
        //        //            objForObjProcHistory.Current_Arrival_Time = ParentObj.Current_Arrival_Time;
        //        //            objForObjProcHistory.Current_Owner = ParentObj.Current_Owner;
        //        //            objForObjProcHistory.Current_Process = ParentObj.Current_Process;
        //        //            objForObjProcHistory.Id = ParentObj.Id;
        //        //            objForObjProcHistory.Obj_Sub_Type = ParentObj.Obj_Sub_Type;
        //        //            objForObjProcHistory.Obj_System_Id = ParentObj.Obj_System_Id;
        //        //            objForObjProcHistory.Obj_Type = ParentObj.Obj_Type;
        //        //            objForObjProcHistory.Priority = ParentObj.Priority;
        //        //            objForObjProcHistory.Parent_Obj_System_Id = ParentObj.Parent_Obj_System_Id;
        //        //            objForObjProcHistory.Parent_Obj_Type = ParentObj.Parent_Obj_Type;
        //        //            objForObjProcHistory.Priority = ParentObj.Priority;
        //        //            objForObjProcHistory.Version = ParentObj.Version;
        //        //            ForObjProcHistory.Add(objForObjProcHistory);
        //        //            //To fill object process history table - End

        //        //            ParentObj.Current_Process = ToProcess;
        //        //            ParentObj.Current_Arrival_Time = DateTime.Now.ToUniversalTime();
        //        //            ParentObj.Current_Owner = sOwner;
        //        //            AllObjects.Add(ParentObj);
        //        //            bMoveOtherObj = true;
        //        //        }
        //        //        else
        //        //        {
        //        //            bMoveOtherObj = false;
        //        //        }

        //        //        //Check all the Childs
        //        //        ChildObjList = GetByParentObjectSystemId(WFobj.Parent_Obj_System_Id, WFobj.Parent_Obj_Type);
        //        //        for (int i = 0; i < ChildObjList.Count; i++)
        //        //        {
        //        //            if (ChildObjList[i].Current_Process == WFobj.Current_Process)
        //        //            {
        //        //                //To fill object process history table - Start
        //        //                objForObjProcHistory = new WFObject();
        //        //                objForObjProcHistory.Current_Arrival_Time = ChildObjList[i].Current_Arrival_Time;
        //        //                objForObjProcHistory.Current_Owner = ChildObjList[i].Current_Owner;
        //        //                objForObjProcHistory.Current_Process = ChildObjList[i].Current_Process;
        //        //                objForObjProcHistory.Id = ChildObjList[i].Id;
        //        //                objForObjProcHistory.Obj_Sub_Type = ChildObjList[i].Obj_Sub_Type;
        //        //                objForObjProcHistory.Obj_System_Id = ChildObjList[i].Obj_System_Id;
        //        //                objForObjProcHistory.Obj_Type = ChildObjList[i].Obj_Type;
        //        //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
        //        //                objForObjProcHistory.Parent_Obj_System_Id = ChildObjList[i].Parent_Obj_System_Id;
        //        //                objForObjProcHistory.Parent_Obj_Type = ChildObjList[i].Parent_Obj_Type;
        //        //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
        //        //                objForObjProcHistory.Version = ChildObjList[i].Version;

        //        //                ForObjProcHistory.Add(objForObjProcHistory);

        //        //                //To fill object process history table - End

        //        //                ChildObjList[i].Current_Process = ToProcess;
        //        //                ChildObjList[i].Current_Arrival_Time = DateTime.Now.ToUniversalTime();
        //        //                ChildObjList[i].Current_Owner = sOwner;
        //        //                AllObjects.Add(ChildObjList[i]);
        //        //                bMoveOtherObj = true;
        //        //            }
        //        //            else
        //        //            {
        //        //                bMoveOtherObj = false;
        //        //                break;
        //        //            }
        //        //        }

        //        //        //If Everythng is in WaitProcess - Move to the Next Process
        //        //        if (bMoveOtherObj == true)
        //        //        {
        //        //            //Changed by Selvaraman - 21-Feb-11
        //        //            SaveUpdateDeleteWithoutTransaction(ref AllAddObjects, AllObjects, null, MySession, sMacAddress);
        //        //            //To Append to the ObjectPorcessHistory Table
        //        //            ObjProcHistoryMngr.AppendToObjectProcessHistory(ForObjProcHistory, MySession, StartTime, sMacAddress);
        //        //            if (MySessionHere == null)
        //        //            {
        //        //                MySession.Flush();
        //        //                trans.Commit();
        //        //                MySession.Close();
        //        //            }
        //        //            return 0;
        //        //        }
        //        //    }
        //        //}
        //        #endregion

        //        //To move the current object into next process
        //        WFobj.Current_Process = ToProcess;
        //        WFobj.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
        //        WFobj.Current_Owner = sOwner;

        //        IList<WFObject> wfObjList = new List<WFObject>();
        //        wfObjList.Add(WFobj);

        //        IList<WFObject> addList = null;


        //        iResult = SaveUpdateDeleteWithoutTransaction(ref addList, wfObjList, null, MySession, sMacAddress);
        //        //if bResult = false then, the deadlock is occured 
        //        if (iResult == 2)
        //        {
        //            if (iTryCount < 5)
        //            {
        //                iTryCount++;
        //                goto TryAgain;
        //            }
        //            else
        //            {
        //                if (MySessionHere == null)
        //                {
        //                    trans.Rollback();
        //                    MySession.Close();
        //                    throw new Exception("Deadlock is occured. Transaction failed");
        //                }
        //                else
        //                {
        //                    return iResult;
        //                }
        //            }
        //        }
        //        else if (iResult == 1)
        //        {
        //            if (MySessionHere == null)
        //            {
        //                trans.Rollback();
        //                MySession.Close();
        //                throw new Exception("Exception is occured. Transaction failed");
        //            }
        //            else
        //            {
        //                return iResult;
        //            }
        //        }
        //        //To Append to the ObjectPorcessHistory Table
        //        iResult = ObjProcHistoryMngr.AppendToObjectProcessHistory(OriginalWFList, MySession, StartTime, sMacAddress);
        //        //if bResult = false then, the deadlock is occured 
        //        if (iResult == 2)
        //        {
        //            if (iTryCount < 5)
        //            {
        //                iTryCount++;
        //                goto TryAgain;
        //            }
        //            else
        //            {
        //                if (MySessionHere == null)
        //                {
        //                    trans.Rollback();
        //                    MySession.Close();
        //                    throw new Exception("Deadlock is occured. Transaction failed");
        //                }
        //                else
        //                {
        //                    return iResult;
        //                }
        //            }
        //        }
        //        else if (iResult == 1)
        //        {
        //            if (MySessionHere == null)
        //            {
        //                trans.Rollback();
        //                MySession.Close();
        //                throw new Exception("Exception is occured. Transaction failed");
        //            }
        //            else
        //            {
        //                return iResult;
        //            }
        //        }

        //        if (MySessionHere == null)
        //        {
        //            MySession.Flush();
        //            trans.Commit();
        //        }
        //    }
        //    catch (NHibernate.Exceptions.GenericADOException)
        //    {
        //        if (MySessionHere == null)
        //        {
        //            trans.Rollback();
        //            MySession.Close();
        //            throw;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        if (MySessionHere == null)
        //        {
        //            trans.Rollback();
        //            MySession.Close();
        //            throw;
        //        }
        //    }
        //    finally
        //    {
        //        if (MySessionHere == null)
        //        {
        //            MySession.Close();
        //        }
        //    }
        //    return 0;
        //}

        //----------------------Adding two parameters(uobjsystemid and objtype)in MoveToNextProcess method done by Manimozhi in order to remove //
        //GetByObjectSystemId() call in U-------------------------//
        public int MoveToNextProcess(ulong uObjSystemId, string ObjType, int iCloseType, string sOwner, DateTime StartTime, string sMacAddress, string[] sCurrentProcess, ISession MySessionHere)
        {
            //To get the latest version
            //srividhya comm
            //ISQLQuery sql = session.GetISession().CreateSQLQuery("Select w.* from wf_object w left join process_master m on(w.current_process=m.process_name) where w.obj_system_id=" + uObjSystemId + " and w.Obj_Type='" + ObjType + "' and m.Is_Deleted_Process<>'Y'")
            //    .AddEntity("w.*", typeof(WFObject));
            //  IList<WFObject> wfObjectList=sql.List<WFObject>();
            WFObject WFobj = new WFObject();
            //  if (wfObjectList != null && wfObjectList.Count > 0)
            //  {
            //      WFobj = wfObjectList[0];//  GetByObjectSystemId(uObjSystemId, ObjType);
            //  }
            //srividhya comm

            ArrayList MyList = new ArrayList();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            // IQuery query1 = iMySession.GetNamedQuery("Get.WFObject.Encounter.Details");
            //IQuery query1 = session.GetISession().GetNamedQuery("Get.WFObject.Encounter.Details"); //For Bud ID: 75702 //Visual studio version upgrade session issue
            IQuery query1 = session.GetISession().GetNamedQuery("Get.WFObject.Encounter.Details");

            if (MySessionHere != null && MySessionHere.IsOpen == true) //If session created in insertintowfobject is still open then use it to avoid "No record found for ObjectSystemID:" error  
            {
                query1 = MySessionHere.GetNamedQuery("Get.WFObject.Encounter.Details");
            }
            
            query1.SetString(0, uObjSystemId.ToString());
            query1.SetString(1, ObjType);

            MyList.AddRange(query1.List());
            //BugID:54697
            if (MyList == null || (MyList != null && MyList.Count == 0) || query1.List().Count == 0)
            {
                string curr_process = string.Empty;
                if (sCurrentProcess != null && sCurrentProcess.Count() > 0)
                {
                    curr_process = "CurrentProcess: " + sCurrentProcess[0];
                }
                throw new Exception("Please report this to the support team and process the next file [No Record found for ObjectSystemID:" + uObjSystemId.ToString() + " and ObjType:" + ObjType + curr_process + "]");
                //return 1;
            }

            if (MyList != null)
            {
                for (int i = 0; i < MyList.Count; i++)
                {
                    object[] oj = (object[])MyList[i];

                    //string s = MyList[0].ToString();
                    WFobj.Id = Convert.ToUInt32(oj[0]);
                    WFobj.Obj_System_Id = Convert.ToUInt32(oj[1]);
                    WFobj.Obj_Type = oj[2].ToString();
                    WFobj.Obj_Sub_Type = oj[3].ToString();
                    WFobj.Fac_Name = oj[4].ToString();
                    WFobj.Current_Process = oj[5].ToString();
                    WFobj.Current_Arrival_Time = Convert.ToDateTime(oj[6]);
                    WFobj.Current_Owner = oj[7].ToString();
                    WFobj.Priority = oj[8].ToString();
                    WFobj.Parent_Obj_Type = oj[9].ToString();
                    WFobj.Parent_Obj_System_Id = Convert.ToUInt32(oj[10]);
                    WFobj.Process_Allocation = oj[11].ToString();
                    WFobj.Version = Convert.ToInt32(oj[12]);
                    WFobj.Doc_Type = oj[13].ToString();
                    WFobj.Doc_Sub_Type = oj[14].ToString();
                    //Srividhya
                    WFobj.Is_Default_MyQ_LineItem = Convert.ToInt32(oj[15]);
                    //naveena For bug ID= 45804
                    WFobj.Is_Abnormal = oj[16].ToString();
                }
            }
            // iMySession.Close();

            if (sCurrentProcess != null)
            {
                Boolean bProceed = false;
                for (int i = 0; i < sCurrentProcess.Length; i++)
                {
                    if (sCurrentProcess[i] == WFobj.Current_Process)
                    {
                        bProceed = true;
                        break;
                    }
                }
                if (bProceed == false)
                {
                    return 0;
                }
            }

            //Modified By Selvaraman on 24-Aug-2010
            WorkFlowManager WFManager = new WorkFlowManager();
            ObjectProcessHistoryManager ObjProcHistoryMngr = new ObjectProcessHistoryManager();
            ObjectProcessHistory ObjProcHistoryRecord = new ObjectProcessHistory();

            //To load the Original Version of the WF Object
            WFObject OriginalWFObj = new WFObject();
            OriginalWFObj.Current_Arrival_Time = WFobj.Current_Arrival_Time;
            if (WFobj.Current_Process == "CHECK_OUT")
            {
                OriginalWFObj.Current_Owner = sOwner;
                sOwner = "UNKNOWN";
            }
            else
            {
                OriginalWFObj.Current_Owner = WFobj.Current_Owner;
            }
            OriginalWFObj.Current_Process = WFobj.Current_Process;
            OriginalWFObj.Fac_Name = WFobj.Fac_Name;
            OriginalWFObj.Id = WFobj.Id;
            OriginalWFObj.Obj_Sub_Type = WFobj.Obj_Sub_Type;
            OriginalWFObj.Obj_System_Id = WFobj.Obj_System_Id;
            OriginalWFObj.Obj_Type = WFobj.Obj_Type;
            OriginalWFObj.Parent_Obj_System_Id = WFobj.Parent_Obj_System_Id;
            OriginalWFObj.Parent_Obj_Type = WFobj.Parent_Obj_Type;
            OriginalWFObj.Priority = WFobj.Priority;
            OriginalWFObj.Version = WFobj.Version;

            IList<WFObject> OriginalWFList = new List<WFObject>();
            OriginalWFList.Add(OriginalWFObj);

            string ToProcess = string.Empty;
            string WorkFlowType = string.Empty;
            //To find the Next process
            //IList<WorkFlow> wfList = NHibernateSessionUtility.Instance.MyWorkFlowList;
            //thows index of out range exception when toprocess not found for current process in workflow 
            //ToProcess = wfList.Where(a => a.Fac_Name == WFobj.Fac_Name && a.Obj_Type == WFobj.Obj_Type && a.Obj_Sub_Type == WFobj.Obj_Sub_Type && a.From_Process == WFobj.Current_Process && a.Close_Type == iCloseType && a.Doc_Type == WFobj.Doc_Type && a.Doc_Sub_Type == WFobj.Doc_Sub_Type).ToList()[0].To_Process;
            //Commented for Performance Tuning - To Reduce Memmory Usage --//BugID:53695 
            // var objWorkflow = wfList.Where(a => a.Fac_Name == WFobj.Fac_Name && a.Obj_Type == WFobj.Obj_Type && a.Obj_Sub_Type == WFobj.Obj_Sub_Type && a.From_Process == WFobj.Current_Process && a.Close_Type == iCloseType && a.Doc_Type == WFobj.Doc_Type && a.Doc_Sub_Type == WFobj.Doc_Sub_Type).ToList();

            //Modified By Selvaraman on 26-Aug-2022 - To move the DIAGNOSTIC_RESULT with Facility_Name as ALL
            if (WFobj.Fac_Name == "ALL")
            {
                var objWorkflowType = NHibernateSessionUtility.Instance.MyWorkFlowTypeMasterList.ToList();

                WorkFlowType = objWorkflowType[0].Workflow_Type;
            }
            IList<FacilityLibrary> FacList = NHibernateSessionUtility.Instance.MyFacilityList.Where(a => a.Fac_Name == WFobj.Fac_Name).ToList();

            if (FacList.Count > 0)
            {
                var objWorkflowType = NHibernateSessionUtility.Instance.MyWorkFlowTypeMasterList.Where(a => a.Legal_Org == FacList[0].Legal_Org && a.Facility_Name == WFobj.Fac_Name).ToList();

                if (objWorkflowType.Count == 0)
                {
                    var objWorkflowTypeDefault = NHibernateSessionUtility.Instance.MyWorkFlowTypeMasterList.Where(a => a.Legal_Org == FacList[0].Legal_Org && a.Facility_Name == "DEFAULT").ToList();
                    if (objWorkflowTypeDefault.Count == 0)
                    {
                        throw new Exception("Workflow Type is not found -" + FacList[0].Legal_Org + " -" + "DEFAULT");
                    }
                    else
                    {
                        WorkFlowType = objWorkflowTypeDefault[0].Workflow_Type;
                    }
                }
                else
                {
                    WorkFlowType = objWorkflowType[0].Workflow_Type;
                }
            }

            //var objWorkflow = NHibernateSessionUtility.Instance.MyWorkFlowList.Where(a => a.Fac_Name == WFobj.Fac_Name && a.Obj_Type == WFobj.Obj_Type && a.Obj_Sub_Type == WFobj.Obj_Sub_Type && a.From_Process == WFobj.Current_Process && a.Close_Type == iCloseType && a.Doc_Type == WFobj.Doc_Type && a.Doc_Sub_Type == WFobj.Doc_Sub_Type).ToList();
            //if (objWorkflow.Count() > 0)m 
            //    ToProcess = ((WorkFlow)objWorkflow[0]).To_Process;
            //ToProcess = WFManager.GetNextProcess(WFobj.Fac_Name, WFobj.Obj_Type, WFobj.Obj_Sub_Type, WFobj.Current_Process, iCloseType, WFobj.Doc_Type, WFobj.Doc_Sub_Type);

            var objWorkflow = NHibernateSessionUtility.Instance.MyWorkFlowList.Where(a => a.Workflow_Type == WorkFlowType && a.Obj_Type == WFobj.Obj_Type && a.Obj_Sub_Type == WFobj.Obj_Sub_Type && a.From_Process == WFobj.Current_Process && a.Close_Type == iCloseType && a.Doc_Type == WFobj.Doc_Type && a.Doc_Sub_Type == WFobj.Doc_Sub_Type).ToList();
            if (objWorkflow.Count() > 0)
                ToProcess = ((WorkFlow)objWorkflow[0]).To_Process;

            if (ToProcess == string.Empty)
            {
                //throw new Exception("To Process is not found from the Workflow Table");
                if (WFobj.Id != 0)
                    throw new Exception("To Process is not found -" + WFobj.Fac_Name + " -" + WFobj.Id.ToString() + " _" + iCloseType.ToString() + " -" + WFobj.Obj_Type + " -" + WFobj.Obj_Sub_Type + " -" + WFobj.Current_Process);
                else
                {
                    throw new Exception("Please report this to the support team and process the next file [No Record found for ObjectSystemID:" + uObjSystemId.ToString() + " and ObjType:" + ObjType + "with current process:" + WFobj.Current_Process + " and close type:" + iCloseType.ToString() + "]");//BugID:54697
                }
                //return 1;
            }

            IList<ObjectMaster> ObjMasterList = new List<ObjectMaster>();
            ObjectMasterManager ObjMasterMngr = new ObjectMasterManager();

            ProcessMasterManager ProcMngr = new ProcessMasterManager();
            IList<ProcessMaster> ProcFulllist = new List<ProcessMaster>();
            IList<ProcessMaster> Proclist = new List<ProcessMaster>();


            /***  commented for perfomance tuning
                      * not required whole ProcessList,filtering by ToProcess
                      *   by Jisha   ***/

            //ProcFulllist = ProcMngr.GetAllProcessList(ToProcess);
            //var Proc = from p in ProcFulllist where p.Process_Name == ToProcess select p;
            //Proclist = Proc.ToList<ProcessMaster>();
            //IList<ProcessMaster> ProcessList = NHibernateSessionUtility.Instance.MyProcessmasterList;
            //Commented for Performance Tuning - To Reduce Memmory Usage --//BugID:53695 
            //Proclist = ProcessList.Where(a => a.Process_Name == ToProcess).ToList(); // ProcMngr.GetProcessList(ToProcess);
            Proclist = NHibernateSessionUtility.Instance.MyProcessmasterList.Where(a => a.Process_Name == ToProcess).ToList(); // ProcMngr.GetProcessList(ToProcess);
            //To Create the Session and the Transaction
            ISession MySession = null;
            ITransaction trans = null;

            try
            {
                iTryCount = 0;

            TryAgain:
                int iResult = 0;

                if (MySessionHere == null)
                {
                    MySession = session.GetISession();
                    trans = MySession.BeginTransaction();
                }
                else
                {
                    MySession = MySessionHere;
                }

                //Note: Currently, In EHR - Wellness, ParentChild Relationship is not maintained.
                #region ParentChildPush
                ////To Find whether the Current Process is the Wait Process
                //Proclist = ProcMngr.GetAllProcessList();
                //var Proc = from p in Proclist where p.Process_Name == WFobj.Current_Process select p;
                //Proclist = Proc.ToList<ProcessMaster>();

                //if (Proclist[0].Wait_Process.ToUpper() == "Y")
                //{
                //    Boolean bMoveOtherObj = false;

                //    IList<WFObject> AllAddObjects = null;
                //    IList<WFObject> AllObjects = new List<WFObject>();

                //    IList<WFObject> ForObjProcHistory = new List<WFObject>();
                //    WFObject objForObjProcHistory;

                //    //If Current Object is Parent Object
                //    if (WFobj.Parent_Obj_System_Id == 0)
                //    {
                //        WFobj.Current_Process = ToProcess;
                //        WFobj.Current_Arrival_Time = DateTime.Now.ToUniversalTime();
                //        WFobj.Current_Owner = sOwner;
                //        AllObjects.Add(WFobj);

                //        //Check all the Childs
                //        IList<WFObject> ChildObjList = null;
                //        ChildObjList = GetByParentObjectSystemId(WFobj.Obj_System_Id, WFobj.Obj_Type);

                //        for (int i = 0; i < ChildObjList.Count; i++)
                //        {
                //            if (ChildObjList[i].Current_Process == WFobj.Current_Process)
                //            {
                //                //To fill object process history table - Start
                //                objForObjProcHistory = new WFObject();
                //                objForObjProcHistory.Current_Arrival_Time = ChildObjList[i].Current_Arrival_Time;
                //                objForObjProcHistory.Current_Owner = ChildObjList[i].Current_Owner;
                //                objForObjProcHistory.Current_Process = ChildObjList[i].Current_Process;
                //                objForObjProcHistory.Id = ChildObjList[i].Id;
                //                objForObjProcHistory.Obj_Sub_Type = ChildObjList[i].Obj_Sub_Type;
                //                objForObjProcHistory.Obj_System_Id = ChildObjList[i].Obj_System_Id;
                //                objForObjProcHistory.Obj_Type = ChildObjList[i].Obj_Type;
                //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
                //                objForObjProcHistory.Parent_Obj_System_Id = ChildObjList[i].Parent_Obj_System_Id;
                //                objForObjProcHistory.Parent_Obj_Type = ChildObjList[i].Parent_Obj_Type;
                //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
                //                objForObjProcHistory.Version = ChildObjList[i].Version;
                //                ForObjProcHistory.Add(objForObjProcHistory);
                //                //To fill object process history table - End

                //                ChildObjList[i].Current_Process = ToProcess;
                //                ChildObjList[i].Current_Arrival_Time = DateTime.Now.ToUniversalTime();
                //                ChildObjList[i].Current_Owner = sOwner;
                //                AllObjects.Add(ChildObjList[i]);
                //                bMoveOtherObj = true;
                //            }
                //            else
                //            {
                //                bMoveOtherObj = false;
                //                break;
                //            }
                //        }


                //        //If Everythng is in WaitProcess - Move to the Next Process
                //        if (bMoveOtherObj == true)
                //        {
                //            //Changed by Selvaraman - 21-Feb-11
                //            SaveUpdateDeleteWithoutTransaction(ref AllAddObjects, AllObjects, null, MySession, sMacAddress);
                //            //To Append to the ObjectPorcessHistory Table
                //            ObjProcHistoryMngr.AppendToObjectProcessHistory(ForObjProcHistory, MySession, StartTime, sMacAddress);
                //            if (MySessionHere == null)
                //            {
                //                MySession.Flush();
                //                trans.Commit();
                //                MySession.Close();
                //            }
                //            return 0;
                //        }
                //    }
                //    //Else if Current Object is Child Object
                //    else
                //    {
                //        //Check the Parent and all the Childs 
                //        WFObject ParentObj = null;
                //        IList<WFObject> ChildObjList = null;

                //        //Check the Parent
                //        ParentObj = GetByObjectSystemId(WFobj.Parent_Obj_System_Id, WFobj.Parent_Obj_Type);
                //        if (ParentObj.Current_Process == WFobj.Current_Process)
                //        {
                //            //To fill object process history table - Start
                //            objForObjProcHistory = new WFObject();
                //            objForObjProcHistory.Current_Arrival_Time = ParentObj.Current_Arrival_Time;
                //            objForObjProcHistory.Current_Owner = ParentObj.Current_Owner;
                //            objForObjProcHistory.Current_Process = ParentObj.Current_Process;
                //            objForObjProcHistory.Id = ParentObj.Id;
                //            objForObjProcHistory.Obj_Sub_Type = ParentObj.Obj_Sub_Type;
                //            objForObjProcHistory.Obj_System_Id = ParentObj.Obj_System_Id;
                //            objForObjProcHistory.Obj_Type = ParentObj.Obj_Type;
                //            objForObjProcHistory.Priority = ParentObj.Priority;
                //            objForObjProcHistory.Parent_Obj_System_Id = ParentObj.Parent_Obj_System_Id;
                //            objForObjProcHistory.Parent_Obj_Type = ParentObj.Parent_Obj_Type;
                //            objForObjProcHistory.Priority = ParentObj.Priority;
                //            objForObjProcHistory.Version = ParentObj.Version;
                //            ForObjProcHistory.Add(objForObjProcHistory);
                //            //To fill object process history table - End

                //            ParentObj.Current_Process = ToProcess;
                //            ParentObj.Current_Arrival_Time = DateTime.Now.ToUniversalTime();
                //            ParentObj.Current_Owner = sOwner;
                //            AllObjects.Add(ParentObj);
                //            bMoveOtherObj = true;
                //        }
                //        else
                //        {
                //            bMoveOtherObj = false;
                //        }

                //        //Check all the Childs
                //        ChildObjList = GetByParentObjectSystemId(WFobj.Parent_Obj_System_Id, WFobj.Parent_Obj_Type);
                //        for (int i = 0; i < ChildObjList.Count; i++)
                //        {
                //            if (ChildObjList[i].Current_Process == WFobj.Current_Process)
                //            {
                //                //To fill object process history table - Start
                //                objForObjProcHistory = new WFObject();
                //                objForObjProcHistory.Current_Arrival_Time = ChildObjList[i].Current_Arrival_Time;
                //                objForObjProcHistory.Current_Owner = ChildObjList[i].Current_Owner;
                //                objForObjProcHistory.Current_Process = ChildObjList[i].Current_Process;
                //                objForObjProcHistory.Id = ChildObjList[i].Id;
                //                objForObjProcHistory.Obj_Sub_Type = ChildObjList[i].Obj_Sub_Type;
                //                objForObjProcHistory.Obj_System_Id = ChildObjList[i].Obj_System_Id;
                //                objForObjProcHistory.Obj_Type = ChildObjList[i].Obj_Type;
                //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
                //                objForObjProcHistory.Parent_Obj_System_Id = ChildObjList[i].Parent_Obj_System_Id;
                //                objForObjProcHistory.Parent_Obj_Type = ChildObjList[i].Parent_Obj_Type;
                //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
                //                objForObjProcHistory.Version = ChildObjList[i].Version;

                //                ForObjProcHistory.Add(objForObjProcHistory);

                //                //To fill object process history table - End

                //                ChildObjList[i].Current_Process = ToProcess;
                //                ChildObjList[i].Current_Arrival_Time = DateTime.Now.ToUniversalTime();
                //                ChildObjList[i].Current_Owner = sOwner;
                //                AllObjects.Add(ChildObjList[i]);
                //                bMoveOtherObj = true;
                //            }
                //            else
                //            {
                //                bMoveOtherObj = false;
                //                break;
                //            }
                //        }

                //        //If Everythng is in WaitProcess - Move to the Next Process
                //        if (bMoveOtherObj == true)
                //        {
                //            //Changed by Selvaraman - 21-Feb-11
                //            SaveUpdateDeleteWithoutTransaction(ref AllAddObjects, AllObjects, null, MySession, sMacAddress);
                //            //To Append to the ObjectPorcessHistory Table
                //            ObjProcHistoryMngr.AppendToObjectProcessHistory(ForObjProcHistory, MySession, StartTime, sMacAddress);
                //            if (MySessionHere == null)
                //            {
                //                MySession.Flush();
                //                trans.Commit();
                //                MySession.Close();
                //            }
                //            return 0;
                //        }
                //    }
                //}
                #endregion

                //To move the current object into next process
                //Srividhya
                WFobj.Is_Default_MyQ_LineItem = 1;
                WFobj.Current_Process = ToProcess;
                WFobj.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                WFobj.Current_Owner = sOwner;
                if (sOwner == "UNKNOWN")
                {
                    //To get the equivalant allocation process to get the owner
                    if (Proclist[0].Equivalant_Allocation_Process != string.Empty)
                    {
                        string[] sAlloc = WFobj.Process_Allocation.Split('|');
                        string sEquivalantOwner = string.Empty;
                        //for (int i = 0; i < sAlloc.Length; i++)
                        for (int i = sAlloc.Length - 1; i >= 0; i--)
                        {
                            if (sAlloc[i].StartsWith(Proclist[0].Equivalant_Allocation_Process + "-") == true)
                            {
                                string[] sString = sAlloc[i].Split('-');
                                sEquivalantOwner = sString[1];
                                WFobj.Current_Owner = sEquivalantOwner;
                                sOwner = sEquivalantOwner;
                                break;
                            }
                        }
                    }
                    //To check and assign the owner if already the owner is assigned previously.
                    //else
                    //{
                    //    string[] sAlloc = WFobj.Process_Allocation.Split('|');
                    //    string sEquivalantOwner = string.Empty;
                    //    for (int i = 0; i < sAlloc.Length; i++)
                    //    {
                    //        if (sAlloc[i].StartsWith(WFobj.Current_Process + "-") == true)
                    //        {
                    //            string[] sString = sAlloc[i].Split('-');
                    //            sEquivalantOwner = sString[1];
                    //            WFobj.Current_Owner = sEquivalantOwner;
                    //            sOwner = sEquivalantOwner;
                    //            break;
                    //        }
                    //    }
                    //    if (sEquivalantOwner == string.Empty)
                    //    {
                    //        WFobj.Current_Owner = sOwner; //it is UNKNOWN
                    //    }
                    //}
                }

                WFobj.Process_Allocation = WFobj.Process_Allocation + "|" + ToProcess + "-" + sOwner;

                IList<WFObject> wfObjList = new List<WFObject>();
                GenerateXml XMLObj = null;
                wfObjList.Add(WFobj);

                //ISQLQuery sql = session.GetISession().CreateSQLQuery("Select w.* from wf_object w where w.obj_system_id=" + uObjSystemId + " and w.Obj_Type='" + ObjType + "' ")
                //    .AddEntity("w.*", typeof(WFObject));
                //IList<WFObject> wfObjectList = sql.List<WFObject>();
                ////WFObject WFobj = new WFObject();
                //if (wfObjectList != null && wfObjectList.Count > 0)
                //{
                //    wfObjectList[0].Doc_Type = "";
                //    wfObjectList[0].Doc_Sub_Type = "";
                //    //WFobj = wfObjectList[0];//  GetByObjectSystemId(uObjSystemId, ObjType);
                //}

                IList<WFObject> addList = null;

                // iResult = SaveUpdateDeleteWithoutTransaction(ref addList, wfObjList, null, MySession, sMacAddress);
                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref addList, ref wfObjList, null, MySession, sMacAddress, false, false, 0, string.Empty, ref XMLObj);

                //iResult = SaveUpdateDeleteWithoutTransaction(ref addList, wfObjectList, null, MySession, sMacAddress);

                //if bResult = false then, the deadlock is occured 
                if (iResult == 2)
                {
                    if (iTryCount < 5)
                    {
                        iTryCount++;
                        goto TryAgain;
                    }
                    else
                    {
                        if (MySessionHere == null)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Deadlock is occured. Transaction failed");
                        }
                        else
                        {
                            return iResult;
                        }
                    }
                }
                else if (iResult == 1)
                {
                    if (MySessionHere == null)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception is occured. Transaction failed");
                    }
                    else
                    {
                        return iResult;
                    }
                }
                //To Append to the ObjectPorcessHistory Table
                iResult = ObjProcHistoryMngr.AppendToObjectProcessHistory(OriginalWFList, MySession, StartTime, sMacAddress);
                //if bResult = false then, the deadlock is occured 
                if (iResult == 2)
                {
                    if (iTryCount < 5)
                    {
                        iTryCount++;
                        goto TryAgain;
                    }
                    else
                    {
                        if (MySessionHere == null)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Deadlock is occured. Transaction failed");
                        }
                        else
                        {
                            return iResult;
                        }
                    }
                }
                else if (iResult == 1)
                {
                    if (MySessionHere == null)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception is occured. Transaction failed");
                    }
                    else
                    {
                        return iResult;
                    }
                }

                if (MySessionHere == null)
                {
                    MySession.Flush();
                    trans.Commit();
                }
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (MySessionHere == null)
                {
                    trans.Rollback();
                    //MySession.Close();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception e)
            {
                if (MySessionHere == null)
                {
                    trans.Rollback();
                    // MySession.Close();
                    throw new Exception(e.Message);
                }
            }
            finally
            {
                if (MySessionHere == null)
                {
                    MySession.Close();
                }
            }
            return 0;
        }
        public int MoveToNextProcessscribe(ulong uObjSystemId, string ObjType, int iCloseType, string sOwner, DateTime StartTime, string sMacAddress, string[] sCurrentProcess, ISession MySessionHere)
        {
            //To get the latest version
            //srividhya comm
            //ISQLQuery sql = session.GetISession().CreateSQLQuery("Select w.* from wf_object w left join process_master m on(w.current_process=m.process_name) where w.obj_system_id=" + uObjSystemId + " and w.Obj_Type='" + ObjType + "' and m.Is_Deleted_Process<>'Y'")
            //    .AddEntity("w.*", typeof(WFObject));
            //  IList<WFObject> wfObjectList=sql.List<WFObject>();
            WFObject WFobj = new WFObject();
            //  if (wfObjectList != null && wfObjectList.Count > 0)
            //  {
            //      WFobj = wfObjectList[0];//  GetByObjectSystemId(uObjSystemId, ObjType);
            //  }
            //srividhya comm

            ArrayList MyList = new ArrayList();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            // IQuery query1 = iMySession.GetNamedQuery("Get.WFObject.Encounter.Details");
            IQuery query1 = session.GetISession().GetNamedQuery("Get.WFObject.Encounter.Details");

            query1.SetString(0, uObjSystemId.ToString());
            query1.SetString(1, ObjType);


            MyList.AddRange(query1.List());
            //BugID:54697
            if (MyList == null || (MyList != null && MyList.Count == 0) || query1.List().Count == 0)
            {
                string curr_process = string.Empty;
                if (sCurrentProcess != null && sCurrentProcess.Count() > 0)
                {
                    curr_process = "CurrentProcess: " + sCurrentProcess[0];
                }
                throw new Exception("Please report this to the support team and process the next file [No Record found for ObjectSystemID:" + uObjSystemId.ToString() + " and ObjType:" + ObjType + curr_process + "]");
                //return 1;
            }

            if (MyList != null)
            {
                for (int i = 0; i < MyList.Count; i++)
                {
                    object[] oj = (object[])MyList[i];

                    //string s = MyList[0].ToString();
                    WFobj.Id = Convert.ToUInt32(oj[0]);
                    WFobj.Obj_System_Id = Convert.ToUInt32(oj[1]);
                    WFobj.Obj_Type = oj[2].ToString();
                    WFobj.Obj_Sub_Type = oj[3].ToString();
                    WFobj.Fac_Name = oj[4].ToString();
                    WFobj.Current_Process = oj[5].ToString();
                    WFobj.Current_Arrival_Time = Convert.ToDateTime(oj[6]);
                    WFobj.Current_Owner = oj[7].ToString();
                    WFobj.Priority = oj[8].ToString();
                    WFobj.Parent_Obj_Type = oj[9].ToString();
                    WFobj.Parent_Obj_System_Id = Convert.ToUInt32(oj[10]);
                    WFobj.Process_Allocation = oj[11].ToString();
                    WFobj.Version = Convert.ToInt32(oj[12]);
                    WFobj.Doc_Type = oj[13].ToString();
                    WFobj.Doc_Sub_Type = oj[14].ToString();
                    //Srividhya
                    WFobj.Is_Default_MyQ_LineItem = Convert.ToInt32(oj[15]);
                    //naveena For bug ID= 45804
                    WFobj.Is_Abnormal = oj[16].ToString();
                }
            }
            // iMySession.Close();

            if (sCurrentProcess != null)
            {
                Boolean bProceed = false;
                for (int i = 0; i < sCurrentProcess.Length; i++)
                {
                    if (sCurrentProcess[i] == WFobj.Current_Process)
                    {
                        bProceed = true;
                        break;
                    }
                }
                if (bProceed == false)
                {
                    return 0;
                }
            }

            //Modified By Selvaraman on 24-Aug-2010
            WorkFlowManager WFManager = new WorkFlowManager();
            ObjectProcessHistoryManager ObjProcHistoryMngr = new ObjectProcessHistoryManager();
            ObjectProcessHistory ObjProcHistoryRecord = new ObjectProcessHistory();

            //To load the Original Version of the WF Object
            WFObject OriginalWFObj = new WFObject();
            OriginalWFObj.Current_Arrival_Time = WFobj.Current_Arrival_Time;
            if (WFobj.Current_Process == "CHECK_OUT")
            {
                OriginalWFObj.Current_Owner = sOwner;
                sOwner = "UNKNOWN";
            }
            else
            {
                OriginalWFObj.Current_Owner = WFobj.Current_Owner;
            }
            OriginalWFObj.Current_Process = WFobj.Current_Process;
            OriginalWFObj.Fac_Name = WFobj.Fac_Name;
            OriginalWFObj.Id = WFobj.Id;
            OriginalWFObj.Obj_Sub_Type = WFobj.Obj_Sub_Type;
            OriginalWFObj.Obj_System_Id = WFobj.Obj_System_Id;
            OriginalWFObj.Obj_Type = WFobj.Obj_Type;
            OriginalWFObj.Parent_Obj_System_Id = WFobj.Parent_Obj_System_Id;
            OriginalWFObj.Parent_Obj_Type = WFobj.Parent_Obj_Type;
            OriginalWFObj.Priority = WFobj.Priority;
            OriginalWFObj.Version = WFobj.Version;

            IList<WFObject> OriginalWFList = new List<WFObject>();
            OriginalWFList.Add(OriginalWFObj);

            string ToProcess = string.Empty;
            string WorkFlowType = string.Empty;
            //To find the Next process
            //IList<WorkFlow> wfList = NHibernateSessionUtility.Instance.MyWorkFlowList;
            //thows index of out range exception when toprocess not found for current process in workflow 
            //ToProcess = wfList.Where(a => a.Fac_Name == WFobj.Fac_Name && a.Obj_Type == WFobj.Obj_Type && a.Obj_Sub_Type == WFobj.Obj_Sub_Type && a.From_Process == WFobj.Current_Process && a.Close_Type == iCloseType && a.Doc_Type == WFobj.Doc_Type && a.Doc_Sub_Type == WFobj.Doc_Sub_Type).ToList()[0].To_Process;
            //Commented for Performance Tuning - To Reduce Memmory Usage --//BugID:53695 
            // var objWorkflow = wfList.Where(a => a.Fac_Name == WFobj.Fac_Name && a.Obj_Type == WFobj.Obj_Type && a.Obj_Sub_Type == WFobj.Obj_Sub_Type && a.From_Process == WFobj.Current_Process && a.Close_Type == iCloseType && a.Doc_Type == WFobj.Doc_Type && a.Doc_Sub_Type == WFobj.Doc_Sub_Type).ToList();
            IList<FacilityLibrary> FacList = NHibernateSessionUtility.Instance.MyFacilityList.Where(a => a.Fac_Name == WFobj.Fac_Name).ToList();

            if (FacList.Count > 0)
            {
                var objWorkflowType = NHibernateSessionUtility.Instance.MyWorkFlowTypeMasterList.Where(a => a.Legal_Org == FacList[0].Legal_Org && a.Facility_Name == WFobj.Fac_Name).ToList();
                if (objWorkflowType.Count == 0)
                {
                    var objWorkflowTypeDefault = NHibernateSessionUtility.Instance.MyWorkFlowTypeMasterList.Where(a => a.Legal_Org == FacList[0].Legal_Org && a.Facility_Name == "DEFAULT").ToList();
                    if (objWorkflowTypeDefault.Count == 0)
                    {
                        throw new Exception("Workflow Type is not found -" + FacList[0].Legal_Org + " -" + "DEFAULT");
                    }
                    else
                    {
                        WorkFlowType = objWorkflowTypeDefault[0].Workflow_Type;
                    }
                }
                else
                {
                    WorkFlowType = objWorkflowType[0].Workflow_Type;
                }
            }
            //var objWorkflow = NHibernateSessionUtility.Instance.MyWorkFlowList.Where(a => a.Fac_Name == WFobj.Fac_Name && a.Obj_Type == WFobj.Obj_Type && a.Obj_Sub_Type == WFobj.Obj_Sub_Type && a.From_Process == WFobj.Current_Process && a.Close_Type == iCloseType && a.Doc_Type == WFobj.Doc_Type && a.Doc_Sub_Type == WFobj.Doc_Sub_Type).ToList();
            //if (objWorkflow.Count() > 0)
            //    ToProcess = ((WorkFlow)objWorkflow[0]).To_Process;
            //ToProcess = WFManager.GetNextProcess(WFobj.Fac_Name, WFobj.Obj_Type, WFobj.Obj_Sub_Type, WFobj.Current_Process, iCloseType, WFobj.Doc_Type, WFobj.Doc_Sub_Type);

            var objWorkflow = NHibernateSessionUtility.Instance.MyWorkFlowList.Where(a => a.Workflow_Type == WorkFlowType && a.Obj_Type == WFobj.Obj_Type && a.Obj_Sub_Type == WFobj.Obj_Sub_Type && a.From_Process == WFobj.Current_Process && a.Close_Type == iCloseType && a.Doc_Type == WFobj.Doc_Type && a.Doc_Sub_Type == WFobj.Doc_Sub_Type).ToList();
            if (objWorkflow.Count() > 0)
                ToProcess = ((WorkFlow)objWorkflow[0]).To_Process;
            if (ToProcess == string.Empty)
            {
                //throw new Exception("To Process is not found from the Workflow Table");
                if (WFobj.Id != 0)
                    throw new Exception("To Process is not found -" + WFobj.Fac_Name + " -" + WFobj.Id.ToString() + " _" + iCloseType.ToString() + " -" + WFobj.Obj_Type + " -" + WFobj.Obj_Sub_Type + " -" + WFobj.Current_Process);
                else
                {
                    throw new Exception("Please report this to the support team and process the next file [No Record found for ObjectSystemID:" + uObjSystemId.ToString() + " and ObjType:" + ObjType + "with current process:" + WFobj.Current_Process + " and close type:" + iCloseType.ToString() + "]");//BugID:54697
                }
                //return 1;
            }

            IList<ObjectMaster> ObjMasterList = new List<ObjectMaster>();
            ObjectMasterManager ObjMasterMngr = new ObjectMasterManager();

            ProcessMasterManager ProcMngr = new ProcessMasterManager();
            IList<ProcessMaster> ProcFulllist = new List<ProcessMaster>();
            IList<ProcessMaster> Proclist = new List<ProcessMaster>();


            /***  commented for perfomance tuning
                      * not required whole ProcessList,filtering by ToProcess
                      *   by Jisha   ***/

            //ProcFulllist = ProcMngr.GetAllProcessList(ToProcess);
            //var Proc = from p in ProcFulllist where p.Process_Name == ToProcess select p;
            //Proclist = Proc.ToList<ProcessMaster>();
            //IList<ProcessMaster> ProcessList = NHibernateSessionUtility.Instance.MyProcessmasterList;
            //Commented for Performance Tuning - To Reduce Memmory Usage --//BugID:53695 
            //Proclist = ProcessList.Where(a => a.Process_Name == ToProcess).ToList(); // ProcMngr.GetProcessList(ToProcess);
            Proclist = NHibernateSessionUtility.Instance.MyProcessmasterList.Where(a => a.Process_Name == ToProcess).ToList(); // ProcMngr.GetProcessList(ToProcess);
            //To Create the Session and the Transaction
            ISession MySession = null;
            ITransaction trans = null;

            try
            {
                iTryCount = 0;

            TryAgain:
                int iResult = 0;

                if (MySessionHere == null)
                {
                    MySession = session.GetISession();
                    trans = MySession.BeginTransaction();
                }
                else
                {
                    MySession = MySessionHere;
                }

                //Note: Currently, In EHR - Wellness, ParentChild Relationship is not maintained.
                #region ParentChildPush
                ////To Find whether the Current Process is the Wait Process
                //Proclist = ProcMngr.GetAllProcessList();
                //var Proc = from p in Proclist where p.Process_Name == WFobj.Current_Process select p;
                //Proclist = Proc.ToList<ProcessMaster>();

                //if (Proclist[0].Wait_Process.ToUpper() == "Y")
                //{
                //    Boolean bMoveOtherObj = false;

                //    IList<WFObject> AllAddObjects = null;
                //    IList<WFObject> AllObjects = new List<WFObject>();

                //    IList<WFObject> ForObjProcHistory = new List<WFObject>();
                //    WFObject objForObjProcHistory;

                //    //If Current Object is Parent Object
                //    if (WFobj.Parent_Obj_System_Id == 0)
                //    {
                //        WFobj.Current_Process = ToProcess;
                //        WFobj.Current_Arrival_Time = DateTime.Now.ToUniversalTime();
                //        WFobj.Current_Owner = sOwner;
                //        AllObjects.Add(WFobj);

                //        //Check all the Childs
                //        IList<WFObject> ChildObjList = null;
                //        ChildObjList = GetByParentObjectSystemId(WFobj.Obj_System_Id, WFobj.Obj_Type);

                //        for (int i = 0; i < ChildObjList.Count; i++)
                //        {
                //            if (ChildObjList[i].Current_Process == WFobj.Current_Process)
                //            {
                //                //To fill object process history table - Start
                //                objForObjProcHistory = new WFObject();
                //                objForObjProcHistory.Current_Arrival_Time = ChildObjList[i].Current_Arrival_Time;
                //                objForObjProcHistory.Current_Owner = ChildObjList[i].Current_Owner;
                //                objForObjProcHistory.Current_Process = ChildObjList[i].Current_Process;
                //                objForObjProcHistory.Id = ChildObjList[i].Id;
                //                objForObjProcHistory.Obj_Sub_Type = ChildObjList[i].Obj_Sub_Type;
                //                objForObjProcHistory.Obj_System_Id = ChildObjList[i].Obj_System_Id;
                //                objForObjProcHistory.Obj_Type = ChildObjList[i].Obj_Type;
                //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
                //                objForObjProcHistory.Parent_Obj_System_Id = ChildObjList[i].Parent_Obj_System_Id;
                //                objForObjProcHistory.Parent_Obj_Type = ChildObjList[i].Parent_Obj_Type;
                //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
                //                objForObjProcHistory.Version = ChildObjList[i].Version;
                //                ForObjProcHistory.Add(objForObjProcHistory);
                //                //To fill object process history table - End

                //                ChildObjList[i].Current_Process = ToProcess;
                //                ChildObjList[i].Current_Arrival_Time = DateTime.Now.ToUniversalTime();
                //                ChildObjList[i].Current_Owner = sOwner;
                //                AllObjects.Add(ChildObjList[i]);
                //                bMoveOtherObj = true;
                //            }
                //            else
                //            {
                //                bMoveOtherObj = false;
                //                break;
                //            }
                //        }


                //        //If Everythng is in WaitProcess - Move to the Next Process
                //        if (bMoveOtherObj == true)
                //        {
                //            //Changed by Selvaraman - 21-Feb-11
                //            SaveUpdateDeleteWithoutTransaction(ref AllAddObjects, AllObjects, null, MySession, sMacAddress);
                //            //To Append to the ObjectPorcessHistory Table
                //            ObjProcHistoryMngr.AppendToObjectProcessHistory(ForObjProcHistory, MySession, StartTime, sMacAddress);
                //            if (MySessionHere == null)
                //            {
                //                MySession.Flush();
                //                trans.Commit();
                //                MySession.Close();
                //            }
                //            return 0;
                //        }
                //    }
                //    //Else if Current Object is Child Object
                //    else
                //    {
                //        //Check the Parent and all the Childs 
                //        WFObject ParentObj = null;
                //        IList<WFObject> ChildObjList = null;

                //        //Check the Parent
                //        ParentObj = GetByObjectSystemId(WFobj.Parent_Obj_System_Id, WFobj.Parent_Obj_Type);
                //        if (ParentObj.Current_Process == WFobj.Current_Process)
                //        {
                //            //To fill object process history table - Start
                //            objForObjProcHistory = new WFObject();
                //            objForObjProcHistory.Current_Arrival_Time = ParentObj.Current_Arrival_Time;
                //            objForObjProcHistory.Current_Owner = ParentObj.Current_Owner;
                //            objForObjProcHistory.Current_Process = ParentObj.Current_Process;
                //            objForObjProcHistory.Id = ParentObj.Id;
                //            objForObjProcHistory.Obj_Sub_Type = ParentObj.Obj_Sub_Type;
                //            objForObjProcHistory.Obj_System_Id = ParentObj.Obj_System_Id;
                //            objForObjProcHistory.Obj_Type = ParentObj.Obj_Type;
                //            objForObjProcHistory.Priority = ParentObj.Priority;
                //            objForObjProcHistory.Parent_Obj_System_Id = ParentObj.Parent_Obj_System_Id;
                //            objForObjProcHistory.Parent_Obj_Type = ParentObj.Parent_Obj_Type;
                //            objForObjProcHistory.Priority = ParentObj.Priority;
                //            objForObjProcHistory.Version = ParentObj.Version;
                //            ForObjProcHistory.Add(objForObjProcHistory);
                //            //To fill object process history table - End

                //            ParentObj.Current_Process = ToProcess;
                //            ParentObj.Current_Arrival_Time = DateTime.Now.ToUniversalTime();
                //            ParentObj.Current_Owner = sOwner;
                //            AllObjects.Add(ParentObj);
                //            bMoveOtherObj = true;
                //        }
                //        else
                //        {
                //            bMoveOtherObj = false;
                //        }

                //        //Check all the Childs
                //        ChildObjList = GetByParentObjectSystemId(WFobj.Parent_Obj_System_Id, WFobj.Parent_Obj_Type);
                //        for (int i = 0; i < ChildObjList.Count; i++)
                //        {
                //            if (ChildObjList[i].Current_Process == WFobj.Current_Process)
                //            {
                //                //To fill object process history table - Start
                //                objForObjProcHistory = new WFObject();
                //                objForObjProcHistory.Current_Arrival_Time = ChildObjList[i].Current_Arrival_Time;
                //                objForObjProcHistory.Current_Owner = ChildObjList[i].Current_Owner;
                //                objForObjProcHistory.Current_Process = ChildObjList[i].Current_Process;
                //                objForObjProcHistory.Id = ChildObjList[i].Id;
                //                objForObjProcHistory.Obj_Sub_Type = ChildObjList[i].Obj_Sub_Type;
                //                objForObjProcHistory.Obj_System_Id = ChildObjList[i].Obj_System_Id;
                //                objForObjProcHistory.Obj_Type = ChildObjList[i].Obj_Type;
                //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
                //                objForObjProcHistory.Parent_Obj_System_Id = ChildObjList[i].Parent_Obj_System_Id;
                //                objForObjProcHistory.Parent_Obj_Type = ChildObjList[i].Parent_Obj_Type;
                //                objForObjProcHistory.Priority = ChildObjList[i].Priority;
                //                objForObjProcHistory.Version = ChildObjList[i].Version;

                //                ForObjProcHistory.Add(objForObjProcHistory);

                //                //To fill object process history table - End

                //                ChildObjList[i].Current_Process = ToProcess;
                //                ChildObjList[i].Current_Arrival_Time = DateTime.Now.ToUniversalTime();
                //                ChildObjList[i].Current_Owner = sOwner;
                //                AllObjects.Add(ChildObjList[i]);
                //                bMoveOtherObj = true;
                //            }
                //            else
                //            {
                //                bMoveOtherObj = false;
                //                break;
                //            }
                //        }

                //        //If Everythng is in WaitProcess - Move to the Next Process
                //        if (bMoveOtherObj == true)
                //        {
                //            //Changed by Selvaraman - 21-Feb-11
                //            SaveUpdateDeleteWithoutTransaction(ref AllAddObjects, AllObjects, null, MySession, sMacAddress);
                //            //To Append to the ObjectPorcessHistory Table
                //            ObjProcHistoryMngr.AppendToObjectProcessHistory(ForObjProcHistory, MySession, StartTime, sMacAddress);
                //            if (MySessionHere == null)
                //            {
                //                MySession.Flush();
                //                trans.Commit();
                //                MySession.Close();
                //            }
                //            return 0;
                //        }
                //    }
                //}
                #endregion

                //To move the current object into next process
                WFobj.Is_Default_MyQ_LineItem = 1;
                WFobj.Current_Process = ToProcess;
                WFobj.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                WFobj.Current_Owner = sOwner;


                WFobj.Process_Allocation = WFobj.Process_Allocation + "|" + ToProcess + "-" + sOwner;

                IList<WFObject> wfObjList = new List<WFObject>();
                GenerateXml XMLObj = null;
                wfObjList.Add(WFobj);

                //ISQLQuery sql = session.GetISession().CreateSQLQuery("Select w.* from wf_object w where w.obj_system_id=" + uObjSystemId + " and w.Obj_Type='" + ObjType + "' ")
                //    .AddEntity("w.*", typeof(WFObject));
                //IList<WFObject> wfObjectList = sql.List<WFObject>();
                ////WFObject WFobj = new WFObject();
                //if (wfObjectList != null && wfObjectList.Count > 0)
                //{
                //    wfObjectList[0].Doc_Type = "";
                //    wfObjectList[0].Doc_Sub_Type = "";
                //    //WFobj = wfObjectList[0];//  GetByObjectSystemId(uObjSystemId, ObjType);
                //}

                IList<WFObject> addList = null;

                // iResult = SaveUpdateDeleteWithoutTransaction(ref addList, wfObjList, null, MySession, sMacAddress);
                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref addList, ref wfObjList, null, MySession, sMacAddress, false, false, 0, string.Empty, ref XMLObj);

                //iResult = SaveUpdateDeleteWithoutTransaction(ref addList, wfObjectList, null, MySession, sMacAddress);

                //if bResult = false then, the deadlock is occured 
                if (iResult == 2)
                {
                    if (iTryCount < 5)
                    {
                        iTryCount++;
                        goto TryAgain;
                    }
                    else
                    {
                        if (MySessionHere == null)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Deadlock is occured. Transaction failed");
                        }
                        else
                        {
                            return iResult;
                        }
                    }
                }
                else if (iResult == 1)
                {
                    if (MySessionHere == null)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception is occured. Transaction failed");
                    }
                    else
                    {
                        return iResult;
                    }
                }
                //To Append to the ObjectPorcessHistory Table
                iResult = ObjProcHistoryMngr.AppendToObjectProcessHistory(OriginalWFList, MySession, StartTime, sMacAddress);
                //if bResult = false then, the deadlock is occured 
                if (iResult == 2)
                {
                    if (iTryCount < 5)
                    {
                        iTryCount++;
                        goto TryAgain;
                    }
                    else
                    {
                        if (MySessionHere == null)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Deadlock is occured. Transaction failed");
                        }
                        else
                        {
                            return iResult;
                        }
                    }
                }
                else if (iResult == 1)
                {
                    if (MySessionHere == null)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception is occured. Transaction failed");
                    }
                    else
                    {
                        return iResult;
                    }
                }

                if (MySessionHere == null)
                {
                    MySession.Flush();
                    trans.Commit();
                }
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (MySessionHere == null)
                {
                    trans.Rollback();
                    //MySession.Close();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception e)
            {
                if (MySessionHere == null)
                {
                    trans.Rollback();
                    // MySession.Close();
                    throw new Exception(e.Message);
                }
            }
            finally
            {
                if (MySessionHere == null)
                {
                    MySession.Close();
                }
            }
            return 0;
        }

        //Added By ThiyagarajanM
        public int MultipleObjectsMoveToNextProcess(ulong uObjSystemIdForFO, string ObjTypeForFO, int iCloseTypeForFO, IList<ulong> uOrders_Submit_Id, string ObjType, int iCloseType, string sOwner, DateTime StartTime, string sMacAddress, string[] sCurrentProcess, ISession MySessionHere)
        {
            MoveToNextProcess(uObjSystemIdForFO, ObjTypeForFO, iCloseTypeForFO, "UNKNOWN", StartTime, sMacAddress, sCurrentProcess, MySessionHere);
            int j = 0;
            for (int i = 0; i < uOrders_Submit_Id.Count; i++)
            {
                j = MoveToNextProcess(uOrders_Submit_Id[i], ObjType, iCloseType, sOwner, StartTime, sMacAddress, sCurrentProcess, MySessionHere);

            }
            return j;
        }

        //Added By ThiyagarajanM 30-05-2013
        //int iTryCount, j = 0;
        public int MultipleObjectsMoveToNextProcessForDictation(ulong exception_ID, string ObjTypeForDictation, int Close_TypeForDictation, string sOwner, DateTime StartTime, string[] sCurrentProcess, ulong Encounter_ID, string objTypeForDocumentation, int Close_TypeForDocumentation, string MACAddress)
        {

            int i = 0;
            IList<WFObject> lst = new List<WFObject>();
            IList<Encounter> Enclst = new List<Encounter>();
            IList<User> Userlst = new List<User>();

            i = MoveToNextProcess(exception_ID, ObjTypeForDictation, Close_TypeForDictation, "UNKNOWN", StartTime, MACAddress, null, null);

            //ICriteria crit = session.GetISession().CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", Encounter_ID)).Add(Expression.Eq("Obj_Type", objTypeForDocumentation));
            //lst=crit.List<WFObject>();


            if (objTypeForDocumentation.Trim().ToUpper() == "DIAGNOSTIC ORDER")
            {

            }
            else
            {
                ICriteria Enccrit = session.GetISession().CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", Encounter_ID));
                Enclst = Enccrit.List<Encounter>();

                if (Enclst.Count > 0)
                {
                    ICriteria Usercrit = session.GetISession().CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Convert.ToUInt64(Enclst[0].Encounter_Provider_ID)));
                    Userlst = Usercrit.List<User>();
                }

                if (sCurrentProcess != null && sCurrentProcess[0].Trim().ToUpper() == "PROVIDER_PROCESS")
                    i = MoveToNextProcess(Encounter_ID, objTypeForDocumentation, Close_TypeForDocumentation, Userlst[0].user_name, StartTime, MACAddress, null, null);
                else
                    UpdateOwner(Encounter_ID, objTypeForDocumentation, Userlst[0].user_name, MACAddress);

            }
            return i;
        }
        //Added By ThiyagarajanM 19-06-2013
        public void MoveToNextProcessForTwoObject(ulong uObjSystemId, string[] ObjType, string sOwner, int iClose_TypeForFirstObject, int iClose_TypeForSecondObject, DateTime StartTime, string[] sCurrentProcess, string MACAddress)
        {
            if (ObjType.Length == 2)
            {
                MoveToNextProcess(uObjSystemId, ObjType[0].Trim(), iClose_TypeForFirstObject, sOwner, StartTime, MACAddress, null, null);
                MoveToNextProcess(uObjSystemId, ObjType[1].Trim(), iClose_TypeForSecondObject, sOwner, StartTime, MACAddress, null, null);
            }
        }


        public int DictationMoveToNextProcess(ulong Exception_Encounter_Id, string obj_type, int close_Type, DateTime date, string MACAddress)
        {
            int i = 0;
            IList<Encounter> Enclst = new List<Encounter>();

            IList<User> Userlst = new List<User>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", Exception_Encounter_Id));
                Enclst = crit.List<Encounter>();


                if (Enclst.Count > 0)
                {

                    ICriteria Usercrit = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Convert.ToUInt64(Enclst[0].Encounter_Provider_ID)));
                    Userlst = Usercrit.List<User>();
                    //Userlst[0].user_name
                    UpdateOwner(Exception_Encounter_Id, obj_type, Userlst[0].user_name, MACAddress);
                    i = MoveToNextProcess(Exception_Encounter_Id, obj_type, close_Type, Userlst[0].user_name, date, MACAddress, null, null);

                }
                iMySession.Close();
            }
            return i;


        }










        //Added By Dhinesh
        public int MoveToPreviousProcessWithTransaction(ulong ulObjSystemID, string ObjType, int iCloseType, string sOwner, DateTime StartTime, string sMacAddress, ISession MySessionHere)
        {
            ISession MySession = null;
            ITransaction trans = null;

            try
            {
                iTryCount = 0;

            TryAgain:
                int iResult = 0;

                if (MySessionHere == null)
                {
                    MySession = session.GetISession();
                    trans = MySession.BeginTransaction();
                }
                else
                {
                    MySession = MySessionHere;
                }


                //Modified By Selvaraman on 24-Aug-2010
                WorkFlowManager WFManager = new WorkFlowManager();
                ObjectProcessHistoryManager ObjProcHistoryMngr = new ObjectProcessHistoryManager();
                ObjectProcessHistory ObjProcHistoryRecord = new ObjectProcessHistory();

                WFObject WFobj = GetByObjectSystemId(ulObjSystemID, ObjType);

                //To load the Original Version of the WF Object
                WFObject OriginalWFObj = new WFObject();
                OriginalWFObj.Current_Arrival_Time = WFobj.Current_Arrival_Time;
                OriginalWFObj.Current_Owner = WFobj.Current_Owner;
                OriginalWFObj.Current_Process = WFobj.Current_Process;
                OriginalWFObj.Fac_Name = WFobj.Fac_Name;
                OriginalWFObj.Id = WFobj.Id;
                OriginalWFObj.Obj_Sub_Type = WFobj.Obj_Sub_Type;
                OriginalWFObj.Obj_System_Id = WFobj.Obj_System_Id;
                OriginalWFObj.Obj_Type = WFobj.Obj_Type;
                OriginalWFObj.Parent_Obj_System_Id = WFobj.Parent_Obj_System_Id;
                OriginalWFObj.Parent_Obj_Type = WFobj.Parent_Obj_Type;
                OriginalWFObj.Priority = WFobj.Priority;
                OriginalWFObj.Version = WFobj.Version;

                IList<WFObject> OriginalWFList = new List<WFObject>();
                OriginalWFList.Add(OriginalWFObj);

                //Commented by Selvaraman - 20 Nov
                //A process can have multiple with From Process with same close type. So, the process
                //Can not be packpushed by close type. So, updating the table now.
                string PreviousProcess;

                PreviousProcess = WFManager.GetPreviousProcess(WFobj.Fac_Name, WFobj.Obj_Type, WFobj.Obj_Sub_Type, WFobj.Current_Process, iCloseType, WFobj.Doc_Type, WFobj.Doc_Sub_Type);

                //To move the curren object into next process
                WFobj.Is_Default_MyQ_LineItem = 1;
                WFobj.Current_Process = PreviousProcess;
                WFobj.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                WFobj.Current_Owner = sOwner;

                IList<WFObject> wfObjList = new List<WFObject>();
                // IList<WFObject> UpdatewfObjList = null;
                GenerateXml XMLObj = null;
                wfObjList.Add(WFobj);

                IList<WFObject> addList = null;

                //iResult = SaveUpdateDeleteWithoutTransaction(ref addList, wfObjList, null, MySession, sMacAddress);
                //iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref addList, ref UpdatewfObjList, null, MySession, sMacAddress, false, false, 0, string.Empty, ref XMLObj);//Commentted for bugId:53914 
                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref addList, ref wfObjList, null, MySession, sMacAddress, false, false, 0, string.Empty, ref XMLObj);

                if (iResult == 2)
                {
                    if (iTryCount < 5)
                    {
                        iTryCount++;
                        goto TryAgain;
                    }
                    else
                    {
                        if (MySessionHere == null)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Deadlock is occured. Transaction failed");
                        }
                        else
                        {
                            return iResult;
                        }
                    }
                }
                else if (iResult == 1)
                {
                    if (MySessionHere == null)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception is occured. Transaction failed");
                    }
                    else
                    {
                        return iResult;
                    }
                }

                if (MySessionHere == null)
                {
                    MySession.Flush();
                    trans.Commit();
                }
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                if (MySessionHere == null)
                {
                    trans.Rollback();
                    //MySession.Close();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception e)
            {
                if (MySessionHere == null)
                {
                    trans.Rollback();
                    // MySession.Close();
                    throw new Exception(e.Message);
                }
            }
            finally
            {
                if (MySessionHere == null)
                {
                    MySession.Close();
                }
            }
            return 0;
        }

        public int MoveToPreviousProcess(ulong ulObjSystemID, string ObjType, int iCloseType, string sOwner, DateTime StartTime, string sMacAddress)
        {
            //Modified By Selvaraman on 24-Aug-2010
            WorkFlowManager WFManager = new WorkFlowManager();
            ObjectProcessHistoryManager ObjProcHistoryMngr = new ObjectProcessHistoryManager();
            ObjectProcessHistory ObjProcHistoryRecord = new ObjectProcessHistory();

            WFObject WFobj = GetByObjectSystemId(ulObjSystemID, ObjType);

            //To load the Original Version of the WF Object
            WFObject OriginalWFObj = new WFObject();
            OriginalWFObj.Current_Arrival_Time = WFobj.Current_Arrival_Time;
            OriginalWFObj.Current_Owner = WFobj.Current_Owner;
            OriginalWFObj.Current_Process = WFobj.Current_Process;
            OriginalWFObj.Fac_Name = WFobj.Fac_Name;
            OriginalWFObj.Id = WFobj.Id;
            OriginalWFObj.Obj_Sub_Type = WFobj.Obj_Sub_Type;
            OriginalWFObj.Obj_System_Id = WFobj.Obj_System_Id;
            OriginalWFObj.Obj_Type = WFobj.Obj_Type;
            OriginalWFObj.Parent_Obj_System_Id = WFobj.Parent_Obj_System_Id;
            OriginalWFObj.Parent_Obj_Type = WFobj.Parent_Obj_Type;
            OriginalWFObj.Priority = WFobj.Priority;
            OriginalWFObj.Version = WFobj.Version;
            OriginalWFObj.Is_Abnormal = WFobj.Is_Abnormal;

            IList<WFObject> OriginalWFList = new List<WFObject>();
            OriginalWFList.Add(OriginalWFObj);

            //Commented by Selvaraman - 20 Nov
            //A process can have multiple with From Process with same close type. So, the process
            //Can not be packpushed by close type. So, updating the table now.
            string PreviousProcess;

            PreviousProcess = WFManager.GetPreviousProcess(WFobj.Fac_Name, WFobj.Obj_Type, WFobj.Obj_Sub_Type, WFobj.Current_Process, iCloseType, WFobj.Doc_Type, WFobj.Doc_Sub_Type);
            //Jira #CAP-121 - Appointment Empty Current Process is fixed
            if (PreviousProcess == string.Empty)
            {
                return 1;
            }

            //To move the curren object into next process
            WFobj.Is_Default_MyQ_LineItem = 1;
            WFobj.Current_Process = PreviousProcess;
            WFobj.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            WFobj.Current_Owner = sOwner;

            //IList<WFObject> wfObjList = new List<WFObject>();           
            //wfObjList.Add(WFobj);

            IList<WFObject> UpdatewfObjList = new List<WFObject>();
            UpdatewfObjList.Add(WFobj);

            IList<WFObject> addList = null;

            //SaveUpdateDeleteWithTransaction(ref addList, wfObjList, null, sMacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref addList, ref UpdatewfObjList, null, sMacAddress, false, false, 0, string.Empty);
            return 0;
        }
        // Added on 21st Nov 2011
        public int MoveToPreviousProcessForAdmin(ulong ulObjSystemID, string ObjType, string sOwner, string PreviousProcess, DateTime StartTime, string sMacAddress)
        {
            //Modified By Selvaraman on 24-Aug-2010
            WorkFlowManager WFManager = new WorkFlowManager();
            ObjectProcessHistoryManager ObjProcHistoryMngr = new ObjectProcessHistoryManager();
            ObjectProcessHistory ObjProcHistoryRecord = new ObjectProcessHistory();

            WFObject WFobj = GetByObjectSystemId(ulObjSystemID, ObjType);

            //To load the Original Version of the WF Object
            WFObject OriginalWFObj = new WFObject();
            OriginalWFObj.Current_Arrival_Time = WFobj.Current_Arrival_Time;
            OriginalWFObj.Current_Owner = WFobj.Current_Owner;
            OriginalWFObj.Current_Process = WFobj.Current_Process;
            OriginalWFObj.Fac_Name = WFobj.Fac_Name;
            OriginalWFObj.Id = WFobj.Id;
            OriginalWFObj.Obj_Sub_Type = WFobj.Obj_Sub_Type;
            OriginalWFObj.Obj_System_Id = WFobj.Obj_System_Id;
            OriginalWFObj.Obj_Type = WFobj.Obj_Type;
            OriginalWFObj.Parent_Obj_System_Id = WFobj.Parent_Obj_System_Id;
            OriginalWFObj.Parent_Obj_Type = WFobj.Parent_Obj_Type;
            OriginalWFObj.Priority = WFobj.Priority;
            OriginalWFObj.Version = WFobj.Version;

            IList<WFObject> OriginalWFList = new List<WFObject>();
            OriginalWFList.Add(OriginalWFObj);

            //Commented by Selvaraman - 20 Nov
            //A process can have multiple with From Process with same close type. So, the process
            //Can not be packpushed by close type. So, updating the table now.
            //PreviousProcess = WFManager.GetPreviousProcess(WFobj.Fac_Name, WFobj.Obj_Type, WFobj.Obj_Sub_Type, WFobj.Current_Process, iCloseType);

            //To move the curren object into next process
            WFobj.Is_Default_MyQ_LineItem = 1;
            WFobj.Current_Process = PreviousProcess;
            WFobj.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            WFobj.Current_Owner = sOwner;
            if (PreviousProcess == "AKIDO_SCRIBE_PROCESS")
            {
                WFobj.Process_Allocation = WFobj.Process_Allocation + "|" + PreviousProcess + "-" + sOwner;
            }
            IList<WFObject> wfObjList = new List<WFObject>();
            wfObjList.Add(WFobj);

            IList<WFObject> addList = null;

            //SaveUpdateDeleteWithTransaction(ref addList, wfObjList, null, sMacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref addList, ref wfObjList, null, sMacAddress, false, false, 0, string.Empty);
            return 0;
        }

        public void UpdateOwner(ulong ulObjSystemID, string ObjType, string sOwner, string sMacAddress)
        {

            WFObject WFobj = GetByObjectSystemId(ulObjSystemID, ObjType);
            //added by srividhya on 30-May-2015
            WFobj.Is_Default_MyQ_LineItem = 1;
            WFobj.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            WFobj.Current_Owner = sOwner;

            string[] sAlloc = WFobj.Process_Allocation.Split('|');
            string sNewProcessAlloc = string.Empty;
            for (int i = 0; i < sAlloc.Length; i++)
            {
                if (sAlloc[i].StartsWith(WFobj.Current_Process + "-") == true)
                {
                    sNewProcessAlloc = sNewProcessAlloc + "|" + WFobj.Current_Process + "-" + sOwner;
                }
                else if (sAlloc[i] != string.Empty)
                {
                    sNewProcessAlloc = sNewProcessAlloc + "|" + sAlloc[i];
                }
            }
            WFobj.Process_Allocation = sNewProcessAlloc;

            IList<WFObject> wfUpdateList = new List<WFObject>();
            wfUpdateList.Add(WFobj);

            IList<WFObject> wfAddList = null;
            // SaveUpdateDeleteWithTransaction(ref wfAddList, wfUpdateList, null, sMacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref wfAddList, ref wfUpdateList, null, sMacAddress, false, false, 0, string.Empty);
        }

        public void UpdateDocumentation(ulong ulObjSystemID, string ObjType, string sOwner, string sMacAddress)
        {


            WFObject WFobj = GetByObjectSystemId(ulObjSystemID, ObjType);

            //added by srividhya on 30-May-2015
            WFobj.Is_Default_MyQ_LineItem = 1;
            WFobj.Current_Arrival_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            WFobj.Current_Owner = sOwner;

            string[] sAlloc = WFobj.Process_Allocation.Split('|');
            string sNewProcessAlloc = string.Empty;
            for (int i = 0; i < sAlloc.Length; i++)
            {
                if (sAlloc[i].StartsWith(WFobj.Current_Process + "-") == true)
                {
                    sNewProcessAlloc = sNewProcessAlloc + "|" + WFobj.Current_Process + "-" + sOwner;
                }
                else if (sAlloc[i] != string.Empty)
                {
                    sNewProcessAlloc = sNewProcessAlloc + "|" + sAlloc[i];
                }
            }
            WFobj.Process_Allocation = sNewProcessAlloc;

            IList<WFObject> wfUpdateList = new List<WFObject>();
            wfUpdateList.Add(WFobj);

            IList<WFObject> wfAddList = null;
            // SaveUpdateDeleteWithTransaction(ref wfAddList, wfUpdateList, null, sMacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref wfAddList, ref wfUpdateList, null, sMacAddress, false, false, 0, string.Empty);
        }


        public IList<MyQ> GetListofObjects(string FacName, string[] ObjType, string[] ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays)
        {
            IList<MyQ> myqList = new List<MyQ>();

            ObjectManager wfMngr = new ObjectManager();
            myqList = wfMngr.FillMyObjects(FacName, ObjType, ProcessType[0], UserName, bShowAll, DefaultNoofDays);

            return myqList;
        }
        public IList<MyQ> GetListObjects(string FacName, string[] ObjType, string[] ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays, string FacilityName)
        {
            IList<MyQ> myqList = new List<MyQ>();

            ObjectManager wfMngr = new ObjectManager();
            myqList = wfMngr.FillObjects(FacName, ObjType, ProcessType[0], UserName, bShowAll, DefaultNoofDays, FacilityName);

            return myqList;
        }
        public IList<MyQ> GetListObjectsCompleted(string FacName, string[] ObjType, string[] ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays, string FacilityName)
        {
            IList<MyQ> myqList = new List<MyQ>();

            ObjectManager wfMngr = new ObjectManager();
            myqList = wfMngr.FillObjectsCompleted(FacName, ObjType, ProcessType[0], UserName, bShowAll, DefaultNoofDays, FacilityName);

            return myqList;
        }

        public IList<MyQ> GetListObjectsOpenTaskCreatedByMe(string FacName, string[] ObjType, string[] ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays, string FacilityName)
        {
            IList<MyQ> myqList = new List<MyQ>();

            ObjectManager wfMngr = new ObjectManager();
            myqList = wfMngr.FillObjectsOpenTaskCreatedByMe(FacName, ObjType, ProcessType[0], UserName, bShowAll, DefaultNoofDays, FacilityName);

            return myqList;
        }

        public IList<MyQ> GetListofObjectsByProcess(string FacName, string CurrentProcess)
        {
            //Needs to be done

            IList<MyQ> myqList = new List<MyQ>();

            //ArrayList MyList = new ArrayList();

            //IQuery query1 = session.GetISession().GetNamedQuery("FillMyQ.WithFacility.Admin");
            //query1.SetString(0, CurrentProcess);
            //query1.SetString(1, FacName);

            //MyList.AddRange(query1.List());

            //IList<FillMyQ> fillMyQList = new List<FillMyQ>();
            //if (MyList != null)
            //{
            //    for (int i = 0; i < MyList.Count; i++)
            //    {
            //        object[] oj = (object[])MyList[i];

            //        FillMyQ fill = new FillMyQ();
            //        fill.ObjTypeList = oj[0].ToString();
            //        fill.CurrProcessList = oj[1].ToString();
            //        fill.CurrOwner = oj[2].ToString();
            //        fill.CurrObjSubType = oj[3].ToString();
            //        fill.ObjSysId = Convert.ToUInt64(oj[4]);

            //        fillMyQList.Add(fill);
            //    }
            //    ObjectManager wfMngr = new ObjectManager();
            //    myqList = wfMngr.FillMyObjects(fillMyQList);
            //}
            return myqList;
        }

        //public IList<BillingMyQDTO> LoadWfObjectforBilling(string Username)
        //{
        //    IList<BillingMyQDTO> BillingList = new List<BillingMyQDTO>();
        //    BillingMyQDTO objBilling = null;

        //    ISQLQuery sql = session.GetISession().CreateSQLQuery("SELECT * FROM WF_Object W, WorkSet_Proc_Alloc P WHERE  W.curr_process<>'BATCH' AND W.curr_process<>'REDUNDANT' AND  W.curr_process<>'BATCH_COMP' AND W.curr_process<>'BATCH_HOLD' and P.Allocated_To = '" + Username + "' And P.Process_Name = W.Curr_Process And P.DOOS = W.DOOS And P.Batch_Name = W.Batch_Name And P.Doc_Fac_Doc_Number = W.Doc_Fac_Doc_Number ORDER BY P.Process_Name, P.DOOS,P.Batch_Name")
        //       .AddEntity("W", typeof(WFObject))
        //     .AddEntity("P", typeof(Workset_proc_alloc));

        //    foreach (IList<Object> l in sql.List())
        //    {
        //        objBilling = new BillingMyQDTO();
        //        objBilling.Wfobj = (WFObject)l[0];

        //        if (objBilling.Wfobj.Obj_Type.ToUpper() == "CALL")
        //        {
        //            ICriteria crit = session.GetISession().CreateCriteria(typeof(CallLog)).Add(Expression.Eq("Id", objBilling.Wfobj.Obj_System_Id)).AddOrder(Order.Asc("DOOS")).AddOrder(Order.Asc("Batch_Name")).AddOrder(Order.Asc("Doc_Fac_doc_no"));
        //            if (crit.List<CallLog>().Count > 0)
        //            {
        //                CallLog objCall = crit.List<CallLog>()[0];
        //                objBilling.InsName = objCall.Insurance_Name;
        //                objBilling.PageNumber = objCall.Page_Number;
        //            }
        //        }
        //        BillingList.Add(objBilling);
        //    }
        //    return BillingList;
        //}

        public IList<WFObject> GetListofObjectsByCurrentProcess(string CurrentProcess)
        {
            IList<WFObject> WFList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Current_Process", CurrentProcess));
                WFList = crit.List<WFObject>();
                iMySession.Close();
            }

            return WFList;
        }


        public DataSet GetListOfObjectsByFaciltyObjTypeCurrentProcess(string FacilityName, string objType, string currentProcess, int pageNumber, int maxResults, int Limit, string EncouterAndArcCount)
        {
            DataSet ds = new DataSet();
            //Needs to be done
            DataTable dtTable = new DataTable();
            IList<MyQ> myqList = new List<MyQ>();

            ArrayList MyList = new ArrayList();

            //IQuery query1 = session.GetISession().GetNamedQuery("FillOfficeManagerQ.WithFacilityObjTypeCurrentProcess");
            //query1.SetString(0, currentProcess);
            //query1.SetString(1, FacilityName);
            //query1.SetString(2, objType);

            //MyList.AddRange(query1.List());

            //if (MyList != null)
            //{
            //    IList<FillMyQ> fillMyQList = new List<FillMyQ>();
            //    if (MyList != null)
            //    {
            //        for (int i = 0; i < MyList.Count; i++)
            //        {
            //            object[] oj = (object[])MyList[i];

            //            FillMyQ fill = new FillMyQ();
            //            fill.ObjTypeList = oj[0].ToString();
            //            fill.CurrProcessList = oj[1].ToString();
            //            fill.CurrOwner = oj[2].ToString();
            //            fill.CurrObjSubType = oj[3].ToString();
            //            fill.ObjSysId = Convert.ToUInt64(oj[4]);

            //            fillMyQList.Add(fill);
            //        }

            //        ObjectManager wfMngr = new ObjectManager();
            //        myqList = wfMngr.FillMyObjects(FacilityName,objType,ProcType,);
            //    }
            //}
            //if (objType.ToUpper() == "ENCOUNTER" || objType.ToUpper() == "PHONE ENCOUNTER" || objType.ToUpper() == "DOCUMENTATION" || objType.ToUpper() == "DOCUMENT REVIEW")
            //{
            //    dtTable.Columns.Add("Encounter ID");
            //    dtTable.Columns.Add("Date of Service", typeof(DateTime));
            //    dtTable.Columns.Add("Patient Acc#");
            //    dtTable.Columns.Add("Patient Name");
            //    dtTable.Columns.Add("Patient DOB");
            //    dtTable.Columns.Add("Physician Name");
            //    dtTable.Columns.Add("Current Owner");
            //    foreach (MyQ obj in myqList)
            //    {
            //        DataRow dr = dtTable.NewRow();
            //        dr["Encounter ID"] = obj.Encounter_ID;
            //        dr["Date of Service"] = obj.Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm tt");
            //        dr["Patient Acc#"] = obj.Human_ID;
            //        dr["Patient Name"] = obj.Last_Name + "," + obj.First_Name + " " + obj.MI + " " + obj.Sufix;
            //        dr["Patient DOB"] = obj.DOB.ToString("dd-MMM-yyyy");
            //        dr["Physician Name"] = obj.PhyName;
            //        dr["Current Owner"] = obj.Current_Owner;
            //        dtTable.Rows.Add(dr);
            //    }
            //}
            //else if (objType.ToUpper() == "SCAN")
            //{
            //    dtTable.Columns.Add("File Name");
            //    dtTable.Columns.Add("Number of Pages");
            //    dtTable.Columns.Add("Scanned Date");
            //    dtTable.Columns.Add("Scan Type");
            //    foreach (MyQ obj in myqList)
            //    {
            //        DataRow dr = dtTable.NewRow();
            //        dr["File Name"] = obj.Scanned_File_Name;
            //        dr["Number of Pages"] = obj.No_of_Pages;
            //        dr["Scanned Date"] = obj.Scanned_Date.ToString("dd-MMM-yyyy");
            //        dr["Scan Type"] = obj.Scan_Type;
            //        dtTable.Rows.Add(dr);
            //    }

            //}
            //else if (objType.ToUpper() == "DIAGNOSTIC ORDER" || objType.ToUpper() == "IMAGE ORDER")
            //{
            //    dtTable.Columns.Add("Order Submit ID");
            //    dtTable.Columns.Add("Patient Acc#");
            //    dtTable.Columns.Add("Patient Name");
            //    dtTable.Columns.Add("Patient DOB");
            //    dtTable.Columns.Add("Procedure(s) Ordered");
            //    dtTable.Columns.Add("Physician Name");
            //    dtTable.Columns.Add("Current Owner");
            //    foreach (MyQ obj in myqList)
            //    {
            //        DataRow dr = dtTable.NewRow();
            //        dr["Order Submit ID"] = obj.Order_ID;
            //        dr["Patient Acc#"] = obj.Human_ID;
            //        dr["Patient Name"] = obj.Last_Name + "," + obj.First_Name + " " + obj.MI + " " + obj.Sufix;
            //        dr["Patient DOB"] = obj.DOB.ToString("dd-MMM-yyyy");
            //        dr["Procedure(s) Ordered"] = obj.Procedure_Ordered;
            //        dr["Physician Name"] = obj.PhyName;
            //        dr["Current Owner"] = obj.Current_Owner;
            //        dtTable.Rows.Add(dr);
            //    }
            //}
            //else if (objType.ToUpper() == "TASK")
            //{
            //    dtTable.Columns.Add("Patient Acc#");
            //    dtTable.Columns.Add("Patient Name");
            //    dtTable.Columns.Add("Patient DOB");
            //    dtTable.Columns.Add("Message Description");
            //    dtTable.Columns.Add("Assigned To");
            //    //dtTable.Columns.Add("Current Owner");
            //    //dtTable.Columns.Add("Order Submit ID");
            //    foreach (MyQ obj in myqList)
            //    {
            //        DataRow dr = dtTable.NewRow();
            //        dr["Patient Acc#"] = obj.Human_ID;
            //        dr["Patient Name"] = obj.Last_Name + "," + obj.First_Name + " " + obj.MI + " " + obj.Sufix;
            //        dr["Patient DOB"] = obj.DOB.ToString("dd-MMM-yyyy");
            //        dr["Message Description"] = obj.Message_Description;
            //        dr["Assigned To"] = obj.Assigned_To;
            //        //dr["Current Owner"] = obj.Current_Owner;
            //        dtTable.Rows.Add(dr);
            //    }
            //}

            ArrayList MySubList = new ArrayList();
            //string Tablename = string.Empty;
            // Modified by priyangha on 2/1/2013 Bugid:12008
            if (objType.ToUpper() == "ENCOUNTER" || objType.ToUpper() == "PHONE ENCOUNTER" || objType.ToUpper() == "DOCUMENTATION" || objType.ToUpper() == "DOCUMENT REVIEW")
            {
                string TotalNumberOfRecords = string.Empty;
                if (pageNumber == 1)
                {
                    ArrayList EncounterCount = new ArrayList();
                    ArrayList EncounterArcCount = new ArrayList();

                    Limit = 0;
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Encounter.Objects.ForAdmin");
                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);

                    MySubList.AddRange(query2.List());
                    EncounterCount.AddRange(query2.List());


                    ArrayList MySubList1 = new ArrayList();
                    IQuery query3 = session.GetISession().GetNamedQuery("Get.Encounter_Arc.Objects.ForAdmin");
                    query3.SetString(0, objType);
                    query3.SetString(1, FacilityName);
                    query3.SetString(2, currentProcess);

                    MySubList.AddRange(query3.List());
                    EncounterArcCount.AddRange(query3.List());


                    TotalNumberOfRecords = EncounterCount.Count.ToString() + '-' + EncounterArcCount.Count.ToString() + '|' + MySubList.Count.ToString();
                }
                else
                {
                    int TotalRecordsforEncounter = 0;
                    int TotalRecordsforEncounterArc = 0;
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Encounter.Objects.ForAdmin.WithLimit");
                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);

                    //Added By Saravanakumar On 19-02-2014
                    query2.SetInt32(3, ((pageNumber - 1) * maxResults));
                    query2.SetInt32(4, maxResults);
                    MySubList.AddRange(query2.List());
                    TotalRecordsforEncounter = Convert.ToInt32(MySubList.Count);

                    if (TotalRecordsforEncounter < 25)
                    {
                        //if(Tablename==string.Empty)
                        //{
                        //    Tablename = "EncounterArc";
                        //}

                        ArrayList MySubList1 = new ArrayList();
                        TotalRecordsforEncounterArc = 25 - TotalRecordsforEncounter;

                        IQuery query3 = session.GetISession().GetNamedQuery("Get.Encounter_Arc.Objects.ForAdmin.WithLimit");
                        query3.SetString(0, objType);
                        query3.SetString(1, FacilityName);
                        query3.SetString(2, currentProcess);

                        //Added By Saravanakumar On 19-02-2014
                        if (Limit == 0 && TotalRecordsforEncounter > 0)
                        {
                            Limit = TotalRecordsforEncounterArc;
                            query3.SetInt32(3, 0);
                            query3.SetInt32(4, TotalRecordsforEncounterArc);

                        }
                        else
                        {
                            int a = ((pageNumber - 1) * maxResults);
                            int b = 0;
                            int c = 0;
                            //int d = 0;
                            if (EncouterAndArcCount != string.Empty)
                            {
                                b = Convert.ToInt32(EncouterAndArcCount.Split('|')[1]);
                            }
                            c = b - a;
                            Limit = ((Convert.ToInt32(EncouterAndArcCount.Split('-')[1].Split('|')[0])) - c);

                            query3.SetInt32(3, Limit);
                            //Limit = Limit + 25;
                            query3.SetInt32(4, maxResults);
                            ////query3.SetInt32(3, ((pageNumber - 1) * maxResults));


                        }

                        //query3.SetInt32(4, maxResults);
                        MySubList1.AddRange(query3.List());


                        if (TotalRecordsforEncounterArc > 0)
                        {
                            for (int j = 0; j <= MySubList1.Count - 1; j++)
                            {
                                if (j <= TotalRecordsforEncounterArc)
                                {
                                    MySubList.Add(MySubList1[j]);
                                }

                            }
                        }

                    }

                }


                //MySubList.AddRange(query2.List());

                dtTable.Columns.Add("Encounter ID", typeof(int));
                dtTable.Columns.Add("Date of Service", typeof(DateTime));
                dtTable.Columns.Add("Patient Acc#", typeof(int));
                dtTable.Columns.Add("Patient Name");
                dtTable.Columns.Add("Patient DOB", typeof(DateTime));
                dtTable.Columns.Add("Physician Name");
                dtTable.Columns.Add("Current Owner");
                dtTable.Columns.Add("Appointment Date", typeof(DateTime));
                dtTable.Columns.Add("Total No of Record");
                dtTable.Columns.Add("Limit");

                if (pageNumber == 1)
                {
                    for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > MySubList.Count ? MySubList.Count : (maxResults * pageNumber)); i++)
                    {
                        object[] obj = (object[])MySubList[i];

                        DataRow dr = dtTable.NewRow();
                        dr["Encounter ID"] = obj[6].ToString();
                        dr["Date of Service"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[2].ToString();
                        dr["Patient Name"] = obj[3].ToString() + "," + obj[4].ToString() + " " + obj[5].ToString() + " " + obj[14].ToString();
                        dr["Patient DOB"] = obj[11].ToString();
                        dr["Physician Name"] = obj[9].ToString();
                        dr["Current Owner"] = obj[17].ToString();
                        dr["Appointment Date"] = obj[12].ToString();
                        dr["Total No of Record"] = TotalNumberOfRecords;
                        dr["Limit"] = Limit.ToString();
                        dtTable.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < MySubList.Count; i++)
                    {
                        object[] obj = (object[])MySubList[i];

                        DataRow dr = dtTable.NewRow();
                        dr["Encounter ID"] = obj[6].ToString();
                        dr["Date of Service"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[2].ToString();
                        dr["Patient Name"] = obj[3].ToString() + "," + obj[4].ToString() + " " + obj[5].ToString() + " " + obj[14].ToString();
                        dr["Patient DOB"] = obj[11].ToString();
                        dr["Physician Name"] = obj[9].ToString();
                        dr["Current Owner"] = obj[17].ToString();
                        dr["Appointment Date"] = obj[12].ToString();
                        dr["Limit"] = Limit.ToString();
                        dtTable.Rows.Add(dr);
                    }
                }
            }
            else if (objType.ToUpper() == "SCAN")
            {
                MySubList.Clear();
                int TotalNumberOfRecords = 0;
                if (pageNumber == 1)
                {
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Scan.Objects.ForAdmin");
                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    MySubList.AddRange(query2.List());
                    TotalNumberOfRecords = Convert.ToInt32(MySubList.Count);
                }
                else
                {
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Scan.Objects.ForAdmin.WithLimit");
                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    //Added By Saravanakumar On 19-02-2014
                    query2.SetInt32(3, ((pageNumber - 1) * maxResults));
                    query2.SetInt32(4, maxResults);
                    MySubList.AddRange(query2.List());
                }





                dtTable.Columns.Add("Scan ID", typeof(int));
                dtTable.Columns.Add("File Name");
                dtTable.Columns.Add("Number of Pages", typeof(int));
                dtTable.Columns.Add("Scanned Date", typeof(DateTime));
                dtTable.Columns.Add("Scan Type");
                dtTable.Columns.Add("Current Owner");
                dtTable.Columns.Add("Total No of Record", typeof(int));

                if (pageNumber == 1)
                {
                    for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > MySubList.Count ? MySubList.Count : (maxResults * pageNumber)); i++)
                    {
                        object[] obj = (object[])MySubList[i];
                        DataRow dr = dtTable.NewRow();
                        dr["Scan ID"] = obj[9].ToString();
                        dr["File Name"] = obj[0].ToString();
                        dr["Number of Pages"] = obj[1].ToString();
                        dr["Scanned Date"] = Convert.ToDateTime(obj[2]);
                        dr["Scan Type"] = obj[4].ToString();
                        dr["Current Owner"] = obj[6].ToString();
                        dr["Total No of Record"] = MySubList.Count;
                        dtTable.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < MySubList.Count; i++)
                    {
                        object[] obj = (object[])MySubList[i];

                        DataRow dr = dtTable.NewRow();
                        dr["Scan ID"] = obj[9].ToString();
                        dr["File Name"] = obj[0].ToString();
                        dr["Number of Pages"] = obj[1].ToString();
                        dr["Scanned Date"] = Convert.ToDateTime(obj[2]);
                        dr["Scan Type"] = obj[4].ToString();
                        dr["Current Owner"] = obj[6].ToString();
                        dtTable.Rows.Add(dr);
                    }
                }

            }
            else if (objType.ToUpper() == "ADDENDUM")
            {
                MySubList.Clear();
                string TotalNumberOfRecords = string.Empty;
                if (pageNumber == 1)
                {
                    ArrayList EncounterCount = new ArrayList();
                    ArrayList EncounterArcCount = new ArrayList();
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Addendum.Objects.ForAdmin");
                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);

                    MySubList.AddRange(query2.List());
                    EncounterCount.AddRange(query2.List());

                    IQuery query3 = session.GetISession().GetNamedQuery("Get.Addendum_arc.Objects.ForAdmin");
                    query3.SetString(0, objType);
                    query3.SetString(1, FacilityName);
                    query3.SetString(2, currentProcess);



                    MySubList.AddRange(query3.List());
                    EncounterArcCount.AddRange(query3.List());


                    TotalNumberOfRecords = EncounterCount.Count.ToString() + '-' + EncounterArcCount.Count.ToString() + '|' + MySubList.Count.ToString();
                }
                else
                {

                    int TotalRecordsforEncounter = 0;
                    int TotalRecordsforEncounterArc = 0;
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Addendum.Objects.ForAdmin.WithinLimit");
                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);

                    //Added By Saravanakumar On 19-02-2014
                    query2.SetInt32(3, ((pageNumber - 1) * maxResults));
                    query2.SetInt32(4, maxResults);
                    MySubList.AddRange(query2.List());
                    //TotalNumberOfRecords = Convert.ToInt32(MySubList[0]);
                    TotalRecordsforEncounter = Convert.ToInt32(MySubList.Count);

                    if (TotalRecordsforEncounter < 25)
                    {
                        ArrayList MySubList1 = new ArrayList();
                        TotalRecordsforEncounterArc = 25 - TotalRecordsforEncounter;

                        IQuery query3 = session.GetISession().GetNamedQuery("Get.Addendum_arc.Objects.ForAdmin.WithinLimit");
                        query3.SetString(0, objType);
                        query3.SetString(1, FacilityName);
                        query3.SetString(2, currentProcess);



                        if (Limit == 0 && TotalRecordsforEncounter > 0)
                        {
                            Limit = TotalRecordsforEncounterArc;
                            query3.SetInt32(3, 0);
                            query3.SetInt32(4, TotalRecordsforEncounterArc);

                        }
                        else
                        {
                            int a = ((pageNumber - 1) * maxResults);
                            int b = 0;
                            int c = 0;
                            //int d = 0;
                            if (EncouterAndArcCount != string.Empty)
                            {
                                b = Convert.ToInt32(EncouterAndArcCount.Split('|')[1]);
                            }
                            c = b - a;
                            Limit = ((Convert.ToInt32(EncouterAndArcCount.Split('-')[1].Split('|')[0])) - c);

                            query3.SetInt32(3, Limit);
                            //Limit = Limit + 25;
                            query3.SetInt32(4, maxResults);
                        }
                        //Added By Saravanakumar On 19-02-2014
                        //query3.SetInt32(3, ((pageNumber - 1) * maxResults));
                        //query3.SetInt32(4, maxResults);
                        MySubList1.AddRange(query3.List());


                        if (TotalRecordsforEncounterArc > 0)
                        {
                            for (int j = 0; j <= MySubList1.Count - 1; j++)
                            {
                                if (j <= TotalRecordsforEncounterArc)
                                {
                                    MySubList.Add(MySubList1[j]);
                                }

                            }
                        }

                    }
                }


                //MySubList.AddRange(query2.List());

                dtTable.Columns.Add("Encounter ID", typeof(int));
                dtTable.Columns.Add("Date of Service", typeof(DateTime));
                dtTable.Columns.Add("Patient Acc#", typeof(int));
                dtTable.Columns.Add("Patient Name");
                dtTable.Columns.Add("Patient DOB", typeof(DateTime));
                dtTable.Columns.Add("Physician Name");
                dtTable.Columns.Add("Current Owner");
                dtTable.Columns.Add("Appointment Date", typeof(DateTime));
                dtTable.Columns.Add("Total No of Record");
                dtTable.Columns.Add("Limit");

                if (pageNumber == 1)
                {
                    for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > MySubList.Count ? MySubList.Count : (maxResults * pageNumber)); i++)
                    {
                        object[] obj = (object[])MySubList[i];

                        DataRow dr = dtTable.NewRow();
                        dr["Encounter ID"] = obj[6].ToString();
                        dr["Date of Service"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[2].ToString();
                        dr["Patient Name"] = obj[3].ToString() + "," + obj[4].ToString() + " " + obj[5].ToString() + " " + obj[14].ToString();
                        dr["Patient DOB"] = obj[11].ToString();
                        dr["Physician Name"] = obj[9].ToString();
                        dr["Current Owner"] = obj[17].ToString();
                        dr["Appointment Date"] = obj[12].ToString();
                        dr["Total No of Record"] = TotalNumberOfRecords;
                        dr["Limit"] = Limit.ToString();
                        dtTable.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < MySubList.Count; i++)
                    {
                        object[] obj = (object[])MySubList[i];

                        DataRow dr = dtTable.NewRow();
                        dr["Encounter ID"] = obj[6].ToString();
                        dr["Date of Service"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[2].ToString();
                        dr["Patient Name"] = obj[3].ToString() + "," + obj[4].ToString() + " " + obj[5].ToString() + " " + obj[14].ToString();
                        dr["Patient DOB"] = obj[11].ToString();
                        dr["Physician Name"] = obj[9].ToString();
                        dr["Current Owner"] = obj[17].ToString();
                        dr["Appointment Date"] = obj[12].ToString();
                        dr["Limit"] = Limit.ToString();
                        dtTable.Rows.Add(dr);
                    }
                }
            }
            else if (objType.ToUpper() == "DIAGNOSTIC ORDER" || objType.ToUpper() == "IMAGE ORDER")
            {
                MySubList.Clear();
                int TotalNumberOfRecords = 0;
                if (pageNumber == 1)
                {
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Lab.Objects.ForAdmin");

                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    MySubList.AddRange(query2.List());
                    TotalNumberOfRecords = Convert.ToInt32(MySubList.Count);
                }
                else
                {
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Lab.Objects.ForAdmin.WithLimit");
                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    //Added By Saravanakumar On 19-02-2014
                    query2.SetInt32(3, ((pageNumber - 1) * maxResults));
                    query2.SetInt32(4, maxResults);

                    MySubList.AddRange(query2.List());
                }



                dtTable.Columns.Add("Order Submit ID", typeof(int));
                dtTable.Columns.Add("Patient Acc#", typeof(int));
                dtTable.Columns.Add("Patient Name");
                dtTable.Columns.Add("Patient DOB", typeof(DateTime));
                dtTable.Columns.Add("Procedure(s) Ordered");
                dtTable.Columns.Add("Physician Name");
                dtTable.Columns.Add("Current Owner");
                dtTable.Columns.Add("Total No of Record", typeof(int));

                if (pageNumber == 1)
                {
                    for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > MySubList.Count ? MySubList.Count : (maxResults * pageNumber)); i++)
                    {
                        object[] obj = (object[])MySubList[i];
                        DataRow dr = dtTable.NewRow();
                        if (obj[0] == null)
                        {
                            continue;
                        }
                        dr["Order Submit ID"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[4].ToString();
                        dr["Patient Name"] = obj[5].ToString() + "," + obj[6].ToString() + " " + obj[7].ToString() + " " + obj[8].ToString(); ;
                        dr["Patient DOB"] = obj[13].ToString();
                        dr["Procedure(s) Ordered"] = obj[2].ToString();
                        dr["Physician Name"] = obj[9].ToString();
                        dr["Current Owner"] = obj[20].ToString();
                        dr["Total No of Record"] = MySubList.Count;
                        dtTable.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < MySubList.Count; i++)
                    {
                        object[] obj = (object[])MySubList[i];

                        DataRow dr = dtTable.NewRow();
                        if (obj[0] == null)
                        {
                            continue;
                        }
                        dr["Order Submit ID"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[4].ToString();
                        dr["Patient Name"] = obj[5].ToString() + "," + obj[6].ToString() + " " + obj[7].ToString() + " " + obj[8].ToString(); ;
                        dr["Patient DOB"] = obj[13].ToString();
                        dr["Procedure(s) Ordered"] = obj[2].ToString();
                        dr["Physician Name"] = obj[9].ToString();
                        dr["Current Owner"] = obj[20].ToString();
                        dtTable.Rows.Add(dr);
                    }
                }
            }
            else if (objType.ToUpper() == "IMMUNIZATION ORDER")
            {
                MySubList.Clear();
                int TotalNumberOfRecords = 0;
                if (pageNumber == 1)
                {
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Immunization.Objects.ForAdmin");

                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    MySubList.AddRange(query2.List());
                    TotalNumberOfRecords = Convert.ToInt32(MySubList.Count);
                }
                else
                {
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Immunization.Objects.ForAdmin.WithLimit");

                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    //Added By Saravanakumar On 19-02-2014
                    query2.SetInt32(3, ((pageNumber - 1) * maxResults));
                    query2.SetInt32(4, maxResults);
                    MySubList.AddRange(query2.List());
                }


                dtTable.Columns.Add("Immunization Group ID", typeof(int));
                dtTable.Columns.Add("Patient Acc#", typeof(int));
                dtTable.Columns.Add("Patient Name");
                dtTable.Columns.Add("Patient DOB", typeof(DateTime));
                dtTable.Columns.Add("Procedure(s) Ordered");
                dtTable.Columns.Add("Physician Name");
                dtTable.Columns.Add("Current Owner");
                dtTable.Columns.Add("Total No of Record", typeof(int));

                if (pageNumber == 1)
                {
                    for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > MySubList.Count ? MySubList.Count : (maxResults * pageNumber)); i++)
                    {
                        object[] obj = (object[])MySubList[i];
                        DataRow dr = dtTable.NewRow();

                        if (obj[0] == null)
                        {
                            continue;
                        }

                        dr["Immunization Group ID"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[3].ToString();
                        dr["Patient Name"] = obj[4].ToString() + "," + obj[5].ToString() + " " + obj[6].ToString() + " " + obj[7].ToString(); ;
                        dr["Patient DOB"] = obj[10].ToString();
                        dr["Procedure(s) Ordered"] = obj[1].ToString();
                        dr["Physician Name"] = obj[8].ToString();
                        dr["Current Owner"] = obj[15].ToString();
                        dr["Total No of Record"] = MySubList.Count;
                        dtTable.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < MySubList.Count; i++)
                    {
                        object[] obj = (object[])MySubList[i];

                        DataRow dr = dtTable.NewRow();


                        if (obj[0] == null)
                        {
                            continue;
                        }

                        dr["Immunization Group ID"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[3].ToString();
                        dr["Patient Name"] = obj[4].ToString() + "," + obj[5].ToString() + " " + obj[6].ToString() + " " + obj[7].ToString(); ;
                        dr["Patient DOB"] = obj[10].ToString();
                        dr["Procedure(s) Ordered"] = obj[1].ToString();
                        dr["Physician Name"] = obj[8].ToString();
                        dr["Current Owner"] = obj[15].ToString();
                        dtTable.Rows.Add(dr);
                    }
                }
            }
            else if (objType.ToUpper() == "REFERRAL ORDER")
            {
                MySubList.Clear();
                int TotalNumberOfRecords = 0;
                if (pageNumber == 1)
                {
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Referral.Objects.ForAdmin");

                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    MySubList.AddRange(query2.List());
                    TotalNumberOfRecords = Convert.ToInt32(MySubList.Count);
                }
                else
                {

                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Referral.Objects.ForAdmin.WithLimit");

                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    //Added By Saravanakumar On 19-02-2014
                    query2.SetInt32(3, ((pageNumber - 1) * maxResults));
                    query2.SetInt32(4, maxResults);
                    MySubList.AddRange(query2.List());
                }


                dtTable.Columns.Add("Referral Group ID", typeof(int));
                dtTable.Columns.Add("Patient Acc#", typeof(int));
                dtTable.Columns.Add("Patient Name");
                dtTable.Columns.Add("Patient DOB", typeof(DateTime));
                dtTable.Columns.Add("Reason For Referral");
                dtTable.Columns.Add("Physician Name");
                dtTable.Columns.Add("Current Owner");
                dtTable.Columns.Add("Total No of Record", typeof(int));

                if (pageNumber == 1)
                {
                    for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > MySubList.Count ? MySubList.Count : (maxResults * pageNumber)); i++)
                    {
                        object[] obj = (object[])MySubList[i];
                        DataRow dr = dtTable.NewRow();

                        if (obj[0] == null)
                        {
                            continue;
                        }

                        dr["Referral Group ID"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[3].ToString();
                        dr["Patient Name"] = obj[4].ToString() + "," + obj[5].ToString() + " " + obj[6].ToString() + " " + obj[7].ToString(); ;
                        dr["Patient DOB"] = obj[10].ToString();
                        dr["Reason For Referral"] = obj[1].ToString();
                        dr["Physician Name"] = obj[8].ToString();
                        dr["Current Owner"] = obj[15].ToString();
                        dr["Total No of Record"] = MySubList.Count;
                        dtTable.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < MySubList.Count; i++)
                    {
                        object[] obj = (object[])MySubList[i];

                        DataRow dr = dtTable.NewRow();

                        if (obj[0] == null)
                        {
                            continue;
                        }

                        dr["Referral Group ID"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[3].ToString();
                        dr["Patient Name"] = obj[4].ToString() + "," + obj[5].ToString() + " " + obj[6].ToString() + " " + obj[7].ToString(); ;
                        dr["Patient DOB"] = obj[10].ToString();
                        dr["Reason For Referral"] = obj[1].ToString();
                        dr["Physician Name"] = obj[8].ToString();
                        dr["Current Owner"] = obj[15].ToString();
                        dtTable.Rows.Add(dr);
                    }
                }
            }
            /* For Bug Id 54510
                        else if (objType.ToUpper() == "INTERNAL ORDER")
                        {
                            MySubList.Clear();
                            int TotalNumberOfRecords = 0;
                            if (pageNumber == 1)
                            {
                                IQuery query2 = session.GetISession().GetNamedQuery("Get.InHouseProcedure.Objects.ForAdmin");

                                query2.SetString(0, objType);
                                query2.SetString(1, FacilityName);
                                query2.SetString(2, currentProcess);
                                MySubList.AddRange(query2.List());
                                TotalNumberOfRecords = Convert.ToInt32(MySubList.Count);
                            }
                            else
                            {
                                IQuery query2 = session.GetISession().GetNamedQuery("Get.InHouseProcedure.Objects.ForAdmin.WithLimit");

                                query2.SetString(0, objType);
                                query2.SetString(1, FacilityName);
                                query2.SetString(2, currentProcess);
                                //Added By Saravanakumar On 19-02-2014
                                query2.SetInt32(3, ((pageNumber - 1) * maxResults));
                                query2.SetInt32(4, maxResults);


                                MySubList.AddRange(query2.List());
                            }
                            dtTable.Columns.Add("In House Procedure Group_ID", typeof(int));
                            dtTable.Columns.Add("Patient Acc#", typeof(int));
                            dtTable.Columns.Add("Patient Name");
                            dtTable.Columns.Add("Patient DOB", typeof(DateTime));
                            dtTable.Columns.Add("Procedure(s) Ordered");
                            dtTable.Columns.Add("Physician Name");
                            dtTable.Columns.Add("Current Owner");
                            dtTable.Columns.Add("Total No of Record", typeof(int));

                            if (pageNumber == 1)
                            {
                                for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > MySubList.Count ? MySubList.Count : (maxResults * pageNumber)); i++)
                                {
                                    object[] obj = (object[])MySubList[i];
                                    DataRow dr = dtTable.NewRow();
                                    dr["In House Procedure Group_ID"] = obj[0].ToString();
                                    dr["Patient Acc#"] = obj[3].ToString();
                                    dr["Patient Name"] = obj[4].ToString() + "," + obj[5].ToString() + " " + obj[6].ToString() + " " + obj[7].ToString(); ;
                                    dr["Patient DOB"] = obj[10].ToString();
                                    dr["Procedure(s) Ordered"] = obj[1].ToString();
                                    dr["Physician Name"] = obj[8].ToString();
                                    dr["Current Owner"] = obj[15].ToString();
                                    dr["Total No of Record"] = MySubList.Count;
                                    dtTable.Rows.Add(dr);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < MySubList.Count; i++)
                                {
                                    object[] obj = (object[])MySubList[i];

                                    DataRow dr = dtTable.NewRow();
                                    dr["In House Procedure Group_ID"] = obj[0].ToString();
                                    dr["Patient Acc#"] = obj[3].ToString();
                                    dr["Patient Name"] = obj[4].ToString() + "," + obj[5].ToString() + " " + obj[6].ToString() + " " + obj[7].ToString(); ;
                                    dr["Patient DOB"] = obj[10].ToString();
                                    dr["Procedure(s) Ordered"] = obj[1].ToString();
                                    dr["Physician Name"] = obj[8].ToString();
                                    dr["Current Owner"] = obj[15].ToString();
                                    dtTable.Rows.Add(dr);
                                }
                            }
                        }
                            */
            else if (objType.ToUpper() == "TASK")
            {
                MySubList.Clear();
                int TotalNumberOfRecords = 0;
                if (pageNumber == 1)
                {
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Task.Objects.ForAdmin");

                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    MySubList.AddRange(query2.List());
                    TotalNumberOfRecords = Convert.ToInt32(MySubList.Count);
                }
                else
                {
                    IQuery query2 = session.GetISession().GetNamedQuery("Get.Task.Objects.ForAdmin");

                    query2.SetString(0, objType);
                    query2.SetString(1, FacilityName);
                    query2.SetString(2, currentProcess);
                    //Added By Saravanakumar On 19-02-2014
                    query2.SetInt32(3, ((pageNumber - 1) * maxResults));
                    query2.SetInt32(4, maxResults);

                    MySubList.AddRange(query2.List());
                }

                dtTable.Columns.Add("Message ID", typeof(int));
                dtTable.Columns.Add("Patient Acc#", typeof(int));
                dtTable.Columns.Add("Patient Name");
                dtTable.Columns.Add("Patient DOB", typeof(DateTime));
                dtTable.Columns.Add("Message Description");
                dtTable.Columns.Add("Assigned To");
                dtTable.Columns.Add("Current Owner");
                dtTable.Columns.Add("Total No of Record", typeof(int));
                //dtTable.Columns.Add("Current Owner");
                //dtTable.Columns.Add("Order Submit ID");
                if (pageNumber == 1)
                {
                    for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > MySubList.Count ? MySubList.Count : (maxResults * pageNumber)); i++)
                    {
                        object[] obj = (object[])MySubList[i];
                        DataRow dr = dtTable.NewRow();
                        dr["Message ID"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[1].ToString();
                        dr["Patient Name"] = obj[15].ToString() + "," + obj[16].ToString() + " " + obj[17].ToString() + " " + obj[18].ToString(); ;
                        dr["Patient DOB"] = obj[12].ToString();
                        dr["Message Description"] = obj[2].ToString();
                        dr["Assigned To"] = obj[3].ToString();
                        dr["Current Owner"] = obj[19].ToString();
                        dr["Total No of Record"] = MySubList.Count;
                        dtTable.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < MySubList.Count; i++)
                    {
                        object[] obj = (object[])MySubList[i];
                        DataRow dr = dtTable.NewRow();
                        dr["Message ID"] = obj[0].ToString();
                        dr["Patient Acc#"] = obj[1].ToString();
                        dr["Patient Name"] = obj[15].ToString() + "," + obj[16].ToString() + " " + obj[17].ToString() + " " + obj[18].ToString(); ;
                        dr["Patient DOB"] = obj[12].ToString();
                        dr["Message Description"] = obj[2].ToString();
                        dr["Assigned To"] = obj[3].ToString();
                        dr["Current Owner"] = obj[19].ToString();
                        //dr["Current Owner"] = obj.Current_Owner;
                        dtTable.Rows.Add(dr);
                    }
                }
            }
            ds.Tables.Add(dtTable);

            return ds;
        }
        public Stream LoadMyQ(string GenQFacilityName, string[] GenQObjType, string[] GenQProcessType, string GenQUserName, string MyQFacilityName, string[] MyQObjType, string[] MyQProcessType, string MyQUserName, Boolean bShowAllGeneral, Boolean bShowAllMyQ, int DefaultNoofDays)
        {


            //Srividhya added on 30-May-2015==START

            IList<MyQ> GeneralQ = new List<MyQ>();
            IList<MyQ> MyQLst = new List<MyQ>();
            GeneralQ = GetListofGeneralAndMyQ(GenQFacilityName, GenQObjType, GenQProcessType, GenQUserName, bShowAllGeneral, DefaultNoofDays);

            //if (bShowAllMyQ == false) //General Q or MyQ showall unchecked//bShowAllGeneral == false || 
            //{
            //    MyQLst = GeneralQ;
            //}
            //else
            //{
            //    log.Info("before GetListofGeneralAndMyQ Method call start");
            //    MyQLst = GetListofGeneralAndMyQ(MyQFacilityName, MyQObjType, MyQProcessType, MyQUserName, bShowAllMyQ, DefaultNoofDays);
            //    log.Info("before GetListofGeneralAndMyQ Method call end");
            //} 

            Hashtable ht = new Hashtable();
            //if (GeneralQ.Count > 0)
            //    ht.Add("GeneralQ", GeneralQ);
            //if (MyQLst.Count>0)
            //    ht.Add("DashBoardQ", MyQLst);
            if (GeneralQ.Count > 0)
            {
                var GenQ = from g in GeneralQ where g.Current_Owner == "UNKNOWN" && g.Facility_Name == GenQFacilityName select g;
                ht.Add("GeneralQ", GenQ.ToList<MyQ>());

                var MyQ = from g in GeneralQ where g.Current_Owner == MyQUserName select g;
                ht.Add("DashBoardQ", MyQ.ToList<MyQ>());
            }
            //if (GeneralQ.Count > 0)
            //{
            //    var MyQ = from g in GeneralQ where g.Current_Owner == MyQUserName select g;   
            //    ht.Add("DashBoardQ", MyQ.ToList<MyQ>());
            //}

            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();

            serializer.WriteObject(stream, ht);
            stream.Seek(0L, SeekOrigin.Begin);
            return stream;
            //Srividhya added on 30-May-2015==END
        }

        //StartSarvanan 18-06-2015
        public Hashtable LoadMyQUsingHashTable(string GenQFacilityName, string[] GenQObjType, string[] GenQProcessType, string GenQUserName, string MyQFacilityName, string[] MyQObjType, string[] MyQProcessType, string MyQUserName, Boolean bShowAllGeneral, Boolean bShowAllMyQ, int DefaultNoofDays)
        {

            IList<MyQ> GeneralQ = new List<MyQ>();
            IList<MyQ> MyQLst = new List<MyQ>();


            GeneralQ = GetListofGeneralAndMyQ(GenQFacilityName, GenQObjType, GenQProcessType, GenQUserName, bShowAllGeneral, DefaultNoofDays);

            string[] ProcessType = new string[2];
            ProcessType[0] = "UNASSIGNED";
            ProcessType[1] = "ASSIGNED";

            string[] ObjType = new string[11];
            ObjType[0] = "DIAGNOSTIC ORDER";
            //ObjType[1] = "IMAGE ORDER";
            ObjType[1] = "INTERNAL ORDER";
            ObjType[2] = "IMMUNIZATION ORDER";
            ObjType[3] = "REFERRAL ORDER";
            //ObjType[4] = "SCAN";
            //ObjType[5] = "SCAN RESULT";
            //ObjType[6] = "E-PRESCRIBE";
            //ObjType[7] = "ADDENDUM";
            //ObjType[8] = "DICTATION_RESULT";
            //ObjType[9] = "DIAGNOSTIC_RESULT";
            //ObjType[10] = "TASK";
            ObjectManager wfMngr = new ObjectManager();
            IList<MyQueueCountDTO> GenQCount = new List<MyQueueCountDTO>();
            IList<MyQueueCountDTO> lstGeneralQ = new List<MyQueueCountDTO>();
            IList<MyQueueCountDTO> lstMyQLst = new List<MyQueueCountDTO>();
            MyQueueCountDTO objGeneralQ = new MyQueueCountDTO();
            MyQueueCountDTO objMyQLst = new MyQueueCountDTO();

            GenQCount = wfMngr.FillCountObjects(GenQFacilityName, ObjType, GenQUserName, DefaultNoofDays);
            Hashtable ht = new Hashtable();
            if (GenQCount.Count > 0)
            {
                ht.Add("GenQCount", GenQCount);
            }

            if (GeneralQ.Count > 0)
            {
                var GenQ = from g in GeneralQ where g.Current_Owner == "UNKNOWN" && g.Facility_Name == GenQFacilityName select g;
                objGeneralQ.Queue = GenQ.ToList<MyQ>();
                lstGeneralQ.Add(objGeneralQ);
                //  ht.Add("GeneralQ", GenQ.ToList<MyQ>());
                ht.Add("GeneralQ", lstGeneralQ[0].Queue);

                var MyQ = from g in GeneralQ where g.Current_Owner == MyQUserName select g;
                objMyQLst.Queue = MyQ.ToList<MyQ>();
                lstMyQLst.Add(objMyQLst);
                //  ht.Add("DashBoardQ", MyQ.ToList<MyQ>());
                ht.Add("DashBoardQ", lstMyQLst[0].Queue);
            }
            else
            {
                IList<MyQ> listGeneralQ = new List<MyQ>();
                IList<MyQ> listMyQLst = new List<MyQ>();
                ht.Add("GeneralQ", listGeneralQ);
                ht.Add("DashBoardQ", listMyQLst);

            }
            return ht;

        }
     
        public Hashtable LoadMyQHashTable(string FacName, string[] ObjectType, string[] ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays, string facilityName)
        {


            IList<MyQ> GeneralQ = new List<MyQ>();
            IList<MyQ> MyQLst = new List<MyQ>();
            IList<MyQ> myqList = new List<MyQ>();

            ObjectManager objMngr = new ObjectManager();
            myqList = objMngr.FillObjects(FacName, ObjectType, ProcessType[0], UserName, bShowAll, DefaultNoofDays, facilityName);


            string[] ObjType = new string[11];

            if (ProcessType[0] == "ASSIGNED")
            {
                ObjType[0] = "DIAGNOSTIC ORDER";
                //ObjType[1] = "INTERNAL ORDER";
                ObjType[1] = "IMMUNIZATION ORDER";
                ObjType[2] = "REFERRAL ORDER";
                ObjType[3] = "SCAN";
                ObjType[4] = "E-PRESCRIBE";
                ObjType[5] = "ADDENDUM";
                ObjType[6] = "DIAGNOSTIC_RESULT";
                ObjType[7] = "TASK";
                ObjType[8] = "DME ORDER";

            }
            else
            {
                ObjType[0] = "DIAGNOSTIC ORDER";
                //  ObjType[1] = "INTERNAL ORDER";
                ObjType[1] = "IMMUNIZATION ORDER";
                ObjType[2] = "REFERRAL ORDER";
                ObjType[3] = "ADDENDUM";
                ObjType[4] = "DIAGNOSTIC_RESULT";
                ObjType[5] = "DME ORDER";
                ObjType[6] = "TASK";


            }

            IList<MyQueueCountDTO> GenQCount = new List<MyQueueCountDTO>();
            IList<MyQueueCountDTO> lstGeneralQ = new List<MyQueueCountDTO>();
            IList<MyQueueCountDTO> lstMyQLst = new List<MyQueueCountDTO>();
            MyQueueCountDTO objGeneralQ = new MyQueueCountDTO();
            MyQueueCountDTO objMyQLst = new MyQueueCountDTO();
            //ulong encountercount=0;


            string[] ObjTypeshowall = new string[4];

            if (ProcessType[0] == "ASSIGNED")
            {
                ObjTypeshowall[0] = "ENCOUNTER";
                ObjTypeshowall[1] = "DOCUMENTATION";
                //ObjTypeshowall[2] = "PHONE ENCOUNTER";
                ObjTypeshowall[2] = "DOCUMENT REVIEW";
            }
            else
            {
                ObjTypeshowall[0] = "ENCOUNTER";
                ObjTypeshowall[1] = "DOCUMENTATION";
                // ObjTypeshowall[2]   ="PHONE ENCOUNTER";
            }
            ulong encountercount = 0;
            encountercount = objMngr.GetEncountershowallcount(ProcessType[0], UserName, ObjTypeshowall, FacName);
            GenQCount = objMngr.ObjectCount(FacName, ObjType, UserName, DefaultNoofDays);
            Hashtable ht = new Hashtable();
            if (myqList.Count > 0)
            {
                ht.Add("MyQ", myqList);
            }
            if (GenQCount.Count > 0)
            {
                ht.Add("Qcount", GenQCount);
            }
            ht.Add("EncounterShowallCount", encountercount);
            return ht;

        }

        public IList<MyQ> GetListofGeneralAndMyQ(string FacName, string[] ObjType, string[] ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays)
        {
            IList<MyQ> myqList = new List<MyQ>();
            ObjectManager wfMngr = new ObjectManager();
            myqList = wfMngr.FillMyObjects(FacName, ObjType, ProcessType[0], UserName, bShowAll, DefaultNoofDays);
            return myqList;
        }
        // Added By Manimozhi On jan 24th 2012 - For Move To MA- Physician Can Back Push the Encounter To MA

        public void updateOwnerForDictationException(string FileName, string sObjType, string sOwner, string MACAddress)
        {

            if (FileName != string.Empty)
            {
                ulong Enc_Id = 0;
                IList<DictationException> ResultList = new List<DictationException>();
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    ICriteria criteria = iMySession.CreateCriteria(typeof(DictationException)).Add(Expression.Eq("File_Name", FileName)).Add(Expression.Eq("Encounter_ID", Enc_Id)).Add(Expression.Eq("Data", "Result Mismatch"));
                    ResultList = criteria.List<DictationException>();
                    iMySession.Close();
                }

                if (ResultList != null)
                {
                    for (int i = 0; i < ResultList.Count; i++)

                        UpdateOwner(ResultList[i].Id, sObjType, sOwner, MACAddress);
                }
            }
        }
        public IList<WFObject> GetListofObjectsByObySystemIdforScan(ulong uObjSystemId)
        {

            WFObject ResultWFObj = new WFObject();
            IList<WFObject> listWFobject = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", uObjSystemId))
              .Add(Expression.Eq("Obj_Type", "SCAN"));
                listWFobject = criteria.List<WFObject>();
                iMySession.Close();
            }
            return listWFobject;
        }

        public void DeleteDocumentationObject(WFObject DeleteWFObject)
        {
            IList<WFObject> addWfbjectList = null;
            IList<WFObject> updateWfbjectList = null;
            IList<WFObject> deletewfobjectList = new List<WFObject>();
            deletewfobjectList.Add(DeleteWFObject);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref addWfbjectList, ref updateWfbjectList, deletewfobjectList, string.Empty, false, false, 0, string.Empty);
        }

        public WFObject GetByObjectSystemIDandCurrentProcess(ulong uObjSystemId, string sObjType)
        {
            IList<WFObject> wfObjectList = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select w.* from wf_object w left join process_master m on(w.current_process=m.process_name) where w.obj_system_id=" + uObjSystemId + " and w.Obj_Type='" + sObjType + "' and m.Is_Deleted_Process<>'Y'")
                       .AddEntity("w.*", typeof(WFObject));
                wfObjectList = sql.List<WFObject>();
                iMySession.Close();
            }
            WFObject WFobj = new WFObject();
            if (wfObjectList != null && wfObjectList.Count > 0)
            {
                WFobj = wfObjectList[0];//  GetByObjectSystemId(uObjSystemId, ObjType);
            }
            return WFobj;
        }



        public IList<WFObject> GetDocumentationObject(ulong[] uObjSystemId)
        {
            IList<WFObject> lstWfObj = new List<WFObject>();
            ArrayList arryList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Get.DocumentaionList");
                query1.SetParameterList("ObjSystemId", uObjSystemId);
                arryList = new ArrayList(query1.List());
                if (arryList != null)
                {
                    for (int i = 0; i < arryList.Count; i++)
                    {
                        WFObject objWF = new WFObject(); ;
                        object[] oj = (object[])arryList[i];
                        objWF.Obj_System_Id = Convert.ToUInt64(oj[0].ToString());
                        objWF.Current_Process = oj[1].ToString();
                        lstWfObj.Add(objWF);
                    }

                }
                iMySession.Close();
            }
            return lstWfObj;
        }

        public int GetLabResultsWaiting(string FacName, string[] ObjType, string[] ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays)
        {

            int Orderscount = 0;
            IQuery query1 = null;
            ArrayList FavoriteList = null;
            //ISession Mysession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession Mysession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (ObjType.Contains("DIAGNOSTIC ORDER") || ObjType.Contains("IMAGE ORDER"))
                {
                    string[] myObjType = new string[2];
                    myObjType[0] = "DIAGNOSTIC ORDER";
                    myObjType[1] = "IMAGE ORDER";
                    StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                    if (bShowAll == true)
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithoutFacility");
                        }
                        else
                        {
                            if (objStaticLookupMgr.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.Trim().ToUpper() != FacName)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility");
                                query1.SetString(2, FacName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.CMG");
                            }
                        }
                    }
                    else
                    {

                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithoutFacility.ShowAllFalse");
                            query1.SetInt32(2, DefaultNoofDays);
                        }
                        else
                        {
                            if (objStaticLookupMgr.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.Trim().ToUpper() != FacName)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.ShowAllFalse");
                                query1.SetString(2, FacName);
                                query1.SetInt32(3, DefaultNoofDays);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.ShowAllFalse.CMG");
                                query1.SetInt32(2, DefaultNoofDays);
                            }
                        }
                    }
                    query1.SetString(0, UserName);
                    if (ProcessType[0].ToString() == "UNASSIGNED")
                    {
                        query1.SetString(1, "UNKNOWN");
                    }
                    else
                    {
                        query1.SetString(1, UserName);
                    }
                    query1.SetParameterList("ObjList", myObjType);
                    FavoriteList = new ArrayList(query1.List());

                }
                Orderscount = FavoriteList.Count;
                /* For Bug Id 54510
                if (ObjType.Contains("INTERNAL ORDER"))
                {
                    string[] myObjType = new string[1];
                    myObjType[0] = "INTERNAL ORDER";
                    if (bShowAll == true)
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithoutFacility");
                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithFacility");
                            query1.SetString(2, FacName);
                        }
                    }
                    else
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithoutFacility.ShowAllFalse");
                            query1.SetInt32(2, DefaultNoofDays);
                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithFacility.ShowAllFalse");
                            query1.SetString(2, FacName);
                            query1.SetInt32(3, DefaultNoofDays);
                        }
                    }
                    query1.SetString(0, UserName);
                    if (ProcessType[0].ToString() == "UNASSIGNED")
                    {
                        query1.SetString(1, "UNKNOWN");
                    }
                    else
                    {
                        query1.SetString(1, UserName);
                    }
                    query1.SetParameterList("ObjList", myObjType);
                    FavoriteList = new ArrayList(query1.List());

                }*/
                Orderscount = Orderscount + FavoriteList.Count;
                if (ObjType.Contains("IMMUNIZATION ORDER"))
                {
                    string[] myObjType = new string[1];
                    myObjType[0] = "IMMUNIZATION ORDER";
                    if (bShowAll == true)
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithoutFacility");
                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithFacility");
                            query1.SetString(2, FacName);
                        }
                    }
                    else
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithoutFacility.ShowAllFalse");
                            query1.SetInt32(2, DefaultNoofDays);
                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithFacility.ShowAllFalse");
                            query1.SetString(2, FacName);
                            query1.SetInt32(3, DefaultNoofDays);
                        }
                    }
                    query1.SetString(0, UserName);
                    if (ProcessType[0].ToString() == "UNASSIGNED")
                    {
                        query1.SetString(1, "UNKNOWN");
                    }
                    else
                    {
                        query1.SetString(1, UserName);
                    }
                    query1.SetParameterList("ObjList", myObjType);
                    FavoriteList = new ArrayList(query1.List());

                }
                Orderscount = Orderscount + FavoriteList.Count;
                if (ObjType.Contains("REFERRAL ORDER"))
                {
                    string[] myObjType = new string[1];
                    myObjType[0] = "REFERRAL ORDER";
                    if (bShowAll == true)
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithoutFacility");
                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithFacility");
                            query1.SetString(2, FacName);
                        }
                    }
                    else
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithoutFacility.ShowAllFalse");
                            query1.SetInt32(2, DefaultNoofDays);
                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithFacility.ShowAllFalse");
                            query1.SetString(2, FacName);
                            query1.SetInt32(3, DefaultNoofDays);
                        }
                    }
                    query1.SetString(0, UserName);
                    if (ProcessType[0].ToString() == "UNASSIGNED")
                    {
                        query1.SetString(1, "UNKNOWN");
                    }
                    else
                    {
                        query1.SetString(1, UserName);
                    }
                    query1.SetParameterList("ObjList", myObjType);
                    FavoriteList = new ArrayList(query1.List());

                }
                Orderscount = Orderscount + FavoriteList.Count;
                //ObjectManager wfMngr = new ObjectManager();
                //myqList = wfMngr.FillMyObjects(FacName, ObjType, ProcessType[0], UserName, bShowAll, DefaultNoofDays);

                Mysession.Close();
            }
            return Orderscount;
        }

        //Added by Saravanakumar
        public DataSet GetPatientCurrentProcess(ulong HumanID, string sdob, string sPatientName)
        {
            DataTable dtTable = new DataTable();
            DataSet ds = new DataSet();
            ArrayList MyList = new ArrayList();
            IList<string> sObjType = new List<string> { "ENCOUNTER", "DOCUMENTATION", "DOCUMENT REVIEW" };
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.HumanCurrentProcess");
                query.SetString("HumanID", HumanID.ToString());
                query.SetParameterList("ObjType", sObjType.ToList());
                MyList = new ArrayList(query.List());
                iMySession.Close();

            }
            dtTable.Columns.Add("Encounter ID", typeof(int));
            dtTable.Columns.Add("Date of Service", typeof(DateTime));
            dtTable.Columns.Add("Patient Acc#", typeof(int));
            dtTable.Columns.Add("Patient Name");
            dtTable.Columns.Add("Patient DOB", typeof(DateTime));
            //dtTable.Columns.Add("Physician Name");
            dtTable.Columns.Add("Appointment Provider Name");
            dtTable.Columns.Add("Encounter Provider Name");
            dtTable.Columns.Add("Current Process");
            dtTable.Columns.Add("Current Owner");
            dtTable.Columns.Add("Appointment Date", typeof(DateTime));
            dtTable.Columns.Add("Object Type");
            dtTable.Columns.Add("MA Name");
            dtTable.Columns.Add("Batch Status");
            dtTable.Columns.Add("Facility Name");

            if (MyList.Count > 0)
            {
                for (int i = 0; i < MyList.Count; i++)
                {
                    object[] obj = (object[])MyList[i];

                    DataRow dr = dtTable.NewRow();
                    dr["Encounter ID"] = Convert.ToUInt32(obj[15].ToString());
                    dr["Date of Service"] = Convert.ToDateTime(obj[17]).ToString("dd-MMM-yyyy hh:mm tt");
                    dr["Patient Acc#"] = HumanID;
                    dr["Patient Name"] = sPatientName;
                    dr["Patient DOB"] = Convert.ToDateTime(sdob);
                    //dr["Physician Name"] = obj[18].ToString();
                    dr["Appointment Provider Name"] = obj[18].ToString();
                    if (obj[19] != null)//for bug id 54525 
                        dr["Encounter Provider Name"] = obj[19].ToString();
                    dr["Current Process"] = obj[5].ToString();
                    dr["Current Owner"] = obj[7].ToString();
                    dr["Appointment Date"] = Convert.ToDateTime(obj[16]);
                    dr["Object Type"] = obj[2].ToString();
                    dr["MA Name"] = obj[20].ToString();
                    dr["Batch Status"] = obj[21].ToString();
                    dr["Facility Name"] = obj[4].ToString();
                    dtTable.Rows.Add(dr);
                }
                ds.Tables.Add(dtTable);
                var aa = (from p in dtTable.AsEnumerable() select p.Field<int>("Encounter ID")).Distinct();

                DataTable dtNew = new DataTable();

                dtNew.Columns.Add("Encounter ID", typeof(int));
                dtNew.Columns.Add("Date of Service", typeof(DateTime));
                dtNew.Columns.Add("Patient Acc#", typeof(int));
                dtNew.Columns.Add("Patient Name");
                dtNew.Columns.Add("Patient DOB", typeof(DateTime));
                //dtNew.Columns.Add("Physician Name");
                dtNew.Columns.Add("Appointment Provider Name");
                dtNew.Columns.Add("Encounter Provider Name");
                dtNew.Columns.Add("Current Process");
                dtNew.Columns.Add("Current Owner");
                dtNew.Columns.Add("Appointment Date", typeof(DateTime));
                dtNew.Columns.Add("Object Type");
                dtNew.Columns.Add("MA Name");
                dtNew.Columns.Add("Batch Status");
                dtNew.Columns.Add("Facility Name");

                foreach (int val in aa)
                {
                    var cc = (from p in dtTable.AsEnumerable() where p.Field<int>("Encounter ID") == val && p.Field<string>("Object Type") != "ENCOUNTER" && p.Field<string>("Current Process") != "PROVIDER_PROCESS_WAIT" && p.Field<string>("Current Process") != "SCRIBE_PROCESS_WAIT" select p);
                    if (cc.Count() == 0)
                    {
                        var cc1 = (from p in dtTable.AsEnumerable() where p.Field<int>("Encounter ID") == val && p.Field<string>("Object Type") == "ENCOUNTER" select p);
                        for (int i = 0; i < cc1.Count(); i++)
                        {
                            dtNew.Rows.Add(((DataRow)cc1.ToArray()[i]).ItemArray);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cc.Count(); i++)
                        {
                            dtNew.Rows.Add(((DataRow)cc.ToArray()[i]).ItemArray);
                        }

                    }
                }
                ds = new DataSet();
                ds.Tables.Add(dtNew);
            }
            return ds;

        }
        //Added by Suvarnni
        public void saveListwfobj(IList<WFObject> ilist)
        {
            //  SaveUpdateDeleteWithTransaction(ref ilist, null, null, string.Empty);
            IList<WFObject> Updatelist = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ilist, ref Updatelist, null, string.Empty, false, false, 0, string.Empty);
        }

        public IList<WFObject> LoadUpdatewfobj(IList<WFObject> updateList, String operation)
        {
            IList<WFObject> returnList = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (operation == "Load")
                {
                    ICriteria crit = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Is_Default_MyQ", 1));
                    returnList = crit.List<WFObject>();
                    iMySession.Close();
                }
            }
            if (operation == "Update")
            {
                IList<WFObject> emptySaveList = new List<WFObject>();
                //SaveUpdateDeleteWithTransaction(ref emptySaveList, updateList, null, string.Empty);
                SaveUpdateDelete_DBAndXML_WithTransaction(ref emptySaveList, ref updateList, null, string.Empty, false, false, 0, string.Empty);
            }
            return returnList;

        }

        public IList<Encounter> GetEncounterDetailsByIsProgressNoteGenerate()
        {
            IList<Encounter> EncList = new List<Encounter>();
            Encounter EncRecord = new Encounter();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select E.* from wf_object W, encounter E where W.current_process='REVIEW_CODING' and W.obj_type='DOCUMENTATION' and W.obj_system_id=E.encounter_id and E.Is_ProgressNote_Generated='N'")
                 .AddEntity("E", typeof(Encounter));

                foreach (Object l in sql.List())
                {
                    EncRecord = new Encounter();
                    EncRecord = (Encounter)l;
                    EncList.Add(EncRecord);
                }
                iMySession.Close();
            }
            return EncList;
        }

        public bool IsPreviousEncounterPhysicianProcess(ulong ulEncounterID)
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria result = iMySession.CreateCriteria(typeof(WFObject))
                                  .Add(Expression.Eq("Obj_System_Id", ulEncounterID))
                                  .Add(Expression.Eq("Obj_Type", "DOCUMENTATION"));

                IList<WFObject> lstObjectProcessHistory = result.List<WFObject>();

                iMySession.Close();

                if (lstObjectProcessHistory != null &&
                    lstObjectProcessHistory.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsPreviousEncounterPhysicianProcess(ulong ulEncounterID, bool isFromArchive)
        {
            bool Is_physicain_process = false;

            //  IList<WFObject> lstObjectProcessHistory;//For Bug ID 61690 
            IList<Encounter> lstObjectProcessHistory;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //For Bug ID 61690 
                /* var querySQL = string.Format(@"SELECT E.*
                                                FROM   {0} E 
                                                WHERE  E.OBJ_SYSTEM_ID = :OBJ_SYSTEM_ID 
                                                AND    E.OBJ_TYPE = :OBJ_TYPE", isFromArchive ? "WF_OBJECT_ARC" : "WF_OBJECT");

                 var SQLQuery = iMySession.CreateSQLQuery(querySQL)
                        .AddEntity("E", typeof(WFObject));

                 SQLQuery.SetParameter("OBJ_SYSTEM_ID", ulEncounterID);
                 SQLQuery.SetParameter("OBJ_TYPE", "DOCUMENTATION");

                 lstObjectProcessHistory = SQLQuery.List<WFObject>();*/

                var querySQL = string.Format(@"SELECT E.* 
                                               FROM   {0} E 
                                               WHERE  E.ENCOUNTER_ID = :ENCOUNTER_ID
                                               AND E.ENCOUNTER_PROVIDER_SIGNED_DATE<>'0001-01-01 00:00:00' ", isFromArchive ? "ENCOUNTER_ARC" : "ENCOUNTER");

                var SQLQuery = iMySession.CreateSQLQuery(querySQL)
                       .AddEntity("E", typeof(Encounter));

                SQLQuery.SetParameter("ENCOUNTER_ID", ulEncounterID);
                //  SQLQuery.SetParameter("OBJ_TYPE", "DOCUMENTATION");//For Bug ID 61690 

                lstObjectProcessHistory = SQLQuery.List<Encounter>();

                if (lstObjectProcessHistory != null && lstObjectProcessHistory.Count > 0)
                    Is_physicain_process = true;
                iMySession.Close();
            }
            return Is_physicain_process;
        }


        public IList<WFObject> GetWFObjectsByObjSystemIdForPFSH(ulong uSystemID)
        {
            IList<WFObject> listWFobject = new List<WFObject>();
            object[] lstobj_Type = new object[] { "ENCOUNTER", "DOCUMENTATION" };
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", uSystemID))
                                                                                .Add(Expression.In("Obj_Type", lstobj_Type));
                listWFobject = criteria.List<WFObject>();
                iMySession.Close();
            }
            return listWFobject;
        }

        public IList<WFObject> GetByDocumentObjectSystemId(ulong ObjectSystemId, string ObjectType)
        {
            IList<WFObject> listWFobject = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", ObjectSystemId))
                                                                                .Add(Expression.Eq("Obj_Type", ObjectType));
                listWFobject = criteria.List<WFObject>();
                iMySession.Close();
            }
            return listWFobject;
        }


        //Added by Naveena 12 9.2017 for immunization Submission
        public IList<WFObject> GetImmunizationOrder()
        {
            IList<WFObject> ilstWfobject = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria iCriteria = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", "IMMUNIZATION ORDER")).Add(Expression.Eq("Current_Process", "ORDER_GENERATE"));
                ilstWfobject = iCriteria.List<WFObject>();
                iMySession.Close();

            }
            return ilstWfobject;
        }

        public void UpdateAbnormalFlag(ulong ulObjSystemID, string ObjType, string sAbnormal)
        {
            WFObject WFobj = GetByObjectSystemIdAnfObjType(ulObjSystemID, ObjType);

            WFobj.Is_Abnormal = sAbnormal;
            DateTime StartTime = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);

            IList<WFObject> wfUpdateList = new List<WFObject>();
            wfUpdateList.Add(WFobj);

            IList<WFObject> wfAddList = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref wfAddList, ref wfUpdateList, null, string.Empty, false, false, 0, string.Empty);

        }
        public WFObject GetByObjectSystemIdAnfObjType(ulong ObjectSystemId, string ObjectType)
        {
            WFObject ResultWFObj = new WFObject();
            ArrayList MyList = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Get.Enc.Details");
                query1.SetString(0, ObjectSystemId.ToString());
                query1.SetString(1, ObjectType);

                MyList.AddRange(query1.List());

                if (MyList != null)
                {
                    for (int i = 0; i < MyList.Count; i++)
                    {
                        object[] oj = (object[])MyList[i];
                        ResultWFObj.Id = Convert.ToUInt32(oj[0]);
                        ResultWFObj.Obj_System_Id = Convert.ToUInt32(oj[1]);
                        ResultWFObj.Obj_Type = oj[2].ToString();
                        ResultWFObj.Obj_Sub_Type = oj[3].ToString();
                        ResultWFObj.Fac_Name = oj[4].ToString();
                        ResultWFObj.Current_Process = oj[5].ToString();
                        ResultWFObj.Current_Arrival_Time = Convert.ToDateTime(oj[6]);
                        ResultWFObj.Current_Owner = oj[7].ToString();
                        ResultWFObj.Priority = oj[8].ToString();
                        ResultWFObj.Parent_Obj_Type = oj[9].ToString();
                        ResultWFObj.Parent_Obj_System_Id = Convert.ToUInt32(oj[10]);
                        ResultWFObj.Process_Allocation = oj[11].ToString();
                        ResultWFObj.Version = Convert.ToInt32(oj[12]);
                        ResultWFObj.Doc_Type = oj[13].ToString();
                        ResultWFObj.Doc_Sub_Type = oj[14].ToString();
                        ResultWFObj.Is_Default_MyQ_LineItem = Convert.ToInt32(oj[15]);
                        ResultWFObj.Is_Abnormal = oj[16].ToString(); ;
                    }

                }
                iMySession.Close();
            }
            return ResultWFObj;
        }
        //Added for CarePointe
        public void GetWfObjDetails(string UserName, ulong encounter_id, out string Obj_Type, out string Curr_Process, out bool Owner_Enc_Mismatch)
        {
            IList<WFObject> listWFobject = new List<WFObject>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", encounter_id))
                                                                                .Add(Expression.Eq("Current_Owner", UserName));
                listWFobject = criteria.List<WFObject>();
                iMySession.Close();
            }
            if (listWFobject != null && listWFobject.Count > 0)
            {
                Obj_Type = listWFobject[0].Obj_Type;
                Curr_Process = listWFobject[0].Current_Process;
                Owner_Enc_Mismatch = false;
            }
            else
            {
                Obj_Type = "";
                Curr_Process = "";
                Owner_Enc_Mismatch = true;
            }
        }


        public WFObject GetWfObjArchiveByObjectSystemId(ulong ObjectSystemId, string ObjectType)
        {
            WFObject ResultWFObj = new WFObject();

            ArrayList MyList = new ArrayList();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Get.Enc.Details.wfobjArchive.Tables");
                query1.SetString(0, ObjectSystemId.ToString());
                query1.SetString(1, ObjectType);

                MyList.AddRange(query1.List());

                if (MyList != null)
                {
                    for (int i = 0; i < MyList.Count; i++)
                    {
                        object[] oj = (object[])MyList[i];
                        ResultWFObj.Id = Convert.ToUInt32(oj[0]);
                        ResultWFObj.Obj_System_Id = Convert.ToUInt32(oj[1]);
                        ResultWFObj.Obj_Type = oj[2].ToString();
                        ResultWFObj.Obj_Sub_Type = oj[3].ToString();
                        ResultWFObj.Fac_Name = oj[4].ToString();
                        ResultWFObj.Current_Process = oj[5].ToString();
                        ResultWFObj.Current_Arrival_Time = Convert.ToDateTime(oj[6]);
                        ResultWFObj.Current_Owner = oj[7].ToString();
                        ResultWFObj.Priority = oj[8].ToString();
                        ResultWFObj.Parent_Obj_Type = oj[9].ToString();
                        ResultWFObj.Parent_Obj_System_Id = Convert.ToUInt32(oj[10]);
                        ResultWFObj.Process_Allocation = oj[11].ToString();
                        ResultWFObj.Version = Convert.ToInt32(oj[12]);
                        ResultWFObj.Doc_Type = oj[13].ToString();
                        ResultWFObj.Doc_Sub_Type = oj[14].ToString();
                        ResultWFObj.Is_Default_MyQ_LineItem = Convert.ToInt32(oj[15]);
                    }

                }
                iMySession.Close();
            }
            return ResultWFObj;
        }
        #endregion
    }
}

