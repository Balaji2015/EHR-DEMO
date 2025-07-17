using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.DataAccess;
using System.Globalization;
using System.Threading.Tasks;
using static Acurus.Capella.DataAccess.RCopiaXMLResponseProcess;
using System.Configuration;
using Acurus.Capella.Core.DTO;
using System.Timers;
using System.IO;
using System.Net;
using Acurus.Capella.Core.DTOJson;
using Newtonsoft.Json;

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
    }
}
