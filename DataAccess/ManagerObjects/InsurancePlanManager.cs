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
    public partial interface IInsurancePlanManager : IManagerBase<InsurancePlan, uint>
    {
        IList<InsurancePlan> GetInsurancebyID(ulong InsPlanId);
        IList<InsurancePlan> GetInsurancebyIDAll();
        IList<SelectPayerDTO> getInsurancePlanAndCarrierByPlanIDandCarrierNameDTO(string insurancePlanId, string carrierName, int PageNumber, int MaxResultSet, string CityName, string Zipcode, string PlanName);
        PayerLibraryDTO updateInsurancePlan(InsurancePlan insPlan, int PageNumber, int MaxResultSet, string MACAddress);
        IList<Carrier> GetCarrierList();
        IList<InsurancePlan> GetInsurancebyCarrierID(ulong CarrierId);
        PayerLibraryDTO SaveInsurancePlan(InsurancePlan insPlan, int PageNumber, int MaxResultSet, string MACAddress);
        IList<InsurancePlan> CheckInsuranceDuplicate(IList<InsurancePlan> InsPlanList);
        PayerLibraryDTO GetInsuranceList(int CarrierId, int iPageNumber, int iMaxResultSet);
        PayerLibraryDTO DeleteInsurancePlan(InsurancePlan insPlan, int PageNumber, int MaxResultSet, string MACAddress);
        //PaymentPostingDTO LoadHumanAndInsuranceDetails(ulong HumanId);
        int CheckInsuranceDuplicate(string PlanName, string CarrierName, string Address, string city, string state, string zip, string id);

        //Adde by Gopal 20131025
        //IList<InsuranceDTO> GetInsuranceNameByHumanID(ulong HumanID);
        InsurancePlan GetInsuranceByInsuranceName(string InsuranceName);
    }
    public partial class InsurancePlanManager : ManagerBase<InsurancePlan, uint>, IInsurancePlanManager
    {
        #region Constructors

        public InsurancePlanManager()
            : base()
        {

        }
        public InsurancePlanManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion


        #region Get Methods

        public IList<InsurancePlan> GetInsurancebyID(ulong InsPlanId)
        {
            IList<InsurancePlan> lstInsPln = new List<InsurancePlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", InsPlanId));
                lstInsPln = crit.List<InsurancePlan>();
                iMySession.Close();
            }
            return lstInsPln;
        }
        public IList<InsurancePlan> GetInsuranceListbyIDList(ulong[] InsPlanId)
        {
            IList<InsurancePlan> lstInsPln = new List<InsurancePlan>();
            //ArrayList arryChkList;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crit = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.In("Id", InsPlanId));
                lstInsPln = crit.List<InsurancePlan>();

                //IQuery query1 = iMySession.GetNamedQuery("Get.InsurancePlanbyID");
                //query1.SetParameterList("InsurancePlanID", InsPlanId);
                //arryChkList = new ArrayList(query1.List());

                //if (arryChkList != null)
                //{
                //    for (int i = 0; i < arryChkList.Count; i++)
                //    {
                //        InsurancePlan objPlan = new InsurancePlan();
                //        object[] oj = (object[])arryChkList[i];
                //        objPlan.Carrier_ID = Convert.ToInt32(oj[0].ToString());
                //        objPlan.Id = Convert.ToUInt64(oj[1].ToString());
                //        lstInsPln.Add(objPlan);
                //    }
                //}
                iMySession.Close();
            }
            return lstInsPln;
        }
        public IList<InsurancePlan> GetInsurancebyCarrierID(ulong CarrierId)
        {
            IList<InsurancePlan> lstInsPln = new List<InsurancePlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //Jira #CAP-516
                //ICriteria crit = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Carrier_ID", Convert.ToInt32(CarrierId)));
                ICriteria crit = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Carrier_ID", Convert.ToInt32(CarrierId))).Add(Expression.Eq("Active", "Y"));
                lstInsPln = crit.List<InsurancePlan>();
                iMySession.Close();
            }
            return lstInsPln;
        }


        public IList<InsurancePlan> GetInsurancebyIDAll()
        {
            IList<InsurancePlan> lstInsPln = new List<InsurancePlan>();
            //CAP-2241
            //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            //{
            //    //Jira #CAP-516
            //    //ICriteria crit = iMySession.CreateCriteria(typeof(InsurancePlan));
            //    ICriteria crit = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Active", "Y"));
            //    lstInsPln = crit.List<InsurancePlan>();
            //    iMySession.Close();
            //}
            IList<object> ilistInsurancePlan = new List<object>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ilistInsurancePlan = iMySession.CreateSQLQuery("SELECT Insurance_Plan_ID, Insurance_Plan_Name from insurance_plan where Active='Y'").List<object>();
                iMySession.Close();
            }
            if (ilistInsurancePlan != null && ilistInsurancePlan.Any())
            {
                lstInsPln = ilistInsurancePlan.Select(a => new InsurancePlan()
                {
                    Id = Convert.ToUInt64(((object[])a)[0]),
                    Ins_Plan_Name = Convert.ToString(((object[])a)[1])
                }).ToList();
            }
            return lstInsPln;
        }

        public PayerLibraryDTO updateInsurancePlan(InsurancePlan insPlan, int PageNumber, int MaxResultSet, string MACAddress)
        {
            IList<InsurancePlan> insPlanList = new List<InsurancePlan>();
            insPlanList.Add(insPlan);
            IList<InsurancePlan> insPlanInsertList = null;
            if (insPlanList.Count > 0)
            {
                SaveUpdateDelete_DBAndXML_WithTransaction(ref insPlanInsertList, ref insPlanList, null, MACAddress, false, false, 0, "");
            }
            return GetInsuranceList(insPlanList[0].Carrier_ID, PageNumber, MaxResultSet);
        }
        public PayerLibraryDTO SaveInsurancePlan(InsurancePlan insPlan, int PageNumber, int MaxResultSet, string MACAddress)
        {

            IList<InsurancePlan> insPlanList = new List<InsurancePlan>();
            insPlanList.Add(insPlan);
            if (insPlanList.Count > 0)
            {
                IList<InsurancePlan> InsPlanTemp = null;
                SaveUpdateDelete_DBAndXML_WithTransaction(ref insPlanList, ref InsPlanTemp, null, MACAddress, false, false, 0, "");
            }
            return GetInsuranceList(insPlanList[0].Carrier_ID, PageNumber, MaxResultSet);
        }

        public PayerLibraryDTO DeleteInsurancePlan(InsurancePlan insPlan, int PageNumber, int MaxResultSet, string MACAddress)
        {
            IList<InsurancePlan> insPlanList = new List<InsurancePlan>();
            insPlanList.Add(insPlan);
            IList<InsurancePlan> insPlanInsertList = null;
            if (insPlanList.Count > 0)
            {
                IList<InsurancePlan> InsPlanTemp = null;
                SaveUpdateDelete_DBAndXML_WithTransaction(ref insPlanInsertList, ref InsPlanTemp, insPlanList, MACAddress, false, false, 0, "");
            }
            return GetInsuranceList(insPlanList[0].Carrier_ID, PageNumber, MaxResultSet);
        }

        public IList<Carrier> GetCarrierList()
        {
            IList<Carrier> lstCarrier = new List<Carrier>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Carrier));
                lstCarrier = crit.List<Carrier>();
                iMySession.Close();
            }
            return lstCarrier;
        }
        public IList<SelectPayerDTO> getInsurancePlanAndCarrierByPlanIDandCarrierNameDTO(string insurancePlanId, string carrierName, int PageNumber, int MaxResultSet, string CityName, string Zipcode, string PlanName)
        {
            string s = insurancePlanId + '%';
            int Insurancecount;
            IList<SelectPayerDTO> SelectPayerList = new List<SelectPayerDTO>();
            SelectPayerDTO ObjSelect = null;//= new SelectPayerDTO();
            ArrayList InsuranceList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.insurancePlanAndCarrier.ByInsurancePlanIDandCarrierName");
                query.SetString(0, s);
                query.SetString(1, "%" + carrierName);
                query.SetString(2, CityName);
                query.SetString(3, Zipcode);
                query.SetString(4, "%" + PlanName);
                InsuranceList = new ArrayList(query.List());

                Insurancecount = Convert.ToInt16(InsuranceList[0]);
                InsuranceList.Clear();

                IQuery query1 = iMySession.GetNamedQuery("Get.insurancePlanAndCarrier.ByInsurancePlanIDandCarrierName.WithLimit");
                query1.SetString(0, s);
                query1.SetString(1, "%" + carrierName);
                PageNumber = PageNumber - 1;
                query1.SetString(2, CityName);
                query1.SetString(3, Zipcode);
                query1.SetString(4, "%" + PlanName);
                query1.SetInt32(5, PageNumber * MaxResultSet);
                query1.SetInt32(6, MaxResultSet);

                InsuranceList = new ArrayList(query1.List());

                if (InsuranceList != null)
                {
                    for (int i = 0; i < InsuranceList.Count; i++)
                    {
                        ObjSelect = new SelectPayerDTO();
                        object[] obj = (object[])InsuranceList[i];
                        ObjSelect.Insurance_Plan_ID = Convert.ToUInt64(obj[0]);
                        ObjSelect.Carrier_Name = obj[1].ToString();
                        ObjSelect.Ins_Plan_Name = obj[2].ToString();
                        ObjSelect.Payer_Addrress1 = obj[3].ToString();
                        ObjSelect.Payer_City = obj[4].ToString();
                        ObjSelect.Payer_State = obj[5].ToString();
                        ObjSelect.Payer_Zip = obj[6].ToString();
                        ObjSelect.Payer_Ph_No = obj[7].ToString();
                        ObjSelect.Financial_Class_ID = obj[8].ToString();
                        ObjSelect.Carrier_ID = Convert.ToUInt64(obj[9]);
                        ObjSelect.InsuranceCount = Insurancecount;

                        SelectPayerList.Add(ObjSelect);
                    }
                }
                iMySession.Close();
            }

            return SelectPayerList;
        }

        public IList<InsurancePlan> CheckInsuranceDuplicate(IList<InsurancePlan> InsPlanList)
        {
            IList<InsurancePlan> ResultList = null;
            if (InsPlanList.Count > 0)
            {

                //ICriteria crit = session.GetISession().CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Ins_Plan_Name", InsPlanList[0].Ins_Plan_Name))
                //    .Add(Expression.NotEqProperty("Id", InsPlanList[0].Id.ToString())).Add(Expression.Eq("Payer_Addrress1", InsPlanList[0].Payer_Addrress1))
                //    .Add(Expression.Eq("Payer_City", InsPlanList[0].Payer_City)).Add(Expression.Eq("Payer_State", InsPlanList[0].Payer_State))
                //    .Add(Expression.Eq("Payer_Zip", InsPlanList[0].Payer_Zip)).Add(Expression.Eq("Carrier_ID", InsPlanList[0].Carrier_ID.ToString()));
                // ResultList = crit.List<InsurancePlan>();
                // return crit.List<InsurancePlan>();
            }
            return ResultList;
        }


        public PayerLibraryDTO GetInsuranceList(int CarrierId, int iPageNumber, int iMaxResultSet)
        {
            PayerLibraryDTO objPayerLibraryDTO = new PayerLibraryDTO();
            IList<InsurancePlan> InsuranceList = new List<InsurancePlan>();
            ArrayList aryPayerList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.InsurancePlan");
                query1.SetString(0, Convert.ToString(CarrierId));
                if (query1.List().Count > 0)
                    objPayerLibraryDTO.InsurancePlanListCount = Convert.ToInt32(query1.List()[0]);
                IQuery query2 = iMySession.GetNamedQuery("Get.InsurancePlan.WithLimit");
                int PageNumber = iPageNumber - 1;
                query2.SetString(0, Convert.ToString(CarrierId));
                query2.SetInt32(1, PageNumber * iMaxResultSet);
                query2.SetInt32(2, iMaxResultSet);
                aryPayerList = new ArrayList(query2.List());
                foreach (object[] obj in aryPayerList)
                {
                    InsurancePlan objInsurance = new InsurancePlan();

                    objInsurance.Id = Convert.ToUInt64(obj[0]);
                    objInsurance.Ins_Plan_Name = obj[1].ToString();
                    objInsurance.Carrier_ID = Convert.ToInt32(obj[2]);
                    objInsurance.Payer_Rec_Code = obj[3].ToString();
                    objInsurance.Payer_Addrress1 = obj[4].ToString();
                    objInsurance.Payer_Addrress2 = obj[5].ToString();
                    objInsurance.Payer_City = obj[6].ToString();
                    objInsurance.Payer_State = obj[7].ToString();
                    objInsurance.Payer_Zip = obj[8].ToString();
                    objInsurance.Payer_Phone_Number = obj[9].ToString();
                    objInsurance.Eligibility_Phone_Number = obj[10].ToString();
                    objInsurance.Office_Number = obj[11].ToString();
                    objInsurance.Fax_Number = obj[12].ToString();
                    objInsurance.Payer_Notes = obj[13].ToString();
                    objInsurance.Form_Number = obj[14].ToString();
                    objInsurance.Format_File_Number = obj[15].ToString();
                    objInsurance.Emc_ID1 = obj[16].ToString();
                    objInsurance.Emc_ID2 = obj[17].ToString();
                    objInsurance.Govt_Type = obj[18].ToString();
                    objInsurance.Financial_Class_Name = obj[19].ToString();
                    objInsurance.Is_Workers_Comp = obj[20].ToString();
                    objInsurance.Active = obj[21].ToString();
                    objInsurance.External_Plan_Number = obj[22].ToString();
                    objInsurance.Created_By = obj[23].ToString();
                    objInsurance.Created_Date_And_Time = Convert.ToDateTime(obj[24]);
                    objInsurance.Modified_By = obj[25].ToString();
                    objInsurance.Modified_Date_And_Time = Convert.ToDateTime(obj[26]);
                    objInsurance.Version = Convert.ToInt32(obj[27]);
                    InsuranceList.Add(objInsurance);

                }
                objPayerLibraryDTO.InsurancePlanList = InsuranceList;
                iMySession.Close();
            }
            return objPayerLibraryDTO;
        }

        //public PaymentPostingDTO LoadHumanAndInsuranceDetails(ulong HumanId)
        //{
        //    PaymentPostingDTO objPaymentPostingDTO = new PaymentPostingDTO();
        //    ICriteria crit = session.GetISession().CreateCriteria(typeof(Human)).Add(Expression.Eq("Id", HumanId));
        //    if (crit.List<Human>().Count != 0)
        //    {
        //        objPaymentPostingDTO.HumanRecord = crit.List<Human>()[0];
        //        if (objPaymentPostingDTO.HumanRecord.PatientInsuredBag != null)
        //        {
        //            for (int i = 0; i < objPaymentPostingDTO.HumanRecord.PatientInsuredBag.Count; i++)
        //            {
        //                InsurancePlan objInsurancePlan = null;
        //                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //                {
        //                    crit = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Id", objPaymentPostingDTO.HumanRecord.PatientInsuredBag[i].Insurance_Plan_ID));
        //                    if (crit.List<InsurancePlan>().Count != 0)
        //                        objInsurancePlan = crit.List<InsurancePlan>()[0];
        //                    iMySession.Close();
        //                }
        //                objPaymentPostingDTO.InsurancePlanList.Add(objInsurancePlan);
        //            }
        //        }
        //    }
        //    return objPaymentPostingDTO;
        //}
        public int CheckInsuranceDuplicate(string PlanName, string CarrierName, string Address, string city, string state, string zip, string id)
        {
            int count = 0;
            ArrayList arrList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Get.Duplicate.Insurance.Count");
                query1.SetString(0, PlanName);
                query1.SetString(1, Address);
                query1.SetString(2, CarrierName);
                query1.SetString(3, city);
                query1.SetString(4, state);
                query1.SetString(5, zip);
                query1.SetString(6, id);
                arrList = new ArrayList(query1.List());
                count = Convert.ToInt32(arrList[0]);
                iMySession.Close();
            }
            return count;
        }

        //public IList<InsuranceDTO> GetInsuranceNameByHumanID(ulong HumanID)
        //{
        //    IList<InsuranceDTO> ilstInsName = new List<InsuranceDTO>();
        //    InsuranceDTO objInsName;
        //    string sWhereCriteria = "WHERE p.human_id=" + HumanID + "";
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IList<object> objLst = iMySession.CreateSQLQuery("SELECT i.Insurance_plan_Name,p.insurance_plan_id,p.pat_insured_plan_id FROM pat_insured_plan p inner join insurance_plan i on (p.insurance_plan_id=i.insurance_plan_id) " + sWhereCriteria).List<object>();
        //        for (int i = 0; i < objLst.Count; i++)
        //        {
        //            object[] obj = (object[])objLst[i];
        //            objInsName = new InsuranceDTO();
        //            objInsName.Ins_Plan_Name = obj[0].ToString();
        //            objInsName.Ins_Plan_Id = obj[1].ToString();
        //            objInsName.Pat_Ins_Plan_Id = obj[2].ToString();
        //            ilstInsName.Add(objInsName);
        //        }
        //        iMySession.Close();
        //    }
        //    return ilstInsName;
        //}
        public InsurancePlan GetInsuranceByInsuranceName(string InsuranceName)
        {
            InsurancePlan InsPlanRecord = new InsurancePlan();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria InsCriteria = iMySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.Eq("Ins_Plan_Name", InsuranceName)); ;
                if (InsCriteria.List<InsurancePlan>().Count != 0)
                    InsPlanRecord = InsCriteria.List<InsurancePlan>()[0];
                iMySession.Close();
            }
            return InsPlanRecord;
        }
        public IList<object> GetInsuranceName(string sCarrierName)
        {
            IList<object> ilistInsurancePlan = new List<object>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ilistInsurancePlan = iMySession.CreateSQLQuery("SELECT i.insurance_plan_name from insurance_plan i join carrier c on (c.carrier_id=i.carrier_id) where c.carrier_name='" + sCarrierName + "'").List<object>();
                iMySession.Close();
            }

            return ilistInsurancePlan;
        }
        public IList<InsurancePlan> GetInsDetails(IList<ulong> InsurancID)
        {
            IList<InsurancePlan> objPatInsPlan = new List<InsurancePlan>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(InsurancePlan)).Add(Expression.In("Id", InsurancID.ToArray<ulong>()));
                objPatInsPlan = crit.List<InsurancePlan>();
                mySession.Close();
            }
            return objPatInsPlan;
        }

        public IList<InsurancePlan> GetPlanFromTokens(string text_searched)
        {

            ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<InsurancePlan> lstMatchingPlan = new List<InsurancePlan>();

            string sQuery = "select Insurance_Plan_ID,Insurance_Plan_Name,Carrier_ID,concat(Payer_Addrress1,if(Payer_City<>'',concat(', ',Payer_City),''),if(Payer_State<>'',concat(', ',Payer_State),''),if((Payer_Zip<>'' and payer_zip<>'_____-____'),concat(', ',Payer_Zip),''))as addr from insurance_plan where Insurance_Plan_Name like '" + text_searched + "%' and Active<>'N'";

            ISQLQuery query = iMySession.CreateSQLQuery(sQuery);
            ArrayList arrPatients = new ArrayList(query.List());
            iMySession.Close();
            for (int iCount = 0; iCount < arrPatients.Count; iCount++)
            {
                InsurancePlan tempHuman = new InsurancePlan();
                object[] objHuman = (object[])arrPatients[iCount];
                tempHuman.Id = Convert.ToUInt32(objHuman[0].ToString());
                tempHuman.Ins_Plan_Name = objHuman[1].ToString();
                tempHuman.Carrier_ID = Convert.ToInt32(objHuman[2].ToString());
                tempHuman.Payer_Addrress1 = objHuman[3].ToString();
                lstMatchingPlan.Add(tempHuman);

            }
            return lstMatchingPlan;

        }
        #endregion
    }
}
