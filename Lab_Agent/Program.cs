using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DomainObjects;
using System.Reflection;
using Acurus.Capella.DataAccess;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Runtime.Serialization;
using System.Net;
using System.Xml;
using System.Data;
using System.Configuration;
using Acurus.Capella;
using System.Diagnostics;
//using Acurus.Capella.Lab_Agent;
//using Acurus.Capella.UI;

#region "OpenXML Import"
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;
//using DocumentFormat.OpenXml;
//using WP = DocumentFormat.OpenXml.Wordprocessing;
//using wp = DocumentFormat.OpenXml.Drawing.Wordprocessing;
//using a = DocumentFormat.OpenXml.Drawing;
//using pic = DocumentFormat.OpenXml.Drawing.Pictures;
#endregion

namespace Acurus.Capella.LabAgent
{
    class Program
    {
        public static string LabAgentLog = string.Empty;

        static void Main(string[] args)
        {

            //string currentEnvironment = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

            #region "Lab Agent"

            //string StrSourceLocation = Application.StartupPath + "\\app.config";
            //string StrTargetLocation = Application.StartupPath + "\\Acurus.Capella.LabAgent.exe.config";
            //File.Delete(StrTargetLocation);
            //File.Copy(StrSourceLocation, StrTargetLocation);
            LabAgentLog = System.Configuration.ConfigurationSettings.AppSettings["LogFileLabAgent"];
            //if (!Directory.Exists(LabAgentLog.Split('\\')[0] + "\\" + LabAgentLog.Split('\\')[1]))
            //    Directory.CreateDirectory(LabAgentLog.Split('\\')[0] + "\\" + LabAgentLog.Split('\\')[1]);
            //if (!File.Exists(LabAgentLog))
            //{
            //    File.Create(LabAgentLog);
            //}
            //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(AppError);

            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            try
            {
                //Program objProgram = new Program();
                //objProgram.startPendingDownLoad();

                if (System.Configuration.ConfigurationSettings.AppSettings["Mode"].ToUpper().Contains("LAB"))
                {
                    Console.WriteLine(Environment.NewLine + "LAB Import Start..." + Environment.NewLine);
                    OrdersUtilityManager ordersUtilMngr = new OrdersUtilityManager();
                    ordersUtilMngr.CreateOrders();

                    ResultsUtilityManager resultsUtilMngr = new ResultsUtilityManager();
                    resultsUtilMngr.ResultSplitProcess();

                    Console.WriteLine(Environment.NewLine + "LAB Import End..." + Environment.NewLine);

                }
                else if (System.Configuration.ConfigurationSettings.AppSettings["Mode"].ToUpper().Contains("ABSTRACT"))
                {
                    Console.WriteLine(Environment.NewLine + "ABSTARCT Medication,Allergy Start..." + Environment.NewLine);
                    //RcopiaMedicationManager objRcopiaMedicationmngr = new RcopiaMedicationManager();
                    //objRcopiaMedicationmngr.RcopiaMedicationTempUpload();
                    Console.WriteLine(Environment.NewLine + "ABSTARCT Medication,Allergy End..." + Environment.NewLine);
                }
                else if (System.Configuration.ConfigurationSettings.AppSettings["Mode"].ToUpper().Contains("FAX"))
                {

                    Console.WriteLine("FAX Send Start...");
                    //TwilioFaxManager objTwilioFax = new TwilioFaxManager();
                    //objTwilioFax.FaxSentFiles();
                    Console.WriteLine("Send End...");

                    Console.WriteLine("Receive Start...");
                    //objTwilioFax = new TwilioFaxManager();
                    //objTwilioFax.FaxReceiveFiles();
                    Console.WriteLine("Receive End");
                }

            }
            catch (Exception e)
            {
                StringBuilder logmsg = new StringBuilder();
                logmsg.Append("Error Date and Time : " + DateTime.Now.ToString() + " - ");
                logmsg.Append("Error Message : " + e.Message.ToString() + " - ");
                if (e.InnerException != null)
                    logmsg.Append(e.InnerException.Message != null ? "InnerException Message : " + e.InnerException.Message.ToString() + " - " : "");
                else
                    logmsg.Append("Error : " + e.ToString() + Environment.NewLine);

                logmsg.Append("Stack Trace : " + e.StackTrace.ToString() + Environment.NewLine);
                Console.WriteLine(e.Message);
                using (TextWriter tx = new StreamWriter(LabAgentLog, true))
                {
                    tx.WriteLine(logmsg);
                }

                Console.ReadLine();
                System.Environment.Exit(1);

            }
            #endregion
        }

        #region "User Defined Methods"

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.ReadLine();
        }

        static void AppError(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            StringBuilder logmsg = new StringBuilder();
            logmsg.Append(e.Exception.Message.ToString());
            logmsg.Append(e.Exception.InnerException.Message != null ? "-" + e.Exception.InnerException.Message.ToString() : "-");
            logmsg.Append(DateTime.Now.ToString());
            Console.WriteLine(e.Exception.Message);
            using (TextWriter tx = new StreamWriter(LabAgentLog, true))
            {
                tx.WriteLine(logmsg);
            }
            Console.ReadLine();
        }



        static bool GenerateNotesForSignedEncounters(ulong humanID, ulong enocunterID, string physicianID)
        {
            bool notesStatus = false;





            return notesStatus;

        }


        #endregion

    }
}
