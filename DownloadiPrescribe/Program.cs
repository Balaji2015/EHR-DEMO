using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.DataAccess;
using System.Globalization;
using static Acurus.Capella.DataAccess.RCopiaXMLResponseProcess;
using System.Configuration;
using Acurus.Capella.Core.DTO;
using System.Timers;
using System.IO;
using System.Net;
using Acurus.Capella.Core.DTOJson;
using Newtonsoft.Json;
using System.Collections;
using System.Reflection;
using iTextSharp.text.pdf;
using NHibernate;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;

namespace DownloadiPrescribe
{
    public class Program
    {
        //Jira CAP-3038
        ////Jira CAP-2977
        //static string sSub_Type = ConfigurationManager.AppSettings["Document_Sub_Type"].ToUpper();
        //static string[] sDocument_Sub_Type = sSub_Type.Split(',');
        ////Jira CAP-2977 - End
        
        //Jira CAP-3038
        static Document_Sub_Type_Lookup_List ilstDocument_Sub_type = new Document_Sub_Type_Lookup_List();
        public static string LabAgentLog = string.Empty;
        //static async Task Main(string[] args)
        static void Main(string[] args)
        {
            switch (ConfigurationManager.AppSettings["AgentName"])
            {
                case "DownloadRcopia":
                    Console.WriteLine("Download RCopia Started");
                    DownloadRCopiaInfo();
                    Console.WriteLine("Download RCopia Method Invoked");
                    break;
                case "CreateOrderTask":
                    Console.WriteLine("Order Task Creation Started.");
                    CreateLabOrderTask();
                    Console.WriteLine("Order Task Creation Method Invoked.");
                    break;
                case "ImageResultsAgent":
                    Console.WriteLine("ImageResultsAgent Started.");
                    ImportImageResultsAgent();
                    Console.WriteLine("ImageResultsAgent Method Invoked.");
                    break;
                case "CDCXmlRegenerateJob":
                    Console.WriteLine("CDCXmlRegenerateJob Started.");
                    CDCXmlRegenerateJob();
                    Console.WriteLine("CDCXmlRegenerateJob Method Invoked.");
                    break;
                case "ImportIndexedDocuments":
                    Console.WriteLine("ImportIndexedDocuments Started.");
                    ImportIndexedDocumentsJob();
                    Console.WriteLine("ImportIndexedDocuments Method Invoked.");
                    Console.WriteLine("ImportIndexingExceptionLog Started.");
                    ImportIndexingExceptionLogJob();
                    Console.WriteLine("ImportIndexingExceptionLog Method Invoked.");
                    break;
                case "CCDXmlGenerateAgent":
                    Console.WriteLine("CCDXmlGenerateAgent Started.");
                    CCDXmlGenerateAgent();
                    Console.WriteLine("CCDXmlGenerateAgent Method Invoked.");
                    break;
            }
            //Console.WriteLine("Order Task Creation Started.");
            //Timer method1Timer = new Timer();
            //method1Timer.Elapsed += (sender, e) => HandleException(CreateLabOrderTask, method1Timer);
            //method1Timer.Interval = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["OrderTaskTimeSpan"])).TotalMilliseconds;
            //method1Timer.Start();
            //Console.WriteLine("Order Task Creation Method Invoked.");

            //Console.WriteLine("Download RCopia Started");
            //Timer method2Timer = new Timer();
            //method2Timer.Elapsed += (sender, e) => HandleException(DownloadRCopiaInfo, method2Timer);
            //method2Timer.Interval = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["RCopiaTimeSpan"])).TotalMilliseconds;
            //method2Timer.Start();
            //Console.WriteLine("Download RCopia Method Invoked");

            //Console.WriteLine("Press Enter to exit.");
            //Console.ReadLine();
        }

        private static void HandleException(Action methodCallback, Timer timer)
        {
            try
            {
                methodCallback(); // Execute the method
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Restart the timer to ensure it continues running on schedule
                timer.Stop();
                timer.Start();
            }
        }

