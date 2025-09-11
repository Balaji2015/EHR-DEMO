using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System.Collections;
using System;
using System.Linq;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public interface IStaticLookupManager : IManagerBase<StaticLookup, ulong>
    {
        IList<StaticLookup> getStaticLookup();
        IList<StaticLookup> getStaticLookupByFieldName(string Field_Name);
        IList<StaticLookup> getStaticLookupByFieldName(string Field_Name, string DocType, string SubDocType, string sOrder);
        IList<StaticLookup> getStaticLookupByFieldName(string[] Field_Name);
        IList<StaticLookup> getStaticLookupByFieldName(string Field_Name, string sOrder);
        IList<StaticLookup> getStaticLookupByDocType(string DocType, string sOrder);

        IList<StaticLookup> getStaticLookupByFieldNameandDescription(string Field_Name, string Description);

        //Added by srividhya on 12-12-2013

        IList<StaticLookup> GetStaticLookupbyDocTypeAndDocSubType(string Field_Name, string DocType, string SubDocType, string sOrder);

        string GetDescriptionByMessageCode(string MessageCode);

        ArrayList GetPOSByFieldName(string Field_Name, string sOrder);
        IList<StaticLookup> getStaticLookupByFieldNameSortOrder(string sFieldName, string sOrder);

        //Added by Saravanakumar for MuEhr
        IList<StaticLookup> getStaticLookupByFieldNameAndValue(string sFieldName, string sValue);
        IList<StaticLookup> GetLookupValuesbyDocTypeAndDocSubType(string Field_Name,string DocType,string SubDocType);
        IList<StaticLookup> getStaticLookupCategoryByFieldName(string Field_Name, string DocType, string SubDocType);
        IList<StaticLookup> GetLookupValuesbyDocTypeSubTypeAndDefaultvalue(string Field_Name, string DocType, string SubDocType,string Value);
        IList<ReasonCodeLookup> GetReasonCodeByDocTypeAndSubType(string FieldName, string Doc_Type, string Sub_Type);
        //IList<ReasonCodeLookup> GetSubcodesDescription(string DocType, string DocSubType, string SubCode);
        IList<ReasonCodeLookup> GetDescription(List<string> Codes, string Doc_Type, string Sub_Type);
        IList<ReasonCodeLookup> GetSubcodeFromReasonCode(string ReasonCode, string DocType, string DocSubType);
        IList<StaticLookup> GetLookupValuesbyDocTypeSubTypeAndvalue(string Field_Name, string DocType, string SubDocType, string Value);
        IList<StaticLookup> getStaticLookupByFieldNameAndSorder(string Field_Name, string sOrder);
        IList<StaticLookup> getStaticLookupByFieldName(string[] Field_Name, string sOrder);
        IList<StaticLookup> GetStaticLookupByFieldNameAndPartialValue(string sFieldName, string sValue);
    }
    public partial class StaticLookupManager : ManagerBase<StaticLookup, ulong>, IStaticLookupManager
    {
        public StaticLookupManager()
            : base()
        {

        }
        public StaticLookupManager(INHibernateSession session)
            : base(session)
        {

        }

        public IList<StaticLookup> getStaticLookup()
        {
            return GetAll();
        }
        public IList<StaticLookup> getStaticLookupByFieldName(string Field_Name)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).AddOrder(Order.Asc("Sort_Order"));;

                staticList = criteria.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }
        //BugID:47880
        public IList<StaticLookup> getStaticLookupByFieldNameDefaultValue(string Field_Name,string Default_Value)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Like("Default_Value",Default_Value)).AddOrder(Order.Asc("Sort_Order"));
                staticList = criteria.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }
        public IList<StaticLookup> getStaticLookupByFieldNameForreason(string Field_Name)
        {
            IList<StaticLookup> ilstEncounter = new List<StaticLookup>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from static_lookup e where e.field_name  ='" + Field_Name + "' and e.doc_type='MACRA' and e.doc_sub_type='FREQUENTLY USED' order by sort_order asc").AddEntity("e", typeof(StaticLookup));
            ilstEncounter = sqlquery.List<StaticLookup>();
            return ilstEncounter;
        }
        public IList<StaticLookup> getStaticLookupByFieldName(string[] Field_Name)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.In("Field_Name", Field_Name));
                 staticList = criteria.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }

        public IList<StaticLookup> getStaticLookupByFieldName(string[] Field_Name, string sOrder)
        {
           // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
             //CAP-2498
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.In("Field_Name", Field_Name.Where(x=>x != null).ToArray())).AddOrder(Order.Asc(sOrder));
                staticList = criteria.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }


        public IList<StaticLookup> getStaticLookupByFieldName(string Field_Name, string DocType, string SubDocType, string sOrder)
        {
           // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
               IList<StaticLookup> staticList = new List<StaticLookup>();
               using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
               {
                   ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", DocType.ToUpper())).Add(Expression.Eq("Doc_Sub_Type", SubDocType.ToUpper())).AddOrder(Order.Asc(sOrder));

                   staticList = criteria.List<StaticLookup>();
                   iMySession.Close();
               }
            return staticList;
        }
        public IList<StaticLookup> getStaticLookupCategoryByFieldName(string Field_Name, string DocType, string SubDocType)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
               IList<StaticLookup> staticList = new List<StaticLookup>();
               using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
               {
                   ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", DocType.ToUpper())).Add(Expression.Eq("Doc_Sub_Type", SubDocType.ToUpper()));
                   staticList = criteria.List<StaticLookup>();
                   iMySession.Close();
               }
            return staticList;
        }

        public IList<StaticLookup> getStaticLookupByFieldName(string Field_Name, string sOrder)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
               IList<StaticLookup> staticList = new List<StaticLookup>();
               using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
               {
                   ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).AddOrder(Order.Asc(sOrder));

                   staticList = criteria.List<StaticLookup>();
                   iMySession.Close();
               }
            return staticList;
        }

        //public IList<StaticLookup> getStaticLookupByFieldName(string[] Field_Name, string sOrder)
        //{
        //    ICriteria criteria = session.GetISession().CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name));
        //    return criteria.List<StaticLookup>();
        //}

        public IList<StaticLookup> getStaticLookupByDocType(string DocType, string sOrder)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
               IList<StaticLookup> staticList = new List<StaticLookup>();
               using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
               {
                   ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Doc_Type", DocType.ToUpper())).AddOrder(Order.Asc(sOrder));
                   staticList = criteria.List<StaticLookup>();
                   iMySession.Close();
               }
            return staticList;
        }

        //Added by srividhya on 12-12-2013
        public IList<StaticLookup> GetStaticLookupbyDocTypeAndDocSubType(string Field_Name, string DocType, string SubDocType, string sOrder)
        {
            IList<StaticLookup> StaticLookupList = new List<StaticLookup>();
            IList<StaticLookup> FinalStaticLookupList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (DocType != "ALL" && SubDocType != "ALL")
                {
                    ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", "ALL")).Add(Expression.Eq("Doc_Sub_Type", "ALL")).AddOrder(Order.Asc(sOrder));
                    StaticLookupList = criteria.List<StaticLookup>();

                    for (int i = 0; i < StaticLookupList.Count; i++)
                    {
                        FinalStaticLookupList.Add(StaticLookupList[i]);
                    }

                    ICriteria criteria1 = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", DocType.ToUpper())).Add(Expression.Eq("Doc_Sub_Type", "ALL")).AddOrder(Order.Asc(sOrder));
                    StaticLookupList = criteria1.List<StaticLookup>();

                    for (int i = 0; i < StaticLookupList.Count; i++)
                    {
                        FinalStaticLookupList.Add(StaticLookupList[i]);
                    }

                    ICriteria criteria2 = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", DocType.ToUpper())).Add(Expression.Eq("Doc_Sub_Type", SubDocType.ToUpper())).AddOrder(Order.Asc(sOrder));
                    StaticLookupList = criteria2.List<StaticLookup>();

                    for (int i = 0; i < StaticLookupList.Count; i++)
                    {
                        FinalStaticLookupList.Add(StaticLookupList[i]);
                    }
                }
                else
                {
                    ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", "ALL")).Add(Expression.Eq("Doc_Sub_Type", "ALL")).AddOrder(Order.Asc(sOrder));
                    StaticLookupList = criteria.List<StaticLookup>();

                    for (int i = 0; i < StaticLookupList.Count; i++)
                    {
                        FinalStaticLookupList.Add(StaticLookupList[i]);
                    }
                }
                iMySession.Close();
            }

            return FinalStaticLookupList;
        }

        public IList<StaticLookup> GetStaticLookupByFieldNameAndValue(string Field_Name, string sValue, string DocType, string SubDocType, string sOrder)
        {
           // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Value", sValue)).Add(Expression.Eq("Doc_Type", DocType.ToUpper())).Add(Expression.Eq("Doc_Sub_Type", SubDocType.ToUpper())).AddOrder(Order.Asc(sOrder));
                 staticList = criteria.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }

        public IList<StaticLookup> getStaticLookupByFieldName(string Field_Name, ulong phyId)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
             IList<StaticLookup> staticList = new List<StaticLookup>();
             using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
             {
                 ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Description", phyId.ToString()));
                 staticList = criteria.List<StaticLookup>();
                 iMySession.Close();
             }
            return staticList;
        }
        public IList<StaticLookup> getStaticLookupByFieldNameforReport(string Field_Name, string StageName)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
               IList<StaticLookup> staticList = new List<StaticLookup>();
               using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
               {
                   ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", StageName));
                   staticList = criteria.List<StaticLookup>();
                   iMySession.Close();
               }
            return staticList;
        }

        public string GetDescriptionByMessageCode(string MessageCode)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();             
            IList<StaticLookup> ilstStaticLokkUp = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Value", MessageCode.ToUpper()));
                if (criteria.List<StaticLookup>().Count > 0)
                {
                    string sDesc = criteria.List<StaticLookup>()[0].Description;
                    iMySession.Close();
                    return sDesc;
                }
            }
            string sValue = "";
            return sValue;
        }


        public IList<StaticLookup> getStaticLookupByFieldNameandDescription(string Field_Name, string Description)
        {
           // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Description", Description));
                staticList = criteria.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }

        public ArrayList GetPOSByFieldName(string Field_Name, string sOrder)
        {
            IList<StaticLookup> LookupList = new List<StaticLookup>();
            ArrayList arylstPOS = new ArrayList();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (Field_Name != "")
                {
                    string[] strFieldName = Field_Name.Split('|');

                    if (strFieldName != null)
                    {
                        if (strFieldName[0].ToString().Trim() != "")
                        {
                            ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", strFieldName[0].ToString().ToUpper())).AddOrder(Order.Asc(sOrder));
                            LookupList = criteria.List<StaticLookup>();
                        }
                        if (strFieldName[1].ToString().Trim() != "" && LookupList.Count > 0)
                        {
                            IList<StaticLookup> LookupListTemp = new List<StaticLookup>();
                            ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", strFieldName[1].ToString().ToUpper())).AddOrder(Order.Asc(sOrder));
                            LookupListTemp = criteria.List<StaticLookup>();

                            for (int i = 0; i < LookupList.Count; i++)
                            {
                                arylstPOS.Add(LookupList[i].Value + "-" + LookupListTemp[i].Value);
                            }
                        }
                    }
                }
                iMySession.Close();
            }

            return arylstPOS;
        }

        public IList<StaticLookup> getStaticLookupByFieldNameSortOrder(string sFieldName, string sPOSNo)
        {
           // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", sFieldName.ToUpper())).Add(Expression.Eq("Sort_Order", Convert.ToInt32(sPOSNo))).AddOrder(Order.Asc("Sort_Order"));
                staticList = criteria.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }

        //Added for MUEHR
        public IList<StaticLookup> getStaticLookupByFieldNameAndValue(string sFieldName, string sValue)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", sFieldName.ToUpper())).Add(Expression.Eq("Value", sValue));
                staticList = criteria.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }

        public IList<StaticLookup> GetStaticLookupByFieldNameAndPartialValue(string sFieldName, string sValue)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", sFieldName.ToUpper())).Add(Expression.Like("Value", "%" + sValue + "%")).AddOrder(Order.Asc("Sort_Order"));
                staticList = criteria.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }
        public IList<StaticLookup> GetLookupValuesbyDocTypeAndDocSubType(string Field_Name, string DocType, string SubDocType)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria1 = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", DocType.ToUpper())).Add(Expression.Eq("Doc_Sub_Type", "ALL"));
                staticList = criteria1.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;

        }
        public IList<StaticLookup> GetLookupValuesbyDocTypeSubTypeAndDefaultvalue(string Field_Name, string DocType, string SubDocType,string Value)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria1 = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", DocType.ToUpper())).Add(Expression.Eq("Doc_Sub_Type", "ALL")).Add(Expression.Eq("Default_Value", Value)).AddOrder(Order.Asc("Sort_Order"));
                staticList = criteria1.List<StaticLookup>();
                iMySession.Close();
            }
            return staticList;
        }
        public IList<ReasonCodeLookup> GetReasonCodeByDocTypeAndSubType(string FieldName,string Doc_Type,string Sub_Type) 
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<ReasonCodeLookup> ReasonList = new List<ReasonCodeLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria ReasonCriteria = iMySession.CreateCriteria(typeof(ReasonCodeLookup)).Add(Expression.Eq("Field_Name", FieldName.ToUpper())).Add(Expression.Eq("Doc_Type", Doc_Type)).Add(Restrictions.Or(Restrictions.Eq("Doc_Sub_Type", Sub_Type), Restrictions.Eq("Doc_Sub_Type", "ALL")));
                 ReasonList = ReasonCriteria.List<ReasonCodeLookup>();
                iMySession.Close();
            }
            return ReasonList;
        }
        public IList<ReasonCodeLookup> GetDescription(List<string> Codes, string Doc_Type, string Sub_Type)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
              IList<ReasonCodeLookup> ReasonList = new List<ReasonCodeLookup>();
              using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
              {
                  ICriteria ReasonCriteria = iMySession.CreateCriteria(typeof(ReasonCodeLookup)).Add(Expression.In("Reason_Code", Codes)).Add(Expression.Eq("Field_Name", "REASON CODE")).Add(Expression.Eq("Doc_Type", Doc_Type)).Add(Restrictions.Or(Restrictions.Eq("Doc_Sub_Type", Sub_Type), Restrictions.Eq("Doc_Sub_Type", "ALL")));
                  ReasonList = ReasonCriteria.List<ReasonCodeLookup>();
                  iMySession.Close();
              }
            return ReasonList;
        }
        public IList<ReasonCodeLookup> GetSubcodeFromReasonCode(string ReasonCode,string DocType, string DocSubType)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
              IList<ReasonCodeLookup> ReasonList = new List<ReasonCodeLookup>();
              using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
              {
                  ICriteria ReasonCriteria = iMySession.CreateCriteria(typeof(ReasonCodeLookup)).Add(Expression.Eq("Reason_Code", ReasonCode)).Add(Expression.Eq("Doc_Type", DocType)).Add(Restrictions.Or(Restrictions.Eq("Doc_Sub_Type", DocSubType), Restrictions.Eq("Doc_Sub_Type", "ALL"))).Add(Expression.Eq("Field_Name", "CODE_SUB_CODE_LOOKUP"));
                  ReasonList = ReasonCriteria.List<ReasonCodeLookup>();
                  iMySession.Close();
              }
            return ReasonList;
        }
        public IList<StaticLookup> GetLookupValuesbyDocTypeSubTypeAndvalue(string Field_Name, string DocType, string SubDocType, string Value)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
              IList<StaticLookup> staticList = new List<StaticLookup>();
              using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
              {
                  ICriteria criteria1 = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).Add(Expression.Eq("Doc_Type", DocType.ToUpper())).Add(Expression.Eq("Doc_Sub_Type", "ALL")).Add(Expression.Eq("Value", Value)).AddOrder(Order.Asc("Sort_Order"));
                  staticList = criteria1.List<StaticLookup>();
                  iMySession.Close();
              }
            return staticList;
        }
        public IList<StaticLookup> getStaticLookupByFieldNameAndSorder(string Field_Name, string sOrder)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
              IList<StaticLookup> staticList = new List<StaticLookup>();
              using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
              {
                  ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.Eq("Field_Name", Field_Name.ToUpper())).AddOrder(Order.Asc("Sort_Order"));
                  staticList = criteria.List<StaticLookup>();
                  iMySession.Close();
              }
            return staticList;
        }

        public IList<StaticLookup> getStaticLookupByFieldNamelikeSorder(string Field_Name)
        {
            IList<StaticLookup> ilstEncounter = new List<StaticLookup>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from static_lookup e where e.field_name like '%" + Field_Name + "%' order by e.sort_order").AddEntity("e", typeof(StaticLookup));
            ilstEncounter = sqlquery.List<StaticLookup>();
            return ilstEncounter;
        }

        public IList<StaticLookup> getStaticLookupByFieldNameEthinicity(string Field_Name)
        {
            IList<StaticLookup> ilstEncounter = new List<StaticLookup>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
           // ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from static_lookup e where e.field_name = '" + Field_Name + "' and value in ('Not Hispanic or Latino','Hispanic or Latino')").AddEntity("e", typeof(StaticLookup));
            ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from static_lookup e where e.field_name = '" + Field_Name + "'").AddEntity("e", typeof(StaticLookup));
            ilstEncounter = sqlquery.List<StaticLookup>();
            return ilstEncounter;
        }
        public IList<StaticLookup> getStaticLookupByFieldNameRace(string Field_Name)
        {
            IList<StaticLookup> ilstEncounter = new List<StaticLookup>();
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
         //   ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from static_lookup e where e.field_name = '" + Field_Name + "' and value in ('Black or African America','White','Asian')").AddEntity("e", typeof(StaticLookup));
            ISQLQuery sqlquery = iMySession.CreateSQLQuery("select e.* from static_lookup e where e.field_name = '" + Field_Name + "'").AddEntity("e", typeof(StaticLookup));
            ilstEncounter = sqlquery.List<StaticLookup>();
            return ilstEncounter;
        }
        
    }
}
