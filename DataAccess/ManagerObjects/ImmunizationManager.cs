using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IImmunizationManager : IManagerBase<Immunization, ulong>
    {
        ImmunizationDTO InsertToImmunization(Immunization immun, ImmunizationHistory immunHist, ulong EncounterID, string MACAddress, string sImmunization, string sLocalTime);
        ImmunizationDTO InsertToImmunizationAndHistory(Immunization immun, IList<ImmunizationHistory> immunHist, ulong EncounterID, string MACAddress, string sImmunization, string sLocalTime);//Add Bug ID:66113 
        ImmunizationDTO UpdateToImmunization(Immunization immun, ImmunizationHistory immunHist, ulong EncounterID, string MACAddress, string sPlanText, string sImmunization, bool IsFileMgnt_delete, string sLocalTime);
        ImmunizationDTO DeleteImmunization(Immunization immun, ImmunizationHistory immunHist, ulong EncounterID, string MACAddress, string sPln);
        //ImmunizationDTO LoadImmunization(ulong HumanId, ulong PhysicianID, ulong EncounterID, DateTime todaysDate);
        ImmunizationDTO FillImmunization(ulong HumanId, ulong EncounterID);
        ImmunizationDTO GetImmunizationUsingHumanID(ulong HumanId, DateTime createdDate, ulong PhysicianId);
        IList<FillOrdersManagementDTO> SearchImmunizationOrder(string order_type, string order_status, string FacilityName, int orderInDays, DateTime fromDate, DateTime toDate, ulong humanID, ulong physicianID, int pageNumber, int maxResults);
        int BatchOperationsToImmunization(IList<Immunization> savelist, IList<Immunization> updtList, IList<Immunization> delList, ISession MySession, string MACAddress);
        object SubmitImmunization(IList<Immunization> immunizationList, string MACAddress);
        void SaveImmunizationforSummary(IList<Immunization> lstimmunization);
        IList<Immunization> GetImmunizationUsingHumanIDEncID(ulong HumanId, ulong EncounterID);
        IList<Immunization> GetImmunizationUsingGroupID(ulong ulImmunGroupID);
    }
    public partial class ImmunizationManager : ManagerBase<Immunization, ulong>, IImmunizationManager
    {
        #region Constructors

        public ImmunizationManager()
            : base()
        {

        }
        public ImmunizationManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods

        int iTryCount = 0;

        public ImmunizationDTO InsertToImmunization(Immunization immun, ImmunizationHistory immunHist, ulong EncounterID, string MACAddress, string sImmunization,string sLocalTime)
        {
            IList<Immunization> immulist = new List<Immunization>();
            IList<ImmunizationHistory> ImnuHistorysaveList = new List<ImmunizationHistory>();
            immulist.Add(immun);
            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjEncounter = new GenerateXml();

            ulong ulMyEncounterID = immun.Encounter_Id;
            TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();

            string FileMgnt_ID = string.Empty;
            Session.GetISession().Clear();
            ISession MySession = Session.GetISession();
            try
            {
                bool IsImmunization = true, IsImmunizationHistory = true, IsTreatmentPlanConsistent = true;
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if (immulist != null && immulist.Count > 0)
                        {
                            IList<Immunization> UpdateImmunization = new List<Immunization>();
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref immulist, ref UpdateImmunization, null, MySession, MACAddress, true, true, immulist[0].Human_ID, string.Empty, ref XMLObj);
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
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                            IsImmunization = XMLObj.CheckDataConsistency(immulist.Concat(UpdateImmunization).Cast<object>().ToList(), true, string.Empty);
                        }
                        if (immunHist != null && immun.Is_Administration_Refused != "Y")//if Administration Refused Do not save in ImmHis Table
                        {
                            IList<ImmunizationHistory> UpdateImmunizationHistory = new List<ImmunizationHistory>();
                            ImmunizationHistoryManager ImmunizationHistoryMngr = new ImmunizationHistoryManager();
                            if (immulist != null && immulist.Count > 0)
                            {
                                immunHist.Immunization_Order_ID = immulist[0].Id;
                                immunHist.Created_By = immulist[0].Created_By;
                                immunHist.Created_Date_And_Time = immulist[0].Created_Date_And_Time;
                                if (immunHist.Procedure_Code.Trim() == "90732" || immunHist.Procedure_Code.Trim() == "90670")
                                {
                                    immunHist.Snomed_Code = "310578008";
                                }
                                else
                                {
                                    immunHist.Snomed_Code = "";
                                }
                            }

                            ImnuHistorysaveList.Add(immunHist);

                           // iResult = ImmunizationHistoryMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ImnuHistorysaveList, ref UpdateImmunizationHistory, null, MySession, MACAddress, true, true, immunHist.Human_ID, string.Empty, ref XMLObj);
                          ImmunizationHistoryMngr.InsertIntoImmunizationHistoryFromOrders(null, ImnuHistorysaveList,ImnuHistorysaveList[0].Human_ID,0,0,string.Empty,0,false,MySession,ref XMLObj);       
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
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                            IsImmunizationHistory = XMLObj.CheckDataConsistency(ImnuHistorysaveList.Concat(UpdateImmunizationHistory).Cast<object>().ToList(), true, string.Empty);
                        }
                        #region Treatment plan
                        if (ulMyEncounterID != 0 && immun.Is_Administration_Refused != "Y")//if Administration Refused Do not save in GeneralPlan Table
                        {
                            TreatmentPlan item = new TreatmentPlan();
                            item.Human_ID = immun.Human_ID;
                            item.Encounter_Id = EncounterID;
                            item.Physician_Id = immun.Physician_Id;
                            item.Created_By = immun.Created_By;
                            item.Created_Date_And_Time = immun.Created_Date_And_Time;
                            item.Plan_Type = "IMMUNIZATION/INJECTION";
                            string plan_txt = string.Empty;
                            plan_txt = "* " + sImmunization;
                            item.Plan = plan_txt;
                            item.Version = 0;
                            item.Source_ID = immun.Id;
                            item.Local_Time = sLocalTime;
                            Insert_Tplan.Add(item);
                        }
                        if (Insert_Tplan != null && Insert_Tplan.Count > 0)
                        {

                           iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, null, session.GetISession(), MACAddress, true, true, ulMyEncounterID, string.Empty, ref XMLObjEncounter);
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

                            IsTreatmentPlanConsistent = XMLObjEncounter.CheckDataConsistency(Insert_Tplan.Cast<object>().ToList(), true, string.Empty);
                        }
                        #endregion

                        if (IsImmunization && IsImmunizationHistory && IsTreatmentPlanConsistent)
                        {
                            try
                            {
                                if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                                {
                                   // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
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
                                if (XMLObjEncounter.strXmlFilePath != null && XMLObjEncounter.strXmlFilePath != "")
                                {
                                 //   XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);

                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
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
                                //session.GetISession().BeginTransaction(System.Data.IsolationLevel.ReadUncommitted).Commit();
                            }
                            catch (XmlException xmlexcep)
                            {
                                throw new Exception(xmlexcep.Message);
                            }
                            catch (IOException ex)
                            {
                                throw new Exception(ex.Message);
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
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        if (MySession.IsOpen)
                            MySession.Close();
                        else
                            session.GetISession().Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (EncounterID != 0)
            {
                return FillImmunization(immun.Human_ID, EncounterID);
            }
            else
            {
                return GetImmunizationUsingHumanID(immun.Human_ID, immun.Created_Date_And_Time, immun.Physician_Id);
            }
        }

        //Add BugID: 66113 
        public ImmunizationDTO InsertToImmunizationAndHistory(Immunization immun, IList<ImmunizationHistory> immunHist, ulong EncounterID, string MACAddress, string sImmunization,string sLocalTime)
        {
            IList<Immunization> immulist = new List<Immunization>();
            IList<ImmunizationHistory> ImnuHistorysaveList = new List<ImmunizationHistory>();
            immulist.Add(immun);
            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjEncounter = new GenerateXml();

            ulong ulMyEncounterID = immun.Encounter_Id;
            TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();

            string FileMgnt_ID = string.Empty;
            Session.GetISession().Clear();
            //ISession MySession = Session.GetISession();
            using (ISession MySession = Session.GetISession())
            {
                try
                {
                    bool IsImmunization = true, IsImmunizationHistory = true, IsTreatmentPlanConsistent = true;
                    using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        try
                        {
                            if (immulist != null && immulist.Count > 0)
                            {
                                IList<Immunization> UpdateImmunization = new List<Immunization>();
                                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref immulist, ref UpdateImmunization, null, MySession, MACAddress, true, true, immulist[0].Human_ID, string.Empty, ref XMLObj);
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
                                        throw new Exception("Deadlock is occured. Transaction failed");
                                    }
                                }
                                else if (iResult == 1)
                                {
                                    trans.Rollback();
                                    throw new Exception("Exception is occured. Transaction failed");
                                }
                                IsImmunization = XMLObj.CheckDataConsistency(immulist.Concat(UpdateImmunization).Cast<object>().ToList(), true, string.Empty);
                            }
                            if (immunHist != null && immunHist.Count > 0 && immun.Is_Administration_Refused != "Y")//if Administration Refused Do not save in ImmHis Table
                            {
                                IList<ImmunizationHistory> UpdateImmunizationHistory = new List<ImmunizationHistory>();
                                ImmunizationHistoryManager ImmunizationHistoryMngr = new ImmunizationHistoryManager();
                                if (immulist != null && immulist.Count > 0)
                                {
                                    immunHist[0].Immunization_Order_ID = immulist[0].Id;
                                    immunHist[0].Created_By = immulist[0].Created_By;
                                    immunHist[0].Created_Date_And_Time = immulist[0].Created_Date_And_Time;
                                    if (immunHist[0].Procedure_Code.Trim() == "90732" || immunHist[0].Procedure_Code.Trim() == "90670")
                                    {
                                        immunHist[0].Snomed_Code = "310578008";
                                    }
                                    else
                                    {
                                        immunHist[0].Snomed_Code = "";
                                    }
                                }

                                for (int i = 0; i < immunHist.Count; i++)
                                {
                                    ImnuHistorysaveList.Add(immunHist[i]);
                                }

                                // iResult = ImmunizationHistoryMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref ImnuHistorysaveList, ref UpdateImmunizationHistory, null, MySession, MACAddress, true, true, immunHist.Human_ID, string.Empty, ref XMLObj);
                                ImmunizationHistoryMngr.InsertIntoImmunizationHistoryFromOrders(null, ImnuHistorysaveList, ImnuHistorysaveList[0].Human_ID, 0, 0, string.Empty, 0, false, MySession, ref XMLObj);
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
                                        throw new Exception("Deadlock is occured. Transaction failed");
                                    }
                                }
                                else if (iResult == 1)
                                {
                                    trans.Rollback();
                                    throw new Exception("Exception is occured. Transaction failed");
                                }
                                IsImmunizationHistory = XMLObj.CheckDataConsistency(ImnuHistorysaveList.Concat(UpdateImmunizationHistory).Cast<object>().ToList(), true, string.Empty);
                            }
                            #region Treatment plan
                            if (ulMyEncounterID != 0 && immun.Is_Administration_Refused != "Y")//if Administration Refused Do not save in GeneralPlan Table
                            {
                                TreatmentPlan item = new TreatmentPlan();
                                item.Human_ID = immun.Human_ID;
                                item.Encounter_Id = EncounterID;
                                item.Physician_Id = immun.Physician_Id;
                                item.Created_By = immun.Created_By;
                                item.Created_Date_And_Time = immun.Created_Date_And_Time;
                                item.Plan_Type = "IMMUNIZATION/INJECTION";
                                string plan_txt = string.Empty;
                                plan_txt = "* " + sImmunization;
                                item.Plan = plan_txt;
                                item.Version = 0;
                                item.Source_ID = immun.Id;
                                item.Local_Time = sLocalTime;
                                Insert_Tplan.Add(item);
                            }
                            if (Insert_Tplan != null && Insert_Tplan.Count > 0)
                            {

                                iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, null, session.GetISession(), MACAddress, true, true, ulMyEncounterID, string.Empty, ref XMLObjEncounter);
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

                                IsTreatmentPlanConsistent = XMLObjEncounter.CheckDataConsistency(Insert_Tplan.Cast<object>().ToList(), true, string.Empty);
                            }
                            #endregion

                            if (IsImmunization && IsImmunizationHistory && IsTreatmentPlanConsistent)
                            {
                                try
                                {
                                    //  if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                                    if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                                    {
                                        //  XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);

                                        int trycount = 0;
                                    trytosaveagain:
                                        try
                                        {
                                            // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                            WriteBlob(ImnuHistorysaveList[0].Human_ID, XMLObj.itemDoc, MySession, null, immulist, null, XMLObj, false);

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
                                        //   XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);

                                        int trycount = 0;
                                    trytosaveagain:
                                        try
                                        {
                                            // XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);

                                            objTreatmentPlanMgr.WriteBlob(EncounterID, XMLObjEncounter.itemDoc, MySession, Insert_Tplan, Update_Tplan, Delete_Tplan, XMLObjEncounter, false);
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
                                    //session.GetISession().BeginTransaction(System.Data.IsolationLevel.ReadUncommitted).Commit();
                                }
                                catch (XmlException xmlexcep)
                                {
                                    throw new Exception(xmlexcep.Message);
                                }
                                catch (IOException ex)
                                {
                                    throw new Exception(ex.Message);
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
                            throw new Exception(ex.Message);
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            throw new Exception(e.Message);
                        }
                        finally
                        {
                            if (MySession.IsOpen)
                                MySession.Close();
                            else
                                session.GetISession().Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            if (EncounterID != 0)
            {
                return FillImmunization(immun.Human_ID, EncounterID);
            }
            else
            {
                return GetImmunizationUsingHumanID(immun.Human_ID, immun.Created_Date_And_Time, immun.Physician_Id);
            }
        }

        public ImmunizationDTO UpdateToImmunization(Immunization immun, ImmunizationHistory immunHist, ulong EncounterID, string MACAddress, string sPlanText, string sImmunization, bool IsFileMgnt_delete, string sLocalTime)
        {
            IList<Immunization> immulist = new List<Immunization>();
            IList<ImmunizationHistory> updtImmunHisList = new List<ImmunizationHistory>();
            IList<ImmunizationHistory> deleteImmunHisList = new List<ImmunizationHistory>();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            IList<TreatmentPlan> SaveTreatementPlanList = new List<TreatmentPlan>();
            IList<TreatmentPlan> UpdateTreatmentPlanList = new List<TreatmentPlan>();
            IList<TreatmentPlan> DeleteTreatmentPlanList = new List<TreatmentPlan>();

            immulist.Add(immun);
            IList<Immunization> addList = null;
            iTryCount = 0;
            #region TplanGet
            string FileName = "Encounter" + "_" + EncounterID + ".xml";
            string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            XmlTextReader XmlText = null;
            try
            {
                if (File.Exists(strXmlFilePath) == true)
                {
                    XmlDocument itemDoc = new XmlDocument();
                    XmlText = new XmlTextReader(strXmlFilePath);
                    XmlNodeList xmlTagName = null;
                    // itemDoc.Load(XmlText);
                    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        itemDoc.Load(fs);

                        XmlText.Close();
                        #region Treatment_plan
                        if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
                        {
                            xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

                            if (xmlTagName.Count > 0)
                            {
                                for (int j = 0; j < xmlTagName.Count; j++)
                                {
                                    if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("IMMUNIZATION/INJECTION"))
                                    {

                                        string TagName = xmlTagName[j].Name;
                                        XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
                                        TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
                                        IEnumerable<PropertyInfo> propInfo = null;
                                        propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

                                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                                        {

                                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
                                            {
                                                foreach (PropertyInfo property in propInfo)
                                                {
                                                    if (property.Name == nodevalue.Name)
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                                                            property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                                                            property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                                                            property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
                                                        else
                                                            property.SetValue(TreatmentPlan, nodevalue.Value, null);
                                                    }
                                                }
                                            }

                                        }
                                        objTreatmentPlan.Add(TreatmentPlan);
                                    }
                                }
                            }
                        }
                        #endregion
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
            catch(Exception Ex)
            {
                if (XmlText != null)
                    XmlText.Close();
                throw Ex;
            }
            #endregion

        TryAgain:
            int iResult = 0;
            ulong ulMyEncounterID = immun.Encounter_Id;
            TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            TreatmentPlan UpdateTreatmentPlan = null;
            TreatmentPlan InsertTreatmentPlan = null;
            if (ulMyEncounterID != 0)
            {

                IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Source_ID == immun.Id).ToList<TreatmentPlan>();
                if (itemlst != null && itemlst.Count > 0)
                {
                    TreatmentPlan item = itemlst[0];
                    item.Plan = "* " + sImmunization;
                    item.Modified_By = immun.Modified_By;
                    item.Modified_Date_And_Time = immun.Modified_Date_And_Time;
                    UpdateTreatmentPlan = item;
                }
                else if (ulMyEncounterID != 0 && immun.Is_Administration_Refused != "Y")//if Administration Refused Do not save in GeneralPlan Table
                {
                    TreatmentPlan item = new TreatmentPlan();
                    item.Human_ID = immun.Human_ID;
                    item.Encounter_Id = EncounterID;
                    item.Physician_Id = immun.Physician_Id;
                    item.Created_By = immun.Created_By;
                    item.Created_Date_And_Time = immun.Created_Date_And_Time;
                    item.Plan_Type = "IMMUNIZATION/INJECTION";
                    string plan_txt = string.Empty;
                    plan_txt = "* " + sImmunization;
                    item.Plan = plan_txt;
                    item.Version = 0;
                    item.Source_ID = immun.Id;
                    item.Local_Time = sLocalTime;
                    InsertTreatmentPlan = item;
                }
            }
            Session.GetISession().Clear();
            string FileMgnt_ID = string.Empty;
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjEncounter = new GenerateXml();
            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        bool IsImmunization = true, IsImmunizationHistory = true, IsTreatmentPlanConsistent = true;
                        if (immulist != null && immulist.Count > 0)
                        {
                            if (immulist.Any(a => a.Encounter_Id == 0))
                            {
                                IList<Immunization> ImmunizList = new List<Immunization>();
                                ICriteria cri = session.GetISession().CreateCriteria(typeof(Immunization)).Add(Expression.In("Id", immulist.Where(a => a.Encounter_Id == 0).Select(a => a.Id).ToList<ulong>()));
                                ImmunizList = cri.List<Immunization>();
                                foreach (Immunization obj in ImmunizList)
                                {
                                    foreach (var temp in immulist.Where(a => a.Id == obj.Id))
                                        temp.Version = obj.Version;
                                }
                            }
                            addList = new List<Immunization>();
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref addList, ref immulist, null, MySession, MACAddress, true, true, immulist[0].Human_ID, string.Empty, ref XMLObj);
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
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                            IsImmunization = XMLObj.CheckDataConsistency(addList.Concat(immulist).Cast<object>().ToList(), true, string.Empty);
                        }

                        if (immunHist != null)
                        {
                            ImmunizationHistoryManager ImmnunizHistoryMngr = new ImmunizationHistoryManager();
                            ICriteria crit = session.GetISession().CreateCriteria(typeof(ImmunizationHistory)).Add(Expression.Eq("Immunization_Order_ID", immunHist.Immunization_Order_ID));
                            deleteImmunHisList = crit.List<ImmunizationHistory>();
                            if (immulist != null && immulist.Count > 0)
                            {
                                immunHist.Immunization_Order_ID = immulist[0].Id;
                                immunHist.Created_By = immulist[0].Modified_By;
                                immunHist.Created_Date_And_Time = immulist[0].Modified_Date_And_Time;
                                if (immunHist.Procedure_Code.Trim() == "90732" || immunHist.Procedure_Code.Trim() == "90670")
                                {
                                    immunHist.Snomed_Code = "310578008";
                                }
                                else
                                {
                                    immunHist.Snomed_Code = "";
                                }
                            }
                            if (immun.Is_Administration_Refused != "Y")//In this case the existing item in ImmHis is deleted and new item is created with updated details..If this was to be deleted ()
                                updtImmunHisList.Add(immunHist);

                            IList<ImmunizationHistory> UpdateImmunizationHistory = new List<ImmunizationHistory>(); ;
                            IList<ImmunizationHistory> UpdateImmunHistory = null;
                            IList<ImmunizationMasterHistory> _updateMasterList = new List<ImmunizationMasterHistory>();
                            IList<ImmunizationMasterHistory> ImmuHisMAsterDeletelist = new List<ImmunizationMasterHistory>();
                            IList<ImmunizationMasterHistory> lstsave = new List<ImmunizationMasterHistory>();
                            crit = session.GetISession().CreateCriteria(typeof(ImmunizationMasterHistory)).Add(Expression.Eq("Immunization_Order_ID", immunHist.Immunization_Order_ID));
                            ImmuHisMAsterDeletelist = crit.List<ImmunizationMasterHistory>();


                            if (updtImmunHisList.Count > 0)
                                for (int i = 0; i < updtImmunHisList.Count; i++)
                                {
                                    ImmunizationMasterHistory objMaster = new ImmunizationMasterHistory();
                                    //  ICriteria crite = Session.GetISession().CreateCriteria(typeof(ImmunizationMasterHistory)).Add(Expression.Eq("Immunization_Order_ID", updtImmunHisList[i].Immunization_Order_ID));
                                    //  IList<ImmunizationMasterHistory> tempmasterhistory = new List<ImmunizationMasterHistory>();

                                    // tempmasterhistory = crite.List<ImmunizationMasterHistory>();
                                    //  if (tempmasterhistory.Count > 0)
                                    // {
                                    // objMaster = tempmasterhistory[0];
                                    objMaster.Administered_Amount = updtImmunHisList[i].Administered_Amount;
                                    objMaster.Administered_Date = updtImmunHisList[i].Administered_Date;
                                    objMaster.Administered_Unit = updtImmunHisList[i].Administered_Unit;
                                    objMaster.CVX_Code = updtImmunHisList[i].CVX_Code;
                                    objMaster.Date_On_Vis = updtImmunHisList[i].Date_On_Vis;
                                    objMaster.Dose = updtImmunHisList[i].Dose;
                                    objMaster.Dose_No = updtImmunHisList[i].Dose_No;
                                    objMaster.Expiry_Date = updtImmunHisList[i].Expiry_Date;
                                    objMaster.Human_ID = updtImmunHisList[i].Human_ID;
                                    objMaster.Immunization_Description = updtImmunHisList[i].Immunization_Description;
                                    objMaster.Immunization_Order_ID = updtImmunHisList[i].Immunization_Order_ID;
                                    objMaster.Immunization_Source = updtImmunHisList[i].Immunization_Source;
                                    objMaster.Is_Deleted = updtImmunHisList[i].Is_Deleted;
                                    objMaster.Is_VIS_Given = updtImmunHisList[i].Is_VIS_Given;
                                    objMaster.Location = updtImmunHisList[i].Location;
                                    objMaster.Lot_Number = updtImmunHisList[i].Lot_Number;
                                    objMaster.Manufacturer = updtImmunHisList[i].Manufacturer;
                                    objMaster.Notes = updtImmunHisList[i].Notes;
                                    objMaster.Physician_ID = updtImmunHisList[i].Physician_ID;
                                    objMaster.Procedure_Code = updtImmunHisList[i].Procedure_Code;
                                    objMaster.Protection_State = updtImmunHisList[i].Protection_State;
                                    objMaster.Route_Of_Administration = updtImmunHisList[i].Route_Of_Administration;
                                    objMaster.Snomed_Code = updtImmunHisList[i].Snomed_Code;
                                    objMaster.Vis_Given_Date = updtImmunHisList[i].Vis_Given_Date;
                                    objMaster.Modified_By = updtImmunHisList[i].Modified_By;
                                    objMaster.Modified_Date_And_Time = updtImmunHisList[i].Modified_Date_And_Time;
                                    // objMaster.Version = updtImmunHisList[i].Version;
                                    lstsave.Add(objMaster);
                                    //  }
                                }

                            if (lstsave.Count > 0)
                            {
                                ImmunizationMasterHistoryManager masterManager = new ImmunizationMasterHistoryManager();
                                //  SaveUpdateDelete_DBAndXML_WithoutTransaction(ref objImmuHistorySave, ref DeleteList, null, sMacAddress, true, true, DeleteList[0].Human_ID, string.Empty);


                                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                                {
                                    iResult = masterManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstsave, ref _updateMasterList, ImmuHisMAsterDeletelist, iMySession, MACAddress, true, true, immunHist.Human_ID, string.Empty, ref XMLObj); ;
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
                                            throw new Exception("Deadlock is occured. Transaction failed");
                                        }
                                    }
                                    else if (iResult == 1)
                                    {
                                        trans.Rollback();
                                        throw new Exception("Exception is occured. Transaction failed");
                                    }
                                    //  IsImmunizationHistory = XMLObj.CheckDataConsistency(updtImmunHisList.Concat(UpdateImmunizationHistory).Cast<object>().ToList(), true, string.Empty);
                                }

                                // masterManager.UpdateImmunizationHistoryDetails(null, null, _updateMasterList, _updateMasterList[0].Human_ID, 0, 0, string.Empty, 0, true);
                                if (updtImmunHisList.Count > 0)
                                {
                                    updtImmunHisList[0].Immunization_History_Master_ID = lstsave[0].Id;
                                }
                            }
                            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                            {
                                iResult = ImmnunizHistoryMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref updtImmunHisList, ref UpdateImmunHistory, deleteImmunHisList, iMySession, MACAddress, true, true, immunHist.Human_ID, string.Empty, ref XMLObj);
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
                                        throw new Exception("Deadlock is occured. Transaction failed");
                                    }
                                }
                                else if (iResult == 1)
                                {
                                    trans.Rollback();
                                    throw new Exception("Exception is occured. Transaction failed");
                                }
                                IsImmunizationHistory = XMLObj.CheckDataConsistency(updtImmunHisList.Concat(UpdateImmunizationHistory).Cast<object>().ToList(), true, string.Empty);
                            }
                        }
                        if (UpdateTreatmentPlan != null || InsertTreatmentPlan != null)
                        {
                            
                            if (InsertTreatmentPlan != null)
                            {
                                SaveTreatementPlanList.Add(InsertTreatmentPlan);
                            }
                            else if (UpdateTreatmentPlan != null)
                            {
                                if (immun.Is_Administration_Refused != "Y")
                                    UpdateTreatmentPlanList.Add(UpdateTreatmentPlan);
                                else
                                    DeleteTreatmentPlanList.Add(UpdateTreatmentPlan);
                            }
                            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                            {
                                iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveTreatementPlanList, ref UpdateTreatmentPlanList, DeleteTreatmentPlanList, iMySession, MACAddress, true, true, ulMyEncounterID, string.Empty, ref XMLObjEncounter);

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
                                IsTreatmentPlanConsistent = XMLObjEncounter.CheckDataConsistency(UpdateTreatmentPlanList.Cast<object>().ToList(), true, string.Empty);
                            }
                        }
                        if (IsImmunization && IsImmunizationHistory && IsTreatmentPlanConsistent)
                        {
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
                                        // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                        WriteBlob(immunHist.Human_ID, XMLObj.itemDoc, MySession, null, immulist, null, XMLObj, false);

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
                                //if (XMLObjEncounter.strXmlFilePath != null && XMLObjEncounter.strXmlFilePath != "")
                                if (XMLObjEncounter.itemDoc.InnerXml != null && XMLObjEncounter.itemDoc.InnerXml != "")
                                {
                                    // XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        //   XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
                                        objTreatmentPlanMgr.WriteBlob(EncounterID, XMLObjEncounter.itemDoc, MySession, SaveTreatementPlanList, UpdateTreatmentPlanList, DeleteTreatmentPlanList, XMLObjEncounter, false);

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
                                throw new Exception(xmlexcep.Message);
                            }
                            catch (IOException ex)
                            {
                                throw new Exception(ex.Message);
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


            if (EncounterID != 0)
            {
                return FillImmunization(immun.Human_ID, EncounterID);
            }
            else
            {
                return GetImmunizationUsingHumanID(immun.Human_ID, immun.Created_Date_And_Time, immun.Physician_Id);
            }
        }

        public ImmunizationDTO DeleteImmunization(Immunization immun, ImmunizationHistory immunHist, ulong EncounterID, string MACAddress, string sPln)
        {
            WFObjectManager wfMngr = new WFObjectManager();
            IList<ImmunizationHistory> delImmunHisList = new List<ImmunizationHistory>();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            IList<TreatmentPlan> SaveTreatmentPlanList = null;
            IList<TreatmentPlan> UpdateTreatmentPlanList = new List<TreatmentPlan>();
            IList<TreatmentPlan> DeleteTreatmentPlanList = new List<TreatmentPlan>();

            WFObject TempWF = wfMngr.GetByObjectSystemId(immun.Immunization_Group_ID, "IMMUNIZATION ORDER");
            bool deleteFlag = false;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Immunization_Group_ID", immun.Immunization_Group_ID));
                if (crit.List<Immunization>() != null && crit.List<Immunization>().Count == 1)
                {
                    deleteFlag = true;
                }
                iMySession.Close();
            }
            //if (immun.Encounter_Id == 0)//BugID : 59972 
            //    immun = GetById(immun.Id);
            IList<Immunization> immulist = new List<Immunization>();
            immulist.Add(immun);
            IList<Immunization> Addlist = null;
            iTryCount = 0;
            string FileName = "Encounter" + "_" + EncounterID + ".xml";
            XmlTextReader XmlText = null;
            string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            try
            {
                if (File.Exists(strXmlFilePath) == true)
                {
                    XmlDocument itemDoc = new XmlDocument();
                    XmlText = new XmlTextReader(strXmlFilePath);
                    XmlNodeList xmlTagName = null;
                    // itemDoc.Load(XmlText);
                    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        itemDoc.Load(fs);

                        XmlText.Close();
                        #region Treatment_plan
                        if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
                        {
                            xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

                            if (xmlTagName.Count > 0)
                            {
                                for (int j = 0; j < xmlTagName.Count; j++)
                                {
                                    if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("IMMUNIZATION/INJECTION"))
                                    {

                                        string TagName = xmlTagName[j].Name;
                                        XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
                                        TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
                                        IEnumerable<PropertyInfo> propInfo = null;
                                        propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

                                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                                        {

                                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
                                            {
                                                foreach (PropertyInfo property in propInfo)
                                                {
                                                    if (property.Name == nodevalue.Name)
                                                    {
                                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                                                            property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                                                            property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                            property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
                                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                                                            property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
                                                        else
                                                            property.SetValue(TreatmentPlan, nodevalue.Value, null);
                                                    }
                                                }
                                            }

                                        }
                                        objTreatmentPlan.Add(TreatmentPlan);
                                    }
                                }
                            }
                        }
                        #endregion
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
            catch(Exception Ex)
            {
                if (XmlText != null)
                    XmlText.Close();

                throw Ex;
            }
        TryAgain:
            int iResult = 0;
            ulong ulMyEncounterID = immun.Encounter_Id;
            TreatmentPlanManager objTreatmentPlanMgr = new TreatmentPlanManager();
            //TreatmentPlan UpdateTreatmentPlan = null;
            TreatmentPlan DeleteTreatmentPlan = null;

            if (ulMyEncounterID != 0)
            {
                IList<TreatmentPlan> delitemlst = objTreatmentPlan.Where(a => a.Source_ID == immun.Id).ToList<TreatmentPlan>();
                if (delitemlst != null && delitemlst.Count > 0)
                    DeleteTreatmentPlan = delitemlst[0];
            }

            Session.GetISession().Clear();
            GenerateXml XMLObj = new GenerateXml();
            GenerateXml XMLObjEncounter = new GenerateXml();
            bool IsTreatmentPlanConsistent = true;


            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if (immulist != null && immulist.Count > 0)
                        {
                            //IList<Immunization> UpdateImmunizList = null;
                            //iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Addlist, ref UpdateImmunizList, immulist, MySession, MACAddress, true, false, immulist[0].Human_ID, string.Empty, ref XMLObj);
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Addlist, ref immulist, null, MySession, MACAddress, true, true, immulist[0].Human_ID, string.Empty, ref XMLObj);

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
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }
                        }

                        if (immunHist != null)
                        {
                            ImmunizationHistoryManager ImmnunizHistoryMngr = new ImmunizationHistoryManager();

                            IList<ImmunizationHistory> SaveImmunizHistoryList = null;
                           // IList<ImmunizationHistory> UpdateImmunizHisList = null;
                            delImmunHisList.Add(immunHist);
                            //iResult = ImmnunizHistoryMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveImmunizHistoryList, ref UpdateImmunizHisList, delImmunHisList, MySession, MACAddress, true, false, immunHist.Human_ID, string.Empty, ref XMLObj);
                            IList<ImmunizationMasterHistory> _updateMasterList = new List<ImmunizationMasterHistory>();
                            IList<ImmunizationMasterHistory> lstsavemasterimm = new List<ImmunizationMasterHistory>();
                           
                            //if (delImmunHisList.Count > 0)
                               for (int i = 0; i < delImmunHisList.Count; i++)
                               {
                                   // ImmunizationMasterHistory objMaster = new ImmunizationMasterHistory();
                                    ICriteria crit = Session.GetISession().CreateCriteria(typeof(ImmunizationMasterHistory)).Add(Expression.Eq("Id", delImmunHisList[i].Immunization_History_Master_ID));
                                    _updateMasterList = crit.List<ImmunizationMasterHistory>();
                                  
                            //objMaster.Administered_Amount = delImmunHisList[i].Administered_Amount;
                            //        objMaster.Administered_Date = delImmunHisList[i].Administered_Date;
                            //        objMaster.Administered_Unit = delImmunHisList[i].Administered_Unit;
                            //        objMaster.CVX_Code = delImmunHisList[i].CVX_Code;
                            //        objMaster.Date_On_Vis = delImmunHisList[i].Date_On_Vis;
                            //        objMaster.Dose = delImmunHisList[i].Dose;
                            //        objMaster.Dose_No = delImmunHisList[i].Dose_No;
                            //        objMaster.Expiry_Date = delImmunHisList[i].Expiry_Date;
                            //        objMaster.Human_ID = delImmunHisList[i].Human_ID;
                            //        objMaster.Immunization_Description = delImmunHisList[i].Immunization_Description;
                            //        objMaster.Immunization_Order_ID = delImmunHisList[i].Immunization_Order_ID;
                            //        objMaster.Immunization_Source = delImmunHisList[i].Immunization_Source;
                            //        objMaster.Is_Deleted = delImmunHisList[i].Is_Deleted;
                            //        objMaster.Is_VIS_Given = delImmunHisList[i].Is_VIS_Given;
                            //        objMaster.Location = delImmunHisList[i].Location;
                            //        objMaster.Lot_Number = delImmunHisList[i].Lot_Number;
                            //        objMaster.Manufacturer = delImmunHisList[i].Manufacturer;
                            //        objMaster.Notes = delImmunHisList[i].Notes;
                            //        objMaster.Physician_ID = delImmunHisList[i].Physician_ID;
                            //        objMaster.Procedure_Code = delImmunHisList[i].Procedure_Code;
                            //        objMaster.Protection_State = delImmunHisList[i].Protection_State;
                            //        objMaster.Route_Of_Administration = delImmunHisList[i].Route_Of_Administration;
                            //        objMaster.Snomed_Code = delImmunHisList[i].Snomed_Code;
                            //        objMaster.Vis_Given_Date = delImmunHisList[i].Vis_Given_Date;
                            //        objMaster.Modified_By = delImmunHisList[i].Modified_By;
                            //        objMaster.Modified_Date_And_Time = delImmunHisList[i].Modified_Date_And_Time;
                            //        objMaster.Version = delImmunHisList[i].Version;
                            //        _updateMasterList.Add(objMaster);
                               }

                            if (_updateMasterList.Count > 0)
                            {
                                _updateMasterList[0].Is_Deleted = "Y";
                                ImmunizationMasterHistoryManager masterManager = new ImmunizationMasterHistoryManager();
                                masterManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstsavemasterimm, ref _updateMasterList, null, MySession, MACAddress, true, true, immunHist.Human_ID, string.Empty, ref XMLObj);
                               // masterManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(null, null, _updateMasterList, delImmunHisList[0].Human_ID, 0, 0, string.Empty, 0, true);
                            }
                            iResult = ImmnunizHistoryMngr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveImmunizHistoryList, ref delImmunHisList, null, MySession, MACAddress, true, true, immunHist.Human_ID, string.Empty, ref XMLObj);
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
                                    throw new Exception("Deadlock is occured. Transaction failed");
                                }
                            }
                            else if (iResult == 1)
                            {
                                trans.Rollback();
                                throw new Exception("Exception is occured. Transaction failed");
                            }

                        }

                        if (DeleteTreatmentPlan != null)
                        {
                           
                            if (DeleteTreatmentPlan != null)
                                DeleteTreatmentPlanList.Add(DeleteTreatmentPlan);

                            iResult = objTreatmentPlanMgr.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref SaveTreatmentPlanList, ref UpdateTreatmentPlanList, DeleteTreatmentPlanList, MySession, MACAddress, true, true, ulMyEncounterID, string.Empty, ref XMLObjEncounter);

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

                            IsTreatmentPlanConsistent = XMLObjEncounter.CheckDataConsistency(DeleteTreatmentPlanList.Cast<object>().ToList(), true, string.Empty);

                        }
                        if (deleteFlag == true && TempWF != null)
                        {
                            if (TempWF.Current_Process == "MA_REVIEW")
                            {
                                wfMngr.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 3, "UNKNOWN", immun.Modified_Date_And_Time, MACAddress, null, MySession);
                            }
                            else if (TempWF.Current_Process == "BILLING_WAIT")
                            {
                                wfMngr.MoveToNextProcess(TempWF.Obj_System_Id, TempWF.Obj_Type, 2, "UNKNOWN", immun.Modified_Date_And_Time, MACAddress, null, MySession);
                            }
                        }
                        if (IsTreatmentPlanConsistent)
                        {
                            try
                            {
                               // if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                                    if (XMLObj.itemDoc.InnerXml != null && XMLObj.itemDoc.InnerXml != "")
                                    {
                                   // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                        //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                        WriteBlob(immunHist.Human_ID, XMLObj.itemDoc, MySession, null, null, immulist, XMLObj, false);
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
                                //if (XMLObjEncounter.strXmlFilePath != null && XMLObjEncounter.strXmlFilePath != "")
                                if (XMLObjEncounter.itemDoc.InnerXml != null && XMLObjEncounter.itemDoc.InnerXml != "")
                                {
                                   // XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                       // XMLObjEncounter.itemDoc.Save(XMLObjEncounter.strXmlFilePath);
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
                                throw new Exception(xmlexcep.Message);
                            }
                            catch (IOException ex)
                            {
                                throw new Exception(ex.Message);
                            }
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

            if (EncounterID != 0)
            {
                return FillImmunization(immun.Human_ID, EncounterID);
            }
            else
            {
                return GetImmunizationUsingHumanID(immun.Human_ID, immun.Created_Date_And_Time, immun.Physician_Id);
            }
        }

        public ImmunizationDTO LoadImmunization(ulong HumanId, ulong PhysicianID, ulong EncounterID, DateTime todaysDate,string sLegalOrg)
        {
            ImmunizationDTO objImmunizationDTO = new ImmunizationDTO();
            StaticLookupManager objStaticMngr = new StaticLookupManager();
            EAndMCodingManager objEAndMCodingMngr = new EAndMCodingManager();
            VaccineManufacturerCodesManager objVaccineManufacturerMngr = new VaccineManufacturerCodesManager();
            objImmunizationDTO.objStaticLookupList = objStaticMngr.getStaticLookupByFieldName(new string[] { "IMMUNIZATIONLOCATION", "ROUTE OF ADMINISTRATION", "VFC STATUS", "DOSE", "IMMUNIZATION SOURCE", "IMMUNIZATION ADMINISTRATION UNITS", "INJECTION PROCEDURES BEGINING CHARACTERS", "OBSERVATION", "REFUSED_ADMINISTRATION", "ELIGIBILITY_CAPTURED", "IMMUNIZATION_INFORMATION_SOURCE", "IMMUNIZATION EVIDENCE", "VACCINE TYPE", "CPT_FOR_REFUSED_ADMINISTRATION" }, "Sort_Order");
            objImmunizationDTO.phyProcedureList = objEAndMCodingMngr.GetPhysicianProcedure(PhysicianID, "IMMUNIZATION PROCEDURE", 0,sLegalOrg);
            objImmunizationDTO.manufacturerList = objVaccineManufacturerMngr.GetManufacturerList();
            return objImmunizationDTO;
        }

        public ulong FileCountAndFileName(ulong HumanID)
        {
            ImmunizationDTO objDTO = new ImmunizationDTO();
            IList<FileManagementIndex> lst_FileCount = new List<FileManagementIndex>();
            int prevNum = 0;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                lst_FileCount = iMySession.CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Human_ID", HumanID)).List<FileManagementIndex>();

                int[] sortIndexNum = new int[lst_FileCount.Count];
                for (int j = 0; j < lst_FileCount.Count; j++) //only for Max number of file number
                {
                    if (lst_FileCount[j].File_Path != "")
                    {
                        try
                        {
                            string fi = lst_FileCount[j].File_Path.Substring(lst_FileCount[j].File_Path.LastIndexOf("/") + 1);
                            prevNum = Convert.ToInt32(fi.Substring(fi.LastIndexOf("_") + 1, (fi.LastIndexOf(".") - 1) - fi.LastIndexOf("_")));
                            sortIndexNum[j] = prevNum;
                            prevNum = sortIndexNum.Max();
                        }
                        catch { }
                    }
                }

                // objDTO = FillImageProcedure(EncounterId , HumanID);
                //  objDTO.File_Count = Convert.ToUInt64(prevNum);
                iMySession.Close();
            }
            return Convert.ToUInt64(prevNum);
        }

        public IList<FileManagementIndex> FillImageProcedureWithoutEncounterID(ulong HumanId)
        {
            IList<FileManagementIndex> lstFileManagementIndex = new List<FileManagementIndex>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IList<Immunization> lstImmunuzation = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Human_ID", HumanId)).AddOrder(Order.Desc("Modified_Date_And_Time")).List<Immunization>();


                foreach (Immunization obj in lstImmunuzation)
                {



                    foreach (string id in obj.File_Management_Index_Id.Split('|'))
                    {
                        if (id != "0" && id != "")
                        {
                            try
                            {
                                lstFileManagementIndex.Add(session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Id", Convert.ToUInt64(id))).Add(Expression.Eq("Source", "Immunization")).List<FileManagementIndex>()[0]);
                            }
                            catch
                            {

                            }
                        }
                    }
                }


                iMySession.Close();
            }
            return lstFileManagementIndex;
        }

        public IList<FileManagementIndex> FillImageProcedure(ulong EncounterId, ulong HumanId)
        {


            IList<FileManagementIndex> lstFileManagementIndex = new List<FileManagementIndex>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {

                IList<Immunization> lstImmunuzation = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Encounter_Id", EncounterId)).Add(Expression.Eq("Human_ID", HumanId)).AddOrder(Order.Desc("Modified_Date_And_Time")).List<Immunization>();
                IList<FileManagementIndex> lstAll = new List<FileManagementIndex>();
                //foreach (Immunization obj in lstImmunuzation)
                //{
                //    foreach (string id in obj.File_Management_Index_Id.Split('|'))
                //    {
                //        if (id != "0" && id != "")
                //        {
                //            try
                //            {
                //                lstFileManagementIndex.Add(session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Id", Convert.ToUInt64(id))).Add(Expression.Eq("Source", "Immunization")).List<FileManagementIndex>()[0]);
                //            }
                //            catch
                //            {

                //            }
                //        }
                //    }
                //}
                uint uID;
                ArrayList lstID = new ArrayList();
                foreach (Immunization obj in lstImmunuzation)
                {
                    foreach (string id in obj.File_Management_Index_Id.Split('|'))
                    {
                        if (id != "0" && id != "" && UInt32.TryParse(id, out uID))
                        {
                            lstID.Add(uID);
                        }
                    }
                }
                lstAll = session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.In("Id", lstID)).Add(Expression.Eq("Source", "Immunization")).List<FileManagementIndex>();
                foreach (object objId in lstID)
                {
                    IList<FileManagementIndex> tempLst = (from obj in lstAll where obj.Id == Convert.ToUInt32(objId) select obj).ToList<FileManagementIndex>();
                    if (tempLst.Count > 0)
                        lstFileManagementIndex.Add(tempLst[0]);
                }
                iMySession.Close();
            }
            return lstFileManagementIndex;
        }

        public ImmunizationDTO FillImmunization(ulong HumanId, ulong EncounterID)
        {
            ImmunizationDTO objImmunizationDTO = new ImmunizationDTO();
            IList<ulong> uList = new List<ulong>();
            IList<object> grpId = new List<object>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria immunCrit = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Encounter_Id", EncounterID)).Add(Expression.Eq("Human_ID", HumanId)).AddOrder(Order.Desc("Modified_Date_And_Time"));
                objImmunizationDTO.Immunization = immunCrit.List<Immunization>();

                ICriteria critGrpID = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", "IMMUNIZATION ORDER"))
                  .SetProjection(Projections.Max("Obj_System_Id"));
                grpId = critGrpID.List<object>();
                if (grpId != null && grpId.Count > 0)
                    objImmunizationDTO.MaxGroupId = Convert.ToUInt64(grpId[0]);
                else
                    objImmunizationDTO.MaxGroupId = 0;
                foreach (Immunization obj in objImmunizationDTO.Immunization)
                {
                    uList.Add(obj.Id);
                }

                if (uList.Count > 0)
                {
                    ICriteria crit = iMySession.CreateCriteria(typeof(ImmunizationHistory)).Add(Expression.In("Immunization_Order_ID", uList.ToArray<ulong>()));
                    objImmunizationDTO.ImmunizationHistoryList = crit.List<ImmunizationHistory>();
                }
                iMySession.Close();
            }
            return objImmunizationDTO;
        }

        public ImmunizationDTO GetImmunizationUsingHumanID(ulong HumanId, DateTime createdDate, ulong PhysicianId)
        {
            ImmunizationDTO objImmunizationDTO = new ImmunizationDTO();
            IList<Immunization> ImmunizationList = new List<Immunization>();
            IList<ulong> uList = new List<ulong>();
            DateTime dt = createdDate.Date.AddHours(24);

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria immunCrit = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Physician_Id", PhysicianId)).Add(Expression.Eq("Encounter_Id", Convert.ToUInt64(0)));
                objImmunizationDTO.Immunization = immunCrit.List<Immunization>();
                foreach (Immunization obj in objImmunizationDTO.Immunization)
                {
                    uList.Add(obj.Id);
                }
                if (uList.Count > 0)
                {
                    ICriteria crit = iMySession.CreateCriteria(typeof(ImmunizationHistory)).Add(Expression.In("Immunization_Order_ID", uList.ToArray<ulong>()));
                    objImmunizationDTO.ImmunizationHistoryList = crit.List<ImmunizationHistory>();
                }
                iMySession.Close();
            }
            return objImmunizationDTO;
        }

        public IList<FillOrdersManagementDTO> SearchImmunizationOrder(string order_type, string order_status, string FacilityName, int orderInDays, DateTime fromDate, DateTime toDate, ulong humanID, ulong physicianID, int pageNumber, int maxResults)
        {
            IList<FillOrdersManagementDTO> ilstFillOrdersManagementDTO = new List<FillOrdersManagementDTO>();
            FillOrdersManagementDTO objFillOrdersManagementDTO = new FillOrdersManagementDTO();
            IList<Immunization> immunizationList = new List<Immunization>();
            IList<WFObject> wfObjectList = new List<WFObject>();
            string human = string.Empty;
            string physician = string.Empty;
            string orderType = string.Empty;
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
            if (order_type.Trim() == string.Empty)
            {
                orderType = "%";
            }
            else
            {
                orderType = order_type;
            }
            if (order_status.Trim() == string.Empty)
            {
                order_status = "%";
            }
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query;
                int startIndex = 0;
                startIndex = ((pageNumber - 1) * maxResults);
                query = iMySession.GetNamedQuery("Get.GetImmunizationOrderSearch");
                query.SetString(0, orderType);
                query.SetString(1, FacilityName);
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
                        objFillOrdersManagementDTO.Order_Type = obj[1].ToString();
                        objFillOrdersManagementDTO.Facility_Name = obj[2].ToString();
                        objFillOrdersManagementDTO.Current_Process = obj[3].ToString();
                        objFillOrdersManagementDTO.Ordered_Date_And_Time = Convert.ToDateTime(obj[4]);
                        objFillOrdersManagementDTO.Human_Id = (obj[5].ToString());
                        objFillOrdersManagementDTO.Human_Name = obj[6].ToString();
                        objFillOrdersManagementDTO.Date_Of_Birth = Convert.ToDateTime(obj[7]);
                        objFillOrdersManagementDTO.Physician_ID = Convert.ToUInt64(obj[8]);
                        objFillOrdersManagementDTO.Procedures = obj[9].ToString();
                        objFillOrdersManagementDTO.Total_Record_Found = Convert.ToUInt64(arr.Count);
                        if (obj[10] != null)
                            objFillOrdersManagementDTO.Ordering_Provider = obj[10].ToString();
                        ilstFillOrdersManagementDTO.Add(objFillOrdersManagementDTO);

                    }
                }
                iMySession.Close();
            }
            return ilstFillOrdersManagementDTO;
        }

        #region IImmunizationManager Members
        //BatchOperationsToImmunization Method Declared But Nevere Used.
        public int BatchOperationsToImmunization(IList<Immunization> savelist, IList<Immunization> updtList, IList<Immunization> delList, ISession MySession, string MACAddress)
        {
            GenerateXml XMLObj = new GenerateXml();
            return SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savelist, ref updtList, delList, MySession, MACAddress, false, false, 0, string.Empty, ref XMLObj);
        }

        #endregion

        #region IImmunizationManager Members

        public object SubmitImmunization(IList<Immunization> immunizationList, string MACAddress)
        {
            iTryCount = 0;
        TryAgain:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            //ITransaction trans = null;
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        //trans = MySession.BeginTransaction();
                        if (immunizationList != null && immunizationList.Count > 0)
                        {
                            GenerateXml XMLObj = new GenerateXml();
                            IList<Immunization> list = null;
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref list, ref immunizationList, null, MySession, MACAddress, true, false, immunizationList[0].Human_ID, string.Empty, ref XMLObj);
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

                            WFObject WFObj = new WFObject();
                            WFObj.Obj_Type = "IMMUNIZATION ORDER";
                            WFObj.Current_Arrival_Time = immunizationList[0].Modified_Date_And_Time;
                            WFObj.Current_Owner = "UNKNOWN";
                            WFObj.Fac_Name = immunizationList[0].Facility_Name;
                            WFObj.Parent_Obj_Type = string.Empty;
                            WFObj.Obj_System_Id = immunizationList[0].Immunization_Group_ID;
                            WFObj.Current_Process = "START";
                            WFObjectManager objWfMnger = new WFObjectManager();
                            iResult = objWfMnger.InsertToWorkFlowObject(WFObj, 1, MACAddress, MySession);
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
                            MySession.Flush();
                            trans.Commit();
                            if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                            {
                             //   XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                    XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
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
                        // MySession.Close();
                        throw new Exception(ex.Message);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        //MySession.Close();
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        MySession.Close();
                    }
                }
            }
            catch (Exception ex1)
            {
                throw new Exception(ex1.Message);
            }
            //if (immunizationList != null && immunizationList.Count>0)
            //{
            //    for (int i = 0; i < immunizationList.Count; i++)
            //    {
            //        immunizationList[i].Version = immunizationList[i].Version + 1;
            //    }

            //    ulong uHuman_Id = immunizationList[0].Human_ID;
            //    List<object> lstObj = immunizationList.Cast<object>().ToList();
            //    XMLObj.GenerateXmlUpdate(lstObj, uHuman_Id, string.Empty); 
            //}
            return (object)immunizationList[0].Immunization_Group_ID;
        }


        #endregion

        public void SaveImmunizationforSummary(IList<Immunization> lstimmunization)
        {
            IList<Immunization> UpdateImmunization = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstimmunization, ref UpdateImmunization, null, string.Empty, false, false, 0, string.Empty);
        }

        public IList<Immunization> GetImmunizationUsingHumanIDEncID(ulong HumanId, ulong EncounterID)
        {
            ImmunizationDTO objImmunizationDTO = new ImmunizationDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria immunCrit = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Encounter_Id", EncounterID));
                objImmunizationDTO.Immunization = immunCrit.List<Immunization>();
                iMySession.Close();
            }
            return (objImmunizationDTO.Immunization);
        }

        public IList<Immunization> GetImmunizationUsingGroupID(ulong ulImmunGroupID)
        {
            IList<Immunization> objImmunization = new List<Immunization>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria immunCrit = iMySession.CreateCriteria(typeof(Immunization)).Add(Expression.Eq("Immunization_Group_ID", ulImmunGroupID));
                objImmunization = immunCrit.List<Immunization>();
                iMySession.Close();
            }
            return objImmunization;
        }

        #endregion
    }
}
