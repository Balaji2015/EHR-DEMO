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
using System.Threading;
using System.Configuration;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IReferralOrderManager : IManagerBase<ReferralOrder, ulong>
    {
        ReferralOrderDTO InsertToReferralOrders(IList<ReferralOrder> saveList, IList<ReferralOrdersAssessment> ordAssSaveList, ulong EncounterID, string MACAddress, string sFinalText, ulong HumanID, string sLocalTime);
        ReferralOrderDTO UpdateToReferralOrders(IList<ReferralOrder> updtList, IList<ReferralOrdersAssessment> ordAssSaveList, IList<ReferralOrdersAssessment> ordAssDelList, ulong EncounterID, string MACAddress, string sOldText, string sNewText, ulong HumanID);
        ReferralOrderDTO GetReferralOrderUsingEncounterID(ulong EncounterID);
        ReferralOrderDTO GetReferralOrderUsingReferralOrderID(ulong ReferralOrderID);
        ReferralOrderDTO DeleteReferralorders(ReferralOrder obj, IList<ReferralOrdersAssessment> ordAssDelList, ulong EncounterID, string MACAddress, string sOldText, ulong HumanID);
        ReferralOrderDTO GetReferralOrderUsingHumanID(ulong HumanID,ulong PhysicianID,ulong EncounterID);// DateTime creadtedDate);
        ReferralOrderDTO LoadReferralOrder(ulong HumanID, ulong EncounterID,ulong PhysicianID);
        ReferralOrderDTO GetReferralOrderUsingEncounterIDWithoutPageNavigator(ulong EncounterID);
       IList<FillOrdersManagementDTO> SearchReferralOrder(int orderInDays, DateTime fromDate, DateTime toDate, ulong humanID, ulong physicianID,  int pageNumber, int maxResults, string order_type, string order_status, string FacilityName);
        ReferralOrderDTO FillReferralOrder(ulong HumanId,ulong Physician_Id,ulong EncounterID);
        object SubmitReferralOrder(IList<ReferralOrder> referralOrderList,string FacilityName,string MedAsstName, string MACAddress);
        void SaveReferalOrderforSummary(IList<ReferralOrder> lstreferalorder);
    }
    public partial class ReferralOrderManager : ManagerBase<ReferralOrder, ulong>, IReferralOrderManager
    {
        #region Constructors

        public ReferralOrderManager()
            : base()
        {

        }
        public ReferralOrderManager
            (INHibernateSession session)
            : base(session)
        {
        }

        #endregion


        #region CRUD
        int iTryCount = 0;
        public ReferralOrderDTO InsertToReferralOrders(IList<ReferralOrder> saveList, IList<ReferralOrdersAssessment> ordAssSaveList, ulong EncounterID, string MACAddress, string sFinalText, ulong HumanID, string sLocalTime)
        {
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjEncounter = new GenerateXml();
            TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();

            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            ISession MySession = Session.GetISession();
            try
            {
                bool IsReferralOrder = true, IsRefOrderAssesment = true, IsTreatmentPlanConsistent = true;
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    //TreatmentPlan SaveTreatmentPlan = null;
                    //TreatmentPlan UpdateTreatmentPlan = null;
                    //if (EncounterID != 0)
                    //{
                    //    IList<TreatmentPlan> Treatment_Plan = new List<TreatmentPlan>();
                    //    TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
                    //    ulong ulHumanID = GetHumanID(saveList, null, null);
                    //    Treatment_Plan = objTreatmentPlanMgr.GetTreatmentPlanUsingEncounterId(EncounterID, ulHumanID);
                    //    IList<TreatmentPlan> Treatmentlst = (from t in Treatment_Plan where t.Plan_Type.ToUpper() == "REFERRAL ORDER" select t).ToList<TreatmentPlan>();
                    //    if (Treatmentlst.Count == 0)
                    //    {
                    //        SaveTreatmentPlan = new TreatmentPlan();
                    //        SaveTreatmentPlan.Encounter_Id = EncounterID;
                    //        SaveTreatmentPlan.Human_ID = saveList[0].Human_ID;
                    //        SaveTreatmentPlan.Physician_Id = saveList[0].From_Physician_ID;
                    //        SaveTreatmentPlan.Plan_Type = "REFERRAL ORDER";
                    //        SaveTreatmentPlan.Plan = "* " + sFinalText;
                    //        SaveTreatmentPlan.Created_By = saveList[0].Created_By;
                    //        SaveTreatmentPlan.Created_Date_And_Time = saveList[0].Created_Date_And_Time;
                    //    }
                    //    else if (Treatmentlst[0].Plan.Contains(sFinalText) == false)
                    //    {
                    //        UpdateTreatmentPlan = new TreatmentPlan();
                    //        UpdateTreatmentPlan = Treatmentlst[0];
                    //        UpdateTreatmentPlan.Plan = (Treatmentlst[0].Plan.EndsWith("\r\n") == true) ? Treatmentlst[0].Plan + "* " + sFinalText : Treatmentlst[0].Plan + Environment.NewLine + "* " + sFinalText;
                    //        UpdateTreatmentPlan.Modified_Date_And_Time = saveList[0].Created_Date_And_Time;
                    //        UpdateTreatmentPlan.Modified_By = saveList[0].Created_By;
                    //    }
                    //}
                    try
                    {
                        if (saveList != null && saveList.Count > 0)
                        {
                            IList<ReferralOrder> UpdateReferralOrder = new List<ReferralOrder>();
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref UpdateReferralOrder, null, MySession, MACAddress, true, true, HumanID, string.Empty, ref XMLObj);
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
                            IsReferralOrder = XMLObj.CheckDataConsistency(saveList.Concat(UpdateReferralOrder).Cast<object>().ToList(), true, string.Empty);
                        }
                        if (ordAssSaveList != null && ordAssSaveList.Count > 0)
                        {
                            foreach (ReferralOrdersAssessment obj in ordAssSaveList)
                            {
                                obj.Referral_Order_ID = saveList[0].Id;
                            }
                            ReferralOrdersAssessmentManager objManager = new ReferralOrdersAssessmentManager();
                            IList<ReferralOrdersAssessment> UpdateRefOrderAss = new List<ReferralOrdersAssessment>();
                            iResult = objManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordAssSaveList, ref UpdateRefOrderAss, null, MySession, MACAddress, true, true, HumanID, string.Empty, ref XMLObj);
                            //iResult = objManager.BatchOperationsToReferralOrdersAssessment(ordAssSaveList, null, null, MySession, MACAddress);
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
                            IsRefOrderAssesment = XMLObj.CheckDataConsistency(ordAssSaveList.Concat(UpdateRefOrderAss).Cast<object>().ToList(), true, string.Empty);
                        }

                        #region Treatment plan
                        if (EncounterID != 0)
                        {
                            TreatmentPlan item = new TreatmentPlan();
                            item.Human_ID = saveList[0].Human_ID;
                            item.Encounter_Id = EncounterID;
                            item.Physician_Id = saveList[0].From_Physician_ID;
                            item.Created_By = saveList[0].Created_By;
                            item.Created_Date_And_Time = saveList[0].Created_Date_And_Time;
                            item.Plan_Type = "REFERRAL ORDER";
                            string plan_txt = string.Empty;
                            plan_txt = "* " + sFinalText;
                            item.Plan = plan_txt;
                            item.Version = 0;
                            item.Source_ID = saveList[0].Id;
                            item.Local_Time = sLocalTime;
                            Insert_Tplan.Add(item);
                        }
                        if (Insert_Tplan != null && Insert_Tplan.Count > 0)
                        {

                            //IList<TreatmentPlan> SaveTreatementPlanList = new List<TreatmentPlan>();
                            //IList<TreatmentPlan> UpdateTreatementPlanList = new List<TreatmentPlan>();
                            //if (SaveTreatmentPlan != null)
                            //{
                            //    SaveTreatementPlanList = new List<TreatmentPlan>();
                            //    SaveTreatementPlanList.Add(SaveTreatmentPlan);
                            //}
                            //if (UpdateTreatmentPlan != null)
                            //{
                            //    UpdateTreatementPlanList = new List<TreatmentPlan>();
                            //    UpdateTreatementPlanList.Add(UpdateTreatmentPlan);
                            //}

                            iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, null, MySession, MACAddress, true, true, EncounterID, string.Empty, ref XMLObjEncounter);
                            //iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveTreatementPlanList, ref UpdateTreatementPlanList, null, MySession, MACAddress, true, true, EncounterID, string.Empty, ref XMLObjEncounter);
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

                            //IList<TreatmentPlan> CombinedList = new List<TreatmentPlan>();
                            //if (UpdateTreatementPlanList != null && UpdateTreatementPlanList.Count > 0)
                            //    CombinedList = SaveTreatementPlanList.Concat(UpdateTreatementPlanList).ToList<TreatmentPlan>();
                            //else
                            //    CombinedList = SaveTreatementPlanList;

                            IsTreatmentPlanConsistent = XMLObjEncounter.CheckDataConsistency(Insert_Tplan.Cast<object>().ToList(), true, string.Empty);
                        }
                        #endregion

                        if (IsReferralOrder && IsRefOrderAssesment && IsTreatmentPlanConsistent)
                        {
                            try
                            {
                                //int trycount = 0;
                                //if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                                if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                                {
                                    // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    
                            //trytosaveagain:
                                try
                                {
                                    // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    WriteBlob(HumanID, XMLObj.itemDoc, MySession, saveList, null, null, XMLObj, true);

                                }
                                catch (Exception xmlexcep)
                                {
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
                                //if (XMLObjEncounter.strXmlFilePath != null && XMLObjEncounter.strXmlFilePath != "")
                                if (XMLObjEncounter.itemDoc.InnerXml != null && XMLObjEncounter.itemDoc.InnerXml != "")
                                {
                                    //XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
                                    // int trycount = 0;
                                    // trytosaveagain:
                                    try
                                {
                                        //XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
                                        //WriteBlob(EncounterID, XMLObjEncounter.itemDoc, MySession, saveList, null, null, XMLObjEncounter, true);
                                        objTreatmentPlanMgr.WriteBlob(EncounterID, XMLObjEncounter.itemDoc, MySession, Insert_Tplan, Update_Tplan, Delete_Tplan, XMLObjEncounter, false);

                                    }
                                catch (Exception xmlexcep)
                                {
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
                            catch (XmlException xmlexcep)
                            {
                                //CAP-1942
                                throw new Exception(xmlexcep.Message,xmlexcep);
                            }
                            catch (IOException ex)
                            {
                                //CAP-1942
                                throw new Exception(ex.Message,ex);
                            }
                        }
                        else
                        {
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                        }
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        //CAP-1942
                        throw new Exception(ex.Message,ex);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
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
                throw new Exception(ex1.Message,ex1);
            }
            // if (saveList.Count > 0)
            // {
            //     List<object> lstObj = saveList.Cast<object>().ToList();
            //     XMLObj.GenerateXmlSaveStatic(lstObj, HumanID, string.Empty);
            // }
            //if (ordAssSaveList.Count > 0)
            // {
            //     List<object> lstObj = ordAssSaveList.Cast<object>().ToList();
            //     XMLObj.GenerateXmlSaveStatic(lstObj, HumanID, string.Empty);
            // }
            if (EncounterID != 0)
            {
                return GetReferralOrderUsingEncounterID(EncounterID);
            }
            else
            {
                return GetReferralOrderUsingHumanID(saveList[0].Human_ID, saveList[0].From_Physician_ID, saveList[0].Encounter_ID);
            }
        }
        public ReferralOrderDTO UpdateToReferralOrders(IList<ReferralOrder> updtList, IList<ReferralOrdersAssessment> ordAssSaveList, IList<ReferralOrdersAssessment> ordAssDelList, ulong EncounterID, string MACAddress, string sOldText, string sNewText, ulong HumanID)
        {
            IList<TreatmentPlan> SaveTreatementPlanList = null;
            IList<TreatmentPlan> UpdateTreatmentPlanList = new List<TreatmentPlan>();
            IList<ReferralOrder> saveList = new List<ReferralOrder>();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjEncounter = new GenerateXml();
            iTryCount = 0;
            #region Commented By Deepak
           
            //string FileName = "Encounter" + "_" + EncounterID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //XmlTextReader XmlText = null;
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
            //            #region Treatment_plan
            //            if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("REFERRAL ORDER"))
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
            //            #endregion

            //            fs.Close();
            //            fs.Dispose();
            //        }

            //    }

                


            //}
            //catch(Exception Ex)
            //{
            //    if (XmlText != null)
            //        XmlText.Close();
            //    throw Ex;
            //}
            #endregion

            IList<string> ilstEntityListTagName = new List<string>();
            IList<object> ilstBlobFinal = new List<object>();
            ilstEntityListTagName.Add("TreatmentPlanList");
            ilstBlobFinal = ReadBlob(EncounterID, ilstEntityListTagName);
            if (ilstBlobFinal != null && ilstBlobFinal.Count > 0)
            {
                if (ilstBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstBlobFinal[0]).Count; iCount++)
                    {
                        objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstBlobFinal[0])[iCount]);
                    }
                }
            }

        TryAgain:
            int iResult = 0;
            TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            TreatmentPlan UpdateTreatmentPlan = null;
            ISession MySession = Session.GetISession();
            try
            {
                bool IsReferralOrder = true, IsRefOrderAssessment = true, IsTreatmentPlan = true;
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    if (EncounterID != 0)
                    {
                        //IList<TreatmentPlan> Treatment_Plan = new List<TreatmentPlan>();
                        //ulong ulHumanID = updtList.Count > 0 ? updtList[0].Human_ID : 0;
                        //Treatment_Plan = objTreatmentPlanMgr.GetTreatmentPlanUsingEncounterId(EncounterID, ulHumanID);
                        //IList<TreatmentPlan> Treatmentlst = (from t in Treatment_Plan where t.Plan_Type.ToUpper() == "REFERRAL ORDER" select t).ToList<TreatmentPlan>();
                        //if (Treatmentlst.Count > 0)
                        //{
                        //    UpdateTreatmentPlan = new TreatmentPlan();
                        //    UpdateTreatmentPlan = Treatmentlst[0];
                        //    string sPlan = UpdateTreatmentPlan.Plan;
                        //    sPlan = sPlan.Replace(sOldText, sNewText);
                        //    if (sPlan.LastIndexOf("\r\n") == -1)
                        //        sPlan = sPlan + Environment.NewLine;
                        //    UpdateTreatmentPlan.Plan = sPlan;
                        //    UpdateTreatmentPlan.Modified_By = updtList[0].Modified_By;
                        //    UpdateTreatmentPlan.Modified_Date_And_Time = updtList[0].Modified_Date_And_Time;
                        //}
                        IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Source_ID == updtList[0].Id).ToList<TreatmentPlan>();
                        if (itemlst != null && itemlst.Count > 0)
                        {
                            TreatmentPlan item = itemlst[0];
                            string sPlan = item.Plan;
                            sPlan = "* " + sNewText;
                            item.Plan = sPlan;
                            item.Modified_By = updtList[0].Modified_By;
                            item.Modified_Date_And_Time = updtList[0].Modified_Date_And_Time;
                            UpdateTreatmentPlan = item;
                        }
                    }
                    try
                    {
                        if (updtList != null && updtList.Count > 0)
                        {
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref updtList, null, MySession, MACAddress, true, true, HumanID, string.Empty, ref XMLObj);
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
                            IsReferralOrder = XMLObj.CheckDataConsistency(updtList.Concat(saveList).Cast<object>().ToList(), true, string.Empty);
                        }
                        if (ordAssSaveList.Count > 0 || ordAssDelList.Count > 0)
                        {
                            ReferralOrdersAssessmentManager objManager = new ReferralOrdersAssessmentManager();
                            IList<ReferralOrdersAssessment> UpdateRefOrderAss = new List<ReferralOrdersAssessment>();
                            iResult = objManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ordAssSaveList, ref UpdateRefOrderAss, ordAssDelList, MySession, MACAddress, true, true, HumanID, string.Empty, ref XMLObj);
                            //iResult = objManager.BatchOperationsToReferralOrdersAssessment(ordAssSaveList, null, ordAssDelList, MySession, MACAddress);
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
                            IsRefOrderAssessment = XMLObj.CheckDataConsistency(ordAssSaveList.Concat(UpdateRefOrderAss).Cast<object>().ToList(), true, string.Empty);
                        }
                        if (UpdateTreatmentPlan != null)
                        {
                            
                            UpdateTreatmentPlanList.Add(UpdateTreatmentPlan);

                            iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveTreatementPlanList, ref UpdateTreatmentPlanList, null, MySession, MACAddress, true, true, EncounterID, string.Empty, ref XMLObjEncounter);
                            //iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveTreatmentPlan, ref UpdateTreatmentPlanList, null, MySession, MACAddress, true, true, EncounterID, string.Empty, ref XMLObjEncounter);

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
                            IsTreatmentPlan = XMLObjEncounter.CheckDataConsistency(UpdateTreatmentPlanList.Cast<object>().ToList(), true, string.Empty);
                        }
                        if (IsReferralOrder && IsRefOrderAssessment && IsTreatmentPlan)
                        {
                            try
                            {
                                //if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                                if(XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                                {
                                   // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        //Commented By Deepak
                                        //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                        WriteBlob(HumanID, XMLObj.itemDoc,MySession, null, updtList, null, XMLObj,true);
                                        
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
                                }
                                //if (XMLObjEncounter.strXmlFilePath != null && XMLObjEncounter.strXmlFilePath != "")
                                if (XMLObjEncounter.itemDoc.InnerXml != null && XMLObjEncounter.itemDoc.InnerXml != "")
                                {
                                   // XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        //XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
                                        //WriteBlob(EncounterID, XMLObjEncounter.itemDoc, MySession, null, updtList, null, XMLObjEncounter, true);
                                        objTreatmentPlanMgr.WriteBlob(EncounterID, XMLObjEncounter.itemDoc, MySession, SaveTreatementPlanList, UpdateTreatmentPlanList, null, XMLObjEncounter, false);
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
                            }
                            catch (XmlException xmlexcep)
                            {
                                //CAP-1942
                                throw new Exception(xmlexcep.Message,xmlexcep);
                            }
                            catch (IOException ex)
                            {
                                //CAP-1942
                                throw new Exception(ex.Message,ex);
                            }
                        }
                        else
                        {
                            throw new Exception("Data inconsistency detected while saving. Please try again or notify support.");
                        }

                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        //CAP-1942
                        throw new Exception(ex.Message,ex);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
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
                throw new Exception(ex1.Message,ex1);
            }
            //if (updtList.Count > 0)
            //{
            //    foreach (var obj in updtList)
            //        obj.Version += 1;
            //    List<object> lstObj = updtList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlUpdate(lstObj, HumanID, string.Empty);
            //}
            //if (ordAssDelList.Count > 0)
            //{
            //    ulong encounterid = ordAssDelList[0].Encounter_ID;
            //    List<object> lstObj = ordAssDelList.Cast<object>().ToList();
            //    XMLObj.DeleteXmlNode(HumanID, lstObj, string.Empty);
            //}
            //if (ordAssSaveList.Count > 0)
            //{
            //    ulong encounterid = ordAssSaveList[0].Encounter_ID;
            //    List<object> lstObj = ordAssSaveList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlSaveStatic(lstObj, HumanID, string.Empty);
            //}
            if (EncounterID != 0)
            {
                return GetReferralOrderUsingEncounterID(EncounterID);
            }
            else
            {
                return GetReferralOrderUsingHumanID(updtList[0].Human_ID, updtList[0].From_Physician_ID, updtList[0].Encounter_ID);
            }
        }
        public ReferralOrderDTO DeleteReferralorders(ReferralOrder obj, IList<ReferralOrdersAssessment> ordAssDelList, ulong EncounterID, string MACAddress, string sOldText, ulong HumanID)
        {
            if (obj.Encounter_ID == 0)
            {
                obj = GetById(obj.Id);
            }
            IList<TreatmentPlan> SaveTreatmentPlanList = null;
            IList<TreatmentPlan> UpdateTreatmentPlanList = null;
            IList<TreatmentPlan> DeleteTreatmentPlanList = new List<TreatmentPlan>();
            IList<ReferralOrder> list = null;
            IList<ReferralOrder> delList = new List<ReferralOrder>();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            delList.Add(obj);
            iTryCount = 0;
            //string FileName = "Encounter" + "_" + EncounterID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            //XmlTextReader XmlText = null;
            try
            {
                #region Commented By Deepak
                // if (File.Exists(strXmlFilePath) == true)
                // {
                XmlDocument itemDoc = new XmlDocument();
                    //XmlText = new XmlTextReader(strXmlFilePath);
                    XmlNodeList xmlTagName = null;
                //itemDoc.Load(XmlText);
                //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //    itemDoc.Load(fs);

                //    XmlText.Close();
                //    #region Treatment_plan
                //    if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
                //    {
                //        xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("REFERRAL ORDER"))
                //                {

                //                    string TagName = xmlTagName[j].Name;
                //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
                //                    TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
                //                    IEnumerable<PropertyInfo> propInfo = null;
                //                    TreatmentPlan = (TreatmentPlan)TreatmentPlan;
                //                    propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {

                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            foreach (PropertyInfo property in propInfo)
                //                            {
                //                                if (property.Name == nodevalue.Name)
                //                                {
                //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                        property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                        property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                        property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                        property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
                //                                    else
                //                                        property.SetValue(TreatmentPlan, nodevalue.Value, null);
                //                                }
                //                            }
                //                        }

                //                    }
                //                    objTreatmentPlan.Add(TreatmentPlan);
                //                }
                //            }
                //        }
                //    }
                //#endregion
                #endregion
                IList<string> ilstEncounterTag = new List<string>();
                    ilstEncounterTag.Add("TreatmentPlanList");

                    IList<object> ilstEncounterBlobList = new List<object>();

                    ilstEncounterBlobList = ReadBlob(EncounterID, ilstEncounterTag);

                    if (ilstEncounterBlobList != null && ilstEncounterBlobList.Count > 0)
                    {
                        if (ilstEncounterBlobList[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstEncounterBlobList[0]).Count; iCount++)
                            {
                                objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstEncounterBlobList[0])[iCount]);
                            }
                        }
                    }

                                //fs.Close();
                                //fs.Dispose();
                                //}
                           // }
            }
            catch(Exception Ex)
            {
                //if (XmlText != null)
                //    XmlText.Close();
                throw Ex;
            }

        TryAgain:
            int iResult = 0;
            string sMyFinalFormatString = string.Empty;
            TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            TreatmentPlan UpdateTreatmentPlan = null;
            TreatmentPlan DeleteTreatmentPlan = null;
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjEncounter = new GenerateXml();

            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    if (EncounterID != 0)
                    {

                        IList<TreatmentPlan> delitemlst = objTreatmentPlan.Where(a => a.Source_ID == obj.Id).ToList<TreatmentPlan>();
                        if (delitemlst != null && delitemlst.Count > 0)
                            DeleteTreatmentPlan = delitemlst[0];

                        //IList<TreatmentPlan> Treatment_Plan = new List<TreatmentPlan>();
                        //Treatment_Plan = objTreatmentPlanMgr.GetTreatmentPlanUsingEncounterId(EncounterID, obj.Human_ID);
                        //IList<TreatmentPlan> Treatmentlst = (from t in Treatment_Plan where t.Plan_Type.ToUpper() == "REFERRAL ORDER" select t).ToList<TreatmentPlan>();
                        //if (Treatmentlst.Count > 0)
                        //{
                        //    if (Treatmentlst[0].Plan != string.Empty)
                        //    {

                        //        string sPlanFinal = Treatmentlst[0].Plan;
                        //        // string sPlanFinal = sPlan.Replace("* " + sOldText, "").Trim();
                        //        string RefOrderPlan = "* " + sOldText.Trim();
                        //        int from = sPlanFinal.IndexOf(RefOrderPlan);
                        //        string[] sNewlineSplittxt = { "\r\n" };
                        //        string Del_RefOrder_Plan = string.Empty;
                        //        if (from > -1)
                        //        {
                        //            Del_RefOrder_Plan = sPlanFinal.Substring(from).Split(sNewlineSplittxt, StringSplitOptions.None)[0];
                        //            sPlanFinal = sPlanFinal.Replace(Del_RefOrder_Plan.Trim() + Environment.NewLine, "").Replace(Del_RefOrder_Plan.Trim(), "");
                        //        }

                        //        if (sPlanFinal != string.Empty)
                        //        {
                        //            sPlanFinal = sPlanFinal + Environment.NewLine;
                        //        }
                        //        string refOrderFinal = sPlanFinal.Replace("\r", "").Replace("\n", "");
                        //        if (refOrderFinal == string.Empty)
                        //        {
                        //            DeleteTreatmentPlan = new TreatmentPlan();
                        //            DeleteTreatmentPlan = Treatmentlst[0];
                        //            DeleteTreatmentPlan.Modified_By = obj.Modified_By;
                        //            DeleteTreatmentPlan.Modified_Date_And_Time = obj.Modified_Date_And_Time;
                        //        }
                        //        else
                        //        {
                        //            UpdateTreatmentPlan = new TreatmentPlan();
                        //            UpdateTreatmentPlan = Treatmentlst[0];
                        //            UpdateTreatmentPlan.Plan = sPlanFinal;
                        //            UpdateTreatmentPlan.Modified_By = obj.Modified_By;
                        //            UpdateTreatmentPlan.Modified_Date_And_Time = obj.Modified_Date_And_Time;
                        //        }
                        //    }

                        //}
                        //if (Treatmentlst.Count > 0)
                        //{
                        //    if (Treatmentlst[0].Plan != string.Empty)
                        //    {

                        //        string sPlan = Treatmentlst[0].Plan;
                        //        string sPlanFinal = sPlan.Replace("* " + sOldText, "").Trim();
                        //        if (sPlanFinal != string.Empty)
                        //        {
                        //            sPlanFinal = sPlanFinal + Environment.NewLine;
                        //        }
                        //        if (sPlanFinal == string.Empty)
                        //        {
                        //            DeleteTreatmentPlan = new TreatmentPlan();
                        //            DeleteTreatmentPlan = Treatmentlst[0];
                        //            DeleteTreatmentPlan.Modified_By = obj.Modified_By;
                        //            DeleteTreatmentPlan.Modified_Date_And_Time = obj.Modified_Date_And_Time;
                        //        }
                        //        else
                        //        {
                        //            UpdateTreatmentPlan = new TreatmentPlan();
                        //            UpdateTreatmentPlan = Treatmentlst[0];
                        //            UpdateTreatmentPlan.Plan = sPlanFinal;
                        //            UpdateTreatmentPlan.Modified_By = obj.Modified_By;
                        //            UpdateTreatmentPlan.Modified_Date_And_Time = obj.Modified_Date_And_Time;
                        //        }
                        //    }

                        //}
                    }
                    try
                    {
                        if (ordAssDelList.Count > 0)
                        {
                            ReferralOrdersAssessmentManager objManager = new ReferralOrdersAssessmentManager();
                            IList<ReferralOrdersAssessment> SaveRefOrderAssList = null;
                            IList<ReferralOrdersAssessment> UpdateRefOrderAssList = null;
                            iResult = objManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveRefOrderAssList, ref UpdateRefOrderAssList, ordAssDelList, MySession, MACAddress, true, false, HumanID, string.Empty, ref XMLObj);
                            //iResult = objManager.BatchOperationsToReferralOrdersAssessment(null, null, ordAssDelList, MySession, MACAddress);
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

                        if (delList != null)
                        {
                            if (delList.Count > 0)
                            {
                                IList<ReferralOrder> UpdateReferralOrder = null;
                                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref list, ref UpdateReferralOrder, delList, MySession, MACAddress, true, false, HumanID, string.Empty, ref XMLObj);
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
                        }

                        if (UpdateTreatmentPlan != null || DeleteTreatmentPlan != null)
                        {
                            
                            if (DeleteTreatmentPlan != null)
                                DeleteTreatmentPlanList.Add(DeleteTreatmentPlan);


                            iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveTreatmentPlanList, ref UpdateTreatmentPlanList, DeleteTreatmentPlanList, MySession, MACAddress, true, true, EncounterID, string.Empty, ref XMLObjEncounter);


                            //if (UpdateTreatmentPlan != null)
                            //{
                            //    UpdateTreatementPlanList = new List<TreatmentPlan>();
                            //    UpdateTreatementPlanList.Add(UpdateTreatmentPlan);
                            //}
                            //if (DeleteTreatmentPlan != null)
                            //{
                            //    DeleteTreatementPlanList = new List<TreatmentPlan>();
                            //    DeleteTreatementPlanList.Add(DeleteTreatmentPlan);
                            //}
                            //iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveTreatementPlanList, ref UpdateTreatementPlanList, DeleteTreatementPlanList, MySession, MACAddress, true, true, EncounterID, string.Empty, ref XMLObjEncounter);
                            //iResult = objTreatmentPlanMgr.SaveUpdateorDeleteTreatmentPlan(null, UpdateTreatmentPlan, DeleteTreatmentPlan, MySession, MACAddress);
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
                        try
                        {
                            //if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                            if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                            {
                                //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                    //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    WriteBlob(HumanID, XMLObj.itemDoc, MySession,null,null, delList, XMLObj,false);
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
                            // if (XMLObjEncounter.strXmlFilePath != null && XMLObjEncounter.strXmlFilePath != "")
                            if (XMLObjEncounter.itemDoc.InnerXml != null && XMLObjEncounter.itemDoc.InnerXml != "")
                            {
                                
                                
                                    //XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
                                    int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                    //XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
                                   // WriteBlob(EncounterID, XMLObjEncounter.itemDoc, MySession, null, null, delList, XMLObjEncounter, false);
                                    objTreatmentPlanMgr.WriteBlob(EncounterID, XMLObjEncounter.itemDoc, MySession, SaveTreatmentPlanList, UpdateTreatmentPlanList, DeleteTreatmentPlanList, XMLObjEncounter, false);

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
                        }
                        catch (XmlException xmlexcep)
                        {
                            //CAP-1942
                            throw new Exception(xmlexcep.Message,xmlexcep);
                        }
                        catch (IOException ex)
                        {
                            //CAP-1942
                            throw new Exception(ex.Message,ex);
                        }
                    }
                    catch (NHibernate.Exceptions.GenericADOException ex)
                    {
                        trans.Rollback();
                        //CAP-1942
                        throw new Exception(ex.Message,ex);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
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
            //  GenerateXml XMLObj = new GenerateXml();
            //  if (delList.Count > 0)
            //  {
            //      ulong encounterid = delList[0].Encounter_ID;
            //      List<object> lstObj = delList.Cast<object>().ToList();
            //      XMLObj.DeleteXmlNode(HumanID, lstObj, string.Empty);
            //  }
            //if (ordAssDelList.Count > 0)
            //  {
            //      ulong encounterid = ordAssDelList[0].Encounter_ID;
            //      List<object> lstObj = ordAssDelList.Cast<object>().ToList();
            //      XMLObj.DeleteXmlNode(HumanID, lstObj, string.Empty);
            //  }            
            if (EncounterID != 0)
            {
                return GetReferralOrderUsingEncounterID(EncounterID);
            }
            else
            {
                return GetReferralOrderUsingHumanID(obj.Human_ID, obj.From_Physician_ID, obj.Encounter_ID);
            }
        }

        #endregion

        #region GetMethods

        public ReferralOrderDTO GetReferralOrderUsingReferralOrderID(ulong ReferralOrderID)
        {
            ReferralOrderDTO objOrdDTO = new ReferralOrderDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ReferralOrder)).Add(Expression.Eq("Id", ReferralOrderID));
                objOrdDTO.RefOrdList = crit.List<ReferralOrder>();

                ICriteria crit1 = iMySession.CreateCriteria(typeof(ReferralOrdersAssessment)).Add(Expression.Eq("Referral_Order_ID", ReferralOrderID));
                objOrdDTO.RefOrdAssList = crit1.List<ReferralOrdersAssessment>();
            }
            return objOrdDTO;
        }

        public ReferralOrderDTO GetReferralOrderUsingEncounterID(ulong EncounterID)
        {

            ReferralOrderDTO objOrdDTO = new ReferralOrderDTO();
            IList<ulong> ulList = new List<ulong>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ReferralOrdersAssessment objOrdAss = new ReferralOrdersAssessment();
                ICriteria crit = iMySession.CreateCriteria(typeof(ReferralOrder)).Add(Expression.Eq("Encounter_ID", EncounterID)).AddOrder(Order.Desc("Modified_Date_And_Time"));
                objOrdDTO.RefOrdList = crit.List<ReferralOrder>();
                //objOrdDTO.RefOrdCount = crit.List<ReferralOrder>().Count;

                foreach (ReferralOrder obj in objOrdDTO.RefOrdList)
                {
                    ulList.Add(obj.Id); 
                }
                if (ulList.Count > 0)
                {
                    ICriteria crit1 = iMySession.CreateCriteria(typeof(ReferralOrdersAssessment)).Add(Expression.In("Referral_Order_ID", ulList.ToArray<ulong>()));
                    objOrdDTO.RefOrdAssList = crit1.List<ReferralOrdersAssessment>();

                    //ICriteria crit2 = session.GetISession().CreateCriteria(typeof(ReferralOrdersProblemList)).Add(Expression.In("Referral_Order_ID", ulList.ToArray<ulong>()));
                    //objOrdDTO.RefOrdProbList = crit2.List<ReferralOrdersProblemList>();
                }
                //TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
                //objTreatmentPlanMgr.GetTreatmentPlanUsingEncounterId(EncounterID, objOrdDTO.objHuman.Human_ID);
                //objOrdDTO.Treatment_Plan = objTreatmentPlanMgr.GetTreatmentPlan(EncounterID);
                //ICriteria crt = iMySession.CreateCriteria(typeof(TreatmentPlan)).Add(Expression.Eq("Encounter_Id", EncounterID));
                //objOrdDTO.Treatment_Plan = crt.List<TreatmentPlan>();
                iMySession.Close();
            }
            return objOrdDTO;

        }

        public ReferralOrderDTO GetReferralOrderUsingHumanID(ulong HumanID,ulong PhysicianID,ulong EncounterID) //DateTime creadtedDate)
        {
            ReferralOrderDTO objOrdDTO = new ReferralOrderDTO();
            IList<ulong> ulList = new List<ulong>();
            ReferralOrdersAssessment objOrdAss = new ReferralOrdersAssessment();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ReferralOrder)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("From_Physician_ID", PhysicianID)).Add(Expression.Eq("Encounter_ID", EncounterID));
                objOrdDTO.RefOrdList = crit.List<ReferralOrder>();
                foreach (ReferralOrder obj in objOrdDTO.RefOrdList)
                {
                    ulList.Add(obj.Id);
                }

                if (ulList.Count > 0)
                {
                    ICriteria crit1 = iMySession.CreateCriteria(typeof(ReferralOrdersAssessment)).Add(Expression.In("Referral_Order_ID", ulList.ToArray<ulong>()));
                    objOrdDTO.RefOrdAssList = crit1.List<ReferralOrdersAssessment>();
                }
                iMySession.Close();
            }
            return objOrdDTO;
        }

        public ReferralOrderDTO LoadReferralOrder(ulong HumanID, ulong EncounterID,ulong PhysicianID)
        {
            ReferralOrderDTO objDTO = new ReferralOrderDTO();
            if (EncounterID != 0)
            {
                objDTO = GetReferralOrderUsingEncounterID(EncounterID);
            }
            else
            {
                objDTO = GetReferralOrderUsingHumanID(HumanID,PhysicianID,EncounterID);
            }
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
               // ICriteria criteria = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Assessment_Type", "Selected")).Add(Expression.Eq("Version_Year", "ICD_10")).AddOrder(Order.Desc("Primary_Diagnosis"));
                ICriteria criteria = iMySession.CreateCriteria(typeof(Assessment)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("Assessment_Type", "Selected")).Add(Expression.Eq("Version_Year", "ICD_10")).AddOrder(Order.Desc("Primary_Diagnosis"));
                objDTO.AssessmentList = criteria.List<Assessment>();

                ICriteria crt = iMySession.CreateCriteria(typeof(ProblemList)).Add(Expression.Eq("Human_ID", HumanID)).Add(Expression.Eq("Status", "Active")).Add(Expression.Eq("Is_Active", "Y")).Add(Expression.Eq("Version_Year", "ICD_10")).Add(Expression.Not(Expression.Eq("ICD", "0000")));
                objDTO.MedAdvProbList = crt.List<ProblemList>();

                IList<PatientResults> objPatientList = new List<PatientResults>();
                IList<ProblemList> ProblemlistVitals = new List<ProblemList>();
                IList<AllICD_9> allICD9ForVitalsProblemListPFSH = new List<AllICD_9>();
                AllICD_9Manager objAllIcdMgr = new AllICD_9Manager();
                //ISQLQuery sqlquery = session.GetISession().CreateSQLQuery("select v.* from patient_results v inner join dynamic_screen s on (v.Loinc_Observation=s.control_Name)   where v.encounter_id='" + encounterID + "' and v.human_id='" + humanID + "' and Results_Type='Vitals' and v.Vitals_group_id in(select max(Vitals_group_id) from patient_results where human_id=v.human_id and encounter_id= v.encounter_id) group by v.Loinc_Observation order by s.sort_order").AddEntity("v", typeof(PatientResults));
                ArrayList aryPatientResultList;
                IQuery query1 = iMySession.GetNamedQuery("Get.PatientResultsList.Test");
                query1.SetString(0, EncounterID.ToString());
                query1.SetString(1, HumanID.ToString());
                aryPatientResultList = new ArrayList(query1.List());
                foreach (object[] oj in aryPatientResultList)
                {
                    PatientResults objPatientResults = new PatientResults();
                    objPatientResults.Id = Convert.ToUInt32(oj[0]);
                    objPatientResults.Loinc_Observation = oj[1].ToString();
                    objPatientResults.Value = oj[2].ToString();
                    objPatientResults.Units = oj[3].ToString();
                    objPatientList.Add(objPatientResults);

                }

                if (objDTO.MedAdvProbList.Count > 0)
                {
                    IList<AssessmentVitalsLookup> AssessmentVitalsLookupList = new List<AssessmentVitalsLookup>();
                    IList<AssessmentVitalsLookup> AssessmentVitalLookuplst = new List<AssessmentVitalsLookup>();

                    ICriteria crit2 = iMySession.CreateCriteria(typeof(AssessmentVitalsLookup));
                    AssessmentVitalsLookupList = crit2.List<AssessmentVitalsLookup>();
                    int count = 0;
                    IList<string> VitalsBasedICD_List = new List<string>();
                    AssessmentVitalsLookupManager objAssessVitalsMngr = new AssessmentVitalsLookupManager();
                    VitalsBasedICD_List = objAssessVitalsMngr.GetIcdBasedOnVitals(AssessmentVitalsLookupList, objPatientList);//objPatientList : vitals data for current encounter
                    string[] VitalsICD = new string[AssessmentVitalsLookupList.Count];
                    int k = 0;
                    string[] prblVitalICD = new string[objDTO.MedAdvProbList.Count];

                    for (int i = 0; i < AssessmentVitalsLookupList.Count; i++)
                    {
                        if (AssessmentVitalsLookupList[i].ICD_10.Trim() != "")
                            VitalsICD[i] = AssessmentVitalsLookupList[i].ICD_10;
                    }

                    if (VitalsBasedICD_List.Count > 0)
                    {
                        for (int i = 0; i < objDTO.AssessmentList.Count; i++)
                        {
                            if (VitalsBasedICD_List.IndexOf(objDTO.AssessmentList[i].ICD) > -1)
                            {
                                count = 1;
                                break;
                            }
                        }
                        for (int i = 0; i < objDTO.MedAdvProbList.Count; i++)
                        {
                            if (VitalsICD.Contains(objDTO.MedAdvProbList[i].ICD) && VitalsBasedICD_List.IndexOf(objDTO.MedAdvProbList[i].ICD) == -1)
                            {
                                prblVitalICD[k] = objDTO.MedAdvProbList[i].ICD;
                                k++;

                            }
                            if (VitalsBasedICD_List.IndexOf(objDTO.MedAdvProbList[i].ICD) > -1)
                            {
                                ProblemlistVitals.Add(objDTO.MedAdvProbList[i]);
                            }
                        }

                        if (k != 0)
                        {
                            for (int i = 0; i < k; i++)
                            {
                                // ProblemlistVitals.Add(objDiagnosticDTO.MedAdvProblemList.First(a => a.ICD == prblVitalICD[i]));
                                objDTO.MedAdvProbList.Remove(objDTO.MedAdvProbList.First(a => a.ICD == prblVitalICD[i]));

                            }
                        }

                        if (AssessmentVitalsLookupList.Count > 0 && count == 0)
                        {
                            AssessmentVitalLookuplst = (from val in AssessmentVitalsLookupList
                                                        where (val.Field_Name == "BMI" || val.Field_Name == "BMI PERCENTILE" || val.Field_Name == "BMI STATUS")
                                                        select val).Distinct().ToList();
                            if (AssessmentVitalLookuplst.Count > 0)
                            {
                                objDTO.AssessmentList = (from a in objDTO.AssessmentList where !AssessmentVitalLookuplst.Any(b => b.ICD_10 == a.ICD) select a).ToList();
                                objDTO.MedAdvProbList = (from a in objDTO.MedAdvProbList where !AssessmentVitalLookuplst.Any(b => b.ICD_10 == a.ICD) select a).ToList();

                            }

                        }
                    }
                    foreach (ProblemList obj in ProblemlistVitals)
                        objDTO.MedAdvProbList.Add(obj);
                }

                iMySession.Close();
            }
            return objDTO;
        }

        public ReferralOrderDTO GetReferralOrderUsingEncounterIDWithoutPageNavigator(ulong EncounterID)
        {
            ReferralOrderDTO objOrdDTO = new ReferralOrderDTO();
            IList<ulong> ulList = new List<ulong>();
            ReferralOrdersAssessment objOrdAss = new ReferralOrdersAssessment();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ReferralOrder)).Add(Expression.Eq("Encounter_ID", EncounterID)).AddOrder(Order.Desc("Modified_Date_And_Time"));
                objOrdDTO.RefOrdList = crit.List<ReferralOrder>();
                foreach (ReferralOrder obj in objOrdDTO.RefOrdList)
                {
                    ulList.Add(obj.Id);
                }
                if (ulList.Count > 0)
                {
                    ICriteria crit1 = iMySession.CreateCriteria(typeof(ReferralOrdersAssessment)).Add(Expression.In("Referral_Order_ID", ulList.ToArray<ulong>()));
                    objOrdDTO.RefOrdAssList = crit1.List<ReferralOrdersAssessment>();
                }
                iMySession.Close();
            }
            return objOrdDTO;
        }
        #endregion


        public IList<FillOrdersManagementDTO> SearchReferralOrder(int orderInDays, DateTime fromDate, DateTime toDate, ulong humanID, ulong physicianID, int pageNumber, int maxResults, string order_type, string order_status, string FacilityName)
        {
            IList<FillOrdersManagementDTO> ilstFillOrdersManagementDTO = new List<FillOrdersManagementDTO>();
            FillOrdersManagementDTO objFillOrdersManagementDTO = new FillOrdersManagementDTO();
            IList<ReferralOrder> referralOrderList = new List<ReferralOrder>();
            IList<ulong> referralOrderIdList = new List<ulong>();
            IList<ReferralOrdersAssessment> referralOrderAssessmentList = new List<ReferralOrdersAssessment>();
            string human = string.Empty;
            string physician = string.Empty;
            ArrayList arr = null;
            if (humanID == 0)
            {
                human = "%";
            }
            else
            {
                human = humanID.ToString();
            }
            if (physicianID == 0)
            {
                physician = "%";
            }
            else
            {
                physician = physicianID.ToString();
            }
            IQuery query;
            //Cap - 2622
            //int startIndex = 0;
            //startIndex = ((pageNumber - 1) * maxResults);
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                query = iMySession.GetNamedQuery("Fill.SearchOrderWithReferral");

                if (order_status == string.Empty)
                    order_status = "%";
                //CAP-2299
                query.SetString(0, FacilityName);
                query.SetString(1, order_type);
                query.SetString(2, order_status);
                query.SetString(3, fromDate.ToString("yyyy-MM-dd"));
                query.SetString(4, toDate.ToString("yyyy-MM-dd"));
                query.SetString(5, physician);
                query.SetString(6, human);
                arr = new ArrayList(query.List());
                if (arr != null)
                {
                    for (int i = 0; i < arr.Count; i++)
                    {
                        objFillOrdersManagementDTO = new FillOrdersManagementDTO();
                        object[] obj = (object[])arr[i];
                        objFillOrdersManagementDTO.Group_ID = Convert.ToUInt64(obj[0]);
                        objFillOrdersManagementDTO.Order_Type = obj[1]?.ToString();
                        objFillOrdersManagementDTO.Facility_Name = obj[2]?.ToString();
                        objFillOrdersManagementDTO.Current_Process = obj[3]?.ToString();
                        objFillOrdersManagementDTO.Ordered_Date_And_Time = Convert.ToDateTime(obj[4] ?? DateTime.MinValue);
                        objFillOrdersManagementDTO.Human_Id = (obj[5].ToString());
                        objFillOrdersManagementDTO.Human_Name = obj[6].ToString();
                        objFillOrdersManagementDTO.Date_Of_Birth = Convert.ToDateTime(obj[7]);
                        objFillOrdersManagementDTO.Physician_ID = Convert.ToUInt64(obj[8]);
                        objFillOrdersManagementDTO.Procedures = obj[9].ToString();
                        objFillOrdersManagementDTO.To_Physician_Name = obj[11].ToString();
                        objFillOrdersManagementDTO.To_Facility_Name = obj[12].ToString();
                        objFillOrdersManagementDTO.Total_Record_Found = Convert.ToUInt64(arr.Count);
                        objFillOrdersManagementDTO.Temp_Property = obj[13].ToString();
                        //Added By Saravanakumar
                        if (obj[14] != null)
                            objFillOrdersManagementDTO.Ordering_Provider = obj[14].ToString();
                        ilstFillOrdersManagementDTO.Add(objFillOrdersManagementDTO);
                        referralOrderIdList.Add(Convert.ToUInt64(obj[13]));
                    }
                    ICriteria crit1 = iMySession.CreateCriteria(typeof(ReferralOrdersAssessment)).Add(Expression.In("Referral_Order_ID", referralOrderIdList.ToArray<ulong>()));
                    IList<ReferralOrdersAssessment> ilstReferralOrdersAssessment = crit1.List<ReferralOrdersAssessment>();
                    foreach (FillOrdersManagementDTO obj in ilstFillOrdersManagementDTO)
                    {
                        var temp = from rec in ilstReferralOrdersAssessment where rec.Referral_Order_ID == Convert.ToUInt64(obj.Temp_Property) select rec;
                        foreach (var rec in temp)
                            obj.Assessment += rec.ICD + "-" + rec.Assessment_Description;
                    }
                }
                iMySession.Close();
            }
            return ilstFillOrdersManagementDTO;
        }
      public ulong GetHumanID(IList<ReferralOrder> savelist, IList<ReferralOrder> updatelist, IList<ReferralOrder> deletelist)
        {
            ulong ulHumanID = 0;
            if (savelist != null && savelist.Count > 0)
                return savelist[0].Human_ID;
            if (updatelist != null && updatelist.Count > 0)
                return updatelist[0].Human_ID;
            if (deletelist != null && deletelist.Count > 0)
                return deletelist[0].Human_ID;
            return ulHumanID;
        }
      public ReferralOrderDTO FillReferralOrder(ulong HumanId,ulong Physician_Id,ulong EncounterID)
      {
          ReferralOrderDTO objReferralOrderDTO = new ReferralOrderDTO();
          IList<object> grpId = new List<object>();
          using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
          {
              ICriteria ReferralCrit = iMySession.CreateCriteria(typeof(ReferralOrder)).Add(Expression.Eq("Encounter_ID", EncounterID)).Add(Expression.Eq("From_Physician_ID", Physician_Id)).Add(Expression.Eq("Human_ID", HumanId)).AddOrder(Order.Desc("Modified_Date_And_Time"));
              objReferralOrderDTO.RefOrdList = ReferralCrit.List<ReferralOrder>();
              ICriteria critGrpID = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", "REFERRAL ORDER"))
                .SetProjection(Projections.Max("Obj_System_Id"));
              grpId = critGrpID.List<object>();
              if (grpId != null && grpId.Count > 0)
                  objReferralOrderDTO.MaxGroupId = Convert.ToUInt64(grpId[0]);
              else
                  objReferralOrderDTO.MaxGroupId = 0;
              iMySession.Close();
          }
          return objReferralOrderDTO;
      }


      public object SubmitReferralOrder(IList<ReferralOrder> referralOrderList, string FacilityName, string MedAsstName, string MACAddress)
      {
          GenerateXml XMLObj = new GenerateXml();
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

                      if (referralOrderList != null)
                      {
                          if (referralOrderList.Count > 0)
                          {
                              IList<ReferralOrder> list = null;
                              iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref list, ref referralOrderList, null, MySession, MACAddress, true, false, referralOrderList[0].Human_ID, string.Empty, ref XMLObj);
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

                              WFObject WFObj = new WFObject();
                              WFObj.Obj_Type = "REFERRAL ORDER";
                              WFObj.Current_Arrival_Time = referralOrderList[0].Modified_Date_And_Time;
                              WFObj.Current_Owner = MedAsstName;
                              WFObj.Fac_Name = FacilityName;
                              WFObj.Parent_Obj_Type = string.Empty;
                              WFObj.Obj_System_Id = referralOrderList[0].Referral_Order_Group_ID;
                              WFObj.Current_Process = "START";
                              WFObjectManager objWfMnger = new WFObjectManager();
                              if (referralOrderList[0].Move_To_MA == "Y")
                              {
                                  iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
                              }
                              else
                              {
                                  iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 2, MACAddress, MySession);

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
                                      throw new Exception("Deadlock occurred. Transaction failed.");
                                  }
                              }
                              else if (iResult == 1)
                              {
                                  trans.Rollback();
                                  throw new Exception("Exception occurred. Transaction failed.");

                              }
                          }

                            //if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                            if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                            {
                             // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                              int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                    //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    WriteBlob(referralOrderList[0].Human_ID, XMLObj.itemDoc, MySession, null, referralOrderList, null, XMLObj, false);
                                    trans.Commit();
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
                      }
                  }
                  catch (NHibernate.Exceptions.GenericADOException ex)
                  {
                      trans.Rollback();
                        //CAP-1942
                        throw new Exception(ex.Message,ex);
                  }
                  catch (Exception e)
                  {
                      trans.Rollback();
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
                throw new Exception(ex1.Message,ex1);
          }
          return (object)referralOrderList[0].Referral_Order_Group_ID;
      }

      public void SaveReferalOrderforSummary(IList<ReferralOrder> lstreferalorder)
      {
          IList<ReferralOrder> UpdateReferralOrder = null;
          SaveUpdateDelete_DBAndXML_WithTransaction(ref lstreferalorder, ref UpdateReferralOrder, null, string.Empty,false,false,0,string.Empty);
      }

    }
}
