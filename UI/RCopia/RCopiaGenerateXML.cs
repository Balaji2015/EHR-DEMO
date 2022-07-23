//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.Reflection;
//using Acurus.Capella.Core.DomainObjects;
//using System.IO;
//using Acurus.Capella.DataAccess.ManagerObjects;

//namespace Acurus.Capella.UI.RCopia
//{
//    public partial class RCopiaGenerateXML
//    {

//        XmlDocument xmlDoc = new XmlDocument();
//        XmlWriterSettings wSettings = new XmlWriterSettings();
//        MemoryStream ms;
//        XmlWriter xmlWriter;
//        IList<Rcopia_Settings> ilstRcopSett;
//        Rcopia_Settings objRcopSettings;
//        // RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
//        public string CreateGetNotificationCountXML()
//        {
//            FillRequiredInfo("get_notification_count");
//            xmlWriter.WriteElementString("ReturnPrescriptionIDs", "y");
//            xmlWriter.WriteStartElement("Provider");
//            xmlWriter.WriteElementString("Username", ClientSession.RCopiaUserName);
//            //xmlWriter.WriteElementString("Username", "");
//            xmlWriter.WriteEndElement();
//            xmlWriter.WriteElementString("Type", "all");
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

//        public string CreateGetURLXML()
//        {
//            ms = new MemoryStream();
//            wSettings.Indent = true;
//            xmlWriter = XmlWriter.Create(ms, wSettings);
//            xmlWriter.WriteStartDocument();
//            ilstRcopSett = ApplicationObject.RCopiaSettingsList;
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


//        public void FillRequiredInfo(string sXMLName)
//        {


//            ms = new MemoryStream();
//            wSettings.Indent = true;
//            xmlWriter = XmlWriter.Create(ms, wSettings);
//            xmlWriter.WriteStartDocument();

//            ilstRcopSett = ApplicationObject.RCopiaSettingsList;
//            if (ilstRcopSett.Count > 0)
//            {
//                objRcopSettings = (from g in ilstRcopSett where g.Command == sXMLName select g).ToList<Rcopia_Settings>()[0];

//                xmlWriter.WriteStartElement("RCExtRequest");
//                xmlWriter.WriteAttributeString("version", "2.19");//changed version from 2.13 to 2.19 as per Dr.First's instructions[RajeevMotwani] as Assessment data were not getting uploaded(under Problem section) to Dr.First //*              
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

//        public void UpdateToHumanWithoutMail(string Id, string LastName, string FirstName, DateTime DOB, string MI, string sex, string height, string weight, DateTime DOS, string MACAddress)
//        {

//            // RCopiaTransactionManager objRcopiaMngr = new RCopiaTransactionManager();
//            SendPatientToRCopiaforeRX(Id, LastName, FirstName, DOB, MI, sex, height, weight, DOS, MACAddress);
//        }
//        public void SendPatientToRCopiaforeRX(string Id, string LastName, string FirstName, DateTime DOB, string MI, string sex, string height, string weight, DateTime DOS, string sMacAddress)
//        {
//            string xmlDoc = string.Empty;
//            string UploadAddress = string.Empty;

//            xmlDoc = creteSendPatientXmleRx(Id, LastName, FirstName, DOB, MI, sex, height, weight, DOS);

//            //Upload XML to RCopia Server
//            string sContent = xmlDoc.ToString();
//            sContent = sContent.Trim();
//            sContent = sContent.Replace("\n", "");
//            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();



