//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Acurus.Capella.Core.DomainObjects;
//using Acurus.Capella.Core.DTO;
//using Acurus.Capella.DataAccess.ManagerObjects;
//using System.IO;
//using System.Xml;
//using System.Reflection;
//using System.Net;
//using NHibernate;
//using NHibernate.Criterion;

//namespace Acurus.Capella.DataAccess
//{
//    public partial class RCopiaTransactionManager
//    {
//        XmlDocument xmlDoc = new XmlDocument();
//        XmlWriterSettings wSettings = new XmlWriterSettings();
//        MemoryStream ms;
//        XmlWriter xmlWriter;
//        IList<Rcopia_Settings> ilstRcopSett;
//        Rcopia_Settings objRcopSettings;

//        RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();

//        public string creteSendPatientXml(ulong MyhumanID)
//        {

//            IList<Human> hnList = new List<Human>();
//            Human HumanRecord;
//            HumanManager hnMngr = new HumanManager();
//            VitalsManager vitalMngr = new VitalsManager();
//            PatientResultsDTO objVitalDTO = new PatientResultsDTO();
//            IList<PatientResults> vitalList = new List<PatientResults>();
//            PatientResults vitalRecord;
//            IList<Encounter> EncList = new List<Encounter>();
//            EncounterManager EncMngr = new EncounterManager();
//            string Weight = string.Empty;
//            string Height = string.Empty;

//            hnList = hnMngr.GetPatientDetailsUsingPatientInformattion(MyhumanID);
//            HumanRecord = hnList[0];

//            objVitalDTO = vitalMngr.GetPastVitalDetailsByPatientforRcopia(MyhumanID, 1, 10000);
//            vitalList = objVitalDTO.VitalsList;

//            EncList = EncMngr.GetEncounterUsingHumanID(HumanRecord.Id);
//            DateTime dtpDateOfService = new DateTime();
//            if (vitalList.Count > 0)
//            {
//                ulong maxId = Convert.ToUInt64((from obj in vitalList select obj.Vitals_Group_ID).Max());
//                IList<PatientResults> list = (from obj in vitalList where obj.Vitals_Group_ID == maxId && (obj.Loinc_Observation.ToUpper() == "HEIGHT" || obj.Loinc_Observation.ToUpper() == "WEIGHT") select obj).ToList<PatientResults>();
//                if (list != null)
//                {
//                    if (list.Count > 0)
//                    {
//                        Height = list.First().Value;
//                        Weight = list.Last().Value;
//                    }
//                }
//            }


//            FillRequiredInfo("send_patient");
//            if (objRcopSettings == null)
//            {
//                return string.Empty;
//            }
//            xmlWriter.WriteElementString("Synchronous", objRcopSettings.Synchronous);
//            xmlWriter.WriteElementString("CheckEligibility", objRcopSettings.Check_Eligibilityt);

//            xmlWriter.WriteStartElement("PatientList");
//            xmlWriter.WriteStartElement("Patient");

//            PropertyInfo[] propertyInfos = typeof(Human).GetProperties();
//            foreach (PropertyInfo propertyInfo in propertyInfos)
//            {
//                string sreturn = FillObject(propertyInfo.Name);
//                if (sreturn != string.Empty)
//                {
//                    PropertyInfo o = HumanRecord.GetType().GetProperty(propertyInfo.Name);
//                    var v = (o.GetValue(HumanRecord, null));

//                    if (sreturn == "DOB")
//                    {
//                        xmlWriter.WriteElementString(sreturn, ((DateTime)v).ToString("MM/dd/yyyy").Replace("-", "/"));
//                    }
//                    else
//                    {
//                        xmlWriter.WriteElementString(sreturn, v.ToString());
//                    }
//                }
//            }
//            xmlWriter.WriteElementString("Weight", ConvertLbsToKg(Weight));
//            xmlWriter.WriteElementString("WeightUnit", "kg");
//            //xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
//            if (Height.Contains("'") == true && Height.Contains("''") == true)
//            {
//                // string[] ChargeLineItemIDs = hdnChargeLineItemID.Value.Split(',');
//                //int ss= Height.Remove( Height.IndexOf("'"));
//                // string [] resultHeight =Height.IndexOf("'");
//                //Height = ConvertFeetInchToInch(Height.Remove(Height.IndexOf("'")), Height.Remove(0,Height.IndexOf("'")));

//                string[] resultHeight = (Height.ToString()).Split('\''); //added by balaji
//                Height = ConvertFeetInchToInch(resultHeight[0], resultHeight[1]);
//                xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
//            }
//            else
//                xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
//            xmlWriter.WriteElementString("HeightUnit", "cm");
//            if (EncList.Count > 0 && EncList.Count != 1)
//            {
//                dtpDateOfService = EncList[0].Date_of_Service.Date;
//                xmlWriter.WriteElementString("LastVisitDate", dtpDateOfService.ToString("MM/dd/yyyy").Replace("-", "/"));
//            }
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            // Close the document
//            xmlWriter.WriteEndDocument();
//            // Flush the write
//            xmlWriter.Flush();

//            Byte[] buffer = new Byte[ms.Length];
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

//            return xmlOutput;
//        }



//        public void FillRequiredInfo(string sXMLName)
//        {

//            ms = new MemoryStream();
//            wSettings.Indent = true;
//            xmlWriter = XmlWriter.Create(ms, wSettings);
//            xmlWriter.WriteStartDocument();

//            Rcopia_SettingsManager rcopiaMngr = new Rcopia_SettingsManager();
//            ilstRcopSett = rcopiaMngr.GetRcopia_Settings();
//            if (ilstRcopSett.Count > 0)
//            {
//                objRcopSettings = (from g in ilstRcopSett where g.Command == sXMLName select g).ToList<Rcopia_Settings>()[0];
//                if (objRcopSettings == null)
//                {
//                    return;
//                }
//                xmlWriter.WriteStartElement("RCExtRequest");
//                xmlWriter.WriteAttributeString("version", "2.19");//changed version from 2.13 to 2.19 as per Dr.First's instructions[RajeevMotwani] as Assessment data were not getting uploaded(under Problem section) to Dr.First
//                xmlWriter.WriteStartElement("TraceInformation");
//                xmlWriter.WriteElementString("RequestMessageID", objRcopSettings.Request_Message_ID);
//                xmlWriter.WriteEndElement();

//                xmlWriter.WriteStartElement("Caller");
//                PropertyInfo[] propertyInfoColl = typeof(Rcopia_Settings).GetProperties();
//                foreach (PropertyInfo propertyInfo in propertyInfoColl)
//                {
//                    string sreturn = FillObject(propertyInfo.Name);
//                    if (sreturn != string.Empty)
//                    {
//                        PropertyInfo o = objRcopSettings.GetType().GetProperty(propertyInfo.Name);
//                        var v = (o.GetValue(objRcopSettings, null));
//                        xmlWriter.WriteElementString(sreturn, v.ToString());
//                    }
//                }
//                xmlWriter.WriteEndElement();
//                xmlWriter.WriteElementString("SystemName", objRcopSettings.System_Name);
//                xmlWriter.WriteElementString("RcopiaPracticeUsername", objRcopSettings.Rcopia_Practice_User_name);
//                xmlWriter.WriteStartElement("Request");
//                xmlWriter.WriteElementString("Command", objRcopSettings.Command);
//            }

