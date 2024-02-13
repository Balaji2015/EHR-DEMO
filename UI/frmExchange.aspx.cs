using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using Acurus.Capella.Core;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Net;
using System.Xml;
using System.Text;
using Ionic.Zip;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Web.Services;

namespace Acurus.Capella.UI
{
    public partial class frmExchange : System.Web.UI.Page
    {
        static ArrayList aryPrint;
        FileStream fs = null;
        static IList<string> Filelst = new List<string>();
        static IDictionary<string, string> ImmLookupList = new Dictionary<string, string>();
        static IDictionary<string, string> ImmLookupErrList = new Dictionary<string, string>();
        static string PatientList = string.Empty;
        static string PatientProvince = string.Empty;
        static IDictionary<string, string> ImmLogsData = new Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rbtnAck.Checked = true;
                if (Request.QueryString["TabName"] != null && (Request.QueryString["TabName"] == "ImmunizationRegistry" || Request.QueryString["TabName"] == "ImmunizationRegistryQuery"))
                {
                    InsertIntoImmSubmissionLog(string.Empty, "Viewed", string.Empty, string.Empty, Request.QueryString["TabName"]);

                    rbtnAck.Visible = false;
                    rbtnWithoutAck.Visible = false;
                }
                else
                {
                    rbtnAck.Visible = true;
                    rbtnWithoutAck.Visible = true;
                }

