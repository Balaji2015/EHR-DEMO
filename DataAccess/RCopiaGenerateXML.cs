//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.Reflection;
//using Acurus.Capella.Core.DomainObjects;
//using Acurus.Capella.DataAccess.ManagerObjects;
//using System.IO;
//using System.Web.UI;

//namespace Acurus.Capella.DataAccess
//{
//    public partial class RCopiaGenerateXML
//    {
//        XmlDocument xmlDoc = new XmlDocument();
//        XmlWriterSettings wSettings = new XmlWriterSettings();
//        MemoryStream ms;
//        XmlWriter xmlWriter;
//        IList<Rcopia_Settings> ilstRcopSett;
//        Rcopia_Settings objRcopSettings;



//        public string CreateGetURLXML()
//        {
//            ms = new MemoryStream();
//            wSettings.Indent = true;
//            xmlWriter = XmlWriter.Create(ms, wSettings);
//            xmlWriter.WriteStartDocument();
//            Rcopia_SettingsManager rcopiaSettingMngr = new Rcopia_SettingsManager();
//            ilstRcopSett = rcopiaSettingMngr.GetRcopia_Settings();
//            if (ilstRcopSett.Count > 0)
//            {
//                objRcopSettings = (from g in ilstRcopSett where g.Command == "get_url" select g).ToList<Rcopia_Settings>()[0];
//                xmlWriter.WriteStartElement("RCExtRequest");
//                xmlWriter.WriteAttributeString("version", "2.19");//changed version from 2.13 to 2.19 as per Dr.First's instructions[RajeevMotwani] as Assessment data were not getting uploaded(under Problem section) to Dr.First
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
//                xmlWriter.WriteElementString("RcopiaPracticeUsername", objRcopSettings.Rcopia_Practice_User_name);
//                xmlWriter.WriteStartElement("Request");
//                xmlWriter.WriteElementString("Command", "get_url");
//                xmlWriter.WriteEndElement();
//                xmlWriter.WriteEndDocument();
//                xmlWriter.Flush();
//            }
//            Byte[] buffer = new Byte[ms.Length];
//            buffer.DefaultIfEmpty();
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//            return xmlOutput;
//        }

//        public string CreateUpdatePatientXML()
//        {
//            FillRequiredInfo("update_patient");

//            Rcopia_Update_InfoManager rcopiaUpdateMngr = new Rcopia_Update_InfoManager();
//            string ilstRcopUpdateInfoDate = rcopiaUpdateMngr.GetRcopiaUpdateInfoCommandName("update_patient");
//            if (ilstRcopUpdateInfoDate != "")
//            {

//                //Rcopia_Update_info objupdateInfo = (from j in ilstRcopUpdateInfo where j.Command == "update_patient" select j).ToList<Rcopia_Update_info>()[0];
//                xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
//            }
//            xmlWriter.WriteElementString("IncludeAliasPatients", "y");
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndDocument();
//            xmlWriter.Flush();
//            Byte[] buffer = new Byte[ms.Length];
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

//            return xmlOutput;
//        }

//        public string CreateUpdatePatientXMLforSinglePatient(string sHuman_ID)
//        {
//            FillRequiredInfo("update_patient");

//            Rcopia_Update_InfoManager rcopiaUpdateMngr = new Rcopia_Update_InfoManager();
//            string ilstRcopUpdateInfoDate = rcopiaUpdateMngr.GetRcopiaUpdateInfoCommandName("update_patient");
//            if (ilstRcopUpdateInfoDate != "")
//            {

//                //Rcopia_Update_info objupdateInfo = (from j in ilstRcopUpdateInfo where j.Command == "update_patient" select j).ToList<Rcopia_Update_info>()[0];
//                //xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
//            }
//            xmlWriter.WriteElementString("IncludeAliasPatients", "y");
//            xmlWriter.WriteStartElement("PatientList");
//            xmlWriter.WriteStartElement("Patient");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", sHuman_ID);
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndDocument();
//            xmlWriter.Flush();
//            Byte[] buffer = new Byte[ms.Length];
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

//            return xmlOutput;
//        }

//        public string CreateUpdatePrescriptionXML()
//        {
//            FillRequiredInfo("update_prescription");
//            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();

//            string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_prescription");
//            if (ilstRcopUpdateInfoDate != "")
//                xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
//            xmlWriter.WriteStartElement("Patient");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", string.Empty);
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteElementString("Status", "all");
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndDocument();
//            xmlWriter.Flush();
//            Byte[] buffer = new Byte[ms.Length];
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

//            return xmlOutput;

//        }

//        public string CreateUpdateMedicationXML()
//        {
//            FillRequiredInfo("update_medication");
//            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
//            //IList<Rcopia_Update_info> ilstRcopUpdateInfo = rcopiaUpdatemngr.GetRcopiaUpdateInfo();

//            string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_medication");
//            if (ilstRcopUpdateInfoDate != "")
//                xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));

//            //if (ilstRcopUpdateInfo.Count > 0)
//            //{
//            //    Rcopia_Update_info objupdateInfo = (from j in ilstRcopUpdateInfo where j.Command == "update_medication" select j).ToList<Rcopia_Update_info>()[0];
//            //    xmlWriter.WriteElementString("LastUpdateDate", objupdateInfo.Last_Updated_Date_Time.ToString("MM/dd/yyyy hh:mm:ss").Replace("-", "/"));
//            //}
//            xmlWriter.WriteStartElement("Patient");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", string.Empty);
//            xmlWriter.WriteEndElement();
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

//        public string CreateUpdateProblemXML()
//        {

//            FillRequiredInfo("update_problem");
//            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
//            string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_problem");
//            if (ilstRcopUpdateInfoDate != "")
//                xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
//            xmlWriter.WriteStartElement("Patient");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", string.Empty);
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndDocument();
//            xmlWriter.Flush();
//            Byte[] buffer = new Byte[ms.Length];
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

//            return xmlOutput;
//        }

//        public string CreateUpdateAllergyXML()
//        {
//            FillRequiredInfo("update_allergy");
//            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
//            string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_allergy");
//            if (ilstRcopUpdateInfoDate != "")
//                xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
//            xmlWriter.WriteElementString("ReturnAllNDCIDs", "y");
//            xmlWriter.WriteStartElement("Patient");
//            xmlWriter.WriteElementString("RcopiaID", string.Empty);
//            xmlWriter.WriteElementString("ExternalID", string.Empty);
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndDocument();
//            xmlWriter.Flush();
//            Byte[] buffer = new Byte[ms.Length];
//            buffer = ms.ToArray();
//            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

//            return xmlOutput;
//        }
//        //for medication review status

//        public string CreateGetReviewStatusXML(string sRequest, DateTime dtLastDate)
//        {
//            FillRequiredInfo(sRequest);

//            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();

//            string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName(sRequest);
//            if (ilstRcopUpdateInfoDate != "")
//                xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
//            //xmlWriter.WriteStartElement("PatientList");
//            //xmlWriter.WriteStartElement("Patient");
//            //xmlWriter.WriteStartElement("RcopiaID");
//            //xmlWriter.WriteEndElement();
//            //xmlWriter.WriteStartElement("ExternalID");
//            //xmlWriter.WriteEndElement();
//            //xmlWriter.WriteEndElement();
//            //xmlWriter.WriteEndElement();

//            xmlWriter.WriteStartElement("MaximumReviews");
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteEndElement();

//            xmlWriter.Flush();
//            Byte[] buffer = new Byte[ms.Length];
//            buffer.DefaultIfEmpty();
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