//            UploadAddress = rcopiaSessionMngr.UploadAddress;  //System.Configuration.ConfigurationSettings.AppSettings["RCopiaGetURLAddress"];
//            if (UploadAddress != null && UploadAddress != string.Empty)
//            {
//                if (rcopiaSessionMngr.HttpPost(UploadAddress + sContent, 1) == null)
//                {
//                    //Muthu
//                    HumanManager objmngr = new HumanManager();
//                    IList<Human> ilsthuman = new List<Human>();
//                    ilsthuman = objmngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt32(Id));
//                    ilsthuman[0].Is_Sent_To_Rcopia = "N";
//                    objmngr.UpdateFailedRCopiaHuman(ilsthuman[0], sMacAddress);
//                }
//            }
//        }
//        public string creteSendPatientXmleRx(string Id, string LastName, string FirstName, DateTime DOB, string MI, string sex, string height, string weight, DateTime DOS)
//        {
//            string Weight = string.Empty;
//            string Height = string.Empty;

//            Height = height;
//            Weight = weight;
//            FillRequiredInfo("send_patient");
//            if (objRcopSettings == null)
//            {
//                return string.Empty;
//            }
//            xmlWriter.WriteElementString("Synchronous", objRcopSettings.Synchronous);
//            xmlWriter.WriteElementString("CheckEligibility", objRcopSettings.Check_Eligibilityt);

//            xmlWriter.WriteStartElement("PatientList");
//            xmlWriter.WriteStartElement("Patient");
//            xmlWriter.WriteElementString("Id", Id);
//            xmlWriter.WriteElementString("Last_Name", LastName);
//            xmlWriter.WriteElementString("First_Name", FirstName);
//            xmlWriter.WriteElementString("MI", MI);
//            xmlWriter.WriteElementString("Sex", sex);
//            xmlWriter.WriteElementString("DOB", DOB.ToString("MM/dd/yyyy"));

//            if (Weight == "0")
//                xmlWriter.WriteElementString("Weight", Weight);
//            else
//            {
//                string s3 = Weight.Split(' ')[1];
//                xmlWriter.WriteElementString("Weight", ConvertLbsToKg(s3));
//                xmlWriter.WriteElementString("WeightUnit", "kg");
//            }

//            if (Height == "0")
//                xmlWriter.WriteElementString("Height", Height);
//            else
//            {
//                string s1 = Height.Split(' ')[1];
//                string s2 = Height.Split(' ')[3];

//                Height = ConvertFeetInchToInch(s1, s2);
//                xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
//                xmlWriter.WriteElementString("HeightUnit", "cm");
//            }

//            xmlWriter.WriteElementString("LastVisitDate", DOS.ToString("MM/dd/yyyy"));

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
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using Acurus.Capella.Core.DomainObjects;
using System.IO;
using Acurus.Capella.DataAccess.ManagerObjects;

namespace Acurus.Capella.UI.RCopia
{
    public partial class RCopiaGenerateXML
    {

