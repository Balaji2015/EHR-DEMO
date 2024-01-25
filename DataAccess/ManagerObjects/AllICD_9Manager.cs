using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using MySql.Data.MySqlClient;
using System.Data;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IAllICD_9Manager : IManagerBase<AllICD_9, string>
    {
        IList<AllICD_9> GetAllSearchByCODEAndSearchICD_9ForChild(string code, string desc, string ICDVersion);
        IList<string> TakingTheParentForProblemListCode(IList<string> code);
        IList<QuestionsDTO> GetQuestionMasterRecord(string parentID, ref string mutuallyExclusive, ref string sourceTable);
        IList<AllICD_9> GetProblemListCodeUsingCode(string[] probCode);
        IList<string> GetDescriptionUsingICDCode(IList<string> icd);
        //IList<CodeLookupDTO> GetAllIcdAndDescriptionForCodeLookup(string code, string desc);
        string LoadDescription(string IcdCode);
        //void ImportCDCOrderCodeLibrary(IList<OrderCodeLibrary> ilstOrderCodeLibrary, string MACAddress);
        //IList<AllICD_9> GetAllSearchByCODEAndSearchICD_9(string code, string desc);
        //IList<AllICD_9> GetMutuallyExclusiveICDs();
        IList<AllICD_9> GetMutuallyExclusiveICDs(IList<string> inputList);
        //srividhya added on 17-Mar-2014
        IList<AllICD_9> GetICDbyICDType(string sICDType, string sICD);

        IList<ICD9ICD10Mapping> GetICD10CodeDesc(string[] ICDDesc);

        IList<ICD9ICD10Mapping> GetCodesDesc(string ICDDesc);

        IList<string> TakeParentICDLeafandMutuallyExclusive(string code);
        string GetBySnomed(string snomedcode);
        IList<string> GetDescriptionListPotentialDiagnosis(string sDescp, string type, string ICDType);
        IList<AllICD_9> GetAllICDCodes(string[] ICD);
        IList<string> GetICDAndDescription(string sSearchText);
        IList<string> CheckTruncatedICD10Codes(IList<string> lstICD10Codes, string sToDate);
    }

    public partial class AllICD_9Manager : ManagerBase<AllICD_9, string>, IAllICD_9Manager
    {

        #region Constructors

        public AllICD_9Manager()
            : base()
        {

        }
        public AllICD_9Manager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        //Latha - Branch_3_0_52_production - Start
        //If in search result we receive the parent alone then all the child for that parent needs to be fetched.
        public IList<AllICD_9> RecursiveSearch(IList<AllICD_9> allIcd, string code, string desc)
        {
            IList<AllICD_9> All_Icd_List = new List<AllICD_9>();
            using (ISession iMySessionRecursive = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySessionRecursive = NHibernateSessionManager.Instance.CreateISession();
                
                IQuery query = iMySessionRecursive.CreateSQLQuery("select a.all_icd,a.parent_icd,a.icd_description,a.icd_synonyms,a.leaf_node,a.mutually_exclusive,a.hcc_category,a.manageable,a.version_year from all_icd a where a.all_icd like '" + code + "' and (a.icd_description || '|' || a.icd_synonyms) like '" + desc + "'");
                ArrayList arrList = new ArrayList(query.List());
                if (arrList != null && arrList.Count != 0)
                {
                    foreach (object[] obj in arrList)
                    {
                        AllICD_9 all = new AllICD_9();
                        if (obj.Count() > 0)
                        {
                            all.ICD_9 = obj[0].ToString();
                            all.Parent_ICD_9 = obj[1].ToString();
                            all.ICD_9_Description = obj[2].ToString();
                            all.ICD_9_Description_Synonyms = obj[3].ToString();
                            all.Leaf_Node = obj[4].ToString();
                            all.Mutually_Exclusive = obj[5].ToString();
                            all.HCC_Category = obj[6].ToString();
                            all.Manageable = obj[7].ToString();
                            all.Version_Year = obj[8].ToString();
                            if (allIcd.Contains(all) == false)
                            {
                                allIcd.Add(all);
                            }
                            if (obj[4].ToString().ToUpper() != "Y")
                            {
                                GetInternalChildWithParent(allIcd, obj[0].ToString());
                            }
                        }
                    }
                }
                iMySessionRecursive.Close();
            }
            return All_Icd_List;
        }

        public void GetInternalChildWithParent(IList<AllICD_9> all_Icd, string whereCond)
        {
            using (ISession iMySessionInternalClid = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySessionInternalClid = NHibernateSessionManager.Instance.CreateISession();
                IQuery query1 = iMySessionInternalClid.CreateSQLQuery("select a.all_icd,a.parent_icd,a.icd_description,a.icd_synonyms,a.leaf_node,a.mutually_exclusive,a.hcc_category,a.manageable,a.version_year from all_icd a where a.parent_icd like '" + whereCond + "'");
                ArrayList arrayList = new ArrayList(query1.List());
                if (arrayList != null && arrayList.Count != 0)
                {
                    foreach (object[] obj1 in arrayList)
                    {
                        AllICD_9 a = new AllICD_9();
                        if (obj1.Count() > 0)
                        {
                            a.ICD_9 = obj1[0].ToString();
                            a.Parent_ICD_9 = obj1[1].ToString();
                            a.ICD_9_Description = obj1[2].ToString();
                            a.ICD_9_Description_Synonyms = obj1[3].ToString();
                            a.Leaf_Node = obj1[4].ToString();
                            a.Mutually_Exclusive = obj1[5].ToString();
                            a.HCC_Category = obj1[6].ToString();
                            a.Manageable = obj1[7].ToString();
                            a.Version_Year = obj1[8].ToString();
                            if (all_Icd.Contains(a) == false)
                            {
                                all_Icd.Add(a);
                            }
                            if (obj1[3].ToString().ToUpper() != "Y")
                            {
                                GetInternalChildWithParent(all_Icd, obj1[0].ToString());
                            }
                        }
                    }
                }
                iMySessionInternalClid.Close();
            }
        }


        #region Methods
        public IList<AllICD_9> GetAllSearchByCODEAndSearchICD_9ForChild(string code, string desc, string ICDVersion)
        {
            IList<AllICD_9> il = new List<AllICD_9>();
            using (ISession iMySessionSearchByCODE = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySessionSearchByCODE = NHibernateSessionManager.Instance.CreateISession();
                ArrayList parentList = new ArrayList();

                ArrayList FavoriteList = new ArrayList();
                IQuery query1 = iMySessionSearchByCODE.CreateSQLQuery("select a.* FROM all_icd a where (a.Leaf_Node= 'Y' and a.Is_Active='Y' and a.all_icd like '" + code + "' and (a.icd_description like '" + desc + "'or a.icd_synonyms like '" + desc + "') and Version_Year = '" + ICDVersion + "')").AddEntity("a", typeof(AllICD_9));
                il = query1.List<AllICD_9>();
                iMySessionSearchByCODE.Close();
            }

            //IQuery query1 = session.GetISession().GetNamedQuery("Get.all.searchBycodeAndDescICD_9");
            //query1.SetString(0, code);
            //query1.SetString(1, desc);

            //IQuery query1 = session.GetISession().GetNamedQuery("Get.all.searchBycodeAndDescICD_9");

            //query1.SetString(0, code);

            //query1.SetString(1, desc);


            //query1.SetString(2, code);

            //query1.SetString(3, desc);

            //query1.SetString(4, code);

            //query1.SetString(5, desc);

            //query1.SetString(6, code);

            //query1.SetString(7, desc);

            //query1.SetString(8, code);

            //query1.SetString(9, desc);

            //query1.SetString(10, code);

            //query1.SetString(11, desc);

            //query1.SetString(12, code);

            //query1.SetString(13, desc);

            //query1.SetString(14, code);

            //query1.SetString(15, desc);

            //FavoriteList = (ArrayList)query1.List();

            //FavoriteList = (ArrayList)query1.List();


            //Added Latha - 2/8/10 - Checking for null value.

            //if (FavoriteList != null)
            //{
            //    if (FavoriteList.Count > 0)
            //    {
            //        foreach (object[] obj in FavoriteList)
            //        {
            //            AllICD_9 a = new AllICD_9();
            //            if (obj.Count() > 0)
            //            {
            //                a.ICD_9 = obj[0].ToString();
            //                a.Parent_ICD_9 = obj[1].ToString();
            //                a.ICD_9_Description = obj[2].ToString();
            //                a.ICD_9_Description_Synonyms = obj[3].ToString(); 
            //                a.Leaf_Node = obj[4].ToString();
            //                a.Mutually_Exclusive = obj[5].ToString();
            //                a.HCC_Category = obj[6].ToString();
            //                a.Manageable = obj[7].ToString();
            //                a.Version_Year = obj[8].ToString();
            //                if (il.Contains(a) == false)
            //                {
            //                    il.Add(a);
            //                }
            //            }
            //        }
            //    }
            //}


            //if (FavoriteList != null)

            //{

            //    if (FavoriteList.Count > 0)

            //    {

            //        foreach (object[] obj in FavoriteList)

            //        {

            //            AllICD_9 a = new AllICD_9();

            //            if (obj.Count() > 0)

            //            {

            //                a.ICD_9 = obj[0].ToString();

            //                a.Parent_ICD_9 = obj[1].ToString();

            //                a.ICD_9_Description = obj[2].ToString();

            //                a.ICD_9_Description_Synonyms = obj[3].ToString();

            //                a.Leaf_Node = obj[4].ToString();

            //                a.Mutually_Exclusive = obj[5].ToString();

            //                a.HCC_Category = obj[6].ToString();

            //                a.Manageable = obj[7].ToString();

            //                a.Version_Year = obj[8].ToString();

            //                if (il.Contains(a) == false)

            //                {

            //                    il.Add(a);

            //                }

            //            }

            //        }

            //    }

            //}



            //Comment by Bala on Duplicates generated in All diagnosis and Manage frequently used diagnosis

            //RecursiveSearch(il, code, desc);//













            //IQuery query1 = session.GetISession().GetNamedQuery("Get.all.searchBycodeAndDescICD_9_WithParentFetchChild");

            //query1.SetString(0, code);

            //query1.SetString(1, desc);

            //query1.SetString(2, code);

            //query1.SetString(3, desc);

            //query1.SetString(4, code);

            //query1.SetString(5, desc);

            //query1.SetString(6, code);

            //query1.SetString(7, desc);

            //query1.SetString(8, code);

            //query1.SetString(9, desc);

            //query1.SetString(10, code);

            //query1.SetString(11, desc);

            //query1.SetString(12, code);

            //query1.SetString(13, desc);

            //query1.SetString(14, code);

            //query1.SetString(15, desc);



            //parentList = (ArrayList)query1.List();



            ////Added Latha - 2/8/10 - Checking for null value.

            //if (parentList != null)

            //{

            //    if (parentList.Count > 0)

            //    {

            //        foreach (object[] obj in parentList)

            //        {

            //            AllICD_9 a = new AllICD_9();

            //            if (obj.Count() > 0)

            //            {

            //                a.ICD_9 = obj[0].ToString();

            //                a.Parent_ICD_9 = obj[1].ToString();

            //                a.ICD_9_Description = obj[2].ToString();

            //                a.Leaf_Node = obj[3].ToString();

            //                a.Mutually_Exclusive = obj[4].ToString();

            //                a.HCC_Category = obj[5].ToString();

            //                a.Manageable = obj[6].ToString();

            //                a.Version_Year = obj[7].ToString();

            //                if (il.Contains(a) == false)

            //                {

            //                    il.Add(a);

            //                }

            //            }

            //        }

            //    }

            //}

            return il;
        }
        //Latha - Branch_3_0_52_production - End

        //Added for stored procedure
        public IList<string> TakeParentICD(string code)
        {
            string[] sarr = new string[] { };
            //Added by vaishali for bug 33947
            string NewCode = "";
            if (code != string.Empty && code.Contains('|'))
            {
                string[] SplitCodes = code.Split('|');
                for (int iNumber = 0; iNumber < SplitCodes.Count(); iNumber++)
                {
                    if (SplitCodes[iNumber] != "")
                    {
                        if (NewCode == "")
                            NewCode = SplitCodes[iNumber];
                        else
                            NewCode = NewCode + "|" + SplitCodes[iNumber];
                    }
                }
            }
            else
                NewCode = code;
            if (NewCode != string.Empty)
            {
                using (ISession iMySessionTakeParent = NHibernateSessionManager.Instance.CreateISession())
                {
                    IQuery iQuery = iMySessionTakeParent.GetNamedQuery("SP.Assessment.GetParentICD");

                    iQuery.SetParameter("ICDS1", NewCode);

                    ArrayList arlstReturn = new ArrayList(iQuery.List());
                    sarr = arlstReturn[0].ToString().Split('$');
                }
            }
            return sarr.ToList<string>();
        }
        //End for stored procedure

        string strproblemListWithParent = null;

        public IList<string> TakingTheParentForProblemListCode(IList<string> code)
        {
            IList<string> parentList = new List<string>();

            //Added Latha - 2/8/10 - Checking for null value.
            if (code != null)
            {
                for (int i = 0; i < code.Count; i++)
                {
                    strproblemListWithParent = null;
                    parentList.Add(TakeParent(code[i]));
                }
            }
            return parentList;


        }

        public string LoadDescription(string IcdCode)
        {
            string Description = string.Empty;
            using (ISession iMySessionLoadDescription = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySessionLoadDescription = NHibernateSessionManager.Instance.CreateISession();
                ICriteria crit = iMySessionLoadDescription.CreateCriteria(typeof(AllICD_9)).Add(Expression.Eq("ICD_9", IcdCode));
                if (crit.List<AllICD_9>().Count != 0)
                    Description = crit.List<AllICD_9>()[0].ICD_9_Description;
                iMySessionLoadDescription.Close();
            }
            return Description;
        }

        public string TakeParent(string code)
        {
            using (ISession iMySessionTakeParent = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySessionTakeParent = NHibernateSessionManager.Instance.CreateISession();
                if (strproblemListWithParent == null)
                {
                    strproblemListWithParent = code;
                }
                ArrayList ResultList = null;
                IQuery query3 = iMySessionTakeParent.GetNamedQuery("Get.ProblemList.ParentICDCode");
                query3.SetString(0, code);
                ResultList = new ArrayList(query3.List());

                //Added Latha - 2/8/10 - Checking for null value.
                if (ResultList != null)
                {
                    if (ResultList.Count > 0)
                    {
                        if (ResultList[0].ToString() != string.Empty)
                        {
                            strproblemListWithParent += "!" + ResultList[0].ToString();
                            TakeParent(ResultList[0].ToString());
                        }
                    }
                }
                iMySessionTakeParent.Close();
            }
            return strproblemListWithParent;
        }

        public IList<QuestionsDTO> GetQuestionMasterRecord(string parentID, ref string mutuallyExclusive, ref string sourceTable)
        {
            IList<QuestionsDTO> QuestionsList = new List<QuestionsDTO>();
            using (ISession iMySessionQuestionMaster = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySessionQuestionMaster = NHibernateSessionManager.Instance.CreateISession();
                ArrayList ResultList = null;
                
                if (sourceTable == "ALLICD")
                {
                    IQuery query3 = iMySessionQuestionMaster.GetNamedQuery("Get.Questions.FromAiiICDTable.ByPassingParentID");
                    query3.SetString(0, parentID.ToString());
                    ResultList = new ArrayList(query3.List());

                    //Added Latha - 2/8/10 - Checking for null value.
                    if (ResultList != null)
                    {
                        for (int i = 0; i < ResultList.Count; i++)
                        {
                            object[] oj = (object[])ResultList[i];
                            QuestionsDTO q = new QuestionsDTO();
                            q.ICD_9 = oj[0].ToString();
                            q.Parent_ID = oj[1].ToString();
                            q.Diagnosis_Description = oj[2].ToString();
                            q.Leaf_Node = oj[3].ToString();
                            q.Mutually_Exclusive = oj[4].ToString();
                            q.HCC_Category = Convert.ToUInt64(oj[5].ToString());
                            QuestionsList.Add(q);
                            mutuallyExclusive = q.Mutually_Exclusive;
                        }
                    }
                }
                else if (sourceTable == "ASSOCIATEDPRIMARYICD")
                {
                    IQuery query3 = iMySessionQuestionMaster.GetNamedQuery("Get.Questions.FromAssociatedPrimaryICD.ByPassingParentID");
                    query3.SetString(0, parentID.ToString());
                    ResultList = new ArrayList(query3.List());

                    //Added Latha - 2/8/10 - Checking for null value.
                    if (ResultList != null)
                    {
                        for (int i = 0; i < ResultList.Count; i++)
                        {
                            object[] oj = (object[])ResultList[i];
                            QuestionsDTO q = new QuestionsDTO();
                            q.ICD_9 = oj[1].ToString();
                            q.Parent_ID = oj[0].ToString();
                            q.Diagnosis_Description = oj[2].ToString();
                            q.Mutually_Exclusive = oj[3].ToString();
                            q.Leaf_Node = oj[4].ToString();
                            q.Prefix = oj[5].ToString();
                            QuestionsList.Add(q);
                            mutuallyExclusive = q.Mutually_Exclusive;
                        }
                    }
                }
                iMySessionQuestionMaster.Close();
            }
            return QuestionsList;
        }

        public IList<AllICD_9> GetProblemListCodeUsingCode(string[] probCode)
        {
            IList<AllICD_9> IcdList = new List<AllICD_9>();
            using (ISession iMySessionProblemListCode = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySessionProblemListCode = NHibernateSessionManager.Instance.CreateISession();
                AllICD_9 a = null;
                ArrayList allicd = null;
                IQuery query = iMySessionProblemListCode.GetNamedQuery("Get.AllIcd.GetProblemListFromAllIcd");
                query.SetParameterList("IcdCode", probCode);

                allicd = new ArrayList(query.List());

                //Added Latha - 2/8/10 - Checking for null value.
                if (allicd != null)
                {
                    for (int i = 0; i < allicd.Count; i++)
                    {
                        object[] oj = (object[])allicd[i];
                        a = new AllICD_9();
                        a.ICD_9 = oj[0].ToString();
                        a.Parent_ICD_9 = oj[1].ToString();
                        a.ICD_9_Description = oj[2].ToString();
                        a.ICD_9_Description_Synonyms = oj[3].ToString();
                        a.Leaf_Node = oj[4].ToString();
                        a.Mutually_Exclusive = oj[5].ToString();
                        a.HCC_Category = oj[6].ToString();
                        a.Manageable = oj[7].ToString();
                        a.Version_Year = oj[8].ToString();
                        IcdList.Add(a);
                    }
                }

                iMySessionProblemListCode.Close();
            }
            return IcdList;
        }

        public IList<string> GetDescriptionUsingICDCode(IList<string> icd)
        {
            //ISession iMySessionDescriptionUsingICD = NHibernateSessionManager.Instance.CreateISession();
            IList<string> codeAndDesc = new List<string>();
            using (ISession iMySessionDescriptionUsingICD = NHibernateSessionManager.Instance.CreateISession())
            {
                ArrayList code = null;
                IQuery query = iMySessionDescriptionUsingICD.GetNamedQuery("Get.AllIcd.GetDescriptionUsingICDCode");
                query.SetParameterList("IcdCode", icd.ToArray<string>());
                code = new ArrayList(query.List());
                if (code != null)
                {
                    for (int i = 0; i < code.Count; i++)
                    {
                        codeAndDesc.Add(code[i].ToString());
                    }
                }
                iMySessionDescriptionUsingICD.Close();
            }
            return codeAndDesc;
        }

        //public IList<CodeLookupDTO> GetAllIcdAndDescriptionForCodeLookup(string code, string desc)
        //{
        //    //ISession iMySessionIcdAndDescriptionForCode = NHibernateSessionManager.Instance.CreateISession();
        //    IList<CodeLookupDTO> CodeLookUpList = new List<CodeLookupDTO>();
        //    using (ISession iMySessionIcdAndDescriptionForCode = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        CodeLookupDTO objCodeLookUp;
        //        IList<AllICD_9> AllICD_9List = iMySessionIcdAndDescriptionForCode.CreateCriteria(typeof(AllICD_9)).Add(Expression.Like("ICD_9", code))
        //            .Add(Expression.Like("ICD_9_Description", desc)).List<AllICD_9>();

        //        for (int i = 0; i < AllICD_9List.Count; i++)
        //        {
        //            objCodeLookUp = new CodeLookupDTO();
        //            objCodeLookUp.Code = AllICD_9List[i].ICD_9;
        //            objCodeLookUp.Description = AllICD_9List[i].ICD_9_Description;
        //            CodeLookUpList.Add(objCodeLookUp);
        //        }
        //        iMySessionIcdAndDescriptionForCode.Close();
        //    }
        //    return CodeLookUpList;
        //}

        //public IList<AllICD_9> GetAllSearchByCODEAndSearchICD_9(string code, string desc)
        //{
        //    ArrayList FavoriteList = null;
        //    IList<AllICD_9> il = new List<AllICD_9>();
        //    //string s = null;
        //    IQuery query = session.GetISession().GetNamedQuery("Get.all.searchBycodeAndDescICD_9");
        //    query.SetString(0, code);
        //    query.SetString(1, desc);

        //    FavoriteList = new ArrayList(query.List());
        //    IList<AllICD_9> il = new List<AllICD_9>();

        //    //Added Latha - 2/8/10 - Checking for null value.
        //    if (FavoriteList != null)
        //    {
        //        for (int i = 0; i < FavoriteList.Count; i++)
        //        {
        //            il.Add((AllICD_9)FavoriteList[i]);
        //        }
        //    }
        //    return il;
        //}

        //public IList<AllICD_9> GetMutuallyExclusiveICDs()
        //{
        //    return session.GetISession().CreateCriteria(typeof(AllICD_9)).List<AllICD_9>();
        //}

        public IList<AllICD_9> GetMutuallyExclusiveICDs(IList<string> inputList)
        {
            IList<AllICD_9> iListICd9 = new List<AllICD_9>();
            using (ISession iMySessionIcdAndDescriptionForCode = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySessionIcdAndDescriptionForCode = NHibernateSessionManager.Instance.CreateISession();
                iListICd9 = iMySessionIcdAndDescriptionForCode.CreateCriteria(typeof(AllICD_9)).Add(Expression.In("ICD_9", inputList.ToArray())).List<AllICD_9>();
                iMySessionIcdAndDescriptionForCode.Close();
            }
            return iListICd9;
        }

        //srividhya
        public IList<AllICD_9> GetICDbyICDType(string sICDType, string sICD)
        {
            //ISession iMySessionICDbyICDType = NHibernateSessionManager.Instance.CreateISession();
            IList<AllICD_9> ICDList = new List<AllICD_9>();
            using (ISession iMySessionICDbyICDType = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySessionICDbyICDType.CreateCriteria(typeof(AllICD_9)).Add(Expression.Eq("Version_Year", sICDType)).Add(Expression.Eq("ICD_9", sICD));
                ICDList = crit.List<AllICD_9>();
                iMySessionICDbyICDType.Close();
            }
            return ICDList;
        }



        public IList<string> GetDescriptionList(string sDescp, string type, string ICDType, string sTodate)
        {
            ISession iMySessiondesc = NHibernateSessionManager.Instance.CreateISession();
            ArrayList arrList = new ArrayList();
            switch (type)
            {
                case "txtICD10":
                    IQuery query = iMySessiondesc.GetNamedQuery("GetAllICDBy10Code");
                    query.SetString(0, sDescp.Trim() + "%");
                    query.SetString(1, ICDType);
                    query.SetString(2,  sTodate );
                    query.SetString(3,  sTodate );
                    arrList = new ArrayList(query.List());
                    break;
                case "txtDescription":
                    query = iMySessiondesc.GetNamedQuery("GetAllICDDescription");
                    query.SetString(0, "%" + sDescp.Trim() + "%");
                    query.SetString(1, "%" + sDescp.Trim() + "%");
                    query.SetString(2, ICDType);
                    query.SetString(3, sTodate);
                    query.SetString(4,  sTodate );
                    arrList = new ArrayList(query.List());
                    break;
            }

            IList<string> sDescList = new List<string>();
            for (int i = 0; i < arrList.Count; i++)
                sDescList.Add(arrList[i].ToString());

            iMySessiondesc.Close();
            return sDescList;
        }


        public IList<ICD9ICD10Mapping> GetICD10CodeDesc(string[] ICDDesc)
        {
            IList<ICD9ICD10Mapping> ICDList = new List<ICD9ICD10Mapping>();
            using (ISession iMySessionICDbyICDType = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySessionICDbyICDType.CreateCriteria(typeof(ICD9ICD10Mapping)).Add(Expression.In("ICD9", ICDDesc));
                ICDList = crit.List<ICD9ICD10Mapping>();
                iMySessionICDbyICDType.Close();
            }
            return ICDList;
        }


        public IList<ICD9ICD10Mapping> GetCodesDesc(string ICDDesc)
        {
            IList<ICD9ICD10Mapping> ICDList = new List<ICD9ICD10Mapping>();
            using (ISession iMySessionICDbyICDType = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySessionICDbyICDType.CreateCriteria(typeof(ICD9ICD10Mapping)).Add(Expression.Eq("ICD9", ICDDesc));
                ICDList = crit.List<ICD9ICD10Mapping>();
                iMySessionICDbyICDType.Close();
            }
            return ICDList;
        }

        public IList<AllICD_9> GetBysnomedcode(string snomedcode, string description)
        {
            //ISession iMySessionICDbyICDType = NHibernateSessionManager.Instance.CreateISession();
            IList<AllICD_9> ICDList = new List<AllICD_9>();
            using (ISession iMySessionICDbyICDType = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySessionICDbyICDType.CreateCriteria(typeof(AllICD_9)).Add(Expression.Eq("Version_Year", "ICD_10")).Add(Expression.Eq("SNOMED_Code", snomedcode)).Add(Expression.Eq("SNOMED_Code_Description", description));
                ICDList = crit.List<AllICD_9>();
                iMySessionICDbyICDType.Close();
            }
            return ICDList;
        }

        public string GetBySnomed(string snomedcode)
        {
            string icd = string.Empty;
            IList<AllICD_9> ICDList = new List<AllICD_9>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
               // ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT * FROM  all_icd where snomed_code=" + snomedcode + " and version_year='ICD_10';").AddEntity("e", typeof(AllICD_9));
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("SELECT * FROM  all_icd where snomed_code=" + snomedcode + ";").AddEntity("e", typeof(AllICD_9));
                ICDList = sqlquery.List<AllICD_9>();
                if (ICDList.Count > 0)
                { 
                    IList<AllICD_9> ICDListtemp = new List<AllICD_9>();

                    ICDListtemp = (from m in ICDList where m.Version_Year == "ICD_10" select m).ToList<AllICD_9>();
                    if(ICDListtemp.Count==0)
                        ICDListtemp = (from m in ICDList where m.Version_Year == "ICD_9" select m).ToList<AllICD_9>();
                    if (ICDListtemp.Count > 0)
                        icd = ICDListtemp[0].ICD_9;
                }
                iMySession.Close();
            }
           
            return icd;
        }

        public IList<string> TakeParentICDLeafandMutuallyExclusive(string code)
        {
            if (code != string.Empty)
                code = code.Replace("\t", "");//For bug Id 57688
            string[] sarr = new string[] { };
            //Added by vaishali for bug 33947
            string NewCode = "";
            if (code != string.Empty && code.Contains('|'))
            {
                string[] SplitCodes = code.Split('|');
                for (int iNumber = 0; iNumber < SplitCodes.Count(); iNumber++)
                {
                    if (SplitCodes[iNumber] != "")
                    {
                        if (NewCode == "")
                            NewCode = "'" + SplitCodes[iNumber] + "'";
                        else
                            NewCode = NewCode + "|" + "'" + SplitCodes[iNumber] + "'";
                    }
                }
            }
            else
                NewCode = "'" + code + "'";
            if (NewCode != string.Empty && NewCode != "''")
            {
                //using (ISession iMySessionTakeParent = NHibernateSessionManager.Instance.CreateISession())
                //{
                //    IQuery iQuery = iMySessionTakeParent.GetNamedQuery("SP.Assessment.geticd");

                //    iQuery.SetParameter("ICDS1", NewCode);
                //    ArrayList arlstReturn = new ArrayList(iQuery.List());
                //    sarr = arlstReturn[0].ToString().Split('$');
                //}
                using (ISession iMySessionTakeParent = NHibernateSessionManager.Instance.CreateISession())
                {
                    IQuery iQuery = iMySessionTakeParent.CreateSQLQuery("select group_concat(concat(all_icd,'!',Mutually_Exclusive_ICD) separator '$') from all_icd where all_icd in (" + NewCode.Replace("|", ",") + ")");
                    
                    ArrayList arlstReturn = new ArrayList(iQuery.List());
                    sarr = arlstReturn[0].ToString().Split('$');
                }
            }
            return sarr.ToList<string>();
        }

        public IList<string> GetAllICDS()
        {
            IList<string> allICDs;
            ISession iMySessionAllICD = NHibernateSessionManager.Instance.CreateISession();
            IQuery ICDQuery = iMySessionAllICD.GetNamedQuery("GetICDWithDescription");
            allICDs = ICDQuery.List<string>();
            iMySessionAllICD.Close();
            return allICDs;
        }
        public IList<AllICD_9> GetAllICDCodes(string[] ICD)
        {
            IList<AllICD_9> AssociatedList = new List<AllICD_9>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
                ICriteria criteria = iMySession.CreateCriteria(typeof(AllICD_9)).Add(Expression.In("ICD_9", ICD)).Add(Expression.Eq("Leaf_Node", "N"));
                AssociatedList = criteria.List<AllICD_9>();
                iMySession.Close();
            }
            return AssociatedList;
        }
        public IList<string> GetDescriptionListPotentialDiagnosis(string sDescp, string type, string ICDType)
        {
            ISession iMySessiondesc = NHibernateSessionManager.Instance.CreateISession();
            ArrayList arrList = new ArrayList();
            switch (type)
            {
                case "txtICD10":
                    IQuery query = iMySessiondesc.GetNamedQuery("GetAllICDBy10CodePotentialDiagnosis");
                    query.SetString(0, sDescp.Trim() + "%");
                    query.SetString(1, ICDType);
                    arrList = new ArrayList(query.List());
                    break;
                case "txtDescription":
                    query = iMySessiondesc.GetNamedQuery("GetAllICDDescriptionPotentialDiagnosis");
                    query.SetString(0, "%" + sDescp.Trim() + "%");
                    query.SetString(1, "%" + sDescp.Trim() + "%");
                    query.SetString(2, ICDType);
                    arrList = new ArrayList(query.List());
                    break;
            }

            IList<string> sDescList = new List<string>();
            for (int i = 0; i < arrList.Count; i++)
                sDescList.Add(arrList[i].ToString());

            iMySessiondesc.Close();
            return sDescList;
        }

        public IList<string> GetICDAndDescription(string sSearchText)
        {
            IList<string> allCPTs = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query = iMySession.GetNamedQuery("GetICDCodeListBasedOnICDAndDesc");
                query.SetString(0, sSearchText);
                query.SetString(1, sSearchText);
                query.SetString(2, sSearchText);
                allCPTs = query.List<string>();
                iMySession.Close();
            }
            return allCPTs;

        }

        public IList<string> CheckTruncatedICD10Codes(IList<string> lstICD10Codes,string sToDate)
        {
            IList<string> allTruncatedICDs = null;
            using (ISession iMySessionRecursive = NHibernateSessionManager.Instance.CreateISession())
            {
                string stype = lstICD10Codes.Aggregate((x, y) => x + "','" + y );
                IQuery query = iMySessionRecursive.CreateSQLQuery("select all_icd from all_icd  where all_icd in ('" + stype + "') and Leaf_Node= 'Y'  and Is_Active='Y' and Version_Year='ICD_10' and  date_format(from_date,'%Y-%m-%d')<='" + sToDate + "' and date_format(to_date,'%Y-%m-%d')>='" + sToDate + "' ;");
                ArrayList arrList = new ArrayList(query.List());
                if (arrList != null && arrList.Count != 0)
                {
                    IList<string> lstallICD = new List<string>();
                    foreach (string obj in arrList)
                    {
                       
                        if (obj.Count() > 0)
                        {
                            lstallICD.Add(obj.Trim());
                        }
                    }
                    allTruncatedICDs = new List<string>();
                    allTruncatedICDs = lstICD10Codes.Except(lstallICD).ToList<string>();
                }
                //Cap - 1461
                else
                {
                    allTruncatedICDs = new List<string>();
                    allTruncatedICDs = lstICD10Codes.ToList<string>();
                }

                iMySessionRecursive.Close();
            }
            return allTruncatedICDs;
        }
        #endregion
    }
}
