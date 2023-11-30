using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IAddendumNotesManager : IManagerBase<AddendumNotes, ulong>
    {
        //FillPatientChart loadAddendum(ulong humanID, ulong encounterID, DateTime LocalDate, String userName, string macAddress);
        void saveAddendum(IList<AddendumNotes> saveList, string facilityName, String ownerName, String userRole, bool isReview, string macAddress);
        void updateAddendum(IList<AddendumNotes> saveList, string facilityName, String ownerName, int closeType, string macAddress);
        IList<AddendumNotes> getAddendumNotesForPhysician(ulong addendumID, ulong encounterID);
        void moveToAddendum(IList<AddendumNotes> loadList, string facilityName, String ownerName, String userRole, bool isSave, bool isReview, int closeType, string macAddress, bool bAddendumSaved, GenerateXml objGenerateXML);
        void saveUpdateAddendum(IList<AddendumNotes> saveList, IList<AddendumNotes> updateList, string facilityName, String ownerName, String userRole, bool isSave, bool isReview, int closeType, bool isDirectMoveToProvider, string macAddress, bool bSaveXML);
    }

    public partial class AddendumNotesManager : ManagerBase<AddendumNotes, ulong>, IAddendumNotesManager
    {
        #region Constructors

        public AddendumNotesManager()
            : base()
        {
        }

        public AddendumNotesManager(INHibernateSession session)
            : base(session)
        {
        }

        #endregion

        #region Methods

        //public FillPatientChart loadAddendum(ulong humanID, ulong encounterID, DateTime LocalDate, String userName, string macAddress)
        //{
        //    FillPatientChart pateintChart = null;

        //    EncounterManager encounterManagerObj = new EncounterManager();
        //    pateintChart = encounterManagerObj.LoadPatientChart(humanID, encounterID, LocalDate, macAddress, userName, true);

        //    return pateintChart;
        //}

        int iTryCount = 0;

        public void saveAddendum(IList<AddendumNotes> saveList, string facilityName, String ownerName, String userRole, bool isReview, string macAddress)
        {
            iTryCount = 0;
        TryAgain:
            int iResult = 0;
           
                ISession MySession = Session.GetISession();
                ITransaction trans = null;
                try
                {
                    trans = MySession.BeginTransaction();

                    if (saveList != null && saveList.Count > 0)
                    {
                        iResult = SaveUpdateDeleteWithoutTransaction(ref saveList, null, null, MySession, macAddress);
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

                        //WFObject WFObj = new WFObject();
                        //WFObj.Obj_Type = "ADDENDUM";
                        //WFObj.Current_Arrival_Time = saveList[0].Created_Date_And_Time;
                        //WFObj.Current_Owner = ownerName == string.Empty && userRole.ToUpper() == "PHYSICIAN" ? "UNKNOWN" : ownerName;
                        //WFObj.Fac_Name = facilityName;
                        //WFObj.Parent_Obj_Type = string.Empty;
                        //WFObj.Obj_System_Id = saveList[0].Id;
                        //WFObj.Parent_Obj_System_Id = saveList[0].Encounter_ID;
                        //WFObj.Current_Process = "START";
                        //WFObjectManager objWfMnger = new WFObjectManager();
                        //iResult = objWfMnger.InsertToWorkFlowObject(WFObj, userRole == string.Empty ? 1 : userRole.ToUpper() == "PHYSICIAN" ? 2 : !isReview ? 2 : 3, macAddress, MySession);

                        //if (iResult == 2)
                        //{
                        //    if (iTryCount < 5)
                        //    {
                        //        iTryCount++;
                        //        goto TryAgain;
                        //    }
                        //    else
                        //    {
                        //        trans.Rollback();
                        //        throw new Exception("Deadlock occurred. Transaction failed.");
                        //    }
                        //}
                        //else if (iResult == 1)
                        //{
                        //    trans.Rollback();
                        //    throw new Exception("Exception occurred. Transaction failed.");

                        //}
                        MySession.Flush();
                        trans.Commit();
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

        public void updateAddendum(IList<AddendumNotes> updateList, string facilityName, String ownerName, int closeType, string macAddress)
        {
            iTryCount = 0;
        TryAgain:
            int iResult = 0;
            
                ISession MySession = Session.GetISession();
                ITransaction trans = null;
                try
                {
                    trans = MySession.BeginTransaction();

                    if (updateList != null && updateList.Count > 0)
                    {
                        IList<AddendumNotes> saveList = null;
                        iResult = SaveUpdateDeleteWithoutTransaction(ref saveList, updateList, null, MySession, macAddress);
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

                        //if (ownerName == "UNKNOWN")
                        //{
                        //    ICriteria criteria = Session.GetISession().CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", updateList[0].Id)).Add(Expression.Eq("Obj_Type", "ADDENDUM"));

                        //    if (criteria.List<WFObject>().Count > 0)
                        //        ownerName = criteria.List<WFObject>()[0].Process_Allocation.Split('|').FirstOrDefault(s => s.StartsWith("ADDENDUM_CODING")).Split('-')[1];
                        //}
                        //else
                        //    ownerName = closeType == 1 && (ownerName == string.Empty) ? "UNKNOWN" : ownerName;

                        //WFObjectManager objWfMnger = new WFObjectManager();
                        //iResult = objWfMnger.MoveToNextProcess(updateList[0].Id, "ADDENDUM", closeType, ownerName, updateList[0].Provider_Review_Signed_Date_And_Time, macAddress, null, MySession);

                        //if (iResult == 2)
                        //{
                        //    if (iTryCount < 5)
                        //    {
                        //        iTryCount++;
                        //        goto TryAgain;
                        //    }
                        //    else
                        //    {
                        //        trans.Rollback();
                        //        throw new Exception("Deadlock occurred. Transaction failed.");
                        //    }
                        //}
                        //else if (iResult == 1)
                        //{
                        //    trans.Rollback();
                        //    throw new Exception("Exception occurred. Transaction failed.");

                        //}
                        MySession.Flush();
                        trans.Commit();
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

        public void saveUpdateAddendum(IList<AddendumNotes> saveList, IList<AddendumNotes> updateList, string facilityName, String ownerName, String userRole, bool isSave, bool isReview, int closeType, bool isDirectMoveToProvider, string macAddress, bool bSaveXML)
        {
            iTryCount = 0;
        TryAgain:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = MySession.BeginTransaction();

                if ((saveList != null && saveList.Count > 0) || (updateList != null && updateList.Count > 0))
                {
                    ulong encID = 0;
                    EncounterManager encounterMngr = new EncounterManager();
                    if (saveList != null && saveList.Count > 0)
                        encID = saveList[0].Encounter_ID;
                    else
                        if (updateList != null && updateList.Count > 0)
                        encID = updateList[0].Encounter_ID;
                    GenerateXml XMLObj = new GenerateXml();
                    //iResult = SaveUpdateDeleteWithoutTransaction(ref saveList, updateList, null, MySession, macAddress);
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref updateList, null, MySession, macAddress, true, true, encID, string.Empty, ref XMLObj);
                    bool bAddendumSaved = false;
                    if (saveList != null && saveList.Count > 0)
                        bAddendumSaved = XMLObj.CheckDataConsistency(saveList.Cast<object>().ToList(), true, string.Empty);
                    else
                        bAddendumSaved = XMLObj.CheckDataConsistency(updateList.Cast<object>().ToList(), true, string.Empty);
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

                    if (encID != 0)
                    {
                        //IList<Encounter> encRecord = encounterMngr.GetEncounterByEncounterID(encID);
                        //encRecord[0].Is_Progressnote_Generated = "N";
                        //IList<Encounter> emptySave=null;
                        //encounterMngr.SaveUpdateDeleteWithTransaction(ref emptySave, encRecord, null, string.Empty);
                    }
                    if (saveList != null && saveList.Count > 0)
                    {
                        //Jira #CAP-598
                        //closeType = isDirectMoveToProvider ? closeType : userRole.ToUpper() == "MEDICAL ASSISTANT" ? 4 : 1;
                        closeType = isDirectMoveToProvider ? closeType : userRole.ToUpper() == "MEDICAL ASSISTANT" || userRole.ToUpper() == "OFFICE MANAGER" ? 4 : 1;
                        //moveToAddendum(saveList, facilityName, ownerName, userRole, isSave, isReview, closeType, macAddress, bAddendumSaved, XMLObj, trans,MySession);
                        if (isSave)
                        {
                            string userName = string.Empty;
                            WFObject WFObj = new WFObject();
                            WFObj.Obj_Type = "ADDENDUM";
                            WFObj.Current_Arrival_Time = saveList[0].Created_Date_And_Time;
                            //UserManager userManager = new UserManager();//Commented for Bug ID:37104
                            //IList<User> userList = userManager.GetUserList();
                            //userName = userList.Any(a => a.person_name.ToUpper() == loadList[0].Created_By.ToUpper()) ? userList.Where(d => d.person_name.ToUpper() == loadList[0].Created_By.ToUpper()).Select(u => u.user_name).ToList()[0] : string.Empty;
                            //WFObj.Current_Owner = userName == string.Empty ? ownerName == string.Empty && userRole.ToUpper() == "PHYSICIAN" ? "UNKNOWN" : ownerName : ownerName == string.Empty ? userName : ownerName;
                            WFObj.Current_Owner = ownerName;
                            WFObj.Fac_Name = facilityName;
                            WFObj.Parent_Obj_Type = string.Empty;
                            WFObj.Obj_System_Id = saveList[0].Id;
                            WFObj.Parent_Obj_System_Id = saveList[0].Encounter_ID;
                            WFObj.Current_Process = "START";
                            WFObjectManager objWfMnger = new WFObjectManager();
                            iResult = objWfMnger.InsertToWorkFlowObject(WFObj, closeType, macAddress, MySession);
                            //iResult = objWfMnger.InsertToWorkFlowObject(WFObj, (userRole == string.Empty || userRole.ToUpper() == "PHYSICIAN ASSISTANT") ? 1 : userRole.ToUpper() == "PHYSICIAN" ? 2 : userRole.ToUpper() == "MEDICAL ASSISTANT" ? 4 : 3, macAddress, MySession);
                            //iResult = objWfMnger.InsertToWorkFlowObject(WFObj, userRole == string.Empty ? 1 : userRole.ToUpper() == "PHYSICIAN" ? 2 : userRole.ToUpper() == "MEDICAL ASSISTANT" ? 4 : !isReview ? 2 : 3, macAddress, MySession);

                            WriteBlob(saveList[0].Encounter_ID, XMLObj.itemDoc, MySession, saveList, null, null, XMLObj, false);

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
                        if (bAddendumSaved)
                        {
                            trans.Commit();

                            //// XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            ////    int trycount = 0;
                            ////trytosaveagain:
                            //try
                            //{
                            //    //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                            //    WriteBlob(encID, XMLObj.itemDoc, MySession, null, saveList, updateList, XMLObj, false);

                            //}
                            //catch (Exception xmlexcep)
                            //{
                            //    //trycount++;
                            //    //if (trycount <= 3)
                            //    //{
                            //    //    int TimeMilliseconds = 0;
                            //    //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                            //    //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                            //    //    Thread.Sleep(TimeMilliseconds);
                            //    //    string sMsg = string.Empty;
                            //    //    string sExStackTrace = string.Empty;

                            //    //    string version = "";
                            //    //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                            //    //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                            //    //    string[] server = version.Split('|');
                            //    //    string serverno = "";
                            //    //    if (server.Length > 1)
                            //    //        serverno = server[1].Trim();

                            //    //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                            //    //        sMsg = xmlexcep.InnerException.Message;
                            //    //    else
                            //    //        sMsg = xmlexcep.Message;

                            //    //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                            //    //        sExStackTrace = xmlexcep.StackTrace;

                            //    //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                            //    //    string ConnectionData;
                            //    //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                            //    //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                            //    //    {
                            //    //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                            //    //        {
                            //    //            cmd.Connection = con;
                            //    //            try
                            //    //            {
                            //    //                con.Open();
                            //    //                cmd.ExecuteNonQuery();
                            //    //                con.Close();
                            //    //            }
                            //    //            catch
                            //    //            {
                            //    //            }
                            //    //        }
                            //    //    }
                            //    //    goto trytosaveagain;
                            //    //}
                            //}

                        }
                        else
                            throw new Exception("Data inconsistent on Save in DB and XML. Please contact support");
                    }
                    //CAP-1227
                    else if (updateList != null && updateList.Count > 0)
                    {
                        //if (!isSave && (updateList != null && updateList.Count > 0))
                        //{
                            //moveToAddendum(updateList, facilityName, ownerName, userRole, isSave, isReview, closeType, macAddress, bAddendumSaved, XMLObj, trans, MySession);

                            if (!isSave)
                            {

                                if (ownerName == "UNKNOWN")
                                {
                                    ICriteria criteria = Session.GetISession().CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", updateList[0].Id)).Add(Expression.Eq("Obj_Type", "ADDENDUM"));
                                    IList<WFObject> lstWFObj = new List<WFObject>();
                                    lstWFObj = criteria.List<WFObject>();
                                    if (lstWFObj.Count > 0 && lstWFObj.Any(a => a.Process_Allocation.Contains("ADDENDUM_CODING")))
                                        ownerName = lstWFObj[0].Process_Allocation.Split('|').FirstOrDefault(s => s.StartsWith("ADDENDUM_CODING")).Split('-')[1];
                                }
                                else
                                    ownerName = closeType == 1 && (ownerName == "UNKNOWN") ? "UNKNOWN" : ownerName;

                                WFObjectManager objWfMnger = new WFObjectManager();
                                iResult = objWfMnger.MoveToNextProcess(updateList[0].Id, "ADDENDUM", closeType, ownerName, updateList[0].Provider_Review_Signed_Date_And_Time, macAddress, null, MySession);

                                WriteBlob(updateList[0].Encounter_ID, XMLObj.itemDoc, MySession, null, updateList, null, XMLObj, false);

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
                            if (bAddendumSaved)
                            {
                                trans.Commit();

                                //// XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);

                                ////    int trycount = 0;
                                ////trytosaveagain:
                                //try
                                //{
                                //    // XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                                //    WriteBlob(encID, XMLObj.itemDoc, MySession, null, saveList, updateList, XMLObj, false);
                                //}
                                //catch (Exception xmlexcep)
                                //{
                                //    //trycount++;
                                //    //if (trycount <= 3)
                                //    //{
                                //    //    int TimeMilliseconds = 0;
                                //    //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                //    //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                //    //    Thread.Sleep(TimeMilliseconds);
                                //    //    string sMsg = string.Empty;
                                //    //    string sExStackTrace = string.Empty;

                                //    //    string version = "";
                                //    //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                //    //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                //    //    string[] server = version.Split('|');
                                //    //    string serverno = "";
                                //    //    if (server.Length > 1)
                                //    //        serverno = server[1].Trim();

                                //    //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                //    //        sMsg = xmlexcep.InnerException.Message;
                                //    //    else
                                //    //        sMsg = xmlexcep.Message;

                                //    //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                                //    //        sExStackTrace = xmlexcep.StackTrace;

                                //    //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                //    //    string ConnectionData;
                                //    //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                //    //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                //    //    {
                                //    //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                //    //        {
                                //    //            cmd.Connection = con;
                                //    //            try
                                //    //            {
                                //    //                con.Open();
                                //    //                cmd.ExecuteNonQuery();
                                //    //                con.Close();
                                //    //            }
                                //    //            catch
                                //    //            {
                                //    //            }
                                //    //        }
                                //    //    }
                                //    //    goto trytosaveagain;
                                //    //}
                                //}
                            }
                            else
                                throw new Exception("Data inconsistent on Save in DB and XML. Please contact support");
                        // }
                        //else
                        //{
                        //    //MySession.Flush();
                        //    if (bAddendumSaved)
                        //    {
                        //        trans.Commit();
                        //        //// XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                        //        ////    int trycount = 0;
                        //        ////trytosaveagain:
                        //        //try
                        //        //{
                        //        //    //XMLObj.itemDoc.Save(XMLObj.strXmlFilePath);
                        //        //    WriteBlob(encID, XMLObj.itemDoc, MySession, null, saveList, updateList, XMLObj, false);
                        //        //}
                        //        //catch (Exception xmlexcep)
                        //        //{
                        //        //    //trycount++;
                        //        //    //if (trycount <= 3)
                        //        //    //{
                        //        //    //    int TimeMilliseconds = 0;
                        //        //    //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //        //    //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //        //    //    Thread.Sleep(TimeMilliseconds);
                        //        //    //    string sMsg = string.Empty;
                        //        //    //    string sExStackTrace = string.Empty;

                        //        //    //    string version = "";
                        //        //    //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //        //    //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //        //    //    string[] server = version.Split('|');
                        //        //    //    string serverno = "";
                        //        //    //    if (server.Length > 1)
                        //        //    //        serverno = server[1].Trim();

                        //        //    //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //        //    //        sMsg = xmlexcep.InnerException.Message;
                        //        //    //    else
                        //        //    //        sMsg = xmlexcep.Message;

                        //        //    //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //        //    //        sExStackTrace = xmlexcep.StackTrace;

                        //        //    //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //        //    //    string ConnectionData;
                        //        //    //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //        //    //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //        //    //    {
                        //        //    //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //        //    //        {
                        //        //    //            cmd.Connection = con;
                        //        //    //            try
                        //        //    //            {
                        //        //    //                con.Open();
                        //        //    //                cmd.ExecuteNonQuery();
                        //        //    //                con.Close();
                        //        //    //            }
                        //        //    //            catch
                        //        //    //            {
                        //        //    //            }
                        //        //    //        }
                        //        //    //    }
                        //        //    //    goto trytosaveagain;
                        //        //    //}
                        //        //}
                                //trans.Commit();
                        //    }
                        //    else
                        //        throw new Exception("Data inconsistent on Save in DB and XML. Please contact support");
                        //}
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
                if (isSave && (updateList != null && updateList.Count > 0))
                    MySession.Close();
            }

            //GenerateXml XMLObj = new GenerateXml();
            //if (bSaveXML == true)
            //{
            //    if (saveList != null && saveList.Count > 0)
            //    {
            //        XMLObj.GenerateXmlSaveStatic(saveList.Cast<object>().ToList(), saveList[0].Encounter_ID, string.Empty);
            //    }
            //    else
            //        if (updateList != null && updateList.Count > 0)
            //        {
            //            XMLObj.DeleteXmlNode(updateList[0].Encounter_ID, updateList.Cast<object>().ToList(), string.Empty);
            //            foreach (AddendumNotes item in updateList)
            //            {
            //                item.Version += 1;
            //            }
            //            XMLObj.GenerateXmlSaveStatic(updateList.Cast<object>().ToList(), updateList[0].Encounter_ID, string.Empty);
            //        }
            //}
        }

        public IList<AddendumNotes> getAddendumNotesForPhysician(ulong addendumID, ulong encounterID)
        {
            IList<AddendumNotes> ilstAddendumNotes = new List<AddendumNotes>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(AddendumNotes)).Add(Expression.Eq("Id", addendumID)).Add(Expression.Eq("Encounter_ID", encounterID));
                ilstAddendumNotes= criteria.List<AddendumNotes>();
                iMySession.Close();
            }
            return ilstAddendumNotes;
        }

        public void moveToAddendum(IList<AddendumNotes> loadList, string facilityName, String ownerName, String userRole, bool isSave, bool isReview, int closeType, string macAddress, bool bAddendumSaved, GenerateXml objGenerateXML)
        {
            iTryCount = 0;
        TryAgain:
            int iResult = 0;

            ISession MySession = Session.GetISession();
            ITransaction trans = null;
            try
            {
                trans = MySession.BeginTransaction();

                if (isSave)
                {
                    string userName = string.Empty;
                    WFObject WFObj = new WFObject();
                    WFObj.Obj_Type = "ADDENDUM";
                    WFObj.Current_Arrival_Time = loadList[0].Created_Date_And_Time;
                    //UserManager userManager = new UserManager();//Commented for Bug ID:37104
                    //IList<User> userList = userManager.GetUserList();
                    //userName = userList.Any(a => a.person_name.ToUpper() == loadList[0].Created_By.ToUpper()) ? userList.Where(d => d.person_name.ToUpper() == loadList[0].Created_By.ToUpper()).Select(u => u.user_name).ToList()[0] : string.Empty;
                    //WFObj.Current_Owner = userName == string.Empty ? ownerName == string.Empty && userRole.ToUpper() == "PHYSICIAN" ? "UNKNOWN" : ownerName : ownerName == string.Empty ? userName : ownerName;
                    WFObj.Current_Owner = ownerName;
                    WFObj.Fac_Name = facilityName;
                    WFObj.Parent_Obj_Type = string.Empty;
                    WFObj.Obj_System_Id = loadList[0].Id;
                    WFObj.Parent_Obj_System_Id = loadList[0].Encounter_ID;
                    WFObj.Current_Process = "START";
                    WFObjectManager objWfMnger = new WFObjectManager();
                    iResult = objWfMnger.InsertToWorkFlowObject(WFObj, closeType, macAddress, MySession);
                    //iResult = objWfMnger.InsertToWorkFlowObject(WFObj, (userRole == string.Empty || userRole.ToUpper() == "PHYSICIAN ASSISTANT") ? 1 : userRole.ToUpper() == "PHYSICIAN" ? 2 : userRole.ToUpper() == "MEDICAL ASSISTANT" ? 4 : 3, macAddress, MySession);
                    //iResult = objWfMnger.InsertToWorkFlowObject(WFObj, userRole == string.Empty ? 1 : userRole.ToUpper() == "PHYSICIAN" ? 2 : userRole.ToUpper() == "MEDICAL ASSISTANT" ? 4 : !isReview ? 2 : 3, macAddress, MySession);

                    WriteBlob(loadList[0].Encounter_ID, objGenerateXML.itemDoc, MySession, loadList, null, null, objGenerateXML, false);

                }
                else
                {

                    if (ownerName == "UNKNOWN")
                    {
                        ICriteria criteria = Session.GetISession().CreateCriteria(typeof(WFObject)).Add(Expression.Eq("Obj_System_Id", loadList[0].Id)).Add(Expression.Eq("Obj_Type", "ADDENDUM"));
                        IList<WFObject> lstWFObj = new List<WFObject>();
                        lstWFObj = criteria.List<WFObject>();
                        if (lstWFObj.Count > 0 && lstWFObj.Any(a => a.Process_Allocation.Contains("ADDENDUM_CODING")))
                            ownerName = lstWFObj[0].Process_Allocation.Split('|').FirstOrDefault(s => s.StartsWith("ADDENDUM_CODING")).Split('-')[1];
                    }
                    else
                        ownerName = closeType == 1 && (ownerName == "UNKNOWN") ? "UNKNOWN" : ownerName;

                    WFObjectManager objWfMnger = new WFObjectManager();
                    iResult = objWfMnger.MoveToNextProcess(loadList[0].Id, "ADDENDUM", closeType, ownerName, loadList[0].Provider_Review_Signed_Date_And_Time, macAddress, null, MySession);

                    WriteBlob(loadList[0].Encounter_ID, objGenerateXML.itemDoc, MySession, null, loadList, null, objGenerateXML, false);

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

                MySession.Flush();
                if (bAddendumSaved)
                    trans.Commit();
                else
                    throw new Exception("Data inconsistent on Save in DB and XML. Please contact support");
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
        #endregion
    }
}
