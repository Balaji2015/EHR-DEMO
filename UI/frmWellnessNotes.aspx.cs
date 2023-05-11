using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using HTMLtoPDF;
using System.Net;
using Acurus.Capella.Core.DomainObjects;

using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text.RegularExpressions;

namespace Acurus.Capella.UI
{
    public partial class frmWellnessNotes : System.Web.UI.Page
    {
        string strXmlEncounterPath1 = "";
        string sNotesName = string.Empty;
        string Name = "";
        string sXMLHumanDoc = string.Empty;
        string sXMLEncounterDoc = string.Empty;
        HumanBlobManager HumanBlobMngr = new HumanBlobManager();
        EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            sNotesName = Request.QueryString["SubMenuName"].Split('?')[0];


            ulong Encounter_Id = 0;

            if (Request.QueryString["EncounterId"] != null)
            {
                Encounter_Id = Convert.ToUInt32(Request.QueryString["EncounterId"].ToString());
                // ClientSession.Selectedencounterid = Encounter_Id;
            }
            string FileName = string.Empty;
            if (Encounter_Id != 0)
            {
                IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
                EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();

                ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Encounter_Id);
                if (ilstEncounterBlob.Count > 0)
                {
                    sXMLEncounterDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                    if (sXMLEncounterDoc.Substring(0, 1) != "<")
                        sXMLEncounterDoc = sXMLEncounterDoc.Substring(1, sXMLEncounterDoc.Length - 1);
                    //Jira #CAP-115
                    sXMLEncounterDoc = UtilityManager.ReplaceSpecialCharaters(sXMLEncounterDoc);
                }
                //GitLab #3933   
                else
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + Encounter_Id.ToString() + "','Encounter','summary');", true);
                    //return;
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryHumanIDAlert('Encounter XML');", true);
                    return;
                }
                FileName = "Encounter" + "_" + Encounter_Id + ".xml";
            }
            else if (ClientSession.EncounterId > 0)
            {
                IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
                EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();

                ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ClientSession.EncounterId);
                if (ilstEncounterBlob.Count > 0)
                {
                    sXMLEncounterDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                    if (sXMLEncounterDoc.Substring(0, 1) != "<")
                        sXMLEncounterDoc = sXMLEncounterDoc.Substring(1, sXMLEncounterDoc.Length - 1);
                    //Jira #CAP-115
                    sXMLEncounterDoc = UtilityManager.ReplaceSpecialCharaters(sXMLEncounterDoc);
                }
                //GitLab #3933
                else
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.EncounterId.ToString() + "','Encounter','summary');", true);
                    //return;
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryHumanIDAlert('Encounter XML');", true);
                    return;
                }
                FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            }
            //GitLab #3933
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryHumanIDAlert('Encounter ID');", true);
                return;
            }

           // strXmlEncounterPath1 = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            if ((Request.QueryString["Menu"] != null && Request.QueryString["Menu"].Contains("True")) || (sNotesName != null))
            {

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "downloadFilewellness();", true);

            }

        }
        protected void btnWordwellness_Click(object sender, EventArgs e)
        {


            string NotesName = "";
            // string xsltFile = HttpContext.Current.Server.MapPath("EHR_Progress_Notes.xsl");
            string xsltFile = "";
            string OutputNamingConvention = "";
            if (sNotesName.ToUpper() == "WELLNESS NOTES")
            {
                string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];

                string file_location = System.Configuration.ConfigurationSettings.AppSettings["XMLPath"];


                bool status = DownloadFromImageServer(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ftpServerIP, file_location);

                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Wellness_Notes.xsl");
                OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["WellnessNotesNamingConvention"];
                NotesName = FileNameGeneration(OutputNamingConvention);
                if (!status)
                {

                    DownloadNotes(xsltFile, NotesName, sNotesName);
                }
                else
                {
                    string file = Path.Combine(file_location, Name);
                    Response.ContentType = "application/x-download";
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", NotesName + ".pdf"));
                    Response.WriteFile(file);

                    Response.Flush();
                    System.IO.File.Delete(file);
                    Response.End();
                }
            }
            if (sNotesName.ToUpper() == "TREATMENT NOTES")
            {
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Treatment_Notes.xsl");
                OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["TreatmentNotesNamingConvention"];
                NotesName = FileNameGeneration(OutputNamingConvention);
                DownloadNotes(xsltFile, NotesName, sNotesName);
            }
            if (sNotesName.ToUpper() == "CARE NOTE")
            {
                xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Care_Notes.xsl");
                OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["CareNotesNamingConvention"];
                NotesName = FileNameGeneration(OutputNamingConvention);
                DownloadNotes(xsltFile, NotesName, sNotesName);
            }

        }

        public string FileNameGeneration(string OutputNamingConvention)
        {
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    if (result.ToUpper().Contains("FACILITY"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    }

                    if (result.ToUpper().Contains("HUMAN"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.HumanId.ToString());
                    }
                    if (result.ToUpper().Contains("ENCOUNTER"))
                    {
                        NotesName = NotesName.Replace("[" + result + "]", ClientSession.EncounterId.ToString());

                    }
                    if (result.ToUpper().Contains("MEMBER") || result.ToUpper().Contains("LAST") || result.ToUpper().Contains("FIRST") || result.ToUpper().Contains("DOB"))
                    {
                        string sExternalAccNo = string.Empty;
                        string sLastName = string.Empty;
                        string sFirstName = string.Empty;
                        string sDOB = string.Empty;
                        XmlDocument itemDoc = new XmlDocument();
                        string sXMLContent = string.Empty;

                        HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                        IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(ClientSession.HumanId);
                        if (ilstHumanBlob.Count > 0)
                        {
                            sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                            if (sXMLContent.Substring(0, 1) != "<")
                                sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                            itemDoc.LoadXml(sXMLContent);
                        }
                        else
                        {
                            throw new Exception("Human XML is not found");
                        }

                        //string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml";

                        //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);
                        //if (File.Exists(strXmlHumanPath) == true)
                        //{
                            
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            //itemDoc.Load(XmlText);
                            //XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                //if (result.ToUpper().Contains("MEMBER"))
                                //{
                                //    sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                //    NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                //}
                                if (result.ToUpper().Contains("LAST"))
                                {
                                    sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                    NotesName = NotesName.Replace("[" + result + "]", sLastName);
                                }
                                else if (result.ToUpper().Contains("FIRST"))
                                {
                                    sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                    NotesName = NotesName.Replace("[" + result + "]", sFirstName);
                                }
                                else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                {
                                    sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                    NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                }
                            }
                            if (result.ToUpper().Contains("MEMBER"))
                            {
                                if (itemDoc.GetElementsByTagName("PatientInsuredPlanList")[0] != null)
                                {
                                TextReader HumanXMLContent = new StringReader(sXMLContent);
                                XDocument xmlInsurance = XDocument.Load(HumanXMLContent);

                                //XDocument xmlInsurance = XDocument.Load(strXmlHumanPath);
                                    var xmlPatientInsuredPlanList = xmlInsurance.Descendants("PatientInsuredPlanList")
                                        .Elements("PatientInsuredPlan").Where(aa => aa.Attribute("Insurance_Type").Value.ToString() == "PRIMARY");
                                    if (xmlPatientInsuredPlanList != null && xmlPatientInsuredPlanList.Count() > 0)
                                    {
                                        sExternalAccNo = xmlPatientInsuredPlanList.Attributes("Policy_Holder_ID").First().Value.ToString();
                                    }
                                }
                                NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                            }
                        //}

                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")))
                    {
                       // if (File.Exists(strXmlEncounterPath1) == true)
                        //{
                            XmlDocument itemDoc = new XmlDocument();
                        TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
                        XmlTextReader XmlText = new XmlTextReader(EncXMLContent);
                            itemDoc.Load(XmlText);
                            XmlText.Close();
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                string sDOS = string.Empty;
                                if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                {
                                    sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                }
                                NotesName = NotesName.Replace("[" + result + "]", sDOS);
                            }
                      //  }
                    }
                    if (result.ToUpper().Contains("RENDERING"))
                    {
                       // if (File.Exists(strXmlEncounterPath1) == true)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
                            XmlTextReader XmlText = new XmlTextReader(EncXMLContent);
                            itemDoc.Load(XmlText);
                            XmlText.Close();
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                string sRenderingProviderID = string.Empty;
                                string sRenderingProviderNPI = string.Empty;
                                if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Encounter_Provider_ID").Value != "")
                                {
                                    sRenderingProviderID = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Encounter_Provider_ID").Value;
                                    if (sRenderingProviderID != "")
                                    {
                                        XmlDocument xmldoc = new XmlDocument();
                                        string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                                        if (File.Exists(xmlFilepathUser))
                                        {
                                            xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                                            XmlNode nodeMatchingPhysicianAddress = xmldoc.SelectSingleNode("/PhysicianAddress/p" + sRenderingProviderID);
                                            if (nodeMatchingPhysicianAddress != null)
                                            {
                                                sRenderingProviderNPI = nodeMatchingPhysicianAddress.Attributes["Physician_NPI"].Value.ToString();
                                            }
                                        }
                                    }
                                }
                                NotesName = NotesName.Replace("[" + result + "]", sRenderingProviderNPI);
                            }
                        }
                    }
                }
            }
            if (NotesName.Contains('['))
            {
                string[] sName = NotesName.Split('~');

                for (int i = 0; i < sName.Length; i++)
                {
                    if (sName[i] != "")
                    {
                        if (sName[i].Contains("["))
                        {
                            result = ExtractBetween(sName[i], "[", "]");
                            NotesName = NotesName.Replace("[" + result + "]", "");
                        }
                    }
                }
            }
            NotesName = NotesName.Replace("~", "").Replace("__", "_");
            return NotesName;
        }
        public void DownloadNotes(string xsltFile, string NotesName, string sNotesName)
        {
           // string xmlDataFile = strXmlEncounterPath1;
            string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);
            IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
            ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64( ClientSession.HumanId));
            if (ilstHumanBlob.Count > 0)
            {
                sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                if (sXMLHumanDoc.Substring(0, 1) != "<")
                    sXMLHumanDoc = sXMLHumanDoc.Substring(1, sXMLHumanDoc.Length - 1);
                //Jira #CAP-115
                sXMLHumanDoc = UtilityManager.ReplaceSpecialCharaters(sXMLHumanDoc);
            }

            IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
            ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Convert.ToUInt64(ClientSession.EncounterId));
            if (ilstEncounterBlob.Count > 0)
            {
                sXMLEncounterDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                if (sXMLEncounterDoc.Substring(0, 1) != "<")
                    sXMLEncounterDoc = sXMLEncounterDoc.Substring(1, sXMLEncounterDoc.Length - 1);
                //Jira #CAP-115
                sXMLEncounterDoc = UtilityManager.ReplaceSpecialCharaters(sXMLEncounterDoc);
            }

            DataSet ds;
            XmlDataDocument xmlDoc;
            XslCompiledTransform xslTran;
            XmlElement root;
            XPathNavigator nav;
            XmlTextWriter writer;
            XsltSettings settings = new XsltSettings(true, false);
            ds = new DataSet();
            //ds.ReadXml(xmlDataFile);
            StringBuilder sb = new StringBuilder();
            sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
            ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));

            xmlDoc = new XmlDataDocument(ds);
            xslTran = new XslCompiledTransform();
            using (var stream = File.Open(xsltFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            }
            root = xmlDoc.DocumentElement;
            nav = root.CreateNavigator();
            if (File.Exists(outputDocument))
            {
                File.Delete(outputDocument);
            }
            writer = new XmlTextWriter(outputDocument, System.Text.Encoding.Default);
            xslTran.Transform(nav, writer);
            writer.Close();
            writer = null;
            nav = null;
            root = null;
            xmlDoc = null;
            ds = null;

            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);





            string Encounter_signedDate = "";
            string Encounter_Provider_Name = "";
            string Encounter_ReveiwsignedDate = "";
            string Encounter_ReveiwProvider_Name = "";
            IList<PhysicianLibrary> lstphysicianReview = new List<PhysicianLibrary>();
            IList<PhysicianLibrary> lstphysician = new List<PhysicianLibrary>();
            PhysicianManager objPhysician = new PhysicianManager();
            TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
            XDocument xmlDocumentType = XDocument.Load(EncXMLContent);

            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                    Encounter_ReveiwsignedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");

                    Encounter_ReveiwProvider_Name = Encounter.Attribute("Encounter_Provider_Review_ID").Value.ToString();
                }
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    Encounter_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm:ss tt");
                    Encounter_Provider_Name = Encounter.Attribute("Encounter_Provider_ID").Value.ToString();
                }

            }
            if (Encounter_ReveiwProvider_Name != "")
            {
                lstphysicianReview = objPhysician.GetphysiciannameByPhyID(Convert.ToUInt32(Encounter_ReveiwProvider_Name));
            }
            if (Encounter_Provider_Name != "")
            {
                lstphysician = objPhysician.GetphysiciannameByPhyID(Convert.ToUInt32(Encounter_Provider_Name));
            }
            //foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            //{
            //    foreach (XElement Encounter in elements.Elements())
            //    {
            //        Encounter_Provider_Name = Encounter.Value;
            //        break;

            //    }
            //}
            string htmlString = System.IO.File.ReadAllText(outputDocument);

            string headerstring = string.Empty;

            int index = htmlString.IndexOf("<table");
            int index1 = htmlString.IndexOf("</table>");
            headerstring = htmlString.Substring(index, index1);
            headerstring = headerstring + "</table>";
            headerstring.Replace("height=' 30% '", "");
            //htmlString = htmlString.Replace(headerstring, "");
            htmlString = htmlString.Substring(index1 + 8, htmlString.Length - index1 - 8);
            //  }
            if (htmlString.Contains("PMH_Split_Start"))
            {
                string sOriginalSectionString = ExtractBetween(htmlString, "PMH_Split_Start", "PMH_Split_End");
                string sFinalPMHsection = string.Empty;
                var countWord = Regex.Matches(sOriginalSectionString, "</td></tr>", RegexOptions.IgnoreCase).Count;
                if (countWord % 2 == 0)
                {
                    htmlString = htmlString.Replace("PMH_Split_Start", "").Replace("PMH_Split_End", "");
                }
                else
                {

                    sFinalPMHsection = FindAndReplace(sOriginalSectionString, "</td></tr>", "</td><td><xsl:text disable-output-escaping=\"yes\"> </xsl:text></td><td><xsl:text disable-output-escaping=\"yes\"></xsl:text></td></tr>");
                    string sBefore = Before(htmlString, "PMH_Split_Start");
                    string sAfter = After(htmlString, "PMH_Split_End");
                    htmlString = htmlString.Replace("PMH_Split_Start", "").Replace("PMH_Split_End", "");
                    htmlString = sBefore + sFinalPMHsection + sAfter;
                }
            }
            if (htmlString.Contains("Allery_Split_Start"))
            {
                string sOriginalSectionString = ExtractBetween(htmlString, "Allery_Split_Start", "Allery_Split_End");
                string sFinalAllergysection = string.Empty;
                var countWord = Regex.Matches(sOriginalSectionString, "</td></tr>", RegexOptions.IgnoreCase).Count;
                if (countWord % 2 == 0)
                {
                    htmlString = htmlString.Replace("Allery_Split_Start", "").Replace("Allery_Split_End", "");
                }
                else
                {
                    sFinalAllergysection = FindAndReplace(sOriginalSectionString, "</td></tr>", "</td><td><xsl:text disable-output-escaping=\"yes\"> </xsl:text></td><td><xsl:text disable-output-escaping=\"yes\"></xsl:text></td></tr>");
                    string sBefore = Before(htmlString, "Allery_Split_Start");
                    string sAfter = After(htmlString, "Allery_Split_End");
                    htmlString = htmlString.Replace("Allery_Split_Start", "").Replace("Allery_Split_End", "");
                    htmlString = sBefore + sFinalAllergysection + sAfter;
                }
            }
            if (htmlString.Contains("SH_Split_Start"))
            {
                string sOriginalSectionString = ExtractBetween(htmlString, "SH_Split_Start", "SH_Split_End");
                string sFinalSHsection = string.Empty;
                var countWord = Regex.Matches(sOriginalSectionString, "</td></tr>", RegexOptions.IgnoreCase).Count;
                if (countWord % 2 == 0)
                {
                    htmlString = htmlString.Replace("SH_Split_Start", "").Replace("SH_Split_End", "");
                }
                else
                {
                    sFinalSHsection = FindAndReplace(sOriginalSectionString, "</td></tr>", "</td><td><xsl:text disable-output-escaping=\"yes\"> </xsl:text></td><td><xsl:text disable-output-escaping=\"yes\"></xsl:text></td></tr>");
                    string sBefore = Before(htmlString, "SH_Split_Start");
                    string sAfter = After(htmlString, "SH_Split_End");
                    htmlString = htmlString.Replace("SH_Split_Start", "").Replace("SH_Split_End", "");
                    htmlString = sBefore + sFinalSHsection + sAfter;
                }
            }

            string strfooterPA = "";
            string strfooterP = "";
            string strfooterF = "";
            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();
            string strDocBody = ""; ;



            string sbTop = "";

            sbTop = sbTop + htmlString;


            if ((Encounter_ReveiwsignedDate != "" && Encounter_ReveiwsignedDate != "01-Jan-0001 12:00:00 AM"))
            {
                if (lstphysician.Count > 0)
                    strfooterPA = "Electronically Signed by " + lstphysician[0].PhyPrefix + " " + lstphysician[0].PhyFirstName + " "
                    + lstphysician[0].PhyMiddleName + " " + lstphysician[0].PhyLastName + " " + lstphysician[0].PhySuffix + " at " + Encounter_signedDate;
                if (lstphysicianReview.Count > 0)
                {
                    //strfooterP = "I " + lstphysicianReview[0].PhyPrefix + " " + lstphysicianReview[0].PhyFirstName
                    //    + " " + lstphysicianReview[0].PhyMiddleName + " " + lstphysicianReview[0].PhyLastName + " " + lstphysicianReview[0].PhySuffix +
                    // " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

                    //strfooterF = "Electronically Signed by " + lstphysicianReview[0].PhyPrefix + " " + lstphysicianReview[0].PhyFirstName + " "
                    //     + lstphysicianReview[0].PhyMiddleName + " " + lstphysicianReview[0].PhyLastName + " " + lstphysicianReview[0].PhySuffix + " at " + Encounter_ReveiwsignedDate;
                    //strfooter = strfooter + " " + Encounter_Provider_Name + " On " + Encounter_signedDate;

                    string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
                    StaticLookupManager staticMngr = new StaticLookupManager();
                    string strfooterProviderReviewed = string.Empty;
                    IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
                    if (CommonList.Count > 0)
                        strfooterP = CommonList[0].Value.Replace("<Physician>", lstphysicianReview[0].PhyPrefix + " " + lstphysicianReview[0].PhyFirstName + " "
                         + lstphysicianReview[0].PhyMiddleName + " " + lstphysicianReview[0].PhyLastName + " " + lstphysicianReview[0].PhySuffix + " at " + Encounter_ReveiwsignedDate).Replace("|", "");
                }

            }
            else if ((Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00:00 AM"))
            {
                if (lstphysician.Count > 0)
                    strfooterF = "Electronically Signed by " + lstphysician[0].PhyPrefix + " " + lstphysician[0].PhyFirstName
                   + " " + lstphysician[0].PhyMiddleName + " " + lstphysician[0].PhyLastName + " " + lstphysician[0].PhySuffix + " at " + Encounter_signedDate;

            }

            else
            {
                strfooterF = "";
            }
            string strHtml = string.Empty;
            string pdfFileName = System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + "test" + ".pdf";

            string pdfFileNamewithHeader = System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + ".pdf";
            if (htmlString.Length > 0)
            {
                CreatePDFFromHTMLFile(sbTop, pdfFileName);

                var reader = new PdfReader(pdfFileName);

                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {

                    // var document = new Document(reader.GetPageSizeWithRotation(1));


                    var document1 = new Document(new Rectangle(625, 975));
                    //Document document1 = new Document(new Rectangle(650, 975)); 
                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = reader.NumberOfPages;

                    //  string header = headerstring;
                    string[] Header = headerstring.Split('$');
                    string sProHeader = System.Configuration.ConfigurationSettings.AppSettings["WellnessNotesMainHeader"];

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        // contentByte.BeginText();

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();
                        if (sProHeader == "Y" && sNotesName == "WELLNESS NOTES")
                        {


                            float U = 75;
                            float S = 60f;



                            string pdfFilePath = Request.PhysicalApplicationPath + "\\Resources\\";
                            iTextSharp.text.Image eye = iTextSharp.text.Image.GetInstance(pdfFilePath + "/Palliative Care.gif");
                            eye.SetAbsolutePosition(pageSize.GetLeft(60), pageSize.GetTop(U));
                            eye.ScaleToFit(S, S);
                            con.AddImage(eye);
                            string FacilityName = System.Configuration.ConfigurationSettings.AppSettings["WellnessFacilityName"];
                            string FacilityAddress = System.Configuration.ConfigurationSettings.AppSettings["WellnessFacilityAddress"];
                            string FacilityPhone = System.Configuration.ConfigurationSettings.AppSettings["WellnessFacilityPhone"];


                            con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 16);

                            con.SetColorFill(BaseColor.RED);
                            con.SetTextMatrix(pageSize.GetLeft(160), pageSize.GetTop(40));
                            con.ShowText(FacilityName);
                            con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
                            con.SetTextMatrix(pageSize.GetLeft(180), pageSize.GetTop(55));
                            con.ShowText(FacilityAddress);
                            con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(250), pageSize.GetTop(70));
                            con.ShowText(FacilityPhone);
                        }
                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 7);

                        //  var Footer =  " page " + i ;

                        if (strfooterF == "" && strfooterP == "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        }
                        else if (strfooterF == "" && strfooterP != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterPA, 30, 30, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterP, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 10, 0);

                            //ColumnText ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterPA, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 77, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go();

                            //ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterP, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 65, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go(); 

                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 10, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        }


                        #region Column1
                        if (sProHeader == "Y" && sNotesName == "WELLNESS NOTES")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        for (int j = 1; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 103;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("PATIENT"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() + " " : string.Empty) + "\n");
                            }
                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }

                            X -= 103;
                            Y += 10;
                        }


                        #endregion


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();

                        #region Column2
                        X = 360f;
                        Y = 20f;

                        if (sProHeader == "Y" && sNotesName == "WELLNESS NOTES")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        for (int j = 3; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 20, pageSize.GetTop(Y));
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 97;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("ENCOUNTER"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length >= 3 ? Header[j].Split(':')[1].ToString() + ":" + Header[j].Split(':')[2].ToString() + ":" + Header[j].Split(':')[3].ToString() : string.Empty) + "\n");
                            }

                            else if (Header[j].Split(':')[0].ToUpper().Contains("FACILITY"))
                            {
                                if (Header[j].Split(':').Length > 1)
                                {
                                    string[] facility = Header[j].Split(':')[1].Split(',');
                                    string fac = "";
                                    if (facility.Length > 0)
                                    {
                                        for (int k = 0; k < facility.Length; k++)
                                        {
                                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                                            if (k == 0)
                                            {
                                                con.ShowText(":" + facility[k].ToString() + ",");
                                                Y += 10;

                                            }
                                            else if (k == facility.Length - 1)
                                            {
                                                fac = fac + facility[k] + ".";
                                            }
                                            else
                                            {
                                                fac = fac + facility[k] + ",";


                                            }
                                            // X -= 97;

                                        }
                                        con.SetTextMatrix(pageSize.GetLeft(X) - 37, pageSize.GetTop(Y));
                                        con.ShowText(fac + "\n");

                                    }
                                }
                            }

                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }
                            X -= 97;
                            Y += 10;
                        }



                        #endregion
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();
                        if (sProHeader == "Y" && sNotesName == "WELLNESS NOTES")
                        {

                            con.AddTemplate(importedPage, 0, 0);
                        }
                        else
                            con.AddTemplate(importedPage, 0, 60);

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
                if (File.Exists(pdfFileName))
                {
                    File.Delete(pdfFileName);
                    // string pdfPathCheck = path.Replace(".doc", ".pdf").ToString();

                }
            }
            else
            {
                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {

                    // var document = new Document(reader.GetPageSizeWithRotation(1));


                    var document1 = new Document(new Rectangle(625, 975));
                    //Document document1 = new Document(new Rectangle(650, 975)); 
                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = 1;

                    //  string header = headerstring;
                    string[] Header = headerstring.Split('$');
                    string sProHeader = System.Configuration.ConfigurationSettings.AppSettings["WellnessNotesMainHeader"];

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        //var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        // contentByte.BeginText();

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();
                        if (sProHeader == "Y" && sNotesName == "WELLNESS NOTES")
                        {


                            float U = 75;
                            float S = 60f;



                            string pdfFilePath = Request.PhysicalApplicationPath + "\\Resources\\";
                            iTextSharp.text.Image eye = iTextSharp.text.Image.GetInstance(pdfFilePath + "/Palliative Care.gif");
                            eye.SetAbsolutePosition(pageSize.GetLeft(60), pageSize.GetTop(U));
                            eye.ScaleToFit(S, S);
                            con.AddImage(eye);
                            string FacilityName = System.Configuration.ConfigurationSettings.AppSettings["WellnessFacilityName"];
                            string FacilityAddress = System.Configuration.ConfigurationSettings.AppSettings["WellnessFacilityAddress"];
                            string FacilityPhone = System.Configuration.ConfigurationSettings.AppSettings["WellnessFacilityPhone"];


                            con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 16);

                            con.SetColorFill(BaseColor.RED);
                            con.SetTextMatrix(pageSize.GetLeft(160), pageSize.GetTop(40));
                            con.ShowText(FacilityName);
                            con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
                            con.SetTextMatrix(pageSize.GetLeft(180), pageSize.GetTop(55));
                            con.ShowText(FacilityAddress);
                            con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(250), pageSize.GetTop(70));
                            con.ShowText(FacilityPhone);
                        }
                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 8);

                        if (strfooterF == "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        }
                        else if (strfooterF != "" && strfooterP != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterPA, 30, 30, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterP, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 10, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 10, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 10, 0);
                        }



                        #region Column1
                        if (sProHeader == "Y" && sNotesName == "WELLNESS NOTES")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        for (int j = 1; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 103;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("PATIENT"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() + " " : string.Empty) + "\n");
                            }
                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }

                            X -= 103;
                            Y += 10;
                        }


                        #endregion


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();

                        #region Column2
                        X = 360f;
                        Y = 20f;

                        if (sProHeader == "Y" && sNotesName == "WELLNESS NOTES")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;
                        for (int j = 3; j < Header.Length; j += 4)
                        {
                            con.SetFontAndSize(baseFont1, 10);
                            con.SetColorFill(BaseColor.BLACK);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 20, pageSize.GetTop(Y));
                            con.ShowText(Header[j].Split(':')[0].ToString());
                            X += 97;
                            con.SetColorFill(BaseColor.BLUE);
                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                            if (Header[j].Split(':')[0].ToUpper().Contains("ENCOUNTER"))
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length >= 3 ? Header[j].Split(':')[1].ToString() + ":" + Header[j].Split(':')[2].ToString() + ":" + Header[j].Split(':')[3].ToString() : string.Empty) + "\n");
                            }

                            else if (Header[j].Split(':')[0].ToUpper().Contains("FACILITY"))
                            {
                                if (Header[j].Split(':').Length > 1)
                                {
                                    string[] facility = Header[j].Split(':')[1].Split(',');
                                    string fac = "";
                                    if (facility.Length > 0)
                                    {
                                        for (int k = 0; k < facility.Length; k++)
                                        {
                                            con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                                            if (k == 0)
                                            {
                                                con.ShowText(":" + facility[k].ToString() + ",");
                                                Y += 10;

                                            }
                                            else if (k == facility.Length - 1)
                                            {
                                                fac = facility[k] + ".";
                                            }
                                            else
                                            {
                                                fac = facility[k] + ",";


                                            }
                                            // X -= 97;

                                        }
                                        con.SetTextMatrix(pageSize.GetLeft(X) - 37, pageSize.GetTop(Y));
                                        con.ShowText(fac + "\n");

                                    }
                                }
                            }

                            else
                            {
                                con.ShowText(": " + (Header[j].Split(':').Length > 1 ? Header[j].Split(':')[1].ToString() : string.Empty) + "\n");
                            }
                            X -= 97;
                            Y += 10;
                        }



                        #endregion
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();
                        //if (sProHeader == "Y" && sNotesName == "WELLNESS NOTES")
                        //{

                        //    con.AddTemplate(importedPage, 0, 0);
                        //}
                        //else
                        //    con.AddTemplate(importedPage, 0, 60);

                    }

                    document1.Close();
                    writerpdf.Close();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            Response.ContentType = "application/x-download";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", NotesName + ".pdf"));
            Response.WriteFile(pdfFileNamewithHeader);

            Response.Flush();
            System.IO.File.Delete(pdfFileNamewithHeader);
            Response.End();
            // }
        }
        string ExtractBetween(string text, string start, string end)
        {
            int iStart = text.IndexOf(start);
            iStart = (iStart == -1) ? 0 : iStart + start.Length;
            int iEnd = text.LastIndexOf(end);
            if (iEnd == -1)
            {
                iEnd = text.Length;
            }
            int len = iEnd - iStart;

            return text.Substring(iStart, len);
        }
        public void CreatePDFFromHTMLFile(string HtmlStream, string FileName)
        {
            try
            {
                object TargetFile = FileName;
                string ModifiedFileName = string.Empty;
                string FinalFileName = string.Empty;



                GeneratePDF.HtmlToPdfBuilder builder = new GeneratePDF.HtmlToPdfBuilder(iTextSharp.text.PageSize.A4);
                GeneratePDF.HtmlPdfPage first = builder.AddPage();
                first.AppendHtml(HtmlStream);
                byte[] file = builder.RenderPdf();
                File.WriteAllBytes(TargetFile.ToString(), file);

                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(TargetFile.ToString());
                ModifiedFileName = TargetFile.ToString();
                ModifiedFileName = ModifiedFileName.Insert(ModifiedFileName.Length - 4, "1");
                iTextSharp.text.pdf.PdfEncryptor.Encrypt(reader, new FileStream(ModifiedFileName, FileMode.Append), iTextSharp.text.pdf.PdfWriter.STRENGTH128BITS, "", "", iTextSharp.text.pdf.PdfWriter.ALLOW_SCREENREADERS);

                // iTextSharp.text.pdf.PdfEncryptor.Encrypt()
                reader.Close();
                if (File.Exists(TargetFile.ToString()))
                    File.Delete(TargetFile.ToString());
                FinalFileName = ModifiedFileName.Remove(ModifiedFileName.Length - 5, 1);
                File.Copy(ModifiedFileName, FinalFileName);
                if (File.Exists(ModifiedFileName))
                    File.Delete(ModifiedFileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DownloadFromImageServer(string encounterId, string HumanID, string serverIP, string localPath)
        {
            string uri = string.Empty;

            /* Credential To Access NAS Server */
            string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPath"];
            string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPath"];
            string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            string userName = System.Configuration.ConfigurationSettings.AppSettings["UserName"];
            string password = System.Configuration.ConfigurationSettings.AppSettings["Password"];
            string domain = System.Configuration.ConfigurationSettings.AppSettings["Domain"];
            bool result = false;

            if (HumanID != "0" && encounterId != "0")
            {
                // uri = serverIP + HumanID +"\\" + encounterId +"\\" + FileName+".pdf";
                uri = serverIP + HumanID + "\\" + encounterId + "\\";
            }

            using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
            {
                if (unc.NetUseWithCredentials(UNCAuthPath, userName, domain, password))
                {
                    {

                        try
                        {

                            string[] filePaths = Directory.GetFiles((uri.Replace(ftpIP, UNCPath)));
                            if (filePaths.Count() > 0)
                            {




                                var myFile = Directory.GetFiles((uri.Replace(ftpIP, UNCPath)))
                                .Select(x => new FileInfo(x)).Where(x => x.Extension.ToUpper() == ".PDF")
                               .OrderByDescending(x => x.LastWriteTime)
                                .Take(1)
                               .ToArray();
                                if (myFile.Count() > 0)
                                {
                                    Name = myFile[0].Name.ToString();
                                    if (File.Exists(Path.Combine(localPath, Name)))
                                    {
                                        File.Delete(Path.Combine(localPath, Name));


                                    }
                                    uri = uri + "\\" + Name;

                                    System.IO.File.Copy(uri.Replace(ftpIP, UNCPath), (Path.Combine(localPath, Name)), true);
                                    result = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            if (e.Message.Contains("DirectoryNotFoundException"))
                                result = false;
                        }

                    }
                }
            }
            return result;
        }

        public string FindAndReplace(string sOriginalString, string sFindValue, string sReplaceValue)
        {
            string sResult = string.Empty;
            int place = sOriginalString.LastIndexOf(sFindValue);

            if (place == -1)
            {
                return sResult;
            }
            else
            {
                sResult = sOriginalString.Remove(place, sFindValue.Length).Insert(place, sReplaceValue);
                return sResult;
            }
        }


        public string Before(string value, string a)
        {
            int posA = value.IndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }
        public string After(string value, string a)
        {
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }
    }


}