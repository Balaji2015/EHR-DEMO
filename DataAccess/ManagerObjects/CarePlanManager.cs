using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.Collections;
using NHibernate;
using NHibernate.Criterion;
using System.Globalization;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface ICarePlanManager : IManagerBase<CarePlan, ulong>
    {
        IList<CarePlanDTO> SaveCarePlan(IList<CarePlan> CarePlanLst, string sex, int iYear, string MacAddress);
        IList<CarePlanDTO> UpdateCarePLan(IList<CarePlan> CarePlanLst, string sSex, int iYear, string MacAddress);
        IList<CarePlanDTO> GetCarePlanForPastEncounter(ulong encounter_ID, ulong human_ID, ulong Physician_ID, string gender, int age);
        IList<CarePlanDTO> GetPlanCareFromServerforThin(ulong UlEncounterID, ulong ulHumanID, string Sex, int Months);
        void SaveCarePlanforSummary(IList<CarePlan> lstcareplan);
        IList<CarePlanDTO> UpdateCarePLanForCopyPrevious(IList<CarePlan> CarePlanLst, string sSex, int iYear, string MacAddress, ulong Encoutner_Id, ulong Human_Id);
        IList<CarePlan> UpdateCarePLanXML(IList<CarePlan> CarePlanLst, string sSex, int iYear, string MacAddress);
        IList<CarePlan> SaveCarePlanXML(IList<CarePlan> CarePlanLst, string sex, int iYear, string MacAddress);
        IList<CarePlan> GetCarePlanByHuman(ulong humanID);

    }

    public partial class CarePlanManager : ManagerBase<CarePlan, ulong>, ICarePlanManager
    {
        #region Constructors

        public CarePlanManager()
            : base()
        {

        }
        public CarePlanManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion


        #region ICarePlanManager Members


        private SocialHistory GetSocialByHumanID(ulong ulHumanID)
        {
            SocialHistory socialHistoryList = new SocialHistory();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crt = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Social_Info", "Marital Status"));
                if (crt.List<SocialHistory>().Count > 0)
                    socialHistoryList = crt.List<SocialHistory>()[0];
                iMySession.Close();
            }
            return socialHistoryList;
        }

        public IList<CarePlanDTO> SaveCarePlan(IList<CarePlan> CarePlanLst, string sex, int iYear, string MacAddress)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<MeasuresRuleMaster> lstMeasures = new List<MeasuresRuleMaster>();

            MeasuresRuleMasterManager measuresMngr = new MeasuresRuleMasterManager();
            lstMeasures = measuresMngr.GetAll();

            IList<ImmunizationHistory> saveImmunizationHistory = null;
            ImmunizationHistoryManager objImmhstryMngr = new ImmunizationHistoryManager();

            IList<PatientResults> savePatientResults = null;
            VitalsManager objPateintresultsMngr = new VitalsManager();

            IList<SurgicalHistory> SurgicalSaveUpdateDelete = null;
            SurgicalHistoryManager objSurgicalhistoryMngr = new SurgicalHistoryManager();

            IList<Human> saveHuman = new List<Human>();
            Human objHuman = new Human();
            IList<AdvanceDirective> saveAD = null;
            AdvanceDirectiveManager objADMngr = new AdvanceDirectiveManager();
            HumanManager hnMngr = new HumanManager();

            IList<Human> insertHuman = null;
            IList<ImmunizationHistory> ilstUpdateImmunization = null;
            IList<SurgicalHistory> ilstUpdateSurgical = null;
            IList<PatientResults> ilstUpdatePatientResults = null;
            IList<AdvanceDirective> ilstUpdateAD = null;


            PhysicianPatientManager phyPatMgr = new PhysicianPatientManager();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                DateTime Dob = DateTime.MinValue;
                DateTime AdminsterdDate = DateTime.MinValue;
                int CurrentAge = 0;
                if (CarePlanLst != null && CarePlanLst.Count > 0)
                {
                    ulong HumanID = 0;
                    HumanID = CarePlanLst[0].Human_ID;
                    if (HumanID != 0)
                    {
                        ISQLQuery sqlqueryHuman = iMySession.CreateSQLQuery("select h.* from human h   where h.Human_ID='" + HumanID + "'").AddEntity("h", typeof(Human));
                        IList<Human> lstHuman = sqlqueryHuman.List<Human>();

                        double humanAgeInMonths = 0;
                        int humanAgeInYears = 0;
                        if (lstHuman != null && lstHuman.Count > 0)
                        {
                            Dob = lstHuman[0].Birth_Date;
                            humanAgeInMonths = ((DateTime.Now.Date - lstHuman[0].Birth_Date.Date).TotalDays) / 30.4375;
                            CarePlanManager careMngr = new CarePlanManager();
                            humanAgeInYears = careMngr.CalculateAge(lstHuman[0].Birth_Date);
                        }
                    }

                    for (int i = 0; i < CarePlanLst.Count; i++)
                    {
                        if (CarePlanLst[i].Internal_Property_bInsert == true)
                        {

                            var res = from m in lstMeasures where m.Measure_Name == CarePlanLst[i].Care_Name_Value select m;
                            IList<MeasuresRuleMaster> measures = res.ToList<MeasuresRuleMaster>();
                            if (measures.Count > 0)
                            {
                                switch (measures[0].Table_Name)
                                {
                                    case "immunization_history":

                                        Boolean bFirst = true;

                                        ImmunizationHistory objImmuHistory = new ImmunizationHistory();

                                    First:
                                        objImmuHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objImmuHistory.Physician_ID = CarePlanLst[i].Physician_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] CodeDesc = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            if (CodeDesc.Length > 0)
                                            {
                                                for (int j = 0; j < CodeDesc.Length; j++)
                                                {
                                                    if (CodeDesc[j].Contains(";"))
                                                    {
                                                        string[] Code = CodeDesc[j].Split(new char[] { ';' });
                                                        string[] CodeAlone = Code[1].Split(new char[] { '$' });
                                                        if (Code.Length > 0)
                                                        {
                                                            if (CarePlanLst[i].Plan_Date != "01-01-0001")
                                                            {
                                                                if (CarePlanLst[i].Plan_Date != string.Empty)
                                                                {
                                                                    //commented by saravanakumar on12-02-2014 for change customdatetimepicker
                                                                    if (CarePlanLst[i].Plan_Date.Contains('-'))
                                                                    {
                                                                        string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                                                        if (dates.Length == 2)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime("01-" + dates[0] + dates[1]);
                                                                            AdminsterdDate = Convert.ToDateTime("01-" + dates[1] + dates[0]);
                                                                        }
                                                                        if (dates.Length == 3)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                            AdminsterdDate = Convert.ToDateTime(dates[2] + "-" + dates[1] + "-" + dates[0]);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        AdminsterdDate = Convert.ToDateTime("01-" + "Jan-" + CarePlanLst[i].Plan_Date);
                                                                    }
                                                                    // AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                    CurrentAge = AdminsterdDate.Year - Dob.Year;
                                                                }
                                                            }
                                                            if (Code[0].Contains(">"))
                                                            {
                                                                string[] Codegreater = Code[0].Split(new char[] { '>' });
                                                                if (Codegreater.Length > 0)
                                                                {
                                                                    if (CurrentAge > Convert.ToInt32(Codegreater[1]))
                                                                    {
                                                                        objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                        objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            if (Code[0].Contains("<"))
                                                            {
                                                                if (Code[0].Contains("=") == false)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new char[] { '<' });
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[1]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }

                                                                }
                                                                else if (Code[0].Contains("=") == true)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new string[] { "<=" }, StringSplitOptions.RemoveEmptyEntries);
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[0]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            objImmuHistory.Procedure_Code = measures[0].Where_Criteria.Split(new char[] { '$' })[0];
                                            objImmuHistory.Immunization_Description = measures[0].Where_Criteria.Split(new char[] { '$' })[1];
                                        }

                                        if (CarePlanLst[i].Plan_Date.Contains('-'))
                                        {
                                            string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                            if (dates.Length == 2)
                                                objImmuHistory.Administered_Date = dates[1] + "-" + dates[0];
                                            if (dates.Length == 3)
                                                objImmuHistory.Administered_Date = dates[2] + "-" + dates[1] + "-" + dates[0];
                                        }
                                        else
                                        {
                                            objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        }

                                        //objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        objImmuHistory.Encounter_ID = CarePlanLst[i].Encounter_ID;
                                        objImmuHistory.Created_By = CarePlanLst[i].Created_By;
                                        objImmuHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objImmuHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objImmuHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;

                                        ImmunizationHistoryDTO ImmunizationHistorydto = objImmhstryMngr.GetFromImmunizationHistory(CarePlanLst[0].Human_ID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<ImmunizationHistory> ilstImmunization = new List<ImmunizationHistory>();

                                        if (ImmunizationHistorydto.ImmunizationCount > 0)
                                        {
                                            var immun = from im in ImmunizationHistorydto.Immunization where im.Procedure_Code == objImmuHistory.Procedure_Code select im;

                                            ilstImmunization = immun.ToList<ImmunizationHistory>();
                                            if (ilstImmunization.Count > 0)
                                            {
                                                objImmuHistory = ilstImmunization[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto First;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateImmunization == null)
                                                        ilstUpdateImmunization = new List<ImmunizationHistory>();
                                                    ilstUpdateImmunization.Add(objImmuHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (saveImmunizationHistory == null)
                                                    saveImmunizationHistory = new List<ImmunizationHistory>();
                                                saveImmunizationHistory.Add(objImmuHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (saveImmunizationHistory == null)
                                                saveImmunizationHistory = new List<ImmunizationHistory>();
                                            saveImmunizationHistory.Add(objImmuHistory);
                                        }
                                        break;

                                    case "surgical_history":

                                        bFirst = true;

                                    second: SurgicalHistory objSurgicalHistory = new SurgicalHistory();
                                        objSurgicalHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objSurgicalHistory.Surgery_Name = CarePlanLst[i].Care_Name_Value;
                                        objSurgicalHistory.Date_Of_Surgery = CarePlanLst[i].Plan_Date;
                                        objSurgicalHistory.Description = CarePlanLst[i].Care_Plan_Notes;
                                        objSurgicalHistory.Created_By = CarePlanLst[i].Created_By;
                                        objSurgicalHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objSurgicalHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objSurgicalHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        SurgicalHistoryDTO SurgicalHisoryDetails = objSurgicalhistoryMngr.LoadSurgicalHistory(HumanID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<SurgicalHistory> ilstSurgicalHistory = new List<SurgicalHistory>();

                                        if (SurgicalHisoryDetails.SurgicalCount > 0)
                                        {
                                            var surgical = from sur in SurgicalHisoryDetails.SurgicalList where sur.Description == objSurgicalHistory.Description && sur.Date_Of_Surgery == objSurgicalHistory.Date_Of_Surgery select sur;

                                            ilstSurgicalHistory = surgical.ToList<SurgicalHistory>();
                                            if (ilstSurgicalHistory.Count > 0)
                                            {
                                                objSurgicalHistory = ilstSurgicalHistory[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto second;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateSurgical == null)
                                                        ilstUpdateSurgical = new List<SurgicalHistory>();
                                                    ilstUpdateSurgical.Add(objSurgicalHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (SurgicalSaveUpdateDelete == null)
                                                    SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                                SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (SurgicalSaveUpdateDelete == null)
                                                SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                            SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                        }

                                        break;



                                    case "patient_results":
                                        bFirst = true;
                                    Third: PatientResults objPatient_Results = new PatientResults();
                                        objPatient_Results.Human_ID = CarePlanLst[i].Human_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] ResultsCode = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            objPatient_Results.Loinc_Identifier = ResultsCode[0];
                                            objPatient_Results.Loinc_Observation = ResultsCode[1];
                                            objPatient_Results.Units = ResultsCode[2];
                                            objPatient_Results.Reference_Range = ResultsCode[3];
                                            objPatient_Results.Acurus_Result_Code = ResultsCode[4];
                                            objPatient_Results.Acurus_Result_Description = ResultsCode[5];
                                            objPatient_Results.Results_Type = ResultsCode[6];
                                            //objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            if (CarePlanLst[i].Plan_Date != "")
                                                objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            else
                                            {
                                                if (CarePlanLst[i].Created_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Created_Date_And_Time);
                                                else if (CarePlanLst[i].Modified_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Modified_Date_And_Time);
                                                else
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(DateTime.Now);
                                            }
                                        }
                                        objPatient_Results.Physician_ID = CarePlanLst[i].Physician_ID;
                                        objPatient_Results.Value = CarePlanLst[i].Status_Value;
                                        objPatient_Results.Created_By = CarePlanLst[i].Created_By;
                                        objPatient_Results.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        // savePatientResults.Add(objPatient_Results);

                                        PatientResultsDTO patientResultdto = objPateintresultsMngr.GetPastVitalDetailsByPatientforGrowthChart(HumanID, 1, 10000);
                                        IList<PatientResults> ilstPatientResults = new List<PatientResults>();

                                        if (patientResultdto.VitalCount > 0)
                                        {
                                            var patientres = from resu in patientResultdto.VitalsList where resu.Acurus_Result_Code == objPatient_Results.Acurus_Result_Code && resu.Captured_date_and_time == objPatient_Results.Captured_date_and_time select resu;

                                            ilstPatientResults = patientres.ToList<PatientResults>();
                                            if (ilstPatientResults.Count > 0)
                                            {
                                                objPatient_Results = ilstPatientResults[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto Third;
                                                }
                                                else
                                                {
                                                    if (ilstUpdatePatientResults == null)
                                                        ilstUpdatePatientResults = new List<PatientResults>();
                                                    ilstUpdatePatientResults.Add(objPatient_Results);
                                                }
                                            }
                                            else
                                            {
                                                if (savePatientResults == null)
                                                    savePatientResults = new List<PatientResults>();
                                                savePatientResults.Add(objPatient_Results);
                                            }
                                        }
                                        else
                                        {
                                            if (savePatientResults == null)
                                                savePatientResults = new List<PatientResults>();
                                            savePatientResults.Add(objPatient_Results);
                                        }

                                        break;

                                    case "human":
                                        if (objHuman != null && objHuman.Id == 0)
                                        {
                                            objHuman = hnMngr.GetById(CarePlanLst[i].Human_ID);
                                        }


                                        objHuman.Modified_By = CarePlanLst[i].Modified_By;
                                        objHuman.Marital_Status = CarePlanLst[i].Status;
                                        objHuman.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        //saveHuman.Add(objHuman);
                                        break;

                                    case "advance_directive":
                                        bFirst = true;
                                    Forur: AdvanceDirective objAD = new AdvanceDirective();
                                        objAD.Human_ID = CarePlanLst[i].Human_ID;
                                        objAD.Encounter_Id = CarePlanLst[i].Encounter_ID;
                                        objAD.Status = CarePlanLst[i].Status;
                                        objAD.Comments = CarePlanLst[i].Care_Plan_Notes;
                                        objAD.Created_By = CarePlanLst[i].Created_By;
                                        objAD.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;

                                        FillOtherHistory AdvanceDirectivedto = phyPatMgr.LoadPhysicianPatient(1, 10000, HumanID, CarePlanLst[0].Encounter_ID, false);
                                        if (AdvanceDirectivedto.Advance_Directive.Count > 0)
                                        {
                                            objAD = AdvanceDirectivedto.Advance_Directive[0];
                                            if (bFirst == true)
                                            {
                                                bFirst = false;
                                                goto Forur;
                                            }
                                            else
                                            {
                                                if (ilstUpdateAD == null)
                                                    ilstUpdateAD = new List<AdvanceDirective>();
                                                ilstUpdateAD.Add(objAD);
                                            }
                                        }
                                        else
                                        {
                                            if (saveAD == null)
                                                saveAD = new List<AdvanceDirective>();
                                            saveAD.Add(objAD);
                                        }
                                        break;


                                }
                            }


                            //  saveImmunizationHistory.Add(objImmuHistory);



                        }
                    }


                }




                //SaveUpdateDeleteWithTransaction(ref CarePlanLst, null, null, MacAddress);
                IList<CarePlan> CarePlanLstnull = null;
                SaveUpdateDelete_DBAndXML_WithTransaction(ref CarePlanLst, ref CarePlanLstnull, null, string.Empty, false, false, 0, string.Empty);
                if (CarePlanLst.Any(a => a.Care_Name_Value == "Marital Status"))
                    SocialHistorySaveOrUpdate(CarePlanLst, MacAddress);

                //objImmhstryMngr.SaveUpdateDeleteWithTransaction(ref saveImmunizationHistory, ilstUpdateImmunization, null, string.Empty);
                objImmhstryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveImmunizationHistory, ref ilstUpdateImmunization, null, string.Empty, false, false, 0, string.Empty);
                //objSurgicalhistoryMngr.SaveUpdateDeleteWithTransaction(ref SurgicalSaveUpdateDelete, ilstUpdateSurgical, null, string.Empty);
                objSurgicalhistoryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref SurgicalSaveUpdateDelete, ref ilstUpdateSurgical, null, string.Empty, false, false, 0, string.Empty);
                //objPateintresultsMngr.SaveUpdateDeleteWithTransaction(ref savePatientResults, ilstUpdatePatientResults, null, string.Empty);
                objPateintresultsMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref savePatientResults, ref ilstUpdatePatientResults, null, string.Empty, false, false, 0, string.Empty);
                //if (saveHuman.Count > 0)
                //    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, false, false, 0, string.Empty);
                if (objHuman != null && objHuman.Id != 0)
                {
                    saveHuman.Add(objHuman);
                    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, false, false, 0, string.Empty);
                }

                hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, false, false, 0, string.Empty);
                //hnMngr.SaveUpdateDeleteWithTransaction(ref insertHuman, saveHuman, null, string.Empty);
                //objADMngr.SaveUpdateDeleteWithTransaction(ref saveAD, ilstUpdateAD, null, string.Empty);
                objADMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveAD, ref ilstUpdateAD, null, string.Empty, false, false, 0, string.Empty);
                iMySession.Close();
            }
            //GenerateXml XMLObj = new GenerateXml();
            //if (CarePlanLst.Count > 0)
            //{

            //    ulong encounterid = CarePlanLst[0].Encounter_ID;
            //    List<object> lstObj = CarePlanLst.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}

            return GetPlanCareFromServerforThin(CarePlanLst[0].Encounter_ID, CarePlanLst[0].Human_ID, sex, iYear);
        }
        public IList<CarePlan> SaveCarePlanXML(IList<CarePlan> CarePlanLst, string sex, int iYear, string MacAddress)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<MeasuresRuleMaster> lstMeasures = new List<MeasuresRuleMaster>();

            MeasuresRuleMasterManager measuresMngr = new MeasuresRuleMasterManager();
            lstMeasures = measuresMngr.GetAll();

            IList<ImmunizationHistory> saveImmunizationHistory = null;
            ImmunizationHistoryManager objImmhstryMngr = new ImmunizationHistoryManager();

            IList<PatientResults> savePatientResults = null;
            VitalsManager objPateintresultsMngr = new VitalsManager();

            IList<SurgicalHistory> SurgicalSaveUpdateDelete = null;
            SurgicalHistoryManager objSurgicalhistoryMngr = new SurgicalHistoryManager();

            IList<Human> saveHuman = new List<Human>();
            Human objHuman = new Human();
            IList<AdvanceDirective> saveAD = null;
            AdvanceDirectiveManager objADMngr = new AdvanceDirectiveManager();
            HumanManager hnMngr = new HumanManager();

            IList<Human> insertHuman = null;
            IList<ImmunizationHistory> ilstUpdateImmunization = null;
            IList<SurgicalHistory> ilstUpdateSurgical = null;
            IList<PatientResults> ilstUpdatePatientResults = null;
            IList<AdvanceDirective> ilstUpdateAD = null;


            PhysicianPatientManager phyPatMgr = new PhysicianPatientManager();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                DateTime Dob = DateTime.MinValue;
                DateTime AdminsterdDate = DateTime.MinValue;
                int CurrentAge = 0;
                if (CarePlanLst != null && CarePlanLst.Count > 0)
                {
                    ulong HumanID = 0;
                    HumanID = CarePlanLst[0].Human_ID;
                    if (HumanID != 0)
                    {
                        ISQLQuery sqlqueryHuman = iMySession.CreateSQLQuery("select h.* from human h   where h.Human_ID='" + HumanID + "'").AddEntity("h", typeof(Human));
                        IList<Human> lstHuman = sqlqueryHuman.List<Human>();

                        double humanAgeInMonths = 0;
                        int humanAgeInYears = 0;
                        if (lstHuman != null && lstHuman.Count > 0)
                        {
                            Dob = lstHuman[0].Birth_Date;
                            humanAgeInMonths = ((DateTime.Now.Date - lstHuman[0].Birth_Date.Date).TotalDays) / 30.4375;
                            CarePlanManager careMngr = new CarePlanManager();
                            humanAgeInYears = careMngr.CalculateAge(lstHuman[0].Birth_Date);
                        }
                    }

                    for (int i = 0; i < CarePlanLst.Count; i++)
                    {
                        if (CarePlanLst[i].Internal_Property_bInsert == true)
                        {

                            var res = from m in lstMeasures where m.Measure_Name == CarePlanLst[i].Care_Name_Value select m;
                            IList<MeasuresRuleMaster> measures = res.ToList<MeasuresRuleMaster>();
                            if (measures.Count > 0)
                            {
                                switch (measures[0].Table_Name)
                                {
                                    case "immunization_history":

                                        Boolean bFirst = true;

                                        ImmunizationHistory objImmuHistory = new ImmunizationHistory();

                                    First:
                                        objImmuHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objImmuHistory.Physician_ID = CarePlanLst[i].Physician_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] CodeDesc = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            if (CodeDesc.Length > 0)
                                            {
                                                for (int j = 0; j < CodeDesc.Length; j++)
                                                {
                                                    if (CodeDesc[j].Contains(";"))
                                                    {
                                                        string[] Code = CodeDesc[j].Split(new char[] { ';' });
                                                        string[] CodeAlone = Code[1].Split(new char[] { '$' });
                                                        if (Code.Length > 0)
                                                        {
                                                            if (CarePlanLst[i].Plan_Date != "01-01-0001")
                                                            {
                                                                if (CarePlanLst[i].Plan_Date != string.Empty)
                                                                {
                                                                    //commented by saravanakumar on12-02-2014 for change customdatetimepicker
                                                                    if (CarePlanLst[i].Plan_Date.Contains('-'))
                                                                    {
                                                                        string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                                                        if (dates.Length == 2)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime("01-" + dates[0] + dates[1]);
                                                                            AdminsterdDate = Convert.ToDateTime("01-" + dates[1] + dates[0]);
                                                                        }
                                                                        if (dates.Length == 3)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                            AdminsterdDate = Convert.ToDateTime(dates[2] + "-" + dates[1] + "-" + dates[0]);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        AdminsterdDate = Convert.ToDateTime("01-" + "Jan-" + CarePlanLst[i].Plan_Date);
                                                                    }
                                                                    // AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                    CurrentAge = AdminsterdDate.Year - Dob.Year;
                                                                }
                                                            }
                                                            if (Code[0].Contains(">"))
                                                            {
                                                                string[] Codegreater = Code[0].Split(new char[] { '>' });
                                                                if (Codegreater.Length > 0)
                                                                {
                                                                    if (CurrentAge > Convert.ToInt32(Codegreater[1]))
                                                                    {
                                                                        objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                        objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            if (Code[0].Contains("<"))
                                                            {
                                                                if (Code[0].Contains("=") == false)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new char[] { '<' });
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[1]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }

                                                                }
                                                                else if (Code[0].Contains("=") == true)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new string[] { "<=" }, StringSplitOptions.RemoveEmptyEntries);
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[0]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            objImmuHistory.Procedure_Code = measures[0].Where_Criteria.Split(new char[] { '$' })[0];
                                            objImmuHistory.Immunization_Description = measures[0].Where_Criteria.Split(new char[] { '$' })[1];
                                        }

                                        if (CarePlanLst[i].Plan_Date.Contains('-'))
                                        {
                                            string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                            if (dates.Length == 2)
                                                objImmuHistory.Administered_Date = dates[1] + "-" + dates[0];
                                            if (dates.Length == 3)
                                                objImmuHistory.Administered_Date = dates[2] + "-" + dates[1] + "-" + dates[0];
                                        }
                                        else
                                        {
                                            objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        }

                                        //objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        objImmuHistory.Encounter_ID = CarePlanLst[i].Encounter_ID;
                                        objImmuHistory.Created_By = CarePlanLst[i].Created_By;
                                        objImmuHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objImmuHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objImmuHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;

                                        ImmunizationHistoryDTO ImmunizationHistorydto = objImmhstryMngr.GetFromImmunizationHistory(CarePlanLst[0].Human_ID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<ImmunizationHistory> ilstImmunization = new List<ImmunizationHistory>();

                                        if (ImmunizationHistorydto.ImmunizationCount > 0)
                                        {
                                            var immun = from im in ImmunizationHistorydto.Immunization where im.Procedure_Code == objImmuHistory.Procedure_Code select im;

                                            ilstImmunization = immun.ToList<ImmunizationHistory>();
                                            if (ilstImmunization.Count > 0)
                                            {
                                                objImmuHistory = ilstImmunization[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto First;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateImmunization == null)
                                                        ilstUpdateImmunization = new List<ImmunizationHistory>();
                                                    ilstUpdateImmunization.Add(objImmuHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (saveImmunizationHistory == null)
                                                    saveImmunizationHistory = new List<ImmunizationHistory>();
                                                saveImmunizationHistory.Add(objImmuHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (saveImmunizationHistory == null)
                                                saveImmunizationHistory = new List<ImmunizationHistory>();
                                            saveImmunizationHistory.Add(objImmuHistory);
                                        }
                                        break;

                                    case "surgical_history":

                                        bFirst = true;

                                    second: SurgicalHistory objSurgicalHistory = new SurgicalHistory();
                                        objSurgicalHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objSurgicalHistory.Surgery_Name = CarePlanLst[i].Care_Name_Value;
                                        objSurgicalHistory.Date_Of_Surgery = CarePlanLst[i].Plan_Date;
                                        objSurgicalHistory.Description = CarePlanLst[i].Care_Plan_Notes;
                                        objSurgicalHistory.Created_By = CarePlanLst[i].Created_By;
                                        objSurgicalHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objSurgicalHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objSurgicalHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        SurgicalHistoryDTO SurgicalHisoryDetails = objSurgicalhistoryMngr.LoadSurgicalHistory(HumanID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<SurgicalHistory> ilstSurgicalHistory = new List<SurgicalHistory>();

                                        if (SurgicalHisoryDetails.SurgicalCount > 0)
                                        {
                                            var surgical = from sur in SurgicalHisoryDetails.SurgicalList where sur.Description == objSurgicalHistory.Description && sur.Date_Of_Surgery == objSurgicalHistory.Date_Of_Surgery select sur;

                                            ilstSurgicalHistory = surgical.ToList<SurgicalHistory>();
                                            if (ilstSurgicalHistory.Count > 0)
                                            {
                                                objSurgicalHistory = ilstSurgicalHistory[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto second;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateSurgical == null)
                                                        ilstUpdateSurgical = new List<SurgicalHistory>();
                                                    ilstUpdateSurgical.Add(objSurgicalHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (SurgicalSaveUpdateDelete == null)
                                                    SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                                SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (SurgicalSaveUpdateDelete == null)
                                                SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                            SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                        }

                                        break;



                                    case "patient_results":
                                        bFirst = true;
                                    Third: PatientResults objPatient_Results = new PatientResults();
                                        objPatient_Results.Human_ID = CarePlanLst[i].Human_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] ResultsCode = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            objPatient_Results.Loinc_Identifier = ResultsCode[0];
                                            objPatient_Results.Loinc_Observation = ResultsCode[1];
                                            objPatient_Results.Units = ResultsCode[2];
                                            objPatient_Results.Reference_Range = ResultsCode[3];
                                            objPatient_Results.Acurus_Result_Code = ResultsCode[4];
                                            objPatient_Results.Acurus_Result_Description = ResultsCode[5];
                                            objPatient_Results.Results_Type = ResultsCode[6];
                                            //objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            if (CarePlanLst[i].Plan_Date != "")
                                                objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            else
                                            {
                                                if (CarePlanLst[i].Created_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Created_Date_And_Time);
                                                else if (CarePlanLst[i].Modified_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Modified_Date_And_Time);
                                                else
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(DateTime.Now);
                                            }
                                        }
                                        objPatient_Results.Physician_ID = CarePlanLst[i].Physician_ID;
                                        objPatient_Results.Value = CarePlanLst[i].Status_Value;
                                        objPatient_Results.Created_By = CarePlanLst[i].Created_By;
                                        objPatient_Results.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        // savePatientResults.Add(objPatient_Results);

                                        PatientResultsDTO patientResultdto = objPateintresultsMngr.GetPastVitalDetailsByPatientforGrowthChart(HumanID, 1, 10000);
                                        IList<PatientResults> ilstPatientResults = new List<PatientResults>();

                                        if (patientResultdto.VitalCount > 0)
                                        {
                                            var patientres = from resu in patientResultdto.VitalsList where resu.Acurus_Result_Code == objPatient_Results.Acurus_Result_Code && resu.Captured_date_and_time == objPatient_Results.Captured_date_and_time select resu;

                                            ilstPatientResults = patientres.ToList<PatientResults>();
                                            if (ilstPatientResults.Count > 0)
                                            {
                                                objPatient_Results = ilstPatientResults[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto Third;
                                                }
                                                else
                                                {
                                                    if (ilstUpdatePatientResults == null)
                                                        ilstUpdatePatientResults = new List<PatientResults>();
                                                    ilstUpdatePatientResults.Add(objPatient_Results);
                                                }
                                            }
                                            else
                                            {
                                                if (savePatientResults == null)
                                                    savePatientResults = new List<PatientResults>();
                                                savePatientResults.Add(objPatient_Results);
                                            }
                                        }
                                        else
                                        {
                                            if (savePatientResults == null)
                                                savePatientResults = new List<PatientResults>();
                                            savePatientResults.Add(objPatient_Results);
                                        }

                                        break;

                                    case "human":
                                        if (objHuman != null && objHuman.Id == 0)
                                        {
                                            objHuman = hnMngr.GetById(CarePlanLst[i].Human_ID);
                                        }
                                        objHuman.Modified_By = CarePlanLst[i].Modified_By;
                                        if (CarePlanLst[i].Care_Name_Value.Contains("Sexual Orientation"))
                                        {
                                            objHuman.Sexual_Orientation = CarePlanLst[i].Status;
                                            objHuman.Sexual_Orientation_Specify = CarePlanLst[i].Care_Plan_Notes;
                                        }
                                        else if (CarePlanLst[i].Care_Name_Value.Contains("Gender Identity"))
                                        {
                                            objHuman.Gender_Identity = CarePlanLst[i].Status;
                                            objHuman.Gender_Identity_Specify = CarePlanLst[i].Care_Plan_Notes;
                                        }
                                        else

                                            objHuman.Marital_Status = CarePlanLst[i].Status;
                                        objHuman.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        //saveHuman.Add(objHuman);
                                        break;


                                    case "advance_directive":
                                        bFirst = true;
                                    Forur: AdvanceDirective objAD = new AdvanceDirective();
                                        objAD.Human_ID = CarePlanLst[i].Human_ID;
                                        objAD.Encounter_Id = CarePlanLst[i].Encounter_ID;
                                        objAD.Status = CarePlanLst[i].Status;
                                        objAD.Comments = CarePlanLst[i].Care_Plan_Notes;
                                        objAD.Created_By = CarePlanLst[i].Created_By;
                                        objAD.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;

                                        FillOtherHistory AdvanceDirectivedto = phyPatMgr.LoadPhysicianPatient(1, 10000, HumanID, CarePlanLst[0].Encounter_ID, false);
                                        if (AdvanceDirectivedto.Advance_Directive.Count > 0)
                                        {
                                            objAD = AdvanceDirectivedto.Advance_Directive[0];
                                            if (bFirst == true)
                                            {
                                                bFirst = false;
                                                goto Forur;
                                            }
                                            else
                                            {
                                                if (ilstUpdateAD == null)
                                                    ilstUpdateAD = new List<AdvanceDirective>();
                                                ilstUpdateAD.Add(objAD);
                                            }
                                        }
                                        else
                                        {
                                            if (saveAD == null)
                                                saveAD = new List<AdvanceDirective>();
                                            saveAD.Add(objAD);
                                        }
                                        break;


                                }
                            }


                            //  saveImmunizationHistory.Add(objImmuHistory);



                        }
                    }


                }




                //SaveUpdateDeleteWithTransaction(ref CarePlanLst, null, null, MacAddress);
                //IList<CarePlan> CarePlanLstnull = null;
                //  SaveUpdateDelete_DBAndXML_WithTransaction(ref CarePlanLst, ref CarePlanLstnull, null, string.Empty, true, true, CarePlanLst[0].Encounter_ID, string.Empty);
                if (CarePlanLst.Any(a => a.Care_Name_Value == "Marital Status"))
                    SocialHistorySaveOrUpdate(CarePlanLst, MacAddress);

                //objImmhstryMngr.SaveUpdateDeleteWithTransaction(ref saveImmunizationHistory, ilstUpdateImmunization, null, string.Empty);
                if (saveImmunizationHistory != null && saveImmunizationHistory.Count > 0)
                {
                    objImmhstryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveImmunizationHistory, ref ilstUpdateImmunization, null, string.Empty, true, true, saveImmunizationHistory[0].Human_ID, string.Empty);
                }
                //objSurgicalhistoryMngr.SaveUpdateDeleteWithTransaction(ref SurgicalSaveUpdateDelete, ilstUpdateSurgical, null, string.Empty);
                if (SurgicalSaveUpdateDelete != null && SurgicalSaveUpdateDelete.Count > 0)
                {
                    objSurgicalhistoryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref SurgicalSaveUpdateDelete, ref ilstUpdateSurgical, null, string.Empty, true, true, SurgicalSaveUpdateDelete[0].Human_ID, string.Empty);
                }
                // objPateintresultsMngr.SaveUpdateDeleteWithTransaction(ref savePatientResults, ilstUpdatePatientResults, null, string.Empty);
                if (savePatientResults != null && savePatientResults.Count > 0)
                {
                    // objPateintresultsMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref savePatientResults, ref ilstUpdatePatientResults, null, string.Empty, true, false, savePatientResults[0].Human_ID, string.Empty);
                    objPateintresultsMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref savePatientResults, ref ilstUpdatePatientResults, null, string.Empty, true, true, savePatientResults[0].Human_ID, string.Empty);//changes staticbool to true since it removes patientresults tag from xml
                }
                if (objHuman != null && objHuman.Id != 0)
                {
                    saveHuman.Add(objHuman);
                    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, true, false, saveHuman[0].Id, string.Empty);
                }
                //if (saveHuman != null && saveHuman.Count > 0)
                //{
                //    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, true, false, saveHuman[0].Id, string.Empty);
                //}
                //hnMngr.SaveUpdateDeleteWithTransaction(ref insertHuman, saveHuman, null, string.Empty);
                //objADMngr.SaveUpdateDeleteWithTransaction(ref saveAD, ilstUpdateAD, null, string.Empty);
                if (saveAD != null && saveAD.Count > 0)
                {
                    objADMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveAD, ref ilstUpdateAD, null, string.Empty, true, false, saveAD[0].Human_ID, string.Empty);
                }
                iMySession.Close();
            }
            //GenerateXml XMLObj1 = new GenerateXml();
            //if (CarePlanLst.Count > 0)
            //{

            //    ulong encounterid = CarePlanLst[0].Encounter_ID;
            //    List<object> lstObj = CarePlanLst.Cast<object>().ToList();
            //    XMLObj1.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}
            return CarePlanLst;
            //return GetPlanCareFromServerforThin(CarePlanLst[0].Encounter_ID, CarePlanLst[0].Human_ID, sex, iYear);
        }
        public int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }
        public IList<CarePlanDTO> UpdateCarePLan(IList<CarePlan> CarePlanLst, string sSex, int iYear, string MacAddress)
        {
            //IList<MeasuresRuleMaster> lstMeasures = new List<MeasuresRuleMaster>();
            ////ICriteria crt = session.GetISession().CreateCriteria(typeof(MeasuresRuleMaster));
            ////lstMeasures = crt.List<MeasuresRuleMaster>();

            //MeasuresRuleMasterManager measuresMngr = new MeasuresRuleMasterManager();
            //lstMeasures = measuresMngr.GetAll();

            //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
            //ImmunizationHistoryManager objImmhstryMngr = new ImmunizationHistoryManager();

            //IList<PatientResults> savePatientResults = new List<PatientResults>();
            //VitalsManager objPateintresultsMngr = new VitalsManager();

            //IList<SurgicalHistory> SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
            //SurgicalHistoryManager objSurgicalhistoryMngr = new SurgicalHistoryManager();

            //IList<Human> saveHuman = new List<Human>();
            //IList<AdvanceDirective> saveAD = new List<AdvanceDirective>();
            //AdvanceDirectiveManager objADMngr = new AdvanceDirectiveManager();
            //HumanManager hnMngr = new HumanManager();

            //IList<Human> insertHuman = null;

            ////IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
            ////IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
            //DateTime Dob = DateTime.MinValue;
            //DateTime AdminsterdDate = DateTime.MinValue;
            //int CurrentAge = 0;
            //if (CarePlanLst != null && CarePlanLst.Count > 0)
            //{
            //    ulong HumanID = 0;
            //    HumanID = CarePlanLst[0].Human_ID;
            //    if (HumanID != 0)
            //    {
            //        ISQLQuery sqlqueryHuman = session.GetISession().CreateSQLQuery("select h.* from human h   where h.Human_ID='" + HumanID + "'").AddEntity("h", typeof(Human));
            //        IList<Human> lstHuman = sqlqueryHuman.List<Human>();

            //        double humanAgeInMonths = 0;
            //        int humanAgeInYears = 0;
            //        if (lstHuman != null && lstHuman.Count > 0)
            //        {
            //            Dob = lstHuman[0].Birth_Date;
            //            humanAgeInMonths = ((DateTime.Now.Date - lstHuman[0].Birth_Date.Date).TotalDays) / 30.4375;
            //            CarePlanManager careMngr = new CarePlanManager();
            //            humanAgeInYears = careMngr.CalculateAge(lstHuman[0].Birth_Date);
            //        }
            //    }

            //    for (int i = 0; i < CarePlanLst.Count; i++)
            //    {
            //        if (CarePlanLst[i].bInsert == true)
            //        {

            //            var res = from m in lstMeasures where m.Measure_Name == CarePlanLst[i].Care_Name_Value select m;
            //            IList<MeasuresRuleMaster> measures = res.ToList<MeasuresRuleMaster>();
            //            if (measures.Count > 0)
            //            {
            //                switch (measures[0].Table_Name)
            //                {
            //                    case "immunization_history":

            //                        ImmunizationHistory objImmuHistory = new ImmunizationHistory();
            //                        objImmuHistory.Human_ID = CarePlanLst[i].Human_ID;
            //                        objImmuHistory.Physician_ID = CarePlanLst[i].Physician_ID;
            //                        if (measures[0].Where_Criteria.Contains("|"))
            //                        {
            //                            string[] CodeDesc = measures[0].Where_Criteria.Split(new char[] { '|' });
            //                            if (CodeDesc.Length > 0)
            //                            {
            //                                for (int j = 0; j < CodeDesc.Length; j++)
            //                                {
            //                                    if (CodeDesc[j].Contains(";"))
            //                                    {
            //                                        string[] Code = CodeDesc[j].Split(new char[] { ';' });
            //                                        string[] CodeAlone = Code[1].Split(new char[] { '$' });
            //                                        if (Code.Length > 0)
            //                                        {
            //                                            if (CarePlanLst[i].Plan_Date != "01-01-0001")
            //                                            {
            //                                                if (CarePlanLst[i].Plan_Date != string.Empty)
            //                                                {
            //                                                    if (CarePlanLst[i].Plan_Date.Contains('-'))
            //                                                    {
            //                                                        string[] dates = CarePlanLst[i].Plan_Date.Split('-');
            //                                                        if (dates.Length == 2)
            //                                                        {
            //                                                            //AdminsterdDate = Convert.ToDateTime("01-" + dates[0] + dates[1]);
            //                                                            AdminsterdDate = Convert.ToDateTime("01-" + dates[1] + dates[0]);
            //                                                        }
            //                                                        if (dates.Length == 3)
            //                                                        {
            //                                                            //AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
            //                                                            AdminsterdDate = Convert.ToDateTime(dates[2] + "-" + dates[1] + "-" + dates[0]); 
            //                                                        }
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        AdminsterdDate = Convert.ToDateTime("01-" + "Jan-" + CarePlanLst[i].Plan_Date);
            //                                                    }
            //                                                    CurrentAge = AdminsterdDate.Year - Dob.Year;
            //                                                }
            //                                            }
            //                                            if (Code[0].Contains(">"))
            //                                            {
            //                                                string[] Codegreater = Code[0].Split(new char[] { '>' });
            //                                                if (Codegreater.Length > 0)
            //                                                {
            //                                                    if (CurrentAge > Convert.ToInt32(Codegreater[1]))
            //                                                    {
            //                                                        objImmuHistory.Procedure_Code = CodeAlone[0]; ;
            //                                                        objImmuHistory.Immunization_Description = CodeAlone[1]; ;
            //                                                        break;
            //                                                    }
            //                                                }
            //                                            }
            //                                            if (Code[0].Contains("<"))
            //                                            {
            //                                                if (Code[0].Contains("=") == false)
            //                                                {
            //                                                    string[] Codelesser = Code[0].Split(new char[] { '<' });
            //                                                    if (Codelesser.Length > 0)
            //                                                    {
            //                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[1]))
            //                                                        {
            //                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
            //                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
            //                                                            break;
            //                                                        }
            //                                                    }
            //                                                }
            //                                                else if (Code[0].Contains("=") == true)
            //                                                {
            //                                                    string[] Codelesser = Code[0].Split(new string[] { "<=" }, StringSplitOptions.RemoveEmptyEntries);
            //                                                    if (Codelesser.Length > 0)
            //                                                    {
            //                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[0]))
            //                                                        {
            //                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
            //                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
            //                                                            break;
            //                                                        }
            //                                                    }
            //                                                }
            //                                            }
            //                                        }
            //                                    }
            //                                }

            //                            }
            //                        }
            //                        else
            //                        {
            //                            objImmuHistory.Procedure_Code = measures[0].Where_Criteria.Split(new char[] { '$' })[0];
            //                            objImmuHistory.Immunization_Description = measures[0].Where_Criteria.Split(new char[] { '$' })[1];
            //                        }


            //                        //objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
            //                        if (CarePlanLst[i].Plan_Date.Contains('-'))
            //                        {
            //                            string[] dates = CarePlanLst[i].Plan_Date.Split('-');
            //                            if (dates.Length == 2)
            //                                objImmuHistory.Administered_Date = dates[1]+"-" + dates[0];
            //                             if (dates.Length == 3)
            //                                  objImmuHistory.Administered_Date =dates[2] + "-" + dates[1] + "-" + dates[0];
            //                        }
            //                        else
            //                        {
            //                            objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
            //                        }
            //                        objImmuHistory.Created_By = CarePlanLst[i].Created_By;
            //                        objImmuHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
            //                        objImmuHistory.Modified_By = CarePlanLst[i].Modified_By;
            //                        objImmuHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
            //                        saveImmunizationHistory.Add(objImmuHistory);
            //                        break;

            //                    case "surgical_history":

            //                        SurgicalHistory objSurgicalHistory = new SurgicalHistory();
            //                        objSurgicalHistory.Human_ID = CarePlanLst[i].Human_ID;
            //                        objSurgicalHistory.Surgery_Name = CarePlanLst[i].Care_Name_Value;
            //                        objSurgicalHistory.Date_Of_Surgery = CarePlanLst[i].Plan_Date;
            //                        objSurgicalHistory.Description = CarePlanLst[i].Care_Plan_Notes;
            //                        objSurgicalHistory.Created_By = CarePlanLst[i].Created_By;
            //                        objSurgicalHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
            //                        objSurgicalHistory.Modified_By = CarePlanLst[i].Modified_By;
            //                        objSurgicalHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
            //                        SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
            //                        break;



            //                    case "patient_results":
            //                        PatientResults objPatient_Results = new PatientResults();
            //                        objPatient_Results.Human_ID = CarePlanLst[i].Human_ID;
            //                        if (measures[0].Where_Criteria.Contains("|"))
            //                        {
            //                            string[] ResultsCode = measures[0].Where_Criteria.Split(new char[] { '|' });
            //                            objPatient_Results.Loinc_Identifier = ResultsCode[0];
            //                            objPatient_Results.Loinc_Observation = ResultsCode[1];
            //                            objPatient_Results.Units = ResultsCode[2];
            //                            objPatient_Results.Reference_Range = ResultsCode[3];
            //                            objPatient_Results.Acurus_Result_Code = ResultsCode[4];
            //                            objPatient_Results.Acurus_Result_Description = ResultsCode[5];
            //                            objPatient_Results.Results_Type = ResultsCode[6];
            //                        }
            //                        objPatient_Results.Physician_ID = CarePlanLst[i].Physician_ID;
            //                        objPatient_Results.Value = CarePlanLst[i].Status_Value;
            //                        objPatient_Results.Created_By = CarePlanLst[i].Created_By;
            //                        objPatient_Results.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
            //                        savePatientResults.Add(objPatient_Results);
            //                        break;

            //                    case "human":
            //                        Human objHuman = new Human();
            //                        objHuman.Id = CarePlanLst[i].Human_ID;

            //                        objHuman = hnMngr.GetById(CarePlanLst[i].Human_ID);
            //                        objHuman.Marital_Status = CarePlanLst[i].Status;
            //                        objHuman.Modified_By = CarePlanLst[i].Modified_By;
            //                        objHuman.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
            //                        saveHuman.Add(objHuman);
            //                        break;

            //                    case "advance_directive":
            //                        AdvanceDirective objAD = new AdvanceDirective();
            //                        objAD.Human_ID = CarePlanLst[i].Human_ID;
            //                        objAD.Status = CarePlanLst[i].Status;
            //                        objAD.Comments = CarePlanLst[i].Care_Plan_Notes;
            //                        objAD.Created_By = CarePlanLst[i].Created_By;
            //                        objAD.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
            //                        saveAD.Add(objAD);
            //                        break;


            //                }
            //            }


            //            //  saveImmunizationHistory.Add(objImmuHistory);



            //        }
            //    }


            //}


            IList<MeasuresRuleMaster> lstMeasures = new List<MeasuresRuleMaster>();

            MeasuresRuleMasterManager measuresMngr = new MeasuresRuleMasterManager();
            lstMeasures = measuresMngr.GetAll();

            IList<ImmunizationHistory> saveImmunizationHistory = null;
            ImmunizationHistoryManager objImmhstryMngr = new ImmunizationHistoryManager();

            IList<PatientResults> savePatientResults = null;
            VitalsManager objPateintresultsMngr = new VitalsManager();

            IList<SurgicalHistory> SurgicalSaveUpdateDelete = null;
            SurgicalHistoryManager objSurgicalhistoryMngr = new SurgicalHistoryManager();

            IList<Human> saveHuman = new List<Human>();
            Human objHuman = new Human();
            IList<AdvanceDirective> saveAD = null;
            AdvanceDirectiveManager objADMngr = new AdvanceDirectiveManager();
            HumanManager hnMngr = new HumanManager();

            IList<Human> insertHuman = null;
            IList<ImmunizationHistory> ilstUpdateImmunization = null;
            IList<SurgicalHistory> ilstUpdateSurgical = null;
            IList<PatientResults> ilstUpdatePatientResults = null;
            IList<AdvanceDirective> ilstUpdateAD = null;
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                PhysicianPatientManager phyPatMgr = new PhysicianPatientManager();

                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                DateTime Dob = DateTime.MinValue;
                DateTime AdminsterdDate = DateTime.MinValue;
                int CurrentAge = 0;
                if (CarePlanLst != null && CarePlanLst.Count > 0)
                {
                    ulong HumanID = 0;
                    HumanID = CarePlanLst[0].Human_ID;
                    if (HumanID != 0)
                    {

                        ISQLQuery sqlqueryHuman = iMySession.CreateSQLQuery("select h.* from human h   where h.Human_ID='" + HumanID + "'").AddEntity("h", typeof(Human));
                        IList<Human> lstHuman = sqlqueryHuman.List<Human>();

                        double humanAgeInMonths = 0;
                        int humanAgeInYears = 0;
                        if (lstHuman != null && lstHuman.Count > 0)
                        {
                            Dob = lstHuman[0].Birth_Date;
                            humanAgeInMonths = ((DateTime.Now.Date - lstHuman[0].Birth_Date.Date).TotalDays) / 30.4375;
                            CarePlanManager careMngr = new CarePlanManager();
                            humanAgeInYears = careMngr.CalculateAge(lstHuman[0].Birth_Date);
                        }
                    }

                    for (int i = 0; i < CarePlanLst.Count; i++)
                    {
                        if (CarePlanLst[i].Internal_Property_bInsert == true)
                        {

                            var res = from m in lstMeasures where m.Measure_Name == CarePlanLst[i].Care_Name_Value select m;
                            IList<MeasuresRuleMaster> measures = res.ToList<MeasuresRuleMaster>();
                            if (measures.Count > 0)
                            {
                                switch (measures[0].Table_Name)
                                {
                                    case "immunization_history":

                                        Boolean bFirst = true;

                                        ImmunizationHistory objImmuHistory = new ImmunizationHistory();

                                    First:
                                        objImmuHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objImmuHistory.Physician_ID = CarePlanLst[i].Physician_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] CodeDesc = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            if (CodeDesc.Length > 0)
                                            {
                                                for (int j = 0; j < CodeDesc.Length; j++)
                                                {
                                                    if (CodeDesc[j].Contains(";"))
                                                    {
                                                        string[] Code = CodeDesc[j].Split(new char[] { ';' });
                                                        string[] CodeAlone = Code[1].Split(new char[] { '$' });
                                                        if (Code.Length > 0)
                                                        {
                                                            if (CarePlanLst[i].Plan_Date != "01-01-0001")
                                                            {
                                                                if (CarePlanLst[i].Plan_Date != string.Empty)
                                                                {
                                                                    //commented by saravanakumar on12-02-2014 for change customdatetimepicker
                                                                    if (CarePlanLst[i].Plan_Date.Contains('-'))
                                                                    {
                                                                        string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                                                        if (dates.Length == 2)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime("01-" + dates[0] + dates[1]);
                                                                            AdminsterdDate = Convert.ToDateTime("01-" + dates[1] + dates[0]);
                                                                        }
                                                                        if (dates.Length == 3)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                            AdminsterdDate = Convert.ToDateTime(dates[2] + "-" + dates[1] + "-" + dates[0]);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        AdminsterdDate = Convert.ToDateTime("01-" + "Jan-" + CarePlanLst[i].Plan_Date);
                                                                    }
                                                                    // AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                    CurrentAge = AdminsterdDate.Year - Dob.Year;
                                                                }
                                                            }
                                                            if (Code[0].Contains(">"))
                                                            {
                                                                string[] Codegreater = Code[0].Split(new char[] { '>' });
                                                                if (Codegreater.Length > 0)
                                                                {
                                                                    if (CurrentAge > Convert.ToInt32(Codegreater[1]))
                                                                    {
                                                                        objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                        objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            if (Code[0].Contains("<"))
                                                            {
                                                                if (Code[0].Contains("=") == false)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new char[] { '<' });
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[1]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }

                                                                }
                                                                else if (Code[0].Contains("=") == true)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new string[] { "<=" }, StringSplitOptions.RemoveEmptyEntries);
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[0]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            objImmuHistory.Procedure_Code = measures[0].Where_Criteria.Split(new char[] { '$' })[0];
                                            objImmuHistory.Immunization_Description = measures[0].Where_Criteria.Split(new char[] { '$' })[1];
                                        }

                                        if (CarePlanLst[i].Plan_Date.Contains('-'))
                                        {
                                            string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                            if (dates.Length == 2)
                                                objImmuHistory.Administered_Date = dates[1] + "-" + dates[0];
                                            if (dates.Length == 3)
                                                objImmuHistory.Administered_Date = dates[2] + "-" + dates[1] + "-" + dates[0];
                                        }
                                        else
                                        {
                                            objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        }

                                        //objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        objImmuHistory.Encounter_ID = CarePlanLst[i].Encounter_ID;
                                        objImmuHistory.Created_By = CarePlanLst[i].Created_By;
                                        objImmuHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objImmuHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objImmuHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;

                                        ImmunizationHistoryDTO ImmunizationHistorydto = objImmhstryMngr.GetFromImmunizationHistory(CarePlanLst[0].Human_ID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<ImmunizationHistory> ilstImmunization = new List<ImmunizationHistory>();

                                        if (ImmunizationHistorydto.ImmunizationCount > 0)
                                        {
                                            var immun = from im in ImmunizationHistorydto.Immunization where im.Procedure_Code == objImmuHistory.Procedure_Code && im.Encounter_ID == objImmuHistory.Encounter_ID select im;
                                            ilstImmunization = immun.ToList<ImmunizationHistory>();
                                            if (ilstImmunization.Count > 0)
                                            {
                                                objImmuHistory = ilstImmunization[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto First;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateImmunization == null)
                                                        ilstUpdateImmunization = new List<ImmunizationHistory>();
                                                    ilstUpdateImmunization.Add(objImmuHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (saveImmunizationHistory == null)
                                                    saveImmunizationHistory = new List<ImmunizationHistory>();
                                                saveImmunizationHistory.Add(objImmuHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (saveImmunizationHistory == null)
                                                saveImmunizationHistory = new List<ImmunizationHistory>();
                                            saveImmunizationHistory.Add(objImmuHistory);
                                        }
                                        break;

                                    case "surgical_history":

                                        bFirst = true;

                                    second: SurgicalHistory objSurgicalHistory = new SurgicalHistory();
                                        objSurgicalHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objSurgicalHistory.Surgery_Name = CarePlanLst[i].Care_Name_Value;
                                        objSurgicalHistory.Date_Of_Surgery = CarePlanLst[i].Plan_Date;
                                        objSurgicalHistory.Description = CarePlanLst[i].Care_Plan_Notes;
                                        objSurgicalHistory.Created_By = CarePlanLst[i].Created_By;
                                        objSurgicalHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objSurgicalHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objSurgicalHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        //SurgicalHistoryDTO SurgicalHisoryDetails = objSurgicalhistoryMngr.LoadSurgicalHistory(HumanID, 1, 10000);
                                        SurgicalHistoryDTO SurgicalHisoryDetails = objSurgicalhistoryMngr.LoadSurgicalHistory(HumanID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<SurgicalHistory> ilstSurgicalHistory = new List<SurgicalHistory>();

                                        if (SurgicalHisoryDetails.SurgicalCount > 0)
                                        {
                                            var surgical = from sur in SurgicalHisoryDetails.SurgicalList where sur.Description == objSurgicalHistory.Description && sur.Date_Of_Surgery == objSurgicalHistory.Date_Of_Surgery select sur;

                                            ilstSurgicalHistory = surgical.ToList<SurgicalHistory>();
                                            if (ilstSurgicalHistory.Count > 0)
                                            {
                                                objSurgicalHistory = ilstSurgicalHistory[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto second;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateSurgical == null)
                                                        ilstUpdateSurgical = new List<SurgicalHistory>();
                                                    ilstUpdateSurgical.Add(objSurgicalHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (SurgicalSaveUpdateDelete == null)
                                                    SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                                SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (SurgicalSaveUpdateDelete == null)
                                                SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                            SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                        }

                                        break;



                                    case "patient_results":
                                        bFirst = true;
                                    Third: PatientResults objPatient_Results = new PatientResults();
                                        objPatient_Results.Human_ID = CarePlanLst[i].Human_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] ResultsCode = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            objPatient_Results.Loinc_Identifier = ResultsCode[0];
                                            objPatient_Results.Loinc_Observation = ResultsCode[1];
                                            objPatient_Results.Units = ResultsCode[2];
                                            objPatient_Results.Reference_Range = ResultsCode[3];
                                            objPatient_Results.Acurus_Result_Code = ResultsCode[4];
                                            objPatient_Results.Acurus_Result_Description = ResultsCode[5];
                                            objPatient_Results.Results_Type = ResultsCode[6];
                                            //objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            if (CarePlanLst[i].Plan_Date != "")
                                                objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            else
                                            {
                                                if (CarePlanLst[i].Created_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Created_Date_And_Time);
                                                else if (CarePlanLst[i].Modified_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Modified_Date_And_Time);
                                                else
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(DateTime.Now);
                                            }
                                        }
                                        objPatient_Results.Physician_ID = CarePlanLst[i].Physician_ID;
                                        objPatient_Results.Value = CarePlanLst[i].Status_Value;
                                        objPatient_Results.Created_By = CarePlanLst[i].Created_By;
                                        objPatient_Results.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        // savePatientResults.Add(objPatient_Results);

                                        PatientResultsDTO patientResultdto = objPateintresultsMngr.GetPastVitalDetailsByPatientforGrowthChart(HumanID, 1, 10000);
                                        IList<PatientResults> ilstPatientResults = new List<PatientResults>();

                                        if (patientResultdto.VitalCount > 0)
                                        {
                                            var patientres = from resu in patientResultdto.VitalsList where resu.Acurus_Result_Code == objPatient_Results.Acurus_Result_Code && resu.Captured_date_and_time == objPatient_Results.Captured_date_and_time select resu;

                                            ilstPatientResults = patientres.ToList<PatientResults>();
                                            if (ilstPatientResults.Count > 0)
                                            {
                                                objPatient_Results = ilstPatientResults[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto Third;
                                                }
                                                else
                                                {
                                                    if (ilstUpdatePatientResults == null)
                                                        ilstUpdatePatientResults = new List<PatientResults>();
                                                    ilstUpdatePatientResults.Add(objPatient_Results);
                                                }
                                            }
                                            else
                                            {
                                                if (savePatientResults == null)
                                                    savePatientResults = new List<PatientResults>();
                                                savePatientResults.Add(objPatient_Results);
                                            }
                                        }
                                        else
                                        {
                                            if (savePatientResults == null)
                                                savePatientResults = new List<PatientResults>();
                                            savePatientResults.Add(objPatient_Results);
                                        }

                                        break;

                                    case "human":
                                        //Human objHuman = new Human();
                                        //objHuman.Id = CarePlanLst[i].Human_ID;

                                        if (objHuman != null && objHuman.Id == 0)
                                        {
                                            objHuman = hnMngr.GetById(CarePlanLst[i].Human_ID);
                                        }


                                        objHuman.Modified_By = CarePlanLst[i].Modified_By;
                                        objHuman.Marital_Status = CarePlanLst[i].Status;
                                        objHuman.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        //saveHuman.Add(objHuman);
                                        break;

                                    case "advance_directive":
                                        bFirst = true;
                                    Forur: AdvanceDirective objAD = new AdvanceDirective();
                                        objAD.Human_ID = CarePlanLst[i].Human_ID;
                                        objAD.Encounter_Id = CarePlanLst[i].Encounter_ID;
                                        objAD.Status = CarePlanLst[i].Status;
                                        objAD.Comments = CarePlanLst[i].Care_Plan_Notes;
                                        objAD.Created_By = CarePlanLst[i].Created_By;
                                        objAD.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;

                                        FillOtherHistory AdvanceDirectivedto = phyPatMgr.LoadPhysicianPatient(1, 10000, HumanID, CarePlanLst[0].Encounter_ID, false);
                                        if (AdvanceDirectivedto.Advance_Directive.Count > 0)
                                        {
                                            objAD = AdvanceDirectivedto.Advance_Directive[0];
                                            if (bFirst == true)
                                            {
                                                bFirst = false;
                                                goto Forur;
                                            }
                                            else
                                            {
                                                if (ilstUpdateAD == null)
                                                    ilstUpdateAD = new List<AdvanceDirective>();
                                                ilstUpdateAD.Add(objAD);
                                            }
                                        }
                                        else
                                        {
                                            if (saveAD == null)
                                                saveAD = new List<AdvanceDirective>();
                                            saveAD.Add(objAD);
                                        }
                                        break;


                                }
                            }


                            //  saveImmunizationHistory.Add(objImmuHistory);



                        }
                    }


                }

                IList<CarePlan> Nulllist = null;
                //SaveUpdateDeleteWithTransaction(ref Nulllist, CarePlanLst, null, MacAddress);
                SaveUpdateDelete_DBAndXML_WithTransaction(ref Nulllist, ref CarePlanLst, null, MacAddress, true, false, CarePlanLst[0].Encounter_ID, string.Empty);
                if (CarePlanLst.Any(a => a.Care_Name_Value == "Marital Status"))
                    SocialHistorySaveOrUpdate(CarePlanLst, MacAddress);

                objImmhstryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveImmunizationHistory, ref ilstUpdateImmunization, null, string.Empty, false, false, 0, string.Empty);
                //objImmhstryMngr.SaveUpdateDeleteWithTransaction(ref saveImmunizationHistory, ilstUpdateImmunization, null, string.Empty);
                //objSurgicalhistoryMngr.SaveUpdateDeleteWithTransaction(ref SurgicalSaveUpdateDelete, ilstUpdateSurgical, null, string.Empty);
                objSurgicalhistoryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref SurgicalSaveUpdateDelete, ref ilstUpdateSurgical, null, string.Empty, false, false, 0, string.Empty);
                //objPateintresultsMngr.SaveUpdateDeleteWithTransaction(ref savePatientResults, ilstUpdatePatientResults, null, string.Empty);
                objPateintresultsMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref savePatientResults, ref ilstUpdatePatientResults, null, string.Empty, false, false, 0, string.Empty);
                //if (saveHuman.Count > 0)
                //    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, false, false, 0, string.Empty);
                if (objHuman != null && objHuman.Id != 0)
                {
                    saveHuman.Add(objHuman);
                    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, false, false, 0, string.Empty);
                }
                //hnMngr.SaveUpdateDeleteWithTransaction(ref insertHuman, saveHuman, null, string.Empty);
                //objADMngr.SaveUpdateDeleteWithTransaction(ref saveAD, ilstUpdateAD, null, string.Empty);
                objADMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveAD, ref ilstUpdateAD, null, string.Empty, false, false, 0, string.Empty);
                iMySession.Close();
            }
            //GenerateXml XMLObj = new GenerateXml();
            //if (CarePlanLst.Count > 0)
            //{

            //    ulong encounterid = CarePlanLst[0].Encounter_ID;
            //    List<object> lstObj = CarePlanLst.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}

            return GetPlanCareFromServerforThin(CarePlanLst[0].Encounter_ID, CarePlanLst[0].Human_ID, sSex, iYear);
        }
        public IList<CarePlan> UpdateCarePLanXML(IList<CarePlan> CarePlanLst, string sSex, int iYear, string MacAddress)
        {

            IList<MeasuresRuleMaster> lstMeasures = new List<MeasuresRuleMaster>();

            MeasuresRuleMasterManager measuresMngr = new MeasuresRuleMasterManager();
            lstMeasures = measuresMngr.GetAll();

            IList<ImmunizationHistory> saveImmunizationHistory = null;
            ImmunizationHistoryManager objImmhstryMngr = new ImmunizationHistoryManager();

            IList<PatientResults> savePatientResults = null;
            VitalsManager objPateintresultsMngr = new VitalsManager();

            IList<SurgicalHistory> SurgicalSaveUpdateDelete = null;
            SurgicalHistoryManager objSurgicalhistoryMngr = new SurgicalHistoryManager();

            IList<Human> saveHuman = new List<Human>();
            Human objHuman = new Human();
            IList<AdvanceDirective> saveAD = null;
            AdvanceDirectiveManager objADMngr = new AdvanceDirectiveManager();
            HumanManager hnMngr = new HumanManager();

            IList<Human> insertHuman = null;
            IList<ImmunizationHistory> ilstUpdateImmunization = null;
            IList<SurgicalHistory> ilstUpdateSurgical = null;
            IList<PatientResults> ilstUpdatePatientResults = null;
            IList<AdvanceDirective> ilstUpdateAD = null;
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                PhysicianPatientManager phyPatMgr = new PhysicianPatientManager();

                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                DateTime Dob = DateTime.MinValue;
                DateTime AdminsterdDate = DateTime.MinValue;
                int CurrentAge = 0;
                if (CarePlanLst != null && CarePlanLst.Count > 0)
                {
                    ulong HumanID = 0;
                    HumanID = CarePlanLst[0].Human_ID;
                    if (HumanID != 0)
                    {

                        ISQLQuery sqlqueryHuman = iMySession.CreateSQLQuery("select h.* from human h   where h.Human_ID='" + HumanID + "'").AddEntity("h", typeof(Human));
                        IList<Human> lstHuman = sqlqueryHuman.List<Human>();

                        double humanAgeInMonths = 0;
                        int humanAgeInYears = 0;
                        if (lstHuman != null && lstHuman.Count > 0)
                        {
                            Dob = lstHuman[0].Birth_Date;
                            humanAgeInMonths = ((DateTime.Now.Date - lstHuman[0].Birth_Date.Date).TotalDays) / 30.4375;
                            CarePlanManager careMngr = new CarePlanManager();
                            humanAgeInYears = careMngr.CalculateAge(lstHuman[0].Birth_Date);
                        }
                    }

                    for (int i = 0; i < CarePlanLst.Count; i++)
                    {
                        if (CarePlanLst[i].Internal_Property_bInsert == true)
                        {

                            var res = from m in lstMeasures where m.Measure_Name == CarePlanLst[i].Care_Name_Value select m;
                            IList<MeasuresRuleMaster> measures = res.ToList<MeasuresRuleMaster>();
                            if (measures.Count > 0)
                            {
                                switch (measures[0].Table_Name)
                                {
                                    case "immunization_history":

                                        Boolean bFirst = true;

                                        ImmunizationHistory objImmuHistory = new ImmunizationHistory();

                                    First:
                                        objImmuHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objImmuHistory.Physician_ID = CarePlanLst[i].Physician_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] CodeDesc = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            if (CodeDesc.Length > 0)
                                            {
                                                for (int j = 0; j < CodeDesc.Length; j++)
                                                {
                                                    if (CodeDesc[j].Contains(";"))
                                                    {
                                                        string[] Code = CodeDesc[j].Split(new char[] { ';' });
                                                        string[] CodeAlone = Code[1].Split(new char[] { '$' });
                                                        if (Code.Length > 0)
                                                        {
                                                            if (CarePlanLst[i].Plan_Date != "01-01-0001")
                                                            {
                                                                if (CarePlanLst[i].Plan_Date != string.Empty)
                                                                {
                                                                    //commented by saravanakumar on12-02-2014 for change customdatetimepicker
                                                                    if (CarePlanLst[i].Plan_Date.Contains('-'))
                                                                    {
                                                                        string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                                                        if (dates.Length == 2)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime("01-" + dates[0] + dates[1]);
                                                                            AdminsterdDate = Convert.ToDateTime("01-" + dates[1] + dates[0]);
                                                                        }
                                                                        if (dates.Length == 3)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                            AdminsterdDate = Convert.ToDateTime(dates[2] + "-" + dates[1] + "-" + dates[0]);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        AdminsterdDate = Convert.ToDateTime("01-" + "Jan-" + CarePlanLst[i].Plan_Date);
                                                                    }
                                                                    // AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                    CurrentAge = AdminsterdDate.Year - Dob.Year;
                                                                }
                                                            }
                                                            if (Code[0].Contains(">"))
                                                            {
                                                                string[] Codegreater = Code[0].Split(new char[] { '>' });
                                                                if (Codegreater.Length > 0)
                                                                {
                                                                    if (CurrentAge > Convert.ToInt32(Codegreater[1]))
                                                                    {
                                                                        objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                        objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            if (Code[0].Contains("<"))
                                                            {
                                                                if (Code[0].Contains("=") == false)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new char[] { '<' });
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[1]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }

                                                                }
                                                                else if (Code[0].Contains("=") == true)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new string[] { "<=" }, StringSplitOptions.RemoveEmptyEntries);
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[0]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            objImmuHistory.Procedure_Code = measures[0].Where_Criteria.Split(new char[] { '$' })[0];
                                            objImmuHistory.Immunization_Description = measures[0].Where_Criteria.Split(new char[] { '$' })[1];
                                        }

                                        if (CarePlanLst[i].Plan_Date.Contains('-'))
                                        {
                                            string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                            if (dates.Length == 2)
                                                objImmuHistory.Administered_Date = dates[1] + "-" + dates[0];
                                            if (dates.Length == 3)
                                                objImmuHistory.Administered_Date = dates[2] + "-" + dates[1] + "-" + dates[0];
                                        }
                                        else
                                        {
                                            objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        }

                                        //objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        objImmuHistory.Encounter_ID = CarePlanLst[i].Encounter_ID;
                                        objImmuHistory.Created_By = CarePlanLst[i].Created_By;
                                        objImmuHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objImmuHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objImmuHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;

                                        ImmunizationHistoryDTO ImmunizationHistorydto = objImmhstryMngr.GetFromImmunizationHistory(CarePlanLst[0].Human_ID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<ImmunizationHistory> ilstImmunization = new List<ImmunizationHistory>();

                                        if (ImmunizationHistorydto.ImmunizationCount > 0)
                                        {
                                            var immun = from im in ImmunizationHistorydto.Immunization where im.Procedure_Code == objImmuHistory.Procedure_Code && im.Encounter_ID == objImmuHistory.Encounter_ID select im;
                                            ilstImmunization = immun.ToList<ImmunizationHistory>();
                                            if (ilstImmunization.Count > 0)
                                            {
                                                objImmuHistory = ilstImmunization[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto First;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateImmunization == null)
                                                        ilstUpdateImmunization = new List<ImmunizationHistory>();
                                                    ilstUpdateImmunization.Add(objImmuHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (saveImmunizationHistory == null)
                                                    saveImmunizationHistory = new List<ImmunizationHistory>();
                                                saveImmunizationHistory.Add(objImmuHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (saveImmunizationHistory == null)
                                                saveImmunizationHistory = new List<ImmunizationHistory>();
                                            saveImmunizationHistory.Add(objImmuHistory);
                                        }
                                        break;

                                    case "surgical_history":

                                        bFirst = true;

                                    second: SurgicalHistory objSurgicalHistory = new SurgicalHistory();
                                        objSurgicalHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objSurgicalHistory.Surgery_Name = CarePlanLst[i].Care_Name_Value;
                                        objSurgicalHistory.Date_Of_Surgery = CarePlanLst[i].Plan_Date;
                                        objSurgicalHistory.Description = CarePlanLst[i].Care_Plan_Notes;
                                        objSurgicalHistory.Created_By = CarePlanLst[i].Created_By;
                                        objSurgicalHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objSurgicalHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objSurgicalHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        //SurgicalHistoryDTO SurgicalHisoryDetails = objSurgicalhistoryMngr.LoadSurgicalHistory(HumanID, 1, 10000);
                                        SurgicalHistoryDTO SurgicalHisoryDetails = objSurgicalhistoryMngr.LoadSurgicalHistory(HumanID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<SurgicalHistory> ilstSurgicalHistory = new List<SurgicalHistory>();

                                        if (SurgicalHisoryDetails.SurgicalCount > 0)
                                        {
                                            var surgical = from sur in SurgicalHisoryDetails.SurgicalList where sur.Description == objSurgicalHistory.Description && sur.Date_Of_Surgery == objSurgicalHistory.Date_Of_Surgery select sur;

                                            ilstSurgicalHistory = surgical.ToList<SurgicalHistory>();
                                            if (ilstSurgicalHistory.Count > 0)
                                            {
                                                objSurgicalHistory = ilstSurgicalHistory[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto second;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateSurgical == null)
                                                        ilstUpdateSurgical = new List<SurgicalHistory>();
                                                    ilstUpdateSurgical.Add(objSurgicalHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (SurgicalSaveUpdateDelete == null)
                                                    SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                                SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (SurgicalSaveUpdateDelete == null)
                                                SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                            SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                        }

                                        break;



                                    case "patient_results":
                                        bFirst = true;
                                    Third: PatientResults objPatient_Results = new PatientResults();
                                        objPatient_Results.Human_ID = CarePlanLst[i].Human_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] ResultsCode = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            objPatient_Results.Loinc_Identifier = ResultsCode[0];
                                            objPatient_Results.Loinc_Observation = ResultsCode[1];
                                            objPatient_Results.Units = ResultsCode[2];
                                            objPatient_Results.Reference_Range = ResultsCode[3];
                                            objPatient_Results.Acurus_Result_Code = ResultsCode[4];
                                            objPatient_Results.Acurus_Result_Description = ResultsCode[5];
                                            objPatient_Results.Results_Type = ResultsCode[6];
                                            //objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            if (CarePlanLst[i].Plan_Date != "")
                                                objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            else
                                            {
                                                if (CarePlanLst[i].Created_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Created_Date_And_Time);
                                                else if (CarePlanLst[i].Modified_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Modified_Date_And_Time);
                                                else
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(DateTime.Now);
                                            }
                                        }
                                        objPatient_Results.Physician_ID = CarePlanLst[i].Physician_ID;
                                        objPatient_Results.Value = CarePlanLst[i].Status_Value;
                                        objPatient_Results.Created_By = CarePlanLst[i].Created_By;
                                        objPatient_Results.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        // savePatientResults.Add(objPatient_Results);

                                        PatientResultsDTO patientResultdto = objPateintresultsMngr.GetPastVitalDetailsByPatientforGrowthChart(HumanID, 1, 10000);
                                        IList<PatientResults> ilstPatientResults = new List<PatientResults>();

                                        if (patientResultdto.VitalCount > 0)
                                        {
                                            var patientres = from resu in patientResultdto.VitalsList where resu.Acurus_Result_Code == objPatient_Results.Acurus_Result_Code && resu.Captured_date_and_time == objPatient_Results.Captured_date_and_time select resu;

                                            ilstPatientResults = patientres.ToList<PatientResults>();
                                            if (ilstPatientResults.Count > 0)
                                            {
                                                objPatient_Results = ilstPatientResults[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto Third;
                                                }
                                                else
                                                {
                                                    if (ilstUpdatePatientResults == null)
                                                        ilstUpdatePatientResults = new List<PatientResults>();
                                                    ilstUpdatePatientResults.Add(objPatient_Results);
                                                }
                                            }
                                            else
                                            {
                                                if (savePatientResults == null)
                                                    savePatientResults = new List<PatientResults>();
                                                savePatientResults.Add(objPatient_Results);
                                            }
                                        }
                                        else
                                        {
                                            if (savePatientResults == null)
                                                savePatientResults = new List<PatientResults>();
                                            savePatientResults.Add(objPatient_Results);
                                        }

                                        break;

                                    case "human":
                                        if (objHuman != null && objHuman.Id == 0)
                                        {
                                            objHuman = hnMngr.GetById(CarePlanLst[i].Human_ID);
                                        }

                                        objHuman.Modified_By = CarePlanLst[i].Modified_By;
                                        if (CarePlanLst[i].Care_Name_Value.Contains("Sexual Orientation"))
                                        {
                                            objHuman.Sexual_Orientation = CarePlanLst[i].Status;
                                            objHuman.Sexual_Orientation_Specify = CarePlanLst[i].Care_Plan_Notes;
                                        }
                                        else if (CarePlanLst[i].Care_Name_Value.Contains("Gender Identity"))
                                        {
                                            objHuman.Gender_Identity = CarePlanLst[i].Status;
                                            objHuman.Gender_Identity_Specify = CarePlanLst[i].Care_Plan_Notes;
                                        }
                                        else

                                            objHuman.Marital_Status = CarePlanLst[i].Status;
                                        objHuman.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        // saveHuman.Add(objHuman);
                                        break;

                                    case "advance_directive":
                                        bFirst = true;
                                    Forur: AdvanceDirective objAD = new AdvanceDirective();
                                        objAD.Human_ID = CarePlanLst[i].Human_ID;
                                        objAD.Encounter_Id = CarePlanLst[i].Encounter_ID;
                                        objAD.Status = CarePlanLst[i].Status;
                                        objAD.Comments = CarePlanLst[i].Care_Plan_Notes;
                                        objAD.Created_By = CarePlanLst[i].Created_By;
                                        objAD.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;

                                        FillOtherHistory AdvanceDirectivedto = phyPatMgr.LoadPhysicianPatient(1, 10000, HumanID, CarePlanLst[0].Encounter_ID, false);
                                        if (AdvanceDirectivedto.Advance_Directive.Count > 0)
                                        {
                                            objAD = AdvanceDirectivedto.Advance_Directive[0];
                                            if (bFirst == true)
                                            {
                                                bFirst = false;
                                                goto Forur;
                                            }
                                            else
                                            {
                                                if (ilstUpdateAD == null)
                                                    ilstUpdateAD = new List<AdvanceDirective>();
                                                ilstUpdateAD.Add(objAD);
                                            }
                                        }
                                        else
                                        {
                                            if (saveAD == null)
                                                saveAD = new List<AdvanceDirective>();
                                            saveAD.Add(objAD);
                                        }
                                        break;


                                }
                            }


                            //  saveImmunizationHistory.Add(objImmuHistory);



                        }
                    }


                }

                //IList<CarePlan> Nulllist = null;
                //SaveUpdateDeleteWithTransaction(ref Nulllist, CarePlanLst, null, MacAddress);
                // SaveUpdateDelete_DBAndXML_WithTransaction(ref Nulllist, ref CarePlanLst, null, MacAddress, true, true, CarePlanLst[0].Encounter_ID, string.Empty);
                if (CarePlanLst.Any(a => a.Care_Name_Value == "Marital Status"))
                    SocialHistorySaveOrUpdate(CarePlanLst, MacAddress);
                //objImmhstryMngr.SaveUpdateDeleteWithTransaction(ref saveImmunizationHistory, ilstUpdateImmunization, null, string.Empty);
                if (saveImmunizationHistory != null || ilstUpdateImmunization != null)
                {
                    objImmhstryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveImmunizationHistory, ref ilstUpdateImmunization, null, MacAddress, true, false, CarePlanLst[0].Human_ID, string.Empty);
                }

                //GenerateXml XMLObj = new GenerateXml();
                //if (saveImmunizationHistory != null && saveImmunizationHistory.Count > 0)
                //{
                //    ulong HumanId = saveImmunizationHistory[0].Human_ID;
                //    List<object> lstObj = saveImmunizationHistory.Cast<object>().ToList();
                //    XMLObj.GenerateXmlSaveStatic(lstObj, HumanId, string.Empty);
                //}

                //XMLObj = new GenerateXml();
                //if (ilstUpdateImmunization != null && ilstUpdateImmunization.Count > 0)
                //{
                //    ulong HumanID = ilstUpdateImmunization[0].Human_ID;
                //    List<object> lstObj = ilstUpdateImmunization.Cast<object>().ToList();
                //    XMLObj.GenerateXmlUpdate(lstObj, HumanID, string.Empty);
                //}


                //objSurgicalhistoryMngr.SaveUpdateDeleteWithTransaction(ref SurgicalSaveUpdateDelete, ilstUpdateSurgical, null, string.Empty);               
                if (SurgicalSaveUpdateDelete != null || ilstUpdateSurgical != null)
                {
                    objSurgicalhistoryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref SurgicalSaveUpdateDelete, ref ilstUpdateSurgical, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);
                }
                //ulong encounterid = 0;
                //XMLObj = new GenerateXml();
                //if (SurgicalSaveUpdateDelete != null && SurgicalSaveUpdateDelete.Count > 0)
                //{
                //    encounterid = SurgicalSaveUpdateDelete[0].Encounter_Id;
                //    List<object> lstSaveObj = SurgicalSaveUpdateDelete.Cast<object>().ToList();
                //    XMLObj.GenerateXmlSaveStatic(lstSaveObj, SurgicalSaveUpdateDelete[0].Human_ID, "");
                //}
                //XMLObj = new GenerateXml();
                //if (ilstUpdateSurgical != null && ilstUpdateSurgical.Count > 0)
                //{
                //    encounterid = ilstUpdateSurgical[0].Encounter_Id;
                //    foreach (SurgicalHistory obj in ilstUpdateSurgical)
                //    {

                //        obj.Version += 1;
                //    }
                //    List<object> lstDelObj = ilstUpdateSurgical.Cast<object>().ToList();
                //    XMLObj.GenerateXmlUpdate(lstDelObj, ilstUpdateSurgical[0].Human_ID, "");

                //}     
                if (savePatientResults != null || ilstUpdatePatientResults != null)
                {
                    objPateintresultsMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref savePatientResults, ref ilstUpdatePatientResults, null, string.Empty, true, true, CarePlanLst[0].Human_ID, string.Empty);
                    //objPateintresultsMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref savePatientResults, ref ilstUpdatePatientResults, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);
                }
                //objPateintresultsMngr.SaveUpdateDeleteWithTransaction(ref savePatientResults, ilstUpdatePatientResults, null, string.Empty);
                //XMLObj = new GenerateXml();
                //if (savePatientResults != null && savePatientResults.Count > 0)
                //{
                //    List<object> lstObj = savePatientResults.Cast<object>().ToList();
                //    XMLObj.GenerateXmlSaveStatic(lstObj, savePatientResults[0].Human_ID, string.Empty);
                //}

                //if (ilstUpdatePatientResults != null && ilstUpdatePatientResults.Count > 0)
                //{

                //    ulong humanId = ilstUpdatePatientResults[0].Human_ID;
                //    List<object> lstObj = ilstUpdatePatientResults.Cast<object>().ToList();
                //    XMLObj.GenerateXmlUpdate(lstObj, humanId, string.Empty);
                //}


                //if (saveHuman.Count > 0)
                //    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);

                if (objHuman != null && objHuman.Id != 0)
                {
                    saveHuman.Add(objHuman);
                    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);

                }


                //hnMngr.SaveUpdateDeleteWithTransaction(ref insertHuman, saveHuman, null, string.Empty);
                if (saveAD != null || ilstUpdateAD != null)
                {
                    objADMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveAD, ref ilstUpdateAD, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);
                }
                //objADMngr.SaveUpdateDeleteWithTransaction(ref saveAD, ilstUpdateAD, null, string.Empty);

                //XMLObj = new GenerateXml();
                //if (saveAD != null && saveAD.Count > 0)
                //{
                //    List<object> lstObj = saveAD.Cast<object>().ToList();
                //    XMLObj.GenerateXmlSaveStatic(lstObj, saveAD[0].Human_ID, string.Empty);
                //}



                //XMLObj = new GenerateXml();
                //if (ilstUpdateAD != null && ilstUpdateAD.Count > 0)
                //    ilstUpdateAD[0].Version += 1;
                //if (ilstUpdateAD != null && ilstUpdateAD.Count > 0)
                //{
                //    List<object> lstObjUpdateAD = ilstUpdateAD.Cast<object>().ToList();
                //    XMLObj.GenerateXmlUpdate(lstObjUpdateAD, ilstUpdateAD[0].Human_ID, string.Empty);
                //}

                iMySession.Close();
            }
            //GenerateXml XMLObj1 = new GenerateXml();
            //if (CarePlanLst.Count > 0)
            //{

            //    ulong encounterid = CarePlanLst[0].Encounter_ID;
            //    for (int i = 0; i < CarePlanLst.Count; i++)
            //    {
            //        CarePlanLst[i].Version = CarePlanLst[i].Version + 1;
            //    }
            //    List<object> lstObj = CarePlanLst.Cast<object>().ToList();
            //    XMLObj1.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}
            return CarePlanLst;
            // return GetPlanCareFromServerforThin(CarePlanLst[0].Encounter_ID, CarePlanLst[0].Human_ID, sSex, iYear);
        }
        public IList<CarePlan> DeleteCarePLanXML(IList<CarePlan> CarePlanLst, string sSex, int iYear, string MacAddress)
        {

            IList<MeasuresRuleMaster> lstMeasures = new List<MeasuresRuleMaster>();

            MeasuresRuleMasterManager measuresMngr = new MeasuresRuleMasterManager();
            lstMeasures = measuresMngr.GetAll();

            IList<ImmunizationHistory> saveImmunizationHistory = null;
            ImmunizationHistoryManager objImmhstryMngr = new ImmunizationHistoryManager();

            IList<PatientResults> savePatientResults = null;
            VitalsManager objPateintresultsMngr = new VitalsManager();

            IList<SurgicalHistory> SurgicalSaveUpdateDelete = null;
            SurgicalHistoryManager objSurgicalhistoryMngr = new SurgicalHistoryManager();

            IList<Human> saveHuman = new List<Human>();
            Human objHuman = new Human();
            IList<AdvanceDirective> saveAD = null;
            AdvanceDirectiveManager objADMngr = new AdvanceDirectiveManager();
            HumanManager hnMngr = new HumanManager();

            IList<Human> insertHuman = null;
            IList<ImmunizationHistory> ilstUpdateImmunization = null;
            IList<SurgicalHistory> ilstUpdateSurgical = null;
            IList<PatientResults> ilstUpdatePatientResults = null;
            IList<AdvanceDirective> ilstUpdateAD = null;
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                PhysicianPatientManager phyPatMgr = new PhysicianPatientManager();

                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                //IList<ImmunizationHistory> saveImmunizationHistory = new List<ImmunizationHistory>();
                DateTime Dob = DateTime.MinValue;
                DateTime AdminsterdDate = DateTime.MinValue;
                int CurrentAge = 0;
                if (CarePlanLst != null && CarePlanLst.Count > 0)
                {
                    ulong HumanID = 0;
                    HumanID = CarePlanLst[0].Human_ID;
                    if (HumanID != 0)
                    {

                        ISQLQuery sqlqueryHuman = iMySession.CreateSQLQuery("select h.* from human h   where h.Human_ID='" + HumanID + "'").AddEntity("h", typeof(Human));
                        IList<Human> lstHuman = sqlqueryHuman.List<Human>();

                        double humanAgeInMonths = 0;
                        int humanAgeInYears = 0;
                        if (lstHuman != null && lstHuman.Count > 0)
                        {
                            Dob = lstHuman[0].Birth_Date;
                            humanAgeInMonths = ((DateTime.Now.Date - lstHuman[0].Birth_Date.Date).TotalDays) / 30.4375;
                            CarePlanManager careMngr = new CarePlanManager();
                            humanAgeInYears = careMngr.CalculateAge(lstHuman[0].Birth_Date);
                        }
                    }

                    for (int i = 0; i < CarePlanLst.Count; i++)
                    {
                        if (CarePlanLst[i].Internal_Property_bInsert == true)
                        {

                            var res = from m in lstMeasures where m.Measure_Name == CarePlanLst[i].Care_Name_Value select m;
                            IList<MeasuresRuleMaster> measures = res.ToList<MeasuresRuleMaster>();
                            if (measures.Count > 0)
                            {
                                switch (measures[0].Table_Name)
                                {
                                    case "immunization_history":

                                        Boolean bFirst = true;

                                        ImmunizationHistory objImmuHistory = new ImmunizationHistory();

                                    First:
                                        objImmuHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objImmuHistory.Physician_ID = CarePlanLst[i].Physician_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] CodeDesc = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            if (CodeDesc.Length > 0)
                                            {
                                                for (int j = 0; j < CodeDesc.Length; j++)
                                                {
                                                    if (CodeDesc[j].Contains(";"))
                                                    {
                                                        string[] Code = CodeDesc[j].Split(new char[] { ';' });
                                                        string[] CodeAlone = Code[1].Split(new char[] { '$' });
                                                        if (Code.Length > 0)
                                                        {
                                                            if (CarePlanLst[i].Plan_Date != "01-01-0001")
                                                            {
                                                                if (CarePlanLst[i].Plan_Date != string.Empty)
                                                                {
                                                                    //commented by saravanakumar on12-02-2014 for change customdatetimepicker
                                                                    if (CarePlanLst[i].Plan_Date.Contains('-'))
                                                                    {
                                                                        string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                                                        if (dates.Length == 2)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime("01-" + dates[0] + dates[1]);
                                                                            AdminsterdDate = Convert.ToDateTime("01-" + dates[1] + dates[0]);
                                                                        }
                                                                        if (dates.Length == 3)
                                                                        {
                                                                            //AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                            AdminsterdDate = Convert.ToDateTime(dates[2] + "-" + dates[1] + "-" + dates[0]);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        AdminsterdDate = Convert.ToDateTime("01-" + "Jan-" + CarePlanLst[i].Plan_Date);
                                                                    }
                                                                    // AdminsterdDate = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                                                    CurrentAge = AdminsterdDate.Year - Dob.Year;
                                                                }
                                                            }
                                                            if (Code[0].Contains(">"))
                                                            {
                                                                string[] Codegreater = Code[0].Split(new char[] { '>' });
                                                                if (Codegreater.Length > 0)
                                                                {
                                                                    if (CurrentAge > Convert.ToInt32(Codegreater[1]))
                                                                    {
                                                                        objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                        objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            if (Code[0].Contains("<"))
                                                            {
                                                                if (Code[0].Contains("=") == false)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new char[] { '<' });
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[1]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }

                                                                }
                                                                else if (Code[0].Contains("=") == true)
                                                                {
                                                                    string[] Codelesser = Code[0].Split(new string[] { "<=" }, StringSplitOptions.RemoveEmptyEntries);
                                                                    if (Codelesser.Length > 0)
                                                                    {
                                                                        if (CurrentAge <= Convert.ToInt32(Codelesser[0]))
                                                                        {
                                                                            objImmuHistory.Procedure_Code = CodeAlone[0]; ;
                                                                            objImmuHistory.Immunization_Description = CodeAlone[1]; ;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            objImmuHistory.Procedure_Code = measures[0].Where_Criteria.Split(new char[] { '$' })[0];
                                            objImmuHistory.Immunization_Description = measures[0].Where_Criteria.Split(new char[] { '$' })[1];
                                        }

                                        if (CarePlanLst[i].Plan_Date.Contains('-'))
                                        {
                                            string[] dates = CarePlanLst[i].Plan_Date.Split('-');
                                            if (dates.Length == 2)
                                                objImmuHistory.Administered_Date = dates[1] + "-" + dates[0];
                                            if (dates.Length == 3)
                                                objImmuHistory.Administered_Date = dates[2] + "-" + dates[1] + "-" + dates[0];
                                        }
                                        else
                                        {
                                            objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        }

                                        //objImmuHistory.Administered_Date = CarePlanLst[i].Plan_Date;
                                        objImmuHistory.Encounter_ID = CarePlanLst[i].Encounter_ID;
                                        objImmuHistory.Created_By = CarePlanLst[i].Created_By;
                                        objImmuHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objImmuHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objImmuHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;

                                        ImmunizationHistoryDTO ImmunizationHistorydto = objImmhstryMngr.GetFromImmunizationHistory(CarePlanLst[0].Human_ID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<ImmunizationHistory> ilstImmunization = new List<ImmunizationHistory>();

                                        if (ImmunizationHistorydto.ImmunizationCount > 0)
                                        {
                                            var immun = from im in ImmunizationHistorydto.Immunization where im.Procedure_Code == objImmuHistory.Procedure_Code && im.Encounter_ID == objImmuHistory.Encounter_ID select im;
                                            ilstImmunization = immun.ToList<ImmunizationHistory>();
                                            if (ilstImmunization.Count > 0)
                                            {
                                                objImmuHistory = ilstImmunization[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto First;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateImmunization == null)
                                                        ilstUpdateImmunization = new List<ImmunizationHistory>();
                                                    ilstUpdateImmunization.Add(objImmuHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (saveImmunizationHistory == null)
                                                    saveImmunizationHistory = new List<ImmunizationHistory>();
                                                saveImmunizationHistory.Add(objImmuHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (saveImmunizationHistory == null)
                                                saveImmunizationHistory = new List<ImmunizationHistory>();
                                            saveImmunizationHistory.Add(objImmuHistory);
                                        }
                                        break;

                                    case "surgical_history":

                                        bFirst = true;

                                    second: SurgicalHistory objSurgicalHistory = new SurgicalHistory();
                                        objSurgicalHistory.Human_ID = CarePlanLst[i].Human_ID;
                                        objSurgicalHistory.Surgery_Name = CarePlanLst[i].Care_Name_Value;
                                        objSurgicalHistory.Date_Of_Surgery = CarePlanLst[i].Plan_Date;
                                        objSurgicalHistory.Description = CarePlanLst[i].Care_Plan_Notes;
                                        objSurgicalHistory.Created_By = CarePlanLst[i].Created_By;
                                        objSurgicalHistory.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        objSurgicalHistory.Modified_By = CarePlanLst[i].Modified_By;
                                        objSurgicalHistory.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        //SurgicalHistoryDTO SurgicalHisoryDetails = objSurgicalhistoryMngr.LoadSurgicalHistory(HumanID, 1, 10000);
                                        SurgicalHistoryDTO SurgicalHisoryDetails = objSurgicalhistoryMngr.LoadSurgicalHistory(HumanID, 1, 10000, CarePlanLst[0].Encounter_ID, false);
                                        IList<SurgicalHistory> ilstSurgicalHistory = new List<SurgicalHistory>();

                                        if (SurgicalHisoryDetails.SurgicalCount > 0)
                                        {
                                            var surgical = from sur in SurgicalHisoryDetails.SurgicalList where sur.Description == objSurgicalHistory.Description && sur.Date_Of_Surgery == objSurgicalHistory.Date_Of_Surgery select sur;

                                            ilstSurgicalHistory = surgical.ToList<SurgicalHistory>();
                                            if (ilstSurgicalHistory.Count > 0)
                                            {
                                                objSurgicalHistory = ilstSurgicalHistory[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto second;
                                                }
                                                else
                                                {
                                                    if (ilstUpdateSurgical == null)
                                                        ilstUpdateSurgical = new List<SurgicalHistory>();
                                                    ilstUpdateSurgical.Add(objSurgicalHistory);
                                                }
                                            }
                                            else
                                            {
                                                if (SurgicalSaveUpdateDelete == null)
                                                    SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                                SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                            }
                                        }
                                        else
                                        {
                                            if (SurgicalSaveUpdateDelete == null)
                                                SurgicalSaveUpdateDelete = new List<SurgicalHistory>();
                                            SurgicalSaveUpdateDelete.Add(objSurgicalHistory);
                                        }

                                        break;



                                    case "patient_results":
                                        bFirst = true;
                                    Third: PatientResults objPatient_Results = new PatientResults();
                                        objPatient_Results.Human_ID = CarePlanLst[i].Human_ID;
                                        if (measures[0].Where_Criteria.Contains("|"))
                                        {
                                            string[] ResultsCode = measures[0].Where_Criteria.Split(new char[] { '|' });
                                            objPatient_Results.Loinc_Identifier = ResultsCode[0];
                                            objPatient_Results.Loinc_Observation = ResultsCode[1];
                                            objPatient_Results.Units = ResultsCode[2];
                                            objPatient_Results.Reference_Range = ResultsCode[3];
                                            objPatient_Results.Acurus_Result_Code = ResultsCode[4];
                                            objPatient_Results.Acurus_Result_Description = ResultsCode[5];
                                            objPatient_Results.Results_Type = ResultsCode[6];
                                            //objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            if (CarePlanLst[i].Plan_Date != "")
                                                objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Plan_Date);
                                            else
                                            {
                                                if (CarePlanLst[i].Created_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Created_Date_And_Time);
                                                else if (CarePlanLst[i].Modified_Date_And_Time.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(CarePlanLst[i].Modified_Date_And_Time);
                                                else
                                                    objPatient_Results.Captured_date_and_time = Convert.ToDateTime(DateTime.Now);
                                            }
                                        }
                                        objPatient_Results.Physician_ID = CarePlanLst[i].Physician_ID;
                                        objPatient_Results.Value = CarePlanLst[i].Status_Value;
                                        objPatient_Results.Created_By = CarePlanLst[i].Created_By;
                                        objPatient_Results.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                                        // savePatientResults.Add(objPatient_Results);

                                        PatientResultsDTO patientResultdto = objPateintresultsMngr.GetPastVitalDetailsByPatientforGrowthChart(HumanID, 1, 10000);
                                        IList<PatientResults> ilstPatientResults = new List<PatientResults>();

                                        if (patientResultdto.VitalCount > 0)
                                        {
                                            var patientres = from resu in patientResultdto.VitalsList where resu.Acurus_Result_Code == objPatient_Results.Acurus_Result_Code && resu.Captured_date_and_time == objPatient_Results.Captured_date_and_time select resu;

                                            ilstPatientResults = patientres.ToList<PatientResults>();
                                            if (ilstPatientResults.Count > 0)
                                            {
                                                objPatient_Results = ilstPatientResults[0];
                                                if (bFirst == true)
                                                {
                                                    bFirst = false;
                                                    goto Third;
                                                }
                                                else
                                                {
                                                    if (ilstUpdatePatientResults == null)
                                                        ilstUpdatePatientResults = new List<PatientResults>();
                                                    ilstUpdatePatientResults.Add(objPatient_Results);
                                                }
                                            }
                                            else
                                            {
                                                if (savePatientResults == null)
                                                    savePatientResults = new List<PatientResults>();
                                                savePatientResults.Add(objPatient_Results);
                                            }
                                        }
                                        else
                                        {
                                            if (savePatientResults == null)
                                                savePatientResults = new List<PatientResults>();
                                            savePatientResults.Add(objPatient_Results);
                                        }

                                        break;

                                    case "human":
                                        if (objHuman != null && objHuman.Id == 0)
                                        {
                                            objHuman = hnMngr.GetById(CarePlanLst[i].Human_ID);
                                        }

                                        objHuman.Modified_By = CarePlanLst[i].Modified_By;
                                        if (CarePlanLst[i].Care_Name_Value.Contains("Sexual Orientation"))
                                        {
                                            objHuman.Sexual_Orientation = CarePlanLst[i].Status;
                                            objHuman.Sexual_Orientation_Specify = CarePlanLst[i].Care_Plan_Notes;
                                        }
                                        else if (CarePlanLst[i].Care_Name_Value.Contains("Gender Identity"))
                                        {
                                            objHuman.Gender_Identity = CarePlanLst[i].Status;
                                            objHuman.Gender_Identity_Specify = CarePlanLst[i].Care_Plan_Notes;
                                        }
                                        else

                                            objHuman.Marital_Status = CarePlanLst[i].Status;
                                        objHuman.Modified_Date_And_Time = CarePlanLst[i].Modified_Date_And_Time;
                                        // saveHuman.Add(objHuman);
                                        break;

                                    case "advance_directive":
                                        bFirst = true;
                                    Forur: AdvanceDirective objAD = new AdvanceDirective();
                                        objAD.Human_ID = CarePlanLst[i].Human_ID;
                                        objAD.Encounter_Id = CarePlanLst[i].Encounter_ID;
                                        objAD.Status = CarePlanLst[i].Status;
                                        objAD.Comments = CarePlanLst[i].Care_Plan_Notes;
                                        objAD.Created_By = CarePlanLst[i].Created_By;
                                        objAD.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;

                                        FillOtherHistory AdvanceDirectivedto = phyPatMgr.LoadPhysicianPatient(1, 10000, HumanID, CarePlanLst[0].Encounter_ID, false);
                                        if (AdvanceDirectivedto.Advance_Directive.Count > 0)
                                        {
                                            objAD = AdvanceDirectivedto.Advance_Directive[0];
                                            if (bFirst == true)
                                            {
                                                bFirst = false;
                                                goto Forur;
                                            }
                                            else
                                            {
                                                if (ilstUpdateAD == null)
                                                    ilstUpdateAD = new List<AdvanceDirective>();
                                                ilstUpdateAD.Add(objAD);
                                            }
                                        }
                                        else
                                        {
                                            if (saveAD == null)
                                                saveAD = new List<AdvanceDirective>();
                                            saveAD.Add(objAD);
                                        }
                                        break;


                                }
                            }


                            //  saveImmunizationHistory.Add(objImmuHistory);



                        }
                    }


                }

                IList<CarePlan> Nulllist = null;
                IList<CarePlan> Nulllistupd = null;
                //SaveUpdateDeleteWithTransaction(ref Nulllist, CarePlanLst, null, MacAddress);
                SaveUpdateDelete_DBAndXML_WithTransaction(ref Nulllist, ref Nulllistupd, CarePlanLst, MacAddress, true, false, CarePlanLst[0].Encounter_ID, string.Empty);
                //CAP-2131
                //if (CarePlanLst.Any(a => a.Care_Name_Value == "Marital Status"))
                //    SocialHistorySaveOrUpdate(CarePlanLst, MacAddress);

                //objImmhstryMngr.SaveUpdateDeleteWithTransaction(ref saveImmunizationHistory, ilstUpdateImmunization, null, string.Empty);
                if (saveImmunizationHistory != null || ilstUpdateImmunization != null)
                {
                    objImmhstryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveImmunizationHistory, ref ilstUpdateImmunization, null, MacAddress, true, false, CarePlanLst[0].Human_ID, string.Empty);
                }

                //GenerateXml XMLObj = new GenerateXml();
                //if (saveImmunizationHistory != null && saveImmunizationHistory.Count > 0)
                //{
                //    ulong HumanId = saveImmunizationHistory[0].Human_ID;
                //    List<object> lstObj = saveImmunizationHistory.Cast<object>().ToList();
                //    XMLObj.GenerateXmlSaveStatic(lstObj, HumanId, string.Empty);
                //}

                //XMLObj = new GenerateXml();
                //if (ilstUpdateImmunization != null && ilstUpdateImmunization.Count > 0)
                //{
                //    ulong HumanID = ilstUpdateImmunization[0].Human_ID;
                //    List<object> lstObj = ilstUpdateImmunization.Cast<object>().ToList();
                //    XMLObj.GenerateXmlUpdate(lstObj, HumanID, string.Empty);
                //}


                //objSurgicalhistoryMngr.SaveUpdateDeleteWithTransaction(ref SurgicalSaveUpdateDelete, ilstUpdateSurgical, null, string.Empty);               
                if (SurgicalSaveUpdateDelete != null || ilstUpdateSurgical != null)
                {
                    objSurgicalhistoryMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref SurgicalSaveUpdateDelete, ref ilstUpdateSurgical, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);
                }
                //ulong encounterid = 0;
                //XMLObj = new GenerateXml();
                //if (SurgicalSaveUpdateDelete != null && SurgicalSaveUpdateDelete.Count > 0)
                //{
                //    encounterid = SurgicalSaveUpdateDelete[0].Encounter_Id;
                //    List<object> lstSaveObj = SurgicalSaveUpdateDelete.Cast<object>().ToList();
                //    XMLObj.GenerateXmlSaveStatic(lstSaveObj, SurgicalSaveUpdateDelete[0].Human_ID, "");
                //}
                //XMLObj = new GenerateXml();
                //if (ilstUpdateSurgical != null && ilstUpdateSurgical.Count > 0)
                //{
                //    encounterid = ilstUpdateSurgical[0].Encounter_Id;
                //    foreach (SurgicalHistory obj in ilstUpdateSurgical)
                //    {

                //        obj.Version += 1;
                //    }
                //    List<object> lstDelObj = ilstUpdateSurgical.Cast<object>().ToList();
                //    XMLObj.GenerateXmlUpdate(lstDelObj, ilstUpdateSurgical[0].Human_ID, "");

                //}     
                if (savePatientResults != null || ilstUpdatePatientResults != null)
                {
                    objPateintresultsMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref savePatientResults, ref ilstUpdatePatientResults, null, string.Empty, true, true, CarePlanLst[0].Human_ID, string.Empty);
                    //objPateintresultsMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref savePatientResults, ref ilstUpdatePatientResults, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);
                }
                //objPateintresultsMngr.SaveUpdateDeleteWithTransaction(ref savePatientResults, ilstUpdatePatientResults, null, string.Empty);
                //XMLObj = new GenerateXml();
                //if (savePatientResults != null && savePatientResults.Count > 0)
                //{
                //    List<object> lstObj = savePatientResults.Cast<object>().ToList();
                //    XMLObj.GenerateXmlSaveStatic(lstObj, savePatientResults[0].Human_ID, string.Empty);
                //}

                //if (ilstUpdatePatientResults != null && ilstUpdatePatientResults.Count > 0)
                //{

                //    ulong humanId = ilstUpdatePatientResults[0].Human_ID;
                //    List<object> lstObj = ilstUpdatePatientResults.Cast<object>().ToList();
                //    XMLObj.GenerateXmlUpdate(lstObj, humanId, string.Empty);
                //}


                //if (saveHuman.Count > 0)
                //    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);

                if (objHuman != null && objHuman.Id != 0)
                {
                    saveHuman.Add(objHuman);
                    hnMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref insertHuman, ref saveHuman, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);

                }


                //hnMngr.SaveUpdateDeleteWithTransaction(ref insertHuman, saveHuman, null, string.Empty);
                if (saveAD != null || ilstUpdateAD != null)
                {
                    objADMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref saveAD, ref ilstUpdateAD, null, string.Empty, true, false, CarePlanLst[0].Human_ID, string.Empty);
                }
                //objADMngr.SaveUpdateDeleteWithTransaction(ref saveAD, ilstUpdateAD, null, string.Empty);

                //XMLObj = new GenerateXml();
                //if (saveAD != null && saveAD.Count > 0)
                //{
                //    List<object> lstObj = saveAD.Cast<object>().ToList();
                //    XMLObj.GenerateXmlSaveStatic(lstObj, saveAD[0].Human_ID, string.Empty);
                //}



                //XMLObj = new GenerateXml();
                //if (ilstUpdateAD != null && ilstUpdateAD.Count > 0)
                //    ilstUpdateAD[0].Version += 1;
                //if (ilstUpdateAD != null && ilstUpdateAD.Count > 0)
                //{
                //    List<object> lstObjUpdateAD = ilstUpdateAD.Cast<object>().ToList();
                //    XMLObj.GenerateXmlUpdate(lstObjUpdateAD, ilstUpdateAD[0].Human_ID, string.Empty);
                //}

                iMySession.Close();
            }
            //GenerateXml XMLObj1 = new GenerateXml();
            //if (CarePlanLst.Count > 0)
            //{

            //    ulong encounterid = CarePlanLst[0].Encounter_ID;
            //    for (int i = 0; i < CarePlanLst.Count; i++)
            //    {
            //        CarePlanLst[i].Version = CarePlanLst[i].Version + 1;
            //    }
            //    List<object> lstObj = CarePlanLst.Cast<object>().ToList();
            //    XMLObj1.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}
            return CarePlanLst;
            // return GetPlanCareFromServerforThin(CarePlanLst[0].Encounter_ID, CarePlanLst[0].Human_ID, sSex, iYear);
        }
        public IList<CarePlanDTO> GetCarePlanForPastEncounter(ulong encounterId, ulong humanId, ulong physicainId, string gender, int age)
        {
            CarePlanDTO objCarePlanDTO = new CarePlanDTO();
            EncounterManager objEncounterManager = new EncounterManager();

            IList<CarePlanDTO> ilstCarePlanDTO = new List<CarePlanDTO>();

            ulong previousEncounterId = 0;
            bool isPhysicianProcess = false;
            bool isFromArchive = false;

            var ilstEncounter = objEncounterManager.GetPreviousEncounterDetails(encounterId,
                                                                         humanId,
                                                                         physicainId, out isPhysicianProcess,
                                                                         out isFromArchive);

            if (ilstEncounter.Count > 0)
            {
                previousEncounterId = ilstEncounter[0].Id;

                objCarePlanDTO.Physician_Process = isPhysicianProcess;
                objCarePlanDTO.PreviousEncounterId = previousEncounterId;


                if (isPhysicianProcess)
                {
                    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                    {
                        IList<string> strArray = new List<string>();
                        strArray.Add("ALL");
                        strArray.Add(gender);

                        IQuery query;
                        if (isFromArchive)
                            query = iMySession.GetNamedQuery("Fill.CarePlan.Server.Archive");
                        else
                            query = iMySession.GetNamedQuery("Fill.CarePlan.Server");

                        query.SetParameter(0, previousEncounterId);
                        query.SetParameter(1, age);
                        query.SetParameterList("Gender", strArray.ToArray<string>());

                        var aryCare = new ArrayList(query.List());

                        if (aryCare.Count > 0)
                        {
                            for (int i = 0; i < aryCare.Count; i++)
                            {
                                object[] ob = (object[])aryCare[i];

                                objCarePlanDTO = new CarePlanDTO();
                                if (Convert.ToString(ob[0]) != "")
                                {
                                    objCarePlanDTO.Care_Name = Convert.ToString(ob[0]);
                                    objCarePlanDTO.Care_Name_Value = Convert.ToString(ob[1]);
                                    objCarePlanDTO.Options = Convert.ToString(ob[2]);
                                    objCarePlanDTO.Status = Convert.ToString(ob[3]);
                                    objCarePlanDTO.Care_Plan_Notes = Convert.ToString(ob[4]);
                                    objCarePlanDTO.Created_By = Convert.ToString(ob[5]);
                                    objCarePlanDTO.Created_Date_And_Time = Convert.ToDateTime(ob[6]);
                                    objCarePlanDTO.Care_Plan_Lookup_ID = Convert.ToUInt64(ob[7]);
                                    objCarePlanDTO.Plan_Care_ID = Convert.ToUInt64(ob[8]);
                                    objCarePlanDTO.Version = Convert.ToInt32(ob[9]);
                                    objCarePlanDTO.Controls = Convert.ToString(ob[10]);
                                    objCarePlanDTO.Plan_Date = Convert.ToString(ob[11]);
                                    objCarePlanDTO.Status_Value = Convert.ToString(ob[12]);
                                    objCarePlanDTO.Control_Name = Convert.ToString(ob[13]);
                                    objCarePlanDTO.Master_ID = Convert.ToUInt64(ob[14]);
                                    objCarePlanDTO.Physician_Process = isPhysicianProcess;
                                    objCarePlanDTO.PreviousEncounterId = previousEncounterId;
                                    ilstCarePlanDTO.Add(objCarePlanDTO);
                                }
                            }
                        }
                        else
                        {
                            objCarePlanDTO.Physician_Process = isPhysicianProcess;
                            objCarePlanDTO.PreviousEncounterId = previousEncounterId;
                            ilstCarePlanDTO.Add(objCarePlanDTO);
                        }
                        iMySession.Close();
                    }
                }
                else
                    ilstCarePlanDTO.Add(objCarePlanDTO);
            }

            return ilstCarePlanDTO;

        }

        public IList<CarePlanDTO> GetPlanCareFromServerforThin(ulong UlEncounterID, ulong ulHumanID, string Sex, int Months)
        {
            IList<CarePlanDTO> CareDTO = new List<CarePlanDTO>();
            /*  For git Id 1594 Start
             CarePlanDTO PlanDto = null;           
             ArrayList aryCare = null;
           IList<string> strArray = new List<string>();
           strArray.Add("ALL");
           strArray.Add(Sex);

           using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
           {
               //IQuery query = iMySession.GetNamedQuery("Fill.CarePlan.Server")
               //                         .SetParameter("ENCOUNTER_ID", Convert.ToString(UlEncounterID));

               IQuery query = iMySession.GetNamedQuery("Fill.CarePlan.Server");
               query.SetParameter(0, UlEncounterID.ToString());
               query.SetParameter(1, Months);
               query.SetParameterList("Gender", strArray.ToArray<string>());

               aryCare = new ArrayList(query.List());
               if (aryCare.Count > 0)
               {
                   for (int i = 0; i < aryCare.Count; i++)
                   {
                       object[] ob = (object[])aryCare[i];
                       PlanDto = new CarePlanDTO();
                       PlanDto.Care_Name = Convert.ToString(ob[0]);
                       PlanDto.Care_Name_Value = Convert.ToString(ob[1]);
                       PlanDto.Options = Convert.ToString(ob[2]);
                       PlanDto.Status = Convert.ToString(ob[3]);
                       PlanDto.Care_Plan_Notes = Convert.ToString(ob[4]);
                       PlanDto.Created_By = Convert.ToString(ob[5]);
                       PlanDto.Created_Date_And_Time = Convert.ToDateTime(ob[6]);
                       PlanDto.Care_Plan_Lookup_ID = Convert.ToUInt64(ob[7]);
                       PlanDto.Plan_Care_ID = Convert.ToUInt64(ob[8]);
                       PlanDto.Version = Convert.ToInt32(ob[9]);
                       PlanDto.Controls = Convert.ToString(ob[10]);
                       PlanDto.Plan_Date = Convert.ToString(ob[11]);
                       PlanDto.Status_Value = Convert.ToString(ob[12]);
                       PlanDto.Control_Name = Convert.ToString(ob[13]);
                       PlanDto.Master_ID = Convert.ToUInt64(ob[14]);
                       //PlanDto.Gender = Convert.ToString(ob[15]);
                       CareDTO.Add(PlanDto);
                   }
               }

               IList<CarePlan> lstCarePlan = new List<CarePlan>();
               lstCarePlan = GetCarePlanByEncounter(UlEncounterID);

              //if (CareDTO.Count == 0)
               if (lstCarePlan.Count == 0)
               {
                   CarePlanLookupManager carePlanMngr = new CarePlanLookupManager();
                   CareDTO = carePlanMngr.GetCarePlanLookupFromLocalForThin("Care_Name_Value", ulHumanID, UlEncounterID, Sex, Months);
               }

               iMySession.Close();
           }
            For git Id 1594 End  */
            CarePlanLookupManager carePlanMngr = new CarePlanLookupManager();
            CareDTO = carePlanMngr.GetCarePlanLookupFromLocalForThin("Care_Name_Value", ulHumanID, UlEncounterID, Sex, Months);
            return CareDTO;
        }


        public void SaveCarePlanforSummary(IList<CarePlan> lstcareplan)
        {
            //SaveUpdateDeleteWithTransaction(ref lstcareplan, null, null, string.Empty);
            IList<CarePlan> lstcareplannull = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstcareplan, ref lstcareplannull, null, string.Empty, false, true, 0, string.Empty);
        }

        public IList<CarePlanDTO> UpdateCarePLanForCopyPrevious(IList<CarePlan> CarePlanLst, string sSex, int iYear, string MacAddress, ulong Encoutner_Id, ulong Human_Id)
        {
            IList<CarePlan> Savelst = new List<CarePlan>();
            IList<CarePlan> Deletelst = new List<CarePlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

                ICriteria crit = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Encounter_ID", Encoutner_Id)).Add(Expression.Eq("Human_ID", Human_Id));
                Deletelst = crit.List<CarePlan>();
                //SaveUpdateDeleteWithTransaction(ref Savelst, null, Deletelst, MacAddress);
                IList<CarePlan> lstcareplannull = null;
                //IList<AdvanceDirective> ilstUpdateADlist = saveAD.Concat(ilstUpdateAD).ToList();
                SaveUpdateDelete_DBAndXML_WithTransaction(ref Savelst, ref lstcareplannull, Deletelst, string.Empty, false, true, 0, string.Empty);
                iMySession.Close();
            }

            return SaveCarePlan(CarePlanLst, sSex, iYear, MacAddress);
        }

        public void SocialHistorySaveOrUpdate(IList<CarePlan> CarePlanLst, string MacAddress)
        {
            SocialHistoryManager objSocialHistoryManager = new SocialHistoryManager();
            IList<SocialHistory> lstSave = new List<SocialHistory>();
            IList<SocialHistory> lstUpdate = new List<SocialHistory>();
            IList<SocialHistory> lstDelete = new List<SocialHistory>();
            ulong uHuman_id = CarePlanLst[0].Human_ID;
            bool bSaved = false;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
                //START BugID:45387
                IList<SocialHistory> lst = new List<SocialHistory>();
                lst = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", CarePlanLst[0].Human_ID)).List<SocialHistory>();

                if (lst != null && lst.Count > 0)
                {
                    IList<SocialHistory> lstSoc = lst.OrderByDescending(a => a.Encounter_ID).ToList<SocialHistory>();
                    IList<ulong> lstEnc = lstSoc.Select(a => a.Encounter_ID).Distinct().ToList<ulong>();

                    if (lstEnc != null && lstEnc.Count > 0)
                    {
                        IList<SocialHistory> lstCurrEncData = lstSoc.Where(a => a.Encounter_ID == CarePlanLst[0].Encounter_ID).ToList<SocialHistory>();
                        if (lstCurrEncData != null && lstCurrEncData.Count > 0)
                        {
                            IList<SocialHistory> lstMaritalStatus = lstCurrEncData.Where(a => a.Social_Info.ToUpper() == "MARITAL STATUS").ToList<SocialHistory>();
                            if (lstMaritalStatus != null && lstMaritalStatus.Count > 0)
                            {
                                lstUpdate = lstMaritalStatus;
                                if (lstUpdate[0].Value != CarePlanLst.Where(a => a.Care_Name_Value == "Marital Status").Select(a => a.Status).ToArray<string>()[0])
                                {
                                    lstUpdate[0].Value = CarePlanLst.Where(a => a.Care_Name_Value == "Marital Status").Select(a => a.Status).ToArray<string>()[0];
                                }
                                if (lstUpdate[0].Description != CarePlanLst.Where(a => a.Care_Name_Value == "Marital Status").Select(a => a.Care_Plan_Notes).ToArray<string>()[0])
                                {
                                    lstUpdate[0].Description = CarePlanLst.Where(a => a.Care_Name_Value == "Marital Status").Select(a => a.Care_Plan_Notes).ToArray<string>()[0];
                                }
                                lstUpdate[0].Modified_By = CarePlanLst[0].Modified_By != string.Empty ? CarePlanLst[0].Modified_By : CarePlanLst[0].Modified_By == string.Empty ? CarePlanLst[0].Created_By : string.Empty;
                                lstUpdate[0].Modified_Date_And_Time = CarePlanLst[0].Modified_Date_And_Time != DateTime.MinValue ? CarePlanLst[0].Modified_Date_And_Time : CarePlanLst[0].Modified_Date_And_Time == DateTime.MinValue ? CarePlanLst[0].Created_Date_And_Time : DateTime.MinValue;
                                bSaved = true;
                            }
                        }
                        else if (lstEnc.Count >= 1)
                        {
                            ulong pastencID = 0;
                            pastencID = lstEnc[0];
                            IList<SocialHistory> lstSocHis = lstSoc.Where(a => a.Encounter_ID == pastencID).ToList<SocialHistory>();
                            if (lstSocHis != null && lstSocHis.Count > 0)
                            {
                                foreach (SocialHistory soc in lstSocHis)
                                {
                                    SocialHistory objSocialHistory = new SocialHistory();
                                    objSocialHistory.Human_ID = soc.Human_ID;
                                    objSocialHistory.Encounter_ID = CarePlanLst[0].Encounter_ID;
                                    objSocialHistory.Created_By = CarePlanLst[0].Modified_By != string.Empty ? CarePlanLst[0].Modified_By : CarePlanLst[0].Modified_By == string.Empty ? CarePlanLst[0].Created_By : string.Empty;
                                    objSocialHistory.Created_Date_And_Time = CarePlanLst[0].Modified_Date_And_Time != DateTime.MinValue ? CarePlanLst[0].Modified_Date_And_Time : CarePlanLst[0].Modified_Date_And_Time == DateTime.MinValue ? CarePlanLst[0].Created_Date_And_Time : DateTime.MinValue;

                                    if (soc.Social_Info.ToUpper() != "MARITAL STATUS")
                                    {
                                        objSocialHistory.Social_Info = soc.Social_Info;
                                        objSocialHistory.Is_Present = soc.Is_Present;
                                        objSocialHistory.Value = soc.Value;
                                        objSocialHistory.Description = soc.Description;
                                    }
                                    else
                                    {
                                        objSocialHistory.Social_Info = "Marital Status";
                                        objSocialHistory.Is_Present = "Y";
                                        objSocialHistory.Value = CarePlanLst.Where(a => a.Care_Name_Value == "Marital Status").Select(a => a.Status).ToArray<string>()[0];
                                        objSocialHistory.Description = CarePlanLst.Where(a => a.Care_Name_Value == "Marital Status").Select(a => a.Care_Plan_Notes).ToArray<string>()[0];
                                        bSaved = true;
                                    }
                                    lstSave.Add(objSocialHistory);
                                }
                            }
                        }


                    }

                }
                if (bSaved == false)
                {
                    SocialHistory objSocialHistory = new SocialHistory();
                    objSocialHistory.Human_ID = CarePlanLst[0].Human_ID;
                    objSocialHistory.Social_Info = "Marital Status";
                    objSocialHistory.Encounter_ID = CarePlanLst[0].Encounter_ID;
                    objSocialHistory.Is_Present = "Y";
                    objSocialHistory.Value = CarePlanLst.Where(a => a.Care_Name_Value == "Marital Status").Select(a => a.Status).ToArray<string>()[0];
                    objSocialHistory.Description = CarePlanLst.Where(a => a.Care_Name_Value == "Marital Status").Select(a => a.Care_Plan_Notes).ToArray<string>()[0];
                    objSocialHistory.Created_By = CarePlanLst[0].Modified_By != string.Empty ? CarePlanLst[0].Modified_By : CarePlanLst[0].Modified_By == string.Empty ? CarePlanLst[0].Created_By : string.Empty;
                    objSocialHistory.Created_Date_And_Time = CarePlanLst[0].Modified_Date_And_Time != DateTime.MinValue ? CarePlanLst[0].Modified_Date_And_Time : CarePlanLst[0].Modified_Date_And_Time == DateTime.MinValue ? CarePlanLst[0].Created_Date_And_Time : DateTime.MinValue;
                    lstSave.Add(objSocialHistory);
                    bSaved = true;
                }
                //END BugID:45387
                iMySession.Close();
            }

            //objSocialHistoryManager.SaveUpdateDeleteWithTransaction(ref lstSave, lstUpdate, lstDelete, MacAddress);
            if (bSaved == true)
            {
                objSocialHistoryManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref lstSave, ref lstUpdate, lstDelete, string.Empty, true, true, uHuman_id, string.Empty);
                //BUGID:47384-Changed bsaveStatic to true to retain the previously saved Soc_His fields for the current encounter
            }



            //GenerateXml XMLObj = new GenerateXml();
            //if (lstSave != null && lstSave.Count > 0)
            //{
            //    XMLObj.GenerateXmlSaveStatic(lstSave.Cast<object>().ToList(), lstSave[0].Human_ID, string.Empty);
            //}
            //if (lstUpdate != null && lstUpdate.Count > 0)
            //{
            //    for (int i = 0; i < lstUpdate.Count; i++)
            //    {
            //        lstUpdate[i].Version += 1;

            //    }
            //    XMLObj.GenerateXmlUpdate(lstUpdate.Cast<object>().ToList(), lstUpdate[0].Human_ID, string.Empty);
            //}


        }


        public IList<CarePlan> GetCarePlanByEncounter(ulong encounterId)
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Encounter_ID", encounterId));
                var ilstCarePlan = criteria.List<CarePlan>();
                iMySession.Close();
                return ilstCarePlan;
            }
        }
        public IList<CarePlan> GetCarePlanByHuman(ulong humanID)
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Human_ID", humanID));
                var ilstCarePlan = criteria.List<CarePlan>();
                iMySession.Close();
                return ilstCarePlan;
            }
        }
        #endregion
    }
}