//        }


//        public string FillObject(string sPropname)
//        {
//            string ReturnString = string.Empty;
//            switch (sPropname)
//            {
//                case "Id":
//                    ReturnString = "ExternalID";
//                    break;
//                case "Street_Address1":
//                    ReturnString = "Address1";
//                    break;
//                case "Street_Address2":
//                    ReturnString = "Address2";
//                    break;
//                case "City":
//                    ReturnString = "City";
//                    break;
//                case "Birth_Date":
//                    ReturnString = "DOB";
//                    break;
//                case "First_Name":
//                    ReturnString = "FirstName";
//                    break;
//                case "Last_Name":
//                    ReturnString = "LastName";
//                    break;
//                case "MI":
//                    ReturnString = "MiddleName";
//                    break;
//                case "Sex":
//                    ReturnString = "Sex";
//                    break;
//                case "Home_Phone_No":
//                    ReturnString = "HomePhone";
//                    break;
//                case "SSN":
//                    ReturnString = "SSN";
//                    break;
//                case "State":
//                    ReturnString = "State";
//                    break;
//                case "ZipCode":
//                    ReturnString = "Zip";
//                    break;
//                case "Vendor_Name":
//                    ReturnString = "VendorName";
//                    break;
//                case "Vendor_Password":
//                    ReturnString = "VendorPassword";
//                    break;
//                case "Application":
//                    ReturnString = "Application";
//                    break;
//                case "Rcopia_Version":
//                    ReturnString = "Version";
//                    break;
//                case "Practice_Name":
//                    ReturnString = "PracticeName";
//                    break;
//                case "Station":
//                    ReturnString = "Station";
//                    break;

//            }
//            return ReturnString;
//        }

//        public void SendPatientToRCopia(ulong ulHumanID, string sMacAddress)
//        {
//            string xmlDoc = string.Empty;
//            string UploadAddress = string.Empty;

//            xmlDoc = creteSendPatientXml(ulHumanID);

//            //Upload XML to RCopia Server
//            string sContent = xmlDoc.ToString();
//            sContent = sContent.Trim();
//            sContent = sContent.Replace("\n", "");

//            UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//            if (UploadAddress != null && UploadAddress != string.Empty)
//            {
//                if (rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1) == null)
//                {
//                    //Muthu
//                    HumanManager objmngr = new HumanManager();
//                    IList<Human> ilsthuman = new List<Human>();
//                    ilsthuman = objmngr.GetPatientDetailsUsingPatientInformattion(ulHumanID);
//                    ilsthuman[0].Is_Sent_To_Rcopia = "N";
//                    objmngr.UpdateFailedRCopiaHuman(ilsthuman[0], sMacAddress);
//                }
//            }
//        }



//        public void SendProblemToRCopia(IList<ulong> listofinsertIDs, string sSource, bool isDelete, IList<Assessment> ilstAssessment)
//        {
//            string xmlDoc = string.Empty;
//            string UploadAddress = string.Empty;

//            xmlDoc = CreateSendproblemXML(listofinsertIDs, sSource, isDelete, ilstAssessment);

//            //Upload XML to RCopia Server
//            string sContent = xmlDoc.ToString();
//            sContent = sContent.Trim();
//            sContent = sContent.Replace("\n", "");
//            UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//            if (UploadAddress != null && UploadAddress != string.Empty)
//            {
//                rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//            }
//        }

//        public string CreateSendproblemXML(IList<ulong> listofinsertIDs, string sSource, bool isDelete, IList<Assessment> ilstassement)
//        {

//            FillRequiredInfo("send_problem");
//            if (objRcopSettings == null)
//            {
//                return string.Empty;
//            }
//            xmlWriter.WriteStartElement("ProblemList");

//            IList<ProblemList> ilstProblem = new List<ProblemList>();
//            Assessment objAssesment = new Assessment();
//            AssessmentManager objAssmngr = new AssessmentManager();

//            if (sSource.ToUpper() == "PROBLEMLIST")
//            {
//                ProblemListManager objmngr = new ProblemListManager();
//                ilstProblem = objmngr.GetProblemListUsingProblemListId(listofinsertIDs[0]);
//            }
//            else if (sSource.ToUpper() == "ASSESMENT")
//            {
//                if (ilstassement != null && ilstassement.Count > 0)
//                {

//                    for (int p = 0; p < ilstassement.Count; p++)
//                    {
//                        objAssesment = ilstassement[p];
//                        Fillassessment(objAssesment, isDelete);
//                    }
//                }
//                else if (sSource.ToUpper() == "ASSESMENT" && ilstassement == null)
//                {
//                    if (listofinsertIDs.Count > 0)
//                    {
//                        for (int u = 0; u < listofinsertIDs.Count; u++)
//                        {
//                            objAssesment = objAssmngr.GetAssesmentUsingAssesmentId(listofinsertIDs[u]);
//                            Fillassessment(objAssesment, isDelete);
//                        }
//                    }
//                }
//            }
//            if (sSource.ToUpper() == "PROBLEMLIST")
//            {
//                if (ilstProblem.Count > 0)
//                {
//                    xmlWriter.WriteStartElement("Problem");
//                    if (ilstProblem[0].Is_Active.ToLower() == "y" && ilstProblem[0].Status.ToUpper() == "ACTIVE")
//                        xmlWriter.WriteElementString("Deleted", "n");
//                    else
//                        xmlWriter.WriteElementString("Deleted", "y");
//                    xmlWriter.WriteStartElement("Status");
//                    if (ilstProblem[0].Status.ToUpper() == "ACTIVE")
//                    {
//                        if (ilstProblem[0].Is_Active.ToLower() == "y" && ilstProblem[0].Status.ToUpper() == "ACTIVE")
//                        {
//                            xmlWriter.WriteElementString("Active", "y");
//                        }
//                        else
//                        {
//                            xmlWriter.WriteElementString("Inactive", "y");
//                        }
//                    }
//                    else if (ilstProblem[0].Status.ToUpper() == "INACTIVE")
//                    {
//                        xmlWriter.WriteElementString("Inactive", "y");
//                    }
//                    else if (ilstProblem[0].Status.ToUpper() == "RESOLVED")
//                    {
//                        xmlWriter.WriteElementString("Resolved", "y");
//                    }
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteElementString("RcopiaID", string.Empty);
//                    xmlWriter.WriteElementString("ExternalID", "Prob" + ilstProblem[0].Id.ToString());
//                    xmlWriter.WriteStartElement("Patient");
//                    xmlWriter.WriteElementString("RcopiaID", string.Empty);
//                    xmlWriter.WriteElementString("ExternalID", ilstProblem[0].Human_ID.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteElementString("Code", ilstProblem[0].ICD);
//                    xmlWriter.WriteElementString("Description", ilstProblem[0].Problem_Description);
//                    xmlWriter.WriteEndElement();
//                }
//            }
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndDocument();
//            xmlWriter.Flush();
//            Byte[] buffer = new Byte[ms.Length];
//            buffer.DefaultIfEmpty();
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

//            return xmlOutput;

//        }

