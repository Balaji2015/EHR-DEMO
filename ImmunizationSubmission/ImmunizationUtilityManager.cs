using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.DataAccess.QuestResultService;
using System.Collections;
using System.Security.Authentication;
using Acurus.Capella.UI;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Configuration;

namespace Acurus.Capella.ImmunizationSubmission
{
    public partial class ImmunizationUtilityManager
    {

        //generate HL7 immunization registry
        public Human GetHumanByHumanID(ulong HumanID)
        {
            IList<Human> Humanlist = null;
            HumanManager objHumanManager = new HumanManager();
            Humanlist = objHumanManager.GetPatientDetailsUsingPatientInformattion(HumanID);
            if (Humanlist != null)
            {
                if (Humanlist.Count > 0)
                {
                    return Humanlist[0];
                }
            }
            return new Human();
        }
        public void GenerateImmunizationRegistry()
        {
            Human hn = null;
            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
            Encounter Enc = null;
            FillClinicalSummary lstFillClinicalSummary = null;
            WFObjectManager objwfobject = new WFObjectManager();
            IList<WFObject> lstlstwfobject = new List<WFObject>();
            Console.WriteLine("Get Immunization Order from wf_object table");
            lstlstwfobject = objwfobject.GetImmunizationOrder();
            ulong ulEncounterId = 0;
            ulong ulHumanId = 0;
            //Cap - 3164
            string sXmlRequest = string.Empty;
            EncounterManager objencountermanager = new EncounterManager();
            ImmunizationManager objImmunMngr = new ImmunizationManager();
            IList<ImmunizationSubmissionLog> lstlog = new List<ImmunizationSubmissionLog>();
            ImmunizationSubmissionLog objlog = new ImmunizationSubmissionLog();
            ImmunizationSubmissionLogManager objlogmanager = new ImmunizationSubmissionLogManager();
            for (int i = 0; i < lstlstwfobject.Count; i++)
            {
                Encounter lstencounter = new Encounter();
                lstlog = new List<ImmunizationSubmissionLog>();
                objlog = new ImmunizationSubmissionLog();
                objlogmanager = new ImmunizationSubmissionLogManager();
                Console.WriteLine("Get Immunization Order from Immunization table using wf_object_ID");
                IList<Immunization> objImmun = objImmunMngr.GetImmunizationUsingGroupID(lstlstwfobject[i].Obj_System_Id);

                if (objImmun != null && objImmun.Count > 0)
                {
                    if (objImmun[0].Encounter_Id == 0)
                    {
                        //Menu Level Orders are not submitted and moved to next process
                        DateTime dt = new DateTime();
                        dt = System.DateTime.Now;
                        objwfobject.MoveToNextProcess(lstlstwfobject[i].Obj_System_Id, lstlstwfobject[i].Obj_Type, 6, "UNKNOWN", dt, "", null, null);
                        continue;
                    }

                    ulEncounterId = objImmun[0].Encounter_Id;
                    ulHumanId = objImmun[0].Human_ID;



                    ArrayList aryResult = new ArrayList();



                    //*************************************************************
                    //To print the patient details
                    // WellnessNotes = ClinicalProxy.GetClinicalSummary(ClientSession.EncounterId, ulHumanId

                    ChiefComplaintsManager oblchiefcomplaints = new ChiefComplaintsManager();

                    lstFillClinicalSummary = oblchiefcomplaints.GetClinicalSummaryBulk(ulEncounterId, ulHumanId);
                    Console.WriteLine("Get human details using human_id");
                    hn = GetHumanByHumanID(ulHumanId);
                }
                if (lstFillClinicalSummary.Encounter != null && lstFillClinicalSummary.Encounter.Count > 0)
                {
                    Enc = lstFillClinicalSummary.Encounter[0];
                }
                PhysicianList = lstFillClinicalSummary.phyList;


                //string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

                //DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

                //if (!ObjSearchDir.Exists)
                //{
                //    ObjSearchDir.Create();
                //}

                //string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                //string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["ImmunizationRegistriesPathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                //Directory.CreateDirectory(sFolderPathName);
                //if (sFolderPathName == string.Empty)
                //{
                //    Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["ImmunizationRegistriesPathName"]);
                //}
                //else
                //{
                //    Directory.CreateDirectory(sFolderPathName);
                //}

                //To find the Primary Plan Name
                string PriPlan = string.Empty;
                IList<PatientInsuredPlan> PatInsList = new List<PatientInsuredPlan>();
                InsurancePlanManager InsurancePlanMngr = new InsurancePlanManager();
                PatInsList = lstFillClinicalSummary.Pat_Ins_Plan;
                if (PatInsList != null)
                {
                    for (int j = 0; j < PatInsList.Count; j++)
                    {
                        if (PatInsList[j].Insurance_Type.ToUpper() == "PRIMARY")
                        {
                            IList<InsurancePlan> InsPlan = InsurancePlanMngr.GetInsurancebyID(PatInsList[j].Insurance_Plan_ID);
                            //PriPlan = InsPlan.Ins_Plan_Name;
                            if (InsPlan != null && InsPlan.Count > 0)
                                PriPlan = InsPlan[0].External_Plan_Number;
                        }
                    }
                }

                string sResult = string.Empty;
                string immunizationRegistry;
                string sConnectivityTest = string.Empty;
                string sHL7Message = string.Empty;
                HL7Generator hl7Gen = new HL7Generator();
                string str2 = string.Empty;
                Console.WriteLine("Generate Immunization result...");

                if (PhysicianList.Count > 0)
                    immunizationRegistry = hl7Gen.CreateImmunizationRegistry(hn, PhysicianList[0], lstFillClinicalSummary, "");
                else
                    continue;


                string sDBConnectivity = System.Configuration.ConfigurationSettings.AppSettings["DBConnectivity"];
                if (sDBConnectivity != null && sDBConnectivity.ToUpper() == "PRODUCTION")
                {
                    try
                    {
                        //string immunizationRegistry = hl7Gen.CreateImmunizationRegistry(hn, PhysicianList[0], lstFillClinicalSummary, "");
                        //Cap - 1828 - Old Code - Start
                        //Console.WriteLine("Connect to production server...");
                        //ImmunizationSubmissionProduction.IS_PortTypeClient objImmunizationServiceProduction = new ImmunizationSubmissionProduction.IS_PortTypeClient();
                        //////ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                        ////ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                        //const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                        //const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                        //ServicePointManager.SecurityProtocol = Tls12;
                        //sConnectivityTest = objImmunizationServiceProduction.connectivityTest("TestPatientSep9");
                        //sHL7Message = objImmunizationServiceProduction.submitSingleMessage("SF-008625", "Qbe3iQzm", "CAIR", sResult);
                        //Console.WriteLine("Submitted Sucessfully...");
                        //Cap - 1828 - Old Code - End
                        //Cap - 1828 - New Code - Start
                        Console.WriteLine("Connect to production server...");
                        HttpWebRequest soapWebRequest = ImmunizationUtilityManager.CreateSOAPWebRequest();
                        string empty3 = string.Empty;
                        foreach (string readAllLine in System.IO.File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ConfigXML\\SOAPTemplate.txt"))
                            empty3 += readAllLine;
                        XmlDocument xmlDocument = new XmlDocument();
                        string xml = empty3.Replace("ImmunizationHL7Content", immunizationRegistry);
                        xmlDocument.LoadXml(xml);
                        //Cap - 3164
                        sXmlRequest = xml;

                        using (Stream requestStream = soapWebRequest.GetRequestStream())
                            xmlDocument.Save(requestStream);
                        using (WebResponse response = soapWebRequest.GetResponse())
                        {
                            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                string end = streamReader.ReadToEnd();
                                Console.WriteLine(end);
                                str2 = end;
                            }
                        }
                        Console.WriteLine("Submitted Sucessfully...");
                        //Cap - 1828 - New Code - End
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to submit ...");
                        DateTime dt = new DateTime();
                        dt = System.DateTime.Now;
                        objwfobject.MoveToNextProcess(lstlstwfobject[i].Obj_System_Id, lstlstwfobject[i].Obj_Type, 7, "UNKNOWN", dt, "", null, null);
                        objlog.Human_ID = ulHumanId;
                        objlog.Encounter_ID = ulEncounterId;
                        //if (resultsplit.Length > 10)
                        //    objlog.Control_ID = resultsplit[9].ToString();
                        //else
                        //    objlog.Control_ID = "";
                        objlog.Submission_Result_Type = "Failed to connect";
                        if (PhysicianList != null && PhysicianList.Count > 0)
                            objlog.Physician_ID = PhysicianList[0].Id;
                        //objlog.Result_Message = test; // resultsplit[resultsplit.Length - 1].ToString(); ;
                        objlog.Result_Message = str2 + Environment.NewLine + ex?.InnerException ??"" + Environment.NewLine + ex?.InnerException?.Message ?? ""; 
                        objlog.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        //Cap - 3164
                        objlog.Request_Message = sXmlRequest;
                        lstlog.Add(objlog);
                        objlogmanager.SaveUpdateDeleteWithTransaction(ref lstlog, null, null, string.Empty);
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine(ex.InnerException);
                        }
                        else
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Connect to testing server...");
                    try
                    {
                        // ImmunizationSubmissionTesting.IS_PortTypeClient objImmunizationServiceTesting = new ImmunizationSubmissionTesting.IS_PortTypeClient();
                        //// ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                        // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                        // sConnectivityTest = objImmunizationServiceTesting.connectivityTest("Acurus");
                        // sHL7Message = objImmunizationServiceTesting.submitSingleMessage("SF-008625", "Qbe3iQzm", "CAIR", sResult);
                        Console.WriteLine("Submitted Sucessfully...");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to submit ...");
                        DateTime dt = new DateTime();
                        dt = System.DateTime.Now;
                        objwfobject.MoveToNextProcess(lstlstwfobject[i].Obj_System_Id, lstlstwfobject[i].Obj_Type, 7, "UNKNOWN", dt, "", null, null);
                        objlog.Human_ID = ulHumanId;
                        objlog.Encounter_ID = ulEncounterId;
                        //if (resultsplit.Length > 10)
                        //    objlog.Control_ID = resultsplit[9].ToString();
                        //else
                        //    objlog.Control_ID = "";
                        objlog.Submission_Result_Type = "Failed to connect";
                        if (PhysicianList != null && PhysicianList.Count > 0)
                            objlog.Physician_ID = PhysicianList[0].Id;
                        //objlog.Result_Message = test; // resultsplit[resultsplit.Length - 1].ToString(); ;
                        objlog.Result_Message = str2 + Environment.NewLine + ex?.InnerException ??"" + Environment.NewLine + ex?.InnerException?.Message ?? ""; 
                        objlog.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        //Cap - 3164
                        objlog.Request_Message = sXmlRequest;
                        lstlog.Add(objlog);
                        objlogmanager.SaveUpdateDeleteWithTransaction(ref lstlog, null, null, string.Empty);
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine(ex.InnerException);
                        }
                        else
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }

                //  string test = sHL7Message;
                string test = str2;

                string[] resultsplit = test.Split('|');
                if (test.Contains("MSA|AA|"))
                {
                    DateTime dt = new DateTime();
                    dt = System.DateTime.Now;
                    objwfobject.MoveToNextProcess(lstlstwfobject[i].Obj_System_Id, lstlstwfobject[i].Obj_Type, 6, "UNKNOWN", dt, "", null, null);
                    objlog.Human_ID = ulHumanId;
                    objlog.Encounter_ID = ulEncounterId;
                    if (resultsplit.Length > 10)
                        objlog.Control_ID = resultsplit[9].ToString();
                    else
                        objlog.Control_ID = "";
                    objlog.Submission_Result_Type = "Success";
                    if (PhysicianList != null && PhysicianList.Count > 0)
                        objlog.Physician_ID = PhysicianList[0].Id;
                    //objlog.Result_Message = "";
                    objlog.Result_Message = test;
                    objlog.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    //Cap - 3164
                    objlog.Request_Message = sXmlRequest;
                    lstlog.Add(objlog);
                    objlogmanager.SaveUpdateDeleteWithTransaction(ref lstlog, null, null, string.Empty);

                }
                else
                {
                    DateTime dt = new DateTime();
                    dt = System.DateTime.Now;
                    objwfobject.MoveToNextProcess(lstlstwfobject[i].Obj_System_Id, lstlstwfobject[i].Obj_Type, 7, "UNKNOWN", dt, "", null, null);
                    objlog.Human_ID = ulHumanId;
                    objlog.Encounter_ID = ulEncounterId;
                    if (resultsplit.Length > 10)
                        objlog.Control_ID = resultsplit[9].ToString();
                    else
                        objlog.Control_ID = "";
                    objlog.Submission_Result_Type = "Fail";
                    if (PhysicianList != null && PhysicianList.Count > 0)
                        objlog.Physician_ID = PhysicianList[0].Id;
                    objlog.Result_Message = test; // resultsplit[resultsplit.Length - 1].ToString(); ;
                    objlog.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    //Cap - 3164
                    objlog.Request_Message = sXmlRequest;
                    lstlog.Add(objlog);
                    objlogmanager.SaveUpdateDeleteWithTransaction(ref lstlog, null, null, string.Empty);
                }
                //StreamWriter sr = new StreamWriter(sPrintPathName);
                //sr.Write(sResult);
                //sr.Close();
                //sr.Dispose();

                // aryResult.Add(sResult);
                //aryResult.Add(sPrintPathName);

                //return aryResult;

            }
        }

        public static HttpWebRequest CreateSOAPWebRequest()
        {
            HttpWebRequest soapWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationSettings.AppSettings["SubmitURL"]);
            soapWebRequest.ContentType = "application/soap+xml;charset=UTF-8;action=\"urn: cdc: iisb: 2011:submitSingleMessage\"";
            soapWebRequest.Accept = "text/xml";
            soapWebRequest.Method = "POST";
            return soapWebRequest;
        }

    }
}
