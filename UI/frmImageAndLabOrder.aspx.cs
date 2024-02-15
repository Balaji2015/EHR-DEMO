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
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web.UI;
using System.Drawing;
using Acurus.Capella.Core.DTO;
using System.Runtime.Serialization;
using System.Web.Services;
using System.IO;
using Acurus.Capella.DataAccess.com.labcorp.www4;
using Newtonsoft.Json;
using System.Xml;

//using Acurus.Capella.LabAgent;

namespace Acurus.Capella.UI
{
    public partial class frmImageAndLabOrder : System.Web.UI.Page
    {
        bool IsImport = false;
        bool IsCollectionDate = false;
        LabInsurancePlanManager objLabInsurancePlanManager = new LabInsurancePlanManager();
        IList<PhysicianProcedure> procedureList = new List<PhysicianProcedure>();
        EAndMCodingManager objEAndMCodingManager = new EAndMCodingManager();
        LabLocationManager objLabLocationManager = new LabLocationManager();
        StaticLookupManager objStaticLookupManager = new StaticLookupManager();
        OrdersManager objOrdersManager = new OrdersManager();
        AssessmentManager objAssessmentManager = new AssessmentManager();
        public IList<Assessment> AssessmentList = new List<Assessment>();
        public IList<ProblemList> ilstProblemList = new List<ProblemList>();
        IList<OrdersRequiredForms> ilstRequiredForms = new List<OrdersRequiredForms>();
        OrdersRequiredFormsManager objOrderRequiredMngr = new OrdersRequiredFormsManager();
        string OrderType = "DIAGNOSTIC ORDER";
        string sScreenMode = string.Empty;
        // public string sFacilityCmg = string.Empty;
        public bool IsDefaultLab = false;
        //bool TriggerClearAll = true;
        public Dictionary<string, string> AssessmentSource
        {
            get
            {
                return (Dictionary<string, string>)ViewState["AssessmentSource"] ?? new Dictionary<string, string>();
            }
            set
            {
                ViewState["AssessmentSource"] = value;
            }
        }
        public Dictionary<string, string> LookUpPerRequest = new Dictionary<string, string>();
        public Dictionary<string, string> LookUpValues
        {
            get
            {
                return (Dictionary<string, string>)ViewState["LookUpValues"] ?? new Dictionary<string, string>();
            }
            set
            {
                ViewState["LookUpValues"] = value;
            }
        }
        public String FacilityCity
        {
            get
            {
                return ViewState["FacilityCity"].ToString() ?? string.Empty;
            }
            set
            {
                ViewState["FacilityCity"] = value;
            }
        }
        public IList<string> ListViewHeader
        {
            get
            {
                return (IList<string>)ViewState["ListViewHeader"] ?? new List<string>();
            }
            set
            {
                ViewState["ListViewHeader"] = value;
            }
        }
        public ulong HumanID
        {
            get
            {
                return ViewState["HumanID"] == null ? 0 : Convert.ToUInt32(ViewState["HumanID"]);
            }
            set
            {
                ViewState["HumanID"] = value;
            }
        }
        public ulong EncounterID
        {
            get
            {
                return ViewState["EncounterID"] == null ? 0 : Convert.ToUInt32(ViewState["EncounterID"]);
            }
            set
            {
                ViewState["EncounterID"] = value;
            }
        }
        public ulong PhysicianID
        {
            get
            {
                return ViewState["PhysicianID"] == null ? 0 : Convert.ToUInt32(ViewState["PhysicianID"]);
            }
            set
            {
                ViewState["PhysicianID"] = value;
            }
        }
        public string sAppointment
        {
            get
            {
                return ViewState["FromAppointmnet"] == null ? "N" : ViewState["FromAppointmnet"].ToString();
            }
            set
            {
                ViewState["FromAppointmnet"] = value;
            }
        }
        public string sFromAppointmentFacility
        {
            get
            {
                return ViewState["FromAppointmentFacility"] == null ? "N" : ViewState["FromAppointmentFacility"].ToString();
            }
            set
            {
                ViewState["FromAppointmentFacility"] = value;
            }
        }
        public string sCmgorder = string.Empty;
        public ulong sCmgorderSubmitId = 0;

        public bool IsEditable;
        public bool IsNormalMode;
        public bool FirstTime;
        public bool DontThrowMsg;
        ArrayList errList = new ArrayList();

        public delegate bool OnCheckForValidation();
        OnCheckForValidation objOnCheckForValidation;
        //private void CreateControlsForLabOrder()
        //{
        //    this.gbProcedures.GroupingText = "Lab Procedures";
        //    this.gbOrderDetails.GroupingText = "Diagnostic Order Details";
        //    //lblLab.Text = "Lab*";
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Diagnostic Order" + " - " + ClientSession.UserName;
            txtOrderNotes.DName = "pbDropdown";
            hdnUserName.Value = ClientSession.UserName;
            IsEditable = true;
            LookUpPerRequest = LookUpValues;
            //MessageWindow.DestroyOnClose = true;
            //ClientSession.UserRole = "PHYSICIAN";
            //HumanID = 20201;
            //EncounterID = 82831;
            //PhysicianID = 105;
            //ClientSession.UserName = "RPRADHAN";
            //ClientSession.FacilityName = "1904 N OG AVE";
            //btnOrderSubmit.Enabled = false;


            string ClientSideObjects = string.Empty;
            ClientSideObjects += "var UserRole='" + ClientSession.UserRole + "';";
            this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "PageObjects", ClientSideObjects, true);
            sScreenMode = Request["ScreenMode"];
            lnkDiagnosticOrder.Attributes.Add("onclick", "WaitCursor();");
            lnkOrderList.Attributes.Add("onclick", "WaitCursor();");
            //CAP-1551
            //chklstFrequentlyUsedProcedures.Attributes.Add("onclick", "ChklstFrequentlyEnable();");
            // sFacilityCmg = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].Trim().ToUpper();
            var vfacAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
            IList<FacilityLibrary> ilstFacAncillary = vfacAncillary.ToList<FacilityLibrary>();