//            Rcopia_SettingsManager rcopiaSettingsMngr = new Rcopia_SettingsManager();
//            ilstRcopSett = rcopiaSettingsMngr.GetRcopia_Settings();
//            if (ilstRcopSett.Count > 0)
//            {

//                objRcopSettings = (from g in ilstRcopSett where g.Command == sXMLName select g).ToList<Rcopia_Settings>()[0];
//                if (sXMLName == "get_review_status" || sXMLName == "update_patient_office_visits")
//                {
//                    xmlWriter.WriteStartElement("RCExtRequest");
//                    xmlWriter.WriteAttributeString("version", "2.19");//changed version from 2.13 to 2.19 as per Dr.First's instructions[RajeevMotwani] as Assessment data were not getting uploaded(under Problem section) to Dr.First
//                    xmlWriter.WriteStartElement("TraceInformation");
//                    xmlWriter.WriteElementString("RequestMessageID", objRcopSettings.Request_Message_ID);
//                    xmlWriter.WriteEndElement();

//                    xmlWriter.WriteStartElement("Caller");
//                    PropertyInfo[] propertyInfoColl = typeof(Rcopia_Settings).GetProperties();
//                    foreach (PropertyInfo propertyInfo in propertyInfoColl)
//                    {
//                        string sreturn = FillObject(propertyInfo.Name);
//                        if (sreturn != string.Empty && sreturn != "ExternalID")
//                        {
//                            PropertyInfo o = objRcopSettings.GetType().GetProperty(propertyInfo.Name);
//                            var v = (o.GetValue(objRcopSettings, null));
//                            xmlWriter.WriteElementString(sreturn, v.ToString());
//                        }
//                    }
//                }
//                else
//                {

//                    xmlWriter.WriteStartElement("RCExtRequest");
//                    xmlWriter.WriteAttributeString("version", "2.19");//changed version from 2.13 to 2.19 as per Dr.First's instructions[RajeevMotwani] as Assessment data were not getting uploaded(under Problem section) to Dr.First
//                    xmlWriter.WriteStartElement("TraceInformation");
//                    xmlWriter.WriteElementString("RequestMessageID", objRcopSettings.Request_Message_ID);
//                    xmlWriter.WriteEndElement();

//                    xmlWriter.WriteStartElement("Caller");
//                    PropertyInfo[] propertyInfoColl = typeof(Rcopia_Settings).GetProperties();
//                    foreach (PropertyInfo propertyInfo in propertyInfoColl)
//                    {
//                        string sreturn = FillObject(propertyInfo.Name);
//                        if (sreturn != string.Empty)
//                        {
//                            PropertyInfo o = objRcopSettings.GetType().GetProperty(propertyInfo.Name);
//                            var v = (o.GetValue(objRcopSettings, null));
//                            xmlWriter.WriteElementString(sreturn, v.ToString());
//                        }
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

//        public string creteSendMedicationXml(IList<Rcopia_Medication> insertList, IList<Rcopia_Medication> UpdateList, IList<Rcopia_Medication> deleteList)
//        {
//            IList<Rcopia_Medication> ilstFinalResult = new List<Rcopia_Medication>();
//            string xmlOutput1 = string.Empty;
//            //if (objRcopSettings == null)
//            //{
//            //    return string.Empty;
//            //}
//            if (deleteList != null && deleteList.Count > 0)
//            {
//                for (int i = 0; i < deleteList.Count; i++)
//                {
//                    FillRequiredInfo("send_medication");
//                    xmlWriter.WriteStartElement("MedicationList");
//                    xmlWriter.WriteStartElement("Medication");
//                    xmlWriter.WriteElementString("Deleted", "y");
//                    xmlWriter.WriteElementString("RcopiaID", deleteList[i].Id.ToString());
//                    xmlWriter.WriteElementString("ExternalID", deleteList[i].Id.ToString());
//                    xmlWriter.WriteStartElement("Patient");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("ExternalID", deleteList[i].Human_ID.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Provider");
//                    xmlWriter.WriteElementString("Username", deleteList[i].Last_Modified_By);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Sig");
//                    xmlWriter.WriteStartElement("Drug");
//                    xmlWriter.WriteElementString("NDCID", deleteList[i].NDC_ID);
//                    xmlWriter.WriteElementString("BrandName", deleteList[i].Brand_Name);
//                    xmlWriter.WriteElementString("GenericName", deleteList[i].Generic_Name);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteElementString("PatientNotes", deleteList[i].Patient_Notes);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteElementString("DoseOther", deleteList[i].Dose_Other);
//                    xmlWriter.WriteElementString("StartDate", deleteList[i].Start_Date.ToString());
//                    xmlWriter.WriteElementString("StopDate", deleteList[i].Stop_Date.ToString());
//                    xmlWriter.WriteElementString("FillDate", deleteList[i].Fill_Date.ToString());
//                    xmlWriter.WriteElementString("StopReason", deleteList[i].Stop_Reason);
//                    xmlWriter.WriteElementString("RxnormID", deleteList[i].Rxnorm_ID.ToString());
//                    xmlWriter.WriteElementString("RxnormType", deleteList[i].Rxnorm_ID_Type.ToString());

//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.Flush();


//                    Byte[] buffer = new Byte[ms.Length];
//                    buffer = ms.ToArray();
//                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//                    string sContent = xmlOutput.ToString();
//                    sContent = sContent.Trim();
//                    sContent = sContent.Replace("\n", "");
//                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
//                    string UploadAddress = string.Empty;
//                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//                    if (UploadAddress != null && UploadAddress != string.Empty)
//                    {
//                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//                    }
//                    xmlOutput1 = xmlOutput;

//                }
//                ilstFinalResult = deleteList;

//            }

//            if (insertList != null && insertList.Count > 0)
//            {
//                for (int i = 0; i < insertList.Count; i++)
//                {
//                    FillRequiredInfo("send_medication");
//                    xmlWriter.WriteStartElement("MedicationList");
//                    xmlWriter.WriteStartElement("Medication");
//                    xmlWriter.WriteElementString("Deleted", "n");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("ExternalID", "");
//                    xmlWriter.WriteStartElement("Patient");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("ExternalID", insertList[i].Human_ID.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Provider");
//                    xmlWriter.WriteElementString("Username", insertList[i].Last_Modified_By);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Sig");
//                    xmlWriter.WriteStartElement("Drug");
//                    xmlWriter.WriteElementString("NDCID", insertList[i].NDC_ID);
//                    xmlWriter.WriteElementString("BrandName", insertList[i].Brand_Name);
//                    xmlWriter.WriteElementString("GenericName", insertList[i].Generic_Name);
//                    xmlWriter.WriteElementString("Route", insertList[i].Route);
//                    xmlWriter.WriteElementString("Form", insertList[i].Dose_Unit);
//                    xmlWriter.WriteElementString("Strength", insertList[i].Strength);
//                    xmlWriter.WriteEndElement();



//                    //

//                    xmlWriter.WriteElementString("Route", insertList[i].Route);
//                    //xmlWriter.WriteElementString("Strength", insertList[i].Strength);
//                    //
//                    xmlWriter.WriteElementString("Dose", insertList[i].Dose);
//                    xmlWriter.WriteElementString("DoseOther", insertList[i].Dose_Other);

//                    xmlWriter.WriteElementString("DoseUnit", insertList[i].Dose_Unit);

//                    xmlWriter.WriteElementString("DoseTiming", insertList[i].Dose_Timing);

