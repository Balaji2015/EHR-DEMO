using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public interface IInHouseProcedureManager : IManagerBase<InHouseProcedure, ulong>
    {
        InHouseProcedureDTO LoadInHouseProcedure(ulong EncounterId, ulong PhysicianID, ulong HumanID, string sLegalOrg);
        InHouseProcedureDTO FillInHouseProcedure(ulong EncounterId, ulong Physician_Id, ulong HumanId);
        InHouseProcedureDTO InsertInHouseProcedure(IList<InHouseProcedure> savelist, WFObject wfObj, string MACAddress, string sOtherProcedure, string sLocalTime, string sLegalOrg);
       // InHouseProcedureDTO UpdateInHouseProcedure(InHouseProcedure obj, string MACAddress, string sPlanText, string sOtherProcedure);
        InHouseProcedureDTO UpdateAndDeleteAndSaveInHouseProcedure(IList<InHouseProcedure> lstSave, IList<InHouseProcedure> lstUpdate, IList<InHouseProcedure> lstDelete, bool IsFileMgnt_delete, string MACAddress, string sLocalTime, string sLegalOrg);
        InHouseProcedureDTO DeleteInHouseProcedureBYList(IList<InHouseProcedure> lstDelete, string MACAddress, string sPln, string sLegalOrg);
       // int BatchOperationsToInHouseProcedure(IList<InHouseProcedure> savelist, IList<InHouseProcedure> updtList, IList<InHouseProcedure> delList, ISession MySession, string MACAddress);
        object SubmitInHouseProcedures(IList<InHouseProcedure> orderList, string MACAddress);
        InHouseProcedureDTO FillImageProcedure(ulong EncounterId, ulong Physician_Id, ulong HumanId);
        void SaveProceduresforSummary(IList<InHouseProcedure> lstprocedure);

        IList<FillOrdersManagementDTO> SearchProcedureOrder(string order_type, string order_status, string FacilityName, int orderInDays, DateTime fromDate, DateTime toDate, ulong humanID, ulong physicianID, int pageNumber, int maxResults);
        IList<InHouseProcedure> GetImplantableDeviceInformation(string sDeviceID, bool is_DI);
        //Added by Saravanakumar 
    }
    public partial class InHouseProcedureManager : ManagerBase<InHouseProcedure, ulong>, IInHouseProcedureManager
    {
        #region Constructors

        public InHouseProcedureManager()
            : base()
        {

        }
        public InHouseProcedureManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        public InHouseProcedureDTO LoadInHouseProcedure(ulong encounterId, ulong physicianId, ulong humanId, string sLegalOrg)
        {
            InHouseProcedureDTO objDTO = new InHouseProcedureDTO();
            objDTO = FillImageProcedure(encounterId, physicianId, humanId);
            PhysicianProcedureManager objPhysicianProcedureManager = new PhysicianProcedureManager();
            objDTO.lstprocedureList = objPhysicianProcedureManager.GetProceduresUsingPhysicianIDAndLabID(physicianId, "OTHER PROCEDURE", 0,sLegalOrg);
            return objDTO;
        }

        public InHouseProcedureDTO FillImageProcedure(ulong EncounterId, ulong Physician_Id, ulong HumanId)
        {
            InHouseProcedureDTO objDTO = new InHouseProcedureDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                objDTO.OtherProcedure = iMySession.CreateCriteria(typeof(InHouseProcedure))
                                                  .Add(Expression.Eq("Encounter_ID", EncounterId))
                                                  .Add(Expression.Eq("Human_ID", HumanId))
                                                  .AddOrder(Order.Desc("Modified_Date_And_Time")).List<InHouseProcedure>();
                iMySession.Close();

            }
            return objDTO;
        }


        public InHouseProcedureDTO FillInHouseProcedure(ulong EncounterId, ulong Physician_Id, ulong HumanId)
        {
            InHouseProcedureDTO objDTO = new InHouseProcedureDTO();
            SpirometryManager ObjSpirometryMgr = new SpirometryManager();
            ABI_ResultsManager ObjABIMgr = new ABI_ResultsManager();
            ArrayList arrList = new ArrayList();
            IList<string> procedureList = new List<string>();
            IList<object> grpId = new List<object>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = null;
                if (EncounterId == 0)
                {
                    crit = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Encounter_ID", EncounterId)).Add(Expression.Eq("Human_ID", HumanId)).Add(Expression.Eq("Physician_ID", Physician_Id)).AddOrder(Order.Desc("Modified_Date_And_Time"));
                }
                else
                {
                    crit = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Encounter_ID", EncounterId)).Add(Expression.Eq("Human_ID", HumanId)).AddOrder(Order.Desc("Modified_Date_And_Time"));
                }
                objDTO.OtherProcedure = crit.List<InHouseProcedure>();
                ICriteria critGrpID = iMySession.CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_Type", "INTERNAL ORDER"))
                    .SetProjection(Projections.Max("Obj_System_Id"));
                grpId = critGrpID.List<object>();
                if (grpId != null && grpId.Count > 0)
                    objDTO.MaxGroupId = Convert.ToUInt64(grpId[0]);
                else
                    objDTO.MaxGroupId = 0;
                arrList.Clear();
                iMySession.Close();
            }
            return objDTO;
        }
        int iTryCount = 0;
        public InHouseProcedureDTO InsertInHouseProcedure(IList<InHouseProcedure> savelist, WFObject wfObj, string MACAddress, string sOtherProcedure, string sLocalTime,string sLegalOrg)
        {
            string FileMgnt_ID = string.Empty;
            FileManagementIndexManager objFileManagementMgr = new FileManagementIndexManager();
            IList<object> lstObjectGroup = new List<object>();
            IList<InHouseProcedure> ListProcedure_old = new List<InHouseProcedure>();
            InHouseProcedureDTO objInHouseProcdureDTO = new InHouseProcedureDTO();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<InHouseProcedure> UpdateInHouse = null;
            IList<TreatmentPlan> Update_Tplan = null;
            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
            GenerateXml ObjXmlHuman = new GenerateXml();
            GenerateXml ObjXmlEncounter = new GenerateXml();
            iTryCount = 0;

        TryAgain:
            int iResult = 0;
            Session.GetISession().Clear();
            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        bool IsProcedureConsistent = true, IsTreatmentPlanConsistent = true;
                        ICriteria critGrpID = MySession.CreateCriteria(typeof(InHouseProcedure))
                   .SetProjection(Projections.Max("In_House_Procedure_Group_ID"));
                        ulong Group_Id = Convert.ToUInt64(critGrpID.List<object>()[0]) + 1;
                        for (int j = 0; j < savelist.Count; j++)
                        {
                            savelist[j].File_Management_Index_ID = FileMgnt_ID;
                            savelist[j].In_House_Procedure_Group_ID = Group_Id;

                        }
                        if (savelist.Count > 0)
                        {
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savelist, ref UpdateInHouse, null, MySession, MACAddress, true, true, savelist[0].Human_ID, string.Empty, ref ObjXmlHuman);

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
                            IsProcedureConsistent = ObjXmlHuman.CheckDataConsistency(savelist.Cast<object>().ToList(), true, string.Empty);
                            #region TplanInsert
                            foreach (InHouseProcedure obj in savelist)
                            {
                                TreatmentPlan item = new TreatmentPlan();
                                item.Human_ID = obj.Human_ID;
                                item.Encounter_Id = obj.Encounter_ID;
                                item.Physician_Id = obj.Physician_ID;
                                item.Created_By = obj.Created_By;
                                item.Created_Date_And_Time = obj.Created_Date_And_Time;
                                item.Plan_Type = "PROCEDURES";
                                string plan_txt = string.Empty;
                                plan_txt = "* " + obj.Procedure_Code_Description + (obj.Notes != string.Empty ? "-" + obj.Notes : "");
                                item.Plan = plan_txt;
                                item.Version = 0;
                                item.Source_ID = obj.Id;
                                item.Local_Time = sLocalTime;
                                Insert_Tplan.Add(item);
                            }
                            #endregion
                            if (Insert_Tplan.Count > 0 && Insert_Tplan[0].Encounter_Id != 0)
                            {
                                iResult = objTreatmentPlanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, null, MySession, MACAddress, true, true, Insert_Tplan[0].Encounter_Id, string.Empty, ref ObjXmlEncounter);

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
                            if (IsProcedureConsistent && IsTreatmentPlanConsistent)
                            {
                                try
                                {
                                    if (ObjXmlHuman != null)
                                    {
                                        //ObjXmlHuman.itemDoc.Save(ObjXmlHuman.strXmlFilePath);
                                        int trycount = 0;
                                    trytosaveagain:
                                        try
                                        {
                                           // ObjXmlHuman.itemDoc.Save(ObjXmlHuman.strXmlFilePath);
                                            WriteBlob(savelist[0].Human_ID, ObjXmlHuman.itemDoc, MySession, null, savelist, null, ObjXmlHuman, false);
                                            
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
                                    if (ObjXmlEncounter != null)
                                    {
                                       // ObjXmlEncounter.itemDoc.Save(ObjXmlEncounter.strXmlFilePath);
                                        int trycount = 0;
                                    trytosaveagain:
                                        try
                                        {
                                        // ObjXmlEncounter.itemDoc.Save(ObjXmlEncounter.strXmlFilePath);
                                        objTreatmentPlanManager.WriteBlob(Insert_Tplan[0].Encounter_Id, ObjXmlEncounter.itemDoc, MySession, Insert_Tplan, Update_Tplan, null, ObjXmlEncounter, false);
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
            catch (Exception ex)
            {
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }
         
            objInHouseProcdureDTO = LoadInHouseProcedure(savelist[0].Encounter_ID, savelist[0].Physician_ID, savelist[0].Human_ID,sLegalOrg);
            return objInHouseProcdureDTO;
        }

        public InHouseProcedureDTO UpdateAndDeleteAndSaveInHouseProcedure(IList<InHouseProcedure> lstSave, IList<InHouseProcedure> lstUpdate, IList<InHouseProcedure> lstDelete, bool IsFileMgnt_delete, string MACAddress, string sLocalTime, string sLegalOrg)
        {
            IList<InHouseProcedure> lstProcedure = new List<InHouseProcedure>();
            IList<InHouseProcedure> ListProcedure_old = new List<InHouseProcedure>();
            InHouseProcedureDTO objInHouseProcdureDTO;
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();
            string FileMgnt_ID = string.Empty;
            string sPlanOld = string.Empty;
            GenerateXml ObjXmlHuman = new GenerateXml();
            GenerateXml ObjXmlEncounter = new GenerateXml();

            ulong HumanID = 0;
            ulong EncounterID = 0;
            if (lstUpdate.Count > 0)
            {
                lstProcedure = lstUpdate;
                HumanID = lstUpdate[0].Human_ID;
                EncounterID = lstUpdate[0].Encounter_ID;

            }
            else if (lstSave.Count > 0)
            {
                lstProcedure = lstSave;
                HumanID = lstSave[0].Human_ID;
                EncounterID = lstSave[0].Encounter_ID;

            }
            else if (lstDelete.Count > 0)
            {
                HumanID = lstDelete[0].Human_ID;
                EncounterID = lstDelete[0].Encounter_ID;

            }

            #region TplanGet

            IList<string> ilstProcedureLSTTagList = new List<string>();
            ilstProcedureLSTTagList.Add("TreatmentPlanList");

            IList<object> ilstProcedureLSTFinal = new List<object>();
            ilstProcedureLSTFinal = ReadBlob(EncounterID, ilstProcedureLSTTagList);

            if (ilstProcedureLSTFinal != null && ilstProcedureLSTFinal.Count > 0)
            {
                if (ilstProcedureLSTFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstProcedureLSTFinal[0]).Count; iCount++)
                    {
                        objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstProcedureLSTFinal[0])[iCount]);
                    }
                }
            }

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
            //        //  itemDoc.Load(XmlText);
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
            //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("PROCEDURES"))
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
            //    if(XmlText!=null)
            //    XmlText.Close();

            //    throw Ex;
            //}

            #endregion

            iTryCount = 0;

        TryAgain:
            int iResult = 0;
            Session.GetISession().Clear();
            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        bool IsProcedureConsistent = true, IsTreatmentPlanConsistent = true;
                        if ((lstSave != null && lstSave.Count > 0) || (lstUpdate != null && lstUpdate.Count > 0) || (lstDelete != null && lstDelete.Count > 0))
                        {
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref lstSave, ref lstUpdate, lstDelete, MySession, MACAddress, true, true, HumanID, string.Empty, ref ObjXmlHuman);

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
                            IList<InHouseProcedure> Combinedlst = new List<InHouseProcedure>();
                            if (lstSave.Count > 0 && lstUpdate.Count > 0)
                                Combinedlst = lstSave.Concat(lstUpdate).ToList<InHouseProcedure>();
                            else if (lstSave.Count > 0)
                                Combinedlst = lstSave;
                            else
                                Combinedlst = lstUpdate;
                            IsProcedureConsistent = ObjXmlHuman.CheckDataConsistency(Combinedlst.Cast<object>().ToList(), true, string.Empty);

                            
                            #region Tplan

                            if (lstSave != null && lstSave.Count > 0)
                            {
                                foreach (InHouseProcedure obj in lstSave)
                                {
                                    TreatmentPlan item = new TreatmentPlan();
                                    item.Human_ID = obj.Human_ID;
                                    item.Encounter_Id = obj.Encounter_ID;
                                    item.Physician_Id = obj.Physician_ID;
                                    item.Created_By = obj.Created_By;
                                    item.Created_Date_And_Time = obj.Created_Date_And_Time;
                                    item.Plan_Type = "PROCEDURES";
                                    string plan_txt = string.Empty;
                                    plan_txt = "* " + obj.Procedure_Code_Description + (obj.Notes != string.Empty ? "-" + obj.Notes : "");
                                    item.Plan = plan_txt;
                                    item.Version = 0;
                                    item.Source_ID = obj.Id;
                                    item.Local_Time = sLocalTime;
                                    Insert_Tplan.Add(item);
                                }
                            }
                            if (lstUpdate != null && lstUpdate.Count > 0)
                            {
                                foreach (InHouseProcedure obj in lstUpdate)
                                {
                                    if (objTreatmentPlan.Count > 0)
                                    {
                                        IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Source_ID == obj.Id).ToList();
                                        if (itemlst != null && itemlst.Count > 0)
                                        {
                                            TreatmentPlan item = itemlst[0];
                                            string plan_txt = string.Empty;
                                            plan_txt = "* " + obj.Procedure_Code_Description + (obj.Notes != string.Empty ? "-" + obj.Notes : "");
                                            item.Plan = plan_txt;
                                            item.Modified_By = obj.Modified_By;
                                            item.Modified_Date_And_Time = obj.Modified_Date_And_Time;
                                            Update_Tplan.Add(item);
                                        }

                                    }

                                }
                            }

                            if (lstDelete != null && lstDelete.Count > 0)
                                if (objTreatmentPlan.Count > 0)
                                {
                                    Delete_Tplan = (from item in objTreatmentPlan where lstDelete.Any(a => a.Id == item.Source_ID) select item).ToList();
                                }
                            #endregion
                            if ((Insert_Tplan.Count > 0 && Insert_Tplan[0].Encounter_Id != 0) || (Update_Tplan.Count > 0 && Update_Tplan[0].Encounter_Id != 0) || (Delete_Tplan.Count > 0 && Delete_Tplan[0].Encounter_Id != 0))
                            {
                                iResult = objTreatmentPlanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, Delete_Tplan, MySession, MACAddress, true, true, EncounterID, string.Empty, ref ObjXmlEncounter);

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
                            IList<TreatmentPlan> Combinedlist = new List<TreatmentPlan>();
                            if ((Insert_Tplan.Count > 0 && Insert_Tplan.Count > 0) || (Update_Tplan.Count > 0 && Update_Tplan.Count > 0))
                                Combinedlist = Insert_Tplan.Concat(Update_Tplan).ToList<TreatmentPlan>();
                            else if (Insert_Tplan.Count > 0)
                                Combinedlist = Insert_Tplan;
                            else
                                Combinedlist = Update_Tplan;
                            IsTreatmentPlanConsistent = ObjXmlEncounter.CheckDataConsistency(Combinedlist.Cast<object>().ToList(), true, string.Empty);
                            if (IsProcedureConsistent && IsTreatmentPlanConsistent)
                            {
                                try
                                {
                                    if (ObjXmlHuman != null )
                                    {
                                        //ObjXmlHuman.itemDoc.Save(ObjXmlHuman.strXmlFilePath);

                                        int trycount = 0;
                                    trytosaveagain:
                                        try
                                        {
                                           // ObjXmlHuman.itemDoc.Save(ObjXmlHuman.strXmlFilePath);
                                            WriteBlob(HumanID, ObjXmlHuman.itemDoc, MySession, lstSave, lstUpdate, null, ObjXmlHuman, false);

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
                                    if (ObjXmlEncounter !=null)
                                    {
                                       // ObjXmlEncounter.itemDoc.Save(ObjXmlEncounter.strXmlFilePath);

                                        int trycount = 0;
                                    trytosaveagain:
                                        try
                                        {
                                            //ObjXmlEncounter.itemDoc.Save(ObjXmlEncounter.strXmlFilePath);
                                            objTreatmentPlanManager.WriteBlob(EncounterID, ObjXmlEncounter.itemDoc, MySession, Insert_Tplan, Update_Tplan, null, ObjXmlEncounter, false);
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
                        if (MySession.IsOpen)
                        {
                            MySession.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }

            objInHouseProcdureDTO = LoadInHouseProcedure(lstProcedure[0].Encounter_ID, lstProcedure[0].Physician_ID, lstProcedure[0].Human_ID,sLegalOrg);
            return objInHouseProcdureDTO;
        }

        public InHouseProcedureDTO DeleteInHouseProcedureBYList(IList<InHouseProcedure> lstDelete, string MACAddress, string sPln, string sLegalOrg)
        {
            IList<InHouseProcedure> Savelist = new List<InHouseProcedure>();
            IList<InHouseProcedure> updatelist = new List<InHouseProcedure>();
            IList<InHouseProcedure> deletelist = new List<InHouseProcedure>();
            IList<InHouseProcedure> ListProcedure_old = new List<InHouseProcedure>();
            FileManagementIndexManager filemanagementmgr = new FileManagementIndexManager();
            IList<FileManagementIndex> addlist = new List<FileManagementIndex>();
            IList<FileManagementIndex> lstupdate = new List<FileManagementIndex>();
            IList<FileManagementIndex> lstdelete = new List<FileManagementIndex>();
            FileManagementIndex obj_fileManagement = new FileManagementIndex();
            InHouseProcedureDTO objInHouseProcdureDTO = new InHouseProcedureDTO();
            //ulong uIn_House_Procedure_Group_ID = 0;
            ulong EncounterID = lstDelete[0].Encounter_ID;
            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            GenerateXml ObjXmlHuman = new GenerateXml();
            GenerateXml ObjXmlEncounter = new GenerateXml();
            IList<TreatmentPlan> Insert_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Update_Tplan = new List<TreatmentPlan>();
            IList<TreatmentPlan> Delete_Tplan = new List<TreatmentPlan>();
            iTryCount = 0;
            #region TplanGet

            IList<string> ilstProcedureTagList = new List<string>();
            ilstProcedureTagList.Add("TreatmentPlanList");

            IList<object> ilstProcedureFinal = new List<object>();
            ilstProcedureFinal = ReadBlob(EncounterID, ilstProcedureTagList);

            if (ilstProcedureFinal != null && ilstProcedureFinal.Count > 0)
            {
                if (ilstProcedureFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstProcedureFinal[0]).Count; iCount++)
                    {
                        objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstProcedureFinal[0])[iCount]);
                    }
                }
            }

            //    string FileName = "Encounter" + "_" + EncounterID + ".xml";
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
            //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == EncounterID && Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Plan_Type").Value).Equals("PROCEDURES"))
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
        TryAgain:
            int iResult = 0;
            Session.GetISession().Clear();
            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if (lstDelete != null && lstDelete.Count > 0)
                        {
                            iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Savelist, ref updatelist, lstDelete, MySession, MACAddress, true, true, lstDelete[0].Human_ID, string.Empty, ref ObjXmlHuman);

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
                            Delete_Tplan = (from obj in objTreatmentPlan where lstDelete.Any(a => a.Id == obj.Source_ID) select obj).ToList<TreatmentPlan>();
                            if (Delete_Tplan != null && Delete_Tplan.Count > 0)
                            {
                                iResult = objTreatmentPlanManager.SaveUpdateDelete_DBAndXML_WithoutTransaction(ref Insert_Tplan, ref Update_Tplan, Delete_Tplan, MySession, MACAddress, true, true, EncounterID, string.Empty, ref ObjXmlEncounter);

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
                               if (ObjXmlHuman != null)
                                {
                                   // ObjXmlHuman.itemDoc.Save(ObjXmlHuman.strXmlFilePath);
                                    int trycount = 0;
                                trytosaveagain:
                                    try
                                    {
                                       // ObjXmlHuman.itemDoc.Save(ObjXmlHuman.strXmlFilePath);
                                        WriteBlob(lstDelete[0].Human_ID, ObjXmlHuman.itemDoc, MySession, null, null, lstDelete, ObjXmlHuman, false);
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
                                if (ObjXmlEncounter != null )
                                    {
                                    // ObjXmlEncounter.itemDoc.Save(ObjXmlEncounter.strXmlFilePath);

                                    int trycount = 0;
                                trytosaveagain:
                                try
                                    {
                                        //ObjXmlEncounter.itemDoc.Save(ObjXmlEncounter.strXmlFilePath);
                                        objTreatmentPlanManager.WriteBlob(EncounterID, ObjXmlEncounter.itemDoc, MySession, Insert_Tplan, Update_Tplan, Delete_Tplan, ObjXmlEncounter, false);
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
            catch (Exception ex)
            {
                //CAP-1942
                throw new Exception(ex.Message,ex);
            }
          
            objInHouseProcdureDTO = LoadInHouseProcedure(lstDelete[0].Encounter_ID, lstDelete[0].Physician_ID, lstDelete[0].Human_ID,sLegalOrg);
            return objInHouseProcdureDTO;

        }

        #region IOtherProcedureManager Members
        //not in use
        //public int BatchOperationsToInHouseProcedure(IList<InHouseProcedure> savelist, IList<InHouseProcedure> updtList, IList<InHouseProcedure> delList, ISession MySession, string MACAddress)
        //{
        //    GenerateXml XMLObj = new GenerateXml();
        //    return SaveUpdateDelete_DBAndXML_WithoutTransaction(ref savelist, ref updtList, delList, MySession, MACAddress, false, false, 0, string.Empty, ref XMLObj);
        //}
        public object SubmitInHouseProcedures(IList<InHouseProcedure> orderList, string MACAddress)
        {
            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            GenerateXml XMLObj = new GenerateXml();
            ISession MySession = Session.GetISession();
            try
            {
                using (ITransaction trans = MySession.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        if (orderList != null)
                        {
                            if (orderList.Count > 0)
                            {
                                IList<InHouseProcedure> list = null;
                                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref list, ref orderList, null, MySession, MACAddress, true, false, orderList[0].Human_ID, string.Empty, ref XMLObj);
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
                                WFObj.Obj_Type = "INTERNAL ORDER";
                                WFObj.Current_Arrival_Time = orderList[0].Modified_Date_And_Time;
                                WFObj.Current_Owner = "UNKNOWN";
                                WFObj.Fac_Name = orderList[0].Facility_Name;
                                WFObj.Parent_Obj_Type = string.Empty;
                                WFObj.Obj_System_Id = orderList[0].In_House_Procedure_Group_ID;
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
                                        throw new Exception("Deadlock occurred. Transaction failed.");
                                    }
                                }
                                else if (iResult == 1)
                                {
                                    trans.Rollback();
                                    throw new Exception("Exception occurred. Transaction failed.");

                                }
                            }
                            MySession.Flush();
                          //  if (XMLObj.strXmlFilePath != null && XMLObj.strXmlFilePath != "")
                            {
                               // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                int trycount = 0;
                            trytosaveagain:
                                try
                                {
                                    //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                    WriteBlob(orderList[0].Human_ID, XMLObj.itemDoc, MySession, null, orderList, null, XMLObj, false);
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
                MySession.Close();
                //CAP-1942
                throw new Exception(ex1.Message,ex1);
            }
            return (object)orderList[0].In_House_Procedure_Group_ID;
        }
        #endregion


        public IList<FileManagementIndex> GetList(ulong EncounterId, ulong HumanId)
        {

            InHouseProcedureDTO objDTO = new InHouseProcedureDTO();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                objDTO.OtherProcedure = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Encounter_ID", EncounterId)).Add(Expression.Eq("Human_ID", HumanId)).AddOrder(Order.Desc("Modified_Date_And_Time")).List<InHouseProcedure>();

                //For fill the fileName
                for (int k = 0; k < objDTO.OtherProcedure.Count; k++)
                {
                    if (objDTO.OtherProcedure[k].File_Management_Index_ID != string.Empty && objDTO.OtherProcedure[k].File_Management_Index_ID.Contains('|'))
                    {
                        foreach (string id in objDTO.OtherProcedure[k].File_Management_Index_ID.Split('|').ToArray().Where(A => A != null && A != string.Empty).ToArray())
                        {
                            IList<FileManagementIndex> lstFile = session.GetISession().CreateCriteria(typeof(FileManagementIndex)).Add(Expression.Eq("Id", Convert.ToUInt64(id))).Add(Expression.Eq("Source", "Dermatology")).List<FileManagementIndex>();
                            if (lstFile.Count > 0)
                            {
                                if (objDTO.OtherProcedure[k].Internal_Property_File_Name == string.Empty)
                                    objDTO.OtherProcedure[k].Internal_Property_File_Name = lstFile[0].File_Path;
                                else
                                    objDTO.OtherProcedure[k].Internal_Property_File_Name += "|" + lstFile[0].File_Path;

                            }
                        }
                    }
                }
                iMySession.Close();
            }
            return new List<FileManagementIndex>();
        }

        public void SaveProceduresforSummary(IList<InHouseProcedure> lstprocedure)
        {
            IList<InHouseProcedure> UpdateInHouseProc = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref lstprocedure, ref UpdateInHouseProc, null, string.Empty, false, false, 0, string.Empty);
        }

        //Added by Saravanakumar
        public IList<FillOrdersManagementDTO> SearchProcedureOrder(string order_type, string order_status, string FacilityName, int orderInDays, DateTime fromDate, DateTime toDate, ulong humanID, ulong physicianID, int pageNumber, int maxResults)
        {
            IList<FillOrdersManagementDTO> ilstFillOrdersManagementDTO = new List<FillOrdersManagementDTO>();
            FillOrdersManagementDTO objFillOrdersManagementDTO = new FillOrdersManagementDTO();
            IList<InHouseProcedure> otherProcList = new List<InHouseProcedure>();
            ArrayList arr = null;
            string human = string.Empty;
            string physician = string.Empty;
            string orderType = string.Empty;
            IList<WFObject> wfObjectList = new List<WFObject>();
            if (physicianID == 0)
            {
                physician = "%";
            }
            else
            {
                physician = physicianID.ToString();
            }
            if (humanID == 0)
            {
                human = "%";
            }
            else
            {
                human = humanID.ToString();
            }
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query;
                int startIndex = 0;
                startIndex = ((pageNumber - 1) * maxResults);
                query = iMySession.GetNamedQuery("Get.GetProcedureOrderSearch");
                query.SetString(0, human);
                query.SetString(1, physician);
                //CAP-2299
                query.SetString(2, fromDate.ToString("yyyy-MM-dd"));
                query.SetString(3, toDate.ToString("yyyy-MM-dd"));
                query.SetString(4, human);
                query.SetString(5, physician);
                query.SetString(6, FacilityName);
                query.SetString(7, fromDate.ToString("yyyy-MM-dd"));
                query.SetString(8, toDate.ToString("yyyy-MM-dd"));
                arr = new ArrayList(query.List());
                if (arr != null)
                {
                    for (int i = 0; i < arr.Count; i++)
                    {
                        objFillOrdersManagementDTO = new FillOrdersManagementDTO();
                        object[] obj = (object[])arr[i];
                        objFillOrdersManagementDTO.Facility_Name = obj[0].ToString();
                        objFillOrdersManagementDTO.Ordered_Date_And_Time = Convert.ToDateTime(obj[1].ToString());
                        objFillOrdersManagementDTO.Human_Id = (obj[2].ToString());
                        objFillOrdersManagementDTO.Human_Name = obj[3].ToString();
                        objFillOrdersManagementDTO.Date_Of_Birth = Convert.ToDateTime(obj[4]);
                        objFillOrdersManagementDTO.Physician_ID = Convert.ToUInt64(obj[5]);
                        objFillOrdersManagementDTO.Procedures = obj[6].ToString();
                        objFillOrdersManagementDTO.To_Physician_Name = obj[7].ToString();
                        objFillOrdersManagementDTO.Group_ID = Convert.ToUInt64(obj[8]);
                        objFillOrdersManagementDTO.Total_Record_Found = Convert.ToUInt64(arr.Count);
                        ilstFillOrdersManagementDTO.Add(objFillOrdersManagementDTO);

                    }
                }
                iMySession.Close();
            }
            return ilstFillOrdersManagementDTO;
        }

        public IList<InHouseProcedure> GetImplantableDeviceInformation(string sDeviceID, bool is_DI)
        { 
        IList<InHouseProcedure> ilistInhouseProcedure=new List<InHouseProcedure>();
        using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        {
            ICriteria iCriteria = null;
            if(is_DI==true)
                iCriteria = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Device_Identifier_DI", sDeviceID)).SetMaxResults(1);
            else
                iCriteria = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Eq("Device_Identifier_UDI", sDeviceID)).SetMaxResults(1);

            ilistInhouseProcedure = iCriteria.List<InHouseProcedure>();
            iMySession.Close();
        }


        return ilistInhouseProcedure;
        }
        //iCriteria = iMySession.CreateCriteria(typeof(InHouseProcedure)).Add(Expression.Or(Expression.Eq("Device_Identifier_UDI", sDeviceID), Expression.Eq("Device_Identifier_DI", sDeviceID))).SetMaxResults(1);

    }
}
