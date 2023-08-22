using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTO;
using System.IO;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;


namespace Acurus.Capella.UI
{
    public partial class frmResultInterpretation : System.Web.UI.Page
    {
        StaticLookupManager staticMngr = new StaticLookupManager();
        IList<StaticLookup> lstTestOrdered = null;
        public Dictionary<string, string> Templatesource
        {
            get
            {
                return (Dictionary<string, string>)ViewState["Templatesource"] ?? new Dictionary<string, string>();
            }
            set
            {
                ViewState["Templatesource"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["HumanText"] != null && Request["HumanText"] != "")
                {
                    txtPatientInformation.Value = Request["HumanText"].ToString();
                    txtPatientInformation.Visible = true;
                }
                else
                    txtPatientInformation.Visible = false;
                if (Request["FileText"] != null && Request["FileText"] != "")
                {
                    txtFileInformation.Value = Request["FileText"].ToString();
                }

                txtSummary.Attributes.Add("onkeydown", "insertTab(this,event);");
                txtSummary.Attributes.Add("onfocus", "focusTab(this,event);");
                lstTestOrdered = staticMngr.getStaticLookupByFieldName("RESULT INTERPRETATION TEST FOR " + Request["DocumentSubType"].ToString());
                Templatesource = new Dictionary<string, string>();
                //lstTestOrdered = staticMngr.getStaticLookupByFieldName("RESULT INTERPRETATION TEST FOR NUCLEAR MEDICINE");
                ddlTemplate.Items.Add("");
                Templatesource.Add("", "");
                for (int iCount = 0; iCount < lstTestOrdered.Count; iCount++)
                {
                    ddlTemplate.Items.Add(lstTestOrdered[iCount].Value.ToString());
                    Templatesource.Add(lstTestOrdered[iCount].Value.ToString(), lstTestOrdered[iCount].Description.ToString());
                }
                if (Request["ProviderNotes"] != null && Request["ProviderNotes"] != "")
                {
                    string[] sSeperator = new string[] { "Test Reviewed: " };

                    //Test Reviewed: 

                    // string[] reviewcomments = objresultmaster.Result_Review_Comments.Split(new string[] { "]]]" }, StringSplitOptions.None);

                    string[] sTemplate = Request["ProviderNotes"].ToString().Split(new string[] { "Test Reviewed: " }, StringSplitOptions.None);
                    if (sTemplate.Count() > 1)
                    {
                        for (int i = 0; i < sTemplate.Length; i++)
                        {
                            if (sTemplate[i].Trim() != string.Empty && sTemplate[i].Contains(';'))
                            {
                                ListItem item = new ListItem();
                                item.Text = sTemplate[i].Split(';')[0];
                                ddlTemplate.SelectedIndex = ddlTemplate.Items.IndexOf(item);

                                // Cap - 747
                                //string Notes = sTemplate[i].Split(';')[1].Replace("\\n", "\n").Replace("\\t", "\t").Replace("\\r", "\r").Replace("\"", "");
                                string Notes = sTemplate[i].Split(';')[1].Replace("$|$|$|$|", "&").Replace("!^!^!^!^", "#").Replace("~|~|~|~|","+").Replace("\\n", "\n").Replace("\\t", "\t").Replace("\\r", "\r").Replace("\"", "");

                                txtSummary.Text = Notes.TrimStart('\n');
                                break;
                            }
                        }
                    }
                }
                if (ddlTemplate.Text == string.Empty)
                    btnSaveInt.Disabled = true;
            }
        }

