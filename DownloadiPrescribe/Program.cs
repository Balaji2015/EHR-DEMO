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
using NHibernate;
using System.Collections;
using static Acurus.Capella.DataAccess.RCopiaXMLResponseProcess;
using System.Configuration;
using Acurus.Capella.Core.DTO;
using System.Timers;

namespace DownloadiPrescribe
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Order Task Creation Started.");
            Timer method1Timer = new Timer();
            method1Timer.Elapsed += (sender, e) => HandleException(CreateLabOrderTask, method1Timer);
            method1Timer.Interval = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["OrderTaskTimeSpan"])).TotalMilliseconds;
            method1Timer.Start();
            Console.WriteLine("Order Task Creation Completed.");

            Console.WriteLine("Download RCopia Started");
            Timer method2Timer = new Timer();
            method2Timer.Elapsed += (sender, e) => HandleException(DownloadRCopiaInfo, method2Timer);
            method2Timer.Interval = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["RCopiaTimeSpan"])).TotalMilliseconds;
            method2Timer.Start();
            Console.WriteLine("Download RCopia Completed");

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
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

                        if (order.Stat == "Y" && order.Created_Date_And_Time.ToUniversalTime().AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["statTimeSpan"])) >= currentDateTimeUTC)
                        {
                            Console.WriteLine($"Add patient notes for order with stat value as 'Y'.");
                            AddPatientNotes(order);
                        }
                        else if (order.Stat != "Y" && order.Created_Date_And_Time.ToUniversalTime().AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["withoutStatTimeSpan"])) >= currentDateTimeUTC)
                        {
                            AddPatientNotes(order);
                        }
                        Console.WriteLine($"Task created for order submit id: {order.Order_Submit_ID}");
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
            objpatientnotes.Message_Date_And_Time = orderSubmitData.Created_Date_And_Time.ToUniversalTime();
            objpatientnotes.Is_PatientChart = "N";
            objpatientnotes.Created_Date_And_Time = DateTime.UtcNow;

            var notesPriority = orderSubmitData.Stat == "Y" ? "Stat" : "";

            Console.WriteLine("Creating notes message based on order status");
            //New Order
            if ((orderSubmitData.Created_Date_And_Time != null) && (orderSubmitData.Created_Date_And_Time.ToString() != "0001-01-01") && (orderSubmitData.Modified_Date_And_Time == null || (orderSubmitData.Modified_Date_And_Time.ToString("yyyy-MM-dd") == "0001-01-01")) && (orderSubmitData?.Current_Process ?? string.Empty) != "DELETED_ORDER")
            {
                strNotes.AppendLine("The new ancillary order has been submitted.");
                strNotes.AppendLine($"Ancillary Order Date : {orderSubmitData.Created_Date_And_Time:MM-dd-yyyy}");
                strNotes.AppendLine($"Ancillary Order # : {orderSubmitData.Order_Submit_ID}");
                strNotes.AppendLine($"Priority :  {notesPriority}");
                strNotes.AppendLine($"Ordering Provider :  {orderSubmitData.Physician_Name}");
                strNotes.AppendLine($"Ancillary Order(s) :  {orderSubmitData.Lab_Procedure_Message}");
                strNotes.AppendLine($"Facility to perform Test :  {orderSubmitData.Lab_Name}");
                strNotes.AppendLine($"Associated Order ICD Codes :  {orderSubmitData.ICD_Description_Message}");

            }
            //Deleted Order
            else if ((orderSubmitData?.Current_Process ?? string.Empty) == "DELETED_ORDER")
            {
                strNotes.AppendLine("The ancillary order has been deleted.");
                strNotes.AppendLine($"Ancillary Order Date : {orderSubmitData.Created_Date_And_Time:MM-dd-yyyy}");
                strNotes.AppendLine($"Ancillary Order # : {orderSubmitData.Order_Submit_ID}");
                strNotes.AppendLine($"Priority :  {notesPriority}");
                strNotes.AppendLine($"Ordering Provider :  {orderSubmitData.Physician_Name}");
                strNotes.AppendLine($"Ancillary Order(s) :  {orderSubmitData.Lab_Procedure_Message}");
                strNotes.AppendLine($"Facility to perform Test :  {orderSubmitData.Lab_Name}");
                strNotes.AppendLine($"Associated Order ICD Codes :  {orderSubmitData.ICD_Description_Message}");
            }
            //Updated Order
            else
            {
                strNotes.AppendLine("The ancillary order has been updated.");
                strNotes.AppendLine($"Ancillary Order Date : {orderSubmitData.Created_Date_And_Time:MM-dd-yyyy}");
                strNotes.AppendLine($"Ancillary Order # : {orderSubmitData.Order_Submit_ID}");
                strNotes.AppendLine($"Priority :  {notesPriority}");
                strNotes.AppendLine($"Ordering Provider :  {orderSubmitData.Physician_Name}");
                strNotes.AppendLine($"Ancillary Order(s) :  {orderSubmitData.Lab_Procedure_Message}");
                strNotes.AppendLine($"Facility to perform Test :  {orderSubmitData.Lab_Name}");
                strNotes.AppendLine($"Associated Order ICD Codes :  {orderSubmitData.ICD_Description_Message}");
            }


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
    }
}
