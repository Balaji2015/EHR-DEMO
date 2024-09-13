using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using MySql.Data.MySqlClient;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Threading;
using System.Configuration;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IAssessmentManager : IManagerBase<Assessment, ulong>
    {
        IList<Assessment> GetAssessmentUsingEncounterID(ulong encounterID);
        IList<Assessment> GetAssessmentUsingPatientID(ulong PatientID, string Year);
        FillAssessment BatchOperationsToAssessment(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate,
            IList<Assessment> ListToDelete, IList<ProblemList> ListToInsertProblemList, IList<ProblemList> ListToUpdateProblemList,
            IList<ProblemList> ListToDeleteProblemList, string sMacAddress, GeneralNotes generalNotes,
            TreatmentPlan SavePlan, string UserName, ulong ulEncounterID, ulong ulHumanID, ulong PhysicianID,
            IList<string> sMacraICDChkList, string sIs_Assessment_CopyPrevious, string sLocalTime, string sLegalOrg, IList<EandMCodingICD> lstEandMICDInsert, IList<EandMCodingICD> lstEandMICDDelete);
        //FillAssessment BatchOperationsToAssessment(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate, IList<Assessment> ListToDelete, IList<ProblemList> ListToInsertProblemList, IList<ProblemList> ListToUpdateProblemList, IList<ProblemList> ListToDeleteProblemList, string sMacAddress, GeneralNotes generalNotes, TreatmentPlan SavePlan, string UserName, ulong ulEncounterID, ulong ulHumanID, string Is_Sent_To_RCopia, ulong PhysicianID, IList<string> sMacraICDChkList, string sIs_Assessment_CopyPrevious);
        //FillAssessmentRuledOut GetAssessmentUsingEncounterIDForRuledOut(ulong encounterId, ulong physicianId);
        //FillAssessmentRuledOut BatchOperationsToAssessmentFromRuledOut(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate, IList<Assessment> ListToDelete, ulong encounterId, string sMacAddress, GeneralNotes generalNotes);
        //FillAssessment LoadAssessment(ulong encounterID, ulong humanID);
        //Latha - 11 Aug 2011 - Single Service call - Start
        FillAssessment LoadAssessment(ulong encounterID, ulong humanID, string Is_Ruled_Out, string UserName, string type);
        //Latha - 11 Aug 2011 - Single Service call - End

        FillAssessment GetAssesmentForPastEncounter(ulong encounter_ID, ulong human_ID, ulong Physician_ID, DateTime dtDOS);
        //FillAssessmentRuledOut GetAssessmentRuledOutGetAssesmentForPastEncounter(ulong encounterId, ulong human_ID, ulong Physician_ID);

        IList<ICD9ICD10Mapping> loadICD10Temp(string icd9);
        IList<Assessment> GetAssessmentForRecommendedMaterials(ulong ulEncounterId, ulong ulHumanId);
        IList<Assessment> GetAssessmentICD(ulong EncID, ulong Human_ID, IList<string> ICD);
        void SaveAssesmentforSummary(IList<Assessment> lstassesment);
        IList<String> GetICDListForAssessment(IList<AssessmentVitalsLookup> AssessVitalsLookup, IList<ROS> ROSlist, IList<Rcopia_Medication> Medlist, IList<PatientResults> Patient_Resultslist, IList<Healthcare_Questionnaire> Questionnaire, ulong encounterID);
        string GetAssessmentICDbyDos(ulong uHuman_id, string sYear, string sRaf_Status);
    }

    public partial class AssessmentManager : ManagerBase<Assessment, ulong>, IAssessmentManager
    {
        #region Constructors

        public AssessmentManager()
            : base()
        {

        }
        public AssessmentManager
            (INHibernateSession session)
            : base(session)
        {

        }

        #endregion

        #region Get Methods

        public IList<Assessment> GetAssessmentUsingEncounterID(ulong encounterId)
        {
            IList<Assessment> lstAssessment = new List<Assessment>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", encounterId));
                lstAssessment = criteria.List<Assessment>();
                iMySession.Close();
            }

            return lstAssessment;

        }

        public IList<Assessment> GetAssessmentUsingPatientID(ulong PatientID, string Year)
        {
            IList<Assessment> asseslist = new List<Assessment>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select a.*,e.* from assessment a,encounter e where a.encounter_id=e.encounter_id and a.human_id='" + PatientID.ToString() + "' and Date_Format(e.date_of_service,'%Y')='" + Year + "'")
                   .AddEntity("a", typeof(Assessment))
                 .AddEntity("e", typeof(Encounter));

                IList<object> lstAssessment = (IList<object>)sql.List();

                foreach (IList<Object> l in lstAssessment)
                {
                    Assessment asses = (Assessment)l[0];
                    asseslist.Add(asses);
                }
                iMySession.Close();
            }
            return asseslist;
            //ICriteria criteria = session.GetISession().CreateCriteria(typeof(Assessment)).Add(Expression.Like("Human_ID", PatientID));
            //return criteria.List<Assessment>();

        }
        #endregion

        int iTryCount = 0;
        // For Reference: Original Function commented and copied below this function.\\Changes made For Bug Id :55981  \\Rcopia section commented and assessmentvitals lookup get from Config xml and remove the Rule out Function call
        // public FillAssessment BatchOperationsToAssessment(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate, IList<Assessment> ListToDelete, IList<ProblemList> ListToInsertProblemList, IList<ProblemList> ListToUpdateProblemList, IList<ProblemList> ListToDeleteProblemList, string sMacAddress, GeneralNotes generalNotes, TreatmentPlan SavePlan, string UserName, ulong ulEncounterID, ulong ulHumanID, string Is_Sent_To_RCopia, ulong PhysicianID, IList<string> sMacraICDChkList, string sIs_Assessment_CopyPrevious)
        public FillAssessment BatchOperationsToAssessment(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate, IList<Assessment> ListToDelete,
            IList<ProblemList> ListToInsertProblemList, IList<ProblemList> ListToUpdateProblemList, IList<ProblemList> ListToDeleteProblemList,
            string sMacAddress, GeneralNotes generalNotes, TreatmentPlan SavePlan, string UserName, ulong ulEncounterID, ulong ulHumanID, ulong PhysicianID,
            IList<string> sMacraICDChkList, string sIs_Assessment_CopyPrevious, string sLocalTime, string sLegalOrg, IList<EandMCodingICD> lstEandMICDInsert, IList<EandMCodingICD> lstEandMICDDelete)
        {
            iTryCount = 0;
            if (sIs_Assessment_CopyPrevious != null && sIs_Assessment_CopyPrevious != string.Empty && sIs_Assessment_CopyPrevious != "Yes")
            {
                //Remove Assessment Duplicate fron insert list For Bug Id  56330
                AssessmentManager objAssessment = new AssessmentManager();
                IList<Assessment> AssDBlst = new List<Assessment>();
                AssDBlst = objAssessment.GetAssessmentUsingEncounterID(ulEncounterID);
                if (ListToInsert != null && ListToInsert.Count > 0)
                {
                    if (AssDBlst != null && AssDBlst.Count > 0)
                    {
                        IList<Assessment> AssDuplicatelst = new List<Assessment>();
                        IList<Assessment> AssDuplicateTemplst = new List<Assessment>();

                        foreach (Assessment objTemp in AssDBlst)
                        {
                            AssDuplicateTemplst = new List<Assessment>();
                            AssDuplicateTemplst = ListToInsert.Where(a => a.ICD == objTemp.ICD && objTemp.Diagnosis_Source.Trim().ToUpper() != "VITALS|DELETED").ToList<Assessment>();
                            if (AssDuplicateTemplst.Count > 0)
                            {
                                ListToInsert = ListToInsert.Except(AssDuplicateTemplst).ToList();
                            }
                        }
                    }
                }
            }
            //End Bug Id 56330
            IList<GeneralNotes> generalNotesInsert = new List<GeneralNotes>();
            IList<GeneralNotes> generalNotesUpdate = new List<GeneralNotes>();

            string sPlan = string.Empty;
            IList<ProblemList> acsProblemlist = new List<ProblemList>();
            ulong[] proList = new ulong[ListToUpdateProblemList.Count];
            if (ListToUpdateProblemList.Count > 0)
            {
                acsProblemlist = ListToUpdateProblemList.OrderBy(e => e.Id).ToList();
                for (int i = 0; i < ListToUpdateProblemList.Count; i++)
                    proList[i] = acsProblemlist[i].Id;
            }
            ProblemListManager problemMngr = new ProblemListManager();
            ulong humanId = 0;
            IList<ProblemList> probLst = new List<ProblemList>();
            TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();

            IList<AssessmentVitalsLookup> AssessmentVitalsLookupList = new List<AssessmentVitalsLookup>();
            // AssessmentVitalsLookupManager objAssessVitalsMngr = new AssessmentVitalsLookupManager();
            // AssessmentVitalsLookupList = objAssessVitalsMngr.GetByCriteria(Restrictions.Eq("Is_Macra_Field", "Y"));

            string AssessmentMappingXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\AssessmentVitalsLookup.xml");
            if (File.Exists(AssessmentMappingXmlFilePath) == true)
            {
                using (FileStream fs = new FileStream(AssessmentMappingXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    XmlDocument itemDoc = new XmlDocument();
                    XmlTextReader xmltxtReader = new XmlTextReader(fs);
                    itemDoc.Load(xmltxtReader);
                    xmltxtReader.Close();
                    XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("Assessment_vitals_lookupList");
                    if (xmlNodeList != null && xmlNodeList.Count > 0 && xmlNodeList[0].ChildNodes != null && xmlNodeList[0].ChildNodes.Count > 0)
                    {
                        for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                        {
                            AssessmentVitalsLookup objAssVitalsLookup = new AssessmentVitalsLookup();
                            objAssVitalsLookup.Description = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Description").Value;
                            objAssVitalsLookup.Entity_Name = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Entity_Name").Value;
                            objAssVitalsLookup.Field_Name = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value;
                            objAssVitalsLookup.Hirarrchy = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Hirarrchy").Value;
                            objAssVitalsLookup.ICD = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("ICD").Value;
                            objAssVitalsLookup.ICD_10 = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("ICD_10").Value;
                            objAssVitalsLookup.ICD_10_Description = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("ICD_10_Description").Value;
                            objAssVitalsLookup.Is_Current_Encounter = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Is_Current_Encounter").Value;
                            objAssVitalsLookup.Is_Macra_Field = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Is_Macra_Field").Value;
                            objAssVitalsLookup.Is_Mutually_Exclusive = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Is_Mutually_Exclusive").Value;
                            objAssVitalsLookup.Sort_Order = Convert.ToInt32(xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Sort_Order").Value);
                            objAssVitalsLookup.Value = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Value").Value;
                            objAssVitalsLookup.Id = Convert.ToUInt32(xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Id").Value);
                            AssessmentVitalsLookupList.Add(objAssVitalsLookup);
                        }
                    }
                    fs.Close();
                    fs.Dispose();
                }
            }
            AssessmentVitalsLookupList = AssessmentVitalsLookupList.Where(a => a.Is_Macra_Field == "Y").ToList<AssessmentVitalsLookup>();

            IList<string> MacraICDlst = AssessmentVitalsLookupList.Select(a => a.ICD_10).ToList<string>();


            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                if (ListToInsertProblemList.Count > 0)
                {
                    humanId = ListToInsertProblemList[0].Human_ID;

                }
                if (ListToUpdateProblemList.Count > 0)
                {
                    humanId = ListToUpdateProblemList[0].Human_ID;
                }
                if (ListToDeleteProblemList.Count > 0)
                {
                    humanId = ListToDeleteProblemList[0].Human_ID;
                }
                //probLst = problemMngr.GetICDByHumanId(humanId);

            }
            #region TreatmentPlan Get
            IList<string> ilstGeneralPlanTagList = new List<string>();
            ilstGeneralPlanTagList.Add("TreatmentPlanList");


            IList<object> ilstGeneralPlanBlobFinal = new List<object>();
            ilstGeneralPlanBlobFinal = ReadBlob( ulEncounterID, ilstGeneralPlanTagList);

            if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
            {
                if (ilstGeneralPlanBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                    {
                        objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                    }
                }
            }
        //string FileName = "Encounter" + "_" + ulEncounterID + ".xml";
        //XmlTextReader XmlText = null;
        //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
        //try
        //{
        //    if (File.Exists(strXmlFilePath) == true)
        //    {
        //        XmlDocument itemDoc = new XmlDocument();
        //        XmlText = new XmlTextReader(strXmlFilePath);
        //        XmlNodeList xmlTagName = null;
        //        // itemDoc.Load(XmlText);

        //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        {
        //            itemDoc.Load(fs);

        //            XmlText.Close();
        //            if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
        //            {
        //                xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

        //                if (xmlTagName.Count > 0)
        //                {
        //                    for (int j = 0; j < xmlTagName.Count; j++)
        //                    {
        //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == ulEncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("ASSESSMENT"))
        //                        {

        //                            string TagName = xmlTagName[j].Name;
        //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
        //                            TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
        //                            IEnumerable<PropertyInfo> propInfo = null;
        //                            propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

        //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
        //                            {

        //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
        //                                {
        //                                    foreach (PropertyInfo property in propInfo)
        //                                    {
        //                                        if (property.Name == nodevalue.Name)
        //                                        {
        //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
        //                                                property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
        //                                                property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
        //                                                property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
        //                                                property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
        //                                            else
        //                                                property.SetValue(TreatmentPlan, nodevalue.Value, null);
        //                                        }
        //                                    }
        //                                }

        //                            }
        //                            objTreatmentPlan.Add(TreatmentPlan);
        //                        }
        //                    }
        //                }
        //            }
        //            fs.Close();
        //            fs.Dispose();
        //        }
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    if (XmlText != null)
        //        XmlText.Close();

        //    throw Ex;
        //}
        #endregion
        TryAgain:
            int iResult = 0;
            //Session.GetISession().Clear();
            //  ulong id = 0;
            // ulong phyId = 0;
            string strIsRuledOut = string.Empty;
            Human humanRecord = new Human();
            ulong EncounterORHumanId = 0;
            GenerateXml xmlobjEncounter = new GenerateXml();
            GenerateXml xmlobjHuman = new GenerateXml();
            String GeneralNotesText = string.Empty;
            bool objAssessmentConsistent = true, objProblemlistConsistent = true, objGeneralNotesConsistent = true;
            using (ISession MySession = Session.GetISession())
            {
                try
                {
                    using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        HumanManager hManager = new HumanManager();


                        #region GetHumanRecordForRCopia
                        //if (ListToInsert != null && ListToInsert.Count > 0)
                        //{
                        //    id = ListToInsert[0].Human_ID;
                        //}
                        //else if (ListToUpdate != null && ListToUpdate.Count > 0)
                        //{
                        //    id = ListToUpdate[0].Human_ID;
                        //}
                        //else if (ListToDelete != null && ListToDelete.Count > 0)
                        //{
                        //    id = ListToDelete[0].Human_ID;
                        //}

                        ////humanRecord = hManager.GetById(id);
                        //humanRecord.Is_Sent_To_Rcopia = Is_Sent_To_RCopia;
                        #endregion

                        try
                        {

                            IList<Assessment> ListToInsertAssessment = new List<Assessment>();
                            ListToInsertAssessment = ListToInsert.OrderByDescending(a => a.Primary_Diagnosis).ToList<Assessment>();

                            #region Assessment

                            //Added Latha - 2/8/10 - Checking for null value.
                            if ((ListToInsertAssessment != null) && (ListToUpdate != null) && (ListToDelete != null))
                            {
                                if ((ListToInsertAssessment.Count > 0) || (ListToUpdate.Count > 0) || (ListToDelete.Count > 0))
                                {
                                    if (ListToInsertAssessment.Count == 0)
                                    {
                                        ListToInsertAssessment = null;

                                    }
                                    else if (ListToUpdate.Count == 0)
                                    {
                                        ListToUpdate = null;
                                    }
                                    else if (ListToDelete.Count == 0)
                                    {
                                        ListToDelete = null;
                                    }
                                    //Now there is no Ruledout tab. So below functions are commented
                                    strIsRuledOut = "N";
                                    //To assign physician id and whether selected assessment or ruled out assessment which is needed in the load function parameter.
                                    //if (ListToInsertAssessment != null && ListToInsertAssessment.Count > 0)
                                    //{
                                    //    AssignPhyIdAndIsRuledOutForLoadFunction(ref phyId, ref strIsRuledOut, ListToInsertAssessment);
                                    //}
                                    //else if (ListToUpdate != null && ListToUpdate.Count > 0)
                                    //{
                                    //    AssignPhyIdAndIsRuledOutForLoadFunction(ref phyId, ref strIsRuledOut, ListToUpdate);
                                    //}
                                    //else if (ListToDelete != null && ListToDelete.Count > 0)
                                    //{
                                    //    AssignPhyIdAndIsRuledOutForLoadFunction(ref phyId, ref strIsRuledOut, ListToDelete);
                                    //}
                                    #region

                                    if (ListToInsertAssessment != null && ListToInsertAssessment.Count > 0)
                                    {
                                        EncounterORHumanId = ListToInsertAssessment[0].Encounter_ID;
                                    }
                                    else if (ListToUpdate != null && ListToUpdate.Count > 0)
                                        EncounterORHumanId = ListToUpdate[0].Encounter_ID;
                                    else
                                        EncounterORHumanId = ListToDelete[0].Encounter_ID;
                                    #endregion
                                    if (ListToInsertAssessment == null)
                                        ListToInsertAssessment = new List<Assessment>();
                                    if (ListToUpdate == null)
                                        ListToUpdate = new List<Assessment>();
                                    //Assessment Save in Xml to be seperated from maangerbase.cs//For Bug Id 56330
                                    //iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ListToInsertAssessment, ref ListToUpdate, ListToDelete, MySession, sMacAddress, true, true, EncounterORHumanId, string.Empty, ref xmlobjEncounter);

                                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ListToInsertAssessment, ref ListToUpdate, ListToDelete, MySession, sMacAddress, true, true, EncounterORHumanId, string.Empty, ref xmlobjEncounter);

                                    // iResult = SaveUpdateDeleteWithoutTransaction(ref ListToInsertAssessment, ListToUpdate, ListToDelete, MySession, sMacAddress);
                                    //if bResult = false then, the deadlock is occured 
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
                            //if (ListToUpdate != null)
                            //    objAssessmentConsistent = xmlobjEncounter.CheckDataConsistency(ListToInsertAssessment.Concat(ListToUpdate).Cast<object>().ToList(), false, string.Empty);
                            //else
                            //    objAssessmentConsistent = xmlobjEncounter.CheckDataConsistency(ListToInsertAssessment.Cast<object>().ToList(), false, string.Empty);

                            #endregion

                            #region problemList
                            IList<Assessment> assList = null;
                            assList = ListToInsertAssessment;
                            IList<ProblemList> updatedList = new List<ProblemList>();
                            if ((ListToInsertProblemList != null) && (ListToUpdateProblemList != null) && (ListToDeleteProblemList != null))
                            {
                                if ((ListToInsertProblemList.Count > 0) || (ListToUpdateProblemList.Count > 0) || (ListToDeleteProblemList.Count > 0))
                                {
                                    IList<ProblemList> prob = new List<ProblemList>();


                                    if (ListToUpdateProblemList.Count > 0)
                                    {
                                        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                                        {
                                            ICriteria criteriaProblemList = iMySession.CreateCriteria(typeof(ProblemList)).Add(Expression.In("Id", proList)).AddOrder(Order.Asc("Id"));
                                            prob = criteriaProblemList.List<ProblemList>();
                                        }

                                        for (int i = 0; i < prob.Count; i++)
                                            acsProblemlist[i].Version = prob[i].Version;

                                        ListToUpdateProblemList = acsProblemlist;
                                    }

                                    DateTime currTime = DateTime.UtcNow;


                                    if (ListToInsertProblemList.Count > 0)
                                    {
                                        humanId = ListToInsertProblemList[0].Human_ID;
                                        currTime = ListToInsertProblemList[0].Created_Date_And_Time;
                                    }
                                    else if (ListToUpdateProblemList.Count > 0)
                                    {
                                        humanId = ListToUpdateProblemList[0].Human_ID;
                                        currTime = ListToUpdateProblemList[0].Created_Date_And_Time;
                                    }
                                    else if (ListToDeleteProblemList.Count > 0)
                                    {
                                        humanId = ListToDeleteProblemList[0].Human_ID;
                                        currTime = ListToDeleteProblemList[0].Created_Date_And_Time;
                                    }

                                    if (humanId != 0 && (ListToUpdateProblemList.Count != 0 || ListToInsertProblemList.Count != 0))
                                    {

                                        var GetActiveProb = from obj in probLst where obj.ICD == "0000" select obj;
                                        if (GetActiveProb.ToList().Count > 0)
                                        {
                                            ProblemList prblst = GetActiveProb.ToList<ProblemList>()[0];
                                            prblst.Is_Active = "N";
                                            prblst.Modified_By = UserName;
                                            prblst.Human_ID = humanId;
                                            prblst.Modified_Date_And_Time = currTime;
                                            updatedList = new List<ProblemList>(ListToUpdateProblemList);
                                            updatedList.Add(prblst);
                                        }
                                    }
                                    //if (updatedList.Count > 0)
                                    //    iResult = problemMngr.BatchOperationsToProblem(ListToInsertProblemList, updatedList, ListToDeleteProblemList, MySession, sMacAddress, false);
                                    //else
                                    //    iResult = problemMngr.BatchOperationsToProblem(ListToInsertProblemList, ListToUpdateProblemList, ListToDeleteProblemList, MySession, sMacAddress, false);

                                    if (ListToInsertProblemList.Count > 0)
                                        EncounterORHumanId = ListToInsertProblemList[0].Human_ID;
                                    else if (updatedList.Count > 0)
                                        EncounterORHumanId = updatedList[0].Human_ID;
                                    else if (ListToDeleteProblemList.Count > 0)
                                        EncounterORHumanId = ListToDeleteProblemList[0].Human_ID;
                                    else
                                        EncounterORHumanId = ListToUpdateProblemList[0].Human_ID;
                                    if (updatedList.Count > 0)
                                        iResult = problemMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ListToInsertProblemList, ref updatedList, ListToDeleteProblemList, MySession, sMacAddress, true, true, EncounterORHumanId, string.Empty, ref xmlobjHuman);
                                    else
                                        iResult = problemMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ListToInsertProblemList, ref ListToUpdateProblemList, ListToDeleteProblemList, MySession, sMacAddress, true, true, EncounterORHumanId, string.Empty, ref xmlobjHuman);

                                    //if bResult = false then, the deadlock is occured 
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
                                        //  MySession.Close();
                                        throw new Exception("Exception occurred. Transaction failed.");
                                    }
                                }
                            }

                            //if (updatedList.Count > 0)
                            //    objProblemlistConsistent = xmlobjHuman.CheckDataConsistency(ListToInsertProblemList.Concat(updatedList).Cast<object>().ToList(), true, string.Empty);
                            //else
                            //    if (ListToUpdateProblemList != null)
                            //        objProblemlistConsistent = xmlobjHuman.CheckDataConsistency(ListToInsertProblemList.Concat(ListToUpdateProblemList).Cast<object>().ToList(), true, string.Empty);
                            //    else
                            //        objProblemlistConsistent = xmlobjHuman.CheckDataConsistency(ListToInsertProblemList.Cast<object>().ToList(), true, string.Empty);
                            ////IList<AssessmentProblemListDifference> ListToUpdateDifference = new List<AssessmentProblemListDifference>();
                            //if ((ListToInsertAssessmentProbListDiff != null) && (ListToUpdateAssessmentProbListDiff != null) && (ListToDeleteAssessmentProbListDiff != null))
                            //{
                            //    if ((ListToInsertAssessmentProbListDiff.Count > 0) || (ListToUpdateAssessmentProbListDiff.Count > 0) || (ListToDeleteAssessmentProbListDiff.Count > 0))
                            //    {
                            //        AssessmentProblemListDifferenceManager assessmentProblemListDifferenceMgr = new AssessmentProblemListDifferenceManager();
                            //        bResult = assessmentProblemListDifferenceMgr.BatchOperationsToAssessmentProblemListDifference(ListToInsertAssessmentProbListDiff, ListToUpdateAssessmentProbListDiff, ListToDeleteAssessmentProbListDiff, MySession, sMacAddress);
                            //        //if bResult = false then, the deadlock is occured 
                            //        if (bResult == false)
                            //        {
                            //            if (iTryCount < 5)
                            //            {
                            //                iTryCount++;
                            //                goto TryAgain;
                            //            }
                            //        }
                            //    }
                            //}


                            #endregion

                            #region General Notes


                            if (generalNotes != null)
                            {
                                if (generalNotes.Id == 0)
                                {
                                    generalNotesInsert.Add(generalNotes);
                                    EncounterORHumanId = generalNotesInsert[0].Encounter_ID;
                                    GeneralNotesText = generalNotesInsert[0].Parent_Field.Replace(" ", "");
                                }
                                else
                                {
                                    generalNotesUpdate.Add(generalNotes);
                                    EncounterORHumanId = generalNotesUpdate[0].Encounter_ID;
                                    GeneralNotesText = generalNotesUpdate[0].Parent_Field.Replace(" ", "");

                                }
                            }
                            if (generalNotes != null)
                            {
                                if (generalNotes.Id != 0)
                                {
                                    IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Source_ID == generalNotes.Id).ToList<TreatmentPlan>();
                                    if (itemlst != null && itemlst.Count > 0)
                                    {
                                        TreatmentPlan item = itemlst[0];
                                        item.Plan = "* " + generalNotes.Notes;
                                        item.Modified_By = generalNotes.Modified_By;
                                        item.Modified_Date_And_Time = generalNotes.Modified_Date_And_Time;
                                        item.Plan_Reference = "GENERAL NOTES";
                                        Update_Tplan.Add(item);
                                    }
                                }
                                else if (generalNotes.Id == 0)
                                {
                                    TreatmentPlan item = new TreatmentPlan();
                                    item.Human_ID = generalNotes.Human_ID;
                                    item.Encounter_Id = generalNotes.Encounter_ID;
                                    item.Physician_Id = PhysicianID;
                                    item.Created_By = generalNotes.Created_By;
                                    item.Created_Date_And_Time = generalNotes.Created_Date_And_Time;
                                    item.Plan_Type = "ASSESSMENT";
                                    string plan_txt = string.Empty;
                                    plan_txt = "* " + generalNotes.Notes;
                                    item.Plan = plan_txt;
                                    item.Version = 0;
                                    item.Plan_Reference = "GENERAL NOTES";
                                    item.Local_Time = sLocalTime;
                                    Insert_Tplan.Add(item);
                                }
                                GeneralNotesManager genMgr = new GeneralNotesManager();
                                //iResult = genMgr.SaveUpdateDeleteGeneralNotes(generalNotesInsert, generalNotesUpdate, MySession, sMacAddress);
                                iResult = genMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref generalNotesInsert, ref generalNotesUpdate, null, MySession, sMacAddress, true, true, EncounterORHumanId, GeneralNotesText, ref xmlobjEncounter);
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
                                //if (generalNotesUpdate != null)
                                //    objGeneralNotesConsistent = xmlobjEncounter.CheckDataConsistency(generalNotesInsert.Concat(generalNotesUpdate).Cast<object>().ToList(), true, "SelectedAssessment");
                                //else
                                //    objGeneralNotesConsistent = xmlobjEncounter.CheckDataConsistency(generalNotesInsert.Cast<object>().ToList(), true, "SelectedAssessment");

                            }

                            #endregion
                            #region Treatment plan

                            if (ListToInsertAssessment != null && ListToInsertAssessment.Count > 0)
                            {
                                foreach (Assessment obj in ListToInsertAssessment)
                                {
                                    if (obj.Diagnosis_Source.Trim().ToUpper() != "VITALS|DELETED")//BUGID:45554
                                    {
                                        TreatmentPlan item = new TreatmentPlan();
                                        item.Human_ID = obj.Human_ID;
                                        item.Encounter_Id = obj.Encounter_ID;
                                        item.Physician_Id = obj.Physician_ID;
                                        item.Created_By = obj.Created_By;
                                        item.Created_Date_And_Time = obj.Created_Date_And_Time;
                                        item.Plan_Type = "ASSESSMENT";
                                        string plan_txt = string.Empty;
                                        plan_txt = "* " + obj.ICD_Description + "(ICD- " + obj.ICD + ")" + (obj.Assessment_Notes.Trim() != string.Empty ? "-" + obj.Assessment_Notes : "") + (obj.Assessment_Status.Trim() != string.Empty ? "-" + obj.Assessment_Status : "");
                                        item.Plan = plan_txt;
                                        item.Version = 0;
                                        item.Source_ID = obj.Id;
                                        item.Local_Time = sLocalTime;
                                        if (obj.Primary_Diagnosis == "Y")
                                            item.Plan_Reference = "PRIMARY";
                                        else
                                            item.Plan_Reference = "NONE";
                                        if (MacraICDlst.IndexOf(obj.ICD) != -1)
                                        {
                                            item.Plan_Reference = "OTHER";
                                        }
                                        foreach (string s in sMacraICDChkList)
                                        {
                                            string icdParent = s.Split('|')[0];
                                            string icdChild = s.Split('|')[1];
                                            if (MacraICDlst.IndexOf(icdParent) != -1 && obj.ICD.ToString().Trim().Equals(icdChild.ToString().Trim()))
                                            {
                                                item.Plan_Reference = "OTHER";
                                            }

                                        }
                                        Insert_Tplan.Add(item);
                                    }
                                }
                            }
                            if (ListToUpdate != null && ListToUpdate.Count > 0)
                            {
                                foreach (Assessment obj in ListToUpdate)
                                {
                                    if (objTreatmentPlan.Count > 0)
                                    {
                                        IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Source_ID == obj.Id).ToList();
                                        if (itemlst != null && itemlst.Count > 0)
                                        {
                                            TreatmentPlan item = itemlst[0];
                                            string plan_txt = string.Empty;
                                            plan_txt = "* " + obj.ICD_Description + "(ICD- " + obj.ICD + ")" + (obj.Assessment_Notes.Trim() != string.Empty ? "-" + obj.Assessment_Notes : "") + (obj.Assessment_Status.Trim() != string.Empty ? "-" + obj.Assessment_Status : "");
                                            item.Plan = plan_txt;
                                            item.Modified_By = obj.Modified_By;
                                            item.Modified_Date_And_Time = obj.Modified_Date_And_Time;
                                            if (obj.Primary_Diagnosis == "Y")
                                                item.Plan_Reference = "PRIMARY";
                                            else
                                                item.Plan_Reference = "NONE";
                                            if (MacraICDlst.IndexOf(obj.ICD) != -1)
                                            {
                                                item.Plan_Reference = "OTHER";
                                            }
                                            foreach (string s in sMacraICDChkList)
                                            {
                                                string icdParent = s.Split('|')[0];
                                                string icdChild = s.Split('|')[1];
                                                if (MacraICDlst.IndexOf(icdParent) != -1 && obj.ICD.ToString().Trim().Equals(icdChild.ToString().Trim()))
                                                {
                                                    item.Plan_Reference = "OTHER";
                                                }

                                            }
                                            Update_Tplan.Add(item);
                                        }

                                    }

                                }
                            }

                            if (ListToDelete != null && ListToDelete.Count > 0)
                                if (objTreatmentPlan.Count > 0)
                                {
                                    Delete_Tplan = (from item in objTreatmentPlan where ListToDelete.Any(a => a.Id == item.Source_ID) select item).ToList();
                                }

                            if (generalNotesInsert != null && generalNotesInsert.Count > 0)
                            {
                                TreatmentPlan tp = null;
                                IList<TreatmentPlan> TplanNotes = (from obj in Insert_Tplan where (obj.Plan.IndexOf("(ICD-") == -1) select obj).ToList<TreatmentPlan>();
                                if (TplanNotes != null && TplanNotes.Count > 0)
                                {
                                    tp = TplanNotes[0];
                                    Insert_Tplan.Remove(tp);
                                    tp.Source_ID = generalNotesInsert[0].Id;
                                    Insert_Tplan.Add(tp);
                                }

                            }

                            if ((Insert_Tplan != null && Insert_Tplan.Count > 0) || (Update_Tplan != null && Update_Tplan.Count > 0) || (Delete_Tplan != null && Delete_Tplan.Count > 0))
                            {
                                iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, Delete_Tplan, MySession, sMacAddress, true, true, ulEncounterID, string.Empty, ref xmlobjEncounter);

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
                                        MySession.Close();
                                        throw new Exception("Deadlock occurred. Transaction failed.");
                                    }
                                }
                                else if (iResult == 1)
                                {
                                    trans.Rollback();
                                    MySession.Close();
                                    throw new Exception("Exception occurred. Transaction failed.");
                                }
                            }



                            #endregion
                            #region EandmICd
                            EandMCodingICDManager eandmicdMngr = new EandMCodingICDManager();
                            IList<EandMCodingICD> lstupdateicd = new List<EandMCodingICD>();
                            WFObjectManager objwf = new WFObjectManager();
                            WFObject lstobj = new WFObject();
                            lstobj = objwf.GetByObjectSystemId(ulEncounterID, "BILLING");
                            if (lstobj != null)
                            {
                                if (lstobj.Current_Process.ToString().ToUpper() != "BATCHING_COMPLETE")
                                {

                                    //if (lstEandMICDInsert.Count > 0 )
                                    if (lstEandMICDInsert.Count > 0 || lstEandMICDDelete.Count > 0)
                                    {
                                        //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                                        //{
                                        //    ISQLQuery uqery = iMySession.CreateSQLQuery("DELETE r.* FROM e_m_coding_icd" +
                                        //    " as r Where r.Encounter_ID=" + ulEncounterID + " and source='ASSESSMENT'").AddEntity("r", typeof(EandMCodingICD));
                                        //    int uqery_result = uqery.ExecuteUpdate();
                                        //    iMySession.Close();
                                        //}
                                        //string FileNameICD = "Encounter" + "_" + ulEncounterID + ".xml";
                                        //string strXmlFilePathICD = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileNameICD);
                                
                                        //if (File.Exists(strXmlFilePathICD) == true)
                                        //{
                                        //    XmlDocument itemDocICD = new XmlDocument();
                                        //    XmlTextReader XmlTextICD = new XmlTextReader(strXmlFilePathICD);
                                        //    itemDocICD.Load(XmlTextICD);
                                       
                                        //    XmlNodeList xmlCC = itemDocICD.GetElementsByTagName("EandMCodingICDList");
                                        //    if (xmlCC != null && xmlCC.Count > 0)
                                        //    {

                                        //        XmlNodeList xmlRemove = itemDocICD.GetElementsByTagName("EandMCodingICD");
                                        //        if (xmlRemove != null && xmlRemove.Count > 0)
                                        //        {
                                        //            xmlRemove[0].ParentNode.RemoveAll();
                                        //            //for (int n = 0; n < xmlRemove.Count; n++)
                                        //            //{
                                        //            //    if (xmlRemove[n].Attributes["Source"].InnerText.ToString().ToUpper().Trim()=="ASSESSMENT")
                                        //            //    xmlRemove[0].ParentNode.RemoveChild(xmlRemove[n]);

                                        //            //}

                                        //        }
                                               
                                        //    }
                                        //    XmlTextICD.Close();

                                        //    //itemDoc.Save(strXmlFilePath);
                                        //    int trycount = 0;
                                        //    trytosaveagain:
                                        //        try
                                        //        {
                                        //        itemDocICD.Save(strXmlFilePathICD);
                                        //        }
                                        //        catch (Exception xmlexcep)
                                        //        {
                                        //            trycount++;
                                        //            if (trycount <= 3)
                                        //            {
                                        //                int TimeMilliseconds = 0;
                                        //                if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                        //                    TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                        //                Thread.Sleep(TimeMilliseconds);
                                        //                string sMsg = string.Empty;
                                        //                string sExStackTrace = string.Empty;

                                        //                string version = "";
                                        //                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                        //                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                        //                string[] server = version.Split('|');
                                        //                string serverno = "";
                                        //                if (server.Length > 1)
                                        //                    serverno = server[1].Trim();

                                        //                if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                        //                    sMsg = xmlexcep.InnerException.Message;
                                        //                else
                                        //                    sMsg = xmlexcep.Message;

                                        //                if (xmlexcep != null && xmlexcep.StackTrace != null)
                                        //                    sExStackTrace = xmlexcep.StackTrace;

                                        //                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                        //                string ConnectionData;
                                        //                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                        //                using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                        //                {
                                        //                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                        //                    {
                                        //                        cmd.Connection = con;
                                        //                        try
                                        //                        {
                                        //                            con.Open();
                                        //                            cmd.ExecuteNonQuery();
                                        //                            con.Close();
                                        //                        }
                                        //                        catch
                                        //                        {
                                        //                        }
                                        //                    }
                                        //                }'''
                                        //                goto trytosaveagain;
                                        //            }
                                        //        }

                                            
                                        //}


                                        iResult = eandmicdMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstEandMICDInsert, ref lstupdateicd, lstEandMICDDelete, MySession, string.Empty, true, true, ulEncounterID, string.Empty, ref xmlobjEncounter);
                                        //iResult = eandmicdMngr.SaveUpdateDeleteWithoutTransaction(ref SaveEMICDList, UpdateEMICDList, null, MySession, string.Empty);

                                        if (iResult == 2)
                                        {
                                            if (iTryCount < 5)
                                            {
                                                iTryCount++;
                                            }
                                            else
                                            {
                                                trans.Rollback();
                                                throw new Exception("Deadlock is occured. Transaction failed");
                                            }
                                        }
                                        else if (iResult == 1)
                                        {
                                            trans.Rollback();
                                            throw new Exception("Exception is occured. Transaction failed");
                                        }
                                        IList<EandMCodingICD> CombICDConsischk = new List<EandMCodingICD>();
                                        if (lstEandMICDInsert != null)
                                        {
                                            foreach (EandMCodingICD obj in lstEandMICDInsert)
                                            {
                                                CombICDConsischk.Add(obj);
                                            }
                                        }


                                        //  bEandMCodingICDConsistent = XMLObj.CheckDataConsistency(CombICDConsischk.Cast<object>().ToList(), true, string.Empty);
                                    }
                                }
                            }
                            #endregion

                        
                            if (objAssessmentConsistent && objProblemlistConsistent && objGeneralNotesConsistent)
                            {

                                if (xmlobjEncounter.itemDoc.InnerXml != null && xmlobjEncounter.itemDoc.InnerXml != "")
                                {
                                    // xmlobjEncounter.itemDoc.Save(xmlobjEncounter.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {


                                        //if (FileName.Contains("Human"))
                                        //{
                                        //    WriteBlob("Human", ulHumanID, xmlobjEncounter.itemDoc, MySession, ListToInsert, ListToUpdate, ListToDelete, xmlobjEncounter);
                                        //}
                                        //if  (FileName.Contains("Encounter"))
                                        //{
                                            WriteBlob(ulEncounterID, xmlobjEncounter.itemDoc, MySession, ListToInsert, ListToUpdate, ListToDelete, xmlobjEncounter,false);
                                        //}

                                        // xmlobjEncounter.itemDoc.Save(xmlobjEncounter.strXmlFilePath);
                                    }
                                    catch (Exception xmlexcep)
                                    {
                                        trycount++;
                                        if (trycount <= 3)
                                        {
                                            int TimeMilliseconds = 0;
                                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                            Thread.Sleep(TimeMilliseconds);
                                            string sMsg = string.Empty;
                                            string sExStackTrace = string.Empty;

                                            string version = "";
                                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                            string[] server = version.Split('|');
                                            string serverno = "";
                                            if (server.Length > 1)
                                                serverno = server[1].Trim();

                                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                                sMsg = xmlexcep.InnerException.Message;
                                            else
                                                sMsg = xmlexcep.Message;

                                            if (xmlexcep != null && xmlexcep.StackTrace != null)
                                                sExStackTrace = xmlexcep.StackTrace;

                                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                            string ConnectionData;
                                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                            {
                                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                                {
                                                    cmd.Connection = con;
                                                    try
                                                    {
                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                            }
                                            goto trytosaveagain;
                                        }
                                    }

                                }
                                if (xmlobjHuman.itemDoc.InnerXml != null && xmlobjHuman.itemDoc.InnerXml != "")
                                {
                                    //  xmlobjHuman.itemDoc.Save(xmlobjHuman.strXmlFilePath);

                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        //  xmlobjHuman.itemDoc.Save(xmlobjHuman.strXmlFilePath);
                                        //if (FileName.Contains("Human"))

                                       
                                        //{
                                        if (updatedList.Count > 0)

                                            problemMngr.WriteBlob( ulHumanID, xmlobjHuman.itemDoc, MySession, ListToInsertProblemList, updatedList, ListToDeleteProblemList, xmlobjHuman, false);
                                        else
                                            problemMngr.WriteBlob(ulHumanID, xmlobjHuman.itemDoc, MySession, ListToInsertProblemList, ListToUpdateProblemList, ListToDeleteProblemList, xmlobjHuman, false);
                                        
                                        // }
                                        //else if (FileName.Contains("Encounter"))
                                        //{
                                        //    WriteBlob("Encounter", ulEncounterID, xmlobjHuman.itemDoc, MySession, ListToInsert, ListToUpdate, ListToDelete, xmlobjHuman);
                                        //}
                                    }
                                    catch (Exception xmlexcep)
                                    {
                                        trycount++;
                                        if (trycount <= 3)
                                        {
                                            int TimeMilliseconds = 0;
                                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                            Thread.Sleep(TimeMilliseconds);
                                            string sMsg = string.Empty;
                                            string sExStackTrace = string.Empty;

                                            string version = "";
                                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                            string[] server = version.Split('|');
                                            string serverno = "";
                                            if (server.Length > 1)
                                                serverno = server[1].Trim();

                                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                                sMsg = xmlexcep.InnerException.Message;
                                            else
                                                sMsg = xmlexcep.Message;

                                            if (xmlexcep != null && xmlexcep.StackTrace != null)
                                                sExStackTrace = xmlexcep.StackTrace;

                                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                            string ConnectionData;
                                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                            {
                                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                                {
                                                    cmd.Connection = con;
                                                    try
                                                    {
                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                            }
                                            goto trytosaveagain;
                                        }
                                    }
                                }

                                //For Bug Id 56330
                                //if (ListToInsertAssessment != null && ListToInsertAssessment.Count > 0 && ListToUpdate != null && ListToUpdate.Count > 0)
                                //{
                                //    GenerateXml objGenXml = new GenerateXml();
                                //    EncounterORHumanId = ListToInsertAssessment[0].Encounter_ID;
                                //    IList<Assessment> lstInsertpdate = new List<Assessment>();
                                //    lstInsertpdate = ListToInsertAssessment.Concat(ListToUpdate).ToList();
                                //    List<object> lstObj = lstInsertpdate.Cast<object>().ToList();
                                //    objGenXml.itemDoc = null;
                                //    objGenXml.GenerateXmlSave(lstObj, EncounterORHumanId, string.Empty, false, false, true, false, objGenXml);
                                //}
                                //if (ListToInsertAssessment != null && ListToInsertAssessment.Count > 0 && ListToUpdate != null && ListToUpdate.Count == 0)
                                //{
                                //    GenerateXml objGenXml = new GenerateXml();
                                //    EncounterORHumanId = ListToInsertAssessment[0].Encounter_ID;
                                //    List<object> lstObj = ListToInsertAssessment.Cast<object>().ToList();
                                //    objGenXml.itemDoc = null;
                                //    objGenXml.GenerateXmlSave(lstObj, EncounterORHumanId, string.Empty, false, false, true, false, objGenXml);
                                //}
                                //if (ListToInsertAssessment != null && ListToInsertAssessment.Count == 0 && ListToUpdate != null && ListToUpdate.Count > 0)
                                //{
                                //    GenerateXml objGenXml = new GenerateXml();
                                //    EncounterORHumanId = ListToUpdate[0].Encounter_ID;
                                //    List<object> lstObj = ListToUpdate.Cast<object>().ToList();
                                //    objGenXml.itemDoc = null;
                                //    objGenXml.GenerateXmlSave(lstObj, EncounterORHumanId, string.Empty, false, false, true, false, objGenXml);
                                //}
                                //if (ListToDelete != null && ListToDelete.Count > 0)
                                //{
                                //    GenerateXml objGenXml = new GenerateXml();
                                //    EncounterORHumanId = ListToDelete[0].Encounter_ID;
                                //    List<object> lstObj = ListToDelete.Cast<object>().ToList();
                                //    objGenXml.GenerateXmlDelete(lstObj, EncounterORHumanId, string.Empty, true, objGenXml);
                                 
                                //   // WriteBlob( ulEncounterID, objGenXml.itemDoc, MySession, ListToInsert, ListToUpdate, ListToDelete, objGenXml, false);

                                //}
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
                            throw new Exception(ex.Message, ex);
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
                    throw new Exception(ex1.Message, ex1);
                }

                #region Insert into Rcopia


                //Added by Selvaraman for RCopiaUpload on 27th Jun 2019
                RCopiaThread rcopThread = new RCopiaThread(ListToInsert, ListToUpdate, ListToDelete, humanRecord, UserName, sMacAddress, sLegalOrg);
                Thread threadRCopia = new Thread(rcopThread.UploadProblemListToRCopia);
                threadRCopia.Start();



                // UploadProblemListToRCopia(ListToInsert, ListToUpdate, ListToDelete, humanRecord, sMacAddress);

                ////Added by velmurugan for Rcopia on 14.05.11
                //    RCopiaTransactionManager objrcoptranMngr = new RCopiaTransactionManager();
                //    if (ListToInsert != null && ListToInsert.Count > 0)
                //    {
                //        IList<ulong> ilstinsertId = new List<ulong>();
                //        IList<Assessment> ilstassessment = null;
                //        for (int x = 0; x < ListToInsert.Count; x++)
                //        {
                //            ilstinsertId.Add(ListToInsert[x].Id);
                //        }
                //        if (ilstinsertId != null && ilstinsertId.Count > 0)
                //        {
                //            if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
                //            {
                //                objrcoptranMngr.SendPatientToRCopia(id, sMacAddress);
                //            }
                //            objrcoptranMngr.SendProblemToRCopia(ilstinsertId, "Assesment", false, ilstassessment);
                //        }
                //    }
                //    if (ListToUpdate != null && ListToUpdate.Count > 0)
                //    {
                //        IList<ulong> ilstUpdateId = new List<ulong>();
                //        IList<Assessment> ilstassessment = null;
                //        for (int y = 0; y < ListToUpdate.Count; y++)
                //        {
                //            ilstUpdateId.Add(ListToUpdate[y].Id);
                //        }
                //        if (ilstUpdateId != null && ilstUpdateId.Count > 0)
                //        {
                //            if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
                //            {
                //                objrcoptranMngr.SendPatientToRCopia(id, sMacAddress);
                //            }
                //            objrcoptranMngr.SendProblemToRCopia(ilstUpdateId, "Assesment", false, ilstassessment);
                //        }
                //    }
                //    if (ListToDelete != null && ListToDelete.Count > 0)
                //    {
                //        IList<ulong> ilstDeleteId = new List<ulong>();
                //        for (int z = 0; z < ListToDelete.Count; z++)
                //        {
                //            ilstDeleteId.Add(ListToDelete[z].Id);
                //        }
                //        if (ilstDeleteId != null && ilstDeleteId.Count > 0)
                //        {
                //            if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
                //            {
                //                objrcoptranMngr.SendPatientToRCopia(id, sMacAddress);
                //            }
                //            objrcoptranMngr.SendProblemToRCopia(ilstDeleteId, "Assesment", true, ListToDelete);
                //        }
                //    }
                #endregion
            }

            return LoadAssessment(ulEncounterID, ulHumanID, strIsRuledOut, UserName, "reload");
        }



        //Commented for  bug ID 55981
        //public FillAssessment BatchOperationsToAssessment(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate, IList<Assessment> ListToDelete, IList<ProblemList> ListToInsertProblemList, IList<ProblemList> ListToUpdateProblemList, IList<ProblemList> ListToDeleteProblemList, string sMacAddress, GeneralNotes generalNotes, TreatmentPlan SavePlan, string UserName, ulong ulEncounterID, ulong ulHumanID, string Is_Sent_To_RCopia, ulong PhysicianID, IList<string> sMacraICDChkList)
        //{
        //    iTryCount = 0;
        //    IList<GeneralNotes> generalNotesInsert = new List<GeneralNotes>();
        //    IList<GeneralNotes> generalNotesUpdate = new List<GeneralNotes>();

        //    string sPlan = string.Empty;
        //    IList<ProblemList> acsProblemlist = new List<ProblemList>();
        //    ulong[] proList = new ulong[ListToUpdateProblemList.Count];
        //    if (ListToUpdateProblemList.Count > 0)
        //    {
        //        acsProblemlist = ListToUpdateProblemList.OrderBy(e => e.Id).ToList();
        //        for (int i = 0; i < ListToUpdateProblemList.Count; i++)
        //            proList[i] = acsProblemlist[i].Id;
        //    }
        //    ProblemListManager problemMngr = new ProblemListManager();
        //    ulong humanId = 0;
        //    IList<ProblemList> probLst = new List<ProblemList>();
        //    TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
        //    IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
        //    IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
        //    IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();
        //    IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
        //    IList<AssessmentVitalsLookup> AssessmentVitalsLookupList = new List<AssessmentVitalsLookup>();
        //    AssessmentVitalsLookupManager objAssessVitalsMngr = new AssessmentVitalsLookupManager();
        //    AssessmentVitalsLookupList = objAssessVitalsMngr.GetByCriteria(Restrictions.Eq("Is_Macra_Field", "Y"));
        //    IList<string> MacraICDlst = AssessmentVitalsLookupList.Select(a => a.ICD_10).ToList<string>();

        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {

        //        if (ListToInsertProblemList.Count > 0)
        //        {
        //            humanId = ListToInsertProblemList[0].Human_ID;

        //        }
        //        if (ListToUpdateProblemList.Count > 0)
        //        {
        //            humanId = ListToUpdateProblemList[0].Human_ID;
        //        }
        //        if (ListToDeleteProblemList.Count > 0)
        //        {
        //            humanId = ListToDeleteProblemList[0].Human_ID;
        //        }
        //        //probLst = problemMngr.GetICDByHumanId(humanId);

        //    }
        //    #region TreatmentPlan Get
        //    string FileName = "Encounter" + "_" + ulEncounterID + ".xml";
        //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
        //    if (File.Exists(strXmlFilePath) == true)
        //    {
        //        XmlDocument itemDoc = new XmlDocument();
        //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
        //        XmlNodeList xmlTagName = null;
        //        // itemDoc.Load(XmlText);
        //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        {
        //            itemDoc.Load(fs);

        //            XmlText.Close();
        //            if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
        //            {
        //                xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

        //                if (xmlTagName.Count > 0)
        //                {
        //                    for (int j = 0; j < xmlTagName.Count; j++)
        //                    {
        //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == ulEncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("ASSESSMENT"))
        //                        {

        //                            string TagName = xmlTagName[j].Name;
        //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
        //                            TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
        //                            IEnumerable<PropertyInfo> propInfo = null;
        //                            TreatmentPlan = (TreatmentPlan)TreatmentPlan;
        //                            propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

        //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
        //                            {

        //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
        //                                {
        //                                    foreach (PropertyInfo property in propInfo)
        //                                    {
        //                                        if (property.Name == nodevalue.Name)
        //                                        {
        //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
        //                                                property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
        //                                                property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
        //                                                property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
        //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
        //                                                property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
        //                                            else
        //                                                property.SetValue(TreatmentPlan, nodevalue.Value, null);
        //                                        }
        //                                    }
        //                                }

        //                            }
        //                            objTreatmentPlan.Add(TreatmentPlan);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion
        //TryAgain:
        //    int iResult = 0;
        //    //Session.GetISession().Clear();
        //    ulong id = 0;
        //    ulong phyId = 0;
        //    string strIsRuledOut = string.Empty;
        //    Human humanRecord = new Human();
        //    ulong EncounterORHumanId = 0;
        //    GenerateXml xmlobjEncounter = new GenerateXml();
        //    GenerateXml xmlobjHuman = new GenerateXml();
        //    String GeneralNotesText = string.Empty;
        //    bool objAssessmentConsistent = true, objProblemlistConsistent = true, objGeneralNotesConsistent = true;
        //    using (ISession MySession = Session.GetISession())
        //    {
        //        try
        //        {
        //            using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //            {
        //                HumanManager hManager = new HumanManager();


        //                #region GetHumanRecordForRCopia
        //                if (ListToInsert != null && ListToInsert.Count > 0)
        //                {
        //                    id = ListToInsert[0].Human_ID;
        //                }
        //                else if (ListToUpdate != null && ListToUpdate.Count > 0)
        //                {
        //                    id = ListToUpdate[0].Human_ID;
        //                }
        //                else if (ListToDelete != null && ListToDelete.Count > 0)
        //                {
        //                    id = ListToDelete[0].Human_ID;
        //                }

        //                //humanRecord = hManager.GetById(id);
        //                humanRecord.Is_Sent_To_Rcopia = Is_Sent_To_RCopia;
        //                #endregion

        //                try
        //                {

        //                    IList<Assessment> ListToInsertAssessment = new List<Assessment>();
        //                    ListToInsertAssessment = ListToInsert.OrderByDescending(a => a.Primary_Diagnosis).ToList<Assessment>();

        //                    #region Assessment

        //                    //Added Latha - 2/8/10 - Checking for null value.
        //                    if ((ListToInsertAssessment != null) && (ListToUpdate != null) && (ListToDelete != null))
        //                    {
        //                        if ((ListToInsertAssessment.Count > 0) || (ListToUpdate.Count > 0) || (ListToDelete.Count > 0))
        //                        {
        //                            if (ListToInsertAssessment.Count == 0)
        //                            {
        //                                ListToInsertAssessment = null;

        //                            }
        //                            else if (ListToUpdate.Count == 0)
        //                            {
        //                                ListToUpdate = null;
        //                            }
        //                            else if (ListToDelete.Count == 0)
        //                            {
        //                                ListToDelete = null;
        //                            }

        //                            //To assign physician id and whether selected assessment or ruled out assessment which is needed in the load function parameter.
        //                            if (ListToInsertAssessment != null && ListToInsertAssessment.Count > 0)
        //                            {
        //                                AssignPhyIdAndIsRuledOutForLoadFunction(ref phyId, ref strIsRuledOut, ListToInsertAssessment);
        //                            }
        //                            else if (ListToUpdate != null && ListToUpdate.Count > 0)
        //                            {
        //                                AssignPhyIdAndIsRuledOutForLoadFunction(ref phyId, ref strIsRuledOut, ListToUpdate);
        //                            }
        //                            else if (ListToDelete != null && ListToDelete.Count > 0)
        //                            {
        //                                AssignPhyIdAndIsRuledOutForLoadFunction(ref phyId, ref strIsRuledOut, ListToDelete);
        //                            }
        //                            #region

        //                            if (ListToInsertAssessment != null && ListToInsertAssessment.Count > 0)
        //                            {
        //                                EncounterORHumanId = ListToInsertAssessment[0].Encounter_ID;
        //                            }
        //                            else if (ListToUpdate != null && ListToUpdate.Count > 0)
        //                                EncounterORHumanId = ListToUpdate[0].Encounter_ID;
        //                            else
        //                                EncounterORHumanId = ListToDelete[0].Encounter_ID;
        //                            #endregion
        //                            if (ListToInsertAssessment == null)
        //                                ListToInsertAssessment = new List<Assessment>();
        //                            if (ListToUpdate == null)
        //                                ListToUpdate = new List<Assessment>();

        //                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ListToInsertAssessment, ref ListToUpdate, ListToDelete, MySession, sMacAddress, true, true, EncounterORHumanId, string.Empty, ref xmlobjEncounter);
        //                            // iResult = SaveUpdateDeleteWithoutTransaction(ref ListToInsertAssessment, ListToUpdate, ListToDelete, MySession, sMacAddress);
        //                            //if bResult = false then, the deadlock is occured 
        //                            if (iResult == 2)
        //                            {
        //                                if (iTryCount < 5)
        //                                {
        //                                    iTryCount++;
        //                                    goto TryAgain;
        //                                }
        //                                else
        //                                {
        //                                    trans.Rollback();
        //                                    //MySession.Close();
        //                                    throw new Exception("Deadlock occurred. Transaction failed.");
        //                                }
        //                            }
        //                            else if (iResult == 1)
        //                            {
        //                                trans.Rollback();
        //                                // MySession.Close();
        //                                throw new Exception("Exception occurred. Transaction failed.");
        //                            }
        //                        }
        //                    }
        //                    if (ListToUpdate != null)
        //                        objAssessmentConsistent = xmlobjEncounter.CheckDataConsistency(ListToInsertAssessment.Concat(ListToUpdate).Cast<object>().ToList(), false, string.Empty);
        //                    else
        //                        objAssessmentConsistent = xmlobjEncounter.CheckDataConsistency(ListToInsertAssessment.Cast<object>().ToList(), false, string.Empty);

        //                    #endregion

        //                    #region unused Methods
        //                    //ICriteria crit = MySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", encounterId));

        //                    //Added Latha - 2/8/10 - Checking for null value.
        //                    //if (assList != null && ListToInsertProblemList != null && ListToUpdateProblemList != null)
        //                    //{
        //                    //    for (int i = 0; i < assList.Count; i++)
        //                    //    {
        //                    //        //for (int j = 0; j < ListToInsertAssessment.Count; j++)
        //                    //        //{
        //                    //        //    if (assList[i].ICD_9 == ListToInsertAssessment[j].ICD_9)
        //                    //        //    {
        //                    //        //        if (ListToInsertSource[j].Assessment_Id == 0)
        //                    //        //        {
        //                    //        //            ListToInsertSource[j].Assessment_Id = assList[i].Id;
        //                    //        //            break;
        //                    //        //        }
        //                    //        //    }
        //                    //        //}
        //                    //        for (int k = 0; k < ListToInsertProblemList.Count; k++)
        //                    //        {
        //                    //            if ((assList[i].Chronic_Problem.ToUpper() == "PROBLEM") || (assList[i].Chronic_Problem.ToUpper() == "BOTH"))
        //                    //            {
        //                    //                if ((ListToInsertProblemList[k].Reference_ID == 0) && (assList[i].ICD_9 == ListToInsertProblemList[k].ICD_Code))
        //                    //                {
        //                    //                    ListToInsertProblemList[k].Reference_ID = assList[i].Id;
        //                    //                    break;
        //                    //                }
        //                    //            }
        //                    //        }
        //                    //        for (int t = 0; t < ListToUpdateProblemList.Count; t++)
        //                    //        {
        //                    //            if ((assList[i].Chronic_Problem.ToUpper() == "PROBLEM") || (assList[i].Chronic_Problem.ToUpper() == "BOTH"))
        //                    //            {
        //                    //                if ((ListToUpdateProblemList[t].Reference_ID == 0) && (assList[i].ICD_9 == ListToUpdateProblemList[t].ICD_Code))
        //                    //                {
        //                    //                    ListToUpdateProblemList[t].Reference_ID = assList[i].Id;
        //                    //                    break;
        //                    //                }
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //}
        //                    //if(ListToUpdate != null && ListToInsertProblemList != null && ListToUpdateProblemList != null)
        //                    //{
        //                    //    for (int i = 0; i < ListToUpdate.Count; i++)
        //                    //    {
        //                    //        for (int k = 0; k < ListToInsertProblemList.Count; k++)
        //                    //        {
        //                    //            if ((ListToUpdate[i].Chronic_Problem.ToUpper() == "PROBLEM") || (ListToUpdate[i].Chronic_Problem.ToUpper() == "BOTH"))
        //                    //            {
        //                    //                if ((ListToInsertProblemList[k].Reference_ID == 0) && (ListToUpdate[i].ICD_9 == ListToInsertProblemList[k].ICD_Code))
        //                    //                {
        //                    //                    ListToInsertProblemList[k].Reference_ID = ListToUpdate[i].Id;
        //                    //                    break;
        //                    //                }
        //                    //            }
        //                    //        }
        //                    //        for (int t = 0; t < ListToUpdateProblemList.Count; t++)
        //                    //        {
        //                    //            if ((ListToUpdate[i].Chronic_Problem.ToUpper() == "PROBLEM") || (ListToUpdate[i].Chronic_Problem.ToUpper() == "BOTH"))
        //                    //            {
        //                    //                if ((ListToUpdateProblemList[t].Reference_ID == 0) && (ListToUpdate[i].ICD_9 == ListToUpdateProblemList[t].ICD_Code))
        //                    //                {
        //                    //                    ListToUpdateProblemList[t].Reference_ID = ListToUpdate[i].Id;
        //                    //                    break;
        //                    //                }
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //}
        //                    //Added Latha - 2/8/10 - Checking for null value.
        //                    //if ((ListToInsertSource != null) && (ListToUpdateSource != null) && (ListToDeleteSource != null))
        //                    //{
        //                    //    if ((ListToInsertSource.Count > 0) || (ListToUpdateSource.Count > 0) || (ListToDeleteSource.Count > 0))
        //                    //    {
        //                    //        AssessmentSourceManager assessmentSourceMngr = new AssessmentSourceManager();
        //                    //        bResult = assessmentSourceMngr.BatchOperationsToAssessmentSource(ListToInsertSource, ListToUpdateSource, ListToDeleteSource, MySession, sMacAddress);

        //                    //        //if bResult = false then, the deadlock is occured 
        //                    //        if (bResult == false)
        //                    //        {
        //                    //            if (iTryCount < 5)
        //                    //            {
        //                    //                iTryCount++;
        //                    //                goto TryAgain;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //}

        //                    #endregion

        //                    #region problemList
        //                    //Added Latha - 2/8/10 - Checking for null value.


        //                    IList<Assessment> assList = null;
        //                    assList = ListToInsertAssessment;
        //                    IList<ProblemList> updatedList = new List<ProblemList>();
        //                    if ((ListToInsertProblemList != null) && (ListToUpdateProblemList != null) && (ListToDeleteProblemList != null))
        //                    {
        //                        if ((ListToInsertProblemList.Count > 0) || (ListToUpdateProblemList.Count > 0) || (ListToDeleteProblemList.Count > 0))
        //                        {
        //                            IList<ProblemList> prob = new List<ProblemList>();


        //                            if (ListToUpdateProblemList.Count > 0)
        //                            {
        //                                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //                                {
        //                                    ICriteria criteriaProblemList = iMySession.CreateCriteria(typeof(ProblemList)).Add(Expression.In("Id", proList)).AddOrder(Order.Asc("Id"));
        //                                    prob = criteriaProblemList.List<ProblemList>();
        //                                }

        //                                for (int i = 0; i < prob.Count; i++)
        //                                    acsProblemlist[i].Version = prob[i].Version;

        //                                ListToUpdateProblemList = acsProblemlist;
        //                            }

        //                            DateTime currTime = DateTime.UtcNow;


        //                            if (ListToInsertProblemList.Count > 0)
        //                            {
        //                                humanId = ListToInsertProblemList[0].Human_ID;
        //                                currTime = ListToInsertProblemList[0].Created_Date_And_Time;
        //                            }
        //                            else if (ListToUpdateProblemList.Count > 0)
        //                            {
        //                                humanId = ListToUpdateProblemList[0].Human_ID;
        //                                currTime = ListToUpdateProblemList[0].Created_Date_And_Time;
        //                            }
        //                            else if (ListToDeleteProblemList.Count > 0)
        //                            {
        //                                humanId = ListToDeleteProblemList[0].Human_ID;
        //                                currTime = ListToDeleteProblemList[0].Created_Date_And_Time;
        //                            }

        //                            if (humanId != 0 && (ListToUpdateProblemList.Count != 0 || ListToInsertProblemList.Count != 0))
        //                            {

        //                                var GetActiveProb = from obj in probLst where obj.ICD == "0000" select obj;
        //                                if (GetActiveProb.ToList().Count > 0)
        //                                {
        //                                    ProblemList prblst = GetActiveProb.ToList<ProblemList>()[0];
        //                                    prblst.Is_Active = "N";
        //                                    prblst.Modified_By = UserName;
        //                                    prblst.Human_ID = humanId;
        //                                    prblst.Modified_Date_And_Time = currTime;
        //                                    updatedList = new List<ProblemList>(ListToUpdateProblemList);
        //                                    updatedList.Add(prblst);
        //                                }
        //                            }
        //                            //if (updatedList.Count > 0)
        //                            //    iResult = problemMngr.BatchOperationsToProblem(ListToInsertProblemList, updatedList, ListToDeleteProblemList, MySession, sMacAddress, false);
        //                            //else
        //                            //    iResult = problemMngr.BatchOperationsToProblem(ListToInsertProblemList, ListToUpdateProblemList, ListToDeleteProblemList, MySession, sMacAddress, false);

        //                            if (ListToInsertProblemList.Count > 0)
        //                                EncounterORHumanId = ListToInsertProblemList[0].Human_ID;
        //                            else if (updatedList.Count > 0)
        //                                EncounterORHumanId = updatedList[0].Human_ID;
        //                            else if (ListToDeleteProblemList.Count > 0)
        //                                EncounterORHumanId = ListToDeleteProblemList[0].Human_ID;
        //                            else
        //                                EncounterORHumanId = ListToUpdateProblemList[0].Human_ID;
        //                            if (updatedList.Count > 0)
        //                                iResult = problemMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ListToInsertProblemList, ref updatedList, ListToDeleteProblemList, MySession, sMacAddress, true, true, EncounterORHumanId, string.Empty, ref xmlobjHuman);
        //                            else
        //                                iResult = problemMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ListToInsertProblemList, ref ListToUpdateProblemList, ListToDeleteProblemList, MySession, sMacAddress, true, true, EncounterORHumanId, string.Empty, ref xmlobjHuman);

        //                            //if bResult = false then, the deadlock is occured 
        //                            if (iResult == 2)
        //                            {
        //                                if (iTryCount < 5)
        //                                {
        //                                    iTryCount++;
        //                                    goto TryAgain;
        //                                }
        //                                else
        //                                {
        //                                    trans.Rollback();
        //                                    //  MySession.Close();
        //                                    throw new Exception("Deadlock occurred. Transaction failed.");
        //                                }
        //                            }
        //                            else if (iResult == 1)
        //                            {
        //                                trans.Rollback();
        //                                //  MySession.Close();
        //                                throw new Exception("Exception occurred. Transaction failed.");
        //                            }
        //                        }
        //                    }

        //                    if (updatedList.Count > 0)
        //                        objProblemlistConsistent = xmlobjHuman.CheckDataConsistency(ListToInsertProblemList.Concat(updatedList).Cast<object>().ToList(), true, string.Empty);
        //                    else
        //                        if (ListToUpdateProblemList != null)
        //                            objProblemlistConsistent = xmlobjHuman.CheckDataConsistency(ListToInsertProblemList.Concat(ListToUpdateProblemList).Cast<object>().ToList(), true, string.Empty);
        //                        else
        //                            objProblemlistConsistent = xmlobjHuman.CheckDataConsistency(ListToInsertProblemList.Cast<object>().ToList(), true, string.Empty);
        //                    ////IList<AssessmentProblemListDifference> ListToUpdateDifference = new List<AssessmentProblemListDifference>();
        //                    //if ((ListToInsertAssessmentProbListDiff != null) && (ListToUpdateAssessmentProbListDiff != null) && (ListToDeleteAssessmentProbListDiff != null))
        //                    //{
        //                    //    if ((ListToInsertAssessmentProbListDiff.Count > 0) || (ListToUpdateAssessmentProbListDiff.Count > 0) || (ListToDeleteAssessmentProbListDiff.Count > 0))
        //                    //    {
        //                    //        AssessmentProblemListDifferenceManager assessmentProblemListDifferenceMgr = new AssessmentProblemListDifferenceManager();
        //                    //        bResult = assessmentProblemListDifferenceMgr.BatchOperationsToAssessmentProblemListDifference(ListToInsertAssessmentProbListDiff, ListToUpdateAssessmentProbListDiff, ListToDeleteAssessmentProbListDiff, MySession, sMacAddress);
        //                    //        //if bResult = false then, the deadlock is occured 
        //                    //        if (bResult == false)
        //                    //        {
        //                    //            if (iTryCount < 5)
        //                    //            {
        //                    //                iTryCount++;
        //                    //                goto TryAgain;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //}


        //                    #endregion

        //                    #region General Notes


        //                    if (generalNotes != null)
        //                    {
        //                        if (generalNotes.Id == 0)
        //                        {
        //                            generalNotesInsert.Add(generalNotes);
        //                            EncounterORHumanId = generalNotesInsert[0].Encounter_ID;
        //                            GeneralNotesText = generalNotesInsert[0].Parent_Field.Replace(" ", "");
        //                        }
        //                        else
        //                        {
        //                            generalNotesUpdate.Add(generalNotes);
        //                            EncounterORHumanId = generalNotesUpdate[0].Encounter_ID;
        //                            GeneralNotesText = generalNotesUpdate[0].Parent_Field.Replace(" ", "");

        //                        }
        //                    }
        //                    if (generalNotes != null)
        //                    {
        //                        if (generalNotes.Id != 0)
        //                        {
        //                            IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Source_ID == generalNotes.Id).ToList<TreatmentPlan>();
        //                            if (itemlst != null && itemlst.Count > 0)
        //                            {
        //                                TreatmentPlan item = itemlst[0];
        //                                item.Plan = "* " + generalNotes.Notes;
        //                                item.Modified_By = generalNotes.Modified_By;
        //                                item.Modified_Date_And_Time = generalNotes.Modified_Date_And_Time;
        //                                item.Plan_Reference = "GENERAL NOTES";
        //                                Update_Tplan.Add(item);
        //                            }
        //                        }
        //                        else if (generalNotes.Id == 0)
        //                        {
        //                            TreatmentPlan item = new TreatmentPlan();
        //                            item.Human_ID = generalNotes.Human_ID;
        //                            item.Encounter_Id = generalNotes.Encounter_ID;
        //                            item.Physician_Id = PhysicianID;
        //                            item.Created_By = generalNotes.Created_By;
        //                            item.Created_Date_And_Time = generalNotes.Created_Date_And_Time;
        //                            item.Plan_Type = "ASSESSMENT";
        //                            string plan_txt = string.Empty;
        //                            plan_txt = "* " + generalNotes.Notes;
        //                            item.Plan = plan_txt;
        //                            item.Version = 0;
        //                            item.Plan_Reference = "GENERAL NOTES";
        //                            Insert_Tplan.Add(item);
        //                        }
        //                        GeneralNotesManager genMgr = new GeneralNotesManager();
        //                        //iResult = genMgr.SaveUpdateDeleteGeneralNotes(generalNotesInsert, generalNotesUpdate, MySession, sMacAddress);
        //                        iResult = genMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref generalNotesInsert, ref generalNotesUpdate, null, MySession, sMacAddress, true, true, EncounterORHumanId, GeneralNotesText, ref xmlobjEncounter);
        //                        if (iResult == 2)
        //                        {
        //                            if (iTryCount < 5)
        //                            {
        //                                iTryCount++;
        //                                goto TryAgain;
        //                            }
        //                            else
        //                            {
        //                                trans.Rollback();
        //                                //MySession.Close();
        //                                throw new Exception("Deadlock occurred. Transaction failed.");
        //                            }
        //                        }
        //                        else if (iResult == 1)
        //                        {
        //                            trans.Rollback();
        //                            // MySession.Close();
        //                            throw new Exception("Exception occurred. Transaction failed.");
        //                        }
        //                        if (generalNotesUpdate != null)
        //                            objGeneralNotesConsistent = xmlobjEncounter.CheckDataConsistency(generalNotesInsert.Concat(generalNotesUpdate).Cast<object>().ToList(), true, "SelectedAssessment");
        //                        else
        //                            objGeneralNotesConsistent = xmlobjEncounter.CheckDataConsistency(generalNotesInsert.Cast<object>().ToList(), true, "SelectedAssessment");

        //                    }

        //                    #endregion
        //                    #region Treatment plan

        //                    if (ListToInsertAssessment != null && ListToInsertAssessment.Count > 0)
        //                    {
        //                        foreach (Assessment obj in ListToInsertAssessment)
        //                        {
        //                            if (obj.Diagnosis_Source.Trim().ToUpper() != "VITALS|DELETED")//BUGID:45554
        //                            {
        //                                TreatmentPlan item = new TreatmentPlan();
        //                                item.Human_ID = obj.Human_ID;
        //                                item.Encounter_Id = obj.Encounter_ID;
        //                                item.Physician_Id = obj.Physician_ID;
        //                                item.Created_By = obj.Created_By;
        //                                item.Created_Date_And_Time = obj.Created_Date_And_Time;
        //                                item.Plan_Type = "ASSESSMENT";
        //                                string plan_txt = string.Empty;
        //                                plan_txt = "* " + obj.ICD_Description + "(ICD- " + obj.ICD + ")" + (obj.Assessment_Notes.Trim() != string.Empty ? "-" + obj.Assessment_Notes : "") + (obj.Assessment_Status.Trim() != string.Empty ? "-" + obj.Assessment_Status : "");
        //                                item.Plan = plan_txt;
        //                                item.Version = 0;
        //                                item.Source_ID = obj.Id;
        //                                if (obj.Primary_Diagnosis == "Y")
        //                                    item.Plan_Reference = "PRIMARY";
        //                                else
        //                                    item.Plan_Reference = "NONE";
        //                                if (MacraICDlst.IndexOf(obj.ICD) != -1)
        //                                {
        //                                    item.Plan_Reference = "OTHER";
        //                                }
        //                                foreach (string s in sMacraICDChkList)
        //                                {
        //                                    string icdParent = s.Split('|')[0];
        //                                    string icdChild = s.Split('|')[1];
        //                                    if (MacraICDlst.IndexOf(icdParent) != -1 && obj.ICD.ToString().Trim().Equals(icdChild.ToString().Trim()))
        //                                    {
        //                                        item.Plan_Reference = "OTHER";
        //                                    }

        //                                }
        //                                Insert_Tplan.Add(item);
        //                            }
        //                        }
        //                    }
        //                    if (ListToUpdate != null && ListToUpdate.Count > 0)
        //                    {
        //                        foreach (Assessment obj in ListToUpdate)
        //                        {
        //                            if (objTreatmentPlan.Count > 0)
        //                            {
        //                                IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Source_ID == obj.Id).ToList();
        //                                if (itemlst != null && itemlst.Count > 0)
        //                                {
        //                                    TreatmentPlan item = itemlst[0];
        //                                    string plan_txt = string.Empty;
        //                                    plan_txt = "* " + obj.ICD_Description + "(ICD- " + obj.ICD + ")" + (obj.Assessment_Notes.Trim() != string.Empty ? "-" + obj.Assessment_Notes : "") + (obj.Assessment_Status.Trim() != string.Empty ? "-" + obj.Assessment_Status : "");
        //                                    item.Plan = plan_txt;
        //                                    item.Modified_By = obj.Modified_By;
        //                                    item.Modified_Date_And_Time = obj.Modified_Date_And_Time;
        //                                    if (obj.Primary_Diagnosis == "Y")
        //                                        item.Plan_Reference = "PRIMARY";
        //                                    else
        //                                        item.Plan_Reference = "NONE";
        //                                    if (MacraICDlst.IndexOf(obj.ICD) != -1)
        //                                    {
        //                                        item.Plan_Reference = "OTHER";
        //                                    }
        //                                    foreach (string s in sMacraICDChkList)
        //                                    {
        //                                        string icdParent = s.Split('|')[0];
        //                                        string icdChild = s.Split('|')[1];
        //                                        if (MacraICDlst.IndexOf(icdParent) != -1 && obj.ICD.ToString().Trim().Equals(icdChild.ToString().Trim()))
        //                                        {
        //                                            item.Plan_Reference = "OTHER";
        //                                        }

        //                                    }
        //                                    Update_Tplan.Add(item);
        //                                }

        //                            }

        //                        }
        //                    }

        //                    if (ListToDelete != null && ListToDelete.Count > 0)
        //                        if (objTreatmentPlan.Count > 0)
        //                        {
        //                            Delete_Tplan = (from item in objTreatmentPlan where ListToDelete.Any(a => a.Id == item.Source_ID) select item).ToList();
        //                        }

        //                    if (generalNotesInsert != null && generalNotesInsert.Count > 0)
        //                    {
        //                        TreatmentPlan tp = null;
        //                        IList<TreatmentPlan> TplanNotes = (from obj in Insert_Tplan where (obj.Plan.IndexOf("(ICD-") == -1) select obj).ToList<TreatmentPlan>();
        //                        if (TplanNotes != null && TplanNotes.Count > 0)
        //                        {
        //                            tp = TplanNotes[0];
        //                            Insert_Tplan.Remove(tp);
        //                            tp.Source_ID = generalNotesInsert[0].Id;
        //                            Insert_Tplan.Add(tp);
        //                        }

        //                    }

        //                    if ((Insert_Tplan != null && Insert_Tplan.Count > 0) || (Update_Tplan != null && Update_Tplan.Count > 0) || (Delete_Tplan != null && Delete_Tplan.Count > 0))
        //                    {
        //                        iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, Delete_Tplan, MySession, sMacAddress, true, true, ulEncounterID, string.Empty, ref xmlobjEncounter);

        //                        if (iResult == 2)
        //                        {
        //                            if (iTryCount < 5)
        //                            {
        //                                iTryCount++;
        //                                goto TryAgain;
        //                            }
        //                            else
        //                            {
        //                                trans.Rollback();
        //                                MySession.Close();
        //                                throw new Exception("Deadlock occurred. Transaction failed.");
        //                            }
        //                        }
        //                        else if (iResult == 1)
        //                        {
        //                            trans.Rollback();
        //                            MySession.Close();
        //                            throw new Exception("Exception occurred. Transaction failed.");
        //                        }
        //                    }



        //                    #endregion
        //                    if (objAssessmentConsistent && objProblemlistConsistent && objGeneralNotesConsistent)
        //                    {

        //                        if (xmlobjEncounter.strXmlFilePath != null && xmlobjEncounter.strXmlFilePath != "")
        //                        {
        //                            xmlobjEncounter.itemDoc.Save(xmlobjEncounter.strXmlFilePath);
        //                        }
        //                        if (xmlobjHuman.strXmlFilePath != null && xmlobjHuman.strXmlFilePath != "")
        //                        {
        //                            xmlobjHuman.itemDoc.Save(xmlobjHuman.strXmlFilePath);
        //                        }
        //                        trans.Commit();
        //                    }
        //                    else
        //                        throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");

        //                }
        //                catch (NHibernate.Exceptions.GenericADOException ex)
        //                {
        //                    trans.Rollback();
        //                    // MySession.Close();
        //                    throw new Exception(ex.Message);
        //                }
        //                catch (Exception e)
        //                {
        //                    trans.Rollback();
        //                    //MySession.Close();
        //                    throw new Exception(e.Message);
        //                }

        //                finally
        //                {
        //                    MySession.Close();
        //                }
        //            }
        //        }
        //        catch (Exception ex1)
        //        {
        //            //MySession.Close();
        //            throw new Exception(ex1.Message);
        //        }

        //        #region Insert into Rcopia

        //        //Added by velmurugan for Rcopia on 14.05.11

        //        RCopiaTransactionManager objrcoptranMngr = new RCopiaTransactionManager();
        //        if (ListToInsert != null && ListToInsert.Count > 0)
        //        {
        //            IList<ulong> ilstinsertId = new List<ulong>();
        //            IList<Assessment> ilstassessment = null;
        //            for (int x = 0; x < ListToInsert.Count; x++)
        //            {
        //                ilstinsertId.Add(ListToInsert[x].Id);
        //            }
        //            if (ilstinsertId != null && ilstinsertId.Count > 0)
        //            {
        //                if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
        //                {
        //                    objrcoptranMngr.SendPatientToRCopia(id, sMacAddress);
        //                }
        //                objrcoptranMngr.SendProblemToRCopia(ilstinsertId, "Assesment", false, ilstassessment);
        //            }
        //        }
        //        if (ListToUpdate != null && ListToUpdate.Count > 0)
        //        {
        //            IList<ulong> ilstUpdateId = new List<ulong>();
        //            IList<Assessment> ilstassessment = null;
        //            for (int y = 0; y < ListToUpdate.Count; y++)
        //            {
        //                ilstUpdateId.Add(ListToUpdate[y].Id);
        //            }
        //            if (ilstUpdateId != null && ilstUpdateId.Count > 0)
        //            {
        //                if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
        //                {
        //                    objrcoptranMngr.SendPatientToRCopia(id, sMacAddress);
        //                }
        //                objrcoptranMngr.SendProblemToRCopia(ilstUpdateId, "Assesment", false, ilstassessment);
        //            }
        //        }
        //        if (ListToDelete != null && ListToDelete.Count > 0)
        //        {
        //            IList<ulong> ilstDeleteId = new List<ulong>();
        //            IList<Assessment> ilstassessment = null;
        //            for (int z = 0; z < ListToDelete.Count; z++)
        //            {
        //                ilstDeleteId.Add(ListToDelete[z].Id);
        //            }
        //            if (ilstDeleteId != null && ilstDeleteId.Count > 0)
        //            {
        //                if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
        //                {
        //                    objrcoptranMngr.SendPatientToRCopia(id, sMacAddress);
        //                }
        //                objrcoptranMngr.SendProblemToRCopia(ilstDeleteId, "Assesment", true, ListToDelete);
        //            }
        //        }
        //    }
        //        #endregion
        //    //FillAssessment objFillAssessment = new FillAssessment();
        //    # region Commented
        //    //ulong EncounterID = 0;
        //    //ulong HumanID = 0;
        //    //if (ListToInsert.Count > 0)
        //    //{
        //    //    EncounterID = ListToInsert[0].Encounter_ID;
        //    //    HumanID = ListToInsert[0].Human_ID;
        //    //    phyId = ListToInsert[0].Physician_ID;
        //    //}
        //    //else if (ListToUpdate.Count > 0)
        //    //{
        //    //    EncounterID = ListToUpdate[0].Encounter_ID;
        //    //    HumanID = ListToUpdate[0].Human_ID;
        //    //    phyId = ListToUpdate[0].Physician_ID;
        //    //}

        //    //GenerateXml XMLObj = new GenerateXml();
        //    //if (ListToInsert != null && ListToInsert.Count > 0)
        //    //{
        //    //    ulong encounterid = ListToInsert[0].Encounter_ID;
        //    //    List<object> lstObj = ListToInsert.Cast<object>().ToList();
        //    //    XMLObj.GenerateXmlSaveStatic(lstObj, encounterid, string.Empty);
        //    //}
        //    //if (ListToUpdate != null && ListToUpdate.Count > 0)
        //    //{
        //    //    ulong encounterid = ListToUpdate[0].Encounter_ID;

        //    //    List<object> lstObj = ListToUpdate.Cast<object>().ToList();
        //    //    XMLObj.GenerateXmlUpdate(lstObj, encounterid, string.Empty);
        //    //}
        //    //if (ListToDelete != null && ListToDelete.Count > 0)
        //    //{
        //    //    ulong encounterid = ListToDelete[0].Encounter_ID;

        //    //    List<object> lstObj = ListToDelete.Cast<object>().ToList();
        //    //    XMLObj.DeleteXmlNode(encounterid, lstObj, string.Empty);
        //    //}
        //    //if (ListToInsertProblemList != null && ListToInsertProblemList.Count > 0)
        //    //{
        //    //    ulong uHumanID = ListToInsertProblemList[0].Human_ID;
        //    //    List<object> lstObj = ListToInsertProblemList.Cast<object>().ToList();
        //    //    XMLObj.GenerateXmlSaveStatic(lstObj, HumanID, string.Empty);
        //    //}
        //    //if (ListToUpdateProblemList != null && ListToUpdateProblemList.Count > 0)
        //    //{
        //    //    ulong uHumanID = ListToUpdateProblemList[0].Human_ID;
        //    //    foreach (var obj in ListToUpdateProblemList)
        //    //        obj.Version += 1;
        //    //    XMLObj.GenerateXmlUpdate(ListToUpdateProblemList.Cast<object>().ToList(), uHumanID, string.Empty);
        //    //}
        //    //if (ListToDeleteProblemList != null && ListToDeleteProblemList.Count > 0)
        //    //{
        //    //    ulong uHumanID = ListToDeleteProblemList[0].Human_ID;
        //    //    List<object> lstObj = ListToDeleteProblemList.Cast<object>().ToList();
        //    //    XMLObj.DeleteXmlNode(HumanID, lstObj, string.Empty);
        //    //}
        //    //if (generalNotesInsert != null && generalNotesInsert.Count > 0)
        //    //{
        //    //    ulong uEncounterId = generalNotesInsert[0].Encounter_ID;
        //    //    List<object> lstObj = generalNotesInsert.Cast<object>().ToList();
        //    //    XMLObj.GenerateXmlSave(lstObj, uEncounterId, generalNotesInsert[0].Parent_Field.Replace(" ", ""));
        //    //}
        //    //if (generalNotesUpdate != null && generalNotesUpdate.Count > 0)
        //    //{
        //    //    ulong uEncounterId = generalNotesUpdate[0].Encounter_ID;
        //    //    List<object> lstObj = generalNotesUpdate.Cast<object>().ToList();
        //    //    XMLObj.GenerateXmlSave(lstObj, uEncounterId, generalNotesUpdate[0].Parent_Field.Replace(" ", ""));
        //    //}

        //    //return objFillAssessment;
        //    #endregion
        //    return LoadAssessment(ulEncounterID, ulHumanID, strIsRuledOut, UserName, "reload");
        //}

        public void AssignPhyIdAndIsRuledOutForLoadFunction(ref ulong phyId, ref string strIsRuledOut, IList<Assessment> AssessmentList)
        {
            if (AssessmentList != null && AssessmentList.Count > 0)
            {
                phyId = AssessmentList[0].Physician_ID;
                if (AssessmentList[0].Assessment_Type != string.Empty && AssessmentList[0].Assessment_Type.ToUpper() == "SELECTED")
                {
                    strIsRuledOut = "N";
                }
                else if (AssessmentList[0].Assessment_Type != string.Empty && AssessmentList[0].Assessment_Type.ToUpper() == "RULED OUT")
                {
                    strIsRuledOut = "Y";
                }
            }
        }

        //public FillAssessmentRuledOut GetAssessmentUsingEncounterIDForRuledOut(ulong encounterId, ulong physicianId)
        //{

        //    //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //    FillAssessmentRuledOut fillAssessmentRuledOutDto = new FillAssessmentRuledOut();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria criteria = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", encounterId)).Add(Expression.Eq("Assessment_Type", "Ruled Out"));
        //        fillAssessmentRuledOutDto.Assessment = criteria.List<Assessment>();

        //        ICriteria criteriaGeneralNotes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", encounterId)).Add(Expression.Eq("Parent_Field", "Ruled Out Assessment"));
        //        fillAssessmentRuledOutDto.General_Notes = criteriaGeneralNotes.List<GeneralNotes>();

        //        //PhysicianICD_9Manager phyMgr = new PhysicianICD_9Manager();
        //        //fillAssessmentRuledOutDto.Physician_Icd = phyMgr.GetFavouriteFullBasedOnAssessmentTab(physicianId,"Y");
        //        iMySession.Close();
        //    }
        //    return fillAssessmentRuledOutDto;

        //}
        //Latha - 11 Aug 2011 - Single Service call - End

        //public FillAssessmentRuledOut BatchOperationsToAssessmentFromRuledOut(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate, IList<Assessment> ListToDelete, ulong encounterId, string sMacAddress, GeneralNotes generalNotes)
        //{
        //    iTryCount = 0;

        //TryAgain:
        //    int iResult = 0;
        //    ulong phyId = 0;
        //    using (ISession MySession = Session.GetISession())
        //    {
        //        //  ITransaction trans = null;

        //        try
        //        {
        //            using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //            {
        //                try
        //                {
        //                    if ((ListToInsert != null) && (ListToUpdate != null) && (ListToDelete != null))
        //                    {
        //                        if ((ListToInsert.Count != 0) || (ListToUpdate.Count != 0) || (ListToDelete.Count != 0))
        //                        {
        //                            #region Ruled Out Assessment
        //                            if (ListToInsert.Count == 0)
        //                            {
        //                                ListToInsert = null;
        //                            }
        //                            else if (ListToUpdate.Count == 0)
        //                            {
        //                                ListToUpdate = null;
        //                            }
        //                            else if (ListToDelete.Count == 0)
        //                            {
        //                                ListToDelete = null;
        //                            }
        //                            if (ListToInsert != null && ListToInsert.Count > 0)
        //                            {
        //                                phyId = ListToInsert[0].Physician_ID;
        //                            }
        //                            else if (ListToUpdate != null && ListToUpdate.Count > 0)
        //                            {
        //                                phyId = ListToUpdate[0].Physician_ID;
        //                            }
        //                            else if (ListToDelete != null && ListToDelete.Count > 0)
        //                            {
        //                                phyId = ListToDelete[0].Physician_ID;
        //                            }
        //                            //Commented on 24-03-2016 as it is not in use
        //                            //  iResult = SaveUpdateDeleteWithoutTransaction(ref ListToInsert, ListToUpdate, ListToDelete, MySession, sMacAddress);
        //                            //if bResult = false then, the deadlock is occured 
        //                            if (iResult == 2)
        //                            {
        //                                if (iTryCount < 5)
        //                                {
        //                                    iTryCount++;
        //                                    goto TryAgain;
        //                                }
        //                                else
        //                                {
        //                                    trans.Rollback();
        //                                    //MySession.Close();
        //                                    throw new Exception("Deadlock occurred. Transaction failed.");
        //                                }
        //                            }
        //                            else if (iResult == 1)
        //                            {
        //                                trans.Rollback();
        //                                //MySession.Close();
        //                                throw new Exception("Exception occurred. Transaction failed.");
        //                            }
        //                            #endregion


        //                        }
        //                    }

        //                    #region General Notes
        //                    IList<GeneralNotes> generalNotesInsert = new List<GeneralNotes>();
        //                    IList<GeneralNotes> generalNotesUpdate = new List<GeneralNotes>();

        //                    if (generalNotes.Id == 0)
        //                    {
        //                        generalNotesInsert.Add(generalNotes);
        //                    }
        //                    else
        //                    {
        //                        generalNotesUpdate.Add(generalNotes);
        //                    }


        //                    GeneralNotesManager genMgr = new GeneralNotesManager();
        //                    //Commented on 24-03-2016 as it is not in use
        //                    // iResult = genMgr.SaveUpdateDeleteGeneralNotes(generalNotesInsert, generalNotesUpdate, MySession, sMacAddress);
        //                    if (iResult == 2)
        //                    {
        //                        if (iTryCount < 5)
        //                        {
        //                            iTryCount++;
        //                            goto TryAgain;
        //                        }
        //                        else
        //                        {
        //                            trans.Rollback();
        //                            //MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        //MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                    #endregion

        //                    MySession.Flush();
        //                    trans.Commit();
        //                }
        //                catch (NHibernate.Exceptions.GenericADOException ex)
        //                {
        //                    trans.Rollback();
        //                    // MySession.Close();
        //                    throw new Exception(ex.Message);
        //                }
        //                catch (Exception e)
        //                {
        //                    trans.Rollback();
        //                    //MySession.Close();
        //                    throw new Exception(e.Message);
        //                }

        //                finally
        //                {
        //                    MySession.Close();
        //                }
        //            }
        //        }
        //        catch (Exception ex1)
        //        {
        //            //MySession.Close();
        //            throw new Exception(ex1.Message);
        //        }
        //    }
        //    #region RCopia
        //    //Added by velmurugan for Rcopia on 14.05.11
        //    RCopiaTransactionManager objrcoptranMngr = new RCopiaTransactionManager();
        //    if (ListToInsert != null && ListToInsert.Count > 0)
        //    {
        //        IList<ulong> ilstinsertId = new List<ulong>();
        //        IList<Assessment> ilstassessment = null;
        //        for (int x = 0; x < ListToInsert.Count; x++)
        //        {
        //            ilstinsertId.Add(ListToInsert[x].Id);
        //        }
        //        if (ilstinsertId != null && ilstinsertId.Count > 0)
        //        {
        //            objrcoptranMngr.SendProblemToRCopia(ilstinsertId, "Assesment", false, ilstassessment);
        //        }
        //    }
        //    if (ListToUpdate != null && ListToUpdate.Count > 0)
        //    {
        //        IList<ulong> ilstUpdateId = new List<ulong>();
        //        IList<Assessment> ilstassessment = null;
        //        for (int y = 0; y < ListToUpdate.Count; y++)
        //        {
        //            ilstUpdateId.Add(ListToUpdate[y].Id);
        //        }
        //        if (ilstUpdateId != null && ilstUpdateId.Count > 0)
        //        {
        //            objrcoptranMngr.SendProblemToRCopia(ilstUpdateId, "Assesment", false, ilstassessment);
        //        }
        //    }
        //    if (ListToDelete != null && ListToDelete.Count > 0)
        //    {

        //        IList<ulong> ilstDeleteId = new List<ulong>();
        //        //IList<Assessment> ilstassessment = null;
        //        for (int z = 0; z < ListToDelete.Count; z++)
        //        {
        //            ilstDeleteId.Add(ListToDelete[z].Id);
        //        }
        //        if (ilstDeleteId != null && ilstDeleteId.Count > 0)
        //        {
        //            objrcoptranMngr.SendProblemToRCopia(ilstDeleteId, "Assesment", true, ListToDelete);
        //        }
        //    }

        //    #endregion

        //    return GetAssessmentUsingEncounterIDForRuledOut(encounterId, phyId);
        //}

        int UpdateDBforAssessProbList(IList<Assessment> AssesLst, IList<ProblemList> ProbLst, IList<TreatmentPlan> TplanLst)
        {
            int iTryCount = 0;
        TryAgain:
            int iResult = 0;
            int status = 0;
            string sMacAddress = string.Empty;
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjHuman = new GenerateXml();
            bool objAssessmentConsistent = true, objProblemlistConsistent = true;
            using (ISession MySession = Session.GetISession())
            {
                try
                {
                    using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        try
                        {
                            #region AssessDelete
                            IList<Assessment> savelst = new List<Assessment>();
                            IList<Assessment> ListToUpdate = new List<Assessment>();
                            if (AssesLst.Count > 0)
                            {

                                ulong encounterid = AssesLst[0].Encounter_ID;

                                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savelst, ref ListToUpdate, AssesLst, MySession, sMacAddress, true, true, encounterid, string.Empty, ref XMLObj);
                                //iResult = SaveUpdateDeleteWithoutTransaction(ref savelst, null, AssesLst, MySession, sMacAddress);
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
                                objAssessmentConsistent = XMLObj.CheckDataConsistency(savelst.Concat(ListToUpdate).Cast<object>().ToList(), true, string.Empty);

                            }

                            #endregion
                            #region ProbLstDelete
                            IList<ProblemList> ProbSavelst = new List<ProblemList>();
                            ProblemListManager problemMngr = new ProblemListManager();
                            if (ProbLst.Count > 0)
                            {
                                ulong uHumanID = ProbLst[0].Human_ID;
                                iResult = problemMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ProbSavelst, ref ProbLst, null, MySession, sMacAddress, true, true, uHumanID, string.Empty, ref XMLObjHuman);
                                //iResult = problemMngr.BatchOperationsToProblem(ProbSavelst, ProbLst, null, MySession, sMacAddress, false);
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
                                objProblemlistConsistent = XMLObjHuman.CheckDataConsistency(ProbSavelst.Concat(ProbLst).Cast<object>().ToList(), true, string.Empty);
                            }

                            #endregion
                            #region TreatmentPlan
                            if (TplanLst != null && TplanLst.Count > 0)
                            {
                                ulong ulEncounterID = AssesLst[0].Encounter_ID;
                                ulong ulHumanID = AssesLst[0].Human_ID;
                                TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
                                IList<TreatmentPlan> SaveList = null;
                                IList<TreatmentPlan> UpdateList = null;
                                IList<TreatmentPlan> DeleteList = null;
                                DeleteList = TplanLst;
                                iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveList, ref UpdateList, DeleteList, MySession, sMacAddress, true, true, ulEncounterID, string.Empty, ref XMLObj);

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
                                        MySession.Close();
                                        throw new Exception("Deadlock occurred. Transaction failed.");
                                    }
                                }
                                else if (iResult == 1)
                                {
                                    trans.Rollback();
                                    MySession.Close();
                                    throw new Exception("Exception occurred. Transaction failed.");
                                }
                            }
                            #endregion
                            if (objAssessmentConsistent && objProblemlistConsistent)
                            {

                                if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                                {
                                    // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    }
                                    catch (Exception xmlexcep)
                                    {
                                        trycount++;
                                        if (trycount <= 3)
                                        {
                                            int TimeMilliseconds = 0;
                                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                            Thread.Sleep(TimeMilliseconds);
                                            string sMsg = string.Empty;
                                            string sExStackTrace = string.Empty;

                                            string version = "";
                                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                            string[] server = version.Split('|');
                                            string serverno = "";
                                            if (server.Length > 1)
                                                serverno = server[1].Trim();

                                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                                sMsg = xmlexcep.InnerException.Message;
                                            else
                                                sMsg = xmlexcep.Message;

                                            if (xmlexcep != null && xmlexcep.StackTrace != null)
                                                sExStackTrace = xmlexcep.StackTrace;

                                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                            string ConnectionData;
                                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                            {
                                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                                {
                                                    cmd.Connection = con;
                                                    try
                                                    {
                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                            }
                                            goto trytosaveagain;
                                        }
                                    }
                                }
                                if (XMLObjHuman.itemDoc.InnerXml != null && XMLObjHuman.itemDoc.InnerXml != "")
                                {
                                    // XMLObjHuman.itemDoc.Save(XMLObjHuman.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        //XMLObjHuman.itemDoc.Save(XMLObjHuman.strXmlFilePath);
                                    }
                                    catch (Exception xmlexcep)
                                    {
                                        trycount++;
                                        if (trycount <= 3)
                                        {
                                            int TimeMilliseconds = 0;
                                            if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                                TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                            Thread.Sleep(TimeMilliseconds);
                                            string sMsg = string.Empty;
                                            string sExStackTrace = string.Empty;

                                            string version = "";
                                            if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                                version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                            string[] server = version.Split('|');
                                            string serverno = "";
                                            if (server.Length > 1)
                                                serverno = server[1].Trim();

                                            if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                                sMsg = xmlexcep.InnerException.Message;
                                            else
                                                sMsg = xmlexcep.Message;

                                            if (xmlexcep != null && xmlexcep.StackTrace != null)
                                                sExStackTrace = xmlexcep.StackTrace;

                                            string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                            string ConnectionData;
                                            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                            using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                            {
                                                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                                {
                                                    cmd.Connection = con;
                                                    try
                                                    {
                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                            }
                                            goto trytosaveagain;
                                        }
                                    }
                                }
                                trans.Commit();
                                status = 1;
                            }
                            else
                                throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");

                        }
                        catch (NHibernate.Exceptions.GenericADOException ex)
                        {
                            trans.Rollback();
                            // MySession.Close();
                            //CAP-1942
                            throw new Exception(ex.Message, ex);
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
                    //CAP-1942
                    throw new Exception(ex1.Message, ex1);
                }
            }
            return status;
        }

        //Latha - 11 Aug 2011 - Single Service call - Start
        public FillAssessment LoadAssessment(ulong encounterID, ulong humanID, string Is_Ruled_Out, string UserName, string type)
        {
            IList<Assessment> assessList = new List<Assessment>();
            IList<ProblemList> probList = new List<ProblemList>();
            IList<Healthcare_Questionnaire> Questionnaire = new List<Healthcare_Questionnaire>();
            IList<string> QuestionnaireICDlist = new List<string>();
            FillAssessment objFillAssessment = new FillAssessment();
            int Status = 0;

            IList<Assessment> objAssessment = new List<Assessment>();
            IList<ProblemList> objProblemList = new List<ProblemList>();
            IList<PotentialDiagnosis> objPotentialDiagnosis = new List<PotentialDiagnosis>();
            IList<Healthcare_Questionnaire> objQuestionnaire = new List<Healthcare_Questionnaire>();
            IList<PatientResults> objPatientList = new List<PatientResults>();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            IList<GeneralNotes> objAssessmentGeneralNotes = new List<GeneralNotes>();
            IList<Rcopia_Medication> objMed = new List<Rcopia_Medication>();
            IList<ROS> objROS = new List<ROS>();
            IList<EandMCodingICD> objEandMICD = new List<EandMCodingICD>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                #region Data fetched from DB
                ICriteria criteriaAssessment = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", encounterID)); //.Add(Expression.Eq("Assessment_Type", "Selected"));
                objAssessment = criteriaAssessment.List<Assessment>();

                //ICriteria criteriaProblemList = iMySession.CreateCriteria(typeof(ProblemList)).Add(Expression.Eq("Human_ID", humanID));
                //probList = criteriaProblemList.List<ProblemList>();

                //objFillAssessment.Assessment = assessList;
                //if (probList != null && probList.Count != 0)
                //    probList = (from obj in probList where obj.ICD != "0000" select obj).ToList<ProblemList>();
                //objFillAssessment.Problem_List = probList;

                //ICriteria criteriaQuestionnaire = iMySession.CreateCriteria(typeof(Healthcare_Questionnaire)).Add(Expression.Eq("Human_ID", humanID)).Add(Expression.Eq("Encounter_ID", encounterID));
                //Questionnaire = criteriaQuestionnaire.List<Healthcare_Questionnaire>();


                //ProblemListManager probMgr = new ProblemListManager();
                ICriteria criteriaGeneralNotes = iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", encounterID)).Add(Expression.Eq("Parent_Field", "Selected Assessment"));
                objAssessmentGeneralNotes = criteriaGeneralNotes.List<GeneralNotes>();

                //IList<PatientResults> objPatientList = new List<PatientResults>();

                //ArrayList aryPatientResultList;
                //IQuery query1 = iMySession.GetNamedQuery("Get.PatientResultsList.Test");
                //query1.SetString(0, encounterID.ToString());
                //query1.SetString(1, humanID.ToString());
                //aryPatientResultList = new ArrayList(query1.List());
                //foreach (object[] oj in aryPatientResultList)
                //{
                //    PatientResults objPatientResults = new PatientResults();
                //    objPatientResults.Id = Convert.ToUInt32(oj[0]);
                //    objPatientResults.Loinc_Observation = oj[1].ToString();
                //    objPatientResults.Value = oj[2].ToString();
                //    objPatientResults.Units = oj[3].ToString();
                //    objPatientList.Add(objPatientResults);

                //}
                #endregion


                #region Fetch data from XML


                IList<string> ilsAssessmentTagList = new List<string>();
                ilsAssessmentTagList.Add("PatientResultsList");
                ilsAssessmentTagList.Add("ProblemListList");
                ilsAssessmentTagList.Add("PotentialDiagnosisList");
                

                IList<object> ilstAsshumanBlobFinal = new List<object>();

                ilstAsshumanBlobFinal = ReadBlob( humanID, ilsAssessmentTagList);

                if (ilstAsshumanBlobFinal != null && ilstAsshumanBlobFinal.Count > 0)
                {
                    if (ilstAsshumanBlobFinal[0] != null)
                    {
                        IList<PatientResults> lsttemppatientresults = new List<PatientResults>();

                        for (int iCount = 0; iCount < ((IList<object>)ilstAsshumanBlobFinal[0]).Count; iCount++)
                        {
                            lsttemppatientresults.Add((PatientResults)((IList<object>)ilstAsshumanBlobFinal[0])[iCount]);
                        }
                        objPatientList = (from m in lsttemppatientresults where m.Results_Type == "Vitals" select m).ToList<PatientResults>();
                    }
                    if (ilstAsshumanBlobFinal[1] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstAsshumanBlobFinal[1]).Count; iCount++)
                        {
                            //Cap - 1881
                            if(((ProblemList)((IList<object>)ilstAsshumanBlobFinal[1])[iCount]).Version_Year!= "ICD_9")
                            {
                                objProblemList.Add((ProblemList)((IList<object>)ilstAsshumanBlobFinal[1])[iCount]);
                            }                            
                        }
                    }
                    if (ilstAsshumanBlobFinal[2] != null)
                    {
                        IList<PotentialDiagnosis> lsttemp = new List<PotentialDiagnosis>();
                        for (int iCount = 0; iCount < ((IList<object>)ilstAsshumanBlobFinal[2]).Count; iCount++)
                        {
                            //Code comment by balaji.TJ
                            //objPotentialDiagnosis.Add((PotentialDiagnosis)((IList<object>)ilstAsshumanBlobFinal[2])[iCount]);
                            lsttemp.Add((PotentialDiagnosis)((IList<object>)ilstAsshumanBlobFinal[2])[iCount]);
                        }
                        objPotentialDiagnosis=(from m in lsttemp where m.Move_To_Assessment != "Y" select m).ToList<PotentialDiagnosis>();
                    }
                }
                //string HumanFileName = "Human" + "_" + humanID + ".xml";
                //string HumanXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);
                //if (File.Exists(HumanXmlFilePath) == true)
                //{


                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(HumanXmlFilePath);
                //    XmlNodeList xmlTagName = null;
                //    // itemDoc.Load(XmlText);
                //    using (FileStream fs = new FileStream(HumanXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);
                //        XmlText.Close();
                        #region Vitals
                        //if (itemDoc.GetElementsByTagName("PatientResultsList")[0] != null)
                        //{
                        //    xmlTagName = itemDoc.GetElementsByTagName("PatientResultsList")[0].ChildNodes;

                        //    if (xmlTagName.Count > 0)
                        //    {
                        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        {
                        //            if (Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Results_Type").Value) == "Vitals")
                        //            {

                        //                string TagName = xmlTagName[j].Name;
                        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(PatientResults));
                        //                PatientResults PatientResults = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PatientResults;
                        //                IEnumerable<PropertyInfo> propInfo = null;
                        //                propInfo = from obji in ((PatientResults)PatientResults).GetType().GetProperties() select obji;

                        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                {

                        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                    {
                        //                        foreach (PropertyInfo property in propInfo)
                        //                        {
                        //                            if (property.Name == nodevalue.Name)
                        //                            {
                        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                    property.SetValue(PatientResults, Convert.ToUInt64(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                    property.SetValue(PatientResults, Convert.ToString(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                    property.SetValue(PatientResults, Convert.ToDateTime(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                    property.SetValue(PatientResults, Convert.ToInt32(nodevalue.Value), null);
                        //                                else
                        //                                    property.SetValue(PatientResults, nodevalue.Value, null);
                        //                            }
                        //                        }
                        //                    }

                        //                }
                        //                objPatientList.Add(PatientResults);
                        //            }
                        //        }
                        //    }
                        //}
                              #endregion
                        //        //#region Medication
                        //        //if (itemDoc.GetElementsByTagName("Rcopia_MedicationList")[0] != null)
                        //        //{
                        //        //    xmlTagName = itemDoc.GetElementsByTagName("Rcopia_MedicationList")[0].ChildNodes;

                        //        //    if (xmlTagName.Count > 0)
                        //        //    {
                        //        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        //        {
                        //        //            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Human_ID").Value) == humanID)
                        //        //            {
                        //        //                string TagName = xmlTagName[j].Name;
                        //        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(Rcopia_Medication));
                        //        //                Rcopia_Medication Medication = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Rcopia_Medication;
                        //        //                IEnumerable<PropertyInfo> propInfo = null;
                        //        //                propInfo = from obji in ((Rcopia_Medication)Medication).GetType().GetProperties() select obji;

                        //        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //        //                {
                        //        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //        //                    {
                        //        //                        foreach (PropertyInfo property in propInfo)
                        //        //                        {
                        //        //                            if (property.Name == nodevalue.Name)
                        //        //                            {
                        //        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //        //                                    property.SetValue(Medication, Convert.ToUInt64(nodevalue.Value), null);
                        //        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //        //                                    property.SetValue(Medication, Convert.ToString(nodevalue.Value), null);
                        //        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //        //                                    property.SetValue(Medication, Convert.ToDateTime(nodevalue.Value), null);
                        //        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //        //                                    property.SetValue(Medication, Convert.ToInt32(nodevalue.Value), null);
                        //        //                                else
                        //        //                                    property.SetValue(Medication, nodevalue.Value, null);
                        //        //                            }
                        //        //                        }
                        //        //                    }

                        //        //                }
                        //        //                objMed.Add(Medication);
                        //        //            }
                        //        //        }
                        //        //    }
                        //        //}
                        //        //objMed = (from m in objMed where m.Deleted.ToUpper() != "Y" select m).ToList<Rcopia_Medication>();
                        //        //#endregion
                        //#region Problemlist
                        //if (itemDoc.GetElementsByTagName("ProblemListList")[0] != null)
                        //{
                        //    xmlTagName = itemDoc.GetElementsByTagName("ProblemListList")[0].ChildNodes;

                        //    if (xmlTagName.Count > 0)
                        //    {
                        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        {
                        //            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Human_ID").Value) == humanID)
                        //            {

                        //                string TagName = xmlTagName[j].Name;
                        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(ProblemList));
                        //                ProblemList ProblemList = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ProblemList;
                        //                IEnumerable<PropertyInfo> propInfo = null;
                        //                propInfo = from obji in ((ProblemList)ProblemList).GetType().GetProperties() select obji;

                        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                {

                        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                    {
                        //                        foreach (PropertyInfo property in propInfo)
                        //                        {
                        //                            if (property.Name == nodevalue.Name)
                        //                            {
                        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                    property.SetValue(ProblemList, Convert.ToUInt64(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                    property.SetValue(ProblemList, Convert.ToString(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                    property.SetValue(ProblemList, Convert.ToDateTime(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                    property.SetValue(ProblemList, Convert.ToInt32(nodevalue.Value), null);
                        //                                else
                        //                                    property.SetValue(ProblemList, nodevalue.Value, null);
                        //                            }
                        //                        }
                        //                    }

                        //                }
                        //                objProblemList.Add(ProblemList);
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                        //#region PotentialDiagnosis
                        //if (itemDoc.GetElementsByTagName("PotentialDiagnosisList")[0] != null)
                        //{
                        //    xmlTagName = itemDoc.GetElementsByTagName("PotentialDiagnosisList")[0].ChildNodes;

                        //    if (xmlTagName.Count > 0)
                        //    {
                        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        {
                        //            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Human_ID").Value) == humanID)
                        //            {

                        //                string TagName = xmlTagName[j].Name;
                        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(PotentialDiagnosis));
                        //                PotentialDiagnosis PotentialDiagnosis = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PotentialDiagnosis;
                        //                IEnumerable<PropertyInfo> propInfo = null;
                        //                propInfo = from obji in ((PotentialDiagnosis)PotentialDiagnosis).GetType().GetProperties() select obji;

                        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                {

                        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                    {
                        //                        foreach (PropertyInfo property in propInfo)
                        //                        {
                        //                            if (property.Name == nodevalue.Name)
                        //                            {
                        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                    property.SetValue(PotentialDiagnosis, Convert.ToUInt64(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                    property.SetValue(PotentialDiagnosis, Convert.ToString(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                    property.SetValue(PotentialDiagnosis, Convert.ToDateTime(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                    property.SetValue(PotentialDiagnosis, Convert.ToInt32(nodevalue.Value), null);
                        //                                else
                        //                                    property.SetValue(PotentialDiagnosis, nodevalue.Value, null);
                        //                            }
                        //                        }
                        //                    }

                        //                }
                        //                if (PotentialDiagnosis.Move_To_Assessment != "Y")
                        //                    objPotentialDiagnosis.Add(PotentialDiagnosis);
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}

                string FileName = "Encounter" + "_" + encounterID + ".xml";


                IList<string> ilsAssessmentEncounterTagList = new List<string>();
                ilsAssessmentEncounterTagList.Add("GeneralNotesSelectedAssessmentList");
                ilsAssessmentEncounterTagList.Add("TreatmentPlanList");
                ilsAssessmentEncounterTagList.Add("EandMCodingICDList");
             

                    IList<object> ilstAsEncounterBlobFinal = new List<object>();

                ilstAsEncounterBlobFinal = ReadBlob( encounterID, ilsAssessmentEncounterTagList);

                if (ilstAsEncounterBlobFinal != null && ilstAsEncounterBlobFinal.Count > 0)
                {
                    if (ilstAsEncounterBlobFinal[0] != null)
                    {
                       

                        for (int iCount = 0; iCount < ((IList<object>)ilstAsEncounterBlobFinal[0]).Count; iCount++)
                        {
                            objAssessmentGeneralNotes.Add((GeneralNotes)((IList<object>)ilstAsEncounterBlobFinal[0])[iCount]);
                        }
                       // objPatientList = (from m in lsttemppatientresults where m.Results_Type == "Vitals" select m).ToList<PatientResults>();
                    }
                    if (ilstAsEncounterBlobFinal[1] != null)
                    {


                        for (int iCount = 0; iCount < ((IList<object>)ilstAsEncounterBlobFinal[1]).Count; iCount++)
                        {
                            objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstAsEncounterBlobFinal[1])[iCount]);
                        }
                        // objPatientList = (from m in lsttemppatientresults where m.Results_Type == "Vitals" select m).ToList<PatientResults>();
                    }

                    
                                 
                    if (ilstAsEncounterBlobFinal[2] != null)
                    {


                        for (int iCount = 0; iCount < ((IList<object>)ilstAsEncounterBlobFinal[2]).Count; iCount++)
                        {
                            objEandMICD.Add((EandMCodingICD)((IList<object>)ilstAsEncounterBlobFinal[2])[iCount]);
                        }
                        // objPatientList = (from m in lsttemppatientresults where m.Results_Type == "Vitals" select m).ToList<PatientResults>();
                    }

                }
                //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //    XmlNodeList xmlTagName = null;
                //    //itemDoc.Load(XmlText);
                //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlText.Close();
                        //#region ROS
                        //if (itemDoc.GetElementsByTagName("ROSList")[0] != null)
                        //{
                        //    xmlTagName = itemDoc.GetElementsByTagName("ROSList")[0].ChildNodes;

                        //    if (xmlTagName.Count > 0)
                        //    {
                        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        {
                        //            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == encounterID)
                        //            {

                        //                string TagName = xmlTagName[j].Name;
                        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(ROS));
                        //                ROS ROS = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ROS;
                        //                IEnumerable<PropertyInfo> propInfo = null;
                        //                propInfo = from obji in ((ROS)ROS).GetType().GetProperties() select obji;

                        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                {

                        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                    {
                        //                        foreach (PropertyInfo property in propInfo)
                        //                        {
                        //                            if (property.Name == nodevalue.Name)
                        //                            {
                        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                    property.SetValue(ROS, Convert.ToUInt64(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                    property.SetValue(ROS, Convert.ToString(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                    property.SetValue(ROS, Convert.ToDateTime(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                    property.SetValue(ROS, Convert.ToInt32(nodevalue.Value), null);
                        //                                else
                        //                                    property.SetValue(ROS, nodevalue.Value, null);
                        //                            }
                        //                        }
                        //                    }

                        //                }
                        //                objROS.Add(ROS);
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                        //#region Assessment
                        //if (itemDoc.GetElementsByTagName("AssessmentList")[0] != null)
                        //{
                        //    xmlTagName = itemDoc.GetElementsByTagName("AssessmentList")[0].ChildNodes;

                        //    if (xmlTagName.Count > 0)
                        //    {
                        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        {
                        //            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value) == encounterID)
                        //            {

                        //                string TagName = xmlTagName[j].Name;
                        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(Assessment));
                        //                Assessment Assessment = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Assessment;
                        //                IEnumerable<PropertyInfo> propInfo = null;
                        //                propInfo = from obji in ((Assessment)Assessment).GetType().GetProperties() select obji;

                        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                {

                        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                    {
                        //                        foreach (PropertyInfo property in propInfo)
                        //                        {
                        //                            if (property.Name == nodevalue.Name)
                        //                            {
                        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                    property.SetValue(Assessment, Convert.ToUInt64(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                    property.SetValue(Assessment, Convert.ToString(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                    property.SetValue(Assessment, Convert.ToDateTime(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                    property.SetValue(Assessment, Convert.ToInt32(nodevalue.Value), null);
                        //                                else
                        //                                    property.SetValue(Assessment, nodevalue.Value, null);
                        //                            }
                        //                        }
                        //                    }

                        //                }
                        //                objAssessment.Add(Assessment);
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                        //#region Assessment General Notes
                        //if (itemDoc.GetElementsByTagName("GeneralNotesSelectedAssessmentList")[0] != null)
                        //{
                        //    xmlTagName = itemDoc.GetElementsByTagName("GeneralNotesSelectedAssessmentList")[0].ChildNodes;

                        //    if (xmlTagName.Count > 0)
                        //    {
                        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        {
                        //            if ((Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value) == encounterID) && (xmlTagName[j].Attributes.GetNamedItem("Parent_Field").Value == "Selected Assessment"))
                        //            {

                        //                string TagName = xmlTagName[j].Name;
                        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                        //                GeneralNotes GeneralNotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as GeneralNotes;
                        //                IEnumerable<PropertyInfo> propInfo = null;
                        //                propInfo = from obji in ((GeneralNotes)GeneralNotes).GetType().GetProperties() select obji;

                        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                {

                        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                    {
                        //                        foreach (PropertyInfo property in propInfo)
                        //                        {
                        //                            if (property.Name == nodevalue.Name)
                        //                            {
                        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                    property.SetValue(GeneralNotes, Convert.ToUInt64(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                    property.SetValue(GeneralNotes, Convert.ToString(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                    property.SetValue(GeneralNotes, Convert.ToDateTime(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                    property.SetValue(GeneralNotes, Convert.ToInt32(nodevalue.Value), null);
                        //                                else
                        //                                    property.SetValue(GeneralNotes, nodevalue.Value, null);
                        //                            }
                        //                        }
                        //                    }

                        //                }
                        //                objAssessmentGeneralNotes.Add(GeneralNotes);
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                        //#region Questionnaire
                        //if (itemDoc.GetElementsByTagName("Healthcare_Questionnaire")[0] != null)
                        //{
                        //    xmlTagName = itemDoc.GetElementsByTagName("Healthcare_Questionnaire");

                        //    if (xmlTagName.Count > 0)
                        //    {
                        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        {
                        //            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value) == encounterID)
                        //            {

                        //                string TagName = xmlTagName[j].Name;
                        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(Healthcare_Questionnaire));
                        //                Healthcare_Questionnaire Healthcare_Questionnaire = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Healthcare_Questionnaire;
                        //                IEnumerable<PropertyInfo> propInfo = null;
                        //                propInfo = from obji in ((Healthcare_Questionnaire)Healthcare_Questionnaire).GetType().GetProperties() select obji;

                        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                {

                        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                    {
                        //                        foreach (PropertyInfo property in propInfo)
                        //                        {
                        //                            if (property.Name == nodevalue.Name)
                        //                            {
                        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                    property.SetValue(Healthcare_Questionnaire, Convert.ToUInt64(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                    property.SetValue(Healthcare_Questionnaire, Convert.ToString(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                    property.SetValue(Healthcare_Questionnaire, Convert.ToDateTime(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                    property.SetValue(Healthcare_Questionnaire, Convert.ToInt32(nodevalue.Value), null);
                        //                                else
                        //                                    property.SetValue(Healthcare_Questionnaire, nodevalue.Value, null);
                        //                            }
                        //                        }
                        //                    }

                        //                }
                        //                objQuestionnaire.Add(Healthcare_Questionnaire);
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                        //#region Treatment_plan
                        //if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
                        //{
                        //    xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

                        //    if (xmlTagName.Count > 0)
                        //    {
                        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        {
                        //            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == encounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("ASSESSMENT"))
                        //            {

                        //                string TagName = xmlTagName[j].Name;
                        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
                        //                TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
                        //                IEnumerable<PropertyInfo> propInfo = null;
                        //                propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

                        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                {

                        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                    {
                        //                        foreach (PropertyInfo property in propInfo)
                        //                        {
                        //                            if (property.Name == nodevalue.Name)
                        //                            {
                        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                    property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                    property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                    property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                    property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
                        //                                else
                        //                                    property.SetValue(TreatmentPlan, nodevalue.Value, null);
                        //                            }
                        //                        }
                        //                    }

                        //                }
                        //                objTreatmentPlan.Add(TreatmentPlan);
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion

                        //#region EandMICD
                        //if (itemDoc.GetElementsByTagName("EandMCodingICDList")[0] != null)
                        //{
                        //    xmlTagName = itemDoc.GetElementsByTagName("EandMCodingICDList")[0].ChildNodes;

                        //    if (xmlTagName.Count > 0)
                        //    {
                        //        for (int j = 0; j < xmlTagName.Count; j++)
                        //        {
                        //            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value) == encounterID && (xmlTagName[j].Attributes.GetNamedItem("Source").Value.ToString().ToUpper() == "ASSESSMENT") && (xmlTagName[j].Attributes.GetNamedItem("Is_Delete").Value.ToString().ToUpper() == "N"))
                        //            {
                        //                string TagName = xmlTagName[j].Name;
                        //                XmlSerializer xmlserializer = new XmlSerializer(typeof(EandMCodingICD));
                        //                EandMCodingICD EandMicd = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EandMCodingICD;
                        //                IEnumerable<PropertyInfo> propInfo = null;
                        //                propInfo = from obji in ((EandMCodingICD)EandMicd).GetType().GetProperties() select obji;

                        //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                        //                {

                        //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                        //                    {
                        //                        foreach (PropertyInfo property in propInfo)
                        //                        {
                        //                            if (property.Name == nodevalue.Name)
                        //                            {
                        //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                        //                                    property.SetValue(EandMicd, Convert.ToUInt64(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                        //                                    property.SetValue(EandMicd, Convert.ToString(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        //                                    property.SetValue(EandMicd, Convert.ToDateTime(nodevalue.Value), null);
                        //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                        //                                    property.SetValue(EandMicd, Convert.ToInt32(nodevalue.Value), null);
                        //                                else
                        //                                    property.SetValue(EandMicd, nodevalue.Value, null);
                        //                            }
                        //                        }
                        //                    }

                        //                }
                                        
                        //                objEandMICD.Add(EandMicd);
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                       // fs.Close();
                      //  fs.Dispose();
                  //  }
              //  }
                #endregion
                objFillAssessment.Assessment = objAssessment;
                objFillAssessment.EandMICD = objEandMICD;

                objFillAssessment.Potential_Diagnosis = objPotentialDiagnosis;
                objFillAssessment.General_NotesCurrentList = objAssessmentGeneralNotes;//BugID:49067
                //Commented for BugID:53007 
                //if (objFillAssessment.Assessment != null && objFillAssessment.Assessment.Count > 0)//BugID:48668 -- to prevent suggestions when Assessment.count>0
                //    goto d;
                //objProblemList = (from obj in objProblemList where obj.ICD != "0000" select obj).ToList<ProblemList>();
                //objFillAssessment.Problem_List = objProblemList;



                IList<AssessmentVitalsLookup> AssessmentVitalsLookupList = new List<AssessmentVitalsLookup>();
                //AssessmentVitalsLookupManager objAssessVitalsMngr = new AssessmentVitalsLookupManager();

                IList<Assessment> AssesToDelete = new List<Assessment>();
                IList<ProblemList> ProbLstToUpdate = new List<ProblemList>();
                IList<ProblemList> ProbLstToUpdateNew = new List<ProblemList>();

                //BugID:54773 -- Load AssessmentMappingLookup  from ConfigXML 
                string AssessmentMappingXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\AssessmentVitalsLookup.xml");
                if (File.Exists(AssessmentMappingXmlFilePath) == true)
                {
                    using (FileStream fs = new FileStream(AssessmentMappingXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        XmlDocument itemDoc = new XmlDocument();
                        XmlTextReader xmltxtReader = new XmlTextReader(fs);
                        itemDoc.Load(xmltxtReader);
                        xmltxtReader.Close();
                        XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("Assessment_vitals_lookupList");
                        if (xmlNodeList != null && xmlNodeList.Count > 0 && xmlNodeList[0].ChildNodes != null && xmlNodeList[0].ChildNodes.Count > 0)
                        {
                            for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                            {
                                AssessmentVitalsLookup objAssVitalsLookup = new AssessmentVitalsLookup();
                                objAssVitalsLookup.Description = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Description").Value;
                                objAssVitalsLookup.Entity_Name = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Entity_Name").Value;
                                objAssVitalsLookup.Field_Name = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value;
                                objAssVitalsLookup.Hirarrchy = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Hirarrchy").Value;
                                objAssVitalsLookup.ICD = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("ICD").Value;
                                objAssVitalsLookup.ICD_10 = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("ICD_10").Value;
                                objAssVitalsLookup.ICD_10_Description = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("ICD_10_Description").Value;
                                objAssVitalsLookup.Is_Current_Encounter = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Is_Current_Encounter").Value;
                                objAssVitalsLookup.Is_Macra_Field = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Is_Macra_Field").Value;
                                objAssVitalsLookup.Is_Mutually_Exclusive = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Is_Mutually_Exclusive").Value;
                                objAssVitalsLookup.Sort_Order = Convert.ToInt32(xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Sort_Order").Value);
                                objAssVitalsLookup.Value = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Value").Value;
                                objAssVitalsLookup.Id = Convert.ToUInt32(xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Id").Value);
                                AssessmentVitalsLookupList.Add(objAssVitalsLookup);
                            }
                        }
                        fs.Close();
                        fs.Dispose();
                    }
                }


                objProblemList = (from obj in objProblemList where obj.ICD != "0000" select obj).ToList<ProblemList>();

                IList<ProblemList> ilstVitalsBasedProblem = new List<ProblemList>();
                for (int iCount = 0; iCount < objProblemList.Count; iCount++)
                {
                    var AssessmentVitalsicds = (from obj in AssessmentVitalsLookupList where obj.ICD_10 == objProblemList[iCount].ICD select obj).ToList<AssessmentVitalsLookup>();
                    if (AssessmentVitalsicds.Count > 0)
                    {
                        ilstVitalsBasedProblem.Add(objProblemList[iCount]);
                    }
                }

                //CAP-2128
                //for (int iCount = 0; iCount < ilstVitalsBasedProblem.Count; iCount++)
                //{
                //    objProblemList.Remove(ilstVitalsBasedProblem[iCount]);
                //}

                objFillAssessment.Problem_List = objProblemList;


                if (AssessmentVitalsLookupList.Count > 0)
                {
                    objFillAssessment.VitalsBasedICD_List = GetICDListForAssessment(AssessmentVitalsLookupList, objROS, objMed, objPatientList, objQuestionnaire, encounterID);
                }

                //AssessmentVitalsLookupList = (from AssVitalLookup in AssessmentVitalsLookupList where !AssVitalLookup.Field_Name.Equals("eGFR") select AssVitalLookup).ToList();
                //objFillAssessment.VitalsBasedICD_List = objAssessVitalsMngr.GetIcdBasedOnVitals(AssessmentVitalsLookupList, objPatientList);//objPatientList : vitals data for current encounter

                //if (Questionnaire != null && Questionnaire.Count > 0)
                //{
                //    IList<AssessmentVitalsLookup> QuestionnaireAssessICD = AssessmentVitalsLookupList.Where(a => a.Entity_Name.Split('$')[0] == "Healthcare_Questionnaire").ToList<AssessmentVitalsLookup>();
                //    QuestionnaireICDlist = (from obj in QuestionnaireAssessICD where Questionnaire.Any(a => a.Questionnaire_Type.ToUpper() == obj.Entity_Name.Split('$')[1].ToUpper() && a.Question.ToUpper() == obj.Field_Name.ToUpper() && a.Selected_Option != string.Empty && (Convert.ToInt32(a.Selected_Option) >= Convert.ToInt32(obj.Value.Split('-')[0]) && Convert.ToInt32(a.Selected_Option) <= Convert.ToInt32(obj.Value.Split('-')[1]))) select obj.ICD_10).ToList<string>();
                //}
                //foreach (string s in QuestionnaireICDlist)
                //    objFillAssessment.VitalsBasedICD_List.Add(s);

                IList<string> AssesVitalICD = (from AssVitICD in AssessmentVitalsLookupList where AssVitICD.Is_Mutually_Exclusive == "Y" select AssVitICD.ICD_10).ToArray();//contains only 'BMI' , 'BMI STATUS' and 'BMI PERCENTILE' icds.
                AssesVitalICD = AssesVitalICD.Except(objFillAssessment.VitalsBasedICD_List).ToList();

                if (objFillAssessment.VitalsBasedICD_List.Count > 0 && type == "load")
                {
                    AssesToDelete = (from objAsses in objFillAssessment.Assessment where AssesVitalICD.Any(a => a == objAsses.ICD) select objAsses).ToList();
                    ProbLstToUpdate = (from objProb in objFillAssessment.Problem_List where AssesVitalICD.Any(b => b == objProb.ICD) select objProb).ToList();

                    objFillAssessment.Assessment = (from objAsses in objFillAssessment.Assessment where !AssesVitalICD.Any(a => a == objAsses.ICD) select objAsses).ToList();
                    objFillAssessment.Problem_List = (from objProb in objFillAssessment.Problem_List where !AssesVitalICD.Any(b => b == objProb.ICD) select objProb).ToList();
                }

                foreach (ProblemList prob in ProbLstToUpdate)
                {
                    string[] ref_src_lst = prob.Reference_Source.Split('|');
                    if (ref_src_lst[ref_src_lst.Length - 1].IndexOf("Deleted") == -1)
                    {
                        prob.Reference_Source = prob.Reference_Source + "|Deleted";
                        prob.Is_Active = "N";
                    }

                    else
                    {
                        ProbLstToUpdateNew.Add(prob);
                    }

                }
                foreach (ProblemList pb in ProbLstToUpdateNew)
                {
                    ProbLstToUpdate.Remove(pb);
                }
                IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();
                if (AssesToDelete.Count > 0)
                {
                    Delete_Tplan = (from oj in objTreatmentPlan where AssesToDelete.Any(a => a.Id == oj.Source_ID) select oj).ToList<TreatmentPlan>();
                }
                if (AssesToDelete.Count > 0 || ProbLstToUpdate.Count > 0)
                    Status = UpdateDBforAssessProbList(AssesToDelete, ProbLstToUpdate, Delete_Tplan);
                if (Status == 1)
                {
                    foreach (ProblemList obj in ProbLstToUpdate)
                    {
                        // obj.Version = obj.Version + 1;

                        objFillAssessment.Problem_List.Add(obj);
                    }
                }
                iMySession.Close();
                #region oldCode
                //    if (type == "load")
                //    {
                //        IList<AssessmentVitalsLookup> AssessmentVitalsLookupList = new List<AssessmentVitalsLookup>();
                //        IList<AssessmentVitalsLookup> AssessmentVitalLookuplst = new List<AssessmentVitalsLookup>();

                //        ICriteria crit2 = iMySession.CreateCriteria(typeof(AssessmentVitalsLookup));
                //        AssessmentVitalsLookupList = crit2.List<AssessmentVitalsLookup>();
                //        int count = 0;

                //        AssessmentVitalsLookupManager objAssessVitalsMngr = new AssessmentVitalsLookupManager();
                //        objFillAssessment.VitalsBasedICD_List = objAssessVitalsMngr.GetIcdBasedOnVitals(AssessmentVitalsLookupList, objPatientList);//objPatientList : vitals data for current encounter
                //        string[] VitalsICD = new string[AssessmentVitalsLookupList.Count];
                //        int k = 0, l = 0;
                //        string[] assessIcd = new string[objFillAssessment.VitalsBasedICD_List.Count];

                //        string[] prblVitalICD = new string[objFillAssessment.Problem_List.Count];

                //        for (int i = 0; i < AssessmentVitalsLookupList.Count; i++)
                //        {
                //            if (AssessmentVitalsLookupList[i].ICD_10.Trim() != "")
                //                VitalsICD[i] = AssessmentVitalsLookupList[i].ICD_10;
                //        }

                //        if (objFillAssessment.VitalsBasedICD_List.Count > 0)
                //        {
                //            for (int i = 0; i < objFillAssessment.Assessment.Count; i++)
                //            {
                //                if (objFillAssessment.VitalsBasedICD_List.IndexOf(objFillAssessment.Assessment[i].ICD) > -1)
                //                {
                //                    assessIcd[l] = objFillAssessment.Assessment[i].ICD;
                //                    l++;
                //                }
                //            }
                //            for (int i = 0; i < objFillAssessment.Problem_List.Count; i++)
                //            {
                //                if (VitalsICD.Contains(objFillAssessment.Problem_List[i].ICD) && objFillAssessment.VitalsBasedICD_List.IndexOf(objFillAssessment.Problem_List[i].ICD) == -1)
                //                {
                //                    prblVitalICD[k] = objFillAssessment.Problem_List[i].ICD;
                //                    k++;

                //                }
                //            }
                //            if (k != 0)
                //            {
                //                for (int i = 0; i < k; i++)
                //                {
                //                    objFillAssessment.Problem_List.Remove(objFillAssessment.Problem_List.First(a => a.ICD == prblVitalICD[i]));
                //                }
                //            }

                //            if (AssessmentVitalsLookupList.Count > 0 && count == 0)
                //            {
                //                AssessmentVitalLookuplst = (from val in AssessmentVitalsLookupList
                //                                            where (val.Field_Name == "BMI" || val.Field_Name == "BMI PERCENTILE" || val.Field_Name == "BMI STATUS")
                //                                            select val).Distinct().ToList();
                //                if (AssessmentVitalLookuplst.Count > 0)
                //                {
                //                    IList<Assessment> assesslist = new List<Assessment>();
                //                    assesslist = (from d in objFillAssessment.Assessment where assessIcd.Any(a => a == d.ICD) select d).ToList();

                //                    objFillAssessment.Assessment = (from a in objFillAssessment.Assessment.Except(assesslist) where !AssessmentVitalLookuplst.Any(b => b.ICD_10 == a.ICD) select a).ToList();
                //                    objFillAssessment.Problem_List = (from a in objFillAssessment.Problem_List where !AssessmentVitalLookuplst.Any(b => b.ICD_10 == a.ICD) select a).ToList();
                //                    foreach (Assessment obj in assesslist)
                //                        objFillAssessment.Assessment.Add(obj);
                //                }

                //            }
                //        }
                //    }

                //    iMySession.Close();
                #endregion
            }
            // d: 
            return objFillAssessment;
        }

        public FillAssessment LoadProblemList(ulong encounterID, ulong humanID,  string type)
        {
            FillAssessment objFillAssessment = new FillAssessment();           

            IList<Assessment> objAssessment = new List<Assessment>();
            IList<ProblemList> objProblemList = new List<ProblemList>();
            
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                #region Data fetched from DB
                ICriteria criteriaAssessment = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", encounterID)); //.Add(Expression.Eq("Assessment_Type", "Selected"));
                objAssessment = criteriaAssessment.List<Assessment>();

                #endregion


                #region Fetch data from XML


                IList<string> ilsAssessmentTagList = new List<string>();
                ilsAssessmentTagList.Add("ProblemListList");
                

                IList<object> ilstAsshumanBlobFinal = new List<object>();

                ilstAsshumanBlobFinal = ReadBlob(humanID, ilsAssessmentTagList);

                if (ilstAsshumanBlobFinal != null && ilstAsshumanBlobFinal.Count > 0)
                {
                 
                    if (ilstAsshumanBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstAsshumanBlobFinal[0]).Count; iCount++)
                        {
                            //Cap - 1881
                            if (((ProblemList)((IList<object>)ilstAsshumanBlobFinal[0])[iCount]).Version_Year!= "ICD_9")
                            {
                                objProblemList.Add((ProblemList)((IList<object>)ilstAsshumanBlobFinal[0])[iCount]);
                            }                            
                        }
                    }                   
                }                

                
                #endregion
                objFillAssessment.Assessment = objAssessment;
                
                IList<AssessmentVitalsLookup> AssessmentVitalsLookupList = new List<AssessmentVitalsLookup>();
                IList<Assessment> AssesToDelete = new List<Assessment>();
                IList<ProblemList> ProbLstToUpdate = new List<ProblemList>();
                IList<ProblemList> ProbLstToUpdateNew = new List<ProblemList>();

                string AssessmentMappingXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\AssessmentVitalsLookup.xml");
                if (File.Exists(AssessmentMappingXmlFilePath) == true)
                {
                    using (FileStream fs = new FileStream(AssessmentMappingXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        XmlDocument itemDoc = new XmlDocument();
                        XmlTextReader xmltxtReader = new XmlTextReader(fs);
                        itemDoc.Load(xmltxtReader);
                        xmltxtReader.Close();
                        XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("Assessment_vitals_lookupList");
                        if (xmlNodeList != null && xmlNodeList.Count > 0 && xmlNodeList[0].ChildNodes != null && xmlNodeList[0].ChildNodes.Count > 0)
                        {
                            for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                            {
                                AssessmentVitalsLookup objAssVitalsLookup = new AssessmentVitalsLookup();
                                objAssVitalsLookup.Description = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Description").Value;
                                objAssVitalsLookup.Entity_Name = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Entity_Name").Value;
                                objAssVitalsLookup.Field_Name = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value;
                                objAssVitalsLookup.Hirarrchy = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Hirarrchy").Value;
                                objAssVitalsLookup.ICD = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("ICD").Value;
                                objAssVitalsLookup.ICD_10 = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("ICD_10").Value;
                                objAssVitalsLookup.ICD_10_Description = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("ICD_10_Description").Value;
                                objAssVitalsLookup.Is_Current_Encounter = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Is_Current_Encounter").Value;
                                objAssVitalsLookup.Is_Macra_Field = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Is_Macra_Field").Value;
                                objAssVitalsLookup.Is_Mutually_Exclusive = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Is_Mutually_Exclusive").Value;
                                objAssVitalsLookup.Sort_Order = Convert.ToInt32(xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Sort_Order").Value);
                                objAssVitalsLookup.Value = xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Value").Value;
                                objAssVitalsLookup.Id = Convert.ToUInt32(xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Id").Value);
                                AssessmentVitalsLookupList.Add(objAssVitalsLookup);
                            }
                        }
                        fs.Close();
                        fs.Dispose();
                    }
                }


                objProblemList = (from obj in objProblemList where obj.ICD != "0000" select obj).ToList<ProblemList>();

                IList<ProblemList> ilstVitalsBasedProblem = new List<ProblemList>();
                for (int iCount = 0; iCount < objProblemList.Count; iCount++)
                {
                    var AssessmentVitalsicds = (from obj in AssessmentVitalsLookupList where obj.ICD_10 == objProblemList[iCount].ICD select obj).ToList<AssessmentVitalsLookup>();
                    if (AssessmentVitalsicds.Count > 0)
                    {
                        ilstVitalsBasedProblem.Add(objProblemList[iCount]);
                    }
                }

                //CAP-2128
                //for (int iCount = 0; iCount < ilstVitalsBasedProblem.Count; iCount++)
                //{
                //    objProblemList.Remove(ilstVitalsBasedProblem[iCount]);
                //}

                objFillAssessment.Problem_List = objProblemList;


               
                IList<string> AssesVitalICD = (from AssVitICD in AssessmentVitalsLookupList where AssVitICD.Is_Mutually_Exclusive == "Y" select AssVitICD.ICD_10).ToArray();//contains only 'BMI' , 'BMI STATUS' and 'BMI PERCENTILE' icds.
                AssesVitalICD = AssesVitalICD.Except(objFillAssessment.VitalsBasedICD_List).ToList();

                if (objFillAssessment.VitalsBasedICD_List.Count > 0 && type == "load")
                {
                    AssesToDelete = (from objAsses in objFillAssessment.Assessment where AssesVitalICD.Any(a => a == objAsses.ICD) select objAsses).ToList();
                    ProbLstToUpdate = (from objProb in objFillAssessment.Problem_List where AssesVitalICD.Any(b => b == objProb.ICD) select objProb).ToList();

                    objFillAssessment.Assessment = (from objAsses in objFillAssessment.Assessment where !AssesVitalICD.Any(a => a == objAsses.ICD) select objAsses).ToList();
                    objFillAssessment.Problem_List = (from objProb in objFillAssessment.Problem_List where !AssesVitalICD.Any(b => b == objProb.ICD) select objProb).ToList();
                }

                foreach (ProblemList prob in ProbLstToUpdate)
                {
                    string[] ref_src_lst = prob.Reference_Source.Split('|');
                    if (ref_src_lst[ref_src_lst.Length - 1].IndexOf("Deleted") == -1)
                    {
                        prob.Reference_Source = prob.Reference_Source + "|Deleted";
                        prob.Is_Active = "N";
                    }

                    else
                    {
                        ProbLstToUpdateNew.Add(prob);
                    }

                }
                foreach (ProblemList pb in ProbLstToUpdateNew)
                {
                    ProbLstToUpdate.Remove(pb);
                }
               
                    foreach (ProblemList obj in ProbLstToUpdate)
                    {
                        objFillAssessment.Problem_List.Add(obj);
                    }
                
                iMySession.Close();
                
            }
            
            return objFillAssessment;
        }

        //Latha - 11 Aug 2011 - Single Service call - End

        //public FillAssessment LoadAssessment(ulong encounterID, ulong humanID)
        //{
        //    IList<FillAssessment> objFillAssessment = new List<FillAssessment>();
        //    IList<Assessment> assessList = new List<Assessment>();
        //    IList<AssessmentSource> assessSourceList = new List<AssessmentSource>();
        //    IList<ProblemList> probList = new List<ProblemList>();
        //    string referenceSource = "Assessment";

        //    ICriteria criteriaAssessment = session.GetISession().CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", encounterID)).Add(Expression.Eq("Assessment_Type", "Selected"));
        //    assessList = criteriaAssessment.List<Assessment>();

        //    ICriteria criteriaAssessmentSource = session.GetISession().CreateCriteria(typeof(AssessmentSource)).Add(Expression.Eq("Encounter_Id", encounterID));
        //    assessSourceList = criteriaAssessmentSource.List<AssessmentSource>();

        //    ICriteria criteriaProblemList = session.GetISession().CreateCriteria(typeof(ProblemList)).Add(Expression.Eq("Human_ID", humanID)).Add(Expression.Eq("Reference_Source", referenceSource));
        //    probList = criteriaProblemList.List<ProblemList>();

        //    FillAssessment a = new FillAssessment();
        //    a.Assessment = assessList;
        //    a.Assessment_Source = assessSourceList;
        //    a.Problem_List = probList;

        //    ProblemListManager probMgr = new ProblemListManager();
        //    a.Problem_List_Human = probMgr.GetProblemDescriptionByHumanId(humanID);

        //    AssessmentProblemListDifferenceManager assessmentProblemListMgr = new AssessmentProblemListDifferenceManager();
        //    a.Assessment_Problem_List_Difference =  assessmentProblemListMgr.GetAssessmentProblemListDifferenceByEncounterId(encounterID);


        //    return a;

        //}

        //public IList<QuestionsDTO> GetQuestionMasterRecord(string parentID, ref string mutuallyExclusive, ref string sourceTable)
        //{
        //    ArrayList ResultList = null;
        //    IList<QuestionsDTO> QuestionsList = new List<QuestionsDTO>();
        //    if (sourceTable == "ALLICD")
        //    {
        //        IQuery query3 = session.GetISession().GetNamedQuery("Get.Questions.FromAiiICDTable.ByPassingParentID");
        //        query3.SetString(0, parentID.ToString());
        //        ResultList = new ArrayList(query3.List());

        //        //Added Latha - 2/8/10 - Checking for null value.
        //        if (ResultList != null)
        //        {
        //            for (int i = 0; i < ResultList.Count; i++)
        //            {
        //                object[] oj = (object[])ResultList[i];
        //                QuestionsDTO q = new QuestionsDTO();
        //                q.ICD_9 = oj[0].ToString();
        //                q.Parent_ID = oj[1].ToString();
        //                q.Diagnosis_Description = oj[2].ToString();
        //                q.Leaf_Node = oj[3].ToString();
        //                q.Mutually_Exclusive = oj[4].ToString();
        //                q.HCC_Category = Convert.ToUInt64(oj[5].ToString());
        //                QuestionsList.Add(q);
        //                mutuallyExclusive = q.Mutually_Exclusive;
        //            }
        //        }
        //    }
        //    else if (sourceTable == "ASSOCIATEDPRIMARYICD")
        //    {
        //        IQuery query3 = session.GetISession().GetNamedQuery("Get.Questions.FromAssociatedPrimaryICD.ByPassingParentID");
        //        query3.SetString(0, parentID.ToString());
        //        ResultList = new ArrayList(query3.List());

        //        //Added Latha - 2/8/10 - Checking for null value.
        //        if (ResultList != null)
        //        {
        //            for (int i = 0; i < ResultList.Count; i++)
        //            {
        //                object[] oj = (object[])ResultList[i];
        //                QuestionsDTO q = new QuestionsDTO();
        //                q.ICD_9 = oj[1].ToString();
        //                q.Parent_ID = oj[0].ToString();
        //                q.Diagnosis_Description = oj[2].ToString();
        //                q.Mutually_Exclusive = oj[3].ToString();
        //                q.Leaf_Node = oj[4].ToString();
        //                QuestionsList.Add(q);
        //                mutuallyExclusive = q.Mutually_Exclusive;
        //            }
        //        }
        //    }
        //    return QuestionsList;
        //}

        //public IList<FillAssessment> FillAssessmentUsingEncounterID(ulong encounterID, ulong humanID)
        //{
        //    IList<FillAssessment> objFillAssessment = new List<FillAssessment>();
        //    IList<Assessment> assessList = new List<Assessment>();
        //    IList<AssessmentSource> assessSourceList = new List<AssessmentSource>();
        //    IList<ProblemList> probList = new List<ProblemList>();
        //    string referenceSource = "Assessment";

        //    ICriteria criteriaAssessment = session.GetISession().CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", encounterID)).Add(Expression.Eq("Assessment_Type","Selected"));
        //    assessList = criteriaAssessment.List<Assessment>();

        //    ICriteria criteriaAssessmentSource = session.GetISession().CreateCriteria(typeof(AssessmentSource)).Add(Expression.Eq("Encounter_Id", encounterID));
        //    assessSourceList = criteriaAssessmentSource.List<AssessmentSource>();

        //    ICriteria criteriaProblemList = session.GetISession().CreateCriteria(typeof(ProblemList)).Add(Expression.Eq("Human_ID", humanID)).Add(Expression.Eq("Reference_Source", referenceSource));
        //    probList = criteriaProblemList.List<ProblemList>();

        //    FillAssessment a = null;
        //    for (int i = 0; i < assessList.Count; i++)
        //    {
        //        a = new FillAssessment();
        //a.Assessment_Type = assessList[i].Assessment_Type;
        //a.Primary_Diagnosis = assessList[i].Primary_Diagnosis;
        //a.Chronic_Problem = assessList[i].Chronic_Problem;
        //a.ICD_9 = assessList[i].ICD_9;
        //a.Description = assessList[i].Description;
        //a.Status = assessList[i].Status;
        //a.Assessment_Notes = assessList[i].Assessment_Notes;
        //a.Assessment_ID = assessList[i].Id;
        //a.Assessment_Version = assessList[i].Version;

        //var sourceVersion = (from source in assessSourceList
        //                     where source.Assessment_Id == assessList[i].Id && source.Encounter_Id == assessList[i].Encounter_ID
        //                     select source);
        //foreach (var version in sourceVersion)
        //{
        //    a.Assessment_Source_Version = version.Version;
        //    a.Assessment_Source_ID = version.Id;
        //}
        //if (sourceVersion.Count() == 0)
        //{
        //    a.Assessment_Source_Version = 0;
        //}

        //var problemList = (from prob in probList
        //                   where prob.Encounter_ID == assessList[i].Encounter_ID && prob.Reference_ID == assessList[i].Id &&
        //                   prob.ICD_Code == assessList[i].ICD_9
        //                   select prob);

        //foreach (var pro in problemList)
        //{
        //    a.Problem_List_ID = pro.Id;
        //    a.Problem_List_Version = pro.Version;
        //}
        //if (problemList.Count() == 0)
        //{
        //    a.Problem_List_ID = 0;
        //    a.Problem_List_Version = 0;
        //}
        //        objFillAssessment.Add(a);
        //    }
        //    //IQuery query1 = session.GetISession().GetNamedQuery("Fill.Assessment.GetAssessmentAndProblemListUsingEncounterId");
        //    //query1.SetString(0, encounterID.ToString());
        //    //AssessmentList = new ArrayList(query1.List());
        //    //FillAssessment a = null;
        //    ////Added Latha - 2/8/10 - Checking for null value.
        //    //if (AssessmentList != null)
        //    //{
        //    //    for (int i = 0; i < AssessmentList.Count; i++)
        //    //    {
        //    //        a = new FillAssessment();
        //    //        object[] obj = (object[])AssessmentList[i];
        //    //        a.Assessment_Type = obj[0].ToString();
        //    //        a.Primary_Diagnosis = obj[1].ToString();
        //    //        a.Chronic_Problem = obj[2].ToString();
        //    //        a.ICD_9 = obj[3].ToString();
        //    //        a.Description = obj[4].ToString();
        //    //        a.Status = obj[5].ToString();
        //    //        a.Assessment_Notes = obj[6].ToString();
        //    //        a.Assessment_ID = Convert.ToUInt64(obj[7]);
        //    //        a.Problem_List_ID = Convert.ToUInt64(obj[8]);
        //    //        a.Assessment_Version = Convert.ToInt32(obj[9]);
        //    //        a.Problem_List_Version = Convert.ToInt32(obj[10]);
        //    //        a.Assessment_Source_Version = Convert.ToInt32(obj[11]);
        //    //        //a.Assessment_Problem_List_Id_Diff = Convert.ToUInt64(obj[12]);
        //    //        //a.Assessment_Problem_List_Version = Convert.ToInt32(obj[13]);
        //    //        objFillAssessment.Add(a);
        //    //    }
        //    //}
        //    return objFillAssessment;
        //}

        public Assessment GetAssesmentUsingAssesmentId(ulong ulAssementID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            Assessment objAssesment = new Assessment();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Id", ulAssementID));
                if (criteria.List<Assessment>().Count > 0)
                {
                    objAssesment = criteria.List<Assessment>()[0];
                }
                iMySession.Close();
            }
            return objAssesment;

        }

        public FillAssessment GetAssesmentForPastEncounter(ulong encounterId, ulong humanId, ulong physicianId, DateTime dtDOS)
        {
            FillAssessment objFillAssessment = new FillAssessment();
            EncounterManager objEncounterManager = new EncounterManager();
            IList<AssessmentVitalsLookup> AssessmentVitalsLookupList = new List<AssessmentVitalsLookup>();

            ulong previousEncounterId = 0;

            bool isPhysicianProcess = false;
            bool isFromArchive = false;

            var ilstEncounter = objEncounterManager.GetPreviousEncounterDetails(encounterId, humanId, physicianId, out isPhysicianProcess, out isFromArchive);

            if (ilstEncounter.Count > 0)
            {
                previousEncounterId = ilstEncounter[0].Id;
                dtDOS = ilstEncounter[0].Date_of_Service;

                objFillAssessment.Physician_Process = isPhysicianProcess;
                objFillAssessment.PEncID = previousEncounterId;

                if (isPhysicianProcess)
                {
                    GeneralNotesManager objGeneralNotesManager = new GeneralNotesManager();
                    ProblemListManager objProblemListManager = new ProblemListManager();
                    VitalsManager objVitalsManager = new VitalsManager();

                    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                    {
                        ICriteria crit2 = iMySession.CreateCriteria(typeof(AssessmentVitalsLookup));
                        AssessmentVitalsLookupList = crit2.List<AssessmentVitalsLookup>();
                    }

                    //objFillAssessment.Assessment = GetAssessmentPerEncounterIDForOrders(previousEncounterId, isFromArchive, dtDOS);
                    //GitLab # 1590
                    IList<Assessment> ilstAssessment = new List<Assessment>();
                    ilstAssessment = GetAssessmentPerEncounterIDForOrders(previousEncounterId, isFromArchive, dtDOS);
                    IList<Assessment> ilstVitalsBasedAssessment = new List<Assessment>();
                    for (int iCount = 0; iCount < ilstAssessment.Count; iCount++)
                    {
                        var AssessmentVitalsicd = (from obj in AssessmentVitalsLookupList where obj.ICD_10 == ilstAssessment[iCount].ICD select obj).ToList<AssessmentVitalsLookup>();
                        if (AssessmentVitalsicd.Count > 0)
                        {
                            ilstVitalsBasedAssessment.Add(ilstAssessment[iCount]);
                        }
                    }

                    for (int iCount = 0; iCount < ilstVitalsBasedAssessment.Count; iCount++)
                    {
                        ilstAssessment.Remove(ilstVitalsBasedAssessment[iCount]);
                    }
                    EandMCodingICDManager eandMManager = new EandMCodingICDManager();
                    IList<EandMCodingICD> lsticd = new List<EandMCodingICD>();
                    lsticd = eandMManager.GetEandMCodingICDPastEncounters(previousEncounterId, isFromArchive);
                    objFillAssessment.Assessment = ilstAssessment;
                    objFillAssessment.EandMICD = lsticd;
                    objFillAssessment.General_Notes = objGeneralNotesManager.GetGeneralNotes(previousEncounterId,
                                                                                            "Selected Assessment");

                    objFillAssessment.AssessmentCurrentList = GetAssessmentPerEncounterIDForOrders(encounterId, false, dtDOS);

                    objFillAssessment.General_NotesCurrentList = objGeneralNotesManager.GetGeneralNotes(encounterId,
                                                                                                       "Selected Assessment");
                    objFillAssessment.Potential_Diagnosis = new List<PotentialDiagnosis>();

                    //objFillAssessment.Problem_List = objProblemListManager.GetProblemDescription(humanId);
                    //GitLab # 1590
                    IList<ProblemList> ilstProblem_List = new List<ProblemList>();
                    ilstProblem_List = objProblemListManager.GetProblemDescription(humanId);
                    IList<ProblemList> ilstVitalsBasedProblem = new List<ProblemList>();
                    for (int iCount = 0; iCount < ilstProblem_List.Count; iCount++)
                    {
                        var Problem_listAssessmentVitalsicd = (from obj in AssessmentVitalsLookupList where obj.ICD_10 == ilstProblem_List[iCount].ICD select obj).ToList<AssessmentVitalsLookup>();
                        if (Problem_listAssessmentVitalsicd.Count > 0)
                        {
                            ilstVitalsBasedProblem.Add(ilstProblem_List[iCount]);
                        }
                    }

                    for (int iCount = 0; iCount < ilstVitalsBasedProblem.Count; iCount++)
                    {
                        ilstProblem_List.Remove(ilstVitalsBasedProblem[iCount]);
                    }
                    objFillAssessment.Problem_List = ilstProblem_List;

                    //var lstPatientResults = objVitalsManager.GetVitalsByHumanEncounter(previousEncounterId, humanId);
                    //GitLab # 1590
                    var lstPatientResults = objVitalsManager.GetVitalsByHumanEncounter(encounterId, humanId);


                    if (lstPatientResults.Count > 0 || AssessmentVitalsLookupList.Count > 0)
                    {
                        AssessmentVitalsLookupManager objAssessVitalsMngr = new AssessmentVitalsLookupManager();
                        objFillAssessment.VitalsBasedICD_List = objAssessVitalsMngr.GetIcdBasedOnVitals(AssessmentVitalsLookupList, lstPatientResults);
                    }

                }
            }
            return objFillAssessment;
        }

        //public FillAssessmentRuledOut GetAssessmentRuledOutGetAssesmentForPastEncounter(ulong encounter_ID, ulong human_ID, ulong Physician_ID)
        //{
        //    //ISession iMySessionAssessmentRuledOut = NHibernateSessionManager.Instance.CreateISession();
        //    FillAssessmentRuledOut objFillAssessment = new FillAssessmentRuledOut();
        //    //EncounterManager encManager = new EncounterManager();
        //    //IList<Encounter> encounter = encManager.GetEncounterByEncounterID(encounter_ID);
        //    WFObjectManager objWFObjectManager = new WFObjectManager();
        //    ulong encID=0;
        //    ulong physicianId = Physician_ID;
        //    using (ISession iMySessionAssessmentRuledOut = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        try
        //        {
        //            ISQLQuery ResultList = iMySessionAssessmentRuledOut.CreateSQLQuery("SELECT e.* FROM encounter e WHERE e.Human_ID='" + human_ID + "' and e.Encounter_Provider_ID='" + Convert.ToInt32(physicianId) + "' and e.Date_of_Service <>'0001-01-01 00:00:00' and e.Date_of_Service<(select e.Date_of_Service from encounter e where e.Encounter_ID=" + encounter_ID + ")and e.Encounter_ID<>" + encounter_ID + " and e.Is_Phone_Encounter <> 'Y'  Order By e.Date_of_Service desc").AddEntity("e", typeof(Encounter));
        //            if (ResultList.List<Encounter>().Count == 1)
        //            {
        //                encID = ResultList.List<Encounter>()[0].Id;
        //                if (!objWFObjectManager.IsPreviousEncounterPhysicianProcess(encID))
        //                {
        //                    objFillAssessment.Physician_Process = false;
        //                    objFillAssessment.PEncID = encID;
        //                    return objFillAssessment;
        //                }
        //                else
        //                {
        //                    objFillAssessment.Physician_Process = true;
        //                    objFillAssessment.PEncID = encID;
        //                }
        //            }
        //            else
        //            {
        //                bool phy_process = false;
        //                for (int i = 0; i < ResultList.List<Encounter>().Count; i++)
        //                {
        //                    phy_process = objWFObjectManager.IsPreviousEncounterPhysicianProcess(ResultList.List<Encounter>()[i].Id);
        //                    if (phy_process == true)
        //                    {

        //                        encID = ResultList.List<Encounter>()[i].Id;
        //                        objFillAssessment.Physician_Process = true;
        //                        objFillAssessment.PEncID = encID;
        //                        break;
        //                    }
        //                }
        //                if (phy_process == false)
        //                {
        //                    objFillAssessment.Physician_Process = false;
        //                    objFillAssessment.PEncID = encID;
        //                    return objFillAssessment;
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            encID = 0;
        //        }

        //        if (encID != 0)
        //        {
        //            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
        //            objFillAssessment.Physician_Process = true;
        //            ICriteria criteria = iMySessionAssessmentRuledOut.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", encID)).Add(Expression.Eq("Assessment_Type", "Ruled Out"));
        //            objFillAssessment.Assessment = criteria.List<Assessment>();

        //            ICriteria criteriaGeneralNotes = iMySessionAssessmentRuledOut.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", encID)).Add(Expression.Eq("Parent_Field", "Ruled Out Assessment"));
        //            objFillAssessment.General_Notes = criteriaGeneralNotes.List<GeneralNotes>();
        //            objFillAssessment.PEncID = encID;

        //            ICriteria criteriaAssessmentCurrent = iMySessionAssessmentRuledOut.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", encounter_ID)).Add(Expression.Eq("Assessment_Type", "Ruled Out"));
        //            objFillAssessment.AssessmentCurrentList = criteriaAssessmentCurrent.List<Assessment>();

        //            ICriteria criteriaGeneralNotesCurrent = iMySessionAssessmentRuledOut.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Encounter_ID", encounter_ID)).Add(Expression.Eq("Parent_Field", "Ruled Out Assessment"));
        //            objFillAssessment.General_NotesCurrentList = criteriaGeneralNotesCurrent.List<GeneralNotes>();

        //            //iMySessionAssessmentRuledOut.Close();
        //            //IList<Encounter> enclst = new List<Encounter>();
        //            //ICriteria crit = session.GetISession().CreateCriteria(typeof(Encounter)).Add(Expression.Eq("Id", encounter_ID));
        //            //enclst = crit.List<Encounter>();

        //            //if (enclst.Count != 0)
        //            //{
        //            //    if (enclst[0].Is_Previous_Encounter_Copied == "N")
        //            //    {
        //            //        ChiefComplaintsManager chiefcomplaint = new ChiefComplaintsManager();
        //            //        chiefcomplaint.UpdateEncounter(encounter_ID, "Y", string.Empty);
        //            //    }

        //            //}
        //        }
        //        iMySessionAssessmentRuledOut.Close();
        //    }
        //    return objFillAssessment;
        //}

        public IList<Assessment> GetAssessmentPerEncounterIDForOrders(ulong EncounterID)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<Assessment> returnList = new List<Assessment>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                if (EncounterID != 0)
                {
                    ICriteria criteria1 = iMySession.CreateCriteria(typeof(Assessment))
                        .Add(Expression.Eq("Encounter_ID", EncounterID))
                        .Add(Expression.Eq("Assessment_Type", "Selected"));
                    returnList = criteria1.List<Assessment>();
                }
                iMySession.Close();
            }
            return returnList;
        }

        public IList<Assessment> GetAssessmentPerEncounterIDForOrders(ulong encounterId, bool isFromArchive, DateTime dtDOS)
        {
            IList<Assessment> lstAssessment = new List<Assessment>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                var querySQL = string.Format(@"SELECT E.* 
                                               FROM   {0} E  LEFT JOIN ALL_ICD A on E.ICD=A.ALL_ICD
                                               WHERE  E.ASSESSMENT_TYPE = 'Selected' 
                                               AND    E.ENCOUNTER_ID = :ENCOUNTER_ID
                                               AND  DATE_FORMAT(A.FROM_DATE,'%Y-%m-%d')<= DATE('" + dtDOS.ToString("yyyy-MM-dd") + "') AND DATE_FORMAT(A.TO_DATE,'%Y-%m-%d')>=DATE('" + dtDOS.ToString("yyyy-MM-dd") + "')",
                                               isFromArchive ? "ASSESSMENT_ARC" : "ASSESSMENT");

                var SQLQuery = iMySession.CreateSQLQuery(querySQL)
                       .AddEntity("E", typeof(Assessment));

                SQLQuery.SetParameter("ENCOUNTER_ID", encounterId);

                lstAssessment = SQLQuery.List<Assessment>();

                iMySession.Close();
            }
            return lstAssessment;
        }
        //$

        public IList<ICD9ICD10Mapping> loadICD10Temp(string icd9)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<ICD9ICD10Mapping> ICD9MappingList = new List<ICD9ICD10Mapping>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteriaICD9ICD10Mapping = iMySession.CreateCriteria(typeof(ICD9ICD10Mapping)).Add(Expression.Eq("ICD9", icd9));
                ICD9MappingList = criteriaICD9ICD10Mapping.List<ICD9ICD10Mapping>();
                iMySession.Close();
            }
            return ICD9MappingList;
        }

        public IList<Assessment> GetAssessmentForRecommendedMaterials(ulong ulEncounterId, ulong ulHumanId)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<Assessment> AssesmentList = new List<Assessment>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", ulEncounterId)).Add(Expression.Eq("Human_ID", ulHumanId));
                AssesmentList = criteria.List<Assessment>();
                iMySession.Close();
            }
            return AssesmentList;
        }
        public IList<Assessment> GetAssessmentICD(ulong EncID, ulong Human_ID, IList<string> ICD)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<Assessment> AssessmentICDList = new List<Assessment>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria Asscriteria = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", EncID)).Add(Expression.Eq("Human_ID", Human_ID)).Add(Expression.In("ICD_9", ICD.ToArray()));
                AssessmentICDList = Asscriteria.List<Assessment>();
                iMySession.Close();
            }
            return AssessmentICDList;

        }

        public void SaveAssesmentforSummary(IList<Assessment> lstassesment)
        {
            //IList<Assessment> updatelist = null;
            //commented on 24-03-2016 as it is not being used
            //SaveUpdateDeleteWithTransaction(ref lstassesment, null, null, string.Empty);


        }

        //not in use
        //public void BatchOperationsinCopyPreviousEncounterToAssessment(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate, IList<Assessment> ListToDelete, IList<ProblemList> ListToInsertProblemList, IList<ProblemList> ListToUpdateProblemList, IList<ProblemList> ListToDeleteProblemList, string sMacAddress, GeneralNotes generalNotes, TreatmentPlan SavePlan, string UserName)
        //{
        //    iTryCount = 0;
        //    #region Treatment Plan Get
        //    string sPlan = string.Empty;
        //    IList<ProblemList> acsProblemlist = new List<ProblemList>();
        //    ulong[] proList = new ulong[ListToUpdateProblemList.Count];
        //    if (ListToUpdateProblemList.Count > 0)
        //    {
        //        acsProblemlist = ListToUpdateProblemList.OrderBy(e => e.Id).ToList();
        //        for (int i = 0; i < ListToUpdateProblemList.Count; i++)
        //            proList[i] = acsProblemlist[i].Id;
        //    }
        //    ulong humanId = 0;
        //    IList<ProblemList> probLst = new List<ProblemList>();
        //    ProblemListManager problemMngr = new ProblemListManager();
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria criteriaProblemList = iMySession.CreateCriteria(typeof(ProblemList)).Add(Expression.In("Id", proList)).AddOrder(Order.Asc("Id"));

        //        if (ListToInsertProblemList.Count > 0)
        //        {
        //            humanId = ListToInsertProblemList[0].Human_ID;

        //        }
        //        if (ListToUpdateProblemList.Count > 0)
        //        {
        //            humanId = ListToUpdateProblemList[0].Human_ID;
        //        }
        //        if (ListToDeleteProblemList.Count > 0)
        //        {
        //            humanId = ListToDeleteProblemList[0].Human_ID;
        //        }
        //        probLst = problemMngr.GetICDByHumanId(humanId);
        //        TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
        //        IList<TreatmentPlan> Treatment_Plan = objTreatmentPlanMgr.GetTreatmentPlanUsingEncounterId(SavePlan.Encounter_Id, SavePlan.Human_ID);
        //        TreatmentPlan SaveTreatmentPlan = null;
        //        TreatmentPlan UpdateTreatmentPlan = null;
        //        TreatmentPlan DeleteTreatmentPlan = null;
        //        IList<TreatmentPlan> Treatmentlst = new List<TreatmentPlan>();
        //        bool bdelCheck = false;
        //        string sMyFinalFormatString = string.Empty;
        //        IList<int> index = new List<int>();
        //        Treatmentlst = (from t in Treatment_Plan where t.Plan_Type.ToUpper() == "ASSESSMENT" select t).ToList<TreatmentPlan>();
        //        if (Treatmentlst.Count > 0)
        //            sPlan = Treatmentlst[0].Plan;

        //        if (sPlan.Contains("\r\n\r\n") == true)
        //        {
        //            sPlan = sPlan.Replace("\r\n\r\n", "\r\n");
        //        }
        //        string strIcd = SavePlan.Plan;
        //        if (ListToUpdate != null && ListToUpdate.Count > 0)
        //        {
        //            for (int b = 0; b < ListToUpdate.Count; b++)
        //            {
        //                string strUpdatePlan = "* " + ListToUpdate[b].ICD_Description + "(ICD- " + ListToUpdate[b].ICD_9 + ")";

        //                if (ListToUpdate[b].Assessment_Notes != string.Empty || ListToUpdate[b].Assessment_Status != string.Empty)
        //                    strUpdatePlan += (ListToUpdate[b].Assessment_Notes != string.Empty ? " - " + ListToUpdate[b].Assessment_Notes : string.Empty) + (ListToUpdate[b].Assessment_Status != string.Empty ? " - " + ListToUpdate[b].Assessment_Status : string.Empty);

        //                IList<Assessment> UpdateAss = (iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Id", ListToUpdate[b].Id))).List<Assessment>();
        //                if (UpdateAss != null && UpdateAss.Count > 0)
        //                {
        //                    string sPlanOld = "* " + UpdateAss[0].ICD_Description + "(ICD- " + UpdateAss[0].ICD_9 + ")";
        //                    if (UpdateAss[0].Assessment_Notes != string.Empty || UpdateAss[0].Assessment_Status != string.Empty)
        //                        sPlanOld += (UpdateAss[0].Assessment_Notes != string.Empty ? " - " + UpdateAss[0].Assessment_Notes : string.Empty) + (UpdateAss[0].Assessment_Status != string.Empty ? " - " + UpdateAss[0].Assessment_Status : string.Empty);
        //                    sPlan = sPlan.Replace(sPlanOld.Trim(), strUpdatePlan.Trim());
        //                }
        //            }
        //        }

        //        if (ListToDelete != null && ListToDelete.Count > 0)
        //        {
        //            //bdelCheck = true;

        //            for (int c = 0; c < ListToDelete.Count; c++)
        //            {

        //                IList<Assessment> DeleteAss = (iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Id", ListToDelete[c].Id))).List<Assessment>();
        //                if (DeleteAss != null && DeleteAss.Count > 0)
        //                {
        //                    string strAss = "* " + DeleteAss[0].ICD_Description + "(ICD- " + DeleteAss[0].ICD_9 + ")";
        //                    if (DeleteAss[0].Assessment_Notes != string.Empty || DeleteAss[0].Assessment_Status != string.Empty)
        //                        strAss += (DeleteAss[0].Assessment_Notes != string.Empty ? " - " + DeleteAss[0].Assessment_Notes : string.Empty) + (DeleteAss[0].Assessment_Status != string.Empty ? " - " + DeleteAss[0].Assessment_Status : string.Empty);



        //                    sPlan = sPlan.Replace(strAss.Trim() + Environment.NewLine, "");

        //                }
        //            }



        //        }

        //        if (generalNotes != null)
        //        {
        //            if (generalNotes.Id != 0)
        //            {
        //                IList<GeneralNotes> GeneralNotesUpdate = (iMySession.CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Id", generalNotes.Id))).List<GeneralNotes>();
        //                if (GeneralNotesUpdate != null && GeneralNotesUpdate.Count > 0)
        //                {
        //                    string strPlanOld = string.Empty;
        //                    if (GeneralNotesUpdate[0].Notes != string.Empty)
        //                        strPlanOld = "* " + GeneralNotesUpdate[0].Notes;
        //                    string strNewNotes = string.Empty;
        //                    if (generalNotes.Notes != string.Empty)
        //                        strNewNotes = "* " + generalNotes.Notes;
        //                    if (strPlanOld != string.Empty)
        //                        sPlan = sPlan.Replace(strPlanOld.Trim(), "$" + strNewNotes).Trim();
        //                    else
        //                        sPlan += "$" + strNewNotes;
        //                }
        //            }
        //            else if (generalNotes.Id == 0 && generalNotes.Notes != string.Empty)
        //            {
        //                if (strIcd.Split('*').Count() > 0 && !strIcd.Split('*').Any(s => s.ToUpper() == (" " + generalNotes.Notes.ToUpper() + Environment.NewLine)))
        //                    strIcd += "* " + generalNotes.Notes + Environment.NewLine;
        //            }
        //        }

        //        if (Treatmentlst.Count == 0)
        //        {

        //            SaveTreatmentPlan = new TreatmentPlan();
        //            SaveTreatmentPlan.Encounter_Id = SavePlan.Encounter_Id;
        //            SaveTreatmentPlan.Human_ID = SavePlan.Human_ID;
        //            SaveTreatmentPlan.Physician_Id = SavePlan.Physician_Id;
        //            SaveTreatmentPlan.Plan_Type = "ASSESSMENT";
        //            SaveTreatmentPlan.Plan = strIcd;
        //            SaveTreatmentPlan.Created_By = SavePlan.Created_By;
        //            SaveTreatmentPlan.Created_Date_And_Time = SavePlan.Created_Date_And_Time;
        //        }
        //        else if (Treatmentlst.Count > 0)
        //        {
        //            //if (strIcd != string.Empty)
        //            //  sPlan += Environment.NewLine +"*"+strIcd;
        //            if (strIcd != string.Empty)
        //            {
        //                string[] splitNotes = sPlan.Split('$');
        //                try
        //                {
        //                    sPlan = splitNotes[0] + strIcd + splitNotes[1] + Environment.NewLine;
        //                }
        //                catch
        //                {
        //                    sPlan += strIcd;
        //                }

        //            }
        //            if (sPlan.Contains('$') == true)
        //            {
        //                sPlan = sPlan.Replace("$", "");
        //            }
        //            if (sPlan.EndsWith("\r\n") == false)
        //            {
        //                sPlan = sPlan.Insert(sPlan.Length, "\r\n");
        //            }
        //            if (sPlan.Trim() != string.Empty)
        //            {
        //                UpdateTreatmentPlan = new TreatmentPlan();
        //                UpdateTreatmentPlan = Treatmentlst[0];
        //                string sPrimaryIcd = string.Empty;

        //                if (ListToInsert.Count > 0 || ListToUpdate.Count > 0)
        //                {
        //                    var insert = from l in ListToInsert where l.Primary_Diagnosis == "Y" select l;
        //                    var update = from q in ListToUpdate where q.Primary_Diagnosis == "Y" select q;
        //                    if (insert.Count() > 0)
        //                    {
        //                        sPrimaryIcd = "* " + insert.First().ICD_Description + "(ICD- " + insert.First().ICD_9 + ")";
        //                        sPrimaryIcd += (insert.First().Assessment_Notes != string.Empty ? " - " + insert.First().Assessment_Notes : string.Empty) + (insert.First().Assessment_Status != string.Empty ? " - " + insert.First().Assessment_Status : string.Empty);
        //                    }
        //                    else if (update.Count() > 0)
        //                    {
        //                        sPrimaryIcd = "* " + update.First().ICD_Description + "(ICD- " + update.First().ICD_9 + ")";
        //                        sPrimaryIcd += (update.First().Assessment_Notes != string.Empty ? " - " + update.First().Assessment_Notes : string.Empty) + (update.First().Assessment_Status != string.Empty ? " - " + update.First().Assessment_Status : string.Empty);
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(sPrimaryIcd))
        //                    sPlan = sPlan.Replace(sPrimaryIcd + "\r\n", "");
        //                UpdateTreatmentPlan.Plan = string.IsNullOrEmpty(sPrimaryIcd) ? sPlan : sPrimaryIcd + Environment.NewLine + sPlan;
        //                UpdateTreatmentPlan.Modified_By = SavePlan.Created_By;
        //                UpdateTreatmentPlan.Modified_Date_And_Time = SavePlan.Created_Date_And_Time;
        //            }
        //            else
        //            {
        //                DeleteTreatmentPlan = new TreatmentPlan();
        //                DeleteTreatmentPlan = Treatmentlst[0];
        //                DeleteTreatmentPlan.Modified_By = SavePlan.Created_By;
        //                DeleteTreatmentPlan.Modified_Date_And_Time = SavePlan.Created_Date_And_Time;
        //            }
        //        }
        //    //iMySession.Close();
        //    #endregion
        //    TryAgain:
        //        int iResult = 0;
        //        //Session.GetISession().Clear();
        //        ulong id = 0;
        //        ulong phyId = 0;
        //        string strIsRuledOut = string.Empty;
        //        Human humanRecord = new Human();
        //        using (ISession MySession = Session.GetISession())
        //        {
        //            try
        //            {
        //                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //                {
        //                    HumanManager hManager = new HumanManager();


        //                    #region GetHumanRecordForRCopia
        //                    if (ListToInsert != null && ListToInsert.Count > 0)
        //                    {
        //                        id = ListToInsert[0].Human_ID;
        //                    }
        //                    else if (ListToUpdate != null && ListToUpdate.Count > 0)
        //                    {
        //                        id = ListToUpdate[0].Human_ID;
        //                    }
        //                    else if (ListToDelete != null && ListToDelete.Count > 0)
        //                    {
        //                        id = ListToDelete[0].Human_ID;
        //                    }

        //                    humanRecord = hManager.GetById(id);
        //                    #endregion

        //                    try
        //                    {
        //                        // trans = 

        //                        IList<Assessment> ListToInsertAssessment = new List<Assessment>();
        //                        ListToInsertAssessment = ListToInsert;

        //                        #region Assessment

        //                        //Added Latha - 2/8/10 - Checking for null value.
        //                        if ((ListToInsertAssessment != null) && (ListToUpdate != null) && (ListToDelete != null))
        //                        {
        //                            if ((ListToInsertAssessment.Count > 0) || (ListToUpdate.Count > 0) || (ListToDelete.Count > 0))
        //                            {
        //                                if (ListToInsertAssessment.Count == 0)
        //                                {
        //                                    ListToInsertAssessment = null;

        //                                }
        //                                else if (ListToUpdate.Count == 0)
        //                                {
        //                                    ListToUpdate = null;
        //                                }
        //                                else if (ListToDelete.Count == 0)
        //                                {
        //                                    ListToDelete = null;
        //                                }

        //                                //To assign physician id and whether selected assessment or ruled out assessment which is needed in the load function parameter.
        //                                if (ListToInsertAssessment != null && ListToInsertAssessment.Count > 0)
        //                                {
        //                                    AssignPhyIdAndIsRuledOutForLoadFunction(ref phyId, ref strIsRuledOut, ListToInsertAssessment);
        //                                }
        //                                else if (ListToUpdate != null && ListToUpdate.Count > 0)
        //                                {
        //                                    AssignPhyIdAndIsRuledOutForLoadFunction(ref phyId, ref strIsRuledOut, ListToUpdate);
        //                                }
        //                                else if (ListToDelete != null && ListToDelete.Count > 0)
        //                                {
        //                                    AssignPhyIdAndIsRuledOutForLoadFunction(ref phyId, ref strIsRuledOut, ListToDelete);
        //                                }

        //                                //iResult = SaveUpdateDeleteWithoutTransaction(ref ListToInsertAssessment, ListToUpdate, ListToDelete, MySession, sMacAddress);
        //                                //if bResult = false then, the deadlock is occured 
        //                                if (iResult == 2)
        //                                {
        //                                    if (iTryCount < 5)
        //                                    {
        //                                        iTryCount++;
        //                                        goto TryAgain;
        //                                    }
        //                                    else
        //                                    {
        //                                        trans.Rollback();
        //                                        //MySession.Close();
        //                                        throw new Exception("Deadlock occurred. Transaction failed.");
        //                                    }
        //                                }
        //                                else if (iResult == 1)
        //                                {
        //                                    trans.Rollback();
        //                                    // MySession.Close();
        //                                    throw new Exception("Exception occurred. Transaction failed.");
        //                                }
        //                            }
        //                        }

        //                        #endregion



        //                        #region unused Methods

        //                        #endregion

        //                        #region problemList
        //                        //Added Latha - 2/8/10 - Checking for null value.


        //                        IList<Assessment> assList = null;
        //                        assList = ListToInsertAssessment;
        //                        if ((ListToInsertProblemList != null) && (ListToUpdateProblemList != null) && (ListToDeleteProblemList != null))
        //                        {
        //                            if ((ListToInsertProblemList.Count > 0) || (ListToUpdateProblemList.Count > 0) || (ListToDeleteProblemList.Count > 0))
        //                            {




        //                                IList<ProblemList> prob = new List<ProblemList>();


        //                                if (ListToUpdateProblemList.Count > 0)
        //                                {





        //                                    prob = criteriaProblemList.List<ProblemList>();

        //                                    for (int i = 0; i < prob.Count; i++)
        //                                        acsProblemlist[i].Version = prob[i].Version;

        //                                    ListToUpdateProblemList = acsProblemlist;
        //                                }

        //                                DateTime currTime = DateTime.UtcNow;


        //                                if (ListToInsertProblemList.Count > 0)
        //                                {
        //                                    humanId = ListToInsertProblemList[0].Human_ID;
        //                                    currTime = ListToInsertProblemList[0].Created_Date_And_Time;
        //                                }
        //                                else if (ListToUpdateProblemList.Count > 0)
        //                                {
        //                                    humanId = ListToUpdateProblemList[0].Human_ID;
        //                                    currTime = ListToUpdateProblemList[0].Created_Date_And_Time;
        //                                }
        //                                else if (ListToDeleteProblemList.Count > 0)
        //                                {
        //                                    humanId = ListToDeleteProblemList[0].Human_ID;
        //                                    currTime = ListToDeleteProblemList[0].Created_Date_And_Time;
        //                                }
        //                                IList<ProblemList> updatedList = new List<ProblemList>();
        //                                if (humanId != 0 && (ListToUpdateProblemList.Count != 0 || ListToInsertProblemList.Count != 0))
        //                                {

        //                                    var GetActiveProb = from obj in probLst where obj.ICD == "0000" select obj;
        //                                    if (GetActiveProb.ToList().Count > 0)
        //                                    {
        //                                        ProblemList prblst = GetActiveProb.ToList<ProblemList>()[0];
        //                                        prblst.Is_Active = "N";
        //                                        prblst.Modified_By = UserName;
        //                                        prblst.Human_ID = humanId;
        //                                        prblst.Modified_Date_And_Time = currTime;
        //                                        updatedList = new List<ProblemList>(ListToUpdateProblemList);
        //                                        updatedList.Add(prblst);
        //                                    }
        //                                }
        //                                if (updatedList.Count > 0)
        //                                    iResult = problemMngr.BatchOperationsToProblem(ListToInsertProblemList, updatedList, ListToDeleteProblemList, MySession, sMacAddress, false);
        //                                else
        //                                    iResult = problemMngr.BatchOperationsToProblem(ListToInsertProblemList, ListToUpdateProblemList, ListToDeleteProblemList, MySession, sMacAddress, false);

        //                                //if bResult = false then, the deadlock is occured 
        //                                if (iResult == 2)
        //                                {
        //                                    if (iTryCount < 5)
        //                                    {
        //                                        iTryCount++;
        //                                        goto TryAgain;
        //                                    }
        //                                    else
        //                                    {
        //                                        trans.Rollback();
        //                                        //  MySession.Close();
        //                                        throw new Exception("Deadlock occurred. Transaction failed.");
        //                                    }
        //                                }
        //                                else if (iResult == 1)
        //                                {
        //                                    trans.Rollback();
        //                                    //  MySession.Close();
        //                                    throw new Exception("Exception occurred. Transaction failed.");
        //                                }
        //                            }
        //                        }

        //                        ////IList<AssessmentProblemListDifference> ListToUpdateDifference = new List<AssessmentProblemListDifference>();
        //                        //if ((ListToInsertAssessmentProbListDiff != null) && (ListToUpdateAssessmentProbListDiff != null) && (ListToDeleteAssessmentProbListDiff != null))
        //                        //{
        //                        //    if ((ListToInsertAssessmentProbListDiff.Count > 0) || (ListToUpdateAssessmentProbListDiff.Count > 0) || (ListToDeleteAssessmentProbListDiff.Count > 0))
        //                        //    {
        //                        //        AssessmentProblemListDifferenceManager assessmentProblemListDifferenceMgr = new AssessmentProblemListDifferenceManager();
        //                        //        bResult = assessmentProblemListDifferenceMgr.BatchOperationsToAssessmentProblemListDifference(ListToInsertAssessmentProbListDiff, ListToUpdateAssessmentProbListDiff, ListToDeleteAssessmentProbListDiff, MySession, sMacAddress);
        //                        //        //if bResult = false then, the deadlock is occured 
        //                        //        if (bResult == false)
        //                        //        {
        //                        //            if (iTryCount < 5)
        //                        //            {
        //                        //                iTryCount++;
        //                        //                goto TryAgain;
        //                        //            }
        //                        //        }
        //                        //    }
        //                        //}


        //                        #endregion

        //                        #region General Notes

        //                        IList<GeneralNotes> generalNotesInsert = new List<GeneralNotes>();
        //                        IList<GeneralNotes> generalNotesUpdate = new List<GeneralNotes>();
        //                        if (generalNotes != null)
        //                        {
        //                            if (generalNotes.Id == 0)
        //                            {
        //                                generalNotesInsert.Add(generalNotes);
        //                            }
        //                            else
        //                            {
        //                                generalNotesUpdate.Add(generalNotes);
        //                            }
        //                        }

        //                        GeneralNotesManager genMgr = new GeneralNotesManager();
        //                        //iResult = genMgr.SaveUpdateDeleteGeneralNotes(generalNotesInsert, generalNotesUpdate, MySession, sMacAddress);
        //                        if (iResult == 2)
        //                        {
        //                            if (iTryCount < 5)
        //                            {
        //                                iTryCount++;
        //                                goto TryAgain;
        //                            }
        //                            else
        //                            {
        //                                trans.Rollback();
        //                                //MySession.Close();
        //                                throw new Exception("Deadlock occurred. Transaction failed.");
        //                            }
        //                        }
        //                        else if (iResult == 1)
        //                        {
        //                            trans.Rollback();
        //                            // MySession.Close();
        //                            throw new Exception("Exception occurred. Transaction failed.");
        //                        }


        //                        #endregion
        //                        #region Treatment plan

        //                        //  iResult = objTreatmentPlanMgr.SaveUpdateorDeleteTreatmentPlan(SaveTreatmentPlan, UpdateTreatmentPlan, DeleteTreatmentPlan, MySession, sMacAddress);
        //                        if (iResult == 2)
        //                        {
        //                            if (iTryCount < 5)
        //                            {
        //                                iTryCount++;
        //                                goto TryAgain;
        //                            }
        //                            else
        //                            {
        //                                trans.Rollback();
        //                                MySession.Close();
        //                                throw new Exception("Deadlock occurred. Transaction failed.");
        //                            }
        //                        }
        //                        else if (iResult == 1)
        //                        {
        //                            trans.Rollback();
        //                            MySession.Close();
        //                            throw new Exception("Exception occurred. Transaction failed.");
        //                        }


        //                        #endregion
        //                        iMySession.Close();
        //                        MySession.Flush();
        //                        trans.Commit();
        //                    }
        //                    catch (NHibernate.Exceptions.GenericADOException ex)
        //                    {
        //                        trans.Rollback();
        //                        // MySession.Close();
        //                        throw new Exception(ex.Message);
        //                    }
        //                    catch (Exception e)
        //                    {
        //                        trans.Rollback();
        //                        //MySession.Close();
        //                        throw new Exception(e.Message);
        //                    }

        //                    finally
        //                    {
        //                        MySession.Close();
        //                    }
        //                }
        //            }
        //            catch (Exception ex1)
        //            {
        //                //MySession.Close();
        //                throw new Exception(ex1.Message);
        //            }

        //        }
        //    }


        //    //  return LoadAssessment(SavePlan.Encounter_Id, SavePlan.Human_ID, phyId, strIsRuledOut, UserName);
        //}


        public void BatchOperationsinCopyPreviousEncounterToAssessmentFromRuledOut(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate, IList<Assessment> ListToDelete, ulong encounterId, string sMacAddress, GeneralNotes generalNotes)
        {
            iTryCount = 0;

        TryAgain:
            int iResult = 0;

            //  ITransaction trans = null;
            ulong phyId = 0;
            using (ISession MySession = Session.GetISession())
            {
                try
                {
                    using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        try
                        {
                            if ((ListToInsert != null) && (ListToUpdate != null) && (ListToDelete != null))
                            {
                                if ((ListToInsert.Count != 0) || (ListToUpdate.Count != 0) || (ListToDelete.Count != 0))
                                {
                                    #region Ruled Out Assessment
                                    if (ListToInsert.Count == 0)
                                    {
                                        ListToInsert = null;
                                    }
                                    else if (ListToUpdate.Count == 0)
                                    {
                                        ListToUpdate = null;
                                    }
                                    else if (ListToDelete.Count == 0)
                                    {
                                        ListToDelete = null;
                                    }
                                    if (ListToInsert != null && ListToInsert.Count > 0)
                                    {
                                        phyId = ListToInsert[0].Physician_ID;
                                    }
                                    else if (ListToUpdate != null && ListToUpdate.Count > 0)
                                    {
                                        phyId = ListToUpdate[0].Physician_ID;
                                    }
                                    else if (ListToDelete != null && ListToDelete.Count > 0)
                                    {
                                        phyId = ListToDelete[0].Physician_ID;
                                    }
                                    //  iResult = SaveUpdateDeleteWithoutTransaction(ref ListToInsert, ListToUpdate, ListToDelete, MySession, sMacAddress);
                                    //if bResult = false then, the deadlock is occured 
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
                                    #endregion


                                }
                            }

                            #region General Notes
                            IList<GeneralNotes> generalNotesInsert = new List<GeneralNotes>();
                            IList<GeneralNotes> generalNotesUpdate = new List<GeneralNotes>();

                            if (generalNotes.Id == 0)
                            {
                                generalNotesInsert.Add(generalNotes);
                            }
                            else
                            {
                                generalNotesUpdate.Add(generalNotes);
                            }


                            GeneralNotesManager genMgr = new GeneralNotesManager();
                            // iResult = genMgr.SaveUpdateDeleteGeneralNotes(generalNotesInsert, generalNotesUpdate, MySession, sMacAddress);
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
                            #endregion

                            MySession.Flush();
                            trans.Commit();
                        }
                        catch (NHibernate.Exceptions.GenericADOException ex)
                        {
                            trans.Rollback();
                            // MySession.Close();
                            //CAP-1942
                            throw new Exception(ex.Message, ex);
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
                    throw new Exception(ex1.Message, ex1);
                }

            }


        }

        public IList<String> GetICDListForAssessment(IList<AssessmentVitalsLookup> AssessVitalsLookup, IList<ROS> ROSlist, IList<Rcopia_Medication> Medlist, IList<PatientResults> Patient_Resultslist, IList<Healthcare_Questionnaire> Questionnaire, ulong encounterID)
        {
            IList<String> ICDlist = new List<String>();
            IList<String> ROSICDlist = new List<String>();
            IList<String> MedICDlist = new List<String>();
            IList<String> QuestionnaireICDlist = new List<String>();
            IList<PatientResults> Vitalslist = new List<PatientResults>();
            IList<String> VitalICDlist = new List<String>();
            IList<String> VitalListnew = new List<String>();
            IList<string> VitalICd = new List<string>();
            AssessmentVitalsLookupManager objAssessVitalsMngr = new AssessmentVitalsLookupManager();




            if (AssessVitalsLookup != null && AssessVitalsLookup.Count > 0)
            {
                #region ROS
                if (ROSlist != null && ROSlist.Count > 0)
                {
                    IList<AssessmentVitalsLookup> ROSAssessICD = AssessVitalsLookup.Where(a => a.Entity_Name == "review_of_systems").ToList<AssessmentVitalsLookup>();
                    ROSICDlist = (from obj in ROSAssessICD where ROSlist.Any(a => a.Symptom_Name.ToUpper() == obj.Field_Name.ToUpper() && a.Status.ToUpper() == obj.Value.ToUpper()) select obj.ICD_10).ToList<string>();
                }
                #endregion
                #region Medication
                if (Medlist != null && Medlist.Count > 0)
                {
                    IList<AssessmentVitalsLookup> MedAssessICD = AssessVitalsLookup.Where(a => a.Entity_Name == "medication").ToList<AssessmentVitalsLookup>();
                    MedICDlist = (from obj in MedAssessICD where Medlist.Any(a => a.Brand_Name.ToUpper() == obj.Field_Name.ToUpper()) select obj.ICD_10).ToList<string>();
                }
                #endregion
                #region PatientResults
                IList<String> VitalsAssessICDlst = AssessVitalsLookup.Where(a => a.Entity_Name == "patient_results").Select(a => a.Field_Name + "~" + a.Is_Current_Encounter).Distinct().ToList<string>();
                IDictionary<string, string> VitalLoinc = new Dictionary<string, string>();
                foreach (string s in VitalsAssessICDlst)
                {
                    VitalLoinc.Add(s.Split('~')[0].ToString().ToUpper(), s.Split('~')[1].ToString());
                    VitalICd.Add(s.Split('~')[0].ToUpper());
                }
                IList<string> Loinc_Obv = new List<string>();
                IList<string> obv = new List<string>();
                IList<PatientResults> PatRslts = new List<PatientResults>();
                PatientResults ptrs = null;
                Loinc_Obv = (from Vital in Patient_Resultslist select Vital.Loinc_Observation).Distinct().ToList<string>();
                foreach (string obj in Loinc_Obv)
                {
                    IList<PatientResults> PatLoinc = new List<PatientResults>();
                    if (VitalICd.IndexOf(obj.ToUpper()) != -1)
                    {
                        string Is_currentEnc = VitalLoinc[obj.ToUpper()];
                        if (Is_currentEnc == "N")
                        {
                            obv.Add(obj.ToUpper());
                            PatLoinc = (from oj in Patient_Resultslist where oj.Loinc_Observation.ToUpper() == obj.ToUpper() select oj).ToList<PatientResults>();
                            foreach (PatientResults pr in PatLoinc)
                            {
                                PatRslts.Add(pr);
                            }
                        }
                        else
                        {
                            ptrs = Patient_Resultslist.Where(a => a.Loinc_Observation == obj && a.Encounter_ID == encounterID).FirstOrDefault();
                            if (ptrs != null)
                                PatRslts.Add(ptrs);
                        }
                    }
                }
                IList<AssessmentVitalsLookup> VitalsAssessICD = AssessVitalsLookup.Where(a => a.Entity_Name == "patient_results").ToList<AssessmentVitalsLookup>();
                VitalICDlist = objAssessVitalsMngr.GetIcdBasedOnVitals(VitalsAssessICD, PatRslts);//objPatientList : vitals data for current encounter

                foreach (string Loinc_ObvSTring in VitalICDlist)
                {
                    if ((obv.IndexOf(Loinc_ObvSTring.Split('!')[2]) == -1) && (VitalListnew.IndexOf(Loinc_ObvSTring.Split('!')[0]) == -1))
                    {
                        VitalListnew.Add(Loinc_ObvSTring.Split('!')[0]);//to add those with current encounter='Y'
                    }
                }
                foreach (string Loinc_Obvatn in obv)
                {
                    IList<string> VitICD = new List<string>();
                    ulong[] hierarchy = (from obj in VitalICDlist where obj.Split('!')[2].ToUpper() == Loinc_Obvatn.ToUpper() select Convert.ToUInt64(obj.Split('!')[1])).ToArray<ulong>();
                    if (hierarchy.Count() > 0)
                    {
                        ulong max_hiearchy = hierarchy.Max();
                        VitICD = VitalICDlist.Where(a => a.Split('!')[2] == Loinc_Obvatn.ToUpper() && Convert.ToUInt64(a.Split('!')[1]) == max_hiearchy).ToList<string>();
                        foreach (string sICD in VitICD)
                        {
                            if (VitalListnew.IndexOf(sICD.Split('!')[0]) == -1)
                                VitalListnew.Add(sICD.Split('!')[0]);
                        }
                    }


                }
                #endregion
                #region Questionnaire
                if (Questionnaire != null && Questionnaire.Count > 0)
                {
                    IList<AssessmentVitalsLookup> QuestionnaireAssessICD = AssessVitalsLookup.Where(a => a.Entity_Name.Split('$')[0] == "Healthcare_Questionnaire").ToList<AssessmentVitalsLookup>();
                    QuestionnaireICDlist = (from obj in QuestionnaireAssessICD where Questionnaire.Any(a => a.Questionnaire_Type.ToUpper() == obj.Entity_Name.Split('$')[1].ToUpper() && a.Question.ToUpper() == obj.Field_Name.ToUpper() && a.Selected_Option.Trim() != string.Empty && (Convert.ToInt32(a.Selected_Option) >= Convert.ToInt32(obj.Value.Split('-')[0]) && Convert.ToInt32(a.Selected_Option) <= Convert.ToInt32(obj.Value.Split('-')[1]))) select obj.ICD_10).ToList<string>();
                }
                #endregion
            }
            //string Project_Name = string.Empty;
            //if (System.Configuration.ConfigurationManager.AppSettings["ProjectName"] != null)
            //    Project_Name = System.Configuration.ConfigurationManager.AppSettings["ProjectName"];
            // if (Project_Name.ToUpper() == "WISH")
            // {
            for (int i = 0; i < ROSICDlist.Count; i++)
            {
                ICDlist.Add(ROSICDlist[i]);
            }
            for (int i = 0; i < MedICDlist.Count; i++)
            {
                ICDlist.Add(MedICDlist[i]);
            }
            //}

            for (int i = 0; i < VitalListnew.Count; i++)
            {
                ICDlist.Add(VitalListnew[i]);
            }
            for (int i = 0; i < QuestionnaireICDlist.Count; i++)
            {
                ICDlist.Add(QuestionnaireICDlist[i]);
            }
            return ICDlist;
        }


        //Added for Influenza Macra
        public int AssessmentCount(ulong ulHumanID, IList<string> sICD)//string[] sICD)
        {
            int iTotal = 0;
            string Icd = string.Empty;
            foreach (string str in sICD)
            {
                if (Icd.Trim() == string.Empty)
                    Icd = "'" + str + "'";
                else
                    Icd += ",'" + str + "'";
            }
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ArrayList allicd = null;
                //IQuery query = iMySession.GetNamedQuery("Get.AssessmentIcd");
                ISQLQuery sql = iMySession.CreateSQLQuery("select SUM(cnt) from (Select count(*) cnt from assessment where human_id='" + ulHumanID + "' and icd in(" + Icd + ") union (Select count(*) cnt from assessment_arc where human_id='" + ulHumanID + "' and icd in( " + Icd + ")))a");
                allicd = new ArrayList(sql.List());
                //query.SetString(0, ulHumanID.ToString());
                //query.SetParameterList("IcdCode", sICD.ToArray<string>());
                //query.SetString(1, ulHumanID.ToString());
                //query.SetParameterList("IcdCode1", sICD.ToArray<string>());

                //allicd = new ArrayList(query.List());
                if (allicd.Count > 0)
                {
                    iTotal = Convert.ToInt32(allicd[0].ToString());
                }
                iMySession.Close();
            }

            return iTotal;
        }
        public string GetAssessmentICDbyDos(ulong uHuman_id, string sYear, string sRaf_Status)
        {
            string sIcdList = string.Empty;
            ArrayList arr = null;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                string[] RafStatus = sRaf_Status.Split(',');
                ISQLQuery sql = iMySession.CreateSQLQuery("select group_concat(aa) from (select group_concat(a.icd,'|',year(e.date_of_service))  aa  from encounter_arc e, assessment_arc a "
+ "where e.encounter_id=a.encounter_id and e.human_id=a.human_id and e.human_id='" + uHuman_id + "' and "
+ "year(e.date_of_service)='" + sYear + "' and a.Assessment_Status not in(:sRaf_Status) and a.diagnosis_source not like '%|DELETED%' "
+ "union all "
+ "select group_concat(a.icd,'|',year(e.date_of_service))  aa from encounter e, assessment a "
+ "where e.encounter_id=a.encounter_id and e.human_id=a.human_id and e.human_id='" + uHuman_id.ToString() + "' and "
+ "year(e.date_of_service)='" + sYear + "' and a.Assessment_Status not in(:sRaf_Status) and a.diagnosis_source not like '%|DELETED%' ) as d;");
                sql.SetParameterList("sRaf_Status", RafStatus);
                arr = new ArrayList(sql.List());
                if (arr != null && arr.Count > 0)
                {
                    if (arr[0] != null)
                    {
                        sIcdList = arr[0].ToString();
                    }
                }
            }
            return sIcdList;
        }

        public void UploadProblemListToRCopia(IList<Assessment> ListToInsert, IList<Assessment> ListToUpdate, IList<Assessment> ListToDelete, Human MyhumanRecord, string sMacAddress, string sLegalOrg)
        {


            RCopiaTransactionManager objrcoptranMngr = new RCopiaTransactionManager();
            if (ListToInsert != null && ListToInsert.Count > 0)
            {
                IList<ulong> ilstinsertId = new List<ulong>();
                IList<Assessment> ilstassessment = null;
                for (int x = 0; x < ListToInsert.Count; x++)
                {
                    ilstinsertId.Add(ListToInsert[x].Id);
                }
                if (ilstinsertId != null && ilstinsertId.Count > 0)
                {
                    //if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")// For Bug id: 77226
                    //{
                    objrcoptranMngr.SendPatientToRCopia(ListToInsert[0].Human_ID, sMacAddress, sLegalOrg);
                    //}
                    objrcoptranMngr.SendProblemToRCopia(ilstinsertId, "Assesment", false, ilstassessment, sLegalOrg);
                }
            }
            if (ListToUpdate != null && ListToUpdate.Count > 0)
            {
                IList<ulong> ilstUpdateId = new List<ulong>();
                IList<Assessment> ilstassessment = null;
                for (int y = 0; y < ListToUpdate.Count; y++)
                {
                    ilstUpdateId.Add(ListToUpdate[y].Id);
                }
                if (ilstUpdateId != null && ilstUpdateId.Count > 0)
                {
                    //if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
                    //{
                    objrcoptranMngr.SendPatientToRCopia(ListToUpdate[0].Human_ID, sMacAddress, sLegalOrg);
                    //}
                    objrcoptranMngr.SendProblemToRCopia(ilstUpdateId, "Assesment", false, ilstassessment, sLegalOrg);
                }
            }
            if (ListToDelete != null && ListToDelete.Count > 0)
            {
                IList<ulong> ilstDeleteId = new List<ulong>();
                for (int z = 0; z < ListToDelete.Count; z++)
                {
                    ilstDeleteId.Add(ListToDelete[z].Id);
                }
                if (ilstDeleteId != null && ilstDeleteId.Count > 0)
                {
                    //if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
                    //{
                    objrcoptranMngr.SendPatientToRCopia(ListToDelete[0].Human_ID, sMacAddress, sLegalOrg);
                    //}
                    objrcoptranMngr.SendProblemToRCopia(ilstDeleteId, "Assesment", true, ListToDelete, sLegalOrg);
                }
            }
        }
    }

    public class RCopiaThread
    {
        public IList<Assessment> ListToInsert = null;
        public IList<Assessment> ListToUpdate = null;
        public IList<Assessment> ListToDelete = null;
        public Human humanRecord = null;
        public string sMacAddress = string.Empty;
        public string sUserName = string.Empty;
        public string sLegalOrg = string.Empty;
        public ulong ulHumanID = 0;
        public ulong ulEncID = 0;
        public ulong ulPhyID = 0;

        public RCopiaThread(IList<Assessment> MyListToInsert, IList<Assessment> MyListToUpdate, IList<Assessment> MyListToDelete, Human MyhumanRecord, string UserName, string sMyMacAddress, string sMyLegalOrg)
        {
            ListToInsert = MyListToInsert;
            ListToUpdate = MyListToUpdate;
            ListToDelete = MyListToDelete;
            humanRecord = MyhumanRecord;
            sUserName = UserName;
            sMacAddress = sMyMacAddress;
            sLegalOrg = sMyLegalOrg;
        }

        public void UploadProblemListToRCopia()
        {
            try
            {
                RCopiaTransactionManager objrcoptranMngr = new RCopiaTransactionManager();
                if (ListToInsert != null && ListToInsert.Count > 0)
                {
                    IList<ulong> ilstinsertId = new List<ulong>();
                    IList<Assessment> ilstassessment = null;
                    for (int x = 0; x < ListToInsert.Count; x++)
                    {
                        ilstinsertId.Add(ListToInsert[x].Id);
                    }
                    if (ilstinsertId != null && ilstinsertId.Count > 0)
                    {
                        ulHumanID = ListToInsert[0].Human_ID;
                        ulEncID = ListToInsert[0].Encounter_ID;
                        ulPhyID = ListToInsert[0].Physician_ID;
                        //if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")// For Bug id: 77226
                        //{
                        objrcoptranMngr.SendPatientToRCopia(ListToInsert[0].Human_ID, sMacAddress, sLegalOrg);
                        //}
                        objrcoptranMngr.SendProblemToRCopia(ilstinsertId, "Assesment", false, ilstassessment, sLegalOrg);
                    }
                }
                if (ListToUpdate != null && ListToUpdate.Count > 0)
                {
                    IList<ulong> ilstUpdateId = new List<ulong>();
                    IList<Assessment> ilstassessment = null;
                    for (int y = 0; y < ListToUpdate.Count; y++)
                    {
                        ilstUpdateId.Add(ListToUpdate[y].Id);
                    }
                    if (ilstUpdateId != null && ilstUpdateId.Count > 0)
                    {
                        ulHumanID = ListToUpdate[0].Human_ID;
                        ulEncID = ListToUpdate[0].Encounter_ID;
                        ulPhyID = ListToUpdate[0].Physician_ID;
                        //if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
                        //{
                        objrcoptranMngr.SendPatientToRCopia(ListToUpdate[0].Human_ID, sMacAddress, sLegalOrg);
                        //}
                        objrcoptranMngr.SendProblemToRCopia(ilstUpdateId, "Assesment", false, ilstassessment, sLegalOrg);
                    }
                }
                if (ListToDelete != null && ListToDelete.Count > 0)
                {
                    IList<ulong> ilstDeleteId = new List<ulong>();
                    for (int z = 0; z < ListToDelete.Count; z++)
                    {
                        ilstDeleteId.Add(ListToDelete[z].Id);
                    }
                    if (ilstDeleteId != null && ilstDeleteId.Count > 0)
                    {
                        ulHumanID = ListToDelete[0].Human_ID;
                        ulEncID = ListToDelete[0].Encounter_ID;
                        ulPhyID = ListToDelete[0].Physician_ID;
                        //if (humanRecord.Is_Sent_To_Rcopia.ToUpper() == "N")
                        //{
                        objrcoptranMngr.SendPatientToRCopia(ListToDelete[0].Human_ID, sMacAddress, sLegalOrg);
                        //}
                        objrcoptranMngr.SendProblemToRCopia(ilstDeleteId, "Assesment", true, ListToDelete, sLegalOrg);
                    }
                }
            }
            catch (Exception ex)
            {
                string sMsg = string.Empty;
                string sExStackTrace = string.Empty;

                string version = "";
                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                string[] server = version.Split('|');
                string serverno = "";
                if (server.Length > 1)
                    serverno = server[1].Trim();

                if (ex.InnerException != null && ex.InnerException.Message != null)
                    sMsg = ex.InnerException.Message;
                else
                    sMsg = ex.Message;

                if (ex != null && ex.StackTrace != null)
                    sExStackTrace = ex.StackTrace;

                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + "', '" + serverno + "','" + DateTime.Now + "','" + sUserName + "','" + ulEncID + "','" + ulHumanID + "','" + ulPhyID + "','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";

                string ConnectionData;
                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(ConnectionData))
                {
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                    {
                        cmd.Connection = con;
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

    }
}