                if (Request.QueryString["TabName"] != null && Request.QueryString["TabName"] == "ImmunizationRegistry")
                {
                    string sMyPath = string.Empty;
                    frmImmunizationRegistry objImmunReg = new frmImmunizationRegistry();
                    aryPrint = new ArrayList();
                    aryPrint = objImmunReg.GenerateImmunizationRegistry(ClientSession.Selectedencounterid, ClientSession.HumanId, false, ref sMyPath, string.Empty);
                    EditorSummary.Enabled = true;
                    if (aryPrint != null && aryPrint.Count > 0)
                    {
                        EditorSummary.Content = aryPrint[0].ToString();
                    }
                    EditorSummary.EditModes = EditModes.Design;
                    this.Title = "Immunization Registry";
                }
                else if (Request.QueryString["TabName"] != null && Request.QueryString["TabName"] == "ImmunizationRegistryQuery")
                {
                    string sMyPath = string.Empty;
                    ContainerDiv.Style.Add("width", "1090px!important");
                    queryDiv.Style.Add("width", "525px!important");
                    tblSummary.Style.Add("width", "500px!important");
                    responseDiv.Style.Add("display", "inline-block!important");
                    frmImmunizationRegistry objImmunReg = new frmImmunizationRegistry();
                    ulong uHumanID = 0;
                    if (Request.QueryString["HumanID"] != null && Request.QueryString["HumanID"] != "")
                    {
                        uHumanID = Convert.ToUInt64(Request.QueryString["HumanID"].ToString());
                    }
                    aryPrint = new ArrayList();
                    aryPrint = objImmunReg.GenerateImmunizationRegistryQuery(uHumanID, false, ref sMyPath, string.Empty);
                    EditorSummary.Enabled = true;
                    if (aryPrint != null && aryPrint.Count > 0)
                    {
                        EditorSummary.Content = aryPrint[0].ToString();
                    }
                    EditorSummary.EditModes = EditModes.Design;
                    this.Title = "Immunization Registry Query";
                }
                else if (Request.QueryString["TabName"] != null && Request.QueryString["TabName"] == "ImmunizationRegistryQueryHistory")
                {
                    string sMyPath = string.Empty;
                    ContainerDiv.Style.Add("width", "1090px!important");
                    queryDiv.Style.Add("display", "none!important");
                    DatesList.Style.Add("width", "525px!important");
                    tblSummary.Style.Add("width", "500px!important");
                    responseDiv.Style.Add("display", "inline-block!important");
                    DatesList.Style.Add("display", "inline-block!important");
                    ulong uHumanID = 0;
                    if (Request.QueryString["HumanID"] != null && Request.QueryString["HumanID"] != "")
                    {
                        uHumanID = Convert.ToUInt64(Request.QueryString["HumanID"].ToString());
                    }
                    aryPrint = new ArrayList();

                    EditorSummary.Enabled = true;
                    if (aryPrint != null && aryPrint.Count > 0)
                    {
                        EditorSummary.Content = aryPrint[0].ToString();
                    }
                    EditorSummary.EditModes = EditModes.Design;
                    this.Title = "Immunization Registry Query History";
                    GetImmunizationHistoryDataFromLog(uHumanID);
                    LoadImmunizationSubmissionHistoryGrid();
                }
                else if (Request.QueryString["TabName"] != null && Request.QueryString["TabName"].Contains("SyndromicSurveillance"))
                {
                    hdnXmlPath.Value = Request.QueryString["TabName"].ToString().Split('|')[1];
                    string sMyPath = string.Empty;
                    string sFileName = string.Empty;
                    if (rbtnAck.Checked == true)
                    {
                        sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnAck.Text.ToUpper();
                    }
                    else
                    {
                        sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnWithoutAck.Text.ToUpper();
                    }
                    frmImmunizationRegistry objImmunReg = new frmImmunizationRegistry();
                    aryPrint = new ArrayList();
                    aryPrint = objImmunReg.GenerateHL7ForSyndromicSurveillance(ClientSession.Selectedencounterid, ClientSession.HumanId, ClientSession.PhysicianId, sFileName);
                    CreateAuditentry(sFileName);
                    EditorSummary.Enabled = true;
                    if (aryPrint != null && aryPrint.Count > 0)
                    {
                        if (aryPrint[0].ToString().Contains("Error"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Syndromic Survilence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); var oWindow = null;if (window.radWindow) oWindow = window.radWindow;else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;var WindowName = $find('ctl00_ModalWindow');oWindow.Close();}DisplayErrorMessage('9004');", true);
                            return;
                        }
                        EditorSummary.Content = aryPrint[0].ToString();
                    }
                    EditorSummary.EditModes = EditModes.Design;
                    this.Title = "Syndromic Surveillance ";

                }
                if (ImmLookupList != null && ImmLookupList.Count == 0)
                {
                    string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\ImmztnRegistry_Lookup.xml");
                    if (File.Exists(strXmlFilePath) == true)
                    {
                        try
                        {
                            XmlDocument itemDoc = new XmlDocument();
                            XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                            itemDoc.Load(XmlText);
                            XmlText.Close();
                            XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("ImmunizationRegistryRespList");
                            if (xmlNodeList != null && xmlNodeList.Count > 0 && xmlNodeList[0].ChildNodes.Count > 0)
                            {
                                for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                                {
                                    //CAP-1716,CAP-1719 - Immunization Submission to CAIR Records - To include PI(human_id ) for all  submissions instead of MRN Number 
                                    //ImmLookupList.Add(xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value, xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("value").Value);
                                    ImmLookupList.Add(xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("key").Value, xmlNodeList[0].ChildNodes[j].Attributes.GetNamedItem("value").Value);
                                }
                            }
                            XmlNodeList xmlErrNodeList = itemDoc.GetElementsByTagName("ImmunizationMsgLookupList");
                            if (xmlErrNodeList != null && xmlErrNodeList.Count > 0 && xmlErrNodeList[0].ChildNodes.Count > 0)
                            {
                                for (int j = 0; j < xmlErrNodeList[0].ChildNodes.Count; j++)
                                {
                                    ImmLookupErrList.Add(xmlErrNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value, xmlErrNodeList[0].ChildNodes[j].Attributes.GetNamedItem("value").Value);
                                }
                            }
                            XmlNodeList xmlPatNodeList = itemDoc.GetElementsByTagName("ImmunizationErrPatList");
                            if (xmlPatNodeList != null && xmlPatNodeList.Count > 0 && xmlPatNodeList[0].ChildNodes.Count > 0)
                            {
                                for (int j = 0; j < xmlPatNodeList[0].ChildNodes.Count; j++)
                                {
                                    if (xmlPatNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value == "patientList")
                                        PatientList = xmlPatNodeList[0].ChildNodes[j].Attributes.GetNamedItem("value").Value;
                                    if (xmlPatNodeList[0].ChildNodes[j].Attributes.GetNamedItem("Field_Name").Value == "patientProvince")
                                        PatientProvince = xmlPatNodeList[0].ChildNodes[j].Attributes.GetNamedItem("value").Value;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            else
            {
                if (Request.QueryString["TabName"] != null && Request.QueryString["TabName"].Contains("SyndromicSurveillance"))
                {
                    string sMyPath = string.Empty;
                    string sFileName = string.Empty;
                    if (rbtnAck.Checked == true)
                    {
                        sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnAck.Text.ToUpper();
                    }
                    else
                    {
                        sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnWithoutAck.Text.ToUpper();
                    }
                    frmImmunizationRegistry objImmunReg = new frmImmunizationRegistry();
                    aryPrint = new ArrayList();
                    aryPrint = objImmunReg.GenerateHL7ForSyndromicSurveillance(ClientSession.Selectedencounterid, ClientSession.HumanId, ClientSession.PhysicianId, sFileName);
                    CreateAuditentry(sFileName);
                    EditorSummary.Enabled = true;
                    if (aryPrint != null && aryPrint.Count > 0)
                    {
                        if (aryPrint[0].ToString().Contains("Error"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Syndromic Survilence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); var oWindow = null;if (window.radWindow) oWindow = window.radWindow;else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;var WindowName = $find('ctl00_ModalWindow');oWindow.Close();}DisplayErrorMessage('9004');", true);
                            return;
                        }
                        EditorSummary.Content = aryPrint[0].ToString();
                    }
                    EditorSummary.EditModes = EditModes.Design;
                    this.Title = "Syndromic Surveillance ";

                }
            }





        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //1.ConnectivityTest
            try
            {
                Filelst.Clear();
                bool IsConnected = ConnectivityTest();
                //2.SubmitSingleMessage
                if (IsConnected)
                {
                    string UserName = System.Configuration.ConfigurationManager.AppSettings["NIST_UserName"].ToString();
                    string Password = System.Configuration.ConfigurationManager.AppSettings["NIST_Password"].ToString();
                    string FacilityID = System.Configuration.ConfigurationManager.AppSettings["NIST_FacilityID"].ToString();
                    string sResponse = string.Empty;
                    if (aryPrint != null && aryPrint.Count > 1 && aryPrint[0].ToString() != string.Empty)
                    {
                        aryPrint[0] = EditorSummary.Content;
                        ImmSubmission_NIST.client_Service objImmSubmission = new ImmSubmission_NIST.client_Service();
                        sResponse = objImmSubmission.submitSingleMessage(UserName, Password, FacilityID, aryPrint[0].ToString());
                        string[] querySplit = aryPrint[0].ToString().Split('|');
                        string[] resultsplit = sResponse.Split('|');
                        string Result_Type = "Fail";
                        string Result_Msg = string.Empty;
                        string Response_Msg = string.Empty;
                        string ControlID = string.Empty;
                        IList<string> PatientsList = new List<string>();
                        PatientsList = PatientList.Split('|').ToList<string>();
                        foreach (string sPat in PatientsList)
                        {
                            if (aryPrint[0].ToString().IndexOf(sPat) > -1 && aryPrint[0].ToString().IndexOf(PatientProvince) > -1)
                            {

                                string sErrResponse = string.Empty;
                                if (ImmLookupErrList.ContainsKey(sPat))
                                    sErrResponse = ImmLookupErrList[sPat];
                                if (sErrResponse.Trim() != string.Empty && sErrResponse.Substring(sErrResponse.LastIndexOf('|') + 1) != string.Empty)
                                {
                                    if (sErrResponse.IndexOf("999^Application error") > -1)
                                        sResponse = sResponse.Replace("|AA|", "|AE|");
                                    else
                                        sResponse = sResponse.Replace("|AA|", "|AR|");
                                    sResponse += sErrResponse;
                                    lbldetailedErrMsg.InnerText = sErrResponse.Substring(sErrResponse.LastIndexOf('|') + 1);
                                    tdWarning.Style.Add("display", "inline-block");
                                }
                            }
                        }
                        Result_Msg = sResponse;
                        if (sResponse.Contains("MSA|AA|"))
                        {
                            Result_Type = "Success";
                        }
                        if (resultsplit.Length > 10)
                            ControlID = resultsplit[9].ToString();
                        string KEY = querySplit[9].Replace("&nbsp;", "").Replace(" ", "").ToString();
                        if (ImmLookupList.ContainsKey(KEY))
                            Response_Msg = ImmLookupList[KEY];
                        string dateValue = UtilityManager.ConvertToUniversal().ToString("yyyyMMddhhmmss") + "-0500";
                        Response_Msg = Response_Msg.Replace("datetimevalue", dateValue);
                        string[] resultVal = Result_Msg.Split(new string[] { "|||NE" }, StringSplitOptions.None);
                        Result_Msg = resultVal[0] + "|||NE|NE|||||Z23^CDCPHINVS|NISTIISFAC|CapellaEHRFAC" + resultVal[1];
                        InsertIntoImmSubmissionLog(ControlID, Result_Type, Result_Msg, Response_Msg, "Immunization");
                        CreateTextFile("Query_AckMsg", Result_Msg);
                        CreateTextFile("Query_Response", Response_Msg);
                        Create_DownloadZipFile(Filelst);
                        string Response_Display = string.Empty;
                        if (Response_Msg.ToString().Trim() != "")
                            Response_Display = DisplayResponse(Response_Msg);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Immunization Registry Query", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();DisplayResponseInfo('" + Response_Display + "');}", true);
                    }

                }

            }
            catch (Exception ex)
            {
                Filelst.Clear();
                throw new Exception(ex.Message);
            }


            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Syndromic Survilence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); var oWindow = null;if (window.radWindow) oWindow = window.radWindow;else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;var WindowName = $find('ctl00_ModalWindow');oWindow.Close();}", true);
        }
        protected void hiddenButton_Click(object sender, EventArgs e)
        {
            if (fs != null)
            {
                HttpContext context = HttpContext.Current;
                context.Response.Clear();

                context.Response.ClearHeaders();

                context.Response.ClearContent();

                // fs = new FileStream(hdnFilePath.Value.ToString(), FileMode.Open, FileAccess.Read, FileShare.Read);

                // context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" +hdnFilePath.Value.ToString()+  "\"");

                context.Response.AddHeader("Content-Length", fs.Length.ToString());

                context.Response.ContentType = "application/text";
                fs.Close();
                fs.Dispose();
                context.Response.Flush();

                // context.Response.TransmitFile(hdnFilePath.Value.ToString().ToString());

                context.Response.End();
                context.Response.Close();
                // ClientScript.RegisterStartupScript(this.GetType(), "somekey",  "alert('Some data missing!');", true);


            }

        }

