using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
using System.Web.Mail;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Diagnostics;
using System.Web;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading;
using System.Web.UI.WebControls;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IHumanMananger : IManagerBase<Human, ulong>
    {
        //int GetTotalPatientsForCriteria(string sLastName, string sFirstName, string ulAccNo,
        //        string sActive, DateTime dtDOB, string sSSN, string sSex, string sExtAccNo, string PolicyHolderId, string MedicalRecordNo);

        //Stream FindPatient(string sLastName, string sFirstName, string ulAccNo,string sActive, DateTime dtDOB, string sSSN, string sSex, string sExtAccNo, string PolicyHolderId, string MedicalRecordNo, int PageNumber, int MaxResultSet, string HumanType);
        //int GetTotalPatientsForCriteriaSOUNDEX(string sLastName, string sFirstName, string ulAccNo,
        //        string sActive, DateTime dtDOB, string sSSN, string sSex, string sExtAccNo, string PolicyHolderId, string MedicalRecordNo);

        //Stream FindPatientSOUNDEX(string sLastName, string sFirstName, string ulAccNo,string sActive, DateTime dtDOB, string sSSN, string sSex, string sExtAccNo, string PolicyHolderId, string MedicalRecordNo, int PageNumber, int MaxResultSet, string HumanType);
        //int BatchOperationsToHuman(IList<Human> savelist, IList<Human> updtList, IList<Human> delList, ISession MySession, string MACAddress);
        //IList<BatchDTO> GetHumanListForCPManualReport(ulong ulWFObjectID, string sDOOS, string sBatchName);
        //void UpdateACOQueries(ulong HumanId, string IsPatientAccepted, string IsPatientDiscussed, ulong IsDiscussedBy);
        //string GetHumanSex(ulong HumanId);
        //IList<Human> GetHumanListForEhrMeasure(string HumanId);



        #region Interface Members
        IList<Human> GetPatientDetailsUsingPatientInformattion(ulong ulHumanId);

        Human AppendToHuman(Human objHuman, string MACAddress);

        Human UpdateToHuman(Human objHuman, string MACAddress);

        HumanDTO GetPatientDetailsUsingPatientDetails(string sLastname, string sFirstname, DateTime dtBirthDate, string sSex, string sMedicalRecordNo, string sExternalAccNo, string sLegalOrg);

        ulong BatchOperationsToQuickPatient(IList<Human> ListToInsertHuman, IList<Human> ListToUpdateHuman, IList<Eligibility_Verification> ListToInserElig, IList<VisitPayment> ListToInsertVisitPayment, IList<Check> SaveCheckList, IList<PPHeader> SavePPHeaderList, IList<PPLineItem> SavePPLineItemList, IList<AccountTransaction> SaveAccountList, IList<PatientNotes> SavePatList, PatientInsuredPlan SavePatInsList, string MACAddress, ulong checkOut_Encounter_Id, IList<VisitPaymentHistory> SaveVisitPaymentHistoryList, bool bCheckInToMa = true);

        FillQuickPatient LoadQuickPatient(ulong EncounterId);


        FillQuickPatient LoadQuickPatientforPatientPayment(ulong EncounterId, Boolean IsEncounterBased);

        Stream SearchGeneratePatientLists(string AgeCondition, int Age, int AgeFrom, int AgeTo, string Gender, string Race, string Ethnicity, string CommPreference, IList<string> MedicationList, IList<string> Medication_allergyList, IList<string> ProblemList, string[] sLabResultCondition, string[] sCodition, IList<string> FreqProblemList, string fromDate, string toDate, int pageNumber, int maxResults);

        void InsertOrUpdateHumanByRcopia(IList<Human> ilsthuman, string sMacAddress);

        void UpdateFailedRCopiaHuman(Human objHuman, string MACAddress);

        HumanDTO GetHumanInuranceAndCarrierDetails(ulong ulHuman_id);

        IList<Human> CheckPatientPortal(string sMail, string sPassword, ulong PatientId);

        Human GetHumanIfDuplicateEMail(string email, string sLegalOrg);

        IList<Human> CheckQuarantorPatientPortal(string sMail, string sPassword);

        void SaveHumanPatientPortal(int id, string sPassword);

        IList<Human> patientdetails(string patientid);

        Human AppendBatchToHuman(Human objHuman, PatGuarantor objPatGuarantor, string sCarrier);

        Human UpdateBatchToHuman(Human objHuman, PatGuarantor objPatGuarantor, PatientInsuredPlan objPatInsPlan, string sPriCarrier);

        HumanDTO FindHumanForPaymentPostingSearch(ulong ulHumanID, string FirstName, string LastName, DateTime dtDOB);

        IList<Human> GetHumanListForDemoReport(string sDOOS, string sBatchName, string sObjectType, ulong ulWFObjectID);

        //IList<BatchDTO> GetHumanListForCPAutoReport(string sDOOS, string sBatchName, string sObjectType, ulong ulWFObjectID);

        void UpdateACOQueries(ulong HumanId, string IsPatientAccepted, string IsPatientDiscussed, ulong IsDiscussedBy, ulong Encounter_id, bool flag, string Source_info, string UserName, DateTime dt);

        bool IsACOValid(ulong HumanID);

        void UpdateHumanEmailDetails(IList<Human> UpdatehumanList, string MACAddress);

        IList<Human> GetHumanByName(string FirstName, string LastName, DateTime DOB, string Sex);

        int GetHumanListForEhrMeasureRecordCount(string HumanId);

        IList<Human> GetHumanListForEhrMeasureByLimit(string HumanId, int PageNumber, int maxResults);

        IList<Human> GetHumanListById(IList<ulong> HumanLst);

        ulong SaveHumanforSummary(IList<Human> lsthuman);

        string IsACOEligibility(ulong HumanID);

        Stream ExportPatientLists(string AgeCondition, int Age, int AgeFrom, int AgeTo, string Gender, string Race, string Ethnicity, string CommPreference, IList<string> MedicationList, IList<string> Medication_allergyList, IList<string> ProblemList, string[] sLabResultCondition, string[] sCodition, IList<string> FreqProblemList, string fromDate, string toDate);

        string GetPatientNameByHumanID(ulong HumanID);

        IList<string> GetACODetails(ulong HumanID);

        FillQuickPatient GetStaticLookupandCarrier(string[] Field_Name);

        FillQuickPatient GetVisitPaymentDetails(string VisitID, string PPHeaderID, string PPLineItemID, string CheckID);

        FillQuickPatientArc GetVisitPaymentDetailsArc(string VisitID, string PPHeaderID, string PPLineItemID, string CheckID);

        //IList<Encounter> GetAutoIncreamentVoucherNo(ulong Human);
        Human GetHumanIdbyname(ulong ulHumanID, string sFirstname, string sLastname, string sDob);

        #endregion
    }
    public partial class HumanManager : ManagerBase<Human, ulong>, IHumanMananger
    {
        #region Constructors

        public HumanManager()
            : base()
        {

        }
        public HumanManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Method

        #region UnUsed Methods
        //public int GetTotalPatientsForCriteria(string sLastName, string sFirstName, string ulAccNo,
        //                string sActive, DateTime dtDOB, string sSSN, string sSex, string sExtAccNo, string PolicyHolderId, string MedicalRecordNo)
        //{
        //    //IList<Human> HumanList;

        //    IQuery query1 = null;
        //    if (PolicyHolderId == string.Empty)
        //    {
        //        query1 = session.GetISession().GetNamedQuery("Find.Patient.Count.Without.Insurance");
        //    }
        //    else
        //    {
        //        query1 = session.GetISession().GetNamedQuery("Find.Patient.Count.With.Insurance");
        //    }
        //    query1.SetString(0, sLastName + "%");
        //    query1.SetString(1, sFirstName + "%");
        //    query1.SetString(2, sFirstName + "%");
        //    query1.SetString(3, sLastName + "%");
        //    if (dtDOB == DateTime.MinValue)
        //    {
        //        query1.SetString(4, "%");
        //    }
        //    else
        //    {
        //        query1.SetString(4, dtDOB.ToString("yyyy-MM-dd") + "%");
        //    }
        //    query1.SetString(5, sSSN + "%");
        //    query1.SetString(6, sActive + "%");
        //    query1.SetString(7, sSex + "%");
        //    query1.SetString(8, ulAccNo + "%");
        //    query1.SetString(9, sExtAccNo + "%");
        //    query1.SetString(10, "%|" + sExtAccNo + "%");
        //    if (PolicyHolderId == string.Empty)
        //    {
        //        query1.SetString(11, MedicalRecordNo + "%");
        //    }
        //    else
        //    {
        //        query1.SetString(11, PolicyHolderId + "%");
        //        query1.SetString(12, MedicalRecordNo + "%");
        //    }

        //    return Convert.ToInt32(query1.List()[0]);
        //}


        //public Stream FindPatient(string sLastName, string sFirstName, string ulAccNo,
        //                         string sActive, DateTime dtDOB, string sSSN, string sSex, string sExtAccNo, string PolicyHolderId, string MedicalRecordNo, int PageNumber, int MaxResultSet, string HumanType)
        //{
        //    var stream = new MemoryStream();
        //    var serializer = new NetDataContractSerializer();
        //    //       IList<FindPatient> HumanList = new List<FindPatient>();
        //    //FindPatient human;
        //    ArrayList ary = null;
        //    //IQuery query1 = null;

        //    //if (PolicyHolderId == string.Empty)
        //    //{
        //    //    if (PageNumber != 1)
        //    //    {
        //    //        query1 = session.GetISession().GetNamedQuery("Find.Patient.Without.Insurance");
        //    //    }
        //    //    else
        //    //    {
        //    //        query1 = session.GetISession().GetNamedQuery("Find.Patient.Without.Insurance.All");
        //    //    }

        //    //}
        //    //else
        //    //{
        //    //    if (PageNumber != 1)
        //    //    {
        //    //        query1 = session.GetISession().GetNamedQuery("Find.Patient.With.Insurance");
        //    //    }
        //    //    else
        //    //    {
        //    //        query1 = session.GetISession().GetNamedQuery("Find.Patient.With.Insurance.All");
        //    //    }

        //    //}
        //    for (int i = sSSN.Length - 1; i > 0; i--)
        //    {
        //        if (sSSN.Trim().EndsWith("-") == true)
        //        {
        //            int index = sSSN.LastIndexOf('-');
        //            sSSN = sSSN.Remove(index);
        //        }
        //        else
        //        {
        //            break;
        //        }

        //    }

        //    string sQuery = string.Empty;
        //    if (PolicyHolderId == string.Empty)
        //    {
        //        if (PageNumber != 1)
        //        {
        //            sQuery = "SELECT P.Human_ID,P.Prefix,P.Last_Name,P.First_Name,P.MI,P.Suffix,cast(p.Birth_Date as char(100)) as BirthDate,P.SSN,p.Account_Status,p.Medical_Record_Number,P.Patient_Account_External,P.People_In_Collection,ifnull(i.policy_holder_id,''),concat(ifnull(pl.Physician_Prefix,''),' ',ifnull(pl.Physician_First_Name,''),' ',ifnull(pl.Physician_Middle_Name,''),' ',ifnull(pl.Physician_Last_Name,''),' ',ifnull(pl.Physician_Suffix,'')) as PCP,P.Patient_Status,P.Human_Type,P.Sex,p.ACO_Is_Eligible_Patient from Human P left join pat_insured_plan i on (i.Human_ID=P.Human_ID and i.Insurance_Type='Primary') left join physician_library pl on pl.physician_library_id=i.pcp_id where ";
        //        }
        //        else
        //        {
        //            sQuery = "SELECT P.Human_ID,P.Prefix,P.Last_Name,P.First_Name,P.MI,P.Suffix,cast(p.Birth_Date as char(100)) as BirthDate,P.SSN,p.Account_Status,p.Medical_Record_Number,P.Patient_Account_External,P.People_In_Collection,ifnull(i.policy_holder_id,''),concat(ifnull(pl.Physician_Prefix,''),' ',ifnull(pl.Physician_First_Name,''),' ',ifnull(pl.Physician_Middle_Name,''),' ',ifnull(pl.Physician_Last_Name,''),' ',ifnull(pl.Physician_Suffix,'')) as PCP,P.Patient_Status,P.Human_Type,P.Sex,p.ACO_Is_Eligible_Patient from Human P left join pat_insured_plan i on (i.Human_ID=P.Human_ID and i.Insurance_Type='Primary') left join physician_library pl on pl.physician_library_id=i.pcp_id where ";
        //        }
        //    }
        //    else
        //    {
        //        if (PageNumber != 1)
        //        {
        //            sQuery = "SELECT P.Human_ID,P.Prefix,P.Last_Name,P.First_Name,P.MI,P.Suffix,cast(p.Birth_Date as char(100)) as BirthDate,P.SSN,p.Account_Status,p.Medical_Record_Number,P.Patient_Account_External,P.People_In_Collection,ifnull(i.policy_holder_id,''),concat(ifnull(pl.Physician_Prefix,''),' ',ifnull(pl.Physician_First_Name,''),' ',ifnull(pl.Physician_Middle_Name,''),' ',ifnull(pl.Physician_Last_Name,''),' ',ifnull(pl.Physician_Suffix,'')) as PCP,P.Patient_Status,P.Human_Type,P.Sex,p.ACO_Is_Eligible_Patient from Human P left join pat_insured_plan i on (i.Human_ID=P.Human_ID and i.Insurance_Type='Primary') left join physician_library pl on pl.physician_library_id=i.pcp_id where ";

        //        }
        //        else
        //        {
        //            sQuery = "SELECT P.Human_ID,P.Prefix,P.Last_Name,P.First_Name,P.MI,P.Suffix,cast(p.Birth_Date as char(100)) as BirthDate,P.SSN,p.Account_Status,p.Medical_Record_Number,P.Patient_Account_External,P.People_In_Collection,ifnull(i.policy_holder_id,''),concat(ifnull(pl.Physician_Prefix,''),' ',ifnull(pl.Physician_First_Name,''),' ',ifnull(pl.Physician_Middle_Name,''),' ',ifnull(pl.Physician_Last_Name,''),' ',ifnull(pl.Physician_Suffix,'')) as PCP,P.Patient_Status,P.Human_Type,P.Sex,p.ACO_Is_Eligible_Patient from Human P left join pat_insured_plan i on (i.Human_ID=P.Human_ID and i.Insurance_Type='Primary') left join physician_library pl on pl.physician_library_id=i.pcp_id where ";
        //        }

        //    }
        //    string sWhere = string.Empty;

        //    if (ulAccNo != "")
        //    {
        //        if (sWhere == string.Empty)
        //        {
        //            sWhere = "P.Human_ID  like '" + ulAccNo + "%" + "'";
        //        }
        //        else
        //        {
        //            sWhere += " and P.Human_ID  like '" + ulAccNo + "%" + "'";
        //        }

        //    }

        //    if (sLastName != "" || sFirstName != "")
        //    {
        //        sWhere = "((P.Last_Name like '" + sLastName + "%" + "' and P.First_Name like '" + sFirstName + "%" + "') OR  (P.Last_Name like '" + sFirstName + "%" + "' and P.First_Name like '" + sLastName + "%" + "')) ";
        //    }

        //    if (dtDOB != DateTime.MinValue)
        //    {
        //        if (sWhere == string.Empty)
        //        {
        //            sWhere = "P.Birth_Date like '" + dtDOB.ToString("yyyy-MM-dd") + "%" + "'";
        //        }
        //        else
        //        {
        //            sWhere += " and P.Birth_Date like '" + dtDOB.ToString("yyyy-MM-dd") + "%" + "'";
        //        }
        //    }
        //    if (sSSN != "")
        //    {
        //        if (sWhere == string.Empty)
        //        {
        //            sWhere = "P.SSN like '" + sSSN + "%" + "'";
        //        }
        //        else
        //        {
        //            sWhere += " and P.SSN like '" + sSSN + "%" + "'";
        //        }

        //    }

        //    if (sActive != "")
        //    {
        //        if (sWhere == string.Empty)
        //        {
        //            sWhere = "P.Account_Status  like '" + sActive + "%" + "'";
        //        }
        //        else
        //        {
        //            sWhere += " and P.Account_Status  like '" + sActive + "%" + "'";
        //        }

        //    }

        //    if (sSex != "")
        //    {
        //        if (sWhere == string.Empty)
        //        {
        //            sWhere = "P.Sex  like '" + sSex + "%" + "'";
        //        }
        //        else
        //        {
        //            sWhere += " and P.Sex  like '" + sSex + "%" + "'";
        //        }

        //    }



        //    if (sExtAccNo != "")
        //    {
        //        if (sWhere == string.Empty)
        //        {
        //            sWhere = "(P.Patient_Account_External like '" + sExtAccNo + "%" + "' or P.Patient_Account_External like '" + "%|" + sExtAccNo + "%" + "')";
        //        }
        //        else
        //        {
        //            sWhere += " and (P.Patient_Account_External like '" + sExtAccNo + "%" + "' or P.Patient_Account_External like '" + "%|" + sExtAccNo + "%" + "')";
        //        }

        //    }

        //    if (MedicalRecordNo != "")
        //    {
        //        if (sWhere == string.Empty)
        //        {
        //            sWhere = "P.Medical_Record_Number  like '" + MedicalRecordNo + "%" + "'";
        //        }
        //        else
        //        {
        //            sWhere += " and P.Medical_Record_Number  like '" + MedicalRecordNo + "%" + "'";
        //        }

        //    }
        //    if (HumanType != "")
        //    {
        //        if (sWhere == string.Empty)
        //        {
        //            sWhere = "P.Human_Type   like '" + HumanType + "%" + "'";
        //        }
        //        else
        //        {
        //            sWhere += " and P.Human_Type   like '" + HumanType + "%" + "'";
        //        }

        //    }

        //    if (PolicyHolderId != "")
        //    {
        //        if (sWhere == string.Empty)
        //        {
        //            sWhere = "i.Policy_Holder_ID   like '" + PolicyHolderId + "%" + "'";
        //        }
        //        else
        //        {
        //            sWhere += " and i.Policy_Holder_ID   like '" + PolicyHolderId + "%" + "'";
        //        }

        //    }

        //    sQuery += sWhere;
        //    sQuery += "order by P.Last_Name,P.First_Name,P.Birth_Date, P.Human_ID";

        //    PageNumber = PageNumber - 1;
        //    if (PageNumber != 0)
        //    {
        //        sQuery += " limit ";
        //        sQuery += PageNumber * MaxResultSet + "," + MaxResultSet;

        //    }

        //    //query1.SetString(0, sLastName + "%");
        //    //query1.SetString(1, sFirstName + "%");
        //    //query1.SetString(2, sFirstName + "%");
        //    //query1.SetString(3, sLastName + "%");
        //    //if (dtDOB == DateTime.MinValue)
        //    ////if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
        //    //{
        //    //    query1.SetString(4, "%");
        //    //}
        //    //else
        //    //{
        //    //    query1.SetString(4, dtDOB.ToString("yyyy-MM-dd") + "%");
        //    //}
        //    // Added by Manimozhi - On 29th june 2012 - bug id 10239 

        //    //if (sSSN.EndsWith("-") == true)
        //    //{
        //    //    int index = sSSN.LastIndexOf('-');
        //    //    sSSN = sSSN.Remove(index);
        //    //}
        //    //query1.SetString(5, sSSN.Trim() + "%");
        //    //query1.SetString(6, sActive + "%");
        //    //query1.SetString(7, sSex + "%");
        //    //query1.SetString(8, ulAccNo + "%");
        //    //query1.SetString(9, sExtAccNo + "%");
        //    //query1.SetString(10, "%|" + sExtAccNo + "%");

        //    //query1.SetString(0, sWhere);
        //    //if (PolicyHolderId == string.Empty)
        //    //{
        //    //    //query1.SetString(11, MedicalRecordNo + "%");
        //    //    //query1.SetString(12, HumanType + "%");
        //    //    PageNumber = PageNumber - 1;
        //    //    if (PageNumber != 0)
        //    //    {
        //    //        query1.SetInt32(1, PageNumber * MaxResultSet);
        //    //        query1.SetInt32(2, MaxResultSet);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    //query1.SetString(11, PolicyHolderId + "%");
        //    //    //query1.SetString(12, MedicalRecordNo + "%");
        //    //    //query1.SetString(13, HumanType + "%");
        //    //    PageNumber = PageNumber - 1;
        //    //    if (PageNumber != 0)
        //    //    {
        //    //        query1.SetInt32(1, PageNumber * MaxResultSet);
        //    //        query1.SetInt32(2, MaxResultSet);
        //    //    }

        //    //}
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ISQLQuery HumanQuery = iMySession.CreateSQLQuery(sQuery);
        //        ary = new ArrayList(HumanQuery.List());
        //        //Jayachandran 02-08-2010

        //        int result = 0;
        //        if (PageNumber == 0)
        //        {
        //            if (ary.Count > MaxResultSet)
        //            {
        //                result = MaxResultSet;
        //            }
        //            else
        //            {
        //                result = ary.Count;
        //            }

        //        }
        //        else
        //        {
        //            result = ary.Count;
        //        }

        //        DataSet ds = new DataSet();
        //        DataRow dr = null;
        //        DataTable dt = new DataTable();

        //        if (result != 0)
        //        {
        //            dt.Columns.Add("Account #", typeof(string));
        //            dt.Columns.Add("Last Name", typeof(string));
        //            dt.Columns.Add("First Name", typeof(string));
        //            dt.Columns.Add("Middle Name", typeof(string));
        //            dt.Columns.Add("Date Of Birth", typeof(string));
        //            dt.Columns.Add("SSN", typeof(string));
        //            dt.Columns.Add("Status", typeof(string));
        //            dt.Columns.Add("Medical Record #", typeof(string));
        //            dt.Columns.Add("External Account #", typeof(string));
        //            dt.Columns.Add("Subscriber ID", typeof(string));
        //            dt.Columns.Add("Is People Collection", typeof(string));
        //            dt.Columns.Add("PCP", typeof(string));
        //            dt.Columns.Add("Patient Status", typeof(string));
        //            dt.Columns.Add("Patient Type", typeof(string));
        //            dt.Columns.Add("Gender", typeof(string));
        //            dt.Columns.Add("ACO_Eligible", typeof(string));

        //            for (int i = 0; i < result; i++)
        //            {
        //                object[] obj = (object[])ary[i];
        //                if (obj != null)
        //                {
        //                    dr = dt.NewRow();
        //                    dr["Account #"] = Convert.ToUInt64(obj[0]);
        //                    dr["Last Name"] = obj[2].ToString();
        //                    dr["First Name"] = obj[3].ToString();
        //                    dr["Middle Name"] = obj[4].ToString();
        //                    dr["Date Of Birth"] = Convert.ToDateTime(obj[6]).ToString("dd-MMM-yyyy");
        //                    dr["SSN"] = obj[7].ToString();
        //                    dr["Status"] = obj[8].ToString();
        //                    dr["Medical Record #"] = obj[9].ToString();
        //                    dr["External Account #"] = obj[10].ToString();

        //                    IList<PatientInsuredPlan> ilstHuman = new List<PatientInsuredPlan>();
        //                    ICriteria criteria = session.GetISession().CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", Convert.ToUInt64(obj[0]))).Add(Expression.Eq("Insurance_Type", "PRIMARY"));
        //                    ilstHuman = criteria.List<PatientInsuredPlan>();
        //                    if (ilstHuman.Count > 0)
        //                    {
        //                        dr["Subscriber ID"] = ilstHuman[0].Policy_Holder_ID;
        //                    }

        //                    dr["Is People Collection"] = obj[11].ToString();
        //                    dr["PCP"] = obj[13].ToString();
        //                    dr["Patient Status"] = obj[14].ToString();
        //                    dr["Patient Type"] = obj[15].ToString();
        //                    dr["Gender"] = obj[16].ToString();
        //                    dr["ACO_Eligible"] = obj[17].ToString();
        //                    dt.Rows.Add(dr);
        //                }
        //            }
        //        }
        //        ds.Tables.Add(dt);

        //        Hashtable hsResult = new Hashtable();
        //        hsResult.Add("PatientList", dt);

        //        if (PageNumber == 0)
        //        {
        //            hsResult.Add("TotalCount", ary.Count);
        //        }


        //        serializer.WriteObject(stream, hsResult);
        //        stream.Seek(0L, SeekOrigin.Begin);
        //        iMySession.Close();
        //    }
        //    return stream;
        //}

        //public int GetTotalPatientsForCriteriaSOUNDEX(string sLastName, string sFirstName, string ulAccNo,
        //               string sActive, DateTime dtDOB, string sSSN, string sSex, string sExtAccNo, string PolicyHolderId, string MedicalRecordNo)
        //{
        //    //IList<Human> HumanList;

        //    IQuery query1 = null;
        //    if (PolicyHolderId == string.Empty)
        //    {
        //        if (sLastName != string.Empty && sFirstName != string.Empty)
        //        {
        //            query1 = session.GetISession().GetNamedQuery("Find.Patient.Query1.Count.SOUNDEX.Without.Insurance");
        //        }
        //        if (sLastName != string.Empty && sFirstName == string.Empty)
        //        {
        //            query1 = session.GetISession().GetNamedQuery("Find.Patient.Query2.Count.SOUNDEX.Without.Insurance");
        //        }
        //        if (sLastName == string.Empty && sFirstName != string.Empty)
        //        {
        //            query1 = session.GetISession().GetNamedQuery("Find.Patient.Query3.Count.SOUNDEX.Without.Insurance");
        //        }

        //        if (sLastName == string.Empty && sFirstName == string.Empty)
        //        {
        //            query1 = session.GetISession().GetNamedQuery("Find.Patient.Query4.Count.SOUNDEX.Without.Insurance");
        //        }
        //    }
        //    else
        //    {
        //        if (sLastName != string.Empty && sFirstName != string.Empty)
        //        {
        //            query1 = session.GetISession().GetNamedQuery("Find.Patient.Query1.Count.SOUNDEX.With.Insurance");
        //        }
        //        if (sLastName != string.Empty && sFirstName == string.Empty)
        //        {
        //            query1 = session.GetISession().GetNamedQuery("Find.Patient.Query2.Count.SOUNDEX.With.Insurance");
        //        }
        //        if (sLastName == string.Empty && sFirstName != string.Empty)
        //        {
        //            query1 = session.GetISession().GetNamedQuery("Find.Patient.Query3.Count.SOUNDEX.With.Insurance");
        //        }
        //        if (sLastName == string.Empty && sFirstName == string.Empty)
        //        {
        //            query1 = session.GetISession().GetNamedQuery("Find.Patient.Query4.Count.SOUNDEX.With.Insurance");
        //        }

        //    }
        //    if (sLastName != string.Empty && sFirstName != string.Empty)
        //    {
        //        query1.SetString(0, sLastName);
        //        query1.SetString(1, sFirstName);
        //        query1.SetString(2, sFirstName);
        //        query1.SetString(3, sLastName);
        //        if (dtDOB == DateTime.MinValue)
        //        //if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
        //        {
        //            query1.SetString(4, "%");
        //        }
        //        else
        //        {
        //            query1.SetString(4, dtDOB.ToString("yyyy-MM-dd") + "%");
        //        }
        //        query1.SetString(5, sSSN + "%");
        //        query1.SetString(6, sActive + "%");
        //        query1.SetString(7, sSex + "%");
        //        query1.SetString(8, ulAccNo + "%");
        //        query1.SetString(9, sExtAccNo + "%");
        //        if (PolicyHolderId == string.Empty)
        //        {
        //            query1.SetString(10, MedicalRecordNo + "%");
        //        }
        //        else
        //        {
        //            query1.SetString(10, PolicyHolderId + "%");
        //            query1.SetString(11, MedicalRecordNo + "%");
        //        }
        //    }
        //    if (sLastName != string.Empty && sFirstName == string.Empty)
        //    {
        //        query1.SetString(0, sLastName);
        //        if (dtDOB == DateTime.MinValue)
        //        //if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
        //        {
        //            query1.SetString(1, "%");
        //        }
        //        else
        //        {
        //            query1.SetString(1, dtDOB.ToString("yyyy-MM-dd") + "%");
        //        }
        //        query1.SetString(2, sSSN + "%");
        //        query1.SetString(3, sActive + "%");
        //        query1.SetString(4, sSex + "%");
        //        query1.SetString(5, ulAccNo + "%");
        //        query1.SetString(6, sExtAccNo + "%");
        //        if (PolicyHolderId == string.Empty)
        //        {
        //            query1.SetString(7, MedicalRecordNo + "%");
        //        }
        //        else
        //        {
        //            query1.SetString(7, PolicyHolderId + "%");
        //            query1.SetString(8, MedicalRecordNo + "%");
        //        }
        //    }
        //    if (sLastName == string.Empty && sFirstName != string.Empty)
        //    {
        //        query1.SetString(0, sFirstName);
        //        if (dtDOB == DateTime.MinValue)
        //        //if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
        //        {
        //            query1.SetString(1, "%");
        //        }
        //        else
        //        {
        //            query1.SetString(1, dtDOB.ToString("yyyy-MM-dd") + "%");
        //        }
        //        query1.SetString(2, sSSN + "%");
        //        query1.SetString(3, sActive + "%");
        //        query1.SetString(4, sSex + "%");
        //        query1.SetString(5, ulAccNo + "%");
        //        query1.SetString(6, sExtAccNo + "%");
        //        if (PolicyHolderId == string.Empty)
        //        {
        //            query1.SetString(7, MedicalRecordNo + "%");
        //        }
        //        else
        //        {
        //            query1.SetString(7, PolicyHolderId + "%");
        //            query1.SetString(8, MedicalRecordNo + "%");
        //        }
        //    }
        //    if (sLastName == string.Empty && sFirstName == string.Empty)
        //    {
        //        if (dtDOB == DateTime.MinValue)
        //        //if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
        //        {
        //            query1.SetString(0, "%");
        //        }
        //        else
        //        {
        //            query1.SetString(0, dtDOB.ToString("yyyy-MM-dd") + "%");
        //        }
        //        query1.SetString(1, sSSN + "%");
        //        query1.SetString(2, sActive + "%");
        //        query1.SetString(3, sSex + "%");
        //        query1.SetString(4, ulAccNo + "%");
        //        query1.SetString(5, sExtAccNo + "%");
        //        if (PolicyHolderId == string.Empty)
        //        {
        //            query1.SetString(6, MedicalRecordNo + "%");
        //        }
        //        else
        //        {
        //            query1.SetString(6, PolicyHolderId + "%");
        //            query1.SetString(7, MedicalRecordNo + "%");
        //        }
        //    }

        //    return Convert.ToInt32(query1.List()[0]);
        //}
        //public Stream FindPatientSOUNDEX(string sLastName, string sFirstName, string ulAccNo,
        //                      string sActive, DateTime dtDOB, string sSSN, string sSex, string sExtAccNo, string PolicyHolderId, string MedicalRecordNo, int PageNumber, int MaxResultSet, string HumanType)
        //{
        //    var stream = new MemoryStream();
        //    var serializer = new NetDataContractSerializer();
        //    IList<FindPatient> HumanList = new List<FindPatient>();
        //    //FindPatient human;
        //    ArrayList ary = null;
        //    IQuery query1 = null;
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        if (PolicyHolderId == string.Empty)
        //        {
        //            if (sLastName != string.Empty && sFirstName != string.Empty)
        //            {
        //                if (PageNumber != 1)
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query1.SOUNDEX.Without.Insurance");
        //                }
        //                else
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query1.SOUNDEX.Without.Insurance.All");
        //                }

        //            }
        //            if (sLastName != string.Empty && sFirstName == string.Empty)
        //            {
        //                if (PageNumber != 1)
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query2.SOUNDEX.Without.Insurance");
        //                }
        //                else
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query2.SOUNDEX.Without.Insurance.All");
        //                }

        //            }
        //            if (sLastName == string.Empty && sFirstName != string.Empty)
        //            {
        //                if (PageNumber != 1)
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query3.SOUNDEX.Without.Insurance");
        //                }
        //                else
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query3.SOUNDEX.Without.Insurance.All");
        //                }

        //            }
        //            if (sLastName == string.Empty && sFirstName == string.Empty)
        //            {
        //                if (PageNumber != 1)
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query4.SOUNDEX.Without.Insurance");
        //                }
        //                else
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query4.SOUNDEX.Without.Insurance.All");
        //                }

        //            }
        //        }
        //        else
        //        {
        //            if (sLastName != string.Empty && sFirstName != string.Empty)
        //            {
        //                if (PageNumber != 1)
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query1.SOUNDEX.With.Insurance");
        //                }
        //                else
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query1.SOUNDEX.With.Insurance.All");
        //                }

        //            }
        //            if (sLastName != string.Empty && sFirstName == string.Empty)
        //            {
        //                if (PageNumber != 1)
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query2.SOUNDEX.With.Insurance");
        //                }
        //                else
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query2.SOUNDEX.With.Insurance.All");
        //                }

        //            }
        //            if (sLastName == string.Empty && sFirstName != string.Empty)
        //            {
        //                if (PageNumber != 1)
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query3.SOUNDEX.With.Insurance");
        //                }
        //                else
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query3.SOUNDEX.With.Insurance.All");
        //                }

        //            }
        //            if (sLastName == string.Empty && sFirstName == string.Empty)
        //            {
        //                if (PageNumber != 1)
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query4.SOUNDEX.With.Insurance");
        //                }
        //                else
        //                {
        //                    query1 = iMySession.GetNamedQuery("Find.Patient.Query4.SOUNDEX.With.Insurance.All");
        //                }

        //            }
        //        }

        //        if (sLastName != string.Empty && sFirstName != string.Empty)
        //        {
        //            query1.SetString(0, sLastName);
        //            query1.SetString(1, sFirstName);
        //            query1.SetString(2, sFirstName);
        //            query1.SetString(3, sLastName);
        //            if (dtDOB == DateTime.MinValue)
        //            //if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
        //            {
        //                query1.SetString(4, "%");
        //            }
        //            else
        //            {
        //                query1.SetString(4, dtDOB.ToString("yyyy-MM-dd") + "%");
        //            }
        //            // Added by Manimozhi - On 29th june 2012 - bug id 10239 
        //            for (int i = sSSN.Length - 1; i > 0; i--)
        //            {
        //                if (sSSN.Trim().EndsWith("-") == true)
        //                {
        //                    int index = sSSN.LastIndexOf('-');
        //                    sSSN = sSSN.Remove(index);
        //                }
        //                else
        //                {
        //                    break;
        //                }

        //            }
        //            query1.SetString(5, sSSN.Trim() + "%");
        //            query1.SetString(6, sActive + "%");
        //            query1.SetString(7, sSex + "%");
        //            query1.SetString(8, ulAccNo + "%");
        //            query1.SetString(9, sExtAccNo + "%");
        //            if (PolicyHolderId == string.Empty)
        //            {
        //                query1.SetString(10, MedicalRecordNo + "%");
        //                query1.SetString(11, HumanType + "%");
        //                PageNumber = PageNumber - 1;
        //                if (PageNumber != 0)
        //                {
        //                    query1.SetInt32(12, PageNumber * MaxResultSet);
        //                    query1.SetInt32(13, MaxResultSet);
        //                }
        //            }
        //            else
        //            {
        //                query1.SetString(10, PolicyHolderId + "%");
        //                query1.SetString(11, MedicalRecordNo + "%");
        //                query1.SetString(12, HumanType + "%");
        //                PageNumber = PageNumber - 1;
        //                if (PageNumber != 0)
        //                {
        //                    query1.SetInt32(13, PageNumber * MaxResultSet);
        //                    query1.SetInt32(14, MaxResultSet);
        //                }
        //            }

        //        }
        //        if (sLastName != string.Empty && sFirstName == string.Empty)
        //        {
        //            query1.SetString(0, sLastName);
        //            if (dtDOB == DateTime.MinValue)
        //            //if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
        //            {
        //                query1.SetString(1, "%");
        //            }
        //            else
        //            {
        //                query1.SetString(1, dtDOB.ToString("yyyy-MM-dd") + "%");
        //            }
        //            // Added by Manimozhi - On 29th june 2012 - bug id 10239 
        //            for (int i = sSSN.Length - 1; i > 0; i--)
        //            {
        //                if (sSSN.Trim().EndsWith("-") == true)
        //                {
        //                    int index = sSSN.LastIndexOf('-');
        //                    sSSN = sSSN.Remove(index);
        //                }
        //                else
        //                {
        //                    break;
        //                }

        //            }
        //            query1.SetString(2, sSSN.Trim() + "%");
        //            query1.SetString(3, sActive + "%");
        //            query1.SetString(4, sSex + "%");
        //            query1.SetString(5, ulAccNo + "%");
        //            query1.SetString(6, sExtAccNo + "%");
        //            if (PolicyHolderId == string.Empty)
        //            {
        //                query1.SetString(7, MedicalRecordNo + "%");
        //                query1.SetString(8, HumanType + "%");
        //                PageNumber = PageNumber - 1;
        //                if (PageNumber != 0)
        //                {
        //                    query1.SetInt32(9, PageNumber * MaxResultSet);
        //                    query1.SetInt32(10, MaxResultSet);
        //                }
        //            }
        //            else
        //            {
        //                query1.SetString(7, PolicyHolderId + "%");
        //                query1.SetString(8, MedicalRecordNo + "%");
        //                query1.SetString(9, HumanType + "%");
        //                PageNumber = PageNumber - 1;
        //                if (PageNumber != 0)
        //                {
        //                    query1.SetInt32(10, PageNumber * MaxResultSet);
        //                    query1.SetInt32(11, MaxResultSet);
        //                }
        //            }

        //        }

        //        if (sLastName == string.Empty && sFirstName != string.Empty)
        //        {
        //            query1.SetString(0, sFirstName);
        //            if (dtDOB == DateTime.MinValue)
        //            //if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
        //            {
        //                query1.SetString(1, "%");
        //            }
        //            else
        //            {
        //                query1.SetString(1, dtDOB.ToString("yyyy-MM-dd") + "%");
        //            }
        //            // Added by Manimozhi - On 29th june 2012 - bug id 10239 
        //            for (int i = sSSN.Length - 1; i > 0; i--)
        //            {
        //                if (sSSN.Trim().EndsWith("-") == true)
        //                {
        //                    int index = sSSN.LastIndexOf('-');
        //                    sSSN = sSSN.Remove(index);
        //                }
        //                else
        //                {
        //                    break;
        //                }

        //            }
        //            query1.SetString(2, sSSN.Trim() + "%");
        //            query1.SetString(3, sActive + "%");
        //            query1.SetString(4, sSex + "%");
        //            query1.SetString(5, ulAccNo + "%");
        //            query1.SetString(6, sExtAccNo + "%");
        //            if (PolicyHolderId == string.Empty)
        //            {
        //                query1.SetString(7, MedicalRecordNo + "%");
        //                query1.SetString(8, HumanType + "%");
        //                PageNumber = PageNumber - 1;
        //                if (PageNumber != 0)
        //                {
        //                    query1.SetInt32(9, PageNumber * MaxResultSet);
        //                    query1.SetInt32(10, MaxResultSet);
        //                }
        //            }
        //            else
        //            {
        //                query1.SetString(7, PolicyHolderId + "%");
        //                query1.SetString(8, MedicalRecordNo + "%");
        //                query1.SetString(9, HumanType + "%");
        //                PageNumber = PageNumber - 1;
        //                if (PageNumber != 0)
        //                {
        //                    query1.SetInt32(10, PageNumber * MaxResultSet);
        //                    query1.SetInt32(11, MaxResultSet);
        //                }
        //            }

        //        }
        //        if (sLastName == string.Empty && sFirstName == string.Empty)
        //        {
        //            if (dtDOB == DateTime.MinValue)
        //            //if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
        //            {
        //                query1.SetString(0, "%");
        //            }
        //            else
        //            {
        //                query1.SetString(0, dtDOB.ToString("yyyy-MM-dd") + "%");
        //            }
        //            // Added by Manimozhi - On 29th june 2012 - bug id 10239 
        //            for (int i = sSSN.Length - 1; i > 0; i--)
        //            {
        //                if (sSSN.Trim().EndsWith("-") == true)
        //                {
        //                    int index = sSSN.LastIndexOf('-');
        //                    sSSN = sSSN.Remove(index);
        //                }
        //                else
        //                {
        //                    break;
        //                }

        //            }
        //            query1.SetString(1, sSSN.Trim() + "%");
        //            query1.SetString(2, sActive + "%");
        //            query1.SetString(3, sSex + "%");
        //            query1.SetString(4, ulAccNo + "%");
        //            query1.SetString(5, sExtAccNo + "%");
        //            if (PolicyHolderId == string.Empty)
        //            {
        //                query1.SetString(6, MedicalRecordNo + "%");
        //                query1.SetString(7, HumanType + "%");
        //                PageNumber = PageNumber - 1;
        //                if (PageNumber != 0)
        //                {
        //                    query1.SetInt32(8, PageNumber * MaxResultSet);
        //                    query1.SetInt32(9, MaxResultSet);
        //                }
        //            }
        //            else
        //            {
        //                query1.SetString(6, PolicyHolderId + "%");
        //                query1.SetString(7, MedicalRecordNo + "%");
        //                query1.SetString(8, HumanType + "%");
        //                PageNumber = PageNumber - 1;
        //                if (PageNumber != 0)
        //                {
        //                    query1.SetInt32(9, PageNumber * MaxResultSet);
        //                    query1.SetInt32(10, MaxResultSet);
        //                }
        //            }

        //        }

        //        ary = new ArrayList(query1.List());
        //        //Jayachandran 02-08-2010

        //        int result = 0;
        //        if (PageNumber == 0)
        //        {
        //            if (ary.Count > MaxResultSet)
        //            {
        //                result = MaxResultSet;
        //            }
        //            else
        //            {
        //                result = ary.Count;
        //            }

        //        }
        //        else
        //        {
        //            result = ary.Count;
        //        }

        //        DataSet ds = new DataSet();
        //        DataRow dr = null;
        //        DataTable dt = new DataTable();



        //        if (result != 0)
        //        {
        //            dt.Columns.Add("Account #", typeof(string));
        //            dt.Columns.Add("Last Name", typeof(string));
        //            dt.Columns.Add("First Name", typeof(string));
        //            dt.Columns.Add("Middle Name", typeof(string));
        //            dt.Columns.Add("Date Of Birth", typeof(string));
        //            dt.Columns.Add("SSN", typeof(string));
        //            dt.Columns.Add("Status", typeof(string));
        //            dt.Columns.Add("Medical Record #", typeof(string));
        //            dt.Columns.Add("External Account #", typeof(string));
        //            dt.Columns.Add("Subscriber ID", typeof(string));
        //            dt.Columns.Add("Is People Collection", typeof(string));
        //            dt.Columns.Add("PCP", typeof(string));
        //            dt.Columns.Add("Patient Status", typeof(string));
        //            dt.Columns.Add("Patient Type", typeof(string));
        //            dt.Columns.Add("Gender", typeof(string));
        //            dt.Columns.Add("ACO_Eligible", typeof(string));

        //            for (int i = 0; i < result; i++)
        //            {
        //                object[] obj = (object[])ary[i];
        //                if (obj != null)
        //                {
        //                    dr = dt.NewRow();

        //                    dr["Account #"] = Convert.ToUInt64(obj[0]);
        //                    dr["Last Name"] = obj[2].ToString();
        //                    dr["First Name"] = obj[3].ToString();
        //                    dr["Middle Name"] = obj[4].ToString();
        //                    dr["Date Of Birth"] = Convert.ToDateTime(obj[6]).ToString("dd-MMM-yyyy");
        //                    dr["SSN"] = obj[7].ToString();
        //                    dr["Status"] = obj[8].ToString();
        //                    dr["Medical Record #"] = obj[9].ToString();
        //                    dr["External Account #"] = obj[10].ToString();

        //                    IList<PatientInsuredPlan> ilstHuman = new List<PatientInsuredPlan>();
        //                    ICriteria criteria = iMySession.CreateCriteria(typeof(PatientInsuredPlan)).Add(Expression.Eq("Human_ID", Convert.ToUInt64(obj[0]))).Add(Expression.Eq("Insurance_Type", "PRIMARY"));
        //                    ilstHuman = criteria.List<PatientInsuredPlan>();
        //                    if (ilstHuman.Count > 0)
        //                    {
        //                        dr["Subscriber ID"] = ilstHuman[0].Policy_Holder_ID;
        //                    }

        //                    dr["Is People Collection"] = obj[11].ToString();
        //                    dr["PCP"] = obj[13].ToString();
        //                    dr["Patient Status"] = obj[14].ToString();
        //                    dr["Patient Type"] = obj[15].ToString();
        //                    dr["Gender"] = obj[16].ToString();
        //                    dr["ACO_Eligible"] = obj[17].ToString();
        //                    dt.Rows.Add(dr);
        //                }
        //            }
        //        }
        //        ds.Tables.Add(dt);

        //        Hashtable hsResult = new Hashtable();
        //        hsResult.Add("PatientList", dt);

        //        if (PageNumber == 0)
        //        {
        //            hsResult.Add("TotalCount", ary.Count);
        //        }

        //        serializer.WriteObject(stream, hsResult);
        //        stream.Seek(0L, SeekOrigin.Begin);
        //        iMySession.Close();
        //    }
        //    return stream;

        //}
        //public Human UpdateToHumanWithoutMail(Human objHuman, string MACAddress)
        //{
        //    IList<Human> humanList = new List<Human>();
        //    IList<Human> humanListadd = null;
        //    objHuman.Is_Sent_To_Rcopia = "Y";
        //    humanList.Add(objHuman);
        //    SaveUpdateDeleteWithTransaction(ref humanListadd, humanList, null, MACAddress);
        //    Human objhuman;
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", objHuman.Id));
        //        objhuman = crit.List<Human>()[0];
        //        //Latha - Branch_56 - Start - 23 Jul 2011
        //        //if (objhuman != null && objhuman.EMail.Trim() != string.Empty && objhuman.Is_Mail_Sent.ToUpper() == "Y")
        //        //{
        //        //    SendingMail(objhuman);
        //        //}
        //        //Latha - Branch_56 - End - 23 Jul 2011
        //        if (objhuman != null && objhuman.Id != 0)
        //        {
        //            RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
        //            objRcopiaMngr.SendPatientToRCopia(objhuman.Id, MACAddress);
        //        }
        //        iMySession.Close();
        //    }
        //    return objhuman;
        //}
        //public int BatchOperationsToHuman(IList<Human> savelist, IList<Human> updtList, IList<Human> delList, ISession MySession, string MACAddress)
        //{
        //    return SaveUpdateDeleteWithoutTransaction(ref savelist, updtList, delList, MySession, MACAddress);
        //}


        //Added by velmurugan for generatepatientlists 
        //public IList<Human> GetHumanDetailsForDemoReport(string BatchCurrentCompletedID, ulong wfObjectID, string processName)
        //{

        //    IList<Human> HumanList = new List<Human>();
        //    string CompletedLIst = string.Empty;
        //    IList<ulong> FinalCompList = new List<ulong>();
        //    IList<string> DBHumanIDList = new List<string>();
        //    IList<string> CurrentCompList = new List<string>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria criteriaCompletedList = iMySession.CreateCriteria(typeof(ObjectProcessHistoryBilling)).Add(Expression.Eq("Wf_Object_ID", wfObjectID)).Add(Expression.Eq("Process_Name", processName));
        //        IList<ObjectProcessHistoryBilling> DBcompletedList = criteriaCompletedList.List<ObjectProcessHistoryBilling>();

        //        for (int i = 0; i < DBcompletedList.Count; i++)
        //        {
        //            if (DBcompletedList[i].Completed_List != string.Empty)
        //            {
        //                CompletedLIst = CompletedLIst + "," + DBcompletedList[i].Completed_List;
        //            }
        //        }
        //        string[] HumanID = CompletedLIst.Split(',');

        //        for (int j = 0; j < HumanID.Length; j++)
        //        {
        //            if (HumanID[j] != string.Empty)
        //            {
        //                DBHumanIDList.Add(HumanID[j]);
        //            }
        //        }

        //        if (BatchCurrentCompletedID != string.Empty)
        //        {
        //            string[] CurrentCompID = BatchCurrentCompletedID.Split(',');
        //            for (int k = 0; k < CurrentCompID.Length; k++)
        //            {
        //                CurrentCompList.Add(CurrentCompID[k]);
        //            }

        //            for (int l = 0; l < CurrentCompList.Count; l++)
        //            {
        //                if (!DBHumanIDList.Contains(CurrentCompList[l]))
        //                {
        //                    DBHumanIDList.Add(CurrentCompList[l]);
        //                }
        //            }
        //        }
        //        for (int m = 0; m < DBHumanIDList.Count; m++)
        //        {
        //            ICriteria criteriaHumanList = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", Convert.ToUInt64(DBHumanIDList[m])));
        //            IList<Human> tempList = criteriaHumanList.List<Human>();
        //            if (tempList != null && tempList.Count > 0)
        //            {
        //                HumanList.Add(tempList[0]);
        //            }

        //        }

        //        iMySession.Close();
        //    }
        //    return HumanList;

        //}
        //public IList<BatchDTO> GetHumanListForCPManualReport(ulong ulWFObjectID, string sDOOS, string sBatchName)
        //{
        //    IList<BatchDTO> BatchDetailDTOList = new List<BatchDTO>();
        //    IList<ulong> HumanList = new List<ulong>();
        //    IList<Human> FinalHumanList = new List<Human>();
        //    IList<ObjectProcessHistoryBilling> ObjProcHistBillList = new List<ObjectProcessHistoryBilling>();
        //    ObjectProcessHistoryBilling ObjProcHistBillRecord = new ObjectProcessHistoryBilling();
        //    ArrayList arylstAlreadyCompleted = new ArrayList();
        //    ArrayList MyList = new ArrayList();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = iMySession.GetNamedQuery("Get.Object.Process.History.Billing.Details.CP");
        //        query1.SetString(0, ulWFObjectID.ToString());
        //        query1.SetString(1, sDOOS);
        //        query1.SetString(2, sBatchName);

        //        MyList.AddRange(query1.List());

        //        if (MyList != null)
        //        {
        //            for (int i = 0; i < MyList.Count; i++)
        //            {
        //                object[] oj = (object[])MyList[i];

        //                ObjProcHistBillRecord = new ObjectProcessHistoryBilling();
        //                ObjProcHistBillRecord.Id = Convert.ToUInt32(oj[0]);
        //                ObjProcHistBillRecord.Wf_Object_ID = Convert.ToUInt32(oj[1]);
        //                ObjProcHistBillRecord.Process_Name = oj[2].ToString();
        //                ObjProcHistBillRecord.Process_Start_Date_And_Time = Convert.ToDateTime(oj[3]);
        //                ObjProcHistBillRecord.Process_End_Date_And_Time = Convert.ToDateTime(oj[4]);
        //                ObjProcHistBillRecord.Obj_Type = oj[5].ToString();
        //                ObjProcHistBillRecord.Obj_Sub_Type = oj[6].ToString();
        //                ObjProcHistBillRecord.Comments = oj[7].ToString();
        //                ObjProcHistBillRecord.User_Name = oj[8].ToString();
        //                ObjProcHistBillRecord.DOOS = oj[9].ToString();
        //                ObjProcHistBillRecord.Batch_Name = oj[10].ToString();
        //                ObjProcHistBillRecord.Doc_Name = oj[11].ToString();
        //                ObjProcHistBillRecord.Demos_Enc_PPLine_Comp = Convert.ToUInt32(oj[12]);
        //                ObjProcHistBillRecord.Wf_Obj_Amount = Convert.ToDecimal(oj[13]);
        //                ObjProcHistBillRecord.Sub_Batch_Range = oj[14].ToString();
        //                ObjProcHistBillRecord.Doc_Type = oj[15].ToString();
        //                ObjProcHistBillRecord.Doc_Sub_Type = oj[16].ToString();
        //                ObjProcHistBillRecord.Version = Convert.ToInt32(oj[17]);
        //                ObjProcHistBillRecord.Completed_List = oj[18].ToString();
        //                ObjProcHistBillRecord.Duplicate_Demos_Enc_PPLine = Convert.ToInt32(oj[19]);
        //                ObjProcHistBillRecord.Duplicate_Amount = Convert.ToDecimal(oj[20]);
        //                ObjProcHistBillRecord.Batch_Name_in_Billing = oj[21].ToString();
        //                ObjProcHistBillRecord.Print_Name_in_Billing = oj[22].ToString();
        //                ObjProcHistBillRecord.Report_Verified = oj[23].ToString();

        //                ObjProcHistBillList.Add(ObjProcHistBillRecord);
        //            }
        //        }

        //        for (int i = 0; i < ObjProcHistBillList.Count; i++)
        //        {
        //            ObjProcHistBillRecord = new ObjectProcessHistoryBilling();
        //            ObjProcHistBillRecord = ObjProcHistBillList[i];

        //            if (ObjProcHistBillRecord.Completed_List != "")
        //            {
        //                string[] sSplittedAlreadyCompletedList = null;
        //                sSplittedAlreadyCompletedList = ObjProcHistBillRecord.Completed_List.Split(',');
        //                for (int j = 0; j < sSplittedAlreadyCompletedList.Length; j++)
        //                {
        //                    string[] sSplittedAlreadyCompletedHumanList = null;
        //                    sSplittedAlreadyCompletedHumanList = sSplittedAlreadyCompletedList[j].Split('_');

        //                    if (sSplittedAlreadyCompletedHumanList.Length > 0)
        //                    {
        //                        if (sSplittedAlreadyCompletedHumanList[0] != "" && Convert.ToUInt32(sSplittedAlreadyCompletedHumanList[0]) > 0)
        //                        {
        //                            if (HumanList.Contains(Convert.ToUInt32(sSplittedAlreadyCompletedHumanList[0])) == false && sSplittedAlreadyCompletedHumanList[0] != "")
        //                            {
        //                                HumanList.Add(Convert.ToUInt32(sSplittedAlreadyCompletedHumanList[0])); //Unique Already Completed count
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        IList<ObjectProcessHistoryBillingTemp> ObjProcHistBillTempList = new List<ObjectProcessHistoryBillingTemp>();
        //        ObjectProcessHistoryBillingTemp ObjProcHistBillTempRecord = new ObjectProcessHistoryBillingTemp();
        //        ArrayList arylstAlreadyTempCompleted = new ArrayList();

        //        ArrayList MyList1 = new ArrayList();

        //        IQuery query2 = iMySession.GetNamedQuery("Get.Object.Process.History.Billing.Temp.Details.CP");
        //        query2.SetString(0, ulWFObjectID.ToString());
        //        query2.SetString(1, sDOOS);
        //        query2.SetString(2, sBatchName);

        //        MyList1.AddRange(query2.List());

        //        if (MyList1 != null)
        //        {
        //            for (int i = 0; i < MyList1.Count; i++)
        //            {
        //                object[] oj = (object[])MyList1[i];

        //                ObjProcHistBillTempRecord = new ObjectProcessHistoryBillingTemp();
        //                ObjProcHistBillTempRecord.Id = Convert.ToUInt32(oj[0]);
        //                ObjProcHistBillTempRecord.Wf_Object_ID = Convert.ToUInt32(oj[1]);
        //                ObjProcHistBillTempRecord.Process_Name = oj[2].ToString();
        //                ObjProcHistBillTempRecord.Process_Start_Date_And_Time = Convert.ToDateTime(oj[3]);
        //                ObjProcHistBillTempRecord.Process_End_Date_And_Time = Convert.ToDateTime(oj[4]);
        //                ObjProcHistBillTempRecord.Obj_Type = oj[5].ToString();
        //                ObjProcHistBillTempRecord.Obj_Sub_Type = oj[6].ToString();
        //                ObjProcHistBillTempRecord.Comments = oj[7].ToString();
        //                ObjProcHistBillTempRecord.User_Name = oj[8].ToString();
        //                ObjProcHistBillTempRecord.DOOS = oj[9].ToString();
        //                ObjProcHistBillTempRecord.Batch_Name = oj[10].ToString();
        //                ObjProcHistBillTempRecord.Doc_Name = oj[11].ToString();
        //                ObjProcHistBillTempRecord.Demos_Enc_PPLine_Comp = Convert.ToUInt32(oj[12]);
        //                ObjProcHistBillTempRecord.Wf_Obj_Amount = Convert.ToDecimal(oj[13]);
        //                ObjProcHistBillTempRecord.Sub_Batch_Range = oj[14].ToString();
        //                ObjProcHistBillTempRecord.Doc_Type = oj[15].ToString();
        //                ObjProcHistBillTempRecord.Doc_Sub_Type = oj[16].ToString();
        //                ObjProcHistBillTempRecord.Version = Convert.ToInt32(oj[17]);
        //                ObjProcHistBillTempRecord.Completed_List = oj[18].ToString();
        //                ObjProcHistBillTempRecord.Duplicate_Demos_Enc_PPLine = Convert.ToInt32(oj[19]);
        //                ObjProcHistBillTempRecord.Duplicate_Amount = Convert.ToDecimal(oj[20]);
        //                ObjProcHistBillTempRecord.Batch_Name_in_Billing = oj[21].ToString();
        //                ObjProcHistBillTempRecord.Print_Name_in_Billing = oj[22].ToString();
        //                ObjProcHistBillTempRecord.Report_Verified = oj[23].ToString();
        //                ObjProcHistBillTempRecord.Is_Marked_in_History_Table = oj[24].ToString();

        //                ObjProcHistBillTempList.Add(ObjProcHistBillTempRecord);
        //            }
        //        }

        //        for (int i = 0; i < ObjProcHistBillTempList.Count; i++)
        //        {
        //            ObjProcHistBillTempRecord = new ObjectProcessHistoryBillingTemp();
        //            ObjProcHistBillTempRecord = ObjProcHistBillTempList[i];

        //            if (ObjProcHistBillTempRecord.Completed_List != "")
        //            {
        //                string[] sSplittedAlreadyCompletedList = null;
        //                sSplittedAlreadyCompletedList = ObjProcHistBillTempRecord.Completed_List.Split(',');
        //                for (int j = 0; j < sSplittedAlreadyCompletedList.Length; j++)
        //                {
        //                    string[] sSplittedAlreadyCompletedHumanList = null;
        //                    sSplittedAlreadyCompletedHumanList = sSplittedAlreadyCompletedList[j].Split('_');

        //                    if (sSplittedAlreadyCompletedHumanList.Length > 0)
        //                    {
        //                        if (sSplittedAlreadyCompletedHumanList[0] != "" && Convert.ToUInt32(sSplittedAlreadyCompletedHumanList[0]) > 0)
        //                        {
        //                            if (HumanList.Contains(Convert.ToUInt32(sSplittedAlreadyCompletedHumanList[0])) == false && sSplittedAlreadyCompletedHumanList[0] != "")
        //                            {
        //                                HumanList.Add(Convert.ToUInt32(sSplittedAlreadyCompletedHumanList[0])); //Unique Already Completed count
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //==========================

        //        BatchDTO BatchDetailDTO = new BatchDTO();
        //        ChargeLineItem ChargeLineItemRecord = new ChargeLineItem();
        //        Human HumanRecord = new Human();
        //        IList<PatientInsuredPlan> PatInsPlanList = new List<PatientInsuredPlan>();
        //        IList<InsurancePlan> InsPlanList = new List<InsurancePlan>();
        //        PatientInsuredPlanManager PatInsPlanMngr = new PatientInsuredPlanManager();
        //        InsurancePlanManager InsPlanMngr = new InsurancePlanManager();

        //        for (int m = 0; m < HumanList.Count; m++)
        //        {
        //            ISQLQuery sql = iMySession.CreateSQLQuery("SELECT a.*,b.*,h.* FROM charge_line_item a,charge_header b,human h WHERE b.human_id = " + HumanList[m].ToString() + " and a.charge_header_id=b.charge_header_id and b.human_id=h.human_id and a.Deleted<>'Y' ORDER BY a.Sort_Order")
        //                                .AddEntity("a", typeof(ChargeLineItem)).AddEntity("b", typeof(ChargeHeader)).AddEntity("h", typeof(Human));
        //            //ChargeLineItemList = sql.List<ChargeLineItem>();

        //            foreach (IList<object> l in sql.List())
        //            {
        //                ChargeLineItemRecord = (ChargeLineItem)l[0];
        //                HumanRecord = (Human)l[2];

        //                if (HumanRecord != null)
        //                {
        //                    BatchDetailDTO.Human_id = HumanRecord.Id;
        //                    BatchDetailDTO.patientname = HumanRecord.Last_Name + "," + HumanRecord.First_Name;
        //                    BatchDetailDTO.DOB = HumanRecord.Birth_Date;
        //                    BatchDetailDTO.Gender = HumanRecord.Sex;
        //                    BatchDetailDTO.MRN_No = HumanRecord.Medical_Record_Number;

        //                    if (ChargeLineItemRecord != null)
        //                    {
        //                        IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
        //                        PhysicianManager PhyMngr = new PhysicianManager();

        //                        PhyList = PhyMngr.GetphysiciannameByPhyID(ChargeLineItemRecord.Rendering_Provider_ID);
        //                        if (PhyList.Count > 0)
        //                        {
        //                            BatchDetailDTO.providername = PhyList[0].PhyLastName + "," + PhyList[0].PhyFirstName + " " + PhyList[0].PhyMiddleName + " " + PhyList[0].PhySuffix;
        //                        }

        //                        BatchDetailDTO.DOS = ChargeLineItemRecord.From_DOS.ToString("yyyy-MM-dd");
        //                        BatchDetailDTO.Procedure_code = ChargeLineItemRecord.Procedure_Code;
        //                        BatchDetailDTO.Units = ChargeLineItemRecord.Units;
        //                        PatInsPlanList = PatInsPlanMngr.getInsuranceDetailsByPatInsuredId(ChargeLineItemRecord.Pat_Insured_Plan_ID);

        //                        if (PatInsPlanList.Count > 0)
        //                        {
        //                            InsPlanList = InsPlanMngr.GetInsurancebyID(PatInsPlanList[0].Insurance_Plan_ID);// ChargeLineItemList[0].Primary_Payer_ID);

        //                            if (InsPlanList.Count > 0)
        //                            {
        //                                BatchDetailDTO.Pri_Plan_Name = InsPlanList[0].Ins_Plan_Name;// ChargeLineItemRecord.Primary_Payer_Name;
        //                            }
        //                        }

        //                        BatchDetailDTO.Charge_Amount_Line_Item = ChargeLineItemRecord.Charge_Amount;

        //                    }

        //                    BatchDetailDTOList.Add(BatchDetailDTO);
        //                }
        //            }
        //        }
        //        iMySession.Close();
        //    }
        //    return BatchDetailDTOList;

        //}
        //  public void UpdateACOQueries(ulong HumanId, string IsPatientAccepted, string IsPatientDiscussed, ulong IsDiscussedBy)
        //{
        //    Human objHuman = new Human();
        //    objHuman = GetById(HumanId);
        //    if (objHuman != null)
        //    {
        //        objHuman.Patient_Accepted = IsPatientAccepted.ToString();
        //        objHuman.Patient_Discussed = IsPatientDiscussed.ToString();
        //        objHuman.Discussed_By = IsDiscussedBy;
        //        IList<Human> saveList = new List<Human>();
        //        IList<Human> updateList = new List<Human>();
        //        updateList.Add(objHuman);

        //        SaveUpdateDeleteWithTransaction(ref saveList, updateList, null, string.Empty);
        //    }
        //}
        //public string GetHumanSex(ulong HumanId)
        //{
        //    IList<Human> list_image = new List<Human>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanId));
        //        list_image = crit.List<Human>();
        //        iMySession.Close();
        //    }
        //    return list_image[0].Sex;
        //}

        //Added for ACO by srividhya on 9-Jul-2014
        //public IList<Human> GetHumanListForEhrMeasure(string HumanId)
        //{
        //    string[] human = HumanId.Split(',');
        //    IList<Human> hum = new List<Human>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.In("Id", human));
        //        hum = crit.List<Human>();
        //        iMySession.Close();
        //    }
        //    return hum;
        //}
        //public FillQuickPatient GetEmaillink(string[] Field_Name)
        //{
        //    FillQuickPatient FillQPDTO = new FillQuickPatient();
        //    IList<StaticLookup> staticList = new List<StaticLookup>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.In("Field_Name", Field_Name));
        //        FillQPDTO.StaticLookupList = criteria.List<StaticLookup>();

        //        staticList = criteria.List<StaticLookup>();
        //        iMySession.Close();
        //    }
        //    return FillQPDTO;
        //}
        #endregion

        #region Used Methods
        public IList<Human> GetPatientDetailsUsingPatientInformattion(ulong ulHumanId)
        {
            IList<Human> lstHuman = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", ulHumanId));
                lstHuman = criteria.List<Human>();
                iMySession.Close();
            }
            return lstHuman;
        }

        public IList<Human> GetHumanbyAppointmentStatus(string[] Appointment_Status)
        {
            IList<Human> lstHuman = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Human)).Add(Expression.In("Appointment_Status", Appointment_Status));
                lstHuman = criteria.List<Human>();
                iMySession.Close();
            }
            return lstHuman;
        }

        public IList<Human> GetHumanbyAppointmentStatusshowall()
        {
            IList<Human> lstHuman = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("SELECT  h.* from human as h  where Appointment_Status!='' and  Appointment_Status!='WAITING FOR CALL' and modified_date_and_time>' " + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd") + "'").AddEntity("h", typeof(Human));

                // ICriteria criteria = iMySession.CreateCriteria(typeof(Human)).Add(Expression.IsNotEmpty("Appointment_Status")).Add(Expression.Lt("Modified_Date_and_Time", DateTime.Now.AddDays(-15)).; ;
                lstHuman = sql.List<Human>();
                iMySession.Close();
            }
            return lstHuman;
        }
        public HumanDTO GetPatientDetailsUsingPatientDetails(string sLastname, string sFirstname, DateTime dtBirthDate, string sSex, string sMedicalRecordNo, string sExternalAccNo, string sLegalOrg)
        {
            HumanDTO CheckhumanDTO = new HumanDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (sLastname != string.Empty)
                {
                    ICriteria criteria = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Last_Name", sLastname)).Add(Expression.Eq("First_Name", sFirstname)).Add(Expression.Eq("Birth_Date", dtBirthDate.Date)).Add(Expression.Eq("Sex", sSex)).Add(Expression.Eq("Legal_Org", sLegalOrg)); 
                    if (criteria.List<Human>().Count > 0)
                    {
                        CheckhumanDTO.HumanDetails = criteria.List<Human>()[0];
                    }
                    else
                    {
                        CheckhumanDTO.HumanDetails = null;
                    }
                }

                if (sMedicalRecordNo != string.Empty)
                {
                    ICriteria criteria1 = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Medical_Record_Number", sMedicalRecordNo));
                    if (criteria1.List<Human>().Count > 0)
                    {
                        CheckhumanDTO.MedicalRecordNoList = true;
                        CheckhumanDTO.ulMedicalRecordID = criteria1.List<Human>()[0].Id;
                    }
                    else
                    {
                        CheckhumanDTO.MedicalRecordNoList = false;
                    }
                }

                if (sExternalAccNo != string.Empty)
                {
                    ICriteria criteria1 = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Patient_Account_External", sExternalAccNo));
                    if (criteria1.List<Human>().Count > 0)
                    {
                        CheckhumanDTO.Patient_Account_External = true;
                        CheckhumanDTO.ulPatientAccountExternalID = criteria1.List<Human>()[0].Id;
                    }
                    else
                    {
                        CheckhumanDTO.Patient_Account_External = false;
                    }
                }
                iMySession.Close();
            }
            return CheckhumanDTO;
        }

        int iTryCount = 0;

        public FillQuickPatient LoadQuickPatient(ulong EncounterId)
        {
            FillQuickPatient FillQuickPatientObj = new FillQuickPatient();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ISQLQuery query1 = iMySession.CreateSQLQuery("select u.* FROM encounter u where u.encounter_id in (:Id)").AddEntity("u", typeof(Encounter));
                query1.SetParameter("Id", EncounterId);


                ICriteria criteria;

                //  ICriteria criteria = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", EncounterId));
                if (query1.List<Encounter>() != null && query1.List<Encounter>().Count > 0)
                {
                    FillQuickPatientObj.EncounterObj = query1.List<Encounter>()[0];
                }

                if (FillQuickPatientObj.EncounterObj != null)
                {
                    //criteria = iMySession.CreateCriteria(typeof(VisitPayment)).Add(Expression.Eq("Encounter_ID", EncounterId))
                    //    .AddOrder(Order.Desc("Created_Date_And_Time"));
                    //FillQuickPatientObj.PaymentList = criteria.List<VisitPayment>();

                    IList<object> ilistObj = iMySession.CreateSQLQuery("select v.method_of_payment,v.check_card_no,v.auth_no,v.patient_payment,v.refund_amount,v.rec_on_acc,cast(v.check_date as char(50)),v.payment_note,cast(v.visit_payment_id as char(50)),cast(p.pp_line_item_id as char(50)),cast(p.pp_header_id as char(50)),cast(p.check_table_int_id as char(50)),cast(v.created_date_and_time as char(50)) from visit_payment v left join pp_line_item p on (v.visit_payment_id=p.visit_payment_id) where v.encounter_id=" + EncounterId + " and v.is_delete='N' order by v.created_date_and_time").List<object>();
                    if (ilistObj != null)
                    {
                        foreach (IList<object> obj in ilistObj)
                        {
                            VisitPaymentDTO objVisitPaymentDTO = new VisitPaymentDTO();
                            objVisitPaymentDTO.Method_of_Payment = obj[0].ToString();
                            objVisitPaymentDTO.Check_Card_No = obj[1].ToString();
                            objVisitPaymentDTO.Auth_No = obj[2].ToString();
                            if (obj[3].ToString() != "")
                                objVisitPaymentDTO.Patient_Payment = Convert.ToDecimal(obj[3]);
                            if (obj[4].ToString() != "")
                                objVisitPaymentDTO.Refund_Amount = Convert.ToDecimal(obj[4]);
                            if (obj[5].ToString() != "")
                                objVisitPaymentDTO.Rec_On_Acc = Convert.ToDecimal(obj[5]);
                            if (obj[6].ToString() != "")
                                objVisitPaymentDTO.Check_Date = Convert.ToDateTime(obj[6]);
                            objVisitPaymentDTO.Payment_Note = obj[7].ToString();
                            objVisitPaymentDTO.Visit_Payment_Id = Convert.ToUInt32(obj[8]);
                            if (obj[9] != null)
                                objVisitPaymentDTO.PP_Line_Item_Id = Convert.ToUInt32(obj[9]);
                            if (obj[10] != null)
                                objVisitPaymentDTO.PP_Header_Id = Convert.ToUInt32(obj[10]);
                            if (obj[11] != null)
                                objVisitPaymentDTO.Check_Table_Int_Id = Convert.ToUInt32(obj[11]);
                            if (obj[12] != null)
                                objVisitPaymentDTO.Created_Date_and_Time = Convert.ToDateTime(obj[12]);
                            FillQuickPatientObj.VisitPaymentDTO.Add(objVisitPaymentDTO);
                        }
                    }

                    criteria = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", FillQuickPatientObj.EncounterObj.Human_ID));
                    if (criteria.List<Human>() != null && criteria.List<Human>().Count > 0)
                    {
                        FillQuickPatientObj.HumanObj = criteria.List<Human>()[0];
                    }
                    //criteria = iMySession.CreateCriteria(typeof(Eligibility_Verification)).Add(Expression.Eq("Human_ID", FillQuickPatientObj.EncounterObj.Human_ID));
                    //FillQuickPatientObj.EligibilityList = criteria.List<Eligibility_Verification>();
                    ISQLQuery sql = iMySession.CreateSQLQuery("select e.* from eligibility_verification e where e.human_id='" + FillQuickPatientObj.EncounterObj.Human_ID + "' ORDER BY CAST(if(e.Modified_Date_And_Time<>'0001-01-01 00:00:00',e.Modified_Date_And_Time,e.Created_date_and_time) AS CHAR(100)) DESC;").AddEntity("e", typeof(Eligibility_Verification));
                    FillQuickPatientObj.EligibilityList = sql.List<Eligibility_Verification>();

                    if (FillQuickPatientObj.HumanObj.PatientInsuredBag != null)
                    {
                        ulong[] insId = new ulong[FillQuickPatientObj.HumanObj.PatientInsuredBag.Count];
                        for (int i = 0; i < FillQuickPatientObj.HumanObj.PatientInsuredBag.Count; i++)
                        {
                            insId[i] = FillQuickPatientObj.HumanObj.PatientInsuredBag[i].Insurance_Plan_ID;
                        }
                    }
                    //criteria = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.In("Id", insId));
                    //FillQuickPatientObj.InsurancePlanList = criteria.List<InsurancePlan>();
                }
                iMySession.Close();
            }
            return FillQuickPatientObj;
        }

        public FillQuickPatient LoadQuickPatientforPatientPayment(ulong EncounterId, Boolean IsEncounterBased)
        {
            FillQuickPatient FillQuickPatientObj = new FillQuickPatient();
            ICriteria criteria;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (IsEncounterBased == true)
                {
                    ISQLQuery query1 = iMySession.CreateSQLQuery("select u.* FROM encounter u where u.encounter_id in (:Id)").AddEntity("u", typeof(Encounter));
                    query1.SetParameter("Id", EncounterId);

                    if (query1.List<Encounter>() != null && query1.List<Encounter>().Count > 0)
                    {
                        FillQuickPatientObj.EncounterObj = query1.List<Encounter>()[0];
                    }
                }

                if (FillQuickPatientObj.EncounterObj != null)
                {
                    string sQuery = string.Empty;
                    if (IsEncounterBased == true)
                    {
                        sQuery = "(select v.method_of_payment,v.check_card_no,v.auth_no,v.patient_payment,v.refund_amount,v.rec_on_acc,cast(v.check_date as char(50)),v.payment_note,cast(v.visit_payment_id as char(50)) as VisitID,cast(p.pp_line_item_id as char(50)),cast(p.pp_header_id as char(50)),cast(p.check_table_int_id as char(50)),if(v.modified_date_and_time = '0001-01-01 00:00:00', cast(v.created_date_and_time as char(50)),cast(v.modified_date_and_time as char(50))) as MyDate,v.voucher_no from visit_payment v left join pp_line_item p on (v.visit_payment_id=p.visit_payment_id) where v.encounter_id=" + EncounterId + " and v.is_delete='N') union all (select v.method_of_payment,v.check_card_no,v.auth_no,v.patient_payment,v.refund_amount,v.rec_on_acc,cast(v.check_date as char(50)),v.payment_note,cast(v.visit_payment_id as char(50)),cast(p.pp_line_item_id as char(50)),cast(p.pp_header_id as char(50)),cast(p.check_table_int_id as char(50)),cast(v.created_date_and_time as char(50)) as MyDate,v.voucher_no from visit_payment_arc v left join pp_line_item_arc p on (v.visit_payment_id=p.visit_payment_id) where v.encounter_id=" + EncounterId + " and v.is_delete='N') order by VisitID";
                    }
                    else
                    {
                        sQuery = "(select v.method_of_payment,v.check_card_no,v.auth_no,v.patient_payment,v.refund_amount,v.rec_on_acc,cast(v.check_date as char(50)),v.payment_note,cast(v.visit_payment_id as char(50)) as VisitID,cast(p.pp_line_item_id as char(50)),cast(p.pp_header_id as char(50)),cast(p.check_table_int_id as char(50)),if(v.modified_date_and_time = '0001-01-01 00:00:00', cast(v.created_date_and_time as char(50)),cast(v.modified_date_and_time as char(50))) as MyDate,v.voucher_no from visit_payment v left join pp_line_item p on (v.visit_payment_id=p.visit_payment_id) where v.voucher_no=" + EncounterId + " and v.is_delete='N') union all (select v.method_of_payment,v.check_card_no,v.auth_no,v.patient_payment,v.refund_amount,v.rec_on_acc,cast(v.check_date as char(50)),v.payment_note,cast(v.visit_payment_id as char(50)),cast(p.pp_line_item_id as char(50)),cast(p.pp_header_id as char(50)),cast(p.check_table_int_id as char(50)),cast(v.created_date_and_time as char(50)) as MyDate,v.voucher_no from visit_payment_arc v left join pp_line_item_arc p on (v.visit_payment_id=p.visit_payment_id) where v.voucher_no=" + EncounterId + " and v.is_delete='N') order by VisitID";
                    }

                    IList<object> ilistObj = iMySession.CreateSQLQuery(sQuery).List<object>();
                    if (ilistObj != null)
                    {
                        foreach (IList<object> obj in ilistObj)
                        {
                            VisitPaymentDTO objVisitPaymentDTO = new VisitPaymentDTO();
                            objVisitPaymentDTO.Method_of_Payment = obj[0].ToString();
                            objVisitPaymentDTO.Check_Card_No = obj[1].ToString();
                            objVisitPaymentDTO.Auth_No = obj[2].ToString();
                            if (obj[3].ToString() != "")
                                objVisitPaymentDTO.Patient_Payment = Convert.ToDecimal(obj[3]);
                            if (obj[4].ToString() != "")
                                objVisitPaymentDTO.Refund_Amount = Convert.ToDecimal(obj[4]);
                            if (obj[5].ToString() != "")
                                objVisitPaymentDTO.Rec_On_Acc = Convert.ToDecimal(obj[5]);
                            if (obj[6].ToString() != "")
                                objVisitPaymentDTO.Check_Date = Convert.ToDateTime(obj[6]);
                            objVisitPaymentDTO.Payment_Note = obj[7].ToString();
                            objVisitPaymentDTO.Visit_Payment_Id = Convert.ToUInt32(obj[8]);
                            if (obj[9] != null)
                                objVisitPaymentDTO.PP_Line_Item_Id = Convert.ToUInt32(obj[9]);
                            if (obj[10] != null)
                                objVisitPaymentDTO.PP_Header_Id = Convert.ToUInt32(obj[10]);
                            if (obj[11] != null)
                                objVisitPaymentDTO.Check_Table_Int_Id = Convert.ToUInt32(obj[11]);
                            if (obj[12] != null)
                                objVisitPaymentDTO.Created_Date_and_Time = Convert.ToDateTime(obj[12]);
                            if (obj[13] != null)
                                objVisitPaymentDTO.Voucher_No = Convert.ToUInt64(obj[13]);

                            FillQuickPatientObj.VisitPaymentDTO.Add(objVisitPaymentDTO);
                        }
                    }

                    criteria = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", FillQuickPatientObj.EncounterObj.Human_ID));
                    if (criteria.List<Human>() != null && criteria.List<Human>().Count > 0)
                    {
                        FillQuickPatientObj.HumanObj = criteria.List<Human>()[0];
                    }
                }
                iMySession.Close();
            }
            return FillQuickPatientObj;
        }

        public FillQuickPatient LoadQuickPatient(ulong EncounterId, ulong Human_id)
        {
            FillQuickPatient FillQuickPatientObj = new FillQuickPatient();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (Human_id != 0)
                {

                    ICriteria criteria = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", Human_id));
                    if (criteria.List<Human>() != null && criteria.List<Human>().Count > 0)
                    {
                        FillQuickPatientObj.HumanObj = criteria.List<Human>()[0];
                    }

                    ISQLQuery sql = iMySession.CreateSQLQuery("select e.* from eligibility_verification e where e.human_id='" + Human_id + "' ORDER BY CAST(if(e.Modified_Date_And_Time<>'0001-01-01 00:00:00',e.Modified_Date_And_Time,e.Created_date_and_time) AS CHAR(100)) DESC;").AddEntity("e", typeof(Eligibility_Verification));
                    FillQuickPatientObj.EligibilityList = sql.List<Eligibility_Verification>();
                }
                iMySession.Close();
            }

            return FillQuickPatientObj;
        }

        public Stream SearchGeneratePatientLists(string AgeCondition, int Age, int AgeFrom, int AgeTo, string Gender, string Race, string Ethnicity, string CommPreference, IList<string> MedicationList, IList<string> Medication_allergyList, IList<string> ProblemList, string[] sLabResultCondition, string[] sCodition, IList<string> FreqProblemList, string fromDate, string toDate, int pageNumber, int maxResults)
        {
            var stream = new MemoryStream();
            //stream.Capacity = int.MaxValue;
            var serializer = new NetDataContractSerializer();
            var foundObjects = new object();

            IList<GeneratePatientListsDTO> PatientList = new List<GeneratePatientListsDTO>();
            GeneratePatientListsDTO objGeneratePatientListsDTO = null;

            IList<GeneratePatientListsDTO> TestList = new List<GeneratePatientListsDTO>();
            // GeneratePatientListsDTO objGeneratePatientTestListsDTO = null;
            IList<GeneratePatientListsDTO> Test_List = new List<GeneratePatientListsDTO>();

            #region Human Query

            //commented by prabu on 18.05.2011 during merge
            //<<<<<<< HumanManager.cs
            //            string Query = "Select h.Human_Id,Concat(h.Last_Name,',',h.First_Name,',',h.MI) as Patient_Name,cast(h.Birth_Date as char(100)) as Birth_Date,h.Sex,group_concat(distinct(m.Generic_Name) separator ','),group_concat(distinct(concat(p.ICD,'-',p.Problem_Description)) separator ','),i.Procedure_Code,i.Immunization_Description from human h left join rcopia_medication m on (m.human_id = h.human_id) left join problem_list P on (p.human_id = h.human_id) left join immunization i on (i.human_id = h.human_id) left join result_master s on(s.PID_External_Patient_ID=h.human_id) left join result_obx o on (o.Result_Master_ID=s.Result_Master_ID)where ";

            //||||||| 1.47
            //            string Query = "Select h.Human_Id,Concat(h.Last_Name,',',h.First_Name,',',h.MI) as Patient_Name,cast(h.Birth_Date as char(100)) as Birth_Date,h.Sex,group_concat(distinct(m.Generic_Name) separator ','),group_concat(distinct(concat(p.ICD,'-',p.Problem_Description)) separator ','),i.Procedure_Code,i.Immunization_Description from human h inner join rcopia_medication m on (m.human_id = h.human_id) inner join problem_list P on (p.human_id = h.human_id) inner join immunization i on (i.human_id = h.human_id) inner join result_master s on(s.PID_External_Patient_ID=h.human_id) inner join result_obx o on (o.Result_Master_ID=s.Result_Master_ID)where ";
            //=======
            //&commented by prabu on 18.05.2011 during merge
            //Query changed on 18.05.11 by velmurugan
            //Selvamani - 09/02/2012 - Query modified
            string Query = "Select h.human_Id" +
                            ",if (h.mi='',Concat(h.Last_Name,',',h.First_Name),Concat(h.Last_Name,',',h.First_Name,',',h.MI)) as Patient_Name" +
                            ",cast(h.Birth_Date as char(100)) as Birth_Date,h.Sex";
            if (MedicationList.Count > 0)
            {
                Query += ",if(m.Generic_Name = '','', group_concat(distinct(m.Generic_Name) separator ',')),cast(m.Created_Date_And_Time as char(100)) as Med_date";
            }
            else
            {
                Query += ",null,null";
            }
            if (Medication_allergyList.Count > 0)
            {
                Query += ",if(a.Allergy_Name = '','', group_concat(distinct(a.Allergy_Name) separator ',')),cast(a.Created_Date_And_Time as char(100)) as Med_Allrgydate";
            }
            else
            {
                Query += ",null,null";
            }
            if (ProblemList.Count > 0)
            {
                Query += ",group_concat(distinct(concat(p.ICD,'-',p.Problem_Description)) separator ','),cast(p.Created_Date_And_Time as char(100)) as Prob_Date";
            }
            else
            {
                Query += ",null,null";
            }
            if (!string.IsNullOrEmpty(Race))
            {
                Query += ",h.Race ";
            }
            else
            {
                Query += ",null";
            }

            if (!string.IsNullOrEmpty(Ethnicity))
            {
                Query += ",h.Ethnicity ";
            }
            else
            {
                Query += ",null";
            }

            if (!string.IsNullOrEmpty(CommPreference))
            {
                Query += ",h.Preferred_Confidential_Correspodence_Mode";
            }
            else
            {
                Query += ",null";
            }



            Query += ",cast(e.Date_of_Service as char(100)) as Enc_Date  from encounter e " +
                            "inner join human h on (h.human_id= e.human_id)";

            if (MedicationList.Count > 0)
            {
                Query += "inner join rcopia_medication m on (m.human_id = h.human_id)";
            }

            if (Medication_allergyList.Count > 0)
            {
                Query += "inner join rcopia_allergy a on (a.human_id = h.human_id)";
            }

            if (ProblemList.Count > 0)
            {
                Query += "inner join problem_list P on (p.human_id = h.human_id) ";
            }
            Query += "where ";

            //if (Age > 0 || AgeCondition != string.Empty || AgeFrom > 0 || Gender != string.Empty)
            //{
            //    // Query += "h.human_id in (Select h.human_Id from human h where ";

            //    if (!string.IsNullOrEmpty(AgeCondition))
            //    {
            //        switch (AgeCondition)
            //        {
            //            case "<":
            //                AgeCondition = ">";
            //                //if (Age != 1)
            //                //    Age -= 1; Commented for Bug Id:29093
            //                break;
            //            case ">":
            //                AgeCondition = "<";
            //                //Age += 1;Commented for Bug Id:29093
            //                break;
            //            case "<=":
            //                AgeCondition = ">=";
            //                break;
            //            case ">=":
            //                AgeCondition = "<=";
            //                break;
            //            default:
            //                AgeCondition = "=";
            //                break;
            //        }
            //    }
            //}Commented and altered the Query for Bug ID: 29192

            if (AgeCondition != string.Empty)
                //Query += " extract( year from h.birth_date) " + AgeCondition + "extract(year from( date_sub(current_date,interval " + Convert.ToString(Age) + " year)))";
                //else if(AgeCondition=="=")
                Query += "Floor((DATEDIFF(Now(),h.Birth_Date)/365.25))" + AgeCondition + Convert.ToString(Age);//To Calculate the Age Approximately Bug Id:29152
            else if (AgeFrom == 0 && AgeTo == 0) { }
            //else if (AgeFrom > 0)Changed condition for Bug Id: 29094           
            else if (AgeFrom >= 0)
                Query += "h.birth_date <= date_sub(current_date,interval " + Convert.ToString(AgeFrom) + " year) and h.birth_date >= date_sub(current_date,interval " + Convert.ToString(AgeTo) + " year)";



            //if (Gender != string.Empty && (AgeCondition != string.Empty || AgeFrom > 0))
            //    Query += " and sex= '" + Gender + "'";//) ";
            if (Gender != string.Empty)
            {
                if (!Query.ToUpper().EndsWith("WHERE "))
                {
                    Query += " and ";
                }
                Query += " h.sex= '" + Gender + "'";
            }//) ";

            //if (!string.IsNullOrEmpty(Race) && (AgeCondition != string.Empty || AgeFrom > 0) && Gender != string.Empty)
            //    Query += " and race= '" + Race + "'";
            if (!string.IsNullOrEmpty(Race))
            {
                if (!Query.ToUpper().EndsWith("WHERE "))
                {
                    Query += " and ";
                }
                Query += " h.race= '" + Race + "'";
            }//) ";

            //if (!string.IsNullOrEmpty(Ethnicity) && (AgeCondition != string.Empty || AgeFrom > 0) && Gender != string.Empty && !string.IsNullOrEmpty(Race))
            //    Query += " and ethnicity= '" + Ethnicity + "'";
            if (!string.IsNullOrEmpty(Ethnicity))
            {
                if (!Query.ToUpper().EndsWith("WHERE "))
                {
                    Query += " and ";
                }
                Query += " h.ethnicity= '" + Ethnicity + "'";
            }//) ";

            //if (!string.IsNullOrEmpty(CommPreference) && (AgeCondition != string.Empty || AgeFrom > 0) && Gender != string.Empty && !string.IsNullOrEmpty(Race) && !string.IsNullOrEmpty(Ethnicity))
            //    Query += " and Preferred_Confidential_Correspodence_Mode= '" + CommPreference + "') ";

            if (!string.IsNullOrEmpty(CommPreference))
            {
                if (!Query.ToUpper().EndsWith("WHERE "))
                {
                    Query += " and ";
                }
                if (CommPreference == "All")
                {
                    IList<StaticLookup> comList = new List<StaticLookup>();
                    StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                    comList = objStaticLookupMgr.getStaticLookupByFieldName("COMMUNICATION TYPE"); ;

                    Query += " h.Preferred_Confidential_Correspodence_Mode in  ('";
                    for (int i = 0; i < comList.Count; i++)
                    {
                        if (i + 1 == comList.Count)
                        {
                            Query += comList[i].Value.ToString() + "') ";

                        }
                        else
                        {
                            Query += comList[i].Value.ToString() + "','";
                        }
                    }
                }
                else
                {
                    Query += " h.Preferred_Confidential_Correspodence_Mode= '" + CommPreference + "'";
                }
            }
            //else
            //    Query += ") ";




            #endregion

            #region Medication Query


            string NdcQuery = string.Empty;
            if (MedicationList != null && MedicationList.Count != 0)
            {
                //NdcQuery = "SELECT atv  FROM `rxnsat` where sab='RXNORM' and atn='ndc' and  rxcui in ( '";

                //  NdcQuery += "('";

                if (!Query.ToUpper().EndsWith("WHERE "))
                    Query += " and ";
                Query += "m.Generic_Name in ('";

                for (int i = 0; i < MedicationList.Count; i++)
                {

                    if (i + 1 == MedicationList.Count)
                    {
                        Query += MedicationList[i].ToString() + "') ";

                    }
                    else
                    {
                        Query += MedicationList[i].ToString() + "','";
                    }
                }

                //if (!Query.ToUpper().EndsWith("WHERE "))
                //    Query += " and ";
                //Query += "m.ndc_id in (" + NdcQuery + ")";
            }



            #endregion

            #region MedicationAllergy Query


            if (Medication_allergyList != null && Medication_allergyList.Count != 0)
            {
                //NdcQuery = "SELECT atv  FROM `rxnsat` where sab='RXNORM' and atn='ndc' and  rxcui in ( '";

                //  NdcQuery += "('";

                if (!Query.ToUpper().EndsWith("WHERE "))
                    Query += " and ";
                Query += "a.Allergy_Name in ('";

                for (int i = 0; i < Medication_allergyList.Count; i++)
                {

                    if (i + 1 == Medication_allergyList.Count)
                    {
                        Query += Medication_allergyList[i].ToString() + "') ";

                    }
                    else
                    {
                        Query += Medication_allergyList[i].ToString() + "','";
                    }
                }

                //if (!Query.ToUpper().EndsWith("WHERE "))
                //    Query += " and ";
                //Query += "m.ndc_id in (" + NdcQuery + ")";
            }



            #endregion

            #region ProblemList Query


            IList<string> problist = new List<string>();
            if (ProblemList.Count > 0)
            {
                for (int y = 0; y < ProblemList.Count; y++)
                    problist.Add(ProblemList[y]);
            }



            //if (FreqProblemList != null && FreqProblemList.Count > 0)
            //{
            //    IList<object> ilstAlllIcd = session.GetISession().CreateSQLQuery("SELECT all_icd FROM `all_icd` where all_icd like  '%" + FreqProblemList[0].ToString() + "%'").List<object>();
            //    if (ilstAlllIcd.Count > 0)
            //    {
            //        for (int i = 0; i < ilstAlllIcd.Count; i++)
            //        {
            //            if (problist.Contains(ilstAlllIcd[i].ToString()) == false)
            //            {
            //                problist.Add(ilstAlllIcd[i].ToString());
            //            }
            //        }
            //    }
            //}
            if (problist != null && problist.Count != 0)
            {
                if (!Query.ToUpper().EndsWith("WHERE "))
                    Query += " and ";

                Query += "p.ICD in ('";
                for (int i = 0; i < problist.Count; i++)
                {
                    if (i + 1 == problist.Count)
                        Query += problist[i] + "') ";
                    else
                        Query += problist[i] + "','";
                }
            }

            #endregion

            #region Immunization Query

            //if (ImmunDesc != string.Empty)
            //{
            //    if (!Query.ToUpper().EndsWith("WHERE "))
            //        Query += " and ";

            //    string[] splitValue = ImmunDesc.Split('+');
            //    string sDescription = splitValue[0].ToString();
            //    string sCondition = splitValue[1].ToString();

            //    Query += " i.Immunization_Description " + splitValue[1].ToString() + " '" + splitValue[0].ToString() + "' ";
            //}

            //if (ImmuFromDate != DateTime.MinValue)
            //    Query += " and i.Created_Date_And_Time >= '" + ImmuFromDate.ToString("yyyy-MM-dd") + "'";

            //if (ImmuToDate != DateTime.MinValue)
            //    Query += " and i.Created_Date_And_Time <= '" + ImmuToDate.ToString("yyyy-MM-dd") + "'";

            #endregion



            if (!Query.ToUpper().EndsWith("WHERE "))
                Query += " and e.Date_of_Service between '" + fromDate + "' and '" + toDate;
            else
                Query += "e.Date_of_Service between '" + fromDate + "' and '" + toDate;
            //jis
            Query += "' group by h.human_id";

            if (pageNumber > 1)
            {
                int Result = maxResults * pageNumber;
                int start = Result - 25;
                Query += " limit " + start + " , " + Result + "";
            }

            //PageNumber = PageNumber - 1;
            //IList<object> hList = session.GetISession().CreateSQLQuery(Query).SetFirstResult(PageNumber * MaxResultSet).SetMaxResults(MaxResultSet).List<object>();
            IList<object> hList = new List<object>();
            IList<object> idList = new List<object>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                hList = iMySession.CreateSQLQuery(Query).List<object>();
                iMySession.Close();
            }
            idList = hList;


            if (hList != null)
            {
                if (pageNumber == 1)
                {
                    for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > hList.Count ? hList.Count : (maxResults * pageNumber)); i++)
                    {

                        objGeneratePatientListsDTO = new GeneratePatientListsDTO();
                        object[] oj = (object[])hList[i];
                        objGeneratePatientListsDTO.PatientAccountNo = Convert.ToUInt64(oj[0]);
                        objGeneratePatientListsDTO.PatientName = Convert.ToString(oj[1]);
                        DateTime Birth_Date = Convert.ToDateTime(oj[2]);
                        objGeneratePatientListsDTO.DOB = Birth_Date.ToString("dd-MMM-yyyy");
                        objGeneratePatientListsDTO.Age = DateTime.Now.Year - Birth_Date.Year;
                        if (DateTime.Now.Month < Birth_Date.Month || (DateTime.Now.Month == Birth_Date.Month && DateTime.Now.Day < Birth_Date.Day))//To Calculate the Age Approximately Bug Id:29152
                            --objGeneratePatientListsDTO.Age;

                        objGeneratePatientListsDTO.Gender = Convert.ToString(oj[3]);
                        objGeneratePatientListsDTO.Medication = Convert.ToString(oj[4]);
                        if (oj[5] == null)
                        {
                            objGeneratePatientListsDTO.Medication_Date = Convert.ToString(oj[5]);
                        }
                        else
                        {
                            DateTime Med_Date = Convert.ToDateTime(oj[5]);
                            objGeneratePatientListsDTO.Medication_Date = Med_Date.ToString("dd-MMM-yyyy HH:mm:ss");
                        }
                        objGeneratePatientListsDTO.MedicationAllergy = Convert.ToString(oj[6]);
                        if (oj[7] == null)
                        {
                            objGeneratePatientListsDTO.Medication_Allergy_Date = Convert.ToString(oj[7]);
                        }
                        else
                        {
                            DateTime Med_Date = Convert.ToDateTime(oj[7]);
                            objGeneratePatientListsDTO.Medication_Allergy_Date = Med_Date.ToString("dd-MMM-yyyy HH:mm:ss");
                        }

                        objGeneratePatientListsDTO.ProblemList = Convert.ToString(oj[8]);
                        if (oj[9] == null)
                        {
                            objGeneratePatientListsDTO.ProblemList_Date = Convert.ToString(oj[9]);
                        }
                        else
                        {
                            DateTime Prob_Date = Convert.ToDateTime(oj[9]);
                            objGeneratePatientListsDTO.ProblemList_Date = Prob_Date.ToString("dd-MMM-yyyy HH:mm:ss");
                        }
                        //objGeneratePatientListsDTO.Immunization = Convert.ToString(oj[6]) + "-" + Convert.ToString(oj[7]);                   
                        objGeneratePatientListsDTO.Race = Convert.ToString(oj[10]);
                        objGeneratePatientListsDTO.Ethnicity = Convert.ToString(oj[11]);
                        objGeneratePatientListsDTO.CommunicationPreference = Convert.ToString(oj[12]);
                        DateTime enc_date = Convert.ToDateTime(oj[13]);
                        objGeneratePatientListsDTO.Encounter_Date = enc_date.ToString("dd-MMM-yyyy HH:mm:ss");
                        objGeneratePatientListsDTO.LabResult = null;
                        objGeneratePatientListsDTO.LabResult_Date = null;
                        if (pageNumber == 1)
                            objGeneratePatientListsDTO.Total_Record_Found = Convert.ToUInt64(hList.Count);
                        //if (objGeneratePatientListsDTO.Immunization == "-")
                        //    objGeneratePatientListsDTO.Immunization = string.Empty;
                        PatientList.Add(objGeneratePatientListsDTO);
                    }
                }
                else
                {
                    foreach (object[] oj in hList)
                    {
                        objGeneratePatientListsDTO = new GeneratePatientListsDTO();
                        objGeneratePatientListsDTO.PatientAccountNo = Convert.ToUInt64(oj[0]);
                        objGeneratePatientListsDTO.PatientName = Convert.ToString(oj[1]);
                        DateTime Birth_Date = Convert.ToDateTime(oj[2]);
                        objGeneratePatientListsDTO.DOB = Birth_Date.ToString("dd-MMM-yyyy");
                        objGeneratePatientListsDTO.Age = DateTime.Now.Year - Birth_Date.Year;
                        if (DateTime.Now.Month < Birth_Date.Month || (DateTime.Now.Month == Birth_Date.Month && DateTime.Now.Day < Birth_Date.Day))//To Calculate the Age Approximately Bug Id:29152
                            --objGeneratePatientListsDTO.Age;
                        objGeneratePatientListsDTO.Gender = Convert.ToString(oj[3]);
                        objGeneratePatientListsDTO.Medication = Convert.ToString(oj[4]);
                        if (oj[5] == null)
                        {
                            objGeneratePatientListsDTO.Medication_Date = Convert.ToString(oj[5]);
                        }
                        else
                        {
                            DateTime Med_Date = Convert.ToDateTime(oj[5]);
                            objGeneratePatientListsDTO.Medication_Date = Med_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                        }
                        objGeneratePatientListsDTO.MedicationAllergy = Convert.ToString(oj[6]);
                        if (oj[7] == null)
                        {
                            objGeneratePatientListsDTO.Medication_Allergy_Date = Convert.ToString(oj[7]);
                        }
                        else
                        {
                            DateTime Med_Date = Convert.ToDateTime(oj[7]);
                            objGeneratePatientListsDTO.Medication_Allergy_Date = Med_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                        }

                        objGeneratePatientListsDTO.ProblemList = Convert.ToString(oj[8]);
                        if (oj[9] == null)
                        {
                            objGeneratePatientListsDTO.ProblemList_Date = Convert.ToString(oj[9]);
                        }
                        else
                        {
                            DateTime Prob_Date = Convert.ToDateTime(oj[9]);
                            objGeneratePatientListsDTO.ProblemList_Date = Prob_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                        }
                        //objGeneratePatientListsDTO.Immunization = Convert.ToString(oj[6]) + "-" + Convert.ToString(oj[7]);                   
                        objGeneratePatientListsDTO.Race = Convert.ToString(oj[10]);
                        objGeneratePatientListsDTO.Ethnicity = Convert.ToString(oj[11]);
                        objGeneratePatientListsDTO.CommunicationPreference = Convert.ToString(oj[12]);
                        DateTime enc_date = Convert.ToDateTime(oj[13]);
                        objGeneratePatientListsDTO.Encounter_Date = enc_date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                        objGeneratePatientListsDTO.LabResult = null;
                        objGeneratePatientListsDTO.LabResult_Date = null;
                        //objGeneratePatientListsDTO.Total_Record_Found = Convert.ToUInt64(hList.Count);
                        //if (objGeneratePatientListsDTO.Immunization == "-")
                        //    objGeneratePatientListsDTO.Immunization = string.Empty;
                        PatientList.Add(objGeneratePatientListsDTO);
                    }

                }

            }
            if (idList != null)
            {
                foreach (object[] oj in idList)
                {
                    objGeneratePatientListsDTO = new GeneratePatientListsDTO();
                    objGeneratePatientListsDTO.PatientAccountNo = Convert.ToUInt64(oj[0]);
                    objGeneratePatientListsDTO.PatientName = Convert.ToString(oj[1]);
                    DateTime Birth_Date = Convert.ToDateTime(oj[2]);
                    objGeneratePatientListsDTO.DOB = Birth_Date.ToString("dd-MMM-yyyy");
                    objGeneratePatientListsDTO.Age = DateTime.Now.Year - Birth_Date.Year;
                    objGeneratePatientListsDTO.Gender = Convert.ToString(oj[3]);
                    objGeneratePatientListsDTO.Medication = Convert.ToString(oj[4]);
                    if (oj[5] == null)
                    {
                        objGeneratePatientListsDTO.Medication_Date = Convert.ToString(oj[5]);
                    }
                    else
                    {
                        DateTime Med_Date = Convert.ToDateTime(oj[5]);
                        objGeneratePatientListsDTO.Medication_Date = Med_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                    }
                    objGeneratePatientListsDTO.MedicationAllergy = Convert.ToString(oj[6]);
                    if (oj[7] == null)
                    {
                        objGeneratePatientListsDTO.Medication_Allergy_Date = Convert.ToString(oj[7]);
                    }
                    else
                    {
                        DateTime Med_Date = Convert.ToDateTime(oj[7]);
                        objGeneratePatientListsDTO.Medication_Allergy_Date = Med_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                    }

                    objGeneratePatientListsDTO.ProblemList = Convert.ToString(oj[8]);
                    if (oj[9] == null)
                    {
                        objGeneratePatientListsDTO.ProblemList_Date = Convert.ToString(oj[9]);
                    }
                    else
                    {
                        DateTime Prob_Date = Convert.ToDateTime(oj[9]);
                        objGeneratePatientListsDTO.ProblemList_Date = Prob_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                    }
                    //objGeneratePatientListsDTO.Immunization = Convert.ToString(oj[6]) + "-" + Convert.ToString(oj[7]);                   
                    objGeneratePatientListsDTO.Race = Convert.ToString(oj[10]);
                    objGeneratePatientListsDTO.Ethnicity = Convert.ToString(oj[11]);
                    objGeneratePatientListsDTO.CommunicationPreference = Convert.ToString(oj[12]);
                    DateTime enc_date = Convert.ToDateTime(oj[13]);
                    objGeneratePatientListsDTO.Encounter_Date = enc_date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                    objGeneratePatientListsDTO.LabResult = null;
                    objGeneratePatientListsDTO.LabResult_Date = null;
                    //objGeneratePatientListsDTO.Total_Record_Found = Convert.ToUInt64(hList.Count);
                    //if (objGeneratePatientListsDTO.Immunization == "-")
                    //    objGeneratePatientListsDTO.Immunization = string.Empty;
                    TestList.Add(objGeneratePatientListsDTO);
                }
            }
            List<object> ids = new List<object>();
            IList<object> rList = new List<object>();



            #region Testresult Query


            if ((sLabResultCondition.Length > 0 && sLabResultCondition[0] != null) || (sCodition.Length > 0 && sCodition[0] != null))
            {
                foreach (object[] oj in idList)
                {
                    ids.Add(Convert.ToUInt64(oj[0]).ToString());
                }
                string RQuery = "Select c.human_id," +

                "group_concat(distinct(concat(u.OBX_Observation_Text,'-',u.OBX_Observation_Value,' ',u.OBX_Units)) separator ',')as LabResult,cast(u.Created_Date_And_Time as char(100)) as Lab_Date" +
                 " from encounter c" +
                 " inner join human h on (h.human_id=c.human_id)" +
                 " inner join result_master e on (e.Matching_Patient_ID = c.human_id) " +
                 " inner join result_obx u on (u.Result_Master_ID = e.Result_Master_ID)" +
                 " where ";


                if (!RQuery.ToUpper().EndsWith("WHERE "))
                    RQuery += " and " + " (";
                else
                    RQuery += "(";
                RQuery = constructQuery(sLabResultCondition, RQuery, 0);
                if (sCodition[0] != null && Convert.ToString(sCodition[0]) != string.Empty)
                {
                    RQuery += Convert.ToString(sCodition[0]);
                    RQuery = constructQuery(sLabResultCondition, RQuery, 1);
                }
                if (sCodition[1] != null && Convert.ToString(sCodition[1]) != string.Empty)
                {
                    RQuery += Convert.ToString(sCodition[1]);
                    RQuery = constructQuery(sLabResultCondition, RQuery, 2);
                }
                if (sCodition[0] != null && Convert.ToString(sCodition[2]) != string.Empty)
                {
                    RQuery += Convert.ToString(sCodition[2]);
                    RQuery = constructQuery(sLabResultCondition, RQuery, 3);
                }
                RQuery += " )";


                RQuery += " and c.human_id in ('";
                for (int i = 0; i < ids.Count; i++)
                {
                    if (i + 1 == ids.Count)
                        RQuery += ids[i] + "') ";
                    else
                        RQuery += ids[i] + "','";
                }
                RQuery += " group by c.human_id ";


                //if (pageNumber > 1)
                //{
                //    Query += "limit " + ((pageNumber - 1) * maxResults) + " , " + maxResults + "";
                //}
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    rList = iMySession.CreateSQLQuery(RQuery).List<object>();
                    iMySession.Close();
                }
                IList<GeneratePatientListsDTO> Lablist = new List<GeneratePatientListsDTO>();
                if (rList.Count > 0)
                {
                    if (pageNumber == 1)
                    {
                        for (int i = ((pageNumber - 1) * maxResults); i < ((pageNumber * maxResults) > rList.Count ? rList.Count : (maxResults * pageNumber)); i++)
                        {

                            object[] lresult = (object[])rList[i];
                            if (ids.Contains(lresult[0].ToString()))
                            {
                                ids.Remove(lresult[0].ToString());
                            }
                            Lablist = (from p in TestList where p.PatientAccountNo == Convert.ToUInt64(lresult[0]) select p).ToList();
                            if (Lablist.Count > 0)
                            {
                                Lablist[0].LabResult = Convert.ToString(lresult[1]);
                                if (lresult[2] == null)
                                {
                                    Lablist[0].LabResult_Date = Convert.ToString(lresult[2]);
                                }
                                else
                                {
                                    DateTime Lab_Date = Convert.ToDateTime(lresult[2]);
                                    Lablist[0].LabResult_Date = Lab_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                                }
                                Lablist[0].Total_Record_Found = Convert.ToUInt64(rList.Count);
                            }

                            //for (int idsCount = 0; idsCount < ids.Count; idsCount++)
                            //{
                            //    Lablist = (from p in TestList where p.PatientAccountNo == Convert.ToUInt64(ids[idsCount]) select p).ToList();
                            //    if (Lablist.Count > 0)
                            //    {
                            Test_List.Add(Lablist[0]);
                            //    }
                            //}
                        }
                    }
                    else
                    {

                        foreach (object[] lresult in rList)
                        {
                            if (ids.Contains(lresult[0].ToString()))
                            {
                                ids.Remove(lresult[0].ToString());
                            }
                            Lablist = (from p in TestList where p.PatientAccountNo == Convert.ToUInt64(lresult[0]) select p).ToList();
                            if (Lablist.Count > 0)
                            {
                                Lablist[0].LabResult = Convert.ToString(lresult[1]);
                                if (lresult[2] == null)
                                {
                                    Lablist[0].LabResult_Date = Convert.ToString(lresult[2]);
                                }
                                else
                                {
                                    DateTime Lab_Date = Convert.ToDateTime(lresult[2]);
                                    Lablist[0].LabResult_Date = Lab_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                                }
                                Lablist[0].Total_Record_Found = Convert.ToUInt64(rList.Count);
                            }
                        }
                        for (int idsCount = 0; idsCount < ids.Count; idsCount++)
                        {
                            Lablist = (from p in TestList where p.PatientAccountNo == Convert.ToUInt64(ids[idsCount]) select p).ToList();
                            if (Lablist.Count > 0)
                            {
                                TestList.Remove(Lablist[0]);
                            }
                        }
                    }

                }
                else
                {
                    TestList.Clear();
                }


            }
            #endregion




            Hashtable patientResult = new Hashtable();
            // patientResult.Add("GeneratedPatientList", PatientList);
            if ((sLabResultCondition.Length > 0 && sLabResultCondition[0] != null) || (sCodition.Length > 0 && sCodition[0] != null))
            {
                patientResult.Add("GeneratedPatientList", Test_List);
            }
            else
            {
                patientResult.Add("GeneratedPatientList", PatientList);
            }

            if (pageNumber == 0)
                patientResult.Add("TotalCount", PatientList.Count);

            serializer.WriteObject(stream, patientResult);
            stream.Seek(0L, SeekOrigin.Begin);

            return stream;
        }

        //Added by velmurugan on 9th May 2011.
        public string constructQuery(string[] sTestResultname, string sReturnQuery, int n)
        {
            if (sTestResultname[n] != null && sTestResultname[n] != string.Empty)
            {
                sReturnQuery += " (" + "u. OBX_Observation_Text";
                string[] split = sTestResultname[n].ToString().Split('@');
                sReturnQuery += " = " + "'" + split[0].ToString() + "'" + " and u.OBX_Observation_Value";
                if (split[1].ToString().Contains("-") == true)
                {
                    string[] splbetween = split[1].Split('-');
                    sReturnQuery += " between " + splbetween[0].ToString() + " and " + splbetween[1].ToString() + " )";
                }
                else
                    sReturnQuery += split[1] + " ) ";
            }
            return sReturnQuery;
        }

        public Human FillUpdateObj(Human obj, Human objhuman)
        {
            obj.First_Name = objhuman.First_Name;
            obj.Last_Name = objhuman.Last_Name;
            obj.MI = objhuman.MI;
            obj.Suffix = objhuman.Suffix;
            obj.Birth_Date = objhuman.Birth_Date;
            obj.Sex = objhuman.Sex;
            obj.Home_Phone_No = objhuman.Home_Phone_No;
            obj.Street_Address1 = objhuman.Street_Address1;
            obj.Street_Address2 = objhuman.Street_Address2;
            obj.City = objhuman.City;
            obj.State = objhuman.State;
            obj.ZipCode = objhuman.ZipCode;
            obj.SSN = objhuman.SSN;

            return obj;
        }

        public HumanDTO GetHumanInuranceAndCarrierDetails(ulong ulHuman_id)
        {
            HumanDTO objHuman = new HumanDTO();
            //Latha - Branch_52_production_for_Rcopia - Start - 4 Jul 2011
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteriaHuman = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", ulHuman_id));
                //Latha - Branch_52_production_for_Rcopia - End - 4 Jul 2011
                if (criteriaHuman.List<Human>().Count != 0)
                {
                    objHuman.HumanDetails = criteriaHuman.List<Human>()[0];
                }
                //ICriteria criteriaPCP = session.GetISession().CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", Convert.ToUInt64(objHuman.HumanDetails.Encounter_Provider_ID)));

                //if (criteriaPCP.List<PhysicianLibrary>().Count != 0)
                //{
                //    objHuman.PhyFirstName = criteriaPCP.List<PhysicianLibrary>()[0].PhyLastName + "," + criteriaPCP.List<PhysicianLibrary>()[0].PhyFirstName + " " + criteriaPCP.List<PhysicianLibrary>()[0].PhyMiddleName;

                //}
                foreach (PatientInsuredPlan p in objHuman.HumanDetails.PatientInsuredBag)
                {
                    if (p.Insurance_Type.ToUpper() == "PRIMARY" && p.Active.ToUpper() == "YES")
                    {
                        ICriteria criteriaPlan = session.GetISession().CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", p.Insurance_Plan_ID));

                        if (criteriaPlan.List<InsurancePlan>().Count != 0)
                        {
                            objHuman.InsuranceDetails = criteriaPlan.List<InsurancePlan>()[0];
                        }

                    }
                }
                //ICriteria criteriaCarrier = session.GetISession().CreateCriteria(typeof(Carrier)).Add(Expression.Eq("Id", Convert.ToUInt64(objHuman.InsuranceDetails.Carrier_ID)));
                //if (criteriaCarrier.List<Carrier>().Count != 0)
                //{
                //    objHuman.CarrierDetails = criteriaCarrier.List<Carrier>()[0];
                //}

                //Added by bala for Electronic Super Bill
                foreach (PatientInsuredPlan p in objHuman.HumanDetails.PatientInsuredBag)
                {
                    if (p.Insurance_Type.ToUpper() == "SECONDARY" && p.Active.ToUpper() == "YES")
                    {
                        ICriteria criteriaPlan = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", p.Insurance_Plan_ID));

                        if (criteriaPlan.List<InsurancePlan>().Count != 0)
                        {
                            objHuman.SecInsuranceDetails = criteriaPlan.List<InsurancePlan>()[0];
                        }

                    }
                }
                iMySession.Close();
            }


            return objHuman;
        }

        public IList<Human> CheckPatientPortal(string sMail, string sPassword, ulong PatientId)
        {
            IList<Human> HumanList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //Cap - 2033    
                //ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("EMail", sMail)).Add(Expression.Eq("Password", sPassword));
                ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("EMail", sMail)).Add(Expression.Eq("Password", sPassword)).Add(Expression.Eq("Id", PatientId));
                HumanList = crit.List<Human>();
                iMySession.Close();
            }

            return HumanList;
        }

        public IList<Human> CheckQuarantorPatientPortal(string sMail, string sPassword)
        {
            IList<Human> HumanList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Representative_Email", sMail)).Add(Expression.Eq("Representative_Password", sPassword));
                HumanList = crit.List<Human>();
                iMySession.Close();
            }

            return HumanList;
        }
        //Latha - Branch_52_production_for_Rcopia - Start - 4 Jul 2011. End - 4 Jul 2011.
        public Human GetHumanIfDuplicateEMail(string email, string sLegalOrg)
        {
            Human humanObj = new Human();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("EMail", email)).Add(Expression.Eq("Legal_Org", sLegalOrg)) ;
                IList<Human> humanListObj = crit.List<Human>();
                if (humanListObj != null && humanListObj.Count != 0)
                {
                    humanObj = humanListObj[0];
                }
                else
                {
                    humanObj = null;
                }
                iMySession.Close();
            }
            return humanObj;
        }

        //Latha - Branch_56 - Start - 23 Jul 2011. End - 23 Jul 2011
        public void SendingMail(Human objHuman)
        {
            string[] ToAddress = new string[2];
            IList<StaticLookup> objlookupvalue = new List<StaticLookup>();
            IList<StaticLookup> objEhrLink = new List<StaticLookup>();
            StaticLookupManager objStaticLookupMgr = new StaticLookupManager();

            string[] FieldName = { "E MAIL LINK", "E MAIL" };

            objlookupvalue = objStaticLookupMgr.getStaticLookupByFieldName(FieldName, "Sort_Order");

            IList<StaticLookup> EMAILLINKStaticLst = null;
            EMAILLINKStaticLst = objlookupvalue.Where(l => l.Field_Name == "E MAIL LINK").ToList<StaticLookup>();

            IList<StaticLookup> EMAILStaticLst = null;
            EMAILStaticLst = objlookupvalue.Where(l => l.Field_Name == "E MAIL").ToList<StaticLookup>();
            SmtpClient SmtpMail = new SmtpClient(EMAILStaticLst[0].Default_Value, 25);
            //SmtpClient SmtpMail = new SmtpClient("mail.mdofficemail.com", 25);
            SmtpMail.EnableSsl = true;
            ToAddress[0] = objHuman.EMail;
            //ToAddress[1] = objHuman.Representative_EMail;
            ToAddress[1] = objHuman.Representative_Email;
            SmtpMail.Credentials = new System.Net.NetworkCredential(EMAILStaticLst[0].Value, EMAILStaticLst[0].Description);
            string Message = EMAILLINKStaticLst[0].Description;
            for (int i = 0; i < ToAddress.Count(); i++)
            {
                if (ToAddress[i].ToString() != string.Empty)
                {
                    SmtpMail.Send(EMAILStaticLst[0].Value, ToAddress[i].ToString(), "Online Access Registration with Capella EHR", Message.Replace("?", objHuman.First_Name + "" + objHuman.MI + " " + objHuman.Last_Name).Replace("|", EMAILLINKStaticLst[0].Value + objHuman.Id + "&Email=" + ToAddress[i].ToString()));// "Online Access Registration with Capella EHR", "Dear " + objHuman.First_Name + "" + objHuman.MI + " " + objHuman.Last_Name + "," + Environment.NewLine + "You have been enrolled with Capella EHR for viewing your Health Information online." + Environment.NewLine + Environment.NewLine + "Please click on the following link or copy and paste the link into your browser to start your registration." + Environment.NewLine + objEhrLink[0].Value + objHuman.Id + Environment.NewLine + Environment.NewLine + "Thanks" + Environment.NewLine + "Chapparal Medical Group");
                }
            }
            //MailMessage message = new MailMessage();
            //message.From = "noreply@admresources.com";
            //message.To = objHuman.EMail;
            //message.Subject = "Online Access Registration with Capella EHR";
            //string link = string.Empty;

            //message.Body = "Dear " + objHuman.First_Name + "" + objHuman.MI + " " + objHuman.Last_Name + "," + Environment.NewLine + "You have been enrolled with Capella EHR for viewing your Health Information online." + Environment.NewLine + Environment.NewLine + "Please click on the following link or copy and paste the link into your browser to start your registration." + Environment.NewLine + link + objHuman.Id + Environment.NewLine + Environment.NewLine + "Thanks" + Environment.NewLine + "Chapparal Medical Group";
            //message.BodyFormat = MailFormat.Text;
            //SmtpMail.SmtpServer = "localhost";
            //SmtpMail.Send(message);
        }

        public IList<Human> GetPatientListForRcopia(int First, int Last)
        {
            IList<Human> lstHuman = new List<Human>();
            using (ISession imySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crt = imySession.CreateCriteria(typeof(Human)).SetFirstResult(First).SetMaxResults(Last);
                lstHuman = crt.List<Human>();
                imySession.Close();
            }
            return lstHuman;
        }

        public HumanDTO GetHumanInuranceAndCarrierDetailsRCM(ulong ulHuman_id, string BatchName, string DOOS)
        {

            HumanDTO objHuman = new HumanDTO();
            using (ISession imySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteriaHuman = imySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", ulHuman_id));
                if (criteriaHuman.List<Human>().Count != 0)
                {
                    objHuman.HumanDetails = criteriaHuman.List<Human>()[0];
                }
                if (objHuman.HumanDetails.PatientInsuredBag != null)//Added this condition by srividhya on 21-Jul-2014
                {
                    foreach (PatientInsuredPlan p in objHuman.HumanDetails.PatientInsuredBag)
                    {
                        if (p.Insurance_Type.ToUpper() == "PRIMARY" && p.Active.ToUpper() == "YES")
                        {
                            ICriteria criteriaPlan = imySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", p.Insurance_Plan_ID));

                            ICriteria criteriaEligibility = imySession.CreateCriteria(typeof(Eligibility_Verification)).Add(Expression.Eq("Insurance_Plan_ID", p.Insurance_Plan_ID)).Add(Expression.Eq("Human_ID", p.Human_ID));
                            IList<Eligibility_Verification> eligibilityStatus = criteriaEligibility.List<Eligibility_Verification>();
                            if (criteriaPlan.List<InsurancePlan>().Count != 0)
                            {
                                objHuman.InsuranceDetails = criteriaPlan.List<InsurancePlan>()[0];
                            }
                            if (eligibilityStatus.Count > 0)
                            {
                                IList<Eligibility_Verification> TempList = eligibilityStatus.OrderByDescending(t => t.Eligibility_Verified_Date).ToList<Eligibility_Verification>();
                                if (TempList != null && TempList.Count > 0)
                                {
                                    objHuman.Primary_EligiblityStatus = TempList[0].Eligibility_Status;
                                    if (objHuman.EligiblityVerified == string.Empty)
                                    {
                                        objHuman.EligiblityVerified = "The Last Eligibility verification was done by  " + TempList[0].Created_By + " on " + TempList[0].Eligibility_Verified_Date.ToString("dd-MMM-yyyy") + " for the Primary insurance " + objHuman.InsuranceDetails.Ins_Plan_Name;
                                    }
                                    else
                                    {
                                        objHuman.EligiblityVerified = objHuman.EligiblityVerified + "\n" + "The Last Eligibility verification was done by  " + TempList[0].Created_By + " on " + TempList[0].Eligibility_Verified_Date.ToString("dd-MMM-yyyy") + " for the Primary insurance " + objHuman.InsuranceDetails.Ins_Plan_Name;
                                    }
                                }
                            }
                            if (criteriaPlan.List<InsurancePlan>().Count != 0)
                            {
                                objHuman.InsuranceDetails = criteriaPlan.List<InsurancePlan>()[0];
                            }
                            objHuman.sPriRelation = p.Relationship;
                            objHuman.sPriInsuredID = p.Insured_Human_ID.ToString();
                            IList<Human> humanList = GetPatientDetailsUsingPatientInformattion(p.Insured_Human_ID);
                            if (humanList.Count > 0 && humanList != null)
                            {
                                objHuman.sPriInsuredName = humanList[0].Last_Name + "," + humanList[0].First_Name + " " + humanList[0].MI;
                            }

                        }
                        else if (p.Insurance_Type.ToUpper() == "SECONDARY" && p.Active.ToUpper() == "YES")
                        {
                            ICriteria criteriaPlan = imySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", p.Insurance_Plan_ID));
                            objHuman.SecInsuranceDetails = criteriaPlan.List<InsurancePlan>()[0];
                            ICriteria criteriaEligibility = imySession.CreateCriteria(typeof(Eligibility_Verification)).Add(Expression.Eq("Insurance_Plan_ID", p.Insurance_Plan_ID)).Add(Expression.Eq("Human_ID", p.Human_ID));
                            IList<Eligibility_Verification> eligibilityStatus = criteriaEligibility.List<Eligibility_Verification>();
                            if (eligibilityStatus.Count > 0)
                            {
                                IList<Eligibility_Verification> TempList = eligibilityStatus.OrderByDescending(t => t.Eligibility_Verified_Date).ToList<Eligibility_Verification>();
                                if (TempList != null && TempList.Count > 0)
                                {
                                    objHuman.Secondry_EligiblityStatus = TempList[0].Eligibility_Status;
                                    if (objHuman.EligiblityVerified == string.Empty)
                                    {
                                        objHuman.EligiblityVerified = "The Last Eligibility verification was done by  " + TempList[0].Created_By + " on " + TempList[0].Eligibility_Verified_Date.ToString("dd-MMM-yyyy") + " for the Secondary insurance " + objHuman.SecInsuranceDetails.Ins_Plan_Name;
                                    }

                                    else
                                    {
                                        objHuman.EligiblityVerified = objHuman.EligiblityVerified + "\n" + "The Last Eligibility verification was done by  " + TempList[0].Created_By + " on " + TempList[0].Eligibility_Verified_Date.ToString("dd-MMM-yyyy") + " for the Secondary insurance " + objHuman.SecInsuranceDetails.Ins_Plan_Name;
                                    }

                                }
                            }

                            IList<Human> humanList = GetPatientDetailsUsingPatientInformattion(p.Insured_Human_ID);
                            if (humanList.Count > 0 && humanList != null)
                            {
                                objHuman.sSecondaryInsuranceName = humanList[0].Last_Name + "," + humanList[0].First_Name + " " + humanList[0].MI;
                            }

                            //objHuman.sSecondaryInsuranceName = p.Insurance_Type;
                            objHuman.sSecondaryInsuranceRelation = p.Relationship;
                        }
                        else if (p.Insurance_Type.ToUpper() == "TERTIARY" && p.Active.ToUpper() == "YES")
                        {
                            ICriteria criteriaPlan = imySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", p.Insurance_Plan_ID));
                            InsurancePlan objInsurancePlan = criteriaPlan.List<InsurancePlan>()[0];
                            ICriteria criteriaEligibility = session.GetISession().CreateCriteria(typeof(Eligibility_Verification)).Add(Expression.Eq("Insurance_Plan_ID", p.Insurance_Plan_ID)).Add(Expression.Eq("Human_ID", p.Human_ID));
                            IList<Eligibility_Verification> eligibilityStatus = criteriaEligibility.List<Eligibility_Verification>();
                            if (eligibilityStatus.Count > 0)
                            {
                                IList<Eligibility_Verification> TempList = eligibilityStatus.OrderByDescending(t => t.Eligibility_Verified_Date).ToList<Eligibility_Verification>();
                                if (TempList != null && TempList.Count > 0)
                                {
                                    if (objHuman.EligiblityVerified == string.Empty)
                                    {
                                        objHuman.EligiblityVerified = "The Last Eligibility verification was done by  " + TempList[0].Created_By + " on " + TempList[0].Eligibility_Verified_Date.ToString("dd-MMM-yyyy") + " for the Tertiary insurance " + objInsurancePlan.Ins_Plan_Name;
                                    }

                                    else
                                    {
                                        objHuman.EligiblityVerified = objHuman.EligiblityVerified + "\n" + "The Last Eligibility verification was done by  " + TempList[0].Created_By + " on " + TempList[0].Eligibility_Verified_Date.ToString("dd-MMM-yyyy") + " for the Tertiary insurance " + objInsurancePlan.Ins_Plan_Name;
                                    }

                                }
                            }
                        }
                    }
                }
                //if (BatchName != string.Empty)
                //{
                //    ICriteria criteriaBatchHuman = imySession.CreateCriteria(typeof(BatchHumanMap)).Add(Expression.Eq("Human_ID", ulHuman_id)).Add(Expression.Eq("Batch_Name", BatchName)).Add(Expression.Eq("DOOS", DOOS));
                //    if (criteriaBatchHuman.List<BatchHumanMap>().Count != 0)
                //    {
                //        objHuman.BatchHumanDetails = criteriaBatchHuman.List<BatchHumanMap>()[0];
                //    }
                //    else
                //    {
                //        objHuman.BatchHumanDetails = null;
                //    }
                //}
                ICriteria criteriaGuarantor = imySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", ulHuman_id));
                IList<FileManagementIndex> FileMngmtindexlist = criteriaGuarantor.List<FileManagementIndex>();


                if (FileMngmtindexlist.Count > 0)
                {
                    StaticLookupManager StaticMngr = new StaticLookupManager();
                    string sScannedInfo = string.Empty;
                    IList<StaticLookup> ScannedDocTypeList = StaticMngr.getStaticLookupByDocType("DEMOGRAPHICS", "Sort_Order");
                    if (ScannedDocTypeList != null && ScannedDocTypeList.Count > 0)
                    {
                        for (int i = 0; i < ScannedDocTypeList.Count; i++)
                        {
                            IList<FileManagementIndex> DemoScannedList = FileMngmtindexlist.Where(a => a.Document_Type == ScannedDocTypeList[i].Value).OrderByDescending(t => t.Created_Date_And_Time).ToList<FileManagementIndex>();
                            if (DemoScannedList != null && DemoScannedList.Count > 0)
                            {
                                if (sScannedInfo == string.Empty)
                                {
                                    if (ScannedDocTypeList[i].Default_Value.Split('|').Length > 1)
                                        sScannedInfo = ScannedDocTypeList[i].Default_Value.Split('|')[0] + DemoScannedList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy") + ScannedDocTypeList[i].Default_Value.Split('|')[1] + " " + DemoScannedList[0].Created_By;
                                    else
                                        sScannedInfo = ScannedDocTypeList[i].Default_Value.Split('|')[0] + DemoScannedList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy") + " " + DemoScannedList[0].Created_By;

                                }
                                else
                                {
                                    if (ScannedDocTypeList[i].Default_Value.Split('|').Length > 1)
                                        sScannedInfo = sScannedInfo + "\n" + ScannedDocTypeList[i].Default_Value.Split('|')[0] + DemoScannedList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy") + ScannedDocTypeList[i].Default_Value.Split('|')[1] + " " + DemoScannedList[0].Created_By;
                                    else
                                        sScannedInfo = sScannedInfo + "\n" + ScannedDocTypeList[i].Default_Value.Split('|')[0] + DemoScannedList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy") + " " + DemoScannedList[0].Created_By;

                                }
                            }
                        }
                        objHuman.InsuranceCopy = sScannedInfo;
                    }

                    //IList<FileManagementIndex> Insurance = FileMngmtindexlist.Where(a => a.Document_Type == "Insurance").OrderByDescending(t => t.Created_Date_And_Time).ToList<FileManagementIndex>();
                    //IList<FileManagementIndex> PatientInfo = FileMngmtindexlist.Where(a => a.Document_Type == "Patient Documents").OrderByDescending(t => t.Created_Date_And_Time).ToList<FileManagementIndex>();

                    //if (Insurance != null && Insurance.Count > 0)
                    //{
                    //    objHuman.InsuranceCopy = "Recent Insurance Scanned on  " + Insurance[0].Document_Date.ToString("dd-MMM-yyyy") + " Scanned BY  " + Insurance[0].Created_By;

                    //}
                    //if (PatientInfo.Count > 0)
                    //{
                    //    objHuman.PatientInfo = "Recent Patient Information Scanned on " + PatientInfo[0].Document_Date.ToString("dd-MMM-yyyy") + " Scanned BY " + PatientInfo[0].Created_By;

                    //}
                }
                //ICriteria criteriaEligibillity = session.GetISession().CreateCriteria(typeof(Eligibility_Verification)).Add(Expression.Eq("Human_ID", ulHuman_id)).AddOrder(Order.Desc("Eligibility_Verified_Date"));
                //IList<Eligibility_Verification> EligibillityList = criteriaEligibillity.List<Eligibility_Verification>();
                //if (EligibillityList.Count > 0)
                //{
                //    //objHuman.EligiblityVerified = "The Last Eligibility verification was done by  " + EligibillityList[0].Created_By + " on " + EligibillityList[0].Eligibility_Verified_Date.ToString("dd-MMM-yyyy")+" for the Primary insurance" +;
                //}
                imySession.Close();
            }
            return objHuman;
        }

        //public IList<DenialMyQDTO> GetHumanDetailsFromHuman(string sUserName, string[] ObjType)
        //{
        //    IList<DenialMyQDTO> ilstDenialDetails = new List<DenialMyQDTO>();
        //    Human objHuman = new Human();
        //    DenialMyQDTO objCharge;
        //    using (ISession imySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = imySession.GetNamedQuery("Get.HumanList.From.Human");
        //        query1.SetString(0, sUserName);
        //        query1.SetString(1, sUserName);
        //        query1.SetString(2, sUserName);
        //        query1.SetParameterList("ObjTypeList", ObjType);
        //        ArrayList arySum;
        //        arySum = new ArrayList(query1.List());
        //        for (int i = 0; i < arySum.Count; i++)
        //        {
        //            object[] obj = (object[])arySum[i];
        //            if (arySum[0] != null && arySum.Count != 0)
        //            {
        //                objCharge = new DenialMyQDTO();
        //                objCharge.WF_Object_Id = Convert.ToUInt32(obj[0]);
        //                objCharge.Obj_Type = obj[1].ToString();
        //                objCharge.Obj_Sub_Type = obj[2].ToString();
        //                objCharge.Current_Process = obj[3].ToString();
        //                objCharge.DOOS = obj[4].ToString();
        //                objCharge.Batch_Name = obj[5].ToString();
        //                objCharge.Doc_Type = obj[6].ToString();
        //                objCharge.Doc_Sub_Type = obj[7].ToString();
        //                objCharge.Demos_Enc_PPLine_Rcvd = Convert.ToInt32(obj[8]);
        //                objCharge.Obj_System_Id = obj[9].ToString();
        //                objCharge.Patient_Acc_No = obj[10].ToString();
        //                objCharge.Patient_Name = obj[11].ToString();
        //                objCharge.CPT = obj[12].ToString();
        //                objCharge.Billed_Amt = obj[13].ToString();
        //                objCharge.Insurance_Name = obj[14].ToString();
        //                objCharge.Dr_Name = obj[15].ToString();
        //                objCharge.Speciality = obj[16].ToString();
        //                objCharge.Denial_Category = obj[17].ToString();
        //                objCharge.Arrival_Date = Convert.ToDateTime(obj[18]);
        //                objCharge.Charge_Line_Item_Id = obj[19].ToString();
        //                objCharge.DOS = obj[20].ToString();

        //                ilstDenialDetails.Add(objCharge);
        //            }

        //        }
        //        imySession.Close();
        //    }
        //    return ilstDenialDetails;
        //}

        public IList<Human> patientdetails(string patientid)
        {
            IList<Human> details = new List<Human>();

            Human ObjHuman = null;
            using (ISession imySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = imySession.GetNamedQuery("Get.PatientDetails");
                query1.SetString(0, patientid);

                ArrayList arySum;
                arySum = new ArrayList(query1.List());

                for (int i = 0; i < arySum.Count; i++)
                {
                    object[] obj = (object[])arySum[i];
                    ObjHuman = new Human();


                    ObjHuman.First_Name = obj[0].ToString();
                    ObjHuman.Birth_Date = Convert.ToDateTime(obj[1]);

                    ObjHuman.Sex = obj[2].ToString();
                    ObjHuman.Marital_Status = obj[3].ToString();
                    ObjHuman.Patient_Status = obj[4].ToString();
                    ObjHuman.Human_Type = obj[5].ToString();
                    ObjHuman.Home_Phone_No = obj[6].ToString();
                    ObjHuman.Cell_Phone_Number = obj[7].ToString();
                    ObjHuman.Encounter_Provider_ID = Convert.ToInt32(obj[8]);

                    details.Add(ObjHuman);
                }
                imySession.Close();
            }

            return details;
        }

        public ulong GetExceptionCallQCHumanID(string ObjectType, ulong ObjSystemid)
        {
            ulong HumanID = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (ObjectType == "CALL")
                {
                    ICriteria criteriaCall = iMySession.CreateCriteria(typeof(CallLog)).Add(Expression.Eq("Id", ObjSystemid));
                    IList<CallLog> CallLogList = criteriaCall.List<CallLog>();
                    if (CallLogList.Count > 0)
                    {
                        HumanID = CallLogList[0].Account_Number;
                    }
                }
                if (ObjectType == "EXCEPTION")
                {
                    ICriteria criteriaException = iMySession.CreateCriteria(typeof(AddException)).Add(Expression.Eq("Id", ObjSystemid));
                    IList<AddException> ExceptionList = criteriaException.List<AddException>();
                    if (ExceptionList.Count > 0)
                    {
                        HumanID = ExceptionList[0].Account_Number;
                    }
                }
                if (ObjectType == "QC_ERROR")
                {
                    ICriteria criteriaQCError = iMySession.CreateCriteria(typeof(Qc_Err)).Add(Expression.Eq("Id", ObjSystemid));
                    IList<Qc_Err> QCErrorList = criteriaQCError.List<Qc_Err>();
                    if (QCErrorList.Count > 0)
                    {
                        HumanID = QCErrorList[0].Account_Number;
                    }
                }
                iMySession.Close();
            }
            return HumanID;
        }

        public IList<Human> GetHumanListForDemoReport(string sDOOS, string sBatchName, string sObjectType, ulong ulWFObjectID)
        {
            IList<string> HumanList = new List<string>();
            IList<Human> FinalHumanList = new List<Human>();
            IList<ObjectProcessHistoryBilling> ObjProcHistBillList = new List<ObjectProcessHistoryBilling>();
            ObjectProcessHistoryBilling ObjProcHistBillRecord = new ObjectProcessHistoryBilling();
            ArrayList arylstAlreadyCompleted = new ArrayList();
            ArrayList MyList = new ArrayList();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Get.Object.Process.History.Billing.Details");
                query1.SetString(0, sDOOS);
                query1.SetString(1, sBatchName);
                query1.SetString(2, sObjectType);
                query1.SetString(3, ulWFObjectID.ToString());

                MyList.AddRange(query1.List());

                if (MyList != null)
                {
                    for (int i = 0; i < MyList.Count; i++)
                    {
                        object[] oj = (object[])MyList[i];

                        ObjProcHistBillRecord = new ObjectProcessHistoryBilling();
                        ObjProcHistBillRecord.Id = Convert.ToUInt32(oj[0]);
                        ObjProcHistBillRecord.Wf_Object_ID = Convert.ToUInt32(oj[1]);
                        ObjProcHistBillRecord.Process_Name = oj[2].ToString();
                        ObjProcHistBillRecord.Process_Start_Date_And_Time = Convert.ToDateTime(oj[3]);
                        ObjProcHistBillRecord.Process_End_Date_And_Time = Convert.ToDateTime(oj[4]);
                        ObjProcHistBillRecord.Obj_Type = oj[5].ToString();
                        ObjProcHistBillRecord.Obj_Sub_Type = oj[6].ToString();
                        ObjProcHistBillRecord.Comments = oj[7].ToString();
                        ObjProcHistBillRecord.User_Name = oj[8].ToString();
                        ObjProcHistBillRecord.DOOS = oj[9].ToString();
                        ObjProcHistBillRecord.Batch_Name = oj[10].ToString();
                        ObjProcHistBillRecord.Doc_Name = oj[11].ToString();
                        ObjProcHistBillRecord.Demos_Enc_PPLine_Comp = Convert.ToUInt32(oj[12]);
                        ObjProcHistBillRecord.Wf_Obj_Amount = Convert.ToDecimal(oj[13]);
                        ObjProcHistBillRecord.Sub_Batch_Range = oj[14].ToString();
                        ObjProcHistBillRecord.Doc_Type = oj[15].ToString();
                        ObjProcHistBillRecord.Doc_Sub_Type = oj[16].ToString();
                        ObjProcHistBillRecord.Version = Convert.ToInt32(oj[17]);
                        ObjProcHistBillRecord.Completed_List = oj[18].ToString();
                        ObjProcHistBillRecord.Duplicate_Demos_Enc_PPLine = Convert.ToInt32(oj[19]);
                        ObjProcHistBillRecord.Duplicate_Amount = Convert.ToDecimal(oj[20]);
                        ObjProcHistBillRecord.Batch_Name_in_Billing = oj[21].ToString();
                        ObjProcHistBillRecord.Print_Name_in_Billing = oj[22].ToString();
                        ObjProcHistBillRecord.Report_Verified = oj[23].ToString();

                        ObjProcHistBillList.Add(ObjProcHistBillRecord);
                    }
                }

                for (int i = 0; i < ObjProcHistBillList.Count; i++)
                {
                    ObjProcHistBillRecord = new ObjectProcessHistoryBilling();
                    ObjProcHistBillRecord = ObjProcHistBillList[i];

                    if (ObjProcHistBillRecord.Completed_List != "")
                    {
                        string[] sSplittedAlreadyCompletedList = null;
                        sSplittedAlreadyCompletedList = ObjProcHistBillRecord.Completed_List.Split(',');
                        for (int j = 0; j < sSplittedAlreadyCompletedList.Length; j++)
                        {
                            if (HumanList.Contains(sSplittedAlreadyCompletedList[j]) == false && sSplittedAlreadyCompletedList[j] != "")
                            {
                                HumanList.Add(sSplittedAlreadyCompletedList[j]); //Unique Already Completed count
                            }
                        }
                    }
                }

                IList<ObjectProcessHistoryBillingTemp> ObjProcHistBillTempList = new List<ObjectProcessHistoryBillingTemp>();
                ObjectProcessHistoryBillingTemp ObjProcHistBillTempRecord = new ObjectProcessHistoryBillingTemp();
                ArrayList arylstAlreadyTempCompleted = new ArrayList();

                ArrayList MyList1 = new ArrayList();

                IQuery query2 = iMySession.GetNamedQuery("Get.Object.Process.History.Billing.Temp.Details");
                query2.SetString(0, sDOOS);
                query2.SetString(1, sBatchName);
                query2.SetString(2, sObjectType);
                query2.SetString(3, ulWFObjectID.ToString());

                MyList1.AddRange(query2.List());

                if (MyList1 != null)
                {
                    for (int i = 0; i < MyList1.Count; i++)
                    {
                        object[] oj = (object[])MyList1[i];

                        ObjProcHistBillTempRecord = new ObjectProcessHistoryBillingTemp();
                        ObjProcHistBillTempRecord.Id = Convert.ToUInt32(oj[0]);
                        ObjProcHistBillTempRecord.Wf_Object_ID = Convert.ToUInt32(oj[1]);
                        ObjProcHistBillTempRecord.Process_Name = oj[2].ToString();
                        ObjProcHistBillTempRecord.Process_Start_Date_And_Time = Convert.ToDateTime(oj[3]);
                        ObjProcHistBillTempRecord.Process_End_Date_And_Time = Convert.ToDateTime(oj[4]);
                        ObjProcHistBillTempRecord.Obj_Type = oj[5].ToString();
                        ObjProcHistBillTempRecord.Obj_Sub_Type = oj[6].ToString();
                        ObjProcHistBillTempRecord.Comments = oj[7].ToString();
                        ObjProcHistBillTempRecord.User_Name = oj[8].ToString();
                        ObjProcHistBillTempRecord.DOOS = oj[9].ToString();
                        ObjProcHistBillTempRecord.Batch_Name = oj[10].ToString();
                        ObjProcHistBillTempRecord.Doc_Name = oj[11].ToString();
                        ObjProcHistBillTempRecord.Demos_Enc_PPLine_Comp = Convert.ToUInt32(oj[12]);
                        ObjProcHistBillTempRecord.Wf_Obj_Amount = Convert.ToDecimal(oj[13]);
                        ObjProcHistBillTempRecord.Sub_Batch_Range = oj[14].ToString();
                        ObjProcHistBillTempRecord.Doc_Type = oj[15].ToString();
                        ObjProcHistBillTempRecord.Doc_Sub_Type = oj[16].ToString();
                        ObjProcHistBillTempRecord.Version = Convert.ToInt32(oj[17]);
                        ObjProcHistBillTempRecord.Completed_List = oj[18].ToString();
                        ObjProcHistBillTempRecord.Duplicate_Demos_Enc_PPLine = Convert.ToInt32(oj[19]);
                        ObjProcHistBillTempRecord.Duplicate_Amount = Convert.ToDecimal(oj[20]);
                        ObjProcHistBillTempRecord.Batch_Name_in_Billing = oj[21].ToString();
                        ObjProcHistBillTempRecord.Print_Name_in_Billing = oj[22].ToString();
                        ObjProcHistBillTempRecord.Report_Verified = oj[23].ToString();
                        ObjProcHistBillTempRecord.Is_Marked_in_History_Table = oj[24].ToString();

                        ObjProcHistBillTempList.Add(ObjProcHistBillTempRecord);
                    }
                }

                for (int i = 0; i < ObjProcHistBillTempList.Count; i++)
                {
                    ObjProcHistBillTempRecord = new ObjectProcessHistoryBillingTemp();
                    ObjProcHistBillTempRecord = ObjProcHistBillTempList[i];

                    if (ObjProcHistBillTempRecord.Completed_List != "")
                    {
                        string[] sSplittedAlreadyCompletedList = null;
                        sSplittedAlreadyCompletedList = ObjProcHistBillTempRecord.Completed_List.Split(',');
                        for (int j = 0; j < sSplittedAlreadyCompletedList.Length; j++)
                        {
                            if (HumanList.Contains(sSplittedAlreadyCompletedList[j]) == false && sSplittedAlreadyCompletedList[j] != "")
                            {
                                HumanList.Add(sSplittedAlreadyCompletedList[j]); //Unique Already Completed count
                            }
                        }
                    }
                }

                //==========================

                for (int m = 0; m < HumanList.Count; m++)
                {
                    ICriteria criteriaHumanList = session.GetISession().CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", Convert.ToUInt64(HumanList[m])));
                    IList<Human> tempList = criteriaHumanList.List<Human>();
                    if (tempList != null && tempList.Count > 0)
                    {
                        FinalHumanList.Add(tempList[0]);
                    }

                }
                iMySession.Close();
            }
            return FinalHumanList;

        }

        //public IList<BatchDTO> GetHumanListForCPAutoReport(string sDOOS, string sBatchName, string sObjectType, ulong ulWFObjectID)
        //{

        //    IList<string> IntRefIDHumanIDDOSList = new List<string>();
        //    IList<Human> FinalHumanList = new List<Human>();
        //    IList<ObjectProcessHistoryBilling> ObjProcHistBillList = new List<ObjectProcessHistoryBilling>();
        //    ObjectProcessHistoryBilling ObjProcHistBillRecord = new ObjectProcessHistoryBilling();
        //    ArrayList arylstAlreadyCompleted = new ArrayList();
        //    ArrayList MyList = new ArrayList();
        //    IList<BatchDTO> BatchDetailDTOList = new List<BatchDTO>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = iMySession.GetNamedQuery("Get.Object.Process.History.Billing.Details.CP");
        //        query1.SetString(0, sDOOS);
        //        query1.SetString(1, sBatchName);
        //        query1.SetString(2, sObjectType);
        //        query1.SetString(3, ulWFObjectID.ToString());

        //        MyList.AddRange(query1.List());

        //        if (MyList != null)
        //        {
        //            for (int i = 0; i < MyList.Count; i++)
        //            {
        //                object[] oj = (object[])MyList[i];

        //                ObjProcHistBillRecord = new ObjectProcessHistoryBilling();
        //                ObjProcHistBillRecord.Id = Convert.ToUInt32(oj[0]);
        //                ObjProcHistBillRecord.Wf_Object_ID = Convert.ToUInt32(oj[1]);
        //                ObjProcHistBillRecord.Process_Name = oj[2].ToString();
        //                ObjProcHistBillRecord.Process_Start_Date_And_Time = Convert.ToDateTime(oj[3]);
        //                ObjProcHistBillRecord.Process_End_Date_And_Time = Convert.ToDateTime(oj[4]);
        //                ObjProcHistBillRecord.Obj_Type = oj[5].ToString();
        //                ObjProcHistBillRecord.Obj_Sub_Type = oj[6].ToString();
        //                ObjProcHistBillRecord.Comments = oj[7].ToString();
        //                ObjProcHistBillRecord.User_Name = oj[8].ToString();
        //                ObjProcHistBillRecord.DOOS = oj[9].ToString();
        //                ObjProcHistBillRecord.Batch_Name = oj[10].ToString();
        //                ObjProcHistBillRecord.Doc_Name = oj[11].ToString();
        //                ObjProcHistBillRecord.Demos_Enc_PPLine_Comp = Convert.ToUInt32(oj[12]);
        //                ObjProcHistBillRecord.Wf_Obj_Amount = Convert.ToDecimal(oj[13]);
        //                ObjProcHistBillRecord.Sub_Batch_Range = oj[14].ToString();
        //                ObjProcHistBillRecord.Doc_Type = oj[15].ToString();
        //                ObjProcHistBillRecord.Doc_Sub_Type = oj[16].ToString();
        //                ObjProcHistBillRecord.Version = Convert.ToInt32(oj[17]);
        //                ObjProcHistBillRecord.Completed_List = oj[18].ToString();
        //                ObjProcHistBillRecord.Duplicate_Demos_Enc_PPLine = Convert.ToInt32(oj[19]);
        //                ObjProcHistBillRecord.Duplicate_Amount = Convert.ToDecimal(oj[20]);
        //                ObjProcHistBillRecord.Batch_Name_in_Billing = oj[21].ToString();
        //                ObjProcHistBillRecord.Print_Name_in_Billing = oj[22].ToString();
        //                ObjProcHistBillRecord.Report_Verified = oj[23].ToString();

        //                ObjProcHistBillList.Add(ObjProcHistBillRecord);
        //            }
        //        }

        //        for (int i = 0; i < ObjProcHistBillList.Count; i++)
        //        {
        //            ObjProcHistBillRecord = new ObjectProcessHistoryBilling();
        //            ObjProcHistBillRecord = ObjProcHistBillList[i];

        //            if (ObjProcHistBillRecord.Completed_List != "")
        //            {
        //                string[] sSplittedAlreadyCompletedList = null;
        //                sSplittedAlreadyCompletedList = ObjProcHistBillRecord.Completed_List.Split(',');
        //                for (int j = 0; j < sSplittedAlreadyCompletedList.Length; j++)
        //                {
        //                    if (IntRefIDHumanIDDOSList.Contains(sSplittedAlreadyCompletedList[j]) == false && sSplittedAlreadyCompletedList[j] != "")
        //                    {
        //                        IntRefIDHumanIDDOSList.Add(sSplittedAlreadyCompletedList[j]); //Unique Already Completed count
        //                    }
        //                }
        //            }
        //        }

        //        IList<ObjectProcessHistoryBillingTemp> ObjProcHistBillTempList = new List<ObjectProcessHistoryBillingTemp>();
        //        ObjectProcessHistoryBillingTemp ObjProcHistBillTempRecord = new ObjectProcessHistoryBillingTemp();
        //        ArrayList arylstAlreadyTempCompleted = new ArrayList();

        //        ArrayList MyList1 = new ArrayList();

        //        IQuery query2 = iMySession.GetNamedQuery("Get.Object.Process.History.Billing.Temp.Details.CP");
        //        query2.SetString(0, sDOOS);
        //        query2.SetString(1, sBatchName);
        //        query2.SetString(2, sObjectType);
        //        query2.SetString(3, ulWFObjectID.ToString());

        //        MyList1.AddRange(query2.List());

        //        if (MyList1 != null)
        //        {
        //            for (int i = 0; i < MyList1.Count; i++)
        //            {
        //                object[] oj = (object[])MyList1[i];

        //                ObjProcHistBillTempRecord = new ObjectProcessHistoryBillingTemp();
        //                ObjProcHistBillTempRecord.Id = Convert.ToUInt32(oj[0]);
        //                ObjProcHistBillTempRecord.Wf_Object_ID = Convert.ToUInt32(oj[1]);
        //                ObjProcHistBillTempRecord.Process_Name = oj[2].ToString();
        //                ObjProcHistBillTempRecord.Process_Start_Date_And_Time = Convert.ToDateTime(oj[3]);
        //                ObjProcHistBillTempRecord.Process_End_Date_And_Time = Convert.ToDateTime(oj[4]);
        //                ObjProcHistBillTempRecord.Obj_Type = oj[5].ToString();
        //                ObjProcHistBillTempRecord.Obj_Sub_Type = oj[6].ToString();
        //                ObjProcHistBillTempRecord.Comments = oj[7].ToString();
        //                ObjProcHistBillTempRecord.User_Name = oj[8].ToString();
        //                ObjProcHistBillTempRecord.DOOS = oj[9].ToString();
        //                ObjProcHistBillTempRecord.Batch_Name = oj[10].ToString();
        //                ObjProcHistBillTempRecord.Doc_Name = oj[11].ToString();
        //                ObjProcHistBillTempRecord.Demos_Enc_PPLine_Comp = Convert.ToUInt32(oj[12]);
        //                ObjProcHistBillTempRecord.Wf_Obj_Amount = Convert.ToDecimal(oj[13]);
        //                ObjProcHistBillTempRecord.Sub_Batch_Range = oj[14].ToString();
        //                ObjProcHistBillTempRecord.Doc_Type = oj[15].ToString();
        //                ObjProcHistBillTempRecord.Doc_Sub_Type = oj[16].ToString();
        //                ObjProcHistBillTempRecord.Version = Convert.ToInt32(oj[17]);
        //                ObjProcHistBillTempRecord.Completed_List = oj[18].ToString();
        //                ObjProcHistBillTempRecord.Duplicate_Demos_Enc_PPLine = Convert.ToInt32(oj[19]);
        //                ObjProcHistBillTempRecord.Duplicate_Amount = Convert.ToDecimal(oj[20]);
        //                ObjProcHistBillTempRecord.Batch_Name_in_Billing = oj[21].ToString();
        //                ObjProcHistBillTempRecord.Print_Name_in_Billing = oj[22].ToString();
        //                ObjProcHistBillTempRecord.Report_Verified = oj[23].ToString();
        //                ObjProcHistBillTempRecord.Is_Marked_in_History_Table = oj[24].ToString();

        //                ObjProcHistBillTempList.Add(ObjProcHistBillTempRecord);
        //            }
        //        }

        //        for (int i = 0; i < ObjProcHistBillTempList.Count; i++)
        //        {
        //            ObjProcHistBillTempRecord = new ObjectProcessHistoryBillingTemp();
        //            ObjProcHistBillTempRecord = ObjProcHistBillTempList[i];

        //            if (ObjProcHistBillTempRecord.Completed_List != "")
        //            {
        //                string[] sSplittedAlreadyCompletedList = null;
        //                sSplittedAlreadyCompletedList = ObjProcHistBillTempRecord.Completed_List.Split(',');
        //                for (int j = 0; j < sSplittedAlreadyCompletedList.Length; j++)
        //                {
        //                    if (IntRefIDHumanIDDOSList.Contains(sSplittedAlreadyCompletedList[j]) == false && sSplittedAlreadyCompletedList[j] != "")
        //                    {
        //                        IntRefIDHumanIDDOSList.Add(sSplittedAlreadyCompletedList[j]); //Unique Already Completed count
        //                    }
        //                }
        //            }
        //        }

        //        //==========================

        //        EncounterManager EncMngr = new EncounterManager();
        //        IList<Encounter> EncTempList = new List<Encounter>();
        //        IList<ChargeLineItem> ChargeLineItemList = new List<ChargeLineItem>();
        //        Human HumanRecord = new Human();
        //        ChargeHeader ChargeHeaderRecord = new ChargeHeader();
        //        ChargeLineItem ChargeLineItemRecord = new ChargeLineItem();
        //        BatchDTO BatchDetailDTO = new BatchDTO();
        //        IList<InsurancePlan> InsPlanList = new List<InsurancePlan>();
        //        IList<PatientInsuredPlan> PatInsPlanList = new List<PatientInsuredPlan>();
        //        PatientInsuredPlanManager PatInsPlanMngr = new PatientInsuredPlanManager();
        //        InsurancePlanManager InsPlanMngr = new InsurancePlanManager();

        //        for (int m = 0; m < IntRefIDHumanIDDOSList.Count; m++)
        //        {
        //            string[] strSplitIntRefIDHumanIDDOS = IntRefIDHumanIDDOSList[m].Split('_');

        //            if (strSplitIntRefIDHumanIDDOS != null && strSplitIntRefIDHumanIDDOS[0] != "" && strSplitIntRefIDHumanIDDOS[1] != "" && strSplitIntRefIDHumanIDDOS[2] != "")
        //            {
        //                //EncTempList = EncMngr.GetEncounterByEncounterID(EncList[m]);

        //                //if (EncTempList.Count > 0)
        //                //{
        //                //ICriteria criteriaHumanList = session.GetISession().CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", Convert.ToUInt64(EncTempList[0].Human_ID)));
        //                //IList<Human> tempList = criteriaHumanList.List<Human>();
        //                //if (tempList != null && tempList.Count > 0)
        //                //{

        //                ISQLQuery sql = iMySession.CreateSQLQuery("SELECT a.*,b.*,h.* FROM charge_line_item a,charge_header b,human h WHERE b.internal_reference_id=" + strSplitIntRefIDHumanIDDOS[0].ToString() + " and b.human_id = " + strSplitIntRefIDHumanIDDOS[1].ToString() + " and a.from_dos='" + strSplitIntRefIDHumanIDDOS[2].ToString() + "' and a.charge_header_id=b.charge_header_id and b.human_id=h.human_id and a.Deleted<>'Y' ORDER BY a.Sort_Order")
        //                                .AddEntity("a", typeof(ChargeLineItem)).AddEntity("b", typeof(ChargeHeader)).AddEntity("h", typeof(Human));
        //                //ChargeLineItemList = sql.List<ChargeLineItem>();

        //                foreach (IList<object> l in sql.List())
        //                {
        //                    ChargeLineItemRecord = (ChargeLineItem)l[0];
        //                    ChargeHeaderRecord = (ChargeHeader)l[1];
        //                    HumanRecord = (Human)l[2];

        //                    if (HumanRecord != null)
        //                    {
        //                        BatchDetailDTO = new BatchDTO();
        //                        BatchDetailDTO.Human_id = HumanRecord.Id;
        //                        BatchDetailDTO.patientname = HumanRecord.Last_Name + "," + HumanRecord.First_Name;
        //                        BatchDetailDTO.DOB = HumanRecord.Birth_Date;
        //                        BatchDetailDTO.Gender = HumanRecord.Sex;
        //                        BatchDetailDTO.MRN_No = HumanRecord.Medical_Record_Number;
        //                        BatchDetailDTO.External_Acc_No = HumanRecord.Patient_Account_External;

        //                        BatchHumanMapManager BatchHumanMapMngr = new BatchHumanMapManager();
        //                        IList<BatchHumanMap> BatchHumanMapList = new List<BatchHumanMap>();

        //                        BatchHumanMapList = BatchHumanMapMngr.GetBatchHumanDetailsbyIntRefIDHumanIDDOS(Convert.ToUInt32(strSplitIntRefIDHumanIDDOS[0]), Convert.ToUInt32(strSplitIntRefIDHumanIDDOS[1]), strSplitIntRefIDHumanIDDOS[2].ToString(), sDOOS, sBatchName);
        //                        if (BatchHumanMapList.Count > 0)
        //                        {
        //                            if (BatchHumanMapList[0].Encounter_ID.ToString() != "0")
        //                                BatchDetailDTO.Voucher_No = BatchHumanMapList[0].Encounter_ID.ToString();
        //                            else
        //                                BatchDetailDTO.Voucher_No = "";
        //                        }

        //                        if (ChargeHeaderRecord != null)
        //                        {
        //                            BatchDetailDTO.Charge_Header_ID = ChargeHeaderRecord.Id;
        //                            BatchDetailDTO.Billing_Facility = ChargeHeaderRecord.Billing_Facility;
        //                            BatchDetailDTO.Claim_on_Hold = ChargeHeaderRecord.Hold_Claim;
        //                            BatchDetailDTO.Internal_Reference_ID = ChargeHeaderRecord.Internal_Reference_ID;
        //                        }
        //                        if (ChargeLineItemRecord != null)
        //                        {
        //                            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
        //                            PhysicianManager PhyMngr = new PhysicianManager();

        //                            PhyList = PhyMngr.GetphysiciannameByPhyID(ChargeLineItemRecord.Rendering_Provider_ID);
        //                            if (PhyList.Count > 0)
        //                            {
        //                                BatchDetailDTO.Provider_ID = PhyList[0].Id;
        //                                BatchDetailDTO.providername = PhyList[0].PhyLastName + "," + PhyList[0].PhyFirstName + " " + PhyList[0].PhyMiddleName + " " + PhyList[0].PhySuffix;
        //                            }

        //                            BatchDetailDTO.DOS = ChargeLineItemRecord.From_DOS.ToString("yyyy-MM-dd");
        //                            BatchDetailDTO.Procedure_code = ChargeLineItemRecord.Procedure_Code;
        //                            if (ChargeLineItemRecord.Modifier1 != "")
        //                                BatchDetailDTO.Modifier = ChargeLineItemRecord.Modifier1;// +"," + ChargeLineItemRecord.Modifier2 + "," + ChargeLineItemRecord.Modifier3 + "," + ChargeLineItemRecord.Modifier4;
        //                            if (ChargeLineItemRecord.Modifier2 != "")
        //                                BatchDetailDTO.Modifier = BatchDetailDTO.Modifier + "," + ChargeLineItemRecord.Modifier2;
        //                            if (ChargeLineItemRecord.Modifier3 != "")
        //                                BatchDetailDTO.Modifier = BatchDetailDTO.Modifier + "," + ChargeLineItemRecord.Modifier3;
        //                            if (ChargeLineItemRecord.Modifier4 != "")
        //                                BatchDetailDTO.Modifier = BatchDetailDTO.Modifier + "," + ChargeLineItemRecord.Modifier4;

        //                            BatchDetailDTO.Units = ChargeLineItemRecord.Units;

        //                            PatInsPlanList = PatInsPlanMngr.getInsuranceDetailsByPatInsuredId(ChargeLineItemRecord.Pat_Insured_Plan_ID);

        //                            if (PatInsPlanList.Count > 0)
        //                            {
        //                                InsPlanList = InsPlanMngr.GetInsurancebyID(PatInsPlanList[0].Insurance_Plan_ID);// ChargeLineItemList[0].Primary_Payer_ID);

        //                                if (InsPlanList.Count > 0)
        //                                {
        //                                    BatchDetailDTO.Pri_Plan_Name = InsPlanList[0].Ins_Plan_Name;// ChargeLineItemRecord.Primary_Payer_Name;
        //                                }

        //                                PatInsPlanList = new List<PatientInsuredPlan>();
        //                                PatInsPlanList = PatInsPlanMngr.getInsuranceDetailsByPatInsuredId(ChargeLineItemRecord.Pat_Other_Insured_Plan_ID);

        //                                if (PatInsPlanList.Count > 0)
        //                                {
        //                                    InsPlanList = new List<InsurancePlan>();
        //                                    InsPlanList = InsPlanMngr.GetInsurancebyID(PatInsPlanList[0].Insurance_Plan_ID);// ChargeLineItemList[0].Primary_Payer_ID);

        //                                    if (InsPlanList.Count > 0)
        //                                    {
        //                                        BatchDetailDTO.Pri_Plan_Name = BatchDetailDTO.Pri_Plan_Name + "/" + InsPlanList[0].Ins_Plan_Name;// ChargeLineItemRecord.Primary_Payer_Name;
        //                                    }
        //                                }
        //                            }

        //                            BatchDetailDTO.Charge_Amount_Line_Item = ChargeLineItemRecord.Charge_Amount;
        //                            BatchDetailDTO.Created_By = ChargeLineItemRecord.Created_By;
        //                            BatchDetailDTO.Created_Date_And_Time = ChargeLineItemRecord.Created_Date_And_Time;
        //                            BatchDetailDTO.Modified_By = ChargeLineItemRecord.Modified_By;
        //                            BatchDetailDTO.Modified_Date_And_Time = ChargeLineItemRecord.Modified_Date_And_Time;
        //                        }

        //                        BatchDetailDTOList.Add(BatchDetailDTO);
        //                    }
        //                }
        //                //}
        //            }
        //        }
        //        iMySession.Close();
        //    }
        //    return BatchDetailDTOList;

        //}

        public HumanDTO FindHumanForPaymentPostingSearch(ulong ulHumanID, string FirstName, string LastName, DateTime dtDOB)
        {

            IQuery query1 = null;
            Human ObjHuman = null;
            HumanDTO objHumanDTO = new HumanDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                query1 = iMySession.GetNamedQuery("Get.HumanDetailsForPaymentPosting");

                query1.SetString(0, LastName + "%");
                query1.SetString(1, FirstName + "%");


                if (ulHumanID != 0)
                {
                    query1.SetString(3, ulHumanID + "%");
                }
                else
                {
                    query1.SetString(3, "%");
                }



                if (dtDOB == DateTime.MinValue)
                //if (dtDOB.ToString() == "1/1/1900 12:00:00 AM")
                {
                    query1.SetString(2, "%");
                }
                else
                {
                    query1.SetString(2, dtDOB.ToString("yyyy-MM-dd") + "%");
                }

                ArrayList ary = new ArrayList(query1.List());

                for (int i = 0; i < ary.Count; i++)
                {
                    object[] obj = (object[])ary[i];
                    ObjHuman = new Human();

                    ObjHuman.Id = Convert.ToUInt64(obj[0]);
                    ObjHuman.Last_Name = obj[2].ToString();
                    ObjHuman.First_Name = obj[3].ToString();
                    ObjHuman.Birth_Date = Convert.ToDateTime(obj[6]);
                    ObjHuman.SSN = obj[7].ToString();
                    ObjHuman.Account_Status = obj[8].ToString();
                    objHumanDTO.HumanList.Add(ObjHuman);

                }
                objHumanDTO.HumanListCount = ary.Count;

                iMySession.Close();
            }
            return objHumanDTO;

        }

        public bool IsACOValid(ulong HumanID)
        {
            Human objHuman = new Human();
            objHuman = GetById(HumanID);
            if (objHuman != null && objHuman.ACO_Is_Eligible_Patient != string.Empty && objHuman.ACO_Is_Eligible_Patient != "N" && objHuman.ACO_Is_Eligible_Patient != "ACO" && objHuman.Patient_Discussed != "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string IsACOEligibility(ulong HumanID)
        {
            string ACOEligibility = string.Empty;
            if (HumanID != 0 && HumanID != null)
            {
                Human objHuman = new Human();
                objHuman = GetById(HumanID);
                ACOEligibility = objHuman.ACO_Is_Eligible_Patient;
            }
            return ACOEligibility;
        }

        public IList<Human> GetHumanByName(string FirstName, string LastName, DateTime DOB, string Sex)
        {

            IList<Human> HumanList = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery query1 = iMySession.CreateSQLQuery("select u.* FROM Human u where u.Last_Name in (:LN) and u.First_Name in (:FN) and u.Birth_Date in (:DOB) and u.Sex in (:Sex)").AddEntity("u", typeof(Human));
                query1.SetParameter("LN", LastName);
                query1.SetParameter("FN", FirstName);
                query1.SetParameter("DOB", DOB);
                query1.SetParameter("Sex", Sex);
                HumanList = query1.List<Human>();
                iMySession.Close();
            }
            return HumanList;
        }

        public IList<Human> GetHumanListForEhrMeasureByLimit(string HumanId, int PageNumber, int maxResults)
        {
            int startIndex = (PageNumber - 1) * maxResults;
            string[] human = HumanId.Split(',');
            IList<Human> hum = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.In("Id", human)).SetFirstResult(startIndex).SetMaxResults(maxResults);
                hum = crit.List<Human>();
                iMySession.Close();
            }
            return hum;
        }

        public int GetHumanListForEhrMeasureRecordCount(string HumanId)
        {
            //IList<int> iVal = new List<int>();
            string[] human = HumanId.Split(',');
            int hum = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.In("Id", human)).SetProjection(Projections.RowCount());
                hum = crit.List<int>()[0];
                iMySession.Close();
            }
            return hum;
        }

        public IList<Human> GetHumanListById(IList<ulong> LstHuman)
        {
            IList<Human> lsthum = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.In("Id", LstHuman.ToArray<ulong>()));
                lsthum = crit.List<Human>();
                iMySession.Close();
            }
            return lsthum;
        }

        public Stream ExportPatientLists(string AgeCondition, int Age, int AgeFrom, int AgeTo, string Gender, string Race, string Ethnicity, string CommPreference, IList<string> MedicationList, IList<string> Medication_allergyList, IList<string> ProblemList, string[] sLabResultCondition, string[] sCodition, IList<string> FreqProblemList, string fromDate, string toDate)
        {

            var stream = new MemoryStream();

            var serializer = new NetDataContractSerializer();
            var foundObjects = new object();

            IList<GeneratePatientListsDTO> PatientList = new List<GeneratePatientListsDTO>();
            GeneratePatientListsDTO objGeneratePatientListsDTO = null;

            IList<GeneratePatientListsDTO> TestList = new List<GeneratePatientListsDTO>();
            // GeneratePatientListsDTO objGeneratePatientTestListsDTO = null;
            IList<GeneratePatientListsDTO> Test_List = new List<GeneratePatientListsDTO>();

            #region Human Query
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                string Query = "Select h.human_Id" +
                                ",if (h.mi='',Concat(h.Last_Name,',',h.First_Name),Concat(h.Last_Name,',',h.First_Name,',',h.MI)) as Patient_Name" +
                                ",cast(h.Birth_Date as char(100)) as Birth_Date,h.Sex";
                if (MedicationList.Count > 0)
                {
                    Query += ",if(m.Generic_Name = '','', group_concat(distinct(m.Generic_Name) separator ',')),cast(m.Created_Date_And_Time as char(100)) as Med_date";
                }
                else
                {
                    Query += ",null,null";
                }
                if (Medication_allergyList.Count > 0)
                {
                    Query += ",if(a.Allergy_Name = '','', group_concat(distinct(a.Allergy_Name) separator ',')),cast(a.Created_Date_And_Time as char(100)) as Med_Allrgydate";
                }
                else
                {
                    Query += ",null,null";
                }
                if (ProblemList.Count > 0)
                {
                    Query += ",group_concat(distinct(concat(p.ICD,'-',p.Problem_Description)) separator ','),cast(p.Created_Date_And_Time as char(100)) as Prob_Date";
                }
                else
                {
                    Query += ",null,null";
                }
                if (!string.IsNullOrEmpty(Race))
                {
                    Query += ",h.Race ";
                }
                else
                {
                    Query += ",null";
                }

                if (!string.IsNullOrEmpty(Ethnicity))
                {
                    Query += ",h.Ethnicity ";
                }
                else
                {
                    Query += ",null";
                }

                if (!string.IsNullOrEmpty(CommPreference))
                {
                    Query += ",h.Preferred_Confidential_Correspodence_Mode";
                }
                else
                {
                    Query += ",null";
                }



                Query += ",cast(e.Date_of_Service as char(100)) as Enc_Date  from encounter e " +
                                "inner join human h on (h.human_id= e.human_id)";

                if (MedicationList.Count > 0)
                {
                    Query += "inner join rcopia_medication m on (m.human_id = h.human_id)";
                }

                if (Medication_allergyList.Count > 0)
                {
                    Query += "inner join rcopia_allergy a on (a.human_id = h.human_id)";
                }

                if (ProblemList.Count > 0)
                {
                    Query += "inner join problem_list P on (p.human_id = h.human_id) ";
                }
                Query += "where ";

                if (Age > 0 || AgeCondition != string.Empty || AgeFrom > 0 || Gender != string.Empty)
                {

                    if (!string.IsNullOrEmpty(AgeCondition))
                    {
                        switch (AgeCondition)
                        {
                            case "<":
                                AgeCondition = ">";
                                Age -= 1;
                                break;
                            case ">":
                                AgeCondition = "<";
                                Age += 1;
                                break;
                            case "<=":
                                AgeCondition = ">=";
                                break;
                            case ">=":
                                AgeCondition = "<=";
                                break;
                            default:
                                AgeCondition = "=";
                                break;
                        }
                    }
                }

                if (AgeCondition != string.Empty)
                    Query += " extract( year from h.birth_date) " + AgeCondition + "extract(year from( date_sub(current_date,interval " + Convert.ToString(Age) + " year)))";
                else if (AgeFrom > 0)
                    Query += "h.birth_date <= date_sub(current_date,interval " + Convert.ToString(AgeFrom) + " year) and h.birth_date >= date_sub(current_date,interval " + Convert.ToString(AgeTo) + " year)";




                if (Gender != string.Empty)
                {
                    if (!Query.ToUpper().EndsWith("WHERE "))
                    {
                        Query += " and ";
                    }
                    Query += " h.sex= '" + Gender + "'";
                }
                if (!string.IsNullOrEmpty(Race))
                {
                    if (!Query.ToUpper().EndsWith("WHERE "))
                    {
                        Query += " and ";
                    }
                    Query += " h.race= '" + Race + "'";
                }
                if (!string.IsNullOrEmpty(Ethnicity))
                {
                    if (!Query.ToUpper().EndsWith("WHERE "))
                    {
                        Query += " and ";
                    }
                    Query += " h.ethnicity= '" + Ethnicity + "'";
                }
                if (!string.IsNullOrEmpty(CommPreference))
                {
                    if (!Query.ToUpper().EndsWith("WHERE "))
                    {
                        Query += " and ";
                    }
                    if (CommPreference == "All")
                    {
                        IList<StaticLookup> comList = new List<StaticLookup>();
                        StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                        comList = objStaticLookupMgr.getStaticLookupByFieldName("COMMUNICATION TYPE"); ;

                        Query += " h.Preferred_Confidential_Correspodence_Mode in  ('";
                        for (int i = 0; i < comList.Count; i++)
                        {
                            if (i + 1 == comList.Count)
                            {
                                Query += comList[i].Value.ToString() + "') ";

                            }
                            else
                            {
                                Query += comList[i].Value.ToString() + "','";
                            }
                        }
                    }
                    else
                    {
                        Query += " h.Preferred_Confidential_Correspodence_Mode= '" + CommPreference + "'";
                    }
                }


            #endregion

                #region Medication Query


                string NdcQuery = string.Empty;
                if (MedicationList != null && MedicationList.Count != 0)
                {


                    if (!Query.ToUpper().EndsWith("WHERE "))
                        Query += " and ";
                    Query += "m.Generic_Name in ('";

                    for (int i = 0; i < MedicationList.Count; i++)
                    {

                        if (i + 1 == MedicationList.Count)
                        {
                            Query += MedicationList[i].ToString() + "') ";

                        }
                        else
                        {
                            Query += MedicationList[i].ToString() + "','";
                        }
                    }

                }



                #endregion

                #region MedicationAllergy Query


                if (Medication_allergyList != null && Medication_allergyList.Count != 0)
                {


                    if (!Query.ToUpper().EndsWith("WHERE "))
                        Query += " and ";
                    Query += "a.Allergy_Name in ('";

                    for (int i = 0; i < Medication_allergyList.Count; i++)
                    {

                        if (i + 1 == Medication_allergyList.Count)
                        {
                            Query += Medication_allergyList[i].ToString() + "') ";

                        }
                        else
                        {
                            Query += Medication_allergyList[i].ToString() + "','";
                        }
                    }
                }

                #endregion

                #region ProblemList Query


                IList<string> problist = new List<string>();
                if (ProblemList.Count > 0)
                {
                    for (int y = 0; y < ProblemList.Count; y++)
                        problist.Add(ProblemList[y]);
                }
                if (problist != null && problist.Count != 0)
                {
                    if (!Query.ToUpper().EndsWith("WHERE "))
                        Query += " and ";

                    Query += "p.ICD in ('";
                    for (int i = 0; i < problist.Count; i++)
                    {
                        if (i + 1 == problist.Count)
                            Query += problist[i] + "') ";
                        else
                            Query += problist[i] + "','";
                    }
                }

                #endregion



                if (!Query.ToUpper().EndsWith("WHERE "))
                    Query += " and e.Date_of_Service between '" + fromDate + "' and '" + toDate;
                else
                    Query += "e.Date_of_Service between '" + fromDate + "' and '" + toDate;

                Query += "' group by h.human_id";


                IList<object> hList = new List<object>();
                IList<object> idList = new List<object>();

                hList = iMySession.CreateSQLQuery(Query).List<object>();


                idList = hList;

                if (idList != null)
                {
                    foreach (object[] oj in idList)
                    {
                        objGeneratePatientListsDTO = new GeneratePatientListsDTO();
                        objGeneratePatientListsDTO.PatientAccountNo = Convert.ToUInt64(oj[0]);
                        objGeneratePatientListsDTO.PatientName = Convert.ToString(oj[1]);
                        DateTime Birth_Date = Convert.ToDateTime(oj[2]);
                        objGeneratePatientListsDTO.DOB = Birth_Date.ToString("dd-MMM-yyyy");
                        objGeneratePatientListsDTO.Age = DateTime.Now.Year - Birth_Date.Year;
                        objGeneratePatientListsDTO.Gender = Convert.ToString(oj[3]);
                        objGeneratePatientListsDTO.Medication = Convert.ToString(oj[4]);
                        if (oj[5] == null)
                        {
                            objGeneratePatientListsDTO.Medication_Date = Convert.ToString(oj[5]);
                        }
                        else
                        {
                            DateTime Med_Date = Convert.ToDateTime(oj[5]);
                            objGeneratePatientListsDTO.Medication_Date = Med_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                        }
                        objGeneratePatientListsDTO.MedicationAllergy = Convert.ToString(oj[6]);
                        if (oj[7] == null)
                        {
                            objGeneratePatientListsDTO.Medication_Allergy_Date = Convert.ToString(oj[7]);
                        }
                        else
                        {
                            DateTime Med_Date = Convert.ToDateTime(oj[7]);
                            objGeneratePatientListsDTO.Medication_Allergy_Date = Med_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                        }

                        objGeneratePatientListsDTO.ProblemList = Convert.ToString(oj[8]);
                        if (oj[9] == null)
                        {
                            objGeneratePatientListsDTO.ProblemList_Date = Convert.ToString(oj[9]);
                        }
                        else
                        {
                            DateTime Prob_Date = Convert.ToDateTime(oj[9]);
                            objGeneratePatientListsDTO.ProblemList_Date = Prob_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                        }
                        //objGeneratePatientListsDTO.Immunization = Convert.ToString(oj[6]) + "-" + Convert.ToString(oj[7]);                   
                        objGeneratePatientListsDTO.Race = Convert.ToString(oj[10]);
                        objGeneratePatientListsDTO.Ethnicity = Convert.ToString(oj[11]);
                        objGeneratePatientListsDTO.CommunicationPreference = Convert.ToString(oj[12]);
                        DateTime enc_date = Convert.ToDateTime(oj[13]);
                        objGeneratePatientListsDTO.Encounter_Date = enc_date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                        objGeneratePatientListsDTO.LabResult = null;
                        objGeneratePatientListsDTO.LabResult_Date = null;
                        //objGeneratePatientListsDTO.Total_Record_Found = Convert.ToUInt64(hList.Count);
                        //if (objGeneratePatientListsDTO.Immunization == "-")
                        //    objGeneratePatientListsDTO.Immunization = string.Empty;
                        TestList.Add(objGeneratePatientListsDTO);
                    }
                }
                List<object> ids = new List<object>();
                IList<object> rList = new List<object>();



                #region Testresult Query


                if ((sLabResultCondition.Length > 0 && sLabResultCondition[0] != null) || (sCodition.Length > 0 && sCodition[0] != null))
                {
                    foreach (object[] oj in idList)
                    {
                        ids.Add(Convert.ToUInt64(oj[0]).ToString());
                    }
                    string RQuery = "Select c.human_id," +

                    "group_concat(distinct(concat(u.OBX_Observation_Text,'-',u.OBX_Observation_Value,' ',u.OBX_Units)) separator ',')as LabResult,cast(u.Created_Date_And_Time as char(100)) as Lab_Date" +
                     " from encounter c" +
                     " inner join human h on (h.human_id=c.human_id)" +
                     " inner join result_master e on (e.Matching_Patient_ID = c.human_id) " +
                     " inner join result_obx u on (u.Result_Master_ID = e.Result_Master_ID)" +
                     " where ";


                    if (!RQuery.ToUpper().EndsWith("WHERE "))
                        RQuery += " and " + " (";
                    else
                        RQuery += "(";
                    RQuery = constructQuery(sLabResultCondition, RQuery, 0);
                    if (sCodition[0] != null && Convert.ToString(sCodition[0]) != string.Empty)
                    {
                        RQuery += Convert.ToString(sCodition[0]);
                        RQuery = constructQuery(sLabResultCondition, RQuery, 1);
                    }
                    if (sCodition[1] != null && Convert.ToString(sCodition[1]) != string.Empty)
                    {
                        RQuery += Convert.ToString(sCodition[1]);
                        RQuery = constructQuery(sLabResultCondition, RQuery, 2);
                    }
                    if (sCodition[0] != null && Convert.ToString(sCodition[2]) != string.Empty)
                    {
                        RQuery += Convert.ToString(sCodition[2]);
                        RQuery = constructQuery(sLabResultCondition, RQuery, 3);
                    }
                    RQuery += " )";


                    RQuery += " and c.human_id in ('";
                    for (int i = 0; i < ids.Count; i++)
                    {
                        if (i + 1 == ids.Count)
                            RQuery += ids[i] + "') ";
                        else
                            RQuery += ids[i] + "','";
                    }
                    RQuery += " group by c.human_id ";


                    //if (pageNumber > 1)
                    //{
                    //    Query += "limit " + ((pageNumber - 1) * maxResults) + " , " + maxResults + "";
                    //}

                    rList = iMySession.CreateSQLQuery(RQuery).List<object>();
                    IList<GeneratePatientListsDTO> Lablist = new List<GeneratePatientListsDTO>();
                    if (rList.Count > 0)
                    {
                        foreach (object[] lresult in rList)
                        {
                            if (ids.Contains(lresult[0].ToString()))
                            {
                                ids.Remove(lresult[0].ToString());
                            }
                            Lablist = (from p in TestList where p.PatientAccountNo == Convert.ToUInt64(lresult[0]) select p).ToList();
                            if (Lablist.Count > 0)
                            {
                                Lablist[0].LabResult = Convert.ToString(lresult[1]);
                                if (lresult[2] == null)
                                {
                                    Lablist[0].LabResult_Date = Convert.ToString(lresult[2]);
                                }
                                else
                                {
                                    DateTime Lab_Date = Convert.ToDateTime(lresult[2]);
                                    Lablist[0].LabResult_Date = Lab_Date.ToString("dd-MMM-yyyy HH:mm:ss tt");
                                }
                                Lablist[0].Total_Record_Found = Convert.ToUInt64(rList.Count);
                            }
                        }
                        for (int idsCount = 0; idsCount < ids.Count; idsCount++)
                        {
                            Lablist = (from p in TestList where p.PatientAccountNo == Convert.ToUInt64(ids[idsCount]) select p).ToList();
                            if (Lablist.Count > 0)
                            {
                                TestList.Remove(Lablist[0]);
                            }
                        }


                    }
                    else
                    {
                        TestList.Clear();
                    }


                }
                #endregion




                Hashtable patientResult = new Hashtable();
                // patientResult.Add("GeneratedPatientList", PatientList);
                if ((sLabResultCondition.Length > 0 && sLabResultCondition[0] != null) || (sCodition.Length > 0 && sCodition[0] != null))
                {
                    patientResult.Add("GeneratedPatientList", TestList);
                }
                else
                {
                    patientResult.Add("GeneratedPatientList", TestList);
                }

                serializer.WriteObject(stream, patientResult);
                stream.Seek(0L, SeekOrigin.Begin);
                iMySession.Close();
            }
            return stream;
        }

        public string GetPatientNameByHumanID(ulong HumanID)
        {

            string PatientName = string.Empty;
            IList<Human> HumanList = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID));
                HumanList = crit.List<Human>();
                if (HumanList.Count > 0)
                {
                    PatientName = HumanList[0].First_Name + " " + HumanList[0].Last_Name + " " + HumanList[0].MI;
                }
                iMySession.Close();
            }
            return PatientName;
        }

        //added for perfomance tuning  * returns only Patient_Discussed,Discussed_By,ACO_Is_Eligible_Patient *by Jisha 
        public IList<string> GetACODetails(ulong HumanID)
        {
            IList<string> objHuman = new List<string>();
            ArrayList aryHuman = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("GetACODetails");
                query.SetParameter(0, HumanID);
                aryHuman = new ArrayList(query.List());
                for (int i = 0; i < aryHuman.Count; i++)
                {
                    object[] ob = (object[])aryHuman[i];
                    objHuman.Add(ob[0].ToString());
                    objHuman.Add(ob[1].ToString());
                    objHuman.Add(ob[2].ToString());
                    objHuman.Add(ob[3].ToString());
                    objHuman.Add(ob[4].ToString());
                    objHuman.Add(ob[5].ToString());
                    objHuman.Add(ob[6].ToString());
                }
                iMySession.Close();
            }
            return objHuman;
        }

        public IList<Human_Token> GetHumanFromTokens(string _Token, string account_status, string patient_status, string patient_type, out double DBTime, string legal_org, string user_carrier)
        {
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<Human_Token> lstMatchingHuman = new List<Human_Token>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (_Token.Trim() != string.Empty)
            {
                string sQuery = "select human_id,result,Patient_Status,account_status from human_token where ";
                if (patient_type != "REGULAR")
                {
                    if (patient_type != "ALL")
                    {
                        sQuery += "human_type ='" + patient_type + "' ";
                        sQuery += "and token like'" + _Token + "%' ";
                    }
                    else
                        sQuery += " token like'" + _Token + "%' ";

                    if (account_status == "ACTIVE")
                        sQuery += "and account_status ='ACTIVE' ";

                    if (patient_status == "ALIVE")
                        sQuery += "and patient_status ='ALIVE' ";
                }
                else if (patient_status != "ALIVE")
                {
                    sQuery += " token like'" + _Token + "%' ";
                    if (account_status == "ACTIVE")
                        sQuery += "and account_status ='ACTIVE' ";

                    sQuery += "and human_type ='REGULAR' ";
                }
                else if (account_status != "ACTIVE")
                {
                    sQuery += " token like'" + _Token + "%' ";
                    if (patient_status == "ALIVE")
                        sQuery += "and patient_status ='ALIVE' ";

                    sQuery += "and human_type ='REGULAR' ";
                }
                else
                    sQuery += "token like '" + _Token + "%' and account_status='active' and patient_status = 'alive' and human_type ='REGULAR' ";
                //Jira CAP-449 - Add group by human_id in the query
                if (user_carrier != "0")
                {
                    sQuery += " and legal_org ='" + legal_org + "' and Primary_Carrier_ID in (" + user_carrier + ") group by human_id  order by result";
                }
                else
                {
                    sQuery += " and legal_org ='" + legal_org + "' group by human_id  order by result";
                }

                ISQLQuery query = iMySession.CreateSQLQuery(sQuery);
                ArrayList arrPatients = new ArrayList(query.List());
                iMySession.Close();
                for (int iCount = 0; iCount < arrPatients.Count; iCount++)
                {
                    Human_Token tempHuman = new Human_Token();
                    object[] objHuman = (object[])arrPatients[iCount];
                    tempHuman.Human_ID = Convert.ToUInt32(objHuman[0].ToString());
                    tempHuman.Result = objHuman[1].ToString();
                    tempHuman.Patient_Status = objHuman[2].ToString();
                    tempHuman.Account_Status = objHuman[3].ToString();

                    lstMatchingHuman.Add(tempHuman);
                }
            }

            watch.Stop();
            DBTime = watch.Elapsed.TotalSeconds;
            return lstMatchingHuman;
        }

        public Human GetHumanFromHumanID(ulong HumanID)
        {
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            Human MatchingHuman = new Human();

            ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID));
            //CAP-267: Check whether human Id exists in db or not
            IList<Human> lstHuman = crit.List<Human>();
            MatchingHuman = lstHuman.Count > 0 ? lstHuman[0] : MatchingHuman;
            iMySession.Close();

            return MatchingHuman;
        }

        public FillQuickPatient GetStaticLookupandCarrier(string[] Field_Name)
        {
            FillQuickPatient FillQPDTO = new FillQuickPatient();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> staticList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(StaticLookup)).Add(Expression.In("Field_Name", Field_Name));
                FillQPDTO.StaticLookupList = criteria.List<StaticLookup>();


                ICriteria crit = iMySession.CreateCriteria(typeof(Carrier));
                FillQPDTO.CarrierList = crit.List<Carrier>();

                iMySession.Close();
            }
            return FillQPDTO;
        }

        public FillQuickPatient GetVisitPaymentDetails(string VisitID, string PPHeaderID, string PPLineItemID, string CheckID)
        {
            FillQuickPatient FillQPDTO = new FillQuickPatient();


            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                if (VisitID != string.Empty)
                {
                    ICriteria critVisitPayment = iMySession.CreateCriteria(typeof(VisitPayment)).Add(Expression.Eq("Id", Convert.ToUInt64(VisitID)));
                    FillQPDTO.VisitPaymentList = critVisitPayment.List<VisitPayment>();
                }

                if (PPHeaderID != string.Empty)
                {
                    ICriteria PPHeadercrit = iMySession.CreateCriteria(typeof(PPHeader)).Add(Expression.Eq("Id", Convert.ToUInt64(PPHeaderID)));
                    FillQPDTO.PPHeaderList = PPHeadercrit.List<PPHeader>();
                }

                if (PPLineItemID != string.Empty)
                {
                    ICriteria PPLineItemcrit = iMySession.CreateCriteria(typeof(PPLineItem)).Add(Expression.Eq("Id", Convert.ToUInt64(PPLineItemID)));
                    FillQPDTO.PPLineItemList = PPLineItemcrit.List<PPLineItem>();

                    IList<AccountTransaction> AccTranList = new List<AccountTransaction>();
                    ICriteria CritAccountTransaction = iMySession.CreateCriteria(typeof(AccountTransaction)).Add(Expression.Eq("Bill_To_PP_Line_Item_ID", Convert.ToUInt64(PPLineItemID)));

                    FillQPDTO.AccountTransaction = CritAccountTransaction.List<AccountTransaction>();
                }

                if (CheckID != string.Empty)
                {
                    ICriteria critCheckManager = iMySession.CreateCriteria(typeof(Check)).Add(Expression.Eq("Id", Convert.ToUInt64(CheckID)));
                    FillQPDTO.CheckList = critCheckManager.List<Check>();
                }

                if (VisitID != string.Empty)
                {
                    ISQLQuery sql = session.GetISession().CreateSQLQuery("select * from visit_payment_history where visit_payment_id='" + VisitID.ToString() + "' order by visit_payment_history_id desc limit 1").AddEntity(typeof(VisitPaymentHistory));
                    FillQPDTO.VisitPaymentHistoryList = sql.List<VisitPaymentHistory>();
                    //ICriteria critVisitPaymentHistory = iMySession.CreateCriteria(typeof(VisitPaymentHistory)).Add(Expression.Eq("Visit_Payment_ID", Convert.ToUInt64(VisitID))).AddOrder(Order.Desc("Id")).SetMaxResults(1);
                    //FillQPDTO.VisitPaymentHistoryList = critVisitPaymentHistory.List<VisitPaymentHistory>();
                }
                iMySession.Close();
            }

            return FillQPDTO;
        }

        public FillQuickPatientArc GetVisitPaymentDetailsArc(string VisitID, string PPHeaderID, string PPLineItemID, string CheckID)
        {
            FillQuickPatientArc FillQPDTO = new FillQuickPatientArc();


            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                if (VisitID != string.Empty)
                {
                    ICriteria critVisitPayment = iMySession.CreateCriteria(typeof(VisitPaymentArc)).Add(Expression.Eq("Id", Convert.ToUInt64(VisitID)));
                    FillQPDTO.VisitPaymentList = critVisitPayment.List<VisitPaymentArc>();
                }

                if (PPHeaderID != string.Empty)
                {
                    ICriteria PPHeadercrit = iMySession.CreateCriteria(typeof(PPHeaderArc)).Add(Expression.Eq("Id", Convert.ToUInt64(PPHeaderID)));
                    FillQPDTO.PPHeaderList = PPHeadercrit.List<PPHeaderArc>();
                }

                if (PPLineItemID != string.Empty)
                {
                    ICriteria PPLineItemcrit = iMySession.CreateCriteria(typeof(PPLineItemArc)).Add(Expression.Eq("Id", Convert.ToUInt64(PPLineItemID)));
                    FillQPDTO.PPLineItemList = PPLineItemcrit.List<PPLineItemArc>();

                    IList<AccountTransactionArc> AccTranList = new List<AccountTransactionArc>();
                    ICriteria CritAccountTransaction = iMySession.CreateCriteria(typeof(AccountTransactionArc)).Add(Expression.Eq("Bill_To_PP_Line_Item_ID", Convert.ToUInt64(PPLineItemID)));

                    FillQPDTO.AccountTransaction = CritAccountTransaction.List<AccountTransactionArc>();
                }

                if (CheckID != string.Empty)
                {
                    ICriteria critCheckManager = iMySession.CreateCriteria(typeof(CheckArc)).Add(Expression.Eq("Id", Convert.ToUInt64(CheckID)));
                    FillQPDTO.CheckList = critCheckManager.List<CheckArc>();
                }
                iMySession.Close();
            }

            return FillQPDTO;
        }


        //public IList<Encounter> GetAutoIncreamentVoucherNo(ulong Human)
        //{
        //    IList<Encounter> ilstEncounter = new List<Encounter>();
        //    ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    ISQLQuery sqlquery = iMySession.CreateSQLQuery("select * from charge_header e where e.voucher_no  like 'vn%' order by voucher_no desc").AddEntity("e", typeof(Encounter));
        //    ilstEncounter = sqlquery.List<Encounter>();
        //    return ilstEncounter;
        //}
        //public Human GetHumanFromHumanIDAndLastNameFirstName(ulong HumanID,string sFirstName, string sLastName)
        //{
        //    ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    Human MatchingHuman = new Human();

        //    ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanID)).Add(Expression.Eq("Last_Name", sLastName)).Add(Expression.Eq("First_Name", sFirstName));
        //    IList<Human> lstHuman = crit.List<Human>();
        //    MatchingHuman = lstHuman.Count > 0 ? lstHuman[0] : MatchingHuman;
        //    iMySession.Close();

        //    return MatchingHuman;
        //}
        public Human GetHumanFromHumanIDAndDOB(ulong HumanID, string sDOB)
        {
            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            Human MatchingHuman = new Human();
            ISQLQuery sql = iMySession.CreateSQLQuery("select * from human where human_id=" + HumanID.ToString() + " and Birth_Date ='" + sDOB + "';").AddEntity(typeof(Human));
            IList<Human> lstHuman = sql.List<Human>();
            MatchingHuman = lstHuman.Count > 0 ? lstHuman[0] : MatchingHuman;
            iMySession.Close();

            return MatchingHuman;
        }
        #endregion

        #endregion

        #region Set Methods
        public void SaveHumanPatientPortal(int id, string sPassword)
        {
            IList<Human> humanList = new List<Human>();
            IList<Human> humanListadd = null;
            Human hnRecord = GetById(Convert.ToUInt64(id));
            hnRecord.Password = sPassword;
            humanList.Add(hnRecord);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref humanListadd, ref humanList, null, string.Empty, true, false, Convert.ToUInt32(id), string.Empty);
        }

        public Human AppendToHuman(Human objHuman, string MACAddress)
        {
            IList<Human> humanList = new List<Human>();
            IList<Human> nullList = null;
            objHuman.Is_Sent_To_Rcopia = "Y";
            humanList.Add(objHuman);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref humanList, ref nullList, null, MACAddress, true, false, 0, string.Empty);
            if (humanList != null && humanList.Count != 0)
            {
                int iResult = 0;
                Human_TokenManager humanTokenMgn = new Human_TokenManager();
                IList<Human> ListToUpdateHuman = null;
                ISession MySession = NHibernateSessionManager.Instance.CreateISession();

                iResult = humanTokenMgn.SaveHuman_tokenWithoutTransaction(humanList, ListToUpdateHuman, MySession, string.Empty, string.Empty);
                MySession.Close();

            }
            //Latha - Branch_56 - Start - 23 Jul 2011
            if (humanList != null && humanList.Count > 0 && humanList[0].EMail.Trim() != string.Empty && humanList[0].Is_Mail_Sent.ToUpper() == "Y")
            {
                SendingMail(humanList[0]);
            }
            //Latha - Branch_56 - End - 23 Jul 2011
            //if (humanList[0].Id != 0)
            //{
            //    RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
            //    objRcopiaMngr.SendPatientToRCopia(humanList[0].Id, MACAddress);
            //}
            return humanList[0];
        }

        public Human UpdateToHuman(Human objHuman, string MACAddress)
        {
            IList<Human> humanList = new List<Human>();
            IList<Human> humanListadd = null;
            Human objhuman = null;
            objHuman.Is_Sent_To_Rcopia = "Y";
            humanList.Add(objHuman);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref humanListadd, ref humanList, null, MACAddress, true, false, objHuman.Id, string.Empty);
            //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            //{
            //    if (humanList != null && humanList.Count != 0)
            //    {
            //        int iResult = 0;
            //        Human_TokenManager humanTokenMgn = new Human_TokenManager();
            //        IList<Human> ListToUpdateHumanDummuy = null;                  

            //        iResult = humanTokenMgn.SaveHuman_tokenWithoutTransactionS(ListToUpdateHumanDummuy, humanList, string.Empty);


            //    }
            //    iMySession.Close();
            //}
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ICriteria crit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", objHuman.Id));
                //objhuman = crit.List<Human>()[0];

                //Latha - Branch_56 - Start - 23 Jul 2011
                if (humanList != null && humanList.Count > 0 && humanList[0].EMail.Trim() != string.Empty && humanList[0].Is_Mail_Sent.ToUpper() == "Y")
                {
                    SendingMail(humanList[0]);
                }
                //Latha - Branch_56 - End - 23 Jul 2011
                //if (humanList != null && humanList.Count > 0 && humanList[0].Id != 0)
                //{
                //    RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
                //    objRcopiaMngr.SendPatientToRCopia(humanList[0].Id, MACAddress);
                //}
                iMySession.Close();
            }
            return objhuman;
        }

        public void UpdateFailedRCopiaHuman(Human objHuman, string MACAddress)
        {
            IList<Human> humanList = new List<Human>();
            IList<Human> humanListadd = null;
            humanList.Add(objHuman);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref humanListadd, ref humanList, null, MACAddress, true, false, objHuman.Id, string.Empty);
        }

        //Latha - Branch_52_production_for_Rcopia - Start - 4 Jul 2011.End - 4 Jul 2011
        public ulong BatchOperationsToQuickPatient(IList<Human> ListToInsertHuman, IList<Human> ListToUpdateHuman, IList<Eligibility_Verification> ListToInserElig, IList<VisitPayment> ListToInsertVisitPayment, IList<Check> SaveCheckList, IList<PPHeader> SavePPHeaderList, IList<PPLineItem> SavePPLineItemList, IList<AccountTransaction> SaveAccountList, IList<PatientNotes> SavePatList, PatientInsuredPlan SavePatInsList, string MACAddress, ulong checkOut_Encounter_Id, IList<VisitPaymentHistory> SaveVisitPaymentHistoryList, bool bCheckInToMa = true)
        {
            iTryCount = 0;
            ulong HumanId = 0;
            GenerateXml XMLObj = new GenerateXml();
            bool bHumanConsistent = true;
        TryAgain:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = MySession.BeginTransaction();

                if (ListToInsertHuman != null || ListToUpdateHuman != null)
                {
                    if (ListToUpdateHuman.Count == 0)
                    {
                        ListToUpdateHuman = null;
                    }
                    else if (ListToInsertHuman != null && ListToInsertHuman.Count == 0)
                    {
                        ListToInsertHuman = null;
                    }
                    if (ListToInsertHuman != null)
                    {
                        for (int i = 0; i < ListToInsertHuman.Count; i++)
                        {
                            ListToInsertHuman[i].Is_Sent_To_Rcopia = "Y";
                        }
                    }
                    //if ((ListToInsertHuman != null) && ListToUpdateHuman == null)
                    //{
                    //    iResult = SaveUpdateDeleteWithoutTransaction(ref ListToInsertHuman, ListToUpdateHuman, null, MySession, MACAddress);
                    //}
                    //else if ((ListToInsertHuman != null) && ListToUpdateHuman != null)
                    //{
                    //    IList<Human> ListToInsert = null;
                    //    iResult = SaveUpdateDeleteWithoutTransaction(ref ListToInsert, ListToUpdateHuman, null, MySession, MACAddress);
                    //}

                    if (ListToInsertHuman != null && ListToInsertHuman.Count > 0 || ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                    {
                        //IList<Human> ListToInsert = null;
                        ulong Human_ID = 0;
                        if (ListToInsertHuman == null)
                            ListToInsertHuman = new List<Human>();
                        if (ListToUpdateHuman == null)
                            ListToUpdateHuman = new List<Human>();
                        if (ListToInsertHuman.Concat(ListToUpdateHuman).Count() > 0)
                            Human_ID = ListToInsertHuman.Concat(ListToUpdateHuman).First().Id;
                        iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ListToInsertHuman, ref ListToUpdateHuman, null, MySession, MACAddress, true, false, Human_ID, string.Empty, ref XMLObj);
                        bHumanConsistent = XMLObj.CheckDataConsistency(ListToInsertHuman.Concat(ListToUpdateHuman).Cast<object>().ToList(), true, string.Empty);
                    }

                    if (ListToInsertHuman != null && ListToInsertHuman.Count > 0)
                    {
                        HumanId = ListToInsertHuman[0].Id;
                    }
                    if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                    {
                        HumanId = ListToUpdateHuman[0].Id;
                    }
                    if (iResult == 2)
                    {
                        if (iTryCount < 5)
                        {
                            iTryCount++;
                            goto TryAgain;
                        }
                        else
                        {
                            trans.Rollback();
                            // MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }


                    else if (iResult != 1 && iResult != 2)//Vasanth
                    {
                        if (HumanId != 0)
                        {
                            string sCarrier = string.Empty;
                            PatientInsuredPlanManager patinsMngr = new PatientInsuredPlanManager();
                            Carrier objCarrier = new Carrier();
                            objCarrier = patinsMngr.GetActivePrimaryInsuranceNameByHumanId(HumanId);
                            if (objCarrier.Id != 0)
                            {
                                sCarrier = objCarrier.Carrier_Name;
                            }

                            Human_TokenManager humanTokenMgn = new Human_TokenManager();
                            iResult = humanTokenMgn.SaveHuman_tokenWithoutTransaction(ListToInsertHuman, ListToUpdateHuman, MySession, MACAddress, sCarrier);
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }
                    }


                    if (ListToInserElig != null && ListToInserElig.Count > 0)
                    {
                        if (ListToInsertHuman != null && ListToInsertHuman.Count > 0)
                        {
                            ListToInserElig[0].Human_ID = ListToInsertHuman[0].Id;
                        }
                        else if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                        {
                            ListToInserElig[0].Human_ID = ListToUpdateHuman[0].Id;
                        }
                    }
                    if (SavePatList != null && SavePatList.Count > 0)
                    {
                        if (ListToInsertHuman != null && ListToInsertHuman.Count > 0)
                        {
                            SavePatList[0].Human_ID = ListToInsertHuman[0].Id;
                        }
                        else if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
                        {
                            SavePatList[0].Human_ID = ListToUpdateHuman[0].Id;
                        }
                    }
                }

                if (ListToInserElig != null && ListToInserElig.Count > 0)
                {
                    Eligibility_VerficationManager ObjElig = new Eligibility_VerficationManager();
                    iResult = ObjElig.SaveEligWithoutTransaction(ListToInserElig, MySession, MACAddress);

                    if (iResult == 2)
                    {
                        if (iTryCount < 5)
                        {
                            iTryCount++;
                            goto TryAgain;
                        }
                        else
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (SavePatList != null && SavePatList.Count > 0)
                {
                    PatientNotesManager objPatNot = new PatientNotesManager();
                    iResult = objPatNot.SavePatientWithoutTransaction(SavePatList, MySession, MACAddress);

                    if (iResult == 2)
                    {
                        if (iTryCount < 5)
                        {
                            iTryCount++;
                            goto TryAgain;
                        }
                        else
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Deadlock occurred. Transaction failed.");
                        }
                    }
                    else if (iResult == 1)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception("Exception occurred. Transaction failed.");
                    }
                }
                if (ListToInsertVisitPayment != null && ListToInsertVisitPayment.Count > 0)
                {
                    //VisitPaymentManager ObjVisit = new VisitPaymentManager();
                    //iResult = ObjVisit.SaveVisitPaymentWithoutTransaction(ListToInsertVisitPayment, MySession, MACAddress);
                    if (ListToInsertVisitPayment != null && ListToInsertVisitPayment.Count > 0)
                    {
                        if (ListToInsertVisitPayment[0].Encounter_ID == 0 && ListToInsertVisitPayment[0].Voucher_No == 0)
                        {
                            IList<object> grpId = new List<object>();
                            //ICriteria crit = MySession.(select max(voucherno) from (select voucher_no as voucherno from visit_payment union select voucher_no as voucherno from visit_payment_arc)a );
                            IQuery query = MySession.CreateSQLQuery("select max(voucherno) from (select voucher_no as voucherno from visit_payment union select voucher_no as voucherno from visit_payment_arc)a");
                            grpId = query.List<object>();

                            if (grpId.Count > 0)
                            {
                                ListToInsertVisitPayment[0].Voucher_No = Convert.ToUInt64(grpId[0]) + 1;
                                ListToInsertVisitPayment[0].Batch_Status = "OPEN";
                            }
                            else
                            {
                                ListToInsertVisitPayment[0].Voucher_No = 10000000;
                            }

                            SaveVisitPaymentHistoryList[0].Voucher_No = ListToInsertVisitPayment[0].Voucher_No;
                        }
                        else
                        {
                            SaveVisitPaymentHistoryList[0].Voucher_No = ListToInsertVisitPayment[0].Voucher_No;
                        }
                    }
                    if (ListToInsertVisitPayment != null && ListToInsertVisitPayment.Count > 0)
                    {
                        IList<VisitPayment> VisitPaymentTemp = null;
                        VisitPaymentManager objVisitPaymentMngr = new VisitPaymentManager();
                        iResult = objVisitPaymentMngr.SaveVisitPaymentWithoutTransaction(ref ListToInsertVisitPayment, MySession, MACAddress);
                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {
                                trans.Rollback();
                                //  MySession.Close();
                                throw new Exception("Deadlock occurred. Transaction failed.");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }

                        VisitPaymentHistoryManager objVisitPaymentHistory = new VisitPaymentHistoryManager();
                        IList<VisitPaymentHistory> VisHisTemp = null;
                        if (SaveVisitPaymentHistoryList.Count > 0)
                            SaveVisitPaymentHistoryList[0].Visit_Payment_ID = ListToInsertVisitPayment[0].Id;

                        iResult = objVisitPaymentHistory.SaveVisitPaymentWithoutTransaction(SaveVisitPaymentHistoryList, MySession, MACAddress);

                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {

                                trans.Rollback();
                                //  MySession.Close();
                                throw new Exception("Deadlock is occured. Transaction failed");

                            }
                        }
                        else if (iResult == 1)
                        {

                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Exception is occured. Transaction failed");

                        }

                        CheckManager objCheck = new CheckManager();
                        iResult = objCheck.SaveCheckWithoutTransaction(SaveCheckList, MySession, MACAddress);

                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {
                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Deadlock occurred. Transaction failed.");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }

                        SavePPHeaderList[0].Payment_ID = SaveCheckList[0].Payment_ID;
                        SavePPHeaderList[0].Check_Table_Int_ID = SaveCheckList[0].Id;

                        PPHeaderManager objPPHeader = new PPHeaderManager();
                        iResult = objPPHeader.SavePPHeaderWithoutTransaction(SavePPHeaderList, MySession, MACAddress);

                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {
                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Deadlock occurred. Transaction failed.");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }

                        SavePPLineItemList[0].PP_Header_ID = SavePPHeaderList[0].Id;
                        SavePPLineItemList[0].Payment_ID = SaveCheckList[0].Payment_ID;
                        SavePPLineItemList[0].Check_Table_Int_ID = SaveCheckList[0].Id;
                        SavePPLineItemList[0].Visit_Payment_ID = ListToInsertVisitPayment[0].Id;
                        PPLineItemManager objPPLineItem = new PPLineItemManager();
                        iResult = objPPLineItem.SavePPLineItemWithoutTransaction(SavePPLineItemList, null, MySession, MACAddress);

                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {
                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Deadlock occurred. Transaction failed.");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            //MySession.Close();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }
                        if (SaveAccountList != null && SaveAccountList.Count > 0)
                        {
                            for (int iNumber = 0; iNumber < SaveAccountList.Count; iNumber++)
                            {
                                if (SavePPLineItemList != null && SavePPLineItemList.Count > 0)
                                    SaveAccountList[iNumber].Bill_To_PP_Line_Item_ID = SavePPLineItemList[0].Id;
                                if (SavePPHeaderList != null && SavePPHeaderList.Count > 0)
                                    SaveAccountList[iNumber].PP_Header_ID = SavePPHeaderList[0].Id;
                                if (SaveCheckList != null && SaveCheckList.Count > 0)
                                {
                                    SaveAccountList[iNumber].Payment_ID = SaveCheckList[0].Payment_ID;
                                    SaveAccountList[iNumber].Check_Table_Int_ID = SaveCheckList[0].Id;
                                }
                            }

                            AccountTransactionManager objAccount = new AccountTransactionManager();
                            iResult = objAccount.SaveAccountWithoutTransaction(SaveAccountList, MySession, MACAddress);

                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    //MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }
                    }
                    if (SavePatInsList != null)
                    {
                        PatientInsuredPlanManager objPatInsured = new PatientInsuredPlanManager();
                        iResult = objPatInsured.addPatInsuredPlan(SavePatInsList, MySession, MACAddress);

                        if (iResult == 2)
                        {
                            if (iTryCount < 5)
                            {
                                iTryCount++;
                                goto TryAgain;
                            }
                            else
                            {
                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Deadlock occurred. Transaction failed.");
                            }
                        }
                        else if (iResult == 1)
                        {
                            trans.Rollback();
                            // MySession.Close();
                            throw new Exception("Exception occurred. Transaction failed.");
                        }
                    }
                }
                //MySession.Flush();
                if (bHumanConsistent)
                {
                    
                    if (XMLObj.itemDoc.InnerXml != "")
                    {
                       // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                        //int trycount = 0;
                    //trytosaveagain:
                        try
                        {
                            // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            if (ListToInsertHuman.Count>0)
                            {
                                WriteBlob(ListToInsertHuman[0].Id, XMLObj.itemDoc, MySession, ListToInsertHuman, null, null, XMLObj, true);
                            }
                            else if (ListToUpdateHuman.Count > 0)
                            {
                                WriteBlob(ListToUpdateHuman[0].Id, XMLObj.itemDoc, MySession, null, ListToUpdateHuman, null, XMLObj, true);
                            }
                        }
                        catch (Exception xmlexcep)
                        {
                            throw new Exception(xmlexcep.Message);
                            //trycount++;
                            //if (trycount <= 3)
                            //{
                            //    int TimeMilliseconds = 0;
                            //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                            //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                            //    Thread.Sleep(TimeMilliseconds);
                            //    string sMsg = string.Empty;
                            //    string sExStackTrace = string.Empty;

                            //    string version = "";
                            //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                            //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                            //    string[] server = version.Split('|');
                            //    string serverno = "";
                            //    if (server.Length > 1)
                            //        serverno = server[1].Trim();

                            //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                            //        sMsg = xmlexcep.InnerException.Message;
                            //    else
                            //        sMsg = xmlexcep.Message;

                            //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                            //        sExStackTrace = xmlexcep.StackTrace;

                            //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            //    string ConnectionData;
                            //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                            //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                            //    {
                            //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                            //        {
                            //            cmd.Connection = con;
                            //            try
                            //            {
                            //                con.Open();
                            //                cmd.ExecuteNonQuery();
                            //                con.Close();
                            //            }
                            //            catch
                            //            {
                            //            }
                            //        }
                            //    }
                            //    goto trytosaveagain;
                            //}
                        }
                    }
                    trans.Commit();
                }
                else
                    throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");


                //if (HumanId != 0)
                //{
                //    RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
                //    objRcopiaMngr.SendPatientToRCopia(HumanId, MACAddress);
                //}

            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                trans.Rollback();
                // MySession.Close();
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }
            catch (Exception e)
            {
                trans.Rollback();
                //MySession.Close();
                //CAP-1942
                throw new Exception(e.Message,e);
            }
            finally
            {
                MySession.Close();
            }


            if (checkOut_Encounter_Id != 0)
            {
                WFObjectManager WFMngr = new WFObjectManager();
                WFObject TempWF = new WFObject();
                EncounterManager encounterManager = new EncounterManager();
                TempWF = WFMngr.GetByObjectSystemId(checkOut_Encounter_Id, "ENCOUNTER");
                //CAP-1998
                string sOwner = string.Empty;
                if (!bCheckInToMa)
                {
                    
                    IList<Encounter> lstEncounter = encounterManager.GetEncounterByEncounterID(checkOut_Encounter_Id);
                    if(lstEncounter.Count > 0)
                    {
                        UserManager userManager = new UserManager();
                        IList<User> lstUser = userManager.getUserByPHYID(Convert.ToUInt64(lstEncounter[0].Appointment_Provider_ID));
                        if(lstUser.Count > 0)
                        {
                            sOwner = lstUser[0].user_name;
                        }
                    }
                }
                //Added by Selvaraman - 20 Aug - To avoid Duplicate Check In
                if (TempWF.Current_Process == "SCHEDULED" || TempWF.Current_Process == "NO_SHOW" || TempWF.Current_Process == "WALKED_AWAY")
                {
                    ISession MyDocumentationWorkflowSession = Session.GetISession();

                    ITransaction transDocumentation = null;
                    try
                    {
                        transDocumentation = MyDocumentationWorkflowSession.BeginTransaction();

                        WFObject objDocumentationWF = new WFObject();
                        objDocumentationWF.Fac_Name = TempWF.Fac_Name;
                        objDocumentationWF.Obj_Type = "DOCUMENTATION";
                        objDocumentationWF.Parent_Obj_Type = "ENCOUNTER";
                        objDocumentationWF.Parent_Obj_System_Id = TempWF.Obj_System_Id;
                        objDocumentationWF.Obj_System_Id = TempWF.Obj_System_Id;
                        objDocumentationWF.Current_Process = "START";
                        objDocumentationWF.Current_Arrival_Time = TempWF.Current_Arrival_Time;
                        //objDocumentationWF.Current_Owner = "UNKNOWN";
                        //CAP-1998
                        if (bCheckInToMa)
                        {
                            objDocumentationWF.Current_Owner = "UNKNOWN";
                        }
                        else
                        {
                            objDocumentationWF.Current_Owner = sOwner;
                        }
                        if (bCheckInToMa)
                        {
                            WFMngr.InsertToWorkFlowObject(objDocumentationWF, 1, MACAddress, MyDocumentationWorkflowSession);
                        }
                        else
                        {
                            WFMngr.InsertToWorkFlowObject(objDocumentationWF, 2, MACAddress, MyDocumentationWorkflowSession);
                        }

                        transDocumentation.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        transDocumentation.Rollback();
                        // MySession.Close();
                        //CAP-1942
                        throw new Exception(ex.Message,ex);
                    }
                    catch (Exception e)
                    {
                        transDocumentation.Rollback();
                        //MySession.Close();
                        //CAP-1942
                        throw new Exception(e.Message,e);
                    }
                    finally
                    {
                        MyDocumentationWorkflowSession.Close();
                    }

                    ISession MyBillingWorkflowSession = Session.GetISession();

                    ITransaction transBilling = null;
                    try
                    {
                        transBilling = MyBillingWorkflowSession.BeginTransaction();

                        WFObject objBillingWF = new WFObject();
                        objBillingWF.Fac_Name = TempWF.Fac_Name;
                        objBillingWF.Obj_Type = "BILLING";
                        objBillingWF.Parent_Obj_Type = "ENCOUNTER";
                        objBillingWF.Parent_Obj_System_Id = TempWF.Obj_System_Id;
                        objBillingWF.Obj_System_Id = TempWF.Obj_System_Id;
                        objBillingWF.Current_Process = "START";
                        objBillingWF.Current_Arrival_Time = TempWF.Current_Arrival_Time;
                        objBillingWF.Current_Owner = "UNKNOWN";

                        WFMngr.InsertToWorkFlowObject(objBillingWF, 1, MACAddress, MyBillingWorkflowSession);

                        transBilling.Commit();
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        transBilling.Rollback();
                        // MySession.Close();
                        //CAP-1942
                        throw new Exception(ex.Message,ex);
                    }
                    catch (Exception e)
                    {
                        transBilling.Rollback();
                        //MySession.Close();
                        //CAP-1942
                        throw new Exception(e.Message,e);
                    }
                    finally
                    {
                        MyBillingWorkflowSession.Close();
                    }

                    WFMngr.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 1, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), MACAddress, null, null);
                }

                //Added by Manimozhi - 21st May 2012 - Bug ID 8927
                //WFMngr.UpdateOwner(TempWF.Obj_System_Id, TempWF.Obj_Type, "UNKNOWN", MACAddress);

                if (TempWF.Current_Process == "NO_SHOW" || TempWF.Current_Process == "WALKED_AWAY")
                {
                    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                    {
                        IList<Encounter> objEncounterupdatelst = new List<Encounter>();
                        EncounterManager objencountermanager = new EncounterManager();
                        IList<Encounter> objEncountersavelst = null;
                        IList<Encounter> objEncounterdeletelst = null;
                        ICriteria criteriaBycheckOut_Encounter_Id = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", checkOut_Encounter_Id));
                        objEncounterupdatelst = criteriaBycheckOut_Encounter_Id.List<Encounter>();

                        objEncounterupdatelst[0].Is_Batch_Created = "N";
                        objEncounterupdatelst[0].Is_EandM_Submitted = "N";

                        objencountermanager.SaveUpdateDelete_DBAndXML_WithTransaction(ref objEncountersavelst, ref objEncounterupdatelst, objEncounterdeletelst, MACAddress, false, false, 0, "");
                    }
                }
            }
            return HumanId;
        }

        //Will Never be used as per Selva. Hence SaveUpdateDelete_DBANDXML functionality not implemented properly. If it has to implemented in the future, include bIsRCopia boolian variable as a parameter in ManagerBase method and set to true.
        public void InsertOrUpdateHumanByRcopia(IList<Human> ilsthuman, string sMacAddress)
        {
            if (ilsthuman.Count > 0)
            {
                IList<Human> Rcopia_HumanList = new List<Human>();
                IList<Human> Rcopia_HumanUpdateList = new List<Human>();
                Human objhuman = null;

                for (int i = 0; i < ilsthuman.Count; i++)
                {
                    if (ilsthuman[i].Id != 0)
                    {
                        IList<Human> ilstReturnhuman = GetPatientDetailsUsingPatientInformattion(ilsthuman[i].Id);
                        if (ilstReturnhuman.Count > 0)
                            objhuman = ilstReturnhuman[0];
                        if (objhuman == null)
                        {
                            Rcopia_HumanList.Add(ilsthuman[i]);
                        }
                        else
                        {
                            Human objUpdateHuman = FillUpdateObj(objhuman, ilsthuman[i]);
                            Rcopia_HumanUpdateList.Add(objUpdateHuman);
                        }
                    }
                }
                SaveUpdateDelete_DBAndXML_WithTransaction(ref Rcopia_HumanList, ref Rcopia_HumanUpdateList, null, sMacAddress, true, false, Rcopia_HumanList.Concat(Rcopia_HumanUpdateList).First().Id, string.Empty);
            }
        }

        //yet to modify
        public void UpdateHumanHomePhoneNumber(ulong HumanId, string HumanPhoneNumber, ISession Mysession, string MACAddress)
        {
            IList<Human> humanlist = new List<Human>();
            IList<Human> Savehumanlist = new List<Human>();
            using (ISession imySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crt = imySession.CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanId));
                humanlist = crt.List<Human>();
                foreach (Human obj in humanlist)
                {
                    if (obj.Account_Status.ToUpper() == "ACTIVE")
                        obj.Home_Phone_No = HumanPhoneNumber;
                }

                SaveUpdateDeleteWithoutTransaction(ref Savehumanlist, humanlist, null, Mysession, MACAddress);
                imySession.Close();
            }
        }
        
        public Human AppendBatchToHuman(Human objHuman, PatGuarantor objPatGuarantor, string sCarrier)//Thamizh
        {
            iTryCount = 0;
            ulong HumanId = 0;

        TryAgain:
            int iResult = 0;
            GenerateXml XMLObj = new GenerateXml();
            bool bHumanConsistent = true;
            ISession MySession = Session.GetISession();
            //ITransaction trans = null;
            //IList<BatchHumanMap> BatchListAdd = new List<BatchHumanMap>();
            IList<Human> HumanListAdd = new List<Human>();
            IList<PatGuarantor> PatGuarantorListAdd = new List<PatGuarantor>();
            objHuman.Is_Sent_To_Rcopia = "Y";
            HumanListAdd.Add(objHuman);
            //if (objBatchHuman != null)
            //{
            //    BatchListAdd.Add(objBatchHuman);

            //}
            if (objPatGuarantor != null)
            {
                PatGuarantorListAdd.Add(objPatGuarantor);

            }
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        //trans = MySession.BeginTransaction();

                        if (HumanListAdd != null || HumanListAdd.Count > 0)
                        {
                            IList<Human> nullList = null;
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref HumanListAdd, ref nullList, null, MySession, string.Empty, true, false, 0, string.Empty, ref XMLObj);
                            bHumanConsistent = XMLObj.CheckDataConsistency(HumanListAdd.Cast<object>().ToList(), false, string.Empty);
                            //if (BatchListAdd != null && BatchListAdd.Count > 0)
                            //{
                            //    BatchListAdd[0].Human_ID = HumanListAdd[0].Id;
                            //}
                            if (HumanListAdd != null && HumanListAdd.Count > 0 && HumanListAdd[0].EMail.Trim() != string.Empty && HumanListAdd[0].Is_Mail_Sent.ToUpper() == "Y")
                            {
                                SendingMail(HumanListAdd[0]);
                            }
                            if (HumanListAdd[0].Id != 0)
                            {
                                HumanId = HumanListAdd[0].Id;

                            }
                            if (PatGuarantorListAdd != null && PatGuarantorListAdd.Count > 0)
                            {
                                if (PatGuarantorListAdd[0].Guarantor_Human_ID == 0)
                                {
                                    PatGuarantorListAdd[0].Guarantor_Human_ID = Convert.ToInt32(HumanListAdd[0].Id);
                                }
                                PatGuarantorListAdd[0].Human_ID = Convert.ToInt32(HumanListAdd[0].Id);
                            }
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                            else if (iResult != 1 && iResult != 2)
                            {
                                if (HumanId != 0)
                                {

                                    Human_TokenManager humanTokenMgn = new Human_TokenManager();
                                    IList<Human> ListToUpdateHuman = null;
                                    iResult = humanTokenMgn.SaveHuman_tokenWithoutTransaction(HumanListAdd, ListToUpdateHuman, MySession, string.Empty, sCarrier);
                                    if (iResult == 2)
                                    {
                                        if (iTryCount < 5)
                                        {
                                            iTryCount++;
                                            goto TryAgain;
                                        }
                                        else
                                        {
                                            trans.Rollback();
                                            // MySession.Close();
                                            throw new Exception("Deadlock occurred. Transaction failed.");
                                        }
                                    }
                                    else if (iResult == 1)
                                    {
                                        trans.Rollback();
                                        // MySession.Close();
                                        throw new Exception("Exception occurred. Transaction failed.");
                                    }
                                }
                            }

                        }

                        //if (BatchListAdd != null && BatchListAdd.Count > 0)
                        //{

                        //    BatchHumanMapManager BatchHumanMngr = new BatchHumanMapManager();
                        //    iResult = BatchHumanMngr.SaveBatchWithoutTransaction(BatchListAdd, MySession, string.Empty);

                        //    if (iResult == 2)
                        //    {
                        //        if (iTryCount < 5)
                        //        {
                        //            iTryCount++;
                        //            goto TryAgain;
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            //MySession.Close();
                        //            throw new Exception("Deadlock occurred. Transaction failed.");
                        //        }
                        //    }
                        //    else if (iResult == 1)
                        //    {
                        //        trans.Rollback();
                        //        //MySession.Close();
                        //        throw new Exception("Exception occurred. Transaction failed.");
                        //    }
                        //}
                        if (PatGuarantorListAdd != null && PatGuarantorListAdd.Count > 0)
                        {

                            PatGuarantorManager PatGuarantorMngr = new PatGuarantorManager();
                            iResult = PatGuarantorMngr.SaveBatchWithoutTransaction(PatGuarantorListAdd, MySession, string.Empty);

                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    //MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }
                        //MySession.Flush();
                        if (bHumanConsistent)
                        {
                           // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);

                        //    int trycount = 0;
                        //trytosaveagain:
                            try
                            {
                               //Commented By Deepak
                               // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                if (HumanListAdd.Count > 0)
                                {
                                    WriteBlob(HumanListAdd[0].Id, XMLObj.itemDoc, MySession, HumanListAdd, null, null, XMLObj, true);
                                }
                                
                            }
                            catch (Exception xmlexcep)
                            {
                                throw new Exception(xmlexcep.Message.ToString());
                                //trycount++;
                                //if (trycount <= 3)
                                //{
                                //    int TimeMilliseconds = 0;
                                //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                //    Thread.Sleep(TimeMilliseconds);
                                //    string sMsg = string.Empty;
                                //    string sExStackTrace = string.Empty;

                                //    string version = "";
                                //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                //    string[] server = version.Split('|');
                                //    string serverno = "";
                                //    if (server.Length > 1)
                                //        serverno = server[1].Trim();

                                //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                //        sMsg = xmlexcep.InnerException.Message;
                                //    else
                                //        sMsg = xmlexcep.Message;

                                //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                                //        sExStackTrace = xmlexcep.StackTrace;

                                //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                //    string ConnectionData;
                                //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                //    {
                                //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                //        {
                                //            cmd.Connection = con;
                                //            try
                                //            {
                                //                con.Open();
                                //                cmd.ExecuteNonQuery();
                                //                con.Close();
                                //            }
                                //            catch
                                //            {
                                //            }
                                //        }
                                //    }
                                //    goto trytosaveagain;
                                //}
                            }

                            trans.Commit();
                        }
                        else
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        //CAP-1942
                        throw new Exception(ex.Message,ex);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        //CAP-1942
                        throw new Exception(e.Message,e);
                    }
                    finally
                    {
                        MySession.Close();
                    }
                }
            }
            catch (Exception ex1)
            {
                //MySession.Close();
                //CAP-1942
                throw new Exception(ex1.Message,ex1);
            }
            if (HumanId != 0)
            {
                RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
                //CAP-676: Uncomment SendPatientToRCopia method
                objRcopiaMngr.SendPatientToRCopia(HumanId, string.Empty, objHuman.Legal_Org);
            }
            return HumanListAdd[0];



            //IList<Human> humanList = new List<Human>();
            //humanList.Add(objHuman);
            //SaveUpdateDeleteWithTransaction(ref humanList, null, null, string.Empty);

            //if (objBatchHuman != null)
            //{
            //    if (humanList[0].Id != 0)
            //        objBatchHuman.Human_ID = humanList[0].Id;
            //    BatchHumanMapManager BatchHumanMngr = new BatchHumanMapManager();
            //    BatchHumanMngr.SaveBatchHumanDetails(objBatchHuman);
            //}
            //return humanList[0];
        }

        public Human UpdateBatchToHuman(Human objHuman, PatGuarantor objPatGuarantor, PatientInsuredPlan objPatInsPlan, string sPriCarrier)
        {
            IList<Human> HumanListAdd = null;
            IList<Human> HumanUpdateList = new List<Human>();
            IList<PatientInsuredPlan> PatinsUpdateList = new List<PatientInsuredPlan>();
            // IList<PatientInsuredPlan> PatinsAddList = null;
            iTryCount = 0;
            ulong HumanId = 0;

        TryAgain:
            int iResult = 0;
            GenerateXml XMLObj = new GenerateXml();
            bool bHumanConsistent = true;
            ISession MySession = Session.GetISession();
            //ITransaction trans = null;
            //IList<BatchHumanMap> BatchListUpdate = new List<BatchHumanMap>();
            if (objHuman != null)
            {
                objHuman.Is_Sent_To_Rcopia = "Y";
                HumanUpdateList.Add(objHuman);
            }
            //if (objBatchHuman != null)
            //{
            //    BatchListUpdate.Add(objBatchHuman);
            //}
            if (objPatInsPlan != null)
            {
                PatinsUpdateList.Add(objPatInsPlan);
            }
            IList<PatGuarantor> PatGuarantorUpdateList = new List<PatGuarantor>();
            if (objPatGuarantor != null)
            {
                PatGuarantorUpdateList.Add(objPatGuarantor);
            }
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        // trans = MySession.BeginTransaction();

                        if (HumanUpdateList != null || HumanUpdateList.Count > 0)
                        {

                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref HumanListAdd, ref HumanUpdateList, null, MySession, string.Empty, true, false, HumanUpdateList[0].Id, string.Empty, ref XMLObj);

                            //if (BatchListUpdate != null && BatchListUpdate.Count > 0)
                            //{
                            //    BatchListUpdate[0].Human_ID = HumanUpdateList[0].Id;
                            //}
                            if (objHuman != null && objHuman.EMail.Trim() != string.Empty && objHuman.Is_Mail_Sent.ToUpper() == "Y")
                            {
                                SendingMail(objHuman);
                            }
                            HumanId = HumanUpdateList[0].Id;
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                            else if (iResult != 1 && iResult != 2)
                            {
                                if (HumanId != 0)
                                {

                                    Human_TokenManager humanTokenMgn = new Human_TokenManager();
                                    IList<Human> ListToUpdateHuman = null;
                                    iResult = humanTokenMgn.SaveHuman_tokenWithoutTransaction(ListToUpdateHuman, HumanUpdateList, MySession, string.Empty, sPriCarrier);
                                    if (iResult == 2)
                                    {
                                        if (iTryCount < 5)
                                        {
                                            iTryCount++;
                                            goto TryAgain;
                                        }
                                        else
                                        {
                                            trans.Rollback();
                                            // MySession.Close();
                                            throw new Exception("Deadlock occurred. Transaction failed.");
                                        }
                                    }
                                    else if (iResult == 1)
                                    {
                                        trans.Rollback();
                                        // MySession.Close();
                                        throw new Exception("Exception occurred. Transaction failed.");
                                    }
                                }
                            }

                        }
                        #region Commented for bugid 22806
                        //if (PatinsUpdateList != null && PatinsUpdateList.Count > 0)
                        //{
                        //    PatientInsuredPlanManager PatInsMngr = new PatientInsuredPlanManager();
                        //    iResult = PatInsMngr.SaveUpdateDeleteWithoutTransaction(ref PatinsAddList, PatinsUpdateList, null, MySession, string.Empty);
                        //    if (iResult == 2)
                        //    {
                        //        if (iTryCount < 5)
                        //        {
                        //            iTryCount++;
                        //            goto TryAgain;
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            // MySession.Close();
                        //            throw new Exception("Deadlock occurred. Transaction failed.");
                        //        }
                        //    }
                        //    else if (iResult == 1)
                        //    {
                        //        trans.Rollback();
                        //        // MySession.Close();
                        //        throw new Exception("Exception occurred. Transaction failed.");
                        //    }
                        //}
                        #endregion
                        //if (BatchListUpdate != null && BatchListUpdate.Count > 0)
                        //{

                        //    BatchHumanMapManager BatchHumanMngr = new BatchHumanMapManager();
                        //    iResult = BatchHumanMngr.SaveBatchWithoutTransaction(BatchListUpdate, MySession, string.Empty);

                        //    if (iResult == 2)
                        //    {
                        //        if (iTryCount < 5)
                        //        {
                        //            iTryCount++;
                        //            goto TryAgain;
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            //MySession.Close();
                        //            throw new Exception("Deadlock occurred. Transaction failed.");
                        //        }
                        //    }
                        //    else if (iResult == 1)
                        //    {
                        //        trans.Rollback();
                        //        //MySession.Close();
                        //        throw new Exception("Exception occurred. Transaction failed.");
                        //    }
                        //}
                        if (PatGuarantorUpdateList != null && PatGuarantorUpdateList.Count > 0)
                        {

                            PatGuarantorManager PatGuarantorMngr = new PatGuarantorManager();
                            iResult = PatGuarantorMngr.SaveBatchWithoutTransaction(PatGuarantorUpdateList, MySession, string.Empty);

                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    //MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                //MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                        }
                        //MySession.Flush();
                        if (bHumanConsistent)
                        {
                            
                           // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);

                            int trycount = 0;
                        trytosaveagain:
                            try
                            {
                                //Commented By Deepak
                                //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                if (HumanUpdateList.Count > 0)
                                {
                                    WriteBlob(HumanUpdateList[0].Id, XMLObj.itemDoc, MySession, null, HumanUpdateList, null, XMLObj, false);
                                }
                                
                            }
                            catch (Exception xmlexcep)
                            {
                                throw new Exception(xmlexcep.Message.ToString());
                                //trycount++;
                                //if (trycount <= 3)
                                //{
                                //    int TimeMilliseconds = 0;
                                //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                //    Thread.Sleep(TimeMilliseconds);
                                //    string sMsg = string.Empty;
                                //    string sExStackTrace = string.Empty;

                                //    string version = "";
                                //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                //    string[] server = version.Split('|');
                                //    string serverno = "";
                                //    if (server.Length > 1)
                                //        serverno = server[1].Trim();

                                //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                //        sMsg = xmlexcep.InnerException.Message;
                                //    else
                                //        sMsg = xmlexcep.Message;

                                //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                                //        sExStackTrace = xmlexcep.StackTrace;

                                //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                //    string ConnectionData;
                                //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                //    {
                                //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                //        {
                                //            cmd.Connection = con;
                                //            try
                                //            {
                                //                con.Open();
                                //                cmd.ExecuteNonQuery();
                                //                con.Close();
                                //            }
                                //            catch
                                //            {
                                //            }
                                //        }
                                //    }
                                //    goto trytosaveagain;
                                //}
                            }
                            trans.Commit();

                        }
                        else
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        //CAP-1942
                        throw new Exception(ex.Message,ex);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        //CAP-1942
                        throw new Exception(e.Message,e);
                    }
                    finally
                    {
                        MySession.Close();
                    }
                }
            }
            catch (Exception ex1)
            {
                //MySession.Close();
                //CAP-1942
                throw new Exception(ex1.Message,ex1);
            }
            if (HumanId != 0)
            {
                RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
                //CAP-676: Uncomment SendPatientToRCopia method
                objRcopiaMngr.SendPatientToRCopia(HumanId, string.Empty, objHuman.Legal_Org);
            }
            return HumanUpdateList[0];


            //HumanUpdateList.Add(objHuman);
            //SaveUpdateDeleteWithTransaction(ref HumanListAdd, HumanUpdateList, null, string.Empty);

            //if (objBatchHuman != null)
            //{
            //    BatchHumanMapManager BatchHumanMngr = new BatchHumanMapManager();
            //    BatchHumanMngr.UpdateBatchHumanDetais(objBatchHuman);
            //}
            //return HumanUpdateList[0];
        }

        public void UpdateACOQueries(ulong HumanId, string IsPatientAccepted, string IsPatientDiscussed, ulong IsDiscussedBy, ulong Encounter_id, bool flag, string Source_info, string UserName, DateTime dt)
        {
            Human objHuman = new Human();
            objHuman = GetById(HumanId);
            if (objHuman != null)
            {
                objHuman.Patient_Accepted = IsPatientAccepted.ToString();
                objHuman.Patient_Discussed = IsPatientDiscussed.ToString();
                objHuman.Discussed_By = IsDiscussedBy;
                IList<Human> saveList = new List<Human>();
                IList<Human> updateList = new List<Human>();
                updateList.Add(objHuman);

                SaveUpdateDelete_DBAndXML_WithTransaction(ref saveList, ref updateList, null, string.Empty, true, false, objHuman.Id, string.Empty);
            }
            if (flag)
            {
                EncounterManager objEncounterManager = new EncounterManager();
                IList<Encounter> EncList = objEncounterManager.GetEncounterByEncounterID(Encounter_id);
                if (EncList != null && EncList.Count > 0)
                {
                    Encounter currentEncounter = new Encounter();
                    currentEncounter = EncList[0];
                    currentEncounter.Is_PFSH_Verified = "Y";
                    currentEncounter.Modified_By = UserName;
                    currentEncounter.Modified_Date_and_Time = dt;
                    if (Source_info != string.Empty)
                    {
                        currentEncounter.Source_Of_Information = Source_info.ToString();
                    }
                    objEncounterManager.UpdateEncounter(currentEncounter, string.Empty, new object[] { "false" });
                }

            }
        }

        public void UpdateHumanEmailDetails(IList<Human> UpdatehumanList, string MACAddress)
        {
            IList<Human> humanaddList = null;
            if (UpdatehumanList.Count > 0)
                SaveUpdateDelete_DBAndXML_WithTransaction(ref humanaddList, ref UpdatehumanList, null, MACAddress, true, false, UpdatehumanList[0].Id, string.Empty);
        }

        public ulong SaveHumanforSummary(IList<Human> lsthuman)
        {
            lsthuman[0].Is_Sent_To_Rcopia = "Y";
            IList<Human> nullList = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lsthuman, ref nullList, null, string.Empty, true, false, 0, string.Empty);
            //RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
            //objRcopiaMngr.SendPatientToRCopia(lsthuman[0].Id, string.Empty);
            return Convert.ToUInt64(lsthuman[0].Id);
        }
        //bugId:45349
        public ulong AppendHumanFromImport(Human objHuman, string sLegalOrg)//Thamizh
        {
            iTryCount = 0;
            ulong HumanId = 0;
        TryAgain:
            int iResult = 0;
            GenerateXml XMLObj = new GenerateXml();
            ISession MySession = Session.GetISession();
            IList<Human> HumanListAdd = new List<Human>();
            objHuman.Is_Sent_To_Rcopia = "Y";
            HumanListAdd.Add(objHuman);
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        //trans = MySession.BeginTransaction();

                        if (HumanListAdd != null || HumanListAdd.Count > 0)
                        {
                            IList<Human> nullList = null;
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref HumanListAdd, ref nullList, null, MySession, string.Empty, false, false, 0, string.Empty, ref XMLObj);

                            if (HumanListAdd != null && HumanListAdd.Count > 0 && HumanListAdd[0].EMail.Trim() != string.Empty && HumanListAdd[0].Is_Mail_Sent.ToUpper() == "Y")
                            {
                                SendingMail(HumanListAdd[0]);
                            }
                            if (HumanListAdd[0].Id != 0)
                            {
                                HumanId = HumanListAdd[0].Id;

                            }
                            if (iResult == 2)
                            {
                                if (iTryCount < 5)
                                {
                                    iTryCount++;
                                    goto TryAgain;
                                }
                                else
                                {
                                    trans.Rollback();
                                    // MySession.Close();
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                // MySession.Close();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                            else if (iResult != 1 && iResult != 2)
                            {
                                if (HumanId != 0)
                                {
                                    string sCarrier = string.Empty;
                                    PatientInsuredPlanManager patinsMngr = new PatientInsuredPlanManager();
                                    Carrier objCarrier = new Carrier();
                                    objCarrier = patinsMngr.GetActivePrimaryInsuranceNameByHumanId(HumanId);
                                    if (objCarrier.Id != 0)
                                    {
                                        sCarrier = objCarrier.Carrier_Name;
                                    }
                                    Human_TokenManager humanTokenMgn = new Human_TokenManager();
                                    IList<Human> ListToUpdateHuman = null;
                                    iResult = humanTokenMgn.SaveHuman_tokenWithoutTransaction(HumanListAdd, ListToUpdateHuman, MySession, string.Empty, sCarrier);
                                    if (iResult == 2)
                                    {
                                        if (iTryCount < 5)
                                        {
                                            iTryCount++;
                                            goto TryAgain;
                                        }
                                        else
                                        {
                                            trans.Rollback();
                                            // MySession.Close();
                                            throw new Exception("Deadlock occurred. Transaction failed.");
                                        }
                                    }
                                    else if (iResult == 1)
                                    {
                                        trans.Rollback();
                                        // MySession.Close();
                                        throw new Exception("Exception occurred. Transaction failed.");
                                    }
                                }
                            }

                        }

                        trans.Commit();

                        MySession.Close();

                        /*Generating Human XML*/
                        GenerateXml objXML = new GenerateXml();
                        if (objHuman.Id != 0)
                        {
                            //string HumanFileName = "Human" + "_" + objHuman.Id + ".xml";
                            //string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                            //if (File.Exists(strXmlHumanFilePath) == false && objHuman.Id > 0)
                            //{
                            HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                            Human_Blob objHumanblob = new Human_Blob();
                            IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(objHuman.Id);
                            if (ilstHumanBlob.Count == 0 && objHuman.Id > 0)
                            {
                                string sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                                string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                                XmlDocument itemDoc = new XmlDocument();
                                XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                                itemDoc.Load(XmlText);
                                XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
                                xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);

                                int iAge = 0;

                                DateTime now = DateTime.Today;
                                int years = now.Year - objHuman.Birth_Date.Year;
                                if (now.Month < objHuman.Birth_Date.Month || (now.Month == objHuman.Birth_Date.Month && now.Day < objHuman.Birth_Date.Day))
                                    --years;
                                iAge = years;

                                XmlNodeList xmlAge = itemDoc.GetElementsByTagName("Age");
                                if (xmlAge != null && xmlAge.Count > 0)
                                    xmlAge[0].Attributes[0].Value = iAge.ToString();

                                XmlText.Close();
                                //itemDoc.Save(strXmlHumanFilePath);
                            //    int trycount = 0;
                            //trytosaveagain:
                                try
                                {

                                    //itemDoc.Save(strXmlHumanFilePath);
                                    IList<Human_Blob> ilstUpdateBlob = new List<Human_Blob>();
                                    byte[] bytes = null;
                                    try
                                    {
                                        bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);    
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    objHumanblob.Id = objHuman.Id;
                                    objHumanblob.Human_ID= objHuman.Id;
                                    objHumanblob.Human_XML = bytes;
                                    objHumanblob.Created_By = objHuman.Created_By;
                                    objHumanblob.Created_Date_And_Time = objHuman.Created_Date_And_Time;

                                    ilstUpdateBlob.Add(objHumanblob);
                                 
                                    MySession = session.GetISession();

                                    using (ITransaction trans1 = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                                    {
                                        try
                                        {
                                            HumanBlobMngr.SaveHumanBlobWithoutTransaction(ilstUpdateBlob, null, MySession, string.Empty);

                                            trans1.Commit();
                                        }
                                        catch (Exception ex1)
                                        {
                                            trans1.Rollback();
                                        }
                                    }

                                    MySession.Close();

                                  //  MySession = session.GetISession();

                                }
                                catch (Exception xmlexcep)
                                {
                                    throw new Exception(xmlexcep.Message.ToString());

                                    //trycount++;
                                    //if (trycount <= 3)
                                    //{
                                    //    int TimeMilliseconds = 0;
                                    //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                    //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                    //    Thread.Sleep(TimeMilliseconds);
                                    //    string sMsg = string.Empty;
                                    //    string sExStackTrace = string.Empty;

                                    //    string version = "";
                                    //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                    //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                    //    string[] server = version.Split('|');
                                    //    string serverno = "";
                                    //    if (server.Length > 1)
                                    //        serverno = server[1].Trim();

                                    //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                    //        sMsg = xmlexcep.InnerException.Message;
                                    //    else
                                    //        sMsg = xmlexcep.Message;

                                    //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                                    //        sExStackTrace = xmlexcep.StackTrace;

                                    //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                    //    string ConnectionData;
                                    //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                    //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                    //    {
                                    //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                    //        {
                                    //            cmd.Connection = con;
                                    //            try
                                    //            {
                                    //                con.Open();
                                    //                cmd.ExecuteNonQuery();
                                    //                con.Close();
                                    //            }
                                    //            catch
                                    //            {
                                    //            }
                                    //        }
                                    //    }
                                    //    goto trytosaveagain;
                                    //}
                                }



                                IList<Human> ilstHuman = new List<Human>();
                                ilstHuman.Add(objHuman);
                                objXML.itemDoc = null;
                                List<object> lsthumanObj = ilstHuman.Cast<object>().ToList();
                                objXML.Copy_Previous_GenerateXmlSave(lsthumanObj, objHuman.Id, string.Empty, false, ref objXML);
                            }
                        }
                       
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        // MySession.Close();
                        //CAP-1942
                        throw new Exception(ex.Message,ex);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        //CAP-1942
                        throw new Exception(e.Message,e);
                    }
                    finally
                    {
                       if (MySession != null && MySession.IsOpen == true)
                        {
                            MySession.Close();
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                //MySession.Close();
                //CAP-1942
                throw new Exception(ex1.Message,ex1);
            }
            if (HumanId != 0)
            {
                RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
                objRcopiaMngr.SendPatientToRCopia(HumanId, string.Empty, sLegalOrg);
            }
            //return HumanListAdd[0];
            return HumanId;



            //IList<Human> humanList = new List<Human>();
            //humanList.Add(objHuman);
            //SaveUpdateDeleteWithTransaction(ref humanList, null, null, string.Empty);

            //if (objBatchHuman != null)
            //{
            //    if (humanList[0].Id != 0)
            //        objBatchHuman.Human_ID = humanList[0].Id;
            //    BatchHumanMapManager BatchHumanMngr = new BatchHumanMapManager();
            //    BatchHumanMngr.SaveBatchHumanDetails(objBatchHuman);
            //}
            //return humanList[0];
        }
        public Boolean Is_Available_E_Mail(string sMailId)
        {
            Boolean Is_Mail = false;
            using (ISession iMysession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMysession.GetNamedQuery("Get.MailId");
                query.SetParameter(0, sMailId);
                query.SetParameter(1, sMailId);
                ArrayList iCount = new ArrayList(query.List());
                if (iCount.Count > 0)
                {
                    if (Convert.ToInt32(iCount[0]) > 0)
                        Is_Mail = true;
                }

                iMysession.Close();
            }
            return Is_Mail;
        }
        public Human GetHumanIdbyname(ulong ulHumanID, string sFirstname, string sLastname, string sDob)
        {
            IList<Human> ilstHuman = new List<Human>();
            if (sDob.Length == 8)
                sDob = sDob.Insert(4, "-").Insert(7, "-");
            else if (sDob.Length == 7)
                sDob = sDob.Insert(4, "-").Insert(6, "-");

            ISQLQuery sql = session.GetISession().CreateSQLQuery("select h.* from Human h  where h.human_id = " + ulHumanID + " and h.First_Name = '" + sFirstname.Replace("'", "''") + "' and h.Last_Name= '" + sLastname.Replace("'", "''") + "' and h.Birth_Date='" + sDob + "';").AddEntity("h", typeof(Human));
            ilstHuman = sql.List<Human>();
            
            return ilstHuman.FirstOrDefault();
        }
        #endregion
    }
}

