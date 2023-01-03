using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;

//Added by Selvaraman - for AuditTrail
namespace Acurus.Capella.DataAccess
{
    public class NHibernateSessionUtility
    {
        // Singleton instance
        private static NHibernateSessionUtility _instance;
        public ISession MySession = null;
        public string MACAddress = string.Empty;
        public Hashtable AuditTrailTableList = null;
        public IList<AuditLog> MyAuditLogList = new List<AuditLog>();
        public IList<WorkFlow> MyWorkFlowList = new List<WorkFlow>();
        public IList<WorkFlowTypeMaster> MyWorkFlowTypeMasterList = new List<WorkFlowTypeMaster>();
        public IList<ProcessMaster> MyProcessmasterList = new List<ProcessMaster>();
        public IList<FacilityLibrary> MyAncillaryFacilityList = new List<FacilityLibrary>();
        public IList<FacilityLibrary> MyFacilityList = new List<FacilityLibrary>();
        public IList<MapXMLBlob> MyMapXMLBlobList = new List<MapXMLBlob>();

        private NHibernateSessionUtility()
        {
        }
        
        public void FillInstance()
        {
            //Handeled through trigger -jisha
            //Fill AuditTrailList
            try
            {
                AuditTrailTableList = new Hashtable();
                MasterTableListManager masterMngr = new MasterTableListManager();
                IList<MasterTableList> MasterList = new List<MasterTableList>();
                MasterList = masterMngr.GetAuditTrailTableList();

                for (int i = 0; i < MasterList.Count; i++)
                {
                    if (!AuditTrailTableList.Contains(MasterList[i].Domain_Object_Name))
                        AuditTrailTableList.Add(MasterList[i].Domain_Object_Name, "AuditTrail");
                }
            }
            catch 
            {

            }

            try
            {
                if (MyWorkFlowList.Count == 0)
                {
                    WorkFlowManager workflowMngr = new WorkFlowManager();
                    MyWorkFlowList = workflowMngr.GetAll();
                }

            }
            catch 
            {

            }

            try
            {
                if (MyWorkFlowTypeMasterList.Count == 0)
                {
                    WorkFlowTypeMasterManager workflowTypeMasterMngr = new WorkFlowTypeMasterManager();
                    MyWorkFlowTypeMasterList = workflowTypeMasterMngr.GetAll();
                }

            }
            catch
            {

            }

            try
            {
                if (MyProcessmasterList.Count == 0)
                {
                    ProcessMasterManager processmasterMngr = new ProcessMasterManager();
                    MyProcessmasterList = processmasterMngr.GetAll();
                }

            }
            catch 
            {

            }

            try
            {
                if (MyAncillaryFacilityList.Count == 0)
                {
                    FacilityManager facMngr = new FacilityManager();
                    MyAncillaryFacilityList = facMngr.GetAncillaryFacilityList();
                }

            }
            catch
            {

            }

            try
            {
                if (MyFacilityList.Count == 0)
                {
                    FacilityManager facMngr = new FacilityManager();
                    MyFacilityList = facMngr.GetAll();
                }

            }
            catch
            {

            }
            try
            {
                if (MyMapXMLBlobList.Count == 0)
                {
                    MapXMLBlobManager mapxmlblobMngr = new MapXMLBlobManager();
                    MyMapXMLBlobList = mapxmlblobMngr.GetAll();
                }

            }
            catch
            {

            }
        }

        public static NHibernateSessionUtility Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NHibernateSessionUtility();

                    _instance.FillInstance();
                }

                return _instance;
            }
        }
    }
}








//################Old Code######################### Till 22-Feb-2011
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NHibernate;
//using Acurus.Capella.Core.DomainObjects;
//using Acurus.Capella.DataAccess.ManagerObjects;

////Added by Selvaraman - for AuditTrail
//namespace Acurus.Capella.DataAccess
//{
//    public static class NHibernateSessionUtility
//    {
//      public static ISession MySession = null;
//      public static string MACAddress = string.Empty;
//      //public static Boolean bAuditTrailOn = false;
//      public static Hashtable AuditTrailTableList = null;

//      public static void CheckAuditTrailTableOrNot(string DomainObjName)
//      {
//          if (AuditTrailTableList == null)
//          {
//              AuditTrailTableList = new Hashtable();
//              MasterTableListManager masterMngr = new MasterTableListManager();
//              masterMngr.GetByDomainName();
//          }

//          //if (AuditTrailTableList.Contains(DomainObjName)==true)
//          //{
//          //    bAuditTrailOn=true;
//          //}
//          //else
//          //{
//          //    bAuditTrailOn=false;
//          //}
//      }
//    }
//}

