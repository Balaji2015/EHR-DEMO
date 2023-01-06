using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading;


namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public interface IPastMedicalHistoryManager : IManagerBase<PastMedicalHistory, uint>
    {
        //ProblemHistoryDTO SaveUpdateDeleteProblemMedicalHistory(IList<PastMedicalHistory> insertList, IList<PastMedicalHistory> updateList, IList<PastMedicalHistory> deleteList, GeneralNotes generalNotesObject, string macAddress, ulong Encounter_ID, ulong Human_Id, ulong Physician_ID, string UserName, DateTime date, string medicalInfo, string Version_Yr, Dictionary<string, string> Data);
        ProblemHistoryDTO SaveUpdateDeletePastMedicalHistory(IList<PastMedicalHistory> SavePastMedicalList, IList<PastMedicalHistory> UpdatePastMedicalList, IList<PastMedicalHistory> DeletePastMedicalList, IList<ProblemList> SaveProblemList, IList<ProblemList> UpdateProblemList, IList<GeneralNotes> SaveGeneralNotes, IList<GeneralNotes> UpdateGeneralNotes, string macAddress, ulong HumanId, IList<PastMedicalHistoryMaster> SaveListMaster, IList<PastMedicalHistoryMaster> UpdateListMaster, IList<PastMedicalHistoryMaster> DeleteListMaster);
    }
    public partial class PastMedicalHistoryManager : ManagerBase<PastMedicalHistory, uint>, IPastMedicalHistoryManager
    {
        #region Constructors

        public PastMedicalHistoryManager()
            : base()
        {

        }
        public PastMedicalHistoryManager(INHibernateSession session)
            : base(session)
        {

        }
        #endregion
        int iTryCount = 0;
        //public ProblemHistoryDTO SaveUpdateDeleteProblemMedicalHistory(IList<PastMedicalHistory> insertList, IList<PastMedicalHistory> updateList, IList<PastMedicalHistory> deleteList, GeneralNotes generalNotesObject, string macAddress, ulong Encounter_ID, ulong Human_Id, ulong Physician_ID, string UserName, DateTime date, string medicalInfo, string Version_Yr, Dictionary<string,string> ICDCodes)
        //{
        //    GeneralNotesManager generalNotesManager = new GeneralNotesManager();
        //    IList<GeneralNotes> generalNotesListInsert = new List<GeneralNotes>();
        //    IList<GeneralNotes> generalNotesListUpdate = new List<GeneralNotes>();
        //    //ProblemList
        //    ProblemList objProblemList = null;
        //    ProblemListManager objProblemListManager = new ProblemListManager();
        //    ProblemHistoryDTO pblm = new ProblemHistoryDTO();
        //    IList<ProblemList> SaveProblemList = new List<ProblemList>();
        //    IList<ProblemList> UpdateProblemList = new List<ProblemList>();
        //    IList<ProblemList> DeleteProblemList = new List<ProblemList>();
        //    IList<ProblemList> CheckProblemList = new List<ProblemList>();
        //    IList<PastMedicalHistory> CheckPastMedicalHistory = new List<PastMedicalHistory>();
        //    //IList<ProblemList> CheckProblemList = objProblemListManager.ICDByHumanID(Human_Id);
        //    string FileName = "Human" + "_" + Human_Id + ".xml";
        //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["XMLPath"], FileName);
        //    if (File.Exists(strXmlFilePath) == true)
        //    {
        //        XmlDocument itemDoc = new XmlDocument();
        //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
        //        XmlNodeList xmlTagName = null;
        //        itemDoc.Load(XmlText);
        //        XmlText.Close();
        //        if (itemDoc.GetElementsByTagName("ProblemListList").Count > 0 && itemDoc.GetElementsByTagName("ProblemListList")[0] != null)
        //        {
        //            xmlTagName = itemDoc.GetElementsByTagName("ProblemListList")[0].ChildNodes;
        //            if (xmlTagName.Count > 0)
        //            {
        //                for (int j = 0; j < xmlTagName.Count; j++)
        //                {

        //                    string TagName = xmlTagName[j].Name;
        //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(ProblemList));
        //                    ProblemList objPbList = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ProblemList;
        //                    IEnumerable<PropertyInfo> propInfo = null;
        //                    if (objPbList != null)
        //                    {
        //                        propInfo = from obji in ((ProblemList)objPbList).GetType().GetProperties() select obji;

        //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
        //                        {
        //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
        //                            {
        //                                foreach (PropertyInfo property in propInfo)
        //                                {
        //                                    if (property.Name == nodevalue.Name)
        //                                    {
        //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
        //                                            property.SetValue(objPbList, Convert.ToUInt64(nodevalue.Value), null);
        //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
        //                                            property.SetValue(objPbList, Convert.ToString(nodevalue.Value), null);
        //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
        //                                            property.SetValue(objPbList, Convert.ToDateTime(nodevalue.Value), null);
        //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
        //                                            property.SetValue(objPbList, Convert.ToInt32(nodevalue.Value), null);
        //                                        else
        //                                            property.SetValue(objPbList, nodevalue.Value, null);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        CheckProblemList.Add(objPbList);
        //                    }
        //                }
        //            }
        //        }
        //        if (itemDoc.GetElementsByTagName("PastMedicalHistoryList")[0] != null && itemDoc.GetElementsByTagName("PastMedicalHistoryList").Count > 0)
        //        {
        //            xmlTagName = itemDoc.GetElementsByTagName("PastMedicalHistoryList")[0].ChildNodes;
        //            if (xmlTagName.Count > 0)
        //            {
        //                for (int j = 0; j < xmlTagName.Count; j++)
        //                {
        //                    string TagName = xmlTagName[j].Name;
        //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(PastMedicalHistory));
        //                    PastMedicalHistory PastMedicalHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PastMedicalHistory;
        //                    IEnumerable<PropertyInfo> propInfo = null;
        //                    propInfo = from obji in ((PastMedicalHistory)PastMedicalHistory).GetType().GetProperties() select obji;

        //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
        //                    {
        //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
        //                        {
        //                            foreach (PropertyInfo property in propInfo)
        //                            {
        //                                if (property.Name == nodevalue.Name)
        //                                {
        //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
        //                                        property.SetValue(PastMedicalHistory, Convert.ToUInt64(nodevalue.Value), null);
        //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
        //                                        property.SetValue(PastMedicalHistory, Convert.ToString(nodevalue.Value), null);
        //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
        //                                        property.SetValue(PastMedicalHistory, Convert.ToDateTime(nodevalue.Value), null);
        //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
        //                                        property.SetValue(PastMedicalHistory, Convert.ToInt32(nodevalue.Value), null);
        //                                    else
        //                                        property.SetValue(PastMedicalHistory, nodevalue.Value, null);
        //                                }
        //                            }
        //                        }

        //                    }
        //                    CheckPastMedicalHistory.Add(PastMedicalHistory);
        //                }
        //            }
        //        }
        //    }

        //    iTryCount = 0;
        //TryAgain:

        //    int iResult = 0;

        //    ISession MySession = Session.GetISession();
        //    //ITransaction trans = null;
        //    try
        //    {
        //        using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
        //        {
        //            try
        //            {
        //                //trans = MySession.BeginTransaction();
        //                //PMH
        //                //SaveUpdateDeleteWithTransaction(ref insertList, updateList, deleteList, macAddress);
        //                iResult = SaveUpdateDeleteWithoutTransaction(ref insertList, updateList, deleteList, MySession, macAddress);

        //                if (iResult == 2)
        //                {
        //                    if (iTryCount < 5)
        //                    {
        //                        iTryCount++;
        //                        goto TryAgain;
        //                    }
        //                    else
        //                    {
        //                        trans.Rollback();
        //                        // MySession.Close();
        //                        throw new Exception("Deadlock occurred. Transaction failed.");
        //                    }
        //                }
        //                else if (iResult == 1)
        //                {
        //                    trans.Rollback();
        //                    //MySession.Close();
        //                    throw new Exception("Exception occurred. Transaction failed.");
        //                }
        //                string[] Others = { "Other1", "Other2", "Other3", "Other4", "Other5" };
        //                if (CheckProblemList != null && CheckProblemList.Count > 0)
        //                {

        //                    if (insertList != null && insertList.Count > 0)
        //                    {

        //                        foreach (var item in insertList)
        //                        {
        //                            if (!Others.Contains(item.Past_Medical_Info))
        //                            {
        //                                if (!CheckProblemList.Any(a => a.ICD == item.AHA_Question_ICD))
        //                                {
        //                                    if (item.Is_present == "Y")//Save ProblemList
        //                                    {
        //                                        objProblemList = new ProblemList();
        //                                        objProblemList.Encounter_ID = Encounter_ID;
        //                                        objProblemList.Human_ID = Human_Id;
        //                                        objProblemList.Physician_ID = Physician_ID;
        //                                        objProblemList.ICD = item.AHA_Question_ICD;
        //                                        objProblemList.Problem_Description = item.Past_Medical_Info;

        //                                        if (item.To_Date == "Current")
        //                                            objProblemList.Status = "Active";
        //                                        else if (item.To_Date != string.Empty)
        //                                            objProblemList.Status = "Resolved";
        //                                        else if (item.From_Date == string.Empty)
        //                                            objProblemList.Status = "Inactive";
        //                                        else if (item.To_Date == string.Empty)
        //                                            objProblemList.Status = "Active";

        //                                        if (item.From_Date != string.Empty)
        //                                            objProblemList.Date_Diagnosed = item.From_Date;
        //                                        else
        //                                            objProblemList.Date_Diagnosed = string.Empty;


        //                                        objProblemList.Is_Active = "Y";
        //                                        objProblemList.Reference_Source = "AHA Questionnaire";


        //                                        objProblemList.Created_By = UserName;
        //                                        objProblemList.Created_Date_And_Time = date;
        //                                       //objProblemList.Version_Year = Version_Yr;

        //                                       // objProblemList.ICD_9 = string.Empty;
        //                                       // objProblemList.ICD_9_Description = string.Empty;
        //                                        objProblemList.Version_Year = "ICD_10";// Always Version Year as  ICD10 because if selected ICD as ICD9 that ICD is converted to ICD10.
        //                                        if (ICDCodes.ContainsKey(objProblemList.ICD))
        //                                        {
        //                                            objProblemList.ICD_9 = ICDCodes[objProblemList.ICD].ToString();
        //                                            objProblemList.ICD_9_Description = objProblemList.Problem_Description;
        //                                        }
        //                                        SaveProblemList.Add(objProblemList);
        //                                    }
        //                                }
        //                                else//Update ProblemList
        //                                {
        //                                    //ProblemList ProblemLst = CheckProblemList.Where(a => a.ICD_Code == item.AHA_Question_ICD && a.Problem_Description == item.Past_Medical_Info).ToList<ProblemList>()[0];//ICD 401 Description Different in Problem List and User_LookUp Table
        //                                    ProblemList ProblemLst = CheckProblemList.Where(a => a.ICD == item.AHA_Question_ICD).ToList<ProblemList>()[0];
        //                                    string Pb_Status=string.Empty;
        //                                    if (item.To_Date == "Current")
        //                                        Pb_Status = "Active";
        //                                    else if (item.To_Date != string.Empty)
        //                                        Pb_Status = "Resolved";
        //                                    else if (item.From_Date == string.Empty)
        //                                        Pb_Status = "Inactive";
        //                                    else if (item.To_Date == string.Empty)
        //                                        Pb_Status = "Active";
        //                                    if (ProblemLst.Date_Diagnosed != item.From_Date || ProblemLst.Status != Pb_Status || CheckPastMedicalHistory.Any(p => p.AHA_Question_ICD == item.AHA_Question_ICD && p.Notes != item.Notes))
        //                                    {
        //                                        ProblemLst.Status = Pb_Status;
        //                                        if (item.From_Date != string.Empty)
        //                                            ProblemLst.Date_Diagnosed = item.From_Date;
        //                                        else
        //                                            ProblemLst.Date_Diagnosed = string.Empty;


        //                                        if (ProblemLst.Reference_Source.Contains("AHA Questionnaire"))
        //                                        {
        //                                            if (ProblemLst.Reference_Source.Contains("Deleted"))
        //                                                ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");
        //                                        }
        //                                        else
        //                                        {
        //                                            if (ProblemLst.Reference_Source.Contains("Deleted"))
        //                                                ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");

        //                                            ProblemLst.Reference_Source += "|AHA Questionnaire";
        //                                        }

        //                                        ProblemLst.Is_Active = item.Is_present;
        //                                        ProblemLst.Modified_By = UserName;
        //                                        ProblemLst.Modified_Date_And_Time = date;
        //                                        //ProblemLst.Version_Year = Version_Yr;
        //                                        //ProblemLst.ICD_9 = string.Empty;
        //                                        //ProblemLst.ICD_9_Description = string.Empty;
        //                                        ProblemLst.Version_Year = "ICD_10";// Always Version Year as  ICD10 because if selected ICD as ICD9 that ICD is converted to ICD10.
        //                                        if (ICDCodes.ContainsKey(ProblemLst.ICD))
        //                                        {
        //                                            ProblemLst.ICD_9 = ICDCodes[ProblemLst.ICD].ToString();
        //                                            ProblemLst.ICD_9_Description = ProblemLst.Problem_Description;
        //                                        }
        //                                        UpdateProblemList.Add(ProblemLst);
        //                                    }


        //                                    //if (item.To_Date == "Current")
        //                                    //    ProblemLst.Status = "Active";
        //                                    //else if (item.To_Date != string.Empty)
        //                                    //    ProblemLst.Status = "Resolved";
        //                                    //else if (item.From_Date == string.Empty)
        //                                    //    ProblemLst.Status = "Inactive";
        //                                    //else if (item.To_Date == string.Empty)
        //                                    //    ProblemLst.Status = "Active";

        //                                }
        //                            }
        //                        }
        //                    }


        //                    if (updateList != null && updateList.Count > 0)//Update ProblemList
        //                    {

        //                        foreach (var item in updateList)
        //                        {
        //                            if (!Others.Contains(item.Past_Medical_Info))
        //                            {
        //                                if (CheckProblemList.Any(a => a.ICD == item.AHA_Question_ICD))
        //                                {
        //                                    ProblemList ProblemLst = CheckProblemList.Where(a => a.ICD == item.AHA_Question_ICD).ToList<ProblemList>()[0];
        //                                    //Added By Manimaran for check problem list update.
        //                                    string Pb_Status = string.Empty;
        //                                    if (item.To_Date == "Current")
        //                                        Pb_Status = "Active";
        //                                    else if (item.To_Date != string.Empty)
        //                                        Pb_Status = "Resolved";
        //                                    else if (item.From_Date == string.Empty)
        //                                        Pb_Status = "Inactive";
        //                                    else if (item.To_Date == string.Empty)
        //                                        Pb_Status = "Active";
        //                                    if (ProblemLst.Date_Diagnosed != item.From_Date || ProblemLst.Status != Pb_Status || CheckPastMedicalHistory.Any(p => p.AHA_Question_ICD == item.AHA_Question_ICD && p.Notes != item.Notes))
        //                                    {
        //                                        ProblemLst.Status = Pb_Status;
        //                                        if (item.From_Date != string.Empty)
        //                                            ProblemLst.Date_Diagnosed = item.From_Date;
        //                                        else
        //                                            ProblemLst.Date_Diagnosed = string.Empty;


        //                                        if (ProblemLst.Reference_Source.Contains("AHA Questionnaire"))
        //                                        {
        //                                            if (ProblemLst.Reference_Source.Contains("Deleted"))
        //                                                ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");
        //                                        }
        //                                        else
        //                                        {
        //                                            if (ProblemLst.Reference_Source.Contains("Deleted"))
        //                                                ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");

        //                                            ProblemLst.Reference_Source += "|AHA Questionnaire";
        //                                        }

        //                                        ProblemLst.Is_Active = item.Is_present;
        //                                        ProblemLst.Modified_By = UserName;
        //                                        ProblemLst.Modified_Date_And_Time = date;
        //                                        //ProblemLst.Version_Year = Version_Yr;
        //                                        //ProblemLst.ICD_9 = string.Empty;
        //                                        //ProblemLst.ICD_9_Description = string.Empty;
        //                                        ProblemLst.Version_Year = "ICD_10";// Always Version Year as  ICD10 because if selected ICD as ICD9 that ICD is converted to ICD10.
        //                                        if (ICDCodes.ContainsKey(ProblemLst.ICD))
        //                                        {
        //                                            ProblemLst.ICD_9 = ICDCodes[ProblemLst.ICD].ToString();
        //                                            ProblemLst.ICD_9_Description = ProblemLst.Problem_Description;
        //                                        }
        //                                        UpdateProblemList.Add(ProblemLst);
        //                                    }
        //                                    /*
        //                                    if (item.To_Date == "Current")
        //                                        ProblemLst.Status = "Active";
        //                                    else if (item.To_Date != string.Empty)
        //                                        ProblemLst.Status = "Resolved";
        //                                    else if (item.From_Date == string.Empty)
        //                                        ProblemLst.Status = "Inactive";
        //                                    else if (item.To_Date == string.Empty)
        //                                        ProblemLst.Status = "Active";

        //                                    if (item.From_Date != string.Empty)
        //                                        ProblemLst.Date_Diagnosed = item.From_Date;
        //                                    else
        //                                        ProblemLst.Date_Diagnosed = string.Empty;




        //                                    if (ProblemLst.Reference_Source.Contains("AHA Questionnaire"))
        //                                    {
        //                                        if (ProblemLst.Reference_Source.Contains("Deleted"))
        //                                            ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");
        //                                    }
        //                                    else
        //                                    {
        //                                        if (ProblemLst.Reference_Source.Contains("Deleted"))
        //                                            ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");

        //                                        ProblemLst.Reference_Source += "|AHA Questionnaire";
        //                                    }

        //                                    ProblemLst.Is_Active = item.Is_present;
        //                                    ProblemLst.Modified_By = UserName;
        //                                    ProblemLst.Modified_Date_And_Time = date;
        //                                    //ProblemLst.Version_Year = Version_Yr;
        //                                    //ProblemLst.ICD_9 = string.Empty;
        //                                    //ProblemLst.ICD_9_Description = string.Empty;
        //                                    ProblemLst.Version_Year = "ICD_10";// Always Version Year as  ICD10 because if selected ICD as ICD9 that ICD is converted to ICD10.
        //                                    if (ICDCodes.ContainsKey(ProblemLst.ICD))
        //                                    {
        //                                        ProblemLst.ICD_9 = ICDCodes[ProblemLst.ICD].ToString();
        //                                        ProblemLst.ICD_9_Description = ProblemLst.Problem_Description;
        //                                    }
        //                                    UpdateProblemList.Add(ProblemLst);
        //                                    */
        //                                }
        //                                else//Save
        //                                {

        //                                    if (item.Is_present == "Y")
        //                                    {
        //                                        objProblemList = new ProblemList();
        //                                        objProblemList.Encounter_ID = Encounter_ID;
        //                                        objProblemList.Human_ID = Human_Id;
        //                                        objProblemList.Physician_ID = Physician_ID;
        //                                        objProblemList.ICD = item.AHA_Question_ICD;
        //                                        objProblemList.Problem_Description = item.Past_Medical_Info;

        //                                        if (item.To_Date == "Current")
        //                                            objProblemList.Status = "Active";
        //                                        else if (item.To_Date != string.Empty)
        //                                            objProblemList.Status = "Resolved";
        //                                        else if (item.From_Date == string.Empty)
        //                                            objProblemList.Status = "Inactive";
        //                                        else if (item.To_Date == string.Empty)
        //                                            objProblemList.Status = "Active";

        //                                        if (item.From_Date != string.Empty)
        //                                            objProblemList.Date_Diagnosed = item.From_Date;
        //                                        else
        //                                            objProblemList.Date_Diagnosed = string.Empty;


        //                                        objProblemList.Is_Active = "Y";
        //                                        objProblemList.Reference_Source = "AHA Questionnaire";
        //                                        //objProblemList.Version_Year = Version_Yr;
        //                                        //objProblemList.ICD_9 = string.Empty;
        //                                        //objProblemList.ICD_9_Description = string.Empty;
        //                                        objProblemList.Version_Year = "ICD_10";// Always Version Year as  ICD10 because if selected ICD as ICD9 that ICD is converted to ICD10.
        //                                        if (ICDCodes.ContainsKey(objProblemList.ICD))
        //                                        {
        //                                            objProblemList.ICD_9 = ICDCodes[objProblemList.ICD].ToString();
        //                                            objProblemList.ICD_9_Description = objProblemList.Problem_Description;
        //                                        }

        //                                        objProblemList.Created_By = UserName;
        //                                        objProblemList.Created_Date_And_Time = date;
        //                                        SaveProblemList.Add(objProblemList);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    if (deleteList != null && deleteList.Count > 0)//Delete ProblemList
        //                    {
        //                        foreach (var item in deleteList)
        //                        {
        //                            if (!Others.Contains(item.Past_Medical_Info))
        //                            {
        //                                if (CheckProblemList.Any(a => a.ICD == item.AHA_Question_ICD))
        //                                {
        //                                    ProblemList ProblemLst = CheckProblemList.Where(a => a.ICD == item.AHA_Question_ICD).ToList<ProblemList>()[0];


        //                                    if (ProblemLst.Reference_Source.Contains("AHA Questionnaire"))
        //                                    {
        //                                        if (ProblemLst.Reference_Source.Contains("Deleted"))
        //                                            ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");
        //                                    }
        //                                    else
        //                                    {
        //                                        if (ProblemLst.Reference_Source.Contains("Deleted"))
        //                                            ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");

        //                                        ProblemLst.Reference_Source += "|AHA Questionnaire";
        //                                    }

        //                                    ProblemLst.Status = "Inactive";
        //                                    ProblemLst.Is_Active = "N";
        //                                    ProblemLst.Modified_By = UserName;
        //                                    ProblemLst.Modified_Date_And_Time = date;
        //                                    //ProblemLst.Version_Year = Version_Yr;
        //                                    ProblemLst.Version_Year = "ICD_10";// Always Version Year as  ICD10 because if selected ICD as ICD9 that ICD is converted to ICD10.
        //                                    if (ICDCodes.ContainsKey(ProblemLst.ICD))
        //                                    {
        //                                        ProblemLst.ICD_9 = ICDCodes[ProblemLst.ICD].ToString();
        //                                        ProblemLst.ICD_9_Description = ProblemLst.Problem_Description;
        //                                    }
        //                                    UpdateProblemList.Add(ProblemLst);
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //                else
        //                {
        //                    foreach (var item in insertList)//Save ProblemList
        //                    {
        //                        if (!Others.Contains(item.Past_Medical_Info))
        //                        {
        //                            if (item.Is_present == "Y")
        //                            {
        //                                objProblemList = new ProblemList();
        //                                objProblemList.Encounter_ID = Encounter_ID;
        //                                objProblemList.Human_ID = Human_Id;
        //                                objProblemList.Physician_ID = Physician_ID;
        //                                objProblemList.ICD = item.AHA_Question_ICD;
        //                                objProblemList.Problem_Description = item.Past_Medical_Info;

        //                                if (item.To_Date == "Current")
        //                                    objProblemList.Status = "Active";
        //                                else if (item.To_Date != string.Empty)
        //                                    objProblemList.Status = "Resolved";
        //                                else if (item.From_Date == string.Empty)
        //                                    objProblemList.Status = "Inactive";
        //                                else if (item.To_Date == string.Empty)
        //                                    objProblemList.Status = "Active";

        //                                if (item.From_Date != string.Empty)
        //                                    objProblemList.Date_Diagnosed = item.From_Date;
        //                                else
        //                                    objProblemList.Date_Diagnosed = string.Empty;

        //                                objProblemList.Is_Active = item.Is_present;
        //                                objProblemList.Reference_Source = "AHA Questionnaire";
        //                                objProblemList.Created_By = UserName;
        //                                objProblemList.Created_Date_And_Time = date;
        //                                //objProblemList.Version_Year = Version_Yr;
        //                                objProblemList.Version_Year = "ICD_10";// Always Version Year as  ICD10 because if selected ICD as ICD9 that ICD is converted to ICD10.
        //                                if (ICDCodes.ContainsKey(objProblemList.ICD))
        //                                {
        //                                    objProblemList.ICD_9 = ICDCodes[objProblemList.ICD].ToString();
        //                                    objProblemList.ICD_9_Description = objProblemList.Problem_Description;
        //                                }
        //                                SaveProblemList.Add(objProblemList);
        //                            }
        //                        }
        //                    }
        //                }
        //                var curr_Insert = from objIns in SaveProblemList where objIns.Status == "Active" select objIns;
        //                var curr_Update = from objUp in UpdateProblemList where objUp.Status == "Active" select objUp;
        //                var curr_Delete = from objDel in DeleteProblemList where objDel.Status == "Active" select objDel;
        //                if (curr_Insert.ToList().Count > 0 || curr_Update.ToList().Count > 0 || curr_Delete.ToList().Count > 0)
        //                {
        //                    var GetActiveProb = from obj in CheckProblemList where obj.ICD == "0000" select obj;
        //                    if (GetActiveProb.ToList().Count > 0)
        //                    {
        //                        ProblemList prblst = GetActiveProb.ToList<ProblemList>()[0];
        //                        prblst.Is_Active = "N";
        //                        prblst.Modified_By = UserName;
        //                        prblst.Modified_Date_And_Time = date;
        //                        UpdateProblemList.Add(prblst);
        //                    }
        //                }
        //                if (SaveProblemList.Count > 0 || UpdateProblemList.Count > 0 || DeleteProblemList.Count > 0)
        //                {
        //                    iResult = objProblemListManager.SaveUpdateDeleteWithoutTransaction(ref SaveProblemList, UpdateProblemList, DeleteProblemList, MySession, macAddress);

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
        //                            // MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        //MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                }
        //                //GeneralNotes
        //                //GeneralNotesManager generalNotesManager = new GeneralNotesManager();

        //                //IList<GeneralNotes> generalNotesListInsert = new List<GeneralNotes>();
        //                //IList<GeneralNotes> generalNotesListUpdate = new List<GeneralNotes>();
        //                /* Loaded The GeneralNotes From PageLoad Events, Instead Of Hitting Database */
        //                //   ICriteria crit = session.GetISession().CreateCriteria(typeof(GeneralNotes)).Add(Expression.Eq("Human_ID", Human_Id)).Add(Expression.Eq("Parent_Field", "Past Medical History")).Add(Expression.Eq("Encounter_ID", Encounter_ID));
        //                //   IList<GeneralNotes> ResultLst = crit.List<GeneralNotes>();
        //                /*
        //                IList<GeneralNotes> ResultLst = new List<GeneralNotes>();// Need to change this statement because created by and time not saved.
        //                ResultLst.Add(generalNotesObject);
        //                */
        //                if (generalNotesObject != null)
        //                {
        //                    if (generalNotesObject.Id != 0)
        //                    {
        //                        generalNotesObject.Modified_By = UserName;
        //                        generalNotesObject.Modified_Date_And_Time = date;
        //                        generalNotesListUpdate.Add(generalNotesObject);
        //                    }
        //                    else
        //                    {
        //                        generalNotesObject.Encounter_ID = Encounter_ID;
        //                        generalNotesObject.Version = 0;
        //                        generalNotesObject.Created_By = UserName;
        //                        generalNotesObject.Created_Date_And_Time = date;
        //                        generalNotesListInsert.Add(generalNotesObject);
        //                    }
        //                    /*
        //                    if (ResultLst != null && ResultLst.Count > 0)
        //                    {
        //                        ResultLst[0].Encounter_ID = Encounter_ID;
        //                        ResultLst[0].Notes = generalNotesObject.Notes;
        //                        ResultLst[0].Modified_By = UserName;
        //                        ResultLst[0].Modified_Date_And_Time = date;
        //                        generalNotesListUpdate.Add(ResultLst[0]);
        //                    }
        //                    else
        //                    {
        //                        generalNotesObject.Id = 0;
        //                        generalNotesObject.Encounter_ID = Encounter_ID;
        //                        generalNotesObject.Version = 0;
        //                        generalNotesObject.Created_By = UserName;
        //                        generalNotesObject.Created_Date_And_Time = date;
        //                        generalNotesListInsert.Add(generalNotesObject);
        //                    }
        //                    */
        //                    iResult = generalNotesManager.SaveUpdateDeleteGeneralNotes(generalNotesListInsert, generalNotesListUpdate, MySession, macAddress);

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
        //                            //  MySession.Close();
        //                            throw new Exception("Deadlock occurred. Transaction failed.");
        //                        }
        //                    }
        //                    else if (iResult == 1)
        //                    {
        //                        trans.Rollback();
        //                        //MySession.Close();
        //                        throw new Exception("Exception occurred. Transaction failed.");
        //                    }
        //                }

        //                MySession.Flush();
        //                trans.Commit();
        //            }
        //            catch (NHibernate.Exceptions.GenericADOException ex)
        //            {
        //                trans.Rollback();
        //                // MySession.Close();
        //                throw new Exception(ex.Message);
        //            }
        //            catch (Exception e)
        //            {
        //                trans.Rollback();
        //                //MySession.Close();
        //                throw new Exception(e.Message);
        //            }
        //            finally
        //            {
        //                MySession.Close();
        //            }

        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        //MySession.Close();
        //        throw new Exception(ex.Message);
        //    }


        //    GenerateXml XMLObj = new GenerateXml();
        //    IList<ProblemList> problist = new List<ProblemList>();
        //    List<object> prob = new List<object>();

        //    List<object> lstGenrl = new List<object>();
        //    //List<object> combList = new List<object>();
        //    IList<PastMedicalHistory> past=new List<PastMedicalHistory>();

        //    ulong encounterid=0;
        //    if (insertList.Count > 0)
        //    {
        //        foreach(PastMedicalHistory obj in insertList)
        //        {
        //            past.Add(obj);
        //        }
        //        //List<object> lstObj = insertList.Cast<object>().ToList();
        //        XMLObj.GenerateXmlSaveStatic(insertList.Cast<object>().ToList(), Human_Id, string.Empty);
        //        //combList.AddRange(lstObj);
        //        encounterid = insertList[0].Encounter_Id;
        //    }
        //    if (updateList.Count > 0)
        //    { 
        //        foreach(PastMedicalHistory obj in updateList)
        //        {
        //            past.Add(obj);
        //        }
        //        foreach (var obj in updateList)
        //            obj.Version += 1;
        //        //List<object> lstObj = updateList.Cast<object>().ToList();
        //        XMLObj.GenerateXmlUpdate(updateList.Cast<object>().ToList(), Human_Id, string.Empty);
        //        //combList.AddRange(lstObj);
        //        encounterid = updateList[0].Encounter_Id;
        //    }
        //    //XMLObj.GenerateXmlSave(combList, encounterid, string.Empty);//Commented By Manimaran (18-11-2015 1:31:55 PM) for Encounter Id is Incorrect While save general notes without any past medical history.
        //    //XMLObj.GenerateXmlSave(combList, Human_Id, string.Empty);//Added By Manimaran (18-11-2015 1:31:55 PM) for Encounter Id is Incorrect While save general notes without any past medical history.
        //    if (deleteList.Count > 0)
        //    {
        //        List<object> lstObj = deleteList.Cast<object>().ToList();
        //        XMLObj.DeleteXmlNode(Human_Id, lstObj, string.Empty);
        //    }
        //    prob.Clear();
        //    if (SaveProblemList.Count>0)
        //    {
        //        foreach (ProblemList obj in SaveProblemList)
        //        {
        //            problist.Add(obj);
        //        }
        //        List<object> lstObj = SaveProblemList.Cast<object>().ToList();
        //        prob.AddRange(lstObj);
        //        XMLObj.GenerateXmlSaveStatic(prob, Human_Id, string.Empty);
        //    }
        //    prob.Clear();
        //    if (UpdateProblemList.Count > 0)
        //    {
        //        foreach (ProblemList obj in UpdateProblemList)
        //        {
        //            problist.Add(obj);
        //        }
        //        foreach (var obj in UpdateProblemList)
        //            obj.Version += 1;
        //        List<object> lstObj = UpdateProblemList.Cast<object>().ToList();
        //        prob.AddRange(lstObj);
        //        XMLObj.GenerateXmlUpdate(prob, Human_Id, string.Empty);
        //    }
        //    //XMLObj.GenerateXmlSave(prob, encounterid, string.Empty);//Commented By Manimaran (18-11-2015 1:31:55 PM) for Encounter Id is Incorrect While save general notes without any past medical history.
        //    //XMLObj.GenerateXmlSave(prob, Human_Id, string.Empty);//Added By Manimaran (18-11-2015 1:31:55 PM) for Encounter Id is Incorrect While save general notes without any past medical history.

        //    if (generalNotesListInsert.Count > 0)
        //    {
        //        //List<object> lstObj = generalNotesListInsert.Cast<object>().ToList();
        //        //lstGenrl.AddRange(lstObj);
        //        XMLObj.GenerateXmlSaveStatic(generalNotesListInsert.Cast<object>().ToList(), Human_Id, "PastMedicalHistory");
        //    }
        //    else if (generalNotesListUpdate.Count > 0)
        //    {
        //        generalNotesListUpdate[0].Version += 1;
        //        XMLObj.GenerateXmlUpdate(generalNotesListUpdate.Cast<object>().ToList(), Human_Id, "PastMedicalHistory");
        //        //List<object> lstObj = generalNotesListUpdate.Cast<object>().ToList();
        //        //lstGenrl.AddRange(lstObj);
        //    }
        //    //XMLObj.GenerateXmlSave(lstGenrl, encounterid, "PastMedicalHistory");//Commented By Manimaran (18-11-2015 1:31:55 PM) for Encounter Id is Incorrect While save general notes without any past medical history.
        //    //XMLObj.GenerateXmlSave(lstGenrl, Human_Id, "PastMedicalHistory");//Added By Manimaran (18-11-2015 1:31:55 PM) for Encounter Id is Incorrect While save general notes without any past medical history.
        //    pblm.GeneralNotesObject = generalNotesObject;
        //    pblm.PastMedicalList = past;
        //    pblm.ProblemList = problist;
        //    return pblm;
        //  //  return GetPastMedicalHistoryByHumanID(Human_Id, Encounter_ID, medicalInfo, Physician_ID, "PAST MEDICAL INFO", "Sort_Order");
        //}
        public ProblemHistoryDTO SaveUpdateDeletePastMedicalHistory(IList<PastMedicalHistory> SavePastMedicalList, IList<PastMedicalHistory> UpdatePastMedicalList, IList<PastMedicalHistory> DeletePastMedicalList, IList<ProblemList> SaveProblemList, IList<ProblemList> UpdateProblemList, IList<GeneralNotes> SaveGeneralNotes, IList<GeneralNotes> UpdateGeneralNotes, string macAddress, ulong HumanId, IList<PastMedicalHistoryMaster> SaveListMaster, IList<PastMedicalHistoryMaster> UpdateListMaster, IList<PastMedicalHistoryMaster> DeleteListMaster)
        {
            GeneralNotesManager generalNotesManager = new GeneralNotesManager();
            ProblemListManager objProblemListManager = new ProblemListManager();
            ProblemHistoryDTO pblm = new ProblemHistoryDTO();
            GenerateXml XMLObj = new GenerateXml();
            PastMedicalHistoryMasterManager objPastMedicalHistoryManagerMaster = new PastMedicalHistoryMasterManager();
            IList<PastMedicalHistory> SavePastMedicalListFinal = new List<PastMedicalHistory>();
            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if ((SaveListMaster != null && SaveListMaster.Count > 0) || (UpdateListMaster != null && UpdateListMaster.Count > 0) || (DeleteListMaster != null && DeleteListMaster.Count > 0))
                        {
                            objPastMedicalHistoryManagerMaster.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveListMaster, ref UpdateListMaster, DeleteListMaster, MySession, macAddress, true, true, HumanId, string.Empty, ref XMLObj);
                            pblm.PastMedicalMasterList = SaveListMaster.Concat(UpdateListMaster).ToList<PastMedicalHistoryMaster>();
                        }
                        bool IsPastMedicalHistory = true, IsProblemList = true, IsGeneralNotes = true;
                        if ((SavePastMedicalList != null && SavePastMedicalList.Count > 0) || (UpdatePastMedicalList != null && UpdatePastMedicalList.Count > 0) || (DeletePastMedicalList != null && DeletePastMedicalList.Count > 0))
                        {
                            //Save master table 
                            if ((SaveListMaster != null && SaveListMaster.Count > 0) || (UpdateListMaster != null && UpdateListMaster.Count > 0) || (DeleteListMaster != null && DeleteListMaster.Count > 0))
                            {
                                IList<PastMedicalHistoryMaster> lstSaveUpdateDeletePMHMaster = SaveListMaster.Concat(UpdateListMaster).ToList<PastMedicalHistoryMaster>();
                                pblm.PastMedicalMasterList = SaveListMaster.Concat(UpdateListMaster).ToList<PastMedicalHistoryMaster>();
                                //Update Master Id in PMH
                                foreach (PastMedicalHistory objTemp in SavePastMedicalList)
                                {
                                    IList<PastMedicalHistoryMaster> lstPMHmasterTemp = new List<PastMedicalHistoryMaster>();
                                    lstPMHmasterTemp = lstSaveUpdateDeletePMHMaster.Where(a => a.Past_Medical_Info.Trim().ToUpper() == objTemp.Past_Medical_Info.Trim().ToUpper()).ToList<PastMedicalHistoryMaster>();
                                    objTemp.Past_Medical_History_Master_ID = lstPMHmasterTemp[0].Id;
                                    SavePastMedicalListFinal.Add(objTemp);
                                }
                            }
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SavePastMedicalListFinal, ref UpdatePastMedicalList, DeletePastMedicalList, MySession, macAddress, true, true, HumanId, string.Empty, ref XMLObj);
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
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }



                            pblm.PastMedicalList = SavePastMedicalListFinal.Concat(UpdatePastMedicalList).ToList<PastMedicalHistory>();
                            IsPastMedicalHistory = XMLObj.CheckDataConsistency(SavePastMedicalListFinal.Concat(UpdatePastMedicalList).Cast<object>().ToList(), false, string.Empty);

                        }
                        if ((SaveProblemList != null && SaveProblemList.Count > 0) || (UpdateProblemList != null && UpdateProblemList.Count > 0))
                        {
                            iResult = objProblemListManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveProblemList, ref UpdateProblemList, null, MySession, macAddress, true, true, HumanId, string.Empty, ref XMLObj);



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
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                            pblm.ProblemList = SaveProblemList.Concat(UpdateProblemList).ToList<ProblemList>();
                            IsProblemList = XMLObj.CheckDataConsistency(SaveProblemList.Concat(UpdateProblemList).Cast<object>().ToList(), true, string.Empty);
                        }
                        if ((SaveGeneralNotes != null && SaveGeneralNotes.Count > 0) || (UpdateGeneralNotes != null && UpdateGeneralNotes.Count > 0))
                        {
                            iResult = generalNotesManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveGeneralNotes, ref UpdateGeneralNotes, null, MySession, macAddress, true, true, HumanId, "PastMedicalHistory", ref XMLObj);
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
                                    throw new Exception("Deadlock occurred. Transaction failed.");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception occurred. Transaction failed.");
                            }
                            pblm.GeneralNotesObject = SaveGeneralNotes.Count > 0 ? SaveGeneralNotes[0] : UpdateGeneralNotes.Count > 0 ? UpdateGeneralNotes[0] : new GeneralNotes();
                            IsGeneralNotes = XMLObj.CheckDataConsistency(SaveGeneralNotes.Concat(UpdateGeneralNotes).Cast<object>().ToList(), false, "PastMedicalHistory");
                        }
                        if (IsPastMedicalHistory && IsProblemList && IsGeneralNotes)
                        {
                            //trans.Commit();
                           // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            int trycount = 0;
                        trytosaveagain:
                            try
                            {
                                #region "Code comitted by balaji.TJ 2023-01-06"
                                // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                #endregion
                                #region "Code Modified by balaji.TJ 2023-01-06"
                                WriteBlob(HumanId, XMLObj.itemDoc, MySession, SavePastMedicalList, UpdatePastMedicalList, DeletePastMedicalList, XMLObj, false);
                                #endregion


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
                            trans.Commit();
                        }
                        else
                        {
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                        }
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        MySession.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return pblm;
        }

    }
}