//                    xmlWriter.WriteElementString("PatientNotes", insertList[i].Patient_Notes);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteElementString("StartDate", insertList[i].Start_Date.ToString());
//                    xmlWriter.WriteElementString("StopDate", insertList[i].Stop_Date.ToString());
//                    xmlWriter.WriteElementString("FillDate", insertList[i].Fill_Date.ToString());
//                    xmlWriter.WriteElementString("StopReason", insertList[i].Stop_Reason);
//                    xmlWriter.WriteElementString("RxnormID", insertList[i].Rxnorm_ID.ToString());
//                    xmlWriter.WriteElementString("RxnormType", insertList[i].Rxnorm_ID_Type.ToString());

//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.Flush();

//                    Byte[] buffer = new Byte[ms.Length];
//                    buffer = ms.ToArray();
//                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//                    string sContent = xmlOutput.ToString();
//                    //
//                    sContent = sContent.Replace("&lt;", "<");
//                    sContent = sContent.Replace("&gt;", ">");
//                    //
//                    sContent = sContent.Trim();
//                    sContent = sContent.Replace("\n", "");
//                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
//                    string UploadAddress = string.Empty;
//                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//                    if (UploadAddress != null && UploadAddress != string.Empty)
//                    {
//                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//                    }
//                    xmlOutput1 = xmlOutput;
//                }
//                ilstFinalResult = insertList;

//            }

//            if (UpdateList != null && UpdateList.Count > 0)
//            {
//                for (int i = 0; i < UpdateList.Count; i++)
//                {
//                    FillRequiredInfo("send_medication");
//                    xmlWriter.WriteStartElement("MedicationList");
//                    xmlWriter.WriteStartElement("Medication");
//                    xmlWriter.WriteElementString("Deleted", "n");
//                    xmlWriter.WriteElementString("RcopiaID", UpdateList[i].Id.ToString());
//                    xmlWriter.WriteElementString("ExternalID", UpdateList[i].Id.ToString());
//                    xmlWriter.WriteStartElement("Patient");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("ExternalID", UpdateList[i].Human_ID.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Provider");
//                    xmlWriter.WriteElementString("Username", UpdateList[i].Last_Modified_By);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Sig");
//                    xmlWriter.WriteStartElement("Drug");
//                    xmlWriter.WriteElementString("NDCID", UpdateList[i].NDC_ID);
//                    xmlWriter.WriteElementString("BrandName", UpdateList[i].Brand_Name);
//                    xmlWriter.WriteElementString("GenericName", UpdateList[i].Generic_Name);
//                    xmlWriter.WriteElementString("DoseOther", UpdateList[i].Dose_Other);
//                    xmlWriter.WriteElementString("Route", UpdateList[i].Route);
//                    xmlWriter.WriteElementString("Form", UpdateList[i].Dose_Unit);
//                    xmlWriter.WriteElementString("Strength", UpdateList[i].Strength);

//                    xmlWriter.WriteEndElement();
//                    //
//                    xmlWriter.WriteElementString("Route", UpdateList[i].Route);
//                    ////
//                    xmlWriter.WriteElementString("Dose", UpdateList[i].Dose);

//                    xmlWriter.WriteElementString("DoseUnit", UpdateList[i].Dose_Unit);

//                    xmlWriter.WriteElementString("DoseTiming", UpdateList[i].Dose_Timing);
//                    xmlWriter.WriteElementString("PatientNotes", UpdateList[i].Patient_Notes);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteElementString("StartDate", UpdateList[i].Start_Date.ToString());
//                    xmlWriter.WriteElementString("StopDate", UpdateList[i].Stop_Date.ToString());
//                    xmlWriter.WriteElementString("FillDate", UpdateList[i].Fill_Date.ToString());
//                    xmlWriter.WriteElementString("StopReason", UpdateList[i].Stop_Reason);
//                    xmlWriter.WriteElementString("RxnormID", UpdateList[i].Rxnorm_ID.ToString());
//                    xmlWriter.WriteElementString("RxnormType", UpdateList[i].Rxnorm_ID_Type.ToString());

//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.Flush();

//                    Byte[] buffer = new Byte[ms.Length];
//                    buffer = ms.ToArray();
//                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//                    string sContent = xmlOutput.ToString();
//                    sContent = sContent.Trim();
//                    sContent = sContent.Replace("\n", "");
//                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
//                    string UploadAddress = string.Empty;
//                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//                    if (UploadAddress != null && UploadAddress != string.Empty)
//                    {
//                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//                    }
//                    xmlOutput1 = xmlOutput;
//                }
//                ilstFinalResult = UpdateList;

//            }







//            return xmlOutput1;
//        }


//        public string creteSendAllergyXml(IList<Rcopia_Allergy> insertList, IList<Rcopia_Allergy> UpdateList, IList<Rcopia_Allergy> deleteList)
//        {
//            IList<Rcopia_Allergy> ilstFinalResult = new List<Rcopia_Allergy>();

//            //if (objRcopSettings == null)
//            //{
//            //    return string.Empty;
//            //}
//            string xmlOutput1 = string.Empty;


//            if (deleteList != null && deleteList.Count > 0)
//            {
//                for (int i = 0; i < deleteList.Count; i++)
//                {
//                    FillRequiredInfo("send_allergy");
//                    xmlWriter.WriteStartElement("AllergyList");
//                    xmlWriter.WriteStartElement("Allergy");
//                    xmlWriter.WriteElementString("ExternalID", deleteList[i].Id.ToString());
//                    xmlWriter.WriteElementString("RcopiaID", deleteList[i].Id.ToString());
//                    xmlWriter.WriteElementString("Deleted", "y");
//                    xmlWriter.WriteStartElement("Status");
//                    xmlWriter.WriteElementString("Deleted", "");
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Patient");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("ExternalID", deleteList[i].Human_ID.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Allergen");
//                    xmlWriter.WriteElementString("Name", deleteList[i].Allergy_Name);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Drug");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("NDCID", deleteList[i].NDC_ID);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Group");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteEndElement();

//                    xmlWriter.WriteElementString("Reaction", deleteList[i].Reaction);
//                    xmlWriter.WriteElementString("OnsetDate", deleteList[i].OnsetDate.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    // Close the document
//                    xmlWriter.WriteEndDocument();
//                    // Flush the write
//                    xmlWriter.Flush();


//                    Byte[] buffer = new Byte[ms.Length];
//                    buffer = ms.ToArray();
//                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//                    string sContent = xmlOutput.ToString();
//                    sContent = sContent.Trim();
//                    sContent = sContent.Replace("\n", "");
//                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
//                    string UploadAddress = string.Empty;
//                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//                    if (UploadAddress != null && UploadAddress != string.Empty)
//                    {
//                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//                    }
//                    xmlOutput1 = xmlOutput;
//                }
//                ilstFinalResult = deleteList;
//            }

//            if (insertList != null && insertList.Count > 0)
//            {
//                for (int i = 0; i < insertList.Count; i++)
//                {
//                    FillRequiredInfo("send_allergy");
//                    xmlWriter.WriteStartElement("AllergyList");
//                    xmlWriter.WriteStartElement("Allergy");
//                    xmlWriter.WriteElementString("ExternalID", "");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("Deleted", "n");
//                    xmlWriter.WriteStartElement("Status");
//                    xmlWriter.WriteElementString("Active", "");
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Patient");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("ExternalID", insertList[i].Human_ID.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Allergen");
//                    xmlWriter.WriteElementString("Name", insertList[i].Allergy_Name);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Drug");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("NDCID", insertList[i].NDC_ID);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Group");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteEndElement();
//                    // xmlWriter.WriteEndElement();
//                    xmlWriter.WriteElementString("Reaction", insertList[i].Reaction);

