using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.IO;

using System.Net;
using System.Threading;
using System.Diagnostics;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.DataAccess.QuestResultService;
using Mapping;


namespace Acurus.Capella.LabAgent
{
    public partial class OrdersUtilityManager
    {
        public static class LabAgentStaticManager
        {
            public static string sICDVersion = string.Empty;
        }
        public void CreateOrders()
        {
            //ResultMasterProxy ObjResulMasterProxy = new ResultMasterProxy();
            //IList<LabSettings> ObjLabsettings = new List<LabSettings>();
            //ObjLabsettings = ObjResulMasterProxy.GetLabcorpSettings(2);
            //QuestProxy ObjQuestProxy = new QuestProxy();
            string ResultURL = string.Empty;
            WFObjectManager WFProxy = new WFObjectManager();
            IList<FillLabAgentDTO> ilstFillLabAgentDTO = new List<FillLabAgentDTO>();
            List<string> Current_Process = new List<string>();
            // List<string> LabCenters = new List<string>();//commented by naveena 10.5.2017

            Current_Process.Add("ORDER_GENERATE");
            Current_Process.Add("ORDER_SEND");
            //LabCenters.Add("LabCorp");
            //LabCenters.Add("Quest Diagnostics");
            //LabCenters.Add("CMG and Ancillary-Image");
            LabcorpSettingsManager objLabcorpSettingsManager = new LabcorpSettingsManager();
            LabManager objLabManager = new LabManager();
            IList<LabSettings> ilstLabSetting = objLabcorpSettingsManager.GetLabcorpSettings();
            IList<Lab> ilstLabList = objLabManager.GetLabList();
            IList<ulong> LabIdForQuest = (from lst in ilstLabList where lst.Lab_Name == "Quest Diagnostics" && lst.Lab_Type == "LAB" select lst.Id).ToList<ulong>();
            IList<ulong> LabIdForLabCorp = (from lst in ilstLabList where lst.Lab_Name == "LabCorp" && lst.Lab_Type == "LAB" select lst.Id).ToList<ulong>();
            IList<LabSettings> objLabSettingsForQuest = (from lst in ilstLabSetting where lst.Lab_ID == LabIdForQuest[0] select lst).ToList<LabSettings>();
            IList<LabSettings> objLabSettingsForLabCorp = (from lst in ilstLabSetting where lst.Lab_ID == LabIdForLabCorp[0] select lst).ToList<LabSettings>();
            string ConnectionStringForQuest = string.Empty;
            string ConnectionStringForLabcorp = string.Empty;
            OrdersSample os = new OrdersSample();
            if (objLabSettingsForLabCorp.Count > 0)
                ConnectionStringForLabcorp = objLabSettingsForLabCorp[0].Credential_Informantion;
            if (objLabSettingsForQuest.Count > 0)
            {
                ConnectionStringForQuest = objLabSettingsForQuest[0].Credential_Informantion;
                ResultURL = objLabSettingsForQuest[0].Result_URL;
                os = new OrdersSample(objLabSettingsForQuest[0].User_Name, objLabSettingsForQuest[0].Password, objLabSettingsForQuest[0].URL, objLabSettingsForQuest[0].Lab_Practice_Application, objLabSettingsForQuest[0].Lab_Practice_ID, objLabSettingsForQuest[0].Receiving_Facility);
            }
            ilstFillLabAgentDTO = WFProxy.GetWfObjectBasedOnLab(Current_Process, "DIAGNOSTIC ORDER");
            IList<FillLabAgentDTO> OrderGeneratedList = new List<FillLabAgentDTO>();
            IList<FillLabAgentDTO> OrderSendList = new List<FillLabAgentDTO>();
            if (ilstFillLabAgentDTO.Count() > 0)
            {
                OrderGeneratedList = (from dto in ilstFillLabAgentDTO where dto.Current_Process == "ORDER_GENERATE" select dto).ToList<FillLabAgentDTO>();
                OrderSendList = (from dto in ilstFillLabAgentDTO where dto.Current_Process == "ORDER_SEND" select dto).ToList<FillLabAgentDTO>();
            }
            Console.Out.WriteLine("Mapping Application");
            try
            {
                string filepathQuest = System.Configuration.ConfigurationSettings.AppSettings["QuestSenderFilePathName"];
                string filepathLabCorp = System.Configuration.ConfigurationSettings.AppSettings["LabCorpSenderFilePathName"];
                string sQuestService = System.Configuration.ConfigurationSettings.AppSettings["QuestServiceMode"];

                string TimeDelay = System.Configuration.ConfigurationSettings.AppSettings["TimeDelayToSendOrderInMins"];
                TraceTargetConsole ttc = new TraceTargetConsole();
                MappingMapToORM_O01 MappingMapToORM_O01Object = new MappingMapToORM_O01();
                MappingMapToORM_O01_Quest MappingMapToORM_O01_Quest_Object = new MappingMapToORM_O01_Quest();
                MappingMapToORM_O01Object.RegisterTraceTarget(ttc);
                DirectoryInfo dir = new DirectoryInfo(filepathLabCorp);
                if (!Directory.Exists(filepathLabCorp))
                {
                    Directory.CreateDirectory(filepathLabCorp);
                }
                if (!Directory.Exists(filepathQuest))
                {
                    Directory.CreateDirectory(filepathQuest);
                }
                if (!Directory.Exists(filepathQuest + "\\Send"))
                    Directory.CreateDirectory(filepathQuest + "\\Send");

                if (OrderGeneratedList.Count > 0)
                {
                    foreach (FillLabAgentDTO obj in OrderGeneratedList)
                    {
                        if ((obj.Is_Submit_Imediately == "Y") ||
                            ((obj.Is_Submit_Imediately == "N") && obj.Current_Arrival_Time.AddMinutes(Convert.ToDouble(TimeDelay)) <= DateTime.Now) ||
                            (obj.Is_Submit_Imediately == ""))
                        {
                            if (obj.LabName == "LabCorp" && obj.Order_Code_Type.Trim() != "")
                            {
                                string sOrderFileName = filepathLabCorp + "\\ACUR" + obj.WF_Obj_System_ID.ToString() + ".hl7";
                                string sOrderCheckSend = filepathLabCorp + "\\Send\\ACUR" + obj.WF_Obj_System_ID.ToString() + ".hl7";

                                Altova.IO.Output ORM_O01Target = new Altova.IO.FileOutput(sOrderFileName);
                                try
                                {
                                    if (File.Exists(sOrderCheckSend) == false)
                                    {
                                        MappingMapToORM_O01Object.Run(ConnectionStringForLabcorp, Altova.CoreTypes.CastToInt((int)obj.WfObj_ID), ORM_O01Target);
                                    }

                                    //08-11-2017 DB based process 
                                    string[] MatchedFiles = Directory.GetFiles(filepathLabCorp + "\\Send", "ACUR" + obj.WF_Obj_System_ID.ToString() + ".hl7", SearchOption.AllDirectories);
                                    if (MatchedFiles != null && MatchedFiles.Length != 0)
                                    {
                                        WFProxy.MoveToNextProcess(obj.WF_Obj_System_ID, obj.Obj_Type, 1, "UNKNOWN", DateTime.Now, string.Empty, null, null);
                                    }
                                }
                                catch (Exception e)
                                {
                                    StringBuilder logmsg = new StringBuilder();
                                    logmsg.Append("Error : Orders Date and Time : " + DateTime.Now.ToString() + Environment.NewLine);
                                    logmsg.Append("Move to Next Process for file name : ACUR" + obj.WF_Obj_System_ID.ToString() + ".hl7." + Environment.NewLine);
                                    logmsg.Append("Error Message : " + e.Message.ToString() + Environment.NewLine);
                                    if (e.InnerException != null)
                                        logmsg.Append(e.InnerException.Message != null ? "InnerException Message : " + e.InnerException.Message.ToString() + Environment.NewLine : "");
                                    else
                                        logmsg.Append("Error : " + e.ToString() + Environment.NewLine);
                                    Console.WriteLine(e.Message);
                                    using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                                    {
                                        tx.WriteLine(logmsg);
                                    }
                                }

                            }
                            else if (obj.LabName == "Quest Diagnostics" && obj.Order_Code_Type.Trim() != "")
                            {
                                string sOrderFileName = filepathQuest + "\\ACUR" + obj.WF_Obj_System_ID.ToString() + ".hl7";
                                string sOrderCheckSend = filepathQuest + "\\Send\\ACUR" + obj.WF_Obj_System_ID.ToString() + ".hl7";

                                Altova.IO.Output ORM_O01Target = new Altova.IO.FileOutput(sOrderFileName);
                                try
                                {
                                    if (File.Exists(sOrderCheckSend) == false)
                                    {
                                        MappingMapToORM_O01_Quest_Object.Run(ConnectionStringForQuest, Altova.CoreTypes.CastToInt((int)obj.WfObj_ID), ORM_O01Target);
                                    }
                                }
                                catch (Exception e)
                                {
                                    StringBuilder logmsg = new StringBuilder();
                                    logmsg.Append("Error : Quest Diagnostics Date and Time : " + DateTime.Now.ToString() + Environment.NewLine);
                                    logmsg.Append("Move to Next Process for file name : ACUR" + obj.WF_Obj_System_ID.ToString() + ".hl7." + Environment.NewLine);
                                    logmsg.Append("Error Message : " + e.Message.ToString() + Environment.NewLine);
                                    if (e.InnerException != null)
                                        logmsg.Append(e.InnerException.Message != null ? "InnerException Message : " + e.InnerException.Message.ToString() + Environment.NewLine : "");
                                    else
                                        logmsg.Append("Error : " + e.ToString() + Environment.NewLine);
                                    Console.WriteLine(e.Message);
                                    using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                                    {
                                        tx.WriteLine(logmsg);
                                    }
                                }
                                if (File.Exists(sOrderCheckSend) == false)
                                {
                                    os.ORDER_MESSAGE = GetBytesFromFileReturnString(sOrderFileName);
                                    string ContentToBeWritenInHL7File = os.FormatedHl7File();
                                    File.WriteAllText(sOrderFileName, ContentToBeWritenInHL7File);
                                    // }
                                    //Moved to DB loop on 8.11.2017 by Naveena

                                    // string[] FilesInOutBoxQuest = Directory.GetFiles(filepathQuest, "*.hl7");
                                    os.ORDER_MESSAGE = GetBytesFromFileReturnString(sOrderFileName);
                                    if (sQuestService != null && sQuestService.ToString().ToUpper() == "PRODUCTION")
                                    {
                                        object[] objWebResult = os.sendOrder(true, false, sOrderFileName);
                                        Console.WriteLine(objWebResult[0].ToString());
                                        if (objWebResult[0].ToString() == "SUCCESS")
                                        {
                                            File.Move(sOrderFileName, filepathQuest + "\\Send" + sOrderFileName.Substring(filepathQuest.Length));

                                        }
                                        else
                                        {
                                            StringBuilder logmsg = new StringBuilder();
                                            logmsg.Append("Error :Order Web Service Send Failed Detalis : " + DateTime.Now.ToString() + Environment.NewLine);
                                            logmsg.Append(objWebResult[0].ToString() + " - " + objWebResult[1].ToString() + " - " + objWebResult[2].ToString() + Environment.NewLine);
                                            using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                                            {
                                                tx.WriteLine(logmsg);
                                            }
                                        }
                                    }
                                }
                                if (File.Exists(sOrderCheckSend) == true)
                                {
                                    //Move quest order to next process till 9.Nov.2017
                                    //    var OrderSendlst = OrderGeneratedList.Where(a => a.WF_Obj_System_ID == Convert.ToUInt64((sOrderFileName.Substring(filepathQuest.Length + 5)).Split('.')[0])).ToList();//.Select(b => new { b.WF_Obj_System_ID, b.Obj_Type });
                                    //    Console.WriteLine(OrderSendlst.Count().ToString());
                                    //    if (OrderSendlst.Count() > 0)
                                    //    {
                                    //        foreach (var objtemp in OrderSendlst)
                                    //        {
                                    //            Console.WriteLine("Moving Next Process for Quest objsystemid - " + objtemp.WF_Obj_System_ID);
                                    //            WFProxy.MoveToNextProcess(objtemp.WF_Obj_System_ID, objtemp.Obj_Type, 1, "UNKNOWN", DateTime.Now, string.Empty, null, null);
                                    //        }
                                    //    }
                                    WFProxy.MoveToNextProcess(obj.WF_Obj_System_ID, obj.Obj_Type, 1, "UNKNOWN", DateTime.Now, string.Empty, null, null);
                                }
                                //End Naveena

                            }
                            //for this bug ID 53414
                            //else if (obj.LabName.StartsWith("CMG and Ancillary-Image") && obj.Order_Code_Type.Trim() != "")
                            //{
                            //    WFProxy.MoveToNextProcess(obj.WF_Obj_System_ID, obj.Obj_Type, 1, "UNKNOWN", DateTime.Now, string.Empty, null, null);
                            //}
                            //else
                            //{
                            //    WFProxy.MoveToNextProcess(obj.WF_Obj_System_ID, obj.Obj_Type, 9, "UNKNOWN", DateTime.Now, string.Empty, null, null);
                            //}
                        }
                    }
                }


                Console.WriteLine("Hypersend Agent Running\n");
                ProcessStartInfo objProcessStartInfo = new ProcessStartInfo("schtasks", "/run /tn outbox");
                Process p = Process.Start(objProcessStartInfo);
                p.WaitForExit();
                //LabCorp Move To Next Process-ORDER_GENERATE->RESULT_PROCESS
                //foreach (FillLabAgentDTO obj in OrderGeneratedList.Where(a => a.LabName == "LabCorp"))
                //{
                //    if ((obj.Is_Submit_Imediately == "Y") ||
                //            ((obj.Is_Submit_Imediately == "N") && obj.Current_Arrival_Time.AddMinutes(Convert.ToDouble(TimeDelay)) <= DateTime.Now))
                //    {
                //        string[] MatchedFiles = Directory.GetFiles(filepathLabCorp + "\\Send", "ACUR" + obj.WF_Obj_System_ID.ToString() + ".hl7", SearchOption.AllDirectories);
                //        if (MatchedFiles != null && MatchedFiles.Length != 0)
                //        {
                //            WFProxy.MoveToNextProcess(obj.WF_Obj_System_ID, obj.Obj_Type, 1, "UNKNOWN", DateTime.Now, string.Empty, null, null);
                //        }
                //    }
                //}

                // Get by file type???
                //string[] FilesInOutBoxQuest = Directory.GetFiles(filepathQuest, "*.hl7");
                //if (!Directory.Exists(filepathQuest + "\\Send"))
                //    Directory.CreateDirectory(filepathQuest + "\\Send");
                //for (int i = 0; i < FilesInOutBoxQuest.Count(); i++)
                //{
                //    os.ORDER_MESSAGE = GetBytesFromFileReturnString(FilesInOutBoxQuest[i]);   //First

                //    object[] obj = os.sendOrder(true, false, FilesInOutBoxQuest[i]);

                //    Console.WriteLine(obj[0].ToString());
                //    if (obj[0].ToString() == "SUCCESS")
                //    {

                //        var OrderSendlst = OrderGeneratedList.Where(a => a.WF_Obj_System_ID == Convert.ToUInt64((FilesInOutBoxQuest[i].Substring(filepathQuest.Length + 5)).Split('.')[0])).ToList();//.Select(b => new { b.WF_Obj_System_ID, b.Obj_Type });
                //        //var OrderSendlst = from lst in OrderGeneratedList
                //        //                   where lst.WF_Obj_System_ID == Convert.ToUInt64((FilesInOutBoxQuest[i].Substring(filepathQuest.Length + 5)).Split('.')[0])
                //        //                   select new { lst.WF_Obj_System_ID, lst.Obj_Type };

                //        Console.WriteLine(OrderSendlst.Count().ToString());
                //        if (OrderSendlst.Count() > 0)
                //        {
                //            foreach (var objtemp in OrderSendlst)
                //            {
                //                Console.WriteLine("Moving Next Process for Quest....");
                //                WFProxy.MoveToNextProcess(objtemp.WF_Obj_System_ID, objtemp.Obj_Type, 1, "UNKNOWN", DateTime.Now, string.Empty, null, null);
                //                //  WFProxy.MoveToNextProcess(objtemp.WF_Obj_System_ID, objtemp.Obj_Type, 1, "UNKNOWN", DateTime.Now, null);
                //            }
                //            File.Move(FilesInOutBoxQuest[i], filepathQuest + "\\Send" + FilesInOutBoxQuest[i].Substring(filepathQuest.Length));

                //        }

                //    }
                //    else
                //    {
                //        StringBuilder logmsg = new StringBuilder();
                //        logmsg.Append("Order Web Service Send Failed Detalis : " + DateTime.Now.ToString() + Environment.NewLine);
                //        logmsg.Append(obj[0].ToString()+" - " + obj[1].ToString() + " - " + obj[2].ToString()+Environment.NewLine);
                //        using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                //        {
                //            tx.WriteLine(logmsg);
                //        }
                //    }
                //}
                RetriveResults(os.USERNAME, os.PASSWORD, ResultURL);
            }
            catch (Altova.UserException e)
            {
                Console.Out.Write("Error Create Orders Altova: ");
                Console.Out.WriteLine(e.Message);
                StringBuilder logmsg = new StringBuilder();
                logmsg.Append("Error : Create Orders Altova Date and Time : " + DateTime.Now.ToString() + Environment.NewLine);
                logmsg.Append("Error Message : " + e.Message.ToString() + Environment.NewLine);
                if (e.InnerException != null)
                    logmsg.Append(e.InnerException.Message != null ? "InnerException Message : " + e.InnerException.Message.ToString() + Environment.NewLine : "");
                else
                    logmsg.Append("Error : " + e.ToString() + Environment.NewLine);

                logmsg.Append("Stack Trace : " + e.StackTrace.ToString() + Environment.NewLine);
                using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                {
                    tx.WriteLine(logmsg);
                }
                //Console.ReadLine();
                //System.Environment.Exit(1);
            }
            catch (Exception e)
            {
                //Log as "ordersutilitymanager"
                Console.Out.Write("Exception Create Orders Exception");
                Console.Out.WriteLine(e.Message);
                StringBuilder logmsg = new StringBuilder();
                logmsg.Append("Error : Create Orders Exception Date and Time : " + DateTime.Now.ToString() + Environment.NewLine);
                logmsg.Append("Error Message : " + e.Message.ToString() + Environment.NewLine);
                if (e.InnerException != null)
                    logmsg.Append(e.InnerException.Message != null ? "InnerException Message : " + e.InnerException.Message.ToString() + Environment.NewLine : "");
                else
                    logmsg.Append("Error : " + e.ToString() + Environment.NewLine);

                logmsg.Append("Stack Trace : " + e.StackTrace.ToString() + Environment.NewLine);
                using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                {
                    tx.WriteLine(logmsg);
                }
                //Console.ReadLine();
                // System.Environment.Exit(1);
            }


        }
        public static string GetBytesFromFileReturnString(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                string s = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                return s.Replace("\n", "");
            }
            finally
            {
                fs.Close();
            }

        }
        public bool GenerateABNForQuest(IList<ulong> Order_Submit_ID, bool IsABNPDFNeed)
        {
            LabcorpSettingsManager objLabcorpSettingsManager = new LabcorpSettingsManager();
            LabManager objLabManager = new LabManager();
            bool QuestABNIsGenerated = false;
            //Remove the log file???
            IList<string> ResponseMsg = new List<string>();
            IList<string> GeneratedHL7FileNames = new List<string>();
            IList<LabSettings> ilstLabSetting = objLabcorpSettingsManager.GetLabcorpSettings();
            IList<Lab> ilstLabList = objLabManager.GetLabList();
            IList<ulong> LabIdForQuest = (from lst in ilstLabList where lst.Lab_Name == "Quest Diagnostics" && lst.Lab_Type == "LAB" select lst.Id).ToList<ulong>();
            IList<LabSettings> objLabSettingsForQuest = (from lst in ilstLabSetting where lst.Lab_ID == LabIdForQuest[0] select lst).ToList<LabSettings>();
            TraceTargetConsole ttc = new TraceTargetConsole();
            MappingMapToORM_O01_Quest MappingMapToORM_O01_Quest_Object = new MappingMapToORM_O01_Quest();
            MappingMapToORM_O01_Quest_Object.RegisterTraceTarget(ttc);
            string TempFile = System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["HL7TempFolder"];
            if (!Directory.Exists(TempFile))
            {
                Directory.CreateDirectory(TempFile);
            }
            OrdersManager objOrdersProxy = new OrdersManager();
            OrdersSample os = new OrdersSample(objLabSettingsForQuest[0].User_Name, objLabSettingsForQuest[0].Password, objLabSettingsForQuest[0].URL, objLabSettingsForQuest[0].Lab_Practice_Application, objLabSettingsForQuest[0].Lab_Practice_ID, objLabSettingsForQuest[0].Receiving_Facility);
            IList<ulong> wfObjectIds = objOrdersProxy.GetWfObjectIDsFromObjSystemIDs(Order_Submit_ID, "DIAGNOSTIC ORDER");
            if (wfObjectIds.Count > 0)
            {
                for (int i = 0; i < wfObjectIds.Count; i++)
                {
                    string sOrderFileName = TempFile + "\\ACUR" + wfObjectIds[i].ToString() + DateTime.Now.ToString("yyyyMMddhhmmss") + ".hl7";
                    GeneratedHL7FileNames.Add(sOrderFileName);
                    Altova.IO.Output ORM_O01Target = new Altova.IO.FileOutput(sOrderFileName);
                    MappingMapToORM_O01_Quest_Object.Run(ilstLabSetting[0].Credential_Informantion, Altova.CoreTypes.CastToInt((int)wfObjectIds[i]), ORM_O01Target);
                    os.ORDER_MESSAGE = GetBytesFromFileReturnString(sOrderFileName);
                    string ContentToBeWritenInHL7File = os.FormatedHl7File();
                    File.WriteAllText(sOrderFileName, ContentToBeWritenInHL7File);
                }
            }
            string[] FilesInOutBoxQuest = Directory.GetFiles(TempFile);
            object[] objPDF = new object[FilesInOutBoxQuest.Count()];
            for (int i = 0; i < GeneratedHL7FileNames.Count(); i++)
            {
                //OrdersSample os = new OrdersSample(objLabSettingsForQuest[0].User_Name, objLabSettingsForQuest[0].Password, objLabSettingsForQuest[0].URL, objLabSettingsForQuest[0].Lab_Practice_Application, objLabSettingsForQuest[0].Lab_Practice_ID, objLabSettingsForQuest[0].Receiving_Facility);
                os.ORDER_MESSAGE = GetBytesFromFileReturnString(GeneratedHL7FileNames[i]);
                object[] obj = os.sendOrder(false, true, GeneratedHL7FileNames[i]);
                if (obj.Count() > 1)
                {
                    objPDF[i] = obj[1];
                }
                else
                {
                    StringBuilder logmsg = new StringBuilder();
                    if (obj.Count() == 3)
                        logmsg.Append(obj[0].ToString() + obj[1].ToString() + "-" + obj[2].ToString() + DateTime.Now.ToString());
                    else if (obj[0] != null)
                        logmsg.Append(obj[0].ToString() + DateTime.Now.ToString());
                    using (TextWriter tx = new StreamWriter(System.Configuration.ConfigurationSettings.AppSettings["LogFileLabAgent"], true))
                    {
                        tx.WriteLine(logmsg);
                    }
                    ResponseMsg.Add(obj[0] as string);
                }
                File.Delete(GeneratedHL7FileNames[i]);
            }
            for (int i = 0; i < objPDF.Count(); i++)
            {
                if (objPDF[i] != null && IsABNPDFNeed)
                {
                    string openFilename = string.Empty;
                    File.WriteAllBytes(openFilename = TempFile + "\\ABN_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf", Convert.FromBase64String(objPDF[i].ToString()));
                    //ConvertBase64(TempFile, objPDF[i].ToString());
                    System.Diagnostics.Process.Start(openFilename);
                }
                if (!IsABNPDFNeed && objPDF[i] != null)
                {
                    QuestABNIsGenerated = true;
                    break;
                }

            }
            return QuestABNIsGenerated;
        }
        public void ConvertBase64(string path, string Base64PDFString)
        {
            try
            {
                using (var f = System.IO.File.Create(path))
                {
                    byte[] encodedDataAsBytes = System.Convert.FromBase64String(Base64PDFString);
                    f.Write(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (IOException k)
            {
                //Console.ReadLine();
                //System.Environment.Exit(1);
            }
        }
        #region ResultPartQuest
        public void RetriveResults(string UserName, string Password, string ResultURL)
        {
            ResultsService reTrial = new ResultsService();
            reTrial.Url = ResultURL;
            ResultsRequest resultreqTrial = new ResultsRequest();
            resultreqTrial.startDate = null;

            resultreqTrial.endDate = null;
            NetworkCredential networkCredsTrial = new NetworkCredential(UserName, Password);

            reTrial.Credentials = networkCredsTrial;
            reTrial.PreAuthenticate = true;
            HL7ResultsResponse respTrial = reTrial.getHL7Results(resultreqTrial);
            string dirpath = System.Configuration.ConfigurationSettings.AppSettings["QuestReceivedResultPath"];
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
            //ResultsResponse responsehere = reTrial.getResults(resultreqTrial);
            if (respTrial.HL7Messages != null)
            {
                HL7Message[] Ackhl7Msgs = new HL7Message[respTrial.HL7Messages.Length];
                string requestID = respTrial.requestId;
                for (int i = 0; i < respTrial.HL7Messages.Length; i++)
                {
                    string s = string.Empty;
                    System.Text.Encoding encoding = new System.Text.ASCIIEncoding();
                    s = encoding.GetString(respTrial.HL7Messages[i].message);
                    string dirpathRst = System.Configuration.ConfigurationSettings.AppSettings["QuestReceivedResultPath"];
                    using (File.Create(dirpathRst + "\\" + respTrial.HL7Messages[i].controlId.ToString() + ".dat"))
                    {
                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(dirpathRst + "\\" + respTrial.HL7Messages[i].controlId.ToString() + ".dat"))
                    {
                        file.WriteLine(s);
                        HL7Message hl7msg = new HL7Message();
                        System.Object[] msg_params = new System.Object[7];
                        string[] Segments = s.Split('\r');
                        string HeaderSeg = Segments[0];
                        string[] header_fields = HeaderSeg.Split('|');
                        msg_params[0] = header_fields[4]; // receiving app
                        msg_params[1] = header_fields[5]; // receiving fac
                        msg_params[2] = header_fields[2]; // sending app
                        msg_params[3] = header_fields[3]; // sending fac
                        msg_params[4] = System.DateTime.Now; // time stamp
                        msg_params[5] = (System.DateTime.Now.Ticks - 621355968000000000) / 10000; // some unique ID
                        msg_params[6] = header_fields[9];
                        String ACK_MESSAGE_FORMAT = "MSH|^~\\&|{0}|{1}|{2}|{3}|{4:yyyyMMddHHmm}||ACK|{5:#####}|D|2.3\rMSA|CA|{6}\r";
                        string message_string_ret = String.Format(ACK_MESSAGE_FORMAT, msg_params);
                        byte[] message_UTF8 = System.Text.Encoding.UTF8.GetBytes(message_string_ret);
                        hl7msg.message = message_UTF8;
                        hl7msg.controlId = header_fields[9].ToString();
                        Ackhl7Msgs[i] = hl7msg;
                    }
                    reTrial.acknowledgeHL7Results(requestID, Ackhl7Msgs);
                }
                //using (File.Create(dirpathRst + "\\" + respTrial.HL7Messages[i].controlId.ToString() + ".dat"))
                //{
                //    filec
                //}
                //File.WriteAllText(dirpathRst + "\\" + respTrial.HL7Messages[i].controlId.ToString() + ".dat", s);
                //using (TextWriter tx = new StreamWriter(dirpathRst + "\\" + respTrial.HL7Messages[i].controlId.ToString() + ".dat",true))
                //{
                //    tx.WriteLine(s);
                //}
                //Write s into hl7 file????
                //if (s.Contains("Base64^") == true)
                //{
                //    int iIndex = s.IndexOf("Base64^");
                //    string sPDFContent = s.Substring(iIndex + 7, s.Length - iIndex - 15);
                //    Decode(sPDFContent);
                //}

            }



            //for (int i = 0; i < responsehere.HL7Messages.Length; i++)
            //{
            //    string s = responsehere.HL7Messages[i].ToString();
            //    if (s.Contains("Base64^") == true)
            //    {
            //        int iIndex = s.IndexOf("Base64^");
            //        string sPDFContent = s.Substring(iIndex + 7, s.Length - iIndex - 15);
            //        Decode(sPDFContent);
            //    }
            //}
        }
        //public void Decode(string encodedData)
        //{
        //    string dirpath = System.Configuration.ConfigurationSettings.AppSettings["QuestResultPath"];
        //    DirectoryInfo dir = new DirectoryInfo(dirpath);
        //    if (dir.Exists == false)
        //    {
        //        DirectoryInfo d = Directory.CreateDirectory(dirpath);
        //    }
        //    string path = dirpath + "Result_" + DateTime.Now.ToString("dd-MMM-yyyy_hhmmsstt") + ".pdf";
        //    ConvertBase64(path, encodedData);
        //}

        #endregion
    }


    class TraceTargetConsole : Altova.ITraceTarget
    {
        public void WriteTrace(string info)
        {
            Console.Out.WriteLine(info);
        }
    }
}