        public static void CreateLabOrderTask()
        {
            try
            {
                IList<OrderTaskDTO> lstOrderTaskDTOs = new List<OrderTaskDTO>();
                OrdersSubmitManager OrderSubmitProxy = new OrdersSubmitManager();
                IList<ulong> lstOrderSubmitIds = new List<ulong>();

                Console.WriteLine("Get order submit records for task notification.");
                var totalRecords = OrderSubmitProxy.GetOrderDetailsCountForTaskNotification();
                if(totalRecords == 0)
                {
                    return;
                }

                Console.WriteLine("Get order submit detail for task notification.");
                lstOrderTaskDTOs = OrderSubmitProxy.GetOrderDetailsForTaskNotification();

                DateTime currentDateTimeUTC = DateTime.UtcNow;
                foreach (var order in lstOrderTaskDTOs)
                {
                    if (!lstOrderSubmitIds.Any(x => x == order.Order_Submit_ID))
                    {
                        Console.WriteLine($"Order task creation for order_submit_id : {order.Order_Submit_ID}");
                        var labAndICDMessages = SetLabAndICDMessage(lstOrderTaskDTOs.Where(x => x.Order_Submit_ID == order.Order_Submit_ID).ToList());
                        order.Lab_Procedure_Message = labAndICDMessages.Item1;
                        order.ICD_Description_Message = labAndICDMessages.Item2;

                        if (order.Stat == "Y" &&
                            ((order.Created_Date_And_Time <= currentDateTimeUTC &&
                            order.Modified_Date_And_Time.ToString("yyyy-MM-dd") == "0001-01-01" &&
                            (order?.Current_Process ?? string.Empty) != "DELETED_ORDER")
                            || ((order.Modified_Date_And_Time.ToString("yyyy-MM-dd") != "0001-01-01" &&
                            (order?.Current_Process ?? string.Empty) != "DELETED_ORDER"
                            && order.Modified_Date_And_Time <= currentDateTimeUTC))
                            || ((order?.Current_Process ?? string.Empty) == "DELETED_ORDER" && order.Current_Arrival_Time <= currentDateTimeUTC))
                        )
                        {
                            Console.WriteLine($"Add patient notes for order with stat value as 'Y'.");
                            AddPatientNotes(order);
                            Console.WriteLine($"Task created for order submit id: {order.Order_Submit_ID}");
                        }
                        else if (order.Stat != "Y" &&
                            ((order.Created_Date_And_Time.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["withoutStatTimeSpan"])) <= currentDateTimeUTC &&
                            order.Modified_Date_And_Time.ToString("yyyy-MM-dd") == "0001-01-01" &&
                            (order?.Current_Process ?? string.Empty) != "DELETED_ORDER")
                            || ((order.Modified_Date_And_Time.ToString("yyyy-MM-dd") != "0001-01-01" &&
                            (order?.Current_Process ?? string.Empty) != "DELETED_ORDER"
                            && order.Modified_Date_And_Time.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["withoutStatTimeSpan"])) <= currentDateTimeUTC))
                            || ((order?.Current_Process ?? string.Empty) == "DELETED_ORDER" && order.Current_Arrival_Time.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["withoutStatTimeSpan"])) <= currentDateTimeUTC)))
                        {
                            Console.WriteLine($"Add patient notes for order with stat value as 'N'.");
                            AddPatientNotes(order);
                            Console.WriteLine($"Task created for order submit id: {order.Order_Submit_ID}");
                        }
                        else
                        {
                            Console.WriteLine($"Task creation for order submit id: {order.Order_Submit_ID} is skipped for this iteration.");
                        }
                        
                        lstOrderSubmitIds.Add(order.Order_Submit_ID);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public static void AddPatientNotes(OrderTaskDTO orderSubmitData)
        {
            IList<PatientNotes> patientlst = new List<PatientNotes>();
            PatientNotes objpatientnotes = new PatientNotes();
            objpatientnotes.Human_ID = orderSubmitData.Human_ID;
            objpatientnotes.Assigned_To = "UNKNOWN";
            objpatientnotes.Relationship = "";
            objpatientnotes.Facility_Name = orderSubmitData.Facility_Name;
            objpatientnotes.Caller_Name = "";
            objpatientnotes.Message_Orign = "";
            objpatientnotes.Priority = orderSubmitData.Priority;
            objpatientnotes.Message_Description = orderSubmitData.Ancillary_Order;
            //CAP-674 - If the note is coming then concat.
            StringBuilder strNotes = new StringBuilder();
            //objpatientnotes.Notes = "@" + ClientSession.UserName + "(" + Convert.ToDateTime(DateTime).ToString("dd-MMM-yyyy hh:mm:ss tt") + "): " + DLC;
            objpatientnotes.Orders_Submit_ID = Convert.ToInt32(orderSubmitData.Order_Submit_ID);
            objpatientnotes.Created_By = orderSubmitData.Created_By;
            objpatientnotes.Type = orderSubmitData.Task;
            objpatientnotes.Message_Date_And_Time = orderSubmitData.Created_Date_And_Time;
            objpatientnotes.Is_PatientChart = "N";
            objpatientnotes.Created_Date_And_Time = DateTime.UtcNow;

            var notesPriority = orderSubmitData.Stat == "Y" ? "Stat" : "";

            Console.WriteLine("Creating notes message based on order status");

            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            
            //New Order
            if ((orderSubmitData.Created_Date_And_Time != null) && (orderSubmitData.Created_Date_And_Time.ToString() != "0001-01-01") && (orderSubmitData.Modified_Date_And_Time == null || (orderSubmitData.Modified_Date_And_Time.ToString("yyyy-MM-dd") == "0001-01-01")) && (orderSubmitData?.Current_Process ?? string.Empty) != "DELETED_ORDER")
            {
                //@MVILLA(18-Sep-2023 03:24:55 AM): tests
                var pstCreatedDateTime = TimeZoneInfo.ConvertTimeFromUtc(orderSubmitData.Created_Date_And_Time, timeInfo);
                strNotes.Append($"@{orderSubmitData.Created_By}({pstCreatedDateTime:dd-MMM-yyyy hh:mm:ss tt}): ");
                strNotes.AppendLine("The new ancillary order has been submitted.");
                strNotes.AppendLine($"Ancillary Order Date : {orderSubmitData.Created_Date_And_Time:dd-MMM-yyyy}");
                strNotes.AppendLine($"Ancillary Order # : {orderSubmitData.Order_Submit_ID}");
                strNotes.AppendLine($"Priority : {notesPriority}");
                strNotes.AppendLine($"Ordering Provider : {orderSubmitData.Physician_Name}");
                strNotes.AppendLine($"Ancillary Order(s) : {orderSubmitData.Lab_Procedure_Message}");
                strNotes.AppendLine($"Facility to perform Test : {orderSubmitData.Lab_Name}");
                strNotes.AppendLine($"Associated Order ICD Codes : {orderSubmitData.ICD_Description_Message}");

            }
            //Deleted Order
            else if ((orderSubmitData?.Current_Process ?? string.Empty) == "DELETED_ORDER")
            {
                var pstUpdatedDateTime = TimeZoneInfo.ConvertTimeFromUtc(orderSubmitData.Current_Arrival_Time, timeInfo);
                strNotes.Append($"@{orderSubmitData.Created_By}({pstUpdatedDateTime:dd-MMM-yyyy hh:mm:ss tt}): ");
                strNotes.AppendLine("The ancillary order has been deleted.");
                strNotes.AppendLine($"Ancillary Order Date : {orderSubmitData.Created_Date_And_Time:dd-MMM-yyyy}");
                strNotes.AppendLine($"Ancillary Order # : {orderSubmitData.Order_Submit_ID}");
                strNotes.AppendLine($"Priority : {notesPriority}");
                strNotes.AppendLine($"Ordering Provider : {orderSubmitData.Physician_Name}");
                strNotes.AppendLine($"Ancillary Order(s) : {orderSubmitData.Lab_Procedure_Message}");
                strNotes.AppendLine($"Facility to perform Test : {orderSubmitData.Lab_Name}");
                strNotes.AppendLine($"Associated Order ICD Codes : {orderSubmitData.ICD_Description_Message}");
            }
            //Updated Order
            else
            {
                var pstUpdatedDateTime = TimeZoneInfo.ConvertTimeFromUtc(orderSubmitData.Modified_Date_And_Time, timeInfo);
                strNotes.Append($"@{orderSubmitData.Created_By}({pstUpdatedDateTime:dd-MMM-yyyy hh:mm:ss tt}): ");
                strNotes.AppendLine("The ancillary order has been updated.");
                strNotes.AppendLine($"Ancillary Order Date : {orderSubmitData.Created_Date_And_Time:dd-MMM-yyyy}");
                strNotes.AppendLine($"Ancillary Order # : {orderSubmitData.Order_Submit_ID}");
                strNotes.AppendLine($"Priority : {notesPriority}");
                strNotes.AppendLine($"Ordering Provider : {orderSubmitData.Physician_Name}");
                strNotes.AppendLine($"Ancillary Order(s) : {orderSubmitData.Lab_Procedure_Message}");
                strNotes.AppendLine($"Facility to perform Test : {orderSubmitData.Lab_Name}");
                strNotes.AppendLine($"Associated Order ICD Codes : {orderSubmitData.ICD_Description_Message}");
            }

            objpatientnotes.Notes = strNotes.ToString();

            Console.WriteLine("Creating wf objects");
            PatientNotesManager patientnotesmngr = new PatientNotesManager();
            WFObject objWf = new WFObject();
            objWf.Current_Process = "START";
            objWf.Current_Owner = "UNKNOWN";
            objWf.Fac_Name = orderSubmitData.Facility_Name;
            objWf.Obj_Type = "TASK";
            objWf.Current_Arrival_Time = DateTime.UtcNow;
            Console.WriteLine("Save Patient notes, wfobject and update column in order submit table");
            patientnotesmngr.SavePatientTaskByOrder(objpatientnotes, objWf, null);
            Console.WriteLine("Data saved successfully");

        }
        public static void DownloadRCopiaInfo()
        {
            try
            {
                Console.WriteLine("Execution Started...");
                DateTime dtRCopia_Last_Updated_Date_Time = DateTime.MinValue;
                string sLegalOrgListConfig = ConfigurationManager.AppSettings["sLegalOrgList"];
                List<string> sLegalOrgList = new List<string>(sLegalOrgListConfig.Split(','));
                Rcopia_Update_InfoManager rcopiaUpdateMngr = new Rcopia_Update_InfoManager();
                CultureInfo culture = new CultureInfo("en-US");
                foreach (var sLegalOrg in sLegalOrgList)
                {
                    Console.WriteLine($"Running for legal org {sLegalOrg}");
                    DateTime.TryParse(rcopiaUpdateMngr.GetRcopiaUpdateInfoCommandNameAndLegalOrg("get_rcopia_event", sLegalOrg), out dtRCopia_Last_Updated_Date_Time);
                    RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                    RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();
                    Console.WriteLine("Calling iPrescribe API...");
                    string sInputXML = rcopiaXML.CreateGetRcopiaEventXML(dtRCopia_Last_Updated_Date_Time, sLegalOrg);
                    string sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1);
                    EventXMLResponseModel responseModel = rcopiaResponseXML.ReadEventXMLResponse(sOutputXML, sLegalOrg);
                    foreach (var patientId in responseModel.ilstPatientIds)
                    {
                        Console.WriteLine($"Calling DownloadRCopiaInfo method for Human_Id {patientId}");

                        HumanManager humanManager = new HumanManager();
                        var human = humanManager.GetHumanFromHumanID(Convert.ToUInt64(patientId));
                        if (human != null && human.Id != 0)
                        {
                            try
                            {
                                Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                                objUpdateInfoMngr.DownloadRCopiaInfo(string.Empty, "Acurus", string.Empty, DateTime.Now, string.Empty, 0, Convert.ToUInt64(patientId), sLegalOrg);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Something went wrong :- " + ex.Message);
                            }
                        }
                    }

                    Console.WriteLine($"Update LastUpdatedDate in Rcopia_Update_info table for legal org {sLegalOrg}");
                    rcopiaUpdateMngr.InsertinToRcopia_Update_info("get_rcopia_event", Convert.ToDateTime(responseModel.dtLastUpdateDate, culture), string.Empty, string.Empty, sLegalOrg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong :- " + ex.Message);
            }
        }

        public static Tuple<string, string> SetLabAndICDMessage(List<OrderTaskDTO> lstOrderTaskDTO)
        {
            string labMessage = string.Empty, ICDMessage = string.Empty;

            if (lstOrderTaskDTO.Count > 0)
            {
                labMessage += string.Join("; ", lstOrderTaskDTO.Select(x => x.Lab_Procedure + " - " + x.Lab_Procedure_Description).Distinct());
                ICDMessage += string.Join("; ", lstOrderTaskDTO.Select(x => x.ICD + " - " + x.ICD_Description).Distinct());
            }

            return new Tuple<string, string>(labMessage, ICDMessage);
        }

        public static void ImportImageResultsAgent()
        {
            var str = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "ConfigJson\\Document_Sub_Type_Lookup.json");
            ilstDocument_Sub_type = JsonConvert.DeserializeObject<Document_Sub_Type_Lookup_List>(str);

            string sFacility = ConfigurationManager.AppSettings["FacilityNameForResults"];
            string sIncoming_StudiesFilePath = string.Empty;
            string sImported_StudiesFilePath = string.Empty;
            string sErrored_StudiesFilePath = string.Empty;
            string sScanned_Location = string.Empty;
            bool bIsErroredFile = false;
            string sFile = string.Empty;
            ulong ulOrderSubmitId = 0;
            HumanManager MngrHuman = new HumanManager();
            Human objHuman = new Human();
            IList<FileManagementIndex> fileManagementIndexList = new List<FileManagementIndex>();
            IList<Scan> ilstScan = new List<Scan>();
            IList<scan_index> ilstscan_index = new List<scan_index>();
            IList<OrdersSubmit> insertordersubmitList = new List<OrdersSubmit>();
            IList<Orders> insertOrderList = new List<Orders>();
            IList<FileManagementIndex> UpdatefileManagementIndexList = null;
            IList<Scan> UpdateScanList = null;
            IList<scan_index> UpdateScan_indexList = null;
            FileManagementIndexManager fileManagementIndexmanager = new FileManagementIndexManager();
            ScanManager scanManager = new ScanManager();
            Scan_IndexManager scanindexmanager = new Scan_IndexManager();
            OrdersManager ordersManager = new OrdersManager();

            IList<scan_index> ilstScanForFileNumber = new List<scan_index>();

            sIncoming_StudiesFilePath = ConfigurationManager.AppSettings["Incoming_StudiesFilePath"];
            sImported_StudiesFilePath = ConfigurationManager.AppSettings["Imported_StudiesFilePath"];
            sErrored_StudiesFilePath = ConfigurationManager.AppSettings["Errored_StudiesFilePath"];
            sScanned_Location = ConfigurationManager.AppSettings["Scanned_Location"];
            //Jira CAP-2977
            //DirectoryInfo[] Directorys = new DirectoryInfo(sIncoming_StudiesFilePath).GetDirectories();
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            //Jira CAP-2977
            //foreach (DirectoryInfo dir in Directorys)
            //{
            //sIncoming_StudiesFilePath = sIncoming_StudiesFilePath + @"\" + dir.Name;
            //sImported_StudiesFilePath = sImported_StudiesFilePath + @"\" + dir.Name;
            //sErrored_StudiesFilePath = sErrored_StudiesFilePath + @"\" + dir.Name;
            //Jira CAP-2977 - End
            if (Directory.Exists(sIncoming_StudiesFilePath) && Directory.Exists(sImported_StudiesFilePath) && Directory.Exists(sErrored_StudiesFilePath))
            {
                FileInfo[] sFiles = new DirectoryInfo(sIncoming_StudiesFilePath).GetFiles("*.*");
                foreach (FileInfo finfFile in sFiles)
                {
                    insertordersubmitList = new List<OrdersSubmit>();
                    insertOrderList = new List<Orders>();
                    ilstScan = new List<Scan>();
                    ilstscan_index = new List<scan_index>();
                    fileManagementIndexList = new List<FileManagementIndex>();

                    sFile = finfFile.Name;
                    bIsErroredFile = FileValidation(sFile);
                    if (!bIsErroredFile)
                    {
                        //objHuman = MngrHuman.GetHumanFromHumanIDAndLastNameFirstName(Convert.ToUInt64(sFile.ToUpper().Split('_')[0].Replace("ID", "")), sFile.ToUpper().Split('_')[1], sFile.ToUpper().Split('_')[2]);
                        string sDOB = DateTime.ParseExact(sFile.ToUpper().Split('_')[1], "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        objHuman = MngrHuman.GetHumanFromHumanIDAndDOB(Convert.ToUInt64(sFile.ToUpper().Split('_')[0].Replace("ID", "")), sDOB);
                        if (objHuman?.Id != null && objHuman.Id > 0)
                        {
                            Console.WriteLine("FTP started");
                            #region FTP Transfer
                            string ftpServerIP = ConfigurationManager.AppSettings["ftpServerIP"];
                            bool bCreateDirectory = CreateDirectory(objHuman.Id.ToString(), ftpServerIP, string.Empty, string.Empty, out string sCheckFileNotFoundException);
                            if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                                //return;
                            }
                            if (bCreateDirectory)
                            {
                                string serverPath = string.Empty;
                                //Jira CAP-3035
                                int iNumberOfFile = 1;
                                ilstScanForFileNumber = scanindexmanager.GetLastTransactionByHuman(Convert.ToUInt64(sFile.ToUpper().Split('_')[0].Replace("ID", "")));
                                string sIndexed_File_Path = (ilstScanForFileNumber != null && ilstScanForFileNumber.Count > 0) ? ilstScanForFileNumber.FirstOrDefault().Indexed_File_Path : string.Empty;
                                if (sIndexed_File_Path != string.Empty)
                                {
                                    iNumberOfFile = Convert.ToInt32(sIndexed_File_Path.ToString().Substring(sIndexed_File_Path.ToString().LastIndexOf("_") + 1, (sIndexed_File_Path.ToString().LastIndexOf(".") - 1) - sIndexed_File_Path.ToString().LastIndexOf("_"))) + 1;
                                }
                                sIndexed_File_Path = sFile.Replace(".pdf", "") + "_" + ((iNumberOfFile.ToString().Length == 1) ? "0" + iNumberOfFile.ToString() : iNumberOfFile.ToString()) + ".pdf";
                               
                                //serverPath = UploadToImageServer(objHuman.Id.ToString(), ftpServerIP, string.Empty, string.Empty, sIncoming_StudiesFilePath + "//" + sFile, string.Empty, out string sCheckFileNotFoundExceptions);
                                serverPath = UploadToImageServer(objHuman.Id.ToString(), ftpServerIP, string.Empty, string.Empty, sIncoming_StudiesFilePath + "//" + sFile, sIndexed_File_Path, out string sCheckFileNotFoundExceptions);
                                //Jira CAP-3035 - End
                                if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                                {
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                                    //return;
                                }

                                if (serverPath != string.Empty)
                                {

                                    OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                                    objOrdersSubmit.Human_ID = objHuman.Id;
                                    objOrdersSubmit.Created_By = "ImageResultsAgent";
                                    objOrdersSubmit.Created_Date_And_Time = DateTime.UtcNow;
                                    objOrdersSubmit.Facility_Name = sFacility;
                                    //objOrdersSubmit.Specimen_Collection_Date_And_Time = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[3].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                                    objOrdersSubmit.Specimen_Collection_Date_And_Time = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[2].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                                    objOrdersSubmit.Order_Type = "DIAGNOSTIC ORDER";
                                    insertordersubmitList.Add(objOrdersSubmit);


                                    Orders objOrder = new Orders
                                    {
                                        Lab_Procedure = "Paper Order",
                                        Human_ID = objHuman.Id,
                                        Created_By = "ImageResultsAgent",
                                        Created_Date_And_Time = DateTime.UtcNow
                                    };
                                    insertOrderList.Add(objOrder);

                                    ulOrderSubmitId = ordersManager.InsertDummyOrder(insertordersubmitList, insertOrderList, "DIAGNOSTIC ORDER", sFacility, string.Empty);

                                    Scan scan = new Scan();
                                    scan.Scanned_File_Path = sScanned_Location + "\\" + sFile;
                                    //scan.Scanned_Date = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[3].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                    scan.Scanned_Date = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[2].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                    scan.Facility_Name = sFacility;
                                    scan.No_of_Pages = 1;
                                    scan.Scanned_File_Name = sFile;
                                    scan.Scan_Type = "Online Chart - LOCAL";
                                    scan.Created_By = "ImageResultsAgent";
                                    scan.Created_Date_And_Time = DateTime.UtcNow;
                                    ilstScan.Add(scan);
                                    scanManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstScan, ref UpdateScanList, null, string.Empty, false, false, objHuman.Id, string.Empty);

                                    scan_index scan_Index = new scan_index();
                                    scan_Index.Human_ID = objHuman.Id;
                                    scan_Index.Scan_ID = Convert.ToUInt64(ilstScan.FirstOrDefault().Id);
                                    //scan_Index.Document_Date = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[3].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                    scan_Index.Document_Date = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[2].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                    scan_Index.Document_Type = "Results";
                                    //Jira CAP-2977
                                    //scan_Index.Document_Sub_Type = dir.Name.ToUpper();
                                    //Jira CAP-3038
                                    //scan_Index.Document_Sub_Type = sFile.Split('_')[4].ToUpper().Replace(".PDF", "").ToUpper();
                                    //scan_Index.Document_Sub_Type = ilstDocument_Sub_type.Document_Sub_Type_Lookup.Where(x => x.File_Report_Type.ToUpper() == sFile.Split('_')[4].ToUpper().Replace(".PDF", "")).FirstOrDefault().Document_Sub_Type;
                                    scan_Index.Document_Sub_Type = ilstDocument_Sub_type.Document_Sub_Type_Lookup.Where(x => x.File_Report_Type.ToUpper() == sFile.Split('_')[3].ToUpper().Replace(".PDF", "")).FirstOrDefault().Document_Sub_Type;
                                    scan_Index.Order_ID = ulOrderSubmitId;
                                    //scan_Index.Indexed_File_Path = sImported_StudiesFilePath + "\\" + sFile;
                                    scan_Index.Indexed_File_Path = serverPath.Replace("ftp:", "").Replace(@"//", @"\\").Replace(@"/", @"\"); ;
                                    scan_Index.Page_Selected = "1";
                                    scan_Index.Created_By = "ImageResultsAgent";
                                    scan_Index.Created_Date_And_Time = DateTime.UtcNow;
                                    scan_Index.Is_Manually_Reviewed_And_Signed = "Y";
                                    ilstscan_index.Add(scan_Index);

                                    scanindexmanager.SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstscan_index, ref UpdateScan_indexList, null, string.Empty, false, false, objHuman.Id, string.Empty);

                                    FileManagementIndex filemanagementIndex = new FileManagementIndex();
                                    filemanagementIndex.Created_By = "ImageResultsAgent";
                                    filemanagementIndex.Created_Date_And_Time = DateTime.UtcNow;
                                    //filemanagementIndex.Document_Date = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[3].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                    filemanagementIndex.Document_Date = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[2].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                    filemanagementIndex.Document_Type = "Results";
                                    //Jira CAP-2977
                                    //filemanagementIndex.Document_Sub_Type = dir.Name.ToUpper();
                                    //Jira CAP-3038
                                    //filemanagementIndex.Document_Sub_Type = sFile.Split('_')[4].ToUpper().Replace(".PDF", "").ToUpper();
                                    //filemanagementIndex.Document_Sub_Type = ilstDocument_Sub_type.Document_Sub_Type_Lookup.Where(x => x.File_Report_Type.ToUpper() == sFile.Split('_')[4].ToUpper().Replace(".PDF", "")).FirstOrDefault().Document_Sub_Type;
                                    filemanagementIndex.Document_Sub_Type = ilstDocument_Sub_type.Document_Sub_Type_Lookup.Where(x => x.File_Report_Type.ToUpper() == sFile.Split('_')[3].ToUpper().Replace(".PDF", "")).FirstOrDefault().Document_Sub_Type;
                                    filemanagementIndex.Source = "SCAN";
                                    filemanagementIndex.Order_ID = ulOrderSubmitId;
                                    filemanagementIndex.Human_ID = objHuman.Id;
                                    filemanagementIndex.Scan_Index_Conversion_ID = ilstscan_index.FirstOrDefault().Id;
                                    filemanagementIndex.File_Path = serverPath;
                                    filemanagementIndex.Is_Delete = "N";
                                    filemanagementIndex.Facility_Name = sFacility;

                                    fileManagementIndexList.Add(filemanagementIndex);
                                    ulong[] uScanID = { 0 };

                                    fileManagementIndexmanager.SaveUpdateDelete_DBAndXML_WithTransaction(ref fileManagementIndexList, ref UpdatefileManagementIndexList, null, string.Empty, true, true, objHuman.Id, string.Empty);
                                    Console.WriteLine(sFile + " - Import to Imported_Studies Folder");
                                }
                            }
                            Console.WriteLine("FTP Ended");
                            #endregion

                            MoveAndReplace(sIncoming_StudiesFilePath + "//" + sFile, sImported_StudiesFilePath + "//" + sFile);
                        }
                        else
                        {
                            MoveAndReplace(sIncoming_StudiesFilePath + "//" + sFile, sErrored_StudiesFilePath + "//" + sFile);
                            Console.WriteLine(sFile + " - Import to Errored_Studies Folder");
                        }
                    }
                    else
                    {
                        MoveAndReplace(sIncoming_StudiesFilePath + "//" + sFile, sErrored_StudiesFilePath + "//" + sFile);
                        Console.WriteLine(sFile + " - Import to Errored_Studies Folder");
                    }
                }
            }
            //Jira CAP-2977
            //}
        }
        public static void CDCXmlRegenerateJob()
        {
            IList<Blob_Progress_Note> ilstBlopProgressNote = new List<Blob_Progress_Note>();
            BlobProgressNoteManager blobProgressNoteManager = new BlobProgressNoteManager();
            string sDuration = ConfigurationSettings.AppSettings["Duration"]?.ToString() ?? "";

            ilstBlopProgressNote = blobProgressNoteManager.GetBlobProgressNotesByStatus(new List<string>() { "'Error'", "'Completed'" }, sDuration);
            Console.WriteLine("Total encounters : " + ilstBlopProgressNote.Count);
            foreach (Blob_Progress_Note blob_Progress_Note in ilstBlopProgressNote)
            {
                Console.WriteLine("HumanID : " + blob_Progress_Note.Human_ID + " EncounterID : " + blob_Progress_Note.Id + " - Start");
                IsAkidoCDC(blob_Progress_Note.Human_ID.ToString(), blob_Progress_Note.Id.ToString(), "Acurus", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                Console.WriteLine("HumanID : " + blob_Progress_Note.Human_ID + " EncounterID : " + blob_Progress_Note.Id + " - End");
            }

        }
        public static void IsAkidoCDC(string sHumanID, string sEncounterID, string sTransactionBy, string sTransactionDateTime)
        {
            string bIsAkidoEncounter = "false";
            //Jira CAP-1379
            int iRetryCount = 0;

        retry:
            try
            {
                iRetryCount = iRetryCount + 1;

                string akidoNoteCDCURL = ConfigurationSettings.AppSettings["AkidoNoteCDCURL"].ToString();
                akidoNoteCDCURL = akidoNoteCDCURL.Replace("[CapellaHumanID]", sHumanID).Replace("[CapellaEncounterID]", sEncounterID).Replace("[CapellaTransactionBy]", sTransactionBy).Replace("[CapellaTransactionDateTime]", sTransactionDateTime);
                var myUri = new Uri(akidoNoteCDCURL);
                string AccessToken = ConfigurationSettings.AppSettings["AkidoNoteCDCURLToken"].ToString();
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();
                responseStream.Close();
                myWebResponse.Close();
            }
            catch (Exception ex)
            {
            }

        }
        public static bool MoveAndReplace(string sSourceFile, string sDestinationFile)
        {
            if (File.Exists(sDestinationFile))
            {
                File.Delete(sDestinationFile);
                File.Move(sSourceFile, sDestinationFile);
            }
            else
            {
                File.Move(sSourceFile, sDestinationFile);
            }

            return true;
        }
        public static bool FileValidation(string sFile)
        {
            DateTime dateValue;
            if (!sFile.ToUpper().StartsWith("ID"))
            {
                return true;
            }
            else if (!sFile.ToUpper().EndsWith(".PDF"))
            {
                return true;
            }
            //Jira CAP-2977
            //else if (sFile.Split('_').Length != 4)
            //else if (sFile.Split('_').Length != 5)
            //{
            //    return true;
            //}
            else if (sFile.Split('_').Length != 4)
            {
                return true;
            }
            //HumanID validation
            //Jira CAP-2977
            //else if (sFile.Split('_').Length == 4 && sFile.Split('_')[0].ToUpper().Replace("ID", "").Any(a => char.IsLetter(a)))
            //else if (sFile.Split('_').Length == 5 && (sFile.Split('_')[0].ToUpper().Replace("ID", "").Any(a => char.IsLetter(a)) || (sFile.Split('_')[0].ToUpper().Replace("ID", "") == "")))
            //{
            //    return true;
            //}
            else if (sFile.Split('_').Length == 4 && (sFile.Split('_')[0].ToUpper().Replace("ID", "").Any(a => char.IsLetter(a)) || (sFile.Split('_')[0].ToUpper().Replace("ID", "") == "")))
            {
                return true;
            }

            //Date of study digit validation
            //else if (sFile.Split('_')[3].ToUpper().Replace(".PDF", "").Length != 8)
            //{
            //    return true;
            //}
            else if (sFile.Split('_')[2].ToUpper().Replace(".PDF", "").Length != 8)
            {
                return true;
            }

            //Date of study Alfabetic validation
            //else if (sFile.Split('_')[3].ToUpper().Replace(".PDF", "").Any(a => char.IsLetter(a)) || sFile.Split('_')[3].ToUpper().Replace(".PDF", "") == "")
            //{
            //    return true;
            //}
            else if (sFile.Split('_')[2].ToUpper().Replace(".PDF", "").Any(a => char.IsLetter(a)) || sFile.Split('_')[2].ToUpper().Replace(".PDF", "") == "")
            {
                return true;
            }

            //Patient DOB digit validation
            else if (sFile.Split('_')[1].ToUpper().Replace(".PDF", "").Length != 8)
            {
                return true;
            }

            //Patient DOB Alfabetic Validation
            else if (sFile.Split('_')[1].ToUpper().Replace(".PDF", "").Any(a => char.IsLetter(a)) || sFile.Split('_')[1].ToUpper().Replace(".PDF", "") == "")
            {
                return true;
            }

            //Document_Sub_Type validation
            //Jira CAP-3038
            ////Jira CAP-3035
            //////Jira CAP-2977
            //else if (!sDocument_Sub_Type.Contains(sFile.Split('_')[4].ToUpper().Replace(".PDF", "")))
            //{
            //    return true;
            //}
            //else if (ilstDocument_Sub_type != null && ilstDocument_Sub_type.Document_Sub_Type_Lookup.Where(x => x.File_Report_Type.ToUpper() == sFile.Split('_')[4].ToUpper().Replace(".PDF", "")).ToList().Count == 0)
            //{
            //    return true;
            //}
            else if (ilstDocument_Sub_type != null && ilstDocument_Sub_type.Document_Sub_Type_Lookup.Where(x => x.File_Report_Type.ToUpper() == sFile.Split('_')[3].ToUpper().Replace(".PDF", "")).ToList().Count == 0)
            {
                return true;
            }
            else if (sFile.Contains("__"))
            {
                return true;
            }

            //Date of study Datetime formate validation
            //Jira CAP-3035
            ////Jira CAP-2977
            //else if (sFile.Split('_').Length == 4
            //  && sFile.Split('_')[3].ToUpper().Replace(".PDF", "").Length == 8)
            //if (sFile.Split('_').Length == 5
            //    && sFile.Split('_')[3].ToUpper().Replace(".PDF", "").Length == 8)
            //{
            //    try
            //    {
            //        if (!DateTime.TryParse(DateTime.ParseExact(sFile.Split('_')[3].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"), out dateValue))
            //        {
            //            return true;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        return true;

            //    }
            //}
            if (sFile.Split('_').Length == 4
                && sFile.Split('_')[2].ToUpper().Replace(".PDF", "").Length == 8)
            {
                try
                {
                    if (!DateTime.TryParse(DateTime.ParseExact(sFile.Split('_')[2].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"), out dateValue))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return true;

                }
            }

            //Date of study Datetime formate validation
            if (sFile.Split('_').Length == 4
                && sFile.Split('_')[1].ToUpper().Replace(".PDF", "").Length == 8)
            {
                try
                {
                    if (!DateTime.TryParse(DateTime.ParseExact(sFile.Split('_')[1].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"), out dateValue))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return true;

                }
            }

            //Jira CAP-3035
            ////Jira CAP-2977
            //else if (!sDocument_Sub_Type.Contains(sFile.Split('_')[4].ToUpper()))
            //{
            //    return true;
            //}

            return false;
        }
        public static string UploadToImageServer(string HumanID, string serverIP, string UserName, string Password, string SelectedFilePath, string File_Name_Convention, out string sCheckFileNotFoundException)
        {
            string uri = string.Empty;
            FtpWebRequest reqFTP;
            FtpWebResponse responseFTP;

            sCheckFileNotFoundException = "";
            #region "Ftp Operations"
            //bool result = true;
            string serverPath = string.Empty;


            FileInfo fileInf = new FileInfo(SelectedFilePath);
            //    if (File_Name_Convention != string.Empty)
            //    {
            //        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri + File_Name_Convention));
            //    }
            //    else
            //    {
            //        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri + fileInf.Name));
            //    }
            //    reqFTP.Credentials = new NetworkCredential(UserName, Password);
            //    reqFTP.KeepAlive = false;
            //    reqFTP.UsePassive = false;
            //    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            //    reqFTP.UseBinary = true;
            //    reqFTP.ContentLength = fileInf.Length;
            //    int buffLength = 1;
            //    byte[] buff = new byte[fileInf.Length];
            //    int contentLen;
            //    FileStream fs = fileInf.OpenRead();
            //    try
            //    {
            //        Stream strm = reqFTP.GetRequestStream();
            //        contentLen = fs.Read(buff, 0, buffLength);
            //        while (contentLen != 0)
            //        {
            //            strm.Write(buff, 0, contentLen);
            //            contentLen = fs.Read(buff, 0, buffLength);
            //        }
            //        strm.Close();
            //        fs.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        result = false;
            //    }

            //    if (result)
            //    {
            //        if (File_Name_Convention != string.Empty)
            //        {
            //            serverPath = uri + File_Name_Convention;
            //        }
            //        else
            //        {
            //            serverPath = uri + fileInf.Name;
            //        }
            //    }  

            //return serverPath;

            #endregion

            /* Credential To Access NAS Server */
            string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
            string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
            string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
            string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
            string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
            bool result = false;
            if (!HumanID.ToString().ToUpper().Contains("EFAX"))
                uri = serverIP + HumanID + "/";
            else
                uri = serverIP + HumanID;

            if (File_Name_Convention != string.Empty)
            {
                uri = ((uri + File_Name_Convention));
            }
            else
            {
                uri = ((uri + fileInf.Name));
            }
            //Jira #CAP-39
            int iTrycount = 1;
        TryAgain:
            try
            {
                using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                {
                    if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                    {
                        {
                            System.IO.File.Copy(SelectedFilePath, uri.Replace(ftpIP, UNCPath), true);
                            Console.WriteLine("File moved to the serever");
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erroed - "+ex.ToString());
                string sErrorMessage = "";
                if (CheckFileNotFoundException(ex, out sErrorMessage))
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + sErrorMessage + "');", true);
                    sCheckFileNotFoundException = "CheckFileNotFoundException ~" + sErrorMessage;
                    return string.Empty;
                }
                else
                {
                    //Jira #CAP-39
                    if (iTrycount <= 3)
                    {
                        iTrycount++;
                        System.Threading.Thread.Sleep(1500);
                        goto TryAgain;
                    }
                    else
                    {
                        //UtilityManager.RetryExecptionLog(ex, iTrycount);
                        //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "FtpImageProcess,cs Line No - 113 - " + ex.Message + " - Username is " + userName + " -  Password " + password + " - UNCAuthPath " + UNCAuthPath + " - UNCPAth" + UNCPath + " - Selected Path - " + SelectedFilePath + " - URI - " + uri.Replace(ftpIP, UNCPath), DateTime.Now, "0", "frmimageviewer");

                        result = false;
                    }
                }
            }


            if (result)
            {
                serverPath = uri;
            }

            return serverPath;


        }

        public static bool CreateDirectory(string HumanID, string serverIP, string UserName, string Password, out string sCheckFileNotFoundException)
        {
            string uri = string.Empty;
            FtpWebRequest reqFTP;
            FtpWebResponse responseFTP;

            sCheckFileNotFoundException = "";
            uri = serverIP + HumanID + "/";

            string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
            string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
            string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
            string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
            string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
            //Jira #CAP-39
            int iTrycount = 1;
        TryAgain:
            try
            {
                using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                {
                    if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                    {
                        {
                            Directory.CreateDirectory(uri.Replace(ftpIP, UNCPath));
                            Console.WriteLine("Directory Created sucessfully for "+ HumanID);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errored - "+ex.ToString());
                string sErrorMessage = "";
                if (CheckFileNotFoundException(ex, out sErrorMessage))
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + sErrorMessage + "');", true);
                    sCheckFileNotFoundException = "CheckFileNotFoundException ~" + sErrorMessage;
                    return false;
                }
                else
                {
                    //Jira #CAP-39
                    if (iTrycount <= 3)
                    {
                        iTrycount++;
                        System.Threading.Thread.Sleep(1500);
                        goto TryAgain;
                    }
                    else
                    {
                        //UtilityManager.RetryExecptionLog(ex, iTrycount);
                        //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "FTpImageProcess.cs Line No - 155 - " + ex.Message + " - Username is " + userName + " -  Password " + password + " - UNCAuthPath " + UNCAuthPath + " - UNCPAth" + UNCPath + " - URI - " + uri.Replace(ftpIP, UNCPath), DateTime.Now, "0", "frmimageviewer");
                    }
                }
            }
            return true;
        }

        public static bool CheckFileNotFoundException(Exception ex, out string sErrorMessage)
        {
            sErrorMessage = string.Empty;
            bool bCheckFileNotFoundException = false;
            if (ex.Message != null)
            {
                sErrorMessage = "MESSAGE: " + ex.Message + "\\n\\n";
            }
            if (ex.InnerException != null && ex.InnerException.Message != null)
            {
                sErrorMessage += "INNER EXCEPTION: " + ex.InnerException.Message + "\\n\\n";
            }
            if (ex.StackTrace != null)
            {
                sErrorMessage += "STACK TRACE: " + ex.StackTrace.ToString();
            }
            sErrorMessage = sErrorMessage.Replace("'", "");
            sErrorMessage = sErrorMessage.Replace(System.Environment.NewLine, "");
            bCheckFileNotFoundException = (sErrorMessage.Contains("Could not find file") ||
                                           sErrorMessage.Contains("Access to the path") ||
                                           sErrorMessage.Contains("Specific file is not present in the location") ||
                                           sErrorMessage.Contains("The process cannot access the file") ||
                                           sErrorMessage.Contains("Could not find a part of the path") ||
                                           sErrorMessage.Contains("pdf not found as file or resource") ||
                                           sErrorMessage.Contains("Invalid page number") ||
                                           sErrorMessage.Contains("trailer not found"));
            return bCheckFileNotFoundException;

        }

        public static void ImportIndexedDocumentsJob()
        {
            string sFacility = ConfigurationManager.AppSettings["FacilityNameForResults"];
            string sIncoming_IndexingFilePath = ConfigurationManager.AppSettings["IncomingIndexingFilePath"];
            string sImportIndexingFilePath = ConfigurationManager.AppSettings["ImportIndexingFilePath"];
            string sExceptionIndexingFilePath = ConfigurationManager.AppSettings["ExceptionIndexingFilePath"];
            string sCompletedTextFilePath = ConfigurationManager.AppSettings["CompletedTextFilePath"];
            IList<IndexingFileLookup> indexingFileLookupList = new IndexingFileLookupManager().GetIndexingFileLookup();
            IList<scan_index> ilstScanForFileNumber = new List<scan_index>();
            Scan_IndexManager scanindexmanager = new Scan_IndexManager();
            FileInfo[] sFiles = new DirectoryInfo(sIncoming_IndexingFilePath).GetFiles("*.txt");
            string sFile = string.Empty;
            ulong ulOrderSubmitId = 0;
            HumanManager MngrHuman = new HumanManager();
            IList<FileManagementIndex> fileManagementIndexList = new List<FileManagementIndex>();
            IList<Scan> ilstScan = new List<Scan>();
            IList<scan_index> ilstscan_index = new List<scan_index>();
            IList<OrdersSubmit> insertordersubmitList = new List<OrdersSubmit>();
            IList<Orders> insertOrderList = new List<Orders>();
            IList<FileManagementIndex> UpdatefileManagementIndexList = null;
            IList<Scan> UpdateScanList = null;
            IList<scan_index> UpdateScan_indexList = null;
            FileManagementIndexManager fileManagementIndexmanager = new FileManagementIndexManager();
            ScanManager scanManager = new ScanManager();
            OrdersManager ordersManager = new OrdersManager();

            if (sFiles.Length > 0)
            {
                FileInfo[] rgFiles = sFiles;
                foreach (FileInfo fi in rgFiles)
                {
                    bool bUnimportedFile = false;
                    try
                    {
                        ArrayList myResults = new ArrayList();
                        TextReader trs = new StreamReader(@fi.DirectoryName + "\\" + fi.Name);
                        string sOutput = trs.ReadToEnd();
                        trs.Close();
                        trs.Dispose();

                        string[] newarray = sOutput.Split('\n');
                        if (newarray != null)
                        {
                            for (int g = 0; g < newarray.Length; g++)
                            {
                                if (newarray[g].Contains("\r") == false)
                                {
                                    newarray[g] += "\r";
                                }
                            }
                        }
                        string sCurrentResult = string.Empty;
                        int j = 0;
                        string segmentField = string.Empty;
                        string segmentSubField = string.Empty;
                        string propName = string.Empty;

                        foreach (var item in newarray)
                        {
                            myResults.Add(item);
                        }

                        for (int k = 0; k < myResults.Count; k++)
                        {
                            string lineData = myResults[k].ToString().Replace("\r", "");
                            string[] line = { lineData };
                            if (line.Length > 0 && line.FirstOrDefault().Trim() != "")
                            {
                                for (int l = 0; l < line.Length; l++)
                                {
                                    insertordersubmitList = new List<OrdersSubmit>();
                                    insertOrderList = new List<Orders>();
                                    ilstScan = new List<Scan>();
                                    ilstscan_index = new List<scan_index>();
                                    fileManagementIndexList = new List<FileManagementIndex>();

                                    sFile = string.Empty;
                                    string[] segments = line[l].Split('|');
                                    if (segments.Length > 0)
                                    {
                                        object instance = null;
                                        object humanInstance = null;
                                        var humanResultLookupList = indexingFileLookupList.Where(a => a.Segment_Name == "HHH").ToList();
                                        if (humanResultLookupList.Any())
                                        {
                                            string domain = humanResultLookupList[0].Domain_Object;
                                            var currentAssembly = Assembly.Load("Acurus.Capella.Core");
                                            var myType = currentAssembly.GetType(domain);
                                            humanInstance = Activator.CreateInstance(myType);
                                        }
                                        else if (humanResultLookupList.Count == 0)
                                        {
                                            humanInstance = null;
                                        }
                                        var fileResultLookupList = indexingFileLookupList.Where(a => a.Segment_Name == "FMI").ToList();
                                        if (fileResultLookupList.Any())
                                        {
                                            string domain = fileResultLookupList[0].Domain_Object;
                                            var currentAssembly = Assembly.Load("Acurus.Capella.Core");
                                            var myType = currentAssembly.GetType(domain);
                                            instance = Activator.CreateInstance(myType);
                                        }
                                        else if (fileResultLookupList.Count == 0)
                                        {
                                            instance = null;
                                        }
                                        Human humanData = new Human();
                                        if (humanInstance != null && humanResultLookupList.Any())
                                        {
                                            for (int i = 0; i < segments.Length; i++)
                                            {
                                                IndexingFileLookup re = null;
                                                string segmentName = humanResultLookupList[0].Segment_Name;
                                                segmentField = segmentName + "-" + (i + 1).ToString();

                                                IList<IndexingFileLookup> reList = humanResultLookupList.Where(res => res.Segment_Name == segmentName
                                                                                                    && res.Segment_Field == segmentField
                                                                                                    && res.Segment_Sub_Field.Trim() == string.Empty).ToList();
                                                if (reList.Count != 0)
                                                {
                                                    re = reList[0];
                                                }
                                                if (re != null)
                                                {
                                                    /*With the column name the property can be found and value can be assigned.*/
                                                    PropertyInfo propInfo = humanInstance.GetType().GetProperty(re.Column_Name);
                                                    object value = segments[i];
                                                    if (propInfo.PropertyType == typeof(DateTime))
                                                    {
                                                        if (!string.IsNullOrEmpty(value.ToString()))
                                                        {
                                                            value = DateTime.ParseExact(Convert.ToDateTime(segments[i]).ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                                        }
                                                        else
                                                        {
                                                            value = DateTime.MinValue;
                                                        }
                                                    }
                                                    else if (propInfo.PropertyType != typeof(string))
                                                    {
                                                        if (propInfo.PropertyType.Name.ToLower().Contains("int") && string.IsNullOrEmpty(value.ToString()))
                                                        {
                                                            value = Convert.ChangeType("0", propInfo.PropertyType);
                                                        }
                                                        else
                                                        {
                                                            value = Convert.ChangeType(value, propInfo.PropertyType);
                                                        }
                                                    }
                                                    propInfo.SetValue(humanInstance, value, null);
                                                }
                                            }

                                            if (instance != null && humanInstance != null)
                                            {
                                                Human human = (Human)humanInstance;
                                                HumanManager humanManager = new HumanManager();
                                                humanData = humanManager.GetHumanIdbyname(human.Id, human.First_Name, human.Last_Name, human.Birth_Date.ToString("yyyy-MM-dd"));
                                            }
                                        }
                                        
                                        if (instance != null)
                                        {
                                            for (int i = 0; i < segments.Length; i++)
                                            {
                                                IndexingFileLookup re = null;
                                                string segmentName = fileResultLookupList[0].Segment_Name;
                                                segmentField = segmentName + "-" + (i + 1).ToString();

                                                IList<IndexingFileLookup> reList = fileResultLookupList.Where(res => res.Segment_Name == segmentName
                                                                                                    && res.Segment_Field == segmentField
                                                                                                    && res.Segment_Sub_Field.Trim() == string.Empty).ToList();
                                                if (reList.Count != 0)
                                                {
                                                    re = reList[0];
                                                }
                                                if (re != null)
                                                {
                                                    /*With the column name the property can be found and value can be assigned.*/
                                                    PropertyInfo propInfo = instance.GetType().GetProperty(re.Column_Name);
                                                    object value = segments[i];
                                                    if (propInfo.PropertyType == typeof(DateTime))
                                                    {
                                                        if (!string.IsNullOrEmpty(value.ToString()))
                                                        {
                                                            value = DateTime.ParseExact(Convert.ToDateTime(segments[i]).ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                                        }
                                                        else
                                                        {
                                                            value = DateTime.MinValue;
                                                        }
                                                    }
                                                    else if (propInfo.PropertyType != typeof(string))
                                                    {
                                                        if (propInfo.PropertyType.Name.ToLower().Contains("int") && string.IsNullOrEmpty(value.ToString()))
                                                        {
                                                            value = Convert.ChangeType("0", propInfo.PropertyType);
                                                        }
                                                        else
                                                        {
                                                            value = Convert.ChangeType(value, propInfo.PropertyType);
                                                        }
                                                    }
                                                    propInfo.SetValue(instance, value, null);
                                                }
                                            }
                                            if (instance != null)
                                            {
                                                FileManagementIndex fileManagementIndex = (FileManagementIndex)instance;
                                                sFile = fileManagementIndex.File_Path;
                                                if (humanData == null || (humanData != null && humanData.Id == 0))
                                                {
                                                    SaveExceptionIntoIndexingExceptionLog(line[l], "Incorrect Patient Match");
                                                    MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                    continue;
                                                }
                                                if (fileManagementIndex.File_Path == string.Empty)
                                                {
                                                    SaveExceptionIntoIndexingExceptionLog(line[l], "Missing File Name from "+ fi.Name);
                                                    continue;
                                                }
                                                if (!File.Exists(sIncoming_IndexingFilePath + "//" + sFile))
                                                {
                                                    SaveExceptionIntoIndexingExceptionLog(line[l], "File not found");
                                                    continue;
                                                }

                                                if (string.IsNullOrEmpty(fileManagementIndex.Document_Type))
                                                {
                                                    SaveExceptionIntoIndexingExceptionLog(line[l], "Missing Document Type");
                                                    MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                    continue;
                                                }
                                                else
                                                {
                                                    doctypeList objDoctypeList = new doctypeList();
                                                    StaticLookupManager staticLookupManager = new StaticLookupManager();
                                                    var docType = staticLookupManager.getStaticLookupByFieldName("document type");
                                                    if (docType == null || !docType.Any(a => a.Value.ToLower() == fileManagementIndex.Document_Type.ToLower()))
                                                    {
                                                        SaveExceptionIntoIndexingExceptionLog(line[l], "Incorrect Document Type");
                                                        MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                        continue;
                                                    }
                                                }
                                                if (string.IsNullOrEmpty(fileManagementIndex.Document_Sub_Type))
                                                {
                                                    SaveExceptionIntoIndexingExceptionLog(line[l], "Missing Document Sub Type");
                                                    MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                    continue;
                                                }
                                                else
                                                {
                                                    doctypeList objDoctypeList = new doctypeList();
                                                    StaticLookupManager staticLookupManager = new StaticLookupManager();
                                                    var subDoc = staticLookupManager.getStaticLookupByFieldName(fileManagementIndex.Document_Type);
                                                    if (subDoc == null || !subDoc.Any(a => a.Value.ToLower() == fileManagementIndex.Document_Sub_Type.ToLower()))
                                                    {
                                                        SaveExceptionIntoIndexingExceptionLog(line[l], "Incorrect Document Sub Type");
                                                        MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                        continue;
                                                    }
                                                }

                                                if (fileManagementIndex.Document_Type.ToUpper() == "ENCOUNTERS")
                                                {
                                                    if (fileManagementIndex.Encounter_ID == 0)
                                                    {
                                                        SaveExceptionIntoIndexingExceptionLog(line[l], "Encounter details not found");
                                                        MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        EncounterManager encounterManager = new EncounterManager();
                                                        IList<Encounter> ilstEncounter = new List<Encounter>();
                                                        ilstEncounter = encounterManager.GetEncounterByEncounterIDIncludeArchive(fileManagementIndex.Encounter_ID);
                                                        if (ilstEncounter.Count == 0 || ilstEncounter.FirstOrDefault().Human_ID != fileManagementIndex.Human_ID)
                                                        {
                                                            SaveExceptionIntoIndexingExceptionLog(line[l], "Encounter details not match with the Patient");
                                                            MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                            continue;
                                                        }
                                                    }
                                                }
                                                
                                                if (fileManagementIndex.Document_Date == DateTime.MinValue)
                                                {
                                                    SaveExceptionIntoIndexingExceptionLog(line[l], "Missing Document Date");
                                                    MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                    continue;
                                                }
                                                if (fileManagementIndex.Document_Date < humanData.Birth_Date)
                                                {
                                                    SaveExceptionIntoIndexingExceptionLog(line[l], "Invalid Document Date");
                                                    MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                    continue;
                                                }
                                                if (fileManagementIndex.Document_Date > TimeZoneInfo.ConvertTimeToUtc(DateTime.Now))
                                                {
                                                    SaveExceptionIntoIndexingExceptionLog(line[l], "Document_Date cannot be the future date");
                                                    MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sExceptionIndexingFilePath + "//" + sFile);
                                                    continue;
                                                }

                                                Console.WriteLine("FTP started");
                                                #region FTP Transfer
                                                fileManagementIndex.Document_Date = Convert.ToDateTime(DateTime.ParseExact(fileManagementIndex.Document_Date.ToString("yyyyMMdd"), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                                string ftpServerIP = ConfigurationManager.AppSettings["ftpServerIP"];
                                                bool bCreateDirectory = CreateDirectory(fileManagementIndex.Human_ID.ToString(), ftpServerIP, string.Empty, string.Empty, out string sCheckFileNotFoundException);
                                                if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                                                {
                                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                                                    //return;
                                                }
                                                if (bCreateDirectory)
                                                {
                                                    string serverPath = string.Empty;
                                                    //Jira CAP-3035
                                                    int iNumberOfFile = 1;
                                                    ilstScanForFileNumber = scanindexmanager.GetLastTransactionByHuman(Convert.ToUInt64(fileManagementIndex.Human_ID));
                                                    string sIndexed_File_Path = (ilstScanForFileNumber != null && ilstScanForFileNumber.Count > 0) ? ilstScanForFileNumber.FirstOrDefault().Indexed_File_Path : string.Empty;
                                                    if (sIndexed_File_Path != string.Empty)
                                                    {
                                                        iNumberOfFile = Convert.ToInt32(sIndexed_File_Path.ToString().Substring(sIndexed_File_Path.ToString().LastIndexOf("_") + 1, (sIndexed_File_Path.ToString().LastIndexOf(".") - 1) - sIndexed_File_Path.ToString().LastIndexOf("_"))) + 1;
                                                    }
                                                    sIndexed_File_Path = sFile.Replace(".pdf", "") + "_" + ((iNumberOfFile.ToString().Length == 1) ? "0" + iNumberOfFile.ToString() : iNumberOfFile.ToString()) + ".pdf";

                                                    //serverPath = UploadToImageServer(objHuman.Id.ToString(), ftpServerIP, string.Empty, string.Empty, sIncoming_StudiesFilePath + "//" + sFile, string.Empty, out string sCheckFileNotFoundExceptions);
                                                    serverPath = UploadToImageServer(fileManagementIndex.Human_ID.ToString(), ftpServerIP, string.Empty, string.Empty, sIncoming_IndexingFilePath + "//" + sFile, sIndexed_File_Path, out string sCheckFileNotFoundExceptions);
                                                    //Jira CAP-3035 - End
                                                    if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                                                    {
                                                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                                                        //return;
                                                    }

                                                    if (serverPath != string.Empty)
                                                    {
                                                        if (fileManagementIndex.Document_Type.ToUpper() == "RESULTS")
                                                        {
                                                            OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                                                            objOrdersSubmit.Human_ID = fileManagementIndex.Human_ID;
                                                            objOrdersSubmit.Created_By = "IndexingFileAgent";
                                                            objOrdersSubmit.Created_Date_And_Time = DateTime.UtcNow;
                                                            objOrdersSubmit.Facility_Name = sFacility;
                                                            objOrdersSubmit.Specimen_Collection_Date_And_Time = fileManagementIndex.Document_Date;
                                                            objOrdersSubmit.Order_Type = "DIAGNOSTIC ORDER";
                                                            //ordersSubmit.Physician_ID = 4387;
                                                            insertordersubmitList.Add(objOrdersSubmit);


                                                            Orders objOrder = new Orders
                                                            {
                                                                Lab_Procedure = "Paper Order",
                                                                Human_ID = fileManagementIndex.Human_ID,
                                                                Created_By = "IndexingFileAgent",
                                                                Created_Date_And_Time = DateTime.UtcNow
                                                            };
                                                            insertOrderList.Add(objOrder);

                                                            ulOrderSubmitId = ordersManager.InsertDummyOrder(insertordersubmitList, insertOrderList, "DIAGNOSTIC ORDER", sFacility, string.Empty);
                                                        }

                                                        Scan scan = new Scan();
                                                        scan.Scanned_File_Path = sImportIndexingFilePath + "\\" + sFile;
                                                        //scan.Scanned_Date = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[3].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                                        scan.Scanned_Date = fileManagementIndex.Document_Date;
                                                        scan.Facility_Name = sFacility;
                                                        scan.No_of_Pages = GetPageCountFromPDF(sIncoming_IndexingFilePath + "//" + sFile);
                                                        scan.Scanned_File_Name = sFile;
                                                        scan.Scan_Type = "Online Chart - LOCAL";
                                                        scan.Created_By = "IndexingFileAgent";
                                                        scan.Created_Date_And_Time = DateTime.UtcNow;
                                                        ilstScan.Add(scan);
                                                        scanManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstScan, ref UpdateScanList, null, string.Empty, false, false, fileManagementIndex.Human_ID, string.Empty);

                                                        scan_index scan_Index = new scan_index();
                                                        scan_Index.Human_ID = fileManagementIndex.Human_ID;
                                                        scan_Index.Scan_ID = Convert.ToUInt64(ilstScan.FirstOrDefault().Id);
                                                        //scan_Index.Document_Date = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[3].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                                        scan_Index.Document_Date = fileManagementIndex.Document_Date;
                                                        scan_Index.Document_Type = fileManagementIndex.Document_Type;
                                                        //Jira CAP-2977
                                                        //scan_Index.Document_Sub_Type = dir.Name.ToUpper();
                                                        //Jira CAP-3038
                                                        //scan_Index.Document_Sub_Type = sFile.Split('_')[4].ToUpper().Replace(".PDF", "").ToUpper();
                                                        //scan_Index.Document_Sub_Type = ilstDocument_Sub_type.Document_Sub_Type_Lookup.Where(x => x.File_Report_Type.ToUpper() == sFile.Split('_')[4].ToUpper().Replace(".PDF", "")).FirstOrDefault().Document_Sub_Type;
                                                        scan_Index.Document_Sub_Type = fileManagementIndex.Document_Sub_Type;
                                                        scan_Index.Order_ID = ulOrderSubmitId;
                                                        //scan_Index.Indexed_File_Path = sImported_StudiesFilePath + "\\" + sFile;
                                                        scan_Index.Indexed_File_Path = serverPath.Replace("ftp:", "").Replace(@"//", @"\\").Replace(@"/", @"\"); ;
                                                        scan_Index.Page_Selected = "1-" + ilstScan.FirstOrDefault().No_of_Pages;
                                                        scan_Index.Created_By = "IndexingFileAgent";
                                                        scan_Index.Created_Date_And_Time = DateTime.UtcNow;
                                                        scan_Index.Is_Manually_Reviewed_And_Signed = "N";
                                                        scan_Index.Is_External_Medical_Record = "N";
                                                        //scan_Index.Appointment_Provider_ID = 4387;
                                                        scan_Index.Encounter_ID = fileManagementIndex.Encounter_ID;
                                                        scan_Index.Facility_Name = sFacility;
                                                        ilstscan_index.Add(scan_Index);

                                                        scanindexmanager.SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstscan_index, ref UpdateScan_indexList, null, string.Empty, false, false, fileManagementIndex.Human_ID, string.Empty);

                                                        //FileManagementIndex filemanagementIndex = new FileManagementIndex();
                                                        fileManagementIndex.Created_By = "IndexingFileAgent";
                                                        fileManagementIndex.Created_Date_And_Time = DateTime.UtcNow;
                                                        //filemanagementIndex.Document_Date = Convert.ToDateTime(DateTime.ParseExact(sFile.Split('_')[3].ToUpper().Replace(".PDF", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 15:30:00");
                                                        //fileManagementIndex.Document_Date = fileManagementIndex.Document_Date;
                                                        //Jira CAP-2977
                                                        //filemanagementIndex.Document_Sub_Type = dir.Name.ToUpper();
                                                        //Jira CAP-3038
                                                        //filemanagementIndex.Document_Sub_Type = sFile.Split('_')[4].ToUpper().Replace(".PDF", "").ToUpper();
                                                        //filemanagementIndex.Document_Sub_Type = ilstDocument_Sub_type.Document_Sub_Type_Lookup.Where(x => x.File_Report_Type.ToUpper() == sFile.Split('_')[4].ToUpper().Replace(".PDF", "")).FirstOrDefault().Document_Sub_Type;
                                                        fileManagementIndex.Source = "SCAN";
                                                        fileManagementIndex.Order_ID = ulOrderSubmitId;
                                                        fileManagementIndex.Scan_Index_Conversion_ID = ilstscan_index.FirstOrDefault().Id;
                                                        fileManagementIndex.File_Path = serverPath;
                                                        fileManagementIndex.Is_Delete = "N";
                                                        fileManagementIndex.Facility_Name = sFacility;
                                                        if (fileManagementIndex.Document_Sub_Type.Trim().ToUpper() == "ADVANCE DIRECTIVE" || fileManagementIndex.Document_Sub_Type.Trim().ToUpper() == "BIRTH PLAN")
                                                        {
                                                            fileManagementIndex.Generate_Link_File_Path = fileManagementIndex.File_Path;
                                                        }
                                                        fileManagementIndex.Batch_Status = "OPEN";
                                                        fileManagementIndex.Printed_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                        fileManagementIndexList.Add(fileManagementIndex);
                                                        ulong[] uScanID = { 0 };

                                                        fileManagementIndexmanager.SaveUpdateDelete_DBAndXML_WithTransaction(ref fileManagementIndexList, ref UpdatefileManagementIndexList, null, string.Empty, true, true, fileManagementIndex.Human_ID, string.Empty);
                                                        Console.WriteLine(sFile + " - Import to Imported_Files Folder");
                                                    }
                                                }

                                                #endregion
                                                Console.WriteLine("FTP Ended");
                                                MoveAndReplace(sIncoming_IndexingFilePath + "//" + sFile, sImportIndexingFilePath + "//" + sFile);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }

                    MoveAndReplace(sIncoming_IndexingFilePath + "//" + fi.Name, sCompletedTextFilePath + "//" + fi.Name);
                }
            }
        }

        public static void SaveExceptionIntoIndexingExceptionLog(string sExceptionLine, string sExceptionDiscription)
        {
            IList<IndexingFileLookup> indexingFileLookupList = new IndexingFileLookupManager().GetIndexingFileLookup();
            string sIncoming_IndexingFilePath = ConfigurationManager.AppSettings["IncomingIndexingFilePath"];
            string sExceptionIndexingFilePath = ConfigurationManager.AppSettings["ExceptionIndexingFilePath"];
            string sCompletedTextFilePath = ConfigurationManager.AppSettings["CompletedTextFilePath"];
            string segmentField = string.Empty;
            string segmentSubField = string.Empty;
            string propName = string.Empty;

            string lineData = sExceptionLine.Replace("\r", "");
            string[] line = { lineData };
            if (line.Length > 0 && line.FirstOrDefault().Trim() != "")
            {
                for (int l = 0; l < line.Length; l++)
                {
                    string[] segments = line[l].Split('|');
                    if (segments.Length > 0)
                    {
                        object instance = null;
                        var fileResultLookupList = indexingFileLookupList.Where(a => a.Segment_Name == "IEL").ToList();
                        if (fileResultLookupList.Any())
                        {
                            string domain = fileResultLookupList[0].Domain_Object;
                            var currentAssembly = Assembly.Load("Acurus.Capella.Core");
                            var myType = currentAssembly.GetType(domain);
                            instance = Activator.CreateInstance(myType);
                        }
                        else if (fileResultLookupList.Count == 0)
                        {
                            instance = null;
                        }

                        if (instance != null)
                        {
                            for (int i = 0; i < segments.Length; i++)
                            {
                                IndexingFileLookup re = null;
                                string segmentName = fileResultLookupList[0].Segment_Name;
                                segmentField = segmentName + "-" + (i + 1).ToString();

                                IList<IndexingFileLookup> reList = fileResultLookupList.Where(res => res.Segment_Name == segmentName
                                                                                    && res.Segment_Field == segmentField
                                                                                    && res.Segment_Sub_Field.Trim() == string.Empty).ToList();
                                if (reList.Count != 0)
                                {
                                    re = reList[0];
                                }
                                if (re != null)
                                {
                                    /*With the column name the property can be found and value can be assigned.*/
                                    PropertyInfo propInfo = instance.GetType().GetProperty(re.Column_Name);
                                    object value = segments[i];
                                    if (propInfo.PropertyType == typeof(DateTime))
                                    {
                                        value = DateTime.ParseExact(segments[i], "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    }
                                    else if (propInfo.PropertyType != typeof(string))
                                    {
                                        if (propInfo.PropertyType.Name.ToLower().Contains("int") && string.IsNullOrEmpty(value.ToString()))
                                        {
                                            value = Convert.ChangeType("0", propInfo.PropertyType);
                                        }
                                        else
                                        {
                                            value = Convert.ChangeType(value, propInfo.PropertyType);
                                        }
                                    }
                                    propInfo.SetValue(instance, value, null);
                                }
                            }
                            if (instance != null)
                            {
                                IndexingExceptionLog indexingExceptionLog = (IndexingExceptionLog)instance;
                                IList<IndexingExceptionLog> indexingExceptionLogList = new List<IndexingExceptionLog>();
                                
                                if (Path.GetExtension(sIncoming_IndexingFilePath + @"\" + indexingExceptionLog.File_Name).ToUpper() == ".PDF")
                                {
                                    if (File.Exists(sIncoming_IndexingFilePath + @"\" + indexingExceptionLog.File_Name))
                                    {
                                        var pdfReader = new PdfReader(sIncoming_IndexingFilePath + @"\" + indexingExceptionLog.File_Name);
                                        indexingExceptionLog.No_of_Pages = pdfReader.NumberOfPages;
                                    }
                                }
                                indexingExceptionLog.Order_Number = "Paper Order";
                                indexingExceptionLog.Created_By = "IndexingFileAgent";
                                indexingExceptionLog.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                indexingExceptionLog.Is_Active = "Y";
                                indexingExceptionLog.Reason_Description = sExceptionDiscription;
                                indexingExceptionLog.File_Name = sExceptionIndexingFilePath + @"\" + indexingExceptionLog.File_Name;
                                indexingExceptionLog.File_Name = indexingExceptionLog.File_Name.Replace(@"\", @"\\");
                                indexingExceptionLogList.Add(indexingExceptionLog);
                                IndexingExceptionLogManager indexingExceptionLogManager = new IndexingExceptionLogManager();
                                indexingExceptionLogManager.SaveUpdateDeleteWithTransaction(ref indexingExceptionLogList, new List<IndexingExceptionLog>(), new List<IndexingExceptionLog>(), "");


                            }
                        }
                    }

                }
            }
        }
        public static int GetPageCountFromPDF(string sFullPath)
        {
            int PageCount = 1;
            try
            {
                PdfReader reader = new PdfReader(sFullPath);
                PageCount = reader.NumberOfPages;
            }
            catch (Exception ex)
            {
                
            }
            return PageCount;
        }
        public static void ImportIndexingExceptionLogJob()
        {
            IList<IndexingFileLookup> indexingFileLookupList = new IndexingFileLookupManager().GetIndexingFileLookup();
            string sExceptionIndexingFilePath = ConfigurationManager.AppSettings["ExceptionIndexingFilePath"];
            string sCompletedTextFilePath = ConfigurationManager.AppSettings["CompletedTextFilePath"];

            FileInfo[] sFiles = new DirectoryInfo(sExceptionIndexingFilePath).GetFiles("*.txt");
            if (sFiles.Length > 0)
            {
                FileInfo[] rgFiles = sFiles;
                foreach (FileInfo fi in rgFiles)
                {
                    bool bUnimportedFile = false;
                    try
                    {
                        ArrayList myResults = new ArrayList();
                        TextReader trs = new StreamReader(@fi.DirectoryName + "\\" + fi.Name);
                        string sOutput = trs.ReadToEnd();
                        trs.Close();
                        trs.Dispose();

                        string[] newarray = sOutput.Split('\n');
                        if (newarray != null)
                        {
                            for (int g = 0; g < newarray.Length; g++)
                            {
                                if (newarray[g].Contains("\r") == false)
                                {
                                    newarray[g] += "\r";
                                }
                            }
                        }
                        string sCurrentResult = string.Empty;
                        int j = 0;
                        string segmentField = string.Empty;
                        string segmentSubField = string.Empty;
                        string propName = string.Empty;

                        foreach (var item in newarray)
                        {
                            myResults.Add(item);
                        }

                        for (int k = 0; k < myResults.Count; k++)
                        {
                            string lineData = myResults[k].ToString().Replace("\r", "");
                            string[] line = { lineData };
                            if (line.Length > 0 && line.FirstOrDefault().Trim() != "")
                            {
                                for (int l = 0; l < line.Length; l++)
                                {
                                    string[] segments = line[l].Split('|');
                                    if (segments.Length > 0)
                                    {
                                        object instance = null;
                                        var fileResultLookupList = indexingFileLookupList.Where(a => a.Segment_Name == "IEL").ToList();
                                        if (fileResultLookupList.Any())
                                        {
                                            string domain = fileResultLookupList[0].Domain_Object;
                                            var currentAssembly = Assembly.Load("Acurus.Capella.Core");
                                            var myType = currentAssembly.GetType(domain);
                                            instance = Activator.CreateInstance(myType);
                                        }
                                        else if (fileResultLookupList.Count == 0)
                                        {
                                            instance = null;
                                        }

                                        if (instance != null)
                                        {
                                            for (int i = 0; i < segments.Length; i++)
                                            {
                                                IndexingFileLookup re = null;
                                                string segmentName = fileResultLookupList[0].Segment_Name;
                                                segmentField = segmentName + "-" + (i + 1).ToString();

                                                IList<IndexingFileLookup> reList = fileResultLookupList.Where(res => res.Segment_Name == segmentName
                                                                                                    && res.Segment_Field == segmentField
                                                                                                    && res.Segment_Sub_Field.Trim() == string.Empty).ToList();
                                                if (reList.Count != 0)
                                                {
                                                    re = reList[0];
                                                }
                                                if (re != null)
                                                {
                                                    /*With the column name the property can be found and value can be assigned.*/
                                                    PropertyInfo propInfo = instance.GetType().GetProperty(re.Column_Name);
                                                    object value = segments[i];
                                                    if (propInfo.PropertyType == typeof(DateTime))
                                                    {
                                                        value = DateTime.ParseExact(segments[i], "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                                    }
                                                    else if (propInfo.PropertyType != typeof(string))
                                                    {
                                                        if (propInfo.PropertyType.Name.ToLower().Contains("int") && string.IsNullOrEmpty(value.ToString()))
                                                        {
                                                            value = Convert.ChangeType("0", propInfo.PropertyType);
                                                        }
                                                        else
                                                        {
                                                            value = Convert.ChangeType(value, propInfo.PropertyType);
                                                        }
                                                    }
                                                    propInfo.SetValue(instance, value, null);
                                                }
                                            }
                                            if (instance != null)
                                            {
                                                IndexingExceptionLog indexingExceptionLog = (IndexingExceptionLog)instance;
                                                IList<IndexingExceptionLog> indexingExceptionLogList = new List<IndexingExceptionLog>();
                                                indexingExceptionLog.File_Name = sExceptionIndexingFilePath + @"\" + indexingExceptionLog.File_Name;
                                                if (Path.GetExtension(indexingExceptionLog.File_Name).ToUpper() == ".PDF")
                                                {
                                                    var pdfReader = new PdfReader(indexingExceptionLog.File_Name);
                                                    indexingExceptionLog.No_of_Pages = pdfReader.NumberOfPages;
                                                }
                                                indexingExceptionLog.Order_Number = "Paper Order";
                                                indexingExceptionLog.Created_By = "IndexingFileAgent";
                                                indexingExceptionLog.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                indexingExceptionLog.Is_Active = "Y";
                                                
                                                indexingExceptionLog.File_Name = indexingExceptionLog.File_Name.Replace(@"\", @"\\");
                                                indexingExceptionLogList.Add(indexingExceptionLog);
                                                IndexingExceptionLogManager indexingExceptionLogManager = new IndexingExceptionLogManager();
                                                indexingExceptionLogManager.SaveUpdateDeleteWithTransaction(ref indexingExceptionLogList, new List<IndexingExceptionLog>(), new List<IndexingExceptionLog>(), "");
                                                
                                                
                                            }
                                        }
                                    }
                                    
                                }
                            }
                        }
                        

                    }
                    catch (Exception e)
                    {

                        

                    }
                    MoveAndReplace(sExceptionIndexingFilePath + "//" + fi.Name, sCompletedTextFilePath + "//" + fi.Name);
                }
            }
        }
        //Cap - 3904
        public static void CCDXmlGenerateAgent()
        {
            string Facility_Name = ConfigurationManager.AppSettings["ECMFacilityName"];
            string sFolderPathName = ConfigurationManager.AppSettings["ECMSummaryPathName"];
            ConnectionStringSettingsCollection strConnectionData = ConfigurationManager.ConnectionStrings;
            string sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            string sStatus = string.Empty;
            try
            {
                string querydt = "select * from ccd_medex_update_info";

                DataSet dsGetDate = DBConnector.ReadData(querydt);
                DataTable dtGetDate = dsGetDate.Tables[0];
                string FileCreateDateTime = DateTime.Now.ToString("yyyyMMdd");

                string query = "select contact_id,h.human_id,e.Encounter_ID,w.Current_Arrival_Time,convert_tz(e.Date_of_Service,'Gmt','Us/Pacific') as Date_of_Service from patient_consent p left join human h on replace(p.contact_number,'Contact-','')=h.Dynamics_Number left join encounter e on h.human_id = e.Human_ID left join wf_object w on e.Encounter_ID = w.Obj_System_Id where p.medical_authorization_all_records='true' and h.Human_ID is not null and e.Facility_Name in " + Facility_Name + " and w.Obj_Type = 'Documentation' and w.Current_Process = 'Document_complete'  and w.Current_Arrival_Time >= '"+ Convert.ToDateTime(dtGetDate.Rows[0]["Last_Generated_Date_Time"]).ToString("yyyy-MM-dd") + "' union select contact_id,h.human_id,e.Encounter_ID,w.Current_Arrival_Time,convert_tz(e.Date_of_Service,'Gmt','Us/Pacific') as Date_of_Service from patient_consent p left join human h on replace(p.contact_number,'Contact-','')=h.Dynamics_Number left join encounter_arc e on h.human_id = e.Human_ID left join wf_object_arc w on e.Encounter_ID = w.Obj_System_Id where p.medical_authorization_all_records='true' and h.Human_ID is not null and e.Facility_Name in" + Facility_Name + " and w.Obj_Type = 'Documentation' and w.Current_Process = 'Document_complete'  and w.Current_Arrival_Time >= '" + Convert.ToDateTime(dtGetDate.Rows[0]["Last_Generated_Date_Time"]).ToString("yyyy-MM-dd")+"'";

                DataSet dsHuman = DBConnector.ReadData(query);
                DataTable dtHumanList = dsHuman.Tables[0];

                string sPrintPathName = string.Empty;
                sPrintPathName = sFolderPathName;
                Directory.CreateDirectory(sPrintPathName);

                for (int iCount = 0; iCount < dtHumanList.Rows.Count; iCount++)
                {
                    String InputValue = "Reason Of Visit,Vitals,Clinical Instruction,Immunizations,Mental Status,Care Plan,Laboratory Test(s),Smoking Status,Allergy,Functional Status,Procedure(s),Laboratory Values/Results,Encounter,Goals,Assessment,Medication,Medications Administered During visit,Treatment Plan,Problem List,Reason for Referral,Implants,Future Appointment,Health Concern,Lab Test,Laboratory Information,Diagnostics Tests Pending,Future Scheduled Tests,Patient Decision Aids,Payer";

                    string DateOfService = Convert.ToDateTime(dtHumanList.Rows[iCount]["Date_of_Service"]).ToString("yyyyMMdd");  

                    string filePath = Path.Combine(sPrintPathName,"Clinical_Summary_"+ dtHumanList.Rows[iCount]["Human_Id"]+"_"+dtHumanList.Rows[iCount]["Encounter_ID"]+"_"+ DateOfService+ "_"+ FileCreateDateTime+".xml");
                    XDocument doc = new XDocument(new XElement("Root"));
                    doc.Save(filePath);

                    sStatus = GenerateCCD(Convert.ToUInt64(dtHumanList.Rows[iCount]["Human_Id"]), Convert.ToUInt64(dtHumanList.Rows[iCount]["Encounter_Id"]), InputValue, filePath, string.Empty);
                    if (sStatus == "Success")
                    {
                        Console.WriteLine("CCD Generated Successfully For Encounter ID : "+ Convert.ToUInt64(dtHumanList.Rows[iCount]["Encounter_Id"]));

                    }
                    else
                    {
                        Console.WriteLine(sStatus +" For Encounter ID : " + Convert.ToUInt64(dtHumanList.Rows[iCount]["Encounter_Id"]));
                        Console.ReadLine();
                        System.Environment.Exit(1);
                    }
                }
                if (sStatus == "Success")
                {
                    string Query1 = "update ccd_medex_update_info set last_generated_date_time = date(now())";
                    var UpdateDate = new MySqlConnectionStringBuilder(sConnectionString);
                    MySqlConnection MyConn3 = new MySqlConnection(UpdateDate.ConnectionString);
                    MySqlCommand MyCommand3 = new MySqlCommand(Query1, MyConn3);
                    MySqlDataReader MyReader3;
                    MyConn3.Open();
                    MyReader3 = MyCommand3.ExecuteReader();
                    MyConn3.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message +"-"+ex.InnerException?.ToString() + " - " + ex.StackTrace);
                Console.ReadLine();
            }
        }
        public static string GenerateCCD(ulong ulHumanID, ulong ulEncounterID, string sCheckedItems, string sOutputLocation, string sDSN)
        {
            string sResult = string.Empty;

            try
            {
                bool ishumancount = GetListCCD();
                if (ishumancount)
                {
                    //Thread.Sleep(3000);
                    //goto ln;
                    Environment.Exit(0);
                }
                bool isHumanDone = InsertIntoListCCD(ulHumanID, ulEncounterID, sCheckedItems, sOutputLocation, sDSN);

                if (ulHumanID != null)
                {
                    // CreateXMLByBackupProcess("Human", Application, XML_ID.ToString());
                    string status = CreateCCDXMLByBatchProcess(sOutputLocation);
                    if (status == string.Empty)

                        sResult = "Success";

                    else
                        sResult = status;

                    string sConnectionString = string.Empty;
                    sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    var builder = new MySqlConnectionStringBuilder(sConnectionString);
                    try
                    {
                        using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                        {
                            DBConnection.Open();
                            using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                            {
                                string sQuery = "delete from list_ccd where encounter_id=" + ulEncounterID.ToString() + ";";
                                using (MySqlCommand cmdInsert = new MySqlCommand(sQuery, DBConnection, DBTransaction))
                                {
                                    cmdInsert.CommandText = sQuery;
                                    cmdInsert.CommandType = System.Data.CommandType.Text;
                                    try
                                    {
                                        cmdInsert.ExecuteNonQuery();
                                        DBTransaction.Commit();
                                    }
                                    catch (Exception e)
                                    {
                                        DBTransaction.Rollback();
                                        throw;
                                    }
                                    finally { }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //CAP-1942
                        sResult = e.Message + e;
                    }
                }
            }

            catch (Exception ex)
            {
                //CAP-1942
                sResult = "ERROR: " + ex.Message + " STACKTRACE: " + ex.StackTrace + ex;
            }
            return sResult;
        }

        public static string CreateCCDXMLByBatchProcess(string sOutputLocation)
        {
            string status = "";

            try
            {
                status = "First Block";
                status = "Third Block Block";
                string batchfile = System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                string CCDOutputLocation = ConfigurationManager.AppSettings["CCDOutputLocation"];
                if (File.Exists(batchfile))
                {
                    try
                    {
                        status = "Third Block - Sub 1";
                        var proc1 = new Process();
                        proc1.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString());
                        proc1.StartInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                        proc1.StartInfo.Arguments = "-v -s -a";
                        status = "Third Block - Sub 2";
                        bool bStart = proc1.Start();
                        status = bStart + " Third Block - Sub 3 ";
                        proc1.WaitForExit();
                        status = bStart + " Third Block - Sub 4 ";
                        var exitCode = proc1.ExitCode;
                        proc1.Close();

                        File.Copy(CCDOutputLocation, sOutputLocation, true);
                    }
                    catch (Exception ex)
                    {
                        //CAP-1942
                        throw new Exception(status + " " + ex.Message + "  " + ex.InnerException, ex);
                    }

                    //using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
                    //{
                    //    status = "Third Block - Sub 1";
                    //    proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString());
                    //    proc.StartInfo.FileName = "@" + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                    //    status = "Third Block - Sub 2" +" " + proc.StartInfo.WorkingDirectory + " " + proc.StartInfo.FileName;
                    //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "UtilityManager - 4522 - CreateCCDXMLByBatchProcess: status  - " + status + " : Start", DateTime.Now, "0", "frmimageviewer");
                    //    bool bStart = proc.Start();
                    //    status = bStart + " Third Block - Sub 3 " + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                    //    proc.WaitForExit();
                    //    status = bStart + " Third Block - Sub 4 " + System.Configuration.ConfigurationManager.AppSettings["XmlBatchFileForCCD"].ToString();
                    //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "UtilityManager - 4527 - CreateCCDXMLByBatchProcess: status  - " + status + " : End", DateTime.Now, "0", "frmimageviewer");
                    //}
                    status = string.Empty;
                }
                else
                {
                    status = "Batch File Not found-FileName:" + batchfile;
                }
                return status;

            }
            catch (Exception Ex)
            {
                //CAP-1942
                throw new Exception(status + " " + Ex.Message + "  " + Ex.InnerException, Ex);
            }

        }

        public static bool GetListCCD()
        {
            string sConnectionString = string.Empty;
            sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(sConnectionString);
            bool bExists = false;
            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                    {
                        string sQuery = "Select *  from list_ccd ; ";
                        using (MySqlCommand cmdCheck = new MySqlCommand(sQuery, DBConnection))
                        {
                            cmdCheck.CommandText = sQuery;
                            cmdCheck.CommandType = System.Data.CommandType.Text;
                            try
                            {
                                int iRows = 0;
                                string sResult = string.Empty;
                                iRows = Convert.ToInt32(cmdCheck.ExecuteScalar());
                                if (iRows > 0)
                                    bExists = true;

                            }
                            catch (Exception e)
                            {
                                //CAP-1942
                                throw new Exception(e.Message, e);
                            }
                            finally { }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //CAP-1942
                throw new Exception(e.Message, e);
            }
            return bExists;
        }
        public static bool InsertIntoListCCD(ulong humanID, ulong encounterID, string sInput, string sOutputLocation, string sDSN)
        {
            bool isInserted = false;
            string sConnectionString = string.Empty;
            sConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            var builder = new MySqlConnectionStringBuilder(sConnectionString);

            try
            {
                using (MySqlConnection DBConnection = new MySqlConnection(builder.ConnectionString))
                {
                    DBConnection.Open();
                    using (MySqlTransaction DBTransaction = DBConnection.BeginTransaction())
                    {
                        string sQuery = "insert into list_ccd values(" + humanID.ToString() + ", " + encounterID.ToString() + ", '" + sInput + "', '" + sOutputLocation.ToString() + "', '" + sDSN.ToString() + "');";
                        using (MySqlCommand cmdInsert = new MySqlCommand(sQuery, DBConnection, DBTransaction))
                        {
                            cmdInsert.CommandText = sQuery;
                            cmdInsert.CommandType = System.Data.CommandType.Text;
                            try
                            {
                                cmdInsert.ExecuteNonQuery();
                                DBTransaction.Commit();
                                isInserted = true;
                            }
                            catch (Exception e)
                            {
                                DBTransaction.Rollback();
                                isInserted = false;
                                //CAP-1942
                                throw new Exception(e.Message, e);
                            }
                            finally { }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //CAP-1942
                throw new Exception(e.Message, e);
            }

            return isInserted;
        }

    }
}
