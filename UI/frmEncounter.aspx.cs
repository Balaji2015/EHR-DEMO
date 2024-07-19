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
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.Collections.Generic;
using Telerik.Web.UI;
using Acurus.Capella.DataAccess.ManagerObjects;
using UI;
using System.Reflection;
using Acurus.Capella.UI.RCopia;
using System.Xml;
using System.IO;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Threading;
using DocumentFormat.OpenXml.Wordprocessing;
using ListItem = System.Web.UI.WebControls.ListItem;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Newtonsoft.Json;
using Acurus.Capella.UI.Extensions;

namespace Acurus.Capella.UI
{

    public partial class frmEncounter : System.Web.UI.Page
    {
        Boolean bIsWorkFlowButtonClicked = false;
        Boolean bMyDigitalSign = false;
        string sMyObjType = string.Empty;
        public WFObject ehrwfobj = new WFObject();

        FillEncounterandWFObject MyEncWFRecord;
        public Encounter EncRecord;
        static DateTime localTime;
        int iMySelectedNode = 0;
        RCopiaSessionManager rcopiaSessionMngr;
        IList<PatientPane> PatientPaneDetails;
        string sHumanName = string.Empty;
        public string strPhysicianDetails = string.Empty;
        DateTime sDateofService;
        PhysicianManager objPhysicianManager = new PhysicianManager();
        string sMyPhyUserName = string.Empty;
        Boolean bFormClose = false;
        //string sMySentToRCopia = string.Empty;//For Bug ID 74727
        EncounterManager objEncounterManager = new EncounterManager();
        bool bDuplicateCheck = true;
        MapPhysicianPhysicianAssitantManager ObjUserMgr = new MapPhysicianPhysicianAssitantManager();
        DocumentManager objDocumentManager = new DocumentManager();
        WFObjectManager objWFObjectManager = new WFObjectManager();
        Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
        //TemplateMasterManager objtempMngr = new TemplateMasterManager();
        FollowUpEncounterDTO ObjFollowUpEncounterDTO = null;
        bool Copy_Previous_Click = true;
        public bool is_save_click = false;
        bool Is_First = false;
        FillPhysicianUser PhyUserList;
        string DefaultTabName = string.Empty;
        string ChildTabName = string.Empty;
        string SourceOfInformation = string.Empty;
        string sContent = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageWindow.Visible = false;
            MessageWindow.VisibleOnPageLoad = false;
            hdnUserRole.Value = ClientSession.UserRole.ToString();
            //CAP-1511
            if (!string.IsNullOrEmpty(Request.QueryString["Screen"]))
            {
                hdnScreen.Value = Request.QueryString["Screen"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["SubScreen"]))
            {
                hdnSubScreen.Value = Request.QueryString["SubScreen"];
            }

            int node = 0;

            //if (ClientSession.EncounterId == 0)
            //{
            //    return;
            //}
            Page.Header.DataBind();
            if (Request.QueryString["tabName"] != null)
                DefaultTabName = Request.QueryString["tabName"].ToString();

            if (Request.QueryString["ChildTabName"] != null)
                ChildTabName = Request.QueryString["ChildTabName"].ToString();

            if (Request.QueryString["cboValue"] != null)
                SourceOfInformation = Request.QueryString["cboValue"].ToString();


            if (Request.QueryString["Date"] != null && Request.QueryString["Date"].ToString().Trim() != string.Empty)
            {
                hdnLocalTime.Value = Request.QueryString["Date"].ToString();
                localTime = Convert.ToDateTime(hdnLocalTime.Value);
            }

            if (DateTime.TryParse(hdnLocalTime.Value, out localTime))
                localTime = Convert.ToDateTime(hdnLocalTime.Value);


            if (Request.QueryString["currentAddendumId"] != null)
                hdnAddendumID.Value = Request.QueryString["currentAddendumId"].ToString();
            else
                hdnAddendumID.Value = "0";
            if (Request.QueryString["EncounterID"] != null)
                ClientSession.EncounterId = Convert.ToUInt32(Request.QueryString["EncounterID"].ToString());

            if (ClientSession.SetSelectedTab != null && ClientSession.SetSelectedTab != string.Empty && ClientSession.SetSelectedTab.Contains('#') && ClientSession.SetSelectedTab.Split('#')[1].ToUpper() == "OPENED")
            {
                DefaultTabName = ClientSession.SetSelectedTab.Split('#')[0].Split('*')[0];
            }
            if (!IsPostBack)
            {
                if (ViewState["status"] == null)
                {
                    ViewState["status"] = false;
                }

                ClientSession.FlushSession();
                sMyPreviousTab.Value = "CC / HPI";

                //  MyEncWFRecord = ClientSession.FillEncounterandWFObject;
                if (Request.QueryString["leftpane"] != null && Request.QueryString["leftpane"].ToString() == "Y")
                {
                    // ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.EncounterId, UtilityManager.ConvertToLocal(DateTime.ParseExact(hdnLocalTime.Value.Trim(), "M/d/yyyy H:m:s", null)), string.Empty, ClientSession.UserName, true, Convert.ToUInt32(hdnAddendumID.Value), ClientSession.CurrentObjectType, false);// 0);
                    ClientSession.FillPatientChart.Fill_Encounter_and_WFObject = objEncounterManager.GetEncounterandWFObject(ClientSession.EncounterId, 0, ClientSession.CurrentObjectType);
                    //CAP-1767
                    ClientSession.UserCurrentProcess = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject?.DocumentationWFRecord?.Current_Process ?? ClientSession.UserCurrentProcess;
                    ClientSession.UserCurrentOwner = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject?.DocumentationWFRecord?.Current_Owner ?? ClientSession.UserCurrentOwner;
                }


                if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.Fill_Encounter_and_WFObject != null)
                    ClientSession.FillEncounterandWFObject = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject;
                if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.Fill_Encounter_and_WFObject != null && ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord != null)
                    EncRecord = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord;
                if (ClientSession.PatientPaneList != null)
                    PatientPaneDetails = ClientSession.PatientPaneList;
                if (ClientSession.CurrentObjectType != null && ClientSession.CurrentObjectType == string.Empty)
                {
                    // When patient chart is opened from Menu - Obj Type and Encounter ID must be cleared
                    sMyObjType = "ENCOUNTER";
                }
                else
                {
                    sMyObjType = ClientSession.CurrentObjectType;
                }

                //ClientSession.processCheck = false;
                if (ClientSession.UserCurrentProcess == string.Empty)
                {
                    ClientSession.processCheck = true;
                    ClientSession.UserCurrentProcess = "DISABLE";
                }
                else
                {
                    ClientSession.processCheck = false;
                }
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                if (ClientSession.UserCurrentProcess == "DISABLE")
                {
                    tabStripEncounter_tbEPrescription.Disabled = true;
                }

                //To visible\envisible QR Code Generator button
                if (ClientSession.UserPermissionDTO != null && ClientSession.UserPermissionDTO.Userscntab != null)
                {
                    var userQRCode = from u in ClientSession.UserPermissionDTO.Userscntab where u.scn_id == 101131 && u.user_name == ClientSession.UserName select u;
                    if (userQRCode.ToList().Count > 0)
                    {
                        btnQRCode.Visible = true;
                    }
                    else
                    {
                        btnQRCode.Visible = false;
                        btnAkidoNote.Attributes.CssStyle.Add("margin-left", "8px !important");
                    }

                    var userAkidoNote = from u in ClientSession.UserPermissionDTO.Userscntab where u.scn_id == 101132 && u.user_name == ClientSession.UserName select u;
                    //CAP-2170 - Disable Akido Note button where the primary physician of encounter is not supervising provider
                    if (userAkidoNote.ToList().Count > 0 && !ClientSession.UserCurrentProcess.Contains("PROVIDER_REVIEW"))
                    {
                        btnAkidoNote.Visible = true;
                        hdnAkidoNote.Value = System.Configuration.ConfigurationSettings.AppSettings["AkidoNoteURL"];
                        hdnEncounterID.Value = ClientSession.EncounterId.ToString();
                    }
                    else
                    {
                        btnAkidoNote.Visible = false;
                    }
                }

                ClientSession.CurrentObjectType = sMyObjType;
                if (PatientPaneDetails != null && PatientPaneDetails.Count > 0 && PatientPaneDetails.Any(item => item.Encounter_ID == ClientSession.EncounterId))
                    iMySelectedNode = PatientPaneDetails.ToList().FindIndex(item => item.Encounter_ID == ClientSession.EncounterId);
                else
                    iMySelectedNode = 0;
                //iMySelectedNode = (PatientPaneDetails.Count > 0 ? ((PatientPaneDetails.Select((item, index) => new { Obj = item, Index = index }).Count() > 0) ? (PatientPaneDetails.Select((item, index) => new { Obj = item, Index = index }).First(a => a.Obj.Encounter_ID == ClientSession.EncounterId).Index) : 0) : 0);
                // sMySentToRCopia = Request["sMySentToRCopia"];//For Bug ID 74727
                if (PatientPaneDetails != null && PatientPaneDetails.Count > 0)
                    ClientSession.PhysicianUserName = PatientPaneDetails[iMySelectedNode].Assigned_Physician_User_Name;

                node = iMySelectedNode;
                ViewState["Node"] = node;
                //CAP-790
                if (hdnTab != null)
                    hdnTab.Value = string.Empty;


                //Encounter> ilstEncounter = EncRecord;// objEncounterManager.GetEncounterByEncounterID(ClientSession.EncounterId);
                //CAP-951
                if ((EncRecord?.Is_PFSH_Verified?.Trim()??"") == "Y")
                    ClientSession.bPFSHVerified = true;
                hdnACOValidated.Value = "False";

                // For Provider Validation..
                if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                {
                    hdnAppointmentProviderId.Value = Convert.ToString(ClientSession.FillEncounterandWFObject.EncRecord.Appointment_Provider_ID);
                    hdnEncounterProviderId.Value = Convert.ToString(ClientSession.FillEncounterandWFObject.EncRecord.Encounter_Provider_ID);
                }
                if (hdnAppointmentProviderId.Value != Convert.ToString(0) && hdnEncounterProviderId.Value != Convert.ToString(0))
                {
                    if (hdnAppointmentProviderId.Value != hdnEncounterProviderId.Value)
                    {
                        PhysicianManager objPhysicianManager = new PhysicianManager();
                        PhysicianLibrary objPhysicianLibraryEncId = null;
                        if (hdnEncounterProviderId.Value != null)
                            objPhysicianLibraryEncId = objPhysicianManager.GetphysiciannameByPhyID(Convert.ToUInt32(hdnEncounterProviderId.Value))
                                                                                          .ToList<PhysicianLibrary>()
                                                                                          .FirstOrDefault();

                        PhysicianLibrary objPhysicianLibraryAppId = null;
                        if (hdnAppointmentProviderId.Value != null)
                            objPhysicianLibraryAppId = objPhysicianManager.GetphysiciannameByPhyID(Convert.ToUInt32(hdnAppointmentProviderId.Value))
                                                                                           .ToList<PhysicianLibrary>()
                                                                                           .FirstOrDefault();


                        hdnAppointmentProviderName.Value = objPhysicianLibraryAppId.PhyPrefix +
                                                           objPhysicianLibraryAppId.PhyFirstName + " " +
                                                           objPhysicianLibraryAppId.PhyMiddleName + " " +
                                                           objPhysicianLibraryAppId.PhyLastName;

                        hdnEncounterProviderName.Value = objPhysicianLibraryEncId.PhyPrefix +
                                                         objPhysicianLibraryEncId.PhyFirstName + " " +
                                                         objPhysicianLibraryEncId.PhyMiddleName + " " +
                                                         objPhysicianLibraryEncId.PhyLastName;
                    }
                }
                bool IsEligibleForReview = false;
                if (ClientSession.UserRole != null && ClientSession.UserCurrentProcess != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && ClientSession.UserCurrentProcess != "REVIEW_CORRECTION" &&
                ClientSession.UserCurrentProcess != "PROVIDER_REVIEW_CORRECTION" && ClientSession.UserCurrentProcess != "CODER_REVIEW_CORRECTION")
                {
                    chkProviderReview.Visible = true;
                    if (EncRecord != null && EncRecord.Id != 0)
                        IsEligibleForReview = CheckIfEligibleForProviderReview(EncRecord.Id);
                    if (IsEligibleForReview)
                        chkProviderReview.Checked = true;
                }
                else
                {
                    chkProviderReview.Visible = false;
                }
                if (EncRecord != null && EncRecord.Assigned_Scribe_User_Name.Trim() != string.Empty && ClientSession.UserCurrentProcess.ToUpper() == "PROVIDER_PROCESS")
                {
                    chkcorrectionreview.Visible = true;
                }
                else
                {
                    chkcorrectionreview.Visible = false;
                }
                if (EncRecord != null && EncRecord.Id != 0)
                {
                    Session["AppointmenttProvID"] = EncRecord.Appointment_Provider_ID;
                    Session["EligibleProvReview"] = IsEligibleForReview.ToString();
                    hdnReviewStatus.Value = IsEligibleForReview.ToString();
                }
                if (ClientSession.UserCurrentProcess.ToUpper() == "SURGERY_COORDINATOR_PROCESS")
                {
                    if (ehrwfobj.Current_Owner.ToUpper() == ClientSession.UserName.ToUpper())
                    {
                        btnMove.Visible = true;
                    }
                    else
                    {
                        btnMove.Visible = false;
                    }
                    btnMove.Value = "Move to Next Process";
                    btnCopyPreviousEncounter.Visible = false;
                }

                bool issuccess = LoadPage();
                if (!issuccess)
                {
                    ViewState["IsRegenerateXML"] = true;
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.EncounterId + "','Encounter','encounter');", true);