//        public void Fillassessment(Assessment objAssesment, bool isDelete)
//        {
//            xmlWriter.WriteStartElement("Problem");
//            if (isDelete == true)
//                xmlWriter.WriteElementString("Deleted", "y");
//            else
//                xmlWriter.WriteElementString("Deleted", "n");
//            xmlWriter.WriteStartElement("Status");
//            if (isDelete == false)
//            {
//                xmlWriter.WriteElementString("Active", "y");
//            }
//            else
//            {
//                xmlWriter.WriteElementString("Inactive", "y");
//            }
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", "ASS" + objAssesment.Id.ToString());
//            xmlWriter.WriteStartElement("Patient");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", objAssesment.Human_ID.ToString());
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteStartElement("ICD10");
//            xmlWriter.WriteElementString("Code", objAssesment.ICD);
//            xmlWriter.WriteElementString("Description", objAssesment.ICD_Description);
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//        }

//        public string ConvertInchesToCM(string s)
//        {
//            if (s != string.Empty)
//            {
//                decimal cmValue = decimal.Round((Convert.ToDecimal(s) * 2.54m), 2);
//                return cmValue.ToString();
//            }
//            else
//                return string.Empty;
//        }
//        public string ConvertFeetInchToInch(string s1, string s2)
//        {
//            if (s1 == string.Empty)
//            {
//                return string.Empty;
//            }
//            decimal inValue = 0;
//            if (s2 != string.Empty)
//                inValue = decimal.Round((Convert.ToDecimal(s1) * 12m) + Convert.ToDecimal(s2), 2);
//            else
//                inValue = decimal.Round((Convert.ToDecimal(s1) * 12m), 2);
//            if (inValue != 0m)
//                return inValue.ToString();
//            else
//                return string.Empty;

//        }
//        public string ConvertLbsToKg(string s)
//        {
//            if (s != string.Empty)
//            {
//                decimal kgValue = decimal.Round(Convert.ToDecimal(s) / 2.2m, 2);
//                return kgValue.ToString();
//            }
//            else
//            {
//                return string.Empty;
//            }
//        }
//        public void SendBulkUploadPatientToRCopia(ulong ulHumanID, string sMacAddress)
//        {
//            string xmlDoc = string.Empty;
//            string UploadAddress = string.Empty;
//            HumanManager HumanMgr = new HumanManager();
//            string sFileName = System.Configuration.ConfigurationSettings.AppSettings["XMLFileName"] + DateTime.Now.ToString("dd-MMM-yyyy");

//            int i = 1, Count = 0;
//            while (i >= 0)
//            {
//                Count = (i - 1) * 500;
//                IList<Human> HumanList = HumanMgr.GetPatientListForRcopia(Count, 500);
//                creteSendPatientXmlPost(HumanList, sFileName + i + ".xml");
//                if (HumanList.Count == 500)
//                    i++;
//                else
//                    break;

//            }
//        }
//        public void creteSendPatientXmlPost(IList<Human> HumanList, string sFileName)
//        {

//            string xmlOutput = string.Empty;

//            Human HumanRecord;
//            VitalsManager vitalMngr = new VitalsManager();
//            PatientResultsDTO objVitalDTO = new PatientResultsDTO();
//            IList<PatientResults> vitalList = new List<PatientResults>();
//            PatientResults vitalRecord;
//            string Weight = string.Empty;
//            string Height = string.Empty;
//            xmlWriter = new XmlTextWriter(sFileName, Encoding.UTF8);
//            FillRequiredInfo("send_patient");
//            if (objRcopSettings == null)
//            {
//                return;
//            }
//            xmlWriter.WriteElementString("Synchronous", objRcopSettings.Synchronous);
//            xmlWriter.WriteElementString("CheckEligibility", objRcopSettings.Check_Eligibilityt);

//            xmlWriter.WriteStartElement("PatientList");
//            if (HumanList != null)
//            {
//                for (int i = 0; i < HumanList.Count; i++)
//                {
//                    HumanRecord = new Human();
//                    HumanRecord = HumanList[i];
//                    objVitalDTO = vitalMngr.GetPastVitalDetailsByPatientforRcopia(HumanRecord.Id, 1, 10000);
//                    vitalList = objVitalDTO.VitalsList;
//                    if (vitalList.Count > 0)
//                    {
//                        ulong maxId = Convert.ToUInt64((from obj in vitalList select obj.Vitals_Group_ID).Max());
//                        IList<PatientResults> list = (from obj in vitalList where obj.Vitals_Group_ID == maxId && (obj.Loinc_Observation.ToUpper() == "HEIGHT" || obj.Loinc_Observation.ToUpper() == "WEIGHT") select obj).ToList<PatientResults>();
//                        if (list != null)
//                        {
//                            if (list.Count > 0)
//                            {
//                                Height = list.First().Value;
//                                Weight = list.Last().Value;
//                            }
//                        }
//                    }
//                    xmlWriter.WriteStartElement("Patient");
//                    PropertyInfo[] propertyInfos = typeof(Human).GetProperties();
//                    foreach (PropertyInfo propertyInfo in propertyInfos)
//                    {
//                        string sreturn = FillObject(propertyInfo.Name);
//                        if (sreturn != string.Empty)
//                        {
//                            PropertyInfo o = HumanRecord.GetType().GetProperty(propertyInfo.Name);
//                            var v = (o.GetValue(HumanRecord, null));
//                            xmlWriter.WriteElementString(sreturn, v.ToString());
//                        }
//                    }
//                    xmlWriter.WriteElementString("Weight", ConvertLbsToKg(Weight));
//                    xmlWriter.WriteElementString("WeightUnit", "kg");
//                    if (Height.Contains("'") == true && Height.Contains("''") == true)
//                    {

//                        // Height = ConvertFeetInchToInch(Height);
//                        //xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
//                        string[] resultHeight = (Height.ToString()).Split('\'');
//                        Height = ConvertFeetInchToInch(resultHeight[0], resultHeight[1]);
//                        xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
//                    }
//                    else
//                        xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
//                    xmlWriter.WriteElementString("HeightUnit", "cm");
//                    xmlWriter.WriteEndElement();

//                }

//            }
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            // Close the document
//            xmlWriter.WriteEndDocument();
//            // Flush the write
//            xmlWriter.Flush();
//        }



//        public void SendMedicationToRCopia(Rcopia_Medication_Temp objMed, string sUserName)
//        {
//            RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();
//            string xmlDoc = string.Empty;
//            string UploadAddress = string.Empty;
//            string sOutputXML = string.Empty;
//            xmlDoc = CreateSendMedicationXML(objMed, sUserName);

//            //Upload XML to RCopia Server
//            string sContent = xmlDoc.ToString();
//            sContent = sContent.Trim();
//            sContent = sContent.Replace("\n", "");
//            UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//            if (UploadAddress != null && UploadAddress != string.Empty)
//            {
//                sOutputXML = rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//                //sOutputXML = sOutputXML.Replace("<Command>send_", "<Command>update_");
//                //rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, string.Empty, DateTime.Now, "", 0);
//            }
//        }

//        public string CreateSendMedicationXML(Rcopia_Medication_Temp objMed, string sUserName)
//        {

//            FillRequiredInfo("send_medication");
//            if (objRcopSettings == null)
//            {
//                return string.Empty;
//            }
//            xmlWriter.WriteStartElement("MedicationList");
//            xmlWriter.WriteStartElement("Medication");
//            xmlWriter.WriteElementString("Deleted", objMed.Deleted);

//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", string.Empty);

//            xmlWriter.WriteStartElement("Patient");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", objMed.Human_ID.ToString());
//            xmlWriter.WriteEndElement();

