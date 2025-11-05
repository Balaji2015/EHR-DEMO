//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using System.Xml;
//using System.Reflection;
//using System.IO;
////using Acurus.Capella.Rcopia;
//using Acurus.Capella.Core.DomainObjects;
//using Acurus.Capella.Core.DTO;
//using Acurus.Capella.DataAccess.ManagerObjects;
//using System.Globalization;

//namespace Acurus.Capella.UI.RCopia
//{
//    public partial class RCopiaXMLResponseProcess
//    {
//        IList<Human> ilstPatient = new List<Human>();
//        IList<Rcopia_Medication> ilstMedication = new List<Rcopia_Medication>();
//        IList<Rcopia_Allergy> ilstAllergy = new List<Rcopia_Allergy>();
//        IList<ProblemList> ilstProblem = new List<ProblemList>();
//        IList<Rcopia_Prescription_List> ilstRcopiaPrescription = new List<Rcopia_Prescription_List>();
//        public static IList<Rcopia_Update_info> ilstRcopupdateInfo = new List<Rcopia_Update_info>();
//        public static IList<Rcopia_NotificationDTO> ilstNotification = new List<Rcopia_NotificationDTO>();

//        ulong MyhumanID = 0;
//        Human objPatient;
//        Human HumanRecord;
//        Rcopia_Medication ObjMedication;
//        Rcopia_Allergy objAllergy;
//        ProblemList objProbList;
//        Rcopia_Prescription_List objPrescription;
//        Rcopia_Update_info objrcopUpdatInfo;
//        Rcopia_NotificationDTO objrcopnotification;
//        string CmdElementText = string.Empty;
//        Rcopia_MedicationManager objRcopProxy = new Rcopia_MedicationManager();
//        Rcopia_AllergyManager objAllergyMngr = new Rcopia_AllergyManager();
//        Rcopia_Prescription_ListManager objPrescMngr = new Rcopia_Prescription_ListManager();
//        ProblemListManager objProbListProxy = new ProblemListManager();
//        HumanManager objHumanMngr = new HumanManager();

//        CultureInfo culture = new CultureInfo("en-US");

//        public void ReadXMLResponse(string XMLResponse)
//        {
//            if (XMLResponse == null)
//            {
//                return;
//            }

//            if (XMLResponse == string.Empty)
//            {
//                return;
//            }

//            CmdElementText = string.Empty;
//            #region Responsexml
//            string XMLFileName = string.Empty;

//            XmlDocument XMLDoc = new XmlDocument();
//            XMLDoc.LoadXml(XMLResponse);

//            XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("Command");
//            CmdElementText = ((XmlElement)xmlReqNode[0]).InnerText;

//            XmlNodeList nodeList = XMLDoc.GetElementsByTagName("Response");
//            XmlElement SubElement = null;
//            ilstPatient.Clear();
//            ilstMedication.Clear();
//            ilstAllergy.Clear();
//            ilstProblem.Clear();
//            ilstRcopiaPrescription.Clear();



//            if (CmdElementText != string.Empty)
//            {
//                #region Read_ResponseXML Classes

//                foreach (XmlElement Element in nodeList)
//                {
//                    for (int i = 0; i < Element.ChildNodes.Count; i++)
//                    {
//                        XmlElement xmlLastUpdateTime = (XmlElement)Element.ChildNodes[i];
//                        InsertintoProperties(xmlLastUpdateTime, xmlLastUpdateTime.Name);
//                        if (Element.ChildNodes[i].Name == "MedicationList" || Element.ChildNodes[i].Name == "AllergyList" || Element.ChildNodes[i].Name == "ProblemList" || Element.ChildNodes[i].Name == "PatientList" || Element.ChildNodes[i].Name == "PrescriptionList" || Element.ChildNodes[i].Name == "NotificationCountList")
//                        {
//                            XmlNodeList nodeList3 = Element.GetElementsByTagName(Element.ChildNodes[i].Name);
//                            for (int k = 0; k < nodeList3[0].ChildNodes.Count; k++)
//                            {
//                                XmlElement xmlnode = (XmlElement)nodeList3[0].ChildNodes[k];
//                                if (xmlnode.Name == "Medication" || xmlnode.Name == "Allergy" || xmlnode.Name == "Problem" || xmlnode.Name == "Patient" || xmlnode.Name == "Prescription" || xmlnode.Name == "NotificationCount")
//                                {
//                                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                                        ObjMedication = new Rcopia_Medication();
//                                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
//                                        objAllergy = new Rcopia_Allergy();
//                                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
//                                        objProbList = new ProblemList();
//                                    else if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
//                                        objPatient = new Human();
//                                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                                        objPrescription = new Rcopia_Prescription_List();
//                                    else if (CmdElementText.ToUpper() == "GET_NOTIFICATION_COUNT")
//                                        objrcopnotification = new Rcopia_NotificationDTO();
//                                    for (int m = 0; m < xmlnode.ChildNodes.Count; m++)
//                                    {
//                                        SubElement = (XmlElement)xmlnode.ChildNodes[m];
//                                        InsertintoProperties(SubElement, SubElement.Name);
//                                        if (SubElement.Name == "Sig" || SubElement.Name == "Patient" || SubElement.Name == "Status" || SubElement.Name == "Allergen" || SubElement.Name == "ProblemList" || SubElement.Name == "Pharmacy")
//                                        {
//                                            for (int c = 0; c < SubElement.ChildNodes.Count; c++)
//                                            {
//                                                XmlElement ParentElement = (XmlElement)SubElement.ChildNodes[c];
//                                                InsertintoProperties(ParentElement, ParentElement.Name);
//                                                if (ParentElement.Name == "Drug" || ParentElement.Name == "Problem")
//                                                {
//                                                    for (int b = 0; b < ParentElement.ChildNodes.Count; b++)
//                                                    {
//                                                        XmlElement DrugElement = (XmlElement)ParentElement.ChildNodes[b];
//                                                        InsertintoProperties(DrugElement, DrugElement.Name);
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                    if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
//                                        ilstAllergy.Add(objAllergy);
//                                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                                        ilstMedication.Add(ObjMedication);
//                                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
//                                        ilstProblem.Add(objProbList);
//                                    else if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
//                                        ilstPatient.Add(objPatient);
//                                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                                        ilstRcopiaPrescription.Add(objPrescription);
//                                    else if (CmdElementText.ToUpper() == "GET_NOTIFICATION_COUNT")
//                                        ilstNotification.Add(objrcopnotification);
//                                }
//                            }
//                        }
//                    }
//                }

//                if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && ilstMedication.Count > 0)
//                    objRcopProxy.InsertOrUpdateMedication(ilstMedication.ToArray<Rcopia_Medication>(), ClientSession.UserName, string.Empty, DateTime.UtcNow, ClientSession.FacilityName, ClientSession.EncounterId);
//                else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY" && ilstAllergy.Count > 0)
//                    objAllergyMngr.InsertOrUpdateAllergy(ilstAllergy.ToArray<Rcopia_Allergy>(), ClientSession.UserName, string.Empty, DateTime.UtcNow);
//                else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM" && ilstProblem.Count > 0)
//                    objProbListProxy.InsertOrUpdateProblemList(ilstProblem.ToArray<ProblemList>(), string.Empty, DateTime.UtcNow);
//                else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && ilstRcopiaPrescription.Count > 0)
//                    objPrescMngr.InsertOrUpdatePrescription_List(ilstRcopiaPrescription.ToArray<Rcopia_Prescription_List>(), ClientSession.UserName, string.Empty, DateTime.UtcNow);
//                else if (CmdElementText.ToUpper() == "UPDATE_PATIENT" && ilstPatient.Count > 0)
//                    objHumanMngr.InsertOrUpdateHumanByRcopia(ilstPatient.ToArray<Human>(), string.Empty);
//                #endregion
//            }
//            #endregion
//        }


