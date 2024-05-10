using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
using System.Linq;


namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IPhysicianManager : IManagerBase<PhysicianLibrary, ulong>
    {
        //IList<PhysicianLibrary> GetPhysicianList();//prabu 04/05/2011
        Stream GetPhysicianList();
        IList<PhysicianLibrary> GetphysiciannameByPhyID(ulong PhyID);
        void UpdatePhysicians(PhysicianLibrary libraries, string sMacAddress);
        void DeletePhysicians(PhysicianLibrary libraries, string sMacAddress);
        IList<PhysicianLibrary> GetPhysicianListbyFacilityForRCM(string FacName, string sActive);
        ulong SavePhysicians(PhysicianLibrary libraries, string sMacAddress);//vinoth 15/04/2010
        IList<PhysicianLibrary> GetPhysicianListbyFacility(string FacName, string sActive);
        IList<PhysicianLibrary> GetDTOLibraryList(ulong PhyID, int Pagenumber, int MaxResults);
        //IList<PhysicianPOV> GetDTOInterMediateList(ulong PhyID, int Pagenumber, int MaxResults);
        //FillPhysicianManager DTOPhysicianFill(ulong PhyID, int Pagenumber, int MaxResults);
        int GetPhysicianCount();
        FillPhysicianUser GetPhysicianandUserForEncounter(Boolean bFacilityBasis, string sFacName);
        FillPhysicianUser GetPhysicianandUser(Boolean bFacilityBasis, string sFacName, string sLegalOrg);
        //PlanDTO GetReferralPhysician(string sLastName, string sFirstName, int iPageNumber, int iMaxresult);
        FindPhysican FindPhysicianForReferral(string sLastName, string sFirstName, string sSpecialty,
            string sNPI, string sFacility, string sAddress, string sCity, string sState, string sZip, int Pagenumber, int MaxResult);
        IList<PhysicianLibrary> GetPhysicianListbyType(string PhyType);
        ///vince 18-11-11
        IList<PhysicianLibrary> SaveUpdateDeletePhysicianLibrary(IList<PhysicianLibrary> SaveList, IList<PhysicianLibrary> UpdateList, IList<PhysicianLibrary> DeleteList, string MACAddress);
        //FillPhysicianManager SaveUpdateDeletePhysicianManager(IList<PhysicianLibrary> savePhyLibList, IList<PhysicianLibrary> updatePhyLibList, IList<PhysicianLibrary> deletePhyLibList, IList<PhysicianPOV> savePhyPOVList, IList<PhysicianPOV> updatePhyPOVList, IList<PhysicianPOV> deletePhyPOVList, IList<MapFacilityPhysician> saveMapPhyList, IList<MapFacilityPhysician> updateMapPhyList, IList<MapFacilityPhysician> deleteMapPhyList, string macAddress);
        //FillPhysicianManager GetSavedPhyscianByID(ulong ulPhyID);
        IList<PhysicianLibrary> GetPhysicianListbyFacility(string FacName, string sActive, string AppointBy);
        IList<PhysicianLibrary> GetPCPNameByPatInsuredId(int InsurancePlanId);
        //Added by suresh for RCM
        IList<PhysicianLibrary> LoadPhysicianList();
        //Added by suresh for RCM

        //Added by Gopal For RCM
        //ArrayList GetPhyisicianList();
        //IList<Physician_Company> GetphysicianCompanyByPhyID(ulong ulBillID, ulong ulRendProvID, string sFacilityName);
        //Added by srividhya on 18-Jul-2014
        //ArrayList GetPhyisicianListbyFacilityName(string sFacilityName);
        ulong SavePhysicanforSummary(IList<PhysicianLibrary> lstphysician);
        //Added by balaji on 22-Aug-2014
        //IList<PhysicianLibrary> Get_PhysicianList();
        IList<PhysicianLibrary> GetRenderingPhyisicianList();
        IList<PhysicianLibrary> Get_PhysicianList(string proviserid);
        IList<PhysicianLibrary> GetphysiciannameByUserName(string sUserName);
        IList<FillPhysicianLibrary> GetPhysicianByCategory(IList<string> Category);
        IList<PhysicianLibrary> GetPhysicianByFax(string Fax);
        //IList<PhysicianFacilityCompanyCarrier> GetPhyFacCompanyCarrierDetails(ulong ulBillID, ulong ulRendProvID, string sBillingFacility, ulong ulCarrierID);
    }

    public partial class PhysicianManager : ManagerBase<PhysicianLibrary, ulong>, IPhysicianManager
    {
        #region Constructors
        //public IList<FillPhysicianLibrary> ilstPhysicianLibraryCategory = new List<FillPhysicianLibrary>();
        public PhysicianManager()
            : base()
        {

        }
        public PhysicianManager(INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods

        public int GetPhysicianCount()
        {
            ArrayList arrayList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PhysicianManager.GetCount");
                arrayList = new ArrayList(query1.List());
                iMySession.Close();
            }
            return arrayList.Count;
        }

        //public FillPhysicianManager DTOPhysicianFill(ulong PhyID, int Pagenumber, int MaxResults)
        //{
        //    FillPhysicianManager phyManager = new FillPhysicianManager();
        //    phyManager.GetCount = GetPhysicianCount();
        //    phyManager.PhyLibrary = GetDTOLibraryList(PhyID, Pagenumber, MaxResults);
        //    if (PhyID != 0)
        //    {
        //        phyManager.PhyInterMediate = GetDTOInterMediateList(PhyID, Pagenumber, MaxResults);
        //    }
        //    return phyManager;
        //}

        public IList<PhysicianLibrary> GetDTOLibraryList(ulong PhyID, int Pagenumber, int MaxResults)
        {
            IList<PhysicianLibrary> libraryList = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query1;
                if (PhyID != 0)
                {
                    query1 = iMySession.GetNamedQuery("Fill.PhysicianLibrary.GetListWithID");
                    query1.SetParameter(0, PhyID);
                }
                else
                {
                    query1 = iMySession.GetNamedQuery("Fill.PhysicianLibrary.GetListWithoutID");
                    Pagenumber = Pagenumber - 1;
                    query1.SetParameter(0, Pagenumber * MaxResults);
                    query1.SetParameter(1, MaxResults);
                }

                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PhysicianLibrary library = new PhysicianLibrary();
                    object[] phyObject = (object[])arrayList[i];
                    library.Id = Convert.ToUInt32(phyObject[0].ToString());
                    library.PhyPrefix = phyObject[1].ToString();
                    library.PhyFirstName = phyObject[2].ToString();
                    library.PhyMiddleName = phyObject[3].ToString();
                    library.PhyLastName = phyObject[4].ToString();
                    library.PhySuffix = phyObject[5].ToString();
                    library.PhyAddress1 = phyObject[6].ToString();
                    library.PhyCity = phyObject[7].ToString();
                    library.PhyState = phyObject[8].ToString();
                    library.PhyZip = phyObject[9].ToString();
                    library.PhyTelephone = phyObject[10].ToString();
                    library.PhyFax = phyObject[11].ToString();
                    library.PhyEMail = phyObject[12].ToString();
                    library.PhyNPI = phyObject[13].ToString();
                    library.PhyOtherID = phyObject[14].ToString();
                    library.Version = Convert.ToInt32(phyObject[15].ToString());
                    libraryList.Add(library);
                }
                iMySession.Close();
            }
            return libraryList;
        }

        //public IList<PhysicianPOV> GetDTOInterMediateList(ulong PhyID, int Pagenumber, int MaxResults)
        //{
        //    IList<PhysicianPOV> intermediateList = new List<PhysicianPOV>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = iMySession.GetNamedQuery("Fill.PhysicianIntermediate.GetList");
        //        Pagenumber = Pagenumber - 1;
        //        query1.SetParameter(0, PhyID);
        //        query1.SetParameter(1, Pagenumber * MaxResults);
        //        query1.SetParameter(2, MaxResults);
        //        ArrayList arrayList = new ArrayList(query1.List());
        //        for (int i = 0; i < arrayList.Count; i++)
        //        {
        //            PhysicianPOV intermediate = new PhysicianPOV();
        //            object[] phyObject = (object[])arrayList[i];
        //            intermediate.Id = Convert.ToUInt32(phyObject[0].ToString());
        //            intermediate.Phy_ID = Convert.ToUInt32(phyObject[1].ToString());
        //            intermediate.Purpose_of_Visit = phyObject[2].ToString();
        //            intermediate.Duration = Convert.ToInt32(phyObject[3].ToString());
        //            intermediate.Version = Convert.ToInt32(phyObject[4].ToString());
        //            intermediateList.Add(intermediate);
        //        }
        //        iMySession.Close();
        //    }
        //    return intermediateList;
        //}

        //prabu 04/05/2011
        public Stream GetPhysicianList()
        {
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            IList<PhysicianLibrary> objPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(PhysicianLibrary));
                objPhysicianLibrary = criteria.List<PhysicianLibrary>();

                serializer.WriteObject(stream, objPhysicianLibrary);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;
        }
        //prabu 04/05/2011
        public IList<PhysicianLibrary> GetphysiciannameByPhyID(ulong PhyID)
        {
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", PhyID));
                ilstPhysicianLibrary = criteria.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return ilstPhysicianLibrary;
            //return criteria.List<PhysicianLibrary>();
        }
        public void UpdatePhysicians(PhysicianLibrary libraries, string sMacAddress)
        {
            IList<PhysicianLibrary> libraryList = new List<PhysicianLibrary>();
            IList<PhysicianLibrary> nullList = new List<PhysicianLibrary>();
            nullList = null;
            libraryList.Add(libraries);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref nullList, ref libraryList, null, sMacAddress, false, false, 0, "");

        }
        public void DeletePhysicians(PhysicianLibrary libraries, string sMacAddress)
        {
            IList<PhysicianLibrary> libraryList = new List<PhysicianLibrary>();
            IList<PhysicianLibrary> nullList = new List<PhysicianLibrary>();
            nullList = null;
            libraryList.Add(libraries);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref nullList, ref nullList, libraryList, sMacAddress, false, false, 0, "");
        }

        //vinoth 15/04/2010
        public ulong SavePhysicians(PhysicianLibrary libraries, string sMacAddress)
        {
            IList<PhysicianLibrary> libraryList = new List<PhysicianLibrary>();
            IList<PhysicianLibrary> nullList = null;
            libraryList.Add(libraries);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref libraryList, ref nullList, null, sMacAddress, false, false, 0, "");
            return libraryList[0].Id;
        }


        public IList<PhysicianLibrary> GetPhysicianListbyFacility(string FacName, string sActive)
        {
            if (sActive == "N")
            {
                sActive = string.Empty;
            }
            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq;
                if (FacName != string.Empty)
                {
                    sq = iMySession.CreateSQLQuery("SELECT p.*,m.* FROM physician_library p,map_facility_physician m where p.Physician_Library_ID=m.physician_id and m.Status LIKE '" + sActive + "%' and m.facility_name = '" + FacName + "' order by p.sort_order,p.Physician_First_Name,p.Physician_Last_Name")
                     .AddEntity("p", typeof(PhysicianLibrary))
                     .AddEntity("m", typeof(MapFacilityPhysician));
                }
                else
                {
                    sq = iMySession.CreateSQLQuery("SELECT p.*,m.* FROM physician_library p,map_facility_physician m where p.Physician_Library_ID=m.physician_id and m.Status LIKE'" + sActive + "%' Group by m.Physician_ID order by p.sort_order,p.Physician_First_Name,p.Physician_Last_Name")
                     .AddEntity("p", typeof(PhysicianLibrary))
                     .AddEntity("m", typeof(MapFacilityPhysician));
                }

                foreach (IList<Object> l in sq.List())
                {
                    PhysicianLibrary Phy = (PhysicianLibrary)l[0];

                    PhyList.Add(Phy);
                }
                iMySession.Close();
            }
            return PhyList;
        }
        //Added by Gopal for Load Physician List

        public IList<PhysicianLibrary> GetPhysicianListbyFacilityForRCM(string FacName, string sActive)
        {
            if (sActive == "N")
            {
                sActive = string.Empty;
            }

            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PhysicianList.GetList");
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PhysicianLibrary phyLibrary = new PhysicianLibrary();
                    object[] phyObject = (object[])arrayList[i];
                    phyLibrary.PhyId = Convert.ToUInt32(phyObject[0]);
                    phyLibrary.PhyLastName = phyObject[1].ToString();
                    phyLibrary.PhyFirstName = phyObject[2].ToString();
                    phyLibrary.PhyMiddleName = phyObject[3].ToString();
                    phyLibrary.PhyPrefix = phyObject[4].ToString();
                    PhyList.Add(phyLibrary);
                }
                iMySession.Close();
            }
            return PhyList;
        }

        public IList<PhysicianLibrary> GetPhysicianListbyFacility(string FacName, string sActive, string AppointBy)
        {
            if (sActive == "N")
            {
                sActive = string.Empty;
            }
            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq;

                if (FacName != string.Empty)
                {
                    sq = iMySession.CreateSQLQuery("SELECT p.*,m.* FROM physician_library p,map_facility_physician m where p.Physician_Library_ID=m.physician_id and m.Status LIKE '" + sActive + "%' and m.facility_name = '" + FacName + "' and p.Category = '" + AppointBy + "'")
                     .AddEntity("p", typeof(PhysicianLibrary))
                     .AddEntity("m", typeof(MapFacilityPhysician));
                }
                else
                {
                    sq = iMySession.CreateSQLQuery("SELECT p.*,m.* FROM physician_library p,map_facility_physician m where p.Physician_Library_ID=m.physician_id and m.Status LIKE'" + sActive + "%' and p.Category = '" + AppointBy + "' Group by m.Physician_ID")
                     .AddEntity("p", typeof(PhysicianLibrary))
                     .AddEntity("m", typeof(MapFacilityPhysician));
                }
                foreach (IList<Object> l in sq.List())
                {
                    PhysicianLibrary Phy = (PhysicianLibrary)l[0];

                    PhyList.Add(Phy);
                }
                iMySession.Close();
            }
            return PhyList;
        }

        public FillPhysicianUser GetPhysicianandUserForEncounter(Boolean bFacilityBasis, string sFacName)
        {
            ISQLQuery sq;
            FillPhysicianUser PhyUserList = new FillPhysicianUser();

            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            IList<User> UserList = new List<User>();
            IList<MapFacilityPhysician> MapList = new List<MapFacilityPhysician>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (bFacilityBasis == false)
                {
                    sq = iMySession.CreateSQLQuery("SELECT p.*,u.*,m.* FROM physician_library p,user u,map_facility_physician m where p.Physician_Library_ID=u.Physician_Library_ID and m.Physician_ID=p.Physician_Library_ID order by p.physician_first_Name asc")
                    .AddEntity("p", typeof(PhysicianLibrary))
                    .AddEntity("u", typeof(User))
                    .AddEntity("m", typeof(MapFacilityPhysician));
                }
                //if (bFacilityBasis == false)
                //{
                //    sq = iMySession.CreateSQLQuery("SELECT p.*,u.* FROM physician_library p,user u where p.Physician_Library_ID=u.Physician_Library_ID")
                // .AddEntity("p", typeof(PhysicianLibrary))
                // .AddEntity("u", typeof(User));
                //}
                else
                {
                    sq = iMySession.CreateSQLQuery("SELECT p.*,u.* FROM physician_library p,user u,map_facility_physician m where p.Physician_Library_ID=u.Physician_Library_ID and m.Physician_ID=p.Physician_Library_ID and m.Facility_Name = '" + sFacName + "'order by p.physician_first_Name asc")
                 .AddEntity("p", typeof(PhysicianLibrary))
                 .AddEntity("u", typeof(User));
                }
                ArrayList arylst;
                arylst = new ArrayList(sq.List());//added by naveena for bug_id 26744
                foreach (IList<Object> l in arylst)
                {
                    PhysicianLibrary Phy = (PhysicianLibrary)l[0];
                    User User = (User)l[1];
                    PhyList.Add(Phy);
                    UserList.Add(User);
                    if (bFacilityBasis == false)
                    {
                        MapFacilityPhysician Map = (MapFacilityPhysician)l[2];
                        MapList.Add(Map);
                    }
                }

                PhyUserList.PhyList = PhyList.Distinct().ToList<PhysicianLibrary>();
                PhyUserList.UserList = UserList.Distinct().ToList<User>();
                //PhyUserList.UserList = UserList;
                PhyUserList.MapFacList = MapList;
                iMySession.Close();
            }
            return PhyUserList;
        }

        public FillPhysicianUser GetPhysicianUser(string facilityName)
        {
            ISQLQuery sqlQuery;
            FillPhysicianUser PhyUserList = new FillPhysicianUser();

            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            IList<User> UserList = new List<User>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

            var query = string.Empty;



            if (string.IsNullOrEmpty(facilityName))
            {
                query = @"SELECT P.*, 
                                 U.* 
                          FROM   PHYSICIAN_LIBRARY P, 
                                 USER U 
                          WHERE  P.PHYSICIAN_LIBRARY_ID = U.PHYSICIAN_LIBRARY_ID ";

                sqlQuery = iMySession.CreateSQLQuery(query)
                                     .AddEntity("P", typeof(PhysicianLibrary))
                                     .AddEntity("U", typeof(User));
            }
            else
            {
                query = @"SELECT P.*, 
                                 U.* 
                          FROM   PHYSICIAN_LIBRARY P, 
                                 USER U, 
                                 MAP_FACILITY_PHYSICIAN M 
                          WHERE  P.PHYSICIAN_LIBRARY_ID = U.PHYSICIAN_LIBRARY_ID 
                                 AND M.PHYSICIAN_ID = P.PHYSICIAN_LIBRARY_ID 
                                 AND M.FACILITY_NAME = :FACILITY_NAME 
                          ORDER  BY P.PHYSICIAN_FIRST_NAME ASC ";

                sqlQuery = iMySession.CreateSQLQuery(query)
                                     .AddEntity("P", typeof(PhysicianLibrary))
                                     .AddEntity("U", typeof(User));

                sqlQuery.SetParameter("FACILITY_NAME", facilityName);

            }

            ArrayList arylst;
            arylst = new ArrayList(sqlQuery.List());

            foreach (IList<Object> l in arylst)
            {
                PhysicianLibrary Phy = (PhysicianLibrary)l[0];
                User User = (User)l[1];
                PhyList.Add(Phy);
                UserList.Add(User);
            }

            PhyUserList.PhyList = PhyList;
            PhyUserList.UserList = UserList;
            iMySession.Close();
            return PhyUserList;
        }

        public FillPhysicianUser GetPhysicianandUser(Boolean bFacilityBasis, string sFacName, string sLegalOrg)
        {
            ISQLQuery sq;
            FillPhysicianUser PhyUserList = new FillPhysicianUser();

            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            IList<User> UserList = new List<User>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();


            if (bFacilityBasis == false)
            {
                //GitLab#4177 and 4179
                sq = iMySession.CreateSQLQuery("SELECT p.*,u.* FROM physician_library p,user u where p.Physician_Library_ID=u.Physician_Library_ID and u.Legal_Org='" + sLegalOrg + "' and u.Status='A' order by p.physician_last_Name asc")
             .AddEntity("p", typeof(PhysicianLibrary))
             .AddEntity("u", typeof(User));
            }
            else
            {
                //GitLab#4177 and 4179
                sq = iMySession.CreateSQLQuery("SELECT p.*,u.* FROM physician_library p,user u,map_facility_physician m where p.Physician_Library_ID=u.Physician_Library_ID and m.Physician_ID=p.Physician_Library_ID and m.Facility_Name = '" + sFacName + "' and u.Status='A' order by p.physician_last_Name asc")
             .AddEntity("p", typeof(PhysicianLibrary))
             .AddEntity("u", typeof(User));
            }
            ArrayList arylst;
            arylst = new ArrayList(sq.List());//added by naveena for bug_id 26744
            foreach (IList<Object> l in arylst)
            {
                PhysicianLibrary Phy = (PhysicianLibrary)l[0];
                User User = (User)l[1];
                PhyList.Add(Phy);
                UserList.Add(User);
            }

            PhyUserList.PhyList = PhyList;
            PhyUserList.UserList = UserList;
            iMySession.Close();
            return PhyUserList;
        }


        public FindPhysican FindPhysicianForReferral(string sLastName, string sFirstName, string sSpecialty,
            string sNPI, string sFacility, string sAddress, string sCity, string sState, string sZip, int Pagenumber, int MaxResult)
        {
            IList<PhysicianFacilityDTO> phylst = new List<PhysicianFacilityDTO>();
            FindPhysican objFindPhy = new FindPhysican();
            ArrayList resultList = null;
            IQuery iquery1 = null;
            IQuery iquery = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (sFacility != string.Empty)
                {
                    iquery1 = iMySession.GetNamedQuery("Fill.FindPhysicianCountWithFacility");
                    iquery1.SetString(0, sLastName + "%");
                    iquery1.SetString(1, sFirstName + "%");
                    iquery1.SetString(2, sFirstName + "%");
                    iquery1.SetString(3, sLastName + "%");
                    iquery1.SetString(4, sSpecialty + "%");
                    iquery1.SetString(5, sSpecialty + "%");
                    iquery1.SetString(6, sNPI + "%");
                    iquery1.SetString(7, sFacility + "%");
                    iquery1.SetString(8, sAddress + "%");
                    iquery1.SetString(9, sCity + "%");
                    iquery1.SetString(10, sState + "%");
                    iquery1.SetString(11, sZip + "%");
                    iquery1.SetString(12, sNPI + "%");
                    iquery1.SetString(13, sFacility + "%");
                    iquery1.SetString(14, sAddress + "%");
                    iquery1.SetString(15, sCity + "%");
                    iquery1.SetString(16, sState + "%");
                    iquery1.SetString(17, sZip + "%");
                }
                else
                {
                    iquery1 = iMySession.GetNamedQuery("Fill.FindPhysicianCountWithoutFacility");
                    iquery1.SetString(0, sLastName + "%");
                    iquery1.SetString(1, sFirstName + "%");
                    iquery1.SetString(2, sFirstName + "%");
                    iquery1.SetString(3, sLastName + "%");
                    iquery1.SetString(4, sSpecialty + "%");
                    iquery1.SetString(5, sSpecialty + "%");
                    iquery1.SetString(6, sNPI + "%");
                    iquery1.SetString(7, sAddress + "%");
                    iquery1.SetString(8, sCity + "%");
                    iquery1.SetString(9, sState + "%");
                    iquery1.SetString(10, sZip + "%");
                    iquery1.SetString(11, sNPI + "%");
                    iquery1.SetString(12, sAddress + "%");
                    iquery1.SetString(13, sCity + "%");
                    iquery1.SetString(14, sState + "%");
                    iquery1.SetString(15, sZip + "%");
                }

                resultList = new ArrayList(iquery1.List());

                if (resultList.Count > 0)
                    objFindPhy.PhyCount = Convert.ToInt16(resultList[0]);
                resultList.Clear();
                if (sFacility != string.Empty)
                {
                    iquery = iMySession.GetNamedQuery("Fill.FindPhysicianWithFacility");
                    iquery.SetString(0, sLastName + "%");
                    iquery.SetString(1, sFirstName + "%");
                    iquery.SetString(2, sFirstName + "%");
                    iquery.SetString(3, sLastName + "%");
                    iquery.SetString(4, sSpecialty + "%");
                    iquery.SetString(5, sSpecialty + "%");
                    iquery.SetString(6, sNPI + "%");
                    iquery.SetString(7, sFacility + "%");
                    iquery.SetString(8, sAddress + "%");
                    iquery.SetString(9, sCity + "%");
                    iquery.SetString(10, sState + "%");
                    iquery.SetString(11, sZip + "%");
                    iquery.SetString(12, sNPI + "%");
                    iquery.SetString(13, sFacility + "%");
                    iquery.SetString(14, sAddress + "%");
                    iquery.SetString(15, sCity + "%");
                    iquery.SetString(16, sState + "%");
                    iquery.SetString(17, sZip + "%");
                    iquery.SetInt32(18, (Pagenumber - 1) * MaxResult);
                    iquery.SetInt32(19, MaxResult);
                }
                else
                {
                    iquery = iMySession.GetNamedQuery("Fill.FindPhysicianWithoutFacility");
                    iquery.SetString(0, sLastName + "%");
                    iquery.SetString(1, sFirstName + "%");
                    iquery.SetString(2, sFirstName + "%");
                    iquery.SetString(3, sLastName + "%");
                    iquery.SetString(4, sSpecialty + "%");
                    iquery.SetString(5, sSpecialty + "%");
                    iquery.SetString(6, sNPI + "%");
                    iquery.SetString(7, sAddress + "%");
                    iquery.SetString(8, sCity + "%");
                    iquery.SetString(9, sState + "%");
                    iquery.SetString(10, sZip + "%");
                    iquery.SetString(11, sNPI + "%");
                    iquery.SetString(12, sAddress + "%");
                    iquery.SetString(13, sCity + "%");
                    iquery.SetString(14, sState + "%");
                    iquery.SetString(15, sZip + "%");
                    iquery.SetInt32(16, (Pagenumber - 1) * MaxResult);
                    iquery.SetInt32(17, MaxResult);
                }
                resultList = new ArrayList(iquery.List());
                if (resultList.Count > 0)
                {
                    foreach (object[] obj in resultList)
                    {
                        PhysicianFacilityDTO objPhy = new PhysicianFacilityDTO();
                        objPhy.PhyId = Convert.ToUInt64(obj[0]);
                        objPhy.PhyPrefix = obj[1].ToString();
                        objPhy.PhyFirstName = obj[2].ToString();
                        objPhy.PhyMiddleName = obj[3].ToString();
                        objPhy.PhyLastName = obj[4].ToString();
                        objPhy.PhySuffix = obj[5].ToString();
                        objPhy.PhySpecialtyCode = Convert.ToString(obj[6]);
                        objPhy.PhyCity = obj[7].ToString();
                        objPhy.PhyState = obj[8].ToString();
                        objPhy.PhyZip = obj[9].ToString();
                        objPhy.PhyNPI = obj[10].ToString();
                        // objPhy.PhySpecialtyID = Convert.ToUInt64(obj[11]); 7/2/
                        objPhy.PhySpecialtyID = Convert.ToString(obj[11]);
                        objPhy.PhyFax = obj[13].ToString();
                        objPhy.PhyAddrs = obj[14].ToString();
                        objPhy.PhyPhone = obj[15].ToString();
                        if (obj[12] != null)
                            objPhy.PhyFacility = obj[12].ToString();
                        phylst.Add(objPhy);
                    }
                }

                objFindPhy.PhyList = phylst;

                iMySession.Close();
            }
            return objFindPhy;
        }

        public FindPhysican FindPhysician(string token)
        {
            IList<PhysicianFacilityDTO> phylst = new List<PhysicianFacilityDTO>();
            FindPhysican objFindPhy = new FindPhysican();
            ArrayList resultList = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //CAP-1639
                IQuery query = iMySession.GetNamedQuery("FindPhysician");
                query.SetParameter(0, "%" + token + "%");
                query.SetParameter(1, "%" + token + "%");
                query.SetParameter(2, "%" + token + "%");
                query.SetParameter(3, "%" + token + "%");
                query.SetParameter(4, "%" + token + "%");
                query.SetParameter(5, "%" + token + "%");
                resultList = new ArrayList(query.List());
                if (resultList.Count > 0)
                {
                    FacilityManager Fmgr = new FacilityManager();
                    IList<FacilityLibrary> ilstFacility = new List<FacilityLibrary>();
                    ilstFacility = Fmgr.GetFacilityList();

                    foreach (object[] obj in resultList)
                    {
                        PhysicianFacilityDTO objPhy = new PhysicianFacilityDTO();
                        objPhy.PhyId = Convert.ToUInt64(obj[0]);
                        objPhy.PhyPrefix = obj[1].ToString();
                        if (obj[17] != null && obj[17].ToString().ToUpper() != "ORGANIZATION")
                        {
                            objPhy.PhyFirstName = obj[2].ToString();
                            objPhy.PhyMiddleName = obj[3].ToString();
                            objPhy.PhyLastName = obj[4].ToString();
                            objPhy.PhySuffix = obj[5].ToString();
                        }
                        else
                        {
                            objPhy.PhyFirstName = obj[16]?.ToString()??string.Empty;
                        }
                        
                        
                        objPhy.PhySpecialtyCode = Convert.ToString(obj[6]);
                        //objPhy.PhyCity = obj[7].ToString();
                        //objPhy.PhyState = obj[8].ToString();
                        //objPhy.PhyZip = obj[9].ToString();
                        objPhy.PhyNPI = obj[10].ToString();
                        //objPhy.PhySpecialtyID = Convert.ToUInt64(obj[11]);7/2/
                        objPhy.PhySpecialtyID = Convert.ToString(obj[11]);
                        //objPhy.PhyFax = obj[13].ToString();
                        objPhy.PhyAddrs = obj[14].ToString();
                        //objPhy.PhyPhone = obj[15].ToString();
                        if (obj[17] != null)
                            objPhy.Category = obj[17].ToString();
                        if (obj[12] != null)
                        {
                            objPhy.PhyFacility = obj[12].ToString();
                            //FacilityManager Fmgr = new FacilityManager();
                            IList<FacilityLibrary> ilstFacilityLibrary = new List<FacilityLibrary>();
                            //ilstFacilityLibrary = Fmgr.GetFacilityByFacilityname(obj[12].ToString());

                            ilstFacilityLibrary = (from objfac in ilstFacility where objfac.Fac_Name == obj[12].ToString() select objfac).ToList<FacilityLibrary>();
                            if (ilstFacilityLibrary != null && ilstFacilityLibrary.Count > 0)
                            {
                                objPhy.PhyCity = ilstFacilityLibrary[0].Fac_City.ToString();
                                objPhy.PhyState = ilstFacilityLibrary[0].Fac_State.ToString();
                                objPhy.PhyZip = ilstFacilityLibrary[0].Fac_Zip.ToString();
                                objPhy.PhyFax = ilstFacilityLibrary[0].Fac_Fax.ToString();
                                objPhy.PhyPhone = ilstFacilityLibrary[0].Fac_Telephone.ToString();
                            }
                        }
                        else
                        {
                            if (obj[12] != null)
                                objPhy.PhyFacility = obj[12].ToString();
                            if (obj[7] != null)
                                objPhy.PhyCity = obj[7].ToString();
                            if (obj[8] != null)
                                objPhy.PhyState = obj[8].ToString();
                            if (obj[9] != null)
                                objPhy.PhyZip = obj[9].ToString();
                            if (obj[13] != null)
                                objPhy.PhyFax = obj[13].ToString();
                            if (obj[15] != null)
                                objPhy.PhyPhone = obj[15].ToString();
                        }

                        phylst.Add(objPhy);
                    }
                }

                objFindPhy.PhyList = phylst;

                iMySession.Close();
            }
            return objFindPhy;
        }
        public FindPhysican FindPhysicianID(IList<ulong> token)
        {
            IList<PhysicianFacilityDTO> phylst = new List<PhysicianFacilityDTO>();
            FindPhysican objFindPhy = new FindPhysican();
            ArrayList resultList = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Find.Physician.by.ID");
                query.SetParameterList("PhyIds", token.ToArray());

                //query.SetParameter(3, "%" + token + "%");
                //query.SetParameter(4, "%" + token + "%");
                resultList = new ArrayList(query.List());
                if (resultList.Count > 0)
                {
                    foreach (object[] obj in resultList)
                    {
                        PhysicianFacilityDTO objPhy = new PhysicianFacilityDTO();
                        objPhy.PhyId = Convert.ToUInt64(obj[0]);
                        objPhy.PhyPrefix = obj[1].ToString();
                        objPhy.PhyFirstName = obj[2].ToString();
                        objPhy.PhyMiddleName = obj[3].ToString();
                        objPhy.PhyLastName = obj[4].ToString();
                        objPhy.PhySuffix = obj[5].ToString();
                        objPhy.PhySpecialtyCode = Convert.ToString(obj[6]);
                        //objPhy.PhyCity = obj[7].ToString();
                        //objPhy.PhyState = obj[8].ToString();
                        //objPhy.PhyZip = obj[9].ToString();
                        objPhy.PhyNPI = obj[10].ToString();
                        //objPhy.PhySpecialtyID = Convert.ToUInt64(obj[11]);7/2/
                        objPhy.PhySpecialtyID = Convert.ToString(obj[11]);
                        //objPhy.PhyFax = obj[13].ToString();
                        objPhy.PhyAddrs = obj[14].ToString();
                        //objPhy.PhyPhone = obj[15].ToString();
                        if (obj[12] != null)
                        {
                            objPhy.PhyFacility = obj[12].ToString();
                            FacilityManager Fmgr = new FacilityManager();
                            IList<FacilityLibrary> ilstFacilityLibrary = new List<FacilityLibrary>();
                            ilstFacilityLibrary = Fmgr.GetFacilityByFacilityname(obj[12].ToString());
                            if (ilstFacilityLibrary != null && ilstFacilityLibrary.Count > 0)
                            {
                                objPhy.PhyCity = ilstFacilityLibrary[0].Fac_City.ToString();
                                objPhy.PhyState = ilstFacilityLibrary[0].Fac_State.ToString();
                                objPhy.PhyZip = ilstFacilityLibrary[0].Fac_Zip.ToString();
                                objPhy.PhyFax = ilstFacilityLibrary[0].Fac_Fax.ToString();
                                objPhy.PhyPhone = ilstFacilityLibrary[0].Fac_Telephone.ToString();
                            }
                        }
                        else
                        {
                            if (obj[12] != null)
                                objPhy.PhyFacility = obj[12].ToString();
                            if (obj[7] != null)
                                objPhy.PhyCity = obj[7].ToString();
                            if (obj[8] != null)
                                objPhy.PhyState = obj[8].ToString();
                            if (obj[9] != null)
                                objPhy.PhyZip = obj[9].ToString();
                            if (obj[13] != null)
                                objPhy.PhyFax = obj[13].ToString();
                            if (obj[15] != null)
                                objPhy.PhyPhone = obj[15].ToString();
                        }

                        phylst.Add(objPhy);
                    }
                }

                objFindPhy.PhyList = phylst;

                iMySession.Close();
            }
            return objFindPhy;
        }

        public FindPhysican FindPhysicianByID(ulong token)
        {
            IList<PhysicianFacilityDTO> phylst = new List<PhysicianFacilityDTO>();
            FindPhysican objFindPhy = new FindPhysican();
            ArrayList resultList = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Find.PhysicianBy.ID");
                query.SetParameter(0, token);

                //query.SetParameter(3, "%" + token + "%");
                //query.SetParameter(4, "%" + token + "%");
                resultList = new ArrayList(query.List());
                if (resultList.Count > 0)
                {
                    foreach (object[] obj in resultList)
                    {
                        PhysicianFacilityDTO objPhy = new PhysicianFacilityDTO();
                        objPhy.PhyId = Convert.ToUInt64(obj[0]);
                        objPhy.PhyPrefix = obj[1].ToString();
                        objPhy.PhyFirstName = obj[2].ToString();
                        objPhy.PhyMiddleName = obj[3].ToString();
                        objPhy.PhyLastName = obj[4].ToString();
                        objPhy.PhySuffix = obj[5].ToString();
                        objPhy.PhySpecialtyCode = Convert.ToString(obj[6]);
                        //objPhy.PhyCity = obj[7].ToString();
                        //objPhy.PhyState = obj[8].ToString();
                        //objPhy.PhyZip = obj[9].ToString();
                        objPhy.PhyNPI = obj[10].ToString();
                        //objPhy.PhySpecialtyID = Convert.ToUInt64(obj[11]);7/2/
                        objPhy.PhySpecialtyID = Convert.ToString(obj[11]);
                        //objPhy.PhyFax = obj[13].ToString();
                        objPhy.PhyAddrs = obj[14].ToString();
                        //objPhy.PhyPhone = obj[15].ToString();
                        if (obj[12] != null)
                        {
                            objPhy.PhyFacility = obj[12].ToString();
                            FacilityManager Fmgr = new FacilityManager();
                            IList<FacilityLibrary> ilstFacilityLibrary = new List<FacilityLibrary>();
                            ilstFacilityLibrary = Fmgr.GetFacilityByFacilityname(obj[12].ToString());
                            if (ilstFacilityLibrary != null && ilstFacilityLibrary.Count > 0)
                            {
                                objPhy.PhyCity = ilstFacilityLibrary[0].Fac_City.ToString();
                                objPhy.PhyState = ilstFacilityLibrary[0].Fac_State.ToString();
                                objPhy.PhyZip = ilstFacilityLibrary[0].Fac_Zip.ToString();
                                objPhy.PhyFax = ilstFacilityLibrary[0].Fac_Fax.ToString();
                                objPhy.PhyPhone = ilstFacilityLibrary[0].Fac_Telephone.ToString();
                            }
                        }
                        else
                        {
                            if (obj[12] != null)
                                objPhy.PhyFacility = obj[12].ToString();
                            if (obj[7] != null)
                                objPhy.PhyCity = obj[7].ToString();
                            if (obj[8] != null)
                                objPhy.PhyState = obj[8].ToString();
                            if (obj[9] != null)
                                objPhy.PhyZip = obj[9].ToString();
                            if (obj[13] != null)
                                objPhy.PhyFax = obj[13].ToString();
                            if (obj[15] != null)
                                objPhy.PhyPhone = obj[15].ToString();
                        }

                        phylst.Add(objPhy);
                    }
                }

                objFindPhy.PhyList = phylst;

                iMySession.Close();
            }
            return objFindPhy;
        }
        public FindPhysican FindPhysicianFax(string token)
        {
            IList<PhysicianFacilityDTO> phylst = new List<PhysicianFacilityDTO>();
            FindPhysican objFindPhy = new FindPhysican();
            ArrayList resultList = null;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("FindPhysicianFax");
                query.SetParameter(0, "%" + token + "%");
                query.SetParameter(1, "%" + token + "%");
                query.SetParameter(2, "%" + token + "%");
                query.SetParameter(3, "%" + token + "%");
                //query.SetParameter(3, "%" + token + "%");
                //query.SetParameter(4, "%" + token + "%");
                resultList = new ArrayList(query.List());
                if (resultList.Count > 0)
                {
                    foreach (object[] obj in resultList)
                    {
                        PhysicianFacilityDTO objPhy = new PhysicianFacilityDTO();
                        objPhy.PhyId = Convert.ToUInt64(obj[0]);
                        objPhy.PhyPrefix = obj[1].ToString();
                        objPhy.PhyFirstName = obj[2].ToString();
                        objPhy.PhyMiddleName = obj[3].ToString();
                        objPhy.PhyLastName = obj[4].ToString();
                        objPhy.PhySuffix = obj[5].ToString();
                        objPhy.PhySpecialtyCode = Convert.ToString(obj[6]);
                        //objPhy.PhyCity = obj[7].ToString();
                        //objPhy.PhyState = obj[8].ToString();
                        //objPhy.PhyZip = obj[9].ToString();
                        objPhy.PhyNPI = obj[10].ToString();
                        //objPhy.PhySpecialtyID = Convert.ToUInt64(obj[11]);7/2/
                        objPhy.PhySpecialtyID = Convert.ToString(obj[11]);
                        //objPhy.PhyFax = obj[13].ToString();
                        objPhy.PhyAddrs = obj[14].ToString();
                        //objPhy.PhyPhone = obj[15].ToString();
                        if (obj[12] != null)
                        {
                            objPhy.PhyFacility = obj[12].ToString();
                            FacilityManager Fmgr = new FacilityManager();
                            IList<FacilityLibrary> ilstFacilityLibrary = new List<FacilityLibrary>();
                            ilstFacilityLibrary = Fmgr.GetFacilityByFacilityname(obj[12].ToString());
                            if (ilstFacilityLibrary != null && ilstFacilityLibrary.Count > 0)
                            {
                                objPhy.PhyCity = ilstFacilityLibrary[0].Fac_City.ToString();
                                objPhy.PhyState = ilstFacilityLibrary[0].Fac_State.ToString();
                                objPhy.PhyZip = ilstFacilityLibrary[0].Fac_Zip.ToString();
                                objPhy.PhyFax = ilstFacilityLibrary[0].Fac_Fax.ToString();
                                objPhy.PhyPhone = ilstFacilityLibrary[0].Fac_Telephone.ToString();
                            }
                        }
                        else
                        {
                            //Jira Jira CAP-1535
                            //objPhy.PhyFax = obj[13].ToString();
                            //objPhy.PhyPhone = obj[15].ToString();

                            //Jira Jira CAP-1535 - Start
                            if (obj[7] != null)
                                objPhy.PhyCity = obj[7].ToString();
                            if (obj[8] != null)
                                objPhy.PhyState = obj[8].ToString();
                            if (obj[9] != null)
                                objPhy.PhyZip = obj[9].ToString();
                            if (obj[13] != null)
                                objPhy.PhyFax = obj[13].ToString();
                            if (obj[15] != null)
                                objPhy.PhyPhone = obj[15].ToString();
                            //Jira Jira CAP-1535 - End
                        }
                        objPhy.PhyEmail = obj[16].ToString();
                        objPhy.PhyCompany = obj[17].ToString();
                        if (obj[18] != null && obj[18] != "")
                            objPhy.Category = obj[18].ToString();
                        phylst.Add(objPhy);
                    }
                }

                objFindPhy.PhyList = phylst;

                iMySession.Close();
            }
            return objFindPhy;
        }
        public IList<PhysicianLibrary> GetPhysicianListbyType(string PhyType)
        {
            IList<PhysicianLibrary> listPhyLib = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("PhyType", PhyType));
                iMySession.Close();
                listPhyLib = criteria.List<PhysicianLibrary>();
            }
            return listPhyLib;
        }


        ///vince 18-11-11
        public IList<PhysicianLibrary> SaveUpdateDeletePhysicianLibrary(IList<PhysicianLibrary> SaveList, IList<PhysicianLibrary> UpdateList, IList<PhysicianLibrary> DeleteList, string MACAddress)
        {
            SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveList, ref UpdateList, DeleteList, string.Empty, false, false, 0, "");
            IList<PhysicianLibrary> getid = new List<PhysicianLibrary>();
            if (SaveList != null)
            {
                for (int i = 0; i < SaveList.Count; i++)
                {
                    getid.Add(SaveList[i]);
                }
            }
            return getid;
        }


        //public FillPhysicianManager SaveUpdateDeletePhysicianManager(IList<PhysicianLibrary> savePhyLibList, IList<PhysicianLibrary> updatePhyLibList, IList<PhysicianLibrary> deletePhyLibList, IList<PhysicianPOV> savePhyPOVList, IList<PhysicianPOV> updatePhyPOVList, IList<PhysicianPOV> deletePhyPOVList, IList<MapFacilityPhysician> saveMapPhyList, IList<MapFacilityPhysician> updateMapPhyList, IList<MapFacilityPhysician> deleteMapPhyList, string macAddress)
        //{

        //    //ulong EncId = 0, HumanId = 0;
        //    int iTryCount = 0;


        //TryAgain:
        //    int iResult = 0;
        //    ISession MySession = Session.GetISession();
        //    //ITransaction trans = null;
        //    try
        //    {
        //        using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //        {
        //            try
        //            {
        //                //  trans = MySession.BeginTransaction();
        //                GenerateXml XMLObj = null;
        //                if (savePhyLibList != null && savePhyLibList.Count > 0 || updatePhyLibList != null && updatePhyLibList.Count > 0 || deletePhyLibList != null && deletePhyLibList.Count > 0)
        //                {
        //                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savePhyLibList, ref updatePhyLibList, deletePhyLibList, MySession, macAddress, false, false, 0, "", ref XMLObj);
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
        //                            //MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                }

        //                PhysicianPOVManager PhysicianPOVManagerObj = new PhysicianPOVManager();
        //                if (savePhyPOVList != null && savePhyPOVList.Count > 0 || updatePhyPOVList != null && updatePhyPOVList.Count > 0 || deletePhyPOVList != null && deletePhyPOVList.Count > 0)
        //                {
        //                    if (savePhyLibList != null && savePhyLibList.Count > 0)
        //                    {
        //                        for (int i = 0; i < savePhyPOVList.Count; i++)
        //                        {
        //                            savePhyPOVList[i].Phy_ID = savePhyLibList[0].Id;
        //                        }
        //                    }

        //                    iResult = PhysicianPOVManagerObj.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savePhyPOVList, ref updatePhyPOVList, deletePhyPOVList, MySession, macAddress, false, false, 0, "", ref XMLObj);
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
        //                            //MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                }

        //                MapFacilityPhysicianManager MapFacilityPhysicianManagerObj = new MapFacilityPhysicianManager();
        //                if (saveMapPhyList != null && saveMapPhyList.Count > 0 || updateMapPhyList != null && updateMapPhyList.Count > 0 || deleteMapPhyList != null && deleteMapPhyList.Count > 0)
        //                {
        //                    if (saveMapPhyList != null && saveMapPhyList.Count > 0)
        //                    {
        //                        for (int i = 0; i < saveMapPhyList.Count; i++)
        //                        {
        //                            saveMapPhyList[i].Phy_Rec_ID = savePhyLibList[0].Id;
        //                        }
        //                    }


        //                    iResult = MapFacilityPhysicianManagerObj.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveMapPhyList, ref updateMapPhyList, deleteMapPhyList, MySession, macAddress, false, false, 0, "", ref XMLObj);
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
        //                            //MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        // MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                }

        //                MySession.Flush();
        //                trans.Commit();
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
        //    return DTOPhysicianFill(0, 1, 25);

        //}

        //public FillPhysicianManager GetSavedPhyscianByID(ulong ulPhyID)
        //{
        //    FillPhysicianManager objFillPhyDTO = new FillPhysicianManager();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria PhyList = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", ulPhyID));
        //        objFillPhyDTO.PhyLibrary = PhyList.List<PhysicianLibrary>();
        //        ICriteria POVlist = iMySession.CreateCriteria(typeof(PhysicianPOV)).Add(Expression.Eq("Phy_ID", ulPhyID));
        //        objFillPhyDTO.Physician_POV_List = POVlist.List<PhysicianPOV>();
        //        ICriteria MapList = iMySession.CreateCriteria(typeof(MapFacilityPhysician)).Add(Expression.Eq("Phy_Rec_ID", ulPhyID));
        //        objFillPhyDTO.MapFacilityPhysicianList = MapList.List<MapFacilityPhysician>();
        //        iMySession.Close();
        //    }
        //    return objFillPhyDTO;
        //}
        //Added by suresh For RCM
        public IList<PhysicianLibrary> LoadPhysicianList()
        {
            IList<PhysicianLibrary> objPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(PhysicianLibrary));
                objPhysicianLibrary = criteria.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return objPhysicianLibrary;
        }
        //Added by suresh For RCM

        //added by Gopal For RCM_20131024
        //public ArrayList GetPhyisicianList()
        //{
        //    ArrayList arrayList = new ArrayList();
        //    IList<Physician_Company> PhyList = new List<Physician_Company>();
        //    IList<Physician_Company> PhyListReturn = new List<Physician_Company>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ISQLQuery sq = iMySession.CreateSQLQuery("SELECT * FROM physician_company p group by Physician_Library_ID").AddEntity("p", typeof(Physician_Company));
        //        PhyList = sq.List<Physician_Company>();

        //        for (int iCount = 0; iCount < PhyList.Count; iCount++)
        //        {

        //            IQuery query1 = iMySession.GetNamedQuery("Get.PhysicianList.GetList");
        //            query1.SetString(0, PhyList[iCount].Physician_Library_ID.ToString());
        //            arrayList.Add(query1.List());
        //        }
        //        iMySession.Close();
        //    }
        //    return arrayList;
        //}

        //public IList<Physician_Company> GetphysicianCompanyByPhyID(ulong ulBillID, ulong ulRendProvID, string sFacilityName)
        //{
        //    IList<Physician_Company> listPhyComp = new List<Physician_Company>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria criteria = iMySession.CreateCriteria(typeof(Physician_Company)).Add(Expression.Eq("Physician_Library_ID", ulRendProvID)).Add(Expression.Eq("Company_ID", ulBillID)).Add(Expression.Eq("Facility_Name", sFacilityName));
        //        listPhyComp = criteria.List<Physician_Company>();
        //        iMySession.Close();
        //    }
        //    return listPhyComp;
        //}
        public IList<PhysicianLibrary> GetPhysicianByNPI(string NPI)
        {
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria PhyList = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("PhyNPI", NPI));
                ilstPhysicianLibrary = PhyList.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return ilstPhysicianLibrary;
        }
        public IList<PhysicianLibrary> GetPCPNameByPatInsuredId(int PatInsurPlanId)
        {

            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetPcpName");
                query1.SetParameter(0, PatInsurPlanId);
                ArrayList arrayList = new ArrayList(query1.List());
                PhysicianLibrary phyLibrary = new PhysicianLibrary();
                if (arrayList != null && arrayList.Count > 0)
                {
                    foreach (object[] obj in arrayList)
                    {
                        phyLibrary.PhyLastName = obj[0].ToString();
                        //phyLibrary.Policy_Holder_ID = obj[1].ToString();
                        phyLibrary.Id = Convert.ToUInt64(obj[3].ToString());
                        //phyLibrary.Insurance_Plan_ID = Convert.ToInt32(obj[2].ToString());

                        PhyList.Add(phyLibrary);
                    }

                }
                iMySession.Close();
            }
            return PhyList;


        }

        //added by srividhya For RCM_20140718
        //public ArrayList GetPhyisicianListbyFacilityName(string sFacilityName)
        //{
        //    ArrayList arrayList = new ArrayList();
        //    IList<PhysicianFacilityCompanyCarrier> PhyList = new List<PhysicianFacilityCompanyCarrier>();
        //    IList<PhysicianFacilityCompanyCarrier> PhyListReturn = new List<PhysicianFacilityCompanyCarrier>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ISQLQuery sq = iMySession.CreateSQLQuery("SELECT * FROM physician_facility_company_carrier p where p.Facility_name='" + sFacilityName + "' group by Physician_Library_ID").AddEntity("p", typeof(PhysicianFacilityCompanyCarrier));
        //        PhyList = sq.List<PhysicianFacilityCompanyCarrier>();

        //        for (int iCount = 0; iCount < PhyList.Count; iCount++)
        //        {
        //            IQuery query1 = iMySession.GetNamedQuery("Get.PhysicianList.GetList");
        //            query1.SetString(0, PhyList[iCount].Physician_Library_ID.ToString());
        //            arrayList.Add(query1.List());
        //        }
        //        iMySession.Close();
        //    }
        //    return arrayList;

        //}


        public ulong SavePhysicanforSummary(IList<PhysicianLibrary> lstphysician)
        {
            IList<PhysicianLibrary> nullList = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstphysician, ref nullList, null, string.Empty, false, false, 0, "");
            return Convert.ToUInt64(lstphysician[0].Id);
        }

        //added by Balaji.TJ For RCM_2014-08-21
        public IList<PhysicianLibrary> Get_PhysicianList()
        {
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("PhyType", "rendering"));
                ilstPhysicianLibrary = criteria.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return ilstPhysicianLibrary;
        }
        public IList<PhysicianLibrary> Get_PhysicianList(string proviserid)
        {
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", Convert.ToUInt64(proviserid)));
                ilstPhysicianLibrary = criteria.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return ilstPhysicianLibrary;
        }

        //added by srividhya on 14-Aug-2014
        public IList<PhysicianLibrary> GetRenderingPhyisicianList()
        {
            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq1 = iMySession.CreateSQLQuery("SELECT * FROM physician_library where Physician_Type='RENDERING'").AddEntity("l", typeof(PhysicianLibrary));
                PhyList = sq1.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return PhyList;
        }

        public IList<PhysicianLibrary> GetphysiciannameByUserName(string sUserName)
        {
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sq1 = iMySession.CreateSQLQuery("SELECT p.* FROM physician_library p, User u  where p.Physician_Library_ID=u.Physician_Library_ID and u.User_Name='" + sUserName + "'").AddEntity("p", typeof(PhysicianLibrary));
                ilstPhysicianLibrary = sq1.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return ilstPhysicianLibrary;
            //return criteria.List<PhysicianLibrary>();
        }

        public IList<FillPhysicianLibrary> GetPhysicianByCategory(IList<string> Category)
        {
            ArrayList FavoriteList = null;
            IList<FillPhysicianLibrary> ilstPhysicianLibraryCategory = new List<FillPhysicianLibrary>();
            string[] sValue = new string[Category.Count];
            if (Category.Count > 0)
            {
                for (int i = 0; i < Category.Count; i++)
                {
                    sValue[i] = Category[i];
                }

            }

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery FillPhysicianLibraryByCategory = iMySession.GetNamedQuery("GetFillPhysicianLibraryByCategory");
                FillPhysicianLibraryByCategory.SetParameterList("Category", sValue);
                FavoriteList = new ArrayList(FillPhysicianLibraryByCategory.List());
                ilstPhysicianLibraryCategory = FillDTO(FavoriteList);
            }
            return ilstPhysicianLibraryCategory;
        }

        public IList<PhysicianLibrary> GetPhysicianByFax(string Fax)
        {
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria PhyList = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Category", Fax));
                ilstPhysicianLibrary = PhyList.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return ilstPhysicianLibrary;
        }

        private IList<FillPhysicianLibrary> FillDTO(ArrayList FavoriteList)
        {
            IList <FillPhysicianLibrary> ilstPhysicianLibraryCategory = new List<FillPhysicianLibrary>();
            for (int i = 0; i < FavoriteList.Count; i++)
            {

                object[] oj = (object[])FavoriteList[i];

                FillPhysicianLibrary Phylbr = new FillPhysicianLibrary();
                Phylbr.Category = oj[0].ToString();
                Phylbr.Physician_prefix = oj[1].ToString();
                Phylbr.Physician_First_Name = oj[2].ToString();
                Phylbr.Physician_Middle_Name = oj[3].ToString();
                Phylbr.Physician_Last_Name = oj[4].ToString();
                Phylbr.Physician_Suffix = oj[5].ToString();
                if (oj[6] != null)
                    Phylbr.Specialties = oj[6].ToString();
                Phylbr.Physician_NPI = oj[7].ToString();
                if (oj[8] != null)
                    Phylbr.Facility_Name = oj[8].ToString();
                Phylbr.Physician_Library_ID = oj[9].ToString();
                Phylbr.Physician_Type = oj[10].ToString();
                Phylbr.Company = oj[11].ToString();
                Phylbr.Physician_Address1 = oj[12].ToString();
                Phylbr.Physician_Address2 = oj[13].ToString();
                Phylbr.Physician_City = oj[14].ToString();
                Phylbr.Physician_State = oj[15].ToString();
                Phylbr.Physician_Zip = oj[16].ToString();
                Phylbr.Physician_Telephone = oj[17].ToString();
                Phylbr.Physician_Fax = oj[18].ToString();
                Phylbr.Physician_EMail = oj[19].ToString();
                ilstPhysicianLibraryCategory.Add(Phylbr);
            }

            return ilstPhysicianLibraryCategory;
        }

        //public IList<PhysicianFacilityCompanyCarrier> GetPhyFacCompanyCarrierDetails(ulong ulBillID, ulong ulRendProvID, string sBillingFacility, ulong ulCarrierID)
        //{
        //    ICriteria criteria;
        //    IList<PhysicianFacilityCompanyCarrier> PhyFacCompCarList = new List<PhysicianFacilityCompanyCarrier>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        if (ulCarrierID > 0)
        //        {
        //            criteria = iMySession.CreateCriteria(typeof(PhysicianFacilityCompanyCarrier)).Add(Expression.Eq("Company_ID", ulBillID)).Add(Expression.Eq("Rendering_Provider_ID", ulRendProvID)).Add(Expression.Eq("Facility_Name", sBillingFacility)).Add(Expression.Eq("Carrier_Reference_ID", ulCarrierID.ToString()));
        //            PhyFacCompCarList = criteria.List<PhysicianFacilityCompanyCarrier>();
        //        }
        //        else
        //        {
        //            criteria = iMySession.CreateCriteria(typeof(PhysicianFacilityCompanyCarrier)).Add(Expression.Eq("Company_ID", ulBillID)).Add(Expression.Eq("Rendering_Provider_ID", ulRendProvID)).Add(Expression.Eq("Facility_Name", sBillingFacility)).Add(Expression.Eq("Carrier_Reference_ID", "ALL"));
        //            PhyFacCompCarList = criteria.List<PhysicianFacilityCompanyCarrier>();
        //        }
        //        iMySession.Close();
        //    }
        //    return PhyFacCompCarList;
        //}

        #endregion
    }
   
}

