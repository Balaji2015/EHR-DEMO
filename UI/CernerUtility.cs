using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;

namespace Acurus.Capella.UI
{
    public class CernerUtility
    {
        public void RegisterPatient(string shumanid)
        {
            LogWrite("RegisterPatient", "Start");
            string query = "select human_id,First_Name,Last_Name,Home_Phone_No,Sex,Birth_Date,Street_Address1,City,State,ZipCode from human where (Is_Cerner_Registered='N' or Is_Cerner_Registered='' ) and human_id='" + shumanid + "'";
            DataSet dsReturn = DBConnector.ReadData(query);
            DataTable dtHuman = dsReturn.Tables[0];
            if (dtHuman.Rows.Count == 0)
            {
                LogWrite("RegisterPatient", "Selected patient was not found in human table.");
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine("No Records to found in human table.");
                //Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            string sWebRequestService = ConfigurationSettings.AppSettings["RegisterPatient"];
            for (int i = 0; i < dtHuman.Rows.Count; i++)
            {
                string sInput = File.ReadAllText(ConfigurationSettings.AppSettings["MyMapDrive"] + "CernerRegistrationQueryTemplate.xml", Encoding.UTF8);
                sInput = sInput.Replace(":CreationTime", DateTime.Now.ToString("yyyyMMddhhmmss"));
                sInput = sInput.Replace(":CapellaHumanID", dtHuman.Rows[i][0].ToString());
                sInput = sInput.Replace(":FirstName", dtHuman.Rows[i][1].ToString());
                sInput = sInput.Replace(":LastName", dtHuman.Rows[i][2].ToString());
                sInput = sInput.Replace(":HomePhoneNumber", dtHuman.Rows[i][3].ToString());
                sInput = sInput.Replace(":Gender", dtHuman.Rows[i][4].ToString().Substring(0, 1).ToUpper());
                sInput = sInput.Replace(":DOB", Convert.ToDateTime(dtHuman.Rows[i][5]).ToString("yyyyMMdd"));
                sInput = sInput.Replace(":StreetAddress", dtHuman.Rows[i][6].ToString());
                sInput = sInput.Replace(":City", dtHuman.Rows[i][7].ToString());
                sInput = sInput.Replace(":State", dtHuman.Rows[i][8].ToString());
                sInput = sInput.Replace(":ZipCode", dtHuman.Rows[i][9].ToString());

                //Invoke API
                string sOutput = "";
                sOutput = InvokePayerServiceforCerner(sInput, sWebRequestService);
                //Console.WriteLine(Environment.NewLine + sOutput);
                // //Console.Read();

                if (sOutput != null && sOutput != "")
                {
                    XmlDocument xDocPatReg = new XmlDocument();
                    xDocPatReg.LoadXml(sOutput);
                    string sAcknowledgement = xDocPatReg.GetElementsByTagName("urn:acknowledgement")[0].ChildNodes[0].Attributes.GetNamedItem("code").Value;
                    if (sAcknowledgement == "CA")
                    {
                        //update Is_Cerner_Registered = 'Y'  Cerner_Universal_Patient_ID ='" + sOutput + "' ,
                        string updateQuery = "update human set Is_Cerner_Registered='Y' where human_id='" + dtHuman.Rows[i][0].ToString() + "'";
                        DataSet dsReturnupdate = DBConnector.ReadData(updateQuery);
                    }
                    else
                    {
                        LogWrite("RegisterPatient ", sAcknowledgement + " " + dtHuman.Rows[i][0].ToString() + " " + DateTime.Now);
                    }
                }
            }

            LogWrite("RegisterPatient", "END");
        }

        //Get Cernerregister patient from human table to update Cerner_Universal_Patient_ID'
        public string  RetrievePatientUniversalID(string shumanID)
        {
            LogWrite("RetrievePatientUniversalID", "Start");
            string query = "select human_id from human where human_id='" + shumanID + "' and Is_Cerner_Registered='Y' and Cerner_Universal_Patient_ID='' ";
            DataSet dsReturn = DBConnector.ReadData(query);
            DataTable dtHuman = dsReturn.Tables[0];
            string sCernerID =string.Empty;
            if (dtHuman.Rows.Count == 0)
            {
                LogWrite("RetrievePatientUniversalID", "No Records to found in human table.");
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine("No Records to found in human table.");
                //Console.ForegroundColor = ConsoleColor.White;
                return sCernerID;
            }
            string sWebRequestService = ConfigurationSettings.AppSettings["RegisterPatient"];
            for (int i = 0; i < dtHuman.Rows.Count; i++)
            {
                string sInput = File.ReadAllText(System.Configuration.ConfigurationSettings.AppSettings["MyMapDrive"] + "CernerPatientRegisteration.xml", Encoding.UTF8);
                sInput = sInput.Replace(":CreationTime", DateTime.Now.ToString("yyyyMMddhhmmss"));
                sInput = sInput.Replace(":CapellaHumanID", dtHuman.Rows[i][0].ToString());
                string sOutput = InvokePayerServiceforCerner(sInput, sWebRequestService);
                //Console.WriteLine(Environment.NewLine + sOutput);
                if (sOutput != null && sOutput != "")
                {
                    XmlDocument xDocPatReg = new XmlDocument();
                    xDocPatReg.LoadXml(sOutput);
                    string sAcknowledgement = xDocPatReg.GetElementsByTagName("urn:acknowledgement")[0].ChildNodes[1].Attributes.GetNamedItem("code").Value;
                    if (sAcknowledgement == "AA")//&& xDocPatReg.GetElementsByTagName("urn:patient")[0].ChildNodes[0].Attributes.GetNamedItem("root").Value == "2.16.840.1.113883.3.8375")
                    {
                         sCernerID = xDocPatReg.GetElementsByTagName("urn:patient")[0].ChildNodes[0].Attributes.GetNamedItem("extension").Value;
                        //update Cerner_Universal_Patient_ID
                        string updateQuery = "update human set Cerner_Universal_Patient_ID='" + sCernerID + "' where human_id='" + dtHuman.Rows[i][0].ToString() + "'";
                        DataSet dsReturnupdate = DBConnector.ReadData(updateQuery);
                    }
                    else
                    {
                        LogWrite("RegisterPatient ", sAcknowledgement + " " + dtHuman.Rows[i][0].ToString() + " " + DateTime.Now);
                    }
                }
            }
            LogWrite("RetrievePatientUniversalID", "Stop");
            return "Cerner UID for this Patient : " + sCernerID + ".";
        }

        public string RegistryStoredQueryRequest(string shumanID)
        {
            string sReturn = string.Empty;
            DateTime dtDateTime = DateTime.UtcNow;
            string sDatetime = dtDateTime.ToString("yyyy-MM-dd");
            ////Console.WriteLine("Schedule Appointment_Date " + sDatetime);

            //As per selva encounter table will be removed from the query
            //string query = "select e.human_id, e.Appointment_Provider_ID,h.Cerner_Universal_Patient_ID  from encounter e, wf_object w, human h where h.human_id='" + shumanID + "' and date(e.appointment_date) ='" + sDatetime + "' and w.obj_type='ENCOUNTER' and w.obj_system_id=e.encounter_id and w.current_process='SCHEDULED' and h.human_id=e.human_id and Cerner_Universal_Patient_ID<>''";

            string query = "select h.human_id, '0',h.Cerner_Universal_Patient_ID  from  human h where h.human_id='" + shumanID + "' and h.Cerner_Universal_Patient_ID<>''";
            DataSet dsReturn = DBConnector.ReadData(query);
            DataTable dtEnc = dsReturn.Tables[0];
            if (dtEnc.Rows.Count == 0)
            {
                LogWrite("ProvideandRegisterDocumentSet", "Selected patient was not registered in the Cerner.");
                ////Console.ForegroundColor = ConsoleColor.Red;
                ////Console.WriteLine("No Records to found in human table.");
                ////Console.ForegroundColor = ConsoleColor.White;
                //sReturn = "Please schedule an appointment for the date " + sDatetime +".";
               // return sReturn;
                sReturn = "Selected patient was not successfully registered in the Cerner.";
                return sReturn;
            }

            string sWebRequestService = ConfigurationSettings.AppSettings["RegistryStoredQuery"];
            for (int i = 0; i < dtEnc.Rows.Count; i++)
            {
                string sInput = File.ReadAllText(System.Configuration.ConfigurationSettings.AppSettings["MyMapDrive"] + "CernerRegistryStoredQueryRequest.xml", Encoding.UTF8);
                sInput = sInput.Replace(":ReturnType", System.Configuration.ConfigurationSettings.AppSettings["ReturnType"]);
                sInput = sInput.Replace(":SearchType", System.Configuration.ConfigurationSettings.AppSettings["SearchType"]);

                /*   //As per selva encounter table will be removed. 
                 string queryDt = "select Appointment_Date from encounter where  human_id='" + dtEnc.Rows[i][0].ToString() + "' and date_of_service<>'0001-01-01 00:00:00' order by Appointment_Date desc limit 1";
                DataSet ReturnDt = DBConnector.ReadData(queryDt);
                DataTable EncDt = ReturnDt.Tables[0];
                if (EncDt.Rows.Count == 0)
                {
                    string queryDtArc = "select Appointment_Date from encounter_arc where  human_id='" + dtEnc.Rows[i][0].ToString() + "' and date_of_service<>'0001-01-01 00:00:00' order by Appointment_Date desc limit 1";
                    DataSet ReturnDtArc = DBConnector.ReadData(queryDtArc);
                    DataTable EncDtArc = dsReturn.Tables[0];
                    if (EncDtArc.Rows.Count == 0)
                    {
                        sInput = sInput.Replace(":ServiceStartTimeFrom", Convert.ToDateTime(EncDtArc.Rows[0][0]).ToString("yyyyMMddhhmmss"));
                        sInput = sInput.Replace(":ServiceStartTimeTo", Convert.ToDateTime(dtDateTime).ToString("yyyyMMddhhmmss"));
                        sInput = sInput.Replace(":ServiceStopTimeFrom", Convert.ToDateTime(EncDtArc.Rows[0][0]).ToString("yyyyMMddhhmmss"));
                        sInput = sInput.Replace(":ServiceStopTimeTo", Convert.ToDateTime(dtDateTime).ToString("yyyyMMddhhmmss"));
                    }
                    else
                    {
                        sInput = sInput.Replace(":ServiceStartTimeFrom", System.Configuration.ConfigurationSettings.AppSettings["ServiceTime"]);
                        sInput = sInput.Replace(":ServiceStartTimeTo", Convert.ToDateTime(dtDateTime).ToString("yyyyMMddhhmmss"));
                        sInput = sInput.Replace(":ServiceStopTimeFrom", System.Configuration.ConfigurationSettings.AppSettings["ServiceTime"]);
                        sInput = sInput.Replace(":ServiceStopTimeTo", Convert.ToDateTime(dtDateTime).ToString("yyyyMMddhhmmss"));
                    }
                }
                else
                {
                    sInput = sInput.Replace(":ServiceStartTimeFrom", Convert.ToDateTime(EncDt.Rows[0][0]).ToString("yyyyMMddhhmmss"));
                    sInput = sInput.Replace(":ServiceStartTimeTo", Convert.ToDateTime(dtDateTime).ToString("yyyyMMddhhmmss"));
                    sInput = sInput.Replace(":ServiceStopTimeFrom", Convert.ToDateTime(EncDt.Rows[0][0]).ToString("yyyyMMddhhmmss"));
                    sInput = sInput.Replace(":ServiceStopTimeTo", Convert.ToDateTime(dtDateTime).ToString("yyyyMMddhhmmss"));
                }*/

                sInput = sInput.Replace(":ServiceStartTimeFrom", System.Configuration.ConfigurationSettings.AppSettings["ServiceTime"]);
                sInput = sInput.Replace(":ServiceStartTimeTo", Convert.ToDateTime(dtDateTime).ToString("yyyyMMddhhmmss"));
                sInput = sInput.Replace(":ServiceStopTimeFrom", System.Configuration.ConfigurationSettings.AppSettings["ServiceTime"]);
                sInput = sInput.Replace(":ServiceStopTimeTo", Convert.ToDateTime(dtDateTime).ToString("yyyyMMddhhmmss"));

                sInput = sInput.Replace(":Cerner_Universal_Patient_ID", dtEnc.Rows[i][2].ToString());
                //sReturn = "Cerner UID for this Patient : " + dtEnc.Rows[i][2].ToString() + ".";
                string sOutput = InvokePayerServiceforCerner(sInput, sWebRequestService);
                  DirectoryInfo directorySelected = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"] + "\\" + ClientSession.PhysicianId);

                if (!directorySelected.Exists)
                {
                    directorySelected.Create();
                }
                FileInfo[] XmlFile = directorySelected.GetFiles();
                int iFilesLengthBeforedownload=0 ;
                if(XmlFile!=null&&XmlFile.Length>0)
                {
                 iFilesLengthBeforedownload= XmlFile.Length;
                }

                if (sOutput != null && sOutput != "")
                {
                    if (sOutput.Contains("ResponseStatusType:Success"))
                    {
                        XmlDocument xDocPatReg = new XmlDocument();
                        xDocPatReg.LoadXml(sOutput);
                        XmlNodeList sValue = xDocPatReg.GetElementsByTagName("rim:LocalizedString");
                       
                        foreach (XmlNode xCheck in sValue)
                        {
                            if (xCheck.Attributes.GetNamedItem("value").Value == "XDSDocumentEntry.uniqueId")
                            {
                               // sReturn = "File '" + xCheck.ParentNode.ParentNode.Attributes.GetNamedItem("value").Value + ".xml' download for the selected patient. Cerner UID for this Patient : " + dtEnc.Rows[i][2].ToString() + ".";
                               
                                RetrieveDocumentSetRequest(xCheck.ParentNode.ParentNode.Attributes.GetNamedItem("value").Value, dtEnc.Rows[i][1].ToString());

                                XmlFile = directorySelected.GetFiles();
                                int iFilesLengthCurrentdownload = 0;
                                if (XmlFile != null && XmlFile.Length > 0)
                                {
                                    try
                                    {
                                        if (XmlFile.Length > iFilesLengthBeforedownload)
                                          iFilesLengthCurrentdownload = XmlFile.Length - iFilesLengthBeforedownload ;
                                    }
                                    catch { 
                                    
                                    }
                                }
                                sReturn = "There are " + iFilesLengthCurrentdownload + " files downloaded.";
                            }
                        }
                    }
                    else
                    {
                       
                        LogWrite("RegisterPatient ", sOutput + " " + dtEnc.Rows[i][0].ToString() + " " + DateTime.Now);
                        sReturn = "Error occured: Failed to download from cerner. Please refer the LogFile for detailed error response from cerner. ";
                       
                        if (sOutput.Contains("rs:RegistryError"))
                        {
                            XmlDocument xDocPatReg = new XmlDocument();
                            xDocPatReg.LoadXml(sOutput);
                            XmlNodeList sValue = xDocPatReg.GetElementsByTagName("rs:RegistryError"); 
                            foreach (XmlNode xCheck in sValue)
                            {
                                if (xCheck.Attributes.GetNamedItem("errorCode")!=null && xCheck.Attributes.GetNamedItem("errorCode").Value != "")
                                {
                                    sReturn = "Error code received when try to download the document from the cerner. Error Code : " + xCheck.Attributes.GetNamedItem("errorCode").Value;
                                }
                            }
                        }
                    }
                }
            }
            return sReturn;
        }

        public void RetrieveDocumentSetRequest(string sDocumentIDs, string sProviderID)
        {
            string sWebRequestService = ConfigurationSettings.AppSettings["RegisterDocumentSet"];
            LogWrite("RetrieveDocumentSetRequest", "Start");
            string sInput = File.ReadAllText(System.Configuration.ConfigurationSettings.AppSettings["MyMapDrive"] + "CernerRetrieveDocumentSetRequest.xml", Encoding.UTF8);
            sInput = sInput.Replace(":DocumentUniqueID", sDocumentIDs);
            LogWrite("RetrieveDocumentSetRequest", "sInput : " + sInput );
            LogWrite("RetrieveDocumentSetRequest", "sDocumentIDs : " + sDocumentIDs);

            string sOutput = InvokePayerServiceforCerner(sInput, sWebRequestService);
            LogWrite("RetrieveDocumentSetRequest", "sOutput : " + sOutput);
            ////Console.WriteLine(Environment.NewLine + sOutput);
            if (sOutput != null && sOutput != "")
            {
                sOutput = sOutput.Replace("\0", "");
                ////Console.WriteLine(Environment.NewLine + sOutput);
                if (sOutput.Contains("ResponseStatusType:Success"))
                {
                    LogWrite("RetrieveDocumentSetRequest ", "ResponseStatusType:Success : " + sOutput + " " + DateTime.Now);
                    //As per selva Encounter table data will not able to use .So instead on Appointment_provider_id I am using ClientSession.PhysicianId. 
                    //string sCCDXMLPhyPath = System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"] + "\\" + sProviderID + "\\" + sDocumentIDs + ".xml";// + sProviderID;
                    string sCCDXMLPhyPath = System.Configuration.ConfigurationSettings.AppSettings["phiMailDownloadDirectory"] + "\\" + ClientSession.PhysicianId + "\\" + sDocumentIDs + ".xml";// + sProviderID;
                    LogWrite("RetrieveDocumentSetRequest ", "DirectoryName : " + Path.GetDirectoryName(sCCDXMLPhyPath) + " " + DateTime.Now);
                    LogWrite("RetrieveDocumentSetRequest ", "CCD Saved File Path : " + sCCDXMLPhyPath + " " + DateTime.Now);                    
                    if (!Directory.Exists(Path.GetDirectoryName(sCCDXMLPhyPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(sCCDXMLPhyPath));
                        LogWrite("RetrieveDocumentSetRequest ", "DirectoryName created. ");
                    }
                    else
                    {
                        LogWrite("RetrieveDocumentSetRequest ", "DirectoryName exists. ");
                    }
                    string[] output = sOutput.Split(new string[] { "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" }, StringSplitOptions.None);
                    if (output.Length > 0)
                    {
                        LogWrite("RetrieveDocumentSetRequest ", "output doc length :" + output.Length);
                        for (int i = 0; i < output.Length; i++)
                        {
                            LogWrite("RetrieveDocumentSetRequest ", "For Loop : "+ i);
                            if (output[i].Contains("</ClinicalDocument>"))
                            {
                                try
                                {
                                    LogWrite("RetrieveDocumentSetRequest ","try : " + output[i]);
                                    XmlDocument doc = new XmlDocument();
                                    string[] outputFinal = output[i].Split(new string[] { "</ClinicalDocument>" }, StringSplitOptions.None);
                                    LogWrite("RetrieveDocumentSetRequest ", "Outputfinal : " + outputFinal[0]);
                                    doc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + outputFinal[0] + "</ClinicalDocument>");
                                    LogWrite("RetrieveDocumentSetRequest ", "Xml load successfully : " + i);
                                    doc.Save(sCCDXMLPhyPath);
                                    LogWrite("RetrieveDocumentSetRequest ", "Document saved successfully.");
                                }
                                catch(Exception ex)
                                {
                                    LogWrite("RetrieveDocumentSetRequest", "Exception InnerException: " + ex.InnerException + Environment.NewLine +
                                              "Exception StackTrace: " + ex.StackTrace + Environment.NewLine +
                                              "Exception Message: " + ex.Message + Environment.NewLine +
                                              "Exception Source: " + ex.Source + Environment.NewLine);
                                }
                            }
                        }
                    }

                    // /*/To decode Xml
                    // var base64EncodedBytes = System.Convert.FromBase64String(str);
                    // File.WriteAllText(sPrintPathName.Replace("CCD_","Decode_"), System.Text.Encoding.UTF8.GetString(base64EncodedBytes));

                }
                else
                {
                    LogWrite("RetrieveDocumentSetRequest ", "sOutPut Error: " + sOutput + " " + DateTime.Now);
                }
            }
            LogWrite("RetrieveDocumentSetRequest", "Stop");
        }
       

        public string ProvideandRegisterDocumentSet(string sEncID,string sFileCCDPath)
        {
            //string sCurrentArrivalDate=Convert.ToDateTime(DateTime.UtcNow.AddDays(-7)).ToString("yyyy-MM-dd");
            //string Provider_Id = ConfigurationSettings.AppSettings["Provider_Id"];
            //string query = "select e.human_id,e.encounter_id,e.Appointment_Provider_ID  from encounter e, wf_object w where  date(w.Current_Arrival_Time)<='" + sCurrentArrivalDate + "' and w.obj_system_id=e.encounter_id and w.current_process='REVIEW_CODING' and e.Appointment_Provider_ID in ('" + Provider_Id + "') union all " +-
            //   "select ea.human_id,ea.encounter_id,ea.Appointment_Provider_ID  from encounter_arc ea, wf_object_arc wa where  date(wa.Current_Arrival_Time)<='" + sCurrentArrivalDate + "' and wa.obj_system_id=ea.encounter_id and wa.current_process='REVIEW_CODING' and ea.Appointment_Provider_ID in ('" + Provider_Id + "')";
            //DataSet dsReturn = DBConnector.ReadData(query);
            //DataTable dtEnc = dsReturn.Tables[0];

            //string sCerner_CCDXML = ConfigurationSettings.AppSettings["Cerner_CCDXML"];
            //string sCerner_CCDXMLSend = ConfigurationSettings.AppSettings["Cerner_CCDXMLSend"];
            //DirectoryInfo dir = new DirectoryInfo(sCerner_CCDXML);
            //FileInfo[] sCCDFiles = dir.GetFiles("*.xml");
            //if (sCCDFiles.Length == 0)
            //{
            //    LogWrite("ProvideandRegisterDocumentSet", "No files in CCD file folder.");
            //    ////Console.ForegroundColor = ConsoleColor.Red;
            //    //Console.WriteLine("No files in CCD file folder.");
            //    //Console.ForegroundColor = ConsoleColor.White;
            //    return;
            //}
            //bool bEncCheck = false;
            //for (int i = 0; i < sCCDFiles.Length; i++)
            //{
            //    string sEncounterID = sCCDFiles[i].Name.Replace("CCD_", "").Replace(".xml", "");
            //    if (sEncounterID == sEncID)
            //    {
            //        bEncCheck = true;
            //        break;
            //    }

            //}
           // if (bEncCheck == false)
            string sReturn = string.Empty;
            string sCerner = string.Empty;
            if (sFileCCDPath == "" || !File.Exists(sFileCCDPath))
            {
                LogWrite("ProvideandRegisterDocumentSet", "No files in CCD file folder.");
                sReturn = "Please regenerate the CCD file.";
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine("No files in CCD file folder.");
                //Console.ForegroundColor = ConsoleColor.White;
                return sReturn;
            }
            string sWebRequestService = ConfigurationSettings.AppSettings["RegisterDocumentSet"];
            //for (int i = 0; i < sCCDFiles.Length; i++)
            //{
                //Generate the CCD document for the current encounter ID and convert it into base64Encrypted format
                string sFileName = sFileCCDPath;//sCCDFiles[i].Name;
                string sEncounterID = sEncID; //sCCDFiles[i].Name.Replace("CCD_", "").Replace(".xml", "");
                //if (sEncounterID == sEncID)
                //{
                string queryhuman = "select h.human_id,h.First_Name,h.Last_Name,h.Sex,h.Birth_Date,h.Street_Address1,h.City,h.State,h.ZipCode,e.encounter_id,h.Cerner_Universal_Patient_ID from human h, encounter e where h.human_id=e.human_id  and e.encounter_id= '" + sEncounterID + "' union all " +
                                        "select h.human_id,h.First_Name,h.Last_Name,h.Sex,h.Birth_Date,h.Street_Address1,h.City,h.State,h.ZipCode,ea.encounter_id,h.Cerner_Universal_Patient_ID from human h, encounter_arc ea where h.human_id=ea.human_id  and ea.encounter_id= '" + sEncounterID + "'";
                    DataSet dsReturnhuman = DBConnector.ReadData(queryhuman);
                    DataTable dtHuman = dsReturnhuman.Tables[0];

                    string sInput = File.ReadAllText(ConfigurationSettings.AppSettings["MyMapDrive"] + "CernerProvideAndRegisterDocumentSet.xml", Encoding.UTF8);
                    sInput = sInput.Replace(":CreationTime", DateTime.Now.ToString("yyyyMMdd"));
                    sInput = sInput.Replace(":CapellaHumanID", dtHuman.Rows[0][0].ToString());
                    sInput = sInput.Replace(":FirstName", dtHuman.Rows[0][1].ToString());
                    sInput = sInput.Replace(":LastName", dtHuman.Rows[0][2].ToString());
                    sInput = sInput.Replace(":Gender", dtHuman.Rows[0][3].ToString().Substring(0, 1).ToUpper());
                    sInput = sInput.Replace(":DOB", Convert.ToDateTime(dtHuman.Rows[0][4]).ToString("yyyyMMdd"));
                    sInput = sInput.Replace(":StreetAddress", dtHuman.Rows[0][5].ToString());
                    sInput = sInput.Replace(":City", dtHuman.Rows[0][6].ToString());
                    sInput = sInput.Replace(":State", dtHuman.Rows[0][7].ToString());
                    sInput = sInput.Replace(":ZipCode", dtHuman.Rows[0][8].ToString());
                    sInput = sInput.Replace(":CurrentDateTime", DateTime.Now.ToString("yyyyMMddhhmmss").ToString());
                    sInput = sInput.Replace(":EncounterID", dtHuman.Rows[0][9].ToString());
                    sCerner = dtHuman.Rows[0][10].ToString();
                    
                    //string readText = File.ReadAllText(Path.Combine(sCerner_CCDXML, sFileName));
                    string readText = File.ReadAllText(sFileName);
                    //var encoding = new UnicodeEncoding();
                    //string sBase64CCD = Convert.ToBase64String(encoding.GetBytes(readText));
                    byte[] encData_byte = new byte[readText.Length];
                    encData_byte = System.Text.Encoding.UTF8.GetBytes(readText);
                    string sBase64CCD = Convert.ToBase64String(encData_byte);
                    sInput = sInput.Replace(":Based64EncryptedvalueofCCDDocument", sBase64CCD);

                    // sInput = File.ReadAllText(ConfigurationSettings.AppSettings["MyMapDrive"] + "ProvideAndRegisterDocumentSet-bRequest_sample.xml", Encoding.UTF8);


                    // /*/To decode Xml
                    // var base64EncodedBytes = System.Convert.FromBase64String(str);
                    // File.WriteAllText(sPrintPathName.Replace("CCD_","Decode_"), System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
                    LogWrite("sInput : ", sInput);
                    //Invoke API
                    string sOutput = "";
                    sOutput = InvokePayerServiceforCerner(sInput, sWebRequestService);
                    //Console.WriteLine(Environment.NewLine + sOutput);
                    // //Console.Read();
                    LogWrite("sOutput : ", sOutput);

                    if (sOutput != null && sOutput != "")
                    {
                        //XmlDocument xDocPatReg = new XmlDocument();
                        //xDocPatReg.LoadXml(sOutput);
                        //string sAcknowledgement = xDocPatReg.GetElementsByTagName("urn:acknowledgement")[0].ChildNodes[0].Attributes.GetNamedItem("code").Value;
                        //if (sAcknowledgement == "CA")
                        //{
                        //    string sDocUniID = xDocPatReg.GetElementsByTagName("urn:acknowledgement")[0].ChildNodes[0].Attributes.GetNamedItem("code").Value;

                        if (sOutput.Contains("ResponseStatusType:Success"))
                        {
                            //update Cerner_Document_Unique_ID
                            // string updateQuery = "update encounter set Cerner_Document_Unique_ID='" + sDocUniID + "' where ecounter_id='" + sEncounterID + "'";
                            //DataSet dsReturnupdate = DBConnector.ReadData(updateQuery);
                            sReturn = "Successfully submitted the document to the Cerner. For the cerner patient UID: " + sCerner;
                            //Move the CCD file to send folder
                            LogWrite("", "2.16.840.1.113883.3.8375." + dtHuman.Rows[0][0].ToString() + "." + dtHuman.Rows[0][9].ToString());
                            //if (File.Exists(Path.Combine(sCerner_CCDXML, sFileName)))
                            //{
                            //    if (!Directory.Exists(Path.Combine(sCerner_CCDXMLSend)))
                            //    {
                            //        Directory.CreateDirectory(Path.Combine(sCerner_CCDXMLSend));
                            //    }
                            //    File.Move(Path.Combine(sCerner_CCDXML, sFileName), Path.Combine(sCerner_CCDXMLSend, sFileName));
                            //}

                        }
                        else
                        {
                            if (sOutput.Contains("XDSDuplicateUniqueIdInRegistry"))
                            {
                                sReturn = "Error occured: XDSDuplicateUniqueIdInRegistry. Failed to submit the document. For the cerner patient UID: " + sCerner;
                            }
                            else
                            {
                                sReturn = "Error occured: Failed to submit the document. Please refer the LogFile for response from cerner. For the cerner patient UID: " + sCerner;
                            }
                            LogWrite("RegisterPatient ", sOutput + " " + dtHuman.Rows[0][0].ToString() + " " + DateTime.Now);
                        }
                    }
                //}
            //}
        return sReturn;
        }

        public string LogWrite(string sAPIName, string logMessage)
        {
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.Write(Environment.NewLine + logMessage + Environment.NewLine);

            string sLogFileName = ConfigurationManager.AppSettings["LogTxt"];
            if (!File.Exists(sLogFileName))
            {
                File.Create(sLogFileName);
            }
            try
            {
                using (StreamWriter txtWriter = File.AppendText(sLogFileName))
                {

                    txtWriter.WriteLine(Environment.NewLine);
                    txtWriter.WriteLine("-------------------------------");
                    txtWriter.WriteLine("{0}", sAPIName + " - " + DateTime.Now + " - " + logMessage);
                    txtWriter.WriteLine("-------------------------------");
                }
            }
            catch (Exception ex)
            {
                LogWrite("LogFunction", "Exception InnerException: " + ex.InnerException + Environment.NewLine +
                                              "Exception StackTrace: " + ex.StackTrace + Environment.NewLine +
                                              "Exception Message: " + ex.Message + Environment.NewLine +
                                              "Exception Source: " + ex.Source + Environment.NewLine);
            }
            return "";
        }



        public static class DBConnector
        {
            static MySqlDataAdapter MyDataAdap = null;
            private static string ReadConnection()
            {
                string ConnectionData;
                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                return ConnectionData;
            }
            public static DataSet ReadData(string Query)
            {
                DataSet dsReturn = new DataSet();
                MyDataAdap = new MySqlDataAdapter(Query, ReadConnection());
                MyDataAdap.SelectCommand.CommandTimeout = 300;
                MyDataAdap.Fill(dsReturn);
                return dsReturn;
            }
            public static string createFile()
            {
                string OutputDir = ConfigurationManager.AppSettings["Output"];
                if (!Directory.Exists(OutputDir))
                    Directory.CreateDirectory(OutputDir);
                return OutputDir;
            }
        }

        public HttpWebRequest PayerWebRequest()
        {
            //Making Web Request    
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://wsd.officeally.com/TransactionService/rtx.svc");
            //SOAPAction    
            Req.Headers.Add(@"SOAPAction:RealTimeTransaction");
            //Content_type    
            //Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.ContentType = " application/soap+xml;action=\"RealTimeTransaction\"";
            Req.Accept = "gzip,deflate";
            //HTTP method    
            Req.Method = "POST";
            //return HttpWebRequest    
            return Req;
        }

        public HttpWebRequest CernerWebRequest(string sWebRequestService)
        {
            //Making Web Request  
            ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)768 | (SecurityProtocolType)3072;F:\Acurus\2019\June\Cerner project\Cerner\Cerner\Utils\SOAPUtils.cs
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11;

            //HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://pvhcertsync.hiedirectconnect.org:9052/ihe/services/PIXManager_Service/");
            //// HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://pvhcertsync.hiedirectconnect.org:9050/ihe/services/xdsregistryb/");

            ////HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://pvhcertsync.hiedirectconnect.org:9051/ihe/services/xdsrepositoryb/");
            string sReq = @"" + sWebRequestService + "";
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(sReq);




            Req.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            Req.Method = "PUT"; // Post method
            Req.ContentType = "text/xml";// content type
            Req.KeepAlive = false;
            Req.ProtocolVersion = HttpVersion.Version10;

            X509Certificate2 cert = new X509Certificate2(System.Configuration.ConfigurationSettings.AppSettings["MyMapDrive"] + "hiedirectconnect_chain\\cert.pfx", "acurus123");
            Req.ClientCertificates.Add(cert);
            cert = new X509Certificate2(System.Configuration.ConfigurationSettings.AppSettings["MyMapDrive"] + "hiedirectconnect_chain\\entrust_root");
            Req.ClientCertificates.Add(cert);
            cert = new X509Certificate2(System.Configuration.ConfigurationSettings.AppSettings["MyMapDrive"] + "hiedirectconnect_chain\\G2_entrust");
            Req.ClientCertificates.Add(cert);
            cert = new X509Certificate2(System.Configuration.ConfigurationSettings.AppSettings["MyMapDrive"] + "hiedirectconnect_chain\\L1K_entrust");
            Req.ClientCertificates.Add(cert);
            cert = new X509Certificate2(System.Configuration.ConfigurationSettings.AppSettings["MyMapDrive"] + "hiedirectconnect_chain\\hiedirectconnect.org");
            Req.ClientCertificates.Add(cert);

            //        Req.ServerCertificateValidationCallback +=
            //delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //                        System.Security.Cryptography.X509Certificates.X509Chain chain,
            //                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //{
            //    return true; // **** Always accept
            //};

            ////SOAPAction    
            //Req.Headers.Add(@"SOAPAction:RealTimeTransaction");
            ////Content_type    
            ////Req.ContentType = "text/xml;charset=\"utf-8\"";
            //Req.ContentType = " application/soap+xml;action=\"RealTimeTransaction\"";
            //Req.Accept = "gzip,deflate";
            ////HTTP method    
            //Req.Method = "POST";
            //return HttpWebRequest    
            return Req;
        }

