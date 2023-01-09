using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Linq;
using Acurus.Capella.Core.DomainObjects;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Acurus.Capella.Core.DTO
{
    public partial class GenerateXml
    {
        Encounter encounter = null;
        PatientInsuredPlan patInsurance = null;
        ChiefComplaints cc = null;
        Healthcare_Questionnaire questionnaire = null;
        PastMedicalHistory psfhMedHistory = null;
        PastMedicalHistoryMaster psfhMedHistoryMaster = null;
        SocialHistory socialhistory = null;
        SocialHistoryMaster socialhistoryMaster = null;
        SurgicalHistory SurHistory = null;
        SurgicalHistoryMaster SurHistoryMaster = null;
        FamilyHistory FamilyHis = null;
        FamilyDisease familyDisease = null;
        FamilyHistoryMaster FamilyHisMaster = null;
        FamilyDiseaseMaster familyDiseaseMaster = null;
        ImmunizationHistory ImmunHistory = null;
        ImmunizationMasterHistory ImmunMasterHistory = null;
        NonDrugAllergy nondrugAllery = null;
        NonDrugAllergyMaster nondrugAlleryMaster = null;
        AdvanceDirective ad = null;
        AdvanceDirectiveMaster adMaster = null;
        PhysicianPatient physicianpatient = null;
        PhysicianPatientMaster physicianpatientmaster = null;
        HospitalizationHistory HospHistory = null;
        HospitalizationHistoryMaster HospHistoryMaster = null;
        FileManagementIndex filemanagementindex = null;
        ProblemList ProblemLst = null;
        ROS ros = null;
        PatientResults patResults = null;
        Examination exam = null;
        Assessment assmnt = null;
        OrdersSubmit ordersSubmit = null;
        Orders order = null;
        OrdersAssessment Orderassessment = null;
        ReferralOrder referalOrder = null;
        ReferralOrdersAssessment referalAssessment = null;
        Immunization Immunization = null;
        InHouseProcedure Inhouseprocedure = null;
        EAndMCoding SerProc = null;
        EandMCodingICD SerProcICD = null;
        Documents document = null;
        TreatmentPlan treatmentPlan = null;
        CarePlan careplan = null;
        PreventiveScreen prevPlan = null;
        GeneralNotes generalNotes = null;
        Rcopia_Allergy rcopiaAllergy = null;
        Rcopia_Medication rcopiamedication = null;
        Rcopia_Prescription_List rcopiaprescription = null;
        Test test = null;
        AddendumNotes Addendum = null;
        Human human = null;
        PotentialDiagnosis potentialDiagnosis = null;

        IList<string> ilstHumanXml = null;
        IEnumerable<PropertyInfo> propInfo = null;
        string id = "";
        public XmlDocument itemDoc = new XmlDocument();
        public string strXmlFilePath = "";

        public int iHumanBlobVersion = 0;
        public int iEncounterBlobVersion = 0;

        public ulong ulHumanID = 0;
        public ulong ulEncounterID = 0;

        public string sCreatedBy = string.Empty;
        public DateTime dtCreatedDateandTime = DateTime.MinValue;

        /*Added a bool bSave_In_Human in GenerateXml-Save,SaveStatic,Update and Copy_Previous_ to indicate that the list(Encounterlist) needs to be saved/updated in HUmanXML too.
         bSave_In_Human is set to "true" when EncounterTag is saved in HumanXML.*/
        public void GenerateXmlSaveStatic(IList<object> obj, ulong EncounterOrHumanId, string sGeneralNotesText, bool bSave_In_Human, GenerateXml XMLObj)
        {
            string sLocalTime = string.Empty;
            string FileName = "Encounter" + "_" + EncounterOrHumanId + ".xml";
            ulEncounterID = EncounterOrHumanId;
            if (obj.Count > 0)
            {
                if (ilstHumanXml == null)
                {
                    SetHumanXmlList();
                }
                string SourceName = obj[0].GetType().Name;
                string GeneralNotesList = SourceName + sGeneralNotesText + "List";

                if (ilstHumanXml.Contains(GeneralNotesList) || ilstHumanXml.Contains(SourceName))
                {
                    FileName = FileName.Replace("Encounter", "Human");
                    ulEncounterID = 0;
                    ulHumanID = EncounterOrHumanId;
                }
                if (bSave_In_Human)
                {
                    FileName = FileName.Replace("Encounter", "Human");
                    ulEncounterID = 0;
                    ulHumanID = EncounterOrHumanId;
                }
            }
            //XmlTextReader XmlText = null;
            //strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            try
            {

                if (XMLObj.itemDoc != null && XMLObj.itemDoc.InnerXml != "")
                {
                    itemDoc = XMLObj.itemDoc;
                }
                else
                {
                    if (FileName.Contains("Human"))
                    {
                        itemDoc = ReadBlob("Human", EncounterOrHumanId);
                    }
                    else if (FileName.Contains("Encounter"))
                    {
                        itemDoc = ReadBlob("Encounter", EncounterOrHumanId);
                    }
                }
                //  if (File.Exists(strXmlFilePath) == true)
                if (itemDoc != null && itemDoc.OuterXml != string.Empty)
                {
                    //string itemDoc_URI = itemDoc.BaseURI;
                    //if (itemDoc_URI == string.Empty || (Path.GetFileName(strXmlFilePath) != Path.GetFileName(itemDoc_URI)))
                    //{
                    //itemDoc = new XmlDocument();
                    //XmlText = new XmlTextReader(strXmlFilePath);
                    //itemDoc.Load(XmlText);
                    //XmlText.Close();
                    //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //{
                    //    itemDoc.Load(fs);
                    //}
                    if (obj[0].GetType().Name == "Encounter")
                    {
                        GetXmlNodeID(obj[0]);
                        XmlNodeList EncounterLocalTime = itemDoc.GetElementsByTagName(obj[0].GetType().Name);
                        for (int i = 0; i < EncounterLocalTime.Count; i++)
                        {
                            if (EncounterLocalTime[i].Attributes.GetNamedItem("Id").Value == id)
                            {
                                sLocalTime = EncounterLocalTime[i].Attributes.GetNamedItem("Local_Time").Value;
                            }
                        }
                    }
                    //}

                    XmlAttribute attlabel = null;
                    for (int k = 0; k < obj.Count; k++)
                    {
                        if (k == 0)
                        {
                            XmlNodeList xmlList = null;
                            if (sGeneralNotesText != string.Empty)
                            {
                                xmlList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                            }
                            else
                            {
                                xmlList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + "List");
                            }

                            XmlNode NewnodeList = null;
                            if (xmlList.Count == 0)
                            {
                                if (sGeneralNotesText != string.Empty)
                                {
                                    NewnodeList = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name + sGeneralNotesText + "List", "");
                                }
                                else
                                {
                                    NewnodeList = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name + "List", "");
                                }
                                XmlNodeList xmlModule = itemDoc.GetElementsByTagName("Modules");
                                xmlModule[0].AppendChild(NewnodeList);
                            }
                        }
                        XmlNode Newnode = null;
                        Newnode = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name, "");

                        SetObject(obj[k]);

                        foreach (PropertyInfo property in propInfo)
                        {
                            if (property.Name.Contains("Internal_Property") != true && property.PropertyType.Name.Contains("IList") != true)
                            {
                                attlabel = itemDoc.CreateAttribute(property.Name);

                                switch (obj[0].GetType().Name)
                                {
                                    case "Encounter":
                                        {
                                            if (sLocalTime != string.Empty && attlabel.Name.ToUpper() == "LOCAL_TIME")
                                            {
                                                attlabel.Value = sLocalTime;
                                            }
                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(encounter, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(encounter, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PatientInsuredPlan":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(patInsurance, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(patInsurance, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ProblemList":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ProblemLst, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ProblemLst, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ChiefComplaints":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(cc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(cc, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Healthcare_Questionnaire":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(questionnaire, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(questionnaire, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Test":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(test, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(test, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PastMedicalHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(psfhMedHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(psfhMedHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PastMedicalHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(psfhMedHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(psfhMedHistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SocialHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(socialhistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(socialhistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SocialHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(socialhistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(socialhistoryMaster, null).ToString();
                                            }
                                            break;
                                        }

                                    case "SurgicalHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SurHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SurHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SurgicalHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SurHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SurHistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(FamilyHis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(FamilyHis, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyDisease":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(familyDisease, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(familyDisease, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(FamilyHisMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(FamilyHisMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyDiseaseMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(familyDiseaseMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(familyDiseaseMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FileManagementIndex":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(filemanagementindex, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(filemanagementindex, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ImmunizationHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ImmunHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ImmunHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ImmunizationMasterHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ImmunMasterHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ImmunMasterHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "NonDrugAllergy":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(nondrugAllery, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(nondrugAllery, null).ToString();
                                            }
                                            break;
                                        }
                                    case "NonDrugAllergyMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(nondrugAlleryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(nondrugAlleryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "AdvanceDirective":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ad, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ad, null).ToString();
                                            }
                                            break;
                                        }
                                    case "AdvanceDirectiveMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(adMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(adMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PhysicianPatient":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(physicianpatient, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(physicianpatient, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PhysicianPatientMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(physicianpatientmaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(physicianpatientmaster, null).ToString();
                                            }
                                            break;
                                        }

                                    case "HospitalizationHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(HospHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(HospHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "HospitalizationHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(HospHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(HospHistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ROS":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ros, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ros, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PatientResults":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(patResults, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(patResults, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Examination":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(exam, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(exam, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Assessment":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(assmnt, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(assmnt, null).ToString();
                                            }
                                            break;
                                        }
                                    case "OrdersSubmit":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ordersSubmit, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ordersSubmit, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Orders":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(order, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(order, null).ToString();
                                            }
                                            break;
                                        }
                                    case "OrdersAssessment":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Orderassessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Orderassessment, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ReferralOrder":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(referalOrder, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(referalOrder, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ReferralOrdersAssessment":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(referalAssessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(referalAssessment, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Immunization":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Immunization, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Immunization, null).ToString();
                                            }
                                            break;
                                        }
                                    case "InHouseProcedure":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Inhouseprocedure, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Inhouseprocedure, null).ToString();
                                            }
                                            break;
                                        }
                                    case "EAndMCoding":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SerProc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SerProc, null).ToString();
                                            }
                                            break;
                                        }
                                    case "EandMCodingICD":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SerProcICD, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SerProcICD, null).ToString();
                                            }
                                            break;
                                        }
                                    case "TreatmentPlan":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(treatmentPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(treatmentPlan, null).ToString();
                                            }
                                            break;
                                        }
                                    case "CarePlan":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(careplan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(careplan, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Documents":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(document, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(document, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PreventiveScreen":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(prevPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(prevPlan, null).ToString();
                                            }
                                            break;
                                        }
                                    case "GeneralNotes":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(generalNotes, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(generalNotes, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Rcopia_Allergy":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiaAllergy, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(rcopiaAllergy, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Rcopia_Medication":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiamedication, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(rcopiamedication, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Rcopia_Prescription_List":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiaprescription, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(rcopiaprescription, null).ToString();
                                            }
                                            break;
                                        }
                                    case "AddendumNotes":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Addendum, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Addendum, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Human":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(human, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(human, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PotentialDiagnosis":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(potentialDiagnosis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(potentialDiagnosis, null).ToString();
                                            }
                                            break;
                                        }
                                }
                                Newnode.Attributes.Append(attlabel);
                            }
                        }

                        XmlNodeList xmlSectionList = null;
                        if (sGeneralNotesText != string.Empty)
                        {
                            xmlSectionList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                        }
                        else
                        {
                            xmlSectionList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + "List");
                        }
                        xmlSectionList[0].AppendChild(Newnode);
                    }
                }
            }
            catch (Exception Ex)
            {
                //if (XmlText != null)
                //    XmlText.Close();

                throw Ex;
            }
        }

        public void GenerateXmlSaveRCopia(IList<object> obj, ulong EncounterOrHumanId, string sGeneralNotesText, bool bSave_In_Human, bool IsPhoneEncounter, bool IsAssessment, bool IsRcopiaMedication)
        {
            string sLocalTime = string.Empty;
            string FileName = "Encounter" + "_" + EncounterOrHumanId + ".xml";
            ulEncounterID = EncounterOrHumanId;
            if (obj.Count > 0)
            {
                if (ilstHumanXml == null)
                {
                    SetHumanXmlList();
                }

                string SourceName = obj[0].GetType().Name;
                string GeneralNotesList = SourceName + sGeneralNotesText + "List";
                if (ilstHumanXml.Contains(GeneralNotesList) || ilstHumanXml.Contains(SourceName))
                {
                    FileName = FileName.Replace("Encounter", "Human");
                    ulEncounterID = 0;
                    ulHumanID = EncounterOrHumanId;
                }
                if (bSave_In_Human)
                {
                    FileName = FileName.Replace("Encounter", "Human");
                    ulEncounterID = 0;
                    ulHumanID = EncounterOrHumanId;

                }
            }
            //XmlTextReader XmlText = null;
            //strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            try
            {
                if (FileName.Contains("Human"))
                {
                    itemDoc = ReadBlob("Human", EncounterOrHumanId);
                }
                else if (FileName.Contains("Encounter"))
                {
                    itemDoc = ReadBlob("Encounter", EncounterOrHumanId);
                }
                //  if (File.Exists(strXmlFilePath) == true)
                if (itemDoc != null && itemDoc.OuterXml != string.Empty)
                {
                    //string itemDoc_URI = itemDoc.BaseURI;
                    //if (itemDoc_URI == string.Empty || (Path.GetFileName(strXmlFilePath) != Path.GetFileName(itemDoc_URI)))
                    //{
                    //    itemDoc = new XmlDocument();
                    //    XmlText = new XmlTextReader(strXmlFilePath);
                    //    itemDoc.Load(XmlText);
                    //    XmlText.Close();
                    //    //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //    //{
                    //    //    itemDoc.Load(fs);
                    //    //}
                    //}
                    if (obj[0].GetType().Name == "Encounter")
                    {
                        GetXmlNodeID(obj[0]);
                        XmlNodeList EncounterLocalTime = itemDoc.GetElementsByTagName(obj[0].GetType().Name);
                        for (int i = 0; i < EncounterLocalTime.Count; i++)
                        {
                            if (EncounterLocalTime[i].Attributes.GetNamedItem("Id").Value == id)
                            {
                                sLocalTime = EncounterLocalTime[i].Attributes.GetNamedItem("Local_Time").Value;
                            }
                        }
                    }


                    XmlAttribute attlabel = null;
                    for (int k = 0; k < obj.Count; k++)
                    {
                        if (k == 0)
                        {
                            XmlNodeList xmlRemove = null;
                            if (sGeneralNotesText != string.Empty)
                            {
                                xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                                for (int l = 0; l < xmlRemove.Count; l++)
                                {
                                    if (xmlRemove[l].ParentNode.Name == obj[k].GetType().Name + sGeneralNotesText + "List")
                                    {
                                        xmlRemove[l].ParentNode.RemoveAll();
                                    }
                                }
                            }
                            else
                            {
                                //if (!bSave_In_Human)
                                //{
                                xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                                if (xmlRemove.Count > 0)
                                {
                                    xmlRemove[0].ParentNode.RemoveAll();
                                }
                                //}
                                //else
                                //{
                                //    GetXmlNodeID(obj[k]);
                                //    xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                                //    for (int i = 0; i < xmlRemove.Count; i++)
                                //    {
                                //        //for (int c = 0; c < xmlRemove[i].Attributes.Count; c++)
                                //        //{
                                //        //if (xmlRemove[i].Attributes[c].Value.ToUpper() == id)
                                //        //{
                                //        //    xmlRemove[0].ParentNode.RemoveChild(xmlRemove[i]);
                                //        //    break;
                                //        //}

                                //        //}
                                //        if (xmlRemove[i].Attributes.GetNamedItem("Id").Value.ToUpper() == id)
                                //        {
                                //            xmlRemove[0].ParentNode.RemoveChild(xmlRemove[i]);
                                //            break;
                                //        }
                                //    }
                                //}
                            }
                            XmlNodeList xmlList = null;
                            if (sGeneralNotesText != string.Empty)
                            {
                                xmlList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                            }
                            else
                            {
                                xmlList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + "List");
                            }

                            XmlNode NewnodeList = null;
                            if (xmlList.Count == 0)
                            {

                                if (sGeneralNotesText != string.Empty)
                                {
                                    NewnodeList = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name + sGeneralNotesText + "List", "");
                                }
                                else
                                {
                                    NewnodeList = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name + "List", "");
                                }
                                XmlNodeList xmlModule = itemDoc.GetElementsByTagName("Modules");
                                xmlModule[0].AppendChild(NewnodeList);
                            }
                        }
                        XmlNode Newnode = null;
                        Newnode = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name, "");
                        #region SwitchCase
                        SetObject(obj[k]);

                        foreach (PropertyInfo property in propInfo)
                        {
                            if (property.Name.Contains("Internal_Property") != true && property.PropertyType.Name.Contains("IList") != true)
                            {
                                attlabel = itemDoc.CreateAttribute(property.Name);

                                switch (obj[0].GetType().Name)
                                {
                                    case "Encounter":
                                        {
                                            if (sLocalTime != string.Empty && attlabel.Name.ToUpper() == "LOCAL_TIME")
                                            {
                                                attlabel.Value = sLocalTime;
                                            }
                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(encounter, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(encounter, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PatientInsuredPlan":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(patInsurance, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(patInsurance, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ProblemList":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ProblemLst, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ProblemLst, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ChiefComplaints":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(cc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(cc, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Healthcare_Questionnaire":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(questionnaire, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(questionnaire, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Test":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(test, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(test, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PastMedicalHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(psfhMedHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(psfhMedHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PastMedicalHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(psfhMedHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(psfhMedHistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SocialHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(socialhistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(socialhistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SocialHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(socialhistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(socialhistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SurgicalHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SurHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SurHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SurgicalHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SurHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SurHistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(FamilyHis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(FamilyHis, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyDisease":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(familyDisease, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(familyDisease, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(FamilyHisMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(FamilyHisMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyDiseaseMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(familyDiseaseMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(familyDiseaseMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FileManagementIndex":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(filemanagementindex, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(filemanagementindex, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ImmunizationHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ImmunHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ImmunHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ImmunizationMasterHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ImmunMasterHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ImmunMasterHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "NonDrugAllergy":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(nondrugAllery, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(nondrugAllery, null).ToString();
                                            }
                                            break;
                                        }
                                    case "NonDrugAllergyMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(nondrugAlleryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(nondrugAlleryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "AdvanceDirective":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ad, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ad, null).ToString();
                                            }
                                            break;
                                        }
                                    case "AdvanceDirectiveMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(adMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(adMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PhysicianPatient":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(physicianpatient, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(physicianpatient, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PhysicianPatientMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(physicianpatientmaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(physicianpatientmaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "HospitalizationHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(HospHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(HospHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "HospitalizationHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(HospHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(HospHistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ROS":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ros, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ros, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PatientResults":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(patResults, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(patResults, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Examination":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(exam, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(exam, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Assessment":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(assmnt, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(assmnt, null).ToString();
                                            }
                                            break;
                                        }
                                    case "OrdersSubmit":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ordersSubmit, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ordersSubmit, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Orders":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(order, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(order, null).ToString();
                                            }
                                            break;
                                        }
                                    case "OrdersAssessment":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Orderassessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Orderassessment, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ReferralOrder":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(referalOrder, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(referalOrder, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ReferralOrdersAssessment":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(referalAssessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(referalAssessment, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Immunization":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Immunization, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Immunization, null).ToString();
                                            }
                                            break;
                                        }
                                    case "InHouseProcedure":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Inhouseprocedure, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Inhouseprocedure, null).ToString();
                                            }
                                            break;
                                        }
                                    case "EAndMCoding":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SerProc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SerProc, null).ToString();
                                            }
                                            break;
                                        }
                                    case "EandMCodingICD":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SerProcICD, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SerProcICD, null).ToString();
                                            }
                                            break;
                                        }
                                    case "TreatmentPlan":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(treatmentPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(treatmentPlan, null).ToString();
                                            }
                                            break;
                                        }
                                    case "CarePlan":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(careplan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(careplan, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Documents":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(document, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(document, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PreventiveScreen":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(prevPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(prevPlan, null).ToString();
                                            }
                                            break;
                                        }
                                    case "GeneralNotes":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(generalNotes, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(generalNotes, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Rcopia_Allergy":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiaAllergy, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(rcopiaAllergy, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Rcopia_Medication":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiamedication, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(rcopiamedication, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Rcopia_Prescription_List":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiaprescription, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(rcopiaprescription, null).ToString();
                                            }
                                            break;
                                        }
                                    case "AddendumNotes":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Addendum, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Addendum, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Human":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(human, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(human, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PotentialDiagnosis":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(potentialDiagnosis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(potentialDiagnosis, null).ToString();
                                            }
                                            break;
                                        }
                                }
                                Newnode.Attributes.Append(attlabel);
                            }
                        }
                        #endregion
                        XmlNodeList xmlSectionList = null;
                        if (sGeneralNotesText != string.Empty)
                        {
                            xmlSectionList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                        }
                        else
                        {
                            xmlSectionList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + "List");
                        }
                        xmlSectionList[0].AppendChild(Newnode);
                    }
                }
            }
            catch (Exception Ex)
            {
                //if (XmlText != null)
                //    XmlText.Close();
                throw Ex;
            }
            if (IsPhoneEncounter == true || IsAssessment == true || IsRcopiaMedication == true)
            {
                //itemDoc.Save(strXmlFilePath);
            }
        }

        public void GenerateXmlSave(IList<object> obj, ulong EncounterOrHumanId, string sGeneralNotesText, bool bSave_In_Human, bool IsPhoneEncounter, bool IsAssessment, bool IsRcopiaMedication, GenerateXml XMLObj)
        {
            string sLocalTime = string.Empty;
            string FileName = "Encounter" + "_" + EncounterOrHumanId + ".xml";
            ulEncounterID = EncounterOrHumanId;
            XmlTextReader XmlText = null;
            if (obj.Count > 0)
            {
                if (ilstHumanXml == null)
                {
                    SetHumanXmlList();
                }

                string SourceName = obj[0].GetType().Name;
                string GeneralNotesList = SourceName + sGeneralNotesText + "List";
                if (ilstHumanXml.Contains(GeneralNotesList) || ilstHumanXml.Contains(SourceName))
                {
                    FileName = FileName.Replace("Encounter", "Human");
                    ulEncounterID = 0;
                    ulHumanID = EncounterOrHumanId;
                }
                if (bSave_In_Human)
                {
                    FileName = FileName.Replace("Encounter", "Human");
                    ulEncounterID = 0;
                    ulHumanID = EncounterOrHumanId;
                }
            }
            // strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            try
            {
                if (XMLObj.itemDoc != null && XMLObj.itemDoc.InnerXml != "")
                {
                    itemDoc = XMLObj.itemDoc;
                }
                else
                {
                    if (FileName.Contains("Human"))
                    {
                        itemDoc = ReadBlob("Human", EncounterOrHumanId);
                    }
                    else if (FileName.Contains("Encounter"))
                    {
                        itemDoc = ReadBlob("Encounter", EncounterOrHumanId);
                    }
                }
                //  if (File.Exists(strXmlFilePath) == true)
                if (FileName.Contains("Human"))
                {
                    if (itemDoc.OuterXml == string.Empty)
                    {
                        string HumanFileName = "Human" + "_" + EncounterOrHumanId + ".xml";
                        string strXmlHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFileName);

                        string sDirectoryPath = System.Web.HttpContext.Current.Server.MapPath("Template_XML");
                        string sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                        //XmlDocument itemDoc = new XmlDocument();
                        XmlTextReader XmlTextTemp = new XmlTextReader(sXmlPath);
                        itemDoc.Load(XmlTextTemp);
                        XmlNodeList xmlnode = itemDoc.GetElementsByTagName("EncounterDetails");
                        xmlnode[0].ParentNode.RemoveChild(xmlnode[0]);
                        XmlTextTemp.Close();

                        //itemDoc = new XmlDocument();
                        //XmlText = new XmlTextReader(strXmlFilePath);
                        //itemDoc.Load(XmlText);
                        //XmlText.Close();
                        //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        //{
                        //    itemDoc.Load(fs);
                        //}
                    }
                }
                if (itemDoc != null && itemDoc.OuterXml != string.Empty)
                {
                    //string itemDoc_URI = itemDoc.BaseURI;
                    //if (itemDoc_URI == string.Empty || (Path.GetFileName(strXmlFilePath) != Path.GetFileName(itemDoc_URI)))
                    //{
                    //    itemDoc = new XmlDocument();
                    //    XmlText = new XmlTextReader(strXmlFilePath);
                    //    itemDoc.Load(XmlText);
                    //    XmlText.Close();
                    //    //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //    //{
                    //    //    itemDoc.Load(fs);
                    //    //}
                    //}
                    if (obj[0].GetType().Name == "Encounter")
                    {
                        GetXmlNodeID(obj[0]);
                        XmlNodeList EncounterLocalTime = itemDoc.GetElementsByTagName(obj[0].GetType().Name);
                        for (int i = 0; i < EncounterLocalTime.Count; i++)
                        {
                            if (EncounterLocalTime[i].Attributes.GetNamedItem("Id").Value == id)
                            {
                                sLocalTime = EncounterLocalTime[i].Attributes.GetNamedItem("Local_Time").Value;
                            }
                        }
                    }


                    XmlAttribute attlabel = null;
                    for (int k = 0; k < obj.Count; k++)
                    {
                        if (k == 0)
                        {
                            XmlNodeList xmlRemove = null;
                            if (sGeneralNotesText != string.Empty)
                            {
                                xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                                for (int l = 0; l < xmlRemove.Count; l++)
                                {
                                    if (xmlRemove[l].ParentNode.Name == obj[k].GetType().Name + sGeneralNotesText + "List")
                                    {
                                        xmlRemove[l].ParentNode.RemoveAll();
                                    }
                                }
                            }
                            else
                            {
                                if (!bSave_In_Human)
                                {
                                    xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                                    if (xmlRemove.Count > 0)
                                    {
                                        xmlRemove[0].ParentNode.RemoveAll();
                                    }
                                }
                                else
                                {
                                    GetXmlNodeID(obj[k]);
                                    xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                                    for (int i = 0; i < xmlRemove.Count; i++)
                                    {
                                        //for (int c = 0; c < xmlRemove[i].Attributes.Count; c++)
                                        //{
                                        //if (xmlRemove[i].Attributes[c].Value.ToUpper() == id)
                                        //{
                                        //    xmlRemove[0].ParentNode.RemoveChild(xmlRemove[i]);
                                        //    break;
                                        //}

                                        //}
                                        if (xmlRemove[i].Attributes.GetNamedItem("Id").Value.ToUpper() == id)
                                        {
                                            xmlRemove[0].ParentNode.RemoveChild(xmlRemove[i]);
                                            break;
                                        }
                                    }
                                }
                            }
                            XmlNodeList xmlList = null;
                            if (sGeneralNotesText != string.Empty)
                            {
                                xmlList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                            }
                            else
                            {
                                xmlList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + "List");
                            }

                            XmlNode NewnodeList = null;
                            if (xmlList.Count == 0)
                            {

                                if (sGeneralNotesText != string.Empty)
                                {
                                    NewnodeList = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name + sGeneralNotesText + "List", "");
                                }
                                else
                                {
                                    NewnodeList = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name + "List", "");
                                }
                                XmlNodeList xmlModule = itemDoc.GetElementsByTagName("Modules");
                                xmlModule[0].AppendChild(NewnodeList);
                            }
                        }
                        XmlNode Newnode = null;
                        Newnode = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name, "");
                        #region SwitchCase
                        SetObject(obj[k]);

                        foreach (PropertyInfo property in propInfo)
                        {
                            if (property.Name.Contains("Internal_Property") != true && property.PropertyType.Name.Contains("IList") != true)
                            {
                                attlabel = itemDoc.CreateAttribute(property.Name);

                                switch (obj[0].GetType().Name)
                                {
                                    case "Encounter":
                                        {
                                            if (sLocalTime != string.Empty && attlabel.Name.ToUpper() == "LOCAL_TIME")
                                            {
                                                attlabel.Value = sLocalTime;
                                            }
                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(encounter, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(encounter, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PatientInsuredPlan":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(patInsurance, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(patInsurance, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ProblemList":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ProblemLst, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ProblemLst, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ChiefComplaints":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(cc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(cc, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Healthcare_Questionnaire":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(questionnaire, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(questionnaire, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Test":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(test, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(test, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PastMedicalHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(psfhMedHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(psfhMedHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PastMedicalHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(psfhMedHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(psfhMedHistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SocialHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(socialhistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(socialhistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SocialHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(socialhistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(socialhistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SurgicalHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SurHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SurHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "SurgicalHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SurHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SurHistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(FamilyHis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(FamilyHis, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyDisease":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(familyDisease, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(familyDisease, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(FamilyHisMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(FamilyHisMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FamilyDiseaseMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(familyDiseaseMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(familyDiseaseMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "FileManagementIndex":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(filemanagementindex, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(filemanagementindex, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ImmunizationHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ImmunHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ImmunHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ImmunizationMasterHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ImmunMasterHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ImmunMasterHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "NonDrugAllergy":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(nondrugAllery, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(nondrugAllery, null).ToString();
                                            }
                                            break;
                                        }
                                    case "NonDrugAllergyMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(nondrugAlleryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(nondrugAlleryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "AdvanceDirective":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ad, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ad, null).ToString();
                                            }
                                            break;
                                        }
                                    case "AdvanceDirectiveMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(adMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(adMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PhysicianPatient":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(physicianpatient, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(physicianpatient, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PhysicianPatientMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(physicianpatientmaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(physicianpatientmaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "HospitalizationHistory":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(HospHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(HospHistory, null).ToString();
                                            }
                                            break;
                                        }
                                    case "HospitalizationHistoryMaster":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(HospHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(HospHistoryMaster, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ROS":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ros, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ros, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PatientResults":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(patResults, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(patResults, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Examination":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(exam, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(exam, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Assessment":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(assmnt, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(assmnt, null).ToString();
                                            }
                                            break;
                                        }
                                    case "OrdersSubmit":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(ordersSubmit, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(ordersSubmit, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Orders":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(order, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(order, null).ToString();
                                            }
                                            break;
                                        }
                                    case "OrdersAssessment":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Orderassessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Orderassessment, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ReferralOrder":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(referalOrder, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(referalOrder, null).ToString();
                                            }
                                            break;
                                        }
                                    case "ReferralOrdersAssessment":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(referalAssessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(referalAssessment, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Immunization":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Immunization, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Immunization, null).ToString();
                                            }
                                            break;
                                        }
                                    case "InHouseProcedure":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Inhouseprocedure, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Inhouseprocedure, null).ToString();
                                            }
                                            break;
                                        }
                                    case "EAndMCoding":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SerProc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SerProc, null).ToString();
                                            }
                                            break;
                                        }
                                    case "EandMCodingICD":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(SerProcICD, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(SerProcICD, null).ToString();
                                            }
                                            break;
                                        }
                                    case "TreatmentPlan":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(treatmentPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(treatmentPlan, null).ToString();
                                            }
                                            break;
                                        }
                                    case "CarePlan":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(careplan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(careplan, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Documents":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(document, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(document, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PreventiveScreen":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(prevPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(prevPlan, null).ToString();
                                            }
                                            break;
                                        }
                                    case "GeneralNotes":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(generalNotes, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(generalNotes, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Rcopia_Allergy":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiaAllergy, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(rcopiaAllergy, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Rcopia_Medication":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiamedication, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(rcopiamedication, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Rcopia_Prescription_List":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiaprescription, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(rcopiaprescription, null).ToString();
                                            }
                                            break;
                                        }
                                    case "AddendumNotes":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(Addendum, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(Addendum, null).ToString();
                                            }
                                            break;
                                        }
                                    case "Human":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(human, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(human, null).ToString();
                                            }
                                            break;
                                        }
                                    case "PotentialDiagnosis":
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                            {
                                                attlabel.Value = Convert.ToDateTime(property.GetValue(potentialDiagnosis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                            }
                                            else
                                            {
                                                attlabel.Value = property.GetValue(potentialDiagnosis, null).ToString();
                                            }
                                            break;
                                        }
                                }
                                Newnode.Attributes.Append(attlabel);
                            }
                        }
                        #endregion
                        XmlNodeList xmlSectionList = null;
                        if (sGeneralNotesText != string.Empty)
                        {
                            xmlSectionList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                        }
                        else
                        {
                            xmlSectionList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + "List");
                        }
                        xmlSectionList[0].AppendChild(Newnode);
                    }
                }
            }
            catch (Exception Ex)
            {
                if (XmlText != null)
                    XmlText.Close();

                throw Ex;
            }
            if (IsPhoneEncounter == true || IsAssessment == true || IsRcopiaMedication == true)
            {

                //itemDoc.Save(strXmlFilePath);
            }
        }


        public void GenerateXmlUpdate(IList<object> obj, ulong EncounterOrHumanId, string sGeneralNotesText, bool bSave_In_Human, GenerateXml XMLObj)
        {

            string sLocalTime = string.Empty;
            string FileName = "Encounter" + "_" + EncounterOrHumanId + ".xml";
            ulEncounterID = EncounterOrHumanId;
            XmlTextReader XmlText = null;
            if (obj.Count > 0)
            {
                if (ilstHumanXml == null)
                {
                    SetHumanXmlList();
                }
                string SourceName = obj[0].GetType().Name;
                string GeneralNotesList = SourceName + sGeneralNotesText + "List";
                if (ilstHumanXml.Contains(GeneralNotesList) || ilstHumanXml.Contains(SourceName))
                {
                    FileName = FileName.Replace("Encounter", "Human");
                    ulEncounterID = 0;
                    ulHumanID = EncounterOrHumanId;
                }
                if (bSave_In_Human)
                {
                    FileName = FileName.Replace("Encounter", "Human");
                    ulEncounterID = 0;
                    ulHumanID = EncounterOrHumanId;
                }
            }
            //strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            try
            {

                if (XMLObj.itemDoc != null && XMLObj.itemDoc.InnerXml != "")
                {
                    itemDoc = XMLObj.itemDoc;
                }
                else
                {
                    if (FileName.Contains("Human"))
                    {
                        itemDoc = ReadBlob("Human", EncounterOrHumanId);
                    }
                    else if (FileName.Contains("Encounter"))
                    {
                        itemDoc = ReadBlob("Encounter", EncounterOrHumanId);
                    }
                }
                //  if (File.Exists(strXmlFilePath) == true)
                if (itemDoc != null && itemDoc.OuterXml != string.Empty)
                {
                    //string itemDoc_URI = itemDoc.BaseURI;
                    //if (itemDoc_URI == string.Empty || (Path.GetFileName(strXmlFilePath) != Path.GetFileName(itemDoc_URI)))
                    //{
                    //itemDoc = new XmlDocument();
                    //XmlText = new XmlTextReader(strXmlFilePath);
                    //itemDoc.Load(XmlText);
                    //XmlText.Close();
                    //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //{
                    //    itemDoc.Load(fs);
                    //}
                    if (obj[0].GetType().Name == "Encounter")
                    {
                        GetXmlNodeID(obj[0]);
                        XmlNodeList EncounterLocalTime = itemDoc.GetElementsByTagName(obj[0].GetType().Name);
                        for (int i = 0; i < EncounterLocalTime.Count; i++)
                        {
                            if (EncounterLocalTime[i].Attributes.GetNamedItem("Id").Value == id)
                            {
                                sLocalTime = EncounterLocalTime[i].Attributes.GetNamedItem("Local_Time").Value;
                            }
                        }
                    }
                    //}
                    for (int k = 0; k < obj.Count; k++)
                    {
                        XmlNodeList xmlRemove = null;
                        if (sGeneralNotesText == string.Empty)
                        {
                            GetXmlNodeID(obj[k]);
                            xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                            for (int n = 0; n < xmlRemove.Count; n++)
                            {
                                //for (int c = 0; c < xmlRemove[n].Attributes.Count; c++)
                                //{
                                //if (xmlRemove[n].Attributes[c].Value.ToUpper() == id)
                                if (xmlRemove[n].Attributes.GetNamedItem("Id").Value.ToUpper() == id)
                                {
                                    xmlRemove[n].RemoveAll();
                                    XmlAttribute att = null;
                                    foreach (PropertyInfo property in propInfo)
                                    {
                                        if (property.Name.Contains("Internal_Property") != true && property.PropertyType.Name.Contains("IList") != true)
                                        {
                                            att = itemDoc.CreateAttribute(property.Name);
                                            switch (obj[0].GetType().Name)
                                            {
                                                case "Encounter":
                                                    {
                                                        if (sLocalTime != string.Empty && att.Name.ToUpper() == "LOCAL_TIME")
                                                        {
                                                            att.Value = sLocalTime;
                                                        }
                                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(encounter, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(encounter, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "PatientInsuredPlan":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(patInsurance, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(patInsurance, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "ProblemList":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(ProblemLst, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(ProblemLst, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "ChiefComplaints":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(cc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(cc, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Healthcare_Questionnaire":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(questionnaire, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(questionnaire, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Test":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(test, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(test, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "PastMedicalHistory":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(psfhMedHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(psfhMedHistory, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "PastMedicalHistoryMaster":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(psfhMedHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(psfhMedHistoryMaster, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "SocialHistory":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(socialhistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(socialhistory, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "SocialHistoryMaster":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(socialhistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(socialhistoryMaster, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "SurgicalHistory":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(SurHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(SurHistory, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "SurgicalHistoryMaster":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(SurHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(SurHistoryMaster, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "FamilyHistory":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(FamilyHis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(FamilyHis, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "FamilyDisease":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(familyDisease, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(familyDisease, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "FamilyHistoryMaster":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(FamilyHisMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(FamilyHisMaster, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "FamilyDiseaseMaster":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(familyDiseaseMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(familyDiseaseMaster, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "FileManagementIndex":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(filemanagementindex, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(filemanagementindex, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "ImmunizationHistory":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(ImmunHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(ImmunHistory, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "ImmunizationMasterHistory":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(ImmunMasterHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(ImmunMasterHistory, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "NonDrugAllergy":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(nondrugAllery, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(nondrugAllery, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "NonDrugAllergyMaster":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(nondrugAlleryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(nondrugAlleryMaster, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "AdvanceDirective":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(ad, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(ad, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "AdvanceDirectiveMaster":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(adMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(adMaster, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "PhysicianPatient":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(physicianpatient, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(physicianpatient, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "PhysicianPatientMaster":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(physicianpatientmaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(physicianpatientmaster, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "HospitalizationHistory":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(HospHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(HospHistory, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "HospitalizationHistoryMaster":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(HospHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(HospHistoryMaster, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "ROS":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(ros, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(ros, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "PatientResults":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(patResults, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(patResults, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Examination":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(exam, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(exam, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Assessment":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(assmnt, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(assmnt, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "OrdersSubmit":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(ordersSubmit, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(ordersSubmit, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Orders":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(order, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(order, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "OrdersAssessment":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(Orderassessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(Orderassessment, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "ReferralOrder":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(referalOrder, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(referalOrder, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "ReferralOrdersAssessment":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(referalAssessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(referalAssessment, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Immunization":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(Immunization, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(Immunization, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "InHouseProcedure":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(Inhouseprocedure, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(Inhouseprocedure, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "EAndMCoding":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(SerProc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(SerProc, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "EandMCodingICD":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(SerProcICD, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(SerProcICD, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "TreatmentPlan":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(treatmentPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(treatmentPlan, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "CarePlan":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(careplan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(careplan, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Documents":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(document, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(document, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "PreventiveScreen":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(prevPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(prevPlan, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "GeneralNotes":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(generalNotes, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(generalNotes, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Rcopia_Allergy":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(rcopiaAllergy, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(rcopiaAllergy, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Rcopia_Medication":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(rcopiamedication, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(rcopiamedication, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Rcopia_Prescription_List":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(rcopiaprescription, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(rcopiaprescription, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "AddendumNotes":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(Addendum, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(Addendum, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "Human":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(human, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(human, null).ToString();
                                                        }
                                                        break;
                                                    }
                                                case "PotentialDiagnosis":
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        {
                                                            att.Value = Convert.ToDateTime(property.GetValue(potentialDiagnosis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                        }
                                                        else
                                                        {
                                                            att.Value = property.GetValue(potentialDiagnosis, null).ToString();
                                                        }
                                                        break;
                                                    }
                                            }

                                            xmlRemove[n].Attributes.Append(att);
                                        }
                                    }
                                }
                                //}
                            }
                        }
                        else
                        {
                            GetXmlNodeID(obj[k]);
                            xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                            if (xmlRemove != null && xmlRemove[0] != null)
                            {
                                xmlRemove = xmlRemove[0].ChildNodes;
                            }
                            if (xmlRemove.Count > 0)
                            {
                                for (int n = 0; n < xmlRemove.Count; n++)
                                {
                                    //for (int c = 0; c < xmlRemove[n].Attributes.Count; c++)
                                    //{
                                    // if (xmlRemove[n].Attributes[c].Value.ToUpper() == id)
                                    if (xmlRemove[n].Attributes.GetNamedItem("Id").Value.ToUpper() == id)
                                    {
                                        XmlNode newnode = null;
                                        newnode = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name, "");
                                        XmlAttribute att = null;
                                        foreach (PropertyInfo property in propInfo)
                                        {
                                            if (property.Name.Contains("Internal_Property") != true && property.PropertyType.Name.Contains("IList") != true)
                                            {
                                                att = itemDoc.CreateAttribute(property.Name);
                                                switch (obj[0].GetType().Name)
                                                {
                                                    case "Encounter":
                                                        {
                                                            if (sLocalTime != string.Empty && att.Name.ToUpper() == "LOCAL_TIME")
                                                            {
                                                                att.Value = sLocalTime;
                                                            }
                                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(encounter, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(encounter, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "PatientInsuredPlan":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(patInsurance, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(patInsurance, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "ProblemList":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(ProblemLst, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(ProblemLst, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "ChiefComplaints":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(cc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(cc, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Healthcare_Questionnaire":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(questionnaire, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(questionnaire, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Test":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(test, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(test, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "PastMedicalHistory":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(psfhMedHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(psfhMedHistory, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "PastMedicalHistoryMaster":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(psfhMedHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(psfhMedHistoryMaster, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "SocialHistory":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(socialhistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(socialhistory, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "SocialHistoryMaster":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(socialhistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(socialhistoryMaster, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "SurgicalHistory":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(SurHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(SurHistory, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "SurgicalHistoryMaster":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(SurHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(SurHistoryMaster, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "FamilyHistory":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(FamilyHis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(FamilyHis, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "FamilyDisease":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(familyDisease, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(familyDisease, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "FamilyHistoryMaster":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(FamilyHisMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(FamilyHisMaster, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "FamilyDiseaseMaster":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(familyDiseaseMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(familyDiseaseMaster, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "FileManagementIndex":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(filemanagementindex, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(filemanagementindex, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "ImmunizationHistory":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(ImmunHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(ImmunHistory, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "ImmunizationMasterHistory":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(ImmunMasterHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(ImmunMasterHistory, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "NonDrugAllergy":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(nondrugAllery, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(nondrugAllery, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "NonDrugAllergyMaster":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(nondrugAlleryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(nondrugAlleryMaster, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "AdvanceDirective":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(ad, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(ad, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "AdvanceDirectiveMaster":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(adMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(adMaster, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "PhysicianPatient":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(physicianpatient, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(physicianpatient, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "PhysicianPatientMaster":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(physicianpatientmaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(physicianpatientmaster, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "HospitalizationHistory":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(HospHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(HospHistory, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "HospitalizationHistoryMaster":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(HospHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(HospHistoryMaster, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "ROS":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(ros, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(ros, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "PatientResults":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(patResults, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(patResults, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Examination":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(exam, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(exam, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Assessment":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(assmnt, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(assmnt, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "OrdersSubmit":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(ordersSubmit, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(ordersSubmit, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Orders":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(order, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(order, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "OrdersAssessment":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(Orderassessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(Orderassessment, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "ReferralOrder":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(referalOrder, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(referalOrder, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "ReferralOrdersAssessment":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(referalAssessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(referalAssessment, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Immunization":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(Immunization, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(Immunization, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "InHouseProcedure":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(Inhouseprocedure, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(Inhouseprocedure, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "EAndMCoding":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(SerProc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(SerProc, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "EandMCodingICD":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(SerProcICD, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(SerProcICD, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "TreatmentPlan":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(treatmentPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(treatmentPlan, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "CarePlan":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(careplan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(careplan, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Documents":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(document, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(document, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "PreventiveScreen":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(prevPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(prevPlan, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "GeneralNotes":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(generalNotes, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(generalNotes, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Rcopia_Allergy":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(rcopiaAllergy, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(rcopiaAllergy, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Rcopia_Medication":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(rcopiamedication, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(rcopiamedication, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Rcopia_Prescription_List":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(rcopiaprescription, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(rcopiaprescription, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "AddendumNotes":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(Addendum, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(Addendum, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "Human":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(human, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(human, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                    case "PotentialDiagnosis":
                                                        {
                                                            if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            {
                                                                att.Value = Convert.ToDateTime(property.GetValue(potentialDiagnosis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                            }
                                                            else
                                                            {
                                                                att.Value = property.GetValue(potentialDiagnosis, null).ToString();
                                                            }
                                                            break;
                                                        }
                                                }
                                                newnode.Attributes.Append(att);
                                            }
                                        }
                                        xmlRemove[0].ParentNode.RemoveChild(xmlRemove[n]);
                                        XmlNodeList xmlParentNode = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                                        xmlParentNode[0].AppendChild(newnode);
                                    }
                                    //}
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                if (XmlText != null)
                    XmlText.Close();

                throw Ex;


            }
        }

        public void GenerateXmlDelete(IList<object> ilstobj, ulong EncounterOrHumanId, string sGeneralNotesText, bool IsAssessment,GenerateXml XMLObj)
        {
            string FileName = "Encounter" + "_" + EncounterOrHumanId + ".xml";
            ulEncounterID = EncounterOrHumanId;
            if (ilstobj.Count > 0)
            {
                if (ilstHumanXml == null)
                {
                    SetHumanXmlList();
                }
                string SourceName = ilstobj[0].GetType().Name;
                string GeneralNotesList = SourceName + sGeneralNotesText + "List";
                if (ilstHumanXml.Contains(GeneralNotesList) || ilstHumanXml.Contains(SourceName))
                {
                    FileName = FileName.Replace("Encounter", "Human");
                    ulEncounterID = 0;
                    ulHumanID = EncounterOrHumanId;
                }
            }
            //XmlTextReader XmlText=null;

            //strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            try
            {
                if (XMLObj.itemDoc != null && XMLObj.itemDoc.InnerXml != "")
                {
                    itemDoc = XMLObj.itemDoc;
                }
                else
                {
                    if (FileName.Contains("Human"))
                    {
                        itemDoc = ReadBlob("Human", EncounterOrHumanId);
                    }
                    else if (FileName.Contains("Encounter"))
                    {
                        itemDoc = ReadBlob("Encounter", EncounterOrHumanId);
                    }
                }
                //  if (File.Exists(strXmlFilePath) == true)
                if (itemDoc != null && itemDoc.OuterXml != string.Empty)
                {
                    //string itemDoc_URI = itemDoc.BaseURI;
                    //if (itemDoc_URI == string.Empty || (Path.GetFileName(strXmlFilePath) != Path.GetFileName(itemDoc_URI)))
                    //{
                    //    itemDoc = new XmlDocument();
                    //    XmlText = new XmlTextReader(strXmlFilePath);
                    //    itemDoc.Load(XmlText);
                    //    XmlText.Close();
                    //    //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //    //{
                    //    //    itemDoc.Load(fs);
                    //    //}
                    //}
                    string sobjectName = ilstobj[0].GetType().Name;
                    if (sGeneralNotesText == string.Empty)
                    {
                        for (int k = 0; k < ilstobj.Count; k++)
                        {
                            GetXmlNodeID(ilstobj[k]);
                            XmlNodeList xmlRemove = itemDoc.GetElementsByTagName(ilstobj[k].GetType().Name);
                            if (xmlRemove != null && xmlRemove.Count > 0)
                            {
                                for (int n = 0; n < xmlRemove.Count; n++)
                                {
                                    //int iAttributeCount = xmlRemove[n].Attributes.Count;
                                    //for (int c = 0; c < iAttributeCount; c++)
                                    //{
                                    //  if (xmlRemove[n].Attributes[c].Name.ToUpper() == "ID" && xmlRemove[n].Attributes[c].Value.ToUpper() == id)
                                    if (xmlRemove[n].Attributes.GetNamedItem("Id").Value.ToUpper() == id)
                                    {
                                        xmlRemove[0].ParentNode.RemoveChild(xmlRemove[n]);
                                        break;
                                    }
                                    // }
                                }
                            }
                        }
                        XmlNodeList xmlRemoveParentList = itemDoc.GetElementsByTagName(ilstobj[0].GetType().Name);
                        if (xmlRemoveParentList.Count == 0)
                        {
                            XmlNodeList ParentNodeList = itemDoc.GetElementsByTagName(ilstobj[0].GetType().Name + "List");
                            XmlNodeList xmlModules = itemDoc.GetElementsByTagName("Modules");
                            if (ParentNodeList.Count > 0)
                                xmlModules[0].RemoveChild(ParentNodeList[0]);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < ilstobj.Count; k++)
                        {
                            GetXmlNodeID(ilstobj[k]);
                            XmlNodeList xmlRemove = itemDoc.GetElementsByTagName(ilstobj[k].GetType().Name + sGeneralNotesText);
                            if (xmlRemove != null)
                            {
                                for (int n = 0; n < xmlRemove.Count; n++)
                                {
                                    //int iAttributeCount = xmlRemove[n].Attributes.Count;
                                    //for (int c = 0; c < iAttributeCount; c++)
                                    //{
                                    //if (xmlRemove[n].Attributes[c].Name.ToUpper() == "ID" && xmlRemove[n].Attributes[c].Value.ToUpper() == id)
                                    if (xmlRemove[n].Attributes.GetNamedItem("Id").Value.ToUpper() == id)
                                    {
                                        xmlRemove[0].ParentNode.RemoveChild(xmlRemove[n]);
                                        break;
                                    }
                                    //}
                                    //break;
                                }
                            }
                        }
                        XmlNodeList xmlRemoveParentList = itemDoc.GetElementsByTagName(ilstobj[0].GetType().Name);
                        if (xmlRemoveParentList.Count == 0)
                        {
                            XmlNodeList ParentNodeList = itemDoc.GetElementsByTagName(ilstobj[0].GetType().Name + sGeneralNotesText + "List");
                            XmlNodeList xmlModules = itemDoc.GetElementsByTagName("Modules");
                            xmlModules[0].RemoveChild(ParentNodeList[0]);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //if (XmlText != null)
                //    XmlText.Close();

                throw Ex;
            }
            //if (IsAssessment == true)
            //{
            //    itemDoc.Save(strXmlFilePath);
            //}
        }

        public void SetObject(object obj)
        {
            switch (obj.GetType().Name)
            {
                case "Encounter":
                    {
                        encounter = (Encounter)obj;
                        propInfo = from obji in ((Encounter)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "PatientInsuredPlan":
                    {
                        patInsurance = (PatientInsuredPlan)obj;
                        propInfo = from obji in ((PatientInsuredPlan)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "ProblemList":
                    {
                        ProblemLst = (ProblemList)obj;
                        propInfo = from obji in ((ProblemList)obj).GetType().GetProperties() select obji;
                        break;
                    }

                case "ChiefComplaints":
                    {
                        cc = (ChiefComplaints)obj;
                        propInfo = from obji in ((ChiefComplaints)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Healthcare_Questionnaire":
                    {
                        questionnaire = (Healthcare_Questionnaire)obj;
                        propInfo = from obji in ((Healthcare_Questionnaire)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Test":
                    {
                        test = (Test)obj;
                        propInfo = from obji in ((Test)obj).GetType().GetProperties() select obji;
                        break;
                    }

                case "PastMedicalHistory":
                    {
                        psfhMedHistory = (PastMedicalHistory)obj;
                        propInfo = from obji in ((PastMedicalHistory)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "PastMedicalHistoryMaster":
                    {
                        psfhMedHistoryMaster = (PastMedicalHistoryMaster)obj;
                        propInfo = from obji in ((PastMedicalHistoryMaster)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "SocialHistory":
                    {
                        socialhistory = (SocialHistory)obj;
                        propInfo = from obji in ((SocialHistory)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "SocialHistoryMaster":
                    {
                        socialhistoryMaster = (SocialHistoryMaster)obj;
                        propInfo = from obji in ((SocialHistoryMaster)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "SurgicalHistory":
                    {
                        SurHistory = (SurgicalHistory)obj;
                        propInfo = from obji in ((SurgicalHistory)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "SurgicalHistoryMaster":
                    {
                        SurHistoryMaster = (SurgicalHistoryMaster)obj;
                        propInfo = from obji in ((SurgicalHistoryMaster)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "FamilyHistory":
                    {
                        FamilyHis = (FamilyHistory)obj;
                        propInfo = from obji in ((FamilyHistory)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "FileManagementIndex":
                    {
                        filemanagementindex = (FileManagementIndex)obj;
                        propInfo = from obji in ((FileManagementIndex)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "FamilyDisease":
                    {
                        familyDisease = (FamilyDisease)obj;
                        propInfo = from obji in ((FamilyDisease)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "FamilyHistoryMaster":
                    {
                        FamilyHisMaster = (FamilyHistoryMaster)obj;
                        propInfo = from obji in ((FamilyHistoryMaster)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "FamilyDiseaseMaster":
                    {
                        familyDiseaseMaster = (FamilyDiseaseMaster)obj;
                        propInfo = from obji in ((FamilyDiseaseMaster)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "ImmunizationHistory":
                    {
                        ImmunHistory = (ImmunizationHistory)obj;
                        propInfo = from obji in ((ImmunizationHistory)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "ImmunizationMasterHistory":
                    {
                        ImmunMasterHistory = (ImmunizationMasterHistory)obj;
                        propInfo = from obji in ((ImmunizationMasterHistory)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "NonDrugAllergy":
                    {
                        nondrugAllery = (NonDrugAllergy)obj;
                        propInfo = from obji in ((NonDrugAllergy)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "NonDrugAllergyMaster":
                    {
                        nondrugAlleryMaster = (NonDrugAllergyMaster)obj;
                        propInfo = from obji in ((NonDrugAllergyMaster)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "AdvanceDirective":
                    {
                        ad = (AdvanceDirective)obj;
                        propInfo = from obji in ((AdvanceDirective)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "AdvanceDirectiveMaster":
                    {
                        adMaster = (AdvanceDirectiveMaster)obj;
                        propInfo = from obji in ((AdvanceDirectiveMaster)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "PhysicianPatient":
                    {
                        physicianpatient = (PhysicianPatient)obj;
                        propInfo = from obji in ((PhysicianPatient)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "PhysicianPatientMaster":
                    {
                        physicianpatientmaster = (PhysicianPatientMaster)obj;
                        propInfo = from obji in ((PhysicianPatientMaster)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "HospitalizationHistory":
                    {
                        HospHistory = (HospitalizationHistory)obj;
                        propInfo = from obji in ((HospitalizationHistory)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "HospitalizationHistoryMaster":
                    {
                        HospHistoryMaster = (HospitalizationHistoryMaster)obj;
                        propInfo = from obji in ((HospitalizationHistoryMaster)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "ROS":
                    {
                        ros = (ROS)obj;
                        propInfo = from obji in ((ROS)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "PatientResults":
                    {
                        patResults = (PatientResults)obj;
                        propInfo = from obji in ((PatientResults)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Examination":
                    {
                        exam = (Examination)obj;
                        propInfo = from obji in ((Examination)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Assessment":
                    {
                        assmnt = (Assessment)obj;
                        propInfo = from obji in ((Assessment)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "OrdersSubmit":
                    {
                        ordersSubmit = (OrdersSubmit)obj;
                        propInfo = from obji in ((OrdersSubmit)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Orders":
                    {
                        order = (Orders)obj;
                        propInfo = from obji in ((Orders)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "OrdersAssessment":
                    {
                        Orderassessment = (OrdersAssessment)obj;
                        propInfo = from obji in ((OrdersAssessment)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "ReferralOrder":
                    {
                        referalOrder = (ReferralOrder)obj;
                        propInfo = from obji in ((ReferralOrder)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "ReferralOrdersAssessment":
                    {
                        referalAssessment = (ReferralOrdersAssessment)obj;
                        propInfo = from obji in ((ReferralOrdersAssessment)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Immunization":
                    {
                        Immunization = (Immunization)obj;
                        propInfo = from obji in ((Immunization)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "InHouseProcedure":
                    {
                        Inhouseprocedure = (InHouseProcedure)obj;
                        propInfo = from obji in ((InHouseProcedure)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "EAndMCoding":
                    {
                        SerProc = (EAndMCoding)obj;
                        propInfo = from obji in ((EAndMCoding)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "EandMCodingICD":
                    {
                        SerProcICD = (EandMCodingICD)obj;
                        propInfo = from obji in ((EandMCodingICD)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "TreatmentPlan":
                    {
                        treatmentPlan = (TreatmentPlan)obj;
                        propInfo = from obji in ((TreatmentPlan)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "CarePlan":
                    {
                        careplan = (CarePlan)obj;
                        propInfo = from obji in ((CarePlan)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Documents":
                    {
                        document = (Documents)obj;
                        propInfo = from obji in ((Documents)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "PreventiveScreen":
                    {
                        prevPlan = (PreventiveScreen)obj;
                        propInfo = from obji in ((PreventiveScreen)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "GeneralNotes":
                    {
                        generalNotes = (GeneralNotes)obj;
                        propInfo = from obji in ((GeneralNotes)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Rcopia_Allergy":
                    {
                        rcopiaAllergy = (Rcopia_Allergy)obj;
                        propInfo = from obji in ((Rcopia_Allergy)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Rcopia_Medication":
                    {
                        rcopiamedication = (Rcopia_Medication)obj;
                        propInfo = from obji in ((Rcopia_Medication)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Rcopia_Prescription_List":
                    {
                        rcopiaprescription = (Rcopia_Prescription_List)obj;
                        propInfo = from obji in ((Rcopia_Prescription_List)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "AddendumNotes":
                    {
                        Addendum = (AddendumNotes)obj;
                        propInfo = from obji in ((AddendumNotes)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "Human":
                    {
                        human = (Human)obj;
                        propInfo = from obji in ((Human)obj).GetType().GetProperties() select obji;
                        break;
                    }
                case "PotentialDiagnosis":
                    {
                        potentialDiagnosis = (PotentialDiagnosis)obj;
                        propInfo = from obji in ((PotentialDiagnosis)obj).GetType().GetProperties() select obji;
                        break;
                    }
            }
        }

        public void GetXmlNodeID(object obj)
        {
            SetObject(obj);
            foreach (PropertyInfo property in propInfo)
            {
                if (property.Name.ToUpper() == "ID")
                {
                    switch (obj.GetType().Name)
                    {
                        case "Encounter":
                            id = property.GetValue(encounter, null).ToString();
                            break;
                        case "PatientInsuredPlan":
                            id = property.GetValue(patInsurance, null).ToString();
                            break;
                        case "ProblemList":
                            id = property.GetValue(ProblemLst, null).ToString();
                            break;
                        case "ChiefComplaints":
                            id = property.GetValue(cc, null).ToString();
                            break;
                        case "Healthcare_Questionnaire":
                            id = property.GetValue(questionnaire, null).ToString();
                            break;
                        case "PastMedicalHistory":
                            id = property.GetValue(psfhMedHistory, null).ToString();
                            break;
                        case "PastMedicalHistoryMaster":
                            id = property.GetValue(psfhMedHistoryMaster, null).ToString();
                            break;
                        case "SocialHistory":
                            id = property.GetValue(socialhistory, null).ToString();
                            break;
                        case "SocialHistoryMaster":
                            id = property.GetValue(socialhistoryMaster, null).ToString();
                            break;
                        case "SurgicalHistory":
                            id = property.GetValue(SurHistory, null).ToString();
                            break;
                        case "SurgicalHistoryMaster":
                            id = property.GetValue(SurHistoryMaster, null).ToString();
                            break;
                        case "FamilyHistory":
                            id = property.GetValue(FamilyHis, null).ToString();
                            break;
                        case "FamilyDisease":
                            id = property.GetValue(familyDisease, null).ToString();
                            break;
                        case "FamilyHistoryMaster":
                            id = property.GetValue(FamilyHisMaster, null).ToString();
                            break;
                        case "FamilyDiseaseMaster":
                            id = property.GetValue(familyDiseaseMaster, null).ToString();
                            break;
                        case "FileManagementIndex":
                            id = property.GetValue(filemanagementindex, null).ToString();
                            break;
                        case "ImmunizationHistory":
                            id = property.GetValue(ImmunHistory, null).ToString();
                            break;
                        case "ImmunizationMasterHistory":
                            id = property.GetValue(ImmunMasterHistory, null).ToString();
                            break;
                        case "NonDrugAllergy":
                            id = property.GetValue(nondrugAllery, null).ToString();
                            break;
                        case "NonDrugAllergyMaster":
                            id = property.GetValue(nondrugAlleryMaster, null).ToString();
                            break;
                        case "AdvanceDirective":
                            id = property.GetValue(ad, null).ToString();
                            break;
                        case "AdvanceDirectiveMaster":
                            id = property.GetValue(adMaster, null).ToString();
                            break;
                        case "PhysicianPatient":
                            id = property.GetValue(physicianpatient, null).ToString();
                            break;
                        case "PhysicianPatientMaster":
                            id = property.GetValue(physicianpatientmaster, null).ToString();
                            break;
                        case "HospitalizationHistory":
                            id = property.GetValue(HospHistory, null).ToString();
                            break;
                        case "HospitalizationHistoryMaster":
                            id = property.GetValue(HospHistoryMaster, null).ToString();
                            break;
                        case "ROS":
                            id = property.GetValue(ros, null).ToString();
                            break;
                        case "PatientResults":
                            id = property.GetValue(patResults, null).ToString();
                            break;
                        case "Examination":
                            id = property.GetValue(exam, null).ToString();
                            break;
                        case "Assessment":
                            id = property.GetValue(assmnt, null).ToString();
                            break;
                        case "OrdersSubmit":
                            id = property.GetValue(ordersSubmit, null).ToString();
                            break;
                        case "Orders":
                            id = property.GetValue(order, null).ToString();
                            break;
                        case "OrdersAssessment":
                            id = property.GetValue(Orderassessment, null).ToString();
                            break;
                        case "ReferralOrder":
                            id = property.GetValue(referalOrder, null).ToString();
                            break;
                        case "ReferralOrdersAssessment":
                            id = property.GetValue(referalAssessment, null).ToString();
                            break;
                        case "Immunization":
                            id = property.GetValue(Immunization, null).ToString();
                            break;
                        case "InHouseProcedure":
                            id = property.GetValue(Inhouseprocedure, null).ToString();
                            break;
                        case "EAndMCoding":
                            id = property.GetValue(SerProc, null).ToString();
                            break;
                        case "EandMCodingICD":
                            id = property.GetValue(SerProcICD, null).ToString();
                            break;
                        case "TreatmentPlan":
                            id = property.GetValue(treatmentPlan, null).ToString();
                            break;
                        case "CarePlan":
                            id = property.GetValue(careplan, null).ToString();
                            break;
                        case "Documents":
                            id = property.GetValue(document, null).ToString();
                            break;
                        case "PreventiveScreen":
                            id = property.GetValue(prevPlan, null).ToString();
                            break;
                        case "GeneralNotes":
                            id = property.GetValue(generalNotes, null).ToString();
                            break;
                        case "Rcopia_Allergy":
                            id = property.GetValue(rcopiaAllergy, null).ToString();
                            break;
                        case "Rcopia_Medication":
                            id = property.GetValue(rcopiamedication, null).ToString();
                            break;
                        case "Rcopia_Prescription_List":
                            id = property.GetValue(rcopiaprescription, null).ToString();
                            break;
                        case "AddendumNotes":
                            id = property.GetValue(Addendum, null).ToString();
                            break;
                        case "Human":
                            id = property.GetValue(human, null).ToString();
                            break;
                        case "PotentialDiagnosis":
                            id = property.GetValue(potentialDiagnosis, null).ToString();
                            break;
                    }
                }
            }
        }

        public bool CheckDataConsistency(IList<object> lstExistingNodes, bool bIsSelectiveEntryCheck, string sGeneralNotesText)
        {
            string User_name = string.Empty;
            string human_id = string.Empty;
            string encounter_id = string.Empty;
            string physician_id = string.Empty;
            string time = string.Empty;
            Exception ex = null;
            bool bResult = true;
            try
            {
                if (lstExistingNodes != null && lstExistingNodes.Count > 0 && lstExistingNodes[0].GetType().Name.ToUpper() == "RCOPIA_MEDICATION_TEMP")
                {
                    XmlNodeList existing_nodes = null;
                    if (sGeneralNotesText.Trim() == string.Empty)
                    {
                        if (lstExistingNodes.Count > 0)
                            existing_nodes = itemDoc.GetElementsByTagName(lstExistingNodes[0].GetType().Name);
                    }
                    else
                    {
                        if (lstExistingNodes.Count > 0)
                        {
                            XmlNodeList parent_node = itemDoc.GetElementsByTagName(lstExistingNodes[0].GetType().Name + sGeneralNotesText + "List");
                            if (parent_node != null && parent_node.Count > 0)
                                existing_nodes = parent_node[0].ChildNodes;
                        }
                    }

                    bool bExistingNodeCountMatch = (existing_nodes == null && lstExistingNodes.Count == 0) || (existing_nodes != null && (lstExistingNodes.Count == existing_nodes.Count)) ? true : false;
                    bool bExistingNodesDataMatch = true;

                    //if (bIsSelectiveEntryCheck)
                    bExistingNodeCountMatch = true;
                    if (lstExistingNodes.Count > 0 && bExistingNodeCountMatch && existing_nodes != null && existing_nodes.Count > 0)
                    {
                        for (int iCount = 0; iCount < lstExistingNodes.Count && bExistingNodesDataMatch; iCount++)
                        {
                            string node_path = GetXPath(existing_nodes[0]) + @"[@Id='" + lstExistingNodes[iCount].GetType().GetProperty("Id").GetValue(lstExistingNodes[iCount], null) + @"']";
                            XmlNode matchingNode = itemDoc.SelectSingleNode(node_path);
                            if (matchingNode != null)
                            {
                                XmlAttributeCollection matchingnodeAttributes = matchingNode.Attributes;
                                for (int iIndex = 0; iIndex < matchingnodeAttributes.Count; iIndex++)
                                {
                                    IEnumerable<PropertyInfo> nodeAttributesProperties = propInfo.Where(prop => prop.Name.ToUpper() == lstExistingNodes[iCount].GetType().GetProperty(matchingnodeAttributes[iIndex].Name).Name.ToUpper()).ToList<PropertyInfo>();
                                    string DB_Attr_Value = "";
                                    if (nodeAttributesProperties != null)
                                    {
                                        if (nodeAttributesProperties.First().PropertyType.Name.ToString().ToUpper() != "DATETIME")
                                            DB_Attr_Value = lstExistingNodes[iCount].GetType().GetProperty(matchingnodeAttributes[iIndex].Name).GetValue(lstExistingNodes[iCount], null).ToString();
                                        else
                                            DB_Attr_Value = Convert.ToDateTime(lstExistingNodes[iCount].GetType().GetProperty(matchingnodeAttributes[iIndex].Name).GetValue(lstExistingNodes[iCount], null).ToString()).ToString("yyyy-MM-dd hh:mm:ss tt");
                                    }
                                    else
                                    {

                                        bExistingNodesDataMatch = false;
                                        string class_name = "";
                                        if (lstExistingNodes.Count > 0)
                                        {
                                            time = DateTime.Now.ToString() + " . UTC TIME: " + DateTime.UtcNow.ToString() + System.Environment.NewLine;
                                            User_name = lstExistingNodes[0].GetType().GetProperty("Created_By").GetValue(lstExistingNodes[0], null).ToString() == "" ? lstExistingNodes[0].GetType().GetProperty("Modified_By").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Created_By").GetValue(lstExistingNodes[0], null).ToString();
                                            human_id = lstExistingNodes[0].GetType().GetProperty("Human_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Human_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Human_ID").GetValue(lstExistingNodes[0], null).ToString();
                                            encounter_id = lstExistingNodes[0].GetType().GetProperty("Encounter_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Encounter_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Encounter_ID").GetValue(lstExistingNodes[0], null).ToString();
                                            physician_id = lstExistingNodes[0].GetType().GetProperty("Physician_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Physician_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Physician_ID").GetValue(lstExistingNodes[0], null).ToString();
                                            class_name = lstExistingNodes[0].GetType().Name;
                                        }
                                        string exceptionDetails = System.Environment.NewLine + System.Environment.NewLine + "------------------------------BEGINNING OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                                        exceptionDetails += "MESSAGE : Data inconsistency detected while saving. Please try again or notify support." + System.Environment.NewLine + "TIME :" + time + System.Environment.NewLine + "CURRENT USER :" + User_name + System.Environment.NewLine + "HUMAN ID :" + human_id + System.Environment.NewLine + "ENCOUNTER ID :" + encounter_id + System.Environment.NewLine + "PHYSICIAN ID :" + physician_id + System.Environment.NewLine + "CLASS NAME :" + class_name + System.Environment.NewLine;
                                        exceptionDetails += "Attribute value :" + lstExistingNodes[iCount].GetType().GetProperty(matchingnodeAttributes[iIndex].Name).Name.ToUpper() + " missing in XML for " + node_path.Split('/')[4];
                                        exceptionDetails += System.Environment.NewLine + System.Environment.NewLine + "------------------------------END OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                                        //logInconsistency.Error(exceptionDetails, ex);
                                        break;
                                    }
                                    if (matchingnodeAttributes[iIndex].Value.ToUpper() != DB_Attr_Value.ToUpper())
                                    {
                                        bExistingNodesDataMatch = false;
                                        bResult = false;
                                        string class_name = "";
                                        if (lstExistingNodes.Count > 0)
                                        {
                                            time = DateTime.Now.ToString() + " . UTC TIME: " + DateTime.UtcNow.ToString() + System.Environment.NewLine;
                                            User_name = lstExistingNodes[0].GetType().GetProperty("Created_By").GetValue(lstExistingNodes[0], null).ToString() == "" ? lstExistingNodes[0].GetType().GetProperty("Modified_By").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Created_By").GetValue(lstExistingNodes[0], null).ToString();
                                            human_id = lstExistingNodes[0].GetType().GetProperty("Human_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Human_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Human_ID").GetValue(lstExistingNodes[0], null).ToString();
                                            encounter_id = lstExistingNodes[0].GetType().GetProperty("Encounter_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Encounter_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Encounter_ID").GetValue(lstExistingNodes[0], null).ToString();
                                            physician_id = lstExistingNodes[0].GetType().GetProperty("Physician_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Physician_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Physician_ID").GetValue(lstExistingNodes[0], null).ToString();
                                            class_name = lstExistingNodes[0].GetType().Name;
                                        }
                                        string exceptionDetails = System.Environment.NewLine + System.Environment.NewLine + "------------------------------BEGINNING OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                                        exceptionDetails += "MESSAGE : Data inconsistency detected while saving. Please try again or notify support." + System.Environment.NewLine + "TIME :" + time + System.Environment.NewLine + "CURRENT USER :" + User_name + System.Environment.NewLine + "HUMAN ID :" + human_id + System.Environment.NewLine + "ENCOUNTER ID :" + encounter_id + System.Environment.NewLine + "PHYSICIAN ID :" + physician_id + System.Environment.NewLine + "CLASS NAME :" + class_name + System.Environment.NewLine;
                                        exceptionDetails += "Attribute value :" + lstExistingNodes[iCount].GetType().GetProperty(matchingnodeAttributes[iIndex].Name).Name.ToUpper() + " mismatch in XML for " + node_path.Split('/')[4] + System.Environment.NewLine + "DB VALUE :" + DB_Attr_Value.ToUpper() + System.Environment.NewLine + "XML VALUE :" + matchingnodeAttributes[iIndex].Value.ToUpper();
                                        exceptionDetails += System.Environment.NewLine + System.Environment.NewLine + "------------------------------END OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                                        //logInconsistency.Error(exceptionDetails, ex);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                bResult = false;
                                bExistingNodesDataMatch = false;
                                string class_name = "";
                                if (lstExistingNodes.Count > 0)
                                {
                                    time = DateTime.Now.ToString() + " . UTC TIME: " + DateTime.UtcNow.ToString() + System.Environment.NewLine;
                                    User_name = lstExistingNodes[0].GetType().GetProperty("Created_By").GetValue(lstExistingNodes[0], null).ToString() == "" ? lstExistingNodes[0].GetType().GetProperty("Modified_By").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Created_By").GetValue(lstExistingNodes[0], null).ToString();
                                    human_id = lstExistingNodes[0].GetType().GetProperty("Human_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Human_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Human_ID").GetValue(lstExistingNodes[0], null).ToString();
                                    encounter_id = lstExistingNodes[0].GetType().GetProperty("Encounter_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Encounter_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Encounter_ID").GetValue(lstExistingNodes[0], null).ToString();
                                    physician_id = lstExistingNodes[0].GetType().GetProperty("Physician_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Physician_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Physician_ID").GetValue(lstExistingNodes[0], null).ToString();
                                    class_name = lstExistingNodes[0].GetType().Name;
                                }
                                string exceptionDetails = System.Environment.NewLine + System.Environment.NewLine + "------------------------------BEGINNING OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                                exceptionDetails += "MESSAGE : Data inconsistency detected while saving. Please try again or notify support." + System.Environment.NewLine + "TIME :" + time + System.Environment.NewLine + "CURRENT USER :" + User_name + System.Environment.NewLine + "HUMAN ID :" + human_id + System.Environment.NewLine + "ENCOUNTER ID :" + encounter_id + System.Environment.NewLine + "PHYSICIAN ID :" + physician_id + System.Environment.NewLine + "CLASS NAME :" + class_name + System.Environment.NewLine;
                                exceptionDetails += "Matching Node not found in XML for " + node_path.Split('/')[4];
                                exceptionDetails += System.Environment.NewLine + System.Environment.NewLine + "------------------------------END OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                                //logInconsistency.Error(exceptionDetails, ex);
                                break;
                            }
                        }
                    }
                }
                //return bExistingNodeCountMatch && bExistingNodesDataMatch;
                return bResult;
            }
            catch
            {
                //string class_name = "";
                //if (lstExistingNodes.Count > 0)
                //{
                //    time = DateTime.Now.ToString() + " . UTC TIME: " + DateTime.UtcNow.ToString() + System.Environment.NewLine;
                //    User_name = lstExistingNodes[0].GetType().GetProperty("Created_By").GetValue(lstExistingNodes[0], null).ToString() == "" ? lstExistingNodes[0].GetType().GetProperty("Modified_By").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Created_By").GetValue(lstExistingNodes[0], null).ToString();
                //    human_id = lstExistingNodes[0].GetType().GetProperty("Human_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Human_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Human_ID").GetValue(lstExistingNodes[0], null).ToString();
                //    encounter_id = lstExistingNodes[0].GetType().GetProperty("Encounter_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Encounter_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Encounter_ID").GetValue(lstExistingNodes[0], null).ToString();
                //    physician_id = lstExistingNodes[0].GetType().GetProperty("Physician_ID") == null ? lstExistingNodes[0].GetType().GetProperty("Physician_Id").GetValue(lstExistingNodes[0], null).ToString() : lstExistingNodes[0].GetType().GetProperty("Physician_ID").GetValue(lstExistingNodes[0], null).ToString();
                //    class_name = lstExistingNodes[0].GetType().Name;
                //}
                //string exceptionDetails = System.Environment.NewLine + System.Environment.NewLine + "------------------------------BEGINNING OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                //exceptionDetails += "MESSAGE : Data inconsistency detected while saving. Please try again or notify support." + System.Environment.NewLine + "TIME :" + time + System.Environment.NewLine + "CURRENT USER :" + User_name + System.Environment.NewLine + "HUMAN ID :" + human_id + System.Environment.NewLine + "ENCOUNTER ID :" + encounter_id + System.Environment.NewLine + "PHYSICIAN ID :" + physician_id + System.Environment.NewLine + "CLASS NAME :" + class_name + System.Environment.NewLine;
                //exceptionDetails += System.Environment.NewLine + System.Environment.NewLine + "------------------------------END OF THIS EXCEPTION------------------------------------" + System.Environment.NewLine + System.Environment.NewLine;
                //logInconsistency.Error(exceptionDetails, e);
                //return true;
                return bResult;
            }
            // return true;
            return bResult;
        }

        private void SetHumanXmlList()
        {
            ilstHumanXml = new List<string>();
            ilstHumanXml.Add("GeneralNotesFamilyHistoryList");
            ilstHumanXml.Add("ProblemList");
            ilstHumanXml.Add("PastMedicalHistory");
            ilstHumanXml.Add("PastMedicalHistoryMaster");
            ilstHumanXml.Add("SocialHistory");
            ilstHumanXml.Add("SocialHistoryMaster");
            ilstHumanXml.Add("SurgicalHistory");
            ilstHumanXml.Add("SurgicalHistoryMaster");
            ilstHumanXml.Add("FamilyHistory");
            ilstHumanXml.Add("FamilyDisease");
            ilstHumanXml.Add("FamilyHistoryMaster");
            ilstHumanXml.Add("FamilyDiseaseMaster");
            ilstHumanXml.Add("ImmunizationHistory");
            ilstHumanXml.Add("ImmunizationMasterHistory");
            ilstHumanXml.Add("NonDrugAllergy");
            ilstHumanXml.Add("NonDrugAllergyMaster");
            ilstHumanXml.Add("AdvanceDirective");
            ilstHumanXml.Add("AdvanceDirectiveMaster");
            ilstHumanXml.Add("HospitalizationHistory");
            ilstHumanXml.Add("HospitalizationHistoryMaster");
            ilstHumanXml.Add("GeneralNotesPastMedicalHistoryList");
            ilstHumanXml.Add("Immunization");
            ilstHumanXml.Add("GeneralNotesNonDrugAllergyList");
            ilstHumanXml.Add("Rcopia_Allergy");
            ilstHumanXml.Add("Rcopia_Prescription_List");
            ilstHumanXml.Add("Rcopia_Medication");
            ilstHumanXml.Add("PatientResults");
            ilstHumanXml.Add("InHouseProcedure");
            ilstHumanXml.Add("GeneralNotesSocialHistoryList");
            ilstHumanXml.Add("PhysicianPatient");
            ilstHumanXml.Add("PhysicianPatientMaster");
            ilstHumanXml.Add("PatientResultsList");
            ilstHumanXml.Add("PatientInsuredPlan");
            ilstHumanXml.Add("ReferralOrder");
            ilstHumanXml.Add("ReferralOrdersAssessment");
            ilstHumanXml.Add("Orders");
            ilstHumanXml.Add("OrdersSubmit");
            ilstHumanXml.Add("OrdersAssessment");
            ilstHumanXml.Add("FileManagementIndex");
            ilstHumanXml.Add("Human");
            ilstHumanXml.Add("PotentialDiagnosis");
        }

        private string GetXPath(XmlNode passedNode)
        {
            XmlNode currentNode = passedNode;
            string xPath = currentNode.Name;
            while (currentNode.ParentNode != null && currentNode.ParentNode.GetType().Name != "XmlDocument")
            {
                xPath = currentNode.ParentNode.Name + @"/" + xPath;
                currentNode = currentNode.ParentNode;
            }
            return @"/" + xPath;
        }
        public void Copy_Previous_GenerateXmlSave(IList<object> obj, ulong EncounterOrHumanId, string sGeneralNotesText, bool bSave_In_Human, ref GenerateXml XMLObj)
        {
            if (obj == null || obj.Count == 0)//BugID:4779
                return;
            string sLocalTime = string.Empty;
            string FileName = "Encounter" + "_" + EncounterOrHumanId + ".xml";
            ulEncounterID = EncounterOrHumanId;

            if (ilstHumanXml == null)
            {
                SetHumanXmlList();
            }

            string SourceName = obj[0].GetType().Name;
            string GeneralNotesList = SourceName + sGeneralNotesText + "List";
            if (ilstHumanXml.Contains(GeneralNotesList) || ilstHumanXml.Contains(SourceName))
            {
                FileName = FileName.Replace("Encounter", "Human");
                ulEncounterID = 0;
                ulHumanID = EncounterOrHumanId;
            }
            if (bSave_In_Human)
            {
                FileName = FileName.Replace("Encounter", "Human");
                ulEncounterID = 0;
                ulHumanID = EncounterOrHumanId;
            }
            if (XMLObj.itemDoc != null && XMLObj.itemDoc.InnerXml != "")
            {
                itemDoc = XMLObj.itemDoc;
            }
            else
            {
                if (FileName.Contains("Human"))
                {
                    itemDoc = ReadBlob("Human", EncounterOrHumanId);
                }
                else if (FileName.Contains("Encounter"))
                {
                    itemDoc = ReadBlob("Encounter", EncounterOrHumanId);
                }
            }
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            // if (File.Exists(strXmlFilePath) == true)
            {
                //XmlDocument itemDoc = new XmlDocument();
                //XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //itemDoc.Load(XmlText);
                //XmlText.Close();
                //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //    itemDoc.Load(fs);
                //}

                if (obj[0].GetType().Name == "Encounter")
                {
                    GetXmlNodeID(obj[0]);
                    XmlNodeList EncounterLocalTime = itemDoc.GetElementsByTagName(obj[0].GetType().Name);
                    for (int i = 0; i < EncounterLocalTime.Count; i++)
                    {
                        if (EncounterLocalTime[i].Attributes.GetNamedItem("Id").Value == id)
                        {
                            sLocalTime = EncounterLocalTime[i].Attributes.GetNamedItem("Local_Time").Value;
                        }
                    }
                }

                XmlAttribute attlabel = null;
                for (int k = 0; k < obj.Count; k++)
                {
                    if (k == 0)
                    {
                        XmlNodeList xmlRemove = null;
                        if (sGeneralNotesText != string.Empty)
                        {
                            xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                            for (int l = 0; l < xmlRemove.Count; l++)
                            {
                                if (xmlRemove[l].ParentNode.Name == obj[k].GetType().Name + sGeneralNotesText + "List")
                                {
                                    xmlRemove[l].ParentNode.RemoveAll();
                                }
                            }
                        }
                        else
                        {
                            if (!bSave_In_Human && obj[0].GetType().Name.ToUpper() != "TREATMENTPLAN")
                            {
                                xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                                if (xmlRemove.Count > 0)
                                {
                                    xmlRemove[0].ParentNode.RemoveAll();
                                }
                            }
                            else
                            {
                                GetXmlNodeID(obj[k]);
                                xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                                for (int i = 0; i < xmlRemove.Count; i++)
                                {
                                    //for (int c = 0; c < xmlRemove[i].Attributes.Count; c++)
                                    //{
                                    //if (xmlRemove[i].Attributes[c].Value.ToUpper() == id)
                                    if (xmlRemove[i].Attributes.GetNamedItem("Id").Value.ToUpper() == id)
                                    {
                                        xmlRemove[0].ParentNode.RemoveChild(xmlRemove[i]);
                                        break;
                                    }
                                    // }
                                }
                            }
                        }
                        /*else
                         {
                             xmlRemove = itemDoc.GetElementsByTagName(obj[k].GetType().Name);
                             if (xmlRemove.Count > 0)
                             {
                                 xmlRemove[0].ParentNode.RemoveAll();
                             }
                         }*/
                        XmlNodeList xmlList = null;
                        if (sGeneralNotesText != string.Empty)
                        {
                            xmlList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                        }
                        else
                        {
                            xmlList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + "List");
                        }

                        XmlNode NewnodeList = null;
                        if (xmlList.Count == 0)
                        {

                            if (sGeneralNotesText != string.Empty)
                            {
                                NewnodeList = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name + sGeneralNotesText + "List", "");
                            }
                            else
                            {
                                NewnodeList = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name + "List", "");
                            }
                            XmlNodeList xmlModule = itemDoc.GetElementsByTagName("Modules");
                            xmlModule[0].AppendChild(NewnodeList);
                        }
                    }
                    XmlNode Newnode = null;
                    Newnode = itemDoc.CreateNode(XmlNodeType.Element, obj[k].GetType().Name, "");

                    SetObject(obj[k]);

                    foreach (PropertyInfo property in propInfo)
                    {
                        if (property.Name.Contains("Internal_Property") != true && property.PropertyType.Name.Contains("IList") != true)
                        {
                            attlabel = itemDoc.CreateAttribute(property.Name);

                            switch (obj[0].GetType().Name)
                            {
                                case "Encounter":
                                    {
                                        if (sLocalTime != string.Empty && attlabel.Name.ToUpper() == "LOCAL_TIME")
                                        {
                                            attlabel.Value = sLocalTime;
                                        }
                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(encounter, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(encounter, null).ToString();
                                        }
                                        break;
                                    }
                                case "PatientInsuredPlan":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(patInsurance, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(patInsurance, null).ToString();
                                        }
                                        break;
                                    }
                                case "ProblemList":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(ProblemLst, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(ProblemLst, null).ToString();
                                        }
                                        break;
                                    }
                                case "ChiefComplaints":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(cc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(cc, null).ToString();
                                        }
                                        break;
                                    }
                                case "Healthcare_Questionnaire":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(questionnaire, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(questionnaire, null).ToString();
                                        }
                                        break;
                                    }
                                case "Test":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(test, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(test, null).ToString();
                                        }
                                        break;
                                    }
                                case "PastMedicalHistory":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(psfhMedHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(psfhMedHistory, null).ToString();
                                        }
                                        break;
                                    }
                                case "PastMedicalHistoryMaster":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(psfhMedHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(psfhMedHistoryMaster, null).ToString();
                                        }
                                        break;
                                    }
                                case "SocialHistory":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(socialhistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(socialhistory, null).ToString();
                                        }
                                        break;
                                    }
                                case "SocialHistoryMaster":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(socialhistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(socialhistoryMaster, null).ToString();
                                        }
                                        break;
                                    }
                                case "SurgicalHistory":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(SurHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(SurHistory, null).ToString();
                                        }
                                        break;
                                    }
                                case "SurgicalHistoryMaster":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(SurHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(SurHistoryMaster, null).ToString();
                                        }
                                        break;
                                    }
                                case "FamilyHistory":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(FamilyHis, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(FamilyHis, null).ToString();
                                        }
                                        break;
                                    }
                                case "FamilyDisease":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(familyDisease, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(familyDisease, null).ToString();
                                        }
                                        break;
                                    }
                                case "FamilyHistoryMaster":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(FamilyHisMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(FamilyHisMaster, null).ToString();
                                        }
                                        break;
                                    }
                                case "FamilyDiseaseMaster":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(familyDiseaseMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(familyDiseaseMaster, null).ToString();
                                        }
                                        break;
                                    }
                                case "FileManagementIndex":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(filemanagementindex, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(filemanagementindex, null).ToString();
                                        }
                                        break;
                                    }
                                case "ImmunizationHistory":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(ImmunHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(ImmunHistory, null).ToString();
                                        }
                                        break;
                                    }
                                case "ImmunizationMasterHistory":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(ImmunMasterHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(ImmunMasterHistory, null).ToString();
                                        }
                                        break;
                                    }
                                case "NonDrugAllergy":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(nondrugAllery, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(nondrugAllery, null).ToString();
                                        }
                                        break;
                                    }
                                case "NonDrugAllergyMaster":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(nondrugAlleryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(nondrugAlleryMaster, null).ToString();
                                        }
                                        break;
                                    }
                                case "AdvanceDirective":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(ad, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(ad, null).ToString();
                                        }
                                        break;
                                    }
                                case "AdvanceDirectiveMaster":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(adMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(adMaster, null).ToString();
                                        }
                                        break;
                                    }
                                case "PhysicianPatient":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(physicianpatient, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(physicianpatient, null).ToString();
                                        }
                                        break;
                                    }
                                case "PhysicianPatientMaster":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(physicianpatientmaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(physicianpatientmaster, null).ToString();
                                        }
                                        break;
                                    }
                                case "HospitalizationHistory":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(HospHistory, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(HospHistory, null).ToString();
                                        }
                                        break;
                                    }
                                case "HospitalizationHistoryMaster":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(HospHistoryMaster, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(HospHistoryMaster, null).ToString();
                                        }
                                        break;
                                    }
                                case "ROS":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(ros, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(ros, null).ToString();
                                        }
                                        break;
                                    }
                                case "PatientResults":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(patResults, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(patResults, null).ToString();
                                        }
                                        break;
                                    }
                                case "Examination":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(exam, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(exam, null).ToString();
                                        }
                                        break;
                                    }
                                case "Assessment":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(assmnt, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(assmnt, null).ToString();
                                        }
                                        break;
                                    }
                                case "OrdersSubmit":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(ordersSubmit, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(ordersSubmit, null).ToString();
                                        }
                                        break;
                                    }
                                case "Orders":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(order, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(order, null).ToString();
                                        }
                                        break;
                                    }
                                case "OrdersAssessment":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(Orderassessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(Orderassessment, null).ToString();
                                        }
                                        break;
                                    }
                                case "ReferralOrder":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(referalOrder, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(referalOrder, null).ToString();
                                        }
                                        break;
                                    }
                                case "ReferralOrdersAssessment":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(referalAssessment, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(referalAssessment, null).ToString();
                                        }
                                        break;
                                    }
                                case "Immunization":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(Immunization, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(Immunization, null).ToString();
                                        }
                                        break;
                                    }
                                case "InHouseProcedure":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(Inhouseprocedure, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(Inhouseprocedure, null).ToString();
                                        }
                                        break;
                                    }
                                case "EAndMCoding":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(SerProc, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(SerProc, null).ToString();
                                        }
                                        break;
                                    }
                                case "EandMCodingICD":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(SerProcICD, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(SerProcICD, null).ToString();
                                        }
                                        break;
                                    }
                                case "TreatmentPlan":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(treatmentPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(treatmentPlan, null).ToString();
                                        }
                                        break;
                                    }
                                case "CarePlan":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(careplan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(careplan, null).ToString();
                                        }
                                        break;
                                    }
                                case "Documents":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(document, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(document, null).ToString();
                                        }
                                        break;
                                    }
                                case "PreventiveScreen":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(prevPlan, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(prevPlan, null).ToString();
                                        }
                                        break;
                                    }
                                case "GeneralNotes":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(generalNotes, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(generalNotes, null).ToString();
                                        }
                                        break;
                                    }
                                case "Rcopia_Allergy":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiaAllergy, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(rcopiaAllergy, null).ToString();
                                        }
                                        break;
                                    }
                                case "Rcopia_Medication":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiamedication, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(rcopiamedication, null).ToString();
                                        }
                                        break;
                                    }
                                case "Rcopia_Prescription_List":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiaprescription, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(rcopiaprescription, null).ToString();
                                        }
                                        break;
                                    }
                                case "AddendumNotes":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(Addendum, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(Addendum, null).ToString();
                                        }
                                        break;
                                    }
                                case "Human":
                                    {
                                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                        {
                                            attlabel.Value = Convert.ToDateTime(property.GetValue(human, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        }
                                        else
                                        {
                                            attlabel.Value = property.GetValue(human, null).ToString();
                                        }
                                        break;
                                    }
                            }
                            Newnode.Attributes.Append(attlabel);
                        }
                    }
                    XmlNodeList xmlSectionList = null;
                    if (sGeneralNotesText != string.Empty)
                    {
                        xmlSectionList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + sGeneralNotesText + "List");
                    }
                    else
                    {
                        xmlSectionList = itemDoc.GetElementsByTagName(obj[k].GetType().Name + "List");
                    }
                    xmlSectionList[0].AppendChild(Newnode);
                }
                //itemDoc.Save(strXmlFilePath);
            }
        }


        public XmlDocument ReadBlob(string sXMLType, ulong EntityID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string sXMLContent = String.Empty;

            if (sXMLType == "Human")
            {
                string query = @"select * from blob_human where human_id=" + EntityID;
                DataSet dsReturn = DBConnector.ReadData(query);
                DataTable dt = dsReturn.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    sXMLContent = System.Text.Encoding.UTF8.GetString((byte[])dt.Rows[0]["Human_XML"]);
                    if (sXMLContent.Substring(0, 1) != "<")
                        sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                    xmlDoc.LoadXml(sXMLContent);
                    if (dt.Rows[0]["Version"] != null && dt.Rows[0]["Version"].ToString() != string.Empty)
                        iHumanBlobVersion = Convert.ToInt32(dt.Rows[0]["Version"]);
                    else
                        iHumanBlobVersion = 0;
                    if (dt.Rows[0]["Created_By"] != null)
                        sCreatedBy = Convert.ToString(dt.Rows[0]["Created_By"]);
                    else
                        sCreatedBy = string.Empty;
                    if (dt.Rows[0]["Created_Date_And_Time"] != null && dt.Rows[0]["Created_Date_And_Time"].ToString() != string.Empty)
                        dtCreatedDateandTime = Convert.ToDateTime(dt.Rows[0]["Created_Date_And_Time"].ToString());
                    else
                        dtCreatedDateandTime = DateTime.MinValue;
                }
            }
            else if (sXMLType == "Encounter")
            {

                string query = @"select * from blob_encounter where encounter_id=" + EntityID;
                DataSet dsReturn = DBConnector.ReadData(query);
                DataTable dt = dsReturn.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    sXMLContent = System.Text.Encoding.UTF8.GetString((byte[])dt.Rows[0]["Encounter_XML"]);
                    if (sXMLContent.Substring(0, 1) != "<")
                        sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                    xmlDoc.LoadXml(sXMLContent);
                    //iEncounterBlobVersion = Convert.ToInt32(dt.Rows[0]["Version"]);
                    //sCreatedBy = (string)dt.Rows[0]["Created_By"];
                    //dtCreatedDateandTime = Convert.ToDateTime(dt.Rows[0]["Created_Date_And_Time"].ToString());
                    if (dt.Rows[0]["Version"] != null && dt.Rows[0]["Version"].ToString() != string.Empty)
                        iEncounterBlobVersion = Convert.ToInt32(dt.Rows[0]["Version"]);
                    else
                        iEncounterBlobVersion = 0;
                    if (dt.Rows[0]["Created_By"] != null)
                        sCreatedBy = Convert.ToString(dt.Rows[0]["Created_By"]);
                    else
                        sCreatedBy = string.Empty;
                    if (dt.Rows[0]["Created_Date_And_Time"] != null && dt.Rows[0]["Created_Date_And_Time"].ToString() != string.Empty)
                        dtCreatedDateandTime = Convert.ToDateTime(dt.Rows[0]["Created_Date_And_Time"].ToString());
                    else
                        dtCreatedDateandTime = DateTime.MinValue;
                }
            }

            return xmlDoc;
        }




    }

    public static class DBConnector
    {
        static MySqlDataAdapter MyDataAdap = null;
        private static string ReadConnection()
        {
            string ConnectionData;
            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            return ConnectionData;
        }
        public static DataSet ReadData(string Query)
        {
            DataSet dsReturn = new DataSet();
            MyDataAdap = new MySqlDataAdapter(Query, ReadConnection());
            MyDataAdap.Fill(dsReturn);
            return dsReturn;
        }

        public static int WriteData(string Query)
        {
            int iReturn = 0;
            using (MySqlConnection con = new MySqlConnection(ReadConnection()))
            {
                using (MySqlCommand cmd = new MySqlCommand(Query))
                {
                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        iReturn = 1;
                    }
                    catch
                    {
                        iReturn = 2;
                    }
                }
            }
            return iReturn;
        }
    }
}