using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Criterion;


namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public interface IPQRI_DataManager : IManagerBase<PQRI_Data, int>
    {
        IList<PQRI_Data> GetPQRILookupDetails();
        IList<PQRI_Data> GetPQRIListBySection();
        IList<PQRI_Data> GetPQRIListByStandardConcept(string value);
        IList<PQRI_Data> GetPQRIListByStandardConceptAndPQRIType(string StandardConcept, string PQRI_Type);
        IList<PQRI_Data> GetPQRIListBySectionForStage3();
    }

    public partial class PQRI_DataManager : ManagerBase<PQRI_Data, int>, IPQRI_DataManager
    {
         #region Constructors
        
        public PQRI_DataManager()
            : base()
        {

        }
        public PQRI_DataManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        public IList<PQRI_Data> GetPQRILookupDetails()
        {
            IList<PQRI_Data> listGetPQRI_Lookup = new List<PQRI_Data>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria critGetPQRI_LookupDetails =iMySession.CreateCriteria(typeof(PQRI_Data));
                listGetPQRI_Lookup = critGetPQRI_LookupDetails.List<PQRI_Data>();
                iMySession.Close();
            }
            return listGetPQRI_Lookup;

        }
        public IList<PQRI_Data> GetPQRIListBySection()
        {
             IList<PQRI_Data> listGetPQRI_Lookup = new List<PQRI_Data>();
             using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
             {
                 ICriteria critGetPQRI_LookupDetails =iMySession.CreateCriteria(typeof(PQRI_Data));//.Add(Expression.NotEqProperty("PQRI_Selection_XML", ""));
                 listGetPQRI_Lookup = critGetPQRI_LookupDetails.List<PQRI_Data>().Where(a => a.PQRI_Selection_XML.Trim() != string.Empty).ToList<PQRI_Data>();
                 iMySession.Close();
             }
            return listGetPQRI_Lookup;


        }

        public IList<PQRI_Data> GetPQRIListBySectionForStage3()
        {

           
           
         
            IList<PQRI_Data> listGetPQRI_Lookup = new List<PQRI_Data>();
            //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            //{
            //    ICriteria critGetPQRI_LookupDetails = iMySession.CreateCriteria(typeof(PQRI_Data));//.Add(Expression.NotEqProperty("PQRI_Selection_XML", ""));
            //    listGetPQRI_Lookup = critGetPQRI_LookupDetails.List<PQRI_Data>().Where(a => a.PQRI_Selection_XML.Trim() != string.Empty && a.Sort_Order.ToString()=="3").ToList<PQRI_Data>();
            //    iMySession.Close();
            //}
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            //ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from cqm_data e where e.Sort_Order  =3 and e.Section_XML!=''").AddEntity("e", typeof(PQRI_Data));
            ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from cqm_data e where e.Section_XML!=''").AddEntity("e", typeof(PQRI_Data));
            listGetPQRI_Lookup = sqlquery.List<PQRI_Data>();


            return listGetPQRI_Lookup;


        }


        public IList<PQRI_Data> GetPQRIListByStandardConcept(string value)
        {
             IList<PQRI_Data> listGetPQRI_Lookup = new List<PQRI_Data>();
             using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
             {
                 ICriteria crit =iMySession.CreateCriteria(typeof(PQRI_Data)).Add(Expression.Eq("Standard_concept", value));
                 listGetPQRI_Lookup= crit.List<PQRI_Data>();
                 iMySession.Close();
             }
             return listGetPQRI_Lookup;
        }

        public IList<PQRI_Data> GetPQRIListByStandardConceptAndPQRIType(string StandardConcept, string PQRI_Type)
        {
            IList<PQRI_Data> listGetPQRI_Lookup = new List<PQRI_Data>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(PQRI_Data)).Add(Expression.Eq("Standard_concept", StandardConcept)).Add(Expression.Eq("NQF_Number", PQRI_Type));
                listGetPQRI_Lookup = crit.List<PQRI_Data>();
                iMySession.Close();
            }
            return listGetPQRI_Lookup;
        }

        public IList<PQRI_Data> GetPQRIListByStandardConceptList(IList<string> Standard_concept)
        {
            IList<PQRI_Data> listGetPQRI_Lookup = new List<PQRI_Data>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(PQRI_Data)).Add(Expression.InG("Standard_concept", Standard_concept));
                listGetPQRI_Lookup = crit.List<PQRI_Data>();
                iMySession.Close();
            }
            return listGetPQRI_Lookup;
        }


    }
}