//            xmlWriter.WriteStartElement("Provider");
//            xmlWriter.WriteElementString("Username", sUserName);
//            xmlWriter.WriteEndElement();

//            xmlWriter.WriteStartElement("Sig");
//            xmlWriter.WriteStartElement("Drug");
//            xmlWriter.WriteElementString("NDCID", objMed.NDC_ID);
//            xmlWriter.WriteElementString("BrandName", objMed.Brand_Name);
//            xmlWriter.WriteElementString("GenericName", objMed.Generic_Name);
//            xmlWriter.WriteElementString("Form", objMed.Form);
//            xmlWriter.WriteElementString("Strength", objMed.Strength);
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteElementString("DoseUnit", objMed.Dose_Unit);
//            xmlWriter.WriteElementString("DoseTiming", objMed.Dose_Timing);
//            xmlWriter.WriteElementString("Duration", objMed.Duration);
//            xmlWriter.WriteElementString("Quantity", objMed.Quantity);
//            xmlWriter.WriteElementString("QuantityUnit", objMed.Quantity_Unit);
//            xmlWriter.WriteElementString("Refills", objMed.Refills);
//            xmlWriter.WriteElementString("SubstitutionPermitted", objMed.Substitution_Permitted);
//            xmlWriter.WriteElementString("OtherNotes", objMed.Other_Notes);
//            xmlWriter.WriteElementString("PatientNotes", objMed.Patient_Notes);
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteElementString("StartDate", objMed.Start_Date.ToString("dd/MM/yyyy"));
//            xmlWriter.WriteElementString("StopDate", objMed.Stop_Date.ToString("dd/MM/yyyy"));
//            xmlWriter.WriteElementString("FillDate", objMed.Fill_Date.ToString("dd/MM/yyyy"));
//            xmlWriter.WriteElementString("StopReason", objMed.Stop_Reason);
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndDocument();
//            xmlWriter.Flush();
//            Byte[] buffer = new Byte[ms.Length];
//            buffer.DefaultIfEmpty();
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//            return xmlOutput;

//        }



//        public void SendAllergyToRCopia(Rcopia_Allergy_Temp objMed, string sUserName)
//        {
//            RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();
//            string xmlDoc = string.Empty;
//            string UploadAddress = string.Empty;
//            string sOutputXML = string.Empty;
//            xmlDoc = CreateSendAllergyXML(objMed, sUserName);

//            //Upload XML to RCopia Server
//            string sContent = xmlDoc.ToString();
//            sContent = sContent.Trim();
//            sContent = sContent.Replace("\n", "");
//            UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//            if (UploadAddress != null && UploadAddress != string.Empty)
//            {
//                sOutputXML = rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//                //sOutputXML = sOutputXML.Replace("<Command>send_", "<Command>update_");
//                //rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, string.Empty, DateTime.Now, "", 0);
//            }
//        }

//        public string CreateSendAllergyXML(Rcopia_Allergy_Temp objAllergy, string sUserName)
//        {

//            FillRequiredInfo("send_allergy");
//            if (objRcopSettings == null)
//            {
//                return string.Empty;
//            }
//            xmlWriter.WriteStartElement("AllergyList");
//            xmlWriter.WriteStartElement("Allergy");
//            xmlWriter.WriteElementString("ExternalID", string.Empty);
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("Deleted", objAllergy.Deleted);

//            xmlWriter.WriteStartElement("Status");
//            xmlWriter.WriteElementString("Active", objAllergy.Status);
//            xmlWriter.WriteEndElement();

//            xmlWriter.WriteStartElement("Patient");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", objAllergy.Human_ID.ToString());
//            xmlWriter.WriteEndElement();



//            xmlWriter.WriteStartElement("Allergen");
//            xmlWriter.WriteElementString("Name", objAllergy.Allergy_Name);
//            xmlWriter.WriteStartElement("Drug");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("NDCID", objAllergy.NDC_ID);
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteStartElement("Group");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteEndElement();

//            xmlWriter.WriteEndElement();

//            xmlWriter.WriteElementString("Reaction", objAllergy.Reaction);
//            xmlWriter.WriteElementString("OnsetDate", objAllergy.OnsetDate.ToString("dd/MM/yyyy"));

//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();

//            xmlWriter.WriteEndDocument();
//            xmlWriter.Flush();
//            Byte[] buffer = new Byte[ms.Length];
//            buffer.DefaultIfEmpty();
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//            return xmlOutput;

//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Net;
using NHibernate;
using NHibernate.Criterion;

namespace Acurus.Capella.DataAccess
{
    public partial class RCopiaTransactionManager
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlWriterSettings wSettings = new XmlWriterSettings();
        MemoryStream ms;
        XmlWriter xmlWriter;
        IList<Rcopia_Settings> ilstRcopSett;
        Rcopia_Settings objRcopSettings;

        //RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();

        public string creteSendPatientXml(ulong MyhumanID, string sLegalOrg)
        {

            IList<Human> hnList = new List<Human>();
            Human HumanRecord;
            HumanManager hnMngr = new HumanManager();
            VitalsManager vitalMngr = new VitalsManager();
            PatientResultsDTO objVitalDTO = new PatientResultsDTO();
            IList<PatientResults> vitalList = new List<PatientResults>();
            //PatientResults vitalRecord;
            IList<Encounter> EncList = new List<Encounter>();
            EncounterManager EncMngr = new EncounterManager();
            string Weight = string.Empty;
            string Height = string.Empty;
            DateTime dtObservationDate = DateTime.MinValue;

            hnList = hnMngr.GetPatientDetailsUsingPatientInformattion(MyhumanID);
            HumanRecord = hnList[0];

            objVitalDTO = vitalMngr.GetPastVitalDetailsByPatientforRcopia(MyhumanID, 1, 10000);
            vitalList = objVitalDTO.VitalsList;

            EncList = EncMngr.GetEncounterUsingHumanID(HumanRecord.Id);
            DateTime dtpDateOfService = new DateTime();
            if (vitalList.Count > 0)
            {
                ulong maxId = Convert.ToUInt64((from obj in vitalList select obj.Vitals_Group_ID).Max());
                IList<PatientResults> listHeight = (from obj in vitalList where obj.Vitals_Group_ID == maxId && obj.Loinc_Observation.ToUpper() == "HEIGHT" select obj).ToList<PatientResults>();


                IList<PatientResults> listWeight = (from obj in vitalList where obj.Vitals_Group_ID == maxId && obj.Loinc_Observation.ToUpper() == "WEIGHT" select obj).ToList<PatientResults>();
                if (listHeight != null)
                {
                    if (listHeight.Count > 0)
                    {
                        Height = listHeight[0].Value;
                        dtObservationDate = listHeight[0].Captured_date_and_time;
                    }
                }

                if (listWeight != null)
                {
                    if (listWeight.Count > 0)
                    {
                        Weight = listWeight[0].Value;
                        dtObservationDate = listWeight[0].Captured_date_and_time;
                    }
                }
            }


            FillRequiredInfo("send_patient",sLegalOrg);
            if (objRcopSettings == null)
            {
                return string.Empty;
            }
            xmlWriter.WriteElementString("Synchronous", objRcopSettings.Synchronous);
            xmlWriter.WriteElementString("CheckEligibility", objRcopSettings.Check_Eligibilityt);

            xmlWriter.WriteStartElement("PatientList");
            xmlWriter.WriteStartElement("Patient");

            PropertyInfo[] propertyInfos = typeof(Human).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                string sreturn = FillObject(propertyInfo.Name);
                if (sreturn != string.Empty)
                {
                    PropertyInfo o = HumanRecord.GetType().GetProperty(propertyInfo.Name);
                    var v = (o.GetValue(HumanRecord, null));

                    if (sreturn == "DOB")
                    {
                        xmlWriter.WriteElementString(sreturn, ((DateTime)v).ToString("MM/dd/yyyy").Replace("-", "/"));
                    }
                    else
                    {
                        xmlWriter.WriteElementString(sreturn, v.ToString());
                    }
                }
            }
            //xmlWriter.WriteElementString("ObservationDate", ((DateTime)dtObservationDate).ToString("MM/dd/yyyy").Replace("-", "/"));

