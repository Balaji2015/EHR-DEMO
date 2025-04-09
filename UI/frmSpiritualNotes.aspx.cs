using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Text;

namespace Acurus.Capella.UI
{
    public partial class frmSpiritualNotes : System.Web.UI.Page
    {
        string strXmlFilePath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml"; 
         //  string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
          //  string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);

          //  string xmlDataFile = strXmlFilePath;
            string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Spiritual_Notes.xsl");
            string WordOutputName = ClientSession.FacilityName.Replace(",", "") + "_Spiritual_Notes_" + ClientSession.HumanId + "_" + DateTime.Now.ToString("yyyyMMdd hhmmsstt") +".html";
            string outputDocument = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], WordOutputName);
            //Jira #CAP-344 - OldCode
            //DataSet ds;
            //XmlDataDocument xmlDoc;
            //XslCompiledTransform xslTran;
            //XmlElement root;
            //XPathNavigator nav;
            //XmlTextWriter writer;
            //XsltSettings settings = new XsltSettings(true, false);

            //ds = new DataSet();
            IList<Encounter_Blob> ilstEncounterBlob = new List<Encounter_Blob>();
            EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
            string sXMLEncounterDoc = "";
            ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ClientSession.EncounterId);
            if (ilstEncounterBlob.Count > 0)
            {
                sXMLEncounterDoc = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                if (sXMLEncounterDoc.Substring(0, 1) != "<")
                    sXMLEncounterDoc = sXMLEncounterDoc.Substring(1, sXMLEncounterDoc.Length - 1);
            }
            //Jira #CAP-344 - OldCode
            //TextReader EncXMLContent = new StringReader(sXMLEncounterDoc);
            ////XDocument xmlDocumentType = XDocument.Load(EncXMLContent);
            //ds.ReadXml(EncXMLContent);

            //xmlDoc = new XmlDataDocument(ds);
            //xslTran = new XslCompiledTransform();
            //xslTran.Load(xsltFile);
            //xslTran.Load(xsltFile, settings, new XmlUrlResolver());
            //root = xmlDoc.DocumentElement;
            //nav = root.CreateNavigator();
            //if (File.Exists(outputDocument))
            //{
            //    File.Delete(outputDocument);
            //}
            //writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
            //xslTran.Transform(nav, writer);
            //writer.Close();
            //writer = null;
            //nav = null;
            //root = null;
            //xmlDoc = null;
            //ds = null;

            //Jira #CAP-344 - NewCode
            string htmlString = string.Empty;
            htmlString = UtilityManager.PrintPDFUsingXSLT(sXMLEncounterDoc, String.Empty, xsltFile, outputDocument, string.Empty);
            System.IO.FileInfo file = new System.IO.FileInfo(outputDocument);
            //string htmlString = System.IO.File.ReadAllText(outputDocument);
            if (System.Configuration.ConfigurationSettings.AppSettings["XsltTransformVersion"] != null
                    && System.Configuration.ConfigurationSettings.AppSettings["XsltTransformVersion"].ToString().ToUpper() == "V1")
            {
                htmlString = System.IO.File.ReadAllText(outputDocument);
            }
                string Patient_Name = "";
            string Encounter_signedDate = "";
            string Encounter_Provider_Name = "";
            string Provider_Speciality = "";
            XDocument xmlDocumentType = XDocument.Load(strXmlFilePath);
            GenerateXml objxml = new GenerateXml();

            XDocument xmlDocument =XDocument.Load(objxml.ReadBlob("Human", ClientSession.HumanId).InnerXml);
            string Encounter_Provider_id = "";

            foreach (XElement elements in xmlDocument.Descendants("HumanList"))
            {

                foreach (XElement Human in elements.Elements())
                {
                    if (Human.Attribute("First_Name") != null && Human.Attribute("Last_Name") != null && Human.Attribute("MI") != null)
                        Patient_Name = Human.Attribute("Last_Name").Value + "," + "" + Human.Attribute("First_Name").Value + " " + Human.Attribute("MI").Value;

                }
            }

            foreach (XElement elements in xmlDocumentType.Descendants("EncounterList"))
            {
                foreach (XElement Encounter in elements.Elements())
                {

                    Encounter_signedDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Review_Signed_Date").Value)).ToString("dd-MMM-yyyy hh:mm:ss tt");
                    Encounter_Provider_id = Encounter.Attribute("Encounter_Provider_ID").Value;
                }
                if (Encounter_signedDate == "" || Encounter_signedDate == "01-Jan-0001 12:00:00 AM")
                {
                    foreach (XElement Encounter in elements.Elements())
                    {
                        Encounter_signedDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(Encounter.Attribute("Encounter_Provider_Signed_Date").Value)).ToString("dd-MMM-yyyy hh:mm:ss tt");

                    }
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
            string strfooter = "Electronically Signed by ";
            if (file.Exists)
            {
                File.Delete(outputDocument);
            }
            var strBody = new StringBuilder();
            //string strDocBody = ""; ;
            string imgpath = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath"];
            string imgpath_phy = System.Configuration.ConfigurationSettings.AppSettings["phyConsultationLogoPath"];
            string imgpath_PalliativeCare = System.Configuration.ConfigurationSettings.AppSettings["ConsultationLogoPath_PalliativeCare"];
            string phyheaderstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_phy + "' alt='logo' height='100' width='200'></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";

            string headerstring = "<table  style='width:100%'><tr><td><span><img src='" + imgpath + "' alt='logo' ></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";

            string header = "<table style='width:100%'><tr><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";

            string headerLogo_PalliativeCare = "<table  style='width:100%'><tr><td><span><img src='" + imgpath_PalliativeCare + "' alt='logo' /></span></td><td style=' text-align:right;font-size: 9pt; font-family:Arial;'><span>" + "RE: " + Patient_Name + "</span></td></tr></table> ";


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
            sbTop.Append(@"<table id='hrdftrtbl' border='1' cellspacing='0' cellpadding='0'>
<tr>
<td>
<div style='mso-element:header' id=h1 >");
            if (Provider_Speciality.ToUpper().Contains("ENDOCRINOLOGY"))
                sbTop.Append(headerstring);

            else if (Encounter_Provider_id == "4198")
                sbTop.Append(phyheaderstring);
            else if (Encounter_Provider_id == "3101" || Encounter_Provider_id == "321")
                sbTop.Append(headerLogo_PalliativeCare);
            else
                sbTop.Append(header);

            sbTop.Append(htmlString);
           
            sbTop.Append(@"
</div>
</td>
<td>");
            if (Encounter_signedDate != "" && Encounter_signedDate != "01-Jan-0001 12:00:00 AM")
            {
                sbTop.Append(@"<div style='mso-element:footer' id=f1><p class=MsoFooter style='font-size: 9pt; font-family:Arial;'>" + strfooter + " " + Encounter_Provider_Name + " On " + Encounter_signedDate +
@"<span style=padding-right:10px;mso-tab-count:2'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span></p></div>
</td>
</tr>
</table>
</body></html>
");
            }
            else
            {

                sbTop.Append(@"<div style='mso-element:footer' id=f1><p class=MsoFooter><span style=padding-right:10px;mso-tab-count:2'>Page&#32;</span><span style='mso-field-code:"" PAGE "";font-family:Arial;font-size:9pt'>
</span>&nbsp;of&nbsp;<span style='mso-field-code:"" NUMPAGES "";font-family:Arial;font-size:9pt'></span>
</p></div>
</td></tr>
</table>
</body></html>
");




                sbTop.Append(@"</td></tr>
</table>
</body></html>");
            }
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + ClientSession.FacilityName.Replace(",", "") + "_Spiritual_Notes_" + ClientSession.HumanId + "_" + DateTime.Now.ToString("yyyyMMdd hhmmsstt") + ".doc");
            Response.ContentType = "application/msword";
            Response.Write(sbTop);
            Response.Flush();
            Response.End();
        }
    }
}