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
using System.Diagnostics;
using System.Threading;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace Acurus.Capella.UI
{
    public partial class frmConsultationNotes : System.Web.UI.Page
    {

        string sXMLHumanDoc = string.Empty;
        string sXMLEncounterDoc = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            ulong Encounter_Id = 0;
            string FileNames = string.Empty;

            if (Request.QueryString["EncounterId"] != null)
            {
                Encounter_Id = Convert.ToUInt32(Request.QueryString["EncounterId"].ToString());
                // ClientSession.Selectedencounterid = Encounter_Id;
            }
            EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
            HumanBlobManager HumanBlobMngr=new HumanBlobManager();
            IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
            ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(ClientSession.HumanId));
            if (ilstHumanBlob.Count > 0)
            {
                sXMLHumanDoc = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                if (sXMLHumanDoc.Substring(0, 1) != "<")
                    sXMLHumanDoc = sXMLHumanDoc.Substring(1, sXMLHumanDoc.Length - 1);
                //Jira #CAP-115
                sXMLHumanDoc = UtilityManager.ReplaceSpecialCharaters(sXMLHumanDoc);
            }
            if (Encounter_Id != 0)

            {
                IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
                ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Encounter_Id);
                if (ilstEncounterBlob.Count > 0)
                {
                    sXMLEncounterDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                    if (sXMLEncounterDoc.Substring(0, 1) != "<")
                        sXMLEncounterDoc = sXMLEncounterDoc.Substring(1, sXMLEncounterDoc.Length - 1);
                    //Jira #CAP-115
                    sXMLEncounterDoc = UtilityManager.ReplaceSpecialCharaters(sXMLEncounterDoc);
                }
                FileNames = "Encounter" + "_" + Encounter_Id + ".xml";
            }
            else
            {
                FileNames = "Encounter" + "_" + ClientSession.EncounterId + ".xml";

                IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
                ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ClientSession.EncounterId);
                if (ilstEncounterBlob.Count > 0)
                {
                    sXMLEncounterDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                    if (sXMLEncounterDoc.Substring(0, 1) != "<")
                        sXMLEncounterDoc = sXMLEncounterDoc.Substring(1, sXMLEncounterDoc.Length - 1);
                    //Jira #CAP-115
                    sXMLEncounterDoc = UtilityManager.ReplaceSpecialCharaters(sXMLEncounterDoc);
                }
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "DisplayErrorMessage('110063'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }


               
            
            if (!IsPostBack)
            {
                XmlDocument itemDoc = new XmlDocument();
                string sXMLContent = string.Empty;

                  string sXMLHumanDoc = string.Empty;
        string sXMLEncounterDoc = string.Empty;
                UpdateDOSinBlob(ClientSession.HumanId);

                if (Request.QueryString["Menu"] != null && Request.QueryString["Menu"].Contains("Word"))
                {

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "downloadFile();", true);

                }
                else if (Request.QueryString["Menu"] != null && Request.QueryString["Menu"].Contains("PDF"))
                {

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "downloadFilePDF();", true);

                }
                else if (Request.QueryString["Menu"] != null && Request.QueryString["Menu"].Contains("FAX"))
                {

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "downloadFileFax();", true);

                }
            }
        }
      
      
        protected void btnFax_Click(object sender, EventArgs e)
        {
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //string FileName = strXmlEncounterPath; ;// "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml"; ;
            // string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);


            //string xmlDataFile = strXmlEncounterPath;
            string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Consultation_Notes.xsl");
            string OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["ConsultationNotesNamingConvention"];
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            string[] dtFormat = OutputNamingConvention.Split('^');
            string sDateFormat = string.Empty;
            if (dtFormat.Count() > 1)
            {
                if (dtFormat[1].Contains("@"))
                {
                    dtFormat[1] = dtFormat[1].Split('@')[0];
                }
            }
            string[] FormatCase = OutputNamingConvention.Split('@');
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    //if (result.ToUpper().Contains("FACILITY"))
                    //{
                    //    if (FormatCase.Count() > 1)
                    //    {
                    //        if (FormatCase[1].ToUpper().Contains("YES"))
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", "")).ToUpper();
                    //        else
                    //        {
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //        }
                    //        NotesName = NotesName.Replace(FormatCase[1], "");
                    //    }
                    //    else
                    //    {
                    //        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //    }

                    //}

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
                        //try
                        //{
                        //if (File.Exists(strXmlHumanPath) == true)
                        if (sXMLHumanDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLHumanDoc);
                            //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                if (result.ToUpper().Contains("MEMBER"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value != "")
                                    {
                                        sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                }
                                else if (result.ToUpper().Contains("LAST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("FIRST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value != "")
                                    {
                                        if (dtFormat.Count() > 1)
                                        {
                                            if (dtFormat[1] == "yyyyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            else if (dtFormat[1] == "MMddyyyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyyyy");
                                            else if (dtFormat[1] == "MMddyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyy");
                                            else if (dtFormat[1] == "yyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyMMdd");
                                            else
                                            {
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            }
                                            NotesName = NotesName.Replace(dtFormat[1], "");
                                        }
                                        else
                                        {
                                            sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                        //}

                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("FACILITY"))
                    {

                        //try
                        //{
                        //if (File.Exists(strXmlEncounterPath) == true)
                        if (sXMLEncounterDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                string sDOS = string.Empty;

                                if ((result.ToUpper().Contains("DOS") || OutputName[i].ToUpper().Contains("DATE")) && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                {
                                    if (dtFormat.Count() > 1)
                                    {
                                        if (dtFormat[1] == "yyyyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        else if (dtFormat[1] == "MMddyyyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyyyy");
                                        else if (dtFormat[1] == "MMddyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyy");
                                        else if (dtFormat[1] == "yyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyMMdd");
                                        else
                                        {
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        }
                                        NotesName = NotesName.Replace(dtFormat[1], "");
                                    }
                                    else
                                    {
                                        sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOS);
                                }
                                else if (result.ToUpper().Contains("FACILITY") && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Trim() != "")
                                {
                                    if (FormatCase.Count() > 1)
                                    {
                                        if (FormatCase[1].ToUpper().Contains("YES"))
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", "")).ToUpper();
                                        else
                                        {
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                        }
                                        NotesName = NotesName.Replace(FormatCase[1], "");
                                    }
                                    else
                                    {
                                        NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                    }
                                }
                                else
                                {
                                    NotesName = NotesName.Replace("[" + result + "]", "");
                                }

                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlEncounterPath);
                        //}
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
            NotesName = NotesName.Replace("~", "").Replace("__", "_").Replace("^", "").Replace("@", "");
            string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);

            DataSet ds;
            XmlDataDocument xmlDoc;
            XslCompiledTransform xslTran;
            XmlElement root;
            XPathNavigator nav;
            XmlTextWriter writer;
            XsltSettings settings = new XsltSettings(true, false);

            ds = new DataSet();
            //ds.ReadXml(xmlDataFile);
            //ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));
            StringBuilder sb = new StringBuilder();
            sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
            ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));

            xmlDoc = new XmlDataDocument(ds);
            xslTran = new XslCompiledTransform();
            // xslTran.Load(xsltFile);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            root = xmlDoc.DocumentElement;


            nav = root.CreateNavigator();
            if (File.Exists(outputDocument))
            {
                File.Delete(outputDocument);
            }
            writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            xslTran.Transform(nav, writer);
            writer.Close();
            writer = null;
            nav = null;
            root = null;
            xmlDoc = null;
            ds = null;
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);
            //  string htmlString = System.IO.File.ReadAllText(outputDocument);

            //  IList<PatientPane> lstpane = new List<PatientPane>();
            //   lstpane = ClientSession.PatientPaneList.ToList<PatientPane>();
            string Patient_Name = "";
            // if(lstpane.Count>0)
            // Patient_Name=lstpane[0].Last_Name + "," + "" + lstpane[0].First_Name + " " + lstpane[0].MI;

            string Encounter_signedDate = "";

            string Encounter_Provider_Name = "";
            string Provider_Speciality = "";
            TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
            XDocument xmlDocumentType = XDocument.Load(EncXMLContent);
            TextReader HumanXMLContentdoc = new StringReader(sXMLHumanDoc);
            XDocument xmlDocument = XDocument.Load(HumanXMLContentdoc);
            string Encounter_Provider_id = "";

            foreach (XElement elements in xmlDocument.Descendants("HumanList"))
            {

                foreach (XElement Human in elements.Elements())
                {
                    if (Human.Attribute("First_Name") != null && Human.Attribute("Last_Name") != null && Human.Attribute("MI") != null)
                        Patient_Name = Human.Attribute("Last_Name").Value + "," + "" + Human.Attribute("First_Name").Value + " " + Human.Attribute("MI").Value;

                }
            }





            string Encounter_Reviewed_signedDate = "";
            string Encounter_Reviewed_Name = "";
            string Encounter_Reviewed_Id = "";
            // xmlDocumentType = XDocument.Load(strXmlEncounterPath);

            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                    Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                    DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                    Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;

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


            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    // if (Encounter.LastAttribute != null && Encounter.LastAttribute.Name.ToString() == "Specialties")
                    if (Encounter.Attribute("Specialties") != null)
                    {
                        Provider_Speciality = Encounter.LastAttribute.Value;
                        break;


                    }


                }

            }



            string strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
            //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
            //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

            string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            StaticLookupManager staticMngr = new StaticLookupManager();
            string strfooterProviderReviewed = string.Empty;
            IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            if (CommonList.Count > 0)
                strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");


            string htmlString = System.IO.File.ReadAllText(outputDocument);
            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();
            string imgpath = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath"];
            string imgpath_phy = System.Configuration.ConfigurationSettings.AppSettings["phyConsultationLogoPath"];
            string imgpath_PalliativeCare = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_PalliativeCare"];
            string imgpath_Integrum = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_Integrum"];

            string phyheaderstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_phy + "' alt='logo' height='100' width='200'></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string header = "<table style='width:100%'><tr><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_PalliativeCare = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_PalliativeCare + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_Integrum = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_Integrum + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string finalHeadeString = string.Empty;
            string PatientName = "RE: " + Patient_Name;

            string sProHeader = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesMainHeader"];
            if (System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] != null && System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] == "ALL")
                finalHeadeString = imgpath_Integrum;
            else if (Provider_Speciality.ToUpper().Contains("ENDOCRINOLOGY"))
                finalHeadeString = imgpath;
            else if (Encounter_Provider_id == "4198")
                finalHeadeString = imgpath_phy;
            else if (Encounter_Provider_id == "3101" || Encounter_Provider_id == "321")
                finalHeadeString = imgpath_PalliativeCare;
            else
                finalHeadeString = "";


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


            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            // var strBody = new StringBuilder();

            string sbTop = "";

            sbTop = sbTop + htmlString;


            string strHtml = string.Empty;
            string pdfFileName = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + "test" + ".pdf");//System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + "test" + ".pdf";

            string pdfFileNamewithHeader = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + ".pdf");//)System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + ".pdf";

            if (htmlString.Length > 0)
            {
                try
                {
                    CreatePDFFromHTMLFile(sbTop, pdfFileName);
                }

                catch (Exception ex)
                {
                    if (ex.Message.ToUpper().Contains("NO PAGES"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('1011187'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;

                    }
                    else
                        throw ex;
                }

                var reader = new PdfReader(pdfFileName);

                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {


                    var document1 = new Document(new Rectangle(625, 975));

                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = reader.NumberOfPages;



                    sProHeader = "N";
                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();

                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 9);

                        if (strfooterF == "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterPA != "" && strfooterP != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterPA, 30, 30, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterP, 30, 20, 0);

                            //ColumnText ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterPA, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 77, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go();

                            //ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterP, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 65, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go(); 

                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }

                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;

                        con.SetFontAndSize(baseFont1, 10);
                        con.SetColorFill(BaseColor.BLACK);
                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));

                        X += 103;

                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        // string imagepath = "";

                        if (finalHeadeString != "")
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(finalHeadeString);
                            logo.ScaleAbsolute(50, 50);
                            logo.SetAbsolutePosition(pageSize.GetLeft(X) - 150, pageSize.GetTop(Y) - 30);
                            con.AddImage(logo);


                        }


                        con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PatientName, pageSize.GetLeft(X) + 200, pageSize.GetTop(Y) - 30, 0);
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();
                        if (sProHeader == "Y")
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

                }
            }
            else
            {
                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {
                    var document1 = new Document(new Rectangle(625, 975));

                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = 1;


                    sProHeader = "N";

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var con = writerpdf.DirectContent;

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();
                        if (sProHeader == "Y")
                        {


                            //float U = 75;
                            //float S = 60f;


                        }
                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 8);

                        if (strfooterProvider != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProvider, 30, 30, 0);
                            if (strfooterProviderReviewed != "")
                                con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProviderReviewed, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterF != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));


                        #region Column1
                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;

                        con.SetFontAndSize(baseFont1, 10);
                        con.SetColorFill(BaseColor.BLACK);
                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        if (finalHeadeString != "")
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(finalHeadeString);
                            logo.ScaleAbsolute(50, 50);
                            logo.SetAbsolutePosition(pageSize.GetLeft(X) - 150, pageSize.GetTop(Y) - 30);
                            con.AddImage(logo);


                        }


                        con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PatientName, pageSize.GetLeft(X) + 200, pageSize.GetTop(Y) - 30, 0);



                        #endregion


                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();


                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();

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
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

        }


        protected void btnword_Click(object sender, EventArgs e)
        {
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Document : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //string FileName = strXmlEncounterPath;// "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml"; ;
            // string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);


            //string xmlDataFile = strXmlEncounterPath;
            string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Consultation_Notes.xsl");
            string OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["ConsultationNotesNamingConvention"];
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            string[] dtFormat = OutputNamingConvention.Split('^');
            string sDateFormat = string.Empty;
            if (dtFormat.Count() > 1)
            {
                if (dtFormat[1].Contains("@"))
                {
                    dtFormat[1] = dtFormat[1].Split('@')[0];
                }
            }
            string[] FormatCase = OutputNamingConvention.Split('@');
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    //if (result.ToUpper().Contains("FACILITY"))
                    //{
                    //    if (FormatCase.Count() > 1)
                    //    {
                    //        if (FormatCase[1].ToUpper().Contains("YES"))
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", "")).ToUpper();
                    //        else
                    //        {
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //        }
                    //        NotesName = NotesName.Replace(FormatCase[1], "");
                    //    }
                    //    else
                    //    {
                    //        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //    }

                    //}

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
                        //try
                        //{
                        //if (File.Exists(strXmlHumanPath) == true)
                        if (sXMLHumanDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLHumanDoc);
                            //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                if (result.ToUpper().Contains("MEMBER"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value != "")
                                    {
                                        sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                }
                                else if (result.ToUpper().Contains("LAST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("FIRST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value != "")
                                    {
                                        if (dtFormat.Count() > 1)
                                        {
                                            if (dtFormat[1] == "yyyyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            else if (dtFormat[1] == "MMddyyyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyyyy");
                                            else if (dtFormat[1] == "MMddyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyy");
                                            else if (dtFormat[1] == "yyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyMMdd");
                                            else
                                            {
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            }
                                            NotesName = NotesName.Replace(dtFormat[1], "");
                                        }
                                        else
                                        {
                                            sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                        //}

                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("FACILITY"))
                    {
                        //try
                        //{
                        //if (File.Exists(strXmlEncounterPath) == true)
                        if (sXMLEncounterDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                string sDOS = string.Empty;
                                if (((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE"))) && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                {
                                    if (dtFormat.Count() > 1)
                                    {
                                        if (dtFormat[1] == "yyyyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        else if (dtFormat[1] == "MMddyyyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyyyy");
                                        else if (dtFormat[1] == "MMddyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyy");
                                        else if (dtFormat[1] == "yyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyMMdd");
                                        else
                                        {
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        }
                                        NotesName = NotesName.Replace(dtFormat[1], "");
                                    }
                                    else
                                    {
                                        sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOS);
                                }
                                else if (result.ToUpper().Contains("FACILITY") && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Trim() != "")
                                {
                                    if (FormatCase.Count() > 1)
                                    {
                                        if (FormatCase[1].ToUpper().Contains("YES"))
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", "")).ToUpper();
                                        else
                                        {
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                        }
                                        NotesName = NotesName.Replace(FormatCase[1], "");
                                    }
                                    else
                                    {
                                        NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                    }
                                }
                                else
                                {
                                    NotesName = NotesName.Replace("[" + result + "]", "");
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlEncounterPath);
                        //}
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
            NotesName = NotesName.Replace("~", "").Replace("__", "_").Replace("^", "").Replace("@", "");
            string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);

            DataSet ds;
            XmlDataDocument xmlDoc;
            XslCompiledTransform xslTran;
            XmlElement root;
            XPathNavigator nav;
            XmlTextWriter writer;
            XsltSettings settings = new XsltSettings(true, false);

            ds = new DataSet();
            //ds.ReadXml(xmlDataFile);
            // ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));
            StringBuilder sb = new StringBuilder();
            sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
            ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));

            xmlDoc = new XmlDataDocument(ds);
            xslTran = new XslCompiledTransform();
            // xslTran.Load(xsltFile);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Document XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Document XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            root = xmlDoc.DocumentElement;


            nav = root.CreateNavigator();
            if (File.Exists(outputDocument))
            {
                File.Delete(outputDocument);
            }
            writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            xslTran.Transform(nav, writer);
            writer.Close();
            writer = null;
            nav = null;
            root = null;
            xmlDoc = null;
            ds = null;
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);
            string htmlString = System.IO.File.ReadAllText(outputDocument);

            //  IList<PatientPane> lstpane = new List<PatientPane>();
            //   lstpane = ClientSession.PatientPaneList.ToList<PatientPane>();
            string Patient_Name = "";
            // if(lstpane.Count>0)
            // Patient_Name=lstpane[0].Last_Name + "," + "" + lstpane[0].First_Name + " " + lstpane[0].MI;


            string Encounter_signedDate = "";
            string Encounter_Provider_Name = "";
            string Encounter_Reviewed_signedDate = "";
            string Encounter_Reviewed_Name = "";
            string Encounter_Reviewed_Id = "";
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

                }

            }
            //Provider Name 
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    Encounter_Provider_Name = Encounter.Value;
                    break;
                }
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




            string Provider_Speciality = "";
            TextReader EncXMLContentdoc = new StringReader(sXMLEncounterDoc);
            xmlDocumentType = XDocument.Load(EncXMLContentdoc);
            TextReader HumanXMLContentdoc = new StringReader(sXMLHumanDoc);
            XDocument xmlDocument = XDocument.Load(HumanXMLContentdoc);

            string Encounter_Provider_id = "";

            foreach (XElement elements in xmlDocument.Descendants("HumanList"))
            {

                foreach (XElement Human in elements.Elements())
                {
                    if (Human.Attribute("First_Name") != null && Human.Attribute("Last_Name") != null && Human.Attribute("MI") != null)
                        Patient_Name = Human.Attribute("Last_Name").Value + "," + "" + Human.Attribute("First_Name").Value + " " + Human.Attribute("MI").Value;

                }
            }





            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {

                foreach (XElement Encounter in elements.Elements())
                {
                    Encounter_Provider_Name = Encounter.Value;
                    break;

                }
                break;
            }
            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    // if (Encounter.LastAttribute != null && Encounter.LastAttribute.Name.ToString() == "Specialties")
                    if (Encounter.Attribute("Specialties") != null)
                    {
                        Provider_Speciality = Encounter.LastAttribute.Value;
                        break;


                    }


                }

            }



            //string strfooter = "Electronically Signed by ";


            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();
            string imgpath = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath"];
            string imgpath_phy = System.Configuration.ConfigurationSettings.AppSettings["phyConsultationLogoPath"];
            string imgpath_PalliativeCare = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_PalliativeCare"];
            string imgpath_Integrum = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_Integrum"];

            string phyheaderstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_phy + "' alt='logo' height='100' width='200'></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string header = "<table style='width:100%'><tr><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_PalliativeCare = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_PalliativeCare + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_Integrum = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_Integrum + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";


            StringBuilder sbTop = new System.Text.StringBuilder();
            sbTop.Append(@"
<html 
xmlns:o='urn:schemas-microsoft-com:office:office' 
xmlns:w='urn:schemas-microsoft-com:office:word'
xmlns='http://www.w3.org/TR/REC-html40'>
<head><title></title>

<!--[if gte mso 9]>
<xml>
<w:WordDocument>
<w:View>Print</w:View>
<w:Zoom>90</w:Zoom>
<w:DoNotOptimizeForBrowser/>
</w:WordDocument>
</xml>
<![endif]-->


<style>
p.MsoFooter, li.MsoFooter, div.MsoFooter
{
margin:0in;
margin-bottom:.0001pt;
mso-pagination:widow-orphan;
tab-stops:center 3.0in right 6.0in;
font-size:12.0pt;
}
<style>

<!-- /* Style Definitions */

@page Section1
{
size:8.5in 11.0in; 
margin:1.0in 1.25in 1.0in 1.25in ;
mso-header-margin:.5in;
mso-header:h1;
mso-footer: f1; 
mso-footer-margin:.5in;
}


div.Section1
{
page:Section1;
}

table#hrdftrtbl
{
margin:0in 0in 0in 9in;
}
-->
</style></head>

<body lang=EN-US style='tab-interval:.5in'>
<div class=Section1>");
            sbTop.Append(htmlString);
            sbTop.Append(@"<table id='hrdftrtbl' border='1' cellspacing='0' cellpadding='0'>
<tr><td>
<div style='mso-element:header' id=h1 >");
            if (System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] != null && System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] == "ALL")
                sbTop.Append(headerLogo_Integrum);
            else if (Provider_Speciality.ToUpper().Contains("ENDOCRINOLOGY"))
                sbTop.Append(headerstring);
            else if (Encounter_Provider_id == "4198")
                sbTop.Append(phyheaderstring);
            else if (Encounter_Provider_id == "3101" || Encounter_Provider_id == "321")
                sbTop.Append(headerLogo_PalliativeCare);
            else
                sbTop.Append(header);

            sbTop.Append(@"
</div>
</td>
<td>");

            string strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
            //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
            //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

            string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            StaticLookupManager staticMngr = new StaticLookupManager();
            string strfooterProviderReviewed = string.Empty;
            IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            if (CommonList.Count > 0)
                strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");


            string strfooterF = "";


            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterProvider = strfooterProvider + "<br/>" + strfooterProviderReviewed;
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            {
                strfooterF = strfooterProvider;
            }
            else
            {


                strfooterF = "";
                strfooterProvider = "";
                strfooterProviderReviewed = "";
            }
            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM" && Encounter_Reviewed_signedDate != "" && Encounter_Reviewed_signedDate != "01-Jan-0001 12:00 AM")
            {
                sbTop.Append(@"<div style='mso-element:footer;font-family:Arial;font-size:9pt' id=f1><p class=MsoFooter style='font-family:Arial;font-size:9pt'>" + strfooterProvider +
@"<span style=padding-right:10px;mso-tab-count:2'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span></p></div>
</td></tr>

</table>

</body></html>
");
            }
            else if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00 AM")
            {
                sbTop.Append(@"<div style='mso-element:footer;font-family:Arial;font-size:9pt' id=f1><p class=MsoFooter style='font-family:Arial;font-size:9pt'>" + strfooterProvider +
@"<span style=padding-right:10px;mso-tab-count:2'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span></p></div>
</td></tr>

</table>

</body></html>
");
            }
            else
            {

                sbTop.Append(@"<div style='mso-element:footer;' id=f1><p class=MsoFooter><span style=padding-right:10px;mso-tab-count:2;font-family:Arial;font-size:9pt'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span>
</p></div>
</td></tr>
</table>
</body></html>
");
            }

            sbTop.Replace("\uFFFD", " ");
            if (System.Configuration.ConfigurationManager.AppSettings["DocToPdf"].ToUpper() == "Y")
            {
                string path = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["UserSessionFolderPath"], Session.SessionID + "DocToPdf") + "/" + ClientSession.FacilityName.Replace(",", "") + "_Consultation_Notes_" + ClientSession.EncounterId + ".doc";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    string pdfPathCheck = path.Replace(".doc", ".pdf").ToString();
                    File.Delete(pdfPathCheck);
                }

                if (!File.Exists(path))
                {
                    string strbody = sbTop.ToString();
                    File.WriteAllText(path, strbody);
                }
                DocToPdf objDoctoPdf = new DocToPdf();
                FileInfo filepath = new FileInfo(path);
                Boolean ConvertCheck = objDoctoPdf.ConvertDocToPdf(filepath);
                if (ConvertCheck)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + NotesName + ".pdf");
                    Response.ContentType = "application/pdf";
                    Response.TransmitFile(path.Replace(".doc", ".pdf"));
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + NotesName + ".doc");
                Response.ContentType = "application/msword";


                Response.Write(sbTop);
                Response.Flush();
                Response.End();
            }

            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation Document : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

        }
        protected void btnpdf_click(object sender, EventArgs e)
        {
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            //string FileName = strXmlEncounterPath; ;// "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml"; ;
            // string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);


            //string xmlDataFile = strXmlEncounterPath;
            string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Consultation_Notes.xsl");
            string OutputNamingConvention = System.Configuration.ConfigurationSettings.AppSettings["ConsultationNotesNamingConvention"];
            string sNamingConvention = string.Empty;
            string[] OutputName = OutputNamingConvention.Split('~');
            string NotesName = OutputNamingConvention;
            string result = string.Empty;
            string[] dtFormat = OutputNamingConvention.Split('^');
            string sDateFormat = string.Empty;
            if (dtFormat.Count() > 1)
            {
                if (dtFormat[1].Contains("@"))
                {
                    dtFormat[1] = dtFormat[1].Split('@')[0];
                }
            }
            string[] FormatCase = OutputNamingConvention.Split('@');
            for (int i = 0; i < OutputName.Length; i++)
            {
                if (OutputName[i] != "")
                {
                    result = ExtractBetween(OutputName[i], "[", "]");
                    //if (result.ToUpper().Contains("FACILITY"))
                    //{
                    //    if (FormatCase.Count() > 1)
                    //    {
                    //        if (FormatCase[1].ToUpper().Contains("YES"))
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", "")).ToUpper();
                    //        else
                    //        {
                    //            NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //        }
                    //        NotesName = NotesName.Replace(FormatCase[1], "");
                    //    }
                    //    else
                    //    {
                    //        NotesName = NotesName.Replace("[" + result + "]", ClientSession.FacilityName.Replace(",", ""));
                    //    }

                    //}

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
                        //try
                        //{
                        //if (File.Exists(strXmlHumanPath) == true)
                        if (sXMLHumanDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLHumanDoc);
                            //using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                            {
                                if (result.ToUpper().Contains("MEMBER"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value != "")
                                    {
                                        sExternalAccNo = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Patient_Account_External").Value.ToString();
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sExternalAccNo);
                                }
                                else if (result.ToUpper().Contains("LAST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sLastName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sLastName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("FIRST"))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != "")
                                    {
                                        if (FormatCase.Count() > 1)
                                        {
                                            if (FormatCase[1].ToUpper().Contains("YES"))
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToUpper().ToString();
                                            else
                                            {
                                                sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                            }
                                            NotesName = NotesName.Replace(FormatCase[1], "");
                                        }
                                        else
                                        {
                                            sFirstName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", (sFirstName.Replace("/", "").Replace(",", "_").Replace(":", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("*", "").Replace("?", "").Replace(";", "").Replace("\\", "").Replace("\"", "")).Trim());
                                }
                                else if (result.ToUpper().Contains("DOB") || (OutputName[i].ToUpper().Contains("DATE") && OutputName[i].ToUpper().Contains("BIRTH")))
                                {
                                    if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value != "")
                                    {
                                        if (dtFormat.Count() > 1)
                                        {
                                            if (dtFormat[1] == "yyyyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            else if (dtFormat[1] == "MMddyyyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyyyy");
                                            else if (dtFormat[1] == "MMddyy")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("MMddyy");
                                            else if (dtFormat[1] == "yyMMdd")
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyMMdd");
                                            else
                                            {
                                                sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                            }
                                            NotesName = NotesName.Replace(dtFormat[1], "");
                                        }
                                        else
                                        {
                                            sDOB = Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("yyyyMMdd");
                                        }
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOB);
                                }
                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlHumanPath);
                        //}

                    }
                    if ((result.ToUpper().Contains("DOS")) || (OutputName[i].ToUpper().Contains("DATE")) || result.ToUpper().Contains("FACILITY"))
                    {

                        //try
                        //{
                        //if (File.Exists(strXmlEncounterPath) == true)
                        if (sXMLEncounterDoc != string.Empty)
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            //XmlTextReader XmlText = new XmlTextReader(strXmlEncounterPath);
                            // itemDoc.Load(XmlText);
                            itemDoc.LoadXml(sXMLEncounterDoc);
                            //using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //{
                            //    itemDoc.Load(fs);

                            //    XmlText.Close();
                            if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                            {
                                string sDOS = string.Empty;

                                if ((result.ToUpper().Contains("DOS") || OutputName[i].ToUpper().Contains("DATE")) && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value != "")
                                {
                                    if (dtFormat.Count() > 1)
                                    {
                                        if (dtFormat[1] == "yyyyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        else if (dtFormat[1] == "MMddyyyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyyyy");
                                        else if (dtFormat[1] == "MMddyy")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("MMddyy");
                                        else if (dtFormat[1] == "yyMMdd")
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyMMdd");
                                        else
                                        {
                                            sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                        }
                                        NotesName = NotesName.Replace(dtFormat[1], "");
                                    }
                                    else
                                    {
                                        sDOS = Convert.ToDateTime(itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value).ToString("yyyyMMdd");
                                    }
                                    NotesName = NotesName.Replace("[" + result + "]", sDOS);
                                }
                                else if (result.ToUpper().Contains("FACILITY") && itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Trim() != "")
                                {
                                    if (FormatCase.Count() > 1)
                                    {
                                        if (FormatCase[1].ToUpper().Contains("YES"))
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", "")).ToUpper();
                                        else
                                        {
                                            NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                        }
                                        NotesName = NotesName.Replace(FormatCase[1], "");
                                    }
                                    else
                                    {
                                        NotesName = NotesName.Replace("[" + result + "]", itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes.GetNamedItem("Facility_Name").Value.Replace(",", ""));
                                    }
                                }
                                else
                                {
                                    NotesName = NotesName.Replace("[" + result + "]", "");
                                }

                            }
                            //    fs.Close();
                            //    fs.Dispose();
                            //}
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(ex.Message + " - " + strXmlEncounterPath);
                        //}
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
            NotesName = NotesName.Replace("~", "").Replace("__", "_").Replace("^", "").Replace("@", "");
            string WordOutputName = NotesName + ".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);

            DataSet ds;
            XmlDataDocument xmlDoc;
            XslCompiledTransform xslTran;
            XmlElement root;
            XPathNavigator nav;
            XmlTextWriter writer;
            XsltSettings settings = new XsltSettings(true, false);

            ds = new DataSet();
            //ds.ReadXml(xmlDataFile);
            //ds.ReadXml(new XmlTextReader(new StringReader(sXMLEncounterDoc)));
            StringBuilder sb = new StringBuilder();
            sb.Append(sXMLEncounterDoc.ToString().Replace("</notes>", "").Replace("</Modules>", ""));

            string SUB = sXMLHumanDoc.ToString().Substring(0, sXMLHumanDoc.LastIndexOf("?>") + 2);

            sb.Append(sXMLHumanDoc.ToString().Replace(SUB, "").Replace("<notes>", "").Replace("<Modules>", ""));
            ds.ReadXml(new XmlTextReader(new StringReader(sb.ToString())));

            xmlDoc = new XmlDataDocument(ds);
            xslTran = new XslCompiledTransform();
            // xslTran.Load(xsltFile);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF XSLT Load : Start", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");
            xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF XSLT Load : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

            root = xmlDoc.DocumentElement;


            nav = root.CreateNavigator();
            if (File.Exists(outputDocument))
            {
                File.Delete(outputDocument);
            }
            writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            xslTran.Transform(nav, writer);
            writer.Close();
            writer = null;
            nav = null;
            root = null;
            xmlDoc = null;
            ds = null;
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);
            //  string htmlString = System.IO.File.ReadAllText(outputDocument);

            //  IList<PatientPane> lstpane = new List<PatientPane>();
            //   lstpane = ClientSession.PatientPaneList.ToList<PatientPane>();
            string Patient_Name = "";
            // if(lstpane.Count>0)
            // Patient_Name=lstpane[0].Last_Name + "," + "" + lstpane[0].First_Name + " " + lstpane[0].MI;

            string Encounter_signedDate = "";

            string Encounter_Provider_Name = "";
            string Provider_Speciality = "";
            TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
            XDocument xmlDocumentType = XDocument.Load(EncXMLContent);
            TextReader HumanXMLContentdoc = new StringReader(sXMLHumanDoc);
            XDocument xmlDocument = XDocument.Load(HumanXMLContentdoc);
            string Encounter_Provider_id = "";

            foreach (XElement elements in xmlDocument.Descendants("HumanList"))
            {

                foreach (XElement Human in elements.Elements())
                {
                    if (Human.Attribute("First_Name") != null && Human.Attribute("Last_Name") != null && Human.Attribute("MI") != null)
                        Patient_Name = Human.Attribute("Last_Name").Value + "," + "" + Human.Attribute("First_Name").Value + " " + Human.Attribute("MI").Value;

                }
            }





            string Encounter_Reviewed_signedDate = "";
            string Encounter_Reviewed_Name = "";
            string Encounter_Reviewed_Id = "";
            // xmlDocumentType = XDocument.Load(strXmlEncounterPath);

            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    DateTime dt = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value);
                    Encounter_Reviewed_signedDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt");

                    DateTime dtPro = Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value);
                    Encounter_signedDate = UtilityManager.ConvertToLocal(dtPro).ToString("dd-MMM-yyyy hh:mm tt");

                    Encounter_Reviewed_Id = Encounter.Attribute("Encounter_Provider_Review_ID").Value;

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


            foreach (XElement elements in xmlDocumentType.Descendants("EncounterDetails"))
            {
                foreach (XElement Encounter in elements.Elements())
                {
                    // if (Encounter.LastAttribute != null && Encounter.LastAttribute.Name.ToString() == "Specialties")
                    if (Encounter.Attribute("Specialties") != null)
                    {
                        Provider_Speciality = Encounter.LastAttribute.Value;
                        break;


                    }


                }

            }



            string strfooterProvider = "Electronically Signed by " + Encounter_Provider_Name + " at " + Encounter_signedDate;
            //string strfooterProviderReviewed = "I " + Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate +
            //     " have reviewed the chart and agree with the management plan with the changes to the plan as indicated.";

            string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
            StaticLookupManager staticMngr = new StaticLookupManager();
            string strfooterProviderReviewed = string.Empty;
            IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
            if (CommonList.Count > 0)
                strfooterProviderReviewed = CommonList[0].Value.Replace("<Physician>", Encounter_Reviewed_Name + " at " + Encounter_Reviewed_signedDate).Replace("|", "");


            string htmlString = System.IO.File.ReadAllText(outputDocument);
            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();
            string imgpath = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath"];
            string imgpath_phy = System.Configuration.ConfigurationSettings.AppSettings["phyConsultationLogoPath"];
            string imgpath_PalliativeCare = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_PalliativeCare"];
            string imgpath_Integrum = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_Integrum"];

            string phyheaderstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_phy + "' alt='logo' height='100' width='200'></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string header = "<table style='width:100%'><tr><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_PalliativeCare = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_PalliativeCare + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string headerLogo_Integrum = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_Integrum + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";
            string finalHeadeString = string.Empty;
            string PatientName = "RE: " + Patient_Name;

            string sProHeader = System.Configuration.ConfigurationSettings.AppSettings["ProgressNotesMainHeader"];
            if (System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] != null && System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoIntegrum"] == "ALL")
                finalHeadeString = imgpath_Integrum;
            else if (Provider_Speciality.ToUpper().Contains("ENDOCRINOLOGY"))
                finalHeadeString = imgpath;
            else if (Encounter_Provider_id == "4198")
                finalHeadeString = imgpath_phy;
            else if (Encounter_Provider_id == "3101" || Encounter_Provider_id == "321")
                finalHeadeString = imgpath_PalliativeCare;
            else
                finalHeadeString = "";


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


            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            // var strBody = new StringBuilder();

            string sbTop = "";

            sbTop = sbTop + htmlString;


            string strHtml = string.Empty;
            string pdfFileName = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + "test" + ".pdf");//System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + "test" + ".pdf";

            string pdfFileNamewithHeader = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], NotesName + ".pdf");//)System.Configuration.ConfigurationSettings.AppSettings["XMLPath"] + NotesName + ".pdf";

            if (htmlString.Length > 0)
            {
                try
                {
                    CreatePDFFromHTMLFile(sbTop, pdfFileName);
                }

                catch (Exception ex)
                {
                    if (ex.Message.ToUpper().Contains("NO PAGES"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('1011187'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;

                    }
                    else
                        throw ex;
                }

                var reader = new PdfReader(pdfFileName);

                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {


                    var document1 = new Document(new Rectangle(625, 975));

                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = reader.NumberOfPages;



                    sProHeader = "N";
                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var importedPage = writerpdf.GetImportedPage(reader, i);


                        var con = writerpdf.DirectContent;

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();

                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 9);

                        if (strfooterF == "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterPA != "" && strfooterP != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterPA, 30, 30, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterP, 30, 20, 0);

                            //ColumnText ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterPA, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 77, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go();

                            //ct = new ColumnText(con);
                            //ct.SetSimpleColumn(new Phrase(new Chunk(strfooterP, FontFactory.GetFont(FontFactory.TIMES_ITALIC, 9, Font.NORMAL))),
                            //                   35, 65, 530, 36, 14, iTextSharp.text.Element.ALIGN_LEFT | iTextSharp.text.Element.ALIGN_BOTTOM);
                            //ct.Go(); 

                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }

                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;

                        con.SetFontAndSize(baseFont1, 10);
                        con.SetColorFill(BaseColor.BLACK);
                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));

                        X += 103;

                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        // string imagepath = "";

                        if (finalHeadeString != "")
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(finalHeadeString);
                            logo.ScaleAbsolute(50, 50);
                            logo.SetAbsolutePosition(pageSize.GetLeft(X) - 150, pageSize.GetTop(Y) - 30);
                            con.AddImage(logo);


                        }


                        con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PatientName, pageSize.GetLeft(X) + 200, pageSize.GetTop(Y) - 30, 0);
                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();
                        if (sProHeader == "Y")
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

                }
            }
            else
            {
                using (var fileStream = new FileStream(pdfFileNamewithHeader, FileMode.Create, FileAccess.Write))
                {
                    var document1 = new Document(new Rectangle(625, 975));

                    Rectangle pageSize = new Rectangle(625, 975);
                    var writerpdf = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();
                    int count = 1;


                    sProHeader = "N";

                    for (var i = 1; i <= count; i++)
                    {
                        document1.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        PdfReader.unethicalreading = true;

                        var con = writerpdf.DirectContent;

                        float X = 80f, Y = 20f;
                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        con.BeginText();
                        if (sProHeader == "Y")
                        {


                            //float U = 75;
                            //float S = 60f;


                        }
                        con.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 8);

                        if (strfooterProvider != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProvider, 30, 30, 0);
                            if (strfooterProviderReviewed != "")
                                con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterProviderReviewed, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else if (strfooterF != "")
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, strfooterF, 30, 20, 0);
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }
                        else
                        {
                            con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, " page " + i, 550, 20, 0);
                        }


                        Y += 10;
                        con.MoveTo(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));


                        #region Column1
                        if (sProHeader == "Y")
                        {
                            Y += 70;
                        }

                        else
                            Y += 10;

                        con.SetFontAndSize(baseFont1, 10);
                        con.SetColorFill(BaseColor.BLACK);
                        con.SetTextMatrix(pageSize.GetLeft(X) - 40, pageSize.GetTop(Y));
                        if (finalHeadeString != "")
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(finalHeadeString);
                            logo.ScaleAbsolute(50, 50);
                            logo.SetAbsolutePosition(pageSize.GetLeft(X) - 150, pageSize.GetTop(Y) - 30);
                            con.AddImage(logo);


                        }


                        con.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PatientName, pageSize.GetLeft(X) + 200, pageSize.GetTop(Y) - 30, 0);



                        #endregion


                        con.LineTo(pageSize.GetRight(X) + 40, pageSize.GetTop(Y));
                        con.Stroke();


                        con.SetColorFill(BaseColor.BLACK);
                        con.EndText();

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
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Summary Consultation PDF : End", DateTime.Now, sGroup_ID_Log, "frmSummaryNew");

        }
        public static void CreatePDFFromHTMLFile(string HtmlStream, string FileName)
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

        public void UpdateDOSinBlob(ulong ulHumanID)
        {
            XmlDocument itemDoc = new XmlDocument();
            string sXMLContent = String.Empty;
            HumanBlobManager HumanBlobMngr = new HumanBlobManager();
            Human_Blob objHumanblob = null;
            IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(ulHumanID);
            if (ilstHumanBlob.Count > 0)
            {
                objHumanblob = ilstHumanBlob[0];
                sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                if (sXMLContent.Substring(0, 1) != "<")
                    sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                itemDoc.LoadXml(sXMLContent);
            }

            int flag = 0;
            string HumanID = "";

            XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("EncounterList");
            if (xmlNodeList.Count > 0)
            {
                for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                {
                    ////objStaticLookup = new StaticLookup();
                    if (xmlNodeList[0].ChildNodes[j].Attributes["Local_Time"].Value == "")
                    {
                        flag = 1;
                        xmlNodeList[0].ChildNodes[j].Attributes["Local_Time"].Value = UtilityManager.ConvertToLocal(Convert.ToDateTime(xmlNodeList[0].ChildNodes[j].Attributes["Date_of_Service"].Value)).ToString("yyyy-MM-dd hh:mm:ss tt");
                    }
                    HumanID = xmlNodeList[0].ChildNodes[j].Attributes["Human_ID"].Value;



                    //iFieldLookupList.Add(objStaticLookup);
                }
            }




            //XmlText.Close();
            // itemDoc.Save(strXmlHumanFilePath);
            //int trycount = 0;
            //trytosaveagain:
            try
            {
                if (flag == 1)
                {
                    //itemDoc.Save(strXmlHumanFilePath);
                    IList<Human_Blob> ilstUpdateBlob = new List<Human_Blob>();
                    byte[] bytes = null;
                    try
                    {
                        bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                    }
                    catch (Exception ex)
                    {

                    }
                    objHumanblob.Human_XML = bytes;
                    ilstUpdateBlob.Add(objHumanblob);
                    HumanBlobMngr.SaveHumanBlobWithTransaction(ilstUpdateBlob, string.Empty);
                }
            }
            catch (Exception xmlexcep)
            {
                throw new Exception(xmlexcep.Message.ToString());

                //trycount++;
                //if (trycount <= 3)
                //{
                //    int TimeMilliseconds = 0;
                //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                //    Thread.Sleep(TimeMilliseconds);
                //    string sMsg = string.Empty;
                //    string sExStackTrace = string.Empty;

                //    string version = "";
                //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                //    string[] server = version.Split('|');
                //    string serverno = "";
                //    if (server.Length > 1)
                //        serverno = server[1].Trim();

                //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                //        sMsg = xmlexcep.InnerException.Message;
                //    else
                //        sMsg = xmlexcep.Message;

                //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                //        sExStackTrace = xmlexcep.StackTrace;

                //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                //    string ConnectionData;
                //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                //    {
                //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                //        {
                //            cmd.Connection = con;
                //            try
                //            {
                //                con.Open();
                //                cmd.ExecuteNonQuery();
                //                con.Close();
                //            }
                //            catch
                //            {
                //            }
                //        }
                //    }
                //    goto trytosaveagain;
                //}
            }

            flag = 0;

            xmlNodeList = itemDoc.GetElementsByTagName("EncounterList");
            if (xmlNodeList.Count > 0)
            {
                for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                {
                    ////objStaticLookup = new StaticLookup();
                    if (xmlNodeList[0].ChildNodes[j].Attributes["Human_ID"].Value == HumanID)
                    {
                        if (xmlNodeList[0].ChildNodes[j].Attributes["Local_Time"].Value == "")
                        {
                            flag = 1;
                            xmlNodeList[0].ChildNodes[j].Attributes["Local_Time"].Value = UtilityManager.ConvertToLocal(Convert.ToDateTime(xmlNodeList[0].ChildNodes[j].Attributes["Date_of_Service"].Value)).ToString("yyyy-MM-dd hh:mm:ss tt");

                        }
                        break;

                    }


                    //iFieldLookupList.Add(objStaticLookup);
                }

                try
                {
                    if (flag == 1)
                    {
                        //itemDoc.Save(strXmlHumanFilePath);
                        IList<Human_Blob> ilstUpdateBlob = new List<Human_Blob>();
                        byte[] bytes = null;
                        try
                        {
                            bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                        }
                        catch (Exception ex)
                        {

                        }
                        objHumanblob.Human_XML = bytes;
                        ilstUpdateBlob.Add(objHumanblob);
                        HumanBlobMngr.SaveHumanBlobWithTransaction(ilstUpdateBlob, string.Empty);
                    }
                }
                catch (Exception xmlexcep)
                {
                    throw new Exception(xmlexcep.Message.ToString());

                    //trycount++;
                    //if (trycount <= 3)
                    //{
                    //    int TimeMilliseconds = 0;
                    //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                    //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                    //    Thread.Sleep(TimeMilliseconds);
                    //    string sMsg = string.Empty;
                    //    string sExStackTrace = string.Empty;

                    //    string version = "";
                    //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    //    string[] server = version.Split('|');
                    //    string serverno = "";
                    //    if (server.Length > 1)
                    //        serverno = server[1].Trim();

                    //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                    //        sMsg = xmlexcep.InnerException.Message;
                    //    else
                    //        sMsg = xmlexcep.Message;

                    //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                    //        sExStackTrace = xmlexcep.StackTrace;

                    //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //    string ConnectionData;
                    //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                    //    {
                    //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                    //        {
                    //            cmd.Connection = con;
                    //            try
                    //            {
                    //                con.Open();
                    //                cmd.ExecuteNonQuery();
                    //                con.Close();
                    //            }
                    //            catch
                    //            {
                    //            }
                    //        }
                    //    }
                    //    goto trytosaveagain;
                    //}
                }

            }
        }


    }
}