            if (((DateTime)dtObservationDate).ToString("MM/dd/yyyy") == "01/01/0001")
            {
                xmlWriter.WriteElementString("ObservationDate", DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/"));
            }
            else
            {
                xmlWriter.WriteElementString("ObservationDate", ((DateTime)dtObservationDate).ToString("MM/dd/yyyy").Replace("-", "/"));
            }

            xmlWriter.WriteElementString("Weight", ConvertLbsToKg(Weight));
            xmlWriter.WriteElementString("WeightUnit", "kg");
            //xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
            if (Height.Contains("'") == true && Height.Contains("''") == true)
            {
                // string[] ChargeLineItemIDs = hdnChargeLineItemID.Value.Split(',');
                //int ss= Height.Remove( Height.IndexOf("'"));
                // string [] resultHeight =Height.IndexOf("'");
                //Height = ConvertFeetInchToInch(Height.Remove(Height.IndexOf("'")), Height.Remove(0,Height.IndexOf("'")));

                string[] resultHeight = (Height.ToString()).Split('\''); //added by balaji
                Height = ConvertFeetInchToInch(resultHeight[0], resultHeight[1]);
                xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
            }
            else
                xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
            xmlWriter.WriteElementString("HeightUnit", "cm");
            if (EncList.Count > 0 && EncList.Count != 1)
            {
                dtpDateOfService = EncList[0].Date_of_Service.Date;
                if (EncList[0].Date_of_Service.Date.ToString("yyyy-MM-dd") != "0001-01-01")//For Bug Id 63315
                    xmlWriter.WriteElementString("LastVisitDate", dtpDateOfService.ToString("MM/dd/yyyy").Replace("-", "/"));
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            // Close the document
            xmlWriter.WriteEndDocument();
            // Flush the write
            xmlWriter.Flush();

            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

            return xmlOutput;
        }


        public string createSendMedicationXml(ulong MyhumanID, string sLegalOrg ,string sRequestXML)
        {

            FillRequiredInfo("send_medication", sLegalOrg);
            if (objRcopSettings == null)
            {
                return string.Empty;
            }
            
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            // Close the document
            xmlWriter.WriteEndDocument();
            // Flush the write
            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
            xmlOutput = xmlOutput.Replace("</Request>", "").Replace("</RCExtRequest>", "");
            xmlOutput = xmlOutput + sRequestXML + "</Request> </RCExtRequest>";
            xmlOutput = @"<?xml version=""1.0"" encoding=""utf-8""?>" + xmlOutput.Substring(xmlOutput.IndexOf("<RCExtRequest"));
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.LoadXml(xmlOutput);

            int iMedicationListCount = xmlDocument.SelectNodes("RCExtRequest/Request/MedicationList/Medication").Count;
            for (int iCont = 1; iCont <= iMedicationListCount; iCont++)
            {
                xmlDocument.SelectSingleNode("RCExtRequest/Request/MedicationList/Medication[" + iCont + "]/Sig/Drug/FirstDataBankMedID")?.RemoveAll();
                xmlDocument.SelectSingleNode("RCExtRequest/Request/MedicationList/Medication[" + iCont + "]/Sig/Drug/RcopiaID")?.RemoveAll();
                xmlDocument.SelectSingleNode("RCExtRequest/Request/MedicationList/Medication[" + iCont + "]/Sig/Drug/RxnormID")?.RemoveAll();
                xmlDocument.SelectSingleNode("RCExtRequest/Request/MedicationList/Medication[" + iCont + "]/Sig/Drug/RxnormIDType")?.RemoveAll();

            }
            xmlOutput = xmlDocument.InnerXml;

            return xmlOutput;
        }

        public string createSendAllergyXml(ulong MyhumanID, string sLegalOrg, string sRequestXML)
        {

            FillRequiredInfo("send_allergy", sLegalOrg);
            if (objRcopSettings == null)
            {
                return string.Empty;
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            // Close the document
            xmlWriter.WriteEndDocument();
            // Flush the write
            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
            xmlOutput = xmlOutput.Replace("</Request>", "").Replace("</RCExtRequest>", "");
            xmlOutput = xmlOutput + sRequestXML + "</Request> </RCExtRequest>";
            xmlOutput = @"<?xml version=""1.0"" encoding=""utf-8""?>" + xmlOutput.Substring(xmlOutput.IndexOf("<RCExtRequest"));
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.LoadXml(xmlOutput);

            int iAllergyListCount = xmlDocument.SelectNodes("RCExtRequest/Request/AllergyList/Allergy").Count;
            for (int iCont = 1; iCont <= iAllergyListCount; iCont++)
            {
                xmlDocument.SelectSingleNode("RCExtRequest/Request/AllergyList/Allergy[" + iCont + "]/Allergen/Drug/NDCID")?.RemoveAll();
                xmlDocument.SelectSingleNode("RCExtRequest/Request/AllergyList/Allergy[" + iCont + "]/Allergen/Drug/FirstDataBankMedID")?.RemoveAll();
                xmlDocument.SelectSingleNode("RCExtRequest/Request/AllergyList/Allergy[" + iCont + "]/Allergen/Drug/RxnormID")?.RemoveAll();
                xmlDocument.SelectSingleNode("RCExtRequest/Request/AllergyList/Allergy[" + iCont + "]/Allergen/Drug/RxnormIDType")?.RemoveAll();
                xmlDocument.SelectSingleNode("RCExtRequest/Request/AllergyList/Allergy[" + iCont + "]/Status/Active")?.RemoveAll();               

            }
            xmlOutput = xmlDocument.InnerXml;

            return xmlOutput;
        }

        public void FillRequiredInfo(string sXMLName, string sLegalOrg)
        {

            ms = new MemoryStream();
            wSettings.Indent = true;
            xmlWriter = XmlWriter.Create(ms, wSettings);
            xmlWriter.WriteStartDocument();

            Rcopia_SettingsManager rcopiaMngr = new Rcopia_SettingsManager();
            ilstRcopSett = rcopiaMngr.GetRcopia_Settings(sLegalOrg);
            if (ilstRcopSett.Count > 0)
            {
                objRcopSettings = (from g in ilstRcopSett where g.Command == sXMLName && g.Legal_Org == sLegalOrg select g).ToList<Rcopia_Settings>()[0];
                if (objRcopSettings == null)
                {
                    return;
                }
                xmlWriter.WriteStartElement("RCExtRequest");
                xmlWriter.WriteAttributeString("version", "2.35");//changed version from 2.13 to 2.19 as per Dr.First's instructions[RajeevMotwani] as Assessment data were not getting uploaded(under Problem section) to Dr.First
                xmlWriter.WriteStartElement("TraceInformation");
                xmlWriter.WriteElementString("RequestMessageID", objRcopSettings.Request_Message_ID);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Caller");
                PropertyInfo[] propertyInfoColl = typeof(Rcopia_Settings).GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfoColl)
                {
                    string sreturn = FillObject(propertyInfo.Name);
                    if (sreturn != string.Empty)
                    {
                        PropertyInfo o = objRcopSettings.GetType().GetProperty(propertyInfo.Name);
                        var v = (o.GetValue(objRcopSettings, null));
                        xmlWriter.WriteElementString(sreturn, v.ToString());
                    }
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteElementString("SystemName", objRcopSettings.System_Name);
                xmlWriter.WriteElementString("RcopiaPracticeUsername", objRcopSettings.Rcopia_Practice_User_name);
                xmlWriter.WriteStartElement("Request");
                xmlWriter.WriteElementString("Command", objRcopSettings.Command);
            }

        }


        public string FillObject(string sPropname)
        {
            string ReturnString = string.Empty;
            switch (sPropname)
            {
                case "Id":
                    ReturnString = "ExternalID";
                    break;
                case "Street_Address1":
                    ReturnString = "Address1";
                    break;
                case "Street_Address2":
                    ReturnString = "Address2";
                    break;
                case "City":
                    ReturnString = "City";
                    break;
                case "Birth_Date":
                    ReturnString = "DOB";
                    break;
                case "First_Name":
                    ReturnString = "FirstName";
                    break;
                case "Last_Name":
                    ReturnString = "LastName";
                    break;
                case "MI":
                    ReturnString = "MiddleName";
                    break;
                case "Sex":
                    ReturnString = "Sex";
                    break;
                case "Home_Phone_No":
                    ReturnString = "HomePhone";
                    break;
                case "Cell_Phone_Number":
                    ReturnString = "MobilePhone";
                    break;
                case "Work_Phone_No":
                    ReturnString = "WorkPhone";
                    break;
                case "SSN":
                    ReturnString = "SSN";
                    break;
                case "State":
                    ReturnString = "State";
                    break;
                case "ZipCode":
                    ReturnString = "Zip";
                    break;
                case "Vendor_Name":
                    ReturnString = "VendorName";
                    break;
                case "Vendor_Password":
                    ReturnString = "VendorPassword";
                    break;
                case "Application":
                    ReturnString = "Application";
                    break;
                case "Rcopia_Version":
                    ReturnString = "Version";
                    break;
                case "Practice_Name":
                    ReturnString = "PracticeName";
                    break;
                case "Station":
                    ReturnString = "Station";
                    break;

            }
            return ReturnString;
        }

        public void SendPatientToRCopia(ulong ulHumanID, string sMacAddress, string sLegalOrg)
        {
            string xmlDoc = string.Empty;
            string UploadAddress = string.Empty;

            xmlDoc = creteSendPatientXml(ulHumanID,sLegalOrg);

            //Upload XML to RCopia Server
            string sContent = xmlDoc.ToString();
            sContent = sContent.Trim();
            sContent = sContent.Replace("\n", "");

            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
            UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
            if (UploadAddress != null && UploadAddress != string.Empty)
            {
                if (rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1) == null)
                {
                    //Muthu
                    HumanManager objmngr = new HumanManager();
                    IList<Human> ilsthuman = new List<Human>();
                    ilsthuman = objmngr.GetPatientDetailsUsingPatientInformattion(ulHumanID);
                    ilsthuman[0].Is_Sent_To_Rcopia = "N";
                    objmngr.UpdateFailedRCopiaHuman(ilsthuman[0], sMacAddress);
                }
            }
        }



        public void SendProblemToRCopia(IList<ulong> listofinsertIDs, string sSource, bool isDelete, IList<Assessment> ilstAssessment, string sLegalOrg)
        {
            string xmlDoc = string.Empty;
            string UploadAddress = string.Empty;
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);

            xmlDoc = CreateSendproblemXML(listofinsertIDs, sSource, isDelete, ilstAssessment,sLegalOrg);

            //Upload XML to RCopia Server
            string sContent = xmlDoc.ToString();
            sContent = sContent.Trim();
            sContent = sContent.Replace("\n", "");
            UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
            if (UploadAddress != null && UploadAddress != string.Empty)
            {
                rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
            }
        }

