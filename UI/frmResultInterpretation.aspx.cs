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
using Org.BouncyCastle.Asn1.Ocsp;

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
                //Cap - 686
                if (Request["CurrentProcess"] != null)
                {
                    if (Request["CurrentProcess"].ToString() == "BILLING_WAIT")
                    {
                        btnDelete.Visible = false;
                    }
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
                        //if (sTemplate[i].Trim() != string.Empty && sTemplate[i].Contains(';') && sTemplate[i].Contains(ddlTemplate.Items[ddlTemplate.SelectedIndex].Text))
                        if (sTemplate[i].Trim() != string.Empty && sTemplate[i].Contains(';') && sTemplate[i].Split(';')[0] == ddlTemplate.Items[ddlTemplate.SelectedIndex].Text)
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

        //Cap - 686
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ResultMasterManager masterProxy = new ResultMasterManager();
            IList<ResultMaster> lstResultMaster = new List<ResultMaster>();
            ResultMaster objResultMaster = new ResultMaster();
            ResultMasterManager rsManager = new ResultMasterManager();
            ulong ulFileManagementIndexID = 0;
            ulong resMasID = 0;

            if (Session["Order_Id"] != null && Session["Order_Id"] != string.Empty && Convert.ToUInt32(Session["Order_Id"]) != 0)
            {
                lstResultMaster = rsManager.GetResultReviewNotesBasedOnOrderSubmitId(Convert.ToUInt32(Session["Order_Id"]));
                if (lstResultMaster != null && lstResultMaster.Count > 0 && lstResultMaster.Count == 1)
                {
                    //Jira #CAP-921 - Condition added in the top if condition
                   // if (lstResultMaster.Count == 1)
                    //{
                        objResultMaster = lstResultMaster[0];
                    //}
                }
                else if (lstResultMaster.Count > 1)
                {
                    if (Session["Result_Master_Id"] != null && UInt64.TryParse(Session["Result_Master_Id"].ToString(), out resMasID))
                    {
                        objResultMaster = rsManager.GetById(resMasID);
                    }
                    else if (Request["ResultMasterID"] != null && UInt64.TryParse(Request["ResultMasterID"], out resMasID))
                    {
                        objResultMaster = rsManager.GetById(resMasID);
                    }

                }
            }
            else if (Session["Result_Master_Id"] != null && UInt64.TryParse(Session["Result_Master_Id"].ToString(), out resMasID))
            {
                objResultMaster = rsManager.GetById(resMasID);
                if (objResultMaster != null && objResultMaster.Id != 0)
                {
                    objResultMaster.Modified_By = ClientSession.UserName;
                    objResultMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    objResultMaster = new ResultMaster();
                }
            }
            else if (Request["ResultMasterID"] != null && UInt64.TryParse(Request["ResultMasterID"], out resMasID) && resMasID != 0)
            {
                objResultMaster = rsManager.GetById(resMasID);
                if (objResultMaster != null && objResultMaster.Id != 0)
                {
                    objResultMaster.Modified_By = ClientSession.UserName;
                    objResultMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115057'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                    return;
                }
            }

            if (objResultMaster != null && objResultMaster.Id != 0)
            {
                objResultMaster.Modified_By = ClientSession.UserName;
                objResultMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                ulFileManagementIndexID = Convert.ToUInt64(Session["Key_id"]);

                string[] Result_Review_Comments = objResultMaster.Result_Review_Comments.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                objResultMaster.Result_Review_Comments = string.Empty;
                for (int i = 0; i < Result_Review_Comments.Length; i++)
                {

                    if (Result_Review_Comments[i].Trim() != string.Empty && Result_Review_Comments[i].Trim().Contains("Test Reviewed"))
                    {
                        String sHeader_Test = Result_Review_Comments[i].Substring(0, Result_Review_Comments[i].IndexOf(";")).Replace("[[[Test Reviewed: ", "");
                        string[] sHeader = sHeader_Test.Split(':');
                        string sddlTemplate = ddlTemplate.SelectedValue;

                        if (sHeader[0] != string.Empty && sHeader[2] != string.Empty && sHeader[2].Trim() == sddlTemplate.Trim() && sHeader[0].Split('(').Length > 0 && sHeader[0].Split('(')[0].Replace("@", "") != ClientSession.UserName && Request["CheckVisible"] != null && Request["CheckVisible"] == "false")
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115069'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                            return;
                        }
                        if (sHeader[2] != string.Empty && sHeader[2].Trim() != sddlTemplate.Trim())
                        {
                            if (objResultMaster.Result_Review_Comments == string.Empty)
                            {
                                objResultMaster.Result_Review_Comments = Result_Review_Comments[i];
                            }
                            else
                            {
                                objResultMaster.Result_Review_Comments = objResultMaster.Result_Review_Comments + "<br/>" + Result_Review_Comments[i];
                            }
                        }
                    }

                }
                if (objResultMaster.Result_Review_Comments.EndsWith("<br/>") == true)
                {
                    objResultMaster.Result_Review_Comments = objResultMaster.Result_Review_Comments.Substring(0, objResultMaster.Result_Review_Comments.Length - 5);
                }

                objResultMaster = rsManager.SaveResultMasterItem(objResultMaster, ulFileManagementIndexID);

                if (Templatesource != null && (Templatesource.ContainsKey(ddlTemplate.Items[ddlTemplate.SelectedIndex].Text.ToString())))
                {
                    txtSummary.Text = Templatesource[ddlTemplate.Items[ddlTemplate.SelectedIndex].Text];
                }

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DeletedInterpretationNotes();", true);

            }
            //Jira #CAP-925
            else
            {
                if (Templatesource != null && (Templatesource.ContainsKey(ddlTemplate.Items[ddlTemplate.SelectedIndex].Text.ToString())))
                {
                    txtSummary.Text = Templatesource[ddlTemplate.Items[ddlTemplate.SelectedIndex].Text];
                }

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DeletedInterpretationNotes();", true);
            }



        }

    }
}