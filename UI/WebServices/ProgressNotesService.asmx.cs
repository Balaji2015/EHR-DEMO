using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using System.Xml;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Web.Services.Protocols;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for ProgressNotesJsonService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ProgressNotesService : System.Web.Services.WebService
    {

        
        [WebMethod]
        public string LoadProgressNotes(string sHumanID, string sEncounterID)
        {
            string sJson = string.Empty;
            WFObjectManager wfObjMngr = new WFObjectManager();
            try
            {
                if (sEncounterID == "")
                {
                    sJson = "{\"EncounterID\":" + sEncounterID + ",\"status\":\"ValidationError\",\"ErrorDescription\":\"EncounterID is not valid. Cannot generate progress note.\"}";
                    return sJson;
                }
                WFObject DocumentationWfObject = wfObjMngr.GetByObjectSystemId(Convert.ToUInt64(sEncounterID), "DOCUMENTATION");

                if (DocumentationWfObject != null && DocumentationWfObject.Current_Process == string.Empty)
                {
                    DocumentationWfObject = wfObjMngr.GetWfObjArchiveByObjectSystemId(Convert.ToUInt64(sEncounterID), "DOCUMENTATION");
                }

                if (DocumentationWfObject == null)
                {
                    sJson = "{\"EncounterID\":" + sEncounterID + ",\"status\":\"ValidationError\",\"ErrorDescription\":\"EncounterID is not present in DB. Cannot generate progress note.\"}";
                    return sJson;
                }

                if (DocumentationWfObject.Current_Process != "DOCUMENT_COMPLETE")
                {
                    sJson = "{\"EncounterID\":" + sEncounterID + ",\"status\":\"ValidationError\",\"ErrorDescription\":\"Encounter is not in DOCUMENT_COMPLETE. Cannot generate progress note.\"}";
                    return sJson;
                }
                sJson = GenerateJsonNotes(sHumanID, sEncounterID);
            }
            catch (Exception ex)
            {
                sJson = "{\"EncounterID\":" + sEncounterID + ",\"status\":\"Error\",\"ErrorDescription\":\"Error in processing the request. " + ex.Message + "\"}";
                return sJson;
            }
            return sJson;
        }
        [WebMethod]
        public string GenerateJsonNotes(string sHumanID, string sEncounterID)
        {

            IList<Blob_Progress_Note> ilstBlob_Progress_Note = new List<Blob_Progress_Note>();
            BlobProgressNoteManager BlobProgressNoteMngr = new BlobProgressNoteManager();
            EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
            IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();

            HumanBlobManager HumanBlobMngr = new HumanBlobManager();
            IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
            Blob_Progress_Note objBlobProgressNotesInitiated = new Blob_Progress_Note();
            ilstBlob_Progress_Note = BlobProgressNoteMngr.GetBlobProgressNotes(Convert.ToUInt64(sEncounterID));
            try
            {
                //Initiate save call
                if (ilstBlob_Progress_Note.Count == 0)
                {
                    ilstBlob_Progress_Note.Add(objBlobProgressNotesInitiated);
                }

                ilstBlob_Progress_Note[0].Id = Convert.ToUInt64(sEncounterID);
                ilstBlob_Progress_Note[0].Human_ID = Convert.ToUInt64(sHumanID);
                ilstBlob_Progress_Note[0].Progress_Note_Json = null;
                ilstBlob_Progress_Note[0].Status = "Initiated";
                ilstBlob_Progress_Note[0].Error_Description = string.Empty;
                ilstBlob_Progress_Note[0].Created_By = "";
                ilstBlob_Progress_Note[0].Created_Date_And_Time = DateTime.UtcNow;

                BlobProgressNoteMngr.SaveBlobProgressNotesWithTransaction(ilstBlob_Progress_Note, string.Empty);
                //End

                if (sEncounterID != "")
                {
                    ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Convert.ToUInt64(sEncounterID));
                }
                //if (sHumanID != "")
                //{
                //    ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(sHumanID));
                //}
                string sTabMode = "false";
                string sIsPhoneEncounter = "N";
                string sXMLEncounterDoc = string.Empty;
                string sXMLHumanDoc = string.Empty;
                string sFinalOutPut = string.Empty;
                XmlDocument xmlEncounterDoc = new XmlDocument();
                XmlDocument xmlHumanDoc = new XmlDocument();
                UtilityManager utilitymngr = new UtilityManager();

                if (ilstEncounterBlob.Count > 0)
                {
                    sXMLEncounterDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                    if (sXMLEncounterDoc.Substring(0, 1) != "<")
                        sXMLEncounterDoc = sXMLEncounterDoc.Substring(1, sXMLEncounterDoc.Length - 1);
                    //Jira #CAP-115
                    sXMLEncounterDoc = UtilityManager.ReplaceSpecialCharaters(sXMLEncounterDoc);
                    xmlEncounterDoc.LoadXml(sXMLEncounterDoc);
                }

                sIsPhoneEncounter = xmlEncounterDoc.SelectSingleNode("notes/Modules/EncounterList/Encounter").Attributes.GetNamedItem("Is_Phone_Encounter").Value.ToUpper();

                WFObjectManager wfObjMngr = new WFObjectManager();
                WFObject DocumentationWfObject = wfObjMngr.GetByObjectSystemId(Convert.ToUInt64(sEncounterID), "DOCUMENTATION");

                if (DocumentationWfObject != null && DocumentationWfObject.Current_Process == string.Empty)
                {
                    DocumentationWfObject = wfObjMngr.GetWfObjArchiveByObjectSystemId(Convert.ToUInt64(sEncounterID), "DOCUMENTATION");
                }

                if (DocumentationWfObject.Current_Process == "DOCUMENT_COMPLETE" || sIsPhoneEncounter.ToUpper() == "Y")
                {
                    if (ilstEncounterBlob != null && ilstEncounterBlob.Count > 0 && ilstEncounterBlob[0].Human_XML != null)
                    {
                        sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Human_XML);
                    }
                    else
                    {
                        ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(sHumanID));
                        if (ilstHumanBlob != null && ilstHumanBlob.Count > 0 && ilstHumanBlob[0].Human_XML != null)
                        {
                            sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                        }
                    }




                    if (sXMLHumanDoc != null && sXMLHumanDoc != "" && sXMLHumanDoc != string.Empty)
                    {
                        //sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                        if (sXMLHumanDoc.Substring(0, 1) != "<")
                            sXMLHumanDoc = sXMLHumanDoc.Substring(1, sXMLHumanDoc.Length - 1);
                        //Jira #CAP-115
                        sXMLHumanDoc = UtilityManager.ReplaceSpecialCharaters(sXMLHumanDoc);
                        xmlHumanDoc.LoadXml(sXMLHumanDoc);
                    }

                    string xsltFile = string.Empty;
                    if (sIsPhoneEncounter != null && sIsPhoneEncounter == "Y")
                    {// jira cap-499
                        sIsPhoneEncounter = "Y";
                        xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Phone_Encounter_Notes.xsl");
                    }
                    else
                    {
                        xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Progress_Notes.xsl");
                    }

                    string WordOutputName = sHumanID + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".html";
                    string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);

                    UtilityManager.PrintPDFUsingXSLT(sXMLEncounterDoc, sXMLHumanDoc, xsltFile, outputDocument, "");
                    System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);



                    string Encounter_signedDate = "";
                    string Encounter_Provider_Name = "";
                    string Encounter_Reviewed_signedDate = "";
                    string Encounter_Reviewed_Name = "";
                    string Encounter_Reviewed_Id = "";
                    string sIsphoneEncounter = "";
                    string sCreatedBy = "";

                    TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
                    XDocument xmlDocumentType = XDocument.Load(EncXMLContent);
                    foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
                    {
                        foreach (XElement Encounter in elements.Elements())
                        {
                            DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                            Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                            DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                            Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                            Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;
                            sIsphoneEncounter = Encounter.Attribute("Is_Phone_Encounter").Value;
                            sCreatedBy = Encounter.Attribute("Created_By").Value;
                        }

                        //if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
                        //{
                        // foreach (XElement Encounter in elements.Elements())
                        // {
                        //     DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                        //     Encounter_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        //}
                        //}
                    }
                    //Provider Name 
                    foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
                    {
                        foreach (XElement Encounter in elements.Elements())
                        {
                            Encounter_Provider_Name = Encounter.Value;
                            break;
                        }
                        break;
                    }
                    //Provider Reviewed Name 
                    if (Encounter_Reviewed_Id != "")
                    {
                        string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                        if (File.Exists(xmlFilepathUser))
                        {
                            XmlDocument xdoc = new XmlDocument();
                            XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                            xdoc.Load(itext);
                            itext.Close();
                            XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");
                            if (xnodelst != null && xnodelst.Count > 0)
                            {
                                foreach (XmlNode xnode in xnodelst)
                                {
                                    if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == Encounter_Reviewed_Id)
                                    {
                                        Encounter_Reviewed_Name = xnode.Attributes.GetNamedItem("person_name").Value;
                                    }
                                }
                            }
                        }
                    }

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(sXMLEncounterDoc);

                    string sPhysicianID = xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter").Attributes.GetNamedItem("Encounter_Provider_ID").Value;
                    string sEncounterIDForPatientDetails = (xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter")?.Attributes?.GetNamedItem("Id")?.Value != null) ?
                        xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter")?.Attributes?.GetNamedItem("Id")?.Value :
                        (xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter")?.Attributes?.GetNamedItem("Encounter_ID")?.Value != null) ?
                        xmldoc.SelectSingleNode("notes/Modules/EncounterList/Encounter")?.Attributes?.GetNamedItem("Encounter_ID")?.Value : "";
                    UserManager Usermngr = new UserManager();
                    IList<User> ilstUser = new List<User>();
                    ilstUser = Usermngr.getUserByPHYID(Convert.ToUInt64(sPhysicianID));
                    string sUserEmailAddr = (ilstUser.Count > 0) ? ilstUser[0].EMail_Address : "";

                    //Generate Json
                    string htmlString = System.IO.File.ReadAllText(outputDocument);
                    string xmls = htmlString.Replace("&nbsp;", "").Replace("&bull;", "").Replace("&amp;", "");
                    xmls = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + xmls + "</content>";
                    htmlString = htmlString.Replace("<subtab>", "").Replace("</subtab>", "").Replace("<plan>", "").Replace("</plan>", "");
                    //Jira CAP-1015
                    htmlString = htmlString.Replace("amp;", "");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmls);
                    string sTyepeOfPattern = string.Empty;
                    string sSection = string.Empty;
                    sFinalOutPut = "{";
                    for (int i = 0; i < doc.SelectNodes("content/p").Count; i++)
                    {
                        sTyepeOfPattern = string.Empty;
                        sSection = string.Empty;
                        if (doc.SelectSingleNode("content/p[" + i + "]")?.InnerXml != null && doc.SelectSingleNode("content/p[" + i + "]").InnerXml != "")
                        {
                            // This patern for normal screen like CC - Content is present under Secion Name directly
                            if (doc.SelectSingleNode("content/p[" + i + "]/b")?.InnerXml != null
                                && doc.SelectSingleNode("content/p[" + i + "]/i")?.InnerXml == null
                                && doc.SelectSingleNode("content/p[" + i + "]/b/i")?.InnerXml == null
                                && doc.SelectSingleNode("content/p[" + i + "]/table")?.InnerXml == null
                                && doc.SelectSingleNode("content/p[" + i + "]/font/b/i")?.InnerXml == null) 
                            {

                                sTyepeOfPattern = "1";
                                sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                            }
                            //// This patern Only for Screening and prev screening - Content is present under Section and multiple level sub sections & heading
                            else if (doc.SelectSingleNode("content/p[" + i + "]/i")?.InnerXml != null || doc.SelectSingleNode("content/p[" + i + "]/b/i")?.InnerXml != null) 
                            {
                                sTyepeOfPattern = "2";
                                sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                            }
                            // This patern Only for Individualized Care Plan - Content is present under Section and multiple level sub sections & heading. But content has exta tags like font
                            else if (doc.SelectSingleNode("content/p[" + i + "]/font/b/i")?.InnerXml != null) 
                            {
                                sTyepeOfPattern = "3";
                                sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                            }
                            // This pattern for table formate in the progress note - Content is in Table format
                            else if (i != 1 && doc.SelectSingleNode("content/p[" + i + "]/table")?.InnerXml != null) 
                            {
                                sTyepeOfPattern = "4";
                                sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                            }
                            // This patern for Patient Details - Content is Progress Note header
                            else if (i == 1
                                && doc.SelectSingleNode("content/p[" + i + "]/table/thead/tr/td")?.InnerXml != null
                                && doc.SelectSingleNode("content/p[" + i + "]/table/thead/tr/td").InnerText.Contains("Patient Name"))
                            {

                                XmlNode AdditionalParentElelemnt = doc.CreateElement("tr");
                                XmlNode AdditionalElement = null;
                                IList<string> AdditionalValues = new List<string>();
                                AdditionalValues.Add("EncounterID :" + sEncounterIDForPatientDetails);
                                AdditionalValues.Add("PhysicianID :" + sPhysicianID);
                                AdditionalValues.Add("Physician EMail Address :" + sUserEmailAddr);
                                for (int iCount = 0; iCount < AdditionalValues.Count; iCount++)
                                {
                                    AdditionalElement = doc.CreateElement("td");
                                    AdditionalElement.InnerText = AdditionalValues[iCount];
                                    AdditionalParentElelemnt.AppendChild(AdditionalElement);
                                }

                                sTyepeOfPattern = "5";
                                doc.SelectSingleNode("content/p[" + i + "]/table/thead").AppendChild(AdditionalParentElelemnt);
                                sSection = doc.SelectSingleNode("content/p[" + i + "]").InnerXml;
                            }

                            //Final Json formation
                            string sSectionOutPut = string.Empty;
                            if (sSection != string.Empty)
                            {
                                sSectionOutPut = GenerateJson(sSection, sTyepeOfPattern);
                            }
                            if (sSectionOutPut != string.Empty)
                            {
                                sFinalOutPut = sFinalOutPut + ((sFinalOutPut.Length > 1) ? "," : "") + sSectionOutPut;
                            }
                        }
                    }

                    string strfooterProvider = "";
                    if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && sIsphoneEncounter != "Y")
                    {
                        strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
                    }
                    else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && sIsphoneEncounter == "Y")
                    {
                        if (Encounter_Provider_Name != "")
                        {
                            strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
                        }
                        else
                        {
                            strfooterProvider = "Electronically Signed by " + sCreatedBy + " at " + Encounter_signedDate;
                        }
                    }
                    //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
                    //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

                    string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
                    StaticLookupManager staticMngr = new StaticLookupManager();
                    string strfooterProviderReviewed = string.Empty;
                    IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
                    if (CommonList.Count > 0)
                        strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");


                    if (file.Exists)
                    {
                        File.Delete(outputDocument);
                    }
                    var strBody = new StringBuilder();

                    string sbTop = "";

                    sbTop = sbTop + htmlString;



                    string strfooterF = "";

                    string strfooterPA = "";
                    string strfooterP = "";
                    if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
                    {
                        strfooterPA = strfooterProvider;

                        strfooterP = strfooterProviderReviewed;
                        strfooterF = " ";
                        //  strfooterProvider = strfooterProvider + "<br/>" + strfooterProviderReviewed;
                    }
                    else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
                    {
                        strfooterF = strfooterProvider;
                    }
                    else
                    {


                        strfooterF = "";

                    }
                    string sFooter = string.Empty;
                    if (strfooterF == "")
                    {
                        sFooter = ",\"Footer" + "\":[]";
                        sFinalOutPut = sFinalOutPut + sFooter;
                    }
                    else if (strfooterPA != "" && strfooterP != "")
                    {
                        sFooter = ",\"Footer" + "\":[\"" + strfooterPA + "  " + strfooterP + "\"]";
                        sFinalOutPut = sFinalOutPut + sFooter;
                    }
                    else
                    {
                        sFooter = ",\"Footer" + "\":[\"" + strfooterF + "\"]";
                        sFinalOutPut = sFinalOutPut + sFooter;
                    }

                    sFinalOutPut = sFinalOutPut + "}";
                }

                if (sFinalOutPut != string.Empty)
                {
                    ilstBlob_Progress_Note[0].Id = Convert.ToUInt64(sEncounterID);
                    ilstBlob_Progress_Note[0].Human_ID = Convert.ToUInt64(sHumanID);
                    byte[] bytesKeep = null;
                    try
                    {
                        bytesKeep = System.Text.Encoding.Default.GetBytes(sFinalOutPut);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error : " + ex?.Message);
                    }
                    ilstBlob_Progress_Note[0].Status = "Completed";
                    ilstBlob_Progress_Note[0].Progress_Note_Json = bytesKeep;
                    ilstBlob_Progress_Note[0].Error_Description = string.Empty;
                    ilstBlob_Progress_Note[0].Created_By = "";
                    ilstBlob_Progress_Note[0].Created_Date_And_Time = DateTime.UtcNow;


                    BlobProgressNoteMngr.SaveBlobProgressNotesWithTransaction(ilstBlob_Progress_Note, string.Empty);
                }

                return "{\"EncounterID\":" + sEncounterID + ",\"status\":\"Completed\",\"Description\":\"Request is available in Blob Progress Note table\"}";
            }
            catch (Exception eex)
            {
                try
                {
                    ilstBlob_Progress_Note[0].Id = Convert.ToUInt64(sEncounterID);
                    ilstBlob_Progress_Note[0].Human_ID = Convert.ToUInt64(sHumanID);
                    ilstBlob_Progress_Note[0].Progress_Note_Json = null;
                    ilstBlob_Progress_Note[0].Status = "Error";
                    ilstBlob_Progress_Note[0].Error_Description = "Message : " + eex?.Message + "Stack Trace : " + eex?.StackTrace;
                    ilstBlob_Progress_Note[0].Created_By = "";
                    ilstBlob_Progress_Note[0].Created_Date_And_Time = DateTime.UtcNow;
                    BlobProgressNoteMngr.SaveBlobProgressNotesWithTransaction(ilstBlob_Progress_Note, string.Empty);
                    throw new Exception("Error : " + eex?.Message);
                }
                catch { throw new Exception("Error : " + eex?.Message); }
            }
            return string.Empty;
        }
        public string GenerateJson(string sSection, string sType)
        {
            string[] heading = { "<br />" };
            string[] PlanTag = { "<plan>" };
            string[] subtab = { "<subtab>" };
            string[] section = { "<i>" };
            switch (sType)
            {
                case "1":
                    {
                        //Direct page
                        UserManager usermngr = new UserManager();
                        IList<User> ilstUser = new List<User>();
                        string[] PlanTagcontent = sSection.Split(PlanTag, System.StringSplitOptions.RemoveEmptyEntries);

                        IList<string> ilstsection = PlanTagcontent[0].Split(heading, System.StringSplitOptions.RemoveEmptyEntries).ToList();

                        if (PlanTagcontent.Length > 1)
                        {
                            ilstsection.Add(PlanTagcontent[1].Replace("</plan>", "").Replace("<br />", "").Replace("<br/>", ""));
                        }
                        string[] sectopns = ilstsection.ToArray();
                        string sFormationJson = string.Empty;
                        for (int i = 0; i < sectopns.Length; i++)
                        {
                            if (i == 0 && sectopns[i].Contains("<b>"))
                            {
                                //Key
                                sFormationJson = "\"" + sectopns[i].Replace("<b>", "").Replace("</b>", "").Replace(":", "") + "\"" + ":[";
                            }
                            else if (sectopns[0].Replace("<b>", "").Replace("</b>", "").Replace(":", "") == "Amendment Notes")
                            {
                                if (sectopns[i].IndexOf("AddendumReviewProviderID") > -1)
                                {
                                    string[] sTempNotesName = sectopns[i].Split(new string[] { "AM:", "PM:" }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string sNotesName = sTempNotesName.Length > 1 ? sTempNotesName[1] : "";
                                    string[] sTempCreatedAt = sectopns[i].Split(new string[] { "on" }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string sCreatedAt = sTempCreatedAt.Length > 1 ? sTempCreatedAt[1].Replace(":" + sNotesName, "") : "";

                                    string sProviderUserID = string.Empty;
                                    if (sectopns[i].IndexOf("<AddendumProviderID>") > -1)
                                    {
                                        sProviderUserID = sectopns[i].Substring(sectopns[i].IndexOf("<AddendumProviderID>"), sectopns[i].IndexOf("</AddendumProviderID>") + "</AddendumProviderID>".Length);
                                    sectopns[i] = sectopns[i].Replace(sProviderUserID, "");
                                    sProviderUserID = sProviderUserID.Replace("<AddendumProviderID>", "").Replace("</AddendumProviderID>", "");
                                    ilstUser = usermngr.getUserByPHYID(Convert.ToUInt64(sProviderUserID));
                                    }
                                    string UserID = string.Empty;
                                    UserID = ilstUser[0].EMail_Address;
                                    string sReviewProviderUserID = string.Empty;
                                    string sPAandPhysicianName = string.Empty;
                                    string sPAname = string.Empty;
                                    string sPhysician = string.Empty;
                                    string sReviewPhysicianEmailID = string.Empty;
                                    if (sectopns[i].IndexOf("<AddendumReviewProviderID>") > -1 && sectopns[i].IndexOf("</AddendumReviewProviderID>") > -1)
                                    {
                                        sReviewProviderUserID = sectopns[i].Substring(sectopns[i].IndexOf("<AddendumReviewProviderID>"), sectopns[i].IndexOf("</AddendumReviewProviderID>") + "</AddendumReviewProviderID>".Length);
                                        sectopns[i] = sectopns[i].Replace(sReviewProviderUserID, "");
                                        //sReviewProviderUserID = sReviewProviderUserID.Replace("<AddendumReviewProviderID>", "").Replace("</AddendumReviewProviderID>", "");
                                        Match matchProviderID = Regex.Match(sReviewProviderUserID, @"<AddendumReviewProviderID>(\d+)</AddendumReviewProviderID>");
                                        if (matchProviderID.Success)
                                        {
                                            sReviewProviderUserID = matchProviderID.Groups[1].Value;
                                        ilstUser = usermngr.getUserByPHYID(Convert.ToUInt64(sReviewProviderUserID));
                                        }
                                        sReviewPhysicianEmailID = ilstUser[0].EMail_Address;

                                        string[] sTempPAandPhysicianName = sectopns[i].Split(new string[] { "on" }, System.StringSplitOptions.RemoveEmptyEntries);
                                        sPAandPhysicianName = sTempPAandPhysicianName.Length > 1 ? sTempPAandPhysicianName[1].Replace("Signed by", "") : "";
                                        if (!string.IsNullOrEmpty(sPAandPhysicianName))
                                        {
                                            string[] sTempPAname = sPAandPhysicianName.Split(new string[] { "and reviewed by" }, System.StringSplitOptions.RemoveEmptyEntries);
                                            sPAname = sTempPAname.Length > 0 ? sTempPAname[0] : "";
                                            sPhysician = sTempPAname.Length > 1 ? sTempPAname[1] : "";
                                        }
                                    }

                                    sFormationJson = sFormationJson + ((sFormationJson[sFormationJson.Length - 1] == '}') ? "," : "")
                                        + "{\"" + "text" + "\":\"" + sNotesName + "\"," +
                                        "\"" + "createdBy" + "\":\"" + sPAname + "\"," +
                                        "\"" + "UserID" + "\":\"" + UserID + "\"," +
                                        "\"" + "ProviderID" + "\":\"" + sProviderUserID + "\"," +
                                        "\"" + "ReviewedBy" + "\":\"" + sPhysician + "\"," +
                                        "\"" + "ReviewedUserID" + "\":\"" + sReviewPhysicianEmailID + "\"," +
                                        "\"" + "ReviewedProviderID" + "\":\"" + sReviewProviderUserID + "\"," +
                                        "\"" + "createdAt" + "\":\"" + sCreatedAt + "\"}";
                                }
                                else
                                {
                                    string sProviderEmailID = string.Empty;
                                    string sProviderUserID = string.Empty;
                                    if(sectopns[i].IndexOf("<AddendumProviderID>") > -1)
                                    {
                                        sProviderUserID = sectopns[i].Substring(sectopns[i].IndexOf("<AddendumProviderID>"), sectopns[i].IndexOf("</AddendumProviderID>") + "</AddendumProviderID>".Length);
                                    sectopns[i] = sectopns[i].Replace(sProviderUserID, "");
                                    sProviderUserID = sProviderUserID.Replace("<AddendumProviderID>", "").Replace("</AddendumProviderID>", "");
                                    ilstUser = usermngr.getUserByPHYID(Convert.ToUInt64(sProviderUserID));
                                    }
                                    sProviderEmailID = ilstUser[0].EMail_Address;

                                    string[] sTempNotesName = sectopns[i].Split(new string[] { "AM:", "PM:" }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string sNotesName = sTempNotesName.Length > 1 ? sTempNotesName[1] : "";
                                    string[] sTempCreatedAt = sectopns[i].Split(new string[] { "on" }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string sCreatedAt = sTempCreatedAt.Length > 1 ? sTempCreatedAt[1].Replace(":" + sNotesName, "") : "";

                                    string[] tempCreatedBy = sectopns[i].Split(new string[] { "on" }, System.StringSplitOptions.RemoveEmptyEntries);
                                    string createdBy = tempCreatedBy.Length > 0 ? tempCreatedBy[0].Replace("Signed by", "") : "";

                                    sFormationJson = sFormationJson + ((sFormationJson[sFormationJson.Length - 1] == '}') ? "," : "")
                                        + "{\"" + "text" + "\":\"" + sNotesName + "\"," +
                                        "\"" + "createdBy" + "\":\"" + createdBy + "\"," +
                                        "\"" + "UserID" + "\":\"" + sProviderEmailID + "\"," +
                                         "\"" + "ProviderID" + "\":\"" + sProviderUserID + "\"," +
                                        "\"" + "createdAt" + "\":\"" + sCreatedAt + "\"}";
                                }
                            }
                            else
                            {
                                //value
                                sFormationJson = sFormationJson + "\"" + sectopns[i].Replace("</b>", "").TrimStart().TrimEnd() + "\"" + ((sectopns.Length - 1 == i) ? string.Empty : ",");
                            }
                        }
                        sFormationJson = sFormationJson + "]";
                        return sFormationJson;
                        break;
                    }
                case "2":
                    {
                        //Screening and pre screening
                        string sXmlSection = sSection;
                        string sScreenName = string.Empty;
                        int count = 1;
                        sXmlSection = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + sXmlSection + "</content>";
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(sXmlSection);

                        if (doc.SelectSingleNode("content/b")?.InnerText != null)
                        {
                            sScreenName = doc.SelectSingleNode("content/b")?.InnerText.Replace(":", "");
                            sSection = sSection.Replace(doc.SelectSingleNode("content/b").OuterXml + "\"<br />", "").Replace(doc.SelectSingleNode("content/b").OuterXml, "");
                        }

                        string[] sContent = sSection.Split(subtab, System.StringSplitOptions.RemoveEmptyEntries);
                        if (sContent.Length > 0)
                        {
                            count = sContent.Length;
                        }
                        string sSectioncontent = string.Empty;
                        string sSubtabcontent = string.Empty;
                        for (int iSubtab = 0; iSubtab < count; iSubtab++)
                        {
                            string sSubtabName = string.Empty;
                            if (sContent[iSubtab] != "<br />" && sContent[iSubtab] != "<br /><b>" && sContent[iSubtab] != "<b>")
                            {
                                if (sContent[iSubtab].Contains("</subtab>"))
                                {
                                    sSubtabName = sContent[iSubtab].Substring(0, sContent[iSubtab].IndexOf("</subtab>")).Replace(":", "");
                                    sContent[iSubtab] = sContent[iSubtab].Replace(sSubtabName + ":</subtab>", "");
                                }
                                string[] sSectioniteams = sContent[iSubtab].Split(section, System.StringSplitOptions.RemoveEmptyEntries);
                                sSectioncontent = string.Empty;
                                for (int iSectionCount = 0; iSectionCount < sSectioniteams.Length; iSectionCount++)
                                {
                                    if (sSectioniteams[iSectionCount] != "<br />" && sSectioniteams[iSectionCount] != "<br /><b>" && sSectioniteams[iSectionCount] != "<b>")
                                    {
                                        string[] iSectionValuesplit = sSectioniteams[iSectionCount].Split(heading, System.StringSplitOptions.RemoveEmptyEntries);
                                        sSectioncontent = sSectioncontent + ((sSectioncontent != string.Empty) ? "," : "");
                                        for (int iSectionValueCount = 0; iSectionValueCount < iSectionValuesplit.Length; iSectionValueCount++)
                                        {
                                            if (iSectionValuesplit[iSectionValueCount] != "<br />" && iSectionValuesplit[iSectionValueCount] != "<br /><b>" && iSectionValuesplit[iSectionValueCount] != "<b>")
                                            {
                                                if (iSectionValuesplit[iSectionValueCount].Contains("</i>"))
                                                {
                                                    sSectioncontent = sSectioncontent + ((sSectioncontent != string.Empty && sSectioncontent.Substring(sSectioncontent.LastIndexOf("],")) != "],") ? "]," : "");
                                                    sSectioncontent = sSectioncontent + "\"" + iSectionValuesplit[iSectionValueCount].Replace("</i>", "").Replace("</b>", "").Replace(":", "") + "\"" + ":["; ;
                                                }
                                                else
                                                {
                                                    sSectioncontent = sSectioncontent + ((sSectioncontent != string.Empty && sSectioncontent.Substring(sSectioncontent.LastIndexOf(":[")) == ":[") ? "" : ",") + "\"" + iSectionValuesplit[iSectionValueCount] + "\"";
                                                }

                                            }
                                            if (iSectionValueCount == iSectionValuesplit.Length - 1)
                                            {
                                                sSectioncontent = sSectioncontent + ((sSectioncontent != string.Empty) ? "]" : "");
                                            }
                                        }




                                    }


                                }

                            }
                            if (sSubtabName != string.Empty)
                            {
                                string subtabtemp = string.Empty;
                                subtabtemp = "\"" + sSubtabName + "\":{" + sSectioncontent + "}";
                                sSubtabcontent = sSubtabcontent + ((sSubtabcontent != string.Empty) ? "," : "") + subtabtemp;
                            }
                        }

                        if (sSubtabcontent != string.Empty)
                        {
                            sScreenName = "\"" + sScreenName + "\":{" + sSubtabcontent + "}";
                        }
                        else
                        {
                            sScreenName = "\"" + sScreenName + "\":{" + sSectioncontent + "}";
                        }

                        return sScreenName;
                        break;
                    }
                case "3":
                    {
                        //Individualized Care Plan
                        sSection = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + sSection + "</content>";
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(sSection);
                        int iChildTagCount = 0;
                        string sHeaderName = string.Empty;
                        if (doc.SelectSingleNode("content/font[1]/b")?.InnerText != null)
                        {
                            sHeaderName = doc.SelectSingleNode("content/font[1]/b").InnerText.Replace(":", "");
                        }

                        if (doc.SelectSingleNode("content/font[2]")?.ChildNodes != null)
                        {
                            iChildTagCount = (int)doc.SelectSingleNode("content/font[2]")?.ChildNodes.Count;
                        }
                        string sBody = string.Empty;
                        for (int iCount = 0; iCount < iChildTagCount; iCount++)
                        {

                            if (doc.SelectSingleNode("content/font[2]")?.ChildNodes[iCount].Name == "b")
                            {
                                sBody = sBody + ((sBody != string.Empty) ? "]," : "");
                                string header = doc.SelectSingleNode("content/font[2]")?.ChildNodes[iCount].InnerText.Replace(":", "");
                                sBody = sBody + "\"" + header + "\"" + ":[";
                            }
                            else if (doc.SelectSingleNode("content/font[2]")?.ChildNodes[iCount].Name == "ul")
                            {
                                string value = doc.SelectSingleNode("content/font[2]")?.ChildNodes[iCount].InnerText;
                                sBody = sBody + ((sBody.Substring(sBody.LastIndexOf(":[")) == ":[") ? "" : ",") + "\"" + value + "\"";
                            }

                            if (iCount == iChildTagCount - 1)
                            {
                                sBody = sBody + ((sBody != string.Empty) ? "]" : "");
                            }

                        }
                        sHeaderName = "\"" + sHeaderName + "\":{" + sBody + "}";
                        return sHeaderName;

                        break;
                    }
                case "4":
                    {
                        //table formate Pages
                        sSection = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + sSection + "</content>";
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(sSection);
                        string sTableRow = string.Empty;
                        int iSubtabCount = 1;
                        bool isSubtabOccure = false;
                        string sScubtabHeader = string.Empty;
                        string sSubtabContent = string.Empty;
                        string sScreenName = string.Empty;
                        if (doc.SelectNodes("content/subtab")?.Count != null && doc.SelectNodes("content/subtab").Count > 0) // if this screen have subtab this codition is applayed
                        {
                            iSubtabCount = doc.SelectNodes("content/subtab").Count;
                            isSubtabOccure = true;
                        }
                        for (int iCountSubtab = 1; iCountSubtab <= iSubtabCount; iCountSubtab++)
                        {
                            sSubtabContent = string.Empty;
                            sTableRow = string.Empty;
                            if (isSubtabOccure)
                            {
                                sSubtabContent = doc.SelectSingleNode("content/subtab[" + iCountSubtab + "]").InnerText.Replace(":", "");
                            }
                            else
                            {
                                sSubtabContent = doc.SelectSingleNode("content/b").InnerText.Replace(":", "");
                            }
                            for (int iCountRow = 1; iCountRow <= doc.SelectNodes("content/table[" + iCountSubtab + "]/tr").Count; iCountRow++)
                            {
                                string sTablecolumn = string.Empty;
                                int rowcount = doc.SelectNodes("content/table[" + iCountSubtab + "]/tr[" + iCountRow + "]/td").Count;
                                for (int iCounttd = 1; iCounttd <= rowcount; iCounttd++)
                                {
                                    sTablecolumn = sTablecolumn + ((sTablecolumn != string.Empty) ? "," : "") + "\"" + doc.SelectSingleNode("content/table[" + iCountSubtab + "]/thead/tr/td[" + iCounttd + "]").InnerText + "\"" + ":" + "\"" + doc.SelectSingleNode("content/table[" + iCountSubtab + "]/tr[" + iCountRow + "]/td[" + iCounttd + "]").InnerText + "\"";
                                }

                                sTableRow = sTableRow + ((sTableRow != string.Empty) ? "," : "") + "{" + sTablecolumn + "}";
                            }
                            sSubtabContent = "\"" + sSubtabContent + "\":[" + sTableRow + "]";
                            sScubtabHeader = sScubtabHeader + ((sScubtabHeader != string.Empty) ? "," : "") + sSubtabContent;
                        }
                        if (isSubtabOccure)
                        {
                            sScreenName = "\"" + doc.SelectSingleNode("content/b").InnerText + "\":";
                            sScreenName = sScreenName + "{" + sScubtabHeader + "}";
                            return sScreenName;
                        }


                        return sSubtabContent;
                        break;
                    }
                case "5":
                    {
                        //Patient Details - header
                        sSection = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <content>" + sSection + "</content>";
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(sSection);
                        int iRowLength = doc.SelectNodes("content/table/thead/tr").Count;
                        string value = string.Empty;
                        string sPatientDetails = string.Empty;
                        for (int iCount = 1; iCount <= iRowLength; iCount++)
                        {
                            if (doc.SelectNodes("content/table/thead/tr[" + iCount + "]/td")?.Count != null)
                            {
                                int iColumnLength = doc.SelectNodes("content/table/thead/tr[" + iCount + "]/td").Count;
                                for (int iCounttd = 1; iCounttd <= iColumnLength; iCounttd++)
                                {
                                    value = string.Empty;
                                    if (doc.SelectSingleNode("content/table/thead/tr[" + iCount + "]/td[" + iCounttd + "]")?.InnerText != null)
                                    {
                                        value = doc.SelectSingleNode("content/table/thead/tr[" + iCount + "]/td[" + iCounttd + "]").InnerText.Replace("#$%", "");
                                        if (!value.EndsWith(": "))
                                        { value = value.Replace(": ", ":"); }
                                    }
                                    if (value != "")
                                    {
                                        sPatientDetails = sPatientDetails + ((sPatientDetails != string.Empty) ? "," : "") + "\"" + value.Substring(0, value.IndexOf(":")) + "\":\"" + value.Substring(((value.IndexOf(":") != value.Length - 1) ? value.IndexOf(":") + 1 : 0)) + "\"";
                                    }
                                }
                            }
                        }
                        sPatientDetails = "\"" + "Patient Details" + "\":{" + sPatientDetails + "}";
                        return sPatientDetails;
                    }
            }
            return string.Empty;
        }

    }
}
