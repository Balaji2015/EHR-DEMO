using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System;
using Acurus.Capella.Core.DTO;
using System.Linq;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IPhysicianProcedureManager : IManagerBase<PhysicianProcedure, ulong>
    {
        //IList<PhysicianProcedure> GetProceduresUsingPhysicianID(ulong PhysicianID, string procedureType);
        //FillLookupDetails InsertToPhysicianProcedure(IList<PhysicianProcedure> saveList, IList<PhysicianProcedure> delList, ulong PhysicianID, string procedureType, string MACAddress);
        IList<PhysicianProcedure> GetProceduresUsingPhysicianIDAndLabID(ulong PhysicianID, string procedureType, ulong LabID,string sLegalOrg);
        //IList<PhysicianProcedure> LoadPhysicianProcedureFromServer(ulong Physician_ID, string[] Field_Names);
        //int SaveintoPhysicianProcedure(IList<PhysicianProcedure> SavePhyProcedure, IList<PhysicianProcedure> UpdatePhyProcedure, IList<PhysicianProcedure> DeletePhyProcedure, ISession Mysession);
        //FillLookupDetails SaveUpdateDeletePhysicianProcedure(IList<PhysicianProcedure> SavePhyProcLst, IList<PhysicianProcedure> UpdatePhyProcLst, IList<PhysicianProcedure> DeletePhyProcLst, string sLastModified, string sMacAddress);
        //IList<PhysicianProcedure> GetProceduresUsingPhysicianIDForResultEntry(ulong PhysicianID, string procedureType);
        //IList<PhysicianProcedure> GetPhyCPTs(ulong phyID);
        void SaveUpdateDeleteLabProcedure(IList<PhysicianProcedure> ilstSaveList, IList<PhysicianProcedure> ilstDeleteList);
        IList<PhysicianProcedure> GetProceduresUsingPhysicianIDAndGroupname(ulong PhysicianID, string Groupname, ulong LabID, string sLegalOrg);
        //IList<PhysicianProcedure> GetProceduresUsingPhysicianIdAndTypeOfVisit(ulong PhysicianID, string type_of_visit);

    }
    public partial class PhysicianProcedureManager : ManagerBase<PhysicianProcedure, ulong>, IPhysicianProcedureManager
    {
        #region Constructors

        public PhysicianProcedureManager()
            : base()
        {

        }
        public PhysicianProcedureManager
            (INHibernateSession session)
            : base(session)
        {
        }

        #endregion

        #region CRUDMethods


        //public FillLookupDetails InsertToPhysicianProcedure(IList<PhysicianProcedure> saveList, IList<PhysicianProcedure> delList, ulong PhysicianID, string procedureType, string MACAddress)
        //{
        //    FillLookupDetails objFillLookup = new FillLookupDetails();
        //    //ulong HumanId = 0;
        //    //IList<PhysicianProcedure> updateList = null;
        //    //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
        //    //SaveUpdateDeleteWithTransaction(ref templist, null, deletelist, MACAddress);
        //    //SaveUpdateDeleteWithTransaction(ref saveList, null, delList, MACAddress);
        //    if (saveList != null && saveList.Count > 0)
        //    {
        //        IList<PhysicianProcedure> PhyLst = new List<PhysicianProcedure>();
        //        foreach (PhysicianProcedure p in saveList)
        //        {
        //            PhyLst.Add(p);
        //        }
        //    }

        //    return objFillLookup;
        //}
        #endregion

        #region GetMethods

        //public IList<PhysicianProcedure> GetProceduresUsingPhysicianID(ulong PhysicianID, string procedureType)
        //{
        //    IList<PhysicianProcedure> listProc = new List<PhysicianProcedure>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Procedure_Type", procedureType))
        //            .AddOrder(Order.Asc("Sort_Order"));
        //        listProc = criteria.List<PhysicianProcedure>();
        //        iMySession.Close();

        //    }
        //    return listProc;
        //}

        public IList<PhysicianProcedure> GetProceduresUsingPhysicianIdAndTypeOfVisit(ulong PhysicianID, string type_of_visit, string sLegalOrg)
        {
            IList<PhysicianProcedure> listProc = new List<PhysicianProcedure>();
            ICriteria criteria;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Type_Of_Visit", type_of_visit)).Add(Expression.Eq("Procedure_Type", "E AND M PROCEDURE")).AddOrder(Order.Asc("Order_Group_Name")).AddOrder(Order.Asc("Sort_Order")); //Commented for Bug ID : 55634 
                if (PhysicianID != 0)
                {
                    criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Type_Of_Visit", type_of_visit)).Add(Expression.Eq("Procedure_Type", "E AND M PROCEDURE")).Add(Expression.Eq("Legal_Org",sLegalOrg)).AddOrder(Order.Asc("Sort_Order"));
                    listProc = criteria.List<PhysicianProcedure>();
                    if (listProc.Count == 0)
                    {
                        //criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Type_Of_Visit", "ALL")).Add(Expression.Eq("Procedure_Type", "E AND M PROCEDURE")).AddOrder(Order.Asc("Order_Group_Name")).AddOrder(Order.Asc("Sort_Order")); //Commented for Bug ID : 55634 
                        criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Type_Of_Visit", "ALL")).Add(Expression.Eq("Procedure_Type", "E AND M PROCEDURE")).Add(Expression.Eq("Legal_Org", sLegalOrg)).AddOrder(Order.Asc("Sort_Order"));
                        listProc = criteria.List<PhysicianProcedure>();
                    }
                }
                else
                {
                    ISQLQuery sq = iMySession.CreateSQLQuery("select * from physician_procedure where type_of_visit='" + type_of_visit + "' and physician_id in (select max(physician_id) from physician_procedure where type_of_visit='" + type_of_visit + "') and Legal_Org ='"+ sLegalOrg+"'").AddEntity(typeof(PhysicianProcedure));
                    listProc = sq.List<PhysicianProcedure>();
                }
                iMySession.Close();
            }
            return listProc;
        }




        public IList<PhysicianProcedure> GetProceduresUsingPhysicianIDAndLabID(ulong PhysicianID, string procedureType, ulong LabID, string sLegalOrg)
        {

            IList<PhysicianProcedure> ilstPhysicianProcedure = new List<PhysicianProcedure>();
            ICriteria criteria;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (LabID != 0)
                    criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Lab_ID", LabID)).Add(Expression.Eq("Legal_Org", sLegalOrg)).AddOrder(Order.Asc("Sort_Order"));

                else
                    criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Procedure_Type", procedureType)).Add(Expression.Eq("Lab_ID", LabID)).Add(Expression.Eq("Legal_Org", sLegalOrg)).AddOrder(Order.Asc("Sort_Order"));
                ilstPhysicianProcedure = criteria.List<PhysicianProcedure>();
                iMySession.Close();

            }
            return ilstPhysicianProcedure;
        }

        public IList<PhysicianProcedure> GetProceduresUsingPhysicianIDAndGroupname(ulong PhysicianID, string Groupname, ulong LabID, string sLegalOrg)
        {

            IList<PhysicianProcedure> ilstPhysicianProcedure = new List<PhysicianProcedure>();
            ICriteria criteria;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {                
                criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", PhysicianID)).Add(Expression.Eq("Order_Group_Name", Groupname)).Add(Expression.Eq("Lab_ID", LabID)).Add(Expression.Eq("Legal_Org", sLegalOrg)).AddOrder(Order.Asc("Sort_Order"));
                ilstPhysicianProcedure = criteria.List<PhysicianProcedure>();
                iMySession.Close();

            }
            return ilstPhysicianProcedure;
        }
        //public IList<PhysicianProcedure> GetProcedures(ulong PhysicianID, string procedureType, ulong LabID, string sLegalOrg)
        //{
        //    IList<PhysicianProcedure> temp = new List<PhysicianProcedure>();
        //    temp = GetProceduresUsingPhysicianIDAndLabID(PhysicianID, procedureType, LabID,sLegalOrg);
        //    return (temp).Any(a => a.Sort_Order == 0) ? (temp).Where(a => a.Sort_Order != 0).ToList().Concat(temp.Where(a => a.Sort_Order == 0).OrderBy(b => b.Procedure_Description)).ToList() : temp;
        //}

        #endregion

        #region IPhysicianProcedureManager Members


        //public IList<PhysicianProcedure> LoadPhysicianProcedureFromServer(ulong Physician_ID, string[] Field_Names, string sLegalOrg)
        //{
        //    IList<PhysicianProcedure> listProc = new List<PhysicianProcedure>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria criteria = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", Physician_ID)).Add(Expression.In("Procedure_Type", Field_Names)).Add(Expression.Eq("Legal_Org", sLegalOrg));
        //        listProc = criteria.List<PhysicianProcedure>();
        //        iMySession.Close();

        //    }
        //    return listProc;
        //}




        //public int SaveintoPhysicianProcedure(IList<PhysicianProcedure> SavePhyProcedure, IList<PhysicianProcedure> UpdatePhyProcedure, IList<PhysicianProcedure> DeletePhyProcedure, ISession Mysession)
        //{
        //    GenerateXml XMLObj = new GenerateXml();
        //    //XMLObj=string.Empty;
        //    return SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SavePhyProcedure, ref UpdatePhyProcedure, DeletePhyProcedure, Mysession, string.Empty, false, false, 0, string.Empty, ref XMLObj);
        //    //return SaveUpdateDeleteWithoutTransaction(ref SavePhyProcedure, UpdatePhyProcedure, DeletePhyProcedure, Mysession, "");
        //}






        //public FillLookupDetails SaveUpdateDeletePhysicianProcedure(IList<PhysicianProcedure> SavePhyProcLst, IList<PhysicianProcedure> UpdatePhyProcLst, IList<PhysicianProcedure> DeletePhyProcLst, string sLastModified, string sMacAddress)
        //{
        //    FillLookupDetails objFillLookup = new FillLookupDetails();
        //    //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
        //    //SaveUpdateDeleteWithTransaction(ref SavePhyProcLst, UpdatePhyProcLst, DeletePhyProcLst, "");
        //    //UserRelatedModificationManager objUserRelatedModificationMgr = new UserRelatedModificationManager();
        //    //objFillLookup.User_Related = objUserRelatedModificationMgr.GetAllLookupTablesFromServer(sLastModified, sMacAddress);
        //    objFillLookup.UTC_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
        //    //objFillLookup.UTC_Time = objFillLookup.User_Related.Count > 0 ? objFillLookup.User_Related.Max(s => s.Last_Modified_Time).ToString("yyyy-MM-dd HH:mm:ss") : System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");

        //    return objFillLookup;
        //}


        //public IList<PhysicianProcedure> GetProceduresUsingPhysicianIDForResultEntry(ulong PhysicianID, string procedureType)
        //{
        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    IList<PhysicianProcedure> physicianProcList = new List<PhysicianProcedure>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ISQLQuery sq = iMySession.CreateSQLQuery("Select * from Physician_Procedure where Physician_ID=" + PhysicianID + " and Procedure_Type='" + procedureType + "' group  by Physician_Procedure_Code").AddEntity(typeof(PhysicianProcedure));
        //        physicianProcList = sq.List<PhysicianProcedure>();
        //        iMySession.Close();
        //    }
        //    return physicianProcList;
        //}
        public IList<PhysicianProcedure> GetPhyCPTs(ulong phyID,string sLegalOrg)
        {
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<PhysicianProcedure> PhysicianCPTList = new List<PhysicianProcedure>();

            ICriteria critPhyCPTList = iMySession.CreateCriteria(typeof(PhysicianProcedure)).Add(Expression.Eq("Physician_ID", phyID)).Add(Expression.Eq("Procedure_Type", "E AND M PROCEDURE")).Add(Expression.Eq("Legal_Org", sLegalOrg)).AddOrder(Order.Asc("Sort_Order"));

            PhysicianCPTList = critPhyCPTList.List<PhysicianProcedure>();

            iMySession.Close();

            return PhysicianCPTList;
        }

        public void SaveUpdateDeleteLabProcedure(IList<PhysicianProcedure> ilstSaveList, IList<PhysicianProcedure> ilstDeleteList)
        {
            IList<PhysicianProcedure> updatelist = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstSaveList, ref updatelist, ilstDeleteList, string.Empty, false, false, 0, string.Empty);
            //SaveUpdateDeleteWithTransaction(ref ilstSaveList, null, ilstDeleteList, string.Empty);
        }
        #endregion
    }
}