        protected void rbtnWithAck_CheckedChanged(object sender, EventArgs e)
        {
            //string sMyPath = string.Empty;
            //string sFileName = string.Empty;
            //if (rbtnWithAck.Checked == true)
            //{
            //    sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnWithAck.Text.ToUpper();
            //}
            //else
            //{
            //    sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnWithoutAck.Text.ToUpper();
            //}
            ////sFileName = Request.QueryString["TabName"].ToString().Split('|')[1];
            //frmImmunizationRegistry objImmunReg = new frmImmunizationRegistry();
            //aryPrint = objImmunReg.GenerateHL7ForSyndromicSurveillance(ClientSession.Selectedencounterid, ClientSession.HumanId, ClientSession.PhysicianId, sFileName);
            //EditorSummary.Content = string.Empty;
            //EditorSummary.Enabled = true;
            //if (aryPrint != null && aryPrint.Count > 0)
            //{
            //    if (aryPrint[0].ToString().Contains("Error"))
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Syndromic Survilence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); var oWindow = null;if (window.radWindow) oWindow = window.radWindow;else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;var WindowName = $find('ctl00_ModalWindow');oWindow.Close();}DisplayErrorMessage('9004');", true);
            //        return;
            //    }
            //    EditorSummary.Content = aryPrint[0].ToString();
            //}
            //EditorSummary.EditModes = EditModes.Design;
            //this.Title = "Syndromic Surveillance ";

            //if (aryPrint != null && aryPrint.Count > 0)
            //{
            //    Response.Clear();

            //    Response.ClearHeaders();

            //    Response.ClearContent();
            //    FileStream fs = null;
            //    fs = new FileStream(aryPrint[1].ToString(), FileMode.Open, FileAccess.Read, FileShare.Read);

            //    //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            //    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + aryPrint[1].ToString() + "\"");

            //    Response.AddHeader("Content-Length", fs.Length.ToString());

            //    Response.ContentType = "text/plain";
            //    fs.Close();
            //    fs.Dispose();
            //    Response.Flush();

            //    Response.TransmitFile(aryPrint[1].ToString());

            //    Response.End();
            //}
        }

        protected void rbtnWithoutAck_CheckedChanged(object sender, EventArgs e)
        {
            string sMyPath = string.Empty;
            string sFileName = string.Empty;
            if (rbtnAck.Checked == true)
            {
                sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnAck.Text.ToUpper();
            }
            else
            {
                sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnWithoutAck.Text.ToUpper();
            }
            //sFileName = Request.QueryString["TabName"].ToString().Split('|')[1];
            frmImmunizationRegistry objImmunReg = new frmImmunizationRegistry();
            aryPrint = new ArrayList();
            aryPrint = objImmunReg.GenerateHL7ForSyndromicSurveillance(ClientSession.Selectedencounterid, ClientSession.HumanId, ClientSession.PhysicianId, sFileName);
            CreateAuditentry(sFileName);
            EditorSummary.Content = string.Empty;
            EditorSummary.Enabled = true;
            if (aryPrint != null && aryPrint.Count > 0)
            {
                if (aryPrint[0].ToString().Contains("Error"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Syndromic Survilence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); var oWindow = null;if (window.radWindow) oWindow = window.radWindow;else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;var WindowName = $find('ctl00_ModalWindow');oWindow.Close();}DisplayErrorMessage('9004');", true);
                    return;
                }
                EditorSummary.Content = aryPrint[0].ToString();
            }
            EditorSummary.EditModes = EditModes.Design;
            this.Title = "Syndromic Surveillance ";

            //if (aryPrint != null && aryPrint.Count > 0)
            //{
            //    Response.Clear();

            //    Response.ClearHeaders();

            //    Response.ClearContent();
            //    FileStream fs = null;
            //    fs = new FileStream(aryPrint[1].ToString(), FileMode.Open, FileAccess.Read, FileShare.Read);

            //    //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            //    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + aryPrint[1].ToString() + "\"");

            //    Response.AddHeader("Content-Length", fs.Length.ToString());

            //    Response.ContentType = "text/plain";
            //    fs.Close();
            //    fs.Dispose();
            //    Response.Flush();

            //    Response.TransmitFile(aryPrint[1].ToString());

            //    Response.End();
            //}
        }

        protected void rbtnAck_CheckedChanged(object sender, EventArgs e)
        {
            string sMyPath = string.Empty;
            string sFileName = string.Empty;
            if (rbtnAck.Checked == true)
            {
                sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnAck.Text.ToUpper();
            }
            else
            {
                sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnWithoutAck.Text.ToUpper();
            }
            //sFileName = Request.QueryString["TabName"].ToString().Split('|')[1];
            frmImmunizationRegistry objImmunReg = new frmImmunizationRegistry();
            aryPrint = new ArrayList();
            aryPrint = objImmunReg.GenerateHL7ForSyndromicSurveillance(ClientSession.Selectedencounterid, ClientSession.HumanId, ClientSession.PhysicianId, sFileName);
            CreateAuditentry(sFileName);
            EditorSummary.Content = string.Empty;
            EditorSummary.Enabled = true;
            if (aryPrint != null && aryPrint.Count > 0)
            {
                if (aryPrint[0].ToString().Contains("Error"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Syndromic Survilence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); var oWindow = null;if (window.radWindow) oWindow = window.radWindow;else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;var WindowName = $find('ctl00_ModalWindow');oWindow.Close();}DisplayErrorMessage('9004');", true);
                    return;
                }
                EditorSummary.Content = aryPrint[0].ToString();
            }
            EditorSummary.EditModes = EditModes.Design;
            this.Title = "Syndromic Surveillance ";

            //if (aryPrint != null && aryPrint.Count > 0)
            //{
            //    Response.Clear();

            //    Response.ClearHeaders();

            //    Response.ClearContent();
            //    FileStream fs = null;
            //    fs = new FileStream(aryPrint[1].ToString(), FileMode.Open, FileAccess.Read, FileShare.Read);

            //    //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            //    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + aryPrint[1].ToString() + "\"");

            //    Response.AddHeader("Content-Length", fs.Length.ToString());

            //    Response.ContentType = "text/plain";
            //    fs.Close();
            //    fs.Dispose();
            //    Response.Flush();

            //    Response.TransmitFile(aryPrint[1].ToString());

            //    Response.End();
            //}
        }

        protected void hdnDownload_Click(object sender, EventArgs e)
        {
            string sMyPath = string.Empty;
            string sFileName = string.Empty;
            if (rbtnAck.Checked == true)
            {
                sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnAck.Text.ToUpper();
            }
            else
            {
                sFileName = Request.QueryString["TabName"].ToString().Split('|')[1] + " " + rbtnWithoutAck.Text.ToUpper();
            }
            //sFileName = Request.QueryString["TabName"].ToString().Split('|')[1];
            frmImmunizationRegistry objImmunReg = new frmImmunizationRegistry();
            aryPrint = new ArrayList();
            aryPrint = objImmunReg.GenerateHL7ForSyndromicSurveillance(ClientSession.Selectedencounterid, ClientSession.HumanId, ClientSession.PhysicianId, sFileName);

            EditorSummary.Content = string.Empty;
            EditorSummary.Enabled = true;
            if (aryPrint != null && aryPrint.Count > 0)
            {
                if (aryPrint[0].ToString().Contains("Error"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Syndromic Survilence", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); var oWindow = null;if (window.radWindow) oWindow = window.radWindow;else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;var WindowName = $find('ctl00_ModalWindow');oWindow.Close();}DisplayErrorMessage('9004');", true);
                    return;
                }
                EditorSummary.Content = aryPrint[0].ToString();
            }
            EditorSummary.EditModes = EditModes.Design;
            this.Title = "Syndromic Surveillance ";
            DownloadFile(aryPrint);
        }
        public void DownloadFile(ArrayList aryPrint)
        {


            if (aryPrint != null && aryPrint.Count > 0)
            {
                Response.Clear();

                Response.ClearHeaders();

                Response.ClearContent();
                FileStream fs = null;
                fs = new FileStream(aryPrint[1].ToString(), FileMode.Open, FileAccess.Read, FileShare.Read);

                //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + aryPrint[1].ToString() + "\"");

                Response.AddHeader("Content-Length", fs.Length.ToString());

                Response.ContentType = "text/plain";
                fs.Close();
                fs.Dispose();
                Response.Flush();

                Response.TransmitFile(aryPrint[1].ToString());

                Response.End();
            }
        }
        //public void DownloadZipFile(string zipPath)
        //{
        //    if (zipPath != null && zipPath.Trim().ToString()!=string.Empty)
        //    {
        //        Response.Clear();

        //        Response.ClearHeaders();

        //        Response.ClearContent();

        //        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + zipPath + "\"");

        //        Response.ContentType = "application/x-zip-compressed";

        //        Response.Flush();

        //        Response.TransmitFile(Server.MapPath("atala-capture-download//" + Session.SessionID+"//"+zipPath));

        //        Response.End();
        //    }
        //}
        private void CreateAuditentry(string TransactType)//BugID:50433
        {
            AuditLogManager alManager = new AuditLogManager();
            string TransactionType = "GENERATE - " + TransactType;
            alManager.InsertIntoAuditLog("Syndromic Surveillance", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685
        }

        private bool ConnectivityTest()
        {
            bool Is_Connected = false;

            HttpWebRequest request = CreateWebRequest();
            XmlDocument soapEnvelopeXml = new XmlDocument();
            String str_SenderEnvelope = "<Envelope xmlns='http://www.w3.org/2003/05/soap-envelope'> <Body><connectivityTest xmlns='urn:cdc:iisb:2011' xmlns:ns2='http://www.w3.org/2003/05/soap-envelope'><echoBack>Hello World!</echoBack></connectivityTest></Body></Envelope>";
            soapEnvelopeXml.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>" + str_SenderEnvelope + "");

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                    Is_Connected = true;
                    string FileName = "ConnectivityTest_SenderEnvelope";
                    CreateTextFile(FileName, str_SenderEnvelope);
                    string FileName_Response = "ConnectivityTest_ResponseEnvelope";

                    CreateTextFile(FileName_Response, soapResult);
                    //Console.WriteLine(soapResult);
                }
            }
            return Is_Connected;
        }
        public HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://hl7v2-iz-r1.5-testing.nist.gov:8098/iztool/ws/iisService");
            webRequest.ContentType = "application/soap+xml;charset=\"utf-8\"";
            webRequest.Method = "POST";
            return webRequest;
        }

        public void CreateTextFile(string FileName, string soapResult)
        {
            string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
            string sFolderPathName = TargetFileDirectory + "\\Immunization_Registry";
            if (!Directory.Exists(sFolderPathName))
            {
                Directory.CreateDirectory(sFolderPathName);
            }
            FileName = sFolderPathName + "\\" + FileName + ".txt";
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }

            // Create a new file 
            using (StreamWriter sw = File.CreateText(FileName))
            {
                sw.WriteLine(soapResult);
            }
            FileName = FileName.Substring(FileName.IndexOf("Documents"));
            Filelst.Add(FileName);
        }

        //public void AppendFiles(string soapResult)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string output = "Response created at" + DateTime.Now;
        //    sb.Append(soapResult);
        //    sb.Append("-----------------------------------------------------------------------");
        //    sb.Append("\r\n");

        //    string text = sb.ToString();

        //    Response.Clear();
        //    Response.ClearHeaders();

        //     Response.ContentType = "application/x-zip-compressed";

        //        Response.AppendHeader("Content-Disposition", "attachment; filename=Immunization");
        //        Response.TransmitFile(Server.MapPath(PDFPath));
        //        Response.End();
        //}

        private void InsertIntoImmSubmissionLog(string ControlID, string Result_Type, string Result_Msg, string Response_Msg, string sRegistry_Type)
        {
            IList<RegistryLog> ImmSubLoglst = new List<RegistryLog>();
            IList<RegistryLog> ImmSubLogUpdtlst = new List<RegistryLog>();
            RegistryLog ImmSubLog = new RegistryLog();
            ImmSubLog.Human_ID = ClientSession.HumanId;
            ImmSubLog.Encounter_ID = ClientSession.EncounterId;
            ImmSubLog.Physician_ID = ClientSession.PhysicianId;
            ImmSubLog.Created_By = ClientSession.UserName;
            ImmSubLog.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
            ImmSubLog.Submission_Result_Type = Result_Type;
            ImmSubLog.Result_Message = Result_Msg;
            ImmSubLog.Response_Message = Response_Msg;
            ImmSubLog.Control_ID = ControlID;
            ImmSubLog.Registry_Type = sRegistry_Type;
            if (Result_Type == "Success")
                ImmSubLog.Status = "Submitted";
            else if (Result_Type == "Viewed")
                ImmSubLog.Status = "Viewed";
            else
                ImmSubLog.Status = "Not Submitted";
            ImmSubLog.FileName = string.Empty;
            ImmSubLoglst.Add(ImmSubLog);
            RegistryLogManager ImmSubLogManager = new RegistryLogManager();
            ImmSubLogManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref ImmSubLoglst, ref ImmSubLogUpdtlst, null, string.Empty, false, false, ClientSession.HumanId, String.Empty);
        }

        protected void Create_DownloadZipFile(IList<string> filesList)
        {
            string zipName = string.Empty;
            if (filesList != null && filesList.Count > 0)
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;

                    DirectoryInfo directorySelected = new DirectoryInfo(Server.MapPath(filesList[0]));

                    string format = "*.txt";
                    foreach (FileInfo fileToCompress in directorySelected.Parent.GetFiles(format))
                    {
                        foreach (string filename in filesList)
                        {
                            if (Server.MapPath(filename) == fileToCompress.FullName)
                            {
                                string filePath = fileToCompress.FullName;
                                zip.AddFile(filePath, "");
                            }
                        }
                    }
                    zipName = String.Format("ImmunizationSubmissionFiles_{0}.zip", DateTime.Now.ToString("dd_MMM_yyyy hh_mm tt"));
                    DirectoryInfo dirSave = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID));
                    if (!dirSave.Exists)
                    {
                        dirSave.Create();
                    }
                    zip.Save(Server.MapPath("atala-capture-download//" + Session.SessionID + "//" + zipName));
                    Filelst.Clear();
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "downloadURI('" + Page.ResolveClientUrl("atala-capture-download//" + Session.SessionID + "//" + zipName) + "');", true);

                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "downloadURI('" + Page.ResolveClientUrl("atala-capture-download//" + Session.SessionID + "//" + zipName) + "');", true);
                }
            }
        }
        class PatientInfo
        {
            public string PID { get; set; }
            public string PatientName { get; set; }
            public string DOB { get; set; }
            public string Gender { get; set; }
        };
        class ImmunizationScheduleInfo
        {
            public string ImmSchedule { get; set; }
        };
        class ImmunizationHistoryInfo
        {
            public string Vaccine_Group { get; set; }
            public string Vaccine_Administered { get; set; }
            public string Date_Administered { get; set; }
            public string Valid_Dose { get; set; }
            public string Validity_Reason { get; set; }
            public string Completion_Status { get; set; }
        };
        class ImmunizationForcastInfo
        {
            public string Vaccine_Group { get; set; }
            public string Due_Date { get; set; }
            public string Earliest_Date_To_Give { get; set; }
            public string Latest_Date_To_Give { get; set; }
        };

        protected string DisplayResponse(string ResponseMsg)
        {
            string[] RespnseArray = ResponseMsg.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            IList<PatientInfo> lstPatInfo = new List<PatientInfo>();
            IList<ImmunizationScheduleInfo> lstSchedule = new List<ImmunizationScheduleInfo>();
            IList<ImmunizationHistoryInfo> lstImmHistory = new List<ImmunizationHistoryInfo>();
            IList<ImmunizationForcastInfo> lstImmForcast = new List<ImmunizationForcastInfo>();
            if (RespnseArray.Length > 0)
            {
                IList<string> lstPID = RespnseArray.Where(a => a.StartsWith("PID")).ToList<string>();
                if (lstPID != null && lstPID.Count > 0)
                {
                    string[] PatInfo = lstPID[0].Split('|');
                    PatientInfo patientInfo = new PatientInfo();
                    patientInfo.PID = PatInfo[3].Split('^')[0];
                    patientInfo.DOB = PatInfo[7].ToString().Substring(4, 2) + "/" + PatInfo[7].ToString().Substring(6, 2) + "/" + PatInfo[7].ToString().Substring(0, 4);
                    patientInfo.PatientName = PatInfo[5].ToString().Split('^')[1] + " " + PatInfo[5].ToString().Split('^')[2] + " " + PatInfo[5].ToString().Split('^')[0];
                    patientInfo.Gender = PatInfo[8].ToString().Equals("M") ? "Male" : PatInfo[7].ToString().Equals("F") ? "Female" : "Others";
                    lstPatInfo.Add(patientInfo);
                    string[] ImmSectin = ResponseMsg.Split(new string[] { "ORC" }, StringSplitOptions.None);
                    for (int i = 1; i < ImmSectin.Count(); i++)
                    {
                        string ImmDataSet = ImmSectin[i].Insert(0, "ORC");
                        string[] ImmArray = ImmDataSet.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        IList<string> lstORC = ImmArray.Where(a => a.StartsWith("ORC")).ToList<string>();
                        IList<string> lstRXA = ImmArray.Where(a => a.StartsWith("RXA")).ToList<string>();
                        IList<string> lstOBX = ImmArray.Where(a => a.StartsWith("OBX")).ToList<string>();
                        IList<string> lstImmSchedInfo = lstOBX.Where(a => a.Contains("Immunization Schedule used")).Select(a => a.Split('|')[5].Split('^')[1].ToString()).Distinct().ToList<string>();
                        foreach (string Immschedule in lstImmSchedInfo)
                        {
                            ImmunizationScheduleInfo objImmSchdle = new ImmunizationScheduleInfo();
                            objImmSchdle.ImmSchedule = Immschedule.ToString();
                            lstSchedule.Add(objImmSchdle);
                        }
                        if (lstRXA != null && lstRXA.Count > 0 && lstOBX != null && lstOBX.Count > 0)
                        {
                            if (lstRXA[0].Contains("no vaccine admin"))
                            {
                                string[] Vacc_Types = ImmDataSet.Split(new string[] { "vaccine type" }, StringSplitOptions.None);
                                for (int j = 1; j < Vacc_Types.Count(); j++)
                                {
                                    string ImmInfo = Vacc_Types[j].Insert(0, "OBX|");
                                    string[] VaccArr = ImmInfo.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                                    IList<string> lstOBXArray = VaccArr.Where(a => a.StartsWith("OBX")).ToList<string>();
                                    IList<string> lstOBXVaccineGroup = lstOBXArray.Where(a => a.ToString().Contains("CVX")).ToList<string>();
                                    IList<string> lstOBXDueDate = lstOBXArray.Where(a => a.ToString().Contains("Date vaccination due")).ToList<string>();
                                    IList<string> lstOBXEarliestDate = lstOBXArray.Where(a => a.ToString().Contains("Earliest Date to give")).ToList<string>();
                                    IList<string> lstOBXLatestDate = lstOBXArray.Where(a => a.ToString().Contains("Latest Date to Give")).ToList<string>();
                                    ImmunizationForcastInfo objImmForecast = new ImmunizationForcastInfo();
                                    objImmForecast.Latest_Date_To_Give = string.Empty;
                                    objImmForecast.Earliest_Date_To_Give = string.Empty;
                                    objImmForecast.Due_Date = string.Empty;
                                    objImmForecast.Vaccine_Group = string.Empty;
                                    if (lstOBXVaccineGroup != null && lstOBXVaccineGroup.Count > 0)
                                        objImmForecast.Vaccine_Group = lstOBXVaccineGroup[0].ToString().Split('|')[3].Split('^')[1];
                                    if (lstOBXDueDate != null && lstOBXDueDate.Count > 0)
                                    {
                                        string Dt = lstOBXDueDate[0].ToString().Split('|')[5];
                                        objImmForecast.Due_Date = Dt.Substring(4, 2) + "/" + Dt.Substring(6, 2) + "/" + Dt.Substring(0, 4);
                                    }

                                    if (lstOBXEarliestDate != null && lstOBXEarliestDate.Count > 0)
                                    {
                                        string Dt = lstOBXEarliestDate[0].ToString().Split('|')[5];
                                        objImmForecast.Earliest_Date_To_Give = Dt.Substring(4, 2) + "/" + Dt.Substring(6, 2) + "/" + Dt.Substring(0, 4);

                                    }
                                    if (lstOBXLatestDate != null && lstOBXLatestDate.Count > 0)
                                    {
                                        string Dt = lstOBXLatestDate[0].ToString().Split('|')[5];
                                        objImmForecast.Latest_Date_To_Give = Dt.Substring(4, 2) + "/" + Dt.Substring(6, 2) + "/" + Dt.Substring(0, 4);

                                    }
                                    lstImmForcast.Add(objImmForecast);
                                }
                            }
                            else
                            {
                                string[] Vacc_Types = ImmDataSet.Split(new string[] { "vaccine type" }, StringSplitOptions.None);
                                for (int j = 1; j < Vacc_Types.Count(); j++)
                                {
                                    string[] VaccInfo = Vacc_Types[j].Split('|');
                                    string[] RxaLst = lstRXA[0].Split('|');
                                    string[] VaccArr = Vacc_Types[j].Split(new string[] { "\r\n" }, StringSplitOptions.None);

                                    IList<string> lstOBXArray = VaccArr.Where(a => a.StartsWith("OBX")).ToList<string>();
                                    IList<string> lstOBXDoseValidity = lstOBXArray.Where(a => a.ToString().Contains("dose validity")).ToList<string>();
                                    IList<string> lstOBXReason = lstOBXArray.Where(a => a.ToString().Contains("Reason applied")).ToList<string>();

                                    ImmunizationHistoryInfo objImmHis = new ImmunizationHistoryInfo();
                                    objImmHis.Vaccine_Group = string.Empty;
                                    objImmHis.Vaccine_Administered = string.Empty;
                                    objImmHis.Valid_Dose = string.Empty;
                                    objImmHis.Validity_Reason = string.Empty;
                                    objImmHis.Completion_Status = string.Empty;
                                    objImmHis.Date_Administered = string.Empty;
                                    objImmHis.Vaccine_Administered = RxaLst[5].Split('^')[1];
                                    objImmHis.Vaccine_Group = VaccInfo[2].Split('^')[1];
                                    objImmHis.Date_Administered = RxaLst[3].ToString().Substring(4, 2) + "/" + lstRXA[0].Split('|')[3].ToString().Substring(6, 2) + "/" + lstRXA[0].Split('|')[3].ToString().Substring(0, 4);
                                    if (lstOBXDoseValidity != null && lstOBXDoseValidity.Count > 0)
                                    {
                                        objImmHis.Valid_Dose = lstOBXDoseValidity[0].ToString().Split('|')[5].Equals("Y") ? "Yes" : lstOBXDoseValidity[0].ToString().Split('|')[5].Equals("N") ? "No" : "";
                                        if (objImmHis.Valid_Dose == "No")
                                        {
                                            if (lstOBXReason != null && lstOBXReason.Count > 0)
                                            {
                                                objImmHis.Validity_Reason = lstOBXReason[0].ToString().Split('|')[5];
                                            }
                                        }

                                    }
                                    objImmHis.Completion_Status = RxaLst[RxaLst.Length - 1].Equals("CP") ? "Complete" : "";
                                    lstImmHistory.Add(objImmHis);
                                }
                            }
                        }

                    }
                }
                else
                {
                    IList<string> lstQPD = RespnseArray.Where(a => a.StartsWith("QPD")).ToList<string>();
                    IList<string> lstQAK = RespnseArray.Where(a => a.StartsWith("QAK")).ToList<string>();
                    if (lstQPD != null && lstQPD.Count > 0)
                    {
                        PatientInfo patientInfo = new PatientInfo();
                        patientInfo.PID = lstQPD[0].ToString().Split('|')[3].Split('^')[0];
                        patientInfo.PatientName = lstQPD[0].ToString().Split('|')[4].Split('^')[1] + " " + lstQPD[0].ToString().Split('|')[4].Split('^')[2] + " " + lstQPD[0].ToString().Split('|')[4].Split('^')[0];
                        patientInfo.Gender = lstQPD[0].ToString().Split('|')[7].Equals("M") ? "Male" : lstQPD[0].ToString().Split('|')[7].Equals("F") ? "Female" : "Others";
                        string Dt = lstQPD[0].ToString().Split('|')[6];
                        patientInfo.DOB = Dt.Substring(4, 2) + "/" + Dt.Substring(6, 2) + "/" + Dt.Substring(0, 4);
                        lstPatInfo.Add(patientInfo);

                    }
                    if (lstQAK != null && lstQAK.Count > 0)
                    {
                        string[] QakResponse = lstQAK[0].Split('|');
                        ImmunizationScheduleInfo objImmschedule = new ImmunizationScheduleInfo();
                        objImmschedule.ImmSchedule = QakResponse[2].Equals("NF") ? "The query for an Evaluated Immunization History and Immunization Forecast is complete but no matching records were found for the person in the query" : QakResponse[2].Equals("TM") ? "The query for an Evaluated Immunization History and Immunization Forecast is complete but too many matching records were found for the person in the query" : "No Valid Response";
                        lstSchedule.Add(objImmschedule);
                    }
                }

            }
            var lstPatInformation = lstPatInfo.Select(a => new
            {
                PID = a.PID,
                PatientName = a.PatientName,
                DOB = a.DOB,
                Gender = a.Gender
            });
            var lstImmSchedule = lstSchedule.Select(a => new
            {
                Imm_Schedule = a.ImmSchedule
            }).Distinct();
            var lstImmHistInfo = lstImmHistory.Select(a => new
            {
                Vaccine_Group = a.Vaccine_Group,
                Vaccine_Administered = a.Vaccine_Administered,
                Date_Administered = a.Date_Administered,
                Valid_Dose = a.Valid_Dose,
                Validity_Reason = a.Validity_Reason,
                Completion_Status = a.Completion_Status
            });
            var lstImmForecastInfo = lstImmForcast.Select(a => new
            {
                Vaccine_Group = a.Vaccine_Group,
                Due_Date = a.Due_Date,
                Earliest_Date_To_Give = a.Earliest_Date_To_Give,
                Latest_Date_To_Give = a.Latest_Date_To_Give
            });
            var Result = new
            {
                PatientInfo = lstPatInformation,
                ImmScheduleInfo = lstImmSchedule,
                ImmHistoryInfo = lstImmHistInfo,
                ImmHistoryForcstInfo = lstImmForecastInfo
            };
            return JsonConvert.SerializeObject(Result);
        }
        public void GetImmunizationHistoryDataFromLog(ulong HumanID)
        {
            ImmLogsData.Clear();
            RegistryLogManager ImmSubManager = new RegistryLogManager();
            IList<RegistryLog> ImmLogs = ImmSubManager.GetImmunizationLogsByHumanID(HumanID);
            if (ImmLogs != null && ImmLogs.Count > 0)
            {
                foreach (RegistryLog log in ImmLogs)
                {
                    if (log.Response_Message.Trim() != string.Empty)
                    {
                        string dt = UtilityManager.ConvertToLocal(log.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        ImmLogsData.Add(dt, log.Response_Message);
                    }
                }
            }
        }
        public void LoadImmunizationSubmissionHistoryGrid()
        {
            if (ImmLogsData != null && ImmLogsData.Count > 0)
            {
                foreach (var item in ImmLogsData.Keys)
                {
                    System.Web.UI.HtmlControls.HtmlAnchor anchor = new System.Web.UI.HtmlControls.HtmlAnchor();
                    anchor.ID = item.ToString();
                    anchor.Style.Add("color", "blue");
                    anchor.Style.Add("text-decoration", "underline");
                    anchor.Style.Add("cursor", "pointer");
                    anchor.InnerText = item.ToString();
                    anchor.Style.Add("font-size", "14px");
                    anchor.Attributes.Add("onclick", "PopulateGrid('" + item.ToString() + "');");
                    anchor.Attributes.Add("target", "_self");
                    DatesListItems.Controls.Add(anchor);
                    DatesListItems.Controls.Add(new LiteralControl("<br/>"));

                }
            }

        }

        [WebMethod(EnableSession = true)]
        public static string PopulatePatientImmHistoryData(string DateTimeInfo)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string responseMsg = string.Empty;
            if (ImmLogsData != null && ImmLogsData.Count > 0 && ImmLogsData.ContainsKey(DateTimeInfo))
            {
                frmExchange objExchnage = new frmExchange();
                responseMsg = objExchnage.DisplayResponse(ImmLogsData[DateTimeInfo]);
            }
            return responseMsg;
        }

    }
}