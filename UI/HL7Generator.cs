using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.Web.UI;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Web;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net;
using System.Security.Authentication;
//using Acurus.Capella.Proxy.LookupTables;
//using Acurus.Capella.Proxy;

namespace Acurus.Capella.UI
{
    public partial class HL7Generator
    {
        UserLookupManager localFieldLookupManager = new UserLookupManager();
        StaticLookupManager objStaticLookupManager = new StaticLookupManager();
        XmlWriterSettings wSettings = new XmlWriterSettings();
        MemoryStream ms;
      //  XmlWriter xmlWriter;
        public XmlDocument CreateCarePlanXML(PhysicianLibrary Phy, FillClinicalSummary ClinicalSummary, string sPrintFileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlRef = new XmlDocument();


            // xmlRef.Load(Application.StartupPath + "\\CCDConfig.xml");
            xmlDoc.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "SampleXML\\Care_Plan_Sample.xml"));
            XmlNodeList xmlReqNode = null;
            string sContent = string.Empty;
            xmlReqNode = xmlDoc.GetElementsByTagName("effectiveTime");
            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss+0500");
            string sMAName = string.Empty;
            
            #region OLdHEADER_CAREPLAN
            /* Old Header
            #region PatientRole

            // xmlReqNode = xmlDoc.GetElementsByTagName("id");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.ID.ToString();
            xmlReqNode = xmlDoc.GetElementsByTagName("languageCode");
            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Preferred_Language.ToString();
            xmlReqNode = xmlDoc.GetElementsByTagName("patientRole");
            xmlDoc.GetElementsByTagName("patientRole")[0].ChildNodes[0].Attributes[0].Value = ClinicalSummary.ID.ToString();


            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[0].InnerText = ClinicalSummary.Street_Address1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[0].InnerText = ClinicalSummary.City;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[0].InnerText = ClinicalSummary.State;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[0].InnerText = ClinicalSummary.ZipCode;
            xmlReqNode = xmlDoc.GetElementsByTagName("country");
            xmlReqNode[0].InnerText = "US";

            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            string sTelephoneno = ClinicalSummary.Home_Phone_No;




            if (sTelephoneno != string.Empty)
                xmlReqNode[0].Attributes[0].Value = "tel:+1" + sTelephoneno;
            else
                //For BugID : 28462
                //xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(909) 621-4949";
                xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(111) 111-1111";


            if (ClinicalSummary.HumanList[0].Work_Phone_No != string.Empty)
                xmlReqNode[1].Attributes[0].Value = "tel:+1" + ClinicalSummary.HumanList[0].Work_Phone_No;
            else
                //For BugID : 28462
                //xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(909) 621-4949";
                xmlReqNode[1].Attributes[0].Value = "tel:+1" + "(111) 111-1111";

            if (ClinicalSummary.HumanList[0].Cell_Phone_Number != string.Empty)
                xmlReqNode[2].Attributes[0].Value = "tel:+1" + ClinicalSummary.HumanList[0].Cell_Phone_Number;
            else
                //For BugID : 28462
                //xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(909) 621-4949";
                xmlReqNode[2].Attributes[0].Value = "tel:+1" + "(111) 111-1111";

            #endregion

            #region Patient


            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[0].InnerText = ClinicalSummary.First_Name;

            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[1].InnerText = ClinicalSummary.MI;

            //xmlReqNode = xmlDoc.GetElementsByTagName("givens");
            // xmlDoc.GetElementsByTagName("patient")[0].ChildNodes[0].ChildNodes[1].InnerText = ClinicalSummary.MI;

            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[0].InnerText = ClinicalSummary.Last_Name;


            xmlReqNode = xmlDoc.GetElementsByTagName("administrativeGenderCode");
            if (ClinicalSummary.Sex != null && ClinicalSummary.Sex != string.Empty)
            {
                xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Sex.Substring(0, 1).ToUpper();
                xmlReqNode[0].Attributes[2].Value = ClinicalSummary.Sex;
            }

            xmlReqNode = xmlDoc.GetElementsByTagName("birthTime");
            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Birth_Date.ToString("yyyyMMdd");


            xmlReqNode = xmlDoc.GetElementsByTagName("maritalStatusCode");
            if (ClinicalSummary.MaritalStatus != null && ClinicalSummary.MaritalStatus != string.Empty)
            {
                xmlReqNode[0].Attributes[0].Value = ClinicalSummary.MaritalStatus.Substring(0, 1).ToUpper(); ;
                xmlReqNode[0].Attributes[3].Value = ClinicalSummary.MaritalStatus;
            }
            if (ClinicalSummary.lookupList != null)
            {
                for (int Race = 0; Race < ClinicalSummary.lookupList.Count; Race++)
                {
                    if (ClinicalSummary.lookupList[Race].Field_Name == "ETHNICITY")
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
                        xmlReqNode[0].Attributes[0].Value = ClinicalSummary.lookupList[Race].Default_Value.ToString();
                        xmlReqNode[0].Attributes[1].Value = ClinicalSummary.lookupList[Race].Value.ToString();

                    }
                    else if (ClinicalSummary.lookupList[Race].Field_Name == "RACE")
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("raceCode");
                        xmlReqNode[0].Attributes[0].Value = ClinicalSummary.lookupList[Race].Default_Value.ToString();
                        xmlReqNode[0].Attributes[1].Value = ClinicalSummary.lookupList[Race].Value.ToString();
                    }
                }
            }




            //xmlReqNode = xmlDoc.GetElementsByTagName("raceCode");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Race_No;
            //xmlReqNode[0].Attributes[3].Value = ClinicalSummary.Race;

            //xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Ethinicity_No;
            //xmlReqNode[0].Attributes[3].Value = ClinicalSummary.Ethnicity;


            #endregion

            #region author

            xmlReqNode = xmlDoc.GetElementsByTagName("time");
            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss+0500");


            string sfacility = string.Empty;
            if (ClinicalSummary.Encounter != null && ClinicalSummary.Encounter.Count > 0)
                sfacility = ClinicalSummary.Encounter[0].Facility_Name;
            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[1].InnerText = sfacility + ", " + Phy.PhyAddress1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[1].InnerText = Phy.PhyCity;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[1].InnerText = Phy.PhyState;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[1].InnerText = Phy.PhyZip;
            xmlReqNode = xmlDoc.GetElementsByTagName("country");
            xmlReqNode[1].InnerText = "US";


            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            string sphyTelephoneno = Phy.PhyTelephone;
            if (sphyTelephoneno != string.Empty)
                xmlDoc.GetElementsByTagName("author")[0].ChildNodes[1].ChildNodes[2].Attributes[1].Value = "tel:+1" + sphyTelephoneno;
            else
                xmlDoc.GetElementsByTagName("author")[0].ChildNodes[1].ChildNodes[2].Attributes[1].Value = "tel:+1" + "(909) 621-4949";

            xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
            xmlReqNode[0].InnerText = Phy.PhyPrefix;
            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[1].InnerText = Phy.PhyFirstName;
            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[1].InnerText = Phy.PhyLastName;


            #endregion
            //new
            #region custodian
            xmlReqNode = xmlDoc.GetElementsByTagName("name");
            if (ClinicalSummary.Guarantor_human_MI == "")
            {
                xmlReqNode[2].InnerText = ClinicalSummary.Guarantor_human_Lastname + "," + ClinicalSummary.Guarantor_human_name;
            }
            else
            {
                xmlReqNode[2].InnerText = ClinicalSummary.Guarantor_human_Lastname + "," + ClinicalSummary.Guarantor_human_name + ClinicalSummary.Guarantor_human_MI;
            }
            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            //string sgurantorphoneno = ClinicalSummary.Guarantor_Home_Phone_No;
            //if (sgurantorphoneno != string.Empty)
            //    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + sgurantorphoneno;
            //else
            //    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + "(909) 621-4949";

            if (ClinicalSummary.facilityLibraryCustodian != null && ClinicalSummary.facilityLibraryCustodian.Count > 0)
            {
                for (int i = 0; i < ClinicalSummary.facilityLibraryCustodian.Count; i++)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                    xmlReqNode[2].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_Address1;
                    xmlReqNode = xmlDoc.GetElementsByTagName("city");
                    xmlReqNode[2].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_City;
                    xmlReqNode = xmlDoc.GetElementsByTagName("state");
                    xmlReqNode[2].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_State;
                    xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                    xmlReqNode[2].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_Zip;
                    xmlReqNode = xmlDoc.GetElementsByTagName("country");
                    xmlReqNode[2].InnerText = "US";

                    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_Name;

                    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + ClinicalSummary.facilityLibraryCustodian[i].Fac_Telephone;
                }
            }


            #endregion

            xmlReqNode = xmlDoc.GetElementsByTagName("id");
            xmlReqNode[1].Attributes[0].Value = ClinicalSummary.Encounter[0].Id.ToString();//doubt

            #region documentationOf
            if (ClinicalSummary.PhysicianLibraryDocumentation != null && ClinicalSummary.PhysicianLibraryDocumentation.Count != 0)
            {
                for (int i = 0; i < ClinicalSummary.PhysicianLibraryDocumentation.Count; i++)
                {
                    //XmlElement TempElement = null;
                    //XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("component")[0];
                    //XmlDocument docAllergyEntry = new XmlDocument();
                    //docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CareTeam.xml"));


                    //XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                    //xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                    ////xmlDoc.DocumentElement.LastChild.ChildNodes[0].ChildNodes[27].AppendChild(xfragAllergy);
                    //elemOldentry.PreviousSibling.AppendChild(xfragAllergy);
                    //xmlDoc.Save(sPrintFileName);
                    //xmlDoc.Load(sPrintFileName);



                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("custodian")[0];
                    //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();



                    xmlReqNode = xmlDoc.GetElementsByTagName("low");
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = ClinicalSummary.PhysicianLibraryDocumentation[i].Encounter_Date_of_Service.ToString("yyyyMMdd");


                    xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyPrefix;

                    xmlReqNode = xmlDoc.GetElementsByTagName("given");
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyFirstName;
                    xmlReqNode = xmlDoc.GetElementsByTagName("family");
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyLastName;

                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = Phy.PhyAddress1;
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = Phy.PhyCity;
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = Phy.PhyState;
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = Phy.PhyZip;

                    if (sphyTelephoneno != string.Empty)
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + sphyTelephoneno;
                    else
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + "(909) 621-4949";

                    if (ClinicalSummary.PhysicianLibraryDocumentation.Count == 1)
                    {
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
                        string sMedicalAssistant = string.Empty;
                        if (ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant == string.Empty)
                            sMedicalAssistant = "_";
                        else
                            sMedicalAssistant = ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = sMedicalAssistant;
                    }
                    else
                    {
                        i += 1;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyPrefix + " " + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyFirstName + " " + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyLastName;
                    }

                    //i += 1;
                    break;


                    // xfrag.InnerXml = sInnerXML;
                    //elemOld.LastChild.AppendChild(xfrag);
                    //elemOld.InsertBefore(xfrag);
                }

            }

            #endregion
            */
            #endregion

            #region NEWHEADER_SAME AS CCD
            #region PatientRole

            // xmlReqNode = xmlDoc.GetElementsByTagName("id");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.ID.ToString();
            xmlReqNode = xmlDoc.GetElementsByTagName("languageCode");
            if (ClinicalSummary.Preferred_Language != "")
                xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Preferred_Language.ToString();
            xmlReqNode = xmlDoc.GetElementsByTagName("patientRole");
            xmlDoc.GetElementsByTagName("patientRole")[0].ChildNodes[0].Attributes[0].Value = ClinicalSummary.ID.ToString();


            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[0].InnerText = ClinicalSummary.Street_Address1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[0].InnerText = ClinicalSummary.City;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[0].InnerText = ClinicalSummary.State;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[0].InnerText = ClinicalSummary.ZipCode;
            xmlReqNode = xmlDoc.GetElementsByTagName("country");
            xmlReqNode[0].InnerText = "US";

            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            string sTelephoneno = ClinicalSummary.Home_Phone_No;




            if (sTelephoneno != string.Empty)
                xmlReqNode[0].Attributes[0].Value = "tel:+1" + sTelephoneno;
            else
                //For BugID : 28462
                //xmlReqNode[0].Attributes[0].Value = "tel:" + "(909) 621-4949";
                xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(111) 111-1111";


            if (ClinicalSummary.HumanList[0].Work_Phone_No != string.Empty)
                xmlReqNode[1].Attributes[0].Value = "tel:+1" + ClinicalSummary.HumanList[0].Work_Phone_No;
            else
                //For BugID : 28462
                //xmlReqNode[0].Attributes[0].Value = "tel:" + "(909) 621-4949";
                xmlReqNode[1].Attributes[0].Value = "tel:+1" + "(111) 111-1111";

            if (ClinicalSummary.HumanList[0].Cell_Phone_Number != string.Empty)
                xmlReqNode[2].Attributes[0].Value = "tel:+1" + ClinicalSummary.HumanList[0].Cell_Phone_Number;
            else
                //For BugID : 28462
                //xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(909) 621-4949";
                xmlReqNode[2].Attributes[0].Value = "tel:+1" + "(111) 111-1111";
            #endregion

            #region Patient


            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[0].InnerText = ClinicalSummary.First_Name;



            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[0].InnerText = ClinicalSummary.Last_Name;

            xmlReqNode = xmlDoc.GetElementsByTagName("suffix");
            if (ClinicalSummary.Suffix.Trim() == "")
            {
                xmlReqNode[0].ParentNode.RemoveChild(xmlReqNode[0]);
            }
            else
                xmlReqNode[0].InnerText = ClinicalSummary.Suffix;
            xmlReqNode = xmlDoc.GetElementsByTagName("name");
            if (ClinicalSummary.Previous_Name.Trim() != "")
            {
                xmlReqNode[1].ChildNodes[0].InnerText = ClinicalSummary.Previous_Name.Trim();
                xmlReqNode[1].ChildNodes[1].InnerText = ClinicalSummary.First_Name;
                xmlReqNode[1].ChildNodes[2].InnerText = ClinicalSummary.Last_Name;
            }
            else
            {
                xmlReqNode[1].ParentNode.RemoveChild(xmlReqNode[1]);
            }
            xmlReqNode = xmlDoc.GetElementsByTagName("administrativeGenderCode");
            if (ClinicalSummary.Sex != null && ClinicalSummary.Sex != string.Empty)
            {
                //if (ClinicalSummary.Sex.Substring(0, 1).ToUpper() != "U")
                //{
                xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Sex.Substring(0, 1).ToUpper();
                xmlReqNode[0].Attributes[2].Value = ClinicalSummary.Sex;
                //}
                //else
                //{
                //    xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Sex.Substring(0, 3).ToUpper();
                //    xmlReqNode[0].Attributes[2].Value = ClinicalSummary.Sex;
                //}
            }

            xmlReqNode = xmlDoc.GetElementsByTagName("birthTime");
            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Birth_Date.ToString("yyyyMMdd");


            xmlReqNode = xmlDoc.GetElementsByTagName("maritalStatusCode");
            if (ClinicalSummary.MaritalStatus != null && ClinicalSummary.MaritalStatus != string.Empty && ClinicalSummary.MaritalStatus!= "Other")
            {
                xmlReqNode[0].Attributes[0].Value = ClinicalSummary.MaritalStatus.Substring(0, 1).ToUpper(); ;
                xmlReqNode[0].Attributes[3].Value = ClinicalSummary.MaritalStatus;
            }
            else
            {
                xmlReqNode[0].RemoveAll();
                XmlAttribute xmlNullsdtc = null;
                xmlNullsdtc = xmlDoc.CreateAttribute("nullFlavor");
                xmlNullsdtc.Value = "UNK";
                xmlReqNode[0].Attributes.Append(xmlNullsdtc);

            }

            if (ClinicalSummary.lookupList != null)
            {
                for (int Race = 0; Race < ClinicalSummary.lookupList.Count; Race++)
                {
                    if (ClinicalSummary.lookupList[Race].Field_Name == "ETHNICITY")
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
                        if (ClinicalSummary.lookupList[Race].Default_Value == "")
                        {
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNull = null;
                            xmlNull = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNull.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNull);

                            xmlReqNode = xmlDoc.GetElementsByTagName("sdtc:raceCode");
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNullsdtc = null;
                            xmlNullsdtc = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNullsdtc.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNullsdtc);
                        }
                        else
                        {
                            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.lookupList[Race].Default_Value.ToString();
                            xmlReqNode[0].Attributes[1].Value = ClinicalSummary.lookupList[Race].Value.ToString();
                        }
                    }
                    if (ClinicalSummary.lookupList[Race].Field_Name == "GRANULARITY")
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("sdtc:raceCode");
                        if (ClinicalSummary.lookupList[Race].Default_Value == "")
                        {
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNull = null;
                            xmlNull = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNull.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNull);

                            xmlReqNode = xmlDoc.GetElementsByTagName("sdtc:raceCode");
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNullsdtc = null;
                            xmlNullsdtc = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNullsdtc.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNullsdtc);
                        }
                        else
                        {
                            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.lookupList[Race].Default_Value.ToString();
                            xmlReqNode[0].Attributes[1].Value = ClinicalSummary.lookupList[Race].Value.ToString();
                        }
                    }
                    else if (ClinicalSummary.lookupList[Race].Field_Name == "RACE")
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("raceCode");
                        if (ClinicalSummary.lookupList[Race].Default_Value == "")
                        {
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNull = null;
                            xmlNull = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNull.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNull);
                        }
                        else
                        {
                            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.lookupList[Race].Default_Value.ToString();
                            xmlReqNode[0].Attributes[1].Value = ClinicalSummary.lookupList[Race].Value.ToString();
                        }
                    }
                }

                if (ClinicalSummary.lookupList.Count == 0)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
                    xmlReqNode[0].RemoveAll();
                    XmlAttribute xmlNull = null;
                    xmlNull = xmlDoc.CreateAttribute("nullFlavor");
                    xmlNull.Value = "UNK";
                    xmlReqNode[0].Attributes.Append(xmlNull);

                    xmlReqNode = xmlDoc.GetElementsByTagName("sdtc:raceCode");
                    xmlReqNode[0].RemoveAll();
                    XmlAttribute xmlNullsdtc = null;
                    xmlNullsdtc = xmlDoc.CreateAttribute("nullFlavor");
                    xmlNullsdtc.Value = "UNK";
                    xmlReqNode[0].Attributes.Append(xmlNullsdtc);

                    xmlReqNode = xmlDoc.GetElementsByTagName("raceCode"); xmlReqNode[0].RemoveAll();
                    XmlAttribute xmlNullraceCode = null;
                    xmlNullraceCode = xmlDoc.CreateAttribute("nullFlavor");
                    xmlNullraceCode.Value = "UNK";
                    xmlReqNode[0].Attributes.Append(xmlNullraceCode);
                }
            }




            //xmlReqNode = xmlDoc.GetElementsByTagName("raceCode");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Race_No;
            //xmlReqNode[0].Attributes[3].Value = ClinicalSummary.Race;

            //xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Ethinicity_No;
            //xmlReqNode[0].Attributes[3].Value = ClinicalSummary.Ethnicity;


            #endregion

            #region author

            xmlReqNode = xmlDoc.GetElementsByTagName("time");
            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss+0500");


            string sfacility = string.Empty;
            if (ClinicalSummary.Encounter != null && ClinicalSummary.Encounter.Count > 0)
                sfacility = ClinicalSummary.Encounter[0].Facility_Name;
            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[1].InnerText = sfacility + ", " + Phy.PhyAddress1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[1].InnerText = Phy.PhyCity;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[1].InnerText = Phy.PhyState;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[1].InnerText = Phy.PhyZip;
            xmlReqNode = xmlDoc.GetElementsByTagName("country");
            xmlReqNode[1].InnerText = "US";


            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            string sphyTelephoneno = Phy.PhyTelephone;
            if (sphyTelephoneno != string.Empty)
                xmlDoc.GetElementsByTagName("author")[0].ChildNodes[1].ChildNodes[2].Attributes[1].Value = "tel:+1" + sphyTelephoneno;
            else
                xmlDoc.GetElementsByTagName("author")[0].ChildNodes[1].ChildNodes[2].Attributes[1].Value = "tel:+1" + "(909) 621-4949";

            xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
            xmlReqNode[0].InnerText = Phy.PhyPrefix;
            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[1].InnerText = Phy.PhyFirstName;
            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[1].InnerText = Phy.PhyLastName;


            #endregion
            #region dataenterer
            xmlReqNode = xmlDoc.GetElementsByTagName("dataEnterer");

            if (ClinicalSummary.facilityLibraryCustodian.Count > 0)
            {
                xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_Address1;
                xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_City;
                xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[2].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_State;
                xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[3].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_Zip;
                if (ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone.Trim() != "")
                    xmlReqNode[0].ChildNodes[0].ChildNodes[2].Attributes[1].Value = "tel:+1" + ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone;
                else
                    xmlReqNode[0].ChildNodes[0].ChildNodes[2].Attributes[1].Value = "tel:+1" + "(111) 111-1111";

            }
            xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[4].InnerText = "US";
            if (ClinicalSummary.Encounter != null && ClinicalSummary.Encounter.Count > 0)
            {

                UserManager obj = new UserManager();
                IList<User> lst = new List<User>();
                lst = obj.GetMedicalAssistantUserList();
                lst = (from m in lst where m.user_name == ClinicalSummary.Encounter[0].Assigned_Med_Asst_User_Name select m).ToList<User>();
                if (lst.Count > 0)
                {
                    string[] name = lst[0].person_name.Split(' ');
                    xmlReqNode[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].InnerText = name[0];
                    if (name.Count() > 1)
                        xmlReqNode[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].InnerText = name[0];
                }
            }
            #endregion
            //new
            #region custodian
            xmlReqNode = xmlDoc.GetElementsByTagName("name");
            if (ClinicalSummary.Guarantor_human_MI == "")
            {
                xmlReqNode[2].InnerText = ClinicalSummary.Guarantor_human_Lastname + "," + ClinicalSummary.Guarantor_human_name;
            }
            else
            {
                xmlReqNode[2].InnerText = ClinicalSummary.Guarantor_human_Lastname + "," + ClinicalSummary.Guarantor_human_name + ClinicalSummary.Guarantor_human_MI;
            }
            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            //string sgurantorphoneno = ClinicalSummary.Guarantor_Home_Phone_No;
            //if (sgurantorphoneno != string.Empty)
            //    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + sgurantorphoneno;
            //else
            //    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + "(909) 621-4949";

            if (ClinicalSummary.facilityLibraryCustodian != null && ClinicalSummary.facilityLibraryCustodian.Count > 0)
            {
                for (int i = 0; i < ClinicalSummary.facilityLibraryCustodian.Count; i++)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                    xmlReqNode[3].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_Address1;
                    xmlReqNode = xmlDoc.GetElementsByTagName("city");
                    xmlReqNode[3].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_City;
                    xmlReqNode = xmlDoc.GetElementsByTagName("state");
                    xmlReqNode[3].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_State;
                    xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                    xmlReqNode[3].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_Zip;
                    xmlReqNode = xmlDoc.GetElementsByTagName("country");
                    xmlReqNode[3].InnerText = "US";

                    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_Name;
                    if (ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone.Trim() != "")
                        xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone;
                    else
                        xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + "(111) 111-1111";


                }
            }


            #endregion

            xmlReqNode = xmlDoc.GetElementsByTagName("id");
            xmlReqNode[1].Attributes[0].Value = ClinicalSummary.Encounter[0].Id.ToString();//doubt

            #region documentationOf
            if (ClinicalSummary.PhysicianLibraryDocumentation != null && ClinicalSummary.PhysicianLibraryDocumentation.Count != 0)
            {
                for (int i = 0; i < ClinicalSummary.PhysicianLibraryDocumentation.Count; i++)
                {
                    //XmlElement TempElement = null;
                    //XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("component")[0];
                    //XmlDocument docAllergyEntry = new XmlDocument();
                    //docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CareTeam.xml"));


                    //XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                    //xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                    ////xmlDoc.DocumentElement.LastChild.ChildNodes[0].ChildNodes[27].AppendChild(xfragAllergy);
                    //elemOldentry.PreviousSibling.AppendChild(xfragAllergy);
                    //xmlDoc.Save(sPrintFileName);
                    //xmlDoc.Load(sPrintFileName);



                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("custodian")[0];
                    //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();



                    xmlReqNode = xmlDoc.GetElementsByTagName("low");
                    if (ClinicalSummary.PhysicianLibraryDocumentation[i].Encounter_Date_of_Service.ToString() != "" || ClinicalSummary.PhysicianLibraryDocumentation[i].Encounter_Date_of_Service.ToString("yyyyMMdd") != "00010101")
                    {
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = ClinicalSummary.PhysicianLibraryDocumentation[i].Encounter_Date_of_Service.ToString("yyyyMMdd");
                    }
                    else if (ClinicalSummary.Encounter.Count > 0)
                    {
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                    }
                    else
                    {
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = "00010101";
                    }
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");

                    //commented Start
                    //xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyPrefix;

                    //xmlReqNode = xmlDoc.GetElementsByTagName("given");
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyFirstName;
                    //xmlReqNode = xmlDoc.GetElementsByTagName("family");
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyLastName;

                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = Phy.PhyAddress1;
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = Phy.PhyCity;
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = Phy.PhyState;
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = Phy.PhyZip;

                    //if (sphyTelephoneno != string.Empty)
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + sphyTelephoneno;
                    //else
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + "(909) 621-4949";

                    //if (ClinicalSummary.PhysicianLibraryDocumentation.Count == 1)
                    //{
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
                    //    string sMedicalAssistant = string.Empty;
                    //    if (ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant == string.Empty)
                    //        sMedicalAssistant = "_";
                    //    else
                    //        sMedicalAssistant = ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = sMedicalAssistant;
                    //}
                    //else
                    //{
                    //    i += 1;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyPrefix + " " + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyFirstName + " " + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyLastName;
                    //}


                    //commented end

                    //i += 1;


                    xmlReqNode = xmlDoc.GetElementsByTagName("performer");

                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[4].InnerText = "US";
                    if (ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone.Trim() != "")
                        xmlReqNode[0].ChildNodes[1].ChildNodes[3].Attributes[1].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
                    else
                        xmlReqNode[0].ChildNodes[1].ChildNodes[3].Attributes[1].Value = "tel:+1" + "(111) 111-1111";

                    xmlReqNode[0].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyPrefix;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyFirstName;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyLastName;





                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_Address1;
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_City;
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_State;
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_Zip;
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[4].InnerText = "US";
                    if (ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone.Trim() != "")
                        xmlReqNode[1].ChildNodes[1].ChildNodes[3].Attributes[1].Value = "tel:+1" + ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone;
                    else
                        xmlReqNode[1].ChildNodes[1].ChildNodes[3].Attributes[1].Value = "tel:+1" + "(111) 111-1111";

                    if (ClinicalSummary.Encounter != null && ClinicalSummary.Encounter.Count > 0)
                    {

                        UserManager obj = new UserManager();
                        IList<User> lst = new List<User>();
                        lst = obj.GetMedicalAssistantUserList();
                        lst = (from m in lst where m.user_name == ClinicalSummary.Encounter[0].Assigned_Med_Asst_User_Name select m).ToList<User>();
                        if (lst.Count > 0)
                        {
                            string[] name = lst[0].person_name.Split(' ');
                            xmlReqNode[1].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = name[0];
                            sMAName = name[0];
                            if (name.Count() > 1)
                                xmlReqNode[1].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[1].InnerText = name[1];
                        }
                    }



                    break;


                    // xfrag.InnerXml = sInnerXML;
                    //elemOld.LastChild.AppendChild(xfrag);
                    //elemOld.InsertBefore(xfrag);
                }

            }
            else
            {
                //Documentation Low value 
                if (ClinicalSummary.Encounter.Count > 0)
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                else
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = "00010101";

            }
            //xmlReqNode = xmlDoc.GetElementsByTagName("low");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");//doubt



            //xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = Phy.PhyPrefix;
            //xmlReqNode[0].InnerText = Phy.PhyPrefix;

            //xmlReqNode = xmlDoc.GetElementsByTagName("given");
            //xmlReqNode[2].InnerText = Phy.PhyFirstName;
            //xmlReqNode = xmlDoc.GetElementsByTagName("family");
            //xmlReqNode[2].InnerText = Phy.PhyLastName;

            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = Phy.PhyAddress1;
            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = Phy.PhyCity;
            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = Phy.PhyState;
            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = Phy.PhyZip;

            //if (sphyTelephoneno != string.Empty)
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + sphyTelephoneno;
            //else
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + "(909) 621-4949";

            ////PhyAssistant
            //for (int i = 0; i < ClinicalSummary.PhysicianLibraryDocumentation.Count; i++)
            //{
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
            //    //name
            //    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = "Dr.";
            //    //if (ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant != string.Empty)
            //    //{
            //    //string[] sMedAssistant = ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant.Split(' ');
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant;
            //    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[2].InnerText = sMedAssistant[1].ToString();
            //    // }
            //}

            #endregion

            #endregion

            //XmlElement Parent = null;
            //XmlElement Child = null;
            //XmlElement MyParent = null;
            //XmlElement MyTempParent = null;
            //int iTableCount = 0;

            #region Health Concern Section


            if (ClinicalSummary.ProblemListing != null && ClinicalSummary.ProblemListing.Count != 0)
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[3].InnerText == "Health Concerns Section")
                {

                   // XmlElement TempElement = null;

                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[4];

                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    for (int i = 0; i < ClinicalSummary.ProblemListing.Count; i++)
                    {
                        if (i == 0)
                        {
                            sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                        }
                        //DateTime datelow = new DateTime();
                        //string sdate = ClinicalSummary.ProblemListing[i].Date_Diagnosed;
                        //switch (sdate.Length)
                        //{
                        //    case 0:
                        //        sdate = "01-Jan-0001";
                        //        break;
                        //    case 1:
                        //        sdate = sdate + "-Jan-0001";
                        //        break;
                        //    case 2:
                        //        sdate = sdate + "-Jan-0001";
                        //        break;
                        //    case 3:
                        //        sdate = "01-" + sdate + "-0001";
                        //        break;
                        //    case 4:
                        //        sdate = "01-Jan-" + sdate;
                        //        break;
                        //    case 5:
                        //        sdate = sdate + "0001";
                        //        break;
                        //    case 6:
                        //        sdate = sdate.Substring(0, 2) + "Jan" + sdate.Substring(2, 4);
                        //        break;
                        //    case 7:
                        //        sdate = "01" + sdate;
                        //        break;
                        //}
                        //datelow = Convert.ToDateTime(sdate);

                        string sDate = string.Empty;
                        if (ClinicalSummary.ProblemListing[i].Date_Diagnosed.Trim() != string.Empty)
                        {
                            if (ClinicalSummary.ProblemListing[i].Date_Diagnosed.Contains('-') == true)
                            {
                                if (ClinicalSummary.ProblemListing[i].Date_Diagnosed.Split('-').Length > 2)
                                    sDate = Convert.ToDateTime(ClinicalSummary.ProblemListing[i].Date_Diagnosed).ToString("MMMM dd,yyyy");
                                else if (ClinicalSummary.ProblemListing[i].Date_Diagnosed.Split('-').Length > 1)
                                    sDate = Convert.ToDateTime(ClinicalSummary.ProblemListing[i].Date_Diagnosed).ToString("MMMM ,yyyy");

                            }
                            else
                                sDate = ClinicalSummary.ProblemListing[i].Date_Diagnosed;
                        }

                        sInnerXML += "<tr><td>" + ClinicalSummary.ProblemListing[i].Problem_Description + ", SNOMED-CT: " + ClinicalSummary.ProblemListing[i].Snomed_Code + ", " + ClinicalSummary.ProblemListing[i].Status + "</td><td>" + "Active" + "</td><td>" + sDate + "</td></tr>";
                        if (i == ClinicalSummary.ProblemListing.Count - 1)
                        {
                            if (ClinicalSummary.Assessment.Count > 0)
                            {

                                IList<Assessment> ass = new List<Assessment>();
                                ass = ClinicalSummary.Assessment.Where(a => a.Chronic_Problem.ToUpper() == "BOTH" || a.Chronic_Problem.ToUpper() == "CHRONIC").ToList();
                                if (ass.Count > 0)
                                {
                                    sInnerXML += "<tr><td>Chronic Sickness exhibited by patient, 161901003 SNOMED-CT</td></tr>";
                                }
                            }

                            sInnerXML += "</tbody>";
                        }
                    }


                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);
                    xmlDoc.Save(sPrintFileName);
                    xmlDoc.Load(sPrintFileName);
                    XmlElement elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1];

                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "SampleXML\\HealthConcern_Careplan.xml"));

                    for (int i = 0; i < ClinicalSummary.ProblemListing.Count; i++)
                    {
                       
                        String sDate = DateTime.Now.ToString("yyyyMMddhhmmss");

                        DateTime digDate = DateTime.MinValue;
                        if (ClinicalSummary.ProblemListing[i].Date_Diagnosed.Length>9)
                        {
                            digDate= DateTime.Parse(ClinicalSummary.ProblemListing[i].Date_Diagnosed);
                        }
                        

                            XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                       
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml.Replace("{CurrentDate}", sDate).Replace("{PhysicianLibraryID}", Phy.PhyId.ToString()).Replace("{DateDiagnosed}", digDate.ToString("yyyyMMdd")).Replace("{NPI}", Phy.PhyNPI).Replace("{TaxonomicalCode}", Phy.Taxonomy_Code).Replace("{SnomedCode}",ClinicalSummary.ProblemListing[i].Snomed_Code).Trim().Replace("{SnomedDescription}", ClinicalSummary.ProblemListing[i].Snomed_Code_Description).Replace("{TaxonomicalDescription}", Phy.Taxonomy_Description).Replace("&", "&amp;");

                      
                        elemOldEntry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1];
                        
                        
                         //XmlElement elemProcedureCode = null;
                        //elemProcedureCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[5].ChildNodes[0].ChildNodes[2];
                        //elemProcedureCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[1].ChildNodes[4 + i].LastChild.ChildNodes[5].LastChild.ChildNodes[1];
                        // elemProcedureCode.Attributes[0].Value = ClinicalSummary.ProblemListing[i].ICD_9_Description;//ICD_ID value
                        //if (ClinicalSummary.ProblemListing[i].ICD_9_Description != "")
                        //    elemProcedureCode.Attributes[0].Value = ClinicalSummary.ProblemListing[i].ICD_9_Description;//ICD_ID value
                        //else
                        //    elemProcedureCode.Attributes[0].Value = "a2788ab0-2993-5ff9-aaae-8e120e83b31b";//ICD_ID value

                        //elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37];
                        //XmlElement elemProcedureCode = null;
                        //elemProcedureCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[4 + i].LastChild.ChildNodes[5].LastChild.ChildNodes[1];
                        //elemProcedureCode.Attributes[0].Value = ClinicalSummary.ProblemListing[i].ICD;
                    }

                    //int iOldValue = 0;

                    //for (int i = 0; i < ClinicalSummary.Allergy.Count; i++)
                    //{

                    //    XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                    //    xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                    //    elemOldEntry.LastChild.AppendChild(xfragAllergy);
                    //    xmlDoc.Save(sPrintFileName);
                    //    xmlDoc.Load(sPrintFileName);
                    //    elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    //    elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1];

                    //    XmlElement elemReaction = null;
                    //    XmlElement elemSeverity = null;

                    //    elemReaction = (XmlElement)xmlDoc.GetElementsByTagName("participant")[i].ChildNodes[0].ChildNodes[0].ChildNodes[0];

                    //    if (i == 0)
                    //    {
                    //        elemSeverity = (XmlElement)xmlDoc.GetElementsByTagName("entryRelationship")[1].ChildNodes[0].ChildNodes[6];
                    //        iOldValue = 1;
                    //    }
                    //    else
                    //    {
                    //        iOldValue = iOldValue + 4;
                    //        elemSeverity = (XmlElement)xmlDoc.GetElementsByTagName("entryRelationship")[iOldValue].ChildNodes[0].ChildNodes[6];

                    //    }
                    //    //elemReaction.Attributes[0].Value = ClinicalSummary.Allergy[i].First_DataBank_Med_ID;
                    //    elemReaction.Attributes[0].Value = ClinicalSummary.Allergy[i].Rxnorm_ID.ToString();
                    //    elemReaction.Attributes[3].Value = ClinicalSummary.Allergy[i].Allergy_Name;
                    //    // elemSeverity.Attributes[0].Value = ClinicalSummary.Allergy[i].First_DataBank_Med_ID;
                    //    elemSeverity.Attributes[3].Value = ClinicalSummary.Allergy[i].Reaction;
                    //}
                    ////elemOld.LastChild.AppendChild(xfrag);
                }
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[4];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>No Information</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion

            #region Goals Section
            if (ClinicalSummary.TreatmentPlan != null && ClinicalSummary.TreatmentPlan.Count != 0)
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[3].InnerText == "Goals Section")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    for (int i = 0; i < ClinicalSummary.TreatmentPlan.Count; i++)
                    {
                        if (i == 0)
                        {
                            sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                        }
                        if (ClinicalSummary.TreatmentPlan[i].Plan_For_Plan.Trim() != "")
                            sInnerXML += "<tr><td>" + ClinicalSummary.TreatmentPlan[i].Plan.Replace("-Vitals", "").Replace("-Problem List", "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</td><td>" + ClinicalSummary.TreatmentPlan[i].Plan_For_Plan + " </td><td>" + ClinicalSummary.Encounter[0].Date_of_Service.ToString("MMMM dd, yyyy") + "</td></tr>";
                        else
                            sInnerXML += "<tr><td>" + ClinicalSummary.TreatmentPlan[i].Plan.Replace("-Vitals", "").Replace("-Problem List", "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</td><td> N/A </td><td>" + ClinicalSummary.Encounter[0].Date_of_Service.ToString("MMMM dd, yyyy") + "</td></tr>";
                        if (i == ClinicalSummary.TreatmentPlan.Count - 1)
                        {
                            sInnerXML += "</tbody>";
                        }
                    }

                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3];
                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "SampleXML\\Goal_Section.xml"));

                    for (int i = 0; i < ClinicalSummary.TreatmentPlan.Count; i++)
                    {
                        String sDate = DateTime.Now.ToString("yyyyMMddhhmmss");

                        XmlDocumentFragment xfragGoals = xmlDoc.CreateDocumentFragment();

                        xfragGoals.InnerXml = docAllergyEntry.DocumentElement.InnerXml.Replace("{PhysicianFirstName}", Phy.PhyFirstName).Replace("{PhysicianLastName}", Phy.PhyLastName).Replace("{PhysicianSuffix}", Phy.PhySuffix).Replace("{CurrentDate}", sDate).Replace("{PhysicianLibraryID}", Phy.Id.ToString()).Replace("{NPI}", Phy.PhyNPI).Replace("{TaxonomicalCode}", Phy.Taxonomy_Code).Replace("{TaxonomicalDescription}", Phy.Taxonomy_Description).Replace("&", "&amp;");


                        elemOldentry.LastChild.AppendChild(xfragGoals);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        //XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        //xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        //elemOldentry.LastChild.AppendChild(xfragAllergy);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);


                        //elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3];

                        //XmlElement elemEffectiveTime = null;
                        //XmlElement ePlanGoal = null;
                        //XmlElement family = null;
                        //XmlElement eSpecialityNPI = null;
                        //XmlElement given = null;
                        //XmlElement suffix = null;
                        //XmlElement eTime = null;
                        //XmlElement AssignedAuthor = null;
                        //elemEffectiveTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[4];
                        //elemEffectiveTime.Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                        //ePlanGoal = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[5];
                        //ePlanGoal.InnerText = ClinicalSummary.TreatmentPlan[i].Plan.Replace("-Vitals", "").Replace("-Problem List", "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                        //eTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[6].ChildNodes[1];
                        //eTime.Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                        //eSpecialityNPI = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[1];
                        ////if (ClinicalSummary.phyList[0].PhyColor.Trim() != string.Empty)
                        ////{
                        ////if (ClinicalSummary.phyList[0].PhyColor.Contains('-'))
                        ////{
                        ////eSpecialityNPI.Attributes[0].Value = ClinicalSummary.phyList[0].PhyColor.Split('-')[0];
                        ////eSpecialityNPI.Attributes[1].Value = ClinicalSummary.phyList[0].PhyColor.Split('-')[1];
                        ////    }
                        ////}
                        //eSpecialityNPI.Attributes[0].Value = ClinicalSummary.phyList[0].Taxonomy_Code;
                        //eSpecialityNPI.Attributes[1].Value = ClinicalSummary.phyList[0].Taxonomy_Description.Replace("&", "&amp;");
                        //given = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[0];
                        //given.InnerText = ClinicalSummary.phyList[0].PhyFirstName;
                        //family = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[1];
                        //family.InnerText = ClinicalSummary.phyList[0].PhyLastName;
                        //suffix = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[1];
                        //suffix.InnerText = ClinicalSummary.phyList[0].PhySuffix;
                        //AssignedAuthor = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[7].ChildNodes[2].ChildNodes[0];
                        //if (ClinicalSummary.PhysicianLibraryDocumentation.Count > 0)
                        //{
                        //    AssignedAuthor.Attributes[0].Value = ClinicalSummary.PhysicianLibraryDocumentation[0].Physician_SSN_Number;
                        //}
                        //else
                        //    AssignedAuthor.Attributes[0].Value = "111-111-111";
                        }
                }
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4];

                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>No Information</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            #endregion

            #region Interventions Section
            #region INETRVENTION_VITALS_CAREPLAN
            string sInnerVitalsIntervention = string.Empty;
            if (ClinicalSummary.Vitals != null && ClinicalSummary.Vitals.Count > 0)
            {
                for (int i = 0; i < ClinicalSummary.Vitals.Count; i++)
                {
                    if (ClinicalSummary.Vitals[i].Notes.Trim() != string.Empty)
                    {
                        sInnerVitalsIntervention += "<tr><td>" + ClinicalSummary.Vitals[i].Loinc_Observation + " - " + ClinicalSummary.Vitals[i].Notes + "</td><td>Yes</td><td>" + ClinicalSummary.Vitals[i].Captured_date_and_time.ToString("MMMM dd, yyyy") + "</td></tr>";
                    }
                }

            }
            string sInnerCarePlanIntervention = string.Empty;
            if (ClinicalSummary.Careplan != null && ClinicalSummary.Careplan.Count > 0)
            {
                for (int i = 0; i < ClinicalSummary.Careplan.Count; i++)
                {
                    sInnerCarePlanIntervention += "<tr><td>" + ClinicalSummary.Careplan[i].Care_Plan_Notes + "</td><td>" + ClinicalSummary.Careplan[i].Status + "</td><td>" + ClinicalSummary.Encounter[0].Date_of_Service.ToString("MMMM dd, yyyy") + "</td></tr>";
                }
            }

            if (sInnerVitalsIntervention.Trim() != string.Empty || sInnerCarePlanIntervention.Trim() != string.Empty)
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[4].InnerText == "Interventions Section")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[5];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                    sInnerXML += sInnerCarePlanIntervention + sInnerVitalsIntervention;
                    sInnerXML += "</tbody>";
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5];
                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "SampleXML\\Intervention.xml"));

                    string sEntryTextHere = string.Empty;

                    for (int i = 0; i < ClinicalSummary.Vitals.Count; i++)
                    {
                        String sDate = DateTime.Now.ToString("yyyyMMddhhmmss");

                        XmlDocumentFragment xfragIntervention = xmlDoc.CreateDocumentFragment();

                        sEntryTextHere = "<code code=\"LoincCode\" displayName=\"LoincDescription\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName =\"LOINC\"/><text>LoincDescription</text>";

                        sEntryTextHere = sEntryTextHere.Replace("LoincCode", ClinicalSummary.Vitals[i].Loinc_Identifier);
                        sEntryTextHere = sEntryTextHere.Replace("LoincDescription", ClinicalSummary.Vitals[i].Loinc_Observation);

                        xfragIntervention.InnerXml = docAllergyEntry.DocumentElement.InnerXml.Replace("{EntryTextHere}", sEntryTextHere).Replace("{CurrentDate}", sDate).Replace("{TaxonomicalCode}", Phy.Taxonomy_Code).Replace("{TaxonomicalDescription}", Phy.Taxonomy_Description).Replace("&", "&amp;");


                        elemOldentry.LastChild.AppendChild(xfragIntervention);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }


                    for (int i = 0; i < ClinicalSummary.Careplan.Count; i++)
                    {
                        String sDate = DateTime.Now.ToString("yyyyMMddhhmmss");

                        XmlDocumentFragment xfragIntervention = xmlDoc.CreateDocumentFragment();

                        sEntryTextHere = "<code nullFlavor=\"UNK\"/><text>Description</text>";

                        sEntryTextHere = sEntryTextHere.Replace("Description", ClinicalSummary.Careplan[i].Care_Name);

                        xfragIntervention.InnerXml = docAllergyEntry.DocumentElement.InnerXml.Replace("{EntryTextHere}", sEntryTextHere).Replace("{CurrentDate}", sDate).Replace("{TaxonomicalCode}", Phy.Taxonomy_Code).Replace("{TaxonomicalDescription}", Phy.Taxonomy_Description).Replace("&", "&amp;");

                        elemOldentry.LastChild.AppendChild(xfragIntervention);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }

                    xmlDoc.Save(sPrintFileName);
                    xmlDoc.Load(sPrintFileName);
                }

            }
            #endregion
            #region old_INTERVENTIONS
            //if (ClinicalSummary.Careplan != null && ClinicalSummary.Careplan.Count != 0)
            //{
            //    if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[3].InnerText == "Interventions Section")
            //    {

            //        XmlElement TempElement = null;

            //        XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[4];
            //        XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
            //        string sInnerXML = string.Empty;

            //        for (int i = 0; i < ClinicalSummary.Careplan.Count; i++)
            //        {
            //            if (i == 0)
            //            {
            //                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
            //            }
            //            sInnerXML += "<tr><td>" + ClinicalSummary.Careplan[i].Care_Plan_Notes + "</td><td>" + ClinicalSummary.Careplan[i].Status + "</td><td>" + ClinicalSummary.Encounter[0].Date_of_Service.ToString("MMMM dd, yyyy") + "</td></tr>";
            //            if (i == ClinicalSummary.Careplan.Count - 1)
            //            {
            //                sInnerXML += "</tbody>";
            //            }
            //        }

            //        xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
            //        elemOld.LastChild.AppendChild(xfrag);

            //        XmlElement elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3];
            //        XmlDocument docAllergyEntry = new XmlDocument();

                    //docAllergyEntry.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "SampleXML\\Intervention.xml"));



                    //for (int i = 0; i < ClinicalSummary.Encounter.Count; i++)
            //{

                    //    XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
            //    xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
            //    elemOldEntry.LastChild.AppendChild(xfragAllergy);
            //    xmlDoc.Save(sPrintFileName);
            //    xmlDoc.Load(sPrintFileName);

                    //    elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3];
            //    XmlElement elemVisitType = null;
            //    XmlElement elemDOS = null;
            //    XmlElement elemFacilityName = null;
            //    XmlElement elemAddress = null;
            //    XmlElement elemCity = null;
            //    XmlElement elemState = null;
            //    XmlElement elemPostalCode = null;

                    //    XmlElement elemName = null;
            //    XmlElement elemReasonDOS = null;
            //    XmlElement elemReasonTypeVisit = null;

                    //    elemVisitType = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[2];
            //    elemDOS = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[4];

                    //    elemFacilityName = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[1];
            //    elemAddress = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[0];
            //    elemCity = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[1];
            //    elemState = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[2];
            //    elemPostalCode = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[3];
            //    elemName = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[4].ChildNodes[0];

                    //    elemReasonDOS = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[9].ChildNodes[0].ChildNodes[4].ChildNodes[0];
            //    elemReasonTypeVisit = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[9].ChildNodes[0].ChildNodes[5];

                    //    elemVisitType.Attributes[3].Value = "InPatient Admission";//ClinicalSummary.Encounter[i].Visit_Type;
            //    elemDOS.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");

                    //    elemFacilityName.Attributes[3].Value = ClinicalSummary.Encounter[i].Facility_Name;
            //    FacilityManager objFacilityMngr = new FacilityManager();
            //    IList<FacilityLibrary> ilstFacility = new List<FacilityLibrary>();
            //    ilstFacility = objFacilityMngr.GetFacilityByFacilityname(ClinicalSummary.Encounter[i].Facility_Name.ToString());
            //    if (ilstFacility != null && ilstFacility.Count > 0)
            //    {
            //        elemAddress.InnerText = ilstFacility[0].Fac_Address1;

                    //        elemCity.InnerText = ilstFacility[0].Fac_City;
            //        elemState.InnerText = ilstFacility[0].Fac_State;

                    //        elemPostalCode.InnerText = ilstFacility[0].Fac_Zip;
            //        elemName.InnerText = ilstFacility[0].Fac_Name;
            //    }
            //    elemReasonDOS.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
            //    elemReasonTypeVisit.Attributes[2].Value = ClinicalSummary.Encounter[i].Purpose_of_Visit;

                    //    int k = 0;
            //    for (int j = 0; j < ClinicalSummary.EncounterDiagnosis.Count; j++)
            //    {
            //        XmlElement elemOldEntry1 = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i];
            //        XmlDocument docAllergyEntry1 = new XmlDocument();

                    //        docAllergyEntry1.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\EncounterDiagnosis.xml"));
            //        XmlDocumentFragment xfragAllergy1 = xmlDoc.CreateDocumentFragment();
            //        xfragAllergy1.InnerXml = docAllergyEntry1.DocumentElement.InnerXml;
            //        elemOldEntry1.AppendChild(xfragAllergy1);
            //        xmlDoc.Save(sPrintFileName);
            //        xmlDoc.Load(sPrintFileName);

                    //        XmlElement elemDiagnosisActLow = null;
            //        XmlElement elemDiagnosisobsLow = null;
            //        XmlElement elemDiagnosisValue = null;
            //        if (j == 0)
            //        {
            //            elemDiagnosisActLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[11].ChildNodes[0].ChildNodes[4].ChildNodes[0];
            //            elemDiagnosisobsLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[11].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[4].ChildNodes[0];
            //            elemDiagnosisActLow.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
            //            elemDiagnosisobsLow.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
            //            elemDiagnosisValue = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[11].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[5];
            //            elemDiagnosisValue.Attributes[1].Value = ClinicalSummary.EncounterDiagnosis[j].Snomed_Code;
            //            elemDiagnosisValue.Attributes[3].Value = ClinicalSummary.EncounterDiagnosis[j].Snomed_Code_Description;
            //            k = 11;
            //        }
            //        else
            //        {
            //            elemDiagnosisActLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[k + 2].ChildNodes[0].ChildNodes[4].ChildNodes[0];
            //            elemDiagnosisobsLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[k + 2].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[4].ChildNodes[0];
            //            elemDiagnosisActLow.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
            //            elemDiagnosisobsLow.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
            //            elemDiagnosisValue = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[k + 2].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[5];
            //            elemDiagnosisValue.Attributes[1].Value = ClinicalSummary.EncounterDiagnosis[j].Snomed_Code;
            //            elemDiagnosisValue.Attributes[3].Value = ClinicalSummary.EncounterDiagnosis[j].Snomed_Code_Description;
            //            k += 2;
            //        }

                    //    }


                    //}
            //}
            //}
            #endregion
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[5];

                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            #endregion

            #region Health Status Evaluations


            if (ClinicalSummary.Vitals != null && ClinicalSummary.Vitals.Count != 0)
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[3].InnerText == "Health Status Evaluations/Outcomes Section")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    for (int i = 0; i < ClinicalSummary.Vitals.Count; i++)
                    {
                        if (i == 0)
                        {
                            sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                        }
                        sInnerXML += "<tr><td>" + ClinicalSummary.Vitals[i].Loinc_Observation + "</td><td>" + ClinicalSummary.Vitals[i].Value + ClinicalSummary.Vitals[i].Units + "</td><td>" + ClinicalSummary.Encounter[0].Date_of_Service.ToString("MMMM dd, yyyy") + "</td></tr>";
                        if (i == ClinicalSummary.Vitals.Count - 1)
                        {
                            sInnerXML += "</tbody>";
                        }
                    }


                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7];
                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "SampleXML\\Health_Status_Evaluation.xml"));

                    for (int i = 0; i < ClinicalSummary.Vitals.Count; i++)
                    {
                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldentry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);


                        elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7];

                        XmlElement elemcode = null;
                        XmlElement eledisplayName = null;
                        XmlElement elemEffectiveTime = null;
                        XmlElement evalue = null;
                        //XmlElement eUnit = null;
                        XmlElement eTime = null;
                        elemcode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[2];
                        elemcode.Attributes[0].Value = ClinicalSummary.Vitals[i].Loinc_Identifier;
                        eledisplayName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[2];
                        eledisplayName.Attributes[3].Value = ClinicalSummary.Vitals[i].Loinc_Observation;
                        elemEffectiveTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[4];
                        elemEffectiveTime.Attributes[0].Value = ClinicalSummary.Vitals[i].Captured_date_and_time.ToString("yyyyMMdd");
                        evalue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[5];
                        evalue.Attributes[1].Value = ClinicalSummary.Vitals[i].Value;
                        //eUnit = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[5];
                        // eUnit.Attributes[2].Value = ClinicalSummary.Vitals[i].Units;
                        //eTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[6].ChildNodes[0];
                        //eTime.Attributes[0].Value = ClinicalSummary.Vitals[i].Captured_date_and_time.ToString("yyyyMMdd")+"selva";

                        if (i == ClinicalSummary.Vitals.Count - 1)
                        {
                            XmlDocument docvitalEntry = new XmlDocument();
                            docvitalEntry.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "SampleXML\\Health_Status_Evaluation_Last_Entry_Section.xml"));
                            XmlDocumentFragment xfragAllergy1 = xmlDoc.CreateDocumentFragment();
                            xfragAllergy1.InnerXml = docvitalEntry.DocumentElement.InnerXml;
                            elemOldentry.LastChild.AppendChild(xfragAllergy1);
                            xmlDoc.Save(sPrintFileName);
                            xmlDoc.Load(sPrintFileName);
                        }
                    }
                }
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion

            #region legalAuthenticator
            xmlReqNode = xmlDoc.GetElementsByTagName("legalAuthenticator");
            xmlReqNode[0].ChildNodes[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss+0500"); ;
            xmlReqNode[0].ChildNodes[2].ChildNodes[1].Attributes[0].Value = Phy.Taxonomy_Code;
            xmlReqNode[0].ChildNodes[2].ChildNodes[1].Attributes[1].Value = Phy.Taxonomy_Description;
            xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[0].InnerText = Phy.PhyAddress1;
            xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = Phy.PhyCity;
            xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[2].InnerText = Phy.PhyState;
            xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[3].InnerText = Phy.PhyZip;
            xmlReqNode[0].ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = Phy.PhyFirstName;
            xmlReqNode[0].ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[1].InnerText = Phy.PhyLastName;
            xmlReqNode[0].ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[2].InnerText = Phy.PhySuffix;

            //  xfragLegal.InnerXml = docLegalentry.DocumentElement.InnerXml.Replace("{TaxonomyCode}", ).Replace("{TaxonomyDiscription}",).Replace("{PhyAddress}",).Replace("{PhyCity}", Phy.PhyCity).Replace("{PhyState}", Phy.PhyState).Replace("{PhyZip}", Phy.PhyZip).Replace("{PhyFirstNmae}", Phy.PhyFirstName).Replace("{PhyFirstNmae}", Phy.PhyLastName).Replace("{PhyFirstNmae}", Phy.PhySuffix);


            #endregion

            #region authenticator
            xmlReqNode = xmlDoc.GetElementsByTagName("authenticator");
            xmlReqNode[0].ChildNodes[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss+0500"); ;
            xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[0].InnerText = Phy.PhyAddress1;
            xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = Phy.PhyCity;
            xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[2].InnerText = Phy.PhyState;
            xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[3].InnerText = Phy.PhyZip;
            xmlReqNode[0].ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = Phy.PhyFirstName;
            xmlReqNode[0].ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[1].InnerText = Phy.PhyLastName;
            xmlReqNode[0].ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[2].InnerText = Phy.PhySuffix;

            //  xfragLegal.InnerXml = docLegalentry.DocumentElement.InnerXml.Replace("{TaxonomyCode}", ).Replace("{TaxonomyDiscription}",).Replace("{PhyAddress}",).Replace("{PhyCity}", Phy.PhyCity).Replace("{PhyState}", Phy.PhyState).Replace("{PhyZip}", Phy.PhyZip).Replace("{PhyFirstNmae}", Phy.PhyFirstName).Replace("{PhyFirstNmae}", Phy.PhyLastName).Replace("{PhyFirstNmae}", Phy.PhySuffix);


            #endregion
            //Middle Name
            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[1].InnerText = ClinicalSummary.MI;
            xmlDoc.GetElementsByTagName("dataEnterer")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].InnerText = sMAName;

            String sCurrentDate = DateTime.Now.ToString("yyyyMMddhhmmss");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("{CurrentDate}", sCurrentDate);
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("{TaxonomicalCode}", Phy.Taxonomy_Code);

            xmlDoc.Save(sPrintFileName);            
            return xmlDoc;
           

        }
        public XmlDocument CreateCCDXML(PhysicianLibrary Phy, FillClinicalSummary ClinicalSummary, string sPrintFileName, Hashtable hashCheckedList)
        {
            //StaticLookupManager staticLookupManager = new StaticLookupManager();
            //IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            //StaticLookUpList = staticLookupManager.getStaticLookupByFieldName(new string[] { "SOCIAL HISTORY OPTION FOR SMOKING HABIT" });
            XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlRef = new XmlDocument();


            // xmlRef.Load(Application.StartupPath + "\\CCDConfig.xml");
            xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CCD_Sample.xml"));
            XmlNodeList xmlReqNode = null;
            string sContent = string.Empty;
            xmlReqNode = xmlDoc.GetElementsByTagName("effectiveTime");
            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss+0500");

            #region PatientRole

            // xmlReqNode = xmlDoc.GetElementsByTagName("id");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.ID.ToString();
            xmlReqNode = xmlDoc.GetElementsByTagName("languageCode");
            if (ClinicalSummary.Preferred_Language != "")
                xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Preferred_Language.ToString();
            xmlReqNode = xmlDoc.GetElementsByTagName("patientRole");
            xmlDoc.GetElementsByTagName("patientRole")[0].ChildNodes[0].Attributes[0].Value = ClinicalSummary.ID.ToString();


            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[0].InnerText = ClinicalSummary.Street_Address1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[0].InnerText = ClinicalSummary.City;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[0].InnerText = ClinicalSummary.State;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[0].InnerText = ClinicalSummary.ZipCode;
            xmlReqNode = xmlDoc.GetElementsByTagName("country");
            xmlReqNode[0].InnerText = "US";

            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            string sTelephoneno = ClinicalSummary.Home_Phone_No;




            if (sTelephoneno != string.Empty)
                xmlReqNode[0].Attributes[0].Value = "tel:+1" + sTelephoneno;
            else
                //For BugID : 28462
                //xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(909) 621-4949";
                xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(111) 111-1111";


            if (ClinicalSummary.HumanList[0].Work_Phone_No != string.Empty)
                xmlReqNode[1].Attributes[0].Value = "tel:+1" + ClinicalSummary.HumanList[0].Work_Phone_No;
            else
                //For BugID : 28462
                //xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(909) 621-4949";
                xmlReqNode[1].Attributes[0].Value = "tel:+1" + "(111) 111-1111";

            if (ClinicalSummary.HumanList[0].Cell_Phone_Number != string.Empty)
                xmlReqNode[2].Attributes[0].Value = "tel:+1" + ClinicalSummary.HumanList[0].Cell_Phone_Number;
            else
                //For BugID : 28462
                //xmlReqNode[0].Attributes[0].Value = "tel:+1" + "(909) 621-4949";
                xmlReqNode[2].Attributes[0].Value = "tel:+1" + "(111) 111-1111";
            #endregion

            #region Patient


            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[0].InnerText = ClinicalSummary.First_Name;



            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[0].InnerText = ClinicalSummary.Last_Name;

            xmlReqNode = xmlDoc.GetElementsByTagName("suffix");
            if (ClinicalSummary.Suffix.Trim() == "")
            {
                xmlReqNode[0].ParentNode.RemoveChild(xmlReqNode[0]);
            }
            else
                xmlReqNode[0].InnerText = ClinicalSummary.Suffix;
            xmlReqNode = xmlDoc.GetElementsByTagName("name");
            if (ClinicalSummary.Previous_Name.Trim() != "")
            {
                xmlReqNode[1].ChildNodes[0].InnerText = ClinicalSummary.Previous_Name.Trim();
                xmlReqNode[1].ChildNodes[1].InnerText = ClinicalSummary.First_Name;
                xmlReqNode[1].ChildNodes[2].InnerText = ClinicalSummary.Last_Name;
            }
            else
            {
                xmlReqNode[1].ParentNode.RemoveChild(xmlReqNode[1]);
            }
            xmlReqNode = xmlDoc.GetElementsByTagName("administrativeGenderCode");
            if (ClinicalSummary.Sex != null && ClinicalSummary.Sex != string.Empty)
            {
                //if (ClinicalSummary.Sex.Substring(0, 1).ToUpper() != "U")
                //{
                xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Sex.Substring(0, 1).ToUpper();
                xmlReqNode[0].Attributes[2].Value = ClinicalSummary.Sex;
                //}
                //else
                //{
                //    xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Sex.Substring(0, 3).ToUpper();
                //    xmlReqNode[0].Attributes[2].Value = ClinicalSummary.Sex;
                //}
            }

            xmlReqNode = xmlDoc.GetElementsByTagName("birthTime");
            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Birth_Date.ToString("yyyyMMdd");


            xmlReqNode = xmlDoc.GetElementsByTagName("maritalStatusCode");
            if (ClinicalSummary.MaritalStatus != null && ClinicalSummary.MaritalStatus != string.Empty)
            {
                xmlReqNode[0].Attributes[0].Value = ClinicalSummary.MaritalStatus.Substring(0, 1).ToUpper(); ;
                xmlReqNode[0].Attributes[3].Value = ClinicalSummary.MaritalStatus;
            }
            if (ClinicalSummary.lookupList != null)
            {
                for (int Race = 0; Race < ClinicalSummary.lookupList.Count; Race++)
                {
                    if (ClinicalSummary.lookupList[Race].Field_Name == "ETHNICITY")
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
                        if (ClinicalSummary.lookupList[Race].Default_Value == "")
                        {
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNull = null;
                            xmlNull = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNull.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNull);

                            xmlReqNode = xmlDoc.GetElementsByTagName("sdtc:raceCode");
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNullsdtc = null;
                            xmlNullsdtc = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNullsdtc.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNullsdtc);
                        }
                        else
                        {
                            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.lookupList[Race].Default_Value.ToString();
                            xmlReqNode[0].Attributes[1].Value = ClinicalSummary.lookupList[Race].Value.ToString();
                        }
                    }
                    if (ClinicalSummary.lookupList[Race].Field_Name == "GRANULARITY")
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("sdtc:raceCode");
                        if (ClinicalSummary.lookupList[Race].Default_Value == "")
                        {
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNull = null;
                            xmlNull = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNull.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNull);

                            xmlReqNode = xmlDoc.GetElementsByTagName("sdtc:raceCode");
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNullsdtc = null;
                            xmlNullsdtc = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNullsdtc.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNullsdtc);
                        }
                        else
                        {
                            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.lookupList[Race].Default_Value.ToString();
                            xmlReqNode[0].Attributes[1].Value = ClinicalSummary.lookupList[Race].Value.ToString();
                        }
                    }
                    else if (ClinicalSummary.lookupList[Race].Field_Name == "RACE")
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("raceCode");
                        if (ClinicalSummary.lookupList[Race].Default_Value == "")
                        {
                            xmlReqNode[0].RemoveAll();
                            XmlAttribute xmlNull = null;
                            xmlNull = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNull.Value = "UNK";
                            xmlReqNode[0].Attributes.Append(xmlNull);
                        }
                        else
                        {
                            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.lookupList[Race].Default_Value.ToString();
                            xmlReqNode[0].Attributes[1].Value = ClinicalSummary.lookupList[Race].Value.ToString();
                        }
                    }
                }

                if (ClinicalSummary.lookupList.Count == 0)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
                    xmlReqNode[0].RemoveAll();
                    XmlAttribute xmlNull = null;
                    xmlNull = xmlDoc.CreateAttribute("nullFlavor");
                    xmlNull.Value = "UNK";
                    xmlReqNode[0].Attributes.Append(xmlNull);

                    xmlReqNode = xmlDoc.GetElementsByTagName("sdtc:raceCode");
                    xmlReqNode[0].RemoveAll();
                    XmlAttribute xmlNullsdtc = null;
                    xmlNullsdtc = xmlDoc.CreateAttribute("nullFlavor");
                    xmlNullsdtc.Value = "UNK";
                    xmlReqNode[0].Attributes.Append(xmlNullsdtc);

                    xmlReqNode = xmlDoc.GetElementsByTagName("raceCode"); xmlReqNode[0].RemoveAll();
                    XmlAttribute xmlNullraceCode = null;
                    xmlNullraceCode = xmlDoc.CreateAttribute("nullFlavor");
                    xmlNullraceCode.Value = "UNK";
                    xmlReqNode[0].Attributes.Append(xmlNullraceCode);
                }
            }




            //xmlReqNode = xmlDoc.GetElementsByTagName("raceCode");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Race_No;
            //xmlReqNode[0].Attributes[3].Value = ClinicalSummary.Race;

            //xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Ethinicity_No;
            //xmlReqNode[0].Attributes[3].Value = ClinicalSummary.Ethnicity;


            #endregion

            #region author

            xmlReqNode = xmlDoc.GetElementsByTagName("time");
            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss+0500");

            string sfacility = string.Empty;
            if (ClinicalSummary.Encounter != null && ClinicalSummary.Encounter.Count > 0)
                sfacility = ClinicalSummary.Encounter[0].Facility_Name;
            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[1].InnerText = sfacility + ", " + Phy.PhyAddress1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[1].InnerText = Phy.PhyCity;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[1].InnerText = Phy.PhyState;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[1].InnerText = Phy.PhyZip;
            xmlReqNode = xmlDoc.GetElementsByTagName("country");
            xmlReqNode[1].InnerText = "US";


            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            string sphyTelephoneno = Phy.PhyTelephone;
            if (sphyTelephoneno != string.Empty)
                xmlDoc.GetElementsByTagName("author")[0].ChildNodes[1].ChildNodes[2].Attributes[1].Value = "tel:+1" + sphyTelephoneno;
            else
                xmlDoc.GetElementsByTagName("author")[0].ChildNodes[1].ChildNodes[2].Attributes[1].Value = "tel:+1" + "(909) 621-4949";

            xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
            xmlReqNode[0].InnerText = Phy.PhyPrefix;
            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[1].InnerText = Phy.PhyFirstName;
            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[1].InnerText = Phy.PhyLastName;
            #endregion

            #region author2
            xmlReqNode = xmlDoc.GetElementsByTagName("author");
            xmlReqNode[1].ChildNodes[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss+0500");


            //For VDT Measure: Lab Location Details
            if (ClinicalSummary.LabLocationList.Count > 0)
            {


                xmlReqNode[1].ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText = ClinicalSummary.LabLocationList[0].Street_Address1;
                xmlReqNode[1].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText = ClinicalSummary.LabLocationList[0].City;
                xmlReqNode[1].ChildNodes[1].ChildNodes[1].ChildNodes[2].InnerText = ClinicalSummary.LabLocationList[0].State;
                xmlReqNode[1].ChildNodes[1].ChildNodes[1].ChildNodes[3].InnerText = ClinicalSummary.LabLocationList[0].ZipCode;
                xmlReqNode[1].ChildNodes[1].ChildNodes[1].ChildNodes[4].InnerText = "US";
                if (ClinicalSummary.LabLocationList[0].Phone_No.Trim() != "")
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].Attributes[1].Value = "tel:+1" + ClinicalSummary.LabLocationList[0].Phone_No;
                else
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].Attributes[1].Value = "tel:+1" + "(111) 111-1111";

            }
            #endregion

            #region dataenterer
            xmlReqNode = xmlDoc.GetElementsByTagName("dataEnterer");

            if (ClinicalSummary.facilityLibraryCustodian.Count > 0)
            {
                xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_Address1;
                xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_City;
                xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[2].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_State;
                xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[3].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_Zip;
                if (ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone.Trim() != "")
                    xmlReqNode[0].ChildNodes[0].ChildNodes[2].Attributes[1].Value = "tel:+1" + ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone;
                else
                    xmlReqNode[0].ChildNodes[0].ChildNodes[2].Attributes[1].Value = "tel:+1" + "(111) 111-1111";

            }
            xmlReqNode[0].ChildNodes[0].ChildNodes[1].ChildNodes[4].InnerText = "US";
            if (ClinicalSummary.Encounter != null && ClinicalSummary.Encounter.Count > 0)
            {

                UserManager obj = new UserManager();
                IList<User> lst = new List<User>();
                lst = obj.GetMedicalAssistantUserList();
                lst = (from m in lst where m.user_name == ClinicalSummary.Encounter[0].Assigned_Med_Asst_User_Name select m).ToList<User>();
                if (lst.Count > 0)
                {
                    string[] name = lst[0].person_name.Split(' ');
                    xmlReqNode[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[0].InnerText = name[0];
                    if (name.Count() > 1)
                        xmlReqNode[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[1].InnerText = name[1];
                }
            }
            #endregion
            //new
            #region custodian
            xmlReqNode = xmlDoc.GetElementsByTagName("name");
            if (ClinicalSummary.Guarantor_human_MI == "")
            {
                xmlReqNode[3].InnerText = ClinicalSummary.Guarantor_human_Lastname + "," + ClinicalSummary.Guarantor_human_name;
            }
            else
            {
                xmlReqNode[3].InnerText = ClinicalSummary.Guarantor_human_Lastname + "," + ClinicalSummary.Guarantor_human_name + ClinicalSummary.Guarantor_human_MI;
            }
            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            //string sgurantorphoneno = ClinicalSummary.Guarantor_Home_Phone_No;
            //if (sgurantorphoneno != string.Empty)
            //    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + sgurantorphoneno;
            //else
            //    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + "(909) 621-4949";

            if (ClinicalSummary.facilityLibraryCustodian != null && ClinicalSummary.facilityLibraryCustodian.Count > 0)
            {
                for (int i = 0; i < ClinicalSummary.facilityLibraryCustodian.Count; i++)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                    xmlReqNode[4].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_Address1;
                    xmlReqNode = xmlDoc.GetElementsByTagName("city");
                    xmlReqNode[4].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_City;
                    xmlReqNode = xmlDoc.GetElementsByTagName("state");
                    xmlReqNode[4].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_State;
                    xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                    xmlReqNode[4].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_Zip;
                    xmlReqNode = xmlDoc.GetElementsByTagName("country");
                    xmlReqNode[4].InnerText = "US";

                    xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerText = ClinicalSummary.facilityLibraryCustodian[i].Fac_Name;
                    if (ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone.Trim() != "")
                        xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone;
                    else
                        xmlDoc.GetElementsByTagName("custodian")[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].Attributes[0].Value = "tel:+1" + "(111) 111-1111";


                }
            }


            #endregion

            xmlReqNode = xmlDoc.GetElementsByTagName("id");
            xmlReqNode[1].Attributes[0].Value = ClinicalSummary.Encounter[0].Id.ToString();//doubt

            #region documentationOf
            if (ClinicalSummary.PhysicianLibraryDocumentation != null && ClinicalSummary.PhysicianLibraryDocumentation.Count != 0)
            {
                for (int i = 0; i < ClinicalSummary.PhysicianLibraryDocumentation.Count; i++)
                {
                    //XmlElement TempElement = null;
                    //XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("component")[0];
                    //XmlDocument docAllergyEntry = new XmlDocument();
                    //docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CareTeam.xml"));


                    //XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                    //xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                    ////xmlDoc.DocumentElement.LastChild.ChildNodes[0].ChildNodes[27].AppendChild(xfragAllergy);
                    //elemOldentry.PreviousSibling.AppendChild(xfragAllergy);
                    //xmlDoc.Save(sPrintFileName);
                    //xmlDoc.Load(sPrintFileName);



                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("custodian")[0];
                    //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();



                    xmlReqNode = xmlDoc.GetElementsByTagName("low");
                    if (ClinicalSummary.PhysicianLibraryDocumentation[i].Encounter_Date_of_Service.ToString() != "" || ClinicalSummary.PhysicianLibraryDocumentation[i].Encounter_Date_of_Service.ToString("yyyyMMdd") != "00010101")
                    {
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = ClinicalSummary.PhysicianLibraryDocumentation[i].Encounter_Date_of_Service.ToString("yyyyMMdd");
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].Attributes[0].Value = ClinicalSummary.PhysicianLibraryDocumentation[i].Encounter_Date_of_Service.ToString("yyyyMMdd");
                    }
                    else if (ClinicalSummary.Encounter.Count > 0)
                    {
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                    }
                    else
                    {
                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = "00010101";

                        xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].Attributes[0].Value = "00010101";
                    }

                    //  xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].Attributes[0].Value = "00010101";
                    //  }


                    //commented Start
                    //xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyPrefix;

                    //xmlReqNode = xmlDoc.GetElementsByTagName("given");
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyFirstName;
                    //xmlReqNode = xmlDoc.GetElementsByTagName("family");
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyLastName;

                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = Phy.PhyAddress1;
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = Phy.PhyCity;
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = Phy.PhyState;
                    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = Phy.PhyZip;

                    //if (sphyTelephoneno != string.Empty)
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + sphyTelephoneno;
                    //else
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + "(909) 621-4949";

                    //if (ClinicalSummary.PhysicianLibraryDocumentation.Count == 1)
                    //{
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
                    //    string sMedicalAssistant = string.Empty;
                    //    if (ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant == string.Empty)
                    //        sMedicalAssistant = "_";
                    //    else
                    //        sMedicalAssistant = ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = sMedicalAssistant;
                    //}
                    //else
                    //{
                    //    i += 1;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
                    //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyPrefix + " " + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyFirstName + " " + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyLastName;
                    //}


                    //commented end

                    //i += 1;


                    xmlReqNode = xmlDoc.GetElementsByTagName("performer");

                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[2].ChildNodes[4].InnerText = "US";
                    if (ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone.Trim() != "")
                        xmlReqNode[0].ChildNodes[1].ChildNodes[3].Attributes[1].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
                    else
                        xmlReqNode[0].ChildNodes[1].ChildNodes[3].Attributes[1].Value = "tel:+1" + "(111) 111-1111";

                    xmlReqNode[0].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyPrefix;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyFirstName;
                    xmlReqNode[0].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyLastName;





                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_Address1;
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_City;
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_State;
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.facilityLibraryCustodian[0].Fac_Zip;
                    xmlReqNode[1].ChildNodes[1].ChildNodes[2].ChildNodes[4].InnerText = "US";
                    if (ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone.Trim() != "")
                        xmlReqNode[1].ChildNodes[1].ChildNodes[3].Attributes[1].Value = "tel:+1" + ClinicalSummary.facilityLibraryCustodian[0].Fac_Telephone;
                    else
                        xmlReqNode[1].ChildNodes[1].ChildNodes[3].Attributes[1].Value = "tel:+1" + "(111) 111-1111";

                    if (ClinicalSummary.Encounter != null && ClinicalSummary.Encounter.Count > 0)
                    {

                        UserManager obj = new UserManager();
                        IList<User> lst = new List<User>();
                        lst = obj.GetMedicalAssistantUserList();
                        lst = (from m in lst where m.user_name == ClinicalSummary.Encounter[0].Assigned_Med_Asst_User_Name select m).ToList<User>();
                        if (lst.Count > 0)
                        {
                            string[] name = lst[0].person_name.Split(' ');
                            xmlReqNode[1].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = name[0];
                            if (name.Count() > 1)
                                xmlReqNode[1].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[1].InnerText = name[1];
                        }
                    }

                    break;
                    // xfrag.InnerXml = sInnerXML;
                    //elemOld.LastChild.AppendChild(xfrag);
                    //elemOld.InsertBefore(xfrag);
                }

            }
            else
            {
                //Documentation Low value 
                if (ClinicalSummary.Encounter.Count > 0)
                {
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                }
                else
                    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].Attributes[0].Value = "00010101";

            }
            //xmlReqNode = xmlDoc.GetElementsByTagName("low");
            //xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");//doubt



            //xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = Phy.PhyPrefix;
            //xmlReqNode[0].InnerText = Phy.PhyPrefix;

            //xmlReqNode = xmlDoc.GetElementsByTagName("given");
            //xmlReqNode[2].InnerText = Phy.PhyFirstName;
            //xmlReqNode = xmlDoc.GetElementsByTagName("family");
            //xmlReqNode[2].InnerText = Phy.PhyLastName;

            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = Phy.PhyAddress1;
            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = Phy.PhyCity;
            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = Phy.PhyState;
            //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = Phy.PhyZip;

            //if (sphyTelephoneno != string.Empty)
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + sphyTelephoneno;
            //else
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + "(909) 621-4949";

            ////PhyAssistant
            //for (int i = 0; i < ClinicalSummary.PhysicianLibraryDocumentation.Count; i++)
            //{
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyAddress1;
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyCity;
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyState;
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[3].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].PhyZip;
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[3].Attributes[0].Value = "tel:+1" + ClinicalSummary.PhysicianLibraryDocumentation[i].PhyTelephone;
            //    //name
            //    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = "Dr.";
            //    //if (ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant != string.Empty)
            //    //{
            //    //string[] sMedAssistant = ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant.Split(' ');
            //    xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText = ClinicalSummary.PhysicianLibraryDocumentation[i].Med_Phy_Assistant;
            //    //xmlDoc.GetElementsByTagName("documentationOf")[0].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[2].InnerText = sMedAssistant[1].ToString();
            //    // }
            //}

            #endregion

            //XmlElement Parent = null;
            //XmlElement Child = null;
            //XmlElement MyParent = null;
            //XmlElement MyTempParent = null;
            //int iTableCount = 0;

            #region Allergies
            //StringBuilder sb = new StringBuilder();
            //string sLogPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"], "log.txt");
            if (ClinicalSummary.Allergy != null && ClinicalSummary.Allergy.Count != 0 && hashCheckedList["chkAllergies"].ToString().ToUpper() == "TRUE")
            {
                //sb.Append("Log 1: " + ClinicalSummary.Allergy.Count() + Environment.NewLine);
                //File.AppendAllText(sLogPath, sb.ToString());

                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[3].InnerText == "Allergies")
                {
                    //sb.Append("Log 2: " + "Allergies" + Environment.NewLine);
                    //File.AppendAllText(sLogPath, sb.ToString());

                    //XmlElement TempElement = null;

                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[4];

                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    if (hashCheckedList["chkAllergies"].ToString().ToUpper() == "TRUE")
                    {
                        //sb.Append("Log 3: " + "chkAllergies true" + Environment.NewLine);
                        //File.AppendAllText(sLogPath, sb.ToString());
                        //XmlElement TempElement = null;

                        //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[3];

                        //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                        //string sInnerXML = string.Empty;
                        string response = "0";

                        for (int i = 0; i < ClinicalSummary.Allergy.Count; i++)
                        {
                            string sUri = System.Configuration.ConfigurationManager.AppSettings["RXNormId"];
                            sUri = sUri + "?name=" + ClinicalSummary.Allergy[i].Allergy_Name;

                            //sb.Append("Log 4: " + sUri + Environment.NewLine);
                            //File.AppendAllText(sLogPath, sb.ToString());
                            try
                            {
                                //sb.Append("Log 5: " + "within try " + Environment.NewLine);
                                //File.AppendAllText(sLogPath, sb.ToString());


                                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;

                                //sb.Append("Log 6: " + _Tls12 + Environment.NewLine);
                                //File.AppendAllText(sLogPath, sb.ToString());

                                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

                                //sb.Append("Log 7: " + Tls12 + Environment.NewLine);
                                //File.AppendAllText(sLogPath, sb.ToString());

                                ServicePointManager.SecurityProtocol = Tls12;

                                //sb.Append("Log 8: " + ServicePointManager.SecurityProtocol + Environment.NewLine);
                                //File.AppendAllText(sLogPath, sb.ToString());

                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUri);

                                //sb.Append("Log 9: " + request + Environment.NewLine);
                                //File.AppendAllText(sLogPath, sb.ToString());

                                using (WebResponse Serviceres = request.GetResponse())
                                {
                                    //sb.Append("Log 10: " + request.GetResponse() + Environment.NewLine + Serviceres + Environment.NewLine);
                                    //File.AppendAllText(sLogPath, sb.ToString());
                                    using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                                    {
                                        //sb.Append("Log 11: " + rd + Environment.NewLine);
                                        //File.AppendAllText(sLogPath, sb.ToString());
                                        response = rd.ReadToEnd();
                                        //sb.Append("Log 12: " + response + Environment.NewLine);
                                        //File.AppendAllText(sLogPath, sb.ToString());
                                        int iStart = response.IndexOf("[");
                                        //sb.Append("Log 13: " + iStart + Environment.NewLine);
                                        //File.AppendAllText(sLogPath, sb.ToString());
                                        int iEnd = response.IndexOf("]");
                                        //sb.Append("Log 14: " + iEnd + Environment.NewLine);
                                        //File.AppendAllText(sLogPath, sb.ToString());

                                        if (iStart != -1 && iEnd != -1)
                                        {
                                            //sb.Append("Log 15: " + "Within if" + Environment.NewLine);
                                            //File.AppendAllText(sLogPath, sb.ToString());
                                            response = response.Substring(iStart + 2, (iEnd - 1) - (iStart + 2));
                                        }
                                        else
                                        {
                                            if (ClinicalSummary.Allergy[i].Allergy_Name.Contains("(") == true)
                                            {
                                                sUri = System.Configuration.ConfigurationManager.AppSettings["RXNormId"];
                                                sUri = sUri + "?name=" + ClinicalSummary.Allergy[i].Allergy_Name.Substring(0, ClinicalSummary.Allergy[i].Allergy_Name.IndexOf('('));

                                                request = (HttpWebRequest)WebRequest.Create(sUri);
                                                using (WebResponse Serviceres1 = request.GetResponse())
                                                {
                                                    using (StreamReader rd1 = new StreamReader(Serviceres1.GetResponseStream()))
                                                    {
                                                        response = rd1.ReadToEnd();
                                                        iStart = response.IndexOf("[");
                                                        iEnd = response.IndexOf("]");

                                                        if (iStart != -1 && iEnd != -1)
                                                        {
                                                            response = response.Substring(iStart + 2, (iEnd - 1) - (iStart + 2));
                                                        }
                                                        else
                                                        {
                                                            response = "0";
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //sb.Append("Log 16: " + "Within else" + Environment.NewLine);
                                                //File.AppendAllText(sLogPath, sb.ToString());
                                                response = "0";
                                            }
                                        }

                                        //sb.Append("Log 17: " + "end" + Environment.NewLine);
                                        //File.AppendAllText(sLogPath, sb.ToString());
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                //sb.Append("Log 18: " + "Catch " + Environment.NewLine
                                //    + e.InnerException + Environment.NewLine
                                //    + e.Message + Environment.NewLine
                                //    + e.StackTrace + Environment.NewLine
                                //     + e.TargetSite + Environment.NewLine
                                //      + e.Source + Environment.NewLine
                                //       + e.Data + Environment.NewLine);
                                //File.AppendAllText(sLogPath, sb.ToString());
                                response = "0";
                            }
                            //sb.Append("Log 19: " + "end formation" + Environment.NewLine);
                            //File.AppendAllText(sLogPath, sb.ToString());
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";


                                //sb.Append("Log 20: " + sInnerXML + Environment.NewLine);
                                //File.AppendAllText(sLogPath, sb.ToString());
                            }
                            string sSeverity = string.Empty;
                            string sReaction = string.Empty;
                            if (ClinicalSummary.Allergy[i].Reaction != string.Empty)
                            {
                                //if (ClinicalSummary.Allergy[i].Reaction.Contains(':'))
                                //{
                                //    sReaction = ClinicalSummary.Allergy[i].Reaction.Split(':')[0].ToString();
                                //    sSeverity = ClinicalSummary.Allergy[i].Reaction.Split(':')[1].ToString();
                                //}
                                //else
                                //{
                                    sReaction = ClinicalSummary.Allergy[i].Reaction.ToString();
                                //}
                            }
                            //sInnerXML += "<tr><td>" + ClinicalSummary.Allergy[i].Allergy_Name + ", " + response + "</td><td>" + ClinicalSummary.Allergy[i].Reaction + "</td><td></td><td>Active</td></tr>";

                            if (ClinicalSummary.Allergy[i].Allergy_Name.ToString().ToUpper() == "NKDA")
                            {
                                sInnerXML += "<tr><td>" + ClinicalSummary.Allergy[i].Allergy_Name + "</td><td></td><td></td><td></td></tr>";
                            }
                            else
                            {
                                string sReactionCode = string.Empty;
                                if (ClinicalSummary.Allergy[i].First_DataBank_Med_ID != string.Empty)
                                    sReactionCode = ", SNOMED-CT: " + ClinicalSummary.Allergy[i].First_DataBank_Med_ID;

                                if (response == "0")
                                {
                                    sInnerXML += "<tr><td>" + ClinicalSummary.Allergy[i].Allergy_Name + "</td><td>" + sReaction + sReactionCode + "</td><td>" + sSeverity + "</td><td>Active</td></tr>";
                                }
                                else
                                {
                                    sInnerXML += "<tr><td>" + ClinicalSummary.Allergy[i].Allergy_Name + ", RxNorm: " + response + "(IN)</td><td>" + sReaction + sReactionCode + "</td><td>" + sSeverity + "</td><td>Active</td></tr>";
                                }
                            }
                            if (i == ClinicalSummary.Allergy.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                            //sb.Append("Log 21: " + sInnerXML + Environment.NewLine);
                            //File.AppendAllText(sLogPath, sb.ToString());
                        }
                    }
                    else
                    {

                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                        //xfrag.InnerXml = sInnerXML;
                        //elemOld.LastChild.AppendChild(xfrag);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }

                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1];

                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Allergy_Entry.xml"));

                    int iOldValue = 0;



                    for (int i = 0; i < ClinicalSummary.Allergy.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldEntry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);
                        elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[4].ChildNodes[0];
                        elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1];

                        XmlElement elemReaction = null;
                        XmlElement elemSeverity = null;

                        elemReaction = (XmlElement)xmlDoc.GetElementsByTagName("participant")[i].ChildNodes[0].ChildNodes[0].ChildNodes[0];

                        if (i == 0)
                        {
                            elemSeverity = (XmlElement)xmlDoc.GetElementsByTagName("entryRelationship")[1].ChildNodes[0].ChildNodes[6];
                            iOldValue = 1;
                        }
                        else
                        {
                            iOldValue = iOldValue + 4;
                            elemSeverity = (XmlElement)xmlDoc.GetElementsByTagName("entryRelationship")[iOldValue].ChildNodes[0].ChildNodes[6];

                        }

                        string sUri = System.Configuration.ConfigurationManager.AppSettings["RXNormId"];
                        sUri = sUri + "?name=" + ClinicalSummary.Allergy[i].Allergy_Name;
                        string response = "0";
                        try
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUri);

                            using (WebResponse Serviceres = request.GetResponse())
                            {
                                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                                {
                                    response = rd.ReadToEnd();
                                    int iStart = response.IndexOf("[");
                                    int iEnd = response.IndexOf("]");
                                    if (iStart != -1 && iEnd != -1)
                                        response = response.Substring(iStart + 2, (iEnd - 1) - (iStart + 2));
                                    else
                                    {
                                        if (ClinicalSummary.Allergy[i].Allergy_Name.Contains("(") == true)
                                        {
                                            sUri = System.Configuration.ConfigurationManager.AppSettings["RXNormId"];
                                            sUri = sUri + "?name=" + ClinicalSummary.Allergy[i].Allergy_Name.Substring(0, ClinicalSummary.Allergy[i].Allergy_Name.IndexOf('(')) ;

                                            request = (HttpWebRequest)WebRequest.Create(sUri);
                                            using (WebResponse Serviceres1 = request.GetResponse())
                                            {
                                                using (StreamReader rd1 = new StreamReader(Serviceres1.GetResponseStream()))
                                                {
                                                    response = rd1.ReadToEnd();
                                                    iStart = response.IndexOf("[");
                                                    iEnd = response.IndexOf("]");

                                                    if (iStart != -1 && iEnd != -1)
                                                    {
                                                        response = response.Substring(iStart + 2, (iEnd - 1) - (iStart + 2));
                                                    }
                                                    else
                                                    {
                                                        response = "0";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            response = "0";
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            response = "0";
                        }

                        //elemReaction.Attributes[0].Value = ClinicalSummary.Allergy[i].First_DataBank_Med_ID;
                        elemReaction.Attributes[0].Value = response; // ClinicalSummary.Allergy[i].Rxnorm_ID.ToString();
                        elemReaction.Attributes[3].Value = ClinicalSummary.Allergy[i].Allergy_Name;
                        // elemSeverity.Attributes[0].Value = ClinicalSummary.Allergy[i].First_DataBank_Med_ID;
                        elemSeverity.Attributes[3].Value = ClinicalSummary.Allergy[i].Reaction;
                    }
                    //elemOld.LastChild.AppendChild(xfrag);
                }
            }
            else if (hashCheckedList["chkAllergies"] != null && hashCheckedList["chkAllergies"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[4];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[4];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0];
                XmlDocument docAllergyEntry = new XmlDocument();
                docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Allergies_Nullflavor.xml"));
                XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                xelemSection.AppendChild(xfragAllergy);
                //XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                //xAttribute.Value = "NI";
                //xelemSection.Attributes.Append(xAttribute);




                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion



            #region Encounter
            if (ClinicalSummary.Encounter != null && ClinicalSummary.Encounter.Count != 0 && hashCheckedList["chkEncounter"].ToString().ToUpper() == "TRUE")
            {
                // if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].InnerText == "Encounters")
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[2].InnerText == "Encounters Diagnosis")
                {

                    //XmlElement TempElement = null;

                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    if (hashCheckedList["chkEncounter"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.Encounter.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }
                            string strValue = (ClinicalSummary.EncounterDiagnosis.Count > 0) ? ClinicalSummary.EncounterDiagnosis[0].Snomed_Code_Description + ", SNOMED-CT: " + ClinicalSummary.EncounterDiagnosis[0].Snomed_Code : "";
                            sInnerXML += "<tr><td>" + strValue + "</td><td>" + Phy.PhyLastName + "," + Phy.PhyFirstName + "</td><td>" + ClinicalSummary.Encounter[i].Facility_Name + "</td><td>" + ClinicalSummary.Encounter[i].Date_of_Service.ToString("MMMM dd,yyyy") + "</td></tr>";
                            if (i == ClinicalSummary.Encounter.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3];
                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Encounter.xml"));

                    //ssa
                    //XmlElement elemOldNew = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3];


                    for (int i = 0; i < ClinicalSummary.Encounter.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldEntry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        //elemOldNew = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[i];

                        //elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].childnotes[0];
                        elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3];
                        XmlElement elemVisitType = null;
                        XmlElement elemDOS = null;
                        XmlElement elemFacilityName = null;
                        XmlElement elemAddress = null;
                        XmlElement elemCity = null;
                        XmlElement elemState = null;
                        XmlElement elemPostalCode = null;
                        XmlElement elemPhone = null;

                        XmlElement elemName = null;
                        XmlElement elemReasonDOS = null;
                       // XmlElement elemReasonTypeVisit = null;
                        XmlElement elemReasonValue = null;

                        //elemVisitType=(XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0+i].ChildNodes[2];
                        //elemDOS = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0 + i].ChildNodes[4];

                        //elemFacilityName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0 + i].ChildNodes[7].ChildNodes[0].ChildNodes[1];
                        //elemAddress = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0 + i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[0];
                        //elemCity = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0 + i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[1];
                        //elemState = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0 + i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[2];
                        //elemPostalCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0 + i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[3];
                        //elemName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0 + i].ChildNodes[7].ChildNodes[0].ChildNodes[4].ChildNodes[0];

                        //elemReasonDOS = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0 + i].ChildNodes[9].ChildNodes[0].ChildNodes[4].ChildNodes[0];
                        //elemReasonTypeVisit = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[4].ChildNodes[0 + i].ChildNodes[9].ChildNodes[0].ChildNodes[5];



                        elemVisitType = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[2];
                        elemDOS = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[4];

                        elemFacilityName = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[1];
                        elemAddress = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[0];
                        elemCity = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[1];
                        elemState = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[2];
                        elemPostalCode = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[2].ChildNodes[3];
                        elemPhone = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[3];

                        elemName = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[7].ChildNodes[0].ChildNodes[4].ChildNodes[0];

                        elemReasonDOS = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[9].ChildNodes[0].ChildNodes[4].ChildNodes[0];
                        //elemReasonTypeVisit = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[9].ChildNodes[0].ChildNodes[5];
                        elemReasonValue = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[9].ChildNodes[0].ChildNodes[5];

                        elemVisitType.Attributes[3].Value = "InPatient Admission";//ClinicalSummary.Encounter[i].Visit_Type;
                        elemDOS.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");

                        elemFacilityName.Attributes[3].Value = ClinicalSummary.Encounter[i].Facility_Name;
                        FacilityManager objFacilityMngr = new FacilityManager();
                        IList<FacilityLibrary> ilstFacility = new List<FacilityLibrary>();
                        IList<Assessment> assEncounterDiagnosis = new List<Assessment>();
                        ilstFacility = objFacilityMngr.GetFacilityByFacilityname(ClinicalSummary.Encounter[i].Facility_Name.ToString());
                        if (ClinicalSummary.EncounterDiagnosis != null && ClinicalSummary.EncounterDiagnosis.Count > 0)
                            assEncounterDiagnosis = ClinicalSummary.EncounterDiagnosis.Where(a => a.Encounter_ID == ClinicalSummary.Encounter[i].Id).ToList<Assessment>();
                        if (assEncounterDiagnosis.Count > 0)
                        {
                            elemReasonValue.Attributes[1].Value = assEncounterDiagnosis[0].Snomed_Code;
                            elemReasonValue.Attributes[2].Value = assEncounterDiagnosis[0].Snomed_Code_Description;
                        }
                        if (ilstFacility != null && ilstFacility.Count > 0)
                        {
                            elemAddress.InnerText = ilstFacility[0].Fac_Address1;

                            elemCity.InnerText = ilstFacility[0].Fac_City;
                            elemState.InnerText = ilstFacility[0].Fac_State;

                            elemPostalCode.InnerText = ilstFacility[0].Fac_Zip;
                            elemName.InnerText = ilstFacility[0].Fac_Name;

                            elemPhone.Attributes[0].Value = "tel:+1" + ilstFacility[0].Fac_Telephone;
                        }
                        elemReasonDOS.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
                        //elemReasonTypeVisit.Attributes[2].Value = ClinicalSummary.Encounter[i].Purpose_of_Visit;

                        //XmlElement elemDiagnosisActLow = null;
                        //XmlElement elemDiagnosisobsLow = null;
                        //elemDiagnosisActLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[11].ChildNodes[0].ChildNodes[4].ChildNodes[0];
                        //elemDiagnosisobsLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[11].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[4].ChildNodes[0];
                        //elemDiagnosisActLow.Attributes[0].Value=ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
                        //elemDiagnosisobsLow.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd"); 



                        int k = 0;
                        for (int j = 0; j < ClinicalSummary.EncounterDiagnosis.Count; j++)
                        {
                            XmlElement elemOldEntry1 = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i];
                            XmlDocument docAllergyEntry1 = new XmlDocument();

                            docAllergyEntry1.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\EncounterDiagnosis.xml"));
                            XmlDocumentFragment xfragAllergy1 = xmlDoc.CreateDocumentFragment();
                            xfragAllergy1.InnerXml = docAllergyEntry1.DocumentElement.InnerXml;
                            elemOldEntry1.AppendChild(xfragAllergy1);
                            xmlDoc.Save(sPrintFileName);
                            xmlDoc.Load(sPrintFileName);

                            XmlElement elemDiagnosisActLow = null;
                            XmlElement elemDiagnosisobsLow = null;
                            XmlElement elemDiagnosisValue = null;
                            if (j == 0)
                            {
                                elemDiagnosisActLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[11].ChildNodes[0].ChildNodes[4].ChildNodes[0];
                                elemDiagnosisobsLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[11].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[5].ChildNodes[0];
                                elemDiagnosisActLow.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
                                elemDiagnosisobsLow.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
                                elemDiagnosisValue = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[11].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[6];
                                elemDiagnosisValue.Attributes[1].Value = ClinicalSummary.EncounterDiagnosis[j].Snomed_Code;
                                elemDiagnosisValue.Attributes[3].Value = ClinicalSummary.EncounterDiagnosis[j].Snomed_Code_Description;
                                k = 11;
                            }
                            else
                            {
                                elemDiagnosisActLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[k + 2].ChildNodes[0].ChildNodes[4].ChildNodes[0];
                                elemDiagnosisobsLow = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[k + 2].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[5].ChildNodes[0];
                                elemDiagnosisActLow.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
                                elemDiagnosisobsLow.Attributes[0].Value = ClinicalSummary.Encounter[i].Date_of_Service.ToString("yyyyMMdd");
                                elemDiagnosisValue = (XmlElement)xmlDoc.GetElementsByTagName("encounter")[i].ChildNodes[k + 2].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[6];
                                elemDiagnosisValue.Attributes[1].Value = ClinicalSummary.EncounterDiagnosis[j].Snomed_Code;
                                elemDiagnosisValue.Attributes[3].Value = ClinicalSummary.EncounterDiagnosis[j].Snomed_Code_Description;
                                k += 2;
                            }

                        }


                    }
                    //elemOld.LastChild.AppendChild(xfrag);
                }
            }
            else if (hashCheckedList["chkEncounter"] != null && hashCheckedList["chkEncounter"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[3];

                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[3].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            #endregion

            #region Immunization


            if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count != 0 && hashCheckedList["chkImmunization"] != null && hashCheckedList["chkImmunization"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "IMMUNIZATIONS")
                {

                    //XmlElement TempElement = null;

                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    if (hashCheckedList["chkImmunization"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
                        {
                            //For Bug Id: 28529
                            //string sdate = ClinicalSummary.ImmunizationList[i].Vis_Given_Date.ToString("yyyy-MM-dd");
                            string sdate = "";
                            DateTime dtAdministrativedate = new DateTime();
                            if (ClinicalSummary.immunhistoryList[i].Administered_Date != "")
                            {
                                string date_val = string.Empty;
                                string dtAdministrd = ClinicalSummary.immunhistoryList[i].Administered_Date.ToString();
                                switch (dtAdministrd.Split('-').Length)
                                {
                                    case 1: date_val = "01-Jan-" + dtAdministrd; break;
                                    case 2: date_val = "01-" + dtAdministrd; break;
                                    case 3: date_val = dtAdministrd; break;
                                }
                                sdate = Convert.ToDateTime(date_val).ToString("yyyy-MM-dd");

                                dtAdministrativedate = Convert.ToDateTime(sdate);
                            }
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }
                            if (sdate != "")
                            {
                                if (ClinicalSummary.immunhistoryList[i].Immunization_Source != "" && ClinicalSummary.immunhistoryList[i].Immunization_Source == "Refused Administration")
                                {
                                    sInnerXML += "<tr><td>" + ClinicalSummary.immunhistoryList[i].Immunization_Description + " CVX:" + ClinicalSummary.immunhistoryList[i].CVX_Code + "</td><td>" + dtAdministrativedate.ToString("MMMM dd,yyyy") + "</td><td>Cancelled</td><td>" + ClinicalSummary.immunhistoryList[i].Notes + "</td></tr>";
                                }
                                else
                                {
                                    sInnerXML += "<tr><td>" + ClinicalSummary.immunhistoryList[i].Immunization_Description + " CVX:" + ClinicalSummary.immunhistoryList[i].CVX_Code + "</td><td>" + dtAdministrativedate.ToString("MMMM dd,yyyy") + "</td><td>Completed</td><td></td></tr>";
                                }
                            }
                            else
                            {
                                if (ClinicalSummary.immunhistoryList[i].Immunization_Source != "" && ClinicalSummary.immunhistoryList[i].Immunization_Source == "Refused Administration")
                                {
                                    sInnerXML += "<tr><td>" + ClinicalSummary.immunhistoryList[i].Immunization_Description + " CVX:" + ClinicalSummary.immunhistoryList[i].CVX_Code + "</td><td></td><td>Cancelled</td><td>" + ClinicalSummary.immunhistoryList[i].Notes + "</td></tr>";
                                }
                                else
                                {
                                    sInnerXML += "<tr><td>" + ClinicalSummary.immunhistoryList[i].Immunization_Description + " CVX:" + ClinicalSummary.immunhistoryList[i].CVX_Code + "</td><td></td><td>Completed</td><td></td></tr>";
                                }

                            }
                            if (i == ClinicalSummary.immunhistoryList.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        //XmlElement TempElement = null;
                        //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[3];
                        //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                        //string sInnerXML = string.Empty;
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                        //xfrag.InnerXml = sInnerXML;
                        //elemOld.LastChild.AppendChild(xfrag);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }

                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);


                    XmlElement elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5];

                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Immunization.xml"));
                    //int iOldValue = 0;

                    for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldEntry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[3].ChildNodes[1];

                        elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5];

                        XmlElement elemReference = null;
                        XmlElement elemGivenDate = null;
                        XmlElement elemImmCvxCode = null;
                        XmlElement elemImmLotNumber = null;

                        elemReference = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[3];
                        elemGivenDate = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[5];

                        elemImmCvxCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[6].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                        elemImmLotNumber = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[6].ChildNodes[0].ChildNodes[3].ChildNodes[1];
                        if (ClinicalSummary.immunhistoryList[i].Lot_Number.Trim() != "")
                            elemImmLotNumber.InnerText = ClinicalSummary.immunhistoryList[i].Lot_Number;
                        else
                        {
                            XmlAttribute xmlNull = null;
                            xmlNull = xmlDoc.CreateAttribute("nullFlavor");
                            xmlNull.Value = "UNK";
                            elemImmLotNumber.Attributes.Append(xmlNull);
                        }

                        elemReference.InnerText = ClinicalSummary.immunhistoryList[i].Notes;

                        string sdate = "";
                        DateTime dtAdministrativedate = new DateTime();
                        if (ClinicalSummary.immunhistoryList[i].Administered_Date != "")
                        {
                            if (ClinicalSummary.immunhistoryList[i].Administered_Date != "")
                            {
                                string date_val = string.Empty;
                                string dtAdministrd = ClinicalSummary.immunhistoryList[i].Administered_Date.ToString();
                                switch (dtAdministrd.Split('-').Length)
                                {
                                    case 1: date_val = "01-Jan-" + dtAdministrd; break;
                                    case 2: date_val = "01-" + dtAdministrd; break;
                                    case 3: date_val = dtAdministrd; break;
                                }
                                sdate = Convert.ToDateTime(date_val).ToString("yyyy-MM-dd");

                                dtAdministrativedate = Convert.ToDateTime(sdate);
                            }
                        }
                        if (sdate != "")
                            elemGivenDate.Attributes[0].Value = dtAdministrativedate.ToString("yyyyMMdd");
                        else
                        {
                            elemGivenDate.Attributes[0].Value = "";
                        }
                        elemImmCvxCode.Attributes[0].Value = ClinicalSummary.immunhistoryList[i].CVX_Code;
                        elemImmCvxCode.Attributes[3].Value = ClinicalSummary.immunhistoryList[i].Immunization_Description;
                    }
                    //elemOld.LastChild.AppendChild(xfrag);
                }
            }
            else if (hashCheckedList["chkImmunization"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[5].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion


            #region MedicationAdministrative
            if (ClinicalSummary.MedicationAdministrative != null && ClinicalSummary.MedicationAdministrative.Count != 0 && hashCheckedList["chkMedicationAdministrative"] != null && hashCheckedList["chkMedicationAdministrative"].ToString().ToUpper() == "TRUE")
            {

                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[3].InnerText == "Medications Administered")
                {

                    //XmlElement TempElement = null;


                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    //string sInnerXML = string.Empty;
                    if (hashCheckedList["chkMedicationAdministrative"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.MedicationAdministrative.Count; i++)
                        {
                            //sInnerXML=ClinicalSummary.MedicationAdministrative[i].Generic_Name;
                            XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4];
                            elemOld.InnerText = ClinicalSummary.MedicationAdministrative[i].Generic_Name + " (" + ClinicalSummary.MedicationAdministrative[i].Internal_Property_Code + ")" + ", " + ClinicalSummary.MedicationAdministrative[i].Strength + ", " + ClinicalSummary.MedicationAdministrative[i].Dose + ", " + ClinicalSummary.MedicationAdministrative[i].Dose_Unit + ", " + ClinicalSummary.MedicationAdministrative[i].Dose_Timing;
                        }
                    }
                    else
                    {
                        XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4];
                        elemOld.InnerText = "This Section is Omitted";
                    }
                    //xfrag.InnerXml = sInnerXML;
                    //elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7];


                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\MedicationAdministration.xml"));
                  //  int iOldValue = 0;

                    for (int i = 0; i < ClinicalSummary.MedicationAdministrative.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldEntry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        // elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                        elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7];

                        XmlElement elemMedStartDate = null;
                        XmlElement elemMedStopDate = null;
                        XmlElement elemCode1 = null;
                        XmlElement elemMedDoseQuantity = null;
                        XmlElement elemMedCode = null;

                        XmlElement elemStateDate1 = null;
                        XmlElement elemStopDate1 = null;


                        //xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[3];

                        // elemReference = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[2].ChildNodes[0];

                        elemMedStartDate = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[4].ChildNodes[0];
                        elemMedStopDate = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[4].ChildNodes[1];

                        elemMedDoseQuantity = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5];

                        elemMedCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[2].ChildNodes[0];

                        elemMedStartDate.Attributes[0].Value = ClinicalSummary.MedicationAdministrative[i].Start_Date.ToString("yyyyMMdd");
                        elemMedStopDate.Attributes[0].Value = ClinicalSummary.MedicationAdministrative[i].Stop_Date.ToString("yyyyMMdd");
                        elemMedDoseQuantity.Attributes[0].Value = ClinicalSummary.MedicationAdministrative[i].Strength.Split(' ')[0].ToString();//strength
                        if (ClinicalSummary.MedicationAdministrative[i].Strength.Split(' ').Count() > 1)
                            elemMedDoseQuantity.Attributes[1].Value = ClinicalSummary.MedicationAdministrative[i].Strength.Split(' ')[1].ToString();
                        elemMedCode.Attributes[0].Value = ClinicalSummary.MedicationAdministrative[i].Internal_Property_Code;
                        elemMedCode.Attributes[2].Value = ClinicalSummary.MedicationAdministrative[i].Generic_Name;

                        elemStateDate1 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[7].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                        elemStateDate1.Attributes[0].Value = ClinicalSummary.MedicationAdministrative[i].Start_Date.ToString("yyyyMMdd");
                        elemStopDate1 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[7].ChildNodes[0].ChildNodes[3].ChildNodes[1];
                        elemStopDate1.Attributes[0].Value = ClinicalSummary.MedicationAdministrative[i].Stop_Date.ToString("yyyyMMdd");
                        elemCode1 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[7].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[2].ChildNodes[0];
                        elemCode1.Attributes[0].Value = ClinicalSummary.MedicationAdministrative[i].Internal_Property_Code;
                        elemCode1.Attributes[3].Value = ClinicalSummary.MedicationAdministrative[i].Generic_Name;

                    }
                    //elemOld.LastChild.AppendChild(xfrag);
                }
            }
            else if (hashCheckedList["chkMedicationAdministrative"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[3];
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion


            #region Medication


            if (ClinicalSummary.Medication != null && ClinicalSummary.Medication.Count != 0 && hashCheckedList["chkMedication"] != null && hashCheckedList["chkMedication"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[4].InnerText == "Medications")
                {

                    //XmlElement TempElement = null;

                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    if (hashCheckedList["chkMedication"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.Medication.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }
                            string sIndication = string.Empty;
                            if (ClinicalSummary.Medication[i].Substitution_Permitted.ToUpper() == "Y")
                            {
                                sIndication = ClinicalSummary.Medication[i].Substitution_Permitted.ToString();
                            }
                            else
                            {
                                sIndication = string.Empty;
                            }

                            string response = string.Empty;

                            if (ClinicalSummary.Medication[i].Rxnorm_ID.ToString() == "0")
                            {
                                string sUri = System.Configuration.ConfigurationManager.AppSettings["RXNormId"];
                                sUri = sUri + "?name=" + ClinicalSummary.Medication[i].Generic_Name;

                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUri);
                                using (WebResponse Serviceres1 = request.GetResponse())
                                {
                                    using (StreamReader rd1 = new StreamReader(Serviceres1.GetResponseStream()))
                                    {
                                        int iStart = 0;
                                        int iEnd = 0;

                                        response = rd1.ReadToEnd();
                                        iStart = response.IndexOf("[");
                                        iEnd = response.IndexOf("]");

                                        if (iStart != -1 && iEnd != -1)
                                        {
                                            response = response.Substring(iStart + 2, (iEnd - 1) - (iStart + 2));
                                        }
                                        else
                                        {
                                            response = "0";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                response = ClinicalSummary.Medication[i].Rxnorm_ID.ToString() + " (" + ClinicalSummary.Medication[i].Rxnorm_ID_Type + ")";
                            }

                            // sInnerXML += "<tr><td><content ID='" + "Med" + i.ToString() + "'>" + ClinicalSummary.Medication[i].Generic_Name + " (" + ClinicalSummary.Medication[i].Rxnorm_ID + ")" + "</content></td><td>" + ClinicalSummary.Medication[i].Strength + " " + ClinicalSummary.Medication[i].Action + " " + ClinicalSummary.Medication[i].Dose + " " + ClinicalSummary.Medication[i].Dose_Unit + " " + ClinicalSummary.Medication[i].Dose_Timing + " " + ClinicalSummary.Medication[i].Patient_Notes + "</td><td>" + ClinicalSummary.Medication[i].Start_Date.ToString("yyyyMMdd") + "</td><td>" + ClinicalSummary.Medication[i].Stop_Date.ToString("yyyyMMdd") + "</td><td>" + "Active" + "</td><td>" + sIndication + "</td><td>" + ClinicalSummary.Medication[i].Comments + "</td></tr>";

                            //if ((ClinicalSummary.Medication[i].Stop_Date.ToString("yyyy-MM-dd")) == (Convert.ToDateTime("0001-01-01").ToString("yyyy-MM-dd")))
                            //{
                            //    sInnerXML += "<tr><td><content ID='" + "Med" + i.ToString() + "'>" + ClinicalSummary.Medication[i].Brand_Name + " " + ClinicalSummary.Medication[i].Strength + ", RXNorm: " + ClinicalSummary.Medication[i].Rxnorm_ID + " (" + ClinicalSummary.Medication[i].Rxnorm_ID_Type + ")" + "</content></td><td>" + ClinicalSummary.Medication[i].Action + " " + ClinicalSummary.Medication[i].Dose + " " + ClinicalSummary.Medication[i].Dose_Unit + " " + ClinicalSummary.Medication[i].Dose_Timing + " " + ClinicalSummary.Medication[i].Dose_Other + " " + ClinicalSummary.Medication[i].Patient_Notes + "</td><td>" + ClinicalSummary.Medication[i].Start_Date.ToString("MMMM dd,yyyy") + "</td><td></td><td>Active</td></tr>";
                            //}
                            //else
                            //{
                            if (response == "0")
                                sInnerXML += "<tr><td><content ID='" + "Med" + i.ToString() + "'>" + ClinicalSummary.Medication[i].Generic_Name + " " + ClinicalSummary.Medication[i].Strength + "</content></td><td>" + ClinicalSummary.Medication[i].Action + " " + ClinicalSummary.Medication[i].Dose + " " + ClinicalSummary.Medication[i].Dose_Unit + " " + ClinicalSummary.Medication[i].Dose_Timing + " " + ClinicalSummary.Medication[i].Dose_Other + " " + ClinicalSummary.Medication[i].Patient_Notes + "</td><td>" + ClinicalSummary.Medication[i].Start_Date.ToString("MMMM dd,yyyy") + "</td><td>" + ClinicalSummary.Medication[i].Stop_Date.ToString("MMMM dd,yyyy") + "</td><td>Active</td></tr>";
                            else
                                sInnerXML += "<tr><td><content ID='" + "Med" + i.ToString() + "'>" + ClinicalSummary.Medication[i].Generic_Name + " " + ClinicalSummary.Medication[i].Strength + ", RXNorm: " +response  + "</content></td><td>" + ClinicalSummary.Medication[i].Action + " " + ClinicalSummary.Medication[i].Dose + " " + ClinicalSummary.Medication[i].Dose_Unit + " " + ClinicalSummary.Medication[i].Dose_Timing + " " + ClinicalSummary.Medication[i].Dose_Other + " " + ClinicalSummary.Medication[i].Patient_Notes + "</td><td>" + ClinicalSummary.Medication[i].Start_Date.ToString("MMMM dd,yyyy") + "</td><td>" + ClinicalSummary.Medication[i].Stop_Date.ToString("MMMM dd,yyyy") + "</td><td>Active</td></tr>";
                            //}

                            if (i == ClinicalSummary.Medication.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        //XmlElement TempElement = null;
                        //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[4];
                        //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                        //string sInnerXML = string.Empty;
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                        //xfrag.InnerXml = sInnerXML;
                        //elemOld.LastChild.AppendChild(xfrag);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9];


                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Medication.xml"));
                   // int iOldValue = 0;

                    for (int i = 0; i < ClinicalSummary.Medication.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldEntry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        // elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                        elemOldEntry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9];


                       // XmlElement elemMedExtension = null;
                        XmlElement elemMedStartDate = null;
                        XmlElement elemMedStopDate = null;
                        XmlElement elemMedRoute = null;
                        XmlElement elemMedDoseQuantity = null;
                        XmlElement elemMedCode = null;
                        //XmlElement elemMedID = null;
                       // XmlElement elemReference = null;
                        //XmlElement elemValue = null;

                        //XmlElement elemStateDate1 = null;
                        //XmlElement elemStopDate1 = null;
                        //XmlElement elemRefills = null;
                        //XmlElement elemquantity = null;


                        //xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[3];

                        // elemReference = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[2].ChildNodes[0];

                        // elemMedExtension = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[1];
                        //if (ClinicalSummary.Medication[i].Start_Date != null && ClinicalSummary.Medication[i].Start_Date.ToString() != "0001-01-01 00:00:00")
                        //    elemMedExtension.Attributes[1].Value = ClinicalSummary.Medication[i].Start_Date.ToString("yyyy-MM-dd");
                        //else
                        //    elemMedExtension.RemoveAttribute("extension");
                        elemMedStartDate = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[6 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0];

                        elemMedStopDate = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[6 + i].ChildNodes[0].ChildNodes[5].ChildNodes[1];

                        elemMedRoute = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[6 + i].ChildNodes[0].ChildNodes[6];

                        elemMedDoseQuantity = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[6 + i].ChildNodes[0].ChildNodes[7];

                        elemMedCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[6 + i].ChildNodes[0].ChildNodes[8].ChildNodes[0].ChildNodes[3].ChildNodes[0];

                        // elemMedID = (XmlElement)xmlDoc.GetElementsByTagName("entry")[i].ChildNodes[0].ChildNodes[8].ChildNodes[0].ChildNodes[1];
                        //elemReference.Attributes[0].Value = "#Med" + (1+i).ToString();
                        //elemMedID = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[8].ChildNodes[0].ChildNodes[1];
                        

                        XmlAttribute xAttribute = elemMedStartDate.OwnerDocument.CreateAttribute("nullFlavor");
                        xAttribute.Value = "UNK";
                        if (ClinicalSummary.Medication[i].Start_Date.ToString("yyyyMMdd") == "00010101")
                        {
                            elemMedStartDate.Attributes.RemoveAll();
                            elemMedStartDate.Attributes.Append(xAttribute);
                        }
                        else
                        {
                            elemMedStartDate.Attributes[0].Value = ClinicalSummary.Medication[i].Start_Date.ToString("yyyyMMdd");
                        }

                        if (ClinicalSummary.Medication[i].Stop_Date.ToString("yyyyMMdd") == "00010101")
                        {
                            elemMedStopDate.Attributes.RemoveAll();
                            elemMedStopDate.Attributes.Append(xAttribute);
                        }
                        else
                        {
                            elemMedStopDate.Attributes[0].Value = ClinicalSummary.Medication[i].Stop_Date.ToString("yyyyMMdd");
                        }

                        //elemMedRoute.Attributes[0].Value = ClinicalSummary.Medication[i].NDC_ID;
                        //elemMedRoute.Attributes[3].Value = ClinicalSummary.Medication[i].Generic_Name;
                        // elemMedDoseQuantity.Attributes[0].Value = ClinicalSummary.Medication[i].Strength.Split(' ')[0].ToString();//strength
                        if (ClinicalSummary.Medication[i].Dose.Trim() != "" && ClinicalSummary.Medication[i].Dose.Trim() != string.Empty)
                            elemMedDoseQuantity.Attributes[0].Value = ClinicalSummary.Medication[i].Dose;//strength
                        else
                            elemMedDoseQuantity.Attributes[0].Value = "0";
                        //bool IsUnit = false;
                        //if (ClinicalSummary.Medication[i].Dose_Unit != "")
                        //{
                        //    elemMedDoseQuantity.Attributes[1].Value = ClinicalSummary.Medication[i].Dose_Unit;
                        //    IsUnit = true;
                        //}
                        //else
                        //{
                        //    if (ClinicalSummary.Medication[i].Strength.Split(' ').Count() > 1)
                        //    {
                        //        elemMedDoseQuantity.Attributes[1].Value = ClinicalSummary.Medication[i].Strength.Split(' ')[1].ToString();
                        //        IsUnit = true;
                        //    }
                        //}
                        //if (IsUnit == false)
                        //{
                        //    elemMedDoseQuantity.RemoveAttribute("unit");
                        //}

                        //if (ClinicalSummary.Medication[i].Dose != "")
                        //{
                        //    elemMedDoseQuantity.Attributes[1].Value = ClinicalSummary.Medication[i].Dose;
                        //}
                        //else
                        //{
                        //    elemMedDoseQuantity.RemoveAttribute("unit");
                        //}

                        if (ClinicalSummary.Medication[i].Dose_Unit != "")
                        {
                            elemMedDoseQuantity.Attributes[1].Value = ClinicalSummary.Medication[i].Dose_Unit;
                        }
                        else
                        {
                            elemMedDoseQuantity.RemoveAttribute("unit");
                        }

                        if (ClinicalSummary.Medication[i].Rxnorm_ID.ToString() == "0")
                        {
                            string sUri = System.Configuration.ConfigurationManager.AppSettings["RXNormId"];
                            sUri = sUri + "?name=" + ClinicalSummary.Medication[i].Generic_Name;

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUri);
                            using (WebResponse Serviceres1 = request.GetResponse())
                            {
                                using (StreamReader rd1 = new StreamReader(Serviceres1.GetResponseStream()))
                                {
                                    int iStart = 0;
                                    int iEnd = 0;

                                    string response = rd1.ReadToEnd();
                                    iStart = response.IndexOf("[");
                                    iEnd = response.IndexOf("]");

                                    if (iStart != -1 && iEnd != -1)
                                    {
                                        response = response.Substring(iStart + 2, (iEnd - 1) - (iStart + 2));
                                        elemMedCode.Attributes[0].Value = response;
                                    }
                                    else
                                    {
                                        response = "0";
                                    }
                                }
                            }
                        }
                        else
                        {
                            elemMedCode.Attributes[0].Value = ClinicalSummary.Medication[i].Rxnorm_ID.ToString();
                        }
                        elemMedCode.Attributes[2].Value = ClinicalSummary.Medication[i].Generic_Name;

                        //elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[8].ChildNodes[0].ChildNodes[5];
                        IList<string> strCodeDes = new List<string>();
                        //IList<Assessment> assessmentList = ClinicalSummary.Assessment.Where(a => a.Primary_Diagnosis == "Y").ToList<Assessment>();
                        //if (assessmentList.Count > 0)
                        //{
                        //    elemMedID.Attributes[1].Value = assessmentList[0].ICD_9;//ClinicalSummary.Medication[i].ICD_Code.ToString();
                        //    elemValue.Attributes[1].Value = assessmentList[0].ICD_9;
                        //    elemValue.Attributes[3].Value = assessmentList[0].ICD_Description;
                        //}
                        //else
                        //{
                        //    elemMedID.Attributes[1].Value = "";
                        //    elemValue.Attributes[1].Value = "";
                        //    elemValue.Attributes[3].Value = "";
                        //}
                        ////elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[7].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[8].ChildNodes[0].ChildNodes[5];

                        //elemStateDate1 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[9].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                        //elemStateDate1.Attributes[0].Value = ClinicalSummary.Medication[i].Start_Date.ToString("yyyyMMdd");
                        //elemStopDate1 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[9].ChildNodes[0].ChildNodes[3].ChildNodes[1];
                        //elemStopDate1.Attributes[0].Value = ClinicalSummary.Medication[i].Stop_Date.ToString("yyyyMMdd");
                        //elemRefills = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[9].ChildNodes[0].ChildNodes[4];
                        //string sRefills = ClinicalSummary.Medication[i].Refills;
                        //if (sRefills != string.Empty)
                        //    elemRefills.Attributes[0].Value = sRefills;//notmen
                        //else
                        //    elemRefills.Attributes[0].Value = "2";
                        //elemquantity = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[9].ChildNodes[0].ChildNodes[5];
                        //string sQuantity = ClinicalSummary.Medication[i].Quantity;
                        //if (sQuantity != string.Empty)
                        //    elemquantity.Attributes[0].Value = sQuantity;//notmen
                        //else
                        //    elemquantity.Attributes[0].Value = "4";
                        //XmlElement elemcodeMaterial = null;
                        //elemcodeMaterial = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[9].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[2].ChildNodes[0];
                        //elemcodeMaterial.Attributes[0].Value = ClinicalSummary.Medication[i].Rxnorm_ID.ToString();
                        //elemcodeMaterial.Attributes[3].Value = ClinicalSummary.Medication[i].Generic_Name;
                    }
                    //elemOld.LastChild.AppendChild(xfrag);
                }
            }
            else if (hashCheckedList["chkMedication"] != null && hashCheckedList["chkMedication"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[5];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion


            #region Plan of care



            //if (ClinicalSummary.TreatmentPlan.Count != 0 && hashCheckedList["chkCarePlan"].ToString().ToUpper() == "TRUE")
            if (hashCheckedList["chkCarePlan"] != null && hashCheckedList["chkCarePlan"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[11].ChildNodes[0].ChildNodes[2].InnerText == "Plan of Care")
                {

                    //XmlElement TempElement = null;

                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[11].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    //if (ClinicalSummary.TreatmentPlan.Count != 0)
                    //{
                    if (hashCheckedList["chkCarePlan"].ToString().ToUpper() == "TRUE")
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                        for (int i = 0; i < ClinicalSummary.TreatmentPlan.Count; i++)
                        {

                            string sPlan = string.Empty;
                            if (ClinicalSummary.TreatmentPlan[i].Plan_Type.ToUpper() == "PLAN")
                            {
                                sPlan = ClinicalSummary.TreatmentPlan[i].Plan;
                                if (ClinicalSummary.TreatmentPlan[i].Plan.Contains('>'))
                                {
                                    sPlan = ClinicalSummary.TreatmentPlan[i].Plan.Replace(">", "greater then ");
                                }
                                if (ClinicalSummary.TreatmentPlan[i].Plan.Contains('<'))
                                {
                                    sPlan = ClinicalSummary.TreatmentPlan[i].Plan.Replace("<", "less then ");
                                }
                                sInnerXML += "<tr><td>" + sPlan + "</td><td>" + ClinicalSummary.TreatmentPlan[i].Created_Date_And_Time.ToString("MMMM dd,yyyy") + "</td></tr>";
                            }

                        }
                        string sdiagnosisPending = string.Empty;
                        string sFutureTests = string.Empty;
                        if (ClinicalSummary.OrdersTestPending_Future.Count != 0)
                        {
                            //for (int j = 0; j < ClinicalSummary.OrdersTestPending_Future.Count; j++)
                            //{
                            for (int i = 0; i < ClinicalSummary.OrdersTestPending_Future[0].Lists.Count; i++)
                            {
                                DateTime dt = new DateTime();

                                DateTime dt1 = new DateTime();

                                string sdate = ClinicalSummary.OrdersTestPending_Future[0].ilstOrdersSubmitForPartialOrders[i].Test_Date;
                                dt1 = ClinicalSummary.OrdersTestPending_Future[0].ilstOrdersSubmitForPartialOrders[i].Internal_Property_EncounterDate;

                                //string sdate = ClinicalSummary.OrdersTestPending_Future[0].ilstOrdersSubmitForPartialOrders[i].Test_Date;
                                //string SytemDate = ClinicalSummary.OrdersTestPending_Future[0].ilstOrdersSubmitForPartialOrders[i].EncounterDate;

                                //if (sdate != string.Empty && SytemDate != string.Empty)
                                //{
                                //DateTime dt = new DateTime();
                                if (sdate != "")
                                    dt = Convert.ToDateTime(sdate);
                                //DateTime dt1 = new DateTime();
                                //dt1 = Convert.ToDateTime(SytemDate);
                                if (dt <= dt1)
                                {
                                    if (sdiagnosisPending == string.Empty)
                                    {
                                        sdiagnosisPending = "<tr><td>" + "Diagnostic Tests Pending:" + "</td></tr>";
                                        //sInnerXML += sdiagnosisPending + "<tr><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Lab_Procedure_Description + "</td><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Created_Date_And_Time.ToString("yyyyMMdd") + "</td></tr>";
                                        sInnerXML += sdiagnosisPending + "<tr><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Lab_Procedure_Description + "</td><td>" + dt.ToString("MMMM dd,yyyy") + "</td></tr>";
                                    }
                                    else
                                    {
                                        sInnerXML += "<tr><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Lab_Procedure_Description + "</td><td>" + dt.ToString("MMMM dd,yyyy") + "</td></tr>";

                                    }
                                    // sInnerXML += "<tr><td>" + "Diagnostic Tests Pending:" + "</td></tr><tr><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Lab_Procedure_Description + "</td><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Created_Date_And_Time.ToString("yyyyMMdd") + "</td></tr>";

                                }
                                //}
                            }
                            for (int i = 0; i < ClinicalSummary.OrdersTestPending_Future[0].ilstOrdersSubmitForPartialOrders.Count; i++)
                            {
                                DateTime dt = new DateTime();

                                DateTime dt1 = new DateTime();

                                string sdate = ClinicalSummary.OrdersTestPending_Future[0].ilstOrdersSubmitForPartialOrders[i].Test_Date;
                                dt1 = ClinicalSummary.OrdersTestPending_Future[0].ilstOrdersSubmitForPartialOrders[i].Internal_Property_EncounterDate;

                                //string sdate = ClinicalSummary.OrdersTestPending_Future[0].ilstOrdersSubmitForPartialOrders[i].Test_Date;
                                //string SytemDate = ClinicalSummary.OrdersTestPending_Future[0].ilstOrdersSubmitForPartialOrders[i].EncounterDate;

                                //if (sdate != string.Empty && SytemDate != string.Empty)
                                //{
                                // DateTime dt = new DateTime();
                                if (sdate != "")
                                    dt = Convert.ToDateTime(sdate);
                                //DateTime dt1 = new DateTime();
                                //dt1 = Convert.ToDateTime(SytemDate);
                                if (dt >= dt1)
                                {
                                    if (sFutureTests == string.Empty)
                                    {
                                        sFutureTests = "<tr><td>" + "Future Scheduled Tests:" + "</td></tr>";
                                        //sInnerXML += "<tr><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Lab_Procedure_Description + "</td><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Created_Date_And_Time.ToString("yyyyMMdd") + "</td></tr>";
                                        sInnerXML += sFutureTests + "<tr><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Lab_Procedure_Description + "</td><td>" + dt.ToString("MMMM dd,yyyy") + "</td></tr>";
                                    }
                                    else
                                    {
                                        sInnerXML += "<tr><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Lab_Procedure_Description + "</td><td>" + dt.ToString("MMMM dd,yyyy") + "</td></tr>";

                                    }
                                    //sInnerXML += "<tr><td>" + "Future Scheduled Tests:" + "</td></tr><tr><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Lab_Procedure_Description + "</td><td>" + ClinicalSummary.OrdersTestPending_Future[0].Lists[i].Created_Date_And_Time.ToString("yyyyMMdd") + "</td></tr>";

                                }
                                //}
                            }
                        }
                        // }
                        if (ClinicalSummary.ReferralOrder.Count != 0)
                        {
                            for (int i = 0; i < ClinicalSummary.ReferralOrder.Count; i++)
                            {

                                string sTestDate = string.Empty;
                                if (i == 0)
                                {
                                    sInnerXML += "<tr><td>" + "Future Appointments:" + "</td></tr>";


                                    if (ClinicalSummary.Encounter.Count > 0)
                                    {
                                        if (ClinicalSummary.Encounter[0].Return_In_Weeks != 0 || ClinicalSummary.Encounter[0].Return_In_Days != 0 || ClinicalSummary.Encounter[0].Return_In_Months != 0)
                                        {
                                            //IList<PhysicianLibrary> ilstphyLibrary = new List<PhysicianLibrary>();
                                            //PhysicianManager objPhyMngr = new PhysicianManager();
                                            //ilstphyLibrary = objPhyMngr.GetphysiciannameByPhyID(Convert.ToUInt32(ClinicalSummary.Encounter[0].Appointment_Provider_ID));
                                            //if (ilstphyLibrary.Count > 0)
                                            //{
                                            //    string sTestDate = string.Empty;
                                            //    string sphyname = ilstphyLibrary[0].PhyPrefix + " " + ilstphyLibrary[0].PhyFirstName + " " + ilstphyLibrary[0].PhyLastName + " " + ilstphyLibrary[0].PhySuffix;
                                            //    if (ClinicalSummary.Encounter[0].Return_In_Days != 0)
                                            //        sTestDate = ClinicalSummary.Encounter[0].Return_In_Days + "Days ";
                                            //    if (ClinicalSummary.Encounter[0].Return_In_Weeks != 0)
                                            //        sTestDate += ClinicalSummary.Encounter[0].Return_In_Weeks + "Weeks ";
                                            //    if (ClinicalSummary.Encounter[0].Return_In_Months != 0)
                                            //        sTestDate += ClinicalSummary.Encounter[0].Return_In_Months + "Months";
                                            //    sInnerXML += "<tr><td>" + sphyname + "" + " " + ilstphyLibrary[0].PhyAddress1 + " " + ilstphyLibrary[0].PhyCity + " " + ilstphyLibrary[0].PhyState + "on Test Date +" + sTestDate + "</td></tr>";


                                            if (ClinicalSummary.Encounter[0].Return_In_Days != 0)
                                                sTestDate = ClinicalSummary.Encounter[0].Return_In_Days + "Days ";
                                            if (ClinicalSummary.Encounter[0].Return_In_Weeks != 0)
                                                sTestDate += ClinicalSummary.Encounter[0].Return_In_Weeks + "Weeks ";
                                            if (ClinicalSummary.Encounter[0].Return_In_Months != 0)
                                                sTestDate += ClinicalSummary.Encounter[0].Return_In_Months + "Months";
                                            sTestDate = "on Test Date +" + sTestDate;

                                        }
                                        else if (ClinicalSummary.Encounter[0].Due_On.ToString() != "0001-01-01")
                                        {
                                            sTestDate = "on Test Date " + ClinicalSummary.Encounter[0].Due_On.ToString("MMMM dd,yyyy");
                                        }
                                    }
                                }
                                //string SytemDate = ClientSession.LocalDate;
                                //if (SytemDate != string.Empty)
                                //{
                                //<<<<<<< HL7Generator.cs
                                //                                    DateTime dt1 = new DateTime();
                                //                                    //dt1 = Convert.ToDateTime(SytemDate);
                                //                                    dt1 = ClinicalSummary.ReferralOrder[i].Date_of_service;
                                //                                    if (ClinicalSummary.ReferralOrder[i].Valid_Till > dt1)
                                //                                    //sInnerXML += "<tr><td>" + "Future Appointments:" + "</td></tr><tr><td>" + ClinicalSummary.ReferralOrder[i].To_Physician_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Street_Address + " " + ClinicalSummary.ReferralOrder[i].To_Facility_City + " " + ClinicalSummary.ReferralOrder[i].To_Facility_State + "</td></tr>";
                                //                                    {
                                //                                        sTestDate ="on " + ClinicalSummary.ReferralOrder[i].Valid_Till.ToString("yyyyMMdd");
                                //                                        sInnerXML += "<tr><td>" + ClinicalSummary.ReferralOrder[i].To_Physician_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Street_Address + " " + ClinicalSummary.ReferralOrder[i].To_Facility_City + " " + ClinicalSummary.ReferralOrder[i].To_Facility_State + ClinicalSummary.ReferralOrder[i].To_Facility_Zip + ", " +sTestDate + "</td></tr>";
                                //                                    }
                                //||||||| 1.74
                                DateTime dt1 = new DateTime();
                                //dt1 = Convert.ToDateTime(SytemDate);
                                dt1 = ClinicalSummary.ReferralOrder[i].Internal_Property_Date_of_service;
                                if (ClinicalSummary.ReferralOrder[i].Valid_Till > dt1)
                                {
                                    //sInnerXML += "<tr><td>" + "Future Appointments:" + "</td></tr><tr><td>" + ClinicalSummary.ReferralOrder[i].To_Physician_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Street_Address + " " + ClinicalSummary.ReferralOrder[i].To_Facility_City + " " + ClinicalSummary.ReferralOrder[i].To_Facility_State + "</td></tr>";
                                    if (sTestDate != string.Empty)
                                        sInnerXML += "<tr><td>" + ClinicalSummary.ReferralOrder[i].To_Physician_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Street_Address + " " + ClinicalSummary.ReferralOrder[i].To_Facility_City + " " + ClinicalSummary.ReferralOrder[i].To_Facility_State + ClinicalSummary.ReferralOrder[i].To_Facility_Zip + ", " + sTestDate + "</td></tr>";
                                    else
                                        sInnerXML += "<tr><td>" + ClinicalSummary.ReferralOrder[i].To_Physician_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Street_Address + " " + ClinicalSummary.ReferralOrder[i].To_Facility_City + " " + ClinicalSummary.ReferralOrder[i].To_Facility_State + ClinicalSummary.ReferralOrder[i].To_Facility_Zip + "</td><td>" + ClinicalSummary.ReferralOrder[i].Valid_Till.ToString("MMMM dd,yyyy") + "</td></tr>";
                                }
                                //=======
                                //                                    DateTime dt1 = new DateTime();
                                //                                    //dt1 = Convert.ToDateTime(SytemDate);
                                //                                    dt1 = ClinicalSummary.ReferralOrder[i].Date_of_service;
                                //                                    if (ClinicalSummary.ReferralOrder[i].Valid_Till > dt1)
                                //                                    //sInnerXML += "<tr><td>" + "Future Appointments:" + "</td></tr><tr><td>" + ClinicalSummary.ReferralOrder[i].To_Physician_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Street_Address + " " + ClinicalSummary.ReferralOrder[i].To_Facility_City + " " + ClinicalSummary.ReferralOrder[i].To_Facility_State + "</td></tr>";
                                //                                    {
                                //                                        if (sTestDate == string.Empty)
                                //                                        {
                                //                                            sTestDate = "on " + ClinicalSummary.ReferralOrder[i].Valid_Till.ToString("yyyyMMdd");
                                //                                        }
                                //                                        sInnerXML += "<tr><td>" + ClinicalSummary.ReferralOrder[i].To_Physician_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Name + " " + ClinicalSummary.ReferralOrder[i].To_Facility_Street_Address + " " + ClinicalSummary.ReferralOrder[i].To_Facility_City + " " + ClinicalSummary.ReferralOrder[i].To_Facility_State + ClinicalSummary.ReferralOrder[i].To_Facility_Zip + ", " +sTestDate + "</td></tr>";
                                //                                    }
                                //>>>>>>> 1.74.2.10
                                //}
                            }
                        }

                        sInnerXML += "</tbody>";


                    }
                    else
                    {
                        //XmlElement TempElement = null;
                        //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[11].ChildNodes[0].ChildNodes[3];
                        //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                        //string sInnerXML = string.Empty;
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                        //xfrag.InnerXml = sInnerXML;
                        //elemOld.LastChild.AppendChild(xfrag);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }

                    //string sheader = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                    string sheader = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + "></tbody>";
                    //<tbody xmlns="urn:hl7-org:v3"></tbody>
                    if (sInnerXML != sheader)
                    {
                        xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                        elemOld.LastChild.AppendChild(xfrag);
                    }
                    else
                    {
                        XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[11].ChildNodes[0];
                        XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                        xAttribute.Value = "NI";
                        xelemSection.Attributes.Append(xAttribute);
                        XmlElement elemOld1 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[11].ChildNodes[0].ChildNodes[3];
                        elemOld1.RemoveAll();
                        elemOld1.InnerText = "No known data found";
                        //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                        //string sInnerXML = string.Empty;
                        //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                        //xfrag.InnerXml = sInnerXML;
                        //elemOld.LastChild.AppendChild(xfrag);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);
                    }

                    //XmlElement elemcode = null;
                    //elemcode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[9].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[2];
                    //elemcode.Attributes[3].Value = ClinicalSummary.TreatmentPlan[0].Plan;
                    // }
                }
            }
            else if (hashCheckedList["chkCarePlan"] != null && hashCheckedList["chkCarePlan"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[11].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[11].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + "></tbody>"+"<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";

                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[11].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[11].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion

            #region ReasonForReferral
            if (ClinicalSummary.ReferralOrder != null && ClinicalSummary.ReferralOrder.Count != 0 && hashCheckedList["chkReasonforReferral"] != null && hashCheckedList["chkReasonforReferral"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "REASON FOR REFERRAL")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];
                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\ReasonforReferral.xml"));
                    if (hashCheckedList["chkReasonforReferral"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.ReferralOrder.Count; i++)
                        {

                            XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                            xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                            elemOld.AppendChild(xfragAllergy);
                            xmlDoc.Save(sPrintFileName);
                            xmlDoc.Load(sPrintFileName);
                            elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];
                            XmlElement elemRefParagraph = null;
                            elemRefParagraph = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3].ChildNodes[i];
                            elemRefParagraph.InnerText = ClinicalSummary.ReferralOrder[i].Reason_For_Referral;
                        }
                    }
                    else
                    {
                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOld.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);
                        elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];
                        XmlElement elemRefParagraph = null;
                        elemRefParagraph = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                        elemRefParagraph.InnerText = "This Section is Omitted";
                    }

                }
            }
            else if (hashCheckedList["chkReasonforReferral"] != null && hashCheckedList["chkReasonforReferral"].ToString().ToUpper() == "FALSE")
            {
               // XmlElement TempElement = null;

                //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];

                XmlDocument docAllergyEntry = new XmlDocument();

                docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\ReasonforReferral.xml"));

                XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                elemOld.AppendChild(xfragAllergy);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
                elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];
                // elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[1].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                XmlElement elemRefParagraph = null;
                elemRefParagraph = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                elemRefParagraph.InnerText = "This Section is Omitted";

                //XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0];
                //XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                //xAttribute.Value = "NI";
                //xelemSection.Attributes.Append(xAttribute);
                //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = "This Section is Omitted";
                //elemOld.LastChild.AppendChild(xfrag);
                //xmlDoc.Save(sPrintFileName);
                //xmlDoc.Load(sPrintFileName);
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }




            #endregion

            #region ProblemList
            if (ClinicalSummary.ProblemListing != null && ClinicalSummary.ProblemListing.Count != 0 && hashCheckedList["chkProblemList"] != null && hashCheckedList["chkProblemList"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[3].InnerText.ToUpper() == "PROBLEMS")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[4];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    if (hashCheckedList["chkProblemList"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.ProblemListing.Count; i++)
                        {
                            if (ClinicalSummary.ProblemListing[i].Date_Diagnosed != "")
                            {
                                if (ClinicalSummary.ProblemListing[i].Resolved_Date != "")
                                    sInnerXML += "<item xmlns=" + '"' + "urn:hl7-org:v3" + '"' + "><content ID='" + "problem" + i.ToString() + "'>" + ClinicalSummary.ProblemListing[i].Snomed_Code_Description + "," + "SNOMED-CT " + ClinicalSummary.ProblemListing[i].Snomed_Code + ",Status - " + ClinicalSummary.ProblemListing[i].Status + "," + ClinicalSummary.ProblemListing[i].Date_Diagnosed + ", " + ClinicalSummary.ProblemListing[i].Resolved_Date + "</content></item>";
                                else
                                    sInnerXML += "<item xmlns=" + '"' + "urn:hl7-org:v3" + '"' + "><content ID='" + "problem" + i.ToString() + "'>" + ClinicalSummary.ProblemListing[i].Snomed_Code_Description + "," + "SNOMED-CT " + ClinicalSummary.ProblemListing[i].Snomed_Code + ",Status - " + ClinicalSummary.ProblemListing[i].Status + "," + ClinicalSummary.ProblemListing[i].Date_Diagnosed + "</content></item>";
                            }
                            else
                                sInnerXML += "<item xmlns=" + '"' + "urn:hl7-org:v3" + '"' + "><content ID='" + "problem" + i.ToString() + "'>" + ClinicalSummary.ProblemListing[i].Snomed_Code_Description + "," + "SNOMED-CT " + ClinicalSummary.ProblemListing[i].Snomed_Code + ",Status - " + ClinicalSummary.ProblemListing[i].Status + "</content></item>";
                        }
                    }
                    else
                    {
                        sInnerXML = "<item xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "This Section is Omitted" + "</item>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);


                    //XmlElement TempElement = null;


                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15];


                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Problem.xml"));
                   // int iOldValue = 0;

                    for (int i = 0; i < ClinicalSummary.ProblemListing.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldentry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);


                        elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15];


                        //elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];


                        XmlElement elemStatusCode = null;
                        //  XmlElement elemExtension1 = null;
                        XmlElement elemProbDate = null;
                        // XmlElement elemExtension2 = null;
                        XmlElement elemlow = null;
                        XmlElement elemhigh = null;
                        XmlElement elemprobDescription = null;
                        XmlElement elemUnit = null;

                        DateTime datelow = new DateTime();
                        string sdate = ClinicalSummary.ProblemListing[i].Created_Date_And_Time.ToString();
                        //if (sdate == string.Empty)
                        //    sdate = "01-Jan-0001";
                        switch (sdate.Length)
                        {
                            case 0:
                                sdate = "01-Jan-0001";
                                break;
                            case 1:
                                sdate = sdate + "-Jan-0001";
                                break;
                            case 2:
                                sdate = sdate + "-Jan-0001";
                                break;
                            case 3:
                                sdate = "01-" + sdate + "-0001";
                                break;
                            case 4:
                                sdate = "01-Jan-" + sdate;
                                break;
                            case 5:
                                sdate = sdate + "0001";
                                break;
                            case 6:
                                sdate = sdate.Substring(0, 2) + "Jan" + sdate.Substring(2, 4);
                                break;
                            case 7:
                                sdate = "01" + sdate;
                                break;
                        }
                        if (sdate.Contains("00-") == true)
                        {
                            sdate = sdate.Replace("00-", "01-");
                            datelow = Convert.ToDateTime(sdate);
                        }
                        else
                        {
                            datelow = Convert.ToDateTime(sdate);
                        }

                        elemStatusCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[4];
                        elemStatusCode.Attributes[0].Value = ClinicalSummary.ProblemListing[i].Status.ToLower();

                        //elemExtension1=(XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[1];
                        //if (ClinicalSummary.ProblemListing[i].Date_Diagnosed.Trim() == "")
                        //    elemExtension1.RemoveAttribute("extension");
                        //else
                        //    elemExtension1.Attributes[1].Value = datelow.ToString("yyyy-MM-dd");

                        elemProbDate = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[5].ChildNodes[0];
                        elemProbDate.Attributes[0].Value = datelow.ToString("yyyyMMdd");  //ClinicalSummary.ProblemListing[i].Date_Diagnosed;

                        //elemExtension2 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[6].ChildNodes[1].ChildNodes[1];
                        //if (ClinicalSummary.ProblemListing[i].Date_Diagnosed.Trim() == "")
                        //    elemExtension2.RemoveAttribute("extension");
                        //else
                        //    elemExtension2.Attributes[1].Value = datelow.ToString("yyyy-MM-dd");

                        elemlow = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[6].ChildNodes[1].ChildNodes[6].ChildNodes[0];
                        elemlow.Attributes[0].Value = datelow.ToString("yyyyMMdd");

                        elemhigh = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[6].ChildNodes[1].ChildNodes[6].ChildNodes[1];
                        elemhigh.Attributes[0].Value = datelow.ToString("yyyyMMdd");


                        elemprobDescription = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[6].ChildNodes[1].ChildNodes[7];
                        elemprobDescription.Attributes[1].Value = ClinicalSummary.ProblemListing[i].Snomed_Code;
                        elemprobDescription.Attributes[4].Value = ClinicalSummary.ProblemListing[i].Snomed_Code_Description;

                        elemUnit = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[6].ChildNodes[1].ChildNodes[8].ChildNodes[1].ChildNodes[3];
                        //elemUnit.Attributes[1].Value = "65";//CLARIFICATIONS
                        ////  ClinicalSummary.Birth_Date
                        //DateTime startTime = ClinicalSummary.Birth_Date;

                        //DateTime endTime =Convert.ToDateTime(ClientSession.LocalDate);

                        //TimeSpan span = endTime.Subtract(startTime);
                        //elemUnit.Attributes[1].Value = span.TotalDays.ToString();
                        string startTime = ClinicalSummary.Birth_Date.ToString();

                        string endTime = ClientSession.LocalDate;

                        TimeSpan duration = DateTime.ParseExact(endTime, "M'/'d'/'yyyy", null).Subtract(DateTime.Parse(startTime));

                        int iAge = Convert.ToInt32(duration.TotalDays) / 365;
                        if (iAge != null)
                            elemUnit.Attributes[1].Value = iAge.ToString();

                    }

                }
            }
            else if (hashCheckedList["chkProblemList"] != null && hashCheckedList["chkProblemList"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[4];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<item xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "This Section is Omitted</item>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0].ChildNodes[4];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[15].ChildNodes[0];
                XmlDocument docAllergyEntry = new XmlDocument();
                docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\ProblemlistNullflavor.xml"));
                XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                xelemSection.AppendChild(xfragAllergy);



                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }


            #endregion


            #region Procedures

            if (ClinicalSummary.laborderList != null && ClinicalSummary.laborderList.Count != 0 && hashCheckedList["chkProcedures"] != null && hashCheckedList["chkProcedures"].ToString().ToUpper() == "TRUE")
            {
                UtilityManager umanger = new UtilityManager();
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "PROCEDURES")
                {

                    //XmlElement TempElement = null;

                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    if (hashCheckedList["chkProcedures"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.laborderList.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }

                            sInnerXML += "<tr><td>" + ClinicalSummary.laborderList[i].labProcedureDescription.Lab_Procedure_Description + " (" + ClinicalSummary.laborderList[i].labProcedureDescription.Lab_Procedure + ") - SNOMED-CT: " + umanger.GetSnomedForCPT(ClinicalSummary.laborderList[i].labProcedure.Lab_Procedure) + "</td><td>" + ClinicalSummary.laborderList[i].labProcedureDescription.Created_Date_And_Time.ToString("MMMM dd,yyyy") + "</td></tr>";
                            if (i == ClinicalSummary.laborderList.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        //XmlElement TempElement = null;
                        //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[3];
                        //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                        //string sInnerXML = string.Empty;
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                        //xfrag.InnerXml = sInnerXML;
                        //elemOld.LastChild.AppendChild(xfrag);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);



                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17];


                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Procedures.xml"));
                    //int iOldValue = 0;

                    for (int i = 0; i < ClinicalSummary.laborderList.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldentry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);


                        elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17];


                        //elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];


                        XmlElement elemStatusCode = null;
                        XmlElement elemProbDate = null;
                        XmlElement elemstreetAddressLine = null;
                        XmlElement elemcity = null;
                        XmlElement elemstate = null;
                        XmlElement elempostalCode = null;
                        XmlElement elemTelephone = null;
                        XmlElement elemName = null;

                        elemStatusCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[2];
                        if (ClinicalSummary.laborderList[i].labProcedure.Lab_Procedure.Trim() == "")
                            elemStatusCode.Attributes[0].Value = "168731009";
                        else
                            elemStatusCode.Attributes[0].Value = umanger.GetSnomedForCPT(ClinicalSummary.laborderList[i].labProcedure.Lab_Procedure);//Get snomed from static lookup xml
                        elemStatusCode.Attributes[2].Value = ClinicalSummary.laborderList[i].labProcedureDescription.Lab_Procedure_Description;
                        elemProbDate = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[4];
                        elemProbDate.Attributes[0].Value = ClinicalSummary.laborderList[i].labProcedureDescription.Created_Date_And_Time.ToString("yyyyMMdd");
                        elemstreetAddressLine = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[1].ChildNodes[0];

                        PhysicianManager objphyMngr = new PhysicianManager();
                        PhysicianLibrary objphy = new PhysicianLibrary();
                        IList<PhysicianLibrary> ilstPhyList = new List<PhysicianLibrary>();
                        ilstPhyList = objphyMngr.GetphysiciannameByPhyID(ClinicalSummary.laborderList[i].labProcedureDescription.Physician_ID);
                        if (ilstPhyList != null && ilstPhyList.Count > 0)
                        {
                            elemstreetAddressLine.InnerText = ilstPhyList[0].PhyAddress1.ToString();
                            elemcity = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[1].ChildNodes[1];
                            elemcity.InnerText = ilstPhyList[0].PhyCity;
                            elemstate = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[1].ChildNodes[2];
                            elemstate.InnerText = ilstPhyList[0].PhyState;
                            elempostalCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[1].ChildNodes[3];
                            elempostalCode.InnerText = ilstPhyList[0].PhyZip;
                            elemTelephone = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[2];
                            elemTelephone.Attributes[1].Value = ilstPhyList[0].PhyTelephone.ToString();
                            elemName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[3].ChildNodes[1];
                            elemName.InnerText = ilstPhyList[0].PhyFirstName + "" + ilstPhyList[0].PhyMiddleName + "" + ilstPhyList[0].PhyLastName;
                        }
                    }

                }
            }
            else if (hashCheckedList["chkProcedures"] != null && hashCheckedList["chkProcedures"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                //XmlElement elemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].add;

                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }





            #endregion


            #region LaboratoryTests
            if (ClinicalSummary.OrdersList != null && ClinicalSummary.OrdersList.Count != 0 && hashCheckedList["chkLabTest"] != null && hashCheckedList["chkLabTest"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[19].ChildNodes[0].ChildNodes[0].InnerText.ToUpper() == "LABORATORY TESTS")
                {

                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[19].ChildNodes[0].ChildNodes[1];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    if (hashCheckedList["chkLabTest"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.OrdersList.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }

                            sInnerXML += "<tr><td>24357 - 6</td><td>LOINC</td><td>" + ClinicalSummary.OrdersList[i].Lab_Procedure_Description + "</td><td>" + ClinicalSummary.OrdersList[i].Created_Date_And_Time.ToString("MMMM dd,yyyy") + "</td></tr>";
                            if (i == ClinicalSummary.OrdersList.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);
                }
            }
            else if (hashCheckedList["chkLabTest"] != null && hashCheckedList["chkLabTest"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[19].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[19].ChildNodes[0].ChildNodes[1];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[19].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[19].ChildNodes[0].ChildNodes[1];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            #endregion

            #region Laboratory Information
            if (ClinicalSummary.LabLocationList != null && ClinicalSummary.LabLocationList.Count != 0 && hashCheckedList["chkLab"] != null && hashCheckedList["chkLab"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[21].ChildNodes[0].ChildNodes[0].InnerText == "Laboratory Information")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[21].ChildNodes[0].ChildNodes[1];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    if (hashCheckedList["chkLab"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.LabLocationList.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }

                            sInnerXML += "<tr><td>" + ClinicalSummary.LabLocationList[i].Modified_By + "</td><td>" + ClinicalSummary.LabLocationList[0].Location_Name + ", " + ClinicalSummary.LabLocationList[0].Street_Address1 + ", " + ClinicalSummary.LabLocationList[0].City + ", " + ClinicalSummary.LabLocationList[0].State + ", " + ClinicalSummary.LabLocationList[0].ZipCode + ", tel: " + ClinicalSummary.LabLocationList[0].Phone_No + "</td><td>" + ClinicalSummary.LabLocationList[i].Created_Date_And_Time.ToString("MMMM dd,yyyy") + "</td><td>" + ClinicalSummary.LabLocationList[i].E_Mail + "</td><td>" + ClinicalSummary.LabLocationList[i].Suit_Number + ", 122575003(SNOMED-CT) </td><td> - </td></tr>";
                            if (i == ClinicalSummary.LabLocationList.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);
                }
            }
            else if (hashCheckedList["chkLab"] != null && hashCheckedList["chkLab"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[21].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[21].ChildNodes[0].ChildNodes[1];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[21].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[21].ChildNodes[0].ChildNodes[1];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            #endregion

            #region Results
            if (ClinicalSummary.ResultList != null && ClinicalSummary.ResultList.Count != 0 && hashCheckedList["chkLaboratoryResultValues"] != null && hashCheckedList["chkLaboratoryResultValues"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "RESULTS")
                {

                    //XmlElement TempElement = null;

                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    if (hashCheckedList["chkLaboratoryResultValues"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.ResultList.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                                //if (ClinicalSummary.LabLocationList.Count > 0)
                                //{
                                //    //sInnerXML += "<tr><td style='font-weight:bold;'>" + ClinicalSummary.LabLocationList[0].Lab_NPI + "</td><td colspan='4' style='font-weight:bold;'> Laboratory: " + ClinicalSummary.LabLocationList[0].Modified_By + " | " + ClinicalSummary.LabLocationList[0].Location_Name + ", " + ClinicalSummary.LabLocationList[0].Street_Address1 + ", " + ClinicalSummary.LabLocationList[0].City + ", " + ClinicalSummary.LabLocationList[0].State + ", " + ClinicalSummary.LabLocationList[0].ZipCode + ", tel: " + ClinicalSummary.LabLocationList[0].Phone_No + "</td></tr>";
                                //    sInnerXML += "<tr><td>" + ClinicalSummary.LabLocationList[0].E_Mail + "</td><td> Laboratory: " + ClinicalSummary.LabLocationList[0].Modified_By + " " + ClinicalSummary.LabLocationList[0].Lab_NPI + " | " + ClinicalSummary.LabLocationList[0].Location_Name + ", " + ClinicalSummary.LabLocationList[0].Street_Address1 + ", " + ClinicalSummary.LabLocationList[0].City + ", " + ClinicalSummary.LabLocationList[0].State + ", " + ClinicalSummary.LabLocationList[0].ZipCode + ", tel: " + ClinicalSummary.LabLocationList[0].Phone_No + "</td></tr>";
                                //}
                            }
                            if (!ClinicalSummary.ResultList[i].OBX_Observation_Value.ToUpper().Contains("SEE REPORT BELOW: "))
                            {
                                //if (ClinicalSummary.ResultList[i].OBX_Observation_Value.Contains("<"))
                                //    ClinicalSummary.ResultList[i].OBX_Observation_Value = ClinicalSummary.ResultList[i].OBX_Observation_Value.Replace("<", "&lt;");
                                //else if (ClinicalSummary.ResultList[i].OBX_Observation_Value.Contains(">"))
                                   // ClinicalSummary.ResultList[i].OBX_Observation_Value = ClinicalSummary.ResultList[i].OBX_Observation_Value.Replace(">", "&gt;");
                                // sInnerXML += "<tr><td>" + ClinicalSummary.ResultList[i].OBX_Observation_Text + "," + ClinicalSummary.ResultList[i].OBX_Observation_Identifier + " " + ClinicalSummary.ResultList[i].Created_Date_And_Time.ToString("yyyyMMdd") + "</td><td>" + ClinicalSummary.ResultList[i].OBX_Observation_Value + " " + ClinicalSummary.ResultList[i].OBX_Units + "</td></tr>";
                                //ClinicalSummary.ResultList[i].temp_property= value from "Result_Review_Comments" column from result_master table 
                                sInnerXML += "<tr><td>" + ClinicalSummary.ResultList[i].OBX_Observation_Text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;")
                                    + ", LOINC: " + ClinicalSummary.ResultList[i].OBX_Loinc_Identifier.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;")
                                    + "</td><td>" + ClinicalSummary.ResultList[i].OBX_Observation_Value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;")
                                    + " " + ClinicalSummary.ResultList[i].OBX_Units.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;")
                                    + "</td><td>" + ClinicalSummary.ResultList[i].OBX_Reference_Range.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;")
                                    + "</td><td>" + ClinicalSummary.ResultList[i].OBX_Value_Type.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;")
                                    + "</td><td>" + ClinicalSummary.ResultList[i].Created_Date_And_Time.ToString("MMMM dd,yyyy") + "</td></tr>";
                            }
                            if (i == ClinicalSummary.ResultList.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        //XmlElement TempElement = null;
                        //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[19].ChildNodes[0].ChildNodes[3];
                        //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                        //string sInnerXML = string.Empty;
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                        //xfrag.InnerXml = sInnerXML;
                        //elemOld.LastChild.AppendChild(xfrag);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }
                    //xfrag.InnerXml = sInnerXML.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    xfrag.InnerXml = sInnerXML;
                    elemOld.LastChild.AppendChild(xfrag);



                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23];


                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Result.xml"));

                    IList<ResultOBX> ResultList = (from r in ClinicalSummary.ResultList where r.OBX_Value_Type.Trim() == "" select r).ToList<ResultOBX>();
                    for (int i = 0; i < ResultList.Count; i++)
                    {
                        if (ResultList[i].OBX_Value_Type.Trim() == "")
                        {
                            XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                            xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                            elemOldentry.LastChild.AppendChild(xfragAllergy);
                            xmlDoc.Save(sPrintFileName);
                            xmlDoc.Load(sPrintFileName);


                            elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23];


                            //elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];


                            XmlElement elemLonicIdentifierobservation = null;
                           // XmlElement elemreference = null;
                            XmlElement elemResultCaptureDatetime = null;
                            XmlElement elemValue = null;


                            elemLonicIdentifierobservation = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[3];
                            if (ResultList[i].OBX_Loinc_Identifier==string.Empty)
                                elemLonicIdentifierobservation.Attributes[1].Value = "0";
                            else
                                elemLonicIdentifierobservation.Attributes[1].Value = ResultList[i].OBX_Loinc_Identifier.ToString();
                            //elemLonicIdentifierobservation.Attributes[1].Value = ResultList[i].OBX_Observation_Identifier.ToString();
                            elemLonicIdentifierobservation.Attributes[4].Value = ResultList[i].OBX_Observation_Text.ToString();
                            //elemreference = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                            //elemreference.InnerText = "";
                            elemResultCaptureDatetime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[6];
                            //string sResult = "Result";
                            //if (ClinicalSummary.ResultList[i]. == "Result")
                            elemResultCaptureDatetime.Attributes[0].Value = ResultList[i].Created_Date_And_Time.ToString("yyyyMMdd");//PatientResultcaptureddatetime

                            elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[7];


                            if (Regex.IsMatch(ResultList[i].OBX_Observation_Value.Replace(" ", ""), @"^[a-zA-Z]+$"))
                            {
                                elemValue.InnerText = ResultList[i].OBX_Observation_Value.ToString();
                                elemValue.Attributes[0].Value = "ST";
                                elemValue.RemoveAttribute("value");
                                elemValue.RemoveAttribute("unit");
                                elemValue.RemoveAttribute("nullFlavor");
                            }
                            else
                            {
                                elemValue.Attributes[1].Value = ResultList[i].OBX_Observation_Value.ToString();
                                elemValue.RemoveAttribute("nullFlavor");
                            }

                            if (elemValue.Attributes.Count > 2)
                            {
                                if (ResultList[i].OBX_Units.ToString() != "")
                                {
                                    elemValue.Attributes[2].Value = ResultList[i].OBX_Units.ToString();
                                    elemValue.RemoveAttribute("nullFlavor");
                                }
                                else
                                {
                                    elemValue.RemoveAttribute("unit");

                                }
                            }
                        }
                    }
                }
            }
            else if (hashCheckedList["chkLaboratoryResultValues"] != null && hashCheckedList["chkLaboratoryResultValues"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                //XmlElement elemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].add;

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[23].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }



            #endregion



            #region SocialHistory



            if (ClinicalSummary.SocialHistory != null && ClinicalSummary.SocialHistory.Count != 0 && hashCheckedList["chkSmokingStatus"] != null && hashCheckedList["chkSmokingStatus"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[3].InnerText.ToUpper() == "SOCIAL HISTORY")
                {

                    //XmlElement TempElement = null;

                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[4];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;


                    IList<SocialHistory> ilstSocialHistory1 = new List<SocialHistory>();
                    for (int i = 0; i < ClinicalSummary.SocialHistory.Count; i++)
                    {
                        if (ClinicalSummary.SocialHistory[i].Social_Info.ToUpper() == "SMOKING HABIT" || ClinicalSummary.SocialHistory[i].Social_Info.ToUpper() == "TOBACCO USE AND EXPOSURE")
                        {
                            //IList<SocialHistory> ilstSocialHistory = new List<SocialHistory>();
                            //ilstSocialHistory = (from p in ClinicalSummary.SocialHistory where p.Social_Info.ToUpper() == "SMOKING HABIT" select p).ToList<SocialHistory>();
                            //SocialHistory objSocialHistory = new SocialHistory();
                            //objSocialHistory = ilstSocialHistory[0];
                            //ilstSocialHistory1.Add(objSocialHistory);
                            ilstSocialHistory1.Add(ClinicalSummary.SocialHistory[i]);
                        }
                    }
                    //if (hashCheckedList["chkSmokingStatus"].ToString().ToUpper() == "TRUE")
                    if (ilstSocialHistory1.Count > 0)
                    {
                        for (int i = 0; i < ilstSocialHistory1.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }

                            //sInnerXML += "<tr><td>" + ilstSocialHistory1[i].Social_Info.ToString() + "</td><td>" + ilstSocialHistory1[i].Value + "-" + ilstSocialHistory1[i].Description + "</td><td>" + ilstSocialHistory1[i].Recodes + "</td></tr>";
                            sInnerXML += "<tr><td>" + ilstSocialHistory1[i].Social_Info.ToString() + "</td><td>" + ilstSocialHistory1[i].Value + ", SNOMED-CT: " + ilstSocialHistory1[i].Recodes + "</td><td>" + (ilstSocialHistory1[i].Modified_Date_And_Time.ToString("yyyyMMdd") != "00010101" ? ilstSocialHistory1[i].Modified_Date_And_Time.ToString("MMMM dd,yyyy") : ilstSocialHistory1[i].Created_Date_And_Time.ToString("MMMM dd,yyyy")) + "</td></tr>";

                            if (i == ilstSocialHistory1.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        //XmlElement TempElement = null;
                        //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[21].ChildNodes[0].ChildNodes[3];
                        //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                        //string sInnerXML = string.Empty;
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                        //xfrag.InnerXml = sInnerXML;
                        //elemOld.LastChild.AppendChild(xfrag);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }

                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);



                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25];




                    for (int i = 0; i < ilstSocialHistory1.Count; i++)
                    {

                        if (ilstSocialHistory1[i].Social_Info.ToUpper() == "TOBACCO USE AND EXPOSURE")
                        {

                            XmlDocument docAllergyEntry = new XmlDocument();
                            docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\SocialHistoryTobacco.xml"));
                            XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                            xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                            elemOldentry.LastChild.AppendChild(xfragAllergy);
                            xmlDoc.Save(sPrintFileName);
                            xmlDoc.Load(sPrintFileName);


                            elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25];
                            XmlElement eleDisplayName = null;
                            XmlElement eleLowTime = null;
                            XmlElement elemValue = null;

                            eleDisplayName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[3];
                            eleDisplayName.Attributes[3].Value = ilstSocialHistory1[i].Social_Info;

                            eleLowTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0];
                            eleLowTime.Attributes[0].Value = ilstSocialHistory1[i].Modified_Date_And_Time.ToString("yyyyMMdd") != "00010101" ? ilstSocialHistory1[i].Modified_Date_And_Time.ToString("yyyyMMdd") : ilstSocialHistory1[i].Created_Date_And_Time.ToString("yyyyMMdd");

                            elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[6];
                            elemValue.Attributes[1].Value = ilstSocialHistory1[i].Recodes.ToString();
                            elemValue.Attributes[2].Value = ilstSocialHistory1[i].Social_Info;


                        }
                        else if (ilstSocialHistory1[i].Social_Info.ToUpper() == "SMOKING HABIT")
                        {


                            XmlDocument docAllergyEntry = new XmlDocument();
                            docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\SocialHistory.xml"));
                            XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                            xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                            elemOldentry.LastChild.AppendChild(xfragAllergy);
                            xmlDoc.Save(sPrintFileName);
                            xmlDoc.Load(sPrintFileName);


                            elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25];


                            //elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];

                            XmlElement elemValue = null;

                            elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[5];
                            elemValue.Attributes[1].Value = ilstSocialHistory1[i].Recodes.ToString();
                            elemValue.Attributes[3].Value = ilstSocialHistory1[i].Social_Info;

                            elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[5 + i].ChildNodes[1].ChildNodes[4].ChildNodes[0];
                            elemValue.Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                        }
                    }

                }
            }
            else if (hashCheckedList["chkSmokingStatus"] != null && hashCheckedList["chkSmokingStatus"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[4];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                //XmlElement elemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].add;

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[4];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }





            #endregion



            #region Vitalsign

            if (ClinicalSummary.Vitals != null && ClinicalSummary.Vitals.Count != 0 && hashCheckedList["chkVitals"] != null && hashCheckedList["chkVitals"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[3].InnerText.ToUpper() == "VITAL SIGNS")
                {

                    IList<PatientResults> ilstPatientResult1 = new List<PatientResults>();
                    for (int i = 0; i < ClinicalSummary.Vitals.Count; i++)
                    {
                        if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "HEIGHT")
                        {
                            IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                            ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "HEIGHT" select p).ToList<PatientResults>();
                            PatientResults objPatientResult = new PatientResults();
                            ilstPatientResult[0].Units = "cm";
                            objPatientResult = ilstPatientResult[0];
                            ilstPatientResult1.Add(objPatientResult);
                        }
                        if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "WEIGHT")
                        {
                            IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                            ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "WEIGHT" select p).ToList<PatientResults>();
                            PatientResults objPatientResult = new PatientResults();
                            ilstPatientResult[0].Units = "kg";
                            objPatientResult = ilstPatientResult[0];
                            ilstPatientResult1.Add(objPatientResult);
                        }
                        if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "BMI")
                        {
                            IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                            ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "BMI" select p).ToList<PatientResults>();
                            PatientResults objPatientResult = new PatientResults();
                            objPatientResult = ilstPatientResult[0];
                            ilstPatientResult1.Add(objPatientResult);
                        }
                        if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "BP-SITTING SYS/DIA")
                        {
                            IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                            ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "BP-SITTING SYS/DIA" select p).ToList<PatientResults>();
                            PatientResults objPatientResult = new PatientResults();
                            ilstPatientResult[0].Units = "mm[Hg]";
                            objPatientResult = ilstPatientResult[0];
                            ilstPatientResult1.Add(objPatientResult);
                        }
                        if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "BODY TEMPERATURE")
                        {
                            IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                            //ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "BODY TEMPERATURE" select p).ToList<PatientResults>();
                            ilstPatientResult = ClinicalSummary.Vitals.Where(a => a.Loinc_Observation.ToUpper().Contains("BODY TEMPERATURE")).ToList<PatientResults>();
                            ilstPatientResult = ilstPatientResult.OrderByDescending(x => x.Captured_date_and_time).ToList<PatientResults>();
                            PatientResults objPatientResult = new PatientResults();
                            double celsius;
                            double fahrenheit = Convert.ToDouble(ilstPatientResult[0].Value);
                            celsius = (fahrenheit - 32) * 5 / 9;
                            double finalvalue = Math.Round(celsius);
                            ilstPatientResult[0].Value = finalvalue.ToString();
                            ilstPatientResult[0].Units = "Cel";
                            objPatientResult = ilstPatientResult[0];
                            ilstPatientResult1.Add(objPatientResult);
                        }
                        //else if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "BODY TEMPERATURE$")
                        //{
                        //    IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                        //    ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "BODY TEMPERATURE$" select p).ToList<PatientResults>();
                        //    PatientResults objPatientResult = new PatientResults();
                        //    objPatientResult = ilstPatientResult[0];
                        //    ilstPatientResult1.Add(objPatientResult);
                        //}
                        if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "PULSE OXIMETRY")
                        {
                            IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                            //ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "PULSE OXIMETRY" select p).ToList<PatientResults>();
                            ilstPatientResult = ClinicalSummary.Vitals.Where(a => a.Loinc_Observation.ToUpper().Contains("PULSE OXIMETRY")).ToList<PatientResults>();
                            ilstPatientResult = ilstPatientResult.OrderByDescending(x => x.Captured_date_and_time).ToList<PatientResults>();
                            PatientResults objPatientResult = new PatientResults();
                            ilstPatientResult[0].Units = "%";
                            objPatientResult = ilstPatientResult[0];
                            ilstPatientResult1.Add(objPatientResult);
                        }
                        //else if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "PULSE OXIMETRY$")
                        //{
                        //    IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                        //    ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "PULSE OXIMETRY$" select p).ToList<PatientResults>();
                        //    PatientResults objPatientResult = new PatientResults();
                        //    objPatientResult = ilstPatientResult[0];
                        //    ilstPatientResult1.Add(objPatientResult);
                        //}
                        if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "PULSE RATE")
                        {
                            IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                            ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "PULSE RATE" select p).ToList<PatientResults>();
                            PatientResults objPatientResult = new PatientResults();
                            ilstPatientResult[0].Units = "/min";
                            objPatientResult = ilstPatientResult[0];
                            ilstPatientResult1.Add(objPatientResult);
                        }
                        if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "RESPIRATORY RATE")
                        {
                            IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                            ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "RESPIRATORY RATE" select p).ToList<PatientResults>();
                            PatientResults objPatientResult = new PatientResults();
                            ilstPatientResult[0].Units = "/min";
                            objPatientResult = ilstPatientResult[0];
                            ilstPatientResult1.Add(objPatientResult);
                        }
                        if (ClinicalSummary.Vitals[i].Loinc_Observation.ToUpper() == "INHALED OXYGEN CONCENTRATION")
                        {
                            IList<PatientResults> ilstPatientResult = new List<PatientResults>();
                            ilstPatientResult = (from p in ClinicalSummary.Vitals where p.Loinc_Observation.ToUpper() == "INHALED OXYGEN CONCENTRATION" select p).ToList<PatientResults>();
                            PatientResults objPatientResult = new PatientResults();
                            ilstPatientResult[0].Units = "%";
                            objPatientResult = ilstPatientResult[0];
                            ilstPatientResult1.Add(objPatientResult);
                        }

                    }

                    //XmlElement TempElement = null;

                    //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[2].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;
                    XmlElement elemDate1 = null;
                    //XmlElement elemDate2 = null;
                    elemDate1 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[1];
                    // elemDate2 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[21].ChildNodes[0].ChildNodes[3].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2];
                    if (ilstPatientResult1.Count > 0)
                        elemDate1.InnerText = ilstPatientResult1[0].Captured_date_and_time.ToString("yyyy-MM-dd");
                    else
                        elemDate1.InnerText = string.Empty;
                    //elemDate2.InnerText = ClinicalSummary.Vitals[0].Captured_date_and_time.ToString("yyyy-MM-dd");


                    if (hashCheckedList["chkVitals"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ilstPatientResult1.Count; i++)
                        {

                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }
                            if (ilstPatientResult1[i].Loinc_Observation == "Height")
                            {
                                if (ilstPatientResult1[i].Value == string.Empty)
                                {
                                    ilstPatientResult1[i].Value = "0";
                                }
                                //decimal dInch = 0;
                                string sHeight = string.Empty;
                                Decimal heigntCM = 0;
                                string sFormula = "2.54";
                                if (ilstPatientResult1[i].Value != "")
                                {
                                    heigntCM = Convert.ToInt32(ilstPatientResult1[i].Value) * Convert.ToDecimal(sFormula);
                                }
                                if (Convert.ToString(heigntCM).Contains('.') == true)
                                {
                                    heigntCM = Convert.ToDecimal(Convert.ToString(heigntCM).Split('.')[0]);
                                }
                                //if (ilstPatientResult1[i].Value.Contains("'"))
                                //{
                                //    dInch = Convert.ToDecimal(ilstPatientResult1[i].Value.Split('\'')[0].Trim()) * 12 + Convert.ToDecimal(ilstPatientResult1[i].Value.Split('\'')[1].Trim());
                                //    decimal dFeet = Math.Floor(dInch / 12m);
                                //    decimal remainInch = decimal.Round((dInch % 12m), 2);
                                //    sHeight = dFeet.ToString() + "Ft " + remainInch.ToString() + "Inch";
                                //}
                                //else
                                //{
                                //    decimal inch = Convert.ToDecimal(ilstPatientResult1[i].Value);
                                //    decimal feet = Math.Floor(inch / 12m);
                                //    decimal remainInch = decimal.Round((inch % 12m), 2);
                                //    sHeight = feet.ToString() + "Ft " + remainInch.ToString() + "Inch";
                                //}
                                ////decimal inch = Convert.ToDecimal(ilstPatientResult1[i].Value);
                                ////decimal feet = Math.Floor(inch / 12m);
                                ////decimal remainInch = decimal.Round((inch % 12m), 2);
                                ////sHeight = feet.ToString() + "Ft " + remainInch.ToString() + "Inch";
                                //sInnerXML += "<tr><th>" + ilstPatientResult1[i].Loinc_Observation + "</th><td>" + sHeight + "</td></tr>";
                                sInnerXML += "<tr><th>" + ilstPatientResult1[i].Loinc_Observation + ", LOINC: " + ilstPatientResult1[i].Loinc_Identifier + "</th><td>" + heigntCM + " " + ilstPatientResult1[i].Units.ToString() + "</td></tr>";
                            }
                            else if (ilstPatientResult1[i].Loinc_Observation == "Weight")
                            {
                                Decimal weightKG = 0;
                                if (ilstPatientResult1[i].Value != "")
                                {
                                    string s = "0.454";
                                    if (ilstPatientResult1[i].Value.Contains('.') == true)
                                    {
                                        weightKG = Convert.ToInt32(ilstPatientResult1[i].Value.Split('.')[0]) * Convert.ToDecimal(s);
                                        Decimal weightKG1 = 0;
                                        s = "0.045";
                                        weightKG1 = Convert.ToInt32(ilstPatientResult1[i].Value.Split('.')[1]) * Convert.ToDecimal(s);
                                        weightKG = Math.Round(weightKG + weightKG1);
                                    }
                                    else
                                        weightKG = Math.Round(Convert.ToInt32(ilstPatientResult1[i].Value) * Convert.ToDecimal(s));

                                }

                                sInnerXML += "<tr><th>" + ilstPatientResult1[i].Loinc_Observation + ", LOINC: " + ilstPatientResult1[i].Loinc_Identifier + "</th><td>" + weightKG + " " + ilstPatientResult1[i].Units.ToString() + "</td></tr>";
                            }
                            else if (ilstPatientResult1[i].Loinc_Observation == "BP-Sitting Sys/Dia")
                            {
                                string[] sBPValue = ilstPatientResult1[i].Value.Split('/');
                                sInnerXML += "<tr><th>" + "BP-Sitting Systolic, LOINC: " + ilstPatientResult1[i].Loinc_Identifier + "</th><td>" + sBPValue[0].ToString() + " " + ilstPatientResult1[i].Units.ToString() + "</td></tr>";
                                sInnerXML += "<tr><th>" + "BP-Sitting Diastolic, LOINC: 8462-4" + "</th><td>" + sBPValue[1].ToString() + " " + ilstPatientResult1[i].Units.ToString() + "</td></tr>";
                            }
                            else
                                sInnerXML += "<tr><th>" + ilstPatientResult1[i].Loinc_Observation + ", LOINC: " + ilstPatientResult1[i].Loinc_Identifier + "</th><td>" + ilstPatientResult1[i].Value.ToString() + " " + ilstPatientResult1[i].Units.ToString() + "</td></tr>";
                            if (i == ilstPatientResult1.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);



                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27];


                    XmlDocument docAllergyEntry = new XmlDocument();

                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\VitalSign.xml"));
                    PatientResults objpatient = new PatientResults();
                    IList<PatientResults> ilstPatientResultbp = new List<PatientResults>();
                    ilstPatientResultbp = (from p in ilstPatientResult1 where p.Loinc_Observation.ToUpper() == "BP-SITTING SYS/DIA" select p).ToList<PatientResults>();
                    //IList<PatientResults> ilstPatientResultothers = new List<PatientResults>();
                    ilstPatientResult1 = (from p in ilstPatientResult1 where p.Loinc_Observation.ToUpper() != "BP-SITTING SYS/DIA" select p).ToList<PatientResults>();
                    ilstPatientResult1 = ilstPatientResult1.Union(ilstPatientResultbp).ToList<PatientResults>();
                    for (int i = 0; i < ilstPatientResult1.Count; i++)
                    {
                        if (ilstPatientResult1[i].Loinc_Observation.ToUpper() != "BP-SITTING SYS/DIA")
                        {
                            XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                            xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                            elemOldentry.LastChild.AppendChild(xfragAllergy);
                            xmlDoc.Save(sPrintFileName);
                            xmlDoc.Load(sPrintFileName);
                            elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27];

                            //elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];

                            XmlElement elemVitalCapturedDateTime = null;
                            XmlElement elemCode = null;
                            //XmlElement elemReference = null;
                            XmlElement elemEffectiveTime = null;
                            XmlElement elemValue = null;


                            //       elemVitalCapturedDateTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5];

                            elemVitalCapturedDateTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[5];

                            if (ilstPatientResult1[i].Results_Type.ToString().ToUpper() == "VITALS")
                                elemVitalCapturedDateTime.Attributes[0].Value = ilstPatientResult1[i].Captured_date_and_time.ToString("yyyyMMdd");

                            //    elemCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[3];

                            elemCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[3];

                            string sLoincIdentifier = ilstPatientResult1[i].Loinc_Identifier;
                            if (sLoincIdentifier != string.Empty)
                                elemCode.Attributes[0].Value = sLoincIdentifier; //ClinicalSummary.Vitals[i].Loinc_Identifier;
                            else
                                elemCode.Attributes[0].Value = "39156-5";
                            elemCode.Attributes[3].Value = ilstPatientResult1[i].Loinc_Observation;
                            //elemReference = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[21].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                            //elemReference.Attributes[0].Value = "";

                            //  elemEffectiveTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[6];

                            elemEffectiveTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[6];

                            if (ilstPatientResult1[i].Results_Type.ToString().ToUpper() == "VITALS")
                                elemEffectiveTime.Attributes[0].Value = ilstPatientResult1[i].Captured_date_and_time.ToString("yyyyMMdd");

                            //    elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[7];

                            elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[5 + i].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[7];

                            string sValues = ilstPatientResult1[i].Value;
                            if (ilstPatientResult1[i].Loinc_Observation == "Height")
                            {
                                //if (sValues == string.Empty)
                                //{
                                //    sValues = "0";
                                //}
                                //decimal dInches = 0;
                                //string sHeight = string.Empty;
                                //if (sValues.Contains("'"))
                                //{
                                //    dInches = Convert.ToDecimal(sValues.Split('\'')[0].Trim()) * 12 + Convert.ToDecimal(sValues.Split('\'')[1].Trim());
                                //    decimal dFeet = Math.Floor(dInches / 12m);
                                //    decimal remainInch = decimal.Round((dInches % 12m), 2);
                                //    sHeight = dFeet.ToString() + "." + remainInch.ToString();
                                //}
                                //else
                                //{
                                //    decimal inch = Convert.ToDecimal(sValues);
                                //    decimal feet = Math.Floor(inch / 12m);
                                //    decimal remainInch = decimal.Round((inch % 12m), 2);
                                //    sHeight = feet.ToString() + "." + remainInch.ToString();
                                //}
                                ////decimal inch = Convert.ToDecimal(sValues);
                                ////decimal feet = Math.Floor(inch / 12m);
                                ////decimal remainInch = decimal.Round((inch % 12m), 2);
                                ////string sHeight = feet.ToString() + "." + remainInch.ToString();
                                //elemValue.Attributes[1].Value = sHeight;
                                if (ilstPatientResult1[i].Value == string.Empty)
                                {
                                    ilstPatientResult1[i].Value = "0";
                                }
                                //decimal dInch = 0;
                                string sHeight = string.Empty;
                                Decimal heigntCM = 0;
                                string sFormula = "2.54";
                                if (ilstPatientResult1[i].Value != "")
                                {
                                    heigntCM = Convert.ToInt32(ilstPatientResult1[i].Value) * Convert.ToDecimal(sFormula);
                                }
                                if (Convert.ToString(heigntCM).Contains('.') == true)
                                {
                                    heigntCM = Convert.ToDecimal(Convert.ToString(heigntCM).Split('.')[0]);
                                }
                                elemValue.Attributes[1].Value = heigntCM.ToString();
                            }
                            else if (ilstPatientResult1[i].Loinc_Observation == "Weight")
                            {
                                Decimal weightKG = 0;
                                if (ilstPatientResult1[i].Value != "")
                                {
                                    string s = "0.454";
                                    if (ilstPatientResult1[i].Value.Contains('.') == true)
                                    {

                                        weightKG = Convert.ToInt32(ilstPatientResult1[i].Value.Split('.')[0]) * Convert.ToDecimal(s);
                                        Decimal weightKG1 = 0;
                                        s = "0.045";
                                        weightKG1 = Convert.ToInt32(ilstPatientResult1[i].Value.Split('.')[1]) * Convert.ToDecimal(s);
                                        weightKG = Math.Round(weightKG + weightKG1);
                                    }
                                    else
                                        weightKG = Math.Round(Convert.ToInt32(ilstPatientResult1[i].Value) * Convert.ToDecimal(s));

                                    elemValue.Attributes[1].Value = weightKG.ToString();
                                }
                            }
                            else
                            {
                                if (sValues != string.Empty)
                                    elemValue.Attributes[1].Value = sValues.ToString().Replace("/", ""); //ClinicalSummary.Vitals[i].Value;
                                else
                                    elemValue.Attributes[1].Value = "2";
                            }
                            string sUnits = ilstPatientResult1[i].Units;
                            if (ilstPatientResult1[i].Loinc_Observation == "Height")
                            {
                                sUnits = sUnits.Replace(" ", "/");
                            }
                            if (sUnits != string.Empty)
                            {
                                if (ilstPatientResult1[i].Loinc_Observation == "Height")
                                {
                                    elemValue.Attributes[2].Value = "cm";
                                }
                                else if (ilstPatientResult1[i].Loinc_Observation == "Weight")
                                {
                                    elemValue.Attributes[2].Value = "kg";
                                }
                                else
                                {
                                    elemValue.Attributes[2].Value = sUnits;// ClinicalSummary.Vitals[i].Units;

                                }

                            }
                            else
                                elemValue.Attributes[2].Value = "kg/m2";
                        }
                        else
                        {

                            {
                                string[] s = ilstPatientResult1[i].Loinc_Observation.Split(' ')[1].Split('/');
                                foreach (string sdia in s)
                                {
                                    int iArryCount = i;
                                    if (sdia.ToUpper() != "SYS")
                                    {
                                        iArryCount++;
                                    }

                                    XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                                    xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                                    elemOldentry.LastChild.AppendChild(xfragAllergy);
                                    xmlDoc.Save(sPrintFileName);
                                    xmlDoc.Load(sPrintFileName);
                                    elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27];

                                    //elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[13].ChildNodes[0].ChildNodes[3];

                                    XmlElement elemVitalCapturedDateTime = null;
                                    XmlElement elemCode = null;
                                    XmlElement elemEffectiveTime = null;
                                    XmlElement elemValue = null;


                                    //      elemVitalCapturedDateTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4 + iArryCount].ChildNodes[0].ChildNodes[5];

                                    elemVitalCapturedDateTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[5 + iArryCount].ChildNodes[0].ChildNodes[5];

                                    if (ilstPatientResult1[i].Results_Type.ToString().ToUpper() == "VITALS")
                                        elemVitalCapturedDateTime.Attributes[0].Value = ilstPatientResult1[i].Captured_date_and_time.ToString("yyyyMMdd");

                                    //    elemCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4 + iArryCount].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[3];

                                    elemCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[5 + iArryCount].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[3];

                                    string sLoincIdentifier = ilstPatientResult1[i].Loinc_Identifier;

                                    if (sLoincIdentifier != string.Empty)
                                        elemCode.Attributes[0].Value = sLoincIdentifier; //ClinicalSummary.Vitals[i].Loinc_Identifier;
                                    else
                                        elemCode.Attributes[0].Value = "39156-5";
                                    if (sdia.ToUpper() == "SYS")
                                    {
                                        elemCode.Attributes[0].Value = "8480-6";
                                        elemCode.Attributes[3].Value = "Intravascular Systolic";
                                    }
                                    else
                                    {
                                        elemCode.Attributes[0].Value = "8462-4";
                                        elemCode.Attributes[3].Value = "BP Diastolic";
                                    }

                                    //    elemEffectiveTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4 + iArryCount].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[6];

                                    elemEffectiveTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[5 + iArryCount].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[6];

                                    if (ilstPatientResult1[i].Results_Type.ToString().ToUpper() == "VITALS")
                                        elemEffectiveTime.Attributes[0].Value = ilstPatientResult1[i].Captured_date_and_time.ToString("yyyyMMdd");

                                    //  elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4 + iArryCount].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[7];

                                    elemValue = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[5 + iArryCount].ChildNodes[0].ChildNodes[6].ChildNodes[0].ChildNodes[7];

                                    string sValues = ilstPatientResult1[i].Value;
                                    if (ilstPatientResult1[i].Loinc_Observation == "Height")
                                    {
                                        if (sValues == string.Empty)
                                        {
                                            sValues = "0";
                                        }
                                        decimal dInches = 0;
                                        string sHeight = string.Empty;
                                        if (sValues.Contains("'"))
                                        {
                                            dInches = Convert.ToDecimal(sValues.Split('\'')[0].Trim()) * 12 + Convert.ToDecimal(sValues.Split('\'')[1].Trim());
                                            decimal dFeet = Math.Floor(dInches / 12m);
                                            decimal remainInch = decimal.Round((dInches % 12m), 2);
                                            sHeight = dFeet.ToString() + "." + remainInch.ToString();
                                        }
                                        else
                                        {
                                            decimal inch = Convert.ToDecimal(sValues);
                                            decimal feet = Math.Floor(inch / 12m);
                                            decimal remainInch = decimal.Round((inch % 12m), 2);
                                            sHeight = feet.ToString() + "." + remainInch.ToString();
                                        }
                                        //decimal inch = Convert.ToDecimal(sValues);
                                        //decimal feet = Math.Floor(inch / 12m);
                                        //decimal remainInch = decimal.Round((inch % 12m), 2);
                                        //string sHeight = feet.ToString() + "." + remainInch.ToString();
                                        elemValue.Attributes[1].Value = sHeight;
                                    }
                                    else
                                    {
                                        if (sValues != string.Empty)
                                        {
                                            if (sdia.ToUpper() == "SYS")
                                            {
                                                elemValue.Attributes[1].Value = sValues.ToString().Split('/')[0];
                                            }
                                            else
                                            {
                                                elemValue.Attributes[1].Value = sValues.ToString().Split('/')[1];
                                            }
                                        }
                                        //elemValue.Attributes[1].Value = sValues.ToString().Replace("/", ""); //ClinicalSummary.Vitals[i].Value;
                                        else
                                            elemValue.Attributes[1].Value = "2";



                                    }
                                    string sUnits = ilstPatientResult1[i].Units;
                                    if (ilstPatientResult1[i].Loinc_Observation == "Height")
                                    {
                                        sUnits = sUnits.Replace(" ", "/");
                                    }
                                    if (sUnits != string.Empty)
                                        elemValue.Attributes[2].Value = "mm[Hg]";// ClinicalSummary.Vitals[i].Units;
                                    else
                                        elemValue.Attributes[2].Value = "kg/m2";
                                }
                            }
                        }
                    }

                }
            }
            else if (hashCheckedList["chkVitals"] != null && hashCheckedList["chkVitals"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4];

                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                //XmlElement elemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].add;

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0].ChildNodes[4];

                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }



            #endregion


            #region Instructions

            if (ClinicalSummary.Encounter != null && ClinicalSummary.Encounter.Count != 0 && hashCheckedList["chkClinicalInstruction"] != null && hashCheckedList["chkClinicalInstruction"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[29].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "INSTRUCTIONS")
                {

                    XmlElement elemInsPlan = null;
                    elemInsPlan = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[29].ChildNodes[0].ChildNodes[3];
                    if (hashCheckedList["chkClinicalInstruction"].ToString().ToUpper() == "TRUE")
                    {
                        string sRecomentedMaterial = string.Empty;
                        for (int i = 0; i < ClinicalSummary.Document_Material.Count; i++)
                        {
                            //<<<<<<< HL7Generator.cs
                            //                            if (i == 0)
                            //                            {
                            //                                sRecomentedMaterial += "\n" + "Recommended Patient Decision Aids:" + "\n" + ClinicalSummary.Document_Material[i].Document_Type;
                            //                            }
                            //                            else
                            //||||||| 1.74
                            //string sRecomentedMaterial=string.Empty;
                            //for (int i = 0; i < ClinicalSummary.Document_Material.Count; i++)
                            //=======
                            if (i == 0)
                            //>>>>>>> 1.74.2.10
                            {
                                //<<<<<<< HL7Generator.cs
                                //                                sRecomentedMaterial += ", " + ClinicalSummary.Document_Material[i].Document_Type;
                                //||||||| 1.74
                                if (i == 0)
                                {
                                    sRecomentedMaterial += "\n" + "Recommended Patient Decision Aids:" + "\n" + ClinicalSummary.Document_Material[i].Document_Type;
                                }
                                else
                                {
                                    sRecomentedMaterial += ", " + ClinicalSummary.Document_Material[i].Document_Type;
                                }
                                //=======
                                //                                sRecomentedMaterial += "\n" + "Recommended Patient Decision Aids:" + "\n" + ClinicalSummary.Document_Material[i].Document_Type;
                                //                            }
                                //                            else
                                //                            {
                                //                                sRecomentedMaterial += ", " + ClinicalSummary.Document_Material[i].Document_Type;
                                //>>>>>>> 1.74.2.10
                            }
                        }
                        // sRecomentedMaterial=ClinicalSummary.Encounter[0].Follow_Reason_Notes+"\n"+
                        // elemInsPlan.InnerText = ClinicalSummary.Encounter[0].Follow_Reason_Notes;
                        elemInsPlan.InnerText = ClinicalSummary.Encounter[0].Follow_Reason_Notes + sRecomentedMaterial;
                    }
                    else
                    {
                        //XmlElement elemInsPlan = null;
                        //elemInsPlan = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[25].ChildNodes[0].ChildNodes[3];
                        elemInsPlan.InnerText = "This Section is Omitted.";

                    }
                }
            }
            else if (hashCheckedList["chkClinicalInstruction"] != null && hashCheckedList["chkClinicalInstruction"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[29].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[29].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xelemSection.Attributes.Append(xAttribute);
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[29].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }




            #endregion

            //chief_complaints.hpi_value where hpi_element=chief complaints
            #region ChiefComplaints
            if (ClinicalSummary.ChiefComplaints != null && ClinicalSummary.ChiefComplaints.Count != 0 && hashCheckedList["chkChiefComplaints"] != null && hashCheckedList["chkChiefComplaints"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[31].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "ASSESSMENTS")
                {

                    XmlElement elemChiefText = null;
                    elemChiefText = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[31].ChildNodes[0].ChildNodes[3];
                    // string hpi_element = "chief complaints";
                    string sAssessmentText = string.Empty;
                    string startTime = ClinicalSummary.Birth_Date.ToString();

                    string endTime = ClientSession.LocalDate;

                    TimeSpan duration = DateTime.ParseExact(endTime, "M'/'d'/'yyyy", null).Subtract(DateTime.Parse(startTime));

                    int iAge = Convert.ToInt32(duration.TotalDays) / 365;

                    if (hashCheckedList["chkChiefComplaints"].ToString().ToUpper() == "TRUE")
                    {
                        if (ClinicalSummary.ChiefComplaints[0].HPI_Element.ToString().ToUpper() == "CHIEF COMPLAINTS")
                        {
                            //sAssessmentText = ClinicalSummary.Last_Name + ", " + ClinicalSummary.First_Name + " " + ClinicalSummary.MI;
                            //if (iAge != null)
                            //{
                            //    sAssessmentText += " is a " + iAge + " -year-old ";
                            //}
                            //if (ClinicalSummary.Sex.ToUpper() == "FEMALE")
                            //    sAssessmentText += "female patient presented for her ";
                            //else if (ClinicalSummary.Sex.ToUpper() == "MALE")
                            //    sAssessmentText += "male patient presented for his ";
                            //else if (ClinicalSummary.Sex.ToUpper() == "UNKNOWN")
                            //    sAssessmentText += "unknown patient presented for his/her ";


                            //elemChiefText.InnerText = sAssessmentText + ClinicalSummary.ChiefComplaints[0].HPI_Value;
                            elemChiefText.InnerText = ClinicalSummary.ChiefComplaints[0].HPI_Value;
                        }
                    }
                    else
                    {
                        elemChiefText.InnerText = "This Section is Omitted";
                    }

                }
            }
            else if (hashCheckedList["chkChiefComplaints"] != null && hashCheckedList["chkChiefComplaints"].ToString().ToUpper() == "FALSE")
            {

                XmlElement elemChiefText = null;
                elemChiefText = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[31].ChildNodes[0].ChildNodes[3];
                // string hpi_element = "chief complaints";

                elemChiefText.InnerText = "This Section is Omitted";
                //XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[27].ChildNodes[0];
                //XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                //xAttribute.Value = "NI";
                //xelemSection.Attributes.Append(xAttribute);
                //xmlDoc.Save(sPrintFileName);
                //xmlDoc.Load(sPrintFileName);
            }
            else
            {

                XmlElement xelemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[31].ChildNodes[0];
                XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[31].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xelemSection.Attributes.Append(xAttribute);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }



            #endregion

            #region cognitive function

            ////ClinicalSummary.Care_Plan_Cognitive_Function = ClinicalSummary.Care_Plan_Cognitive_Function.Where(a => a.Status != string.Empty && a.Plan_Date != string.Empty).ToList<CarePlan>();
            //ClinicalSummary.Care_Plan_Cognitive_Function = ClinicalSummary.Care_Plan_Cognitive_Function.Where(a => a.Status != string.Empty).ToList<CarePlan>();
            //if (ClinicalSummary.Care_Plan_Cognitive_Function != null && ClinicalSummary.Care_Plan_Cognitive_Function.Count != 0)
            //{
            //    XmlElement TempElement = null;
            //    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0];
            //    XmlDocument docAllergyEntry = new XmlDocument();
            //    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CognitiveFunction.xml"));


            //    XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
            //    xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
            //    //xmlDoc.DocumentElement.LastChild.ChildNodes[0].ChildNodes[27].AppendChild(xfragAllergy);
            //    elemOldentry.AppendChild(xfragAllergy);
            //    xmlDoc.Save(sPrintFileName);
            //    xmlDoc.Load(sPrintFileName);



            //    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[29].ChildNodes[0].ChildNodes[3];
            //    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
            //    string sInnerXML = string.Empty;

            //    string sPlanDate = string.Empty;
            //    DateTime dtPlandate = new DateTime();

            //    for (int i = 0; i < ClinicalSummary.Care_Plan_Cognitive_Function.Count; i++)
            //    {
            //        if (i == 0)
            //        {
            //            sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
            //        }
            //        sPlanDate = ClinicalSummary.Care_Plan_Cognitive_Function[i].Plan_Date;

            //        if (sPlanDate != "")
            //        {
            //            dtPlandate = Convert.ToDateTime(sPlanDate);
            //        }
            //        if (sPlanDate != "")
            //        {
            //            sInnerXML += "<tr><td>" + ClinicalSummary.Care_Plan_Cognitive_Function[i].Care_Plan_Notes + "</td><td>" + dtPlandate.ToString("yyyyMMdd") + "</td><td>" + "Active" + "</td></tr>";
            //        }
            //        else
            //        {
            //            sInnerXML += "<tr><td>" + ClinicalSummary.Care_Plan_Cognitive_Function[i].Care_Plan_Notes + "</td><td>" + String.Empty + "</td><td>" + "Active" + "</td></tr>";
            //        }
            //        if (i == ClinicalSummary.Care_Plan_Cognitive_Function.Count - 1)
            //        {
            //            sInnerXML += "</tbody>";
            //        }
            //    }
            //    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
            //    elemOld.LastChild.AppendChild(xfrag);


            //    for (int i = 0; i < ClinicalSummary.Care_Plan_Cognitive_Function.Count; i++)
            //    {

            //        XmlElement Problemlow = null;
            //        XmlElement Problemhigh = null;
            //        XmlElement ProblemCodeAndDisplayName = null;
            //        sPlanDate = ClinicalSummary.Care_Plan_Cognitive_Function[i].Plan_Date;
            //        //DateTime dtPlandate = new DateTime();
            //        if (sPlanDate != "")
            //        {
            //            dtPlandate = Convert.ToDateTime(sPlanDate);
            //        }

            //        Problemlow = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[29].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0];
            //        if (sPlanDate != "")
            //        {
            //            Problemlow.Attributes[0].Value = dtPlandate.ToString("yyyyMMdd");//ClinicalSummary.Care_Plan_Cognitive_Function[i].Plan_Date;
            //        }
            //        else
            //        {
            //            Problemlow.Attributes[0].Value = string.Empty;
            //        }
            //        ProblemCodeAndDisplayName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[29].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6];
            //        // if(ClinicalSummary.Care_Plan_Cognitive_Function[i].Care_Plan_Notes.ToUpper()=="")
            //        // ProblemCodeAndDisplayName.Attributes[1].Value = ClinicalSummary.Care_Plan_Cognitive_Function[i].Status_Value;
            //        ProblemCodeAndDisplayName.Attributes[3].Value = ClinicalSummary.Care_Plan_Cognitive_Function[i].Care_Plan_Notes;
            //    }
            //}
            //else
            //{
            //    XmlElement TempElement = null;
            //    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0];
            //    XmlDocument docAllergyEntry = new XmlDocument();
            //    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CognitiveFunction_DP.xml"));

            //    XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
            //    xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
            //    //xmlDoc.DocumentElement.LastChild.ChildNodes[0].ChildNodes[27].AppendChild(xfragAllergy);
            //    elemOldentry.AppendChild(xfragAllergy);
            //    xmlDoc.Save(sPrintFileName);
            //    xmlDoc.Load(sPrintFileName);

            //}

            #endregion



            #region CdaFile
            // XmlElement xelemSection1 = (XmlElement)xmlDoc.GetElementsByTagName("xml-stylesheet")[0];
            // //XmlAttribute xAttribute = xelemSection.OwnerDocument.CreateAttribute("href");
            //// xAttribute.Value = '"'+sPrintFileName+'"';
            // xelemSection1.Attributes[1].Value = '"' + sPrintFileName + '"';
            // //xelemSection.Attributes.Append(xAttribute);
            // xmlDoc.Save(sPrintFileName);
            // xmlDoc.Load(sPrintFileName);

            #endregion


            #region Implant

            if (ClinicalSummary.ImplantProcedure != null && ClinicalSummary.ImplantProcedure.Count != 0 && hashCheckedList["chkImplant"] != null && hashCheckedList["chkImplant"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "IMPLANTS")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    if (hashCheckedList["chkImplant"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.ImplantProcedure.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }
                            if (ClinicalSummary.ImplantProcedure[i].Device_Identifier_UDI.Trim() != "" || ClinicalSummary.ImplantProcedure[i].Device_Identifier_DI.Trim() != "")
                                if (ClinicalSummary.ImplantProcedure[i].Device_Identifier_UDI.Trim() != "")
                                    sInnerXML += "<tr><td>" + ClinicalSummary.ImplantProcedure[i].GMDN_PT_Name + "</td><td></td><td>" + ClinicalSummary.ImplantProcedure[i].Device_Identifier_UDI + "</td><td>FDA</td></tr>";
                                else
                                    sInnerXML += "<tr><td>" + ClinicalSummary.ImplantProcedure[i].GMDN_PT_Name + "</td><td></td><td>" + ClinicalSummary.ImplantProcedure[i].Device_Identifier_DI + "</td><td>FDA</td></tr>";
                            if (i == ClinicalSummary.ImplantProcedure.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        //XmlElement TempElement = null;
                        //XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].ChildNodes[3];
                        //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                        //string sInnerXML = string.Empty;
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                        //xfrag.InnerXml = sInnerXML;
                        //elemOld.LastChild.AppendChild(xfrag);
                        //xmlDoc.Save(sPrintFileName);
                        //xmlDoc.Load(sPrintFileName);
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33];
                    XmlDocument docAllergyEntry = new XmlDocument();
                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Implant.xml"));
                    for (int i = 0; i < ClinicalSummary.ImplantProcedure.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldentry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);
                        elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33];

                        XmlElement elemCode = null;
                        XmlElement elemlow = null;
                        XmlElement elemextension = null;
                        XmlElement elemGMPTName = null;
                        //XmlElement elemTargetSite = null;

                        elemCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[3];
                        elemlow = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[1];
                        // elemTargetSite = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[29].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6];
                        elemlow.Attributes[0].Value = Convert.ToDateTime(ClinicalSummary.ImplantProcedure[i].Created_Date_And_Time).ToString("yyyyMMdd");
                        elemextension = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[8].ChildNodes[0].ChildNodes[7];
                        elemGMPTName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[8].ChildNodes[0].ChildNodes[8].ChildNodes[1];
                        if (ClinicalSummary.ImplantProcedure[i].Device_Identifier_UDI.Trim() != "")
                        {
                            elemextension.Attributes[1].Value = ClinicalSummary.ImplantProcedure[i].Device_Identifier_UDI;
                            elemGMPTName.Attributes[0].Value = ClinicalSummary.ImplantProcedure[i].Device_Identifier_UDI;
                            elemCode.Attributes[0].Value = ClinicalSummary.ImplantProcedure[i].Device_Identifier_UDI;
                            // elemTargetSite.Attributes[0].Value = ClinicalSummary.ImplantProcedure[i].Device_Identifier_UDI;
                        }
                        else
                        {
                            elemextension.Attributes[1].Value = ClinicalSummary.ImplantProcedure[i].Device_Identifier_DI;
                            elemGMPTName.Attributes[0].Value = ClinicalSummary.ImplantProcedure[i].Device_Identifier_DI;
                            elemCode.Attributes[0].Value = ClinicalSummary.ImplantProcedure[i].Device_Identifier_UDI;
                            // elemTargetSite.Attributes[0].Value = ClinicalSummary.ImplantProcedure[i].Device_Identifier_DI;
                        }
                        // elemTargetSite.Attributes[1].Value = ClinicalSummary.ImplantProcedure[i].GMDN_PT_Name;
                        elemGMPTName.Attributes[3].Value = ClinicalSummary.ImplantProcedure[i].GMDN_PT_Definition;
                        elemCode.Attributes[3].Value = ClinicalSummary.ImplantProcedure[i].GMDN_PT_Definition;
                    }
                }
            }
            else if (hashCheckedList["chkImplant"] != null && hashCheckedList["chkImplant"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                //XmlElement elemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].add;

                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[33].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }





            #endregion


            #region MentalSatus

            if (ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus != null && ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus.Count != 0 && hashCheckedList["chkMentalStatus"] != null && hashCheckedList["chkMentalStatus"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "MENTAL STATUS")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    if (hashCheckedList["chkMentalStatus"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }
                            string sDate = string.Empty;
                            if (ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus[i].Plan_Date != null)
                            {
                                if (ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus[i].Plan_Date.Length > 9)
                                {
                                    sDate = Convert.ToDateTime(ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus[i].Plan_Date).ToString("MMM dd,yyyy");
                                }
                                else
                                {
                                    sDate = ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus[i].Plan_Date;
                                }
                            }


                            sInnerXML += "<tr><td>" + ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus[i].Care_Name_Value + "</td><td>" + sDate + "</td></tr>";
                            if (i == ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35];
                    XmlDocument docAllergyEntry = new XmlDocument();
                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\MentalStatus.xml"));
                    for (int i = 0; i < ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldentry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35];

                        XmlElement eEffectiveTimeLow = null;
                        XmlElement eCode = null;
                        XmlElement eDisplayName = null;


                        eEffectiveTimeLow = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0];
                        if (ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus[i].Plan_Date.Trim() != "")
                            eEffectiveTimeLow.Attributes[0].Value = Convert.ToDateTime(ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus[i].Plan_Date).ToString("yyyyMMdd");
                        eCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6];
                        eCode.Attributes[1].Value = ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus[i].Snomed_Code;
                        eDisplayName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6];
                        eDisplayName.Attributes[2].Value = ClinicalSummary.Care_Plan_Cognitive_Function_MentalStatus[i].Care_Name_Value;

                    }
                }
            }
            else if (hashCheckedList["chkMentalStatus"] != null && hashCheckedList["chkMentalStatus"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {

                //XmlElement elemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].add;

                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[35].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            #endregion

            #region FunctionalSatus

            if (ClinicalSummary.Care_Plan_FunctionalStatus != null && ClinicalSummary.Care_Plan_FunctionalStatus.Count != 0 && hashCheckedList["chkFunctionalStatus"] != null && hashCheckedList["chkFunctionalStatus"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "FUNCTIONAL STATUS")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    if (hashCheckedList["chkFunctionalStatus"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.Care_Plan_FunctionalStatus.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }
                            string sDate = string.Empty;
                            if (ClinicalSummary.Care_Plan_FunctionalStatus[i].Plan_Date != null)
                            {
                                if (ClinicalSummary.Care_Plan_FunctionalStatus[i].Plan_Date.Length > 9)
                                {
                                    sDate = Convert.ToDateTime(ClinicalSummary.Care_Plan_FunctionalStatus[i].Plan_Date).ToString("MMM dd,yyyy");
                                }
                                else
                                {
                                    sDate = ClinicalSummary.Care_Plan_FunctionalStatus[i].Plan_Date;
                                }
                            }


                            sInnerXML += "<tr><td ID='FUNC" + i + 1 + "'>" + ClinicalSummary.Care_Plan_FunctionalStatus[i].Care_Name_Value + "</td><td>" + sDate + "</td></tr>";
                            if (i == ClinicalSummary.Care_Plan_FunctionalStatus.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37];
                    XmlDocument docAllergyEntry = new XmlDocument();
                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\FunctionalStatus.xml"));
                    for (int i = 0; i < ClinicalSummary.Care_Plan_FunctionalStatus.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldentry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37];

                        XmlElement eReference = null;
                        XmlElement eEffectiveTime = null;
                        XmlElement eCode = null;
                        XmlElement eDisplayName = null;
                        XmlElement eEffectiveTime1 = null;
                        eReference = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[3].ChildNodes[0];
                        eReference.Attributes[0].Value = "#FUNC" + i + 1;//ClinicalSummary.Care_Plan_FunctionalStatus[i].Care_Name_Value;
                        eEffectiveTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[5];
                        if (ClinicalSummary.Care_Plan_FunctionalStatus[i].Plan_Date.Trim() != "")
                            eEffectiveTime.Attributes[0].Value = Convert.ToDateTime(ClinicalSummary.Care_Plan_FunctionalStatus[i].Plan_Date).ToString("yyyyMMdd");
                        eCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[6];
                        eCode.Attributes[1].Value = ClinicalSummary.Care_Plan_FunctionalStatus[i].Snomed_Code;
                        eDisplayName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[6];
                        eDisplayName.Attributes[2].Value = ClinicalSummary.Care_Plan_FunctionalStatus[i].Care_Name_Value;
                        eEffectiveTime1 = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5].ChildNodes[0].ChildNodes[4];
                        if (ClinicalSummary.Care_Plan_FunctionalStatus[i].Plan_Date.Trim() != "")
                            eEffectiveTime1.Attributes[0].Value = Convert.ToDateTime(ClinicalSummary.Care_Plan_FunctionalStatus[i].Plan_Date).ToString("yyyyMMdd");
                    }
                }
            }
            else if (hashCheckedList["chkFunctionalStatus"] != null && hashCheckedList["chkFunctionalStatus"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                //XmlElement elemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].add;

                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[37].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            #endregion

            #region Goals

            if (ClinicalSummary.TreatmentPlan != null && ClinicalSummary.TreatmentPlan.Count != 0 && hashCheckedList["chkGoals"] != null && hashCheckedList["chkGoals"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "GOALS SECTION")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    if (hashCheckedList["chkGoals"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.TreatmentPlan.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }
                            sInnerXML += "<tr><td>" + ClinicalSummary.TreatmentPlan[i].Plan.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</td><td> N/A </td><td>" + ClinicalSummary.Encounter[0].Date_of_Service.ToString("MMM dd,yyyy") + "</td></tr>";
                            if (i == ClinicalSummary.TreatmentPlan.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39];
                    XmlDocument docAllergyEntry = new XmlDocument();
                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Goals.xml"));
                    for (int i = 0; i < ClinicalSummary.TreatmentPlan.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldentry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39];

                        XmlElement eEffectiveTime = null;
                        XmlElement ePlanGoal = null;
                        XmlElement eTime = null;
                        XmlElement eSpecialityNPI = null;
                        XmlElement eSpeciality = null;
                        XmlElement egiven = null;
                        XmlElement eFamily = null;
                        XmlElement eSuffix = null;
                        XmlElement AssignedAuthor = null;


                        eEffectiveTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[4];
                        eEffectiveTime.Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                        ePlanGoal = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[5];
                        ePlanGoal.InnerText = ClinicalSummary.TreatmentPlan[i].Plan.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                        eTime = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[1];
                        eTime.Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                        eSpecialityNPI = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[1];
                        eSpecialityNPI.Attributes[1].Value = ClinicalSummary.phyList[0].Taxonomy_Description.Replace("&", "&amp;");
                        //if (Phy.PhyColor.Trim() != string.Empty)
                        //    eSpecialityNPI.Attributes[0].Value = Phy.PhyColor.Split('-')[0];
                        eSpeciality = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[1];
                        eSpecialityNPI.Attributes[0].Value = ClinicalSummary.phyList[0].Taxonomy_Code;
                        //if (Phy.PhyColor.Trim() != string.Empty)
                        //    eSpeciality.Attributes[1].Value = Phy.PhyColor.Split('-')[1];



                        egiven = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[0];
                        egiven.InnerText = Phy.PhyFirstName;
                        eFamily = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[1];
                        eFamily.InnerText = Phy.PhyLastName;
                        eSuffix = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[2];
                        eSuffix.InnerText = Phy.PhySuffix;




                        AssignedAuthor = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[4 + i].ChildNodes[0].ChildNodes[7].ChildNodes[2].ChildNodes[0];
                        if (ClinicalSummary.PhysicianLibraryDocumentation.Count > 0 && ClinicalSummary.PhysicianLibraryDocumentation[0].Physician_SSN_Number.Trim() != "")
                        {
                            AssignedAuthor.Attributes[0].Value = ClinicalSummary.PhysicianLibraryDocumentation[0].Physician_SSN_Number;
                        }
                        else
                            AssignedAuthor.Attributes[0].Value = "111-111-111";


                    }
                }
            }
            else if (hashCheckedList["chkGoals"] != null && hashCheckedList["chkGoals"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                //XmlElement elemSection = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[17].ChildNodes[0].add;

                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[39].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion

            #region HealthConcern

            if (ClinicalSummary.HealthConcernProblemList != null && ClinicalSummary.HealthConcernProblemList.Count != 0 && hashCheckedList["chkHealthConcern"] != null && hashCheckedList["chkHealthConcern"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[41].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "HEALTH CONCERNS SECTION")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[41].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    if (hashCheckedList["chkHealthConcern"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.HealthConcernProblemList.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";

                            }
                            string sDate = string.Empty;
                            if (ClinicalSummary.HealthConcernProblemList[i].Date_Diagnosed.Trim() != string.Empty)
                            {
                                if (ClinicalSummary.HealthConcernProblemList[i].Date_Diagnosed.Contains('-') == true)
                                {
                                    if (ClinicalSummary.HealthConcernProblemList[i].Date_Diagnosed.Split('-').Length > 2)
                                        sDate = Convert.ToDateTime(ClinicalSummary.HealthConcernProblemList[i].Date_Diagnosed).ToString("MMM dd,yyyy");
                                    else if (ClinicalSummary.HealthConcernProblemList[i].Date_Diagnosed.Split('-').Length > 1)
                                        sDate = Convert.ToDateTime(ClinicalSummary.HealthConcernProblemList[i].Date_Diagnosed).ToString("MMM ,yyyy");

                                }
                                else
                                    sDate = ClinicalSummary.HealthConcernProblemList[i].Date_Diagnosed;
                            }


                            sInnerXML += "<tr><td>" + ClinicalSummary.HealthConcernProblemList[i].Problem_Description + "</td><td>" + ClinicalSummary.HealthConcernProblemList[i].Status + "</td><td>" + sDate + "</td></tr>";
                            if (i == ClinicalSummary.HealthConcernProblemList.Count - 1)
                            {
                                if (ClinicalSummary.Assessment.Count > 0)
                                {

                                    IList<Assessment> ass = new List<Assessment>();
                                    ass = ClinicalSummary.Assessment.Where(a => a.Chronic_Problem.ToUpper() == "BOTH" || a.Chronic_Problem.ToUpper() == "CHRONIC").ToList();
                                    if (ass.Count > 0)
                                    {
                                        sInnerXML += "<tr><td>Chronic Sickness exhibited by patient, 161901003 SNOMED-CT</td></tr>";
                                    }
                                }

                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[41];
                    XmlDocument docAllergyEntry = new XmlDocument();
                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\HealthConcern.xml"));
                    for (int i = 0; i < ClinicalSummary.HealthConcernProblemList.Count; i++)
                    {
                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldentry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);
                        elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[41];
                        XmlElement elemProcedureCode = null;
                        elemProcedureCode = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[41].ChildNodes[0].ChildNodes[4 + i].LastChild.ChildNodes[5].LastChild.ChildNodes[1];
                        if (ClinicalSummary.HealthConcernProblemList[i].ICD_9_Description != "")
                            elemProcedureCode.Attributes[0].Value = ClinicalSummary.HealthConcernProblemList[i].ICD_9_Description;//ICD_ID value
                        else
                            elemProcedureCode.Attributes[0].Value = "a2788ab0-2993-5ff9-aaae-8e120e83b31b";//ICD_ID value
                    }
                }
            }
            else if (hashCheckedList["chkHealthConcern"] != null && hashCheckedList["chkHealthConcern"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[41].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[41].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[41].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[41].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion

            #region Plan

            if (ClinicalSummary.OthertreatmentPlan != null && ClinicalSummary.OthertreatmentPlan.Count != 0 && hashCheckedList["chkTreatmentPlan"] != null && hashCheckedList["chkTreatmentPlan"].ToString().ToUpper() == "TRUE")
            {
                if (xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43].ChildNodes[0].ChildNodes[2].InnerText.ToUpper() == "TREATMENT PLAN")
                {
                    XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43].ChildNodes[0].ChildNodes[3];
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    string sInnerXML = string.Empty;

                    if (hashCheckedList["chkTreatmentPlan"].ToString().ToUpper() == "TRUE")
                    {
                        for (int i = 0; i < ClinicalSummary.OthertreatmentPlan.Count; i++)
                        {
                            if (i == 0)
                            {
                                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">";
                            }
                            if (ClinicalSummary.OthertreatmentPlan[i].Plan_For_Plan.Trim() != "")
                                sInnerXML += "<tr><td>" + ClinicalSummary.OthertreatmentPlan[i].Plan.Replace("&#xD;&#xA;", "").Replace("&#xA;", " ").Replace("&#xD;", " ").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("-Vitals", "").Replace("-Problem List", "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + " - " + ClinicalSummary.OthertreatmentPlan[i].Plan_For_Plan.Replace("&#xD;&#xA;", "").Replace("&#xA;", " ").Replace("&#xD;", " ").Replace("\r\n", "").Replace("\n", "").Replace("\r", "") + "</td><td>" + ClinicalSummary.Encounter[0].Date_of_Service.ToString("MMM dd,yyyy") + "</td></tr>";
                            else
                                sInnerXML += "<tr><td>" + ClinicalSummary.OthertreatmentPlan[i].Plan.Replace("&#xD;&#xA;", "").Replace("&#xA;", " ").Replace("&#xD;", " ").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("-Vitals", "").Replace("-Problem List", "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</td><td>" + ClinicalSummary.Encounter[0].Date_of_Service.ToString("MMM dd,yyyy") + "</td></tr>";
                            if (i == ClinicalSummary.OthertreatmentPlan.Count - 1)
                            {
                                sInnerXML += "</tbody>";
                            }
                        }
                    }
                    else
                    {
                        sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>" + "This Section is Omitted" + "</td></tr></tbody>";
                    }
                    xfrag.InnerXml = sInnerXML.Replace("&", "&amp;");
                    elemOld.LastChild.AppendChild(xfrag);

                    XmlElement elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43];
                    XmlDocument docAllergyEntry = new XmlDocument();
                    docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\TreatmentPlan.xml"));
                    //docAllergyEntry.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\TreatmentPlan.xml"));
                    for (int i = 0; i < ClinicalSummary.OthertreatmentPlan.Count; i++)
                    {

                        XmlDocumentFragment xfragAllergy = xmlDoc.CreateDocumentFragment();
                        xfragAllergy.InnerXml = docAllergyEntry.DocumentElement.InnerXml;
                        elemOldentry.LastChild.AppendChild(xfragAllergy);
                        xmlDoc.Save(sPrintFileName);
                        xmlDoc.Load(sPrintFileName);

                        elemOldentry = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43];

                        XmlElement elemPlanName = null;
                        XmlElement elemelemdate = null;

                        elemPlanName = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[4];
                        if (ClinicalSummary.OthertreatmentPlan[i].Plan_For_Plan.Trim() != "")
                            elemPlanName.Attributes[3].Value = ClinicalSummary.OthertreatmentPlan[i].Plan.Replace("&#xD;&#xA;", "").Replace("&#xA;", " ").Replace("&#xD;", " ").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("-Vitals", "").Replace("-Problem List", "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;") + " - " + ClinicalSummary.OthertreatmentPlan[i].Plan_For_Plan.Replace("&#xD;&#xA;", "").Replace("&#xA;", " ").Replace("&#xD;", " ").Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                        else
                            elemPlanName.Attributes[3].Value = ClinicalSummary.OthertreatmentPlan[i].Plan.Replace("&#xD;&#xA;", "").Replace("&#xA;", " ").Replace("&#xD;", " ").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("-Vitals", "").Replace("-Problem List", "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                        elemelemdate = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43].ChildNodes[0].ChildNodes[4 + i].ChildNodes[1].ChildNodes[7];
                        elemelemdate.Attributes[0].Value = ClinicalSummary.Encounter[0].Date_of_Service.ToString("yyyyMMdd");
                    }
                }
            }
            else if (hashCheckedList["chkTreatmentPlan"] != null && hashCheckedList["chkTreatmentPlan"].ToString().ToUpper() == "FALSE")
            {
                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43].ChildNodes[0].ChildNodes[3];
                XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                string sInnerXML = string.Empty;
                sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td>This Section is Omitted</td></tr></tbody>";
                xfrag.InnerXml = sInnerXML;
                elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }
            else
            {
                XmlElement xBookParticipant = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43].ChildNodes[0];
                XmlAttribute xAttribute = xBookParticipant.OwnerDocument.CreateAttribute("nullFlavor");
                xAttribute.Value = "NI";
                xBookParticipant.Attributes.Append(xAttribute);

                XmlElement elemOld = (XmlElement)xmlDoc.GetElementsByTagName("structuredBody")[0].ChildNodes[43].ChildNodes[0].ChildNodes[3];
                elemOld.RemoveAll();
                elemOld.InnerText = "No known data found";
                //XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                //string sInnerXML = string.Empty;
                //sInnerXML = "<tbody xmlns=" + '"' + "urn:hl7-org:v3" + '"' + ">" + "<tr><td></td></tr></tbody>";
                //xfrag.InnerXml = sInnerXML;
                //elemOld.LastChild.AppendChild(xfrag);
                xmlDoc.Save(sPrintFileName);
                xmlDoc.Load(sPrintFileName);
            }

            #endregion
            if (ClinicalSummary.MI.Trim() != "")
            {
                xmlReqNode = xmlDoc.GetElementsByTagName("given");
                xmlReqNode[1].InnerText = ClinicalSummary.MI;
            }
            else
            {
                xmlReqNode = xmlDoc.GetElementsByTagName("given");
                xmlReqNode[1].ParentNode.RemoveChild(xmlReqNode[1]);
                // xmlReqNode[1].RemoveAll();
            }

            xmlDoc.Save(sPrintFileName);

            //xmlDoc.DocumentElement.InnerText.Replace("<tbody xmlns='"+""+"'>","<tbody>");

            return xmlDoc;
        }

        //Immunization for NIST

        public string CreateImmunizationRegistryNISTQuery(Human hn, FillClinicalSummary ClinicalSummary, string sPrintFileName)
        {
            string sResult = string.Empty;
            string CityCode = string.Empty;
            string LocalNumber = string.Empty;
            string RaceIdentifier = string.Empty;
            string EthnicityIdentifier = string.Empty;
            string PublicitycodeIdentifier = string.Empty;
            string RelationshipIdentifier = string.Empty;
            string AdministrationIdentifier = string.Empty;
            string ImmunizationInformationIdentifier = string.Empty;
            string ProtectionstateIdentifier = string.Empty;
            string ObservationIdentifier = string.Empty;
            string EvidenceIdentifier = string.Empty;
            string RefusedReasonIdentifier = string.Empty;

            string Vfc = string.Empty;
            string VaccinetypeCode = string.Empty;
            string sMSH4 = System.Configuration.ConfigurationSettings.AppSettings["MSH4_NIST"];
            string sMSH6 = System.Configuration.ConfigurationSettings.AppSettings["MSH6_NIST"];
            string sMSH22 = System.Configuration.ConfigurationSettings.AppSettings["MSH22_NIST"];
            string sMSH7 = System.Configuration.ConfigurationSettings.AppSettings["MSH7_NIST"];


            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
            {
                iFieldLookupList = ClinicalSummary.lookupValues.Where(q => q.Field_Name == "RACE" || q.Field_Name == "ETHNICITY" || q.Field_Name == "PUBLICITY CODE" || q.Field_Name == "RELATIONSHIP" || q.Field_Name == "ROUTE OF ADMINISTRATION" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VFC STATUS" || q.Field_Name == "IMMUNIZATION_INFORMATION_SOURCE" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VACCINE TYPE" || q.Field_Name == "IMMUNIZATION PROTECTION TYPE" || q.Field_Name == "OBSERVATION" || q.Field_Name == "IMMUNIZATION EVIDENCE" || q.Field_Name.ToUpper() == "IMMUNIZATIONLOCATION" || q.Field_Name.ToUpper() == "IMMUNIZATION DOCUMENTATION").ToList();
            }
            if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
            {
                var RaceIdentifierlst = from d in iFieldLookupList where d.Value == hn.Race select d;
                if (RaceIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListRace = RaceIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListRace != null && iFieldLookupListRace.Count > 0)
                        RaceIdentifier = iFieldLookupListRace[0].Default_Value;
                }
                var EthnicityIdentifierlst = from d in iFieldLookupList where d.Value == hn.Ethnicity && d.Field_Name == "ETHNICITY" select d;
                if (EthnicityIdentifierlst != null && EthnicityIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListEthnicity = EthnicityIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListEthnicity.Count > 0)
                        EthnicityIdentifier = iFieldLookupListEthnicity[0].Default_Value;
                }
                var PublicitycodeIdentifierlst = from d in iFieldLookupList where d.Value == hn.Publicity_Code select d;
                if (PublicitycodeIdentifierlst != null && PublicitycodeIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListPublicity = PublicitycodeIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListPublicity.Count > 0)
                        PublicitycodeIdentifier = iFieldLookupListPublicity[0].Description;
                }
                var RelationshipIdentifierlst = from d in iFieldLookupList where d.Value == hn.Guarantor_Relationship select d;
                if (RelationshipIdentifierlst != null && RelationshipIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListRelationship = RelationshipIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListRelationship != null && iFieldLookupListRelationship.Count > 0)
                        RelationshipIdentifier = iFieldLookupListRelationship[0].Default_Value;
                }

            }



            sResult = "MSH|^~\\&|CapellaEHR^2.16.840.1.113883.3.72.5.40.1^ISO|" + sMSH4 + "^2.16.840.1.113883.3.72.5.40.2^ISO|" + sMSH6 + "^2.16.840.1.113883.3.72.5.40.3^ISO|" + sMSH7 + "|" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-0500" + "||QBP^Q11^QBP_Q11|IR" + DateTime.Now.ToString("yyyyMMddhhmmss") + "|P|2.5.1|||ER|AL|||||Z44^CDCPHINVS|" + sMSH22 + "^^^^^NIST-AA-IZ-1&2.16.840.1.113883.3.72.5.40.9&ISO^XX^^^100-6482|" + sMSH7 + "^^^^^NIST-AA-IZ-1&2.16.840.1.113883.3.72.5.40.9&ISO^XX^^^100-3322";

            if (hn.Home_Phone_No != String.Empty)
            {
                if (hn.Home_Phone_No.Contains(')'))
                {
                    string[] Phone_No = hn.Home_Phone_No.Split(')');
                    if (Phone_No.Length > 0)
                    {
                        if (hn.Home_Phone_No.Contains('('))
                        {
                            string[] Code_No = Phone_No[0].ToString().Split('(');
                            if (Code_No.Length > 0)
                            {
                                CityCode = Code_No[1].ToString();
                                if (Phone_No[1].Contains("-"))
                                    LocalNumber = Phone_No[1].Replace("-", "").Trim().ToString();
                            }
                        }
                    }
                }
                else
                {
                    if (hn.Home_Phone_No.Length == 10)
                    {
                        CityCode = hn.Home_Phone_No.Substring(0, 3);
                        LocalNumber = hn.Home_Phone_No.Substring(2, 8);
                    }
                }
            }
            string Mothers_Maiden_First_Name = string.Empty;
            string Mothers_Maiden_Last_Name = string.Empty;
            string Mothers_Maiden_Middle_Name = string.Empty;

            if (hn.Mothers_Maiden_Name != string.Empty)
            {
                if (hn.Mothers_Maiden_Name.Contains(","))
                {
                    string[] Mothers_Maiden_Name = hn.Mothers_Maiden_Name.Split(',');
                    if (Mothers_Maiden_Name.Length == 2)
                    {
                        Mothers_Maiden_First_Name = Mothers_Maiden_Name[0].ToString();
                        //Mothers_Maiden_Middle_Name = Mothers_Maiden_Name[1].ToString();
                        Mothers_Maiden_Last_Name = Mothers_Maiden_Name[1].ToString();
                    }
                }
            }

            //Human_id for nist tool
            string sDeathIndicator = string.Empty;
            if (hn.Patient_Status == "DECEASED")
                sDeathIndicator = "Y";
            else
                sDeathIndicator = "N";
            //if (EthnicityIdentifier == string.Empty)
            //{
            //    sResult = sResult + "\n" + "QPD|Z44^Request Evaluated History and Forecast^CDCPHINVS|IZ-1.1-2015|" + hn.Medical_Record_Number + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^MR|" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + hn.Mothers_Maiden_Name + "^^^^^^M|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P";
            //}
            //else
            //{
            //if (hn.EMail == string.Empty)
            //    sResult = sResult + "\n" + "QPD|Z44^Request Evaluated History and Forecast^CDCPHINVS|IZ-1.1-2015|" + hn.Id + "^^^ACUR^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + hn.Mothers_Maiden_Name + "^^^^^^M|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC" + "||" + hn.Birth_Indicator + "|" + hn.Birth_Order + "|||||" + sDeathIndicator;
            //else
            string sSex = string.Empty;
            if (hn.Sex != null && hn.Sex != "")
               sSex= hn.Sex.Substring(0, 1);
            

            if (hn.Mothers_Maiden_Name != string.Empty)
                sResult = sResult + "\n" + "QPD|Z44^Request Evaluated History and Forecast^CDCPHINVS|IZ-1.1-2015|" + hn.Medical_Record_Number + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^MR|" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + hn.Mothers_Maiden_Name + "^^^^^^M|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P";
            else
                sResult = sResult + "\n" + "QPD|Z44^Request Evaluated History and Forecast^CDCPHINVS|IZ-1.1-2015|" + hn.Medical_Record_Number + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^MR|" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L||" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P";

            if (hn.Home_Phone_No != string.Empty)
            {
                sResult = sResult + "|^PRN^PH^^^" + CityCode + "^" + LocalNumber;
            }
            if (hn.Birth_Indicator != string.Empty)
            {
                sResult = sResult + "|" + hn.Birth_Indicator + "|" + hn.Birth_Order;
            }
            // }

            sResult = sResult + "\n" + "RCP|I|1^RD&Records&HL70126";


            return sResult;
        }


        public string CreateImmunizationRegistryNIST(Human hn, PhysicianLibrary Phy, FillClinicalSummary ClinicalSummary, string sPrintFileName)
        {
            string sResult = string.Empty;
            string CityCode = string.Empty;
            string LocalNumber = string.Empty;
            string RaceIdentifier = string.Empty;
            string RaceValue = string.Empty;
            string EthnicityIdentifier = string.Empty;
            string EthnicityValue = string.Empty;
            string PublicitycodeIdentifier = string.Empty;
            string RelationshipIdentifier = string.Empty;
            var AdministrationIdentifier = string.Empty;
            string ImmunizationInformationIdentifier = string.Empty;
            string ProtectionstateIdentifier = string.Empty;
            string ObservationIdentifier = string.Empty;
            string EvidenceIdentifier = string.Empty;
            string RefusedReasonIdentifier = string.Empty;
            string PhyFirstName = string.Empty;
            string PhyLastName = string.Empty;
            string PhyMIName = string.Empty;
            string PhySuffix = string.Empty;
            string Vfc = string.Empty;
            string VaccinetypeCode = string.Empty;

            string sMSH4 = System.Configuration.ConfigurationSettings.AppSettings["MSH4_NIST"];
            string sMSH6 = System.Configuration.ConfigurationSettings.AppSettings["MSH6_NIST"];
            string sMSH22 = System.Configuration.ConfigurationSettings.AppSettings["MSH22_NIST"];
            string sMSH7 = System.Configuration.ConfigurationSettings.AppSettings["MSH7_NIST"];

            //string sMSH4 = System.Configuration.ConfigurationSettings.AppSettings["MSH4"];
            //string sMSH6 = System.Configuration.ConfigurationSettings.AppSettings["MSH6"];
            //string sMSH22 = System.Configuration.ConfigurationSettings.AppSettings["MSH22"];
            string sRXA11 = System.Configuration.ConfigurationSettings.AppSettings["RXA11_NIST"];


            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
            {
                iFieldLookupList = ClinicalSummary.lookupValues.Where(q => q.Field_Name == "RACE" || q.Field_Name == "ETHNICITY" || q.Field_Name == "PUBLICITY CODE" || q.Field_Name == "RELATIONSHIP" || q.Field_Name == "ROUTE OF ADMINISTRATION" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VFC STATUS" || q.Field_Name == "IMMUNIZATION_INFORMATION_SOURCE" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VACCINE TYPE" || q.Field_Name == "IMMUNIZATION PROTECTION TYPE" || q.Field_Name == "OBSERVATION" || q.Field_Name == "IMMUNIZATION EVIDENCE" || q.Field_Name.ToUpper() == "IMMUNIZATIONLOCATION" || q.Field_Name.ToUpper() == "IMMUNIZATION DOCUMENTATION").ToList();
            }
            if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
            {//Cap - 1716,CAP-1719
                //var RaceIdentifierlst = from d in iFieldLookupList where d.Value == hn.Race select d;
                var RaceIdentifierlst = from d in iFieldLookupList where d.Value == hn.Race && d.Field_Name == "RACE" select d;
                if (RaceIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListRace = RaceIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListRace != null && iFieldLookupListRace.Count > 0)
                    {
                        RaceIdentifier = iFieldLookupListRace[0].Default_Value;
                        RaceValue = iFieldLookupListRace[0].Doc_Type;
                    }
                }
                var EthnicityIdentifierlst = from d in iFieldLookupList where d.Value == hn.Ethnicity && d.Field_Name == "ETHNICITY" select d;
                if (EthnicityIdentifierlst != null && EthnicityIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListEthnicity = EthnicityIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListEthnicity.Count > 0)
                    {
                        EthnicityIdentifier = iFieldLookupListEthnicity[0].Default_Value;
                        EthnicityValue = iFieldLookupListEthnicity[0].Doc_Type;
                    }
                }
                var PublicitycodeIdentifierlst = from d in iFieldLookupList where d.Value == hn.Publicity_Code select d;
                if (PublicitycodeIdentifierlst != null && PublicitycodeIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListPublicity = PublicitycodeIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListPublicity.Count > 0)
                        PublicitycodeIdentifier = iFieldLookupListPublicity[0].Description;
                }
                var RelationshipIdentifierlst = from d in iFieldLookupList where d.Value == hn.Guarantor_Relationship select d;
                if (RelationshipIdentifierlst != null && RelationshipIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListRelationship = RelationshipIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListRelationship != null && iFieldLookupListRelationship.Count > 0)
                        RelationshipIdentifier = iFieldLookupListRelationship[0].Default_Value;
                }

                //IList<StaticLookup> iFieldLookupListEthnicity = ClinicalSummary.lookupValues.Where(q => q.Field_Name == "ETHNICITY").ToList();
            }

            //commented for immunization submission Console app 12.9.2017

            //if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
            // {
            //     IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.Id == ClientSession.PhysicianId).ToList();
            //     if (iPhyList != null && iPhyList.Count > 0)
            //     {
            //         PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
            //         PhyLastName = iPhyList[0].PhyLastName;
            //         PhyMIName = iPhyList[0].PhyMiddleName;
            //     }
            // }
            string sPhy_ID = string.Empty;
            if (Phy != null)
            {
                if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
                {
                    IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.Id == Phy.Id).ToList();
                    if (iPhyList != null && iPhyList.Count > 0)
                    {
                        PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
                        PhyLastName = iPhyList[0].PhyLastName;
                        PhyMIName = iPhyList[0].PhyMiddleName;
                        sPhy_ID = iPhyList[0].Id.ToString();
                        PhySuffix = iPhyList[0].PhySuffix;
                    }


                }

            }

            //sResult = "MSH|^~\\&|Capella EHR|X68||CA Immunization Reg|" + DateTime.Now.ToString("yyyyMMddHHMM") + "||VXU^V04^VXU_V04|NIST-IZ-001.00|P|2.5.1|||AL|ER";
            //sResult = "MSH|^~\\&|Capella EHR|" + sMSH4 + "|" + sMSH6 + "|" + sMSH6 + "|" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-0800" + "||VXU^V04^VXU_V04|IR" + DateTime.Now.ToString("yyyyMMddhhmmss") + "|P|2.5.1|||ER|AL|||||Z22^CDCPHINVS|" + sMSH22 + "|" + sMSH22;


            sResult = "MSH|^~\\&|CapellaEHR^2.16.840.1.113883.3.72.5.40.1^ISO|" + sMSH4 + "^2.16.840.1.113883.3.72.5.40.2^ISO|" + sMSH6 + "^2.16.840.1.113883.3.72.5.40.3^ISO|" + sMSH7 + "^2.16.840.1.113883.3.72.5.40.4^ISO|" + DateTime.Now.ToString("yyyyMMddhhmmss") + "-0500" + "||VXU^V04^VXU_V04|IR" + DateTime.Now.ToString("yyyyMMddhhmmss") + "|P|2.5.1|||ER|AL|||||Z22^CDCPHINVS|" + sMSH22 + "^^^^^NIST-AA-IZ-1&2.16.840.1.113883.3.72.5.40.9&ISO^XX^^^100-6482|" + sMSH7 + "^^^^^NIST-AA-IZ-1&2.16.840.1.113883.3.72.5.40.9&ISO^XX^^^100-3322";



            if (hn.Home_Phone_No != String.Empty)
            {
                if (hn.Home_Phone_No.Contains(')'))
                {
                    string[] Phone_No = hn.Home_Phone_No.Split(')');
                    if (Phone_No.Length > 0)
                    {
                        if (hn.Home_Phone_No.Contains('('))
                        {
                            string[] Code_No = Phone_No[0].ToString().Split('(');
                            if (Code_No.Length > 0)
                            {
                                CityCode = Code_No[1].ToString();
                                if (Phone_No[1].Contains("-"))
                                    LocalNumber = Phone_No[1].Replace("-", "").Trim().ToString();
                            }
                        }
                    }
                }
                else
                {
                    if (hn.Home_Phone_No.Length == 10)
                    {
                        CityCode = hn.Home_Phone_No.Substring(0, 3);
                        LocalNumber = hn.Home_Phone_No.Substring(2, 8);
                    }
                }
            }
            string Mothers_Maiden_First_Name = string.Empty;
            string Mothers_Maiden_Last_Name = string.Empty;
            string Mothers_Maiden_Middle_Name = string.Empty;

            if (hn.Mothers_Maiden_Name != string.Empty)
            {
                if (hn.Mothers_Maiden_Name.Contains(","))
                {
                    string[] Mothers_Maiden_Name = hn.Mothers_Maiden_Name.Split(',');
                    if (Mothers_Maiden_Name.Length == 2)
                    {
                        Mothers_Maiden_First_Name = Mothers_Maiden_Name[0].ToString();
                        //Mothers_Maiden_Middle_Name = Mothers_Maiden_Name[1].ToString();
                        Mothers_Maiden_Last_Name = Mothers_Maiden_Name[1].ToString();
                    }
                }
            }
            //if (hn.Patient_Account_External == string.Empty)
            //{
            //    if (EthnicityIdentifier == string.Empty)
            //    {
            //        sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||";
            //    }
            //    else
            //    {
            //        sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC";
            //    }
            //}
            //else
            //{
            //    if (EthnicityIdentifier == string.Empty)
            //    {
            //        sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^MR~" + hn.Patient_Account_External + "^^^MAA^SS||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "^~^NET^^" + hn.EMail + "|||||||||";
            //    }
            //    else
            //    {
            //        sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^MR~" + hn.Patient_Account_External + "^^^MAA^SS||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "^~^NET^^" + hn.EMail + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC";
            //    }
            //}


            //Human_id for nist tool
            string sDeathIndicator = string.Empty;
            if (hn.Patient_Status == "DECEASED")
                sDeathIndicator = "Y";
            else
                sDeathIndicator = "N";

            string sSex = string.Empty;
            if (hn.Sex != null && hn.Sex != "")
                sSex = hn.Sex.Substring(0, 1);

           // if (EthnicityIdentifier == string.Empty)
           if(EthnicityValue==string.Empty)
            {
                //CAP-1716,CAP-1719 - Immunization Submission to CAIR Records - To include PI(human_id ) for all  submissions instead of MRN Number 
                //if (hn.Mothers_Maiden_Name != string.Empty)
                //    sResult = sResult + "\n" + "PID|1||" + hn.Medical_Record_Number + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + hn.Mothers_Maiden_Name + "^^^^^^M|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||";
                //else
                //    sResult = sResult + "\n" + "PID|1||" + hn.Medical_Record_Number + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L||" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||";
                if (hn.Mothers_Maiden_Name != string.Empty)
                    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + hn.Mothers_Maiden_Name + "^^^^^^M|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||";
                else
                    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L||" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||";
            }
            else
            {
                if (hn.EMail == string.Empty)
                {
                    //CAP-1716,CAP-1719 - Immunization Submission to CAIR Records - To include PI(human_id ) for all  submissions instead of MRN Number 
                    //if (hn.Mothers_Maiden_Name != string.Empty)
                    //    sResult = sResult + "\n" + "PID|1||" + hn.Medical_Record_Number + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + hn.Mothers_Maiden_Name + "^^^^^^M|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC" + "||" + hn.Birth_Indicator + "|" + hn.Birth_Order + "|||||" + sDeathIndicator;
                    //else
                    //    sResult = sResult + "\n" + "PID|1||" + hn.Medical_Record_Number + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L||" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC" + "||" + hn.Birth_Indicator + "|" + hn.Birth_Order + "|||||" + sDeathIndicator;
                    if (hn.Mothers_Maiden_Name != string.Empty)
                        sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + hn.Mothers_Maiden_Name + "^^^^^^M|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||" + EthnicityIdentifier + "^" + EthnicityValue + "^CDCREC" + "||" + hn.Birth_Indicator + "|" + hn.Birth_Order + "|||||" + sDeathIndicator;
                    else
                        sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L||" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||" + EthnicityIdentifier + "^" + EthnicityValue + "^CDCREC" + "||" + hn.Birth_Indicator + "|" + hn.Birth_Order + "|||||" + sDeathIndicator;
                }
                else {
                    //CAP-1716,CAP-1719 - Immunization Submission to CAIR Records - To include PI(human_id ) for all  submissions instead of MRN Number 
                    //if (hn.Mothers_Maiden_Name != string.Empty)
                    //    sResult = sResult + "\n" + "PID|1||" + hn.Medical_Record_Number + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + hn.Mothers_Maiden_Name + "^^^^^^M|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "~^NET^^" + hn.EMail + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC" + "||" + hn.Birth_Indicator + "|" + hn.Birth_Order + "|||||" + sDeathIndicator;
                    //else
                    //    sResult = sResult + "\n" + "PID|1||" + hn.Medical_Record_Number + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L||" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "~^NET^^" + hn.EMail + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC" + "||" + hn.Birth_Indicator + "|" + hn.Birth_Order + "|||||" + sDeathIndicator;
                    if (hn.Mothers_Maiden_Name != string.Empty)
                        sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + hn.Mothers_Maiden_Name + "^^^^^^M|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "~^NET^^" + hn.EMail + "|||||||||" + EthnicityIdentifier + "^" + EthnicityValue + "^CDCREC" + "||" + hn.Birth_Indicator + "|" + hn.Birth_Order + "|||||" + sDeathIndicator;
                    else
                        sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^NIST-MPI-1&2.16.840.1.113883.3.72.5.40.5&ISO^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L||" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "~^NET^^" + hn.EMail + "|||||||||" + EthnicityIdentifier + "^" + EthnicityValue + "^CDCREC" + "||" + hn.Birth_Indicator + "|" + hn.Birth_Order + "|||||" + sDeathIndicator;
                }
            }

            DateTime sImmu_Date = (hn.Modified_Date_And_Time.ToString("yyyy-MM-dd mm:hh:ss") == "0001-01-01 00:00:00" ? hn.Modified_Date_And_Time : hn.Created_Date_And_Time);
            if (PublicitycodeIdentifier != string.Empty && hn.Immunization_Registry_Status != string.Empty)
            {
                sResult = sResult + "\n" + "PD1|||||||||||" + PublicitycodeIdentifier + "^" + hn.Publicity_Code + "^HL70215|" + hn.Data_Sharing_Preference + "|" + sImmu_Date.ToString("yyyyMMdd") + "|||" + hn.Immunization_Registry_Status.Substring(0, 1) + "|" + sImmu_Date.ToString("yyyyMMdd") + "|" + sImmu_Date.ToString("yyyyMMdd");
            }
            else
            {
                sResult = sResult + "\n" + "PD1||||||||||||N||||||";
            }
            //else if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count > 0)
            //{
            //    for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
            //    {
            //        if (ClinicalSummary.immunhistoryList[i].Protection_State != string.Empty)
            //        {
            //            var ProtectionStateIdentifier = from d in iFieldLookupList where d.Value == ClinicalSummary.immunhistoryList[i].Protection_State select d;
            //            if (ProtectionStateIdentifier != null && ProtectionStateIdentifier.Count() > 0)
            //            {
            //                IList<StaticLookup> iFieldLookupListProtection = ProtectionStateIdentifier.ToList<StaticLookup>();
            //                if (iFieldLookupListProtection.Count > 0)
            //                    ProtectionstateIdentifier = iFieldLookupListProtection[0].Description;

            //                sResult = sResult + "\n" + "PD1||||||||||||" + ProtectionstateIdentifier + "|" + ClinicalSummary.immunhistoryList[i].Created_Date_And_Time.ToString("yyyyMMdd");
            //            }

            //        }
            //        else
            //        {
            //            //sResult = sResult + "\n" + "PD1|||||||||||" + PublicitycodeIdentifier + "^" + hn.Publicity_Code + "^HL70215|||||" + hn.Immunization_Registry_Status.Substring(0, 1) + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd") + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd");
            //            //break;
            //        }
            //    }
            //}

            if (hn.Guarantor_Relationship != "Self")
            {
                int sval = 1;
                if ((hn.Guarantor_Last_Name != string.Empty || hn.Guarantor_First_Name != string.Empty) && (hn.Guarantor_MI != string.Empty))
                    sResult = sResult + "\n" + "NK1|" + sval + "|" + hn.Guarantor_Last_Name + "^" + hn.Guarantor_First_Name + "^" + hn.Guarantor_MI + "^^^^L|" + RelationshipIdentifier + "^" + hn.Guarantor_Relationship + "^HL70063|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P|^PRN^PH^^^" + CityCode + "^" + LocalNumber + "";
                else if (hn.Guarantor_Last_Name != string.Empty || hn.Guarantor_First_Name != string.Empty)
                    sResult = sResult + "\n" + "NK1|" + sval + "|" + hn.Guarantor_Last_Name + "^" + hn.Guarantor_First_Name + "^^^^^L|" + RelationshipIdentifier + "^" + hn.Guarantor_Relationship + "^HL70063|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^P|^PRN^PH^^^" + CityCode + "^" + LocalNumber + "";

                if (ClinicalSummary.PatGuarantor.Count > 0)
                {
                    for (int i = 0; i < ClinicalSummary.PatGuarantor.Count; i++)
                    {
                        IList<Human> objHuman = (from h in ClinicalSummary.HumanList where h.Id == Convert.ToUInt32(ClinicalSummary.PatGuarantor[i].Guarantor_Human_ID) select h).ToList();
                        if (objHuman.Count > 0)
                        {

                            if (objHuman[0].Home_Phone_No != String.Empty)
                            {
                                if (objHuman[0].Home_Phone_No.Contains(')'))
                                {
                                    string[] Phone_No = objHuman[0].Home_Phone_No.Split(')');
                                    if (Phone_No.Length > 0)
                                    {
                                        if (objHuman[0].Home_Phone_No.Contains('('))
                                        {
                                            string[] Code_No = Phone_No[0].ToString().Split('(');
                                            if (Code_No.Length > 0)
                                            {
                                                CityCode = Code_No[1].ToString();
                                                if (Phone_No[1].Contains("-"))
                                                    LocalNumber = Phone_No[1].Replace("-", "").Trim().ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (objHuman[0].Home_Phone_No.Length == 10)
                                    {
                                        CityCode = objHuman[0].Home_Phone_No.Substring(0, 3);
                                        LocalNumber = objHuman[0].Home_Phone_No.Substring(2, 8);
                                    }
                                }
                            }

                            var RelationshipIdentifierlst = from d in iFieldLookupList where d.Value == ClinicalSummary.PatGuarantor[i].Relationship select d;
                            if (RelationshipIdentifierlst != null && RelationshipIdentifierlst.Count() > 0)
                            {
                                IList<StaticLookup> iFieldLookupListRelationship = RelationshipIdentifierlst.ToList<StaticLookup>();
                                if (iFieldLookupListRelationship != null && iFieldLookupListRelationship.Count > 0)
                                    RelationshipIdentifier = iFieldLookupListRelationship[0].Default_Value;
                            }
                            sval = sval + 1;
                            if ((objHuman[0].Last_Name != string.Empty || hn.Guarantor_First_Name != string.Empty) && (hn.Guarantor_MI != string.Empty))
                                sResult = sResult + "\n" + "NK1|" + sval + "|" + objHuman[0].Last_Name + "^" + objHuman[0].First_Name + "^" + objHuman[0].MI + "^^^^L|" + RelationshipIdentifier + "^" + ClinicalSummary.PatGuarantor[i].Relationship + "^HL70063|" + objHuman[0].Street_Address1 + "^^" + objHuman[0].City + "^" + objHuman[0].State + "^" + objHuman[0].ZipCode + "^USA^P|^PRN^CP^^^" + CityCode + "^" + LocalNumber + "";
                            else if (hn.Guarantor_Last_Name != string.Empty || hn.Guarantor_First_Name != string.Empty)
                                sResult = sResult + "\n" + "NK1|" + sval + "|" + objHuman[0].Last_Name + "^" + objHuman[0].First_Name + "^^^^^L|" + RelationshipIdentifier + "^" + ClinicalSummary.PatGuarantor[i].Relationship + "^HL70063|" + objHuman[0].Street_Address1 + "^^" + objHuman[0].City + "^" + objHuman[0].State + "^" + objHuman[0].ZipCode + "^USA^P|^PRN^CP^^^" + CityCode + "^" + LocalNumber + "";
                        }
                    }
                }

            }
            IList<string> ilstIdentifier = new List<string>() { "C28161-Intra Muscular", "C38238-Intra dermal", "C38276-Intravenous", "C38284-Nasal", "C38288-Oral", "C38299-Subcutaneous", "C38305-Transdermal", "C38676-Percutaneous" };



            //if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count > 0)
            //{
            //if (ClinicalSummary.ImmunizationList.Count < 2)
            //{
            //if (ClinicalSummary.ImmunizationList.Count != 0)
            //{
            //for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
            //{
            //    if (ClinicalSummary.ImmunizationList.Any(a => a.Given_Date.ToString("dd-MMM-yyyy").ToUpper() != (ClinicalSummary.immunhistoryList[i].Administered_Date) && a.Procedure_Code != ClinicalSummary.immunhistoryList[i].Procedure_Code && a.Immunization_Description != ClinicalSummary.immunhistoryList[i].Immunization_Description && a.Encounter_Id != ClinicalSummary.immunhistoryList[i].Encounter_ID))
            //    {
            //        sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.immunhistoryList[i].Id + "^CAA";
            //    }
            //}
            //}
            //else if (ClinicalSummary.immunhistoryList.Count > 0)
            //{
            //    for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
            //    {
            //        sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.immunhistoryList[i].Id + "^CAA";
            //    }
            //}
            //}

            //}

            //if (ClinicalSummary.ImmunizationList != null && ClinicalSummary.ImmunizationList.Count == 0)
            //{
            //    if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count > 0)
            //    {
            //        for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
            //        {
            //             sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.immunhistoryList[i].Id+ "^CAA|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CA-AA-1^L^^^PRN||" +"|||||"+sMSH6+"^"+sMSH6+"^HL70362|";

            //            sResult = sResult + "\n" + "RXA|" + "0|1|" + Convert.ToDateTime(ClinicalSummary.immunhistoryList[i].Administered_Date).ToString("yyyyMMdd") + "||"
            //                    + ClinicalSummary.immunhistoryList[i].CVX_Code + "^" +
            //                    ClinicalSummary.immunhistoryList[i].Immunization_Description + "^CVX|999|||" + "01" + "^" +ClinicalSummary.immunhistoryList[i].Notes + "^NIP001|||||||||||CP|A";
            //        }
            //    }
            //}
            if (ClinicalSummary.ImmunizationList != null && ClinicalSummary.ImmunizationList.Count > 0)
            {
                //if (ClinicalSummary.ImmunizationList.Count < 2)
                //{
                for (int i = 0; i < ClinicalSummary.ImmunizationList.Count; i++)
                {
                    //if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() == "Y")
                    //{
                    //    continue;
                    //}
                    //if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() == "Y")
                    //{
                    //    sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^CDC";
                    //}
                    //else if (ClinicalSummary.ImmunizationList[i].Immunization_Evidence != string.Empty)
                    //{
                    //    sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^CDC";
                    //}
                    //else
                    //{
                    //    sResult = sResult + "\n" + "ORC|RE|" + ClinicalSummary.ImmunizationList[i].Id+"^CAA|"+ ClinicalSummary.ImmunizationList[i].Id+ "^CAA|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CA-AA-1^L^^^PRN||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName +"^" + PhyMIName + "^^^^^CA-AA-1^L^^^PRN||" +"|||||"+sMSH6+"^"+sMSH6+"^HL70362";
                    //}




                    if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() != "Y")
                        sResult = sResult + "\n" + "ORC|RE|" + ClinicalSummary.ImmunizationList[i].Id + "^NIST-AA-IZ-2AA-IZ-2^2.16.840.1.113883.3.72.5.40.10^ISO|" + ClinicalSummary.ImmunizationList[i].Id + "^NIST-AA-IZ-2AA-IZ-2^2.16.840.1.113883.3.72.5.40.10^ISO|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^NIST-PI-1&2.16.840.1.113883.3.72.5.40.7&ISO^L^^^PRN||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^NIST-PI-1&2.16.840.1.113883.3.72.5.40.7&ISO^L^^^PRN" + "|||||" + sMSH6 + "^" + sMSH6 + "^HL70362|";
                    else
                        //sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.immunhistoryList[i].Id+ "^CAA|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CA-AA-1^L^^^PRN||" +"|||||"+sMSH6+"^"+sMSH6+"^HL70362|";
                        //sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^NIST-AA-IZ-2|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^NIST-PI-1^L^^^PRN||" + "|||||" + sMSH6 + "^" + sMSH6 + "^HL70362|";
                        sResult = sResult + "\n" + "ORC|RE||" + "9999" + "^NIST-AA-IZ-2^2.16.840.1.113883.3.72.5.40.10^ISO|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^NIST-PI-1&2.16.840.1.113883.3.72.5.40.7&ISO^L^^^PRN||" + "|||||" + sMSH6 + "^" + sMSH6 + "^HL70362|";
                    //sResult = sResult + "\n" + "ORC|RE||" + "9999" + "^CAA|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CA-AA-1^L^^^PRN||" + "|||||" + sMSH6 + "^" + sMSH6 + "^HL70362|";







                    //RXA

                    string phyID = string.Empty;
                    //PhyFirstName = string.Empty;
                    //PhyLastName = string.Empty;
                    //PhyMIName = string.Empty;
                    //if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
                    //{
                    //    //if (ClinicalSummary.ImmunizationList[i].Given_By.Contains(","))
                    //    if (ClinicalSummary.ImmunizationList[i].Given_By != string.Empty)
                    //    {
                    //        string phyLName = string.Empty;
                    //        string phyFName = string.Empty;
                    //        string phyMName = string.Empty;
                    //        string[] phyName = ClinicalSummary.ImmunizationList[i].Given_By.Split(',');
                    //        if (phyName.Length > 0)
                    //        {
                    //            phyLName = phyName[0].ToString();
                    //            if (phyName.Length > 1)
                    //                phyFName = phyName[1].ToString();
                    //            if (phyName.Length > 2)
                    //                phyMName = phyName[2].ToString();
                    //            IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.PhyLastName == phyLName && q.PhyFirstName == phyFName).ToList();
                    //            if (iPhyList.Count > 0)
                    //            {
                    //                PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
                    //                PhyLastName = iPhyList[0].PhyLastName;
                    //                PhyMIName = iPhyList[0].PhyMiddleName;
                    //                phyID = iPhyList[0].Id.ToString();
                    //            }
                    //            else
                    //            {
                    //                PhyFirstName = phyFName;
                    //                PhyLastName = phyLName;
                    //                PhyMIName = phyMName;
                    //                phyID = "0";
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        phyID = sPhy_ID;
                    //    }
                    //}
                    phyID = sPhy_ID;

                    DateTime AdminDate = DateTime.MinValue;
                    if (ClinicalSummary.ImmunizationList[i].Given_Date.ToString() != string.Empty)
                        AdminDate = Convert.ToDateTime(ClinicalSummary.ImmunizationList[i].Given_Date);

                    string AdminAmount = string.Empty;
                    string VaccineCode = string.Empty;
                    IList<VaccineManufacturerCodes> VaccineCodes = new List<VaccineManufacturerCodes>();
                    IList<VaccineManufacturerCodes> VaccineCodeforManufacturer = new List<VaccineManufacturerCodes>();
                    if (ClinicalSummary != null)
                        VaccineCodes = ClinicalSummary.VaccineCodes;
                    if (VaccineCodes != null && VaccineCodes.Count > 0)
                    {
                        var CodeList = from d in VaccineCodes where d.Manufacturer_Name == ClinicalSummary.ImmunizationList[i].Manufacturer select d;
                        if (CodeList != null && CodeList.Count() > 0)
                        {
                            VaccineCodeforManufacturer = CodeList.ToList<VaccineManufacturerCodes>();
                            if (VaccineCodeforManufacturer != null && VaccineCodeforManufacturer.Count > 0)
                                VaccineCode = VaccineCodeforManufacturer[0].MVX_Code;
                        }

                    }

                    // AdminAmount = "999";

                    if (PhyLastName != string.Empty || PhyLastName != string.Empty || PhyLastName != string.Empty)
                    {
                        //RXA|0|1|20171207||00006-4171-00^ProQuad^NDC|0.5|mL^mL^UCUM||00^New Record^NIP001|4430^ LILY^JACKSON^SUZANNE^^^^^CA-AA-1^L^^^PRN|^^^NIST-Clinic-1||||407453|00010101|MSD^Merck and Co., Inc.^MVX|||CP|A
                        // RXA|0|1|20171207||00006-4171-00^ProQuad^NDC|0.5|mL^mL^UCUM||00^New Record^NIP001|4430^ LILY^JACKSON^SUZANNE^^^^^CA-AA-1^L^^^PRN|^^^NIST-Clinic-1||||407453|00010101|MSD^Merck and Co., Inc.^MVX|||CP|A
                        string sCurd = string.Empty;
                       
                        if (ClinicalSummary.ImmunizationList[i].Modified_Date_And_Time.ToString("yyyy-MM-dd hh:mm:ss") != "0001-01-01 12:00:00" && ClinicalSummary.ImmunizationList[i].Is_Deleted == "N")
                            sCurd = "U";
                        else if (ClinicalSummary.ImmunizationList[i].Modified_Date_And_Time.ToString("yyyy-MM-dd hh:mm:ss") != "0001-01-01 12:00:00" && ClinicalSummary.ImmunizationList[i].Is_Deleted == "Y")
                            sCurd = "D";
                        else
                            sCurd = "A";
                        if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() != "Y")
                        {
                            sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                                   + ClinicalSummary.ImmunizationList[i].NDC + "^" +
                                   ClinicalSummary.ImmunizationList[i].Immunization_Description + "^NDC|" + ClinicalSummary.ImmunizationList[i].Administered_Amount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier
                                   + "^UCUM||" + "00" + "^" + ClinicalSummary.ImmunizationList[i].Notes + "^NIP001|" +
                                   "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^NIST-PI-1&2.16.840.1.113883.3.72.5.40.7&ISO^L^^^PRN" + "|^^^" + sRXA11 + "&2.16.840.1.113883.3.72.5.40.12&ISO" + "||||" +
                                   ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                                   + "|" + VaccineCode
                                   + "^" +
                                   ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|" + sCurd;

                            //RXA|0|1|20171207||58160-0820-01^Hepatitis b vaccine ped to 17 yrs^NDC|0.50|mL^mL^UCUM||00^New Record^NIP001|4430^ LILY^JACKSON^SUZANNE^^^^^NIST-PI-1&2.16.840.1.113883.3.72.5.40.7&ISO^L^^^PRN|^^^DE‐000001&2.16.840.1.113883.3.72.5.40.12&ISO||||797397|20181228|SKB^GlaxoSmithKline^MVX|||CP|A

                        }
                        else
                        {
                            string sCompletionstatus = string.Empty;
                            if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused == "Y")
                                sCompletionstatus = "RE";
                            else
                                sCompletionstatus = "CP";
                            //sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                            //  + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                            //  ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|999|||01^" + ClinicalSummary.ImmunizationList[i].Refused_Administration + "^NIP001|||||||||||" +
                            //  ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||"+sCompletionstatus+"|"+sCurd;


                            sResult = sResult + "\n" + "RXA|" + "0|1|" + Convert.ToDateTime(ClinicalSummary.ImmunizationList[i].Given_Date).ToString("yyyyMMdd") + "||"
                            + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                            ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|999||||||||||||" + "00" + "^" + ClinicalSummary.ImmunizationList[i].Refused_Administration + "^NIP002||" + sCompletionstatus + "|" + sCurd;
                        }



                        //if (ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier != null && ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier.Trim() != string.Empty && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source != null && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source.Trim() != string.Empty)
                        //{
                        //    sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                        //        + ClinicalSummary.ImmunizationList[i].NDC + "^" +
                        //        ClinicalSummary.ImmunizationList[i].Immunization_Description + "^NDC|" + ClinicalSummary.ImmunizationList[i].Administered_Amount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier
                        //        + "^UCUM||" + "00" + "^" + ClinicalSummary.ImmunizationList[i].Notes + "^NIP001|" +
                        //        "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CA-AA-1^L^^^PRN"+ "|^^^" + sRXA11 + "||||" +
                        //        ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                        //        + "|" + VaccineCode
                        //        + "^" +
                        //        ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|" + sCurd;
                        //}
                        //else if (ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier != null && ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier.Trim() == string.Empty && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source != null && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source.Trim() != string.Empty)
                        //{
                        //    sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                        //      + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                        //      ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Immunization_Information_Source + "^NIP001|" +
                        //      "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CMS^^^^NPI^^^^^^^^" + PhySuffix + "|^^^" + sRXA11 + "||||" +
                        //      ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                        //      + "|" + VaccineCode
                        //      + "^" +
                        //      ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";

                        //}
                        //else if (ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier != null && ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier.Trim() != string.Empty && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source != null && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source.Trim() == string.Empty)
                        //{
                        //    sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                        //  + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                        //  ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit
                        //  + "^UCUM|||" +
                        //  "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CMS^^^^NPI^^^^^^^^" + PhySuffix + "|^^^" + sRXA11 + "||||" +
                        //  ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                        //  + "|" + VaccineCode
                        //  + "^" +
                        //  ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";

                        //}
                        //else if (ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier != null && ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier.Trim() == string.Empty && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source != null && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source.Trim() == string.Empty)
                        //{
                        //    sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                        //      + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                        //      ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "||||" +
                        //      "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CMS^^^^NPI^^^^^^^^" + PhySuffix + "|^^^" + sRXA11 + "||||" +
                        //      ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                        //      + "|" + VaccineCode
                        //      + "^" +
                        //      ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";
                        //}



                    }

                    //RXR
                    VaccineInfoStatementProcedureManager objVaccineInfoStatementManager = new VaccineInfoStatementProcedureManager();
                    IList<VaccineInfoStatement> VISList = objVaccineInfoStatementManager.GetVaccineInfoStatementForSelectedprocedure(ClinicalSummary.ImmunizationList[i].Procedure_Code);
                    string LocationIdentifier = string.Empty;
                    if (ClinicalSummary.ImmunizationList[i].Location != string.Empty)
                        LocationIdentifier = iFieldLookupList.Where(a => a.Value.ToUpper() == ClinicalSummary.ImmunizationList[i].Location.ToUpper()).ToList<StaticLookup>()[0].Description;





                    //sResult = sResult + "\n" + "RXR|" + AdministrationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Route_of_Administration + "^NCIT|" + LocationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Location + "^HL70163";



                    if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() != "Y")
                    {
                        if (ClinicalSummary.ImmunizationList[i].Route_of_Administration != "")
                            //Cap - 1817
                            //AdministrationIdentifier = (from p in ilstIdentifier where p.ToString().Split('-')[1] == ClinicalSummary.ImmunizationList[i].Route_of_Administration select p.ToString().Split('-')[0]).FirstOrDefault().ToString();
                            AdministrationIdentifier = (from p in ilstIdentifier where p.ToString().Split('-')[1] == ClinicalSummary.ImmunizationList[i].Route_of_Administration select p).FirstOrDefault();
                        if(AdministrationIdentifier != null)
                        {
                            AdministrationIdentifier = AdministrationIdentifier.Split('-')[0].ToString();
                        }

                        var VfcIdentifierlst = from d in iFieldLookupList where d.Field_Name == "VFC STATUS" && d.Value == ClinicalSummary.ImmunizationList[i].Vfc select d;
                        if (VfcIdentifierlst.Count() > 0)
                        {
                            IList<StaticLookup> iFieldLookupListVfc = VfcIdentifierlst.ToList<StaticLookup>();
                            if (iFieldLookupListVfc != null && iFieldLookupListVfc.Count > 0)
                                Vfc = iFieldLookupListVfc[0].Description;
                        }

                        if (ClinicalSummary.ImmunizationList[i].Route_of_Administration == "Oral")
                            sResult = sResult + "\n" + "RXR|" + AdministrationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Route_of_Administration + "^NCIT|";
                        else
                            sResult = sResult + "\n" + "RXR|" + AdministrationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Route_of_Administration + "^NCIT|" + LocationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Location + "^HL70163";

                        string sSource = string.Empty;
                        if (ClinicalSummary.ImmunizationList[i].Immunization_Source == "Patient supplied")
                            sSource = "PHC70^Private";
                        else
                            sSource = "VXC50^Public";

                        string sEligibility = string.Empty;
                        if (ClinicalSummary.ImmunizationList[i].Eligibility_Captured.ToUpper() == "IMMUNIZATION LEVEL")
                            sEligibility = "VXC40^";
                        else
                            sEligibility = "VXC41^";

                        sResult = sResult + "\n" + "OBX|1|CE|30963-3^Vaccine Funding Source^LN|1|" + sSource + "^CDCPHINVS||||||F|||" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd");
                        sResult = sResult + "\n" + "OBX|2|CE|64994-7^Vaccine funding program eligibility category^LN^|2|" + Vfc + "^" + ClinicalSummary.ImmunizationList[i].Vfc + "^HL70064||||||F|||" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "|||" + sEligibility + ClinicalSummary.ImmunizationList[i].Eligibility_Captured + "^CDCPHINVS";


                        string[] sArrDocuments = ClinicalSummary.ImmunizationList[i].Document_Type.Split(',');
                        int iOBX = 2;
                        int iDocument = 2;
                        for (int d = 0; d < sArrDocuments.Count(); d++)
                        {
                            iOBX = iOBX + 1;
                            iDocument = iDocument + 1;
                            var Documentation = (from p in ClinicalSummary.lookupValues where p.Field_Name == "IMMUNIZATION DOCUMENTATION" && p.Value == sArrDocuments[d].ToString().TrimStart().TrimEnd() select p.Description).FirstOrDefault();
                            sResult = sResult + "\n" + "OBX|" + iOBX + "|CE|69764-9^Document Type^LN|" + iDocument + "|" + Documentation + "^" + sArrDocuments[d].ToString() + "^cdcgs1vis||||||F|||" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd");
                            iOBX = iOBX + 1;
                            sResult = sResult + "\n" + "OBX|" + iOBX + "|DT|29769-7^Date Vis Presented^LN|" + iDocument + "|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||||||F|||" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd");
                        }
                        //if (VISList != null && VISList.Count >= 2)
                        //{
                        //    sResult = sResult + "\n" + "OBX|5|CE|30956-7^vaccine type^LN|3|" + VISList[1].CVX + "^" + VISList[1].Vaccine_Name + "^CVX||||||F";
                        //    sResult = sResult + "\n" + "OBX|6|TS|29768-9^Date vaccine information statement published^LN|3|" + VISList[1].VIS_Date.ToString("yyyyMMdd") + "||||||F";
                        //    sResult = sResult + "\n" + "OBX|7|TS|29769-7^Date vaccine information statement presented^LN|3|" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "||||||F";
                        //}
                        //if (VISList != null && VISList.Count == 3)
                        //{
                        //    sResult = sResult + "\n" + "OBX|8|CE|30956-7^vaccine type^LN|4|" + VISList[2].CVX + "^" + VISList[2].Vaccine_Name + "^CVX||||||F";
                        //    sResult = sResult + "\n" + "OBX|9|TS|29768-9^Date vaccine information statement published^LN|4|" + VISList[2].VIS_Date.ToString("yyyyMMdd") + "||||||F";
                        //    sResult = sResult + "\n" + "OBX|10|TS|29769-7^Date vaccine information statement presented^LN|4|" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "||||||F";
                        //}
                    }
                    //else
                    //{
                    //    sResult = sResult + "\n" + "ORC|RE||" + "9999" + "^CAA|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CA-AA-1^L^^^PRN||" + "|||||" + sMSH6 + "^" + sMSH6 + "^HL70362|";

                    //    sResult = sResult + "\n" + "RXA|" + "0|1|" + Convert.ToDateTime(ClinicalSummary.ImmunizationList[i].Given_Date).ToString("yyyyMMdd") + "||"
                    //    + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                    //    ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|999|||" + "00" + "^" + ClinicalSummary.ImmunizationList[i].Notes + "^NIP002|||||||||||CP|A";
                    //}
                }
            }
            var ImmunizationHistoryList = (from p in ClinicalSummary.immunhistoryList where !ClinicalSummary.ImmunizationList.Any(i => i.Encounter_Id == p.Encounter_ID && (i.Given_Date).ToString("yyyy-MM-dd") == (p.Administered_Date != "" && p.Administered_Date.Length == 11 ? Convert.ToDateTime(p.Administered_Date).ToString("yyyy-MM-dd") : Convert.ToDateTime("0001-01-01").ToString("yyyy-MM-dd")) && i.Procedure_Code == p.Procedure_Code) select p).ToList();
            if (ImmunizationHistoryList.Count > 0)
            {
                for (int i = 0; i < ImmunizationHistoryList.Count; i++)
                {
                    sResult = sResult + "\n" + "ORC|RE||" + ImmunizationHistoryList[i].Id + "^NIST-AA-IZ-2AA-IZ-2^2.16.840.1.113883.3.72.5.40.10^ISO|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^NIST-PI-1&2.16.840.1.113883.3.72.5.40.7&ISO^L^^^PRN" + "|||||||" + sMSH6 + "^" + sMSH6 + "^HL70362|";
                    string sAdminDate = string.Empty;
                    if (ImmunizationHistoryList[i].Administered_Date != string.Empty && ImmunizationHistoryList[i].Administered_Date.Length ==11)
                        sAdminDate = Convert.ToDateTime(ImmunizationHistoryList[i].Administered_Date).ToString("yyyyMMdd");
                    sResult = sResult + "\n" + "RXA|" + "0|1|" + sAdminDate + "||"
                            + ImmunizationHistoryList[i].CVX_Code + "^" +
                            ImmunizationHistoryList[i].Immunization_Description + "^CVX|999|||" + "01" + "^" + ImmunizationHistoryList[i].Notes + "^NIP001|||||||||||CP|A";
                }
            }
            return sResult;
        }


        public string CreateImmunizationRegistry(Human hn, PhysicianLibrary Phy, FillClinicalSummary ClinicalSummary, string sPrintFileName)
        {
            string sResult = string.Empty;
            string CityCode = string.Empty;
            string LocalNumber = string.Empty;
            string CellNumber = string.Empty;
            string CellCityCode = string.Empty;
            String Phone = string.Empty;
            string RaceIdentifier = string.Empty;
            string RaceValue = string.Empty;
            string EthnicityIdentifier = string.Empty;
            string EthnicityValue = string.Empty;
            string PublicitycodeIdentifier = string.Empty;
            string RelationshipIdentifier = string.Empty;
            string AdministrationIdentifier = string.Empty;
            string ImmunizationInformationIdentifier = "00";
            string ProtectionstateIdentifier = string.Empty;
            string ObservationIdentifier = string.Empty;
            string EvidenceIdentifier = string.Empty;
            string RefusedReasonIdentifier = string.Empty;
            string PhyFirstName = string.Empty;
            string PhyLastName = string.Empty;
            string PhyMIName = string.Empty;
            string PhySuffix = string.Empty;
            string Vfc = string.Empty;
            string VaccinetypeCode = string.Empty;
            string sMSH4 = System.Configuration.ConfigurationSettings.AppSettings["MSH4"];
            string sMSH6 = System.Configuration.ConfigurationSettings.AppSettings["MSH6"];
            string sMSH22 = System.Configuration.ConfigurationSettings.AppSettings["MSH22"];
            string sRXA11 = System.Configuration.ConfigurationSettings.AppSettings["RXA11"];


            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
            {
                iFieldLookupList = ClinicalSummary.lookupValues.Where(q => q.Field_Name == "RACE" || q.Field_Name == "ETHNICITY" || q.Field_Name == "PUBLICITY CODE" || q.Field_Name == "RELATIONSHIP" || q.Field_Name == "ROUTE OF ADMINISTRATION" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VFC STATUS" || q.Field_Name == "IMMUNIZATION_INFORMATION_SOURCE" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VACCINE TYPE" || q.Field_Name == "IMMUNIZATION PROTECTION TYPE" || q.Field_Name == "OBSERVATION" || q.Field_Name == "IMMUNIZATION EVIDENCE" || q.Field_Name.ToUpper() == "IMMUNIZATIONLOCATION").ToList();
            }
            if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
            {//Cap - 1716,CAP-1719
                //var RaceIdentifierlst = from d in iFieldLookupList where d.Value == hn.Race select d;
                var RaceIdentifierlst = from d in iFieldLookupList where d.Value == hn.Race && d.Field_Name == "RACE" select d;
                if (RaceIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListRace = RaceIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListRace != null && iFieldLookupListRace.Count > 0)
                    {
                        RaceIdentifier = iFieldLookupListRace[0].Default_Value;
                        RaceValue = iFieldLookupListRace[0].Doc_Type;
                    }
                }
                var EthnicityIdentifierlst = from d in iFieldLookupList where d.Value == hn.Ethnicity && d.Field_Name == "ETHNICITY" select d;
                if (EthnicityIdentifierlst != null && EthnicityIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListEthnicity = EthnicityIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListEthnicity.Count > 0)
                    {
                        EthnicityIdentifier = iFieldLookupListEthnicity[0].Default_Value;
                        EthnicityValue = iFieldLookupListEthnicity[0].Doc_Type;
                    }
                }
                var PublicitycodeIdentifierlst = from d in iFieldLookupList where d.Value == hn.Publicity_Code select d;
                if (PublicitycodeIdentifierlst != null && PublicitycodeIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListPublicity = PublicitycodeIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListPublicity.Count > 0)
                        PublicitycodeIdentifier = iFieldLookupListPublicity[0].Description;
                }
                var RelationshipIdentifierlst = from d in iFieldLookupList where d.Value == hn.Guarantor_Relationship select d;
                if (RelationshipIdentifierlst != null && RelationshipIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListRelationship = RelationshipIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListRelationship != null && iFieldLookupListRelationship.Count > 0)
                        RelationshipIdentifier = iFieldLookupListRelationship[0].Default_Value;
                }

                //IList<StaticLookup> iFieldLookupListEthnicity = ClinicalSummary.lookupValues.Where(q => q.Field_Name == "ETHNICITY").ToList();
            }

            //commented for immunization submission Console app 12.9.2017

            //if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
            // {
            //     IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.Id == ClientSession.PhysicianId).ToList();
            //     if (iPhyList != null && iPhyList.Count > 0)
            //     {
            //         PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
            //         PhyLastName = iPhyList[0].PhyLastName;
            //         PhyMIName = iPhyList[0].PhyMiddleName;
            //     }
            // }
            string sPhy_ID = string.Empty;
            if (Phy != null)
            {
                if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
                {
                    IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.Id == Phy.Id).ToList();
                    if (iPhyList != null && iPhyList.Count > 0)
                    {
                        PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
                        PhyLastName = iPhyList[0].PhyLastName;
                        PhyMIName = iPhyList[0].PhyMiddleName;
                        sPhy_ID = iPhyList[0].Id.ToString();
                        PhySuffix = iPhyList[0].PhySuffix;
                    }


                }

            }

            //sResult = "MSH|^~\\&|Capella EHR|X68||CA Immunization Reg|" + DateTime.Now.ToString("yyyyMMddHHMM") + "||VXU^V04^VXU_V04|NIST-IZ-001.00|P|2.5.1|||AL|ER";
            sResult = "MSH|^~\\&|Capella EHR|" + sMSH4 + "||" + sMSH6 + "|" + DateTime.Now.ToString("yyyyMMddhhmm") + "||VXU^V04^VXU_V04|IR" + DateTime.Now.ToString("yyyyMMddhhmmss") + "|P|2.5.1|||AL|AL||||||" + sMSH22;

            if (hn.Home_Phone_No != String.Empty)
            {
                if (hn.Home_Phone_No.Contains(')'))
                {
                    string[] Phone_No = hn.Home_Phone_No.Split(')');
                    if (Phone_No.Length > 0)
                    {
                        if (hn.Home_Phone_No.Contains('('))
                        {
                            string[] Code_No = Phone_No[0].ToString().Split('(');
                            if (Code_No.Length > 0)
                            {
                                CityCode = Code_No[1].ToString();
                                if (Phone_No[1].Contains("-"))
                                    LocalNumber = Phone_No[1].Replace("-", "").Trim().ToString();
                            }
                        }
                    }
                }
                else
                {
                    if (hn.Home_Phone_No.Length == 10)
                    {
                        CityCode = hn.Home_Phone_No.Substring(0, 3);
                        LocalNumber = hn.Home_Phone_No.Substring(2, 8);
                    }
                }
            }
            //Cap - 1940
            if (hn.Cell_Phone_Number != String.Empty)
            {
                if (hn.Cell_Phone_Number.Contains(')'))
                {
                    string[] Phone_No = hn.Cell_Phone_Number.Split(')');
                    if (Phone_No.Length > 0)
                    {
                        if (hn.Cell_Phone_Number.Contains('('))
                        {
                            string[] Code_No = Phone_No[0].ToString().Split('(');
                            if (Code_No.Length > 0)
                            {
                                CellCityCode = Code_No[1].ToString();
                                if (Phone_No[1].Contains("-"))
                                    CellNumber = Phone_No[1].Replace("-", "").Trim().ToString();
                            }
                        }
                    }
                }
                else
                {
                    if (hn.Cell_Phone_Number.Length == 10)
                    {
                        CellCityCode = hn.Cell_Phone_Number.Substring(0, 3);
                        CellNumber = hn.Cell_Phone_Number.Substring(2, 8);
                    }
                }
            }

            if(hn.Home_Phone_No != string.Empty && hn.Cell_Phone_Number != string.Empty)
            {
                 Phone = "^PRN^PH^^^" + CityCode + "^" + LocalNumber + "~^PRN^CP^^^" + CellCityCode + "^" + CellNumber;
            }
            else if(hn.Home_Phone_No == string.Empty && hn.Cell_Phone_Number != string.Empty)
            {
                 Phone = "^PRN^CP^^^" + CellCityCode + "^" + CellNumber;
            }
            else if (hn.Cell_Phone_Number == string.Empty && hn.Home_Phone_No != string.Empty)
            {
                 Phone = "^PRN^PH^^^" + CityCode + "^" + LocalNumber;
            }
            


            string Mothers_Maiden_First_Name = string.Empty;
            string Mothers_Maiden_Last_Name = string.Empty;
            string Mothers_Maiden_Middle_Name = string.Empty;

            if (hn.Mothers_Maiden_Name != string.Empty)
            {
                if (hn.Mothers_Maiden_Name.Contains(","))
                {
                    string[] Mothers_Maiden_Name = hn.Mothers_Maiden_Name.Split(',');
                    if (Mothers_Maiden_Name.Length == 2)
                    {
                        Mothers_Maiden_First_Name = Mothers_Maiden_Name[0].ToString();
                        //Mothers_Maiden_Middle_Name = Mothers_Maiden_Name[1].ToString();
                        Mothers_Maiden_Last_Name = Mothers_Maiden_Name[1].ToString();
                    }
                }
            }
            string sSex = string.Empty;
            if (hn.Sex != null && hn.Sex != "")
                sSex = hn.Sex.Substring(0, 1);
            if (hn.Patient_Account_External == string.Empty)
            {
                //CAP-1716,CAP-1719 - Immunization Submission to CAIR Records - To include PI(human_id ) for all  submissions instead of MRN Number  
                //Cap -1940
                //if (EthnicityIdentifier == string.Empty)
                //{
                //    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||";
                //}
                //else
                //{
                //    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC";
                //}
                // if (EthnicityIdentifier == string.Empty)
                if (EthnicityValue == string.Empty)
                {
                    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||"+Phone+"|||||||||";
                }
                else
                {
                    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^PI||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||" + Phone + "|||||||||" + EthnicityIdentifier + "^" + EthnicityValue + "^CDCREC";
                }
            }
            else
            {
                //CAP-1716,CAP-1719 - Immunization Submission to CAIR Records - To include PI(human_id ) for all  submissions instead of MRN Number
                //Cap - 1939 & 1940
                //if (EthnicityIdentifier == string.Empty)
                //{
                //    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^PI~" + hn.Patient_Account_External + "^^^MAA^SS||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "^~^NET^^" + hn.EMail + "|||||||||";
                //}
                //else
                //{
                //    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^PI~" + hn.Patient_Account_External + "^^^MAA^SS||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "^~^NET^^" + hn.EMail + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC";
                //}
                //if (EthnicityIdentifier == string.Empty)
                if (EthnicityValue == string.Empty)
                {
                    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^PI~" + hn.Patient_Account_External + "^^^MAA^PT||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||" + Phone + "~^NET^^" + hn.EMail + "|||||||||";
                }
                else
                {
                    sResult = sResult + "\n" + "PID|1||" + hn.Id + "^^^ACUR^PI~" + hn.Patient_Account_External + "^^^MAA^PT||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + sSex + "||" + RaceIdentifier + "^" + RaceValue + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||" + Phone + "~^NET^^" + hn.EMail + "|||||||||" + EthnicityIdentifier + "^" + EthnicityValue + "^CDCREC";
                }
            }


            if (PublicitycodeIdentifier != string.Empty && hn.Immunization_Registry_Status != string.Empty)
            {
                sResult = sResult + "\n" + "PD1|||||||||||" + PublicitycodeIdentifier + "^" + hn.Publicity_Code + "^HL70215|||||" + hn.Immunization_Registry_Status.Substring(0, 1) + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd") + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd");
            }
            else
            {
                sResult = sResult + "\n" + "PD1||||||||||||N||||||";
            }
            //else if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count > 0)
            //{
            //    for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
            //    {
            //        if (ClinicalSummary.immunhistoryList[i].Protection_State != string.Empty)
            //        {
            //            var ProtectionStateIdentifier = from d in iFieldLookupList where d.Value == ClinicalSummary.immunhistoryList[i].Protection_State select d;
            //            if (ProtectionStateIdentifier != null && ProtectionStateIdentifier.Count() > 0)
            //            {
            //                IList<StaticLookup> iFieldLookupListProtection = ProtectionStateIdentifier.ToList<StaticLookup>();
            //                if (iFieldLookupListProtection.Count > 0)
            //                    ProtectionstateIdentifier = iFieldLookupListProtection[0].Description;

            //                sResult = sResult + "\n" + "PD1||||||||||||" + ProtectionstateIdentifier + "|" + ClinicalSummary.immunhistoryList[i].Created_Date_And_Time.ToString("yyyyMMdd");
            //            }

            //        }
            //        else
            //        {
            //            //sResult = sResult + "\n" + "PD1|||||||||||" + PublicitycodeIdentifier + "^" + hn.Publicity_Code + "^HL70215|||||" + hn.Immunization_Registry_Status.Substring(0, 1) + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd") + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd");
            //            //break;
            //        }
            //    }
            //}



            if (hn.Guarantor_Last_Name != string.Empty || hn.Guarantor_First_Name != string.Empty)
                sResult = sResult + "\n" + "NK1|1|" + hn.Guarantor_Last_Name + "^" + hn.Guarantor_First_Name + "^^^^^L|" + RelationshipIdentifier + "^" + hn.Guarantor_Relationship + "^HL70063|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L|^PRN^PH^^^" + CityCode + "^" + LocalNumber + "";


            if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count > 0)
            {
                //if (ClinicalSummary.ImmunizationList.Count < 2)
                //{
                //if (ClinicalSummary.ImmunizationList.Count != 0)
                //{
                //for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
                //{
                //    if (ClinicalSummary.ImmunizationList.Any(a => a.Given_Date.ToString("dd-MMM-yyyy").ToUpper() != (ClinicalSummary.immunhistoryList[i].Administered_Date) && a.Procedure_Code != ClinicalSummary.immunhistoryList[i].Procedure_Code && a.Immunization_Description != ClinicalSummary.immunhistoryList[i].Immunization_Description && a.Encounter_Id != ClinicalSummary.immunhistoryList[i].Encounter_ID))
                //    {
                //        sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.immunhistoryList[i].Id + "^CAA";
                //    }
                //}
                //}
                //else if (ClinicalSummary.immunhistoryList.Count > 0)
                //{
                //    for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
                //    {
                //        sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.immunhistoryList[i].Id + "^CAA";
                //    }
                //}
                //}

            }
            if (ClinicalSummary.ImmunizationList != null && ClinicalSummary.ImmunizationList.Count > 0)
            {
                //if (ClinicalSummary.ImmunizationList.Count < 2)
                //{
                for (int i = 0; i < ClinicalSummary.ImmunizationList.Count; i++)
                {
                    if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() == "Y")
                    {
                        continue;
                    }
                    if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() == "Y")
                    {
                        sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^CDC";
                    }
                    else if (ClinicalSummary.ImmunizationList[i].Immunization_Evidence != string.Empty)
                    {
                        sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^CDC";
                    }
                    else
                    {
                        sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^CAA|||||||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CAA||" + sPhy_ID + "^" + PhyFirstName + "^" + PhyLastName + "^^^^^^CA-AA-1^L";
                    }

                    //RXA

                    string phyID = string.Empty;
                    //PhyFirstName = string.Empty;
                    //PhyLastName = string.Empty;
                    //PhyMIName = string.Empty;
                    //if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
                    //{
                    //    //if (ClinicalSummary.ImmunizationList[i].Given_By.Contains(","))
                    //    if (ClinicalSummary.ImmunizationList[i].Given_By != string.Empty)
                    //    {
                    //        string phyLName = string.Empty;
                    //        string phyFName = string.Empty;
                    //        string phyMName = string.Empty;
                    //        string[] phyName = ClinicalSummary.ImmunizationList[i].Given_By.Split(',');
                    //        if (phyName.Length > 0)
                    //        {
                    //            phyLName = phyName[0].ToString();
                    //            if (phyName.Length > 1)
                    //                phyFName = phyName[1].ToString();
                    //            if (phyName.Length > 2)
                    //                phyMName = phyName[2].ToString();
                    //            IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.PhyLastName == phyLName && q.PhyFirstName == phyFName).ToList();
                    //            if (iPhyList.Count > 0)
                    //            {
                    //                PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
                    //                PhyLastName = iPhyList[0].PhyLastName;
                    //                PhyMIName = iPhyList[0].PhyMiddleName;
                    //                phyID = iPhyList[0].Id.ToString();
                    //            }
                    //            else
                    //            {
                    //                PhyFirstName = phyFName;
                    //                PhyLastName = phyLName;
                    //                PhyMIName = phyMName;
                    //                phyID = "0";
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        phyID = sPhy_ID;
                    //    }
                    //}
                    phyID = sPhy_ID;

                    DateTime AdminDate = DateTime.MinValue;
                    if (ClinicalSummary.ImmunizationList[i].Given_Date.ToString() != string.Empty)
                        AdminDate = Convert.ToDateTime(ClinicalSummary.ImmunizationList[i].Given_Date);

                    string AdminAmount = string.Empty;
                    string VaccineCode = string.Empty;
                    IList<VaccineManufacturerCodes> VaccineCodes = new List<VaccineManufacturerCodes>();
                    IList<VaccineManufacturerCodes> VaccineCodeforManufacturer = new List<VaccineManufacturerCodes>();
                    if (ClinicalSummary != null)
                        VaccineCodes = ClinicalSummary.VaccineCodes;
                    if (VaccineCodes != null && VaccineCodes.Count > 0)
                    {
                        var CodeList = from d in VaccineCodes where d.Manufacturer_Name == ClinicalSummary.ImmunizationList[i].Manufacturer select d;
                        if (CodeList != null && CodeList.Count() > 0)
                        {
                            VaccineCodeforManufacturer = CodeList.ToList<VaccineManufacturerCodes>();
                            if (VaccineCodeforManufacturer != null && VaccineCodeforManufacturer.Count > 0)
                                VaccineCode = VaccineCodeforManufacturer[0].MVX_Code;
                        }

                    }

                    AdminAmount = "999";

                    if (PhyLastName != string.Empty || PhyLastName != string.Empty || PhyLastName != string.Empty)
                    {
                        //sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                        //    + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                        //    ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit
                        //    + "^UCUM||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Immunization_Information_Source + "^NIP001|" +
                        //    "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^NIST-AA-1|^^^" + sRXA11 + "||||" +
                        //    ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                        //    + "|" + VaccineCode
                        //    + "^" +
                        //    ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";
                        if (ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier != null && ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier.Trim() != string.Empty && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source != null && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source.Trim() != string.Empty)
                        {
                            sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                                + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                                ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit
                                + "^UCUM||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Immunization_Information_Source + "^NIP001|" +
                                "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CMS^^^^NPI^^^^^^^^" + PhySuffix + "|^^^" + sRXA11 + "||||" +
                                ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                                + "|" + VaccineCode
                                + "^" +
                                ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";
                        }
                        else if (ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier != null && ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier.Trim() == string.Empty && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source != null && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source.Trim() != string.Empty)
                        {
                            sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                              + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                              ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX||||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Immunization_Information_Source + "^NIP001|" +
                              "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CMS^^^^NPI^^^^^^^^" + PhySuffix + "|^^^" + sRXA11 + "||||" +
                              ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                              + "|" + VaccineCode
                              + "^" +
                              ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";

                        }
                        else if (ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier != null && ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier.Trim() != string.Empty && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source != null && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source.Trim() == string.Empty)
                        {
                            sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                          + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                          ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit
                          + "^UCUM|||" +
                          "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CMS^^^^NPI^^^^^^^^" + PhySuffix + "|^^^" + sRXA11 + "||||" +
                          ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                          + "|" + VaccineCode
                          + "^" +
                          ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";

                        }
                        else if (ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier != null && ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier.Trim() == string.Empty && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source != null && ClinicalSummary.ImmunizationList[i].Immunization_Information_Source.Trim() == string.Empty)
                        {
                            sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
                              + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
                              ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "||||" +
                              "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CMS^^^^NPI^^^^^^^^" + PhySuffix + "|^^^" + sRXA11 + "||||" +
                              ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
                              + "|" + VaccineCode
                              + "^" +
                              ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";
                        }



                    }

                    //RXR
                    VaccineInfoStatementProcedureManager objVaccineInfoStatementManager = new VaccineInfoStatementProcedureManager();
                    IList<VaccineInfoStatement> VISList = objVaccineInfoStatementManager.GetVaccineInfoStatementForSelectedprocedure(ClinicalSummary.ImmunizationList[0].Procedure_Code);
                    string LocationIdentifier = string.Empty;
                    if (ClinicalSummary.ImmunizationList[i].Location != string.Empty)
                        LocationIdentifier = iFieldLookupList.Where(a => a.Value.ToUpper() == ClinicalSummary.ImmunizationList[i].Location.ToUpper()).ToList<StaticLookup>()[0].Description;

                    if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() != "Y" && ClinicalSummary.ImmunizationList[i].Immunization_Evidence == string.Empty && VISList.Count > 0)
                    {
                        sResult = sResult + "\n" + "RXR|" + AdministrationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Route_of_Administration + "^NCIT|" + LocationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Location + "^HL70163";
                        sResult = sResult + "\n" + "OBX|1|CE|64994-7^Vaccine funding program eligibility category^LN^|1|" + Vfc + "^" + ClinicalSummary.ImmunizationList[i].Vfc + "^HL70064||||||F|||" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "|||VXC40^" + "Eligibility captured at the" + ClinicalSummary.ImmunizationList[i].Eligibility_Captured + "^CDCPHINVS";
                        sResult = sResult + "\n" + "OBX|2|CE|30956-7^vaccine type^LN|2|" + VISList[0].CVX.ToString() + "^" + VISList[0].Vaccine_Name + "^CVX||||||F";
                        sResult = sResult + "\n" + "OBX|3|TS|29768-9^Date vaccine information statement published^LN|2|" + VISList[0].VIS_Date.ToString("yyyyMMdd") + "||||||F";
                        sResult = sResult + "\n" + "OBX|4|TS|29769-7^Date vaccine information statement presented^LN|2|" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "||||||F";
                    }
                    if (VISList != null && VISList.Count >= 2)
                    {
                        sResult = sResult + "\n" + "OBX|5|CE|30956-7^vaccine type^LN|3|" + VISList[1].CVX + "^" + VISList[1].Vaccine_Name + "^CVX||||||F";
                        sResult = sResult + "\n" + "OBX|6|TS|29768-9^Date vaccine information statement published^LN|3|" + VISList[1].VIS_Date.ToString("yyyyMMdd") + "||||||F";
                        sResult = sResult + "\n" + "OBX|7|TS|29769-7^Date vaccine information statement presented^LN|3|" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "||||||F";
                    }
                    if (VISList != null && VISList.Count == 3)
                    {
                        sResult = sResult + "\n" + "OBX|8|CE|30956-7^vaccine type^LN|4|" + VISList[2].CVX + "^" + VISList[2].Vaccine_Name + "^CVX||||||F";
                        sResult = sResult + "\n" + "OBX|9|TS|29768-9^Date vaccine information statement published^LN|4|" + VISList[2].VIS_Date.ToString("yyyyMMdd") + "||||||F";
                        sResult = sResult + "\n" + "OBX|10|TS|29769-7^Date vaccine information statement presented^LN|4|" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "||||||F";
                    }
                }
            }
            return sResult;
        }

        //public string CreateImmunizationRegistry(Human hn, PhysicianLibrary Phy, FillClinicalSummary ClinicalSummary, string sPrintFileName)
        //{
        //    string sResult = string.Empty;
        //    string CityCode = string.Empty;
        //    string LocalNumber = string.Empty;
        //    string RaceIdentifier = string.Empty;
        //    string EthnicityIdentifier = string.Empty;
        //    string PublicitycodeIdentifier = string.Empty;
        //    string RelationshipIdentifier = string.Empty;
        //    string AdministrationIdentifier = string.Empty;
        //    string ImmunizationInformationIdentifier = string.Empty;
        //    string ProtectionstateIdentifier = string.Empty;
        //    string ObservationIdentifier = string.Empty;
        //    string EvidenceIdentifier = string.Empty;
        //    string RefusedReasonIdentifier = string.Empty;
        //    string PhyFirstName = string.Empty;
        //    string PhyLastName = string.Empty;
        //    string PhyMIName = string.Empty;
        //    string Vfc = string.Empty;
        //    string VaccinetypeCode = string.Empty;
        //    IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
        //    if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
        //    {
        //        iFieldLookupList = ClinicalSummary.lookupValues.Where(q => q.Field_Name == "RACE" || q.Field_Name == "ETHNICITY" || q.Field_Name == "PUBLICITY CODE" || q.Field_Name == "RELATIONSHIP" || q.Field_Name == "ROUTE OF ADMINISTRATION" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VFC STATUS" || q.Field_Name == "IMMUNIZATION_INFORMATION_SOURCE" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VACCINE TYPE" || q.Field_Name == "IMMUNIZATION PROTECTION TYPE" || q.Field_Name == "OBSERVATION" || q.Field_Name == "IMMUNIZATION EVIDENCE" || q.Field_Name.ToUpper() == "IMMUNIZATIONLOCATION").ToList();
        //    }
        //    if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
        //    {
        //        var RaceIdentifierlst = from d in iFieldLookupList where d.Value == hn.Race select d;
        //        if (RaceIdentifierlst.Count() > 0)
        //        {
        //            IList<StaticLookup> iFieldLookupListRace = RaceIdentifierlst.ToList<StaticLookup>();
        //            if (iFieldLookupListRace != null && iFieldLookupListRace.Count > 0)
        //                RaceIdentifier = iFieldLookupListRace[0].Default_Value;
        //        }
        //        var EthnicityIdentifierlst = from d in iFieldLookupList where d.Value == hn.Ethnicity select d;
        //        if (EthnicityIdentifierlst != null && EthnicityIdentifierlst.Count() > 0)
        //        {
        //            IList<StaticLookup> iFieldLookupListEthnicity = EthnicityIdentifierlst.ToList<StaticLookup>();
        //            if (iFieldLookupListEthnicity.Count > 0)
        //                EthnicityIdentifier = iFieldLookupListEthnicity[0].Default_Value;
        //        }
        //        var PublicitycodeIdentifierlst = from d in iFieldLookupList where d.Value == hn.Publicity_Code select d;
        //        if (PublicitycodeIdentifierlst != null && PublicitycodeIdentifierlst.Count() > 0)
        //        {
        //            IList<StaticLookup> iFieldLookupListPublicity = PublicitycodeIdentifierlst.ToList<StaticLookup>();
        //            if (iFieldLookupListPublicity.Count > 0)
        //                PublicitycodeIdentifier = iFieldLookupListPublicity[0].Description;
        //        }
        //        var RelationshipIdentifierlst = from d in iFieldLookupList where d.Value == hn.Guarantor_Relationship select d;
        //        if (RelationshipIdentifierlst != null && RelationshipIdentifierlst.Count() > 0)
        //        {
        //            IList<StaticLookup> iFieldLookupListRelationship = RelationshipIdentifierlst.ToList<StaticLookup>();
        //            if (iFieldLookupListRelationship != null && iFieldLookupListRelationship.Count > 0)
        //                RelationshipIdentifier = iFieldLookupListRelationship[0].Default_Value;
        //        }

        //        //IList<StaticLookup> iFieldLookupListEthnicity = ClinicalSummary.lookupValues.Where(q => q.Field_Name == "ETHNICITY").ToList();
        //    }
        //    if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
        //    {
        //        IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.Id == Phy.Id).ToList();
        //        //ClientSession.PhysicianId).ToList();
        //        if (iPhyList != null && iPhyList.Count > 0)
        //        {
        //            PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
        //            PhyLastName = iPhyList[0].PhyLastName;
        //            PhyMIName = iPhyList[0].PhyMiddleName;
        //        }
        //    }
        //    //sResult = "MSH|^~\\&|Capella EHR|X68||CA Immunization Reg|" + DateTime.Now.ToString("yyyyMMddHHMM") + "||VXU^V04^VXU_V04|NIST-IZ-001.00|P|2.5.1|||AL|ER";
        //    sResult = "MSH|^~\\&|Capella EHR|X68||CA Immunization Reg|" + DateTime.Now.ToString("yyyyMMddhhmm") + "||VXU^V04^VXU_V04|IR" + DateTime.Now.ToString("yyyyMMddhhmmss") + "|P|2.5.1|||AL|ER";

        //    if (hn.Home_Phone_No != String.Empty)
        //    {
        //        if (hn.Home_Phone_No.Contains(')'))
        //        {
        //            string[] Phone_No = hn.Home_Phone_No.Split(')');
        //            if (Phone_No.Length > 0)
        //            {
        //                if (hn.Home_Phone_No.Contains('('))
        //                {
        //                    string[] Code_No = Phone_No[0].ToString().Split('(');
        //                    if (Code_No.Length > 0)
        //                    {
        //                        CityCode = Code_No[1].ToString();
        //                        if (Phone_No[1].Contains("-"))
        //                            LocalNumber = Phone_No[1].Replace("-", "").Trim().ToString();
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (hn.Home_Phone_No.Length == 10)
        //            {
        //                CityCode = hn.Home_Phone_No.Substring(0, 3);
        //                LocalNumber = hn.Home_Phone_No.Substring(2, 8);
        //            }
        //        }
        //    }
        //    string Mothers_Maiden_First_Name = string.Empty;
        //    string Mothers_Maiden_Last_Name = string.Empty;
        //    string Mothers_Maiden_Middle_Name = string.Empty;

        //    if (hn.Mothers_Maiden_Name != string.Empty)
        //    {
        //        if (hn.Mothers_Maiden_Name.Contains(","))
        //        {
        //            string[] Mothers_Maiden_Name = hn.Mothers_Maiden_Name.Split(',');
        //            if (Mothers_Maiden_Name.Length == 2)
        //            {
        //                Mothers_Maiden_First_Name = Mothers_Maiden_Name[0].ToString();
        //                //Mothers_Maiden_Middle_Name = Mothers_Maiden_Name[1].ToString();
        //                Mothers_Maiden_Last_Name = Mothers_Maiden_Name[1].ToString();
        //            }
        //        }
        //    }
        //    if (hn.Patient_Account_External == string.Empty)
        //        sResult = sResult + "\n" + "PID|1||" + hn.Medical_Record_Number + "^^^ACUR^MR||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC";
        //    else
        //        sResult = sResult + "\n" + "PID|1||" + hn.Medical_Record_Number + "^^^ACUR^MR~" + hn.Patient_Account_External + "^^^MAA^SS||" + hn.Last_Name + "^" + hn.First_Name + "^" + hn.MI + "^^^^L|" + Mothers_Maiden_First_Name + "^" + Mothers_Maiden_Last_Name + "|" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "||" + RaceIdentifier + "^" + hn.Race + "^CDCREC|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L||^PRN^PH^^^" + CityCode + "^" + LocalNumber + "^~^NET^^" + hn.EMail + "|||||||||" + EthnicityIdentifier + "^" + hn.Ethnicity + "^CDCREC";


        //    if (PublicitycodeIdentifier != string.Empty)
        //    {
        //        sResult = sResult + "\n" + "PD1|||||||||||" + PublicitycodeIdentifier + "^" + hn.Publicity_Code + "^HL70215|||||" + hn.Immunization_Registry_Status.Substring(0, 1) + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd") + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd");
        //    }
        //    else if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count > 0)
        //    {
        //        for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
        //        {
        //            if (ClinicalSummary.immunhistoryList[i].Protection_State != string.Empty)
        //            {
        //                var ProtectionStateIdentifier = from d in iFieldLookupList where d.Value == ClinicalSummary.immunhistoryList[i].Protection_State select d;
        //                if (ProtectionStateIdentifier != null && ProtectionStateIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListProtection = ProtectionStateIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListProtection.Count > 0)
        //                        ProtectionstateIdentifier = iFieldLookupListProtection[0].Description;

        //                    sResult = sResult + "\n" + "PD1||||||||||||" + ProtectionstateIdentifier + "|" + ClinicalSummary.immunhistoryList[i].Created_Date_And_Time.ToString("yyyyMMdd");
        //                }

        //            }
        //            else
        //            {
        //                //sResult = sResult + "\n" + "PD1|||||||||||" + PublicitycodeIdentifier + "^" + hn.Publicity_Code + "^HL70215|||||" + hn.Immunization_Registry_Status.Substring(0, 1) + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd") + "|" + hn.Created_Date_And_Time.ToString("yyyyMMdd");
        //                //break;
        //            }
        //        }
        //    }


        //    if (hn.Guarantor_Last_Name != string.Empty || hn.Guarantor_First_Name != string.Empty)
        //        sResult = sResult + "\n" + "NK1|1|" + hn.Guarantor_Last_Name + "^" + hn.Guarantor_First_Name + "^^^^^L|" + RelationshipIdentifier + "^" + hn.Guarantor_Relationship + "^HL70063|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^USA^L|^PRN^PH^^^" + CityCode + "^" + LocalNumber + "";


        //    if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count > 0)
        //    {
        //        if (ClinicalSummary.ImmunizationList.Count < 2)
        //        {
        //            if (ClinicalSummary.ImmunizationList.Count != 0)
        //            {
        //                for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
        //                {
        //                    if (ClinicalSummary.ImmunizationList.Any(a => a.Given_Date.ToString("dd-MMM-yyyy").ToUpper() != (ClinicalSummary.immunhistoryList[i].Administered_Date) && a.Procedure_Code != ClinicalSummary.immunhistoryList[i].Procedure_Code && a.Immunization_Description != ClinicalSummary.immunhistoryList[i].Immunization_Description && a.Encounter_Id != ClinicalSummary.immunhistoryList[i].Encounter_ID))
        //                    {
        //                        sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.immunhistoryList[i].Id + "^CAA";
        //                    }
        //                }
        //            }
        //            else if (ClinicalSummary.immunhistoryList.Count > 0)
        //            {
        //                for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
        //                {
        //                    sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.immunhistoryList[i].Id + "^CAA";
        //                }
        //            }
        //        }

        //    }
        //    if (ClinicalSummary.ImmunizationList != null && ClinicalSummary.ImmunizationList.Count > 0)
        //    {
        //        if (ClinicalSummary.ImmunizationList.Count < 2)
        //        {
        //            for (int i = 0; i < ClinicalSummary.ImmunizationList.Count; i++)
        //            {
        //                if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() == "Y")
        //                {
        //                    sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^CDC";
        //                }
        //                else if (ClinicalSummary.ImmunizationList[i].Immunization_Evidence != string.Empty)
        //                {
        //                    sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^CDC";
        //                }
        //                else
        //                    sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^CAA|||||||" +
        //                                 Phy.Id + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^CAA||" + Phy.Id + "^" + PhyFirstName + "^" + PhyLastName + "^^^^^^CA-AA-1^L";
        //            }
        //        }
        //    }

        //    //sResult = sResult + hn.Last_Name + "^" + hn.First_Name + "||" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "||2106-3^" + hn.Race + "^HL70005|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^^M";

        //    //try
        //    //{
        //    //    sResult = sResult + "||^PRN^^^^" + hn.Home_Phone_No.Substring(1, 3) + "^" + hn.Home_Phone_No.Substring(6, 3) + hn.Home_Phone_No.Substring(10, 4) + "|||||||||N^Not Hispanic or Latino^HL70189";
        //    //}
        //    //catch
        //    //{
        //    //    sResult = sResult + "||^PRN^^^^" + "" + "^" + "" + "" + "|||||||||N^Not Hispanic or Latino^HL70189";
        //    //}
        //    IList<VaccineManufacturerCodes> VaccineCodes = new List<VaccineManufacturerCodes>();
        //    IList<VaccineManufacturerCodes> VaccineCodeforManufacturer = new List<VaccineManufacturerCodes>();
        //    string VaccineCode = string.Empty;
        //    if (ClinicalSummary != null)
        //        VaccineCodes = ClinicalSummary.VaccineCodes;
        //    //immunization history
        //    if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count > 0)
        //    {
        //        if (ClinicalSummary.ImmunizationList != null && ClinicalSummary.ImmunizationList.Count < 2)
        //        {

        //            //var HistList = from h in ClinicalSummary.immunhistoryList where h.Administered_Date == ClinicalSummary.ImmunizationList[i].Manufacturer select d;
        //            if (ClinicalSummary.ImmunizationList.Count != 0)
        //            {
        //                for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
        //                {
        //                    if (ClinicalSummary.ImmunizationList.Any(a => a.Given_Date.ToString("dd-MMM-yyyy").ToUpper() != (ClinicalSummary.immunhistoryList[i].Administered_Date) && a.Procedure_Code != ClinicalSummary.immunhistoryList[i].Procedure_Code && a.Immunization_Description != ClinicalSummary.immunhistoryList[i].Immunization_Description && a.Encounter_Id != ClinicalSummary.immunhistoryList[i].Encounter_ID))
        //                    {
        //                        if (VaccineCodes != null && VaccineCodes.Count > 0)
        //                        {
        //                            var CodeList = from d in VaccineCodes where d.Manufacturer_Name == ClinicalSummary.immunhistoryList[i].Manufacturer select d;
        //                            if (CodeList != null && CodeList.Count() > 0)
        //                            {
        //                                VaccineCodeforManufacturer = CodeList.ToList<VaccineManufacturerCodes>();
        //                                if (VaccineCodeforManufacturer != null && VaccineCodeforManufacturer.Count > 0)
        //                                    VaccineCode = VaccineCodeforManufacturer[0].MVX_Code;
        //                            }

        //                        }
        //                        string phyID = string.Empty;
        //                        PhyFirstName = string.Empty;
        //                        PhyLastName = string.Empty;
        //                        PhyMIName = string.Empty;
        //                        if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
        //                        {
        //                            if (ClinicalSummary.immunhistoryList[i].Created_By.Contains(","))
        //                            {
        //                                string phyLName = string.Empty;
        //                                string phyFName = string.Empty;
        //                                string phyMName = string.Empty;
        //                                string[] phyName = ClinicalSummary.immunhistoryList[i].Created_By.Split(',');
        //                                if (phyName.Length > 0)
        //                                {
        //                                    phyLName = phyName[0].ToString();
        //                                    phyFName = phyName[1].ToString();
        //                                    phyMName = phyName[2].ToString();
        //                                    IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.PhyLastName == phyLName && q.PhyFirstName == phyFName && q.PhyMiddleName == phyMName).ToList();
        //                                    if (iPhyList.Count > 0)
        //                                    {
        //                                        PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
        //                                        PhyLastName = iPhyList[0].PhyLastName;
        //                                        PhyMIName = iPhyList[0].PhyMiddleName;
        //                                        phyID = iPhyList[0].Id.ToString();
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        var ImmunInformationIdentifier = from d in iFieldLookupList where d.Description == ClinicalSummary.immunhistoryList[i].Notes select d;
        //                        if (ImmunInformationIdentifier != null && ImmunInformationIdentifier.Count() > 0)
        //                        {
        //                            IList<StaticLookup> iFieldLookupListImmunInformation = ImmunInformationIdentifier.ToList<StaticLookup>();
        //                            if (iFieldLookupListImmunInformation != null && iFieldLookupListImmunInformation.Count > 0)
        //                                ImmunizationInformationIdentifier = iFieldLookupListImmunInformation[0].Value;
        //                        }
        //                        string AdminAmount = string.Empty;
        //                        //if (ClinicalSummary.immunhistoryList[i].Administered_Amount.ToString() != string.Empty)
        //                        //{
        //                        //    if (ClinicalSummary.immunhistoryList[i].Administered_Amount.ToString().Contains("."))
        //                        //    {
        //                        //        string[] Adamnt = ClinicalSummary.immunhistoryList[i].Administered_Amount.ToString().Split('.');
        //                        //        if (Adamnt.Length > 0)
        //                        //        {
        //                        //            //string Preamnt = Adamnt[0].ToString();
        //                        //            //string Postamnt = Adamnt[1].ToString();
        //                        //            if (Adamnt[1].ToString().Contains("0"))
        //                        //            {
        //                        //                string ad = Adamnt[1].ToString().Replace("0", "");
        //                        //                if (ad.Length > 0)
        //                        //                    AdminAmount = Adamnt[0].ToString() + "." + ad;
        //                        //                else
        //                        //                    AdminAmount = Adamnt[0].ToString();
        //                        //            }
        //                        //            else
        //                        //                AdminAmount = Adamnt[0].ToString() + "." + Adamnt[1].ToString();
        //                        //        }
        //                        //    }
        //                        //}
        //                        DateTime AdminDate = DateTime.MinValue;
        //                        if (ClinicalSummary.immunhistoryList[i].Administered_Date != string.Empty)
        //                            AdminDate = Convert.ToDateTime(ClinicalSummary.immunhistoryList[i].Administered_Date);

        //                        AdminAmount = "999";
        //                        if (PhyLastName != string.Empty || PhyLastName != string.Empty || PhyLastName != string.Empty)
        //                        {

        //                            sResult = sResult + "\n" + "RXA|" + "0|1|" + AdminDate.ToString("yyyyMMdd") + "||"
        //                                + ClinicalSummary.immunhistoryList[i].CVX_Code + "^" +
        //                                ClinicalSummary.immunhistoryList[i].Immunization_Description + "^CVX|" + AdminAmount + "|||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.immunhistoryList[i].Notes + "^NIP001";
        //                        }
        //                        else
        //                        {
        //                            sResult = sResult + "\n" + "RXA|" + "0|1|" + AdminDate.ToString("yyyyMMdd") + "||"
        //                                + ClinicalSummary.immunhistoryList[i].CVX_Code + "^" +
        //                                ClinicalSummary.immunhistoryList[i].Immunization_Description + "^CVX|" + AdminAmount + "|||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.immunhistoryList[i].Notes + "^NIP001";
        //                        }
        //                    }
        //                }
        //            }
        //            else if (ClinicalSummary.immunhistoryList.Count > 0)
        //            {
        //                for (int i = 0; i < ClinicalSummary.immunhistoryList.Count; i++)
        //                {


        //                    if (VaccineCodes.Count > 0)
        //                    {
        //                        var CodeList = from d in VaccineCodes where d.Manufacturer_Name == ClinicalSummary.immunhistoryList[i].Manufacturer select d;
        //                        if (CodeList != null && CodeList.Count() > 0)
        //                        {
        //                            VaccineCodeforManufacturer = CodeList.ToList<VaccineManufacturerCodes>();
        //                            if (VaccineCodeforManufacturer != null && VaccineCodeforManufacturer.Count > 0)
        //                                VaccineCode = VaccineCodeforManufacturer[0].MVX_Code;
        //                        }

        //                    }
        //                    string phyID = string.Empty;
        //                    PhyFirstName = string.Empty;
        //                    PhyLastName = string.Empty;
        //                    PhyMIName = string.Empty;
        //                    if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
        //                    {
        //                        if (ClinicalSummary.immunhistoryList[i].Created_By.Contains(","))
        //                        {
        //                            string phyLName = string.Empty;
        //                            string phyFName = string.Empty;
        //                            string phyMName = string.Empty;
        //                            string[] phyName = ClinicalSummary.immunhistoryList[i].Created_By.Split(',');
        //                            if (phyName.Length > 0)
        //                            {
        //                                phyLName = phyName[0].ToString();
        //                                phyFName = phyName[1].ToString();
        //                                phyMName = phyName[2].ToString();
        //                                IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.PhyLastName == phyLName && q.PhyFirstName == phyFName && q.PhyMiddleName == phyMName).ToList();
        //                                if (iPhyList.Count > 0)
        //                                {
        //                                    PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
        //                                    PhyLastName = iPhyList[0].PhyLastName;
        //                                    PhyMIName = iPhyList[0].PhyMiddleName;
        //                                    phyID = iPhyList[0].Id.ToString();
        //                                }
        //                            }
        //                        }
        //                    }
        //                    var ImmunInformationIdentifier = from d in iFieldLookupList where d.Description == ClinicalSummary.immunhistoryList[i].Notes select d;
        //                    if (ImmunInformationIdentifier != null && ImmunInformationIdentifier.Count() > 0)
        //                    {
        //                        IList<StaticLookup> iFieldLookupListImmunInformation = ImmunInformationIdentifier.ToList<StaticLookup>();
        //                        if (iFieldLookupListImmunInformation != null && iFieldLookupListImmunInformation.Count > 0)
        //                            ImmunizationInformationIdentifier = iFieldLookupListImmunInformation[0].Value;
        //                    }
        //                    string AdminAmount = string.Empty;
        //                    //if (ClinicalSummary.immunhistoryList[i].Administered_Amount.ToString() != string.Empty)
        //                    //{
        //                    //    if (ClinicalSummary.immunhistoryList[i].Administered_Amount.ToString().Contains("."))
        //                    //    {
        //                    //        string[] Adamnt = ClinicalSummary.immunhistoryList[i].Administered_Amount.ToString().Split('.');
        //                    //        if (Adamnt.Length > 0)
        //                    //        {
        //                    //            //string Preamnt = Adamnt[0].ToString();
        //                    //            //string Postamnt = Adamnt[1].ToString();
        //                    //            if (Adamnt[1].ToString().Contains("0"))
        //                    //            {
        //                    //                string ad = Adamnt[1].ToString().Replace("0", "");
        //                    //                if (ad.Length > 0)
        //                    //                    AdminAmount = Adamnt[0].ToString() + "." + ad;
        //                    //                else
        //                    //                    AdminAmount = Adamnt[0].ToString();
        //                    //            }
        //                    //            else
        //                    //                AdminAmount = Adamnt[0].ToString() + "." + Adamnt[1].ToString();
        //                    //        }
        //                    //    }
        //                    //}
        //                    DateTime AdminDate = DateTime.MinValue;
        //                    if (ClinicalSummary.immunhistoryList[i].Administered_Date != string.Empty)
        //                    {
        //                        if (ClinicalSummary.immunhistoryList[i].Administered_Date.Length == 11)
        //                            AdminDate = Convert.ToDateTime(ClinicalSummary.immunhistoryList[i].Administered_Date);
        //                        else
        //                        {
        //                            string sAdminDate = string.Empty;
        //                            string[] sDate = ClinicalSummary.immunhistoryList[i].Administered_Date.Split('-');
        //                            if (sDate.Length == 2)
        //                            {
        //                                sAdminDate = "01-" + ClinicalSummary.immunhistoryList[i].Administered_Date;
        //                            }
        //                            if (sDate.Length == 1)
        //                            {
        //                                sAdminDate = "01-Jan-" + ClinicalSummary.immunhistoryList[i].Administered_Date;
        //                            }
        //                            AdminDate = Convert.ToDateTime(sAdminDate);
        //                        }

        //                    }


        //                    AdminAmount = "999";
        //                    if (PhyLastName != string.Empty || PhyLastName != string.Empty || PhyLastName != string.Empty)
        //                    {

        //                        sResult = sResult + "\n" + "RXA|" + "0|1|" + AdminDate.ToString("yyyyMMdd") + "||"
        //                            + ClinicalSummary.immunhistoryList[i].CVX_Code + "^" +
        //                            ClinicalSummary.immunhistoryList[i].Immunization_Description + "^CVX|" + AdminAmount + "|||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.immunhistoryList[i].Notes + "^NIP001";
        //                    }
        //                    else
        //                    {
        //                        sResult = sResult + "\n" + "RXA|" + "0|1|" + AdminDate.ToString("yyyyMMdd") + "||"
        //                            + ClinicalSummary.immunhistoryList[i].CVX_Code + "^" +
        //                            ClinicalSummary.immunhistoryList[i].Immunization_Description + "^CVX|" + AdminAmount + "|||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.immunhistoryList[i].Notes + "^NIP001";
        //                    }

        //                }
        //            }

        //        }
        //    }

        //    if (ClinicalSummary.ImmunizationList != null && ClinicalSummary.ImmunizationList.Count > 0)
        //    {
        //        if (ClinicalSummary.ImmunizationList.Count < 2)
        //        {
        //            for (int i = 0; i < ClinicalSummary.ImmunizationList.Count; i++)
        //            {
        //                //if (ClinicalSummary.immunhistoryList.Any(a => a.Administered_Date != (ClinicalSummary.ImmunizationList[i].Given_Date.ToString("dd-MMM-yyyy")) && a.Procedure_Code != ClinicalSummary.ImmunizationList[i].Procedure_Code && a.Immunization_Description != ClinicalSummary.ImmunizationList[i].Immunization_Description && a.Encounter_ID != ClinicalSummary.ImmunizationList[i].Encounter_Id))
        //                //{
        //                VaccineInfoStatementProcedureManager objVaccineInfoStatementManager = new VaccineInfoStatementProcedureManager();
        //                IList<VaccineInfoStatement> VISList = objVaccineInfoStatementManager.GetVaccineInfoStatementForSelectedprocedure(ClinicalSummary.ImmunizationList[0].Procedure_Code);
        //                if (VaccineCodes.Count > 0)
        //                {
        //                    var CodeList = from d in VaccineCodes where d.Manufacturer_Name == ClinicalSummary.ImmunizationList[i].Manufacturer select d;
        //                    if (CodeList.Count() > 0)
        //                    {
        //                        VaccineCodeforManufacturer = CodeList.ToList<VaccineManufacturerCodes>();
        //                        if (VaccineCodeforManufacturer.Count > 0)
        //                            VaccineCode = VaccineCodeforManufacturer[0].MVX_Code;
        //                    }
        //                }
        //                string phyID = string.Empty;
        //                PhyFirstName = string.Empty;
        //                PhyLastName = string.Empty;
        //                PhyMIName = string.Empty;
        //                if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
        //                {
        //                    //if (ClinicalSummary.ImmunizationList[i].Given_By.Contains(","))
        //                    if (ClinicalSummary.ImmunizationList[i].Given_By != string.Empty)
        //                    {
        //                        string phyLName = string.Empty;
        //                        string phyFName = string.Empty;
        //                        string phyMName = string.Empty;
        //                        string[] phyName = ClinicalSummary.ImmunizationList[i].Given_By.Split(',');
        //                        if (phyName.Length > 0)
        //                        {
        //                            phyLName = phyName[0].ToString();
        //                            if (phyName.Length > 1)
        //                                phyFName = phyName[1].ToString();
        //                            if (phyName.Length > 2)
        //                                phyMName = phyName[2].ToString();
        //                            IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.PhyLastName == phyLName && q.PhyFirstName == phyFName).ToList();
        //                            if (iPhyList.Count > 0)
        //                            {
        //                                PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
        //                                PhyLastName = iPhyList[0].PhyLastName;
        //                                PhyMIName = iPhyList[0].PhyMiddleName;
        //                                phyID = iPhyList[0].Id.ToString();
        //                            }
        //                            else
        //                            {
        //                                PhyFirstName = phyFName;
        //                                PhyLastName = phyLName;
        //                                PhyMIName = phyMName;
        //                                phyID = "0";
        //                            }
        //                        }
        //                    }
        //                }
        //                var ImmunInformationIdentifier = from d in iFieldLookupList where d.Description == ClinicalSummary.ImmunizationList[i].Immunization_Information_Source select d;
        //                if (ImmunInformationIdentifier != null && ImmunInformationIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListImmunInformation = ImmunInformationIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListImmunInformation != null && iFieldLookupListImmunInformation.Count > 0)
        //                        ImmunizationInformationIdentifier = iFieldLookupListImmunInformation[0].Value;
        //                }
        //                var RefusedIdentifier = from d in iFieldLookupList where d.Description == ClinicalSummary.ImmunizationList[i].Refused_Administration select d;
        //                if (RefusedIdentifier != null && RefusedIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListRefused = RefusedIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListRefused != null && iFieldLookupListRefused.Count > 0)
        //                        RefusedReasonIdentifier = iFieldLookupListRefused[0].Value;
        //                }
        //                string AdminAmount = string.Empty;
        //                if (ClinicalSummary.ImmunizationList[i].Administered_Amount.ToString() != string.Empty)
        //                {
        //                    if (ClinicalSummary.ImmunizationList[i].Administered_Amount.ToString().Contains("."))
        //                    {
        //                        string[] Adamnt = ClinicalSummary.ImmunizationList[i].Administered_Amount.ToString().Split('.');
        //                        if (Adamnt.Length > 0)
        //                        {
        //                            //string Preamnt = Adamnt[0].ToString();
        //                            //string Postamnt = Adamnt[1].ToString();
        //                            if (Adamnt[1].ToString().Contains("0"))
        //                            {
        //                                string ad = Adamnt[1].ToString().Replace("0", "");
        //                                if (ad.Length > 0)
        //                                    AdminAmount = Adamnt[0].ToString() + "." + ad;
        //                                else
        //                                    AdminAmount = Adamnt[0].ToString();
        //                            }
        //                            else
        //                                AdminAmount = Adamnt[0].ToString() + "." + Adamnt[1].ToString();
        //                        }
        //                    }
        //                }
        //                if (PhyLastName != string.Empty || PhyLastName != string.Empty || PhyLastName != string.Empty)
        //                {
        //                    if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() == "Y")
        //                    {
        //                        sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
        //                            + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
        //                            ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + "999" + "||||||||||||" + RefusedReasonIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Refused_Administration + "^NIP002||RE";
        //                    }
        //                    else if (ClinicalSummary.ImmunizationList[i].Immunization_Evidence != string.Empty)
        //                    {
        //                        sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
        //                            + "998" + "^" +
        //                            ClinicalSummary.ImmunizationList[i].Notes + "^CVX|" + "999" + "||||||||||||||" + "NA";
        //                    }
        //                    else
        //                    {
        //                        sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
        //                            + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
        //                            ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit
        //                            + "^UCUM||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Immunization_Information_Source + "^NIP001|" +
        //                            "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName + "^^^^^NIST-AA-1|^^^X68||||" +
        //                            ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
        //                            + "|" + VaccineCode
        //                            + "^" +
        //                            ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";
        //                    }
        //                }
        //                else
        //                {
        //                    if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() == "Y")
        //                    {
        //                        sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
        //                            + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
        //                            ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + "999" + "||||||||||||" + RefusedReasonIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Refused_Administration + "^NIP002||RE";
        //                    }
        //                    else if (ClinicalSummary.ImmunizationList[i].Immunization_Evidence != string.Empty)
        //                    {
        //                        sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
        //                            + "998" + "^" +
        //                            ClinicalSummary.ImmunizationList[i].Notes + "^CVX|" + "999" + "||||||||||||||" + "NA";
        //                    }
        //                    else
        //                    {
        //                        sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
        //                            + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
        //                            ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit
        //                            + "^UCUM||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Immunization_Information_Source + "^NIP001|" +
        //                            "|^^^X68||||" +
        //                            ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
        //                            + "|" + VaccineCode
        //                            + "^" +
        //                            ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";
        //                    }
        //                }
        //                var AdminIdentifier = from d in iFieldLookupList where d.Value == ClinicalSummary.ImmunizationList[i].Route_of_Administration select d;
        //                if (AdminIdentifier != null && AdminIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListAdmin = AdminIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListAdmin != null && iFieldLookupListAdmin.Count > 0)
        //                        AdministrationIdentifier = iFieldLookupListAdmin[0].Default_Value;
        //                }
        //                var VFCIdentifier = from d in iFieldLookupList where (d.Value == ClinicalSummary.ImmunizationList[i].Vfc && d.Field_Name == "VFC STATUS") select d;
        //                if (VFCIdentifier != null && VFCIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListVFC = VFCIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListVFC != null && iFieldLookupListVFC.Count > 0)
        //                        Vfc = iFieldLookupListVFC[0].Description;
        //                }
        //                var VaccinneIdentifier = from d in iFieldLookupList where d.Value == ClinicalSummary.ImmunizationList[i].Vaccine_Type select d;
        //                if (VaccinneIdentifier != null && VaccinneIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListvaccine = VaccinneIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListvaccine != null && iFieldLookupListvaccine.Count > 0)
        //                        VaccinetypeCode = iFieldLookupListvaccine[0].Description;
        //                }
        //                string LocationIdentifier = string.Empty;
        //                if (ClinicalSummary.ImmunizationList[i].Location != string.Empty)
        //                    LocationIdentifier = iFieldLookupList.Where(a => a.Value.ToUpper() == ClinicalSummary.ImmunizationList[i].Location.ToUpper()).ToList<StaticLookup>()[0].Description;


        //                if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() != "Y" && ClinicalSummary.ImmunizationList[i].Immunization_Evidence == string.Empty)
        //                {
        //                    sResult = sResult + "\n" + "RXR|" + AdministrationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Route_of_Administration + "^NCIT|" + LocationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Location + "^HL70163";
        //                    sResult = sResult + "\n" + "OBX|1|CE|64994-7^Vaccine funding program eligibility category^LN^|1|" + Vfc + "^" + ClinicalSummary.ImmunizationList[i].Vfc + "^HL70064||||||F|||" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "|||VXC40^" + "Eligibility captured at the" + ClinicalSummary.ImmunizationList[i].Eligibility_Captured + "^CDCPHINVS";
        //                    sResult = sResult + "\n" + "OBX|2|CE|30956-7^vaccine type^LN|2|" + VISList[0].CVX.ToString() + "^" + VISList[0].Vaccine_Name + "^CVX||||||F";
        //                    sResult = sResult + "\n" + "OBX|3|TS|29768-9^Date vaccine information statement published^LN|2|" + VISList[0].VIS_Date.ToString("yyyyMMdd") + "||||||F";
        //                    sResult = sResult + "\n" + "OBX|4|TS|29769-7^Date vaccine information statement presented^LN|2|" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "||||||F";
        //                }
        //                else if (ClinicalSummary.ImmunizationList[i].Immunization_Evidence != string.Empty)
        //                {
        //                    var ObservationIdentifierLST = from d in iFieldLookupList where d.Description == ClinicalSummary.ImmunizationList[i].Observation select d;
        //                    if (ObservationIdentifierLST != null && ObservationIdentifierLST.Count() > 0)
        //                    {
        //                        IList<StaticLookup> iFieldLookupListobservationIdentifier = ObservationIdentifierLST.ToList<StaticLookup>();
        //                        if (iFieldLookupListobservationIdentifier != null && iFieldLookupListobservationIdentifier.Count > 0)
        //                            ObservationIdentifier = iFieldLookupListobservationIdentifier[0].Value;
        //                    }
        //                    var EvidenceIdentifierlst = from d in iFieldLookupList where d.Value == ClinicalSummary.ImmunizationList[i].Immunization_Evidence select d;
        //                    if (EvidenceIdentifierlst != null && EvidenceIdentifierlst.Count() > 0)
        //                    {
        //                        IList<StaticLookup> iFieldLookupListEvidenceIdentifier = EvidenceIdentifierlst.ToList<StaticLookup>();
        //                        if (iFieldLookupListEvidenceIdentifier != null && iFieldLookupListEvidenceIdentifier.Count > 0)
        //                            EvidenceIdentifier = iFieldLookupListEvidenceIdentifier[0].Description;
        //                    }
        //                    sResult = sResult + "\n" + "OBX|1|CE|" + ObservationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Observation + "^LN^|1|" + EvidenceIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Immunization_Evidence + "^SCT||||||F";
        //                }
        //                //}
        //            }
        //        }
        //    }
        //    if (ClinicalSummary.ImmunizationList != null && ClinicalSummary.ImmunizationList.Count > 1)
        //    {
        //        for (int i = 0; i < ClinicalSummary.ImmunizationList.Count; i++)
        //        {
        //            VaccineInfoStatementProcedureManager objVaccineInfoStatementManager = new VaccineInfoStatementProcedureManager();
        //            IList<VaccineInfoStatement> VISList = objVaccineInfoStatementManager.GetVaccineInfoStatementForSelectedprocedure(ClinicalSummary.ImmunizationList[i].Procedure_Code);
        //            var VaccinneIdentifier = from d in iFieldLookupList where d.Value == ClinicalSummary.ImmunizationList[i].Vaccine_Type select d;
        //            if (VaccinneIdentifier != null && VaccinneIdentifier.Count() > 0)
        //            {
        //                IList<StaticLookup> iFieldLookupListvaccine = VaccinneIdentifier.ToList<StaticLookup>();
        //                if (iFieldLookupListvaccine != null && iFieldLookupListvaccine.Count > 0)
        //                    VaccinetypeCode = iFieldLookupListvaccine[0].Description;
        //            }
        //            //if (i < 2)
        //            {
        //                if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
        //                {
        //                    IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.Id == Phy.Id).ToList();
        //                    if (iPhyList.Count > 0)
        //                    {
        //                        PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
        //                        PhyLastName = iPhyList[0].PhyLastName;
        //                        PhyMIName = iPhyList[0].PhyMiddleName;
        //                    }
        //                }
        //                sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.ImmunizationList[i].Id + "^CAA|||||||||" + Phy.Id + "^" + PhyFirstName + "^" + PhyLastName + "^^^^^^NDA^L";

        //                if (VaccineCodes != null && VaccineCodes.Count > 0)
        //                {
        //                    var CodeList = from d in VaccineCodes where d.Manufacturer_Name == ClinicalSummary.ImmunizationList[i].Manufacturer select d;
        //                    if (CodeList != null && CodeList.Count() > 0)
        //                    {
        //                        VaccineCodeforManufacturer = CodeList.ToList<VaccineManufacturerCodes>();
        //                        if (VaccineCodeforManufacturer != null && VaccineCodeforManufacturer.Count > 0)
        //                            VaccineCode = VaccineCodeforManufacturer[0].MVX_Code;
        //                    }
        //                }
        //                string phyID = string.Empty;
        //                PhyFirstName = string.Empty;
        //                PhyLastName = string.Empty;
        //                PhyMIName = string.Empty;
        //                if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
        //                {
        //                    // if (ClinicalSummary.ImmunizationList[i].Given_By.Contains(","))
        //                    if (ClinicalSummary.ImmunizationList[i].Given_By != string.Empty)
        //                    {
        //                        string phyLName = string.Empty;
        //                        string phyFName = string.Empty;
        //                        string phyMName = string.Empty;
        //                        string[] phyName = ClinicalSummary.ImmunizationList[i].Given_By.Split(',');
        //                        if (phyName.Length > 0)
        //                        {
        //                            phyLName = phyName[0].ToString();
        //                            if (phyName.Length > 1)
        //                                phyFName = phyName[1].ToString();
        //                            if (phyName.Length > 2)
        //                                phyMName = phyName[2].ToString();
        //                            IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.PhyLastName == phyLName && q.PhyFirstName == phyFName).ToList();
        //                            if (iPhyList.Count > 0)
        //                            {
        //                                PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
        //                                PhyLastName = iPhyList[0].PhyLastName;
        //                                PhyMIName = iPhyList[0].PhyMiddleName;
        //                                phyID = iPhyList[0].Id.ToString();
        //                            }
        //                            else
        //                            {
        //                                PhyFirstName = phyFName;
        //                                PhyLastName = phyLName;
        //                                PhyMIName = phyMName;
        //                                phyID = "0";
        //                            }
        //                        }
        //                    }
        //                }
        //                var ImmunInformationIdentifier = from d in iFieldLookupList where d.Description == ClinicalSummary.ImmunizationList[i].Immunization_Information_Source select d;
        //                if (ImmunInformationIdentifier != null && ImmunInformationIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListImmunInformation = ImmunInformationIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListImmunInformation != null && iFieldLookupListImmunInformation.Count > 0)
        //                        ImmunizationInformationIdentifier = iFieldLookupListImmunInformation[0].Value;
        //                }
        //                var RefusedIdentifier = from d in iFieldLookupList where d.Description == ClinicalSummary.ImmunizationList[i].Refused_Administration select d;
        //                if (RefusedIdentifier != null && RefusedIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListRefused = RefusedIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListRefused != null && iFieldLookupListRefused.Count > 0)
        //                        RefusedReasonIdentifier = iFieldLookupListRefused[0].Value;
        //                }
        //                string AdminAmount = string.Empty;
        //                if (ClinicalSummary.ImmunizationList[i].Administered_Amount.ToString() != string.Empty)
        //                {
        //                    if (ClinicalSummary.ImmunizationList[i].Administered_Amount.ToString().Contains("."))
        //                    {
        //                        string[] Adamnt = ClinicalSummary.ImmunizationList[i].Administered_Amount.ToString().Split('.');
        //                        if (Adamnt.Length > 0)
        //                        {
        //                            //string Preamnt = Adamnt[0].ToString();
        //                            //string Postamnt = Adamnt[1].ToString();
        //                            if (Adamnt[1].ToString().Contains("0"))
        //                            {
        //                                string ad = Adamnt[1].ToString().Replace("0", "");
        //                                if (ad.Length > 0)
        //                                    AdminAmount = Adamnt[0].ToString() + "." + ad;
        //                                else
        //                                    AdminAmount = Adamnt[0].ToString();
        //                            }
        //                            else
        //                                AdminAmount = Adamnt[0].ToString() + "." + Adamnt[1].ToString();
        //                        }
        //                    }
        //                }


        //                //sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
        //                //    + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
        //                //    ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit
        //                //    + "^UCUM||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Immunization_Information_Source + "^NIP001|" +
        //                //    "|^^^X68||||" +
        //                //    ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
        //                //    + "|" + VaccineCode
        //                //    + "^" +
        //                //    ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";

        //                sResult = sResult + "\n" + "RXA|" + "0|1|" + ClinicalSummary.ImmunizationList[i].Given_Date.ToString("yyyyMMdd") + "||"
        //                   + ClinicalSummary.ImmunizationList[i].CVX_Code + "^" +
        //                   ClinicalSummary.ImmunizationList[i].Immunization_Description + "^CVX|" + AdminAmount + "|" + ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" + ClinicalSummary.ImmunizationList[i].Administered_Unit
        //                   + "^UCUM||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Immunization_Information_Source + "^NIP001|" + "" + phyID + "^" + PhyFirstName + "^" + PhyLastName + "^" + PhyMIName +
        //                   "^^^^^NIST-AA-1|^^^X68||||" +
        //                   ClinicalSummary.ImmunizationList[i].Lot_Number + "|" + ClinicalSummary.ImmunizationList[i].Expiry_Date.ToString("yyyyMMdd")
        //                   + "|" + VaccineCode
        //                   + "^" +
        //                   ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX|||CP|A";


        //                var AdminIdentifier = from d in iFieldLookupList where d.Value == ClinicalSummary.ImmunizationList[i].Route_of_Administration select d;
        //                if (AdminIdentifier != null && AdminIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListAdmin = AdminIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListAdmin != null && iFieldLookupListAdmin.Count > 0)
        //                        AdministrationIdentifier = iFieldLookupListAdmin[0].Default_Value;
        //                }
        //                var VFCIdentifier = from d in iFieldLookupList where (d.Value == ClinicalSummary.ImmunizationList[i].Vfc && d.Field_Name == "VFC STATUS") select d;
        //                if (VFCIdentifier != null && VFCIdentifier.Count() > 0)
        //                {
        //                    IList<StaticLookup> iFieldLookupListVFC = VFCIdentifier.ToList<StaticLookup>();
        //                    if (iFieldLookupListVFC != null && iFieldLookupListVFC.Count > 0)
        //                        Vfc = iFieldLookupListVFC[0].Description;
        //                }

        //                string LocationIdentifier = string.Empty;
        //                if (ClinicalSummary.ImmunizationList[i].Location != string.Empty)
        //                    LocationIdentifier = iFieldLookupList.Where(a => a.Value.ToUpper() == ClinicalSummary.ImmunizationList[i].Location.ToUpper()).ToList<StaticLookup>()[0].Description;

        //                if (ClinicalSummary.ImmunizationList[i].Is_Administration_Refused.ToUpper() != "Y" && ClinicalSummary.ImmunizationList[i].Immunization_Evidence == string.Empty && VISList.Count > 0)
        //                {
        //                    sResult = sResult + "\n" + "RXR|" + AdministrationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Route_of_Administration + "^NCIT|" + LocationIdentifier + "^" + ClinicalSummary.ImmunizationList[i].Location + "^HL70163";
        //                    sResult = sResult + "\n" + "OBX|1|CE|64994-7^Vaccine funding program eligibility category^LN^|1|" + Vfc + "^" + ClinicalSummary.ImmunizationList[i].Vfc + "^HL70064||||||F|||" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "|||VXC40^" + "Eligibility captured at the" + ClinicalSummary.ImmunizationList[i].Eligibility_Captured + "^CDCPHINVS";
        //                    sResult = sResult + "\n" + "OBX|2|CE|30956-7^vaccine type^LN|2|" + VISList[0].CVX + "^" + VISList[0].Vaccine_Name + "^CVX||||||F";
        //                    sResult = sResult + "\n" + "OBX|3|TS|29768-9^Date vaccine information statement published^LN|2|" + VISList[0].VIS_Date.ToString("yyyyMMdd") + "||||||F";
        //                    sResult = sResult + "\n" + "OBX|4|TS|29769-7^Date vaccine information statement presented^LN|2|" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "||||||F";
        //                }
        //            }
        //            //else if (i == 2)
        //            if (VISList != null && VISList.Count >= 2)
        //            {
        //                sResult = sResult + "\n" + "OBX|5|CE|30956-7^vaccine type^LN|3|" + VISList[1].CVX + "^" + VISList[1].Vaccine_Name + "^CVX||||||F";
        //                sResult = sResult + "\n" + "OBX|6|TS|29768-9^Date vaccine information statement published^LN|3|" + VISList[1].VIS_Date.ToString("yyyyMMdd") + "||||||F";
        //                sResult = sResult + "\n" + "OBX|7|TS|29769-7^Date vaccine information statement presented^LN|3|" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "||||||F";
        //            }
        //            //else if (i == 3)
        //            if (VISList != null && VISList.Count == 3)
        //            {
        //                sResult = sResult + "\n" + "OBX|8|CE|30956-7^vaccine type^LN|4|" + VISList[2].CVX + "^" + VISList[2].Vaccine_Name + "^CVX||||||F";
        //                sResult = sResult + "\n" + "OBX|9|TS|29768-9^Date vaccine information statement published^LN|4|" + VISList[2].VIS_Date.ToString("yyyyMMdd") + "||||||F";
        //                sResult = sResult + "\n" + "OBX|10|TS|29769-7^Date vaccine information statement presented^LN|4|" + ClinicalSummary.ImmunizationList[i].VIS_Given_Date.ToString("yyyyMMdd") + "||||||F";
        //            }
        //            if (ClinicalSummary.immunhistoryList != null && ClinicalSummary.immunhistoryList.Count > 0)
        //            {
        //                if (i == 0)
        //                {
        //                    for (int j = 0; j < ClinicalSummary.immunhistoryList.Count; j++)
        //                    {
        //                        if (!ClinicalSummary.ImmunizationList.Any(item => item.Procedure_Code == ClinicalSummary.immunhistoryList[j].Procedure_Code && item.Created_Date_And_Time.Date == ClinicalSummary.immunhistoryList[j].Created_Date_And_Time.Date))
        //                        {
        //                            sResult = sResult + "\n" + "ORC|RE||" + ClinicalSummary.immunhistoryList[j].Id + "^CAA";

        //                            if (VaccineCodes != null && VaccineCodes.Count > 0)
        //                            {
        //                                var CodeList = from d in VaccineCodes where d.Manufacturer_Name == ClinicalSummary.immunhistoryList[i].Manufacturer select d;
        //                                if (CodeList != null && CodeList.Count() > 0)
        //                                {
        //                                    VaccineCodeforManufacturer = CodeList.ToList<VaccineManufacturerCodes>();
        //                                    if (VaccineCodeforManufacturer != null && VaccineCodeforManufacturer.Count > 0)
        //                                        VaccineCode = VaccineCodeforManufacturer[0].MVX_Code;
        //                                }
        //                            }
        //                            string phyID = string.Empty;
        //                            PhyFirstName = string.Empty;
        //                            PhyLastName = string.Empty;
        //                            PhyMIName = string.Empty;
        //                            if (ClinicalSummary.phyList != null && ClinicalSummary.phyList.Count > 0)
        //                            {
        //                                if (ClinicalSummary.immunhistoryList[i].Created_By.Contains(","))
        //                                {
        //                                    string phyLName = string.Empty;
        //                                    string phyFName = string.Empty;
        //                                    string phyMName = string.Empty;
        //                                    string[] phyName = ClinicalSummary.immunhistoryList[i].Created_By.Split(',');
        //                                    if (phyName != null && phyName.Length > 0)
        //                                    {
        //                                        phyLName = phyName[0].ToString();
        //                                        phyFName = phyName[1].ToString();
        //                                        phyMName = phyName[2].ToString();
        //                                        IList<PhysicianLibrary> iPhyList = ClinicalSummary.phyList.Where(q => q.PhyLastName == phyLName && q.PhyFirstName == phyFName && q.PhyMiddleName == phyMName).ToList();
        //                                        if (iPhyList != null && iPhyList.Count > 0)
        //                                        {
        //                                            PhyFirstName = iPhyList[0].PhyPrefix + " " + iPhyList[0].PhyFirstName;
        //                                            PhyLastName = iPhyList[0].PhyLastName;
        //                                            PhyMIName = iPhyList[0].PhyMiddleName;
        //                                            phyID = iPhyList[0].Id.ToString();
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            var ImmunInformationIdentifier = from d in iFieldLookupList where d.Description == ClinicalSummary.immunhistoryList[i].Notes select d;
        //                            if (ImmunInformationIdentifier != null && ImmunInformationIdentifier.Count() > 0)
        //                            {
        //                                IList<StaticLookup> iFieldLookupListImmunInformation = ImmunInformationIdentifier.ToList<StaticLookup>();
        //                                if (iFieldLookupListImmunInformation != null && iFieldLookupListImmunInformation.Count > 0)
        //                                    ImmunizationInformationIdentifier = iFieldLookupListImmunInformation[0].Value;
        //                            }
        //                            DateTime AdminDate = DateTime.MinValue;
        //                            if (ClinicalSummary.immunhistoryList[i].Administered_Date != string.Empty)
        //                                AdminDate = Convert.ToDateTime(ClinicalSummary.immunhistoryList[i].Administered_Date);
        //                            string AdminAmount = string.Empty;
        //                            AdminAmount = "999";
        //                            if (PhyLastName != string.Empty || PhyLastName != string.Empty || PhyLastName != string.Empty)
        //                            {

        //                                sResult = sResult + "\n" + "RXA|" + "0|1|" + AdminDate.ToString("yyyyMMdd") + "||"
        //                                    + ClinicalSummary.immunhistoryList[i].CVX_Code + "^" +
        //                                    ClinicalSummary.immunhistoryList[i].Immunization_Description + "^CVX|" + AdminAmount + "|||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.immunhistoryList[i].Notes + "^NIP001";
        //                            }
        //                            else
        //                            {
        //                                sResult = sResult + "\n" + "RXA|" + "0|1|" + AdminDate.ToString("yyyyMMdd") + "||"
        //                                    + ClinicalSummary.immunhistoryList[i].CVX_Code + "^" +
        //                                    ClinicalSummary.immunhistoryList[i].Immunization_Description + "^CVX|" + AdminAmount + "|||" + ImmunizationInformationIdentifier + "^" + ClinicalSummary.immunhistoryList[i].Notes + "^NIP001";
        //                            }

        //                            break;
        //                        }
        //                    }

        //                }
        //            }
        //        }

        //    }
        //    return sResult;
        //}


        public string CreateHL7ForSyndromicSurveillance(Encounter objEncounter, Human objHuman, FacilityLibrary objFacility, FillClinicalSummary ClinicalSummary, string sMSH, string sACK)
        {
            string sMyHL7 = string.Empty;
            DateTime currentDateTime = DateTime.Now;
            //Latha - 10 Aug 2011 - Start - AllLookups singleton
            //AllLookups allLookups = new AllLookups();
            //Latha - 10 Aug 2011 - End
            //IList<FieldLookup> lookupList;
            DateTime ccDate = DateTime.MinValue;
            string raceCode = string.Empty, raceCodeSys = string.Empty;
            string ethnicityCode = string.Empty, ethnicityCodeSys = string.Empty;
            string sCC = string.Empty;
            string sSex = string.Empty;
            int humanAgeInYears = 0;

            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
            {
                iFieldLookupList = ClinicalSummary.lookupValues.Where(q => q.Field_Name == "RACE" || q.Field_Name == "ETHNICITY" || q.Field_Name == "PUBLICITY CODE" || q.Field_Name == "RELATIONSHIP" || q.Field_Name == "ROUTE OF ADMINISTRATION" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VFC STATUS" || q.Field_Name == "IMMUNIZATION_INFORMATION_SOURCE" || q.Field_Name == "REFUSED_ADMINISTRATION" || q.Field_Name == "VACCINE TYPE" || q.Field_Name == "IMMUNIZATION PROTECTION TYPE" || q.Field_Name == "OBSERVATION" || q.Field_Name == "IMMUNIZATION EVIDENCE").ToList();
            }
            if (ClinicalSummary.lookupValues != null && ClinicalSummary.lookupValues.Count > 0)
            {
                //var RaceIdentifierlst = from d in iFieldLookupList where d.Value == objHuman.Race select d;
                string[] sarr = objHuman.Race.Split(',');
                raceCode = string.Empty;
                foreach (string sStr in sarr)
                {
                    var RaceIdentifierlst = from d in iFieldLookupList where d.Value == sStr select d;
                    if (RaceIdentifierlst != null && RaceIdentifierlst.Count() > 0)
                    {
                        IList<StaticLookup> iFieldLookupListRace = RaceIdentifierlst.ToList<StaticLookup>();
                        if (iFieldLookupListRace != null && iFieldLookupListRace.Count > 0)
                        {
                            if (raceCode == string.Empty)
                            {
                                raceCode = iFieldLookupListRace[0].Default_Value + "^" + sStr + "^CDCREC";
                            }
                            else
                            {
                                raceCode += "~" + iFieldLookupListRace[0].Default_Value + "^" + sStr + "^CDCREC";
                            }
                        }
                        //raceCode = iFieldLookupListRace[0].Default_Value;
                    }
                }

                var EthnicityIdentifierlst = from d in iFieldLookupList where d.Value == objHuman.Ethnicity select d;
                if (EthnicityIdentifierlst != null && EthnicityIdentifierlst.Count() > 0)
                {
                    IList<StaticLookup> iFieldLookupListEthnicity = EthnicityIdentifierlst.ToList<StaticLookup>();
                    if (iFieldLookupListEthnicity != null && iFieldLookupListEthnicity.Count > 0)
                    {
                        ethnicityCode = iFieldLookupListEthnicity[0].Default_Value;
                    }

                }
            }
            string sState_Code = string.Empty;
            string sCounty_Code = string.Empty;
            if (objHuman.State != string.Empty)
            {
                IList<State> ilstState = new List<State>();
                StateManager objState = new StateManager();
                ilstState = objState.GetCountybyState(objHuman.State);
                var CountyState = (from s in ilstState where s.City == objHuman.City select s).ToList<State>();
                if (CountyState.Count > 0)
                {
                    sState_Code = CountyState[0].State_Concept_Code;
                    sCounty_Code = CountyState[0].County_Codes;
                }
            }
            string FacilityName = string.Empty;
            if (objFacility.Fac_Name.Trim().Length >= 20)
                FacilityName = objFacility.Fac_Name.Trim().Substring(0, 19);
            else
                FacilityName = objFacility.Fac_Name.Trim();
            Boolean bMonth = false;
            //if (objHuman.Birth_Date.Year==DateTime.UtcNow.Year)
            //{
            humanAgeInYears = UtilityManager.CalculateAgeInMonths(objHuman.Birth_Date, DateTime.UtcNow);
            //    bMonth = true;
            //}
            //else
            //    humanAgeInYears = UtilityManager.CalculateAge(objHuman.Birth_Date);
            //sMyHL7 = "MSH|^~\\&||" + FacilityName + "^" + objFacility.Fac_NPI + "^NPI|||" + currentDateTime.ToString("yyyyMMddhhmm") + "||ADT^A04^ADT_A01|SS" + currentDateTime.ToString("yyyyMMddhhmmss") + "|P|2.5.1|||||||||PH_SS-NoAck^SS Sender^2.16.840.1.114222.4.10.3^ISO" + Environment.NewLine;
            if (sACK == "PH_SS-Ack")
                sMyHL7 = "MSH|^~\\&||" + FacilityName + "^" + objFacility.Fac_NPI + "^NPI|||" + currentDateTime.ToString("yyyyMMddhhmm") + "||" + sMSH + "|NIST-SS-003.41|P|2.5.1|||AL|AL|||||" + sACK + "^SS Sender^2.16.840.1.114222.4.10.3^ISO" + Environment.NewLine;
            else
                sMyHL7 = "MSH|^~\\&||" + FacilityName + "^" + objFacility.Fac_NPI + "^NPI|||" + currentDateTime.ToString("yyyyMMddhhmm") + "||" + sMSH + "|NIST-SS-003.41|P|2.5.1|||NE||||||" + sACK + "^SS Sender^2.16.840.1.114222.4.10.3^ISO" + Environment.NewLine;

            //sMyHL7 += "EVN||" + currentDateTime.ToString("yyyyMMddhhmm") + "|||||" + FacilityName + "^" + objFacility.Fac_NPI + "^NPI" + Environment.NewLine;
            sMyHL7 += "EVN||" + currentDateTime.ToString("yyyyMMddhhmm") + "|||||" + FacilityName + "^" + objFacility.Fac_NPI + "^NPI" + Environment.NewLine;


            sSex = (objHuman.Sex.ToUpper() == "MALE") ? "M" : "F";

            string sTempRace = string.Empty;
            string sTempEthnicity = string.Empty;

            if (objHuman.Ethnicity != string.Empty)
            {
                sTempEthnicity = ethnicityCode + "^" + objHuman.Ethnicity + "^CDCREC";
            }

            if (objHuman.Race != string.Empty)
            {
                //sTempRace = raceCode + "^" + objHuman.Race + "^CDCREC";
                sTempRace = raceCode;
            }

            string input = objHuman.First_Name + objHuman.Last_Name;
            bool isDigitPresent = input.Any(c => char.IsDigit(c));

            //sMyHL7 += "PID|1||" + objHuman.Medical_Record_Number + "#^^^^MR||^^^^^^~^^^^^^S|||" + sSex + "||" + raceCode + "^" + objHuman.Race + "^CDCREC|^^^^" + objHuman.ZipCode + "^^^^|||||||||||" + ethnicityCode + "^" + objHuman.Ethnicity + "^CDCREC" + Environment.NewLine;
            // sMyHL7 += "PID|1||" + objHuman.Medical_Record_Number + "^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR||^^^^^^~^^^^^^U|||" + sSex + "||" + raceCode + "^" + objHuman.Race + "^CDCREC|^^" + objHuman.City + "^" + sState_Code + "^" + objHuman.ZipCode + "^USA^^^" + sCounty_Code + "|||||||||||" + ethnicityCode + "^" + objHuman.Ethnicity + "^CDCREC" + Environment.NewLine;
            if (isDigitPresent)
            {
                //CAP-1716,CAP-1719 - Immunization Submission to CAIR Records - To include PI(human_id ) for all  submissions instead of MRN Number 
                //if (sMSH == "ADT^A03^ADT_A03" && objHuman.Patient_Status == "DECEASED")
                //{
                //    sMyHL7 += "PID|1||" + objHuman.Medical_Record_Number + "^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR||^^^^^^~^^^^^^S|||" + sSex + "||" + sTempRace + "|^^" + objHuman.City + "^" + sState_Code + "^" + objHuman.ZipCode + "^USA^^^" + sCounty_Code + "|||||||||||" + sTempEthnicity + "|||||||" + objHuman.Date_Of_Death.ToString("yyyyMMddhhmmss") + "|Y" + Environment.NewLine;
                //}
                //else
                //    sMyHL7 += "PID|1||" + objHuman.Medical_Record_Number + "^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR||^^^^^^~^^^^^^S|||" + sSex + "||" + sTempRace + "|^^" + objHuman.City + "^" + sState_Code + "^" + objHuman.ZipCode + "^USA^^^" + sCounty_Code + "|||||||||||" + sTempEthnicity + Environment.NewLine;
                if (sMSH == "ADT^A03^ADT_A03" && objHuman.Patient_Status == "DECEASED")
                {
                    sMyHL7 += "PID|1||" + objHuman.Id + "^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^PI||^^^^^^~^^^^^^S|||" + sSex + "||" + sTempRace + "|^^" + objHuman.City + "^" + sState_Code + "^" + objHuman.ZipCode + "^USA^^^" + sCounty_Code + "|||||||||||" + sTempEthnicity + "|||||||" + objHuman.Date_Of_Death.ToString("yyyyMMddhhmmss") + "|Y" + Environment.NewLine;
                }
                else
                    sMyHL7 += "PID|1||" + objHuman.Id + "^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^PI||^^^^^^~^^^^^^S|||" + sSex + "||" + sTempRace + "|^^" + objHuman.City + "^" + sState_Code + "^" + objHuman.ZipCode + "^USA^^^" + sCounty_Code + "|||||||||||" + sTempEthnicity + Environment.NewLine;
            }
            else
            {
                //CAP-1716,CAP-1719 - Immunization Submission to CAIR Records - To include PI(human_id ) for all  submissions instead of MRN Number 
                //if (sMSH == "ADT^A03^ADT_A03" && objHuman.Patient_Status == "DECEASED")
                //{
                //    sMyHL7 += "PID|1||" + objHuman.Medical_Record_Number + "^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR||^^^^^^~^^^^^^U|||" + sSex + "||" + sTempRace + "|^^" + objHuman.City + "^" + sState_Code + "^" + objHuman.ZipCode + "^USA^^^" + sCounty_Code + "|||||||||||" + sTempEthnicity + "|||||||" + objHuman.Date_Of_Death.ToString("yyyyMMddhhmmss") + "|Y" + Environment.NewLine;
                //}
                //else
                //    sMyHL7 += "PID|1||" + objHuman.Medical_Record_Number + "^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR||^^^^^^~^^^^^^U|||" + sSex + "||" + sTempRace + "|^^" + objHuman.City + "^" + sState_Code + "^" + objHuman.ZipCode + "^USA^^^" + sCounty_Code + "|||||||||||" + sTempEthnicity + Environment.NewLine;
                if (sMSH == "ADT^A03^ADT_A03" && objHuman.Patient_Status == "DECEASED")
                {
                    sMyHL7 += "PID|1||" + objHuman.Id + "^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^PI||^^^^^^~^^^^^^U|||" + sSex + "||" + sTempRace + "|^^" + objHuman.City + "^" + sState_Code + "^" + objHuman.ZipCode + "^USA^^^" + sCounty_Code + "|||||||||||" + sTempEthnicity + "|||||||" + objHuman.Date_Of_Death.ToString("yyyyMMddhhmmss") + "|Y" + Environment.NewLine;
                }
                else
                    sMyHL7 += "PID|1||" + objHuman.Id + "^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^PI||^^^^^^~^^^^^^U|||" + sSex + "||" + sTempRace + "|^^" + objHuman.City + "^" + sState_Code + "^" + objHuman.ZipCode + "^USA^^^" + sCounty_Code + "|||||||||||" + sTempEthnicity + Environment.NewLine;
            }

            // sMyHL7 += "PV1|1||||||||||||||||||" + objEncounter.Id + "^^^^VN|||||||||||||||||||||||||" + objEncounter.Appointment_Date.ToString("yyyyMMddhhmm") + Environment.NewLine;

            if (sMSH == "ADT^A03^ADT_A03")
            {
                if (ClinicalSummary.HospitalizationHistory.Count > 0)
                    //bug id:50490 
                    if (ClinicalSummary.HospitalizationHistory[0].To_Date.Length == 4)
                    {
                        ClinicalSummary.HospitalizationHistory[0].To_Date = "01-Jan-" + ClinicalSummary.HospitalizationHistory[0].To_Date;
                    }
                    //else if (ClinicalSummary.HospitalizationHistory[0].To_Date == "Current")
                    //{
                    //    ClinicalSummary.HospitalizationHistory[0].To_Date = ClinicalSummary.HospitalizationHistory[0].Created_Date_And_Time.ToString("dd-MMM-yyyy");
                    //}
                    else if (ClinicalSummary.HospitalizationHistory[0].To_Date.Length == 8)
                    {
                        ClinicalSummary.HospitalizationHistory[0].To_Date = "01-" + ClinicalSummary.HospitalizationHistory[0].To_Date;
                    }
                if (ClinicalSummary.HospitalizationHistory[0].To_Date != "" && ClinicalSummary.HospitalizationHistory[0].To_Date != "Current")
                {
                    string sCode = string.Empty;
                    string sEmergency = string.Empty;
                    if (ClinicalSummary.HospitalizationHistory[0].Hospitalization_Notes != string.Empty)
                    {
                        string sLookupXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                        if (File.Exists(sLookupXmlFilePath) == true)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            XmlTextReader XmlText = new XmlTextReader(sLookupXmlFilePath);
                            itemDoc.Load(XmlText);
                            XmlText.Close();
                            XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("HospitalizationHistoryList");
                            if (xmlNodeList != null && xmlNodeList.Count > 0)
                            {
                                for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                                {
                                    if (xmlNodeList[0].ChildNodes[j].Attributes[0].Value.ToString() == ClinicalSummary.HospitalizationHistory[0].Hospitalization_Notes)
                                        sCode = xmlNodeList[0].ChildNodes[j].Attributes[1].Value.ToString();
                                }
                            }
                        }
                    }

                    if (objFacility.Taxonomy_Code == "261QE0002X")
                    {
                        sEmergency = "E";
                    }
                    else
                        sEmergency = "O";



                    sMyHL7 += "PV1|1|" + sEmergency + "|||||||||||||||||" + objHuman.Medical_Record_Number + "_001^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR|||||||||||||||||" + sCode + "||||||||" + objEncounter.Date_of_Service.ToString("yyyyMMddhhmm") + "|" + Convert.ToDateTime(ClinicalSummary.HospitalizationHistory[0].To_Date).ToString("yyyyMMddhhmm") + Environment.NewLine;
                }

                //if (objFacility.Taxonomy_Code == "261QE0002X")
                //{
                //    if (objHuman.Patient_Status == "DECEASED")
                //        sMyHL7 += "PV1|1|E|||||||||||||||||" + objHuman.Medical_Record_Number + "_001^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR|||||||||||||||||20||||||||" + objEncounter.Date_of_Service.ToString("yyyyMMddhhmm") + "|" + Convert.ToDateTime(ClinicalSummary.HospitalizationHistory[0].To_Date).ToString("yyyyMMddhhmm") + Environment.NewLine;
                //    else 
                //        sMyHL7 += "PV1|1|E|||||||||||||||||" + objHuman.Medical_Record_Number + "_001^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR|||||||||||||||||01||||||||" + objEncounter.Date_of_Service.ToString("yyyyMMddhhmm") + "|" + Convert.ToDateTime(ClinicalSummary.HospitalizationHistory[0].To_Date).ToString("yyyyMMddhhmm") + Environment.NewLine;
                //}
                //else
                //{
                //    sMyHL7 += "PV1|1|O|||||||||||||||||" + objHuman.Medical_Record_Number + "_001^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR|||||||||||||||||01||||||||" + objEncounter.Date_of_Service.ToString("yyyyMMddhhmm") + "|" + Convert.ToDateTime(ClinicalSummary.HospitalizationHistory[0].To_Date).ToString("yyyyMMddhhmm") + Environment.NewLine;
                //}

            }
            else
            {
                if (objFacility.Taxonomy_Code == "261QE0002X")
                    sMyHL7 += "PV1|1|E|||||||||||||||||" + objHuman.Medical_Record_Number + "_001^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR|||||||||||||||||||||||||" + objEncounter.Date_of_Service.ToString("yyyyMMddhhmm") + "|" + Environment.NewLine;
                else
                    sMyHL7 += "PV1|1|O|||||||||||||||||" + objHuman.Medical_Record_Number + "_001^^^" + FacilityName + "&" + objFacility.Fac_NPI + "&NPI^MR|||||||||||||||||||||||||" + objEncounter.Date_of_Service.ToString("yyyyMMddhhmm") + "|" + Environment.NewLine;
            }
            if (ClinicalSummary.Assessment != null && ClinicalSummary.Assessment.Count > 0)
            {
                var Assessment = (from a in ClinicalSummary.Assessment where a.Primary_Diagnosis == "Y" && a.Assessment_Notes == "" select new { ICD = a.ICD_9, Description = a.ICD_Description, ICDVersion = (a.Version_Year == "ICD_10" ? "I10C" : "I9CDX") }).ToArray();
                if (Assessment.Length > 0)
                    sMyHL7 += "PV2|||" + Assessment[0].ICD + "^" + Assessment[0].Description + "^" + Assessment[0].ICDVersion + "|" + Environment.NewLine;
                var AssessmentNotes = (from a in ClinicalSummary.Assessment where a.Primary_Diagnosis == "Y" && a.Assessment_Notes != "" select new { ICD = a.ICD_9, Description = a.ICD_Description, Notes = a.Assessment_Notes, ICDVersion = (a.Version_Year == "ICD_10" ? "I10C" : "I9CDX") }).ToArray();
                if (AssessmentNotes.Length > 0)
                    sMyHL7 += "PV2|||^" + AssessmentNotes[0].Notes + Environment.NewLine;
            }
            if (sMSH == "ADT^A03^ADT_A03")
            {
                if (ClinicalSummary.Assessment != null && ClinicalSummary.Assessment.Count > 0)
                {
                    int j = 1;
                    for (int i = 0; i < ClinicalSummary.Assessment.Count; i++)
                    {
                        if (ClinicalSummary.Assessment[i].Assessment_Notes != "" || ClinicalSummary.Assessment[i].Primary_Diagnosis != "Y")
                        {

                            if (ClinicalSummary.Assessment[i].Version_Year == "ICD_10")
                                sMyHL7 += "DG1|" + j + "||" + ClinicalSummary.Assessment[i].ICD_9 + "^" + ClinicalSummary.Assessment[i].ICD_Description + "^" + "I10C" + "|||F" + Environment.NewLine;
                            else if (ClinicalSummary.Assessment[i].Version_Year == "ICD_9")
                                sMyHL7 += "DG1|" + j + "||" + ClinicalSummary.Assessment[i].ICD_9 + "^" + ClinicalSummary.Assessment[i].ICD_Description + "^" + "I9CDX" + "|||F" + Environment.NewLine;
                            j++;
                        }
                    }
                }
            }
            int iObx = 1;
            //sMyHL7 += "OBX|1|CWE|SS003^^PHINQUESTION||261QU0200X^Urgent Care^NUCC||||||F" + Environment.NewLine;
            if (sACK == "PH_SS-Ack")
            {
                string sTaxnomic_Code_Description = string.Empty;
                sTaxnomic_Code_Description = objFacility.Taxonomy_Code + "^" + objFacility.Taxonomy_Description + "^" + "HCPTNUCC^^^^^^Urgent Care Center";
                sMyHL7 += "OBX|" + iObx + "|CWE|SS003^Facility/Visit Type^PHINQUESTION||" + sTaxnomic_Code_Description + "||||||F" + Environment.NewLine;
            }
            else
                sMyHL7 += "OBX|" + iObx + "|CWE|SS001^TREATING FACILITY IDENTIFIER^PHINQUESTION||" + FacilityName + "^" + objFacility.Fac_NPI + "^NPI||||||F|||" + objEncounter.Date_of_Service.ToString("yyyyMMddhhmm") + Environment.NewLine;

            //sMyHL7 += "OBX|2|NM|21612-7^Age Time Patient Reported^LN|" + humanAgeInYears + "|0|a^Years^UCUM|||||F" + Environment.NewLine;
            iObx = iObx + 1;

            if (humanAgeInYears > 11)
            {
                humanAgeInYears = UtilityManager.CalculateAge(objHuman.Birth_Date);
                sMyHL7 += "OBX|" + iObx + "|NM|21612-7^Age at Time Patient Reported^LN||" + humanAgeInYears + "|a^Year^UCUM|||||F" + Environment.NewLine;
            }
            else
                sMyHL7 += "OBX|" + iObx + "|NM|21612-7^Age at Time Patient Reported^LN||" + humanAgeInYears + "|mo^Month^UCUM|||||F" + Environment.NewLine;

            if (ClinicalSummary.ChiefComplaints != null && ClinicalSummary.ChiefComplaints.Count > 0)
            {
                for (int i = 0; i < ClinicalSummary.ChiefComplaints.Count; i++)
                {
                    iObx = iObx + 1;
                    //sMyHL7 += "OBX|3|CWE|8661-1^Chief Complaints Reported^LN||^^^^^^^^" + ClinicalSummary.ChiefComplaints[i].HPI_Value + "||||||F" + Environment.NewLine;
                    sMyHL7 += "OBX|" + iObx + "|TX|8661-1^Chief complaint^LN||" + ClinicalSummary.ChiefComplaints[i].HPI_Value + "||||||F" + Environment.NewLine;
                }

            }
            if (ClinicalSummary.Vitals.Count > 0)
            {
                var sWeight = (from w in ClinicalSummary.Vitals where w.Loinc_Observation == "Weight" select new { Humanweight = w.Value }).ToArray();
                var sHeight = (from h in ClinicalSummary.Vitals where h.Loinc_Observation == "Height" select new { Humanheight = h.Value }).ToArray();
                if (sHeight.Length > 0)
                {
                    if (sHeight[0].Humanheight != "")
                    {
                        iObx = iObx + 1;
                        sMyHL7 += "OBX|" + iObx + "|NM|8302-2^Height^LN||" + sHeight[0].Humanheight + "|[in_us]^inch^UCUM|||||F" + Environment.NewLine;
                    }
                }
                if (sWeight.Length > 0)
                {
                    if (sWeight[0].Humanweight != "")
                    {
                        iObx = iObx + 1;
                        sMyHL7 += "OBX|" + iObx + "|NM|3141-9^Weight^LN||" + sWeight[0].Humanweight + "|[lb_av]^pound^UCUM|||||F" + Environment.NewLine;
                    }
                }
            }
            if (ClinicalSummary.SocialHistory.Count > 0)
            {
                var social = (from s in ClinicalSummary.SocialHistory where s.Social_Info == "Smoking habit" select new { History_Description = s.Value, code = s.Recodes }).ToArray();
                if (social.Count() > 0)
                {
                    iObx = iObx + 1;
                    sMyHL7 += "OBX|" + iObx + "|CWE|72166-2^Tobacco Smoking Status^LN||" + social[0].code + "^" + social[0].History_Description + "^SCT||||||F" + Environment.NewLine;
                }
            }


            if (sMSH != "ADT^A03^ADT_A03")
            {

                if (ClinicalSummary.Assessment != null && ClinicalSummary.Assessment.Count > 0)
                {
                    int j = 1;
                    for (int i = 0; i < ClinicalSummary.Assessment.Count; i++)
                    {

                        //sMyHL7 += "DG1|" + j + "||" + ClinicalSummary.Assessment[i].ICD_9 + "^" + ClinicalSummary.Assessment[i].ICD_Description + "^I9CDX|||W" + Environment.NewLine;
                        //j++;
                        if (ClinicalSummary.Assessment[i].Assessment_Notes != "" || ClinicalSummary.Assessment[i].Primary_Diagnosis != "Y")
                        {
                            if (ClinicalSummary.Assessment[i].Version_Year == "ICD_10")
                                sMyHL7 += "DG1|" + j + "||" + ClinicalSummary.Assessment[i].ICD_9 + "^" + ClinicalSummary.Assessment[i].ICD_Description + "^" + "I10C" + "|||W" + Environment.NewLine;
                            else if (ClinicalSummary.Assessment[i].Version_Year == "ICD_9")
                                sMyHL7 += "DG1|" + j + "||" + ClinicalSummary.Assessment[i].ICD_9 + "^" + ClinicalSummary.Assessment[i].ICD_Description + "^" + "I9CDX" + "|||W" + Environment.NewLine;
                            j++;
                        }
                    }

                }
            }
            return sMyHL7;

            #region OLD
            //sMyHL7 = "MSH|^~\\&||" + objFacility.Fac_Name + "^" + objFacility.Fac_NPI + "^NPI|||" + currentDateTime.ToString("yyyyMMddhhmmss") + "||" + "ADT^A04^ADT_A01|" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + objHuman.Id.ToString() + "|P|2.3.1" + Environment.NewLine;
            //sMyHL7 += "EVN||" + currentDateTime.ToString("yyyyMMddhhmmss") + Environment.NewLine;
            //if (objHuman.Race != string.Empty)
            //{
            //    IList<UserLookup> lookupListRace = null;
            //    //Latha - 10 Aug 2011 - Start - AllLookups singleton
            //    lookupListRace = localFieldLookupManager.GetFieldLookupList(0, objHuman.Race);
            //    //Latha - 10 Aug 2011 - End
            //    if (lookupListRace != null && lookupListRace.Count > 0)
            //    {
            //        raceCode = lookupListRace[0].Value;
            //        if (raceCode != string.Empty)
            //        {
            //            raceCodeSys = "HL70005";
            //        }

            //    }
            //}
            //if (objHuman.Ethnicity != string.Empty)
            //{
            //    IList<UserLookup> lookupListEthincity = null;
            //    //Latha - 10 Aug 2011 - Start - AllLookups singleton
            //    lookupListEthincity = localFieldLookupManager.GetFieldLookupList(0, objHuman.Ethnicity);
            //    //Latha - 10 Aug 2011 - End
            //    if (lookupListEthincity != null && lookupListEthincity.Count > 0)
            //    {
            //        ethnicityCode = lookupListEthincity[0].Value;

            //    }
            //}
            //ethnicityCodeSys = "HL70189";
            //if (ethnicityCode == string.Empty)
            //{
            //    ethnicityCode = "U";
            //}
            //sMyHL7 += "PID|1||" + objHuman.Id.ToString() + "^^^^PI" + "||" + objHuman.First_Name + "^" + objHuman.Last_Name + "^" + objHuman.MI + "^" + objHuman.Suffix + "^" + objHuman.Prefix + "^^L|||" + objHuman.Sex.Substring(0, 1) + "||" + raceCode + "^" + objHuman.Race + "^" + raceCodeSys + "|^^^" + objHuman.State + "^" + objHuman.ZipCode
            //    + "^|||||||||||" + ethnicityCode + "^" + objHuman.Ethnicity + "^" + ethnicityCodeSys + Environment.NewLine;
            //sMyHL7 += "PV1||O|||||||||||||||||" + objEncounter.Id.ToString() + "^^^^VN" + "|||||||||||||||||||||||||" + objEncounter.Date_of_Service.ToString("yyyyMMddhhmmss") + "|" + Environment.NewLine;
            //if (ClinicalSummary.ChiefComplaints != null && ClinicalSummary.ChiefComplaints.Count > 0)
            //{
            //    IList<string> CCList = (from obj in ClinicalSummary.ChiefComplaints where obj.HPI_Element.ToUpper() == "CHIEF COMPLAINTS" select obj.HPI_Value).ToList<string>();
            //    ccDate = (from obj in ClinicalSummary.ChiefComplaints where obj.HPI_Element.ToUpper() == "CHIEF COMPLAINTS" select obj.Modified_Date_And_Time).Max();
            //    if (CCList != null)
            //    {
            //        foreach (string str in CCList)
            //        {
            //            if (sCC == string.Empty)
            //                sCC = str;
            //            else
            //                sCC += "," + str;
            //        }
            //    }
            //}
            //if (sCC != string.Empty)
            //{
            //    sMyHL7 += "PV2|||^" + sCC + Environment.NewLine;
            //}
            //DateTime now = DateTime.Today;
            //DateTime birthDate = objHuman.Birth_Date;
            //int years = now.Year - birthDate.Year;
            //if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
            //    --years;

            //sMyHL7 += "OBX|1|NM|21612-7^AGE TIME PATIENT REPORTED^LN||" + years.ToString() + "|A^YEAR^UCUM|||||F|||" + currentDateTime.ToString("yyyyMMddhhmmss") + Environment.NewLine;
            //int obxCount = 2;
            //PatientResults objTemperature = null;
            //PatientResults objPulseOximetry = null;
            //if (ClinicalSummary.Vitals.Count > 0)
            //{
            //    ulong maxId = Convert.ToUInt64((from obj in ClinicalSummary.Vitals select obj.Vital_Group_ID).Max());
            //    IList<PatientResults> list = (from obj in ClinicalSummary.Vitals where obj.Vital_Group_ID == maxId && (obj.Loinc_Observation.ToUpper() == "BODY TEMPERATURE" || obj.Loinc_Observation.ToUpper() == "PULSE OXIMETRY") && obj.Results_Type.ToUpper() == "VITALS" select obj).ToList<PatientResults>();
            //    if (list != null)
            //    {
            //        if (list.Count > 0)
            //        {
            //            objTemperature = list.First();
            //            objPulseOximetry = list.Last();
            //        }
            //    }
            //}
            //if (objTemperature != null && objTemperature.Value != string.Empty)
            //{
            //    sMyHL7 += "OBX|" + obxCount.ToString() + "|NM|11289-6^BODY TEMPERATURE:TEMP:ENCTRFRST:PATIENT:QN:^LN||" + objTemperature.Value + "|[degF]^FAHRENHEIT^UCUM|||||F|||" + UtilityManager.ConvertToLocal(objTemperature.Created_Date_And_Time).ToString("yyyyMMddhhmmss") + Environment.NewLine;
            //    obxCount += 1;
            //}
            //if (objPulseOximetry != null && objPulseOximetry.Value != string.Empty)
            //{
            //    sMyHL7 += "OBX|" + obxCount.ToString() + "|NM|59408-5^OXYGEN SATURATION:MFR:PT:BLDA:QN:PULSE OXIMETRY^LN||" + objPulseOximetry.Value + "|%^PERCENT^UCUM|||||F|||" + UtilityManager.ConvertToLocal(objPulseOximetry.Created_Date_And_Time).ToString("yyyyMMddhhmmss") + Environment.NewLine;
            //    obxCount += 1;
            //}
            //if (sCC != string.Empty && ccDate != DateTime.MinValue)
            //{
            //    sMyHL7 += "OBX|" + obxCount.ToString() + "|TS|11368-8^ILLNESS OR INJURY ONSET DATE AND TIME:TMSTP:PT:PATIENT:QN:^LN||" + UtilityManager.ConvertToLocal(ccDate).ToString("yyyyMMddhhmmss") + "||||||F|||" + UtilityManager.ConvertToLocal(objEncounter.Date_of_Service).ToString("yyyyMMddhhmmss") + Environment.NewLine;
            //    obxCount += 1;
            //}
            //sMyHL7 += "OBX|" + obxCount.ToString() + "|HD|SS001^TREATING FACILITY IDENTIFIER^PHINQUESTION||" + objFacility.Fac_Name + "^" + objFacility.Fac_NPI + "^NPI" + "||||||F|||" + currentDateTime.ToString("yyyyMMddhhmmss") + Environment.NewLine;
            //obxCount += 1;
            //sMyHL7 += "OBX|" + obxCount.ToString() + "|XAD|SS002^TREATING FACILITY LOCATION^PHINQUESTION||" + objFacility.Fac_Address1 + "^^" + objFacility.Fac_City + "^" + objFacility.Fac_State + "^" + objFacility.Fac_Zip + "||||||F|||" + currentDateTime.ToString("yyyyMMddhhmmss") + Environment.NewLine;
            //obxCount = 1;
            //IList<string> ICDCodes = new List<string>();
            //foreach (Assessment obj in ClinicalSummary.Assessment)
            //{
            //    sMyHL7 += "DG1|" + obxCount.ToString() + "||" + obj.ICD_9 + "^" + obj.Description + "^I9" + "|||F" + Environment.NewLine;
            //    ICDCodes.Add(obj.ICD_9);
            //    obxCount += 1;
            //}
            //foreach (ProblemList obj in ClinicalSummary.ProblemListing)
            //{
            //    if (obj.Is_Active.ToUpper() == "Y" && ICDCodes.Contains(obj.ICD_Code) == false)
            //    {
            //        sMyHL7 += "DG1|" + obxCount.ToString() + "||" + obj.ICD_Code + "^" + obj.Problem_Description + "^I9" + "|||F" + Environment.NewLine;
            //        obxCount += 1;
            //    }
            //}
            //return sMyHL7;
            #endregion
        }



    }
}
//=======
//﻿using System;
//using System.IO;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using Acurus.Capella.Core.DomainObjects;
//using Acurus.Capella.Core.DTO;
//using System.Windows.Forms;
//using Acurus.Capella.DataAccess.ManagerObjects;
////using Acurus.Capella.Proxy.LookupTables;
////using Acurus.Capella.Proxy;

//namespace Acurus.Capella.UI
//{
//    public partial class HL7Generator
//    {
//        UserLookupManager localFieldLookupManager = new UserLookupManager();

//        public XmlDocument CreateCCDXML(PhysicianLibrary Phy, FillClinicalSummary ClinicalSummary, string sPrintFileName)
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            XmlDocument xmlRef = new XmlDocument();

//            xmlRef.Load(Application.StartupPath + "\\CCDConfig.xml");
//            xmlDoc.Load(Application.StartupPath + "\\CCD_Sample.xml");
//            XmlNodeList xmlReqNode = null;
//            string sContent = string.Empty;

//            #region Patient
//            xmlReqNode = xmlDoc.GetElementsByTagName("id");
//            xmlReqNode[1].Attributes[0].Value = ClinicalSummary.Medical_Record_Number;
//            xmlReqNode = xmlDoc.GetElementsByTagName("given");
//            xmlReqNode[0].InnerText = ClinicalSummary.First_Name;
//            xmlReqNode[1].InnerText = ClinicalSummary.MI;
//            xmlReqNode = xmlDoc.GetElementsByTagName("family");
//            xmlReqNode[0].InnerText = ClinicalSummary.Last_Name;
//            xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
//            xmlReqNode[0].InnerText = ClinicalSummary.Prefix;
//            xmlReqNode = xmlDoc.GetElementsByTagName("suffix");
//            xmlReqNode[0].InnerText = ClinicalSummary.Suffix;
//            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
//            xmlReqNode[0].InnerText = ClinicalSummary.Street_Address1;
//            xmlReqNode = xmlDoc.GetElementsByTagName("city");
//            xmlReqNode[0].InnerText = ClinicalSummary.City;
//            xmlReqNode = xmlDoc.GetElementsByTagName("state");
//            xmlReqNode[0].InnerText = ClinicalSummary.State;
//            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
//            xmlReqNode[0].InnerText = ClinicalSummary.ZipCode;
//            xmlReqNode = xmlDoc.GetElementsByTagName("administrativeGenderCode");
//            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Sex.Substring(0, 1).ToUpper();
//            xmlReqNode[0].Attributes[1].Value = ClinicalSummary.Sex;
//            xmlReqNode = xmlDoc.GetElementsByTagName("birthTime");
//            xmlReqNode[0].Attributes[0].Value = ClinicalSummary.Birth_Date.ToString("dd-MMM-yyyy");
//            #endregion

//            #region Physician
//            xmlReqNode = xmlDoc.GetElementsByTagName("given");
//            xmlReqNode[2].InnerText = Phy.PhyFirstName;
//            xmlReqNode[3].InnerText = Phy.PhyMiddleName;
//            xmlReqNode = xmlDoc.GetElementsByTagName("family");
//            xmlReqNode[1].InnerText = Phy.PhyLastName;
//            xmlReqNode = xmlDoc.GetElementsByTagName("prefix");
//            xmlReqNode[1].InnerText = Phy.PhyPrefix;
//            xmlReqNode = xmlDoc.GetElementsByTagName("suffix");
//            xmlReqNode[1].InnerText = Phy.PhySuffix;
//            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
//            xmlReqNode[1].InnerText = Phy.PhyAddress1;
//            xmlReqNode = xmlDoc.GetElementsByTagName("city");
//            xmlReqNode[1].InnerText = Phy.PhyCity;
//            xmlReqNode = xmlDoc.GetElementsByTagName("state");
//            xmlReqNode[1].InnerText = Phy.PhyState;
//            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
//            xmlReqNode[1].InnerText = Phy.PhyZip;
//            #endregion

//            //XmlNode root = xmlDoc.DocumentElement;
//            XmlNodeList xmlList = xmlDoc.GetElementsByTagName("ClinicalDocument");
//            XmlNode root = xmlList[0];

//            XmlElement Parent = null;
//            XmlElement Child = null;
//            XmlElement MyParent = null;

//            Parent = (XmlElement)root;

//            Child = xmlDoc.CreateElement("component", "urn:hl7-org:v3");
//            Child.RemoveAllAttributes();
//            Parent.AppendChild(Child);
//            Parent = Child;
//            Child = xmlDoc.CreateElement("structuredBody", "urn:hl7-org:v3");
//            Parent.AppendChild(Child);
//            Parent = Child;
//            MyParent = Parent;

//            int iTableCount = 0;


//            #region Problems
//            if (ClinicalSummary.ProblemListing.Count != 0)
//            {
//                XmlElement TempElement = null;

//                Child = xmlDoc.CreateElement("component", "urn:hl7-org:v3");
//                MyParent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("section", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                TempElement = Child;
//                Child = xmlDoc.CreateElement("templateId", "urn:hl7-org:v3");
//                Child.SetAttribute("root", "2.16.840.1.113883.3.88.11.83.103");
//                Child.SetAttribute("assigningAuthorityName", "HITSP/C83");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("templateId", "urn:hl7-org:v3");
//                Child.SetAttribute("root", "1.3.6.1.4.1.19376.1.5.3.1.3.6");
//                Child.SetAttribute("assigningAuthorityName", "IHE PCC");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("templateId", "urn:hl7-org:v3");
//                Child.SetAttribute("root", "2.16.840.1.113883.10.20.1.11");
//                Child.SetAttribute("assigningAuthorityName", "HL7 CCD");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("code", "urn:hl7-org:v3");
//                Child.SetAttribute("code", "11450-4");
//                Child.SetAttribute("codeSystem", "2.16.840.1.113883.6.1");
//                Child.SetAttribute("codeSystemName", "ICD-9-CM");
//                Child.SetAttribute("displayName", "Problem list");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("title", "urn:hl7-org:v3");
//                Child.InnerText = "Problems";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("text", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("table", "urn:hl7-org:v3");
//                Child.SetAttribute("border", "1");
//                Child.SetAttribute("width", "100%");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("thead", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "ICD-9 Code";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Problem Name";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Problem Status";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Date Diagnosed";
//                Parent.AppendChild(Child);

//                for (int i = 0; i < ClinicalSummary.ProblemListing.Count; i++)
//                {
//                    Parent = (XmlElement)xmlDoc.GetElementsByTagName("table")[iTableCount];
//                    Child = xmlDoc.CreateElement("tbody", "urn:hl7-org:v3");
//                    Parent.AppendChild(Child);
//                    Parent = Child;
//                    Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//                    Parent.AppendChild(Child);
//                    Parent = Child;
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.ProblemListing[i].ICD_Code;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.ProblemListing[i].Problem_Description;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.ProblemListing[i].Status;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.ProblemListing[i].Date_Diagnosed;
//                    Parent.AppendChild(Child);
//                }
//                Child = xmlDoc.CreateElement("DummyProblem", "urn:hl7-org:v3");
//                Child.InnerText = "";
//                TempElement.AppendChild(Child);


//                //TextReader sr = new StreamReader(@Application.StartupPath + "\\ProblemEntry.txt");

//                //for (int i = 0; i < ClinicalSummary.ProblemListing.Count; i++)
//                //{
//                //    XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
//                //    frag.InnerXml = @sr.ReadToEnd();
//                //    TempElement.AppendChild(frag);
//                //}

//                iTableCount = iTableCount + 1;
//            }
//            #endregion

//            #region Medications
//            if (ClinicalSummary.Medication.Count != 0)
//            {
//                XmlElement TempElement = null;
//                Child = xmlDoc.CreateElement("component", "urn:hl7-org:v3");
//                MyParent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("section", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                TempElement = Child;
//                Child = xmlDoc.CreateElement("templateId", "urn:hl7-org:v3");
//                Child.SetAttribute("root", "2.16.840.1.113883.10.20.1.8");
//                Child.SetAttribute("assigningAuthorityName", "HL7 CCD");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("code", "urn:hl7-org:v3");
//                Child.SetAttribute("code", "10160-0");
//                Child.SetAttribute("codeSystem", "2.16.840.1.113883.6.1");
//                Child.SetAttribute("codeSystemName", "LOINC");
//                Child.SetAttribute("displayName", "History of medication use");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("title", "urn:hl7-org:v3");
//                Child.InnerText = "Medications";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("text", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("table", "urn:hl7-org:v3");
//                Child.SetAttribute("border", "1");
//                Child.SetAttribute("width", "100%");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("thead", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "RXNorm Code";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Generic Name";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Brand Name";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Strength";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Dose";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Route of Administration";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Frequency";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Date Started";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Status";
//                Parent.AppendChild(Child);

//                for (int i = 0; i < ClinicalSummary.Medication.Count; i++)
//                {
//                    Parent = (XmlElement)xmlDoc.GetElementsByTagName("table")[iTableCount];
//                    Child = xmlDoc.CreateElement("tbody", "urn:hl7-org:v3");
//                    Parent.AppendChild(Child);
//                    Parent = Child;
//                    Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//                    Parent.AppendChild(Child);
//                    Parent = Child;
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.RXNormList[i].ToString();
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Medication[i].Generic_Name;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Medication[i].Brand_Name;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Medication[i].Strength;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Medication[i].Dose;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Medication[i].Route;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Medication[i].Dose_Other;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Medication[i].Start_Date.ToString("dd-MMM-yyyy");
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = "Active";
//                    Parent.AppendChild(Child);
//                }

//                Child = xmlDoc.CreateElement("DummyMedication", "urn:hl7-org:v3");
//                Child.InnerText = "";
//                TempElement.AppendChild(Child);

//                //TextReader sr=   new StreamReader(@Application.StartupPath + "\\MedicationEntry.txt");

//                //for (int i = 0; i < ClinicalSummary.Allergy.Count; i++)
//                //{
//                //    XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
//                //    frag.InnerXml = @sr.ReadToEnd();
//                //    TempElement.AppendChild(frag);
//                //}

//                iTableCount = iTableCount + 1;
//            }
//            #endregion

//            #region Allergies
//            if (ClinicalSummary.Allergy.Count != 0)
//            {
//                XmlElement TempElement = null;

//                Child = xmlDoc.CreateElement("component", "urn:hl7-org:v3");
//                MyParent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("section", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                TempElement = Child;
//                Child = xmlDoc.CreateElement("templateId", "urn:hl7-org:v3");
//                Child.SetAttribute("root", "2.16.840.1.113883.10.20.1.2");
//                Child.SetAttribute("assigningAuthorityName", "HL7 CCD");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("code", "urn:hl7-org:v3");
//                Child.SetAttribute("code", "48765-2");
//                Child.SetAttribute("codeSystem", "2.16.840.1.113883.6.1");
//                Child.SetAttribute("codeSystemName", "LOINC");
//                Child.SetAttribute("displayName", "Allergies");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("title", "urn:hl7-org:v3");
//                Child.InnerText = "Allergies and Adverse Reactions";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("text", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("table", "urn:hl7-org:v3");
//                Child.SetAttribute("border", "1");
//                Child.SetAttribute("width", "100%");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("thead", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "SNOMED Allergy Type Code";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Allergy";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Allergy NDC code";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Reaction";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Adverse Event Date";
//                Parent.AppendChild(Child);

//                for (int i = 0; i < ClinicalSummary.Allergy.Count; i++)
//                {
//                    Parent = (XmlElement)xmlDoc.GetElementsByTagName("table")[iTableCount];
//                    Child = xmlDoc.CreateElement("tbody", "urn:hl7-org:v3");
//                    Parent.AppendChild(Child);
//                    Parent = Child;
//                    Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//                    Parent.AppendChild(Child);
//                    Parent = Child;
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = "416098002";
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Allergy[i].Allergy_Name;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Allergy[i].NDC_ID;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Allergy[i].Reaction;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.Allergy[i].OnsetDate.ToString("dd-MMM-yyyy");
//                    Parent.AppendChild(Child);
//                }
//                Child = xmlDoc.CreateElement("DummyAllergy", "urn:hl7-org:v3");
//                Child.InnerText = "";
//                TempElement.AppendChild(Child);

//                //TextReader sr = new StreamReader(@Application.StartupPath + "\\AllergyEntry.txt");

//                //for (int i = 0; i < ClinicalSummary.Medication.Count; i++)
//                //{
//                //    XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
//                //    frag.InnerXml = @sr.ReadToEnd();
//                //    TempElement.AppendChild(frag);
//                //}

//                iTableCount = iTableCount + 1;
//            }
//            #endregion

//            //#region TestResults
//            //if (ClinicalSummary.ResultList.Count != 0 || ClinicalSummary.ResultSubEntry.Count != 0)
//            //{
//            //    XmlElement TempElement = null;

//            //    Child = xmlDoc.CreateElement("component", "urn:hl7-org:v3");
//            //    MyParent.AppendChild(Child);
//            //    Parent = Child;
//            //    Child = xmlDoc.CreateElement("section", "urn:hl7-org:v3");
//            //    Parent.AppendChild(Child);
//            //    Parent = Child;
//            //    TempElement = Child;
//            //    Child = xmlDoc.CreateElement("templateId", "urn:hl7-org:v3");
//            //    Child.SetAttribute("root", "2.16.840.1.113883.10.20.1.14");
//            //    Child.SetAttribute("assigningAuthorityName", "HL7 CCD");
//            //    Parent.AppendChild(Child);
//            //    Child = xmlDoc.CreateElement("code", "urn:hl7-org:v3");
//            //    Child.SetAttribute("code", "30954-2");
//            //    Child.SetAttribute("codeSystem", "2.16.840.1.113883.6.1");
//            //    Child.SetAttribute("codeSystemName", "LOINC");
//            //    Child.SetAttribute("displayName", "Results");
//            //    Parent.AppendChild(Child);
//            //    Child = xmlDoc.CreateElement("title", "urn:hl7-org:v3");
//            //    Child.InnerText = "Test Results";
//            //    Parent.AppendChild(Child);
//            //    Child = xmlDoc.CreateElement("text", "urn:hl7-org:v3");
//            //    Parent.AppendChild(Child);
//            //    Parent = Child;
//            //    Child = xmlDoc.CreateElement("paragraph", "urn:hl7-org:v3");
//            //    Child.InnerText = "Lab Results";
//            //    Parent.AppendChild(Child);
//            //    Child = xmlDoc.CreateElement("table", "urn:hl7-org:v3");
//            //    Child.SetAttribute("border", "1");
//            //    Child.SetAttribute("width", "100%");
//            //    Parent.AppendChild(Child);
//            //    Parent = Child;
//            //    Child = xmlDoc.CreateElement("thead", "urn:hl7-org:v3");
//            //    Parent.AppendChild(Child);
//            //    Parent = Child;
//            //    Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//            //    Parent.AppendChild(Child);
//            //    Parent = Child;
//            //    Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//            //    Child.InnerText = "LOINC code";
//            //    Parent.AppendChild(Child);
//            //    Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//            //    Child.InnerText = "Test Name";
//            //    Parent.AppendChild(Child);
//            //    Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//            //    Child.InnerText = "Result";
//            //    Parent.AppendChild(Child);
//            //    Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//            //    Child.InnerText = "Abnormal Flag";
//            //    Parent.AppendChild(Child);
//            //    Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//            //    Child.InnerText = "Date Performed";
//            //    Parent.AppendChild(Child);

//            //    for (int i = 0; i < ClinicalSummary.ResultList.Count; i++)
//            //    {
//            //        Parent = (XmlElement)xmlDoc.GetElementsByTagName("table")[iTableCount];
//            //        Child = xmlDoc.CreateElement("tbody", "urn:hl7-org:v3");
//            //        Parent.AppendChild(Child);
//            //        Parent = Child;
//            //        Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//            //        Parent.AppendChild(Child);
//            //        Parent = Child;
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        Child.InnerText = ClinicalSummary.ResultList[i].OBX_Observation_Identifier;
//            //        Parent.AppendChild(Child);
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        Child.InnerText = ClinicalSummary.ResultList[i].OBX_Observation_Text;
//            //        Parent.AppendChild(Child);
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        Child.InnerText = ClinicalSummary.ResultList[i].OBX_Observation_Value;
//            //        Parent.AppendChild(Child);
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        Child.InnerText = ClinicalSummary.ResultList[i].OBX_Abnormal_Flag;
//            //        Parent.AppendChild(Child);
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        if (ClinicalSummary.ResultList[i].OBX_Date_And_Time_Of_Observation != string.Empty)
//            //        {
//            //            Child.InnerText = ClinicalSummary.ResultList[i].OBX_Date_And_Time_Of_Observation.Substring(0, 8);
//            //        }
//            //        else
//            //        {
//            //            Child.InnerText = string.Empty;
//            //        }
//            //        Parent.AppendChild(Child);
//            //    }
//            //    for (int i = 0; i < ClinicalSummary.ResultSubEntry.Count; i++)
//            //    {
//            //        Parent = (XmlElement)xmlDoc.GetElementsByTagName("table")[iTableCount];
//            //        Child = xmlDoc.CreateElement("tbody", "urn:hl7-org:v3");
//            //        Parent.AppendChild(Child);
//            //        Parent = Child;
//            //        Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//            //        Parent.AppendChild(Child);
//            //        Parent = Child;
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        Child.InnerText = ClinicalSummary.ResultSubEntry[i].Loinc_Num;
//            //        Parent.AppendChild(Child);
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        Child.InnerText = ClinicalSummary.ResultSubEntry[i].Component;
//            //        Parent.AppendChild(Child);
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        Child.InnerText = ClinicalSummary.ResultSubEntry[i].Result + ClinicalSummary.ResultSubEntry[i].Units;
//            //        Parent.AppendChild(Child);
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        Child.InnerText = ClinicalSummary.ResultSubEntry[i].Flag;
//            //        Parent.AppendChild(Child);
//            //        Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//            //        Child.InnerText = ClinicalSummary.ResultSubEntry[i].Created_Date_And_Time.ToString().Substring(0, 8);
//            //        Parent.AppendChild(Child);
//            //    }

//            //    Child = xmlDoc.CreateElement("DummyResult", "urn:hl7-org:v3");
//            //    Child.InnerText = "";
//            //    TempElement.AppendChild(Child);

//            //    //TextReader sr = new StreamReader(@Application.StartupPath + "\\ResultEntry.txt");

//            //    //for (int i = 0; i < ClinicalSummary.ResultList.Count; i++)
//            //    //{
//            //    //    XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
//            //    //    frag.InnerXml = @sr.ReadToEnd();
//            //    //    TempElement.AppendChild(frag);
//            //    //}


//            //    iTableCount = iTableCount + 1;
//            //}
//            //#endregion

//            #region Procedures
//            if (ClinicalSummary.InHouseProcList.Count != 0)
//            {
//                XmlElement TempElement = null;
//                Child = xmlDoc.CreateElement("component", "urn:hl7-org:v3");
//                MyParent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("section", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                TempElement = Child;
//                Child = xmlDoc.CreateElement("templateId", "urn:hl7-org:v3");
//                Child.SetAttribute("root", "2.16.840.1.113883.10.20.1.12");
//                Child.SetAttribute("assigningAuthorityName", "HL7 CCD");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("code", "urn:hl7-org:v3");
//                Child.SetAttribute("code", "47519-4");
//                Child.SetAttribute("codeSystem", "2.16.840.1.113883.6.1");
//                Child.SetAttribute("codeSystemName", "LOINC");
//                Child.SetAttribute("displayName", "List of surgeries");
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("title", "urn:hl7-org:v3");
//                Child.InnerText = "Procedures";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("text", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("table", "urn:hl7-org:v3");
//                Child.SetAttribute("border", "1");
//                Child.SetAttribute("width", "100%");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("thead", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//                Parent.AppendChild(Child);
//                Parent = Child;
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "CPT Code";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Procedure";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Status";
//                Parent.AppendChild(Child);
//                Child = xmlDoc.CreateElement("th", "urn:hl7-org:v3");
//                Child.InnerText = "Date Performed";
//                Parent.AppendChild(Child);

//                for (int i = 0; i < ClinicalSummary.InHouseProcList.Count; i++)
//                {
//                    Parent = (XmlElement)xmlDoc.GetElementsByTagName("table")[iTableCount];
//                    Child = xmlDoc.CreateElement("tbody", "urn:hl7-org:v3");
//                    Parent.AppendChild(Child);
//                    Parent = Child;
//                    Child = xmlDoc.CreateElement("tr", "urn:hl7-org:v3");
//                    Parent.AppendChild(Child);
//                    Parent = Child;
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.InHouseProcList[i].Procedure_Code;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.InHouseProcList[i].Procedure_Code_Description;
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = "Completed";
//                    Parent.AppendChild(Child);
//                    Child = xmlDoc.CreateElement("td", "urn:hl7-org:v3");
//                    Child.InnerText = ClinicalSummary.InHouseProcList[i].Created_Date_And_Time.ToString("dd-MMM-yyyy");
//                    Parent.AppendChild(Child);
//                }
//                Child = xmlDoc.CreateElement("DummyProcedure", "urn:hl7-org:v3");
//                Child.InnerText = "";
//                TempElement.AppendChild(Child);

//                TextReader sr = new StreamReader(@Application.StartupPath + "\\ProcedureEntry.txt");

//                for (int i = 0; i < ClinicalSummary.InHouseProcList.Count; i++)
//                {
//                    XmlDocumentFragment frag = xmlDoc.CreateDocumentFragment();
//                    try
//                    {
//                        frag.InnerXml = @sr.ReadToEnd();
//                    }
//                    catch
//                    {

//                    }
//                    TempElement.AppendChild(frag);
//                }

//                iTableCount = iTableCount + 1;
//            }
//            #endregion

//            xmlDoc.Save(sPrintFileName);
//            TextReader tex = new StreamReader(sPrintFileName);
//            string sTotal = tex.ReadToEnd();
//            tex.Close();
//            tex.Dispose();

//            TextWriter texWriter = new StreamWriter(sPrintFileName.Replace(".xml", "_New.xml"));
//            TextReader srProb = new StreamReader(@Application.StartupPath + "\\ProblemEntry.txt");
//            TextReader srMed = new StreamReader(@Application.StartupPath + "\\MedicationEntry.txt");
//            TextReader srAller = new StreamReader(@Application.StartupPath + "\\AllergyEntry.txt");
//            TextReader srResult = new StreamReader(@Application.StartupPath + "\\ResultEntry.txt");
//            TextReader srProc = new StreamReader(@Application.StartupPath + "\\ProcedureEntry.txt");
//            string sNewProblem = srProb.ReadToEnd();
//            string sNewMedication = srMed.ReadToEnd();
//            string sNewAller = srAller.ReadToEnd();
//            string sNewResult = srResult.ReadToEnd();
//            string sNewProc = srProc.ReadToEnd();

//            sTotal = sTotal.Replace("<DummyProblem>\r\n          </DummyProblem>", sNewProblem);
//            sTotal = sTotal.Replace("<DummyMedication>\r\n          </DummyMedication>", sNewMedication);
//            sTotal = sTotal.Replace("<DummyAllergy>\r\n          </DummyAllergy>", sNewAller);
//            sTotal = sTotal.Replace("<DummyResult>\r\n          </DummyResult>", sNewResult);
//            sTotal = sTotal.Replace("<DummyProcedure>\r\n          </DummyProcedure>", sNewProc);

//            texWriter.WriteLine(sTotal);

//            texWriter.Close();
//            texWriter.Dispose();
//            XmlDocument xmlFinal = new XmlDocument();
//            xmlFinal.Load(sPrintFileName.Replace(".xml", "_New.xml"));
//            xmlFinal.Save(sPrintFileName.Replace(".xml", "_New.xml"));

//            return xmlDoc;
//        }

//        public string CreateImmunizationRegistry(Human hn, PhysicianLibrary Phy, FillClinicalSummary ClinicalSummary, string sPrintFileName)
//        {
//            string sResult = string.Empty;

//            sResult = "MSH|^~\\&|EHR Application|EHR Facility|PH Application|PH Facility|||VXU^V04^VXU_V04|NIST-100929111456141|P|2.3.1";

//            sResult = sResult + "\n" + "PID|||9817566735^^^MPI&2.16.840.1.113883.19.3.2.1&ISO^MR||";

//            sResult = sResult + hn.Last_Name + "^" + hn.First_Name + "||" + hn.Birth_Date.ToString("yyyyMMdd") + "|" + hn.Sex.Substring(0, 1) + "||2106-3^" + hn.Race + "^HL70005|" + hn.Street_Address1 + "^^" + hn.City + "^" + hn.State + "^" + hn.ZipCode + "^^M";

//            try
//            {
//                sResult = sResult + "||^PRN^^^^" + hn.Home_Phone_No.Substring(1, 3) + "^" + hn.Home_Phone_No.Substring(6, 3) + hn.Home_Phone_No.Substring(10, 4) + "|||||||||N^Not Hispanic or Latino^HL70189";
//            }
//            catch
//            {
//                sResult = sResult + "||^PRN^^^^" + "" + "^" + "" + "" + "|||||||||N^Not Hispanic or Latino^HL70189";
//            }

//            for (int i = 0; i < ClinicalSummary.ImmunizationList.Count; i++)
//            {
//                sResult = sResult + "\n" + "RXA|" + "0|1|" + DateTime.Now.ToString("yyyyMMddhhmm") + "|" +
//                    DateTime.Now.ToString("yyyyMMddhhmm") + "|" + ClinicalSummary.ImmunizationList[i].CVX_Code +
//                    "^" + ClinicalSummary.ImmunizationList[i].CVX_Code_Description + "^CVX|" +
//                    ClinicalSummary.ImmunizationList[i].Administered_Amount + "|" +
//                    ClinicalSummary.ImmunizationList[i].Administered_Unit_Identifier + "^" +
//                    ClinicalSummary.ImmunizationList[i].Administered_Unit +
//                    "^ISO+||||||||" + ClinicalSummary.ImmunizationList[i].Lot_Number + "||" +
//                    ClinicalSummary.ImmunizationList[i].MVX_Code + "^" +
//                    ClinicalSummary.ImmunizationList[i].Manufacturer + "^MVX||||A";
//            }

//            return sResult;
//        }

//        public string CreateHL7ForSyndromicSurveillance(Encounter objEncounter, Human objHuman, FacilityLibrary objFacility, FillClinicalSummary ClinicalSummary)
//        {
//            string sMyHL7 = string.Empty;
//            DateTime currentDateTime = DateTime.Now;
//            //Latha - 10 Aug 2011 - Start - AllLookups singleton
//            //AllLookups allLookups = new AllLookups();
//            //Latha - 10 Aug 2011 - End
//            IList<FieldLookup> lookupList;
//            DateTime ccDate = DateTime.MinValue;
//            string raceCode = string.Empty, raceCodeSys = string.Empty;
//            string ethnicityCode = string.Empty, ethnicityCodeSys = string.Empty;
//            string sCC = string.Empty;
//            sMyHL7 = "MSH|^~\\&||" + objFacility.Fac_Name + "^" + objFacility.Fac_NPI + "^NPI|||" + currentDateTime.ToString("yyyyMMddhhmmss") + "||" + "ADT^A04^ADT_A01|" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + objHuman.Id.ToString() + "|P|2.3.1" + Environment.NewLine;
//            sMyHL7 += "EVN||" + currentDateTime.ToString("yyyyMMddhhmmss") + Environment.NewLine;
//            if (objHuman.Race != string.Empty)
//            {
//                IList<UserLookup> lookupListRace = null;
//                //Latha - 10 Aug 2011 - Start - AllLookups singleton
//                lookupListRace = localFieldLookupManager.GetFieldLookupList(0, objHuman.Race);
//                //Latha - 10 Aug 2011 - End
//                if (lookupListRace != null && lookupListRace.Count > 0)
//                {
//                    raceCode = lookupListRace[0].Value;
//                    if (raceCode != string.Empty)
//                    {
//                        raceCodeSys = "HL70005";
//                    }

//                }
//            }
//            if (objHuman.Ethnicity != string.Empty)
//            {
//                IList<UserLookup> lookupListEthincity= null;
//                //Latha - 10 Aug 2011 - Start - AllLookups singleton
//                lookupListEthincity = localFieldLookupManager.GetFieldLookupList(0, objHuman.Ethnicity);
//                //Latha - 10 Aug 2011 - End
//                if (lookupListEthincity != null && lookupListEthincity.Count > 0)
//                {
//                    ethnicityCode = lookupListEthincity[0].Value;              

//                }
//            }
//            ethnicityCodeSys = "HL70189";
//            if (ethnicityCode == string.Empty)
//            {
//                ethnicityCode = "U";
//            }
//            sMyHL7 += "PID|1||" + objHuman.Id.ToString() + "^^^^PI" + "||"+objHuman.First_Name+"^"+objHuman.Last_Name+"^"+objHuman.MI+"^"+objHuman.Suffix+"^"+objHuman.Prefix+"^^L|||" + objHuman.Sex.Substring(0, 1) + "||" + raceCode + "^" + objHuman.Race + "^" + raceCodeSys + "|^^^" + objHuman.State + "^" + objHuman.ZipCode
//                + "^|||||||||||" + ethnicityCode + "^" + objHuman.Ethnicity + "^" + ethnicityCodeSys + Environment.NewLine;
//            sMyHL7 += "PV1||O|||||||||||||||||" + objEncounter.Id.ToString() + "^^^^VN" + "|||||||||||||||||||||||||" + objEncounter.Date_of_Service.ToString("yyyyMMddhhmmss")+"|" + Environment.NewLine;
//            if (ClinicalSummary.ChiefComplaints != null && ClinicalSummary.ChiefComplaints.Count > 0)
//            {
//                IList<string> CCList = (from obj in ClinicalSummary.ChiefComplaints where obj.HPI_Element.ToUpper() == "CHIEF COMPLAINTS" select obj.HPI_Value).ToList<string>();
//                ccDate = (from obj in ClinicalSummary.ChiefComplaints where obj.HPI_Element.ToUpper() == "CHIEF COMPLAINTS" select obj.Modified_Date_And_Time).Max();
//                if (CCList != null)
//                {
//                    foreach (string str in CCList)
//                    {
//                        if (sCC == string.Empty)
//                            sCC = str;
//                        else
//                            sCC += "," + str;
//                    }
//                }
//            }
//            if (sCC != string.Empty)
//            {
//                sMyHL7 += "PV2|||^" + sCC + Environment.NewLine;
//            }
//            DateTime now = DateTime.Today;
//            DateTime birthDate = objHuman.Birth_Date;
//            int years = now.Year - birthDate.Year;
//            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
//                --years;

//            sMyHL7 += "OBX|1|NM|21612-7^AGE TIME PATIENT REPORTED^LN||" + years.ToString() + "|A^YEAR^UCUM|||||F|||" + currentDateTime.ToString("yyyyMMddhhmmss") + Environment.NewLine;
//            int obxCount = 2;
//            PatientResults objTemperature = null;
//            PatientResults objPulseOximetry = null;
//            if (ClinicalSummary.Vitals.Count > 0)
//            {
//                ulong maxId = Convert.ToUInt64((from obj in ClinicalSummary.Vitals select obj.Vital_Group_ID).Max());
//                IList<PatientResults> list = (from obj in ClinicalSummary.Vitals where obj.Vital_Group_ID == maxId && (obj.Loinc_Observation.ToUpper() == "BODY TEMPERATURE" || obj.Loinc_Observation.ToUpper() == "PULSE OXIMETRY") && obj.Results_Type.ToUpper()=="VITALS" select obj).ToList<PatientResults>();
//                if (list != null)
//                {
//                    if (list.Count > 0)
//                    {
//                        objTemperature = list.First();
//                        objPulseOximetry = list.Last();
//                    }
//                }
//            }
//            if (objTemperature != null && objTemperature.Value!=string.Empty)
//            {
//                sMyHL7 += "OBX|" + obxCount.ToString() + "|NM|11289-6^BODY TEMPERATURE:TEMP:ENCTRFRST:PATIENT:QN:^LN||" + objTemperature.Value + "|[degF]^FAHRENHEIT^UCUM|||||F|||" + UtilityManager.ConvertToLocal(objTemperature.Created_Date_And_Time).ToString("yyyyMMddhhmmss") + Environment.NewLine;
//                obxCount += 1;
//            }
//            if (objPulseOximetry != null && objPulseOximetry.Value != string.Empty)
//            {
//                sMyHL7 += "OBX|" + obxCount.ToString() + "|NM|59408-5^OXYGEN SATURATION:MFR:PT:BLDA:QN:PULSE OXIMETRY^LN||" + objPulseOximetry.Value + "|%^PERCENT^UCUM|||||F|||" + UtilityManager.ConvertToLocal(objPulseOximetry.Created_Date_And_Time).ToString("yyyyMMddhhmmss") + Environment.NewLine;
//                obxCount += 1;
//            }
//            if (sCC != string.Empty && ccDate!=DateTime.MinValue)
//            {
//                sMyHL7 += "OBX|" + obxCount.ToString() + "|TS|11368-8^ILLNESS OR INJURY ONSET DATE AND TIME:TMSTP:PT:PATIENT:QN:^LN||" + UtilityManager.ConvertToLocal(ccDate).ToString("yyyyMMddhhmmss") + "||||||F|||" + UtilityManager.ConvertToLocal(objEncounter.Date_of_Service).ToString("yyyyMMddhhmmss") + Environment.NewLine;
//                obxCount += 1;
//            }
//            sMyHL7 += "OBX|" + obxCount.ToString() + "|HD|SS001^TREATING FACILITY IDENTIFIER^PHINQUESTION||" + objFacility.Fac_Name + "^" + objFacility.Fac_NPI + "^NPI" + "||||||F|||" + currentDateTime.ToString("yyyyMMddhhmmss") + Environment.NewLine;
//            obxCount += 1;
//            sMyHL7 += "OBX|" + obxCount.ToString() + "|XAD|SS002^TREATING FACILITY LOCATION^PHINQUESTION||" + objFacility.Fac_Address1 + "^^" + objFacility.Fac_City + "^" + objFacility.Fac_State + "^" + objFacility.Fac_Zip + "||||||F|||" + currentDateTime.ToString("yyyyMMddhhmmss") + Environment.NewLine;
//            obxCount = 1;
//            IList<string> ICDCodes = new List<string>();
//            foreach (Assessment obj in ClinicalSummary.Assessment)
//            {
//                sMyHL7 += "DG1|" + obxCount.ToString() + "||" + obj.ICD_9 + "^" + obj.Description + "^I9" + "|||F" + Environment.NewLine;
//                ICDCodes.Add(obj.ICD_9);
//                obxCount += 1;
//            }
//            foreach (ProblemList obj in ClinicalSummary.ProblemListing)
//            {
//                if (obj.Is_Active.ToUpper() == "Y" && ICDCodes.Contains(obj.ICD_Code) == false)
//                {
//                    sMyHL7 += "DG1|" + obxCount.ToString() + "||" + obj.ICD_Code + "^" + obj.Problem_Description + "^I9" + "|||F" + Environment.NewLine;
//                    obxCount += 1;
//                }
//            }
//            return sMyHL7;
//        }

//    }
//}
//>>>>>>> 1.1.2.1