//                    xmlWriter.WriteElementString("OnsetDate", insertList[i].OnsetDate.ToString());
//                    // xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    // Close the document
//                    xmlWriter.WriteEndDocument();
//                    // Flush the write
//                    xmlWriter.Flush();

//                    Page page = new Page();
//                    page.Session["sRocpiaValue"] = insertList[i].Status;
//                    Byte[] buffer = new Byte[ms.Length];
//                    buffer = ms.ToArray();
//                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//                    string sContent = xmlOutput.ToString();
//                    sContent = sContent.Trim();
//                    sContent = sContent.Replace("\n", "");
//                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
//                    string UploadAddress = string.Empty;
//                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//                    if (UploadAddress != null && UploadAddress != string.Empty)
//                    {
//                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//                    }
//                    xmlOutput1 = xmlOutput;

//                    if (insertList[i].Status.ToUpper() == "INACTIVE")
//                    {
//                        IList<Rcopia_Allergy> DelRcopiaList = new List<Rcopia_Allergy>();
//                        if (page.Session["sRCOPIAID"] != null)
//                        {
//                            page.Session.Remove("sRocpiaValue");
//                            insertList[i].Id = Convert.ToUInt32(page.Session["sRCOPIAID"].ToString());
//                            DelRcopiaList.Add(insertList[i]);
//                            DeleteRcopiaList(DelRcopiaList);
//                        }


//                        // deleteList = insertList.Where(a=>a.Status.ToUpper()=="INACTIVE").ToList<Rcopia_Allergy>();
//                    }
//                }
//                ilstFinalResult = insertList;

//            }

//            if (UpdateList != null && UpdateList.Count > 0)
//            {
//                for (int i = 0; i < UpdateList.Count; i++)
//                {
//                    FillRequiredInfo("send_allergy");
//                    xmlWriter.WriteStartElement("AllergyList");
//                    xmlWriter.WriteStartElement("Allergy");
//                    xmlWriter.WriteElementString("ExternalID", UpdateList[i].Id.ToString());
//                    xmlWriter.WriteElementString("RcopiaID", UpdateList[i].Id.ToString());
//                    xmlWriter.WriteElementString("Deleted", "n");
//                    xmlWriter.WriteStartElement("Patient");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("ExternalID", UpdateList[i].Human_ID.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Allergen");
//                    xmlWriter.WriteElementString("Name", UpdateList[i].Allergy_Name);
//                    xmlWriter.WriteEndElement();

//                    xmlWriter.WriteStartElement("Drug");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("NDCID", UpdateList[i].NDC_ID);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Group");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteEndElement();