      /*  protected void btnPrintInt_ServerClick(object sender, EventArgs e)
        {
            PrintOrders print = new PrintOrders();
            string sOutput = string.Empty;

            string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

            DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);

            string sPhysicianName = string.Empty;
            XmlDocument xmldoc1 = new XmlDocument();
            string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
            if (File.Exists(strXmlFilePath1) == true)
            {
                xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + ClientSession.CurrentPhysicianId);
                if (nodeMatchingPhysicianAddress != null)
                {
                    sPhysicianName = nodeMatchingPhysicianAddress.Attributes["Physician_prefix"].Value.ToString() + " " +
                    nodeMatchingPhysicianAddress.Attributes["Physician_First_Name"].Value.ToString() + " " +
                    nodeMatchingPhysicianAddress.Attributes["Physician_Middle_Name"].Value.ToString() + " " +
                    nodeMatchingPhysicianAddress.Attributes["Physician_Last_Name"].Value.ToString() + " " +
                    nodeMatchingPhysicianAddress.Attributes["Physician_Suffix"].Value.ToString();
                }
            }


            string sPatientInfo = string.Empty;

            if (txtPatientInformation.Value != string.Empty && txtPatientInformation.Value.Split('|').Length > 5)
            {
                sPatientInfo = "Patient Name: " + txtPatientInformation.Value.Split('|')[0] + Environment.NewLine +
                "Date of Birth: " + txtPatientInformation.Value.Split('|')[1] + Environment.NewLine +
                "MRN: " + txtPatientInformation.Value.Split('|')[5].Split(':')[1] + Environment.NewLine;
            }

            string sHumanID = string.Empty;
            if (Session["human_id"] != null)
                sHumanID = Session["human_id"].ToString();
            if (ddlTemplate.Items[ddlTemplate.SelectedIndex].Text != "")
            {
                string sSignDate = string.Empty;
                string Notes = "Test Reviewed: " + ddlTemplate.Items[ddlTemplate.SelectedIndex].Text.ToString() + ";\n" + txtSummary.Text;

                if (Request["ProviderNotesHistory"] != null)
                {

                    string[] sTemplate = Request["ProviderNotes"].Split(new string[] { "Test Reviewed: " }, StringSplitOptions.None);
                    if (sTemplate.Count() > 1)
                    {
                        for (int i = 0; i < sTemplate.Length; i++)
                        {
                            if (sTemplate[i].Trim() != string.Empty && sTemplate[i].Contains(';') && sTemplate[i].StartsWith(ddlTemplate.Items[ddlTemplate.SelectedIndex].Text) == true)
                            {
                                if (Request["ProviderNotesHistory"].Replace("\\n", "\n").Contains('\n') == true && Request["ProviderNotesHistory"].Replace("\\n", "\n").Split('\n').Length > (i - 1))
                                {
                                    if (Request["ProviderNotesHistory"].Replace("\\n", "\n").Split('\n')[i - 1].Contains("(") == true && Request["ProviderNotesHistory"].Replace("\\n", "\n").Split('\n')[i - 1].Contains(")") == true)
                                    {
                                        sSignDate = ExtractBetween(Request["ProviderNotesHistory"].Replace("\\n", "\n").Split('\n')[i - 1], "(", ")");
                                    }
                                }
                            }
                        }
                    }
                }
                string sFaclilityName = ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper();
                // sOutput = print.PrintInterpretationNotes(Notes, sPhysicianName, ClientSession.FacilityName, sPatientInfo, sHumanID, TargetFileDirectory, ddlTemplate.Text, sSignDate);
                sOutput = print.PrintInterpretationNotes(Notes, sPhysicianName, sFaclilityName, sPatientInfo, sHumanID, TargetFileDirectory, ddlTemplate.Text, sSignDate,sPhysicianName);
                string sPrintPathName = sOutput.Split('|')[0];
                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                hdnFileName.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintInterpretation", "PrintInterpretation();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintError", "DisplayErrorMessage('115059');", true);

            }
        }
        */

        protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Templatesource != null && (Templatesource.ContainsKey(ddlTemplate.Items[ddlTemplate.SelectedIndex].Text.ToString())))
            {

                int i = 0;
                string[] sTemplate = Request["ProviderNotes"].ToString().Split(new string[] { "Test Reviewed: " }, StringSplitOptions.None);
                if (sTemplate.Count() > 0)
                {
                    for (i = 0; i < sTemplate.Length; i++)
                    {
                        if (sTemplate[i].Trim() != string.Empty && sTemplate[i].Contains(';') && sTemplate[i].Contains(ddlTemplate.Items[ddlTemplate.SelectedIndex].Text))
                        {
                            ListItem item = new ListItem();
                            item.Text = sTemplate[i].Split(';')[0];
                            ddlTemplate.SelectedIndex = ddlTemplate.Items.IndexOf(item);

                            // Cap - 747
                            //string Notes = sTemplate[i].Split(';')[1].Replace("\\n", "\n").Replace("\\t", "\t").Replace("\\r", "\r").Replace("\"", "");
                            string Notes = sTemplate[i].Split(';')[1].Replace("$|$|$|$|", "&").Replace("!^!^!^!^", "#").Replace("~|~|~|~|", "+").Replace("\\n", "\n").Replace("\\t", "\t").Replace("\\r", "\r").Replace("\"", "");

                            txtSummary.Text = Notes.TrimStart('\n');
                            break;
                        }
                    }
                }

                if (i == sTemplate.Length)
                    txtSummary.Text = Templatesource[ddlTemplate.Items[ddlTemplate.SelectedIndex].Text];
                btnSaveInt.Disabled = false;
                //btnPrintInterpretation.Disabled = false;
                btnReset.Enabled = true;
            }
            else
            {
                btnSaveInt.Disabled = true;
                //btnPrintInterpretation.Disabled = true;
                btnReset.Enabled = false;
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



        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string PrintIntrepretationNotesData(string sResultMasterId)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            ResultMasterManager objResMasMngr = new ResultMasterManager();
            IList<ResultMaster> lstresultmaster = new List<ResultMaster>();
            string sFacilityAddress = string.Empty;
         
            lstresultmaster = objResMasMngr.GetResultMasterListByResultmasterIDForMRE(Convert.ToUInt32(sResultMasterId));

            string sResultReviewComments = string.Empty;
            int iSignedPhysicianId = 0;
            string sSignDate = string.Empty;
            string sSignedPhysicianName = string.Empty;

            if (lstresultmaster.Count() > 0)
            {
                sResultReviewComments = lstresultmaster[0].Result_Review_Comments;
                iSignedPhysicianId = lstresultmaster[0].Reviewed_Provider_Sign_ID;

                OrdersSubmitManager OrdersSubmitMngr = new OrdersSubmitManager();
                sFacilityAddress = OrdersSubmitMngr.GetLabLocationAddressbyOrderSubmitID(lstresultmaster[0].Order_ID);
               
                if (iSignedPhysicianId != 0)
                {
                    if (lstresultmaster[0].Modified_Date_And_Time.ToString("yyyy-MM-dd") != "0001-01-01")
                    {
                        sSignDate = UtilityManager.ConvertToLocal(lstresultmaster[0].Modified_Date_And_Time).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        sSignDate = UtilityManager.ConvertToLocal(lstresultmaster[0].Created_Date_And_Time).ToString("dd-MMM-yyyy");
                    }

                    XmlDocument xmldoc1 = new XmlDocument();
                    string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                    if (File.Exists(strXmlFilePath1) == true )
                    {
                        xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                        XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + iSignedPhysicianId);
                        if (nodeMatchingPhysicianAddress != null)
                        {  
                            sSignedPhysicianName = nodeMatchingPhysicianAddress.Attributes["Physician_prefix"].Value.ToString() + " " +
                            nodeMatchingPhysicianAddress.Attributes["Physician_First_Name"].Value.ToString() + " " +
                            nodeMatchingPhysicianAddress.Attributes["Physician_Middle_Name"].Value.ToString() + " " +
                            nodeMatchingPhysicianAddress.Attributes["Physician_Last_Name"].Value.ToString() + " " +
                            nodeMatchingPhysicianAddress.Attributes["Physician_Suffix"].Value.ToString();
                        }
                    }
                }
            }

            var result = new
            {
                ResultReviewComments = sResultReviewComments,
                SignedDate = sSignDate,
                SignedPhyName = sSignedPhysicianName,
                FacilityAddress = sFacilityAddress

            };
            return JsonConvert.SerializeObject(result);
        }


    }
}