        public string InvokePayerServiceforCerner(string sSoapRequest, string sWebRequestService)
        {

            //ServicePointManager.ServerCertificateValidationCallback =
            //   delegate(
            //       object s,
            //       X509Certificate certificate,
            //       X509Chain chain,
            //       SslPolicyErrors sslPolicyErrors
            //   )
            //   {
            //       return true;
            //   };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Calling CreateSOAPWebRequest method  
            HttpWebRequest request = CernerWebRequest(sWebRequestService);
            XmlDocument SOAPReqBody = new XmlDocument();
            string sInput = @"<?xml version=""1.0"" encoding=""utf-8""?>" + sSoapRequest;
            //SOAP Body Request  
            SOAPReqBody.LoadXml(sInput);
            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            try
            {
                //Geting response from request  
                using (WebResponse Serviceres = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                    {
                        //reading stream  
                        string ServiceResult = rd.ReadToEnd();
                        //writting stream result on console  
                        return ServiceResult;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite("InvokePayerService", "Exception InnerException: " + ex.InnerException + Environment.NewLine +
                                            "Exception StackTrace: " + ex.StackTrace + Environment.NewLine +
                                            "Exception Message: " + ex.Message + Environment.NewLine +
                                            "Exception Source: " + ex.Source + Environment.NewLine);
                //Console.Write(ex.Message);
                //Console.ReadLine();
                return string.Empty;
            }
        }

        public HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request    
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"http://localhost:49808/Service.asmx");
            //SOAPAction    
            Req.Headers.Add(@"SOAPAction:http://tempuri.org/addition");
            //Content_type    
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method    
            Req.Method = "POST";
            //return HttpWebRequest    
            return Req;
        }

        public string InvokeService(int a, int b)
        {
            //Calling CreateSOAPWebRequest method  
            HttpWebRequest request = CreateSOAPWebRequest();

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request  
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  
             <soap:Body>  
                <addition xmlns=""http://tempuri.org/"">  
                  <a>" + a + @"</a>  
                  <b>" + b + @"</b>  
                </addition>  
              </soap:Body>  
            </soap:Envelope>");


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request  
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream  
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console  
                    return ServiceResult;

                }
            }
        }
    }
    public class SoapRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SenderID { get; set; }
        public string RecieverID { get; set; }
        public string Payload { get; set; }
    }
}