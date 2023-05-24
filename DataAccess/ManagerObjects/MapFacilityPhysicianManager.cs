using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System.Collections;
using Acurus.Capella.Core.DTO;
using System;
using System.Linq;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IMapFacilityPhysicianManager : IManagerBase<MapFacilityPhysician, ulong>
    {
        //void SaveMapFacilityPhysician(MapFacilityPhysician facilityPhysicians, string sMacAddress);
        //int SaveMapFacilityPhysician(MapFacilityPhysician facilityPhysicians, string sMacAddress, ISession MySession);        
        //IList<MapFacilityPhysician> SaveUpdateDelete_MapFacility_Physician(IList<MapFacilityPhysician> SaveList, IList<MapFacilityPhysician> UpdateList, IList<MapFacilityPhysician> DeleteList, string MACAddress);
        //IList<MapFacilityPhysician> GetMapFacilityListbyPhyID(ulong phyID);
        //Added by Srividhya on 14-05-2013
        //ArrayList GetPhyisicianListbyFacilityName(string sFacilityName);
        //PhysicianFacilityDTO GetPhyisicianListbyFacName(string sFacilityName);
        IList<ulong> GetMapFacilityListbyPhyStatus(string sStatus);
        IList<MapFacilityPhysician> GetPhyisician_ListbyFacilityName(string Facility_Name, string sLegalOrg);
        IList<string> GetUser_ListbyFacilityName(string sFacilityName);
    }

    public partial class MapFacilityPhysicianManager : ManagerBase<MapFacilityPhysician, ulong>, IMapFacilityPhysicianManager
    {
        #region Constructors

        public MapFacilityPhysicianManager()
            : base()
        {

        }
        public MapFacilityPhysicianManager(INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region UnUsed Methods
        //public void SaveMapFacilityPhysician(MapFacilityPhysician facilityPhysicians, string sMacAddress)
        //{
        //    IList<MapFacilityPhysician> addList=null;
        //    addList.Add(facilityPhysicians);

        //    SaveUpdateDeleteWithTransaction(ref addList, null, null, sMacAddress);
        //}

        //public int SaveMapFacilityPhysician(MapFacilityPhysician facilityPhysicians, string sMacAddress,ISession MySession)
        //{
        //    IList<MapFacilityPhysician> addList = new List<MapFacilityPhysician>();
        //    addList.Add(facilityPhysicians);

        //    return SaveUpdateDeleteWithoutTransaction(ref addList, null, null,MySession, sMacAddress);
        //}

        //public IList<MapFacilityPhysician> SaveUpdateDelete_MapFacility_Physician(IList<MapFacilityPhysician> SaveList, IList<MapFacilityPhysician> UpdateList, IList<MapFacilityPhysician> DeleteList, string MACAddress)
        //{
        //    SaveUpdateDeleteWithTransaction(ref SaveList, UpdateList, DeleteList, string.Empty);
        //    IList<MapFacilityPhysician> getid = new List<MapFacilityPhysician>();
        //    if (SaveList != null)
        //    {
        //        for (int i = 0; i < SaveList.Count; i++)
        //        {
        //            getid.Add(SaveList[i]);

        //        }
        //    }
        //    return getid;
        //}

        //public IList<MapFacilityPhysician> GetMapFacilityListbyPhyID(ulong phyID)
        //{
        //     IList<MapFacilityPhysician> returnvalue =new List<MapFacilityPhysician>();
        //     using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
        //     {
        //         ICriteria crit = mySession.CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.Eq("Phy_Rec_ID", phyID)).AddOrder(Order.Asc("Sort_Order"));
        //         returnvalue = crit.List<MapFacilityPhysician>();
        //         mySession.Close();
        //     }
        //    return returnvalue;
        //}
        //added by srividhya For RCM_20140718
        //public ArrayList GetPhyisicianListbyFacilityName(string sFacilityName)
        //{
        //    ArrayList arrayList = new ArrayList();
        //    IList<MapFacilityPhysician> PhyList = new List<MapFacilityPhysician>();
        //    IList<MapFacilityPhysician> PhyListReturn = new List<MapFacilityPhysician>();
        //    using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ISQLQuery sq = mySession.CreateSQLQuery("SELECT * FROM map_facility_physician p where p.Facility_name='" + sFacilityName + "' group by Physician_ID").AddEntity("p", typeof(MapFacilityPhysician));
        //        PhyList = sq.List<MapFacilityPhysician>();

        //        for (int iCount = 0; iCount < PhyList.Count; iCount++)
        //        {
        //            //get Phy Name From Phy Library
        //            IQuery query1 = mySession.GetNamedQuery("Get.PhysicianList.GetList");
        //            query1.SetString(0, PhyList[iCount].Phy_Rec_ID.ToString());
        //            arrayList.Add(query1.List());

        //            //strPhyDetails = new string[arrayList.Count];
        //            //for (int i = 0; i < arrayList.Count; i++)
        //            //{
        //            //    object[] phyObject = (object[])arrayList[i];
        //            //    Physician_Company phyLibrary = new Physician_Company();
        //            //    //Kept Phy_Company_NPI as a Phy Name
        //            //    strPhyDetails[0] = phyObject[0].ToString();
        //            //    strPhyDetails[1] = phyObject[1].ToString();
        //            //    //phyLibrary.Physician_Library_ID = PhyList[iCount].Physician_Library_ID;
        //            //    //phyLibrary.Phy_Company_NPI = phyObject[0].ToString();

        //            //}
        //        }
        //        //foreach (IList<object> l in sq.List<object>())
        //        //{
        //        //    Physician_Company Phy = (Physician_Company)l[0];
        //        //    //get Phy Name From Phy Library
        //        //    IQuery query1 = session.GetISession().GetNamedQuery("Get.PhysicianList.GetList");
        //        //    ArrayList arrayList = new ArrayList(query1.List());
        //        //    for (int i = 0; i < arrayList.Count; i++)
        //        //    {
        //        //        Physician_Company phyLibrary = new Physician_Company();
        //        //        object[] phyObject = (object[])arrayList[i];
        //        //        //Kept Phy_Company_NPI as a Phy Name
        //        //        phyLibrary.Phy_Company_NPI= phyObject[0].ToString();
        //        //        PhyList.Add(phyLibrary);
        //        //    }
        //        //    return PhyList;
        //        //}
        //        mySession.Close();
        //    }
        //    return arrayList;
        //}
        //code added by balaji
        //added by srividhya on 14-Aug-2014
        //public PhysicianFacilityDTO GetPhyisicianListbyFacName(string sFacilityName)
        //{
        //    PhysicianFacilityDTO PhyFacDTO = new PhysicianFacilityDTO();
        //    IList<MapFacilityPhysician> MapPhyList = new List<MapFacilityPhysician>();
        //    IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
        //    using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ISQLQuery sq = mySession.CreateSQLQuery("SELECT * FROM map_facility_physician p where p.Facility_name='" + sFacilityName + "' group by Physician_ID").AddEntity("p", typeof(MapFacilityPhysician));
        //        MapPhyList = sq.List<MapFacilityPhysician>();
        //        ArrayList arrayList = new ArrayList();

        //        for (int iCount = 0; iCount < MapPhyList.Count; iCount++)
        //        {
        //            //get Phy Name From Phy Library                
        //            ISQLQuery sq1 = mySession.CreateSQLQuery("SELECT * FROM physician_library where Physician_Library_ID='" + MapPhyList[iCount].Phy_Rec_ID + "'").AddEntity("l", typeof(PhysicianLibrary));
        //            PhyList = sq1.List<PhysicianLibrary>();

        //            if (PhyList.Count > 0)
        //            {
        //                PhyFacDTO.PhyIdList.Add(PhyList[0].Id);
        //                PhyFacDTO.PhyNameList.Add(PhyList[0].PhyFirstName + "," + PhyList[0].PhyLastName);
        //            }
        //        }
        //        mySession.Close();
        //    }
        //    return PhyFacDTO;
        //}
        #endregion

        public IList<ulong> GetMapFacilityListbyPhyStatus(string sStatus)
        {
            //ICriteria crit = session.GetISession().CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.Eq("Status", sStatus)).AddOrder(Order.Asc("Sort_Order"));
            //return crit.List<MapFacilityPhysician>();
           // ISQLQuery sql;
            IList<MapFacilityPhysician> MapFacPhyList = new List<MapFacilityPhysician>();
            IList<ulong> NewMapFacPhyList = new List<ulong>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = mySession.CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.Eq("Status", sStatus));
                MapFacPhyList = criteria.List<MapFacilityPhysician>();

                for (int i = 0; i < MapFacPhyList.Count; i++)
                {
                    if (NewMapFacPhyList.Contains(MapFacPhyList[i].Phy_Rec_ID) == false)
                    {
                        NewMapFacPhyList.Add(MapFacPhyList[i].Phy_Rec_ID);
                    }
                }
                mySession.Close();
            }
            return NewMapFacPhyList;
        }

        public PhysicianFacilityDTO GetPhyisician_ListbyFacName(string Facility_Name)
        {
            PhysicianFacilityDTO PhyFacDTO = new PhysicianFacilityDTO();
            IList<MapFacilityPhysician> MapPhyList = new List<MapFacilityPhysician>();
            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.Eq("Facility_Name", Facility_Name)).AddOrder(Order.Asc("Phy_Rec_ID"));
                MapPhyList = crit.List<MapFacilityPhysician>();
                List<ulong> physicianid = new List<ulong>();
                for (int i = 0; i < MapPhyList.Count; i++)
                {
                    physicianid.Add(MapPhyList[i].Phy_Rec_ID);
                    //ICriteria critname = session.GetISession().CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", MapPhyList[i].Phy_Rec_ID));
                    //PhyList = critname.List<PhysicianLibrary>();
                    //if (PhyList.Count > 0)
                    //{
                    //    PhyFacDTO.PhyNameList.Add(PhyList[0].PhyFirstName + " " + PhyList[0].PhyMiddleName + " " + PhyList[0].PhyLastName + " " + PhyList[0].PhySuffix);
                    //    PhyFacDTO.PhyIdList.Add(PhyList[0].Id);
                    //}
                }
                ICriteria critname = mySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.In("Id", physicianid));
                PhyList = critname.List<PhysicianLibrary>();
                for (int i = 0; i < PhyList.Count; i++)
                {
                    if (PhyList.Count > 0)
                    {
                        PhyFacDTO.PhyNameList.Add(PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName + " " + PhyList[i].PhySuffix);
                        PhyFacDTO.PhyIdList.Add(PhyList[i].Id);
                    }
                }
                mySession.Close();
            }
            return PhyFacDTO;
        }
        public IList<MapFacilityPhysician> GetPhyisician_ListbyFacilityName(string Facility_Name,string sLegalOrg)
        {
            IList<MapFacilityPhysician> MapPhyList = new List<MapFacilityPhysician>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.Eq("Facility_Name", Facility_Name)).Add(Expression.Eq("Legal_Org", sLegalOrg)).AddOrder(Order.Asc("Phy_Rec_ID"));
                MapPhyList = crit.List<MapFacilityPhysician>();
                mySession.Close();
            }
            return MapPhyList;
        }
        public IList<string> GetUser_ListbyFacilityName(string sFacilityName)
        {
            IList<string> UserList = new List<string>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IList<string> objLst = mySession.CreateSQLQuery("select user_name from map_facility_user where Facility_Name='" + sFacilityName +"'").List<string>();
                UserList = objLst.ToList<string>(); ;
                mySession.Close();
            }
            return UserList;
        }
        public IList<MapFacilityPhysician> GetPhyisician_ListbyFacNameList(IList<string> Facility_Name, ulong ulPhysicianId)
        {
            IList<MapFacilityPhysician> MapPhyList = new List<MapFacilityPhysician>();
            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.In("Facility_Name", Facility_Name.ToList())).Add(Expression.Eq("Phy_Rec_ID", ulPhysicianId)).AddOrder(Order.Asc("Phy_Rec_ID"));
                MapPhyList = crit.List<MapFacilityPhysician>();
                mySession.Close();
            }
            return MapPhyList;
        }
    }
}