                    //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                    return;
                }
            }
            else
            {
                if (ViewState["Chkoutbtn"] != null && Convert.ToBoolean(ViewState["Chkoutbtn"]) == false)
                {
                    btnPhysiciancorrection.Disabled = true;
                }
                if (ClientSession.CurrentObjectType != null)
                    sMyObjType = ClientSession.CurrentObjectType;
                if (ClientSession.FillEncounterandWFObject.EncounterWFRecord != null && (sMyObjType == "ENCOUNTER" || sMyObjType == "ADDENDUM"))
                    ehrwfobj = ClientSession.FillEncounterandWFObject.EncounterWFRecord;
                else if (ClientSession.FillEncounterandWFObject.DocumentationWFRecord != null && sMyObjType == "DOCUMENTATION")
                    ehrwfobj = ClientSession.FillEncounterandWFObject.DocumentationWFRecord;
                //else if (ClientSession.FillEncounterandWFObject.DocReviewWFRecord != null && sMyObjType == "DOCUMENT REVIEW")
                //    ehrwfobj = ClientSession.FillEncounterandWFObject.DocReviewWFRecord;

                if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                    EncRecord = ClientSession.FillEncounterandWFObject.EncRecord;
                LoadPage();
            }
            if (ClientSession.UserRole != null && ClientSession.UserCurrentProcess != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && ClientSession.UserCurrentProcess == "PROVIDER_REVIEW_CORRECTION")
                btnCorrections.Visible = true;
            else if (ClientSession.UserRole != null && ClientSession.UserCurrentProcess != null && ClientSession.UserRole.ToUpper() == "SCRIBE" && (ClientSession.UserCurrentProcess == "SCRIBE_CORRECTION" || ClientSession.UserCurrentProcess == "SCRIBE_REVIEW_CORRECTION"))
                btnCorrections.Visible = true;
            else
                btnCorrections.Visible = false;
            if (ClientSession.UserRole != null && ClientSession.UserCurrentProcess != null && ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" && ClientSession.UserCurrentProcess == "MA_PROCESS")
            {

                XmlDocument xmldocUser = new XmlDocument();
                if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "CapellaScribeLookupList" + ".xml"))
                    xmldocUser.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "CapellaScribeLookupList" + ".xml");



                XmlNode nodeMatchingFacility = xmldocUser.SelectSingleNode("/CapellaScribeLookupList/CapellaScribeLookup[@ProviderID='" + ClientSession.FillEncounterandWFObject.EncRecord.Appointment_Provider_ID.ToString() + "' and @Legal_Org='" + ClientSession.LegalOrg + "']");
                if (nodeMatchingFacility != null)

                    btnmovetoscribe.Style.Add("display", "block");//.Visible = true;
                else

                    btnmovetoscribe.Style.Add("display", "none");


            }






            // hdnIsACOValid.Value = (objHumanManagerACO.IsACOValid(ClientSession.HumanId) || ClientSession.bPFSHVerified == false).ToString();

            //if (Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"] == "btnPhysiciancorrection")
            //{
            //    btnPhysiciancorrection_Click(new object(), new EventArgs());
            //}

        }

        HumanManager objHumanManagerACO = new HumanManager();
        bool LoadPage()
        {
            bool issuccess = true;
            if (ClientSession.FillEncounterandWFObject.EncounterWFRecord != null && (sMyObjType == "ENCOUNTER" || sMyObjType == "ADDENDUM"))
            {
                ehrwfobj = ClientSession.FillEncounterandWFObject.EncounterWFRecord;// MyEncWFRecord.EncounterWFRecord;
            }
            else if (ClientSession.FillEncounterandWFObject.DocumentationWFRecord != null && sMyObjType == "DOCUMENTATION")
            {
                ehrwfobj = ClientSession.FillEncounterandWFObject.DocumentationWFRecord;// MyEncWFRecord.DocumentationWFRecord;
            }
            //else if (ClientSession.FillEncounterandWFObject.DocReviewWFRecord != null && sMyObjType == "DOCUMENT REVIEW")
            //{
            //    ehrwfobj = ClientSession.FillEncounterandWFObject.DocReviewWFRecord;
            //}
            settingThePFSHFlag(EncRecord);

            int selectednode = Convert.ToInt32(ViewState["Node"]);
            iMySelectedNode = selectednode;

            SecurityServiceUtility sec = new SecurityServiceUtility();

            if (iMySelectedNode < ClientSession.PatientPaneList.Count)
                sMyPhyUserName = ClientSession.PatientPaneList[iMySelectedNode].Assigned_Physician_User_Name;

            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                strPhysicianDetails = ClientSession.PatientPaneList[iMySelectedNode].Assigned_Physician;
                sHumanName = ClientSession.PatientPaneList[iMySelectedNode].Last_Name + "," + ClientSession.PatientPaneList[iMySelectedNode].First_Name + " " + ClientSession.PatientPaneList[iMySelectedNode].MI + " " + ClientSession.PatientPaneList[iMySelectedNode].Suffix;
            }
            if (EncRecord.Date_of_Service != null)
            {
                sDateofService = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service);
            }
           
            pnlBarGroupTabs.InnerText = EncRecord.Facility_Name + "   |   " + sDateofService.ToString("dd-MMM-yyyy hh:mm tt") + "   |   " + EncRecord.Visit_Type + "   |   " + strPhysicianDetails + "   |   " + EncRecord.Assigned_Med_Asst_User_Name + "   |   " + EncRecord.Assigned_Scribe_User_Name; // +facList[0].POS_Description + "   |   ";

        // UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "OpeningEncounterXMLfromMyQ");

        //string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
        //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
        ln:
            XmlTextReader XmlText = null;

            //if (File.Exists(strXmlFilePath) == true)
            EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
            Encounter_Blob objEncounterblob = null;
            string sXMLContent = string.Empty;
            IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ClientSession.EncounterId);
            if (ilstEncounterBlob.Count > 0 && ClientSession.EncounterId > 0)
            {
                objEncounterblob = ilstEncounterBlob[0];

                try
                {
                    XmlDocument itemDoc = new XmlDocument();

                    sXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                    if (sXMLContent.Substring(0, 1) != "<")
                        sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                    itemDoc.LoadXml(sXMLContent);

                    //XmlText = new XmlTextReader(strXmlFilePath);
                    //itemDoc.Load(XmlText);
                    //XmlText.Close();
                    XmlNodeList xmlMember_ID = itemDoc.GetElementsByTagName("Encounter_Provider_Name");
                    if (xmlMember_ID != null && xmlMember_ID.Count > 0)
                        xmlMember_ID[0].InnerText = strPhysicianDetails;

                    string sPhysicianid = string.Empty;
                    if (ClientSession.PhysicianId != null)
                        sPhysicianid = ClientSession.PhysicianId.ToString();
                    XmlNodeList xmlPhysicianAddress = itemDoc.GetElementsByTagName("Physician_Address");
                    if (xmlPhysicianAddress != null)
                    {
                        string sPhysicianXmlPath = string.Empty;
                        if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml"))
                            sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
                        XmlDocument itemPhysiciandoc = new XmlDocument();
                        XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
                        itemPhysiciandoc.Load(XmlPhysicianText);

                        XmlNodeList xmlphy = itemPhysiciandoc.GetElementsByTagName("p" + sPhysicianid);
                        if (xmlphy != null && xmlphy.Count > 0)
                        {
                            if (xmlphy[0].Attributes[0].Value != null)
                                xmlPhysicianAddress[0].Attributes[0].Value = xmlphy[0].Attributes[0].Value;
                            if (xmlphy[0].Attributes[1].Value != null)
                                xmlPhysicianAddress[0].Attributes[1].Value = xmlphy[0].Attributes[1].Value;
                            if (xmlphy[0].Attributes[2].Value != null)
                                xmlPhysicianAddress[0].Attributes[2].Value = xmlphy[0].Attributes[2].Value;
                            if (xmlphy[0].Attributes[3].Value != null)
                                xmlPhysicianAddress[0].Attributes[3].Value = xmlphy[0].Attributes[3].Value;
                            if (xmlphy[0].Attributes[4].Value != null)
                                xmlPhysicianAddress[0].Attributes[4].Value = xmlphy[0].Attributes[4].Value;
                            if (xmlphy[0].Attributes[5].Value != null)
                                xmlPhysicianAddress[0].Attributes[5].Value = xmlphy[0].Attributes[5].Value;
                            if (xmlphy[0].Attributes[6].Value != null)
                                xmlPhysicianAddress[0].Attributes[6].Value = xmlphy[0].Attributes[6].Value;
                            if (xmlphy[0].Attributes[7].Value != null)
                                xmlPhysicianAddress[0].Attributes[7].Value = xmlphy[0].Attributes[7].Value;
                        }
                    }
                    //itemDoc.Save(strXmlFilePath);
                    IList<Encounter_Blob> ilstUpdateBlob = new List<Encounter_Blob>();
                    byte[] bytes = null;
                    try
                    {
                        bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                    }
                    catch (Exception ex)
                    {

                    }
                    objEncounterblob.Encounter_XML = bytes;
                    ilstUpdateBlob.Add(objEncounterblob);
                    EncounterBlobMngr.SaveEncounterBlobWithTransaction(ilstUpdateBlob, string.Empty);
                }
                catch (Exception ex)
                {
                    // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                    {
                        // ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('1011190');", true);

                        issuccess = false;
                        //XmlText.Close();
                        // UtilityManager.GenerateXML(ClientSession.EncounterId.ToString(), "Encounter");
                        // goto ln;
                        return issuccess;
                    }

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "SummaryAlert", "SummaryXMlAlert();", true);
            }

            if (EncRecord.Date_of_Service != null)
            {
                sDateofService = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service);
            }
            if (ClientSession.SummaryList != null && !ClientSession.SummaryList.Contains(strPhysicianDetails))
            {
                ClientSession.SummaryList = ClientSession.SummaryList + " | DOS:" + sDateofService.ToString("dd-MMM-yyyy hh:mm tt") + " | " + strPhysicianDetails;
            }
            //FillPhysicianUser PhyUserList;
            cboPhysicianName.Items.Clear();

            //if (ClientSession.FillPatientChart.PatChartList != null && ClientSession.FillPatientChart.PatChartList.Count > 0 && ClientSession.FillPatientChart.PatChartList[0].ACO_Validation == "Y" && ClientSession.UserRole.ToUpper() != "PHYSICIAN ASSISTANT")
            //{
            //    chkACOValidation.Visible = true;
            //    chkACOValidation.Checked = true;
            //}
            //else
            //{
            chkACOValidation.Visible = false;
            //}


            if (ehrwfobj != null)
            {
                if (ehrwfobj.Current_Process.ToUpper() == "SCRIBE_PROCESS" || ehrwfobj.Current_Process.ToUpper() == "SCRIBE_CORRECTION" || ehrwfobj.Current_Process.ToUpper() == "SCRIBE_REVIEW_CORRECTION")
                {
                    if (ehrwfobj.Current_Owner.ToUpper() == ClientSession.UserName.ToUpper())
                    {
                        //if (EncRecord.Is_Moved_to_MA != "Y") //Commentted by saravanakumar for bug id :36811 
                        //{
                        btnMove.Visible = true;
                        cboPhysicianName.Visible = false;
                        chkShowAllPhysicians.Visible = false;

                        if (ehrwfobj.Current_Process.ToUpper() == "SCRIBE_PROCESS")
                        {
                            btnPhysiciancorrection.Visible = true;
                            btnPhysiciancorrection.Disabled = true;
                            btnMoveToMA.Visible = true;
                            // btnCopyPreviousEncounter.Disabled = false;
                        }
                        else
                        {
                            btnPhysiciancorrection.Visible = false;
                            btnMoveToMA.Visible = false;
                            //  btnCopyPreviousEncounter.Disabled = true;
                        }
                        //}
                        //else
                        //{
                        //    btnMove.Visible = true;
                        //    cboPhysicianName.Visible = false;
                        //    chkShowAllPhysicians.Visible = false;
                        //    btnPhysiciancorrection.Visible = true;
                        //}

                    }
                    else
                    {
                        btnMove.Visible = false;
                        cboPhysicianName.Visible = false;
                        //lblPhysician.Visible = false;
                        chkShowAllPhysicians.Visible = false;
                        btnPhysiciancorrection.Visible = false;
                        btnMoveToMA.Visible = false;
                    }

                    btnMove.Value = "Move to Provider";
                    btnPhysiciancorrection.Value = "Move To Checkout";
                    FillCboPhysician(EncRecord.Facility_Name);
                }
                //Jira #CAP-707 - strat
                else if (ehrwfobj.Current_Process.ToUpper() == "AKIDO_SCRIBE_PROCESS")
                {
                    if (ehrwfobj.Current_Owner.ToUpper() == ClientSession.UserName.ToUpper())
                    {
                        btnMove.Visible = true;
                        btnMove.Value = "Move to Next Process";
                        btnPhysiciancorrection.Value = "Move To Checkout";
                        //FillCboPhysician(EncRecord.Facility_Name);
                        btnPhysiciancorrection.Visible = true;
                        btnPhysiciancorrection.Disabled = true;
                        btnMoveToMA.Visible = false;
                        cboPhysicianName.Visible = false;
                        chkShowAllPhysicians.Visible = false;
                    }

                }
                //Jira #CAP-707 - end
                else if (ehrwfobj.Current_Process.ToUpper() == "MA_PROCESS")
                {
                    if (ehrwfobj.Current_Owner.ToUpper() == ClientSession.UserName.ToUpper())
                    {
                        //if (EncRecord.Is_Moved_to_MA != "Y") //Commentted by saravanakumar for bug id :36811 
                        //{
                        btnMove.Visible = true;
                        cboPhysicianName.Visible = true;
                        chkShowAllPhysicians.Visible = true;
                        btnPhysiciancorrection.Visible = true;
                        //}
                        //else
                        //{
                        //    btnMove.Visible = true;
                        //    cboPhysicianName.Visible = false;
                        //    chkShowAllPhysicians.Visible = false;
                        //    btnPhysiciancorrection.Visible = true;
                        //}

                    }
                    else
                    {
                        btnMove.Visible = false;
                        cboPhysicianName.Visible = false;
                        //lblPhysician.Visible = false;
                        chkShowAllPhysicians.Visible = false;
                        btnPhysiciancorrection.Visible = false;
                    }
                    btnMoveToMA.Visible = false;
                    btnMove.Value = "Move to Provider";
                    btnPhysiciancorrection.Value = "Move To Checkout";
                    FillCboPhysician(EncRecord.Facility_Name);
                }
                else if (ClientSession.UserRole != null && ehrwfobj.Current_Process.ToUpper() == "PROVIDER_PROCESS" && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                {
                    if (ehrwfobj.Current_Owner.ToUpper() == ClientSession.UserName.ToUpper())
                    {
                        btnMove.Visible = true;
                    }
                    else
                    {
                        btnMove.Visible = false;
                    }
                    btnMove.Value = "Move to Next Process";
                    cboPhysicianName.Visible = true;
                    chkShowAllPhysicians.Visible = true;
                    //lblPhysician.Visible = true;
                    btnMoveToMA.Visible = true;
                    btnPhysiciancorrection.Visible = true;
                    btnPhysiciancorrection.Value = "Move to Checkout";
                    FillCboPhysician(EncRecord.Facility_Name);
                }
                else if (ehrwfobj.Current_Process.ToUpper() == "PROVIDER_PROCESS" || ehrwfobj.Current_Process.ToUpper() == "PROVIDER_REVIEW" || ehrwfobj.Current_Process.ToUpper() == "PROVIDER_REVIEW_2" || ehrwfobj.Current_Process.ToUpper() == "DICTATION_REVIEW")//else if (ehrwfobj.Current_Process.ToUpper() == "PROVIDER_PROCESS" || ehrwfobj.Current_Process.ToUpper() == "PROVIDER_REVIEW") Comment By ThiyagarajanM 13-06-2013
                {
                    //Jira CAP-1743
                    if (ehrwfobj.Current_Process.ToUpper() == "PROVIDER_REVIEW" || ehrwfobj.Current_Process.ToUpper() == "PROVIDER_REVIEW_2" || ehrwfobj.Current_Process.ToUpper() == "DICTATION_REVIEW")
                    {
                        btnCopyPreviousEncounter.Disabled = true;
                    }

                    if (ehrwfobj.Current_Process.ToUpper() == "PROVIDER_PROCESS")
                    {
                        btnMoveToMA.Visible = true;
                    }
                    cboPhysicianName.Visible = false;
                    //lblPhysician.Visible = false;
                    chkShowAllPhysicians.Visible = false;
                    btnPhysiciancorrection.Visible = true;
                    btnMove.Value = "Move to Next Process";
                    btnPhysiciancorrection.Value = "Move to Checkout";
                }

                else if (ehrwfobj.Current_Process.ToUpper() == "TECHNICIAN_PROCESS")//CMG Ancilliary
                {
                    btnMove.Visible = true;
                    btnMove.Value = "Move to Next Process";
                    cboPhysicianName.Visible = false;
                    chkShowAllPhysicians.Visible = false;
                    btnPhysiciancorrection.Visible = false;
                    btnCopyPreviousEncounter.Disabled = true;

                }
                //Jira #CAP-707
                //else if (ehrwfobj.Current_Process.ToUpper() == "PROVIDER_REVIEW_CORRECTION" || ehrwfobj.Current_Process.ToUpper() == "CODER_REVIEW_CORRECTION" || ehrwfobj.Current_Process.ToUpper() == "REVIEW_CODING_2" || ehrwfobj.Current_Process.ToUpper() == "REVIEW_CODING")
                //{
                else if (ehrwfobj.Current_Process.ToUpper() == "PROVIDER_REVIEW_CORRECTION" || ehrwfobj.Current_Process.ToUpper() == "CODER_REVIEW_CORRECTION" || ehrwfobj.Current_Process.ToUpper() == "REVIEW_CODING_2" || ehrwfobj.Current_Process.ToUpper() == "REVIEW_CODING" || ehrwfobj.Current_Process.ToUpper() == "AKIDO_REVIEW_CODING")
                {
                    if (ehrwfobj.Current_Owner.ToUpper() == ClientSession.UserName.ToUpper())
                    {
                        btnPhysiciancorrection.Visible = true;
                    }
                    else
                    {
                        btnPhysiciancorrection.Visible = false;
                    }
                    btnMove.Visible = false;
                    cboPhysicianName.Visible = false;
                    btnPhysiciancorrection.Value = "Move to Next Process";
                    //lblPhysician.Visible = false;
                    chkShowAllPhysicians.Visible = false;
                    btnCopyPreviousEncounter.Disabled = true;
                }
                else
                {
                    cboPhysicianName.Visible = false;
                    // lblPhysician.Visible = false;
                    chkShowAllPhysicians.Visible = false;
                    btnPhysiciancorrection.Visible = false;
                    btnMove.Visible = false;
                    btnCopyPreviousEncounter.Disabled = true;
                }

                if (ClientSession.EncounterId != 0 && ClientSession.SelectedFrom != "Menu" && ClientSession.EncounterId != ClientSession.Selectedencounterid)
                {
                    cboPhysicianName.Visible = false;
                    //lblPhysician.Visible = false;
                    chkShowAllPhysicians.Visible = false;
                    btnPhysiciancorrection.Visible = false;
                    btnMove.Visible = false;
                    btnMoveToMA.Visible = false;
                    btnCopyPreviousEncounter.Visible = false;
                    //CAP-790
                    if (hdnTab != null)
                        hdnTab.Value = "tbSummary";
                    hdnEncounterIDSummary.Value = Convert.ToString(ClientSession.EncounterId);
                    // pageViewEncounter.ContentUrl = "frmSummary.aspx?HumanID=" + ClientSession.HumanId.ToString() + "&EncounterID=" + ClientSession.EncounterId.ToString() + "&PhyID=" + ClientSession.PhysicianId.ToString() + "&Date=" + hdnLocalTime.Value;
                    //pageViewEncounter.ContentUrl = "frmSummaryNew.aspx?";
                    //pageViewEncounter.Height = Unit.Percentage(95);
                    //pageContainer.SelectedIndex = tabStripEncounter.SelectedIndex = pageViewEncounter.Index;
                    //pageViewEncounter.Selected = true;
                    //tabStripEncounter.SelectedIndex = 12;
                }
                else if (ehrwfobj.Current_Process.ToUpper() != "DICTATION_REVIEW")
                {
                    FillMyNewEncounter();
                }

                WFObject TempWFRecord = ClientSession.FillEncounterandWFObject.EncounterWFRecord;
                //Jira #CAP-707
                //if ((TempWFRecord.Current_Process.ToUpper() == "CHECK_OUT" || TempWFRecord.Current_Process.ToUpper() == "CHECK_OUT_COMPLETE") && ehrwfobj.Current_Process.ToUpper() != "CODER_REVIEW_CORRECTION" && ehrwfobj.Current_Process.ToUpper() != "PROVIDER_REVIEW_CORRECTION" && ehrwfobj.Current_Process.ToUpper() != "REVIEW_CODING_2" && ehrwfobj.Current_Process.ToUpper() != "REVIEW_CODING")
                //{
                if ((TempWFRecord.Current_Process.ToUpper() == "CHECK_OUT" || TempWFRecord.Current_Process.ToUpper() == "CHECK_OUT_COMPLETE") && ehrwfobj.Current_Process.ToUpper() != "CODER_REVIEW_CORRECTION" && ehrwfobj.Current_Process.ToUpper() != "PROVIDER_REVIEW_CORRECTION" && ehrwfobj.Current_Process.ToUpper() != "REVIEW_CODING_2" && ehrwfobj.Current_Process.ToUpper() != "REVIEW_CODING" && ehrwfobj.Current_Process.ToUpper() != "AKIDO_REVIEW_CODING")
                {
                    btnPhysiciancorrection.Disabled = true;
                    ViewState["Chkoutbtn"] = false;
                }


                RadToolTip Templatetooltip = new RadToolTip();
                Templatetooltip.TargetControlID = "btnTemplate";
                Templatetooltip.Text = "Template";
                //UIManager.Template_Name = string.Empty;
                //UIManager.Template_Id = 0;
            }
            return issuccess;
        }
        public void hdnbtngeneratexml_Click(object sender, EventArgs e)
        {
            if (ClientSession.FillEncounterandWFObject != null && ClientSession.FillEncounterandWFObject.EncRecord != null)
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Summary", "reloadSummaryBar('" + ClientSession.EncounterId + "','" + ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


            //  Patientchartload();
        }
        private void settingThePFSHFlag(Encounter currentEncounter)
        {
            //if (currentEncounter.Is_PFSH_Verified == "Y")
            //{
            //    UIManager.flagPFSHVerified = true;
            //}
            //else
            //{
            //    UIManager.flagPFSHVerified = false;
            //}
        }
        private void FillMyNewEncounter()
        {
            //if (tabStripEncounter.SelectedIndex == 0)
            //{
            //    pageViewChiefComplain.ContentUrl = "~/frmCCPhrase.aspx";
            //    pageContainer.SelectedIndex = tabStripEncounter.SelectedIndex = pageViewChiefComplain.Index;
            //}
        }

        protected void btnmovetoscribe_click(object sender, EventArgs e)
        {
            MoveVerificationDTO objMoveVerifyDTO = null;
            ulong EncProviderID = 0;
            if (cboPhysicianName.Visible == true)
            {
                //if (cboPhysicianName.Text == string.Empty || cboPhysicianName.Text == "\r\n")
                ulong uPhyID;
                if (hdnLocalPhy.Value != null && hdnLocalPhy.Value == string.Empty && !ulong.TryParse(hdnLocalPhy.Value, out uPhyID))
                {
                    if (ClientSession.UserRole != null && ClientSession.UserRole == "Physician Assistant" && ehrwfobj.Current_Process == "PROVIDER_PROCESS")
                    {
                        //if (bMovetoReview == true)
                        //{
                        //    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        //    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180027','','');", true);
                        //    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                        //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180027','','');", true);
                        //    cboPhysicianName.Focus();
                        //    return;
                        //}

                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180027','','');", true);
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180027','','');", true);
                        cboPhysicianName.Focus();
                        return;
                    }
                }
                if (hdnLocalPhy.Value != null && hdnLocalPhy.Value != string.Empty)
                {
                    EncProviderID = Convert.ToUInt32(hdnLocalPhy.Value.Split('~')[0]);
                    ClientSession.PhysicianUserName = hdnLocalPhy.Value.Split('~')[1];
                }
            }
            else
            {
                if (EncRecord == null)
                    EncRecord = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord;
                if (hdnLocalPhy.Value != null && EncRecord.Is_Moved_to_MA == "Y" && hdnLocalPhy.Value != string.Empty)
                {
                    EncProviderID = Convert.ToUInt32(hdnLocalPhy.Value.Split('~')[0]);
                    ClientSession.PhysicianUserName = hdnLocalPhy.Value.Split('~')[1];
                }
                else
                {
                    //EncProviderID = 0;//Bug Id 53137                   
                    EncProviderID = Convert.ToUInt32(EncRecord.Encounter_Provider_ID);
                }
            }
            bool bMovetoReview = true;
            if (chkProviderReview != null)
            {
                if (chkProviderReview.Checked == true)
                    bMovetoReview = true;
                else
                    bMovetoReview = false;
            }
            DateTime dtLocalTime;
            if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty)
                dtLocalTime = Convert.ToDateTime(hdnLocalTime.Value);
            else
                dtLocalTime = DateTime.UtcNow;
            bool VerifyPFSH = false;
            bool flag = false;
            string Source = string.Empty, If_Source_Of_Information_Others = string.Empty;
            string IsPatientDiscussed = "";
            //objMoveVerifyDTO = objEncounterManager.PerformMoveVerification(ClientSession.EncounterId, EncProviderID, ClientSession.HumanId, UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)), ClientSession.FacilityName, ClientSession.UserName, VerifyPFSH, Source, If_Source_Of_Information_Others, ClientSession.UserCurrentProcess, string.Empty, btnMove.Text, bDuplicateCheck, ClientSession.UserRole);
            if (chkACOValidation.Checked == true)
                IsPatientDiscussed = "Y";
            else
                IsPatientDiscussed = "N";


            ulong IsDiscussedBy = 0;
            if (Request["cboPhysicianName"] != "" && Request["cboPhysicianName"] != null)
                IsDiscussedBy = Convert.ToUInt32(Request["cboPhysicianName"]);
            bool bAcoUpdate = false;
            if (chkACOValidation.Visible == true)
                bAcoUpdate = true;
            string sRole = "";
            string sFacilityName = "";
            XmlDocument xmldocUser = new XmlDocument();
            if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml"))
                xmldocUser.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml");
            XmlNodeList xmlUserList = xmldocUser.GetElementsByTagName("User");

            if (xmlUserList != null)
            {
                foreach (XmlNode item in xmlUserList)
                    if (ClientSession.PhysicianUserName.ToString().Trim().ToUpper() == item.Attributes.GetNamedItem("User_Name").Value.ToUpper().Trim())
                    {
                        if (ClientSession.PhysicianUserName.ToString() == item.Attributes[0].Value.ToUpper())
                        {
                            //sFacilityName = item.Attributes[1].Value;
                            //sRole = item.Attributes[3].Value;
                            if (item.Attributes.GetNamedItem("Default_Facility").Value != null)
                                sFacilityName = item.Attributes.GetNamedItem("Default_Facility").Value;
                            if (item.Attributes.GetNamedItem("Role").Value != null)
                                sRole = item.Attributes.GetNamedItem("Role").Value;
                            break;
                        }
                    }
            }
            if (ClientSession.FillEncounterandWFObject != null)
            {
                objMoveVerifyDTO = objEncounterManager.PerformMovetoProvider(ClientSession.EncounterId, EncProviderID, ClientSession.HumanId, UtilityManager.ConvertToLocal(dtLocalTime), ClientSession.FacilityName, ClientSession.UserName, VerifyPFSH, Source, If_Source_Of_Information_Others, ClientSession.UserCurrentProcess, string.Empty, btnMove.Value, bDuplicateCheck, ClientSession.UserRole, "btnMove", bMovetoReview, hdnACOValidated.Value, ClientSession.FillEncounterandWFObject.EncounterWFRecord, ClientSession.FillEncounterandWFObject.EncRecord, IsPatientDiscussed, IsDiscussedBy, bAcoUpdate, sRole, ClientSession.PhysicianUserName, "SCRIBE");
            }

            if (ClientSession.HumanId != null)
                RemoveWindowItem(ClientSession.HumanId);
            Response.Write("<script> window.top.location.href=\" frmMyQueueNew.aspx\"; </script>");
            bFormClose = false;
            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}RefreshNotification('CalculateALL');", true);//BugID:47852

        }
        protected void btnMoveToMA_Click(object sender, EventArgs e)
        {

            //bool VerifyPFSH = false;
            string Source = string.Empty, If_Source_Of_Information_Others = string.Empty;
            if (ClientSession.bPFSHVerified || UIManager.IsPFSHVerified)
            {
                //if (hdnIsACOValid.Value == "True")
                if (hdnACOValidated.Value == "False")
                {
                    if (objHumanManagerACO.IsACOValid(ClientSession.HumanId))
                    {

                        MessageWindow.NavigateUrl = "frmACOValidation.aspx?IsPFSHVald=Y&btnName=" + btnMoveToMA.Value;
                        MessageWindow.Width = Unit.Pixel(550);
                        MessageWindow.Height = Unit.Pixel(250);

                        MessageWindow.OnClientClose = "OnClientCloseMA";
                        MessageWindow.Visible = true;
                        MessageWindow.VisibleStatusbar = false;
                        MessageWindow.VisibleOnPageLoad = true;
                        return;
                    }
                }
            }

            Encounter EncRecord = objEncounterManager.GetEncounterByEncounterID(ClientSession.EncounterId)[0];
            string sOwner = string.Empty;
            if (EncRecord.Assigned_Med_Asst_User_Name.Trim() == string.Empty)
            {
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('210021'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //return;
                sOwner = "UNKNOWN";
            }
            else
                sOwner = EncRecord.Assigned_Med_Asst_User_Name;

            //var AssignedMedicalAssistant = from c in ClientSession.PatientPaneList where c.Encounter_ID == ehrwfobj.Obj_System_Id select c;
            //IList<PatientPane> PatientPaneList = AssignedMedicalAssistant.ToList<PatientPane>();

            // Encounter EncRecord = ClientSession.FillEncounterandWFObject.EncRecord;

            if (EncRecord.is_serviceprocedure_saved.ToUpper() == "N" && System.Configuration.ConfigurationSettings.AppSettings["IsReviewAlert"].ToString().ToUpper() == "Y")
            {
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "alert('Please review and save the changes in  Service Procedure Tab');", true);
                //CAP-790
                if (hdnTab != null)
                    hdnTab.Value = "tbEandM";

                return;
            }
            else
            {
                EncRecord.Is_Moved_to_MA = "Y";
                //objEncounterManager.UpdateEncounterList(EncRecord, string.Empty); //Commentted for Bug : 56886
                //objWFObjectManager.DeleteDocumentation(ehrwfobj.Obj_System_Id, ehrwfobj.Obj_Type, sOwner, string.Empty);
                ////For bug Id  43191
                //objWFObjectManager.MoveToNextProcess(ehrwfobj.Obj_System_Id, ehrwfobj.Obj_Type, 4, "UNKNOWN", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), ehrwfobj.Current_Process, null, null);
                objEncounterManager.UpdateDeleteDocumentation(EncRecord, string.Empty, ehrwfobj, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Today), sOwner);
                btnMoveToMA.Disabled = true;
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('210009');", true);
                RemoveWindowItem(ClientSession.HumanId);
                Response.Write("<script> window.top.location.href=\"frmMyQueueNew.aspx\"; </script>");
            }
        }

        protected void tabStripEncounter_TabClick(object sender, RadTabStripEventArgs e)
        {
            ClientSession.SetSelectedTab = string.Empty;
            ClientSession.SetSelectedTab = e.Tab.Text.ToUpper();
            if (hdnCloseFS.Value == "false")
            {
                RadWindow2.Visible = false;
                RadWindow2.VisibleOnPageLoad = false;
                RadWindowManager2.Visible = false;
                RadWindowManager2.VisibleOnPageLoad = false;
            }

            if (hdnCopyPreviousClick.Value == "true")
            {
                RadWindowManager1.VisibleOnPageLoad = true;
                RadWindowManager1.VisibleStatusbar = false;
                RadWindowManager1.VisibleTitlebar = true;


                RadFollowUpEncounter.VisibleOnPageLoad = true;
                RadFollowUpEncounter.VisibleStatusbar = false;
                RadFollowUpEncounter.VisibleTitlebar = true;

            }
            else
            {
                RadWindowManager1.VisibleOnPageLoad = false;
                RadWindowManager1.VisibleStatusbar = false;
                RadWindowManager1.VisibleTitlebar = false;
                RadWindow2.InitialBehaviors = WindowBehaviors.Minimize;
                RadFollowUpEncounter.VisibleOnPageLoad = false;
                RadFollowUpEncounter.VisibleStatusbar = false;
                RadFollowUpEncounter.VisibleTitlebar = false;
            }

            string sDate = hdnLocalTime.Value;

            sMyPreviousTab.Value = sMyNextTab.Value;

            if (e.Tab.Text.ToUpper() == "CC / HPI")
            {
                sMyNextTab.Value = e.Tab.Text.ToString();
                //pageViewEncounter.ContentUrl = "frmCCPhrase.aspx";
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                if (sMyPreviousTab != sMyNextTab)
                {
                    CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
                }
            }
            else if (e.Tab.Text.ToUpper() == "SCREENING")
            {
                sMyNextTab.Value = e.Tab.Text.ToString();
                //pageViewEncounter.ContentUrl = "frmQuestionnaireTab.aspx";
                //pageViewEncounter.Selected = true;
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "PFSH")
            {
                //Srividhya added the below condition on 3-Jul-2014
                string Openingfrom = string.Empty;
                if (UIManager.is_Menu_Level_PFSH == true)
                {
                    Openingfrom = "Menu";
                }
                else
                {
                    Openingfrom = "Queue";
                }

                //newly added
                UIManager.is_Menu_Level_PFSH = false;

                sMyNextTab.Value = e.Tab.Text.ToString();
                //pageViewEncounter.ContentUrl = "frmPFSH.aspx?openingfrom=" + Openingfrom;
                //pageViewEncounter.Selected = true;
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "ROS")
            {
                ClientSession.FlushSession();
                Cache.Remove("RosTable");
                sMyNextTab.Value = e.Tab.Text.ToString();
                //pageViewEncounter.ContentUrl = "frmRos.aspx";
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;

                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "VITALS")
            {
                ClientSession.FlushSession();
                //Cache.Remove("VitalsTab");
                sMyNextTab.Value = e.Tab.Text.ToString();
                //string Openingfrom = "Queue";
                //pageViewEncounter.ContentUrl = "frmVitals.aspx?Openingfrom=" + Openingfrom + "&Date=" + sDate;
                //pageViewEncounter.Selected = true;
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "EXAM")
            {
                sMyNextTab.Value = e.Tab.Text.ToString();

                //pageViewEncounter.ContentUrl = "frmExamTab.aspx";
                //pageViewEncounter.Selected = true;
                //tabStripEncounter.SelectedIndex = e.Tab.Index;

                //pageViewEncounter.ContentUrl = "frmExamTab.aspx";

                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "ASSESSMENT")
            {
                sMyNextTab.Value = e.Tab.Text.ToString();
                //  pageViewEncounter.ContentUrl = "htmlAssessment.html";

                // pageViewEncounter.ContentUrl = "frmAssessmentTab.aspx";
                //pageViewAssessment.Selected = true;
                //pageViewAssessment.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;

                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "ERX")
            {
                sMyNextTab.Value = e.Tab.Text.ToString();
                //if (ClientSession.UserCurrentProcess.ToUpper() == "PROVIDER_PROCESS")
                //{
                //    pageViewEncounter.ContentUrl = "frmRCopiaWebBrowser.aspx?MyType=GENERAL&IsMoveButton=false&IsMoveCheckbox=true&IsPrescriptiontobePushed=" + ClientSession.FillEncounterandWFObject.EncRecord.Is_Prescription_To_Be_Moved + "&bThroughEHRMenu=false&IsSentToRCopia=" + Request["sMySentToRCopia"] + "&LocalTime=" + hdnLocalTime.Value.ToString();
                //}
                //else
                //{
                //    pageViewEncounter.ContentUrl = "frmRCopiaWebBrowser.aspx?MyType=GENERAL&IsMoveButton=false&IsMoveCheckbox=false&IsPrescriptiontobePushed=" + ClientSession.FillEncounterandWFObject.EncRecord.Is_Prescription_To_Be_Moved + "&bThroughEHRMenu=false&IsSentToRCopia=" + Request["sMySentToRCopia"] + "&LocalTime=" + hdnLocalTime.Value.ToString();
                //}
                //pageViewEncounter.Selected = true;
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "PLAN")
            {
                sMyNextTab.Value = e.Tab.Text.ToString();
                //pageViewEncounter.ContentUrl = "frmPlanTab.aspx";
                //pageViewEncounter.ContentUrl = "HtmlPlanTab.html";
                //pageViewEncounter.Selected = true;
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "SERV./PROC. CODES")
            {
                sMyNextTab.Value = e.Tab.Text.ToString();
                //pageViewEncounter.ContentUrl = "frmEandMcoding.aspx";
                //pageViewEncounter.Selected = true;
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "SUMMARY")
            {
                sDate = hdnLocalTime.Value;
                sMyNextTab.Value = e.Tab.Text.ToString();

                //pageViewEncounter.ContentUrl = "frmSummary.aspx?Date=" + sDate;
                //pageViewEncounter.ContentUrl = "frmSummaryNew.aspx?";
                //pageViewEncounter.Height = Unit.Percentage(95);
                //pageViewEncounter.Selected = true;
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }
            else if (e.Tab.Text.ToUpper() == "TEST")
            {
                sMyNextTab.Value = e.Tab.Text.ToString();
                //pageViewEncounter.ContentUrl = "frmTestTab.aspx";
                //pageViewEncounter.Selected = true;               
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);

            }
            else if (e.Tab.Text.ToUpper() == "ORDERS")
            {
                sMyNextTab.Value = e.Tab.Text.ToString();
                //pageViewEncounter.ContentUrl = "frmOrders.aspx?HumanID=" + ClientSession.HumanId + "&EncounterID=" + ClientSession.EncounterId + "&PhysicianID=" + ClientSession.PhysicianId;
                //pageViewEncounter.Selected = true;
                //pageViewEncounter.Height = Unit.Percentage(100);
                //tabStripEncounter.SelectedIndex = e.Tab.Index;
                CheckTabSave(sMyPreviousTab.Value.ToString(), sMyNextTab.Value);
            }

        }
        [WebMethod(EnableSession = true)]
        public static string DownloadRcoipa()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return string.Empty;
            }
            string sErrorMessage = string.Empty;
            Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
            DateTime dtClientDate = localTime;
            if (ClientSession.UserName != null && ClientSession.FacilityName != null && ClientSession.EncounterId != null)
                //Commented the Patient Level RCopia Download
                sErrorMessage = objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, ClientSession.UserName, string.Empty, dtClientDate, ClientSession.FacilityName, ClientSession.EncounterId, ClientSession.HumanId, ClientSession.LegalOrg);
            //Old 
            //objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, ClientSession.UserName, string.Empty, dtClientDate, ClientSession.FacilityName, ClientSession.EncounterId);

            return JsonConvert.SerializeObject(sErrorMessage);
        }
        //Jira CAP-1567
        [WebMethod(EnableSession = true)]
        public static string DownloadRcoipaOutSidePatientChart(string sHuman_id)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return string.Empty;
            }
            string sErrorMessage = string.Empty;
            if (sHuman_id != null && sHuman_id != "")
            {
                ulong ulHuman_id = Convert.ToUInt64(sHuman_id);
                Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
                DateTime dtClientDate = DateTime.UtcNow;
                if (ClientSession.UserName != null && ClientSession.FacilityName != null && ClientSession.EncounterId != null)
                    //Commented the Patient Level RCopia Download
                    sErrorMessage = objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, ClientSession.UserName, string.Empty, dtClientDate, ClientSession.FacilityName, 0, ulHuman_id, ClientSession.LegalOrg);
                //Old 
                //objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, ClientSession.UserName, string.Empty, dtClientDate, ClientSession.FacilityName, ClientSession.EncounterId);
            }
            return JsonConvert.SerializeObject(sErrorMessage);
        }


        public Boolean CheckTabSave(string sTabName, string newTabName)
        {
            Boolean bResult = true;
            switch (sTabName)
            {
                case "eRx":
                    rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
                    DateTime dtClientDate = Convert.ToDateTime(hdnLocalTime.Value);
                    //objUpdateInfoMngr.DownloadRCopiaInfo(ClientSession.UserName, ApplicationObject.macAddress, dtClientDate, ClientSession.FacilityName, ClientSession.EncounterId);
                    //objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.UploadAddress, ClientSession.UserName, string.Empty, dtClientDate, ClientSession.FacilityName, ClientSession.EncounterId);
                    //Patient Level RCopia Download - Commented
                    objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.UploadAddress, ClientSession.UserName, string.Empty, dtClientDate, ClientSession.FacilityName, ClientSession.EncounterId, ClientSession.HumanId, ClientSession.LegalOrg);
                    //Old
                    //objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.UploadAddress, ClientSession.UserName, string.Empty, dtClientDate, ClientSession.FacilityName, ClientSession.EncounterId);
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "summaryMsg", "refreshSummaryBar('" + newTabName + "');", true);
                    break;
                case "QUESTIONNAIRE":
                    //if (pageViewEncounter == null)
                    //{
                    //    return true;
                    //}
                    //string str = ((RadPageView)pageViewEncounter.MultiPage.PageViews[pageViewEncounter.MultiPage.SelectedIndex]).Page.Title;
                    break;
            }
            return bResult;
        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            //bool stat = false;
            bool enableNotifAlert = false; ;
            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
            if (ClientSession.IsDirtySocialHistory)
            {
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('1091014','','');", true);
                return;
            }

            #region enableNotificationAlert
            if (ehrwfobj.Current_Process.ToUpper() == "SCRIBE_PROCESS" || ehrwfobj.Current_Process.ToUpper() == "SCRIBE_REVIEW_CORRECTION" || ehrwfobj.Current_Process.ToUpper() == "SCRIBE_CORRECTION" || ehrwfobj.Current_Process == "PROVIDER_PROCESS" || ehrwfobj.Current_Process == "CODER_REVIEW_CORRECTION" || ehrwfobj.Current_Process == "PROVIDER_REVIEW_CORRECTION" || ehrwfobj.Current_Process == "REVIEW_CODING" || ehrwfobj.Current_Process == "REVIEW_CODING_2" || ehrwfobj.Current_Process.Trim().ToUpper() == "DICTATION_REVIEW" || ehrwfobj.Current_Process == "TECHNICIAN_PROCESS")
                enableNotifAlert = true;
            #endregion

            #region NotificationCheck
            //For Bug ID 56265 , 56264 
            /*************Note:If any change in this notification part please do this same change in PhysicianCorrection button also *****************/
            if (enableNotifAlert)
            {
                IList<CDSRuleMaster> lstCDSRules = new List<CDSRuleMaster>();
                if (ClientSession.CDSNotificationRule != null && ClientSession.CDSNotificationRule.Count == 0)
                {
                    CDSRuleMasterManager ClinicalDecMgr = new CDSRuleMasterManager();
                    lstCDSRules = ClinicalDecMgr.GellCDSRuleMaster();
                    ClientSession.CDSNotificationRule = lstCDSRules;
                }
                else
                {
                    lstCDSRules = ClientSession.CDSNotificationRule;
                }
                //Get only active notification
                NotificationManager objNotification = new NotificationManager();
                IList<Notification> lstNotification = new List<Notification>();
                lstNotification = objNotification.GetNotificationForHumanIDandEncounterIDwithActiveSatus(ClientSession.HumanId, ClientSession.EncounterId);

                lstCDSRules = lstCDSRules.Where(a => a.Classification.Trim().ToUpper() == "MANDATORY ALERT").ToList<CDSRuleMaster>();
                bool isMandatoryYes = false;

                string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
                XmlDocument itemPhysiciandoc = new XmlDocument();
                XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
                itemPhysiciandoc.Load(XmlPhysicianText);
                string sPhysicianSpecialty = string.Empty;

                XmlNodeList xmlphy = itemPhysiciandoc.GetElementsByTagName("p" + ClientSession.FillEncounterandWFObject.EncRecord.Encounter_Provider_ID.ToString());
                if (xmlphy != null && xmlphy.Count > 0)
                {
                    sPhysicianSpecialty = xmlphy[0].Attributes[7].Value;
                }

                foreach (Notification item in lstNotification)
                {
                    IList<CDSRuleMaster> lst = lstCDSRules.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper() && a.Role_Privilege.ToUpper().Contains(ClientSession.UserCurrentProcess.ToUpper())).ToList<CDSRuleMaster>();
                    if (lst != null && lst.Count > 0)
                    {
                        //For Bug ID: 70525
                        if (lst[0].Physician_Specialty.Trim() != string.Empty)
                        {
                            string[] arySpeciality = sPhysicianSpecialty.Split(',');
                            var ary = lst[0].Physician_Specialty.Split(',').Intersect(arySpeciality);
                            foreach (string s in ary)
                            {
                                isMandatoryYes = true;
                                break;
                            }
                        }
                        else
                        {
                            isMandatoryYes = true;
                            break;
                        }
                        //isMandatoryYes = true;
                        //break;
                    }
                }
                if (isMandatoryYes == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Encounter), "NotificationAlert", "OpenNotification_Before_MovetoNextProcess();", true);
                    return;
                }

            }
            //For Bug ID 56265 , 56264 
            //if (enableNotifAlert && ClientSession.NotificationCount != null && ClientSession.NotificationCount.Trim() != "0" && (ClientSession.bIsMandatoryNotifPresent))
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(Encounter), "NotificationAlert", "OpenNotification_Before_MovetoNextProcess();", true);
            //    return;
            //}
            #endregion
            if (hdnPFSHInfoBy.Value != null && hdnPFSHInfoBy.Value.ToString() == "false")
                hdnPFSHInfoBy.Value = "Self";

            Session["SourceOfInformation"] = hdnPFSHInfoBy.Value;
            if (ClientSession.PatientPaneList != null && iMySelectedNode < ClientSession.PatientPaneList.Count)
                sHumanName = ClientSession.PatientPaneList[iMySelectedNode].Last_Name + "," + ClientSession.PatientPaneList[iMySelectedNode].First_Name + " " + ClientSession.PatientPaneList[iMySelectedNode].MI + " " + ClientSession.PatientPaneList[iMySelectedNode].Suffix;
            //bool result = false;
            MoveVerificationDTO objMoveVerifyDTO = null;
            ulong EncProviderID = 0;
            bool VerifyPFSH = false;
            bool flag = false;
            bIsWorkFlowButtonClicked = true;
            bool bMovetoReview = true;
            if (chkProviderReview != null)
            {
                if (chkProviderReview.Checked == true)
                    bMovetoReview = true;
                else
                    bMovetoReview = false;
            }
            string Source = string.Empty, If_Source_Of_Information_Others = string.Empty;

            #region Validation
            /*Commented as Move to Review Provider request is now taken from the value of checkbox Is_review_required in Encounter form. Part of the bugID: 51361
            if (ClientSession.UserRole == "Physician Assistant" && ehrwfobj.Current_Process == "PROVIDER_PROCESS")
            {
                //Need To Include Electronic Sign Check
               if (Convert.ToBoolean(ClientSession.bElectronicCheck) == false)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", " if (sessionStorage.getItem('CloseNotification') != undefined && sessionStorage.getItem('CloseNotification') == 'true'){DisplayErrorMessage('010015','','')};", true);
                    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('010015','','');", true);
                    ClientSession.SetSelectedTab = "PLAN";
                    hdnTab.Value = "tbPlan";
                    return;
                }
                if (ViewState["flag"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    MessageWindow.NavigateUrl = "frmValidationArea.aspx?Title=Message&ErrorMessages=" + "Do you want to send a copy to provider for review?";
                    MessageWindow.Width = Unit.Pixel(300);
                    MessageWindow.Height = Unit.Pixel(100);
                    MessageWindow.Title = "Message";
                    MessageWindow.ContentContainer.Height = Unit.Pixel(85);
                    MessageWindow.OnClientPageLoad = "OnClientPageLoadForCopyToProviderForReview";
                    MessageWindow.OnClientClose = "OnClientCloseForCopyToProviderForReview";
                    MessageWindow.VisibleStatusbar = false;
                    MessageWindow.Visible = true;
                    MessageWindow.VisibleOnPageLoad = true;
                    flag = true;
                    ViewState["flag"] = flag;
                    //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                    return;
                }
            }
           */
            if (cboPhysicianName.Visible == true)
            {
                //if (cboPhysicianName.Text == string.Empty || cboPhysicianName.Text == "\r\n")
                ulong uPhyID;
                if (hdnLocalPhy.Value != null && hdnLocalPhy.Value == string.Empty && !ulong.TryParse(hdnLocalPhy.Value, out uPhyID))
                {
                    if (ClientSession.UserRole != null && ClientSession.UserRole == "Physician Assistant" && ehrwfobj.Current_Process == "PROVIDER_PROCESS")
                    {
                        if (bMovetoReview == true)
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180027','','');", true);
                            //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180027','','');", true);
                            cboPhysicianName.Focus();
                            return;
                        }

                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180027','','');", true);
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180027','','');", true);
                        cboPhysicianName.Focus();
                        return;
                    }
                }
                if (hdnLocalPhy.Value != null && hdnLocalPhy.Value != string.Empty)
                {
                    EncProviderID = Convert.ToUInt32(hdnLocalPhy.Value.Split('~')[0]);
                    ClientSession.PhysicianUserName = hdnLocalPhy.Value.Split('~')[1];
                }
            }
            else
            {
                if (EncRecord == null)
                    EncRecord = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord;
                if (hdnLocalPhy.Value != null && EncRecord.Is_Moved_to_MA == "Y" && hdnLocalPhy.Value != string.Empty)
                {
                    EncProviderID = Convert.ToUInt32(hdnLocalPhy.Value.Split('~')[0]);
                    ClientSession.PhysicianUserName = hdnLocalPhy.Value.Split('~')[1];
                }
                else
                {
                    //EncProviderID = 0;//Bug Id 53137                   
                    EncProviderID = Convert.ToUInt32(EncRecord.Encounter_Provider_ID);
                }
            }

            #endregion
            objEncounterManager = new EncounterManager();
            //perfomance changes
            /* if (ehrwfobj != null && ehrwfobj.Current_Process == "PROVIDER_PROCESS")
             {
                 if (ClientSession.bElectronicCheck == false)
                 {
                     //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "if (sessionStorage.getItem('CloseNotification') != undefined && sessionStorage.getItem('CloseNotification') == 'true'){DisplayErrorMessage('010015','','');}", true);
                     //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                     //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                     ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('010015','','');", true);
                     ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                     hdnTab.Value = "tbPlan";
                     return;
                 }
             }*/
            //BugID:47782 START
            EncounterManager em = new EncounterManager();
            bool isEandMCPTPresent = false, isEandMICDPresent = false, isEandMPriICDPresent = false;
            if (ClientSession.UserRole != null)
                em.GetEandMCount(ClientSession.EncounterId, ClientSession.UserRole, out isEandMCPTPresent, out isEandMICDPresent, out isEandMPriICDPresent);

            #region E&M verfification
            //if (ehrwfobj.Current_Process == "PROVIDER_PROCESS" || ehrwfobj.Current_Process == "PHYSICIAN_CORRECTION")
            //{
            //    if (isEandMCPTPresent == false)
            //    {
            //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "if (sessionStorage.getItem('CloseNotification') != undefined && sessionStorage.getItem('CloseNotification') == 'true'){DisplayErrorMessage('180030','','');}", true);
            //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
            //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180030','','');", true);
            //        hdnTab.Value = "tbEandM";
            //        bIsWorkFlowButtonClicked = false;
            //        return;
            //    }
            //    if (isEandMICDPresent == false)
            //    {
            //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "if (sessionStorage.getItem('CloseNotification') != undefined && sessionStorage.getItem('CloseNotification') == 'true'){DisplayErrorMessage('180054','','');}", true);
            //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
            //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180054','','');", true);
            //        hdnTab.Value = "tbEandM";
            //        bIsWorkFlowButtonClicked = false;
            //        return;
            //    }
            //}

            //if (ehrwfobj.Current_Process == "REVIEW_CODING" || ehrwfobj.Current_Process == "REVIEW_CODING_2")
            //{
            //    bool iPri = isEandMPriICDPresent;
            //    if (iPri == false)
            //    {
            //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180031','','');", true);
            //        hdnTab.Value = "tbEandM";
            //        bIsWorkFlowButtonClicked = false;
            //        return;
            //    }
            //}
            ////BugID:47782 END


            /* Commented to remove Notification Pop Up on click of MovetoNextProcess
            //BugID:47780 START
            #region NotificationCheck
            if (ClientSession.NotificationCount != null && ClientSession.NotificationCount.Trim() != "0" && (hdnNotificationOkClicked != null && hdnNotificationOkClicked.Value != "true"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Encounter), "NotificationAlert", "NotificationCheck_Before_MovetoNextProcess();", true);
                return;
            }
            //BugID:47761 START
            if (hdnNotificationOkClicked != null && hdnNotificationOkClicked.Value == "true")
            {
                hdnNotificationOkClicked.Value = "";
            }
            //BugID:47761 END
            #endregion
            //BugID:47780 END
             */
            #endregion
            string IsPatientDiscussed = "";
            //objMoveVerifyDTO = objEncounterManager.PerformMoveVerification(ClientSession.EncounterId, EncProviderID, ClientSession.HumanId, UtilityManager.ConvertToLocal(Convert.ToDateTime(hdnLocalTime.Value)), ClientSession.FacilityName, ClientSession.UserName, VerifyPFSH, Source, If_Source_Of_Information_Others, ClientSession.UserCurrentProcess, string.Empty, btnMove.Text, bDuplicateCheck, ClientSession.UserRole);
            if (chkACOValidation.Checked == true)
                IsPatientDiscussed = "Y";
            else
                IsPatientDiscussed = "N";


            ulong IsDiscussedBy = 0;
            if (Request["cboPhysicianName"] != "" && Request["cboPhysicianName"] != null)
                IsDiscussedBy = Convert.ToUInt32(Request["cboPhysicianName"]);
            bool bAcoUpdate = false;
            if (chkACOValidation.Visible == true)
                bAcoUpdate = true;

            XmlDocument xmldocUser = new XmlDocument();
            if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml"))
                xmldocUser.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml");
            XmlNodeList xmlUserList = xmldocUser.GetElementsByTagName("User");
            string sFacilityName = string.Empty;
            string sRole = "";
            if (xmlUserList != null)
            {
                foreach (XmlNode item in xmlUserList)
                    if (ClientSession.PhysicianUserName.ToString().Trim().ToUpper() == item.Attributes.GetNamedItem("User_Name").Value.ToUpper().Trim())
                    {
                        //if (ClientSession.PhysicianUserName.ToString().ToUpper() == item.Attributes[0].Value.ToUpper())
                        //{
                        //sFacilityName = item.Attributes[1].Value;
                        //sRole = item.Attributes[3].Value;
                        if (item.Attributes.GetNamedItem("Default_Facility").Value != null)
                            sFacilityName = item.Attributes.GetNamedItem("Default_Facility").Value;
                        if (item.Attributes.GetNamedItem("Role").Value != null)
                            sRole = item.Attributes.GetNamedItem("Role").Value;
                        break;
                        //}
                    }
            }
            if (ClientSession.EncounterId != null)
                ClientSession.FillEncounterandWFObject.EncounterWFRecord = objWFObjectManager.GetByObjectSystemId(ClientSession.EncounterId, "ENCOUNTER");
            DateTime dtLocalTime;
            if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty)
                dtLocalTime = Convert.ToDateTime(hdnLocalTime.Value);
            else
                dtLocalTime = DateTime.UtcNow;
            if (ClientSession.FillEncounterandWFObject != null)
            {
                //if (btnMove.Value.ToUpper() == "MOVE TO PROVIDER" && ClientSession.FillEncounterandWFObject.DocumentationWFRecord.Current_Process.ToUpper() != "SCRIBE_PROCESS_WAIT" && ClientSession.FillEncounterandWFObject.EncRecord.Assigned_Scribe_User_Name.Trim()!=string.Empty)
                //    objMoveVerifyDTO = objEncounterManager.PerformMovetoProvider(ClientSession.EncounterId, EncProviderID, ClientSession.HumanId, UtilityManager.ConvertToLocal(dtLocalTime), ClientSession.FacilityName, ClientSession.UserName, VerifyPFSH, Source, If_Source_Of_Information_Others, ClientSession.UserCurrentProcess, string.Empty, btnMove.Value, bDuplicateCheck, ClientSession.UserRole, "btnMove", bMovetoReview, hdnACOValidated.Value, ClientSession.FillEncounterandWFObject.EncounterWFRecord, ClientSession.FillEncounterandWFObject.EncRecord, IsPatientDiscussed, IsDiscussedBy, bAcoUpdate, sRole, ClientSession.PhysicianUserName, "SCRIBE");

                if (btnMove.Value.ToUpper() == "MOVE TO PROVIDER" && ClientSession.FillEncounterandWFObject.DocumentationWFRecord.Current_Process.ToUpper() != "PROVIDER_PROCESS"
                   || (btnMove.Value.ToUpper() == "MOVE TO NEXT PROCESS" && ClientSession.FillEncounterandWFObject.DocumentationWFRecord.Current_Process.ToUpper() == "TECHNICIAN_PROCESS"))//CMG Ancilliary
                {
                    if (ClientSession.FillEncounterandWFObject.DocumentationWFRecord.Current_Process.ToUpper() == "SCRIBE_REVIEW_CORRECTION")
                        objMoveVerifyDTO = objEncounterManager.PerformMovetoProvider(ClientSession.EncounterId, EncProviderID, ClientSession.HumanId, UtilityManager.ConvertToLocal(dtLocalTime), ClientSession.FacilityName, ClientSession.UserName, VerifyPFSH, Source, If_Source_Of_Information_Others, ClientSession.UserCurrentProcess, string.Empty, btnMove.Value, bDuplicateCheck, ClientSession.UserRole, "btnMove", bMovetoReview, hdnACOValidated.Value, ClientSession.FillEncounterandWFObject.EncounterWFRecord, ClientSession.FillEncounterandWFObject.EncRecord, IsPatientDiscussed, IsDiscussedBy, bAcoUpdate, sRole, "UNKNOWN", string.Empty);
                    else
                        objMoveVerifyDTO = objEncounterManager.PerformMovetoProvider(ClientSession.EncounterId, EncProviderID, ClientSession.HumanId, UtilityManager.ConvertToLocal(dtLocalTime), ClientSession.FacilityName, ClientSession.UserName, VerifyPFSH, Source, If_Source_Of_Information_Others, ClientSession.UserCurrentProcess, string.Empty, btnMove.Value, bDuplicateCheck, ClientSession.UserRole, "btnMove", bMovetoReview, hdnACOValidated.Value, ClientSession.FillEncounterandWFObject.EncounterWFRecord, ClientSession.FillEncounterandWFObject.EncRecord, IsPatientDiscussed, IsDiscussedBy, bAcoUpdate, sRole, ClientSession.PhysicianUserName, string.Empty);


                }
                else if (btnMove.Value.ToUpper() == "MOVE TO NEXT PROCESS" || ClientSession.FillEncounterandWFObject.DocumentationWFRecord.Current_Process.ToUpper() == "PROVIDER_PROCESS")
                {
                    if (ClientSession.FillEncounterandWFObject.EncRecord.Proceed_with_Surgery_Planned == "Y")
                    {
                        if (ClientSession.FillEncounterandWFObject.EncounterWFRecord.Current_Process.ToUpper() == "SURGERY_COORDINATOR_PROCESS")
                        {
                            WFObjectManager objenco = new WFObjectManager();
                            objenco.MoveToNextProcess(ClientSession.EncounterId, "ENCOUNTER", 6, "UNKNOWN", UtilityManager.ConvertToLocal(dtLocalTime), string.Empty, null, null);
                        }
                        else
                        {
                            string sAlert = string.Empty;
                            WFObjectManager objenco = new WFObjectManager();
                            objenco.MoveToNextProcess(ClientSession.EncounterId, "ENCOUNTER", 6, "UNKNOWN", UtilityManager.ConvertToLocal(dtLocalTime), string.Empty, null, null);
                            objMoveVerifyDTO = objEncounterManager.PerformMovetoNextProcess(ClientSession.EncounterId, EncProviderID, ClientSession.HumanId, UtilityManager.ConvertToLocal(dtLocalTime), ClientSession.FacilityName, ClientSession.UserName, VerifyPFSH, Source, If_Source_Of_Information_Others, ClientSession.UserCurrentProcess, string.Empty, btnMove.Value, bDuplicateCheck, ClientSession.UserRole, "btnMove", bMovetoReview, hdnACOValidated.Value, null, ClientSession.FillEncounterandWFObject.EncRecord, ClientSession.FillEncounterandWFObject.DocumentationWFRecord, null, IsPatientDiscussed, IsDiscussedBy, out sAlert);
                        }
                    }
                    else
                    {
                        if (ehrwfobj.Current_Process == "PROVIDER_PROCESS" && chkcorrectionreview.Checked)
                        {
                            MessageWindow.Visible = true;
                            MessageWindow.VisibleOnPageLoad = true;
                            MessageWindow.Height = Unit.Pixel(730);
                            MessageWindow.Width = Unit.Pixel(855);
                            MessageWindow.VisibleStatusbar = false;
                            MessageWindow.KeepInScreenBounds = true;
                            //MessageWindow.ShowContentDuringLoad = false;
                            MessageWindow.ReloadOnShow = true;
                            MessageWindow.OnClientClose = "ClosePrintDocuments";
                            ViewState["Chkoutbtn"] = false;
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                            string phyid = "0";
                            string isreview = chkProviderReview.Checked.ToString();
                            if (isreview.ToUpper() == "TRUE")
                            {
                                phyid = Convert.ToUInt64(cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value).ToString();
                            }

                            MessageWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + phyid +
                                "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + btnMove.Value
                                + "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + chkProviderReview.Checked.ToString() + "&DS=" + "true" + "&Scribe=" + ClientSession.FillEncounterandWFObject.EncRecord.Assigned_Scribe_User_Name.ToString().Trim();
                            return;

                        }
                        else
                        {
                            string sAlert = string.Empty;
                            objMoveVerifyDTO = objEncounterManager.PerformMovetoNextProcess(ClientSession.EncounterId, EncProviderID, ClientSession.HumanId, UtilityManager.ConvertToLocal(dtLocalTime), ClientSession.FacilityName, ClientSession.UserName, VerifyPFSH, Source, If_Source_Of_Information_Others, ClientSession.UserCurrentProcess, string.Empty, btnMove.Value, bDuplicateCheck, ClientSession.UserRole, "btnMove", bMovetoReview, hdnACOValidated.Value, ClientSession.FillEncounterandWFObject.EncounterWFRecord, ClientSession.FillEncounterandWFObject.EncRecord, ClientSession.FillEncounterandWFObject.DocumentationWFRecord, null, IsPatientDiscussed, IsDiscussedBy, out sAlert);
                            //CAP-975
                            if ((Convert.ToBoolean(objMoveVerifyDTO.IsGcodePresent) == true) && (Convert.ToBoolean(objMoveVerifyDTO.IsICDPresent) == false))
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('" + sAlert + "','','');", true);
                                if (hdnTab != null)
                                    hdnTab.Value = "tbEandM";
                                return;
                            }
                        }
                    }
                }
                else
                    objMoveVerifyDTO = objEncounterManager.PerformMoveVerification(ClientSession.EncounterId, EncProviderID, ClientSession.HumanId, UtilityManager.ConvertToLocal(dtLocalTime), ClientSession.FacilityName, ClientSession.UserName, VerifyPFSH, Source, If_Source_Of_Information_Others, ClientSession.UserCurrentProcess, string.Empty, btnMove.Value, bDuplicateCheck, ClientSession.UserRole, "btnMove", bMovetoReview, hdnACOValidated.Value, out string sAlert);
            }

            string is_revChecked = string.Empty;
            is_revChecked = chkProviderReview.Checked.ToString().ToUpper();
            //if (is_revChecked != string.Empty && is_revChecked == "FALSE" && Session["EligibleProvReview"] != null && is_revChecked.ToString().ToUpper() != Session["EligibleProvReview"].ToString().ToUpper())
            //{
            //    UpdateReviewTrackerforEncounter();
            //}

            //CheckAgain:
            bool triggerSP = false;
            string Input_Mode = string.Empty;
            if (ClientSession.UserRole != null && (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" && btnMove.Value.ToUpper() == "MOVE TO PROVIDER") || (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && btnMove.Value.ToUpper() == "MOVE TO NEXT PROCESS"))
            {
                if (is_revChecked != null && is_revChecked.ToString().ToUpper() == "FALSE" && Session["EligibleProvReview"] != null && is_revChecked.ToString().ToUpper() != Session["EligibleProvReview"].ToString().ToUpper())//trigger SP only if initially its marked for Review but now PA changes it to not be reviewd.
                {
                    triggerSP = true;
                    Input_Mode = "STATUS_CHANGE";
                }
                if (cboPhysicianName.SelectedValue.ToString().Trim() != string.Empty)
                {
                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
                    {
                        if (Session["AppointmenttProvID"] != null && cboPhysicianName.SelectedValue.ToString() != Session["AppointmenttProvID"].ToString())
                        {
                            triggerSP = true;
                            Input_Mode = "PROVIDER_CHANGE";
                        }

                    }
                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                    {
                        if (Session["EncounterProvID"] != null && cboPhysicianName.SelectedValue.ToString() != Session["EncounterProvID"].ToString())
                        {
                            triggerSP = true;
                            Input_Mode = "PROVIDER_CHANGE";
                        }

                    }
                }

            }
            if (triggerSP)
                UpdateReviewTrackerforEncounter();
            //objEncounterManager.TriggerSPforProvReviewStatusTracker(Input_Mode, ClientSession.EncounterId);

            if (objMoveVerifyDTO != null)
            {
                if (objMoveVerifyDTO.IsWorkflowPushed == false)
                {
                    #region E&M verfification COMMENTED
                    //if (ehrwfobj.Current_Process == "PROVIDER_PROCESS" || ehrwfobj.Current_Process == "PHYSICIAN_CORRECTION")
                    //{
                    //    int fillEM = objMoveVerifyDTO.EandMCodingList.Count();
                    //    bool IsEMICDPresent = objMoveVerifyDTO.IsICDFilled;
                    //    if (fillEM == 0)
                    //    {
                    //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "if (sessionStorage.getItem('CloseNotification') != undefined && sessionStorage.getItem('CloseNotification') == 'true'){DisplayErrorMessage('180030','','');}", true);
                    //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180030','','');", true);
                    //        hdnTab.Value = "tbEandM";
                    //        bIsWorkFlowButtonClicked = false;
                    //        return;
                    //    }
                    //    if (IsEMICDPresent == false)
                    //    {
                    //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "if (sessionStorage.getItem('CloseNotification') != undefined && sessionStorage.getItem('CloseNotification') == 'true'){DisplayErrorMessage('180054','','');}", true);
                    //        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180054','','');", true);
                    //        hdnTab.Value = "tbEandM";
                    //        bIsWorkFlowButtonClicked = false;
                    //        return;
                    //    }
                    //}

                    //if (ehrwfobj.Current_Process == "REVIEW_CODING" || ehrwfobj.Current_Process == "REVIEW_CODING_2")
                    //{
                    //    bool iPri = objMoveVerifyDTO.EAndMIsPrimaryFilled;
                    //    if (iPri == false)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180031','','');", true);
                    //        hdnTab.Value = "tbEandM";
                    //        bIsWorkFlowButtonClicked = false;
                    //        return;
                    //    }
                    //}

                    #endregion
                }
                else if (objMoveVerifyDTO.IsWorkflowPushed == true)
                {
                    if (btnMove.Value == "Move to Next Process")
                    {
                        if (ehrwfobj.Current_Process == "PROVIDER_PROCESS" && btnMove.Value == "Move to Next Process")
                        {
                            bMyDigitalSign = true;
                            if (ClientSession.HumanId != null)
                                RemoveWindowItem(ClientSession.HumanId);
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "Confirmation", "MovedSuccessfully();", true);
                            ViewState["Confirmed_To_Proceed"] = null;
                        }
                        else if (ehrwfobj.Current_Process == "TECHNICIAN_PROCESS" && btnMove.Value == "Move to Next Process")//CMG Ancilliary
                        {
                            RemoveWindowItem(ClientSession.HumanId);
                            Response.Write("<script> window.top.location.href=\" frmMyQueueNew.aspx\"; </script>");
                            bFormClose = false;
                        }
                    }
                    else
                    {
                        //Close the form if the enc is moved
                        if (ClientSession.HumanId != null)
                            RemoveWindowItem(ClientSession.HumanId);
                        Response.Write("<script> window.top.location.href=\" frmMyQueueNew.aspx\"; </script>");
                        bFormClose = false;
                    }
                }
            }
            #region ForPhysician
            if (btnMove.Value == "Move to Next Process" && ClientSession.UserRole.ToUpper() == "PHYSICIAN")
            {
                if (ehrwfobj.Current_Process == "PROVIDER_PROCESS")
                {
                    bMyDigitalSign = true;
                }


                else if (ehrwfobj.Current_Process == "PROVIDER_REVIEW" || ehrwfobj.Current_Process == "PROVIDER_REVIEW_2")
                {

                    bMyDigitalSign = true;
                    //if (cboPhysicianName.SelectedItem != null)
                    if (hdnLocalPhy.Value != string.Empty)
                    {
                        MessageWindow.Visible = true;
                        MessageWindow.VisibleOnPageLoad = true;
                        MessageWindow.OnClientClose = "OnClientCloseEncounter";
                        MessageWindow.VisibleStatusbar = false;
                        MessageWindow.ReloadOnShow = true;
                        MessageWindow.ShowContentDuringLoad = false;
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        MessageWindow.NavigateUrl = "frmACOValidation.aspx?IsPFSHVald=Y&btnName=" + btnMove.Value;
                        MessageWindow.Width = Unit.Pixel(550);
                        MessageWindow.Height = Unit.Pixel(250);
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                    }
                    else
                    {
                        MessageWindow.Visible = true;
                        MessageWindow.VisibleOnPageLoad = true;
                        MessageWindow.Height = Unit.Pixel(730);
                        MessageWindow.Width = Unit.Pixel(855);
                        MessageWindow.VisibleStatusbar = false;
                        MessageWindow.KeepInScreenBounds = true;
                        //MessageWindow.ShowContentDuringLoad = false;
                        MessageWindow.ReloadOnShow = true;
                        MessageWindow.OnClientClose = "ClosePrintDocuments";
                        ViewState["Chkoutbtn"] = false;
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        MessageWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + '0' +
                            "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + btnMove.Value
                            + "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "true";
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);

                    }
                    if (bFormClose == true)
                    {
                        bIsWorkFlowButtonClicked = false;
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.removeItem('CloseNotification');sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "RemoveNotification", " {sessionStorage.removeItem('CloseNotification');}", true);
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        return;
                    }

                }
                else if (ehrwfobj.Current_Process.ToUpper().Trim() == "DICTATION_REVIEW")
                {
                    #region E&M verfification
                    //int EMCount = 0;
                    //EAndMCodingManager objEAndMCodingManager = new EAndMCodingManager();
                    //EandMCodingICDManager objEandMCodingICDManager = new EandMCodingICDManager();
                    //if (ehrwfobj.Current_Process.Trim().ToUpper() == "DICTATION_REVIEW")
                    //{
                    //    EMCount = objEAndMCodingManager.GetEMCount(ClientSession.EncounterId);
                    //    int IsEMICDPresent = objEandMCodingICDManager.GetEMICDCount(ClientSession.EncounterId);
                    //    if (EMCount == 0)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180030','','');", true);
                    //        hdnTab.Value = "tbEandM";
                    //        return;
                    //    }
                    //    if (IsEMICDPresent == 0)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180054','','');", true);
                    //        hdnTab.Value = "tbEandM";
                    //        bIsWorkFlowButtonClicked = false;
                    //        return;
                    //    }
                    //}
                    //if (Convert.ToBoolean(ClientSession.bElectronicCheck) == false)
                    //{
                    //    ClientSession.SetSelectedTab = "PLAN";
                    //    hdnTab.Value = "tbPlan";
                    //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('010015','','');", true);
                    //    hdnTab.Value = "tbPlan";
                    //    return;
                    //}
                    #endregion
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value != string.Empty)
                    {
                        objWFObjectManager.MoveToNextProcess(ClientSession.EncounterId, "DOCUMENTATION", 1, "UNKNOWN", Convert.ToDateTime(hdnLocalTime.Value), string.Empty, new string[] { }, null);
                    }
                }
            }

            #endregion

            #region ForPhyAsst
            if (btnMove.Value.ToUpper() == "MOVE TO NEXT PROCESS" && ehrwfobj.Current_Process == "PROVIDER_PROCESS")
            {

                //Added by Selvaraman - for Workflow
                bIsWorkFlowButtonClicked = true;
                bMyDigitalSign = true;
                if (hdnLocalPhy.Value != null && hdnLocalPhy.Value != string.Empty)
                {
                    /*if (hdnLocalTime.Value != string.Empty && ViewState["flag"].ToString().ToLower() == "true")
                    {
                        //objEncounterManager.MoveToNextProcessFromAfterPlan(EncRecord, ClientSession.EncounterId, ClientSession.HumanId, Convert.ToUInt64(cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value), ClientSession.UserName, btnMove.Text, ClientSession.FacilityName, string.Empty, Convert.ToDateTime(hdnLocalTime.Value), bMovetoReview);
                    }*/
                    hdnCopyToProviderForReview.Value = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('180029');", true);
                    // ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    Response.Write("<script> window.top.location.href=\" frmMyQueueNew.aspx\"; </script>");
                }
            }
            #endregion

            #region ForSurCoord
            if (btnMove.Value.ToUpper() == "MOVE TO NEXT PROCESS" && ehrwfobj.Current_Process == "SURGERY_COORDINATOR_PROCESS")
            {
                //Added by Selvaraman - for Workflow
                Response.Write("<script> window.top.location.href=\" frmMyQueueNew.aspx\"; </script>");
            }
            #endregion
            //Jira #CAP-707
            if (btnMove.Value.ToUpper() == "MOVE TO NEXT PROCESS" && ehrwfobj.Current_Process == "AKIDO_SCRIBE_PROCESS")
            {
                //Added by Selvaraman - for Workflow
                Response.Write("<script> window.top.location.href=\" frmMyQueueNew.aspx\"; </script>");
            }
            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}RefreshNotification('CalculateALL');", true);//BugID:47852
        }


        protected void btnPhysiciancorrection_Click(object sender, EventArgs e)
        {
            ulong encProID;
            bool status = false, enableNotifAlert = false;

            #region EnableNotificationAlert
            if (btnPhysiciancorrection.Value.ToUpper() == "MOVE TO CHECKOUT" && (ehrwfobj != null && ehrwfobj.Current_Process == "MA_PROCESS") && hdnChkOut.Value == "true")
                enableNotifAlert = true;
            else if (btnPhysiciancorrection.Value.ToUpper() != "MOVE TO CHECKOUT" && ehrwfobj != null && ((ehrwfobj.Current_Process == "PROVIDER_PROCESS" || ehrwfobj.Current_Process == "CODER_REVIEW_CORRECTION" || ehrwfobj.Current_Process == "PROVIDER_REVIEW_CORRECTION") || (ehrwfobj.Current_Process == "REVIEW_CODING" || ehrwfobj.Current_Process == "REVIEW_CODING_2" || ehrwfobj.Current_Process == "AKIDO_REVIEW_CODING")))
                enableNotifAlert = true;
            #endregion


            #region NotificationCheck
            /*************Note:If any change in this notification part please do this same change in Move button also *****************/
            //For Bug ID 56265 , 56264 
            if (enableNotifAlert)
            {
                IList<CDSRuleMaster> lstCDSRules = new List<CDSRuleMaster>();
                if (ClientSession.CDSNotificationRule != null && ClientSession.CDSNotificationRule.Count == 0)
                {
                    CDSRuleMasterManager ClinicalDecMgr = new CDSRuleMasterManager();
                    lstCDSRules = ClinicalDecMgr.GellCDSRuleMaster();
                    ClientSession.CDSNotificationRule = lstCDSRules;
                }
                else
                {
                    lstCDSRules = ClientSession.CDSNotificationRule;
                }
                //Get only active notification
                NotificationManager objNotification = new NotificationManager();
                IList<Notification> lstNotification = new List<Notification>();
                lstNotification = objNotification.GetNotificationForHumanIDandEncounterIDwithActiveSatus(ClientSession.HumanId, ClientSession.EncounterId);

                lstCDSRules = lstCDSRules.Where(a => a.Classification.Trim().ToUpper() == "MANDATORY ALERT").ToList<CDSRuleMaster>();
                bool isMandatoryYes = false;

                string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
                XmlDocument itemPhysiciandoc = new XmlDocument();
                XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
                itemPhysiciandoc.Load(XmlPhysicianText);
                string sPhysicianSpecialty = string.Empty;

                XmlNodeList xmlphy = itemPhysiciandoc.GetElementsByTagName("p" + ClientSession.FillEncounterandWFObject.EncRecord.Encounter_Provider_ID.ToString());
                if (xmlphy.Count > 0)
                {
                    sPhysicianSpecialty = xmlphy[0].Attributes[7].Value;
                }

                foreach (Notification item in lstNotification)
                {
                    IList<CDSRuleMaster> lst = lstCDSRules.Where(a => a.Clinincal_Decision_Name.Trim().ToUpper() == item.CDS_Rule_Master_Name.Trim().ToUpper()
                        && a.Role_Privilege.ToUpper().Contains(ClientSession.UserCurrentProcess.ToUpper())).ToList<CDSRuleMaster>();

                    if (lst != null && lst.Count > 0)
                    {
                        //For Bug ID: 70525
                        if (lst[0].Physician_Specialty.Trim() != string.Empty)
                        {
                            string[] arySpeciality = sPhysicianSpecialty.Split(',');
                            var ary = lst[0].Physician_Specialty.Split(',').Intersect(arySpeciality);
                            foreach (string s in ary)
                            {
                                isMandatoryYes = true;
                                break;
                            }
                        }
                        else
                        {
                            isMandatoryYes = true;
                            break;
                        }
                    }
                    //if (lst != null && lst.Count > 0)
                    //{
                    //    isMandatoryYes = true;
                    //    break;
                    //}
                }
                if (isMandatoryYes == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Encounter), "NotificationAlert", "OpenNotification_Before_MovetoNextProcess();", true);
                    return;
                }

            }

            //For Bug ID 56265 , 56264 
            //if (enableNotifAlert && ClientSession.NotificationCount != null && ClientSession.NotificationCount.Trim() != "0" && (ClientSession.bIsMandatoryNotifPresent))
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(Encounter), "NotificationAlert", "OpenNotification_Before_MovetoNextProcess();", true);
            //    return;
            //}
            #endregion

            if (btnPhysiciancorrection.Value.ToUpper() == "MOVE TO CHECKOUT")
            {
                if (ClientSession.HumanId != null && objHumanManagerACO.IsACOValid(ClientSession.HumanId) == true && hdnACOValidated.Value != "true")
                {
                    MessageWindow.NavigateUrl = "frmACOValidation.aspx?IsPFSHVald=Y&btnName=" + btnPhysiciancorrection.Value;
                    MessageWindow.Width = Unit.Pixel(550);
                    MessageWindow.Height = Unit.Pixel(250);
                    MessageWindow.OnClientClose = "OnClientCloseForPrintDocument";
                    MessageWindow.VisibleStatusbar = false;
                    MessageWindow.Visible = true;
                    MessageWindow.VisibleOnPageLoad = true;
                    status = true;
                    ViewState["status"] = status;
                    return;
                }
            }

            if (hdnPFSHVerifiedEnable.Value == "TRUE")
                Session["PFSHVerifiedEnable"] = "TRUE";
            else
                Session["PFSHVerifiedEnable"] = "FALSE";
            if (ClientSession.PatientPaneList.Count > 0)
                if (ClientSession.PatientPaneList[iMySelectedNode] != null)
                    sHumanName = ClientSession.PatientPaneList[iMySelectedNode].Last_Name + "," + ClientSession.PatientPaneList[iMySelectedNode].First_Name + " " + ClientSession.PatientPaneList[iMySelectedNode].MI + " " + ClientSession.PatientPaneList[iMySelectedNode].Suffix;
            bIsWorkFlowButtonClicked = true;
            string Source = string.Empty, If_Source_Of_Information_Others = string.Empty;

            //If Move to Checkout - we are opening the Print Document Screen.
            //So, No need to ask the confirmation message. But, needed for Coder and Physician Correction

            if (btnPhysiciancorrection.Value.ToUpper() == "MOVE TO CHECKOUT" && (ehrwfobj != null && ehrwfobj.Current_Process == "MA_PROCESS") && hdnChkOut.Value == "true")
            {
                EAndMCodingManager objEMCOdingManager = new EAndMCodingManager();
                //if (objEMCOdingManager.GetEMCount(ClientSession.EncounterId) == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180030','','');", true);
                //    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //    hdnTab.Value = "tbEandM";
                //    return;
                //}
                bMyDigitalSign = false;

                MessageWindow.Visible = true;
                MessageWindow.VisibleOnPageLoad = true;
                ////comment by balaji.TJ       
                //if (ehrwfobj.Current_Process == "REVIEW_CODING" || ehrwfobj.Current_Process == "REVIEW_CODING_2")
                //{
                //    MessageWindow.Height = Unit.Pixel(750);
                //}
                //else
                //{
                MessageWindow.Height = Unit.Pixel(730);
                // }
                MessageWindow.Width = Unit.Pixel(855);
                MessageWindow.VisibleStatusbar = false;
                MessageWindow.KeepInScreenBounds = true;
                //MessageWindow.ShowContentDuringLoad = false;
                MessageWindow.ReloadOnShow = true;
                MessageWindow.Behaviors = WindowBehaviors.None;
                MessageWindow.OnClientClose = "OnCheckoutMAprocess";
                //if (cboPhysicianName.SelectedItem != null)
                if (ClientSession.EncounterId != null && hdnLocalPhy.Value != null && hdnLocalPhy.Value != string.Empty && ulong.TryParse(hdnLocalPhy.Value, out encProID))
                {
                    //MessageWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value + "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + btnPhysiciancorrection.Text + "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "false";
                    MessageWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + Convert.ToUInt32(hdnLocalPhy.Value.Split('~')[1]) + "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + btnPhysiciancorrection.Value + "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "false";
                }
                else
                {
                    MessageWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + '0' + "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + btnPhysiciancorrection.Value + "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "false";
                }

                return;
            }
            else if (btnPhysiciancorrection.Value.ToUpper() == "MOVE TO CHECKOUT" && hdnChkOut.Value == "true")
            {
                bMyDigitalSign = false;

                MessageWindow.Visible = true;
                MessageWindow.VisibleOnPageLoad = true;
                if (ehrwfobj != null && ehrwfobj.Current_Process == "REVIEW_CODING" || ehrwfobj.Current_Process == "REVIEW_CODING_2")
                {
                    MessageWindow.Height = Unit.Pixel(730);
                }
                else
                {
                    MessageWindow.Height = Unit.Pixel(730);
                }
                MessageWindow.Width = Unit.Pixel(855);
                MessageWindow.VisibleStatusbar = false;
                MessageWindow.KeepInScreenBounds = true;
                //MessageWindow.ShowContentDuringLoad = false;
                MessageWindow.ReloadOnShow = true;
                MessageWindow.Behaviors = WindowBehaviors.None;
                MessageWindow.OnClientClose = "OnCheckoutClosePrintDocuments";
                ViewState["Chkoutbtn"] = false;
                //if (cboPhysicianName.SelectedItem != null)
                if (hdnLocalPhy.Value != null && hdnLocalPhy.Value != string.Empty && ulong.TryParse(hdnLocalPhy.Value, out encProID))
                {
                    MessageWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + Convert.ToUInt32(hdnLocalPhy.Value.Split('~')[1]) + "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + btnPhysiciancorrection.Value + "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "false";
                }
                else
                {
                    MessageWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + '0' + "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + btnPhysiciancorrection.Value + "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "false";
                }

                return;
            }

            string sAlert = string.Empty;
            //MoveVerificationDTO objMoveVerifyDTO = objEncounterManager.PerformMoveVerification(ClientSession.EncounterId, ClientSession.PhysicianId, ClientSession.HumanId, UtilityManager.ConvertToLocal(DateTime.Now), ClientSession.FacilityName, ClientSession.UserName, false, string.Empty, string.Empty, ClientSession.UserCurrentProcess, string.Empty, btnPhysiciancorrection.Text, bDuplicateCheck, ClientSession.UserRole);
            MoveVerificationDTO objMoveVerifyDTO = objEncounterManager.PerformMoveVerification(ClientSession.EncounterId, ClientSession.PhysicianId, ClientSession.HumanId, UtilityManager.ConvertToLocal(DateTime.Now), ClientSession.FacilityName, ClientSession.UserName, false, string.Empty, string.Empty, ClientSession.UserCurrentProcess, string.Empty, btnPhysiciancorrection.Value, bDuplicateCheck, ClientSession.UserRole, "btnPhysiciancorrection", bDuplicateCheck, hdnACOValidated.Value, out sAlert);
            if (btnPhysiciancorrection.Value.ToUpper() != "MOVE TO CHECKOUT")
            {
                if (objMoveVerifyDTO != null)
                {
                    #region E&M verfification
                    //if (ehrwfobj.Current_Process == "PROVIDER_PROCESS" || ehrwfobj.Current_Process == "PHYSICIAN_CORRECTION")
                    //{
                    //    //int fillEM = objMoveVerifyDTO.EAndMCount;
                    //    int fillEM = objMoveVerifyDTO.EandMCodingList.Count();
                    //    bool IsEMICDPresent = objMoveVerifyDTO.IsICDFilled;
                    //    if (fillEM == 0)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage",
                    //            "DisplayErrorMessage('180030','','');", true);
                    //        hdnTab.Value = "tbEandM";
                    //        //   tabStripEncounter.Tabs.FindTabByText("SERV./PROC. CODES").Selected = true;
                    //        //   pageViewServProcCodes.ContentUrl = "frmEandMcoding.aspx";
                    //        //   pageViewServProcCodes.Selected = true;
                    //        return;
                    //    }
                    //    if (IsEMICDPresent == false)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180054','','');", true);
                    //        hdnTab.Value = "tbEandM";
                    //        bIsWorkFlowButtonClicked = false;
                    //        return;
                    //    }
                    //}
                    //Jira Cap- 740
                    // if (ehrwfobj.Current_Process == "REVIEW_CODING" || ehrwfobj.Current_Process == "REVIEW_CODING_2" )
                    if (ehrwfobj.Current_Process == "REVIEW_CODING" || ehrwfobj.Current_Process == "REVIEW_CODING_2" || ehrwfobj.Current_Process == "AKIDO_REVIEW_CODING")
                    {

                        bool IsPrimaryFilled = objMoveVerifyDTO.EAndMIsPrimaryFilled;
                        if (IsPrimaryFilled == false)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180031','','');", true);
                            //CAP-790
                            if (hdnTab != null)
                                hdnTab.Value = "tbEandM";
                            //    tabStripEncounter.Tabs.FindTabByText("SERV./PROC. CODES").Selected = true;
                            //     pageViewServProcCodes.ContentUrl = "frmEandMcoding.aspx";
                            //     pageViewServProcCodes.Selected = true;
                            return;
                        }
                        if ((Convert.ToBoolean(objMoveVerifyDTO.IsGcodePresent) == true) && (Convert.ToBoolean(objMoveVerifyDTO.IsICDPresent) == false))
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('180045','','');", true);
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('" + sAlert + "','','');", true);
                            //CAP-790
                            if (hdnTab != null)
                                hdnTab.Value = "tbEandM";
                            // tabStripEncounter.Tabs.FindTabByText("SERV./PROC. CODES").Selected = true;
                            //  pageViewServProcCodes.ContentUrl = "frmEandMcoding.aspx";
                            //  pageViewServProcCodes.Selected = true;
                            return;
                        }
                        if (Convert.ToBoolean(objMoveVerifyDTO.IsDuplicatePresent) == true)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Encounter", "DuplicateWarningMessage();", true);
                        }
                    }
                    #endregion
                    if (objMoveVerifyDTO.IsWorkflowPushed)
                    {
                        bIsWorkFlowButtonClicked = true;
                        Assembly currentAssembly = Assembly.GetExecutingAssembly();

                        /* for (int i = 0; i < System.Windows.Forms.Application.OpenForms.Count; i++)
                         {
                         }*/
                        if (ClientSession.HumanId != null)
                            RemoveWindowItem(ClientSession.HumanId);
                        Response.Write("<script> window.top.location.href=\" frmMyQueueNew.aspx\"; </script>");
                        bFormClose = false;
                    }
                    else
                    {
                        if (ehrwfobj != null && ehrwfobj.Current_Process == "REVIEW_CODING" || ehrwfobj.Current_Process == "REVIEW_CODING_2")
                        {
                            if (objMoveVerifyDTO.ExceptionCount > 0)
                            {
                            }
                            else
                            {
                            }
                            if (objMoveVerifyDTO.IsFeedBackProvided == false)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "CreateCodingException();", true);
                                //RadWindow1.Visible = true;
                                //RadWindow1.NavigateUrl = "frmException.aspx?formName=" + "Create Coding Exception";
                                //RadWindow1.VisibleTitlebar = true;
                                //RadWindow1.VisibleStatusbar = false;
                                //RadWindow1.VisibleOnPageLoad = true;
                                //RadWindow1.Height = 660;
                                //RadWindow1.Width = 920;
                            }
                        }
                        else if (ehrwfobj.Current_Process == "CODER_REVIEW_CORRECTION")
                        {
                            if (objMoveVerifyDTO.ExceptionCreatedBy != string.Empty)
                            {
                                MessageWindow.Visible = false;
                                if (objMoveVerifyDTO.IsFeedBackProvided == false)
                                {
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "FeedbackCodingException();", true);//For Bug Id 54759

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "FeedbackCodingException(" + hdnAddendumID.Value + ");", true);
                                    //RadWindow1.Visible = true;
                                    //RadWindow1.NavigateUrl = "frmException.aspx?formName=" + "Feedback for Coding Exception";
                                    //RadWindow1.VisibleTitlebar = true;
                                    //RadWindow1.VisibleStatusbar = false;
                                    //RadWindow1.VisibleOnPageLoad = true;
                                    //RadWindow1.Height = 750;
                                    //RadWindow1.Width = 900;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                #region ForMoveToCheckOut
                if (hdnChkOut.Value != null && btnPhysiciancorrection.Value.ToUpper() == "MOVE TO CHECKOUT" && hdnChkOut.Value == "true")
                {
                    bMyDigitalSign = false;
                    //if (cboPhysicianName.SelectedItem != null)
                    if (hdnLocalPhy.Value != null && hdnLocalPhy.Value != string.Empty)
                    {
                        MessageWindow.Visible = true;
                        MessageWindow.VisibleOnPageLoad = true;
                        MessageWindow.Height = Unit.Pixel(730);
                        MessageWindow.Width = Unit.Pixel(855);
                        MessageWindow.VisibleStatusbar = false;
                        MessageWindow.KeepInScreenBounds = true;
                        //MessageWindow.ShowContentDuringLoad = false;
                        MessageWindow.ReloadOnShow = true;
                        MessageWindow.OnClientClose = "OnCheckoutClosePrintDocuments";
                        ViewState["Chkoutbtn"] = false;
                        //MessageWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value + "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + btnMove.Text + "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "false";
                        MessageWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + Convert.ToUInt32(hdnLocalPhy.Value.Split('~')[1]) + "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + btnMove.Value + "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "false";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPrintDocuments('" + ClientSession.EncounterId + "','0','" + ClientSession.HumanId + "','" + ClientSession.UserName + "','" + btnPhysiciancorrection.Value + "','" + sHumanName + "','" + ClientSession.PhysicianId + "','false','false');", true);
                    }
                    if (bFormClose == true)
                    {
                        btnPhysiciancorrection.Disabled = false;
                        return;
                    }
                }
                #endregion
            }


        }



        protected void hiddenButton_Click(object sender, EventArgs e)
        {
            Encounter EncRecord = null;
            if (ClientSession.EncounterId != null)
                EncRecord = objEncounterManager.GetEncounterByEncounterID(ClientSession.EncounterId)[0];
            if (MysignID.Value.Trim() != string.Empty || SignedDateAndTime.Value != string.Empty)
            {
                if (MysignID.Value.Trim() != string.Empty)
                {
                    EncRecord.Encounter_Provider_Review_ID = Convert.ToInt32(MysignID.Value);
                }
                if (SignedDateAndTime.Value.Trim() != string.Empty)
                {
                    EncRecord.Encounter_Provider_Review_Signed_Date = Convert.ToDateTime(SignedDateAndTime.Value);
                }
                objEncounterManager.UpdateEncounter(EncRecord, string.Empty, new object[] { "false" });
            }
            else
            {
                Response.Write("<script> window.top.location.href=\"frmMyQueueNew.aspx\"; </script>");
            }
        }

        protected void btnhdncbofill_Click(object sender, EventArgs e)
        {
            if (EncRecord != null && EncRecord.Facility_Name != "")
                FillCboPhysician(EncRecord.Facility_Name);

        }
        public void chkShowAllPhysicians_CheckedChanged(object sender, EventArgs e)
        {
            if (EncRecord != null && EncRecord.Facility_Name != "")
                FillCboPhysician(EncRecord.Facility_Name);
            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursorStop", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        public void FillCboPhysician(string sFacility_Name)
        {

            cboPhysicianName.Items.Clear();

            #region OLD Code
            ///* Code Block to populate physician list */
            //// bool showAll = true;
            // XDocument xmlDocumentType = null;
            //if (File.Exists(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml")))
            //xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
            ////StaticLookup objStatics = null;
            //ListItem liDropdown = null;
            //IList<string> PhyASStIDlist = new List<string>();
            //IList<string> PhyIDlist = new List<string>();
            //Dictionary<string, string> hashUser = new Dictionary<string, string>();


            //int i = 0;

            //foreach (XElement elements in xmlDocumentType.Elements("ROOT").Elements("PhyAsstList").Elements())
            //{
            //    PhyASStIDlist.Add(elements.Attribute("ID").Value);
            //    if (elements.Attribute("ID").Value.ToString() == ClientSession.PhysicianId.ToString())
            //    {
            //        PhyIDlist.Add(elements.Attribute("Physician_ID").Value);
            //        i++;
            //    }
            //}



            //IList<ListItem> liComboItems = new List<ListItem>();
            ////if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            ////{
            ////    for (int iCount=0;iCount<PhyASStIDlist.Count;iCount++)
            ////    {
            ////        if (PhyASStIDlist[iCount].ToString() == ClientSession.PhysicianId.ToString() && sFacility_Name != string.Empty)
            ////        {
            ////            foreach (XElement elements in xmlDocumentType.Elements("ROOT").Elements("PhyList").Elements())
            ////            {
            ////                string xmlValue = elements.Attribute("name").Value;
            ////                if (elements.Attribute("Legal_Org") != null && elements.Attribute("Legal_Org").Value == ClientSession.LegalOrg)
            ////                {
            ////                    foreach (XElement phyItems in elements.Elements())
            ////                    {
            ////                        string phyName = string.Empty;
            ////                        string username = string.Empty;
            ////                        string prefix = string.Empty;
            ////                        string firstname = string.Empty;
            ////                        string middlename = string.Empty;
            ////                        string lastname = string.Empty;
            ////                        string suffix = string.Empty;
            ////                        string phyID = string.Empty;

            ////                        if (phyItems.Attribute("username").Value != null)
            ////                            username = phyItems.Attribute("username").Value;
            ////                        if (phyItems.Attribute("prefix").Value != null)
            ////                            prefix = phyItems.Attribute("prefix").Value;
            ////                        if (phyItems.Attribute("firstname").Value != null)
            ////                            firstname = phyItems.Attribute("firstname").Value;
            ////                        if (phyItems.Attribute("middlename").Value != null)
            ////                            middlename = phyItems.Attribute("middlename").Value;
            ////                        if (phyItems.Attribute("lastname").Value != null)
            ////                            lastname = phyItems.Attribute("lastname").Value;
            ////                        if (phyItems.Attribute("suffix").Value != null)
            ////                            suffix = phyItems.Attribute("suffix").Value;
            ////                        if (phyItems.Attribute("ID").Value != null)
            ////                            phyID = phyItems.Attribute("ID").Value;

            ////                        //Gitlab# 2485 - Physician Name Display Change
            ////                        if (lastname != String.Empty)
            ////                            phyName += lastname;
            ////                        if (firstname != String.Empty)
            ////                        {
            ////                            if (phyName != String.Empty)
            ////                                phyName += "," + firstname;
            ////                            else
            ////                                phyName += firstname;
            ////                        }
            ////                        if (middlename != String.Empty)
            ////                            phyName += " " + middlename;
            ////                        if (suffix != String.Empty)
            ////                            phyName += "," + suffix;

            ////                        if (username != string.Empty && hashUser.ContainsKey(phyID) == false && phyID == PhyIDlist[iCount])
            ////                        {
            ////                            //Old Code
            ////                            //liDropdown = new ListItem(username + " - " + phyName, phyID);
            ////                            //Gitlab# 2485 - Physician Name Display Change
            ////                            hashUser.Add(phyID.ToString(), username);
            ////                            liDropdown = new ListItem(phyName, phyID);
            ////                            liDropdown.Attributes.Add("FacilityName", ClientSession.FacilityName); // xmlValue);
            ////                            liDropdown.Attributes.Add("default", "true");
            ////                            liDropdown.Attributes.CssStyle.Add("display", "");
            ////                            liComboItems.Add(liDropdown);
            ////                        }
            ////                    }
            ////                }
            ////            }
            ////        }
            ////    }
            ////}
            ////else
            //{
            //    foreach (XElement elements in xmlDocumentType.Elements("ROOT").Elements("PhyList").Elements())
            //    {
            //        string xmlValue = elements.Attribute("name").Value;
            //        // if (xmlValue != string.Empty && xmlValue.ToUpper() == ClientSession.FacilityName.ToUpper())
            //        if (elements.Attribute("Legal_Org") != null && xmlValue != string.Empty && sFacility_Name != "" && xmlValue.ToUpper() == sFacility_Name.ToUpper() && elements.Attribute("Legal_Org").Value == ClientSession.LegalOrg)
            //        {
            //            foreach (XElement phyItems in elements.Elements())
            //            {
            //                string phyName = string.Empty;
            //                string username = string.Empty;
            //                string prefix = string.Empty;
            //                string firstname = string.Empty;
            //                string middlename = string.Empty;
            //                string lastname = string.Empty;
            //                string suffix = string.Empty;
            //                string phyID = string.Empty;

            //                if (phyItems.Attribute("username").Value != null)
            //                    username = phyItems.Attribute("username").Value;
            //                if (phyItems.Attribute("prefix").Value != null)
            //                    prefix = phyItems.Attribute("prefix").Value;
            //                if (phyItems.Attribute("firstname").Value != null)
            //                    firstname = phyItems.Attribute("firstname").Value;
            //                if (phyItems.Attribute("middlename").Value != null)
            //                    middlename = phyItems.Attribute("middlename").Value;
            //                if (phyItems.Attribute("lastname").Value != null)
            //                    lastname = phyItems.Attribute("lastname").Value;
            //                if (phyItems.Attribute("suffix").Value != null)
            //                    suffix = phyItems.Attribute("suffix").Value;
            //                if (phyItems.Attribute("ID").Value != null)
            //                    phyID = phyItems.Attribute("ID").Value;

            //                //old code
            //                //if (prefix != "")
            //                //{
            //                //    phyName += prefix + " ";
            //                //}
            //                //if (firstname != "")
            //                //{
            //                //    phyName += firstname + " ";
            //                //}
            //                //if (middlename != "")
            //                //{
            //                //    phyName += middlename + " ";
            //                //}
            //                //if (lastname != "")
            //                //{
            //                //    phyName += lastname + " ";
            //                //}
            //                //if (suffix != "")
            //                //{
            //                //    phyName += suffix;
            //                //}
            //                //Gitlab# 2485 - Physician Name Display Change
            //                if (lastname != String.Empty)
            //                    phyName += lastname;
            //                if (firstname != String.Empty)
            //                {
            //                    if (phyName != String.Empty)
            //                        phyName += "," + firstname;
            //                    else
            //                        phyName += firstname;
            //                }
            //                if (middlename != String.Empty)
            //                    phyName += " " + middlename;
            //                if (suffix != String.Empty)
            //                    phyName += "," + suffix;
            //                if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && (PhyASStIDlist.Contains(phyID)))
            //                {
            //                    continue;
            //                }
            //                else
            //                {
            //                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            //                    {
            //                        if (PhyIDlist.Contains(phyID) && username != string.Empty && hashUser.ContainsKey(phyID) == false)
            //                        {
            //                            hashUser.Add(phyID.ToString(), username);
            //                            liDropdown = new ListItem(phyName, phyID);
            //                            liDropdown.Attributes.Add("FacilityName", xmlValue);
            //                            liDropdown.Attributes.Add("default", "true");
            //                            liDropdown.Attributes.CssStyle.Add("display", "");
            //                            liComboItems.Add(liDropdown);
            //                        }
            //                    }
            //                    else if (username != string.Empty && hashUser.ContainsKey(phyID) == false)
            //                    {
            //                        //Old Code
            //                        //liDropdown = new ListItem(username + " - " + phyName, phyID);
            //                        //Gitlab# 2485 - Physician Name Display Change
            //                        hashUser.Add(phyID.ToString(), username);
            //                        liDropdown = new ListItem(phyName, phyID);
            //                        liDropdown.Attributes.Add("FacilityName", xmlValue);
            //                        liDropdown.Attributes.Add("default", "true");
            //                        liDropdown.Attributes.CssStyle.Add("display", "");
            //                        liComboItems.Add(liDropdown);
            //                    }
            //                }
            //            }
            //        }
            //        else if (elements.Attribute("Legal_Org") != null && elements.Attribute("Legal_Org").Value == ClientSession.LegalOrg)
            //        {
            //            foreach (XElement phyItems in elements.Elements())
            //            {
            //                string phyName = string.Empty;
            //                string username = string.Empty;
            //                string prefix = string.Empty;
            //                string firstname = string.Empty;
            //                string middlename = string.Empty;
            //                string lastname = string.Empty;
            //                string suffix = string.Empty;
            //                string phyID = string.Empty;

            //                if (phyItems.Attribute("username").Value != null)
            //                    username = phyItems.Attribute("username").Value;
            //                if (phyItems.Attribute("prefix").Value != null)
            //                    prefix = phyItems.Attribute("prefix").Value;
            //                if (phyItems.Attribute("firstname").Value != null)
            //                    firstname = phyItems.Attribute("firstname").Value;
            //                if (phyItems.Attribute("middlename").Value != null)
            //                    middlename = phyItems.Attribute("middlename").Value;
            //                if (phyItems.Attribute("lastname").Value != null)
            //                    lastname = phyItems.Attribute("lastname").Value;
            //                if (phyItems.Attribute("suffix").Value != null)
            //                    suffix = phyItems.Attribute("suffix").Value;
            //                if (phyItems.Attribute("ID").Value != null)
            //                    phyID = phyItems.Attribute("ID").Value;

            //                //old code
            //                //if (prefix != "")
            //                //{
            //                //    phyName += prefix + " ";
            //                //}
            //                //if (firstname != "")
            //                //{
            //                //    phyName += firstname + " ";
            //                //}
            //                //if (middlename != "")
            //                //{
            //                //    phyName += middlename + " ";
            //                //}
            //                //if (lastname != "")
            //                //{
            //                //    phyName += lastname + " ";
            //                //}
            //                //if (suffix != "")
            //                //{
            //                //    phyName += suffix;
            //                //}
            //                //Gitlab# 2485 - Physician Name Display Change
            //                if (lastname != String.Empty)
            //                    phyName += lastname;
            //                if (firstname != String.Empty)
            //                {
            //                    if (phyName != String.Empty)
            //                        phyName += "," + firstname;
            //                    else
            //                        phyName += firstname;
            //                }
            //                if (middlename != String.Empty)
            //                    phyName += " " + middlename;
            //                if (suffix != String.Empty)
            //                    phyName += "," + suffix;
            //                if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && (PhyASStIDlist.Contains(phyID)))
            //                {
            //                    continue;
            //                }
            //                else
            //                {
            //                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && PhyIDlist.Contains(phyID) && username != string.Empty && hashUser.ContainsKey(phyID) == false)
            //                    {
            //                        continue;
            //                        //hashUser.Add(phyID.ToString(), username);
            //                        //liDropdown = new ListItem(phyName, phyID);
            //                        //liDropdown.Attributes.Add("FacilityName", xmlValue);
            //                        //liDropdown.Attributes.Add("default", "true");
            //                        //liDropdown.Attributes.CssStyle.Add("display", "");
            //                        //liComboItems.Add(liDropdown);

            //                    }
            //                    else if (username != string.Empty && hashUser.ContainsKey(phyID) == false)
            //                    {
            //                        //Old Code
            //                        //liDropdown = new ListItem(username + " - " + phyName, phyID);
            //                        //Gitlab# 2485 - Physician Name Display Change
            //                        hashUser.Add(phyID.ToString(), username);
            //                        liDropdown = new ListItem(phyName, phyID);
            //                        liDropdown.Attributes.Add("default", "false");
            //                        liDropdown.Attributes.Add("FacilityName", xmlValue);
            //                        liDropdown.Attributes.CssStyle.Add("display", "none");
            //                        liComboItems.Add(liDropdown);
            //                    }
            //                }

            //            }
            //        }
            //    }
            //}
            ////IList<ListItem> sortlst = liComboItems.OrderBy(x => x.Text).ToList();
            ////Old Code
            ////IList<ListItem> sortlst = liComboItems.OrderBy(x => x.Text.Split('-')[1].Split(new string[] { "Dr." }, StringSplitOptions.None).Length > 1 ? x.Text.Split('-')[1].Split(new string[] { "Dr." }, StringSplitOptions.None)[1] : x.Text.Split('-')[1]).ToList().OrderBy(x => x.Text).ToList();
            ////Gitlab# 2485 - Physician Name Display Change
            //IList<ListItem> sortlst = liComboItems.OrderBy(x => x.Text).ToList();
            ////cboPhysicianName.Items.AddRange(sortlst.Distinct().ToArray());bugId:38683   
            //cboPhysicianName.Items.AddRange(sortlst.ToArray());
            //ListItem phyEmptyItem = new ListItem("", "0");
            //cboPhysicianName.Items.Insert(0, phyEmptyItem);
            //liComboItems.OrderBy(x => x.Value).ToList();

            //if (!IsPostBack || Convert.ToBoolean(ViewState["IsRegenerateXML"]) == true)
            //{
            //    ViewState["IsRegenerateXML"] = false;
            //    if (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && ClientSession.UserCurrentProcess == "PROVIDER_PROCESS")
            //    {
            //        IEnumerable<XElement> MapFacilityPhysician = (from x in xmlDocumentType.Elements("ROOT").Elements("PhyAsstList")
            //                                                   .Elements("PhysicianAssistant")
            //                                                      where x.Attribute("ID").Value == ClientSession.PhysicianId.ToString()
            //                                                      select x);
            //        if (MapFacilityPhysician != null && MapFacilityPhysician.Count() > 0)
            //        {
            //            ulong uDefaultPhysicianID = 0;
            //            //foreach (XElement phyItems in MapFacilityPhysician.Elements())
            //            foreach (XElement phyItems in MapFacilityPhysician)
            //            {
            //                uDefaultPhysicianID = phyItems.Attribute("Default_Physician").Value != "" ? (Convert.ToUInt32(phyItems.Attribute("Default_Physician").Value)) : (0);
            //            }
            //            ListItem SelectedPhysician = (cboPhysicianName.Items.FindByValue(uDefaultPhysicianID.ToString()));
            //            if (SelectedPhysician != null)
            //            {
            //                hdnindex.Value = cboPhysicianName.Items.IndexOf(SelectedPhysician).ToString();
            //                cboPhysicianName.SelectedIndex = Convert.ToInt32(hdnindex.Value);
            //                //Old Code
            //                //hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + cboPhysicianName.SelectedItem.Text.Split('-')[0];
            //                //Gitlab# 2485 - Physician Name Display Change
            //                hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + hashUser[cboPhysicianName.SelectedValue].ToString();
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ListItem SelectedPhysician;
            //        if (EncRecord.Encounter_Provider_ID != 0)
            //        {
            //            SelectedPhysician = (cboPhysicianName.Items.FindByValue(EncRecord.Encounter_Provider_ID.ToString()));

            //        }
            //        else
            //        {
            //            SelectedPhysician = (cboPhysicianName.Items.FindByValue(EncRecord.Appointment_Provider_ID.ToString()));
            //        }

            //        if (SelectedPhysician != null)
            //        {
            //            hdnindex.Value = cboPhysicianName.Items.IndexOf(SelectedPhysician).ToString();
            //            cboPhysicianName.SelectedIndex = Convert.ToInt32(hdnindex.Value);
            //            //Old Code
            //            //hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + cboPhysicianName.SelectedItem.Text.Split('-')[0];
            //            //Gitlab# 2485 - Physician Name Display Change
            //            hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + hashUser[cboPhysicianName.SelectedValue].ToString();
            //        }
            //    }
            //    //for (int i = 0; i < cboPhysicianName.Items.Count; i++)
            //    //{
            //    //  if (Convert.ToUInt32(cboPhysicianName.Items[i].Value) == Convert.ToInt32(EncRecord.Appointment_Provider_ID))
            //    //    {
            //    //        cboPhysicianName.SelectedIndex = i;
            //    //        hdnindex.Value = cboPhysicianName.SelectedIndex.ToString();
            //    //        hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + cboPhysicianName.SelectedItem.Text.Split('-')[0];
            //    //    }
            //    //}
            //    Session["EncounterProvID"] = cboPhysicianName.SelectedValue.ToString();
            //}
            //else
            //{
            //    ulong iIndex = 0;
            //    if (ulong.TryParse(hdnindex.Value, out iIndex) && iIndex != 0)
            //    {
            //        cboPhysicianName.SelectedIndex = Convert.ToInt32(hdnindex.Value);
            //        //Old Code
            //        //hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + cboPhysicianName.SelectedItem.Text.Split('-')[0];
            //        //Gitlab# 2485 - Physician Name Display Change
            //        hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + hashUser[cboPhysicianName.SelectedValue].ToString();
            //    }
            //    else
            //    {
            //        hdnLocalPhy.Value = string.Empty;
            //    }
            //}
            //Session["PhysicianList"] = PhyUserList;
            //Session["DefaultSelectedPhysician"] = cboPhysicianName.SelectedIndex;

            #endregion
            //Jira #Cap-360 - Supervising Provider not listed for MA to select in the chart 
            MapFacilityPhysicianManager objMapFacilityPhysician = new MapFacilityPhysicianManager();
            IList<string> Phylist = new List<string>();
            Dictionary<string, string> hashUser = new Dictionary<string, string>();
            ListItem liDropdown = null;
            IList<ListItem> liComboItems = new List<ListItem>();

            ulong uDefaultPhysicianID = 0;

            if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            {
                if (chkShowAllPhysicians.Checked == true)
                {
                    Phylist = objMapFacilityPhysician.GetPhysicianListByLegalOrg(ClientSession.LegalOrg, "ALL");
                }
                else
                {
                    Phylist = objMapFacilityPhysician.GetSupervisingPhysicianList(ClientSession.LegalOrg, ClientSession.PhysicianId, sFacility_Name);
                }

            }
            else
            {
                if (chkShowAllPhysicians.Checked == true)
                {
                    Phylist = objMapFacilityPhysician.GetPhysicianListByLegalOrg(ClientSession.LegalOrg, "ALL");
                }
                else
                {
                    Phylist = objMapFacilityPhysician.GetPhysicianListByLegalOrg(ClientSession.LegalOrg, sFacility_Name);
                }
            }

            for (int iPhycount = 0; iPhycount < Phylist.Count; iPhycount++)
            {
                string sPhyId = string.Empty;
                string sPhyName = string.Empty;
                string sFacility = string.Empty;
                string username = string.Empty;
                if (Phylist[iPhycount] != null)
                {

                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && chkShowAllPhysicians.Checked == false)
                    {
                        sPhyId = Phylist[iPhycount].Split('$')[0];
                        sPhyName = Phylist[iPhycount].Split('$')[1].Split('@')[0];
                        sFacility = Phylist[iPhycount].Split('$')[1].Split('@')[1].Split('|')[0];
                        uDefaultPhysicianID = Convert.ToUInt64(Phylist[0].Split('$')[1].Split('@')[1].Split('|')[1]);
                        username = Phylist[0].Split('$')[1].Split('@')[1].Split('|')[2];
                    }
                    else
                    {
                        sPhyId = Phylist[iPhycount].Split('$')[0];
                        sPhyName = Phylist[iPhycount].Split('$')[1].Split('@')[0];
                        sFacility = Phylist[iPhycount].Split('$')[1].Split('@')[1].Split('|')[0];
                        username = Phylist[iPhycount].Split('$')[1].Split('@')[1].Split('|')[1];
                    }


                    if (hashUser.ContainsKey(sPhyId) == false && ClientSession.CurrentPhysicianId != Convert.ToUInt64(sPhyId))
                    {
                        hashUser.Add(sPhyId.ToString(), username);
                        liDropdown = new ListItem(sPhyName, sPhyId);
                        liDropdown.Attributes.Add("FacilityName", sFacility);
                        liDropdown.Attributes.Add("default", "true");
                        liDropdown.Attributes.CssStyle.Add("display", "");
                        liComboItems.Add(liDropdown);
                    }
                }
            }

            IList<ListItem> sortlst = liComboItems.OrderBy(x => x.Text).ToList();
            cboPhysicianName.Items.AddRange(sortlst.ToArray());
            ListItem phyEmptyItem = new ListItem("", "0");
            cboPhysicianName.Items.Insert(0, phyEmptyItem);
            liComboItems.OrderBy(x => x.Value).ToList();

            if (!IsPostBack || Convert.ToBoolean(ViewState["IsRegenerateXML"]) == true)
            {
                ViewState["IsRegenerateXML"] = false;
                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && ClientSession.UserCurrentProcess == "PROVIDER_PROCESS")
                {
                    ViewState["DefaultPhysicianID"] = uDefaultPhysicianID;

                    ListItem SelectedPhysician = (cboPhysicianName.Items.FindByValue(uDefaultPhysicianID.ToString()));
                    if (SelectedPhysician != null)
                    {
                        cboPhysicianName.SelectedIndex = cboPhysicianName.Items.IndexOf(cboPhysicianName.Items.FindByValue(SelectedPhysician.Value));
                        hdnindex.Value = cboPhysicianName.SelectedValue;
                        if (hdnindex.Value != "0")
                        {
                            hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + hashUser[cboPhysicianName.SelectedValue].ToString();
                        }
                        else
                        {
                            hdnLocalPhy.Value = string.Empty;
                        }


                    }

                }
                else
                {
                    ListItem SelectedPhysician;
                    if (EncRecord.Encounter_Provider_ID != 0)
                    {
                        SelectedPhysician = (cboPhysicianName.Items.FindByValue(EncRecord.Encounter_Provider_ID.ToString()));

                    }
                    else
                    {
                        SelectedPhysician = (cboPhysicianName.Items.FindByValue(EncRecord.Appointment_Provider_ID.ToString()));
                    }

                    if (SelectedPhysician != null)
                    {
                        cboPhysicianName.SelectedIndex = cboPhysicianName.Items.IndexOf(cboPhysicianName.Items.FindByValue(SelectedPhysician.Value));
                        hdnindex.Value = cboPhysicianName.SelectedValue;
                        ViewState["DefaultPhysicianID"] = hdnindex.Value;
                        if (hdnindex.Value != "0")
                        {
                            hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + hashUser[cboPhysicianName.SelectedValue].ToString();
                        }
                        else
                        {
                            hdnLocalPhy.Value = string.Empty;
                        }
                    }
                }
            }
            else
            {
                //ulong iIndex = 0;
                // if (ulong.TryParse(hdnindex.Value, out iIndex) && iIndex != 0)
                //Jir Cap - 575 && 523
                if (hdnindex.Value != "0" && hdnindex.Value != "")
                {
                    if (cboPhysicianName.Items.FindByValue(hdnindex.Value) != null)
                    {
                        cboPhysicianName.SelectedIndex = cboPhysicianName.Items.IndexOf(cboPhysicianName.Items.FindByValue(hdnindex.Value));
                        hdnindex.Value = cboPhysicianName.SelectedValue;
                        if (hdnindex.Value != "0")
                        {
                            hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + hashUser[cboPhysicianName.SelectedValue].ToString();
                        }
                        else
                        {
                            hdnLocalPhy.Value = string.Empty;
                        }

                    }
                    else
                    {
                        //CAP-2080
                        cboPhysicianName.SelectedIndex = cboPhysicianName.Items.IndexOf(cboPhysicianName.Items.FindByValue(ViewState["DefaultPhysicianID"]?.ToString()??""));
                        hdnindex.Value = cboPhysicianName.SelectedValue;
                        if (hdnindex.Value != "0")
                        {
                            hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + hashUser[cboPhysicianName.SelectedValue].ToString();
                        }
                        else
                        {
                            hdnLocalPhy.Value = string.Empty;
                        }
                    }
                }
                else
                {
                    hdnLocalPhy.Value = string.Empty;
                }
            }
            Session["EncounterProvID"] = cboPhysicianName.SelectedValue.ToString();

            Session["PhysicianList"] = PhyUserList;
            Session["DefaultSelectedPhysician"] = cboPhysicianName.SelectedIndex;


        }
        public void RemoveWindowItem(ulong patientID)
        {
            bool bPresent = false;
            int iLoop = 0;
            ArrayList windowLst = (ArrayList)ClientSession.WindowList;

            for (iLoop = 0; iLoop < windowLst.Count; iLoop++)
            {
                if (windowLst[iLoop].ToString().Contains(patientID.ToString()))
                {
                    bPresent = true;
                    break;
                }
            }
            if (bPresent == true)
                windowLst.RemoveAt(iLoop);
            ClientSession.WindowList = windowLst;
        }

        protected void btnHiddenDuplicateCheck_Click(object sender, EventArgs e)
        {
            MoveVerificationDTO objMoveVerifyDTO;
            bDuplicateCheck = false;
            bool bCheck = true;
            //objMoveVerifyDTO = objEncounterManager.PerformMoveVerification(ClientSession.EncounterId, ClientSession.PhysicianId, ClientSession.HumanId, UtilityManager.ConvertToLocal(DateTime.Now), ClientSession.FacilityName, ClientSession.UserName, false, string.Empty, string.Empty, ClientSession.UserCurrentProcess, string.Empty, btnPhysiciancorrection.Text, bDuplicateCheck, ClientSession.UserRole);
            string check = "True";
            objMoveVerifyDTO = objEncounterManager.PerformMoveVerification(ClientSession.EncounterId, ClientSession.PhysicianId, ClientSession.HumanId, UtilityManager.ConvertToLocal(DateTime.Now), ClientSession.FacilityName, ClientSession.UserName, false, string.Empty, string.Empty, ClientSession.UserCurrentProcess, string.Empty, btnPhysiciancorrection.Value, bDuplicateCheck, ClientSession.UserRole, "btnHiddenDuplicateCheck", bCheck, check, out string sAlert);
            //Jira Cap- 740
            if (objMoveVerifyDTO.IsWorkflowPushed)
            {
                bIsWorkFlowButtonClicked = true;

                Assembly currentAssembly = Assembly.GetExecutingAssembly();

                if (ClientSession.HumanId != null)
                    RemoveWindowItem(ClientSession.HumanId);
                Response.Write("<script> window.top.location.href=\" frmMyQueueNew.aspx\"; </script>");
                bFormClose = false;
            }

        }

        protected void hdnButtonEnable_Click(object sender, EventArgs e)
        {
            btnPhysiciancorrection.Disabled = true;
            ViewState["Chkoutbtn"] = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (sContent == string.Empty && Request.Cookies["AllFacility"] != null)
                sContent = Request.Cookies["AllFacility"].Value;
            List<string> sList = sContent.TrimEnd('!').Split('!').ToList<string>();
            for (int i = 0; i < sList.Count; i++)
            {
                if (sList[i] != string.Empty)
                    sList[i] = sList[i].Split('$')[1].Split('*')[0];
            }
            //register the items from the dropdownlist as possible postbackvalues for the listbox
            foreach (string str in sList)
            {
                Page.ClientScript.RegisterForEventValidation(cboPhysicianName.UniqueID, str);
            }
            base.Render(writer);
        }

        void CopyPreviousClick()
        {
            IList<Encounter> lstEncounter = new List<Encounter>();

            EncounterManager objEncounterManager = new EncounterManager();

            bool isSeenByPhysician = false;
            bool isFromArchive = false;

            ulong physicianId = ClientSession.PhysicianId;

            if (ClientSession.FillEncounterandWFObject.EncRecord.Appointment_Provider_ID !=
                     ClientSession.FillEncounterandWFObject.EncRecord.Encounter_Provider_ID)
            {
                if (string.IsNullOrEmpty(hdnSelectPhysicianId.Value))
                {
                    var scriptMesssage = " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}";
                    scriptMesssage = " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}ProviderValidationCheck();";
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrorMessage", scriptMesssage, true);
                    return;
                }
                physicianId = Convert.ToUInt64(hdnSelectPhysicianId.Value);
            }


            lstEncounter = objEncounterManager.GetPreviousEncounterDetails(ClientSession.EncounterId,
                                                                       ClientSession.HumanId,
                                                                       physicianId,
                                                                       out isSeenByPhysician, out isFromArchive);

            if (lstEncounter.Count == 0)
            {
                var scriptMesssage = @"DisplayErrorMessage('210010');                                       
                                        {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}";
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrorMessage", scriptMesssage, true);
                return;
            }
            else if (!isSeenByPhysician)
            {
                var scriptMesssage = @"DisplayErrorMessage('210016');
                                       StopLoadFromPatChart()";
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrorMessage", scriptMesssage, true);
                return;
            }

            else if (lstEncounter.Count > 0)
            {
                PhysicianManager objPhysicianManager = new PhysicianManager();

                var objPhysicianLibrary = objPhysicianManager.GetphysiciannameByPhyID(physicianId).FirstOrDefault();

                var Assigned_Physician = string.Empty;

                if (objPhysicianLibrary != null)
                {
                    Assigned_Physician = objPhysicianLibrary.PhyPrefix + "" +
                                         objPhysicianLibrary.PhyFirstName + " " +
                                         objPhysicianLibrary.PhyMiddleName + " " +
                                         objPhysicianLibrary.PhyLastName + " " +
                                         objPhysicianLibrary.PhySuffix;
                }
                else
                {
                    // BackUp Option..

                    IList<PatientPane> sPhyName = ClientSession.PatientPaneList
                                                    .Where(a => a.Encounter_ID == lstEncounter[0].Id)
                                                    .ToList<PatientPane>();

                    Assigned_Physician = (sPhyName.Count > 0) ?
                                             sPhyName[0].Assigned_Physician :
                                                   ClientSession.PatientPaneList[0].Assigned_Physician;
                }

                string sMessage = UtilityManager.ConvertToLocal(lstEncounter[0].Date_of_Service).ToString("dd-MMM-yyyy hh:mm tt") +
                                 "-" + Assigned_Physician + "?";

                var scriptMesssage = string.Format(@"copyPreviousClick('{0}','{1}');", sMessage, lstEncounter[0].Id);

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "CopyPrevious", scriptMesssage, true);
                hdnPreviousEncounterId.Value = Convert.ToString(lstEncounter[0].Id);
                return;
            }
            else
            {
                var scriptMesssage = @"DisplayErrorMessage('210010');
                                       StopLoadFromPatChart()";
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrorMessage", scriptMesssage, true);
                return;
            }
        }

        protected void btnCopyPreviousHidden_Click(object sender, EventArgs e)
        {
            CopyPreviousClick();
        }

        protected void btnCopyPrevious_ServerClick(object sender, EventArgs e)
        {
            CopyPreviousClick();
        }

        protected void btnhiddenConfirmOk_Click(object sender, EventArgs e)
        {
            if (hdnPreviousEncounterId.Value != null && string.IsNullOrEmpty(hdnPreviousEncounterId.Value))
            {
                return;// PutSessionExpiredMessage;;
            }

            ulong previousEncounterId = Convert.ToUInt64(hdnPreviousEncounterId.Value);

            EncounterManager objEncounterManager = new EncounterManager();

            ulong physicianId = 0;
            if (ClientSession.PhysicianId != null)
                physicianId = ClientSession.PhysicianId;

            if (ClientSession.FillEncounterandWFObject.EncRecord.Appointment_Provider_ID !=
                     ClientSession.FillEncounterandWFObject.EncRecord.Encounter_Provider_ID)
            {
                if (string.IsNullOrEmpty(hdnSelectPhysicianId.Value))
                {
                    return;
                }

                physicianId = Convert.ToUInt64(hdnSelectPhysicianId.Value);
                hdnSelectPhysicianId.Value = "";
            }

            var returnValue = objEncounterManager.CopyPreviousEncounter(ClientSession.EncounterId,
                                                                              ClientSession.HumanId,
                                                                              physicianId,
                                                                              ClientSession.UserName,
                                                                              previousEncounterId,
                                                                              ClientSession.UserRole,
                                                                              UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service));

            if (returnValue.StartsWith("ERROR"))
            {
                string sMessage = string.Format("copyPreviousClickError(\"{0}\");",
                    returnValue.Replace("ERROR~", string.Empty));
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrorMessage"
                                                  , sMessage, true);
                return;
            }

            var groupArray = returnValue.Split('~').Select(a => a.Trim()).ToList();

            List<string> lstContent = new List<string>();

            lstContent.Add("Chief Complaints");
            lstContent.Add("Review Of Systems");
            lstContent.Add("Review Of Systems General Notes");
            lstContent.Add("Review Of Systems General Notes ROS");
            lstContent.Add("Healthcare Questionnaire");
            lstContent.Add("Examination General With Speciality");
            lstContent.Add("Examination Focused");
            lstContent.Add("Assessment");
            lstContent.Add("Assessment Treatment Plan");
            lstContent.Add("Assessment GENERAL NOTES");
            lstContent.Add("Plan General");
            //Cap - 1357
            //CAP - 2043
            lstContent.Add("Plan Individual Care Plan");
            lstContent.Add("Plan Preventive Screening Plan");
            lstContent.Add("Assessment Problem List");

            List<string> lstMSG = new List<string>();
            List<string> lstSavedUpdated = new List<string>();

            List<string> lstSKIPitem = new List<string>();
            lstSKIPitem.Add("2");
            lstSKIPitem.Add("3");
            lstSKIPitem.Add("7");
            lstSKIPitem.Add("8");

            for (int i = 0; i < groupArray.Count; i++)
            {
                if (string.Compare(groupArray[i], "0", true) == 0)
                {
                    if (!lstSKIPitem.Contains(Convert.ToString(i)))
                    {
                        lstMSG.Add(lstContent[i]);
                    }
                }
                if (string.Compare(groupArray[i], "1", true) == 0)
                {
                    lstSavedUpdated.Add(lstContent[i]);// Writing XML                    
                }
            }
            lstSavedUpdated.Add("Encounter");
            WriteUpdatedXML(lstSavedUpdated);
            PromptMessage(lstMSG, lstSavedUpdated.Count > 0);
        }

        void PromptMessage(List<string> lstMSG, bool isUpdated)
        {
            var Message = string.Empty;

            if (lstMSG.Count > 0)
            {
                Message = string.Join(", ", lstMSG.ToArray());
            }

            if (!string.IsNullOrEmpty(Message))
            {
                var objEncounter = new EncounterManager().GetById(ClientSession.EncounterId);

                var jsMessage = string.Empty;

                if (isUpdated)
                {
                    jsMessage = " DisplayErrorMessage('160001');";
                }

                if (objEncounter != null)
                {
                    jsMessage += "PromptMessage('" + Message + "','" + objEncounter.Id + "','" + objEncounter.Date_of_Service + "');";
                }

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "RefreshSummaryBar"
                                                           , jsMessage, true);

            }
        }

        // Method to write xml for Seleted Tabs ~Ponmozhi Vendan T.
        GenerateXml obj = new GenerateXml();
        GenerateXml objhuman = new GenerateXml();
        Encounter objenc = new Encounter();
        List<Encounter> ilstenc = new List<Encounter>();
        IList<ROS> ilstROS = new List<ROS>();
        // Method to write xml for Seleted Tabs ~Ponmozhi Vendan T.
        void WriteUpdatedXML(List<string> lstSavedUpdated)
        {
            obj.itemDoc = obj.ReadBlob("Encounter", ClientSession.EncounterId);

            for (int i = 0; i < lstSavedUpdated.Count; i++)
            {
                var switch_on = lstSavedUpdated[i].ToUpper();

                switch (switch_on)
                {
                    case "ENCOUNTER":
                        UpdateEncounterTagXML();
                        break;
                    case "CHIEF COMPLAINTS":
                        WriteChiefComplaintsXML();
                        break;
                    case "REVIEW OF SYSTEMS":
                        WriteROSXML();
                        break;
                    case "REVIEW OF SYSTEMS GENERAL NOTES":
                        WriteROSGeneralNotesXML();
                        break;
                    case "REVIEW OF SYSTEMS GENERAL NOTES ROS":
                        WriteGeneralNotesROSXML();
                        break;
                    case "EXAMINATION GENERAL WITH SPECIALITY":
                        WriteExamGeneralWithSpecialityXML();
                        break;
                    case "EXAMINATION FOCUSED":
                        WriteFocusedExamXML();
                        break;
                    case "ASSESSMENT":
                        WriteAssesment();
                        break;
                    case "ASSESSMENT GENERAL NOTES":
                        WriteAssesmentGeneralNotes();
                        break;
                    case "ASSESSMENT PROBLEM LIST":
                        WriteAssesmentProblemList();
                        break;
                    case "ASSESSMENT TREATMENT PLAN":
                        WriteAssesmentTreatmentPlan();
                        break;
                    case "PLAN GENERAL":
                        WriteGeneralPlanXML();
                        break;
                    case "PLAN INDIVIDUAL CARE PLAN":
                        WriteIndividualCarePlanXML();
                        break;
                    case "PLAN PREVENTIVE SCREENING PLAN":
                        WritePreventiveScreenCarePlanXML();
                        break;
                    case "HEALTHCARE QUESTIONNAIRE":
                        WriteHealthcareQuestionnaireXML();
                        break;
                    default:
                        break;
                }
            }

            if (lstSavedUpdated.Count > 0)
            {
                try
                {
                    EncounterManager objencountermanager = new EncounterManager();
                    objencountermanager.writeCopypreviousencounterblobtable(ClientSession.EncounterId, obj, ilstenc);
                    //itemDoc.Save(strXmlFilePath);
                    if (ilstROS.Count > 0)
                    {

                        GeneralNotesManager objGeneralNotesManager = new GeneralNotesManager();
                        var ROSGenSysList = objGeneralNotesManager.GetGeneralNotes(ClientSession.EncounterId, "SYSTEM");
                        obj = new GenerateXml();
                        List<string> ilstSystemName = new List<string>();
                        ulong encounterid = 0;
                        ulong humanid = 0;

                        encounterid = ilstROS[0].Encounter_Id;

                        humanid = ilstROS[0].Human_ID;
                        for (int i = 0; i < ilstROS.Count; i++)
                        {
                            if (ilstROS[i].Status.TrimEnd(' ') != "")
                            {
                                ilstSystemName.Add(ilstROS[i].System_Name);
                            }
                        }
                        for (int i = 0; i < ROSGenSysList.Count; i++)
                        {
                            if (ROSGenSysList[i].Notes.TrimEnd(' ') != "")
                            {
                                ilstSystemName.Add(ROSGenSysList[i].Name_Of_The_Field);
                            }
                        }
                        ilstSystemName = ilstSystemName.Distinct().ToList();

                        // GenerateXml objxml = new GenerateXml();


                        //   if (File.Exists(strXmlFilePath) == true && encounterid > 0)
                        {
                            // XmlDocument itemDoc = new XmlDocument();


                            //  objxml.itemDoc = objxml.ReadBlob("Encounter", encounterId);

                            obj.itemDoc = obj.ReadBlob("Encounter", ClientSession.EncounterId);

                            XmlNodeList xmlsysCheck = obj.itemDoc.GetElementsByTagName("ROSSystemList");
                            if (xmlsysCheck[0] != null)
                            {


                                XmlNodeList ParentNodeList = obj.itemDoc.GetElementsByTagName("ROSSystemList");
                                XmlNodeList xmlModules = obj.itemDoc.GetElementsByTagName("Modules");
                                xmlModules[0].RemoveChild(ParentNodeList[0]);

                            }
                            if (xmlsysCheck[0] == null && ilstSystemName.Count > 0)
                            {
                                XmlNode xmlSystemNodeParent = obj.itemDoc.CreateNode(XmlNodeType.Element, "ROSSystemList", "");
                                XmlNodeList xmlModule = obj.itemDoc.GetElementsByTagName("Modules");
                                xmlModule[0].AppendChild(xmlSystemNodeParent);

                            }

                            XmlNode xmlSystemNode = null;
                            XmlAttribute attSysName = null;
                            XmlAttribute attEncounterid = null;
                            XmlAttribute atthuman_id = null;

                            if (ilstSystemName != null)
                            {
                                for (int i = 0; i < ilstSystemName.Count; i++)
                                {
                                    xmlSystemNode = obj.itemDoc.CreateNode(XmlNodeType.Element, "SystemName", "");

                                    attSysName = obj.itemDoc.CreateAttribute("System_Name");
                                    attSysName.Value = ilstSystemName[i];
                                    xmlSystemNode.Attributes.Append(attSysName);

                                    attEncounterid = obj.itemDoc.CreateAttribute("Encounter_ID");
                                    attEncounterid.Value = encounterid.ToString();
                                    xmlSystemNode.Attributes.Append(attEncounterid);

                                    atthuman_id = obj.itemDoc.CreateAttribute("Human_ID");
                                    atthuman_id.Value = humanid.ToString();
                                    xmlSystemNode.Attributes.Append(atthuman_id);

                                    XmlNodeList xmlsysList = obj.itemDoc.GetElementsByTagName("ROSSystemList");
                                    xmlsysList[0].AppendChild(xmlSystemNode);
                                }
                            }
                        }

                        objencountermanager.writeCopypreviousencounterblobtable(ClientSession.EncounterId, obj, ilstenc);

                    }
                }
                catch (Exception xmlexcep)
                {
                    throw xmlexcep;
                }

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SavedSuccessfully"
                              , "DisplayErrorMessage('160001');", true);
            }

        }

        /*Added to Update Encountertag on Copy_Prev_Enc Click in both EncounterXML and HUmanXML*/

        void UpdateEncounterTagXML()
        {
            var encounterId = ClientSession.EncounterId;

            EncounterManager objEncounterManager = new EncounterManager();
            objenc = objEncounterManager.GetById(encounterId);
            ilstenc = new List<Encounter>();
            if (objenc != null && objenc.Id != 0)
            {
                objenc.Local_Time = UtilityManager.ConvertToLocal(objenc.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                ilstenc.Add(objenc);
                List<object> lstobj = ilstenc.Cast<object>().ToList();

                if (lstobj != null)
                    obj.Copy_Previous_GenerateXmlSave(lstobj, encounterId, string.Empty, false, ref obj);
                //to update the EncounterTag in HumanXML

                var humanId = objenc.Human_ID;
                if (lstobj != null)
                {
                    objhuman.Copy_Previous_GenerateXmlSave(lstobj, humanId, string.Empty, true, ref objhuman);
                    EncounterManager objencountermanager = new EncounterManager();
                    objencountermanager.writeCopyprevioushumanblobtable(humanId, ilstenc, objhuman);
                }
            }

        }

        void WriteChiefComplaintsXML()
        {
            var encounterId = ClientSession.EncounterId;

            ChiefComplaintsManager objChiefComplaintsManager = new ChiefComplaintsManager();
            var ilstCC = objChiefComplaintsManager.GetComplaintsByEncID(encounterId, false);
            List<object> lstObj = null;
            if (ilstCC.Count() > 0)
                lstObj = ilstCC.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, string.Empty, false, ref obj);
        }

        void WriteROSXML()
        {
            var encounterId = ClientSession.EncounterId;

            ROSManager objROSManager = new ROSManager();
            ilstROS = objROSManager.ROSByEncounterId(encounterId, false);

            List<object> lstObj = null;
            if (ilstROS.Count() > 0)
                lstObj = ilstROS.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, string.Empty, false, ref obj);


            //For ROS system list while doing the overall copy previous




            // string FileName = "Encounter" + "_" + encounterId + ".xml";
            // string strXmlFilePath = string.Empty;
            // if (File.Exists(Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName)))
            // strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            //itemDoc.Save(strXmlFilePath);


            //try
            //{
            //    ROSManager objros = new ROSManager();
            //    objros.writesystem(encounterId, objxml);
            //    //itemDoc.Save(strXmlFilePath);
            //}
            //catch (Exception xmlexcep)
            //{
            //    throw xmlexcep;
            //}



        }

        void WriteROSGeneralNotesXML()
        {
            var encounterId = ClientSession.EncounterId;

            GeneralNotesManager objGeneralNotesManager = new GeneralNotesManager();
            var ilstAssesmentGeneralNotes = objGeneralNotesManager.GetGeneralNotes(encounterId, "SYSTEM");
            List<object> lstObj = null;
            if (ilstAssesmentGeneralNotes.Count() > 0)

                lstObj = ilstAssesmentGeneralNotes.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, "ROS", false, ref obj);
        }

        void WriteGeneralNotesROSXML()
        {
            var encounterId = ClientSession.EncounterId;

            GeneralNotesManager objGeneralNotesManager = new GeneralNotesManager();
            var ilstAssesmentGeneralNotes = objGeneralNotesManager.GetGeneralNotes(encounterId, "ROS General Notes");
            List<object> lstObj = null;
            if (ilstAssesmentGeneralNotes.Count() > 0)
                lstObj = ilstAssesmentGeneralNotes.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, "ROSGeneralNotes", false, ref obj);
        }

        void WriteExamGeneralWithSpecialityXML()
        {
            var encounterId = ClientSession.EncounterId;

            ExaminationManager objExaminationManager = new ExaminationManager();
            IList<Examination> ilstEXAM = objExaminationManager.GetExamList(encounterId, "GENERAL WITH SPECIALTY", false);
            List<object> lstObj = null;
            if (ilstEXAM.Count > 0)
                lstObj = ilstEXAM.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, "General", false, ref obj);
        }

        void WriteFocusedExamXML()
        {
            var encounterId = ClientSession.EncounterId;

            ExaminationManager objExaminationManager = new ExaminationManager();
            IList<Examination> ilstEXAM = objExaminationManager.GetExamList(encounterId, "FOCUSED", false);
            List<object> lstObj = null;
            if (ilstEXAM.Count > 0)
                lstObj = ilstEXAM.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, "Focused", false, ref obj);
        }

        void WriteGeneralPlanXML()
        {
            var encounterId = ClientSession.EncounterId;

            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();

            IList<TreatmentPlan> ilstGeneralPlan = objTreatmentPlanManager.GetTreatmentPlan(encounterId, "PLAN", false);
            List<object> lstObj = null;
            if (ilstGeneralPlan.Count > 0)
                lstObj = ilstGeneralPlan.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, "", false, ref obj);
        }

        void WriteIndividualCarePlanXML()
        {
            var encounterId = ClientSession.EncounterId;

            CarePlanManager objCarePlanManager = new CarePlanManager();

            var ilstCarePlan = objCarePlanManager.GetCarePlanByEncounter(encounterId);
            List<object> lstObj = null;

            if (ilstCarePlan.Count > 0)
                lstObj = ilstCarePlan.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, string.Empty, false, ref obj);
        }

        void WritePreventiveScreenCarePlanXML()
        {
            var encounterId = ClientSession.EncounterId;

            PreventiveScreenManager objPreventiveScreenManager = new PreventiveScreenManager();

            var ilstPreventiveScreenPlan = objPreventiveScreenManager.GetPreventiveScreenPlanDetails(encounterId,
                                                                                                     ClientSession.HumanId);
            List<object> lstObj = null;
            if (ilstPreventiveScreenPlan.Count() > 0)
                lstObj = ilstPreventiveScreenPlan.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, string.Empty, false, ref obj);

        }

        void WriteAssesment()
        {
            var encounterId = ClientSession.EncounterId;

            AssessmentManager objAssessmentManager = new AssessmentManager();
            var ilstAssesment = objAssessmentManager.GetAssessmentUsingEncounterID(encounterId)
                .Where(a => string.Compare(a.Assessment_Type, "SELECTED", true) == 0).ToList<Assessment>();

            List<object> lstObj = null;

            if (ilstAssesment.Count() != null)
                lstObj = ilstAssesment.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, string.Empty, false, ref obj);
        }

        void WriteAssesmentGeneralNotes()
        {
            var encounterId = ClientSession.EncounterId;

            GeneralNotesManager objGeneralNotesManager = new GeneralNotesManager();
            var ilstAssesmentGeneralNotes = objGeneralNotesManager.GetGeneralNotes(encounterId, "SELECTED ASSESSMENT");
            List<object> lstObj = null;

            if (ilstAssesmentGeneralNotes.Count() != null)
                lstObj = ilstAssesmentGeneralNotes.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, "Assessment", false, ref obj);

        }

        void WriteAssesmentProblemList()
        {
            var encounterId = ClientSession.EncounterId;

            ProblemListManager objProblemListManager = new ProblemListManager();
            var ilstProblemList = objProblemListManager.GetFromProblemList(encounterId);
            List<object> lstObj = null;
            if (ilstProblemList.Count() != null)
                lstObj = ilstProblemList.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, string.Empty, false, ref obj);
        }

        void WriteAssesmentTreatmentPlan()
        {
            var encounterId = ClientSession.EncounterId;

            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();

            IList<TreatmentPlan> ilstGeneralPlan = objTreatmentPlanManager.GetTreatmentPlan(encounterId, "ASSESSMENT", false);
            List<object> lstObj = null;

            if (ilstGeneralPlan.Count > 0)
                lstObj = ilstGeneralPlan.Cast<object>().ToList();
            if (lstObj != null)
                new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, "", false, ref obj);
        }
        //Added CopyPrev for Questionnaire for BugID:44324 - COPD Screening Feature
        void WriteHealthcareQuestionnaireXML()
        {
            var encounterId = ClientSession.EncounterId;
            IList<Healthcare_Questionnaire> prevEnclist = new List<Healthcare_Questionnaire>();
            HealthcareQuestionnaireManager objQuestionnaireManager = new HealthcareQuestionnaireManager();

            IList<string> Category_lst = new List<string>();
            prevEnclist = objQuestionnaireManager.GetHealthQuestionList(encounterId, out Category_lst);
            if (prevEnclist != null && prevEnclist.Count > 0)
            {
                foreach (string category in Category_lst)
                {
                    string sQuestionnaireTabName = string.Empty;
                    var ilstQuestionnaire = prevEnclist.Where(a => a.Questionnaire_Category == category).ToList<Healthcare_Questionnaire>();
                    sQuestionnaireTabName = category.Replace(" ", "").Replace("/", "");
                    List<object> lstObj = ilstQuestionnaire.Cast<object>().ToList();
                    new GenerateXml().Copy_Previous_GenerateXmlSave(lstObj, encounterId, sQuestionnaireTabName, false, ref obj);
                }
            }
        }
        //Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
        bool CheckIfEligibleForProviderReview(ulong Encounter_ID)
        {
            bool eligibleForReview = false;
            ProviderReviewTrackerManager provRevTrackerManager = new ProviderReviewTrackerManager();
            IList<ProviderReviewTracker> lstProvRevTrack = new List<ProviderReviewTracker>();
            lstProvRevTrack = provRevTrackerManager.GetReviewTrackerInfobyEncounterID(Encounter_ID);
            int ActiveReviews = 0;
            if (lstProvRevTrack != null && lstProvRevTrack.Count > 0)
            {
                hdnProvRev.Value = lstProvRevTrack.Count.ToString();
                ActiveReviews = lstProvRevTrack.Where(a => a.Status.ToUpper() == "ACTIVE").OrderByDescending(a => a.Created_Date_And_Time).Count();
            }

            if (ActiveReviews > 0)
                eligibleForReview = true;
            else if (lstProvRevTrack == null || lstProvRevTrack.Count == 0)
                eligibleForReview = true;
            return eligibleForReview;
        }

        protected void UpdateReviewTrackerforEncounter()
        {
            ProviderReviewTrackerManager provRevTrackerManager = new ProviderReviewTrackerManager();
            IList<ProviderReviewTracker> lstProvRevTrack = new List<ProviderReviewTracker>();
            ProviderReviewTracker objProvReviewTracker = new ProviderReviewTracker();
            lstProvRevTrack = provRevTrackerManager.GetReviewTrackerInfobyEncounterID(ClientSession.EncounterId);
            if (lstProvRevTrack != null && lstProvRevTrack.Count > 0)
            {
                objProvReviewTracker = lstProvRevTrack.OrderByDescending(a => a.Created_Date_And_Time).ToList<ProviderReviewTracker>()[0];
            }
            if (objProvReviewTracker != null && objProvReviewTracker.Id != 0)
            {
                if (chkProviderReview.Checked)
                    objProvReviewTracker.Status = "ACTIVE";
                else
                    objProvReviewTracker.Status = "INACTIVE";
                objProvReviewTracker.Modified_By = ClientSession.UserName;
                objProvReviewTracker.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                provRevTrackerManager.SaveUpdateProviderReviewTracker(null, objProvReviewTracker);
            }
            else if (lstProvRevTrack == null || lstProvRevTrack.Count == 0)
            {
                objProvReviewTracker.Human_ID = ClientSession.HumanId;
                objProvReviewTracker.Encounter_ID = ClientSession.EncounterId;
                objProvReviewTracker.User_Name = ClientSession.UserName;
                if (chkProviderReview.Checked)
                    objProvReviewTracker.Status = "ACTIVE";
                else
                    objProvReviewTracker.Status = "INACTIVE";
                objProvReviewTracker.Created_By = ClientSession.UserName;
                objProvReviewTracker.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                provRevTrackerManager.SaveUpdateProviderReviewTracker(objProvReviewTracker, null);
            }
        }

        protected void btnCorrections_ServerClick(object sender, EventArgs e)
        {
            string sHumanName = string.Empty;
            Human objhuman = new Human();
            HumanManager objHuamManager = new HumanManager();
            objhuman = objHuamManager.GetById(ClientSession.HumanId);
            if (objhuman != null && objhuman.Id != 0)
                sHumanName = objhuman.Last_Name + "," + objhuman.First_Name + " " + objhuman.MI + " " + objhuman.Suffix;

            string url = "";
            if (ClientSession.UserCurrentProcess.ToUpper().Trim() == "SCRIBE_CORRECTION" || ClientSession.UserCurrentProcess.ToUpper().Trim() == "SCRIBE_REVIEW_CORRECTION")
            {
                url = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + '0' +
                         "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + "&OPENING_FROM=ProcessEncounter" +
                          "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "true" + "&Scribe=" + "true";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ReviewCorrectionScreen", "OpenPrintDocReviewCorrection('" + url + "');", true);
            }
            else
            {
                url = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + '0' +
                    "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + "&OPENING_FROM=ProcessEncounter" +
                     "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "true";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ReviewCorrectionScreen", "OpenPrintDocReviewCorrection('" + url + "');", true);
            }
        }

        
    }
}