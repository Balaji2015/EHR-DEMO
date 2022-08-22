using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface ICQMSummaryManager : IManagerBase<CQMSummary, ulong>
    {
        IList<CQMSummary> AppendCQMSummary(IList<CQMSummary> CQMSummaryList, string MACAddress);
        IList<PQRI_Measure> GetCQMSummary(string sLegalOrg,string sMeaurementYear,ulong ulPhysicianId);
        void DeleteCQMData();
       
    }
    public partial class CQMSummaryManager : ManagerBase<CQMSummary, ulong>, ICQMSummaryManager
    {
        #region Constructors

        public CQMSummaryManager()
            : base()
        {

        }
        public CQMSummaryManager(INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods

        public IList<CQMSummary> AppendCQMSummary(IList<CQMSummary> CQMSummaryList, string MACAddress)
        {
            IList<CQMSummary> nullList = null;

            SaveUpdateDelete_DBAndXML_WithTransaction(ref CQMSummaryList, ref nullList, null, MACAddress, false, false, 0, String.Empty);

            //IList<CQMSummary> CQMSummaryList = new List<CQMSummary>();
            //IList<Examination> nullList = null;
            //string ExamTabName = string.Empty;
            //if (ExamList.Count > 0)
            //{
            //    if (ExamList[0].Category.ToUpper() == "GENERAL WITH SPECIALTY")
            //        ExamTabName = "General";
            //    else if (ExamList[0].Category.ToUpper() == "FOCUSED")
            //        ExamTabName = "Focused";
            //    SaveUpdateDelete_DBAndXML_WithTransaction(ref ExamList, ref nullList, null, MACAddress, true, true, ExamList[0].Encounter_ID, ExamTabName);
            //    examScreenList = ExamList;
            //}
            return CQMSummaryList;
        }

        public void DeleteCQMData()
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IList<CQMDetail> ilstCQMDetail = new List<CQMDetail>();
                ISQLQuery sql = iMySession.CreateSQLQuery("truncate cqm_detail");
                ilstCQMDetail = sql.List<CQMDetail>();

                IList<CQMSummary> ilstCQMSummary = new List<CQMSummary>();
                ISQLQuery sqlSummary = iMySession.CreateSQLQuery("truncate cqm_summary");
                ilstCQMSummary = sqlSummary.List<CQMSummary>();
            }
        }

        public IList<PQRI_Measure> GetCQMSummary(string sLegalOrg, string sMeaurementYear, ulong ulPhysicianId)
        {

            IList<PQRI_Measure> ilstPQRI = new List<PQRI_Measure>();
            IList<CQMSummary> ilstCQMSummary = new List<CQMSummary>();
            IList<CQMDetail> ilstCQMDetail = new List<CQMDetail>();
            PQRI_Measure objPQRI = new PQRI_Measure();
            IList<string[]> icdcptListDenominator = new List<string[]>();
            IList<string[]> icdcptListDenominatorExclusion = new List<string[]>();
            IList<string[]> icdcptListDenominatorException = new List<string[]>();
            IList<string[]> icdcptListNumerator = new List<string[]>();

            string Percentage = string.Empty;
            string cleared = string.Empty;


            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(CQMSummary)).Add(Expression.Eq("Legal_Org", sLegalOrg)).Add(Expression.Eq("Measurement_Year",Convert.ToInt32(sMeaurementYear))).Add(Expression.Eq("Physician_ID", ulPhysicianId));
                ilstCQMSummary = criteria.List<CQMSummary>();
           
                for (int iCount = 0; iCount < ilstCQMSummary.Count; iCount++)
                {
                    objPQRI = new PQRI_Measure();

                    icdcptListNumerator = new List<string[]>();
                    icdcptListDenominator = new List<string[]>();
                    icdcptListDenominatorExclusion = new List<string[]>();
                    icdcptListDenominatorException = new List<string[]>();

                    objPQRI.CQM_Summary_ID =Convert.ToInt32(ilstCQMSummary[iCount].Id);
                    objPQRI.Measurement_No = ilstCQMSummary[iCount].Measurement_No;
                    objPQRI.Measurement_Name = ilstCQMSummary[iCount].Measurement_Name;
                    objPQRI.InitialPatientPopulation = ilstCQMSummary[iCount].Initial_Population.ToString();
                    objPQRI.Numerator = ilstCQMSummary[iCount].Numerator.ToString();
                    objPQRI.Denominator = ilstCQMSummary[iCount].Denominator.ToString();
                    objPQRI.DenominatorException = ilstCQMSummary[iCount].Denominator_Exception.ToString();
                    objPQRI.DenominatorExclusion = ilstCQMSummary[iCount].Denominator_Exclusion.ToString();
                    objPQRI.Sort_Order = Convert.ToInt32(ilstCQMSummary[iCount].Id);
                    objPQRI.Measurement_Year = ilstCQMSummary[iCount].Measurement_Year;
                    objPQRI.Percentage = "80%";
                    objPQRI.NumeratorExclusion = ilstCQMSummary[iCount].Numerator_Exclusion.ToString();


                    if (Convert.ToUInt32(objPQRI.Numerator) != 0 && Convert.ToUInt32(objPQRI.Denominator) != 0)
                    {
                        Percentage = Convert.ToString(Decimal.Round(((Convert.ToDecimal(objPQRI.Numerator) / Convert.ToDecimal(objPQRI.Denominator)) * 100), 2) + "%");

                        string[] strDec = Percentage.ToString().Split('%');
                        decimal dDec = Convert.ToDecimal(strDec[0]);
                        decimal dMeasure = Convert.ToDecimal(80);
                        if (dDec >= dMeasure)
                        {
                            cleared = "Yes";
                        }
                        else if (dDec < dMeasure)
                        {
                            cleared = "No";

                        }
                    }
                    else
                    {
                        Percentage = "0%";
                        cleared = "No";
                    }

                    objPQRI.Cleared = cleared;

                    ICriteria criteriaDetail = iMySession.CreateCriteria(typeof(CQMDetail)).Add(Expression.Eq("CQM_Summary_ID", Convert.ToUInt64(objPQRI.CQM_Summary_ID))); //.Add(Expression.Eq("Legal_Org", sLegalOrg)).Add(Expression.Eq("Measurement_Year", Convert.ToInt32(sMeaurementYear))).Add(Expression.Eq("Physician_Library_ID", ulPhysicianId)).Add(Expression.Eq("Measurement_Name", ilstCQMSummary[iCount].Measurement_Name));
                    ilstCQMDetail = criteriaDetail.List<CQMDetail>();


                    //ISQLQuery sql = iMySession.CreateSQLQuery("select * from cqm_detail where measurement_name='" + ilstCQMSummary[iCount].Measurement_Name + "' and Legal_Org='" + sLegalOrg + "' and Physician_Library_Id='" + ulPhysicianId + "' and  Measurement_Year='" +Convert.ToInt32(sMeaurementYear) + "'");
                    //ilstCQMDetail = sql.List<CQMDetail>();

                    for (int iDetailcount = 0; iDetailcount < ilstCQMDetail.Count; iDetailcount++)
                    {
                        if (ilstCQMDetail[iDetailcount].Population_Set == "Denominator")
                        {

                            string[] ary = { ilstCQMDetail[iDetailcount].Encounter_ID.ToString(), ilstCQMDetail[iDetailcount].Human_ID.ToString(), ilstCQMDetail[iDetailcount].ICD, ilstCQMDetail[iDetailcount].Procedure_Code, ilstCQMDetail[iDetailcount].Snomed_Code, ilstCQMDetail[iDetailcount].Loinc_Identifier, ilstCQMDetail[iDetailcount].Documented_Date_Time.ToString(), objPQRI.Measurement_No.Split('v')[0]+"D", objPQRI.Measurement_No.ToString() };
                            icdcptListDenominator.Add(ary);
                        }
                        if (ilstCQMDetail[iDetailcount].Population_Set == "Denominator Exclusion")
                        {

                            string[] ary = { ilstCQMDetail[iDetailcount].Encounter_ID.ToString(), ilstCQMDetail[iDetailcount].Human_ID.ToString(), ilstCQMDetail[iDetailcount].ICD, ilstCQMDetail[iDetailcount].Procedure_Code, ilstCQMDetail[iDetailcount].Snomed_Code, ilstCQMDetail[iDetailcount].Loinc_Identifier, ilstCQMDetail[iDetailcount].Documented_Date_Time.ToString(), objPQRI.Measurement_No.Split('v')[0] + "DE", objPQRI.Measurement_No.ToString() };
                            icdcptListDenominatorExclusion.Add(ary);
                        }
                        if (ilstCQMDetail[iDetailcount].Population_Set == "Denominator Exception")
                        {

                            string[] ary = { ilstCQMDetail[iDetailcount].Encounter_ID.ToString(), ilstCQMDetail[iDetailcount].Human_ID.ToString(), ilstCQMDetail[iDetailcount].ICD, ilstCQMDetail[iDetailcount].Procedure_Code, ilstCQMDetail[iDetailcount].Snomed_Code, ilstCQMDetail[iDetailcount].Loinc_Identifier, ilstCQMDetail[iDetailcount].Documented_Date_Time.ToString(), objPQRI.Measurement_No.Split('v')[0] + "DEX", objPQRI.Measurement_No .ToString() };
                            icdcptListDenominatorException.Add(ary);
                        }
                        if (ilstCQMDetail[iDetailcount].Population_Set == "Numerator")
                        {

                            string[] ary = { ilstCQMDetail[iDetailcount].Encounter_ID.ToString(), ilstCQMDetail[iDetailcount].Human_ID.ToString(), ilstCQMDetail[iDetailcount].ICD, ilstCQMDetail[iDetailcount].Procedure_Code, ilstCQMDetail[iDetailcount].Loinc_Code, ilstCQMDetail[iDetailcount].Loinc_Identifier, ilstCQMDetail[iDetailcount].Snomed_Code, objPQRI.Measurement_No .Split('v')[0] + "N","","", objPQRI.Measurement_No .ToString() };
                            icdcptListNumerator.Add(ary);
                        }

                    }

                    objPQRI.ICDCPTNumeratorList = icdcptListNumerator;
                    objPQRI.ICDCPTDenominatorList = icdcptListDenominator;
                    objPQRI.ICDCPTDenominatorExclusionList = icdcptListDenominatorExclusion;
                    objPQRI.ICDCPTDenominatorExceptionList = icdcptListDenominatorException;
                    ilstPQRI.Add(objPQRI);

                   
                }

                iMySession.Close();
            }
            return ilstPQRI;
        }
        #endregion

    }
}