        public string CreateSendproblemXML(IList<ulong> listofinsertIDs, string sSource, bool isDelete, IList<Assessment> ilstassement, string sLegalOrg)
        {

            FillRequiredInfo("send_problem",sLegalOrg);
            if (objRcopSettings == null)
            {
                return string.Empty;
            }
            xmlWriter.WriteStartElement("ProblemList");

            IList<ProblemList> ilstProblem = new List<ProblemList>();
            Assessment objAssesment = new Assessment();
            AssessmentManager objAssmngr = new AssessmentManager();

            if (sSource.ToUpper() == "PROBLEMLIST")
            {
                ProblemListManager objmngr = new ProblemListManager();
                if (listofinsertIDs!=null&& listofinsertIDs.Count>0)
                    ilstProblem = objmngr.GetProblemListUsingProblemListId(listofinsertIDs[0]);
            }
            else if (sSource.ToUpper() == "ASSESMENT")
            {
                if (ilstassement != null && ilstassement.Count > 0)
                {

                    for (int p = 0; p < ilstassement.Count; p++)
                    {
                        objAssesment = ilstassement[p];
                        Fillassessment(objAssesment, isDelete);
                    }
                }
                else if (sSource.ToUpper() == "ASSESMENT" && ilstassement == null)
                {
                    if (listofinsertIDs.Count > 0)
                    {
                        for (int u = 0; u < listofinsertIDs.Count; u++)
                        {
                            objAssesment = objAssmngr.GetAssesmentUsingAssesmentId(listofinsertIDs[u]);
                            Fillassessment(objAssesment, isDelete);
                        }
                    }
                }
            }
            if (sSource.ToUpper() == "PROBLEMLIST")
            {
                if (ilstProblem.Count > 0)
                {
                    xmlWriter.WriteStartElement("Problem");
                    if (ilstProblem[0].Is_Active.ToLower() == "y" && ilstProblem[0].Status.ToUpper() == "ACTIVE")
                        xmlWriter.WriteElementString("Deleted", "n");
                    else
                        xmlWriter.WriteElementString("Deleted", "y");
                    xmlWriter.WriteStartElement("Status");
                    if (ilstProblem[0].Status.ToUpper() == "ACTIVE")
                    {
                        if (ilstProblem[0].Is_Active.ToLower() == "y" && ilstProblem[0].Status.ToUpper() == "ACTIVE")
                        {
                            //xmlWriter.WriteElementString("Active", "y");
                            xmlWriter.WriteStartElement("Active");
                            xmlWriter.WriteEndElement();
                        }
                        else
                        {
                            //xmlWriter.WriteElementString("Inactive", "y");
                            xmlWriter.WriteStartElement("Inactive");
                            xmlWriter.WriteEndElement();
                        }
                    }
                    else if (ilstProblem[0].Status.ToUpper() == "INACTIVE")
                    {
                        //xmlWriter.WriteElementString("Inactive", "y");
                        xmlWriter.WriteStartElement("Inactive");
                        xmlWriter.WriteEndElement();
                    }
                    else if (ilstProblem[0].Status.ToUpper() == "RESOLVED")
                    {
                        //xmlWriter.WriteElementString("Resolved", "y");
                        xmlWriter.WriteStartElement("Resolved");
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("RcopiaID", string.Empty);
                    //Jira CAP-2612
                    //xmlWriter.WriteElementString("ExternalID", "Prob" + ilstProblem[0].Id.ToString());
                    xmlWriter.WriteElementString("ExternalID", string.Empty);
                    xmlWriter.WriteStartElement("Patient");
                    xmlWriter.WriteElementString("RcopiaID", string.Empty);
                    xmlWriter.WriteElementString("ExternalID", ilstProblem[0].Human_ID.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("Code", ilstProblem[0].ICD);
                    xmlWriter.WriteElementString("Description", ilstProblem[0].Problem_Description);
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer.DefaultIfEmpty();
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

            return xmlOutput;

        }

        public void Fillassessment(Assessment objAssesment, bool isDelete)
        {
            xmlWriter.WriteStartElement("Problem");
            if (isDelete == true)
            {
                xmlWriter.WriteElementString("Deleted", "y");
            }
            else
            {
                if (objAssesment.Assessment_Status.ToUpper() == "RESOLVED")
                    xmlWriter.WriteElementString("Deleted", "y");
                else
                    xmlWriter.WriteElementString("Deleted", "n");
            }
            xmlWriter.WriteStartElement("Status");
            if (isDelete == false)
            {
                if (objAssesment.Assessment_Status.ToUpper() == "RESOLVED")
                {
                    //xmlWriter.WriteElementString("Resolved", "y");
                    xmlWriter.WriteStartElement("Resolved");
                    xmlWriter.WriteEndElement();
                }
                else
                {
                    //xmlWriter.WriteElementString("Active", "y");
                    xmlWriter.WriteStartElement("Active");
                    xmlWriter.WriteEndElement();
                }
            }
            else
            {
                //xmlWriter.WriteElementString("Inactive", "y");
                xmlWriter.WriteStartElement("Inactive");
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteElementString("RcopiaID", string.Empty);
            //Jira CAP-2612
            //xmlWriter.WriteElementString("ExternalID", "ASS" + objAssesment.Id.ToString());
            xmlWriter.WriteElementString("ExternalID", string.Empty);
            xmlWriter.WriteStartElement("Patient");
            xmlWriter.WriteElementString("RcopiaID", string.Empty);
            xmlWriter.WriteElementString("ExternalID", objAssesment.Human_ID.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("ICD10");
            xmlWriter.WriteElementString("Code", objAssesment.ICD);
            xmlWriter.WriteElementString("Description", objAssesment.ICD_Description);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }

        public string ConvertInchesToCM(string s)
        {
            if (s != string.Empty)
            {
                decimal cmValue = decimal.Round((Convert.ToDecimal(s) * 2.54m), 2);
                return cmValue.ToString();
            }
            else
                return string.Empty;
        }
        public string ConvertFeetInchToInch(string s1, string s2)
        {
            if (s1 == string.Empty)
            {
                return string.Empty;
            }
            decimal inValue = 0;
            if (s2 != string.Empty)
                inValue = decimal.Round((Convert.ToDecimal(s1) * 12m) + Convert.ToDecimal(s2), 2);
            else
                inValue = decimal.Round((Convert.ToDecimal(s1) * 12m), 2);
            if (inValue != 0m)
                return inValue.ToString();
            else
                return string.Empty;

        }
        public string ConvertLbsToKg(string s)
        {
            if (s != string.Empty)
            {
                decimal kgValue = decimal.Round(Convert.ToDecimal(s) / 2.2m, 2);
                return kgValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public void SendBulkUploadPatientToRCopia(ulong ulHumanID, string sMacAddress, string sLegalOrg)
        {
            string xmlDoc = string.Empty;
            string UploadAddress = string.Empty;
            HumanManager HumanMgr = new HumanManager();
            string sFileName = System.Configuration.ConfigurationSettings.AppSettings["XMLFileName"] + DateTime.Now.ToString("dd-MMM-yyyy");

            int i = 1, Count = 0;
            while (i >= 0)
            {
                Count = (i - 1) * 500;
                IList<Human> HumanList = HumanMgr.GetPatientListForRcopia(Count, 500);
                creteSendPatientXmlPost(HumanList, sFileName + i + ".xml",sLegalOrg);
                if (HumanList.Count == 500)
                    i++;
                else
                    break;

            }
        }
        public void creteSendPatientXmlPost(IList<Human> HumanList, string sFileName, string sLegalOrg)
        {

            string xmlOutput = string.Empty;

            Human HumanRecord;
            VitalsManager vitalMngr = new VitalsManager();
            PatientResultsDTO objVitalDTO = new PatientResultsDTO();
            IList<PatientResults> vitalList = new List<PatientResults>();
            //PatientResults vitalRecord;
            string Weight = string.Empty;
            string Height = string.Empty;
            xmlWriter = new XmlTextWriter(sFileName, Encoding.UTF8);
            FillRequiredInfo("send_patient",sLegalOrg);
            if (objRcopSettings == null)
            {
                return;
            }
            xmlWriter.WriteElementString("Synchronous", objRcopSettings.Synchronous);
            xmlWriter.WriteElementString("CheckEligibility", objRcopSettings.Check_Eligibilityt);

            xmlWriter.WriteStartElement("PatientList");
            if (HumanList != null)
            {
                for (int i = 0; i < HumanList.Count; i++)
                {
                    HumanRecord = new Human();
                    HumanRecord = HumanList[i];
                    objVitalDTO = vitalMngr.GetPastVitalDetailsByPatientforRcopia(HumanRecord.Id, 1, 10000);
                    vitalList = objVitalDTO.VitalsList;
                    if (vitalList.Count > 0)
                    {
                        ulong maxId = Convert.ToUInt64((from obj in vitalList select obj.Vitals_Group_ID).Max());
                        IList<PatientResults> list = (from obj in vitalList where obj.Vitals_Group_ID == maxId && (obj.Loinc_Observation.ToUpper() == "HEIGHT" || obj.Loinc_Observation.ToUpper() == "WEIGHT") select obj).ToList<PatientResults>();
                        if (list != null)
                        {
                            if (list.Count > 0)
                            {
                                Height = list.First().Value;
                                Weight = list.Last().Value;
                            }
                        }
                    }
                    xmlWriter.WriteStartElement("Patient");
                    PropertyInfo[] propertyInfos = typeof(Human).GetProperties();
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        string sreturn = FillObject(propertyInfo.Name);
                        if (sreturn != string.Empty)
                        {
                            PropertyInfo o = HumanRecord.GetType().GetProperty(propertyInfo.Name);
                            var v = (o.GetValue(HumanRecord, null));
                            xmlWriter.WriteElementString(sreturn, v.ToString());
                        }
                    }
                    xmlWriter.WriteElementString("Weight", ConvertLbsToKg(Weight));
                    xmlWriter.WriteElementString("WeightUnit", "kg");
                    if (Height.Contains("'") == true && Height.Contains("''") == true)
                    {

                        // Height = ConvertFeetInchToInch(Height);
                        //xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
                        string[] resultHeight = (Height.ToString()).Split('\'');
                        Height = ConvertFeetInchToInch(resultHeight[0], resultHeight[1]);
                        xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
                    }
                    else
                        xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
                    xmlWriter.WriteElementString("HeightUnit", "cm");
                    xmlWriter.WriteEndElement();

                }

            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            // Close the document
            xmlWriter.WriteEndDocument();
            // Flush the write
            xmlWriter.Flush();
        }



        //public void SendMedicationToRCopia(Rcopia_Medication_Temp objMed, string sUserName)
        //{
        //    RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();
        //    string xmlDoc = string.Empty;
        //    string UploadAddress = string.Empty;
        //    string sOutputXML = string.Empty;
        //    xmlDoc = CreateSendMedicationXML(objMed, sUserName);

        //    //Upload XML to RCopia Server
        //    string sContent = xmlDoc.ToString();
        //    sContent = sContent.Trim();
        //    sContent = sContent.Replace("\n", "");
        //    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
        //    if (UploadAddress != null && UploadAddress != string.Empty)
        //    {
        //        sOutputXML = rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
        //        //sOutputXML = sOutputXML.Replace("<Command>send_", "<Command>update_");
        //        //rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, string.Empty, DateTime.Now, "", 0);
        //    }
        //}

        //public string CreateSendMedicationXML(Rcopia_Medication_Temp objMed, string sUserName)
        //{

        //    FillRequiredInfo("send_medication");
        //    if (objRcopSettings == null)
        //    {
        //        return string.Empty;
        //    }
        //    xmlWriter.WriteStartElement("MedicationList");
        //    xmlWriter.WriteStartElement("Medication");
        //    xmlWriter.WriteElementString("Deleted", objMed.Deleted);

        //    xmlWriter.WriteElementString("RcopiaID", string.Empty);
        //    xmlWriter.WriteElementString("ExternalID", string.Empty);

        //    xmlWriter.WriteStartElement("Patient");
        //    xmlWriter.WriteElementString("RcopiaID", string.Empty);
        //    xmlWriter.WriteElementString("ExternalID", objMed.Human_ID.ToString());
        //    xmlWriter.WriteEndElement();

        //    xmlWriter.WriteStartElement("Provider");
        //    xmlWriter.WriteElementString("Username", sUserName);
        //    xmlWriter.WriteEndElement();

        //    xmlWriter.WriteStartElement("Sig");
        //    xmlWriter.WriteStartElement("Drug");
        //    xmlWriter.WriteElementString("NDCID", objMed.NDC_ID);
        //    xmlWriter.WriteElementString("BrandName", objMed.Brand_Name);
        //    xmlWriter.WriteElementString("GenericName", objMed.Generic_Name);
        //    xmlWriter.WriteElementString("Form", objMed.Form);
        //    xmlWriter.WriteElementString("Strength", objMed.Strength);
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteElementString("DoseUnit", objMed.Dose_Unit);
        //    xmlWriter.WriteElementString("DoseTiming", objMed.Dose_Timing);
        //    xmlWriter.WriteElementString("Duration", objMed.Duration);
        //    xmlWriter.WriteElementString("Quantity", objMed.Quantity);
        //    xmlWriter.WriteElementString("QuantityUnit", objMed.Quantity_Unit);
        //    xmlWriter.WriteElementString("Refills", objMed.Refills);
        //    xmlWriter.WriteElementString("SubstitutionPermitted", objMed.Substitution_Permitted);
        //    xmlWriter.WriteElementString("OtherNotes", objMed.Other_Notes);
        //    xmlWriter.WriteElementString("PatientNotes", objMed.Patient_Notes);
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteElementString("StartDate", objMed.Start_Date.ToString("dd/MM/yyyy"));
        //    xmlWriter.WriteElementString("StopDate", objMed.Stop_Date.ToString("dd/MM/yyyy"));
        //    xmlWriter.WriteElementString("FillDate", objMed.Fill_Date.ToString("dd/MM/yyyy"));
        //    xmlWriter.WriteElementString("StopReason", objMed.Stop_Reason);
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteEndDocument();
        //    xmlWriter.Flush();
        //    Byte[] buffer = new Byte[ms.Length];
        //    buffer.DefaultIfEmpty();
        //    buffer = ms.ToArray();
        //    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
        //    return xmlOutput;

        //}



        //public void SendAllergyToRCopia(Rcopia_Allergy_Temp objMed, string sUserName)
        //{
        //    RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();
        //    string xmlDoc = string.Empty;
        //    string UploadAddress = string.Empty;
        //    string sOutputXML = string.Empty;
        //    xmlDoc = CreateSendAllergyXML(objMed, sUserName);

        //    //Upload XML to RCopia Server
        //    string sContent = xmlDoc.ToString();
        //    sContent = sContent.Trim();
        //    sContent = sContent.Replace("\n", "");
        //    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
        //    if (UploadAddress != null && UploadAddress != string.Empty)
        //    {
        //        sOutputXML = rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
        //        //sOutputXML = sOutputXML.Replace("<Command>send_", "<Command>update_");
        //        //rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, string.Empty, DateTime.Now, "", 0);
        //    }
        //}

        //public string CreateSendAllergyXML(Rcopia_Allergy_Temp objAllergy, string sUserName)
        //{

        //    FillRequiredInfo("send_allergy");
        //    if (objRcopSettings == null)
        //    {
        //        return string.Empty;
        //    }
        //    xmlWriter.WriteStartElement("AllergyList");
        //    xmlWriter.WriteStartElement("Allergy");
        //    xmlWriter.WriteElementString("ExternalID", string.Empty);
        //    xmlWriter.WriteElementString("RcopiaID", string.Empty);
        //    xmlWriter.WriteElementString("Deleted", objAllergy.Deleted);

        //    xmlWriter.WriteStartElement("Status");
        //    xmlWriter.WriteElementString("Active", objAllergy.Status);
        //    xmlWriter.WriteEndElement();

        //    xmlWriter.WriteStartElement("Patient");
        //    xmlWriter.WriteElementString("RcopiaID", string.Empty);
        //    xmlWriter.WriteElementString("ExternalID", objAllergy.Human_ID.ToString());
        //    xmlWriter.WriteEndElement();



        //    xmlWriter.WriteStartElement("Allergen");
        //    xmlWriter.WriteElementString("Name", objAllergy.Allergy_Name);
        //    xmlWriter.WriteStartElement("Drug");
        //    xmlWriter.WriteElementString("RcopiaID", string.Empty);
        //    xmlWriter.WriteElementString("NDCID", objAllergy.NDC_ID);
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteStartElement("Group");
        //    xmlWriter.WriteElementString("RcopiaID", string.Empty);
        //    xmlWriter.WriteEndElement();

        //    xmlWriter.WriteEndElement();

        //    xmlWriter.WriteElementString("Reaction", objAllergy.Reaction);
        //    xmlWriter.WriteElementString("OnsetDate", objAllergy.OnsetDate.ToString("dd/MM/yyyy"));

        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteEndElement();

        //    xmlWriter.WriteEndDocument();
        //    xmlWriter.Flush();
        //    Byte[] buffer = new Byte[ms.Length];
        //    buffer.DefaultIfEmpty();
        //    buffer = ms.ToArray();
        //    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
        //    return xmlOutput;

        //}
    }
}