//        public void InsertintoProperties(XmlElement Element, string sElementText)
//        {
//            switch (sElementText)
//            {
//                # region SwitchCase

//                case "ExternalID":
//                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT" && Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
//                        objPatient.Id = Convert.ToUInt64(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                    {
//                        if (Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
//                            ObjMedication.Human_ID = Convert.ToUInt64(Element.InnerText);
//                    }
//                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
//                    {
//                        if (Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
//                            objAllergy.Human_ID = Convert.ToUInt64(Element.InnerText);
//                    }
//                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
//                    {
//                        if (Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
//                            objProbList.Human_ID = Convert.ToUInt64(Element.InnerText);
//                    }
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                    {
//                        if (Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
//                            objPrescription.Human_ID = Convert.ToUInt64(Element.InnerText);
//                    }
//                    break;
//                case "FirstName":
//                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
//                        objPatient.First_Name = Convert.ToString(Element.InnerText);
//                    break;
//                case "MiddleName":
//                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
//                        objPatient.MI = Convert.ToString(Element.InnerText);
//                    break;
//                case "Suffix":
//                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
//                        objPatient.Suffix = Convert.ToString(Element.InnerText);
//                    break;
//                case "LastName":
//                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
//                        objPatient.Last_Name = Convert.ToString(Element.InnerText);
//                    break;
//                case "DOB":
//                    if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PATIENT")
//                    {
//                        string sDate = Convert.ToString(Element.InnerText);
//                        DateTime dt = Convert.ToDateTime(sDate);
//                        objPatient.Birth_Date = DateTime.ParseExact(dt.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
//                    }
//                    break;
//                case "Sex":
//                    if (Convert.ToString(Element.InnerText) != string.Empty && Convert.ToString(Element.InnerText).ToUpper() == "F")
//                        objPatient.Sex = "FEMALE";
//                    else if (Convert.ToString(Element.InnerText) != string.Empty && Convert.ToString(Element.InnerText).ToUpper() == "M")
//                        objPatient.Sex = "MALE";
//                    break;
//                case "HomePhone":
//                    objPatient.Home_Phone_No = Convert.ToString(Element.InnerText);
//                    break;
//                case "Address1":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_Address1 = Convert.ToString(Element.InnerText);
//                    else
//                        objPatient.Street_Address1 = Convert.ToString(Element.InnerText);
//                    break;
//                case "Address2":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_Address2 = Convert.ToString(Element.InnerText);
//                    else
//                        objPatient.Street_Address2 = Convert.ToString(Element.InnerText);
//                    break;
//                case "City":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_City = Convert.ToString(Element.InnerText);
//                    else
//                        objPatient.City = Convert.ToString(Element.InnerText);
//                    break;
//                case "State":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_State = Convert.ToString(Element.InnerText);
//                    else
//                        objPatient.State = Convert.ToString(Element.InnerText);
//                    break;
//                case "Zip":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_Zip = Convert.ToString(Element.InnerText);
//                    else
//                        objPatient.ZipCode = Convert.ToString(Element.InnerText);
//                    break;
//                case "SSN":
//                    objPatient.SSN = Convert.ToString(Element.InnerText);
//                    break;
//                case "StartDate":
//                    if (Element.InnerText != string.Empty)
//                    {
//                        DateTime StartDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3));
//                        ObjMedication.Start_Date = DateTime.ParseExact(StartDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
//                    }
//                    break;
//                case "StopDate":
//                    if (Element.InnerText != string.Empty)
//                    {
//                        DateTime StopDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3));
//                        DateTime dt = DateTime.ParseExact(StopDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
//                        if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                            ObjMedication.Stop_Date = dt;
//                        else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                            objPrescription.Stop_Date = dt;
//                    }
//                    break;
//                case "FillDate":
//                    if (Element.InnerText != string.Empty)
//                    {
//                        DateTime FillDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3));
//                        ObjMedication.Fill_Date = DateTime.ParseExact(FillDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
//                    }
//                    break;
//                case "StopReason":
//                    ObjMedication.Stop_Reason = Convert.ToString(Element.InnerText);
//                    break;
//                case "SigChangedDate":
//                    if (Element.InnerText != string.Empty)
//                    {
//                        DateTime SigChangedDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3));
//                        ObjMedication.Sig_Changed_Date = DateTime.ParseExact(SigChangedDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
//                    }
//                    break;
//                case "LastModifiedBy":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Last_Modified_By = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
//                        objAllergy.Last_Modified_By = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
//                        objProbList.Last_Modified_By = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Last_Modified_By = Convert.ToString(Element.InnerText);
//                    break;
//                case "LastModifiedDate":
//                    if (Element.InnerText != string.Empty)
//                    {
//                        DateTime LastModifiedDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3));
//                        DateTime dt = DateTime.ParseExact(LastModifiedDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
//                        if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                            ObjMedication.Last_Modified_Date = dt;
//                        else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
//                            objAllergy.Last_Modified_Date = dt;
//                        else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
//                            objProbList.Last_Modified_Date = dt;
//                        else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                            objPrescription.Last_Modified_Date = dt;
//                    }
//                    break;
//                case "Height":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Height = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Height = Convert.ToString(Element.InnerText);
//                    break;
//                case "Weight":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Weight = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Weight = Convert.ToString(Element.InnerText);
//                    break;
//                case "IntendedUse":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Intended_Use = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Intended_Use = Convert.ToString(Element.InnerText);
//                    break;
//                case "Action":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Action = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Action = Convert.ToString(Element.InnerText);
//                    break;
//                case "Dose":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Dose = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Dose = Convert.ToString(Element.InnerText);
//                    break;
//                case "DoseUnit":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Dose_Unit = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Dose_Unit = Convert.ToString(Element.InnerText);
//                    break;
//                case "Route":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Route = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Route = Convert.ToString(Element.InnerText);
//                    break;
//                case "DoseTiming":
//                    if (Element.ParentNode.Name == "Sig" && Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Dose_Timing = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Dose_Timing = Convert.ToString(Element.InnerText);
//                    break;
//                case "DoseOther":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Dose_Other = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Dose_Other = Convert.ToString(Element.InnerText);
//                    break;
//                case "Duration":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Duration = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Duration = Convert.ToString(Element.InnerText);
//                    break;
//                case "Quantity":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Quantity = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Quantity = Convert.ToString(Element.InnerText);
//                    break;
//                case "QuantityUnit":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Quantity_Unit = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Quantity_Unit = Convert.ToString(Element.InnerText);
//                    break;
//                case "Refills":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Refills = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Refills = Convert.ToString(Element.InnerText);
//                    break;
//                case "SubstitutionPermitted":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Substitution_Permitted = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Substitution_Permitted = Convert.ToString(Element.InnerText);
//                    break;
//                case "OtherNotes":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Other_Notes = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Other_Notes = Convert.ToString(Element.InnerText);
//                    break;
//                case "PatientNotes":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Patient_Notes = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Patient_Notes = Convert.ToString(Element.InnerText);
//                    break;
//                case "Comments":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Comments = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Comments = Convert.ToString(Element.InnerText);
//                    break;
//                case "NDCID":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.NDC_ID = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
//                        objAllergy.NDC_ID = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.NDC_ID = Convert.ToString(Element.InnerText);
//                    break;
//                case "FirstDataBankMedID":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.First_DataBank_Med_ID = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
//                        objAllergy.First_DataBank_Med_ID = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.First_DataBank_Med_ID = Convert.ToString(Element.InnerText);
//                    break;
//                case "BrandName":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Brand_Name = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Brand_Name = Convert.ToString(Element.InnerText);
//                    break;
//                case "GenericName":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Generic_Name = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Generic_Name = Convert.ToString(Element.InnerText);
//                    break;
//                case "Form":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Form = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Form = Convert.ToString(Element.InnerText);
//                    break;
//                case "Strength":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                        ObjMedication.Strength = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Strength = Convert.ToString(Element.InnerText);
//                    break;
//                case "Reaction":
//                    objAllergy.Reaction = Convert.ToString(Element.InnerText);
//                    break;
//                case "OnsetDate":
//                    if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PROBLEM")
//                    {
//                        DateTime dt = DateTime.ParseExact(Element.InnerText, "dd-MMM-yyyy", null);
//                        objProbList.Date_Diagnosed = dt.ToString();
//                    }
//                    else if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_ALLERGY")
//                        objAllergy.OnsetDate = Convert.ToDateTime(Element.InnerText);
//                    break;
//                case "Name":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_Name = Convert.ToString(Element.InnerText);
//                    else
//                        objAllergy.Allergy_Name = Convert.ToString(Element.InnerText);
//                    break;
//                case "Active":
//                    if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PROBLEM")
//                        objProbList.Status = Convert.ToString(Element.InnerText);
//                    else if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_ALLERGY")
//                        objAllergy.Status = "Active"; //Convert.ToString(Element.InnerText);
//                    break;
//                case "RcopiaID":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.InnerText != string.Empty && Element.ParentNode.Name == "Medication")
//                        ObjMedication.Id = Convert.ToUInt64(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY" && Element.InnerText != string.Empty && Element.ParentNode.Name == "Allergy")
//                        objAllergy.Id = Convert.ToUInt64(Element.InnerText);
//                    else if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PROBLEM" && Element.ParentNode.Name == "Problem")
//                        objProbList.Rcopia_ID = Convert.ToUInt64(Element.InnerText);
//                    else if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && Element.ParentNode.Name == "Prescription")
//                    {
//                        string stext = Element.InnerText.Replace("DEV-", "").Replace("BB-", "");
//                        objPrescription.Id = Convert.ToUInt64(stext);
//                    }
//                    break;
//                case "Code":
//                    if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
//                        objProbList.ICD = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                    {
//                        if (objPrescription.ICD_Code != string.Empty)
//                            objPrescription.ICD_Code = objPrescription.ICD_Code + "," + Convert.ToString(Element.InnerText);
//                        else
//                            objPrescription.ICD_Code = Convert.ToString(Element.InnerText);
//                    }
//                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                    {
//                        if (ObjMedication.ICD_Code != string.Empty)
//                            ObjMedication.ICD_Code = ObjMedication.ICD_Code + "," + Convert.ToString(Element.InnerText);
//                        else
//                            ObjMedication.ICD_Code = Convert.ToString(Element.InnerText);
//                    }

//                    break;
//                case "Description":
//                    if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
//                        objProbList.Problem_Description = Convert.ToString(Element.InnerText);
//                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                    {
//                        if (objPrescription.ICD_Code_Description != string.Empty)
//                            objPrescription.ICD_Code_Description = objPrescription.ICD_Code_Description + "," + Convert.ToString(Element.InnerText);
//                        else
//                            objPrescription.ICD_Code_Description = Convert.ToString(Element.InnerText);
//                    }
//                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                    {
//                        if (ObjMedication.ICD_Code_Description != string.Empty)
//                            ObjMedication.ICD_Code_Description = ObjMedication.ICD_Code_Description + "," + Convert.ToString(Element.InnerText);
//                        else
//                            ObjMedication.ICD_Code_Description = Convert.ToString(Element.InnerText);
//                    }
//                    break;
//                case "BrandType":
//                    if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                        objPrescription.Brand_Type = Convert.ToString(Element.InnerText);
//                    break;
//                case "CreatedDate":
//                    if (Element.InnerText != string.Empty)
//                        objPrescription.Created_Date_And_Time = Convert.ToDateTime(Element.InnerText.Substring(0, Element.InnerText.Length - 3));
//                    break;
//                case "CompletedDate":
//                    if (Element.InnerText != string.Empty)
//                    {
//                        Element.InnerText = Element.InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "");
//                        if (Element.InnerText != string.Empty)
//                            objPrescription.Completed_Date = Convert.ToDateTime(Element.InnerText.Substring(0, Element.InnerText.Length - 3), culture);
//                    }
//                    break;
//                case "SignedDate":
//                    if (Element.InnerText != string.Empty)
//                        objPrescription.Signed_Date = Convert.ToDateTime(Element.InnerText.Substring(0, Element.InnerText.Length - 3));
//                    break;
//                case "Order":
//                    if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
//                    {
//                        if (objPrescription.Prescription_Order != string.Empty)
//                            objPrescription.Prescription_Order = objPrescription.Prescription_Order + "," + Convert.ToString(Element.InnerText);
//                        else
//                            objPrescription.Prescription_Order = Convert.ToString(Element.InnerText);
//                    }
//                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
//                    {
//                        if (ObjMedication.Medication_Order != string.Empty)
//                            ObjMedication.Medication_Order = ObjMedication.Medication_Order + "," + Convert.ToString(Element.InnerText);
//                        else
//                            ObjMedication.Medication_Order = Convert.ToString(Element.InnerText);

//                    }
//                    break;

//                case "LastUpdateDate":
//                    if (Element.InnerText != string.Empty)
//                    {
//                        objrcopUpdatInfo = new Rcopia_Update_info();
//                        objrcopUpdatInfo.Command = CmdElementText;
//                        objrcopUpdatInfo.Last_Updated_Date_Time = Convert.ToDateTime(Element.InnerText);
//                        objrcopUpdatInfo.Last_Updated_Date_Time = objrcopUpdatInfo.Last_Updated_Date_Time.AddDays(-1);
//                        ilstRcopupdateInfo.Add(objrcopUpdatInfo);
//                    }
//                    break;

//                case "Deleted":
//                    if (Element.InnerText != string.Empty)
//                    {
//                        if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Medication")
//                            ObjMedication.Deleted = Convert.ToString(Element.InnerText).ToUpper();
//                        else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && Element.ParentNode.Name == "Prescription")
//                            objPrescription.Deleted = Convert.ToString(Element.InnerText).ToUpper();
//                        else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY" && Element.ParentNode.Name == "Allergy")
//                            objAllergy.Deleted = Convert.ToString(Element.InnerText).ToUpper();
//                    }
//                    break;

//                case "Type":
//                    if (Element.ParentNode.Name == "NotificationCount" && Element.InnerText != string.Empty)
//                        objrcopnotification.Type = Convert.ToString(Element.InnerText);
//                    break;
//                case "Number":
//                    if (Element.ParentNode.Name == "NotificationCount" && Element.InnerText != string.Empty)
//                        objrcopnotification.Number = Convert.ToString(Element.InnerText);
//                    break;
//                case "CrossStreet":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_CrossStreet = Convert.ToString(Element.InnerText);
//                    break;
//                case "Phone":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_Phone = Convert.ToString(Element.InnerText);
//                    break;
//                case "Fax":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_Fax = Convert.ToString(Element.InnerText);
//                    break;
//                case "Is24Hour":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_Is24Hour = Convert.ToString(Element.InnerText);
//                    break;
//                case "Level3":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_Level3 = Convert.ToString(Element.InnerText);
//                    break;
//                case "Electronic":
//                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
//                        ObjMedication.Pharmacy_Electronic = Convert.ToString(Element.InnerText);
//                    break;
//                #endregion
//            }
//        }

//        public void FillUpdateObj(Human obj)
//        {
//            HumanRecord.First_Name = obj.First_Name;
//            HumanRecord.Last_Name = obj.Last_Name;
//            HumanRecord.MI = obj.MI;
//            HumanRecord.Suffix = obj.Suffix;
//            HumanRecord.Birth_Date = obj.Birth_Date;
//            HumanRecord.Sex = obj.Sex;
//            HumanRecord.Home_Phone_No = obj.Home_Phone_No;
//            HumanRecord.Street_Address1 = obj.Street_Address1;
//            HumanRecord.Street_Address2 = obj.Street_Address2;
//            HumanRecord.City = obj.City;
//            HumanRecord.State = obj.State;
//            HumanRecord.ZipCode = obj.ZipCode;
//            HumanRecord.SSN = obj.SSN;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
//using Acurus.Capella.Rcopia;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Acurus.Capella.UI.RCopia
{
    public partial class RCopiaXMLResponseProcess
    {
        IList<Human> ilstPatient = new List<Human>();
        IList<Rcopia_Medication> ilstMedication = new List<Rcopia_Medication>();
        IList<Rcopia_Allergy> ilstAllergy = new List<Rcopia_Allergy>();
        IList<ProblemList> ilstProblem = new List<ProblemList>();
        IList<Rcopia_Prescription_List> ilstRcopiaPrescription = new List<Rcopia_Prescription_List>();
        public static IList<Rcopia_Update_info> ilstRcopupdateInfo = new List<Rcopia_Update_info>();
        //Jira CAP-1367
        //public static IList<Rcopia_NotificationDTO> ilstNotification = new List<Rcopia_NotificationDTO>();

        ulong MyhumanID = 0;
        Human objPatient;
        Human HumanRecord;
        Rcopia_Medication ObjMedication;
        Rcopia_Allergy objAllergy;
        ProblemList objProbList;
        Rcopia_Prescription_List objPrescription;
        Rcopia_Update_info objrcopUpdatInfo;
        Rcopia_NotificationDTO objrcopnotification;
        string CmdElementText = string.Empty;
        Rcopia_MedicationManager objRcopProxy = new Rcopia_MedicationManager();
        Rcopia_AllergyManager objAllergyMngr = new Rcopia_AllergyManager();
        Rcopia_Prescription_ListManager objPrescMngr = new Rcopia_Prescription_ListManager();
        ProblemListManager objProbListProxy = new ProblemListManager();
        HumanManager objHumanMngr = new HumanManager();

        CultureInfo culture = new CultureInfo("en-US");

        public void ReadXMLResponse(string XMLResponse, out IList<Rcopia_NotificationDTO> ilstNotification)
        {
            ilstNotification = new List<Rcopia_NotificationDTO>();

            if (XMLResponse == null)
            {
                return;
            }

            if (XMLResponse == string.Empty)
            {
                return;
            }

            CmdElementText = string.Empty;
            #region Responsexml
            string XMLFileName = string.Empty;

            XmlDocument XMLDoc = new XmlDocument();
            XMLDoc.LoadXml(XMLResponse);

            XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("Command");
            CmdElementText = ((XmlElement)xmlReqNode[0]).InnerText;

            XmlNodeList nodeList = XMLDoc.GetElementsByTagName("Response");
            XmlElement SubElement = null;
            ilstPatient.Clear();
            ilstMedication.Clear();
            ilstAllergy.Clear();
            ilstProblem.Clear();
            ilstRcopiaPrescription.Clear();



            if (CmdElementText != string.Empty)
            {
                #region Read_ResponseXML Classes

                foreach (XmlElement Element in nodeList)
                {
                    for (int i = 0; i < Element.ChildNodes.Count; i++)
                    {
                        XmlElement xmlLastUpdateTime = (XmlElement)Element.ChildNodes[i];
                        InsertintoProperties(xmlLastUpdateTime, xmlLastUpdateTime.Name);
                        if (Element.ChildNodes[i].Name == "MedicationList" || Element.ChildNodes[i].Name == "AllergyList" || Element.ChildNodes[i].Name == "ProblemList" || Element.ChildNodes[i].Name == "PatientList" || Element.ChildNodes[i].Name == "PrescriptionList" || Element.ChildNodes[i].Name == "NotificationCountList")
                        {
                            XmlNodeList nodeList3 = Element.GetElementsByTagName(Element.ChildNodes[i].Name);
                            for (int k = 0; k < nodeList3[0].ChildNodes.Count; k++)
                            {
                                XmlElement xmlnode = (XmlElement)nodeList3[0].ChildNodes[k];
                                if (xmlnode.Name == "Medication" || xmlnode.Name == "Allergy" || xmlnode.Name == "Problem" || xmlnode.Name == "Patient" || xmlnode.Name == "Prescription" || xmlnode.Name == "NotificationCount")
                                {
                                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                                        ObjMedication = new Rcopia_Medication();
                                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                                        objAllergy = new Rcopia_Allergy();
                                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                                        objProbList = new ProblemList();
                                    else if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
                                        objPatient = new Human();
                                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                                        objPrescription = new Rcopia_Prescription_List();
                                    else if (CmdElementText.ToUpper() == "GET_NOTIFICATION_COUNT")
                                        objrcopnotification = new Rcopia_NotificationDTO();
                                    for (int m = 0; m < xmlnode.ChildNodes.Count; m++)
                                    {
                                        SubElement = (XmlElement)xmlnode.ChildNodes[m];
                                        InsertintoProperties(SubElement, SubElement.Name);
                                        //Jira CAP-3785
                                        //if (SubElement.Name == "Sig" || SubElement.Name == "Patient" || SubElement.Name == "Status" || SubElement.Name == "Allergen" || SubElement.Name == "ProblemList" || SubElement.Name == "Pharmacy")
                                        if (SubElement.Name == "Sig" || SubElement.Name == "Patient" || SubElement.Name == "Status" || SubElement.Name == "Allergen" || SubElement.Name == "ProblemList" || SubElement.Name == "Pharmacy" || SubElement.Name == "Provider" || SubElement.Name == "Preparer")
                                        {
                                            for (int c = 0; c < SubElement.ChildNodes.Count; c++)
                                            {
                                                XmlElement ParentElement = (XmlElement)SubElement.ChildNodes[c];
                                                InsertintoProperties(ParentElement, ParentElement.Name);
                                                if (ParentElement.Name == "Drug" || ParentElement.Name == "Problem")
                                                {
                                                    for (int b = 0; b < ParentElement.ChildNodes.Count; b++)
                                                    {
                                                        XmlElement DrugElement = (XmlElement)ParentElement.ChildNodes[b];
                                                        InsertintoProperties(DrugElement, DrugElement.Name);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                                        ilstAllergy.Add(objAllergy);
                                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                                        ilstMedication.Add(ObjMedication);
                                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                                        ilstProblem.Add(objProbList);
                                    else if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
                                        ilstPatient.Add(objPatient);
                                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                                        ilstRcopiaPrescription.Add(objPrescription);
                                    else if (CmdElementText.ToUpper() == "GET_NOTIFICATION_COUNT")
                                        ilstNotification.Add(objrcopnotification);
                                }
                            }
                        }
                    }
                }

                if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && ilstMedication.Count > 0)
                    objRcopProxy.InsertOrUpdateMedication(ilstMedication.ToArray<Rcopia_Medication>(), ClientSession.UserName, string.Empty, DateTime.UtcNow, ClientSession.FacilityName, ClientSession.EncounterId);
                else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY" && ilstAllergy.Count > 0)
                    objAllergyMngr.InsertOrUpdateAllergy(ilstAllergy.ToArray<Rcopia_Allergy>(), ClientSession.UserName, string.Empty, DateTime.UtcNow);
                else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM" && ilstProblem.Count > 0)
                    objProbListProxy.InsertOrUpdateProblemList(ilstProblem.ToArray<ProblemList>(), string.Empty, DateTime.UtcNow);
                else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && ilstRcopiaPrescription.Count > 0)
                    objPrescMngr.InsertOrUpdatePrescription_List(ilstRcopiaPrescription.ToArray<Rcopia_Prescription_List>(), ClientSession.UserName, string.Empty, DateTime.UtcNow);
                else if (CmdElementText.ToUpper() == "UPDATE_PATIENT" && ilstPatient.Count > 0)
                    objHumanMngr.InsertOrUpdateHumanByRcopia(ilstPatient.ToArray<Human>(), string.Empty);
                #endregion
            }
            #endregion
        }
        //CAP-2651
        public void ReadXMLResponseForAkido(string XMLResponse, out IList<Rcopia_NotificationDTO> ilstNotification, string UserName, string FacilityName)
        {
            ilstNotification = new List<Rcopia_NotificationDTO>();

            if (XMLResponse == null)
            {
                return;
            }

            if (XMLResponse == string.Empty)
            {
                return;
            }

            CmdElementText = string.Empty;
            #region Responsexml
            string XMLFileName = string.Empty;

            XmlDocument XMLDoc = new XmlDocument();
            XMLDoc.LoadXml(XMLResponse);

            XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("Command");
            CmdElementText = ((XmlElement)xmlReqNode[0]).InnerText;

            XmlNodeList nodeList = XMLDoc.GetElementsByTagName("Response");
            XmlElement SubElement = null;
            ilstPatient.Clear();
            ilstMedication.Clear();
            ilstAllergy.Clear();
            ilstProblem.Clear();
            ilstRcopiaPrescription.Clear();



            if (CmdElementText != string.Empty)
            {
                #region Read_ResponseXML Classes

                foreach (XmlElement Element in nodeList)
                {
                    for (int i = 0; i < Element.ChildNodes.Count; i++)
                    {
                        XmlElement xmlLastUpdateTime = (XmlElement)Element.ChildNodes[i];
                        InsertintoProperties(xmlLastUpdateTime, xmlLastUpdateTime.Name);
                        if (Element.ChildNodes[i].Name == "MedicationList" || Element.ChildNodes[i].Name == "AllergyList" || Element.ChildNodes[i].Name == "ProblemList" || Element.ChildNodes[i].Name == "PatientList" || Element.ChildNodes[i].Name == "PrescriptionList" || Element.ChildNodes[i].Name == "NotificationCountList")
                        {
                            XmlNodeList nodeList3 = Element.GetElementsByTagName(Element.ChildNodes[i].Name);
                            for (int k = 0; k < nodeList3[0].ChildNodes.Count; k++)
                            {
                                XmlElement xmlnode = (XmlElement)nodeList3[0].ChildNodes[k];
                                if (xmlnode.Name == "Medication" || xmlnode.Name == "Allergy" || xmlnode.Name == "Problem" || xmlnode.Name == "Patient" || xmlnode.Name == "Prescription" || xmlnode.Name == "NotificationCount")
                                {
                                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                                        ObjMedication = new Rcopia_Medication();
                                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                                        objAllergy = new Rcopia_Allergy();
                                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                                        objProbList = new ProblemList();
                                    else if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
                                        objPatient = new Human();
                                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                                        objPrescription = new Rcopia_Prescription_List();
                                    else if (CmdElementText.ToUpper() == "GET_NOTIFICATION_COUNT")
                                        objrcopnotification = new Rcopia_NotificationDTO();
                                    for (int m = 0; m < xmlnode.ChildNodes.Count; m++)
                                    {
                                        SubElement = (XmlElement)xmlnode.ChildNodes[m];
                                        InsertintoProperties(SubElement, SubElement.Name);
                                        //Jira CAP-3785
                                        //if (SubElement.Name == "Sig" || SubElement.Name == "Patient" || SubElement.Name == "Status" || SubElement.Name == "Allergen" || SubElement.Name == "ProblemList" || SubElement.Name == "Pharmacy")
                                        if (SubElement.Name == "Sig" || SubElement.Name == "Patient" || SubElement.Name == "Status" || SubElement.Name == "Allergen" || SubElement.Name == "ProblemList" || SubElement.Name == "Pharmacy" || SubElement.Name == "Provider" || SubElement.Name == "Preparer")
                                        {
                                            for (int c = 0; c < SubElement.ChildNodes.Count; c++)
                                            {
                                                XmlElement ParentElement = (XmlElement)SubElement.ChildNodes[c];
                                                InsertintoProperties(ParentElement, ParentElement.Name);
                                                if (ParentElement.Name == "Drug" || ParentElement.Name == "Problem")
                                                {
                                                    for (int b = 0; b < ParentElement.ChildNodes.Count; b++)
                                                    {
                                                        XmlElement DrugElement = (XmlElement)ParentElement.ChildNodes[b];
                                                        InsertintoProperties(DrugElement, DrugElement.Name);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                                        ilstAllergy.Add(objAllergy);
                                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                                        ilstMedication.Add(ObjMedication);
                                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                                        ilstProblem.Add(objProbList);
                                    else if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
                                        ilstPatient.Add(objPatient);
                                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                                        ilstRcopiaPrescription.Add(objPrescription);
                                    else if (CmdElementText.ToUpper() == "GET_NOTIFICATION_COUNT")
                                        ilstNotification.Add(objrcopnotification);
                                }
                            }
                        }
                    }
                }

                if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && ilstMedication.Count > 0)
                    objRcopProxy.InsertOrUpdateMedication(ilstMedication.ToArray<Rcopia_Medication>(), UserName, string.Empty, DateTime.UtcNow, FacilityName, 0);
                else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY" && ilstAllergy.Count > 0)
                    objAllergyMngr.InsertOrUpdateAllergy(ilstAllergy.ToArray<Rcopia_Allergy>(), UserName, string.Empty, DateTime.UtcNow);
                else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM" && ilstProblem.Count > 0)
                    objProbListProxy.InsertOrUpdateProblemList(ilstProblem.ToArray<ProblemList>(), string.Empty, DateTime.UtcNow);
                else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && ilstRcopiaPrescription.Count > 0)
                    objPrescMngr.InsertOrUpdatePrescription_List(ilstRcopiaPrescription.ToArray<Rcopia_Prescription_List>(), UserName, string.Empty, DateTime.UtcNow);
                else if (CmdElementText.ToUpper() == "UPDATE_PATIENT" && ilstPatient.Count > 0)
                    objHumanMngr.InsertOrUpdateHumanByRcopia(ilstPatient.ToArray<Human>(), string.Empty);
                #endregion
            }
            #endregion
        }

        public void InsertintoProperties(XmlElement Element, string sElementText)
        {
            switch (sElementText)
            {
                # region SwitchCase

                case "ExternalID":
                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT" && Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
                        try
                        {
                            //Latha - Branch_52_production_for_Rcopia - Start - 4 Jul 2011
                            objPatient.Id = Convert.ToUInt64(Element.InnerText);
                        }
                        catch
                        {
                            Element.InnerText = Regex.Replace(Element.InnerText, "[^0-9]", "");// Element.InnerText.Replace("test", "");
                            objPatient.Id = Convert.ToUInt64(Element.InnerText);
                            //Latha - Branch_52_production_for_Rcopia - End - 4 Jul 2011
                        }
                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                    {
                        if (Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
                            try
                            {

                                ObjMedication.Human_ID = Convert.ToUInt64(Element.InnerText);
                            }
                            catch
                            {
                                Element.InnerText = Regex.Replace(Element.InnerText, "[^0-9]", "");// Element.InnerText.Replace("test", "");
                                ObjMedication.Human_ID = Convert.ToUInt64(Element.InnerText);

                            }


                        if (Element.ParentNode.Name == "Medication" && Element.InnerText != string.Empty)
                            try
                            {

                                //ObjMedication.Id = Convert.ToUInt64(Element.InnerText);
                                ObjMedication.External_ID = Element.InnerText;
                            }
                            catch
                            {
                                //Element.InnerText = Regex.Replace(Element.InnerText, "[^0-9]", "");// Element.InnerText.Replace("test", "");
                                //ObjMedication.Id = Convert.ToUInt64(Element.InnerText);
                                ObjMedication.External_ID = Element.InnerText;

                            }
                        //ObjMedication.Human_ID = Convert.ToUInt64(Element.InnerText);
                    }
                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                    {
                        if (Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
                            try
                            {

                                objAllergy.Human_ID = Convert.ToUInt64(Element.InnerText);
                            }
                            catch
                            {
                                Element.InnerText = Regex.Replace(Element.InnerText, "[^0-9]", "");// Element.InnerText.Replace("test", "");
                                objAllergy.Human_ID = Convert.ToUInt64(Element.InnerText);

                            }


                        if (Element.ParentNode.Name == "Allergy" && Element.InnerText != string.Empty)
                            try
                            {

                                //objAllergy.Id = Convert.ToUInt64(Element.InnerText);
                                objAllergy.External_ID = Element.InnerText;
                            }
                            catch
                            {
                                //Element.InnerText = Regex.Replace(Element.InnerText, "[^0-9]", "");// Element.InnerText.Replace("test", "");
                                //objAllergy.Id = Convert.ToUInt64(Element.InnerText);
                                objAllergy.External_ID = Element.InnerText;

                            }
                        // objAllergy.Human_ID = Convert.ToUInt64(Element.InnerText);
                    }
                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                    {
                        if (Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
                            try
                            {

                                objProbList.Human_ID = Convert.ToUInt64(Element.InnerText);
                            }
                            catch
                            {
                                Element.InnerText = Regex.Replace(Element.InnerText, "[^0-9]", "");// Element.InnerText.Replace("test", "");
                                objProbList.Human_ID = Convert.ToUInt64(Element.InnerText);

                            }


                        if (Element.ParentNode.Name == "Problem" && Element.InnerText != string.Empty)
                            try
                            {

                                objProbList.Id = Convert.ToUInt64(Element.InnerText);
                            }
                            catch
                            {
                                Element.InnerText = Regex.Replace(Element.InnerText, "[^0-9]", "");// Element.InnerText.Replace("test", "");
                                objProbList.Id = Convert.ToUInt64(Element.InnerText);

                            }
                        // objProbList.Human_ID = Convert.ToUInt64(Element.InnerText);
                    }
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                    {
                        if (Element.ParentNode.Name == "Patient" && Element.InnerText != string.Empty)
                            try
                            {
                                //Latha - Branch_52_production_for_Rcopia - Start - 4 Jul 2011
                                objPrescription.Human_ID = Convert.ToUInt64(Element.InnerText);
                            }
                            catch
                            {
                                Element.InnerText = Regex.Replace(Element.InnerText, "[^0-9]", "");// Element.InnerText.Replace("test", "");
                                objPrescription.Human_ID = Convert.ToUInt64(Element.InnerText);
                                //Latha - Branch_52_production_for_Rcopia - End - 4 Jul 2011
                            }



                        // objPrescription.Human_ID = Convert.ToUInt64(Element.InnerText);
                    }
                    break;
                case "FirstName":
                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
                        objPatient.First_Name = Convert.ToString(Element.InnerText);
                    break;
                case "MiddleName":
                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
                        objPatient.MI = Convert.ToString(Element.InnerText);
                    break;
                case "Suffix":
                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
                        objPatient.Suffix = Convert.ToString(Element.InnerText);
                    break;
                case "LastName":
                    if (CmdElementText.ToUpper() == "UPDATE_PATIENT")
                        objPatient.Last_Name = Convert.ToString(Element.InnerText);
                    break;
                case "DOB":
                    if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PATIENT")
                    {
                        string sDate = Convert.ToString(Element.InnerText);
                        DateTime dt = Convert.ToDateTime(sDate, culture);
                        objPatient.Birth_Date = DateTime.ParseExact(dt.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
                    }
                    break;
                case "Sex":
                    if (Convert.ToString(Element.InnerText) != string.Empty && Convert.ToString(Element.InnerText).ToUpper() == "F")
                        objPatient.Sex = "FEMALE";
                    else if (Convert.ToString(Element.InnerText) != string.Empty && Convert.ToString(Element.InnerText).ToUpper() == "M")
                        objPatient.Sex = "MALE";
                    break;
                case "HomePhone":
                    objPatient.Home_Phone_No = Convert.ToString(Element.InnerText);
                    break;
                case "Address1":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_Address1 = Convert.ToString(Element.InnerText);
                    else if (objPatient != null)
                        objPatient.Street_Address1 = Convert.ToString(Element.InnerText);
                    break;
                case "Address2":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_Address2 = Convert.ToString(Element.InnerText);
                    else if (objPatient != null)
                        objPatient.Street_Address2 = Convert.ToString(Element.InnerText);
                    break;
                case "City":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_City = Convert.ToString(Element.InnerText);
                    else if (objPatient != null)
                        objPatient.City = Convert.ToString(Element.InnerText);
                    break;
                case "State":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_State = Convert.ToString(Element.InnerText);
                    else if (objPatient != null)
                        objPatient.State = Convert.ToString(Element.InnerText);
                    break;
                case "Zip":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_Zip = Convert.ToString(Element.InnerText);
                    else if (objPatient != null)
                        objPatient.ZipCode = Convert.ToString(Element.InnerText);
                    break;
                case "SSN":
                    objPatient.SSN = Convert.ToString(Element.InnerText);
                    break;
                case "StartDate":
                    if (Element.InnerText != string.Empty)
                    {
                        DateTime StartDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3), culture);
                        ObjMedication.Start_Date = DateTime.ParseExact(StartDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
                    }
                    break;
                case "StopDate":
                    if (Element.InnerText != string.Empty)
                    {
                        DateTime StopDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3), culture);
                        DateTime dt = DateTime.ParseExact(StopDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
                        if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                            ObjMedication.Stop_Date = dt;
                        else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                            objPrescription.Stop_Date = dt;
                    }
                    break;
                case "FillDate":
                    if (Element.InnerText != string.Empty)
                    {
                        DateTime FillDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3), culture);
                        ObjMedication.Fill_Date = DateTime.ParseExact(FillDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
                    }
                    break;
                case "StopReason":
                    ObjMedication.Stop_Reason = Convert.ToString(Element.InnerText);
                    break;
                case "SigChangedDate":
                    if (Element.InnerText != string.Empty)
                    {
                        DateTime SigChangedDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3), culture);
                        ObjMedication.Sig_Changed_Date = DateTime.ParseExact(SigChangedDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
                    }
                    break;
                case "LastModifiedBy":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Last_Modified_By = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                        objAllergy.Last_Modified_By = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                        objProbList.Last_Modified_By = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Last_Modified_By = Convert.ToString(Element.InnerText);
                    break;
                case "LastModifiedDate":
                    if (Element.InnerText != string.Empty)
                    {
                        DateTime LastModifiedDate = Convert.ToDateTime(Convert.ToString(Element.InnerText).Substring(0, Element.InnerText.Length - 3), culture);
                        DateTime dt = DateTime.ParseExact(LastModifiedDate.ToString("dd-MM-yyyy"), "dd-MM-yyyy", null);
                        if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                            ObjMedication.Last_Modified_Date = dt;
                        else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                            objAllergy.Last_Modified_Date = dt;
                        else if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                            objProbList.Last_Modified_Date = dt;
                        else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                            objPrescription.Last_Modified_Date = dt;
                    }
                    break;
                case "Height":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Height = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Height = Convert.ToString(Element.InnerText);
                    break;
                case "Weight":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Weight = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Weight = Convert.ToString(Element.InnerText);
                    break;
                case "IntendedUse":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Intended_Use = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Intended_Use = Convert.ToString(Element.InnerText);
                    break;
                case "Action":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Action = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Action = Convert.ToString(Element.InnerText);
                    break;
                case "Dose":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Dose = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Dose = Convert.ToString(Element.InnerText);
                    break;
                case "DoseUnit":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Dose_Unit = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Dose_Unit = Convert.ToString(Element.InnerText);
                    break;
                case "Route":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Route = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Route = Convert.ToString(Element.InnerText);
                    break;
                case "DoseTiming":
                    if (Element.ParentNode.Name == "Sig" && Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Dose_Timing = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Dose_Timing = Convert.ToString(Element.InnerText);
                    break;
                case "DoseOther":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Dose_Other = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Dose_Other = Convert.ToString(Element.InnerText);
                    break;
                case "Duration":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Duration = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Duration = Convert.ToString(Element.InnerText);
                    break;
                case "Quantity":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Quantity = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Quantity = Convert.ToString(Element.InnerText);
                    break;
                case "QuantityUnit":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Quantity_Unit = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Quantity_Unit = Convert.ToString(Element.InnerText);
                    break;
                case "Refills":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Refills = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Refills = Convert.ToString(Element.InnerText);
                    break;
                case "SubstitutionPermitted":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Substitution_Permitted = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Substitution_Permitted = Convert.ToString(Element.InnerText);
                    break;
                case "OtherNotes":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Other_Notes = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Other_Notes = Convert.ToString(Element.InnerText);
                    break;
                case "PatientNotes":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Patient_Notes = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Patient_Notes = Convert.ToString(Element.InnerText);
                    break;
                case "Comments":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Comments = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Comments = Convert.ToString(Element.InnerText);
                    break;
                case "NDCID":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.NDC_ID = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                        objAllergy.NDC_ID = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.NDC_ID = Convert.ToString(Element.InnerText);
                    break;
                case "FirstDataBankMedID":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.First_DataBank_Med_ID = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                        objAllergy.First_DataBank_Med_ID = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.First_DataBank_Med_ID = Convert.ToString(Element.InnerText);
                    break;
                case "BrandName":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Brand_Name = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Brand_Name = Convert.ToString(Element.InnerText);
                    break;
                case "GenericName":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Generic_Name = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Generic_Name = Convert.ToString(Element.InnerText);
                    break;
                case "Form":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Form = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Form = Convert.ToString(Element.InnerText);
                    break;
                case "Strength":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                        ObjMedication.Strength = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Strength = Convert.ToString(Element.InnerText);
                    break;
                case "Reaction":
                    if (Element.InnerXml.StartsWith("<Description>") == true)
                    {
                        objAllergy.Reaction = Element.InnerXml.Substring(0, Element.InnerXml.IndexOf("</Description>")).Replace("<Description>", "");
                        if (Element.InnerXml.Contains("</SNOMED-CTConceptID>") == true)
                            objAllergy.Reaction_Snomed_Code = Element.InnerXml.Substring(Element.InnerXml.IndexOf("<SNOMED-CTConceptID>") + 20, Element.InnerXml.IndexOf("</SNOMED-CTConceptID>") - Element.InnerXml.IndexOf("<SNOMED-CTConceptID>") - 20);
                    }
                    else
                        objAllergy.Reaction = Convert.ToString(Element.InnerText);
                    break;
                case "Severity":
                    if (Element.InnerXml.StartsWith("<Description>") == true)
                    {
                        objAllergy.Severity = Element.InnerXml.Substring(0, Element.InnerXml.IndexOf("</Description>")).Replace("<Description>", "");
                        if (Element.InnerXml.Contains("</SNOMED-CTConceptID>") == true)
                            objAllergy.Severity_Snomed_Code = Element.InnerXml.Substring(Element.InnerXml.IndexOf("<SNOMED-CTConceptID>") + 20, Element.InnerXml.IndexOf("</SNOMED-CTConceptID>") - Element.InnerXml.IndexOf("<SNOMED-CTConceptID>") - 20);
                    }
                    else
                        objAllergy.Severity = Convert.ToString(Element.InnerText);
                    break;
                case "OnsetDate":
                    if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                    {
                        DateTime dt = DateTime.ParseExact(Element.InnerText, "dd-MMM-yyyy", null);
                        objProbList.Date_Diagnosed = dt.ToString();
                    }
                    else if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                    {
                        Regex rgx = new Regex("[a-zA-Z]");
                        string temp = rgx.Replace(Element.InnerText, "");
                        objAllergy.OnsetDate = Convert.ToDateTime(temp, culture);
                    }
                    break;

                case "Name":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_Name = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                        objAllergy.Allergy_Name = Convert.ToString(Element.InnerText);
                    break;
                case "Active":
                    if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                        objProbList.Status = Convert.ToString(Element.InnerText);
                    else if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_ALLERGY")
                        objAllergy.Status = "Active"; //Convert.ToString(Element.InnerText);
                    break;
                case "Status":
                    if (Element.InnerXml != string.Empty && CmdElementText.ToUpper() == "UPDATE_ALLERGY" && Element.InnerText == "")
                        objAllergy.Status = Element.InnerXml.Replace("<", "").Replace("/", "").Replace(">", ""); //Convert.ToString(Element.InnerText);
                    break;
                case "RcopiaID":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.InnerText != string.Empty && Element.ParentNode.Name == "Medication")
                        ObjMedication.Id = Convert.ToUInt64(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY" && Element.InnerText != string.Empty && Element.ParentNode.Name == "Allergy")
                        objAllergy.Id = Convert.ToUInt64(Element.InnerText);
                    else if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PROBLEM" && Element.ParentNode.Name == "Problem")
                        objProbList.Rcopia_ID = Convert.ToUInt64(Element.InnerText);
                    else if (Element.InnerText != string.Empty && CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && Element.ParentNode.Name == "Prescription")
                    {
                        string stext = Element.InnerText.Replace("DEV-", "").Replace("BB-", "").Replace("SB-", "");
                        if (stext.All(Char.IsDigit))
                            objPrescription.Id = Convert.ToUInt64(stext);
                    }
                    break;
                case "Code":
                    if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                        objProbList.ICD = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                    {
                        if (objPrescription.ICD_Code != string.Empty)
                            objPrescription.ICD_Code = objPrescription.ICD_Code + "," + Convert.ToString(Element.InnerText);
                        else
                            objPrescription.ICD_Code = Convert.ToString(Element.InnerText);
                    }
                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                    {
                        if (ObjMedication.ICD_Code != string.Empty)
                            ObjMedication.ICD_Code = ObjMedication.ICD_Code + "," + Convert.ToString(Element.InnerText);
                        else
                            ObjMedication.ICD_Code = Convert.ToString(Element.InnerText);
                    }

                    break;
                case "Description":
                    if (CmdElementText.ToUpper() == "UPDATE_PROBLEM")
                        objProbList.Problem_Description = Convert.ToString(Element.InnerText);
                    else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                    {
                        if (objPrescription.ICD_Code_Description != string.Empty)
                            objPrescription.ICD_Code_Description = objPrescription.ICD_Code_Description + "," + Convert.ToString(Element.InnerText);
                        else
                            objPrescription.ICD_Code_Description = Convert.ToString(Element.InnerText);
                    }
                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                    {
                        if (ObjMedication.ICD_Code_Description != string.Empty)
                            ObjMedication.ICD_Code_Description = ObjMedication.ICD_Code_Description + "," + Convert.ToString(Element.InnerText);
                        else
                            ObjMedication.ICD_Code_Description = Convert.ToString(Element.InnerText);
                    }
                    break;
                case "BrandType":
                    if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                        objPrescription.Brand_Type = Convert.ToString(Element.InnerText);
                    break;
                case "CreatedDate":
                    if (Element.InnerText != string.Empty)
                        objPrescription.Created_Date_And_Time = Convert.ToDateTime(Element.InnerText.Substring(0, Element.InnerText.Length - 3), culture);
                    break;
                case "CompletedDate":
                    if (Element.InnerText != string.Empty)
                    {
                        Element.InnerText = Element.InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "");
                        if (Element.InnerText != string.Empty)
                            objPrescription.Completed_Date = Convert.ToDateTime(Element.InnerText.Substring(0, Element.InnerText.Length - 3), culture);
                    }

                    break;
                case "SignedDate":
                    if (Element.InnerText != string.Empty)
                        objPrescription.Signed_Date = Convert.ToDateTime(Element.InnerText.Substring(0, Element.InnerText.Length - 3), culture);
                    break;
                case "Order":
                    if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION")
                    {
                        if (objPrescription.Prescription_Order != string.Empty)
                            objPrescription.Prescription_Order = objPrescription.Prescription_Order + "," + Convert.ToString(Element.InnerText);
                        else
                            objPrescription.Prescription_Order = Convert.ToString(Element.InnerText);
                    }
                    else if (CmdElementText.ToUpper() == "UPDATE_MEDICATION")
                    {
                        if (ObjMedication.Medication_Order != string.Empty)
                            ObjMedication.Medication_Order = ObjMedication.Medication_Order + "," + Convert.ToString(Element.InnerText);
                        else
                            ObjMedication.Medication_Order = Convert.ToString(Element.InnerText);

                    }
                    break;
                case "LastUpdateDate":
                    if (Element.InnerText != string.Empty)
                    {
                        if (ilstRcopupdateInfo.Count == 6)
                        {
                            ilstRcopupdateInfo.Clear();
                        }
                        objrcopUpdatInfo = new Rcopia_Update_info();
                        objrcopUpdatInfo.Command = CmdElementText;
                        objrcopUpdatInfo.Last_Updated_Date_Time = Convert.ToDateTime(Element.InnerText, culture);
                        ilstRcopupdateInfo.Add(objrcopUpdatInfo);
                    }
                    break;

                case "Deleted":
                    if (Element.InnerText != string.Empty)
                    {
                        if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Medication")
                            ObjMedication.Deleted = Convert.ToString(Element.InnerText).ToUpper();
                        else if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && Element.ParentNode.Name == "Prescription")
                            objPrescription.Deleted = Convert.ToString(Element.InnerText).ToUpper();
                        else if (CmdElementText.ToUpper() == "UPDATE_ALLERGY" && Element.ParentNode.Name == "Allergy")
                            objAllergy.Deleted = Convert.ToString(Element.InnerText).ToUpper();
                    }
                    break;

                case "Type":
                    if (Element.ParentNode.Name == "NotificationCount" && Element.InnerText != string.Empty)
                        objrcopnotification.Type = Convert.ToString(Element.InnerText);
                    break;
                case "Number":
                    if (Element.ParentNode.Name == "NotificationCount" && Element.InnerText != string.Empty)
                        objrcopnotification.Number = Convert.ToString(Element.InnerText);
                    break;
                case "CrossStreet":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_CrossStreet = Convert.ToString(Element.InnerText);
                    break;
                case "Phone":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_Phone = Convert.ToString(Element.InnerText);
                    break;
                case "Fax":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_Fax = Convert.ToString(Element.InnerText);
                    break;
                case "Is24Hour":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_Is24Hour = Convert.ToString(Element.InnerText);
                    break;
                case "Level3":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_Level3 = Convert.ToString(Element.InnerText);
                    break;
                case "Electronic":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        ObjMedication.Pharmacy_Electronic = Convert.ToString(Element.InnerText);
                    if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && Element.ParentNode.Name == "Pharmacy" && Element.InnerText != string.Empty)
                        objPrescription.Electronic = Convert.ToString(Element.InnerText);
                    break;
                case "RxnormID":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Drug" && Element.InnerText != string.Empty)
                        ObjMedication.Rxnorm_ID = Convert.ToUInt64(Element.InnerText);
                    if (CmdElementText.ToUpper() == "UPDATE_ALLERGY" && Element.ParentNode.Name == "Drug" && Element.InnerText != string.Empty)
                        objAllergy.Rxnorm_ID = Convert.ToUInt64(Element.InnerText);
                    break;
                case "RxnormIDType":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Drug" && Element.InnerText != string.Empty)
                        ObjMedication.Rxnorm_ID_Type = Convert.ToString(Element.InnerText);
                    if (CmdElementText.ToUpper() == "UPDATE_ALLERGY" && Element.ParentNode.Name == "Drug" && Element.InnerText != string.Empty)
                        objAllergy.Rxnorm_ID_Type = Convert.ToString(Element.InnerText).ToUpper();
                    break;
                case "Username":
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Provider" && Element.InnerText != string.Empty)
                        ObjMedication.Provider_Rcopia_User_Name = Convert.ToString(Element.InnerText);
                    if (CmdElementText.ToUpper() == "UPDATE_MEDICATION" && Element.ParentNode.Name == "Preparer" && Element.InnerText != string.Empty)
                        ObjMedication.Preparer_Rcopia_User_Name = Convert.ToString(Element.InnerText);
                    if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && Element.ParentNode.Name == "Provider" && Element.InnerText != string.Empty)
                        objPrescription.Provider_Rcopia_User_Name = Convert.ToString(Element.InnerText);
                    if (CmdElementText.ToUpper() == "UPDATE_PRESCRIPTION" && Element.ParentNode.Name == "Preparer" && Element.InnerText != string.Empty)
                        objPrescription.Preparer_Rcopia_User_Name = Convert.ToString(Element.InnerText);
                    break;

                    #endregion
            }
        }

        public void FillUpdateObj(Human obj)
        {
            HumanRecord.First_Name = obj.First_Name;
            HumanRecord.Last_Name = obj.Last_Name;
            HumanRecord.MI = obj.MI;
            HumanRecord.Suffix = obj.Suffix;
            HumanRecord.Birth_Date = obj.Birth_Date;
            HumanRecord.Sex = obj.Sex;
            HumanRecord.Home_Phone_No = obj.Home_Phone_No;
            HumanRecord.Street_Address1 = obj.Street_Address1;
            HumanRecord.Street_Address2 = obj.Street_Address2;
            HumanRecord.City = obj.City;
            HumanRecord.State = obj.State;
            HumanRecord.ZipCode = obj.ZipCode;
            HumanRecord.SSN = obj.SSN;
        }
    }
}