//                    // xmlWriter.WriteEndElement();
//                    xmlWriter.WriteElementString("Reaction", UpdateList[i].Reaction);
//                    xmlWriter.WriteStartElement("Status");
//                    xmlWriter.WriteElementString("Active", "");
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteElementString("OnsetDate", UpdateList[i].OnsetDate.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    // Close the document
//                    xmlWriter.WriteEndDocument();
//                    // Flush the write
//                    xmlWriter.Flush();


//                    Byte[] buffer = new Byte[ms.Length];
//                    buffer = ms.ToArray();
//                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//                    string sContent = xmlOutput.ToString();
//                    sContent = sContent.Trim();
//                    sContent = sContent.Replace("\n", "");
//                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
//                    string UploadAddress = string.Empty;
//                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//                    if (UploadAddress != null && UploadAddress != string.Empty)
//                    {
//                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//                    }
//                    xmlOutput1 = xmlOutput;
//                }
//                ilstFinalResult = UpdateList;
//            }



//            return xmlOutput1;
//        }


//        private void DeleteRcopiaList(IList<Rcopia_Allergy> DeleteListRcopia)
//        {

//            if (DeleteListRcopia != null && DeleteListRcopia.Count > 0)
//            {
//                for (int i = 0; i < DeleteListRcopia.Count; i++)
//                {
//                    FillRequiredInfo("send_allergy");
//                    xmlWriter.WriteStartElement("AllergyList");
//                    xmlWriter.WriteStartElement("Allergy");
//                    xmlWriter.WriteElementString("ExternalID", DeleteListRcopia[i].Id.ToString());
//                    xmlWriter.WriteElementString("RcopiaID", DeleteListRcopia[i].Id.ToString());
//                    xmlWriter.WriteElementString("Deleted", "y");
//                    xmlWriter.WriteStartElement("Patient");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("ExternalID", DeleteListRcopia[i].Human_ID.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Allergen");
//                    xmlWriter.WriteElementString("Name", DeleteListRcopia[i].Allergy_Name);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Drug");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteElementString("NDCID", DeleteListRcopia[i].NDC_ID);
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteStartElement("Group");
//                    xmlWriter.WriteElementString("RcopiaID", "");
//                    xmlWriter.WriteEndElement();

//                    xmlWriter.WriteElementString("Reaction", DeleteListRcopia[i].Reaction);
//                    xmlWriter.WriteElementString("OnsetDate", DeleteListRcopia[i].OnsetDate.ToString());
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    xmlWriter.WriteEndElement();
//                    // Close the document
//                    xmlWriter.WriteEndDocument();
//                    // Flush the write
//                    xmlWriter.Flush();


//                    Byte[] buffer = new Byte[ms.Length];
//                    buffer = ms.ToArray();
//                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
//                    string sContent = xmlOutput.ToString();
//                    sContent = sContent.Trim();
//                    sContent = sContent.Replace("\n", "");
//                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
//                    string UploadAddress = string.Empty;
//                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//                    if (UploadAddress != null && UploadAddress != string.Empty)
//                    {
//                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
//                    }
//                    // xmlOutput1 = xmlOutput;
//                }
//                // ilstFinalResult = deleteList;
//            }

//        }



//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.IO;
using System.Web.UI;

namespace Acurus.Capella.DataAccess
{
    public partial class RCopiaGenerateXML
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlWriterSettings wSettings = new XmlWriterSettings();
        MemoryStream ms;
        XmlWriter xmlWriter;
        IList<Rcopia_Settings> ilstRcopSett;
        Rcopia_Settings objRcopSettings;



        public string CreateGetURLXML(string sLegalOrg)
        {
            ms = new MemoryStream();
            wSettings.Indent = true;
            xmlWriter = XmlWriter.Create(ms, wSettings);
            xmlWriter.WriteStartDocument();
            Rcopia_SettingsManager rcopiaSettingMngr = new Rcopia_SettingsManager();
            ilstRcopSett = rcopiaSettingMngr.GetRcopia_Settings(sLegalOrg);
            if (ilstRcopSett.Count > 0)
            {
                objRcopSettings = (from g in ilstRcopSett where g.Command == "get_url" && g.Legal_Org == sLegalOrg select g).ToList<Rcopia_Settings>()[0];
                xmlWriter.WriteStartElement("RCExtRequest");
                xmlWriter.WriteAttributeString("version", "2.35");//changed version from 2.13 to 2.19 as per Dr.First's instructions[RajeevMotwani] as Assessment data were not getting uploaded(under Problem section) to Dr.First
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
                xmlWriter.WriteElementString("RcopiaPracticeUsername", objRcopSettings.Rcopia_Practice_User_name);
                xmlWriter.WriteStartElement("Request");
                xmlWriter.WriteElementString("Command", "get_url");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }
            Byte[] buffer = new Byte[ms.Length];
            buffer.DefaultIfEmpty();
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
            return xmlOutput;
        }

        public string CreateUpdatePatientXML(string sLegalOrg)
        {
            FillRequiredInfo("update_patient",sLegalOrg);

            Rcopia_Update_InfoManager rcopiaUpdateMngr = new Rcopia_Update_InfoManager();
            string ilstRcopUpdateInfoDate = rcopiaUpdateMngr.GetRcopiaUpdateInfoCommandName("update_patient");
            if (ilstRcopUpdateInfoDate != "")
            {

                //Rcopia_Update_info objupdateInfo = (from j in ilstRcopUpdateInfo where j.Command == "update_patient" select j).ToList<Rcopia_Update_info>()[0];
                xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
            }
            xmlWriter.WriteElementString("IncludeAliasPatients", "y");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

            return xmlOutput;
        }

        public string CreateUpdatePatientXMLforSinglePatient(string sHuman_ID, string sLegalOrg)
        {
            FillRequiredInfo("update_patient",sLegalOrg);

            Rcopia_Update_InfoManager rcopiaUpdateMngr = new Rcopia_Update_InfoManager();
            string ilstRcopUpdateInfoDate = rcopiaUpdateMngr.GetRcopiaUpdateInfoCommandName("update_patient");
            if (ilstRcopUpdateInfoDate != "")
            {

                //Rcopia_Update_info objupdateInfo = (from j in ilstRcopUpdateInfo where j.Command == "update_patient" select j).ToList<Rcopia_Update_info>()[0];
                //xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
            }
            xmlWriter.WriteElementString("IncludeAliasPatients", "y");
            xmlWriter.WriteStartElement("PatientList");
            xmlWriter.WriteStartElement("Patient");
            xmlWriter.WriteElementString("RcopiaID", string.Empty);
            xmlWriter.WriteElementString("ExternalID", sHuman_ID);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

            return xmlOutput;
        }

        //Old CreateUpdatePrescriptionXML
        //public string CreateUpdatePrescriptionXML()
        //{
        //    FillRequiredInfo("update_prescription");
        //    Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
        //    string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_prescription");
        //    if (ilstRcopUpdateInfoDate != "")
        //        xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
        //    xmlWriter.WriteStartElement("Patient");
        //    xmlWriter.WriteElementString("RcopiaID", string.Empty);
        //    xmlWriter.WriteElementString("ExternalID", string.Empty);
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteElementString("Status", "all");
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteEndDocument();
        //    xmlWriter.Flush();
        //    Byte[] buffer = new Byte[ms.Length];
        //    buffer = ms.ToArray();
        //    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
        //    return xmlOutput;

        //}

        //RCopia Patient Level Download - Commented
        public string CreateUpdatePrescriptionXML(ulong ulHumanID, DateTime dtRCopia_Prescription_Last_Updated_Date_and_Time, string sLegalOrg, DateTime dtHumanCreatedDateTime)
        {
            if (ulHumanID == 0)
                return string.Empty;

            FillRequiredInfo("update_prescription",sLegalOrg);
            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();

            //string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_prescription");
            //if (ilstRcopUpdateInfoDate != "")
            //    xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));

            if (dtRCopia_Prescription_Last_Updated_Date_and_Time != DateTime.MinValue)
                xmlWriter.WriteElementString("LastUpdateDate", dtRCopia_Prescription_Last_Updated_Date_and_Time.ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            //Jira CAP-1563
            //else
            //    xmlWriter.WriteElementString("LastUpdateDate", DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            else if (dtHumanCreatedDateTime != DateTime.MinValue)
                xmlWriter.WriteElementString("LastUpdateDate", dtHumanCreatedDateTime.AddDays(-10).ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            else
                xmlWriter.WriteElementString("LastUpdateDate", "01/01/2020 12:00:00");
            xmlWriter.WriteStartElement("Patient");
            xmlWriter.WriteElementString("RcopiaID", string.Empty);
            //xmlWriter.WriteElementString("ExternalID", string.Empty);
            xmlWriter.WriteElementString("ExternalID", ulHumanID.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteElementString("Status", "all");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

            return xmlOutput;

        }

        //Old CreateUpdateMedicationXML
        //public string CreateUpdateMedicationXML()
        //{
        //    FillRequiredInfo("update_medication");
        //    Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
        //    //IList<Rcopia_Update_info> ilstRcopUpdateInfo = rcopiaUpdatemngr.GetRcopiaUpdateInfo();
        //    string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_medication");
        //    if (ilstRcopUpdateInfoDate != "")
        //        xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));

        //    xmlWriter.WriteStartElement("Patient");
        //    xmlWriter.WriteElementString("RcopiaID", string.Empty);
        //    xmlWriter.WriteElementString("ExternalID", string.Empty);
        //    xmlWriter.WriteEndElement();
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

        //RCopia Patient Level Download - Commented
        public string CreateUpdateMedicationXML(ulong ulHumanID, DateTime dtRCopia_Medication_Last_Updated_Date_and_Time, string sLegalOrg, DateTime dtHumanCreatedDateTime)
        {
            if (ulHumanID == 0)
                return string.Empty;

            FillRequiredInfo("update_medication",sLegalOrg);
            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
            //IList<Rcopia_Update_info> ilstRcopUpdateInfo = rcopiaUpdatemngr.GetRcopiaUpdateInfo();

            //string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_medication");
            //if (ilstRcopUpdateInfoDate != "")
            //    xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));

            if (dtRCopia_Medication_Last_Updated_Date_and_Time != DateTime.MinValue)
                xmlWriter.WriteElementString("LastUpdateDate", dtRCopia_Medication_Last_Updated_Date_and_Time.ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            //Jira CAP-1563
            //else
            //    xmlWriter.WriteElementString("LastUpdateDate", DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            else if (dtHumanCreatedDateTime != DateTime.MinValue)
                xmlWriter.WriteElementString("LastUpdateDate", dtHumanCreatedDateTime.AddDays(-10).ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            else
                xmlWriter.WriteElementString("LastUpdateDate", "01/01/2020 12:00:00");
            //if (ilstRcopUpdateInfo.Count > 0)
            //{
            //    Rcopia_Update_info objupdateInfo = (from j in ilstRcopUpdateInfo where j.Command == "update_medication" select j).ToList<Rcopia_Update_info>()[0];
            //    xmlWriter.WriteElementString("LastUpdateDate", objupdateInfo.Last_Updated_Date_Time.ToString("MM/dd/yyyy hh:mm:ss").Replace("-", "/"));
            //}
            xmlWriter.WriteStartElement("Patient");
            xmlWriter.WriteElementString("RcopiaID", string.Empty);
            //xmlWriter.WriteElementString("ExternalID", string.Empty);
            xmlWriter.WriteElementString("ExternalID", ulHumanID.ToString());
            xmlWriter.WriteEndElement();
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

        public string CreateUpdateProblemXML(string sLegalOrg)
        {

            FillRequiredInfo("update_problem",sLegalOrg);
            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
            string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_problem");
            if (ilstRcopUpdateInfoDate != "")
                xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
            xmlWriter.WriteStartElement("Patient");
            xmlWriter.WriteElementString("RcopiaID", string.Empty);
            xmlWriter.WriteElementString("ExternalID", string.Empty);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

            return xmlOutput;
        }

        //Old CreateUpdateAllergyXML
        //public string CreateUpdateAllergyXML()
        //{
        //    FillRequiredInfo("update_allergy");
        //    Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
        //    string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_allergy");
        //    if (ilstRcopUpdateInfoDate != "")
        //        xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
        //    xmlWriter.WriteElementString("ReturnAllNDCIDs", "y");
        //    xmlWriter.WriteStartElement("Patient");
        //    xmlWriter.WriteElementString("RcopiaID", string.Empty);
        //    xmlWriter.WriteElementString("ExternalID", string.Empty);
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteEndElement();
        //    xmlWriter.WriteEndDocument();
        //    xmlWriter.Flush();
        //    Byte[] buffer = new Byte[ms.Length];
        //    buffer = ms.ToArray();
        //    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
        //    return xmlOutput;
        //}

        //Rcopia Patient Level Download - Commented


        // new add method 
        public string CreateGetRcopiaEventXML(DateTime dtRCopia_Allergy_Last_Updated_Date_and_Time, string sLegalOrg)
        {
            
            FillRequiredInfo("get_rcopia_event", sLegalOrg);
            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
            //string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_allergy");
            //if (ilstRcopUpdateInfoDate != "")
            //    xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
            if (dtRCopia_Allergy_Last_Updated_Date_and_Time != DateTime.MinValue)
                xmlWriter.WriteElementString("LastUpdateDate", dtRCopia_Allergy_Last_Updated_Date_and_Time.ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            else
                xmlWriter.WriteElementString("LastUpdateDate", DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            xmlWriter.WriteElementString("IncludePatientList", "y");
            xmlWriter.WriteElementString("MaximumLastUpdateDate", string.Empty);
            xmlWriter.WriteStartElement("Patient");
            xmlWriter.WriteElementString("RcopiaID", string.Empty);
            xmlWriter.WriteElementString("ExternalID", string.Empty);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

            return xmlOutput;
        }
        // new method end

        public string CreateUpdateAllergyXML(ulong ulHumanID, DateTime dtRCopia_Allergy_Last_Updated_Date_and_Time, string sLegalOrg , DateTime dtHumanCreatedDateTime)
        {
            if (ulHumanID == 0)
                return string.Empty;

            FillRequiredInfo("update_allergy",sLegalOrg);
            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();
            //string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName("update_allergy");
            //if (ilstRcopUpdateInfoDate != "")
            //    xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
            if (dtRCopia_Allergy_Last_Updated_Date_and_Time != DateTime.MinValue)
                xmlWriter.WriteElementString("LastUpdateDate", dtRCopia_Allergy_Last_Updated_Date_and_Time.ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            //Jira CAP-1563
            //else
            //    xmlWriter.WriteElementString("LastUpdateDate", DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            else if (dtHumanCreatedDateTime != DateTime.MinValue)
                xmlWriter.WriteElementString("LastUpdateDate", dtHumanCreatedDateTime.AddDays(-10).ToString("MM/dd/yyyy HH:mm:ss").Replace("-", "/"));
            else
                xmlWriter.WriteElementString("LastUpdateDate", "01/01/2020 12:00:00");
            xmlWriter.WriteElementString("ReturnAllNDCIDs", "y");
            xmlWriter.WriteStartElement("Patient");
            xmlWriter.WriteElementString("RcopiaID", string.Empty);
            //xmlWriter.WriteElementString("ExternalID", string.Empty);
            xmlWriter.WriteElementString("ExternalID", ulHumanID.ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);

            return xmlOutput;
        }
        //for medication review status

        public string CreateGetReviewStatusXML(string sRequest, DateTime dtLastDate, string sLegalOrg)
        {
            FillRequiredInfo(sRequest,sLegalOrg);

            Rcopia_Update_InfoManager rcopiaUpdatemngr = new Rcopia_Update_InfoManager();

            string ilstRcopUpdateInfoDate = rcopiaUpdatemngr.GetRcopiaUpdateInfoCommandName(sRequest);
            if (ilstRcopUpdateInfoDate != "")
                xmlWriter.WriteElementString("LastUpdateDate", ilstRcopUpdateInfoDate.Replace("-", "/"));
            //xmlWriter.WriteStartElement("PatientList");
            //xmlWriter.WriteStartElement("Patient");
            //xmlWriter.WriteStartElement("RcopiaID");
            //xmlWriter.WriteEndElement();
            //xmlWriter.WriteStartElement("ExternalID");
            //xmlWriter.WriteEndElement();
            //xmlWriter.WriteEndElement();
            //xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("MaximumReviews");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            Byte[] buffer = new Byte[ms.Length];
            buffer.DefaultIfEmpty();
            buffer = ms.ToArray();
            string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
            return xmlOutput;
        }
        public void FillRequiredInfo(string sXMLName, string sLegalOrg)
        {
            ms = new MemoryStream();
            wSettings.Indent = true;
            xmlWriter = XmlWriter.Create(ms, wSettings);
            xmlWriter.WriteStartDocument();

            Rcopia_SettingsManager rcopiaSettingsMngr = new Rcopia_SettingsManager();
            ilstRcopSett = rcopiaSettingsMngr.GetRcopia_Settings(sLegalOrg);
            if (ilstRcopSett.Count > 0)
            {

                objRcopSettings = (from g in ilstRcopSett where g.Command == sXMLName && g.Legal_Org == sLegalOrg select g).ToList<Rcopia_Settings>()[0];
                if (sXMLName == "get_review_status" || sXMLName == "update_patient_office_visits")
                {
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
                        if (sreturn != string.Empty && sreturn != "ExternalID")
                        {
                            PropertyInfo o = objRcopSettings.GetType().GetProperty(propertyInfo.Name);
                            var v = (o.GetValue(objRcopSettings, null));
                            xmlWriter.WriteElementString(sreturn, v.ToString());
                        }
                    }
                }
                else
                {

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

        public string creteSendMedicationXml(IList<Rcopia_Medication> insertList, IList<Rcopia_Medication> UpdateList, IList<Rcopia_Medication> deleteList, string sLegalOrg)
        {
            IList<Rcopia_Medication> ilstFinalResult = new List<Rcopia_Medication>();
            string xmlOutput1 = string.Empty;
            //if (objRcopSettings == null)
            //{
            //    return string.Empty;
            //}
            if (deleteList != null && deleteList.Count > 0)
            {
                for (int i = 0; i < deleteList.Count; i++)
                {
                    FillRequiredInfo("send_medication",sLegalOrg);
                    xmlWriter.WriteStartElement("MedicationList");
                    xmlWriter.WriteStartElement("Medication");
                    xmlWriter.WriteElementString("Deleted", "y");
                    xmlWriter.WriteElementString("RcopiaID", deleteList[i].Id.ToString());
                    xmlWriter.WriteElementString("ExternalID", deleteList[i].Id.ToString());
                    xmlWriter.WriteStartElement("Patient");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("ExternalID", deleteList[i].Human_ID.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Provider");
                    xmlWriter.WriteElementString("Username", deleteList[i].Last_Modified_By);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Sig");
                    xmlWriter.WriteStartElement("Drug");
                    xmlWriter.WriteElementString("NDCID", deleteList[i].NDC_ID);
                    xmlWriter.WriteElementString("BrandName", deleteList[i].Brand_Name);
                    xmlWriter.WriteElementString("GenericName", deleteList[i].Generic_Name);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("PatientNotes", deleteList[i].Patient_Notes);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("DoseOther", deleteList[i].Dose_Other);
                    xmlWriter.WriteElementString("StartDate", deleteList[i].Start_Date.ToString());
                    xmlWriter.WriteElementString("StopDate", deleteList[i].Stop_Date.ToString());
                    xmlWriter.WriteElementString("FillDate", deleteList[i].Fill_Date.ToString());
                    xmlWriter.WriteElementString("StopReason", deleteList[i].Stop_Reason);
                    xmlWriter.WriteElementString("RxnormID", deleteList[i].Rxnorm_ID.ToString());
                    xmlWriter.WriteElementString("RxnormType", deleteList[i].Rxnorm_ID_Type.ToString());

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();


                    Byte[] buffer = new Byte[ms.Length];
                    buffer = ms.ToArray();
                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
                    string sContent = xmlOutput.ToString();
                    sContent = sContent.Trim();
                    sContent = sContent.Replace("\n", "");
                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                    string UploadAddress = string.Empty;
                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
                    if (UploadAddress != null && UploadAddress != string.Empty)
                    {
                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
                    }
                    xmlOutput1 = xmlOutput;

                }
                ilstFinalResult = deleteList;

            }

            if (insertList != null && insertList.Count > 0)
            {
                for (int i = 0; i < insertList.Count; i++)
                {
                    FillRequiredInfo("send_medication",sLegalOrg);
                    xmlWriter.WriteStartElement("MedicationList");
                    xmlWriter.WriteStartElement("Medication");
                    xmlWriter.WriteElementString("Deleted", "n");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("ExternalID", "");
                    xmlWriter.WriteStartElement("Patient");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("ExternalID", insertList[i].Human_ID.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Provider");
                    xmlWriter.WriteElementString("Username", insertList[i].Last_Modified_By);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Sig");
                    xmlWriter.WriteStartElement("Drug");
                    xmlWriter.WriteElementString("NDCID", insertList[i].NDC_ID);
                    xmlWriter.WriteElementString("BrandName", insertList[i].Brand_Name);
                    xmlWriter.WriteElementString("GenericName", insertList[i].Generic_Name);
                    xmlWriter.WriteElementString("Route", insertList[i].Route);
                    xmlWriter.WriteElementString("Form", insertList[i].Dose_Unit);
                    xmlWriter.WriteElementString("Strength", insertList[i].Strength);
                    xmlWriter.WriteEndElement();



                    //

                    xmlWriter.WriteElementString("Route", insertList[i].Route);
                    //xmlWriter.WriteElementString("Strength", insertList[i].Strength);
                    //
                    xmlWriter.WriteElementString("Dose", insertList[i].Dose);
                    xmlWriter.WriteElementString("DoseOther", insertList[i].Dose_Other);

                    xmlWriter.WriteElementString("DoseUnit", insertList[i].Dose_Unit);

                    xmlWriter.WriteElementString("DoseTiming", insertList[i].Dose_Timing);

                    xmlWriter.WriteElementString("PatientNotes", insertList[i].Patient_Notes);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("StartDate", insertList[i].Start_Date.ToString());
                    xmlWriter.WriteElementString("StopDate", insertList[i].Stop_Date.ToString());
                    xmlWriter.WriteElementString("FillDate", insertList[i].Fill_Date.ToString());
                    xmlWriter.WriteElementString("StopReason", insertList[i].Stop_Reason);
                    xmlWriter.WriteElementString("RxnormID", insertList[i].Rxnorm_ID.ToString());
                    xmlWriter.WriteElementString("RxnormType", insertList[i].Rxnorm_ID_Type.ToString());

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    Byte[] buffer = new Byte[ms.Length];
                    buffer = ms.ToArray();
                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
                    string sContent = xmlOutput.ToString();
                    //
                    sContent = sContent.Replace("&lt;", "<");
                    sContent = sContent.Replace("&gt;", ">");
                    //
                    sContent = sContent.Trim();
                    sContent = sContent.Replace("\n", "");
                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                    string UploadAddress = string.Empty;
                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
                    if (UploadAddress != null && UploadAddress != string.Empty)
                    {
                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
                    }
                    xmlOutput1 = xmlOutput;
                }
                ilstFinalResult = insertList;

            }

            if (UpdateList != null && UpdateList.Count > 0)
            {
                for (int i = 0; i < UpdateList.Count; i++)
                {
                    FillRequiredInfo("send_medication",sLegalOrg);
                    xmlWriter.WriteStartElement("MedicationList");
                    xmlWriter.WriteStartElement("Medication");
                    xmlWriter.WriteElementString("Deleted", "n");
                    xmlWriter.WriteElementString("RcopiaID", UpdateList[i].Id.ToString());
                    xmlWriter.WriteElementString("ExternalID", UpdateList[i].Id.ToString());
                    xmlWriter.WriteStartElement("Patient");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("ExternalID", UpdateList[i].Human_ID.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Provider");
                    xmlWriter.WriteElementString("Username", UpdateList[i].Last_Modified_By);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Sig");
                    xmlWriter.WriteStartElement("Drug");
                    xmlWriter.WriteElementString("NDCID", UpdateList[i].NDC_ID);
                    xmlWriter.WriteElementString("BrandName", UpdateList[i].Brand_Name);
                    xmlWriter.WriteElementString("GenericName", UpdateList[i].Generic_Name);
                    xmlWriter.WriteElementString("DoseOther", UpdateList[i].Dose_Other);
                    xmlWriter.WriteElementString("Route", UpdateList[i].Route);
                    xmlWriter.WriteElementString("Form", UpdateList[i].Dose_Unit);
                    xmlWriter.WriteElementString("Strength", UpdateList[i].Strength);

                    xmlWriter.WriteEndElement();
                    //
                    xmlWriter.WriteElementString("Route", UpdateList[i].Route);
                    ////
                    xmlWriter.WriteElementString("Dose", UpdateList[i].Dose);

                    xmlWriter.WriteElementString("DoseUnit", UpdateList[i].Dose_Unit);

                    xmlWriter.WriteElementString("DoseTiming", UpdateList[i].Dose_Timing);
                    xmlWriter.WriteElementString("PatientNotes", UpdateList[i].Patient_Notes);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("StartDate", UpdateList[i].Start_Date.ToString());
                    xmlWriter.WriteElementString("StopDate", UpdateList[i].Stop_Date.ToString());
                    xmlWriter.WriteElementString("FillDate", UpdateList[i].Fill_Date.ToString());
                    xmlWriter.WriteElementString("StopReason", UpdateList[i].Stop_Reason);
                    xmlWriter.WriteElementString("RxnormID", UpdateList[i].Rxnorm_ID.ToString());
                    xmlWriter.WriteElementString("RxnormType", UpdateList[i].Rxnorm_ID_Type.ToString());

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    Byte[] buffer = new Byte[ms.Length];
                    buffer = ms.ToArray();
                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
                    string sContent = xmlOutput.ToString();
                    sContent = sContent.Trim();
                    sContent = sContent.Replace("\n", "");
                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                    string UploadAddress = string.Empty;
                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
                    if (UploadAddress != null && UploadAddress != string.Empty)
                    {
                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
                    }
                    xmlOutput1 = xmlOutput;
                }
                ilstFinalResult = UpdateList;

            }







            return xmlOutput1;
        }


        public string creteSendAllergyXml(IList<Rcopia_Allergy> insertList, IList<Rcopia_Allergy> UpdateList, IList<Rcopia_Allergy> deleteList, string sLegalOrg)
        {
            IList<Rcopia_Allergy> ilstFinalResult = new List<Rcopia_Allergy>();

            //if (objRcopSettings == null)
            //{
            //    return string.Empty;
            //}
            string xmlOutput1 = string.Empty;


            if (deleteList != null && deleteList.Count > 0)
            {
                for (int i = 0; i < deleteList.Count; i++)
                {
                    FillRequiredInfo("send_allergy",sLegalOrg);
                    xmlWriter.WriteStartElement("AllergyList");
                    xmlWriter.WriteStartElement("Allergy");
                    xmlWriter.WriteElementString("ExternalID", deleteList[i].Id.ToString());
                    xmlWriter.WriteElementString("RcopiaID", deleteList[i].Id.ToString());
                    xmlWriter.WriteElementString("Deleted", "y");
                    xmlWriter.WriteStartElement("Status");
                    xmlWriter.WriteElementString("Deleted", "");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Severity");
                    xmlWriter.WriteElementString("Description", insertList[i].Severity);
                    xmlWriter.WriteElementString("SNOMED-CTConceptID", insertList[i].Severity_Snomed_Code);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Patient");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("ExternalID", deleteList[i].Human_ID.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Allergen");
                    xmlWriter.WriteElementString("Name", deleteList[i].Allergy_Name);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Drug");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("NDCID", deleteList[i].NDC_ID);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Group");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteElementString("Reaction", deleteList[i].Reaction);
                    xmlWriter.WriteElementString("OnsetDate", deleteList[i].OnsetDate.ToString());
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
                    string sContent = xmlOutput.ToString();
                    sContent = sContent.Trim();
                    sContent = sContent.Replace("\n", "");
                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                    string UploadAddress = string.Empty;
                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
                    if (UploadAddress != null && UploadAddress != string.Empty)
                    {
                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
                    }
                    xmlOutput1 = xmlOutput;
                }
                ilstFinalResult = deleteList;
            }

            if (insertList != null && insertList.Count > 0)
            {
                for (int i = 0; i < insertList.Count; i++)
                {
                    FillRequiredInfo("send_allergy",sLegalOrg);
                    xmlWriter.WriteStartElement("AllergyList");
                    xmlWriter.WriteStartElement("Allergy");
                    xmlWriter.WriteElementString("ExternalID", "");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("Deleted", "n");
                    xmlWriter.WriteStartElement("Status");
                    xmlWriter.WriteElementString("Active", "");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Severity");
                    xmlWriter.WriteElementString("Description", insertList[i].Severity);
                    xmlWriter.WriteElementString("SNOMED-CTConceptID", insertList[i].Severity_Snomed_Code);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Patient");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("ExternalID", insertList[i].Human_ID.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Allergen");
                    xmlWriter.WriteElementString("Name", insertList[i].Allergy_Name);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Drug");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("NDCID", insertList[i].NDC_ID);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Group");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteEndElement();
                    // xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("Reaction", insertList[i].Reaction);

                    xmlWriter.WriteElementString("OnsetDate", insertList[i].OnsetDate.ToString());
                    // xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    // Close the document
                    xmlWriter.WriteEndDocument();
                    // Flush the write
                    xmlWriter.Flush();

                    Page page = new Page();
                    page.Session["sRocpiaValue"] = insertList[i].Status;
                    Byte[] buffer = new Byte[ms.Length];
                    buffer = ms.ToArray();
                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
                    string sContent = xmlOutput.ToString();
                    sContent = sContent.Trim();
                    sContent = sContent.Replace("\n", "");
                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                    string UploadAddress = string.Empty;
                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
                    if (UploadAddress != null && UploadAddress != string.Empty)
                    {
                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
                    }
                    xmlOutput1 = xmlOutput;

                    if (insertList[i].Status.ToUpper() == "INACTIVE")
                    {
                        IList<Rcopia_Allergy> DelRcopiaList = new List<Rcopia_Allergy>();
                        if (page.Session["sRCOPIAID"] != null)
                        {
                            page.Session.Remove("sRocpiaValue");
                            insertList[i].Id = Convert.ToUInt32(page.Session["sRCOPIAID"].ToString());
                            DelRcopiaList.Add(insertList[i]);
                            DeleteRcopiaList(DelRcopiaList,sLegalOrg);
                        }


                        // deleteList = insertList.Where(a=>a.Status.ToUpper()=="INACTIVE").ToList<Rcopia_Allergy>();
                    }
                }
                ilstFinalResult = insertList;

            }

            if (UpdateList != null && UpdateList.Count > 0)
            {
                for (int i = 0; i < UpdateList.Count; i++)
                {
                    FillRequiredInfo("send_allergy",sLegalOrg);
                    xmlWriter.WriteStartElement("AllergyList");
                    xmlWriter.WriteStartElement("Allergy");
                    xmlWriter.WriteElementString("ExternalID", UpdateList[i].Id.ToString());
                    xmlWriter.WriteElementString("RcopiaID", UpdateList[i].Id.ToString());
                    xmlWriter.WriteElementString("Deleted", "n");
                    xmlWriter.WriteStartElement("Patient");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("ExternalID", UpdateList[i].Human_ID.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Allergen");
                    xmlWriter.WriteElementString("Name", UpdateList[i].Allergy_Name);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("Drug");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("NDCID", UpdateList[i].NDC_ID);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Group");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteEndElement();


                    // xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("Reaction", UpdateList[i].Reaction);
                    xmlWriter.WriteStartElement("Status");
                    xmlWriter.WriteElementString("Active", "");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Severity");
                    xmlWriter.WriteElementString("Description", insertList[i].Severity);
                    xmlWriter.WriteElementString("SNOMED-CTConceptID", insertList[i].Severity_Snomed_Code);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("OnsetDate", UpdateList[i].OnsetDate.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    // Close the document
                    xmlWriter.WriteEndDocument();
                    // Flush the write
                    xmlWriter.Flush();


                    Byte[] buffer = new Byte[ms.Length];
                    buffer = ms.ToArray();
                    string xmlOutput = System.Text.Encoding.UTF8.GetString(buffer);
                    string sContent = xmlOutput.ToString();
                    sContent = sContent.Trim();
                    sContent = sContent.Replace("\n", "");
                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                    string UploadAddress = string.Empty;
                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
                    if (UploadAddress != null && UploadAddress != string.Empty)
                    {
                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
                    }
                    xmlOutput1 = xmlOutput;
                }
                ilstFinalResult = UpdateList;
            }



            return xmlOutput1;
        }


        private void DeleteRcopiaList(IList<Rcopia_Allergy> DeleteListRcopia, string sLegalOrg)
        {

            if (DeleteListRcopia != null && DeleteListRcopia.Count > 0)
            {
                for (int i = 0; i < DeleteListRcopia.Count; i++)
                {
                    FillRequiredInfo("send_allergy",sLegalOrg);
                    xmlWriter.WriteStartElement("AllergyList");
                    xmlWriter.WriteStartElement("Allergy");
                    xmlWriter.WriteElementString("ExternalID", DeleteListRcopia[i].Id.ToString());
                    xmlWriter.WriteElementString("RcopiaID", DeleteListRcopia[i].Id.ToString());
                    xmlWriter.WriteElementString("Deleted", "y");
                    xmlWriter.WriteStartElement("Patient");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("ExternalID", DeleteListRcopia[i].Human_ID.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Allergen");
                    xmlWriter.WriteElementString("Name", DeleteListRcopia[i].Allergy_Name);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Drug");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteElementString("NDCID", DeleteListRcopia[i].NDC_ID);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteStartElement("Group");
                    xmlWriter.WriteElementString("RcopiaID", "");
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteElementString("Reaction", DeleteListRcopia[i].Reaction);
                    xmlWriter.WriteElementString("OnsetDate", DeleteListRcopia[i].OnsetDate.ToString());
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
                    string sContent = xmlOutput.ToString();
                    sContent = sContent.Trim();
                    sContent = sContent.Replace("\n", "");
                    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
                    string UploadAddress = string.Empty;
                    UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
                    if (UploadAddress != null && UploadAddress != string.Empty)
                    {
                        rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1);
                    }
                    // xmlOutput1 = xmlOutput;
                }
                // ilstFinalResult = deleteList;
            }

        }



    }
}