        XmlDocument xmlDoc = new XmlDocument();
        XmlWriterSettings wSettings = new XmlWriterSettings();
        MemoryStream ms;
        XmlWriter xmlWriter;
        IList<Rcopia_Settings> ilstRcopSett;
        Rcopia_Settings objRcopSettings;
        // RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();
        public string CreateGetNotificationCountXML(string sLegalOrg)
        {
            FillRequiredInfo("get_notification_count",sLegalOrg);
            xmlWriter.WriteElementString("ReturnPrescriptionIDs", "y");
            xmlWriter.WriteStartElement("Provider");
            xmlWriter.WriteElementString("Username", ClientSession.RCopiaUserName);
            //xmlWriter.WriteElementString("Username", "");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteElementString("Type", "all");
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

        public string CreateGetURLXML(string sLegalOrg)
        {
            ms = new MemoryStream();
            wSettings.Indent = true;
            xmlWriter = XmlWriter.Create(ms, wSettings);
            xmlWriter.WriteStartDocument();
            //ilstRcopSett = ApplicationObject.RCopiaSettingsList;
            Rcopia_SettingsManager objRCopiaManager = new Rcopia_SettingsManager();
            ilstRcopSett = objRCopiaManager.GetRcopia_Settings(sLegalOrg);

            //if (ilstRcopSett == null)
            //{
            //    Rcopia_SettingsManager objRCopiaManager = new Rcopia_SettingsManager();
            //    ApplicationObject.RCopiaSettingsList = objRCopiaManager.GetRcopia_Settings();
            //    ilstRcopSett = ApplicationObject.RCopiaSettingsList;
            //}
            if (ilstRcopSett!=null && ilstRcopSett.Count > 0)
            {
                objRcopSettings = (from g in ilstRcopSett where g.Command == "get_url" && g.Legal_Org==sLegalOrg select g).ToList<Rcopia_Settings>()[0];
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


        public void FillRequiredInfo(string sXMLName, string sLegalOrg)
        {


            ms = new MemoryStream();
            wSettings.Indent = true;
            xmlWriter = XmlWriter.Create(ms, wSettings);
            xmlWriter.WriteStartDocument();

            //ilstRcopSett = ApplicationObject.RCopiaSettingsList;
            Rcopia_SettingsManager objRCopiaManager = new Rcopia_SettingsManager();
            ilstRcopSett = objRCopiaManager.GetRcopia_Settings(sLegalOrg);
            //if (ilstRcopSett == null)
            //{
            //    Rcopia_SettingsManager objRCopiaManager = new Rcopia_SettingsManager();
            //    ApplicationObject.RCopiaSettingsList = objRCopiaManager.GetRcopia_Settings();
            //    ilstRcopSett = ApplicationObject.RCopiaSettingsList;
            //}
            if (ilstRcopSett!=null && ilstRcopSett.Count > 0)
            {
                objRcopSettings = (from g in ilstRcopSett where g.Command == sXMLName && g.Legal_Org == sLegalOrg select g).ToList<Rcopia_Settings>()[0];

                xmlWriter.WriteStartElement("RCExtRequest");
                xmlWriter.WriteAttributeString("version", "2.35");//changed version from 2.13 to 2.19 as per Dr.First's instructions[RajeevMotwani] as Assessment data were not getting uploaded(under Problem section) to Dr.First //*              
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

        public string creteSendPatientXmleRx(string Id, string LastName, string FirstName, DateTime DOB, string MI, string sex, string height, string weight, DateTime DOS, string sLegalOrg)
        {
            string Weight = string.Empty;
            string Height = string.Empty;

            Height = height;
            Weight = weight;
            FillRequiredInfo("send_patient",sLegalOrg);
            if (objRcopSettings == null)
            {
                return string.Empty;
            }
            xmlWriter.WriteElementString("Synchronous", objRcopSettings.Synchronous);
            xmlWriter.WriteElementString("CheckEligibility", objRcopSettings.Check_Eligibilityt);

            xmlWriter.WriteStartElement("PatientList");
            xmlWriter.WriteStartElement("Patient");
            xmlWriter.WriteElementString("Id", Id);
            xmlWriter.WriteElementString("Last_Name", LastName);
            xmlWriter.WriteElementString("First_Name", FirstName);
            xmlWriter.WriteElementString("MI", MI);
            xmlWriter.WriteElementString("Sex", sex);
            xmlWriter.WriteElementString("DOB", DOB.ToString("MM/dd/yyyy"));

            if (Weight == "0")
                xmlWriter.WriteElementString("Weight", Weight);
            else
            {
                string s3 = Weight.Split(' ')[1];
                xmlWriter.WriteElementString("Weight", ConvertLbsToKg(s3));
                xmlWriter.WriteElementString("WeightUnit", "kg");
            }

            if (Height == "0")
                xmlWriter.WriteElementString("Height", Height);
            else
            {
                string s1 = Height.Split(' ')[1];
                string s2 = Height.Split(' ')[3];

                Height = ConvertFeetInchToInch(s1, s2);
                xmlWriter.WriteElementString("Height", ConvertInchesToCM(Height));
                xmlWriter.WriteElementString("HeightUnit", "cm");
            }

            xmlWriter.WriteElementString("LastVisitDate", DOS.ToString("MM/dd/yyyy"));

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
    }
}

