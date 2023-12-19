using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IProcedureCodeLibraryManager : IManagerBase<ProcedureCodeLibrary, ulong>
    {
        //FillProcedureCodeLibrary InsertProcedureCode(ProcedureCodeLibrary obj, int pgNumber, int MaxResult, string MACAddress);
        //FillProcedureCodeLibrary GetProcedureCodeList(int pgNumber, int MaxResult);
        //FillProcedureCodeLibrary UpdateProcedureCode(ProcedureCodeLibrary obj, int pgNumber, int MaxResult, string MACAddress);
        //FillProcedureCodeLibrary DeleteProcedureCode(ProcedureCodeLibrary obj, int pgNumber, int MaxResult, string MACAddress);
        IList<ProcedureCodeLibrary> GetProceduresBasedOnProcedureType(string ProcedureType);
        //IList<CodeLookupDTO> GetProcedureCodeAndDescriptionForCodeLookup(string code, string desc);
        IList<ProcedureCodeLibrary> SearchProcedureCodeBasedOnCPTAndDesc(string CPT, string Description, string ProcedureType,string sDate);
        IList<ProcedureCodeLibrary> SearchProcedureCodeBasedOnCPT(string CPT);
        bool VerifyEMCoding(string ProcedureCode);
        //IList<NDCCode> GetDescriptionUsingProcedureCode(string Code);
        double GetChargeforProcedure(string CPT);
        //GeneratePatientListsDTO GetGeneratePatientListLookup(); //prabu commented 16-Mar-2012
        Stream GetGeneratePatientListLookup();
        ProcedureCodeLibrary GetProcedureCodeDetails(string CPT);
        // Manimozhi - Feb 3rd 2012
        IList<ProcedureCodeLibrary> GetCPTAndDescriptionBasedOnProcedureType(string sProcedureCode, string sProcedureDescription, string sDate);
        //added by Srividhya on 10-Apr-2013
        //IList<NDCCode> GetNDCUnitbyNDCCodeAndCPT(string sCPT, string sNDCCode);
        IList<string> GetCPTDescriptionList(string sDescp, string type, string sTodate);
        IList<string> GetCPTAndDescription(string sSearchText);
    }
    public partial class ProcedureCodeLibraryManager : ManagerBase<ProcedureCodeLibrary, ulong>, IProcedureCodeLibraryManager
    {
        #region Constructors


        public ProcedureCodeLibraryManager()
            : base()
        {

        }
        public ProcedureCodeLibraryManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region CRUD

        //public FillProcedureCodeLibrary InsertProcedureCode(ProcedureCodeLibrary obj, int pgNumber, int MaxResult, string MACAddress)
        //{
        //    IList<ProcedureCodeLibrary> saveList = new List<ProcedureCodeLibrary>();
        //    saveList.Add(obj);
        //   // IList<ProcedureCodeLibrary> updatelist = null;
        //    //IList<ProcedureCodeLibrary> DeleteList = null;
        //    //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
        //    //SaveUpdateDeleteWithTransaction(ref saveList, null, null, MACAddress);
        //    return GetProcedureCodeList(pgNumber, MaxResult);
        //}

        //public FillProcedureCodeLibrary UpdateProcedureCode(ProcedureCodeLibrary obj, int pgNumber, int MaxResult, string MACAddress)
        //{
        //    //IList<ProcedureCodeLibrary> saveList = null;
        //    IList<ProcedureCodeLibrary> updtList = new List<ProcedureCodeLibrary>();
        //    //IList<ProcedureCodeLibrary> DeleteList = null;
        //    updtList.Add(obj);
        //    //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
        //    //SaveUpdateDeleteWithTransaction(ref saveList, updtList, null, MACAddress);
        //    return GetProcedureCodeList(pgNumber, MaxResult);
        //}

        //public FillProcedureCodeLibrary DeleteProcedureCode(ProcedureCodeLibrary obj, int pgNumber, int MaxResult, string MACAddress)
        //{
        //    //IList<ProcedureCodeLibrary> saveList = null;
        //    IList<ProcedureCodeLibrary> delList = new List<ProcedureCodeLibrary>();
        //    //IList<ProcedureCodeLibrary> updatelist = null;
        //    delList.Add(obj);
        //    //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
        //    //SaveUpdateDeleteWithTransaction(ref saveList, null, delList, MACAddress);
        //    return GetProcedureCodeList(pgNumber, MaxResult);
        //}

        #endregion

        #region GetMethods

        //public FillProcedureCodeLibrary GetProcedureCodeList(int pgNumber, int MaxResult)
        //{
        //    FillProcedureCodeLibrary objFillProCode = new FillProcedureCodeLibrary();
        //    ArrayList arrList = new ArrayList();
        //    ProcedureCodeLibrary objProCode = new ProcedureCodeLibrary();
        //    IList<ProcedureCodeLibrary> procedureList = new List<ProcedureCodeLibrary>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query = iMySession.GetNamedQuery("Fill.ProcedureCode.GetProcedureCodeCount");
        //        arrList = new ArrayList(query.List());

        //        if (arrList.Count > 0)
        //        {
        //            objFillProCode.ProcedureCount = Convert.ToInt16(arrList[0]);
        //        }
        //        pgNumber = pgNumber - 1;
        //        ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).AddOrder(Order.Desc("Modified_Date_And_Time"));
        //        procedureList = GetByCriteria(MaxResult, pgNumber, crit);
        //        //IQuery query1 = session.GetISession().GetNamedQuery("Fill.ProcedureCode.GetProcedureCodeList")
        //        //                    .SetInt64(0, pgNumber * MaxResult)
        //        //                    .SetInt64(1, MaxResult);
        //        //arrList = new ArrayList(query1.List());
        //        //if (arrList.Count > 0)
        //        //{
        //        //    foreach (Object[] obj in arrList)
        //        //    {

        //        //        objProCode = new ProcedureCodeLibrary();
        //        //        objProCode.Procedure_Code = obj[0].ToString();
        //        //        objProCode.Procedure_Product_Code = obj[1].ToString();
        //        //        objProCode.Procedure_Description = obj[2].ToString();
        //        //        objProCode.Procedure_Contract_ID = obj[3].ToString();
        //        //        objProCode.Procedure_CMN_Req = obj[5].ToString();
        //        //        objProCode.Procedure_Type = obj[5].ToString();
        //        //        objProCode.Created_By = obj[6].ToString();
        //        //        objProCode.Created_Date_And_Time = Convert.ToDateTime(obj[7]);
        //        //        objProCode.Modified_By = obj[8].ToString();
        //        //        objProCode.Modified_Date_And_Time = Convert.ToDateTime(obj[9]);
        //        //        objProCode.Version = Convert.ToInt16(obj[10]);
        //        //        objProCode.CVX_Code = obj[11].ToString();
        //        //        procedureList.Add(objProCode);

        //        //    }
        //        //}
        //        objFillProCode.ProcedureCodeList = procedureList;
        //        IList crit1 = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).SetProjection(Projections.ProjectionList().Add(Projections.Property("Procedure_Code"), "Procedure_Code")).List();
        //        objFillProCode.AllProcedureCodes = crit1;
        //        iMySession.Close();
        //    }
        //    return objFillProCode;
        //}

        public IList<ProcedureCodeLibrary> GetProceduresBasedOnProcedureType(string ProcedureType)
        {
            IList<ProcedureCodeLibrary> listPrb = new List<ProcedureCodeLibrary>();
            //.Add(Expression.Eq("Procedure_Type", ProcedureType));
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Eq("Procedure_Type", ProcedureType));
                listPrb = crit.List<ProcedureCodeLibrary>();
                iMySession.Close();
            }
            return listPrb;

        }

        public IList<ProcedureCodeLibrary> GetProcedureList(List<string> CPT)
        {
            IList<ProcedureCodeLibrary> listPrb = new List<ProcedureCodeLibrary>();
            //.Add(Expression.Eq("Procedure_Type", ProcedureType));
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.In("Procedure_Code", CPT)).AddOrder(Order.Desc("Procedure_Charge"));
                ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.In("Procedure_Code", CPT)).AddOrder(Order.Asc("Sort_Order"));
                listPrb = crit.List<ProcedureCodeLibrary>();
                iMySession.Close();
            }
            return listPrb;

        }
        //public IList<CodeLookupDTO> GetProcedureCodeAndDescriptionForCodeLookup(string code, string desc)
        //{
        //    IList<CodeLookupDTO> CodeLookUpList = new List<CodeLookupDTO>();
        //    CodeLookupDTO objCodeLookUp;
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IList<ProcedureCodeLibrary> ProcList = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Like("Procedure_Code", code))
        //            .Add(Expression.Like("Procedure_Description", desc)).List<ProcedureCodeLibrary>();

        //        for (int i = 0; i < ProcList.Count; i++)
        //        {
        //            objCodeLookUp = new CodeLookupDTO();
        //            objCodeLookUp.Code = ProcList[i].Procedure_Code;
        //            objCodeLookUp.Description = ProcList[i].Procedure_Description;
        //            CodeLookUpList.Add(objCodeLookUp);
        //        }
        //        iMySession.Close();
        //    }
        //    return CodeLookUpList;
        //}

        public IList<ProcedureCodeLibrary> SearchProcedureCodeBasedOnCPTAndDesc(string CPT, string Description, string ProcedureType,string sDate)
        {
            IList<ProcedureCodeLibrary> listPrb = new List<ProcedureCodeLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit;
                if (ProcedureType != string.Empty)
                {
                    crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Eq("Procedure_Type", ProcedureType))
                                    .Add(Expression.Like("Procedure_Code", CPT + "%")).Add(Expression.Like("Procedure_Description", Description + "%")).Add(Expression.Le("From_Date", sDate)).Add(Expression.Ge("To_Date", sDate));
                }
                else
                {
                    crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary))
                                    .Add(Expression.Like("Procedure_Code", CPT + "%")).Add(Expression.Like("Procedure_Description", Description + "%")).Add(Expression.Le("From_Date", sDate)).Add(Expression.Ge("To_Date", sDate));

                }
                listPrb = crit.List<ProcedureCodeLibrary>();
                iMySession.Close();
            }
            return listPrb;
        }

        public IList<ProcedureCodeLibrary> SearchProcedureCodeBasedOnCPT(string CPT)
        {
            IList<ProcedureCodeLibrary> listPrb = new List<ProcedureCodeLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary))
                                .Add(Expression.Eq("Procedure_Code", CPT));
                listPrb = crit.List<ProcedureCodeLibrary>();
                iMySession.Close();
            }
            return listPrb;
        }

        public bool VerifyEMCoding(string ProcedureCode)
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Eq("Procedure_Code", ProcedureCode));
                IList<ProcedureCodeLibrary> listPrb = crit.List<ProcedureCodeLibrary>();
                if (listPrb.Count != 0)
                {
                    iMySession.Close();
                    return true;
                }
                else
                {
                    iMySession.Close();
                    return false;
                }

            }
        }

        //public IList<NDCCode> GetDescriptionUsingProcedureCode(string Code)
        //{
        //    IList<NDCCode> obj = null;
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Eq("Procedure_Code", Code));
        //        if (crit.List<ProcedureCodeLibrary>().Count != 0)
        //        {
        //            obj = new List<NDCCode>();
        //            ProcedureCodeLibrary objProcedure = crit.List<ProcedureCodeLibrary>()[0];
        //            crit = iMySession.CreateCriteria(typeof(NDCCode)).Add(Expression.Eq("Procedure_Code", objProcedure.Procedure_Code));
        //            if (crit.List<NDCCode>().Count != 0)
        //                obj = crit.List<NDCCode>();
        //        }
        //        iMySession.Close();
        //    }
        //    return obj;
        //}

        //=======================commented by srividhya=======================
        //public double GetChargeforProcedure(string CPT)
        //{
        //    double Amount = 0;
        //    ICriteria crit = session.GetISession().CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Eq("Procedure_Code", CPT));
        //    if (crit.List<ProcedureCodeLibrary>().Count != 0)
        //        return Amount;
        //    //return crit.List<ProcedureCodeLibrary>()[0].Procedure_Charge;
        //    else
        //        return Amount;
        //}
        //=======================commented by srividhya=======================

        //=======================added by srividhya=======================
        public double GetChargeforProcedure(string CPT)
        {
            double Amount = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Eq("Procedure_Code", CPT));
                Amount = crit.List<ProcedureCodeLibrary>()[0].Procedure_Charge;
                iMySession.Close();
            }
            return Amount;
        }
        //=======================added by srividhya=======================

        //Query changed on 18.05.11 by velmurugan
        //public GeneratePatientListsDTO GetGeneratePatientListLookup() //prabu commented 16-Mar-2012
        public Stream GetGeneratePatientListLookup()
        {


            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();

            GeneratePatientListsDTO GenerateList = new GeneratePatientListsDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //GenerateList.ProcedureCodeList = session.GetISession().CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Eq("Procedure_Type", "IMMUNIZATION PROCEDURE")).List<ProcedureCodeLibrary>();
                ISQLQuery sql = iMySession.CreateSQLQuery("SELECT *  FROM loinc l   join  procedure_loinc_map p on(l.loinc_num=p.loinc_num)").AddEntity(typeof(Loinc));

                GenerateList.LoincCodeList = sql.List<Loinc>();

                serializer.WriteObject(stream, GenerateList);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;
        }
        public ProcedureCodeLibrary GetProcedureCodeDetails(string CPT)
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Eq("Procedure_Code", CPT));
                IList<ProcedureCodeLibrary> list = crit.List<ProcedureCodeLibrary>();
                if (list.Count > 0)
                {
                    iMySession.Close();
                    return list[0];
                }
                else
                {
                    iMySession.Close();
                    return new ProcedureCodeLibrary();
                }
            }
        }
        // Manimozhi - Feb 3rd 2012

        public IList<ProcedureCodeLibrary> GetCPTAndDescriptionBasedOnProcedureType(string sProcedureCode, string sProcedureDescription,string sDate)
        {
            ArrayList procedurecodeList = null;
            IList<ProcedureCodeLibrary> searchResultList = new List<ProcedureCodeLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query = iMySession.GetNamedQuery("Fill.ProcedureCode.GetProcedureCodeListBasedOnCPTAndDesc");
                //query.SetString(0, sProcedureType);
                query.SetString(0, sProcedureCode);
                query.SetString(1, sProcedureDescription);
                query.SetString(2, sProcedureCode);
                query.SetString(3, sProcedureDescription);
                query.SetString(4, sDate);
                query.SetString(5, sDate);
                procedurecodeList = new ArrayList(query.List());
                if (procedurecodeList != null)
                {
                    //                SELECT Procedure_Code,modifier,Procedure_Charge,Procedure_Allowed,Procedure_Adjust,
                    //Procedure_Code_Description, Procedure_Contract_ID,Procedure_CMN_Req,Procedure_Type,Procedure_Product_Code,CVX_Code,
                    //CVX_Code_Description, Created_By,CAST(Created_Date_And_Time AS CHAR(100)), Modified_By,
                    //CAST(Modified_Date_And_Time AS CHAR(100)),Version FROM procedure_code_library where Procedure_Type=?
                    //and ((Procedure_Code like ? and Procedure_Code_Description like ?) and (Procedure_Code like ? )and
                    //(Procedure_Code_Description like ?))
                    foreach (object[] obj in procedurecodeList)
                    {
                        ProcedureCodeLibrary procedurecode = new ProcedureCodeLibrary();
                        procedurecode.Procedure_Code = obj[0].ToString();
                        procedurecode.Modifier = obj[1].ToString();
                        procedurecode.Procedure_Charge = Convert.ToDouble(obj[2]);
                        procedurecode.Procedure_Allowed = Convert.ToDouble(obj[3]);
                        procedurecode.Procedure_Adjust = Convert.ToDouble(obj[4]);
                        procedurecode.Procedure_Description = obj[5].ToString();
                        procedurecode.Procedure_Contract_ID = obj[6].ToString();
                        procedurecode.Procedure_CMN_Req = obj[7].ToString();
                        procedurecode.Procedure_Type = obj[8].ToString();
                        procedurecode.Procedure_Product_Code = obj[9].ToString();
                        procedurecode.CVX_Code = obj[10].ToString();
                        procedurecode.CVX_Code_Description = obj[11].ToString();
                        procedurecode.Created_By = obj[12].ToString();
                        procedurecode.Created_Date_And_Time = Convert.ToDateTime(obj[13]);
                        procedurecode.Modified_By = obj[14].ToString();
                        procedurecode.Modified_Date_And_Time = Convert.ToDateTime(obj[15]);
                        procedurecode.Version = Convert.ToInt16(obj[16]);
                        searchResultList.Add(procedurecode);
                    }

                }
                iMySession.Close();
            }
            return searchResultList;

        }


        public IList<string> GetCPTAndDescription(string sSearchText)
        {
            IList<string> allCPTs = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query = iMySession.GetNamedQuery("GetProcedureCodeListBasedOnCPTAndDesc");
                query.SetString(0, sSearchText);
                query.SetString(1, sSearchText);
                allCPTs = query.List<string>();
                iMySession.Close();


            }
            return allCPTs;

        }

        

        public IList<ProcedureCodeLibrary> GetProcedureCodeDescription()
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<ProcedureCodeLibrary> listProc = new List<ProcedureCodeLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                listProc = iMySession.CreateCriteria(typeof(ProcedureCodeLibrary)).Add(Expression.Eq("Procedure_Type", "IMMUNIZATION PROCEDURE")).List<ProcedureCodeLibrary>();
                iMySession.Close();
            }
            return listProc;

        }

        //public IList<NDCCode> GetNDCUnitbyNDCCodeAndCPT(string sCPT, string sNDCCode)
        //{
        //    IList<NDCCode> NDCCodeList = new List<NDCCode>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {

        //        ICriteria crit = iMySession.CreateCriteria(typeof(NDCCode)).Add(Expression.Eq("Procedure_Code", sCPT)).Add(Expression.Eq("NDC_Code", sNDCCode));
        //        IList<NDCCode> list = crit.List<NDCCode>();
        //        NDCCodeList = crit.List<NDCCode>();
        //        iMySession.Close();
        //    }
        //    return NDCCodeList;
        //}

        public IList<string> GetCPTDescriptionList(string sDescp, string type, string sTodate)
        {
            ISession iMySessiondesc = NHibernateSessionManager.Instance.CreateISession();
            ArrayList arrList = new ArrayList();
            switch (type)
            {
                case "txtCPT":
                    IQuery query = iMySessiondesc.GetNamedQuery("GetAllCPTCode");
                    query.SetString(0, sDescp + "%");
                    query.SetString(1,  sTodate );
                    query.SetString(2,  sTodate );
                    arrList = new ArrayList(query.List());
                    break;
                case "txtCPTDescription":
                    string sQuery = string.Empty;
                    string[] sDescription = sDescp.Split(' ');
                    //Cap - 1301    
                    //sQuery = "SELECT concat(Procedure_Code,'~',Procedure_Code_Description,'~',Procedure_Charge,'~',sort_order) FROM `procedure_code_library` where ";
                    sQuery = "SELECT concat(a.Procedure_Code,'~',a.Procedure_Code_Description,'~',a.Procedure_charge ,'~',ifnull(b.sort_order,9),'~',ifnull(b.RVU,0)) FROM `procedure_code_library` as a left join  (select * from Procedure_Modifier_Lookup where modifier='')as b on a.procedure_code = b.procedure_code where ";
                    for (int i = 0; i < sDescription.Length; i++)
                    {
                        if(sDescription[i].Contains("'"))
                        {
                            sDescription[i] = sDescription[i].Replace("'","''");
                        }
                        if (i == 0)
                        {
                            sQuery += "Procedure_Code_Description like '%" + sDescription[i] + "%' and Is_Active='Y' and date_format(from_date,'%Y-%m-%d')<= '" + sTodate + "' and date_format(to_date,'%Y-%m-%d')>='"+sTodate +"'";
                        }
                        else
                        {
                            sQuery += "and Procedure_Code_Description like '%" + sDescription[i] + "%' and Is_Active='Y' and date_format(from_date,'%Y-%m-%d')<= '" + sTodate + "' and date_format(to_date,'%Y-%m-%d')>='" + sTodate + "'"; 
                        }
                    }

                    ISQLQuery SQLquery = iMySessiondesc.CreateSQLQuery(sQuery);
                    arrList = new ArrayList(SQLquery.List());
                    break;
            }

            IList<string> sDescList = new List<string>();
            for (int i = 0; i < arrList.Count; i++)
                sDescList.Add(arrList[i].ToString());

            iMySessiondesc.Close();
            return sDescList;
        }

        public IList<string> GetAllProcedureCodes()
        {
            IList<string> allCPTs;
            ISession iMySessionAllCPT = NHibernateSessionManager.Instance.CreateISession();
            IQuery CPTQuery = iMySessionAllCPT.GetNamedQuery("GetCPTWithDescription");
            allCPTs = CPTQuery.List<string>();
            iMySessionAllCPT.Close();
            return allCPTs;
        }
        #endregion


    }
}
