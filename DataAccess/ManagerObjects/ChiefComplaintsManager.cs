using System;
using System.Collections.Generic;
using System.Collections;

using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Data;
using System.IO;
using System.Xml;


namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IChiefComplaintManager : IManagerBase<ChiefComplaints, uint>
    {
        IList<ChiefComplaints> GetComplaintsByHumanID(ulong HumanID, ulong EncounterID, ulong PhysicianID);
        IList<ChiefComplaints> GetComplaintsByEncID(ulong ulEncounterID, bool isFromArchive);
        IList<ChiefComplaints> GetComplaintsByCCID(int ulCCID, ulong ulEncounterID);
        IList<ChiefComplaints> GetPNChiefComplaints(ulong ulEncounterID);

        IList<FillChiefComplaint> GetComplaintsforPastEncounter(ulong HumanID, ulong EncounterID, ulong Physician_ID);
        IList<FillChiefComplaint> SaveComplaints(IList<ChiefComplaints> complaintList,ref IList<Encounter> objEncounter, string macAddress);
        IList<FillChiefComplaint> UpdateComplaints(ref IList<ChiefComplaints> addList, ref IList<ChiefComplaints> updateList, IList<ChiefComplaints> deleteList,ref IList<Encounter> objEncounter, string macAddress);
        IList<FillChiefComplaint> DeleteComplaints(IList<ChiefComplaints> complaintList, string macAddress);
        IList<FillChiefComplaint> FillCC(ulong ulEncounterID);
        IList<FillChiefComplaint> FillPhrases(ulong ulEncounterID);
        IList<FillChiefComplaint> GetPreviousEncounter(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID, string Action, ulong PreEnc);

        IList<PatientResults> GetPNVitals(ulong ulEncounterID);
        //IList<Medication> GetPNMedication(ulong ulEncounterID);
        IList<Examination> GetPNExamination(ulong ulEncounterID);
        IList<Assessment> GetPNAssessment(ulong ulEncounterID);
        IList<TreatmentPlan> GetPNTreatmentPlan(ulong ulEncounterID);
        IList<PastMedicalHistory> GetPNPastMedicalHistory(ulong ulEncounterID);
        IList<SurgicalHistory> GetPNSurgicalHistory(ulong ulEncounterID);
        IList<HospitalizationHistory> GetPNHospitalizationHistory(ulong ulEncounterID);
        IList<FamilyHistory> GetPNFamilyHistory(ulong ulEncounterID);
        IList<SocialHistory> GetPNSocialHistory(ulong ulEncounterID);
        IList<NonDrugAllergy> GetPNNonDrugAllergy(ulong ulEncounterID);
        IList<GeneralNotes> GetPNGeneralNotes(ulong ulEncounterID);
        IList<FamilyDisease> GetPNFamilyDiseaseDetails();
        IList<Encounter> GetPNEncounterDetails(ulong ulEncounterID);
        IList<ROS> GetPNROS(ulong ulEncounterID);

        IList<StaticLookup> GetRaceAndEthnicity(ulong ulEncounterID);

        IList<Encounter> GetEncounterList(ulong ulEncounterID);



        //FillProgressNotes GetProgressNotesList(ulong ulEncounterID);

        FillClinicalSummary GetClinicalSummary(ulong ulEncounterID, ulong ulHumanID);

        //FollowUpEncounterDTO GetUpdatedValues(int switchIndex, ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID, string Current_UserName, string Role, bool Update, string value, string MACAddress, IList<string> FieldLookupList, bool Current_Encounter_Data, ulong PreviousEnc, bool Is_From_CC_Load);

        //FollowUpEncounterDTO GetFollowUpEncounter(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID, string Current_UserName, string Role, bool Update, string value, string MACAddress, IList<string> FieldLookupList, bool Current_Encounter_Data, ulong PreviousEnc, bool Is_From_CC_Load);

        //FillNotesDownLoadInfoDTO NewGetProgressNotesByXML(ulong EncID, ulong Phy_id, DataTable dtXML, ulong humanID);

        //FillNotesDownLoadInfoDTO ConsultationNotes(ulong EncID, ulong Phy_id, DataTable dtXML, ulong humanID);

        FillROS ReplaceROS(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID, ulong PreEnc);

       //FollowUpEncounterDTO GetPreviousEncounterIdByDateOfService(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID, string Current_UserName, string Role, bool Update, string value, string MACAddress, IList<string> FieldLookupList, bool Current_Encounter_Data, bool Is_From_CC_Load);

        ulong getHumanID(ulong ulEncounterID);
        ulong getPhysicianID(ulong ulEncounterID);
        ulong GetPreviousEncounterIdAlone(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID);

        int GetCCCount(ulong ulEncounterID);
        int DeleteExamination(ulong ulEncounterID, string sCategory);

        bool SetIsCopyPreviousEncounter(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID, string MACAddress);

        string GetPurposeOfVisit(ulong ulEncounterID);

        void UpdateEncounter(ulong ulEncounterID, string value, string MACAddress);


        //void GetFollowUpEncounter(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID,
        //     string Current_UserName, ulong PreviousEnc, string sRole);
        IList<InHouseProcedure> GetImplantableDeviceInformationByHumanID(ulong HumanID);
        FillClinicalSummary GetClinicalSummaryCaseReporting(ulong ulEncounterID, ulong ulHumanID);

    }

    public partial class ChiefComplaintsManager : ManagerBase<ChiefComplaints, uint>, IChiefComplaintManager
    {
        #region Constructors

        public ChiefComplaintsManager()
            : base()
        {

        }
        public ChiefComplaintsManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods

        public IList<ChiefComplaints> GetComplaintsByHumanID(ulong HumanID, ulong EncounterID, ulong PhysicianID)
        {

            int index = 0;
            IList<ChiefComplaints> CClist = new List<ChiefComplaints>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Human_ID", HumanID));
                IList<ulong> lstencountID = (from arr in crit.List<ChiefComplaints>() orderby arr.Encounter_ID ascending select (arr.Encounter_ID)).Distinct().ToList<ulong>();


                if (lstencountID != null && lstencountID.Count != 0)
                {

                    int provider_id = Convert.ToInt32(PhysicianID);
                    ICriteria Cri_ENC = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", HumanID));
                    IList<Encounter> lstfrom_encounter = (from a in Cri_ENC.List<Encounter>() where (a.Id > lstencountID[0] && a.Id < EncounterID) select a).ToList<Encounter>();


                    ulong Enc_ID = 0;
                    if (lstfrom_encounter != null)
                    { 
                    if (lstfrom_encounter.Count == 1)
                        Enc_ID = lstfrom_encounter[0].Id;
                    else if (lstfrom_encounter.Count > 1)
                        Enc_ID = lstfrom_encounter[lstfrom_encounter.Count - 1].Id;
                    else
                        Enc_ID = lstencountID[0];
                    }


                    if (lstencountID.Count == 1 && lstencountID[0] <= (EncounterID - 1))
                    {
                        CClist = (from arr in crit.List<ChiefComplaints>() where (arr.Encounter_ID == Enc_ID && arr.Modified_Date_And_Time == (from arrs in crit.List<ChiefComplaints>() where (arrs.Encounter_ID == Enc_ID) select arrs.Modified_Date_And_Time).Max()) select arr).ToList<ChiefComplaints>();
                    }
                    else if (lstencountID.Count > 1 && !lstencountID.Contains(EncounterID))
                    {
                        CClist = (from arr in crit.List<ChiefComplaints>() where (arr.Encounter_ID == Enc_ID && arr.Modified_Date_And_Time == (from arrs in crit.List<ChiefComplaints>() where (arrs.Encounter_ID == Enc_ID) select arrs.Modified_Date_And_Time).Max()) select arr).ToList<ChiefComplaints>();
                    }
                    else
                    {
                        for (int i = 0; i < lstencountID.Count; i++)
                        {
                            if (lstencountID[i] == EncounterID)
                                index = i;
                        }
                        if (index != 0)
                            CClist = (from arr in crit.List<ChiefComplaints>() where (arr.Encounter_ID == lstencountID[index - 1] && arr.Modified_Date_And_Time == (from arrs in crit.List<ChiefComplaints>() where (arrs.Encounter_ID == lstencountID[index - 1]) select arrs.Modified_Date_And_Time).Max()) select arr).ToList<ChiefComplaints>();
                        else
                            CClist = new List<ChiefComplaints>();
                    }
                }
                else
                {
                    CClist = new List<ChiefComplaints>();
                }
                iMySession.Close();
            }
            return CClist;

        }

        public IList<FillChiefComplaint> GetComplaintsforPastEncounter(ulong humanId, ulong encounterId
                                                                     , ulong physicianId)
        {
            FillChiefComplaint fillCC = new FillChiefComplaint();
            IList<FillChiefComplaint> CClist = new List<FillChiefComplaint>();

            ulong previousEncounterId = 0;

            bool isFromArchive = false;
            bool isPhysicianProcess = false;

            EncounterManager objEncounterManager = new EncounterManager();

            var ilstEncounter = objEncounterManager.GetPreviousEncounterDetails(encounterId, humanId,
                physicianId, out isPhysicianProcess, out isFromArchive);

            if (ilstEncounter.Count > 0 && ilstEncounter != null)
            {
                previousEncounterId = ilstEncounter[0].Id;

                fillCC.PEncID = previousEncounterId;
                fillCC.Physician_Process = isPhysicianProcess;

                if (isPhysicianProcess)
                {
                    fillCC.CopypreviousEncounterList = GetComplaintsByEncID(previousEncounterId, isFromArchive);
                }

                CClist.Add(fillCC);
            }

            return CClist;
        }

        public int GetCCCount(ulong ulEncounterID)
        {
            ArrayList arrayList = null;
            IList<FillChiefComplaint> ccList = new List<FillChiefComplaint>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.ChiefComplaints.GetCount");
                query1.SetParameter(0, ulEncounterID);
                arrayList = new ArrayList(query1.List());
                iMySession.Close();
            }
            return arrayList.Count;
        }
        public string GetPurposeOfVisit(ulong ulEncounterID)
        {
            ArrayList arrayList = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.ChiefComplaints.GetPOV");
                query1.SetParameter(0, ulEncounterID);
                arrayList = new ArrayList(query1.List());
                iMySession.Close();
            }
            return arrayList.Count == 0 ? "" : arrayList[0].ToString();
        }
        public int GetLastRecord()
        {
            int iCount = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IList<FillChiefComplaint> ccList = new List<FillChiefComplaint>();
                IQuery query1 = iMySession.GetNamedQuery("Get.ChiefComplaints.LastRecord");
                ArrayList arrayList = new ArrayList(query1.List());
                if (arrayList.Count > 0)
                {
                    iCount = Convert.ToInt32(arrayList[0].ToString());
                }
                iMySession.Close();
            }
            return iCount;
        }
        public IList<FillChiefComplaint> FillCC(ulong ulEncounterID)
        {
            FillChiefComplaint fillCC;
            IList<FillChiefComplaint> ccList = new List<FillChiefComplaint>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.ChiefComplaints.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                if (arrayList.Count == 0)
                {
                    fillCC = new FillChiefComplaint();
                    ccList.Add(fillCC);
                }

                iMySession.Close();
            }
            return ccList;
        }
        public IList<FillChiefComplaint> FillPhrases(ulong ulEncounterID)
        {
            FillChiefComplaint fillCC = new FillChiefComplaint();
            IList<FillChiefComplaint> PhrasesList = new List<FillChiefComplaint>();

            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                fillCC.CurrentCCList = mySession.CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).List<ChiefComplaints>();

                if (fillCC.CurrentCCList.Count == 0 && fillCC.CurrentCCList != null)
                {
                    fillCC.Purpose_Of_Visit = GetPurposeOfVisit(ulEncounterID);
                }

                PhrasesList.Add(fillCC);
                mySession.Close();
            }
            return PhrasesList;
        }
        public IList<ChiefComplaints> GetComplaintsByEncID(ulong ulEncounterID, bool isFromArchive)
        {
            IList<ChiefComplaints> lstChiefComplaints = new List<ChiefComplaints>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                var querySQL = string.Format(@"SELECT E.* 
                                               FROM   {0} E 
                                               WHERE  E.ENCOUNTER_ID = :ENCOUNTER_ID",
                                               isFromArchive ? "CHIEF_COMPLAINT_ARC" : "CHIEF_COMPLAINT");

                var SQLQuery = iMySession.CreateSQLQuery(querySQL)
                       .AddEntity("E", typeof(ChiefComplaints));

                SQLQuery.SetParameter("ENCOUNTER_ID", ulEncounterID);

                lstChiefComplaints = SQLQuery.List<ChiefComplaints>();

                iMySession.Close();
            }
            return lstChiefComplaints;

        }
        public IList<ChiefComplaints> GetComplaintsByCCID(int ulCCID, ulong ulEncounterID)
        {
            IList<ChiefComplaints> ChiefComplaintsList = new List<ChiefComplaints>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ChiefComplaints))
                                           .Add(Expression.Eq("CC_Group_ID", ulCCID))
                                           .Add(Expression.Eq("Encounter_ID", ulEncounterID));
                ChiefComplaintsList = crit.List<ChiefComplaints>();
                iMySession.Close();
            }

            return ChiefComplaintsList;
        }

        //public IList<FillChiefComplaint> UpdateComplaints(IList<ChiefComplaints> addList, IList<ChiefComplaints> updateList, IList<ChiefComplaints> deleteList, string macAddress)
        //{
        //    SaveUpdateDeleteWithTransaction(ref addList, updateList, deleteList, macAddress);
        //    IList<FillChiefComplaint> fillCClst = new List<FillChiefComplaint>();
        //    FillChiefComplaint fillCC = new FillChiefComplaint();
        //    IList<ChiefComplaints> filllst = new List<ChiefComplaints>();
        //    GenerateXml XMLObj = new GenerateXml();
        //    ulong encounterid = 0;

        //    if (addList.Count > 0)
        //    {
        //        foreach (ChiefComplaints obj in addList)
        //            filllst.Add(obj);
        //        encounterid = addList[0].Encounter_ID;
        //    }

        //    if (updateList.Count > 0)
        //    {
        //        encounterid = updateList[0].Encounter_ID;
        //        foreach (var obj in updateList)
        //            obj.Version += 1;
        //        foreach (ChiefComplaints obj in updateList)
        //            filllst.Add(obj);
        //    }

        //    List<object> lstObj = filllst.Cast<object>().ToList();

        //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
        //    fillCC.CurrentCCList = filllst;
        //    fillCClst.Add(fillCC);

        //    return fillCClst;
        //}
        public IList<FillChiefComplaint> UpdateComplaints(ref IList<ChiefComplaints> addList, ref IList<ChiefComplaints> updateList, IList<ChiefComplaints> deleteList,ref IList<Encounter> objEncounter,string macAddress)
        {
            IList<ChiefComplaints> SaveUpdateList = new List<ChiefComplaints>();
            if (addList != null && addList.Count > 0)
                SaveUpdateList = addList;
            if (updateList != null && updateList.Count > 0)
                SaveUpdateList = SaveUpdateList.Concat(updateList).ToList();
            ulong encounter_id = 0;
            if (SaveUpdateList.Count > 0)
            {
                encounter_id = SaveUpdateList[0].Encounter_ID;
                SaveUpdateDelete_DBAndXML_WithTransaction(ref addList, ref updateList, deleteList, macAddress, true, false, encounter_id, string.Empty);
            }

            SaveUpdateList = new List<ChiefComplaints>();
            if (addList != null && addList.Count > 0)
                SaveUpdateList = addList;
            if (updateList != null && updateList.Count > 0)
                SaveUpdateList = SaveUpdateList.Concat(updateList).ToList();

            IList<FillChiefComplaint> fillCClst = new List<FillChiefComplaint>();
            FillChiefComplaint fillCC = new FillChiefComplaint();
            IList<ChiefComplaints> filllst = new List<ChiefComplaints>();

            fillCC.CurrentCCList = SaveUpdateList;
            fillCClst.Add(fillCC);

            EncounterManager ObjEncMngr = new EncounterManager();
            IList<Encounter> objEnc = new List<Encounter>();
            if (objEncounter.Count > 0)
                objEncounter = ObjEncMngr.UpdateEncounterPaperOrder(objEncounter, string.Empty);
            return fillCClst;
        }
        //public IList<FillChiefComplaint> DeleteComplaints(IList<ChiefComplaints> complaintList, string macAddress)
        //{
        //    IList<FillChiefComplaint> gridlist = new List<FillChiefComplaint>();
        //    IList<ChiefComplaints> nullList = new List<ChiefComplaints>();

        //    nullList = null;
        //    SaveUpdateDeleteWithTransaction(ref nullList, null, complaintList, macAddress);

        //    gridlist = FillPhrases(complaintList[0].Encounter_ID);
        //    return gridlist;
        //}
        public IList<FillChiefComplaint> DeleteComplaints(IList<ChiefComplaints> complaintList, string macAddress)
        {
            IList<FillChiefComplaint> gridlist = new List<FillChiefComplaint>();
            IList<ChiefComplaints> nullList = new List<ChiefComplaints>();
            nullList = null;
            IList<ChiefComplaints> nullList1 = new List<ChiefComplaints>();
            nullList = null;
            //ulong encounter_id = 0;
            //SaveUpdateDeleteWithTransaction(ref nullList, null, complaintList, macAddress);
            if (complaintList.Count > 0)
                SaveUpdateDelete_DBAndXML_WithTransaction(ref nullList, ref nullList1, complaintList, macAddress, false, true, 0, string.Empty);
            gridlist = FillPhrases(complaintList[0].Encounter_ID);
            return gridlist;
        }

        //public IList<FillChiefComplaint> SaveComplaints(IList<ChiefComplaints> complaintList, string macAddress)
        //{
        //    GenerateXml XMLObj = new GenerateXml();
        //    IList<FillChiefComplaint> fillcclst = new List<FillChiefComplaint>();
        //    FillChiefComplaint fillcc = new FillChiefComplaint();

        //    SaveUpdateDeleteWithTransaction(ref complaintList, null, null, macAddress);

        //    fillcc.CurrentCCList = complaintList;
        //    fillcclst.Add(fillcc);

        //    if (complaintList.Count > 0)
        //    {
        //        ulong encounterid = complaintList[0].Encounter_ID;
        //        List<object> lstObj = complaintList.Cast<object>().ToList();
        //        XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
        //    }
        //    return fillcclst;
        //}
        public IList<FillChiefComplaint> SaveComplaints(IList<ChiefComplaints> complaintList,ref IList<Encounter> objEncounter, string macAddress)
        {
            GenerateXml XMLObj = new GenerateXml();
            IList<FillChiefComplaint> fillcclst = new List<FillChiefComplaint>();
            FillChiefComplaint fillcc = new FillChiefComplaint();
            EncounterManager ObjEncMngr = new EncounterManager();
            IList<Encounter> objEnc = new List<Encounter>();
            //SaveUpdateDeleteWithTransaction(ref complaintList, null, null, macAddress);
            IList<ChiefComplaints> nullList = new List<ChiefComplaints>();
            IList<ChiefComplaints> deleteListnull = new List<ChiefComplaints>();
            ulong encounter_id = 0;
            if (complaintList.Count > 0 && complaintList != null)
            {
                encounter_id = complaintList[0].Encounter_ID;
                SaveUpdateDelete_DBAndXML_WithTransaction(ref complaintList, ref nullList, deleteListnull, macAddress, true, false, encounter_id, string.Empty);
            }
            fillcc.CurrentCCList = complaintList;
            fillcclst.Add(fillcc);
            if (objEncounter.Count>0 && objEncounter != null)
                objEncounter = ObjEncMngr.UpdateEncounterPaperOrder(objEncounter, string.Empty);
            //if (complaintList.Count > 0)
            //{
            //    ulong encounterid = complaintList[0].Encounter_ID;
            //    List<object> lstObj = complaintList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //}
            return fillcclst;
        }
        public ulong getHumanID(ulong ulEncounterID)
        {
            ulong dat = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Get.Encounter.HumanID");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());

                if (arrayList.Count > 0)
                    dat = Convert.ToUInt32(arrayList[0].ToString());
                iMySession.Close();
            }
            return dat;
        }
        public ulong getPhysicianID(ulong ulEncounterID)
        {
            ulong dat = 0;

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Get.chief_complaint.PhysicianID");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());

                if (arrayList.Count > 0)//added by vijayan
                    dat = Convert.ToUInt32(arrayList[0].ToString());
                iMySession.Close();
            }
            return dat;
        }
        public IList<ChiefComplaints> GetPNChiefComplaints(ulong ulEncounterID)
        {
            IList<ChiefComplaints> ccList = new List<ChiefComplaints>();
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IQuery query1 = iMySession.GetNamedQuery("Fill.PNChiefComplaints.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                if(arrayList.Count>0)
                {
                for (int i = 0; i < arrayList.Count; i++)
                {
                    ChiefComplaints fillCC = new ChiefComplaints();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillCC.HPI_Element = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillCC.HPI_Value = ccObject[1].ToString();
                    ccList.Add(fillCC);
                }
                }
                iMySession.Close();
            }
            return ccList;
        }
        public IList<PatientResults> GetPNVitals(ulong ulEncounterID)
        {
            IList<PatientResults> vitalsList = new List<PatientResults>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNVitals.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                if(arrayList.Count>0)
                { 
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PatientResults fillVitals = new PatientResults();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillVitals.Loinc_Observation = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillVitals.Value = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillVitals.Created_Date_And_Time = Convert.ToDateTime(ccObject[2].ToString());
                    vitalsList.Add(fillVitals);
                }
                }
                iMySession.Close();
            }
            return vitalsList;
        }
        public IList<ROS> GetPNROS(ulong ulEncounterID)
        {
            IList<ROS> rosList = new List<ROS>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNROS.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                if (arrayList.Count > 0)
                {
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        ROS fillROS = new ROS();
                        object[] ccObject = (object[])arrayList[i];
                        if (ccObject[0] != null)
                        fillROS.System_Name = ccObject[0].ToString();
                        if (ccObject[1] != null)
                        fillROS.Symptom_Name = ccObject[1].ToString();
                        if (ccObject[2] != null)
                        fillROS.Created_Date_And_Time = Convert.ToDateTime(ccObject[2].ToString());
                        rosList.Add(fillROS);
                    }
                }
                iMySession.Close();
            }
            return rosList;
        }
        //public IList<Medication> GetPNMedication(ulong ulEncounterID)
        //{
        //    IList<Medication> medicationList = new List<Medication>();
        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = iMySession.GetNamedQuery("Fill.PNMedicationHistory.List");
        //        query1.SetParameter(0, getHumanID(ulEncounterID));
        //        ArrayList arrayList = new ArrayList(query1.List());
        //        for (int i = 0; i < arrayList.Count; i++)
        //        {
        //            Medication fillmedication = new Medication();
        //            object[] ccObject = (object[])arrayList[i];
        //            if (ccObject[0] != null)
        //            fillmedication.Drug_Name = ccObject[0].ToString();
        //            if (ccObject[1] != null)
        //            fillmedication.Dosage = ccObject[1].ToString();
        //            if (ccObject[2] != null)
        //            fillmedication.Frequency = ccObject[2].ToString();
        //            if (ccObject[3] != null)
        //            fillmedication.Route_Of_Administration = ccObject[3].ToString();
        //            if (ccObject[4] != null)
        //            fillmedication.Medication_Notes = ccObject[4].ToString();
        //            medicationList.Add(fillmedication);
        //        }
        //        iMySession.Close();
        //    }
        //    return medicationList;
        //}

        public IList<Examination> GetPNExamination(ulong ulEncounterID)
        {
            IList<Examination> examinationList = new List<Examination>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNExamination.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Examination fillExamination = new Examination();
     
                    object[] ccObject = (object[])arrayList[i];
                    // fillExamination.System_Name = ccObject[0].ToString();
                    // fillExamination.Condition_Name = ccObject[1].ToString();
                    // fillExamination.Status = ccObject[2].ToString();
                    // fillExamination.Exam_Notes = ccObject[3].ToString();
                    examinationList.Add(fillExamination);
                }
                iMySession.Close();
            }
            return examinationList;
        }
        public IList<Assessment> GetPNAssessment(ulong ulEncounterID)
        {
            IList<Assessment> AssessmentList = new List<Assessment>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNAssessment.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Assessment fillAssessment = new Assessment();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillAssessment.ICD_9 = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillAssessment.ICD_Description = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillAssessment.Assessment_Notes = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillAssessment.Assessment_Status = ccObject[3].ToString();
                    AssessmentList.Add(fillAssessment);
                }
                iMySession.Close();
            }
            return AssessmentList;
        }
        //public IList<AssessmentSource> GetPNAssessmentSource(ulong ulEncounterID)
        //{
        //    IList<AssessmentSource> assessmentSourceList = new List<AssessmentSource>();
        //    IQuery query1 = session.GetISession().GetNamedQuery("Fill.PNAssessmentSource.List");
        //    query1.SetParameter(0, ulEncounterID);
        //    ArrayList arrayList = new ArrayList(query1.List());
        //    for (int i = 0; i < arrayList.Count; i++)
        //    {
        //        AssessmentSource fillAssessmentSource = new AssessmentSource();
        //        object[] ccObject = (object[])arrayList[i];
        //        fillAssessmentSource.Source_For_Diagnosis = ccObject[0].ToString();
        //        fillAssessmentSource.Date = Convert.ToDateTime(ccObject[1].ToString());
        //        fillAssessmentSource.Notes = ccObject[2].ToString();
        //        assessmentSourceList.Add(fillAssessmentSource);
        //    }
        //    return assessmentSourceList;
        //}
        public IList<TreatmentPlan> GetPNTreatmentPlan(ulong ulEncounterID)
        {
            IList<TreatmentPlan> treatmentPlanList = new List<TreatmentPlan>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNTreatmentPlan.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                if (arrayList.Count > 0)
                {
                    TreatmentPlan fillTreatmentPlan = new TreatmentPlan();
                    fillTreatmentPlan.Plan = arrayList[0].ToString();
                    treatmentPlanList.Add(fillTreatmentPlan);
                }
                iMySession.Close();
            }
            return treatmentPlanList;
        }
        public IList<PastMedicalHistory> GetPNPastMedicalHistory(ulong ulEncounterID)
        {
            IList<PastMedicalHistory> medicalHistoryList = new List<PastMedicalHistory>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNPastMedicalHistory.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PastMedicalHistory fillmedicalHistory = new PastMedicalHistory();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillmedicalHistory.Past_Medical_Info = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillmedicalHistory.From_Date = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillmedicalHistory.To_Date = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillmedicalHistory.AHA_Question_ICD = ccObject[3].ToString();
                    medicalHistoryList.Add(fillmedicalHistory);
                }
                iMySession.Close();
            }
            return medicalHistoryList;
        }
        public IList<SurgicalHistory> GetPNSurgicalHistory(ulong ulEncounterID)
        {
            IList<SurgicalHistory> SurgicalHistoryList = new List<SurgicalHistory>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNSurgicalHistory.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    SurgicalHistory fillSurgicalHistory = new SurgicalHistory();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillSurgicalHistory.Surgery_Name = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillSurgicalHistory.Description = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillSurgicalHistory.Date_Of_Surgery = ccObject[2].ToString();
                    SurgicalHistoryList.Add(fillSurgicalHistory);
                }
                iMySession.Close();
            }
            return SurgicalHistoryList;
        }
        public IList<HospitalizationHistory> GetPNHospitalizationHistory(ulong ulEncounterID)
        {
            IList<HospitalizationHistory> HospitalizationHistoryList = new List<HospitalizationHistory>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNHospitalizationHistory.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    HospitalizationHistory fillHospitalizationHistory = new HospitalizationHistory();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillHospitalizationHistory.Reason_For_Hospitalization = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillHospitalizationHistory.Hospitalization_Notes = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillHospitalizationHistory.From_Date = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillHospitalizationHistory.To_Date = ccObject[3].ToString();
                    HospitalizationHistoryList.Add(fillHospitalizationHistory);
                }
                iMySession.Close();
            }
            return HospitalizationHistoryList;
        }
        public IList<FamilyHistory> GetPNFamilyHistory(ulong ulEncounterID)
        {
            IList<FamilyHistory> FamilyHistoryList = new List<FamilyHistory>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNFamilyHistory.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    FamilyHistory fillFamilyHistory = new FamilyHistory();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillFamilyHistory.RelationShip = ccObject[0].ToString();
                    //fillFamilyHistory.Family_History_Notes = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillFamilyHistory.Id = Convert.ToUInt32(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    fillFamilyHistory.Age = Convert.ToInt32(ccObject[3].ToString());
                    if (ccObject[4] != null)
                    fillFamilyHistory.Status = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillFamilyHistory.Cause_Of_Death = ccObject[5].ToString();
                    FamilyHistoryList.Add(fillFamilyHistory);
                }
                iMySession.Close();
            }
            return FamilyHistoryList;
        }
        public IList<SocialHistory> GetPNSocialHistory(ulong ulEncounterID)
        {
            IList<SocialHistory> SocialHistoryList = new List<SocialHistory>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNSocialHistory.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    SocialHistory fillSocialHistory = new SocialHistory();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillSocialHistory.Social_Info = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillSocialHistory.Description = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillSocialHistory.Value = ccObject[2].ToString();
                    SocialHistoryList.Add(fillSocialHistory);
                }
                iMySession.Close();
            }
            return SocialHistoryList;
        }
        public IList<NonDrugAllergy> GetPNNonDrugAllergy(ulong ulEncounterID)
        {
            IList<NonDrugAllergy> NonDrugAllergyHistoryList = new List<NonDrugAllergy>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNNonDrugAllergy.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    NonDrugAllergy fillNonDrugAllergyHistory = new NonDrugAllergy();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillNonDrugAllergyHistory.Non_Drug_Allergy_History_Info = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillNonDrugAllergyHistory.Description = ccObject[1].ToString();
                    NonDrugAllergyHistoryList.Add(fillNonDrugAllergyHistory);
                }
                iMySession.Close();
            }
            return NonDrugAllergyHistoryList;
        }
        public IList<GeneralNotes> GetPNGeneralNotes(ulong ulEncounterID)
        {
            IList<GeneralNotes> generalNotesList = new List<GeneralNotes>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNGeneralNotes.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    GeneralNotes generalNotes = new GeneralNotes();
                    object[] ccObject = (object[])arrayList[i];
                    generalNotes.Parent_Field = ccObject[0].ToString();
                    generalNotes.Notes = ccObject[1].ToString();
                    generalNotesList.Add(generalNotes);
                }
                iMySession.Close();
            }
            return generalNotesList;
        }
        public IList<FamilyDisease> GetPNFamilyDiseaseDetails()
        {
            IList<FamilyDisease> familyDiseaseList = new List<FamilyDisease>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNDiseaseDetails.List");
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    FamilyDisease familyDisease = new FamilyDisease();
                    object[] ccObject = (object[])arrayList[i];
                    familyDisease.Family_History_ID = Convert.ToUInt32(ccObject[0].ToString());
                    familyDisease.Disease = ccObject[1].ToString();
                    familyDiseaseList.Add(familyDisease);
                }
                iMySession.Close();
            }
            return familyDiseaseList;
        }
        public IList<Encounter> GetPNEncounterDetails(ulong ulEncounterID)
        {
            IList<Encounter> encounterList = new List<Encounter>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNEncounterDetails.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Encounter encounter = new Encounter();
                    object[] ccObject = (object[])arrayList[i];
                    encounter.Source_Of_Information = ccObject[0].ToString();
                    encounter.If_Source_Of_Information_Others = ccObject[1].ToString();
                    encounter.Is_Hospitalization_Denied = ccObject[2].ToString();
                    encounterList.Add(encounter);
                }
                iMySession.Close();
            }
            return encounterList;
        }
        //public FillProgressNotes GetProgressNotesList(ulong ulEncounterID)
        //{
        //    FillProgressNotes fillPN = new FillProgressNotes();
        //    fillPN.ChiefComplaints = GetPNChiefComplaints(ulEncounterID);
        //    fillPN.Vitals = GetPNVitals(ulEncounterID);
        //    fillPN.Medication = GetPNMedication(ulEncounterID);
        //    fillPN.Examination = GetPNExamination(ulEncounterID);
        //    fillPN.Assessment = GetPNAssessment(ulEncounterID);
        //    // fillPN.AssessmentSource = GetPNAssessmentSource(ulEncounterID);
        //    fillPN.TreatmentPlan = GetPNTreatmentPlan(ulEncounterID);
        //    fillPN.PastMedicalHistory = GetPNPastMedicalHistory(ulEncounterID);
        //    fillPN.SocialHistory = GetPNSocialHistory(ulEncounterID);
        //    fillPN.SurgicalHistory = GetPNSurgicalHistory(ulEncounterID);
        //    fillPN.FamilyHistory = GetPNFamilyHistory(ulEncounterID);
        //    fillPN.HospitalizationHistory = GetPNHospitalizationHistory(ulEncounterID);
        //    fillPN.NonDrugAllergy = GetPNNonDrugAllergy(ulEncounterID);
        //    fillPN.GeneralNotes = GetPNGeneralNotes(ulEncounterID);
        //    fillPN.FamilyDisease = GetPNFamilyDiseaseDetails();
        //    fillPN.Encounter = GetPNEncounterDetails(ulEncounterID);
        //    fillPN.HumanID = getHumanID(ulEncounterID);
        //    return fillPN;
        //}
        public IList<CarePlan> GetCarePlan(ulong ulEncounterID)
        {
            IList<CarePlan> ilstCarePlan=new List<CarePlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria Criteria = session.GetISession().CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Status","Yes")).Add(Restrictions.Not(Restrictions.Eq("Care_Plan_Notes",string.Empty)));
                ilstCarePlan = Criteria.List<CarePlan>();
            }
            return ilstCarePlan;
        }
        public IList<TreatmentPlan> GetTreatmentPlanForcareplan(ulong ulEncounterID)
        {
            IList<TreatmentPlan> ilstTreatmentPlan = new List<TreatmentPlan>();
            List<string> ilstPlanType = new List<string>();
            ilstPlanType.Add("ASSESSMENT");
            ilstPlanType.Add("PLAN");
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ICriteria Criteria = session.GetISession().CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", ulEncounterID)).Add(Expression.Eq("Plan_Type", "ASSESSMENT")).Add(Restrictions.Not(Restrictions.Eq("Plan",string.Empty)));
                ICriteria Criteria = session.GetISession().CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", ulEncounterID)).Add(Expression.In("Plan_Type", ilstPlanType)).Add(Restrictions.Not(Restrictions.Eq("Plan",string.Empty)));
                ilstTreatmentPlan = Criteria.List<TreatmentPlan>();
            }
            return ilstTreatmentPlan;
        }

        public FillClinicalSummary GetCarePlanCDA(ulong ulEncounterID, ulong ulHumanID)
        {
            FillClinicalSummary fillCN = new FillClinicalSummary();
            fillCN.Encounter = GetCSEncounterDetails(ulEncounterID);
            fillCN.Careplan = GetCSCarePlanListforcareplan(ulEncounterID);
            fillCN.Vitals = GetCSVitalsForCareplan(ulEncounterID);
            //fillCN.ProblemListing = GetCSProblemList(ulHumanID);
            fillCN.ProblemListing = GetHealthConcernProblemList(ulHumanID);

            fillCN.TreatmentPlan = GetTreatmentPlanForcareplan(ulEncounterID);
            GetCSHuman(ulHumanID, ref fillCN);
            fillCN.facilityLibraryCustodian = GetFacilityInfoforCustadian(ulEncounterID);
            fillCN.PhysicianLibraryDocumentation = GetPhysicianLibraryDocumentation(ulEncounterID);
            fillCN.lookupList = GetRaceAndEthnicity(ulHumanID);
            fillCN.lookupValues = GetCSlookUpValues();
            fillCN.Assessment = GetCSAssessment(ulEncounterID);
            if (fillCN.Encounter != null)
            {
                fillCN.phyList = GetCSPhyList(Convert.ToUInt64(fillCN.Encounter[0].Encounter_Provider_ID));
            }
            return fillCN;
        }
        //Syndromic Surveillance
        public IList<SocialHistory> GetCSSocialHistoryTobacco(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<SocialHistory> SocialHistoryList = new List<SocialHistory>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria Criteria = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Encounter_ID", ulEncounterID));
                SocialHistoryList = Criteria.List<SocialHistory>();
                iMySession.Close();
            }
            return SocialHistoryList;
        }
        public IList<HospitalizationHistory> GetCSHospitalizationHistory(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<HospitalizationHistory> HospitalizationHistoryList = new List<HospitalizationHistory>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria Criteria = iMySession.CreateCriteria(typeof(HospitalizationHistory)).Add(Expression.Eq("Encounter_Id", ulEncounterID));
                HospitalizationHistoryList = Criteria.List<HospitalizationHistory>();
                iMySession.Close();
            }
            return HospitalizationHistoryList;
        }
        public FillClinicalSummary GetSyndromicSurveillance(ulong ulEncounterID, ulong ulHumanID)
        {
            FillClinicalSummary fillCN = new FillClinicalSummary();
            fillCN.ChiefComplaints = GetCSChiefComplaints(ulEncounterID);
            fillCN.Vitals = GetCSVitals(ulEncounterID);
            fillCN.SocialHistory = GetCSSocialHistoryTobacco(ulEncounterID);
            fillCN.Assessment = GetCSAssessment(ulEncounterID);
            //fillCN.ProblemListing = GetCSProblemList(ulHumanID);
            fillCN.Encounter = GetCSEncounterDetails(ulEncounterID);
            GetCSHuman(ulHumanID, ref fillCN);
            ulong HumanID = fillCN.ID;
            fillCN.lookupList = GetRaceAndEthnicity(ulHumanID);  //Added BY manimara on 27-08-2014
            fillCN.lookupValues = GetCSlookUpValues();
            if (fillCN.Encounter != null && fillCN.Encounter.Count>0)
            {
                if (fillCN.Encounter[0].Encounter_Provider_Review_ID != 0)
                {
                    fillCN.phyList = GetCSPhyList(Convert.ToUInt64(fillCN.Encounter[0].Encounter_Provider_Review_ID));
                }
                else
                {
                    fillCN.phyList = GetCSPhyList(Convert.ToUInt64(fillCN.Encounter[0].Encounter_Provider_ID));
                }
            }
            fillCN.HospitalizationHistory = GetCSHospitalizationHistory(ulEncounterID);
            return fillCN;
        }
        public FillClinicalSummary GetClinicalSummary(ulong ulEncounterID, ulong ulHumanID)
        {
            FillClinicalSummary fillCN = new FillClinicalSummary();
            fillCN.ChiefComplaints = GetCSChiefComplaints(ulEncounterID);
            fillCN.Vitals = GetCSVitals(ulEncounterID);
            fillCN.SocialHistory = GetCSSocialHistory(ulEncounterID, ulHumanID);
            fillCN.Assessment = GetCSAssessment(ulEncounterID);
            //fillCN.ProblemListing = GetCSProblemList(ulHumanID);//for reconcilation v include completed and active status also
            fillCN.ProblemListing = GetCSProblemListActiveandresolved(ulHumanID);
            fillCN.Allergy = GetCSRCopiaAllergy(ulHumanID);
            fillCN.Medication = GetCSMedication(ulHumanID);
            fillCN.ImmunizationList = GetCSImmunization(ulEncounterID, ulHumanID);
            fillCN.Encounter = GetCSEncounterDetails(ulEncounterID);
            fillCN.laborderList = GetCSLabOrderList(ulEncounterID);
            fillCN.referralorderList = GetCSReferralOrderList(ulEncounterID);
            fillCN.Pat_Ins_Plan = GetCSPatInsPlan(ulHumanID);
            fillCN.ResultList = GetCSResultList(ulHumanID);
            GetCSHuman(ulHumanID, ref fillCN);
            ulong HumanID = fillCN.ID;
            fillCN.lookupList = GetRaceAndEthnicity(ulHumanID);  //Added BY manimara on 27-08-2014
            fillCN.immunhistoryList = GetCSImmunizationHistory(HumanID);
            fillCN.lookupValues = GetCSlookUpValues();
            fillCN.VaccineCodes = GetCSVaccineCodes();

            if (fillCN.Encounter != null  && fillCN.Encounter.Count>0)
            {
                if (fillCN.Encounter[0].Encounter_Provider_Review_ID != 0)
                {
                    fillCN.phyList = GetCSPhyList(Convert.ToUInt64(fillCN.Encounter[0].Encounter_Provider_Review_ID));
                }
                else
                {
                    fillCN.phyList = GetCSPhyList(Convert.ToUInt64(fillCN.Encounter[0].Encounter_Provider_ID));
                }
            }
            fillCN.MedicationAdministrative = GetCSMedicationAdministrative(ulHumanID);
            fillCN.ReferralOrder = GetCSReferralOrderforFutureAppointment(ulEncounterID);
            fillCN.OrdersTestPending_Future = GetCSTestforPendingandFuture(ulEncounterID);
            fillCN.Care_Plan_Cognitive_Function = GetCSCarePlanforCognitiveFunction(ulEncounterID);
            fillCN.Document_Material = GetCSRecomentedmaterial(ulEncounterID);
            fillCN.EncounterDiagnosis = GetCSEncounterDiagnosis(ulEncounterID);
            fillCN.facilityLibraryCustodian = GetFacilityInfoforCustadian(ulEncounterID);
            fillCN.PhysicianLibraryDocumentation = GetPhysicianLibraryDocumentation(ulEncounterID);
            fillCN.HospitalizationHistory = GetHospitalizationHistory(ulEncounterID);
            fillCN.ImplantProcedure = GetImplantableDeviceInformationByHumanID(ulHumanID);
            fillCN.Care_Plan_Cognitive_Function_MentalStatus = GetCSCarePlanforCognitiveFunctionMentalStaus(ulEncounterID);
            fillCN.Care_Plan_FunctionalStatus = GetCSCarePlanforFunctionalStaus(ulEncounterID);
            fillCN.OthertreatmentPlan = GetOtherTreatmentPlan(ulEncounterID);
            fillCN.TreatmentPlan = GetTreatmentPlan(ulEncounterID);
            fillCN.HealthConcernProblemList = GetHealthConcernProblemList(ulHumanID);
            fillCN.LabLocationList = GetCSLabList(ulHumanID);            
          // fillCN.Physician_Facility_ID
            //fillCN.addendumNotes = GetCSAddendumNotesList(ulEncounterID);
            //fillCN.imageorderList = GetCSImageOrderList(ulEncounterID);
            //fillCN.medicationHistory = GetCSMedicationHistory(ulEncounterID);
            //fillCN.ROS = GetCSROS(ulEncounterID);
            //fillCN.ROSNotes = GetCSROSNotes(ulEncounterID);
            //fillCN.Examination = GetCSExamination(ulEncounterID);
            //fillCN.AssessmentNotes = GetCSAssessmentNotes(ulEncounterID);
            //fillCN.NonDrugAllergyNotes = GetCSNonDrugAllergyNotes(ulEncounterID);
            //fillCN.PastMedicalHistory = GetCSPastMedicalHistory(ulEncounterID);
            //fillCN.PastMedicalHistoryNotes = GetCSPastMedicalHistoryNotes(ulEncounterID);
            //fillCN.NonDrugAllergy = GetCSNonDrugAllergy(ulEncounterID);

            //IList<string> RXCode = new List<string>();
            //for (int i = 0; i < fillCN.Medication.Count; i++)
            //{
            //    ISQLQuery sql = session.GetISession().CreateSQLQuery("SELECT RXCUI FROM rxnsat where ATV='" + fillCN.Medication[i].NDC_ID + "' and SAB='RXNORM'");
            //    if (sql.List<string>().Count != 0)
            //    {
            //        RXCode.Add(sql.List<string>()[0]);
            //    }
            //    else
            //    {
            //        RXCode.Add(string.Empty);
            //    }
            //}
            //fillCN.RXNormList = RXCode;
            fillCN.OrdersList = GetCSOrders(ulEncounterID);
            //fillCN.InHouseProcList = GetCSInHouseProcedures(ulEncounterID);
            //fillCN.TreatmentPlan = GetCSTreatmentPlan(ulHumanID);
            //fillCN.CarePlanList = GetCSCarePlanList(ulEncounterID);
            //fillCN.PrventivePlan = GetCSPreventivePlanList(ulEncounterID);
            //fillCN.FamilyHistory = GetCSFamilyHistory(ulEncounterID);
            //fillCN.familyHistoryNotes = GetCSFamilyHistoryNotes(ulEncounterID);
            
            // fillCN.ResultSubEntry = GetCSManualResultList(ulEncounterID);
            //IList<FamilyDisease> temp = new List<FamilyDisease>();
            //for (int i = 0; i < fillCN.FamilyHistory.Count; i++)
            //{
            //    temp = GetCSFamilyDiseaseDetails(fillCN.FamilyHistory[i].Id);
            //    for (int j = 0; j < temp.Count; j++)
            //    {
            //        fillCN.FamilyDisease.Add(temp[j]);
            //    }
            //}

           
            //fillCN.SocialHistoryNotes = GetCSSocialHistoryNotes(ulEncounterID);
            //fillCN.AdvanceDirectiveList = GetCSAdvanceDirective(ulEncounterID);
            //fillCN.PhysicianList = GetCSPhysicianPatient(ulEncounterID);
           
            //fillCN.HumanID = getHumanID(ulEncounterID);
            //fillCN.eprescriptionList = GetCSEPrescriptionList(ulEncounterID);
            
            //fillCN.CarePlanLookup = GetCarePlanList(ulEncounterID);
            //fillCN.ClinicalInstructionDocuments = GetClinicalInstruction(ulEncounterID);
            
            //FacilityManager objFacilityMngr = new FacilityManager();
            //for (int i = 0; i < fillCN.Encounter.Count; i++)
            //{
            //    FacilityLibrary objfacilitylib = new FacilityLibrary();
            //    objfacilitylib = objFacilityMngr.GetFacilityByFacilityname(fillCN.Encounter[i].Facility_Name)[0];

            //    fillCN.facilityLibrary.Add(objfacilitylib);
            //}

            //fillCN.PatientResultsMeasure = GetPatientResultsMeasureList(ulEncounterID);
            return fillCN;
        }
        public IList<PatGuarantor> GetPatGuarantor(ulong ulHumanID, string sGuarantorRelationship)
        {
            IList<PatGuarantor> ilstPatGuarantor = new List<PatGuarantor>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria iCrit = iMySession.CreateCriteria(typeof(PatGuarantor)).Add(Expression.Eq("Human_ID", Convert.ToInt32(ulHumanID))).Add(Expression.Not(Expression.Eq("Relationship",sGuarantorRelationship)));
                ilstPatGuarantor = iCrit.List<PatGuarantor>();
            }
            return ilstPatGuarantor;
        }
        public IList<Human> GetHumanList(IList<ulong> ulHumanID)
        {
            IList<Human> ilstHuman = new List<Human>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria iCrit = iMySession.CreateCriteria(typeof(Human)).Add(Expression.In("Id", ulHumanID.ToArray()));
                ilstHuman = iCrit.List<Human>();
            }
            return ilstHuman;
        }
        //Bulk Export Immunization Registry
        public FillClinicalSummary GetClinicalSummaryBulk(ulong ulEncounterID, ulong ulHumanID)
        {
            FillClinicalSummary fillCN = new FillClinicalSummary();
            fillCN.ImmunizationList = GetCSImmunization(ulEncounterID, ulHumanID);
            fillCN.Encounter = GetCSEncounterDetails(ulEncounterID);
            fillCN.Pat_Ins_Plan = GetCSPatInsPlan(ulHumanID);
            GetCSHuman(ulHumanID, ref fillCN);
            ulong HumanID = fillCN.ID;
            fillCN.lookupList = GetRaceAndEthnicity(ulHumanID);  //Added BY manimara on 27-08-2014
            fillCN.immunhistoryList = GetCSImmunizationHistory(HumanID);
            fillCN.lookupValues = GetCSlookUpValues();

            if (fillCN.Encounter != null && fillCN.Encounter.Count>0)
            {
                if (fillCN.Encounter[0].Encounter_Provider_Review_ID != 0)
                {
                    fillCN.phyList = GetCSPhyList(Convert.ToUInt64(fillCN.Encounter[0].Encounter_Provider_Review_ID));
                }
                else
                {
                    fillCN.phyList = GetCSPhyList(Convert.ToUInt64(fillCN.Encounter[0].Encounter_Provider_ID));
                }
            }
            fillCN.VaccineCodes = GetCSVaccineCodes();
            fillCN.PatGuarantor = GetPatGuarantor(ulHumanID,fillCN.Guarantor_Relationship);
            if(fillCN.PatGuarantor !=null && fillCN.PatGuarantor.Count>0)
            {
                var ilstHuman = (from p in fillCN.PatGuarantor select Convert.ToUInt64(p.Guarantor_Human_ID)).ToList();
                fillCN.HumanList = GetHumanList(ilstHuman.ToList<ulong>());
            }
            
            return fillCN;
        }

        //End

        public IList<HospitalizationHistory> GetHospitalizationHistory(ulong ulEncounterID)
        {
            IList<HospitalizationHistory> HospitalizationHistoryList = new List<HospitalizationHistory>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.PNHospitalizationHistory.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    HospitalizationHistory fillHospitalizationHistory = new HospitalizationHistory();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillHospitalizationHistory.Reason_For_Hospitalization = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillHospitalizationHistory.Hospitalization_Notes = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillHospitalizationHistory.From_Date = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillHospitalizationHistory.To_Date = ccObject[3].ToString();
                    HospitalizationHistoryList.Add(fillHospitalizationHistory);
                }
                iMySession.Close();
                ulong Encoun_ID = 0;
                IList<HospitalizationHistory> HospitalizationHistoryListNew = new List<HospitalizationHistory>();
                IList<ulong> lstEncounterId = HospitalizationHistoryList.Where(w => w.Encounter_Id <= ulEncounterID).Select(Q => Q.Encounter_Id).OrderByDescending(a => a).ToList();
                foreach (ulong enc_id in lstEncounterId)
                {
                    if (HospitalizationHistoryList.Any(a => a.Encounter_Id == enc_id))
                    {
                        Encoun_ID = enc_id;
                        break;
                    }
                }

                if (HospitalizationHistoryList.Count > 0)
                {
                    HospitalizationHistoryListNew = (from p in HospitalizationHistoryList where p.Encounter_Id == Encoun_ID select p).ToList();
                }
            }
            return HospitalizationHistoryList;
        }
        private IList<PatientResults> GetPatientResultsMeasureList(ulong EncounterID)
        {
            IList<PatientResults> patientresultList = new List<PatientResults>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSQualityMeasurePatientResults.List");

                ulong ulHumanId = getHumanID(EncounterID);
                query1.SetParameter(0, ulHumanId);
                query1.SetParameter(1, ulHumanId);

                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PatientResults fillPatientList = new PatientResults();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillPatientList.Acurus_Result_Code = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillPatientList.Acurus_Result_Description = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillPatientList.Value = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillPatientList.Captured_date_and_time = Convert.ToDateTime(ccObject[3].ToString());
                    patientresultList.Add(fillPatientList);
                }
                iMySession.Close();
            }
            return patientresultList;
        }
        private IList<CarePlan> GetCarePlanList(ulong EncounterID)
        {
            IList<CarePlan> carePlanList = new List<CarePlan>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSQualityMeasureCarePlan.List");

                ulong ulHuman_ID = getHumanID(EncounterID);
                query1.SetParameter(0, ulHuman_ID);
                query1.SetParameter(1, EncounterID);
                query1.SetParameter(2, ulHuman_ID);
                query1.SetParameter(3, EncounterID);

                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    CarePlan fillCarePlan = new CarePlan();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillCarePlan.Care_Name_Value = ccObject[0].ToString();
                    if (ccObject[1] != null)
                        fillCarePlan.Status_Value = ccObject[1].ToString();
                    if (ccObject[2] != null)
                        fillCarePlan.Status = ccObject[2].ToString();

                    carePlanList.Add(fillCarePlan);
                }
                iMySession.Close();
            }
            return carePlanList;
        }
        private IList<ResultOBX> GetCSResultList(ulong ulHumanID)
        {
            IList<ResultOBX> ResultList = new List<ResultOBX>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSLabResultList.List");
                ulong ulHuman_ID = ulHumanID;
                //query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(0, ulHuman_ID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    ResultOBX fillResult = new ResultOBX();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillResult.OBX_Observation_Identifier = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillResult.OBX_Observation_Text = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillResult.OBX_Observation_Value = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillResult.OBX_Abnormal_Flag = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillResult.OBX_Date_And_Time_Of_Observation = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillResult.OBX_Units = ccObject[5].ToString();
                    if (ccObject[10] != null && ccObject[10].ToString().Trim() != string.Empty)
                    {
                        string formatString = "yyyyMMddHHmmss";
                        string sample = ccObject[10].ToString().Trim();
                        if (sample.Contains('-'))
                        {
                            sample = sample.Split('-')[0].ToString();
                        }
                        if (sample.Length == 14)
                            formatString = "yyyyMMddHHmmss";
                        else if (sample.Length == 12)
                            formatString = "yyyyMMddHHmm";
                        DateTime dt = DateTime.ParseExact(sample, formatString, null);
                        fillResult.Created_Date_And_Time = dt;
                    }                    
                    
                    fillResult.OBX_Loinc_Identifier = ccObject[7].ToString();
                    fillResult.OBX_Loinc_Observation_Text = ccObject[8].ToString();
                    fillResult.temp_property = ccObject[9].ToString();
                    fillResult.OBX_Reference_Range = ccObject[11].ToString();
                    if (ccObject[12] != null)
                    {
                        fillResult.OBX_Value_Type = ccObject[12].ToString();//Result_nte_comments
                    }
                   
                    ResultList.Add(fillResult);
                }
                iMySession.Close();
            }
            return ResultList;
        }
        //private IList<ResultSubEntry> GetCSManualResultList(ulong ulEncounterID)
        //{
        //    IList<ResultSubEntry> ResultList = new List<ResultSubEntry>();
        //    IQuery query1 = session.GetISession().GetNamedQuery("Fill.CSManualLabResultList.List");
        //    query1.SetParameter(0, ulEncounterID);
        //    ArrayList arrayList = new ArrayList(query1.List());
        //    for (int i = 0; i < arrayList.Count; i++)
        //    {
        //        ResultSubEntry fillResult = new ResultSubEntry();
        //        object[] ccObject = (object[])arrayList[i];
        //        fillResult.Loinc_Num = ccObject[0].ToString();
        //        fillResult.Component = ccObject[1].ToString();
        //        fillResult.Result = ccObject[2].ToString();
        //        fillResult.Units = ccObject[3].ToString();
        //        fillResult.Flag = ccObject[4].ToString();
        //        fillResult.Created_Date_And_Time = Convert.ToDateTime(ccObject[5].ToString());
        //        ResultList.Add(fillResult);
        //    }
        //    return ResultList;
        //}
        private void GetCSHuman(ulong ulHumanID, ref FillClinicalSummary FillCN)
        {
            Human hn = new Human();
            IList<Human> lsthuman=new List<Human>();
            HumanManager humanMngr = new HumanManager();
            if (humanMngr.GetPatientDetailsUsingPatientInformattion(ulHumanID).Count > 0)
            {
                hn = humanMngr.GetPatientDetailsUsingPatientInformattion(ulHumanID)[0];
                FillCN.First_Name = hn.First_Name;
                FillCN.Last_Name = hn.Last_Name;
                //FillCN.Patient_status = hn.Patient_Status;
                FillCN.MI = hn.MI;
                FillCN.Suffix = hn.Suffix;
                //FillCN.Date_Of_Death = hn.Date_Of_Death;
                FillCN.Sex = hn.Sex;
                FillCN.Birth_Date = hn.Birth_Date;
                FillCN.ID = hn.Id;
                //FillCN.Medical_Record_Number = hn.Medical_Record_Number;
                //FillCN.Prefix = hn.Prefix;
                FillCN.Street_Address1 = hn.Street_Address1;
                FillCN.City = hn.City;
                FillCN.State = hn.State;
                FillCN.ZipCode = hn.ZipCode;
                FillCN.Previous_Name = hn.Previous_Name;
                FillCN.Granulary_Race = hn.Granularity;
                lsthuman.Add(hn);
                FillCN.HumanList = lsthuman;
                //FillCN.Race = hn.Race;
                //FillCN.Ethnicity = hn.Ethnicity;
                //Added by Saravanan
                IList<StaticLookup> ilstStaticLookup = new List<StaticLookup>();
                StaticLookupManager objStaticLookupMngr = new StaticLookupManager();
                if (hn.Preferred_Language != string.Empty)
                {
                    ilstStaticLookup = objStaticLookupMngr.getStaticLookupByFieldNameAndValue("PREFERRED LANGUAGE", hn.Preferred_Language);
                    if (ilstStaticLookup.Count > 0)
                        FillCN.Preferred_Language = ilstStaticLookup[0].Description;
                }
                //FillCN.Preferred_Language = hn.Preferred_Language;
                FillCN.EMail = hn.EMail;
                FillCN.Home_Phone_No = hn.Home_Phone_No;

                //new 
                //FillCN.Guarantor_human_city = hn.Guarantor_City;
                //FillCN.Guarantor_human_street_address1 = hn.Guarantor_Street_Address1;
                FillCN.Guarantor_human_name = hn.Guarantor_First_Name;
                FillCN.Guarantor_human_MI = hn.Guarantor_MI;
                FillCN.Guarantor_human_Lastname = hn.Guarantor_Last_Name;
                //FillCN.Guarantor_human_state = hn.Guarantor_State;
                //FillCN.Guarantor_human_zipcode = hn.Guarantor_Zip_Code;
                FillCN.MaritalStatus = hn.Marital_Status;
                FillCN.Data_Sharing_Preference = hn.Data_Sharing_Preference;
                FillCN.Birth_Indicator = hn.Birth_Indicator;
                FillCN.Birth_Order = hn.Birth_Order;
                FillCN.Guarantor_Relationship = hn.Guarantor_Relationship;
                //FillCN.Guarantor_Home_Phone_No = hn.Guarantor_Home_Phone_Number;
                //FillCN.Ethinicity_No = hn.Ethnicity_No.ToString();
                //FillCN.Race_No = hn.Race_No;
            }

        }
        private IList<Rcopia_Allergy> GetCSRCopiaAllergy(ulong ulHumanID)
        {
            IList<Rcopia_Allergy> AllergyList = new List<Rcopia_Allergy>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSAllergy.List");
                query1.SetParameter(0, ulHumanID);
                ArrayList arrayList = new ArrayList(query1.List());
                string ids = string.Empty;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Rcopia_Allergy fillAllergy = new Rcopia_Allergy();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillAllergy.Allergy_Name = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillAllergy.NDC_ID = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillAllergy.Reaction = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillAllergy.OnsetDate = Convert.ToDateTime(ccObject[3].ToString());
                    if (ccObject[4] != null)
                    fillAllergy.Status = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillAllergy.First_DataBank_Med_ID = ccObject[5].ToString();
                    if (ccObject[8] != null)
                    {
                        fillAllergy.Id = Convert.ToUInt64(ccObject[8].ToString());
                        ids = ccObject[8].ToString() + ",";
                    }
                
                    if (ccObject[9] != null)
                    fillAllergy.Rxnorm_ID = Convert.ToUInt32(ccObject[9].ToString());
                    AllergyList.Add(fillAllergy);
                }
                IQuery Query2;
                if (arrayList.Count > 0)
                {
                    ids = ids.TrimEnd(',');
                    Query2 = iMySession.GetNamedQuery("Get.CSAllergyWithID");
                    Query2.SetParameter(0, ulHumanID);
                    Query2.SetParameter(1, ids);
                }
                else
                {
                    Query2 = iMySession.GetNamedQuery("Get.CSAllergy");
                    Query2.SetParameter(0, ulHumanID);
                }
                ArrayList arraylst = new ArrayList(Query2.List());
                for (int iNumber = 0; iNumber < arraylst.Count; iNumber++)
                {

                    Rcopia_Allergy fillAllergy = new Rcopia_Allergy();
                    object[] ccObject = (object[])arraylst[iNumber];
                    //Added if condition for avoid duplicate entry in the list

                    if (ccObject[6] != null)
                    {
                        var val = from p in AllergyList where p.Id == Convert.ToUInt64(ccObject[6]) select p;
                        if (val.Count() == 0)
                        {
                            if (ccObject[0] != null)
                            fillAllergy.Allergy_Name = ccObject[0].ToString();
                            if (ccObject[2] != null)
                            fillAllergy.NDC_ID = ccObject[2].ToString();
                            if (ccObject[1] != null)
                            fillAllergy.Reaction = ccObject[1].ToString();
                            if (ccObject[4] != null)
                            fillAllergy.OnsetDate = Convert.ToDateTime(ccObject[4].ToString());
                            if (ccObject[3] != null)
                            fillAllergy.Status = ccObject[3].ToString();
                            if (ccObject[5] != null)
                            fillAllergy.First_DataBank_Med_ID = ccObject[5].ToString();
                            if (ccObject[7] != null)
                            fillAllergy.Rxnorm_ID = Convert.ToUInt32(ccObject[7]);
                            AllergyList.Add(fillAllergy);
                        }
                    }
                }
                iMySession.Close();
            }
            return AllergyList;
        }
        //public FillNotesDownLoadInfoDTO NewGetProgressNotesByXML(ulong EncID, ulong Phy_id, DataTable dtXML, ulong humanID)
        //{
        //    FillNotesDownLoadInfoDTO fillNotesDownLoadInfoDTO = new FillNotesDownLoadInfoDTO();

        //    ArrayList Quest_arrayList = new ArrayList();
        //    IList<FillProgessNotesByXML> PNList = new List<FillProgessNotesByXML>();
        //    ulong HistoryTran = 0;
        //    string SocialHistoryContent = string.Empty;

        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {

        //        IList<GeneralNotes> lstGeneralNotes = new List<GeneralNotes>();
        //        ICriteria Criteria = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", humanID));
        //        lstGeneralNotes = Criteria.List<GeneralNotes>();

        //        ISQLQuery Cri_ENC = iMySession.CreateSQLQuery("SELECT e.* FROM encounter e WHERE e.Human_ID='" + humanID + "' and e.Encounter_Provider_ID='" + Convert.ToInt32(Phy_id) + "' and e.Date_of_Service <>'0001-01-01 00:00:00'   and e.Is_Phone_Encounter <> 'Y'  Order By e.Date_of_Service desc").AddEntity("e", typeof(Encounter));
        //        IList<Encounter> Enc = Cri_ENC.List<Encounter>();



        //        foreach (DataRow dr in dtXML.Rows)
        //        {
        //            string outString = string.Empty;
        //            if (dr[1] != null)
        //            {
        //                if (dr[1].ToString() == string.Empty)
        //                {
        //                    if (dr[2] != null)
        //                    outString = dr[2].ToString() + "\n";
        //                }
        //            }

        //            else
        //            {
        //                //IQuery query1 = session.GetISession().GetNamedQuery(dr[1].ToString());

                       

        //                #region "Set Parameter"

        //                string _temp_value = string.Empty;
        //                if (dr[1] != null)
        //                    _temp_value = dr[1].ToString();
        //                ISQLQuery query1 = iMySession.CreateSQLQuery(_temp_value);
        //                if (dr[0].ToString() != "CurrentDate")
        //                {

        //                    switch (dr[0].ToString())
        //                    {

        //                        case "HeaderDetails":
        //                            {
        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, humanID);
        //                                query1.SetParameter(2, EncID);
        //                            }

        //                            break;

        //                        case "SeparateHeaderDetails":
        //                            {
        //                                if (dr[4] != null)
        //                                {
        //                                    query1.SetParameter(0, dr[4]);
        //                                    query1.SetParameter(1, dr[4]);
        //                                    query1.SetParameter(2, dr[4]);
        //                                    query1.SetParameter(3, dr[4]);
        //                                }
        //                            }

        //                            break;

        //                        case "UniqueHeaderDetails":
        //                            {
        //                                if (dr[4] != null)
        //                                {
        //                                    query1.SetParameter(0, dr[4]);
        //                                    query1.SetParameter(1, dr[4]);
        //                                    query1.SetParameter(2, dr[4]);
        //                                    query1.SetParameter(3, dr[4]);
        //                                }
        //                            }
        //                            break;
        //                        #region "History Content"
        //                        case "PMHistoryContent":
        //                            {

        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, EncID);
        //                            }

        //                            break;

        //                        case "SurgicalHistoryContent":
        //                            {

        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, EncID);
        //                            }

        //                            break;

        //                        case "HospitalizationHistoryContent":
        //                            FillProgessNotesByXML HospitalizationVerificationXml = (from p in PNList where p.Name == "HospitalizationVerification" select p).ToList<FillProgessNotesByXML>()[0];
        //                            if (HospitalizationVerificationXml.Format == "")
        //                            {
        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, EncID);
        //                            }

        //                            break;
        //                        case "FamilyHistoryContent":
        //                            {
        //                                query1.SetParameter(0, EncID);
        //                                query1.SetParameter(1, humanID);
        //                                query1.SetParameter(2, EncID);
        //                            }

        //                            break;
        //                        case "SocialHistoryContent":
        //                            {
        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, EncID);
        //                            }

        //                            break;
        //                        case "NonDrugHistoryContent":
        //                            {
        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, EncID);
        //                            }

        //                            break;
        //                        case "ImmunizationHistoryContent":
        //                            {
        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, EncID);
        //                            }

        //                            break;
        //                        case "RxHistoryContent":
        //                            query1.SetParameter(0, humanID);

        //                            break;
        //                        #endregion
        //                        case "SeparateTableLabResult":
        //                            query1.SetParameter(0, dr[3]);

        //                            break;
        //                        case "UniqueMedicationTable":
        //                            if(dr[4]!=null)
        //                            query1.SetParameter(0, dr[4]);
        //                            query1.SetParameter(1, Convert.ToDateTime(dr[5]).ToString("dd-MMM-yyyy"));

        //                            break;
        //                        case "PatientHeader":
        //                            query1.SetParameter(0, humanID);
        //                            query1.SetParameter(1, humanID);

        //                            break;

        //                        case "PatientDetails":
        //                        case "History":
        //                        case "MedicationAllergyTable":
        //                        case "MedicationTable":
        //                        case "ProblemlistContent":
        //                        case "DrugAlergyContent":
        //                            {
        //                                query1.SetParameter(0, humanID);
        //                            }
        //                            break;
        //                        case "LastTestResultsContent":
        //                            {
        //                                if (query1.ToString() != "CommonQuery")
        //                                { query1.SetParameter("Encounter_ID", EncID); }
        //                            }
        //                            break;
        //                        case "LastknowntestsproceduresContent":
        //                            {
        //                                if (query1.ToString() != "CommonQuery")
        //                                { query1.SetParameter("Encounter_ID", EncID); }
        //                            }
        //                            break;
        //                        case "ScreeningPlanContent":
        //                            {
        //                                query1.SetParameter(0, EncID);
        //                            }
        //                            break;

        //                        case "ResultsContent":
        //                            {
        //                                DateTime start_date = DateTime.MinValue;
        //                                DateTime stop_date = DateTime.MinValue;

        //                                if (Enc.Where(e => e.Id < EncID).Count() > 0)
        //                                {
        //                                    start_date = Enc.Where(e => e.Id < EncID).ToList<Encounter>()[0].Date_of_Service;
        //                                    if (Enc.Where(e => e.Id == EncID).ToList<Encounter>().Count > 0)//By Mohan
        //                                        stop_date = Enc.Where(e => e.Id == EncID).ToList<Encounter>()[0].Date_of_Service;
        //                                }
        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, Phy_id);
        //                                query1.SetParameter(2, start_date.ToString("yyyy-MM-dd H:mm:ss"));
        //                                query1.SetParameter(3, stop_date.ToString("yyyy-MM-dd H:mm:ss"));
        //                            }

        //                            break;
        //                        case "EPrescriptionContent":
        //                            {
        //                                DateTime start_date = DateTime.MinValue;
        //                                DateTime stop_date = DateTime.MinValue;

        //                                IList<Encounter> lstDate_of_service = (from arr in Enc where (arr.Id == EncID) select arr).ToList<Encounter>();
        //                                if (lstDate_of_service.Count != 0)
        //                                {
        //                                    IList<Encounter> hist = (from arr in Enc where (arr.Date_of_Service <= lstDate_of_service[0].Date_of_Service) orderby arr.Date_of_Service descending select arr).Take(2).ToList<Encounter>();
        //                                    if (hist.Count == 2)
        //                                    {
        //                                        start_date = hist[1].Date_of_Service;
        //                                        stop_date = hist[0].Date_of_Service;
        //                                    }
        //                                    else if (hist.Count == 1)
        //                                    {

        //                                        stop_date = hist[0].Date_of_Service;
        //                                    }

        //                                }


        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, stop_date.ToString("yyyy-MM-dd"));
        //                                query1.SetParameter(2, stop_date.ToString("yyyy-MM-dd"));
        //                                query1.SetParameter(3, stop_date.ToString("yyyy-MM-dd"));
        //                            }

        //                            break;
        //                        case "AttestationContent":
        //                        case "ReferalOrderContent":
        //                        case "AssessmentContent":
        //                            { query1.SetParameter(0, EncID); }

        //                            break;
        //                        case "CCContent":
        //                            {
        //                                query1.SetParameter("Human_ID", humanID);
        //                                query1.SetParameter("Encounter_ID", EncID);

        //                            }

        //                            break;
        //                        case "ROSContent":
        //                            {
        //                                query1.SetParameter(0, EncID);
        //                            }

        //                            break;
        //                        case "EncounterHeader":
        //                            {
        //                                query1.SetParameter(0, EncID);
        //                                query1.SetParameter(1, EncID);
        //                                if (dr[4].ToString().ToUpper() != "DATEREMOVE")
        //                                    query1.SetParameter(2, EncID);
        //                            }

        //                            break;
        //                        case "ReferringProviderDetails":
        //                            {
        //                                query1.SetParameter(0, EncID);
        //                                query1.SetParameter(1, EncID);
        //                                if (dr[4].ToString().ToUpper() != "DATEREMOVE")
        //                                    query1.SetParameter(2, EncID);
        //                            }

        //                            break;

        //                        case "CarePlanContent":
        //                            {



        //                                #region AD

        //                                IList<AdvanceDirective> lstAD = new List<AdvanceDirective>();
        //                                ulong ADEncoun_ID = 0;



        //                                Criteria = iMySession.CreateCriteria(typeof(AdvanceDirective)).Add(Expression.Eq("Human_ID", humanID)).AddOrder(Order.Desc("Encounter_Id"));
        //                                lstAD = Criteria.List<AdvanceDirective>();
        //                                if (lstAD.Count > 0)
        //                                {
        //                                    ADEncoun_ID = lstAD[0].Encounter_Id;
        //                                }
        //                                #endregion
        //                                query1.SetParameter(0, EncID);
        //                                query1.SetParameter(1, EncID);
        //                                query1.SetParameter(2, EncID);
        //                                query1.SetParameter(3, EncID);


        //                                //AD
        //                                query1.SetParameter(4, humanID);
        //                                query1.SetParameter(5, ADEncoun_ID);

        //                                query1.SetParameter(6, humanID);
        //                                query1.SetParameter(7, EncID);
        //                            }
        //                            break;
        //                        case "ReferralNotesContent":
        //                            query1.SetParameter(0, EncID);
        //                            query1.SetParameter(1, EncID);

        //                            break;

        //                        case "LabOrderContent":
        //                            query1.SetParameter("Encounter_ID", EncID);
        //                            //query1.SetParameter(1, EncID);

        //                            break;
        //                        case "DIProcedureContent":
        //                            query1.SetParameter(0, EncID);
        //                            query1.SetParameter(1, EncID);

        //                            break;
        //                        case "ExamContentTable":
        //                            query1.SetParameter(0, EncID);
        //                            query1.SetParameter(1, EncID);

        //                            break;

        //                        case "QuestionnaireContent":
        //                            {
        //                                ICriteria ros_result_pEnc_ = iMySession.CreateCriteria(typeof(Healthcare_Questionnaire)).Add(Expression.Eq("Encounter_ID", EncID));
        //                                IList<Healthcare_Questionnaire> lsthealth = ros_result_pEnc_.List<Healthcare_Questionnaire>();

        //                                IList<string> lstgroup = lsthealth.Select(x => x.Questionnaire_Category).Distinct().ToList<string>();

        //                                ICriteria lookupuser = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Phy_id));
        //                                IList<User> lstuser = lookupuser.List<User>();

        //                                if (lstuser!= null && lstuser.Count > 0)
        //                                {
        //                                    ICriteria lookup = iMySession.CreateCriteria(typeof(Questionnaire_Lookup)).Add(Expression.Eq("User_Name", lstuser[0].user_name));
        //                                    IList<Questionnaire_Lookup> lstlookup = lookup.List<Questionnaire_Lookup>();


        //                                    if (lstlookup != null && lstlookup.Count > 0)
        //                                    {
        //                                        var ew = lstlookup.Select(q => new { q.Questionnaire_Category, q.Is_Ros_Type }).Distinct();


        //                                        string quest = string.Empty;

        //                                        foreach (string Category in lstgroup)
        //                                        {
        //                                            if (lstlookup.Where(a => a.Questionnaire_Category == Category).ToList().Count > 0)
        //                                            {
        //                                                if (lstlookup.Where(a => a.Questionnaire_Category == Category).First().Is_Ros_Type == "Y")
        //                                                {
        //                                                    foreach (Healthcare_Questionnaire obj in lsthealth.Where(a => a.Encounter_ID == EncID && a.Questionnaire_Category == Category).ToList())
        //                                                    {
        //                                                        quest = "QuestionnaireContent";

        //                                                        if (obj.Selected_Option == "")
        //                                                            quest += "Not Specified";
        //                                                        else if (obj.Selected_Option == "Yes")
        //                                                            quest += obj.Question;
        //                                                        else
        //                                                            quest += "Negative" + obj.Question;

        //                                                        quest += "^~^" + obj.Questionnaire_Type;


        //                                                        if (obj.Notes != string.Empty)
        //                                                            quest += " $~^  " + obj.Notes.Replace("\n", "");

        //                                                        quest += "^~^" + obj.Questionnaire_Category + "-" + "Y ";

        //                                                        Quest_arrayList.Add(quest);
        //                                                    }

        //                                                }
        //                                                else
        //                                                {
        //                                                    foreach (Healthcare_Questionnaire obj in lsthealth.Where(a => a.Encounter_ID == EncID && a.Questionnaire_Category == Category && (a.Notes != "" || a.Selected_Option != "")).ToList())
        //                                                    {

        //                                                        quest = "QuestionnaireContent" + obj.Question;
        //                                                        if (obj.Selected_Option != "")
        //                                                            quest += " - " + obj.Selected_Option;

        //                                                        if (obj.Notes != string.Empty)
        //                                                            quest += " - " + obj.Notes.Replace("\n", "");

        //                                                        quest += "^~^" + obj.Questionnaire_Type + ":^~^" + obj.Questionnaire_Category + "-" + "N ";

        //                                                        Quest_arrayList.Add(quest);
        //                                                    }

        //                                                }
        //                                            }

        //                                        }
        //                                    }

        //                                }
        //                                query1.SetParameter(0, EncID);
        //                            }

        //                            break;

        //                        #region "History Notes"
        //                        case "PMHistoryNotes":
        //                            {

        //                                var notes = from n in lstGeneralNotes where n.Parent_Field == "Past Medical History" && n.Encounter_ID == HistoryTran select n;
        //                                IList<GeneralNotes> familyHistoryNotes = notes.ToList<GeneralNotes>();
        //                                if (familyHistoryNotes!=null && familyHistoryNotes.Count > 0)
        //                                    if (familyHistoryNotes[0].Notes != string.Empty)
        //                                        outString = "PMHistoryNotesNotes: " + familyHistoryNotes[0].Notes;

        //                                goto XML;
        //                            }
        //                            break;
        //                        case "SocialHistoryNotes":
        //                            {

        //                                var notes = from n in lstGeneralNotes where n.Parent_Field == "Social History" && n.Encounter_ID == HistoryTran select n;
        //                                IList<GeneralNotes> familyHistoryNotes = notes.ToList<GeneralNotes>();
        //                                if (familyHistoryNotes !=null && familyHistoryNotes.Count > 0)
        //                                    if (familyHistoryNotes[0].Notes != string.Empty)
        //                                            outString = "SocialHistoryNotesNotes: " + familyHistoryNotes[0].Notes;

        //                                goto XML;
        //                            }

        //                            break;
        //                        case "NonDrugHistoryNotes":
        //                            {
        //                                var notes = from n in lstGeneralNotes where n.Parent_Field == "Non Drug Allergy History" && n.Encounter_ID == HistoryTran select n;
        //                                IList<GeneralNotes> familyHistoryNotes = notes.ToList<GeneralNotes>();
        //                                if (familyHistoryNotes != null && familyHistoryNotes.Count > 0)
        //                                    if (familyHistoryNotes[0].Notes != string.Empty)
        //                                            outString = "NonDrugHistoryNotesNotes: " + familyHistoryNotes[0].Notes;

        //                                goto XML;
        //                            }

        //                            break;
        //                        case "FamilyHistoryNotes":
        //                            {
        //                                var notes = from n in lstGeneralNotes where n.Parent_Field == "Family History" && n.Encounter_ID == HistoryTran select n;
        //                                IList<GeneralNotes> familyHistoryNotes = notes.ToList<GeneralNotes>();
        //                                if (familyHistoryNotes != null && familyHistoryNotes.Count > 0)
        //                                    if (familyHistoryNotes[0].Notes != string.Empty)
        //                                        outString = "FamilyHistoryNotesNotes: " + familyHistoryNotes[0].Notes;

        //                                goto XML;
        //                            }

        //                            break;
        //                        case "ROSNotes":
        //                            {
        //                                var notes = from n in lstGeneralNotes where n.Parent_Field == "ROS GENERAL NOTES" && n.Encounter_ID == HistoryTran select n;
        //                                IList<GeneralNotes> familyHistoryNotes = notes.ToList<GeneralNotes>();
        //                                if (familyHistoryNotes != null && familyHistoryNotes.Count > 0)
        //                                    if (familyHistoryNotes[0].Notes != string.Empty)
        //                                        outString = "ROSNotesNotes: " + familyHistoryNotes[0].Notes;

        //                                goto XML;
        //                            }
        //                            break;

        //                        #endregion

        //                        default:
        //                            query1.SetParameter(0, EncID);
        //                            break;

        //                    }
        //                }
        //                #endregion
        //                ArrayList arrayList = new ArrayList();
        //                try
        //                {
        //                    arrayList = new ArrayList(query1.List());
        //                }
        //                catch (Exception E)
        //                {
        //                    if (E is ADOException)
        //                    {
        //                        /* Ignore The Exception */
        //                    }

        //                }

        //                #region "Post Processing Content"
        //                #region "Hospitalization History Content"

        //                if (dr[0].ToString() == "HospitalizationHistoryContent")
        //                {
        //                    FillProgessNotesByXML HospitalizationVerificationXml = (from p in PNList where p.Name == "HospitalizationVerification" select p).ToList<FillProgessNotesByXML>()[0];
        //                    if (HospitalizationVerificationXml.Format == "")
        //                    {
        //                        HospitalizationHistory objHospitalization;
        //                        IList<HospitalizationHistory> lstHospitalization = new List<HospitalizationHistory>();
        //                        for (int j = 0; j < arrayList.Count; j++)
        //                        {
        //                            objHospitalization = new HospitalizationHistory();
        //                            object[] obj = (object[])arrayList[j];
        //                            string[] Date =null;
        //                           if( obj[1] != null)
        //                            Date = ((string)obj[1].ToString()).Split('-');
        //                           if (obj[0] != null)
        //                            objHospitalization.Reason_For_Hospitalization = obj[0].ToString();
        //                           if (obj[1] != null)
        //                            objHospitalization.From_Date = obj[1].ToString();
        //                           if (obj[2] != null)
        //                            objHospitalization.To_Date = obj[2].ToString();
        //                           if (obj[3] != null)
        //                            objHospitalization.Hospitalization_Notes = obj[3].ToString();
        //                           if (obj[4] != null)
        //                            objHospitalization.Modified_Date_And_Time = Convert.ToDateTime(obj[4].ToString());

        //                            // string[] Date = ((string)obj[1].ToString()).Split('-');

        //                           if (Date != null && obj[1] != null)
        //                           {
        //                               if (Date.Length == 1 && Date[0] != string.Empty)
        //                                   objHospitalization.Modified_Date_And_Time = Convert.ToDateTime("01-01-" + (string)obj[1]);
        //                               else if (Date.Length == 2)
        //                                   objHospitalization.Modified_Date_And_Time = Convert.ToDateTime("01-" + (string)obj[1]);
        //                               else if (Date[0] != string.Empty)
        //                                   objHospitalization.Modified_Date_And_Time = Convert.ToDateTime((string)obj[1]);
        //                           }


        //                            lstHospitalization.Add(objHospitalization);
        //                        }
        //                        lstHospitalization = lstHospitalization.OrderByDescending(a => a.Modified_Date_And_Time).ToList();

        //                        for (int x = 0; x < lstHospitalization.Count; x++)
        //                        {
        //                            string Dat = string.Empty;
        //                            if (lstHospitalization[x].From_Date != string.Empty)
        //                                Dat = " during the period of " + lstHospitalization[x].From_Date;


        //                            if (lstHospitalization[x].To_Date != string.Empty)
        //                                Dat += " to " + lstHospitalization[x].To_Date;

        //                            object[] objConcatContent = new object[3];
        //                            objConcatContent[0] = (lstHospitalization[x].Reason_For_Hospitalization + Dat + lstHospitalization[x].Hospitalization_Notes).ToString();

        //                            if ((((object[])((object[])arrayList[x]))[5] != null) && (((object[])((object[])arrayList[x]))[6] != null))
        //                            {
        //                                objConcatContent[1] = ((object[])((object[])arrayList[x]))[5].ToString();
        //                                objConcatContent[2] = ((object[])((object[])arrayList[x]))[6].ToString();
        //                            }

        //                            arrayList[x] = objConcatContent;
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region "Surgical History Content"
        //                if (dr[0].ToString() == "SurgicalHistoryContent")
        //                {

        //                    SurgicalHistory objSurgical;
        //                    IList<SurgicalHistory> lstSurgical = new List<SurgicalHistory>();


        //                    for (int j = 0; j < arrayList.Count; j++)
        //                    {
        //                        objSurgical = new SurgicalHistory();
        //                        object[] obj = (object[])arrayList[j];
        //                        string[] Date = null;
        //                        if (obj[1] != null)
        //                        {
        //                            Date = ((string)obj[1].ToString()).Split('-');
        //                            objSurgical.Date_Of_Surgery = (string)obj[1];
        //                        }
        //                        if (obj[0] != null)
        //                        objSurgical.Surgery_Name = (string)obj[0];
        //                        if (obj[2] != null)
        //                        objSurgical.Description = (string)obj[2];
        //                        if (obj[4] != null && obj[4].ToString() == "0001-01-01 00:00:00")
        //                            if (obj[3] != null)
        //                            objSurgical.Modified_Date_And_Time = Convert.ToDateTime(obj[3]);

        //                        if (Date != null && obj[1] != null)
        //                        {
        //                            if (Date.Length == 1 && Date[0] != string.Empty)
        //                                objSurgical.Created_Date_And_Time = Convert.ToDateTime("01-01-" + (string)obj[1]);
        //                            else if (Date.Length == 2)
        //                                objSurgical.Created_Date_And_Time = Convert.ToDateTime("01-" + (string)obj[1]);
        //                            else if (Date[0] != string.Empty)
        //                                objSurgical.Created_Date_And_Time = Convert.ToDateTime((string)obj[1]);
        //                        }


        //                        lstSurgical.Add(objSurgical);


        //                    }
        //                    lstSurgical = lstSurgical.OrderByDescending(a => a.Modified_Date_And_Time).ToList();
        //                    lstSurgical = lstSurgical.OrderByDescending(a => a.Created_Date_And_Time).ToList();


        //                    for (int x = 0; x < lstSurgical.Count; x++)
        //                    {
        //                        string Dat = string.Empty;
        //                        if (lstSurgical[x].Date_Of_Surgery != string.Empty)
        //                            Dat = " on " + lstSurgical[x].Date_Of_Surgery;
        //                        arrayList[x] = lstSurgical[x].Surgery_Name + Dat + lstSurgical[x].Description;
        //                    }


        //                }
        //                #endregion

        //                #region "problem List Content"

        //                if (dr[0].ToString() == "ProblemlistContent")
        //                {
        //                    ProblemList objProblem;
        //                    IList<ProblemList> lstPbm = new List<ProblemList>();
        //                    for (int j = 0; j < arrayList.Count; j++)
        //                    {
        //                        objProblem = new ProblemList();
        //                        object[] obj = (object[])arrayList[j];
        //                        string[] Date = null;
        //                        if (obj[3] != null)
        //                        {
        //                            Date = ((string)obj[3].ToString()).Split('-');
        //                            objProblem.Date_Diagnosed = (string)obj[3];
        //                        }

        //                         if (Date != null && obj[3] != null)
        //                         {
        //                             if (Date.Length == 1 && Date[0] != string.Empty)
        //                                 objProblem.Modified_Date_And_Time = Convert.ToDateTime("01-01-" + (string)obj[3]);
        //                             else if (Date.Length == 2)
        //                                 objProblem.Modified_Date_And_Time = Convert.ToDateTime("01-" + (string)obj[3]);
        //                             else if (Date[0] != string.Empty)
        //                                 objProblem.Modified_Date_And_Time = Convert.ToDateTime((string)obj[3]);
        //                         }
        //                         if (obj[2] != null)
        //                        objProblem.Created_Date_And_Time = Convert.ToDateTime((string)obj[2]);
                                
        //                        if (obj[0] != null)
        //                        objProblem.ICD = (string)obj[0];
        //                        if (obj[1] != null)
        //                        objProblem.Problem_Description = (string)obj[1];
        //                        if (obj[4] != null)
        //                        objProblem.Status = (string)obj[4];
        //                        lstPbm.Add(objProblem);

        //                    }

        //                    IList<ProblemList> lstCreatedDate = new List<ProblemList>();
        //                    lstCreatedDate = lstPbm.Where(q => q.Date_Diagnosed == string.Empty).OrderByDescending(t => t.Created_Date_And_Time).ToList<ProblemList>();

        //                    lstPbm = lstPbm.Where(w => w.Date_Diagnosed != string.Empty).OrderByDescending(a => a.Modified_Date_And_Time).ToList<ProblemList>();

        //                    lstPbm = lstPbm.Concat(lstCreatedDate).ToList<ProblemList>();


        //                    for (int x = 0; x < lstPbm.Count; x++)
        //                    {
        //                        string Dat = string.Empty;
        //                        if (lstPbm[x].Date_Diagnosed != string.Empty)
        //                            Dat = " diagnosed at " + lstPbm[x].Date_Diagnosed;
        //                        arrayList[x] = lstPbm[x].ICD + lstPbm[x].Problem_Description + Dat + lstPbm[x].Status;
        //                    }

        //                }
        //                #endregion

        //                #region "Questionarie Content"
        //                else if (dr[0].ToString() == "QuestionnaireContent")
        //                {
        //                    arrayList.Clear();
        //                    arrayList = Quest_arrayList;
        //                }

        //                #endregion
        //                #endregion

        //                #region "Return Set To String"
        //                string strPrevDOS = string.Empty;
        //                for (int i = 0; i < arrayList.Count; i++)
        //                {

        //                    if (arrayList[i] != null)
        //                    {
        //                        object[] tempObject = null;

        //                        if (arrayList[i] != null && arrayList[i].ToString() != string.Empty)
        //                        {
        //                            if (arrayList[i].GetType().Name != "String")
        //                            {
        //                                tempObject = ((object[])arrayList[i]);
        //                                if (tempObject.Length > 0)
        //                                {
        //                                    if (tempObject[1] != null && tempObject[1].ToString() != strPrevDOS && strPrevDOS != string.Empty)
        //                                    {
        //                                        break;
        //                                    }
        //                                    if (dr[0].ToString().Contains("HistoryContent"))
        //                                    {
        //                                        if (tempObject[1] == null && tempObject[2] == null)
        //                                        {
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        if (arrayList[i] != null && arrayList[i].GetType().Name == "String")
        //                        {
        //                            if (arrayList[i].ToString() != string.Empty)
        //                            {
        //                                outString = outString + arrayList[i].ToString() + "\n";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (tempObject != null)
        //                            {
        //                                for (int j = 0; j < tempObject.Length; j++)
        //                                {
        //                                    if (tempObject[j] != null)
        //                                    {

        //                                        if (tempObject[j].ToString() != string.Empty)
        //                                        {
        //                                            if (dr[0].ToString().Contains("HistoryContent"))
        //                                            {
        //                                                if (tempObject[j].ToString().Contains("HistoryContent"))
        //                                                {
        //                                                    outString = outString + tempObject[j].ToString() + "\n";
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                outString = outString + tempObject[j].ToString() + "\n";
        //                                            }

        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        if (tempObject != null && tempObject.Length > 0 && tempObject[1] != null)
        //                        {
        //                            strPrevDOS = tempObject[1].ToString();
        //                        }
        //                        if (dr[0].ToString().Contains("HistoryContent"))
        //                        {
        //                            if (tempObject != null && tempObject.Length > 0 && tempObject[2] != null)
        //                            {
        //                                HistoryTran = Convert.ToUInt32(tempObject[2].ToString());
        //                                if (dr[0].ToString().Contains("SocialHistoryContent"))
        //                                {
        //                                    object[] socialHistorySections = (object[])arrayList[i];
        //                                    if (socialHistorySections != null && socialHistorySections.Length > 0)
        //                                    {
        //                                        if (socialHistorySections[0].ToString().StartsWith("SocialHistoryContentMarital Status"))
        //                                        {
        //                                            SocialHistoryContent = socialHistorySections[0].ToString().Replace("SocialHistoryContent", "").Replace("Marital Status -", "Marital Status:").Replace(" - ", "^^^");
        //                                            SocialHistoryContent += "^~^Patient Details:^~^";
        //                                        }
        //                                    }

        //                                }
        //                            }
        //                        }
        //                        if (dr[0].ToString().Contains("CarePlanContent"))
        //                        {
        //                            if (i == arrayList.Count - 1)
        //                            {
        //                                outString += SocialHistoryContent;
        //                            }
        //                        }

        //                    }
        //                }
        //                #endregion
        //            }



        //        XML: FillProgessNotesByXML objbyXML = new FillProgessNotesByXML();
        //        if (dr[0] !=null)
        //            objbyXML.Name = dr[0].ToString();
        //            objbyXML.Format = outString;
        //            if (dr[3] != null)
        //            objbyXML.Title = dr[3].ToString();
        //            if (dr[4] != null)
        //            objbyXML.Method = dr[4].ToString();
        //            PNList.Add(objbyXML);
        //        }
        //        iMySession.Close();
        //    }

        //    fillNotesDownLoadInfoDTO.FillProgessNotesByXMLList = PNList;
        //    return fillNotesDownLoadInfoDTO;
        //}
        //public FillNotesDownLoadInfoDTO ConsultationNotes(ulong EncID, ulong Phy_id, DataTable dtXML, ulong humanID)
        //{


        //    FillNotesDownLoadInfoDTO fillNotesDownLoadInfoDTO = new FillNotesDownLoadInfoDTO();

        //    ArrayList Quest_arrayList = new ArrayList();
        //    ulong HistoryTran = 0;

        //    IList<FillProgessNotesByXML> PNList = new List<FillProgessNotesByXML>();

        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {

        //        IList<GeneralNotes> lstGeneralNotes = new List<GeneralNotes>();
        //        ICriteria Criteria = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", humanID));
        //        lstGeneralNotes = Criteria.List<GeneralNotes>();


        //        foreach (DataRow dr in dtXML.Rows)
        //        {
        //            string outString = string.Empty;
        //            if (dr[1].ToString() == string.Empty)
        //            {
        //                outString = dr[2].ToString() + "\n";
        //            }
        //            else
        //            {
        //                ISQLQuery query1 = iMySession.CreateSQLQuery(dr[1].ToString());
        //                if (dr[0].ToString() != "CurrentDate")
        //                {
        //                    switch (dr[0].ToString())
        //                    {
        //                        case "PatientDetails":
        //                        case "ProblemlistContent":
        //                        case "DrugAlergyContent":
        //                            {
        //                                query1.SetParameter("Human_ID", humanID);
        //                            }
        //                            break;

        //                        case "ScreeningPlanContent":
        //                        case "ReferringProviderDetails":
        //                        case "AttestationContent":
        //                        case "ReferralNotesContent":
        //                        case "LabOrderContent":
        //                        case "ExamContentTable":
        //                        case "AssessmentContent":
        //                        case "ROSContent":
        //                            {
        //                                query1.SetParameter("Encounter_ID", EncID);

        //                            }
        //                            break;
        //                        case "LastTestResultsContent":
        //                            {
        //                                if (query1.ToString() != "CommonQuery")
        //                                { query1.SetParameter("Encounter_ID", EncID); }
        //                            }
        //                            break;
        //                        case "LastknowntestsproceduresContent":
        //                            {
        //                                if (query1.ToString() != "CommonQuery")
        //                                { query1.SetParameter("Encounter_ID", EncID); }
        //                            }
        //                            break;

        //                        case "PMHistoryContent":
        //                        case "FamilyHistoryContent":
        //                        case "SocialHistoryContent":
        //                        case "SurgicalHistoryContent":
        //                        case "ImmunizationHistoryContent":
        //                        case "NonDrugHistoryContent":
        //                        case "CCContent":
        //                        case "HospitalizationHistoryContent":
        //                            {
        //                                query1.SetParameter("Human_ID", humanID);
        //                                query1.SetParameter("Encounter_ID", EncID);
        //                            }
        //                            break;

        //                        case "PMHistoryNotes":
        //                            {
        //                                var notes = from n in lstGeneralNotes where n.Parent_Field == "Past Medical History" && n.Encounter_ID == HistoryTran select n;
        //                                IList<GeneralNotes> familyHistoryNotes = notes.ToList<GeneralNotes>();
        //                                if (familyHistoryNotes.Count > 0)
        //                                { outString = "PMHistoryNotesNotes: " + familyHistoryNotes[0].Notes; }
        //                                goto XML;
        //                            }
        //                            break;
        //                        case "SocialHistoryNotes":
        //                            {
        //                                var notes = from n in lstGeneralNotes where n.Parent_Field == "Social History" && n.Encounter_ID == HistoryTran select n;
        //                                IList<GeneralNotes> familyHistoryNotes = notes.ToList<GeneralNotes>();
        //                                if (familyHistoryNotes.Count > 0)
        //                                    outString = "SocialHistoryNotes" + familyHistoryNotes[0].Notes;

        //                                goto XML;
        //                            }
        //                            break;
        //                        case "NonDrugHistoryNotes":
        //                            {
        //                                var notes = from n in lstGeneralNotes where n.Parent_Field == "Non Drug Allergy History" && n.Encounter_ID == HistoryTran select n;
        //                                IList<GeneralNotes> familyHistoryNotes = notes.ToList<GeneralNotes>();
        //                                if (familyHistoryNotes.Count > 0)
        //                                { outString = "NonDrugHistoryNotes: " + familyHistoryNotes[0].Notes; }
        //                                goto XML;
        //                            }
        //                            break;
        //                        case "FamilyHistoryNotes":
        //                            {
        //                                var notes = from n in lstGeneralNotes where n.Parent_Field == "Family History" && n.Encounter_ID == HistoryTran select n;
        //                                IList<GeneralNotes> familyHistoryNotes = notes.ToList<GeneralNotes>();
        //                                if (familyHistoryNotes.Count > 0)
        //                                { outString = "FamilyHistoryNotesNotes: " + familyHistoryNotes[0].Notes; }
        //                                goto XML;
        //                            }
        //                            break;
        //                        case "ResultsContent":
        //                            {
        //                                DateTime start_date = DateTime.MinValue;
        //                                DateTime stop_date = DateTime.MinValue;

        //                                ISQLQuery Cri_ENC = iMySession.CreateSQLQuery("SELECT e.* FROM encounter e WHERE e.Human_ID='" + humanID + "' and e.Encounter_Provider_ID='" + Convert.ToInt32(getPhysicianID(EncID)) + "' and e.Date_of_Service <>'0001-01-01 00:00:00'   and e.Is_Phone_Encounter <> 'Y'  Order By e.Date_of_Service desc").AddEntity("e", typeof(Encounter));

        //                                IList<Encounter> Enc = Cri_ENC.List<Encounter>();

        //                                if (Cri_ENC.List<Encounter>().Where(e => e.Id < EncID).Count() > 0)
        //                                {
        //                                    start_date = Enc.Where(e => e.Id < EncID).ToList<Encounter>()[0].Date_of_Service;
        //                                    if (Enc.Where(e => e.Id == EncID).ToList<Encounter>().Count > 0)//By Mohan
        //                                        stop_date = Enc.Where(e => e.Id == EncID).ToList<Encounter>()[0].Date_of_Service;
        //                                }
        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, getPhysicianID(EncID));
        //                                query1.SetParameter(2, start_date.ToString("yyyy-MM-dd H:mm:ss"));
        //                                query1.SetParameter(3, stop_date.ToString("yyyy-MM-dd H:mm:ss"));
        //                            }
        //                            break;
        //                        case "EPrescriptionContent":
        //                            {
        //                                DateTime start_date = DateTime.MinValue;
        //                                DateTime stop_date = DateTime.MinValue;

        //                                ICriteria Cri_ENC = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", humanID));
        //                                IList<Encounter> Enc = Cri_ENC.List<Encounter>();
        //                                IList<Encounter> lstDate_of_service = (from arr in Enc where (arr.Id == EncID) select arr).ToList<Encounter>();
        //                                if (lstDate_of_service.Count != 0)
        //                                {
        //                                    IList<Encounter> hist = (from arr in Enc where (arr.Date_of_Service <= lstDate_of_service[0].Date_of_Service) orderby arr.Date_of_Service descending select arr).Take(2).ToList<Encounter>();
        //                                    if (hist.Count == 2)
        //                                    {
        //                                        start_date = hist[1].Date_of_Service;
        //                                        stop_date = hist[0].Date_of_Service;
        //                                    }
        //                                    else if (hist.Count == 1)
        //                                    {
        //                                        stop_date = hist[0].Date_of_Service;
        //                                    }
        //                                }
        //                                query1.SetParameter(0, humanID);
        //                                query1.SetParameter(1, stop_date.ToString("yyyy-MM-dd"));
        //                                query1.SetParameter(2, stop_date.ToString("yyyy-MM-dd"));
        //                                query1.SetParameter(3, stop_date.ToString("yyyy-MM-dd"));
        //                            }
        //                            break;
        //                        case "ConsultationContent":
        //                            {
        //                                query1.SetParameter("Human_ID", humanID);
        //                                query1.SetParameter("Encounter_ID", EncID);
        //                            }
        //                            break;
        //                        default:
        //                            {
        //                                try
        //                                {
        //                                    query1.SetParameter("Encounter_ID", EncID);
        //                                }
        //                                catch (ADOException)
        //                                {
        //                                    // Skip if the Parameter is not available
        //                                }
        //                            }
        //                            break;
        //                    }
        //                }
        //                ArrayList arrayList = new ArrayList();
        //                try
        //                {
        //                    arrayList = new ArrayList(query1.List());
        //                }
        //                catch (Exception E)
        //                {
        //                    if (E is ADOException)
        //                    {
        //                        /* Ignore The Exception */
        //                    }
        //                }

        //                #region "Post Processing"
        //                if (dr[0].ToString() == "HospitalizationHistoryContent")
        //                {
        //                    HospitalizationHistory objHospitalization;
        //                    IList<HospitalizationHistory> lstHospitalization = new List<HospitalizationHistory>();
        //                    for (int j = 0; j < arrayList.Count; j++)
        //                    {

        //                        object[] obj = (object[])arrayList[j];

        //                        objHospitalization = new HospitalizationHistory();

        //                        string[] Date = ((string)obj[1].ToString()).Split('-');


        //                        objHospitalization.Reason_For_Hospitalization = obj[0].ToString();
        //                        objHospitalization.From_Date = obj[1].ToString();
        //                        objHospitalization.To_Date = obj[2].ToString();
        //                        objHospitalization.Hospitalization_Notes = obj[3].ToString();
        //                        objHospitalization.Modified_Date_And_Time = Convert.ToDateTime(obj[4].ToString());

        //                        if (Date.Length == 1 && Date[0] != string.Empty)
        //                            objHospitalization.Modified_Date_And_Time = Convert.ToDateTime("01-01-" + (string)obj[1]);
        //                        else if (Date.Length == 2)
        //                            objHospitalization.Modified_Date_And_Time = Convert.ToDateTime("01-" + (string)obj[1]);
        //                        else if (Date[0] != string.Empty)
        //                            objHospitalization.Modified_Date_And_Time = Convert.ToDateTime((string)obj[1]);


        //                        lstHospitalization.Add(objHospitalization);
        //                    }
        //                    lstHospitalization = lstHospitalization.OrderByDescending(a => a.Modified_Date_And_Time).ToList();

        //                    for (int x = 0; x < lstHospitalization.Count; x++)
        //                    {
        //                        string Dat = string.Empty;
        //                        if (lstHospitalization[x].From_Date != string.Empty)
        //                            Dat = " during the period of " + lstHospitalization[x].From_Date;


        //                        if (lstHospitalization[x].To_Date != string.Empty)
        //                            Dat += " to " + lstHospitalization[x].To_Date;

        //                        arrayList[x] = lstHospitalization[x].Reason_For_Hospitalization + Dat + lstHospitalization[x].Hospitalization_Notes;


        //                        object[] objConcatContent = new object[3];
        //                        objConcatContent[0] = (lstHospitalization[x].Reason_For_Hospitalization + Dat + lstHospitalization[x].Hospitalization_Notes).ToString();
        //                        try
        //                        {
        //                            if ((((object[])((object[])arrayList[x]))[5] != null) && (((object[])((object[])arrayList[x]))[6] != null))
        //                            {
        //                                objConcatContent[1] = ((object[])((object[])arrayList[x]))[5].ToString();
        //                                objConcatContent[2] = ((object[])((object[])arrayList[x]))[6].ToString();
        //                            }
        //                        }
        //                        catch
        //                        { /* Do nothing Invalid Cast Exception  */ }

        //                        arrayList[x] = objConcatContent;


        //                    }


        //                }
        //                if (dr[0].ToString() == "SurgicalHistoryContent")
        //                {

        //                    SurgicalHistory objSurgical;
        //                    IList<SurgicalHistory> lstSurgical = new List<SurgicalHistory>();


        //                    for (int j = 0; j < arrayList.Count; j++)
        //                    {

        //                        object[] obj = (object[])arrayList[j];

        //                        objSurgical = new SurgicalHistory();

        //                        string[] Date = ((string)obj[1].ToString()).Split('-');


        //                        objSurgical.Surgery_Name = (string)obj[0];
        //                        objSurgical.Date_Of_Surgery = (string)obj[1];
        //                        objSurgical.Description = (string)obj[2];
        //                        if (obj[4].ToString() == "0001-01-01 00:00:00")
        //                            objSurgical.Modified_Date_And_Time = Convert.ToDateTime(obj[3]);




        //                        if (Date.Length == 1 && Date[0] != string.Empty)
        //                            objSurgical.Created_Date_And_Time = Convert.ToDateTime("01-01-" + (string)obj[1]);
        //                        else if (Date.Length == 2)
        //                            objSurgical.Created_Date_And_Time = Convert.ToDateTime("01-" + (string)obj[1]);
        //                        else if (Date[0] != string.Empty)
        //                            objSurgical.Created_Date_And_Time = Convert.ToDateTime((string)obj[1]);


        //                        lstSurgical.Add(objSurgical);


        //                    }
        //                    lstSurgical = lstSurgical.OrderByDescending(a => a.Modified_Date_And_Time).ToList();
        //                    lstSurgical = lstSurgical.OrderByDescending(a => a.Created_Date_And_Time).ToList();


        //                    for (int x = 0; x < lstSurgical.Count; x++)
        //                    {
        //                        string Dat = string.Empty;
        //                        if (lstSurgical[x].Date_Of_Surgery != string.Empty)
        //                            Dat = " on " + lstSurgical[x].Date_Of_Surgery;
        //                        arrayList[x] = lstSurgical[x].Surgery_Name + Dat + lstSurgical[x].Description;
        //                    }


        //                }



        //                if (dr[0].ToString() == "ProblemlistContent")
        //                {
        //                    ProblemList objProblem;
        //                    IList<ProblemList> lstPbm = new List<ProblemList>();
        //                    for (int j = 0; j < arrayList.Count; j++)
        //                    {

        //                        object[] obj = (object[])arrayList[j];

        //                        objProblem = new ProblemList();

        //                        string[] Date = ((string)obj[3].ToString()).Split('-');


        //                        if (Date.Length == 1 && Date[0] != string.Empty)
        //                            objProblem.Modified_Date_And_Time = Convert.ToDateTime("01-01-" + (string)obj[3]);
        //                        else if (Date.Length == 2)
        //                            objProblem.Modified_Date_And_Time = Convert.ToDateTime("01-" + (string)obj[3]);
        //                        else if (Date[0] != string.Empty)
        //                            objProblem.Modified_Date_And_Time = Convert.ToDateTime((string)obj[3]);

        //                        objProblem.Created_Date_And_Time = Convert.ToDateTime((string)obj[2]);
        //                        objProblem.Date_Diagnosed = (string)obj[3];
        //                        objProblem.ICD = (string)obj[0];
        //                        objProblem.Problem_Description = (string)obj[1];
        //                        objProblem.Status = (string)obj[4];
        //                        lstPbm.Add(objProblem);

        //                    }

        //                    IList<ProblemList> lstCreatedDate = new List<ProblemList>();
        //                    lstCreatedDate = lstPbm.Where(q => q.Date_Diagnosed == string.Empty).OrderByDescending(t => t.Created_Date_And_Time).ToList<ProblemList>();

        //                    lstPbm = lstPbm.Where(w => w.Date_Diagnosed != string.Empty).OrderByDescending(a => a.Modified_Date_And_Time).ToList<ProblemList>();

        //                    lstPbm = lstPbm.Concat(lstCreatedDate).ToList<ProblemList>();


        //                    for (int x = 0; x < lstPbm.Count; x++)
        //                    {
        //                        string Dat = string.Empty;
        //                        if (lstPbm[x].Date_Diagnosed != string.Empty)
        //                            Dat = " diagnosed at " + lstPbm[x].Date_Diagnosed;
        //                        arrayList[x] = lstPbm[x].ICD + lstPbm[x].Problem_Description + Dat + lstPbm[x].Status;
        //                    }

        //                }
        //                else if (dr[0].ToString() == "QuestionnaireContent")
        //                {
        //                    arrayList.Clear();
        //                    arrayList = Quest_arrayList;
        //                }

        //                #endregion

        //                #region "Return Set Manipulation"
        //                for (int i = 0; i < arrayList.Count; i++)
        //                {
        //                    if (arrayList[i] != null)
        //                    {
        //                        object[] tempObject = null;
        //                        string strPrevDOS = string.Empty;

        //                        if (arrayList[i] != null && arrayList[i].ToString() != string.Empty)
        //                        {
        //                            if (arrayList[i].GetType().Name != "String")
        //                            {
        //                                tempObject = ((object[])arrayList[i]);
        //                                if (tempObject.Length > 0)
        //                                {
        //                                    if (tempObject[2] != null && tempObject[2].ToString() != strPrevDOS && strPrevDOS != string.Empty)
        //                                    {
        //                                        break;
        //                                    }
        //                                    //if (tempObject[5] != null && tempObject[5].ToString() != strPrevDOS && strPrevDOS != string.Empty)
        //                                    //{
        //                                    //    break;
        //                                    //}
        //                                }
        //                            }
        //                        }

        //                        if (arrayList[i].GetType().Name == "String")
        //                        {
        //                            if (arrayList[i].ToString() != string.Empty)
        //                            {
        //                                outString = outString + arrayList[i].ToString() + "\n";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            tempObject = (object[])arrayList[i];
        //                            for (int j = 0; j < tempObject.Length; j++)
        //                            {
        //                                if (tempObject[j] != null)
        //                                {
        //                                    if (tempObject[j].ToString() != string.Empty)
        //                                    {
        //                                        outString = outString + tempObject[j].ToString() + "\n";
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        if (tempObject != null && tempObject.Length > 0 && tempObject[2] != null)
        //                        {
        //                            strPrevDOS = tempObject[2].ToString();
        //                            if (dr[0].ToString().Contains("HospitalizationHistoryContent"))
        //                            {
        //                                strPrevDOS = tempObject[5].ToString();
        //                            }
        //                        }

        //                        if (dr[0].ToString().Contains("HistoryContent"))
        //                        {
        //                            //if (tempObject != null && tempObject.Length > 0 && tempObject[2] != null)
        //                            //{
        //                            //    HistoryTran = Convert.ToUInt32(tempObject[2].ToString());
        //                            //    //if (dr[0].ToString().Contains("SocialHistoryContent"))
        //                            //    //{
        //                            //    //    object[] socialHistorySections = (object[])arrayList[i];
        //                            //    //    if (socialHistorySections != null && socialHistorySections.Length > 0)
        //                            //    //    {
        //                            //    //        if (socialHistorySections[0].ToString().StartsWith("SocialHistoryContentMarital Status"))
        //                            //    //        {
        //                            //    //            SocialHistoryContent = socialHistorySections[0].ToString().Replace("SocialHistoryContent", "").Replace("Marital Status -", "Marital Status:").Replace(" - ", "^^^");
        //                            //    //            SocialHistoryContent += "^~^Patient Details:^~^";
        //                            //    //        }
        //                            //    //    }
        //                            //    //}
        //                            //}
        //                        }
        //                    }

        //                }
        //                #endregion
        //            }
        //        XML: FillProgessNotesByXML objbyXML = new FillProgessNotesByXML();
        //            objbyXML.Name = dr[0].ToString();
        //            objbyXML.Format = outString;
        //            objbyXML.Title = dr[3].ToString();
        //            objbyXML.Method = dr[4].ToString();
        //            PNList.Add(objbyXML);
        //        }


        //        fillNotesDownLoadInfoDTO.FillProgessNotesByXMLList = PNList;

        //        iMySession.Close();
        //    }
        //    return fillNotesDownLoadInfoDTO;
        //}
        //public IList<FillProgessNotesByXML> GetProgressNotesByXML(ulong EncID, DataTable dtXML)
        //{
        //    IList<FillProgessNotesByXML> PNList = new List<FillProgessNotesByXML>();
        //    foreach (DataRow dr in dtXML.Rows)
        //    {
        //        string outString = string.Empty;
        //        if (dr[1].ToString() == string.Empty)
        //        {
        //            outString = dr[2].ToString() + "\n";
        //        }
        //        else
        //        {
        //            //IQuery query1 = session.GetISession().GetNamedQuery(dr[1].ToString());
        //            ISQLQuery query1 = session.GetISession().CreateSQLQuery(dr[1].ToString());
        //            if (dr[0].ToString() != "CurrentDate")
        //            {
        //                if (dr[0].ToString() == "HeaderDetails")
        //                {
        //                    query1.SetParameter(0, EncID);
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                    query1.SetParameter(2, getHumanID(EncID));
        //                    query1.SetParameter(3, getHumanID(EncID));
        //                    query1.SetParameter(4, EncID);
        //                    query1.SetParameter(5, EncID);
        //                    query1.SetParameter(6, getHumanID(EncID));
        //                    query1.SetParameter(7, EncID);

        //                }
        //                else if (dr[0].ToString() == "SeparateHeaderDetails" || dr[0].ToString() == "UniqueHeaderDetails")
        //                {
        //                    query1.SetParameter(0, dr[4]);
        //                    query1.SetParameter(1, dr[4]);
        //                    query1.SetParameter(2, dr[4]);
        //                    query1.SetParameter(3, dr[4]);
        //                }
        //                else if (dr[0].ToString() == "SeparateTableLabResult")
        //                {
        //                    query1.SetParameter(0, dr[3]);
        //                }
        //                else if (dr[0].ToString() == "UniqueMedicationTable")
        //                {
        //                    query1.SetParameter(0, dr[4]);
        //                    query1.SetParameter(1, Convert.ToDateTime(dr[5]).ToString("dd-MMM-yyyy"));
        //                }

        //                else if (dr[0].ToString() == "FamilyHistoryContent")
        //                {

        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                    query1.SetParameter(2, getHumanID(EncID));
        //                }
        //                else if (dr[0].ToString() == "PatientHeader" || dr[0].ToString() == "PMHistoryContent" || dr[0].ToString() == "SocialHistoryContent" || dr[0].ToString() == "NonDrugHistoryContent")
        //                {
        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                }
        //                else if (dr[0].ToString() == "HospitalizationHistoryContent")
        //                {
        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, EncID);
        //                }
        //                else if (dr[0].ToString() == "PatientDetails" || dr[0].ToString().Contains("History") || dr[0].ToString() == "MedicationAllergyTable" || dr[0].ToString() == "MedicationTable" || dr[0].ToString() == "ProblemlistContent" || dr[0].ToString() == "DrugAlergyContent")
        //                {
        //                    query1.SetParameter(0, getHumanID(EncID));
        //                }

        //                else if (dr[0].ToString() == "ScreeningPlanContent")
        //                {

        //                    query1.SetParameter(0, EncID);
        //                    query1.SetParameter(1, EncID);
        //                    query1.SetParameter(2, EncID);
        //                    query1.SetParameter(3, EncID);
        //                    query1.SetParameter(4, EncID);

        //                }

        //                else if (dr[0].ToString() == "ResultsContent")
        //                {
        //                    DateTime start_date = DateTime.MinValue;
        //                    DateTime stop_date = DateTime.MinValue;


        //                    ISQLQuery Cri_ENC = session.GetISession().CreateSQLQuery("SELECT e.* FROM encounter e WHERE e.Human_ID='" + getHumanID(EncID) + "' and e.Encounter_Provider_ID='" + Convert.ToInt32(getPhysicianID(EncID)) + "' and e.Date_of_Service <>'0001-01-01 00:00:00'   and e.Is_Phone_Encounter <> 'Y'  Order By e.Date_of_Service desc").AddEntity("e", typeof(Encounter));
        //                    //ICriteria Cri_ENC = session.GetISession().CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", getHumanID(EncID))).Add(Expression.Eq("Encounter_Provider_ID",Convert.ToInt32(getPhysicianID(EncID)))).Add(Expression.NotEqProperty());
        //                    IList<Encounter> Enc = Cri_ENC.List<Encounter>();

        //                    if (Cri_ENC.List<Encounter>().Where(e => e.Id < EncID).Count() > 0)
        //                    {
        //                        start_date = Enc.Where(e => e.Id < EncID).ToList<Encounter>()[0].Date_of_Service;
        //                        if (Enc.Where(e => e.Id == EncID).ToList<Encounter>().Count != 0)
        //                            stop_date = Enc.Where(e => e.Id == EncID).ToList<Encounter>()[0].Date_of_Service;
        //                    }
        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, getPhysicianID(EncID));
        //                    query1.SetParameter(2, start_date.ToString("yyyy-MM-dd H:mm:ss"));
        //                    query1.SetParameter(3, stop_date.ToString("yyyy-MM-dd H:mm:ss"));


        //                }
        //                else if (dr[0].ToString() == "EPrescriptionContent")
        //                {

        //                    DateTime start_date = DateTime.MinValue;
        //                    DateTime stop_date = DateTime.MinValue;

        //                    ICriteria Cri_ENC = session.GetISession().CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", getHumanID(EncID)));
        //                    IList<Encounter> Enc = Cri_ENC.List<Encounter>();



        //                    IList<Encounter> lstDate_of_service = (from arr in Enc where (arr.Id == EncID) select arr).ToList<Encounter>();


        //                    if (lstDate_of_service.Count != 0)
        //                    {
        //                        //IList<Encounter> hist = (from arr in Enc where (arr.Id <= EncID) orderby arr.Date_of_Service descending select arr).Take(2).ToList<Encounter>();
        //                        IList<Encounter> hist = (from arr in Enc where (arr.Date_of_Service <= lstDate_of_service[0].Date_of_Service) orderby arr.Date_of_Service descending select arr).Take(2).ToList<Encounter>();
        //                        if (hist.Count == 2)
        //                        {
        //                            start_date = hist[1].Date_of_Service;
        //                            stop_date = hist[0].Date_of_Service;
        //                        }
        //                        else if (hist.Count == 1)
        //                        {

        //                            stop_date = hist[0].Date_of_Service;
        //                        }



        //                    }


        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, stop_date.ToString("yyyy-MM-dd"));
        //                    query1.SetParameter(2, stop_date.ToString("yyyy-MM-dd"));
        //                    query1.SetParameter(3, stop_date.ToString("yyyy-MM-dd"));


        //                }






        //                else if (dr[0].ToString() == "EncounterHeader" || dr[0].ToString() == "ReferringProviderDetails" || dr[0].ToString() == "AttestationContent")
        //                {
        //                    query1.SetParameter(0, EncID);
        //                    query1.SetParameter(1, EncID);
        //                    query1.SetParameter(2, EncID);
        //                }
        //                else if (dr[0].ToString() == "CCContent")
        //                {
        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                    query1.SetParameter(2, getHumanID(EncID));
        //                    query1.SetParameter(3, getHumanID(EncID));
        //                    query1.SetParameter(4, EncID);
        //                }
        //                else if (dr[0].ToString() == "ConsultationContent")
        //                {
        //                    query1.SetParameter(0, EncID);
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                    query1.SetParameter(2, getHumanID(EncID));
        //                    query1.SetParameter(3, getHumanID(EncID));
        //                    query1.SetParameter(4, getHumanID(EncID));
        //                    query1.SetParameter(5, EncID);
        //                }
        //                else if (dr[0].ToString() == "DVConsultationContent")
        //                {
        //                    query1.SetParameter(0, EncID);
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                    query1.SetParameter(2, getHumanID(EncID));
        //                    query1.SetParameter(3, getHumanID(EncID));
        //                    query1.SetParameter(4, EncID);
        //                }
        //                else if (dr[0].ToString() == "JRConsultationContent" || dr[0].ToString() == "RRConsultationContent" || dr[0].ToString() == "EKConsultationContent")
        //                {
        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                    query1.SetParameter(2, getHumanID(EncID));
        //                    query1.SetParameter(3, EncID);
        //                }
        //                else if (dr[0].ToString() == "CarePlanContent")
        //                {




        //                    query1.SetParameter(0, EncID);
        //                    query1.SetParameter(1, EncID);
        //                    query1.SetParameter(2, EncID);
        //                    query1.SetParameter(3, EncID);
        //                    query1.SetParameter(4, getHumanID(EncID));
        //                    query1.SetParameter(5, getHumanID(EncID));
        //                    query1.SetParameter(6, getHumanID(EncID));
        //                    query1.SetParameter(7, getHumanID(EncID));
        //                    query1.SetParameter(8, getHumanID(EncID));
        //                    query1.SetParameter(9, getHumanID(EncID));
        //                    //query1.SetParameter(10, getHumanID(EncID));
        //                    //query1.SetParameter(11, getHumanID(EncID));
        //                }
        //                else if (dr[0].ToString() == "JTConsultationContent" || dr[0].ToString() == "HAConsultationContent")
        //                {
        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                    query1.SetParameter(2, getHumanID(EncID));
        //                }
        //                else if (dr[0].ToString() == "NPConsultationContent")
        //                {
        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                    query1.SetParameter(2, EncID);
        //                    query1.SetParameter(3, EncID);
        //                }

        //                else if (dr[0].ToString() == "JPRConsultationContent" || dr[0].ToString() == "RPConsultationContent")
        //                {
        //                    query1.SetParameter(0, getHumanID(EncID));
        //                    query1.SetParameter(1, getHumanID(EncID));
        //                    query1.SetParameter(2, EncID);
        //                }


        //                else if (dr[0].ToString() == "ReferralNotesContent" || dr[0].ToString() == "AttestationContent" || dr[0].ToString() == "LabOrderContent" || dr[0].ToString() == "DIProcedureContent" || dr[0].ToString() == "ReferalOrderContent" || dr[0].ToString() == "ExamContentTable" || dr[0].ToString() == "AssessmentContent" || dr[0].ToString() == "ROSContent")
        //                {
        //                    query1.SetParameter(0, EncID);
        //                    query1.SetParameter(1, EncID);
        //                }


        //                else
        //                {
        //                    query1.SetParameter(0, EncID);
        //                }
        //            }
        //            ArrayList arrayList = new ArrayList(query1.List());


        //            if (dr[0].ToString() == "ProblemlistContent")
        //            {
        //                ProblemList objProblem;
        //                IList<ProblemList> lstPbm = new List<ProblemList>();
        //                for (int j = 0; j < arrayList.Count; j++)
        //                {

        //                    object[] obj = (object[])arrayList[j];

        //                    objProblem = new ProblemList();

        //                    string[] Date = ((string)obj[3].ToString()).Split('-');


        //                    if (Date.Length == 1 && Date[0] != string.Empty)
        //                        objProblem.Modified_Date_And_Time = Convert.ToDateTime("01-01-" + (string)obj[3]);
        //                    else if (Date.Length == 2)
        //                        objProblem.Modified_Date_And_Time = Convert.ToDateTime("01-" + (string)obj[3]);
        //                    else if (Date[0] != string.Empty)
        //                        objProblem.Modified_Date_And_Time = Convert.ToDateTime((string)obj[3]);

        //                    objProblem.Created_Date_And_Time = Convert.ToDateTime((string)obj[2]);
        //                    objProblem.Date_Diagnosed = (string)obj[3];
        //                    objProblem.ICD_Code = (string)obj[0];
        //                    objProblem.Problem_Description = (string)obj[1];
        //                    objProblem.Status = (string)obj[4];
        //                    lstPbm.Add(objProblem);

        //                }

        //                IList<ProblemList> lstCreatedDate = new List<ProblemList>();
        //                lstCreatedDate = lstPbm.Where(q => q.Date_Diagnosed == string.Empty).OrderByDescending(t => t.Created_Date_And_Time).ToList<ProblemList>();

        //                lstPbm = lstPbm.Where(w => w.Date_Diagnosed != string.Empty).OrderByDescending(a => a.Modified_Date_And_Time).ToList<ProblemList>();

        //                lstPbm = lstPbm.Concat(lstCreatedDate).ToList<ProblemList>();


        //                for (int x = 0; x < lstPbm.Count; x++)
        //                {
        //                    string Dat = string.Empty;
        //                    if (lstPbm[x].Date_Diagnosed != string.Empty)
        //                        Dat = " diagnosed at " + lstPbm[x].Date_Diagnosed;
        //                    arrayList[x] = lstPbm[x].ICD_Code + lstPbm[x].Problem_Description + Dat + lstPbm[x].Status;
        //                }

        //            }




        //            for (int i = 0; i < arrayList.Count; i++)
        //            {
        //                if (arrayList[i] != null)
        //                {
        //                    if (arrayList[i].GetType().Name == "String")
        //                    {
        //                        if (arrayList[i].ToString() != string.Empty)
        //                        {
        //                            outString = outString + arrayList[i].ToString() + "\n";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        object[] tempObject = (object[])arrayList[i];
        //                        for (int j = 0; j < tempObject.Length; j++)
        //                        {
        //                            if (tempObject[j] != null)
        //                            {
        //                                if (tempObject[j].ToString() != string.Empty)
        //                                {
        //                                    outString = outString + tempObject[j].ToString() + "\n";
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        FillProgessNotesByXML objbyXML = new FillProgessNotesByXML();
        //        objbyXML.Name = dr[0].ToString();
        //        objbyXML.Format = outString;
        //        objbyXML.Title = dr[3].ToString();
        //        objbyXML.Method = dr[4].ToString();
        //        PNList.Add(objbyXML);
        //    }

        //    return PNList;
        //}
        public IList<ChiefComplaints> GetCSChiefComplaints(ulong ulEncounterID)
        {
            IList<ChiefComplaints> ccList = new List<ChiefComplaints>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSChiefComplaints.List");
                query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(1, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    ChiefComplaints fillCC = new ChiefComplaints();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillCC.HPI_Element = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillCC.HPI_Value = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillCC.Modified_Date_And_Time = Convert.ToDateTime(ccObject[2].ToString());
                    ccList.Add(fillCC);
                }
                iMySession.Close();
            }
            return ccList;
        }

        public IList<ChiefComplaints> GetCSChiefComplaintsHPI(ulong ulEncounterID)
        {
            IList<ChiefComplaints> ccList = new List<ChiefComplaints>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSChiefComplaintsHPI.List");
                query1.SetParameter(0, ulEncounterID);
               
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    ChiefComplaints fillCC = new ChiefComplaints();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillCC.HPI_Element = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillCC.HPI_Value = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillCC.Modified_Date_And_Time = Convert.ToDateTime(ccObject[2].ToString());
                    ccList.Add(fillCC);
                }
                iMySession.Close();
            }
            return ccList;
        }

        
        public IList<PatientResults> GetCSVitals(ulong ulEncounterID)
        {
            IList<PatientResults> vitalsList = new List<PatientResults>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSVitals.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PatientResults fillVitals = new PatientResults();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillVitals.Loinc_Observation = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillVitals.Value = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillVitals.Captured_date_and_time = Convert.ToDateTime(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    fillVitals.Notes = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillVitals.Loinc_Identifier = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillVitals.Results_Type = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    fillVitals.Units = ccObject[6].ToString();
                    vitalsList.Add(fillVitals);
                }
                iMySession.Close();
            }
            return vitalsList;
        }
        public IList<PatientResults> GetCSVitalsForCareplan(ulong ulEncounterID)
        {
            IList<PatientResults> vitalsList = new List<PatientResults>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSVitalsForCareplan.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PatientResults fillVitals = new PatientResults();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillVitals.Loinc_Observation = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillVitals.Value = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillVitals.Captured_date_and_time = Convert.ToDateTime(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    fillVitals.Notes = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillVitals.Loinc_Identifier = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillVitals.Results_Type = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    fillVitals.Units = ccObject[6].ToString();
                    vitalsList.Add(fillVitals);
                }
                iMySession.Close();
            }
            return vitalsList;
        }

        public IList<ROS> GetCSROS(ulong ulEncounterID)
        {
            IList<ROS> rosList = new List<ROS>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSROS.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    ROS fillROS = new ROS();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillROS.System_Name = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillROS.Symptom_Name = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillROS.Created_Date_And_Time = Convert.ToDateTime(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    fillROS.Status = ccObject[3].ToString();
                    rosList.Add(fillROS);
                }
                iMySession.Close();
            }
            return rosList;
        }
        public IList<GeneralNotes> GetCSROSNotes(ulong ulEncounterID)
        {
            IList<GeneralNotes> rosList = new List<GeneralNotes>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSROSNotes.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    GeneralNotes fillROS = new GeneralNotes();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillROS.Parent_Field = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillROS.Name_Of_The_Field = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillROS.Notes = ccObject[2].ToString();
                    rosList.Add(fillROS);
                }
                iMySession.Close();
            }
            return rosList;
        }
        public IList<Examination> GetCSExamination(ulong ulEncounterID)
        {
            IList<Examination> examinationList = new List<Examination>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSExamination.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Examination fillExamination = new Examination();
                    if (arrayList[i].ToString() != string.Empty)
                    {
                        object[] ccObject = (object[])arrayList[i];
                        if (ccObject[0] != null)
                        fillExamination.System_Name = ccObject[0].ToString();
                        if (ccObject[1] != null)
                        fillExamination.Condition_Name = ccObject[1].ToString();
                        if (ccObject[2] != null)
                        fillExamination.Status = ccObject[2].ToString();
                        if (ccObject[3] != null)
                        fillExamination.Examination_Notes = ccObject[3].ToString();
                        if (ccObject[4] != null)
                        fillExamination.Short_Description = ccObject[4].ToString();
                        //fillExamination.Examination_Details = ccObject[0].ToString();
                    }
                    examinationList.Add(fillExamination);
                }
                iMySession.Close();
            }
            return examinationList;
        }
        public IList<PastMedicalHistory> GetCSPastMedicalHistory(ulong ulEncounterID)
        {
            IList<PastMedicalHistory> medicalHistoryList = new List<PastMedicalHistory>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSPastMedicalHistory.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PastMedicalHistory fillmedicalHistory = new PastMedicalHistory();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillmedicalHistory.Past_Medical_Info = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillmedicalHistory.From_Date = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillmedicalHistory.To_Date = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillmedicalHistory.AHA_Question_ICD = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillmedicalHistory.Notes = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillmedicalHistory.Is_present = ccObject[5].ToString();
                    medicalHistoryList.Add(fillmedicalHistory);
                }
                iMySession.Close();
            }
            return medicalHistoryList;
        }
        public IList<GeneralNotes> GetCSPastMedicalHistoryNotes(ulong ulEncounterID)
        {
            IList<GeneralNotes> rosList = new List<GeneralNotes>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSPastMedNotes.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    GeneralNotes fillROS = new GeneralNotes();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillROS.Parent_Field = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillROS.Name_Of_The_Field = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillROS.Notes = ccObject[2].ToString();
                    rosList.Add(fillROS);
                }
                iMySession.Close();
            }
            return rosList;
        }
        public IList<Rcopia_Medication> GetCSMedication(ulong ulHumanID)
        {
            IList<Rcopia_Medication> medicationList = new List<Rcopia_Medication>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSMedication.List");
                query1.SetParameter(0, ulHumanID);
                ArrayList arrayList = new ArrayList(query1.List());
                string Ids = string.Empty;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Rcopia_Medication fillmedication = new Rcopia_Medication();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillmedication.NDC_ID = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillmedication.Generic_Name = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillmedication.Strength = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillmedication.Dose = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillmedication.Route = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillmedication.Dose_Other = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    fillmedication.Patient_Notes = ccObject[6].ToString();
                    if (ccObject[8] != null)
                    fillmedication.Start_Date = Convert.ToDateTime(ccObject[8].ToString());
                    if (ccObject[9] != null)
                    fillmedication.Brand_Name = ccObject[9].ToString();
                    if (ccObject[10] != null)
                    fillmedication.Stop_Date = Convert.ToDateTime(ccObject[10].ToString());
                    if (ccObject[11] != null)
                    fillmedication.Quantity = ccObject[11].ToString();
                    if (ccObject[12] != null)
                    fillmedication.Quantity_Unit = ccObject[12].ToString();
                    if (ccObject[13] != null)
                    fillmedication.First_DataBank_Med_ID = ccObject[13].ToString();
                    if (ccObject[14] != null)
                    fillmedication.ICD_Code = ccObject[14].ToString();
                    if (ccObject[15] != null)
                    fillmedication.ICD_Code_Description = ccObject[15].ToString();
                    if (ccObject[16] != null)
                    fillmedication.Refills = ccObject[16].ToString();
                    if (ccObject[17] != null)
                    fillmedication.Action = ccObject[17].ToString();
                    if (ccObject[18] != null)
                    fillmedication.Dose_Unit = ccObject[18].ToString();
                    if (ccObject[19] != null)
                    fillmedication.Dose_Timing = ccObject[19].ToString();
                    if (ccObject[20] != null)
                    fillmedication.Internal_Property_Code = ccObject[20].ToString();
                    if (ccObject[21] != null)
                    {
                        fillmedication.Id = Convert.ToUInt32(ccObject[21].ToString());
                        Ids = ccObject[21].ToString() + ",";
                        fillmedication.Rxnorm_ID = Convert.ToUInt32(ccObject[21].ToString());
                    }
                    if (ccObject[22] != null)
                    fillmedication.Rxnorm_ID_Type = ccObject[22].ToString();
                    medicationList.Add(fillmedication);
                }
                //IQuery Query2;
                //Ids = Ids.TrimEnd(',');
                //if (arrayList.Count == 0)
                //{
                //    Query2 = session.GetISession().GetNamedQuery("Fill.CSMedication");
                //    Query2.SetParameter(0, getHumanID(ulEncounterID));
                //}
                //else
                //{
                //    Query2 = session.GetISession().GetNamedQuery("Fill.CSMedicationWithID");
                //    Query2.SetParameter(0, getHumanID(ulEncounterID));
                //    Query2.SetParameter(1, Ids);
                //}
                //ArrayList arrayLst = new ArrayList(Query2.List());
                //for (int i = 0; i < arrayLst.Count; i++)
                //{
                //    Rcopia_Medication fillmedication = new Rcopia_Medication();
                //    //object[] ccObject = (object[])arrayList[i];// Commended By manimaran for bug id:29013
                //    object[] ccObject = (object[])arrayLst[i];
                //    //Added if condition for avoid duplicate entry in the list
                //    var val = from p in medicationList where p.Id == Convert.ToUInt32(ccObject[20]) select p;
                //    if (val.Count() == 0)
                //    {
                //        fillmedication.NDC_ID = ccObject[0].ToString();
                //        fillmedication.Generic_Name = ccObject[1].ToString();
                //        fillmedication.Strength = ccObject[2].ToString();
                //        fillmedication.Dose = ccObject[3].ToString();
                //        fillmedication.Route = ccObject[4].ToString();
                //        fillmedication.Dose_Other = ccObject[5].ToString();
                //        fillmedication.Patient_Notes = ccObject[6].ToString();
                //        fillmedication.Start_Date = Convert.ToDateTime(ccObject[8].ToString());
                //        fillmedication.Brand_Name = ccObject[9].ToString();
                //        fillmedication.Stop_Date = Convert.ToDateTime(ccObject[10].ToString());
                //        fillmedication.Quantity = ccObject[11].ToString();
                //        fillmedication.Quantity_Unit = ccObject[12].ToString();
                //        fillmedication.First_DataBank_Med_ID = ccObject[13].ToString();
                //        fillmedication.ICD_Code = ccObject[14].ToString();
                //        fillmedication.ICD_Code_Description = ccObject[15].ToString();
                //        fillmedication.Refills = ccObject[16].ToString();
                //        fillmedication.Action = ccObject[17].ToString();
                //        fillmedication.Dose_Unit = ccObject[18].ToString();
                //        fillmedication.Dose_Timing = ccObject[19].ToString();
                //        medicationList.Add(fillmedication);
                //    }
                //}
                iMySession.Close();
            }
            return medicationList;
        }
        //public IList<Medication> GetCSMedicationHistory(ulong ulEncounterID)
        //{
        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    IList<Medication> medicationList = new List<Medication>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = iMySession.GetNamedQuery("Fill.CSMedicationHistory.List");
        //        query1.SetParameter(0, getHumanID(ulEncounterID));
        //        ArrayList arrayList = new ArrayList(query1.List());
        //        for (int i = 0; i < arrayList.Count; i++)
        //        {
        //            Medication fillmedication = new Medication();
        //            object[] ccObject = (object[])arrayList[i];
        //            if (ccObject[0] != null)
        //            fillmedication.Drug_Name = ccObject[0].ToString();
        //            if (ccObject[1] != null)
        //            fillmedication.Dosage = ccObject[1].ToString();
        //            if (ccObject[2] != null)
        //            fillmedication.Frequency = ccObject[2].ToString();
        //            if (ccObject[3] != null)
        //            fillmedication.Medication_Notes = ccObject[3].ToString();
        //            medicationList.Add(fillmedication);
        //        }
        //        iMySession.Close();
        //    }
        //    return medicationList;
        //}
        public IList<NonDrugAllergy> GetCSNonDrugAllergy(ulong ulEncounterID)
        {
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<NonDrugAllergy> NonDrugAllergyHistoryList = new List<NonDrugAllergy>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSNonDrugAllergy.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    NonDrugAllergy fillNonDrugAllergyHistory = new NonDrugAllergy();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillNonDrugAllergyHistory.Non_Drug_Allergy_History_Info = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillNonDrugAllergyHistory.Description = ccObject[1].ToString();
                    NonDrugAllergyHistoryList.Add(fillNonDrugAllergyHistory);
                }
                iMySession.Close();
            }
            return NonDrugAllergyHistoryList;
        }
        public IList<GeneralNotes> GetCSNonDrugAllergyNotes(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<GeneralNotes> rosList = new List<GeneralNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSNonDrugAllergyNotes.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    GeneralNotes fillROS = new GeneralNotes();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillROS.Parent_Field = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillROS.Name_Of_The_Field = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillROS.Notes = ccObject[2].ToString();
                    rosList.Add(fillROS);
                }
                iMySession.Close();
            }
            return rosList;
        }
        public IList<FamilyHistory> GetCSFamilyHistory(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<FamilyHistory> FamilyHistoryList = new List<FamilyHistory>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSFamilyHistory.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    FamilyHistory fillFamilyHistory = new FamilyHistory();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillFamilyHistory.RelationShip = ccObject[0].ToString();
                    //fillFamilyHistory.Family_History_Notes = ccObject[1].ToString();
                    if (ccObject[1] != null)
                    fillFamilyHistory.Id = Convert.ToUInt32(ccObject[1].ToString());
                    if (ccObject[2] != null)
                    fillFamilyHistory.Age = Convert.ToInt32(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    fillFamilyHistory.Status = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillFamilyHistory.Cause_Of_Death = ccObject[4].ToString();
                    FamilyHistoryList.Add(fillFamilyHistory);
                }
                iMySession.Close();
            }
            return FamilyHistoryList;
        }
        public IList<GeneralNotes> GetCSFamilyHistoryNotes(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<GeneralNotes> FamilyHistoryList = new List<GeneralNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSFamilyHistoryNotes.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    GeneralNotes fillFamilyHistory = new GeneralNotes();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillFamilyHistory.Parent_Field = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillFamilyHistory.Name_Of_The_Field = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillFamilyHistory.Notes = ccObject[2].ToString();
                    FamilyHistoryList.Add(fillFamilyHistory);
                }
                iMySession.Close();
            }
            return FamilyHistoryList;
        }
        public IList<AdvanceDirective> GetCSAdvanceDirective(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<AdvanceDirective> AdvList = new List<AdvanceDirective>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSAdvanceDirective.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    AdvanceDirective filladvance = new AdvanceDirective();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    filladvance.Status = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    filladvance.Comments = ccObject[1].ToString();
                    AdvList.Add(filladvance);
                }
                iMySession.Close();
            }
            return AdvList;
        }
        public IList<PhysicianPatient> GetCSPhysicianPatient(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<PhysicianPatient> PhyPatList = new List<PhysicianPatient>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSPhysicianPatient.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PhysicianPatient fillPhyPat = new PhysicianPatient();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillPhyPat.Physician_Name = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillPhyPat.Relationship = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillPhyPat.Phone_No = ccObject[2].ToString();
                    PhyPatList.Add(fillPhyPat);
                }
                iMySession.Close();
            }
            return PhyPatList;
        }
        public IList<SocialHistory> GetCSSocialHistory(ulong ulEncounterID, ulong ulHumanID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<SocialHistory> SocialHistoryList = new List<SocialHistory>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSSocialHistory.List");
                ulong ulHuman_ID = ulHumanID;
                //query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(0, ulHuman_ID);
                ArrayList arrayList = new ArrayList(query1.List());
                //for (int i = 0; i < arrayList.Count; i++)
                //{
                //    SocialHistory fillSocialHistory = new SocialHistory();
                //    object[] ccObject = (object[])arrayList[i];
                //    fillSocialHistory.Social_Info = ccObject[0].ToString();
                //    fillSocialHistory.Description = ccObject[1].ToString();
                //    fillSocialHistory.Value = ccObject[2].ToString();
                //    fillSocialHistory.Is_Present = ccObject[3].ToString();
                //    fillSocialHistory.Created_Date_And_Time =Convert.ToDateTime(ccObject[4].ToString());
                //    fillSocialHistory.Recodes = ccObject[5].ToString();
                //    SocialHistoryList.Add(fillSocialHistory);
                //}
                IList<SocialHistory> ilstSocialHistory = new List<SocialHistory>();
                if (arrayList.Count > 0)
                {
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        SocialHistory fillSocialHistory = new SocialHistory();
                        object[] ccObject = (object[])arrayList[i];
                        if (ccObject[0] != null)
                        fillSocialHistory.Social_Info = ccObject[0].ToString();
                        if (ccObject[1] != null)
                        fillSocialHistory.Description = ccObject[1].ToString();
                        if (ccObject[2] != null)
                        fillSocialHistory.Value = ccObject[2].ToString();
                        if (ccObject[3] != null)
                        fillSocialHistory.Is_Present = ccObject[3].ToString();
                        if (ccObject[4] != null)
                        fillSocialHistory.Created_Date_And_Time = Convert.ToDateTime(ccObject[4].ToString());
                        if (ccObject[5] != null)
                        fillSocialHistory.Recodes = ccObject[5].ToString();
                        if (ccObject[6] != null)
                        fillSocialHistory.Encounter_ID = Convert.ToUInt32(ccObject[6].ToString());
                        ilstSocialHistory.Add(fillSocialHistory);
                        //if (i == 0)
                        //    EncounterId = ilstSocialHistory[0].Encounter_ID;
                        //SocialHistoryList.Add(fillSocialHistory);
                    }
                    ulong Encoun_ID = 0;
                    ICriteria Criteria = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", ulHuman_ID));
                    IList<Encounter> lstEncounter = Criteria.List<Encounter>();
                    IList<ulong> lstEncounterId = lstEncounter.Where(w => w.Id <= ulEncounterID).Select(Q => Q.Id).OrderByDescending(a => a).ToList();
                    foreach (ulong enc_id in lstEncounterId)
                    {
                        if (ilstSocialHistory.Any(a => a.Encounter_ID == enc_id))
                        {
                            Encoun_ID = enc_id;
                            break;
                        }
                    }

                    if (ilstSocialHistory.Count > 0)
                    {
                        SocialHistoryList = (from p in ilstSocialHistory where p.Encounter_ID == Encoun_ID select p).ToList();
                    }

                }
                iMySession.Close();
            }
            return SocialHistoryList;
        }
        public IList<GeneralNotes> GetCSSocialHistoryNotes(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<GeneralNotes> rosList = new List<GeneralNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSSocialNotes.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    GeneralNotes fillROS = new GeneralNotes();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillROS.Parent_Field = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillROS.Name_Of_The_Field = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillROS.Notes = ccObject[2].ToString();
                    rosList.Add(fillROS);
                }
                iMySession.Close();
            }
            return rosList;
        }
        public IList<FamilyDisease> GetCSFamilyDiseaseDetails(ulong ulFamilyId)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<FamilyDisease> familyDiseaseList = new List<FamilyDisease>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSDiseaseDetails.List");
                query1.SetParameter(0, ulFamilyId);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    FamilyDisease familyDisease = new FamilyDisease();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    familyDisease.Family_History_ID = Convert.ToUInt32(ccObject[0].ToString());
                    if (ccObject[1] != null)
                    familyDisease.Disease = ccObject[1].ToString();
                    familyDiseaseList.Add(familyDisease);
                }
                iMySession.Close();
            }
            return familyDiseaseList;
        }
        public IList<Assessment> GetCSAssessment(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<Assessment> AssessmentList = new List<Assessment>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSAssessment.List");
                query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(1, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Assessment fillAssessment = new Assessment();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillAssessment.ICD_9 = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillAssessment.ICD_Description = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillAssessment.Assessment_Notes = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillAssessment.Assessment_Status = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillAssessment.Primary_Diagnosis = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillAssessment.Version_Year = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    fillAssessment.Chronic_Problem = ccObject[6].ToString();
                    AssessmentList.Add(fillAssessment);
                }
                iMySession.Close();
            }
            return AssessmentList;
        }
        public IList<GeneralNotes> GetCSAssessmentNotes(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<GeneralNotes> rosList = new List<GeneralNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSAssessmentNotes.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    GeneralNotes fillROS = new GeneralNotes();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillROS.Parent_Field = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillROS.Name_Of_The_Field = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillROS.Notes = ccObject[2].ToString();
                    rosList.Add(fillROS);
                }
                iMySession.Close();
            }
            return rosList;
        }
        public IList<Immunization> GetCSImmunization(ulong ulEncounterID, ulong ulHumanID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<Immunization> ImmunizationList = new List<Immunization>();
            IList<Immunization> ImmunizationListNew = new List<Immunization>();
            IList<Immunization> ilstImmun = new List<Immunization>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                // ICriteria crit = session.GetISession().CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Encounter_Id", ulEncounterID));
                ICriteria crit = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Human_ID", ulHumanID)).AddOrder(Order.Desc("Encounter_Id"));
                ImmunizationList = crit.List<Immunization>();
                if (ImmunizationList.Count > 0)
                {

                    ilstImmun = (from p in ImmunizationList where p.Encounter_Id == ulEncounterID select p).ToList();
                    for (int i = 0; i < ilstImmun.Count; i++)
                    {
                        ImmunizationListNew.Add(ilstImmun[i]);
                    }
                    //ImmunizationListNew.Add(ImmunizationList[0]);
                }
                if (ImmunizationListNew.Count == 0)
                {
                    ulong Encoun_ID = 0;
                    ICriteria Criteria = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Human_ID", ulHumanID));
                    IList<Encounter> lstEncounter = Criteria.List<Encounter>();
                    IList<ulong> lstEncounterId = lstEncounter.Where(w => w.Id <= ulEncounterID).Select(Q => Q.Id).OrderByDescending(a => a).ToList();
                    foreach (ulong enc_id in lstEncounterId)
                    {
                        if (ImmunizationList.Any(a => a.Encounter_Id == enc_id))
                        {
                            Encoun_ID = enc_id;
                            break;
                        }
                    }

                    if (ImmunizationList.Count > 0)
                    {
                        ImmunizationListNew = (from p in ImmunizationList where p.Encounter_Id == Encoun_ID select p).ToList();
                    }
                }
                //IQuery query1 = session.GetISession().GetNamedQuery("Fill.CSImmunization.List");
                //query1.SetParameter(0, ulEncounterID);
                //ArrayList arrayList = new ArrayList(query1.List());
                //for (int i = 0; i < arrayList.Count; i++)
                //{
                //    Immunization fillImmunization = new Immunization();
                //    object[] ccObject = (object[])arrayList[i];
                //    fillImmunization.Procedure_Code = ccObject[0].ToString();
                //    fillImmunization.Immunization_Description = ccObject[1].ToString();
                //    fillImmunization.Given_Date = Convert.ToDateTime(ccObject[2].ToString());
                //    fillImmunization.Dose = Convert.ToUInt64(ccObject[3].ToString());
                //    fillImmunization.Dose_No = Convert.ToUInt64(ccObject[4].ToString());
                //    fillImmunization.Route_of_Administration = ccObject[5].ToString();
                //    fillImmunization.Notes = ccObject[6].ToString();
                //    ImmunizationList.Add(fillImmunization);
                //}
                iMySession.Close();
            }
            return ImmunizationListNew;
        }
        public IList<ImmunizationHistory> GetCSImmunizationHistory(ulong HumanID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<ImmunizationHistory> ImmunizationHistoryList = new List<ImmunizationHistory>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ImmunizationHistory)).Add(Expression.Eq("Human_ID", HumanID));
                ImmunizationHistoryList = crit.List<ImmunizationHistory>();
                iMySession.Close();
            }
            return ImmunizationHistoryList;
        }
        public IList<VaccineManufacturerCodes> GetCSVaccineCodes()
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<VaccineManufacturerCodes> VaccineCodes = new List<VaccineManufacturerCodes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(VaccineManufacturerCodes));
                VaccineCodes = crit.List<VaccineManufacturerCodes>();
                iMySession.Close();
            }
            return VaccineCodes;
        }


        public IList<StaticLookup> GetCSlookUpValues()
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<StaticLookup> lookUpValues = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select * from static_lookup where field_name in ('RACE' , 'ETHNICITY' , 'PUBLICITY CODE' , 'RELATIONSHIP' , 'ROUTE OF ADMINISTRATION' , 'REFUSED_ADMINISTRATION' , 'VFC STATUS' , 'IMMUNIZATION_INFORMATION_SOURCE' , 'REFUSED_ADMINISTRATION' , 'VACCINE TYPE' , 'IMMUNIZATION PROTECTION TYPE' , 'OBSERVATION' , 'IMMUNIZATION EVIDENCE' ,'IMMUNIZATIONLOCATION', 'IMMUNIZATION DOCUMENTATION','')").AddEntity(typeof(StaticLookup));
                lookUpValues = sql.List<StaticLookup>();
                iMySession.Close();
            }
            return lookUpValues;
        }
        public IList<PhysicianLibrary> GetCSPhyList(ulong ulPhysicianID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(PhysicianLibrary)).Add(Expression.Eq("Id", ulPhysicianID));
                PhyList = crit.List<PhysicianLibrary>();
                iMySession.Close();
            }
            return PhyList;
        }
        public IList<Orders> GetCSOrders(ulong ulencounterid)
        {
            IList<Orders> orderslist = new List<Orders>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSOrders.List");
                query1.SetParameter(0, ulencounterid);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Orders fillorder = new Orders();
                    object[] ccobject = (object[])arrayList[i];
                    if (ccobject[0] != null)
                    fillorder.Lab_Procedure_Description= ccobject[0].ToString();
                    if (ccobject[1] != null)
                    fillorder.Created_Date_And_Time = Convert.ToDateTime(ccobject[1].ToString());//Assing test date
                    //fillorder.order_notes = ccobject[2].ToString();
                    if (ccobject[3] != null)
                    fillorder.Lab_Procedure = ccobject[3].ToString();
                    orderslist.Add(fillorder);
                }
                iMySession.Close();
            }
            return orderslist;
        }
        public IList<InHouseProcedure> GetCSInHouseProcedures(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<InHouseProcedure> OtherProcList = new List<InHouseProcedure>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSInHouseProcedure.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    InHouseProcedure fillOtherProc = new InHouseProcedure();
                    object[] ccObject = (object[])arrayList[i];
                    if(ccObject[0] != null)
                    fillOtherProc.Procedure_Code = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillOtherProc.Procedure_Code_Description = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillOtherProc.Created_Date_And_Time = Convert.ToDateTime(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    fillOtherProc.Notes = ccObject[3].ToString();
                    OtherProcList.Add(fillOtherProc);
                }
                iMySession.Close();
            }
            return OtherProcList;
        }
        public IList<ProblemList> GetCSProblemList(ulong ulHumanID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<Encounter> EncList = new List<Encounter>();
            EncounterManager EncMngr = new EncounterManager();
            IList<ProblemList> ProbList = new List<ProblemList>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //if (EncList.Count > 0)
                //{
                    IQuery query1 = iMySession.GetNamedQuery("Fill.CSProblemList.List");
                    query1.SetParameter(0, ulHumanID);
                    ArrayList arrayList = new ArrayList(query1.List());
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        ProblemList fillProbList = new ProblemList();
                        object[] ccObject = (object[])arrayList[i];
                        if (ccObject[0] != null)
                        fillProbList.Problem_Description = ccObject[0].ToString();
                        if (ccObject[1] != null)
                        fillProbList.Created_Date_And_Time = Convert.ToDateTime(ccObject[1].ToString());
                        if (ccObject[2] != null)
                        fillProbList.Date_Diagnosed = ccObject[2].ToString();
                        if (ccObject[3] != null)
                        fillProbList.Status = ccObject[3].ToString();
                        if (ccObject[4] != null)
                        fillProbList.ICD = ccObject[4].ToString();
                        if (ccObject[5] != null)
                        fillProbList.Is_Active = ccObject[5].ToString();
                        if (ccObject[6] != null)
                        fillProbList.Snomed_Code = ccObject[6].ToString();
                        if (ccObject[7] != null)
                        fillProbList.Snomed_Code_Description = ccObject[7].ToString();
                        ProbList.Add(fillProbList);
                    }
               // }
                iMySession.Close();
            }
            return ProbList;
        }

        public IList<ProblemList> GetCSProblemListActiveandresolved(ulong ulHumanID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<Encounter> EncList = new List<Encounter>();
            EncounterManager EncMngr = new EncounterManager();
            IList<ProblemList> ProbList = new List<ProblemList>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //if (EncList.Count > 0)
                //{
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSProblemListActiveResolved.List");
                query1.SetParameter(0, ulHumanID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    ProblemList fillProbList = new ProblemList();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillProbList.Problem_Description = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillProbList.Created_Date_And_Time = Convert.ToDateTime(ccObject[1].ToString());
                    if (ccObject[2] != null)
                    fillProbList.Date_Diagnosed = ccObject[2].ToString();
                    if (ccObject[3] != null && ccObject[3].ToString().ToUpper()== "RESOLVED")
                        fillProbList.Status = "Completed";
                    else
                        fillProbList.Status = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillProbList.ICD = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillProbList.Is_Active = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    fillProbList.Resolved_Date = ccObject[6].ToString();
                    if (ccObject[7] != null)
                    fillProbList.Snomed_Code = ccObject[7].ToString();
                    if (ccObject[8] != null)
                    fillProbList.Snomed_Code_Description = ccObject[8].ToString();
                    
                    ProbList.Add(fillProbList);
                }
                // }
                iMySession.Close();
            }
            return ProbList;
        }

        public IList<CarePlan> GetCSCarePlanList(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<CarePlan> CareList = new List<CarePlan>();
            IList<CarePlan> CarePlanValue = new List<CarePlan>();
            IList<CarePlan> CarePlanFinal = new List<CarePlan>();
            IList<PatientResults> VitalsValue = new List<PatientResults>();
            IList<PatientResults> LatestVitalsValue = new List<PatientResults>();
            IList<AdvanceDirective> adList = new List<AdvanceDirective>();
            IList<SocialHistory> SocialList = new List<SocialHistory>();
            IList<SocialHistory> SocialListFinal = new List<SocialHistory>();
            IList<CarePlanLookup> CarePlanLookup = new List<CarePlanLookup>();
            IList<CarePlanLookup> CarePlanLookupHead = new List<CarePlanLookup>();
            IList<CarePlanLookup> CarePlanLookupLabel = new List<CarePlanLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ulong ulHumanID = getHumanID(ulEncounterID);


                ICriteria crt1 = iMySession.CreateCriteria(typeof(CarePlanLookup));
                CarePlanLookup = crt1.List<CarePlanLookup>();
                var careplanHead = from l in CarePlanLookup where l.Field_Name == "Care_Name" select l;
                CarePlanLookupHead = careplanHead.ToList<CarePlanLookup>();
                var careplanLabel = from l in CarePlanLookup where l.Field_Name == "Care_Name_Value" select l;
                CarePlanLookupLabel = careplanLabel.ToList<CarePlanLookup>();
                ISQLQuery sql1 = iMySession.CreateSQLQuery("select o.*from care_plan o  where o.Encounter_ID='" + ulEncounterID + "' and (o.Status <>'' or o.Care_Plan_Notes<>'' or o.Status_Value<>'')").AddEntity("o", typeof(CarePlan));
                CarePlanValue = sql1.List<CarePlan>();
                //IQuery query1 = session.GetISession().GetNamedQuery("Fill.CSCarePlan.List");
                //query1.SetParameter(0, ulEncounterID);
                //ArrayList arrayList = new ArrayList(query1.List());

                //for (int i = 0; i < arrayList.Count; i++)
                //{
                //    CarePlan fillCareList = new CarePlan();
                //    object[] ccObject = (object[])arrayList[i];
                //    fillCareList.Care_Name = ccObject[0].ToString();
                //    fillCareList.Care_Name_Value = ccObject[1].ToString();
                //    fillCareList.Status = ccObject[2].ToString();
                //    fillCareList.Care_Plan_Notes = ccObject[3].ToString();
                //    fillCareList.Plan_Date = ccObject[4].ToString();
                //    fillCareList.Status_Value = ccObject[5].ToString();
                //    CareList.Add(fillCareList);
                //}
                foreach (CarePlanLookup obj in CarePlanLookupLabel)
                {
                    if (obj.Value == "Marital Status" || obj.Value == "BMI" || obj.Value == "Advance Directive on File")
                    {

                        CarePlan fillCareList = new CarePlan();
                        if (obj.Value == "Marital Status")
                        {
                            ICriteria crt = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Social_Info", "Marital Status")).Add(Expression.Eq("Is_Present", "Y"));
                            SocialList = crt.List<SocialHistory>();
                            var query1 = from q1 in SocialList where q1.Social_Info == "Marital Status" select q1;
                            SocialListFinal = query1.ToList<SocialHistory>();
                            var quer2 = from q2 in CarePlanValue where q2.Care_Name_Value == "Marital Status" select q2.Care_Plan_Notes;
                            if(CarePlanLookupHead.Count>0)
                            fillCareList.Care_Name = CarePlanLookupHead[1].Value;
                            fillCareList.Care_Name_Value = "Marital Status";
                            if (SocialListFinal.Count > 0)
                            {
                                fillCareList.Status = SocialListFinal[0].Value;
                            }
                            if (quer2.Count() > 0)
                            {
                                fillCareList.Care_Plan_Notes = quer2.First();
                            }
                            else
                            {
                                if (SocialListFinal.Count > 0)
                                {
                                    fillCareList.Care_Plan_Notes = SocialListFinal[0].Description;
                                }
                            }

                            //fillCareList.Care_Plan_Notes = ccObject[3].ToString();
                            //fillCareList.Plan_Date = ccObject[4].ToString();
                            //fillCareList.Status_Value = ccObject[5].ToString();
                            if (fillCareList.Status != string.Empty || fillCareList.Care_Plan_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }
                        }
                        if (obj.Value == "BMI")
                        {
                            ISQLQuery sql = iMySession.CreateSQLQuery("Select a.* from patient_results a where (a.Loinc_Observation ='BMI' or a.Loinc_Observation ='BMI STATUS')  and a.captured_date_and_time=(Select Max(s.captured_date_and_time) from patient_results s where s.Encounter_ID = '" + ulEncounterID.ToString() + "' and (Loinc_Observation ='BMI' or Loinc_Observation ='BMI STATUS') and Results_Type='Vitals')").AddEntity("a", typeof(PatientResults));
                            VitalsValue = sql.List<PatientResults>();
                            var CarePlanBMIExtractList = from b in CarePlanValue where b.Care_Name_Value == "BMI" select b.Care_Plan_Notes;
                            if(CarePlanLookupHead.Count>0)
                            fillCareList.Care_Name = CarePlanLookupHead[2].Value;
                            fillCareList.Care_Name_Value = "BMI";
                            if (VitalsValue.Count > 0 && CarePlanBMIExtractList.Count() > 0)
                            {
                                fillCareList.Status_Value = VitalsValue[0].Value;
                                if (CarePlanBMIExtractList.Count() > 0)
                                {
                                    if (CarePlanBMIExtractList.First() != string.Empty)
                                    {//mohan
                                        fillCareList.Care_Plan_Notes = VitalsValue[0].Value + "-" + CarePlanBMIExtractList.First();
                                    }
                                    else
                                    {
                                        if (VitalsValue[1] != null)
                                        fillCareList.Care_Plan_Notes = VitalsValue[1].Value;
                                    }
                                }
                                else
                                {
                                    if (VitalsValue[1] != null)
                                    fillCareList.Care_Plan_Notes = VitalsValue[1].Value;
                                }
                            }
                            else if (VitalsValue.Count > 0)
                            {
                                if (VitalsValue.Count == 1 || VitalsValue.Count == 2)
                                {
                                    fillCareList.Status_Value = VitalsValue[0].Value;
                                    if (VitalsValue.Count == 2)//Added if condition for BugID:29551 
                                        fillCareList.Care_Plan_Notes = VitalsValue[1].Value;
                                }
                                if (VitalsValue.Count > 2)
                                {
                                    ISQLQuery sqlQuery = iMySession.CreateSQLQuery("Select a.* from patient_results a where a.Encounter_ID = '" + ulEncounterID.ToString() + "' and a.human_id = '" + ulHumanID.ToString() + "'  and (Loinc_Observation ='BMI' or Loinc_Observation ='BMI STATUS') and Results_Type='Vitals' order by a.Captured_date_and_time desc,a.vitals_group_id desc").AddEntity("a", typeof(PatientResults));
                                    LatestVitalsValue = sqlQuery.List<PatientResults>();
                                    if (LatestVitalsValue.Count > 0)
                                        fillCareList.Status_Value = LatestVitalsValue[0].Value;
                                    if (LatestVitalsValue.Count > 1)
                                        fillCareList.Care_Plan_Notes = LatestVitalsValue[1].Value;
                                }
                            }
                            else
                            {
                                if (CarePlanBMIExtractList.Count() > 0)
                                {
                                    fillCareList.Care_Plan_Notes = CarePlanBMIExtractList.First();
                                }
                            }



                            if (fillCareList.Status_Value != string.Empty || fillCareList.Care_Plan_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }
                        }
                        if (obj.Value == "Advance Directive on File")
                        {
                            ICriteria crit1 = iMySession.CreateCriteria(typeof(AdvanceDirective)).Add(Expression.Eq("Human_ID", ulHumanID));
                            adList = crit1.List<AdvanceDirective>();
                            var AdQuery = from a in CarePlanValue where a.Care_Name_Value == "Advance Directive on File" select a.Care_Plan_Notes;
                            if (CarePlanLookupHead[4] != null)
                            fillCareList.Care_Name = CarePlanLookupHead[4].Value;
                            fillCareList.Care_Name_Value = "Advance Directive on File";
                            if (adList.Count > 0)
                            {
                                fillCareList.Status = adList[0].Status;
                            }
                            if (AdQuery.Count() > 0)
                            {
                                fillCareList.Care_Plan_Notes = AdQuery.First();
                            }
                            else
                            {
                                if (adList.Count > 0)
                                {
                                    fillCareList.Care_Plan_Notes = adList[0].Comments;
                                }
                            }
                            if (fillCareList.Status != string.Empty || fillCareList.Care_Plan_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }
                        }

                    }
                    else if (obj.Value == "Hemoglobin" || obj.Value == "Serum Albumin")
                    {
                        var query = from q in CarePlanValue where q.Care_Name_Value == obj.Value select q;
                        CarePlanFinal = query.ToList<CarePlan>();
                        if (CarePlanFinal.Count > 0)
                        {
                            CarePlan fillCareList = new CarePlan();
                            fillCareList.Care_Name = CarePlanFinal[0].Care_Name;
                            fillCareList.Care_Name_Value = CarePlanFinal[0].Care_Name_Value;
                            fillCareList.Status = CarePlanFinal[0].Status_Value;
                            fillCareList.Care_Plan_Notes = CarePlanFinal[0].Care_Plan_Notes;
                            fillCareList.Plan_Date = CarePlanFinal[0].Plan_Date;
                            //fillCareList.Status_Value = CarePlanFinal[0].Status_Value;
                            if (fillCareList.Status != string.Empty || fillCareList.Care_Plan_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }
                        }

                    }
                    else
                    {
                        var query = from q in CarePlanValue where q.Care_Name_Value == obj.Value select q;
                        CarePlanFinal = query.ToList<CarePlan>();
                        if (CarePlanFinal.Count > 0)
                        {
                            CarePlan fillCareList = new CarePlan();
                            fillCareList.Care_Name = CarePlanFinal[0].Care_Name;
                            fillCareList.Care_Name_Value = CarePlanFinal[0].Care_Name_Value;
                            fillCareList.Status = CarePlanFinal[0].Status;
                            fillCareList.Care_Plan_Notes = CarePlanFinal[0].Care_Plan_Notes;
                            fillCareList.Plan_Date = CarePlanFinal[0].Plan_Date;
                            fillCareList.Status_Value = CarePlanFinal[0].Status_Value;
                            CareList.Add(fillCareList);
                        }
                    }

                }

                iMySession.Close();
            }
            return CareList;
        }
        public IList<CarePlan> GetCSCarePlanListforcareplan(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<CarePlan> CareList = new List<CarePlan>();
            IList<CarePlan> CarePlanValue = new List<CarePlan>();
            IList<CarePlan> CarePlanFinal = new List<CarePlan>();
            IList<PatientResults> VitalsValue = new List<PatientResults>();
            IList<PatientResults> LatestVitalsValue = new List<PatientResults>();
            IList<AdvanceDirective> adList = new List<AdvanceDirective>();
            IList<SocialHistory> SocialList = new List<SocialHistory>();
            IList<SocialHistory> SocialListFinal = new List<SocialHistory>();
            IList<CarePlanLookup> CarePlanLookup = new List<CarePlanLookup>();
            IList<CarePlanLookup> CarePlanLookupHead = new List<CarePlanLookup>();
            IList<CarePlanLookup> CarePlanLookupLabel = new List<CarePlanLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ulong ulHumanID = getHumanID(ulEncounterID);


                ICriteria crt1 = iMySession.CreateCriteria(typeof(CarePlanLookup));
                CarePlanLookup = crt1.List<CarePlanLookup>();
                var careplanHead = from l in CarePlanLookup where l.Field_Name == "Care_Name" select l;
                CarePlanLookupHead = careplanHead.ToList<CarePlanLookup>();
                var careplanLabel = from l in CarePlanLookup where l.Field_Name == "Care_Name_Value" select l;
                CarePlanLookupLabel = careplanLabel.ToList<CarePlanLookup>();
                //ISQLQuery sql1 = iMySession.CreateSQLQuery("select o.*from care_plan o  where o.Encounter_ID='" + ulEncounterID + "' and (o.Status ='Yes' and o.Care_Plan_Notes<>'' or o.Status_Value<>'')").AddEntity("o", typeof(CarePlan));
                ISQLQuery sql1 = iMySession.CreateSQLQuery("select o.*from care_plan o  where o.Encounter_ID='" + ulEncounterID + "' and o.Care_Plan_Notes<>''").AddEntity("o", typeof(CarePlan));
                CarePlanValue = sql1.List<CarePlan>();
                //IQuery query1 = session.GetISession().GetNamedQuery("Fill.CSCarePlan.List");
                //query1.SetParameter(0, ulEncounterID);
                //ArrayList arrayList = new ArrayList(query1.List());

                //for (int i = 0; i < arrayList.Count; i++)
                //{
                //    CarePlan fillCareList = new CarePlan();
                //    object[] ccObject = (object[])arrayList[i];
                //    fillCareList.Care_Name = ccObject[0].ToString();
                //    fillCareList.Care_Name_Value = ccObject[1].ToString();
                //    fillCareList.Status = ccObject[2].ToString();
                //    fillCareList.Care_Plan_Notes = ccObject[3].ToString();
                //    fillCareList.Plan_Date = ccObject[4].ToString();
                //    fillCareList.Status_Value = ccObject[5].ToString();
                //    CareList.Add(fillCareList);
                //}
                foreach (CarePlanLookup obj in CarePlanLookupLabel)
                {
                    if (obj.Value == "Marital Status" || obj.Value == "BMI" || obj.Value == "Advance Directive on File")
                    {

                        CarePlan fillCareList = new CarePlan();
                        if (obj.Value == "Marital Status")
                        {
                            ICriteria crt = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Social_Info", "Marital Status")).Add(Expression.Eq("Is_Present", "Y"));
                            SocialList = crt.List<SocialHistory>();
                            var query1 = from q1 in SocialList where q1.Social_Info == "Marital Status" select q1;
                            SocialListFinal = query1.ToList<SocialHistory>();
                            var quer2 = from q2 in CarePlanValue where q2.Care_Name_Value == "Marital Status" select q2.Care_Plan_Notes;
                            fillCareList.Care_Name = CarePlanLookupHead[1].Value;
                            fillCareList.Care_Name_Value = "Marital Status";
                            if (SocialListFinal.Count > 0)
                            {
                                fillCareList.Status = SocialListFinal[0].Value;
                            }
                            if (quer2.Count() > 0)
                            {
                                fillCareList.Care_Plan_Notes = quer2.First();
                            }
                            else
                            {
                                if (SocialListFinal.Count > 0)
                                {
                                    fillCareList.Care_Plan_Notes = SocialListFinal[0].Description;
                                }
                            }

                            //fillCareList.Care_Plan_Notes = ccObject[3].ToString();
                            //fillCareList.Plan_Date = ccObject[4].ToString();
                            //fillCareList.Status_Value = ccObject[5].ToString();
                            if (fillCareList.Status != string.Empty || fillCareList.Care_Plan_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }
                        }
                        if (obj.Value == "BMI")
                        {
                            ISQLQuery sql = iMySession.CreateSQLQuery("Select a.* from patient_results a where (a.Loinc_Observation ='BMI' or a.Loinc_Observation ='BMI STATUS')  and a.captured_date_and_time=(Select Max(s.captured_date_and_time) from patient_results s where s.Encounter_ID = '" + ulEncounterID.ToString() + "' and (Loinc_Observation ='BMI' or Loinc_Observation ='BMI STATUS') and Results_Type='Vitals')").AddEntity("a", typeof(PatientResults));
                            VitalsValue = sql.List<PatientResults>();
                            var CarePlanBMIExtractList = from b in CarePlanValue where b.Care_Name_Value == "BMI" select b.Care_Plan_Notes;
                            fillCareList.Care_Name = CarePlanLookupHead[2].Value;
                            fillCareList.Care_Name_Value = "BMI";
                            if (VitalsValue.Count > 0 && CarePlanBMIExtractList.Count() > 0)
                            {
                                fillCareList.Status_Value = VitalsValue[0].Value;
                                if (CarePlanBMIExtractList.Count() > 0)
                                {
                                    if (CarePlanBMIExtractList.First() != string.Empty)
                                    {//mohan
                                        fillCareList.Care_Plan_Notes = VitalsValue[0].Value + "-" + CarePlanBMIExtractList.First();
                                    }
                                    else
                                    {
                                        if (VitalsValue[1] != null)
                                        fillCareList.Care_Plan_Notes = VitalsValue[1].Value;
                                    }
                                }
                                else
                                {
                                    if (VitalsValue[1] != null)
                                    fillCareList.Care_Plan_Notes = VitalsValue[1].Value;
                                }
                            }
                            else if (VitalsValue.Count > 0)
                            {
                                if (VitalsValue.Count == 1 || VitalsValue.Count == 2)
                                {
                                    fillCareList.Status_Value = VitalsValue[0].Value;
                                    if (VitalsValue.Count == 2)//Added if condition for BugID:29551 
                                        fillCareList.Care_Plan_Notes = VitalsValue[1].Value;
                                }
                                if (VitalsValue.Count > 2)
                                {
                                    ISQLQuery sqlQuery = iMySession.CreateSQLQuery("Select a.* from patient_results a where a.Encounter_ID = '" + ulEncounterID.ToString() + "' and a.human_id = '" + ulHumanID.ToString() + "'  and (Loinc_Observation ='BMI' or Loinc_Observation ='BMI STATUS') and Results_Type='Vitals' order by a.Captured_date_and_time desc,a.vitals_group_id desc").AddEntity("a", typeof(PatientResults));
                                    LatestVitalsValue = sqlQuery.List<PatientResults>();
                                    if (LatestVitalsValue.Count > 0)
                                        fillCareList.Status_Value = LatestVitalsValue[0].Value;
                                    if (LatestVitalsValue.Count > 1)
                                        fillCareList.Care_Plan_Notes = LatestVitalsValue[1].Value;
                                }
                            }
                            else
                            {
                                if (CarePlanBMIExtractList.Count() > 0)
                                {
                                    fillCareList.Care_Plan_Notes = CarePlanBMIExtractList.First();
                                }
                            }



                            if (fillCareList.Status_Value != string.Empty || fillCareList.Care_Plan_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }
                        }
                        if (obj.Value == "Advance Directive on File")
                        {
                            ICriteria crit1 = iMySession.CreateCriteria(typeof(AdvanceDirective)).Add(Expression.Eq("Human_ID", ulHumanID));
                            adList = crit1.List<AdvanceDirective>();
                            var AdQuery = from a in CarePlanValue where a.Care_Name_Value == "Advance Directive on File" select a.Care_Plan_Notes;
                            fillCareList.Care_Name = CarePlanLookupHead[4].Value;
                            fillCareList.Care_Name_Value = "Advance Directive on File";
                            if (adList.Count > 0)
                            {
                                fillCareList.Status = adList[0].Status;
                            }
                            if (AdQuery.Count() > 0)
                            {
                                fillCareList.Care_Plan_Notes = AdQuery.First();
                            }
                            else
                            {
                                if (adList.Count > 0)
                                {
                                    fillCareList.Care_Plan_Notes = adList[0].Comments;
                                }
                            }
                            if (fillCareList.Status != string.Empty || fillCareList.Care_Plan_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }
                        }

                    }
                    else if (obj.Value == "Hemoglobin" || obj.Value == "Serum Albumin")
                    {
                        var query = from q in CarePlanValue where q.Care_Name_Value == obj.Value select q;
                        CarePlanFinal = query.ToList<CarePlan>();
                        if (CarePlanFinal.Count > 0)
                        {
                            CarePlan fillCareList = new CarePlan();
                            fillCareList.Care_Name = CarePlanFinal[0].Care_Name;
                            fillCareList.Care_Name_Value = CarePlanFinal[0].Care_Name_Value;
                            fillCareList.Status = CarePlanFinal[0].Status_Value;
                            fillCareList.Care_Plan_Notes = CarePlanFinal[0].Care_Plan_Notes;
                            fillCareList.Plan_Date = CarePlanFinal[0].Plan_Date;
                            //fillCareList.Status_Value = CarePlanFinal[0].Status_Value;
                            if (fillCareList.Status != string.Empty || fillCareList.Care_Plan_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }
                        }

                    }
                    else
                    {
                        var query = from q in CarePlanValue where q.Care_Name_Value == obj.Value select q;
                        CarePlanFinal = query.ToList<CarePlan>();
                        if (CarePlanFinal.Count > 0)
                        {
                            CarePlan fillCareList = new CarePlan();
                            fillCareList.Care_Name = CarePlanFinal[0].Care_Name;
                            fillCareList.Care_Name_Value = CarePlanFinal[0].Care_Name_Value;
                            fillCareList.Status = CarePlanFinal[0].Status;
                            fillCareList.Care_Plan_Notes = CarePlanFinal[0].Care_Plan_Notes;
                            fillCareList.Plan_Date = CarePlanFinal[0].Plan_Date;
                            fillCareList.Status_Value = CarePlanFinal[0].Status_Value;
                            CareList.Add(fillCareList);
                        }
                    }

                }
                ISQLQuery sqlmedi = iMySession.CreateSQLQuery("select b.* from problem_list as a  join rcopia_medication as b where  a.encounter_id=b.encounter_id and a.icd=b.icd_code and is_health_concern='Y' and  a.encounter_id=" + ulEncounterID).AddEntity("o", typeof(Rcopia_Medication));
                IList<Rcopia_Medication> lst = sqlmedi.List<Rcopia_Medication>();

                for(int j=0;j<lst.Count;j++)
                {
                   CarePlan fillCareList = new CarePlan();

                    fillCareList.Status = "Yes";
                    fillCareList.Care_Plan_Notes = lst[j].Brand_Name + " " + lst[j].Strength + " " + lst[j].Dose_Timing +" " +lst[j].Dose_Other ;
                     CareList.Add(fillCareList);
                  
                }
                iMySession.Close();
            }
            return CareList;
        }
        public IList<PreventiveScreen> GetCSPreventivePlanList(ulong ulEncounterID)
        {

            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<PreventiveScreen> CareList = new List<PreventiveScreen>();
            IList<PreventiveScreenLookup> PreventivePlanLookup = new List<PreventiveScreenLookup>();
            IList<PreventiveScreenLookup> PreventivePlanLookupHead = new List<PreventiveScreenLookup>();
            IList<PreventiveScreenLookup> PreventivePlanLookupLabel = new List<PreventiveScreenLookup>();
            IList<PreventiveScreen> PrePlanValue = new List<PreventiveScreen>();
            IList<PreventiveScreen> PrePlanFinal = new List<PreventiveScreen>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                ICriteria crt2 = iMySession.CreateCriteria(typeof(PreventiveScreenLookup));
                PreventivePlanLookup = crt2.List<PreventiveScreenLookup>();

                var preventiveplanHead = from l in PreventivePlanLookup where l.Field_Name == "Preventive_Service" select l;
                PreventivePlanLookupHead = preventiveplanHead.ToList<PreventiveScreenLookup>();

                var PreplanLabel = from l in PreventivePlanLookup where l.Field_Name == "Preventive_Service_Value" select l;
                PreventivePlanLookupLabel = PreplanLabel.ToList<PreventiveScreenLookup>();

                ISQLQuery sql1 = iMySession.CreateSQLQuery("select o.*from preventive_screening o  where o.Encounter_ID='" + ulEncounterID + "' and (o.Status <>'' or o.Preventive_Screening_Notes<>'')").AddEntity("o", typeof(PreventiveScreen));
                PrePlanValue = sql1.List<PreventiveScreen>();

                foreach (PreventiveScreenLookup obj in PreventivePlanLookupLabel)
                {
                    if (obj.Value == "Most current HbA1c value in 2012 is <8.0" || obj.Value == "Most current BP in 2012 is <140/90")
                    {
                        PreventiveScreen fillCareList = new PreventiveScreen();

                        IList<DynamicScreen> DynamicscreenList = new List<DynamicScreen>();
                        ICriteria crt1 = iMySession.CreateCriteria(typeof(DynamicScreen));
                        DynamicscreenList = crt1.List<DynamicScreen>();

                        var loinc = from l in DynamicscreenList where l.Control_Name == "HbA1C" select l.Loinc_Identifier;
                        var loincbp = from l in DynamicscreenList where (l.Control_Name == "BP-Sitting Sys/Dia" || l.Control_Name == "BP-Standing Sys/Dia" || l.Control_Name == "BP-Lying Sys/Dia") select l.Loinc_Identifier;

                        if (obj.Value == "Most current HbA1c value in 2012 is <8.0")
                        {
                            IList<PatientResults> FinalVitalshba1cList = new List<PatientResults>();
                            IList<PatientResults> vitalsHBA1CList = new List<PatientResults>();
                            var hbalcNotes = from p in PrePlanValue where p.Preventive_Service_Value == "Most current HbA1c value in 2012 is <8.0" select p.Preventive_Screening_Notes;
                            //var hbalcstatus = from p in PrePlanValue where p.Preventive_Service_Value == "Most current HbA1c value in 2012 is <8.0" select p.Status;

                            ISQLQuery sql2 = iMySession.CreateSQLQuery("Select a.* from patient_results a where a.Encounter_ID = '" + ulEncounterID.ToString() + "'  and a.Loinc_Identifier='" + loinc.First() + "'").AddEntity("a", typeof(PatientResults));
                            vitalsHBA1CList = sql2.List<PatientResults>();
                            if (vitalsHBA1CList.Any(v => v.Results_Type == "Vitals") && vitalsHBA1CList.Any(v => v.Results_Type == "Results"))
                            {
                                IList<PatientResults> finalVital = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") && Hab.Captured_date_and_time == ((from Hab1 in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab1.Loinc_Observation.ToLower() == "hba1c status") select Hab1.Captured_date_and_time).Max()) select Hab).ToList<PatientResults>();
                                IList<PatientResults> finalLabResult = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") && Hab.Created_Date_And_Time == ((from Hab1 in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab1.Loinc_Observation.ToLower() == "hba1c status") select Hab1.Created_Date_And_Time).Max()) select Hab).ToList<PatientResults>();
                                if (finalVital !=null && finalVital.Count > 0 && finalLabResult != null && finalLabResult.Count>0 )
                                { 
                                if (finalVital[0].Captured_date_and_time > finalLabResult[0].Created_Date_And_Time)
                                {

                                    if (vitalsHBA1CList.Count > 0)
                                    {
                                        if (PreventivePlanLookupHead[3] != null)
                                        fillCareList.Preventive_Service = PreventivePlanLookupHead[3].Value;
                                        fillCareList.Preventive_Service_Value = "Most current HbA1c value in 2012 is <8.0";
                                        //if (hbalcstatus.Count() > 0)
                                        //{
                                        //    fillCareList.Status = hbalcstatus.First();
                                        //}
                                        //if (hbalcstatus.Count() == 0)
                                        //{
                                        //if (FinalVitalshba1cList[0].Value != string.Empty)
                                        //{
                                        //    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                        //    {
                                        //        fillCareList.Status = "No";

                                        //    }
                                        //    else
                                        //    {
                                        //        fillCareList.Status = "Yes";

                                        //    }
                                        //}
                                        //}
                                        if (hbalcNotes.Count() > 0)
                                        {
                                            if (hbalcNotes.First() != string.Empty)
                                            {
                                                if (finalVital != null && finalVital.Count == 1)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%" + "-" + hbalcNotes.First();
                                                    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "Yes";

                                                    }
                                                }
                                                if (finalVital != null && finalVital.Count > 1)
                                                {
                                                    finalVital = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") orderby Hab.Captured_date_and_time descending, Hab.Vitals_Group_ID descending select Hab).ToList<PatientResults>();
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%" + "-" + hbalcNotes.First();
                                                    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "Yes";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (finalVital[0] != null && finalVital[0].Value != string.Empty)
                                                {
                                                    if (finalVital.Count == 1)
                                                    {
                                                        fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%";
                                                        if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "Yes";

                                                        }
                                                    }
                                                    if (finalVital.Count > 1)
                                                    {
                                                        finalVital = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") orderby Hab.Captured_date_and_time descending, Hab.Vitals_Group_ID descending select Hab).ToList<PatientResults>();
                                                        fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%";
                                                        if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "Yes";

                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            if (finalVital[0] != null && finalVital[0].Value != string.Empty)
                                            {
                                                if (finalVital.Count == 1)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%";
                                                    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "Yes";

                                                    }
                                                }
                                                if (finalVital.Count > 1)
                                                {
                                                    finalVital = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") orderby Hab.Captured_date_and_time descending, Hab.Vitals_Group_ID descending select Hab).ToList<PatientResults>();
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%";
                                                    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "Yes";

                                                    }
                                                }
                                            }
                                        }

                                    }

                                }
                            }
                                if (finalVital !=null && finalVital.Count > 0 && finalLabResult != null && finalLabResult.Count>0 )
                                {

                                if (finalVital[0].Captured_date_and_time < finalLabResult[0].Created_Date_And_Time)
                                {

                                    if (vitalsHBA1CList.Count > 0)
                                    {
                                        if (PreventivePlanLookupHead[3] != null)
                                        fillCareList.Preventive_Service = PreventivePlanLookupHead[3].Value;
                                        fillCareList.Preventive_Service_Value = "Most current HbA1c value in 2012 is <8.0";
                                        //if (hbalcstatus.Count() > 0)
                                        //{
                                        //    fillCareList.Status = hbalcstatus.First();
                                        //}
                                        //if (hbalcstatus.Count() == 0)
                                        //{
                                        //if (FinalVitalshba1cList[0].Value != string.Empty)
                                        //{
                                        //    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                        //    {
                                        //        fillCareList.Status = "No";

                                        //    }
                                        //    else
                                        //    {
                                        //        fillCareList.Status = "Yes";

                                        //    }
                                        //}
                                        //}
                                        if (hbalcNotes.Count() > 0)
                                        {
                                            if (hbalcNotes.First() != string.Empty)
                                            {
                                                if (finalVital.Count == 1)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%" + "-" + hbalcNotes.First();
                                                    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "Yes";

                                                    }
                                                }
                                                if (finalVital.Count > 1)
                                                {
                                                    finalVital = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") orderby Hab.Captured_date_and_time descending, Hab.Vitals_Group_ID descending select Hab).ToList<PatientResults>();
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%" + "-" + hbalcNotes.First();
                                                    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "Yes";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (finalVital[0].Value != string.Empty)
                                                {
                                                    if (finalVital.Count == 1)
                                                    {
                                                        fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + "is " + finalVital[0].Value + "%";
                                                        if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "Yes";

                                                        }
                                                    }
                                                    if (finalVital.Count > 1)
                                                    {
                                                        finalVital = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") orderby Hab.Captured_date_and_time descending, Hab.Vitals_Group_ID descending select Hab).ToList<PatientResults>();
                                                        fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%";
                                                        if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "Yes";

                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            if (finalVital[0].Value != string.Empty)
                                            {
                                                if (finalVital.Count == 1)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + "is " + finalVital[0].Value + "%";
                                                    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "Yes";

                                                    }
                                                }
                                                if (finalVital.Count > 1)
                                                {
                                                    finalVital = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") orderby Hab.Captured_date_and_time descending, Hab.Vitals_Group_ID descending select Hab).ToList<PatientResults>();
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + finalVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalVital[0].Value + "%";
                                                    if (Convert.ToDecimal(finalVital[0].Value) >= Convert.ToDecimal(8.0))
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "Yes";

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
                                if (vitalsHBA1CList.Count > 0)
                                {
                                    if (vitalsHBA1CList[0].Results_Type.ToUpper() == "VITALS")
                                    {
                                        FinalVitalshba1cList = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") && Hab.Captured_date_and_time == ((from Hab1 in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab1.Loinc_Observation.ToLower() == "hba1c status") select Hab1.Captured_date_and_time).Max()) select Hab).ToList<PatientResults>();


                                        if (vitalsHBA1CList.Count > 0)
                                        {
                                            if (PreventivePlanLookupHead[3]!= null)
                                            fillCareList.Preventive_Service = PreventivePlanLookupHead[3].Value;
                                            fillCareList.Preventive_Service_Value = "Most current HbA1c value in 2012 is <8.0";
                                            //if (hbalcstatus.Count() > 0)
                                            //{
                                            //    fillCareList.Status = hbalcstatus.First();
                                            //}
                                            //if (hbalcstatus.Count() == 0)
                                            //{
                                            //if (FinalVitalshba1cList[0].Value != string.Empty)
                                            //{
                                            //    if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                            //    {
                                            //        fillCareList.Status = "No";

                                            //    }
                                            //    else
                                            //    {
                                            //        fillCareList.Status = "Yes";

                                            //    }
                                            //}
                                            //}
                                            if (hbalcNotes.Count() > 0)
                                            {
                                                if (hbalcNotes.First() != string.Empty)
                                                {
                                                    if (FinalVitalshba1cList.Count == 1)
                                                    {
                                                        fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + FinalVitalshba1cList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalshba1cList[0].Value + "%" + "-" + hbalcNotes.First();
                                                        if (FinalVitalshba1cList[0].Value != string.Empty)
                                                        {
                                                            if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "Yes";

                                                            }
                                                        }
                                                    }
                                                    if (FinalVitalshba1cList.Count > 1)
                                                    {
                                                        FinalVitalshba1cList = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") orderby Hab.Captured_date_and_time descending, Hab.Vitals_Group_ID descending select Hab).ToList<PatientResults>();
                                                        fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + FinalVitalshba1cList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalshba1cList[0].Value + "%" + "-" + hbalcNotes.First();
                                                        if (FinalVitalshba1cList[0].Value != string.Empty)
                                                        {
                                                            if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "Yes";

                                                            }
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    if (FinalVitalshba1cList[0].Value != string.Empty)
                                                    {
                                                        if (FinalVitalshba1cList.Count == 1)
                                                        {
                                                            fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + FinalVitalshba1cList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalshba1cList[0].Value + "%";
                                                            if (FinalVitalshba1cList[0].Value != string.Empty)
                                                            {
                                                                if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                                                {
                                                                    fillCareList.Status = "No";

                                                                }
                                                                else
                                                                {
                                                                    fillCareList.Status = "Yes";

                                                                }
                                                            }
                                                        }
                                                        if (FinalVitalshba1cList.Count > 1)
                                                        {
                                                            FinalVitalshba1cList = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") orderby Hab.Captured_date_and_time descending, Hab.Vitals_Group_ID descending select Hab).ToList<PatientResults>();
                                                            fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + FinalVitalshba1cList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalshba1cList[0].Value + "%";
                                                            if (FinalVitalshba1cList[0].Value != string.Empty)
                                                            {
                                                                if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                                                {
                                                                    fillCareList.Status = "No";

                                                                }
                                                                else
                                                                {
                                                                    fillCareList.Status = "Yes";

                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (FinalVitalshba1cList[0].Value != string.Empty)
                                                {
                                                    if (FinalVitalshba1cList.Count == 1)
                                                    {
                                                        fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + FinalVitalshba1cList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalshba1cList[0].Value + "%";
                                                        if (FinalVitalshba1cList[0].Value != string.Empty)
                                                        {
                                                            if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "Yes";

                                                            }
                                                        }
                                                    }
                                                    if (FinalVitalshba1cList.Count > 1)
                                                    {
                                                        FinalVitalshba1cList = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") orderby Hab.Captured_date_and_time descending, Hab.Vitals_Group_ID descending select Hab).ToList<PatientResults>();
                                                        fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + FinalVitalshba1cList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalshba1cList[0].Value + "%";
                                                        if (FinalVitalshba1cList[0].Value != string.Empty)
                                                        {
                                                            if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "Yes";

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    if (vitalsHBA1CList[0].Results_Type.ToUpper() == "RESULTS")
                                    {
                                        FinalVitalshba1cList = (from Hab in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab.Loinc_Observation.ToLower() == "hba1c status") && Hab.Created_Date_And_Time == ((from Hab1 in vitalsHBA1CList where (Hab.Loinc_Observation.ToUpper() == "HBA1C" || Hab1.Loinc_Observation.ToLower() == "hba1c status") select Hab1.Created_Date_And_Time).Max()) select Hab).ToList<PatientResults>();


                                        if (vitalsHBA1CList.Count > 0)
                                        {
                                            if (PreventivePlanLookupHead[3].Value != null)
                                            fillCareList.Preventive_Service = PreventivePlanLookupHead[3].Value;
                                            fillCareList.Preventive_Service_Value = "Most current HbA1c value in 2012 is <8.0";
                                            //if (hbalcstatus.Count() > 0)
                                            //{
                                            //    fillCareList.Status = hbalcstatus.First();
                                            //}
                                            //if (hbalcstatus.Count() == 0)
                                            //{
                                            //if (FinalVitalshba1cList[0].Value != string.Empty)
                                            //{
                                            //    if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                            //    {
                                            //        fillCareList.Status = "No";

                                            //    }
                                            //    else
                                            //    {
                                            //        fillCareList.Status = "Yes";

                                            //    }
                                            //}
                                            //}
                                            if (hbalcNotes.Count() > 0)
                                            {
                                                if (hbalcNotes.First() != string.Empty)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + FinalVitalshba1cList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalshba1cList[0].Value + "%" + "-" + hbalcNotes.First();
                                                    if (FinalVitalshba1cList[0].Value != string.Empty)
                                                    {
                                                        if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "Yes";

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (FinalVitalshba1cList[0].Value != string.Empty)
                                                        fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + FinalVitalshba1cList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalshba1cList[0].Value + "%";
                                                    if (FinalVitalshba1cList[0].Value != string.Empty)
                                                    {
                                                        if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "Yes";

                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (FinalVitalshba1cList[0].Value != string.Empty)
                                                    fillCareList.Preventive_Screening_Notes = "HbA1c value as on " + FinalVitalshba1cList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalshba1cList[0].Value + "%";
                                                if (FinalVitalshba1cList[0].Value != string.Empty)
                                                {
                                                    if (Convert.ToDecimal(FinalVitalshba1cList[0].Value) >= Convert.ToDecimal(8.0))
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "Yes";

                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }

                            if (fillCareList.Status != string.Empty || fillCareList.Preventive_Screening_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }

                        }
                        if (obj.Value == "Most current BP in 2012 is <140/90")
                        {
                            IList<PatientResults> FinalVitalsList = new List<PatientResults>();
                            IList<PatientResults> vitalsList = new List<PatientResults>();
                            ISQLQuery sql2 = iMySession.CreateSQLQuery("Select a.* from patient_results a where a.Encounter_ID = '" + ulEncounterID.ToString() + "'  and a.Loinc_Identifier='" + loincbp.First() + "'").AddEntity("a", typeof(PatientResults));
                            vitalsList = sql2.List<PatientResults>();

                            var bpNotes = from p in PrePlanValue where p.Preventive_Service_Value == "Most current BP in 2012 is <140/90" select p.Preventive_Screening_Notes;
                            //var bpstatus = from p in PrePlanValue where p.Preventive_Service_Value == "Most current BP in 2012 is <140/90" select p.Status;

                            if (vitalsList.Any(v => v.Results_Type == "Vitals") && vitalsList.Any(v => v.Results_Type == "Results"))
                            {
                                IList<PatientResults> finalBPVital = (from bp in vitalsList
                                                                      where (bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                          || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                          || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                          || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                          || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                          || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia") && bp.Captured_date_and_time == ((from bp1 in vitalsList
                                                                                                                                                                         where bp1.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                                                                                                                             || bp1.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                                                                                                                             || bp1.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                                                                                                                             || bp1.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                                                                                                                             || bp1.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                                                                                                                             || bp1.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                                                                                                                         select bp1.Captured_date_and_time).Max())
                                                                      select bp).ToList<PatientResults>();
                                IList<PatientResults> finalBPLabResult = (from bp in vitalsList
                                                                          where (bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                              || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                              || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                              || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                              || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                              || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia") && bp.Captured_date_and_time == ((from bp1 in vitalsList
                                                                                                                                                                             where bp1.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                                                                                                                                 || bp1.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                                                                                                                                 || bp1.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                                                                                                                                 || bp1.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                                                                                                                                 || bp1.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                                                                                                                                 || bp1.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                                                                                                                             select bp1.Captured_date_and_time).Max())
                                                                          select bp).ToList<PatientResults>();
                                if(finalBPVital != null && finalBPVital.Count>0 && finalBPLabResult != null && finalBPLabResult.Count >0)
                                {
                                if (finalBPVital[0].Captured_date_and_time > finalBPLabResult[0].Created_Date_And_Time)
                                {

                                    if (vitalsList.Count > 0)
                                    {
                                        if (PreventivePlanLookupHead[4] != null)
                                        fillCareList.Preventive_Service = PreventivePlanLookupHead[4].Value;
                                        fillCareList.Preventive_Service_Value = "Most current BP in 2012 is <140/90";
                                        //if (bpstatus.Count() > 0)
                                        //{
                                        //    fillCareList.Status = bpstatus.First();
                                        //}
                                        //if (bpstatus.Count() == 0)
                                        //{

                                        //string[] splitBp = finalBPVital[0].Value.Split('/');
                                        //if (splitBp[0] != string.Empty)
                                        //{
                                        //    if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                        //    {
                                        //        fillCareList.Status = "Yes";


                                        //    }
                                        //    else
                                        //    {
                                        //        fillCareList.Status = "No";

                                        //    }
                                        //}

                                        //}
                                        if (bpNotes.Count() > 0)
                                        {
                                            if (bpNotes.First() != string.Empty)
                                            {
                                                if (finalBPVital.Count == 1)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg" + "-" + bpNotes.First();
                                                    string[] splitBp = finalBPVital[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                    }
                                                }
                                                if (finalBPVital.Count > 1)
                                                {
                                                    finalBPVital = (from bp in vitalsList
                                                                    where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                    orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                                    select bp).ToList<PatientResults>();
                                                    fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg" + "-" + bpNotes.First();
                                                    string[] splitBp = finalBPVital[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (finalBPVital[0].Value != string.Empty)
                                                {
                                                    if (finalBPVital.Count == 1)
                                                    {
                                                        fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg";
                                                        string[] splitBp = finalBPVital[0].Value.Split('/');
                                                        if (splitBp[0] != string.Empty)
                                                        {
                                                            if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                            {
                                                                fillCareList.Status = "Yes";


                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                        }
                                                    }
                                                    if (finalBPVital.Count > 1)
                                                    {
                                                        finalBPVital = (from bp in vitalsList
                                                                        where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                        orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                                        select bp).ToList<PatientResults>();
                                                        fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg";
                                                        string[] splitBp = finalBPVital[0].Value.Split('/');
                                                        if (splitBp[0] != string.Empty)
                                                        {
                                                            if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                            {
                                                                fillCareList.Status = "Yes";


                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            if (finalBPVital[0].Value != string.Empty)
                                            {
                                                if (finalBPVital.Count == 1)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg";
                                                    string[] splitBp = finalBPVital[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                    }
                                                }
                                                if (finalBPVital.Count > 1)
                                                {
                                                    finalBPVital = (from bp in vitalsList
                                                                    where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                    orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                                    select bp).ToList<PatientResults>();
                                                    fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg";
                                                    string[] splitBp = finalBPVital[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                    }
                                                }
                                            }

                                        }

                                    }

                                }
                                if (finalBPVital[0].Captured_date_and_time < finalBPLabResult[0].Created_Date_And_Time)
                                {

                                    if (vitalsList.Count > 0)
                                    {
                                        if (PreventivePlanLookupHead[4] != null)
                                        fillCareList.Preventive_Service = PreventivePlanLookupHead[4].Value;
                                        fillCareList.Preventive_Service_Value = "Most current BP in 2012 is <140/90";
                                        //if (bpstatus.Count() > 0)
                                        //{
                                        //    fillCareList.Status = bpstatus.First();
                                        //}
                                        //if (bpstatus.Count() == 0)
                                        //{
                                        //string[] splitBp = finalBPVital[0].Value.Split('/');
                                        //if (splitBp[0] != string.Empty)
                                        //{
                                        //    if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                        //    {
                                        //        fillCareList.Status = "Yes";


                                        //    }
                                        //    else
                                        //    {
                                        //        fillCareList.Status = "No";

                                        //    }
                                        //}
                                        //}
                                        if (bpNotes.Count() > 0)
                                        {
                                            if (bpNotes.First() != string.Empty)
                                            {
                                                if (finalBPVital.Count == 1)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg" + "-" + bpNotes.First();
                                                    string[] splitBp = finalBPVital[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                    }
                                                }
                                                if (finalBPVital.Count > 1)
                                                {
                                                    finalBPVital = (from bp in vitalsList
                                                                    where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                    orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                                    select bp).ToList<PatientResults>();
                                                    fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg" + "-" + bpNotes.First();

                                                    string[] splitBp = finalBPVital[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (finalBPVital[0].Value != string.Empty)
                                                {
                                                    if (finalBPVital.Count == 1)
                                                    {
                                                        fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg";
                                                        string[] splitBp = finalBPVital[0].Value.Split('/');
                                                        if (splitBp[0] != string.Empty)
                                                        {
                                                            if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                            {
                                                                fillCareList.Status = "Yes";


                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                        }
                                                    }
                                                    if (finalBPVital.Count > 1)
                                                    {
                                                        finalBPVital = (from bp in vitalsList
                                                                        where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                            || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                        orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                                        select bp).ToList<PatientResults>();
                                                        fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg";
                                                        string[] splitBp = finalBPVital[0].Value.Split('/');
                                                        if (splitBp[0] != string.Empty)
                                                        {
                                                            if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                            {
                                                                fillCareList.Status = "Yes";


                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                        }
                                                    }
                                                }

                                            }

                                        }
                                        else
                                        {
                                            if (finalBPVital[0].Value != string.Empty)
                                            {
                                                if (finalBPVital.Count == 1)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg";
                                                    string[] splitBp = finalBPVital[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                    }
                                                }
                                                if (finalBPVital.Count > 1)
                                                {
                                                    finalBPVital = (from bp in vitalsList
                                                                    where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                        || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                    orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                                    select bp).ToList<PatientResults>();
                                                    fillCareList.Preventive_Screening_Notes = finalBPVital[0].Loinc_Observation + " as on " + finalBPVital[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + finalBPVital[0].Value + " mm/Hg";
                                                    string[] splitBp = finalBPVital[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

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
                                if (vitalsList.Count > 0)
                                {
                                    if (vitalsList[0].Results_Type.ToUpper() == "VITALS")
                                    {
                                        FinalVitalsList = (from bp in vitalsList
                                                           where (bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia") && bp.Captured_date_and_time == ((from bp1 in vitalsList
                                                                                                                                                              where bp1.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                                                                                                                  || bp1.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                                                                                                                  || bp1.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                                                                                                                  || bp1.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                                                                                                                  || bp1.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                                                                                                                  || bp1.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                                                                                                              select bp1.Captured_date_and_time).Max())
                                                           select bp).ToList<PatientResults>();


                                        if (FinalVitalsList.Count > 0)
                                        {
                                            if (PreventivePlanLookupHead[4] != null)
                                            fillCareList.Preventive_Service = PreventivePlanLookupHead[4].Value;
                                            fillCareList.Preventive_Service_Value = "Most current BP in 2012 is <140/90";
                                            //if (bpstatus.Count() > 0)
                                            //{
                                            //    fillCareList.Status = bpstatus.First();
                                            //}
                                            //if (bpstatus.Count() == 0)
                                            //{
                                            //string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                            //if (splitBp[0] != string.Empty)
                                            //{
                                            //    if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                            //    {
                                            //        fillCareList.Status = "Yes";


                                            //    }
                                            //    else
                                            //    {
                                            //        fillCareList.Status = "No";

                                            //    }
                                            //}
                                            //}
                                            if (bpNotes.Count() > 0)
                                            {
                                                if (bpNotes.First() != string.Empty)
                                                {
                                                    if (FinalVitalsList.Count == 1)
                                                    {
                                                        fillCareList.Preventive_Screening_Notes = FinalVitalsList[0].Loinc_Observation + " as on " + FinalVitalsList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalsList[0].Value + " mm/Hg" + "-" + bpNotes.First();
                                                        string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                                        if (splitBp[0] != string.Empty)
                                                        {
                                                            if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                            {
                                                                fillCareList.Status = "Yes";


                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                        }
                                                    }
                                                    if (FinalVitalsList.Count > 1)
                                                    {
                                                        FinalVitalsList = (from bp in vitalsList
                                                                           where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                           orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                                           select bp).ToList<PatientResults>();
                                                        fillCareList.Preventive_Screening_Notes = FinalVitalsList[0].Loinc_Observation + " as on " + FinalVitalsList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalsList[0].Value + " mm/Hg" + "-" + bpNotes.First();
                                                        string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                                        if (splitBp[0] != string.Empty)
                                                        {
                                                            if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                            {
                                                                fillCareList.Status = "Yes";


                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (FinalVitalsList[0].Value != string.Empty)
                                                    {
                                                        if (FinalVitalsList.Count == 1)
                                                        {
                                                            fillCareList.Preventive_Screening_Notes = FinalVitalsList[0].Loinc_Observation + " as on " + FinalVitalsList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalsList[0].Value + " mm/Hg";
                                                            string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                                            if (splitBp[0] != string.Empty)
                                                            {
                                                                if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                                {
                                                                    fillCareList.Status = "Yes";


                                                                }
                                                                else
                                                                {
                                                                    fillCareList.Status = "No";

                                                                }
                                                            }
                                                        }
                                                        if (FinalVitalsList.Count > 1)
                                                        {
                                                            FinalVitalsList = (from bp in vitalsList
                                                                               where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                                   || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                                   || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                                   || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                                   || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                                   || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                               orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                                               select bp).ToList<PatientResults>();
                                                            fillCareList.Preventive_Screening_Notes = FinalVitalsList[0].Loinc_Observation + " as on " + FinalVitalsList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalsList[0].Value + " mm/Hg";
                                                            string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                                            if (splitBp[0] != string.Empty)
                                                            {
                                                                if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                                {
                                                                    fillCareList.Status = "Yes";


                                                                }
                                                                else
                                                                {
                                                                    fillCareList.Status = "No";

                                                                }
                                                            }

                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (FinalVitalsList[0].Value != string.Empty)
                                                {
                                                    if (FinalVitalsList.Count == 1)
                                                    {
                                                        fillCareList.Preventive_Screening_Notes = FinalVitalsList[0].Loinc_Observation + " as on " + FinalVitalsList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalsList[0].Value + " mm/Hg";
                                                        string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                                        if (splitBp[0] != string.Empty)
                                                        {
                                                            if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                            {
                                                                fillCareList.Status = "Yes";


                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                        }
                                                    }
                                                    if (FinalVitalsList.Count > 1)
                                                    {
                                                        FinalVitalsList = (from bp in vitalsList
                                                                           where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                                               || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                                           orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                                           select bp).ToList<PatientResults>();
                                                        fillCareList.Preventive_Screening_Notes = FinalVitalsList[0].Loinc_Observation + " as on " + FinalVitalsList[0].Captured_date_and_time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalsList[0].Value + " mm/Hg";
                                                        string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                                        if (splitBp[0] != string.Empty)
                                                        {
                                                            if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                            {
                                                                fillCareList.Status = "Yes";


                                                            }
                                                            else
                                                            {
                                                                fillCareList.Status = "No";

                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }




                                    }
                                    if (vitalsList[0].Results_Type.ToUpper() == "RESULTS")
                                    {
                                        FinalVitalsList = (from bp in vitalsList
                                                           where bp.Loinc_Observation.ToLower() == "bp-sitting sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-lying sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-standing sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-sitting$ sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-lying$ sys/dia"
                                                               || bp.Loinc_Observation.ToLower() == "bp-standing$ sys/dia"
                                                           orderby bp.Captured_date_and_time descending, bp.Vitals_Group_ID descending
                                                           select bp).ToList<PatientResults>();


                                        if (FinalVitalsList.Count > 0)
                                        {
                                            if (PreventivePlanLookupHead[4] != null)
                                            fillCareList.Preventive_Service = PreventivePlanLookupHead[4].Value;
                                            fillCareList.Preventive_Service_Value = "Most current BP in 2012 is <140/90";
                                            //if (bpstatus.Count() > 0)
                                            //{
                                            //    fillCareList.Status = bpstatus.First();
                                            //}
                                            //if (bpstatus.Count() == 0)
                                            //{
                                            //string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                            //if (splitBp[0] != string.Empty)
                                            //{
                                            //    if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                            //    {
                                            //        fillCareList.Status = "Yes";


                                            //    }
                                            //    else
                                            //    {
                                            //        fillCareList.Status = "No";

                                            //    }
                                            //}
                                            //}
                                            if (bpNotes.Count() > 0)
                                            {
                                                if (bpNotes.First() != string.Empty)
                                                {
                                                    fillCareList.Preventive_Screening_Notes = FinalVitalsList[0].Loinc_Observation + " as on " + FinalVitalsList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalsList[0].Value + " mm/Hg" + "-" + bpNotes.First();
                                                    string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (FinalVitalsList[0].Value != string.Empty)
                                                        fillCareList.Preventive_Screening_Notes = FinalVitalsList[0].Loinc_Observation + " as on " + FinalVitalsList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalsList[0].Value + " mm/Hg";
                                                    string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                                    if (splitBp[0] != string.Empty)
                                                    {
                                                        if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                        {
                                                            fillCareList.Status = "Yes";


                                                        }
                                                        else
                                                        {
                                                            fillCareList.Status = "No";

                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (FinalVitalsList[0].Value != string.Empty)
                                                    fillCareList.Preventive_Screening_Notes = FinalVitalsList[0].Loinc_Observation + " as on " + FinalVitalsList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") + " is " + FinalVitalsList[0].Value + " mm/Hg";
                                                string[] splitBp = FinalVitalsList[0].Value.Split('/');
                                                if (splitBp[0] != string.Empty)
                                                {
                                                    if ((Convert.ToInt32(splitBp[0]) < 140) && (Convert.ToInt32(splitBp[1]) < 90))
                                                    {
                                                        fillCareList.Status = "Yes";


                                                    }
                                                    else
                                                    {
                                                        fillCareList.Status = "No";

                                                    }
                                                }
                                            }
                                        }


                                    }
                                }
                            }
                            if (fillCareList.Status != string.Empty || fillCareList.Preventive_Screening_Notes != string.Empty)
                            {
                                CareList.Add(fillCareList);
                            }

                        }
                    }
                    else
                    {
                        var query = from q in PrePlanValue where q.Preventive_Service_Value == obj.Value select q;
                        PrePlanFinal = query.ToList<PreventiveScreen>();
                        if (PrePlanFinal.Count > 0)
                        {
                            PreventiveScreen fillCareList = new PreventiveScreen();
                            fillCareList.Preventive_Service = PrePlanFinal[0].Preventive_Service;
                            fillCareList.Preventive_Service_Value = PrePlanFinal[0].Preventive_Service_Value;
                            fillCareList.Status = PrePlanFinal[0].Status;
                            fillCareList.Preventive_Screening_Notes = PrePlanFinal[0].Preventive_Screening_Notes;
                            CareList.Add(fillCareList);

                        }
                    }

                }
                iMySession.Close();
            }

            return CareList;
        }
        IList<TreatmentPlan> GetCSTreatmentPlan(ulong ulHumanID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<TreatmentPlan> treatmentPlanList = new List<TreatmentPlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSTreatmentPlan.List");
                //query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(0, ulHumanID);
                ArrayList arrayList = new ArrayList(query1.List());
                IList<TreatmentPlan> ilsttreatmentPlanList = new List<TreatmentPlan>();
                ulong Encounter = 0;
                if (arrayList.Count > 0)
                {
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        TreatmentPlan fillTreatmentPlan = new TreatmentPlan();
                        object[] ccObject = (object[])arrayList[i];
                        // object[] ccObject = (object[])arrayList[0];
                        if (ccObject[0] != null)
                        fillTreatmentPlan.Plan = ccObject[0].ToString();
                        if (ccObject[1] != null)
                        fillTreatmentPlan.Plan_Type = ccObject[1].ToString();
                        if (ccObject[2] != null)
                        fillTreatmentPlan.Addendum_Plan = ccObject[2].ToString();
                        if (ccObject[3] != null)
                        fillTreatmentPlan.Created_Date_And_Time = Convert.ToDateTime(ccObject[3].ToString());
                        if (ccObject[4] != null)
                        fillTreatmentPlan.Encounter_Id = Convert.ToUInt32(ccObject[4].ToString());
                        //treatmentPlanList.Add(fillTreatmentPlan);
                        ilsttreatmentPlanList.Add(fillTreatmentPlan);
                        if (i == 0)
                            Encounter = ilsttreatmentPlanList[0].Encounter_Id;
                    }
                    if (ilsttreatmentPlanList.Count > 0)
                    {
                        treatmentPlanList = (from p in ilsttreatmentPlanList where p.Encounter_Id == Encounter select p).ToList();
                    }
                }
                iMySession.Close();
            }
            return treatmentPlanList;
        }

        IList<TreatmentPlan> GetOtherTreatmentPlan(ulong ulEncounterID)
        {
            IList<TreatmentPlan> ilsttreatmentPlanList = new List<TreatmentPlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.OtherTreatmentPlan.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                if (arrayList.Count > 0)
                {
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        object[] ccObject = (object[])arrayList[i];
                        TreatmentPlan fillTreatmentPlan = new TreatmentPlan();
                        if (ccObject[0] != null)
                        fillTreatmentPlan.Plan = ccObject[0].ToString();
                        if (ccObject[1] != null)
                        fillTreatmentPlan.Plan_For_Plan = ccObject[1].ToString();
                        ilsttreatmentPlanList.Add(fillTreatmentPlan);
                    }
                   
                }
                iMySession.Close();
            }
            return ilsttreatmentPlanList;
        }

        IList<TreatmentPlan> GetTreatmentPlan(ulong ulEncounterID)
        {
            IList<TreatmentPlan> ilsttreatmentPlanList = new List<TreatmentPlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.TreatmentPlan.List");
                query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(1, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                if (arrayList.Count > 0)
                {
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        TreatmentPlan fillTreatmentPlan = new TreatmentPlan();
                        fillTreatmentPlan.Plan = arrayList[i].ToString();
                        ilsttreatmentPlanList.Add(fillTreatmentPlan);
                    }

                }
                iMySession.Close();
            }
            return ilsttreatmentPlanList;
        }

        IList<ProblemList> GetHealthConcernProblemList(ulong ulHumanID)
        {
            IList<ProblemList> ilstProblemList = new List<ProblemList>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.HealthConcernProblemList.List");
                query1.SetParameter(0, ulHumanID);
                ArrayList arrayList = new ArrayList(query1.List());
                if (arrayList.Count > 0)
                {
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        ProblemList fillProblemList = new ProblemList();
                        object[] ccObject = (object[])arrayList[i];
                        if (ccObject[0] != null)
                        fillProblemList.Problem_Description = ccObject[0].ToString();
                        if (ccObject[1] != null)
                        fillProblemList.Date_Diagnosed = ccObject[1].ToString();
                        if (ccObject[2] != null)
                        fillProblemList.ICD = ccObject[2].ToString();
                        if (ccObject[3] != null)
                        fillProblemList.ICD_9_Description = ccObject[3].ToString();//Assigned ICD ID value
                        //fillProblemList.Status = ccObject[4].ToString();
                        if (ccObject[4] != null)
                        {
                            if (ccObject[4].ToString().ToUpper() == "RESOLVED")
                            {
                                fillProblemList.Status = "Completed";
                            }
                            else
                                fillProblemList.Status = ccObject[4].ToString();
                        }
                        if (ccObject[5] != null)
                        {
                            fillProblemList.Snomed_Code = ccObject[5].ToString();

                            //ilstProblemList.Add(fillProblemList);
                        }
                        if (ccObject[6] != null)
                        {
                            fillProblemList.Snomed_Code_Description = ccObject[6].ToString();
                            //ilstProblemList.Add(fillProblemList);
                        }
                        ilstProblemList.Add(fillProblemList);
                    }


                }
                iMySession.Close();
            }
            return ilstProblemList;
        }
        public IList<Rcopia_Medication> GetCSEPrescriptionList(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<Rcopia_Medication> EPrescriptionList = new List<Rcopia_Medication>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSEPrescription.List");
                query1.SetParameter(0, getHumanID(ulEncounterID));
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Rcopia_Medication fillRcopiaMedication = new Rcopia_Medication();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillRcopiaMedication.Brand_Name = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillRcopiaMedication.Generic_Name = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillRcopiaMedication.Route = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillRcopiaMedication.Form = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillRcopiaMedication.Strength = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillRcopiaMedication.Start_Date = Convert.ToDateTime(ccObject[5].ToString());
                    if (ccObject[6] != null)
                    fillRcopiaMedication.Stop_Date = Convert.ToDateTime(ccObject[6].ToString());
                    if (ccObject[7] != null)
                    fillRcopiaMedication.Refills = ccObject[7].ToString();
                    if (ccObject[8] != null)
                    fillRcopiaMedication.Duration = ccObject[8].ToString();
                    if (ccObject[9] != null)
                    fillRcopiaMedication.Dose = ccObject[9].ToString();
                    if (ccObject[10] != null)
                    fillRcopiaMedication.Dose_Unit = ccObject[10].ToString();
                    if (ccObject[11] != null)
                    fillRcopiaMedication.Dose_Timing = ccObject[11].ToString();
                    EPrescriptionList.Add(fillRcopiaMedication);
                }
                iMySession.Close();
            }
            return EPrescriptionList;
        }
        public IList<FillLabOrder> GetCSLabOrderList(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<FillLabOrder> LabOrderList = new List<FillLabOrder>();
            //IQuery query1 = session.GetISession().GetNamedQuery("Fill.CSLabOrderList.List");
            //query1.SetParameter(0, ulEncounterID);
            // ArrayList arrayList = new ArrayList(query1.List());
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery InhouseQuery = iMySession.GetNamedQuery("Fill.InhouseProcedure.List");
                InhouseQuery.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(InhouseQuery.List());
                //for (int j = 0; j < InhouseList.Count; j++)
                //{
                //    arrayList.Add(InhouseList[j]);
                //}
                for (int i = 0; i < arrayList.Count; i++)
                {
                    FillLabOrder fillLabOrderList = new FillLabOrder();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    {
                        fillLabOrderList.labProcedure.Lab_Procedure = ccObject[0].ToString();
                        fillLabOrderList.labProcedureDescription.Lab_Procedure = ccObject[0].ToString();
                    }
                     if (ccObject[1] != null)
                        fillLabOrderList.labProcedureDescription.Lab_Procedure_Description = ccObject[1].ToString();
                    if (ccObject[2] != null)
                        fillLabOrderList.ordernotes.Specimen_Type = ccObject[2].ToString();
                    if (ccObject[3] != null)
                        fillLabOrderList.ordernotes.Order_Notes = ccObject[3].ToString();
                    if (ccObject[4] != null)
                        fillLabOrderList.labName = ccObject[4].ToString();
                    if (ccObject[5] != null)
                        fillLabOrderList.labLocation = ccObject[5].ToString();
                    if (ccObject[6] != null && ccObject[6] != "")
                    {
                        fillLabOrderList.OrderID.Order_Submit_ID = Convert.ToUInt16(ccObject[6].ToString());
                    }
                    if (ccObject[7] != null)
                        fillLabOrderList.ICD.ICD = ccObject[7].ToString();
                    if (ccObject[8] != null)
                        fillLabOrderList.ICDDescription.ICD_Description = ccObject[8].ToString();
                    if (ccObject[9] != null && ccObject[9] != "")
                        fillLabOrderList.labProcedureDescription.Created_Date_And_Time = Convert.ToDateTime(ccObject[9].ToString());
                    if (ccObject[10] != null)
                        fillLabOrderList.labProcedureDescription.Physician_ID = Convert.ToUInt32(ccObject[10].ToString());
                    LabOrderList.Add(fillLabOrderList);

                }
                iMySession.Close();
            }
            return LabOrderList;
        }
        public IList<FillLabOrder> GetCSImageOrderList(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<FillLabOrder> ImageOrderList = new List<FillLabOrder>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSImageOrderList.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    FillLabOrder fillImageOrderList = new FillLabOrder();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                        fillImageOrderList.labProcedure.Lab_Procedure = ccObject[0].ToString();
                    if (ccObject[1] != null)
                        fillImageOrderList.labProcedureDescription.Lab_Procedure_Description = ccObject[1].ToString();
                    if (ccObject[2] != null)
                        fillImageOrderList.ordernotes.Specimen_Type = ccObject[2].ToString();
                    if (ccObject[3] != null)
                        fillImageOrderList.ordernotes.Order_Notes = ccObject[3].ToString();
                    if (ccObject[4] != null)
                        fillImageOrderList.labName = ccObject[4].ToString();
                    if (ccObject[5] != null)
                        fillImageOrderList.labLocation = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    {
                        fillImageOrderList.OrderID.Order_Submit_ID = Convert.ToUInt16(ccObject[6].ToString());
                    } 
                    if (ccObject[7] != null)
                        fillImageOrderList.ICD.ICD = ccObject[7].ToString();
                    if (ccObject[8] != null)
                        fillImageOrderList.ICDDescription.ICD_Description = ccObject[8].ToString();
                    ImageOrderList.Add(fillImageOrderList);

                }
                iMySession.Close();
            }
            return ImageOrderList;
        }
        //<<<<<<< ChiefComplaintsManager.cs
        //        public IList<FillReferralOrder> GetCSReferralOrderList(ulong ulEncounterID)
        //        {
        //            IList<FillReferralOrder> ReferralOrderList = new List<FillReferralOrder>();
        //            IQuery query1 = session.GetISession().GetNamedQuery("Fill.CSReferralOrderList.List");
        //            query1.SetParameter(0, ulEncounterID);
        //            ArrayList arrayList = new ArrayList(query1.List());
        //            for (int i = 0; i < arrayList.Count; i++)
        //            {
        //               FillReferralOrder fillReferralOrderList = new FillReferralOrder();
        //                object[] ccObject = (object[])arrayList[i];
        //                fillReferralOrderList.tophyName.To_Physician_Name = ccObject[0].ToString();
        //                fillReferralOrderList.referralSpecialty.Referral_Specialty= ccObject[1].ToString();
        //                fillReferralOrderList.toFacName.To_Facility_Name = ccObject[2].ToString();
        //                fillReferralOrderList.reasonforReferral.Reason_For_Referral = ccObject[3].ToString();
        //                fillReferralOrderList.noOfVisit.Number_of_Visit = Convert.ToInt32(ccObject[4].ToString());
        //                fillReferralOrderList.referralNotes.Referral_Notes = ccObject[5].ToString();
        //                fillReferralOrderList.ICD.ICD = ccObject[6].ToString();
        //                fillReferralOrderList.assessmentDescription.Assessment_Description = ccObject[7].ToString();
        //                ReferralOrderList.Add(fillReferralOrderList);

        //            }
        //            return ReferralOrderList;
        //        }
        //||||||| 1.69
        //=======

        public IList<FillReferralOrder> GetCSReferralOrderList(ulong ulEncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<FillReferralOrder> ReferralOrderList = new List<FillReferralOrder>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSReferralOrderList.List");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    FillReferralOrder fillReferralOrderList = new FillReferralOrder();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillReferralOrderList.tophyName.To_Physician_Name = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillReferralOrderList.referralSpecialty.Referral_Specialty = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillReferralOrderList.toFacName.To_Facility_Name = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillReferralOrderList.reasonforReferral.Reason_For_Referral = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillReferralOrderList.noOfVisit.Number_of_Visit = Convert.ToInt32(ccObject[4].ToString());
                    if (ccObject[5] != null)
                    fillReferralOrderList.referralNotes.Referral_Notes = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    fillReferralOrderList.ICD.ICD = ccObject[6].ToString();
                    if (ccObject[7] != null)
                    fillReferralOrderList.assessmentDescription.Assessment_Description = ccObject[7].ToString();
                    ReferralOrderList.Add(fillReferralOrderList);

                }
                iMySession.Close();
            }
            return ReferralOrderList;
        }
        public IList<Encounter> GetCSEncounterDetails(ulong ulEncounterID)
        {
            IList<Encounter> EncList = new List<Encounter>();
            EncounterManager encMngr = new EncounterManager();
           // EncList = encMngr.GetEncounterByEncounterID(ulEncounterID);
            EncList = encMngr.GetEncounterByEncounterIDIncludeArchive(ulEncounterID);            
            return EncList;

        }
        public IList<PatientInsuredPlan> GetCSPatInsPlan(ulong ulHumanID)
        {
            IList<PatientInsuredPlan> PatInsPlanList = new List<PatientInsuredPlan>();
            PatientInsuredPlanManager PatInsPlaMngr = new PatientInsuredPlanManager();
            PatInsPlanList = PatInsPlaMngr.getInsurancePoliciesByHumanId(ulHumanID);
            return PatInsPlanList;

        }
        //public IList<FillAddendumNotes> GetCSAddendumNotesList(ulong ulEncounterID)
        //{
        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    IList<FillAddendumNotes> AddendumNotesList = new List<FillAddendumNotes>();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        IQuery query1 = iMySession.GetNamedQuery("Fill.CSAddendumNotesList.List");
        //        query1.SetParameter(0, ulEncounterID);

        //        ArrayList arrayList = new ArrayList(query1.List());

        //        for (int i = 0; i < arrayList.Count; i++)
        //        {
        //            FillAddendumNotes fillAddendumNotes = new FillAddendumNotes();
        //            object[] ccObject = (object[])arrayList[i];
        //            if (ccObject[0] != null)
        //            fillAddendumNotes.Createdby = ccObject[0].ToString();
        //            if (ccObject[1] != null)
        //            fillAddendumNotes.Notes = ccObject[1].ToString();
        //            AddendumNotesList.Add(fillAddendumNotes);

        //        }
        //        iMySession.Close();
        //    }
        //    return AddendumNotesList;



        //}
        public void SaveExamination(IList<Examination> examSaveList, ulong EncounterID, ulong PhysicianID, string UserName)
        {
            Examination objExamination;
            ExaminationManager objExamMngr = new ExaminationManager();
            IList<Examination> ExamSaveList = new List<Examination>();
            for (int i = 0; i < examSaveList.Count; i++)
            {
                objExamination = new Examination();
                objExamination.Encounter_ID = EncounterID;
                objExamination.Human_ID = examSaveList[i].Human_ID;
                objExamination.System_Name = examSaveList[i].System_Name;
                objExamination.Physician_ID = PhysicianID;
                objExamination.Exam_Lookup_Id = examSaveList[i].Exam_Lookup_Id;
                objExamination.Condition_Name = examSaveList[i].Condition_Name;
                objExamination.Status = examSaveList[i].Status;
                objExamination.Category = examSaveList[i].Category;
                objExamination.Examination_Notes = examSaveList[i].Examination_Notes;
                objExamination.Short_Description = examSaveList[i].Short_Description;
                objExamination.Created_By = UserName;
                objExamination.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
                objExamination.Modified_By = UserName;
                objExamination.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
                ExamSaveList.Add(objExamination);
            }
            IList<Examination> ExamSaveListnull = null;
            //objExamMngr.SaveUpdateDeleteWithTransaction(ref ExamSaveList, null, null, string.Empty);
            objExamMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref ExamSaveList, ref ExamSaveListnull, null, string.Empty, false, false, 0, string.Empty);

        }

        //public void GetFollowUpEncounter(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID,
        //string Current_UserName, ulong PreviousEnc, string sRole)
        //{

        //    //string constr = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        //    //using (MySqlConnection con = new MySqlConnection(constr))
        //    //{
        //    //    using (MySqlCommand cmd = new MySqlCommand("CopyPreviousProcedure", con))
        //    //    {
        //    //        cmd.CommandType = CommandType.StoredProcedure;
        //    //        cmd.Parameters.AddWithValue("humanid", uHumanID);
        //    //        cmd.Parameters.AddWithValue("PreviousEncouter", PreviousEnc);
        //    //        cmd.Parameters.AddWithValue("CurrentEncounter", ulEncounterID);
        //    //        cmd.Parameters.AddWithValue("CurrentPhysician", uPhysicianID);
        //    //        cmd.Parameters.AddWithValue("Currentuser", Current_UserName);
        //    //        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
        //    //        {
        //    //            con.Open();
        //    //            cmd.ExecuteNonQuery();
        //    //            //if (dt.Rows[0].ItemArray.Count() > 0)
        //    //            //    sarr = dt.Rows[0].ItemArray[0].ToString().Split('$');
        //    //        }
        //    //    }
        //    //    con.Close();
        //    //}

        //    #region Old_Codes
        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();

        //    //IList<ulong> ulmyEncounter = new List<ulong>();
        //    //ulmyEncounter.Add(ulEncounterID);
        //    //ulmyEncounter.Add(PreviousEnc);

        //    //#region ChiefComplaints
        //    //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    //{
        //    //    ICriteria CC_result1 = iMySession.CreateCriteria(typeof(ChiefComplaints))
        //    //                                      .Add(Expression.In("Encounter_ID", ulmyEncounter.ToArray()))
        //    //                                      .AddOrder(Order.Desc("Modified_Date_And_Time"));
        //    //    IList<ChiefComplaints> ccPhraseList = CC_result1.List<ChiefComplaints>();


        //    //    if (ccPhraseList.Where(a => a.Encounter_ID == ulEncounterID).Count() == 0)
        //    //    {
        //    //        IList<ChiefComplaints> PrevCCPhraseList = ccPhraseList.Where(a => a.Encounter_ID == PreviousEnc).ToList<ChiefComplaints>();
        //    //        if (PrevCCPhraseList.Count > 0)
        //    //        {
        //    //            ChiefComplaints ObjccPhrase;
        //    //            IList<ChiefComplaints> CCSaveList = new List<ChiefComplaints>();
        //    //            for (int i = 0; i < PrevCCPhraseList.Count; i++)
        //    //            {
        //    //                ObjccPhrase = new ChiefComplaints();
        //    //                ObjccPhrase.Encounter_ID = ulEncounterID;
        //    //                ObjccPhrase.Human_ID = PrevCCPhraseList[i].Human_ID;
        //    //                ObjccPhrase.Physician_ID = uPhysicianID;
        //    //                ObjccPhrase.HPI_Element = PrevCCPhraseList[i].HPI_Element;
        //    //                ObjccPhrase.HPI_Value = PrevCCPhraseList[i].HPI_Value;
        //    //                ObjccPhrase.Created_By = Current_UserName;
        //    //                ObjccPhrase.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                ObjccPhrase.Modified_By = Current_UserName;
        //    //                ObjccPhrase.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                CCSaveList.Add(ObjccPhrase);

        //    //            }

        //    //            SaveUpdateDeleteWithTransaction(ref CCSaveList, null, null, string.Empty);
        //    //        }

        //    //    }

        //    //#endregion

        //    //    IList<string> sParentField = new List<string>();
        //    //    sParentField.Add("System");
        //    //    sParentField.Add("ROS General Notes");
        //    //    sParentField.Add("Selected Assessment");
        //    //    sParentField.Add("Ruled Out Assessment");
        //    //    sParentField.Add("Non Drug Allergy History");
        //    //    ICriteria ros_GeneralNotes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.In("Encounter_ID", ulmyEncounter.ToArray())).Add(Expression.In("Parent_Field", sParentField.ToArray()));
        //    //    IList<GeneralNotes> RosGeneralNotes = ros_GeneralNotes.List<GeneralNotes>();

        //    //    #region ROS
        //    //    ICriteria ros_result = iMySession.CreateCriteria(typeof(ROS)).Add(Expression.In("Encounter_Id", ulmyEncounter.ToArray())).Add(Expression.Eq("Human_ID", uHumanID));
        //    //    IList<ROS> RosList = ros_result.List<ROS>();



        //    //    if (RosList.Where(a => a.Encounter_Id == ulEncounterID).Count() == 0)
        //    //    {
        //    //        IList<ROS> PrevRosList = RosList.Where(a => a.Encounter_Id == PreviousEnc).ToList<ROS>();
        //    //        if (PrevRosList.Count > 0)
        //    //        {
        //    //            ROS ObjRos;
        //    //            IList<ROS> RosSaveList = new List<ROS>();
        //    //            for (int i = 0; i < PrevRosList.Count; i++)
        //    //            {
        //    //                ObjRos = new ROS();
        //    //                ObjRos.Encounter_Id = ulEncounterID;
        //    //                ObjRos.Human_ID = PrevRosList[i].Human_ID;
        //    //                ObjRos.Physician_Id = uPhysicianID;
        //    //                ObjRos.System_Name = PrevRosList[i].System_Name;
        //    //                ObjRos.Symptom_Name = PrevRosList[i].Symptom_Name;
        //    //                ObjRos.Status = PrevRosList[i].Status;
        //    //                ObjRos.Created_By = Current_UserName;
        //    //                ObjRos.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                ObjRos.Modified_By = Current_UserName;
        //    //                ObjRos.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                RosSaveList.Add(ObjRos);
        //    //            }

        //    //            IList<GeneralNotes> saveRosGeneralNotes = RosGeneralNotes.Where(a => a.Encounter_ID == PreviousEnc && a.Parent_Field.ToUpper() == "SYSTEM").ToList<GeneralNotes>();
        //    //            GeneralNotes objsystemGeneralNotes = new GeneralNotes();
        //    //            IList<GeneralNotes> saveSystemGeneralNotes = new List<GeneralNotes>();
        //    //            for (int i = 0; i < saveRosGeneralNotes.Count; i++)
        //    //            {
        //    //                objsystemGeneralNotes = new GeneralNotes();
        //    //                objsystemGeneralNotes.Encounter_ID = ulEncounterID;
        //    //                objsystemGeneralNotes.Human_ID = saveRosGeneralNotes[i].Human_ID;
        //    //                objsystemGeneralNotes.Parent_Field = saveRosGeneralNotes[i].Parent_Field;
        //    //                objsystemGeneralNotes.Name_Of_The_Field = saveRosGeneralNotes[i].Name_Of_The_Field;
        //    //                objsystemGeneralNotes.Notes = saveRosGeneralNotes[i].Notes;
        //    //                objsystemGeneralNotes.Created_By = Current_UserName;
        //    //                objsystemGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                objsystemGeneralNotes.Modified_By = Current_UserName;
        //    //                objsystemGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                saveSystemGeneralNotes.Add(objsystemGeneralNotes);
        //    //            }
        //    //            IList<GeneralNotes> saveNotesGeneralNotes = RosGeneralNotes.Where(a => a.Encounter_ID == PreviousEnc && a.Parent_Field.ToUpper() == "ROS GENERAL NOTES").ToList<GeneralNotes>();
        //    //            IList<GeneralNotes> saveParentROSGeneralNotes = new List<GeneralNotes>();
        //    //            for (int i = 0; i < saveNotesGeneralNotes.Count; i++)
        //    //            {
        //    //                objsystemGeneralNotes = new GeneralNotes();
        //    //                objsystemGeneralNotes.Encounter_ID = ulEncounterID;
        //    //                objsystemGeneralNotes.Human_ID = saveNotesGeneralNotes[i].Human_ID;
        //    //                objsystemGeneralNotes.Parent_Field = saveNotesGeneralNotes[i].Parent_Field;
        //    //                objsystemGeneralNotes.Name_Of_The_Field = saveNotesGeneralNotes[i].Name_Of_The_Field;
        //    //                objsystemGeneralNotes.Notes = saveNotesGeneralNotes[i].Notes;
        //    //                objsystemGeneralNotes.Created_By = Current_UserName;
        //    //                objsystemGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                objsystemGeneralNotes.Modified_By = Current_UserName;
        //    //                objsystemGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //            }

        //    //            ROSManager rosmanager = new ROSManager();
        //    //            IList<ROS> RosUpdateList = new List<ROS>();
        //    //            IList<GeneralNotes> GeneralNotesUpdateList = new List<GeneralNotes>();
        //    //            rosmanager.BatchOperationsinCopyPreviousEncounterToRosAndGeneralNotes(RosSaveList, RosUpdateList, saveSystemGeneralNotes, GeneralNotesUpdateList, objsystemGeneralNotes, ulEncounterID, string.Empty);
        //    //        }
        //    //    }

        //    //    #endregion

        //    //    #region Examination
        //    //    if (sRole.ToUpper() == "PHYSICIAN" || sRole.ToUpper() == "PHYSICIAN ASSISTANT")
        //    //    {
        //    //        IList<string> examCategory = new List<string>();
        //    //        examCategory.Add("General With Specialty");
        //    //        examCategory.Add("Focused");
        //    //        ICriteria CategoryList = iMySession.CreateCriteria(typeof(Examination))
        //    //                                                                     .Add(Expression.In("Encounter_ID", ulmyEncounter.ToArray()))
        //    //                                                                     .Add(Expression.Eq("Human_ID", uHumanID))
        //    //                                                                     .Add(Expression.In("Category", examCategory.ToArray()));

        //    //        IList<Examination> examCategoryList = CategoryList.List<Examination>();


        //    //        IList<Examination> GeneralExamCategoryList = examCategoryList.Where(a => a.Category == "General With Specialty").ToList<Examination>();

        //    //        if (GeneralExamCategoryList.Where(a => a.Encounter_ID == ulEncounterID).Count() == 0)
        //    //        {
        //    //            IList<Examination> PrevExamList = GeneralExamCategoryList.Where(a => a.Encounter_ID == PreviousEnc).ToList<Examination>();
        //    //            if (PrevExamList.Count > 0)
        //    //                SaveExamination(PrevExamList, ulEncounterID, uPhysicianID, Current_UserName);
        //    //        }

        //    //        IList<Examination> FocusedExamCategoryList = examCategoryList.Where(a => a.Category == "Focused").ToList<Examination>();

        //    //        if (FocusedExamCategoryList.Where(a => a.Encounter_ID == ulEncounterID).Count() == 0)
        //    //        {
        //    //            IList<Examination> PrevFocusedExamList = FocusedExamCategoryList.Where(a => a.Encounter_ID == PreviousEnc).ToList<Examination>();
        //    //            if (PrevFocusedExamList.Count > 0)
        //    //                SaveExamination(PrevFocusedExamList, ulEncounterID, uPhysicianID, Current_UserName);
        //    //        }
        //    //    }
        //    //    #endregion


        //    //    IList<string> assessmentType = new List<string>();
        //    //    assessmentType.Add("Selected");
        //    //    assessmentType.Add("Ruled Out");

        //    //    IList<TreatmentPlan> FinalTreatmentList;
        //    //    IList<string> sPlan_Type = new List<string>();
        //    //    sPlan_Type.Add("ASSESSMENT");
        //    //    sPlan_Type.Add("PLAN");
        //    //    ICriteria Treatment_Result = iMySession.CreateCriteria(typeof(TreatmentPlan))
        //    //                      .Add(Expression.In("Encounter_Id", ulmyEncounter.ToArray()))
        //    //                      .Add(Expression.Eq("Human_ID", uHumanID))
        //    //                      .Add(Expression.In("Plan_Type", sPlan_Type.ToArray()));
        //    //    FinalTreatmentList = Treatment_Result.List<TreatmentPlan>();

        //    //    #region Assessment
        //    //    if (sRole.ToUpper() == "PHYSICIAN" || sRole.ToUpper() == "PHYSICIAN ASSISTANT")
        //    //    {

        //    //        ICriteria critAssessmentList = iMySession.CreateCriteria(typeof(Assessment))
        //    //                       .Add(Expression.In("Encounter_ID", ulmyEncounter.ToArray()))
        //    //                       .Add(Expression.Eq("Human_ID", uHumanID))
        //    //                       .Add(Expression.In("Assessment_Type", assessmentType.ToArray()));

        //    //        IList<Assessment> ResultAssessmentList = critAssessmentList.List<Assessment>();

        //    //        IList<Assessment> SelectedAssessmentList = ResultAssessmentList.Where(a => a.Assessment_Type == "Selected").ToList<Assessment>();

        //    //        if (SelectedAssessmentList.Where(a => a.Encounter_ID == ulEncounterID).Count() == 0)
        //    //        {
        //    //            IList<Assessment> PrevSelectedAssessmentList = SelectedAssessmentList.Where(a => a.Encounter_ID == PreviousEnc).ToList<Assessment>();
        //    //            if (PrevSelectedAssessmentList.Count > 0)
        //    //            {
        //    //                Assessment objAssessment;
        //    //                IList<Assessment> AssessmentSaveList = new List<Assessment>();
        //    //                for (int i = 0; i < PrevSelectedAssessmentList.Count; i++)
        //    //                {
        //    //                    objAssessment = new Assessment();
        //    //                    objAssessment.Encounter_ID = ulEncounterID;
        //    //                    objAssessment.Human_ID = PrevSelectedAssessmentList[i].Human_ID;
        //    //                    objAssessment.Physician_ID = uPhysicianID;
        //    //                    objAssessment.ICD_9 = PrevSelectedAssessmentList[i].ICD_9;
        //    //                    objAssessment.Description = PrevSelectedAssessmentList[i].Description;
        //    //                    objAssessment.Primary_Diagnosis = PrevSelectedAssessmentList[i].Primary_Diagnosis;
        //    //                    objAssessment.Diagnosis_Source = PrevSelectedAssessmentList[i].Diagnosis_Source;
        //    //                    objAssessment.Chronic_Problem = PrevSelectedAssessmentList[i].Chronic_Problem;
        //    //                    objAssessment.Assessment_Type = PrevSelectedAssessmentList[i].Assessment_Type;
        //    //                    objAssessment.Status = PrevSelectedAssessmentList[i].Status;
        //    //                    objAssessment.Assessment_Notes = PrevSelectedAssessmentList[i].Assessment_Notes;
        //    //                    objAssessment.Created_By = Current_UserName;
        //    //                    objAssessment.Created_Date = DateTime.Now.ToUniversalTime();
        //    //                    objAssessment.Modified_By = Current_UserName;
        //    //                    objAssessment.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    AssessmentSaveList.Add(objAssessment);

        //    //                }


        //    //                ICriteria Problem_Result = iMySession.CreateCriteria(typeof(ProblemList))
        //    //                                  .Add(Expression.Eq("Encounter_ID", PreviousEnc))
        //    //                                  .Add(Expression.Eq("Human_ID", uHumanID))
        //    //                                  .Add(Expression.Eq("Reference_Source", "Assessment"));
        //    //                IList<ProblemList> ResultProblemList = Problem_Result.List<ProblemList>();
        //    //                ProblemList objProblemList;
        //    //                IList<ProblemList> ProblemSaveList = new List<ProblemList>();
        //    //                for (int i = 0; i < ResultProblemList.Count; i++)
        //    //                {
        //    //                    objProblemList = new ProblemList();
        //    //                    objProblemList.Encounter_ID = ulEncounterID;
        //    //                    objProblemList.Human_ID = ResultProblemList[i].Human_ID;
        //    //                    objProblemList.Physician_ID = uPhysicianID;
        //    //                    objProblemList.Reference_Source = ResultProblemList[i].Reference_Source;
        //    //                    objProblemList.ICD_Code = ResultProblemList[i].ICD_Code;
        //    //                    objProblemList.Problem_Description = ResultProblemList[i].Problem_Description;
        //    //                    objProblemList.Status = ResultProblemList[i].Status;
        //    //                    objProblemList.Date_Diagnosed = ResultProblemList[i].Date_Diagnosed;
        //    //                    objProblemList.Rcopia_ID = ResultProblemList[i].Rcopia_ID;
        //    //                    objProblemList.Last_Modified_By = ResultProblemList[i].Modified_By;
        //    //                    objProblemList.Last_Modified_Date = ResultProblemList[i].Modified_Date_And_Time;
        //    //                    objProblemList.Is_Active = ResultProblemList[i].Is_Active;
        //    //                    objProblemList.Created_By = Current_UserName;
        //    //                    objProblemList.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    objProblemList.Modified_By = Current_UserName;
        //    //                    objProblemList.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    ProblemSaveList.Add(objProblemList);
        //    //                }


        //    //                IList<TreatmentPlan> Ass_TPlan_result = FinalTreatmentList.Where(a => a.Plan_Type == "ASSESSMENT" && a.Encounter_Id == PreviousEnc).ToList<TreatmentPlan>();
        //    //                TreatmentPlan objTreatmentPlanAssessment = new TreatmentPlan();
        //    //                if (Ass_TPlan_result.Count != 0)
        //    //                {
        //    //                    for (int i = 0; i < Ass_TPlan_result.Count; i++)
        //    //                    {
        //    //                        objTreatmentPlanAssessment.Encounter_Id = ulEncounterID;
        //    //                        objTreatmentPlanAssessment.Human_ID = Ass_TPlan_result[i].Human_ID;
        //    //                        objTreatmentPlanAssessment.Physician_Id = uPhysicianID;
        //    //                        objTreatmentPlanAssessment.Plan = Ass_TPlan_result[i].Plan;
        //    //                        objTreatmentPlanAssessment.Addendum_Plan = Ass_TPlan_result[i].Addendum_Plan;
        //    //                        objTreatmentPlanAssessment.Plan_Type = Ass_TPlan_result[i].Plan_Type;
        //    //                        objTreatmentPlanAssessment.Created_By = Current_UserName;
        //    //                        objTreatmentPlanAssessment.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                        objTreatmentPlanAssessment.Modified_By = Current_UserName;
        //    //                        objTreatmentPlanAssessment.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    }
        //    //                }

        //    //                IList<GeneralNotes> AssessmentGeneralNotes = RosGeneralNotes.Where(a => a.Encounter_ID == PreviousEnc && a.Parent_Field == "Selected Assessment").ToList<GeneralNotes>();
        //    //                GeneralNotes objGeneralNotes = new GeneralNotes();
        //    //                for (int i = 0; i < AssessmentGeneralNotes.Count; i++)
        //    //                {
        //    //                    objGeneralNotes.Encounter_ID = ulEncounterID;
        //    //                    objGeneralNotes.Human_ID = AssessmentGeneralNotes[i].Human_ID;
        //    //                    objGeneralNotes.Parent_Field = AssessmentGeneralNotes[i].Parent_Field;
        //    //                    objGeneralNotes.Name_Of_The_Field = AssessmentGeneralNotes[i].Name_Of_The_Field;
        //    //                    objGeneralNotes.Notes = AssessmentGeneralNotes[i].Notes;
        //    //                    objGeneralNotes.Created_By = Current_UserName;
        //    //                    objGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    objGeneralNotes.Modified_By = Current_UserName;
        //    //                    objGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                }

        //    //                IList<Assessment> AssessmentUpdateList = new List<Assessment>();
        //    //                IList<Assessment> AssessmentDeleteList = new List<Assessment>();
        //    //                IList<ProblemList> ProblemUpdateList = new List<ProblemList>();
        //    //                IList<ProblemList> ProblemDeleteList = new List<ProblemList>();
        //    //                AssessmentManager objAssessmentManager = new AssessmentManager();
        //    //                objAssessmentManager.BatchOperationsinCopyPreviousEncounterToAssessment(AssessmentSaveList, AssessmentUpdateList, AssessmentDeleteList, ProblemSaveList, ProblemUpdateList, ProblemDeleteList, string.Empty, objGeneralNotes, objTreatmentPlanAssessment, Current_UserName);
        //    //            }
        //    //        }

        //    //    #endregion

        //    //        #region AssessmentRuledOut
        //    //        IList<Assessment> RuledOutAssessmentList = ResultAssessmentList.Where(a => a.Assessment_Type == "Ruled Out").ToList<Assessment>();

        //    //        if (RuledOutAssessmentList.Where(a => a.Encounter_ID == ulEncounterID).Count() == 0)
        //    //        {
        //    //            IList<Assessment> PrevRuledOutAssessmentList = RuledOutAssessmentList.Where(a => a.Encounter_ID == PreviousEnc).ToList<Assessment>();
        //    //            if (PrevRuledOutAssessmentList.Count > 0)
        //    //            {
        //    //                IList<Assessment> AssessmentRuledSaveList = new List<Assessment>();
        //    //                Assessment objAssessmentRuled;
        //    //                for (int i = 0; i < PrevRuledOutAssessmentList.Count; i++)
        //    //                {
        //    //                    objAssessmentRuled = new Assessment();
        //    //                    objAssessmentRuled.Encounter_ID = ulEncounterID;
        //    //                    objAssessmentRuled.Human_ID = PrevRuledOutAssessmentList[i].Human_ID;
        //    //                    objAssessmentRuled.Physician_ID = uPhysicianID;
        //    //                    objAssessmentRuled.ICD_9 = PrevRuledOutAssessmentList[i].ICD_9;
        //    //                    objAssessmentRuled.Description = PrevRuledOutAssessmentList[i].Description;
        //    //                    objAssessmentRuled.Primary_Diagnosis = PrevRuledOutAssessmentList[i].Primary_Diagnosis;
        //    //                    objAssessmentRuled.Diagnosis_Source = PrevRuledOutAssessmentList[i].Diagnosis_Source;
        //    //                    objAssessmentRuled.Chronic_Problem = PrevRuledOutAssessmentList[i].Chronic_Problem;
        //    //                    objAssessmentRuled.Assessment_Type = PrevRuledOutAssessmentList[i].Assessment_Type;
        //    //                    objAssessmentRuled.Status = PrevRuledOutAssessmentList[i].Status;
        //    //                    objAssessmentRuled.Assessment_Notes = PrevRuledOutAssessmentList[i].Assessment_Notes;
        //    //                    objAssessmentRuled.Created_By = Current_UserName;
        //    //                    objAssessmentRuled.Created_Date = DateTime.Now.ToUniversalTime();
        //    //                    objAssessmentRuled.Modified_By = Current_UserName;
        //    //                    objAssessmentRuled.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    AssessmentRuledSaveList.Add(objAssessmentRuled);
        //    //                }

        //    //                IList<GeneralNotes> AssessmentRuledOutGeneralNotes = RosGeneralNotes.Where(a => a.Encounter_ID == PreviousEnc && a.Parent_Field == "Ruled Out Assessment").ToList<GeneralNotes>();
        //    //                GeneralNotes objRuledOutGeneralNotes = new GeneralNotes();
        //    //                for (int i = 0; i < AssessmentRuledOutGeneralNotes.Count; i++)
        //    //                {
        //    //                    objRuledOutGeneralNotes.Encounter_ID = ulEncounterID;
        //    //                    objRuledOutGeneralNotes.Human_ID = AssessmentRuledOutGeneralNotes[i].Human_ID;
        //    //                    objRuledOutGeneralNotes.Parent_Field = AssessmentRuledOutGeneralNotes[i].Parent_Field;
        //    //                    objRuledOutGeneralNotes.Name_Of_The_Field = AssessmentRuledOutGeneralNotes[i].Name_Of_The_Field;
        //    //                    objRuledOutGeneralNotes.Notes = AssessmentRuledOutGeneralNotes[i].Notes;
        //    //                    objRuledOutGeneralNotes.Created_By = Current_UserName;
        //    //                    objRuledOutGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    objRuledOutGeneralNotes.Modified_By = Current_UserName;
        //    //                    objRuledOutGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                }

        //    //                IList<Assessment> AssessmentRuledUpdateList = new List<Assessment>();
        //    //                IList<Assessment> AssessmentRuledDeleteList = new List<Assessment>();
        //    //                AssessmentManager objAssessmentManager = new AssessmentManager();
        //    //                objAssessmentManager.BatchOperationsinCopyPreviousEncounterToAssessmentFromRuledOut(AssessmentRuledSaveList, AssessmentRuledUpdateList, AssessmentRuledDeleteList, ulEncounterID, string.Empty, objRuledOutGeneralNotes);
        //    //            }


        //    //        }
        //    //    }
        //    //        #endregion

        //    //    #region GeneralPlan
        //    //    if (sRole.ToUpper() == "PHYSICIAN" || sRole.ToUpper() == "PHYSICIAN ASSISTANT")
        //    //    {

        //    //        IList<TreatmentPlan> GeneralPlanList = FinalTreatmentList.Where(a => a.Plan_Type == "PLAN").ToList<TreatmentPlan>();
        //    //        if (GeneralPlanList.Where(a => a.Encounter_Id == ulEncounterID).Count() == 0)
        //    //        {
        //    //            IList<TreatmentPlan> PrevGeneralList = GeneralPlanList.Where(a => a.Encounter_Id == PreviousEnc).ToList<TreatmentPlan>();
        //    //            if (PrevGeneralList.Count > 0)
        //    //            {
        //    //                IList<TreatmentPlan> TreatmentPlanSaveList = new List<TreatmentPlan>();
        //    //                TreatmentPlan objTreatmentPlan;
        //    //                for (int i = 0; i < PrevGeneralList.Count; i++)
        //    //                {
        //    //                    objTreatmentPlan = new TreatmentPlan();
        //    //                    objTreatmentPlan.Encounter_Id = ulEncounterID;
        //    //                    objTreatmentPlan.Physician_Id = uPhysicianID;
        //    //                    objTreatmentPlan.Human_ID = PrevGeneralList[i].Human_ID;
        //    //                    objTreatmentPlan.Plan = PrevGeneralList[i].Plan;
        //    //                    objTreatmentPlan.Addendum_Plan = PrevGeneralList[i].Addendum_Plan;
        //    //                    objTreatmentPlan.Created_By = Current_UserName;
        //    //                    objTreatmentPlan.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    objTreatmentPlan.Modified_By = Current_UserName;
        //    //                    objTreatmentPlan.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    objTreatmentPlan.Plan_Type = PrevGeneralList[i].Plan_Type;
        //    //                    TreatmentPlanSaveList.Add(objTreatmentPlan);
        //    //                }
        //    //                TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
        //    //                objTreatmentPlanManager.SaveUpdateDeleteWithTransaction(ref TreatmentPlanSaveList, null, null, string.Empty);
        //    //            }
        //    //        }
        //    //    #endregion

        //    //        #region CarePlan
        //    //        IList<CarePlan> careplanList = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.In("Encounter_ID", ulmyEncounter.ToArray())).List<CarePlan>();
        //    //        if (careplanList.Where(a => a.Encounter_ID == ulEncounterID).Count() == 0)
        //    //        {
        //    //            IList<CarePlan> PrevCareplanList = careplanList.Where(a => a.Encounter_ID == PreviousEnc).ToList<CarePlan>();
        //    //            if (PrevCareplanList.Count > 0)
        //    //            {
        //    //                IList<CarePlan> CarePlanSaveList = new List<CarePlan>();
        //    //                CarePlan objCarePlan;
        //    //                for (int i = 0; i < PrevCareplanList.Count; i++)
        //    //                {
        //    //                    objCarePlan = new CarePlan();
        //    //                    objCarePlan.Care_Plan_Lookup_ID = PrevCareplanList[i].Care_Plan_Lookup_ID;
        //    //                    objCarePlan.Encounter_ID = ulEncounterID;
        //    //                    objCarePlan.Physician_ID = uPhysicianID;
        //    //                    objCarePlan.Human_ID = PrevCareplanList[i].Human_ID;
        //    //                    objCarePlan.Care_Name = PrevCareplanList[i].Care_Name;
        //    //                    objCarePlan.Care_Name_Value = PrevCareplanList[i].Care_Name_Value;
        //    //                    objCarePlan.Status = PrevCareplanList[i].Status;
        //    //                    objCarePlan.Care_Plan_Notes = PrevCareplanList[i].Care_Plan_Notes;
        //    //                    objCarePlan.Plan_Date = PrevCareplanList[i].Plan_Date;
        //    //                    objCarePlan.Status_Value = PrevCareplanList[i].Status_Value;
        //    //                    objCarePlan.Created_By = Current_UserName;
        //    //                    objCarePlan.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    objCarePlan.Modified_By = Current_UserName;
        //    //                    objCarePlan.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    CarePlanSaveList.Add(objCarePlan);
        //    //                }

        //    //                CarePlanManager objCarePlanManager = new CarePlanManager();

        //    //                //seperate Method
        //    //                objCarePlanManager.SaveCarePlan(CarePlanSaveList, "", 0, string.Empty);
        //    //            }
        //    //        }
        //    //        #endregion

        //    //        #region PreventiveScreen
        //    //        IList<PreventiveScreen> PreventivescrennList = iMySession.CreateCriteria(typeof(PreventiveScreen)).Add(Expression.In("Encounter_ID", ulmyEncounter.ToArray())).List<PreventiveScreen>();

        //    //        if (PreventivescrennList.Where(a => a.Encounter_ID == ulEncounterID).Count() == 0)
        //    //        {
        //    //            IList<PreventiveScreen> PrevPreventivescrennList = PreventivescrennList.Where(a => a.Encounter_ID == PreviousEnc).ToList<PreventiveScreen>();
        //    //            if (PrevPreventivescrennList.Count > 0)
        //    //            {
        //    //                IList<PreventiveScreen> SaveList = new List<PreventiveScreen>();
        //    //                PreventiveScreen objPreventiveScreen;
        //    //                for (int i = 0; i < PrevPreventivescrennList.Count; i++)
        //    //                {
        //    //                    objPreventiveScreen = new PreventiveScreen();
        //    //                    objPreventiveScreen.Human_ID = PrevPreventivescrennList[i].Human_ID;
        //    //                    objPreventiveScreen.Encounter_ID = ulEncounterID;
        //    //                    objPreventiveScreen.Physician_ID = uPhysicianID;
        //    //                    objPreventiveScreen.Preventive_Screen_Lookup_ID = PrevPreventivescrennList[i].Preventive_Screen_Lookup_ID;
        //    //                    objPreventiveScreen.Preventive_Service = PrevPreventivescrennList[i].Preventive_Service;
        //    //                    objPreventiveScreen.Preventive_Service_Value = PrevPreventivescrennList[i].Preventive_Service_Value;
        //    //                    objPreventiveScreen.Status = PrevPreventivescrennList[i].Status;
        //    //                    objPreventiveScreen.Preventive_Screening_Notes = PrevPreventivescrennList[i].Preventive_Screening_Notes;
        //    //                    objPreventiveScreen.Created_By = Current_UserName;
        //    //                    objPreventiveScreen.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    objPreventiveScreen.Modified_By = Current_UserName;
        //    //                    objPreventiveScreen.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                    SaveList.Add(objPreventiveScreen);
        //    //                }

        //    //                PreventiveScreenManager objPreventiveScreenManager = new PreventiveScreenManager();
        //    //                objPreventiveScreenManager.SaveUpdateDeleteWithTransaction(ref SaveList, null, null, string.Empty);
        //    //            }
        //    //        }
        //    //    }
        //    //        #endregion

        //    //    #region NonDrugAllergy
        //    //    ICriteria NDA_result = iMySession.CreateCriteria(typeof(NonDrugAllergy))
        //    //                                      .Add(Expression.In("Encounter_Id", ulmyEncounter.ToArray()))
        //    //                                      .AddOrder(Order.Desc("Modified_Date_And_Time"));
        //    //    IList<NonDrugAllergy> FinalNonDrugAllergyList = NDA_result.List<NonDrugAllergy>();
        //    //    if (FinalNonDrugAllergyList.Where(a => a.Encounter_Id == ulEncounterID).Count() == 0)
        //    //    {
        //    //        IList<NonDrugAllergy> PrevNonDrugAllergyList = FinalNonDrugAllergyList.Where(a => a.Encounter_Id == PreviousEnc).ToList<NonDrugAllergy>();
        //    //        if (PrevNonDrugAllergyList.Count > 0)
        //    //        {
        //    //            IList<NonDrugAllergy> NDASaveList = new List<NonDrugAllergy>();
        //    //            NonDrugAllergy objNDA;

        //    //            for (int i = 0; i < PrevNonDrugAllergyList.Count; i++)
        //    //            {
        //    //                objNDA = new NonDrugAllergy();
        //    //                objNDA.Human_ID = PrevNonDrugAllergyList[i].Human_ID;
        //    //                objNDA.Non_Drug_Allergy_History_Info = PrevNonDrugAllergyList[i].Non_Drug_Allergy_History_Info;
        //    //                objNDA.Is_Present = PrevNonDrugAllergyList[i].Is_Present;
        //    //                objNDA.Description = PrevNonDrugAllergyList[i].Description;
        //    //                objNDA.Created_By = Current_UserName;
        //    //                objNDA.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                objNDA.Modified_By = Current_UserName;
        //    //                objNDA.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                objNDA.Encounter_Id = ulEncounterID;
        //    //                NDASaveList.Add(objNDA);
        //    //            }

        //    //            IList<GeneralNotes> NonDrugAllergyGeneralNotes = RosGeneralNotes.Where(a => a.Encounter_ID == PreviousEnc && a.Parent_Field == "Non Drug Allergy History").OrderByDescending(a => a.Modified_Date_And_Time).ToList<GeneralNotes>();
        //    //            GeneralNotes objNonDrugAllergyGeneralNotes = new GeneralNotes();
        //    //            for (int i = 0; i < NonDrugAllergyGeneralNotes.Count; i++)
        //    //            {
        //    //                objNonDrugAllergyGeneralNotes.Encounter_ID = ulEncounterID;
        //    //                objNonDrugAllergyGeneralNotes.Human_ID = NonDrugAllergyGeneralNotes[i].Human_ID;
        //    //                objNonDrugAllergyGeneralNotes.Parent_Field = NonDrugAllergyGeneralNotes[i].Parent_Field;
        //    //                objNonDrugAllergyGeneralNotes.Name_Of_The_Field = NonDrugAllergyGeneralNotes[i].Name_Of_The_Field;
        //    //                objNonDrugAllergyGeneralNotes.Notes = NonDrugAllergyGeneralNotes[i].Notes;
        //    //                objNonDrugAllergyGeneralNotes.Created_By = Current_UserName;
        //    //                objNonDrugAllergyGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //                objNonDrugAllergyGeneralNotes.Modified_By = Current_UserName;
        //    //                objNonDrugAllergyGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //    //            }

        //    //            NonDrugAllergyManager objNonDrugAllergyManager = new NonDrugAllergyManager();

        //    //            objNonDrugAllergyManager.SaveinCopyPreviousEncounterNonDrugHistory(NDASaveList
        //    //                , new List<NonDrugAllergy>(), new List<NonDrugAllergy>(), uHumanID, objNonDrugAllergyGeneralNotes, string.Empty, ulEncounterID);
        //    //        }
        //    //    }
        //    //    #endregion

        //    //    iMySession.Close();
        //    //}
        //    #endregion
        //}



        public IList<Examination> Examination(string Sex, ulong EncounterID, ulong HumanID, ulong PhysicianID, string username, ulong P_Encounter, string Category)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            //IList<FillExaminationScreen> ExamList = new List<FillExaminationScreen>();//Comment By Thiyagarajan 15-01-2013
            IList<Examination> ExamList = new List<Examination>();
            IList<Examination> Exam_pEnc = new List<Examination>();
            ulong pEncounter = P_Encounter;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (pEncounter != 0)
                {
                    ICriteria Exam_result_pEnc = iMySession.CreateCriteria(typeof(Examination)).Add(Expression.Eq("Encounter_ID", pEncounter)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Category", Category));
                    Exam_pEnc = Exam_result_pEnc.List<Examination>();
                }
                if (Exam_pEnc.Count != 0)
                {
                    IList<Examination> ExamSaveList = new List<Examination>();
                    Examination objExamination;
                    for (int i = 0; i < Exam_pEnc.Count; i++)
                    {
                        objExamination = new Examination();
                        objExamination.Encounter_ID = EncounterID;
                        objExamination.Human_ID = HumanID;
                        // objExamination.Examination_Details = Exam_pEnc[i].Examination_Details;
                        objExamination.System_Name = Exam_pEnc[i].System_Name;
                        objExamination.Physician_ID = PhysicianID;
                        objExamination.Exam_Lookup_Id = Exam_pEnc[i].Exam_Lookup_Id;
                        objExamination.Condition_Name = Exam_pEnc[i].Condition_Name;
                        objExamination.Status = Exam_pEnc[i].Status;
                        objExamination.Category = Exam_pEnc[i].Category;
                        objExamination.Examination_Notes = Exam_pEnc[i].Examination_Notes;
                        objExamination.Short_Description = Exam_pEnc[i].Short_Description;
                        objExamination.Created_By = Exam_pEnc[i].Created_By;
                        objExamination.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
                        objExamination.Modified_By = Exam_pEnc[i].Modified_By;
                        objExamination.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
                        ExamSaveList.Add(objExamination);
                    }
                    ExaminationManager objExaminationManager = new ExaminationManager();
                    // IList<Examination> Result = objExaminationManager.AppendExamination(ExamSaveList, string.Empty, Sex);
                    ICriteria Exam_result = session.GetISession().CreateCriteria(typeof(Examination)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Category", Category));
                    //ExamList = Exam_result.List<FillExaminationScreen>();  //Comment By Thiyagarajan 15-01-2013
                    ExamList = Exam_result.List<Examination>();
                }
                iMySession.Close();
            }
            return ExamList;
        }

        public void UpdateEncounter(ulong ulEncounterID, string value, string MACAddress)
        {
            //IList<Encounter> EncSaveList = null;
            EncounterManager ObjEncManager = new EncounterManager();
            IList<Encounter> EncList = ObjEncManager.GetEncounterByEncounterID(ulEncounterID);
            EncList[0].Is_Previous_Encounter_Copied = value;
            //ObjEncManager.UpdateEncounter(EncList[0], MACAddress); commented by prabu merge cmg ancillary 15-02-13
            ObjEncManager.UpdateEncounter(EncList[0], MACAddress, new object[] { "false" });
        }

        #region Append,Replace For ChiefComplaints

        public IList<FillChiefComplaint> GetPreviousEncounter(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID, string Action, ulong PreEnc)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ulong pEnc = PreEnc;
            IList<FillChiefComplaint> FillCCResults = new List<FillChiefComplaint>();
            //if (Action.ToUpper().Trim() == "APPEND")
            //{
            //    IList<ChiefComplaints> lstResult = new List<ChiefComplaints>();
            //    if (pEnc != 0)
            //    {
            //        ICriteria CC_result = session.GetISession().CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).AddOrder(Order.Desc("Modified_Date_And_Time"));
            //        lstResult = CC_result.List<ChiefComplaints>();
            //    }
            //    if (lstResult.Count > 0)
            //    {
            //        int Latest_Record_Id = GetLastRecord();
            //        int Old_Record_Id = 0;
            //        IList<ChiefComplaints> CCSaveList = new List<ChiefComplaints>();
            //        ChiefComplaints objCC;
            //        for (int i = 0; i < lstResult.Count; i++)
            //        {
            //            if (Old_Record_Id != lstResult[i].CC_Group_ID)
            //            {
            //                Latest_Record_Id = Latest_Record_Id + 1;
            //                Old_Record_Id = lstResult[i].CC_Group_ID;
            //            }
            //            objCC = new ChiefComplaints();
            //            objCC.Encounter_ID = ulEncounterID;
            //            objCC.Human_ID = lstResult[i].Human_ID;
            //            objCC.Physician_ID = uPhysicianID;
            //            objCC.HPI_Element = lstResult[i].HPI_Element;
            //            objCC.HPI_Value = lstResult[i].HPI_Value;
            //            objCC.CC_Group_ID = Latest_Record_Id;
            //            objCC.Created_By = lstResult[i].Created_By;
            //            objCC.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
            //            objCC.Modified_By = lstResult[i].Modified_By;
            //            objCC.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
            //            CCSaveList.Add(objCC);
            //        }
            //       IList<FillChiefComplaint >   savelist=  SaveComplaints(CCSaveList, string.Empty);
            //    }
            //    FillCCResults = FillCC(ulEncounterID);
            //}
            // if (Action.ToUpper().Trim() == "REPLACE")
            //{
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery Query = iMySession.CreateSQLQuery("DELETE c.* FROM chief_complaint as c Where c.Encounter_ID=" + ulEncounterID).AddEntity("c", typeof(ChiefComplaints));
                int Result_Query = Query.ExecuteUpdate();

                IList<ChiefComplaints> lstResult = new List<ChiefComplaints>();

                if (pEnc != 0)
                {
                    ICriteria CC_result = iMySession.CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).AddOrder(Order.Desc("Modified_Date_And_Time"));
                    lstResult = CC_result.List<ChiefComplaints>();
                }
                if (lstResult.Count > 0)
                {
                    // int Latest_Record_Id = GetLastRecord();
                    //  int Old_Record_Id = 0;
                    IList<ChiefComplaints> CCSaveList = new List<ChiefComplaints>();
                    ChiefComplaints objCC;
                    for (int i = 0; i < lstResult.Count; i++)
                    {
                        //if (Old_Record_Id != lstResult[i].CC_Group_ID) 
                        //{
                        //    Latest_Record_Id = Latest_Record_Id + 1;
                        //    Old_Record_Id = lstResult[i].CC_Group_ID;
                        //}
                        objCC = new ChiefComplaints();
                        objCC.Encounter_ID = ulEncounterID;
                        objCC.Human_ID = lstResult[i].Human_ID;
                        objCC.Physician_ID = uPhysicianID;
                        objCC.HPI_Element = lstResult[i].HPI_Element;
                        objCC.HPI_Value = lstResult[i].HPI_Value;
                        // objCC.CC_Group_ID = Latest_Record_Id;
                        objCC.Created_By = lstResult[i].Created_By;
                        objCC.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
                        objCC.Modified_By = lstResult[i].Modified_By;
                        objCC.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
                        CCSaveList.Add(objCC);
                    }
                    IList<Encounter> objEncounter = new List<Encounter>();
                    IList<FillChiefComplaint> savelist = SaveComplaints(CCSaveList, ref objEncounter,string.Empty);
                    FillCCResults = FillCC(ulEncounterID);
                }
                iMySession.Close();
            }
            //}
            return FillCCResults;
        }

        #endregion

        #region Replace Method

        public FillROS ReplaceROS(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID, ulong PreEnc)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ROSManager rosmanager = new ROSManager();
            GeneralNotes rosGeneralNotes = null;
            string Type = "System";
            FillROS RosEncList1 = new FillROS();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery uqery = iMySession.CreateSQLQuery("DELETE r.* FROM review_of_systems as r Where r.Encounter_ID=" + ulEncounterID).AddEntity("r", typeof(ROS));
                int uqery_result = uqery.ExecuteUpdate();

                ISQLQuery Delete_GeneralNotes = iMySession.CreateSQLQuery("DELETE g.* FROM General_notes as g Where g.Parent_Field='" + Type + "'and g.Encounter_ID=" + ulEncounterID).AddEntity("g", typeof(GeneralNotes));
                int Delete_GeneralNotes_result = Delete_GeneralNotes.ExecuteUpdate();

                ISQLQuery Delete_GeneralNotes_ROS = iMySession.CreateSQLQuery("DELETE g.* FROM General_notes as g Where g.Parent_Field='ROS General Notes' and g.Encounter_ID=" + ulEncounterID).AddEntity("g", typeof(GeneralNotes));
                int Delete_GeneralNotes_ROS_result = Delete_GeneralNotes_ROS.ExecuteUpdate();

                ICriteria ros_result_pEnc_ = iMySession.CreateCriteria(typeof(ROS)).Add(Expression.Eq("Encounter_Id", PreEnc)).Add(Expression.Eq("Human_ID", uHumanID));
                IList<ROS> Ros_List = ros_result_pEnc_.List<ROS>();
                ICriteria ros_notes_result_pEnc = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", PreEnc)).Add(Expression.Eq("Parent_Field", "System"));
                IList<GeneralNotes> RosGenerallst = ros_notes_result_pEnc.List<GeneralNotes>();
                ICriteria ros_notes_ROS_result_pEnc = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", PreEnc)).Add(Expression.Eq("Parent_Field", "ROS General Notes"));
                IList<GeneralNotes> RosGenerallst_Notes = ros_notes_ROS_result_pEnc.List<GeneralNotes>();

                FillROS RosEncList = new FillROS();

                if (Ros_List != null && Ros_List.Count != 0)
                {
                    IList<ROS> RosSaveList = new List<ROS>();
                    IList<ROS> RosUpdateList = new List<ROS>();
                    ROS objros;
                    IList<GeneralNotes> GeneralNotesSaveList = new List<GeneralNotes>();
                    IList<GeneralNotes> GeneralNotesUpdateList = new List<GeneralNotes>();
                    GeneralNotes objGeneralNotes;
                    for (int i = 0; i < Ros_List.Count; i++)
                    {
                        objros = new ROS();
                        objros.Encounter_Id = ulEncounterID;
                        objros.Human_ID = Ros_List[i].Human_ID;
                        objros.Physician_Id = uPhysicianID;
                        objros.System_Name = Ros_List[i].System_Name;
                        objros.Symptom_Name = Ros_List[i].Symptom_Name;
                        objros.Status = Ros_List[i].Status;
                        objros.Created_By = Ros_List[i].Created_By;
                        objros.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
                        objros.Modified_By = Ros_List[i].Modified_By;
                        objros.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
                        RosSaveList.Add(objros);
                    }

                    for (int i = 0; i < RosGenerallst.Count; i++)
                    {
                        objGeneralNotes = new GeneralNotes();
                        objGeneralNotes.Encounter_ID = ulEncounterID;
                        objGeneralNotes.Human_ID = RosGenerallst[i].Human_ID;
                        objGeneralNotes.Parent_Field = RosGenerallst[i].Parent_Field;
                        objGeneralNotes.Name_Of_The_Field = RosGenerallst[i].Name_Of_The_Field;
                        objGeneralNotes.Notes = RosGenerallst[i].Notes;
                        objGeneralNotes.Created_By = RosGenerallst[i].Created_By;
                        objGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
                        objGeneralNotes.Modified_By = RosGenerallst[i].Modified_By;
                        objGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
                        GeneralNotesSaveList.Add(objGeneralNotes);
                    }
                    for (int i = 0; i < RosGenerallst_Notes.Count; i++)
                    {
                        rosGeneralNotes = new GeneralNotes();
                        rosGeneralNotes.Encounter_ID = ulEncounterID;
                        rosGeneralNotes.Human_ID = RosGenerallst_Notes[i].Human_ID;
                        rosGeneralNotes.Parent_Field = RosGenerallst_Notes[i].Parent_Field;
                        rosGeneralNotes.Name_Of_The_Field = RosGenerallst_Notes[i].Name_Of_The_Field;
                        rosGeneralNotes.Notes = RosGenerallst_Notes[i].Notes;
                        rosGeneralNotes.Created_By = RosGenerallst_Notes[i].Created_By;
                        rosGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
                        rosGeneralNotes.Modified_By = RosGenerallst_Notes[i].Modified_By;
                        rosGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
                        //GeneralNotesSaveList.Add(objGeneralNotes);
                    }


                    //Commented as not in use
                    //RosEncList = rosmanager.BatchOperationsToRosAndGeneralNotes(RosSaveList, RosUpdateList, GeneralNotesSaveList, GeneralNotesUpdateList, rosGeneralNotes, ulEncounterID, string.Empty);
                }

                RosEncList1 = rosmanager.GetROSAndGeneralNotesByEncounterId(ulEncounterID, uHumanID, false);
                iMySession.Close();
            }
            return RosEncList1;
        }

        #endregion

        #region CheckingMethod

        bool CheckDuplicate_CC(IList<ChiefComplaints> objFollowUpEncounterDTO, IList<ChiefComplaints> lst_PreviousEnc)
        {
            bool enable = false;

            if (objFollowUpEncounterDTO.Count != lst_PreviousEnc.Count)//ADD
                enable = true;
            else//Update
            {
                for (int i = 0; i < objFollowUpEncounterDTO.Count; i++)
                {
                    if (objFollowUpEncounterDTO[i].HPI_Element.ToString().ToLower().Trim() == lst_PreviousEnc[i].HPI_Element.ToString().ToLower().Trim())
                    {
                        if (objFollowUpEncounterDTO[i].HPI_Value.ToString().ToLower().Trim() != lst_PreviousEnc[i].HPI_Value.ToString().ToLower().Trim())
                        {
                            enable = true;
                            break;
                        }
                    }
                    else
                    {
                        enable = true;
                        break;
                    }
                }
            }
            return enable;
        }

        bool CheckDuplicate_ROS(IList<ROS> Ros_List, IList<GeneralNotes> General_Notes_List, IList<ROS> lst_previous_ENC
            , IList<GeneralNotes> lst_previous_ENC_General_Notes
            , IList<GeneralNotes> Pre_Ros_General_notes
            , IList<GeneralNotes> Cur_Ros_General_notes)
        {
            bool duplicate = false;

            IList<GeneralNotes> lst_Current_Notes = (from CUR in General_Notes_List where (CUR.Notes.Trim() != string.Empty) orderby CUR.Id select CUR).ToList<GeneralNotes>();
            IList<GeneralNotes> lst_Previous_Notes = (from PRE in lst_previous_ENC_General_Notes where (PRE.Notes.Trim() != string.Empty) orderby PRE.Id select PRE).ToList<GeneralNotes>();
            if (lst_Current_Notes.Count != lst_Previous_Notes.Count)
            {
                duplicate = true;
                return duplicate;
            }
            else if (lst_Current_Notes.Count == lst_Previous_Notes.Count)
            {
                for (int i = 0; i < lst_Previous_Notes.Count; i++)
                {
                    if ((from CUR in lst_Current_Notes where (CUR.Notes.Trim().ToLower() == lst_Previous_Notes[i].Notes.Trim().ToLower() && CUR.Name_Of_The_Field == lst_Previous_Notes[i].Name_Of_The_Field) select CUR).Count() == 0)
                    {
                        duplicate = true;
                        return duplicate;
                        break;
                    }
                }
            }

            var Pre_GN = Pre_Ros_General_notes.Select(a => a.Notes).OrderBy(a => a).ToList();
            var Cur_GN = Cur_Ros_General_notes.Select(a => a.Notes).OrderBy(a => a).ToList();

            if ((Pre_GN.Count) != (Pre_GN.Count))
            {
                duplicate = true;
                return duplicate;
            }
            else
            {
                duplicate = !Enumerable.SequenceEqual(Pre_GN, Cur_GN);
            }

            if (!duplicate)
            {
                IList<ROS> lst_Current = (from CUR in Ros_List where (CUR.Status != string.Empty) orderby CUR.Id select CUR).ToList<ROS>();
                IList<ROS> lst_Previous = (from PRE in lst_previous_ENC where (PRE.Status != string.Empty) orderby PRE.Id select PRE).ToList<ROS>();
                if (lst_Current.Count == lst_Previous.Count)
                {
                    for (int i = 0; i < lst_Previous.Count; i++)
                    {
                        if ((from CUR in lst_Current where (CUR.Status == lst_Previous[i].Status && CUR.Symptom_Name == lst_Previous[i].Symptom_Name && CUR.System_Name == lst_Previous[i].System_Name) select CUR).Count() == 0)
                        {
                            duplicate = true;
                            break;
                        }
                    }
                }
                else
                    duplicate = true;
            }

            return duplicate;
        }

        bool CheckDuplicateExamination(IList<Examination> Pre_lst, IList<Examination> Curr_lst)
        {
            bool Duplicate = false;
            for (int i = 0; i < Curr_lst.Count; i++)
            {
                Duplicate = Pre_lst.Any(a => a.Condition_Name.Trim() == Curr_lst[i].Condition_Name.Trim() && a.System_Name == Curr_lst[i].System_Name && a.Examination_Notes == Curr_lst[i].Examination_Notes && a.Status == Curr_lst[i].Status);
                if (!Duplicate)
                    break;
            }

            return Duplicate;
        }

        bool CheckDuplicate_ASSESMENT(IList<Assessment> lst_Current_Enc_result, IList<GeneralNotes> lst_Current_GEneral_Result, IList<Assessment> lst_Previous_Enc_result, IList<GeneralNotes> lst_Previous_GEneral_Result)
        {

            bool duplicate = false;

            if (lst_Current_Enc_result.Count > lst_Previous_Enc_result.Count || lst_Current_Enc_result.Count < lst_Previous_Enc_result.Count)
                duplicate = false;
            else
            {
                for (int i = 0; i < lst_Current_Enc_result.Count; i++)
                {
                    duplicate = lst_Previous_Enc_result.Any(a => a.ICD_9 == lst_Current_Enc_result[i].ICD_9 && a.ICD_Description.Trim() == lst_Current_Enc_result[i].ICD_Description && a.Primary_Diagnosis == lst_Current_Enc_result[i].Primary_Diagnosis && a.Assessment_Type == lst_Current_Enc_result[i].Assessment_Type && a.Assessment_Notes.Trim() == lst_Current_Enc_result[i].Assessment_Notes.Trim());
                    if (!duplicate)
                    {
                        break;
                    }
                }

                if (duplicate)
                {

                    for (int i = 0; i < lst_Current_GEneral_Result.Count; i++)
                    {
                        duplicate = lst_Previous_GEneral_Result.Any(a => a.Notes.Trim() == lst_Current_GEneral_Result[i].Notes.Trim());
                        if (duplicate)
                            break;
                    }
                }
                else
                    return duplicate;
            }

            return duplicate;
            //IList<GeneralNotes> lst_Current_Notes = (from CUR in lst_Current_GEneral_Result where (CUR.Notes.Trim() != string.Empty) orderby CUR.Id select CUR).ToList<GeneralNotes>();
            //IList<GeneralNotes> lst_Previous_Notes = (from PRE in lst_Previous_GEneral_Result where (PRE.Notes.Trim() != string.Empty) orderby PRE.Id select PRE).ToList<GeneralNotes>();

            //if (lst_Current_Notes.Count != lst_Previous_Notes.Count)
            //{
            //    duplicate = true;
            //}
            //else if (lst_Current_Notes.Count == lst_Previous_Notes.Count)
            //{
            //    for (int i = 0; i < lst_Previous_Notes.Count; i++)
            //    {
            //        if ((from CUR in lst_Current_Notes where (CUR.Notes.Trim().ToLower() == lst_Previous_Notes[i].Notes.Trim().ToLower() && CUR.Name_Of_The_Field == lst_Previous_Notes[i].Name_Of_The_Field) select CUR).Count() == 0)
            //        {
            //            duplicate = true;
            //            break;
            //        }
            //    }
            //}
            //else
            //    duplicate = true;



            //if (!duplicate)
            //{
            //    if (lst_Current_Enc_result.Count == lst_Previous_Enc_result.Count)
            //    {
            //        for (int i = 0; i < lst_Previous_Enc_result.Count; i++)
            //        {
            //            if ((from CUR in lst_Current_Enc_result where (CUR.ICD_9 == lst_Previous_Enc_result[i].ICD_9 && CUR.Description == lst_Previous_Enc_result[i].Description && CUR.Primary_Diagnosis == lst_Previous_Enc_result[i].Primary_Diagnosis && CUR.Assessment_Type == lst_Previous_Enc_result[i].Assessment_Type && CUR.Assessment_Notes == lst_Previous_Enc_result[i].Assessment_Notes && CUR.Chronic_Problem == lst_Previous_Enc_result[i].Chronic_Problem) select CUR).Count() == 0)
            //            {
            //                duplicate = true;
            //                break;
            //            }
            //        }
            //    }
            //    else
            //        duplicate = true;
            //}


            //return duplicate;
        }

        bool CheckDuplicate_ASSESMENT_Ruled(IList<Assessment> lst_Current_Enc_result, IList<GeneralNotes> lst_Current_GEneral_Result, IList<Assessment> lst_Previous_Enc_result, IList<GeneralNotes> lst_Previous_GEneral_Result)
        {
            bool duplicate = false;


            //IList<GeneralNotes> lst_Current_Notes = (from CUR in lst_Current_GEneral_Result where (CUR.Notes.Trim() != string.Empty) orderby CUR.Id select CUR).ToList<GeneralNotes>();
            //IList<GeneralNotes> lst_Previous_Notes = (from PRE in lst_Previous_GEneral_Result where (PRE.Notes.Trim() != string.Empty) orderby PRE.Id select PRE).ToList<GeneralNotes>();
            //if (lst_Current_Notes.Count != lst_Previous_Notes.Count)
            //{
            //    duplicate = true;
            //}
            //else if (lst_Current_Notes.Count == lst_Previous_Notes.Count)
            //{
            //    for (int i = 0; i < lst_Previous_Notes.Count; i++)
            //    {
            //        if ((from CUR in lst_Current_Notes where (CUR.Notes.Trim().ToLower() == lst_Previous_Notes[i].Notes.Trim().ToLower() && CUR.Name_Of_The_Field == lst_Previous_Notes[i].Name_Of_The_Field) select CUR).Count() == 0)
            //        {
            //            duplicate = true;
            //            break;
            //        }
            //    }
            //}
            //else
            //    duplicate = true;


            //if (!duplicate)
            //{
            //    if (lst_Current_Enc_result.Count == lst_Previous_Enc_result.Count)
            //    {
            //        for (int i = 0; i < lst_Previous_Enc_result.Count; i++)
            //        {
            //            if ((from CUR in lst_Current_Enc_result where (CUR.ICD_9 == lst_Previous_Enc_result[i].ICD_9 && CUR.Description == lst_Previous_Enc_result[i].Description && CUR.Primary_Diagnosis == lst_Previous_Enc_result[i].Primary_Diagnosis && CUR.Assessment_Type == lst_Previous_Enc_result[i].Assessment_Type && CUR.Assessment_Notes == lst_Previous_Enc_result[i].Assessment_Notes && CUR.Chronic_Problem == lst_Previous_Enc_result[i].Chronic_Problem ) select CUR).Count() == 0)
            //            {
            //                duplicate = true;
            //                break;
            //            }
            //        }

            //    }
            //    else
            //        duplicate = true;

            //}
            return duplicate;
        }

        bool CheckDuplicate_TreatmentPlan(IList<TreatmentPlan> lst_Current_TreatmentPlan_result, IList<TreatmentPlan> lst_Previous_TreatmentPlan)
        {
            bool duplicate = false;

            for (int i = 0; i < lst_Current_TreatmentPlan_result.Count; i++)
            {
                duplicate = lst_Previous_TreatmentPlan.Any(a => a.Plan.Trim() == lst_Current_TreatmentPlan_result[i].Plan.Trim());
                if (duplicate)
                    break;

            }
            return duplicate;

            //if (lst_Current_TreatmentPlan_result.Count == lst_Previous_TreatmentPlan.Count)
            //{
            //    for (int i = 0; i < lst_Previous_TreatmentPlan.Count; i++)
            //    {
            //        if ((from CUR in lst_Current_TreatmentPlan_result where (CUR.Addendum_Plan.Trim().ToLower() == lst_Previous_TreatmentPlan[i].Addendum_Plan.Trim().ToLower() && CUR.Plan.Trim().ToLower() == lst_Previous_TreatmentPlan[i].Plan.Trim().ToLower() && CUR.Plan_Type.Trim().ToLower()==lst_Previous_TreatmentPlan[i].Plan_Type.Trim().ToLower()) select CUR).Count() == 0)
            //        {
            //            duplicate = true;
            //            break;
            //        }
            //    }
            //}
            //else
            //    duplicate = true;
            //return duplicate;
        }

        bool CheckDuplicate_CarePlan(IList<CarePlan> lst_Current_CarePlan, IList<CarePlan> lst_Previous_CarePlan)
        {
            bool duplicate = false;

            IList<CarePlan> lst_Current_CarePlan_value = (from CUR in lst_Current_CarePlan where (CUR.Status != string.Empty || CUR.Status_Value != string.Empty || CUR.Care_Plan_Notes != string.Empty) orderby CUR.Id select CUR).ToList<CarePlan>();
            IList<CarePlan> lst_Previous_CarePlan_value = (from PRE in lst_Previous_CarePlan where (PRE.Status != string.Empty || PRE.Status_Value != string.Empty || PRE.Care_Plan_Notes != string.Empty) orderby PRE.Id ascending select PRE).ToList<CarePlan>();


            if (lst_Current_CarePlan_value.Count == lst_Previous_CarePlan_value.Count)
            {
                for (int i = 0; i < lst_Previous_CarePlan_value.Count; i++)
                {
                    if ((from CUR in lst_Current_CarePlan_value where (CUR.Care_Plan_Notes.Trim().ToLower() == lst_Previous_CarePlan_value[i].Care_Plan_Notes.Trim().ToLower() && CUR.Care_Name_Value.Trim().ToLower() == lst_Previous_CarePlan_value[i].Care_Name_Value.Trim().ToLower() && CUR.Status_Value.Trim().ToLower() == lst_Previous_CarePlan_value[i].Status_Value.Trim().ToLower() && CUR.Status.Trim().ToLower() == lst_Previous_CarePlan_value[i].Status.Trim().ToLower()) select CUR).Count() == 0)
                    {
                        duplicate = true;
                        break;
                    }
                }
            }

            else
                duplicate = true;

            return duplicate;
        }

        bool CheckDuplicate_PreventiveScreen(IList<PreventiveScreen> lst_current_PreventiveScreen, IList<PreventiveScreen> lst_prvious_PreventiveScreen)
        {
            bool duplicate = false;


            IList<PreventiveScreen> lst_Current_PreventiveScreen_value = (from CUR in lst_current_PreventiveScreen where (CUR.Status != string.Empty || CUR.Preventive_Screening_Notes != string.Empty) orderby CUR.Id select CUR).ToList<PreventiveScreen>();
            IList<PreventiveScreen> lst_Previous_PreventiveScreen_value = (from PRE in lst_prvious_PreventiveScreen where (PRE.Status != string.Empty || PRE.Preventive_Screening_Notes != string.Empty) orderby PRE.Id ascending select PRE).ToList<PreventiveScreen>();


            if (lst_Current_PreventiveScreen_value.Count == lst_Previous_PreventiveScreen_value.Count)
            {
                for (int i = 0; i < lst_Previous_PreventiveScreen_value.Count; i++)
                {
                    if ((from CUR in lst_Current_PreventiveScreen_value where (CUR.Preventive_Service_Value.Trim().ToLower() == lst_Previous_PreventiveScreen_value[i].Preventive_Service_Value.Trim().ToLower() && CUR.Status.Trim().ToLower() == lst_Previous_PreventiveScreen_value[i].Status.Trim().ToLower() && CUR.Preventive_Screening_Notes.Trim().ToLower() == lst_Previous_PreventiveScreen_value[i].Preventive_Screening_Notes.Trim().ToLower()) select CUR).Count() == 0)
                    {
                        duplicate = true;
                        break;
                    }
                }
            }

            else
                duplicate = true;


            return duplicate;
        }

        #endregion

        public bool SetIsCopyPreviousEncounter(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID, string MACAddress)
        {
            bool IsUpdated = true;
            ulong previousEncounterId = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                WFObjectManager objWFObjectManager = new WFObjectManager();
                var ResultList = iMySession
                    .CreateSQLQuery(@"SELECT e.* 
                                  FROM   encounter e 
                                  WHERE  e.human_id = :Human_ID 
                                         AND e.encounter_provider_id = :Provider_ID 
                                         AND e.date_of_service <> '0001-01-01 00:00:00' 
                                         AND e.date_of_service < (SELECT e.date_of_service 
                                                                  FROM   encounter e 
                                                                  WHERE  e.encounter_id = :Encounter_ID) 
                                         AND e.encounter_id <> :Encounter_ID 
                                         AND e.is_phone_encounter <> 'Y' 
                                  ORDER  BY e.date_of_service DESC ")
                           .AddEntity("e", typeof(Encounter));

                ResultList.SetParameter("Human_ID", uHumanID);
                ResultList.SetParameter("Provider_ID", uPhysicianID);
                ResultList.SetParameter("Encounter_ID", ulEncounterID);

                IList<Encounter> ilstEncounter = ResultList.List<Encounter>();

                if (ilstEncounter.Count == 0)
                {
                    previousEncounterId = 0;
                    return !IsUpdated;
                }
                else if (ilstEncounter.Count == 1)
                {
                    previousEncounterId = ilstEncounter[0].Id;
                    if (!objWFObjectManager.IsPreviousEncounterPhysicianProcess(previousEncounterId))
                    {
                        return !IsUpdated;
                    }
                }
                else
                {
                    bool phy_process = false;
                    for (int i = 0; i < ilstEncounter.Count; i++)
                    {
                        phy_process = objWFObjectManager.IsPreviousEncounterPhysicianProcess(ilstEncounter[i].Id);
                        if (phy_process)
                        {
                            previousEncounterId = ilstEncounter[i].Id;
                            break;
                        }
                    }
                    if (!phy_process)
                    {
                        return !IsUpdated;
                    }
                }
                if (previousEncounterId != 0)
                {
                    EncounterManager ObjEncManager = new EncounterManager();
                    IList<Encounter> EncList = ObjEncManager.GetEncounterByEncounterID(ulEncounterID);
                    if (EncList.Count > 0)
                    {
                        if (EncList[0].Is_Previous_Encounter_Copied != "Y")
                        {
                            IList<Encounter> EncSaveList = new List<Encounter>(); ;
                            EncList[0].Is_Previous_Encounter_Copied = "Y";
                            //ObjEncManager.SaveUpdateDeleteWithTransaction(ref EncSaveList, EncList, null, MACAddress);

                            ObjEncManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref EncSaveList, ref EncList, null, MACAddress, false, false, 0, string.Empty);
                        }
                    }
                }
                iMySession.Close();
            }

            return IsUpdated;
        }

        //public FollowUpEncounterDTO GetPreviousEncounterIdByDateOfService(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID,
        //string Current_UserName, string Role, bool Update, string value, string MACAddress, IList<string> FieldLookupList,
        //bool Current_Encounter_Data, bool Is_From_CC_Load)
        //{
        //    FollowUpEncounterDTO maOnlyVisitDTO = new FollowUpEncounterDTO();
        //    WFObjectManager objWFObjectManager = new WFObjectManager();
        //    if (uHumanID == 0)
        //    {
        //        return maOnlyVisitDTO;
        //    }
        //    ulong previousEncounterId = 0;
        //    //ISQLQuery ResultList;
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        try
        //        {
        //            var ResultList = iMySession.CreateSQLQuery("SELECT e.* FROM encounter e WHERE e.Human_ID='" + uHumanID
        //                   + "' and e.Encounter_Provider_ID='" + uPhysicianID + @"' and e.Date_of_Service <>'0001-01-01 00:00:00' and e.Date_of_Service<(select e.Date_of_Service from encounter e where e.Encounter_ID=" + ulEncounterID
        //                   + ")and e.Encounter_ID<>" + ulEncounterID + " and e.Is_Phone_Encounter <> 'Y'  Order By e.Date_of_Service desc")
        //                   .AddEntity("e", typeof(Encounter)).List<Encounter>();


        //            if (ResultList.Count == 0)
        //            {
        //                previousEncounterId = 0;
        //            }
        //            else if (ResultList.Count == 1)
        //            {
        //                previousEncounterId = ResultList[0].Id;
        //                bool phy_process = objWFObjectManager.IsPreviousEncounterPhysicianProcess(previousEncounterId);
        //                if (!phy_process)
        //                {
        //                    maOnlyVisitDTO.Physician_Process = false;
        //                    maOnlyVisitDTO.PreviousEnc = previousEncounterId;
        //                    return maOnlyVisitDTO;
        //                }
        //                else
        //                {
        //                    maOnlyVisitDTO.Physician_Process = true;
        //                }
        //            }
        //            else
        //            {
        //                bool phy_process = false;
        //                for (int i = 0; i < ResultList.Count; i++)
        //                {
        //                    phy_process = objWFObjectManager.IsPreviousEncounterPhysicianProcess(ResultList[i].Id);
        //                    if (phy_process == true)
        //                    {
        //                        previousEncounterId = ResultList[i].Id;
        //                        maOnlyVisitDTO.Physician_Process = true;
        //                        break;
        //                    }
        //                }
        //                if (phy_process == false)
        //                {
        //                    maOnlyVisitDTO.Physician_Process = false;
        //                    maOnlyVisitDTO.PreviousEnc = previousEncounterId;
        //                    return maOnlyVisitDTO;
        //                }
        //            }
        //        }
        //        catch 
        //        {
        //            previousEncounterId = 0;
        //        }


        //        GetFollowUpEncounter(ulEncounterID, uHumanID, uPhysicianID, Current_UserName, Role, Update, value
        //            , MACAddress, FieldLookupList, Current_Encounter_Data, previousEncounterId, Is_From_CC_Load);

        //        maOnlyVisitDTO.Physician_Process = true;
        //        iMySession.Close();
        //    }

        //    return maOnlyVisitDTO;
        //}
        //public FollowUpEncounterDTO GetFollowUpEncounter(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID,
        //string Current_UserName, string Role, bool Update, string value, string MACAddress, IList<string> FieldLookupList,
        //bool Is_Save_button_click, ulong PreviousEnc, bool Is_From_CC_Load)
        //{
        //    // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    ulong pEnc = PreviousEnc;
        //    FollowUpEncounterDTO objFollowUpEncounterDTO = new FollowUpEncounterDTO();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        HumanManager objHumanManager = new HumanManager();
        //        Human objHuman = objHumanManager.GetById(uHumanID);

        //        int finalYear = CalculateAgeInMonths(objHuman.Birth_Date, DateTime.Now);
        //        string Sex = objHuman.Sex;
        //        if (pEnc != 0)
        //        {
        //            if (Update)
        //            {
        //                UpdateEncounter(ulEncounterID, value, MACAddress);
        //            }

        //            ICriteria EncounterResult = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID));
        //            objFollowUpEncounterDTO.EncounterList = EncounterResult.List<Encounter>();
        //            if (pEnc != 0)
        //            {
        //                ICriteria PreEncounterResult = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", pEnc)).Add(Expression.Eq("Human_ID", uHumanID));
        //                objFollowUpEncounterDTO.PreEncounterList = PreEncounterResult.List<Encounter>();
        //            }

        //            objFollowUpEncounterDTO.PreviousEnc = pEnc;

        //            for (int m = 0; m < FieldLookupList.Count; m++)
        //            {
        //                if (FieldLookupList[m].Trim() == "ChiefComplaints")
        //                {
        //                    #region Chief Complaints

        //                    ICriteria CC_result = iMySession.CreateCriteria(typeof(ChiefComplaints))
        //                                           .Add(Expression.Eq("Encounter_ID", ulEncounterID))
        //                                           .AddOrder(Order.Desc("Modified_Date_And_Time"));
        //                    objFollowUpEncounterDTO.CCValue = CC_result.List<ChiefComplaints>();

        //                    objFollowUpEncounterDTO.FillCC = FillPhrases(ulEncounterID);

        //                    if (objFollowUpEncounterDTO.CCValue.Count == 0 && pEnc != 0)
        //                    {
        //                        ICriteria CC_result_pEnc = iMySession.CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Encounter_ID", pEnc));
        //                        objFollowUpEncounterDTO.CCValue = CC_result_pEnc.List<ChiefComplaints>();
        //                        objFollowUpEncounterDTO.FillCC = FillPhrases(pEnc);


        //                        if (objFollowUpEncounterDTO.CCValue.Count != 0)
        //                            objFollowUpEncounterDTO.SaveEnable = true;

        //                        if (Is_Save_button_click && objFollowUpEncounterDTO.CCValue.Count != 0)
        //                        {

        //                            IList<ChiefComplaints> CCSaveList = new List<ChiefComplaints>();
        //                            ChiefComplaints objCC;
        //                            for (int i = 0; i < objFollowUpEncounterDTO.CCValue.Count; i++)
        //                            {
        //                                objCC = new ChiefComplaints();
        //                                objCC.Encounter_ID = ulEncounterID;
        //                                objCC.Human_ID = objFollowUpEncounterDTO.CCValue[i].Human_ID;
        //                                objCC.Physician_ID = uPhysicianID;
        //                                objCC.HPI_Element = objFollowUpEncounterDTO.CCValue[i].HPI_Element;
        //                                objCC.HPI_Value = objFollowUpEncounterDTO.CCValue[i].HPI_Value;
        //                                objCC.Created_By = Current_UserName;
        //                                objCC.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objCC.Modified_By = Current_UserName;
        //                                objCC.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                CCSaveList.Add(objCC);
        //                            }
        //                            IList<Encounter> objEncounter = new List<Encounter>();
        //                            objFollowUpEncounterDTO.FillCC = SaveComplaints(CCSaveList,ref objEncounter, string.Empty);
        //                            ICriteria CC_Enc_Result = iMySession.CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).AddOrder(Order.Desc("Modified_Date_And_Time"));
        //                            objFollowUpEncounterDTO.CCValue = CC_Enc_Result.List<ChiefComplaints>();
        //                        }
        //                    }

        //                    IList<ChiefComplaints> Current_CC = iMySession.CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).AddOrder(Order.Desc("Modified_Date_And_Time")).List<ChiefComplaints>();
        //                    IList<ChiefComplaints> Previous_CC = iMySession.CreateCriteria(typeof(ChiefComplaints)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).AddOrder(Order.Desc("Modified_Date_And_Time")).List<ChiefComplaints>();

        //                    if (Current_CC.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bChiefComplaints = false;
        //                        objFollowUpEncounterDTO.sChiefComplaints = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }
        //                    else if (CheckDuplicate_CC(Current_CC, Previous_CC))
        //                    {
        //                        objFollowUpEncounterDTO.bChiefComplaints = true;
        //                        if (Current_CC.Count > 0)
        //                            objFollowUpEncounterDTO.sChiefComplaints = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;
        //                        else
        //                            objFollowUpEncounterDTO.sChiefComplaints = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }
        //                    else
        //                    {
        //                        objFollowUpEncounterDTO.bChiefComplaints = false;
        //                        objFollowUpEncounterDTO.sChiefComplaints = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    if (Current_CC.Count == 0 && Previous_CC.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    }
        //                    else
        //                        objFollowUpEncounterDTO.btnList.Add(false);

        //                    #endregion
        //                }
        //                else if (FieldLookupList[m].Trim() == "ROS")
        //                {
        //                    #region Review Of System
        //                    bool bRos_Save = false;
        //                    GeneralNotes rosGeneralNotes = null;
        //                    ICriteria ros_result = iMySession.CreateCriteria(typeof(ROS)).Add(Expression.Eq("Encounter_Id", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID));
        //                    objFollowUpEncounterDTO.Ros.Ros_List = ros_result.List<ROS>();
        //                    IList<ROS> ros = (from r in objFollowUpEncounterDTO.Ros.Ros_List where r.Status != string.Empty select r).ToList<ROS>();
        //                    ROSManager rosmanager = new ROSManager();
        //                    ICriteria ros_notes_result = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Parent_Field", "System"));
        //                    objFollowUpEncounterDTO.Ros.General_Notes_List = ros_notes_result.List<GeneralNotes>();
        //                    IList<GeneralNotes> generalNotes = (from g in objFollowUpEncounterDTO.Ros.General_Notes_List where g.Notes != string.Empty select g).ToList<GeneralNotes>();

        //                    ICriteria criteriaROSGeneralNotes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Parent_Field", "ROS General Notes"));
        //                    objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List = criteriaROSGeneralNotes.List<GeneralNotes>();
        //                    IList<GeneralNotes> rosgeneralNotes = (from g in objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List where g.Notes != string.Empty select g).ToList<GeneralNotes>();

        //                    objFollowUpEncounterDTO.FillRos = rosmanager.GetROSAndGeneralNotesByEncounterId(ulEncounterID, uHumanID, false);

        //                    if (pEnc != 0 && ros.Count == 0 && generalNotes.Count == 0 && rosgeneralNotes.Count == 0)
        //                    {
        //                        ICriteria ros_result_pEnc_ = iMySession.CreateCriteria(typeof(ROS)).Add(Expression.Eq("Encounter_Id", pEnc)).Add(Expression.Eq("Human_ID", uHumanID));
        //                        objFollowUpEncounterDTO.Ros.Ros_List = ros_result_pEnc_.List<ROS>();
        //                        ICriteria ros_notes_result_pEnc = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Parent_Field", "System"));
        //                        objFollowUpEncounterDTO.Ros.General_Notes_List = ros_notes_result_pEnc.List<GeneralNotes>();
        //                        ICriteria ros_general_notes_result_pEnc = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Parent_Field", "ROS General Notes"));
        //                        objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List = ros_general_notes_result_pEnc.List<GeneralNotes>();

        //                        if (objFollowUpEncounterDTO.Ros.General_Notes_List.Count != 0 || objFollowUpEncounterDTO.Ros.Ros_List.Count != 0 || objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List.Count != 0)
        //                            objFollowUpEncounterDTO.SaveEnable = true;

        //                        objFollowUpEncounterDTO.FillRos = rosmanager.GetROSAndGeneralNotesByEncounterId(pEnc, uHumanID, false);

        //                        if (objFollowUpEncounterDTO.Ros.Ros_List.Count != 0 && Is_Save_button_click)
        //                        {
        //                            IList<ROS> RosSaveList = new List<ROS>();
        //                            IList<ROS> RosUpdateList = new List<ROS>();
        //                            ROS objros;
        //                            IList<GeneralNotes> GeneralNotesSaveList = new List<GeneralNotes>();
        //                            IList<GeneralNotes> GeneralNotesUpdateList = new List<GeneralNotes>();
        //                            GeneralNotes objGeneralNotes;
        //                            for (int i = 0; i < objFollowUpEncounterDTO.Ros.Ros_List.Count; i++)
        //                            {
        //                                objros = new ROS();
        //                                objros.Encounter_Id = ulEncounterID;
        //                                objros.Human_ID = objFollowUpEncounterDTO.Ros.Ros_List[i].Human_ID;
        //                                objros.Physician_Id = uPhysicianID;
        //                                objros.System_Name = objFollowUpEncounterDTO.Ros.Ros_List[i].System_Name;
        //                                objros.Symptom_Name = objFollowUpEncounterDTO.Ros.Ros_List[i].Symptom_Name;
        //                                objros.Status = objFollowUpEncounterDTO.Ros.Ros_List[i].Status;
        //                                objros.Created_By = Current_UserName;
        //                                objros.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objros.Modified_By = Current_UserName;
        //                                objros.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                RosSaveList.Add(objros);
        //                            }

        //                            for (int i = 0; i < objFollowUpEncounterDTO.Ros.General_Notes_List.Count; i++)
        //                            {
        //                                objGeneralNotes = new GeneralNotes();
        //                                objGeneralNotes.Encounter_ID = ulEncounterID;
        //                                objGeneralNotes.Human_ID = objFollowUpEncounterDTO.Ros.General_Notes_List[i].Human_ID;
        //                                objGeneralNotes.Parent_Field = objFollowUpEncounterDTO.Ros.General_Notes_List[i].Parent_Field;
        //                                objGeneralNotes.Name_Of_The_Field = objFollowUpEncounterDTO.Ros.General_Notes_List[i].Name_Of_The_Field;
        //                                objGeneralNotes.Notes = objFollowUpEncounterDTO.Ros.General_Notes_List[i].Notes;
        //                                objGeneralNotes.Created_By = Current_UserName;
        //                                objGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objGeneralNotes.Modified_By = Current_UserName;
        //                                objGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                GeneralNotesSaveList.Add(objGeneralNotes);
        //                            }
        //                            for (int i = 0; i < objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List.Count; i++)
        //                            {
        //                                rosGeneralNotes = new GeneralNotes();
        //                                rosGeneralNotes.Encounter_ID = ulEncounterID;
        //                                rosGeneralNotes.Human_ID = objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List[i].Human_ID;
        //                                rosGeneralNotes.Parent_Field = objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List[i].Parent_Field;
        //                                rosGeneralNotes.Name_Of_The_Field = objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List[i].Name_Of_The_Field;
        //                                rosGeneralNotes.Notes = objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List[i].Notes;
        //                                rosGeneralNotes.Created_By = Current_UserName;
        //                                rosGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                rosGeneralNotes.Modified_By = Current_UserName;
        //                                rosGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                            }


        //                            FillROS RosEncList = new FillROS();

        //                            //Commented as not in use
        //                            //RosEncList = rosmanager.BatchOperationsToRosAndGeneralNotes(RosSaveList, RosUpdateList, GeneralNotesSaveList, GeneralNotesUpdateList, rosGeneralNotes, ulEncounterID, string.Empty);

        //                            objFollowUpEncounterDTO.Ros.Ros_List = RosEncList.Ros_List;
        //                            objFollowUpEncounterDTO.Ros.General_Notes_List = RosEncList.General_Notes_List;
        //                            objFollowUpEncounterDTO.Ros.ROS_GeneralNotes_List = RosEncList.ROS_GeneralNotes_List;
        //                            objFollowUpEncounterDTO.FillRos = rosmanager.GetROSAndGeneralNotesByEncounterId(ulEncounterID, uHumanID, false);
        //                        }
        //                    }

        //                    IList<ROS> Cur_Ros = iMySession.CreateCriteria(typeof(ROS)).Add(Expression.Eq("Encounter_Id", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).List<ROS>();
        //                    IList<GeneralNotes> Cur_Ros_notes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Parent_Field", "System")).List<GeneralNotes>();
        //                    IList<GeneralNotes> Cur_Ros_General_notes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Parent_Field", "ROS General Notes")).List<GeneralNotes>();

        //                    IList<ROS> Pre_Ros = iMySession.CreateCriteria(typeof(ROS)).Add(Expression.Eq("Encounter_Id", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).List<ROS>();
        //                    IList<GeneralNotes> Pre_Ros_notes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Parent_Field", "System")).List<GeneralNotes>();
        //                    IList<GeneralNotes> Pre_Ros_General_notes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Parent_Field", "ROS General Notes")).List<GeneralNotes>();


        //                    if (Cur_Ros.Count == 0 && Cur_Ros_notes.Count == 0 && Cur_Ros_General_notes.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.sROS = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                        objFollowUpEncounterDTO.bROS = false;
        //                    }

        //                    else if (CheckDuplicate_ROS(Cur_Ros, Cur_Ros_notes, Pre_Ros, Pre_Ros_notes, Pre_Ros_General_notes, Cur_Ros_General_notes))
        //                    {
        //                        if (Cur_Ros.Count > 0 && Cur_Ros.Any(a => a.Status.Trim() != string.Empty) || Cur_Ros_notes.Count > 0 && Cur_Ros_notes.Any(a => a.Notes.Trim() != string.Empty))
        //                            objFollowUpEncounterDTO.sROS = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;
        //                        else
        //                        {
        //                            objFollowUpEncounterDTO.sROS = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                            bRos_Save = true;
        //                        }
        //                        objFollowUpEncounterDTO.bROS = true;
        //                    }
        //                    else
        //                    {
        //                        objFollowUpEncounterDTO.sROS = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                        objFollowUpEncounterDTO.bROS = false;
        //                    }

        //                    if ((Cur_Ros.Count == 0 || Cur_Ros_notes.Count == 0 || Cur_Ros_General_notes.Count == 0) && (Pre_Ros.Count > 0 || Pre_Ros_notes.Count > 0 || Pre_Ros_General_notes.Count > 0))
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    else if (bRos_Save)
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    else
        //                        objFollowUpEncounterDTO.btnList.Add(false);

        //                    #endregion
        //                }
        //                else if (FieldLookupList[m].Trim() == "PatientResults")
        //                {
        //                    #region Vitals Results

        //                    VitalsManager objVitalsManager = new VitalsManager();
        //                    string human_id = Convert.ToString(uHumanID);
        //                    ulong ID = 0;
        //                    try
        //                    {
        //                        ID = iMySession.CreateCriteria(typeof(ResultMaster))
        //                                    .SetProjection(Projections.Max("Id"))
        //                                    .Add(Expression.Eq("PID_Alternate_Patient_ID", human_id.Trim())).List<ulong>()[0];
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ID = 0;
        //                    }

        //                    if (ID != 0)
        //                    {
        //                        ISQLQuery result_obx_query = iMySession.CreateSQLQuery("SELECT r.* FROM result_obx r where Result_Master_ID='" + ID + "'and OBX_Observation_Identifier in('84432','83718','83721','82043')").AddEntity("r", typeof(ResultOBX));
        //                        if (result_obx_query.List<ResultOBX>().Count != 0)
        //                        {
        //                            objFollowUpEncounterDTO.Latest_Result = result_obx_query.List<ResultOBX>();
        //                        }
        //                        ISQLQuery Loinc_Identifier = iMySession.CreateSQLQuery("SELECT r.* FROM result_obx r where Result_Master_ID='" + ID + "'and OBX_Loinc_Identifier='26464-8'").AddEntity("r", typeof(ResultOBX));
        //                        if (Loinc_Identifier.List<ResultOBX>().Count != 0)
        //                        {
        //                            objFollowUpEncounterDTO.Loinc_Identifier = result_obx_query.List<ResultOBX>()[0].OBX_Observation_Value.ToString();
        //                        }
        //                    }

        //                    //Height,Weight
        //                    //ICriteria Patient_result = session.GetISession().CreateCriteria(typeof(PatientResults)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Results_Type", "Vitals"));
        //                    ISQLQuery sqlquery = iMySession.CreateSQLQuery("select v.* from patient_results v inner join dynamic_screen s on (v.Loinc_Observation=s.control_Name ) inner join map_vitals_physician m on (s.master_vitals_id=m.master_vitals_id)  where v.human_id='" + uHumanID + "' and v.Results_Type='Vitals' and v.encounter_id='" + ulEncounterID + "' and loinc_observation = v.loinc_observation and  v.Captured_date_and_time in(select max(Captured_date_and_time)   from patient_results where human_id=v.human_id and encounter_id=v.encounter_id and loinc_observation = v.loinc_observation) group by v.Loinc_Observation order by m.sort_order").AddEntity("v", typeof(PatientResults));
        //                    objFollowUpEncounterDTO.Vitals_Result = sqlquery.List<PatientResults>();
        //                    // objFollowUpEncounterDTO.Vitals_Result = Patient_result.List<PatientResults>();

        //                    #endregion
        //                }
        //                else if (FieldLookupList[m].Trim() == "Examination")
        //                {
        //                    #region Exam
        //                    IList<Examination> ilstCurrentExam = new List<Examination>();
        //                    IList<Examination> ilstPrevExam = new List<Examination>();

        //                    ICriteria General_List_Result = iMySession.CreateCriteria(typeof(Examination))
        //                                                                    .Add(Expression.Eq("Encounter_ID", ulEncounterID))
        //                                                                    .Add(Expression.Eq("Human_ID", uHumanID))
        //                                                                    .Add(Expression.Eq("Category", "General With Specialty"));

        //                    ilstCurrentExam = General_List_Result.List<Examination>();

        //                    ExamLookupManager objExamLookupManager = new ExamLookupManager();

        //                    if (ilstCurrentExam != null && ilstCurrentExam.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.GeneralExamScreen = objExamLookupManager.GetExamLookupListFromServer("General With Specialty", string.Empty, ulEncounterID, string.Empty);
        //                    }

        //                    var objHumanID = iMySession.CreateCriteria(typeof(Human))
        //                                                        .Add(Expression.Eq("Id", uHumanID))
        //                                                        .List<Human>().FirstOrDefault();

        //                    if (objFollowUpEncounterDTO.GeneralExamScreen.Count == 0 && pEnc != 0 && Is_From_CC_Load == false)
        //                    {
        //                        ICriteria General_List_Result_Previous = iMySession.CreateCriteria(typeof(Examination))
        //                                                                        .Add(Expression.Eq("Encounter_ID", pEnc))
        //                                                                        .Add(Expression.Eq("Human_ID", uHumanID))
        //                                                                        .Add(Expression.Eq("Category", "General With Specialty"));

        //                        ilstPrevExam = General_List_Result_Previous.List<Examination>();

        //                        if (ilstPrevExam.Count > 0)
        //                        {
        //                            objFollowUpEncounterDTO.GeneralExamScreen = objExamLookupManager.GetExamLookupListFromServer("General With Specialty", string.Empty, pEnc, string.Empty);
        //                            if (objFollowUpEncounterDTO.GeneralExamScreen != null
        //                                && objFollowUpEncounterDTO.GeneralExamScreen.Count > 0)
        //                                objFollowUpEncounterDTO.GeneralExamScreen = objFollowUpEncounterDTO.GeneralExamScreen.Where(a => a.Version != 0).Select(b => { b.Version = 0; b.ExaminationID = 0; return b; }).ToList<FillExaminationScreen>();
        //                        }

        //                        if (objFollowUpEncounterDTO.GeneralExamScreen.Count != 0)
        //                            objFollowUpEncounterDTO.SaveEnable = true;

        //                        if (objFollowUpEncounterDTO.GeneralExamScreen.Count != 0 && Is_Save_button_click)
        //                        {
        //                            IList<Examination> ExamLst = Examination(objHumanID.Sex, ulEncounterID, uHumanID, uPhysicianID, Current_UserName, pEnc, "General With Specialty");
        //                            if (ExamLst != null && ExamLst.Count > 0)
        //                            {
        //                                objFollowUpEncounterDTO.GeneralExamScreen = objExamLookupManager.GetExamLookupListFromServer("General With Specialty", string.Empty, ulEncounterID, string.Empty);
        //                            }
        //                        }
        //                    }

        //                    IList<Examination> Cur_Exam_general = iMySession.CreateCriteria(typeof(Examination))
        //                        .Add(Expression.Eq("Encounter_ID", ulEncounterID))
        //                        .Add(Expression.Eq("Human_ID", uHumanID))
        //                        .Add(Expression.Eq("Category", "General With Specialty")).List<Examination>();

        //                    IList<Examination> Pre_Exam_general = iMySession.CreateCriteria(typeof(Examination))
        //                        .Add(Expression.Eq("Encounter_ID", pEnc))
        //                        .Add(Expression.Eq("Human_ID", uHumanID))
        //                        .Add(Expression.Eq("Category", "General With Specialty")).List<Examination>();

        //                    if (Cur_Exam_general.Count == 0 && Pre_Exam_general.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bGeneralExamination = false;
        //                    }
        //                    else if (ilstCurrentExam.Count == 0 && Pre_Exam_general.Count > 0 && Is_From_CC_Load)
        //                    {
        //                        objFollowUpEncounterDTO.bGeneralExamination = true;
        //                    }
        //                    else if (Cur_Exam_general.Count == 0 && Pre_Exam_general.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.bGeneralExamination = false;
        //                        objFollowUpEncounterDTO.sGeneralExamination = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    else if (Cur_Exam_general.Count > 0 && Pre_Exam_general.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bGeneralExamination = true;
        //                        objFollowUpEncounterDTO.sGeneralExamination = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;
        //                    }
        //                    else if (Cur_Exam_general.Count > 0 && Pre_Exam_general.Count > 0)
        //                    {
        //                        if (CheckDuplicateExamination(Pre_Exam_general, Cur_Exam_general))
        //                        {
        //                            objFollowUpEncounterDTO.bGeneralExamination = false;
        //                            objFollowUpEncounterDTO.sGeneralExamination = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                        }
        //                        else
        //                        {
        //                            objFollowUpEncounterDTO.bGeneralExamination = true;
        //                            objFollowUpEncounterDTO.sGeneralExamination = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;
        //                        }
        //                    }

        //                    if (Is_From_CC_Load)
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_Exam_general.Count > 0 || (Cur_Exam_general.Count == 0 && Pre_Exam_general.Count == 0))
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_Exam_general.Count == 0 && Pre_Exam_general.Count > 0)
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    else
        //                        objFollowUpEncounterDTO.btnList.Add(false);

        //                    //Foused

        //                    IList<Examination> examlstFocused = new List<Examination>();
        //                    IList<Examination> PrevexamlstFoused = new List<Examination>();

        //                    ICriteria General_List_Result_Focused = iMySession.CreateCriteria(typeof(Examination))
        //                        .Add(Expression.Eq("Encounter_ID", ulEncounterID))
        //                        .Add(Expression.Eq("Human_ID", uHumanID))
        //                        .Add(Expression.Eq("Category", "Focused"));
        //                    examlstFocused = General_List_Result_Focused.List<Examination>();
        //                    ExamLookupManager objExamLookupManagerFocused = new ExamLookupManager();
        //                    if (examlstFocused != null && examlstFocused.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.FocusedExamScreen = objExamLookupManagerFocused.GetExamLookupListFromServer("Focused", string.Empty, ulEncounterID, string.Empty);
        //                    }

        //                    if (objFollowUpEncounterDTO.FocusedExamScreen.Count == 0 && pEnc != 0 && Is_From_CC_Load == false)
        //                    {
        //                        ICriteria General_List_Result_Previous = iMySession.CreateCriteria(typeof(Examination)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Category", "Focused"));

        //                        PrevexamlstFoused = General_List_Result_Previous.List<Examination>();
        //                        if (PrevexamlstFoused.Count > 0)
        //                        {

        //                            objFollowUpEncounterDTO.FocusedExamScreen = objExamLookupManagerFocused.GetExamLookupListFromServer("Focused", string.Empty, pEnc, string.Empty);
        //                            if (objFollowUpEncounterDTO.FocusedExamScreen != null && objFollowUpEncounterDTO.FocusedExamScreen.Count > 0)
        //                                objFollowUpEncounterDTO.FocusedExamScreen = objFollowUpEncounterDTO.FocusedExamScreen.Where(a => a.Version != 0).Select(b => { b.Version = 0; b.ExaminationID = 0; return b; }).ToList<FillExaminationScreen>();
        //                        }


        //                        if (objFollowUpEncounterDTO.FocusedExamScreen.Count != 0)
        //                            objFollowUpEncounterDTO.SaveEnable = true;


        //                        if (objFollowUpEncounterDTO.FocusedExamScreen.Count != 0 && Is_Save_button_click)
        //                        {
        //                            IList<Examination> ExamLst = Examination(objHumanID.Sex, ulEncounterID, uHumanID, uPhysicianID, Current_UserName, pEnc, "Focused");
        //                            if (ExamLst != null && ExamLst.Count > 0)
        //                            {
        //                                objFollowUpEncounterDTO.FocusedExamScreen = objExamLookupManagerFocused.GetExamLookupListFromServer("Focused", string.Empty, ulEncounterID, string.Empty);
        //                            }
        //                        }
        //                    }

        //                    IList<Examination> Cur_Exam_focused_ = iMySession.CreateCriteria(typeof(Examination)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Category", "Focused")).List<Examination>();
        //                    IList<Examination> Pre_Exam_focused = iMySession.CreateCriteria(typeof(Examination)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Category", "Focused")).List<Examination>();

        //                    if (Cur_Exam_focused_.Count == 0 && Pre_Exam_focused.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bFocusedExamination = false;
        //                    }
        //                    else if (examlstFocused.Count == 0 && Pre_Exam_focused.Count > 0 && Is_From_CC_Load)
        //                    {
        //                        objFollowUpEncounterDTO.bFocusedExamination = true;
        //                    }
        //                    else if (Cur_Exam_focused_.Count == 0 && Pre_Exam_focused.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.bFocusedExamination = false;
        //                        objFollowUpEncounterDTO.sFocusedExamination = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    else if (Cur_Exam_focused_.Count > 0 && Pre_Exam_focused.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bFocusedExamination = true;
        //                        objFollowUpEncounterDTO.sFocusedExamination = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;
        //                    }
        //                    else if (Cur_Exam_focused_.Count > 0 && Pre_Exam_focused.Count > 0)
        //                    {
        //                        if (CheckDuplicateExamination(Pre_Exam_focused, Cur_Exam_focused_))
        //                        {
        //                            objFollowUpEncounterDTO.bFocusedExamination = false;
        //                            objFollowUpEncounterDTO.sFocusedExamination = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                        }
        //                        else
        //                        {
        //                            objFollowUpEncounterDTO.bFocusedExamination = true;
        //                            objFollowUpEncounterDTO.sFocusedExamination = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;
        //                        }
        //                    }

        //                    if (Is_From_CC_Load)
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_Exam_focused_.Count > 0 || (Cur_Exam_focused_.Count == 0 && Pre_Exam_focused.Count == 0))
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_Exam_focused_.Count == 0 && Pre_Exam_focused.Count > 0)
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    else
        //                        objFollowUpEncounterDTO.btnList.Add(false);

        //                    #endregion
        //                }
        //                else if (FieldLookupList[m].Trim() == "Assessment")
        //                {
        //                    #region Assessment Selected

        //                    ICriteria Assessment_result = iMySession.CreateCriteria(typeof(Assessment))
        //                        .Add(Expression.Eq("Encounter_ID", ulEncounterID))
        //                        .Add(Expression.Eq("Human_ID", uHumanID))
        //                        .Add(Expression.Eq("Assessment_Type", "Selected"));

        //                    objFollowUpEncounterDTO.FillAssessment.Assessment = Assessment_result.List<Assessment>();

        //                    ICriteria GeneralNotes_result = iMySession.CreateCriteria(typeof(GeneralNotes))
        //                        .Add(Expression.Eq("Encounter_ID", ulEncounterID))
        //                        .Add(Expression.Eq("Human_ID", uHumanID))
        //                        .Add(Expression.Eq("Parent_Field", "Selected Assessment"));

        //                    objFollowUpEncounterDTO.FillAssessment.General_Notes = GeneralNotes_result.List<GeneralNotes>();

        //                    //added by pravin-02-11-2012

        //                    if (!Is_From_CC_Load)
        //                    {
        //                        //ISQLQuery sqlquery = session.GetISession().CreateSQLQuery("select v.* from patient_results v inner join dynamic_screen s on (v.Loinc_Observation=s.control_Name)   where v.encounter_id='" + pEnc + "' and v.human_id='" + uHumanID + "' and Results_Type='Vitals' and v.Vitals_group_id in(select max(Vitals_group_id) from patient_results where human_id=v.human_id and encounter_id= v.encounter_id) group by v.Loinc_Observation order by s.sort_order").AddEntity("v", typeof(PatientResults));
        //                        //AssessmentVitalsLookupManager objAssessVitalsMngr = new AssessmentVitalsLookupManager();
        //                        //objFollowUpEncounterDTO.FillAssessment.VitalsBasedICD_List = objAssessVitalsMngr.GetIcdBasedOnVitals(sqlquery.List<PatientResults>());
        //                    }

        //                    if (objFollowUpEncounterDTO.FillAssessment.Assessment.Count == 0 &&
        //                        objFollowUpEncounterDTO.FillAssessment.General_Notes.Count == 0
        //                        && pEnc != 0 && !Is_From_CC_Load)
        //                    {
        //                        ICriteria Assessment_General_List_Result_Previous = iMySession.CreateCriteria(typeof(Assessment))
        //                            .Add(Expression.Eq("Encounter_ID", pEnc))
        //                            .Add(Expression.Eq("Human_ID", uHumanID))
        //                            .Add(Expression.Eq("Assessment_Type", "Selected"));
        //                        objFollowUpEncounterDTO.FillAssessment.Assessment = Assessment_General_List_Result_Previous.List<Assessment>();
        //                        ICriteria GeneralNotes_result_Previous = iMySession.CreateCriteria(typeof(GeneralNotes))
        //                            .Add(Expression.Eq("Encounter_ID", pEnc))
        //                            .Add(Expression.Eq("Human_ID", uHumanID))
        //                            .Add(Expression.Eq("Parent_Field", "Selected Assessment"));
        //                        objFollowUpEncounterDTO.FillAssessment.General_Notes = GeneralNotes_result_Previous.List<GeneralNotes>();


        //                        if (objFollowUpEncounterDTO.FillAssessment.Assessment.Count != 0 ||
        //                            objFollowUpEncounterDTO.FillAssessment.General_Notes.Count != 0) objFollowUpEncounterDTO.SaveEnable = true;


        //                        if ((objFollowUpEncounterDTO.FillAssessment.Assessment.Count > 0 ||
        //                            objFollowUpEncounterDTO.FillAssessment.General_Notes.Count > 0) &&
        //                            Is_Save_button_click)
        //                        {
        //                            IList<Assessment> AssessmentSaveList = new List<Assessment>();
        //                            IList<ProblemList> ProblemSaveList = new List<ProblemList>();
        //                            IList<TreatmentPlan> TreatmentSaveList = new List<TreatmentPlan>();
        //                            IList<GeneralNotes> GeneralSaveList = new List<GeneralNotes>();
        //                            Assessment objAssessment;
        //                            ProblemList objProblemList;
        //                            TreatmentPlan objTreatmentPlanAssessment = new TreatmentPlan();
        //                            GeneralNotes objGeneralNotes = new GeneralNotes();

        //                            IList<Assessment> Assessment_pEnc = objFollowUpEncounterDTO.FillAssessment.Assessment;

        //                            for (int i = 0; i < Assessment_pEnc.Count; i++)
        //                            {
        //                                objAssessment = new Assessment();
        //                                objAssessment.Encounter_ID = ulEncounterID;
        //                                objAssessment.Human_ID = Assessment_pEnc[i].Human_ID;
        //                                objAssessment.Physician_ID = uPhysicianID;
        //                                objAssessment.ICD_9 = Assessment_pEnc[i].ICD_9;
        //                                objAssessment.ICD_Description = Assessment_pEnc[i].ICD_Description;
        //                                objAssessment.Primary_Diagnosis = Assessment_pEnc[i].Primary_Diagnosis;
        //                                objAssessment.Diagnosis_Source = Assessment_pEnc[i].Diagnosis_Source;
        //                                objAssessment.Chronic_Problem = Assessment_pEnc[i].Chronic_Problem;
        //                                objAssessment.Assessment_Type = Assessment_pEnc[i].Assessment_Type;
        //                                objAssessment.Assessment_Status = Assessment_pEnc[i].Assessment_Status;
        //                                objAssessment.Assessment_Notes = Assessment_pEnc[i].Assessment_Notes;
        //                                objAssessment.Created_By = Current_UserName;
        //                                objAssessment.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objAssessment.Modified_By = Current_UserName;
        //                                objAssessment.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                AssessmentSaveList.Add(objAssessment);
        //                            }

        //                            ICriteria Problem_Result = iMySession.CreateCriteria(typeof(ProblemList))
        //                                .Add(Expression.Eq("Encounter_ID", pEnc))
        //                                .Add(Expression.Eq("Human_ID", uHumanID))
        //                                .Add(Expression.Eq("Reference_Source", "Assessment"));

        //                            objFollowUpEncounterDTO.FillAssessment.Problem_List = Problem_Result.List<ProblemList>();

        //                            for (int i = 0; i < objFollowUpEncounterDTO.FillAssessment.Problem_List.Count; i++)
        //                            {
        //                                objProblemList = new ProblemList();
        //                                objProblemList.Encounter_ID = ulEncounterID;
        //                                objProblemList.Human_ID = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].Human_ID;
        //                                objProblemList.Physician_ID = uPhysicianID;
        //                                objProblemList.Reference_Source = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].Reference_Source;
        //                                objProblemList.ICD = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].ICD;
        //                                objProblemList.Problem_Description = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].Problem_Description;
        //                                objProblemList.Status = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].Status;
        //                                objProblemList.Date_Diagnosed = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].Date_Diagnosed;
        //                                objProblemList.Rcopia_ID = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].Rcopia_ID;
        //                                objProblemList.Last_Modified_By = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].Modified_By;
        //                                objProblemList.Last_Modified_Date = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].Modified_Date_And_Time;
        //                                objProblemList.Is_Active = objFollowUpEncounterDTO.FillAssessment.Problem_List[i].Is_Active;
        //                                objProblemList.Created_By = Current_UserName;
        //                                objProblemList.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objProblemList.Modified_By = Current_UserName;
        //                                objProblemList.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                ProblemSaveList.Add(objProblemList);
        //                            }
        //                            ICriteria Treatment_Result = iMySession.CreateCriteria(typeof(TreatmentPlan))
        //                                .Add(Expression.Eq("Encounter_Id", pEnc))
        //                                .Add(Expression.Eq("Human_ID", uHumanID))
        //                                .Add(Expression.Eq("Plan_Type", "ASSESSMENT"));
        //                            IList<TreatmentPlan> Ass_TPlan_result = Treatment_Result.List<TreatmentPlan>();
        //                            if (Ass_TPlan_result.Count != 0)
        //                            {
        //                                for (int i = 0; i < Ass_TPlan_result.Count; i++)
        //                                {
        //                                    objTreatmentPlanAssessment.Encounter_Id = ulEncounterID;
        //                                    objTreatmentPlanAssessment.Human_ID = Ass_TPlan_result[i].Human_ID;
        //                                    objTreatmentPlanAssessment.Physician_Id = uPhysicianID;
        //                                    objTreatmentPlanAssessment.Plan = Ass_TPlan_result[i].Plan;
        //                                    objTreatmentPlanAssessment.Addendum_Plan = Ass_TPlan_result[i].Addendum_Plan;
        //                                    objTreatmentPlanAssessment.Plan_Type = Ass_TPlan_result[i].Plan_Type;
        //                                    objTreatmentPlanAssessment.Created_By = Current_UserName;
        //                                    objTreatmentPlanAssessment.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                    objTreatmentPlanAssessment.Modified_By = Current_UserName;
        //                                    objTreatmentPlanAssessment.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                }
        //                            }


        //                            ICriteria GEneral_Result = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Selected Assessment"));
        //                            objFollowUpEncounterDTO.FillAssessment.General_Notes = GEneral_Result.List<GeneralNotes>();
        //                            for (int i = 0; i < objFollowUpEncounterDTO.FillAssessment.General_Notes.Count; i++)
        //                            {
        //                                objGeneralNotes.Encounter_ID = ulEncounterID;
        //                                objGeneralNotes.Human_ID = objFollowUpEncounterDTO.FillAssessment.General_Notes[i].Human_ID;
        //                                objGeneralNotes.Parent_Field = objFollowUpEncounterDTO.FillAssessment.General_Notes[i].Parent_Field;
        //                                objGeneralNotes.Name_Of_The_Field = objFollowUpEncounterDTO.FillAssessment.General_Notes[i].Name_Of_The_Field;
        //                                objGeneralNotes.Notes = objFollowUpEncounterDTO.FillAssessment.General_Notes[i].Notes;
        //                                objGeneralNotes.Created_By = Current_UserName;
        //                                objGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objGeneralNotes.Modified_By = Current_UserName;
        //                                objGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                            }
        //                            IList<Assessment> AssessmentUpdateList = new List<Assessment>();
        //                            IList<Assessment> AssessmentDeleteList = new List<Assessment>();
        //                            IList<ProblemList> ProblemUpdateList = new List<ProblemList>();
        //                            IList<ProblemList> ProblemDeleteList = new List<ProblemList>();
        //                            AssessmentManager objAssessmentManager = new AssessmentManager();
        //                            IList<string> MacraICDlst = new List<string>();
        //                            FillAssessment FillAssessment_result = objAssessmentManager.BatchOperationsToAssessment(AssessmentSaveList, AssessmentUpdateList, AssessmentDeleteList, ProblemSaveList, ProblemUpdateList, ProblemDeleteList, string.Empty, objGeneralNotes, objTreatmentPlanAssessment, Current_UserName, objTreatmentPlanAssessment.Encounter_Id, objTreatmentPlanAssessment.Human_ID, objHuman.Is_Sent_To_Rcopia, uPhysicianID, MacraICDlst,"No");
        //                            ICriteria Assessment_result_Enc = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Assessment_Type", "Selected"));
        //                            objFollowUpEncounterDTO.FillAssessment.Assessment = Assessment_result_Enc.List<Assessment>();
        //                            ICriteria GeneralNotes_result_Enc = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Selected Assessment"));
        //                            objFollowUpEncounterDTO.FillAssessment.General_Notes = GeneralNotes_result_Enc.List<GeneralNotes>();
        //                            objFollowUpEncounterDTO.FillAssessment = objAssessmentManager.LoadAssessment(pEnc, uHumanID, "N", Current_UserName, "reload");
        //                        }
        //                    }

        //                    IList<Assessment> Cur_Assesment = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Assessment_Type", "Selected")).List<Assessment>();
        //                    IList<GeneralNotes> Cur_AssesmentGeneralNotes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Selected Assessment")).List<GeneralNotes>();

        //                    IList<Assessment> Pre_Assesment = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Assessment_Type", "Selected")).List<Assessment>();
        //                    IList<GeneralNotes> Pre_AssesmentGeneralNotes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Selected Assessment")).List<GeneralNotes>();

        //                    if (Cur_Assesment.Count == 0 && Cur_AssesmentGeneralNotes.Count == 0 && Pre_Assesment.Count == 0 && Pre_AssesmentGeneralNotes.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bSelectedAssessment = false;
        //                        //objFollowUpEncounterDTO.sSelectedAssessment = string.Empty;
        //                    }
        //                    else if (objFollowUpEncounterDTO.FillAssessment.Assessment.Count == 0 && objFollowUpEncounterDTO.FillAssessment.General_Notes.Count == 0 && (Pre_Assesment.Count > 0 || Pre_AssesmentGeneralNotes.Count > 0) && Is_From_CC_Load)
        //                    {
        //                        objFollowUpEncounterDTO.bSelectedAssessment = true;
        //                    }
        //                    else if (Cur_Assesment.Count == 0 && Cur_AssesmentGeneralNotes.Count == 0 && (Pre_Assesment.Count > 0 || Pre_AssesmentGeneralNotes.Count > 0))
        //                    {
        //                        objFollowUpEncounterDTO.bSelectedAssessment = false;
        //                        objFollowUpEncounterDTO.sSelectedAssessment = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    else if ((Cur_Assesment.Count > 0 || Cur_AssesmentGeneralNotes.Count > 0) && Pre_Assesment.Count == 0 && Pre_AssesmentGeneralNotes.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bSelectedAssessment = true;
        //                        objFollowUpEncounterDTO.sSelectedAssessment = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;
        //                    }
        //                    else if ((Cur_Assesment.Count > 0 || Cur_AssesmentGeneralNotes.Count > 0) && (Pre_Assesment.Count > 0 || Pre_AssesmentGeneralNotes.Count > 0))
        //                    {
        //                        if (CheckDuplicate_ASSESMENT(Cur_Assesment, Cur_AssesmentGeneralNotes, Pre_Assesment, Pre_AssesmentGeneralNotes))
        //                        {
        //                            objFollowUpEncounterDTO.bSelectedAssessment = false;
        //                            objFollowUpEncounterDTO.sSelectedAssessment = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                        }
        //                        else
        //                        {
        //                            objFollowUpEncounterDTO.bSelectedAssessment = true;
        //                            objFollowUpEncounterDTO.sSelectedAssessment = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;
        //                        }
        //                    }

        //                    if (Is_From_CC_Load)
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if ((Cur_Assesment.Count > 0 || Cur_AssesmentGeneralNotes.Count > 0) || (Cur_Assesment.Count == 0 && Pre_Assesment.Count == 0 && Cur_AssesmentGeneralNotes.Count == 0 && Pre_AssesmentGeneralNotes.Count == 0))
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if ((Cur_Assesment.Count == 0 && Cur_AssesmentGeneralNotes.Count == 0) && (Pre_Assesment.Count > 0 || Pre_AssesmentGeneralNotes.Count > 0))
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    else
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    #endregion

        //                    #region Assessment Ruledout
        //                    ICriteria Ruledout_result = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Assessment_Type", "Ruled Out"));
        //                    objFollowUpEncounterDTO.Ruledout = Ruledout_result.List<Assessment>();
        //                    ICriteria Notes_result = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Ruled Out Assessment"));
        //                    objFollowUpEncounterDTO.Ruledout_general_Notes_List = Notes_result.List<GeneralNotes>();

        //                    if (objFollowUpEncounterDTO.Ruledout.Count == 0 && objFollowUpEncounterDTO.Ruledout_general_Notes_List.Count == 0 && pEnc != 0 && Is_From_CC_Load == false)
        //                    {
        //                        ICriteria Ruledout_result_Previous = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Assessment_Type", "Ruled Out"));
        //                        objFollowUpEncounterDTO.Ruledout = Ruledout_result_Previous.List<Assessment>();
        //                        ICriteria Notes_result_Previous = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Ruled Out Assessment"));
        //                        objFollowUpEncounterDTO.Ruledout_general_Notes_List = Notes_result_Previous.List<GeneralNotes>();


        //                        if (objFollowUpEncounterDTO.Ruledout.Count != 0 || objFollowUpEncounterDTO.Ruledout_general_Notes_List.Count != 0)
        //                            objFollowUpEncounterDTO.SaveEnable = true;



        //                        if ((objFollowUpEncounterDTO.Ruledout.Count != 0 || objFollowUpEncounterDTO.Ruledout_general_Notes_List.Count != 0) && Is_Save_button_click)
        //                        {

        //                            IList<Assessment> AssessmentRuledSaveList = new List<Assessment>();
        //                            GeneralNotes objRuledGeneralNotes = new GeneralNotes();
        //                            Assessment objAssessmentRuled;
        //                            IList<Assessment> Assessment_pEnc = objFollowUpEncounterDTO.Ruledout;

        //                            for (int i = 0; i < Assessment_pEnc.Count; i++)
        //                            {
        //                                objAssessmentRuled = new Assessment();
        //                                objAssessmentRuled.Encounter_ID = ulEncounterID;
        //                                objAssessmentRuled.Human_ID = Assessment_pEnc[i].Human_ID;
        //                                objAssessmentRuled.Physician_ID = uPhysicianID;
        //                                objAssessmentRuled.ICD_9 = Assessment_pEnc[i].ICD_9;
        //                                objAssessmentRuled.ICD_Description = Assessment_pEnc[i].ICD_Description;
        //                                objAssessmentRuled.Primary_Diagnosis = Assessment_pEnc[i].Primary_Diagnosis;
        //                                objAssessmentRuled.Diagnosis_Source = Assessment_pEnc[i].Diagnosis_Source;
        //                                objAssessmentRuled.Chronic_Problem = Assessment_pEnc[i].Chronic_Problem;
        //                                objAssessmentRuled.Assessment_Type = Assessment_pEnc[i].Assessment_Type;
        //                                objAssessmentRuled.Assessment_Status = Assessment_pEnc[i].Assessment_Status;
        //                                objAssessmentRuled.Assessment_Notes = Assessment_pEnc[i].Assessment_Notes;
        //                                objAssessmentRuled.Created_By = Current_UserName;
        //                                objAssessmentRuled.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objAssessmentRuled.Modified_By = Current_UserName;
        //                                objAssessmentRuled.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                AssessmentRuledSaveList.Add(objAssessmentRuled);
        //                            }

        //                            ICriteria Notes_result_pEnc = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Ruled Out Assessment"));
        //                            IList<GeneralNotes> Notes_pEnc = Notes_result_pEnc.List<GeneralNotes>();
        //                            for (int i = 0; i < Notes_pEnc.Count; i++)
        //                            {
        //                                objRuledGeneralNotes.Encounter_ID = ulEncounterID;
        //                                objRuledGeneralNotes.Human_ID = Notes_pEnc[i].Human_ID;
        //                                objRuledGeneralNotes.Parent_Field = Notes_pEnc[i].Parent_Field;
        //                                objRuledGeneralNotes.Name_Of_The_Field = Notes_pEnc[i].Name_Of_The_Field;
        //                                objRuledGeneralNotes.Notes = Notes_pEnc[i].Notes;
        //                                objRuledGeneralNotes.Created_By = Current_UserName;
        //                                objRuledGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objRuledGeneralNotes.Modified_By = Current_UserName;
        //                                objRuledGeneralNotes.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                            }

        //                            IList<Assessment> AssessmentRuledUpdateList = new List<Assessment>();
        //                            IList<Assessment> AssessmentRuledDeleteList = new List<Assessment>();
        //                            AssessmentManager objAssessmentManager = new AssessmentManager();
        //                            FillAssessmentRuledOut Result = objAssessmentManager.BatchOperationsToAssessmentFromRuledOut(AssessmentRuledSaveList, AssessmentRuledUpdateList, AssessmentRuledDeleteList, ulEncounterID, string.Empty, objRuledGeneralNotes);
        //                            ICriteria Ruledout_result_Enc = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Assessment_Type", "Ruled Out"));
        //                            objFollowUpEncounterDTO.Ruledout = Ruledout_result_Enc.List<Assessment>();
        //                            ICriteria Ruledout_Notes_result = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Ruled Out Assessment"));
        //                            objFollowUpEncounterDTO.Ruledout_general_Notes_List = Ruledout_Notes_result.List<GeneralNotes>();
        //                            objFollowUpEncounterDTO.FillAssessmentRuledOut = objAssessmentManager.GetAssessmentUsingEncounterIDForRuledOut(pEnc, uPhysicianID);

        //                        }
        //                    }



        //                    IList<Assessment> Cur_Ruledout_result = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Assessment_Type", "Ruled Out")).List<Assessment>();
        //                    IList<GeneralNotes> Cur_Notes_result = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Ruled Out Assessment")).List<GeneralNotes>();

        //                    IList<Assessment> Pre_Ruledout_result = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Assessment_Type", "Ruled Out")).List<Assessment>();
        //                    IList<GeneralNotes> Pre_Notes_result = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Parent_Field", "Ruled Out Assessment")).List<GeneralNotes>();


        //                    //if(Cur_Notes_result.Count==0 && Cur_Ruledout_result.Count==0)
        //                    //{
        //                    //    objFollowUpEncounterDTO.bRuledOutAssessment = false;
        //                    //    objFollowUpEncounterDTO.sRuledOutAssessment = "Captured On : " + objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //}
        //                    //else if (CheckDuplicate_ASSESMENT_Ruled(Cur_Ruledout_result, Cur_Notes_result, Pre_Ruledout_result, Pre_Notes_result))
        //                    //{
        //                    //    objFollowUpEncounterDTO.bRuledOutAssessment = true;
        //                    //    if (Cur_Ruledout_result.Count > 0 || Cur_Notes_result.Count > 0)
        //                    //        objFollowUpEncounterDTO.sRuledOutAssessment = "Captured On : " + objFollowUpEncounterDTO.EncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //    else
        //                    //        objFollowUpEncounterDTO.sRuledOutAssessment = "Captured On : " + objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //}
        //                    //else
        //                    //{
        //                    //    objFollowUpEncounterDTO.bRuledOutAssessment = false;
        //                    //    objFollowUpEncounterDTO.sRuledOutAssessment = "Captured On : " + objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //}

        //                    //if((Cur_Ruledout_result.Count==0|| Cur_Notes_result.Count==0)&&( Pre_Ruledout_result.Count>0|| Pre_Notes_result.Count>0))
        //                    //    objFollowUpEncounterDTO.btnList.Add(true);
        //                    //else
        //                    //    objFollowUpEncounterDTO.btnList.Add(false);



        //                    if (Cur_Ruledout_result.Count == 0 && Pre_Ruledout_result.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bRuledOutAssessment = false;
        //                        //objFollowUpEncounterDTO.sRuledOutAssessment = string.Empty;
        //                    }
        //                    else if (objFollowUpEncounterDTO.Ruledout.Count == 0 && Pre_Ruledout_result.Count > 0 && Is_From_CC_Load)
        //                    {
        //                        objFollowUpEncounterDTO.bRuledOutAssessment = true;
        //                    }
        //                    else if (Cur_Ruledout_result.Count == 0 && Pre_Ruledout_result.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.bRuledOutAssessment = false;
        //                        objFollowUpEncounterDTO.sRuledOutAssessment = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    else if (Cur_Ruledout_result.Count > 0 && Pre_Ruledout_result.Count == 0)
        //                    {

        //                        objFollowUpEncounterDTO.bRuledOutAssessment = true;
        //                        objFollowUpEncounterDTO.sRuledOutAssessment = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;


        //                    }
        //                    else if (Cur_Ruledout_result.Count > 0 && Pre_Ruledout_result.Count > 0)
        //                    {
        //                        //CheckDuplicate_ASSESMENT
        //                        //if (CheckDuplicate_ASSESMENT_Ruled(Cur_Ruledout_result, Cur_Notes_result, Pre_Ruledout_result, Pre_Notes_result))
        //                        if (CheckDuplicate_ASSESMENT(Cur_Ruledout_result, Cur_Notes_result, Pre_Ruledout_result, Pre_Notes_result))
        //                        {
        //                            objFollowUpEncounterDTO.bRuledOutAssessment = false;
        //                            objFollowUpEncounterDTO.sRuledOutAssessment = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                        }
        //                        else
        //                        {
        //                            objFollowUpEncounterDTO.bRuledOutAssessment = true;
        //                            objFollowUpEncounterDTO.sRuledOutAssessment = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;

        //                        }

        //                    }

        //                    if (Is_From_CC_Load)
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_Ruledout_result.Count > 0 || (Cur_Ruledout_result.Count == 0 && Pre_Ruledout_result.Count == 0))
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_Ruledout_result.Count == 0 && Pre_Ruledout_result.Count > 0)
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    else
        //                        objFollowUpEncounterDTO.btnList.Add(false);

        //                    objFollowUpEncounterDTO.FillAssessmentRuledOut.Assessment = objFollowUpEncounterDTO.Ruledout;
        //                    objFollowUpEncounterDTO.FillAssessmentRuledOut.General_Notes = objFollowUpEncounterDTO.Ruledout_general_Notes_List;

        //                    #endregion
        //                }
        //                else if (FieldLookupList[m].Trim() == "Plan")
        //                {

        //                    #region Treatment Plan
        //                    ICriteria TPlan_result = iMySession.CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", ulEncounterID)).Add(Expression.Eq("Human_Id", uHumanID)).Add(Expression.Eq("Plan_Type", "PLAN"));
        //                    objFollowUpEncounterDTO.TreatmentPlan = TPlan_result.List<TreatmentPlan>();

        //                    ICriteria TPlan_result1 = iMySession.CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", ulEncounterID)).Add(Expression.Eq("Human_Id", uHumanID));
        //                    objFollowUpEncounterDTO.TreatmentPlanAndAssessment = TPlan_result1.List<TreatmentPlan>();

        //                    //foreach (TreatmentPlan tempPlan in objFollowUpEncounterDTO.TreatmentPlanAndAssessment)
        //                    //{
        //                    //    if (tempPlan.Plan_Type.ToUpper() == "ASSESSMENT")
        //                    //        objFollowUpEncounterDTO.TreatmentPlan.Add(tempPlan);
        //                    //}

        //                    if (objFollowUpEncounterDTO.TreatmentPlan.Count == 0 && pEnc != 0 && Is_From_CC_Load == false)
        //                    {
        //                        ICriteria TPlan_result_previous = iMySession.CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", pEnc)).Add(Expression.Eq("Human_Id", uHumanID)).Add(Expression.Eq("Plan_Type", "PLAN"));
        //                        objFollowUpEncounterDTO.TreatmentPlan = TPlan_result_previous.List<TreatmentPlan>();

        //                        ICriteria result = iMySession.CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", ulEncounterID)).Add(Expression.Eq("Human_Id", uHumanID));
        //                        objFollowUpEncounterDTO.TreatmentPlanAndAssessment = result.List<TreatmentPlan>();

        //                        foreach (TreatmentPlan tempPlan in objFollowUpEncounterDTO.TreatmentPlanAndAssessment)
        //                        {
        //                            if (tempPlan.Plan_Type.ToUpper() == "ASSESSMENT")
        //                                objFollowUpEncounterDTO.TreatmentPlan.Add(tempPlan);
        //                        }

        //                        if (objFollowUpEncounterDTO.TreatmentPlan.Count != 0)
        //                            objFollowUpEncounterDTO.SaveEnable = true;


        //                        if (objFollowUpEncounterDTO.TreatmentPlan.Count > 0 && Is_Save_button_click)
        //                        {

        //                            IList<TreatmentPlan> TreatmentPlanSaveList = new List<TreatmentPlan>();
        //                            TreatmentPlan objTreatmentPlan;
        //                            for (int i = 0; i < objFollowUpEncounterDTO.TreatmentPlan.Count; i++)
        //                            {
        //                                objTreatmentPlan = new TreatmentPlan();
        //                                objTreatmentPlan.Encounter_Id = ulEncounterID;
        //                                objTreatmentPlan.Physician_Id = uPhysicianID;
        //                                objTreatmentPlan.Human_ID = objFollowUpEncounterDTO.TreatmentPlan[i].Human_ID;
        //                                objTreatmentPlan.Plan = objFollowUpEncounterDTO.TreatmentPlan[i].Plan;
        //                                objTreatmentPlan.Addendum_Plan = objFollowUpEncounterDTO.TreatmentPlan[i].Addendum_Plan;
        //                                objTreatmentPlan.Created_By = Current_UserName;
        //                                objTreatmentPlan.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objTreatmentPlan.Modified_By = Current_UserName;
        //                                objTreatmentPlan.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objTreatmentPlan.Plan_Type = objFollowUpEncounterDTO.TreatmentPlan[i].Plan_Type;
        //                                TreatmentPlanSaveList.Add(objTreatmentPlan);
        //                            }
        //                            //var d = TreatmentPlanSaveList.Select(a => a.Plan + "test");
        //                            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
        //                            objFollowUpEncounterDTO.TreatmentPlan = objTreatmentPlanManager.SaveUpdateDeleteTreatmentPlanList(TreatmentPlanSaveList, null, null, string.Empty, ulEncounterID, uHumanID);
        //                            ICriteria TPlan_results = iMySession.CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", ulEncounterID)).Add(Expression.Eq("Human_Id", uHumanID));
        //                            objFollowUpEncounterDTO.TreatmentPlanAndAssessment = TPlan_results.List<TreatmentPlan>();
        //                        }

        //                    }


        //                    IList<TreatmentPlan> Cur_plan = iMySession.CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", ulEncounterID)).Add(Expression.Eq("Human_Id", uHumanID)).Add(Expression.Eq("Plan_Type", "PLAN")).List<TreatmentPlan>();
        //                    IList<TreatmentPlan> Pre_plan = iMySession.CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", pEnc)).Add(Expression.Eq("Human_Id", uHumanID)).Add(Expression.Eq("Plan_Type", "PLAN")).List<TreatmentPlan>();


        //                    if (Cur_plan.Count == 0 && Pre_plan.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bGeneralPlan = false;
        //                        //objFollowUpEncounterDTO.sGeneralPlan = string.Empty;
        //                    }
        //                    else if (objFollowUpEncounterDTO.TreatmentPlan.Count == 0 && Pre_plan.Count > 0 && Is_From_CC_Load)
        //                    {
        //                        objFollowUpEncounterDTO.bGeneralPlan = true;
        //                    }
        //                    else if (Cur_plan.Count == 0 && Pre_plan.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.bGeneralPlan = false;
        //                        objFollowUpEncounterDTO.sGeneralPlan = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    else if (Cur_plan.Count > 0 && Pre_plan.Count == 0)
        //                    {

        //                        objFollowUpEncounterDTO.bGeneralPlan = true;
        //                        objFollowUpEncounterDTO.sGeneralPlan = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;


        //                    }
        //                    else if (Cur_plan.Count > 0 && Pre_plan.Count > 0)
        //                    {

        //                        if (CheckDuplicate_TreatmentPlan(Cur_plan, Pre_plan))
        //                        {
        //                            objFollowUpEncounterDTO.bGeneralPlan = false;
        //                            objFollowUpEncounterDTO.sGeneralPlan = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                        }
        //                        else
        //                        {
        //                            objFollowUpEncounterDTO.bGeneralPlan = true;
        //                            objFollowUpEncounterDTO.sGeneralPlan = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;

        //                        }

        //                    }

        //                    //if (Cur_plan.Count == 0)
        //                    //{
        //                    //    //if (Pre_plan.Count == 0)
        //                    //    //{
        //                    //    //    objFollowUpEncounterDTO.btnList.Add(false);
        //                    //    //}
        //                    //    //else
        //                    //    //{
        //                    //    //    objFollowUpEncounterDTO.btnList.Add(true);
        //                    //    //}
        //                    //    objFollowUpEncounterDTO.bGeneralPlan = false;
        //                    //    objFollowUpEncounterDTO.sGeneralPlan = "Captured On : " + objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //}
        //                    //else if (CheckDuplicate_TreatmentPlan(Cur_plan, Pre_plan))
        //                    //{
        //                    //    //objFollowUpEncounterDTO.btnList.Add(false);
        //                    //    objFollowUpEncounterDTO.bGeneralPlan = true;
        //                    //    if (Cur_plan.Count > 0)
        //                    //        objFollowUpEncounterDTO.sGeneralPlan = "Captured On : " + objFollowUpEncounterDTO.EncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //    else
        //                    //        objFollowUpEncounterDTO.sGeneralPlan = "Captured On : " + objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //}
        //                    //else
        //                    //{
        //                    //    //objFollowUpEncounterDTO.btnList.Add(false);
        //                    //    objFollowUpEncounterDTO.bGeneralPlan = false;
        //                    //    objFollowUpEncounterDTO.sGeneralPlan = "Captured On : " + objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //}

        //                    ////if (Cur_plan.Count == 0 && Pre_plan.Count>0)
        //                    ////    objFollowUpEncounterDTO.bGeneralPlan = true;

        //                    if (Is_From_CC_Load)
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_plan.Count > 0 || (Cur_plan.Count == 0 && Pre_plan.Count == 0))
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_plan.Count == 0 && Pre_plan.Count > 0)
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    else
        //                        objFollowUpEncounterDTO.btnList.Add(false);



        //                    #endregion

        //                    #region Care Plan
        //                    objFollowUpEncounterDTO.CarePlan = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).List<CarePlan>();
        //                    //objFollowUpEncounterDTO.CarePlan = GetCSCarePlanList(ulEncounterID);
        //                    CarePlanManager objCarePlanMgr = new CarePlanManager();
        //                    objFollowUpEncounterDTO.FillPlanDTO = objCarePlanMgr.GetPlanCareFromServerforThin(ulEncounterID, uHumanID, Sex, finalYear);

        //                    if (objFollowUpEncounterDTO.CarePlan.Count == 0 && pEnc != 0 && Is_From_CC_Load == false)
        //                    {
        //                        //objFollowUpEncounterDTO.CarePlan = GetCSCarePlanList(pEnc);
        //                        objFollowUpEncounterDTO.CarePlan = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Encounter_ID", pEnc)).List<CarePlan>();
        //                        IList<CarePlan> CarePlansavelst = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).List<CarePlan>();


        //                        if (objFollowUpEncounterDTO.CarePlan.Count != 0)
        //                        {
        //                            objFollowUpEncounterDTO.SaveEnable = true;
        //                            //added by Pravin-03-11-2012
        //                            CarePlanManager objCarePlanManagers = new CarePlanManager();
        //                            objFollowUpEncounterDTO.FillPlanDTO = objCarePlanManagers.GetPlanCareFromServerforThin(pEnc, uHumanID, Sex, finalYear);
        //                        }


        //                        if (objFollowUpEncounterDTO.CarePlan.Count != 0 && Is_Save_button_click)
        //                        {
        //                            IList<CarePlan> CarePlanSaveList = new List<CarePlan>();
        //                            CarePlan objCarePlan;
        //                            for (int i = 0; i < CarePlansavelst.Count; i++)
        //                            {
        //                                objCarePlan = new CarePlan();
        //                                objCarePlan.Care_Plan_Lookup_ID = CarePlansavelst[i].Care_Plan_Lookup_ID;
        //                                objCarePlan.Encounter_ID = ulEncounterID;
        //                                objCarePlan.Physician_ID = uPhysicianID;
        //                                objCarePlan.Human_ID = CarePlansavelst[i].Human_ID;
        //                                objCarePlan.Care_Name = CarePlansavelst[i].Care_Name;
        //                                objCarePlan.Care_Name_Value = CarePlansavelst[i].Care_Name_Value;
        //                                objCarePlan.Status = CarePlansavelst[i].Status;
        //                                objCarePlan.Care_Plan_Notes = CarePlansavelst[i].Care_Plan_Notes;
        //                                objCarePlan.Plan_Date = CarePlansavelst[i].Plan_Date;
        //                                objCarePlan.Status_Value = CarePlansavelst[i].Status_Value;
        //                                objCarePlan.Created_By = Current_UserName;
        //                                objCarePlan.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objCarePlan.Modified_By = Current_UserName;
        //                                objCarePlan.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                CarePlanSaveList.Add(objCarePlan);
        //                            }
        //                            CarePlanManager objCarePlanManager = new CarePlanManager();
        //                            IList<CarePlanDTO> Resultlst = new List<CarePlanDTO>();
        //                            Resultlst = objCarePlanManager.SaveCarePlan(CarePlanSaveList, "", 0, string.Empty);
        //                            objFollowUpEncounterDTO.CarePlan = GetCSCarePlanList(ulEncounterID);

        //                            objFollowUpEncounterDTO.FillPlanDTO = objCarePlanManager.GetPlanCareFromServerforThin(ulEncounterID, uHumanID, Sex, finalYear);
        //                        }

        //                    }



        //                    IList<CarePlan> Cur_careplan = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).List<CarePlan>();
        //                    IList<CarePlan> Pre_careplan = iMySession.CreateCriteria(typeof(CarePlan)).Add(Expression.Eq("Encounter_ID", pEnc)).List<CarePlan>();


        //                    if (Cur_careplan.Count == 0 && Pre_careplan.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bIndividualizedCarePlan = false;
        //                        //objFollowUpEncounterDTO.sIndividualizedCarePlan = string.Empty;
        //                    }
        //                    else if (objFollowUpEncounterDTO.CarePlan.Count == 0 && Pre_careplan.Count > 0 && Is_From_CC_Load)
        //                    {
        //                        objFollowUpEncounterDTO.bIndividualizedCarePlan = true;
        //                    }
        //                    else if (Cur_careplan.Count == 0 && Pre_careplan.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.bIndividualizedCarePlan = false;
        //                        objFollowUpEncounterDTO.sIndividualizedCarePlan = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    else if (Cur_careplan.Count > 0 && Pre_careplan.Count == 0)// && Cur_careplan.Any(a => a.Status.Trim() != string.Empty || a.Care_Plan_Notes !=string.Empty))
        //                    {

        //                        objFollowUpEncounterDTO.bIndividualizedCarePlan = true;
        //                        objFollowUpEncounterDTO.sIndividualizedCarePlan = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;


        //                    }
        //                    else if (Cur_careplan.Count > 0 && Pre_careplan.Count > 0)
        //                    {

        //                        if (!CheckDuplicate_CarePlan(Cur_careplan, Pre_careplan))
        //                        {
        //                            objFollowUpEncounterDTO.bIndividualizedCarePlan = false;
        //                            objFollowUpEncounterDTO.sIndividualizedCarePlan = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                        }
        //                        else
        //                        {
        //                            objFollowUpEncounterDTO.bIndividualizedCarePlan = true;
        //                            objFollowUpEncounterDTO.sIndividualizedCarePlan = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;

        //                        }

        //                    }

        //                    //if (Cur_careplan.Count == 0)
        //                    //{

        //                    //    objFollowUpEncounterDTO.bIndividualizedCarePlan = false;
        //                    //    objFollowUpEncounterDTO.sIndividualizedCarePlan = "Captured On : " + objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //}
        //                    //else if (CheckDuplicate_CarePlan(Cur_careplan, Pre_careplan))
        //                    //{
        //                    //    objFollowUpEncounterDTO.bIndividualizedCarePlan = true;

        //                    //    if (Cur_careplan.Count > 0)
        //                    //        objFollowUpEncounterDTO.sIndividualizedCarePlan = "Captured On : " + objFollowUpEncounterDTO.EncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //    else
        //                    //        objFollowUpEncounterDTO.sIndividualizedCarePlan = "Captured On : " + objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");
        //                    //}
        //                    //else
        //                    //{
        //                    //    //objFollowUpEncounterDTO.btnList.Add(false);
        //                    //    objFollowUpEncounterDTO.bIndividualizedCarePlan = false;
        //                    //    objFollowUpEncounterDTO.sIndividualizedCarePlan = "Captured On : " + objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service.ToLocalTime().ToString("dd-MMM-yyyy hh:mm:ss tt");

        //                    //}

        //                    if (Is_From_CC_Load)
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_careplan.Count > 0 || (Cur_careplan.Count == 0 && Pre_careplan.Count == 0))
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_careplan.Count == 0 && Pre_careplan.Count > 0)
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    else
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    #endregion

        //                    #region PreventiveScreen Plan
        //                    objFollowUpEncounterDTO.PreventiveScreen = iMySession.CreateCriteria(typeof(PreventiveScreen)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).List<PreventiveScreen>();
        //                    //objFollowUpEncounterDTO.PreventiveScreen = GetCSPreventivePlanList(ulEncounterID);
        //                    PreventiveScreenManager objPreventiveScreenMgr = new PreventiveScreenManager();
        //                    objFollowUpEncounterDTO.FillPreventiveScreen = objPreventiveScreenMgr.GetPreventiveScreenFromServer(ulEncounterID, uHumanID);


        //                    if (objFollowUpEncounterDTO.PreventiveScreen.Count == 0 && pEnc != 0 && Is_From_CC_Load == false)
        //                    {
        //                        objFollowUpEncounterDTO.PreventiveScreen = iMySession.CreateCriteria(typeof(PreventiveScreen)).Add(Expression.Eq("Encounter_ID", pEnc)).List<PreventiveScreen>();
        //                        //objFollowUpEncounterDTO.PreventiveScreen = GetCSPreventivePlanList(pEnc);
        //                        IList<PreventiveScreen> PreventiveScreensavelst = iMySession.CreateCriteria(typeof(PreventiveScreen)).Add(Expression.Eq("Encounter_ID", pEnc)).Add(Expression.Eq("Human_ID", uHumanID)).List<PreventiveScreen>();

        //                        if (objFollowUpEncounterDTO.PreventiveScreen.Count != 0)
        //                        {
        //                            objFollowUpEncounterDTO.SaveEnable = true;
        //                            PreventiveScreenManager objPreventiveScreenManager = new PreventiveScreenManager();
        //                            objFollowUpEncounterDTO.FillPreventiveScreen = objPreventiveScreenManager.GetPreventiveScreenFromServer(pEnc, uHumanID);
        //                        }

        //                        if (objFollowUpEncounterDTO.PreventiveScreen.Count != 0 && Is_Save_button_click)
        //                        {
        //                            IList<PreventiveScreen> SaveList = new List<PreventiveScreen>();
        //                            PreventiveScreen objPreventiveScreen;
        //                            for (int i = 0; i < PreventiveScreensavelst.Count; i++)
        //                            {
        //                                objPreventiveScreen = new PreventiveScreen();
        //                                objPreventiveScreen.Human_ID = PreventiveScreensavelst[i].Human_ID;
        //                                objPreventiveScreen.Encounter_ID = ulEncounterID;
        //                                objPreventiveScreen.Physician_ID = uPhysicianID;
        //                                objPreventiveScreen.Preventive_Screening_Lookup_ID = PreventiveScreensavelst[i].Preventive_Screening_Lookup_ID;
        //                                objPreventiveScreen.Preventive_Service = PreventiveScreensavelst[i].Preventive_Service;
        //                                objPreventiveScreen.Preventive_Service_Value = PreventiveScreensavelst[i].Preventive_Service_Value;
        //                                objPreventiveScreen.Status = PreventiveScreensavelst[i].Status;
        //                                objPreventiveScreen.Preventive_Screening_Notes = PreventiveScreensavelst[i].Preventive_Screening_Notes;
        //                                objPreventiveScreen.Created_By = Current_UserName;
        //                                objPreventiveScreen.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objPreventiveScreen.Modified_By = Current_UserName;
        //                                objPreventiveScreen.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                SaveList.Add(objPreventiveScreen);
        //                            }
        //                            PreventiveScreenManager objPreventiveScreenManager = new PreventiveScreenManager();
        //                            IList<PreventiveScreenDTO> Resultlist = new List<PreventiveScreenDTO>();
        //                            Resultlist = objPreventiveScreenManager.SavePreventiveScreen(SaveList, string.Empty);
        //                            objFollowUpEncounterDTO.PreventiveScreen = GetCSPreventivePlanList(ulEncounterID);
        //                            objFollowUpEncounterDTO.FillPreventiveScreen = objPreventiveScreenManager.GetPreventiveScreenFromServer(ulEncounterID, uHumanID);
        //                        }
        //                    }


        //                    IList<PreventiveScreen> Cur_Preventiveplan = iMySession.CreateCriteria(typeof(PreventiveScreen)).Add(Expression.Eq("Encounter_ID", ulEncounterID)).List<PreventiveScreen>();
        //                    IList<PreventiveScreen> Pre_Preventiveplan = iMySession.CreateCriteria(typeof(PreventiveScreen)).Add(Expression.Eq("Encounter_ID", pEnc)).List<PreventiveScreen>();
        //                    //New 
        //                    if (Cur_Preventiveplan.Count == 0 && Pre_Preventiveplan.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bPreventiveScreeningPlan = false;
        //                        //objFollowUpEncounterDTO.sPreventiveScreeningPlan = string.Empty;
        //                    }
        //                    else if (objFollowUpEncounterDTO.FillPreventiveScreen.Count == 0 && Pre_Preventiveplan.Count > 0 && Is_From_CC_Load)
        //                    {
        //                        objFollowUpEncounterDTO.bPreventiveScreeningPlan = true;
        //                    }
        //                    else if (Cur_Preventiveplan.Count == 0 && Pre_Preventiveplan.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.bPreventiveScreeningPlan = false;
        //                        objFollowUpEncounterDTO.sPreventiveScreeningPlan = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    else if (Cur_Preventiveplan.Count > 0 && Pre_Preventiveplan.Count == 0)
        //                    {

        //                        objFollowUpEncounterDTO.bPreventiveScreeningPlan = true;
        //                        objFollowUpEncounterDTO.sPreventiveScreeningPlan = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;


        //                    }
        //                    else if (Cur_Preventiveplan.Count > 0 && Pre_Preventiveplan.Count > 0)
        //                    {

        //                        if (!CheckDuplicate_PreventiveScreen(Cur_Preventiveplan, Pre_Preventiveplan))
        //                        {
        //                            objFollowUpEncounterDTO.bPreventiveScreeningPlan = false;
        //                            objFollowUpEncounterDTO.sPreventiveScreeningPlan = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                        }
        //                        else
        //                        {
        //                            objFollowUpEncounterDTO.bPreventiveScreeningPlan = true;
        //                            objFollowUpEncounterDTO.sPreventiveScreeningPlan = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;

        //                        }

        //                    }

        //                    if (Is_From_CC_Load)
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_Preventiveplan.Count > 0 || (Cur_Preventiveplan.Count == 0 && Pre_Preventiveplan.Count == 0))
        //                        objFollowUpEncounterDTO.btnList.Add(false);
        //                    else if (Cur_Preventiveplan.Count == 0 && Pre_Preventiveplan.Count > 0)
        //                        objFollowUpEncounterDTO.btnList.Add(true);
        //                    else
        //                        objFollowUpEncounterDTO.btnList.Add(false);

        //                    #endregion
        //                }
        //                else if (FieldLookupList[m].Trim() == "Allergies")
        //                {
        //                    #region Non Drug Allergy

        //                    var Isduplicate = true;

        //                    ICriteria NDA_result = iMySession.CreateCriteria(typeof(NonDrugAllergy))
        //                                           .Add(Expression.Eq("Encounter_Id", ulEncounterID))
        //                                           .AddOrder(Order.Desc("Modified_Date_And_Time"));

        //                    objFollowUpEncounterDTO.NonDrugHistory.NonDrugList = NDA_result.List<NonDrugAllergy>();

        //                    ICriteria Query_Result_Notes = iMySession.CreateCriteria(typeof(GeneralNotes))
        //                        .Add(Expression.Eq("Parent_Field", "Non Drug Allergy History"))
        //                        .Add(Expression.Eq("Encounter_ID", ulEncounterID))
        //                        .AddOrder(Order.Desc("Modified_Date_And_Time"));

        //                    var ilstGeneralNotes = Query_Result_Notes.List<GeneralNotes>();

        //                    if (objFollowUpEncounterDTO.NonDrugHistory.NonDrugList.Count == 0
        //                        && ilstGeneralNotes.Count == 0 && pEnc != 0)
        //                    {
        //                        ICriteria NDA_result_pEnc = iMySession.CreateCriteria(typeof(NonDrugAllergy)).Add(Expression.Eq("Encounter_Id", pEnc));
        //                        objFollowUpEncounterDTO.NonDrugHistory.NonDrugList = NDA_result_pEnc.List<NonDrugAllergy>();

        //                        Query_Result_Notes = iMySession.CreateCriteria(typeof(GeneralNotes))
        //                        .Add(Expression.Eq("Parent_Field", "Non Drug Allergy History"))
        //                        .Add(Expression.Eq("Encounter_ID", pEnc))
        //                        .AddOrder(Order.Desc("Modified_Date_And_Time"));

        //                        ilstGeneralNotes = Query_Result_Notes.List<GeneralNotes>();

        //                        if (objFollowUpEncounterDTO.NonDrugHistory.NonDrugList.Count != 0)
        //                            objFollowUpEncounterDTO.SaveEnable = true;

        //                        if (Is_Save_button_click && objFollowUpEncounterDTO.NonDrugHistory.NonDrugList.Count != 0)
        //                        {
        //                            IList<NonDrugAllergy> NDASaveList = new List<NonDrugAllergy>();
        //                            NonDrugAllergy objNDA;

        //                            for (int i = 0; i < objFollowUpEncounterDTO.NonDrugHistory.NonDrugList.Count; i++)
        //                            {
        //                                objNDA = new NonDrugAllergy();
        //                                objNDA.Human_ID = objFollowUpEncounterDTO.NonDrugHistory.NonDrugList[i].Human_ID;
        //                                objNDA.Non_Drug_Allergy_History_Info = objFollowUpEncounterDTO.NonDrugHistory.NonDrugList[i].Non_Drug_Allergy_History_Info;
        //                                objNDA.Is_Present = objFollowUpEncounterDTO.NonDrugHistory.NonDrugList[i].Is_Present;
        //                                objNDA.Description = objFollowUpEncounterDTO.NonDrugHistory.NonDrugList[i].Description;
        //                                objNDA.Created_By = Current_UserName;
        //                                objNDA.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objNDA.Modified_By = Current_UserName;
        //                                objNDA.Modified_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                                objNDA.Encounter_Id = ulEncounterID;
        //                                NDASaveList.Add(objNDA);
        //                            }
        //                            var objGeneralNotes = new GeneralNotes();
        //                            objGeneralNotes.Encounter_ID = ilstGeneralNotes[0].Encounter_ID;
        //                            objGeneralNotes.Human_ID = ilstGeneralNotes[0].Human_ID;
        //                            objGeneralNotes.Parent_Field = ilstGeneralNotes[0].Parent_Field;
        //                            objGeneralNotes.Name_Of_The_Field = ilstGeneralNotes[0].Name_Of_The_Field;
        //                            objGeneralNotes.Notes = ilstGeneralNotes[0].Notes;
        //                            objGeneralNotes.Created_By = Current_UserName;
        //                            objGeneralNotes.Created_Date_And_Time = DateTime.Now.ToUniversalTime();
        //                            objGeneralNotes.Modified_By = "";
        //                            objGeneralNotes.Modified_Date_And_Time = DateTime.MinValue;

        //                            NonDrugAllergyManager objNonDrugAllergyManager = new NonDrugAllergyManager();

        //                            objFollowUpEncounterDTO.NonDrugHistory = objNonDrugAllergyManager.SaveUpdateDeleteNonDrugHistory(null, NDASaveList
        //                                , new List<NonDrugAllergy>(), new List<NonDrugAllergy>(), uHumanID, objGeneralNotes, string.Empty, ulEncounterID);

        //                            objFollowUpEncounterDTO.NonDrugHistory.GeneralNotesObject = objGeneralNotes;
        //                        }
        //                    }

        //                    IList<NonDrugAllergy> Current_NDA = iMySession.CreateCriteria(typeof(NonDrugAllergy))
        //                        .Add(Expression.Eq("Encounter_Id", ulEncounterID))
        //                        .Add(Expression.Eq("Human_ID", uHumanID))
        //                        .AddOrder(Order.Desc("Modified_Date_And_Time")).List<NonDrugAllergy>();
        //                    IList<NonDrugAllergy> Previous_NDA = iMySession.CreateCriteria(typeof(NonDrugAllergy))
        //                        .Add(Expression.Eq("Encounter_Id", pEnc))
        //                        .Add(Expression.Eq("Human_ID", uHumanID))
        //                        .AddOrder(Order.Desc("Modified_Date_And_Time")).List<NonDrugAllergy>();





        //                    IList<GeneralNotes> Current_GN = iMySession.CreateCriteria(typeof(GeneralNotes))
        //                        .Add(Expression.Eq("Parent_Field", "Non Drug Allergy History"))
        //                        .Add(Expression.Eq("Encounter_ID", ulEncounterID))
        //                        .AddOrder(Order.Desc("Modified_Date_And_Time")).List<GeneralNotes>();

        //                    IList<GeneralNotes> Previous_GN = iMySession.CreateCriteria(typeof(GeneralNotes))
        //                       .Add(Expression.Eq("Parent_Field", "Non Drug Allergy History"))
        //                       .Add(Expression.Eq("Encounter_ID", pEnc))
        //                       .AddOrder(Order.Desc("Modified_Date_And_Time")).List<GeneralNotes>();




        //                    var qryCurrent_GN = from b in Current_GN
        //                                        select new
        //                                        {
        //                                            notes = b.Notes
        //                                        };

        //                    var Cur_GN = qryCurrent_GN.ToList();


        //                    var qryPrevious_GN = from b in Previous_GN
        //                                         select new
        //                                         {
        //                                             notes = b.Notes
        //                                         };

        //                    var Pre_GN = qryPrevious_GN.ToList();

        //                    var qryCurrent_NDA = from b in Current_NDA
        //                                         select new
        //                                         {
        //                                             Non_Drug_Allergy_History_Info = b.Non_Drug_Allergy_History_Info,
        //                                             Description = b.Description,
        //                                             Is_Present = b.Is_Present
        //                                         };

        //                    var CurrentSelected = qryCurrent_NDA.OrderBy(t => t.Non_Drug_Allergy_History_Info).ToList();

        //                    var qryPrevious_NDA = from b in Previous_NDA
        //                                          select new
        //                                          {
        //                                              Non_Drug_Allergy_History_Info = b.Non_Drug_Allergy_History_Info,
        //                                              Description = b.Description,
        //                                              Is_Present = b.Is_Present
        //                                          };

        //                    var PrevSelected = qryPrevious_NDA.OrderBy(t => t.Non_Drug_Allergy_History_Info).ToList();



        //                    Isduplicate = CurrentSelected.Count() == PrevSelected.Count();

        //                    if (Isduplicate)
        //                    {
        //                        Isduplicate = Enumerable.SequenceEqual(PrevSelected, CurrentSelected);
        //                    }
        //                    if (Isduplicate)
        //                    {
        //                        Isduplicate = Pre_GN.Count == Cur_GN.Count;
        //                    }
        //                    if (Isduplicate)
        //                    {
        //                        Isduplicate = Enumerable.SequenceEqual(Pre_GN, Cur_GN);
        //                    }

        //                    if (!Isduplicate)
        //                    {
        //                        objFollowUpEncounterDTO.bNonDrugAllergy = true;
        //                        if (Current_NDA.Count > 0)
        //                            objFollowUpEncounterDTO.sNonDrugAllergy = objFollowUpEncounterDTO.EncounterList[0].Date_of_Service;
        //                        else
        //                            objFollowUpEncounterDTO.sNonDrugAllergy = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }
        //                    else
        //                    {
        //                        objFollowUpEncounterDTO.bNonDrugAllergy = false;
        //                        objFollowUpEncounterDTO.sNonDrugAllergy = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    if (Current_NDA.Count == 0)
        //                    {
        //                        objFollowUpEncounterDTO.bNonDrugAllergy = false;
        //                        objFollowUpEncounterDTO.sNonDrugAllergy = objFollowUpEncounterDTO.PreEncounterList[0].Date_of_Service;
        //                    }

        //                    //if (Current_NDA.Count == 0 && Previous_NDA.Count > 0)
        //                    //{
        //                    //    objFollowUpEncounterDTO.btnList.Add(objFollowUpEncounterDTO.bNonDrugAllergy);
        //                    //}
        //                    //else
        //                    if (ilstGeneralNotes.Count > 0)
        //                    {
        //                        objFollowUpEncounterDTO.NonDrugHistory.GeneralNotesObject = ilstGeneralNotes[0];
        //                    }
        //                    objFollowUpEncounterDTO.btnList.Add(objFollowUpEncounterDTO.bNonDrugAllergy);

        //                    #endregion

        //                    var crit = iMySession.CreateCriteria(typeof(Rcopia_Allergy)).Add(Expression.Eq("Human_ID", uHumanID)).Add(Expression.Eq("Deleted", "N"));
        //                    objFollowUpEncounterDTO.DrugHistory = crit.List<Rcopia_Allergy>();
        //                }
        //            }
        //            ICriteria MasterVitals_Result = iMySession.CreateCriteria(typeof(MasterVitals)).Add(Expression.Eq("Vital_Type", "Standard"));
        //            objFollowUpEncounterDTO.MasterVitals = MasterVitals_Result.List<MasterVitals>();
        //        }
        //        ChiefComplaintsManager chiefmngr = new ChiefComplaintsManager();
        //        objFollowUpEncounterDTO.LoadCC = chiefmngr.FillCC(ulEncounterID);
        //        iMySession.Close();
        //    }
        //    return objFollowUpEncounterDTO;
        //}

        public ulong GetPreviousEncounterIdAlone(ulong ulEncounterID, ulong uHumanID, ulong uPhysicianID)
        {
            ulong pEnc = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    ISQLQuery ResultList = iMySession.CreateSQLQuery("SELECT e.* FROM encounter e WHERE e.Human_ID='" + uHumanID + "' and e.Encounter_Provider_ID='" + uPhysicianID + "' and e.Date_of_Service <>'0001-01-01 00:00:00' and e.Date_of_Service<(select e.Date_of_Service from encounter e where e.Encounter_ID=" + ulEncounterID + ")and e.Encounter_ID<>" + ulEncounterID + " and e.Is_Phone_Encounter <> 'Y'  Order By e.Date_of_Service desc").AddEntity("e", typeof(Encounter));
                    pEnc = ResultList.List<Encounter>()[0].Id;
                }
                catch (Exception)
                {
                    pEnc = 0;
                }
                iMySession.Close();
            }
            return pEnc;
        }
        public IList<Encounter> GetEncounterList(ulong ulEncounterID)
        {
            IList<Encounter> ilstEncounter = new List<Encounter>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria result = iMySession.CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", ulEncounterID));
                ilstEncounter = result.List<Encounter>();
                iMySession.Close();
            }
            return ilstEncounter;
        }


        public int DeleteExamination(ulong ulEncounterID, string sCategory)
        {
            int Delete_Result_Result = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery Delete_Result = iMySession.CreateSQLQuery("DELETE e.* FROM Examination as e Where e.Category ='" + sCategory + "' and e.Encounter_ID=" + ulEncounterID).AddEntity("e", typeof(Examination));
                Delete_Result_Result = Delete_Result.ExecuteUpdate();
                iMySession.Close();
            }
            return Delete_Result_Result;
        }
        public string GetPhysicianNameByID(ulong phy_id)
        {
            IList<User> ilstUser = new List<User>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria result = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", phy_id));
                ilstUser = result.List<User>();
                iMySession.Close();
            }
            if (ilstUser.Count != 0)
                return ilstUser[0].user_name;
            else
                return string.Empty;
        }
        private IList<Documents> GetClinicalInstruction(ulong EncounterID)
        {
            IList<Documents> ilstDocuments = new List<Documents>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria result = iMySession.CreateCriteria(typeof(Documents)).Add(Expression.Eq("Encounter_ID", EncounterID));

                ilstDocuments = result.List<Documents>();
                iMySession.Close();
            }
            return ilstDocuments;
        }
        //saravanan
        public IList<Rcopia_Medication> GetCSMedicationAdministrative(ulong ulHumanID)
        {
            IList<Rcopia_Medication> medicationList = new List<Rcopia_Medication>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("MedicationAdministrative");
                query1.SetParameter(0, ulHumanID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Rcopia_Medication fillmedication = new Rcopia_Medication();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillmedication.NDC_ID = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    fillmedication.Generic_Name = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    fillmedication.Strength = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillmedication.Dose = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillmedication.Route = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillmedication.Dose_Other = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    fillmedication.Patient_Notes = ccObject[6].ToString();
                    if (ccObject[8] != null)
                    fillmedication.Start_Date = Convert.ToDateTime(ccObject[8].ToString());
                    if (ccObject[9] != null)
                    fillmedication.Brand_Name = ccObject[9].ToString();
                    if (ccObject[10] != null)
                    fillmedication.Stop_Date = Convert.ToDateTime(ccObject[10].ToString());
                    if (ccObject[11] != null)
                    fillmedication.Quantity = ccObject[11].ToString();
                    if (ccObject[12] != null)
                    fillmedication.Quantity_Unit = ccObject[12].ToString();
                    if (ccObject[13] != null)
                    fillmedication.First_DataBank_Med_ID = ccObject[13].ToString();
                    if (ccObject[14] != null)
                    fillmedication.ICD_Code = ccObject[14].ToString();
                    if (ccObject[15] != null)
                    fillmedication.ICD_Code_Description = ccObject[15].ToString();
                    if (ccObject[16] != null)
                    fillmedication.Refills = ccObject[16].ToString();
                    if (ccObject[17] != null)
                    fillmedication.Action = ccObject[17].ToString();
                    if (ccObject[18] != null)
                    fillmedication.Dose_Unit = ccObject[18].ToString();
                    if (ccObject[19] != null)
                    fillmedication.Dose_Timing = ccObject[19].ToString();
                    if (ccObject[20] != null)
                    fillmedication.Internal_Property_Code = ccObject[20].ToString();
                    medicationList.Add(fillmedication);
                }
                iMySession.Close();
            }
            return medicationList;
        }
        public IList<ReferralOrder> GetCSReferralOrderforFutureAppointment(ulong ulEncounterID)
        {
            IList<ReferralOrder> ilstReferralOrder = new List<ReferralOrder>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISQLQuery sql = session.GetISession().CreateSQLQuery("Select r.* from referral_order r where r.encounter_id='" + ulEncounterID + "'").AddEntity("r", typeof(ReferralOrder));
                IQuery query1 = iMySession.GetNamedQuery("GetCSReferralOrderforFutureAppointment");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    ReferralOrder objReferralOrder = new ReferralOrder();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objReferralOrder.Id = Convert.ToUInt32(ccObject[0].ToString());
                    if (ccObject[1] != null)
                    objReferralOrder.Encounter_ID = Convert.ToUInt32(ccObject[1].ToString());
                    if (ccObject[2] != null)
                    objReferralOrder.Human_ID = Convert.ToUInt32(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    objReferralOrder.From_Physician_ID = Convert.ToUInt32(ccObject[3].ToString());
                    if (ccObject[4] != null)
                    objReferralOrder.Referral_Date = Convert.ToDateTime(ccObject[4].ToString());
                    if (ccObject[5] != null)
                    objReferralOrder.To_Physician_Name = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    objReferralOrder.To_Facility_Name = ccObject[6].ToString();
                    if (ccObject[7] != null)
                    objReferralOrder.To_Facility_Street_Address = ccObject[7].ToString();
                    if (ccObject[8] != null)
                    objReferralOrder.To_Facility_City = ccObject[8].ToString();
                    if (ccObject[9] != null)
                    objReferralOrder.To_Facility_State = ccObject[9].ToString();
                    if (ccObject[10] != null)
                    objReferralOrder.Valid_Till = Convert.ToDateTime(ccObject[10].ToString());
                    if (ccObject[11] != null)
                    objReferralOrder.Internal_Property_Date_of_service = Convert.ToDateTime(ccObject[11].ToString());
                    if (ccObject[12] != null)
                    objReferralOrder.To_Facility_Zip = ccObject[12].ToString();
                    if (ccObject[13] != null)
                    objReferralOrder.Reason_For_Referral = ccObject[13].ToString();
                    ilstReferralOrder.Add(objReferralOrder);
                }
                iMySession.Close();
            }
            return ilstReferralOrder;
        }
        public IList<OrdersDTO> GetCSTestforPendingandFuture(ulong ulEncounterID)
        {
            IList<Orders> ilstOrder = new List<Orders>();
            IList<OrdersSubmit> ilstOrdersubmit = new List<OrdersSubmit>();
            IList<OrdersDTO> ilstOrderDTO = new List<OrdersDTO>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISQLQuery sql = session.GetISession().CreateSQLQuery("select * from orders o inner join orders_submit os on o.order_submit_id=os.order_submit_id where o.encounter_id='" + ulEncounterID + "'").AddEntity("o", typeof(Orders)).AddEntity("os",typeof(OrdersSubmit));
                IQuery query1 = iMySession.GetNamedQuery("GetCSTestforPendingandFuture");
                query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(1, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                //ISQLQuery sql = session.GetISession().CreateSQLQuery("Select r.* from orders r where r.encounter_id='" + ulEncounterID + "'").AddEntity("r", typeof(Orders));
                //ArrayList arrayList = new ArrayList(sql.List());
                OrdersDTO objOrdersDTO = new OrdersDTO();
                for (int i = 0; i < arrayList.Count; i++)
                {

                    Orders objOrder = new Orders();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objOrder.Id = Convert.ToUInt32(ccObject[0].ToString());
                    if (ccObject[1] != null)
                    objOrder.Encounter_ID = Convert.ToUInt32(ccObject[1].ToString());
                    if (ccObject[2] != null)
                    objOrder.Human_ID = Convert.ToUInt32(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    objOrder.Physician_ID = Convert.ToUInt32(ccObject[3].ToString());
                    if (ccObject[4] != null)
                    objOrder.Lab_Procedure = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    objOrder.Lab_Procedure_Description = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    objOrder.Created_Date_And_Time = Convert.ToDateTime(ccObject[6].ToString());
                    if (ccObject[12] != null)
                    objOrder.Internal_Property_EncounterDate = Convert.ToDateTime(ccObject[12].ToString());
                    //objOrdersDTO.Lists.Add(objOrder);
                    ilstOrder.Add(objOrder);
                    OrdersSubmit objOrdersubmit = new OrdersSubmit();
                    //object[] ccObject1 = (object[])arrayList[i];
                    if (ccObject[7] != null)
                    objOrdersubmit.Id = Convert.ToUInt32(ccObject[7].ToString());
                    if (ccObject[8] != null)
                    objOrdersubmit.Encounter_ID = Convert.ToUInt32(ccObject[8].ToString());
                    if (ccObject[9] != null)
                    objOrdersubmit.Human_ID = Convert.ToUInt32(ccObject[9].ToString());
                    if (ccObject[10] != null)
                    objOrdersubmit.Physician_ID = Convert.ToUInt32(ccObject[10].ToString());
                    if (ccObject[11] != null)
                    objOrdersubmit.Test_Date = ccObject[11].ToString();
                    if (ccObject[12] != null)
                    objOrdersubmit.Internal_Property_EncounterDate = Convert.ToDateTime(ccObject[12].ToString());
                    //objOrdersDTO.ilstOrdersSubmitForPartialOrders.Add(objOrdersubmit);
                    ilstOrdersubmit.Add(objOrdersubmit);
                    //ilstOrderDTO.Add(objOrdersDTO);
                }
                for (int i = 0; i < ilstOrder.Count; i++)
                {

                    Orders objOrder = new Orders();
                    objOrder = ilstOrder[i];
                    objOrdersDTO.Lists.Add(objOrder);
                }
                for (int i = 0; i < ilstOrdersubmit.Count; i++)
                {

                    OrdersSubmit objOrdersubmit = new OrdersSubmit();
                    objOrdersubmit = ilstOrdersubmit[i];
                    objOrdersDTO.ilstOrdersSubmitForPartialOrders.Add(objOrdersubmit);
                }
                ilstOrderDTO.Add(objOrdersDTO);
                iMySession.Close();
            }
            return ilstOrderDTO;
        }
        public IList<CarePlan> GetCSCarePlanforCognitiveFunction(ulong ulEncounterID)
        {
            IList<CarePlan> ilstCarePlan = new List<CarePlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetCarePlanValueforCognitiveFunction");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    CarePlan objCarePlan = new CarePlan();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objCarePlan.Encounter_ID = Convert.ToUInt32(ccObject[0].ToString());
                    if (ccObject[1] != null)
                    objCarePlan.Human_ID = Convert.ToUInt32(ccObject[1].ToString());
                    if (ccObject[2] != null)
                    objCarePlan.Physician_ID = Convert.ToUInt32(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    objCarePlan.Care_Name = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    objCarePlan.Care_Name_Value = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    objCarePlan.Status = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    objCarePlan.Care_Plan_Notes = ccObject[6].ToString();
                    if (ccObject[7] != null)
                    objCarePlan.Plan_Date = ccObject[7].ToString();
                    if (ccObject[8] != null)
                    objCarePlan.Status_Value = ccObject[8].ToString();
                    ilstCarePlan.Add(objCarePlan);
                }
                iMySession.Close();
            }
            return ilstCarePlan;
        }

        public IList<CarePlan> GetCSCarePlanforCognitiveFunctionMentalStaus(ulong ulEncounterID)
        {
            IList<CarePlan> ilstCarePlan = new List<CarePlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetCarePlanValueforCognitiveFunctionMentalStaus");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    CarePlan objCarePlan = new CarePlan();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objCarePlan.Encounter_ID = Convert.ToUInt32(ccObject[0].ToString());
                    if (ccObject[1] != null)
                    objCarePlan.Human_ID = Convert.ToUInt32(ccObject[1].ToString());
                    if (ccObject[2] != null)
                    objCarePlan.Physician_ID = Convert.ToUInt32(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    objCarePlan.Care_Name = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    objCarePlan.Care_Name_Value = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    objCarePlan.Status = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    objCarePlan.Care_Plan_Notes = ccObject[6].ToString();
                    if (ccObject[7] != null)
                    objCarePlan.Plan_Date = ccObject[7].ToString();
                    if (ccObject[8] != null)
                    objCarePlan.Status_Value = ccObject[8].ToString();
                    if (ccObject[9] != null)
                    objCarePlan.Snomed_Code = ccObject[9].ToString();
                    ilstCarePlan.Add(objCarePlan);
                }
                iMySession.Close();
            }
            return ilstCarePlan;
        }


        public IList<CarePlan> GetCSCarePlanforFunctionalStaus(ulong ulEncounterID)
        {
            IList<CarePlan> ilstCarePlan = new List<CarePlan>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetCarePlanValueforFunctionalStaus");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    CarePlan objCarePlan = new CarePlan();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objCarePlan.Encounter_ID = Convert.ToUInt32(ccObject[0].ToString());
                    if (ccObject[1] != null)
                    objCarePlan.Human_ID = Convert.ToUInt32(ccObject[1].ToString());
                    if (ccObject[2] != null)
                    objCarePlan.Physician_ID = Convert.ToUInt32(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    objCarePlan.Care_Name = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    objCarePlan.Care_Name_Value = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    objCarePlan.Status = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    objCarePlan.Care_Plan_Notes = ccObject[6].ToString();
                    if (ccObject[7] != null)
                    objCarePlan.Plan_Date = ccObject[7].ToString();
                    if (ccObject[8] != null)
                    objCarePlan.Status_Value = ccObject[8].ToString();
                    if (ccObject[9] != null)
                    objCarePlan.Snomed_Code = ccObject[9].ToString();
                    ilstCarePlan.Add(objCarePlan);
                }
                iMySession.Close();
            }
            return ilstCarePlan;
        }



        public IList<Documents> GetCSRecomentedmaterial(ulong ulEncounterID)
        {
            IList<Documents> ilstDocuments = new List<Documents>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetRecomentedmaterial");
                query1.SetParameter(0, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Documents objDocuments = new Documents();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objDocuments.Id = Convert.ToUInt32(ccObject[0].ToString());
                    if (ccObject[1] != null)
                    objDocuments.Encounter_ID = Convert.ToUInt32(ccObject[1].ToString());
                    if (ccObject[2] != null)
                    objDocuments.Human_ID = Convert.ToUInt32(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    objDocuments.Physician_ID = Convert.ToUInt32(ccObject[3].ToString());
                    if (ccObject[4] != null)
                    objDocuments.Document_Type = ccObject[4].ToString();
                    ilstDocuments.Add(objDocuments);
                }
                iMySession.Close();
            }
            return ilstDocuments;
        }
        public IList<Assessment> GetCSEncounterDiagnosis(ulong ulEncounterID)
        {
            IList<Assessment> ilstAssessment = new List<Assessment>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetEncounterDiagnosis");
                query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(1, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Assessment objAssessment = new Assessment();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objAssessment.Id = Convert.ToUInt32(ccObject[0].ToString());
                    if (ccObject[1] != null)
                    objAssessment.Encounter_ID = Convert.ToUInt32(ccObject[1].ToString());
                    if (ccObject[2] != null)
                    objAssessment.Human_ID = Convert.ToUInt32(ccObject[2].ToString());
                    if (ccObject[3] != null)
                    objAssessment.Physician_ID = Convert.ToUInt32(ccObject[3].ToString());
                    if (ccObject[4] != null)
                    objAssessment.Snomed_Code = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    objAssessment.Snomed_Code_Description = ccObject[5].ToString();
                    ilstAssessment.Add(objAssessment);
                }
                iMySession.Close();
            }
            return ilstAssessment;
        }
        public IList<StaticLookup> GetRaceAndEthnicity(ulong ulHumanID)
        {
            IList<StaticLookup> RaceList = new List<StaticLookup>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetRaceAndEthnicity");
                query1.SetParameter(0, ulHumanID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    StaticLookup objSLookup = new StaticLookup();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objSLookup.Field_Name = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    objSLookup.Value = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    objSLookup.Default_Value = ccObject[2].ToString();
                    RaceList.Add(objSLookup);
                }
                iMySession.Close();
            }
            return RaceList;
        }
        public IList<FacilityLibrary> GetFacilityInfoforCustadian(ulong ulEncounterID)
        {
            IList<FacilityLibrary> ilstFacilityLibrary = new List<FacilityLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetEncounterCustodian");
                query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(1, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    FacilityLibrary objFacilityLibrary = new FacilityLibrary();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objFacilityLibrary.Fac_Name = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    objFacilityLibrary.Fac_Address1 = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    objFacilityLibrary.Fac_City = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    objFacilityLibrary.Fac_State = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    objFacilityLibrary.Fac_Zip = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    objFacilityLibrary.Fac_Telephone = ccObject[5].ToString();
                    ilstFacilityLibrary.Add(objFacilityLibrary);
                }
                iMySession.Close();
            }
            return ilstFacilityLibrary;


        }
        public IList<PhysicianLibrary> GetPhysicianLibraryDocumentation(ulong ulEncounterID)
        {
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetPhysicianAssistantforDocumentation");
                query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(1, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    PhysicianLibrary objPhysicianLibrary = new PhysicianLibrary();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objPhysicianLibrary.Med_Phy_Assistant = ccObject[0].ToString();
                    if (ccObject[2] != null)
                    objPhysicianLibrary.PhyAddress1 = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    objPhysicianLibrary.PhyCity = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    objPhysicianLibrary.PhyState = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    objPhysicianLibrary.PhyTelephone = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    objPhysicianLibrary.PhyZip = ccObject[6].ToString();
                    if (ccObject[7] != null)
                    objPhysicianLibrary.PhyPrefix = ccObject[7].ToString();
                    if (ccObject[8] != null)
                    objPhysicianLibrary.PhyFirstName = ccObject[8].ToString();
                    if (ccObject[9] != null)
                    objPhysicianLibrary.PhyLastName = ccObject[9].ToString();
                    if (ccObject[10] != null)
                    objPhysicianLibrary.Encounter_Date_of_Service = Convert.ToDateTime(ccObject[10].ToString());
                    if (ccObject[11] != null)
                    {
                        if (ccObject[11].ToString().Length == 9)
                        {
                            int number = Convert.ToInt32(ccObject[11].ToString());
                            string SSN_Number = number.ToString("N0", new System.Globalization.NumberFormatInfo()
                             {
                                 NumberGroupSizes = new[] { 3 },
                                 NumberGroupSeparator = "-"
                             });
                            objPhysicianLibrary.Physician_SSN_Number = SSN_Number;
                        }
                    }
                    else
                        objPhysicianLibrary.Physician_SSN_Number = "111-111-111";

                        //objPhysicianLibrary.Physician_SSN_Number = ccObject[11].ToString();
                    ilstPhysicianLibrary.Add(objPhysicianLibrary);
                }
                iMySession.Close();
            }
            return ilstPhysicianLibrary;
        }

        public IList<InHouseProcedure> GetImplantableDeviceInformationByHumanID(ulong HumanID)
        {
            IList<InHouseProcedure> ilistInhouseProcedure = new List<InHouseProcedure>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria iCriteria = null;
                iCriteria = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Is_Active", "Y"));
                ilistInhouseProcedure = iCriteria.List<InHouseProcedure>();
                iMySession.Close();
            }
            return ilistInhouseProcedure;
        }
        public IList<Assessment> GetCSCaseReportEncounterDiagnosis(ulong ulEncounterID)
        {
            IList<Assessment> ilstAssessment = new List<Assessment>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("GetCaseReportEncounterDiagnosis");
                query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(1, ulEncounterID);
                ArrayList arrayList = new ArrayList(query1.List()); ;
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Assessment objAssessment = new Assessment();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    objAssessment.Assessment_Type = ccObject[0].ToString();
                    if (ccObject[1] != null)
                    objAssessment.Snomed_Code = ccObject[1].ToString();
                    if (ccObject[2] != null)
                    objAssessment.Snomed_Code_Description = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    objAssessment.ICD = ccObject[3].ToString();//Value
                    if (ccObject[4] != null)
                    objAssessment.ICD_Description = ccObject[4].ToString();//Units
                    if (ccObject[5] != null)
                    objAssessment.ICD_9_Description = ccObject[5].ToString();//Ref_Range
                    if (ccObject[6] != null)
                    objAssessment.ICD_9 = ccObject[6].ToString();
                    ilstAssessment.Add(objAssessment);
                }
                iMySession.Close();
            }
            return ilstAssessment;
        }
        #endregion

        public static int CalculateAgeInMonths(DateTime birthDate, DateTime now)
        {
            int leap = 0;
            int Months = 0;
            if (1 == now.Month || 3 == now.Month || 5 == now.Month || 7 == now.Month || 8 == now.Month ||
            10 == now.Month || 12 == now.Month)
            {
                leap = 31;
            }
            else if (2 == now.Month)
            {
                if (0 == (now.Year % 4))
                {
                    if (0 == (now.Year % 400))
                    {
                        leap = 29;
                    }
                    else if (0 == (now.Year % 100))
                    {
                        leap = 28;
                    }

                    leap = 29;
                }
                else
                {
                    leap = 28;
                }

            }
            else
            {
                leap = 30;
            }


            if (leap == 28)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (365.25 / 12));
            }
            else if (leap == 31)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (365.25 / 12));
            }
            else if (leap == 29)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (366 / 12));
            }
            else if (leap == 30)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (365.25 / 12));
            }

            return Months;
        }

        private IList<LabLocation> GetCSLabList(ulong ulHumanID)
        {
            IList<LabLocation> ResultList = new List<LabLocation>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("Fill.CSLabDetailsList.List");
                ulong ulHuman_ID = ulHumanID;
                //query1.SetParameter(0, ulEncounterID);
                query1.SetParameter(0, ulHuman_ID);
                ArrayList arrayList = new ArrayList(query1.List());
                for (int i = 0; i < arrayList.Count; i++)
                {
                    //lb.lab_name,l.Location_Name,Lab_NPI , l.Street_Address1, l.Street_Address2, l.City, l.State, l.ZipCode,l.Phone_No
                    LabLocation fillResult = new LabLocation();
                    object[] ccObject = (object[])arrayList[i];
                    if (ccObject[0] != null)
                    fillResult.Modified_By = ccObject[0].ToString();//assign lab name
                    if (ccObject[1] != null)
                    fillResult.Location_Name = ccObject[1].ToString();
                    //fillResult.Lab_NPI = ccObject[2].ToString();
                    if (ccObject[3] != null)
                    fillResult.Street_Address1 = ccObject[3].ToString();
                    if (ccObject[4] != null)
                    fillResult.Street_Address2 = ccObject[4].ToString();
                    if (ccObject[5] != null)
                    fillResult.City = ccObject[5].ToString();
                    if (ccObject[6] != null)
                    fillResult.State = ccObject[6].ToString();
                    if (ccObject[7] != null)
                    fillResult.ZipCode = ccObject[7].ToString();
                    if (ccObject[8] != null)
                    fillResult.Phone_No = ccObject[8].ToString();
                    if (ccObject[9] != null)
                    fillResult.E_Mail = ccObject[9].ToString();//Lab_Procedure_Description
                    if (ccObject[10] != null)
                    fillResult.Lab_NPI= ccObject[10].ToString();
                    if (ccObject[11] != null)
                    fillResult.Suit_Number = ccObject[11].ToString();//Specimen type
                    if (ccObject[12]!=null)
                     fillResult.Created_Date_And_Time = Convert.ToDateTime(ccObject[12]);
                    ResultList.Add(fillResult);
                }
                iMySession.Close();
            }
            return ResultList;
        }
        public IList<PatGuarantor> GetPatGuarantorUsingHumanID(ulong ulHumanID)
        {
            IList<PatGuarantor> ilstPatGuarantor = new List<PatGuarantor>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria iCrit = iMySession.CreateCriteria(typeof(PatGuarantor)).Add(Expression.Eq("Human_ID", Convert.ToInt32(ulHumanID)));
                ilstPatGuarantor = iCrit.List<PatGuarantor>();
            }
            return ilstPatGuarantor;
        }

        public IList<Human> GetGardianHumanList(IList<ulong> LstHuman)
        {
            IList<Human> HumList = new List<Human>();
            HumanManager HumMngr = new HumanManager();
            HumList = HumMngr.GetHumanListById(LstHuman);
            return HumList;

        }


        public FillClinicalSummary GetClinicalSummaryCaseReporting(ulong ulEncounterID, ulong ulHumanID)
        {
            FillClinicalSummary fillCN = new FillClinicalSummary();
            fillCN.ChiefComplaints = GetCSChiefComplaints(ulEncounterID);
            fillCN.ChiefComplaintsHPI = GetCSChiefComplaintsHPI(ulEncounterID);
            fillCN.Vitals = GetCSVitals(ulEncounterID);
            fillCN.SocialHistory = GetCSSocialHistory(ulEncounterID, ulHumanID);
            fillCN.Assessment = GetCSAssessment(ulEncounterID);
            //fillCN.ProblemListing = GetCSProblemList(ulHumanID);//for reconcilation v include completed and active status also
            fillCN.ProblemListing = GetCSProblemListActiveandresolved(ulHumanID);
            //fillCN.Allergy = GetCSRCopiaAllergy(ulHumanID);
            fillCN.Medication = GetCSMedication(ulHumanID);
            //fillCN.ImmunizationList = GetCSImmunization(ulEncounterID, ulHumanID);
            fillCN.Encounter = GetCSEncounterDetails(ulEncounterID);
            fillCN.Case_Report_EncounterDiagnosis=GetCSCaseReportEncounterDiagnosis(ulEncounterID);
            fillCN.laborderList = GetCSLabOrderList(ulEncounterID);
            //fillCN.referralorderList = GetCSReferralOrderList(ulEncounterID);
            fillCN.Pat_Ins_Plan = GetCSPatInsPlan(ulHumanID);
            fillCN.ResultList = GetCSResultList(ulHumanID);
            GetCSHuman(ulHumanID, ref fillCN);
            ulong HumanID = fillCN.ID;
            fillCN.lookupList = GetRaceAndEthnicity(ulHumanID);
            fillCN.immunhistoryList = GetCSImmunizationHistory(HumanID);
            //fillCN.lookupValues = GetCSlookUpValues();
            //fillCN.VaccineCodes = GetCSVaccineCodes();

            if (fillCN.Encounter != null && fillCN.Encounter.Count>0)
            {
                if (fillCN.Encounter[0].Encounter_Provider_Review_ID != 0)
                {
                    fillCN.phyList = GetCSPhyList(Convert.ToUInt64(fillCN.Encounter[0].Encounter_Provider_Review_ID));
                }
                else
                {
                    fillCN.phyList = GetCSPhyList(Convert.ToUInt64(fillCN.Encounter[0].Encounter_Provider_ID));
                }
            }
            fillCN.MedicationAdministrative = GetCSMedicationAdministrative(ulHumanID);
            //fillCN.ReferralOrder = GetCSReferralOrderforFutureAppointment(ulEncounterID);
            //fillCN.OrdersTestPending_Future = GetCSTestforPendingandFuture(ulEncounterID);
            //fillCN.Care_Plan_Cognitive_Function = GetCSCarePlanforCognitiveFunction(ulEncounterID);
            fillCN.Document_Material = GetCSRecomentedmaterial(ulEncounterID);
            fillCN.EncounterDiagnosis = GetCSEncounterDiagnosis(ulEncounterID);
            fillCN.facilityLibraryCustodian = GetFacilityInfoforCustadian(ulEncounterID);
            fillCN.PhysicianLibraryDocumentation = GetPhysicianLibraryDocumentation(ulEncounterID);
            fillCN.PatGuarantor = GetPatGuarantorUsingHumanID(ulHumanID);
            fillCN.LabLocationList = GetCSLabList(ulHumanID);            
            if (fillCN.PatGuarantor != null && fillCN.PatGuarantor.Count > 0)
            {
                IList<ulong>humaid=new List<ulong>();
                for (int i = 0; i < fillCN.PatGuarantor.Count; i++)
                {
                    humaid.Add(Convert.ToUInt64(fillCN.PatGuarantor[i].Guarantor_Human_ID));
                }
                fillCN.GuardianList = GetGardianHumanList(humaid);            
            }


            //fillCN.HospitalizationHistory = GetHospitalizationHistory(ulEncounterID);
            //fillCN.ImplantProcedure = GetImplantableDeviceInformationByHumanID(ulHumanID);
            //fillCN.Care_Plan_Cognitive_Function_MentalStatus = GetCSCarePlanforCognitiveFunctionMentalStaus(ulEncounterID);
            //fillCN.Care_Plan_FunctionalStatus = GetCSCarePlanforFunctionalStaus(ulEncounterID);
            fillCN.OthertreatmentPlan = GetOtherTreatmentPlan(ulEncounterID);
            //fillCN.TreatmentPlan = GetTreatmentPlan(ulEncounterID);
            //fillCN.HealthConcernProblemList = GetHealthConcernProblemList(ulHumanID);
            // fillCN.Physician_Facility_ID
            //fillCN.addendumNotes = GetCSAddendumNotesList(ulEncounterID);
            //fillCN.imageorderList = GetCSImageOrderList(ulEncounterID);
            //fillCN.medicationHistory = GetCSMedicationHistory(ulEncounterID);
            //fillCN.ROS = GetCSROS(ulEncounterID);
            //fillCN.ROSNotes = GetCSROSNotes(ulEncounterID);
            //fillCN.Examination = GetCSExamination(ulEncounterID);
            //fillCN.AssessmentNotes = GetCSAssessmentNotes(ulEncounterID);
            //fillCN.NonDrugAllergyNotes = GetCSNonDrugAllergyNotes(ulEncounterID);
            //fillCN.PastMedicalHistory = GetCSPastMedicalHistory(ulEncounterID);
            //fillCN.PastMedicalHistoryNotes = GetCSPastMedicalHistoryNotes(ulEncounterID);
            //fillCN.NonDrugAllergy = GetCSNonDrugAllergy(ulEncounterID);

            //IList<string> RXCode = new List<string>();
            //for (int i = 0; i < fillCN.Medication.Count; i++)
            //{
            //    ISQLQuery sql = session.GetISession().CreateSQLQuery("SELECT RXCUI FROM rxnsat where ATV='" + fillCN.Medication[i].NDC_ID + "' and SAB='RXNORM'");
            //    if (sql.List<string>().Count != 0)
            //    {
            //        RXCode.Add(sql.List<string>()[0]);
            //    }
            //    else
            //    {
            //        RXCode.Add(string.Empty);
            //    }
            //}
            //fillCN.RXNormList = RXCode;
             fillCN.OrdersList = GetCSOrders(ulEncounterID);
            //fillCN.InHouseProcList = GetCSInHouseProcedures(ulEncounterID);
            //fillCN.TreatmentPlan = GetCSTreatmentPlan(ulHumanID);
            //fillCN.CarePlanList = GetCSCarePlanList(ulEncounterID);
            //fillCN.PrventivePlan = GetCSPreventivePlanList(ulEncounterID);
            //fillCN.FamilyHistory = GetCSFamilyHistory(ulEncounterID);
            //fillCN.familyHistoryNotes = GetCSFamilyHistoryNotes(ulEncounterID);

            // fillCN.ResultSubEntry = GetCSManualResultList(ulEncounterID);
            //IList<FamilyDisease> temp = new List<FamilyDisease>();
            //for (int i = 0; i < fillCN.FamilyHistory.Count; i++)
            //{
            //    temp = GetCSFamilyDiseaseDetails(fillCN.FamilyHistory[i].Id);
            //    for (int j = 0; j < temp.Count; j++)
            //    {
            //        fillCN.FamilyDisease.Add(temp[j]);
            //    }
            //}


            //fillCN.SocialHistoryNotes = GetCSSocialHistoryNotes(ulEncounterID);
            //fillCN.AdvanceDirectiveList = GetCSAdvanceDirective(ulEncounterID);
            //fillCN.PhysicianList = GetCSPhysicianPatient(ulEncounterID);

            //fillCN.HumanID = getHumanID(ulEncounterID);
            //fillCN.eprescriptionList = GetCSEPrescriptionList(ulEncounterID);

            //fillCN.CarePlanLookup = GetCarePlanList(ulEncounterID);
            //fillCN.ClinicalInstructionDocuments = GetClinicalInstruction(ulEncounterID);

            //FacilityManager objFacilityMngr = new FacilityManager();
            //for (int i = 0; i < fillCN.Encounter.Count; i++)
            //{
            //    FacilityLibrary objfacilitylib = new FacilityLibrary();
            //    objfacilitylib = objFacilityMngr.GetFacilityByFacilityname(fillCN.Encounter[i].Facility_Name)[0];

            //    fillCN.facilityLibrary.Add(objfacilitylib);
            //}

            //fillCN.PatientResultsMeasure = GetPatientResultsMeasureList(ulEncounterID);
            return fillCN;
        }



    }
}