            //  if (ClientSession.FillEncounterandWFObject.EncRecord.Facility_Name == sFacilityCmg)
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                sCmgorder = "Y";
                lnkOrderList.Visible = false;
                //cboLab.Disabled = true;
                btnEditQuantity.Disabled = false;

            }
            else
            {
                if (Request["FromAppointmnet"] != null && Request["FromAppointmnet"].Trim() != string.Empty && Request["FromAppointmnet"].ToString().ToUpper() == "Y")
                {
                    sAppointment = Request["FromAppointmnet"].ToString();
                    sFromAppointmentFacility = Request["FromAppointmentFacility"].ToString();
                    lnkOrderList.Visible = false;
                    cboLab.Disabled = true;
                    sCmgorder = "N";
                    //cboReadingProvider.Disabled = true;
                    //lblReadingProvider.InnerText.Replace("*", "");
                    //lblReadingProvider.Attributes.Add("style", "color:black");
                    btnEditQuantity.Disabled = true;
                }
                else
                {
                    sCmgorder = "N";
                    lnkOrderList.Visible = true;
                    cboLab.Disabled = false;
                    btnEditQuantity.Disabled = true;
                }
            }

            if (!IsPostBack)
            {
                rbImageOrder.Attributes.Add("onClick", "return RadioClick(this);");
                rbLabOrder.Attributes.Add("onClick", "return RadioClick(this);");
                hdnToFindSource.Value = string.Empty;
                ulong id = 0;
                if (Request["FromAppointmnet"] != null && Request["FromAppointmnet"].Trim() != string.Empty && Request["FromAppointmnet"].ToString().ToUpper() == "Y")
                {
                    sAppointment = Request["FromAppointmnet"].ToString();
                    sFromAppointmentFacility = Request["FromAppointmentFacility"].ToString();
                    lnkOrderList.Visible = false;
                    cboLab.Disabled = true;
                }
                else
                {
                    //if (ClientSession.UserRole.ToUpper() != "CODER")
                    //{
                    //Cap - 942
                    ClientSession.processCheck = true;
                    SecurityServiceUtility obj = new SecurityServiceUtility();
                    obj.ApplyUserPermissions(this.Page);
                    //}
                    lnkOrderList.Visible = true;
                    //cboLab.Disabled = false;
                }

                if (ClientSession.UserPermission == "R")
                {
                    dbInsurance.ImageUrl = "~/Resources/Database Disable.png";
                    //pbSelectICD.ImageUrl = "~/Resources/Database Disable.png";
                    dbInsurance.Enabled = false;
                    pbSelectICD.Disabled = true; //pbSelectICD.Enabled = false;
                    cstdtpTestDate.Disabled = true; //cstdtpTestDate.Enabled = false;
                    gbOrderDetails.Enabled = false;
                    tblSelectProcedure.Disabled = true; //tblSelectProcedure.Enabled = false;
                }
                else
                {
                    dbInsurance.ImageUrl = "~/Resources/Database Inactive.jpg";
                    //pbSelectICD.ImageUrl = "~/Resources/Database Inactive.jpg";
                    dbInsurance.Enabled = true;
                    pbSelectICD.Disabled = false; //pbSelectICD.Enabled = true;
                    cstdtpTestDate.Disabled = false; //cstdtpTestDate.Enabled = true;
                }
                if (Request["HumanID"] != null && Request["HumanID"].Trim() != string.Empty)
                {
                    HumanID = Convert.ToUInt32(Request["HumanID"]);
                }
                else
                {
                    HumanID = ClientSession.HumanId;
                }
                if (Request["EncounterID"] != null && Request["EncounterID"].Trim() != string.Empty)
                {
                    EncounterID = Convert.ToUInt32(Request["EncounterID"]);
                }
                else
                {
                    if (Request["ScreenMode"] != null && Request["ScreenMode"].Trim() != string.Empty && Request["ScreenMode"].ToString().ToUpper() == "MENU")
                        EncounterID = 0;
                    else
                        EncounterID = ClientSession.EncounterId;
                }
                if (Request["PhysicianID"] != null && Request["PhysicianID"].Trim() != string.Empty)
                {
                    PhysicianID = Convert.ToUInt32(Request["PhysicianID"]);
                    ClientSession.PhysicianId = Convert.ToUInt32(Request["PhysicianID"]);
                }
                else
                {
                    PhysicianID = ClientSession.PhysicianId;
                }
                if (Request["ScreenMode"] != null && Request["ScreenMode"].Trim() != string.Empty && Request["ScreenMode"].ToString() == "Menu")
                {
                    Session["Physician_ID"] = PhysicianID;
                }
                else
                    Session["Physician_ID"] = null;

                if (Request["ScreenMode"] != null && Request["ScreenMode"].Trim() != string.Empty && Request["ScreenMode"].ToString() == "MyQ")
                {
                    btnMoveToNextProcess.Visible = true;
                }
                else
                {
                    btnMoveToNextProcess.Visible = false;
                }
                if (Request["FilledInPaperForm"] != null && Request["FilledInPaperForm"] == "true")
                {
                    chkpaperorder.Disabled = true; //chkpaperorder.Enabled = false;
                    rbImageOrder.Enabled = false;
                    rbLabOrder.Enabled = false;
                }
                if (Session["Physician_ID"] != null)
                    PhysicianID = Convert.ToUInt32(Session["Physician_ID"]);
                //if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
                //{
                //    tblSelectProcedure.Enabled = false;
                //    //btnPlan.Enabled = false;
                //}



                // if (ClientSession.FillEncounterandWFObject.EncRecord.Facility_Name == sFacilityCmg)
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y" && EncounterID != 0)
                {
                    //cboReadingProvider.Disabled = false;
                    //lblReadingProvider.InnerText.Replace("*", "");
                    //lblReadingProvider.InnerText += "*";
                    //lblReadingProvider.Attributes.Add("style", "color:red");
                    lnkOrderList.Visible = false;
                    cboLab.Disabled = true;
                    btnOrderSubmit.Disabled = true;
                    sCmgorder = "Y";
                    sCmgorderSubmitId = Convert.ToUInt32(ClientSession.FillEncounterandWFObject.EncRecord.Order_Submit_ID);
                    OrdersManager objOrdMngr = new OrdersManager();
                    string sSelectedOrder = string.Empty;
                    string sLocalTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    //id = objOrdMngr.InsertToOutstandingOrders(sCmgorderSubmitId, EncounterID, OrderType, string.Empty, ilstRequiredForms, ref sSelectedOrder, ClientSession.UserName, sFacilityCmg);
                    id = objOrdMngr.InsertToOutstandingOrders(sCmgorderSubmitId, EncounterID, OrderType, string.Empty, ilstRequiredForms, ref sSelectedOrder, ClientSession.UserName, ilstFacAncillary[0].Fac_Name, sLocalTime);
                    IList<PhysicianLibrary> PhyListAll = new List<PhysicianLibrary>();
                    //  PhyListAll = UtilityManager.GetPhysicianList("");
                    string sPhysicianXmlPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\ConfigXML\\PhysicianAddressDetails.xml";
                    XmlDocument itemPhysiciandoc = new XmlDocument();
                    XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
                    itemPhysiciandoc.Load(XmlPhysicianText);
                    IList<ListItem> liComboItems = new List<ListItem>();
                    for (int i = 0; i < itemPhysiciandoc.ChildNodes[1].ChildNodes.Count; i++)
                    {
                        if (itemPhysiciandoc.ChildNodes[1].ChildNodes[i].Attributes[13].Value.ToUpper() == "RENDERING" || itemPhysiciandoc.ChildNodes[1].ChildNodes[i].Attributes[13].Value.ToUpper() == "READING")
                        {
                            string sPhyName = itemPhysiciandoc.ChildNodes[1].ChildNodes[i].Attributes[10].Value + " " + itemPhysiciandoc.ChildNodes[1].ChildNodes[i].Attributes[11].Value + " " + itemPhysiciandoc.ChildNodes[1].ChildNodes[i].Attributes[12].Value;
                            ListItem item = new ListItem();
                            item.Value = itemPhysiciandoc.ChildNodes[1].ChildNodes[i].Attributes[14].Value;
                            item.Text = sPhyName;
                            liComboItems.Add(item);
                            //cboReadingProvider.Items.Add(item);
                        }
                    }
                    //IList<ListItem> sortlst = liComboItems.OrderBy(x => x.Text.Split(new string[] { "Dr." }, StringSplitOptions.None).Length > 1 ? x.Text.Split(new string[] { "Dr." }, StringSplitOptions.None)[1] : x.Text).ToList();
                    //cboReadingProvider.Items.AddRange(sortlst.ToArray());
                    //IList<PhysicianLibrary> PhyList = (from p in PhyListAll select p).Distinct().ToList();
                    //cboReadingProvider.Items.Add(new ListItem(""));
                    //for (int i = 0; i < PhyList.Count; i++)
                    //{
                    //    string sPhyName = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyLastName;
                    //    ListItem item = new ListItem();
                    //    item.Value = PhyList[i].Id.ToString();
                    //    item.Text = sPhyName;
                    //    cboReadingProvider.Items.Add(item);
                    //    //if (PhyList[i].Id.ToString() == ClientSession.PhysicianId.ToString())
                    //    //{
                    //    //    cboReadingProvider.SelectedIndex = i + 1;
                    //    //}
                    //}
                }
                else
                {
                    if (Request["FromAppointmnet"] != null && Request["FromAppointmnet"].Trim() != string.Empty && Request["FromAppointmnet"].ToString().ToUpper() == "Y")
                    {
                        sAppointment = Request["FromAppointmnet"].ToString();
                        sFromAppointmentFacility = Request["FromAppointmentFacility"].ToString();
                        lnkOrderList.Visible = false;
                        cboLab.Disabled = true;
                        sCmgorder = "N";
                        //cboReadingProvider.Disabled = true;
                        //lblReadingProvider.InnerText.Replace("*", "");
                        //lblReadingProvider.Attributes.Add("style", "color:black");
                        btnEditQuantity.Disabled = true;
                    }
                    else
                    {
                        sCmgorder = "N";
                        //cboReadingProvider.Disabled = true;
                        //lblReadingProvider.InnerText.Replace("*", "");
                        //lblReadingProvider.Attributes.Add("style", "color:black");
                        lnkOrderList.Visible = true;
                        //cboLab.Disabled = false;
                        btnEditQuantity.Disabled = true;
                    }

                }


                hdnHumanID_EncounterID_PhysicianID.Value = HumanID.ToString() + "," + EncounterID.ToString() + "," + PhysicianID.ToString();
                //if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" || ClientSession.UserRole.ToUpper() == "TECHNICIAN")
                //{
                //    chkMoveToMA.Visible = false;
                //}

                LookUpPerRequest.Add("procedureType", "LAB PROCEDURE");
                LookUpPerRequest.Add("LabType", "LAB");
                FillHumanDTO objFillHumnaDTO = new FillHumanDTO();

                //objFillHumnaDTO = objOrdersManager.GetHumanById(HumanID);

                //objFillHumnaDTO = objOrdersManager.PatientInsuredBag(HumanID);
                DiagnosticDTO objDiagnosticDTO = new DiagnosticDTO();


                // if (ClientSession.FillEncounterandWFObject.EncRecord.Facility_Name != sFacilityCmg)
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
                {
                    if (Request["IsFrontScreen"] != null && Request["IsFrontScreen"] == "Y")
                        id = Convert.ToUInt32(Request["EditedOrderSubmitID"].ToString());
                    else if (Request["EditedOrderSubmitID"] != null && Request["EditedOrderSubmitID"] != "0")
                        id = Convert.ToUInt32(Request["EditedOrderSubmitID"].ToString());

                }


                //objDiagnosticDTO = objOrdersManager.FillDiagnosticDTO(id, EncounterID, HumanID);
                if (LookUpPerRequest.Keys.Contains("procedureType") == true)
                    objDiagnosticDTO = objOrdersManager.FillDiagnosticDTO(id, EncounterID, HumanID, PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), ClientSession.FacilityName, ClientSession.LegalOrg);


                //if (ClientSession.FillEncounterandWFObject.EncRecord.Facility_Name == sFacilityCmg && sCmgorder == "Y")
                //{
                //    ListItem itemlst = new ListItem();
                //    itemlst = cboReadingProvider.Items.FindByValue(objDiagnosticDTO.objOrdersSubmit.Physician_ID.ToString());
                //    cboReadingProvider.SelectedIndex = cboReadingProvider.Items.IndexOf(itemlst);
                //}

                objFillHumnaDTO = objDiagnosticDTO.objFillHumnaDTO;
                Session["objFillHumanDTO"] = objFillHumnaDTO;
                Session["objDiagnosticDTO"] = objDiagnosticDTO;
                //ulong PrimaryInsurance = 0;
                //if (objFillHumnaDTO.PatientInsuredBag != null)
                //    PrimaryInsurance = objFillHumnaDTO.PatientInsuredBag.Where(a => a != null && a.Insurance_Type != null && a.Insurance_Type.StartsWith("PRIMARY")).Select(a => a.Insurance_Plan_ID).SingleOrDefault();
                //if (PrimaryInsurance != 0)
                //{
                //    IList<ulong> PrimaryInsuranceList = new List<ulong>();
                //    PrimaryInsuranceList.Add(PrimaryInsurance);
                //    IList<ulong> LabIdBasedOnInsID = new List<ulong>();
                //    LabIdBasedOnInsID = objLabInsurancePlanManager.GetLabIDBasedOnInsPlanID(PrimaryInsuranceList.ToArray<ulong>(), LookUpPerRequest["LabType"]);
                //    string LabIDList = string.Empty;
                //    foreach (ulong ul in LabIdBasedOnInsID)
                //    {
                //        LabIDList += ul + ",";
                //    }
                //    if (LabIDList != string.Empty)
                //    {
                //        LookUpPerRequest.Add("LabIdBasedOnIns", LabIDList.Substring(0, LabIDList.Length - 1));
                //    }
                //    else
                //    {
                //        LookUpPerRequest.Add("LabIdBasedOnIns", string.Empty);
                //    }
                //}
                //else
                //{
                //    LookUpPerRequest.Add("LabIdBasedOnIns", string.Empty);
                //}

                //Commentted for performance
                IList<ulong> LabIdBasedOnInsID = new List<ulong>();
                if (sAppointment == "Y")
                {
                    var vfacAppointmentAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == sFromAppointmentFacility select f;
                    IList<FacilityLibrary> ilstFacAppointmentAncillary = vfacAppointmentAncillary.ToList<FacilityLibrary>();

                    //  if (ClientSession.FillEncounterandWFObject.EncRecord.Facility_Name == sFacilityCmg)
                    if (ilstFacAppointmentAncillary.Count > 0 && ilstFacAppointmentAncillary[0].Is_Ancillary == "Y")
                    {
                        XDocument xmlLab = XDocument.Load(Server.MapPath(@"ConfigXML\LabList.xml"));

                        //IEnumerable<XElement> xml = xmlLab.Element("LabList")
                        //    .Elements("Lab")
                        //    .OrderBy(s => (string)s.Attribute("name"));
                        IEnumerable<XElement> xml = xmlLab.Element("LabList")
                           .Elements("Lab").Where(a => a.Attribute("type").Value.ToString() != "DME" && a.Attribute("name").Value.ToString() == ilstFacAppointmentAncillary[0].Short_Name)
                           .OrderBy(s => (int)s.Attribute("sort_order"));

                        if (xml != null)
                        {
                            foreach (XElement LabElement in xml)
                            {
                                string xmlValue = LabElement.Attribute("id").Value;
                                LabIdBasedOnInsID.Add(Convert.ToUInt32(xmlValue));
                            }
                        }
                    }
                }
                else
                {
                    if (sCmgorder == "Y")
                    {
                        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                        {
                            XDocument xmlLab = XDocument.Load(Server.MapPath(@"ConfigXML\LabList.xml"));

                            //IEnumerable<XElement> xml = xmlLab.Element("LabList")
                            //    .Elements("Lab")
                            //    .OrderBy(s => (string)s.Attribute("name"));
                            IEnumerable<XElement> xml = xmlLab.Element("LabList")
                               .Elements("Lab").Where(a => a.Attribute("type").Value.ToString() != "DME" && a.Attribute("name").Value.ToString() == ilstFacAncillary[0].Short_Name)
                               .OrderBy(s => (int)s.Attribute("sort_order"));

                            if (xml != null)
                            {
                                foreach (XElement LabElement in xml)
                                {
                                    string xmlValue = LabElement.Attribute("id").Value;
                                    LabIdBasedOnInsID.Add(Convert.ToUInt32(xmlValue));
                                }
                            }
                        }
                    }
                    else
                        LabIdBasedOnInsID = objDiagnosticDTO.PrimaryInsuranceList;
                }

                string LabIDList = string.Empty;
                foreach (ulong ul in LabIdBasedOnInsID)
                {
                    LabIDList += ul + ",";
                }
                if (LabIDList != string.Empty)
                {
                    LookUpPerRequest.Add("LabIdBasedOnIns", LabIDList.Substring(0, LabIDList.Length - 1));
                }
                else
                {
                    LookUpPerRequest.Add("LabIdBasedOnIns", string.Empty);
                }
                //End

                txtLocation.Value = string.Empty;
                // CreateControlsForLabOrder();
                if (Request["hdnForEditErrorMsg"] != null)
                    hdnForEditErrorMsg.Value = Request["hdnForEditErrorMsg"];
                //LoadLookUpValues(objDiagnosticDTO.FieldLookUpList);
                var vfacilityAncillary = from f in ApplicationObject.facilityLibraryList where f.Short_Name != string.Empty select f;
                IList<FacilityLibrary> ilstFacilityAncillary = vfacilityAncillary.ToList<FacilityLibrary>();

                string sShortName = string.Empty;
                for (int iCount = 0; iCount < ilstFacilityAncillary.Count; iCount++)
                {
                    sShortName += ilstFacilityAncillary[iCount].Short_Name + "|";
                }
                LookUpPerRequest.Add("CMGLabNameFromLookUp", sShortName);

                LoadLookUpValues();
                btnSelectLocation.Disabled = true; //btnSelectLocation.Enabled = false;
                btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
                //dtpCollectionDate.DateInput.DateFormat = "dd-MMM-yyyy";
                //cstdtpTestDate.Value = Convert.ToDateTime(cstdtpTestDate.Value).ToString("dd-MMM-yyyy"); //cstdtpTestDate.DateInput.DateFormat = "dd-MMM-yyyy";

                //ProblemListManager probMgr = new ProblemListManager();
                //ilstProblemList = probMgr.GetProblemList(HumanID);
                //AssessmentList = objAssessmentManager.GetAssessmentPerEncounterIDForOrders(EncounterID);

                FillAssesmentAndProblemListICD(objDiagnosticDTO.AssessmentList, objDiagnosticDTO.MedAdvProblemList);
                chkTestDateInDate.Checked = true;
                chkTestDateInDate_CheckedChanged(sender, e);
                hdnPhysicianID.Value = PhysicianID.ToString();
                if (hdnLocalTime.Value.Trim() != string.Empty)
                    cstdtpTestDate.Value = Convert.ToDateTime(hdnLocalTime.Value).ToString("dd-MMM-yyyy");
                SelectedTime(DateTime.Now);
                btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
                btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
                if (Request["IsFrontScreen"] != null && Request["IsFrontScreen"] == "Y")
                {
                    hdnTransferVaraible.Value = Request["EditedOrderSubmitID"].ToString();
                    lnkOrderList.NavigateUrl = "~/frmOrdersList.aspx?HumanID=" + HumanID.ToString() + "&EncounterID=" + EncounterID.ToString() + "&PhysicianID=" + PhysicianID.ToString() + "&EditedOrderSubmitID=" + Request["EditedOrderSubmitID"].ToString() + "&IsFrontScreen=" + "Y";
                    //LoadScreenFromOrderList();
                    LoadScreenFromOrderList(objDiagnosticDTO);
                    btnMoveToNextProcess.Disabled = false; //btnMoveToNextProcess.Enabled = true;
                }
                else if (Request["EditedOrderSubmitID"] != null && Request["EditedOrderSubmitID"] != "0")
                {
                    hdnOrderSubmitID.Value = Request["EditedOrderSubmitID"].ToString();
                    Session["OrderSubmitId"] = hdnOrderSubmitID.Value;
                    //LoadScreenFromOrderList();
                    LoadScreenFromOrderList(objDiagnosticDTO);
                    btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = true;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Keys", "if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null)window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true", true);
                }
                else if (sCmgorder == "Y")
                {
                    hdnOrderSubmitID.Value = id.ToString();
                    Session["OrderSubmitId"] = hdnOrderSubmitID.Value;
                    //LoadScreenFromOrderList();
                    LoadScreenFromOrderList(objDiagnosticDTO);
                    btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = true;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Keys", "if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null)window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true", true);
                }

                if (cboLab.Items[cboLab.SelectedIndex].Text != string.Empty && Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value) == 32 && chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected))
                {
                    btnImportresult.Disabled = false; //btnImportresult.Enabled = true;
                    btnOrderSubmit.Disabled = false; //btnOrderSubmit.Enabled = true;
                }
                else
                    btnImportresult.Disabled = true; //btnImportresult.Enabled = false;

                if (hdnToFindSource.Value == "false")
                {
                    btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
                    btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
                }
                //By Mohan BugID=26088 
                txtOrderNotes.txtDLC.Attributes.Add("onchange", "EnableSaveDiagnosticOrder();");
                txtOrderNotes.txtDLC.Attributes.Add("onkeypress", "EnableSaveDiagnosticOrder();");
                txtOrderNotes.txtDLC.Attributes.Add("onkeydown", "insertTab(this,event);");

                rbImageOrder.Enabled = false;
                rbLabOrder.Enabled = false;
                //Added By Suvarnni for Bud id=28559
                if ((ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" || ClientSession.UserRole.ToUpper() == "PHYSICIAN"))
                {
                    if (cboBillType.Items[cboBillType.SelectedIndex].Text == "Third Party")
                    {
                        dbInsurance.ImageUrl = "~/Resources/Database Disable.png";
                        dbInsurance.Enabled = false;
                    }
                    else
                    {
                        dbInsurance.ImageUrl = "~/Resources/Database Inactive.jpg";
                        dbInsurance.Enabled = true;
                    }
                }
                if (ClientSession.UserCurrentProcess == "MA_REVIEW" && Request["ScreenMode"] == "MyQ" && Request["ScreenMode"] != null)
                {
                    if (Request["Enable"] != "true")
                    {
                        gbCenterDetail.Enabled = false;
                        gbSelectICD.Enabled = false;
                        gbProcedures.Enabled = false;
                        gbOrderDetails.Enabled = false;
                        gbSpecimenDetails.Enabled = false;
                        btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
                        btnClearAll.Disabled = true; //btnClearAll.Enabled = false;
                    }
                }

            }
            else
            {
                if (hdnManageFreqUsed.Value == "true")
                {
                    ulong selectedLabID = 0;
                    string sSelectedItem = string.Empty;


                    //if (chklstFrequentlyUsedProcedures.SelectedItem != null)
                    //{
                    //    sSelectedItem = chklstFrequentlyUsedProcedures.SelectedItem.ToString();
                    //}
                    IList<ListItem> SelectedProcedures = (from p in chklstFrequentlyUsedProcedures.Items.Cast<ListItem>() where p.Selected select p).ToList<ListItem>();

                    if (cboLab.Items[cboLab.SelectedIndex].Value != string.Empty)
                        selectedLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                    //if (procedureList.Count > 0)
                    //{
                    //    if (procedureList[0].Lab_ID != selectedLabID)
                    //    {
                    if (LookUpPerRequest.Keys.Contains("procedureType") == true)
                        procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), selectedLabID, ClientSession.LegalOrg);
                    //    }
                    //}
                    FillLabProcedure(procedureList, new List<string>());
                    if (SelectedProcedures.Count > 0)
                    {
                        foreach (ListItem i in SelectedProcedures)
                        {
                            //chklstFrequentlyUsedProcedures.Items.FindByText(i.Text).Selected = true;
                            if (chklstFrequentlyUsedProcedures.Items.FindByText(i.Text) != null)
                                chklstFrequentlyUsedProcedures.Items.FindByText(i.Text).Selected = chklstFrequentlyUsedProcedures.Items.FindByText(i.Text) != null ? true : false;
                        }
                    }
                    //if (sSelectedItem != "")
                    //{
                    //    for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
                    //    {
                    //        if (chklstFrequentlyUsedProcedures.Items[i].Text == sSelectedItem)
                    //        {
                    //            chklstFrequentlyUsedProcedures.SelectedIndex = i;
                    //            //chklstFrequentlyUsedProcedures.SelectedValue = sSelectedItem;
                    //        }
                    //    }
                    //}
                }

                chkTestDateInWords_CheckedChanged(sender, e);
            }
            //if (ClientSession.UserRole.ToUpper() != "PHYSICIAN")
            //{
            //    dbInsurance.ImageUrl = "~/Resources/Database Disable.png";
            //    dbInsurance.Enabled = false;
            //} oommented this condition for BUG ID: 27985

            if (hdnCollectionDateIsMand.Value == "true")
            {
                if (!lblCollectionDate.InnerText.Contains("*"))
                    lblCollectionDate.InnerText += "*";
                lblCollectionDate.InnerHtml = lblCollectionDate.InnerText;
                lblCollectionDate.InnerHtml = lblCollectionDate.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                //lblCollectionDate.Attributes.Add("style", "color:red");
                lblCollectionDate.Attributes["Class"] = "";
                lblCollectionDate.Attributes["Class"] = "MandLabelstyle";
            }
            else
            {
                lblCollectionDate.InnerText = "Collection Date";// lblCollectionDate.InnerText.Replace("*", string.Empty);
                lblCollectionDate.Attributes.Add("style", "color:black");
                lblCollectionDate.Attributes["Class"] = "";
                lblCollectionDate.Attributes["Class"] = "spanstyle";
            }
            //chkSpecimenInHouseAlterText();
            //txtQuantity_TextChanged();

            objOnCheckForValidation = new OnCheckForValidation(CheckForValidation);


            //txtOrderNotes.SetTheUBACForDynamicControls();

            if (Request.Form["__EVENTTARGET"] == "pbSelectICD")
            {
                pbSelectICD_Click(this, new ImageClickEventArgs(10, 10));
            }

            // btnImportresult.Enabled = true;
            if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" || ClientSession.UserRole.ToUpper() == "SCRIBE")
            {
                tblSelectProcedure.Disabled = true; //tblSelectProcedure.Enabled = false;
                //btnPlan.Enabled = false;
            }
            //Code Modified By balaji.TJ
            //Cap - 942
            //if (ClientSession.UserRole.ToUpper() == "CODER")
            if (ClientSession.UserRole.ToUpper() == "CODER" || ClientSession.UserCurrentProcess != "PROVIDER_REVIEW" || ClientSession.UserCurrentProcess != "PROVIDER_REVIEW_2")
            {
                tblSelectProcedure.Disabled = true; //tblSelectProcedure.Enabled = false;
            }
            else
            {
                chklstFrequentlyUsedProcedures.Attributes.Add("onclick", "SelectItemsUnderHeader(this);");
            }
            if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" || ClientSession.UserRole.ToUpper() == "TECHNICIAN")
            {
                MoveToMA.Visible = false; //chkMoveToMA.Visible = false;
                //chkMoveToMA.Checked = false;
            }
            if (Request["ScreenMode"] != null && Request["ScreenMode"].Trim() != string.Empty && Request["ScreenMode"].ToString() == "MyQ")
            {
                lnkOrderList.NavigateUrl = "frmOrdersList.aspx?ScreenMode=" + Request["ScreenMode"].ToString();
                lnkDiagnosticOrder.NavigateUrl = "frmImageAndLabOrder.aspx?ScreenMode=" + Request["ScreenMode"].ToString();
                btnMoveToNextProcess.Visible = true;
            }
            else
            {
                if (Request["ScreenMode"] != null && Request["ScreenMode"].Trim() != string.Empty && Request["ScreenMode"].ToString().ToUpper() == "MENU")
                {
                    lnkOrderList.NavigateUrl = "frmOrdersList.aspx?ScreenMode=" + Request["ScreenMode"].ToString();
                    lnkDiagnosticOrder.NavigateUrl = "frmImageAndLabOrder.aspx?ScreenMode=" + Request["ScreenMode"].ToString();
                }
                btnMoveToNextProcess.Visible = false;
            }
            if (chkpaperorder.Checked == true)
            {
                rbLabOrder.Enabled = true;
                rbImageOrder.Enabled = true;
                if (hdnpaper.Value == "rbImageOrder")
                    rbImageOrder.Checked = true;
                else if (hdnpaper.Value == "rbLabOrder")
                    rbLabOrder.Checked = true;
            }
            else
            {
                rbImageOrder.Checked = false;
                rbLabOrder.Checked = false;
                rbLabOrder.Enabled = false;
                rbImageOrder.Enabled = false;
            }
            if (ClientSession.UserCurrentProcess == "MA_REVIEW" && Request["ScreenMode"] == "MyQ" && Request["ScreenMode"] != null)
            {
                if (Request["Enable"] != "true")
                {
                    gbCenterDetail.Enabled = false;
                    gbSelectICD.Enabled = false;
                    gbProcedures.Enabled = false;
                    gbOrderDetails.Enabled = false;
                    gbSpecimenDetails.Enabled = false;
                    btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
                    btnClearAll.Disabled = true; //btnClearAll.Enabled = false;
                }
            }
            txtQuantity_TextChanged();
            //if (chklstFrequentlyUsedProcedures.Items.Count > 0)
            //{
            //    if (chklstFrequentlyUsedProcedures.SelectedItem.Selected == true)
            //    {
            //        btnImportresult.Enabled = true;
            //    }
            //}
            if (cboLab.Items[cboLab.SelectedIndex].Text != "")
                btnSelectLocation.Disabled = false; //btnSelectLocation.Enabled = true;
            else
                btnSelectLocation.Disabled = true; //btnSelectLocation.Enabled = false;
            //Cap - 942
            //if (ClientSession.UserRole.ToUpper() == "CODER")
            if (ClientSession.UserRole.ToUpper() == "CODER" || ClientSession.UserCurrentProcess != "PROVIDER_REVIEW" || ClientSession.UserCurrentProcess != "PROVIDER_REVIEW_2")
            {
                //Cap - 942
                ClientSession.processCheck = true;
                SecurityServiceUtility obj1 = new SecurityServiceUtility();
                obj1.ApplyUserPermissions(this.Page);
            }

            if (ClientSession.UserCurrentProcess == "DISABLE")
            {
                btnSelectLocation.Disabled = true;
                pbSelectICD.Disabled = true;
            }
        }
        void SetCollectionDateMand(bool IsMand)
        {
            if (IsMand == false)
            {
                lblCollectionDate.InnerText = "Collection Date";//lblCollectionDate.InnerText.Replace("*", string.Empty);
                lblCollectionDate.Attributes.Add("style", "color:black");
                hdnCollectionDateIsMand.Value = "false";
                lblCollectionDate.Attributes["Class"] = "";
                lblCollectionDate.Attributes["Class"] = "spanstyle";
            }
            else if (IsMand == true)
            {
                if (chkSpecimenInHouse.Checked == true && chkMoveToMA.Checked == false)
                {
                    lblCollectionDate.InnerText = lblCollectionDate.InnerText.Replace("*", string.Empty);
                    lblCollectionDate.InnerText += "*";
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerText;
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                    //lblCollectionDate.Attributes.Add("style", "color:red"); ;
                    lblCollectionDate.Attributes["Class"] = "";
                    lblCollectionDate.Attributes["Class"] = "MandLabelstyle";
                }
                if (chkSpecimenInHouse.Checked == true && chkMoveToMA.Checked == true)
                {
                    if (chkSpecimenInHouse.Checked == true && MoveToMA.Visible == false)//chkMoveToMA.Visible == false)
                    {
                        lblCollectionDate.InnerText = lblCollectionDate.InnerText.Replace("*", string.Empty);
                        lblCollectionDate.InnerText += "*";
                        lblCollectionDate.InnerHtml = lblCollectionDate.InnerText;
                        lblCollectionDate.InnerHtml = lblCollectionDate.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                        //lblCollectionDate.Attributes.Add("style", "color:red");
                        lblCollectionDate.Attributes["Class"] = "";
                        lblCollectionDate.Attributes["Class"] = "MandLabelstyle";
                    }
                    else
                    {
                        lblCollectionDate.InnerText = "Collection Date";//lblCollectionDate.InnerText.Replace("*", string.Empty);
                        lblCollectionDate.Attributes.Add("style", "color:black");
                        hdnCollectionDateIsMand.Value = "false";
                        lblCollectionDate.Attributes["Class"] = "";
                        lblCollectionDate.Attributes["Class"] = "spanstyle";
                    }

                }
                else if (chkSpecimenInHouse.Checked == true)
                {
                    lblCollectionDate.InnerText = lblCollectionDate.InnerText.Replace("*", string.Empty);
                    lblCollectionDate.InnerText += "*";
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerText;
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                    //lblCollectionDate.Attributes.Add("style", "color:red");
                    lblCollectionDate.Attributes["Class"] = "";
                    lblCollectionDate.Attributes["Class"] = "MandLabelstyle";
                }
            }

        }


        void LoadLookUpValues()
        {
            //IList<StaticLookup> FieldLookUpList;
            IList<StaticLookup> iFieldValues = new List<StaticLookup>();
            //FieldLookUpList = objStaticLookupManager.getStaticLookupByFieldName(new string[] { "BILL TYPE", "SPECIMEN", "SPECIMEN UNIT", "CMG LAB NAME", "IN-HOUSE PROCEDURES LAB NAME" });
            // FieldLookUpList = objStaticLookupManager.getStaticLookupByFieldName("BILL TYPE").OrderBy(a=>a.Sort_Order).ToList();
            cboBillType.Items.Clear();
            //Bill Type add for performance
            IList<string> ilstBillType = new List<string> { "Third Party", "Patient", "Client" };
            for (int i = 0; i < ilstBillType.Count; i++)
            {
                ListItem cboItem = new ListItem();
                cboItem.Text = ilstBillType[i].ToString();
                cboItem.Attributes.Add("title", ilstBillType[i].ToString());
                this.cboBillType.Items.Add(cboItem);
                //if (ilstBillType[i].ToString().StartsWith(LookUpPerRequest["DefaultBillType"].ToString()))
                //  cboBillType.Text = cboItem.Text;

            }
            //cboBillType.SelectedIndex = 1;//Commentted for BugID:47904
            cboBillType.SelectedIndex = 0;

            //End

            //SPECIMEN add for performance
            cbospecimen.Items.Clear();
            cbospecimen.Items.Add("");
            IList<string> ilstSPECIMEN = new List<string> { "Aspirate",
"Blood",
"Catheter",
"Clippings",
"Culture swab",
"Endocervical swab",
"Exudate",
"Fluid",
"IUD",
"Scrapings",
"Serum",
"Sputum",
"Stool",
"Swab for chlamydia",
"Swab for mycoplasma",
"Swab for Ureaplasma",
"Swab for virus",
"Tissue",
"Urethral swab",
"Urine",
"Urine for std",
"Washings" };
            for (int i = 0; i < ilstSPECIMEN.Count; i++)
            {
                ListItem cboItem = new ListItem();
                cboItem.Text = ilstSPECIMEN[i].ToString();
                cboItem.Attributes.Add("title", ilstSPECIMEN[i].ToString());
                this.cbospecimen.Items.Add(cboItem);
            }
            //End

            //Add Specimen Unit for performance
            IList<string> ilstSpecimenUnits = new List<string> { "Bottle",
"Each",
"Grams",
"Kilograms",
"Pounds",
"Milliequivalent",
"Milliliters",
"Ounces",
"Square centimeters",
"Tablet",
"Vial"};

            cboSpecimenUnits.Items.Clear();
            this.cboSpecimenUnits.Items.Add("");


            int MaxStringContent = ilstSpecimenUnits.Max(a => a.ToString().Length);
            //cboSpecimenUnits.DropDownWidth = MaxStringContent * 5;
            for (int i = 0; i < ilstSpecimenUnits.Count; i++)
            {
                ListItem cboItem = new ListItem();
                cboItem.Text = ilstSpecimenUnits[i].ToString();
                cboItem.Attributes.Add("title", ilstSpecimenUnits[i].ToString());
                this.cboSpecimenUnits.Items.Add(cboItem);
            }
            //End


            //     iFieldValues = FieldLookUpList.Where(a => a.Field_Name == "BILL TYPE").OrderBy(a => a.Sort_Order).ToList();
            // if (iFieldValues != null)
            // {
            //     for (int i = 0; i < iFieldValues.Count; i++)
            //     {
            //         RadComboBoxItem cboItem = new RadComboBoxItem();
            //         cboItem.Text = iFieldValues[i].Value.ToString();
            //         cboItem.ToolTip = iFieldValues[i].Value.ToString();
            //         if (!LookUpPerRequest.ContainsKey("DefaultBillType"))
            //             LookUpPerRequest.Add("DefaultBillType", iFieldValues[i].Default_Value);
            //         this.cboBillType.Items.Add(cboItem);
            //         if (iFieldValues[i].Value.ToString().StartsWith(LookUpPerRequest["DefaultBillType"].ToString()))
            //             cboBillType.Text = cboItem.Text;
            //     }
            // }
            //// FieldLookUpList = objStaticLookupManager.getStaticLookupByFieldName("SPECIMEN", "Sort_Order");
            // iFieldValues = FieldLookUpList.Where(a => a.Field_Name == "SPECIMEN").OrderBy(a => a.Sort_Order).ToList();
            // cbospecimen.Items.Clear();
            // cbospecimen.Items.Add(new RadComboBoxItem(""));
            // if (iFieldValues != null)
            // {
            //     for (int i = 0; i < iFieldValues.Count; i++)
            //     {
            //         RadComboBoxItem cboItem = new RadComboBoxItem();
            //         cboItem.Text = iFieldValues[i].Value.ToString();
            //         cboItem.ToolTip = iFieldValues[i].Value.ToString();
            //         this.cbospecimen.Items.Add(cboItem);
            //     }
            // }
            // //FieldLookUpList = objStaticLookupManager.getStaticLookupByFieldName("SPECIMEN UNIT", "Sort_Order");
            // iFieldValues = FieldLookUpList.Where(a => a.Field_Name == "SPECIMEN UNIT").OrderBy(a => a.Sort_Order).ToList();
            // cboSpecimenUnits.Items.Clear();
            // this.cboSpecimenUnits.Items.Add(new RadComboBoxItem(""));

            // if (iFieldValues != null)
            // {
            //     int MaxStringContent = iFieldValues.Max(a => a.Value.ToString().Length);
            //     cboSpecimenUnits.DropDownWidth = MaxStringContent * 5;
            //     for (int i = 0; i < iFieldValues.Count; i++)
            //     {
            //         RadComboBoxItem cboItem = new RadComboBoxItem();
            //         cboItem.Text = iFieldValues[i].Value.ToString();
            //         cboItem.ToolTip = iFieldValues[i].Value.ToString();
            //         this.cboSpecimenUnits.Items.Add(cboItem);
            //     }
            // }
            // IList<Lab> labList = null;
            if (cboLab.Items.Count == 0)
            {
                cboLab.Items.Clear();
                ListItem cboItem = new ListItem();
                cboItem.Text = " ";
                cboItem.Value = "0";
                cboLab.Items.Add(cboItem);

            }
            //FacilityManager objFacilityManager = new FacilityManager();
            //IList<FacilityLibrary> facilityList = null;
            // facilityList = objFacilityManager.GetFacilityList();
            // facilityList = facilityList.Where(a => a.Fac_Name == ClientSession.FacilityName).ToList<FacilityLibrary>();

            //facilityList = objFacilityManager.GetFacilityByFacilityname(ClientSession.FacilityName);
            //if (ClientSession.FacilityLst.Facility_Library_List.Count > 0)
            //    facilityList = (from p in ClientSession.FacilityLst.Facility_Library_List where p.Fac_Name == ClientSession.FacilityName select p).ToList<FacilityLibrary>();


            //if (facilityList != null && facilityList.Count > 0)
            //{
            //    LookUpPerRequest.Add("FacilityCity", facilityList[0].Fac_City);
            //}

            XDocument xmlFacility = XDocument.Load(Server.MapPath(@"ConfigXML\Facility_Library.xml"));

            IEnumerable<XElement> xmlFac = xmlFacility.Element("FacilityList")
                .Elements("Facility").Where(aa => aa.Attribute("Name").Value.ToString() == ClientSession.FacilityName);
            //.OrderBy(s => (string)s.Attribute("City"));
            if (xmlFac != null && xmlFac.Count() > 0)
            {
                LookUpPerRequest.Add("FacilityCity", xmlFac.Attributes("City").First().Value.ToString());
            }

            //IList<StaticLookup> CMGLabNameStaticLookupList = objStaticLookupManager.getStaticLookupByFieldName("CMG LAB NAME");
            //iFieldValues = FieldLookUpList.Where(a => a.Field_Name == "CMG LAB NAME").OrderBy(a => a.Sort_Order).ToList();
            //if (iFieldValues.Count > 0)
            //{
            //LookUpPerRequest.Add("CMGLabNameFromLookUp", iFieldValues[0].Value);
            //}
            //LookUpPerRequest.Add("CMGLabNameFromLookUp", "CMG Anc.-1866 #101");

            //CMGLabNameStaticLookupList = objStaticLookupManager.getStaticLookupByFieldName("IN-HOUSE PROCEDURES LAB NAME");
            //iFieldValues = FieldLookUpList.Where(a => a.Field_Name == "IN-HOUSE PROCEDURES LAB NAME").OrderBy(a => a.Sort_Order).ToList();
            //if (iFieldValues.Count > 0)
            //{
            //    LookUpPerRequest.Add("InHouseLabNameFromLookUp", iFieldValues[0].Value);

            //}
            LookUpPerRequest.Add("InHouseLabNameFromLookUp", "CMG Anc.-In House");

            //Commented for bugid :
            //LabManager objLabManager = new LabManager();
            //labList = objLabManager.GetAll();

            //Get Lab from xml
            XDocument xmlLab = XDocument.Load(Server.MapPath(@"ConfigXML\LabList.xml"));

            //IEnumerable<XElement> xml = xmlLab.Element("LabList")
            //    .Elements("Lab")
            //    .OrderBy(s => (string)s.Attribute("name"));
            IEnumerable<XElement> xml = xmlLab.Element("LabList")
               .Elements("Lab").Where(a => a.Attribute("type").Value.ToString() != "DME")
               .OrderBy(s => (int)s.Attribute("sort_order"));
            if (xml != null && (LookUpPerRequest.Keys.Contains("LabIdBasedOnIns") == true))
            {
                string[] FavLab = LookUpPerRequest["LabIdBasedOnIns"].Split(',');
                bool IsFavLabSet = false;
                foreach (XElement LabElement in xml)
                {
                    string xmlValue = LabElement.Attribute("name").Value;
                    string xmlLabId = LabElement.Attribute("id").Value;
                    ListItem cboItem = new ListItem();
                    cboItem.Text = xmlValue;
                    cboItem.Value = xmlLabId;
                    cboItem.Attributes.Add("title", xmlValue);
                    cboLab.Items.Add(cboItem);
                    //if (FavLab.Contains(cboItem.Value) && IsFavLabSet == false)
                    if ((FavLab[0] == cboItem.Value) && IsFavLabSet == false)
                    {
                        //cboLab.Text = cboItem.Text;
                        IsDefaultLab = true;
                        cboLab.SelectedIndex = cboLab.Items.IndexOf(cboItem);
                        cboLab_SelectedIndexChanged(new object(), new EventArgs()); //new EventArgs(cboItem.Text, cboLab.Value, cboLab.SelectedIndex.ToString()));
                        IsFavLabSet = true;
                    }
                }
                if (FavLab[0].ToString() == "")
                {
                    txtLocation.Attributes.Add("readOnly", "true");
                    txtCenterName.Attributes.Add("readOnly", "true");
                    //txtLocation.Style["background"] = "#BFDBFF";
                    //txtCenterName.Style["background"] = "#BFDBFF";
                    txtCenterName.Attributes["Class"] = "";
                    txtCenterName.Attributes["Class"] = "nonEditabletxtbox";
                    txtLocation.Attributes["Class"] = "";
                    txtLocation.Attributes["Class"] = "nonEditabletxtbox";
                }
                if (hdnForEditErrorMsg.Value == "Edit")
                {
                    btnClearAll.Value = "Cancel";
                    //System.Web.UI.HtmlControls.HtmlGenericControl txtcancel = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                    //txtcancel.InnerText = "C";
                    //System.Web.UI.HtmlControls.HtmlGenericControl txtcancel1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                    //txtcancel1.InnerText = "ancel";
                }
                else
                {
                    btnClearAll.Value = "Clear All";
                    //System.Web.UI.HtmlControls.HtmlGenericControl txtcancel = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                    //txtcancel.InnerText = "C";
                    //System.Web.UI.HtmlControls.HtmlGenericControl txtcancel1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                    //txtcancel1.InnerText = "lear All";
                }
                if (cboLab.SelectedIndex == 0)
                {
                    Session["cboLab"] = 0;
                    hdnForEditErrorMsg.Value = string.Empty;
                }
            }
            //End


            //if (Session["LabList"] != null)
            //    labList = (IList<Lab>)Session["LabList"];


            //if (labList != null)
            //{
            //    labList = labList.OrderBy(a => a.Sort_Order).ToList<Lab>();
            //    if (labList.Count > 0)
            //    {
            //        string[] FavLab = LookUpPerRequest["LabIdBasedOnIns"].Split(',');
            //        bool IsFavLabSet = false;
            //        for (int i = 0; i < labList.Count; i++)
            //        {
            //            RadComboBoxItem cboItem = new RadComboBoxItem();
            //            cboItem.Text = labList[i].Lab_Name;
            //            cboItem.Value = labList[i].Id.ToString();
            //            cboItem.ToolTip = labList[i].Lab_Name;
            //            cboLab.Items.Add(cboItem);
            //            //if (FavLab.Contains(cboItem.Value) && IsFavLabSet == false)
            //            if ((FavLab[0]==cboItem.Value) && IsFavLabSet == false)
            //            {
            //                cboLab.Text = cboItem.Text;
            //                cboLab.SelectedIndex = cboLab.Items.IndexOf(cboItem);
            //                cboLab_SelectedIndexChanged(new object(), new RadComboBoxSelectedIndexChangedEventArgs(cboItem.Text, cboLab.Text, cboLab.SelectedItem.Index.ToString(), cboItem.Index.ToString()));
            //                IsFavLabSet = true;
            //                Session["cboLab"] = cboLab.Items.IndexOf(cboItem);
            //            }
            //        }
            //        if (hdnForEditErrorMsg.Value == "Edit")
            //        {
            //            btnClearAll.Text = "Cancel";
            //            System.Web.UI.HtmlControls.HtmlGenericControl txtcancel = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
            //            txtcancel.InnerText = "C";
            //            System.Web.UI.HtmlControls.HtmlGenericControl txtcancel1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
            //            txtcancel1.InnerText = "ancel";
            //        }
            //        else
            //        {
            //            btnClearAll.Text = "Clear All";
            //            System.Web.UI.HtmlControls.HtmlGenericControl txtcancel = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
            //            txtcancel.InnerText = "C";
            //            System.Web.UI.HtmlControls.HtmlGenericControl txtcancel1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
            //            txtcancel1.InnerText = "lear All";
            //        }
            //        if (cboLab.SelectedIndex == 0)
            //        {
            //            Session["cboLab"] = 0;
            //            hdnForEditErrorMsg.Value = string.Empty;
            //        }
            //    }
            //}
        }
        public void FillAssesmentAndProblemListICD(IList<Assessment> assList, IList<ProblemList> probList)
        {
            this.chklstAssessment.Items.Clear();
            AssessmentSource = new Dictionary<string, string>();
            IList<string> ICDCodes = new List<string>();
            if (assList != null && assList.Count > 0)
            {
                for (int i = 0; i < assList.Count; i++)
                {
                    if (!AssessmentSource.Any(a => a.Key == assList[i].ICD.ToString()))
                    {
                        if (chklstAssessment.Items.FindByText(assList[i].ICD.ToString() + "-" + assList[i].ICD_Description.ToString()) == null)
                        {
                            this.chklstAssessment.Items.Add(assList[i].ICD.ToString() + "-" + assList[i].ICD_Description.ToString());
                            AssessmentSource.Add(assList[i].ICD.ToString(), "ASSESSMENT-" + assList[i].Id);

                        }
                    }

                }
            }
            if (probList != null && probList.Count > 0)
            {
                for (int i = 0; i < probList.Count; i++)
                {
                    if (probList[i].ICD.Trim() != string.Empty)
                    {
                        if (!AssessmentSource.Any(a => a.Key == probList[i].ICD.ToString()))
                        {
                            if (!AssessmentSource.ContainsKey(probList[i].ICD.ToString()) && chklstAssessment.Items.FindByText(probList[i].ICD.ToString() + "-" + probList[i].Problem_Description.ToString()) == null)// && probList[i].ICD_Code != string.Empty)
                            {
                                this.chklstAssessment.Items.Add(probList[i].ICD.ToString() + "-" + probList[i].Problem_Description.ToString());
                                AssessmentSource.Add(probList[i].ICD.ToString(), "PROBLEM LIST-" + probList[i].Id);
                            }
                        }
                    }

                }
            }
            //this.chklstAssessment.Refresh();

        }
        protected void cboLab_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cboLab.Items[cboLab.SelectedIndex].Text.ToUpper() == "OTHERS" || cboLab.Items[cboLab.SelectedIndex].Text.ToUpper() == "OTHER")
            {
                txtLocation.Value = string.Empty;
                txtCenterName.Value = string.Empty;
                txtLocation.Attributes.Remove("readOnly");
                txtCenterName.Attributes.Remove("readOnly");
                //lblLocation.InnerText += "*";
                //lblLocation.Attributes.Add("style","color:red;");
                lblCenterName.InnerText += "*";
                lblCenterName.InnerHtml = lblCenterName.InnerText;
                //Jira Cap- 544
                //lblCenterName.InnerHtml = lblCenterName.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                lblCenterName.Attributes["Class"] = "";
                lblCenterName.Attributes["Class"] = "MandLabelstyle";
                txtCenterName.Attributes["Class"] = "";
                txtCenterName.Attributes["Class"] = "Editabletxtbox";
                txtLocation.Attributes["Class"] = "";
                txtLocation.Attributes["Class"] = "Editabletxtbox";
                //txtLocation.Attributes.Remove("background-color");
                //txtCenterName.Attributes.Remove("background-color");
            }
            else
            {
                txtLocation.Value = string.Empty;
                txtCenterName.Value = string.Empty;
                txtLocation.Attributes.Add("readOnly", "true");
                //txtLocation.CssClass = "ReadOnlyTextBox";
                txtCenterName.Attributes.Add("readOnly", "true");
                //txtCenterName.CssClass = "ReadOnlyTextBox";
                lblLocation.InnerText = "Location";
                lblCenterName.InnerText = "Center Name";
                lblLocation.Attributes.Add("style", "color:black;");
                lblCenterName.Attributes.Add("style", "color:black;");
                //txtLocation.Style["background"] = "#BFDBFF";
                //txtCenterName.Style["background"] = "#BFDBFF";
                //txtLocation.Style["background-color"] = "#BFDBFF";
                //txtCenterName.Style["background-color"] = "#BFDBFF";
                txtCenterName.Attributes["Class"] = "";
                txtCenterName.Attributes["Class"] = "nonEditabletxtbox";
                txtLocation.Attributes["Class"] = "";
                txtLocation.Attributes["Class"] = "nonEditabletxtbox";
                txtCenterName.Value = cboLab.Items[cboLab.SelectedIndex].Text;
                lblCenterName.Attributes["Class"] = "";
                lblCenterName.Attributes["Class"] = "spanstyle";
                SetReadOnlyStyle(txtLocation);
                SetReadOnlyStyle(txtCenterName);
                //CMG Anc. - In House
                if (IsEditable && cboLab.Value.Trim() != string.Empty && Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value) == 32)
                {
                    lblBillType.InnerText = "Bill Type";
                    chklstAssessment.Enabled = false;
                    chklstAssessment.ClearSelection();
                    lblBillType.Attributes.Add("style", "color:black");
                    gbSelectICD.Enabled = false;
                    pbSelectICD.Disabled = true;
                    //if (IsEditable && Convert.ToUInt64(cboLab.Items[cboLab.FindItemByText(cboLab.Text).Index]) == 32 && lstvFrequentlyUsedLabProcedures.Items.Count > 0)
                    if (Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value) == 32 && chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected))
                        btnImportresult.Disabled = false; //btnImportresult.Enabled = true;
                    else
                        btnImportresult.Disabled = true; //btnImportresult.Enabled = false;


                    chkSpecimenInHouse.Checked = true;
                    if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" || ClientSession.UserRole.ToUpper() == "TECHNICIAN")
                    {

                    }
                    else
                        chkMoveToMA.Checked = true;
                    //if (chkSpecimenInHouse.Checked)
                    //{
                    //    lblCollectionDate.Text += "*";
                    //    lblCollectionDate.ForeColor = Color.Red;
                    //    hdnCollectionDateIsMand.Value = "true";
                    //}
                    //else
                    //{
                    //    lblCollectionDate.Text = lblCollectionDate.Text.Replace('*', ' ').Trim();
                    //    lblCollectionDate.ForeColor = Color.Black;
                    //}
                    //added by balaji.T
                    if (chkMoveToMA.Checked == false)
                    {
                        lblCollectionDate.InnerText += "*";
                        lblCollectionDate.InnerHtml = lblCollectionDate.InnerText;
                        //lblCollectionDate.Attributes.Add("style", "color:red");
                        lblCollectionDate.InnerHtml = lblCollectionDate.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                        hdnCollectionDateIsMand.Value = "true";
                        lblCollectionDate.Attributes["Class"] = "";
                        lblCollectionDate.Attributes["Class"] = "MandLabelstyle";
                    }
                    else
                    {
                        lblCollectionDate.InnerText = "Collection Date";// lblCollectionDate.InnerText.Replace('*', ' ').Trim();
                        lblCollectionDate.Attributes.Add("style", "color:black");
                        hdnCollectionDateIsMand.Value = "false";
                        lblCollectionDate.Attributes["Class"] = "";
                        lblCollectionDate.Attributes["Class"] = "spanstyle";
                        if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
                        {
                            lblCollectionDate.InnerText += "*";
                            lblCollectionDate.InnerHtml = lblCollectionDate.InnerText;
                            lblCollectionDate.InnerHtml = lblCollectionDate.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                            //lblCollectionDate.Attributes.Add("style", "color:red");
                            hdnCollectionDateIsMand.Value = "true";
                            lblCollectionDate.Attributes["Class"] = "";
                            lblCollectionDate.Attributes["Class"] = "MandLabelstyle";
                        }
                    }

                }
                else
                {
                    if (IsEditable)
                    {
                        lblBillType.InnerText = "Bill Type*";
                        chklstAssessment.Enabled = true;
                        lblBillType.Attributes.Add("style", "color:red");
                        gbSelectICD.Enabled = true;
                        pbSelectICD.Disabled = false;
                        btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
                        chkSpecimenInHouse.Checked = false;
                        chkMoveToMA.Checked = false;
                        SetCollectionDateMand(false);
                        //hdnCollectionDateIsMand.Value = "false";
                    }
                }
            }

            //Get Lab Location from xml
            XDocument xmlLabLocation = XDocument.Load(Server.MapPath(@"ConfigXML\LabLocationList.xml"));

            if (xmlLabLocation != null && (LookUpPerRequest.Keys.Contains("FacilityCity") == true && LookUpPerRequest["FacilityCity"] != null))
            {
                if (LookUpPerRequest["FacilityCity"] != string.Empty)
                {
                    IEnumerable<XElement> xmlLocation = null;
                    try
                    {
                        xmlLocation = xmlLabLocation.Element("LabLocationList")
             .Elements("LabLocation").Where(xx => xx.Attribute("labid").Value == (cboLab.Items[cboLab.SelectedIndex].Value) && xx.Attribute("city").Value.ToUpper() == (LookUpPerRequest["FacilityCity"].ToUpper()));

                    }

                    catch (Exception)
                    {
                        xmlLocation = null;
                    }
                    if (xmlLocation != null && xmlLocation.Count() > 0)
                    {
                        txtLocation.Value = xmlLocation.Attributes("city").First().Value.ToString();
                        if (!LookUpPerRequest.ContainsKey("labLocID"))
                            LookUpPerRequest.Add("labLocID", xmlLocation.Attributes("id").First().Value.ToString());
                        else
                            LookUpPerRequest["labLocID"] = xmlLocation.Attributes("id").First().Value.ToString();
                        //txtLocation.Text = xmlLocation.Attributes("city").SingleOrDefault().Value.ToString();
                        //if (!LookUpPerRequest.ContainsKey("labLocID"))
                        //    LookUpPerRequest.Add("labLocID", xmlLocation.Attributes("id").SingleOrDefault().Value.ToString());
                        //else
                        //    LookUpPerRequest["labLocID"] = xmlLocation.Attributes("id").SingleOrDefault().Value.ToString();
                    }
                    else
                    {
                        if (LookUpPerRequest.ContainsKey("labLocID"))
                            LookUpPerRequest["labLocID"] = "0";
                    }

                }
            }



            //End

            //IList<LabLocation> labLocList = null;
            //if (cboLab.SelectedIndex != -1 && cboLab.SelectedIndex != 0)
            //    labLocList = objLabLocationManager.GetLabLocationUsinglabID(Convert.ToUInt64(cboLab.Items[cboLab.FindItemByText(cboLab.Text).Index].Value));
            ////else
            ////    labLocList = objLabLocProxy.GetLabLocationUsingLabId(Convert.ToUInt64(cboLab.Items[cboLab.Text].Value));
            //LabLocation objLocation = null;
            //if (labLocList != null && LookUpPerRequest["FacilityCity"] != null)
            //{
            //    if (LookUpPerRequest["FacilityCity"] != string.Empty)
            //    {
            //        try
            //        {
            //            objLocation = (from obj in labLocList where obj.City.ToUpper() == LookUpPerRequest["FacilityCity"].ToUpper() select obj).ToList<LabLocation>()[0];
            //        }
            //        catch (Exception)
            //        {
            //            objLocation = null;
            //        }
            //        if (objLocation != null)
            //        {
            //            txtLocation.Text = objLocation.City;
            //            //labLocID = objLocation.Id;
            //            if (!LookUpPerRequest.ContainsKey("labLocID"))
            //                LookUpPerRequest.Add("labLocID", objLocation.Id.ToString());
            //            else
            //                LookUpPerRequest["labLocID"] = objLocation.Id.ToString();
            //        }
            //    }
            //}
            //btnSelectLocation.Enabled = (cboLab.Items[cboLab.SelectedIndex].Text != string.Empty);
            if (cboLab.Items[cboLab.SelectedIndex].Text != string.Empty)
                btnSelectLocation.Disabled = false;
            else
                btnSelectLocation.Disabled = true;
            ulong selectedLabID = 0;
            if (cboLab.Items[cboLab.SelectedIndex].Value != string.Empty)
                selectedLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
            if (LookUpPerRequest.Keys.Contains("procedureType") == true)
                procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), selectedLabID, ClientSession.LegalOrg);
            FillLabProcedure(procedureList, new List<string>());
            if (!DontThrowMsg)
            {
                if (!(procedureList.Count > 0) && IsNormalMode)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230143');", true);

                }
            }

            if (cboLab.Items[cboLab.SelectedIndex].Text.Trim() != string.Empty)
            {
                btnSelectLocation.Disabled = false; //btnSelectLocation.Enabled = true;
            }
            else
            {
                btnSelectLocation.Disabled = true; //btnSelectLocation.Enabled = false;
            }

            if ((cboLab.Items[cboLab.SelectedIndex].Text == "Quest Diagnostics") || (cboLab.Items[cboLab.SelectedIndex].Text == "LabCorp"))
            {
                //btnFillQuestionSets.Enabled = true;
            }
            else
            {
                //btnFillQuestionSets.Enabled = false;
            }



            FirstTime = false;
            if (hdnOrderSubmitID.Value == null || hdnOrderSubmitID.Value == "0" || hdnOrderSubmitID.Value.Trim() == "")
                btnOrderSubmit.Disabled = false; //Commented for cmgancillary
            //btnOrderSubmit.Enabled = true;
            //divLoading.Style.Add("display", "none");
            if (hdnForEditErrorMsg.Value != "Edit")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ManLabel", "warningmethodCbolab();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ManLabel", "warningmethodCbolab('" + IsDefaultLab + "');", true);

        }
        //void SetNonReadOnlyStyle(RadTextBox objRadTextBox)
        //{
        //    objRadTextBox.ReadOnly = false;

        //}
        void SetNonReadOnlyStyle(HtmlInputText objRadTextBox)
        {
            objRadTextBox.Attributes.Add("readOnly", "false");
        }
        //void SetReadOnlyStyle(RadTextBox objRadTextBox)
        //{
        //    objRadTextBox.ReadOnly = true;

        //}
        void SetReadOnlyStyle(HtmlInputText objRadTextBox)
        {
            objRadTextBox.Attributes.Add("readOnly", "true");
        }
        private void FillLabProcedure(IList<PhysicianProcedure> labProList, IList<string> lstChkeditem)
        {
            IList<PhysicianProcedure> templst = new List<PhysicianProcedure>();
            templst = labProList.Where(a => a.Sort_Order == 0).ToList<PhysicianProcedure>();
            labProList = (labProList.Where(a => a.Sort_Order != 0).OrderBy(a => a.Sort_Order).ToList<PhysicianProcedure>()).Concat(templst).ToList<PhysicianProcedure>();
            IList<string> groupedProce = (from rec in labProList select rec.Order_Group_Name).Distinct().ToList<string>();
            IList<string> ItemsToBeAdded = new List<string>();
            foreach (string str in groupedProce)
            {
                ItemsToBeAdded.Add("!!" + str);

                //ListViewItem objRadListViewItem;
                ////objRadListViewItem = new ListViewItem(ListViewItemType.);
                //objRadListViewItem.ForeColor = Color.Blue;
                //objRadListViewItem.Font = new Font("Arial", 10, FontStyle.Bold);
                //lstvFrequentlyUsedLabProcedures.Items.Add(objRadListViewItem);
                var underthisgroup = (from rec in labProList where rec.Order_Group_Name.ToUpper() == str.ToUpper() select rec).ToList<PhysicianProcedure>();
                foreach (var phypro in underthisgroup)
                {
                    ItemsToBeAdded.Add(phypro.Physician_Procedure_Code + "-" + phypro.Procedure_Description);
                    //    objRadListViewItem = new ListViewItem((phypro.Physician_Procedure_Code + "-" + phypro.Procedure_Description).ToString());
                    //    objRadListViewItem.ForeColor = Color.Black;
                    //    objRadListViewItem.Font = new Font("Arial", 8, FontStyle.Regular);
                    //    objRadListViewItem.ToolTipText = SetToolTipText((phypro.Physician_Procedure_Code + "-" + phypro.Procedure_Description).ToString());
                    //    lstvFrequentlyUsedLabProcedures.Items.Add(objRadListViewItem);

                    //    //lstvFrequentlyUsedLabProcedures.Items.Add((phypro.Physician_Procedure_Code + "-" + phypro.Procedure_Description).ToString());
                }

            }
            //int ItemsPerRow = 14;
            int ItemsPerRow = 10;
            decimal tempDouble = ((decimal)ItemsToBeAdded.Count / (decimal)ItemsPerRow);
            int coulumns = (int)(Math.Ceiling(tempDouble));
            //coulumns = Math.Abs((coulumns + ItemsToBeAdded.Count / ItemsPerRow));
            ListViewHeader = new List<string>();
            chklstFrequentlyUsedProcedures.Items.Clear();
            chklstFrequentlyUsedProcedures.RepeatColumns = coulumns;
            IList<int> StringLengthColumn = new List<int>();
            IList<int> maxstringLengthColumnVies = new List<int>();
            string HeaderText = string.Empty;
            for (int i = 0; i < ItemsToBeAdded.Count; i++)
            {
                ListItem tempItem = new ListItem();

                if (ItemsToBeAdded[i].StartsWith("!!"))
                {
                    tempItem.Text = ItemsToBeAdded[i].ToString().Replace("!!", "");
                    //tempItem.Attributes.Add("Criteria","!!");
                    tempItem.Value = "HEADERROW";
                    //tempItem.Attributes.Add("IsHeader", "true");
                    HeaderText = tempItem.Text;
                    ListViewHeader.Add("IsHeader-true;RespectiveHeader-" + HeaderText);

                }
                else
                {
                    tempItem.Attributes.Add("style", "color:black;");
                    tempItem.Text = ItemsToBeAdded[i].ToString();
                    //if (lstChkeditem.Contains(ItemsToBeAdded[i].ToString()))
                    //    tempItem.Selected = true;
                    tempItem.Value = ItemsToBeAdded[i].ToString() + "~" + HeaderText;

                    for (int itemval = 0; itemval < lstChkeditem.Count; itemval++)
                    {
                        if (lstChkeditem[itemval].Split('-')[0].Contains(ItemsToBeAdded[i].Split('-')[0]))
                            tempItem.Selected = true;
                    }


                    //tempItem.Attributes.Add("IsHeader", "false");
                    ListViewHeader.Add("IsHeader-false;RespectiveHeader-" + HeaderText);

                }

                chklstFrequentlyUsedProcedures.Items.Add(tempItem);
                StringLengthColumn.Add(tempItem.Text.ToString().Length);
                if (i != 0 && i % 13 == 0)
                {
                    maxstringLengthColumnVies.Add(StringLengthColumn.Max());
                    StringLengthColumn.Clear();
                }
            }
            string InjectedArrayList = string.Empty;
            InjectedArrayList = "MaxCounts";
            string InjectedArrayValues = string.Empty;

            System.Text.StringBuilder injectedVariable = new System.Text.StringBuilder();
            injectedVariable.Append("this.MaxCounts=new Array();");

            //for (int i = 0; i < maxstringLengthColumnVies.Count; i++)
            //{
            //    //if (i != maxstringLengthColumnVies.Count-1)
            //    //InjectedArrayValues += "\"" + maxstringLengthColumnVies[i].ToString() + "\",";
            //    //else
            //    //    InjectedArrayValues += "\"" + maxstringLengthColumnVies[i].ToString() + "\"";
            //    //injectedVariable.Append("MaxCounts.push(" + maxstringLengthColumnVies[i].ToString() + ");");
            //    injectedVariable.Append(maxstringLengthColumnVies[i].ToString()+",");
            //}

            hdnListViewArray.Value = string.Join(",", maxstringLengthColumnVies.Select(a => a.ToString()).ToArray<string>());
            //Page.ClientScript.RegisterArrayDeclaration(InjectedArrayList, InjectedArrayValues);
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "MaxCountScript", injectedVariable.ToString(), true);
            //if (!ClientScript.IsClientScriptBlockRegistered("FormateListView"))
            //    ClientScript.RegisterStartupScript(this.GetType(), "FormateListView", " FormateFrequentlyUsedList();", true);


            //int RowCount = 14;
            //int ColumnCount = ItemsToBeAdded.Count / RowCount;
            //ColumnCount = ColumnCount == 0 ? 1 : ColumnCount;
            //for (int i = 0; i < RowCount; i++)
            //{

            //    HtmlTableRow tr = new HtmlTableRow();
            //    for (int j = 0; j < ColumnCount; j++)
            //    {
            //        string CheckBoxText = string.Empty;
            //        CheckBox objCheckBox = new CheckBox();
            //        HtmlTableCell tc = new HtmlTableCell();
            //        try
            //        {

            //            if (j == 0)
            //            {
            //                CheckBoxText = ItemsToBeAdded[i];
            //            }
            //            else
            //            {
            //                CheckBoxText = ItemsToBeAdded[((i + 1) * ColumnCount) + (i + 1)];
            //            }
            //        }
            //        catch (ArgumentOutOfRangeException ex)
            //        {
            //            CheckBoxText = ex.ToString();
            //        }
            //        objCheckBox.Text = CheckBoxText;
            //        tc.Controls.Add(objCheckBox);
            //        tr.Cells.Add(tc);
            //    }
            //    FrequentlyUsedProcedures.Rows.Add(tr);



            //lstvFrequentlyUsedLabProcedures.Items.Clear();


            //IList<string> templist = new List<string>();
            //templist = lstChkeditem.Select(a => a.Split('-')[0]).ToList<string>();


            //var item = (from itm in lstvFrequentlyUsedLabProcedures.Items.Cast<ListViewItem>() where templist.Contains(itm.Text.Split('-')[0]) select itm);
            //foreach (ListItem obj in chklstFrequentlyUsedProcedures.Items)
            for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
            {
                chklstFrequentlyUsedProcedures.Items[i].Attributes.Add("onClick", "EnableImportResults();");
            }
            //foreach (ListViewItem itm in item)
            //{
            //    //itm.Checked = true;
            //}
            if (lstChkeditem.Count > 0) // added by balaji.TJ
            {
                btnImportresult.Disabled = false; //btnImportresult.Enabled = true;
            }
        }
        public bool CheckForValidation()
        {
            errList = new ArrayList();


            if (txtQuantity.Value != string.Empty)
            {
                if (!lblUnits.InnerText.Contains("*"))
                    lblUnits.InnerText += "*";
                lblUnits.InnerHtml = lblUnits.InnerText;
                lblUnits.InnerHtml = lblUnits.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                //lblUnits.Attributes.Add("style", "color:red");
                lblUnits.Attributes["Class"] = "";
                lblUnits.Attributes["Class"] = "MandLabelstyle";
            }
            else
            {
                lblUnits.InnerText = "Units";// lblUnits.InnerText.Replace("*", string.Empty);
                lblUnits.Attributes.Add("style", "color:black");
                lblUnits.Attributes["Class"] = "";
                lblUnits.Attributes["Class"] = "spanstyle";
            }
            if ((chkTestDateInDate.Checked == true) && (cstdtpTestDate.Value == string.Empty))
            {
                errList.Add(lblTestDate.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblTestDate.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtCenterName.Focus();
                return false;
            }
            if (cboLab.Items[cboLab.SelectedIndex].Text.Trim() == string.Empty)
            {
                errList.Add(lblLab.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblLab.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //  ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                cboLab.Focus();
                return false;
            }
            else if (lblCenterName.InnerText.Contains('*') && txtCenterName.Value.Trim() == string.Empty)
            {
                errList.Add(lblCenterName.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + "Center Name" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtCenterName.Focus();
                return false;
            }
            else if (lblLocation.InnerText.Contains('*') && txtLocation.Value.Trim() == string.Empty)
            {
                errList.Add(lblLocation.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + "Location" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtLocation.Focus();
                return false;
            }
            else if (lblCollectionDate.InnerText.Contains('*') && dtpCollectionDate.Value == "")
            {
                errList.Add(lblCollectionDate.InnerText.Replace('*', ' ').Trim());
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblCollectionDate.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + "Collection Date" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                //dtpCollectionDate.Focus();
                return false;
            }
            else if (lblUnits.InnerText.Contains('*') && cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text == string.Empty)
            {
                errList.Add(lblUnits.InnerText.Replace('*', ' ').Trim());
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblUnits.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + "Units" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                cboSpecimenUnits.Focus();
                return false;
            }
            else if (cboBillType.Items[cboBillType.SelectedIndex].Text.Trim() == string.Empty && lblBillType.InnerText.Contains("*"))
            {
                errList.Add(lblBillType.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblBillType.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                cboBillType.Focus();
                return false;
            }
            //else if (cboReadingProvider.Items.Count > 0 && cboReadingProvider.Items[cboReadingProvider.SelectedIndex].Text.Trim() == string.Empty && lblReadingProvider.InnerText.Contains("*"))
            //{
            //    errList.Add(lblReadingProvider.InnerText.Replace('*', ' ').Trim());
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230157','','" + lblReadingProvider.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
            //    cboReadingProvider.Focus();
            //    return false;
            //}
            //CAP-1638 - Testing and Production : Diagnostic order saved without lab procedure
            else if ((!chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Where(a => a.Value != "HEADERROW").Any(a => a.Selected == true)))
            {
                errList.Clear();
                string errorMsg = string.Empty;
                if (OrderType.ToUpper() == "DIAGNOSTIC ORDER")
                {
                    errList.Add("a Lab Procedure");
                    errorMsg += " a Lab Procedure";
                }
                else if (OrderType.ToUpper() == "IMAGE ORDER")
                {
                    errList.Add("an Image Procedure");
                    errorMsg += " an Image Procedure";
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230104','','" + errorMsg + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return false;
            }
            else if ((!chklstAssessment.Items.Cast<ListItem>().Any(a => a.Selected == true)) && chklstAssessment.Enabled == true)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230107'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230107", OrderType.ToUpper(), this);
                return false;
            }

            else if (lblCollectionDate.InnerText.Contains('*') && dtpCollectionDate.Value != null && dtpCollectionDate.Value.ToString().Trim() == string.Empty)
            // else if (lblCollectionDate.Text.Contains('*') && dtpCollectionDate.SelectedDate.Value.ToString().Trim() == string.Empty)
            {
                errList.Add(lblCollectionDate.InnerText.Replace('*', ' ').Trim());
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblCollectionDate.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + "Collection Date" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //  ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                cboSpecimenUnits.Focus();
                return false;
            }

            //string sLAb = System.Configuration.ConfigurationManager.AppSettings["AncillaryLab"].ToUpper();
            //CAP-697
            var lab = from l in ApplicationObject.facilityLibraryList where l.Short_Name == cboLab.Items[cboLab.SelectedIndex].Text select l;
            IList<FacilityLibrary> facLabList = lab.ToList<FacilityLibrary>();

            //if (cboLab.Items[cboLab.SelectedIndex].Text.ToUpper() == sLAb)

            if (facLabList.Count > 0)
            {
                var scheckProcedures = (from a in chklstFrequentlyUsedProcedures.Items.Cast<ListItem>() where a.Selected == true && a.Value != "HEADERROW" select a.Value.Split('~')[a.Value.Split('~').Length - 1]).Distinct().ToArray();
                if (scheckProcedures.Count() > 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230158'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return false;
                }
            }

            return true;
        }
        private OrdersAssessment CreateOrderAssObj(string ICD, ulong AssID, string Source)
        {
            OrdersAssessment objOrdAss = new OrdersAssessment();
            string assText = ICD;
            string[] split = assText.Split('-');
            if (split.Length > 0)
            {
                objOrdAss.Source_ID = Convert.ToUInt64(AssID);
                objOrdAss.ICD = split[0];
                for (int i = 1; i < split.Length; i++)
                {
                    if (i == 1)
                        objOrdAss.ICD_Description = split[i];
                    else
                        objOrdAss.ICD_Description += "-" + split[i];
                }
                objOrdAss.Encounter_ID = EncounterID;
                objOrdAss.Human_ID = HumanID;
                objOrdAss.Source = Source.Split('-')[0];
                objOrdAss.Source_ID = Convert.ToUInt64(Source.Split('-')[1]);
                objOrdAss.Created_By = ClientSession.UserName;
                objOrdAss.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objOrdAss.Modified_By = ClientSession.UserName;
                objOrdAss.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            }
            return objOrdAss;


        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            LookUpValues = LookUpPerRequest;
        }
        public void chkSpecimenInHouseAlterText()
        {
            if ((ClientSession.UserRole == "Medical Assistant") || (ClientSession.UserRole == "Technician"))
            {
                if (chkSpecimenInHouse.Checked)
                {
                    if (!lblCollectionDate.InnerText.Contains("*"))
                        lblCollectionDate.InnerText += "*";
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerText;
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                    //lblCollectionDate.Attributes.Add("style", "color:red");
                    lblCollectionDate.Attributes["Class"] = "";
                    lblCollectionDate.Attributes["Class"] = "MandLabelstyle";
                }
                else
                {
                    lblCollectionDate.InnerText = "Collection Date";// lblCollectionDate.InnerText.Replace('*', ' ').Trim();
                    lblCollectionDate.Attributes.Add("style", "color:black");
                    lblCollectionDate.Attributes["Class"] = "";
                    lblCollectionDate.Attributes["Class"] = "spanstyle";
                }
            }
            else
            {
                if (chkSpecimenInHouse.Checked)
                {
                    chkMoveToMA.Checked = chkSpecimenInHouse.Checked;
                }
                else
                {
                    if (!chkMoveToMA.Checked)
                    {
                        lblCollectionDate.InnerText = lblCollectionDate.InnerText.Replace('*', ' ').Trim();
                        lblCollectionDate.Attributes.Add("style", "color:black");
                        lblCollectionDate.Attributes["Class"] = "";
                        lblCollectionDate.Attributes["Class"] = "spanstyle";
                    }

                }
            }
        }
        protected void chkSpecimenInHouse_CheckedChanged(object sender, EventArgs e)
        {
            // Modified by priyangha on 13/3/2013 bugid:14855
            if ((ClientSession.UserRole == "Medical Assistant") || (ClientSession.UserRole == "Technician"))
            {
                if (chkSpecimenInHouse.Checked)
                {
                    lblCollectionDate.InnerText += "*";
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerText;
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                    //lblCollectionDate.Attributes.Add("style", "color:red");
                    lblCollectionDate.Attributes["Class"] = "";
                    lblCollectionDate.Attributes["Class"] = "MandLabelstyle";
                }
                else
                {
                    lblCollectionDate.InnerText = "Collection Date";// lblCollectionDate.InnerText.Replace('*', ' ').Trim();
                    lblCollectionDate.Attributes.Add("style", "color:black");
                    lblCollectionDate.Attributes["Class"] = "";
                    lblCollectionDate.Attributes["Class"] = "spanstyle";
                }
            }
            else
            {
                if (chkSpecimenInHouse.Checked)
                {
                    chkMoveToMA.Checked = chkSpecimenInHouse.Checked;
                }
                else
                {
                    if (!chkMoveToMA.Checked)
                    {
                        lblCollectionDate.InnerText = lblCollectionDate.InnerText.Replace('*', ' ').Trim();
                        lblCollectionDate.Attributes.Add("style", "color:black");
                        lblCollectionDate.Attributes["Class"] = "";
                        lblCollectionDate.Attributes["Class"] = "spanstyle";
                    }

                }
            }
        }
        bool IsICDIsCheckedWithOutCPT()
        {
            // if (chklstAssessment.Items.Count > 0 && (!chklstAssessment.Items.Cast<ListItem>().Any(a => a.Selected == true)) && chklstAssessment.Items != null && chklstAssessment.Items.Count > 0 && (!chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected == true)))bugId:39021
            //CAP-1430
            //if ((!chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected == true)) && (!chklstAssessment.Items.Cast<ListItem>().Any(a => a.Selected == true)))
            if ((!chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected == true)))
            {
                errList.Clear();
                if (OrderType.ToUpper() == "DIAGNOSTIC ORDER")
                    errList.Add("a Lab Procedure");
                else if (OrderType.ToUpper() == "IMAGE ORDER")
                    errList.Add("an Image Procedure");
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230104','','" + errList[0].ToString() + "');", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230104", LookUpPerRequest["OrderType"].ToUpper(), errList, this);
                return false;
            }
            else
                return true;
        }
        protected void chkMoveToMA_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkMoveToMA.Checked)
            {
                objOnCheckForValidation += CheckForValidation;
                objOnCheckForValidation -= IsICDIsCheckedWithOutCPT;
                if (chkSpecimenInHouse.Checked)
                {
                    if (!lblCollectionDate.InnerText.Contains("*"))
                        lblCollectionDate.InnerText += "*";
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerText;
                    lblCollectionDate.InnerHtml = lblCollectionDate.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                    //lblCollectionDate.Attributes.Add("style", "color:red");
                    hdnCollectionDateIsMand.Value = "true";
                    lblCollectionDate.Attributes["Class"] = "";
                    lblCollectionDate.Attributes["Class"] = "MandLabelstyle";
                }
                else
                {
                    lblCollectionDate.InnerText = "Collection Date";// lblCollectionDate.InnerText.Replace('*', ' ').Trim();
                    lblCollectionDate.Attributes.Add("style", "color:black");
                    hdnCollectionDateIsMand.Value = "false";
                    lblCollectionDate.Attributes["Class"] = "";
                    lblCollectionDate.Attributes["Class"] = "spanstyle";
                }
            }
            else
            {
                if (ClientSession.UserRole != "Medical Assistant")
                {
                    objOnCheckForValidation -= CheckForValidation;
                    objOnCheckForValidation += IsICDIsCheckedWithOutCPT;
                    lblCollectionDate.InnerText = "Collection Date";// lblCollectionDate.InnerText.Replace('*', ' ').Trim();
                    lblCollectionDate.Attributes.Add("style", "color:black");
                    hdnCollectionDateIsMand.Value = "false";
                    lblCollectionDate.Attributes["Class"] = "";
                    lblCollectionDate.Attributes["Class"] = "spanstyle";
                }
            }

        }
        protected void cboBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBillType.Items[cboBillType.SelectedIndex].Text.Contains("Third"))
            {
                dbInsurancePlan.Enabled = true;
                // Added for Bug id=28559
                dbInsurance.Enabled = true;
                dbInsurance.ImageUrl = "~/Resources/Database Inactive.jpg";
            }
            else
            {
                dbInsurancePlan.Enabled = false;
                // Added for Bug id=28559
                dbInsurance.Enabled = false;
                dbInsurance.ImageUrl = "~/Resources/Database Disable.png";
            }
            btnOrderSubmit.Disabled = false; //btnOrderSubmit.Enabled = true;
        }
        protected void chkTestDateInDate_CheckedChanged(object sender, EventArgs e)
        {
            chkTestDateInWords.Checked = !chkTestDateInDate.Checked;
            if (chkTestDateInDate.Checked)  //chkStat.Enabled = chkTestDateInDate.Checked;
            {
                chkStat.Disabled = false;
                chkUrgent.Disabled = false;
            }
            else
            {
                chkStat.Disabled = true;
                chkUrgent.Disabled = true;
            }

            chkStat.Checked = false;
            chkUrgent.Checked = false;
            cstdtpTestDate.Disabled = !chkTestDateInDate.Checked; //cstdtpTestDate.Enabled = chkTestDateInDate.Checked;

            if (chkTestDateInDate.Checked)
            {
                txtMonths.Value = "";
                txtMonths.Disabled = true; //txtMonths.Enabled = false;
                txtWeeks.Value = "";
                txtWeeks.Disabled = true; //txtWeeks.Enabled = false;
                txtDays.Value = "";
                txtDays.Disabled = true; //txtDays.Enabled = false;
                txtDays.Attributes.Add("readOnly", "true");
                txtWeeks.Attributes.Add("readOnly", "true");
                txtMonths.Attributes.Add("readOnly", "true");
            }
        }
        protected void chkStat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStat.Checked)
                cstdtpTestDate.Disabled = true; //cstdtpTestDate.Enabled = !chkStat.Checked;
            else
                cstdtpTestDate.Disabled = false;
            if (chkStat.Checked)
            {
                cstdtpTestDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            if (chkStat.Checked == true)
            {
                btnOrderSubmit.Disabled = false; //btnOrderSubmit.Enabled = true;
            }
        }
        protected void chkTestDateInWords_CheckedChanged(object sender, EventArgs e)
        {


            if (chkTestDateInWords.Checked)
            {
                cstdtpTestDate.Disabled = true; //cstdtpTestDate.Enabled = false;
                chkStat.Disabled = true; //chkStat.Enabled = false;
                chkTestDateInDate.Checked = false;
                chkUrgent.Disabled = true;
            }
            else
            {
                cstdtpTestDate.Disabled = false; //cstdtpTestDate.Enabled = true;
                chkStat.Disabled = false; //chkStat.Enabled = true;
                chkTestDateInDate.Checked = true;
                chkUrgent.Disabled = false;
            }

            //txtMonths.Enabled = chkTestDateInWords.Checked;
            //txtWeeks.Enabled = chkTestDateInWords.Checked;
            //txtDays.Enabled = chkTestDateInWords.Checked;
            if (chkTestDateInWords.Checked)
            {
                txtMonths.Disabled = false;
                txtWeeks.Disabled = false;
                txtDays.Disabled = false;
                txtMonths.Attributes.Remove("readOnly");
                txtWeeks.Attributes.Remove("readOnly");
                txtDays.Attributes.Remove("readOnly");
                txtMonths.Attributes["Class"] = "";
                txtMonths.Attributes["Class"] = "Editabletxtbox";
                txtWeeks.Attributes["Class"] = "";
                txtWeeks.Attributes["Class"] = "Editabletxtbox";
                txtDays.Attributes["Class"] = "";
                txtDays.Attributes["Class"] = "Editabletxtbox";
            }
            else
            {
                txtMonths.Disabled = true;
                txtWeeks.Disabled = true;
                txtDays.Disabled = true;
                txtMonths.Attributes.Add("readOnly", "true");
                txtWeeks.Attributes.Add("readOnly", "true");
                txtDays.Attributes.Add("readOnly", "true");
                txtMonths.Attributes["Class"] = "";
                txtMonths.Attributes["Class"] = "nonEditabletxtbox";
                txtWeeks.Attributes["Class"] = "";
                txtWeeks.Attributes["Class"] = "nonEditabletxtbox";
                txtDays.Attributes["Class"] = "";
                txtDays.Attributes["Class"] = "nonEditabletxtbox";
            }

            //txtDays.Attributes.Add("readOnly", "false");//.CssClass = "";
            //txtWeeks.Attributes.Add("readOnly", "false");//CssClass = "";
            //txtMonths.Attributes.Add("readOnly", "false");//CssClass = "";
        }


        void txtQuantity_TextChanged()
        {
            if (txtQuantity.Value != string.Empty && !lblUnits.InnerText.Contains('*'))
            {
                if (!lblUnits.InnerText.Contains("*"))
                    lblUnits.InnerText += "*";
                lblUnits.InnerHtml = lblUnits.InnerText;
                lblUnits.InnerHtml = lblUnits.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                //lblUnits.Attributes.Add("style", "color:red");
                lblUnits.Attributes["Class"] = "";
                lblUnits.Attributes["Class"] = "MandLabelstyle";
            }
            else if (txtQuantity.Value == string.Empty)
            {
                lblUnits.InnerText = "Units";//lblUnits.InnerText.Replace('*', ' ');
                lblUnits.Attributes.Add("style", "color:black");
                lblUnits.Attributes["Class"] = "";
                lblUnits.Attributes["Class"] = "spanstyle";
            }
        }
        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {

            if (txtQuantity.Value != string.Empty && !lblUnits.InnerText.Contains('*'))
            {
                lblUnits.InnerText += "*";
                lblUnits.InnerHtml = lblUnits.InnerText;
                lblUnits.InnerHtml = lblUnits.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                //lblUnits.Attributes.Add("style", "color:red");
                lblUnits.Attributes["Class"] = "";
                lblUnits.Attributes["Class"] = "MandLabelstyle";
            }
            else if (txtQuantity.Value == string.Empty)
            {
                lblUnits.InnerText = "Units";//lblUnits.InnerText.Replace('*', ' ');
                lblUnits.Attributes.Add("style", "color:black");
                lblUnits.Attributes["Class"] = "";
                lblUnits.Attributes["Class"] = "spanstyle";
            }
        }
        protected void chkSelectALLICD_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chklstAssessment.Items.Count; i++)
            {
                chklstAssessment.Items[i].Selected = chkSelectALLICD.Checked;
            }
            if (chkSelectALLICD.Checked == true)
            {
                btnOrderSubmit.Disabled = false; //btnOrderSubmit.Enabled = true;
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void btnSelectLocation_Click(object sender, EventArgs e)
        {
            //if (cboLab.Items[cboLab.SelectedIndex].Selected != null)
            //{
            //    ulong selectedLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
            //    ScriptManager.RegisterStartupScript(this, GetType(), "OpenLabLocationScreenKey", "OpenLabLocationScreen(" + selectedLabID.ToString() + ",'" + cboLab.Items[cboLab.SelectedIndex].Text + "');", true);
            //}
            //else
            //{
            //    errList = new ArrayList();
            //    errList.Add(lblLab.InnerText.Replace('*', ' '));
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230106');", true);
            //    cboLab.Focus();
            //}
            if (cboLab.Items[cboLab.SelectedIndex].Selected != null)
            {
                XDocument xmlLabLocation = XDocument.Load(Server.MapPath(@"ConfigXML\LabLocationList.xml"));

                if (xmlLabLocation != null)
                {
                    if (txtLocation.Value != string.Empty)
                    {
                        IEnumerable<XElement> xmlLocation = null;
                        try
                        {
                            xmlLocation = xmlLabLocation.Element("LabLocationList")
                 .Elements("LabLocation").Where(xx => xx.Attribute("labid").Value == (cboLab.Items[cboLab.SelectedIndex].Value) && xx.Attribute("city").Value.ToUpper() == (txtLocation.Value.ToUpper()));

                        }

                        catch (Exception)
                        {
                            xmlLocation = null;
                        }
                        if (xmlLocation != null && xmlLocation.Count() > 0)
                        {
                            //txtLocation.Value = xmlLocation.Attributes("city").First().Value.ToString();
                            if (!LookUpPerRequest.ContainsKey("labLocID"))
                                LookUpPerRequest.Add("labLocID", xmlLocation.Attributes("id").First().Value.ToString());
                            else
                                LookUpPerRequest["labLocID"] = xmlLocation.Attributes("id").First().Value.ToString();
                        }
                    }
                }
            }
            else
            {
                errList = new ArrayList();
                errList.Add(lblLab.InnerText.Replace('*', ' '));
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230106','','" + lblLab.InnerText.Replace('*', ' ') + "')", true);
                cboLab.Focus();
            }

        }
        private void disableallcontrols(bool IsEnable)
        {
            if (IsEnable)
            {
                //pbPlusClinicalNotes.Image = global::Acurus.Capella.UI.Properties.Resources.plus_new;
                //pbLibraryClinical.Image = global::Acurus.Capella.UI.Properties.Resources.Database_Inactive;
                //pbClearClinical.Image = global::Acurus.Capella.UI.Properties.Resources.close_small_pressed;
                //pbSelectICD.Image = global::Acurus.Capella.UI.Properties.Resources.Database_Inactive;
            }
            else
            {
                //pbPlusClinicalNotes.Image = global::Acurus.Capella.UI.Properties.Resources.plus_new_disabled;
                //pbLibraryClinical.Image = global::Acurus.Capella.UI.Properties.Resources.Database_Disable;
                //pbClearClinical.Image = global::Acurus.Capella.UI.Properties.Resources.close_disabled;
                //pbSelectICD.Image = global::Acurus.Capella.UI.Properties.Resources.Database_Disable;
            }
            gbCenterDetail.Enabled = IsEnable;
            gbSelectICD.Enabled = IsEnable;
            gbOrderDetails.Enabled = IsEnable;
            gbProcedures.Enabled = IsEnable;
            gbSelectICD.Enabled = IsEnable;
            gbSpecimenDetails.Enabled = IsEnable;
            //CAP-667
            pbSelectICD.Disabled = !IsEnable;
            btnAllProcedures.Disabled = !IsEnable;
            btnSelectLocation.Disabled = !IsEnable;
            //btnPrintlabel.Enabled = IsEnable;
            //btnPrintRequsition.Enabled = IsEnable;
            //btnClearAll.Enabled = IsEnable; //Commentted for bugID:37656
            //btnFillQuestionSets.Enabled = IsEnable;
            //btnGenerateABN.Enabled = IsEnable;
            if (IsEnable == true)
                btnGenerateABN.Disabled = false;
            else
                btnGenerateABN.Disabled = true;
            if (IsEnable == false)
            {
                hdnToFindSource.Value = "false";
            }
            else
            {
                hdnToFindSource.Value = string.Empty;
            }
            //btnPlan.Enabled = IsEnable;
        }
        public void NewOrderClick()
        {
            //CAP-667 - disable gbSpecimenDetails, gbBillingDetails and gbOrderDetails all child.
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "$('#gbSpecimenDetails').find('*').attr('disabled', false);$('#gbBillingDetails').find('*').attr('disabled', false);$('#gbOrderDetails').find('*').attr('disabled', false);", true);
            disableallcontrols(true);
            ClearAll(true);
        }
        private void AssignDefaultValueToLab()
        {
            if (LookUpPerRequest.Keys.Contains("LabIdBasedOnIns") == true)
            {
                string[] FavLab = LookUpPerRequest["LabIdBasedOnIns"].Split(',');
                bool IsFavLabSet = false;
                for (int i = 0; i < cboLab.Items.Count; i++)
                {
                    if ((FavLab[0] == cboLab.Items[i].Value) && IsFavLabSet == false)
                    // if (FavLab.Contains(cboLab.Items[i].Value))
                    {
                        //cboLab.Text = cboLab.Items[i].Text;
                        cboLab.SelectedIndex = cboLab.Items.IndexOf(cboLab.Items[i]);
                        cboLab.Items[cboLab.SelectedIndex].Text = cboLab.Items[i].Text;
                        cboLab_SelectedIndexChanged(new object(), new EventArgs());//cboLab.Items[i].Text, cboLab.Items[cboLab.SelectedIndex].Text, cboLab.SelectedIndex.ToString(),cboLab.SelectedIndex.ToString()));//, cboLab.Items[i]..Index.ToString()));
                        IsFavLabSet = true;
                    }
                }
            }
        }
        private void AssignDefaultValueToLab(int LabID)
        {
            for (int i = 0; i < cboLab.Items.Count; i++)
            {
                if (cboLab.Items[i].Value == LabID.ToString())
                {
                    //cboLab.Text = cboLab.Items[i].Text;
                    cboLab.SelectedIndex = cboLab.Items.IndexOf(cboLab.Items[i]);
                    cboLab.Items[cboLab.SelectedIndex].Text = cboLab.Items[i].Text;
                    cboLab_SelectedIndexChanged(new object(), new EventArgs());
                    //cboLab_SelectedIndexChanged(new object(), new RadComboBoxSelectedIndexChangedEventArgs(cboLab.Items[i].Text, cboLab.Text, cboLab.SelectedItem.Index.ToString(), cboLab.Items[i].Index.ToString()));
                }
            }
        }
        public void ClearAll(bool IsNewOrder)
        {
            if (MoveToMA.Visible)
            {
                gbSpecimenDetails.Enabled = true;
            }
            //if (gbSelectICD.Enabled ||LookUpPerRequest["InHouseLabNameFromLookUp"].ToUpper() == "CMG ANC.-IN HOUSE")
            //{
            //    int[] ckcount = lstvFrequentlyUsedLabProcedures.CheckedIndices.Cast<Int32>().ToArray<int>();
            //    if (gbProcedures.Enabled != false)
            //    {
            //        for (int i = 0; i < ckcount.Length; i++)
            //        {
            //            lstvFrequentlyUsedLabProcedures.Items[ckcount[i]].Checked = false;
            //        }
            //    }
            //}
            chklstAssessment.ClearSelection();
            for (int i = 0; i < AssessmentSource.Count; i++)
            {
                chklstAssessment.Items[i].Selected = false;
            }
            if (IsNewOrder)
            {
                //lstvFrequentlyUsedLabProcedures.ForeColor = Color.Black;
                cboLab.SelectedIndex = 0;
                cboLab.Items[cboLab.SelectedIndex].Text = string.Empty;

                //cboLab.SelectedValue = "0";
                txtCenterName.Value = "";
                txtLocation.Value = "";
                cbospecimen.Items[cbospecimen.SelectedIndex].Text = string.Empty;
                cbospecimen.SelectedIndex = 0;
                if (lblUnits.InnerText.Contains('*'))
                {
                    lblUnits.InnerText = "Units"; //lblUnits.InnerText.Replace('*', ' ');
                    lblUnits.Attributes.Add("style", "color:black");
                    lblUnits.Attributes["Class"] = "";
                    lblUnits.Attributes["Class"] = "spanstyle";
                }
                if (LookUpPerRequest.ContainsKey("DefaultBillType"))
                {
                    if (LookUpPerRequest["DefaultBillType"].ToString() != string.Empty)
                        AssignDefaultValueToLab(Convert.ToInt32(LookUpPerRequest["DefaultBillType"]));
                    else
                        cboBillType.SelectedIndex = 0;
                }
                else
                    cboBillType.SelectedIndex = 0;

                //cboBillType.SelectedItem.Text = cboBillType.Items.Cast<RadComboBoxItem>().Where(a => a.Text.StartsWith("Pat")).Single();
                dbInsurancePlan.Enabled = false;
                chkYesABN.Checked = false;
                chkYesFasting.Checked = false;
                chkAuthRequired.Checked = false;
                chkSpecimenInHouse.Checked = false;
                SetCollectionDateMand(false);
                //hdnCollectionDateIsMand.Value = "false";
                chkSelectALLICD.Checked = false;
                chkMoveToMA.Checked = false;
                dtpCollectionDate.Value = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                SelectedTime(DateTime.Now);
                ViewState["CollectionDate"] = dtpCollectionDate.Value;
                //dtpCollectionDate.SelectedDate = DateTime.Now;
                txtQuantity.Value = "";
                cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text = string.Empty;
                cboSpecimenUnits.SelectedIndex = 0;
                txtOrderNotes.txtDLC.Text = "";
                chkStat.Checked = false;
                chkUrgent.Checked = false;
                if (lblCenterName.InnerText.Contains("*"))
                {
                    lblCenterName.InnerText = lblCenterName.InnerText.Replace('*', ' ');
                    lblCenterName.InnerText = lblCenterName.InnerText.Trim();
                    lblCenterName.Attributes.Add("style", "color:black");
                    SetReadOnlyStyle(txtCenterName);
                }
                if (lblLocation.InnerText.Contains("*"))
                {
                    lblLocation.InnerText = lblLocation.InnerText.Replace('*', ' ');
                    lblLocation.InnerText = lblLocation.InnerText.Trim();
                    lblLocation.Attributes.Add("style", "color:black");
                    SetReadOnlyStyle(txtLocation);
                }
                btnOrderSubmit.Attributes.Add("Tag", "SAVE");//btnOrderSubmit.Value = "SAVE";
                Session["OrderSubmitId"] = string.Empty;
                //chklstFrequentlyUsedProcedures.Items.Clear();
                //lstvFrequentlyUsedLabProcedures.Items.Clear();
                gbProcedures.Enabled = true;
                cboLab.Disabled = false;//cboLab.Enabled = true;
                chkMoveToMA.Disabled = false; //chkMoveToMA.Enabled = true;
                chkSpecimenInHouse.Checked = false;
                SetCollectionDateMand(false);
                //hdnCollectionDateIsMand.Value = "false";
                if (hdnLocalTime.Value.Trim() != string.Empty)
                    cstdtpTestDate.Value = Convert.ToDateTime(hdnLocalTime.Value).ToString("dd-MMM-yyyy");
                dtpCollectionDate.Value = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                SelectedTime(DateTime.Now);
                ViewState["CollectionDate"] = dtpCollectionDate.Value;
                //dtpCollectionDate.SelectedDate = DateTime.Now;
                chkTestDateInDate.Checked = true;
                AssignDefaultValueToLab();
                if (Session["cboLab"] != null)
                {
                    cboLab.SelectedIndex = Convert.ToInt32(Session["cboLab"]);
                }
                lblBillType.InnerText = "Bill Type*";
                lblBillType.Attributes.Add("style", "color:red");
                chklstAssessment.Enabled = true;
                if (ClientSession.UserCurrentProcess.ToUpper() == "MA_PROCESS")
                {
                    gbCenterDetail.Enabled = true;
                    gbSelectICD.Enabled = true;
                    gbProcedures.Enabled = true;
                    gbOrderDetails.Enabled = true;
                    gbSpecimenDetails.Enabled = true;
                }
                rbtnSubmitImmediately.Checked = true;
                IsNormalMode = true;
                chkpaperorder.Checked = false;
                rbImageOrder.Checked = false;
                rbLabOrder.Checked = false;
                rbImageOrder.Enabled = false;
                rbLabOrder.Enabled = false;
            }
            //The following code has been modified for BUG-ID:26470 - Pujhitha
            if (LookUpPerRequest.Keys.Contains("procedureType") == true)
            {
                if (cboLab.Items[cboLab.SelectedIndex].Value.Trim() != string.Empty)
                {
                    procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), Convert.ToUInt32(cboLab.Items[cboLab.SelectedIndex].Value), ClientSession.LegalOrg);
                }
                else
                {
                    procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), Convert.ToUInt32(cboLab.Items[1].Value), ClientSession.LegalOrg);
                }
            }
            FillLabProcedure(procedureList, new List<string>());
            btnClearAll.Value = "Clear All";
            if (hdnForEditErrorMsg.Value == "Edit")
                hdnForEditErrorMsg.Value = "";
            btnOrderSubmit.Disabled = true;
            IsDefaultLab = false;
            //----------------------------------------------------------------------
        }
        public void LoadScreenFromOrderList(DiagnosticDTO objDiagnosticDTO)
        {
            string OrdersSubmitIDString = hdnOrderSubmitID.Value != null ? hdnOrderSubmitID.Value.ToString() : string.Empty;
            if (OrdersSubmitIDString != string.Empty)
            {
                ulong OrdersSubmitId = Convert.ToUInt32(OrdersSubmitIDString);
                if (objDiagnosticDTO == null && (LookUpPerRequest.Keys.Contains("procedureType") == true))
                {
                    //DiagnosticDTO objDiagnosticDTO = new DiagnosticDTO();
                    objDiagnosticDTO = objOrdersManager.FillDiagnosticDTO(OrdersSubmitId, EncounterID, HumanID, PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), ClientSession.FacilityName, ClientSession.LegalOrg);
                }
                OrdersSubmit subtempobj = objDiagnosticDTO.objOrdersSubmit;
                HumanID = subtempobj.Human_ID;
                EncounterID = subtempobj.Encounter_ID;
                PhysicianID = subtempobj.Physician_ID;
                IsEditable = objOrdersManager.IsEditable(subtempobj.Id, subtempobj.Order_Type);
                if (!IsEditable)
                {
                    //CAP-667 - disable gbSpecimenDetails, gbBillingDetails and gbOrderDetails all child.
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230139');$('#gbSpecimenDetails').find('*').attr('disabled', true);$('#gbBillingDetails').find('*').attr('disabled', true);$('#gbOrderDetails').find('*').attr('disabled', true);", true);
                    // ApplicationObject.erroHandler.DisplayErrorMessage("230139", OrderType.ToUpper(), this);
                    disableallcontrols(false);
                }
                btnClearAll.Disabled = false; //btnClearAll.Enabled = true;
                AssignDefaultValueToLab(Convert.ToInt32(subtempobj.Lab_ID));
                txtCenterName.Value = subtempobj.Lab_Name;
                if (txtCenterName.Value.StartsWith("In-H"))
                {
                    gbSelectICD.Enabled = false;
                }
                txtLocation.Value = subtempobj.Lab_Location_Name;
                if (subtempobj.Lab_Location_ID != 0)
                {
                    if (!LookUpPerRequest.ContainsKey("labLocID"))
                        LookUpPerRequest.Add("labLocID", subtempobj.Lab_Location_ID.ToString());
                    else
                        LookUpPerRequest["labLocID"] = subtempobj.Lab_Location_ID.ToString();
                }

                //The following code has been modified for BUG-ID:26468 - Pujhitha
                foreach (ListItem item in cboBillType.Items)
                {
                    if (item.Text.Trim().ToUpper() == subtempobj.Bill_Type.Trim().ToUpper())
                    {
                        item.Selected = true;
                        //cboBillType.SelectedIndex = item.Index;
                        cboBillType.Items[cboBillType.SelectedIndex].Text = item.Text;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                //---------------------------------------------------------------------               
                if (cboBillType.Items[cboBillType.SelectedIndex].Text.Contains("Third"))
                    dbInsurancePlan.Enabled = true;
                else
                    dbInsurancePlan.Enabled = false;
                if (subtempobj.Is_Urgent == "Y")
                    chkUrgent.Checked = true;
                else
                    chkUrgent.Checked = false;
                if (subtempobj.Stat != string.Empty)
                {
                    chkStat.Checked = YesOrNo(subtempobj.Stat);
                    chkStat_CheckedChanged(new object(), new EventArgs());
                    chkUrgent.Checked = YesOrNo(subtempobj.Is_Urgent);
                    chkTestDateInDate.Checked = true;
                    if ((subtempobj.Test_Date != "") && (!subtempobj.Test_Date.Contains("0001")))
                    {
                        cstdtpTestDate.Value = DateTime.ParseExact(subtempobj.Test_Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        //cstdtpTestDate.Value = DateTime.ParseExact(subtempobj.Test_Date, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        hdnsaveEnable.Value = "Load";
                    }

                }
                else if (subtempobj.TestDate_In_Days != 0 || subtempobj.TestDate_In_Months != 0 || subtempobj.TestDate_In_Weeks != 0)
                {
                    chkTestDateInWords.Checked = true;
                    chkTestDateInWords_CheckedChanged(new object(), new EventArgs());
                    if (subtempobj.TestDate_In_Days != 0)
                        txtDays.Value = subtempobj.TestDate_In_Days.ToString();
                    if (subtempobj.TestDate_In_Months != 0)
                        txtMonths.Value = subtempobj.TestDate_In_Months.ToString();
                    if (subtempobj.TestDate_In_Weeks != 0)
                        txtWeeks.Value = subtempobj.TestDate_In_Weeks.ToString();

                }
                // The following code has been commented for BUG-ID:26160 - Pujhitha
                //if (chkMoveToMA.Visible)
                // gbSpecimenDetails.Enabled = false;
                //
                chkSpecimenInHouse.Checked = YesOrNo(subtempobj.Specimen_In_House);
                chkSpecimenInHouse_CheckedChanged(new object(), new EventArgs());
                chkAuthRequired.Checked = YesOrNo(subtempobj.Authorization_Required);
                chkMoveToMA.Checked = YesOrNo(subtempobj.Move_To_MA);
                chkMoveToMA_CheckedChanged(new object(), new EventArgs());
                if (subtempobj.Is_Submit_Immediately == "Y")
                    rbtnSubmitImmediately.Checked = true;
                else
                    rbtnSubmitAfter3Hour.Checked = true;
                //rbtnSubmitAfter3Hour.Checked = false;
                //The following code has been modified for BUG-ID:26468 - Pujhitha
                foreach (ListItem item in cboSpecimenUnits.Items)
                {
                    if (item.Text.Trim().ToUpper() == subtempobj.Specimen_Unit.Trim().ToUpper())
                    {
                        item.Selected = true;
                        //cboSpecimenUnits.SelectedIndex = item.Index;
                        cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text = item.Text;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                if (subtempobj.Quantity.ToString() != "0")
                    txtQuantity.Value = subtempobj.Quantity.ToString();
                foreach (ListItem item in cbospecimen.Items)
                {
                    if (item.Text.Trim().ToUpper() == subtempobj.Specimen_Type.Trim().ToUpper())
                    {
                        item.Selected = true;
                        //cbospecimen.SelectedIndex = item.Index;
                        cbospecimen.Items[cbospecimen.SelectedIndex].Text = item.Text;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                //-------------------------------------------------------------------            
                txtOrderNotes.txtDLC.Text = subtempobj.Order_Notes;
                if (subtempobj.Fasting.ToUpper() == "Y")
                {
                    chkYesFasting.Checked = true;
                }
                if (subtempobj.Is_ABN_Signed.ToUpper() == "Y")
                {
                    chkYesABN.Checked = true;
                }
                if (subtempobj.Is_Paper_Order.ToUpper() == "Y")
                {
                    chkpaperorder.Checked = true;
                }

                //lstvFrequentlyUsedLabProcedures.Items.Clear();

                //if (subtempobj.Specimen_Collection_Date_And_Time == DateTime.MinValue)
                //{
                //    //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                //    string strtime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString();
                //    dtpCollectionDate.DateInput.SelectedDate = Convert.ToDateTime(strtime);
                //    SelectedTime(Convert.ToDateTime(strtime));
                //    ViewState["CollectionDate"] = dtpCollectionDate.DateInput.SelectedDate;
                //}
                //else
                //{
                //    dtpCollectionDate.DateInput.SelectedDate = UtilityManager.ConvertToLocal(subtempobj.Specimen_Collection_Date_And_Time);
                //    SelectedTime(UtilityManager.ConvertToLocal(subtempobj.Specimen_Collection_Date_And_Time));
                //    ViewState["CollectionDate"] = dtpCollectionDate.DateInput.SelectedDate;
                //}

                if (subtempobj.Specimen_Collection_Date_And_Time != DateTime.MinValue)
                {
                    dtpCollectionDate.Value = UtilityManager.ConvertToLocal(subtempobj.Specimen_Collection_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    SelectedTime(UtilityManager.ConvertToLocal(subtempobj.Specimen_Collection_Date_And_Time));
                    ViewState["CollectionDate"] = dtpCollectionDate.Value;
                }


                //dtpCollectionDate.SelectedDate = UtilityManager.ConvertToLocal(subtempobj.Specimen_Collection_Date_And_Time);
                //if (subtempobj.Specimen_Collection_Date_And_Time == DateTime.MinValue)
                //    dtpCollectionDate.CustomFormat = " ";
                //else

                //procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), subtempobj.Lab_ID);
                //procedureList = objDiagnosticDTO.procedureList;
                //FillLabProcedure(procedureList, new List<string>());
                //IList<ListItem> tempList = new List<ListItem>();
                //IList<string> ProceduresViewList = new List<string>();
                //ProceduresViewList = objDiagnosticDTO.OrdersLists.Select(a => a.Lab_Procedure + "-" + a.Lab_Procedure_Description).ToList<string>();
                //Session["ProcEditID"] = objDiagnosticDTO.OrdersLists[0].Order_Submit_ID;
                //foreach (string str in ProceduresViewList)
                //{
                //    if (str != string.Empty)
                //    {
                //        ListItem foundItem = chklstFrequentlyUsedProcedures.Items.FindByText(str);
                //        if (foundItem != null)
                //            foundItem.Selected = true;
                //        else
                //        {

                //            ListItem OtherItemIsFound = chklstFrequentlyUsedProcedures.Items.FindByText("OTHER");
                //            if (OtherItemIsFound != null && OtherItemIsFound.Text != "OTHER")
                //                OtherItemIsFound = null;
                //            if (OtherItemIsFound == null)
                //            {
                //                ListItem itemOthers = new ListItem("OTHER");
                //                itemOthers.Attributes.Add("style", "color:Blue;");// = Color.Blue;
                //                //itemOthers.Font = new Font("Arial", 10, FontStyle.Bold);

                //                chklstFrequentlyUsedProcedures.Items.Add(itemOthers);
                //            }

                //            ListItem item = new ListItem(str);
                //            item.Selected = true;
                //            tempList.Add(item);
                //            //lstvFrequentlyUsedLabProcedures.Items.Add(item);
                //        }
                //    }
                //}

                //if (Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value) == 32 && chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected))
                //    btnImportresult.Disabled = false; //btnImportresult.Enabled = true;
                //else
                //    btnImportresult.Disabled = true; //btnImportresult.Enabled = false;

                //Session["ProceduresViewList"] = ProceduresViewList;
                //if (tempList.Count > 0)
                //{
                //    int insertIndex = FindInsertStartIndex();
                //    foreach (ListItem obj in tempList)
                //    {
                //        chklstFrequentlyUsedProcedures.Items.Insert(insertIndex, obj);
                //    }
                //}
                procedureList = objDiagnosticDTO.procedureList;
                FillLabProcedure(procedureList, new List<string>());

                IList<string> ProceduresViewList = new List<string>();
                ProceduresViewList = objDiagnosticDTO.OrdersLists.Select(a => a.Lab_Procedure + "-" + (a.Lab_Procedure_Description.Contains("x_") ? ((a.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None).Count() > 1 ? (a.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None)[0].ToString() + "x______") : a.Lab_Procedure_Description) + "~" + a.Quantity + "|" + a.Id) : a.Lab_Procedure_Description)).ToList<string>();
                Session["ProcEditID"] = objDiagnosticDTO.OrdersLists[0].Order_Submit_ID;
                IList<ListItem> tempList = SetOrderIDForEditQuantity(ProceduresViewList);
                //moved to sperate method
                //IList<ListItem> tempList = new List<ListItem>();
                //for (int i = 0; i < ProceduresViewList.Count;i++)
                //{
                //    if (ProceduresViewList[i] != string.Empty)
                //    {
                //        ListItem foundItem = chklstFrequentlyUsedProcedures.Items.FindByText(ProceduresViewList[i].Split('~')[0]);
                //        if (foundItem != null)
                //        {
                //            foundItem.Selected = true;
                //            if (ProceduresViewList[i].Contains('~'))
                //            {
                //                if (ProceduresViewList[i].Split('~')[1].Split('|')[0] != "" && ProceduresViewList[i].Split('~')[1].Split('|')[0] != "0")
                //                {
                //                    string Qty = "x___" + ProceduresViewList[i].Split('~')[1].Split('|')[0] + "___";
                //                    foundItem.Text = foundItem.Text.Replace("x______", Qty);

                //                }
                //                if (ProceduresViewList[i].Split('~')[1].Split('|')[0] != "")
                //                    foundItem.Attributes.Add("orderid", ProceduresViewList[i].Split('~')[1].Split('|')[1]);
                //                ProceduresViewList[i] = foundItem.Text;
                //            }
                //        }

                //        else
                //        {

                //            ListItem OtherItemIsFound = chklstFrequentlyUsedProcedures.Items.FindByText("OTHER");
                //            if (OtherItemIsFound != null && OtherItemIsFound.Text != "OTHER")
                //                OtherItemIsFound = null;
                //            if (OtherItemIsFound == null)
                //            {
                //                ListItem itemOthers = new ListItem("OTHER");
                //                itemOthers.Attributes.Add("style", "color:Blue;");// = Color.Blue;
                //                //itemOthers.Font = new Font("Arial", 10, FontStyle.Bold);

                //                chklstFrequentlyUsedProcedures.Items.Add(itemOthers);
                //            }

                //            ListItem item = new ListItem(ProceduresViewList[i]);
                //            item.Selected = true;
                //            tempList.Add(item);
                //            //lstvFrequentlyUsedLabProcedures.Items.Add(item);
                //        }
                //    }
                //}

                if (Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value) == 32 && chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected))
                    btnImportresult.Disabled = false; //btnImportresult.Enabled = true;
                else
                    btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
                ////IList<string> ProceduresViewListNew = new List<string>();
                ////ProceduresViewListNew = objDiagnosticDTO.OrdersLists.Select(a => a.Lab_Procedure + "-" + a.Lab_Procedure_Description).ToList<string>();
                //Session["ProceduresViewList"] = ProceduresViewList;
                //if (tempList.Count > 0)
                //{
                //    int insertIndex = FindInsertStartIndex();
                //    foreach (ListItem obj in tempList)
                //    {
                //        chklstFrequentlyUsedProcedures.Items.Insert(insertIndex, obj);
                //    }
                //}
                ////CheckedICDS = new List<string>();
                ////To deselect all ICDS
                for (int i = 0; i < chklstAssessment.Items.Count; i++)
                {
                    chklstAssessment.Items[i].Selected = false;
                }
                //To Select ICDs While Editing
                IList<string> SelectedICD = new List<string>();
                SelectedICD = objDiagnosticDTO.OrderAssList.Select(a => a.ICD + "-" + a.ICD_Description).ToList<string>();
                foreach (string str in SelectedICD)
                {
                    if (AssessmentSource.ContainsKey(str.Split('-')[0]))
                        chklstAssessment.Items[GetIndexOfAICD(str.Split('-')[0])].Selected = true;
                    else
                    {
                        if (!AssessmentSource.Any(a => a.Key == str.Split('-')[0].ToString()))
                        {
                            ListItem objListItem = new ListItem();
                            objListItem.Text = str;
                            objListItem.Selected = true;
                            chklstAssessment.Items.Add(objListItem);
                            AssessmentSource.Add(str.Split('-')[0], "OTHERS-0");
                        }
                    }
                }
                btnOrderSubmit.Attributes.Add("Tag", "UPDATE"); //btnOrderSubmit.Value = "UPDATE";
                //the following line has been added for BugID:26470 -Pujhitha
                btnClearAll.Value = "Cancel";
                //-------------------------------------------------------
                if (Request["IsFrontScreen"] != null && Request["IsFrontScreen"] == "Y")
                {
                    gbCenterDetail.Enabled = false;
                }
            }
        }
        int FindInsertStartIndex()
        {
            int indexOther = CheckForOtherHeader();
            indexOther = indexOther + 1;
            int insertIndex = 0;
            if (indexOther < chklstFrequentlyUsedProcedures.Items.Count)
            {
                for (int i = indexOther; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
                {
                    string ItemStyleContent = chklstFrequentlyUsedProcedures.Items[i].Attributes["Style"];
                    if (ItemStyleContent != null && ItemStyleContent != string.Empty && ItemStyleContent.Contains("Blue"))
                    {
                        insertIndex = i;
                        break;
                    }
                }
            }
            if (insertIndex == 0)
                return indexOther;
            else if (insertIndex != indexOther)
                return insertIndex;
            else
                return indexOther;

        }
        int CheckForOtherHeader()
        {
            for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
            {
                if (chklstFrequentlyUsedProcedures.Items[i].Text == "OTHER")
                {
                    return i;
                }
            }
            return 0;
        }
        private int GetIndexOfAICD(string ICD)
        {
            int IndexFound = 0;
            for (int i = 0; i < chklstAssessment.Items.Count; i++)
            {
                string temp = chklstAssessment.Items[i].ToString();
                if (temp.StartsWith(ICD))
                {
                    IndexFound = i;
                    break;
                }
            }
            return IndexFound;
        }
        //ListViewItem CheckForOtherHeader(int IndexAfter)
        //{
        //    ListViewItem OtherItemIsFound = lstvFrequentlyUsedLabProcedures.FindItemWithText("OTHER", false, IndexAfter);
        //    if (OtherItemIsFound.Text == "OTHER")
        //        return OtherItemIsFound;
        //    else
        //        return CheckForOtherHeader(OtherItemIsFound.Index + 1);
        //}
        //int FindInsertStartIndex()
        //{
        //    int indexOther = CheckForOtherHeader(0).Index;
        //    indexOther = indexOther + 1;
        //    int insertIndex = 0;
        //    if (indexOther < lstvFrequentlyUsedLabProcedures.Items.Count)
        //    {
        //        for (int i = indexOther; i < lstvFrequentlyUsedLabProcedures.Items.Count; i++)
        //        {
        //            if (lstvFrequentlyUsedLabProcedures.Items[i].ForeColor == Color.Blue)
        //            {
        //                insertIndex = i;
        //                break;
        //            }
        //        }
        //    }
        //    if (insertIndex == 0)
        //        return indexOther;
        //    else if (insertIndex != indexOther)
        //        return insertIndex;
        //    else
        //        return indexOther;

        //}
        bool YesOrNo(string YOrN)
        {
            if (YOrN.ToUpper() == "Y")
            {
                return true;
            }
            else
                return false;
        }
        bool ShowRequiredDocumnets(string SaveOrUpdate)
        {
            FillHumanDTO objFillHumnaDTO = new FillHumanDTO();
            objFillHumnaDTO = (FillHumanDTO)Session["objFillHumanDTO"];
            string[] seprator = new string[1] { "  ,  " };
            IList<string> tempCpts = new List<string>();
            //for(int i=0;i<objfrmOrdersList.grdOrders.Rows.Count;i++)
            //{

            //    tempCpts = tempCpts.Concat(objfrmOrdersList.grdOrders.Rows[i].Cells["Procedure"].Value.ToString().Split(seprator, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Split('-')[0]).ToList<string>()).ToList();
            //}
            IList<string> SelectedCPT = new List<string>();
            for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
            {
                if (chklstFrequentlyUsedProcedures.Items[i].Selected)
                    SelectedCPT.Add(chklstFrequentlyUsedProcedures.Items[i].Text);
            }
            for (int k = 0; k < SelectedCPT.Count; k++)
            {
                string itemText = SelectedCPT[k];
                tempCpts.Add(itemText.Split('-')[0]);
            }
            tempCpts = tempCpts.Distinct().ToList();
            ulong insuranceId = 0;
            if (objFillHumnaDTO != null && objFillHumnaDTO.PatientInsuredBag != null && objFillHumnaDTO.PatientInsuredBag.Count > 0)
            {
                if (objFillHumnaDTO.PatientInsuredBag != null)
                {

                    IList<ulong> idList = (from pat in objFillHumnaDTO.PatientInsuredBag where pat.Insurance_Type.ToUpper() == "PRIMARY" && pat.Active.ToUpper() == "YES" select pat.Insurance_Plan_ID).ToList<ulong>();
                    if (idList.Count > 0)
                    {
                        insuranceId = idList[0];

                    }
                }
            }


            //Added by saravanan
            //FillHumanDTO humanRecord;
            //var serializer = new NetDataContractSerializer();
            //OrdersDTO objOrderDTO = null;
            //object objDTO;
            //objDTO = (object)serializer.ReadObject(objOrdersManager.LoadOrders(EncounterID, PhysicianID, HumanID, OrderType, string.Empty, UtilityManager.ConvertToUniversal(DateTime.Now), false));
            //objOrderDTO = (OrdersDTO)objDTO;
            //humanRecord = objOrderDTO.objHuman;

            string sTempCpts = string.Empty;
            foreach (string scpt in tempCpts)
            {
                if (sTempCpts == string.Empty)
                    sTempCpts = scpt;
                else
                    sTempCpts += "_" + scpt;
            }

            string sPhoneNo = string.Empty;

            // sPhoneNo = objFillHumnaDTO.Home_Phone_No.Replace("(", "O");
            if (ClientSession.PatientPaneList.Count > 0)
            {
                sPhoneNo = ClientSession.PatientPaneList[0].HomePhoneNo.Replace("(", "O");
                sPhoneNo = sPhoneNo.Replace(")", "C");
                sPhoneNo = sPhoneNo.Replace("-", "_");
                sPhoneNo = sPhoneNo.Replace(" ", "S");
            }
            //frmPlanCptFormsRequired objfrmPlanCptFormsRequired;


            if (SaveOrUpdate == "UPDATE")
            {
                SaveOrUpdate = "Update";
                string sPreferred_Reading_Physician_ID = string.Empty;
                string scriptContent = string.Empty;
                if (Request["EditedOrderSubmitID"] != null)
                {
                    sPreferred_Reading_Physician_ID = objOrderRequiredMngr.sPreferred_ReadingPhysician(Convert.ToUInt32(Request["EditedOrderSubmitID"]));
                    scriptContent = "OpenPlanCptUpdate('" + sPhoneNo + "','" + SaveOrUpdate + "','" + sTempCpts + "','" + HumanID + "','" + Request["EditedOrderSubmitID"].ToString() + "','" + sPreferred_Reading_Physician_ID + "');";
                }
                else if (sCmgorder == "Y")
                {
                    sPreferred_Reading_Physician_ID = objOrderRequiredMngr.sPreferred_ReadingPhysician(Convert.ToUInt32(hdnOrderSubmitID.Value));
                    scriptContent = "OpenPlanCptUpdate('" + sPhoneNo + "','" + SaveOrUpdate + "','" + sTempCpts + "','" + HumanID + "','" + hdnOrderSubmitID.Value.ToString() + "','" + sPreferred_Reading_Physician_ID + "');";

                }
                //objfrmPlanCptFormsRequired = new frmPlanCptFormsRequired(objFillHumnaDTO.Home_Phone_No, "UPDATE", ilstOrdersRequiredForms, tempCpts, humanRecord, ulMyHumanID, OrderViewObject.ilstOrderLabDetailsDTO.Select(a => a.OrdersSubmit.Preferred_Reading_Physician_ID).Take(1).SingleOrDefault());
                // objfrmPlanCptFormsRequired = new frmPlanCptFormsRequired(objFillHumnaDTO.Home_Phone_No, "UPDATE", ilstOrdersRequiredForms, tempCpts, humanRecord, ulMyHumanID, OrderViewObject.ilstOrderLabDetailsDTO.Select(a => a.OrdersSubmit.Preferred_Reading_Physician_ID).Take(1).SingleOrDefault());

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", scriptContent, true);
                return false;
            }

            else //(SaveOrUpdate == "SAVE")
            {
                SaveOrUpdate = "Save";
                //string sTempCpts=string.Empty;
                //foreach(string scpt in tempCpts)
                //{
                //    if(sTempCpts==string.Empty)
                //        sTempCpts=scpt;
                //    else
                //        sTempCpts+="_"+scpt;
                //}

                //string sPhoneNo;
                //sPhoneNo = objFillHumnaDTO.Home_Phone_No.Replace("(", "O");
                //sPhoneNo = sPhoneNo.Replace(")", "C");
                //sPhoneNo = sPhoneNo.Replace("-", "_");
                //sPhoneNo = sPhoneNo.Replace(" ", "S");
                //string scriptContent = "OpenPlanCpt('" + insuranceId + "','" + sTempCpts + "','" + sPhoneNo + "');";
                string scriptContent = "OpenPlanCpt('" + insuranceId + "','" + sTempCpts + "','" + sPhoneNo + "','" + SaveOrUpdate + "','" + HumanID + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", scriptContent, true);
                return false;
                //objfrmPlanCptFormsRequired = new frmPlanCptFormsRequired(insuranceId, tempCpts, objFillHumnaDTO.Home_Phone_No, SaveOrUpdate, humanRecord, ulMyHumanID);
            }

            //objfrmPlanCptFormsRequired.ShowDialog();
            //if (objfrmPlanCptFormsRequired.DoSave && objFillHumnaDTO.Home_Phone_No != objfrmPlanCptFormsRequired.UpdatedPhoneNumber)
            //{
            //    objFillHumnaDTO.Home_Phone_No = objfrmPlanCptFormsRequired.UpdatedPhoneNumber;
            //    ModifiedPhoneNumber = objfrmPlanCptFormsRequired.UpdatedPhoneNumber;
            //}
            //PreferredReadingPhysician = objfrmPlanCptFormsRequired.SelectedPhysicianID;
            //if (!objfrmPlanCptFormsRequired.DoSave)
            //    return false;
            //ilstOrdersRequiredForms = new List<OrdersRequiredForms>();

            //ilstOrdersRequiredForms = objfrmPlanCptFormsRequired.ilstReturnList;

            //ilstRequiredForms = ClientSession.RequiredForms_ReturnList;
            return true;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string updateOrderQuantity(string Quantity)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string[] quantityp = Quantity.Split('~');
            IList<Orders> lstUpdate = new List<Orders>();
            IList<Orders> lstsavenull = null;
            Orders objorder = new Orders();
            OrdersManager objOrderMngr = new OrdersManager();
            ulong[] uID = new ulong[quantityp.Count()];
            ulong uEncounterID = 0;
            for (int i = 0; i < quantityp.Count(); i++)
            {
                if (quantityp[i].Split('|').Count() > 0 && quantityp[i].Split('|')[1] != null && quantityp[i].Split('|')[1] != string.Empty)
                {
                    uID[i] = Convert.ToUInt32(quantityp[i].Split('|')[1]);
                }
            }
            IList<Orders> lstOrders = new List<Orders>();
            lstOrders = objOrderMngr.GetOrdersByOrderID(uID);
            if (lstOrders != null && lstOrders.Count > 0)
            {
                for (int i = 0; i < quantityp.Count(); i++)
                {
                    objorder = new Orders();
                    string[] xid = quantityp[i].Split('|');
                    //lstTempOrders = lstOrders.Where(a => a.Id == Convert.ToUInt32(xid[1])).ToList<Orders>();
                    objorder = (from Orders u in lstOrders where u.Id == Convert.ToUInt32(xid[1]) select u).First();
                    objorder.Quantity = Convert.ToDecimal(xid[0]);
                    string Qty = "x___" + objorder.Quantity + "___";
                    objorder.Lab_Procedure_Description = (objorder.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None).Count() > 1 ? (objorder.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None)[0].ToString() + Qty) : objorder.Lab_Procedure_Description);
                    uEncounterID = objorder.Encounter_ID;
                    lstUpdate.Add(objorder);
                }
                objOrderMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref lstsavenull, ref lstUpdate, null, string.Empty, true, true, ClientSession.HumanId, string.Empty);

                //Update in treatmentplan table and Xml
                TreatmentPlanManager objTreatmentMngr = new TreatmentPlanManager();
                IList<TreatmentPlan> lstUpdatePlan = new List<TreatmentPlan>();
                IList<TreatmentPlan> lstsaveplannull = null;
                IList<TreatmentPlan> lstUpdatePlanTemp = new List<TreatmentPlan>();
                TreatmentPlan objPlan = new TreatmentPlan();
                lstUpdatePlanTemp = objTreatmentMngr.GetTreatmentPlanusingSourceID(uID, uEncounterID);
                if (lstUpdatePlanTemp != null && lstUpdatePlanTemp.Count > 0)
                {
                    foreach (TreatmentPlan Plantemp in lstUpdatePlanTemp)
                    {
                        objPlan = new TreatmentPlan();
                        objPlan = Plantemp;
                        objorder = new Orders();
                        objorder = (from Orders u in lstUpdate where u.Id == objPlan.Source_ID select u).First();
                        objPlan.Plan = "* " + objorder.Lab_Procedure_Description;
                        lstUpdatePlan.Add(objPlan);
                    }
                    objTreatmentMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref lstsaveplannull, ref lstUpdatePlan, null, string.Empty, true, true, lstUpdatePlan[0].Encounter_Id, string.Empty);
                }
            }

            //DiagnosticDTO objDiagnosticDTO = new DiagnosticDTO();
            //OrdersManager objOrdersManager = new OrdersManager();
            //objDiagnosticDTO = objOrdersManager.FillDiagnosticDTO(sCmgorderSubmitId, EncounterID, HumanID, PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), ClientSession.FacilityName);
            //LoadScreenFromOrderList(objDiagnosticDTO);
            //FillAssesmentAndProblemListICD(objDiagnosticDTO.AssessmentList, objDiagnosticDTO.MedAdvProblemList);
            //for (int i = 0; i < quantityp.Count(); i++)
            //{
            //    string[] xid = quantityp[i].Split('|');
            //    objorder = objOrderMngr.GetById(Convert.ToUInt32(xid[1]));
            //    objorder.Quantity = Convert.ToDecimal(xid[0]);
            //    string Qty = "x___" + objorder.Quantity + "___";
            //    objorder.Lab_Procedure_Description = (objorder.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None).Count() > 1 ? (objorder.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None)[0].ToString() + Qty) : objorder.Lab_Procedure_Description);       
            //    //objor.Lab_Procedure_Description = objor.Lab_Procedure_Description.Replace("x______", Qty);
            //    lstUpdate.Add(objorder);
            //}


            return "";

        }
        DateTime timeOnAddClick = new DateTime();
        bool ValidateSave()
        {
            //if (ClientSession.UserRole == "Medical Assistant" && cboLab.SelectedItem.Text == "CMG Anc.-In House")
            //{
            //    chkMoveToMA.Checked = false;
            //}Commented for Bug Id: 29155
            bool bReturnValue = false;
            if (chkMoveToMA.Checked)
            {
                if (ClientSession.UserRole != "Medical Assistant" && ClientSession.UserCurrentProcess != "MA_REVIEW")
                    bReturnValue = IsICDIsCheckedWithOutCPT();
                else
                    bReturnValue = CheckForValidation();
                //return IsICDIsCheckedWithOutCPT();
                //return true;
            }
            else
            {
                bReturnValue = CheckForValidation();
                //return CheckForValidation();
            }
            return bReturnValue;

        }
        public void CancelTextChanged()
        {
            if (hdnForEditErrorMsg.Value == "Edit")
            {
                btnClearAll.Value = "Cancel";
                //System.Web.UI.HtmlControls.HtmlGenericControl txtcancel = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                //txtcancel.InnerText = "C";
                //System.Web.UI.HtmlControls.HtmlGenericControl txtcancel1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                //txtcancel1.InnerText = "ancel";
            }
            else
            {
                btnClearAll.Value = "Clear All";
                //System.Web.UI.HtmlControls.HtmlGenericControl txtcancel = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                //txtcancel.InnerText = "C";
                //System.Web.UI.HtmlControls.HtmlGenericControl txtcancel1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                //txtcancel1.InnerText = "lear All";
            }
        }

        protected void btnOrderSubmit_Click(object sender, EventArgs e)
        {


            hdnImportResult.Value = "false";
            string YesNoCancel = hdnType.Value;
            hdnType.Value = string.Empty;
            btnOrderSubmit.Disabled = false; //btnOrderSubmit.Enabled = true;
            if (hdnpaper.Value == "rbImageOrder")
                rbImageOrder.Checked = true;
            else if (hdnpaper.Value == "rbLabOrder")
                rbLabOrder.Checked = true;

            if (chkpaperorder.Checked == true)
            {
                if (rbImageOrder.Checked == false && rbLabOrder.Checked == false)
                {
                    hdnImportResult.Value = "true";
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('182223'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                else
                {
                    OrdersSubmitManager OrdersSubmitMngr = new OrdersSubmitManager();
                    IList<OrdersSubmit> ilstSaveOrdersSubmitList = new List<OrdersSubmit>();
                    OrdersSubmit FilledOrdersSubmit = new OrdersSubmit();
                    FilledOrdersSubmit.Human_ID = HumanID;
                    FilledOrdersSubmit.Encounter_ID = EncounterID;
                    FilledOrdersSubmit.Physician_ID = PhysicianID;
                    FilledOrdersSubmit.Is_Paper_Order = "Y";
                    if (rbImageOrder.Checked == true)
                    {
                        FilledOrdersSubmit.Lab_ID = 5;
                    }
                    if (rbLabOrder.Checked == true)
                    {
                        FilledOrdersSubmit.Lab_ID = 6;
                    }
                    FilledOrdersSubmit.Created_By = ClientSession.UserName;
                    //objOrderSubmit.Created_Date_And_Time = timeOnAddClick;
                    //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                    //FilledOrdersSubmit.Created_Date_And_Time = Convert.ToDateTime(strtime);
                    FilledOrdersSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    ilstSaveOrdersSubmitList.Add(FilledOrdersSubmit);

                    WFObject wfObject = new WFObject();
                    wfObject.Obj_Type = "DIAGNOSTIC ORDER";
                    wfObject.Current_Process = "BILLING_WAIT";
                    //wfObject.Current_Arrival_Time = Convert.ToDateTime(strtime);
                    wfObject.Current_Arrival_Time = UtilityManager.ConvertToUniversal();
                    IList<WFObject> ilstWFobject = new List<WFObject>();
                    ilstWFobject.Add(wfObject);
                    OrdersSubmitMngr.CreateOrderSubmitAndWFObject(ilstSaveOrdersSubmitList, ilstWFobject, string.Empty);


                    //if (TriggerClearAll)
                    if (YesNoCancel == "Yes")
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "var win=radalert('Submitted Successfully.',null,null,'Diagnostic Order'); win.setSize(250, 100); win.center(); win.set_iconUrl('Resources/16_16.ico'); $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none'); $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none');  window.setTimeout(function() { win.close();}, 2000);", true);
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "LabOrder_SavedSuccessfully();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "LabOrder_SavedSuccessfully();", true);
                    }
                    //else
                    //if (YesNoCancel == "Yes")
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "LabOrder_SavedSuccessfully();", true);
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "LabOrder_SavedSuccessfully(;)", true);
                    //}
                    btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
                    ClearAll(true);
                    hdnpaper.Value = "";
                }


                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


                // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);

            }
            else
            {
                RadWindowImportResult.VisibleOnPageLoad = false;
                Session["IsSaved"] = "false";
                if (IsEditable && cboLab.Items[cboLab.SelectedIndex].Text.Trim() != string.Empty && Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value) == 32)
                {
                    SetCollectionDateMand(true);
                    if (chkSpecimenInHouse.Checked == true && chkMoveToMA.Checked == true)
                    {

                    }
                    else if (dtpCollectionDate.Value == string.Empty)
                    {
                        hdnImportResult.Value = "true";
                        IsCollectionDate = true;

                        //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('115040'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('115040');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);

                        return;
                    }
                }

                if (ValidateSave())
                {
                    if (LookUpPerRequest.Keys.Contains("CMGLabNameFromLookUp") == true)
                    {
                        //if (cboLab.Items[cboLab.SelectedIndex].Text == LookUpPerRequest["CMGLabNameFromLookUp"].ToString() && (!chkMoveToMA.Checked || ClientSession.UserCurrentProcess == "MA_REVIEW"))
                        if (LookUpPerRequest["CMGLabNameFromLookUp"].ToString().Contains(cboLab.Items[cboLab.SelectedIndex].Text) == true && (!chkMoveToMA.Checked || ClientSession.UserCurrentProcess == "MA_REVIEW"))
                        {
                            if (btnOrderSubmit.Attributes["Tag"] != null && !ShowRequiredDocumnets(btnOrderSubmit.Attributes["Tag"].ToString()))
                            {
                                //ModifiedPhoneNumber = string.Empty;
                                //PreferredReadingPhysician = 0;
                                //divLoading.Style.Add("display", "none");

                                hdnImportResult.Value = "true";
                                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                //CAP-1152
                                hdnCMGAncillarySaveOrder.Value = "true";
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ValidateSave", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                                return;
                            }
                        }
                    }

                    //{

                    //System.Diagnostics.Stopwatch stopwatch = MyLog.StartWatch();
                    timeOnAddClick = UtilityManager.ConvertToUniversal(DateTime.Now);
                    if (btnOrderSubmit.Attributes["Tag"] != null && btnOrderSubmit.Attributes["Tag"].ToString() == "UPDATE")//btnOrderSubmit.Value.ToString() == "UPDATE")
                    {
                        int valid = validateTestDtandCPT();
                        if (valid == 1)
                        {
                            UpdateOrder();
                            if (TriggerClearAll)
                            {
                                ClearAllCmg(true);
                            }
                            else
                            {
                                ClearAll(false);
                                //btnOrderSubmit.Enabled = false;
                                //TriggerClearAll = true;
                            }
                            btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
                            //ClearAll(false);
                            btnClearAll.Value = "Clear All";
                            btnOrderSubmit.Attributes.Add("Tag", "SAVE");//btnOrderSubmit.Value = "SAVE";
                            hdnsaveEnable.Value = "";
                        }
                        else
                        {
                            return;
                        }

                    }
                    else
                    {


                        IList<string> SelectedCPT = new List<string>();
                        for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
                        {
                            if (chklstFrequentlyUsedProcedures.Items[i].Selected)
                                SelectedCPT.Add(chklstFrequentlyUsedProcedures.Items[i].Text);
                        }
                        if (chkMoveToMA.Checked && SelectedCPT.Count == 0)
                        {
                            OrdersSubmit MAOrdersSubmit = new OrdersSubmit();
                            MAOrdersSubmit.Human_ID = HumanID;
                            MAOrdersSubmit.Encounter_ID = EncounterID;
                            MAOrdersSubmit.Physician_ID = PhysicianID;
                            MAOrdersSubmit.Facility_Name = ClientSession.FacilityName;
                            MAOrdersSubmit.Move_To_MA = "Y";
                            MAOrdersSubmit.Order_Type = OrderType.ToUpper();
                            MAOrdersSubmit.Created_By = ClientSession.UserName;
                            //MAOrdersSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                            //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                            //MAOrdersSubmit.Created_Date_And_Time = Convert.ToDateTime(strtime);
                            MAOrdersSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            if (chkAuthRequired.Checked)
                            {
                                MAOrdersSubmit.Authorization_Required = "Y";
                            }
                            else
                            {
                                MAOrdersSubmit.Authorization_Required = "N";
                            }
                            if (chkMoveToMA.Checked == true && MoveToMA.Visible == true)//chkMoveToMA.Visible == true)
                            {
                                MAOrdersSubmit.Move_To_MA = "Y";
                            }
                            else
                            {
                                MAOrdersSubmit.Move_To_MA = "N";
                            }
                            if (chkYesABN.Checked)
                            {
                                MAOrdersSubmit.Is_ABN_Signed = "Y";
                            }
                            else
                            {
                                MAOrdersSubmit.Is_ABN_Signed = "N";
                            }
                            if (chkpaperorder.Checked)
                            {
                                MAOrdersSubmit.Is_Paper_Order = "Y";
                            }
                            else
                            {
                                MAOrdersSubmit.Is_Paper_Order = "N";
                            }

                            if (cboLab.Items[cboLab.SelectedIndex].Selected != null)
                                MAOrdersSubmit.Lab_ID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                            MAOrdersSubmit.Bill_Type = cboBillType.Items[cboBillType.SelectedIndex].Text;
                            MAOrdersSubmit.Height = string.Empty;
                            MAOrdersSubmit.Weight = string.Empty;
                            MAOrdersSubmit.Lab_Location_Name = txtLocation.Value;
                            MAOrdersSubmit.Lab_Name = txtCenterName.Value;
                            if (LookUpPerRequest.ContainsKey("labLocID"))
                                MAOrdersSubmit.Lab_Location_ID = Convert.ToUInt32(LookUpPerRequest["labLocID"]);
                            else
                                MAOrdersSubmit.Lab_Location_ID = Convert.ToUInt32(0);
                            MAOrdersSubmit.Order_Notes = txtOrderNotes.txtDLC.Text;
                            //txtOrderNotes.txtDLC.Text = "";
                            MAOrdersSubmit.Order_Type = OrderType;
                            if (chkSpecimenInHouse.Checked && chkSpecimenInHouse.Visible)
                            {
                                MAOrdersSubmit.Specimen_In_House = "Y";
                            }
                            else
                            {
                                MAOrdersSubmit.Specimen_In_House = "N";
                            }
                            MAOrdersSubmit.Order_Code_Type = MAOrdersSubmit.Lab_Name;
                            if (chkUrgent.Checked == true)
                                MAOrdersSubmit.Is_Urgent = "Y";
                            else
                                MAOrdersSubmit.Is_Urgent = "N";
                            DateTime tempDate = DateTime.MinValue;
                            if (chkTestDateInDate.Checked && DateTime.TryParse(cstdtpTestDate.Value, out tempDate))
                            {
                                MAOrdersSubmit.Test_Date = Convert.ToDateTime(cstdtpTestDate.Value).ToString("dd-MMM-yyyy"); //MAOrdersSubmit.Test_Date = (cstdtpTestDate.SelectedDate ?? new DateTime()).ToString("yyyy-MM-dd");
                                if (chkStat.Checked)
                                {
                                    MAOrdersSubmit.Stat = "Y";
                                }
                                else
                                {
                                    MAOrdersSubmit.Stat = "N";
                                }
                            }
                            else if (chkTestDateInWords.Checked)
                            {
                                if (txtMonths.Value != string.Empty)
                                    MAOrdersSubmit.TestDate_In_Months = Convert.ToInt16(txtMonths.Value);
                                if (txtDays.Value != string.Empty)
                                    MAOrdersSubmit.TestDate_In_Days = Convert.ToInt16(txtDays.Value);
                                if (txtWeeks.Value != string.Empty)
                                    MAOrdersSubmit.TestDate_In_Weeks = Convert.ToInt16(txtWeeks.Value);
                            }
                            if (rbtnSubmitImmediately.Checked)
                                MAOrdersSubmit.Is_Submit_Immediately = "Y";
                            else
                                MAOrdersSubmit.Is_Submit_Immediately = "N";
                            MAOrdersSubmit.Specimen_Type = cbospecimen.Items[cbospecimen.SelectedIndex].Text;
                            MAOrdersSubmit.Specimen_Unit = cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text != string.Empty ? cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text : " ";
                            //CAP-1017
                            if (!string.IsNullOrEmpty(dtpCollectionDate.Value) && Convert.ToDateTime(dtpCollectionDate.Value) != DateTime.Now)
                                //MAOrdersSubmit.Specimen_Collection_Date_And_Time =  Convert.ToDateTime(dtpCollectionDate.SelectedDate.Value);
                                MAOrdersSubmit.Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpCollectionDate.Value));
                            else
                                MAOrdersSubmit.Specimen_Collection_Date_And_Time = DateTime.MinValue;// Convert.ToDateTime("0001-01-01 00:00:00");
                            //if (dtpCollectionDate.SelectedDate != null)
                            //MAOrdersSubmit.Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(dtpCollectionDate.SelectedDate ?? new DateTime());
                            if (txtQuantity.Value != string.Empty)
                                MAOrdersSubmit.Quantity = Convert.ToInt32(txtQuantity.Value);
                            else
                                MAOrdersSubmit.Quantity = 0;
                            if (chkYesFasting.Checked)
                            {
                                MAOrdersSubmit.Fasting = "Y";
                            }
                            else
                            {
                                MAOrdersSubmit.Fasting = "N";
                            }
                            objOrdersManager = new OrdersManager();
                            objOrdersManager.SubmitEmptyOrdersToMA(MAOrdersSubmit, string.Empty);

                        }
                        else
                        {
                            //Added by saravanakumar for avoid Duplicate Procedure

                            // IList<string> ilstLabProcedure = new List<string>();

                            int valid = validateTestDtandCPT();
                            if (valid == 1)
                            {
                                AddOrder();
                            }
                            else
                                return;

                        }
                        btnImportresult.Disabled = true; //btnImportresult.Enabled = false;

                        ClearAll(false);
                    }



                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "var warning_message_label = top.window.document.getElementById('ctl00_tbGeneral').control.findItemByValue('Warning_Message')._element;warning_message_label.textContent = 'Submitted Successfully.';warning_message_label.style.display = 'inline-block'; warning_message_label.style.color = 'red'; warning_message_label.style.position = 'absolute'; warning_message_label.style.top = '40px'; warning_message_label.style.right = '10px'; top.window.setTimeout(function () { warning_message_label.style.display = 'none'; }, 5000); ", true);
                    //if (TriggerClearAll)
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "var win=radalert('Submitted Successfully.',null,null,'Diagnostic Order'); win.setSize(250, 100); win.center(); win.set_iconUrl('Resources/16_16.ico'); $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none'); $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none');  window.setTimeout(function() { win.close();}, 2000);", true);
                    if (YesNoCancel == "Yes")
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "LabOrder_SavedSuccessfully();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "LabOrder_SavedSuccessfully();", true);
                    }
                    //else
                    //if (YesNoCancel == "Yes")
                    //{
                    // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "LabOrder_SavedSuccessfully();", true);
                    //}
                    //else
                    //{
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "LabOrder_SavedSuccessfully();", true);
                    //}
                    btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
                    if (cboLab.Items[cboLab.SelectedIndex].Text == "Quest Diagnostics" && chkSpecimenInHouse.Checked == false)
                    {
                        hdnPrintLabel.Value = "true";
                    }
                    else
                    {
                        hdnPrintLabel.Value = "";
                    }
                    // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230135');", true);
                    // ApplicationObject.erroHandler.DisplayErrorMessage("230135", OrderType.ToUpper(),this);
                    //objfrmOrdersList.frmOrdersList_Load(new object(), new EventArgs());
                    //MyLog.LogDebugMessage(logger, "frmImageAndLabOrders - frmPlanCptFormsRequired - btnOrderSubmit", stopwatch);

                    //}
                    hdnMovetoNextProcess.Value = "true";
                }
                else
                {
                    IsImport = true;
                    //divLoading.Style.Add("display", "none");

                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Import", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                    return;
                }
                //divLoading.Style.Add("display", "none");
                if (hdnOrderSubmitClick.Value == "Yes" && YesNoCancel != "Yes")
                {

                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230150');SavedSuccessfully();top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230150'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "HiddenValue", "LabOrder_SavedSuccessfully();top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230150');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);

                    hdnOrderSubmitClick.Value = string.Empty;
                }
                else
                {

                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "HiddenValue", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                }
            }
            CancelTextChanged();
            if (Request["ScreenMode"] == "MyQ" && ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
            {
                //CAP-667 - disable gbSpecimenDetails, gbBillingDetails and gbOrderDetails all child.
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "$('#gbSpecimenDetails').find('*').attr('disabled', true);$('#gbBillingDetails').find('*').attr('disabled', true);$('#gbOrderDetails').find('*').attr('disabled', true);", true);
                disableallcontrols(false);
                //btnMoveToNextProcess.Enabled = false;
                btnPlan.Disabled = true; //btnPlan.Enabled = false;
            }
            if (sAppointment == "Y")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", " var Own = GetRadWindow(); var result = new Object(); result.Orders = document.getElementById('hdnSelectedOrder').value; Own.returnValue = result;Own.close(result);", true);

                // ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "var result = new Object(); result.Orders = document.getElementById('hdnSelectedOrder').value; returnToParent(result);", true);



                //var Result=new Object(); Result.selectedLabText=document.getElementById('hdnSelectedLabText').value;sessionStorage.setItem('AllDiag_SelectedLabText', document.getElementById('hdnSelectedLabText').value);returnToParent(Result); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}

                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "var win=radalert('Submitted Successfully.',null,null,'Diagnostic Order'); win.setSize(250, 100); win.center(); win.set_iconUrl('Resources/16_16.ico'); $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none'); $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none'); window.setTimeout(function() { win.close();}, 2000);", true);
                //return;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ManLabel", "warningmethod();", true);
            //if(sCmgorder=="Y")
            //{
            //    if(Session["OrderSubmitId"]!=null)
            //    {
            //        ulong uid = Convert.ToUInt32(Session["OrderSubmitId"]);
            //        DiagnosticDTO objDiagnosticDTO = new DiagnosticDTO();
            //        objDiagnosticDTO = objOrdersManager.FillDiagnosticDTO(uid, EncounterID, HumanID, PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), ClientSession.FacilityName);
            //        LoadScreenFromOrderList(objDiagnosticDTO);
            //        FillAssesmentAndProblemListICD(objDiagnosticDTO.AssessmentList, objDiagnosticDTO.MedAdvProblemList);
            //        IList<string> ProceduresViewList = new List<string>();
            //        //ProceduresViewList = objDiagnosticDTO.OrdersLists.Select(a => a.Lab_Procedure + "-" + (a.Lab_Procedure_Description.Contains("x_") ? ((a.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None).Count() > 1 ? (a.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None)[0].ToString() + "x______") : a.Lab_Procedure_Description) + "~" + a.Quantity + "|" + a.Id) : a.Lab_Procedure_Description)).ToList<string>();
            //        ProceduresViewList = objDiagnosticDTO.OrdersLists.Select(a => a.Lab_Procedure + "-" + (a.Lab_Procedure_Description.Contains("x_") ? (a.Lab_Procedure_Description + "~" + a.Quantity + "|" + a.Id) : a.Lab_Procedure_Description)).ToList<string>();
            //        //Session["ProcEditID"] = objDiagnosticDTO.OrdersLists[0].Order_Submit_ID;
            //        IList<ListItem> tempList = SetOrderIDForEditQuantity(ProceduresViewList);

            //    }

            //}
            hdnMovetoOrderSubmitId.Value = Session["OrderSubmitId"].ToString();
            //Jira CAP-1628
            Session["OrderSubmitId"] = string.Empty;
        }

        public int validateTestDtandCPT()
        {
            int valid = 1;
            IList<string> SelectedCPT = new List<string>();
            for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
            {
                if (chklstFrequentlyUsedProcedures.Items[i].Selected)
                    SelectedCPT.Add(chklstFrequentlyUsedProcedures.Items[i].Text);
            }

            IDictionary<int, string> ilstLabProcedure = new Dictionary<int, string>();
            string uSelectCpts = string.Empty;
            if (SelectedCPT.Count > 0)
            {
                for (int i = 0; i < SelectedCPT.Count; i++)
                {
                    string[] arr = SelectedCPT[i].Split('-');
                    if (arr.Count() >= 2)
                    {
                        if (uSelectCpts == string.Empty)
                        {
                            uSelectCpts = arr[0];
                        }
                        else
                        {
                            uSelectCpts += "," + arr[0];
                        }

                    }
                }
            }
            if (sScreenMode != "Menu")
            {
                DateTime tempDate = DateTime.MinValue;
                if (ClientSession.EncounterId != 0 && DateTime.TryParse(cstdtpTestDate.Value, out tempDate))
                {
                    if (txtDays.Value.Trim() == "")
                        txtDays.Value = "0";
                    if (txtWeeks.Value.Trim() == "")
                        txtWeeks.Value = "0";
                    if (txtMonths.Value.Trim() == "")
                        txtMonths.Value = "0";

                    String DaysWeeksMonths = Convert.ToInt16(txtDays.Value) + "-" + Convert.ToInt16(txtWeeks.Value) + "-" + Convert.ToInt16(txtMonths.Value);
                    ilstLabProcedure = objOrdersManager.GetOrder(uSelectCpts, Convert.ToDateTime(cstdtpTestDate.Value), DaysWeeksMonths, ClientSession.EncounterId, Session["ProcEditID"] == null ? "" : Session["ProcEditID"].ToString());
                    Session["ProcEditID"] = string.Empty;
                }
            }
            if (ilstLabProcedure.Count > 0)
            {
                string sProcedure = string.Empty;
                for (int i = 0; i < ilstLabProcedure.Count; i++)
                {
                    if (sProcedure == string.Empty)
                    {
                        sProcedure = ilstLabProcedure[i];
                    }
                    else
                    {
                        sProcedure += ", " + ilstLabProcedure[i];
                    }
                }

                hdnImportResult.Value = "true";
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "Order_SaveUnsuccessful();DisplayErrorMessage('230149','','" + sProcedure + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}Order_SaveUnsuccessful();DisplayErrorMessage('230149','','" + sProcedure + "');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                valid = 0;
                //return ;
            }
            return valid;
        }
        void AddOrder()
        {
            IList<string> SelectedProcedure = new List<string>();
            IList<string> SelectedAssessment = new List<string>();
            IList<Orders> SaveOrderList = new List<Orders>();
            IList<OrdersSubmit> SaveOrdersSubmitList = new List<OrdersSubmit>();
            Orders objOrder = new Orders();
            IList<string> orderingProcedure = new List<string>();
            OrderCodeLibrary objSelectedOrderCode = null;
            IList<OrdersAssessment> SaveOrdersAssList = new List<OrdersAssessment>();
            OrderCodeLibraryManager objOrderCodeLibraryManager = new OrderCodeLibraryManager();
            OrdersSubmit objOrderSubmit = new OrdersSubmit();

            string sLocalTime = string.Empty;

            //IList<StaticLookup> ilstTemp = new List<StaticLookup>();
            //ilstTemp = objStaticLookupManager.getStaticLookupByFieldName("QUEST TEMPRATURE STATE");

            List<KeyValuePair<string, string>> ilstTemp = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Ambient", "A"),
                new KeyValuePair<string, string>("Frozen", "FZ"),
                new KeyValuePair<string, string>("Groupable", "G"),
                new KeyValuePair<string, string>("Handwritten", "H"),
                new KeyValuePair<string, string>("Multiple", "M"),
                new KeyValuePair<string, string>("Room", "R"),
                new KeyValuePair<string, string>("Refrigerated", "RF"),
                new KeyValuePair<string, string>("Room Temperature", "RT"),
                new KeyValuePair<string, string>("Split Requisition", "S"),
                new KeyValuePair<string, string>("Discontinued Test", "T"),
            };



            for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
            {
                if (chklstFrequentlyUsedProcedures.Items[i].Selected && chklstFrequentlyUsedProcedures.Items[i].Value != null && chklstFrequentlyUsedProcedures.Items[i].Value != "HEADERROW")
                    SelectedProcedure.Add(chklstFrequentlyUsedProcedures.Items[i].Text);
            }
            for (int i = 0; i < chklstAssessment.Items.Count; i++)
            {
                if (chklstAssessment.Items[i].Selected)
                    SelectedAssessment.Add(chklstAssessment.Items[i].Text.Split('|')[chklstAssessment.Items[i].Text.Split('|').Count() - 1]);
                //SelectedAssessment.Add(chklstAssessment.Items[i].Text);
            }
            foreach (string str in SelectedProcedure)
            {
                objOrder = new Orders();
                objOrder.Encounter_ID = EncounterID;
                //if (sCmgorder == "Y" && cboReadingProvider.Items.Count > 0)
                //{
                //    objOrder.Physician_ID = Convert.ToUInt32(cboReadingProvider.Items[cboReadingProvider.SelectedIndex].Value);
                //}
                //else
                //{
                objOrder.Physician_ID = PhysicianID;
                //}

                objOrder.Human_ID = HumanID;
                objOrder.Created_By = ClientSession.UserName;
                // objOrder.Created_Date_And_Time = timeOnAddClick;
                //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                //objOrder.Created_Date_And_Time = Convert.ToDateTime(strtime);
                objOrder.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                sLocalTime = UtilityManager.ConvertToUniversal(objOrder.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                //if (DefaultLabName != string.Empty)
                //    objOrder.CMG_Encounter_ID = CMGEncounterIDPassed;
                string itemText = str;
                string[] splitText = itemText.Split('-');
                string procedure = string.Empty;
                if (splitText.Length > 0)
                {
                    objOrder.Lab_Procedure = splitText[0];
                    for (int i = 1; i < splitText.Length; i++)
                    {
                        if (i == 1)
                            procedure = splitText[i];
                        else
                            procedure += "-" + splitText[i];
                    }

                    objOrder.Lab_Procedure_Description = procedure;

                    orderingProcedure.Add(procedure);
                    if (Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value) == 1 || Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value) == 2)
                    {
                        objSelectedOrderCode = objOrderCodeLibraryManager.GetOrderCodeDetailsForSelectedOrderCode(splitText[0]);
                    }
                    if (objSelectedOrderCode != null)
                    {
                        if (objSelectedOrderCode.Order_Code != string.Empty && objSelectedOrderCode.Order_Code_Type != string.Empty && objSelectedOrderCode.Temperature_State == string.Empty)
                        {
                            objOrder.Orders_Question_Set_Segment = objSelectedOrderCode.Order_Code_Question_Set_Segment;
                            objOrder.Order_Code_Type = objSelectedOrderCode.Order_Code_Type;
                        }
                        else if (objSelectedOrderCode.Order_Code != string.Empty && objSelectedOrderCode.Temperature_State != string.Empty)
                        {
                            objOrder.Order_Code_Type = objSelectedOrderCode.Temperature_State;

                        }
                        else
                        {
                            objOrder.Order_Code_Type = cboLab.Items[cboLab.SelectedIndex].Text.ToUpper();
                        }
                    }
                    else
                    {
                        objOrder.Order_Code_Type = cboLab.Items[cboLab.SelectedIndex].Text.ToUpper();
                    }

                }
                SaveOrderList.Add(objOrder);
                //OrdersAssessment objOrdAss;
                //for (int i = 0; i < SelectedAssessment.Count; i++)
                //{
                //    if (AssessmentSource.Count > 0)
                //    {
                //        if (AssessmentSource.ContainsKey(SelectedAssessment[i].ToString().Split('-')[0]))
                //        {
                //            objOrdAss = CreateOrderAssObj(SelectedAssessment[i], 0, AssessmentSource[SelectedAssessment[i].ToString().Split('-')[0]]);
                //            objOrdAss.Order_ID = (ulong)(SaveOrderList.Count - 1);
                //            SaveOrdersAssList.Add(objOrdAss);
                //        }
                //    }
                //}
            }

            var DistinctSubmit = (from rec in SaveOrderList select rec.Order_Code_Type);
            //if (!cboLab.Items[cboLab.SelectedIndex].Text.StartsWith(LookUpPerRequest["CMGLabNameFromLookUp"]))
            //{
            //    DistinctSubmit = DistinctSubmit.Distinct();
            //} //Commentted for CMG Ancillary
            DistinctSubmit = DistinctSubmit.Distinct();
            foreach (string str in DistinctSubmit)
            {
                objOrderSubmit = new OrdersSubmit();
                objOrderSubmit.Order_Code_Type = str;

                if (chkAuthRequired.Checked)
                {
                    objOrderSubmit.Authorization_Required = "Y";
                }
                else
                {
                    objOrderSubmit.Authorization_Required = "N";
                }
                objOrderSubmit.Bill_Type = cboBillType.Items[cboBillType.SelectedIndex].Text;
                objOrderSubmit.Created_By = ClientSession.UserName;
                //objOrderSubmit.Created_Date_And_Time = timeOnAddClick;
                //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                //objOrderSubmit.Created_Date_And_Time = Convert.ToDateTime(strtime);
                objOrderSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objOrderSubmit.Encounter_ID = EncounterID;
                objOrderSubmit.Human_ID = HumanID;
                //if (sCmgorder == "Y" && cboReadingProvider.Items.Count > 0)
                //{
                //    objOrderSubmit.Physician_ID = Convert.ToUInt32(cboReadingProvider.Items[cboReadingProvider.SelectedIndex].Value);
                //}
                //else
                //{
                objOrderSubmit.Physician_ID = PhysicianID;
                //}

                //objOrderSubmit.Facility_Name = ClientSession.FacilityName;
                //GitLab # 2364 - Enabled Menu level Order for Ancillary FO 
                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();

                if (Request["ScreenMode"] != null && Request["ScreenMode"].ToString().ToUpper() == "MENU" && ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                {
                    string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
                    if (File.Exists(strXmlFilePath1) == true)
                    {
                        XmlDocument xmldocUser = new XmlDocument();
                        xmldocUser.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "User" + ".xml");

                        XmlNodeList xmlUserList = xmldocUser.GetElementsByTagName("User");


                        if (xmlUserList.Count > 0)
                        {
                            foreach (XmlNode item in xmlUserList)
                            {
                                if (PhysicianID.ToString() == item.Attributes["Physician_Library_ID"].Value)
                                {
                                    objOrderSubmit.Facility_Name = item.Attributes["Default_Facility"].Value;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    objOrderSubmit.Facility_Name = ClientSession.FacilityName;
                }


                objOrderSubmit.Height = string.Empty;
                objOrderSubmit.Weight = string.Empty;
                objOrderSubmit.Temperature = ilstTemp.Where(a => a.Value == str).Select(a => a.Key).SingleOrDefault();
                if (objOrderSubmit.Temperature == null)
                    objOrderSubmit.Temperature = string.Empty;
                //sarava
                //ulong PreferredReadingPhysician = 0;
                if (ilstRequiredForms.Count > 0)
                {
                    objOrderSubmit.Prefered_Reading_Provider_ID = ilstRequiredForms[0].Ordering_Provider_Id;
                }
                //objOrderSubmit.Preferred_Reading_Physician_ID = PreferredReadingPhysician;
                if (chkMoveToMA.Checked == true)     //if (chkMoveToMA.Checked == true && chkMoveToMA.Visible == true)
                {
                    objOrderSubmit.Move_To_MA = "Y";
                }
                else
                {
                    objOrderSubmit.Move_To_MA = "N";
                }
                if (chkYesABN.Checked)
                {
                    objOrderSubmit.Is_ABN_Signed = "Y";
                }
                else
                {
                    objOrderSubmit.Is_ABN_Signed = "N";

                }

                if (chkpaperorder.Checked)
                {
                    objOrderSubmit.Is_Paper_Order = "Y";
                }
                else
                {
                    objOrderSubmit.Is_Paper_Order = "N";
                }
                if (cboLab.Items[cboLab.SelectedIndex].Selected != null)
                    objOrderSubmit.Lab_ID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                objOrderSubmit.Lab_Location_Name = txtLocation.Value;
                objOrderSubmit.Lab_Name = txtCenterName.Value;
                if (LookUpPerRequest.ContainsKey("labLocID"))
                    objOrderSubmit.Lab_Location_ID = Convert.ToUInt64(LookUpPerRequest["labLocID"]);
                else
                    objOrderSubmit.Lab_Location_ID = Convert.ToUInt64(0);
                objOrderSubmit.Order_Notes = txtOrderNotes.txtDLC.Text;
                //txtOrderNotes.txtDLC.Text = "";
                objOrderSubmit.Order_Type = OrderType;
                //objOrderSubmit.Update_Phone_Number = ModifiedPhoneNumber;

                if (chkSpecimenInHouse.Checked && chkSpecimenInHouse.Visible)
                {
                    objOrderSubmit.Specimen_In_House = "Y";
                }
                else
                {
                    objOrderSubmit.Specimen_In_House = "N";
                }
                DateTime tempDate = DateTime.MinValue;
                if (chkUrgent.Checked == true)
                    objOrderSubmit.Is_Urgent = "Y";
                else
                    objOrderSubmit.Is_Urgent = "N";
                if (chkTestDateInDate.Checked && DateTime.TryParse(cstdtpTestDate.Value, out tempDate))
                {
                    objOrderSubmit.Test_Date = Convert.ToDateTime(cstdtpTestDate.Value).ToString("yyyy-MM-dd");//objOrderSubmit.Test_Date = (cstdtpTestDate.SelectedDate ?? new DateTime()).ToString("yyyy-MM-dd");
                    if (chkStat.Checked)
                    {
                        objOrderSubmit.Stat = "Y";
                    }
                    else
                    {
                        objOrderSubmit.Stat = "N";
                    }
                }
                else if (chkTestDateInWords.Checked)
                {
                    if (txtMonths.Value != string.Empty)
                        objOrderSubmit.TestDate_In_Months = Convert.ToInt16(txtMonths.Value);
                    if (txtDays.Value != string.Empty)
                        objOrderSubmit.TestDate_In_Days = Convert.ToInt16(txtDays.Value);
                    if (txtWeeks.Value != string.Empty)
                        objOrderSubmit.TestDate_In_Weeks = Convert.ToInt16(txtWeeks.Value);
                }
                if (rbtnSubmitImmediately.Checked == true)
                    objOrderSubmit.Is_Submit_Immediately = "Y";
                else
                    objOrderSubmit.Is_Submit_Immediately = "N";
                objOrderSubmit.Specimen_Type = cbospecimen.Items[cbospecimen.SelectedIndex].Text;
                objOrderSubmit.Specimen_Unit = cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text != string.Empty ? cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text : " ";
                if (dtpCollectionDate.Value != null && dtpCollectionDate.Value != string.Empty && Convert.ToDateTime(dtpCollectionDate.Value) != DateTime.MinValue)
                    // objOrderSubmit.Specimen_Collection_Date_And_Time = dtpCollectionDate.SelectedDate.Value;
                    objOrderSubmit.Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpCollectionDate.Value));

                else
                //objOrderSubmit.Specimen_Collection_Date_And_Time = DateTime.Now;
                {
                    //string utc = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                    //objOrderSubmit.Specimen_Collection_Date_And_Time = Convert.ToDateTime(utc);
                    objOrderSubmit.Specimen_Collection_Date_And_Time = DateTime.MinValue;
                }
                //if (dtpCollectionDate.SelectedDate != null)
                //    objOrderSubmit.Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(dtpCollectionDate.SelectedDate ?? new DateTime());
                if (txtQuantity.Value != string.Empty)
                    objOrderSubmit.Quantity = Convert.ToInt32(txtQuantity.Value);
                else
                    objOrderSubmit.Quantity = 0;
                if (chkYesFasting.Checked)
                {
                    objOrderSubmit.Fasting = "Y";
                }
                else
                {
                    objOrderSubmit.Fasting = "N";
                }
                SaveOrdersSubmitList.Add(objOrderSubmit);
                //Add Outside of the for loop for Bug : 56642. Changed into order id to order submit id in ordersassessment table
                OrdersAssessment objOrdAss;
                for (int i = 0; i < SelectedAssessment.Count; i++)
                {
                    if (AssessmentSource.Count > 0)
                    {
                        if (AssessmentSource.ContainsKey(SelectedAssessment[i].ToString().Split('-')[0]))
                        {
                            objOrdAss = CreateOrderAssObj(SelectedAssessment[i], 0, AssessmentSource[SelectedAssessment[i].ToString().Split('-')[0]]);
                            objOrdAss.Order_Submit_ID = (ulong)(SaveOrdersSubmitList.Count - 1);
                            SaveOrdersAssList.Add(objOrdAss);
                        }
                    }
                }
                //End Outside of the for loop for Bug : 56642
            }
            var serializer = new NetDataContractSerializer();
            objOrdersManager = new OrdersManager();
            string sSelectedOrder = string.Empty;
            objOrdersManager.InsertToOrders(SaveOrderList.ToArray<Orders>(), SaveOrdersSubmitList, SaveOrdersAssList.ToArray<OrdersAssessment>(), orderingProcedure.ToArray<string>(), EncounterID, OrderType, string.Empty, ilstRequiredForms, ref sSelectedOrder, sLocalTime);
            if (sSelectedOrder != string.Empty)
            {
                if (sAppointment == "Y")
                {
                    hdnSelectedOrder.Value = sSelectedOrder;
                }

            }


        }
        ulong EditOrdersSubmitID = 0;
        void UpdateOrder()
        {
            IList<string> SelectedAssessment = new List<string>();
            IList<string> SelectedProcedure = new List<string>();
            // IList<string> OrderingProcedure = new List<string>();
            Orders objNewOrder = new Orders();
            IList<Orders> SaveOrderList = new List<Orders>();
            IList<Orders> UpdateOrdersList = new List<Orders>();
            IList<Orders> DeleteOrdersList = new List<Orders>();
            IList<Orders> OriginalOrdersList = new List<Orders>();
            IList<string> OriginalProcedures = new List<string>();
            IList<OrdersAssessment> OriginalOrdersAssessment = new List<OrdersAssessment>();
            OrdersSubmit OriginalOrdersSubmit = new OrdersSubmit();
            OrdersSubmitManager objOrdersSubmitManager = new OrdersSubmitManager();
            OrdersAssessmentManager objOrdersAssessmentManager = new OrdersAssessmentManager();
            EditOrdersSubmitID = hdnOrderSubmitID.Value != null ? Convert.ToUInt32(hdnOrderSubmitID.Value) : 0;
            DiagnosticDTO objDiagnosticDTO = (DiagnosticDTO)Session["objDiagnosticDTO"];
            if (objDiagnosticDTO != null)
            {
                OriginalOrdersSubmit = objDiagnosticDTO.objOrdersSubmit;
                OriginalOrdersList = objDiagnosticDTO.OrdersLists.Where(a => a.Order_Submit_ID == EditOrdersSubmitID).ToList<Orders>();
                IList<ulong> OrderID = objDiagnosticDTO.OrdersLists.Where(a => a.Order_Submit_ID == EditOrdersSubmitID).Select(a => a.Id).Distinct().ToList<ulong>();
                //OriginalOrdersAssessment = objDiagnosticDTO.OrderAssList.Where(a => OrderID.Contains(a.Order_ID)).ToList<OrdersAssessment>();
                OriginalOrdersAssessment = objDiagnosticDTO.OrderAssList.Where(a => a.Order_Submit_ID == EditOrdersSubmitID).ToList<OrdersAssessment>();
            }
            //OriginalOrdersSubmit = objOrdersSubmitManager.GetById(EditOrdersSubmitID);
            //OriginalOrdersAssessment = objOrdersAssessmentManager.GetOrderAssessmentByOrderSubmitID(EditOrdersSubmitID);
            //OriginalOrdersList = objOrdersAssessmentManager.GetOrders(EditOrdersSubmitID);
            OrderCodeLibraryManager objOrderCodeLibraryManager = new OrderCodeLibraryManager();
            OrderCodeLibrary objSelectedOrderCode = new OrderCodeLibrary();
            IList<string> OrderCodeType = new List<string>();
            OriginalProcedures = (IList<string>)Session["ProceduresViewList"];
            string sLocalTime = string.Empty;
            for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
            {
                if (chklstFrequentlyUsedProcedures.Items[i].Selected && chklstFrequentlyUsedProcedures.Items[i].Value != null && chklstFrequentlyUsedProcedures.Items[i].Value != "HEADERROW")
                    SelectedProcedure.Add(chklstFrequentlyUsedProcedures.Items[i].Text);//No Need Selected Procedure
            }
            for (int i = 0; i < chklstAssessment.Items.Count; i++)
            {
                if (chklstAssessment.Items[i].Selected)
                    SelectedAssessment.Add(chklstAssessment.Items[i].Text.Split('|')[chklstAssessment.Items[i].Text.Split('|').Count() - 1]);
                // SelectedAssessment.Add(chklstAssessment.Items[i].Text);
            }
            for (int k = 0; k < SelectedProcedure.Count; k++)
            {
                string itemText = SelectedProcedure[k];
                //OrderingProcedure.Add(itemText);// NO Need Ordering Procedure.
                string[] splitText = itemText.Split('-');
                if (OriginalProcedures.Contains(itemText))
                {
                    objNewOrder = new Orders();
                    objNewOrder = OriginalOrdersList.Where(a => a.Lab_Procedure == splitText[0]).ToList<Orders>()[0];
                    //if (sCmgorder == "Y" && cboReadingProvider.Items.Count > 0)
                    //{
                    //    objNewOrder.Physician_ID = Convert.ToUInt32(cboReadingProvider.Items[cboReadingProvider.SelectedIndex].Value);
                    //}

                    objNewOrder.Modified_By = ClientSession.UserName;
                    // objNewOrder.Modified_Date_And_Time = timeOnAddClick;
                    //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                    //objNewOrder.Modified_Date_And_Time = Convert.ToDateTime(strtime);
                    objNewOrder.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    sLocalTime = UtilityManager.ConvertToUniversal(objNewOrder.Modified_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                }
                else
                {
                    objNewOrder = new Orders();
                    objNewOrder.Created_By = ClientSession.UserName;
                    //objNewOrder.Created_Date_And_Time = timeOnAddClick;
                    //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                    //objNewOrder.Created_Date_And_Time = Convert.ToDateTime(strtime);
                    objNewOrder.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    sLocalTime = UtilityManager.ConvertToUniversal(objNewOrder.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                    objNewOrder.Encounter_ID = EncounterID;
                    //if (sCmgorder == "Y" && cboReadingProvider.Items.Count > 0)
                    //{
                    //    objNewOrder.Physician_ID = Convert.ToUInt32(cboReadingProvider.Items[cboReadingProvider.SelectedIndex].Value);
                    //}
                    //else
                    //{
                    objNewOrder.Physician_ID = PhysicianID;
                    //}

                    objNewOrder.Human_ID = HumanID;
                }
                //Assign Procedure Details
                string procedure = string.Empty;
                if (splitText.Length > 0)
                {
                    objNewOrder.Lab_Procedure = splitText[0];
                    for (int i = 1; i < splitText.Length; i++)// Use Substring to remove this COde
                    {
                        if (i == 1)
                            procedure = splitText[i];
                        else
                            procedure += "-" + splitText[i];
                    }
                    objNewOrder.Lab_Procedure_Description = procedure;
                }

                //Assign Order Code Type
                objSelectedOrderCode = objOrderCodeLibraryManager.GetOrderCodeDetailsForSelectedOrderCode(splitText[0]);
                if (objSelectedOrderCode != null)
                {
                    objNewOrder.Orders_Question_Set_Segment = objSelectedOrderCode.Order_Code_Question_Set_Segment;
                    if (objSelectedOrderCode.Order_Code != string.Empty && (objSelectedOrderCode.Order_Code_Type != string.Empty || objSelectedOrderCode.Temperature_State != string.Empty))
                    {
                        if (objSelectedOrderCode.Order_Code_Type != string.Empty)
                            objNewOrder.Order_Code_Type = objSelectedOrderCode.Order_Code_Type;
                        else if (objSelectedOrderCode.Temperature_State != string.Empty)
                            objNewOrder.Order_Code_Type = objSelectedOrderCode.Temperature_State;
                    }
                    else
                    {
                        //if ((LookUpPerRequest.Keys.Contains("CMGLabNameFromLookUp") == true) && (cboLab.Items[cboLab.SelectedIndex].Text.StartsWith(LookUpPerRequest["CMGLabNameFromLookUp"].ToString())))
                        if ((LookUpPerRequest.Keys.Contains("CMGLabNameFromLookUp") == true) && (LookUpPerRequest["CMGLabNameFromLookUp"].ToString().Contains(cboLab.Items[cboLab.SelectedIndex].Text) == true))
                        {
                            if ((OriginalProcedures.Count > 0 || SaveOrderList.Count > 0 || UpdateOrdersList.Count > 0 || DeleteOrdersList.Count > 0) && !OriginalProcedures.Contains(itemText))
                            {
                                //CMGIncrementer = CMGIncrementer + 1;
                                objNewOrder.Order_Code_Type = cboLab.Items[cboLab.SelectedIndex].Text.ToUpper();// +CMGIncrementer.ToString();
                            }
                            else
                            {
                                objNewOrder.Order_Code_Type = cboLab.Items[cboLab.SelectedIndex].Text.ToUpper();
                            }
                        }
                        else
                        {
                            objNewOrder.Order_Code_Type = cboLab.Items[cboLab.SelectedIndex].Text.ToUpper();
                        }
                    }
                }


                //if (DefaultLabName != string.Empty)
                //    objNewOrder.CMG_Encounter_ID = CMGEncounterIDPassed;
                OrderCodeType.Add(objNewOrder.Order_Code_Type);
                if (OriginalProcedures.Contains(itemText))
                    UpdateOrdersList.Add(objNewOrder);
                else
                    SaveOrderList.Add(objNewOrder);
            }

            DeleteOrdersList = OriginalOrdersList.Except(UpdateOrdersList).ToList();

            //foreach (string s in OriginalProcedures.Except(OrderingProcedure))
            //{
            //    DeleteOrdersList.Add(UpdateOrdersList.Where(a => a.Lab_Procedure == s.Split('-')[0]).ToList<Orders>()[0]);
            //}
            #region UpdatingOrderSubmit
            bool IsUpdate = false;
            IList<OrdersSubmit> SavListOrderSubmit = new List<OrdersSubmit>();
            IList<OrdersSubmit> UpdListOrderSubmit = new List<OrdersSubmit>();
            IList<OrdersSubmit> DelListOrderSubmit = new List<OrdersSubmit>();
            OrdersSubmit UpdateOrderSubmit = new OrdersSubmit();
            UpdateOrderSubmit = OriginalOrdersSubmit;
            if (OrderCodeType.Distinct().Contains(UpdateOrderSubmit.Order_Code_Type.ToUpper()))
            {
                UpdListOrderSubmit.Add(UpdateOrderSubmit);
            }
            else
            {
                if (ClientSession.UserCurrentProcess == "MA_REVIEW")
                {
                    if (objSelectedOrderCode.Lab_ID == UpdateOrderSubmit.Lab_ID)
                    {
                        UpdListOrderSubmit.Add(UpdateOrderSubmit);
                    }
                    else
                    {
                        DelListOrderSubmit.Add(UpdateOrderSubmit);
                        UpdateOrdersList = UpdateOrdersList.Except(UpdateOrdersList.Where(a => a.Order_Submit_ID == UpdateOrderSubmit.Id).ToList<Orders>()).ToList<Orders>();
                    }
                }
                else
                {
                    DelListOrderSubmit.Add(UpdateOrderSubmit);
                    UpdateOrdersList = UpdateOrdersList.Except(UpdateOrdersList.Where(a => a.Order_Submit_ID == UpdateOrderSubmit.Id).ToList<Orders>()).ToList<Orders>();
                }

            }
            foreach (string rec in OrderCodeType.Distinct())
            {
                if (UpdListOrderSubmit != null && UpdListOrderSubmit.Count > 0 && UpdListOrderSubmit[0].Order_Code_Type.ToUpper() == rec.ToUpper())
                {
                    UpdateOrderSubmit = new OrdersSubmit();
                    UpdateOrderSubmit = UpdListOrderSubmit[0];
                    UpdateOrderSubmit.Modified_By = ClientSession.UserName;
                    // UpdateOrderSubmit.Modified_Date_And_Time = timeOnAddClick;
                    //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                    //UpdateOrderSubmit.Modified_Date_And_Time = Convert.ToDateTime(strtime);
                    UpdateOrderSubmit.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                    IsUpdate = true;
                }
                else
                {
                    if (ClientSession.UserCurrentProcess == "MA_REVIEW")
                    {
                        if (UpdListOrderSubmit != null && UpdListOrderSubmit.Count > 0 && objSelectedOrderCode.Lab_ID == UpdateOrderSubmit.Lab_ID)
                        {
                            UpdateOrderSubmit = new OrdersSubmit();
                            UpdateOrderSubmit = UpdListOrderSubmit[0];
                            UpdateOrderSubmit.Modified_By = ClientSession.UserName;
                            // UpdateOrderSubmit.Modified_Date_And_Time = timeOnAddClick;
                            //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                            //UpdateOrderSubmit.Modified_Date_And_Time = Convert.ToDateTime(strtime);
                            UpdateOrderSubmit.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            IsUpdate = true;
                        }
                        else
                        {

                            IsUpdate = false;
                            UpdateOrderSubmit = new OrdersSubmit();
                            UpdateOrderSubmit.Created_By = ClientSession.UserName;
                            //UpdateOrderSubmit.Created_Date_And_Time = timeOnAddClick;
                            //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                            //UpdateOrderSubmit.Created_Date_And_Time = Convert.ToDateTime(strtime);
                            UpdateOrderSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        }
                    }
                    else
                    {

                        IsUpdate = false;
                        UpdateOrderSubmit = new OrdersSubmit();
                        UpdateOrderSubmit.Created_By = ClientSession.UserName;
                        //UpdateOrderSubmit.Created_Date_And_Time = timeOnAddClick;
                        //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                        //UpdateOrderSubmit.Created_Date_And_Time = Convert.ToDateTime(strtime);
                        UpdateOrderSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }

                }
                if (chkAuthRequired.Checked)
                {
                    UpdateOrderSubmit.Authorization_Required = "Y";
                }
                else
                {
                    UpdateOrderSubmit.Authorization_Required = "N";
                }
                UpdateOrderSubmit.Order_Code_Type = rec;
                UpdateOrderSubmit.Bill_Type = cboBillType.Items[cboBillType.SelectedIndex].Text;
                UpdateOrderSubmit.Facility_Name = ClientSession.FacilityName;
                //UpdateOrderSubmit.Update_Phone_Number = ModifiedPhoneNumber;
                if (chkMoveToMA.Checked == true)//Removed the condition to check the visibility of checkbox for Bug Id: 29155
                {
                    UpdateOrderSubmit.Move_To_MA = "Y";
                }
                else
                {
                    UpdateOrderSubmit.Move_To_MA = "N";
                }
                if (chkYesABN.Checked)
                {
                    UpdateOrderSubmit.Is_ABN_Signed = "Y";
                }
                else
                {
                    UpdateOrderSubmit.Is_ABN_Signed = "N";
                }

                if (chkpaperorder.Checked)
                {
                    UpdateOrderSubmit.Is_Paper_Order = "Y";
                }
                else
                {
                    UpdateOrderSubmit.Is_Paper_Order = "N";
                }
                if (cboLab.Items[cboLab.SelectedIndex].Selected != null)
                    UpdateOrderSubmit.Lab_ID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                UpdateOrderSubmit.Lab_Location_Name = txtLocation.Value;
                UpdateOrderSubmit.Lab_Name = txtCenterName.Value;
                if (LookUpPerRequest.ContainsKey("labLocID"))
                    UpdateOrderSubmit.Lab_Location_ID = Convert.ToUInt32(LookUpPerRequest["labLocID"]);
                else
                    UpdateOrderSubmit.Lab_Location_ID = Convert.ToUInt32(0);
                UpdateOrderSubmit.Order_Notes = txtOrderNotes.txtDLC.Text;
                //txtOrderNotes.txtDLC.Text = "";
                UpdateOrderSubmit.Order_Type = OrderType;
                UpdateOrderSubmit.Human_ID = HumanID;
                //if (sCmgorder == "Y" && cboReadingProvider.Items.Count > 0)
                //{
                //    UpdateOrderSubmit.Physician_ID = Convert.ToUInt32(cboReadingProvider.Items[cboReadingProvider.SelectedIndex].Value);
                //}
                //else
                //{
                UpdateOrderSubmit.Physician_ID = PhysicianID;
                //}

                UpdateOrderSubmit.Encounter_ID = EncounterID;
                //sarava
                // ulong PreferredReadingPhysician = 0;
                if (ilstRequiredForms.Count > 0)
                {
                    UpdateOrderSubmit.Prefered_Reading_Provider_ID = ilstRequiredForms[0].Ordering_Provider_Id;
                }
                //UpdateOrderSubmit.Preferred_Reading_Physician_ID = PreferredReadingPhysician;
                if (chkSpecimenInHouse.Checked && chkSpecimenInHouse.Visible)
                {
                    UpdateOrderSubmit.Specimen_In_House = "Y";
                }
                else
                {
                    UpdateOrderSubmit.Specimen_In_House = "N";
                }
                if (chkUrgent.Checked == true)
                    UpdateOrderSubmit.Is_Urgent = "Y";
                else
                    UpdateOrderSubmit.Is_Urgent = "N";
                DateTime tempDate = DateTime.MinValue;
                if (chkTestDateInDate.Checked && DateTime.TryParse(cstdtpTestDate.Value, out tempDate))
                {
                    UpdateOrderSubmit.Test_Date = Convert.ToDateTime(cstdtpTestDate.Value).ToString("yyyy-MM-dd");//UpdateOrderSubmit.Test_Date = (cstdtpTestDate.SelectedDate ?? new DateTime()).ToString("yyyy-MM-dd");
                    if (chkStat.Checked)
                    {
                        UpdateOrderSubmit.Stat = "Y";
                    }
                    else
                    {
                        UpdateOrderSubmit.Stat = "N";
                    }
                    UpdateOrderSubmit.TestDate_In_Months = 0;
                    UpdateOrderSubmit.TestDate_In_Days = 0;
                    UpdateOrderSubmit.TestDate_In_Weeks = 0;
                }
                else if (chkTestDateInWords.Checked)
                {
                    if (txtMonths.Value != string.Empty)
                        UpdateOrderSubmit.TestDate_In_Months = Convert.ToInt16(txtMonths.Value);
                    if (txtDays.Value != string.Empty)
                        UpdateOrderSubmit.TestDate_In_Days = Convert.ToInt16(txtDays.Value);
                    if (txtWeeks.Value != string.Empty)
                        UpdateOrderSubmit.TestDate_In_Weeks = Convert.ToInt16(txtWeeks.Value);
                    UpdateOrderSubmit.Stat = "";
                    UpdateOrderSubmit.Test_Date = string.Empty;
                }
                UpdateOrderSubmit.Specimen_Type = cbospecimen.Items[cbospecimen.SelectedIndex].Text;
                if (txtQuantity.Value != string.Empty)
                    UpdateOrderSubmit.Quantity = Convert.ToInt32(txtQuantity.Value);
                else
                    UpdateOrderSubmit.Quantity = 0;
                UpdateOrderSubmit.Specimen_Unit = cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text != string.Empty ? cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text : " ";
                if (dtpCollectionDate.Value != "")
                    //UpdateOrderSubmit.Specimen_Collection_Date_And_Time = Convert.ToDateTime(dtpCollectionDate.DateInput.SelectedDate);
                    UpdateOrderSubmit.Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpCollectionDate.Value));
                else
                // UpdateOrderSubmit.Specimen_Collection_Date_And_Time = DateTime.Now;
                {
                    string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                    UpdateOrderSubmit.Specimen_Collection_Date_And_Time = DateTime.MinValue;
                }
                //if (dtpCollectionDate.SelectedDate != null)
                //    UpdateOrderSubmit.Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(dtpCollectionDate.SelectedDate ?? new DateTime());
                if (chkYesFasting.Checked)
                {
                    UpdateOrderSubmit.Fasting = "Y";
                }
                else
                {
                    UpdateOrderSubmit.Fasting = "N";
                }



                if (rbtnSubmitImmediately.Checked == true)
                    UpdateOrderSubmit.Is_Submit_Immediately = "Y";
                else
                    UpdateOrderSubmit.Is_Submit_Immediately = "N";
                if (!IsUpdate)
                    SavListOrderSubmit.Add(UpdateOrderSubmit);
            }
            #endregion
            string sPlanText = string.Empty;

            #region OrdersAssessment
            IList<OrdersAssessment> savOrdAssList = new List<OrdersAssessment>();
            IList<OrdersAssessment> delOrdAssList = new List<OrdersAssessment>();
            IList<OrdersAssessment> savOrdAssListFornewCPT = new List<OrdersAssessment>();
            OrdersAssessment AssessmentObj = new OrdersAssessment();
            string currentICD = string.Empty;
            IList<OrdersAssessment> tempUpdOrderAssessment = new List<OrdersAssessment>();
            IList<OrdersAssessment> tempOrderAssessment = new List<OrdersAssessment>();
            IList<OrdersAssessment> SaveAssessment = new List<OrdersAssessment>();
            //Boolean bSet = false;

            if (SavListOrderSubmit.Count > 0)
            {
                for (int so = 0; so < SavListOrderSubmit.Count; so++)
                {
                    for (int sa = 0; sa < SelectedAssessment.Count(); sa++)
                    {
                        //currentICD = Convert.ToString(chklstAssessment.Items[sa].Text);
                        currentICD = Convert.ToString(SelectedAssessment[sa]);
                        AssessmentObj = new OrdersAssessment();
                        AssessmentObj.Created_By = ClientSession.UserName;
                        AssessmentObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        AssessmentObj.Encounter_ID = EncounterID;
                        AssessmentObj.Human_ID = HumanID;
                        AssessmentObj.ICD = currentICD.Split('-')[0];
                        AssessmentObj.ICD_Description = currentICD.Substring(currentICD.IndexOf('-') + 1, currentICD.Length - currentICD.IndexOf('-') - 1);
                        AssessmentObj.Order_Submit_ID = SavListOrderSubmit[so].Id;
                        //AssessmentObj.Internal_Property_Associated_Order_CPT = SaveOrderList[so].Lab_Procedure;
                        AssessmentObj.Source = AssessmentSource[AssessmentObj.ICD].Split('-')[0];
                        AssessmentObj.Source_ID = Convert.ToUInt64(AssessmentSource[AssessmentObj.ICD].Split('-')[1]);
                        savOrdAssListFornewCPT.Add(AssessmentObj);
                    }
                }
            }
            if (UpdListOrderSubmit.Count > 0)
            {
                for (int uo = 0; uo < UpdListOrderSubmit.Count; uo++)
                {
                    IList<OrdersAssessment> CurrentOrderAssessment = OriginalOrdersAssessment.Where(o => o.Order_Submit_ID == UpdListOrderSubmit[uo].Id).ToList<OrdersAssessment>();
                    List<string> ICDList = CurrentOrderAssessment.Select(c => c.ICD).ToList();
                    IList<string> UpdateAssessmentList = SelectedAssessment.Where(s => !ICDList.Any(ic => ic.ToString() == s.ToString().Split('-')[0])).ToList();
                    for (int ua = 0; ua < UpdateAssessmentList.Count(); ua++)
                    {
                        currentICD = Convert.ToString(UpdateAssessmentList[ua]);
                        AssessmentObj = new OrdersAssessment();
                        AssessmentObj.Created_By = ClientSession.UserName;
                        AssessmentObj.Human_ID = HumanID;
                        AssessmentObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();//BugID:50207
                        AssessmentObj.Encounter_ID = EncounterID;
                        AssessmentObj.ICD = currentICD.Split('-')[0];
                        AssessmentObj.Order_Submit_ID = UpdListOrderSubmit[uo].Id;
                        AssessmentObj.ICD_Description = currentICD.Substring(currentICD.IndexOf('-') + 1, currentICD.Length - currentICD.IndexOf('-') - 1);
                        //AssessmentObj.Internal_Property_Associated_Order_CPT = UpdateOrdersList[uo].Lab_Procedure;
                        AssessmentObj.Source = AssessmentSource[AssessmentObj.ICD].Split('-')[0];
                        AssessmentObj.Source_ID = Convert.ToUInt64(AssessmentSource[AssessmentObj.ICD].Split('-')[1]);
                        savOrdAssListFornewCPT.Add(AssessmentObj);
                    }
                }
            }
            List<string> SelectedICD = SelectedAssessment.Select(s => s.Split('-')[0]).ToList();
            if (OriginalOrdersAssessment != null && OriginalOrdersAssessment.Count() > 0 && SelectedICD != null)
                delOrdAssList = OriginalOrdersAssessment.Where(oa => !SelectedICD.Any(si => si == oa.ICD)).ToList<OrdersAssessment>();
            #region Commented
            /*Commanded By Manimaran for incorrect entry 
        for (int i = 0; i < chklstAssessment.Items.Count; i++)
        {
            if (chklstAssessment.Items[i].Selected)
            {
                bSet = false;
                if (OriginalOrdersAssessment.Count != 0)
                {
                    for (int iCount = 0; iCount >= OriginalOrdersAssessment.Count; iCount++)
                    {
                        if (chklstAssessment.Items[i].ToString().Split('-')[0] == OriginalOrdersAssessment[iCount].ICD)
                        {

                            bSet = true;
                            break;
                        }
                    }
                }

                if (bSet == false && SaveOrderList.Count>0)
                {

                    foreach (Orders rec in SaveOrderList)
                    {
                        currentICD = chklstAssessment.Items[i].ToString();
                        AssessmentObj = new OrdersAssessment();
                        AssessmentObj.Created_By = ClientSession.UserName;
                        //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                        //AssessmentObj.Created_Date_And_Time = Convert.ToDateTime(strtime);
                        AssessmentObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        AssessmentObj.Encounter_ID = EncounterID;
                        AssessmentObj.ICD = currentICD.Split('-')[0];
                        string ICDDescription = string.Empty;
                        for (int k = 1; k < currentICD.Split('-').Count(); k++)
                        {
                            if (ICDDescription == string.Empty)
                                ICDDescription = currentICD.Split('-')[k];
                            else
                                ICDDescription += "-" + currentICD.Split('-')[k];
                        }
                        AssessmentObj.ICD_Description = ICDDescription;
                        AssessmentObj.Associated_Order_CPT = rec.Lab_Procedure;
                        AssessmentObj.Source = AssessmentSource[AssessmentObj.ICD].Split('-')[0];
                        AssessmentObj.Source_ID = Convert.ToUInt64(AssessmentSource[AssessmentObj.ICD].Split('-')[1]);
                        savOrdAssListFornewCPT.Add(AssessmentObj);
                    }
                }
            }
        }
        for (int i = 0; i < OriginalOrdersAssessment.Count; i++)
        {
            for (int chklstAssCount = 0; chklstAssCount < chklstAssessment.Items.Count; chklstAssCount++)
            {
                if ((chklstAssessment.Items[chklstAssCount].ToString()).Contains(OriginalOrdersAssessment[i].ICD + "-" + OriginalOrdersAssessment[i].ICD_Description) == false)
                {
                    delOrdAssList.Add(OriginalOrdersAssessment[i]);

                }
            }
        }
        */
            #endregion

            //objOrdersManager.UpdateOrders(SaveOrderList, UpdateOrdersList, DeleteOrdersList, savOrdAssList, delOrdAssList, savOrdAssListFornewCPT, null, null, null, null, new string[] { }, new string[] { }, new string[] { }, EncounterID, OrderType.ToUpper(), string.Empty, null, SavListOrderSubmit, UpdListOrderSubmit, DelListOrderSubmit, null, null, null);
            //saravanan
            //ilstRequiredForms = ClientSession.RequiredForms_ReturnList;
            IList<OrdersRequiredForms> SaveListOrdersRequiredForms = new List<OrdersRequiredForms>();
            IList<OrdersRequiredForms> UpdateListOrdersRequiredForms = new List<OrdersRequiredForms>();
            IList<OrdersRequiredForms> DeleteListOrdersRequiredForms = new List<OrdersRequiredForms>();
            IList<ulong> DelOrderIDList = new List<ulong>();
            DelOrderIDList = DeleteOrdersList.Select(a => a.Id).ToList();
            DeleteListOrdersRequiredForms = ilstRequiredForms.Where(a => DelOrderIDList.Contains(a.Order_Id)).ToList<OrdersRequiredForms>();
            SaveListOrdersRequiredForms = ilstRequiredForms.Where(a => a.Order_Id == 0).ToList<OrdersRequiredForms>();
            UpdateListOrdersRequiredForms = ilstRequiredForms.Where(a => a.Order_Id != 0 && !DelOrderIDList.Contains(a.Order_Id)).ToList<OrdersRequiredForms>();
            object[] OrdersRequiredFormsSaveUpdateList = new object[] { SaveListOrdersRequiredForms, UpdateListOrdersRequiredForms, DeleteListOrdersRequiredForms };

            objOrdersManager = new OrdersManager();
            objOrdersManager.UpdateOrders(SaveOrderList, UpdateOrdersList, DeleteOrdersList, savOrdAssList, delOrdAssList, savOrdAssListFornewCPT, null, null, null, null, new string[] { }, new string[] { }, new string[] { }, EncounterID, OrderType.ToUpper(), string.Empty, null, SavListOrderSubmit, UpdListOrderSubmit, DelListOrderSubmit, SaveListOrdersRequiredForms, UpdateListOrdersRequiredForms, DeleteListOrdersRequiredForms, sLocalTime);
            //IList<Orders> lstTemp = new List<Orders>();            
            //lstTemp = SaveOrderList.Concat(UpdateOrdersList).ToList<Orders>();
            //IList<string> ProceduresViewListTemp = new List<string>();
            //ProceduresViewListTemp = lstTemp.Select(a => a.Lab_Procedure + "-" + (a.Lab_Procedure_Description.Contains("x_") ? ( a.Lab_Procedure_Description + "~" + a.Quantity + "|" + a.Id) : a.Lab_Procedure_Description)).ToList<string>();
            //SetOrderIDForEditQuantity(ProceduresViewListTemp);
            #endregion

        }

        protected void btnPrintRequsition_Click(object sender, EventArgs e)
        {

            hdnSelectedItem.Value = string.Empty;
            //string path = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["LabCorpRequesitionFilePathName"]);
            string path = Server.MapPath("Documents/" + Session.SessionID);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string FileLocation = string.Empty;

            PrintOrders print = new PrintOrders();
            // FileLocation = print.CallPrintLabAndImageOrders(path,EncounterID);
            OrdersDTO objOrderDTO = null;
            object objDTO;
            var serializer = new NetDataContractSerializer();
            objDTO = (object)serializer.ReadObject(objOrdersManager.LoadOrders(EncounterID, PhysicianID, HumanID, OrderType, string.Empty, UtilityManager.ConvertToUniversal(DateTime.Now), false));
            objOrderDTO = (OrdersDTO)objDTO;
            FillHumanDTO objFillHumnaDTO = new FillHumanDTO();
            if (Session["objFillHumanDTO"] != null)
                objFillHumnaDTO = (FillHumanDTO)Session["objFillHumanDTO"];

            //IList<string> LabAndOrderSubmitID = objOrdersManager.GetOrderSubmitIDForPrintRequestion(EncounterID, PhysicianID, HumanID);
            IList<string> LabAndOrderSubmits = new List<string>();
            LabAndOrderSubmits = objOrdersManager.GetOrderSubmitIDForPrintRequestion(EncounterID, PhysicianID, HumanID, OrderType);
            IList<string> LabAndOrderSubmitID = LabAndOrderSubmits.Distinct().ToList<string>();

            foreach (string str in LabAndOrderSubmitID)
            {
                if (str.StartsWith("1"))
                    FileLocation = print.PrintRequisitionUsingDatafromDB(Convert.ToUInt32(str.Split('|')[1]), path, "ORDERS", EncounterID, OrderType);
                else
                    FileLocation = print.PrintSplitRequisitionUsingDatafromDBQuest(Convert.ToUInt32(str.Split('|')[1]), path, "ORDERS", false, OrderType);

                string[] Split1 = new string[] { Server.MapPath("") };
                string[] FileName1 = FileLocation.Split(Split1, StringSplitOptions.RemoveEmptyEntries);


                if (FileName1.Count() > 0)
                {
                    for (int i = 0; i < FileName1.Count(); i++)
                    {
                        if (hdnSelectedItem.Value == string.Empty)
                        {
                            hdnSelectedItem.Value = FileName1[i].ToString();
                        }
                        else
                        {
                            hdnSelectedItem.Value += "|" + FileName1[i].ToString();
                        }

                    }
                }
                //if (hdnSelectedItem.Value == string.Empty)
                //{
                //    hdnSelectedItem.Value = FileName1[0].ToString();
                //}
                //else
                //{
                //    hdnSelectedItem.Value += "|" + FileName1[0].ToString();
                //}
            }

            FileLocation = print.CallPrintLabAndImageOrders(path, EncounterID, objOrderDTO, objFillHumnaDTO, OrderType);
            if (FileLocation != string.Empty)
            {

                string[] Split = new string[] { Server.MapPath("") };
                string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);

                if (FileName.Count() > 0)
                {
                    for (int i = 0; i < FileName.Count(); i++)
                    {
                        if (hdnSelectedItem.Value == string.Empty)
                        {
                            hdnSelectedItem.Value = FileName[i].ToString();
                        }
                        else
                        {
                            hdnSelectedItem.Value += "|" + FileName[i].ToString();
                        }

                    }
                }
                //if (hdnSelectedItem.Value == string.Empty)
                //{
                //    hdnSelectedItem.Value = FileName[0].ToString();
                //}
                //else
                //{
                //    hdnSelectedItem.Value += "|" + FileName[0].ToString();
                //}
            }




            if (hdnSelectedItem.Value != string.Empty)
            {
                string FaxSubject = string.Empty;
                string sLABNAME = string.Empty;

                if (objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit != null)
                {
                    if (objOrderDTO.ilstOrderLabDetailsDTO.Count > 0)
                    {
                        for (int y = 0; y < objOrderDTO.ilstOrderLabDetailsDTO.Count; y++)
                        {
                            if (y == 0)
                                sLABNAME = objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Id + "_" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Lab_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy");
                            else
                                sLABNAME += "|" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Id + "_" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Lab_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[y].OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy");
                        }
                        //
                    }
                }
                //FaxSubject = "_" + objFillHumnaDTO.First_Name + " " + objFillHumnaDTO.Last_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Created_Date_And_Time;    
                FaxSubject = "_" + objFillHumnaDTO.First_Name + " " + objFillHumnaDTO.Last_Name + "$" + sLABNAME; //"$" + objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Created_Date_And_Time;               

                //MessageWindow.Width = Unit.Pixel(750);
                //MessageWindow.Height = Unit.Pixel(600);
                //MessageWindow.OnClientClose = "CloseQuestionSetWindow";
                //MessageWindow.NavigateUrl = "frmPrintPDF.aspx?Location=DYNAMIC&SI=" + hdnSelectedItem.Value.ToString();
                //MessageWindow.VisibleStatusbar = false;
                //MessageWindow.VisibleOnPageLoad = true;

                ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage('" + FaxSubject + "');", true);
            }
            else if (LabAndOrderSubmitID.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Diagnostic Order", "DisplayErrorMessage('230115'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }

            //hdnSelectedItem.Value = string.Empty;
            if ((cboLab.SelectedIndex != 32))
            {
                btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
            }
            if (cboLab.SelectedIndex == 0)
            {
                btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
            }
            //ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDF();", true);

        }
        protected void btnGenerateABN_Click(object sender, EventArgs e)
        {
            hdnSelectedItem.Value = string.Empty;
            //string path = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["LabCorpRequesitionFilePathName"]);
            string path = Server.MapPath("Documents/" + Session.SessionID);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string FileLocation = string.Empty;
            FillHumanDTO humanRecord;
            var serializer = new NetDataContractSerializer();
            OrdersDTO objOrderDTO = null;
            object objDTO;
            objDTO = (object)serializer.ReadObject(objOrdersManager.LoadOrders(EncounterID, PhysicianID, HumanID, OrderType, string.Empty, UtilityManager.ConvertToUniversal(DateTime.Now), false));
            objOrderDTO = (OrdersDTO)objDTO;
            humanRecord = objOrderDTO.objHuman;
            string sHumanName = objOrderDTO.objHuman.Last_Name + "," + objOrderDTO.objHuman.First_Name;
            string sIdentificationNumber = objOrderDTO.objHuman.Human_ID.ToString();
            string humanName = objOrderDTO.objHuman.Last_Name + "," + objOrderDTO.objHuman.First_Name + " " + objOrderDTO.objHuman.MI;
            PrintOrders print = new PrintOrders();
            bool IsABNNeed = false;
            ResultMasterManager objResultMasterManager = new ResultMasterManager();
            IList<string> LabCorpCPT = new List<string>();
            IList<string> LabCorpICD = new List<string>();
            IList<string> QuestOrderSubmitID = new List<string>();
            IList<Orders> QuestORdersList = new List<Orders>();

            IList<OrdersAssessment> OrdersAssessmentList = new List<OrdersAssessment>();
            objResultMasterManager.GenerateABNDatas(EncounterID, PhysicianID, HumanID, out LabCorpCPT, out LabCorpICD, out QuestOrderSubmitID, out OrdersAssessmentList, out QuestORdersList, objOrderDTO.OrderAssList, objOrderDTO.Lists);
            AbnResponseType objAbnResponseType = new AbnResponseType();
            Acurus.Capella.DataAccess.com.labcorp.www4.Message[] objMessage;
            LabCarrierLookUpManager objLabCarrierLookUpManager = new LabCarrierLookUpManager();
            var ilstorder_submit_id = (from ordsubmitID in QuestOrderSubmitID where ordsubmitID != "0" select ordsubmitID).Distinct().ToList();
            InsurancePlanManager objInsurancePlanManager = new InsurancePlanManager();
            string FilePath = string.Empty;
            string RetrunFilePath = string.Empty;
            if (QuestOrderSubmitID != null && QuestOrderSubmitID.Count > 0)
            {
                IList<LabSettings> LabList = new List<LabSettings>();
                LabcorpSettingsManager objLabcorpSettingsManager = new LabcorpSettingsManager();
                LabList = objLabcorpSettingsManager.GetLabcorpSettings();
                LabList = LabList.Where(a => a.Lab_ID == 2).ToList<LabSettings>();

                InsurancePlan objInsurancePlan = null;
                if (humanRecord.PatientInsuredBag != null && humanRecord.PatientInsuredBag.Count > 0)
                    objInsurancePlan = objInsurancePlanManager.GetInsurancebyID(humanRecord.PatientInsuredBag[0].Insurance_Plan_ID)[0];
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230125');", true);
                    return;
                }
                LabCarrierLookUp objLabCarrierLookUp = objLabCarrierLookUpManager.GetLabCarrierDetailsForInsPlanID(objInsurancePlan.Id, 2);
                foreach (string ul in QuestOrderSubmitID)
                {
                    IList<Orders> tempOrders = QuestORdersList.Where(a => a.Order_Submit_ID == Convert.ToUInt64(ul)).ToList<Orders>();
                    string OrdeCode = string.Empty;
                    foreach (Orders obj in tempOrders)
                    {
                        OrdeCode += obj.Lab_Procedure + "\t" + obj.Lab_Procedure_Description + "\n";
                    }

                    IsABNNeed = print.GenerateABN(LabList[0].Lab_Client_Account_No, LabList[0].Receiving_Facility, humanRecord.Human_ID.ToString(), (humanRecord.Last_Name + "," + humanRecord.First_Name + "," + humanRecord.MI), objLabCarrierLookUp.LCA_Carrier_Code, objInsurancePlan.Ins_Plan_Name, objInsurancePlan.Payer_Addrress1, "ACUR" + ul.ToString(), OrdersAssessmentList, OrdeCode, LabList[0].User_Name, LabList[0].Password, objInsurancePlan.Payer_City, objInsurancePlan.Payer_State, objInsurancePlan.Payer_Zip, true, LabList[0].URL, path, out RetrunFilePath); //objOrdersUtilityManager.GenerateABNForQuest(ilstQuestOrderSubmitIDs, true);
                    FilePath = RetrunFilePath;
                    hdnSelectedItem.Value = FilePath;
                }
            }
            FacilityManager objFacilityMgr = new FacilityManager();
            IList<FacilityLibrary> ilstFacility = new List<FacilityLibrary>();
            if (objOrderDTO.ilstOrdersSubmitForPartialOrders.Count > 0)
                ilstFacility = objFacilityMgr.GetFacilityByFacilityname(objOrderDTO.ilstOrdersSubmitForPartialOrders[0].Facility_Name);
            string AccountNumberForLabCorp = string.Empty;
            if (ilstFacility.Count > 0)
            {
                AccountNumberForLabCorp = ilstFacility[0].LabCorp_Account_Number;
            }
            else
                AccountNumberForLabCorp = "04281070";
            objAbnResponseType = (AbnResponseType)objResultMasterManager.GenerateABNForm(AccountNumberForLabCorp, sHumanName, sIdentificationNumber, LabCorpCPT.Distinct().ToArray<string>(), LabCorpICD.Distinct().ToArray<string>());
            if (objAbnResponseType.outputType == "Success")
            {
                if ((bool)objAbnResponseType.isABNRequired)
                {
                    if (objAbnResponseType.contentType.ToUpper().Contains("PDF") && objAbnResponseType.abnContents.Length > 0)
                    {
                        //path = string.Empty;
                        //path = System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["ABNForm"];
                        //DirectoryInfo dir = new DirectoryInfo(path);
                        //if (dir.Exists == false)
                        //{
                        //    DirectoryInfo d = Directory.CreateDirectory(path);
                        //}
                        RetrunFilePath = string.Empty;
                        RetrunFilePath = path + "\\" + sHumanName + "_" + sIdentificationNumber + "_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf";
                        using (var f = System.IO.File.Create(RetrunFilePath))
                        {
                            f.Write(objAbnResponseType.abnContents, 0, objAbnResponseType.abnContents.Length);
                            hdnSelectedItem.Value = "|" + RetrunFilePath.Replace(Server.MapPath(""), string.Empty);
                            f.Close();
                        }
                        try
                        {
                            IsABNNeed = true;
                            //System.Diagnostics.Process.Start(path);
                            IsABNNeed = true;
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    IsABNNeed = false;
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230125');", true);
                }
            }
            else if (objAbnResponseType.outputType == "Failure")
            {
                objMessage = new Acurus.Capella.DataAccess.com.labcorp.www4.Message[objAbnResponseType.messages.Count()];
                string err = string.Empty;
                objMessage = objAbnResponseType.messages;
                foreach (Acurus.Capella.DataAccess.com.labcorp.www4.Message msg in objMessage)
                {
                    err += msg.text + System.Environment.NewLine;
                }
                if (err != string.Empty)
                {
                    if (IsABNNeed)
                    {
                        string[] sErr = err.ToString().Split('.');
                        if (sErr[0].ToString() == "Test Code was not found" && !IsABNNeed)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230132'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        }
                        else
                        {
                            //RadMessageBox.Show("LabCorp Response:" + err);
                        }
                    }
                }
            }

            if (!IsABNNeed)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230132'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            if (hdnSelectedItem.Value != string.Empty)
            {
                if (hdnSelectedItem.Value.ToString().StartsWith("|"))
                {
                    hdnSelectedItem.Value = hdnSelectedItem.Value.ToString().Substring(1);
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "OpenPrintPDF('" + hdnSelectedItem.Value.ToString() + "')", true);

                //MessageWindow.Width = Unit.Pixel(900);
                //MessageWindow.Height = Unit.Pixel(750);
                //MessageWindow.NavigateUrl = "frmPrintPDF.aspx?Location=DYNAMIC&SI=" + hdnSelectedItem.Value.ToString();
                //MessageWindow.VisibleStatusbar = false;
                //MessageWindow.VisibleOnPageLoad = true;
            }
        }

        protected void btnMoveToNextProcess_Click(object sender, EventArgs e)
        {
            #region Jira CAP-1555 and CAP-1646Old Code
            //if (Session["OrderSubmitId"] != null && Session["OrderSubmitId"].ToString() != string.Empty)
            //{
            //    hdnTransferVaraible.Value = Convert.ToString(Session["OrderSubmitId"]);
            //}
            //string OrdersSubmitIDString = hdnTransferVaraible.Value != null ? hdnTransferVaraible.Value.ToString() : string.Empty;
            //if (OrdersSubmitIDString != string.Empty)
            //{
            //    ulong order_submit_id = 0;
            //    ulong.TryParse(OrdersSubmitIDString, out order_submit_id);
            //    int close_type = 0;
            //    string User_name = string.Empty;
            //    string sProcess = string.Empty;
            //    sProcess = ClientSession.UserCurrentProcess;

            //    // Added By Suvarnni M V for Bug id:29140
            //    ulong uLabID = 0;
            //    uLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
            //    if (uLabID != 32)
            //    {
            //        if (!btnOrderSubmit.Disabled)
            //        {
            //            if (hdnType.Value != "")
            //            {
            //                btnOrderSubmit_Click(sender, e);
            //                if (hdnMovetoNextProcess.Value == "false")
            //                    return;
            //            }
            //        }

            //        var serializer = new NetDataContractSerializer();
            //        OrdersDTO objOrderDTO = null;
            //        object objDTO;
            //        objDTO = (object)serializer.ReadObject(objOrdersManager.LoadOrders(EncounterID, PhysicianID, HumanID, OrderType, string.Empty, UtilityManager.ConvertToUniversal(DateTime.Now), false));
            //        objOrderDTO = (OrdersDTO)objDTO;
            //        if (objOrderDTO.ilstOrderLabDetailsDTO.Count > 0 || objOrderDTO.ilstOrdersSubmitForPartialOrders.Count > 0)
            //        {

            //            IList<OrdersSubmit> maReviewOrders = (from o in objOrderDTO.ilstOrderLabDetailsDTO where o.OrdersSubmit.Id == order_submit_id && o.ObjOrder.Internal_Property_Current_Process == "MA_REVIEW" select o.OrdersSubmit).Distinct().ToList<OrdersSubmit>();
            //            int emptyCount = (from o in maReviewOrders where o.Bill_Type == string.Empty select o).ToList<OrdersSubmit>().Count;
            //            IList<OrdersSubmit> maReviewOrders1 = (from o in objOrderDTO.ilstOrdersSubmitForPartialOrders where o.Id == order_submit_id select o).Distinct().ToList<OrdersSubmit>();
            //            if (maReviewOrders1 != null && maReviewOrders1.Count > 0)
            //            {
            //                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230126'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //                //ApplicationObject.erroHandler.DisplayErrorMessage("230126", OrderType.ToUpper());
            //                return;
            //            }
            //            if (emptyCount == 0)
            //            {
            //                WFObjectManager obj_workFlow = new WFObjectManager();
            //                obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
            //                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('280013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowCloseDiagnostics();", true);
            //                return;
            //            }
            //            else if (emptyCount == maReviewOrders.Count)
            //            {
            //                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230126'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //                //ApplicationObject.erroHandler.DisplayErrorMessage("230126", OrderType.ToUpper());
            //                return;
            //            }

            //        }
            //    }
            //    else
            //    {
            //        Boolean bReturnValue;
            //        bReturnValue = CheckForValidation();
            //        if (!bReturnValue)
            //            return;
            //        FileManagementIndexManager objFileManagementMngr = new FileManagementIndexManager();
            //        IList<FileManagementIndex> lstfilemanage = new List<FileManagementIndex>();
            //        lstfilemanage = objFileManagementMngr.GetImagesforAnnotations(HumanID, order_submit_id, "ORDER,ABI,SPIROMETRY,SCAN");
            //        if (lstfilemanage.Count > 0)
            //        {
            //            if (sProcess == "ORDER_GENERATE" && ClientSession.UserRole == "Medical Assistant")
            //            {

            //                PhysicianManager objPhysicianMngr = new PhysicianManager();
            //                close_type = 8;
            //                FillPhysicianUser PhyUserList = objPhysicianMngr.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
            //                User_name = PhyUserList.UserList.Where(P => P.Physician_Library_ID == PhysicianID).ToList()[0].user_name;

            //            }
            //            else if (sProcess == "MA_REVIEW" && ClientSession.UserRole == "Medical Assistant")
            //            {
            //                close_type = 1;
            //                User_name = "UNKNOWN";

            //            }
            //            else if (sProcess == "ORDER_GENERATE" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
            //            {
            //                close_type = 9;
            //                User_name = "UNKNOWN";
            //            }
            //            else if (sProcess == "RESULT_REVIEW" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
            //            {
            //                close_type = 1;
            //                User_name = "UNKNOWN";
            //            }
            //            if (ClientSession.UserRole == "Physician Assistant" || ClientSession.UserRole == "Physician")
            //            {
            //                WFObjectManager obj_workFlow = new WFObjectManager();
            //                obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);

            //                btnMoveToNextProcess.Disabled = true; //btnMoveToNextProcess.Enabled = false;

            //            }
            //            else if (ClientSession.UserRole == "Medical Assistant")
            //            {
            //                WFObjectManager obj_workFlow = new WFObjectManager();
            //                //if (uLabID != 32)
            //                //    obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
            //                ////Response.Write("<script> self.close(); </script>");
            //                //else
            //                //{
            //                obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
            //                if (sProcess == "MA_REVIEW")
            //                {
            //                    PhysicianManager objPhysicianMngr = new PhysicianManager();
            //                    FillPhysicianUser PhyUserList = objPhysicianMngr.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
            //                    User_name = PhyUserList.UserList.Where(P => P.Physician_Library_ID == PhysicianID).ToList()[0].user_name;
            //                    obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", 8, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
            //                }
            //                // }
            //                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('280013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowCloseDiagnostics();", true);
            //            }
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMessage", "DisplayErrorMessage('115039', '',''); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //            //ApplicationObject.erroHandler.DisplayErrorMessage("115039", this.Text);
            //            return;
            //        }
            //    }



            //    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('280013');", true);
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowClose();", true);
            //    Session["OrderSubmitId"] = string.Empty;

            //}
            #endregion
            #region Jira CAP-1555 and CAP-1646New Code
            if (Session["OrderSubmitId"] != null && Session["OrderSubmitId"].ToString() != string.Empty)
            {
                hdnTransferVaraible.Value = Convert.ToString(Session["OrderSubmitId"]);
            }
            else if (hdnMovetoOrderSubmitId.Value != null && hdnMovetoOrderSubmitId.Value != string.Empty)
            {
                hdnTransferVaraible.Value = Convert.ToString(hdnMovetoOrderSubmitId.Value);
            }
           
            string OrdersSubmitIDString = hdnTransferVaraible.Value != null ? hdnTransferVaraible.Value.ToString() : string.Empty;
            if (OrdersSubmitIDString != string.Empty)
            {
                ulong order_submit_id = 0;
                ulong.TryParse(OrdersSubmitIDString, out order_submit_id);
                int close_type = 0;
                string User_name = string.Empty;
                string sProcess = string.Empty;
                sProcess = ClientSession.UserCurrentProcess;

                // Added By Suvarnni M V for Bug id:29140
                //ulong uLabID = 0;
                //uLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                //if (uLabID != 32)
                //{
                    //if (!btnOrderSubmit.Disabled)
                    //{
                        //if (hdnType.Value != "")
                        //{
                        //    btnOrderSubmit_Click(sender, e);
                        //    if (hdnMovetoNextProcess.Value == "false")
                        //        return;
                        //}
                    //}

                    var serializer = new NetDataContractSerializer();
                    OrdersDTO objOrderDTO = null;
                    object objDTO;
                    objDTO = (object)serializer.ReadObject(objOrdersManager.LoadOrders(EncounterID, PhysicianID, HumanID, OrderType, string.Empty, UtilityManager.ConvertToUniversal(DateTime.Now), false));
                    objOrderDTO = (OrdersDTO)objDTO;
                    if (objOrderDTO.ilstOrderLabDetailsDTO.Count > 0 || objOrderDTO.ilstOrdersSubmitForPartialOrders.Count > 0)
                    {

                        IList<OrdersSubmit> maReviewOrders = (from o in objOrderDTO.ilstOrderLabDetailsDTO where o.OrdersSubmit.Id == order_submit_id && o.ObjOrder.Internal_Property_Current_Process == "MA_REVIEW" select o.OrdersSubmit).Distinct().ToList<OrdersSubmit>();
                        int emptyCount = (from o in maReviewOrders where o.Bill_Type == string.Empty select o).ToList<OrdersSubmit>().Count;
                        IList<OrdersSubmit> maReviewOrders1 = (from o in objOrderDTO.ilstOrdersSubmitForPartialOrders where o.Id == order_submit_id select o).Distinct().ToList<OrdersSubmit>();
                        if (maReviewOrders1 != null && maReviewOrders1.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230126'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            //ApplicationObject.erroHandler.DisplayErrorMessage("230126", OrderType.ToUpper());
                            return;
                        }
                        if (emptyCount == 0)
                        {
                            WFObjectManager obj_workFlow = new WFObjectManager();
                            obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('280013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowCloseDiagnostics();", true);
                            return;
                        }
                        else if (emptyCount == maReviewOrders.Count)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230126'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            //ApplicationObject.erroHandler.DisplayErrorMessage("230126", OrderType.ToUpper());
                            return;
                        }

                    }
                //}
                //else
                //{
                //    Boolean bReturnValue;
                //    bReturnValue = CheckForValidation();
                //    if (!bReturnValue)
                //        return;
                //    FileManagementIndexManager objFileManagementMngr = new FileManagementIndexManager();
                //    IList<FileManagementIndex> lstfilemanage = new List<FileManagementIndex>();
                //    lstfilemanage = objFileManagementMngr.GetImagesforAnnotations(HumanID, order_submit_id, "ORDER,ABI,SPIROMETRY,SCAN");
                //    if (lstfilemanage.Count > 0)
                //    {
                //        if (sProcess == "ORDER_GENERATE" && ClientSession.UserRole == "Medical Assistant")
                //        {

                //            PhysicianManager objPhysicianMngr = new PhysicianManager();
                //            close_type = 8;
                //            FillPhysicianUser PhyUserList = objPhysicianMngr.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
                //            User_name = PhyUserList.UserList.Where(P => P.Physician_Library_ID == PhysicianID).ToList()[0].user_name;

                //        }
                //        else if (sProcess == "MA_REVIEW" && ClientSession.UserRole == "Medical Assistant")
                //        {
                //            close_type = 1;
                //            User_name = "UNKNOWN";

                //        }
                //        else if (sProcess == "ORDER_GENERATE" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                //        {
                //            close_type = 9;
                //            User_name = "UNKNOWN";
                //        }
                //        else if (sProcess == "RESULT_REVIEW" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                //        {
                //            close_type = 1;
                //            User_name = "UNKNOWN";
                //        }
                //        if (ClientSession.UserRole == "Physician Assistant" || ClientSession.UserRole == "Physician")
                //        {
                //            WFObjectManager obj_workFlow = new WFObjectManager();
                //            obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);

                //            btnMoveToNextProcess.Disabled = true; //btnMoveToNextProcess.Enabled = false;

                //        }
                //        else if (ClientSession.UserRole == "Medical Assistant")
                //        {
                //            WFObjectManager obj_workFlow = new WFObjectManager();
                //            //if (uLabID != 32)
                //            //    obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
                //            ////Response.Write("<script> self.close(); </script>");
                //            //else
                //            //{
                //            obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
                //            if (sProcess == "MA_REVIEW")
                //            {
                //                PhysicianManager objPhysicianMngr = new PhysicianManager();
                //                FillPhysicianUser PhyUserList = objPhysicianMngr.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
                //                User_name = PhyUserList.UserList.Where(P => P.Physician_Library_ID == PhysicianID).ToList()[0].user_name;
                //                obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", 8, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
                //            }
                //            // }
                //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('280013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowCloseDiagnostics();", true);
                //        }
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMessage", "DisplayErrorMessage('115039', '',''); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //        //ApplicationObject.erroHandler.DisplayErrorMessage("115039", this.Text);
                //        return;
                //    }
                //}



                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('280013');", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowClose();", true);
                Session["OrderSubmitId"] = string.Empty;
                hdnMovetoOrderSubmitId.Value = string.Empty;

            }
            #endregion
        }


        protected void pbSelectICD_Click(object sender, ImageClickEventArgs e)
        {

            //if (Session["Selected_ICDs"] != null)
            //{
            //    IList<string> ItemsToBeAdded = (IList<string>)Session["Selected_ICDs"];
            //    if (ItemsToBeAdded.Count > 0)
            //    {
            //        foreach (string str in ItemsToBeAdded)
            //        {
            //            ListItem itm = chklstAssessment.Items.FindByText(str);
            //            if (itm != null)
            //            {
            //                itm.Selected = true;
            //                continue;
            //            }
            //            else
            //            {
            //                itm = new ListItem();
            //                itm.Text = str;
            //                itm.Selected = true;
            //                chklstAssessment.Items.Add(itm);
            //                AssessmentSource.Add(str.Split('-')[0], "OTHERS-0");
            //            }
            //        }
            //        Session["Selected_ICDs"] = null;
            //    }
            //}



        }
        //(IList<string>)Session["Selected_ICDs"];

        //if (hdnTransferVaraible.Value != null && hdnTransferVaraible.Value.ToString() != string.Empty)
        //{
        //    string[] ItemsToBeAdded = hdnTransferVaraible.Value.ToString().Split('$');
        //    foreach (string str in ItemsToBeAdded)
        //    {
        //        ListItem itm = chklstAssessment.Items.FindByText(str);
        //        if (itm != null)
        //        {
        //            itm.Selected = true;
        //            continue;
        //        }
        //        else
        //        {
        //            itm = new ListItem();
        //            itm.Text = str;
        //            itm.Selected = true;
        //            chklstAssessment.Items.Add(itm);
        //            AssessmentSource.Add(str.Split('-')[0], "OTHERS-0");
        //        }

        //    }
        //    hdnTransferVaraible.Value = "";
        //}
        //}

        protected void tblSelectProcedure_Click(object sender, EventArgs e)
        {
            if (btnOrderSubmit.Attributes["Tag"] != null && btnOrderSubmit.Attributes["Tag"].ToString() == "UPDATE")//btnOrderSubmit.Value.ToString().ToUpper() == "UPDATE")
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMessage", "DisplayErrorMessage('230142', '','');", true);
                //ApplicationObject.erroHandler.DisplayErrorMessage("230142", this.Text);
                return;
            }
            ProcedureSwitch(false);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnAllProcedures_Click(object sender, EventArgs e)
        {
            if (hdnTransferVaraible.Value != null && hdnTransferVaraible.Value != string.Empty)
            {
                IList<string> lstChkeditem = new List<string>();
                for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
                {
                    if (chklstFrequentlyUsedProcedures.Items[i].Selected)
                        lstChkeditem.Add(chklstFrequentlyUsedProcedures.Items[i].Text.ToString());
                }
                IList<string> selectedCodes = new List<string>();
                IList<PhysicianProcedure> Originallist = new List<PhysicianProcedure>();
                string[] MovedProcedures = hdnTransferVaraible.Value.ToString().Split('|');
                ulong selectedLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                if (LookUpPerRequest.Keys.Contains("procedureType") == true)
                {
                    procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), selectedLabID, ClientSession.LegalOrg);
                }
                foreach (string s in MovedProcedures)
                {
                    if (!selectedCodes.Contains(s))
                    {
                        selectedCodes.Add(s);
                        // btnImportresult.Enabled = true; // added by balaji.TJ
                    }

                }
                ListItem objListViewItem = new ListItem();
                objListViewItem.Text = "OTHER";
                if (chklstFrequentlyUsedProcedures.Items.Contains(objListViewItem) == false)
                    chklstFrequentlyUsedProcedures.Items.Add(objListViewItem);
                //sbtnAllProceduresClick = true;
                Originallist = new List<PhysicianProcedure>();
                Originallist = new List<PhysicianProcedure>(procedureList);
                PhysicianProcedure objPhysicianProcedure;
                foreach (string str in selectedCodes.Concat(lstChkeditem))
                {
                    string[] SplitStr = str.Split('-');
                    objPhysicianProcedure = new PhysicianProcedure();
                    objPhysicianProcedure.Physician_Procedure_Code = SplitStr[0].ToString();
                    for (int i = 1; i < SplitStr.Count(); i++)
                    {
                        if (i == 1)
                            objPhysicianProcedure.Procedure_Description = SplitStr[i];
                        else
                            objPhysicianProcedure.Procedure_Description += "-" + SplitStr[i];
                    }
                    objPhysicianProcedure.Order_Group_Name = "OTHER";
                    if (!Originallist.Any(a => a.Physician_Procedure_Code == str.Split('-')[0]))
                        Originallist.Add(objPhysicianProcedure);

                }
                FillLabProcedure(Originallist, selectedCodes.Concat(lstChkeditem).ToList<string>());
                hdnTransferVaraible.Value = string.Empty;
            }
            //if (cboLab.SelectedIndex != 32)//committed by balaji
            if (Convert.ToUInt32(cboLab.Items[cboLab.SelectedIndex].Value) != 32)
            {
                btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
            }
            btnOrderSubmit.Disabled = false; //btnOrderSubmit.Enabled = true;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnOrderList_Click(object sender, EventArgs e)
        {
            DiagnosticDTO objDiagnosticDTO = null;
            LoadScreenFromOrderList(objDiagnosticDTO);
        }
        //protected override object SaveViewState()
        //{
        //    // create object array for Item count + 1
        //    object[] allStates = new object[chklstFrequentlyUsedProcedures.Items.Count + 1];

        //    // the +1 is to hold the base info
        //    object baseState = base.SaveViewState();
        //    allStates[0] = baseState;

        //    Int32 i = 1;
        //    // now loop through and save each Style attribute for the List
        //    foreach (ListItem li in chklstFrequentlyUsedProcedures.Items)
        //    {
        //        Int32 j = 0;
        //        string[][] attributes = new string[li.Attributes.Count][];
        //        foreach (string attribute in li.Attributes.Keys)
        //        {
        //            attributes[j++] = new string[] { attribute, li.Attributes[attribute] };
        //        }
        //        allStates[i++] = attributes;
        //    }
        //    return allStates;
        //}

        //protected override void LoadViewState(object savedState)
        //{
        //    if (savedState != null)
        //    {
        //        object[] myState = (object[])savedState;

        //        // restore base first
        //        if (myState[0] != null)
        //            base.LoadViewState(myState[0]);

        //        Int32 i = 1;
        //        foreach (ListItem li in chklstFrequentlyUsedProcedures.Items)
        //        {
        //            // loop through and restore each style attribute
        //            foreach (string[] attribute in (string[][])myState[i++])
        //            {
        //                li.Attributes[attribute[0]] = attribute[1];
        //            }
        //        }
        //    }
        //}


        void ProcedureSwitch(bool IsAllProcedure)
        {
            if (hdnTransferVaraible.Value != null)
            {
                IList<string> lstChkeditem = new List<string>();
                for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
                {
                    if (chklstFrequentlyUsedProcedures.Items[i].Selected == true)
                        lstChkeditem.Add(chklstFrequentlyUsedProcedures.Items[i].Text.ToString());
                }
                ulong selectedLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                if (LookUpPerRequest.Keys.Contains("procedureType") == true)
                {
                    procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), selectedLabID, ClientSession.LegalOrg);
                }
                FillLabProcedure(procedureList, lstChkeditem);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Write("window.self.location.assign('www.google.com');");
        }

        protected void chklstFrequentlyUsedProcedures_PreRender(object sender, EventArgs e)
        {
            chklstFrequentlyUsedProcedures.Height = Unit.Pixel(50);
            for (int i = 0; i < ListViewHeader.Count; i++)
            {
                chklstFrequentlyUsedProcedures.Items[i].Attributes.Add(ListViewHeader[i].Split(';')[0].Split('-')[0], ListViewHeader[i].Split(';')[0].Split('-')[1]);
                chklstFrequentlyUsedProcedures.Items[i].Attributes.Add(ListViewHeader[i].Split(';')[1].Split('-')[0], ListViewHeader[i].Split(';')[1].Split('-')[1]);

            }

            if (ListViewHeader.Count < 10 && ListViewHeader.Count <= 3)
            {
                chklstFrequentlyUsedProcedures.Height = Unit.Pixel(75);
            }
            if (ListViewHeader.Count < 10 && ListViewHeader.Count >= 4)
            {
                chklstFrequentlyUsedProcedures.Height = Unit.Pixel(200);
            }
            if (ListViewHeader.Count >= 10)
            {
                chklstFrequentlyUsedProcedures.Height = Unit.Pixel(300);
            }
            foreach (ListItem lstitem in chklstFrequentlyUsedProcedures.Items)
            {

                lstitem.Attributes.Add("onclick", "Testw(this);");

            }
        }
        //Added saravanakumar for Get Collection Time
        private void SelectedTime(DateTime dtSelectTime)
        {
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            TimeSpan ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
            //dtpStartTime.Hour = ts.Hours;
            //dtpStartTime.Minute = ts.Minutes;
            //dtpStartTime.Second = ts.Seconds;
            //if (MKB.TimePicker.TimeSelector.AmPmSpec.AM.ToString() == dt.ToString("tt"))
            //{
            //    dtpStartTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            //}
            //else
            //{
            //    dtpStartTime.AmPm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            //}
        }

        protected void btnPrintlabel_Click(object sender, EventArgs e)
        {
            hdnSelectedItem.Value = string.Empty;
            IList<ulong> ilstQuestOrderSubmitIDs = new List<ulong>();
            ilstQuestOrderSubmitIDs = objOrdersManager.GetOrderSubmitIDForPrintLable(EncounterID, PhysicianID, HumanID);
            //if (ilstQuestOrderSubmitIDs.Count <= 0)
            //{
            //    if (hdnPrintLabel.Value == "true")
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230141'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        hdnPrintLabel.Value = "";
            //        return;
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230115'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        return;
            //    }
            //}
            if (ilstQuestOrderSubmitIDs.Count > 0)
            {
                //AgentManager objAgentaManger = new AgentManager();
                //if (objAgentaManger.GetPrinterStatus())
                //{
                //    HumanManager HumanMngr = new HumanManager();
                //    string PatientName = HumanMngr.GetPatientNameByHumanID(HumanID);
                //    foreach (ulong ul in ilstQuestOrderSubmitIDs)
                //    {
                //        FacilityManager objfacilityProxy = new FacilityManager();
                //        string AccountNumberForQuest = string.Empty;
                //        IList<FacilityLibrary> ilstFacility = objfacilityProxy.GetFacilityByFacilityname(ClientSession.FacilityName);
                //        if (ilstFacility.Count > 0)
                //        {
                //            AccountNumberForQuest = ilstFacility[0].Quest_Account_Number;
                //        }
                //        //Add Human Name at the end
                //        objAgentaManger.PrintQuestLabel("Client #: " + AccountNumberForQuest + System.Environment.NewLine + "Lab Ref #: " + "ACUR" + ul.ToString() + System.Environment.NewLine + "Patient Name: " + PatientName);
                //    }
                //}

                string spath = string.Empty;
                string path = Server.MapPath("Documents/" + Session.SessionID);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                PrintOrders objPrint = new PrintOrders();
                HumanManager HumanMngr = new HumanManager();
                string PatientName = HumanMngr.GetPatientNameByHumanID(HumanID);
                FacilityManager objfacilityProxy = new FacilityManager();
                string AccountNumberForQuest = string.Empty;
                IList<FacilityLibrary> ilstFacility = objfacilityProxy.GetFacilityByFacilityname(ClientSession.FacilityName);
                if (ilstFacility.Count > 0)
                {
                    AccountNumberForQuest = ilstFacility[0].Quest_Account_Number;
                }
                foreach (ulong ul in ilstQuestOrderSubmitIDs)
                {
                    if (spath == string.Empty)
                    {
                        spath = objPrint.PrintLabelfromOrders(AccountNumberForQuest, ul, PatientName, path, "QUEST DIAGNOSTICS");
                    }
                    else
                    {
                        spath += "|" + objPrint.PrintLabelfromOrders(AccountNumberForQuest, ul, PatientName, path, "QUEST DIAGNOSTICS");
                    }
                }
                if (spath != string.Empty)
                {

                    string[] Split = new string[] { Server.MapPath("") };
                    string[] FileName = spath.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < FileName.Length; i++)
                    {
                        if (hdnSelectedItem.Value == string.Empty)
                        {
                            hdnSelectedItem.Value = FileName[i].ToString();
                        }
                        else
                        {
                            hdnSelectedItem.Value += "|" + FileName[i].ToString();
                        }
                    }
                }
                if (hdnSelectedItem.Value != string.Empty)
                {

                    ScriptManager.RegisterStartupScript(this, typeof(frmImageAndLabOrder), string.Empty, "OpenPDFImage();", true);
                }
                //}
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else if (ilstQuestOrderSubmitIDs.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Order List", "DisplayErrorMessage('230115'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230141'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
        }


        OrdersDTO GetDatas()
        {
            OrdersDTO objOrderDTO = new OrdersDTO();

            if (EncounterID != 0)
            {
                var serializer = new NetDataContractSerializer();
                object objDTO = (object)serializer.ReadObject(objOrdersManager.GetOrdersUsingEncounterID(EncounterID, "Diagnostic Order", false));
                objOrderDTO = (OrdersDTO)objDTO;

            }
            else
            {

                var serializer = new NetDataContractSerializer();
                //object objDTO = (object)serializer.ReadObject(objOrdersManager.GetOrdersUsingHumanID(HumanID, DateTime.Now, string.Empty, PhysicianID, false)); // Commaded By Manimaran For Bug Id:28644 
                object objDTO = (object)serializer.ReadObject(objOrdersManager.GetOrdersUsingHumanID(HumanID, DateTime.Now, "Diagnostic Order", PhysicianID, false));
                objOrderDTO = (OrdersDTO)objDTO;
            }

            return objOrderDTO;
        }


        protected override void OnUnload(EventArgs e)
        {
            //divLoading.Style.Add("display", "none");
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void Label1_Load(object sender, EventArgs e)
        {
            (sender as Label).Text = DateTime.Now.ToString();
        }
        protected void Button1_Load(object sender, EventArgs e)
        {
            (sender as Button).Attributes.Add("onclick", "return doStuff('" + (sender as Button).ClientID + "')");
            if (ViewState["CollectionDate"] != null)
                dtpCollectionDate.Value = ViewState["CollectionDate"].ToString();
        }
        bool TriggerClearAll = true;
        protected void btnImportresult_Click(object sender, EventArgs e)
        {
            //if (ClientSession.UserRole == "Medical Assistant")
            //    {
            if (ClientSession.UserRole == "Medical Assistant" && cboLab.Items[cboLab.SelectedIndex].Text == "CMG Anc.-In House")
            {
                if (lblCollectionDate.InnerText.Contains('*') && dtpCollectionDate.Value == "")
                {
                    ArrayList errList = new ArrayList();
                    errList.Add(lblCollectionDate.InnerText.Replace('*', ' ').Trim());
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblCollectionDate.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + "Collection Date" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    dtpCollectionDate.Focus();
                    return;
                }
            }
            if (chkMoveToMA.Checked == true && MoveToMA.Visible == true)//chkMoveToMA.Visible == true)
            {
                hdnMoveToMA.Value = "True";
            }
            else
            {
                hdnMoveToMA.Value = "false";
            }

            //hdnOrderSubmitID.Value
            string CurrentProcess = string.Empty;
            ulong Submit_Id = 0;

            OrdersDTO objOrderDTO = new OrdersDTO();




            if (btnClearAll.Value != "Cancel")
            {
                TriggerClearAll = false;
                btnOrderSubmit_Click(sender, e);
                if (hdnImportResult.Value == "true")
                    return;
                if (IsCollectionDate == true)
                    return;
                objOrderDTO = GetDatas();

                if (!IsImport)
                {
                    if (objOrderDTO.ilstOrderLabDetailsDTO.Count > 0)
                    {
                        IList<string> Current_Process = objOrderDTO.ilstOrderLabDetailsDTO.Where(E => E.ObjOrder.Order_Submit_ID == Convert.ToUInt32(objOrderDTO.ilstOrderLabDetailsDTO.Max(W => W.OrdersSubmit.Id))).Select(D => D.ObjOrder.Internal_Property_Current_Process).ToList();
                        CurrentProcess = Current_Process[0];
                        Submit_Id = Convert.ToUInt32(objOrderDTO.ilstOrderLabDetailsDTO.Max(W => W.OrdersSubmit.Id));
                    }
                }
            }

            else
            {
                ulong OrderSubmitID_for_importResult = Convert.ToUInt64(hdnOrderSubmitID.Value);
                TriggerClearAll = false;
                btnOrderSubmit_Click(sender, e);
                if (hdnImportResult.Value == "true")
                    return;
                objOrderDTO = GetDatas();

                IList<string> Current_Process = new List<string>();
                Current_Process = objOrderDTO.ilstOrderLabDetailsDTO.Where(E => E.ObjOrder.Order_Submit_ID == OrderSubmitID_for_importResult).Select(D => D.ObjOrder.Internal_Property_Current_Process).ToList();
                if (Current_Process.Count == 0)
                {
                    Current_Process = objOrderDTO.ilstOrdersSubmitForPartialOrders.Where(w => w.Id == OrderSubmitID_for_importResult).Select(a => a.Culture_Location).ToList();

                    if (Current_Process.Count == 0)
                    {
                        Current_Process = objOrderDTO.ilstOrderLabDetailsDTO.Where(E => E.ObjOrder.Order_Submit_ID == Convert.ToUInt32(objOrderDTO.ilstOrderLabDetailsDTO.Max(W => W.OrdersSubmit.Id))).Select(D => D.ObjOrder.Internal_Property_Current_Process).ToList();
                        OrderSubmitID_for_importResult = Convert.ToUInt32(objOrderDTO.ilstOrderLabDetailsDTO.Max(W => W.OrdersSubmit.Id));
                    }
                }
                CurrentProcess = Current_Process[0];



                Submit_Id = OrderSubmitID_for_importResult;
            }


            if (!IsImport)
            {
                btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
                if (CurrentProcess != string.Empty)
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "var win=radalert('Submitted Successfully.',null,null,'Diagnostic Order'); win.setSize(250, 100); win.center(); win.set_iconUrl('Resources/16_16.ico'); $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none'); $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none');  window.setTimeout(function() { win.close();}, 2000);", true);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openform", "openDiagnosisFormViews('" + CurrentProcess + "','" + Submit_Id + "','" + hdnMoveToMA.Value + "');", true);


                    //RadWindowImportResult.Visible = true;
                    //RadWindowImportResult.VisibleOnPageLoad = true;
                    ////RadWindowImportResult.NavigateUrl = "frmExamPhotos.aspx?type=Result Upload&IsCmg=IsCmg&CurrentProcess=" + CurrentProcess + "&OrderSubmit_ID=" + Submit_Id;
                    //RadWindowImportResult.NavigateUrl = "frmCCPhrase.aspx";
                    //RadWindowImportResult.VisibleOnPageLoad = true;
                    //RadWindowImportResult.Visible = true;
                    //RadWindowImportResult.Height = 400;
                    //RadWindowImportResult.Width = 800;


                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Keys", "if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null)window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false", true);

            }
            IsImport = false;
            // hdnMoveToMA.Value = "";
        }

        protected void btninvisible_Click(object sender, EventArgs e)
        {
            if (Session["IsSaved"].ToString() == "true")
            {
                btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
                Session["IsSaved"] = string.Empty;
                ClearAll(false);
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "var win=radalert('Submitted Successfully.',null,null,'Diagnostic Order'); win.setSize(250, 100); win.center(); win.set_iconUrl('Resources/16_16.ico'); $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none'); $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none'); window.setTimeout(function() { win.close();}, 2000);", true);
                return;
            }
            //ilstRequiredForms = ClientSession.RequiredForms_ReturnList;
            //System.Diagnostics.Stopwatch stopwatch = MyLog.StartWatch();
            timeOnAddClick = UtilityManager.ConvertToUniversal(DateTime.Now);

            if (btnOrderSubmit.Attributes["Tag"] != null && btnOrderSubmit.Attributes["Tag"].ToString() == "UPDATE")//btnOrderSubmit.Value.ToString() == "UPDATE")
            {
                UpdateOrder();
                //if (TriggerClearAll)
                //{
                //    ClearAll(true);
                //}
                //else
                //{
                //    ClearAll(false);
                //    btnOrderSubmit.Enabled = false;
                //    TriggerClearAll = true;
                //}
                btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
                if (sCmgorder != "Y")
                {
                    ClearAll(false);
                    btnClearAll.Value = "Clear All";
                    btnOrderSubmit.Attributes.Add("Tag", "SAVE"); //btnOrderSubmit.Value = "SAVE";
                    hdnsaveEnable.Value = "";
                }
                if (sCmgorder == "Y")
                {
                    if (Session["OrderSubmitId"] != null)
                    {
                        ulong uid = Convert.ToUInt32(Session["OrderSubmitId"]);
                        DiagnosticDTO objDiagnosticDTO = new DiagnosticDTO();
                        if (LookUpPerRequest.Keys.Contains("procedureType") == true)
                        {
                            objDiagnosticDTO = objOrdersManager.FillDiagnosticDTO(uid, EncounterID, HumanID, PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), ClientSession.FacilityName, ClientSession.LegalOrg);
                        }
                        LoadScreenFromOrderList(objDiagnosticDTO);
                        Session["objDiagnosticDTO"] = objDiagnosticDTO;
                        // FillAssesmentAndProblemListICD(objDiagnosticDTO.AssessmentList, objDiagnosticDTO.MedAdvProblemList);
                        //IList<string> ProceduresViewList = new List<string>();
                        //ProceduresViewList = objDiagnosticDTO.OrdersLists.Select(a => a.Lab_Procedure + "-" + (a.Lab_Procedure_Description.Contains("x_") ? (a.Lab_Procedure_Description + "~" + a.Quantity + "|" + a.Id) : a.Lab_Procedure_Description)).ToList<string>();
                        //IList<ListItem> tempList = SetOrderIDForEditQuantity(ProceduresViewList);

                    }

                }

            }
            else
            {
                IList<string> SelectedCPT = new List<string>();
                for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
                {
                    if (chklstFrequentlyUsedProcedures.Items[i].Selected)
                        SelectedCPT.Add(chklstFrequentlyUsedProcedures.Items[i].Text);
                }
                if (chkMoveToMA.Checked && SelectedCPT.Count == 0)
                {
                    OrdersSubmit MAOrdersSubmit = new OrdersSubmit();
                    MAOrdersSubmit.Human_ID = HumanID;
                    MAOrdersSubmit.Encounter_ID = EncounterID;
                    MAOrdersSubmit.Physician_ID = PhysicianID;
                    MAOrdersSubmit.Facility_Name = ClientSession.FacilityName;
                    MAOrdersSubmit.Move_To_MA = "Y";
                    MAOrdersSubmit.Order_Type = OrderType.ToUpper();
                    MAOrdersSubmit.Created_By = ClientSession.UserName;
                    // MAOrdersSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                    //string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                    //MAOrdersSubmit.Created_Date_And_Time = Convert.ToDateTime(strtime);
                    MAOrdersSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    if (chkAuthRequired.Checked)
                    {
                        MAOrdersSubmit.Authorization_Required = "Y";
                    }
                    else
                    {
                        MAOrdersSubmit.Authorization_Required = "N";
                    }
                    if (chkMoveToMA.Checked == true && MoveToMA.Visible == true)//chkMoveToMA.Visible == true)
                    {
                        MAOrdersSubmit.Move_To_MA = "Y";
                    }
                    else
                    {
                        MAOrdersSubmit.Move_To_MA = "N";
                    }
                    if (chkYesABN.Checked)
                    {
                        MAOrdersSubmit.Is_ABN_Signed = "Y";
                    }
                    else
                    {
                        MAOrdersSubmit.Is_ABN_Signed = "N";
                    }

                    if (chkpaperorder.Checked)
                    {
                        MAOrdersSubmit.Is_Paper_Order = "Y";
                    }
                    else
                    {
                        MAOrdersSubmit.Is_Paper_Order = "N";
                    }
                    if (cboLab.Items[cboLab.SelectedIndex].Selected != null)
                        MAOrdersSubmit.Lab_ID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                    MAOrdersSubmit.Bill_Type = cboBillType.Items[cboBillType.SelectedIndex].Text;
                    MAOrdersSubmit.Height = string.Empty;
                    MAOrdersSubmit.Weight = string.Empty;
                    MAOrdersSubmit.Lab_Location_Name = txtLocation.Value;
                    MAOrdersSubmit.Lab_Name = txtCenterName.Value;
                    if (LookUpPerRequest.ContainsKey("labLocID"))
                        MAOrdersSubmit.Lab_Location_ID = Convert.ToUInt32(LookUpPerRequest["labLocID"]);
                    else
                        MAOrdersSubmit.Lab_Location_ID = Convert.ToUInt32(0);
                    MAOrdersSubmit.Order_Notes = txtOrderNotes.txtDLC.Text;
                    //txtOrderNotes.txtDLC.Text = "";
                    MAOrdersSubmit.Order_Type = OrderType;
                    if (chkSpecimenInHouse.Checked && chkSpecimenInHouse.Visible)
                    {
                        MAOrdersSubmit.Specimen_In_House = "Y";
                    }
                    else
                    {
                        MAOrdersSubmit.Specimen_In_House = "N";
                    }
                    MAOrdersSubmit.Order_Code_Type = MAOrdersSubmit.Lab_Name;
                    if (chkUrgent.Checked == true)
                        MAOrdersSubmit.Is_Urgent = "Y";
                    else
                        MAOrdersSubmit.Is_Urgent = "N";
                    DateTime tempDate = DateTime.MinValue;
                    if (chkTestDateInDate.Checked && DateTime.TryParse(cstdtpTestDate.Value, out tempDate))
                    {
                        MAOrdersSubmit.Test_Date = Convert.ToDateTime(cstdtpTestDate.Value).ToString("dd-MMM-yyyy"); //MAOrdersSubmit.Test_Date = (cstdtpTestDate.SelectedDate ?? new DateTime()).ToString("yyyy-MM-dd");
                        if (chkStat.Checked)
                        {
                            MAOrdersSubmit.Stat = "Y";
                        }
                        else
                        {
                            MAOrdersSubmit.Stat = "N";
                        }
                    }
                    else if (chkTestDateInWords.Checked)
                    {
                        if (txtMonths.Value != string.Empty)
                            MAOrdersSubmit.TestDate_In_Months = Convert.ToInt16(txtMonths.Value);
                        if (txtDays.Value != string.Empty)
                            MAOrdersSubmit.TestDate_In_Days = Convert.ToInt16(txtDays.Value);
                        if (txtWeeks.Value != string.Empty)
                            MAOrdersSubmit.TestDate_In_Weeks = Convert.ToInt16(txtWeeks.Value);
                    }
                    if (rbtnSubmitImmediately.Checked)
                        MAOrdersSubmit.Is_Submit_Immediately = "Y";
                    else
                        MAOrdersSubmit.Is_Submit_Immediately = "N";
                    MAOrdersSubmit.Specimen_Type = cbospecimen.Items[cbospecimen.SelectedIndex].Text;
                    MAOrdersSubmit.Specimen_Unit = cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text != string.Empty ? cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text : " ";
                    if (dtpCollectionDate.Value != null && Convert.ToDateTime(dtpCollectionDate.Value) != DateTime.Now)
                        //MAOrdersSubmit.Specimen_Collection_Date_And_Time = Convert.ToDateTime(dtpCollectionDate.SelectedDate.Value);
                        MAOrdersSubmit.Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpCollectionDate.Value));
                    else
                        MAOrdersSubmit.Specimen_Collection_Date_And_Time = DateTime.MinValue;
                    //if (dtpCollectionDate.SelectedDate != null)
                    //MAOrdersSubmit.Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(dtpCollectionDate.SelectedDate ?? new DateTime());
                    if (txtQuantity.Value != string.Empty)
                        MAOrdersSubmit.Quantity = Convert.ToInt32(txtQuantity.Value);
                    else
                        MAOrdersSubmit.Quantity = 0;
                    if (chkYesFasting.Checked)
                    {
                        MAOrdersSubmit.Fasting = "Y";
                    }
                    else
                    {
                        MAOrdersSubmit.Fasting = "N";
                    }
                    objOrdersManager = new OrdersManager();
                    objOrdersManager.SubmitEmptyOrdersToMA(MAOrdersSubmit, string.Empty);

                }
                else
                {
                    AddOrder();
                }
                btnImportresult.Disabled = true; //btnImportresult.Enabled = false;

                ClearAll(false);
            }
            btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
            Session["IsSaved"] = "true";
            // ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "var win=radalert('Submitted Successfully.',null,null,'Diagnostic Order'); win.setSize(250, 100); win.center(); win.set_iconUrl('Resources/16_16.ico'); $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none'); $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none'); window.setTimeout(function() { win.close();}, 2000);", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "HiddenValue1", "LabOrder_SavedSuccessfully();top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
            CancelTextChanged();
            if (sAppointment == "Y")
            {
                // ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "var win=radalert('Submitted Successfully.',null,null,'Diagnostic Order'); win.setSize(250, 100); win.center(); win.set_iconUrl('Resources/16_16.ico'); $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none'); $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none'); window.setTimeout(function() { win.close();}, 2000);", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "HiddenValue2", "LabOrder_SavedSuccessfully();top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                return;
            }

            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230135');", true);
            // ApplicationObject.erroHandler.DisplayErrorMessage("230135", OrderType.ToUpper(),this);
            //objfrmOrdersList.frmOrdersList_Load(new object(), new EventArgs());
            //MyLog.LogDebugMessage(logger, "frmImageAndLabOrders - frmPlanCptFormsRequired - btnOrderSubmit", stopwatch);
        }

        protected void btninvisibleDiagnosis_Click(object sender, EventArgs e)
        {
            if (Session["Selected_ICDs"] != null)
            {
                IList<string> ItemsToBeAdded = (IList<string>)Session["Selected_ICDs"];
                if (ItemsToBeAdded.Count > 0)
                {
                    string sFinalValue = null;
                    foreach (string str in ItemsToBeAdded)
                    {
                        if (str.Split('|').Count() == 2)
                            sFinalValue = str.Split('|')[str.Split('|').Count() - 2].ToString();
                        else
                            sFinalValue = str.Split('|')[str.Split('|').Count() - 1].ToString();

                        ListItem itm = chklstAssessment.Items.FindByText(sFinalValue);
                        if (itm != null)
                        {
                            itm.Selected = true;
                            continue;
                        }
                        else
                        {
                            if (!AssessmentSource.Any(a => a.Key == sFinalValue.Split('-')[0].ToString()))
                            {
                                itm = new ListItem();
                                itm.Text = sFinalValue;
                                itm.Selected = true;
                                chklstAssessment.Items.Add(itm);
                                AssessmentSource.Add(sFinalValue.Split('-')[0], "OTHERS-0");
                            }
                        }
                    }
                    Session["Selected_ICDs"] = null;
                    if (Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value) == 32 && chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected))
                        btnImportresult.Disabled = false; //btnImportresult.Enabled = true;
                    else
                        btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
                    btnOrderSubmit.Disabled = false; //btnOrderSubmit.Enabled = true;
                }
            }
            //divLoading.Style.Add("display", "none");
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void chkpaperorder_CheckedChanged(object sender, EventArgs e)
        {
            if (chkpaperorder.Checked == true)
            {
                btnOrderSubmit.Disabled = false; //btnOrderSubmit.Enabled = true;
                rbImageOrder.Enabled = true;
                rbLabOrder.Enabled = true;
            }
            else
            {
                rbLabOrder.Enabled = false;
                rbImageOrder.Enabled = false;
            }
        }
        protected void chklstFrequentlyUsedProcedures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLab.Items[cboLab.SelectedIndex].Text == "CMG Anc.-In House")
            {
                if (chklstFrequentlyUsedProcedures.SelectedIndex == -1)
                {
                    btnImportresult.Disabled = true; //btnImportresult.Enabled = false;
                }
                else
                {
                    btnImportresult.Disabled = false; //btnImportresult.Enabled = true;
                }
            }
            btnOrderSubmit.Disabled = false; //btnOrderSubmit.Enabled = true;


            if (txtQuantity.Value != string.Empty)
            {
                if (!lblUnits.InnerText.Contains("*"))
                    lblUnits.InnerText += "*";
                lblUnits.InnerHtml = lblUnits.InnerText;
                lblUnits.InnerHtml = lblUnits.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                //lblUnits.Attributes.Add("style", "color:red");
                lblUnits.Attributes["Class"] = "";
                lblUnits.Attributes["Class"] = "MandLabelstyle";
            }
            else
            {
                lblUnits.InnerText = "Units";//lblUnits.InnerText.Replace("*", string.Empty);
                lblUnits.Attributes.Add("style", "color:black");
                lblUnits.Attributes["Class"] = "";
                lblUnits.Attributes["Class"] = "spanstyle";
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();};disableAutoSave();", true);
        }
        protected void hdnbuttonload_Click(object sender, EventArgs e)
        {
            //Jira CAP-1628 - added if condition
            if (Session["OrderSubmitId"] != null && Session["OrderSubmitId"].ToString() != "")
            {
                ulong uid = Convert.ToUInt32(Session["OrderSubmitId"]);
                DiagnosticDTO objDiagnosticDTO = new DiagnosticDTO();
                if (LookUpPerRequest.Keys.Contains("procedureType") == true)
                    objDiagnosticDTO = objOrdersManager.FillDiagnosticDTO(uid, EncounterID, HumanID, PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), ClientSession.FacilityName, ClientSession.LegalOrg);
                LoadScreenFromOrderList(objDiagnosticDTO);
                Session["objDiagnosticDTO"] = objDiagnosticDTO;
            }
        }
        public void ClearAllCmg(bool IsNewOrder)
        {
            if (MoveToMA.Visible)
            {
                gbSpecimenDetails.Enabled = true;
            }
            //if (gbSelectICD.Enabled ||LookUpPerRequest["InHouseLabNameFromLookUp"].ToUpper() == "CMG ANC.-IN HOUSE")
            //{
            //    int[] ckcount = lstvFrequentlyUsedLabProcedures.CheckedIndices.Cast<Int32>().ToArray<int>();
            //    if (gbProcedures.Enabled != false)
            //    {
            //        for (int i = 0; i < ckcount.Length; i++)
            //        {
            //            lstvFrequentlyUsedLabProcedures.Items[ckcount[i]].Checked = false;
            //        }
            //    }
            //}
            chklstAssessment.ClearSelection();
            for (int i = 0; i < AssessmentSource.Count; i++)
            {
                chklstAssessment.Items[i].Selected = false;
            }
            if (IsNewOrder)
            {
                //lstvFrequentlyUsedLabProcedures.ForeColor = Color.Black;
                cboLab.SelectedIndex = 0;
                cboLab.Items[cboLab.SelectedIndex].Text = string.Empty;

                //cboLab.SelectedValue = "0";
                txtCenterName.Value = "";
                txtLocation.Value = "";
                cbospecimen.Items[cbospecimen.SelectedIndex].Text = string.Empty;
                cbospecimen.SelectedIndex = 0;
                if (lblUnits.InnerText.Contains('*'))
                {
                    lblUnits.InnerText = "Units";// lblUnits.InnerText.Replace('*', ' ');
                    lblUnits.Attributes.Add("style", "color:black");
                    lblUnits.Attributes["Class"] = "";
                    lblUnits.Attributes["Class"] = "spanstyle";
                }
                if (LookUpPerRequest.ContainsKey("DefaultBillType"))
                {
                    if (LookUpPerRequest["DefaultBillType"].ToString() != string.Empty)
                        AssignDefaultValueToLab(Convert.ToInt32(LookUpPerRequest["DefaultBillType"]));
                    else
                        cboBillType.SelectedIndex = 0;
                }
                else
                    cboBillType.SelectedIndex = 0;

                dbInsurancePlan.Enabled = false;
                chkYesABN.Checked = false;
                chkYesFasting.Checked = false;
                chkAuthRequired.Checked = false;
                chkSpecimenInHouse.Checked = false;
                SetCollectionDateMand(false);
                //hdnCollectionDateIsMand.Value = "false";
                chkSelectALLICD.Checked = false;
                chkMoveToMA.Checked = false;
                dtpCollectionDate.Value = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                SelectedTime(DateTime.Now);
                ViewState["CollectionDate"] = dtpCollectionDate.Value;
                //dtpCollectionDate.SelectedDate = DateTime.Now;
                txtQuantity.Value = "";
                cboSpecimenUnits.Items[cboSpecimenUnits.SelectedIndex].Text = string.Empty;
                cboSpecimenUnits.SelectedIndex = 0;
                txtOrderNotes.txtDLC.Text = "";
                chkStat.Checked = false;
                chkUrgent.Checked = false;
                if (lblCenterName.InnerText.Contains("*"))
                {
                    lblCenterName.InnerText = lblCenterName.InnerText.Replace('*', ' ');
                    lblCenterName.InnerText = lblCenterName.InnerText.Trim();
                    lblCenterName.Attributes.Add("style", "color:black");
                    SetReadOnlyStyle(txtCenterName);
                }
                if (lblLocation.InnerText.Contains("*"))
                {
                    lblLocation.InnerText = lblLocation.InnerText.Replace('*', ' ');
                    lblLocation.InnerText = lblLocation.InnerText.Trim();
                    lblLocation.Attributes.Add("style", "color:black");
                    SetReadOnlyStyle(txtLocation);
                }
                btnOrderSubmit.Attributes.Add("Tag", "SAVE");//btnOrderSubmit.Value = "SAVE";
                //Session["OrderSubmitId"] = string.Empty;
                //chklstFrequentlyUsedProcedures.Items.Clear();
                //lstvFrequentlyUsedLabProcedures.Items.Clear();
                gbProcedures.Enabled = true;
                cboLab.Disabled = false; //cboLab.Enabled = true;
                chkMoveToMA.Disabled = false; //chkMoveToMA.Enabled = true;
                chkSpecimenInHouse.Checked = false;
                SetCollectionDateMand(false);
                //hdnCollectionDateIsMand.Value = "false";
                if (hdnLocalTime.Value.Trim() != string.Empty)
                    cstdtpTestDate.Value = Convert.ToDateTime(hdnLocalTime.Value).ToString("dd-MMM-yyyy");
                dtpCollectionDate.Value = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                SelectedTime(DateTime.Now);
                ViewState["CollectionDate"] = dtpCollectionDate.Value;
                //dtpCollectionDate.SelectedDate = DateTime.Now;
                chkTestDateInDate.Checked = true;
                AssignDefaultValueToLab();
                if (Session["cboLab"] != null)
                {
                    cboLab.SelectedIndex = Convert.ToInt32(Session["cboLab"]);
                }
                lblBillType.InnerText = "Bill Type*";
                lblBillType.Attributes.Add("style", "color:red");
                chklstAssessment.Enabled = true;
                if (ClientSession.UserCurrentProcess.ToUpper() == "MA_PROCESS")
                {
                    gbCenterDetail.Enabled = true;
                    gbSelectICD.Enabled = true;
                    gbProcedures.Enabled = true;
                    gbOrderDetails.Enabled = true;
                    gbSpecimenDetails.Enabled = true;
                }
                rbtnSubmitImmediately.Checked = true;
                IsNormalMode = true;
                chkpaperorder.Checked = false;
                rbImageOrder.Checked = false;
                rbLabOrder.Checked = false;
                rbImageOrder.Enabled = false;
                rbLabOrder.Enabled = false;
            }
            //The following code has been modified for BUG-ID:26470 - Pujhitha
            if (cboLab.Items[cboLab.SelectedIndex].Value.Trim() != string.Empty && (LookUpPerRequest.Keys.Contains("procedureType") == true))
            {
                procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), Convert.ToUInt32(cboLab.Items[cboLab.SelectedIndex].Value), ClientSession.LegalOrg);
            }
            else
            {
                procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, LookUpPerRequest["procedureType"].ToUpper(), Convert.ToUInt32(cboLab.Items[1].Value), ClientSession.LegalOrg);
            }
            FillLabProcedure(procedureList, new List<string>());
            btnClearAll.Value = "Clear All";
            if (hdnForEditErrorMsg.Value == "Edit")
                hdnForEditErrorMsg.Value = "";
            IsDefaultLab = false;
            //----------------------------------------------------------------------
        }

        public IList<ListItem> SetOrderIDForEditQuantity(IList<string> ProceduresViewList)
        {
            IList<ListItem> tempList = new List<ListItem>();
            bool chk = false;
            for (int i = 0; i < ProceduresViewList.Count; i++)
            {
                if (ProceduresViewList[i] != string.Empty)
                {
                    ListItem foundItem = chklstFrequentlyUsedProcedures.Items.FindByText(ProceduresViewList[i].Split('~')[0]);
                    if (foundItem != null)
                    {
                        foundItem.Selected = true;
                        if (ProceduresViewList[i].Contains('~'))
                        {
                            if (ProceduresViewList[i].Split('~')[1].Split('|')[0] != "" && ProceduresViewList[i].Split('~')[1].Split('|')[0] != "0")
                            {
                                string Qty = "x___" + ProceduresViewList[i].Split('~')[1].Split('|')[0] + "___";
                                foundItem.Text = foundItem.Text.Replace("x______", Qty);
                                chk = true;
                            }
                            if (ProceduresViewList[i].Split('~')[1].Split('|')[0] != "")
                                foundItem.Attributes.Add("orderid", ProceduresViewList[i].Split('~')[1].Split('|')[1]);
                            ProceduresViewList[i] = foundItem.Text;
                        }
                    }

                    else
                    {

                        ListItem OtherItemIsFound = chklstFrequentlyUsedProcedures.Items.FindByText("OTHER");
                        if (OtherItemIsFound != null && OtherItemIsFound.Text != "OTHER")
                            OtherItemIsFound = null;
                        if (OtherItemIsFound == null)
                        {
                            ListItem itemOthers = new ListItem("OTHER");
                            itemOthers.Attributes.Add("style", "color:Blue;");// = Color.Blue;
                            //itemOthers.Font = new Font("Arial", 10, FontStyle.Bold);

                            chklstFrequentlyUsedProcedures.Items.Add(itemOthers);
                        }

                        ListItem item = new ListItem(ProceduresViewList[i]);
                        item.Selected = true;
                        tempList.Add(item);
                        //lstvFrequentlyUsedLabProcedures.Items.Add(item);
                    }
                }
            }
            if (chk)
                btnEditQuantity.Disabled = false; ;
            Session["ProceduresViewList"] = ProceduresViewList;
            if (tempList.Count > 0)
            {
                int insertIndex = FindInsertStartIndex();
                foreach (ListItem obj in tempList)
                {
                    chklstFrequentlyUsedProcedures.Items.Insert(insertIndex, obj);
                }
            }
            return tempList;
        }

        [WebMethod(EnableSession = true)]
        public static string LoadOrdersTab()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string Ordersvalue = string.Empty;
            IList<Encounter> enclst = new List<Encounter>();
            SecurityServiceUtility SecurityService = new SecurityServiceUtility();
            IList<string> Orders_tab_to_disable = SecurityService.GetListTabtoDisable("frmOrders");
            if (Orders_tab_to_disable != null && Orders_tab_to_disable.Count > 0)
            {
                var tabs_to_disable = Orders_tab_to_disable.Select(a => new
                {
                    tab = a
                });
                Ordersvalue = "Disable_Tab=" + JsonConvert.SerializeObject(tabs_to_disable);

            }
            else
            {
                Ordersvalue = "Disable_Tab=" + "";
            }
            return Ordersvalue;
        }
    }
}

